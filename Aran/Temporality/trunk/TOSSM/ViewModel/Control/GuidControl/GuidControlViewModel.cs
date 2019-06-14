using System;
using System.Windows;
using System.Windows.Threading;
using MvvmCore;
using TOSSM.View.Dialog;

namespace TOSSM.ViewModel.Control.GuidControl
{
    public class GuidControlViewModel : ViewModelBase
    {
        public static void ShowDialog(Action<GuidControlViewModel> okAction)
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (Action)(
                    () =>
                    {
                        var dialog = new GuidDialog
                        {
                            Owner = Application.Current.MainWindow
                        };

                        var dialogModel = new GuidControlViewModel
                        {
                            CancelCommand = new RelayCommand(
                            s =>
                            {
                                dialog.Close();
                            }),
                        };

                        dialogModel.OkCommand = new RelayCommand(
                            s =>
                            {
                                dialog.Close();

                                if (okAction != null)
                                {
                                    okAction(dialogModel);
                                }
                            });

                        dialog.DataContext = dialogModel;
                        dialog.ShowDialog();
                    }));
        }


        private RelayCommand _okCommand;
        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                _cancelCommand = value;
                OnPropertyChanged("CancelCommand");
            }
        }

        public RelayCommand OkCommand
        {
            get => _okCommand;
            set
            {
                _okCommand = value;
                OnPropertyChanged("OkCommand");
            }
        }


        private string _guidString;
        private bool _isValueSet;

        public bool IsValueSet
        {
            get => _isValueSet;
            set
            {
                _isValueSet = value;
                OnPropertyChanged("IsValueSet");
            }
        }

        public string GuidString
        {
            get => _guidString;
            set
            {
                _guidString = value;
                OnPropertyChanged("GuidString");
                Guid g;
                if (Guid.TryParse(GuidString, out g))
                {
                    GuidValue = g;
                    IsValueSet = true;
                }
                else
                {
                    IsValueSet = false;
                }
            }
        }

        public Guid GuidValue { get; set; }
    }
}
