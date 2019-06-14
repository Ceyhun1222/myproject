using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Temporality.Common.Logging;
using TOSSM.Util;
using TOSSM.View;

namespace TOSSM.Common
{
    /// <summary>
    /// EntryPoint is working for manage first and second instances
    /// First instance will run App
    /// Second instance will just send message to make first instance active
    /// and also send arguments to MainManagerWindow.xaml.cs -> HandleParameter method
    /// </summary>
    public static class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Selecting process by name (if run from VS, removing .vshost)
                var current = Process.GetCurrentProcess();
                var processName = current.ProcessName.Replace(".vshost", "");// for debug
                var runningProcess = Process.GetProcesses()
                    .FirstOrDefault(x => (x.ProcessName == processName ||
                                          x.ProcessName == current.ProcessName ||
                                          x.ProcessName == current.ProcessName + ".vshost") && x.Id != current.Id);

                // First instance
                if (runningProcess == null)
                {
                    SplashScreen splashScreen = new SplashScreen("Splash.png");
                    splashScreen.Show(true, true);
                    var app = new App();
                    app.InitializeComponent();
                    app.Run();
                    splashScreen.Close(TimeSpan.Zero);
                    return; // First instance load completes here
                }
                
                // Sending message to first instance and process arguments
                if (args.Length > 0)
                    UnsafeNative.SendMessage(runningProcess.MainWindowHandle, string.Join(" ", args));
                // Just activate first instance window using special string
                else
                {
                    UnsafeNative.SendMessage(runningProcess.MainWindowHandle, "ActivateWindow");
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Show($"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
