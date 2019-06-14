using ESRI.ArcGIS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Threading;

namespace HelperDialog
{
    public class SplashManager
    {
        private Thread thread;
        private bool canAbortThread = false;
        private SplashScreen window;
        IApplication _app;
        public SplashManager(IApplication app)
        {
            _app = app; 
        }

        public void BeginWaiting()
        {
            this.thread = new Thread(this.RunThread);
            this.thread.IsBackground = true;
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.Start();
        }
        public void EndWaiting()
        {
            if (this.window != null)
            {
                this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                { this.window.Close(); }));
                while (!this.canAbortThread) { };
            }
            this.thread.Abort();
        }

        public void RunThread()
        {
            this.window = new SplashScreen();
            var parentHandle = new IntPtr(_app.hWnd);
            var helper = new WindowInteropHelper(this.window) { Owner = parentHandle };

            
            this.window.Closed += new EventHandler(waitingWindow_Closed);
            this.window.ShowDialog();
        }
        //public void ChangeStatus(string text)
        //{
        //    if (this.window != null)
        //    {
        //        this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
        //        { this.window.tbStatus.Text = text; }));
        //    }
        //}
        //public void ChangeProgress(double Value)
        //{
        //    if (this.window != null)
        //    {
        //        this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
        //        { this.window.pProgress.Value = Value; }));
        //    }
        //}
        //public void SetProgressMaxValue(double MaxValue)
        //{
        //    Thread.Sleep(100);
        //    if (this.window != null)
        //    {
        //        this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
        //        {
        //            this.window.pProgress.Minimum = 0;
        //            this.window.pProgress.Maximum = MaxValue;
        //        }));
        //    }
        //}
        void waitingWindow_Closed(object sender, EventArgs e)
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
            this.canAbortThread = true;
        }
    }
}
