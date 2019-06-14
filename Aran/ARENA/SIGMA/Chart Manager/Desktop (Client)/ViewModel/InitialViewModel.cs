using ChartManager.ChartServices;
using ChartManager.Helper;
using ChartManager.Logging;
using ESRI.ArcGIS.Framework;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using ArenaStatic;
using ARENA;
using ARENA.Enums_Const;
using ChartManager.Properties;
using DataModule;
using EnrouteChartCompare.View;
using EnrouteChartCompare.ViewModel;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Controls;
using PDM;
using ChartUser = ChartManager.ChartServices.ChartUser;
using ChartManager.View;
using ESRI.ArcGIS.Carto;
using SigmaChart;

namespace ChartManager.ViewModel
{
    public class InitialViewModel : StateViewModel, IChartManagerServiceCallback
    {
        private readonly string _allStr = "All";
        private DateTime _createdFrom;
        private DateTime _createdTo;
        private DateTime _effectiveFrom, _effectiveTo;
        private ObservableCollection<ChartType> _chartTypeSource;
        private ChartType _selectedChartType;
        private ObservableCollection<FilterItem> _statusSource;
        private FilterItem _selectedStatus;
        private ObservableCollection<ChartUser> _users;
        private ChartUser _selectedUser;
        private ObservableCollection<string> _organizationSource;
        private string _selectedOrganization;
        private ObservableCollection<string> _aerodromeSource;
        private string _selectedAerodrome;
        private ObservableCollection<string> _rwyDirSource;
        private string _selectedRwyDir;
        private ObservableCollection<Chart> _chartSource;
        private Chart _selectedChart;
        private bool _isAerodromeEnabled;
        private bool _isRwyDirEnabled;
        private byte[] _selectedPreview;
        private IApplication _app;
        private RelayCommand _openLockCommand;
        private RelayCommand _openCommand;
        private RelayCommand _openAsNewChart;
        private RelayCommand _deleteAllCommand;
        private RelayCommand _deleteLatestVersion;
        private RelayCommand _deleteAllCurrentVersions;
        private RelayCommand _unlockCommand;
        private RelayCommand _exportCommand;
        private bool _isLoading;
        private bool _hasSelectedChart;
        private bool _isUpdateEnabled;
        private ChartUser _currentUser;
        private bool _isUnlockEnabled;
        private RelayCommand _historyCommand;
        private RelayCommand _updateCommand;
        private Dictionary<long, byte[]> _previews;
        private ILogger _logger;
        private RelayCommand _ignoreUpdateCommand;
        private ChartUpdateData _chartUpdateData;
        private bool _showOnlyPendingUpdates;
        private bool _isIgnoreUpdateEnabled;

        public InitialViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _previews = new Dictionary<long, byte[]>();
            _logger = LogManager.GetLogger(this);
        }

        #region Properties

        private IChartManagerService ChartManagerService => MainViewModel.ChartManagerService;
        private INotifyService NotifyService => MainViewModel.NotifyService;

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public bool IsRwyEnabled
        {
            get => _isRwyDirEnabled;
            set => Set(ref _isRwyDirEnabled, value);
        }

        public bool ShowOnlyPendingUpdates
        {
            get => _showOnlyPendingUpdates;
            set
            {
                _showOnlyPendingUpdates = value;

            }
        }

        public bool IsUpdateEnabled
        {
            get => _isUpdateEnabled;
            set => Set(ref _isUpdateEnabled, value);
        }

        public bool IsAerodromeEnabled
        {
            get => _isAerodromeEnabled;
            set => Set(ref _isAerodromeEnabled, value);
        }

        public bool IsUnlockEnabled
        {
            get => _isUnlockEnabled;  
            set => Set(ref _isUnlockEnabled, value);
        }

        public bool IsIgnoreUpdateEnabled
        {
            get => _isIgnoreUpdateEnabled;
            set => Set(ref _isIgnoreUpdateEnabled, value);
        }

        public bool HasSelectedChart
        {
            get => _hasSelectedChart;
            set => Set(ref _hasSelectedChart, value);
        }

        public Chart SelectedChart
        {
            get => _selectedChart;
            set
            {                
                Set(ref _selectedChart, value);
                IsLoading = true;
                HasSelectedChart = _selectedChart != null;
                IsUpdateEnabled = false;
                if (_selectedChart?.Type == ChartType.Enroute)
                    IsUpdateEnabled = !_selectedChart.IsLocked;
                if (_selectedChart != null)
                {
                        IsUnlockEnabled = false;
                        if (_selectedChart.IsLocked)
                        {
                            if (_selectedChart.LockedBy.Id == _currentUser.Id || _currentUser.IsAdmin)
                                IsUnlockEnabled = true;
                        }
                    Task.Factory.StartNew(() =>
                        {
                            _logger.InfoWithMemberName($"Started");
                            if (!_previews.ContainsKey(_selectedChart.Id))
                            {
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
                            }
                            else
                                SelectedPreview = _previews[_selectedChart.Id];
                            

                            _logger.InfoWithMemberName($"Finished");
                        }
                    ).ContinueWith( t => IsLoading = false);
                }
                IsLoading = false;
            }
        }


        public byte[] SelectedPreview
        {
            get => _selectedPreview;
            set
            {
                Set(ref _selectedPreview, value);
                if(_selectedChart != null)
                {
                    if (!_previews.ContainsKey(_selectedChart.Id))
                        _previews.Add(_selectedChart.Id, value);
                }
            }
        }

        public ObservableCollection<Chart> ChartSource
        {
            get => _chartSource;
            set => Set(ref _chartSource, value);
        }

        public string SelectedRwyDir
        {
            get => _selectedRwyDir;
            set
            {
                Set(ref _selectedRwyDir, value);
                PopulateChartSource();
            }
        }

        public ObservableCollection<string> RwyDirSource
        {
            get => _rwyDirSource;
            set => Set(ref _rwyDirSource, value);
        }

        public string SelectedAerodrome
        {
            get => _selectedAerodrome;
            set
            {
                Set(ref _selectedAerodrome, value);
                PopulateChartSource();
            }
        }

        public ObservableCollection<string> AerodromeSource
        {
            get => _aerodromeSource;
            set => Set(ref _aerodromeSource, value);
        }

        public string SelectedOrganization
        {
            get => _selectedOrganization;
            set
            {
                Set(ref _selectedOrganization, value);
                PopulateChartSource();
            }
        }

        public ObservableCollection<string> OrganizationSource
        {
            get => _organizationSource;
            set => Set(ref _organizationSource, value);
        }

        public ChartUser SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(ref _selectedUser, value);
                PopulateChartSource();
            }
        }

        public ObservableCollection<ChartUser> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        public DateTime CreatedFrom
        {
            get => _createdFrom;
            set
            {
                Set(ref _createdFrom, value);
                PopulateChartSource();
            }
        }

        public DateTime CreatedTo
        {
            get => _createdTo;
            set
            {
                Set(ref _createdTo, value);
                PopulateChartSource();
            }
        }

        public DateTime EffectiveFrom
        {
            get => _effectiveFrom;
            set
            {
                Set(ref _effectiveFrom, value);
                PopulateChartSource();
            }
        }

        public DateTime EffectiveTo
        {
            get => _effectiveTo;
            set
            {
                Set(ref _effectiveTo, value);
                PopulateChartSource();
            }
        }

        public ObservableCollection<ChartType> ChartTypeSource
        {
            get => _chartTypeSource;
            set => Set(ref _chartTypeSource, value);
        }

        public ObservableCollection<FilterItem> StatusSource
        {
            get => _statusSource;
            set => Set(ref _statusSource, value);
        }

        public ChartType SelectedChartType
        {
            get => _selectedChartType;
            set
            {
                Set(ref _selectedChartType, value);
                IsAerodromeEnabled = _selectedChartType != ChartType.Enroute;
                IsRwyEnabled = (_selectedChartType != ChartType.Enroute && _selectedChartType != ChartType.Aerodrome);
                PopulateChartSource();
            }
        }

        public FilterItem SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                Set(ref _selectedStatus, value);
                PopulateChartSource();
            }
        }
       
        #endregion

        #region Commands

        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new RelayCommand(
                           async () =>
                           {
                               _logger.InfoWithMemberName($"Started");                               
                               var fileName = await DownloadFile(false);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName, isReadOnly: true);
                               IsLoading = false;
                               _logger.InfoWithMemberName($"Finished");
                           }));
            }
        }

        public RelayCommand OpenLockCommand
        {
            get
            {
                return _openLockCommand ?? (_openLockCommand = new RelayCommand(
                           async () =>
                           {
                               _logger.InfoWithMemberName($"Started");
                               IsLoading = true;
                               var fileName = await DownloadFile(true);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName);
                               IsLoading = false;
                               _logger.InfoWithMemberName($"Finished");
                           }));
            }
        }

        public RelayCommand OpenAsNewChartCommand
        {
            get
            {
                return _openAsNewChart ?? (_openAsNewChart = new RelayCommand(
                           async () =>
                           {
                               _logger.InfoWithMemberName($"Started");
                               //IsLoading = true;
                               var fileName = await DownloadFile(false);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName, isNew: true);
                               IsLoading = false;
                               _logger.InfoWithMemberName($"Finished");
                           }));
            }
        }

        public RelayCommand DeleteAllCommand
        {
            get
            {
                return _deleteAllCommand ?? (_deleteAllCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        _logger.InfoWithMemberName($"DeleteAllChartVersionsAsync({SelectedChart.Identifier}) is calling");

                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteAllChartVersionsAsync(SelectedChart.Identifier);

                            var removedCharts = ChartSource.Where(t => t.Identifier == SelectedChart.Identifier).ToList();
                            foreach (var chart in removedCharts)
                            {
                                ChartSource.Remove(chart);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteAllChartVersions({SelectedChart.Identifier})");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand DeleteLatestVersion
        {
            get
            {
                return _deleteLatestVersion ?? (_deleteLatestVersion = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        _logger.InfoWithMemberName($"DeleteChartByIdAsync({SelectedChart.Id}) is calling");
                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteChartByIdAsync(SelectedChart.Id);

                            _logger.InfoWithMemberName($"GetLatestChartVersionAsync({SelectedChart.Identifier},{SelectedChart.BeginEffectiveDate}) is calling");
                            var latestVersion = await ChartManagerService.GetLatestChartVersionAsync(
                                SelectedChart.Identifier,
                                SelectedChart.BeginEffectiveDate);
                            ChartSource.Remove(SelectedChart);
                            if (latestVersion != null)
                                ChartSource.Add(latestVersion);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteChartByIdAsync({SelectedChart.Id}");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand DeleteAllCurrentVersions
        {
            get
            {
                return _deleteAllCurrentVersions ?? (_deleteAllCurrentVersions = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        _logger.InfoWithMemberName(
                                $"DeleteChartByEffectiveDateAsync({SelectedChart.Identifier},{SelectedChart.BeginEffectiveDate}) is calling");
                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteChartByEffectiveDateAsync(SelectedChart.Identifier,
                                SelectedChart.BeginEffectiveDate);
                            ChartSource.Remove(SelectedChart);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteChartByEffectiveDate({SelectedChart.Identifier})");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand UnlockCommand
        {
            get
            {
                return _unlockCommand ?? (_unlockCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        _logger.InfoWithMemberName($"LockChartAsync({SelectedChart.Id},false) is calling");
                        await ChartManagerService.LockChartAsync(SelectedChart.Id, false);
                        SelectedChart.IsLocked = false;
                        SelectedChart.LockedBy = null;
                        IsUnlockEnabled = false;
                        if (SelectedChart?.Type == ChartType.Enroute)
                            IsUpdateEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService);
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand ExportCommand
        {
            get
            {
                return _exportCommand ?? (_exportCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    var dialog = new SaveFileDialog()
                    {
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        Filter = @"JPEG Files|*.jpeg",
                        DefaultExt = ".jpeg",
                        Title = @"Save As...",
                    };

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;
                    IsLoading = true;
                    byte[] source = null;
                    try
                    {
                        _logger.InfoWithMemberName($"GetPreviewOfAsync({SelectedChart.Id}) is calling");
                        source = await ChartManagerService.GetPreviewOfAsync(SelectedChart.Id);
                        IsLoading = false;
                    }
                    catch (Exception ex)
                    {
                        IsLoading = false;
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"GetPreviewOfAsync({SelectedChart.Id})");
                        return;
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Source is {source.Length / 1024} KB");
                    Directory.CreateDirectory(Path.GetDirectoryName(dialog.FileName));
                    File.Create(dialog.FileName).Close();
                    _logger.InfoWithMemberName($"Writing into ({dialog.FileName})");
                    File.WriteAllBytes(dialog.FileName,
                        source);
                    IsLoading = false;
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand HistoryCommand
        {
            get { return _historyCommand ?? (_historyCommand = new RelayCommand(() => 
            {
                _logger.InfoWithMemberName($"Started");
                Next();
                _logger.InfoWithMemberName($"Finished");
            })); }
        }

        //public RelayCommand UpdateCommand {
        //    get
        //    {
        //        return _updateCommand?? (_updateCommand = new RelayCommand(async () =>
        //                   {
        //                       LogManager.GetLogger(this).InfoWithMemberName("Started");
        //                       //IsLoading = true;
        //                       //System.IO.DirectoryInfo inf = System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\" + DateTime.Now.Ticks.ToString());
        //                       var fileName = await DownloadFile(true);
        //                       if (!string.IsNullOrEmpty(fileName))
        //                       {
        //                           ArenaProjectType prjType = ArenaProjectType.ARENA;
        //                           var folderName = Path.GetDirectoryName(fileName);
        //                           var serverPdmList = ArenaDataModule.GetObjectsFromPdmFile(folderName, ref prjType);
        //                           serverPdmList.RemoveAll(x =>
        //                               (x is Airspace) && (((Airspace) x).CodeType == AirspaceType.OTHER));

        //                           var currentPdmList = DataCash.ProjectEnvironment.Data.PdmObjectList;
        //                           currentPdmList.RemoveAll(x =>
        //                               (x is Airspace) && (((Airspace) x).CodeType == AirspaceType.OTHER));

        //                           var hookHelper = new HookHelperClass
        //                           {
        //                               Hook = _app
        //                           };
        //                           if (hookHelper.ActiveView == null)
        //                               hookHelper = null;

        //                           var ci = EsriUtils.GetChartIno(folderName);
        //                           if (!Enum.TryParse<CODE_ROUTE_SEGMENT_CODE_LVL>(ci.RouteLevel, out var routeLevel))
        //                               routeLevel = CODE_ROUTE_SEGMENT_CODE_LVL.OTHER;
        //                           var viewModel = new LogChartViewModel(serverPdmList, currentPdmList, fileName,
        //                               hookHelper, routeLevel);
        //                           var logWindow = new LogChartView() {DataContext = viewModel};
        //                           viewModel.Close += () =>
        //                           {
        //                               logWindow.Close();
        //                               if (viewModel.DoUpdate)
        //                               {
        //                                   MainViewModel.Close?.Invoke(null, null);
        //                               }
        //                           };
        //                           var parentHandle = new IntPtr(_app.hWnd);
        //                           var helper = new WindowInteropHelper(logWindow) {Owner = parentHandle};
        //                           logWindow.ShowInTaskbar = false;
        //                           // hide from taskbar and alt-tab list
        //                           logWindow.Show();
        //                       }
        //                       IsLoading = false;
        //                       LogManager.GetLogger(this).InfoWithMemberName("Finished");
        //                   }));
        //    }
        //}

        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName("Started");
                    IsLoading = true;

                    try
                    {
                        if (!ShowOnlyPendingUpdates)
                        {
                            var foldername = ArenaStatic.ArenaStaticProc.GetTargetDBFolder();
                            _logger.InfoWithMemberName($"GetZippedChartPath({foldername}) is calling");
                            var zippedFolder = ArenaStaticProc.GetZippedChartPath(foldername);
                            _logger.InfoWithMemberName($"Zipped folder is {zippedFolder}");
                            var source = File.ReadAllBytes(zippedFolder);
                            _logger.InfoWithMemberName($"Zipped folder is {zippedFolder}");
                            //List<string> referenceIdList = ChartsHelperClass.Read_CEFID(foldername);

                            _logger.InfoWithMemberName($"UploadUpdateDateAsync(Note : {_chartUpdateData.Note}; EffectiveDate : {_chartUpdateData.EffectiveDate}; Source (MB) : {source.Length / 1024}) is calling");
                            var resultId = (int)await ChartManagerService.UploadUpdateDataAsync(_chartUpdateData, File.ReadAllBytes(zippedFolder), ChartSource.Select(s => s.Identifier).ToList());

                            var fileName = await DownloadFile(true);
                            if (!string.IsNullOrEmpty(fileName))
                                await OpenFile(fileName);
                            EsriExtensionData.UpdateId = resultId;
                        }
                        else
                        {
                            var fileName = await DownloadFile(true);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                var dirName = Path.GetDirectoryName(fileName);
                                var sourceOfUpdate = await ChartManagerService.GetUpdateSourceAsync(default(long));
                                string zipFileName = Path.Combine(dirName, "update.zip");
                                _logger.InfoWithMemberName($"Creating file ({zipFileName})");
                                File.Create(zipFileName).Close();
                                _logger.InfoWithMemberName($"Writing into file");
                                File.WriteAllBytes(zipFileName, sourceOfUpdate);
                                _logger.InfoWithMemberName($"Decompressing file");
                                var updateDirName = Path.Combine(dirName, "Update");
                                ArenaStaticProc.DecompressToDirectory(zipFileName, updateDirName);
                                File.Delete(zipFileName);

                                ArenaProjectOpener.OpenDecompressedFolder(SelectedChart.Name, _app, updateDirName);
                                MainViewModel.Close?.Invoke(null, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService);
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName("Finished");
                }));
            }
        }

        public RelayCommand IgnoreUpdateCommand
        {
            get
            {
                return _ignoreUpdateCommand ?? (_ignoreUpdateCommand = new RelayCommand(async () =>
                {
                    _logger.InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        _logger.InfoWithMemberName(
                                $"IgnoreUpdateCommand({SelectedChart.Identifier}) is calling");
                        if (NotifyService.ShowConfirmationMessage("Are you sure to ignore update for selected chart") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            ChartSource.Remove(SelectedChart);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"IgnoreUpdateCommand({SelectedChart.Identifier})");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    _logger.InfoWithMemberName($"Finished");
                }));
            }
        }

        #endregion

        public async Task InitializeAsync(IApplication application, ChartUpdateData chartUpdateData = default(ChartUpdateData), bool getOnlyPendingUpdates = false )
        {
            _chartUpdateData = chartUpdateData;
            ShowOnlyPendingUpdates = getOnlyPendingUpdates;
            _logger.InfoWithMemberName($"Started");
            _app = application;
            try
            {
                IsLoading = true;
                Stopwatch st = new Stopwatch();
                _logger.InfoWithMemberName($"GetAllChartsAsync() is calling");
                st.Start();
                DateTime nw = DateTime.Now;
                if (_chartUpdateData != null)
                {
                    IsIgnoreUpdateEnabled = true;
                    var currentPdmList = DataCash.ProjectEnvironment.Data.PdmObjectList;
                    var idList = currentPdmList.Where(item => item.PDM_Type != PDM_ENUM.Airspace).Select(t => t.ID).ToList();
                    var airspaceList = currentPdmList.Where(t => t.PDM_Type == PDM_ENUM.Airspace).ToList();
                    airspaceList.ForEach(k =>
                    {
                        var airspace = (PDM.Airspace)k;
                        if (airspace.AirspaceVolumeList != null)
                        {
                            airspace.AirspaceVolumeList.ForEach(b => idList.Add(b.ID));
                        }
                    });
                    _allCharts = await ChartManagerService.GetAffectedChartsAsync(idList, _chartUpdateData.EffectiveDate);

                }
                else if(getOnlyPendingUpdates)
                {
                    IsIgnoreUpdateEnabled = false;
                    _allCharts = await ChartManagerService.GetPendingUpdateListAsync();
                }
                else
                {
                    _allCharts = await ChartManagerService.GetAllChartsAsync(null, string.Empty, null, null, null, null,
                        null, null, null, null, null);
                }
                st.Stop();
                _logger.InfoWithMemberName(
                        $"GetAllChartsAsync() is called in {st.Elapsed.TotalSeconds} seconds and chart count is {_allCharts.Count}");
                st.Start();
                ChartSource = new ObservableCollection<Chart>();
                st.Start();
                _logger.InfoWithMemberName($"GetAllUser() is calling");
                var allUsers = await ChartManagerService.GetAllUserAsync();
                Users = new ObservableCollection<ChartUser>(allUsers);
                st.Stop();

                _currentUser = Users.FirstOrDefault(user =>
                    user.UserName == Config.Username);
                MainViewModel.Title = $"Chart Manager - {_currentUser.FirstName} {_currentUser.LastName}";

                Users.Insert(0, new ChartUser() { FirstName = _allStr });
                SelectedUser = Users[0];


                CreatedFrom = DateTime.Today.AddDays(-1800);
                CreatedTo = DateTime.Today.AddDays(1800);

                EffectiveFrom = CreatedFrom;
                EffectiveTo = CreatedTo;

                ChartTypeSource =
                    new ObservableCollection<ChartType>(Enum.GetValues(typeof(ChartType)).Cast<ChartType>());
                StatusSource =
                    new ObservableCollection<FilterItem>(FilterBuilder.CreateFilterItems());
                SelectedStatus = StatusSource[0];
                
                OrganizationSource =
                    new ObservableCollection<string>(ChartSource.Select(t => t.Organization).Distinct());
                OrganizationSource.Insert(0, _allStr);
                SelectedOrganization = OrganizationSource[0];

                AerodromeSource = new ObservableCollection<string>(ChartSource.Select(t => t.Airport).Distinct());
                AerodromeSource.Insert(0, _allStr);
                SelectedAerodrome = AerodromeSource[0];

                IsAerodromeEnabled = true;
                IsRwyEnabled = true;

                RwyDirSource = new ObservableCollection<string>(ChartSource.Select(t => t.RunwayDirection).Distinct());
                RwyDirSource.Insert(0, _allStr);
                SelectedRwyDir = RwyDirSource[0];
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(this, ex, NotifyService);
            }
            finally
            {
                IsLoading = false;
            }
            _logger.InfoWithMemberName($"Finished");
        }

        private List<Chart> _allCharts { get; set; }

        #region CallBack methods

        public void ChartChanged(Chart chart, ChartCallBackType type)
        {
            Chart foundChart;
            switch (type)
            {
                case ChartCallBackType.Created:
                    NotifyService.ShowMessage($"Chart named {chart.Name} is created on server");
                    ChartSource.Add(chart);
                    break;
                case ChartCallBackType.Deleted:
                    NotifyService.ShowMessage($"Chart named {chart.Name} is deleted from server");
                    foundChart = ChartSource.FirstOrDefault(t => t.Id == chart.Id);
                    if (foundChart != null)
                        ChartSource.Remove(foundChart);
                    break;
                case ChartCallBackType.Locked:
                    NotifyService.ShowMessage($"Chart named {chart.Name} is locked on server");
                    foundChart = ChartSource.FirstOrDefault(t => t.Id == chart.Id);
                    if (foundChart != null)
                    {
                        ChartSource.Remove(foundChart);
                        ChartSource.Add(chart);
                    }
                    break;
                case ChartCallBackType.Unlocked:
                    NotifyService.ShowMessage($"Chart named {chart.Name} is unlocked on server");
                    foundChart = ChartSource.FirstOrDefault(t => t.Id == chart.Id);
                    if (foundChart != null)
                    {
                        ChartSource.Remove(foundChart);
                        ChartSource.Add(chart);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public void AllChartVersionsDeleted(Guid identifier)
        {
            NotifyService.ShowMessage($"All versions of chart (Id:{identifier}) are deleted from server");
            RemoveChartsBy(identifier, null);
        }

        public void ChartsByEffectiveDateDeleted(Guid identifier, DateTime dateTime)
        {
            NotifyService.ShowMessage(
                $"Versions of chart (Id:{identifier}) that begins at {dateTime.ToShortDateString()}  are deleted");
            RemoveChartsBy(identifier, dateTime);
        }

        public void UserChanged(UserCallbackType type)
        {
            switch (type)
            {
                case UserCallbackType.ChangedPassword:
                    NotifyService.ShowMessage(
                        $"Your password is changed. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.Deleted:
                    NotifyService.ShowMessage(
                        $"Your account is deleted. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.PrivilegeUp:
                    NotifyService.ShowMessage(
                        $"Your account is upgraded. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.PrivilegeDown:
                    NotifyService.ShowMessage(
                        $"Your account is downgraded. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.Disabled:
                    NotifyService.ShowMessage(
                        $"Your account is disabled. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.Enabled:
                    NotifyService.ShowMessage(
                        $"Your account is enabled. Please, contact your administrator for further information");
                    break;
                case UserCallbackType.Updated:
                    NotifyService.ShowMessage(
                        $"Your account is updated. Please, contact your administrator for further information");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

        }

        #endregion

        private async Task<string> DownloadFile(bool setLock)
        {

            var dirName = ShowBrowserDialog();
            if (string.IsNullOrEmpty(dirName))
                return string.Empty;
            if (await SaveSource(setLock, dirName))
            {
                string[] files = Directory.GetFiles(dirName, "*.mxd");
                if (files.Length > 0)
                    return files[0];
            }
            return string.Empty;
        }

        private string ShowBrowserDialog()
        {
            var dialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                Description = Resources.InitialViewModel_DownloadFile_Select_folder_to_save_chart,
                SelectedPath = Settings.Default.Folder_Path
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return string.Empty;
            IsLoading = true;
            if (!String.IsNullOrEmpty(Settings.Default.Folder_Path))
            {
                Settings.Default.Folder_Path = dialog.SelectedPath;
            }
            Settings.Default.Folder_Path = dialog.SelectedPath;
            Settings.Default.Save();

            string dirName = Path.Combine(dialog.SelectedPath, SelectedChart.Name);
            if (Directory.Exists(dirName))
            {
                NotifyService.ShowMessage(
                    "The folder already exists. Please, select another folder to continue");
                return string.Empty;
            }
            return dirName;
        }

        private async Task<bool> SaveSource(bool setLock, string dirName)
        {
            byte[] source = null;
            Stopwatch st = new Stopwatch();
            try
            {
                _logger.InfoWithMemberName($"GetSourceOf({SelectedChart.Id},{setLock}) is calling");
                st.Start();
                source = await ChartManagerService.GetSourceOfAsync(SelectedChart.Id, setLock);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(this, ex, NotifyService,
                    $"GetSourceOf({SelectedChart.Id},{setLock})");
                    return false;
            }

            st.Stop();
            _logger.InfoWithMemberName(
                $"Name of chart is {SelectedChart.Name}; Source is {source.Length / 1024} KB and downloaded in {st.Elapsed.TotalSeconds} seconds; Selected path is {Directory.GetParent(dirName)}");
            string zipFileName = Path.Combine(dirName, "source.zip");
            _logger.InfoWithMemberName($"Creating file ({zipFileName})");
            Directory.CreateDirectory(dirName);
            File.Create(zipFileName).Close();
            _logger.InfoWithMemberName($"Writing into file");
            File.WriteAllBytes(zipFileName,
                source);
            _logger.InfoWithMemberName($"Decompressing file");
            ArenaStaticProc.DecompressToDirectory(zipFileName, dirName);
            File.Delete(zipFileName);
            return true;
        }

        private async Task OpenFile(string fileName, bool isReadOnly = false, bool isNew = false)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                _logger.InfoWithMemberName($"Opening file {fileName} in ArcMap");
                await Task.Factory.StartNew(() => _app.OpenDocument(fileName));
                if (isNew)
                    EsriExtensionData.Clear();
                else
                    EsriExtensionData.IsReadOnly = isReadOnly;

                MainViewModel.Close?.Invoke(null, null);
            }
        }

        private void RemoveChartsBy(Guid identifier, DateTime? dateTime = null)
        {
            foreach (var chart in ChartSource.ToList())
            {
                if (chart.Identifier == identifier)
                {
                    if (dateTime == null || chart.BeginEffectiveDate == dateTime.Value)
                        ChartSource.Remove(chart);
                }
            }
        }

        private void PopulateChartSource()
        {
            ChartSource.Clear();
            var charts = _allCharts.Where(t =>
                (t.CreatedAt >= _createdFrom && t.CreatedAt <= _createdTo) &&
                (t.BeginEffectiveDate >= _effectiveFrom && (!t.EndEffectiveDate.HasValue || t.EndEffectiveDate <= _effectiveTo))).ToList();

            if (_selectedUser.FirstName != _allStr)
                charts = charts.Where(t => t.CreatedBy.Id == _selectedUser.Id).ToList();

            if (!string.IsNullOrEmpty(_selectedAerodrome) && _selectedAerodrome != _allStr)
                charts = charts.Where(t => _selectedAerodrome == t.Airport).ToList();

            if (_selectedStatus != null && _selectedStatus?.FilterType != FilterType.All)
                charts = charts.Where(t => t.IsLocked == _selectedStatus.Value).ToList();

            if (!string.IsNullOrEmpty(_selectedOrganization) && _selectedOrganization != _allStr)
                charts = charts.Where(t => t.Organization == _selectedOrganization).ToList();

            if (!string.IsNullOrEmpty(_selectedRwyDir) && _selectedRwyDir != _allStr)
                charts = charts.Where(t => t.RunwayDirection == _selectedRwyDir).ToList();

            if (_selectedChartType != ChartType.All)
                charts = charts.Where(t => t.Type == _selectedChartType).ToList();
            foreach (var chart in charts.ToList())
                ChartSource.Add(chart);
        }

        public override bool CanNext() => true;

        public override bool CanPrevious() => false;

        protected override void _destroy()
        {            
        }

        protected override void SetNext()
        {
            NextState = new HistoryViewModel(MainViewModel, this, SelectedChart);
        }

        protected override void SetPrevious()
        {
            
        }
    }
}