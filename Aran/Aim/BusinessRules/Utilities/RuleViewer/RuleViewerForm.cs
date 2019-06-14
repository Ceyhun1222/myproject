using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using Aran.Aim.BusinessRules.SbvrParser;
using Aran.Aim.BusinessRules;

namespace RuleViewer
{
    public partial class RuleViewerForm : Form, IMessageFilter
    {
        private SQLiteConnection _conn;
        private List<RuleItem> _rules;
        private int _currentRowIndex;
        private RuleItem _currentItem;
        private List<TaggedItem> _taggedItems;
        private string _currentTaggedText;
        private string _ruleType;
        private List<Control> _pages;
        private int _pageIndex;
        private string _prevNotRealizedReasonTex;
        private string _prevGroupedByText;

        public RuleViewerForm()
        {
            InitializeComponent();

            _rules = new List<RuleItem>();
            _taggedItems = new List<TaggedItem>();
            _currentRowIndex = -1;
            _currentTaggedText = string.Empty;
            _ruleType = "all";
            
            Application.AddMessageFilter(this);

            _pages = new List<Control> { ui_rulesDGV, ui_taggedDGV };
            _pageIndex = 0;
        }

        private void RuleViewerForm_Load(object sender, EventArgs e)
        {
            var fileName = Environment.GetEnvironmentVariable("BRULE_DB", EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                fileName = Path.Combine(Path.GetDirectoryName(loc), "brule-db.sdb");
            }

            _conn = new SQLiteConnection();
            _conn.ConnectionString = @"Data Source = " + fileName;
            _conn.Open();

            ui_ruleTypeCB.SelectedIndex = 0;
            //GetRules();
        }

        private void RolesDGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _rules.Count)
                return;

            var item = _rules[e.RowIndex];
            
            switch(e.ColumnIndex)
            {
                case 0:
                    e.Value = item.UID;
                    break;
                case 1:
                    e.Value = item.Profile;
                    break;
                case 2:
                    e.Value = item.Info == null ? false : item.Info.Active;
                    break;
                case 3:
                    e.Value = item.Info == null ? false : item.Info.Realized;
                    break;
            }
        }

        private void RolesDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            if (ui_rulesDGV.CurrentCell == null || _currentRowIndex == ui_rulesDGV.CurrentCell.RowIndex)
                return;

            _currentRowIndex = ui_rulesDGV.CurrentCell.RowIndex;
            CurrentRowChanged();
        }

        private void CurrentRowChanged()
        {
            if (_currentRowIndex < 0 || _currentRowIndex >= _rules.Count)
                return;

            _currentItem = _rules[_currentRowIndex];

            FillTaggedText();
        }

        private string GetRuleWhereCommand()
        {
            var whereText = " 1 ";

            if (_ruleType == "all")
                whereText = " 1 ";
            else if (_ruleType == "error")
                whereText = " Profile = 'Error' ";
            else if (_ruleType == "warning")
                whereText = " Profile = 'Warning' ";
            else if (_ruleType == "lgs")
                whereText = " lgs_order > 0 ";
            else if (_ruleType == "active")
                whereText = " is_active = 1 ";
            else if (_ruleType == "realized")
                whereText = " not_implemented_reason IS NULL ";
            else if (_ruleType == "notrealized")
                whereText = " not_implemented_reason IS NOT NULL ";

            if (ui_searchTB.Text.Length > 0)
            {
                var sa = ui_searchTB.Text.Split(";".ToCharArray());
                var filter = new StringBuilder();
                for (int i = 0; i < sa.Length; i++)
                    filter.Append("'AIXM-5.1_RULE-" + sa[i].Trim() + "',");
                filter.Remove(filter.Length - 1, 1);
                whereText += " AND (UID IN (" + filter + ")) ";
            }

            return whereText;
        }

        private void GetRules()
        {
            _rules.Clear();

            var whereText = GetRuleWhereCommand();

            var cmd = _conn.CreateCommand();

            cmd.CommandText = string.Format(
                "SELECT " +
                "r.UID, r.Profile, r.is_active, (r.not_implemented_reason IS NULL) AS realized, " +
                "(CASE " +
                "   WHEN Profile = 'Error' THEN 1 " +
                "   WHEN Profile = 'Warning' THEN 2 " +
                "   ELSE 3 " +
                "END) as ord_index " +
                "FROM rules r " +
                "WHERE {0} " +
                "ORDER BY ord_index ", whereText);

            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                _rules.Add(new RuleItem
                {
                    UID = dr[0].ToString().Replace("AIXM-5.1_RULE-", ""),
                    Profile = dr[1].ToString(),
                    Info = new RuleItemInfo
                    {
                        Active = !dr.IsDBNull(2) && Convert.ToBoolean(dr[2]),
                        Realized = !dr.IsDBNull(3) && Convert.ToBoolean(dr[3]),
                    }
                });
            }

            dr.Close();

            ui_rulesDGV.SuspendLayout();
            ui_rulesDGV.RowCount = _rules.Count;
            ui_rulesDGV.ResumeLayout();
            ui_rulesDGV.Refresh();

            ui_rulesCount.Text = $"Count: {_rules.Count}";

            _currentRowIndex = -1;

            RolesDGV_CurrentCellChanged(null, null);
        }

        private void FillTaggedText()
        {
            _taggedItems.Clear();
            _currentTaggedText = string.Empty;

            ui_uidTB.Text = string.Empty;
            ui_nameTB.Text = string.Empty;
            ui_commentsTB.Text = string.Empty;
            ui_profileTB.Text = string.Empty;
            ui_activeChB.Checked = false;
            _prevNotRealizedReasonTex = string.Empty;
            ui_groupedByTB.Text = string.Empty;
            _prevGroupedByText = string.Empty;
            ui_notRealzedReasonTB.Text = _prevNotRealizedReasonTex;

            if (_currentItem != null)
            {
                var cmd = _conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT "+
                    "TaggedDescription, UID, Name, Profile, " +
                    "Comments, is_active, not_implemented_reason, grouped_by " +
                    "FROM rules " + 
                    "WHERE UID = 'AIXM-5.1_RULE-{0}'", 
                    _currentItem.UID);

                var dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    _currentTaggedText = dr[0].ToString();

                    ui_uidTB.Text = dr[1].ToString();
                    ui_nameTB.Text = dr[2].ToString();
                    ui_profileTB.Text = dr[3].ToString();
                    ui_commentsTB.Text = dr[4].ToString();

                    ui_activeChB.Checked = !dr.IsDBNull(5) && Convert.ToBoolean(dr[5]);

                    _prevNotRealizedReasonTex = dr[6].ToString();
                    ui_notRealzedReasonTB.Text = _prevNotRealizedReasonTex;
                    _prevGroupedByText = dr[7].ToString();
                    ui_groupedByTB.Text = _prevGroupedByText;
                }
                dr.Close();

                if (_currentTaggedText.Length > 0)
                {
                    var ttr = new TaggedDocument();
                    ttr.Init(_currentTaggedText);

                    while (ttr.Next())
                        _taggedItems.Add(ttr.Current);
                }
            }

            ui_taggedDGV.RowCount = _taggedItems.Count;
            ui_taggedDGV.Refresh();
        }

        private void TaggedDGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var item = _taggedItems[e.RowIndex];
            e.Value = (e.ColumnIndex == 0 ? item.Key.ToString() : item.Text);
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetRules();
        }

        private void AllOrLGS_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                _ruleType = (sender as Control).Tag as string;
                GetRules();
            }
        }

        private void RuleStatus_Click(object sender, EventArgs e)
        {
            var chb = sender as CheckBox;
            UpdateRuleStatus(chb.Tag.ToString(), chb.Checked);
        }

        private void UpdateRuleStatus(string prop, object value)
        {
            if (_currentItem == null)
                return;

            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"UPDATE rules SET {prop} = :value WHERE UID = :uid";
            cmd.Parameters.AddWithValue(":value", value);
            cmd.Parameters.AddWithValue(":uid", "AIXM-5.1_RULE-" + _currentItem.UID);

            cmd.ExecuteNonQuery();

            if (prop == "is_active")
                _currentItem.Info.Active = (bool)value;
            else if (prop == "not_implemented_reason")
                _currentItem.Info.Realized = (value == null);

            ui_rulesDGV.Refresh();
        }

        private void SetAllRuleActive(bool isActive)
        {
            var whereText = GetRuleWhereCommand();

            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"UPDATE rules SET is_active = {(isActive ? 1 : 0)} WHERE " + whereText;

            cmd.ExecuteNonQuery();

            GetRules();
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0100 && 
                (Keys)m.WParam.ToInt32() == Keys.Tab &&
                ModifierKeys == Keys.Control)
            {
                ChangePage();
                return true;
            }

            return false;
        }


        private void ChangePage()
        {
            if (_pages.Count == 0)
                return;
            _pageIndex++;
            if (_pageIndex >= _pages.Count)
                _pageIndex = 0;

            _pages[_pageIndex].Focus();
        }

        private void FillTaggedPair(string insTableName)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT rowid, TaggedDescription FROM rules";

            var dr = cmd.ExecuteReader();
            var list = new List<Tuple<long, string>>();

            while (dr.Read())
                list.Add(new Tuple<long, string>(dr.GetInt64(0), dr.GetString(1)));
            dr.Close();

            cmd.Transaction = _conn.BeginTransaction();

            cmd.CommandText = 
                $"INSERT INTO {insTableName} (rule_id, type, text, order_index) " +
                "VALUES (:rule_id, :type, :text, :order_index)";
            cmd.Parameters.AddWithValue(":rule_id", null);
            cmd.Parameters.AddWithValue(":type", null);
            cmd.Parameters.AddWithValue(":text", null);
            cmd.Parameters.AddWithValue(":order_index", null);

            var orderIndex = 0;

            foreach (var pair in list)
            {
                var reader = new TaggedDocument();
                reader.Init(pair.Item2);

                cmd.Parameters[0].Value = pair.Item1;
                orderIndex = 0;

                while (reader.Next())
                {
                    cmd.Parameters[1].Value = reader.Current.Key.ToString();
                    cmd.Parameters[2].Value = reader.Current.Text;
                    cmd.Parameters[3].Value = orderIndex++;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error: " + ex.Message);
                        cmd.Transaction.Rollback();
                        return;
                    }
                }
            }

            cmd.Transaction.Commit();
        }

        private void RuleTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ruleType = ui_ruleTypeCB.SelectedItem.ToString().ToLower();
            GetRules();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            ui_rulesContextMenu.Show(ui_menuButton, new Point(ui_menuButton.Width, ui_menuButton.Height), ToolStripDropDownDirection.BelowLeft);
        }

        private void CheckAllListItems_Click(object sender, EventArgs e)
        {
            SetAllRuleActive(true);
        }

        private void UncheckAllListItems_Click(object sender, EventArgs e)
        {
            SetAllRuleActive(false);
        }

        private void NotRealzedReason_Leave(object sender, EventArgs e)
        {
            if (_prevNotRealizedReasonTex != ui_notRealzedReasonTB.Text)
            {
                UpdateRuleStatus("not_implemented_reason", (ui_notRealzedReasonTB.Text.Length == 0 ? null : ui_notRealzedReasonTB.Text));
                _prevNotRealizedReasonTex = ui_notRealzedReasonTB.Text;
            }
        }

        private void GroupedByTB_Leave(object sender, EventArgs e)
        {
            if (_prevGroupedByText != ui_groupedByTB.Text)
            {
                UpdateRuleStatus("grouped_by", (ui_groupedByTB.Text.Length == 0 ? null : ui_groupedByTB.Text));
                _prevGroupedByText = ui_groupedByTB.Text;
            }
        }
    }

    public class RuleItem
    {
        public string UID { get; set; }
        public string Profile { get; set; }
        public RuleItemInfo Info { get; set; }
    }

    public class RuleItemInfo
    {
        public bool Active { get; set; }

        public bool Realized { get; set; }
    }
}
