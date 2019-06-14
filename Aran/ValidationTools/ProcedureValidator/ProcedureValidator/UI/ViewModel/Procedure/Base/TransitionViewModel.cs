using GalaSoft.MvvmLight;
using PVT.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PVT.UI.ViewModel
{
    public class TransitionViewModel: BaseViewModel
    {
        public Transition Transition { get; set; }
        public ObservableCollection<LegsViewModel> LegsViewModels { get; }
        public event SelectionChangedEventHandler SelectionChanged;

        private TransitionLeg _currentLeg;

        public TransitionLeg CurrentLeg
        {
            get => _currentLeg;
            set
            {
                Set(() => CurrentLeg, ref _currentLeg, value);
            }
        }



        public TransitionViewModel(Transition transition)
        {
            LegsViewModels = new ObservableCollection<LegsViewModel>();
            Transition = transition;
            if (Transition.FinalLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Final, Transition.FinalLegs));
            if (Transition.IntermediateLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Intermediate, Transition.IntermediateLegs));
            if (Transition.InitialLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Initial, Transition.InitialLegs));
            if (Transition.MissedApproachLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Missed, Transition.MissedApproachLegs));
            if (Transition.DepartureLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Departure, Transition.DepartureLegs));
            if (Transition.ArrivalLegs.Count > 0)
                LegsViewModels.Add(new LegsViewModel(LegType.Arrival, Transition.ArrivalLegs));

            foreach (LegsViewModel legsViewModel in LegsViewModels)
            {
                legsViewModel.SelectionChanged += LegSelectionChanged;
            }

        }

        private void LegSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }
    }
}
