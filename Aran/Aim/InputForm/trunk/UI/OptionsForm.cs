using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Controls;
using Aran.AranEnvironment;

namespace Aran.Aim.InputForm
{
    public partial class OptionsForm : Form
    {
        public OptionsForm ()
        {
            InitializeComponent ();
        }


        public event EventHandler OKClicked;

        
        public bool EnableConnectionControl
        {
            get { return ui_dbProviderControl.Enabled; }
            set { ui_dbProviderControl.Enabled = value; }
        }

        public void SetConnection (Connection conn)
        {
            ui_dbProviderControl.SetValue (conn);
        }

        public Connection GetConnection ()
        {
            return ui_dbProviderControl.GetValue ();
        }

        public bool IsConnectionChanged
        {
            get { return ui_dbProviderControl.IsValueChanged; }
        }

     
        private void OK_Click (object sender, EventArgs e)
        {
            if (OKClicked != null)
                OKClicked(this, e);
        }
    }
}
