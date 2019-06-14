using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Telerik.Examples.WinControls.Tools.ThemeViewer
{
    public partial class LaunchForm : Telerik.QuickStart.WinControls.CustomThemeExamplesLauncherForm
    {
        public LaunchForm()
        {
            InitializeComponent();

            this.pictureBoxLaunchExample.ButtonImage = Telerik.Examples.WinControls.Properties.Resources.launch;
        }
    }
}
