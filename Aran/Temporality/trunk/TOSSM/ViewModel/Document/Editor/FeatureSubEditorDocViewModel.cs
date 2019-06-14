using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Objects;
using Aran.Aim.PropertyEnum;
using Aran.Aim.PropertyPrecision;
using Aran.Geometries;
using Aran.Metadata.Utils;
using Aran.Temporality.Common.Config;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Geometry;
using TOSSM.Util;
using TOSSM.View.Document.Editor;
using TOSSM.ViewModel.Control.LightElementControl;
using TOSSM.ViewModel.Document.Single.Editable;
using UMCommonModels.Dto;

namespace TOSSM.ViewModel.Document.Editor
{
    public class FeatureSubEditorDocViewModel : HierarchyDocViewerModel
    {
        public ComplexPropertyConfiguration Configuration { get; set; }

        #region AiracDate

        public override void OnAiracDateChanged()
        {
            base.OnAiracDateChanged();
            OnPropertyChanged("AiracDate");
            UpdateTitle();
        }

        #endregion

        #region Columns

        private DataGridColumn _deltaColumn;
        public DataGridColumn DeltaColumn
        {
            get
            {
                if (_deltaColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _deltaColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Δ",
                        CellTemplate = (DataTemplate)CurrentViewer.FindResource("DeltaItemTemplate")
                    };
                }
                return _deltaColumn;
            }
        }

        private DataGridColumn _propertyColumn;
        public DataGridColumn PropertyColumn
        {
            get
            {
                if (_propertyColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _propertyColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Property",
                        CellTemplate = (DataTemplate)CurrentViewer.FindResource("PropertyItemTemplate")
                    };
                }
                return _propertyColumn;
            }
        }

        private DataGridColumn _restoreColumn;
        public DataGridColumn RestoreColumn
        {
            get
            {
                if (_restoreColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _restoreColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Undo Changes",
                        CellTemplate = (DataTemplate)CurrentViewer.FindResource("RestoreOldValueItemTemplate")
                    };
                }
                return _restoreColumn;
            }
        }

        private DataGridColumn _previousValueColumn;
        public DataGridColumn PreviousValueColumn
        {
            get
            {
                if (_previousValueColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _previousValueColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Previous Value",
                        CellTemplate = (DataTemplate)CurrentViewer.FindResource("PreviousValueItemTemplate")
                    };
                }
                return _previousValueColumn;
            }
        }

        private DataGridColumn _valueColumn;
        public DataGridColumn ValueColumn
        {
            get
            {
                if (_valueColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _valueColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Value",
                        CellTemplateSelector = (DataTemplateSelector)CurrentViewer.FindResource("EditorTemplateSelector"),
                        MaxWidth = 600
                    };
                }
                return _valueColumn;
            }
        }

        private MtObservableCollection<DataGridColumn> _columnCollection;
        public MtObservableCollection<DataGridColumn> ColumnCollection
        {
            get => _columnCollection ?? (_columnCollection = new MtObservableCollection<DataGridColumn>());
            set => _columnCollection = value;
        }


        #endregion

        #region Ctor

        public static string ToolContentId = "Feature Viewer";

        public FeatureSubEditorDocViewModel()
        {
        }

        protected FeatureSubEditorDocViewModel(FeatureType type, Guid id, DateTime date) : base(type, id, date)
        {
            try
            {
                if (ConfigUtil.UseWebApiForMetadata)
                {
                    _originators = CommonDataProvider.GetUsers();
                }
            }
            catch (Exception)
            {
                MessageBoxHelper.Show("Originators list is not available");
            }
        }

        #endregion

        #region Commands

        public void DeleteSelected()//called for list only
        {
            var list = EditedFeature as IList;
            if (list == null) return;
            if (SelectedProperty == null) return;
            var index = PropertyList.IndexOf(SelectedProperty);
            if (index > -1)
            {
                list.RemoveAt(index);
                PropertyList.RemoveAt(index);

                for (var i = index; i < PropertyList.Count; i++)
                {
                    var listPropertyModel = PropertyList[i] as ListPropertyModel;
                    listPropertyModel?.UpdateListIndex(i);
                }

                UpdateByChildren();
            }
        }

        private RelayCommand _onAddListItem;
        public RelayCommand OnAddListItem
        {
            get => _onAddListItem ?? (_onAddListItem = new RelayCommand(t =>
                                   {
                                       var list = EditedFeature as IList;
                                       if (list == null) return;
                                       if (ListItemType == null) return;
                                       var newItem = Activator.CreateInstance(ListItemType);
                                       list.Add(newItem);

                                       var listModel = new ListPropertyModel(this)
                                       {
                                           ParentObject = EditedFeature,
                                           GroupName = "Items",
                                           Value = newItem,
                                           PropInfo = PropInfo,
                                       };
                                       listModel.UpdateListIndex(PropertyList.Count);
                                       PropertyList.Add(listModel);
                                       SelectedProperty = PropertyList.Last();
                                       UpdateByChildren();
                                   }));
            set => _onAddListItem = value;
        }

        private RelayCommand _onPasteListItem;
        public RelayCommand OnPasteListItem
        {
            get => _onPasteListItem ?? (_onPasteListItem = new RelayCommand(t =>
                                 {
                                     var list = EditedFeature as IList;
                                     if (list == null) return;
                                     if (ListItemType == null) return;

                                     var reference = new FeatureRef(MainManagerModel.Instance.BufferFeatureIdentifier);
                                     var newItem = new FeatureRefObject { Feature = reference };

                                     list.Add(newItem);

                                     var listModel = new ListPropertyModel(this)
                                     {
                                         ParentObject = EditedFeature,
                                         GroupName = "Items",
                                         Value = newItem,
                                         PropInfo = PropInfo,
                                     };
                                     listModel.UpdateListIndex(PropertyList.Count);
                                     PropertyList.Add(listModel);
                                     SelectedProperty = PropertyList.Last();
                                     UpdateByChildren();
                                 },
                    t =>
                    {

                        if (MainManagerModel.Instance.BufferFeatureType == null) return false;

                        var list = EditedFeature as IList;
                        if (list == null) return false;
                        if (ListItemType == null) return false;

                        // should be same reference feature type
                        if (PropInfo.ReferenceFeature != MainManagerModel.Instance.BufferFeatureType)
                        {
                            return false;
                        }

                        // should not be in list
                        foreach (var item in list)
                        {
                            var i = item as FeatureRefObject;
                            if (i != null && i.Feature.Identifier == MainManagerModel.Instance.BufferFeatureIdentifier)
                            {
                                return false;
                            }
                        }

                        //check for lifetime
                        HierarchyDocViewerModel model = ParentModel;
                        while (model.ParentModel != null)
                        {
                            model = model.ParentModel;
                        }
                        var myDate = model.AiracDate;
                        if (MainManagerModel.Instance.BufferLifeTime.BeginPosition > myDate) return false;
                        if (MainManagerModel.Instance.BufferLifeTime.EndPosition != null && MainManagerModel.Instance.BufferLifeTime.EndPosition <= myDate) return false;


                        return true;

                    }));
            set => _onPasteListItem = value;
        }

        private RelayCommand _onPasteGeoListItem;
        public RelayCommand OnPasteGeoListItem
        {
            get => _onPasteGeoListItem ?? (_onPasteGeoListItem = new RelayCommand(t =>
                       {
                           var list = EditedFeature as IList;
                           if (list == null) return;
                           if (ListItemType == null) return;

                           CodeColour? color = null;
                           LightElementViewModel.ShowDialog(model =>
                               {
                                   color = model.Color;
                               }
                           );

                           if (color == null) return;

                           foreach (var geometry in MainManagerModel.Instance.Clipboard)
                           {
                               if(geometry.Type != GeometryType.Point)
                                   continue;

                               var newItem = Activator.CreateInstance(ListItemType);

                               var light = newItem as LightElement;
                               light.Location = new ElevatedPoint();

                               GeoFormatter.CopyGeometry(geometry, light.Location.Geo);
                               light.Colour = color;
                               list.Add(newItem);

                               var listModel = new ListPropertyModel(this)
                               {
                                   ParentObject = EditedFeature,
                                   GroupName = "Items",
                                   Value = newItem,
                                   PropInfo = PropInfo,
                               };
                               listModel.UpdateListIndex(PropertyList.Count);
                               PropertyList.Add(listModel);
                           }
                           SelectedProperty = PropertyList.Last();
                           UpdateByChildren();

                       },
                       t =>
                       {

                           if (MainManagerModel.Instance.Clipboard.ClipboardAranGeometry == null) return false;

                           var list = EditedFeature as IList;
                           if (list == null) return false;
                           if (ListItemType == null) return false;
                           return ListItemType == typeof(LightElement);
                       }));
            set => _onPasteGeoListItem = value;
        }

        private RelayCommand _onUpListItem;
        public RelayCommand OnUpListItem
        {
            get => _onUpListItem ?? (_onUpListItem = new RelayCommand(t =>
                                 {
                                     var list = EditedFeature as IList;
                                     if (list == null) return;
                                     if (SelectedProperty == null) return;
                                     var index = PropertyList.IndexOf(SelectedProperty);
                                     if (index > 0)
                                     {
                                         var item = list[index - 1];
                                         list.RemoveAt(index - 1);
                                         list.Insert(index, item);

                                         var propItem = PropertyList[index - 1];
                                         PropertyList.RemoveAt(index - 1);
                                         PropertyList.Insert(index, propItem);

                                         var listPropertyModel = PropertyList[index] as ListPropertyModel;
                                         listPropertyModel?.UpdateListIndex(index);
                                         var listPropertyModel2 = PropertyList[index - 1] as ListPropertyModel;
                                         listPropertyModel2?.UpdateListIndex(index - 1);

                                         UpdateByChildren();
                                     }
                                 }, t =>
                                        {
                                            if (SelectedProperty == null) return false;
                                            if (PropertyList.Count < 2) return false;
                                            var list = EditedFeature as IList;
                                            if (list == null) return false;
                                            var index = PropertyList.IndexOf(SelectedProperty);
                                            if (index < 1) return false;
                                            return true;
                                        }));
            set => _onUpListItem = value;
        }

        private RelayCommand _onDownListItem;
        public RelayCommand OnDownListItem
        {
            get => _onDownListItem ?? (_onDownListItem = new RelayCommand(t =>
                                 {
                                     var list = EditedFeature as IList;
                                     if (list == null) return;
                                     if (SelectedProperty == null) return;
                                     var index = PropertyList.IndexOf(SelectedProperty);
                                     if (index != -1 && index < PropertyList.Count - 1)
                                     {
                                         var item = list[index + 1];
                                         list.RemoveAt(index + 1);
                                         list.Insert(index, item);

                                         var propItem = PropertyList[index + 1];
                                         PropertyList.RemoveAt(index + 1);
                                         PropertyList.Insert(index, propItem);

                                         var listPropertyModel = PropertyList[index] as ListPropertyModel;
                                         listPropertyModel?.UpdateListIndex(index);
                                         var listPropertyModel2 = PropertyList[index + 1] as ListPropertyModel;
                                         listPropertyModel2?.UpdateListIndex(index + 1);

                                         UpdateByChildren();
                                     }
                                 }, t =>
                                        {
                                            if (SelectedProperty == null) return false;
                                            if (PropertyList.Count < 2) return false;
                                            var list = EditedFeature as IList;
                                            if (list == null) return false;
                                            var index = PropertyList.IndexOf(SelectedProperty);
                                            if (index == -1) return false;
                                            if (index > PropertyList.Count - 2) return false;
                                            return true;
                                        }));
            set => _onDownListItem = value;
        }

        #endregion

        #region Visibility

        private Visibility _specialViewsToolBarVisibility;
        public Visibility SpecialViewsToolBarVisibility
        {
            get => _specialViewsToolBarVisibility; set
            {
                _specialViewsToolBarVisibility = value;
                OnPropertyChanged("SpecialViewsToolBarVisibility");
            }
        }

        private Visibility _originatorToolBarVisibility;
        public Visibility OriginatorToolBarVisibility
        {
            get => ConfigUtil.UseWebApiForMetadata ? _originatorToolBarVisibility : Visibility.Collapsed;
            set
            {
                _originatorToolBarVisibility = value;
                OnPropertyChanged("OriginatorToolBarVisibility");
            }
        }

        private Visibility _editLabelVisibility;
        public Visibility EditLabelVisibility
        {
            get => _editLabelVisibility; set
            {
                _editLabelVisibility = value;
                OnPropertyChanged("EditLabelVisibility");
            }
        }

        private Visibility _deltaFilterVisibility;
        public Visibility DeltaFilterVisibility
        {
            get => _deltaFilterVisibility; set
            {
                _deltaFilterVisibility = value;
                OnPropertyChanged("DeltaFilterVisibility");
            }
        }


        private Visibility _editButtonVisibility;
        public Visibility EditButtonVisibility
        {
            get => _editButtonVisibility; set
            {
                _editButtonVisibility = value;
                OnPropertyChanged("EditButtonVisibility");
            }
        }

        private Visibility _saveButtonVisibility;
        public Visibility SaveButtonVisibility
        {
            get => _saveButtonVisibility; set
            {
                _saveButtonVisibility = value;
                OnPropertyChanged("SaveButtonVisibility");
            }
        }

        private Visibility _changedFilterVisibility;
        public Visibility ChangedFilterVisibility
        {
            get => _changedFilterVisibility; set
            {
                _changedFilterVisibility = value;
                OnPropertyChanged("ChangedFilterVisibility");
            }
        }

        private Visibility _filterVisibility;
        public Visibility FilterVisibility
        {
            get => _filterVisibility; set
            {
                _filterVisibility = value;
                OnPropertyChanged("FilterVisibility");
            }
        }

        private Visibility _listVisibility;
        public Visibility ListVisibility
        {
            get => _listVisibility; set
            {
                _listVisibility = value;
                OnPropertyChanged("ListVisibility");
            }
        }

        private Visibility _linkListVisibility = Visibility.Hidden;

        public Visibility LinkListVisibility
        {
            get => _linkListVisibility; set
            {
                _linkListVisibility = value;
                OnPropertyChanged("LinkListVisibility");
            }
        }


        #endregion

        #region Properties

        private bool _isAiracSelectionEnabled;
        public bool IsAiracSelectionEnabled
        {
            get => _isAiracSelectionEnabled; set
            {
                _isAiracSelectionEnabled = value;
                OnPropertyChanged("IsAiracSelectionEnabled");
            }
        }

        private UserDto GetUserDtoByDescription(string description)
        {
            return _originators.FirstOrDefault(x => GetUserDtoDescription(x) == description);
        }

        private string GetUserDtoDescription(UserDto userDto)
        {
            return $"{userDto?.Name} {userDto?.Surname}, {userDto?.DataOriginatorCode}";
        }

        private List<UserDto> _originators;
        public List<string> Originators => _originators?.Select(GetUserDtoDescription).OrderBy(x => x).ToList();

        private UserDto _selectedOriginator;
        public string SelectedOriginator
        {
            get
            {
                if (_selectedOriginator != null)
                    return GetUserDtoDescription(_selectedOriginator);
                return "";
            }
            set
            {
                _selectedOriginator = GetUserDtoByDescription(value);
                (_editedFeature as Feature).SetMetadataIdentificationInfo(_selectedOriginator, CiRoleCode.Originator);
                OnPropertyChanged("SelectedOriginator");
            }
        }

        public DeltaMask DeltaMask { get; set; }
        public bool IsReadOnly { get; set; }
        public Type ListItemType { get; set; }

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }

        private BlockerModel _propertyListBlocker;
        public BlockerModel PropertyListBlocker
        {
            get => _propertyListBlocker ?? (_propertyListBlocker = new BlockerModel());
            set => _propertyListBlocker = value;
        }

        private object _editedFeature;
        public object EditedFeature
        {
            get => _editedFeature; set
            {
                _editedFeature = value;
                if (ComplexContent != null)
                {
                    Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Background,
                        (Action)(
                                     () =>
                                         {
                                             ComplexContent.Visibility = Visibility.Collapsed;
                                         }));
                }


                SelectedProperty = null;


                if (EditedFeature == null || ParentModel != null)
                {
                    UpdateFeature();
                    if (ParentModel != null)
                    {
                        // ParentModel.UpdateByChildren();
                    }
                }
                else
                {
                    //here ParentModel == null, so no need to Update Children
                    //lazy load on root only
                    //BlockerModel.BlockForAction(UpdateFeature);

                    if (_editedFeature is Feature feature)
                    {
                        var originatorCode = feature.GetMetadataIdentificationCode(CiRoleCode.Originator);
                        if (originatorCode != null)
                        {
                            _selectedOriginator =
                                _originators.FirstOrDefault(x => x.DataOriginatorCode == originatorCode);
                            OnPropertyChanged("SelectedOriginator");
                        }
                    }

                    UpdateFeature();
                }
            }
        }

        public void UpdateComplexContent(bool fromUser = true)
        {
            if (SelectedProperty is AimEditablePropertyModel)
            {
                ComplexContent = SubViewerProvider.GetSubViewer(ComplexContent as FeatureSubEditor, SelectedProperty as AimEditablePropertyModel, CurrentViewer,
                    fromUser ? null : (bool?)((FeatureSubEditorDocViewModel)ComplexContent.DataContext).IsNotNullFilter);
            }
            else if (SelectedProperty is ReflectionEditablePropertyModel)
            {
                ComplexContent = SubViewerProvider.GetSubViewer(ComplexContent as FeatureSubEditor, SelectedProperty as ReflectionEditablePropertyModel, CurrentViewer,
                    fromUser ? null : (bool?)((FeatureSubEditorDocViewModel)ComplexContent.DataContext).IsNotNullFilter);
            }
            else if (SelectedProperty is ListPropertyModel)
            {
                ComplexContent = SubViewerProvider.GetSubViewer(ComplexContent as FeatureSubEditor, SelectedProperty as ListPropertyModel, CurrentViewer,
                    fromUser ? null : (bool?)((FeatureSubEditorDocViewModel)ComplexContent.DataContext).IsNotNullFilter);
            }
            OnPropertyChanged("ComplexContent");
        }

        private EditableSinglePropertyModel _selectedProperty;
        public EditableSinglePropertyModel SelectedProperty
        {
            get => _selectedProperty; set
            {
                if (_selectedProperty == value) return;
                _selectedProperty = value;
                OnPropertyChanged("SelectedProperty");
                UpdateComplexContent();

                if (SelectedProperty != null && SelectedProperty.PropertyName == "Geo" && SelectedProperty.Value is Aran.Geometries.Geometry)
                {
                    MainManagerModel.Instance.OnMapViewerToolViewModel.SelectGeometryOnMap((Aran.Geometries.Geometry)SelectedProperty.Value);
                }
            }
        }

        private MtObservableCollection<EditableSinglePropertyModel> _propertyList;
        public MtObservableCollection<EditableSinglePropertyModel> PropertyList
        {
            get => _propertyList ?? (_propertyList = new MtObservableCollection<EditableSinglePropertyModel>());
            set
            {
                _propertyList = value;
                OnPropertyChanged("PropertyList");
            }
        }

        private ICollectionView _propertyListFiltered;
        public ICollectionView PropertyListFiltered
        {
            get
            {
                if (_propertyListFiltered == null)
                {

                    _propertyListFiltered = CollectionViewSource.GetDefaultView(PropertyList);

                    _propertyListFiltered.Filter =
                    item =>
                    {
                        var t = item as EditableSinglePropertyModel;
                        return t != null && (((t.IsChanged && IsChangedFilter) || (!IsChangedFilter))
                        &&
                        ((t.IsNotNull && IsNotNullFilter) || (!IsNotNullFilter))
                        &&
                        ((t.IsDelta && IsDeltaFilter) || (!IsDeltaFilter)));
                    };

                    _propertyListFiltered.GroupDescriptions.Clear();
                    _propertyListFiltered.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
                }
                return _propertyListFiltered;
            }
            set
            {
                _propertyListFiltered = value;
                OnPropertyChanged("PropertyListFiltered");
            }
        }

        #endregion

        #region Filter

        private bool _isNotNullFilter;
        public bool IsNotNullFilter
        {
            get => _isNotNullFilter; set
            {
                _isNotNullFilter = value;
                if (PropertyListFiltered != null && PropertyList.Count > 0)
                {
                    PropertyListBlocker.BlockForAction(() =>
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                        (Action)(() =>
                            PropertyListFiltered.Refresh())));
                }
                OnPropertyChanged("IsNotNullFilter");
            }
        }

        private bool _isChangedFilter;
        public bool IsChangedFilter
        {
            get => _isChangedFilter; set
            {
                _isChangedFilter = value;
                if (PropertyListFiltered != null && PropertyList.Count > 0)
                {
                    PropertyListBlocker.BlockForAction(() =>
                       Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                           PropertyListFiltered.Refresh())));
                }
                OnPropertyChanged("IsChangedFilter");
            }
        }


        private bool _isDeltaFilter;
        public bool IsDeltaFilter
        {
            get => _isDeltaFilter; set
            {
                _isDeltaFilter = value;
                if (PropertyListFiltered != null && PropertyList.Count > 0)
                {
                    PropertyListBlocker.BlockForAction(() =>
                       Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                           PropertyListFiltered.Refresh())));
                }
                OnPropertyChanged("IsDeltaFilter");
            }
        }



        #endregion

        #region UI building (creating property list)

        private static readonly string[] TimeSliceNames = new[] { "identifier", "timeslice" };

        private List<EditableSinglePropertyModel> PrepareListProperties()
        {
            FilterVisibility = Visibility.Collapsed;
            ListVisibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;
            LinkListVisibility = Visibility.Collapsed;

            var list = new List<EditableSinglePropertyModel>();
            var items = EditedFeature as IList;
            if (items != null)
                foreach (object item in items)
                {
                    var listModel = new ListPropertyModel(this)
                    {
                        ParentObject = EditedFeature,
                        GroupName = "Items",
                        PropInfo = PropInfo,
                        Value = item,
                    };
                    listModel.UpdateListIndex(list.Count);
                    list.Add(listModel);
                }
            return list;
        }

        private List<EditableSinglePropertyModel> PrepareLinkListProperties()
        {
            FilterVisibility = Visibility.Collapsed;
            LinkListVisibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;
            ListVisibility = Visibility.Collapsed;


            var list = new List<EditableSinglePropertyModel>();
            var items = EditedFeature as IList;
            if (items != null)
                foreach (object item in items)
                {
                    var listModel = new ListPropertyModel(this)
                    {
                        ParentObject = EditedFeature,
                        GroupName = "Items",
                        PropInfo = PropInfo,
                        Value = item,
                    };
                    listModel.UpdateListIndex(list.Count);
                    list.Add(listModel);
                }
            return list;
        }

        public AimPropInfo PropInfo { get; set; }

        private List<EditableSinglePropertyModel> PrepareGeoProperties()
        {
            var result = new List<EditableSinglePropertyModel>();

            var geoObject = EditedFeature as Aran.Geometries.Geometry;
            if (geoObject == null) return result;



            if (geoObject.Type == GeometryType.Point)
            {
                var point = (Aran.Geometries.Point)geoObject;
                result.Add(
                    new ReflectionEditablePropertyModel
                    {
                        GroupName = "Coordinates (DD)",
                        ParentModel = this,
                        IsReadOnly = true,
                        PropertyName = "X (DD)",
                        Value = point.X,
                        StringValue = GeoFormatter.FormatX(point.X),
                        PropertyDescription = "X coordinate in DD format."
                    });


                result.Add(
                   new ReflectionEditablePropertyModel
                   {
                       GroupName = "Coordinates (DMS)",
                       ParentModel = this,
                       IsReadOnly = true,
                       PropertyName = "X (DMS)",
                       Value = point.X,
                       StringValue = GeoFormatter.FormatXdms(point.X),
                       PropertyDescription = "X coordinate in DMS format."
                   });



                result.Add(
                    new ReflectionEditablePropertyModel
                    {
                        GroupName = "Coordinates (DD)",
                        ParentModel = this,
                        IsReadOnly = true,
                        PropertyName = "Y (DD)",
                        Value = point.Y,
                        StringValue = GeoFormatter.FormatY(point.Y),
                        PropertyDescription = "Y coordinate in DD format."
                    });


                result.Add(
                    new ReflectionEditablePropertyModel
                    {
                        GroupName = "Coordinates (DMS)",
                        ParentModel = this,
                        IsReadOnly = true,
                        PropertyName = "Y (DMS)",
                        Value = point.Y,
                        StringValue = GeoFormatter.FormatYdms(point.Y),
                        PropertyDescription = "Y coordinate in DMS format."
                    });
                result.Add(
                    new ReflectionEditablePropertyModel
                    {
                        GroupName = "Other Coordinates",
                        ParentModel = this,
                        IsReadOnly = true,
                        PropertyName = "Z",
                        Value = point.Z,
                        StringValue = GeoFormatter.FormatZ(point.Z),
                        PropertyDescription = "Z coordinate."
                    });


            }
            else if (geoObject is MultiLineString)
            {
                var multiLineString = geoObject as MultiLineString;

                result.Add(
                   new ReflectionEditablePropertyModel
                   {
                       GroupName = "Multilinestring properties",
                       ParentModel = this,
                       IsReadOnly = true,
                       ParentObject = multiLineString,
                       PropertyInfo = multiLineString.GetType().GetProperty("Count"),
                       PropertyDescription = "Count of lines in multilinestring."
                   });
            }
            else if (geoObject is MultiPolygon)
            {
                var multipolygon = geoObject as MultiPolygon;

                result.Add(
                   new ReflectionEditablePropertyModel
                   {
                       GroupName = "Multipolygon properties",
                       ParentModel = this,
                       IsReadOnly = true,
                       ParentObject = multipolygon,
                       PropertyInfo = multipolygon.GetType().GetProperty("Count"),
                       PropertyDescription = "Count of polygons in multipolygon."
                   });
            }


            result.Add(
            new ReflectionEditablePropertyModel
            {
                PropertyName = "Type",
                Value = typeof(Aran.Geometries.Geometry).GetProperty("Type").GetValue(geoObject, null).ToString(),
                StringValue = typeof(Aran.Geometries.Geometry).GetProperty("Type").GetValue(geoObject, null).ToString(),
                GroupName = "Geometry properties",
                ParentModel = this,
                IsReadOnly = true,
                PropertyDescription = "Geometry type."
            });




            return result;
        }


        private List<EditableSinglePropertyModel> PrepareAimProperties()
        {
            var newPropertyList = new List<EditableSinglePropertyModel>();

            FilterVisibility = Visibility.Visible;
            ListVisibility = Visibility.Collapsed;
            LinkListVisibility = Visibility.Collapsed;

            if (EditedFeature is Aran.Geometries.Geometry)
            {
                return PrepareGeoProperties();
            }

            var aimObject = EditedFeature as IAimObject;

            if (aimObject == null)
            {
                return new List<EditableSinglePropertyModel>();
                throw new Exception("Abnormal object");
            }

            if (ParentModel == null)
            {
                //prepare root
                UpdateTitle();
            }
            
            if (aimObject is Feature)
            {
                newPropertyList.Add(new AimEditablePropertyModel
                {
                    PropertyName = "Feature Type",
                    GroupName = "TimeSlice properties",
                    ParentModel = this,
                    OldValue = typeof(Feature).GetProperty("FeatureType").GetValue(aimObject, null),
                    StringValue = typeof(Feature).GetProperty("FeatureType").GetValue(aimObject, null).ToString(),
                    IsReadOnly = true,
                    DeltaColumnVisibility = Visibility.Hidden,
                    PropertyDescription = "Type of feature."
                });
            }


            var aimPropInfoArr = AimMetadata.GetAimPropInfos(aimObject);
            //add Aim properties
            foreach (var aimPropInfo in aimPropInfoArr)
            {
                if (String.IsNullOrEmpty(aimPropInfo.AixmName)) continue;

                if (aimObject is BtAbstractObject && (
                    aimPropInfo.Index == (int)PropertyBtAbstractObject.MetadataId
                    || aimPropInfo.Index == (int)PropertyBtAbstractObject.Uuid
                    || aimPropInfo.Index == (int)PropertyBtAbstractObject.UuidRef))
                    continue;


                var isMasked = true;
                object prevValue = null;

                if (DeltaMask != null)
                {
                    prevValue = DeltaMask.GetPreviousValue(aimPropInfo);
                    isMasked = DeltaMask.IsMasked(aimPropInfo);

                    if (prevValue is IEditAimField)
                    {
                        prevValue = (prevValue as IEditAimField).FieldValue;
                        if (prevValue is uint || prevValue is int || prevValue is Int64)
                        {
                            prevValue = Convert.ToDecimal(prevValue);
                        }
                    }

                    if (prevValue is IEditChoiceClass)
                    {
                        prevValue = (prevValue as IEditChoiceClass).RefValue;
                    }

                    prevValue = HumanReadableConverter.ToHuman(prevValue);
                }

                AimEditablePropertyModel model;
                if (aimObject is RulesProcedures && aimPropInfo.Name == "Content")
                {
                    model = new AimXhtmlEditablePropertyModel
                    {
                        ParentModel = this,
                        PropInfo = aimPropInfo,
                        ParentObject = aimObject,
                        GroupName = "Common properties",
                        WasDelta = isMasked,
                        PreviousValueAsString = (string)prevValue
                    };
                }
                else if (aimObject is TextNote && aimPropInfo.Name == "Value")
                {
                    model = new AimMultiLineEditablePropertyModel()
                    {
                        ParentModel = this,
                        PropInfo = aimPropInfo,
                        ParentObject = aimObject,
                        GroupName = "Common properties",
                        WasDelta = isMasked,
                        PreviousValueAsString = (string)prevValue
                    };
                }
                else
                {
                    model = new AimEditablePropertyModel
                    {
                        ParentModel = this,
                        PropInfo = aimPropInfo,
                        ParentObject = aimObject,
                        GroupName = "Common properties",
                        WasDelta = isMasked,
                        PreviousValueAsString = (string)prevValue
                    };
                }

                var reason = (aimObject as Feature)?.GetNilReason(aimPropInfo.Index);
                if (reason != null)
                {
                    model.OldNilReason = (NilReason)reason;
                }


                if (aimObject is Feature && TimeSliceNames.Contains(aimPropInfo.Name.ToLower()))
                {
                    model.GroupName = "TimeSlice properties";
                    model.IsReadOnly = true;
                    model.DeltaColumnVisibility = Visibility.Hidden;
                }

                if (aimPropInfo.IsFeatureReference)
                {
                    model.GroupName = "References";
                }

                newPropertyList.Add(model);
            }



            var geoProperty = aimObject.GetType().GetProperty("Geo");
            if (geoProperty != null)
            {

                var geoValue = geoProperty.GetValue(aimObject, null);

                var geoDescription = "";
                if (geoValue != null)
                {
                    geoDescription = geoValue.GetType().Name;
                }

                var geoModel = new ReflectionEditablePropertyModel
                {
                    PropertyInfo = geoProperty,
                    PasteToolTip = "Paste " + geoDescription,
                    ParentObject = aimObject,
                    GroupName = "Geometry properties",
                    ParentModel = this,
                    WasDelta = true,
                    PropertyDescription = "Geometry.",
                    GeoTextValue = geoDescription
                };
                newPropertyList.Add(geoModel);

                //var geoSubProperies=geoValue.GetType().GetProperties();

                //foreach (var geoSubProperty in geoSubProperies)
                //{
                //    if (geoSubProperty.CanWrite && geoSubProperty.Name!="Item")
                //    {

                //    }
                //}
            }

            //add Metadata
            if (aimObject is Feature)
            {

                var prop = aimObject.GetType().GetProperty("TimeSliceMetadata");

                var isMasked = true;
                if (DeltaMask != null)
                {
                    isMasked = DeltaMask.IsMasked(prop);
                }

                var model = new ReflectionEditablePropertyModel
                {
                    PropertyInfo = prop,
                    ParentObject = EditedFeature,
                    GroupName = "Metadata",
                    ParentModel = this,
                    WasDelta = isMasked
                };
                newPropertyList.Add(model);
            }

            return newPropertyList;
        }

        private void UpdateFeature()
        {
            if (EditedFeature == null) return;

            List<EditableSinglePropertyModel> newPropertyList = null;

            if (EditedFeature is IList)
            {
                var list = EditedFeature as IList;
                if (list.Count == 0 || !(list[0] is FeatureRefObject))
                {
                    newPropertyList = PrepareListProperties();
                }
                else
                {
                    newPropertyList = PrepareLinkListProperties();
                }
            }
            else
            {
                newPropertyList = PrepareAimProperties();
            }


            //TODO: improve performance?
            Application.Current.Dispatcher.Invoke(delegate
            {
                PropertyList.Clear();
                foreach (var item in newPropertyList)
                {
                    PropertyList.Add(item);
                }
            });
        }

        public virtual void UpdateTitle()
        {
        }

        #endregion

        #region Overrides of HierarchyDocViewerModel

        public override void OnCurrentViewerSet()
        {
        }


        //public override void UpdateListByChildren()
        //{
        //    if (SelectedProperty != null)
        //    {
        //        SelectedProperty.OnApply();
        //    }

        //    //bubble to parent
        //    if (ParentModel != null)
        //    {
        //        ParentModel.UpdateListByChildren();
        //    }
        //}

        public override void UpdateByChildren()
        {
            if (SelectedProperty != null)
            {
                SelectedProperty.UpdateStringValue();
                SelectedProperty.OnApply();
            }


            //bubble to parent
            if (ParentModel != null)
            {
                ParentModel.UpdateByChildren();
            }
            else
            {
                if (SelectedProperty != null)
                {
                    SelectedProperty.IsChangedByChildren = true;
                }
                UpdateTitle();
                UpdateChangedStatus();
            }
        }

        public bool IsChangedByChildren { get; set; }

        public bool IsChanged { get; set; }

        private void UpdateChangedStatus()
        {
            IsChanged = IsChangedByChildren || PropertyList.Any(propertyModel => propertyModel.IsChanged);
            UpdateDirtyStatus();
        }

        public virtual void UpdateDirtyStatus()
        {
        }

        #endregion

        #region Functions called by children or other UI elements

        public void UpdateSelected()
        {
            if (SelectedProperty == null) return;
            OnPropertyChanged("SelectedProperty");
            UpdateComplexContent(false);
        }

        #endregion

    }
}
