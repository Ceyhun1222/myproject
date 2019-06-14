using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.QuickStart.WinControls;

namespace Telerik.Examples.WinControls.DocumentsProcessing.WordGeneration
{
    public partial class Form1 : ExternalExampleHostForm
    {
        public Form1(string themeName)
        {
            this.ThemeName = themeName;
        }

        protected override string GetExecutablePath()
        {
            return @"\..\..\DocumentsProcessing\WordGeneration\bin\WordGeneration.exe";
        }      
    }
}
