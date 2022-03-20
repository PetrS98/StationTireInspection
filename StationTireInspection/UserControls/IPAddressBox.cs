using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace VisualInspection.App.UserControls
{
    public partial class IPAddressBox : UserControl
    {
        private int ipAddressBlinkCounter = 0;
        private Color baseBackColor;

        private Timer timerIPAddressBlink = new Timer();

        private Regex ipRegex = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

        public bool IPAddressValid { get; private set; } = false;

        public Color TextBoxBackColor
        {
            get { return textBox.BackColor; }
            set
            {
                textBox.BackColor = value;
            }
        }

        public Color BlinkBackColor { get; set; } = Color.Salmon;

        public string IPAddress
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public HorizontalAlignment IPAddressAlign
        {
            get { return textBox.TextAlign; }
            set { textBox.TextAlign = value; }
        }

        public IPAddressBox()
        {
            InitializeComponent();

            timerIPAddressBlink.Interval = 500;
            timerIPAddressBlink.Tick += BlinkIPAddress;
        }

        private bool TryParse(string text)
        {
            return ipRegex.IsMatch(text);
        }

        public void Blink()
        {
            //timerIPAddressBlink.Start();
        }

        private void BlinkIPAddress(object sender, EventArgs e)
        {
            TextBoxBackColor = TextBoxBackColor == baseBackColor ? BlinkBackColor : baseBackColor;
            if (++ipAddressBlinkCounter != 4) return;
            ipAddressBlinkCounter = 0;
            timerIPAddressBlink.Stop();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            IPAddressValid = TryParse(textBox.Text);
            textBox.ForeColor = IPAddressValid ? Color.Green : Color.Red;
        }
    }
}
