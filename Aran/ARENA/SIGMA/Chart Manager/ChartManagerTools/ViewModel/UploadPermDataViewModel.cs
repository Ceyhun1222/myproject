using AiracUtil;
using ArenaStatic;
using ChartManager.ChartServices;
using ChartManager.Helper;
using ChartManager.Logging;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartManager.ViewModel
{
    public class UploadPermDataViewModel : ViewModelBase
    {
        private string _note;
        private RelayCommand _applyCommand;
        private RelayCommand _cancelCommand;
        private bool _isLoading;
        private List<AiracModel> _airacList;
        private AiracModel _selectedCycle;
        private IApplication mApplication;
        private ILogger _logger;
        private const int airacCycleCount = 20;

        public UploadPermDataViewModel()
        {
            _logger = LogManager.GetLogger(this);
        }

        public bool ApplyClicked { get; set; }

        public event Action Close;

        public AiracModel PublicationDate
        {
            get => _selectedCycle;
            set => Set(ref _selectedCycle, value);
        }

        public string Note
        {
            get => _note;
            set => Set(ref _note, value);
        }

        public List<AiracModel> AiracList
        {
            get
            {
                return _airacList ?? (_airacList = AiracUtil.AiracUtil.GetAiracList(airacCycleCount).ToList());
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public RelayCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    Result = new ChartUpdateData()
                    {
                        Note = Note,
                        EffectiveDate = PublicationDate.DateTime,
                    };
                    ApplyClicked = true;
                    Close?.Invoke();
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public ChartUpdateData Result { get; private set; }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() =>
                {
                    _logger.InfoWithMemberName($"{Close} is calling");
                    Close?.Invoke();
                }));
            }
        }
    }
}
