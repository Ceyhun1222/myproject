using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using Aran.AranEnvironment;

namespace MapEnv
{
    public partial class NewProConnectionPage : UserControl
    {
        public NewProConnectionPage()
        {
            InitializeComponent();

            ui_dbProviderCont.SetValue(Globals.Settings.DbConnection);
        }


        public Connection GetConnection()
        {
            return ui_dbProviderCont.GetValue();
        }
    }
}
