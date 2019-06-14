
using ESRI.ArcGIS.Framework;
using System;
using System.Threading.Tasks;
using AmdbManager.Helper;
using AerodromeManager.AmdbService;

namespace AmdbManager.ViewModel
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

        public IAmdbManagerService AmdbManagerService { get; internal set; }

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

        public async Task InitializeAsync(IApplication application)
        {
            await ((InitialViewModel) ViewModel).InitializeAsync(application);
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