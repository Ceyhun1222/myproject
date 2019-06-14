using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AranUpdateManager.Data;
using AranUpdManager;

namespace AranUpdateManager
{
    partial class UserForm : Form
    {
        private User _user;

        public UserForm()
        {
            InitializeComponent();

            ui_infoLabel.Text = string.Empty;
        }

        public void SetValue(User user)
        {
            ui_userNameTB.Text = user.UserName;
            ui_fullNameTB.Text = user.FullName;
            ui_groupTB.Text = user.Group.Text ?? string.Empty;
            ui_noteTB.Text = user.Note;

            Text = user.IsNew ? "New User" : "Edit User";
            _user = user;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            var err = string.Empty;

            if (ui_userNameTB.Text.Length == 0)
                err = "User Name Field cannot be empty!";

            if (ui_fullNameTB.Text.Length == 0)
                err += "\nFull Name Field cannot be empty!";

            if (err.Length > 0)
            {
                ui_infoLabel.Text = err;
                return;
            }

            _user.UserName = ui_userNameTB.Text;
            _user.FullName = ui_fullNameTB.Text;
            _user.Note = ui_noteTB.Text;

            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
