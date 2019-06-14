using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiracUtil
{
    public partial class AiracControl : UserControl
    {
        private int _AiracCircleValue;

        public int AiracCircleValue
        {
            get { return _AiracCircleValue; }
            set { _AiracCircleValue = value;
            numericUpDown1.Value = value;
            }
        }

        public AiracControl()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.AiracCircleValue = (int)numericUpDown1.Value;
        }
    }
}
