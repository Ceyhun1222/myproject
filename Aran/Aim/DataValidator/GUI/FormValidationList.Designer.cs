namespace AIM_Data_Validator
{
	partial class FormValidationList
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ( );
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ( )
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormValidationList));
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsBtnSettings = new System.Windows.Forms.ToolStripButton();
			this.tsLblConnection = new System.Windows.Forms.ToolStripLabel();
			this.tsLblConnect = new System.Windows.Forms.ToolStripLabel();
			this.tbCntrlMain = new System.Windows.Forms.TabControl();
			this.tbPageFeats = new System.Windows.Forms.TabPage();
			this.btnCheck = new System.Windows.Forms.Button();
			this.chckBxListFeats = new AControls.CheckBoxList();
			this.chckBxListProps = new AControls.CheckBoxList();
			this.tbPageSettings = new System.Windows.Forms.TabPage();
			this.tbControlSettings = new System.Windows.Forms.TabControl();
			this.tbPageSetupDb = new System.Windows.Forms.TabPage();
			this.btnSetupDbApply = new System.Windows.Forms.Button();
			this.lbDbSetup = new System.Windows.Forms.Label();
			this.flowLPanelDbItems = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlServer = new System.Windows.Forms.Panel();
			this.lblServer = new System.Windows.Forms.Label();
			this.txtBxServer = new System.Windows.Forms.TextBox();
			this.pnlPort = new System.Windows.Forms.Panel();
			this.rchTxBxPort = new System.Windows.Forms.RichTextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.pnlUserData = new System.Windows.Forms.Panel();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtBxUserName = new System.Windows.Forms.TextBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.txtBxPassword = new System.Windows.Forms.TextBox();
			this.ui_dbNamePanel = new System.Windows.Forms.Panel();
			this.lblDbName = new System.Windows.Forms.Label();
			this.txtBxDbName = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictBxDbSetup = new System.Windows.Forms.PictureBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tSFeatsSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tsMain.SuspendLayout();
			this.tbCntrlMain.SuspendLayout();
			this.tbPageFeats.SuspendLayout();
			this.tbPageSettings.SuspendLayout();
			this.tbControlSettings.SuspendLayout();
			this.tbPageSetupDb.SuspendLayout();
			this.flowLPanelDbItems.SuspendLayout();
			this.pnlServer.SuspendLayout();
			this.pnlPort.SuspendLayout();
			this.pnlUserData.SuspendLayout();
			this.ui_dbNamePanel.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictBxDbSetup)).BeginInit();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnSettings,
            this.tsLblConnection});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.MinimumSize = new System.Drawing.Size(592, 35);
			this.tsMain.Name = "tsMain";
			this.tsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsMain.Size = new System.Drawing.Size(592, 35);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsBtnSettings
			// 
			this.tsBtnSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsBtnSettings.AutoSize = false;
			this.tsBtnSettings.CheckOnClick = true;
			this.tsBtnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsBtnSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnSettings.Image")));
			this.tsBtnSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsBtnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnSettings.Name = "tsBtnSettings";
			this.tsBtnSettings.Size = new System.Drawing.Size(40, 37);
			this.tsBtnSettings.Text = "Settings";
			this.tsBtnSettings.ToolTipText = "Settings";
			this.tsBtnSettings.CheckedChanged += new System.EventHandler(this.tsBtnSettings_CheckedChanged);
			this.tsBtnSettings.Click += new System.EventHandler(this.tsBtnSettings_Click);
			// 
			// tsLblConnection
			// 
			this.tsLblConnection.AutoSize = false;
			this.tsLblConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsLblConnection.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tsLblConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tsLblConnection.Name = "tsLblConnection";
			this.tsLblConnection.Size = new System.Drawing.Size(520, 32);
			this.tsLblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tsLblConnection.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			// 
			// tsLblConnect
			// 
			this.tsLblConnect.Name = "tsLblConnect";
			this.tsLblConnect.Size = new System.Drawing.Size(72, 32);
			this.tsLblConnect.Text = "Connected to";
			// 
			// tbCntrlMain
			// 
			this.tbCntrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCntrlMain.Controls.Add(this.tbPageFeats);
			this.tbCntrlMain.Controls.Add(this.tbPageSettings);
			this.tbCntrlMain.Location = new System.Drawing.Point(-4, 12);
			this.tbCntrlMain.Name = "tbCntrlMain";
			this.tbCntrlMain.SelectedIndex = 0;
			this.tbCntrlMain.Size = new System.Drawing.Size(600, 463);
			this.tbCntrlMain.TabIndex = 1;
			// 
			// tbPageFeats
			// 
			this.tbPageFeats.BackColor = System.Drawing.SystemColors.Control;
			this.tbPageFeats.Controls.Add(this.btnCheck);
			this.tbPageFeats.Controls.Add(this.chckBxListFeats);
			this.tbPageFeats.Controls.Add(this.chckBxListProps);
			this.tbPageFeats.Location = new System.Drawing.Point(4, 22);
			this.tbPageFeats.Name = "tbPageFeats";
			this.tbPageFeats.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageFeats.Size = new System.Drawing.Size(592, 437);
			this.tbPageFeats.TabIndex = 0;
			this.tbPageFeats.Text = "tabPage1";
			// 
			// btnCheck
			// 
			this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCheck.AutoSize = true;
			this.btnCheck.Image = global::Aran.Aim.Data.Validator.Properties.Resources.Actions_dialog_ok_apply_icon;
			this.btnCheck.Location = new System.Drawing.Point(504, 399);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(83, 30);
			this.btnCheck.TabIndex = 5;
			this.btnCheck.Text = "Check";
			this.btnCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnCheck.UseVisualStyleBackColor = true;
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			// 
			// chckBxListFeats
			// 
			this.chckBxListFeats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.chckBxListFeats.Location = new System.Drawing.Point(5, 3);
			this.chckBxListFeats.MinimumSize = new System.Drawing.Size(180, 90);
			this.chckBxListFeats.Name = "chckBxListFeats";
			this.chckBxListFeats.SearchTextBoxVisible = true;
			this.chckBxListFeats.Size = new System.Drawing.Size(205, 379);
			this.chckBxListFeats.TabIndex = 4;
			this.chckBxListFeats.Title = "Items";
			this.chckBxListFeats.SelectedRowChanged += new AControls.SelectedRowChanged(this.chckBxListFeats_SelectedRowChanged);
			this.chckBxListFeats.RowCheckedChanged += new AControls.RowCheckedChanged(this.chckBxListFeats_RowCheckedChanged);
			// 
			// chckBxListProps
			// 
			this.chckBxListProps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chckBxListProps.Location = new System.Drawing.Point(214, 3);
			this.chckBxListProps.MinimumSize = new System.Drawing.Size(150, 50);
			this.chckBxListProps.Name = "chckBxListProps";
			this.chckBxListProps.SearchTextBoxVisible = false;
			this.chckBxListProps.Size = new System.Drawing.Size(378, 379);
			this.chckBxListProps.TabIndex = 1;
			this.chckBxListProps.Title = "Properties";
			this.chckBxListProps.RowCheckedChanged += new AControls.RowCheckedChanged(this.chckBxListProps_RowCheckedChanged);
			// 
			// tbPageSettings
			// 
			this.tbPageSettings.Controls.Add(this.tbControlSettings);
			this.tbPageSettings.Controls.Add(this.panel1);
			this.tbPageSettings.Location = new System.Drawing.Point(4, 22);
			this.tbPageSettings.Name = "tbPageSettings";
			this.tbPageSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageSettings.Size = new System.Drawing.Size(592, 437);
			this.tbPageSettings.TabIndex = 1;
			this.tbPageSettings.Text = "tabPage1";
			this.tbPageSettings.UseVisualStyleBackColor = true;
			// 
			// tbControlSettings
			// 
			this.tbControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbControlSettings.Controls.Add(this.tbPageSetupDb);
			this.tbControlSettings.Location = new System.Drawing.Point(92, -22);
			this.tbControlSettings.Name = "tbControlSettings";
			this.tbControlSettings.SelectedIndex = 0;
			this.tbControlSettings.Size = new System.Drawing.Size(502, 461);
			this.tbControlSettings.TabIndex = 1;
			// 
			// tbPageSetupDb
			// 
			this.tbPageSetupDb.Controls.Add(this.btnSetupDbApply);
			this.tbPageSetupDb.Controls.Add(this.lbDbSetup);
			this.tbPageSetupDb.Controls.Add(this.flowLPanelDbItems);
			this.tbPageSetupDb.Location = new System.Drawing.Point(4, 22);
			this.tbPageSetupDb.Name = "tbPageSetupDb";
			this.tbPageSetupDb.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageSetupDb.Size = new System.Drawing.Size(494, 435);
			this.tbPageSetupDb.TabIndex = 0;
			this.tbPageSetupDb.UseVisualStyleBackColor = true;
			// 
			// btnSetupDbApply
			// 
			this.btnSetupDbApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetupDbApply.AutoSize = true;
			this.btnSetupDbApply.Location = new System.Drawing.Point(416, 397);
			this.btnSetupDbApply.Name = "btnSetupDbApply";
			this.btnSetupDbApply.Size = new System.Drawing.Size(71, 30);
			this.btnSetupDbApply.TabIndex = 5;
			this.btnSetupDbApply.Text = "Apply";
			this.btnSetupDbApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnSetupDbApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnSetupDbApply.UseCompatibleTextRendering = true;
			this.btnSetupDbApply.UseVisualStyleBackColor = true;
			this.btnSetupDbApply.Click += new System.EventHandler(this.btnSetupDbApply_Click);
			// 
			// lbDbSetup
			// 
			this.lbDbSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbDbSetup.AutoSize = true;
			this.lbDbSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lbDbSetup.Location = new System.Drawing.Point(137, 13);
			this.lbDbSetup.Name = "lbDbSetup";
			this.lbDbSetup.Size = new System.Drawing.Size(175, 16);
			this.lbDbSetup.TabIndex = 4;
			this.lbDbSetup.Text = "Set up database connection";
			// 
			// flowLPanelDbItems
			// 
			this.flowLPanelDbItems.Controls.Add(this.pnlServer);
			this.flowLPanelDbItems.Controls.Add(this.pnlPort);
			this.flowLPanelDbItems.Controls.Add(this.pnlUserData);
			this.flowLPanelDbItems.Controls.Add(this.ui_dbNamePanel);
			this.flowLPanelDbItems.Location = new System.Drawing.Point(31, 43);
			this.flowLPanelDbItems.Name = "flowLPanelDbItems";
			this.flowLPanelDbItems.Size = new System.Drawing.Size(397, 161);
			this.flowLPanelDbItems.TabIndex = 3;
			// 
			// pnlServer
			// 
			this.pnlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlServer.Controls.Add(this.lblServer);
			this.pnlServer.Controls.Add(this.txtBxServer);
			this.pnlServer.Location = new System.Drawing.Point(3, 3);
			this.pnlServer.Name = "pnlServer";
			this.pnlServer.Size = new System.Drawing.Size(383, 26);
			this.pnlServer.TabIndex = 0;
			// 
			// lblServer
			// 
			this.lblServer.AutoSize = true;
			this.lblServer.Location = new System.Drawing.Point(6, 6);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(41, 13);
			this.lblServer.TabIndex = 0;
			this.lblServer.Text = "Server:";
			// 
			// txtBxServer
			// 
			this.txtBxServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBxServer.Location = new System.Drawing.Point(90, 3);
			this.txtBxServer.Name = "txtBxServer";
			this.txtBxServer.Size = new System.Drawing.Size(278, 20);
			this.txtBxServer.TabIndex = 1;
			this.txtBxServer.Text = "172.30.31.18";
			// 
			// pnlPort
			// 
			this.pnlPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlPort.Controls.Add(this.rchTxBxPort);
			this.pnlPort.Controls.Add(this.lblPort);
			this.pnlPort.Location = new System.Drawing.Point(3, 35);
			this.pnlPort.Name = "pnlPort";
			this.pnlPort.Size = new System.Drawing.Size(383, 26);
			this.pnlPort.TabIndex = 1;
			// 
			// rchTxBxPort
			// 
			this.rchTxBxPort.Location = new System.Drawing.Point(90, 2);
			this.rchTxBxPort.Name = "rchTxBxPort";
			this.rchTxBxPort.Size = new System.Drawing.Size(278, 20);
			this.rchTxBxPort.TabIndex = 1;
			this.rchTxBxPort.Text = "6400";
			this.rchTxBxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rchTxBxPort_KeyPress);
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(6, 6);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 0;
			this.lblPort.Text = "Port:";
			// 
			// pnlUserData
			// 
			this.pnlUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlUserData.Controls.Add(this.lblPassword);
			this.pnlUserData.Controls.Add(this.txtBxUserName);
			this.pnlUserData.Controls.Add(this.lblUserName);
			this.pnlUserData.Controls.Add(this.txtBxPassword);
			this.pnlUserData.Location = new System.Drawing.Point(3, 67);
			this.pnlUserData.Name = "pnlUserData";
			this.pnlUserData.Size = new System.Drawing.Size(383, 53);
			this.pnlUserData.TabIndex = 2;
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(6, 32);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 2;
			this.lblPassword.Text = "Password:";
			// 
			// txtBxUserName
			// 
			this.txtBxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBxUserName.Location = new System.Drawing.Point(90, 3);
			this.txtBxUserName.Name = "txtBxUserName";
			this.txtBxUserName.Size = new System.Drawing.Size(278, 20);
			this.txtBxUserName.TabIndex = 1;
			this.txtBxUserName.Text = "aran";
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.Location = new System.Drawing.Point(6, 6);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(63, 13);
			this.lblUserName.TabIndex = 0;
			this.lblUserName.Text = "User Name:";
			// 
			// txtBxPassword
			// 
			this.txtBxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBxPassword.Location = new System.Drawing.Point(90, 29);
			this.txtBxPassword.Name = "txtBxPassword";
			this.txtBxPassword.PasswordChar = '•';
			this.txtBxPassword.Size = new System.Drawing.Size(278, 20);
			this.txtBxPassword.TabIndex = 3;
			this.txtBxPassword.Text = "airnav2012";
			// 
			// ui_dbNamePanel
			// 
			this.ui_dbNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dbNamePanel.Controls.Add(this.lblDbName);
			this.ui_dbNamePanel.Controls.Add(this.txtBxDbName);
			this.ui_dbNamePanel.Location = new System.Drawing.Point(3, 126);
			this.ui_dbNamePanel.Name = "ui_dbNamePanel";
			this.ui_dbNamePanel.Size = new System.Drawing.Size(383, 26);
			this.ui_dbNamePanel.TabIndex = 3;
			// 
			// lblDbName
			// 
			this.lblDbName.AutoSize = true;
			this.lblDbName.Location = new System.Drawing.Point(6, 6);
			this.lblDbName.Name = "lblDbName";
			this.lblDbName.Size = new System.Drawing.Size(56, 13);
			this.lblDbName.TabIndex = 0;
			this.lblDbName.Text = "DB Name:";
			// 
			// txtBxDbName
			// 
			this.txtBxDbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBxDbName.Location = new System.Drawing.Point(90, 3);
			this.txtBxDbName.Name = "txtBxDbName";
			this.txtBxDbName.Size = new System.Drawing.Size(278, 20);
			this.txtBxDbName.TabIndex = 1;
			this.txtBxDbName.Text = "Kaz_Ilyas";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.pictBxDbSetup);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(87, 431);
			this.panel1.TabIndex = 0;
			// 
			// pictBxDbSetup
			// 
			this.pictBxDbSetup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictBxDbSetup.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictBxDbSetup.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictBxDbSetup.Location = new System.Drawing.Point(0, 0);
			this.pictBxDbSetup.Name = "pictBxDbSetup";
			this.pictBxDbSetup.Size = new System.Drawing.Size(87, 51);
			this.pictBxDbSetup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictBxDbSetup.TabIndex = 0;
			this.pictBxDbSetup.TabStop = false;
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(78, 21);
			this.toolStripLabel1.Text = "toolStripLabel1";
			// 
			// tSFeatsSeparator
			// 
			this.tSFeatsSeparator.Name = "tSFeatsSeparator";
			this.tSFeatsSeparator.Size = new System.Drawing.Size(6, 24);
			// 
			// FormValidationList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 473);
			this.Controls.Add(this.tsMain);
			this.Controls.Add(this.tbCntrlMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(600, 500);
			this.Name = "FormValidationList";
			this.Text = "Data Validator";
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.tbCntrlMain.ResumeLayout(false);
			this.tbPageFeats.ResumeLayout(false);
			this.tbPageFeats.PerformLayout();
			this.tbPageSettings.ResumeLayout(false);
			this.tbControlSettings.ResumeLayout(false);
			this.tbPageSetupDb.ResumeLayout(false);
			this.tbPageSetupDb.PerformLayout();
			this.flowLPanelDbItems.ResumeLayout(false);
			this.pnlServer.ResumeLayout(false);
			this.pnlServer.PerformLayout();
			this.pnlPort.ResumeLayout(false);
			this.pnlPort.PerformLayout();
			this.pnlUserData.ResumeLayout(false);
			this.pnlUserData.PerformLayout();
			this.ui_dbNamePanel.ResumeLayout(false);
			this.ui_dbNamePanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictBxDbSetup)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private AControls.CheckBoxList chckBxListProps;
		private System.Windows.Forms.ToolStripLabel tsLblConnect;
		private System.Windows.Forms.TabControl tbCntrlMain;
		private System.Windows.Forms.TabPage tbPageFeats;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator tSFeatsSeparator;
		private System.Windows.Forms.TabPage tbPageSettings;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tbControlSettings;
		private System.Windows.Forms.TabPage tbPageSetupDb;
		private System.Windows.Forms.Label lbDbSetup;
		private System.Windows.Forms.FlowLayoutPanel flowLPanelDbItems;
		private System.Windows.Forms.Panel pnlServer;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.TextBox txtBxServer;
		private System.Windows.Forms.Panel pnlPort;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.Panel pnlUserData;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtBxUserName;
		private System.Windows.Forms.Label lblUserName;
		private System.Windows.Forms.TextBox txtBxPassword;
		private System.Windows.Forms.Panel ui_dbNamePanel;
		private System.Windows.Forms.Label lblDbName;
		private System.Windows.Forms.TextBox txtBxDbName;
		private System.Windows.Forms.PictureBox pictBxDbSetup;
		private System.Windows.Forms.Button btnSetupDbApply;
		private System.Windows.Forms.RichTextBox rchTxBxPort;
		private AControls.CheckBoxList chckBxListFeats;
		private System.Windows.Forms.ToolStripButton tsBtnSettings;
		private System.Windows.Forms.ToolStripLabel tsLblConnection;
		private System.Windows.Forms.Button btnCheck;


	}
}