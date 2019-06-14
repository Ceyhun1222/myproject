using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.Examples.WinControls.Map.KML;
using Telerik.WinControls;
using Telerik.WinControls.Themes;

namespace Map
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //reuse the database from Examples\DataSources
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (Path.GetDirectoryName(executable));

            string testPathForDebuggingCase = Path.GetFullPath(Path.Combine(path, @"..\..\..\Examples\DataSources"));
            string fileName = "Nwind.mdb";

            if (File.Exists(Path.Combine(testPathForDebuggingCase, fileName)))
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", testPathForDebuggingCase);
            }

            string testPathForInstallationCase = Path.GetFullPath(Path.Combine(path, @"..\..\QuickStart\DataSources"));

            if (File.Exists(Path.Combine(testPathForInstallationCase, fileName)))
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", testPathForInstallationCase);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //we need this to load the needed references from other directory (for the standalone QSF)
            AppDomain.CurrentDomain.AssemblyResolve += MyResolveEventHandler;

            TelerikMetroTheme theme = new TelerikMetroTheme();
            ThemeResolutionService.ApplicationThemeName = theme.ThemeName;

            Application.Run(new Form1());
        }

        private static System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = "";
            string neededAssembly = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll" : args.Name + ".dll";
            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (System.Reflection.AssemblyName strAssmbName in objExecutingAssemblies.GetReferencedAssemblies())
            {
                string currentAssembly = strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) + ".dll";

                if (currentAssembly == neededAssembly)
                {
                    strTempAssmbPath = Path.Combine(Directory.GetCurrentDirectory(), args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");

                    if (!File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
                    {
                        strTempAssmbPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\bin\\ReleaseTrial\\");
                        strTempAssmbPath = Path.Combine(strTempAssmbPath, neededAssembly);
                    }
                    break;
                }
            }

            System.Reflection.Assembly myAssembly = null;

            if (!string.IsNullOrEmpty(strTempAssmbPath))
            {
                myAssembly = System.Reflection.Assembly.LoadFrom(strTempAssmbPath);
            }
            return myAssembly;
        }
    }
}
