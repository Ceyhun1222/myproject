using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Aran.Temporality.Common.TossConverter;
using Aran.Temporality.CommonUtil.Context;
using MvvmCore;
using TossConverter.Model;

namespace TossConverter.ViewModel
{
    internal enum TossConverterOperation
    {
        Convert,
        ConvertWithGeoIndex,
        CleanIncorrectGeometry
    }
    internal class TossConverterViewModel : ViewModelBase
    {
        #region Private fields

        private Task _task;
        private CancellationTokenSource _tokenSource;
        private bool _hasLogToFile;
        private string _logFile;
        private readonly NoDeleteMongoComparison _comparison;
        private readonly NoDeleteMongoConverter _converter;
        private readonly MongoGeometryCleaner _mongoGeometryCleaner;

        #endregion

        #region Constructors

        public TossConverterViewModel()
        {
            Filtered = CollectionViewSource.GetDefaultView(Logs);

            _comparison = new NoDeleteMongoComparison { Logger = Logger };
            _converter = new NoDeleteMongoConverter { Logger = Logger };
            _mongoGeometryCleaner = new MongoGeometryCleaner { Logger = Logger };

            ConnectionProvider.InitServerSettings();
        }

        #endregion

        #region Observal propertire

        public List<TossConverterOperation> AllOperations { get; } = Enum.GetValues(typeof(TossConverterOperation)).Cast<TossConverterOperation>().ToList();

        private TossConverterOperation _operation = TossConverterOperation.Convert;
        public TossConverterOperation Operation
        {
            get => _operation;
            set
            {
                _operation = value;
                if (value == TossConverterOperation.Convert)
                {
                    AllFilters = new List<string>
                    {
                        "All logs",
                        MessageCauseType.SystemMessage.ToString(),
                        MessageCauseType.PolygonOpened.ToString(),
                        MessageCauseType.PolygonReverse.ToString(),
                        MessageCauseType.PolygonShort.ToString(),
                        MessageCauseType.LineHasOneUniqueVertices.ToString(),
                        MessageCauseType.RingHasOneUniqueVertices.ToString(),
                        MessageCauseType.CommonError.ToString(),
                        MessageCauseType.UnknownError.ToString()
                    };
                }
                else if (value == TossConverterOperation.CleanIncorrectGeometry)
                {
                    AllFilters = new List<string>
                    {
                        "All logs",
                        MessageCauseType.SystemMessage.ToString(),
                        MessageCauseType.DuplicateVertices.ToString(),
                        MessageCauseType.SelfIntersect.ToString(),
                        MessageCauseType.IncorrectLatLon.ToString(),
                        MessageCauseType.Less3UniqueVertices.ToString(),
                        MessageCauseType.UnknownGeometryError.ToString(),
                        MessageCauseType.CommonError.ToString(),
                        MessageCauseType.UnknownError.ToString()
                    };
                }
                else
                {
                    AllFilters = new List<string>
                    {
                        "All logs",
                        MessageCauseType.SystemMessage.ToString(),
                        MessageCauseType.PolygonOpened.ToString(),
                        MessageCauseType.PolygonReverse.ToString(),
                        MessageCauseType.PolygonShort.ToString(),
                        MessageCauseType.LineHasOneUniqueVertices.ToString(),
                        MessageCauseType.RingHasOneUniqueVertices.ToString(),
                        MessageCauseType.DuplicateVertices.ToString(),
                        MessageCauseType.SelfIntersect.ToString(),
                        MessageCauseType.IncorrectLatLon.ToString(),
                        MessageCauseType.LineHasOneUniqueVertices.ToString(),
                        MessageCauseType.Less3UniqueVertices.ToString(),
                        MessageCauseType.UnknownGeometryError.ToString(),
                        MessageCauseType.CommonError.ToString(),
                        MessageCauseType.UnknownError.ToString()
                    };
                }
            }
        }

        private List<string> _allFilters = new List<string>
        {
            "All logs",
            MessageCauseType.SystemMessage.ToString(),
            MessageCauseType.PolygonOpened.ToString(),
            MessageCauseType.PolygonReverse.ToString(),
            MessageCauseType.PolygonShort.ToString(),
            MessageCauseType.LineHasOneUniqueVertices.ToString(),
            MessageCauseType.RingHasOneUniqueVertices.ToString(),
            MessageCauseType.CommonError.ToString(),
            MessageCauseType.UnknownError.ToString()
        };

        public ObservableCollection<LogInfo> Logs { get; } = new ObservableCollection<LogInfo>();

        public ICollectionView Filtered { get; }

        private bool _detailButtonEnabled;
        public bool DetailButtonEnabled
        {
            get => _detailButtonEnabled;
            set
            {
                _detailButtonEnabled = value;
                OnPropertyChanged("DetailButtonEnabled");
            }
        }

        private LogInfo _selectedLogInfo;
        public LogInfo SelectedLog
        {
            get => _selectedLogInfo;
            set
            {
                _selectedLogInfo = value;
                DetailButtonEnabled = !string.IsNullOrWhiteSpace(value?.Description);
            }
        }
        public List<string> AllFilters
        {
            get => _allFilters;
            set
            {
                _allFilters = value;
                OnPropertyChanged("AllFilters");
            }
        }

        public string Filter
        {
            set
            {
                if (Enum.TryParse(value, out MessageCauseType result))
                    Filtered.Filter = o => ((LogInfo)o).Type == result;
                else
                    Filtered.Filter = null;
            }
        }

        public string FileRepository { get; set; } = "test";

        public string MongoRepository { get; set; } = "test";

        public string LogPath { get; set; } = "C:/TOSS/logs/converter/";

        private string _startButtonText = "Start";
        public string StartButtonText
        {
            get => _startButtonText;
            set
            {
                _startButtonText = value;
                OnPropertyChanged("StartButtonText");
            }
        }

        public bool _controlEnabled = true;
        public bool ControlEnabled
        {
            get => _controlEnabled;
            set
            {
                _controlEnabled = value;
                OnPropertyChanged("ControlEnabled");
            }
        }

        private string _statusBarText = "Stoped";
        public string StatusBarText
        {
            get => _statusBarText;
            set
            {
                _statusBarText = value;
                OnPropertyChanged("StatusBarText");
            }
        }

        private bool _progressBarStatus;
        public bool ProgressBarStatus
        {
            get => _progressBarStatus;
            set
            {
                _progressBarStatus = value;
                OnPropertyChanged("ProgressBarStatus");
            }
        }

        #endregion

        #region Commands

        private RelayCommand _startCommand;
        public RelayCommand StartCommand => _startCommand ?? (_startCommand = new RelayCommand(_ =>
        {
            if (_task?.Status != TaskStatus.Running)
                Start();
            else
                Stop();
        }));

        private RelayCommand _detailCommand;

        public RelayCommand DetailCommand => _detailCommand ?? (_detailCommand = new RelayCommand(_ =>
        {
            if (!string.IsNullOrWhiteSpace(SelectedLog.Description))
            {
                var featureDetailsWindow = new FeatureDetails(SelectedLog);
                featureDetailsWindow.Show();
            }
        }));

        #endregion

        #region Methods

        private void ChangeControlState(bool controlEnabled)
        {
            if (controlEnabled)
            {
                ControlEnabled = true;
                StartButtonText = "Start";
                ProgressBarStatus = false;
            }
            else
            {
                ControlEnabled = false;
                StartButtonText = "Stop";
                ProgressBarStatus = true;
            }
        }

        private void Logger(MessageCauseType type, string title, string description)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Logs.Add(new LogInfo(type, title, description));
                    if (type == MessageCauseType.SystemMessage)
                        StatusBarText = title;
                }, System.Windows.Threading.DispatcherPriority.Normal, CancellationToken.None, TimeSpan.FromSeconds(1));

                if (_hasLogToFile && type != MessageCauseType.SystemMessage)
                    File.AppendAllText(_logFile + type + ".log", title + "\n");
            }
            catch (Exception e)
            {

            }
        }

        private void CheckLogPath()
        {
            _logFile = LogPath.Trim();
            _hasLogToFile = !string.IsNullOrWhiteSpace(_logFile);

            if (_hasLogToFile)
            {
                var logName = "";
                if (Operation == TossConverterOperation.CleanIncorrectGeometry)
                    logName = MongoRepository;
                else
                    logName = $"{FileRepository}_to_{MongoRepository}";

                _logFile = $"{_logFile}/{Operation}/{DateTime.Now.ToString("yy.MM.dd_H.mm.ss")}_{logName}/";

                try
                {
                    Directory.CreateDirectory(_logFile);
                }
                catch
                {
                    _hasLogToFile = false;
                }
            }

            Logs.Clear();
        }

        private void Start()
        {
            CheckLogPath();

            _tokenSource = new CancellationTokenSource();

            if (Operation == TossConverterOperation.Convert || Operation == TossConverterOperation.ConvertWithGeoIndex)
            {
                if (!_converter.IsExist(MongoRepository) || MessageBoxResult.OK ==
                    MessageBox.Show($"Database {MongoRepository} already exists. Clear?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                {
                    ChangeControlState(false);
                    StatusBarText = "Started...";
                    var withComparison = Operation == TossConverterOperation.ConvertWithGeoIndex;

                    _task = Task.Factory.StartNew(() =>
                        {
                            _converter.Convert(FileRepository, MongoRepository, withComparison, _tokenSource);

                            ChangeControlState(true);
                            StatusBarText = "Task completed";
                        },
                        _tokenSource.Token);
                }
            }
            else
            {
                if (Operation == TossConverterOperation.CleanIncorrectGeometry && MessageBoxResult.OK != MessageBox.Show($"Clear incorrect geometry?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                {
                    return;
                }
                ChangeControlState(false);
                StatusBarText = "Started...";
                _task = Task.Factory.StartNew(() =>
                    {
                        _mongoGeometryCleaner.Clean(MongoRepository, _tokenSource);

                        ChangeControlState(true);
                        StatusBarText = "Task completed";
                    },
                    _tokenSource.Token);
            }
        }

        private void Stop()
        {
            var result = MessageBox.Show("Are you sure?", "Stop proccess", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK);

            if (result == MessageBoxResult.Cancel)
                return;

            _tokenSource?.Cancel();

            try
            {
                _task?.Wait();
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                _tokenSource?.Dispose();
            }

            Logs.Add(new LogInfo(MessageCauseType.SystemMessage, "Stopped by user"));
            StatusBarText = "Stoped by user";
        }

        #endregion
    }
}
