namespace MapEnv
{
    partial class MainForm
	{
		private System.Windows.Forms.MenuStrip ui_mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem ui_fileTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_openTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_saveTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_saveAsTSMI;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem ui_exportMxdTSMI;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_showStatusBarTSB;
		private System.Windows.Forms.ToolStripMenuItem dViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_mapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_selectToolTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_panToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_scaleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem ui_fullExtendToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_prevExtendToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_nextExtendToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem ui_addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_identifyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_measureToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem selectFeatureToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem tsItemManageAerodrome;
		private System.Windows.Forms.ToolStripMenuItem aIMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_addAimFeatureLayerTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_addAIMQueryLayerTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_inputFormsTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_infoTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_applicationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_userManagerTSMI;
		private System.Windows.Forms.ToolStripSeparator ui_toolStripSeparatorUserManager;
		private System.Windows.Forms.ToolStripMenuItem ui_optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ui_aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip ui_mainStatusStrip;
		private System.Windows.Forms.ToolStripStatusLabel ui_springStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel ui_posStatusLabel;
		private System.Windows.Forms.ImageList ui_tocTreeImageList;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList ui_tocTreeViewStateImageList;
		private System.Windows.Forms.SplitContainer ui_mainSplitContainer;
		private ESRI.ArcGIS.Controls.AxMapControl ui_esriMapControl;

        #region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
            System.Windows.Forms.ToolStripMenuItem layoutViewToolStripMenuItem;
            System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
            System.Windows.Forms.ToolStripMenuItem defaultFeatureTypeStylesToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.ui_fileTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_openTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_saveTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_saveAsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_exportLayersToAixmTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_exportMxdTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_showStatusBarTSB = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_showToolbarsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.dViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_selectToolTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_panToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_fullExtendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_prevExtendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_nextExtendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_identifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_measureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsItemManageAerodrome = new System.Windows.Forms.ToolStripMenuItem();
            this.aIMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_addAimFeatureLayerTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_addAIMQueryLayerTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_inputFormsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_infoTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_applicationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_exportToGDBTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_userManagerTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_toolStripSeparatorUserManager = new System.Windows.Forms.ToolStripSeparator();
            this.ui_optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_cadasMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_workPackageManagerTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_viewHelpTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_testTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ui_serveStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_dbStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_userNameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_effectiveDateStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_cacheStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_springStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_posStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_tocTreeImageList = new System.Windows.Forms.ImageList(this.components);
            this.ui_tocTreeViewStateImageList = new System.Windows.Forms.ImageList(this.components);
            this.ui_topPanel = new System.Windows.Forms.Panel();
            this.ui_esriDrawToolbarControl = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.ui_esriMapToolbarControl = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.ui_mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_leftTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ui_leftWindowTitle = new MapEnv.Controls.LeftWindowTitle();
            this.ui_leftWinContainerPanel = new System.Windows.Forms.Panel();
            this.ui_newTocControl = new MapEnv.Controls.TOCControl();
            this.ui_leftWindowToolStrip = new System.Windows.Forms.ToolStrip();
            this.ui_tocTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_esriMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.ui_mainToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            layoutViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            defaultFeatureTypeStylesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_mainMenuStrip.SuspendLayout();
            this.ui_mainStatusStrip.SuspendLayout();
            this.ui_topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriDrawToolbarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapToolbarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).BeginInit();
            this.ui_mainSplitContainer.Panel1.SuspendLayout();
            this.ui_mainSplitContainer.Panel2.SuspendLayout();
            this.ui_mainSplitContainer.SuspendLayout();
            this.ui_leftTableLayoutPanel.SuspendLayout();
            this.ui_leftWinContainerPanel.SuspendLayout();
            this.ui_leftWindowToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.ui_mainToolStripContainer.ContentPanel.SuspendLayout();
            this.ui_mainToolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(48, 20);
            toolStripStatusLabel1.Text = "Server:";
            toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            toolStripStatusLabel3.Margin = new System.Windows.Forms.Padding(6, 3, 0, 2);
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new System.Drawing.Size(27, 20);
            toolStripStatusLabel3.Text = "DB:";
            toolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolStripStatusLabel5.Margin = new System.Windows.Forms.Padding(6, 3, 0, 2);
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Size = new System.Drawing.Size(91, 20);
            toolStripStatusLabel5.Text = "Effective Date:";
            toolStripStatusLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(6, 3, 0, 2);
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(72, 20);
            toolStripStatusLabel2.Text = "User Name:";
            toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layoutViewToolStripMenuItem
            // 
            layoutViewToolStripMenuItem.Name = "layoutViewToolStripMenuItem";
            layoutViewToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            layoutViewToolStripMenuItem.Text = "Layout View";
            layoutViewToolStripMenuItem.Click += new System.EventHandler(this.LayoutView_Click);
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            toolStripStatusLabel4.Margin = new System.Windows.Forms.Padding(6, 3, 0, 2);
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new System.Drawing.Size(43, 20);
            toolStripStatusLabel4.Text = "Cache:";
            toolStripStatusLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // defaultFeatureTypeStylesToolStripMenuItem
            // 
            defaultFeatureTypeStylesToolStripMenuItem.Image = global::MapEnv.Properties.Resources.default_feat_styles_24;
            defaultFeatureTypeStylesToolStripMenuItem.Name = "defaultFeatureTypeStylesToolStripMenuItem";
            defaultFeatureTypeStylesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            defaultFeatureTypeStylesToolStripMenuItem.Text = "Default Feature Styles";
            defaultFeatureTypeStylesToolStripMenuItem.Click += new System.EventHandler(this.DefaultFeatureTypeStyles_Click);
            // 
            // ui_mainMenuStrip
            // 
            this.ui_mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_fileTSMI,
            this.viewToolStripMenuItem,
            this.ui_mapToolStripMenuItem,
            this.aIMToolStripMenuItem,
            this.ui_applicationsToolStripMenuItem,
            this.ui_toolsToolStripMenuItem,
            this.ui_cadasMenuItem,
            this.ui_helpToolStripMenuItem,
            this.ui_testTSMI});
            this.ui_mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.ui_mainMenuStrip.Name = "ui_mainMenuStrip";
            this.ui_mainMenuStrip.Size = new System.Drawing.Size(1084, 24);
            this.ui_mainMenuStrip.TabIndex = 0;
            this.ui_mainMenuStrip.Text = "menuStrip1";
            // 
            // ui_fileTSMI
            // 
            this.ui_fileTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_openTSMI,
            this.ui_saveTSMI,
            this.ui_saveAsTSMI,
            this.toolStripMenuItem6,
            this.ui_exportLayersToAixmTSMI,
            this.toolStripMenuItem1,
            this.ui_exportMxdTSMI,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
            this.ui_fileTSMI.Name = "ui_fileTSMI";
            this.ui_fileTSMI.Size = new System.Drawing.Size(37, 20);
            this.ui_fileTSMI.Text = "&File";
            this.ui_fileTSMI.Click += new System.EventHandler(this.uiEvents_fileTSMI_Click);
            // 
            // ui_openTSMI
            // 
            this.ui_openTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_openTSMI.Image")));
            this.ui_openTSMI.Name = "ui_openTSMI";
            this.ui_openTSMI.Size = new System.Drawing.Size(255, 22);
            this.ui_openTSMI.Text = "Open";
            this.ui_openTSMI.Click += new System.EventHandler(this.uiEvents_openTSMI_Click);
            // 
            // ui_saveTSMI
            // 
            this.ui_saveTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_saveTSMI.Image")));
            this.ui_saveTSMI.Name = "ui_saveTSMI";
            this.ui_saveTSMI.Size = new System.Drawing.Size(255, 22);
            this.ui_saveTSMI.Text = "Save";
            this.ui_saveTSMI.Click += new System.EventHandler(this.uiEvents_saveTSMI_Click);
            // 
            // ui_saveAsTSMI
            // 
            this.ui_saveAsTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_saveAsTSMI.Image")));
            this.ui_saveAsTSMI.Name = "ui_saveAsTSMI";
            this.ui_saveAsTSMI.Size = new System.Drawing.Size(255, 22);
            this.ui_saveAsTSMI.Text = "Save As";
            this.ui_saveAsTSMI.Click += new System.EventHandler(this.uiEvents_saveAsTSMI_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(252, 6);
            // 
            // ui_exportLayersToAixmTSMI
            // 
            this.ui_exportLayersToAixmTSMI.Image = global::MapEnv.Properties.Resources.export_16;
            this.ui_exportLayersToAixmTSMI.Name = "ui_exportLayersToAixmTSMI";
            this.ui_exportLayersToAixmTSMI.Size = new System.Drawing.Size(255, 22);
            this.ui_exportLayersToAixmTSMI.Text = "Export All Layers to AIXM Message";
            this.ui_exportLayersToAixmTSMI.Click += new System.EventHandler(this.ExportLayersToAixm_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(252, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // ui_exportMxdTSMI
            // 
            this.ui_exportMxdTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_exportMxdTSMI.Image")));
            this.ui_exportMxdTSMI.Name = "ui_exportMxdTSMI";
            this.ui_exportMxdTSMI.Size = new System.Drawing.Size(255, 22);
            this.ui_exportMxdTSMI.Text = "Export to ArcMap Document";
            this.ui_exportMxdTSMI.Visible = false;
            this.ui_exportMxdTSMI.Click += new System.EventHandler(this.uiEvents_exportMxdTSMI_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(252, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.uiEvents_exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_showStatusBarTSB,
            this.ui_showToolbarsTSMI,
            this.toolStripMenuItem5,
            layoutViewToolStripMenuItem,
            this.dViewToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // ui_showStatusBarTSB
            // 
            this.ui_showStatusBarTSB.Checked = true;
            this.ui_showStatusBarTSB.CheckOnClick = true;
            this.ui_showStatusBarTSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_showStatusBarTSB.Name = "ui_showStatusBarTSB";
            this.ui_showStatusBarTSB.Size = new System.Drawing.Size(138, 22);
            this.ui_showStatusBarTSB.Text = "Status Bar";
            this.ui_showStatusBarTSB.Click += new System.EventHandler(this.uiEvents_showStatusBarTSB_Click);
            // 
            // ui_showToolbarsTSMI
            // 
            this.ui_showToolbarsTSMI.Name = "ui_showToolbarsTSMI";
            this.ui_showToolbarsTSMI.Size = new System.Drawing.Size(138, 22);
            this.ui_showToolbarsTSMI.Text = "Toolbars";
            this.ui_showToolbarsTSMI.Visible = false;
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(135, 6);
            // 
            // dViewToolStripMenuItem
            // 
            this.dViewToolStripMenuItem.Name = "dViewToolStripMenuItem";
            this.dViewToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.dViewToolStripMenuItem.Text = "3D View";
            this.dViewToolStripMenuItem.Click += new System.EventHandler(this.View3D_Click);
            // 
            // ui_mapToolStripMenuItem
            // 
            this.ui_mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_selectToolTSMI,
            this.ui_panToolStripMenuItem,
            this.ui_scaleToolStripMenuItem,
            this.ui_zoomInToolStripMenuItem,
            this.ui_zoomOutToolStripMenuItem,
            this.toolStripMenuItem3,
            this.ui_fullExtendToolStripMenuItem,
            this.ui_prevExtendToolStripMenuItem,
            this.ui_nextExtendToolStripMenuItem,
            this.toolStripMenuItem2,
            this.ui_addToolStripMenuItem,
            this.ui_identifyToolStripMenuItem,
            this.ui_measureToolStripMenuItem,
            this.toolStripMenuItem7,
            this.selectFeatureToolStripMenuItem,
            this.toolStripSeparator1,
            this.tsItemManageAerodrome});
            this.ui_mapToolStripMenuItem.Name = "ui_mapToolStripMenuItem";
            this.ui_mapToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.ui_mapToolStripMenuItem.Text = "&Map";
            this.ui_mapToolStripMenuItem.Visible = false;
            // 
            // ui_selectToolTSMI
            // 
            this.ui_selectToolTSMI.CheckOnClick = true;
            this.ui_selectToolTSMI.Name = "ui_selectToolTSMI";
            this.ui_selectToolTSMI.Size = new System.Drawing.Size(180, 22);
            this.ui_selectToolTSMI.Tag = "ControlToolsGraphicElement_SelectTool";
            this.ui_selectToolTSMI.Text = "Select Tool";
            // 
            // ui_panToolStripMenuItem
            // 
            this.ui_panToolStripMenuItem.CheckOnClick = true;
            this.ui_panToolStripMenuItem.Name = "ui_panToolStripMenuItem";
            this.ui_panToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_panToolStripMenuItem.Tag = "ControlToolsMapNavigation_Pan";
            this.ui_panToolStripMenuItem.Text = "&Pan";
            // 
            // ui_scaleToolStripMenuItem
            // 
            this.ui_scaleToolStripMenuItem.CheckOnClick = true;
            this.ui_scaleToolStripMenuItem.Name = "ui_scaleToolStripMenuItem";
            this.ui_scaleToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_scaleToolStripMenuItem.Tag = "ControlToolsMapNavigation_ZoomControl";
            this.ui_scaleToolStripMenuItem.Text = "&Scale";
            // 
            // ui_zoomInToolStripMenuItem
            // 
            this.ui_zoomInToolStripMenuItem.CheckOnClick = true;
            this.ui_zoomInToolStripMenuItem.Name = "ui_zoomInToolStripMenuItem";
            this.ui_zoomInToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_zoomInToolStripMenuItem.Tag = "ControlToolsMapNavigation_ZoomIn";
            this.ui_zoomInToolStripMenuItem.Text = "Zoom &In";
            // 
            // ui_zoomOutToolStripMenuItem
            // 
            this.ui_zoomOutToolStripMenuItem.CheckOnClick = true;
            this.ui_zoomOutToolStripMenuItem.Name = "ui_zoomOutToolStripMenuItem";
            this.ui_zoomOutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_zoomOutToolStripMenuItem.Tag = "ControlToolsMapNavigation_ZoomOut";
            this.ui_zoomOutToolStripMenuItem.Text = "Zoom &Out";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // ui_fullExtendToolStripMenuItem
            // 
            this.ui_fullExtendToolStripMenuItem.Name = "ui_fullExtendToolStripMenuItem";
            this.ui_fullExtendToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_fullExtendToolStripMenuItem.Tag = "ControlToolsMapNavigation_FullExtent";
            this.ui_fullExtendToolStripMenuItem.Text = "Full Extend";
            // 
            // ui_prevExtendToolStripMenuItem
            // 
            this.ui_prevExtendToolStripMenuItem.Name = "ui_prevExtendToolStripMenuItem";
            this.ui_prevExtendToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_prevExtendToolStripMenuItem.Tag = "ControlToolsMapNavigation_ZoomToLastExtentBack";
            this.ui_prevExtendToolStripMenuItem.Text = "Previous Extend";
            // 
            // ui_nextExtendToolStripMenuItem
            // 
            this.ui_nextExtendToolStripMenuItem.Name = "ui_nextExtendToolStripMenuItem";
            this.ui_nextExtendToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_nextExtendToolStripMenuItem.Tag = "ControlToolsMapNavigation_ZoomToLastExtentForward";
            this.ui_nextExtendToolStripMenuItem.Text = "Next Extend";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // ui_addToolStripMenuItem
            // 
            this.ui_addToolStripMenuItem.Name = "ui_addToolStripMenuItem";
            this.ui_addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_addToolStripMenuItem.Tag = "ControlToolsGeneric_AddData";
            this.ui_addToolStripMenuItem.Text = "&Add Data";
            // 
            // ui_identifyToolStripMenuItem
            // 
            this.ui_identifyToolStripMenuItem.CheckOnClick = true;
            this.ui_identifyToolStripMenuItem.Name = "ui_identifyToolStripMenuItem";
            this.ui_identifyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_identifyToolStripMenuItem.Tag = "ControlToolsMapInquiry_Identify";
            this.ui_identifyToolStripMenuItem.Text = "Identify";
            // 
            // ui_measureToolStripMenuItem
            // 
            this.ui_measureToolStripMenuItem.CheckOnClick = true;
            this.ui_measureToolStripMenuItem.Name = "ui_measureToolStripMenuItem";
            this.ui_measureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ui_measureToolStripMenuItem.Tag = "ControlToolsMapInquiry_Measure";
            this.ui_measureToolStripMenuItem.Text = "Measure";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(177, 6);
            // 
            // selectFeatureToolStripMenuItem
            // 
            this.selectFeatureToolStripMenuItem.CheckOnClick = true;
            this.selectFeatureToolStripMenuItem.Name = "selectFeatureToolStripMenuItem";
            this.selectFeatureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectFeatureToolStripMenuItem.Tag = "ControlToolsFeatureSelection_SelectFeatures";
            this.selectFeatureToolStripMenuItem.Text = "Select Feature";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // tsItemManageAerodrome
            // 
            this.tsItemManageAerodrome.Image = global::MapEnv.Properties.Resources.airport;
            this.tsItemManageAerodrome.Name = "tsItemManageAerodrome";
            this.tsItemManageAerodrome.Size = new System.Drawing.Size(180, 22);
            this.tsItemManageAerodrome.Text = "Manage Aerodrome";
            this.tsItemManageAerodrome.Click += new System.EventHandler(this.tsItemManageAerodrome_Click);
            // 
            // aIMToolStripMenuItem
            // 
            this.aIMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_addAimFeatureLayerTSMI,
            this.ui_addAIMQueryLayerTSMI,
            this.ui_inputFormsTSMI,
            this.ui_infoTSMI});
            this.aIMToolStripMenuItem.Name = "aIMToolStripMenuItem";
            this.aIMToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.aIMToolStripMenuItem.Text = "A&IM";
            // 
            // ui_addAimFeatureLayerTSMI
            // 
            this.ui_addAimFeatureLayerTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_addAimFeatureLayerTSMI.Image")));
            this.ui_addAimFeatureLayerTSMI.Name = "ui_addAimFeatureLayerTSMI";
            this.ui_addAimFeatureLayerTSMI.Size = new System.Drawing.Size(177, 22);
            this.ui_addAimFeatureLayerTSMI.Text = "Add Simple Layer";
            this.ui_addAimFeatureLayerTSMI.Click += new System.EventHandler(this.AddAimFeatureLayer_Click);
            // 
            // ui_addAIMQueryLayerTSMI
            // 
            this.ui_addAIMQueryLayerTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_addAIMQueryLayerTSMI.Image")));
            this.ui_addAIMQueryLayerTSMI.Name = "ui_addAIMQueryLayerTSMI";
            this.ui_addAIMQueryLayerTSMI.Size = new System.Drawing.Size(177, 22);
            this.ui_addAIMQueryLayerTSMI.Text = "Add Complex Layer";
            this.ui_addAIMQueryLayerTSMI.Click += new System.EventHandler(this.AddAIMQueryLayer_Click);
            // 
            // ui_inputFormsTSMI
            // 
            this.ui_inputFormsTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_inputFormsTSMI.Image")));
            this.ui_inputFormsTSMI.Name = "ui_inputFormsTSMI";
            this.ui_inputFormsTSMI.Size = new System.Drawing.Size(177, 22);
            this.ui_inputFormsTSMI.Text = "Data Manager";
            this.ui_inputFormsTSMI.Visible = false;
            this.ui_inputFormsTSMI.Click += new System.EventHandler(this.uiEvents_inputFormsTSMI_Click);
            // 
            // ui_infoTSMI
            // 
            this.ui_infoTSMI.CheckOnClick = true;
            this.ui_infoTSMI.Image = ((System.Drawing.Image)(resources.GetObject("ui_infoTSMI.Image")));
            this.ui_infoTSMI.ImageTransparentColor = System.Drawing.Color.White;
            this.ui_infoTSMI.Name = "ui_infoTSMI";
            this.ui_infoTSMI.Size = new System.Drawing.Size(177, 22);
            this.ui_infoTSMI.Text = "Info";
            this.ui_infoTSMI.Visible = false;
            this.ui_infoTSMI.CheckedChanged += new System.EventHandler(this.uiEvents_infoTSMI_CheckedChanged);
            this.ui_infoTSMI.Click += new System.EventHandler(this.uiEvents_infoTSMI_Click);
            // 
            // ui_applicationsToolStripMenuItem
            // 
            this.ui_applicationsToolStripMenuItem.Name = "ui_applicationsToolStripMenuItem";
            this.ui_applicationsToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.ui_applicationsToolStripMenuItem.Text = "&Applications";
            this.ui_applicationsToolStripMenuItem.Visible = false;
            // 
            // ui_toolsToolStripMenuItem
            // 
            this.ui_toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_exportToGDBTSMI,
            this.ui_userManagerTSMI,
            this.ui_toolStripSeparatorUserManager,
            defaultFeatureTypeStylesToolStripMenuItem,
            this.ui_optionsToolStripMenuItem});
            this.ui_toolsToolStripMenuItem.Name = "ui_toolsToolStripMenuItem";
            this.ui_toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ui_toolsToolStripMenuItem.Text = "&Tools";
            this.ui_toolsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.ui_toolsToolStripMenuItem_DropDownOpening);
            // 
            // ui_exportToGDBTSMI
            // 
            this.ui_exportToGDBTSMI.Image = global::MapEnv.Properties.Resources.export_file_24;
            this.ui_exportToGDBTSMI.Name = "ui_exportToGDBTSMI";
            this.ui_exportToGDBTSMI.Size = new System.Drawing.Size(187, 22);
            this.ui_exportToGDBTSMI.Text = "&Export to GDB";
            this.ui_exportToGDBTSMI.Click += new System.EventHandler(this.ExportToGDB_Click);
            // 
            // ui_userManagerTSMI
            // 
            this.ui_userManagerTSMI.Image = global::MapEnv.Properties.Resources.user_group;
            this.ui_userManagerTSMI.Name = "ui_userManagerTSMI";
            this.ui_userManagerTSMI.Size = new System.Drawing.Size(187, 22);
            this.ui_userManagerTSMI.Text = "User Manager";
            this.ui_userManagerTSMI.Click += new System.EventHandler(this.userManagerToolStripMenuItem_Click);
            // 
            // ui_toolStripSeparatorUserManager
            // 
            this.ui_toolStripSeparatorUserManager.Name = "ui_toolStripSeparatorUserManager";
            this.ui_toolStripSeparatorUserManager.Size = new System.Drawing.Size(184, 6);
            // 
            // ui_optionsToolStripMenuItem
            // 
            this.ui_optionsToolStripMenuItem.Image = global::MapEnv.Properties.Resources.settings_24;
            this.ui_optionsToolStripMenuItem.Name = "ui_optionsToolStripMenuItem";
            this.ui_optionsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.ui_optionsToolStripMenuItem.Text = "&Options";
            this.ui_optionsToolStripMenuItem.Click += new System.EventHandler(this.uiEvents_optionsToolStripMenuItem_Click);
            // 
            // ui_cadasMenuItem
            // 
            this.ui_cadasMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_workPackageManagerTSMI});
            this.ui_cadasMenuItem.Name = "ui_cadasMenuItem";
            this.ui_cadasMenuItem.Size = new System.Drawing.Size(75, 20);
            this.ui_cadasMenuItem.Text = "CADAS DB";
            this.ui_cadasMenuItem.Visible = false;
            // 
            // ui_workPackageManagerTSMI
            // 
            this.ui_workPackageManagerTSMI.Name = "ui_workPackageManagerTSMI";
            this.ui_workPackageManagerTSMI.Size = new System.Drawing.Size(199, 22);
            this.ui_workPackageManagerTSMI.Text = "Work Package Manager";
            this.ui_workPackageManagerTSMI.Click += new System.EventHandler(this.WorkPackageManager_Click);
            // 
            // ui_helpToolStripMenuItem
            // 
            this.ui_helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_viewHelpTSMI,
            this.ui_aboutToolStripMenuItem});
            this.ui_helpToolStripMenuItem.Name = "ui_helpToolStripMenuItem";
            this.ui_helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.ui_helpToolStripMenuItem.Text = "&Help";
            // 
            // ui_viewHelpTSMI
            // 
            this.ui_viewHelpTSMI.Name = "ui_viewHelpTSMI";
            this.ui_viewHelpTSMI.Size = new System.Drawing.Size(203, 22);
            this.ui_viewHelpTSMI.Text = "View Help";
            this.ui_viewHelpTSMI.Click += new System.EventHandler(this.Help_Click);
            // 
            // ui_aboutToolStripMenuItem
            // 
            this.ui_aboutToolStripMenuItem.Name = "ui_aboutToolStripMenuItem";
            this.ui_aboutToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.ui_aboutToolStripMenuItem.Text = "&About AIM Environment";
            this.ui_aboutToolStripMenuItem.Click += new System.EventHandler(this.uiEvents_aboutToolStripMenuItem_Click);
            // 
            // ui_testTSMI
            // 
            this.ui_testTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test1ToolStripMenuItem,
            this.test2ToolStripMenuItem,
            this.test3ToolStripMenuItem,
            this.test4ToolStripMenuItem});
            this.ui_testTSMI.Name = "ui_testTSMI";
            this.ui_testTSMI.Size = new System.Drawing.Size(41, 20);
            this.ui_testTSMI.Text = "Test";
            // 
            // test1ToolStripMenuItem
            // 
            this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            this.test1ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.test1ToolStripMenuItem.Text = "Test - 1";
            this.test1ToolStripMenuItem.Click += new System.EventHandler(this.Test1_Click);
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.test2ToolStripMenuItem.Text = "Test - 2";
            this.test2ToolStripMenuItem.Click += new System.EventHandler(this.Test2_Click);
            // 
            // test3ToolStripMenuItem
            // 
            this.test3ToolStripMenuItem.Name = "test3ToolStripMenuItem";
            this.test3ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.test3ToolStripMenuItem.Text = "Test - 3";
            this.test3ToolStripMenuItem.Click += new System.EventHandler(this.Test3_Click);
            // 
            // test4ToolStripMenuItem
            // 
            this.test4ToolStripMenuItem.Name = "test4ToolStripMenuItem";
            this.test4ToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.test4ToolStripMenuItem.Text = "Test - 4";
            this.test4ToolStripMenuItem.Click += new System.EventHandler(this.Test4_Click);
            // 
            // ui_mainStatusStrip
            // 
            this.ui_mainStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel1,
            this.ui_serveStatusLabel,
            toolStripStatusLabel3,
            this.ui_dbStatusLabel,
            toolStripStatusLabel2,
            this.ui_userNameStatusLabel,
            toolStripStatusLabel5,
            this.ui_effectiveDateStatusLabel,
            toolStripStatusLabel4,
            this.ui_cacheStatusLabel,
            this.ui_springStatusLabel,
            this.ui_posStatusLabel});
            this.ui_mainStatusStrip.Location = new System.Drawing.Point(0, 565);
            this.ui_mainStatusStrip.Name = "ui_mainStatusStrip";
            this.ui_mainStatusStrip.Size = new System.Drawing.Size(1084, 25);
            this.ui_mainStatusStrip.TabIndex = 1;
            this.ui_mainStatusStrip.Text = "statusStrip1";
            // 
            // ui_serveStatusLabel
            // 
            this.ui_serveStatusLabel.Name = "ui_serveStatusLabel";
            this.ui_serveStatusLabel.Size = new System.Drawing.Size(55, 20);
            this.ui_serveStatusLabel.Text = "<Server>";
            this.ui_serveStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_dbStatusLabel
            // 
            this.ui_dbStatusLabel.Name = "ui_dbStatusLabel";
            this.ui_dbStatusLabel.Size = new System.Drawing.Size(38, 20);
            this.ui_dbStatusLabel.Text = "<DB>";
            this.ui_dbStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_userNameStatusLabel
            // 
            this.ui_userNameStatusLabel.Name = "ui_userNameStatusLabel";
            this.ui_userNameStatusLabel.Size = new System.Drawing.Size(78, 20);
            this.ui_userNameStatusLabel.Text = "<UserName>";
            this.ui_userNameStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_effectiveDateStatusLabel
            // 
            this.ui_effectiveDateStatusLabel.Name = "ui_effectiveDateStatusLabel";
            this.ui_effectiveDateStatusLabel.Size = new System.Drawing.Size(92, 20);
            this.ui_effectiveDateStatusLabel.Text = "<EffectiveDate>";
            this.ui_effectiveDateStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_cacheStatusLabel
            // 
            this.ui_cacheStatusLabel.Name = "ui_cacheStatusLabel";
            this.ui_cacheStatusLabel.Size = new System.Drawing.Size(92, 20);
            this.ui_cacheStatusLabel.Text = "<EffectiveDate>";
            this.ui_cacheStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_cacheStatusLabel.Click += new System.EventHandler(this.CacheStatus_Click);
            // 
            // ui_springStatusLabel
            // 
            this.ui_springStatusLabel.Name = "ui_springStatusLabel";
            this.ui_springStatusLabel.Size = new System.Drawing.Size(59, 20);
            this.ui_springStatusLabel.Spring = true;
            // 
            // ui_posStatusLabel
            // 
            this.ui_posStatusLabel.AutoSize = false;
            this.ui_posStatusLabel.Name = "ui_posStatusLabel";
            this.ui_posStatusLabel.Size = new System.Drawing.Size(350, 20);
            // 
            // ui_tocTreeImageList
            // 
            this.ui_tocTreeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ui_tocTreeImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.ui_tocTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ui_tocTreeViewStateImageList
            // 
            this.ui_tocTreeViewStateImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ui_tocTreeViewStateImageList.ImageStream")));
            this.ui_tocTreeViewStateImageList.TransparentColor = System.Drawing.Color.White;
            this.ui_tocTreeViewStateImageList.Images.SetKeyName(0, "checkBoxUnchecked16x16.png");
            this.ui_tocTreeViewStateImageList.Images.SetKeyName(1, "checkBoxChecked16x16.png");
            // 
            // ui_topPanel
            // 
            this.ui_topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ui_topPanel.Controls.Add(this.ui_esriDrawToolbarControl);
            this.ui_topPanel.Controls.Add(this.ui_esriMapToolbarControl);
            this.ui_topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_topPanel.Location = new System.Drawing.Point(0, 24);
            this.ui_topPanel.Name = "ui_topPanel";
            this.ui_topPanel.Size = new System.Drawing.Size(1084, 34);
            this.ui_topPanel.TabIndex = 3;
            // 
            // ui_esriDrawToolbarControl
            // 
            this.ui_esriDrawToolbarControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_esriDrawToolbarControl.Location = new System.Drawing.Point(849, 3);
            this.ui_esriDrawToolbarControl.Name = "ui_esriDrawToolbarControl";
            this.ui_esriDrawToolbarControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriDrawToolbarControl.OcxState")));
            this.ui_esriDrawToolbarControl.Size = new System.Drawing.Size(290, 28);
            this.ui_esriDrawToolbarControl.TabIndex = 2;
            // 
            // ui_esriMapToolbarControl
            // 
            this.ui_esriMapToolbarControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_esriMapToolbarControl.Location = new System.Drawing.Point(3, 3);
            this.ui_esriMapToolbarControl.Name = "ui_esriMapToolbarControl";
            this.ui_esriMapToolbarControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriMapToolbarControl.OcxState")));
            this.ui_esriMapToolbarControl.Size = new System.Drawing.Size(1050, 28);
            this.ui_esriMapToolbarControl.TabIndex = 5;
            // 
            // ui_mainSplitContainer
            // 
            this.ui_mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ui_mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ui_mainSplitContainer.Name = "ui_mainSplitContainer";
            // 
            // ui_mainSplitContainer.Panel1
            // 
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_leftTableLayoutPanel);
            // 
            // ui_mainSplitContainer.Panel2
            // 
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.ui_esriMapControl);
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.axLicenseControl1);
            this.ui_mainSplitContainer.Size = new System.Drawing.Size(1084, 482);
            this.ui_mainSplitContainer.SplitterDistance = 290;
            this.ui_mainSplitContainer.TabIndex = 0;
            this.ui_mainSplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.MainSplitContainer_SplitterMoved);
            // 
            // ui_leftTableLayoutPanel
            // 
            this.ui_leftTableLayoutPanel.ColumnCount = 2;
            this.ui_leftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ui_leftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ui_leftTableLayoutPanel.Controls.Add(this.ui_leftWindowTitle, 1, 0);
            this.ui_leftTableLayoutPanel.Controls.Add(this.ui_leftWinContainerPanel, 1, 1);
            this.ui_leftTableLayoutPanel.Controls.Add(this.ui_leftWindowToolStrip, 0, 0);
            this.ui_leftTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_leftTableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.ui_leftTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_leftTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ui_leftTableLayoutPanel.Name = "ui_leftTableLayoutPanel";
            this.ui_leftTableLayoutPanel.RowCount = 2;
            this.ui_leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.ui_leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ui_leftTableLayoutPanel.Size = new System.Drawing.Size(290, 482);
            this.ui_leftTableLayoutPanel.TabIndex = 9;
            // 
            // ui_leftWindowTitle
            // 
            this.ui_leftWindowTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ui_leftWindowTitle.BackgroundImage")));
            this.ui_leftWindowTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_leftWindowTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_leftWindowTitle.IsActive = false;
            this.ui_leftWindowTitle.Location = new System.Drawing.Point(30, 0);
            this.ui_leftWindowTitle.Margin = new System.Windows.Forms.Padding(0);
            this.ui_leftWindowTitle.Name = "ui_leftWindowTitle";
            this.ui_leftWindowTitle.Size = new System.Drawing.Size(260, 22);
            this.ui_leftWindowTitle.TabIndex = 7;
            this.ui_leftWindowTitle.Title = "Table Of Contents";
            this.ui_leftWindowTitle.VisibleCloseButton = false;
            this.ui_leftWindowTitle.CloseClicked += new System.EventHandler(this.LeftWindowTitle_CloseClicked);
            // 
            // ui_leftWinContainerPanel
            // 
            this.ui_leftWinContainerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_leftWinContainerPanel.Controls.Add(this.ui_newTocControl);
            this.ui_leftWinContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_leftWinContainerPanel.Location = new System.Drawing.Point(30, 22);
            this.ui_leftWinContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ui_leftWinContainerPanel.Name = "ui_leftWinContainerPanel";
            this.ui_leftWinContainerPanel.Size = new System.Drawing.Size(260, 460);
            this.ui_leftWinContainerPanel.TabIndex = 6;
            this.ui_leftWinContainerPanel.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.LeftWinContainerPanel_ControlRemoved);
            // 
            // ui_newTocControl
            // 
            this.ui_newTocControl.Location = new System.Drawing.Point(9, 26);
            this.ui_newTocControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_newTocControl.Name = "ui_newTocControl";
            this.ui_newTocControl.Size = new System.Drawing.Size(67, 74);
            this.ui_newTocControl.TabIndex = 0;
            this.ui_newTocControl.Enter += new System.EventHandler(this.LeftControl_Enter);
            this.ui_newTocControl.Leave += new System.EventHandler(this.LeftControl_Leave);
            // 
            // ui_leftWindowToolStrip
            // 
            this.ui_leftWindowToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.ui_leftWindowToolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ui_leftWindowToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ui_leftWindowToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ui_leftWindowToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_tocTSB});
            this.ui_leftWindowToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ui_leftWindowToolStrip.Name = "ui_leftWindowToolStrip";
            this.ui_leftWindowToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ui_leftTableLayoutPanel.SetRowSpan(this.ui_leftWindowToolStrip, 2);
            this.ui_leftWindowToolStrip.Size = new System.Drawing.Size(30, 482);
            this.ui_leftWindowToolStrip.TabIndex = 5;
            this.ui_leftWindowToolStrip.Text = "toolStrip2";
            this.ui_leftWindowToolStrip.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            // 
            // ui_tocTSB
            // 
            this.ui_tocTSB.AutoSize = false;
            this.ui_tocTSB.Checked = true;
            this.ui_tocTSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_tocTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ui_tocTSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_tocTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_tocTSB.Image")));
            this.ui_tocTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_tocTSB.Name = "ui_tocTSB";
            this.ui_tocTSB.Size = new System.Drawing.Size(29, 70);
            this.ui_tocTSB.Text = "TOC";
            this.ui_tocTSB.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            this.ui_tocTSB.CheckedChanged += new System.EventHandler(this.LeftWindowButton_CheckedChanged);
            this.ui_tocTSB.Click += new System.EventHandler(this.TocTSB_Click);
            // 
            // ui_esriMapControl
            // 
            this.ui_esriMapControl.Location = new System.Drawing.Point(29, 72);
            this.ui_esriMapControl.Name = "ui_esriMapControl";
            this.ui_esriMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriMapControl.OcxState")));
            this.ui_esriMapControl.Size = new System.Drawing.Size(354, 237);
            this.ui_esriMapControl.TabIndex = 0;
            this.ui_esriMapControl.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.ui_esriMapControl_OnMouseDown);
            this.ui_esriMapControl.OnMouseUp += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseUpEventHandler(this.uiEvents_esriMapControl_OnMouseUp);
            this.ui_esriMapControl.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.uiEvents_esriMapControl_OnMouseMove);
            this.ui_esriMapControl.OnExtentUpdated += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(this.EsriMapControl_OnExtentUpdated);
            this.ui_esriMapControl.Resize += new System.EventHandler(this.EsriMapControl_Resize);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(29, 34);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 6;
            // 
            // ui_mainToolStripContainer
            // 
            this.ui_mainToolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // ui_mainToolStripContainer.ContentPanel
            // 
            this.ui_mainToolStripContainer.ContentPanel.Controls.Add(this.ui_mainSplitContainer);
            this.ui_mainToolStripContainer.ContentPanel.Size = new System.Drawing.Size(1084, 482);
            this.ui_mainToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_mainToolStripContainer.LeftToolStripPanelVisible = false;
            this.ui_mainToolStripContainer.Location = new System.Drawing.Point(0, 58);
            this.ui_mainToolStripContainer.Name = "ui_mainToolStripContainer";
            this.ui_mainToolStripContainer.RightToolStripPanelVisible = false;
            this.ui_mainToolStripContainer.Size = new System.Drawing.Size(1084, 507);
            this.ui_mainToolStripContainer.TabIndex = 4;
            this.ui_mainToolStripContainer.Text = "toolStripContainer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 590);
            this.Controls.Add(this.ui_mainToolStripContainer);
            this.Controls.Add(this.ui_topPanel);
            this.Controls.Add(this.ui_mainStatusStrip);
            this.Controls.Add(this.ui_mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ui_mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "                              ";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ui_mainMenuStrip.ResumeLayout(false);
            this.ui_mainMenuStrip.PerformLayout();
            this.ui_mainStatusStrip.ResumeLayout(false);
            this.ui_mainStatusStrip.PerformLayout();
            this.ui_topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriDrawToolbarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapToolbarControl)).EndInit();
            this.ui_mainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).EndInit();
            this.ui_mainSplitContainer.ResumeLayout(false);
            this.ui_leftTableLayoutPanel.ResumeLayout(false);
            this.ui_leftTableLayoutPanel.PerformLayout();
            this.ui_leftWinContainerPanel.ResumeLayout(false);
            this.ui_leftWindowToolStrip.ResumeLayout(false);
            this.ui_leftWindowToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ui_mainToolStripContainer.ContentPanel.ResumeLayout(false);
            this.ui_mainToolStripContainer.ResumeLayout(false);
            this.ui_mainToolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Controls.TOCControl ui_newTocControl;
        private System.Windows.Forms.Panel ui_topPanel;
        private System.Windows.Forms.ToolStrip ui_leftWindowToolStrip;
        private System.Windows.Forms.ToolStripButton ui_tocTSB;
        private System.Windows.Forms.Panel ui_leftWinContainerPanel;
        private Controls.LeftWindowTitle ui_leftWindowTitle;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl ui_esriMapToolbarControl;
        private System.Windows.Forms.TableLayoutPanel ui_leftTableLayoutPanel;
		private System.Windows.Forms.ToolStripMenuItem ui_viewHelpTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_testTSMI;
		private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem test3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel ui_serveStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel ui_dbStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel ui_userNameStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel ui_effectiveDateStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem ui_exportToGDBTSMI;
        private ESRI.ArcGIS.Controls.AxToolbarControl ui_esriDrawToolbarControl;
        private System.Windows.Forms.ToolStripStatusLabel ui_cacheStatusLabel;
        private System.Windows.Forms.ToolStripContainer ui_mainToolStripContainer;
        private System.Windows.Forms.ToolStripMenuItem ui_showToolbarsTSMI;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem ui_cadasMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_workPackageManagerTSMI;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem ui_exportLayersToAixmTSMI;
    }
}

