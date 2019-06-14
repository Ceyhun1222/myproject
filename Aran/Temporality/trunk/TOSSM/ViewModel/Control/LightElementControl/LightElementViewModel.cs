using System;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim.Enums;
using MvvmCore;
using TOSSM.View.Dialog;

namespace TOSSM.ViewModel.Control.LightElementControl
{
    public class LightElementViewModel : ViewModelBase
    {
        public static void ShowDialog(Action<LightElementViewModel> okAction)
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (Action)(
                    () =>
                    {
                        var dialog = new LightElementDialog()
                        {
                            Owner = Application.Current.MainWindow
                        };

                        var dialogModel = new LightElementViewModel
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

                                okAction?.Invoke(dialogModel);
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


        public CodeColour[] Colors => (CodeColour[]) Enum.GetValues(typeof(CodeColour));

        private CodeColour _color;


        public CodeColour Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }

        }
    }
}