
namespace StationTireInspection.UserControls
{
    partial class PermissionPicker
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
            this.lblTextUserPermissions = new System.Windows.Forms.Label();
            this.lblTextPermissionsList = new System.Windows.Forms.Label();
            this.btnRemovePermission = new System.Windows.Forms.Button();
            this.btnAddPermission = new System.Windows.Forms.Button();
            this.lbUserPermissions = new System.Windows.Forms.ListBox();
            this.lbPermissionsList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblTextUserPermissions
            // 
            this.lblTextUserPermissions.AutoSize = true;
            this.lblTextUserPermissions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTextUserPermissions.ForeColor = System.Drawing.Color.White;
            this.lblTextUserPermissions.Location = new System.Drawing.Point(234, 4);
            this.lblTextUserPermissions.Name = "lblTextUserPermissions";
            this.lblTextUserPermissions.Size = new System.Drawing.Size(129, 20);
            this.lblTextUserPermissions.TabIndex = 25;
            this.lblTextUserPermissions.Text = "User Permissions";
            // 
            // lblTextPermissionsList
            // 
            this.lblTextPermissionsList.AutoSize = true;
            this.lblTextPermissionsList.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTextPermissionsList.ForeColor = System.Drawing.Color.White;
            this.lblTextPermissionsList.Location = new System.Drawing.Point(3, 4);
            this.lblTextPermissionsList.Name = "lblTextPermissionsList";
            this.lblTextPermissionsList.Size = new System.Drawing.Size(122, 20);
            this.lblTextPermissionsList.TabIndex = 24;
            this.lblTextPermissionsList.Text = "Permissions List";
            // 
            // btnRemovePermission
            // 
            this.btnRemovePermission.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRemovePermission.FlatAppearance.BorderSize = 0;
            this.btnRemovePermission.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Purple;
            this.btnRemovePermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemovePermission.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRemovePermission.ForeColor = System.Drawing.Color.White;
            this.btnRemovePermission.Location = new System.Drawing.Point(194, 65);
            this.btnRemovePermission.Name = "btnRemovePermission";
            this.btnRemovePermission.Size = new System.Drawing.Size(34, 32);
            this.btnRemovePermission.TabIndex = 23;
            this.btnRemovePermission.Text = "<--";
            this.btnRemovePermission.UseVisualStyleBackColor = false;
            this.btnRemovePermission.Click += new System.EventHandler(this.btnRemovePermission_Click);
            // 
            // btnAddPermission
            // 
            this.btnAddPermission.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAddPermission.FlatAppearance.BorderSize = 0;
            this.btnAddPermission.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Purple;
            this.btnAddPermission.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPermission.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddPermission.ForeColor = System.Drawing.Color.White;
            this.btnAddPermission.Location = new System.Drawing.Point(194, 27);
            this.btnAddPermission.Name = "btnAddPermission";
            this.btnAddPermission.Size = new System.Drawing.Size(34, 32);
            this.btnAddPermission.TabIndex = 22;
            this.btnAddPermission.Text = "-->";
            this.btnAddPermission.UseVisualStyleBackColor = false;
            this.btnAddPermission.Click += new System.EventHandler(this.btnAddPermission_Click);
            // 
            // lbUserPermissions
            // 
            this.lbUserPermissions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbUserPermissions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbUserPermissions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbUserPermissions.ForeColor = System.Drawing.Color.White;
            this.lbUserPermissions.FormattingEnabled = true;
            this.lbUserPermissions.ItemHeight = 20;
            this.lbUserPermissions.Location = new System.Drawing.Point(234, 27);
            this.lbUserPermissions.Name = "lbUserPermissions";
            this.lbUserPermissions.Size = new System.Drawing.Size(186, 180);
            this.lbUserPermissions.TabIndex = 21;
            // 
            // lbPermissionsList
            // 
            this.lbPermissionsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbPermissionsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbPermissionsList.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbPermissionsList.ForeColor = System.Drawing.Color.White;
            this.lbPermissionsList.FormattingEnabled = true;
            this.lbPermissionsList.ItemHeight = 20;
            this.lbPermissionsList.Location = new System.Drawing.Point(3, 27);
            this.lbPermissionsList.Name = "lbPermissionsList";
            this.lbPermissionsList.Size = new System.Drawing.Size(185, 180);
            this.lbPermissionsList.TabIndex = 20;
            // 
            // PermissionPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.lblTextUserPermissions);
            this.Controls.Add(this.lblTextPermissionsList);
            this.Controls.Add(this.btnRemovePermission);
            this.Controls.Add(this.btnAddPermission);
            this.Controls.Add(this.lbUserPermissions);
            this.Controls.Add(this.lbPermissionsList);
            this.Name = "PermissionPicker";
            this.Size = new System.Drawing.Size(425, 215);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTextUserPermissions;
        private System.Windows.Forms.Label lblTextPermissionsList;
        private System.Windows.Forms.Button btnRemovePermission;
        private System.Windows.Forms.Button btnAddPermission;
        private System.Windows.Forms.ListBox lbUserPermissions;
        private System.Windows.Forms.ListBox lbPermissionsList;
    }
}
