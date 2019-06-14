using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim.Features;
using MvvmCore;
using TOSSM.View.Dialog;

namespace TOSSM.ViewModel.Control.LightElementControl
{
    public class FeatureSelectionViewModel : ViewModelBase
    {
        public static void ShowDialog(Action<FeatureSelectionViewModel> okAction, List<Feature> features)
        {
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                (Action)(
                    () =>
                    {
                        var dialog = new FeatureSelectionDialog()
                        {
                            Owner = Application.Current.MainWindow
                        };

                        var dialogModel = new FeatureSelectionViewModel()
                        {
                            CancelCommand = new RelayCommand(
                                s =>
                                {
                                    dialog.Close();
                                }),
                        };

                        dialogModel.Load(features);

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


        public void Load(List<Feature> feautres)
        {
            Features.Clear();
            feautres.ForEach(t => Features.Add(t));
        }

        private ObservableCollection<Feature> _features = new ObservableCollection<Feature>();
        public ObservableCollection<Feature> Features
        {
            get => _features;
            set
            {
                _features = value;
                OnPropertyChanged(nameof(Features));
            }
        }

        private Feature _feature;

        public Feature Feature
        {
            get => _feature;
            set
            {
                _feature = value;
                OnPropertyChanged(nameof(Feature));
            }

        }
    }
}