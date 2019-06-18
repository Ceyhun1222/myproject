﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.Themes;

namespace WordConvertion
{
    static class Program
    {
        public static string themeName = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //we need this to load the needed references from other directory (for the standalone QSF)
            AppDomain.CurrentDomain.AssemblyResolve += MyResolveEventHandler;

            Form m = Activator.CreateInstance(System.Reflection.Assembly.GetExecutingAssembly().GetType("WordConvertion.Form1")) as Form;

            //Load the themes so the form can start with the QSF theme
            LoadThemes();
            if (args.Length == 1)
                Program.themeName = args[0];

            //run the manually created instance. This is needed as otherwise the static types of the assemblies will be needed prior we get here
            Application.Run(m);
        }

        private static System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = "";
            string neededAssembly = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (System.Reflection.AssemblyName strAssmbName in objExecutingAssemblies.GetReferencedAssemblies())
            {
                string currentAssembly = strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) + ".dll";

                if (currentAssembly == neededAssembly)
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");

                    if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
                    {
                        strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\..\\..\\bin\\ReleaseTrial\\"); 
                        strTempAssmbPath = System.IO.Path.Combine(strTempAssmbPath, neededAssembly);
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

        private static void LoadThemes()
        {
            new AquaTheme();
            new BreezeTheme();
            new DesertTheme();
            new HighContrastBlackTheme();
            new Office2007BlackTheme();
            new Office2007SilverTheme();
            new Office2010BlackTheme();
            new Office2010SilverTheme();
            new Office2010BlueTheme();
            new Office2013DarkTheme();
            new Office2013LightTheme();
            new TelerikMetroTheme();
            new TelerikMetroBlueTheme();
            new TelerikMetroTouchTheme();
            new VisualStudio2012DarkTheme();
            new VisualStudio2012LightTheme();
            new Windows7Theme();
            new Windows8Theme();
        }
    }
}