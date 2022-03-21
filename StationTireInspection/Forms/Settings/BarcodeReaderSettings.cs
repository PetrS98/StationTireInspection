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
    public partial class BarcodeReaderSettings : Form
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public BarcodeReaderSettings()
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
                // TODO dodělat error.
                return;
            }

            if (TextBoxHelper.TbInputIsNumber(tbPort))
            {
                Port = int.Parse(tbPort.Text);
            }
            else
            {
                // TODO dodělat error.
                return;
            }

            // TODO dodělat message o validním uložením dat.
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }
    }
}
