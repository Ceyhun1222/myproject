using System;
using System.Windows;
using System.Windows.Threading;
using MvvmCore;
using TOSSM.View.Dialog;

namespace TOSSM.ViewModel.Control.EventView
{
    public class EventViewModel : ViewModelBase
    {
        private RelayCommand _cancelCommand;

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                if (_endDate < StartDate)
                {
                    _endDate = StartDate;
                } 
                
                OnPropertyChanged("EndDate");
            }
        }

        private DateTime _initialStartDate;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                
                OnPropertyChanged("StartDate");

                if (EndDate < StartDate)
                {
                    EndDate = StartDate;
                } 
                
            }
        }

        public Visibility EndDateVisibility
        {
            get => _endDateVisibility;
            set
            {
                _endDateVisibility = value;
                OnPropertyChanged("EndDateVisibility");
            }
        }

        public bool IsStartEnabled
        {
            get => _isStartEnabled;
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged("IsStartEnabled");
            }
        }


        public bool IsEndEnabled
        {
            get => _isEndEnabled;
            set
            {
                _isEndEnabled = value;
                OnPropertyChanged("IsEndEnabled");
            }
        }

        public int InterpretationIndex
        {
            get => _interpretationIndex;
            set
            {
                _interpretationIndex = value;
                OnPropertyChanged("InterpretationIndex");
                if (InterpretationIndex == 0)
                {
                    StartDate = _initialStartDate;
                }

                IsStartEnabled = InterpretationIndex == 1;
                IsValidEnd = InterpretationIndex == 1;
                IsEndEnabled = InterpretationIndex == 1;
            }
        }

        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set
            {
                _cancelCommand = value;
                OnPropertyChanged("CancelCommand");
            }
        }

        public bool IsValidEnd
        {
            get => _isValidEnd;
            set
            {
                _isValidEnd = value;
                OnPropertyChanged("IsValidEnd");

                EndDateVisibility = _isValidEnd ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private RelayCommand _okCommand;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _interpretationIndex;
        private Visibility _endDateVisibility=Visibility.Hidden;
        private bool _isValidEnd;
        private bool _isStartEnabled;
        private bool _isEndEnabled;

        public RelayCommand OkCommand
        {
            get => _okCommand;
            set
            {
                _okCommand = value;
                OnPropertyChanged("OkCommand");
            }
        }


        public static void ShowDialog(DateTime start, Action<EventViewModel> okAction)
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (Action) (
                    () =>
                    {
                        var dialog = new EventDialog
                        {
                            Owner = Application.Current.MainWindow
                        };


                        var dialogModel = new EventViewModel
                        {
                            _initialStartDate = start,
                            StartDate = start,
                            EndDate = start,
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
                            },
                            s =>
                            {
                                if (dialogModel.InterpretationIndex == 1)
                                    return dialogModel.StartDate < dialogModel.EndDate;
                                return true;
                            });


                        dialog.DataContext = dialogModel;
                        dialog.ShowDialog();
                     

                    }));
           }
    }
}
