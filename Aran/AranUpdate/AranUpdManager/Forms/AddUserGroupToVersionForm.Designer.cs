namespace AranUpdateManager
{
    partial class AddUserGroupToVersionForm
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
            System.Windows.Forms.Panel panel1;
            this.label1 = new System.Windows.Forms.Label();
            this.ui_versionTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ui_userGroupCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ui_noteTB = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BackColor = System.Drawing.Color.Gray;
            panel1.Location = new System.Drawing.Point(10, 242);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(427, 1);
            panel1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version:";
            // 
            // ui_versionTB
            // 
            this.ui_versionTB.Location = new System.Drawing.Point(96, 11);
            this.ui_versionTB.Name = "ui_versionTB";
            this.ui_versionTB.ReadOnly = true;
            this.ui_versionTB.Size = new System.Drawing.Size(178, 20);
            this.ui_versionTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Group:";
            // 
            // ui_userGroupCB
            // 
            this.ui_userGroupCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_userGroupCB.FormattingEnabled = true;
            this.ui_userGroupCB.Location = new System.Drawing.Point(96, 37);
            this.ui_userGroupCB.Name = "ui_userGroupCB";
            this.ui_userGroupCB.Size = new System.Drawing.Size(178, 21);
            this.ui_userGroupCB.TabIndex = 3;
            this.ui_userGroupCB.SelectedIndexChanged += new System.EventHandler(this.UserGroup_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Note:";
            // 
            // ui_noteTB
            // 
            this.ui_noteTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_noteTB.Location = new System.Drawing.Point(96, 64);
            this.ui_noteTB.Multiline = true;
            this.ui_noteTB.Name = "ui_noteTB";
            this.ui_noteTB.Size = new System.Drawing.Size(342, 167);
            this.ui_noteTB.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(362, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(281, 253);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 12;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // AddUserGroupToVersionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(446, 288);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(panel1);
            this.Controls.Add(this.ui_noteTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ui_userGroupCB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ui_versionTB);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUserGroupToVersionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set User Group to Version";
            this.Load += new System.EventHandler(this.AddUserGroupToVersionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ui_versionTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ui_userGroupCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ui_noteTB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ui_okButton;
    }
}