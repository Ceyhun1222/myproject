using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AranUpdateManager.Data;
using AranUpdManager;

namespace AranUpdateManager
{
    public partial class UsersPageControl : UserControl, IPage
    {
        private List<UserGroup> _userGroups;
        private List<User> _users;
        private UserGroup _selectedUserGroup;
        private User _selectedUser;
        private string _statusText;
        private UserGroup _newJoinedGroup;

        private Comparison<User> _userSorter;
        private int _userSortedColumn;
        private SortOrder _userSortedOrder;

        private Comparison<UserGroup> _userGroupSorter;
        private int _userGroupSortedColumn;
        private SortOrder _userGroupSortedOrder;
        private UserLogForm _logForm;

        
        public UsersPageControl()
        {
            InitializeComponent();

            _userGroups = new List<UserGroup>();
            _users = new List<User>();
            _newJoinedGroup = new UserGroup { Name = "New Joined" };

            #region DGV User Sorter

            _userSorter = ((c1, c2) =>
            {
                int res = 0;

                switch (_userSortedColumn)
                {
                    case 0:
                        res = string.Compare(c1.UserName, c2.UserName);
                        break;
                    case 1:
                        res = string.Compare(c1.FullName, c2.FullName);
                        break;
                    case 2:
                        res = string.Compare(c1.LastUpdatedVersion, c2.LastUpdatedVersion);
                        break;
                }

                if (_userSortedOrder == SortOrder.Descending)
                    res = -1 * res;

                return res;
            });

            #endregion

            #region DGV User Group Sorter

            _userGroupSorter = ((c1, c2) =>
            {
                int res = 0;

                if (c1 == _newJoinedGroup)
                    return -1;

                if (c2 == _newJoinedGroup)
                    return 1;

                switch (_userSortedColumn)
                {
                    case 0:
                        res = string.Compare(c1.Name, c2.Name);
                        break;
                    case 1:
                        res = string.Compare(c1.Version.Text, c2.Version.Text);
                        break;
                    case 2:
                        res = string.Compare(c1.Description, c2.Description);
                        break;
                }

                if (_userGroupSortedOrder == SortOrder.Descending)
                    res = -1 * res;

                return res;
            });

            #endregion
        }


        public void OpenPage()
        {
            _userGroups = Global.DbPro.GetUserGroups();
            _userGroups.Insert(0, _newJoinedGroup);

            RefreshGroupsGrid();
        }

        public void ClosePage()
        {
            _userGroups.Clear();
            RefreshGroupsGrid();
        }

        public event EventHandler StatusTextChanged;

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                if (StatusTextChanged != null)
                    StatusTextChanged(this, null);
            }
        }


        private void RefreshGroupsGrid()
        {
            ui_groupDgv.RowCount = _userGroups.Count;
            ui_groupDgv.Refresh();
        }

        private void ReloadUsers()
        {
            if (_selectedUserGroup != null)
                _users = Global.DbPro.GetUsers(_selectedUserGroup == _newJoinedGroup ? -1 : _selectedUserGroup.Id);
            else
                _users = new List<User>();

            foreach (DataGridViewColumn col in ui_userDGV.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            RefreshUsersGrid();
        }

        private void RefreshUsersGrid()
        {
            ui_userDGV.RowCount = _users.Count;
            ui_userDGV.Refresh();

            StatusText = string.Format("Groups: {0}, Users: {1}", _userGroups.Count, _users.Count);

            UserDGV_CurrentCellChanged(null, null);
        }

        private void GroupDGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _userGroups.Count)
                return;

            var userGroup = _userGroups[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = userGroup.Name;
                    break;
                case 1:
                    e.Value = userGroup.Version;
                    break;
                case 2:
                    e.Value = userGroup.Description;
                    break;
            }
        }

        private void GroupDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            _selectedUserGroup = null;

            if (ui_groupDgv.CurrentCell != null)
            {
                var rowIndex = ui_groupDgv.CurrentCell.RowIndex;
                if (rowIndex >= 0 && rowIndex < _userGroups.Count)
                    _selectedUserGroup = _userGroups[rowIndex];
            }

            var b = (_selectedUserGroup != null && _selectedUserGroup.Id > 0);

            ui_editGroupTSB.Enabled = b;
            ui_deleteGroupTSB.Enabled = b;
            ui_newUserTSB.Enabled = b;

            ReloadUsers();
        }

        private void GroupDGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_userGroupSorter == null)
                return;
            
            if (_userGroupSortedColumn != -1 && _userGroupSortedColumn != e.ColumnIndex)
                ui_groupDgv.Columns[_userGroupSortedColumn].HeaderCell.SortGlyphDirection = SortOrder.None;

            var hc = ui_groupDgv.Columns[e.ColumnIndex].HeaderCell;
            hc.SortGlyphDirection = (hc.SortGlyphDirection != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending);

            _userGroupSortedColumn = e.ColumnIndex;
            _userGroupSortedOrder = hc.SortGlyphDirection;

            _userGroups.Sort(_userGroupSorter);

            RefreshGroupsGrid();
        }

        private void GroupDgv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_selectedUserGroup == null || _selectedUserGroup.Id < 1 || e.RowIndex < 0)
                return;

            NewOrEditUserGroup(_selectedUserGroup);
        }
        
        private void GroupDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _userGroups.Count)
                return;

            var isNewJoined = (_userGroups[e.RowIndex] == _newJoinedGroup);
            if (isNewJoined)
            {
                e.CellStyle = new DataGridViewCellStyle(ui_groupDgv.DefaultCellStyle) { Font = new Font(Font, FontStyle.Bold) };
            }
        }

        private void NewGroup_Click(object sender, EventArgs e)
        {
            NewOrEditUserGroup(new UserGroup());
        }

        private void NewOrEditUserGroup(UserGroup userGroup)
        {
            var grForm = new UserGroupForm();
            grForm.SetValue(userGroup);

            if (grForm.ShowDialog() != DialogResult.OK)
                return;

            Global.DbPro.SetUserGroup(userGroup);
            OpenPage();
        }

        private void EditGroup_Click(object sender, EventArgs e)
        {
            if (_selectedUserGroup == null)
                return;

            NewOrEditUserGroup(_selectedUserGroup);
        }

        private void DeleteGroup_Click(object sender, EventArgs e)
        {
            if (_selectedUserGroup == null)
                return;

            var mbr = MessageBox.Show("Do you want to delete the selected user group?", "Users", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (mbr != DialogResult.Yes)
                return;

            try
            {
                Global.DbPro.DeleteUserGroup(_selectedUserGroup.Id);
                OpenPage();
            }
            catch (Exception ex)
            {
                Global.ShowException(ex);
            }
        }

        private void UserDGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _users.Count)
                return;

            var user = _users[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = user.UserName;
                    break;
                case 1:
                    e.Value = user.FullName;
                    break;
                case 2:
                    e.Value = GetLastVersionText(user);
                    break;
                case 3:
                    e.Value = user.HasLog ? Properties.Resources.attach_20 : Properties.Resources.empty_20;
                    if (user.HasLog && ui_userDGV.CurrentCell != null && ui_userDGV.CurrentCell.RowIndex == e.RowIndex)
                        e.Value = Properties.Resources.attach_sel_20;
                    break;
            }
        }

        private void UserDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            _selectedUser = null;

            if (ui_userDGV.CurrentCell != null &&
                ui_userDGV.CurrentCell.RowIndex >= 0 &&
                ui_userDGV.CurrentCell.RowIndex < _users.Count)
            {
                _selectedUser = _users[ui_userDGV.CurrentCell.RowIndex];
            }

            var b = (_selectedUser != null);

            ui_editUserTSB.Enabled = b && (_selectedUserGroup != null && _selectedUserGroup.Id > 0);
            ui_deleteUserTSB.Enabled = b;
            ui_moveUserTSB.Enabled = b;
            ui_showLogTSB.Enabled = b;

            if (_logForm != null && !_logForm.IsDisposed)
                _logForm.User = _selectedUser;
        }

        private void UserDGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_userSorter == null)
                return;

            if (_userSortedColumn != -1 && _userSortedColumn != e.ColumnIndex)
                ui_userDGV.Columns[_userSortedColumn].HeaderCell.SortGlyphDirection = SortOrder.None;

            var hc = ui_userDGV.Columns[e.ColumnIndex].HeaderCell;
            hc.SortGlyphDirection = (hc.SortGlyphDirection != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending);

            _userSortedColumn = e.ColumnIndex;
            _userSortedOrder = hc.SortGlyphDirection;

            _users.Sort(_userSorter);

            RefreshUsersGrid();
        }

        private void UserDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_selectedUser == null || e.RowIndex < 0)
                return;

            NewOrEditUser(_selectedUser);
        }

        private void UserDGV_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = ui_userItemContextMenu;
        }

        private string GetLastVersionText(User user)
        {
            if (string.IsNullOrEmpty(user.LastDownloadedVersion) || string.IsNullOrEmpty(user.LastUpdatedVersion))
                return string.Empty;

            if (user.LastDownloadedVersion == user.LastUpdatedVersion)
                return user.LastUpdatedVersion;

            return string.Format("{0} (Downloaded: {1})", user.LastUpdatedVersion, user.LastDownloadedVersion);
        }

        private void NewUser_Click(object sender, EventArgs e)
        {
            var user = new User();
            user.Group.Assign(_selectedUserGroup);

            NewOrEditUser(user);
        }

        private void NewOrEditUser(User user)
        {
            var userForm = new UserForm();
            userForm.SetValue(user);

            if (userForm.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Global.DbPro.SetUser(user);
                ReloadUsers();
            }
            catch (Exception ex)
            {
                Global.ShowException(ex);
            }
        }

        private void EditUser_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
                return;

            NewOrEditUser(_selectedUser);
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
                return;

            var mbr = MessageBox.Show("Do you want to delete the selected user?", "Users",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (mbr != DialogResult.Yes)
                return;

            try
            {
                Global.DbPro.DeleteUser(_selectedUser.Id);
                ReloadUsers();
            }
            catch (Exception ex)
            {
                Global.ShowException(ex);
            }
        }

        private void MoveUser_Click(object sender, EventArgs e)
        {
            var cms = new ContextMenuStrip();

            foreach (var item in _userGroups)
            {
                if (item == _newJoinedGroup || item == _selectedUserGroup)
                    continue;

                var tsmi = new ToolStripMenuItem(item.Name) { Tag = item };
                tsmi.Click += //MoveUserToGroup;

                (ss, ee) =>
                {
                    var userGroup = (ss as ToolStripMenuItem).Tag as UserGroup;

                    var mbr = MessageBox.Show(string.Format(
                        "Do you want to move {0} User to {1} User Group", _selectedUser.Description, userGroup.Description),
                        "Move User", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (mbr != DialogResult.Yes)
                        return;

                    Global.DbPro.MoveUser(_selectedUser.Id, userGroup.Id);
                    ReloadUsers();
                    UserDGV_CurrentCellChanged(null, null);
                };

                cms.Items.Add(tsmi);
            }

            var pt1 = ui_moveUserTSB.Bounds.Location;
            var pt2 = ui_userToolStrip.Parent.Bounds.Location;
            cms.Show(this, new Point(pt1.X + pt2.X, pt1.Y + pt2.Y + ui_moveUserTSB.Bounds.Height));
        }

        private void ShowLog_Click(object sender, EventArgs e)
        {
            if (_logForm == null || _logForm.IsDisposed)
            {
                _logForm = new UserLogForm();
                _logForm.Show(MainForm.Instance);
            }

            _logForm.User = _selectedUser;
            _logForm.Visible = true;
        }

        //private void MoveUserToGroup(object sender, EventArgs e)
        //{
        //    var userGroup = (sender as ToolStripMenuItem).Tag as UserGroup;

        //    var mbr = MessageBox.Show(string.Format(
        //        "Do you want to move {0} User to {1} User Group", _selectedUser.Description, userGroup.Description),
        //        "Move User", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        //    if (mbr != DialogResult.Yes)
        //        return;

        //    Global.DbPro.MoveUser(_selectedUser.Id, userGroup.Id);
        //    ReloadUsers();
        //}
    }
}
