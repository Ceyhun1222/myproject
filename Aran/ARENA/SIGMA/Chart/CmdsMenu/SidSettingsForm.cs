using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public partial class SidSettingsForm : Form
    {
        public SidSettingsForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void SidSettingsForm_Load(object sender, EventArgs e)
        {
            //comboBoxDist.SelectedIndex = 0;
            //comboBoxVert.SelectedIndex = 0;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            label5.Enabled = checkBox4.Checked;
            label6.Enabled = checkBox4.Checked;
            label7.Enabled = checkBox4.Checked;
            label8.Enabled = checkBox4.Checked;

            numericUpDown2.Enabled = checkBox4.Checked;
            numericUpDown3.Enabled = checkBox4.Checked;

        }
    }
}
