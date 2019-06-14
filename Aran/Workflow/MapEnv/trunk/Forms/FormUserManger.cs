using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data;
using System.Security.Cryptography;

namespace MapEnv.Forms
{
    public partial class FormUserManger : Form
    {
        private List<User> _userList;
        private Privilige _privilege;


        public FormUserManger()
        {
            InitializeComponent();

            //_userManagement = new Aran.Aim.Data.
        }

        public FormUserManger(Privilige privilige)
            : this()
        {
            // TODO: Complete member initialization
            _privilege = privilige;
        }


        public IUserManagement UserManagement { get; set; }


        private void btnShowHide_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = !groupBox1.Visible;
            if (groupBox1.Visible)
                panel1.Dock = DockStyle.None;
            else
                panel1.Dock = DockStyle.Fill;
            if (groupBox1.Visible) {
                //btnShowHide.Location = new Point ( btnShowHide.Location.X, groupBox1.Height - 1 );
                btnShowHide.Image = global::MapEnv.Properties.Resources.arrow_up_16;
            }
            else {
                //btnShowHide.Location = new Point ( btnShowHide.Location.X, 0 );
                btnShowHide.Image = global::MapEnv.Properties.Resources.arrow_down_16;
            }
        }

        private void chckBxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtBxPassword.UseSystemPasswordChar = !chckBxShowPassword.Checked;
        }

        private void btnCreateNewUser_Click(object sender, EventArgs e)
        {
            FormUser formUser = new FormUser();
            formUser.UserList = _userList;
            if (formUser.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                InsertingResult insertResult = UserManagement.InsertUser(formUser.User);
                if (insertResult.IsSucceed) {
                    _userList.Add(formUser.User);
                    int index = dataGridVwUsers.Rows.Add();
                    FillRow(formUser.User, dataGridVwUsers.Rows[index]);
                }
                else {
                    throw new Exception("Couldn't insert user.\r\n" + insertResult.Message);
                }
            }
        }

        private void FillRow(User user, DataGridViewRow dataGridViewRow)
        {
            dataGridViewRow.Tag = user;
            dataGridViewRow.Cells[0].Value = user.Name;
            string privilege = "";
            switch (user.Privilege) {
                case Privilige.prAdmin:

                    break;
                case Privilige.prReadOnly:
                    privilege = "Read Only";
                    break;
                case Privilige.prReadWrite:
                    privilege = "Read/Write";
                    break;

                default:
                    throw new NotImplementedException(user.Privilege + " not implemented");
            }
            dataGridViewRow.Cells[1].Value = privilege;
        }

        private void FormUserManger_Load(object sender, EventArgs e)
        {
            GettingResult getResult = UserManagement.ReadUsers();
            if (!getResult.IsSucceed)
                throw new Exception(getResult.Message);
            _userList = (List<User>)getResult.List;
            if (_privilege == Privilige.prAdmin) {
                btnShowHide_Click(null, null);
                ShowUsers();
            }
        }

        private void btnUserOk_Click(object sender, EventArgs e)
        {
            User adminUser = _userList.Find(user => user.Privilege == Privilige.prAdmin);
            if (txtBxUserName.Text == adminUser.Name) {
                if (VerifyMd5Hash(txtBxPassword.Text, adminUser.Password)) {
                    ShowUsers();
                }
            }
        }

        private void ShowUsers()
        {
            panel1.Visible = true;
            int index;
            foreach (User user in _userList) {
                if (user.Privilege != Privilige.prAdmin) {
                    index = dataGridVwUsers.Rows.Add();
                    FillRow(user, dataGridVwUsers.Rows[index]);
                }
            }
        }

        // Verify a hash against a string. 
        private bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = DbUtility.GetMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (comparer.Compare(hashOfInput, hash) == 0) {
                return true;
            }
            else {
                return false;
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridVwUsers.SelectedRows.Count > 0) {
                User user = (User)dataGridVwUsers.SelectedRows[0].Tag;
                FormUser formUser = new FormUser(user);
                formUser.UserList = _userList;
                if (formUser.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    InsertingResult insertResult = UserManagement.UpdateUser(formUser.User);
                    if (insertResult.IsSucceed) {
                        _userList[_userList.IndexOf(user)] = formUser.User;
                        FillRow(formUser.User, dataGridVwUsers.SelectedRows[0]);
                        //dataGridVwUsers.EndEdit ( );
                    }
                    else
                        throw new Exception(insertResult.Message);
                }
            }
            else {
                MessageBox.Show("Please, select user !");
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridVwUsers.SelectedRows.Count > 0) {
                User user = (User)dataGridVwUsers.SelectedRows[0].Tag;
                if (MessageBox.Show("Are you sure to delete this user ?", "Deleting '" + user.Name + "'", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                    InsertingResult insertResult = UserManagement.DeleteUser(user);
                    if (insertResult.IsSucceed) {
                        _userList.Remove(user);
                        dataGridVwUsers.Rows.Remove(dataGridVwUsers.SelectedRows[0]);
                    }
                    else {
                        if (insertResult.Message.Contains("ERROR: 23503: ")) {
                            MessageBox.Show(this, "Couldn't delete this user.Because some features have links to this user", "Deleting Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            throw new Exception(insertResult.Message);
                    }
                }
            }
            else {
                MessageBox.Show("Please, select user !");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
