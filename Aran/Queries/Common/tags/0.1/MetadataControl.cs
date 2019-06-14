using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Aran.Queries.Common
{
    public partial class MetadataControl : UserControl
    {
        private Aim.MD_Metadata.MDMetadata _metadata;

        public MetadataControl ()
        {
            InitializeComponent ();
        }

        public Aim.MD_Metadata.MDMetadata Metadata
        {
            get { return _metadata; }
            set
            {
                _metadata = value;
                ui_showInWebBrowserButton.Enabled = (_metadata != null);
            }
        }

        private void ui_showInWebBrowserButton_Click (object sender, EventArgs e)
        {
            string tmpXmlFileName = Application.StartupPath + "\\TmpXmlFile.xml";
            FileStream stream = new FileStream (tmpXmlFileName, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter (stream);
            streamWriter.Write (_metadata.XmlText);
            streamWriter.Close ();
            stream.Close ();

            Process.Start (
                @"C:\Users\anarnft\AppData\Local\Google\Chrome\Application\chrome.exe",
                tmpXmlFileName);
        }
    }
}
