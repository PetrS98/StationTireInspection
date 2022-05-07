using StationTireInspection.Classes;
using StationTireInspection.Forms.MessageBoxes;
using StationTireInspection.JDO.SettingsLogin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StationTireInspection.Forms.SettingsLogin
{
    public partial class LoginBox : Form
    {
        private bool mouseDown;
        private Point lastLocation;

        public string LoginUser { get; set; } = "";

        SettingsLoginJDO SettingsLogin;

        private string Error = "";

        public event EventHandler<bool> LogedChanged;

        private bool logedIn;

        public bool LogedIn
        {
            get
            {
                return logedIn;
            }
            set
            {
                if (logedIn != value) LogedChanged?.Invoke(null, value);

                logedIn = value;
            }
        }

        public LoginBox(SettingsLoginJDO settingsLogin)
        {
            InitializeComponent();

            Translator.LanguageChanged += Translate;

            SettingsLogin = settingsLogin;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblUserName.Text = "Uživatelské Jméno:";
                lblPassword.Text = "Heslo:";
                btnLogin.Text = "Přihlásit";
                btnCancel.Text = "Zrušit";

                Error = "Uživatelské jméno nebo heslo není správné!";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblUserName.Text = "User Name:";
                lblPassword.Text = "Password:";
                btnLogin.Text = "Login";
                btnCancel.Text = "Cancel";

                Error = "User name or password is incorrect!";
            }
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Location = new Point((Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);
                Update();
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            HideLoginDialog(false);
        }

        public void ShowLoginDialog()
        {
            if (Visible == true) return;
            Show();
        }

        public void HideLoginDialog(bool LogOff)
        {
            tbPassword.Text = "";
            tbUserName.Text = "";

            if (Visible == false) return;

            Hide();

            if (LogOff == false && LogedIn == true) return;

            LogedIn = false;
            LoginUser = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SettingsLogin.UserNames.Length; i++)
            {
                if(tbUserName.Text == SettingsLogin.UserNames[i] && tbPassword.Text == SettingsLogin.Passwords[i])
                {
                    LoginUser = SettingsLogin.UserNames[i];
                    LogedIn = true;
                    break;
                }
            }

            if(LogedIn == true)
            {
                HideLoginDialog(false);
            }
            else
            {
                CustomMessageBox.ShowPopup("Login Error", Error);
                return;
            }
        }

        public bool CheckLogin()
        {
            if (LogedIn == false)
            {
                ShowLoginDialog();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            HideLoginDialog(true);
        }
    }
}
