using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.Util;
using TOSSM.ViewModel.Control.DataSourceTemplate;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{

    public class TemplateViewModel : ViewModelBase
    {
        private string _name;
        private ChartType _chartType;
        private string _chartTypeDescription;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ChartType ChartType
        {
            get => _chartType;
            set
            {
                _chartType = value;
                OnPropertyChanged("ChartType");
                ChartTypeDescription=EnumHelper.GetDescription(ChartType);
            }
        }

        public string ChartTypeDescription
        {
            get => _chartTypeDescription;
            set
            {
                _chartTypeDescription = value;
                OnPropertyChanged("ChartTypeDescription");
            }
        }

        public int Id { get; set; }
    }

    public enum ChartType
    {
        [Description("AD 2.24.1 Aerodrom Heliport")] AerodromHeliport,

        [Description("AD 2.24.2 Aircraft Parking Docking")] AircraftParkingDocking,

        [Description("AD 2.24.3 Aerodrome Ground Movement")] AerodromeGroundMovement,

        [Description("AD 2.24.4 Aerodrome Obstacle type A")] AerodromeObstacleTypeA,

        [Description("AD 2.24.4 Aerodrome Obstacle type B")] AerodromeObstacleTypeB,

        [Description("AD 2.24.4 Aerodrome Obstacle type C")] AerodromeObstacleTypeC,

        [Description("AD 2.24.5 Precision Approach")] PrecisionApproach,

        [Description("AD 2.24.6 Area")] Area,

        [Description("AD 2.24.7 Standard Instrument Departure")] StandardInstrumentDeparture,

        [Description("AD 2.24.8")] AD_2_24_8,

        [Description("AD 2.24.9 Standard Instrument Arrival")] StandardInstrumentArrival,

        [Description("AD 2.24.10 Radar Minimumum Altitude")] RadarMinimumumAltitude,

        [Description("AD 2.24.11 Standard Instrument Approach")] StandardInstrumentApproach,

        [Description("AD 2.24.12 Visual Approach")] VisualApproach,

        [Description("WorldAeronautical")] WorldAeronautical,

        [Description("Aeronautical")] Aeronautical,

        [Description("AirNavigation small scale")] AirNavigationSmallScale,

        [Description("ENR 6.1 En-Route chart")] EnRouteChart,

        [Description("ENR 6.3 Prohibited, Restricted and Danger Areas")] ProhibitedRestrictedDangerAreas,

        [Description("ENR 6.7 Radio Facility-Index Chart")] RadioFacilityIndexChart,

        [Description("ENR 6.8 Bird concentrations and areas with sensitive fauna ")] BirdConcentrationsAndAreasWithSensitiveFauna,

        [Description("ENR 6.9 Bird migration routes")] BirdMigrationRoutes,

        [Description("Module Inputs")]
        ModuleInputs,

        [Description("Module Outputs")]
        ModuleOutputs,

        [Description("Other")]
        Other,
    }


    public class DataSourceTemplateManagerViewModel : ToolViewModel
    {
        #region Ctor

        public static string ToolContentId = "Data Sources";
        private BlockerModel _blockerModel;
        private TemplateViewModel _selectedTemplate;
        private MtObservableCollection<TemplateViewModel> _templates;

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/database.png", UriKind.RelativeOrAbsolute);

        public DataSourceTemplateManagerViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }

        #endregion

        #region BlockerModel

        public BlockerModel BlockerModel => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });

        #endregion

        #region Templates

        private ICollectionView _templatesFiltered;
        private RelayCommand _newTemplateCommand;
       
        public ICollectionView TemplatesFiltered
        {
            get
            {
                if (_templatesFiltered == null)
                {
                    _templatesFiltered = CollectionViewSource.GetDefaultView(Templates);
                    _templatesFiltered.GroupDescriptions.Clear();
                    _templatesFiltered.GroupDescriptions.Add(new PropertyGroupDescription("ChartType"));
                }
                return _templatesFiltered;
            }
        }

        public MtObservableCollection<TemplateViewModel> Templates => _templates ?? (_templates = new MtObservableCollection<TemplateViewModel>());

        public TemplateViewModel SelectedTemplate
        {
            get => _selectedTemplate;
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged("SelectedTemplate");
                if (SelectedTemplate == null)
                {
                    ConfigurationListViewModel.Configurations.Clear();
                    ConfigurationListViewModel.SelectedConfiguration = null;
                    ConfigurationListViewModel.IsEnabled = false;
                    return;
                }

                BlockerModel.BlockForAction(() =>
                {
                    ConfigurationListViewModel.DataSourceTemplateId = SelectedTemplate.Id;
                    ConfigurationListViewModel.Configurations.Clear();
                    var list=CurrentDataContext.CurrentNoAixmDataService.GetFeatureDependenciesByTemplate(SelectedTemplate.Id);
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            ConfigurationListViewModel.Configurations.Add(new ConfigurationViewModel
                            {
                                Entity = item
                            });
                        }
                    }
                    ConfigurationListViewModel.IsEnabled = true;

                });
               
            }
        }

        #endregion

        #region Load

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            var templateEntities = CurrentDataContext.CurrentNoAixmDataService.GetAllDataSourceTemplates().ToList();

            var templates = templateEntities.Select(entity => new TemplateViewModel
            {
                Name = entity.Name, 
                ChartType = (ChartType) entity.ChartType, 
                Id = entity.Id
            }).ToList();

            templates = templates.OrderBy(t => t.ChartType).ToList();

            Templates.Clear();

            foreach (var template in templates)
            {
                Templates.Add(template);
            }
        }

        #endregion

        #region Commands

        public RelayCommand NewTemplateCommand
        {
            get { return _newTemplateCommand??(_newTemplateCommand=new RelayCommand(
                t =>
                {
                    BlockerModel.BlockForAction(()=>
                    {
                        EditDataSourceTemplateViewModel.Id = 0;
                        EditDataSourceTemplateViewModel.Name = null;
                        EditDataSourceTemplateViewModel.Group=ChartType.Other;
                        EditDataSourceTemplateViewModel.Show();
                    }
                );
                }
                )); }
        }

        private RelayCommand _editTemplateCommand;
        public RelayCommand EditTemplateCommand
        {
            get
            {
                return _editTemplateCommand ?? (_editTemplateCommand = new RelayCommand(

                    t =>
                    {
                        EditDataSourceTemplateViewModel.Id = SelectedTemplate.Id;
                        EditDataSourceTemplateViewModel.Name = SelectedTemplate.Name;
                        EditDataSourceTemplateViewModel.Group = SelectedTemplate.ChartType;
                        EditDataSourceTemplateViewModel.Show();
                    },
                    t => SelectedTemplate != null));
            }
        }

        private RelayCommand _deleteTemplateCommand;
        public RelayCommand DeleteTemplateCommand
        {
            get { return _deleteTemplateCommand??(_deleteTemplateCommand=new RelayCommand(

                t =>
                {
                    if (SelectedTemplate == null) return;

                    if (MessageBoxHelper.Show("Are you sure you want to delete selected data source template?",
                        "Deleting Data Source", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning)
                        == MessageBoxResult.Yes)
                    {
                        if (CurrentDataContext.CurrentNoAixmDataService.DeleteDataSourceTemplate(SelectedTemplate.Id))
                        {
                            Templates.Remove(SelectedTemplate);
                            SelectedTemplate = null;
                        }
                    }
                },
                t => SelectedTemplate != null)); }
        }

        #endregion

        private EditDataSourceTemplateViewModel _editDataSourceTemplateViewModel;
        public EditDataSourceTemplateViewModel EditDataSourceTemplateViewModel => _editDataSourceTemplateViewModel??(_editDataSourceTemplateViewModel=new EditDataSourceTemplateViewModel{OkAction = OnEditDataSource});

        private void OnEditDataSource()
        {
           BlockerModel.BlockForAction(() =>
           {
               if (EditDataSourceTemplateViewModel.Id == 0)
               {
                   var entity = new DataSourceTemplate
                   {
                       Id = EditDataSourceTemplateViewModel.Id,
                       Name = EditDataSourceTemplateViewModel.Name,
                       ChartType = (int)EditDataSourceTemplateViewModel.Group
                   };
                   EditDataSourceTemplateViewModel.Id = CurrentDataContext.CurrentNoAixmDataService.CreateDataSourceTemplate(entity);

                   var newTemplate = new TemplateViewModel
                   {
                       Id = EditDataSourceTemplateViewModel.Id,
                       Name = EditDataSourceTemplateViewModel.Name,
                       ChartType = EditDataSourceTemplateViewModel.Group
                   };
                   Templates.Add(newTemplate);
                   SelectedTemplate = newTemplate;
               }
               else
               {
                   if (SelectedTemplate==null) return;

                   var correspondingTemplate=Templates.FirstOrDefault(t => t.Id == SelectedTemplate.Id);
                   if (correspondingTemplate==null) return;

                   var entity =
                       CurrentDataContext.CurrentNoAixmDataService.GetDataSourceTemplateById(SelectedTemplate.Id);

                   entity.Name = EditDataSourceTemplateViewModel.Name;
                   entity.ChartType = (int)EditDataSourceTemplateViewModel.Group;

                   if (CurrentDataContext.CurrentNoAixmDataService.UpdateDataSourceTemplate(entity))
                   {
                       correspondingTemplate.Name = EditDataSourceTemplateViewModel.Name;
                       correspondingTemplate.ChartType = EditDataSourceTemplateViewModel.Group;
                   }
               }

              
           });
        }

        private FeatureDependencyManagerToolViewModel _configurationListViewModel;
        public FeatureDependencyManagerToolViewModel ConfigurationListViewModel => _configurationListViewModel ??
                                                                                   (_configurationListViewModel = new FeatureDependencyManagerToolViewModel
                                                                                   {
                                                                                       BlockerModel = BlockerModel
                                                                                   });

        private RelayCommand _createGeoDbCommand;
        public RelayCommand CreateGeoDbCommand
        {
            get { return _createGeoDbCommand??(_createGeoDbCommand=new RelayCommand(
                t =>
                {
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        FileName = SelectedTemplate.Name,
                        DefaultExt = ".gdb",
                        Filter = "Geo Database files (.gdb)|*.gdb|Access files (.mdb)|*.mdb",
                    };

                    if (saveFileDialog.ShowDialog() != true) return;

                    BlockerModel.BlockForAction(() =>
                    {
                        GeoDbUtil.CreateGeoDb(saveFileDialog.FileName, ConfigurationListViewModel.Configurations.ToList(),
                            (status) =>
                            {
                                MainManagerModel.Instance.StatusText = status;
                            });
                    });

                },
                t=> SelectedTemplate!=null
                )); }
        }
    }
}
