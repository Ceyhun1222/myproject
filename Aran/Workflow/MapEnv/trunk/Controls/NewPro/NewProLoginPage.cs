using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class NewProLoginPage : UserControl
    {
        public NewProLoginPage()
        {
            InitializeComponent();

            ui_savePswChB.Checked = Globals.Settings.IsUserNamePasswordSaved;
        }

        public bool UserNameReadOnly
        {
            get { return ui_userNameTB.ReadOnly; }
            set { ui_userNameTB.ReadOnly = value; }
        }

        public string UserName
        {
            get { return ui_userNameTB.Text; }
            set
            {
                if (Globals.Settings.IsUserNamePasswordSaved) {
                    if (string.IsNullOrEmpty(value)) {
                        ui_userNameTB.Text = Globals.Settings.UserName;
                        ui_passwordTB.Text = Globals.Settings.Password;
                    }
                    else {
                        ui_userNameTB.Text = value;
                        if (value == Globals.Settings.UserName) {
                            ui_passwordTB.Text = Globals.Settings.Password;
                        }
                    }
                }
                else {
                    ui_userNameTB.Text = value;
                    ui_passwordTB.Text = string.Empty;
                }

                ui_errorLabel.Visible = false;
            }
        }

        public string Password
        {
            get { return ui_passwordTB.Text; }
        }

        public string ErrorMessage
        {
            get { return ui_errorLabel.Text; }
            set
            {
                ui_errorLabel.Text = value;
                ui_errorLabel.Visible = true;
            }
        }

        public bool IsSavePassword
        {
            get { return ui_savePswChB.Checked; }
        }


        private void UserNameOrPassword_TextChanged(object sender, EventArgs e)
        {
            ui_errorLabel.Visible = false;
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            ui_passwordTB.UseSystemPasswordChar = !ui_showPasswordChB.Checked;
        }
    }
}
