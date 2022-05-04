using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
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
        public event EventHandler<Exception> ExceptionChanged;

        public MySQLDatabase()
        {
            _timerStatus.Interval = 100;
            _timerStatus.Elapsed += CheckStatus;
        }

        async public void ConnectToDB_Async(string IpAddress, string UserName, string Password)
        {
            await Task.Run(() => ConnectToDB(IpAddress, UserName, Password));
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
                //CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                ExceptionChanged(this, ex);
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

        public UserInformations ReadUserInformation(string TableName, int PersonalID)
        {
            UserInformations UserInformation = new UserInformations(null, null, 1);

            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"SELECT * FROM " + TableName + " WHERE personal_id = @PersonalID;";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", PersonalID);

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    UserNameAndID NameAndID = new UserNameAndID(mySqlDataReader.GetInt32(1), mySqlDataReader.GetString(2), mySqlDataReader.GetString(3));
                    string Password = mySqlDataReader.GetString(4);
                    byte AskPasswordChanged = mySqlDataReader.GetByte(5);
                    UserInformation = new UserInformations(NameAndID, Password, AskPasswordChanged);
                }
                return UserInformation;
            }
            catch(Exception ex)
            {
                //CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                ExceptionChanged(this, ex);
                return null;
            }
        }

        public string ReadUserPassword(string TableName, int PersonalID)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"SELECT password FROM " + TableName + " WHERE personal_id = @PersonalID;";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", PersonalID);

            try
            {
                using MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                mySqlDataReader.Read();

                return mySqlDataReader.GetString(0);
            }
            catch (Exception ex)
            {
                //CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                ExceptionChanged(this, ex);
                return null;
            }
        }

        public bool UpdateUserInformations(string TableName, int PersonalID, string Password, byte AskPasswordChanged)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"UPDATE " + TableName + " SET password = @Password, ask_password_changed = @AskPasswordChanged WHERE personal_id = @PersonalID";

            mySqlCommand.Parameters.AddWithValue("@PersonalID", PersonalID);
            mySqlCommand.Parameters.AddWithValue("@Password", Password);
            mySqlCommand.Parameters.AddWithValue("@AskPasswordChanged", AskPasswordChanged);

            try
            {
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                ExceptionChanged(this, ex);
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
