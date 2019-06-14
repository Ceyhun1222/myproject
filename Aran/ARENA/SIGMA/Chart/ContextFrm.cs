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
    public partial class ContextFrm : Form
    {
        public ContextFrm()
        {
            InitializeComponent();
        }

        private void ContextFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void ContextFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
