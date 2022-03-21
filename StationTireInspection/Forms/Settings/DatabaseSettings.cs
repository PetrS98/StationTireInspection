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
    public partial class DatabaseSettings : Form
    {
        public string IPAddress { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }

        public DatabaseSettings()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (ipAddressBox.IPAddressValid)
            {
                IPAddress = ipAddressBox.IPAddress;
            }
            else
            {
                // TODO přidat error.
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabaseName))
            {
                DatabaseName = tbDatabaseName.Text;
            }
            else
            {
                // TODO přidat error.
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbTableName))
            {
                TableName = lblTableName.Text;
            }
            else
            {
                // TODO přidat error.
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabaseUserName))
            {
                DatabaseName = tbDatabaseUserName.Text;
            }
            else
            {
                // TODO přidat error.
                return;
            }

            if (TextBoxHelper.TbInputIsText(tbDatabasePassword))
            {
                DatabasePassword = tbDatabasePassword.Text;
            }
            else
            {
                // TODO přidat error.
                return;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }
    }
}
