using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv.Forms
{
    public partial class RasterPropertyForm : Form
    {
        public RasterPropertyForm ()
        {
            InitializeComponent ();
        }

        public string ReaterFileName
        {
            get { return ui_fileNameTB.Text; }
            set { ui_fileNameTB.Text = value; }
        }

        private void Close_Click (object sender, EventArgs e)
        {
            Close ();
        }
    }
}
