using StationTireInspection.Classes;
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
        public Login()
        {
            InitializeComponent();
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
            }
            else if (Translator.Language == Language.ENG)
            {
                lblUserName.Text = "User Name:";
                lblPassword.Text = "Password:";
                btnLogin.Text = "Login";
                btnLogoff.Text = "Logoff";
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
    }
}
