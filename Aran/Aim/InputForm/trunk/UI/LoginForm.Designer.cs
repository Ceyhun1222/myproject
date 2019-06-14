namespace Aran.Aim.InputForm
{
    partial class LoginForm
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.ui_showPasswordChB = new System.Windows.Forms.CheckBox();
            this.ui_errorLabel = new System.Windows.Forms.Label();
            this.ui_passwordTB = new System.Windows.Forms.TextBox();
            this.ui_userNameTB = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 35);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 13);
            label2.TabIndex = 8;
            label2.Text = "Password:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 13);
            label1.TabIndex = 6;
            label1.Text = "User Name:";
            // 
            // ui_showPasswordChB
            // 
            this.ui_showPasswordChB.AutoSize = true;
            this.ui_showPasswordChB.Location = new System.Drawing.Point(81, 58);
            this.ui_showPasswordChB.Name = "ui_showPasswordChB";
            this.ui_showPasswordChB.Size = new System.Drawing.Size(102, 17);
            this.ui_showPasswordChB.TabIndex = 11;
            this.ui_showPasswordChB.Text = "Show Password";
            this.ui_showPasswordChB.UseVisualStyleBackColor = true;
            this.ui_showPasswordChB.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // ui_errorLabel
            // 
            this.ui_errorLabel.ForeColor = System.Drawing.Color.Red;
            this.ui_errorLabel.Location = new System.Drawing.Point(12, 78);
            this.ui_errorLabel.Name = "ui_errorLabel";
            this.ui_errorLabel.Size = new System.Drawing.Size(244, 53);
            this.ui_errorLabel.TabIndex = 10;
            this.ui_errorLabel.Text = "<Error Message>";
            this.ui_errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_errorLabel.Visible = false;
            // 
            // ui_passwordTB
            // 
            this.ui_passwordTB.Location = new System.Drawing.Point(81, 32);
            this.ui_passwordTB.Name = "ui_passwordTB";
            this.ui_passwordTB.Size = new System.Drawing.Size(178, 20);
            this.ui_passwordTB.TabIndex = 9;
            this.ui_passwordTB.UseSystemPasswordChar = true;
            this.ui_passwordTB.TextChanged += new System.EventHandler(this.UserNameOrPassword_TextChanged);
            // 
            // ui_userNameTB
            // 
            this.ui_userNameTB.Location = new System.Drawing.Point(81, 6);
            this.ui_userNameTB.Name = "ui_userNameTB";
            this.ui_userNameTB.Size = new System.Drawing.Size(178, 20);
            this.ui_userNameTB.TabIndex = 7;
            this.ui_userNameTB.TextChanged += new System.EventHandler(this.UserNameOrPassword_TextChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(103, 134);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(184, 134);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(277, 169);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.ui_showPasswordChB);
            this.Controls.Add(this.ui_errorLabel);
            this.Controls.Add(this.ui_passwordTB);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_userNameTB);
            this.Controls.Add(label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ui_showPasswordChB;
        private System.Windows.Forms.Label ui_errorLabel;
        private System.Windows.Forms.TextBox ui_passwordTB;
        private System.Windows.Forms.TextBox ui_userNameTB;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}