using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Aran.Temporality.Common.Logging;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public interface IActivate
    {
        void ActivateMe();
    }

    public class BlockerModel : ViewModelBase
    {
        private DispatcherTimer _dispatcherTimer = null;

        public IActivate ActivatingObject { get; set; }
        private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        private DispatcherTimer DispatcherTimer
        {
            get
            {
                if (_dispatcherTimer == null)
                {
                    _dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                    _dispatcherTimer.Tick += (a, b) =>
                    {
                        _dispatcherTimer.Stop();


                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action)(
                                () =>
                                {
                                    OnPropertyChanged("Visibility");
                                }));
                    };
                }

                return _dispatcherTimer;
            }
        }

        private bool _isBlocked;
        public bool IsBlocked
        {
            get => _isBlocked;
            set
            {
                if (_isBlocked == value) return;
                _isBlocked = value;

                if (IsBlocked)
                {
                    DispatcherTimer.Stop();
                    DispatcherTimer.Start();
                }
                else
                {
                    DispatcherTimer.Stop();
                    OnPropertyChanged("Visibility");
                }
            }
        }

        private void Activate()
        {
            ActivatingObject?.ActivateMe();
        }


        private async Task BlockForActionAsync(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                        Activate();
                        action();
                        //Action lastAction = null;
                        //while (_actions.TryDequeue(out lastAction))
                        //{
                        //    lastAction();
                        //}
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(typeof(BlockerModel)).Error(ex, ex.Message);
                    throw;
                }
                finally
                {
                    Unblock();
                }
            }).ConfigureAwait(false);
        }

        private async Task BlockForActionWithouUnblockAsync(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                    Activate();
                    action();
                }catch(Exception ex)
                {
                    LogManager.GetLogger(typeof(BlockerModel)).Error(ex, ex.Message);
                    throw;
                }
            }).ConfigureAwait(false);
        }

        public async Task BlockForAction(Action action)
        {
            if (IsBlocked)
            {
               // Activate();
               // action();
                await BlockForActionWithouUnblockAsync(action);
                //_actions.Enqueue(action);
                return;
            }

            Block();
            await BlockForActionAsync(action);
            //var worker = new BackgroundWorker();
            //worker.RunWorkerCompleted += (f, e) => Unblock();
            //worker.DoWork += (f, e) =>
            //{
            //   // try
            //    {
            //        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            //        Activate();
            //        action();
            //    }
            //    //catch (Exception)
            //    //{
            //    //    throw;
            //    //}
            //    //finally
            //    {
            //        Unblock();
            //    }
            //};
            //worker.RunWorkerAsync();
        }

        public void Block()
        {
            IsBlocked = true;
        }

        public void Unblock()
        {
            IsBlocked = false;
        }

        public Visibility Visibility
        {
            get { return IsBlocked ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}
