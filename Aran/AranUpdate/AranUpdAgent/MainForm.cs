using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AranUpdAgent
{
    public partial class MainForm : Form
    {
        private bool _isVisible;
        private string _server;
        private int _port;
        private string _userName;
        private string _version;
        private CommandType _lastCommandType;
        private string _lastMessage;
        private DateTime _lastMessageDate;
        private Client _client;
        private bool _isOnline;
        private bool _isFirstTimeOnline;

        public MainForm()
        {            
            InitializeComponent();

            _client = new Client();
            _client.DataRead += OnDataRead;
            _client.Disconnected += OnClientDisconnected;
            _isFirstTimeOnline = true;

            _isVisible = false;
            _isOnline = false;
            _lastCommandType = CommandType.None;

            SetIcon();

            ui_statusLabel.Text = string.Empty;
            ui_versionLabel.Text = string.Empty;
            ui_userNameLabel.Text = string.Empty;
        }

        public bool Start()
        {
            return _client.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isVisible = false;
            Visible = false;

            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_isVisible ? value : false);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            _isVisible = true;
            Visible = true;
        }

        private void OnDataRead(object sender, DataReaderEventArgs e)
        {
            var now = DateTime.Now;

            if (e.CmdType == _lastCommandType &&
                e.Message == _lastMessage &&
                (now - _lastMessageDate).TotalMinutes < 0.5)
            {
                return;
            }

            _lastCommandType = e.CmdType;
            _lastMessage = e.Message;
            _lastMessageDate = now;


            var s = e.CmdType.ToString();

            switch (e.CmdType)
            {
                case CommandType.InitInfo:
                    {
                        _server = string.Empty;
                        _port = 0;
                        _userName = string.Empty;

                        var msg = e.Message.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (msg.Length >= 3)
                        {
                            _server = msg[0];
                            int.TryParse(msg[1], out _port);
                            _userName = msg[2];
                        }

                        ui_userNameLabel.Text = _userName;
                        ui_serverTB.Text = _server;
                        ui_portNud.Value = _port;
                        break;
                    }
                case CommandType.Online:
                    if (!_isOnline)
                    {
                        _isOnline = true;
                        _version = string.Empty;

                        if (!string.IsNullOrEmpty(e.Message))
                            _version = e.Message;

                        ui_versionLabel.Text = _version;

                        SetIcon();

                        if (_isFirstTimeOnline)
                            _isFirstTimeOnline = false;
                        else
                            ui_mainNotifyIcon.ShowBalloonTip(0, "Aran Updater", "Connected to the server", ToolTipIcon.Info);
                    }
                    break;
                case CommandType.Offline:
                    if (_isOnline)
                    {
                        _isOnline = false;
                        SetIcon();
                        ui_mainNotifyIcon.ShowBalloonTip(0, "Aran Updater", "Disconnected from the server", ToolTipIcon.Warning);
                    }
                    break;
                case CommandType.SettingsNotDefined:
                    ui_mainNotifyIcon.ShowBalloonTip(0, "Aran Updater", "Settings is not defined!", ToolTipIcon.Error);
                    break;
                case CommandType.Error:
                    ui_mainNotifyIcon.ShowBalloonTip(0, "Aran Updater - Error", e.Message, ToolTipIcon.Error);
                    break;
                default:
                    break;
            }
        }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                Text = "Disconnected.";
            }));

            Close();

            //if (_isVisible)
            //{
            //    Invoke(new Action(() =>
            //    {
            //        Application.Exit();
            //    }));
            //}
            //else
            //{
            //    Close();
            //}
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            _client.SendCommand(CommandType.SettingsSaved, string.Format("{0};{1}", ui_serverTB.Text, (int)ui_portNud.Value));
        }

        private void SetIcon()
        {
            ui_mainNotifyIcon.Icon = _isOnline ? Properties.Resources.aran_upd_green : Properties.Resources.aran_upd_red;
            if (_isVisible)
                ui_onlinePictureBox.Image = _isOnline ? Properties.Resources.circle_green : Properties.Resources.circle_red;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            ui_onlinePictureBox.Image = _isOnline ? Properties.Resources.circle_green : Properties.Resources.circle_red;
            ui_statusLabel.Text = _isOnline ? "Connected" : "Disconnect";

            base.OnVisibleChanged(e);
        }
    }
}
