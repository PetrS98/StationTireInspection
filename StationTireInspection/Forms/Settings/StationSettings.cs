using StationTireInspection.Classes;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.Forms.SettingsLogin;
using StationTireInspection.JDO.DataToServer;
using StationTireInspection.UDT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StationTireInspection.Forms
{
    public partial class StationSettings : Form
    {
        private SettingsJDO Settings;
        private DataToServerJDO DataToServer;
        private LoginBox LoginBox;

        private string ErrorMessageBoxTitle = "";
        private string Error = "";

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public StationSettings(SettingsJDO settings, DataToServerJDO dataToServer, LoginBox loginBox)
        {
            InitializeComponent();

            Settings = settings;
            DataToServer = dataToServer;
            LoginBox = loginBox;

            SetInitValue();

            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTitle.Text = "Nastavení Databáze";
                lblName.Text = "Jméno:";
                lblStationID.Text = "ID: ";
                btnApply.Text = "Použít";

                ErrorMessageBoxTitle = "Název stanice nesmí být prázdný.";

                Error = "IP Adresa není ve správném tvaru. Např. 192.168.1.1";


                MessageMessageBoxTitle = "Zpráva";

                Message = "Data byla úspěšně uložena.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblTitle.Text = "Database Settings";
                lblName.Text = "Name:";
                lblStationID.Text = "ID: ";
                btnApply.Text = "Apply";

                ErrorMessageBoxTitle = "User Input Error";

                Error = "Station name must not be empty.";

                MessageMessageBoxTitle = "Message";

                Message = "Data was be correctly seved";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (LoginBox.CheckLogin() == false) return;

            if (tbName.Text != null && tbName.Text != "")
            {
                Settings.StationSettings.StationName = tbName.Text;
                DataToServer.StationInformation.StationName = tbName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Error);
                return;
            }

            Settings.StationSettings.StationID = cbStaionID.SelectedIndex + 1;
            DataToServer.StationInformation.StationID = cbStaionID.SelectedIndex + 1;

            CustomMessageBox.ShowPopup(MessageMessageBoxTitle, Message);
        }

        private void SetInitValue()
        {
            tbName.Text = Settings.StationSettings.StationName;

            if (Settings.StationSettings.StationID > 0)
            {
                cbStaionID.SelectedIndex = Settings.StationSettings.StationID - 1;
            }
            else
            {
                cbStaionID.SelectedIndex = Settings.StationSettings.StationID;
            }
        }

        private void StationSettings_VisibleChanged(object sender, EventArgs e)
        {
            SetInitValue();
        }
    }
}
