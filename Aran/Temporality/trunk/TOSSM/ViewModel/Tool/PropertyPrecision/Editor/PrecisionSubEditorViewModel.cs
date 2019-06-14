using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.PropertyPrecision;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.Converter;
using TOSSM.ViewModel.Document.Single.ReadOnly;
using TOSSM.ViewModel.Tool.PropertyPrecision.Single;
using TOSSM.ViewModel.Tool.PropertyPrecision.Util;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Editor
{
    public class PrecisionSubEditorViewModel : HierarchyToolViewerModel
    {
        
        #region Ctor

        public static string ToolContentId = "Precision Editor";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/gear.png", UriKind.RelativeOrAbsolute);


        public PrecisionSubEditorViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }

        #endregion


        #region Load

        public override void Load()
        {
            if (IsLoaded) return;

            ColumnCollection.Clear();
            ColumnCollection.Add(PropertyColumn);
            ColumnCollection.Add(FormatColumn);
        }

        #endregion


        public ComplexPropertyConfiguration PropertyConfiguration { get; set; }


        public override void OnCurrentViewerSet()
        {
        }

        public override void UpdateByChildren()
        {
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
                UpdateChangedStatus();
            }
        }

        private void UpdateChangedStatus()
        {
            IsChanged = PropertyList.Any(t => t.IsChangedByChildren);
        }

        public bool IsChanged { get; set; }

        #region Functions called by children or other UI elements

        public void UpdateSelected()
        {
            if (SelectedProperty == null) return;
            OnPropertyChanged("SelectedProperty");
            UpdateComplexContent();
        }

        #endregion



        private List<SinglePropertyPrecisionViewModel> PrepareAimProperties()
        {
            var newPropertyList = new List<SinglePropertyPrecisionViewModel>();

            if (EditedFeature==null) return null;
            if (PropertyConfiguration == null) return null;


            int objectType = (int) EditedFeature;

            var aimPropInfoArr = AimMetadata.GetAimPropInfos(objectType);
            //add Aim properties
            foreach (var aimPropInfo in aimPropInfoArr)
            {
                if (String.IsNullOrEmpty(aimPropInfo.AixmName)) continue;

                if (aimPropInfo.PropType.Name == "SysDouble")
                {
                    var model = new DoublePropertyPrecisionViewModel
                    {
                        PropInfo = aimPropInfo,
                        GroupName = "Doubles",
                        ParentModel = this,
                        PictureId = SingleModelBase.SimplePicture
                    };

                    if (PropertyConfiguration.ObjectConfiguration.Properties.ContainsKey(aimPropInfo.Index))
                    {
                        var propertyConfiguration = PropertyConfiguration.ObjectConfiguration.Properties[aimPropInfo.Index] as DoublePropertyConfiguration;
                        if (propertyConfiguration != null)
                        { 
                            model.PrecisionFormat = propertyConfiguration.PrecisionFormat;
                        }
                    }

                    
                    newPropertyList.Add(model);
                }
                else if (aimPropInfo.PropType.SubClassType == AimSubClassType.ValClass)
                {
                    var enumType = AimMetadata.GetEnumType(aimPropInfo.PropType.Properties[1].PropType.Index);
                    var enumList = new List<DoublePropertyPrecisionViewModel>();
                    
                    var model = new ValPropertyPrecisionViewModel
                    {
                        PropInfo = aimPropInfo,
                        GroupName = "Value Classes",
                        ParentModel = this,
                        PictureId = SingleModelBase.ListPicture,
                    };

                    foreach (var en in Enum.GetValues(enumType))
                    {
                        var enumModel=new DoublePropertyPrecisionViewModel
                        {
                            Enum = (Enum) en,
                            EnumDescription = GetEnumValueDescription(aimPropInfo,(Enum)en),
                            ParentModel = this,
                            ValPropertyPrecisionViewModel = model
                        };
                        
                        int enumValue = Convert.ToInt32((Enum) en);
                        if (PropertyConfiguration.ObjectConfiguration.Properties.ContainsKey(aimPropInfo.Index))
                        {
                            var propertyConfiguration = PropertyConfiguration.ObjectConfiguration.Properties[aimPropInfo.Index] as ValPropertyConfiguration;
                            if (propertyConfiguration != null && 
                                propertyConfiguration.EnumProperties!=null &&
                                propertyConfiguration.EnumProperties.ContainsKey(enumValue))
                            {
                                enumModel.PrecisionFormat =
                                    propertyConfiguration.EnumProperties[enumValue].PrecisionFormat;
                            }
                        }

                        enumList.Add(enumModel);
                    }

                    model.EnumItems = enumList;
                    newPropertyList.Add(model);
                }
                else if  (aimPropInfo.PropType.AimObjectType == AimObjectType.Object ||
                    aimPropInfo.PropType.AimObjectType == AimObjectType.DataType)
                {
                    if (IsDoubleChildProperties(aimPropInfo.PropType.Index))
                    {
                        var model = new ComplexPropertyPrecisionViewModel
                        {
                            PropInfo = aimPropInfo,
                            GroupName = "Complex Objects",
                            ParentModel = this,
                            PictureId = SingleModelBase.ComplexPicture
                        };
                        newPropertyList.Add(model);
                    }
                }
            }


            return newPropertyList.OrderBy(t=>t.GroupName).ThenBy(t=>t.PropertyName).ToList();
        }


        private static string GetEnumValueDescription(AimPropInfo propInfo, Enum enumValue)
        {
            string result;
            var index = Convert.ToInt32(enumValue);
            if (propInfo.PropType.Properties[1].PropType.EnumValues != null)
            {
                var enumDescription = propInfo.PropType.Properties[1].PropType.EnumValues.FirstOrDefault(t => t.EnumIndex == index);
                if (enumDescription != null)
                {
                    result = String.IsNullOrWhiteSpace(enumDescription.Documentation) ?
                        HumanReadableConverter.ToHuman(enumValue) : enumDescription.Documentation;
                }
                else
                {
                    result = HumanReadableConverter.ToHuman(enumValue);
                }
            }
            else
            {
                result = HumanReadableConverter.ToHuman(enumValue);
            }
            return result;
        }

        public static bool IsDoubleChildProperties(int classIndex, HashSet<int> accounted=null)
        {
            if (accounted==null) accounted=new HashSet<int>();
            if (!accounted.Add(classIndex)) return false;

            var aimPropInfoArr = AimMetadata.GetAimPropInfos(classIndex);
            foreach (var aimPropInfo in aimPropInfoArr)
            {
                if (aimPropInfo.PropType.Name == "SysDouble")
                {
                    return true;
                }

                if (aimPropInfo.PropType.SubClassType == AimSubClassType.ValClass)
                {
                    return true;
                }

                if (aimPropInfo.PropType.AimObjectType == AimObjectType.Object ||
                    aimPropInfo.PropType.AimObjectType == AimObjectType.DataType)
                {
                    if (IsDoubleChildProperties(aimPropInfo.PropType.Index, accounted))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void UpdateFeature()
        {
            if (EditedFeature == null) return;

            var newPropertyList = PrepareAimProperties();

            PropertyList.Clear();
            foreach (var item in newPropertyList)
            {
                PropertyList.Add(item);
            }
        }



        #region Properties


        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }

       

        private int? _editedFeature;
        public int? EditedFeature
        {
            get => _editedFeature;
            set
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

                UpdateFeature();
            }
        }

        public void UpdateComplexContent()
        {
            if (ComplexContent != null)
            {
                ComplexContent.Visibility = Visibility.Collapsed;
            }
           
            var model = SelectedProperty as ComplexPropertyPrecisionViewModel;
            if (model != null)
            {
                ComplexContent = PrecisionSubViewerProvider.GetSubViewer(ComplexContent, model, CurrentViewer);
            }


            OnPropertyChanged("ComplexContent");
        }

        private SinglePropertyPrecisionViewModel _selectedProperty;
        public SinglePropertyPrecisionViewModel SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (_selectedProperty == value) return;
                _selectedProperty = value;
                OnPropertyChanged("SelectedProperty");
                UpdateComplexContent();
            }
        }

        private MtObservableCollection<SinglePropertyPrecisionViewModel> _propertyList;
        public MtObservableCollection<SinglePropertyPrecisionViewModel> PropertyList
        {
            get => _propertyList ?? (_propertyList = new MtObservableCollection<SinglePropertyPrecisionViewModel>());
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


        #region Columns

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


        private DataGridColumn _formatColumn;
        public DataGridColumn FormatColumn
        {
            get
            {
                if (_formatColumn == null)
                {
                    if (CurrentViewer == null) return null;
                    _formatColumn = new DataGridTemplateColumn
                    {
                        Width = DataGridLength.Auto,
                        Header = "Format",
                        CellTemplateSelector = PrecisionEditorPropertyTemplateSelector.Instance
                    };
                }
                return _formatColumn;
            }
        }

        private MtObservableCollection<DataGridColumn> _columnCollection;
        public MtObservableCollection<DataGridColumn> ColumnCollection
        {
            get => _columnCollection ?? (_columnCollection = new MtObservableCollection<DataGridColumn>());
            set => _columnCollection = value;
        }


        #endregion

        public void FormatChanged(DoublePropertyPrecisionViewModel model)
        {

            if (model.ValPropertyPrecisionViewModel == null)
            {
                PropertyConfiguration propertyConfiguration;
                if (!PropertyConfiguration.ObjectConfiguration.Properties.TryGetValue(model.PropInfo.Index, out  propertyConfiguration))
                {
                    propertyConfiguration = new DoublePropertyConfiguration();
                    PropertyConfiguration.ObjectConfiguration.Properties[model.PropInfo.Index] = propertyConfiguration;
                }

                var doublePropertyConfiguration = propertyConfiguration as DoublePropertyConfiguration;
                if (doublePropertyConfiguration != null)
                {
                    doublePropertyConfiguration.PrecisionFormat = model.PrecisionFormat;
                    UpdateByChildren();
                }
            }
            else 
            {
                PropertyConfiguration propertyConfiguration;
                if (!PropertyConfiguration.ObjectConfiguration.Properties.TryGetValue(model.ValPropertyPrecisionViewModel.PropInfo.Index, out  propertyConfiguration))
                {
                    propertyConfiguration = new ValPropertyConfiguration();
                    PropertyConfiguration.ObjectConfiguration.Properties[model.ValPropertyPrecisionViewModel.PropInfo.Index] = propertyConfiguration;
                }

                var valPropertyConfiguration = propertyConfiguration as ValPropertyConfiguration;
                if (valPropertyConfiguration != null)
                {
                    EnumSubProperty enumSubProperty;
                    var enumValue = Convert.ToInt32(model.Enum);

                    if (!valPropertyConfiguration.EnumProperties.TryGetValue(enumValue, out enumSubProperty))
                    {
                        enumSubProperty=new EnumSubProperty
                        {
                            Enum = enumValue
                        };
                        valPropertyConfiguration.EnumProperties[enumValue] = enumSubProperty;
                    }

                    enumSubProperty.PrecisionFormat = model.PrecisionFormat;
                }
            }

           
        }
    }
}
