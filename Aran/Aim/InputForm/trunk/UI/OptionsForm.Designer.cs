namespace Aran.Aim.InputForm
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.ui_mainTabControl = new System.Windows.Forms.TabControl();
            this.ui_dbTabPage = new System.Windows.Forms.TabPage();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_dbProviderControl = new Aran.Controls.DbProviderControl();
            this.ui_mainTabControl.SuspendLayout();
            this.ui_dbTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_mainTabControl
            // 
            this.ui_mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainTabControl.Controls.Add(this.ui_dbTabPage);
            this.ui_mainTabControl.Location = new System.Drawing.Point(3, 3);
            this.ui_mainTabControl.Name = "ui_mainTabControl";
            this.ui_mainTabControl.SelectedIndex = 0;
            this.ui_mainTabControl.Size = new System.Drawing.Size(401, 270);
            this.ui_mainTabControl.TabIndex = 1;
            // 
            // ui_dbTabPage
            // 
            this.ui_dbTabPage.Controls.Add(this.ui_dbProviderControl);
            this.ui_dbTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_dbTabPage.Name = "ui_dbTabPage";
            this.ui_dbTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_dbTabPage.Size = new System.Drawing.Size(393, 244);
            this.ui_dbTabPage.TabIndex = 1;
            this.ui_dbTabPage.Text = "Database";
            this.ui_dbTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(326, 279);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 4;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(245, 279);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 3;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_dbProviderControl
            // 
            this.ui_dbProviderControl.ConnectionType = Aran.AranEnvironment.ConnectionType.Aran;
            this.ui_dbProviderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dbProviderControl.Location = new System.Drawing.Point(3, 3);
            this.ui_dbProviderControl.Name = "ui_dbProviderControl";
            this.ui_dbProviderControl.ReadOnly = false;
            this.ui_dbProviderControl.Size = new System.Drawing.Size(387, 238);
            this.ui_dbProviderControl.TabIndex = 0;
            this.ui_dbProviderControl.UserNameOrPasswordVisible = true;
            this.ui_dbProviderControl.VisibleDbTypePanel = false;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(407, 310);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_mainTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.ui_mainTabControl.ResumeLayout(false);
            this.ui_dbTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ui_mainTabControl;
        private System.Windows.Forms.TabPage ui_dbTabPage;
        private Controls.DbProviderControl ui_dbProviderControl;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;

    }
}