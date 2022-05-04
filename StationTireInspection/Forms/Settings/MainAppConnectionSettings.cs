using StationTireInspection.Classes;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.UDT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VisualInspection.Utils;
using VisualInspection.Utils.Net;

namespace StationTireInspection.Forms.Settings
{
    public partial class MainAppConnectionSettings : Form
    {
        private SettingsJDO Settings;
        private TCPIPClient ServerTCPClient;

        private string ErrorMessageBoxTitle = "";
        private string[] Errors = new string[2];

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public MainAppConnectionSettings(SettingsJDO settings, TCPIPClient serverTCPClient)
        {
            InitializeComponent();

            Settings = settings;
            ServerTCPClient = serverTCPClient;

            SetInitValue();

            Translator.LanguageChanged += Translate;
            ServerTCPClient.StatusChanged += Status_Changed;
        }

        private void Status_Changed(object sender, ClientStatus e)
        {
            if (e.Equals(ClientStatus.Connected))
            {
                btnConnect.InvokeIfRequired((btn) => btn.Enabled = false);
                btnDisconnect.InvokeIfRequired((btn) => btn.Enabled = true);
            }
            else if (e.Equals(ClientStatus.Disconnected))
            {
                btnConnect.InvokeIfRequired((btn) => btn.Enabled = true);
                btnDisconnect.InvokeIfRequired((btn) => btn.Enabled = false);
            }
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTitle.Text = "Nastavení Hlavní Aplikace";
                lblIPAddress.Text = "IP Adresa:";
                lblPort.Text = "Port:";
                btnConnect.Text = "Připojit";
                btnDisconnect.Text = "Odpojit";
                btnApply.Text = "Použít";

                ErrorMessageBoxTitle = "Chyba uživatelského vstupu";

                Errors[0] = "IP Adresa není ve správném tvaru. Např. 192.168.1.1";
                Errors[1] = "Port není ve správném tvaru. Např. 8080";

                MessageMessageBoxTitle = "Zpráva";

                Message = "Data byla úspěšně uložena.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblTitle.Text = "Main App Settings";
                lblIPAddress.Text = "IP Address:";
                lblPort.Text = "Port:";
                btnConnect.Text = "Connect";
                btnDisconnect.Text = "Disconnect";
                btnApply.Text = "Apply";

                ErrorMessageBoxTitle = "User Input Error";

                Errors[0] = "IP Address is not in valide format. Eg. 192.168.1.1";
                Errors[1] = "Port is not in valide format. Eg. 8080";

                MessageMessageBoxTitle = "Message";

                Message = "Data was be correctly seved";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (ipAddressBox.IPAddressValid)
            {
                Settings.MainAppConnectionSettings.IPAddress = ipAddressBox.IPAddress;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[0]);
                return;
            }

            if (TextBoxHelper.TbInputIsNumber(tbPort))
            {
                Settings.MainAppConnectionSettings.Port = int.Parse(tbPort.Text);
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[1]);
                return;
            }

            CustomMessageBox.ShowPopup(MessageMessageBoxTitle, Message);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ServerTCPClient.IPAddress = Settings.MainAppConnectionSettings.IPAddress;
            ServerTCPClient.Port = Settings.MainAppConnectionSettings.Port;
            ServerTCPClient.Connect_Async();

            //ServerTCPClient.Connect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            ServerTCPClient.Disconnect(true);
        }

        private void SetInitValue()
        {
            ipAddressBox.IPAddress = Settings.MainAppConnectionSettings.IPAddress;
            tbPort.Text = Settings.MainAppConnectionSettings.Port.ToString();
        }

        private void MainAppConnectionSettings_VisibleChanged(object sender, EventArgs e)
        {
            SetInitValue();
        }
    }
}
