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
    partial class UserGroupForm : Form
    {
        private UserGroup _userGroup;

        public UserGroupForm()
        {
            InitializeComponent();

            ui_infoLabel.Text = string.Empty;
        }

        public void SetValue(UserGroup value)
        {
            ui_nameTB.Text = value.Name;
            ui_descriptionTB.Text = value.Description;
            ui_noteTB.Text = value.Note;
            ui_versionTB.Text = value.Version.Text;
            Text = value.IsNew ? "New UserGroup" : "Edit UserGroup";

            _userGroup = value;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (_userGroup == null)
                return;

            var err = String.Empty;

            if (ui_nameTB.Text.Length == 0)
                err = "Name Field cannot be empty!";

            if (err.Length > 0)
            {
                ui_infoLabel.Text = err;
                return;
            }

            _userGroup.Name = ui_nameTB.Text;
            _userGroup.SetDescription(ui_descriptionTB.Text);
            _userGroup.Note = ui_noteTB.Text;
            
            DialogResult = DialogResult.OK;
        }
    }
}
