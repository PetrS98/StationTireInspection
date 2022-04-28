using System;
using System.Collections.Generic;
using System.Text;
using SimpleTcp;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.UDT;
using VisualInspection.Utils.Net;

namespace StationTireInspection.Classes
{
    public class TCPIPClient : IHasClientStatus
    {
        SimpleTcpClient Client;

        readonly private bool RECONNECT_ENABLE = true;
        private bool Reconnecting = false;

        System.Timers.Timer ReconnectingTimer = new System.Timers.Timer();
        System.Timers.Timer CheckConnection = new System.Timers.Timer();

        public event EventHandler<ClientStatus> StatusChanged;
        public event EventHandler<byte[]> DataChanged;

        private ClientStatus status = ClientStatus.Disconnected;
        public ClientStatus Status
        {
            get { return status; }
            private set
            {
                bool changed = status != value;
                status = value;
                if (changed) StatusChanged?.Invoke(this, value);
            }
        }

        private byte[] data;

        public byte[] Data
        {
            get { return data; }
            set 
            { 
                bool changed = data != value;
                data = value;
                if (changed) DataChanged?.Invoke(this, value);
            }
        }

        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8080;

        public TCPIPClient()
        {

            ReconnectingTimer.Interval = 5000;
            ReconnectingTimer.Elapsed += TryReconnect;

            CheckConnection.Interval = 300;
            CheckConnection.Elapsed += CheckComm;
        }

        private void CheckComm(object sender, EventArgs e)
        {
            if (Client.IsConnected) Status = ClientStatus.Connected;
            else Status = ClientStatus.Disconnected;
        }

        private void TryReconnect(object sender, EventArgs e)
        {
            if (Client.IsConnected == true) return;

            Reconnecting = true;
            Disconnect(false);
            Connect();
        }

        public bool Connect()
        {
            if (RECONNECT_ENABLE == true)
            {
                ReconnectingTimer.Start();
            }

            try
            {
                Client = new SimpleTcpClient(IPAddress, Port);
                Client.Events.DataReceived += Client_DataReceived;
                Client.Connect();
                Status = ClientStatus.Connected;
                return true;
            }
            catch(Exception ex)
            {
                if(Reconnecting == false) CustomMessageBox.ShowPopup("TCPIP Client Error", ex.Message);
                return false;
            } 
        }

        public void Disconnect(bool DisableReconnect)
        {
            Client.Disconnect();
            Status = ClientStatus.Disconnected;

            if (DisableReconnect) ReconnectingTimer.Stop();
        }

        private void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Data = e.Data;
        }

        public void SendData(string Data)
        {
            if (Status == ClientStatus.Connected)
            {
                try
                {
                    Client.Send(Data);
                }
                catch (Exception ex)
                {
                    CustomMessageBox.ShowPopup("TCPIP Client Error", ex.Message);
                }
            }
        }
    }
}
