
namespace StationTireInspection.UserControls
{
    partial class ClientStatusDot
    {
        /// <summary> 
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód vygenerovaný pomocí Návrháře komponent

        /// <summary> 
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientStatusDot));
            this.pbDot = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDot)).BeginInit();
            this.SuspendLayout();
            // 
            // pbDot
            // 
            this.pbDot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDot.Image = ((System.Drawing.Image)(resources.GetObject("pbDot.Image")));
            this.pbDot.Location = new System.Drawing.Point(0, 0);
            this.pbDot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbDot.Name = "pbDot";
            this.pbDot.Size = new System.Drawing.Size(25, 23);
            this.pbDot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDot.TabIndex = 46;
            this.pbDot.TabStop = false;
            // 
            // ClientStatusDot
            // 
            this.Controls.Add(this.pbDot);
            this.Name = "ClientStatusDot";
            this.Size = new System.Drawing.Size(25, 23);
            ((System.ComponentModel.ISupportInitialize)(this.pbDot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pbDot;
    }
}
