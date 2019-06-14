using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;


namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel
{

    public class BlockerModel : ViewModelBase
    {

        private bool _isWorking;
        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                Set(() => IsWorking, ref _isWorking, value);
            }
        }


        private Visibility _visibility = Visibility.Collapsed;

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                Set(() => Visibility, ref _visibility, value);
            }
        }
        public void BlockForAction(Action action)
        {
            if (IsWorking)
            {
                action();
                return;
            }

            Block();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (f, e) => Unblock();
            
            worker.DoWork += (f, e) =>
            {
                    // try
                    {
                    // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    // Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    action();
                }
                {
                    Unblock();
                }
            };
            worker.RunWorkerAsync();
        }
        public void BlockForAction(Action action, Action postAction)
        {
            if (IsWorking)
            {
                action();
                postAction();
                return;
            }

            Block();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (f, e) =>
            {
                postAction();
                Unblock();
            };

            worker.DoWork += (f, e) =>
            {
                // try
                {
                    // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    // Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    action();
                }
                {
                    Unblock();
                }
            };
            worker.RunWorkerAsync();
        }
        public void BlockForAction(IList<Action> actions)
        {
            if (IsWorking)
            {
                foreach (var action in actions)
                {
                    action();
                }
                return;
            }

            Block();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (f, e) => Unblock();

            worker.DoWork += (f, e) =>
            {
                // try
                {
                    // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    // Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    foreach (var action in actions)
                    {
                        action();
                    }
                }
                {
                    Unblock();
                }
            };
            worker.RunWorkerAsync();
        }
        public void BlockForAction(IList<Action> actions, Action postAction)
        {
            if (IsWorking)
            {
                foreach (var action in actions)
                {
                    action();
                    postAction();
                }
                return;
            }

            Block();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (f, e) =>
            {
                postAction();
                Unblock();
            };

            worker.DoWork += (f, e) =>
            {
                // try
                {
                    // Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    // Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    foreach (var action in actions)
                    {
                        action();
                    }
                }
                {
                    Unblock();
                }
            };
            worker.RunWorkerAsync();
        }

        
        public void Block()
        {
            IsWorking = true;
            Visibility = Visibility.Visible;
        }

        public void Unblock()
        {
            IsWorking = false;
            Visibility = Visibility.Collapsed;
        }
    }
}

