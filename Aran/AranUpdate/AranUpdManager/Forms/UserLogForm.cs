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
    partial class UserLogForm : Form
    {
        private List<Tuple<long, DateTime>> _logs;
        private User _user;

        public UserLogForm()
        {
            InitializeComponent();


            _logs = new List<Tuple<long, DateTime>>();
        }

        public User User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                    return;

                _user = value;
                Reload();
            }
        }

        private void Reload()
        {
            ui_logDGV.Rows.Clear();
            ui_logTB.Clear();

            ui_titleTSLabel.Text = string.Empty;

            if (_user == null)
                return;

            ui_titleTSLabel.Text = _user.Description + " Log";

            _logs = Global.DbPro.GetUserLogDates(_user.Id, ui_readModeUnreadRB.Checked ? "unread" : "all");

            foreach (var item in _logs)
            {
                var rowIndex = ui_logDGV.Rows.Add(new object[] { item.Item2 });
                ui_logDGV.Rows[rowIndex].Tag = item.Item1;
            }

            LogDGV_CurrentCellChanged(null, null);
        }

        private void ReadMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void LogDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            ui_logTB.Clear();
            var curRow = ui_logDGV.CurrentRow;
            if (curRow == null || curRow.Tag == null)
                return;

            var logMessage = Global.DbPro.GetUserLogText((long)curRow.Tag);
            ui_logTB.Text = logMessage;
        }
    }
}
