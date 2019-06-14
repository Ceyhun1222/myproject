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

namespace Telerik.Examples.WinControls.Tools.ThemeViewer
{
    public partial class Form1 : Telerik.QuickStart.WinControls.ExternalProcessForm
    {
        protected override string GetExecutablePath()
        {
            if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\Release\ThemeViewer.exe"))
            {
                return @"\..\..\..\..\Bin\Release\ThemeViewer.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\ReleaseTrial\ThemeViewer.exe"))
            {
                return @"\..\..\..\..\Bin\ReleaseTrial\ThemeViewer.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\Debug\ThemeViewer.exe"))
            {
                return @"\..\..\..\Bin\Debug\ThemeViewer.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\..\Bin\ThemeViewer.exe"))
            {
                return @"\..\..\..\Bin\ThemeViewer.exe";
            }
            else if (File.Exists(Application.StartupPath + @"\..\..\Bin\ReleaseTrial\ThemeViewer.exe")) //qsf as exe case
            {
                return @"\..\..\..\Bin\ReleaseTrial\ThemeViewer.exe";
            }
            else
            {
                RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

            return string.Empty;
        }         
    }
}
