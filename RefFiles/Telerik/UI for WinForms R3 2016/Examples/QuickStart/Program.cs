using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Telerik.QuickStart.WinControls
{
    static class Program
    {
        public static string themeName = "";
        public static string exampleName = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.AssemblyResolve += MyResolveEventHandler;

            //Attempt to load the QuickStart assembly from GAC - for the installation purpose
            Assembly quickStartAssembly = Assembly.Load("QuickStart" + " , Version=" + Telerik.WinControls.VersionNumber.Number + Telerik.WinControls.DesignerConsts.KeyToken);

            //run the manually created instance. This is needed as otherwise the static types of the assemblies will be needed prior we get here
            Form mainForm = Activator.CreateInstance(quickStartAssembly.GetType("Telerik.QuickStart.WinControls.MainForm")) as Form;
           
            Application.ApplicationExit += delegate(object sender, EventArgs e)
            {
                //we have to call this method when the app exits
                mainForm.GetType().GetMethod("ApplicationStop", BindingFlags.Public | BindingFlags.Instance).Invoke(mainForm, null); 
            };

            Application.Run(mainForm);
            
        }

        private static System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = "";
            string neededAssembly = "";

            if (args.Name.Contains(","))
            {
                neededAssembly = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            }
            else
            {
                neededAssembly = args.Name;
            }

            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetCallingAssembly();
         
            foreach (System.Reflection.AssemblyName strAssmbName in objExecutingAssemblies.GetReferencedAssemblies())
            {
                string currentAssembly = strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) + ".dll";

                if (currentAssembly == neededAssembly)
                {
                    strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), neededAssembly);                   
                    if (!System.IO.File.Exists(strTempAssmbPath)) // we are in the case of QSF as exe, so the Path is different
                    {
                        
                        strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\bin\\ReleaseTrial\\");
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
            else
            {
                strTempAssmbPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..\\..\\bin\\ReleaseTrial\\");
                if (System.IO.Directory.Exists(strTempAssmbPath))
                {
                    foreach (string fileName in System.IO.Directory.GetFiles(strTempAssmbPath, "*.dll"))
                    {   
                        if (fileName.EndsWith(neededAssembly))
                        {
                            myAssembly = System.Reflection.Assembly.LoadFrom( Path.Combine(strTempAssmbPath, fileName));
                            break;
                        }
                    }
                }
            }
            return myAssembly;
        }
    }
}