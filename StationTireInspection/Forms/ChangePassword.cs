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
    public partial class ChangePassword : Form
    {
        MySQLDatabase MySQLDatabase;
        SettingsJDO Settings;

        string[] ErrorTitle = new string[2];
        string[] Errors = new string[3];

        string MessageTitle = "";
        string Message = "";

        private string userID_ChangePassword;

        public string UserID_ChangePassword
        {
            get { return userID_ChangePassword; }
            set 
            {
                if (value != "" || value != null) CopyUserID(value);

                userID_ChangePassword = value; 
            }
        }

        private void CopyUserID(string UserID)
        {
            tbUserName.Text = UserID;
        }

        public ChangePassword(MySQLDatabase mySQLDatabase, SettingsJDO settings)
        {
            InitializeComponent();

            MySQLDatabase = mySQLDatabase;
            Settings = settings;

            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblNewPassword.Text = "Nové Heslo:";
                lblConfirmNewPassword.Text = "Potvrzení Nového Hesla:";
                btnChangePassword.Text = "Změnit";
                btnCancle.Text = "Zrušit";

                ErrorTitle[0] = "Chyba uživatelského vstupu";

                Errors[0] = "Uživatelské jméno musí být číslo. Např. 40156312";
                Errors[1] = "Heslo a potvrzení sesla se musí shodovat.";

                ErrorTitle[1] = "Chyba";

                Errors[2] = "Změnu hesla nebylo možné dokončit.";

                MessageTitle = "Zpráva";

                Message = "Heslo bylo uspěšně změněno.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblNewPassword.Text = "New Password:";
                lblConfirmNewPassword.Text = "Confirm New Password:";
                btnChangePassword.Text = "Change";
                btnCancle.Text = "Cancel";

                ErrorTitle[0] = "User Input Error";

                Errors[0] = "User name must be number. Eg. 40156312";
                Errors[1] = "Password and confirm password must be same.";

                ErrorTitle[1] = "Error";

                Errors[2] = "Password change could not be completed.";

                MessageTitle = "Message";

                Message = "Password was be correctly changed.";
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangePasswordCMD();
        }

        public void ClearInputs()
        {
            tbUserName.Text = "";
            tbNewPassword.Text = "";
            tbConfirmNewPassword.Text = "";
        }

        private void ChangePasswordCMD()
        {
            if(TextBoxHelper.TbInputIsNumber(tbUserName) == false)
            {
                CustomMessageBox.ShowPopup(ErrorTitle[0], Errors[0]);
                return;
            }

            if (tbNewPassword.Text != tbConfirmNewPassword.Text)
            {
                CustomMessageBox.ShowPopup(ErrorTitle[0], Errors[1]);
                return;
            }

            if(MySQLDatabase.UpdateUserInformations(Settings.DatabaseSettings.TableName, int.Parse(tbUserName.Text), tbNewPassword.Text, 0))
            {
                ClearInputs();

                CustomMessageBox.ShowPopup(MessageTitle, Message);

                return;
            }
            CustomMessageBox.ShowPopup(ErrorTitle[1], Errors[3]);
        }
    }
}
