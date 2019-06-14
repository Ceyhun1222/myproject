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
    partial class AddUserGroupToVersionForm : Form
    {
        private AranVersion _aranVersion;

        public AddUserGroupToVersionForm()
        {
            InitializeComponent();

            //ui_userGroupCB.DisplayMember = "Text";
        }


        public AranVersion AranVersion
        {
            get { return _aranVersion; }
            set
            {
                if (_aranVersion == value)
                    return;

                _aranVersion = value;
                ui_versionTB.Text = value.Description;
            }
        }

        public RefItem UserGroup
        {
            get { return ui_userGroupCB.SelectedItem as RefItem; }
        }

        public string Note
        {
            get { return ui_noteTB.Text; }
        }


        private void AddUserGroupToVersionForm_Load(object sender, EventArgs e)
        {
            if (AranVersion == null)
                return;

            ui_userGroupCB.DataSource = Global.DbPro.GetUserGroupsNotInVersion(AranVersion.Id);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (AranVersion == null || ui_userGroupCB.SelectedItem == null)
                return;

            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void UserGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ui_okButton.Enabled = (ui_userGroupCB.SelectedItem != null);
        }
    }
}
