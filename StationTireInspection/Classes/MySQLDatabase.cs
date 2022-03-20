using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Timers;
using MySqlConnector;
using StationTireInspection.Forms.MessageBoxes;
using VisualInspection.Utils.Net;

namespace StationTireInspection.Classes
{
    public class MySQLDatabase : IHasClientStatus
    {
        private MySqlConnection mySqlConnection;

        private Timer _timerStatus = new Timer();

        private ClientStatus status = ClientStatus.Disconnected;
        public ClientStatus Status
        {
            get { return status; }
            private set
            {
                status = value;
                StatusChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<ClientStatus> StatusChanged;

        public MySQLDatabase()
        {
            _timerStatus.Interval = 100;
            _timerStatus.Elapsed += CheckStatus;
        }

        public bool ConnectToDB(string IpAddress, string UserName, string Password)
        {
            try
            {
                mySqlConnection = new MySqlConnection("server=" + IpAddress + ";" + "uid=" + UserName + ";" + "pwd=" + Password + ";" + "database=db_visual_inspection");
                mySqlConnection.Open();
                _timerStatus.Start();
                return true;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return false;
            }
        }

        public void DisconnectFromDB()
        {
            _timerStatus.Stop();
            Status = ClientStatus.Disconnected;
            mySqlConnection?.Close();
            mySqlConnection = null;
        }

        public List<Permission> ReadPermissionList()
        {
            List<Permission> UserPermission = new List<Permission>();

            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"SELECT permission_name, bit_position FROM permissions;";

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    Permission permission = new Permission(mySqlDataReader.GetString(0), mySqlDataReader.GetByte(1));

                    UserPermission.Add(permission);
                }
                return UserPermission;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return null;
            }
        }

        public List<UserNameAndID> ReadNamesAndIDs()
        {
            List<UserNameAndID> AllUsersAndIds = new List<UserNameAndID>();

            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"SELECT Personal_id, first_name, last_name FROM users;";

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    AllUsersAndIds.Add(new UserNameAndID(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetString(2)));
                }
                return AllUsersAndIds;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return null;
            }
        }

        public UserInformations ReadUserInformation(int PersonalID)
        {
            List<Permission> permissions = new List<Permission>(ReadPermissionList());

            UserInformations UserInformation = new UserInformations(null, null, null);

            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"SELECT * FROM users WHERE personal_id = @PersonalID;";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", PersonalID);

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    UserNameAndID NameAndID = new UserNameAndID(mySqlDataReader.GetInt32(1), mySqlDataReader.GetString(2), mySqlDataReader.GetString(3));
                    string Password = mySqlDataReader.GetString(4);
                    List<Permission> Permission = PermissionConverter.NumberToPermissions(mySqlDataReader.GetInt32(5), permissions);
                    UserInformation = new UserInformations(NameAndID, Password, Permission);
                }
                return UserInformation;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return null;
            }
        }

        public bool AddUserToDB(UserInformations userInformations)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"INSERT INTO users (personal_id, first_name, last_name, password, permissions) 
                                                   VALUES (@PersonalID, @FirstName, @LastName, @Password, @Permission)";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", userInformations.NameAndID.ID);
            mySqlCommand.Parameters.AddWithValue("@FirstName", userInformations.NameAndID.FirstName);
            mySqlCommand.Parameters.AddWithValue("@LastName", userInformations.NameAndID.LastName);
            mySqlCommand.Parameters.AddWithValue("@Password", userInformations.Password);
            mySqlCommand.Parameters.AddWithValue("@Permission", PermissionConverter.PermissionsToNumber(userInformations.Permission));

            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteUserFromDB(int PersonalID)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"DELETE FROM users WHERE personal_id = @PersonalID;";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", PersonalID);

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                return true;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return false;
            }
        }

        public bool UpdateUserInformations(UserInformations userInformations, UserInformations oldUserInformations)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"UPDATE users SET personal_id = @PersonalID, first_name = @FirstName, last_name = @LastName, password = @Password, permissions = @Permission 
                                         WHERE personal_id = @OldPersonalID AND first_name = @OldFirstName AND last_name = @OldLastName";

            mySqlCommand.Parameters.AddWithValue("@OldPersonalID", oldUserInformations.NameAndID.ID);
            mySqlCommand.Parameters.AddWithValue("@OldFirstName", oldUserInformations.NameAndID.FirstName);
            mySqlCommand.Parameters.AddWithValue("@OldLastName", oldUserInformations.NameAndID.LastName);

            mySqlCommand.Parameters.AddWithValue("@PersonalID", userInformations.NameAndID.ID);
            mySqlCommand.Parameters.AddWithValue("@FirstName", userInformations.NameAndID.FirstName);
            mySqlCommand.Parameters.AddWithValue("@LastName", userInformations.NameAndID.LastName);
            mySqlCommand.Parameters.AddWithValue("@Password", userInformations.Password);
            mySqlCommand.Parameters.AddWithValue("@Permission", PermissionConverter.PermissionsToNumber(userInformations.Permission));

            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                return false;
            }

            return true;
        }

        private void CheckStatus(object sender, ElapsedEventArgs e)
        {
            if (mySqlConnection is null) return;
            var state = mySqlConnection.State;

            if (state == ConnectionState.Open)
            {
                Status = ClientStatus.Connected;
            }
            else if (state == ConnectionState.Connecting)
            {
                Status = ClientStatus.Connecting;
            }
            else
            {
                Status = ClientStatus.Disconnected;
            }
        }
    }
}
