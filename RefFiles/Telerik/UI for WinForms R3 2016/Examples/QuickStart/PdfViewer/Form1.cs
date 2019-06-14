using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;
using System.Reflection;
using System.IO;

namespace Telerik.Examples.WinControls.PdfViewer
{
    public partial class Form1 : ExamplesForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Telerik.Examples.WinControls.PdfViewer.Sample.pdf");
            this.radPdfViewer1.LoadDocument(stream);
            this.radPdfViewer1.ShowThubnails();
        }

        protected override void WireEvents()
        {
        }
    }
}
