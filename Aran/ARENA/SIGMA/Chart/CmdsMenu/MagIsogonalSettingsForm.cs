using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart.CmdsMenu
{
    public partial class MagIsogonalSettingsForm : Form
    {
        public MagIsogonalSettingsForm()
        {
            InitializeComponent();
        }

        private void MagIsogonalSettingsForm_Shown(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
