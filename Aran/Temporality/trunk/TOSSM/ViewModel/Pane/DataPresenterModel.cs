using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Model.AimMetadata;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using ClosedXML.Excel;
using Microsoft.Win32;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Control.PropertySelector.Menu;
using TOSSM.ViewModel.Control.XmlViewer;
using TOSSM.ViewModel.Tool;

namespace TOSSM.ViewModel.Pane
{
    public class DataPresenterModel : ViewModelBase, IChangedHandler<PropertySetMenuItemViewModel>, IChangedHandler<PropertyMenuItemViewModel>
    {
        private static readonly string[] NoGenerationFor = new[] { "Id" };

        public IPresenterParent ViewModel { get; set; }

        #region Ctor

        public DataPresenterModel()
        {
            _filtererThread = new Thread(FilterFunction)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true,
            };

            _filtererThread.Start();
        }

        private readonly Thread _filtererThread;

        public bool IsTerminated { get; set; }

        public void FilterFunction()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            while (!IsTerminated && MainManagerModel.Instance.MainManagerWindow != null && MainManagerModel.Instance.MainManagerWindow.IsVisible)
            {
                Thread.Sleep(200);
                if (!FilterChanged) continue;

                //MainManagerModel.Instance.IsBusy = true;
                //if (IsFirstFilter)
                //{
                //    IsFirstFilter = false;
                //    Status = "Preparing for search...";
                //    Thread.Sleep(200);
                //}
                BlockerModel.Block();
                UpdateFeatureDataFiltered();
                BlockerModel.Unblock();
                //MainManagerModel.Instance.IsBusy = false;
            }
        }

        #endregion

        #region Feature type


        private bool IsFirstFilter = true;

        private void ClearData()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind,
          (Action)(() =>
          {
              ColumnList.Clear();
              FeatureDataFiltered = new ObservableCollection<object>();
              IsFirstFilter = true;
          }));
        }

        private FeatureType? _featureType;
        public FeatureType? FeatureType
        {
            get => _featureType;
            set
            {
                _featureType = value;
                OnPropertyChanged("FeatureType");

                if (FeatureType == null)
                {
                    return;
                }

                Reload();
            }
        }

        private RelayCommand _refresCommand;
        public RelayCommand Refresh
        {
            get
            {
                return _refresCommand ?? (_refresCommand = new RelayCommand(t1 => Reload()));
            }
            set => _refresCommand = value;
        }


        private async void Reload()
        {
            await BlockerModel.BlockForAction(() =>
            {
                ClearData();

                InitContextMenuForColumns();
                InitColumnsAsync();

                ViewModel.ReloadData(this);

            });
        }

        public Action<DataPresenterModel> OnSelectionChanged { get; set; }

        private object _selectedFeature;
        public object SelectedFeature
        {
            get => _selectedFeature;
            set
            {
                _selectedFeature = value;
                OnPropertyChanged("SelectedFeature");

                if (OnSelectionChanged != null)
                {
                    OnSelectionChanged(this);
                }

                var wrapper = SelectedFeature as ReadonlyFeatureWrapper;
                if (wrapper != null)
                {
                    //MainManagerModel.Instance.OnMapViewerToolViewModel.SelectFeatureOnMap(wrapper.Feature);
                }
            }
        }

        #endregion

        #region Filterer

        public bool FilterChanged;

        private string _propertyFilter;
        public string PropertyFilter
        {
            get => _propertyFilter;
            set
            {
                _propertyFilter = value;
                OnPropertyChanged("PropertyFilter");
                OnPropertyChanged("FilterEmptyButtonVisibility");
                FilterChanged = true;
            }
        }

        private RelayCommand _onClearFilter;
        public RelayCommand OnClearFilter
        {
            get
            {
                return _onClearFilter ??
                       (_onClearFilter = new RelayCommand(t1 => PropertyFilter = "", t2 => !String.IsNullOrEmpty(PropertyFilter)));
            }
            set => _onClearFilter = value;
        }

        public Visibility FilterEmptyButtonVisibility => String.IsNullOrEmpty(PropertyFilter) ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Context menu and columns

        public void AddColumn(AimPropInfo propInfo)
        {
            ColumnList.Add(CreateColumn(propInfo.Name));
        }

        public void RemoveColumn(AimPropInfo propInfo)
        {
            var column = ColumnList.FirstOrDefault(t => t.Header.ToString() == propInfo.Name);
            if (column != null)
            {
                ColumnList.Remove(column);
            }
        }

        private IEnumerable<AimPropInfo> DisplayingProperties()
        {
            if (FeatureType == null) return null;

            var result = new List<AimPropInfo>();
            var classInfo = AimMetadata.GetClassInfoByIndex((int)(FeatureType)FeatureType);
            switch (PropertySet)
            {
                case ColumnSet.Simple:
                    result.AddRange(classInfo.Properties.Where(
                        prop => !prop.IsList && prop.PropType != null &&
                                (prop.PropType.AimObjectType == AimObjectType.DataType ||
                                 prop.PropType.AimObjectType == AimObjectType.Field)));
                    break;
                case ColumnSet.All://all
                    result.AddRange(classInfo.Properties);
                    break;
                case ColumnSet.Custom://custom
                    result.AddRange(classInfo.Properties.Where(prop => prop.UiPropInfo().ShowGridView));
                    break;
            }

            result.RemoveAll(t => NoGenerationFor.Contains(t.Name));
            return result.OrderBy(t => t.UiPropInfo().Position);
        }

        private void InitColumns()
        {
            if (FeatureType == null) return;


            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                (Action)(() =>
               {
                   var newList = _predefinedColumnInfos.OrderBy(t => t.Index).Select(info => CreateColumn(info.Header, info.Property, info.Type)).ToList();
                   newList.AddRange(DisplayingProperties().Select(prop => CreateColumn(prop.Name)));

                   ColumnList.Clear();
                   foreach (var item in newList)
                   {
                       ColumnList.Add(item);
                   }
               }));

        }

        private async void InitColumnsAsync()
        {
            if (FeatureType == null) return;

            await BlockerModel.BlockForAction(() =>
            {

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    (Action)(() =>
                   {
                       var newList = _predefinedColumnInfos.OrderBy(t => t.Index).Select(info => CreateColumn(info.Header, info.Property, info.Type)).ToList();
                       newList.AddRange(DisplayingProperties().Select(prop => CreateColumn(prop.Name)));

                       ColumnList.Clear();
                       foreach (var item in newList)
                       {
                           ColumnList.Add(item);
                       }
                   }));
            });


        }

        public void InitContextMenuForColumns()
        {
            if (FeatureType == null) return;

            var classInfo = AimMetadata.GetClassInfoByIndex((int)(FeatureType)FeatureType);
            _updatingMenu = true;


            //force update
            //CustomColumnsViewModel = null;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                (Action)(() =>
                {
                    CustomColumnsViewModel.ItemsSource = new ObservableCollection<MenuItemViewModel>(
                        classInfo.Properties.Where(t => !NoGenerationFor.Contains(t.Name))
                            .Select(
                                prop => new PropertyMenuItemViewModel(this) { PropInfo = prop })
                            .Cast<MenuItemViewModel>()
                            .ToList());
                    //MenuItemList = null;
                }));

            _updatingMenu = false;
        }

        public void OnChanged(PropertyMenuItemViewModel propertyMenuItemViewModel)
        {
            if (_updatingMenu) return;


            if (!CustomColumnsViewModel.IsChecked)
            {
                CustomColumnsViewModel.IsChecked = true;
            }
            else
            {
                if (propertyMenuItemViewModel.IsChecked)
                {
                    AddColumn(propertyMenuItemViewModel.PropInfo);
                }
                else
                {
                    RemoveColumn(propertyMenuItemViewModel.PropInfo);
                }
            }
        }

        public void OnChanged(PropertySetMenuItemViewModel propertyMenuItemViewModel)
        {
            if (propertyMenuItemViewModel.Header == ColumnSet.All.ToString())
            {
                SimpleColumnsViewModel.IsChecked = false;
                CustomColumnsViewModel.IsChecked = false;
                PropertySet = ColumnSet.All;
            }

            if (propertyMenuItemViewModel.Header == ColumnSet.Simple.ToString())
            {
                AllColumnsViewModel.IsChecked = false;
                CustomColumnsViewModel.IsChecked = false;
                PropertySet = ColumnSet.Simple;
            }

            if (propertyMenuItemViewModel.Header == ColumnSet.Custom.ToString())
            {
                AllColumnsViewModel.IsChecked = false;
                SimpleColumnsViewModel.IsChecked = false;
                PropertySet = ColumnSet.Custom;
            }
        }

        public static ColumnSet InitialColumnSet { get; set; } = ColumnSet.Custom;

        private string _selectedColumnSetName = InitialColumnSet.ToString();
        public string SelectedColumnSetName
        {
            get => _selectedColumnSetName;
            set
            {
                _selectedColumnSetName = value;
                OnPropertyChanged("SelectedColumnSetName");
            }
        }

        private ColumnSet _propertySet = InitialColumnSet;
        public ColumnSet PropertySet
        {
            get => _propertySet;
            set
            {
                if (_propertySet == value) return;
                _propertySet = value;
                InitColumns();
                SelectedColumnSetName = PropertySet.ToString();
                InitialColumnSet = PropertySet;
            }
        }

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

        private static DataGridColumn CreateColumn(string propertyName, Type type)
        {
            return CreateColumn(propertyName, propertyName, type);
        }

        private static DataGridColumn CreateColumn(string header, string propertyName, Type type = null)
        {
            FrameworkElementFactory elementFactory = null;
            if (type == typeof(CheckBox))
            {
                elementFactory = new FrameworkElementFactory(typeof(CheckBox));
                elementFactory.SetBinding(ToggleButton.IsCheckedProperty, new Binding
                {
                    Path = new PropertyPath(propertyName),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
            }
            else
            {
                elementFactory = new FrameworkElementFactory(typeof(TextBlock));
                elementFactory.SetBinding(TextBlock.TextProperty, new Binding
                {
                    Path = new PropertyPath(propertyName),
                    Converter = HumanReadableConverter.Instance,
                    Mode = BindingMode.OneTime
                });

            }

            elementFactory.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));

            elementFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);


            var column = new DataGridTemplateColumn
            {
                Header = header,
                CanUserSort = true,
                SortMemberPath = propertyName,
                CellTemplate = new DataTemplate
                {
                    VisualTree = elementFactory
                }
            };


            return column;

        }

        private PropertySetMenuItemViewModel _simpleColumnsViewModel;
        private PropertySetMenuItemViewModel SimpleColumnsViewModel => _simpleColumnsViewModel ?? (_simpleColumnsViewModel = new PropertySetMenuItemViewModel
        {
            PresenterModel = this,
            Header = ColumnSet.Simple.ToString(),
            IsChecked = PropertySet == ColumnSet.Simple
        });


        private PropertySetMenuItemViewModel _allColumnsViewModel;
        private PropertySetMenuItemViewModel AllColumnsViewModel => _allColumnsViewModel ?? (_allColumnsViewModel = new PropertySetMenuItemViewModel
        {
            PresenterModel = this,
            Header = ColumnSet.All.ToString(),
            IsChecked = PropertySet == ColumnSet.All
        });

        private PropertySetMenuItemViewModel _customColumnsViewModel;
        private PropertySetMenuItemViewModel CustomColumnsViewModel => _customColumnsViewModel ?? (_customColumnsViewModel = new PropertySetMenuItemViewModel
        {
            PresenterModel = this,
            Header = ColumnSet.Custom.ToString(),
            IsChecked = PropertySet == ColumnSet.Custom,
        });

        private ObservableCollection<MenuItemViewModel> _menuItemList;
        public ObservableCollection<MenuItemViewModel> MenuItemList
        {
            get
            {
                if (_menuItemList == null)
                {
                    _updatingMenu = true;
                    _menuItemList = new ObservableCollection<MenuItemViewModel>
                                        {
                                            SimpleColumnsViewModel,
                                            AllColumnsViewModel,
                                            CustomColumnsViewModel
                                        };
                    _updatingMenu = false;
                }

                return _menuItemList;
            }
            set
            {
                _menuItemList = value;
                OnPropertyChanged("MenuItemList");
            }
        }

        private ObservableCollection<language?> _langItemList;
        public ObservableCollection<language?> LangItemList
        {
            get
            {
                if (_langItemList == null)
                {
                    _updatingMenu = true;
                    _langItemList = new ObservableCollection<language?>(Enum.GetValues(typeof(language)).Cast<language?>());
                    _updatingMenu = false;
                }

                return _langItemList;
            }
            set
            {
                _langItemList = value;
                OnPropertyChanged("LangItemList");
            }
        }


        private bool _updatingMenu;


        #endregion

        #region Reload data

        private List<object> _featureData;
        public List<object> FeatureData
        {
            get => _featureData;
            set
            {
                _featureData = value;
                OnPropertyChanged("FeatureData");
                PropertyFilter = "";
            }
        }

        private ObservableCollection<object> _featureDataFiltered;
        public ObservableCollection<object> FeatureDataFiltered
        {
            get => _featureDataFiltered;
            set
            {
                _featureDataFiltered = value;
                OnPropertyChanged("FeatureDataFiltered");
            }
        }

        public void UpdateFeatureDataFiltered()
        {
            //Stopwatch s=new Stopwatch();
            //s.Start();
            FilterChanged = false;



            Status = "Filtering...";

            var result = new ObservableCollection<object>();
            var autocomplete = new HashSet<string>();

            if (FeatureData != null)
            {
                foreach (var obj in FeatureData)
                {
                    if (FilterChanged) return;

                    if (String.IsNullOrWhiteSpace(PropertyFilter))
                    {
                        result.Add(obj);
                    }
                    else
                    {
                        var found = false;
                        if (PropertyFilter.StartsWith("="))
                        {
                            var equalString = PropertyFilter.Substring(1, PropertyFilter.Length - 1);
                            foreach (var t in GetDesriptions(obj).Where(t => t.Equals(equalString)))
                            {
                                if (FilterChanged) return;

                                autocomplete.Add(t);
                                found = true;
                            }
                        }
                        else
                        {
                            foreach (var t in GetDesriptions(obj).Where(t => t.ToLower().Contains(PropertyFilter.ToLower())))
                            {
                                if (FilterChanged) return;

                                autocomplete.Add(t);
                                found = true;
                            }
                        }


                        if (found)
                        {
                            result.Add(obj);
                        }

                    }
                }
            }

            //s.Stop();
            //var em=s.ElapsedMilliseconds;
            //s.Restart();

            AutoCompleteList = autocomplete;

            FeatureDataFiltered = result;



            //FeatureDataFiltered=new VirtualizingCollection<object>(new ListDataProvider<object>(result), 100);


            //s.Stop();
            //var em2 = s.ElapsedMilliseconds;

            DataGridVisibility = (FeatureData != null && FeatureData.Count > 0) ? Visibility.Visible : Visibility.Collapsed;

            Status = (FeatureData != null && FeatureData.Count > 0) ? "Displayed " + FeatureDataFiltered.Count + " items from " + FeatureData.Count : "";
        }



        #endregion

        #region Descriptions

        private static IEnumerable<string> GetDesriptions(object obj)
        {
            return obj == null ? new HashSet<string>() : HumanReadableConverter.DescriptionList(obj);
        }

        #endregion

        #region Commands


        private RelayCommand _addToExportCommand;
        public RelayCommand AddToExportCommand
        {
            get
            {


                return _addToExportCommand ?? (_addToExportCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(
                            () =>
                            {
                                if (SelectedFeatures != null)
                                {
                                    foreach (var item in SelectedFeatures)
                                    {
                                        MainManagerModel.Instance.AddToExport(item);
                                    }
                                }
                                else
                                {
                                    MainManagerModel.Instance.AddToExport(SelectedFeature);
                                }

                                MainManagerModel.Instance.ExportToolViewModel.SelectedRelation = null;
                                MainManagerModel.Instance.ExportToolViewModel.Reload();

                            }
                            );

                    },
                    t => SelectedFeature != null));
            }
        }


        private RelayCommand _reportCommand;
        public RelayCommand ReportCommand
        {
            get
            {     
                return _reportCommand ?? (_reportCommand = new RelayCommand( t =>
                {
                    if (FeatureType == null) return;

                    var dialog = new SaveFileDialog
                    {
                        Title = "Save Excel Report",
                        Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                        FileName = CreateFileName()
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        BlockerModel.BlockForAction(() => { ExportToExcel(dialog.FileName); });
                    }
                   
                }));
            }
        }

        private string CreateFileName()
        {
            return $"{FeatureType} {PropertyFilter} {EffectiveDate:dd-MM-yyyy} {DateTime.Now:dd-MM-yyyy HH-mm}";
        }

        private void ExportToExcel(string fileName)
        {
            var result = new List<AimPropInfo>();
            var classInfo = AimMetadata.GetClassInfoByIndex((int)(FeatureType)FeatureType);
            //var props = classInfo.Properties.Where(
            //            prop => !prop.IsList && prop.PropType != null &&
            //                    (prop.PropType.AimObjectType == AimObjectType.DataType ||
            //                     prop.PropType.AimObjectType == AimObjectType.Field));
            var props = classInfo.Properties.Where(
                        prop => !prop.IsList && prop.PropType != null &&
                                (prop.PropType.AimObjectType == AimObjectType.DataType || prop.PropType.AimObjectType == AimObjectType.Field));


            var wb = new XLWorkbook();
            var infoSheet = wb.AddWorksheet("Info");

            infoSheet.Cell(1, 1).Value = "Report Date";
            infoSheet.Cell(1, 2).DataType = XLCellValues.DateTime;
            infoSheet.Cell(1, 2).Value = DateTime.Now;

            infoSheet.Cell(3, 1).Value = "Effective Date";
            infoSheet.Cell(3, 2).DataType = XLCellValues.DateTime;
            infoSheet.Cell(3, 2).Value = EffectiveDate;

            infoSheet.Cell(5, 1).Value = "Filter";
            infoSheet.Cell(5, 2).Value = PropertyFilter;

            infoSheet.Cell(7, 1).Value = "FeatureType";
            infoSheet.Cell(7, 2).Value = FeatureType;

            infoSheet.Cell(9, 1).Value = "User";
            infoSheet.Cell(9, 2).Value = CurrentDataContext.CurrentUserName;

            infoSheet.Cell(11, 1).Value = "Count";
            infoSheet.Cell(11, 2).Value = FeatureDataFiltered?.Count;

            DataTable dt = new DataTable();

            foreach (var prop in props)
            {
                dt.Columns.Add(prop.Name);
            }

            if (FeatureDataFiltered?.Count > 0)
                foreach (var item in FeatureDataFiltered)
                {

                    var aimobj = ((item as ReadonlyFeatureWrapper).Feature.Feature) as IAimObject;
                    var values = new List<object> { };

                    foreach (var prop in props)
                    {
                        var propVal = aimobj.GetValue(prop.Index);
                        if (propVal == null)
                        {
                            values.Add("");
                            continue;
                        }
                        if (propVal.PropertyType == AimPropertyType.AranField)
                            values.Add((propVal as IEditAimField)?.FieldValue?.ToString());
                        if (propVal.PropertyType == AimPropertyType.DataType)
                            values.Add(HumanReadableConverter.ToHuman(propVal));

                    }
                    dt.Rows.Add(values.ToArray());
                }
            wb.Worksheets.Add(dt, ((FeatureType)FeatureType).ToString());
            wb.SaveAs(fileName);
        }


        private RelayCommand _geoIntersectionCommand;
        public RelayCommand GeoIntersectionCommand
        {
            get
            {
                return _geoIntersectionCommand ?? (_geoIntersectionCommand = new RelayCommand(
                    t => MainManagerModel.Instance.GeoIntersection(SelectedFeature, ViewModel.AiracDate),
                    t => SelectedFeature != null));
            }
            set => _geoIntersectionCommand = value;
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand(
                    t => MainManagerModel.Instance.Edit(SelectedFeature, ViewModel.AiracDate),
                    delegate
                        {
                            if (SelectedFeature == null) return false;
                            if (CurrentDataContext.CurrentUser?.ActivePrivateSlot == null) return false;
                            if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                            return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == EffectiveDate;
                        }));
            }
            set => _editCommand = value;
        }

        public DateTime EffectiveDate { get; set; }

        private RelayCommand _viewCommand;
        public RelayCommand ViewCommand
        {
            get
            {
                return _viewCommand ?? (_viewCommand = new RelayCommand(
                    t => MainManagerModel.Instance.View(SelectedFeature, ViewModel.AiracDate),
                    t => SelectedFeature != null));
            }
            set => _viewCommand = value;
        }

        private RelayCommand _copyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                return _copyCommand ?? (_copyCommand = new RelayCommand(
                    t => MainManagerModel.Instance.Copy(SelectedFeature, ViewModel.AiracDate),
                    t => SelectedFeature != null));
            }
            set => _copyCommand = value;
        }

        private RelayCommand _newCommand;
        public RelayCommand NewCommand
        {
            get
            {
                return _newCommand ?? (_newCommand = new RelayCommand(
                           t => MainManagerModel.Instance.New(FeatureType, ViewModel.AiracDate),
                           delegate
                           {
                               if (FeatureType == null) return false;
                               if (CurrentDataContext.CurrentUser == null) return false;
                               if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;
                               if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                               return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == ViewModel.AiracDate;
                           }));
            }
            set => _newCommand = value;
        }
        private RelayCommand _pasteCommand;
        public RelayCommand PasteCommand
        {
            get
            {
                return _pasteCommand ?? (_pasteCommand = new RelayCommand(
                           t => MainManagerModel.Instance.Paste(ViewModel.AiracDate),
                           delegate
                           {
                               if (!MainManagerModel.Instance.CanPaste()) return false;
                               if (CurrentDataContext.CurrentUser == null) return false;
                               if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;
                               if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                               return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == ViewModel.AiracDate;
                           }));
            }
            set => _pasteCommand = value;
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(
                    t => MainManagerModel.Instance.Delete(SelectedFeature, ViewModel.AiracDate),
                    delegate
                    {
                        if (SelectedFeature == null) return false;
                        if (CurrentDataContext.CurrentUser == null) return false;
                        if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;
                        if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;

                        Feature feature = null;
                        if (SelectedFeature is ReadonlyFeatureWrapper)
                        {
                            feature = (SelectedFeature as ReadonlyFeatureWrapper).Feature.Feature;
                        }
                        if (feature?.TimeSlice?.FeatureLifetime == null) return false;

                        return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate == EffectiveDate &&
                            (feature.TimeSlice.FeatureLifetime.EndPosition == null || feature.TimeSlice.FeatureLifetime.EndPosition > EffectiveDate);
                    }));
            }
            set => _deleteCommand = value;
        }

        private RelayCommand _evolutionCommand;
        public RelayCommand EvolutionCommand
        {
            get
            {
                return _evolutionCommand ?? (_evolutionCommand = new RelayCommand(
                    t => MainManagerModel.Instance.Evolution(SelectedFeature, ViewModel.AiracDate),
                    t => SelectedFeature != null));
            }
            set => _evolutionCommand = value;
        }

        private RelayCommand _relationCommand;
        public RelayCommand RelationCommand
        {
            get
            {
                return _relationCommand ?? (_relationCommand = new RelayCommand(
                    t => MainManagerModel.Instance.Relation(SelectedFeature, ViewModel.AiracDate),
                    t => SelectedFeature != null));
            }
            set => _relationCommand = value;
        }


        private XmlViewerViewModel _xmlViewerViewModel;
        public XmlViewerViewModel XmlViewerViewModel => _xmlViewerViewModel ?? (_xmlViewerViewModel = new XmlViewerViewModel());

        private RelayCommand _viewXmlCommand;
        public RelayCommand ViewXmlCommand
        {
            get
            {
                return _viewXmlCommand ?? (_viewXmlCommand = new RelayCommand(

                           t =>
                           {
                               var wrapper = SelectedFeature as ReadonlyFeatureWrapper;
                               if (wrapper == null) return;

                               var xml = wrapper.Xml;

                               if (xml == null)
                               {

                                   StringWriterWithEncoding stringBuilder;
                                   var writer = AixmGenerator.Instance.CreateXmlWriter(out stringBuilder);

                                   var data = wrapper.Feature;

                                   if (data != null)
                                   {
                                       AixmGenerator.Instance.WriteFeature(writer, new List<Feature> { data.Feature });
                                   }

                                   AixmGenerator.Instance.CloseXmlWriter(writer);
                                   xml = stringBuilder.ToString();

                               }

                               if (!string.IsNullOrEmpty(xml))
                               {
                                   var xmlDocument = new XmlDocument();
                                   try
                                   {
                                       xmlDocument.LoadXml(xml);

                                       XmlViewerViewModel.XmlDocument = xmlDocument;
                                   }
                                   catch (XmlException)
                                   {
                                       MessageBoxHelper.Show("The XML content is invalid.");
                                       return;
                                   }



                                   XmlViewerViewModel.Visibility = Visibility.Visible;
                               }

                           },
                           t => SelectedFeature != null));
            }
        }

        private RelayCommand _onMapCommand;
        public RelayCommand OnMapCommand
        {
            get
            {
                return _onMapCommand ?? (_onMapCommand = new RelayCommand(
                    t =>
                        {
                            //MainManagerModel.Instance.OnMapViewerToolViewModel.AddFeatureType(
                            //    (FeatureType)FeatureType, EffectiveDate);
                            //MainManagerModel.Instance.OnMap((FeatureType)FeatureType, ViewModel.AiracDate);
                        },
                    t => FeatureType != null));
            }
            set => _onMapCommand = value;
        }

        private RelayCommand _relationChartCommand;
        public RelayCommand RelationChartCommand
        {
            get
            {
                return _relationChartCommand ?? (_relationChartCommand = new RelayCommand(
              t => MainManagerModel.Instance.RelationChart(SelectedFeature, ViewModel.AiracDate),
              t => SelectedFeature != null
              ));
            }
            set => _relationChartCommand = value;
        }

        #endregion

        #region Other UI properties

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }
        private HashSet<string> _autoCompleteList;
        public HashSet<String> AutoCompleteList
        {
            get => _autoCompleteList;
            set
            {
                _autoCompleteList = value;
                OnPropertyChanged("AutoCompleteList");
            }
        }

        private Visibility _dataGridVisibility = Visibility.Collapsed;
        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged("DataGridVisibility");
            }
        }



        #endregion

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public IList SelectedFeatures { get; set; }


        public void SetColumnOrder(List<string> orderList)
        {
            if (orderList == null) return;
            if (ColumnList == null) return;
            if (ColumnList.Count < 0) return;
            if (orderList.Count < 2) return;

            var classInfo = AimMetadata.GetClassInfoByIndex((int)(FeatureType)FeatureType);

            if (classInfo.Properties.Any(t => t.UiPropInfo().Position == 0))
            {
                //init Positions
                var index = 0;
                foreach (AimPropInfo prop in classInfo.Properties)
                {
                    index++;
                    prop.UiPropInfo().Position = index;
                }
            }

            var list = new List<AimPropInfo>();

            foreach (var header in orderList)
            {
                var correspondingProperty = classInfo.Properties.FirstOrDefault(t => t.Name == header);
                if (correspondingProperty == null)
                {
                    throw new Exception("bad property name: " + header);
                }
                list.Add(correspondingProperty);
            }

            //buble sort
            var go = true;
            while (go)
            {
                go = false;
                for (var i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].UiPropInfo().Position < list[i + 1].UiPropInfo().Position) continue;

                    //swap positions
                    var pos = list[i].UiPropInfo().Position;
                    list[i].UiPropInfo().Position = list[i + 1].UiPropInfo().Position;
                    list[i + 1].UiPropInfo().Position = pos;
                    go = true;
                    break;
                }
            }
        }

        private readonly List<PredefinedColumnInfo> _predefinedColumnInfos = new List<PredefinedColumnInfo>();



        public void AddPredefinedColumn(string header, string propInfo, int index = -1)
        {
            _predefinedColumnInfos.Add(new PredefinedColumnInfo
            {
                Header = header,
                Property = propInfo,
                Index = index
            });
        }
        public void AddPredefinedColumn(string header, string propInfo, Type type, int index = -1)
        {
            _predefinedColumnInfos.Add(new PredefinedColumnInfo
            {
                Header = header,
                Property = propInfo,
                Index = index,
                Type = type
            });
        }
    }

    internal class PredefinedColumnInfo
    {
        public string Header;
        public string Property;
        public int Index;
        public Type Type;
    }

}
