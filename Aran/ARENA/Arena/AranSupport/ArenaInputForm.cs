using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARENA
{
    public partial class ArenaInputForm : Form
    {
        public ArenaInputForm()
        {
            InitializeComponent();
        }

        public int IntValue { get; set; }
        public ArenaInputForm(string CaptionText, string messageText, decimal startValue = 0, decimal minVal =1,  Image img = null)
        {
            InitializeComponent();
            this.Text = CaptionText;
            this.label1.Text = messageText;
            if (img !=null)this.pictureBox1.Image = img;
            this.TopMost = true;
            numericUpDown1.Value = startValue>0? startValue : 1;
            numericUpDown1.Minimum = minVal;
        }

        private void ArenaInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IntValue = (int)numericUpDown1.Value;
        }
    }
}
