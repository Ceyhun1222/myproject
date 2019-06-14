using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.PANDA.Vss
{
    public partial class InfoButtonControl : UserControl
    {
        public InfoButtonControl()
        {
            InitializeComponent();
        }

        
        public event EventHandler Clicked;


        private void InfoButton_Click(object sender, EventArgs e)
        {
            if (Clicked != null)
                Clicked(this, e);
        }
    }
}
