using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using SimpleTcp;

namespace StationTireInspection.Classes
{
    class BarcodeReaderClient
    {
        SimpleTcpClient client;

        readonly private bool RECONNECT_ENABLE = true;

        System.Timers.Timer ReconnectingTimer = new System.Timers.Timer();

        public string IpAddress { get; set; } = "192.168.1.5";
        public int Port { get; set; } = 8080;
        public string Barcode { get; private set; }

        public BarcodeReaderClient()
        {
            ReconnectingTimer.Interval = 10000;
            ReconnectingTimer.Elapsed += TryReconnect;
        }

        private void TryReconnect(object sender, ElapsedEventArgs e)
        {
            if (client.IsConnected == true) return;

            Disconnect();
            Connect();
        }

        public bool Connect()
        {
            try
            {
                client = new SimpleTcpClient(IpAddress, Port);
                client.Events.DataReceived += Client_DataReceived;
                client.Keepalive.EnableTcpKeepAlives = false;
            }
            catch(Exception ex)
            {
                //TODO dodělat eeros mesassges
                return false;
            }
            
            if(RECONNECT_ENABLE == true)
            {
                ReconnectingTimer.Start();
            }
            return true;
        }

        private void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string tmp = Encoding.UTF8.GetString(e.Data);
            char[] data = new char[tmp.Length];
            int index = 0;

            for (int i = 0; i < tmp.Length; i++)
            {
                if (CheckTextIfNumber(tmp[i]))
                {
                    data[index] = tmp[i];
                    index++;
                }
            }

            tmp = "";

            for (int i = 0; i < index; i++)
            {
                tmp += data[i];
            }

            Barcode = tmp;
        }

        public void Disconnect()
        {
            client.Disconnect();

            if (RECONNECT_ENABLE == true) ReconnectingTimer.Stop();
        }

        private bool CheckTextIfNumber(char character)
        {
            if (character != '0' || character != '1' || character != '2' || character != '3' || character != '4' || character != '5' || character != '6' || character != '7' || character != '8' || character != '9')
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
