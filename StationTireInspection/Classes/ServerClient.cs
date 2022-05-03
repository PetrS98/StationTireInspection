using StationTireInspection.JDO.DataToServer;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using VisualInspection.Utils.Net;
using StationTireInspection.Forms;

namespace StationTireInspection.Classes
{
    public class ServerClient
    {
        TCPIPClient ReaderTCPClient;
        TCPIPClient ServerTCPClient;
        DataToServerJDO DataToServer;
        Login Login;

        public ServerClient(TCPIPClient readerTCPClient, TCPIPClient serverTCPClient, DataToServerJDO dataToServer, Login login)
        {
            ReaderTCPClient = readerTCPClient;
            ServerTCPClient = serverTCPClient;
            DataToServer = dataToServer;
            Login = login;

            ReaderTCPClient.DataChanged += BarcodeRead;
            Login.LoginResultChanged += LoginChanged;
            Login.NonOperationChanged += NonOpChanged;

        }

        private void NonOpChanged(object sender, int e)
        {
            DataToServer.NonOperation = e;

            SendDataToServer(DataToServer);
        }

        private void LoginChanged(object sender, LoginResult e)
        {
            DataToServer.UserInformation.Status = e;

            SendDataToServer(DataToServer);
        }

        private void BarcodeRead(object sender, byte[] e)
        {
            DataToServer.Barcode = SeparateBarcode(e);
            SendDataToServer(DataToServer);
        }

        private string SeparateBarcode(byte[] Data)
        {
            string barcode = "";
            char[] data = Encoding.UTF8.GetString(Data).ToCharArray();

            for (int i = 0; i < data.Length; i++)
            {
                if (CheckTextIfNumber(data[i]))
                {
                    barcode += data[i];
                }
            }

            return barcode;
        }

        private bool CheckTextIfNumber(char character)
        {
            if (character == '0' || character == '1' || character == '2' || character == '3' || character == '4' || character == '5' || character == '6' || character == '7' || character == '8' || character == '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendDataToServer(DataToServerJDO Data)
        {
            string DataToServer = JsonConvert.SerializeObject(Data, Formatting.Indented);

            if (ServerTCPClient.Status != ClientStatus.Connected) return;
            ServerTCPClient.SendData(DataToServer);
        }
    }
}
