using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MySqlConnector;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.UDT;
using VisualInspection.Utils.Net;

namespace StationTireInspection.Classes
{
    public class MySQLDatabase : IHasClientStatus
    {
        private readonly bool RECONNECT_ENABLE = true;

        private MySqlConnection mySqlConnection;

        private Timer _timerStatus = new Timer();
        System.Timers.Timer ReconnectingTimer = new System.Timers.Timer();

        private bool Reconnecting = false;

        public string DatabaseName { get; set; }
        public string IPAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

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
            _timerStatus.Start();

            ReconnectingTimer.Interval = 5000;
            ReconnectingTimer.Elapsed += TryReconnect;
            ReconnectingTimer.Start();
        }

         private void TryReconnect(object sender, EventArgs e)
        {
            if (Status == ClientStatus.Connected || Status == ClientStatus.Connecting) return;

            Status = ClientStatus.Connecting;
            Reconnecting = true;
            DisconnectFromDB(false);
            ConnectToDB_Async();

            //Connect();
        }

        async public void ConnectToDB_Async()
        {
            await Task.Run(() => ConnectToDB());
        }

        public bool ConnectToDB()
        {
            if (RECONNECT_ENABLE == true)
            {
                ReconnectingTimer.Start();
            }

            try
            {
                mySqlConnection = new MySqlConnection("server=" + IPAddress + ";" + "uid=" + UserName + ";" + "pwd=" + Password + ";" + "database=" + DatabaseName);
                mySqlConnection.Open();
                return true;
            }
            catch(Exception ex)
            {
                //CustomMessageBox.ShowPopup("MySQL Error", ex.Message);
                if (Reconnecting == false) ExceptionChanged(this, ex);

                return false;
            }
        }

        public void DisconnectFromDB(bool DisableReconnect)
        {
            //_timerStatus.Stop();
            Status = ClientStatus.Disconnected;
            mySqlConnection?.Close();
            mySqlConnection = null;

            if(DisableReconnect) ReconnectingTimer.Stop();
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

        public bool WriteNonOperationToDB(string TableName, NonOperationInformations NonOperationInformations)
        {
            using MySqlCommand mySqlCommand = new MySqlCommand();

            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.CommandText = @"INSERT INTO " + TableName + " (datetime_start_non_op, datetime_stop_non_op, non_operation_time, id_non_operation, id_user_select_non_op, id_user_clear_non_op, id_station) " +
                                                              "VALUES (@StartNonOPDateTime, @StopNonOPDateTime, @NonOperationTime, @IDNonOperation, @IDUserSelectNonOp, @IDUserClearNonOp, @IDStation)";

            mySqlCommand.Parameters.AddWithValue("@StartNonOPDateTime", NonOperationInformations.StartNonOPDateTime);
            mySqlCommand.Parameters.AddWithValue("@StopNonOPDateTime", NonOperationInformations.StopNonOPDateTime);
            mySqlCommand.Parameters.AddWithValue("@NonOperationTime", NonOperationInformations.NonOperationTime);
            mySqlCommand.Parameters.AddWithValue("@IDNonOperation", NonOperationInformations.IDNonOperation);
            mySqlCommand.Parameters.AddWithValue("@IDUserSelectNonOp", NonOperationInformations.IDUserSelectNonOp);
            mySqlCommand.Parameters.AddWithValue("@IDUserClearNonOp", NonOperationInformations.IDUserClearNonOp);
            mySqlCommand.Parameters.AddWithValue("@IDStation", NonOperationInformations.IDStation);

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
