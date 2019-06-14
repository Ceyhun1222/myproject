using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;

namespace Telerik.Examples.WinControls.Integration.Products.AllTelerikProducts
{
    public partial class Form1 : ExternalProcessForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override string GetExecutablePath()
        {
            return @"http://www.telerik.com/demos/";
        }
    }
}
