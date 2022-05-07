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
using VisualInspection.Utils.Net;
using VisualInspection.Utils;
using StationTireInspection.Forms.SettingsLogin;

namespace StationTireInspection.Forms
{
    public partial class DatabaseSettings : Form
    {
        private SettingsJDO Settings;
        MySQLDatabase MySQLDatabase;
        LoginBox LoginBox;

        private string ErrorMessageBoxTitle = "";
        private string[] Errors = new string[7];

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public DatabaseSettings(SettingsJDO settings, MySQLDatabase mySQLDatabase, LoginBox loginBox)
        {
            InitializeComponent();

            MySQLDatabase = mySQLDatabase;
            Settings = settings;
            LoginBox = loginBox;

            SetJSONDataToContols();
            SetControlEnables(true);

            MySQLDatabase.StatusChanged += DBStatusChange;
            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTitle.Text = "Nastavení Databáze";
                lblIPAddress.Text = "IP Adresa:";
                lblDatabaseName.Text = "Jméno Databáze:";
                lblUsersTableName.Text = "Jméno Tabulky s Uživateli:";
                lblNonOPsDataTableName.Text = "Jméno Tabulky s Daty Prostojů:";
                lblDatabaseUserName.Text = "Uživatelské Jméno k Databázi:";
                lblDatabasePassword.Text = "Heslo k Databázi:";
                btnConnect.Text = "Připojit";
                btnDisconnect.Text = "Odpojit";
                btnApply.Text = "Použít";

                ErrorMessageBoxTitle = "Chyba uživatelského vstupu";

                Errors[0] = "IP Adresa není ve správném tvaru. Např. 192.168.1.1";
                Errors[1] = "Jméno databáze není ve správném tvaru nebo obsahuje nepovolené znaky. Např. Databaze_";
                Errors[2] = "Jméno tabulky s uživateli není ve správném tvaru nebo obsahuje nepovolené znaky. Např. Tabulka_";
                Errors[3] = "SPARE";
                Errors[4] = "Jméno tabulky s daty prostojů není ve správném tvaru nebo obsahuje nepovolené znaky. Např.Tabulka_";
                Errors[5] = "Uživatelské jméno pro připojení k databázi není ve správném tvaru nebo obsahuje nepovolené znaky. Např. User1";
                Errors[6] = "Heslo k databázi nesmí být prázdné.";

                MessageMessageBoxTitle = "Zpráva";

                Message = "Data byla úspěšně uložena.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblTitle.Text = "Database Settings";
                lblIPAddress.Text = "IP Address:";
                lblDatabaseName.Text = "Database Name:";
                lblUsersTableName.Text = "Users Table Name:";
                lblNonOPsDataTableName.Text = "NonOPs Data Table Name:";
                lblDatabaseUserName.Text = "Database User Name:";
                lblDatabasePassword.Text = "Database Password:";
                btnConnect.Text = "Connect";
                btnDisconnect.Text = "Disconnect";
                btnApply.Text = "Apply";

                ErrorMessageBoxTitle = "User Input Error";

                Errors[0] = "IP Address is not in valide format. Eg. 192.168.1.1";
                Errors[1] = "Database name is not in correct format or contains illegal characters. Eg. Database_";
                Errors[2] = "Users Table name is not in correct format or contains illegal characters. Eg. Table_";
                Errors[3] = "SPARE";
                Errors[4] = "NonOPs Data Table name is not in correct format or contains illegal characters. Eg. Table_";
                Errors[5] = "Database user name is not in correct format or contains illegal characters. Eg. Table_";
                Errors[6] = "Database password must not be empty.";

                MessageMessageBoxTitle = "Message";

                Message = "Data was be correctly seved";
            }
        }

        private void DBStatusChange(object sender, ClientStatus e)
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

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (LoginBox.CheckLogin() == false) return;

            if (ipAddressBox.IPAddressValid)
            {
                Settings.DatabaseSettings.IPAddress = ipAddressBox.IPAddress;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[0]);
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabaseName))
            {
                Settings.DatabaseSettings.DatabaseName = tbDatabaseName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[1]);
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbUsersTableName))
            {
                Settings.DatabaseSettings.UsersTableName = tbUsersTableName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[2]);
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbNonOPsDataTableName))
            {
                Settings.DatabaseSettings.NonOPsDataTableName = tbNonOPsDataTableName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[4]);
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabaseUserName))
            {
                Settings.DatabaseSettings.DatabaseUserName = tbDatabaseUserName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[5]);
                return;
            }

            if (tbDatabasePassword.Text != null && tbDatabasePassword.Text != "")
            {
                Settings.DatabaseSettings.DatabasePassword = tbDatabasePassword.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[6]);
                return;
            }

            CustomMessageBox.ShowPopup(MessageMessageBoxTitle, Message);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (LoginBox.CheckLogin() == false) return;

            if (MySQLDatabase.Status != ClientStatus.Disconnected) return;
            MySQLDatabase.ConnectToDB_Async(Settings.DatabaseSettings.IPAddress, Settings.DatabaseSettings.DatabaseUserName, Settings.DatabaseSettings.DatabasePassword);

            //MySQLDatabase.ConnectToDB(Settings.DatabaseSettings.IPAddress, Settings.DatabaseSettings.DatabaseUserName, Settings.DatabaseSettings.DatabasePassword);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (LoginBox.CheckLogin() == false) return;

            if (MySQLDatabase.Equals(ClientStatus.Connected)) return;
            MySQLDatabase.DisconnectFromDB();
        }

        private void DatabaseSettings_VisibleChanged(object sender, EventArgs e)
        {
            SetJSONDataToContols();
        }

        private void SetJSONDataToContols()
        {
            ipAddressBox.IPAddress = Settings.DatabaseSettings.IPAddress;
            tbDatabaseName.Text = Settings.DatabaseSettings.DatabaseName;
            tbUsersTableName.Text = Settings.DatabaseSettings.UsersTableName;
            tbNonOPsDataTableName.Text = Settings.DatabaseSettings.NonOPsDataTableName;
            tbDatabaseUserName.Text = Settings.DatabaseSettings.DatabaseUserName;
            tbDatabasePassword.Text = Settings.DatabaseSettings.DatabasePassword;
        }

        private void SetControlEnables(bool Enable)
        {
            btnConnect.Enabled = Enable;
            btnDisconnect.Enabled = !Enable;
        }
    }
}
