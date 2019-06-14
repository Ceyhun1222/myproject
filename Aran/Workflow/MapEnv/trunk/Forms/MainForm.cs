using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Metadata.UI;
using Aran.AranEnvironment;
using Aran.Controls;
using Aran.Package;
using Aran.Queries.Common;
using Aran.Queries.Viewer;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS;
using MapEnv.Forms;
using MapEnv.Toc;
using System.Collections.ObjectModel;
using System.Data;
using CommonUtils;
using Path = System.IO.Path;

namespace MapEnv
{
    public partial class MainForm : Form, IAranEnvironment, IAranUI, IFeatureViewer
    {
        #region Declaration

        private DbProvider _dbProvider;
        private TabDocumentForm _tabDocForm;
        private string _ampFileName;
        private AimInfoTool _aimInfoTool;
        private IToolbarItem _mapAddDataToolbarItem;
        private Dictionary<AranTool, MapToolItem> _dictMapToolItem;
        private IAranGraphics _aranGraphics;
        private IAranLayoutViewGraphics _aranLayoutGraphics;
        private ISpatialReference _wgs84SpatRef;
        private ITool _previousTool;
        private bool _loaded;
        private Aran.Aim.InputForm.MainForm _inputForm;
        private Dictionary<ToolStripButton, int> _leftTSBWidthDict;
        private ToolStripButton _currentLeftButton;
        private Dictionary<Control, ILeftWindow> _controlLeftWindowDict;
        private ToolStripButton _focusChangedOnThisButton;
        private string _tempDirName;
        private string _tempProFileName;
        private int _layerUpdateThreadCount;
        private bool _changeEffectiveDate;
        private EventHandler LayerUpdateThreadEnded;
        private LayoutView.LayoutViewForm _layoutView;
        private CommonData _commonData;
        private Dictionary<AranPlugin, ToolStrip> _aranPluginToolStripDict;


        #endregion


        public MainForm()
        {
            //*** it must be desktop license, don't change it!
            RuntimeManager.Bind(ProductCode.Desktop);
            AoInitialize ao = new AoInitialize();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);

            InitializeComponent();

            #region Adding current executed path to System Environment path

            var oldPath = Environment.GetEnvironmentVariable("Path");
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string executedPath = Path.GetDirectoryName(path);
            Environment.SetEnvironmentVariable("Path", executedPath + ";" + oldPath);

            #endregion

            var tmp = UIMetadata.Instance.ClassInfoList;

            Globals.MainForm = this;
            Globals.MapData = new MapEnvData();

            _leftTSBWidthDict = new Dictionary<ToolStripButton, int>();
            _controlLeftWindowDict = new Dictionary<Control, ILeftWindow>();
            _ampFileName = "";
            _currentLeftButton = ui_tocTSB;
            _commonData = new CommonData();

            _aimInfoTool = new AimInfoTool();

            CustomInitComponent();

            _aranPluginToolStripDict = new Dictionary<AranPlugin, ToolStrip>();
            _aranGraphics = new AranGraphics(ui_esriMapControl);
            _aranLayoutGraphics = new AranLayoutViewGraphics();
            _wgs84SpatRef = Globals.CreateWGS84SR();
            _dictMapToolItem = new Dictionary<AranTool, MapToolItem>();

            ui_exportToGDBTSMI.Image = Properties.Resources.layer_export_32;
            ui_openTSMI.Visible = false;

            _tabDocForm = new TabDocumentForm();
            _tabDocForm.Owner = this;

            _tempDirName = Globals.TempDir + "\\ProFiles";
            _tempProFileName = "TempAmp";

            InitGlobals();

            tsItemManageAerodrome.Visible = false;
            toolStripSeparator1.Visible = false;
#if (KAZ)
			ui_exportMxdTSMI.Enabled = false;
			tsItemManageAerodrome.Visible = true;
			toolStripSeparator1.Visible = true;
#endif
            ui_testTSMI.Visible = false;
#if TEST
			ui_testTSMI.Visible = true;
#endif

            foreach (ToolStripItem item in ui_mainStatusStrip.Items)
                item.Visible = false;

            Text = Globals.AppText;

#if COMSOFTDB
            Text += "  (with CADASDB)";
#endif
        }


        public DbProvider DbProvider
        {
            get
            {
                if (_dbProvider == null)
                {
                    _dbProvider = CreateDBProvider();
                    _dbProvider.UseCacheChanged += DbProvider_UseCacheChanged;
                    _dbProvider.SetEffectiveDateChangedEventHandler(OnDbProvider_EffectiveDateChanged);

                    ui_userManagerTSMI.Visible = (_dbProvider.UserManagement != null);
                    ui_cadasMenuItem.Visible = (_dbProvider.ProviderType == DbProviderType.ComSoft);
                }
                return _dbProvider;
            }
        }

        public void SetLocalDbProvider()
        {
            var localDbPro = DbProviderFactory.Create("Aran.Aim.Data.LocalDbProvider");
            _dbProvider = localDbPro;
        }

        public void RefreshLayer(ILayer layer)
        {
            ui_esriMapControl.Refresh(esriViewDrawPhase.esriViewGeography, layer, null);
        }

        public void UpdateLayer(ILayer layer)
        {
            var b = false;

            if (layer is AimFeatureLayer)
            {
                var aimFL = layer as AimFeatureLayer;
                aimFL.BeginUpdate();

                if (aimFL.IsComplex)
                {
                    aimFL.RemoveSubLayersAndFiles();

                    aimFL.LayerInfoList.Clear();
                    aimFL.ComplexTable = AimFLComplexLoader.LoadQueryInfo(aimFL.BaseQueryInfo, aimFL.LayerInfoList);

                    foreach (var item in aimFL.LayerInfoList)
                        Map.AddLayer(item.Layer);

                    aimFL.EndUpdate();
                    ui_newTocControl.ReOrderLayers();
                }
                else
                {
                    b = true;
                }
            }

            if (b)
            {
                Application.DoEvents();

                ParameterizedThreadStart pts = (arg) =>
                {
                    var aimFL = arg as AimFeatureLayer;

                    if (!aimFL.IsComplex)
                    {
                        var features = Globals.LoadFeatures(aimFL.FeatureType, aimFL.AimFilter);
                        Invoke(new LayerUpdated(OnSimpleShapefileLayerUpdated), aimFL, features, false);
                        Invoke(new Action(delegate () { LayerUpdateThreadCount--; }));
                    }
                };

                var thread = new Thread(pts);
                thread.Start(layer);
            }
        }

        public void AddAimSimpleShapefileLayer(AimLayer aimLayer, bool asFirst, bool zoomToLayer)
        {
            var aimFL = aimLayer.Layer as AimFeatureLayer;

            foreach (var item in aimFL.LayerInfoList)
                Map.AddLayer(item.Layer);

            if (asFirst)
                ui_newTocControl.AddLayerAsFirst(aimLayer);
            else
                ui_newTocControl.AddLayer(aimLayer);

            ui_newTocControl.ReOrderLayers();

            if (zoomToLayer)
                Zoom2Layer(aimLayer.Layer);
        }

        public bool UseWebApi
        {
            get
            {
                var aixmMDCommonExt = new CommonExtData();
                (this as IAranEnvironment).GetExtData("AixmMetadata", aixmMDCommonExt);

                if (aixmMDCommonExt.TryGetValue("IsUseWebApi", out string tempStr) && bool.TryParse(tempStr, out bool temp))
                    return temp;

#if UseWebApi
                return true;
#else
                return false;
#endif
            }
        }

        private void LoadFile()
        {
            var mapData = Globals.MapData;
            Globals.DbConnection = mapData.Connection;
            Globals.MainForm.Map.ClearLayers();



            if (_dbProvider.ProviderType == DbProviderType.ComSoft)
            {
                var strExtDate = new StringExtData();
                if (Globals.Environment.GetExtData("cache-db-path", strExtDate))
                {
                    var localDbFileName = strExtDate.Value;
                    var localDbPro = DbProviderFactory.Create("Aran.Aim.Data.LocalDbProvider");
                    localDbPro.Open(localDbFileName);
                    _dbProvider.CallSpecialMethod("SetCacheDbProvider", localDbPro);
                }
            }



            if (mapData.MapSpatialReference != null)
            {
                Globals.MainForm.Map.SpatialReference = mapData.MapSpatialReference;
                DoSpatialReferenceChanged();
            }

            var layerList = new List<ILayer>();
            var emptyLayerCount = 0;

            foreach (MapEnvLayerInfo layerInfo in mapData.LayerInfoList)
            {
                ILayer layer = null;

                if (layerInfo.LayerType == MapEnvLayerType.SimpleShapefile)
                {
                    layer = layerInfo.MyFeatureLayer;
                    if (layer.Visible && !layerInfo.MyFeatureLayer.IsComplex)
                        emptyLayerCount++;
                }
                else if (layerInfo.LayerType == MapEnvLayerType.Esri)
                    layer = layerInfo.PersistStream as ILayer;

                if (layer != null)
                    layerList.Add(layer);
            }

            LayerUpdateThreadCount = emptyLayerCount;
            LayerUpdateThreadEnded = MainForm_StartupLayerUpdateThreadEnded;

            ui_newTocControl.ClearLayers();

            for (int i = layerList.Count - 1; i >= 0; i--)
            {
                var layer = layerList[i];

                if (layer is AimFeatureLayer)
                {
                    var aimFL = layer as AimFeatureLayer;

                    if (aimFL.IsComplex)
                    {
                        if (aimFL.Visible)
                        {
                            var compTable = AimFLComplexLoader.LoadQueryInfo(aimFL.BaseQueryInfo, aimFL.LayerInfoList);
                            aimFL.ComplexTable = compTable;
                            aimFL.IsLoaded = true;
                        }
                    }
                    else
                    {
                        AimFLGlobal.FillMyFeatureLayer(aimFL);
                    }

                    var aimLayer = new Toc.AimLayer(aimFL);
                    Globals.MainForm.AddAimSimpleShapefileLayer(aimLayer, false, false);
                }
                else
                {
                    Globals.MainForm.Map.AddLayer(layer);
                    Globals.OnLayerAdded(layer);
                }
            }

            Globals.Settings.AddRecentProject(_ampFileName);

            foreach (var item in ui_newTocControl.AimLayers)
            {
                if (item.Layer is AimFeatureLayer)
                {
                    var aimFL = item.Layer as AimFeatureLayer;
                    aimFL.MyFLVisibleChanged += OnMyFeatureLayerVisibilityChanged;

                    if (item.Layer.Visible && !aimFL.IsComplex)
                        UpdateLayer(item.Layer);
                }
            }

            ui_newTocControl.ReOrderLayers();

            #region Load Enabled Plugins

            var pluginStartupErrors = new List<string>();

            foreach (var aranPlugin in Globals.AranPluginList)
            {
                if (aranPlugin.Plugin.IsSystemPlugin || mapData.AranPlugins.Contains(aranPlugin.Plugin.Id))
                {
                    try
                    {
                        aranPlugin.IsEnabled = true;
                        Globals.StartPlugin(aranPlugin.Plugin);

                        foreach (var sp in Globals.SettingsPageList)
                        {
                            if (sp.BaseOnPlugins.Contains(aranPlugin.Plugin.Id))
                            {
                                sp.IsEnabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        pluginStartupErrors.Add(string.Format(
                            "Error on Starting up {0} plugin.\nDetails: {1}",
                            aranPlugin.Plugin.Name, ex.Message));
                    }
                }
            }

            if (pluginStartupErrors.Count > 0)
                Globals.ShowError(string.Join<string>("\n", pluginStartupErrors));

            #endregion

            ShowConnectionInfo();

#if TEST
			Text = "TEST - " + Text;
#endif
        }

        private void MainForm_StartupLayerUpdateThreadEnded(object sender, EventArgs e)
        {
            LayerUpdateThreadEnded = null;

            (this as IAranEnvironment).Graphics.Extent = Globals.MapData.MapEnvelope;
            Globals.MapEdited = false;
            ui_esriMapControl.ActiveView.Refresh();
        }

        private void SaveAMP(string fileName)
        {
            var mapData = new MapEnvData();

            foreach (var item in Globals.AranPluginList)
            {
                if (item.IsEnabled && item.Plugin.Id != Guid.Empty)
                    mapData.AranPlugins.Add(item.Plugin.Id);
            }

            foreach (var aimLayer in ui_newTocControl.AimLayers)
                mapData.LayerInfoList.Add(new MapEnvLayerInfo(aimLayer.Layer));

            var map = Globals.MainForm.Map;

            mapData.LayerInfoList.Reverse();

            mapData.EffectiveDate = DbProvider.DefaultEffectiveDate;
            mapData.AiracDate = (this as IAranEnvironment).CurrentAiracDateTime;
            mapData.MapSpatialReference = Globals.MainForm.Map.SpatialReference;
            mapData.Connection = Globals.DbConnection;
            mapData.SelectedAirport = Globals.MapData.SelectedAirport;

            if (Globals.MapData != null)
            {
                foreach (string key in Globals.MapData.PluginDataDict.Keys)
                    mapData.PluginDataDict.Add(key, Globals.MapData.PluginDataDict[key]);
            }

            mapData.MapEnvelope.Assign((this as IAranEnvironment).Graphics.Extent);
            mapData.ThumbnailImageBytes = GetMapThumbnailImage();
            mapData.Save(fileName);

            Globals.MapEdited = false;
            Globals.Settings.AddRecentProject(fileName);
        }

        private void InitGlobals()
        {
            #region Selected Symbols

            IRgbColor rgb = new RgbColor();
            rgb.Red = 0;
            rgb.Green = 255;
            rgb.Blue = 255;

            var pointSymbol = new Aran.AranEnvironment.Symbols.PointSymbol(rgb.RGB, 12);
            pointSymbol.Style = Aran.AranEnvironment.Symbols.ePointStyle.smsCircle;
            _aranGraphics.SelectedPointSymbol = pointSymbol;

            var lineSymbol = new Aran.AranEnvironment.Symbols.LineSymbol();
            lineSymbol.Color = rgb.RGB;
            lineSymbol.Style = Aran.AranEnvironment.Symbols.eLineStyle.slsSolid;
            lineSymbol.Width = 2;
            _aranGraphics.SelectedLineSymbol = lineSymbol;

            var fillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol();
            fillSymbol.Color = rgb.RGB;
            fillSymbol.Style = Aran.AranEnvironment.Symbols.eFillStyle.sfsNull;
            fillSymbol.Outline = lineSymbol;
            _aranGraphics.SelectedFillSymbol = fillSymbol;

            #endregion

            DefaultStyleLoader.Instance.Load();
        }

        private void PreLoad()
        {
            if (_loaded)
                return;

            _loaded = true;

            Globals.Settings.Load(Application.LocalUserAppDataPath + "\\MapEnvSettings.config");

            //string s = "";
            //for (int i = 0; i < ui_esriToolbarControl.Count; i++)
            //{
            //    var tbi = ui_esriToolbarControl.GetItem(i);
            //    s += tbi.Command.Name + "\n";
            //}

            try
            {
                if (!Globals.PM())
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                Globals.ShowError("Error in parse code\n" + ex.Message);
                Application.Exit();
            }

            SetMapToolItems();
            //SetupToolBar();

            Size sz = ui_tocTreeImageList.ImageSize;
            Bitmap bitmap = new Bitmap(sz.Width, sz.Height);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.FillRectangle(new SolidBrush(ui_tocTreeImageList.TransparentColor), 0, 0, sz.Width, sz.Height);
            }
            ui_tocTreeImageList.Images.Add(bitmap);

            LoadPlugins();

            ui_applicationsToolStripMenuItem.Visible = (ui_applicationsToolStripMenuItem.DropDownItems.Count > 0);

            if (!System.IO.Directory.Exists(_tempDirName))
                System.IO.Directory.CreateDirectory(_tempDirName);

            Globals.MapDocument = new MapDocument();

            int n = 0;
            while (n++ < 20)
            {
                string fileName = string.Format("{0}\\{1}{2}.mxd", _tempDirName, _tempProFileName, n);

                try
                {
                    Globals.MapDocument.New(fileName);
                    break;
                }
                catch { }
            }

            if (n >= 20)
                Application.Exit();

            if (Globals.MapDocument.MapCount > 0)
            {
                ui_esriMapControl.Map = Globals.MapDocument.get_Map(0);
            }

            Globals.LayerAdded += Globals_LayerAdded;

            //ui_cmbBxScaleControl.SelectedIndex = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                PreLoad();

                WindowState = FormWindowState.Maximized;

                //var startThread = new Thread(MainForm_PostLoad);
                //startThread.Start();

                new Thread(() =>
                {
                    try
                    {
                        Invoke(new Action(() => OpenStartup()));
                    }
                    catch (Exception ex)
                    {
                        Globals.ShowError(ex.Message);
                    }
                }).Start();


                // Check PANDA/config.xml file located in same directory. If it is newer one then copy to %appData%
                var dir1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var fileName = System.IO.Path.Combine(dir1, "RISK", "PANDA", Config.FileName);
                string localFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "PANDA", Config.FileName);
                if (!File.Exists(localFileName))
                    return;
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
                File.Copy(localFileName, fileName, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Program.CloseSplash();
            }
        }

        private void OpenStartup()
        {
            var newProForm = new NewProjectForm();
            newProForm.OpenFileClicked += NewProForm_OpenFileClicked;
            newProForm.VerifyUserName += NewProForm_VerifyUserName;
            newProForm.NewProjectClicked += NewProForm_NewProjectClicked;

            var dlgRes = newProForm.ShowDialog();

            if (dlgRes != DialogResult.OK)
            {
                Globals.MapEdited = false;
                Close();
                return;
            }

            if (!newProForm.IsNewProject)
            {

                SetDbProAiracDate(Globals.MapData.AiracDate);
                LoadFile();
                return;
            }

            if (newProForm.IsNewProject)
            {

                var layerFeatList = new List<Aran.Aim.FeatureType>();

                #region Choice Plugins

                var pluginStartupErrors = new List<string>();

                Globals.AranPluginList.ForEach(p => p.IsEnabled = false);

                foreach (var selPlg in newProForm.AranPlugins)
                {
                    foreach (var item in Globals.AranPluginList)
                    {
                        if (item.Plugin == selPlg)
                        {
                            item.IsEnabled = true;

                            try
                            {
                                Globals.StartPlugin(item.Plugin);
                            }
                            catch (Exception ex)
                            {
                                pluginStartupErrors.Add(string.Format(
                                    "Error on Starting up {0} plugin.\nDetails: {1}",
                                    item.Plugin.Name, ex.Message));
                            }


                            try
                            {
                                foreach (var ft in item.Plugin.GetLayerFeatureTypes())
                                {
                                    if (!layerFeatList.Contains(ft))
                                        layerFeatList.Add(ft);
                                }
                            }
                            catch { }

                            break;
                        }
                    }
                }

                #endregion

                #region Choice Settings

                Globals.SettingsPageList.ForEach(eas => eas.IsEnabled = false);

                foreach (var stg in newProForm.SettingsPages)
                {
                    foreach (var item in Globals.SettingsPageList)
                    {
                        if (item.Page == stg)
                        {
                            item.IsEnabled = true;
                            break;
                        }
                    }
                }

                #endregion

                Globals.DbConnection = newProForm.GetConnection();
                Globals.MainForm.Map.SpatialReference = newProForm.GetSpatialReference();
                _ampFileName = newProForm.ProjectFileName;
                SetAppCaption();

                #region Add Layers
                AddLayers(layerFeatList);
                #endregion

                SaveAMP(newProForm.ProjectFileName);

                ShowConnectionInfo();

                if (pluginStartupErrors.Count > 0)
                    Globals.ShowError(string.Join<string>("\n", pluginStartupErrors));
            }
        }

        internal void SetDbProAiracDate(AiracDateTime adt)
        {
            _changeEffectiveDate = true;
            DbProvider.DefaultEffectiveDate = adt.Value;
            _changeEffectiveDate = false;
        }

        private void NewProForm_NewProjectClicked(object sender, EventArgs e)
        {
            Globals.MapData = new MapEnvData();
            _ampFileName = string.Empty;
        }

        private void NewProForm_OpenFileClicked(object sender, OpenFileEventArgs e)
        {
            if (!File.Exists(e.FileName))
            {
                e.ErrorMessage = "File not found!" + "\n" + e.FileName;
                return;
            }

            Globals.MapData = new MapEnvData();
            _ampFileName = e.FileName;
            SetAppCaption();

            try
            {
                Globals.MapData.Load(e.FileName);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "Could not load file. File not correct" +
                    "\nFile: " + e.FileName +
                    "\nError: " + ex.Message;
                return;
            }

            try
            {
                if (DbProvider.State == ConnectionState.Open)
                    DbProvider.Close();

                DbProvider.Open(Globals.MapData.Connection.GetConnectionString());
                e.UserName = Globals.MapData.Connection.UserName;
                e.ShowLogin = (DbProvider.ProviderType == DbProviderType.Aran || DbProvider.ProviderType == DbProviderType.ComSoft);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = ex.Message;
            }

        }

        private void NewProForm_VerifyUserName(object sender, OpenFileEventArgs e)
        {
            var mdbPassword = (DbProvider.ProviderType == DbProviderType.ComSoft ? e.Password : Aran.Aim.Data.DbUtility.GetMd5Hash(e.Password));

            var isOk = DbProvider.Login(e.UserName, mdbPassword);

            if (isOk)
            {
                if (e.IsSavePassword)
                    Globals.Settings.SetUserNamePassword(Globals.MapData.Connection.UserName, e.Password);
            }
            else
            {
                e.ErrorMessage = "Invalid UserName or Password";
                return;
            }
        }

        private void AddLayers(List<Aran.Aim.FeatureType> featTypeList)
        {
            featTypeList.Reverse();

            foreach (var featType in featTypeList)
            {

                var shapeInfos = DefaultStyleLoader.Instance.GetShapeInfo(featType);
                if (shapeInfos == null)
                    continue;

                string layerName = featType.ToString();
                Filter filter = null;

                var features = Aran.Aim.AimObjectFactory.CreateList((int)featType).Cast<Aran.Aim.Features.Feature>();

                var aimFL = new AimFeatureLayer();
                var isLayerVisible = _dbProvider.ProviderType != DbProviderType.ComSoft;
                aimFL.Visible = isLayerVisible;
                aimFL.FeatureType = featType;
                aimFL.AimFeatures = features;
                aimFL.AimFilter = filter;
                aimFL.Name = layerName;
                aimFL.ShapeInfoList.AddRange(shapeInfos);
                aimFL.IsLoaded = isLayerVisible;

                AimFLGlobal.FillMyFeatureLayer(aimFL);

                var aimLayer = new Toc.AimLayer(aimFL);
                Globals.MainForm.AddAimSimpleShapefileLayer(aimLayer, true, false);

                if (isLayerVisible)
                    UpdateLayer(aimFL);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AskForSave())
            {
                e.Cancel = true;
                return;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                UIMetadata.Instance.Save();

                Globals.Settings.DbConnection = Globals.DbConnection;
                Globals.Settings.Save();

                #region Clean all prev opened ESRI map project file.

                var tempProFiles = System.IO.Directory.GetFiles(_tempDirName);
                if (tempProFiles.Length > 10)
                {
                    try
                    {
                        System.IO.Directory.Delete(_tempDirName, true);
                    }
                    catch { }
                }

                var myFLList = new List<AimFeatureLayer>();

                foreach (var aimLayer in ui_newTocControl.AimLayers)
                {
                    if (aimLayer.Layer is AimFeatureLayer)
                        myFLList.Add(aimLayer.Layer as AimFeatureLayer);
                }

                foreach (var item in myFLList)
                    item.RemoveSubLayersAndFiles();


                #endregion
            }
            catch (Exception ex)
            {
                Globals.ShowError(ex.Message);
            }
        }

        private void CustomInitComponent()
        {


            ui_newTocControl.Dock = DockStyle.Fill;
            ui_tocTSB.Tag = ui_newTocControl;
            (ui_newTocControl as Control).Text = ui_leftWindowTitle.Title;
            //ui_esriToolbarControl.Visible = false;

            ui_esriMapToolbarControl.BorderStyle = esriControlsBorderStyle.esriNoBorder;
            ui_esriDrawToolbarControl.BorderStyle = esriControlsBorderStyle.esriNoBorder;

            ui_esriMapToolbarControl.BackColor = ui_esriMapToolbarControl.Parent.BackColor;
            ui_esriDrawToolbarControl.BackColor = ui_esriDrawToolbarControl.Parent.BackColor;
            ui_esriMapControl.Dock = DockStyle.Fill;

            ui_newTocControl.TocMenuClicked += TocControl_TocMenuClicked;
            ui_leftTableLayoutPanel.ColumnStyles[0].Width = 0;
            ui_leftTableLayoutPanel.RowStyles[0].Height = 0;

            var cmd = new CustomCommand();
            cmd.Caption = "ADM";
            cmd.Tooltip = "Data Manager";
            cmd.Image = Properties.Resources.form_24;
            cmd.Clicked += (o, e) => { uiEvents_inputFormsTSMI_Click(null, null); };
            ui_esriMapToolbarControl.AddItem(cmd, 0, -1, false, 10, esriCommandStyles.esriCommandStyleIconAndText);

            cmd = new CustomCommand();
            cmd.Caption = "Refresh";
            cmd.Tooltip = "Refresh all layers";
            cmd.Image = Properties.Resources.refresh_24;
            cmd.Clicked += (o, e) =>
            {
                RefreshAllAimLayers();
                ui_esriMapControl.Refresh(esriViewDrawPhase.esriViewAll, null, null);
            };
            ui_esriMapToolbarControl.AddItem(cmd, 0, 0, false, 0, esriCommandStyles.esriCommandStyleIconOnly);


            cmd = new CustomCommand();
            cmd.Caption = "Save";
            cmd.Tooltip = "Save Project";
            cmd.Image = Properties.Resources.save_24;
            cmd.Clicked += uiEvents_saveTSMI_Click;
            ui_esriMapToolbarControl.AddItem(cmd, 0, 0, false, 0, esriCommandStyles.esriCommandStyleIconOnly);


            _aimInfoTool.SetBitmap(Properties.Resources.info_16);
            _aimInfoTool.SetCaption("AIM Feature Info");
            ui_esriMapToolbarControl.AddItem(_aimInfoTool, 0, 9, false, 0, esriCommandStyles.esriCommandStyleIconOnly);

            for (int i = 0; i < ui_esriMapToolbarControl.Count; i++)
            {
                var item = ui_esriMapToolbarControl.GetItem(i);
                if ("ControlToolsGeneric_AddData".Equals(item.Command.Name))
                {
                    _mapAddDataToolbarItem = item;
                    ui_esriMapToolbarControl.Remove(i);
                    break;
                }
            }

            cmd = new CustomCommand();
            cmd.Caption = _mapAddDataToolbarItem.Command.Caption;
            cmd.Tooltip = _mapAddDataToolbarItem.Command.Tooltip;
            cmd.Message = _mapAddDataToolbarItem.Command.Message;
            cmd.Bitmap = _mapAddDataToolbarItem.Command.Bitmap;
            cmd.Clicked += (o, e) => { AddMapData(); };
            ui_esriMapToolbarControl.AddItem(cmd, 0, 13, false, 10, esriCommandStyles.esriCommandStyleIconAndText);
        }

        private void Globals_LayerAdded(LayerAddedEventArgs e)
        {
            ILayer layer = e.Layer;
            if (layer == null)
                return;

            ui_newTocControl.AddLayer(new AimLayer(layer));
        }

        private void TocControl_TocMenuClicked(object sender, Controls.TocItemMenuEventArg e)
        {
            switch (e.MenuType)
            {
                case MapEnv.Controls.TocItemMenuType.Open:
                    OpenTable(e.AimLayer.Layer);
                    break;
                case MapEnv.Controls.TocItemMenuType.ZoomToLayer:
                    Zoom2Layer(e.AimLayer.Layer);
                    break;
                case MapEnv.Controls.TocItemMenuType.Remove:

                    var dr = MessageBox.Show("Do you want to remove the layer?", "Aim Layer",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (dr != DialogResult.Yes)
                    {
                        e.Result = false;
                        return;
                    }

                    bool isRemoved = RemoveLayer(e.AimLayer.Layer);
                    e.Result = isRemoved;
                    break;
                case MapEnv.Controls.TocItemMenuType.Refresh:
                    UpdateLayer(e.AimLayer.Layer);
                    break;
                case MapEnv.Controls.TocItemMenuType.Property:
                    e.Result = LayerProperty(e.AimLayer.Layer);
                    break;
                case MapEnv.Controls.TocItemMenuType.ExportXml:
                    e.Result = ExportLayerToXML(e.AimLayer.Layer);
                    break;
            }
        }

        private bool ExportLayerToXML(ILayer layer)
        {
            if (!(layer is AimFeatureLayer))
                return false;

            var aimFL = layer as AimFeatureLayer;

            if (!aimFL.IsLoaded)
            {
                MessageBox.Show("Layer is not loaded yet! Please, load Layer by setting visible",
                    Globals.AppText, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var aeof = new AixmExportOptionsForm();

            if (aeof.ShowDialog() != DialogResult.OK)
                return false;

            List<Aran.Aim.Features.Feature> featList = null;

            if (aimFL.IsComplex)
                featList = aimFL.ComplexTable.GetAllFeatures();
            else
                featList = aimFL.AimFeatures.ToList();

            if (featList != null)
            {
                Aran.Aim.InputFormLib.InputFormController.WriteAllFeatureToXML(
                            featList, aeof.FileName, aeof.IsWriteExtensions,
                            aeof.Write3DIfExists, DbProvider.DefaultEffectiveDate, aeof.SrsType);
            }

            MessageBox.Show("Successfully Exported!", Globals.AppText);

            return false;
        }

        #region Plugin

        private void LoadPlugins()
        {

            string fileName = "Plugins.txt";
            if (!File.Exists(fileName))
            {
                fileName = Application.StartupPath + "\\Plugins.txt";
                if (!File.Exists(fileName))
                    return;
            }


            var sr = File.OpenText(fileName);
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] sa = line.Split("\t ;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (sa.Length >= 2)
                {
                    string pluginType = sa[0]; //esri: EsriPlugin; aran: AranPlugin

                    if (pluginType.StartsWith("#"))
                        continue;

                    string dllName = sa[1].Trim();

                    if (dllName.StartsWith("<cur>"))
                        dllName = dllName.Replace("<cur>", Application.StartupPath);

                    try
                    {
                        LoadAranPluginModules(dllName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(string.Format(
                            "While loading plugin: \"{0}\"\nException: ", dllName, exception.Message));
                    }
                }
            }
        }

        private bool LoadAranPluginModules(string assemblyFileName)
        {
            if (!File.Exists(assemblyFileName))
            {
                return false;
            }

            Assembly assembly = Assembly.LoadFile(assemblyFileName);
            Type[] typeArr = assembly.GetExportedTypes();

            foreach (Type type in typeArr)
            {

                if (typeof(AranPlugin).IsAssignableFrom(type))
                {
                    var aranPlugin = Activator.CreateInstance(type) as AranPlugin;

                    try
                    {
                        Globals.AranPluginList.Add(new EnvAranPlugin(aranPlugin));
                    }
                    catch (Exception ex)
                    {
                        string dllName = System.IO.Path.GetFileName(assemblyFileName);
                        Globals.ShowError("Error in Assembly '" + dllName + "'\n" + ex.Message);
                        return false;
                    }
                }

                if (typeof(ISettingsPlugin).IsAssignableFrom(type))
                {
                    var settingsPlugin = Activator.CreateInstance(type) as ISettingsPlugin;

                    try
                    {
                        settingsPlugin.Startup(this);
                    }
                    catch (Exception ex)
                    {
                        string dllName = System.IO.Path.GetFileName(assemblyFileName);
                        Globals.ShowError("Error in Assembly '" + dllName + "'\n" + ex.Message);
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        private void uiEvents_showStatusBarTSB_Click(object sender, EventArgs e)
        {
            ui_mainStatusStrip.Visible = ui_showStatusBarTSB.Checked;
        }

        private void uiEvents_exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uiEvents_mapTSB_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            if (tsmi == null)
                return;

            if (tsmi.CheckOnClick && tsmi.CheckState == CheckState.Unchecked)
            {
                tsmi.CheckState = CheckState.Checked;
                return;
            }

            IToolbarItem tbItem = tsmi.Tag as IToolbarItem;
            int layerCount = Globals.MainForm.Map.LayerCount;
            var map = Globals.MainForm.Map;
            List<ILayer> layerList = null;

            #region Get Prev Layers

            bool isAddDataButton = (tbItem.Command.Name == "ControlToolsGeneric_AddData");

            if (isAddDataButton)
            {
                layerList = new List<ILayer>();
                for (int i = 0; i < map.LayerCount; i++)
                    layerList.Add(map.Layer[i]);
            }

            #endregion


            if (tsmi.CheckOnClick && tbItem.Command is ITool)
            {
                foreach (ToolStripItem item in tsmi.Owner.Items)
                {
                    ToolStripMenuItem tmp;
                    if (!item.Equals(tsmi) && ((tmp = item as ToolStripMenuItem) != null && tmp.CheckOnClick))
                        tmp.Checked = false;
                }

                ui_infoTSMI.Checked = false;
                ui_esriMapToolbarControl.CurrentTool = tbItem.Command as ITool;
            }
            else
            {
                tbItem.Command.OnClick();
            }

            if (isAddDataButton)
            {
                if (layerCount < Globals.MainForm.Map.LayerCount)
                {
                    for (int i = 0; i < map.LayerCount; i++)
                    {
                        if (!layerList.Contains(map.Layer[i]))
                        {
                            Globals.OnLayerAdded(map.Layer[i]);
                        }
                    }
                }
            }
        }

        private void uiEvents_exportMxdTSMI_Click(object sender, EventArgs e)
        {
            //IToolbarItem tbItem = GetToolItem ("ControlToolsGeneric_SaveAsDocCommand");
            //tbItem.Command.OnClick ();
        }

        private void uiEvents_esriMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            var esriPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint;
            esriPoint.PutCoords(e.mapX, e.mapY);

            var prjPoint = new ESRI.ArcGIS.Geometry.Point() as IPoint;

            if (ui_esriMapControl.Map.SpatialReference != null)
            {
                esriPoint.SpatialReference = ui_esriMapControl.Map.SpatialReference;
                esriPoint.Project(_wgs84SpatRef);

                if (ui_esriMapControl.Map.SpatialReference is IProjectedCoordinateSystem)
                    prjPoint.PutCoords(e.mapX, e.mapY);
            }

            if (!esriPoint.IsEmpty)
            {
                int round = Globals.Settings.CoordinateFormatRound;
                string s;

                if (Globals.Settings.CoordinateFormat == Aran.Aim.InputForm.CoordinateFormat.DMS)
                {
                    s = Globals.DD2DMS(esriPoint.X, true, round) +
                        "   " +
                        Globals.DD2DMS(esriPoint.Y, false, round);
                }
                else
                {
                    string rt = new string('#', round);

                    s = string.Format("{0}  {1}  Decimal Degrees",
                        esriPoint.X.ToString("#." + rt),
                        esriPoint.Y.ToString("#." + rt));
                }

                if (!prjPoint.IsEmpty)
                    s += string.Format("  ({0:#.##}  {1:#.##})", prjPoint.X, prjPoint.Y);

                ui_posStatusLabel.Text = s;
            }
        }

        private bool LayerProperty(ILayer layer)
        {
            if (layer is AimFeatureLayer)
            {
                var aimFL = layer as AimFeatureLayer;

                if (aimFL.IsComplex)
                {
                    var complexLayBuild = new ComplexLayer.ComplexLayerBuilderForm();
                    complexLayBuild.SetQueryInfo(aimFL.BaseQueryInfo);
                    if (complexLayBuild.ShowDialog(this) == DialogResult.OK)
                    {
                        var qi = complexLayBuild.GetQueryInfo();
                        aimFL.Name = qi.Name;
                        aimFL.SetQueryInfo(qi);
                        UpdateLayer(aimFL);
                        return true;
                    }
                }
                else
                {
                    var featLayerPropForm = new FeatureLayerPropertyForm();

                    var sc = featLayerPropForm.StyleControl;
                    var fc = featLayerPropForm.FilterControl;

                    featLayerPropForm.LayerName = aimFL.Name;

                    sc.SetShapeInfos(
                        aimFL.FeatureType,
                        aimFL.ShapeInfoList);

                    fc.SetFilter(aimFL.FeatureType, aimFL.AimFilter);

                    if (featLayerPropForm.ShowDialog() == DialogResult.OK)
                    {
                        aimFL.ShapeInfoList.Clear();
                        aimFL.ShapeInfoList.AddRange(sc.GetShapeInfos());
                        aimFL.AimFilter = fc.GetFilter();
                        aimFL.Name = featLayerPropForm.LayerName;

                        UpdateLayer(aimFL);
                        return true;
                    }
                }
            }
            else if (layer is IFeatureLayer)
            {

                ISymbol outSymbol;

                IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)layer;
                ISimpleRenderer simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;
                ISymbol inSymbol = simpleRenderer.Symbol;

                var shapeType = (layer as IFeatureLayer).FeatureClass.ShapeType;
                //ISymbol inSymbol = Globals.CreateDefaultSymbol(shapeType);

                if (Globals.SelectSymbol(inSymbol, out outSymbol))
                {
                    simpleRenderer.Symbol = outSymbol;
                    RefreshLayer(layer);
                }

                //PropertyForm propForm = new PropertyForm();
                //propForm.ShowDialog(this);
                return true;
            }
            else if (layer is IRasterLayer)
            {
                var rl = layer as IRasterLayer;
                var rpf = new Forms.RasterPropertyForm();
                rpf.ReaterFileName = rl.FilePath;
                rpf.ShowDialog(this);
                return true;
            }
            return false;
        }

        private void uiEvents_aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void AddAimFeatureLayer_Click(object sender, EventArgs e)
        {
            AimFLGlobal.AddAimSimpleLayer(DbProvider.CurrentUser);
        }

        private void uiEvents_saveAsTSMI_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Globals.OpenFileFilters;
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            SaveAMP(sfd.FileName);
            _ampFileName = sfd.FileName;
            SetAppCaption();
        }

        private void uiEvents_saveTSMI_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_ampFileName))
            {
                uiEvents_saveAsTSMI_Click(sender, e);
                return;
            }

            SaveAMP(_ampFileName);
        }

        private void uiEvents_openTSMI_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = Globals.OpenFileFilters;
            //if (ofd.ShowDialog() != DialogResult.OK)
            //    return;
            //LoadFile(ofd.FileName);
        }

        private void OnSimpleShapefileLayerUpdated(ILayer esriLayer, IEnumerable<Aran.Aim.Features.Feature> features, bool refreshMap)
        {
            var aimFL = esriLayer as AimFeatureLayer;
            aimFL.RemoveSubLayersAndFiles();
            aimFL.AimFeatures = features;

            AimFLGlobal.FillMyFeatureLayer(aimFL);

            foreach (var item in aimFL.LayerInfoList)
                Map.AddLayer(item.Layer);

            aimFL.EndUpdate();

            if (refreshMap)
            {
                ui_newTocControl.ReOrderLayers();
                ui_esriMapControl.Refresh(esriViewDrawPhase.esriViewGeography, esriLayer, null);
            }
        }

        private void uiEvents_optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var optionForm = new OptionsForm();

            if (sender == null)
            {
                optionForm.TabControl.SelectedIndex = 2;
            }

            _changeEffectiveDate = true;

            var dlgRes = optionForm.ShowDialog(ui_esriMapControl.SpatialReference, Globals.DbConnection);

            _changeEffectiveDate = false;

            if (dlgRes != DialogResult.OK)
                return;

            Globals.MapEdited = true;

            if (ui_esriMapControl.SpatialReference == null ||
                ui_esriMapControl.SpatialReference != optionForm.SpatialReference)
            {
                ui_esriMapControl.SpatialReference = optionForm.SpatialReference;
                DoSpatialReferenceChanged();
                ui_esriMapControl.Refresh();
            }

            RefreshAllAimLayers();
            ui_esriMapControl.Refresh();
        }

        private void DefaultFeatureTypeStyles_Click(object sender, EventArgs e)
        {
            var dfs = new DefaultFeatureStyleForm();
            dfs.ShowDialog(this);
        }

        private bool RemoveLayer(ILayer layer)
        {
            if (layer is AimFeatureLayer)
            {
                var mfl = layer as AimFeatureLayer;
                mfl.RemoveSubLayersAndFiles();
                //Globals.MainForm.Map.DeleteLayer (layer);
                ui_newTocControl.RemoveEsriLayer(layer);
                mfl.Dispose();
                return true;
            }
            else
            {
                for (int i = 0; i < ui_esriMapControl.LayerCount; i++)
                {
                    if (layer == ui_esriMapControl.get_Layer(i))
                    {
                        ui_esriMapControl.DeleteLayer(i);
                        ui_newTocControl.RemoveEsriLayer(layer);
                        return true;
                    }
                }
            }

            return false;
        }

        private void uiEvents_infoTSMI_Click(object sender, EventArgs e)
        {
            if (ui_infoTSMI.Checked)
            {
                foreach (ToolStripItem item in ui_mapToolStripMenuItem.DropDownItems)
                {
                    ToolStripMenuItem tmp;
                    if ((tmp = item as ToolStripMenuItem) != null && tmp.CheckOnClick)
                        tmp.Checked = false;
                }
                ui_esriMapControl.CurrentTool = _aimInfoTool;
            }
            else
            {
                ui_esriMapControl.CurrentTool = null;
            }
        }

        private void uiEvents_infoTSMI_CheckedChanged(object sender, EventArgs e)
        {
            //_infoTSB.Checked = ui_infoTSMI.Checked;
        }

        private void uiEvents_inputFormsTSMI_Click(object sender, EventArgs e)
        {
            if (_inputForm == null)
            {
                _inputForm = new Aran.Aim.InputForm.MainForm();
                _inputForm.SetDbProvider(Globals.DbConnection, Globals.Environment);
                _inputForm.ShowInTaskbar = true;
                _inputForm.StartPosition = FormStartPosition.CenterParent;
                _inputForm.FormClosing += InputForm_FormClosing;
                _inputForm.GetFeatureListHandler = Globals.LoadFeatures;
                _inputForm.Show(Globals.MainForm);
            }

            _inputForm.EffectiveDate = DbProvider.DefaultEffectiveDate;
            _inputForm.CoordinateFormat = Globals.Settings.CoordinateFormat;
            _inputForm.CoordinateFormatRound = Globals.Settings.CoordinateFormatRound;
            _inputForm.Visible = true;

            if (_inputForm.WindowState == FormWindowState.Minimized)
                _inputForm.WindowState = FormWindowState.Normal;
        }

        private void InputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                _inputForm.Hide();
            }
        }

        private void uiEvents_esriMapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (!ui_esriMapControl.Focused)
                ui_esriMapControl.Focus();
        }

        private void uiEvents_fileTSMI_Click(object sender, EventArgs e)
        {
            ui_saveAsTSMI.Enabled = !string.IsNullOrWhiteSpace(_ampFileName);
        }

        private void InfoTSB_CheckedChanged(object sender, EventArgs e)
        {
            var tsb = sender as ToolStripButton;
            ui_infoTSMI.Checked = tsb.Checked;

            uiEvents_infoTSMI_Click(ui_infoTSMI, e);
        }

        private void ZoomControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string text = (sender as ToolStripComboBox).Text;
                int index = text.IndexOf(':');
                double scale;
                if (index != -1)
                {
                    int left = Convert.ToInt32(text.Substring(0, index));
                    //NumberFormatInfo numberFormatInfo = new NumberFormatInfo ( );
                    //numberFormatInfo.NumberGroupSeparator = ".";
                    //numberFormatInfo.NumberDecimalSeparator = ",";
                    string tmp = text.Substring(index + 1);
                    int right = (int)Convert.ToDouble(text.Substring(index + 1));//, numberFormatInfo );
                    scale = right / left;
                }
                else
                {
                    scale = Convert.ToDouble(text);
                    (sender as ToolStripComboBox).Text = "1:" + text;
                }
                ui_esriMapControl.MapScale = scale;
                ui_esriMapControl.Refresh();
            }
        }

        private void ToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripButton tsb = sender as ToolStripButton;
            ToolStripMenuItem tsmi;
            if (tsb != null && (tsmi = tsb.Tag as ToolStripMenuItem) != null)
            {
                tsmi.CheckState = tsb.CheckState;
                uiEvents_mapTSB_Click(tsmi, e);
            }
        }

        private void SetMapToolItems()
        {
            foreach (ToolStripItem tsi in ui_mapToolStripMenuItem.DropDownItems)
            {
                if (tsi is ToolStripMenuItem && tsi.Tag is string)
                {
                    IToolbarItem mapTbi = GetToolItem(tsi.Tag.ToString());
                    tsi.Tag = mapTbi;

                    if (mapTbi == null)
                        continue;

                    if (mapTbi.Command.Bitmap != 0)
                    {
                        Bitmap bitmap = Bitmap.FromHbitmap(new IntPtr(mapTbi.Command.Bitmap));
                        tsi.Image = bitmap;

#if ARCGIS931
#else
                        tsi.ImageTransparentColor = Color.Black;
#endif

                        if (bitmap.Width > 0 && bitmap.Height > 0)
                            tsi.ImageTransparentColor = bitmap.GetPixel(0, 0);
                    }

                    tsi.ToolTipText = mapTbi.Command.Tooltip;
                    tsi.Click += new EventHandler(uiEvents_mapTSB_Click);
                }
            }
            Globals.ZoomToImage = ui_zoomInToolStripMenuItem.Image;
        }

        private IToolbarItem GetToolItem(string commandName)
        {
            for (int i = 0; i < ui_esriMapToolbarControl.Count; i++)
            {
                IToolbarItem tbi = ui_esriMapToolbarControl.GetItem(i);
                if (tbi.Command != null && tbi.Command.Name == commandName)
                    return tbi;
            }
            return null;
        }

        private void TablePageControl_ZoomToClicked(object sender, ZoomToEventArgs e)
        {
            if (e.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                ui_esriMapControl.CenterAt(e.Shape as IPoint);
            else
                ui_esriMapControl.ActiveView.Extent = e.Shape.Envelope;

            ui_esriMapControl.Refresh();
            Application.DoEvents();

            ISymbol symbol = Globals.CreateDefaultSymbol(e.Shape.GeometryType);

            ui_esriMapControl.FlashShape(e.Shape, 3, 300, symbol);
        }

        private ToolStripMenuItem MenuDefToToolStripMenuItem(IMenuDef menuDef, Assembly assembly)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem();
            tsmi.Tag = menuDef;
            tsmi.Text = menuDef.Caption;
            if (tsmi.Text.EndsWith("_NET"))
                tsmi.Text = tsmi.Text.Remove(tsmi.Text.Length - 4);

            for (int i = 0; i < menuDef.ItemCount; i++)
            {
                MyItemDef myItemDef = new MyItemDef();
                menuDef.GetItemInfo(i, myItemDef);
                Type type = GetAssemblyTypeByGUID(assembly, new Guid(myItemDef.ID));

                if (type != null)
                {
                    try
                    {
                        object commandObj = Activator.CreateInstance(type);
                        if (commandObj != null && commandObj is ICommand)
                        {
                            ICommand command = commandObj as ICommand;
                            command.OnCreate(this);

                            ToolStripMenuItem subTsmi = CommandToToolStripMenuItem(command as ICommand);
                            tsmi.DropDownItems.Add(subTsmi);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return tsmi;
        }

        private Type GetAssemblyTypeByGUID(Assembly assembly, Guid guid)
        {
            Type[] typeArr = assembly.GetExportedTypes();
            foreach (Type type in typeArr)
            {
                if (type.GUID == guid)
                    return type;
            }
            return null;
        }

        private ToolStripMenuItem CommandToToolStripMenuItem(ICommand command)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem();
            tsmi.Tag = command;
            tsmi.Text = command.Caption;

            if (tsmi.Text.EndsWith("_NET"))
                tsmi.Text = tsmi.Text.Remove(tsmi.Text.Length - 4);

            tsmi.ToolTipText = command.Tooltip;
            int bitmapHandle = command.Bitmap;
            if (bitmapHandle != 0)
            {
                Bitmap bitmap = Bitmap.FromHbitmap(new IntPtr(bitmapHandle));
                tsmi.Image = bitmap;
                if (bitmap.Height > 0 && bitmap.Width > 0)
                    tsmi.ImageTransparentColor = bitmap.GetPixel(0, 0);
            }
            tsmi.Click += new EventHandler(CommandTSB_Click);
            return tsmi;
        }

        private ToolStripButton CommandToToolStripButton(ICommand command)
        {
            ToolStripButton tsb = new ToolStripButton();
            tsb.Tag = command;
            tsb.Text = command.Caption;
            tsb.ToolTipText = command.Tooltip;
            int bitmapHandle = command.Bitmap;
            if (bitmapHandle != 0)
            {
                Bitmap bitmap = Bitmap.FromHbitmap(new IntPtr(bitmapHandle));
                tsb.Image = bitmap;
                if (bitmap.Height > 0 && bitmap.Width > 0)
                    tsb.ImageTransparentColor = bitmap.GetPixel(0, 0);
            }
            tsb.Click += new EventHandler(CommandTSB_Click);
            return tsb;
        }

        private void CommandTSB_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (sender as ToolStripItem);
            if (tsi == null || !(tsi.Tag is ICommand))
                return;

            ICommand command = tsi.Tag as ICommand;
            command.OnClick();
        }

        private void FeatureGridPageControl_FeatureOpened(object sender, FeatureEventArgs e)
        {
            FeatureViewerForm viewer = new FeatureViewerForm();
            viewer.DefaultEffectiveDate = DbProvider.DefaultEffectiveDate;
            viewer.HideBottomButtons();
            viewer.GetFeature += new GetFeatureHandler(Globals.FeatureViewer_GetFeature);
            viewer.GetFeatsListByDepend += new FeatureListByDependEventHandler(Globals.GetFeatureListByDepend);
            viewer.GoToFeatureClicked = new FeatureEventHandler(FeatureViewer_GoToFeatureClicked);
            viewer.SetFeature(e.Feature);

            TabDocument tabDoc = new TabDocument();
            tabDoc.WorkArea = viewer.MainContainer;
            tabDoc.Text = e.Feature.FeatureType + " [Edit]";

            _tabDocForm.AddPage(tabDoc);
        }

        #region IAranEnvironment Members

        public event EventHandler MapSpatialReferenceChanged;
        public event EventHandler EffectiveDateChanged;

        object IAranEnvironment.HookObject
        {
            get
            {
                return ui_esriMapControl.Object;
            }
        }

        void IAranEnvironment.SaveDocumentAs(string fileName, bool copy)
        {
            Globals.MapDocument.SaveAs(fileName);
        }

        string IAranEnvironment.DocumentFileName
        {
            get { return _ampFileName; }
        }

        IWin32Window IAranEnvironment.Win32Window
        {
            get
            {
                return this;
            }
        }

        object IAranEnvironment.DbProvider
        {
            get
            {
                return this.DbProvider;
            }
        }

        IAranUI IAranEnvironment.AranUI
        {
            get
            {
                return this;
            }
        }

        IAranGraphics IAranEnvironment.Graphics
        {
            get
            {
                return _aranGraphics;
            }
        }

        IAranLayoutViewGraphics IAranEnvironment.LayoutGraphics
        {
            get
            {
                return _aranLayoutGraphics;
            }
        }

        void IAranEnvironment.ShowLogs(IEnumerable<string> logs, bool clearPrev)
        {
            string s = "";
            foreach (string line in logs)
                s += line + "\r\n";

            MessageBox.Show(s, Globals.AppText + " - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void IAranEnvironment.ShowError(string message, bool clearPrev)
        {
            MessageBox.Show(message, Globals.AppText + " - Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        IFeatureViewer IAranEnvironment.GetViewer()
        {
            return this;
        }


        IScreenCapture IAranEnvironment.GetScreenCapture(string nm)
        {
            return ScreenCapture.Get(nm);
        }

        ILogger IAranEnvironment.GetLogger(string name)
        {
            return Logger.Get(name);
        }

        public AranPlugin GetPlugin(string name)
        {
            try
            {
                return Globals.AranPluginList.Find(eap => eap.Plugin.Name == name).Plugin;
            }
            catch
            {
                return null;
            }
        }

        public void RefreshAllAimLayers()
        {
            foreach (var aimLayer in ui_newTocControl.AimLayers)
            {

                if (!aimLayer.Layer.Visible)
                {
                    if (aimLayer.Layer is AimFeatureLayer)
                        (aimLayer.Layer as AimFeatureLayer).IsLoaded = false;
                    continue;
                }

                UpdateLayer(aimLayer.Layer);
            }
        }

        public List<object> GetAimLayers => ui_newTocControl.AimLayers.Cast<object>().ToList();

        public List<object> GetAllLayers
        {
            get
            {
                var result = new List<object>();
                for (var i = 0; i < ui_esriMapControl.LayerCount; i++)
                    result.Add(ui_esriMapControl.get_Layer(i));

                return result;
            }
        }


        public void ShowFeatureInfo(Aran.Aim.Features.Feature feature)
        {
            var fv = new Aran.Aim.FeatureInfo.ROFeatureViewer();
            var featList = new List<Aran.Aim.Features.Feature>();
            featList.Add(feature);
            fv.ShowFeaturesForm(featList);
        }

        internal void DoEffectiveDateChanged()
        {
            (this as IAranEnvironment).RefreshAllAimLayers();

            if (EffectiveDateChanged != null)
                EffectiveDateChanged(this, new EventArgs());
        }

        event EventHandler IAranEnvironment.MapSpatialReferenceChanged
        {
            add { }
            remove { }
        }

        event EventHandler IAranEnvironment.EffectiveDateChanged
        {
            add { }
            remove { }
        }

        AranPlugin IAranEnvironment.GetPlugin(string name)
        {
            return null;
        }

        void IAranEnvironment.RefreshAllAimLayers()
        {
        }

        void IAranEnvironment.ShowFeatureInfo(Aran.Aim.Features.Feature feature)
        {
        }

        Connection IAranEnvironment.ConnectionInfo
        {
            get { return Globals.DbConnection; }
        }

        AiracDateTime IAranEnvironment.CurrentAiracDateTime
        {
            get { return Aran.Controls.Airac.AiracCycle.CreateAiracDateTime(DbProvider.DefaultEffectiveDate); ; }
        }

        CommonData IAranEnvironment.CommonData
        {
            get
            {
                return _commonData;
            }
        }

        object IAranEnvironment.MapControl
        {
            get { return ui_esriMapControl; }
        }

        bool IAranEnvironment.PutExtData(string key, IPackable value)
        {
            if (Globals.MapData == null)
                return false;

            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryPackageWriter pw = new BinaryPackageWriter(memStream);
                value.Pack(pw);
                byte[] data = memStream.ToArray();

                if (Globals.MapData.PluginDataDict.ContainsKey(key))
                    Globals.MapData.PluginDataDict.Remove(key);

                Globals.MapData.PluginDataDict.Add(key, data);
            }

            return true;
        }

        bool IAranEnvironment.GetExtData(string key, IPackable value)
        {
            if (Globals.MapData == null)
                return false;

            byte[] data;
            if (!Globals.MapData.PluginDataDict.TryGetValue(key, out data))
                return false;

            using (BinaryPackageReader pr = new BinaryPackageReader(data))
                value.Unpack(pr);

            return true;
        }

        bool IAranEnvironment.HasExtKey(string key)
        {
            if (Globals.MapData == null)
                return false;

            return Globals.MapData.PluginDataDict.ContainsKey(key);
        }

        void IAranEnvironment.RemoveExtData(string key)
        {
            if (Globals.MapData == null)
                return;

            Globals.MapData.PluginDataDict.Remove(key);
        }

        T IAranEnvironment.ReadConfig<T>(string folder, string name, T defaultValue)
        {
            return CommonUtils.Config.ReadConfig<T>(folder, name, defaultValue);
        }

        void IAranEnvironment.WriteConfig(string folder, string name, object value)
        {
            CommonUtils.Config.WriteConfig(folder, name, value);
        }

        #endregion

        #region IAranUI

        public void AddMenuItem(AranMapMenu mapMenu, ToolStripMenuItem menuItem, string otherName = null)
        {
            switch (mapMenu)
            {
                case AranMapMenu.File:
                    ui_fileTSMI.DropDownItems.Insert(ui_fileTSMI.DropDownItems.Count - 2, menuItem);
                    ui_fileTSMI.DropDownItems[ui_fileTSMI.DropDownItems.Count - 1].Visible = true;
                    break;

                case AranMapMenu.View:
                    break;

                case AranMapMenu.Map:
                    break;

                case AranMapMenu.AIM:
                    break;

                case AranMapMenu.Applications:
                    ui_applicationsToolStripMenuItem.Visible = true;
                    ui_applicationsToolStripMenuItem.DropDownItems.Add(menuItem);
                    break;

                case AranMapMenu.Plugins:
                    ui_mainMenuStrip.Items.Insert(ui_mainMenuStrip.Items.Count - 2, menuItem);
                    break;

                case AranMapMenu.Tools:
                    break;

                case AranMapMenu.Help:
                    break;
                case AranMapMenu.Other:
                    if (otherName != null)
                    {
                        var tsiArr = ui_mainMenuStrip.Items.Find(otherName, false);
                        if (tsiArr != null && tsiArr.Length > 0)
                        {
                            var parentMI = tsiArr[0] as ToolStripMenuItem;
                            if (parentMI != null)
                                parentMI.DropDownItems.Add(menuItem);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public ToolStripMenuItem GetApplicationMenuItem { get { return ui_applicationsToolStripMenuItem; } }

        //public void AddApplicationMenuItem (ToolStripMenuItem menuItem)
        //{
        //    ui_applicationsToolStripMenuItem.Visible = true;			
        //    ui_applicationsToolStripMenuItem.DropDownItems.Add ( menuItem );
        //}

        //public void AddApplicationDropDownItem ( string menuItemText, ToolStripMenuItem menuItem )
        //{
        //    int menuItemIndex = GetMenuItemIndex ( ui_applicationsToolStripMenuItem, menuItemText );
        //    ( ui_applicationsToolStripMenuItem.DropDownItems [ menuItemIndex ] as ToolStripMenuItem ).DropDownItems.Add ( menuItem );
        //}

        public void AddMapTool(AranTool toolStripMenuItem)
        {
            MapToolItem mapToolItem = new MapToolItem(toolStripMenuItem);
            _dictMapToolItem.Add(toolStripMenuItem, mapToolItem);
        }

        public void SetCurrentTool(AranTool toolStripMenuItem)
        {
            if (toolStripMenuItem == null)
            {
                ui_esriMapToolbarControl.CurrentTool = _previousTool;
            }
            else
            {
                _previousTool = ui_esriMapToolbarControl.CurrentTool;
                ui_esriMapToolbarControl.CurrentTool = _dictMapToolItem[toolStripMenuItem];
            }
        }

        public void SetPanTool()
        {
            IToolbarItem toolBarItem = (ui_panToolStripMenuItem.Tag as IToolbarItem);
            ui_esriMapToolbarControl.CurrentTool = toolBarItem.Command as ITool;
        }

        public void AddSettingsPage(Guid[] baseOnPlugin, ISettingsPage page)
        {
            Globals.SettingsPageList.Add(new EnvAranSettingsPage()
            {
                BaseOnPlugins = baseOnPlugin,
                Page = page
            });
        }

        #endregion

        #region IFeatureViewer

        bool IFeatureViewer.SetFeature(List<Aran.Aim.Features.Feature> featureList, int rootIndex)
        {
            var ff = new FeatureFinder(featureList);
            var rootFeature = featureList[rootIndex];

            FeatureViewerForm viewer = new FeatureViewerForm();
            viewer.DefaultEffectiveDate = DbProvider.DefaultEffectiveDate;
            viewer.GetFeature += ff.GetFeature;
            viewer.GetFeatsListByDepend += new FeatureListByDependEventHandler(Globals.GetFeatureListByDepend);
            viewer.SetFeature(rootFeature);
            var dr = viewer.ShowDialog(this);



            return (dr == DialogResult.OK);
        }

        #endregion

        private int GetMenuItemIndex(ToolStripMenuItem menuItem, string menuItemText)
        {

            for (int i = 0; i < menuItem.DropDownItems.Count; i++)
            {
                if (menuItem.DropDownItems[i].Text == menuItemText)
                    return i;
            }
            throw new Exception("Not found Menu Item !");
        }

        private Dictionary<TabDocument, Control> GetOpenedTabDocument()
        {
            Dictionary<TabDocument, Control> dict = new Dictionary<TabDocument, Control>();

            foreach (TabDocument td in _tabDocForm.TDIControl.PageDocuments)
            {
                dict.Add(td, _tabDocForm);
            }

            return dict;
        }

        private void OpenTable(ILayer layer)
        {
            Dictionary<TabDocument, Control> dict = GetOpenedTabDocument();

            foreach (TabDocument tabDoc in dict.Keys)
            {
                if (tabDoc.WorkArea is IAttributePageControl)
                {
                    if (layer.Equals(((IAttributePageControl)tabDoc.WorkArea).Layer))
                    {
                        Control val = dict[tabDoc];
                        if (val is TDIControl)
                        {
                            ((TDIControl)val).CurrentTabDocument = tabDoc;
                        }
                        else if (val is TabDocumentForm)
                        {
                            TabDocumentForm tdForm = (TabDocumentForm)val;
                            tdForm.TDIControl.CurrentTabDocument = tabDoc;
                            tdForm.Visible = true;
                            if (tdForm.WindowState == FormWindowState.Minimized)
                                tdForm.WindowState = FormWindowState.Normal;
                            tdForm.Activate();
                        }
                        return;
                    }
                }
            }

            IAttributePageControl pageControl = null;

            if (layer is AimFeatureLayer)
            {
                var mfl = (layer as AimFeatureLayer);

                #region Update if not updated.

                if (!mfl.IsLoaded)
                {
                    if (mfl.IsComplex)
                    {
                        UpdateLayer(layer);
                    }
                    else
                    {
                        var features = Globals.LoadFeatures(mfl.FeatureType, mfl.AimFilter);
                        OnSimpleShapefileLayerUpdated(layer, features, true);
                    }

                    mfl.Visible = true;
                }

                #endregion

                if (mfl.IsComplex)
                {
                    OpenAimComplexLayer(mfl);
                    return;
                }
                else
                {
                    var fgpc = new FeatureGridPageControl();
                    fgpc.FeatureOpened += new FeatureEventHandler(FeatureGridPageControl_FeatureOpened);
                    pageControl = fgpc;
                }
            }
            else if (layer is IFeatureLayer)
            {
                pageControl = new AttributesPageControl();
            }
            else
                throw new Exception(layer.GetType() + " Layer not supported.");

            if (pageControl == null)
                return;

            pageControl.ZoomToClicked += new ZoomToEventHandler(TablePageControl_ZoomToClicked);
            pageControl.OpenLayer(layer);

            var tabDocument = new TabDocument();
            tabDocument.Text = pageControl.Text;
            tabDocument.WorkArea = pageControl as Control;

            _tabDocForm.AddPage(tabDocument);
            _tabDocForm.Visible = true;
        }

        private void OpenAimComplexLayer(AimFeatureLayer aimFL)
        {
            var clViewer = new ComplexLayer.ComplexLayerViewerControl();
            clViewer.SetComplexLayer(aimFL);

            AddLeftWindow(clViewer);
        }


        internal void DoSpatialReferenceChanged()
        {
            #region Set Central Meridian to CacheDbProvider
            double? centralMeridian = null;

            var sr = ui_esriMapControl.SpatialReference;
            if (sr is IProjectedCoordinateSystem)
            {
                var pcs = sr as IProjectedCoordinateSystem;
                centralMeridian = pcs.get_CentralMeridian(true);
            }

            if (_dbProvider.ProviderType == DbProviderType.ComSoft)
                _dbProvider.SetParameter("central-meridian", centralMeridian);

            #endregion

            if (MapSpatialReferenceChanged != null)
                MapSpatialReferenceChanged(this, null);
        }

        internal IMap Map
        {
            get
            {
                return ui_esriMapControl.Map;
            }
        }

        internal IActiveView ActiveView
        {
            get
            {
                return ui_esriMapControl.ActiveView;
            }
        }

        internal void FeatureViewer_GoToFeatureClicked(object sender, FeatureEventArgs e)
        {
            //bool tableOpened = false;

            //for (int i = 0; i < this.Map.LayerCount; i++)
            //{
            //    ILayer layer = this.Map.Layer [i];
            //    if (layer is AimFeatureLayer &&
            //        ((AimFeatureLayer) layer).AimTable.FeatureType == e.Feature.FeatureType)
            //    {
            //        OpenTable (layer);
            //        tableOpened = true;
            //        break;
            //    }
            //}

            //if (tableOpened)
            //{
            //    Dictionary<TabDocument, Control> dict = GetOpenedTabDocument ();

            //    foreach (TabDocument tabDoc in dict.Keys)
            //    {
            //        if (tabDoc.WorkArea is FeatureGridPageControl)
            //        {
            //            FeatureGridPageControl fgp = tabDoc.WorkArea as FeatureGridPageControl;
            //            AimFeatureLayer aimFeatLayer = fgp.Layer as AimFeatureLayer;
            //            if (aimFeatLayer != null && aimFeatLayer.AimTable.FeatureType == e.Feature.FeatureType)
            //            {
            //                fgp.SelectFeature (e.Feature.Identifier);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        internal System.Drawing.Point MapContToScreen(System.Drawing.Point p)
        {
            return ui_esriMapControl.PointToScreen(p);
        }

        internal void AddMapControlVisibleChanged(EventHandler mapVisibleChanged)
        {
            ui_esriMapControl.VisibleChanged += mapVisibleChanged;
        }

        internal ReadOnlyCollection<AimLayer> AimLayers
        {
            get { return ui_newTocControl.AimLayers; }
        }

#warning Fix it.
        //************************
        //*** Sonda melum oldu ki, bu funksionallıq Abuzer terefinden yazilib
        //*** Lakin orada bezi deyishiylik etmek lazimdir. 
        //*** Indi ise buna vaxt yoxdur :)
        //***           Anar. 25.10.2013 (Riga seferine hazirliq)
        //************************
        internal void SetCurrentTool2(ITool tool)
        {
            ui_esriMapControl.CurrentTool = tool;
        }


        private void EsriMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (sender != null)
                Globals.MapEdited = true;
        }

        private void ui_cmbBxScaleControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZoomControl_KeyUp(sender, new KeyEventArgs(Keys.Enter));
        }

        private void ui_cmbBxScaleControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = (char)e.KeyChar;
            if ((ch == ':' && (sender as ToolStripComboBox).Text.Contains(':')) ||
                (ch != (char)Keys.Back && ch != ':' && !char.IsDigit(ch)))
                e.Handled = true;
        }

        private void EsriMapControl_Resize(object sender, EventArgs e)
        {
            EsriMapControl_OnExtentUpdated(null, null);
        }

        private void MainSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            EsriMapControl_OnExtentUpdated(null, null);

            var w = ui_mainSplitContainer.SplitterDistance;
            if (_leftTSBWidthDict.ContainsKey(_currentLeftButton))
                _leftTSBWidthDict[_currentLeftButton] = w;
            else
                _leftTSBWidthDict.Add(_currentLeftButton, w);
        }

        private void AddAIMQueryLayer_Click(object sender, EventArgs e)
        {
            AimFLComplexLoader.AddComplexLayer();
        }

        private void tsItemManageAerodrome_Click(object sender, EventArgs e)
        {
            FormManageAerodrome frmManageArp = new FormManageAerodrome(DbProvider);
            if (frmManageArp.ShowDialog(Globals.MapData.SelectedAirport, Globals.Settings.AirportRasterPath) == System.Windows.Forms.DialogResult.OK)
            {
                string layerName;
                Globals.Settings.AirportRasterPath = frmManageArp.AirportRasterPath;
                string selectedArpLayerName = Globals.MapData.SelectedAirport + ".jpg";
                for (int i = 0; i < Map.LayerCount; i++)
                {
                    layerName = ui_esriMapControl.get_Layer(i).Name;
                    if (layerName == selectedArpLayerName)
                    {
                        foreach (var aimLayer in ui_newTocControl.AimLayers)
                        {
                            if (aimLayer.Layer == Map.Layer[i])
                            {
                                ui_newTocControl.RemoveLayer(aimLayer);
                                break;
                            }
                        }
                        Map.DeleteLayer(Map.Layer[i]);
                        break;
                    }
                }

                Map.AddLayer(frmManageArp.RasterLayer);
                //Map.MoveLayer ( frmManageArp.RasterLayer, 0);				
                Map.MoveLayer(frmManageArp.RasterLayer, Map.LayerCount - 2);
                Globals.MapData.SelectedAirport = frmManageArp.AirportName;
                Globals.OnLayerAdded(frmManageArp.RasterLayer);
                Zoom2Layer(frmManageArp.RasterLayer);

            }
        }

        private int LayerUpdateThreadCount
        {
            get { return _layerUpdateThreadCount; }
            set
            {
                _layerUpdateThreadCount = value;

                if (_layerUpdateThreadCount <= 0 && LayerUpdateThreadEnded != null)
                    LayerUpdateThreadEnded(this, null);
            }
        }

        private void Zoom2Layer(ILayer layer)
        {
            IEnvelope ext;

            if (layer is AimFeatureLayer)
                ext = (layer as AimFeatureLayer).GetAreaOfInterest();
            else
                ext = layer.AreaOfInterest;

            ui_esriMapControl.Extent = ext;
            ui_esriMapControl.Refresh();
        }

        private void userManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DbProvider.UserManagement == null)
                return;

            var frmUserManager = new FormUserManger(DbProvider.CurrentUser.Privilege);
            frmUserManager.UserManagement = DbProvider.UserManagement;
            frmUserManager.ShowDialog();
        }

        private void ui_toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var isCaw = (_dbProvider.ProviderType == DbProviderType.ComSoft);
            ui_userManagerTSMI.Visible = !isCaw;
        }

        private void LeftWindowButton_CheckedChanged(object sender, EventArgs e)
        {
            var tsb = sender as ToolStripButton;

            if (!tsb.Checked)
            {
                var control = tsb.Tag as Control;
                control.Visible = false;
                return;
            }

            foreach (ToolStripButton tsbItem in ui_leftWindowToolStrip.Items)
            {
                if (tsbItem != tsb && tsbItem.Checked)
                    tsbItem.Checked = false;
            }

            _currentLeftButton = tsb;
            var cnt = tsb.Tag as Control;
            cnt.Visible = true;

            int pw;
            if (_leftTSBWidthDict.TryGetValue(tsb, out pw))
                ui_mainSplitContainer.SplitterDistance = pw;

            ui_leftWindowTitle.VisibleCloseButton = (tsb != ui_tocTSB);
            ActiveControl = cnt;

            ILeftWindow lw;
            if (_controlLeftWindowDict.TryGetValue(cnt, out lw))
                ui_leftWindowTitle.Title = lw.Title;
            else
                ui_leftWindowTitle.Title = cnt.Text;
        }

        private void AddLeftWindow(ILeftWindow leftWindow)
        {
            foreach (Control cont in ui_leftWinContainerPanel.Controls)
            {
                ILeftWindow lw;
                if (_controlLeftWindowDict.TryGetValue(cont, out lw))
                {
                    if (lw.BaseOn != null && lw.BaseOn.Equals(leftWindow.BaseOn))
                    {
                        foreach (ToolStripButton tsi in ui_leftWindowToolStrip.Items)
                        {
                            if (tsi.Tag != null && tsi.Tag.Equals(cont))
                            {
                                tsi.Checked = true;
                                return;
                            }
                        }
                    }
                }
            }

            var control = leftWindow.AreaControl;

            _controlLeftWindowDict.Add(control, leftWindow);

            var tsb = new ToolStripButton();
            tsb.Tag = control;
            tsb.Font = ui_tocTSB.Font;
            tsb.Text = leftWindow.Title;
            //tsb.CheckOnClick = true;
            tsb.Click += TocTSB_Click;
            tsb.CheckedChanged += LeftWindowButton_CheckedChanged;


            control.Visible = false;
            control.Dock = DockStyle.Fill;
            control.Enter += LeftControl_Enter;
            control.Leave += LeftControl_Leave;
            ui_leftWinContainerPanel.Controls.Add(control);

            ui_leftWindowToolStrip.Items.Add(tsb);
            tsb.Checked = true;

            var b = ui_leftWindowToolStrip.Items.Count == 1;
            ui_leftTableLayoutPanel.ColumnStyles[0].Width = (b ? 0 : 30);
            ui_leftTableLayoutPanel.RowStyles[0].Height = (b ? 0 : 22);
        }

        private void RemoveLeftWindow(ToolStripButton tsb)
        {
            ui_tocTSB.Checked = true;

            ui_leftWindowToolStrip.Items.Remove(tsb);
            var cont = tsb.Tag as Control;
            cont.Parent.Controls.Remove(cont);

            var b = ui_leftWindowToolStrip.Items.Count == 1;
            ui_leftTableLayoutPanel.ColumnStyles[0].Width = (b ? 0 : 30);
            ui_leftTableLayoutPanel.RowStyles[0].Height = (b ? 0 : 22);
        }

        private void TocTSB_Click(object sender, EventArgs e)
        {
            var tsb = sender as ToolStripButton;
            tsb.Checked = true;
        }

        private void LeftControl_Leave(object sender, EventArgs e)
        {
            ui_leftWindowTitle.IsActive = false;
        }

        private void LeftControl_Enter(object sender, EventArgs e)
        {
            ui_leftWindowTitle.IsActive = true;
        }

        private void LeftWindowTitle_CloseClicked(object sender, EventArgs e)
        {
            if (_currentLeftButton != null && _currentLeftButton != ui_tocTSB)
                RemoveLeftWindow(_currentLeftButton);
        }

        private bool AskForSave()
        {
            if (!Globals.MapEdited)
                return true;

            var s = "Untitled.amp";
            if (!string.IsNullOrEmpty(_ampFileName))
                s = System.IO.Path.GetFileName(_ampFileName);

            var mbr = MessageBox.Show("Save Changes To " + s, Globals.AppText,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (mbr == DialogResult.Cancel)
                return false;

            if (mbr == DialogResult.Yes)
            {
                uiEvents_saveTSMI_Click(null, null);
            }

            return true;
        }

        private void LeftWinContainerPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (_controlLeftWindowDict.ContainsKey(e.Control))
            {
                _controlLeftWindowDict.Remove(e.Control);
            }
        }

        private void OnMyFeatureLayerVisibilityChanged(object sender, MyFLVisibleChangedEventArgs e)
        {
            var aimFL = sender as AimFeatureLayer;
            if (e.IsVisible && !aimFL.IsLoaded)
            {
                UpdateLayer(aimFL);
            }
        }

        private byte[] GetMapThumbnailImage()
        {
            try
            {
                var pathFileName = Globals.TempDir + "\\ThumbnailImage.jpg";

                ESRI.ArcGIS.Output.IExport export = new ESRI.ArcGIS.Output.ExportJPEGClass();
                export.ExportFileName = pathFileName;

                // Microsoft Windows default DPI resolution
                export.Resolution = 96;
                var exportRECT = ui_esriMapControl.ActiveView.ExportFrame;

                ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
                envelope.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
                export.PixelBounds = envelope;
                System.Int32 hDC = export.StartExporting();
                ui_esriMapControl.ActiveView.Output(hDC, (System.Int16)export.Resolution, ref exportRECT, null, null);

                // Finish writing the export file and cleanup any intermediate files
                export.FinishExporting();
                export.Cleanup();


                var image = Image.FromFile(pathFileName);
                var newWidth = 200;
                var orgSize = ui_esriMapControl.ActiveView.ExportFrame;
                var thumbImage = image.GetThumbnailImage(newWidth, (newWidth * orgSize.bottom) / orgSize.right, null, IntPtr.Zero);
                image.Dispose();

                var imageConverter = new ImageConverter();
                var bytes = imageConverter.ConvertTo(thumbImage, typeof(byte[])) as byte[];

                File.Delete(pathFileName);

                return bytes;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error on GetMapThumbnailImage\nDetails: " + ex.Message);
            }
            return null;
        }

        #region Fix ToolStripButton non focus Click problem.

        private void TSB_MouseEnter(object sender, EventArgs e)
        {
            if (!ContainsFocus)
                _focusChangedOnThisButton = sender as ToolStripButton;
        }

        private void TSB_MouseLeave(object sender, EventArgs e)
        {
            _focusChangedOnThisButton = null;
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (_focusChangedOnThisButton != null)
            {
                var tmp = _focusChangedOnThisButton;
                _focusChangedOnThisButton = null;
                tmp.PerformClick();
            }
        }

        private void MapToolStrip_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            e.Item.MouseEnter += TSB_MouseEnter;
            e.Item.MouseLeave += TSB_MouseLeave;
        }

        private void MapToolStrip_ItemRemoved(object sender, ToolStripItemEventArgs e)
        {
            e.Item.MouseEnter -= TSB_MouseEnter;
            e.Item.MouseLeave -= TSB_MouseLeave;
        }

        #endregion

        private void Help_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this,
                    string.Format("file://{0}\\AIMEnvironmentHelp.chm", Application.StartupPath));
            }
            catch { }
        }

        private void View3D_Click(object sender, EventArgs e)
        {
            var tsf = new Forms.TestSceneForm();
            tsf.ShowScene();
        }

        private void RefreshMap_Click(object sender, EventArgs e)
        {
            ui_esriMapControl.Refresh(esriViewDrawPhase.esriViewAll, null, null);
        }

        private static DbProvider CreateDBProvider()
        {
            try
            {

                #region COMSOFTDB

#if COMSOFTDB
            return new CawDbProvider();
            
#endif

                #endregion

                #region TDB

#if TDB
                var tdbDbPro = DbProviderFactory.Create("Aran.Temporality.Provider");
                tdbDbPro.CallSpecialMethod("SetApplicationEnvironment", null);

                // TODO: Open forms to select connection
                var connectionStrings = tdbDbPro.GetConnectionStrings();

                var selectedConnectionString = connectionStrings.FirstOrDefault();

                tdbDbPro.Open(selectedConnectionString);

                tdbDbPro.Login("", "");

                return tdbDbPro;
#endif

                #endregion

                return DbProviderFactory.Create("Aran.Aim.Data.PgDbProviderComplex");

            }
            catch (Exception ex)
            {
                Logger.Get("MapEnv.MainForm.CreateDbProvider").Error(ex);
                throw;
            }

        }

        private void ShowConnectionInfo()
        {
            foreach (ToolStripItem item in ui_mainStatusStrip.Items)
                item.Visible = true;

            var server = string.Empty;
            var db = string.Empty;

            var connInfo = DbProvider.GetConnectionInfo;
            var sa = connInfo.Split(";".ToCharArray());
            if (sa.Length > 0)
                server = sa[0];
            if (sa.Length > 1)
                db = sa[1];

            ui_serveStatusLabel.Text = server;
            ui_dbStatusLabel.Text = db;
            ui_userNameStatusLabel.Text = DbProvider.CurrentUser.Name;
            ui_effectiveDateStatusLabel.Text = DbProvider.DefaultEffectiveDate.ToString("yyyy-MM-dd");
            DbProvider_UseCacheChanged(null, null);
        }

        private void OnDbProvider_EffectiveDateChanged(object sender, EffectiveDateChangedEventArgs e)
        {
            e.Ignore = !_changeEffectiveDate;

            ShowConnectionInfo();
        }

        private void ExportToGDB_Click(object sender, EventArgs e)
        {
            var ef = new Aran.Exporter.Gdb.ExporterForm();
            ef.Show(this);
        }

        private void LayoutView_Click(object sender, EventArgs e)
        {
            if (_layoutView == null || _layoutView.IsDisposed)
            {
                _layoutView = new LayoutView.LayoutViewForm(_aranLayoutGraphics);
                _layoutView.Show(this);
            }
            _layoutView.Visible = true;
            _layoutView.WindowState = FormWindowState.Normal;
            _layoutView.RefreshMap();
        }

        private void AddMapData()
        {
            var layerCount = Globals.MainForm.Map.LayerCount;
            var map = Globals.MainForm.Map;
            var layerList = new List<ILayer>();

            for (int i = 0; i < map.LayerCount; i++)
                layerList.Add(map.Layer[i]);

            _mapAddDataToolbarItem.Command.OnClick();

            if (layerCount < Globals.MainForm.Map.LayerCount)
            {
                for (int i = 0; i < map.LayerCount; i++)
                {
                    if (!layerList.Contains(map.Layer[i]))
                    {
                        Globals.OnLayerAdded(map.Layer[i]);
                    }
                }
            }
        }

        private void SetAppCaption()
        {
            Text = System.IO.Path.GetFileName(_ampFileName) + " - " + Globals.AppText;

#if COMSOFTDB
            Text += "  (with CADASDB)";
#elif TDB
            Text += "  (TDB)";
#endif
        }

        private void CacheStatus_Click(object sender, EventArgs e)
        {
            DbProvider.UseCache = !DbProvider.UseCache;
        }

        private void DbProvider_UseCacheChanged(object sender, EventArgs e)
        {
            if (_dbProvider.ProviderType == DbProviderType.ComSoft)
                ui_cacheStatusLabel.Text = "Yes";
            else
                ui_cacheStatusLabel.Text = (DbProvider.UseCache ? "Yes" : "No");
        }

        private void WorkPackageManager_Click(object sender, EventArgs e)
        {
            if (_dbProvider.ProviderType == DbProviderType.ComSoft)
            {
                var wpf = new WorkPackageForm();
                wpf.Show(this);
            }
        }

        internal void AranPlugin_ToolbarChanged(object sender, EventArgs e)
        {
            var plugin = sender as AranPlugin;

            if (_aranPluginToolStripDict.ContainsKey(plugin))
            {
                var ts = _aranPluginToolStripDict[plugin];
                ui_mainToolStripContainer.TopToolStripPanel.Controls.Remove(ts);
                _aranPluginToolStripDict.Remove(plugin);

                foreach (ToolStripItem item in ui_showToolbarsTSMI.DropDownItems)
                {
                    if (ts.Equals(item.Tag))
                    {
                        ui_showToolbarsTSMI.DropDownItems.Remove(item);
                        break;
                    }
                }
            }

            if (plugin.Toolbar == null)
            {
                ui_showToolbarsTSMI.Visible = (ui_showToolbarsTSMI.DropDownItems.Count > 0);
                return;
            }

            var toolbar = plugin.Toolbar;
            toolbar.Dock = DockStyle.None;
            toolbar.AutoSize = true;
            toolbar.GripStyle = ToolStripGripStyle.Visible;


            var tsi = new ToolStripLabel(toolbar.Text);
            tsi.Margin = new Padding(0, 0, 10, 0);
            tsi.Font = new Font(tsi.Font, FontStyle.Bold);
            toolbar.Items.Insert(0, tsi);

            ui_mainToolStripContainer.TopToolStripPanel.Controls.Add(toolbar);
            _aranPluginToolStripDict.Add(plugin, toolbar);

            var showToolbarTSB = new ToolStripMenuItem(toolbar.Text);
            showToolbarTSB.CheckOnClick = true;
            showToolbarTSB.Checked = true;
            showToolbarTSB.Tag = toolbar;
            showToolbarTSB.AutoSize = true;
            showToolbarTSB.Click += ((xs, xe) =>
            {
                var tsItem = xs as ToolStripMenuItem;
                var appToolStrip = tsItem.Tag as ToolStrip;
                appToolStrip.Visible = tsItem.Checked;
            });
            ui_showToolbarsTSMI.DropDownItems.Add(showToolbarTSB);

            ui_showToolbarsTSMI.Visible = true;
        }

        private void ExportLayersToAixm_Click(object sender, EventArgs e)
        {
            var aeof = new AixmExportOptionsForm();
            if (aeof.ShowDialog() != DialogResult.OK)
                return;

            var complexLayers = new List<AimFeatureLayer>();
            var simpleLayers = new List<AimFeatureLayer>();

            foreach (var aimLayer in ui_newTocControl.AimLayers)
            {
                if (aimLayer.Layer.Visible &&
                    (aimLayer.Layer is AimFeatureLayer aimFL) &&
                    aimFL.IsLoaded)
                {
                    if (aimFL.IsComplex)
                        complexLayers.Add(aimFL);
                    else
                        simpleLayers.Add(aimFL);
                }
            }

            var allFeatures = new List<Aran.Aim.Features.Feature>();
            var allFeaturesSet = new HashSet<Guid>();

            foreach (var aimLayer in complexLayers)
            {
                var layerFeatures = aimLayer.ComplexTable.GetAllFeatures();
                foreach (var feat in layerFeatures)
                {
                    if (allFeaturesSet.Add(feat.Identifier))
                        allFeatures.Add(feat);
                }
            }

            var dbUtil = new DbUtility(_dbProvider);
            var defaultInterpretationType = Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE;
            dbUtil.SetDefault(defaultInterpretationType, _dbProvider.DefaultEffectiveDate);
            var errorList = new List<string>();

            var allLoadedFeatures = new List<Aran.Aim.Features.Feature>();
            var allLoadedFeaturesSet = new HashSet<Guid>();

            foreach(var feat in allFeatures)
            {
                if (!allLoadedFeaturesSet.Contains(feat.Identifier))
                    dbUtil.LoadWithRefFeatures(feat.FeatureType, feat.Identifier, allLoadedFeatures, errorList);

                foreach(var lf in allLoadedFeatures)
                    allLoadedFeaturesSet.Add(lf.Identifier);
            }

            foreach(var feat in allLoadedFeatures)
            {
                if (allFeaturesSet.Add(feat.Identifier))
                    allFeatures.Add(feat);
            }

            foreach(var simpleLayer in simpleLayers)
            {
                foreach(var feat in simpleLayer.AimFeatures)
                {
                    if (allFeaturesSet.Add(feat.Identifier))
                        allFeatures.Add(feat);
                }
            }

            if (allFeatures.Count > 0)
            {
                Aran.Aim.InputFormLib.InputFormController.WriteAllFeatureToXML(
                            allFeatures, aeof.FileName, aeof.IsWriteExtensions,
                            aeof.Write3DIfExists, DbProvider.DefaultEffectiveDate, aeof.SrsType);

                MessageBox.Show(
                    $"All Visible AIM Layer Features successfully exported!",
                    "Exporter",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region Test Click

        private void Test1_Click(object sender, EventArgs e)
        {
#if TEST
#endif
        }

        private void Test2_Click(object sender, EventArgs e)
        {
#if TEST
#endif
        }

        private void Test3_Click(object sender, EventArgs e)
        {
#if TEST
#endif
        }

        private void Test4_Click(object sender, EventArgs e)
        {
#if TEST
#endif
        }
        #endregion

        private void ui_esriMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            // if Scroll button click
            if (e.button == 4)
                ui_esriMapControl.Pan();
        }
    }

    internal delegate void LayerUpdated(ILayer esriLayer, IEnumerable<Aran.Aim.Features.Feature> features, bool refreshMap);

    #region Classes

    internal class MyItemDef : IItemDef
    {
        public bool Group
        {
            get;
            set;
        }
        public string ID
        {
            get;
            set;
        }
        public int SubType
        {
            get;
            set;
        }
    }

    internal class EnvAranPlugin
    {
        public EnvAranPlugin()
        {
            IsEnabled = false;
        }

        public EnvAranPlugin(AranPlugin plugin)
        {
            Plugin = plugin;
        }

        public bool IsEnabled { get; set; }

        public AranPlugin Plugin { get; set; }
    }

    internal class EnvAranSettingsPage
    {
        public EnvAranSettingsPage()
        {
            IsEnabled = false;
        }

        public bool IsEnabled { get; set; }

        public Guid[] BaseOnPlugins { get; set; }

        public ISettingsPage Page { get; set; }
    }

    #endregion

}