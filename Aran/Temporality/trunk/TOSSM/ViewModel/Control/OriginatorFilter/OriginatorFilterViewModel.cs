using System;
using MvvmCore;

namespace TOSSM.ViewModel.Control.OriginatorFilter
{
    public enum Orinination
    {
        AllData,
        New,
        Changed,
        Deleted,
        InSlot
    }

    public class OriginatorFilterViewModel : ViewModelBase
    {
        public Orinination SelectedOrigination { get; set; }

        public Action<OriginatorFilterViewModel> OnChanged { get; set; }

        private int _selectedOriginationIndex;
        public int SelectedOriginationIndex
        {
            get => _selectedOriginationIndex;
            set
            {
                _selectedOriginationIndex = value;
                SelectedOrigination = (Orinination) SelectedOriginationIndex;
                OnPropertyChanged("SelectedOriginationIndex");

                if (OnChanged!=null)
                {
                    OnChanged(this);
                }
            }
        }
    }
}
