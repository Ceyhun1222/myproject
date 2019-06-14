using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AranUpdateManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Controls.Count > 0)
                {
                    var itemCont = tabPage.Controls[0];
                    itemCont.Dock = DockStyle.Fill;
                    itemCont.Visible = false;
                    ui_mainPanel.Controls.Add(itemCont);

                    var page = itemCont as IPage;
                    if (page != null)
                        page.StatusTextChanged += Page_StatusTextChanged;
                }
            }

            if (Instance == null)
                Instance = this;
        }

        public static MainForm Instance { get; private set; }

        private void Page_StatusTextChanged(object sender, EventArgs e)
        {
            var page = sender as IPage;
            ui_pageStatusLabel.Text = page.StatusText;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RISK\\AranUpdManager\\settings.dat");
            Global.Settings.Load(fileName);

            if (string.IsNullOrEmpty(Global.Settings.Server))
            {
                var sf = new SettingsForm();
                if (sf.ShowDialog() != DialogResult.OK)
                {
                    Close();
                    return;
                }
            }

            try
            {
                Global.DbPro.Open(Global.Settings.Server, Global.Settings.Port);

                NavbarItem_CheckedChanged(ui_versionsNI, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on connections:\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                return;
            }
        }

        private void NavbarItem_CheckedChanged(object sender, EventArgs e)
        {
            var navbarItem = sender as NavbarItem;
            Control itemControl = ui_mainPanel.Controls[navbarItem.BaseControlName];
            var page = itemControl as IPage;

            if (itemControl != null)
                itemControl.Visible = navbarItem.Checked;

            if (page != null)
            {
                if (navbarItem.Checked)
                    page.OpenPage();
                else
                    page.ClosePage();
            }
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            ui_mainContextMenuStrip.Show(ui_menuButton, 
                new Point(ui_menuButton.Width, ui_menuButton.Height), 
                ToolStripDropDownDirection.Left);
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            var sf = new SettingsForm();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                Global.DbPro.Open(Global.Settings.Server, Global.Settings.Port);
            }
        }

        private void AboutMenu_Click(object sender, EventArgs e)
        {

        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
