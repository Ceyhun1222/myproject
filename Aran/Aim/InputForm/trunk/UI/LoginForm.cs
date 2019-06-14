using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.InputForm
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

#if TEST
            ui_userNameTB.Text = "administrator";
            ui_passwordTB.Text = "aim_administrator";
#endif
        }


        public event EventHandler OKClicked;

        
        public string UserName
        {
            get { return ui_userNameTB.Text; }
            set
            {
                ui_userNameTB.Text = value;
                ui_passwordTB.Text = string.Empty;
                ui_errorLabel.Visible = false;
            }
        }

        public string Password
        {
            get { return ui_passwordTB.Text; }
            set { ui_passwordTB.Text = value; }
        }

        public string ErrorMessage
        {
            get { return ui_errorLabel.Text; }
            set
            {
                ui_errorLabel.Text = value;
                ui_errorLabel.Visible = !string.IsNullOrEmpty(value);
            }
        }


        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (OKClicked != null)
                OKClicked(this, e);
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            ui_passwordTB.UseSystemPasswordChar = !ui_showPasswordChB.Checked;
        }

        private void UserNameOrPassword_TextChanged(object sender, EventArgs e)
        {
            ui_errorLabel.Visible = false;
        }
    }
}
