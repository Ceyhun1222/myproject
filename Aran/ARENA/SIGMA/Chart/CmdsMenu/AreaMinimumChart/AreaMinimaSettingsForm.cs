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
    public partial class AreaMinimaSettingsForm : Form
    {
        public AreaMinimaSettingsForm()
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

        private void comboBoxDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            if (comboBoxDist.SelectedIndex == 0)
            {
                comboBox1.Items.Add("5");
                comboBox1.Items.Add("10");
                label11.Text = "NM";
                label12.Text = "NM";
                label6.Text = "NM";
                numericUpDown2.Value = 5;
                numericUpDown4.Value = 30;
            }
            else
            {
                comboBox1.Items.Add("10");
                comboBox1.Items.Add("20");
                label11.Text = "KM";
                label12.Text = "KM";
                label6.Text = "KM";
                numericUpDown2.Value = 10;
                numericUpDown4.Value = 60;
            }

            comboBox1.SelectedIndex = 0;
        }

        private void comboBoxVert_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxVert.SelectedIndex == 0)
                label5.Text = "FT";
            else
                label5.Text = "M";

        }
    }
}
