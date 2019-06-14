using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.PropertyPrecision;
using Aran.Temporality.Common.Entity;
using MvvmCore;
using TOSSM.Util;
using TOSSM.ViewModel.Tool.PropertyPrecision.Editor;

namespace TOSSM.ViewModel.Tool.PropertyPrecision
{
    public class PrecisionEditorToolViewModel : PrecisionSubEditorViewModel
    {
        private Visibility _dataGridVisibility = Visibility.Hidden;
        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged("DataGridVisibility");
            }
        }

        private FeatureType? _featureType = Aran.Aim.FeatureType.AirportHeliport;
        public FeatureType? FeatureType
        {
            get => _featureType;
            set
            {
                _featureType = value;
                if (FeatureType != null && !string.IsNullOrEmpty(SelectedConfiguration))
                {
                    var conf = DataProvider.Configurations.First(t => t.Name==SelectedConfiguration);
                    var features = new FeaturesPrecisionConfiguration();
                    features.FromBytes(conf.Data);

                    ComplexPropertyConfiguration specific;
                    if (!features.FeatureConfigurations.TryGetValue((int)FeatureType, out specific))
                    {
                        specific = new ComplexPropertyConfiguration();
                    }
                    PropertyConfiguration = specific;
                    EditedFeature = (int)FeatureType;

                    DataGridVisibility = Visibility.Visible;
                }
                else
                {
                    DataGridVisibility = Visibility.Hidden;
                }
              
                OnPropertyChanged("FeatureType");
            }
        }

        private string _selectedConfiguration;
        public string SelectedConfiguration
        {
            get => _selectedConfiguration;
            set
            {
                _selectedConfiguration = value;
                OnPropertyChanged("SelectedConfiguration");

                DataGridVisibility = Visibility.Hidden;
                FeatureType = null;
            }

        }

        private List<string> _configurations;
        public List<string> Configurations => _configurations??(_configurations=new List<string>
        {
            ConfigurationName.ChartingResolutionConfiguration,
            ConfigurationName.PublicationResolutionConfiguration
        });

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            ColumnCollection.Clear();
            ColumnCollection.Add(PropertyColumn);
            ColumnCollection.Add(FormatColumn);

            BlockerModel.BlockForAction(
                () =>
                {

                    Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Background,
                        (Action) (
                            () =>
                            {
                                if (Configurations.Count > 0)
                                {
                                    SelectedConfiguration = Configurations.Last();
                                }
                            }));
                }
                );
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get { return _saveCommand??(_saveCommand=new RelayCommand(
                t =>
                {
                    BlockerModel.BlockForAction(
                        () =>
                        {
                            if (FeatureType != null)
                            {
                                var conf = DataProvider.Configurations.First(t2 => t2.Name == SelectedConfiguration); var features = new FeaturesPrecisionConfiguration();
                                features.FromBytes(conf.Data);
                                features.FeatureConfigurations[(int)FeatureType] = PropertyConfiguration;
                                conf.Data = features.ToBytes();
                                DataProvider.UpdatePrecisionConfiguration(conf);
                            }
                        });
                },
                t => SelectedConfiguration != null && FeatureType!=null));
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(
                            () =>
                            {
                                SelectedConfiguration = SelectedConfiguration;
                            });
                    },
                    t => SelectedConfiguration != null ));
            }
        }
    }
}
