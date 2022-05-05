using StationTireInspection.Classes;
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
    public partial class PLCStationInterfaceSettings : Form
    {
        private SettingsJDO Settings;
        TCPIPClient InterfacTCPIPClient;

        private string ErrorMessageBoxTitle = "";
        private string[] Errors = new string[2];

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public PLCStationInterfaceSettings(SettingsJDO settings, TCPIPClient interfacTCPIPClient)
        {
            InitializeComponent();

            Settings = settings;
            InterfacTCPIPClient = interfacTCPIPClient;

            Translator.LanguageChanged += Translate;
            InterfacTCPIPClient.StatusChanged += Status_Changed;
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
                lblTitle.Text = "Nastavení Čtečky Barkódů";
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
                lblTitle.Text = "PLC <--> Station Interface Settings";
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
    }
}
