using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.ViewModel.Pane.Base;
using static Aran.Temporality.CommonUtil.Context.CurrentDataContext;

namespace TOSSM.ViewModel.Tool
{
    public class LogViewerToolViewModel  : ToolViewModel
    {
        public static string ToolContentId = "Logs";
       
        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/log.png", UriKind.RelativeOrAbsolute);

        #region Ctor

        public LogViewerToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
            SelectedLogLevel = LogLevels[0];
            CurrentLogLevel = CurrentNoAixmDataService.GetLogLevel();
        }

        #endregion

        protected override bool Enable()
        {
            return (CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.SuperAdmin) != 0;
        }

        public List<LogEntry> DataFiltered { get; set; }

        private IList<int> _ids=new List<int>();


        #region Pages
        private int PageSize = 100;

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged("TotalPages");
                OnPropertyChanged("CurrentPageNumber");
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
                OnPropertyChanged("CurrentPageNumber");
                ShowPage();
            }
        }

        public int CurrentPageNumber => TotalPages==0?0:CurrentPage + 1;

        private void ShowPage()
        {
            var pageIds=_ids.Skip(CurrentPage*PageSize).Take(PageSize).ToList();
            DataFiltered = pageIds.Count > 0 ? CurrentNoAixmDataService.GetLogByIds(pageIds).OrderByDescending(t=>t.Id).ToList() : 
                new List<LogEntry>();
            OnPropertyChanged("DataFiltered");

            DataGridVisibility = DataFiltered.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            Status = "Displayed " + ((CurrentPage * PageSize)+1) +"-"+((CurrentPage * PageSize) + DataFiltered.Count) + " items from " + _ids.Count;
        }


        #endregion

        private void ReloadData()
        {
           _ids = CurrentNoAixmDataService.GetLogIds(FromDate, ToDate, null, //storage
                Application, User, IpAddress, Action, Parameter, IsAccessGranted);

           _ids = _ids.OrderByDescending(t => t).ToList();

            TotalPages = _ids.Count/PageSize;
            if (_ids.Count > TotalPages*PageSize) TotalPages++;

            CurrentPage = 0;

            Applications=CurrentNoAixmDataService.GetLogValues("Application").Where(t=>t!=null).Select(t => t.ToString()).Except(new []{string.Empty}).ToList();
            Users = CurrentNoAixmDataService.GetLogValues("UserName").Where(t => t != null).Select(t => t.ToString()).Except(new[] { string.Empty }).ToList();
            Addresses = CurrentNoAixmDataService.GetLogValues("Ip").Where(t => t != null).Select(t => t.ToString()).Except(new[] { string.Empty }).ToList();
            Actions = CurrentNoAixmDataService.GetLogValues("Action").Where(t => t != null).Select(t => t.ToString()).Except(new[] { string.Empty }).ToList();
            CurrentLogLevel = CurrentNoAixmDataService.GetLogLevel();
        }

        #region AdminLogVisibility

        public Visibility AdminLogVisiblity => ((CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.Admin) != 0) || ((CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.SuperAdmin) != 0)? Visibility.Visible
            : Visibility.Hidden;

        #endregion
        #region DataGridVisibility
        private Visibility _dataGridVisibility=Visibility.Hidden;
        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged("DataGridVisibility");
            }
        }
        #endregion

        #region Status
        private string _status;
     
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        #endregion

        #region From Date
        private DateTime _fromDate = DateTime.Now.AddMonths(-1);
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }
        #endregion

        #region To Date
        private DateTime _toDate = DateTime.Now.AddMonths(1);
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        #endregion

        #region IpAddress

        private string _ipAddress;
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }

        #endregion

        #region Application

        private string _application ;
        public string Application
        {
            get => _application;
            set
            {
                _application = value;
                OnPropertyChanged("Application");
            }
        }

        #endregion

        #region User

        private string _user ;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        #endregion

        #region Action

        private string _action ;
        public string Action
        {
            get => _action;
            set
            {
                _action = value;
                OnPropertyChanged("Action");
            }
        }

        #endregion

        #region Parameter

        private string _parameter ;
        public string Parameter
        {
            get => _parameter;
            set
            {
                _parameter = value;
                OnPropertyChanged("Parameter");
            }
        }

        #endregion

        #region IsAccessGranted

        private bool _isAccessGranted = true;

        public bool IsAccessGranted
        {
            get => _isAccessGranted;
            set
            {
                _isAccessGranted = value;
                OnPropertyChanged("IsAccessGranted");
            }
        }

        #endregion

        #region SearchCommand
        private RelayCommand _searchCommand;
     
        public RelayCommand SearchCommand
        {
            get { return _searchCommand??(_searchCommand=new RelayCommand(
                t =>
                {
                    BlockerModel.BlockForAction(ReloadData);
                }
                )); }
        }
        #endregion

        #region PrevCommand
        private RelayCommand _prevCommand;

        public RelayCommand PrevCommand
        {
            get
            {
                return _prevCommand ?? (_prevCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (CurrentPage>0) CurrentPage--;
                        });
                    }
                    ));
            }
        }
        #endregion

        #region NextCommand
        private RelayCommand _nextCommand;

        public RelayCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (CurrentPage < TotalPages-1) CurrentPage++;
                        });
                    }
                    ));
            }
        }
        #endregion

        #region FirstCommand
        private RelayCommand _firstCommand;

        public RelayCommand FirstCommand
        {
            get
            {
                return _firstCommand ?? (_firstCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (CurrentPage>0) CurrentPage=0;
                        });
                    }
                    ));
            }
        }
        #endregion

        #region LastCommand
        private RelayCommand _lastCommand;

        public RelayCommand LastCommand
        {
            get
            {
                return _lastCommand ?? (_lastCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (CurrentPage < TotalPages - 1) CurrentPage = TotalPages - 1;
                        });
                    }
                    ));
            }
        }
        #endregion

        #region ChangeLogLevelCommand
        private RelayCommand _changeLogLevelCommand;

        public RelayCommand ChangeLogLevelCommand
        {
            get
            {
                return _changeLogLevelCommand ?? (_changeLogLevelCommand = new RelayCommand(
                    t =>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (CurrentNoAixmDataService.LogConfigured())
                            {
                                if (CurrentNoAixmDataService.SetLogLevel(SelectedLogLevel))
                                {
                                    CurrentLogLevel = CurrentNoAixmDataService.GetLogLevel();
                                }
                            }
                        });
                    }
                    ));
            }
        }
        #endregion
        

        #region Applications

        private List<string> _applications;
        public List<string> Applications
        {
            get => _applications;
            set
            {
                _applications = value;
                OnPropertyChanged("Applications");
            }
        }

        #endregion

        #region Users

        private List<string> _users;
        public List<string> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        #endregion

        #region Addresses

        private List<string> _addresses;
        public List<string> Addresses
        {
            get => _addresses;
            set
            {
                _addresses = value;
                OnPropertyChanged("Addresses");
            }
        }

        #endregion


        #region Actions

        private List<string> _actions;
        public List<string> Actions
        {
            get => _actions;
            set
            {
                _actions = value;
                OnPropertyChanged("Actions");
            }
        }

        #endregion


        private LogLevel _selectedLogLevel;
        public LogLevel SelectedLogLevel
        {
            get => _selectedLogLevel;
            set
            {
                _selectedLogLevel = value;
                OnPropertyChanged(nameof(SelectedLogLevel));
            }
        }

        private LogLevel _currentLogLevel;
        public LogLevel CurrentLogLevel
        {
            get => _currentLogLevel;
            set
            {
                _currentLogLevel = value;
                OnPropertyChanged(nameof(CurrentLogLevel));
            }
        }


        private List<LogLevel> _logLevels;
        public List<LogLevel> LogLevels => _logLevels ?? (_logLevels = new List<LogLevel> {LogLevel.Info, LogLevel.Debug, LogLevel.Trace});

        #region Logs

        #endregion

        #region BlockerModel
        private BlockerModel _blockerModel;
     
        public BlockerModel BlockerModel
        {
            get => _blockerModel??(_blockerModel=new BlockerModel{ActivatingObject = this});
            set => _blockerModel = value;
        }
        #endregion

    }
}
