
namespace Aran.PANDA.RNAV.DMECoverage
{
	partial class MainForm
	{

		// Clean up any resources being used.
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.editMinimalAltitude = new System.Windows.Forms.TextBox();
            this.PageControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbExcludeWide = new System.Windows.Forms.CheckBox();
            this.cbExcludePrecision = new System.Windows.Forms.CheckBox();
            this.cbExcludeNarrow = new System.Windows.Forms.CheckBox();
            this.cbExcludeILS = new System.Windows.Forms.CheckBox();
            this.label101 = new System.Windows.Forms.Label();
            this.comboBox101 = new System.Windows.Forms.ComboBox();
            this.rgCoverageAtSelected = new System.Windows.Forms.GroupBox();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonPolygon = new System.Windows.Forms.RadioButton();
            this.radioButtonSegment = new System.Windows.Forms.RadioButton();
            this.radioButtonPoint = new System.Windows.Forms.RadioButton();
            this.labelMinimalAltitude = new System.Windows.Forms.Label();
            this.labelMinimalAltitudeUnit = new System.Windows.Forms.Label();
            this.labelPointList = new System.Windows.Forms.Label();
            this.labelPointType = new System.Windows.Forms.Label();
            this.labOperationalCoverage = new System.Windows.Forms.Label();
            this.labOperationalCoverageUnit = new System.Windows.Forms.Label();
            this.cbPointList = new System.Windows.Forms.ComboBox();
            this.editOperationalCoverage = new System.Windows.Forms.TextBox();
            this.listAvailableFacilities = new System.Windows.Forms.ListView();
            this._column_0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._column_1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._column_2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbExclude = new System.Windows.Forms.CheckBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.AddBtn = new System.Windows.Forms.Button();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listAvailablePairs = new System.Windows.Forms.ListView();
            this._column_9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._column_7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._column_8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.listThreeDmePairs = new System.Windows.Forms.ListView();
            this.columnDme = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelAvailableFacilities = new System.Windows.Forms.Label();
            this.cb2DME = new System.Windows.Forms.CheckBox();
            this.cb3DME = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listCriticalDME = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PrevBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.LoadDME = new System.Windows.Forms.OpenFileDialog();
            this.SaveDME = new System.Windows.Forms.SaveFileDialog();
            this.Gauge1 = new System.Windows.Forms.ProgressBar();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.PageControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.rgCoverageAtSelected.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // editMinimalAltitude
            // 
            this.editMinimalAltitude.Location = new System.Drawing.Point(117, 144);
            this.editMinimalAltitude.Name = "editMinimalAltitude";
            this.editMinimalAltitude.Size = new System.Drawing.Size(76, 20);
            this.editMinimalAltitude.TabIndex = 1;
            this.editMinimalAltitude.Text = "100";
            this.toolTip1.SetToolTip(this.editMinimalAltitude, "Min: 100 m\r\nMax: 8043 m");
            this.editMinimalAltitude.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editMinimalAltitude_KeyPress);
            this.editMinimalAltitude.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBoxes_PreviewKeyDown);
            this.editMinimalAltitude.Validating += new System.ComponentModel.CancelEventHandler(this.editMinimalAltitude_Validating);
            // 
            // PageControl
            // 
            this.PageControl.Controls.Add(this.tabPage1);
            this.PageControl.Controls.Add(this.tabPage2);
            this.PageControl.Controls.Add(this.tabPage3);
            this.PageControl.Location = new System.Drawing.Point(12, 12);
            this.PageControl.Name = "PageControl";
            this.PageControl.SelectedIndex = 0;
            this.PageControl.Size = new System.Drawing.Size(599, 425);
            this.PageControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbExcludeWide);
            this.tabPage1.Controls.Add(this.cbExcludePrecision);
            this.tabPage1.Controls.Add(this.cbExcludeNarrow);
            this.tabPage1.Controls.Add(this.cbExcludeILS);
            this.tabPage1.Controls.Add(this.label101);
            this.tabPage1.Controls.Add(this.comboBox101);
            this.tabPage1.Controls.Add(this.rgCoverageAtSelected);
            this.tabPage1.Controls.Add(this.labelMinimalAltitude);
            this.tabPage1.Controls.Add(this.labelMinimalAltitudeUnit);
            this.tabPage1.Controls.Add(this.labelPointList);
            this.tabPage1.Controls.Add(this.labelPointType);
            this.tabPage1.Controls.Add(this.labOperationalCoverage);
            this.tabPage1.Controls.Add(this.labOperationalCoverageUnit);
            this.tabPage1.Controls.Add(this.editMinimalAltitude);
            this.tabPage1.Controls.Add(this.cbPointList);
            this.tabPage1.Controls.Add(this.editOperationalCoverage);
            this.tabPage1.Controls.Add(this.listAvailableFacilities);
            this.tabPage1.Controls.Add(this.cbExclude);
            this.tabPage1.Controls.Add(this.GroupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(591, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Conditions";
            // 
            // cbExcludeWide
            // 
            this.cbExcludeWide.AutoSize = true;
            this.cbExcludeWide.Location = new System.Drawing.Point(11, 342);
            this.cbExcludeWide.Name = "cbExcludeWide";
            this.cbExcludeWide.Size = new System.Drawing.Size(129, 17);
            this.cbExcludeWide.TabIndex = 250;
            this.cbExcludeWide.Text = "Uncheck wide DME\'s";
            this.cbExcludeWide.UseVisualStyleBackColor = true;
            this.cbExcludeWide.CheckedChanged += new System.EventHandler(this.cbExcludeWide_CheckedChanged);
            // 
            // cbExcludePrecision
            // 
            this.cbExcludePrecision.AutoSize = true;
            this.cbExcludePrecision.Location = new System.Drawing.Point(11, 319);
            this.cbExcludePrecision.Name = "cbExcludePrecision";
            this.cbExcludePrecision.Size = new System.Drawing.Size(149, 17);
            this.cbExcludePrecision.TabIndex = 249;
            this.cbExcludePrecision.Text = "Uncheck precision DME\'s";
            this.cbExcludePrecision.UseVisualStyleBackColor = true;
            this.cbExcludePrecision.CheckedChanged += new System.EventHandler(this.cbExcludeWide_CheckedChanged);
            // 
            // cbExcludeNarrow
            // 
            this.cbExcludeNarrow.AutoSize = true;
            this.cbExcludeNarrow.Location = new System.Drawing.Point(11, 296);
            this.cbExcludeNarrow.Name = "cbExcludeNarrow";
            this.cbExcludeNarrow.Size = new System.Drawing.Size(139, 17);
            this.cbExcludeNarrow.TabIndex = 248;
            this.cbExcludeNarrow.Text = "Uncheck narrow DME\'s";
            this.cbExcludeNarrow.UseVisualStyleBackColor = true;
            this.cbExcludeNarrow.CheckedChanged += new System.EventHandler(this.cbExcludeWide_CheckedChanged);
            // 
            // cbExcludeILS
            // 
            this.cbExcludeILS.AutoSize = true;
            this.cbExcludeILS.Location = new System.Drawing.Point(11, 273);
            this.cbExcludeILS.Name = "cbExcludeILS";
            this.cbExcludeILS.Size = new System.Drawing.Size(123, 17);
            this.cbExcludeILS.TabIndex = 247;
            this.cbExcludeILS.Text = "Uncheck ILS DME\'s";
            this.cbExcludeILS.UseVisualStyleBackColor = true;
            this.cbExcludeILS.CheckedChanged += new System.EventHandler(this.cbExcludeWide_CheckedChanged);
            // 
            // label101
            // 
            this.label101.AutoSize = true;
            this.label101.Location = new System.Drawing.Point(11, 246);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(39, 13);
            this.label101.TabIndex = 245;
            this.label101.Text = "Route:";
            // 
            // comboBox101
            // 
            this.comboBox101.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox101.FormattingEnabled = true;
            this.comboBox101.Location = new System.Drawing.Point(106, 246);
            this.comboBox101.Name = "comboBox101";
            this.comboBox101.Size = new System.Drawing.Size(87, 21);
            this.comboBox101.TabIndex = 246;
            this.comboBox101.SelectedIndexChanged += new System.EventHandler(this.comboBox101_SelectedIndexChanged);
            // 
            // rgCoverageAtSelected
            // 
            this.rgCoverageAtSelected.Controls.Add(this.radioButtonAll);
            this.rgCoverageAtSelected.Controls.Add(this.radioButtonPolygon);
            this.rgCoverageAtSelected.Controls.Add(this.radioButtonSegment);
            this.rgCoverageAtSelected.Controls.Add(this.radioButtonPoint);
            this.rgCoverageAtSelected.Location = new System.Drawing.Point(10, 12);
            this.rgCoverageAtSelected.Name = "rgCoverageAtSelected";
            this.rgCoverageAtSelected.Size = new System.Drawing.Size(186, 117);
            this.rgCoverageAtSelected.TabIndex = 10;
            this.rgCoverageAtSelected.TabStop = false;
            this.rgCoverageAtSelected.Text = "DME coverage at selected";
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Location = new System.Drawing.Point(11, 86);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(61, 17);
            this.radioButtonAll.TabIndex = 3;
            this.radioButtonAll.Text = "All pairs";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.CheckedChanged += new System.EventHandler(this.rgCoverageAtSelectedClick);
            // 
            // radioButtonPolygon
            // 
            this.radioButtonPolygon.AutoSize = true;
            this.radioButtonPolygon.Location = new System.Drawing.Point(11, 63);
            this.radioButtonPolygon.Name = "radioButtonPolygon";
            this.radioButtonPolygon.Size = new System.Drawing.Size(63, 17);
            this.radioButtonPolygon.TabIndex = 2;
            this.radioButtonPolygon.Text = "Polygon";
            this.radioButtonPolygon.UseVisualStyleBackColor = true;
            this.radioButtonPolygon.CheckedChanged += new System.EventHandler(this.rgCoverageAtSelectedClick);
            // 
            // radioButtonSegment
            // 
            this.radioButtonSegment.AutoSize = true;
            this.radioButtonSegment.Location = new System.Drawing.Point(11, 40);
            this.radioButtonSegment.Name = "radioButtonSegment";
            this.radioButtonSegment.Size = new System.Drawing.Size(67, 17);
            this.radioButtonSegment.TabIndex = 1;
            this.radioButtonSegment.Text = "Segment";
            this.radioButtonSegment.UseVisualStyleBackColor = true;
            this.radioButtonSegment.CheckedChanged += new System.EventHandler(this.rgCoverageAtSelectedClick);
            // 
            // radioButtonPoint
            // 
            this.radioButtonPoint.AutoSize = true;
            this.radioButtonPoint.Checked = true;
            this.radioButtonPoint.Location = new System.Drawing.Point(11, 17);
            this.radioButtonPoint.Name = "radioButtonPoint";
            this.radioButtonPoint.Size = new System.Drawing.Size(49, 17);
            this.radioButtonPoint.TabIndex = 0;
            this.radioButtonPoint.TabStop = true;
            this.radioButtonPoint.Text = "Point";
            this.radioButtonPoint.UseVisualStyleBackColor = true;
            this.radioButtonPoint.CheckedChanged += new System.EventHandler(this.rgCoverageAtSelectedClick);
            // 
            // labelMinimalAltitude
            // 
            this.labelMinimalAltitude.AutoSize = true;
            this.labelMinimalAltitude.Location = new System.Drawing.Point(11, 148);
            this.labelMinimalAltitude.Name = "labelMinimalAltitude";
            this.labelMinimalAltitude.Size = new System.Drawing.Size(82, 13);
            this.labelMinimalAltitude.TabIndex = 0;
            this.labelMinimalAltitude.Text = "Minimal altitude:";
            // 
            // labelMinimalAltitudeUnit
            // 
            this.labelMinimalAltitudeUnit.AutoSize = true;
            this.labelMinimalAltitudeUnit.Location = new System.Drawing.Point(201, 148);
            this.labelMinimalAltitudeUnit.Name = "labelMinimalAltitudeUnit";
            this.labelMinimalAltitudeUnit.Size = new System.Drawing.Size(15, 13);
            this.labelMinimalAltitudeUnit.TabIndex = 1;
            this.labelMinimalAltitudeUnit.Text = "m";
            // 
            // labelPointList
            // 
            this.labelPointList.AutoSize = true;
            this.labelPointList.Location = new System.Drawing.Point(11, 215);
            this.labelPointList.Name = "labelPointList";
            this.labelPointList.Size = new System.Drawing.Size(57, 13);
            this.labelPointList.TabIndex = 2;
            this.labelPointList.Text = "Ref. Point:";
            // 
            // labelPointType
            // 
            this.labelPointType.AutoSize = true;
            this.labelPointType.Location = new System.Drawing.Point(201, 215);
            this.labelPointType.Name = "labelPointType";
            this.labelPointType.Size = new System.Drawing.Size(32, 13);
            this.labelPointType.TabIndex = 4;
            this.labelPointType.Text = "WPT";
            // 
            // labOperationalCoverage
            // 
            this.labOperationalCoverage.Location = new System.Drawing.Point(11, 172);
            this.labOperationalCoverage.Name = "labOperationalCoverage";
            this.labOperationalCoverage.Size = new System.Drawing.Size(68, 26);
            this.labOperationalCoverage.TabIndex = 6;
            this.labOperationalCoverage.Text = "Operational coverage:";
            // 
            // labOperationalCoverageUnit
            // 
            this.labOperationalCoverageUnit.AutoSize = true;
            this.labOperationalCoverageUnit.Location = new System.Drawing.Point(201, 179);
            this.labOperationalCoverageUnit.Name = "labOperationalCoverageUnit";
            this.labOperationalCoverageUnit.Size = new System.Drawing.Size(15, 13);
            this.labOperationalCoverageUnit.TabIndex = 7;
            this.labOperationalCoverageUnit.Text = "m";
            // 
            // cbPointList
            // 
            this.cbPointList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPointList.ItemHeight = 13;
            this.cbPointList.Location = new System.Drawing.Point(106, 212);
            this.cbPointList.Name = "cbPointList";
            this.cbPointList.Size = new System.Drawing.Size(87, 21);
            this.cbPointList.TabIndex = 2;
            this.cbPointList.SelectedIndexChanged += new System.EventHandler(this.cbPointList_SelectedIndexChanged);
            // 
            // editOperationalCoverage
            // 
            this.editOperationalCoverage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.editOperationalCoverage.Location = new System.Drawing.Point(117, 175);
            this.editOperationalCoverage.Name = "editOperationalCoverage";
            this.editOperationalCoverage.ReadOnly = true;
            this.editOperationalCoverage.Size = new System.Drawing.Size(76, 20);
            this.editOperationalCoverage.TabIndex = 5;
            // 
            // listAvailableFacilities
            // 
            this.listAvailableFacilities.CheckBoxes = true;
            this.listAvailableFacilities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._column_0,
            this._column_1,
            this._column_2});
            this.listAvailableFacilities.FullRowSelect = true;
            this.listAvailableFacilities.GridLines = true;
            this.listAvailableFacilities.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listAvailableFacilities.HideSelection = false;
            this.listAvailableFacilities.LabelWrap = false;
            this.listAvailableFacilities.Location = new System.Drawing.Point(280, 5);
            this.listAvailableFacilities.Name = "listAvailableFacilities";
            this.listAvailableFacilities.Size = new System.Drawing.Size(307, 337);
            this.listAvailableFacilities.TabIndex = 6;
            this.listAvailableFacilities.UseCompatibleStateImageBehavior = false;
            this.listAvailableFacilities.View = System.Windows.Forms.View.Details;
            this.listAvailableFacilities.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listAvailableFacilities_ItemChecked);
            // 
            // _column_0
            // 
            this._column_0.Text = "DME";
            this._column_0.Width = 75;
            // 
            // _column_1
            // 
            this._column_1.Text = "Distance";
            this._column_1.Width = 92;
            // 
            // _column_2
            // 
            this._column_2.Text = "Min. covering altitude";
            this._column_2.Width = 121;
            // 
            // cbExclude
            // 
            this.cbExclude.AutoSize = true;
            this.cbExclude.Location = new System.Drawing.Point(11, 365);
            this.cbExclude.Name = "cbExclude";
            this.cbExclude.Size = new System.Drawing.Size(267, 17);
            this.cbExclude.TabIndex = 7;
            this.cbExclude.Text = "Exclude DME pairs that are not covering Ref. Point";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.AddBtn);
            this.GroupBox1.Controls.Add(this.LoadBtn);
            this.GroupBox1.Controls.Add(this.SaveBtn);
            this.GroupBox1.Location = new System.Drawing.Point(280, 344);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(307, 49);
            this.GroupBox1.TabIndex = 8;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "New DME";
            // 
            // AddBtn
            // 
            this.AddBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AddBtn.Location = new System.Drawing.Point(8, 16);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(97, 25);
            this.AddBtn.TabIndex = 0;
            this.AddBtn.Text = "Add...";
            this.AddBtn.UseVisualStyleBackColor = false;
            this.AddBtn.Click += new System.EventHandler(this.AddBtnClick);
            // 
            // LoadBtn
            // 
            this.LoadBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.LoadBtn.Location = new System.Drawing.Point(106, 16);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(97, 25);
            this.LoadBtn.TabIndex = 1;
            this.LoadBtn.Text = "Load from file...";
            this.LoadBtn.UseVisualStyleBackColor = false;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtnClick);
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.SaveBtn.Location = new System.Drawing.Point(204, 16);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(97, 25);
            this.SaveBtn.TabIndex = 2;
            this.SaveBtn.Text = "Save to file...";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtnClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl1);
            this.tabPage2.Controls.Add(this.labelAvailableFacilities);
            this.tabPage2.Controls.Add(this.cb2DME);
            this.tabPage2.Controls.Add(this.cb3DME);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(591, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Coverage";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(245, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(293, 368);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listAvailablePairs);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(285, 342);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "2 DME stations";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listAvailablePairs
            // 
            this.listAvailablePairs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._column_9,
            this._column_7,
            this._column_8});
            this.listAvailablePairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAvailablePairs.FullRowSelect = true;
            this.listAvailablePairs.GridLines = true;
            this.listAvailablePairs.HideSelection = false;
            this.listAvailablePairs.LabelWrap = false;
            this.listAvailablePairs.Location = new System.Drawing.Point(3, 3);
            this.listAvailablePairs.Name = "listAvailablePairs";
            this.listAvailablePairs.Size = new System.Drawing.Size(279, 336);
            this.listAvailablePairs.TabIndex = 0;
            this.listAvailablePairs.UseCompatibleStateImageBehavior = false;
            this.listAvailablePairs.View = System.Windows.Forms.View.Details;
            this.listAvailablePairs.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listAvailablePairs_ItemSelectionChanged);
            // 
            // _column_3
            // 
            this._column_9.Name = "_column_9";
            this._column_9.Text = "DME 1";
            this._column_9.Width = 67;
            // 
            // _column_4
            // 
            this._column_7.Name = "_column_7";
            this._column_7.Text = "DME 2";
            this._column_7.Width = 66;
            // 
            // _column_5
            // 
            this._column_8.Name = "_column_8";
            this._column_8.Text = "Distance";
            this._column_8.Width = 79;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.listThreeDmePairs);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(285, 342);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "More than 3 DME stations";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // listThreeDmePairs
            // 
            this.listThreeDmePairs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDme,
            this.columnHeader});
            this.listThreeDmePairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listThreeDmePairs.FullRowSelect = true;
            this.listThreeDmePairs.GridLines = true;
            this.listThreeDmePairs.HideSelection = false;
            this.listThreeDmePairs.LabelWrap = false;
            this.listThreeDmePairs.Location = new System.Drawing.Point(3, 3);
            this.listThreeDmePairs.Name = "listThreeDmePairs";
            this.listThreeDmePairs.Size = new System.Drawing.Size(279, 336);
            this.listThreeDmePairs.TabIndex = 1;
            this.listThreeDmePairs.UseCompatibleStateImageBehavior = false;
            this.listThreeDmePairs.View = System.Windows.Forms.View.Details;
            this.listThreeDmePairs.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listThreeDmePairs_ItemSelectionChanged);
            // 
            // columnDme
            // 
            this.columnDme.Name = "_column_DME";
            this.columnDme.Text = "DME pair 1";
            this.columnDme.Width = 100;
            // 
            // columnHeader
            // 
            this.columnHeader.Name = "_column_4";
            this.columnHeader.Text = "DME pair 2";
            this.columnHeader.Width = 100;
            // 
            // labelAvailableFacilities
            // 
            this.labelAvailableFacilities.AutoSize = true;
            this.labelAvailableFacilities.Location = new System.Drawing.Point(11, 12);
            this.labelAvailableFacilities.Name = "labelAvailableFacilities";
            this.labelAvailableFacilities.Size = new System.Drawing.Size(105, 13);
            this.labelAvailableFacilities.TabIndex = 0;
            this.labelAvailableFacilities.Text = "Available DME pairs:";
            // 
            // cb2DME
            // 
            this.cb2DME.AutoSize = true;
            this.cb2DME.Location = new System.Drawing.Point(11, 227);
            this.cb2DME.Name = "cb2DME";
            this.cb2DME.Size = new System.Drawing.Size(180, 17);
            this.cb2DME.TabIndex = 1;
            this.cb2DME.Text = "Full coverage for 2 DME stations";
            this.cb2DME.CheckedChanged += new System.EventHandler(this.cb2DME_CheckedChanged);
            // 
            // cb3DME
            // 
            this.cb3DME.AutoSize = true;
            this.cb3DME.Location = new System.Drawing.Point(11, 260);
            this.cb3DME.Name = "cb3DME";
            this.cb3DME.Size = new System.Drawing.Size(180, 17);
            this.cb3DME.TabIndex = 2;
            this.cb3DME.Text = "Full coverage for 3 DME stations";
            this.cb3DME.CheckedChanged += new System.EventHandler(this.cb3DME_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox4);
            this.tabPage3.Controls.Add(this.checkBox3);
            this.tabPage3.Controls.Add(this.checkBox2);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.listCriticalDME);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(591, 399);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Critical DME stations";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(16, 202);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(180, 17);
            this.checkBox4.TabIndex = 4;
            this.checkBox4.Text = "Full coverage for 2 DME stations";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(16, 102);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(82, 17);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "Critical Area";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.drawFlagsChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(16, 66);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(197, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Full coverage without selected DME";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.drawFlagsChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(16, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(168, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Selected DME pairs coverage";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.drawFlagsChanged);
            // 
            // listCriticalDME
            // 
            this.listCriticalDME.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listCriticalDME.FullRowSelect = true;
            this.listCriticalDME.GridLines = true;
            this.listCriticalDME.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listCriticalDME.HideSelection = false;
            this.listCriticalDME.LabelWrap = false;
            this.listCriticalDME.Location = new System.Drawing.Point(245, 0);
            this.listCriticalDME.MultiSelect = false;
            this.listCriticalDME.Name = "listCriticalDME";
            this.listCriticalDME.Size = new System.Drawing.Size(245, 396);
            this.listCriticalDME.TabIndex = 0;
            this.listCriticalDME.UseCompatibleStateImageBehavior = false;
            this.listCriticalDME.View = System.Windows.Forms.View.Details;
            this.listCriticalDME.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listCriticalDME_ItemSelectionChanged);
            this.listCriticalDME.SelectedIndexChanged += new System.EventHandler(this.listCriticalDME_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "DME";
            this.columnHeader1.Width = 99;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Pairs";
            this.columnHeader2.Width = 79;
            // 
            // PrevBtn
            // 
            this.PrevBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PrevBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PrevBtn.Enabled = false;
            this.PrevBtn.Location = new System.Drawing.Point(319, 535);
            this.PrevBtn.Name = "PrevBtn";
            this.PrevBtn.Size = new System.Drawing.Size(75, 25);
            this.PrevBtn.TabIndex = 1;
            this.PrevBtn.Text = "<- Prev";
            this.PrevBtn.UseVisualStyleBackColor = false;
            this.PrevBtn.Click += new System.EventHandler(this.PrevBtnClick);
            // 
            // NextBtn
            // 
            this.NextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NextBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.NextBtn.Location = new System.Drawing.Point(407, 535);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 25);
            this.NextBtn.TabIndex = 2;
            this.NextBtn.Text = "Next ->";
            this.NextBtn.UseVisualStyleBackColor = false;
            this.NextBtn.Click += new System.EventHandler(this.NextBtnClick);
            // 
            // LoadDME
            // 
            this.LoadDME.DefaultExt = "lst";
            this.LoadDME.Filter = "DME list (*.lst)|*.lst|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // SaveDME
            // 
            this.SaveDME.DefaultExt = "lst";
            this.SaveDME.Filter = "DME list (*.lst)|*.lst|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // Gauge1
            // 
            this.Gauge1.Location = new System.Drawing.Point(261, 411);
            this.Gauge1.Name = "Gauge1";
            this.Gauge1.Size = new System.Drawing.Size(336, 21);
            this.Gauge1.TabIndex = 3;
            this.Gauge1.Visible = false;
            // 
            // ExportBtn
            // 
            this.ExportBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExportBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExportBtn.Location = new System.Drawing.Point(498, 535);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ExportBtn.Size = new System.Drawing.Size(75, 25);
            this.ExportBtn.TabIndex = 4;
            this.ExportBtn.Text = "Ok";
            this.ExportBtn.UseVisualStyleBackColor = false;
            this.ExportBtn.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // MainForm
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(916, 567);
            this.Controls.Add(this.ExportBtn);
            this.Controls.Add(this.Gauge1);
            this.Controls.Add(this.PageControl);
            this.Controls.Add(this.PrevBtn);
            this.Controls.Add(this.NextBtn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(582, 144);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "DME/DME Coverage";
            this.Closed += new System.EventHandler(this.FormClose);
            this.PageControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.rgCoverageAtSelected.ResumeLayout(false);
            this.rgCoverageAtSelected.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ProgressBar Gauge1;
		private System.Windows.Forms.TabControl PageControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;

		private System.Windows.Forms.Label labelMinimalAltitude;
		private System.Windows.Forms.Label labelMinimalAltitudeUnit;
		private System.Windows.Forms.Label labelPointList;
		private System.Windows.Forms.Label labelPointType;
		private System.Windows.Forms.Label labOperationalCoverage;
		private System.Windows.Forms.Label labOperationalCoverageUnit;
		private System.Windows.Forms.TextBox editMinimalAltitude;
		private System.Windows.Forms.ComboBox cbPointList;
		private System.Windows.Forms.TextBox editOperationalCoverage;

		private System.Windows.Forms.ColumnHeader _column_0;
		private System.Windows.Forms.ColumnHeader _column_1;
		private System.Windows.Forms.ColumnHeader _column_2;
		private System.Windows.Forms.ColumnHeader _column_9;
		private System.Windows.Forms.ColumnHeader _column_7;
		private System.Windows.Forms.ColumnHeader _column_8;

		private System.Windows.Forms.CheckBox cbExcludeILS;
		private System.Windows.Forms.CheckBox cbExcludeNarrow;
		private System.Windows.Forms.CheckBox cbExcludePrecision;
		private System.Windows.Forms.CheckBox cbExcludeWide;
		private System.Windows.Forms.CheckBox cbExclude;

		private System.Windows.Forms.GroupBox GroupBox1;
		private System.Windows.Forms.Button AddBtn;
		private System.Windows.Forms.Button LoadBtn;
		private System.Windows.Forms.Button SaveBtn;
		private System.Windows.Forms.Label labelAvailableFacilities;
		private System.Windows.Forms.ListView listAvailablePairs;
		private System.Windows.Forms.CheckBox cb2DME;
		private System.Windows.Forms.CheckBox cb3DME;
		private System.Windows.Forms.Button PrevBtn;
		private System.Windows.Forms.Button NextBtn;
		//private  TParameter prmMinimalAltitude;
		private System.Windows.Forms.OpenFileDialog LoadDME;
		private System.Windows.Forms.SaveFileDialog SaveDME;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox rgCoverageAtSelected;
		private System.Windows.Forms.RadioButton radioButtonPolygon;
		private System.Windows.Forms.RadioButton radioButtonSegment;
		private System.Windows.Forms.RadioButton radioButtonPoint;
		private System.Windows.Forms.Label label101;
		private System.Windows.Forms.ComboBox comboBox101;
		private System.Windows.Forms.ListView listAvailableFacilities;
		private System.Windows.Forms.RadioButton radioButtonAll;
		private System.Windows.Forms.ListView listCriticalDME;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView listThreeDmePairs;
        private System.Windows.Forms.ColumnHeader columnDme;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.Button ExportBtn;
    }
}

