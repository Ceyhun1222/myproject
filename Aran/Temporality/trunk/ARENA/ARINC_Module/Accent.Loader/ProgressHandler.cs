using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Xaml;
using App = System.Windows.Application;

namespace Process.Loader
{
    public class ProgressHandler 
    {
        public static MockData MockData { get; set; }
        static  App app;
        static ProgressWindow pgWin;
        static bool IsIntermadiate = false;
        public static bool IsActive = true;


        static ProgressHandler()
        {
            if ( MockData == null )
                MockData = new MockData();
        }

        ~ProgressHandler()
        {
            GC.SuppressFinalize(this);
        }



        public static void Run(bool Intermadiate = false)
        {
            if ( !IsActive )
                return;

            IsIntermadiate = Intermadiate;
            var th = new Thread((ThreadStart) StartProgress) { ApartmentState = System.Threading.ApartmentState.STA, IsBackground = false };
            th.Start();
        }

        public static void Run(string status, int percent, bool Intermadiate = false)
        {
            MockData.Text = status;
            MockData.Percentage = percent;

            Run(Intermadiate);
        }

        public static void Stop()
        {
            if ( !IsActive )
                return;

            MockData.Percentage = 100;


            // if ( pgWin != null )
            ///     pgWin.Dispatcher.InvokeShutdown();
        }

        public static void Close()
        {
            if ( pgWin != null )
            {
                pgWin.Dispatcher.Invoke(new Action(() => pgWin.Close()));
            }
        }

        public static void SetState(string status, int percent)
        {
            if ( !IsActive )
                return;

            MockData.Text = status;
            MockData.Percentage = percent;
        }

        private static void StartProgress()
        {
            if ( !IsActive )
                return;

            pgWin = pgWin ?? new ProgressWindow(MockData);
            app = app ?? new App();

            pgWin.SetProgressStyle(IsIntermadiate);

            if ( pgWin.Dispatcher.Thread != Thread.CurrentThread )
            {
                pgWin.ShowWindow();
            }
            else
                app.Run(pgWin);
        }
    }
}
