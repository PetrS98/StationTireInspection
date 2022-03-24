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
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
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
            }
            else if (Translator.Language == Language.ENG)
            {
                lblNewPassword.Text = "New Password:";
                lblConfirmNewPassword.Text = "Confirm New Password:";
                btnChangePassword.Text = "Change";
                btnCancle.Text = "Cancel";
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {

        }

        public void ClearInputs()
        {
            tbNewPassword.Text = "";
            tbConfirmNewPassword.Text = "";
        }
    }
}
