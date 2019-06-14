using System.Windows.Forms;
using PDM.PropertyExtension;
using System.Drawing;


namespace ARENA
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.aRENAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pANDAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nOTAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDocAS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.decimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalDegreesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilometersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.kilometersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.millimetersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nauticalMilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.milesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yardsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.feetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.WizardTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.FeatureTreeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.layerPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.FeatureTreeViewToolStrip = new System.Windows.Forms.ToolStrip();
            this.LayerPropertiesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ShowOnMapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomToObjectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.EditObjectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DeleteObjectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.PlayButton = new System.Windows.Forms.ToolStripButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.readOnlyPropertyGrid = new PDM.PropertyExtension.ReadOnlyPropertyGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.coordinatSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapControlContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.CustomToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ðóññêèéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.FeatureTreeViewContextMenu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.FeatureTreeViewToolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.mapControlContextMenuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.CustomToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1335, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewDoc,
            this.menuOpenDoc,
            this.menuSaveDoc,
            this.menuSaveDocAS,
            this.menuSaveAs,
            this.menuSeparator,
            this.menuExitApp});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(35, 20);
            this.menuFile.Text = "File";
            // 
            // menuNewDoc
            // 
            this.menuNewDoc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aRENAToolStripMenuItem,
            this.pANDAToolStripMenuItem,
            this.nOTAMToolStripMenuItem});
            this.menuNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuNewDoc.Image")));
            this.menuNewDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuNewDoc.Name = "menuNewDoc";
            this.menuNewDoc.Size = new System.Drawing.Size(152, 22);
            this.menuNewDoc.Text = "New Project";
            // 
            // aRENAToolStripMenuItem
            // 
            this.aRENAToolStripMenuItem.Name = "aRENAToolStripMenuItem";
            this.aRENAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aRENAToolStripMenuItem.Text = "ARENA";
            this.aRENAToolStripMenuItem.Click += new System.EventHandler(this.ARenaToolStripMenuItemClick);
            // 
            // pANDAToolStripMenuItem
            // 
            this.pANDAToolStripMenuItem.Name = "pANDAToolStripMenuItem";
            this.pANDAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pANDAToolStripMenuItem.Text = "PANDA";
            this.pANDAToolStripMenuItem.Click += new System.EventHandler(this.pANDAToolStripMenuItem_Click);
            // 
            // nOTAMToolStripMenuItem
            // 
            this.nOTAMToolStripMenuItem.Name = "nOTAMToolStripMenuItem";
            this.nOTAMToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.nOTAMToolStripMenuItem.Text = "NOTAM";
            this.nOTAMToolStripMenuItem.Click += new System.EventHandler(this.NotamToolStripMenuItemClick);
            // 
            // menuOpenDoc
            // 
            this.menuOpenDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenDoc.Image")));
            this.menuOpenDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuOpenDoc.Name = "menuOpenDoc";
            this.menuOpenDoc.Size = new System.Drawing.Size(152, 22);
            this.menuOpenDoc.Text = "Open project...";
            this.menuOpenDoc.Click += new System.EventHandler(this.MenuOpenDocClick);
            // 
            // menuSaveDoc
            // 
            this.menuSaveDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDoc.Image")));
            this.menuSaveDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDoc.Name = "menuSaveDoc";
            this.menuSaveDoc.Size = new System.Drawing.Size(152, 22);
            this.menuSaveDoc.Text = "Save project";
            this.menuSaveDoc.Click += new System.EventHandler(this.MenuSaveDocClick);
            // 
            // menuSaveDocAS
            // 
            this.menuSaveDocAS.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDocAS.Image")));
            this.menuSaveDocAS.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDocAS.Name = "menuSaveDocAS";
            this.menuSaveDocAS.Size = new System.Drawing.Size(152, 22);
            this.menuSaveDocAS.Text = "Save project as";
            this.menuSaveDocAS.Click += new System.EventHandler(this.menuSaveDocAS_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Enabled = false;
            this.menuSaveAs.Image = global::ARENA.Properties.Resources.ArcMap16;
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(152, 22);
            this.menuSaveAs.Text = "Export to MXD";
            this.menuSaveAs.Click += new System.EventHandler(this.MenuSaveAsClick);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // menuExitApp
            // 
            this.menuExitApp.Image = global::ARENA.Properties.Resources.CatalogHome16;
            this.menuExitApp.Name = "menuExitApp";
            this.menuExitApp.Size = new System.Drawing.Size(152, 22);
            this.menuExitApp.Text = "Exit";
            this.menuExitApp.Click += new System.EventHandler(this.MenuExitAppClick);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(3, 648);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 54);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 649);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY,
            this.toolStripDropDownButton1});
            this.statusStrip.Location = new System.Drawing.Point(3, 680);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1332, 23);
            this.statusStrip.Stretch = false;
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(49, 18);
            this.statusBarXY.Text = "Test 123";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decimetersToolStripMenuItem,
            this.decimalDegreesToolStripMenuItem,
            this.kilometersToolStripMenuItem1,
            this.kilometersToolStripMenuItem,
            this.centimetersToolStripMenuItem,
            this.millimetersToolStripMenuItem,
            this.nauticalMilesToolStripMenuItem,
            this.milesToolStripMenuItem,
            this.yardsToolStripMenuItem,
            this.feetToolStripMenuItem,
            this.pointsToolStripMenuItem,
            this.inchesToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(21, 21);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStripDropDownButton1DropDownItemClicked);
            // 
            // decimetersToolStripMenuItem
            // 
            this.decimetersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("decimetersToolStripMenuItem.Image")));
            this.decimetersToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.decimetersToolStripMenuItem.Name = "decimetersToolStripMenuItem";
            this.decimetersToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.decimetersToolStripMenuItem.Text = "Decimeters";
            // 
            // decimalDegreesToolStripMenuItem
            // 
            this.decimalDegreesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("decimalDegreesToolStripMenuItem.Image")));
            this.decimalDegreesToolStripMenuItem.Name = "decimalDegreesToolStripMenuItem";
            this.decimalDegreesToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.decimalDegreesToolStripMenuItem.Text = "DecimalDegrees";
            // 
            // kilometersToolStripMenuItem1
            // 
            this.kilometersToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("kilometersToolStripMenuItem1.Image")));
            this.kilometersToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.kilometersToolStripMenuItem1.Name = "kilometersToolStripMenuItem1";
            this.kilometersToolStripMenuItem1.Size = new System.Drawing.Size(151, 24);
            this.kilometersToolStripMenuItem1.Text = "Kilometers";
            // 
            // kilometersToolStripMenuItem
            // 
            this.kilometersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("kilometersToolStripMenuItem.Image")));
            this.kilometersToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.kilometersToolStripMenuItem.Name = "kilometersToolStripMenuItem";
            this.kilometersToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.kilometersToolStripMenuItem.Text = "Meters";
            // 
            // centimetersToolStripMenuItem
            // 
            this.centimetersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("centimetersToolStripMenuItem.Image")));
            this.centimetersToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.centimetersToolStripMenuItem.Name = "centimetersToolStripMenuItem";
            this.centimetersToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.centimetersToolStripMenuItem.Text = "Centimeters";
            // 
            // millimetersToolStripMenuItem
            // 
            this.millimetersToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("millimetersToolStripMenuItem.Image")));
            this.millimetersToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.millimetersToolStripMenuItem.Name = "millimetersToolStripMenuItem";
            this.millimetersToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.millimetersToolStripMenuItem.Text = "Millimeters";
            // 
            // nauticalMilesToolStripMenuItem
            // 
            this.nauticalMilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("nauticalMilesToolStripMenuItem.Image")));
            this.nauticalMilesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.nauticalMilesToolStripMenuItem.Name = "nauticalMilesToolStripMenuItem";
            this.nauticalMilesToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.nauticalMilesToolStripMenuItem.Text = "NauticalMiles";
            // 
            // milesToolStripMenuItem
            // 
            this.milesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("milesToolStripMenuItem.Image")));
            this.milesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.milesToolStripMenuItem.Name = "milesToolStripMenuItem";
            this.milesToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.milesToolStripMenuItem.Text = "Miles";
            // 
            // yardsToolStripMenuItem
            // 
            this.yardsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("yardsToolStripMenuItem.Image")));
            this.yardsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.yardsToolStripMenuItem.Name = "yardsToolStripMenuItem";
            this.yardsToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.yardsToolStripMenuItem.Text = "Yards";
            // 
            // feetToolStripMenuItem
            // 
            this.feetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("feetToolStripMenuItem.Image")));
            this.feetToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.feetToolStripMenuItem.Name = "feetToolStripMenuItem";
            this.feetToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.feetToolStripMenuItem.Text = "Feet";
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pointsToolStripMenuItem.Image")));
            this.pointsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.pointsToolStripMenuItem.Text = "Points";
            // 
            // inchesToolStripMenuItem
            // 
            this.inchesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("inchesToolStripMenuItem.Image")));
            this.inchesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.inchesToolStripMenuItem.Name = "inchesToolStripMenuItem";
            this.inchesToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.inchesToolStripMenuItem.Text = "Inches";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 54);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.FeatureTreeViewToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1332, 594);
            this.splitContainer1.SplitterDistance = 328;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 10;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(326, 477);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(318, 449);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.FeatureTreeViewContextMenu;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.TreeViewImageList;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(312, 443);
            this.treeView1.TabIndex = 11;
            // 
            // FeatureTreeViewContextMenu
            // 
            this.FeatureTreeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layerPropertiesToolStripMenuItem,
            this.showOnMapToolStripMenuItem,
            this.zoomToObjectToolStripMenuItem,
            this.editObjectToolStripMenuItem,
            this.deleteObjectToolStripMenuItem});
            this.FeatureTreeViewContextMenu.Name = "contextMenuStrip3";
            this.FeatureTreeViewContextMenu.Size = new System.Drawing.Size(154, 114);
            // 
            // layerPropertiesToolStripMenuItem
            // 
            this.layerPropertiesToolStripMenuItem.Name = "layerPropertiesToolStripMenuItem";
            this.layerPropertiesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.layerPropertiesToolStripMenuItem.Text = "Layer properties";
            this.layerPropertiesToolStripMenuItem.Click += new System.EventHandler(this.LayerPropertiesToolStripMenuItemClick);
            // 
            // showOnMapToolStripMenuItem
            // 
            this.showOnMapToolStripMenuItem.Name = "showOnMapToolStripMenuItem";
            this.showOnMapToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.showOnMapToolStripMenuItem.Text = "Show on Map";
            this.showOnMapToolStripMenuItem.Click += new System.EventHandler(this.ShowOnMapToolStripMenuItemClick);
            // 
            // zoomToObjectToolStripMenuItem
            // 
            this.zoomToObjectToolStripMenuItem.Name = "zoomToObjectToolStripMenuItem";
            this.zoomToObjectToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.zoomToObjectToolStripMenuItem.Text = "Zoom to Object";
            this.zoomToObjectToolStripMenuItem.Visible = false;
            this.zoomToObjectToolStripMenuItem.Click += new System.EventHandler(this.ZoomToObjectToolStripMenuItemClick);
            // 
            // editObjectToolStripMenuItem
            // 
            this.editObjectToolStripMenuItem.Name = "editObjectToolStripMenuItem";
            this.editObjectToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.editObjectToolStripMenuItem.Text = "Edit Object";
            this.editObjectToolStripMenuItem.Visible = false;
            this.editObjectToolStripMenuItem.Click += new System.EventHandler(this.EditObjectToolStripMenuItemClick);
            // 
            // deleteObjectToolStripMenuItem
            // 
            this.deleteObjectToolStripMenuItem.Name = "deleteObjectToolStripMenuItem";
            this.deleteObjectToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteObjectToolStripMenuItem.Text = "Delete Object";
            this.deleteObjectToolStripMenuItem.Click += new System.EventHandler(this.DeleteObjectToolStripMenuItemClick);
            // 
            // TreeViewImageList
            // 
            this.TreeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeViewImageList.ImageStream")));
            this.TreeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeViewImageList.Images.SetKeyName(0, "Flag.bmp");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.axTOCControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(318, 451);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Layers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(312, 445);
            this.axTOCControl1.TabIndex = 6;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            this.axTOCControl1.OnDoubleClick += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnDoubleClickEventHandler(this.axTOCControl1_OnDoubleClick);
            // 
            // FeatureTreeViewToolStrip
            // 
            this.FeatureTreeViewToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FeatureTreeViewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.FeatureTreeViewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LayerPropertiesToolStripButton,
            this.ShowOnMapToolStripButton,
            this.ZoomToObjectToolStripButton,
            this.EditObjectToolStripButton,
            this.DeleteObjectToolStripButton,
            this.PlayButton});
            this.FeatureTreeViewToolStrip.Location = new System.Drawing.Point(0, 477);
            this.FeatureTreeViewToolStrip.Name = "FeatureTreeViewToolStrip";
            this.FeatureTreeViewToolStrip.Size = new System.Drawing.Size(326, 25);
            this.FeatureTreeViewToolStrip.TabIndex = 14;
            this.FeatureTreeViewToolStrip.Text = "toolStrip1";
            // 
            // LayerPropertiesToolStripButton
            // 
            this.LayerPropertiesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.LayerPropertiesToolStripButton.Image = global::ARENA.Properties.Resources.settings;
            this.LayerPropertiesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LayerPropertiesToolStripButton.Name = "LayerPropertiesToolStripButton";
            this.LayerPropertiesToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.LayerPropertiesToolStripButton.Text = "Layer properties";
            this.LayerPropertiesToolStripButton.Click += new System.EventHandler(this.LayerPropertiesToolStripMenuItemClick);
            // 
            // ShowOnMapToolStripButton
            // 
            this.ShowOnMapToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowOnMapToolStripButton.Image = global::ARENA.Properties.Resources.search;
            this.ShowOnMapToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowOnMapToolStripButton.Name = "ShowOnMapToolStripButton";
            this.ShowOnMapToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.ShowOnMapToolStripButton.Text = "Show on map";
            this.ShowOnMapToolStripButton.Click += new System.EventHandler(this.ShowOnMapToolStripMenuItemClick);
            // 
            // ZoomToObjectToolStripButton
            // 
            this.ZoomToObjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomToObjectToolStripButton.Image = global::ARENA.Properties.Resources.zoom_in;
            this.ZoomToObjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomToObjectToolStripButton.Name = "ZoomToObjectToolStripButton";
            this.ZoomToObjectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.ZoomToObjectToolStripButton.Text = "Zoom to selected object";
            this.ZoomToObjectToolStripButton.Visible = false;
            this.ZoomToObjectToolStripButton.Click += new System.EventHandler(this.ZoomToObjectToolStripMenuItemClick);
            // 
            // EditObjectToolStripButton
            // 
            this.EditObjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditObjectToolStripButton.Image = global::ARENA.Properties.Resources.edit;
            this.EditObjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditObjectToolStripButton.Name = "EditObjectToolStripButton";
            this.EditObjectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.EditObjectToolStripButton.Text = "Edit selected object";
            this.EditObjectToolStripButton.Visible = false;
            this.EditObjectToolStripButton.Click += new System.EventHandler(this.EditObjectToolStripMenuItemClick);
            // 
            // DeleteObjectToolStripButton
            // 
            this.DeleteObjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteObjectToolStripButton.Image = global::ARENA.Properties.Resources.cancel;
            this.DeleteObjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteObjectToolStripButton.Name = "DeleteObjectToolStripButton";
            this.DeleteObjectToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.DeleteObjectToolStripButton.Text = "Delete selected object";
            this.DeleteObjectToolStripButton.Click += new System.EventHandler(this.DeleteObjectToolStripMenuItemClick);
            // 
            // PlayButton
            // 
            this.PlayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PlayButton.Image = ((System.Drawing.Image)(resources.GetObject("PlayButton.Image")));
            this.PlayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(23, 22);
            this.PlayButton.Text = "toolStripButton1";
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linkLabel1.Location = new System.Drawing.Point(0, 502);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(326, 15);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Quick search";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 517);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(326, 75);
            this.panel1.TabIndex = 13;
            this.panel1.Visible = false;
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "ARP",
            "RWY",
            "ILS",
            "VOR",
            "VOR/DME",
            "VOR/TACAN",
            "DME",
            "TACAN",
            "NDB",
            "WYP"});
            this.comboBox2.Location = new System.Drawing.Point(78, 40);
            this.comboBox2.MaxDropDownItems = 10;
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(197, 23);
            this.comboBox2.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.button2.Image = global::ARENA.Properties.Resources.search;
            this.button2.Location = new System.Drawing.Point(283, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 27);
            this.button2.TabIndex = 11;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click1);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Identifier = ";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search In";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "",
            "Airport",
            "Navaid",
            "WayPoint",
            "VerticalStructure"});
            this.comboBox1.Location = new System.Drawing.Point(78, 5);
            this.comboBox1.MaxDropDownItems = 10;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(197, 23);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.axMapControl1);
            this.splitContainer2.Panel1.Controls.Add(this.button1);
            this.splitContainer2.Panel1.Controls.Add(this.button3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.readOnlyPropertyGrid);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Size = new System.Drawing.Size(997, 592);
            this.splitContainer2.SplitterDistance = 773;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 11;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(17, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(739, 592);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(756, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(17, 592);
            this.button1.TabIndex = 3;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.button3.Dock = System.Windows.Forms.DockStyle.Left;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(17, 592);
            this.button3.TabIndex = 16;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // readOnlyPropertyGrid
            // 
            this.readOnlyPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.readOnlyPropertyGrid.Location = new System.Drawing.Point(0, 17);
            this.readOnlyPropertyGrid.Name = "readOnlyPropertyGrid";
            this.readOnlyPropertyGrid.ReadOnly = true;
            this.readOnlyPropertyGrid.Size = new System.Drawing.Size(219, 575);
            this.readOnlyPropertyGrid.TabIndex = 0;
            this.readOnlyPropertyGrid.ViewForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.readOnlyPropertyGrid.SelectedObjectsChanged += new System.EventHandler(this.readOnlyPropertyGrid_SelectedObjectsChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(78)))), ((int)(((byte)(109)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 17);
            this.label3.TabIndex = 1;
            this.label3.Visible = false;
            // 
            // coordinatSystemToolStripMenuItem
            // 
            this.coordinatSystemToolStripMenuItem.Name = "coordinatSystemToolStripMenuItem";
            this.coordinatSystemToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.coordinatSystemToolStripMenuItem.Text = "Coordinat System";
            this.coordinatSystemToolStripMenuItem.Click += new System.EventHandler(this.CoordinatSystemToolStripMenuItemClick);
            // 
            // mapControlContextMenuStrip
            // 
            this.mapControlContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.mapControlContextMenuStrip.Name = "contextMenuStrip1";
            this.mapControlContextMenuStrip.Size = new System.Drawing.Size(160, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItem1.Text = "Coordinat System";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.CoordinatSystemToolStripMenuItemClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.CustomToolStrip);
            this.panel2.Controls.Add(this.axToolbarControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1335, 30);
            this.panel2.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(528, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(807, 30);
            this.panel3.TabIndex = 6;
            // 
            // CustomToolStrip
            // 
            this.CustomToolStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CustomToolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.CustomToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.CustomToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton2});
            this.CustomToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.CustomToolStrip.Location = new System.Drawing.Point(442, 0);
            this.CustomToolStrip.Name = "CustomToolStrip";
            this.CustomToolStrip.Size = new System.Drawing.Size(86, 30);
            this.CustomToolStrip.TabIndex = 5;
            this.CustomToolStrip.Text = "toolStrip2";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.ðóññêèéToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(83, 27);
            this.toolStripDropDownButton2.Text = "Language";
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.englishToolStripMenuItem.Tag = "1033";
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // ðóññêèéToolStripMenuItem
            // 
            this.ðóññêèéToolStripMenuItem.Name = "ðóññêèéToolStripMenuItem";
            this.ðóññêèéToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.ðóññêèéToolStripMenuItem.Tag = "1049";
            this.ðóññêèéToolStripMenuItem.Text = "Ðóññêèé";
            this.ðóññêèéToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(442, 28);
            this.axToolbarControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 703);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ARENA v1.11";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeView1KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.FeatureTreeViewContextMenu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.FeatureTreeViewToolStrip.ResumeLayout(false);
            this.FeatureTreeViewToolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.mapControlContextMenuStrip.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.CustomToolStrip.ResumeLayout(false);
            this.CustomToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNewDoc;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExitApp;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.ToolStripMenuItem aRENAToolStripMenuItem;
  
        private ImageList TreeViewImageList;


        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem showOnMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layerPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinatSystemToolStripMenuItem;
      
        private System.Windows.Forms.ContextMenuStrip FeatureTreeViewContextMenu;
        private System.Windows.Forms.ContextMenuStrip mapControlContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem yardsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem feetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centimetersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem millimetersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nauticalMilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem milesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kilometersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimetersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalDegreesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kilometersToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteObjectToolStripMenuItem;
        private ReadOnlyPropertyGrid readOnlyPropertyGrid;
        private System.Windows.Forms.ToolStripMenuItem nOTAMToolStripMenuItem;
        public System.Windows.Forms.TreeView treeView1;
        private ToolStrip FeatureTreeViewToolStrip;
        private ToolStripButton LayerPropertiesToolStripButton;
        private ToolStripButton ShowOnMapToolStripButton;
        private ToolStripButton ZoomToObjectToolStripButton;
        private ToolStripButton EditObjectToolStripButton;
        private ToolStripButton DeleteObjectToolStripButton;
        private Panel panel2;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private WizardTabControl tabControl1;
        private Button button3;
        private ToolStripButton PlayButton;
        private Label label3;
        private ToolStripMenuItem pANDAToolStripMenuItem;
        private ToolStripMenuItem menuSaveDocAS;
        private ToolStrip CustomToolStrip;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem ðóññêèéToolStripMenuItem;
        private Panel panel3;
        //private ReadOnlyPropertyGrid readOnlyPropertyGrid1;

        public System.Windows.Forms.TreeViewEventHandler NotamtreeView_AfterSelect { get; set; }
    }


}


namespace System.Windows.Forms
{
    public class WizardTabControl : TabControl
    {
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x1328 && !DesignMode) // Hide tabs by trapping the TCM_ADJUSTRECT message
        //        m.Result = IntPtr.Zero;
        //    else
        //        base.WndProc(ref m);
        //}
    }
}
