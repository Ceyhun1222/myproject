using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Aran.Aim.InputForm
{
    public partial class FormReport : Form
    {
        private string _fileName;

        public FormReport ()
        {
            InitializeComponent ();
        }

        public void Show (string caption, List<Exception> excList)
        {
            string tempPath = System.IO.Path.GetTempPath ();
            string fileName = tempPath + "\\MapExportReport.txt";
            var sb = new StringBuilder ();

            foreach (var exc in excList)
            {
                sb.AppendLine (exc.Message);
                if (exc.InnerException != null)
                    sb.AppendLine ("\tDetails: " + exc.InnerException.Message);
            }

            System.IO.File.WriteAllText (fileName, sb.ToString ());

            Text = caption;
            _fileName = fileName;
            ShowDialog ();
        }

        private void linkLabel_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start ("notepad.exe", _fileName);
        }

        private void button1_Click (object sender, EventArgs e)
        {
            Close ();
        }
    }
}
