using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Document.Relations.Util
{
    public class SingleTypeRelationViewModel : ViewModelBase
    {
        public bool IsExpanded { get; set; } = true;

        public string Direction { get; set; }

        private RelationPurpose _purpose;
        public RelationPurpose Purpose
        {
            get => _purpose;
            set
            {
                _purpose = value;
                PurposeDescription = EnumHelper.GetDescription(Purpose);
            }
        }

        public TimeSliceInterpretationType TimeSliceInterpretationType
        {
            get => _timeSliceInterpretationType;
            set
            {
                _timeSliceInterpretationType = value;
                PurposeDescription = EnumHelper.GetDescription(TimeSliceInterpretationType);
            }
        }

        private string _purposeDescription;
        public string PurposeDescription
        {
            get => _purposeDescription;
            set
            {
                _purposeDescription = value;
                OnPropertyChanged("PurposeDescription");
            }
        }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        public Action<SingleTypeRelationViewModel> OnFeatureTypeChecked { get; set; }

        public bool IsFeatureTypeChecked
        {
            get => _isFeatureTypeChecked;
            set
            {
                _isFeatureTypeChecked = value;
                OnPropertyChanged("IsFeatureTypeChecked");
                OnFeatureTypeChecked?.Invoke(this);
            }
        }

        public string FeatureTypeDescription
        {
            get => _featureTypeDescription;
            set
            {
                _featureTypeDescription = value;
                OnPropertyChanged("FeatureTypeDescription");
            }
        }

        private FeatureType _featureType;
        public FeatureType FeatureType
        {
            get => _featureType;
            set
            {
                _featureType = value;
                OnPropertyChanged("FeatureType");
                FeatureTypeDescription = FeatureType.ToString();
            }
        }

        private MtObservableCollection<object> _items;
        private bool _isFeatureTypeChecked;
        private TimeSliceInterpretationType _timeSliceInterpretationType;
        private string _featureTypeDescription;

        public MtObservableCollection<object> Items
        {
            get => _items??(_items=new MtObservableCollection<object>());
            set => _items = value;
        }
    }
}
