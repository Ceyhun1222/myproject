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
    public partial class ArenaMessageForm : Form
    {
        public ArenaMessageForm()
        {
            InitializeComponent();
        }
        public ArenaMessageForm(string CaptionText, string messageText, Image img)
        {
            InitializeComponent();
            this.Text = CaptionText;
            this.label1.Text = messageText;
            if (img !=null)this.pictureBox1.Image = img;
            this.TopMost = true;
        }
    }
}
