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
        public bool UserLoged { get; set; } = false;

        public LoginBox()
        {
            InitializeComponent();
        }

        private bool mouseDown;
        private Point lastLocation;

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
            Hide();
        }

        public void ShowLoginDialog(SettingsLoginJDO SettingsLogin)
        {
            Show();
        }
    }
}
