using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.AranEnvironment;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EsriGeom = ESRI.ArcGIS.Geometry;

namespace MapEnv
{
    public partial class NewProjectForm : Form
    {
        private int _pageIndex;
        private int _pageCount;
        private string[] _pageTitles;
        private List<ISettingsPage> _settingsPageList;
        private int _currentSettingsPageIndex;
        private bool _isProjectLoingPage;


        public NewProjectForm()
        {
            InitializeComponent();

            _pageIndex = 0;
            _pageTitles = new string[] {
                "Existing Projects:",
                "Connect to the Database",
                "Plugins",
                null,
                "Project File Name"
            };
            _pageCount = _pageTitles.Length;
            _settingsPageList = new List<ISettingsPage>();

            ui_recentPage.Dock =
                ui_dbProContainerPanel.Dock =
                ui_pluginPage.Dock =
                ui_fileNamePage.Dock =
                ui_pluginSettingsPage.Dock = DockStyle.Fill;

            ui_pluginPage.BorderStyle =
                ui_recentPage.BorderStyle =
                ui_dbProContainerPanel.BorderStyle =
                ui_fileNamePage.BorderStyle =
                ui_pluginSettingsPage.BorderStyle = BorderStyle.None;

            _currentSettingsPageIndex = 0;

            ui_dbProControlServer.Dock = DockStyle.Fill;
            ui_dbProControlServer.BorderStyle = BorderStyle.None;
            ui_dbProControlLocal.Dock = DockStyle.Fill;
            ui_dbProControlLocal.BorderStyle = BorderStyle.None;

#if WITH_LOCALDB
            ui_dbProContainerPanelLeft.Visible = true;

            var conn = new Connection();
            conn.ConnectionType = ConnectionType.AimLocal;
            ui_dbProControlLocal.SetValue(conn);
#endif
        }


        public event OpenFileEventHandler OpenFileClicked;
        public event OpenFileEventHandler VerifyUserName;
        public event EventHandler NewProjectClicked;


        public bool IsNewProject { get; private set; }

        public string ProjectFileName { get; private set; }

        public Connection GetConnection()
        {
            var conn = DbProControl.GetValue();
            return conn;
        }

        public ReadOnlyCollection<ISettingsPage> SettingsPages
        {
            get { return new ReadOnlyCollection<ISettingsPage>(_settingsPageList); }
        }

        public ReadOnlyCollection<AranPlugin> AranPlugins
        {
            get
            {
                return new ReadOnlyCollection<AranPlugin>(ui_pluginPage.GetSelectedPlugins());
            }
        }

        public EsriGeom.ISpatialReference GetSpatialReference()
        {
            return ui_pluginPage.GetSpatialReference();
        }


        #region PageTypes enum

        private enum PageTypes
        {
            ExistingProject,
            Connection,
            //User,
            Plugin,
            PluginSettigns,
            FileName
        }

        #endregion


        private int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;

                if (DbPro.ProviderType == DbProviderType.TDB)
                {
                    if (value == (int)PageTypes.Connection)
                        _pageIndex = (int)PageTypes.Plugin;
                    
                    //else if (value == (int)PageTypes.User)
                    //    _pageIndex = (int)PageTypes.ExistingProject;
                }

                ui_newProjectButton.Visible = (_pageIndex == 0);
                //ui_newLocalProButton.Visible = (_pageIndex == 0);
                ui_openFileButton.Visible = (_pageIndex == 0);
                ui_okButton.Visible = (_pageIndex == 0 || _isProjectLoingPage);
                ui_backButton.Visible = (_pageIndex > 0 || _isProjectLoingPage);
                ui_nextButton.Visible = (_pageIndex > 0 && _pageIndex < _pageCount - 1);
                ui_finishButton.Visible = (_pageIndex == _pageCount - 1);
                ui_recentPage.Visible = (_pageIndex == 0 && !_isProjectLoingPage);
                ui_dbProContainerPanel.Visible = (_pageIndex == (int)PageTypes.Connection || _isProjectLoingPage);
                ui_pluginPage.Visible = (_pageIndex == (int)PageTypes.Plugin);
                ui_pluginSettingsPage.Visible = (_pageIndex == (int)PageTypes.PluginSettigns);
                ui_fileNamePage.Visible = (_pageIndex == (int)PageTypes.FileName);

                if (_isProjectLoingPage) {
                    ui_titleLabel.Text = "Login";
                }
                else {
                    if (_pageTitles[_pageIndex] == null)
                        ui_titleLabel.Text = _settingsPageList[_currentSettingsPageIndex].Title + " Settings Page";
                    else
                        ui_titleLabel.Text = _pageTitles[_pageIndex];

                    RecentFiles_SelectedFileNameChanged(null, null);
                }
            }
        }

        public DbProvider DbPro
        {
            get { return Globals.MainForm.DbProvider; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var recentFile in Globals.Settings.RecentProjectFiles)
                ui_recentPage.AddFile(recentFile);
            ui_recentPage.RefreshGrid();

            PageIndex = _pageIndex;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            IsNewProject = false;

            if (_isProjectLoingPage) {
                Debug.Assert(VerifyUserName != null, "VerifyUserName event not handled!");

                var conn = DbProControl.GetValue();
                var savePassword = false;

                var ofe = new OpenFileEventArgs(conn.Password, false, savePassword);
                ofe.UserName = conn.UserName;
                VerifyUserName(this, ofe);

                if (!string.IsNullOrEmpty(ofe.ErrorMessage)) {
                    DbProControl.ErrorMessage = ofe.ErrorMessage;
                    return;
                }

                if (DbPro.ProviderType == DbProviderType.ComSoft)
                {
                    var wpName = new WorkPackageName();
                    if (Globals.Environment.GetExtData(WorkPackageForm.CurrentWorkPackageDataKey, wpName))
                    {
                        if (wpName.Id > 0)
                            (DbPro as CawDbProvider).IncludedWorkPackage = wpName;
                    }
                }

                DialogResult = DialogResult.OK;
                return;
            }

            if (ui_recentPage.SelectedFileName == null)
                return;

            ProjectFileName = ui_recentPage.SelectedFileName;
            FileSelected();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = Globals.OpenFileFilters;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            ProjectFileName = ofd.FileName;
            FileSelected();
        }

        private void FileSelected()
        {
            Debug.Assert(OpenFileClicked != null, "OpenFileClicked event not handled!");
            
            var ofea = new OpenFileEventArgs(ProjectFileName, true, false);
            OpenFileClicked(this, ofea);

            if (string.IsNullOrEmpty(ofea.ErrorMessage)) {
                if (ofea.ShowLogin) {
                    _isProjectLoingPage = true;
                    ui_okButton.Enabled = true;

                    // Visible Connection Panel;
                    PageIndex = 0;

                    var conn = Globals.MapData.Connection.Clone() as Connection;
                    if (Globals.Settings.IsUserNamePasswordSaved)
                        conn.Password = Globals.Settings.Password;

                    if (DbProControl.IsSupportLogin) {
                        DbProControl.LoginView = true;    
                    }
                    else {
                        DialogResult = DialogResult.OK;
                    }

                    DbProControl.IsVisibleSavePasswordBox = false;
                    DbProControl.SetValue(conn);
                }
                else {
                    DialogResult = DialogResult.OK;
                }
            }
            else {
                Globals.ShowError(ofea.ErrorMessage);
            }
        }

        private void RecentFiles_SelectedFileNameChanged(object sender, EventArgs e)
        {
            ui_okButton.Enabled = (ui_recentPage.SelectedFileName != null);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            _isProjectLoingPage = false;

            var step = 1;

            if (PageIndex == (int)PageTypes.ExistingProject)
            {
                if (NewProjectClicked != null)
                    NewProjectClicked(this, e);

                if (DbPro.ProviderType == DbProviderType.TDB)
                    ui_pluginPage.FillPlugins();

                #region ToConnectionPage

                DbProControl.LoginView = false;
                DbProControl.ErrorMessage = string.Empty;

                var conn = new Connection();
                conn.ConnectionType = Globals.DbProTypeToConnectionType(DbPro.ProviderType);

                if (Globals.Settings.DbConnection != null &&
                    conn.ConnectionType == Globals.Settings.DbConnection.ConnectionType)
                {
                    conn.Server = Globals.Settings.DbConnection.Server;
                    conn.Port = Globals.Settings.DbConnection.Port;
                    conn.Database = Globals.Settings.DbConnection.Database;
                    conn.UserName = Globals.Settings.DbConnection.UserName;

                    if (Globals.Settings.IsUserNamePasswordSaved)
                    {
                        conn.UserName = Globals.Settings.UserName;
                        conn.Password = Globals.Settings.Password;
                        DbProControl.IsPasswordSaved = Globals.Settings.IsUserNamePasswordSaved;
                    }
                }

                DbProControl.IsVisibleSavePasswordBox = true;
                DbProControl.SetValue(conn);

                #endregion

            }
            //*** Connection to User Page
            else if (PageIndex == (int)PageTypes.Connection)
            {
                try
                {
                    if (DbPro.State == ConnectionState.Open)
                        DbPro.Close();

                    var conn = DbProControl.GetValue();
                    var password = conn.Password;
                    conn.Password = string.Empty;

                    if (DbProControl.IsPasswordSaved)
                        Globals.Settings.SetUserNamePassword(conn.UserName, password);

                    if (DbPro.ProviderType == DbProviderType.Aran)
                        password = DbUtility.GetMd5Hash(password);

                    try
                    {
                        DbPro.Open(conn.GetConnectionString());
                    }
                    catch (Exception connEx)
                    {
                        DbProControl.ErrorMessage = connEx.Message;
                        return;
                    }

                    if (!DbPro.Login(conn.UserName, password))
                    {
                        DbProControl.ErrorMessage = "Invalid User name or Password";
                        return;
                    }

                    ui_pluginPage.FillPlugins();
                }
                catch (Exception ex)
                {
                    Globals.ShowError(ex.Message);
                    return;
                }

            }
            //*** Plugin To the Next Page (Plugin's Setting page or FileName Page)
            else if (PageIndex == (int)PageTypes.Plugin)
            {

                Globals.MainForm.SetDbProAiracDate(ui_pluginPage.AiracDateTime);

                _settingsPageList.Clear();
                var pluginList = ui_pluginPage.GetSelectedPlugins();
                foreach (var plugin in pluginList)
                {
                    foreach (var item in Globals.SettingsPageList)
                    {
                        if (item.BaseOnPlugins.Contains(plugin.Id))
                        {
                            if (!_settingsPageList.Contains(item.Page))
                                _settingsPageList.Add(item.Page);
                        }
                    }
                }

                if (_settingsPageList.Count == 0)
                {
                    step++;
                }
                else
                {

                    foreach (var sp in _settingsPageList)
                    {
                        var sc = sp.Page;
                        sc.Dock = DockStyle.Fill;
                        sc.Visible = false;
                        ui_pluginSettingsPage.Controls.Add(sc);
                    }

                    _settingsPageList[0].Page.Visible = true;
                    _settingsPageList[0].OnLoad();
                    ui_titleLabel.Text = _settingsPageList[0].Title;
                    _currentSettingsPageIndex = 0;
                }
            }
            //*** Plugin's Settings
            else if (PageIndex == (int)PageTypes.PluginSettigns)
            {
                _settingsPageList[_currentSettingsPageIndex].OnSave();
                _settingsPageList[_currentSettingsPageIndex].Page.Visible = false;

                if (_settingsPageList.Count > 0 && _currentSettingsPageIndex < _settingsPageList.Count - 1)
                {
                    step = 0;
                    _currentSettingsPageIndex++;
                    _settingsPageList[_currentSettingsPageIndex].Page.Visible = true;
                    _settingsPageList[_currentSettingsPageIndex].OnLoad();
                }
            }

            PageIndex += step;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            var step = 1;
            
            //*** Plugin's Settings
            if (PageIndex == (int)PageTypes.PluginSettigns) {
                if (_currentSettingsPageIndex > 0) {
                    step = 0;
                    _settingsPageList[_currentSettingsPageIndex].Page.Visible = false;
                    _currentSettingsPageIndex--;
                    _settingsPageList[_currentSettingsPageIndex].Page.Visible = true;
                }
            }
            else if (PageIndex == (int)PageTypes.FileName) {
                if (_settingsPageList.Count == 0) {
                    step++;
                }
            }
            else if (_isProjectLoingPage) {
                step = 0;
                _isProjectLoingPage = false;
            }

            PageIndex -= step;
        }

        private void Finish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ui_fileNamePage.FileName)) {
                ui_fileNamePage.ErrorMessage = "Please, set project file name.";
                return;
            }

            ProjectFileName = ui_fileNamePage.FileName;
            IsNewProject = true;
            DialogResult = DialogResult.OK;
        }

        private void dbProviderControl21_LoadedDbList(object sender, Aran.Queries.Common.DbProControlLoadDbListEventArgs e)
        {
            var dbList = DbPro.GetAllDBList(e.Server, e.Port);

            if (dbList != null)
                e.DbList.AddRange(dbList);
        }

        private void NewLocalProject_Click(object sender, EventArgs e)
        {
            Next_Click(sender, e);
        }

        private Aran.Queries.Common.DbProviderControl2 DbProControl
        {
            get
            {
                if (ui_dbProChoiceServerRB.Checked)
                    return ui_dbProControlServer;

                return ui_dbProControlLocal;
            }
        }

        private void DbProChoice_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            rb.Font = new Font(rb.Font, rb.Checked ? FontStyle.Bold : FontStyle.Regular);

            ui_dbProControlServer.Visible = ui_dbProChoiceServerRB.Checked;
            ui_dbProControlLocal.Visible = ui_dbProChoiceLocalRB.Checked;
        }
    }

    public delegate void OpenFileEventHandler (object sender, OpenFileEventArgs e);

    public class OpenFileEventArgs : EventArgs
    {
        public OpenFileEventArgs(string fileNameOrPassword, bool isFileName, bool isSavePassword)
        {
            if (isFileName)
                FileName = fileNameOrPassword;
            else
                Password = fileNameOrPassword;

            IsSavePassword = isSavePassword;
        }

        public string FileName { get; private set; }

        public string ErrorMessage { get; set; }

        public bool ShowLogin { get; set; }

        public string UserName { get; set; }

        public string Password { get; private set; }

        public bool IsSavePassword { get; private set; }
    }
}
