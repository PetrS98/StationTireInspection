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
    public partial class Login : Form
    {
        MySQLDatabase MySQLDatabase;
        SettingsJDO Settings;
        ChangePassword ChangePassword;

        string ErrorTitle = "";
        string[] Errors = new string[3];

        string MessageTitle = "";
        string[] Messages = new string[2];

        public event EventHandler<Result> LoginResultChanged;

        private Result loginResult;

        public Result LoginResult
        {
            get { return loginResult; }
            set
            {
                LoginResultChanged?.Invoke(this, value);

                loginResult = value; 
            }
        }

        public Login(MySQLDatabase mySQLDatabase, SettingsJDO settings, ChangePassword changePassword)
        {
            InitializeComponent();

            MySQLDatabase = mySQLDatabase;
            Settings = settings;
            ChangePassword = changePassword;

            Translator.LanguageChanged += Translate;

            EnableControls(true);
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblUserName.Text = "Uživatelské Jméno:";
                lblPassword.Text = "Heslo:";
                btnLogin.Text = "Přihlásit";
                btnLogoff.Text = "Odhlásit";

                ErrorTitle = "Chyba";

                Errors[0] = "Uživatelské jméno musí být číslo. Např. 40156312";
                Errors[1] = "Data o uživately nelze vyhledat v databázi.";
                Errors[2] = "Heslo není správné.";

                MessageTitle = "Zpráva";

                Messages[0] = "Přihlášení uživatele proběhlo v pořádku.";
                Messages[1] = "Je vyžadována změna hesla. Bez změny hesla se již nelze přihlásit.";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblUserName.Text = "User Name:";
                lblPassword.Text = "Password:";
                btnLogin.Text = "Login";
                btnLogoff.Text = "Logoff";

                ErrorTitle = "Error";

                Errors[0] = "User name must be number. Eg. 40156312";
                Errors[1] = "User data was not be search in database.";
                Errors[2] = "Password is not correct.";

                MessageTitle = "Message";

                Messages[0] = "User was be correctly logged.";
                Messages[1] = "Changed password required. Without changing your passwor you will not be able to loggin!";
            }
        }

        private void EnableControls(bool Enable)
        {
            tbUserName.Enabled = Enable;
            tbPassword.Enabled = Enable;
            btnLogin.Enabled = Enable;
            btnLogoff.Enabled = !Enable;
        }

        public void ClearInputs()
        {
            tbUserName.Text = "";
            tbPassword.Text = "";
        }

        private Result LoginCMD()
        {
            if (TextBoxHelper.TbInputIsNumber(tbUserName) == false)
            {
                CustomMessageBox.ShowPopup(ErrorTitle, Errors[0]);
                return Result.Error;
            }

            UserInformations UserInformations = MySQLDatabase.ReadUserInformation(Settings.DatabaseSettings.TableName, int.Parse(tbUserName.Text));

            if (UserInformations == null)
            {
                CustomMessageBox.ShowPopup(ErrorTitle, Errors[1]);
                return Result.Error;
            }

            if(UserInformations.AskPasswordChanged == 0)
            {
                if (CheckPassword(tbPassword.Text, UserInformations.Password))
                {
                    EnableControls(false);

                    CustomMessageBox.ShowPopup(MessageTitle, Messages[0]);

                    return Result.Logged;
                }
            }
            else
            {
                if (CheckPassword(tbPassword.Text, UserInformations.Password))
                {
                    ChangePassword.ID_ChangePassCMD = tbUserName.Text;
                    ChangePassword.ActualPassword_ChangePassCMD = tbPassword.Text;
                    ClearInputs();

                    CustomMessageBox.ShowPopup(MessageTitle, Messages[1]);

                    return Result.ChangePassword;
                }
            }

            CustomMessageBox.ShowPopup(ErrorTitle, Errors[2]);
            tbPassword.Text = "";
            return Result.Error;
        }

        private bool CheckPassword(string UserInputPassword, string PasswordFromDB)
        {
            if (UserInputPassword == PasswordFromDB)
            {
                return true;
            }

            return false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginResult = LoginCMD();
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            ClearInputs();
            LoginResult = Result.NoLogged;
            EnableControls(true);
        }
    }
}
