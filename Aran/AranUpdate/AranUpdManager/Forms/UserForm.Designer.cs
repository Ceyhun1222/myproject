namespace AranUpdateManager
{
    partial class UserForm
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label2;
            this.ui_infoLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ui_noteTB = new System.Windows.Forms.TextBox();
            this.ui_userNameTB = new System.Windows.Forms.TextBox();
            this.ui_fullNameTB = new System.Windows.Forms.TextBox();
            this.ui_groupTB = new System.Windows.Forms.TextBox();
            panel1 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BackColor = System.Drawing.Color.Gray;
            panel1.Location = new System.Drawing.Point(11, 178);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(390, 1);
            panel1.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(39, 89);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(33, 13);
            label3.TabIndex = 6;
            label3.Text = "Note:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 13);
            label1.TabIndex = 0;
            label1.Text = "User Name:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(15, 37);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(57, 13);
            label4.TabIndex = 2;
            label4.Text = "Full Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(33, 63);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 13);
            label2.TabIndex = 4;
            label2.Text = "Group:";
            // 
            // ui_infoLabel
            // 
            this.ui_infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_infoLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_infoLabel.Location = new System.Drawing.Point(9, 183);
            this.ui_infoLabel.Name = "ui_infoLabel";
            this.ui_infoLabel.Size = new System.Drawing.Size(154, 37);
            this.ui_infoLabel.TabIndex = 9;
            this.ui_infoLabel.Text = "<Info>";
            this.ui_infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(327, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(246, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_noteTB
            // 
            this.ui_noteTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_noteTB.Location = new System.Drawing.Point(78, 86);
            this.ui_noteTB.Multiline = true;
            this.ui_noteTB.Name = "ui_noteTB";
            this.ui_noteTB.Size = new System.Drawing.Size(324, 79);
            this.ui_noteTB.TabIndex = 7;
            // 
            // ui_userNameTB
            // 
            this.ui_userNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_userNameTB.Location = new System.Drawing.Point(78, 8);
            this.ui_userNameTB.Name = "ui_userNameTB";
            this.ui_userNameTB.Size = new System.Drawing.Size(324, 20);
            this.ui_userNameTB.TabIndex = 1;
            // 
            // ui_fullNameTB
            // 
            this.ui_fullNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_fullNameTB.Location = new System.Drawing.Point(78, 34);
            this.ui_fullNameTB.Name = "ui_fullNameTB";
            this.ui_fullNameTB.Size = new System.Drawing.Size(324, 20);
            this.ui_fullNameTB.TabIndex = 3;
            // 
            // ui_groupTB
            // 
            this.ui_groupTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_groupTB.Location = new System.Drawing.Point(77, 60);
            this.ui_groupTB.Name = "ui_groupTB";
            this.ui_groupTB.ReadOnly = true;
            this.ui_groupTB.Size = new System.Drawing.Size(324, 20);
            this.ui_groupTB.TabIndex = 5;
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 234);
            this.Controls.Add(this.ui_groupTB);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_fullNameTB);
            this.Controls.Add(label4);
            this.Controls.Add(this.ui_infoLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(panel1);
            this.Controls.Add(this.ui_noteTB);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_userNameTB);
            this.Controls.Add(label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ui_infoLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ui_noteTB;
        private System.Windows.Forms.TextBox ui_userNameTB;
        private System.Windows.Forms.TextBox ui_fullNameTB;
        private System.Windows.Forms.TextBox ui_groupTB;
    }
}