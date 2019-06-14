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
    public partial class AboutForm : Form
    {
        public AboutForm ()
        {
            InitializeComponent ();

            ui_versionTB.Text = Globals.VersionText;
        }
    }
}
