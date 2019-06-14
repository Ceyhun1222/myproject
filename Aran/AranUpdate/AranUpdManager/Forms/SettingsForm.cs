using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AranUpdateManager
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            ui_serverTB.Text = Global.Settings.Server;
            ui_portNud.Value = Global.Settings.Port;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Global.Settings.Server = ui_serverTB.Text;
            Global.Settings.Port = (int)ui_portNud.Value;
            Global.Settings.Save();

            DialogResult = DialogResult.OK;
        }

        
    }
}
