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
    public partial class LogsForm : Form
    {
        private List<string> _logs;

        public LogsForm ()
        {
            InitializeComponent ();

            _logs = new List<string> ();
        }

        public void AddLogs (string [] logs)
        {
            _logs.AddRange (logs);

            ui_linesTB.Lines = _logs.ToArray ();
        }

        public bool HasLog
        {
            get { return (_logs.Count > 0); }
        }

        private void ui_closeButton_Click (object sender, EventArgs e)
        {
            Close ();
        }
    }
}
