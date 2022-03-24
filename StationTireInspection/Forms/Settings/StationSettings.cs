using StationTireInspection.Classes;
using StationTireInspection.Forms.MessageBoxes;
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
        public string StationName { get; set; }

        private string ErrorMessageBoxTitle = "";
        private string Error = "";

        private string MessageMessageBoxTitle = "";
        private string Message = "";

        public StationSettings()
        {
            InitializeComponent();
            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblTitle.Text = "Nastavení Databáze";
                lblName.Text = "Jméno:";
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
                btnApply.Text = "Apply";

                ErrorMessageBoxTitle = "User Input Error";

                Error = "Station name must not be empty.";

                MessageMessageBoxTitle = "Message";

                Message = "Data was be correctly seved";
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if(tbName.Text != null && tbName.Text != "")
            {
                StationName = tbName.Text;
            }
            else
            {
                CustomMessageBox.ShowPopup(ErrorMessageBoxTitle, Error);
                return;
            }

            CustomMessageBox.ShowPopup(MessageMessageBoxTitle, Message);
        }
    }
}
