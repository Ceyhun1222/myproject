namespace Aran.Controls
{
    partial class DbProviderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.ui_dbsPanel = new System.Windows.Forms.Panel();
            this.ui_tdbRadioButton = new System.Windows.Forms.RadioButton();
            this.ui_comsoftDbRadioButton = new System.Windows.Forms.RadioButton();
            this.ui_xmlDbRadioButton = new System.Windows.Forms.RadioButton();
            this.ui_aranDbRadioButton = new System.Windows.Forms.RadioButton();
            this.ui_dbItemsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_serverPanel = new System.Windows.Forms.Panel();
            this.ui_serverLabel = new System.Windows.Forms.Label();
            this.ui_serverTB = new System.Windows.Forms.TextBox();
            this.ui_portPanel = new System.Windows.Forms.Panel();
            this.ui_portTB = new System.Windows.Forms.TextBox();
            this.ui_portLabel = new System.Windows.Forms.Label();
            this.ui_userNamePswPanel = new System.Windows.Forms.Panel();
            this.chckBxShowPassword = new System.Windows.Forms.CheckBox();
            this.ui_passwordLabel = new System.Windows.Forms.Label();
            this.ui_userNameTB = new System.Windows.Forms.TextBox();
            this.ui_userNameLabel = new System.Windows.Forms.Label();
            this.ui_passwordTB = new System.Windows.Forms.TextBox();
            this.ui_dbNamePanel = new System.Windows.Forms.Panel();
            this.ui_dbNameLabel = new System.Windows.Forms.Label();
            this.ui_dbNameTB = new System.Windows.Forms.TextBox();
            this.ui_xmlFileNamePanel = new System.Windows.Forms.Panel();
            this.ui_setXmlButton = new System.Windows.Forms.Button();
            this.ui_xmlFileNameLabel = new System.Windows.Forms.Label();
            this.ui_xmlFileNameTB = new System.Windows.Forms.TextBox();
            this.ui_useCachePanel = new System.Windows.Forms.Panel();
            this.ui_useCacheCheckBox = new System.Windows.Forms.CheckBox();
            this.ui_useCacheLabel = new System.Windows.Forms.Label();
            this.ui_dbsPanel.SuspendLayout();
            this.ui_dbItemsFlowLayoutPanel.SuspendLayout();
            this.ui_serverPanel.SuspendLayout();
            this.ui_portPanel.SuspendLayout();
            this.ui_userNamePswPanel.SuspendLayout();
            this.ui_dbNamePanel.SuspendLayout();
            this.ui_xmlFileNamePanel.SuspendLayout();
            this.ui_useCachePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_dbsPanel
            // 
            this.ui_dbsPanel.Controls.Add(this.ui_tdbRadioButton);
            this.ui_dbsPanel.Controls.Add(this.ui_comsoftDbRadioButton);
            this.ui_dbsPanel.Controls.Add(this.ui_xmlDbRadioButton);
            this.ui_dbsPanel.Controls.Add(this.ui_aranDbRadioButton);
            this.ui_dbsPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_dbsPanel.Name = "ui_dbsPanel";
            this.ui_dbsPanel.Size = new System.Drawing.Size(309, 44);
            this.ui_dbsPanel.TabIndex = 2;
            // 
            // ui_tdbRadioButton
            // 
            this.ui_tdbRadioButton.AutoSize = true;
            this.ui_tdbRadioButton.Location = new System.Drawing.Point(213, 10);
            this.ui_tdbRadioButton.Name = "ui_tdbRadioButton";
            this.ui_tdbRadioButton.Size = new System.Drawing.Size(47, 17);
            this.ui_tdbRadioButton.TabIndex = 3;
            this.ui_tdbRadioButton.Tag = "";
            this.ui_tdbRadioButton.Text = "TDB";
            this.ui_tdbRadioButton.UseVisualStyleBackColor = true;
            this.ui_tdbRadioButton.CheckedChanged += new System.EventHandler(this.DbTypeRadioButton_CheckedChanged);
            // 
            // ui_comsoftDbRadioButton
            // 
            this.ui_comsoftDbRadioButton.AutoSize = true;
            this.ui_comsoftDbRadioButton.Location = new System.Drawing.Point(70, 10);
            this.ui_comsoftDbRadioButton.Name = "ui_comsoftDbRadioButton";
            this.ui_comsoftDbRadioButton.Size = new System.Drawing.Size(65, 17);
            this.ui_comsoftDbRadioButton.TabIndex = 1;
            this.ui_comsoftDbRadioButton.Tag = "";
            this.ui_comsoftDbRadioButton.Text = "ComSoft";
            this.ui_comsoftDbRadioButton.UseVisualStyleBackColor = true;
            this.ui_comsoftDbRadioButton.CheckedChanged += new System.EventHandler(this.DbTypeRadioButton_CheckedChanged);
            // 
            // ui_xmlDbRadioButton
            // 
            this.ui_xmlDbRadioButton.AutoSize = true;
            this.ui_xmlDbRadioButton.Location = new System.Drawing.Point(141, 10);
            this.ui_xmlDbRadioButton.Name = "ui_xmlDbRadioButton";
            this.ui_xmlDbRadioButton.Size = new System.Drawing.Size(66, 17);
            this.ui_xmlDbRadioButton.TabIndex = 2;
            this.ui_xmlDbRadioButton.Tag = "";
            this.ui_xmlDbRadioButton.Text = "XML File";
            this.ui_xmlDbRadioButton.UseVisualStyleBackColor = true;
            this.ui_xmlDbRadioButton.CheckedChanged += new System.EventHandler(this.DbTypeRadioButton_CheckedChanged);
            // 
            // ui_aranDbRadioButton
            // 
            this.ui_aranDbRadioButton.AutoSize = true;
            this.ui_aranDbRadioButton.Checked = true;
            this.ui_aranDbRadioButton.Location = new System.Drawing.Point(9, 10);
            this.ui_aranDbRadioButton.Name = "ui_aranDbRadioButton";
            this.ui_aranDbRadioButton.Size = new System.Drawing.Size(55, 17);
            this.ui_aranDbRadioButton.TabIndex = 0;
            this.ui_aranDbRadioButton.TabStop = true;
            this.ui_aranDbRadioButton.Tag = "";
            this.ui_aranDbRadioButton.Text = "ARAN";
            this.ui_aranDbRadioButton.UseVisualStyleBackColor = true;
            this.ui_aranDbRadioButton.CheckedChanged += new System.EventHandler(this.DbTypeRadioButton_CheckedChanged);
            // 
            // ui_dbItemsFlowLayoutPanel
            // 
            this.ui_dbItemsFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_dbsPanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_serverPanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_portPanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_userNamePswPanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_dbNamePanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_xmlFileNamePanel);
            this.ui_dbItemsFlowLayoutPanel.Controls.Add(this.ui_useCachePanel);
            this.ui_dbItemsFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_dbItemsFlowLayoutPanel.Name = "ui_dbItemsFlowLayoutPanel";
            this.ui_dbItemsFlowLayoutPanel.Size = new System.Drawing.Size(396, 414);
            this.ui_dbItemsFlowLayoutPanel.TabIndex = 3;
            // 
            // ui_serverPanel
            // 
            this.ui_serverPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_serverPanel.Controls.Add(this.ui_serverLabel);
            this.ui_serverPanel.Controls.Add(this.ui_serverTB);
            this.ui_serverPanel.Location = new System.Drawing.Point(3, 53);
            this.ui_serverPanel.Name = "ui_serverPanel";
            this.ui_serverPanel.Size = new System.Drawing.Size(383, 26);
            this.ui_serverPanel.TabIndex = 0;
            // 
            // ui_serverLabel
            // 
            this.ui_serverLabel.AutoSize = true;
            this.ui_serverLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_serverLabel.Name = "ui_serverLabel";
            this.ui_serverLabel.Size = new System.Drawing.Size(41, 13);
            this.ui_serverLabel.TabIndex = 0;
            this.ui_serverLabel.Text = "Server:";
            // 
            // ui_serverTB
            // 
            this.ui_serverTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_serverTB.Location = new System.Drawing.Point(90, 3);
            this.ui_serverTB.Name = "ui_serverTB";
            this.ui_serverTB.Size = new System.Drawing.Size(278, 20);
            this.ui_serverTB.TabIndex = 1;
            this.ui_serverTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_portPanel
            // 
            this.ui_portPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_portPanel.Controls.Add(this.ui_portTB);
            this.ui_portPanel.Controls.Add(this.ui_portLabel);
            this.ui_portPanel.Location = new System.Drawing.Point(3, 85);
            this.ui_portPanel.Name = "ui_portPanel";
            this.ui_portPanel.Size = new System.Drawing.Size(383, 26);
            this.ui_portPanel.TabIndex = 1;
            // 
            // ui_portTB
            // 
            this.ui_portTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_portTB.Location = new System.Drawing.Point(90, 3);
            this.ui_portTB.Name = "ui_portTB";
            this.ui_portTB.Size = new System.Drawing.Size(278, 20);
            this.ui_portTB.TabIndex = 1;
            this.ui_portTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_portLabel
            // 
            this.ui_portLabel.AutoSize = true;
            this.ui_portLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_portLabel.Name = "ui_portLabel";
            this.ui_portLabel.Size = new System.Drawing.Size(29, 13);
            this.ui_portLabel.TabIndex = 0;
            this.ui_portLabel.Text = "Port:";
            // 
            // ui_userNamePswPanel
            // 
            this.ui_userNamePswPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_userNamePswPanel.Controls.Add(this.chckBxShowPassword);
            this.ui_userNamePswPanel.Controls.Add(this.ui_passwordLabel);
            this.ui_userNamePswPanel.Controls.Add(this.ui_userNameTB);
            this.ui_userNamePswPanel.Controls.Add(this.ui_userNameLabel);
            this.ui_userNamePswPanel.Controls.Add(this.ui_passwordTB);
            this.ui_userNamePswPanel.Location = new System.Drawing.Point(3, 117);
            this.ui_userNamePswPanel.Name = "ui_userNamePswPanel";
            this.ui_userNamePswPanel.Size = new System.Drawing.Size(383, 78);
            this.ui_userNamePswPanel.TabIndex = 2;
            this.ui_userNamePswPanel.Visible = false;
            // 
            // chckBxShowPassword
            // 
            this.chckBxShowPassword.AutoSize = true;
            this.chckBxShowPassword.Location = new System.Drawing.Point(90, 58);
            this.chckBxShowPassword.Name = "chckBxShowPassword";
            this.chckBxShowPassword.Size = new System.Drawing.Size(102, 17);
            this.chckBxShowPassword.TabIndex = 4;
            this.chckBxShowPassword.Text = "Show Password";
            this.chckBxShowPassword.UseVisualStyleBackColor = true;
            this.chckBxShowPassword.CheckedChanged += new System.EventHandler(this.chckBxShowPassword_CheckedChanged);
            // 
            // ui_passwordLabel
            // 
            this.ui_passwordLabel.AutoSize = true;
            this.ui_passwordLabel.Location = new System.Drawing.Point(6, 35);
            this.ui_passwordLabel.Name = "ui_passwordLabel";
            this.ui_passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.ui_passwordLabel.TabIndex = 2;
            this.ui_passwordLabel.Text = "Password:";
            // 
            // ui_userNameTB
            // 
            this.ui_userNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_userNameTB.Location = new System.Drawing.Point(90, 3);
            this.ui_userNameTB.Name = "ui_userNameTB";
            this.ui_userNameTB.Size = new System.Drawing.Size(278, 20);
            this.ui_userNameTB.TabIndex = 1;
            this.ui_userNameTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_userNameLabel
            // 
            this.ui_userNameLabel.AutoSize = true;
            this.ui_userNameLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_userNameLabel.Name = "ui_userNameLabel";
            this.ui_userNameLabel.Size = new System.Drawing.Size(63, 13);
            this.ui_userNameLabel.TabIndex = 0;
            this.ui_userNameLabel.Text = "User Name:";
            // 
            // ui_passwordTB
            // 
            this.ui_passwordTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_passwordTB.Location = new System.Drawing.Point(90, 32);
            this.ui_passwordTB.Name = "ui_passwordTB";
            this.ui_passwordTB.Size = new System.Drawing.Size(278, 20);
            this.ui_passwordTB.TabIndex = 3;
            this.ui_passwordTB.UseSystemPasswordChar = true;
            this.ui_passwordTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_dbNamePanel
            // 
            this.ui_dbNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dbNamePanel.Controls.Add(this.ui_dbNameLabel);
            this.ui_dbNamePanel.Controls.Add(this.ui_dbNameTB);
            this.ui_dbNamePanel.Location = new System.Drawing.Point(3, 201);
            this.ui_dbNamePanel.Name = "ui_dbNamePanel";
            this.ui_dbNamePanel.Size = new System.Drawing.Size(383, 26);
            this.ui_dbNamePanel.TabIndex = 3;
            // 
            // ui_dbNameLabel
            // 
            this.ui_dbNameLabel.AutoSize = true;
            this.ui_dbNameLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_dbNameLabel.Name = "ui_dbNameLabel";
            this.ui_dbNameLabel.Size = new System.Drawing.Size(56, 13);
            this.ui_dbNameLabel.TabIndex = 0;
            this.ui_dbNameLabel.Text = "DB Name:";
            // 
            // ui_dbNameTB
            // 
            this.ui_dbNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dbNameTB.Location = new System.Drawing.Point(90, 3);
            this.ui_dbNameTB.Name = "ui_dbNameTB";
            this.ui_dbNameTB.Size = new System.Drawing.Size(278, 20);
            this.ui_dbNameTB.TabIndex = 1;
            this.ui_dbNameTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_xmlFileNamePanel
            // 
            this.ui_xmlFileNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_xmlFileNamePanel.Controls.Add(this.ui_setXmlButton);
            this.ui_xmlFileNamePanel.Controls.Add(this.ui_xmlFileNameLabel);
            this.ui_xmlFileNamePanel.Controls.Add(this.ui_xmlFileNameTB);
            this.ui_xmlFileNamePanel.Location = new System.Drawing.Point(3, 233);
            this.ui_xmlFileNamePanel.Name = "ui_xmlFileNamePanel";
            this.ui_xmlFileNamePanel.Size = new System.Drawing.Size(383, 26);
            this.ui_xmlFileNamePanel.TabIndex = 4;
            this.ui_xmlFileNamePanel.Visible = false;
            // 
            // ui_setXmlButton
            // 
            this.ui_setXmlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_setXmlButton.AutoSize = true;
            this.ui_setXmlButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_setXmlButton.Location = new System.Drawing.Point(343, 0);
            this.ui_setXmlButton.Name = "ui_setXmlButton";
            this.ui_setXmlButton.Size = new System.Drawing.Size(26, 23);
            this.ui_setXmlButton.TabIndex = 2;
            this.ui_setXmlButton.Text = "...";
            this.ui_setXmlButton.UseVisualStyleBackColor = true;
            this.ui_setXmlButton.Click += new System.EventHandler(this.SelectXmlFile_Click);
            // 
            // ui_xmlFileNameLabel
            // 
            this.ui_xmlFileNameLabel.AutoSize = true;
            this.ui_xmlFileNameLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_xmlFileNameLabel.Name = "ui_xmlFileNameLabel";
            this.ui_xmlFileNameLabel.Size = new System.Drawing.Size(51, 13);
            this.ui_xmlFileNameLabel.TabIndex = 0;
            this.ui_xmlFileNameLabel.Text = "XML File:";
            // 
            // ui_xmlFileNameTB
            // 
            this.ui_xmlFileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_xmlFileNameTB.Location = new System.Drawing.Point(90, 3);
            this.ui_xmlFileNameTB.Name = "ui_xmlFileNameTB";
            this.ui_xmlFileNameTB.Size = new System.Drawing.Size(247, 20);
            this.ui_xmlFileNameTB.TabIndex = 1;
            this.ui_xmlFileNameTB.TextChanged += new System.EventHandler(this.ConnectionInfoProps_TextChanged);
            // 
            // ui_useCachePanel
            // 
            this.ui_useCachePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_useCachePanel.Controls.Add(this.ui_useCacheCheckBox);
            this.ui_useCachePanel.Controls.Add(this.ui_useCacheLabel);
            this.ui_useCachePanel.Location = new System.Drawing.Point(3, 265);
            this.ui_useCachePanel.Name = "ui_useCachePanel";
            this.ui_useCachePanel.Size = new System.Drawing.Size(383, 26);
            this.ui_useCachePanel.TabIndex = 5;
            this.ui_useCachePanel.Visible = false;
            // 
            // ui_useCacheCheckBox
            // 
            this.ui_useCacheCheckBox.AutoSize = true;
            this.ui_useCacheCheckBox.Location = new System.Drawing.Point(90, 6);
            this.ui_useCacheCheckBox.Name = "ui_useCacheCheckBox";
            this.ui_useCacheCheckBox.Size = new System.Drawing.Size(15, 14);
            this.ui_useCacheCheckBox.TabIndex = 1;
            this.ui_useCacheCheckBox.UseVisualStyleBackColor = true;
            this.ui_useCacheCheckBox.CheckedChanged += new System.EventHandler(this.UseCache_CheckedChanged);
            // 
            // ui_useCacheLabel
            // 
            this.ui_useCacheLabel.AutoSize = true;
            this.ui_useCacheLabel.Location = new System.Drawing.Point(6, 6);
            this.ui_useCacheLabel.Name = "ui_useCacheLabel";
            this.ui_useCacheLabel.Size = new System.Drawing.Size(63, 13);
            this.ui_useCacheLabel.TabIndex = 0;
            this.ui_useCacheLabel.Text = "Use Cache:";
            // 
            // DbProviderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_dbItemsFlowLayoutPanel);
            this.Name = "DbProviderControl";
            this.Size = new System.Drawing.Size(402, 282);
            this.ui_dbsPanel.ResumeLayout(false);
            this.ui_dbsPanel.PerformLayout();
            this.ui_dbItemsFlowLayoutPanel.ResumeLayout(false);
            this.ui_serverPanel.ResumeLayout(false);
            this.ui_serverPanel.PerformLayout();
            this.ui_portPanel.ResumeLayout(false);
            this.ui_portPanel.PerformLayout();
            this.ui_userNamePswPanel.ResumeLayout(false);
            this.ui_userNamePswPanel.PerformLayout();
            this.ui_dbNamePanel.ResumeLayout(false);
            this.ui_dbNamePanel.PerformLayout();
            this.ui_xmlFileNamePanel.ResumeLayout(false);
            this.ui_xmlFileNamePanel.PerformLayout();
            this.ui_useCachePanel.ResumeLayout(false);
            this.ui_useCachePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ui_dbsPanel;
        private System.Windows.Forms.RadioButton ui_comsoftDbRadioButton;
        private System.Windows.Forms.RadioButton ui_xmlDbRadioButton;
        private System.Windows.Forms.RadioButton ui_aranDbRadioButton;
        private System.Windows.Forms.FlowLayoutPanel ui_dbItemsFlowLayoutPanel;
        private System.Windows.Forms.Panel ui_serverPanel;
        private System.Windows.Forms.Label ui_serverLabel;
        private System.Windows.Forms.TextBox ui_serverTB;
        private System.Windows.Forms.Panel ui_portPanel;
        private System.Windows.Forms.TextBox ui_portTB;
        private System.Windows.Forms.Label ui_portLabel;
        private System.Windows.Forms.Panel ui_userNamePswPanel;
        private System.Windows.Forms.Label ui_passwordLabel;
        private System.Windows.Forms.TextBox ui_userNameTB;
        private System.Windows.Forms.Label ui_userNameLabel;
        private System.Windows.Forms.TextBox ui_passwordTB;
        private System.Windows.Forms.Panel ui_dbNamePanel;
        private System.Windows.Forms.Label ui_dbNameLabel;
        private System.Windows.Forms.TextBox ui_dbNameTB;
        private System.Windows.Forms.Panel ui_xmlFileNamePanel;
        private System.Windows.Forms.Label ui_xmlFileNameLabel;
        private System.Windows.Forms.TextBox ui_xmlFileNameTB;
        private System.Windows.Forms.Panel ui_useCachePanel;
        private System.Windows.Forms.CheckBox ui_useCacheCheckBox;
        private System.Windows.Forms.Label ui_useCacheLabel;
		private System.Windows.Forms.CheckBox chckBxShowPassword;
		private System.Windows.Forms.Button ui_setXmlButton;
		private System.Windows.Forms.RadioButton ui_tdbRadioButton;
    }
}
