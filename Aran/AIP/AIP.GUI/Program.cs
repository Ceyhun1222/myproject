using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using AIP.GUI.Forms;
using Telerik.WinControls.UI;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using AIP.BaseLib.Class;
using AIP.DB;

namespace AIP.GUI
{
    static class Program
    {
        public static Splash splashForm = null;
        public static string Name = "eAIP Production System";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string argString = string.Join(" ", args);

            // If a mutex with the name below already exists, 
            // one instance of the application is already running 
            Mutex singleMutex = new Mutex(true, Name, out var isNewInstance);
            if (isNewInstance)
            {
                //show splash
                Thread splashThread = new Thread(new ThreadStart(
                    delegate
                    {
                        splashForm = new Splash();
                        Application.Run(splashForm);
                    }
                ));
                splashThread.SetApartmentState(ApartmentState.STA);
                splashThread.Start();

                // To use . in decimal instead of ,
                CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                // This is just for UI controls - can be commented
                CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

                CheckConfig();
                CheckDependencies();

                //run form - time taking operation
                Main mainForm = new Main(argString);
                Application.Run(mainForm);
            }
            else
            {
                // Find the window with the name of the main form
                IntPtr ptrWnd = NativeMethods.FindWindow(Name);
                if (ptrWnd != IntPtr.Zero)
                {
                    IntPtr ptrCopyData = IntPtr.Zero;
                    try
                    {
                        // Create the data structure and fill with data
                        NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT();
                        copyData.dwData = new IntPtr(2);    // Just a number to identify the data type
                        copyData.cbData = argString.Length + 1;  // One extra byte for the \0 character
                        copyData.lpData = Marshal.StringToHGlobalAnsi(argString);

                        // Allocate memory for the data and copy
                        ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                        Marshal.StructureToPtr(copyData, ptrCopyData, false);

                        // Send the message
                        NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                    }
                    finally
                    {
                        // Free the allocated memory after the contol has been returned
                        if (ptrCopyData != IntPtr.Zero)
                            Marshal.FreeCoTaskMem(ptrCopyData);
                    }
                }
            }
        }

        private static void CheckDependencies()
        {
            try
            {
                var javaStatus = CheckJavaInstallation();
                var fopStatus = CheckFOPInstallation();
                if (!javaStatus && !fopStatus)
                {
                    MessageBox.Show(
                        @"JAVA and FOP are not installed on the your computer, please contact with your system administrator, otherwise HTML and PDF generation will not be possible",
                        @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!javaStatus)
                {
                    MessageBox.Show(
                        @"JAVA is not installed on the your computer, please contact with your system administrator, otherwise HTML and PDF generation will not be possible",
                        @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!fopStatus)
                {
                    MessageBox.Show(
                        @"FOP is not installed on the your computer, please contact with your system administrator, otherwise HTML and PDF generation will not be possible",
                        @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static bool CheckJavaInstallation()
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("java", "-version");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                Process proc = new Process {StartInfo = procStartInfo};
                proc.Start();

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CheckFOPInstallation()
        {
            try
            {
                return File.Exists(@"C:\fop\fop\fop.bat");
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void CloseSplash()
        {
            //close splash
            if (splashForm == null)
            {
                return;
            }

            splashForm.Invoke(new Action(splashForm.Close));
            splashForm.Dispose();
            splashForm = null;
        }

        public static void HideSplash()
        {
            splashForm?.Invoke(new Action(splashForm.Hide));
        }

        public static void ShowSplash()
        {
            splashForm?.Invoke(new Action(splashForm.Show));
        }

        public static string Version()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return $@"{v.Major}.{v.Minor}.{v.Build}";
        }

        private static void CheckConfig()
        {
            try
            {
                List<string> UpgradeLayoutVersions = new List<string> { "1.0.19", "1.0.20", "1.0.22" };
                if (Properties.Settings.Default.CurrentVersion != Version())
                {
                    ClearEntityFrameworkCache(); // For stable work, clear EF cache when new version of AIP released
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.CurrentVersion = Version();
                    if (UpgradeLayoutVersions.Contains(Version()))
                    {
                        ResetLayout();
                    }
                    Properties.Settings.Default.Save();
                }
                if (Properties.Settings.Default.PCS != "setup")
                {
                    BaseLib.Class.Encrypt.Protect(AppDomain.CurrentDomain.SetupInformation.ApplicationName);
                    BaseLib.Class.Encrypt.ProtectBaseLib();
                }
                else
                {
                    BaseLib.Class.Encrypt.UnProtect(AppDomain.CurrentDomain.SetupInformation.ApplicationName);
                    BaseLib.Class.Encrypt.ProtectBaseLib(false);
                }
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private static void ResetLayout()
        {
            try
            {
                if(File.Exists(Lib.LayoutFile)) File.Delete(Lib.LayoutFile);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void ClearEntityFrameworkCache()
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(DbContextConfiguration.EFCachePath);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
