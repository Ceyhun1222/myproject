using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Aim.PropertyPrecision;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Validation;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.ViewModel.Document.Single.ReadOnly;

namespace TOSSM.ViewModel.Document.Single.Editable
{
    public class ChoiceCreateMenuItem : ViewModelBase
    {
        public Action<ChoiceCreateMenuItem> OnCommand { get; set; }

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        private RelayCommand _command;
        public RelayCommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand(
                t =>
                   {
                       if (OnCommand != null)
                       {
                           OnCommand(this);
                       }
                   }));
            }
            set => _command = value;
        }

        public AimPropInfo PropInfo { get; set; }
    }

    public class AbstactCreateMenuItem : ViewModelBase
    {
        public Action<AbstactCreateMenuItem> OnCommand { get; set; }

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }

        private RelayCommand _command;
        public RelayCommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand(
                t =>
                {
                    if (OnCommand != null)
                    {
                        OnCommand(this);
                    }
                }));
            }
            set => _command = value;
        }

        public AimClassInfo ClassInfo { get; set; }
    }

    public class CustomPropertyToolTip
    {
        public static Dictionary<string, string> CustomToolTips = new Dictionary<string, string>
        {
            //{ "Identifier", "Identifier" },
            //{ "TimeSlice", "TimeSlice" }
        };
    }

    public class AimEditablePropertyModel : EditableSinglePropertyModel
    {
        #region ValType

        public Type ValEnumType { get; set; }

        private object _valEnum;
        public object ValEnum
        {
            get => _valEnum;
            set
            {
                _valEnum = value;

                if (value != null)
                {
                    var newValue = AimObjectFactory.Create(PropInfo.PropType.Index);
                    var val = newValue as IEditValClass;
                    if (val != null)
                    {
                        val.Uom = (int)ValEnum;
                        val.Value = ValDoubleValue;
                    }

                    Value = newValue;
                }

                OnPropertyChanged("ValEnum");
                OnPropertyChanged("IsChanged");
                OnPropertyChanged("ChangedVisibility");
                OnPropertyChanged("IsNotNull");
                OnPropertyChanged("NullReasonVisibility");
                OnPropertyChanged("MainValueVisibility");

            }
        }

        public IList ValEnumList { get; set; }

        private double _valDoubleValue;
        public double ValDoubleValue
        {
            get => _valDoubleValue;
            set
            {

                _valDoubleValue = value;

                if (Value == null)
                {
                    Value = AimObjectFactory.Create(PropInfo.PropType.Index);
                }

                var val = (IEditValClass)Value;
                val.Value = ValDoubleValue;

                CheckIsNull();
                SetValueToParent();

                OnPropertyChanged("Value");
                OnPropertyChanged("ValDoubleValue");
                OnPropertyChanged("IsChanged");
                OnPropertyChanged("ChangedVisibility");
                OnPropertyChanged("IsNotNull");
                OnPropertyChanged("MainValueVisibility");
                OnPropertyChanged("NullReasonVisibility");
            }
        }

        #endregion

        private Visibility _choiceReferenceVisibility = Visibility.Collapsed;
        public Visibility ChoiceReferenceVisibility
        {
            get => _choiceReferenceVisibility;
            set
            {
                _choiceReferenceVisibility = value;
                OnPropertyChanged("ChoiceReferenceVisibility");
            }
        }

        #region Helpers

        private bool IsRoot()
        {
            if (ParentModel == null) return true;
            return ParentModel.ParentModel == null;
        }

        private bool IsValType()
        {
            if (PropInfo == null) return false;
            return PropInfo.PropType.SubClassType == AimSubClassType.ValClass;
        }

        private bool IsEnum()
        {
            if (PropInfo == null) return false;
            return PropInfo.PropType.SubClassType == AimSubClassType.Enum;
        }

        #endregion

        private AimPropInfo _choicePropInfo;


        #region Special menus

        private MtObservableCollection<ChoiceCreateMenuItem> _choiceCreateMenu;
        public MtObservableCollection<ChoiceCreateMenuItem> ChoiceCreateMenu
        {
            get => _choiceCreateMenu ?? (_choiceCreateMenu = new MtObservableCollection<ChoiceCreateMenuItem>());
            set => _choiceCreateMenu = value;
        }


        private MtObservableCollection<AbstactCreateMenuItem> _abstractCreateMenu;
        public MtObservableCollection<AbstactCreateMenuItem> AbstractCreateMenu
        {
            get => _abstractCreateMenu ?? (_abstractCreateMenu = new MtObservableCollection<AbstactCreateMenuItem>());
            set => _abstractCreateMenu = value;
        }

        #endregion

        #region Overrides EditableSinglePropertyModel


        public override bool CanPaste()
        {
            if (MainManagerModel.Instance.BufferFeatureType == null) return false;


            //check for type

            if (MainManagerModel.Instance.BufferFeatureType != PropInfo.ReferenceFeature
                && MainManagerModel.Instance.BufferFeatureType != ReferenceFeature)
            {
                if (PropInfo.IsFeatureReference && AimMetadata.IsAbstractFeatureRef(PropInfo.TypeIndex))
                {
                    bool found = CanPropertyAccept(PropInfo);
                    if (!found)
                        return false;
                }
                else if (AimMetadata.IsChoice(PropInfo.TypeIndex))
                {
                    bool found = false;
                    if (_choicePropInfo != null)
                        foreach (var propInfo in PropInfo.PropType.Properties.Where(t => t.Name == _choicePropInfo.Name))
                        {
                            if (propInfo.IsFeatureReference && AimMetadata.IsAbstractFeatureRef(propInfo.TypeIndex))
                            {
                                found = CanPropertyAccept(propInfo);
                                if (found)
                                    break;
                            }
                        }
                    if (!found)
                        return false;
                }
                else
                    return false;
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
        }

        private bool CanPropertyAccept(AimPropInfo propInfo)
        {
            AimClassInfo classInfo = propInfo.PropType;

            string s = classInfo.Name.Substring("Abstract".Length);
            s = s.Substring(0, s.Length - "Ref".Length);
            string absEnumTypeName = "Aran.Aim." + s + "Type";

            Type enumType = typeof(FeatureType).Assembly.GetType(absEnumTypeName);
            Array enumItemArr = Enum.GetValues(enumType);

            bool found = false;
            for (int i = 0; i < enumItemArr.Length; i++)
            {
                int featureTypeIndex = (int)enumItemArr.GetValue(i);
                if (featureTypeIndex == (int)MainManagerModel.Instance.BufferFeatureType.Value)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public override void Paste()
        {
            if (!CanPaste())
                return;

            FeatureRef reference;

            if (PropInfo.IsFeatureReference && AimMetadata.IsAbstractFeatureRef(PropInfo.TypeIndex))
            {
                reference = (FeatureRef)AimObjectFactory.CreateADataType(
(DataType)PropInfo.TypeIndex);
                reference.FeatureType = MainManagerModel.Instance.BufferFeatureType;
                ((IAbstractFeatureRef)reference).FeatureTypeIndex = (int)MainManagerModel.Instance.BufferFeatureType.Value;
                reference.Identifier = MainManagerModel.Instance.BufferFeatureIdentifier;
            }
            else if (AimMetadata.IsChoice(PropInfo.TypeIndex))
            {
                bool found = false;
                AimPropInfo currentProp = null;
                foreach (var propInfo in PropInfo.PropType.Properties.Where(t => t.Name == _choicePropInfo.Name))
                {
                    if (propInfo.IsFeatureReference && AimMetadata.IsAbstractFeatureRef(propInfo.TypeIndex))
                    {
                        found = CanPropertyAccept(propInfo);
                        if (found)
                        {
                            currentProp = propInfo;
                            break;
                        }

                    }
                }
                if (currentProp == null)
                    reference = new FeatureRef(MainManagerModel.Instance.BufferFeatureIdentifier);
                else
                {
                    reference = (FeatureRef)AimObjectFactory.CreateADataType((DataType)currentProp.TypeIndex);
                    reference.FeatureType = MainManagerModel.Instance.BufferFeatureType;
                    ((IAbstractFeatureRef)reference).FeatureTypeIndex = (int)MainManagerModel.Instance.BufferFeatureType.Value;
                    reference.Identifier = MainManagerModel.Instance.BufferFeatureIdentifier;
                }
            }
            else
                reference = new FeatureRef(MainManagerModel.Instance.BufferFeatureIdentifier);

            if (PropInfo.IsList)
            {
                var list = new List<object>();
                var vList = Value as IList;
                if (vList != null)
                    list.AddRange(vList.Cast<object>());

                list.Add(new FeatureRefObject
                {
                    Feature = reference
                });

                Value = list;
            }
            else
            {
                Value = reference;
            }
        }

        public override void SetNew()
        {
            base.SetNew();
            if (PropInfo == null) return;

            if (PropInfo.IsList)
            {
                Value = new ArrayList();
            }
            else
            {
                object newValue = AimObjectFactory.Create(PropInfo.PropType.Index);
                //unwrap field
                if (newValue is IEditAimField)
                {
                    newValue = (newValue as IEditAimField).FieldValue;
                }

                Value = newValue;
            }
        }

        public override void SetValueToParent(bool fromUser = true)
        {
            if (!(ParentObject is IAimObject))
            {
                return;
            }



            if (IsValType())
            {
                if (Value == null)
                {
                    _valDoubleValue = 0;
                    _valEnum = null;
                }
                else
                {
                    var val = (IEditValClass)Value;
                    _valDoubleValue = val.Value;
                    _valEnum = Enum.ToObject(ValEnumType, val.Uom);
                }

                OnPropertyChanged("ValEnum");
                OnPropertyChanged("ValDoubleValue");
            }

            if (GettingValueFromParent)
            {
                CheckIsNull();
                //update visibility
                if (IsRoot())
                {
                    OnPropertyChanged("IsChanged");
                    OnPropertyChanged("ChangedVisibility");
                }

                OnPropertyChanged("NullReasonVisibility");
                OnPropertyChanged("MainValueVisibility");

                return;
            }

            //set value
            if (Value == null)
            {
                (ParentObject as IAimObject).SetValue(PropInfo.Index, null);
            }
            else if (PropInfo.PropType.SubClassType == AimSubClassType.Choice)
            {

                //create new choice value
                var choiceValue = AimObjectFactory.Create(PropInfo.PropType.Index);

                if (_choicePropInfo != null)
                {
                    var pi = choiceValue.GetType().GetProperty(_choicePropInfo.Name);
                    pi.SetValue(choiceValue, Value, null);
                    (ParentObject as IAimObject).SetValue(PropInfo.Index, choiceValue as IAimProperty);
                }
                else
                {
                    var property = (ParentObject as IAimObject).GetValue(PropInfo.Index) as IEditChoiceClass;
                    property.RefValue = Value as IAimProperty;
                }
            }
            else if (Value is IAimProperty)
            {
                (ParentObject as IAimObject).SetValue(PropInfo.Index, Value as IAimProperty);
            }
            else
            {
                //create property
                var aimProperty = AimObjectFactory.CreateAimProperty(PropInfo);

                //set value to propertry
                switch (aimProperty.PropertyType)
                {
                    case AimPropertyType.AranField:
                        var aimField = aimProperty as IEditAimField;

                        var newValue = Value;

                        var fieldType = aimField.GetType();
                        if (fieldType.IsGenericType)
                        {
                            var args = fieldType.GetGenericArguments();
                            if (args.Length > 0)
                            {
                                var genericType = args[0];
                                newValue = Convert.ChangeType(newValue, genericType);
                            }
                        }

                        aimField.FieldValue = newValue;
                        break;

                    case AimPropertyType.List:
                        var list = (aimProperty as IList);
                        var items = Value as IList;
                        if (list != null && items != null)
                        {
                            foreach (var item in items)
                            {
                                list.Add(item);
                            }
                        }
                        ResetValue(list);
                        break;

                    default:
                        throw new Exception("Strange property type");
                }

                //set property
                (ParentObject as IAimObject).SetValue(PropInfo.Index, aimProperty);

                //if (ParentModel!=null)
                //{
                //    ParentModel.UpdateByChildren();
                //}
            }

            if (IsEnum())
            {
                UpdateEnumValue();
                UpdateEnumValueDescription();
            }

            if (IsValType())
            {
                UpdateValEnumValueDescription();
            }

            CheckIsNull();
            //update visibility
            if (IsRoot())
            {
                OnPropertyChanged("ChangedVisibility");
            }

            OnPropertyChanged("NullReasonVisibility");
            OnPropertyChanged("MainValueVisibility");

            //update parent
            //string value will be updated as well
            if (ParentModel != null)
            {
                if (ParentModel.SelectedProperty == this)
                {
                    if (fromUser)
                    {
                        ParentModel.UpdateByChildren();
                        ParentModel.UpdateSelected();
                    }
                }
            }

            UpdateReferenceString();
        }

        public override void UpdateStringValue()
        {
            if (PictureId == ListPicture || PictureId == ComplexPicture)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (a, b) =>
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    StringValue = HumanReadableConverter.ToHuman(Value);
                };
                worker.RunWorkerAsync();
            }
        }

        private bool GettingValueFromParent { get; set; }

        public override void GetValueFromParent()
        {
            GettingValueFromParent = true;
            //not now
            if (ParentObject == null) return;

            //set value
            if (ParentObject is AimObject)
            {
                object myValue = (ParentObject as IAimObject).GetValue(PropInfo.Index);

                if (myValue is IEditAimField)
                {
                    myValue = (myValue as IEditAimField).FieldValue;
                    if (myValue is uint || myValue is int || myValue is Int64)
                    {
                        myValue = Convert.ToDecimal(myValue);
                    }
                }

                if (myValue is IEditChoiceClass)
                {
                    var refType = (myValue as IEditChoiceClass).RefType;

                    myValue = (myValue as IEditChoiceClass).RefValue;
                    if (refType > 0 && myValue is FeatureRef)
                    {
                        if (myValue is IAbstractFeatureRef)
                        {
                            ReferenceFeature = (FeatureType)((IAbstractFeatureRef)myValue).FeatureTypeIndex;
                        }
                        else
                            ReferenceFeature = (FeatureType)refType;
                    }
                }

                PropertyConfiguration propertyConfiguration = null;
                if (ParentModel?.Configuration?.ObjectConfiguration?.Properties != null && ParentModel.Configuration.ObjectConfiguration.Properties.ContainsKey(PropInfo.Index))
                {
                    propertyConfiguration = ParentModel.Configuration.ObjectConfiguration.Properties[PropInfo.Index];
                }

                if (IsRoot())
                {
                    OldValue = myValue;
                    OldValueAsString = HumanReadableConverter.ToHuman(OldValue, propertyConfiguration);
                }
                else
                {
                    Value = myValue;
                }

                if (IsEnum())
                {
                    UpdateEnumValue();
                    UpdateEnumValueDescription();
                }

                if (IsValType())
                {
                    UpdateValEnumValueDescription();
                }

                UpdateReadonlyToolTip();

                var worker = new BackgroundWorker();
                worker.DoWork += (a, b) =>
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    StringValue = HumanReadableConverter.ToHuman(Value, propertyConfiguration);
                    UpdateReferenceString();
                };
                worker.RunWorkerAsync();
            }

            GettingValueFromParent = false;
        }



        #endregion

        #region Tooltips

        private string _enumTypeDescription;
        public string EnumTypeDescription
        {
            get => _enumTypeDescription;
            set
            {
                _enumTypeDescription = value;
                OnPropertyChanged("EnumTypeDescription");
            }
        }

        private string _enumValueDescription;
        public string EnumValueDescription
        {
            get => _enumValueDescription;
            set
            {
                _enumValueDescription = value;
                OnPropertyChanged("EnumValueDescription");
            }
        }

        private string _valEnumTypeDescription;
        public string ValEnumTypeDescription
        {
            get => _valEnumTypeDescription;
            set
            {
                _valEnumTypeDescription = value;
                OnPropertyChanged("ValEnumTypeDescription");
            }
        }

        private string _valEnumValueDescription;
        public string ValEnumValueDescription
        {
            get => _valEnumValueDescription;
            set
            {
                _valEnumValueDescription = value;
                OnPropertyChanged("ValEnumValueDescription");
            }
        }


        private void UpdateValEnumTypeDescription(Type type)
        {
            ValEnumTypeDescription = PropInfo.PropType.Properties[1].PropType.Documentation;
        }

        private void UpdateValEnumValueDescription()
        {
            if (!(Value is IEditValClass)) return;

            var val = (IEditValClass)Value;
            //_valDoubleValue = val.Value;
            _valEnum = Enum.ToObject(ValEnumType, val.Uom);
            var index = (int)_valEnum;

            var enumDescription = PropInfo.PropType.Properties[1].PropType.EnumValues.Where(t => t.EnumIndex == index).
                FirstOrDefault();

            if (enumDescription != null)
            {
                ValEnumValueDescription = enumDescription.Documentation;
            }
            else
            {
                ValEnumValueDescription = null;
            }
        }

        private void UpdateEnumTypeDescription(Type type)
        {
            EnumTypeDescription = PropInfo.PropType.Documentation;
        }

        private void UpdateEnumValueDescription()
        {
            if (Value == null)
            {
                EnumValueDescription = null;
                return;
            }

            var index = Convert.ToInt32(Value);
            if (PropInfo.PropType.EnumValues != null)
            {
                var enumDescription = PropInfo.PropType.EnumValues.FirstOrDefault(t => t.EnumIndex == index);
                if (enumDescription != null)
                {
                    EnumValueDescription = String.IsNullOrWhiteSpace(enumDescription.Documentation) ?
                        HumanReadableConverter.ToHuman(Value) : enumDescription.Documentation;
                }
                else
                {
                    EnumValueDescription = HumanReadableConverter.ToHuman(Value);
                }
            }
            else
            {
                EnumValueDescription = HumanReadableConverter.ToHuman(Value);
            }
        }

        #endregion

        #region Property related

        private void SetPropertyName()
        {
            if (PropInfo == null) return;

            if (PropInfo.PropType.SubClassType == AimSubClassType.Choice)
            {
                AimPropInfo choicePropInfo = null;

                var ecc = Value as IEditChoiceClass;
                if (ecc != null)
                {
                    var classInfo = AimMetadata.GetClassInfoByIndex(Value as IAimObject);
                    choicePropInfo = classInfo.Properties.Where(pi => pi.PropType.Index == ecc.RefType).FirstOrDefault();
                }

                if (choicePropInfo != null)
                {
                    PropertyName = PropInfo.Name + " (" + choicePropInfo.Name + ")";
                }
                else
                {
                    PropertyName = PropInfo.Name + " (Choice)";
                }
            }
            else if (PropInfo.PropType.IsAbstract)
            {
                PropertyName = PropInfo.Name + " (Abstract)";
            }
            else
            {
                PropertyName = PropInfo.Name;
            }

        }

        private void OnChoiceMenuCommand(ChoiceCreateMenuItem item)
        {
            _choicePropInfo = item.PropInfo;
            Value = AimObjectFactory.Create(item.PropInfo.PropType.Index);
            if (Value is FeatureRef || Value is FeatureRefObject)
            {
                StringValue = "Reference to " + item.PropInfo.ReferenceFeature;
                ReferenceFeature = item.PropInfo.ReferenceFeature;
                ChoiceReferenceVisibility = Visibility.Visible;
            }
            else
            {
                ChoiceReferenceVisibility = Visibility.Collapsed;
            }
        }


        private void OnAbstactMenuCommand(AbstactCreateMenuItem item)
        {
            Value = AimObjectFactory.Create(item.ClassInfo.Index);
        }


        public override void OnReferenceFeatureChanged()
        {
            base.OnReferenceFeatureChanged();

            UpdateReferenceString();
        }

        private void UpdateReferenceString()
        {
            if ((int)ReferenceFeature > 0)
            {
                if (Value is FeatureRef || Value is FeatureRefObject)
                {
                    StringValue = "Reference to " + ReferenceFeature;
                    PasteToolTip = "Paste reference to " + ReferenceFeature;
                }
            }
        }

        private void PrepareRelatedData()
        {
            if (PropInfo == null) return;


            if (PropInfo.IsFeatureReference && (int)PropInfo.ReferenceFeature > 0)
            {
                ReferenceFeature = PropInfo.ReferenceFeature;
            }

            if (PropInfo.PropType.IsAbstract)
            {
                var abstractChilds = AimMetadataUtility.GetAbstractChilds(PropInfo.PropType);
                AbstractCreateMenu.Clear();
                foreach (var p in abstractChilds)
                {
                    if (!string.IsNullOrEmpty(p.AixmName))
                    {
                        AbstractCreateMenu.Add(new AbstactCreateMenuItem
                        {
                            ClassInfo = p,
                            Header = p.Name,
                            OnCommand = OnAbstactMenuCommand
                        });
                    }
                }

            }


            if (PropInfo.PropType.SubClassType == AimSubClassType.Choice)
            {
                ChoiceCreateMenu.Clear();
                foreach (AimPropInfo p in PropInfo.PropType.Properties)
                {
                    if (!string.IsNullOrEmpty(p.AixmName))
                    {
                        ChoiceCreateMenu.Add(new ChoiceCreateMenuItem
                        {
                            PropInfo = p,
                            Header = p.Name,
                            OnCommand = OnChoiceMenuCommand
                        });
                    }
                }

            }



            StringValueValidationRule = new StringValueValidationRule();

            if (PropInfo.Restriction.Max != null)
            {
                StringValueValidationRule.MaximumLength = (int)PropInfo.Restriction.Max;
                NumericMaximum = PropInfo.Restriction.Max;
            }

            if (PropInfo.Restriction.Min != null)
            {
                StringValueValidationRule.MinimumLength = (int)PropInfo.Restriction.Min;
                NumericMinimum = PropInfo.Restriction.Min;
            }

            if (PropInfo.Restriction.Pattern != null)
            {
                StringValueValidationRule.Pattern = PropInfo.Restriction.Pattern;
            }




            PropertyDescription = CustomPropertyToolTip.CustomToolTips.ContainsKey(PropInfo.Name) ?
                CustomPropertyToolTip.CustomToolTips[PropInfo.Name] :
                string.IsNullOrEmpty(PropInfo.Documentation) ? PropInfo.Name : PropInfo.Documentation;



            if (IsEnum())
            {


                //PropInfo.TypeIndex == ()EnumType.Language
                var enumType = AimMetadata.GetEnumType(PropInfo.PropType.Index);
                UpdateEnumTypeDescription(enumType);

                EnumList = new List<EnumViewModel>();
                foreach (var en in Enum.GetValues(enumType))
                {
                    EnumList.Add(new EnumViewModel { Enum = en });
                }

                // Sorting enum list, 
                // Is Sort requiring for other enums too?
                // ToDo: change for all lists or make local properties.settings.param
                if (enumType.Name == "CodeRuleProcedureTitle")
                {
                    EnumList = EnumList.OrderBy(x => x.Description.Replace("GEN","1").Replace("ENR", "2").Replace("AD", "3")).ToList();
                }
            }
            else if (IsValType())
            {
                ValEnumType = AimMetadata.GetEnumType(PropInfo.PropType.Properties[1].PropType.Index);

                UpdateValEnumTypeDescription(ValEnumType);

                ValEnumList = new ArrayList();
                foreach (var en in Enum.GetValues(ValEnumType))
                {
                    ValEnumList.Add(en);
                }
            }


            var picture = SimplePicture;

            if (!IsValType() &&
                (PropInfo.PropType.AimObjectType == AimObjectType.Object ||
                PropInfo.PropType.AimObjectType == AimObjectType.DataType))
            {
                picture = ComplexPicture;
            }

            if (PropInfo.IsList)
            {
                picture = ListPicture;
            }


            if (IsReadOnly || IsParentReadOnly)
            {
                picture = LockedPicture;
            }



            PictureId = picture;
        }

        private void UpdateReadonlyToolTip()
        {
            ReadOnlyValueToolTipVisibility = Visibility.Hidden;
            ReadOnlyValueToolTips.Clear();


            if (IsEnum())
            {
                if (!string.IsNullOrEmpty(EnumTypeDescription))
                {
                    ReadOnlyValueToolTipVisibility = Visibility.Visible;
                    ReadOnlyValueToolTips.Add(new ReadOnlyValueToolTipViewModel { Header = "Type description:", Message = EnumTypeDescription + "\n" });
                    ReadOnlyValueToolTips.Add(new ReadOnlyValueToolTipViewModel
                    { Header = "Value description:", Message = EnumValueDescription + "\n" });
                }

            }

            if (ReadOnlyValueToolTips.Count > 0)
            {
                var lastMessage =
                    ReadOnlyValueToolTips[ReadOnlyValueToolTips.Count - 1].Message;
                if (lastMessage.EndsWith("\n"))
                {
                    ReadOnlyValueToolTips[ReadOnlyValueToolTips.Count - 1].Message = lastMessage.Substring(0, lastMessage.Length - 1);
                }
            }
        }



        private AimPropInfo _propInfo;
        public AimPropInfo PropInfo
        {
            get => _propInfo;
            set
            {
                _propInfo = value;

                if (PropInfo == null) return;



                GetValueFromParent();
                SetPropertyName();
                PrepareRelatedData();


            }
        }

        #endregion

        #region Validation

        private double? _numericMinimum;
        public double? NumericMinimum
        {
            get => _numericMinimum;
            set
            {
                _numericMinimum = value;
                OnPropertyChanged("NumericMinimum");
            }
        }

        private double? _numericMaximum;
        public double? NumericMaximum
        {
            get => _numericMaximum;
            set
            {
                _numericMaximum = value;
                OnPropertyChanged("NumericMaximum");
            }
        }

        public StringValueValidationRule StringValueValidationRule { get; set; }

        private Visibility _deltaColumnVisibility = Visibility.Visible;

        public Visibility DeltaColumnVisibility
        {
            get => _deltaColumnVisibility;
            set => _deltaColumnVisibility = value;
        }

        public override string ValidateValue()
        {
            if (Value is string)
            {
                var result = StringValueValidationRule.Validate(Value, CultureInfo.CurrentCulture);
                return !result.IsValid ? result.ErrorContent.ToString() : null;
            }

            return null;
        }

        #endregion
    }

    public class AimXhtmlEditablePropertyModel : AimEditablePropertyModel
    {

    }


    public class AimMultiLineEditablePropertyModel : AimEditablePropertyModel
    {

    }
}
