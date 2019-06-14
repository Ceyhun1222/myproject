using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public partial class PropertyForm : Form
    {
        public PropertyForm ()
        {
            InitializeComponent ();
        }

        public void SetControl (Control control)
        {
            Controls.Add (control);

            Width = control.Width + 4;
            Height = control.Height + 60;
        }
    }
}
