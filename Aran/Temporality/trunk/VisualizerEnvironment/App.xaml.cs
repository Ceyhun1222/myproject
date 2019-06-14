
using System;
using System.Windows;
using System.Windows.Shapes;
using ESRI.ArcGIS.esriSystem;
using VisualizerEnvironment.Properties;
using VisualizerEnvironment.Util;

namespace VisualizerEnvironment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>  
        /// Loads the CSM.dll from ArcGIS BIN path in case the process wants to load the Microsoft library of same name.  
        /// Call this method as early as possible to be effective (e.g. main form OnLoad, program's entry point method).  
        /// </summary>  
        private static void PreLoadEsriCsm(bool debugOnly)
        {
            if (!debugOnly || System.Diagnostics.Debugger.IsAttached)
            {
                string path = System.IO.Path.Combine(ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Path, "Bin\\CSM.dll");
                if (System.IO.File.Exists(path))
                {
                    bool isConflict = false;
                    bool isMatch = false;
                    using (var p = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        foreach (System.Diagnostics.ProcessModule m in p.Modules)
                        {
                            if (m.ModuleName.ToLower() == "csm.dll")
                            {
                                if (path.ToLower() != m.FileName.ToLower())
                                    isConflict = true;
                                else
                                {
                                    isMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isConflict && !isMatch)
                    {
                        System.Diagnostics.Debug.WriteLine("It may be necessary to call this method earlier to be effective.");
                    }
                    if (!isMatch)
                        LoadLibrary(path);
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllname);



        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LicenseInitializer.Instance.InitializeApplication(
              new[] { esriLicenseProductCode.esriLicenseProductCodeStandard },
              new esriLicenseExtensionCode[] { });

            PreLoadEsriCsm(true);

            //do load esri
            var polygon = new Polygon();

            var mainWindow=new MainWindow();
            mainWindow.ShowDialog();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
