using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIP.DataSet
{
    static class Program
    {
        public static Splash splashForm = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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

            //run form - time taking operation
            Main mainForm = new Main();
            Application.Run(mainForm);
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
    }
}
