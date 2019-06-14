using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test.Aran.Aim.DBService
{
    public partial class Form1 : Form
    {
        public Form1 ()
        {
            InitializeComponent ();
        }

        private void ui_sendButton_Click (object sender, EventArgs e)
        {
            ServiceReference1.DbServiceClient dbService = new ServiceReference1.DbServiceClient ();
            ui_responseTB.Text = "Responsed: " + dbService.SetFeatures (ui_requestTB.Text);
        }

        private void button1_Click (object sender, EventArgs e)
        {
            ServiceReference1.DbServiceClient dbService = new ServiceReference1.DbServiceClient ();
            DateTime creationDate = new DateTime (2012, 1, 17, 19, 0, 0);
            ui_responseTB.Text = "Responsed: " + dbService.GetFeatures (creationDate);
        }
    }
}
