namespace AranUpdAgent
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_mainNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ui_mainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_serverTB = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.ui_portNud = new System.Windows.Forms.NumericUpDown();
            this.ui_onlinePictureBox = new System.Windows.Forms.PictureBox();
            this.ui_statusLabel = new System.Windows.Forms.Label();
            this.ui_userNameLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_versionLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            this.ui_mainContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_portNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_onlinePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(35, 132);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(54, 17);
            label1.TabIndex = 0;
            label1.Text = "Server:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(51, 163);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(38, 17);
            label2.TabIndex = 2;
            label2.Text = "Port:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(104, 14);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(52, 17);
            label3.TabIndex = 6;
            label3.Text = "Status:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(73, 39);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(83, 17);
            label5.TabIndex = 8;
            label5.Text = "User Name:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 65);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(150, 17);
            label6.TabIndex = 11;
            label6.Text = "Installed Aran Version:";
            // 
            // ui_mainNotifyIcon
            // 
            this.ui_mainNotifyIcon.ContextMenuStrip = this.ui_mainContextMenu;
            this.ui_mainNotifyIcon.Text = "Aran Updater";
            this.ui_mainNotifyIcon.Visible = true;
            this.ui_mainNotifyIcon.BalloonTipClicked += new System.EventHandler(this.Settings_Click);
            this.ui_mainNotifyIcon.DoubleClick += new System.EventHandler(this.Settings_Click);
            // 
            // ui_mainContextMenu
            // 
            this.ui_mainContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_mainContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.ui_mainContextMenu.Name = "ui_mainContextMenu";
            this.ui_mainContextMenu.Size = new System.Drawing.Size(138, 30);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.Settings_Click);
            // 
            // ui_serverTB
            // 
            this.ui_serverTB.Location = new System.Drawing.Point(97, 129);
            this.ui_serverTB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_serverTB.Name = "ui_serverTB";
            this.ui_serverTB.Size = new System.Drawing.Size(254, 22);
            this.ui_serverTB.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 157);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 28);
            this.button3.TabIndex = 4;
            this.button3.Text = "Save Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.SaveSettings_Click);
            // 
            // ui_portNud
            // 
            this.ui_portNud.Location = new System.Drawing.Point(97, 161);
            this.ui_portNud.Margin = new System.Windows.Forms.Padding(4);
            this.ui_portNud.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.ui_portNud.Name = "ui_portNud";
            this.ui_portNud.Size = new System.Drawing.Size(123, 22);
            this.ui_portNud.TabIndex = 3;
            this.ui_portNud.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ui_onlinePictureBox
            // 
            this.ui_onlinePictureBox.Location = new System.Drawing.Point(361, 23);
            this.ui_onlinePictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.ui_onlinePictureBox.Name = "ui_onlinePictureBox";
            this.ui_onlinePictureBox.Size = new System.Drawing.Size(64, 59);
            this.ui_onlinePictureBox.TabIndex = 5;
            this.ui_onlinePictureBox.TabStop = false;
            // 
            // ui_statusLabel
            // 
            this.ui_statusLabel.AutoSize = true;
            this.ui_statusLabel.Location = new System.Drawing.Point(164, 14);
            this.ui_statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ui_statusLabel.Name = "ui_statusLabel";
            this.ui_statusLabel.Size = new System.Drawing.Size(92, 17);
            this.ui_statusLabel.TabIndex = 7;
            this.ui_statusLabel.Text = "<Connected>";
            // 
            // ui_userNameLabel
            // 
            this.ui_userNameLabel.AutoSize = true;
            this.ui_userNameLabel.Location = new System.Drawing.Point(164, 39);
            this.ui_userNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ui_userNameLabel.Name = "ui_userNameLabel";
            this.ui_userNameLabel.Size = new System.Drawing.Size(95, 17);
            this.ui_userNameLabel.TabIndex = 9;
            this.ui_userNameLabel.Text = "<User Name>";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Location = new System.Drawing.Point(27, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 1);
            this.panel1.TabIndex = 10;
            // 
            // ui_versionLabel
            // 
            this.ui_versionLabel.AutoSize = true;
            this.ui_versionLabel.Location = new System.Drawing.Point(164, 65);
            this.ui_versionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ui_versionLabel.Name = "ui_versionLabel";
            this.ui_versionLabel.Size = new System.Drawing.Size(72, 17);
            this.ui_versionLabel.TabIndex = 12;
            this.ui_versionLabel.Text = "<Version>";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 212);
            this.Controls.Add(this.ui_versionLabel);
            this.Controls.Add(label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ui_userNameLabel);
            this.Controls.Add(label5);
            this.Controls.Add(this.ui_statusLabel);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_onlinePictureBox);
            this.Controls.Add(this.ui_portNud);
            this.Controls.Add(label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ui_serverTB);
            this.Controls.Add(label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aran Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ui_mainContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_portNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_onlinePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ui_mainNotifyIcon;
        private System.Windows.Forms.ContextMenuStrip ui_mainContextMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TextBox ui_serverTB;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown ui_portNud;
        private System.Windows.Forms.PictureBox ui_onlinePictureBox;
        private System.Windows.Forms.Label ui_statusLabel;
        private System.Windows.Forms.Label ui_userNameLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ui_versionLabel;
    }
}

