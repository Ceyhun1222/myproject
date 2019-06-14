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
    public partial class EmptyForm : Form
    {
        private Control _workControl;

        public event EventHandler OKClicked;

        public EmptyForm ()
        {
            InitializeComponent ();
        }

        public Control WorkControl
        {
            get { return _workControl; }
            set
            {
                _workControl = value;
                ui_areaPanel.Controls.Add (value);
                Size = value.Size + new Size (10, 100);
            }
        }

        private void Cancel_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (OKClicked != null)
                OKClicked (this, e);
        }
    }
}
