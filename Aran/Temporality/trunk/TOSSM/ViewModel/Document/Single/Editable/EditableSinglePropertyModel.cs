using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Util;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.ViewModel.Document.Editor;
using TOSSM.ViewModel.Document.Single.ReadOnly;

namespace TOSSM.ViewModel.Document.Single.Editable
{
    public class EnumViewModel : ViewModelBase
    {
        private object _enum;

        public object Enum
        {
            get => _enum;
            set
            {
                _enum = value;
                Description = Enum == null ? null : HumanReadableConverter.ToHuman(Enum);
            }
        }

        public string Description { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }

    public abstract class EditableSinglePropertyModel : ReadOnlySinglePropertyModel
    {
        private string _propertyDescription;
        public string PropertyDescription
        {
            get => _propertyDescription;
            set
            {
                _propertyDescription = value;
                OnPropertyChanged("PropertyDescription");
            }
        }

        public override void CheckIsNull()
        {
            IsNotNull = (Value != null);
           
            OnPropertyChanged("IsChanged");

            OnPropertyChanged("ChangedVisibility");
            OnPropertyChanged("NullReasonVisibility");
            OnPropertyChanged("MainValueVisibility");
        }

        #region Old Value

        

        private object _oldValue;
        public object OldValue
        {
            get => _oldValue;
            set
            {
                if (_oldValue==value) return;
                _oldValue = value;
                OnPropertyChanged("OldValue");
             
                //copy old to new
                RestoreOldValue();
            }
        }

        protected void RestoreOldValue()
        {
            IsDelta = WasDelta;

            NilReason = OldNilReason;

            Value = FormatterUtil.Clone(OldValue);

            IsChangedByChildren = false;
        }

        #endregion

        #region Filter

        public bool IsValueChanged()
        {
            if (IsChangedByChildren) return true;

            if (OldValue == null) return Value != null;
            if (Value == null) return true;
         
            //2 lists are equal (sould be changed by children)
            if (OldValue is IList && Value is IList)
            {
                return false;
            }

            return !OldValue.Equals(Value);
        }

        public bool IsDeltaChanged()
        {
            return WasDelta != IsDelta;
        }


        public bool IsChanged => IsNilReasonChanged() || IsValueChanged() || IsDeltaChanged();

        private bool IsNilReasonChanged()
        {
            if (!IsDelta) return false;
            if (OldValue!=null) return false;
            return OldNilReason != NilReason;
        }

        #endregion

        #region Commands

        private RelayCommand _onRestore;
        public RelayCommand OnRestore
        {
            get { return _onRestore ?? (_onRestore = new RelayCommand(t => RestoreOldValue())); }
            set => _onRestore = value;
        }

        public void OnApply()
        {
            CheckIsNull();
            SetValueToParent(false);
            //OnPropertyChanged("Value");
        }

        private RelayCommand _onSetNull;
        public RelayCommand OnSetNull
        {
            get { return _onSetNull ?? (_onSetNull = new RelayCommand(t => { Value = null; })); }
            set => _onSetNull = value;
        }

        private RelayCommand _onCreate;
        public RelayCommand OnCreate
        {
            get { return _onCreate ?? (_onCreate = new RelayCommand(t => SetNew())); }
            set => _onCreate = value;
        }

        public virtual bool CanPaste()
        {
            return true;
        }


        private RelayCommand _onPaste;
        public RelayCommand OnPaste
        {
            get { return _onPaste ?? (_onPaste = new RelayCommand(
                t => Paste(),
                t => CanPaste())); }
            set => _onPaste = value;
        }

        #endregion

        #region Properties

        public FeatureSubEditorDocViewModel ParentModel { get; set; }


        public IList<EnumViewModel> EnumList { get; set; }

        public void UpdateEnumValue()
        {
            EnumValue = Value == null ? null : EnumList.FirstOrDefault(t => t.Enum.ToString() == Value.ToString());
        }

        public EnumViewModel EnumValue
        {
            get => _enumValue;
            set
            {
                _enumValue = value;
                Value = EnumValue?.Enum;
                OnPropertyChanged("EnumValue");
            }
        }

        private bool _isChangedByChildren;
        public bool IsChangedByChildren
        {
            get => _isChangedByChildren;
            set
            {
                _isChangedByChildren = value;
                OnPropertyChanged("IsChanged");
                OnPropertyChanged("ChangedVisibility");
            }
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;

                if (IsReadOnly || IsParentReadOnly)
                {
                    PictureId = LockedPicture;
                }
            }
        }

        private bool _wasDelta;
        public bool WasDelta
        {
            get => _wasDelta;
            set
            {
                _wasDelta = value;
                IsDelta = value;
            }
        }

        public string OldStringValue
        {
            get
            {
                if (IsNilReasonChanged())
                {
                   
                        if (!IsDeltaChanged())
                        {
                            return "Nil Reason " + OldNilReason;
                        }

                        if (WasDelta)
                        {
                            return "Nil Reason " + OldNilReason + " (Add to Delta)";
                        }
                        return "Nil Reason " + OldNilReason + " (Remove from Delta)";
                   
                }
                else
                {
                    if (!IsDeltaChanged())
                    {
                        return OldValueAsString;
                    }

                    if (WasDelta)
                    {
                        return OldValueAsString + " (Add to Delta)";
                    }
                    return OldValueAsString + " (Remove from Delta)";
                }
            }
        }

        public string OldValueAsString { get; set; }


        private string _previousValueAsString;
        public string PreviousValueAsString
        {
            get => _previousValueAsString;
            set
            {
                _previousValueAsString = value;
                OnPropertyChanged("PreviousValueAsString");
            }
        }


        public Visibility DeltaVisibility => (IsDelta && !IsReadOnly) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DeltaInvisibility => (!IsDelta && !IsReadOnly) ? Visibility.Visible : Visibility.Collapsed;

        public override void NilReasonChanged()
        {
            OnPropertyChanged("OldStringValue");
            OnPropertyChanged("ChangedVisibility");
        }

        private bool _isDelta;
        public bool IsDelta
        {
            get => _isDelta;
            set
            {
                if (IsReadOnly) return;
                if (_isDelta==value) return;
                _isDelta = value;
                OnPropertyChanged("IsDelta");
                OnPropertyChanged("OldStringValue");
                OnPropertyChanged("ChangedVisibility");

                OnPropertyChanged("DeltaVisibility");
                OnPropertyChanged("DeltaInvisibility");
            }
        }

        public bool DeltaEnabled { get; set; } = true;

        private object _parentObject;
        private EnumViewModel _enumValue;

        public object ParentObject
        {
            get => _parentObject;
            set
            {
                _parentObject = value;
                GetValueFromParent();
            }
        }

        public bool IsParentReadOnly => ParentModel != null && ParentModel.IsReadOnly;

        #endregion

        #region Visibility

        public Visibility ChangedVisibility => IsChanged ? Visibility.Visible : Visibility.Collapsed;

        public Visibility NullReasonVisibility => IsNotNull ? Visibility.Collapsed : Visibility.Visible;

        public Visibility MainValueVisibility => IsNotNull ? Visibility.Visible : Visibility.Collapsed;

        public Visibility InteractionVisibility => IsReadOnly ? Visibility.Collapsed : Visibility.Visible;

        public Visibility LockedVisibility => IsReadOnly && !IsParentReadOnly ? Visibility.Visible : Visibility.Collapsed;

        #endregion
    }
}
