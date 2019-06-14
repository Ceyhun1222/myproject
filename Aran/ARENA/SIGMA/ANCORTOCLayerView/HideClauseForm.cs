using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANCORTOCLayerView
{
    public partial class HideClauseForm : Form
    {
        private string _clause;
        public string Clause
        {
            get { return _clause; }
            set { _clause = value; }
        }

        public bool Match { get => _match; set => _match = value; }

        private bool _match;

        public int legLength { get; set; }
        public string lengthUOM { get; set; }

        public HideClauseForm()
        {
            InitializeComponent();
            Clause = null;
            this.legLength = -1;
            //this.lengthUOM = null;
        }


        public HideClauseForm( string distUOM)
        {
            InitializeComponent();
            Clause = null;
            this.legLength = -1;
            label2.Text = distUOM;
            //this.lengthUOM = null;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedIndex == 0)
            {
                this.Clause = textBox1.Text;
                this.Match = radioButton1.Checked;
                this.legLength = -1;
                //this.lengthUOM = null;
            }
            else
            {
                this.Clause = null;
                this.legLength = (int)numericUpDown1.Value;
                //this.lengthUOM = comboBox1.Text;
            }
            this.Close();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
