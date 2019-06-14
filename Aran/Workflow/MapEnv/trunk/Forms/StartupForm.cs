using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    internal partial class StartupForm : Form
    { 
        public StartupForm ()
        {
            InitializeComponent ();

            ui_recentFilesLB.ForeColor = Color.Gray;
        }

        public string ProjectFileName { get; private set; }

        private void StartupForm_Load (object sender, EventArgs e)
        {
            ui_recentFilesLB.Items.Add (new LBItem ());

            foreach (var recentFile in Globals.Settings.RecentProjectFiles)
            {
                ui_recentFilesLB.Items.Add (new LBItem (ui_recentFilesLB.Items.Count, recentFile));
            }

            ui_showOnStartupChB.Checked = Globals.Settings.ShowStartupWindow;
        }

        private void ProRB_CheckedChanged (object sender, EventArgs e)
        {
            ui_recentFilesLB.ForeColor = (ui_existingProRB.Checked ? SystemColors.ControlText : Color.Gray);

            if (ui_newProRB.Checked)
                ui_recentFilesLB.SelectedIndex = -1;
            else if (ui_recentFilesLB.SelectedIndex == -1)
                ui_recentFilesLB.SelectedIndex = 0;
        }

        private void RecentFilesLB_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (ui_recentFilesLB.SelectedItem != null)
                ui_existingProRB.Checked = true;
        }

        private void OK_Click (object sender, EventArgs e)
        {
            LBItem lbItem = ui_recentFilesLB.SelectedItem as LBItem;

            if (ui_newProRB.Checked)
            {
                ProjectFileName = null;
            }
            else
            {
                if (lbItem == null)
                    return;

                if (lbItem.IsOpenFile)
                {
                    OpenFileDialog ofd = new OpenFileDialog ();
                    ofd.Filter = Globals.OpenFileFilters;
                    if (ofd.ShowDialog () != DialogResult.OK)
                        return;

                    ProjectFileName = ofd.FileName;
                }
                else
                {
                    ProjectFileName = lbItem.FileName;
                }
            }

            Globals.Settings.ShowStartupWindow = ui_showOnStartupChB.Checked;
            DialogResult = DialogResult.OK;
        }

        private void RecentFilesLB_MouseDoubleClick (object sender, MouseEventArgs e)
        {
            OK_Click (null, null);
        }

        internal class LBItem
        {
            public LBItem ()
            {
                _text = "Open Another File...";
                IsOpenFile = true;
            }

            public LBItem (int index, string fileName)
            {
                string sep = new string (' ', 4 - index.ToString ().Length);
                _text = index + sep + fileName;
                FileName = fileName;
                IsOpenFile = false;
            }

            public override string ToString ()
            {
                return _text;
            }

            public string FileName { get; set; }

            public bool IsOpenFile { get; private set; }

            private string _text;
        }
    }
}
