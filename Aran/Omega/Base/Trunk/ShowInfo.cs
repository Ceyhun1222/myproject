using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Omega.Models;
using Aran.Omega.View;

namespace Aran.Omega
{
    class ShowSurfaceInfo
    {
        public static void Show(DrawingSurface olsSurface)
        {
            var abstractView = new AbstractView();
            abstractView.LoadSurfaces(olsSurface.SurfaceBase.PropertyList, olsSurface.ViewCaption);

            var helper = new WindowInteropHelper(abstractView);
            ElementHost.EnableModelessKeyboardInterop(abstractView);
            helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
            abstractView.ShowInTaskbar = false;

            if (GlobalParams.OLSWindow != null)
            {
                if (GlobalParams.OLSWindow.Left < abstractView.Width)
                    abstractView.Left = GlobalParams.OLSWindow.Left + GlobalParams.OLSWindow.Width + 4;
                else
                    abstractView.Left = GlobalParams.OLSWindow.Left - abstractView.Width - 4;

                abstractView.Top = GlobalParams.OLSWindow.Top + 5;
            }
            else
            {
                if (GlobalParams.ETODWindow.Left < abstractView.Width)
                    abstractView.Left = GlobalParams.ETODWindow.Left + GlobalParams.ETODWindow.Width + 4;
                else
                    abstractView.Left = GlobalParams.ETODWindow.Left - abstractView.Width - 4;

                abstractView.Top = GlobalParams.ETODWindow.Top + 5;

            }
            // hide from taskbar and alt-tab list
            abstractView.Show();

        }
    }
}
