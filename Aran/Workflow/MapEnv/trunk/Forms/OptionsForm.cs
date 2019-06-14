using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.CatalogUI;
using Aran.Aim.Data;
using Aran;
using Aran.Package;
using Aran.AranEnvironment;
using Aran.Controls;
using System.Xml;
using System.Threading;

namespace MapEnv
{
    public partial class OptionsForm : Form
    {
        private ISpatialReference _spatialReference;
        private List<ISettingsPage> _loadedSettingsPageList;
        private bool _isPluginPageOpened;


        public OptionsForm()
        {
            InitializeComponent();

            _loadedSettingsPageList = new List<ISettingsPage>();

            ui_featInfoWinMode.Items.Add(FeatureInfoMode.Popup);
            ui_featInfoWinMode.Items.Add(FeatureInfoMode.Window);
        }


        public DbProviderControl DbProvider
        {
            get { return ui_dbProviderControl; }
        }

        public TabControl TabControl
        {
            get { return ui_mainTabControl; }
        }

        public DialogResult ShowDialog(ISpatialReference spatRef, Connection conn)
        {
            _spatialReference = spatRef;
            if (conn != null)
                ui_dbProviderControl.SetValue(conn);
            else
                ui_dbProviderControl.SetValue(Globals.Settings.DbConnection);

            return ShowDialog();
        }

        public Connection GetConnection()
        {
            return ui_dbProviderControl.GetValue();
        }

        public ISpatialReference SpatialReference
        {
            get
            {
                return _spatialReference;
            }
        }

        public bool IsDbConnectionChanged
        {
            get
            {
                return ui_dbProviderControl.IsValueChanged;
            }
        }
        
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            if (_spatialReference == null) {
                _spatialReference = Globals.CreateWGS84SR();
            }

            ui_spatialRefTB.Text = _spatialReference.Name;

            foreach (var item in Globals.SettingsPageList) {
                if (!item.IsEnabled)
                    continue;

                ISettingsPage page = item.Page;
                if (ui_mainTabControl.TabPages.ContainsKey(page.Title))
                    continue;
                TabPage tabPage = new TabPage();
                tabPage.Name = page.Title;
                tabPage.Text = page.Title;
                tabPage.Tag = page;
                page.Page.Dock = DockStyle.Fill;
                page.Page.Visible = true;
                tabPage.Controls.Add(page.Page);

                ui_mainTabControl.TabPages.Add(tabPage);
            }

            ui_coordFormatCB.SelectedIndex = (Globals.Settings.CoordinateFormat == Aran.Aim.InputForm.CoordinateFormat.DMS ? 0 : 1);
            ui_coordFormatRoundNud.Value = Globals.Settings.CoordinateFormatRound;
            ui_effectiveDateTimePicker.Value = Globals.MainForm.DbProvider.DefaultEffectiveDate;
            ui_featInfoWinMode.SelectedItem = Globals.Settings.FeatureInfoMode;

            var aixmMDCommonExt = new CommonExtData();
            (Globals.MainForm as IAranEnvironment).GetExtData("AixmMetadata", aixmMDCommonExt);

            if (aixmMDCommonExt.TryGetValue("IsUseWebApi", out string tempStr) && bool.TryParse(tempStr, out bool temp))
            {
                ui_useAixmMDWebApi.Checked = temp;
                ui_aixmMetadataWebApiWarningLabel.Visible = !temp;
            }
            else
            {
#if UseWebApi
                ui_useAixmMDWebApi.Checked = true;
                ui_aixmMetadataWebApiWarningLabel.Visible = false;
#else
                ui_useAixmMDWebApi.Checked = false;
                ui_aixmMetadataWebApiWarningLabel.Visible = true;
#endif
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            foreach (ISettingsPage sp in _loadedSettingsPageList) {
                bool saveResult = sp.OnSave();
                if (!saveResult)
                    return;
            }

            Globals.Settings.CoordinateFormat = (ui_coordFormatCB.SelectedIndex == 0 ?
                Aran.Aim.InputForm.CoordinateFormat.DMS : Aran.Aim.InputForm.CoordinateFormat.DD);
            Globals.Settings.CoordinateFormatRound = (int)ui_coordFormatRoundNud.Value;

            bool effectiveDateChanged = Globals.MainForm.DbProvider.DefaultEffectiveDate != ui_effectiveDateTimePicker.Value;
            Globals.MainForm.DbProvider.DefaultEffectiveDate = ui_effectiveDateTimePicker.Value;
            Globals.Settings.FeatureInfoMode = (FeatureInfoMode)ui_featInfoWinMode.SelectedItem;

            var aixmMDCommonExt = new CommonExtData {{"IsUseWebApi", ui_useAixmMDWebApi.Checked.ToString()}};

            (Globals.MainForm as IAranEnvironment).PutExtData("AixmMetadata", aixmMDCommonExt);

            if (_isPluginPageOpened)
                SaveAndLoadPlugins();

            DialogResult = DialogResult.OK;

            if (effectiveDateChanged) {
                var thread = new Thread(DoLayerUpdate);
                thread.Start();
            }
        }

        private void DoLayerUpdate()
        {
            Globals.MainForm.DoEffectiveDateChanged();
        }

        private void SelSpatialRefButton_Click(object sender, EventArgs e)
        {
            var SpRefDialog = new SpatialReferenceDialog() as ISpatialReferenceDialog2;

            ISpatialReference spatRef = SpRefDialog.DoModalEdit(SpatialReference,
                true, true, true, false, false, false, false, 0);

            if (spatRef != null) {
                _spatialReference = spatRef;
                ui_spatialRefTB.Text = SpatialReference.Name;
            }
        }

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tabPage = ui_mainTabControl.SelectedTab;

            if (tabPage == null)
                return;

            if (tabPage.Tag is ISettingsPage) {
                var sp = tabPage.Tag as ISettingsPage;
                if (!_loadedSettingsPageList.Contains(sp)) {
                    sp.OnLoad();
                    _loadedSettingsPageList.Add(sp);
                }
            }
            else if (tabPage == ui_pluginsTabPage && !_isPluginPageOpened) {
                _isPluginPageOpened = true;

                foreach (var item in Globals.AranPluginList) {
                    if (!item.Plugin.IsSystemPlugin)
                        ui_pluginsCLB.Items.Add(new LBItem (item), item.IsEnabled);
                }
            }
        }

        private void SaveAndLoadPlugins()
        {
            Globals.AranPluginList.ForEach(eap => eap.IsEnabled = false);
            foreach (LBItem item in ui_pluginsCLB.CheckedItems)
                item.EnvPlugin.IsEnabled = true;

            var pluginErrors = new List<string>();
            
            foreach (var item in Globals.AranPluginList) {
                if (item.IsEnabled) {
                    try {
                        Globals.StartPlugin(item.Plugin);
                    }
                    catch (Exception ex) {
                        pluginErrors.Add(string.Format("Error in {0} Plugin\rnDeltails: {1}", item.Plugin.Name, ex.Message));
                    }
                }
            }

            if (pluginErrors.Count > 0)
                Globals.ShowError(string.Join<string>("\n", pluginErrors));

            Globals.MapData.AranPlugins.Clear();
            var ids = from eap in Globals.AranPluginList where eap.IsEnabled select eap.Plugin.Id;
            Globals.MapData.AranPlugins.AddRange(ids);
        }

        private void ClearUserNamePassword_Click(object sender, EventArgs e)
        {
            Globals.Settings.SetUserNamePassword(null, null);
        }

        private void UseWebApi_CheckedChanged(object sender, EventArgs e)
        {
            ui_aixmMetadataWebApiWarningLabel.Visible = !(sender as CheckBox).Checked;
        }

        private class LBItem
        {
            public LBItem(EnvAranPlugin envPlugin)
            {
                EnvPlugin = envPlugin;
            }

            public EnvAranPlugin EnvPlugin { get; private set; }

            public override string ToString()
            {
                return EnvPlugin.Plugin.Name;
            }
        }

        
    }
}