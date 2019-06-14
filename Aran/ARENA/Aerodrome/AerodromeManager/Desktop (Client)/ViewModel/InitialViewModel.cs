
using AmdbManager.Helper;
using AmdbManager.Logging;
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
using EsriWorkEnvironment;
using ESRI.ArcGIS.Controls;
using AerodromeManager.AmdbService;
using AerodromeManager.Properties;
using Aerodrome.Metadata;

namespace AmdbManager.ViewModel
{
    public class InitialViewModel : StateViewModel, IAmdbManagerServiceCallback
    {
        private readonly string _allStr = "All";
        private DateTime _createdFrom;
        private DateTime _createdTo;       
       
        private ObservableCollection<FilterItem> _statusSource;
        private FilterItem _selectedStatus;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private ObservableCollection<string> _organizationSource;
        private string _selectedOrganization;
        private ObservableCollection<string> _aerodromeSource;
        private string _selectedAerodrome;
        private ObservableCollection<string> _rwyDirSource;
        private string _selectedRwyDir;
        private ObservableCollection<AmdbMetadata> _amdbSource;
        private AmdbMetadata _selectedAmdb;
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
        private User _currentUser;
        private bool _isUnlockEnabled;
        private RelayCommand _historyCommand;        
        private Dictionary<long, byte[]> _previews;


        public InitialViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _previews = new Dictionary<long, byte[]>();
            //
        }

        #region Properties

        private IAmdbManagerService ChartManagerService => MainViewModel.AmdbManagerService;
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

        public bool IsUpdateEnabled
        {
            get => _isUpdateEnabled;
            set => Set(ref _isUpdateEnabled, value);
        }

        public bool HasSelectedAmdb
        {
            get => _hasSelectedChart;
            set => Set(ref _hasSelectedChart, value);
        }


        public AmdbMetadata SelectedAmdb
        {
            get => _selectedAmdb;
            set
            {                
                Set(ref _selectedAmdb, value);
                IsLoading = true;
                HasSelectedAmdb = _selectedAmdb != null;
                IsUpdateEnabled = false;
                
                if (_selectedAmdb != null)
                {
                    IsUnlockEnabled = false;
                    
                    if (_selectedAmdb.IsLocked)
                    {
                        if (_selectedAmdb.LockedBy.Id == _currentUser.Id || _currentUser.IsAdmin)
                            IsUnlockEnabled = true;
                    }

                }

                IsLoading = false;
            }
        }

        public ObservableCollection<AmdbMetadata> AmdbSource
        {
            get => _amdbSource;
            set => Set(ref _amdbSource, value);
        }

        public string SelectedAerodrome
        {
            get => _selectedAerodrome;
            set
            {
                Set(ref _selectedAerodrome, value);
                PopulateAmdbSource();
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
                PopulateAmdbSource();
            }
        }

        public ObservableCollection<string> OrganizationSource
        {
            get => _organizationSource;
            set => Set(ref _organizationSource, value);
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(ref _selectedUser, value);
                PopulateAmdbSource();
            }
        }

        public ObservableCollection<User> Users
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
                PopulateAmdbSource();
            }
        }

        public DateTime CreatedTo
        {
            get => _createdTo;
            set
            {
                Set(ref _createdTo, value);
                PopulateAmdbSource();
            }
        }

      
        public ObservableCollection<FilterItem> StatusSource
        {
            get => _statusSource;
            set => Set(ref _statusSource, value);
        }

       
        public FilterItem SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                Set(ref _selectedStatus, value);
                PopulateAmdbSource();
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
                               LogManager.GetLogger(this).InfoWithMemberName($"Started");                               
                               var fileName = await DownloadFile(false);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName, isReadOnly: true);
                               IsLoading = false;
                               LogManager.GetLogger(this).InfoWithMemberName($"Finished");
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
                               LogManager.GetLogger(this).InfoWithMemberName($"Started");
                               //IsLoading = true;
                               var fileName = await DownloadFile(true);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName);
                               IsLoading = false;
                               LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                           }));
            }
        }

        public RelayCommand OpenAsNewAmdbCommand
        {
            get
            {
                return _openAsNewChart ?? (_openAsNewChart = new RelayCommand(
                           async () =>
                           {
                               LogManager.GetLogger(this).InfoWithMemberName($"Started");
                               //IsLoading = true;
                               var fileName = await DownloadFile(false);
                               if (!string.IsNullOrEmpty(fileName))
                                   await OpenFile(fileName, isNew: true);
                               IsLoading = false;
                               LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                           }));
            }
        }

        public RelayCommand DeleteAllCommand
        {
            get
            {
                return _deleteAllCommand ?? (_deleteAllCommand = new RelayCommand(async () =>
                {
                    LogManager.GetLogger(this).InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        LogManager.GetLogger(this).InfoWithMemberName($"DeleteAllChartVersionsAsync({SelectedAmdb.Identifier}) is calling");

                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteAllAmdbVersionsAsync(SelectedAmdb.Identifier);

                            var removedCharts = AmdbSource.Where(t => t.Identifier == SelectedAmdb.Identifier).ToList();
                            foreach (var chart in removedCharts)
                            {
                                AmdbSource.Remove(chart);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteAllChartVersions({SelectedAmdb.Identifier})");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand DeleteLatestVersion
        {
            get
            {
                return _deleteLatestVersion ?? (_deleteLatestVersion = new RelayCommand(async () =>
                {
                    LogManager.GetLogger(this).InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        LogManager.GetLogger(this).InfoWithMemberName($"DeleteChartByIdAsync({SelectedAmdb.Id}) is calling");
                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteAmdbByIdAsync(SelectedAmdb.Id);

                            LogManager.GetLogger(this).InfoWithMemberName($"GetLatestChartVersionAsync({SelectedAmdb.Identifier}) is calling");
                            var latestVersion = await ChartManagerService.GetLatestAmdbVersionAsync(
                                SelectedAmdb.Identifier);
                            AmdbSource.Remove(SelectedAmdb);
                            AmdbSource.Add(latestVersion);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteChartByIdAsync({SelectedAmdb.Id}");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand DeleteAllCurrentVersions
        {
            get
            {
                return _deleteAllCurrentVersions ?? (_deleteAllCurrentVersions = new RelayCommand(async () =>
                {
                    LogManager.GetLogger(this).InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        LogManager.GetLogger(this)
                            .InfoWithMemberName(
                                $"DeleteChartByEffectiveDateAsync({SelectedAmdb.Identifier}) is calling");
                        if (NotifyService.ShowConfirmationMessage("Are you sure to delete") ==
                            System.Windows.MessageBoxResult.Yes)
                        {
                            await ChartManagerService.DeleteAmdbByVersionAsync(SelectedAmdb.Identifier,
                                SelectedAmdb.Version);
                            AmdbSource.Remove(SelectedAmdb);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService,
                            $"DeleteChartByEffectiveDate({SelectedAmdb.Identifier})");
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand UnlockCommand
        {
            get
            {
                return _unlockCommand ?? (_unlockCommand = new RelayCommand(async () =>
                {
                    

                    LogManager.GetLogger(this).InfoWithMemberName($"Started");
                    try
                    {
                        IsLoading = true;
                        LogManager.GetLogger(this).InfoWithMemberName($"LockChartAsync({SelectedAmdb.Id},false) is calling");
                        await ChartManagerService.LockAmdbAsync(SelectedAmdb.Id, false);
                        SelectedAmdb.IsLocked = false;
                        SelectedAmdb.LockedBy = null;
                        IsUnlockEnabled = false;
                        
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Handle(this, ex, NotifyService);
                    }
                    finally
                    {
                        IsLoading = false;
                    }
                    LogManager.GetLogger(this).InfoWithMemberName($"Finished");
                }));
            }
        }

        public RelayCommand HistoryCommand
        {
            get { return _historyCommand ?? (_historyCommand = new RelayCommand(() => 
            {
                LogManager.GetLogger(this).InfoWithMemberName($"Started");
                Next();
                LogManager.GetLogger(this).InfoWithMemberName($"Finished");
            })); }
        }

        #endregion

        public async Task InitializeAsync(IApplication application)
        {            
            LogManager.GetLogger(this).InfoWithMemberName($"Started");
            _app = application;
            try
            {
                IsLoading = true;
                Stopwatch st = new Stopwatch();
                LogManager.GetLogger(this).InfoWithMemberName($"GetAllChartsAsync() is calling");
                st.Start();
                DateTime nw = DateTime.Now;
                AllAmdbFiles = await ChartManagerService.GetAllAmdbFilesAsync(null, null, null, null, null, null,
                    null);
                //st.Stop();
                //LogManager.GetLogger(this)
                //    .InfoWithMemberName(
                //        $"GetAllChartsAsync() is called in {st.Elapsed.TotalSeconds} seconds and chart count is {AllCharts.Count}");
                //st.Start();
                AmdbSource = new ObservableCollection<AmdbMetadata>();
                st.Start();
                LogManager.GetLogger(this).InfoWithMemberName($"GetAllUser() is calling");
                var allUsers = await ChartManagerService.GetAllUserAsync();
                Users = new ObservableCollection<User>(allUsers);
                st.Stop();

                _currentUser = Users.FirstOrDefault(user =>
                    user.UserName == Config.Username);
                MainViewModel.Title = $"AMDB Manager - {_currentUser.FirstName} {_currentUser.LastName}";

                Users.Insert(0, new User() { FirstName = _allStr });
                SelectedUser = Users[0];


                CreatedFrom = DateTime.Today.AddDays(-1800);
                CreatedTo = DateTime.Today.AddDays(1800);

                
                StatusSource =
                    new ObservableCollection<FilterItem>(FilterBuilder.CreateFilterItems());
                SelectedStatus = StatusSource[0];
                
                OrganizationSource =
                    new ObservableCollection<string>(AmdbSource.Select(t => t.Organization).Distinct());
                OrganizationSource.Insert(0, _allStr);
                SelectedOrganization = OrganizationSource[0];

                AerodromeSource = new ObservableCollection<string>(AmdbSource.Select(t => t.Airport).Distinct());
                AerodromeSource.Insert(0, _allStr);
                SelectedAerodrome = AerodromeSource[0];

                IsAerodromeEnabled = true;
                IsRwyEnabled = true;
               
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(this, ex, NotifyService);
            }
            finally
            {
                IsLoading = false;
            }
            LogManager.GetLogger(this).InfoWithMemberName($"Finished");
        }

        private List<AmdbMetadata> AllAmdbFiles { get; set; }

        #region CallBack methods

        public async void AmdbChanged(AmdbMetadata amdb, AmdbCallBackType type)
        {
            AmdbMetadata foundChart;
            switch (type)
            {
                case AmdbCallBackType.Created:
                    NotifyService.ShowMessage($"Chart named {amdb.Name} is created on server");
                    AmdbSource.Add(amdb);
                    break;
                case AmdbCallBackType.Deleted:
                    NotifyService.ShowMessage($"Chart named {amdb.Name} is deleted from server");
                    foundChart = AmdbSource.FirstOrDefault(t => t.Id == amdb.Id);
                    if (foundChart != null)
                        AmdbSource.Remove(foundChart);
                    break;
                case AmdbCallBackType.Locked:
                    NotifyService.ShowMessage($"Chart named {amdb.Name} is locked on server");
                    foundChart = AmdbSource.FirstOrDefault(t => t.Id == amdb.Id);
                    if (foundChart != null)
                    {
                        AmdbSource.Remove(foundChart);
                        AmdbSource.Add(amdb);
                    }
                    break;
                case AmdbCallBackType.Unlocked:
                    NotifyService.ShowMessage($"Chart named {amdb.Name} is unlocked on server");
                    foundChart = AmdbSource.FirstOrDefault(t => t.Id == amdb.Id);
                    if (foundChart != null)
                    {
                        AmdbSource.Remove(foundChart);
                        AmdbSource.Add(amdb);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        public async void AllAmdbVersionsDeleted(Guid identifier)
        {
            NotifyService.ShowMessage($"All versions of chart (Id:{identifier}) are deleted from server");
            RemoveAmdbFilesBy(identifier, null);
        }

        public async void AmdbFilesByVersionDeleted(Guid identifier, int version)
        {
            NotifyService.ShowMessage(
                $"Versions of chart (Id:{identifier}) that begins at {version}  are deleted");
            RemoveAmdbFilesBy(identifier, version);
        }


        public async void UserChanged(AerodromeManager.AmdbService.UserCallbackType type)
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
            //var dialog = new FolderBrowserDialog()
            //{
            //    ShowNewFolderButton = true,
            //    Description = Resources.InitialViewModel_DownloadFile_Select_folder_to_save_chart,
            //    SelectedPath = Settings.Default.Folder_Path
            //};

            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = @"Aerodrome type files (*.amdb)|*.amdb",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return string.Empty;

            //if (dialog.ShowDialog() != DialogResult.OK)
            //    return string.Empty;
            IsLoading = true;
            if (!String.IsNullOrEmpty(Settings.Default.Folder_Path))
            {
                Settings.Default.Folder_Path = saveFileDialog1.FileName;
            }
            Settings.Default.Folder_Path = saveFileDialog1.FileName;
            Settings.Default.Save();

            //TODO: выбрать место для сохранения и указать название файла

            string dirName = saveFileDialog1.FileName;

            //TODO: предупредить если есть файл с таким же именем

            //if (Directory.Exists(dirName))
            //{
            //    NotifyService.ShowMessage(
            //        "The folder already exists. Please, select another folder to continue");
            //    return string.Empty;
            //}


            byte[] source = null;
            Stopwatch st = new Stopwatch();

            LogManager.GetLogger(this).InfoWithMemberName($"GetSourceOf({SelectedAmdb.Id},{setLock}) is calling");

            st.Start();
            source = await ChartManagerService.GetSourceOfAsync(SelectedAmdb.Id, setLock);
            st.Stop();

            string zipFileName = saveFileDialog1.FileName;

            File.WriteAllBytes(zipFileName, source);

            return zipFileName;
        }

        private async Task<bool> SaveSource(bool setLock, string dirName)
        {
            byte[] source = null;
            Stopwatch st = new Stopwatch();
            try
            {
                LogManager.GetLogger(this).InfoWithMemberName($"GetSourceOf({SelectedAmdb.Id},{setLock}) is calling");
                st.Start();
                source = await ChartManagerService.GetSourceOfAsync(SelectedAmdb.Id, setLock);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(this, ex, NotifyService,
                    $"GetSourceOf({SelectedAmdb.Id},{setLock})");
                    return false;
            }

            st.Stop();
           
            string zipFileName = Path.Combine(dirName, "source.zip");
            LogManager.GetLogger(this).InfoWithMemberName($"Creating file ({zipFileName})");
            Directory.CreateDirectory(dirName);
            File.Create(zipFileName).Close();
            LogManager.GetLogger(this).InfoWithMemberName($"Writing into file");
            //Здесь нужно записать файл с тем же именем под которым был сохранен на сервере.
            //Также из этого метода нужно возвращать путь к файлу чтобы передать методу для открытия .amdb файла
            File.WriteAllBytes(zipFileName,
                source);

            //В моем случаи пока распаковка не нужна так как на сервере хранится файл формата .amdb для открытия которого есть специальный метод
            LogManager.GetLogger(this).InfoWithMemberName($"Decompressing file");
            ArenaStaticProc.DecompressToDirectory(zipFileName, dirName);
            File.Delete(zipFileName);
            return true;
        }

        private async Task OpenFile(string fileName, bool isReadOnly = false, bool isNew = false)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                LogManager.GetLogger(this).InfoWithMemberName($"Opening file {fileName} in ArcMap");
                HelperMethods.OpenAmdbProject(fileName, _app);
                if (isNew)
                    ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Clear();
                else
                    ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.IsReadOnly = isReadOnly;

                HelperMethods.SaveAmdbProject(_app, isCommitFeatures: false);
                MainViewModel.Close?.Invoke(null, null);
            }
        }

        private void RemoveAmdbFilesBy(Guid identifier, int? version)
        {
            foreach (var amdb in AmdbSource.ToList())
            {
                if (amdb.Identifier == identifier)
                {
                    if (version==null || amdb.Version == version.ToString())
                        AmdbSource.Remove(amdb);
                }
            }
        }

        private void PopulateAmdbSource()
        {
            AmdbSource.Clear();
            var amdbFiless = AllAmdbFiles.Where(t =>
               (t.CreatedAt >= _createdFrom && t.CreatedAt <= _createdTo))?.ToList();

            if (_selectedUser.FirstName != _allStr)
                amdbFiless = amdbFiless.Where(t => t.CreatedBy.Id == _selectedUser.Id).ToList();

            if (!string.IsNullOrEmpty(_selectedAerodrome) && _selectedAerodrome != _allStr)
                amdbFiless = amdbFiless.Where(t => _selectedAerodrome == t.Airport).ToList();

            if (_selectedStatus != null && _selectedStatus?.FilterType != FilterType.All)
                amdbFiless = amdbFiless.Where(t => t.IsLocked == _selectedStatus.Value).ToList();

            if (!string.IsNullOrEmpty(_selectedOrganization) && _selectedOrganization != _allStr)
                amdbFiless = amdbFiless.Where(t => t.Organization == _selectedOrganization).ToList();
                  
            foreach (var chart in amdbFiless.ToList())
                AmdbSource.Add(chart);
        }

        public override bool CanNext() => true;

        public override bool CanPrevious() => false;

        protected override void _destroy()
        {            
        }

        protected override void SetNext()
        {
            NextState = new HistoryViewModel(MainViewModel, this, SelectedAmdb);
        }

        protected override void SetPrevious()
        {
            
        }

        public void AllChartVersionsDeleted(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public void ChartsByEffectiveDateDeleted(Guid identifier, string version)
        {
            throw new NotImplementedException();
        }
    }
}