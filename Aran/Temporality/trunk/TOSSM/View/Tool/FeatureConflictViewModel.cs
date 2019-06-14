using Aran.Aim;
using Aran.Temporality.Common.Entity;
using MvvmCore;
using TOSSM.Util.Wrapper;
using TOSSM.Converter;
using TOSSM.ViewModel;

namespace TOSSM.View.Tool
{
    public class FeatureConflictViewModel : ViewModelBase
    {
        public FeatureConflictViewModel(ReadonlyFeatureWrapper feature1, ReadonlyFeatureWrapper feature2, PrivateSlot slot1, PrivateSlot slot2)
        {
            Feature1 = feature1;
            Feature2 = feature2;
            Slot1 = slot1;
            Slot2 = slot2;
            FeatureType = feature1.Feature.FeatureType;
            Id = feature1.Feature.Feature.Identifier.ToString();
            TimeSlice1 = HumanReadableConverter.ToHuman(feature1.Feature.Feature.TimeSlice);
            TimeSlice2 = HumanReadableConverter.ToHuman(feature2.Feature.Feature.TimeSlice);
            Slot1Name = Slot1.Name;
            Slot2Name = Slot2.Name;

        }

        public ReadonlyFeatureWrapper Feature1 { get; }
        public ReadonlyFeatureWrapper Feature2 { get; }

        public PrivateSlot Slot1 { get; }
        public PrivateSlot Slot2 { get; }


        public string Id { get; private set; }
        public string TimeSlice1 { get; private set; }
        public string TimeSlice2 { get; private set; }

        private string _slot1Name;
        public string Slot1Name
        {
            get { return _slot1Name; }
            set
            {
                _slot1Name = value;
                OnPropertyChanged(nameof(Slot1Name));
            }
        }
        public string Slot2Name { get; }

        private bool _slot1Selected;
        public bool Slot1Selected
        {
            get => _slot1Selected;
            set
            {
                _slot1Selected = value;
                OnPropertyChanged("Slot1Selected");
                if (_slot1Selected)
                    Slot2Selected = false;
            }
        }

        private bool _slot2Selected;
        public bool Slot2Selected
        {
            get => _slot2Selected;
            set
            {
                _slot2Selected = value;
                OnPropertyChanged("Slot2Selected");
                if (_slot2Selected)
                    Slot1Selected = false;
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
            }
        }

        public ReadonlyFeatureWrapper SelectedFeature
        {
            get
            {
                if (Slot1Selected) return Feature1;
                if (Slot2Selected) return Feature2;
                return null;
            }
        }

        private RelayCommand _viewCommand;
        public RelayCommand ViewCommand
        {
            get => _viewCommand ?? (_viewCommand = new RelayCommand(
                   execute: t => ViewFeature(t),
                   canExecute: t => Feature1 != null));

            set => _viewCommand = value;
        }


        private void ViewFeature(object t)
        {
            MainManagerModel.Instance.View(t.Equals("1") ? Feature1 : Feature2, Slot1.PublicSlot.EffectiveDate, t.Equals("1") ? Slot1.Id : Slot2.Id);
        }
    }
}
