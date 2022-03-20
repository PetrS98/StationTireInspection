using System;
using System.Drawing;
using System.Windows.Forms;
using VisualInspection.Utils.Net;

namespace StationTireInspection.UserControls
{
    public partial class ClientStatusDot : UserControl
    {
        private static readonly Image GreenDot = Properties.Resources.GreenDot_3;
        private static readonly Image RedDot = Properties.Resources.RedDot_2;
        private static readonly Image WhiteDot = Properties.Resources.WhiteDot_2;

        private System.Timers.Timer _refreshTimer = new System.Timers.Timer();

        private IHasClientStatus client = null;
        private ClientStatus status = ClientStatus.Disconnected;

        public IHasClientStatus Client
        {
            get { return client; }
            set
            {
                if (value == null || client == value) return;
                if (client != null) client.StatusChanged -= (value, e) => Status = e;
                client = value;
                Status = value.Status;
                client.StatusChanged += (value, e) => Status = e;
            }
        }

        public ClientStatus Status
        {
            get { return status; }
            set 
            {
                bool changed = status != value;
                status = value;
                if (!changed) return;
                if (status == ClientStatus.Connected) _refreshTimer.Start();
                else
                {
                    _refreshTimer.Stop();
                    UpdateDot(this, null);
                }
            }
        }

        public ClientStatusDot()
        {
            InitializeComponent();
            _refreshTimer.Interval = 500;
            _refreshTimer.Elapsed += UpdateDot;
            UpdateDot(this, null);
        }

        private void UpdateDot(object sender, EventArgs e)
        {
            if (pbDot.InvokeRequired)
            {
                pbDot.Invoke(new Action(() =>
                {
                    pbDot.Image = GetImageToSet();
                }));
            }
            else
            {
                pbDot.Image = GetImageToSet();
            }
        }

        private Image GetImageToSet()
        {
            return status switch
            {
                ClientStatus status when status == ClientStatus.Connected => pbDot.Image == WhiteDot ? GreenDot : WhiteDot,
                ClientStatus status when status == ClientStatus.Connecting => WhiteDot,
                ClientStatus status when status == ClientStatus.Disconnected => RedDot,
                _ => RedDot
            };
        }
    }
}
