using ChartManager.ChartServices;
using ESRI.ArcGIS.Framework;
using System;
using System.Threading.Tasks;
using ChartManager.Helper;

namespace ChartManager.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private StateViewModel _stateViewModel;
        private string _title;

        public MainViewModel(INotifyService notifyService)
        {
            NotifyService = notifyService;
            ViewModel = new InitialViewModel(this);            
        }

        public IChartManagerService ChartManagerService { get; internal set; }

        public INotifyService NotifyService { get; }

        public StateViewModel ViewModel
        {
            get => _stateViewModel;
            set => Set(ref _stateViewModel, value);
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public EventHandler Close;

        public Task InitializeAsync(IApplication application, ChartUpdateData chartUpdateData = default(ChartUpdateData),bool getOnlyPendingUpdates = false)
        {
            var initialViewModel = ((InitialViewModel)ViewModel);
            return initialViewModel.InitializeAsync(application, chartUpdateData, getOnlyPendingUpdates);
        }

        public void Next(StateViewModel nextState)
        {

            ViewModel = nextState;
        }

        public void Previous(StateViewModel previousState)
        {
            ViewModel = previousState;
        }
    }
}