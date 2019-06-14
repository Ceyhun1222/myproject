using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PVT.UI.ViewModel
{
    public class AssessmentAreasViewModel: BaseViewModel
    {

        public AssessmentAreasViewModel(List<ObstacleAssessmentArea> areas)
        {
            Areas = new ObservableCollection<ObstacleAssessmentArea>();
            foreach (ObstacleAssessmentArea area in areas)
            {
                Areas.Add(area);
            }
        }

        public ObservableCollection<ObstacleAssessmentArea> Areas { get; }
        public event SelectionChangedEventHandler SelectionChanged;

        private ObstacleAssessmentArea _currentArea;

        public ObstacleAssessmentArea CurrentArea
        {
            get => _currentArea;
            set
            {
                Set(() => CurrentArea, ref _currentArea, value);
            }
        }

        private RelayCommand<SelectionChangedEventArgs> _legRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> LegRowSelectedCommand
        {
            get
            {
                return _legRowSelectedCommand
                       ?? (_legRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                           (e) =>
                           {
                               SelectionChanged?.Invoke(this, e);

                           }));
            }
        }
    }
}
