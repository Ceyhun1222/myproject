using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChartManager.ChartServices;
using ChartManager.Helper;
using ChartManager.Logging;
using GalaSoft.MvvmLight.Command;

namespace ChartManager.ViewModel
{
    public class HistoryViewModel : StateViewModel
    {
        private ObservableCollection<Chart> _chartSource;
        private Chart _selectedChart;
        private byte[] _selectedPreview;
        private RelayCommand _backCommand;
        private Chart _rootChart;
        private bool _isLoading;
        private readonly ILogger _logger;

        public HistoryViewModel(MainViewModel mainViewModel, StateViewModel previousState, Chart chart):base(mainViewModel,previousState)
        {
            _rootChart = chart;
            ChartSource = new ObservableCollection<Chart>();
            _logger = LogManager.GetLogger(this);
            LoadCharts();            
        }
        
        #region Properties

        private IChartManagerService ChartManagerService => MainViewModel.ChartManagerService;
        private INotifyService NotifyService => MainViewModel.NotifyService;

        public byte[] SelectedPreview
        {
            get => _selectedPreview;
            set => Set(ref _selectedPreview, value);
        }

        public Chart RootChart
        {
            get => _rootChart;
            set => Set(ref _rootChart, value);
        }

        public Chart SelectedChart
        {
            get => _selectedChart;
            set
            {
                Set(ref _selectedChart, value);
                if (_selectedChart != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        _logger.InfoWithMemberName($"Started");
                        try
                        {
                            _logger.InfoWithMemberName($"GetPreviewOf({_selectedChart.Id}) is calling");
                            var res = ChartManagerService.GetPreviewOf(_selectedChart.Id);
                            _logger.InfoWithMemberName($"Size is {res.Length / 1024} is KB");
                            SelectedPreview = res;
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Handle(this, ex, NotifyService,
                                $"SelectedChart property. GetPreviewOf({_selectedChart.Id})"
                            );
                        }
                        _logger.InfoWithMemberName($"Finished");
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

        public ObservableCollection<Chart> ChartSource
        {
            get => _chartSource;
            set => Set(ref _chartSource, value);
        }

        #endregion

        #region Commands

        public RelayCommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand(Previous));

        #endregion


        #region Methods

        private async Task LoadCharts()
        {
            IsLoading = true;
            var list = await ChartManagerService.GetHistoryOfAsync(_rootChart.Identifier);
            list.ForEach(t => ChartSource.Add(t));
            IsLoading = false;
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