using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using MapEnv.Forms;
using System.IO;
using System.Threading;

namespace MapEnv
{
	static class Program
	{
		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
		static extern bool SetForegroundWindow(IntPtr hWnd);

	    public static Splash splashForm = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
		static void Main(string[] args)
		{
			string procName = Process.GetCurrentProcess().ProcessName;
			var procArr = Process.GetProcessesByName(procName);

            if (procArr.Length > 1)
                return;

			Globals.ClearTempShapefiles();

            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            // Show Splash screen - begin
		    Thread splashThread = new Thread(new ThreadStart(
		        delegate
		        {
		            splashForm = new Splash();
		            Application.Run(splashForm);
		        }
		    ));
		    splashThread.SetApartmentState(ApartmentState.STA);
		    splashThread.Start();
            // Show Splash screen - end

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			//Application.ThreadException += Application_ThreadException;

			var ci = new CultureInfo("en-US");
			ci.NumberFormat.NumberDecimalSeparator = ".";
			Application.CurrentCulture = ci;

            //var mainForm = new MainForm();
            //string fileName;
            //ParseArguments(args, out fileName);
            ////if (fileName != null)
            ////    mainForm.LoadFile (fileName);
            //Application.Run(mainForm);

            CopyUserFiles();

            MainForm mainForm;

            try
            {
                mainForm = new MainForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IAIM Environment - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(mainForm);

            //Application.Run (new TestForm_Filter ());
            //Application.Run (new Tests.TestForm_PropertySelector ());
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

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			var aranEx = e.Exception as Aran.AranEnvironment.AranException;

			if (aranEx != null)
			{
				MessageBox.Show(
					aranEx.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK,
					(aranEx.ExceptionType ==  Aran.AranEnvironment.ExceptionType.Warning ? MessageBoxIcon.Warning : MessageBoxIcon.Error));
				return;
			}

            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		static void Application_ApplicationExit(object sender, EventArgs e)
		{
			//Process.GetCurrentProcess ().Kill();
		}

		static void ParseArguments(string[] args, out string fileName)
		{
			fileName = null;

			if (args != null && args.Length > 1)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string arg = args[i];

					if (arg.StartsWith("--") && arg.Length > 2)
					{
						char opt = arg[2];

						switch (opt)
						{
							case 'p':
								{
									fileName = args[i + 1].Trim();
									if (fileName.Length > 1 && fileName[0] == '\"' && fileName[fileName.Length - 1] == '\"')
										fileName = fileName.Remove(0).Remove(fileName.Length - 1);
								}
								break;
						}
					}
				}
			}
		}

		// Find window by Caption, and wait 1/2 a second and then try again.
		static IntPtr FindWindow(string windowName, bool wait)
		{
			IntPtr hWnd = FindWindow(null, windowName);
			while (wait && hWnd == IntPtr.Zero)
			{
				System.Threading.Thread.Sleep(500);
				hWnd = FindWindow(null, windowName);
			}

			return hWnd;
		}

		// THE FOLLOWING METHOD REFERENCES THE SetForegroundWindow API
		static bool BringWindowToTop(string windowName, bool wait)
		{
			IntPtr hWnd = FindWindow(windowName, wait);
			if (hWnd != IntPtr.Zero)
			{
				return SetForegroundWindow(hWnd);
			}
			return false;
		}

        static void CopyUserFiles()
        {
            //***
            //*** Copy if not exists
            //***

            var srsDir = Path.Combine(Application.StartupPath, "UAD-RISK");

            if (!Directory.Exists(srsDir))
                return;

            var destDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RISK");

            var n = srsDir.Length + 1;
            var srsFiles = Directory.GetFiles(srsDir, "*.*", SearchOption.AllDirectories).Select(s => s.Substring(n)).ToList();

            n = destDir.Length + 1;
			
			List<string> destFiles = null;
			if (!Directory.Exists(destDir))
				destFiles = new List<string>();
			else
				destFiles = Directory.GetFiles(destDir, "*.*", SearchOption.AllDirectories).Select(s => s.Substring(n)).ToList();

            foreach (var srsFile in srsFiles)
            {
                if (!destFiles.Contains(srsFile))
                {
                    var destFile = Path.Combine(destDir, srsFile);
                    var destFileDir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(destFileDir))
                        Directory.CreateDirectory(destFileDir);

                    File.Copy(Path.Combine(srsDir, srsFile), destFile);
                }
            }

            ////***
            ////*** Force copy
            ////***

            //srsDir = Path.Combine(Application.StartupPath, "UAD-RISK-FRC");
            //n = srsDir.Length + 1;
            //srsFiles = Directory.GetFiles(srsDir, "*.*", SearchOption.AllDirectories).Select(s => s.Substring(n)).ToList();

            //foreach (var srsFile in srsFiles)
            //{
            //    var destFile = Path.Combine(destDir, srsFile);
            //    var destFileDir = Path.GetDirectoryName(destFile);
            //    if (!Directory.Exists(destFileDir))
            //        Directory.CreateDirectory(destFileDir);

            //    File.Copy(Path.Combine(srsDir, srsFile), destFile, true);
            //}
        }
	}
}
