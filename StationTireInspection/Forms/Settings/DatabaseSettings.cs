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

namespace StationTireInspection.Forms
{
    public partial class DatabaseSettings : Form
    {
        private SettingsJDO Settings;

        private string ErrorMessageBoxTitle = "";
        private string[] Errors = new string[5];

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public DatabaseSettings(SettingsJDO settings)
        {
            InitializeComponent();

            Settings = settings;

            SetInitValue();

            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTitle.Text = "Nastavení Databáze";
                lblIPAddress.Text = "IP Adresa:";
                lblDatabaseName.Text = "Jméno Databáze:";
                lblTableName.Text = "Jméno Tabulky:";
                lblDatabaseUserName.Text = "Uživatelské Jméno k Databázi:";
                lblDatabasePassword.Text = "Heslo k Databázi:";
                btnConnect.Text = "Připojit";
                btnDisconnect.Text = "Odpojit";
                btnApply.Text = "Použít";

                ErrorMessageBoxTitle = "Chyba uživatelského vstupu";

                Errors[0] = "IP Adresa není ve správném tvaru. Např. 192.168.1.1";
                Errors[1] = "Jméno databáze není ve správném tvaru nebo obsahuje nepovolené znaky. Např. Databaze_";
                Errors[2] = "Jméno tabulky není ve správném tvaru nebo obsahuje nepovolené znaky. Např. Tabulka_";
                Errors[3] = "Uživatelské jméno pro připojení k databázi není ve správném tvaru nebo obsahuje nepovolené znaky. Např. User1";
                Errors[4] = "Heslo k databázi nesmí být prázdné.";

                MessageMessageBoxTitle = "Zpráva";

                Message = "Data byla úspěšně uložena.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblTitle.Text = "Database Settings";
                lblIPAddress.Text = "IP Address:";
                lblDatabaseName.Text = "Database Name:";
                lblTableName.Text = "Table Name:";
                lblDatabaseUserName.Text = "Database User Name:";
                lblDatabasePassword.Text = "Database Password:";
                btnConnect.Text = "Connect";
                btnDisconnect.Text = "Disconnect";
                btnApply.Text = "Apply";

                ErrorMessageBoxTitle = "User Input Error";

                Errors[0] = "IP Address is not in valide format. Eg. 192.168.1.1";
                Errors[1] = "Database name is not in correct format or contains illegal characters. Eg. Database_";
                Errors[2] = "Table name is not in correct format or contains illegal characters. Eg. Table_";
                Errors[3] = "Database user name is not in correct format or contains illegal characters. Eg. Table_";
                Errors[4] = "Database password must not be empty.";

                MessageMessageBoxTitle = "Message";

                Message = "Data was be correctly seved";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
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

            if (TextBoxHelper.TbInputIsText(tbTableName))
            {
                Settings.DatabaseSettings.TableName = tbTableName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[2]);
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabaseUserName))
            {
                Settings.DatabaseSettings.DatabaseUserName = tbDatabaseUserName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[3]);
                return;
            }

            if (tbDatabasePassword.Text != null && tbDatabasePassword.Text != "")
            {
                Settings.DatabaseSettings.DatabasePassword = tbDatabasePassword.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Errors[4]);
                return;
            }

            CustomMessageBox.ShowPopup(MessageMessageBoxTitle, Message);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }

        private void SetInitValue()
        {
            ipAddressBox.IPAddress = Settings.DatabaseSettings.IPAddress;
            tbDatabaseName.Text = Settings.DatabaseSettings.DatabaseName;
            tbTableName.Text = Settings.DatabaseSettings.TableName;
            tbDatabaseUserName.Text = Settings.DatabaseSettings.DatabaseUserName;
            tbDatabasePassword.Text = "";
        }
    }
}
