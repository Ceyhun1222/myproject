using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using AranUpdater;
using AranUpdWinService.Data;

namespace AranUpdWinService
{
    public partial class AranUpdService : ServiceBase
    {
        private EventLog _eventLog;
        private Timer _timer;
        private UpdaterDbProvider _dbPro;
        private string _userName;
        private Settings _settings;
        //private NamePipeServer _server;
        private AgentCommServer _server;
        private string _lastLogMessage;
        private DateTime _lastLogMessageDateTime;
        private bool _isTimerElapsed;
        private bool _firstTimeNullUserName;


        public AranUpdService()
        {
            InitializeComponent();

            _dbPro = new UpdaterDbProvider();
            _settings = new Settings();
            _firstTimeNullUserName = true;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Enabled = false;
            _timer.Elapsed += Timer_Elapsed;

            _server = new AgentCommServer();
            _server.SettingsSetted += CommServer_SettingsSetted;
        }

        private void CommServer_SettingsSetted(object sender, SettingsSettedEventArgs e)
        {
            _settings.Server = e.Server;
            _settings.Port = e.Port;
            _settings.Save();

            _server.SetInitInfoCommand(_settings.Server, _settings.Port, _userName);
        }

        public void Test(string cmd)
        {
            if (cmd == "Start")
                OnStart(null);
            else if (cmd == "Stop")
                OnStop();
            else if (cmd == "SessionChange")
                OnSessionChange(new SessionChangeDescription());
            else if (cmd == "Timer_Elapsed")
                Timer_Elapsed(null, null);
        }


        protected override void OnStart(string[] args)
        {
            var eventName = "ARANUpdater";

            if (!EventLog.SourceExists(eventName))
                EventLog.CreateEventSource(eventName, "ARAN Updater Log");

            _eventLog = new EventLog();
            _eventLog.Source = eventName;

            try
            {
                _server.Start();
            }
            catch (Exception ex)
            {
                var s = "Error on Start Agent Communication Server: " + ex.Message;
                _eventLog.WriteEntry(s, EventLogEntryType.Error);
                WriteLogMessage(s);
                return;
            }

            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var configFileName = Path.Combine(dir, "AranUpd.config");

            _settings.Open(configFileName);

            _timer.Interval = 1 * 1000;
            _timer.Enabled = true;

            try
            {
                var userName = GetUserName();
                if (userName != null)
                    _userName = userName;
            }
            catch (Exception ex)
            {
                var s = "Error on GetUserName: " + ex.Message;
                _eventLog.WriteEntry(s, EventLogEntryType.Error);
                WriteLogMessage(s);
                return;
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();

            if (_server != null)
            {
                _server.Stop();
            }
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            _userName = GetUserName();
            _server.SetInitInfoCommand(_settings.Server, _settings.Port, _userName);
            base.OnSessionChange(changeDescription);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isTimerElapsed)
                return;

            _isTimerElapsed = true;
            _timer.Enabled = false;

            if (string.IsNullOrWhiteSpace(_settings.Server))
            {
                _server.SendCommand(CommandType.SettingsNotDefined);
                _timer.Enabled = true;
                _isTimerElapsed = false;
                return;
            }

            if (string.IsNullOrEmpty(_userName))
            {
                if (_firstTimeNullUserName)
                {
                    _firstTimeNullUserName = false;
                    _userName = GetUserName();
                }

                if (string.IsNullOrEmpty(_userName))
                {
                    _userName = GetUserNameByProcess();

                    if (string.IsNullOrEmpty(_userName))
                    {
                        _timer.Enabled = true;
                        _isTimerElapsed = false;
                        return;
                    }
                }
            }

            if (!_dbPro.IsOpen)
            {
                try
                {
                    _dbPro.Open(_settings.Server, _settings.Port);
                    _server.SendCommand(CommandType.Online);
                }
                catch (Exception ex)
                {
                    var s = "Error: " + ex.Message;
                    _eventLog.WriteEntry(s, EventLogEntryType.Error);
                    WriteLogMessage(s);
                    _server.SendCommand(CommandType.Offline);
                    _timer.Enabled = true;
                    _isTimerElapsed = false;
                    return;
                }
            }

            if (!_dbPro.IsRegistered)
            {
                try
                {
                    _dbPro.Register(_userName);
                    _server.SendCommand(CommandType.Online);
                }
                catch (Exception ex)
                {
                    var s = "Error: " + ex.Message;
                    _eventLog.WriteEntry(s, EventLogEntryType.Error);
                    WriteLogMessage(s);
                    _server.SendCommand(CommandType.Offline);
                    _timer.Enabled = true;
                    _isTimerElapsed = false;
                    return;
                }
            }

            AranVersionInfo newVersionInfo = null;

            try
            {
                newVersionInfo = _dbPro.GetNewVersion();

                _dbPro.SetLastVersion(newVersionInfo.VersionId.Value, LastVersionType.Downloaded);
                _server.SendCommand(CommandType.Online, newVersionInfo.CurrVersionName);
            }
            catch (WebException)
            {
                _eventLog.WriteEntry("WebException", EventLogEntryType.Error);
                _server.SendCommand(CommandType.Offline);
                _timer.Enabled = true;
                _isTimerElapsed = false;
                return;
            }
            catch (Exception ex)
            {
                var s = "Error: " + ex.Message;
                _eventLog.WriteEntry(s, EventLogEntryType.Error);
                //WriteLogMessage(s);
                _server.SendCommand(CommandType.Error, ex.Message);
                _timer.Enabled = true;
                _isTimerElapsed = false;
                return;
            }

            _timer.Interval = newVersionInfo.UpdateIntervalSec.Value * 1000;

            if (newVersionInfo == null || newVersionInfo.Data == null || newVersionInfo.Data.Length == 0)
            {
                _timer.Enabled = true;
                _isTimerElapsed = false;
                return;
            }

            var updater = new Updater(AppDomain.CurrentDomain.BaseDirectory, _userName);
            string errorText;

            if (updater.UpdateFiles(newVersionInfo.Data, out errorText))
                _dbPro.SetLastVersion(newVersionInfo.VersionId.Value, LastVersionType.Updated);

            if (errorText.Length > 0)
                _dbPro.AddLog(errorText);

            _timer.Enabled = true;
            _isTimerElapsed = false;
        }

        private void WriteLogMessage(string message)
        {
            var now = DateTime.Now;

            if (message == _lastLogMessage && (now - _lastLogMessageDateTime).TotalMinutes < 5)
                return;

            _lastLogMessage = message;
            _lastLogMessageDateTime = now;

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", now.ToString("yyyy-MM-dd") + ".log");
            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("{0}\t{1}", now.ToString("HH:mm"), message));
            sw.Close();
            fs.Close();
            fs.Dispose();
        }

        private static string GetUserName()
        {
            string username = string.Empty;

            try
            {
                var ms = new ManagementScope("\\\\.\\root\\cimv2");
                ms.Connect();

                var query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                var searcher = new ManagementObjectSearcher(ms, query);

                // This loop will only run at most once.
                foreach (ManagementObject mo in searcher.Get())
                {
                    username = mo["UserName"].ToString();
                }

                return username;
            }
            catch { }

            return username;
        }

        private static string GetUserNameByProcess()
        {
            var processes = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name='explorer.exe'");

            foreach (ManagementObject process in processes.Get())
            {
                var OwnerInfo = new string[2];
                process.InvokeMethod("GetOwner", (object[])OwnerInfo);

                return OwnerInfo[1] + "\\" + OwnerInfo[0];
            }

            return string.Empty;
        }
    }
}
