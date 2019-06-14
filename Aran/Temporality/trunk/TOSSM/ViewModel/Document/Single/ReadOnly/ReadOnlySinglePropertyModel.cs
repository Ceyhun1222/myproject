using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Document.Single.ReadOnly
{
    public class ReadOnlyValueToolTipViewModel : ViewModelBase
    {
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

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
    }

    public class ReadOnlySinglePropertyModel : SingleModelBase
    {
        #region Dummy functions (will be implemented in derrived classes)

        public virtual void GetValueFromParent()
        {
        }
        public virtual void SetValueToParent(bool fromUser=true)
        {
        }

        public virtual void Paste()
        {
            
        }

        public virtual void SetNew()
        {
        }

        public virtual void UpdateStringValue()
        {
        }

        #endregion

        public Visibility ReadOnlyValueToolTipVisibility { get; set; } = Visibility.Hidden;

        private ObservableCollection<ReadOnlyValueToolTipViewModel> _readOnlyValueToolTips;
        public ObservableCollection<ReadOnlyValueToolTipViewModel> ReadOnlyValueToolTips => _readOnlyValueToolTips??(_readOnlyValueToolTips=new MtObservableCollection<ReadOnlyValueToolTipViewModel>());

        private string _pasteToolTip;
        public string PasteToolTip
        {
            get => _pasteToolTip;
            set
            {
                _pasteToolTip = value;
                OnPropertyChanged("PasteToolTip");
            }
        }

        private FeatureType _referenceFeature;
        public FeatureType ReferenceFeature
        {
            get => _referenceFeature;
            set
            {
                _referenceFeature = value;
                OnReferenceFeatureChanged();
            }
        }

        public virtual void OnReferenceFeatureChanged()
        {
        }

        public bool IsReference { get; set; }

        public void ResetValue(object obj)
        {
            _value = obj;
        }

        private object _value;
        public virtual object Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                CheckIsNull();
                SetValueToParent();
                OnPropertyChanged("Value");
            }
        }


        public virtual void CheckIsNull()
        {
            if (Value is IList)
            {
                IsNotNull = ((IList) Value).Count > 0;
            }
            else
            {
                IsNotNull = (Value != null);
            }
        }

        #region Nil reason

        private static Dictionary<NilReason, string> _nilReasonToolTips;
        public static Dictionary<NilReason, string> NilReasonToolTips
        {
            get
            {
                if (_nilReasonToolTips == null)
                {
                    _nilReasonToolTips =
                        new Dictionary<NilReason, string>
                        {
                            [NilReason.inapplicable] = "There is no value.",
                            [NilReason.missing] =
                            "The correct value is not readily available to the sender of this data.",
                            [NilReason.template] = "The value will be available later.",
                            [NilReason.unknown] =
                            "The correct value is not known to, and not computable by, the sender of this data.",
                            [NilReason.withheld] = "The value is not divulged."
                        };
                }
                return _nilReasonToolTips;
            }
        }

        private static List<NilReason> _nilReasonList;
        public List<NilReason> NilReasonList
        {
            get
            {
                if (_nilReasonList == null)
                {
                    _nilReasonList = new List<NilReason>();
                    foreach (NilReason nl in Enum.GetValues(typeof(NilReason)))
                    {
                        _nilReasonList.Add(nl);
                    }
                }
                return _nilReasonList;
            }
            set => _nilReasonList = value;
        }

        public string NilReasonToolTip => NilReasonToolTips[NilReason];

        private NilReason _oldNilReason = NilReason.unknown;
        public NilReason OldNilReason
        {
            get => _oldNilReason;
            set
            {
                _oldNilReason = value;
                NilReason = OldNilReason;
            }
        }

        public virtual void NilReasonChanged()
        {
        }

        private NilReason _nilReason = NilReason.unknown;
        public NilReason NilReason
        {
            get => _nilReason;
            set
            {
                if (_nilReason==value) return;
                _nilReason = value;
                OnPropertyChanged("NilReason");
                OnPropertyChanged("NilReasonToolTip");
                NilReasonChanged();
            }
        }

        #endregion
    }
}
