namespace MapEnv
{
    partial class NewProLoginPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.ui_userNameTB = new System.Windows.Forms.TextBox();
            this.ui_passwordTB = new System.Windows.Forms.TextBox();
            this.ui_errorLabel = new System.Windows.Forms.Label();
            this.ui_showPasswordChB = new System.Windows.Forms.CheckBox();
            this.ui_savePswChB = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 13);
            label1.TabIndex = 0;
            label1.Text = "User Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(9, 47);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 2;
            label2.Text = "Password:";
            // 
            // ui_userNameTB
            // 
            this.ui_userNameTB.Location = new System.Drawing.Point(78, 18);
            this.ui_userNameTB.Name = "ui_userNameTB";
            this.ui_userNameTB.Size = new System.Drawing.Size(240, 20);
            this.ui_userNameTB.TabIndex = 1;
            this.ui_userNameTB.TextChanged += new System.EventHandler(this.UserNameOrPassword_TextChanged);
            // 
            // ui_passwordTB
            // 
            this.ui_passwordTB.Location = new System.Drawing.Point(78, 44);
            this.ui_passwordTB.Name = "ui_passwordTB";
            this.ui_passwordTB.Size = new System.Drawing.Size(240, 20);
            this.ui_passwordTB.TabIndex = 3;
            this.ui_passwordTB.UseSystemPasswordChar = true;
            this.ui_passwordTB.TextChanged += new System.EventHandler(this.UserNameOrPassword_TextChanged);
            // 
            // ui_errorLabel
            // 
            this.ui_errorLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_errorLabel.Location = new System.Drawing.Point(22, 96);
            this.ui_errorLabel.Name = "ui_errorLabel";
            this.ui_errorLabel.Size = new System.Drawing.Size(277, 42);
            this.ui_errorLabel.TabIndex = 4;
            this.ui_errorLabel.Text = "<Error Message>";
            this.ui_errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_errorLabel.Visible = false;
            // 
            // ui_showPasswordChB
            // 
            this.ui_showPasswordChB.AutoSize = true;
            this.ui_showPasswordChB.Location = new System.Drawing.Point(217, 76);
            this.ui_showPasswordChB.Name = "ui_showPasswordChB";
            this.ui_showPasswordChB.Size = new System.Drawing.Size(101, 17);
            this.ui_showPasswordChB.TabIndex = 5;
            this.ui_showPasswordChB.Text = "Show password";
            this.ui_showPasswordChB.UseVisualStyleBackColor = true;
            this.ui_showPasswordChB.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // ui_savePswChB
            // 
            this.ui_savePswChB.AutoSize = true;
            this.ui_savePswChB.Location = new System.Drawing.Point(12, 76);
            this.ui_savePswChB.Name = "ui_savePswChB";
            this.ui_savePswChB.Size = new System.Drawing.Size(174, 17);
            this.ui_savePswChB.TabIndex = 6;
            this.ui_savePswChB.Text = "Save User name and password";
            this.ui_savePswChB.UseVisualStyleBackColor = true;
            // 
            // NewProLoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_savePswChB);
            this.Controls.Add(this.ui_showPasswordChB);
            this.Controls.Add(this.ui_errorLabel);
            this.Controls.Add(this.ui_passwordTB);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_userNameTB);
            this.Controls.Add(label1);
            this.Name = "NewProLoginPage";
            this.Size = new System.Drawing.Size(334, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_userNameTB;
        private System.Windows.Forms.TextBox ui_passwordTB;
        private System.Windows.Forms.Label ui_errorLabel;
        private System.Windows.Forms.CheckBox ui_showPasswordChB;
        private System.Windows.Forms.CheckBox ui_savePswChB;
    }
}
