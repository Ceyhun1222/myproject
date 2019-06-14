using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace Telerik.Examples.WinControls.Tools.VisualStyleBuilder
{
    public partial class Form1 :  Telerik.QuickStart.WinControls.ExternalProcessForm
    {
        protected override string GetExecutablePath()
        {
            if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\Release\VisualStyleBuilder.exe"))
            {
                return @"\..\..\..\..\Bin\Release\VisualStyleBuilder.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe"))
            {
                return @"\..\..\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\Debug\VisualStyleBuilder.exe"))
            {
                return @"\..\..\..\Bin\Debug\VisualStyleBuilder.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\VisualStyleBuilder.exe"))
            {
                return @"\..\..\..\Bin\VisualStyleBuilder.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe")) //qsf as exe case
            {
                return @"\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe";
            }
            else
            {
                RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

            return string.Empty;
        }         
    }
}
