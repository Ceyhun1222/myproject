using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AerodromeManager.AmdbService;
using AmdbManager.Helper;
using AmdbManager.Logging;
using GalaSoft.MvvmLight.Command;

namespace AmdbManager.ViewModel
{
    public class HistoryViewModel : StateViewModel
    {
        private ObservableCollection<AmdbMetadata> _amdbSource;
        private AmdbMetadata _selectedChart;
        private byte[] _selectedPreview;
        private RelayCommand _backCommand;
        private AmdbMetadata _rootChart;
        private bool _isLoading;

        public HistoryViewModel(MainViewModel mainViewModel, StateViewModel previousState, AmdbMetadata chart):base(mainViewModel,previousState)
        {
            _rootChart = chart;
            AmdbSource = new ObservableCollection<AmdbMetadata>();
            LoadAmdbFiles();
            
        }
        
        #region Properties

        private IAmdbManagerService ChartManagerService => MainViewModel.AmdbManagerService;
        private INotifyService NotifyService => MainViewModel.NotifyService;

        public byte[] SelectedPreview
        {
            get => _selectedPreview;
            set => Set(ref _selectedPreview, value);
        }

        public AmdbMetadata RootChart
        {
            get => _rootChart;
            set => Set(ref _rootChart, value);
        }

        public AmdbMetadata SelectedAmdb
        {
            get => _selectedChart;
            set
            {
                Set(ref _selectedChart, value);
                if (_selectedChart != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        LogManager.GetLogger(this).InfoWithMemberName($"Started");
                        try
                        {
                            LogManager.GetLogger(this).InfoWithMemberName($"GetPreviewOf({_selectedChart.Id}) is calling");
                            var res = ChartManagerService.GetPreviewOf(_selectedChart.Id);
                            LogManager.GetLogger(this).InfoWithMemberName($"Size is {res.Length / 1024} is KB");
                            SelectedPreview = res;
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Handle(this, ex, NotifyService,
                                $"SelectedChart property. GetPreviewOf({_selectedChart.Id})"
                            );
                        }
                        LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                    }
                    );
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public ObservableCollection<AmdbMetadata> AmdbSource
        {
            get => _amdbSource;
            set => Set(ref _amdbSource, value);
        }

        #endregion

        #region Commands

        public RelayCommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand(Previous));

        #endregion


        #region Methods

        private async Task LoadAmdbFiles()
        {
            //IsLoading = true;
            //var list = await ChartManagerService.GetHistoryOfAsync(_rootChart.Identifier);
            //list.ForEach(t => ChartSource.Add(t));
            //IsLoading = false;
        }

        #endregion


        #region StateViewModel override methods
        public override bool CanNext() => false;

        public override bool CanPrevious() => true;

        protected override void _destroy()
        {

        }

        protected override void SetNext()
        {

        }

        protected override void SetPrevious()
        {

        }

        #endregion
    }
}