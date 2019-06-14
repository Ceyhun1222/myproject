namespace AranUpdateManager
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
            System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_navbarPanel = new System.Windows.Forms.Panel();
            this.ui_menuButton = new System.Windows.Forms.Button();
            this.ui_usersNI = new AranUpdateManager.NavbarItem();
            this.ui_versionsNI = new AranUpdateManager.NavbarItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ui_versionsPage = new AranUpdateManager.VersionsPageControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ui_usersPage = new AranUpdateManager.UsersPageControl();
            this.ui_mainPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ui_pageStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ui_mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_navbarPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ui_mainPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ui_mainContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(1451, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsMenu_Click);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenu_Click);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutMenu_Click);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(134, 6);
            // 
            // ui_navbarPanel
            // 
            this.ui_navbarPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_navbarPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ui_navbarPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_navbarPanel.Controls.Add(this.ui_menuButton);
            this.ui_navbarPanel.Controls.Add(this.ui_usersNI);
            this.ui_navbarPanel.Controls.Add(this.ui_versionsNI);
            this.ui_navbarPanel.Location = new System.Drawing.Point(-3, -2);
            this.ui_navbarPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_navbarPanel.Name = "ui_navbarPanel";
            this.ui_navbarPanel.Size = new System.Drawing.Size(1478, 75);
            this.ui_navbarPanel.TabIndex = 0;
            // 
            // ui_menuButton
            // 
            this.ui_menuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_menuButton.FlatAppearance.BorderSize = 0;
            this.ui_menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_menuButton.Image = global::AranUpdateManager.Properties.Resources.menu_32;
            this.ui_menuButton.Location = new System.Drawing.Point(1401, 12);
            this.ui_menuButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_menuButton.Name = "ui_menuButton";
            this.ui_menuButton.Size = new System.Drawing.Size(47, 43);
            this.ui_menuButton.TabIndex = 3;
            this.toolTip1.SetToolTip(this.ui_menuButton, "Menu");
            this.ui_menuButton.UseVisualStyleBackColor = true;
            this.ui_menuButton.Click += new System.EventHandler(this.MainMenu_Click);
            // 
            // ui_usersNI
            // 
            this.ui_usersNI.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_usersNI.BaseControlName = "ui_usersPage";
            this.ui_usersNI.FlatAppearance.BorderSize = 0;
            this.ui_usersNI.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Highlight;
            this.ui_usersNI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_usersNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_usersNI.Location = new System.Drawing.Point(172, 16);
            this.ui_usersNI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_usersNI.Name = "ui_usersNI";
            this.ui_usersNI.Size = new System.Drawing.Size(147, 39);
            this.ui_usersNI.TabIndex = 2;
            this.ui_usersNI.Text = "Users";
            this.ui_usersNI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ui_usersNI.UseVisualStyleBackColor = true;
            this.ui_usersNI.CheckedChanged += new System.EventHandler(this.NavbarItem_CheckedChanged);
            // 
            // ui_versionsNI
            // 
            this.ui_versionsNI.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_versionsNI.BaseControlName = "ui_versionsPage";
            this.ui_versionsNI.Checked = true;
            this.ui_versionsNI.FlatAppearance.BorderSize = 0;
            this.ui_versionsNI.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.Highlight;
            this.ui_versionsNI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_versionsNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ui_versionsNI.ForeColor = System.Drawing.SystemColors.Window;
            this.ui_versionsNI.Location = new System.Drawing.Point(17, 16);
            this.ui_versionsNI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_versionsNI.Name = "ui_versionsNI";
            this.ui_versionsNI.Size = new System.Drawing.Size(147, 39);
            this.ui_versionsNI.TabIndex = 1;
            this.ui_versionsNI.TabStop = true;
            this.ui_versionsNI.Text = "Versions";
            this.ui_versionsNI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ui_versionsNI.UseVisualStyleBackColor = true;
            this.ui_versionsNI.CheckedChanged += new System.EventHandler(this.NavbarItem_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1463, 713);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ui_versionsPage);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(1455, 684);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Versions";
            // 
            // ui_versionsPage
            // 
            this.ui_versionsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_versionsPage.Location = new System.Drawing.Point(4, 4);
            this.ui_versionsPage.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ui_versionsPage.Name = "ui_versionsPage";
            this.ui_versionsPage.Size = new System.Drawing.Size(1447, 676);
            this.ui_versionsPage.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ui_usersPage);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(1455, 684);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Users";
            // 
            // ui_usersPage
            // 
            this.ui_usersPage.BackColor = System.Drawing.SystemColors.Window;
            this.ui_usersPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_usersPage.Location = new System.Drawing.Point(4, 4);
            this.ui_usersPage.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ui_usersPage.Name = "ui_usersPage";
            this.ui_usersPage.Size = new System.Drawing.Size(1447, 676);
            this.ui_usersPage.StatusText = null;
            this.ui_usersPage.TabIndex = 0;
            // 
            // ui_mainPanel
            // 
            this.ui_mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainPanel.Controls.Add(this.tabControl1);
            this.ui_mainPanel.Location = new System.Drawing.Point(4, 74);
            this.ui_mainPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_mainPanel.Name = "ui_mainPanel";
            this.ui_mainPanel.Size = new System.Drawing.Size(1463, 713);
            this.ui_mainPanel.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel1,
            this.ui_pageStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 795);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1471, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ui_pageStatusLabel
            // 
            this.ui_pageStatusLabel.Name = "ui_pageStatusLabel";
            this.ui_pageStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ui_mainContextMenuStrip
            // 
            this.ui_mainContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            settingsToolStripMenuItem,
            aboutToolStripMenuItem,
            toolStripMenuItem1,
            exitToolStripMenuItem});
            this.ui_mainContextMenuStrip.Name = "ui_mainContextMenuStrip";
            this.ui_mainContextMenuStrip.Size = new System.Drawing.Size(138, 88);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1471, 817);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ui_mainPanel);
            this.Controls.Add(this.ui_navbarPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Aran Update Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ui_navbarPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ui_mainPanel.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ui_mainContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ui_navbarPanel;
        private NavbarItem ui_versionsNI;
        private NavbarItem ui_usersNI;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel ui_mainPanel;
        private VersionsPageControl ui_versionsPage;
        private UsersPageControl ui_usersPage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ui_pageStatusLabel;
        private System.Windows.Forms.Button ui_menuButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip ui_mainContextMenuStrip;

    }
}

