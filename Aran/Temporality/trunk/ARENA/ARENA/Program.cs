using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS;
using ARENA.Util;


namespace ARENA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           // MessageBox.Show("1");
            if (!RuntimeManager.Bind(ProductCode.Desktop))
            {
                if (!RuntimeManager.Bind(ProductCode.Desktop))
                {
                    MessageBox.Show("Unable to bind to ArcGIS runtime. Application will be shut down.");
                    return;
                }
            }
            try
            {
               // MessageBox.Show("2");
                //if (!FileAssociation.IsAssociated)
                {
                    //MessageBox.Show("3");
                    FileAssociation.Associate("Arena File", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\favicon.ico");
                    //MessageBox.Show("4");
                }
            }
            catch (Exception ex) 
            { 
                //MessageBox.Show(ex.Message); 
            }

            //MessageBox.Show("5");
            Application.EnableVisualStyles();
            //MessageBox.Show("6");
            Application.SetCompatibleTextRenderingDefault(false);
            //MessageBox.Show("7");

           // args = new string[] { @"D:\ARINC\DATA\LAT_51_ALL.pdm" };

            ////MessageBox.Show(args[0]);

            MainForm mainForm = new MainForm();
            //MessageBox.Show("8");

            if ((args != null) && (args.Length > 0))
            {
                //MessageBox.Show("9");

                //MessageBox.Show(args[0]);
                mainForm.Tag = args[0];
            }

            //MessageBox.Show("10");

            Application.Run(mainForm);


        }

    }
}