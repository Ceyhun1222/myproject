using System;
using MvvmCore;

namespace TOSSM.ViewModel.Control.Interpretation
{
    public enum StateInterpretation
    {
        Snapshot,
        Baseline,
    }

    public class InterpretationViewModel : ViewModelBase
    {
        public StateInterpretation SelectedInterpretation { get; set; }

        public Action<InterpretationViewModel> OnChanged { get; set; }

        private int _selectedInterpretationIndex;
        public int SelectedInterpretationIndex
        {
            get => _selectedInterpretationIndex;
            set
            {
                _selectedInterpretationIndex = value;
                SelectedInterpretation = (StateInterpretation)_selectedInterpretationIndex;
                OnPropertyChanged("SelectedInterpretationIndex");

                if (OnChanged!=null)
                {
                    OnChanged(this);
                }
            }
        }
    }
}
