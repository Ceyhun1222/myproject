using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Linq;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Control;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Document.Slot;
using TOSSM.ViewModel.Document.Slot.Problems.ProblemList;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;
using Aran.Aim.Features;

namespace TOSSM.ViewModel.Document
{
    public class DataSetProblemViewModel : ViewModelBase
    {
        public DataSetProblemViewModel(IssueViewModel issue)
        {
        }
    }

    public class DataSetCategoryViewModel : ProblemListViewModel
    {
        public readonly MtObservableCollection<ViewModelBase> RulesFiltered = new MtObservableCollection<ViewModelBase>();

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public ViewModelBase SelectedRule { get; set; }

        private Visibility _dataGridVisibility= Visibility.Visible;
        private string _status;

        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set => _dataGridVisibility = value;
        }

        public override void Clear()
        {
            RulesFiltered.Clear();
            Update();
        }

        public override void Add(ViewModelBase problemViewModel)
        {
            RulesFiltered.Add(problemViewModel);
            Update();
        }

        public override void Update()
        {
            Status = RulesFiltered.Count>0? "Displayed " + RulesFiltered.Count + " items from " + RulesFiltered.Count : "";
        }
    }

    public class ConfigurationViewModel : ViewModelBase
    {
        public string Name => Entity == null ? null : Entity.Name + " ("+ (FeatureType)Entity.RootFeatureType+")";
        public FeatureDependencyConfiguration Entity { get; set; }
        public ConfigurationViewModel(FeatureDependencyConfiguration item)
        {
            Entity = item;
        }

        public int ErrorNumber { get; set; }

        private string _errorsMessage;

        public string ErrorsMessage
        {
            get => _errorsMessage;
            set
            {
                _errorsMessage = value;
                OnPropertyChanged("ErrorsMessage");
            }
        }

        public void Init(PrivateSlot privateslot)
        {
            var config = FeatureDependencyUtil.Load(Entity.Data);

            var properties = new HashSet<string>();
            foreach (var property in config.MandatoryProperties ?? new string[0])
            {
                properties.Add(property);
            }
            foreach (var property in config.OptionalProperties ?? new string[0])
            {
                properties.Add(property);
            }

            var featureDataFiltered = new List<LightFeature>();

            var report = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(privateslot.PublicSlot.Id, privateslot.Id, Entity.Id, ReportType.FeatureDependencyReport);
            if (report != null && report.ReportData != null)
            {
                featureDataFiltered = FormatterUtil.ObjectFromBytes<List<LightFeature>>(report.ReportData);
                Date = report.DateTime;
            }
           
            ErrorNumber = featureDataFiltered.Count(item => item.HasProblems);
            ErrorsMessage = ErrorNumber + " / " + featureDataFiltered.Count;
        }

        public DateTime Date { get; set; }
    }

    public class FeatureDependencyReportDocViewModel : DocViewModel, ISelectedItemHolder, IPresenterParent
    {

        private RelayCommand _checkAllCommand;
        public RelayCommand CheckAllCommand
        {
            get
            {
                return _checkAllCommand ?? (_checkAllCommand = new RelayCommand(
                    t =>
                    {
                        foreach (LightFeatureWrapper item in DataPresenter.FeatureData)
                        {
                            item.IsChecked = true;
                        }
                    }));
            }
        }

        private RelayCommand _uncheckAllCommand;
        public RelayCommand UncheckAllCommand
        {
            get
            {
                return _uncheckAllCommand ?? (_uncheckAllCommand = new RelayCommand(
                    t =>
                    {
                        foreach (LightFeatureWrapper item in DataPresenter.FeatureData)
                        {
                            item.IsChecked = false;
                        }
                    }));
            }
        }


        private RelayCommand _createGeoDbCommand;
        public RelayCommand CreateGeoDbCommand
        {
            get
            {
                return _createGeoDbCommand ?? (_createGeoDbCommand = new RelayCommand(
                    t =>
                    {
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                        {
                            FileName = SelectedConfiguration.Entity.DataSourceTemplate.Name,
                            DefaultExt = ".gdb",
                            Filter = "Geo Database files (.gdb)|*.gdb|Access files (.mdb)|*.mdb",
                        };

                        if (saveFileDialog.ShowDialog() != true) return;

                        BlockerModel.BlockForAction(() =>
                        {
                            var list = (from LightFeatureWrapper item in DataPresenter.FeatureData where item.IsChecked select item.LightFeature).ToList();

                            GeoDbUtil.AddDataToGeoDb(saveFileDialog.FileName,
                                SelectedConfiguration,
                                list,
                                (status) =>
                                {
                                    MainManagerModel.Instance.StatusText = status;
                                });
                        });
                    }));
            }
            set => _createGeoDbCommand = value;
        }

        #region Configurations

        private CompressedFeatureDependencyConfiguration _config;

        private ConfigurationViewModel _selectedConfiguration;
        public ConfigurationViewModel SelectedConfiguration
        {
            get => _selectedConfiguration;
            set 
            { 
                _selectedConfiguration = value;
                OnPropertyChanged("SelectedConfiguration");

                if (SelectedConfiguration != null)
                {
                    BlockerModel.BlockForAction(UpdateConfiguration);
                }
            }
        }

        private MtObservableCollection<ConfigurationViewModel> _configurations;
        public MtObservableCollection<ConfigurationViewModel> Configurations => _configurations ?? (_configurations = new MtObservableCollection<ConfigurationViewModel>());


        readonly Dictionary<int, List<object>> _problemReports = new Dictionary<int, List<object>>();

        private ProblemListViewModel _problemListViewModel;
        public ProblemListViewModel ProblemListViewModel => _problemListViewModel??(_problemListViewModel=new DataSetCategoryViewModel());

        private SlotValidationCategoryViewModel _slotValidationCategoryViewModel;
        public SlotValidationCategoryViewModel SlotValidationCategoryViewModel {
            get
            {
                if (_slotValidationCategoryViewModel == null)
                {
                    _slotValidationCategoryViewModel=new SlotValidationCategoryViewModel("Issues", ProblemListViewModel);
                    _slotValidationCategoryViewModel.DataPresenter.BlockerModel = BlockerModel;
                    _slotValidationCategoryViewModel.OnReloadData = () =>
                    {
                        //set datapresenter data here
                        if (SlotValidationCategoryViewModel.SelectedRelation==null) return;
                        var featureType = SlotValidationCategoryViewModel.SelectedRelation.FeatureType;

                        var ids=Issues.Where(t => t.FeatureType == featureType).Select(t=>t.Identifier).Distinct();
                        var dataList = new List<object>();
                        foreach (var id in ids)
                        {
                            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                            {
                                FeatureTypeId = (int)featureType,
                                Guid = id
                            }, false,
                            privateslot.PublicSlot.EffectiveDate);

                            if (states != null && states.Count > 0)
                            {
                                dataList.Add(new ReadonlyFeatureWrapper(states.First().Data));
                            }
                        }
                        SlotValidationCategoryViewModel.DataPresenter.FeatureData = dataList;
                        SlotValidationCategoryViewModel.DataPresenter.UpdateFeatureDataFiltered();
                    };


                    _slotValidationCategoryViewModel.DataPresenter.OnSelectionChanged = (dataPresenter) =>
                    {
                        if (SlotValidationCategoryViewModel.SelectedRelation == null) return;
                        if (SelectedLightFeature == null) return;
                        var featureType = SlotValidationCategoryViewModel.SelectedRelation.FeatureType;
                        var identifier = SelectedLightFeature.Guid;
                        var relatedIssues=Issues.Where(t => t.FeatureType == featureType && t.Identifier == identifier).ToList();
                        _slotValidationCategoryViewModel.ProblemListViewModel.Clear();
                        foreach (var issue in relatedIssues)
                        {
                             _slotValidationCategoryViewModel.ProblemListViewModel.Add(new DataSetProblemViewModel(issue));
                        }
                    };


                }
                _slotValidationCategoryViewModel.AiracDate = AiracDate;
                return _slotValidationCategoryViewModel;
            }
        }

        private void UpdateConfiguration()
        {

            Application.Current.Dispatcher.Invoke(
              DispatcherPriority.Normal,
              (Action)(
                  () =>
                  {
                      DataPresenter.FeatureData=new List<object>();
                      DataPresenter.UpdateFeatureDataFiltered();
                  }));


            _config = FeatureDependencyUtil.Load(SelectedConfiguration.Entity.Data);

            DataPresenter.EffectiveDate = privateslot.PublicSlot.EffectiveDate;
            AiracDate = privateslot.PublicSlot.EffectiveDate;

            //form column list
            var properties = new HashSet<string>();
            foreach (var property in _config.MandatoryProperties ?? new string[0])
            {
                if (property.IndexOf("\\", StringComparison.Ordinal) == -1)
                {
                    properties.Add(property);
                }
            }
            foreach (var property in _config.OptionalProperties ?? new string[0])
            {
                if (property.IndexOf("\\", StringComparison.Ordinal) == -1)
                {
                    properties.Add(property);
                }
            }

            Application.Current.Dispatcher.Invoke(
                          DispatcherPriority.Normal,
                          (Action)(
                              () =>
                              {
                                 

                                  DataPresenter.ColumnList.Clear();
                                  DataPresenter.ColumnList.Add(CreateCheckedColumn());
                                  DataPresenter.ColumnList.Add(CreateColumn("Status"));

                                  foreach (var prop in properties)
                                  {
                                      DataPresenter.ColumnList.Add(CreateColumn(prop));
                                  }
                              }));

            
            
            List<object> list;
            if (!_problemReports.TryGetValue(SelectedConfiguration.Entity.Id, out list))
            {
                list = new List<object>();
                var report = CurrentDataContext.CurrentNoAixmDataService.GetProblemReport(privateslot.PublicSlot.Id, privateslot.Id, SelectedConfiguration.Entity.Id, ReportType.FeatureDependencyReport);
                if (report != null && report.ReportData != null)
                {
                    var lightLlist = FormatterUtil.ObjectFromBytes<List<LightFeature>>(report.ReportData);
                    foreach (var item in lightLlist)
                    {
                        var isIssue = item.HasProblems;
                        var wrapper = new LightFeatureWrapper(item)
                        {
                            Status = isIssue ? "Issue" : "Ok"
                        };
                        list.Add(wrapper);
                    }
                }
                _problemReports[SelectedConfiguration.Entity.Id] = list;
            }

            foreach (LightFeatureWrapper item in list)
            {
                var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                {
                    FeatureTypeId = item.LightFeature.FeatureType,
                    Guid = item.LightFeature.Guid
                }, false,
                privateslot.PublicSlot.EffectiveDate);

                var feature = states.FirstOrDefault()?.Data?.Feature;
                if (feature != null)
                    item.ReadonlyFeatureWrapper = new ReadonlyFeatureWrapper(feature);
            }

            DataPresenter.FeatureData = list;
            DataPresenter.UpdateFeatureDataFiltered();
        }

        private void OnIsCheckedChanged(LightFeatureWrapper wrapper)
        {
            //update checked count, prevent checking of issue
        }


        #endregion

        #region Ctor

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/dependency.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; private set; }

        private PrivateSlot privateslot;

        private FeatureDependencyReportDocViewModel()
        {
            DataPresenter = new DataPresenterModel
            {
                ViewModel = this,
                OnSelectionChanged = (datapresenter) =>
                {
                    var wrapper = datapresenter.SelectedFeature as LightFeatureWrapper;
                    SelectedLightFeature = wrapper == null?null:wrapper.LightFeature;


                    SlotValidationCategoryViewModel.RelationsData.Clear();
                    
                    UpdateIssues();

                    var groups=Issues.Where(t=>((int)t.FeatureType)>0).GroupBy(t => t.FeatureType);
                    foreach (IGrouping<FeatureType, IssueViewModel> group in groups)
                    {
                        var relationModel = new SingleTypeRelationViewModel
                        {
                            FeatureType = group.Key,
                            Count = group.Select(t=>t.Identifier).Distinct().Count()
                        };
                        relationModel.OnFeatureTypeChecked = (model) =>
                        {
                            var featureType = model.FeatureType;
                            SlotValidationCategoryViewModel.DataPresenter.FeatureType = featureType;
                        };
                        SlotValidationCategoryViewModel.RelationsData.Add(relationModel);
                    }
                }
            };


            //add IsChecked
        }

        public FeatureDependencyReportDocViewModel(PrivateSlot slot):this()
        {
            CanClose = true;
            privateslot = slot;
            Id = slot.Id;
        }

        public FeatureDependencyReportDocViewModel(int slotId): this()
        {
            CanClose = true;
            privateslot = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(slotId);
            Id = slotId;
        }


        #endregion

        #region BlockerModel

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel { ActivatingObject = this });
            set => _blockerModel = value;
        }

        #endregion

        #region Load

        public Action OnLoaded { get; set; }


        public void LoadInternal()
        {
            Configurations.Clear();

            DependencyConfigurations = CurrentDataContext.CurrentNoAixmDataService.GetFeatureDependencies().
                OrderBy(t => t.Name).ThenBy(t => ((FeatureType)t.RootFeatureType).ToString()).ToList();

            foreach (FeatureDependencyConfiguration item in DependencyConfigurations)
            {
                var configurationViewModel = new ConfigurationViewModel(item);
                configurationViewModel.Init(privateslot);
                Configurations.Add(configurationViewModel);
            }



            if (Configurations.FirstOrDefault() != null)
            {
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    (Action)(() => { SelectedConfiguration = Configurations.First(); }));
            }

            if (OnLoaded != null)
            {
                OnLoaded();
            }
         
        }

        public override void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            BlockerModel.BlockForAction(
                () =>
                    {
                        Title = "Feature Dependency Report";
                        LoadInternal();               
                    });
        }

        #endregion

        public List<FeatureDependencyConfiguration> DependencyConfigurations { get; set; }

        #region Visibility

        private Visibility _detailVisibility=Visibility.Hidden;
        public Visibility DetailVisibility
        {
            get => _detailVisibility;
            set
            {
                _detailVisibility = value;
                OnPropertyChanged("DetailVisibility");
            }
        }


       

        #endregion

        #region Tree View

        private MtObservableCollection<FeatureTreeViewItemViewModel> _firstGeneration;
        public MtObservableCollection<FeatureTreeViewItemViewModel> FirstGeneration
        {
            get => _firstGeneration ?? (_firstGeneration = new MtObservableCollection<FeatureTreeViewItemViewModel>());
            set
            {
                _firstGeneration = value;
                OnPropertyChanged("FirstGeneration");
            }
        }

        private FeatureTreeViewItemViewModel TreeIt(LightFeature lightFeature)
        {
            var result = new FeatureTreeViewItemViewModel
                             {
                                 SelectedItemHolder = this,
                                 LightFeature = lightFeature,
                                 Name = ((FeatureType) lightFeature.FeatureType).ToString(),
                             };


            foreach (var link in lightFeature.Links??new LightLink[0])
            {
                if (link==null) continue;

                if ((link.Flag & LightData.Missing) != 0)
                {
                        result.Children.Add(new FeatureTreeViewItemViewModel
                        {
                            Flag = link.Flag,
                            SelectedItemHolder = this,
                            Name = "Missing " + link.Name + " (" + ((FeatureType)link.FeatureType) + ")",
                            IsDirect = (link.Flag&LightData.IsDirectLink)!=0
                        });
                }
                else
                {
                    var subTree = TreeIt(link.Value);
                    subTree.Name = link.Name + " (" + subTree.Name + ")";
                    subTree.IsDirect = true;
                    result.Children.Add(subTree);
                }
            }

            result.IsExpanded = true;

            return result;
        }

        private LightFeature _selectedLightFeature;
        public LightFeature SelectedLightFeature
        {
            get => _selectedLightFeature;
            set
            {
                _selectedLightFeature = value;
                FirstGeneration.Clear();
                if (SelectedLightFeature != null)
                {
                    FirstGeneration.Add(TreeIt(SelectedLightFeature));
                    DetailVisibility = Visibility.Visible;
                }
                else
                {
                    DetailVisibility = Visibility.Hidden;
                }
                OnPropertyChanged("FirstGeneration");
                SelectedObject = null;
            }
        }


        #endregion

        #region Issues

        private int _numberOfLightFeatureIssues;
        public int NumberOfLightFeatureIssues
        {
            get => _numberOfLightFeatureIssues;
            set
            {
                _numberOfLightFeatureIssues = value;
                OnPropertyChanged("NumberOfLightFeatureIssues");
            }
        }

        private void UpdateIssues()
        {
            SelectedIssue = null;
            var list = new List<IssueViewModel>();
            if (SelectedLightFeature != null)
            {
                GatherIssues(SelectedLightFeature, list);
            }
            Issues = list;
            NumberOfLightFeatureIssues = list.Select(t=>t.Identifier).Distinct().Count();
        }

        private void GatherIssues(LightFeature lightFeature, List<IssueViewModel> list, string parent=null)
        {
            var lastParent = parent;
            if (parent != null)
            {
                var i = lastParent.LastIndexOf("\\", StringComparison.Ordinal);
                if (i > -1)
                {
                    lastParent = lastParent.Substring(i + 1);
                }
            }

            //gather missing links
            foreach (var link in lightFeature.Links??new LightLink[0])
            {
                if ((link.Flag & LightData.Missing) != 0 && (link.Flag & LightData.IsMandatory) != 0)
                {
                    if (lightFeature.FeatureType == 0)
                    {
                        list.Add(new IssueViewModel
                        {
                            Identifier = lightFeature.Guid,
                            FeatureType = (FeatureType)lightFeature.FeatureType,
                            Path = "Reverse link from " +link.FeatureType+ ": " + link.Name,
                            Path2 = parent == null ? link.Name : parent + "\\" + link.Name,
                            Issue = "Missing Link"
                        });
                    }
                    else
                    {
                        list.Add(new IssueViewModel
                        {
                            Identifier = lightFeature.Guid,
                            FeatureType = (FeatureType)lightFeature.FeatureType,
                            Path = "Link from " + ((FeatureType)lightFeature.FeatureType) + ": " + link.Name,
                            Path2 = parent == null ? link.Name : parent + "\\" + link.Name,
                            Issue = "Missing Link"
                        });
                    }
                }
                else if (link.Value!=null)
                {
                    GatherIssues(link.Value, list, parent == null ? link.Name : parent + "\\" + link.Name);
                }
            }
            
            //gather missing fields
            foreach (var field in lightFeature.Fields ?? new LightField[0])
            {
                if ((field.Flag & LightData.Missing) != 0 && (field.Flag & LightData.IsMandatory) != 0)
                {
                    list.Add(new IssueViewModel
                    {
                        Identifier = lightFeature.Guid,
                        FeatureType = (FeatureType)lightFeature.FeatureType,
                        Path = lastParent == null ? field.Name : lastParent + "." + field.Name,
                        Path2 = parent == null ? field.Name : parent + "." + field.Name,
                        Issue = "Missing Field"
                    });
                }
            }


            foreach (var complexField in lightFeature.ComplexFields ?? new LightComplexField[0])
            {
                if ((complexField.Flag & LightData.Missing) != 0 && (complexField.Flag & LightData.IsMandatory) != 0)
                {
                    list.Add(new IssueViewModel
                    {
                        Identifier = lightFeature.Guid,
                        FeatureType = (FeatureType)lightFeature.FeatureType,
                        Path = lastParent == null ? complexField.Name : lastParent + "." + complexField.Name,
                        Path2 = lastParent == null ? complexField.Name : lastParent + "." + complexField.Name,
                        Issue = "Missing Field"
                    });
                }
                else if (complexField.Value!=null)
                {
                    GatherIssues(complexField.Value, list, lastParent == null ? complexField.Name : lastParent + "." + complexField.Name);
                }
            }
        }

        #endregion


       
        #region Columns

        private ObservableCollection<DataGridColumn> _columnList;
        public ObservableCollection<DataGridColumn> ColumnList
        {
            get => _columnList ?? (_columnList = new ObservableCollection<DataGridColumn>());
            set => _columnList = value;
        }

        private static DataGridColumn CreateColumn(string propertyName)
        {
            return CreateColumn(propertyName, propertyName);
        }

        private static DataGridColumn CreateCheckedColumn()
        {
            // create text
            var checkBox = new FrameworkElementFactory(typeof(CheckBox));

            checkBox.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));

            checkBox.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Path = new PropertyPath("IsChecked"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            });

            var column = new DataGridTemplateColumn
            {
                Header = "#",
                CanUserSort = true,
                SortMemberPath = "IsChecked",
                CellTemplate = new DataTemplate
                {
                    VisualTree = checkBox
                }
            };

            return column;
        }

        private static DataGridColumn CreateColumn(string headerName, string propertyName)
        {
            // create text
            var label = new FrameworkElementFactory(typeof(TextBlock));

            label.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));

            label.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

            label.SetBinding(TextBlock.TextProperty, new Binding
            {
                Path = new PropertyPath(propertyName),
                Converter = HumanReadableConverter.Instance,
                Mode = BindingMode.OneTime
            });

            var column = new DataGridTemplateColumn
            {
                Header = headerName,
                CanUserSort = true,
                SortMemberPath = propertyName,
                CellTemplate = new DataTemplate
                {
                    VisualTree = label
                }
            };


            return column;
        }

        #endregion

        #region Implementation of ISelectedItemHolder

        private object _selectedObject;
        public object SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                var model = SelectedObject as FeatureTreeViewItemViewModel;

                if (model!=null && model.LightFeature!=null)
                {
                    Values = LoadValues(model.LightFeature);
                }
                else
                {
                    Values=new List<FieldValueViewModel>();
                }
            }
        }

        #endregion

        #region Field Values

        private void AddComplexField(LightComplexField complexField, List<FieldValueViewModel> values, string parent=null)
        {
            var nextParent = parent == null ? complexField.Name : parent + "\\" + complexField.Name;
            var lightFeature = complexField.Value;

            foreach (var lightField in (lightFeature.Fields ?? new LightField[0]))
            {
                values.Add(new FieldValueViewModel
                {
                    Flag = lightField.Flag,
                    Name = nextParent+"."+lightField.Name,
                    Value = HumanReadableConverter.ToHuman(lightField.Value)
                });
            }

            foreach (var lightComplexField in lightFeature.ComplexFields ?? new LightComplexField[0])
            {
                if ((lightComplexField.Flag & LightData.Missing) == 0)
                {
                    values.Add(new FieldValueViewModel
                    {
                        Flag = lightComplexField.Flag,
                        Name = lightComplexField.Name,
                        Value = "Complex Value"
                    });
                    AddComplexField(lightComplexField, values, nextParent);
                }
                else
                {
                    values.Add(new FieldValueViewModel
                    {
                        Flag = lightComplexField.Flag,
                        Name = lightComplexField.Name,
                    });
                }
            }
        }

        private List<FieldValueViewModel> LoadValues(LightFeature lightFeature)
        {
            var result = new List<FieldValueViewModel>();

            foreach (var lightField in (lightFeature.Fields ?? new LightField[0]))
            {
                result.Add(new FieldValueViewModel
                               {
                                   Flag = lightField.Flag,
                                   Name = lightField.Name, 
                                   Value = HumanReadableConverter.ToHuman(lightField.Value)
                               });
            }

            foreach (var lightComplexField in lightFeature.ComplexFields ?? new LightComplexField[0])
            {
                if ((lightComplexField.Flag & LightData.Missing) == 0)
                {
                    result.Add(new FieldValueViewModel
                    {
                        Flag = lightComplexField.Flag,
                        Name = lightComplexField.Name,
                        Value = "Complex Value"
                    });
                    AddComplexField(lightComplexField, result);
                }
                else
                {
                    result.Add(new FieldValueViewModel
                    {
                        Flag = lightComplexField.Flag,
                        Name = lightComplexField.Name,
                    });
                }
            }

            return result.OrderBy(t=>t.Name).ToList();
        }


        private IssueViewModel _selectedIssue;
        public IssueViewModel SelectedIssue
        {
            get => _selectedIssue;
            set
            {
                _selectedIssue = value;
                OnPropertyChanged("SelectedIssue");
            }
        }

        private List<IssueViewModel> _issues;
        public List<IssueViewModel> Issues
        {
            get => _issues;
            set
            {
                _issues = value;
                OnPropertyChanged("Issues");
            }
        }


        private List<FieldValueViewModel> _values;
        public List<FieldValueViewModel> Values
        {
            get => _values;
            set
            {
                _values = value;
                OnPropertyChanged("Values");
            }
        }

        private FieldValueViewModel _selectedValue;
        private string _status;

        public FieldValueViewModel SelectedValue
        {
            get => _selectedValue;
            set
            {
                _selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }

        #endregion

        public void ReloadData(DataPresenterModel model)
        {
        }
    }

    public class IssueViewModel : ViewModelBase
    {
        public string Issue { get; set; }
        public string Path { get; set; }
        public string Path2 { get; set; }

        public FeatureType FeatureType { get; set; }
        public Guid Identifier { get; set; }
    }

    public class FieldValueViewModel : ViewModelBase
    {
       
        public string Name { get; set; }
        public string Value { get; set; }


        private int _flag;
        public int Flag
        {
            get => _flag;
            set
            {
                _flag = value;

                if ((Flag & LightData.Missing) == 0)
                {
                    MissingOptionalVisibility = Visibility.Collapsed;
                    MissingMandatoryVisibility = Visibility.Collapsed;
                    OkVisibility=Visibility.Visible;
                }
                else if ((Flag &LightData.Missing)!=0 &&(Flag &LightData.IsMandatory)!=0)
                {
                    MissingOptionalVisibility = Visibility.Collapsed;
                    MissingMandatoryVisibility = Visibility.Visible;
                    OkVisibility = Visibility.Collapsed;
                }
                else if ((Flag & LightData.Missing) != 0 && (Flag & LightData.IsOptional) != 0)
                {
                    MissingOptionalVisibility = Visibility.Visible;
                    MissingMandatoryVisibility = Visibility.Collapsed;
                    OkVisibility = Visibility.Collapsed;
                }
            }
        }


        private Visibility _okVisibility=Visibility.Visible;
        public Visibility OkVisibility
        {
            get => _okVisibility;
            set => _okVisibility = value;
        }


        private Visibility _missingMandatoryVisibility = Visibility.Collapsed;
        public Visibility MissingMandatoryVisibility
        {
            get => _missingMandatoryVisibility;
            set => _missingMandatoryVisibility = value;
        }

        private Visibility _missingOptionalVisibility = Visibility.Collapsed;
        public Visibility MissingOptionalVisibility
        {
            get => _missingOptionalVisibility;
            set => _missingOptionalVisibility = value;
        }

       
    }
}
