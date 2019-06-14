using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart.CmdsMenu.TemplatesManagerMenu
{
    public partial class fileNameForm : Form
    {
        public fileNameForm()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (listBox1.SelectedItem != null)
                textBox1.Text = listBox1.SelectedItem.ToString();
        }
    }
}
