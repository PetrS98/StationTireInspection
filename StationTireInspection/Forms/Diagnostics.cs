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
    public partial class Diagnostics : Form
    {
        private string[] BufferItems = new string[23];

        public Diagnostics()
        {
            InitializeComponent();
            Translator.LanguageChanged += Translate;
        }

        private void Translate(object sender, Language e)
        {
            if (Translator.Language == Language.CZ)
            {
                lblReaderStatus.Text = "Status Čtečky:";
                lblDatabaseStatus.Text = "Status Databáze:";
                lblBarcodeBuffer.Text = "Buffer Barkódů:";
                btnClear.Text = "Vyčistit";
            }
            else if (Translator.Language == Language.ENG)
            {
                lblReaderStatus.Text = "Reader Status:";
                lblDatabaseStatus.Text = "Database Status:";
                lblBarcodeBuffer.Text = "Barcode Buffer:";
                btnClear.Text = "Clear";
            }
        }

        public void AddToBuffer(string Text)
        {
            DateTime dateTime = DateTime.Now;
            string text = "< " + dateTime.ToString("G") + " > " + Text;

            MoveBufferItems();

            BufferItems[0] = text;

            tbBuffer.Text = "";

            for (int i = 0; i < BufferItems.Length; i++)
            {
                tbBuffer.Text = tbBuffer.Text + Environment.NewLine + BufferItems[i];
            }
        }

        private void MoveBufferItems()
        {
            for (int i = BufferItems.Length - 1; i > 0; i--)
            {
                if(i >= 1)
                {
                    BufferItems[i] = BufferItems[i - 1];
                    continue;
                }

                BufferItems[i] = "";
            }
        }
    }
}
