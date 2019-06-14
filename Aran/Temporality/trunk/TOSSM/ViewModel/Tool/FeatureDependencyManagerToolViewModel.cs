using System;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.Util;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private FeatureDependencyConfiguration _entity;
        public FeatureDependencyConfiguration Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                if (Entity==null)
                {
                    return;
                }
                Name = Entity.Name;
                Feature = (FeatureType)Entity.RootFeatureType;
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        private FeatureType _feature;
        public FeatureType Feature
        {
            get => _feature;
            set
            {
                _feature = value;
                OnPropertyChanged("Feature");
            }
        }

    }



    public class FeatureDependencyManagerToolViewModel : ToolViewModel
    {
        #region Ctor

        public static string ToolContentId = "Feature Dependencies";

        public FeatureDependencyManagerToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/dependency.png", UriKind.RelativeOrAbsolute);

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;
            ReloadData();
        }

        public void ReloadData()
        {
            BlockerModel.BlockForAction(
               () =>
               {

                   var depList = DataProvider.GetFeatureDependencies();
                   foreach (FeatureDependencyConfiguration dep in depList)
                   {
                       Configurations.Add(new ConfigurationViewModel
                       {
                           Entity = dep
                       });
                   }

               });

        }

        #endregion

        #region BlockerModel

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set
            {
                _blockerModel = value;
                SelectedConfigurationViewModel.BlockerModel = BlockerModel;
            }
        }

        #endregion

        #region Configurations

        private ConfigurationViewModel _selectedConfiguration;
        public ConfigurationViewModel SelectedConfiguration
        {
            get => _selectedConfiguration;
            set
            {
                _selectedConfiguration = value;
                OnPropertyChanged("SelectedConfiguration");

                SelectedConfigurationViewModel.Mode = ConfiguratorMode.ReadOnly;
                SelectedConfigurationViewModel.LastState = null;
                
                if (SelectedConfiguration != null )
                {
                    //set name
                    
                    SelectedConfigurationViewModel.Name = SelectedConfiguration.Name;
                }

                if (SelectedConfiguration != null && SelectedConfiguration.Entity != null && SelectedConfiguration.Entity.Data!=null)
                {
                    SelectedConfigurationViewModel.IsEnabled = true;
                    //can load
                    SelectedConfigurationViewModel.Deserialize(SelectedConfiguration.Entity.Data);
                }
                else
                {
                    //init default
                    SelectedConfigurationViewModel.Clear();
                    SelectedConfigurationViewModel.IsEnabled = false;
                    if (SelectedConfiguration != null)
                    {
                        SelectedConfigurationViewModel.FeatureType = SelectedConfiguration.Feature;
                    }
                }
            }
        }

        private MtObservableCollection<ConfigurationViewModel> _configurations;
        public MtObservableCollection<ConfigurationViewModel> Configurations
        {
            get => _configurations??(_configurations=new MtObservableCollection<ConfigurationViewModel>());
            set => _configurations = value;
        }

        private FeatureDependencyConfigurationViewModel _selectedConfigurationViewModel;
        public FeatureDependencyConfigurationViewModel SelectedConfigurationViewModel => _selectedConfigurationViewModel??(_selectedConfigurationViewModel=new FeatureDependencyConfigurationViewModel
        {
            SaveCommand = SaveCommand,
            BlockerModel = BlockerModel
        });

        #endregion

        #region Commands

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get { return _saveCommand??(_saveCommand=new RelayCommand(
                t => BlockerModel.BlockForAction(
                    ()=>
                        {
                            SelectedConfiguration.Name = SelectedConfigurationViewModel.Name;
                            SelectedConfiguration.Feature = SelectedConfigurationViewModel.RootFeatureType;

                            SelectedConfiguration.Entity.RootFeatureType = (int)SelectedConfigurationViewModel.RootFeatureType;
                            SelectedConfiguration.Entity.Name = SelectedConfigurationViewModel.Name;
                            SelectedConfiguration.Entity.Data = SelectedConfigurationViewModel.Serialize();
                            CurrentDataContext.CurrentNoAixmDataService.UpdateFeatureDependencyConfiguration(SelectedConfiguration.Entity);

                            var bytes=SelectedConfigurationViewModel.Serialize();
                            SelectedConfigurationViewModel.Mode = ConfiguratorMode.ReadOnly;
                            SelectedConfigurationViewModel.Deserialize(bytes);
                        }),
                t => SelectedConfigurationViewModel.Mode==ConfiguratorMode.ReadWrite)); 
            }
        }


        private RelayCommand _newConfigurationCommand;
        public RelayCommand NewConfigurationCommand
        {
            get { return _newConfigurationCommand??(_newConfigurationCommand=new RelayCommand(
                t=> BlockerModel.BlockForAction(
                    ()=>
                        {
                            int i = 1;
                            var possibleName = "New configuration";
                            while (i < 100 && Configurations.Any(t2 => t2.Name == possibleName))
                            {
                                i++;
                                possibleName = "New configuration (" + i + ")";
                            }
                            var template =
                                CurrentDataContext.CurrentNoAixmDataService.GetDataSourceTemplateById(
                                    DataSourceTemplateId);

                            var depViewModel = new ConfigurationViewModel
                                                   {
                                                       Entity = new FeatureDependencyConfiguration
                                                                    {
                                                                        Name = possibleName,
                                                                        RootFeatureType = (int)FeatureType.AirportHeliport,
                                                                        DataSourceTemplate = template
                                                                    }
                                                   };
                            
                            Configurations.Add(depViewModel);
                            SelectedConfiguration = depViewModel;

                            SelectedConfigurationViewModel.Mode = ConfiguratorMode.ReadWrite;
                            SelectedConfigurationViewModel.FeatureType = depViewModel.Feature;

                            SelectedConfiguration.Entity.Data = SelectedConfigurationViewModel.Serialize();

                            //save to db
                            SelectedConfiguration.Entity.Id = CurrentDataContext.CurrentNoAixmDataService.CreateFeatureDependencyConfiguration(SelectedConfiguration.Entity);
                     
                        }))); 
            }
        }

        private RelayCommand _deleteConfigurationCommand;
        private bool _isEnabled;

        public RelayCommand DeleteConfigurationCommand
        {
            get
            {
                return _deleteConfigurationCommand ?? (_deleteConfigurationCommand = new RelayCommand(
                  t =>
                      {
                          if (MessageBoxHelper.Show("Are you sure you want to delete selected feature dependency configuration?",
                                                           "Deleting Feature Dependency Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                                                           == MessageBoxResult.Yes)
                          {
                              if (SelectedConfiguration != null && 
                                  SelectedConfiguration.Entity != null &&
                                  SelectedConfiguration.Entity.Id != 0)
                              {
                                  if (CurrentDataContext.DeleteFeatureDependencyConfiguration(SelectedConfiguration.Entity.Id))
                                  {
                                      Configurations.Remove(SelectedConfiguration);
                                      SelectedConfiguration = null;
                                  }
                              }
                          }
                      },
                  t=>SelectedConfiguration!=null));
            }
        }

        public int DataSourceTemplateId { get; set; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        #endregion

       
    }
}
