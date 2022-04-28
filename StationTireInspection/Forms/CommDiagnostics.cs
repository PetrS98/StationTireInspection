using StationTireInspection.Classes;
using StationTireInspection.JDO.DataToServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VisualInspection.Utils;
using VisualInspection.Utils.Net;

namespace StationTireInspection.Forms
{
    public partial class CommDiagnostics : Form
    {

        MySQLDatabase MySQLDatabase;
        TCPIPClient ReaderTCPClient;
        TCPIPClient ServerTCPClient;
        DataToServerJDO DataToServer;

        private string BarcodeMemory = "";

        private string[] BufferItems = new string[23];

        public CommDiagnostics(MySQLDatabase mySQLDatabase, TCPIPClient readerTCPClient, TCPIPClient serverTCPClient, DataToServerJDO dataToServer)
        {
            InitializeComponent();

            MySQLDatabase = mySQLDatabase;
            ReaderTCPClient = readerTCPClient;
            ServerTCPClient = serverTCPClient;
            DataToServer = dataToServer;

            Translator.LanguageChanged += Translate;
            ReaderTCPClient.DataChanged += ReaderData_Changed;

            DatabaseStatusDot.Client = MySQLDatabase;
            ReaderClientStatusDot.Client = ReaderTCPClient;
            MainAppStatusDot.Client = ServerTCPClient;
        }

        private void ReaderData_Changed(object sender, byte[] e)
        {
            if (BarcodeMemory == DataToServer.Barcode) return;
            AddToBuffer(DataToServer.Barcode);
            BarcodeMemory = DataToServer.Barcode;
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
            tbBuffer.Invoke((MethodInvoker)delegate ()
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
            });
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbBuffer.Text = "";
            BufferItems = new string[BufferItems.Length];
        }
    }
}
