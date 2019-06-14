namespace MapEnv
{
    partial class NewProjectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox buttonsGroupBox;
            System.Windows.Forms.Panel ui_dbProContainerPanelMain;
            this.ui_dbProContainerPanelLeft = new System.Windows.Forms.Panel();
            this.ui_dbProChoiceLocalRB = new System.Windows.Forms.RadioButton();
            this.ui_dbProChoiceServerRB = new System.Windows.Forms.RadioButton();
            this.ui_dbProControlLocal = new Aran.Queries.Common.DbProviderControl2();
            this.ui_dbProControlServer = new Aran.Queries.Common.DbProviderControl2();
            this.ui_openFileButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_backButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_finishButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_newProjectButton = new System.Windows.Forms.Button();
            this.ui_titleLabel = new System.Windows.Forms.Label();
            this.ui_pagePanel = new System.Windows.Forms.Panel();
            this.ui_dbProContainerPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_topPanel = new System.Windows.Forms.Panel();
            this.ui_bottomPanel = new System.Windows.Forms.Panel();
            this.ui_newLocalProButton = new System.Windows.Forms.Button();
            this.ui_pluginPage = new MapEnv.NewProPluginPage();
            this.ui_recentPage = new MapEnv.NewProRecentFilesPage();
            this.ui_pluginSettingsPage = new MapEnv.NewProPluginSettinsPage();
            this.ui_fileNamePage = new MapEnv.NewProFileNamePage();
            buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ui_dbProContainerPanelMain = new System.Windows.Forms.Panel();
            this.ui_dbProContainerPanelLeft.SuspendLayout();
            ui_dbProContainerPanelMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.ui_pagePanel.SuspendLayout();
            this.ui_dbProContainerPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ui_topPanel.SuspendLayout();
            this.ui_bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsGroupBox
            // 
            buttonsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            buttonsGroupBox.Location = new System.Drawing.Point(-3, 3);
            buttonsGroupBox.Margin = new System.Windows.Forms.Padding(0);
            buttonsGroupBox.Name = "buttonsGroupBox";
            buttonsGroupBox.Padding = new System.Windows.Forms.Padding(0);
            buttonsGroupBox.Size = new System.Drawing.Size(763, 18);
            buttonsGroupBox.TabIndex = 0;
            buttonsGroupBox.TabStop = false;
            // 
            // ui_dbProContainerPanelLeft
            // 
            this.ui_dbProContainerPanelLeft.BackColor = System.Drawing.Color.LightGray;
            this.ui_dbProContainerPanelLeft.Controls.Add(this.ui_dbProChoiceLocalRB);
            this.ui_dbProContainerPanelLeft.Controls.Add(this.ui_dbProChoiceServerRB);
            this.ui_dbProContainerPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.ui_dbProContainerPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.ui_dbProContainerPanelLeft.Name = "ui_dbProContainerPanelLeft";
            this.ui_dbProContainerPanelLeft.Size = new System.Drawing.Size(128, 141);
            this.ui_dbProContainerPanelLeft.TabIndex = 0;
            this.ui_dbProContainerPanelLeft.Visible = false;
            // 
            // ui_dbProChoiceLocalRB
            // 
            this.ui_dbProChoiceLocalRB.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_dbProChoiceLocalRB.BackColor = System.Drawing.Color.LightGray;
            this.ui_dbProChoiceLocalRB.FlatAppearance.BorderSize = 0;
            this.ui_dbProChoiceLocalRB.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.ui_dbProChoiceLocalRB.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_dbProChoiceLocalRB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_dbProChoiceLocalRB.Location = new System.Drawing.Point(8, 42);
            this.ui_dbProChoiceLocalRB.Name = "ui_dbProChoiceLocalRB";
            this.ui_dbProChoiceLocalRB.Size = new System.Drawing.Size(121, 31);
            this.ui_dbProChoiceLocalRB.TabIndex = 1;
            this.ui_dbProChoiceLocalRB.Text = "Local";
            this.ui_dbProChoiceLocalRB.UseVisualStyleBackColor = false;
            this.ui_dbProChoiceLocalRB.CheckedChanged += new System.EventHandler(this.DbProChoice_CheckedChanged);
            // 
            // ui_dbProChoiceServerRB
            // 
            this.ui_dbProChoiceServerRB.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_dbProChoiceServerRB.BackColor = System.Drawing.Color.LightGray;
            this.ui_dbProChoiceServerRB.Checked = true;
            this.ui_dbProChoiceServerRB.FlatAppearance.BorderSize = 0;
            this.ui_dbProChoiceServerRB.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Control;
            this.ui_dbProChoiceServerRB.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.ui_dbProChoiceServerRB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_dbProChoiceServerRB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_dbProChoiceServerRB.Location = new System.Drawing.Point(8, 5);
            this.ui_dbProChoiceServerRB.Name = "ui_dbProChoiceServerRB";
            this.ui_dbProChoiceServerRB.Size = new System.Drawing.Size(121, 31);
            this.ui_dbProChoiceServerRB.TabIndex = 0;
            this.ui_dbProChoiceServerRB.TabStop = true;
            this.ui_dbProChoiceServerRB.Text = "Server";
            this.ui_dbProChoiceServerRB.UseVisualStyleBackColor = false;
            this.ui_dbProChoiceServerRB.CheckedChanged += new System.EventHandler(this.DbProChoice_CheckedChanged);
            // 
            // ui_dbProContainerPanelMain
            // 
            ui_dbProContainerPanelMain.Controls.Add(this.ui_dbProControlLocal);
            ui_dbProContainerPanelMain.Controls.Add(this.ui_dbProControlServer);
            ui_dbProContainerPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            ui_dbProContainerPanelMain.Location = new System.Drawing.Point(128, 0);
            ui_dbProContainerPanelMain.Name = "ui_dbProContainerPanelMain";
            ui_dbProContainerPanelMain.Size = new System.Drawing.Size(155, 141);
            ui_dbProContainerPanelMain.TabIndex = 1;
            // 
            // ui_dbProControlLocal
            // 
            this.ui_dbProControlLocal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_dbProControlLocal.ErrorMessage = "";
            this.ui_dbProControlLocal.IsPasswordSaved = false;
            this.ui_dbProControlLocal.IsVisibleSavePasswordBox = false;
            this.ui_dbProControlLocal.Location = new System.Drawing.Point(20, 66);
            this.ui_dbProControlLocal.LoginView = false;
            this.ui_dbProControlLocal.Margin = new System.Windows.Forms.Padding(4);
            this.ui_dbProControlLocal.Name = "ui_dbProControlLocal";
            this.ui_dbProControlLocal.Size = new System.Drawing.Size(56, 42);
            this.ui_dbProControlLocal.TabIndex = 15;
            this.ui_dbProControlLocal.Visible = false;
            // 
            // ui_dbProControlServer
            // 
            this.ui_dbProControlServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_dbProControlServer.ErrorMessage = "";
            this.ui_dbProControlServer.IsPasswordSaved = false;
            this.ui_dbProControlServer.IsVisibleSavePasswordBox = false;
            this.ui_dbProControlServer.Location = new System.Drawing.Point(20, 16);
            this.ui_dbProControlServer.LoginView = false;
            this.ui_dbProControlServer.Margin = new System.Windows.Forms.Padding(4);
            this.ui_dbProControlServer.Name = "ui_dbProControlServer";
            this.ui_dbProControlServer.Size = new System.Drawing.Size(56, 42);
            this.ui_dbProControlServer.TabIndex = 14;
            this.ui_dbProControlServer.LoadedDbList += new Aran.Queries.Common.DbProControlLoadDbListEventHandler(this.dbProviderControl21_LoadedDbList);
            // 
            // ui_openFileButton
            // 
            this.ui_openFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_openFileButton.Location = new System.Drawing.Point(4, 22);
            this.ui_openFileButton.Name = "ui_openFileButton";
            this.ui_openFileButton.Size = new System.Drawing.Size(125, 23);
            this.ui_openFileButton.TabIndex = 5;
            this.ui_openFileButton.Text = "Open Another File...";
            this.ui_openFileButton.UseVisualStyleBackColor = true;
            this.ui_openFileButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.ui_backButton);
            this.flowLayoutPanel1.Controls.Add(this.ui_nextButton);
            this.flowLayoutPanel1.Controls.Add(this.ui_finishButton);
            this.flowLayoutPanel1.Controls.Add(this.ui_okButton);
            this.flowLayoutPanel1.Controls.Add(this.ui_cancelButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(345, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(405, 29);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // ui_backButton
            // 
            this.ui_backButton.Location = new System.Drawing.Point(3, 3);
            this.ui_backButton.Name = "ui_backButton";
            this.ui_backButton.Size = new System.Drawing.Size(75, 23);
            this.ui_backButton.TabIndex = 2;
            this.ui_backButton.Text = "< Back";
            this.ui_backButton.UseVisualStyleBackColor = true;
            this.ui_backButton.Visible = false;
            this.ui_backButton.Click += new System.EventHandler(this.Back_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Location = new System.Drawing.Point(84, 3);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(75, 23);
            this.ui_nextButton.TabIndex = 1;
            this.ui_nextButton.Text = "Next >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Visible = false;
            this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_finishButton
            // 
            this.ui_finishButton.Location = new System.Drawing.Point(165, 3);
            this.ui_finishButton.Name = "ui_finishButton";
            this.ui_finishButton.Size = new System.Drawing.Size(75, 23);
            this.ui_finishButton.TabIndex = 4;
            this.ui_finishButton.Text = "Finish";
            this.ui_finishButton.UseVisualStyleBackColor = true;
            this.ui_finishButton.Visible = false;
            this.ui_finishButton.Click += new System.EventHandler(this.Finish_Click);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(246, 3);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 3;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Location = new System.Drawing.Point(327, 3);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 0;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_newProjectButton
            // 
            this.ui_newProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_newProjectButton.Location = new System.Drawing.Point(134, 22);
            this.ui_newProjectButton.Name = "ui_newProjectButton";
            this.ui_newProjectButton.Size = new System.Drawing.Size(89, 23);
            this.ui_newProjectButton.TabIndex = 4;
            this.ui_newProjectButton.Text = "New Project >";
            this.ui_newProjectButton.UseVisualStyleBackColor = true;
            this.ui_newProjectButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_titleLabel
            // 
            this.ui_titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_titleLabel.BackColor = System.Drawing.Color.White;
            this.ui_titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_titleLabel.Location = new System.Drawing.Point(-1, 0);
            this.ui_titleLabel.Name = "ui_titleLabel";
            this.ui_titleLabel.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.ui_titleLabel.Size = new System.Drawing.Size(758, 37);
            this.ui_titleLabel.TabIndex = 3;
            this.ui_titleLabel.Text = "Existing Projects:";
            this.ui_titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_pagePanel
            // 
            this.ui_pagePanel.Controls.Add(this.ui_dbProContainerPanel);
            this.ui_pagePanel.Controls.Add(this.ui_pluginPage);
            this.ui_pagePanel.Controls.Add(this.ui_recentPage);
            this.ui_pagePanel.Controls.Add(this.ui_pluginSettingsPage);
            this.ui_pagePanel.Controls.Add(this.ui_fileNamePage);
            this.ui_pagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_pagePanel.Location = new System.Drawing.Point(0, 41);
            this.ui_pagePanel.Name = "ui_pagePanel";
            this.ui_pagePanel.Size = new System.Drawing.Size(753, 274);
            this.ui_pagePanel.TabIndex = 6;
            // 
            // ui_dbProContainerPanel
            // 
            this.ui_dbProContainerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_dbProContainerPanel.Controls.Add(ui_dbProContainerPanelMain);
            this.ui_dbProContainerPanel.Controls.Add(this.ui_dbProContainerPanelLeft);
            this.ui_dbProContainerPanel.Location = new System.Drawing.Point(205, 13);
            this.ui_dbProContainerPanel.Name = "ui_dbProContainerPanel";
            this.ui_dbProContainerPanel.Size = new System.Drawing.Size(285, 143);
            this.ui_dbProContainerPanel.TabIndex = 16;
            this.ui_dbProContainerPanel.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(buttonsGroupBox);
            this.panel1.Location = new System.Drawing.Point(-5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(758, 11);
            this.panel1.TabIndex = 7;
            // 
            // ui_topPanel
            // 
            this.ui_topPanel.Controls.Add(this.ui_titleLabel);
            this.ui_topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_topPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_topPanel.Name = "ui_topPanel";
            this.ui_topPanel.Size = new System.Drawing.Size(753, 41);
            this.ui_topPanel.TabIndex = 8;
            // 
            // ui_bottomPanel
            // 
            this.ui_bottomPanel.Controls.Add(this.ui_newLocalProButton);
            this.ui_bottomPanel.Controls.Add(this.panel1);
            this.ui_bottomPanel.Controls.Add(this.ui_openFileButton);
            this.ui_bottomPanel.Controls.Add(this.ui_newProjectButton);
            this.ui_bottomPanel.Controls.Add(this.flowLayoutPanel1);
            this.ui_bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ui_bottomPanel.Location = new System.Drawing.Point(0, 315);
            this.ui_bottomPanel.Name = "ui_bottomPanel";
            this.ui_bottomPanel.Size = new System.Drawing.Size(753, 56);
            this.ui_bottomPanel.TabIndex = 10;
            // 
            // ui_newLocalProButton
            // 
            this.ui_newLocalProButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_newLocalProButton.Location = new System.Drawing.Point(227, 22);
            this.ui_newLocalProButton.Name = "ui_newLocalProButton";
            this.ui_newLocalProButton.Size = new System.Drawing.Size(111, 23);
            this.ui_newLocalProButton.TabIndex = 8;
            this.ui_newLocalProButton.Text = "New Local Project >";
            this.ui_newLocalProButton.UseVisualStyleBackColor = true;
            this.ui_newLocalProButton.Visible = false;
            this.ui_newLocalProButton.Click += new System.EventHandler(this.NewLocalProject_Click);
            // 
            // ui_pluginPage
            // 
            this.ui_pluginPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_pluginPage.Location = new System.Drawing.Point(538, 28);
            this.ui_pluginPage.Margin = new System.Windows.Forms.Padding(4);
            this.ui_pluginPage.Name = "ui_pluginPage";
            this.ui_pluginPage.Size = new System.Drawing.Size(211, 221);
            this.ui_pluginPage.TabIndex = 15;
            this.ui_pluginPage.Visible = false;
            // 
            // ui_recentPage
            // 
            this.ui_recentPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_recentPage.Location = new System.Drawing.Point(9, 13);
            this.ui_recentPage.Margin = new System.Windows.Forms.Padding(4);
            this.ui_recentPage.Name = "ui_recentPage";
            this.ui_recentPage.Size = new System.Drawing.Size(189, 225);
            this.ui_recentPage.TabIndex = 13;
            this.ui_recentPage.SelectedChanged += new System.EventHandler(this.RecentFiles_SelectedFileNameChanged);
            // 
            // ui_pluginSettingsPage
            // 
            this.ui_pluginSettingsPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_pluginSettingsPage.Location = new System.Drawing.Point(418, 9);
            this.ui_pluginSettingsPage.Margin = new System.Windows.Forms.Padding(4);
            this.ui_pluginSettingsPage.Name = "ui_pluginSettingsPage";
            this.ui_pluginSettingsPage.Size = new System.Drawing.Size(150, 123);
            this.ui_pluginSettingsPage.TabIndex = 10;
            this.ui_pluginSettingsPage.Visible = false;
            // 
            // ui_fileNamePage
            // 
            this.ui_fileNamePage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_fileNamePage.ErrorMessage = "";
            this.ui_fileNamePage.Location = new System.Drawing.Point(273, 162);
            this.ui_fileNamePage.Margin = new System.Windows.Forms.Padding(4);
            this.ui_fileNamePage.Name = "ui_fileNamePage";
            this.ui_fileNamePage.Size = new System.Drawing.Size(150, 96);
            this.ui_fileNamePage.TabIndex = 9;
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 371);
            this.Controls.Add(this.ui_pagePanel);
            this.Controls.Add(this.ui_bottomPanel);
            this.Controls.Add(this.ui_topPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create New Project";
            this.ui_dbProContainerPanelLeft.ResumeLayout(false);
            ui_dbProContainerPanelMain.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ui_pagePanel.ResumeLayout(false);
            this.ui_dbProContainerPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ui_topPanel.ResumeLayout(false);
            this.ui_bottomPanel.ResumeLayout(false);
            this.ui_bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_backButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label ui_titleLabel;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_newProjectButton;
        private System.Windows.Forms.Button ui_openFileButton;
        private System.Windows.Forms.Panel ui_pagePanel;
        private System.Windows.Forms.Button ui_finishButton;
        private NewProPluginSettinsPage ui_pluginSettingsPage;
        private NewProFileNamePage ui_fileNamePage;
        private NewProRecentFilesPage ui_recentPage;
        private System.Windows.Forms.Panel panel1;
        private Aran.Queries.Common.DbProviderControl2 ui_dbProControlServer;
        private System.Windows.Forms.Panel ui_topPanel;
        private System.Windows.Forms.Panel ui_bottomPanel;
        private System.Windows.Forms.Button ui_newLocalProButton;
        private NewProPluginPage ui_pluginPage;
        private System.Windows.Forms.Panel ui_dbProContainerPanel;
        private Aran.Queries.Common.DbProviderControl2 ui_dbProControlLocal;
        private System.Windows.Forms.RadioButton ui_dbProChoiceLocalRB;
        private System.Windows.Forms.RadioButton ui_dbProChoiceServerRB;
        private System.Windows.Forms.Panel ui_dbProContainerPanelLeft;
    }
}