using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public abstract class UpdatableToolViewModel : ToolViewModel
    {

        public int SleepTime { get; set; } = 2000;
       // public ThreadPriority Priority { get; set; } = ThreadPriority.Highest;

        protected UpdatableToolViewModel(string name) : base(name)
        {
         
        }

        public override void OnClosed()
        {
            base.OnClosed();
            IsTerminated = true;
        }

        protected override void OnDispose()
        {
            IsTerminated = true;
        }

        public bool IsTerminated { get; set; }



       // private Thread _loaderThread;
        

        public async void LoaderFunction()
        {

            await Task.Run(async () =>
            {
                while (!IsTerminated)
                    await Task.Delay(SleepTime).ContinueWith(s => LoadFunction());
            });
        }

        protected abstract void LoadFunction();

        public void StopLoading()
        {
            IsTerminated = true;
          //  _loaderThread?.Join();
          //  _loaderThread = null;
        }

        public void StartLoading()
        {
            IsTerminated = false;
            LoaderFunction();
           // _loaderThread.Start();
        }
    }
}