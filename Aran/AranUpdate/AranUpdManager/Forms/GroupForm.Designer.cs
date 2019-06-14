namespace AranUpdateManager
{
    partial class UserGroupForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label4;
            this.ui_nameTB = new System.Windows.Forms.TextBox();
            this.ui_descriptionTB = new System.Windows.Forms.TextBox();
            this.ui_noteTB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ui_infoLabel = new System.Windows.Forms.Label();
            this.ui_versionTB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(37, 12);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 13);
            label1.TabIndex = 0;
            label1.Text = "Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 38);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(63, 13);
            label2.TabIndex = 2;
            label2.Text = "Description:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(42, 91);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(33, 13);
            label3.TabIndex = 4;
            label3.Text = "Note:";
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BackColor = System.Drawing.Color.Gray;
            panel1.Location = new System.Drawing.Point(12, 174);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(390, 1);
            panel1.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(30, 64);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(45, 13);
            label4.TabIndex = 11;
            label4.Text = "Version:";
            // 
            // ui_nameTB
            // 
            this.ui_nameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nameTB.Location = new System.Drawing.Point(81, 9);
            this.ui_nameTB.Name = "ui_nameTB";
            this.ui_nameTB.Size = new System.Drawing.Size(321, 20);
            this.ui_nameTB.TabIndex = 1;
            // 
            // ui_descriptionTB
            // 
            this.ui_descriptionTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_descriptionTB.Location = new System.Drawing.Point(81, 35);
            this.ui_descriptionTB.Name = "ui_descriptionTB";
            this.ui_descriptionTB.Size = new System.Drawing.Size(321, 20);
            this.ui_descriptionTB.TabIndex = 3;
            // 
            // ui_noteTB
            // 
            this.ui_noteTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_noteTB.Location = new System.Drawing.Point(81, 88);
            this.ui_noteTB.Multiline = true;
            this.ui_noteTB.Name = "ui_noteTB";
            this.ui_noteTB.Size = new System.Drawing.Size(321, 77);
            this.ui_noteTB.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(246, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OK_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(327, 190);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_infoLabel
            // 
            this.ui_infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_infoLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_infoLabel.Location = new System.Drawing.Point(10, 179);
            this.ui_infoLabel.Name = "ui_infoLabel";
            this.ui_infoLabel.Size = new System.Drawing.Size(230, 37);
            this.ui_infoLabel.TabIndex = 9;
            this.ui_infoLabel.Text = "<Info>";
            this.ui_infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_versionTB
            // 
            this.ui_versionTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_versionTB.Location = new System.Drawing.Point(81, 61);
            this.ui_versionTB.Name = "ui_versionTB";
            this.ui_versionTB.ReadOnly = true;
            this.ui_versionTB.Size = new System.Drawing.Size(321, 20);
            this.ui_versionTB.TabIndex = 12;
            // 
            // UserGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 225);
            this.Controls.Add(this.ui_versionTB);
            this.Controls.Add(label4);
            this.Controls.Add(this.ui_infoLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(panel1);
            this.Controls.Add(this.ui_noteTB);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_descriptionTB);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_nameTB);
            this.Controls.Add(label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserGroupForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New User Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_nameTB;
        private System.Windows.Forms.TextBox ui_descriptionTB;
        private System.Windows.Forms.TextBox ui_noteTB;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label ui_infoLabel;
        private System.Windows.Forms.TextBox ui_versionTB;
    }
}