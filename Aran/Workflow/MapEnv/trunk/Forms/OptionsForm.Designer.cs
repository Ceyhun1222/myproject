namespace MapEnv
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
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.ui_coordFormatRoundNud = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_coordFormatCB = new System.Windows.Forms.ComboBox();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_mainTabControl = new System.Windows.Forms.TabControl();
            this.ui_globalTabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ui_aixmMetadataWebApiWarningLabel = new System.Windows.Forms.Label();
            this.ui_useAixmMDWebApi = new System.Windows.Forms.CheckBox();
            this.clearUsrPwdButton = new System.Windows.Forms.Button();
            this.ui_featInfoWinMode = new System.Windows.Forms.ComboBox();
            this.ui_mapTabPage = new System.Windows.Forms.TabPage();
            this.ui_selSpatialRefButton = new System.Windows.Forms.Button();
            this.ui_spatialRefTB = new System.Windows.Forms.TextBox();
            this.ui_spatialRefLabel = new System.Windows.Forms.Label();
            this.ui_dbTabPage = new System.Windows.Forms.TabPage();
            this.ui_pluginsTabPage = new System.Windows.Forms.TabPage();
            this.ui_pluginsCLB = new System.Windows.Forms.CheckedListBox();
            this.ui_effectiveDateTimePicker = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_dbProviderControl = new Aran.Controls.DbProviderControl();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_coordFormatRoundNud)).BeginInit();
            this.ui_mainTabControl.SuspendLayout();
            this.ui_globalTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ui_mapTabPage.SuspendLayout();
            this.ui_dbTabPage.SuspendLayout();
            this.ui_pluginsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.ui_coordFormatRoundNud);
            groupBox1.Controls.Add(this.label1);
            groupBox1.Controls.Add(this.ui_coordFormatCB);
            groupBox1.Location = new System.Drawing.Point(12, 107);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(353, 51);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Coordinate Format in Info Bar";
            // 
            // ui_coordFormatRoundNud
            // 
            this.ui_coordFormatRoundNud.Location = new System.Drawing.Point(269, 20);
            this.ui_coordFormatRoundNud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ui_coordFormatRoundNud.Name = "ui_coordFormatRoundNud";
            this.ui_coordFormatRoundNud.Size = new System.Drawing.Size(53, 20);
            this.ui_coordFormatRoundNud.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(221, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Round:";
            // 
            // ui_coordFormatCB
            // 
            this.ui_coordFormatCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_coordFormatCB.FormattingEnabled = true;
            this.ui_coordFormatCB.Items.AddRange(new object[] {
            "Degree Minutes Seconds",
            "Decimal Degrees"});
            this.ui_coordFormatCB.Location = new System.Drawing.Point(6, 19);
            this.ui_coordFormatCB.Name = "ui_coordFormatCB";
            this.ui_coordFormatCB.Size = new System.Drawing.Size(192, 21);
            this.ui_coordFormatCB.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(5, 21);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(115, 13);
            label2.TabIndex = 14;
            label2.Text = "Default Effective Date:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(9, 20);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(139, 13);
            label3.TabIndex = 12;
            label3.Text = "Feature Info Window Mode:";
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(328, 408);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 1;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(409, 408);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 2;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ui_mainTabControl
            // 
            this.ui_mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainTabControl.Controls.Add(this.ui_globalTabPage);
            this.ui_mainTabControl.Controls.Add(this.ui_mapTabPage);
            this.ui_mainTabControl.Controls.Add(this.ui_dbTabPage);
            this.ui_mainTabControl.Controls.Add(this.ui_pluginsTabPage);
            this.ui_mainTabControl.Location = new System.Drawing.Point(12, 12);
            this.ui_mainTabControl.Name = "ui_mainTabControl";
            this.ui_mainTabControl.SelectedIndex = 0;
            this.ui_mainTabControl.Size = new System.Drawing.Size(472, 390);
            this.ui_mainTabControl.TabIndex = 0;
            this.ui_mainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            // 
            // ui_globalTabPage
            // 
            this.ui_globalTabPage.Controls.Add(this.groupBox2);
            this.ui_globalTabPage.Controls.Add(this.clearUsrPwdButton);
            this.ui_globalTabPage.Controls.Add(this.ui_featInfoWinMode);
            this.ui_globalTabPage.Controls.Add(label3);
            this.ui_globalTabPage.Controls.Add(groupBox1);
            this.ui_globalTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_globalTabPage.Name = "ui_globalTabPage";
            this.ui_globalTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_globalTabPage.Size = new System.Drawing.Size(464, 364);
            this.ui_globalTabPage.TabIndex = 2;
            this.ui_globalTabPage.Text = "Global";
            this.ui_globalTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ui_aixmMetadataWebApiWarningLabel);
            this.groupBox2.Controls.Add(this.ui_useAixmMDWebApi);
            this.groupBox2.Location = new System.Drawing.Point(12, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 64);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Metadata";
            // 
            // ui_aixmMetadataWebApiWarningLabel
            // 
            this.ui_aixmMetadataWebApiWarningLabel.BackColor = System.Drawing.Color.Yellow;
            this.ui_aixmMetadataWebApiWarningLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ui_aixmMetadataWebApiWarningLabel.Location = new System.Drawing.Point(3, 39);
            this.ui_aixmMetadataWebApiWarningLabel.Name = "ui_aixmMetadataWebApiWarningLabel";
            this.ui_aixmMetadataWebApiWarningLabel.Size = new System.Drawing.Size(347, 22);
            this.ui_aixmMetadataWebApiWarningLabel.TabIndex = 1;
            this.ui_aixmMetadataWebApiWarningLabel.Text = "Metadata will not saved, if you don\'t use Web API";
            this.ui_aixmMetadataWebApiWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_aixmMetadataWebApiWarningLabel.Visible = false;
            // 
            // ui_useAixmMDWebApi
            // 
            this.ui_useAixmMDWebApi.AutoSize = true;
            this.ui_useAixmMDWebApi.Location = new System.Drawing.Point(6, 19);
            this.ui_useAixmMDWebApi.Name = "ui_useAixmMDWebApi";
            this.ui_useAixmMDWebApi.Size = new System.Drawing.Size(154, 17);
            this.ui_useAixmMDWebApi.TabIndex = 0;
            this.ui_useAixmMDWebApi.Text = "Use Web API for Metadata";
            this.ui_useAixmMDWebApi.UseVisualStyleBackColor = true;
            this.ui_useAixmMDWebApi.CheckedChanged += new System.EventHandler(this.UseWebApi_CheckedChanged);
            // 
            // clearUsrPwdButton
            // 
            this.clearUsrPwdButton.Location = new System.Drawing.Point(6, 67);
            this.clearUsrPwdButton.Name = "clearUsrPwdButton";
            this.clearUsrPwdButton.Size = new System.Drawing.Size(192, 23);
            this.clearUsrPwdButton.TabIndex = 15;
            this.clearUsrPwdButton.Text = "Clear user name and password";
            this.clearUsrPwdButton.UseVisualStyleBackColor = true;
            this.clearUsrPwdButton.Click += new System.EventHandler(this.ClearUserNamePassword_Click);
            // 
            // ui_featInfoWinMode
            // 
            this.ui_featInfoWinMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_featInfoWinMode.FormattingEnabled = true;
            this.ui_featInfoWinMode.Location = new System.Drawing.Point(154, 17);
            this.ui_featInfoWinMode.Name = "ui_featInfoWinMode";
            this.ui_featInfoWinMode.Size = new System.Drawing.Size(121, 21);
            this.ui_featInfoWinMode.TabIndex = 13;
            // 
            // ui_mapTabPage
            // 
            this.ui_mapTabPage.Controls.Add(this.ui_effectiveDateTimePicker);
            this.ui_mapTabPage.Controls.Add(label2);
            this.ui_mapTabPage.Controls.Add(this.ui_selSpatialRefButton);
            this.ui_mapTabPage.Controls.Add(this.ui_spatialRefTB);
            this.ui_mapTabPage.Controls.Add(this.ui_spatialRefLabel);
            this.ui_mapTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_mapTabPage.Name = "ui_mapTabPage";
            this.ui_mapTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_mapTabPage.Size = new System.Drawing.Size(464, 364);
            this.ui_mapTabPage.TabIndex = 0;
            this.ui_mapTabPage.Text = "Map";
            this.ui_mapTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_selSpatialRefButton
            // 
            this.ui_selSpatialRefButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_selSpatialRefButton.Location = new System.Drawing.Point(429, 89);
            this.ui_selSpatialRefButton.Name = "ui_selSpatialRefButton";
            this.ui_selSpatialRefButton.Size = new System.Drawing.Size(29, 23);
            this.ui_selSpatialRefButton.TabIndex = 2;
            this.ui_selSpatialRefButton.Text = "...";
            this.ui_selSpatialRefButton.UseVisualStyleBackColor = true;
            this.ui_selSpatialRefButton.Click += new System.EventHandler(this.SelSpatialRefButton_Click);
            // 
            // ui_spatialRefTB
            // 
            this.ui_spatialRefTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_spatialRefTB.Location = new System.Drawing.Point(6, 89);
            this.ui_spatialRefTB.Name = "ui_spatialRefTB";
            this.ui_spatialRefTB.ReadOnly = true;
            this.ui_spatialRefTB.Size = new System.Drawing.Size(417, 20);
            this.ui_spatialRefTB.TabIndex = 1;
            // 
            // ui_spatialRefLabel
            // 
            this.ui_spatialRefLabel.AutoSize = true;
            this.ui_spatialRefLabel.Location = new System.Drawing.Point(3, 73);
            this.ui_spatialRefLabel.Name = "ui_spatialRefLabel";
            this.ui_spatialRefLabel.Size = new System.Drawing.Size(95, 13);
            this.ui_spatialRefLabel.TabIndex = 0;
            this.ui_spatialRefLabel.Text = "Spatial Reference:";
            // 
            // ui_dbTabPage
            // 
            this.ui_dbTabPage.Controls.Add(this.ui_dbProviderControl);
            this.ui_dbTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_dbTabPage.Name = "ui_dbTabPage";
            this.ui_dbTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_dbTabPage.Size = new System.Drawing.Size(464, 364);
            this.ui_dbTabPage.TabIndex = 1;
            this.ui_dbTabPage.Text = "Database";
            this.ui_dbTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_pluginsTabPage
            // 
            this.ui_pluginsTabPage.Controls.Add(this.ui_pluginsCLB);
            this.ui_pluginsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_pluginsTabPage.Name = "ui_pluginsTabPage";
            this.ui_pluginsTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ui_pluginsTabPage.Size = new System.Drawing.Size(464, 364);
            this.ui_pluginsTabPage.TabIndex = 3;
            this.ui_pluginsTabPage.Text = "Plugins";
            this.ui_pluginsTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_pluginsCLB
            // 
            this.ui_pluginsCLB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_pluginsCLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_pluginsCLB.FormattingEnabled = true;
            this.ui_pluginsCLB.Location = new System.Drawing.Point(3, 3);
            this.ui_pluginsCLB.Name = "ui_pluginsCLB";
            this.ui_pluginsCLB.Size = new System.Drawing.Size(458, 358);
            this.ui_pluginsCLB.TabIndex = 2;
            // 
            // ui_effectiveDateTimePicker
            // 
            this.ui_effectiveDateTimePicker.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_effectiveDateTimePicker.Location = new System.Drawing.Point(125, 16);
            this.ui_effectiveDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
            this.ui_effectiveDateTimePicker.Name = "ui_effectiveDateTimePicker";
            this.ui_effectiveDateTimePicker.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_effectiveDateTimePicker.Size = new System.Drawing.Size(282, 25);
            this.ui_effectiveDateTimePicker.TabIndex = 16;
            this.ui_effectiveDateTimePicker.Value = new System.DateTime(((long)(0)));
            // 
            // ui_dbProviderControl
            // 
            this.ui_dbProviderControl.ConnectionType = Aran.AranEnvironment.ConnectionType.Aran;
            this.ui_dbProviderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dbProviderControl.Location = new System.Drawing.Point(3, 3);
            this.ui_dbProviderControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_dbProviderControl.Name = "ui_dbProviderControl";
            this.ui_dbProviderControl.ReadOnly = true;
            this.ui_dbProviderControl.Size = new System.Drawing.Size(458, 358);
            this.ui_dbProviderControl.TabIndex = 0;
            this.ui_dbProviderControl.UserNameOrPasswordVisible = false;
            this.ui_dbProviderControl.VisibleDbTypePanel = false;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(496, 443);
            this.Controls.Add(this.ui_mainTabControl);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_coordFormatRoundNud)).EndInit();
            this.ui_mainTabControl.ResumeLayout(false);
            this.ui_globalTabPage.ResumeLayout(false);
            this.ui_globalTabPage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ui_mapTabPage.ResumeLayout(false);
            this.ui_mapTabPage.PerformLayout();
            this.ui_dbTabPage.ResumeLayout(false);
            this.ui_pluginsTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.TabControl ui_mainTabControl;
        private System.Windows.Forms.TabPage ui_mapTabPage;
        private System.Windows.Forms.TabPage ui_dbTabPage;
        private System.Windows.Forms.Button ui_selSpatialRefButton;
        private System.Windows.Forms.TextBox ui_spatialRefTB;
		private System.Windows.Forms.Label ui_spatialRefLabel;
		private Aran.Controls.DbProviderControl ui_dbProviderControl;
        private System.Windows.Forms.TabPage ui_globalTabPage;
		private System.Windows.Forms.NumericUpDown ui_coordFormatRoundNud;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ui_coordFormatCB;
		private System.Windows.Forms.ComboBox ui_featInfoWinMode;
        private System.Windows.Forms.TabPage ui_pluginsTabPage;
        private System.Windows.Forms.CheckedListBox ui_pluginsCLB;
        private System.Windows.Forms.Button clearUsrPwdButton;
        private Aran.Controls.Airac.AiracCycleControl ui_effectiveDateTimePicker;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ui_useAixmMDWebApi;
        private System.Windows.Forms.Label ui_aixmMetadataWebApiWarningLabel;
    }
}