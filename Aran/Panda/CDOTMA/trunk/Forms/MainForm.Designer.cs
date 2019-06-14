namespace CDOTMA
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
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewPrijectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unitSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lateralSeparationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.conflictDetectingToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.intersectingTracksWithCommonPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.intersectingTracksWithoutCommonPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nonintersectingSIDAndSTARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.conceptualDesignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.departureRoutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.arrivalRoutesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enrouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbNONE = new System.Windows.Forms.ToolStripButton();
			this.tsbHandFlat = new System.Windows.Forms.ToolStripButton();
			this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
			this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
			this.tsbPrevExtend = new System.Windows.Forms.ToolStripButton();
			this.tsbNextExtend = new System.Windows.Forms.ToolStripButton();
			this.tsbEdit = new System.Windows.Forms.ToolStripButton();
			this.tsbMeasureDistance = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusLabelLat = new System.Windows.Forms.ToolStripStatusLabel();
			this.StatusLabelLong = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.StatusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.viewControl1 = new CDOTMA.Controls.ViewControl();
			this.menuStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.conflictDetectingToolsToolStripMenuItem,
            this.conceptualDesignToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(746, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.closeToolStripMenuItem.Text = "&Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewPrijectionToolStripMenuItem,
            this.unitSettingsMenuItem,
            this.lateralSeparationToolStripMenuItem});
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.settingsToolStripMenuItem.Text = "&Settings";
			// 
			// viewPrijectionToolStripMenuItem
			// 
			this.viewPrijectionToolStripMenuItem.Name = "viewPrijectionToolStripMenuItem";
			this.viewPrijectionToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.viewPrijectionToolStripMenuItem.Text = "View projection...";
			this.viewPrijectionToolStripMenuItem.Click += new System.EventHandler(this.viewProjectionToolStripMenuItem_Click);
			// 
			// unitSettingsMenuItem
			// 
			this.unitSettingsMenuItem.Name = "unitSettingsMenuItem";
			this.unitSettingsMenuItem.Size = new System.Drawing.Size(176, 22);
			this.unitSettingsMenuItem.Text = "Unit settings...";
			this.unitSettingsMenuItem.Click += new System.EventHandler(this.unitSettingsMenuItem_Click);
			// 
			// lateralSeparationToolStripMenuItem
			// 
			this.lateralSeparationToolStripMenuItem.Name = "lateralSeparationToolStripMenuItem";
			this.lateralSeparationToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.lateralSeparationToolStripMenuItem.Text = "&Lateral separation...";
			this.lateralSeparationToolStripMenuItem.Click += new System.EventHandler(this.lateralSeparationToolStripMenuItem_Click);
			// 
			// conflictDetectingToolsToolStripMenuItem
			// 
			this.conflictDetectingToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intersectingTracksWithCommonPointToolStripMenuItem,
            this.intersectingTracksWithoutCommonPointToolStripMenuItem,
            this.nonintersectingSIDAndSTARToolStripMenuItem});
			this.conflictDetectingToolsToolStripMenuItem.Name = "conflictDetectingToolsToolStripMenuItem";
			this.conflictDetectingToolsToolStripMenuItem.Size = new System.Drawing.Size(143, 20);
			this.conflictDetectingToolsToolStripMenuItem.Text = "Conflict detecting &tools";
			// 
			// intersectingTracksWithCommonPointToolStripMenuItem
			// 
			this.intersectingTracksWithCommonPointToolStripMenuItem.Name = "intersectingTracksWithCommonPointToolStripMenuItem";
			this.intersectingTracksWithCommonPointToolStripMenuItem.Size = new System.Drawing.Size(304, 22);
			this.intersectingTracksWithCommonPointToolStripMenuItem.Text = "Intersecting tracks with common point";
			this.intersectingTracksWithCommonPointToolStripMenuItem.Click += new System.EventHandler(this.intersectingTracksWithCommonPointToolStripMenuItem_Click);
			// 
			// intersectingTracksWithoutCommonPointToolStripMenuItem
			// 
			this.intersectingTracksWithoutCommonPointToolStripMenuItem.Name = "intersectingTracksWithoutCommonPointToolStripMenuItem";
			this.intersectingTracksWithoutCommonPointToolStripMenuItem.Size = new System.Drawing.Size(304, 22);
			this.intersectingTracksWithoutCommonPointToolStripMenuItem.Text = "Intersecting tracks without common point";
			this.intersectingTracksWithoutCommonPointToolStripMenuItem.Click += new System.EventHandler(this.intersectingTracksWithoutCommonPointToolStripMenuItem_Click);
			// 
			// nonintersectingSIDAndSTARToolStripMenuItem
			// 
			this.nonintersectingSIDAndSTARToolStripMenuItem.Name = "nonintersectingSIDAndSTARToolStripMenuItem";
			this.nonintersectingSIDAndSTARToolStripMenuItem.Size = new System.Drawing.Size(304, 22);
			this.nonintersectingSIDAndSTARToolStripMenuItem.Text = "Non-intersecting SID, STAR and  ATS routes";
			this.nonintersectingSIDAndSTARToolStripMenuItem.Click += new System.EventHandler(this.nonintersectingSIDAndSTARToolStripMenuItem_Click);
			// 
			// conceptualDesignToolStripMenuItem
			// 
			this.conceptualDesignToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.departureRoutesToolStripMenuItem,
            this.arrivalRoutesToolStripMenuItem,
            this.enrouteToolStripMenuItem});
			this.conceptualDesignToolStripMenuItem.Name = "conceptualDesignToolStripMenuItem";
			this.conceptualDesignToolStripMenuItem.Size = new System.Drawing.Size(118, 20);
			this.conceptualDesignToolStripMenuItem.Text = "Conceptual &design";
			// 
			// departureRoutesToolStripMenuItem
			// 
			this.departureRoutesToolStripMenuItem.Name = "departureRoutesToolStripMenuItem";
			this.departureRoutesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.departureRoutesToolStripMenuItem.Text = "Departure routes";
			this.departureRoutesToolStripMenuItem.Click += new System.EventHandler(this.departureRoutesToolStripMenuItem_Click);
			// 
			// arrivalRoutesToolStripMenuItem
			// 
			this.arrivalRoutesToolStripMenuItem.Name = "arrivalRoutesToolStripMenuItem";
			this.arrivalRoutesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.arrivalRoutesToolStripMenuItem.Text = "Arrival routes";
			this.arrivalRoutesToolStripMenuItem.Click += new System.EventHandler(this.arrivalRoutesToolStripMenuItem_Click);
			// 
			// enrouteToolStripMenuItem
			// 
			this.enrouteToolStripMenuItem.Name = "enrouteToolStripMenuItem";
			this.enrouteToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
			this.enrouteToolStripMenuItem.Text = "En-route";
			this.enrouteToolStripMenuItem.Click += new System.EventHandler(this.enrouteToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNONE,
            this.tsbHandFlat,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.tsbPrevExtend,
            this.tsbNextExtend,
            this.tsbEdit,
            this.tsbMeasureDistance});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(746, 29);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
			// 
			// tsbNONE
			// 
			this.tsbNONE.CheckOnClick = true;
			this.tsbNONE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNONE.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbNONE.Name = "tsbNONE";
			this.tsbNONE.Size = new System.Drawing.Size(23, 26);
			this.tsbNONE.Tag = 0;
			this.tsbNONE.CheckedChanged += new System.EventHandler(this.toolStrip1_ItemCheckedChanged);
			// 
			// tsbHandFlat
			// 
			this.tsbHandFlat.CheckOnClick = true;
			this.tsbHandFlat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbHandFlat.Image = global::CDOTMA.Properties.Resources.bmpHandFlat;
			this.tsbHandFlat.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbHandFlat.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbHandFlat.Name = "tsbHandFlat";
			this.tsbHandFlat.Size = new System.Drawing.Size(25, 26);
			this.tsbHandFlat.Tag = 1;
			this.tsbHandFlat.CheckedChanged += new System.EventHandler(this.toolStrip1_ItemCheckedChanged);
			// 
			// tsbZoomIn
			// 
			this.tsbZoomIn.CheckOnClick = true;
			this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbZoomIn.Image = global::CDOTMA.Properties.Resources.bmpZoomIn;
			this.tsbZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbZoomIn.Name = "tsbZoomIn";
			this.tsbZoomIn.Size = new System.Drawing.Size(26, 26);
			this.tsbZoomIn.Tag = 2;
			this.tsbZoomIn.CheckedChanged += new System.EventHandler(this.toolStrip1_ItemCheckedChanged);
			// 
			// tsbZoomOut
			// 
			this.tsbZoomOut.CheckOnClick = true;
			this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbZoomOut.Image = global::CDOTMA.Properties.Resources.bmpZoomOut;
			this.tsbZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbZoomOut.Name = "tsbZoomOut";
			this.tsbZoomOut.Size = new System.Drawing.Size(26, 26);
			this.tsbZoomOut.Tag = 3;
			this.tsbZoomOut.CheckedChanged += new System.EventHandler(this.toolStrip1_ItemCheckedChanged);
			// 
			// tsbPrevExtend
			// 
			this.tsbPrevExtend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbPrevExtend.Enabled = false;
			this.tsbPrevExtend.Image = global::CDOTMA.Properties.Resources.arrow_left;
			this.tsbPrevExtend.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbPrevExtend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbPrevExtend.Name = "tsbPrevExtend";
			this.tsbPrevExtend.Size = new System.Drawing.Size(24, 26);
			this.tsbPrevExtend.Tag = 20;
			// 
			// tsbNextExtend
			// 
			this.tsbNextExtend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNextExtend.Enabled = false;
			this.tsbNextExtend.Image = global::CDOTMA.Properties.Resources.arrow_right;
			this.tsbNextExtend.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbNextExtend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbNextExtend.Name = "tsbNextExtend";
			this.tsbNextExtend.Size = new System.Drawing.Size(24, 26);
			this.tsbNextExtend.Tag = 21;
			// 
			// tsbEdit
			// 
			this.tsbEdit.CheckOnClick = true;
			this.tsbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbEdit.Image = global::CDOTMA.Properties.Resources.bmpEdit;
			this.tsbEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbEdit.Name = "tsbEdit";
			this.tsbEdit.Size = new System.Drawing.Size(25, 26);
			this.tsbEdit.Tag = 4;
			this.tsbEdit.CheckedChanged += new System.EventHandler(this.toolStrip1_ItemCheckedChanged);
			// 
			// tsbMeasureDistance
			// 
			this.tsbMeasureDistance.CheckOnClick = true;
			this.tsbMeasureDistance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbMeasureDistance.Image = global::CDOTMA.Properties.Resources.measured;
			this.tsbMeasureDistance.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbMeasureDistance.ImageTransparentColor = System.Drawing.Color.Silver;
			this.tsbMeasureDistance.Name = "tsbMeasureDistance";
			this.tsbMeasureDistance.Size = new System.Drawing.Size(23, 26);
			this.tsbMeasureDistance.Text = "toolStripButton1";
			this.tsbMeasureDistance.CheckedChanged += new System.EventHandler(this.tsbMeasureDistance_CheckedChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabelLat,
            this.StatusLabelLong,
            this.toolStripProgressBar1,
            this.StatusLabelInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 465);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(746, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusLabelLat
			// 
			this.StatusLabelLat.AutoSize = false;
			this.StatusLabelLat.Name = "StatusLabelLat";
			this.StatusLabelLat.Size = new System.Drawing.Size(110, 17);
			// 
			// StatusLabelLong
			// 
			this.StatusLabelLong.AutoSize = false;
			this.StatusLabelLong.Name = "StatusLabelLong";
			this.StatusLabelLong.Size = new System.Drawing.Size(110, 17);
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 16);
			this.toolStripProgressBar1.Visible = false;
			// 
			// StatusLabelInfo
			// 
			this.StatusLabelInfo.Name = "StatusLabelInfo";
			this.StatusLabelInfo.Size = new System.Drawing.Size(0, 17);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "PDM files (*.pdm)|*.pdm|All files (*.*)|*.*";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "PDM files (*.pdm)|*.pdm|All files (*.*)|*.*";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Controls.Add(this.treeView1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 53);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(131, 412);
			this.panel1.TabIndex = 7;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBox2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBox3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.checkBox4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.checkBox5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.checkBox7, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.checkBox6, 0, 5);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 249);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(125, 160);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(3, 3);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(66, 17);
			this.checkBox1.TabIndex = 5;
			this.checkBox1.Text = "Aeroport";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new System.Drawing.Point(3, 26);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(67, 17);
			this.checkBox2.TabIndex = 6;
			this.checkBox2.Text = "Airspace";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.Location = new System.Drawing.Point(3, 49);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(98, 17);
			this.checkBox3.TabIndex = 7;
			this.checkBox3.Text = "Approach Legs";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.Location = new System.Drawing.Point(3, 72);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(81, 17);
			this.checkBox4.TabIndex = 8;
			this.checkBox4.Text = "Arrival Legs";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.Location = new System.Drawing.Point(3, 95);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(99, 17);
			this.checkBox5.TabIndex = 9;
			this.checkBox5.Text = "Departure Legs";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox7
			// 
			this.checkBox7.AutoSize = true;
			this.checkBox7.Checked = true;
			this.checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox7.Location = new System.Drawing.Point(3, 141);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(51, 16);
			this.checkBox7.TabIndex = 11;
			this.checkBox7.Text = "WPT";
			this.checkBox7.UseVisualStyleBackColor = true;
			this.checkBox7.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Checked = true;
			this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox6.Location = new System.Drawing.Point(3, 118);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(77, 17);
			this.checkBox6.TabIndex = 10;
			this.checkBox6.Text = "Route legs";
			this.checkBox6.UseVisualStyleBackColor = true;
			this.checkBox6.Click += new System.EventHandler(this.checkBox1_Click);
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(131, 246);
			this.treeView1.TabIndex = 11;
			this.treeView1.Visible = false;
			// 
			// viewControl1
			// 
			this.viewControl1.BackColor = System.Drawing.SystemColors.Control;
			this.viewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewControl1.Location = new System.Drawing.Point(0, 53);
			this.viewControl1.Mode = CDOTMA.Controls.ViewerMode.None;
			this.viewControl1.Name = "viewControl1";
			this.viewControl1.Size = new System.Drawing.Size(746, 412);
			this.viewControl1.TabIndex = 6;
			this.viewControl1.ProjectionChanged += new System.EventHandler(this.viewControl1_ProjectionChanged);
			this.viewControl1.ViewChanged += new System.EventHandler(this.viewControl1_ViewChanged);
			this.viewControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewControl1_MouseDown);
			this.viewControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.viewControl1_MouseMove);
			this.viewControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.viewControl1_MouseUp);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(746, 487);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.viewControl1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "PBN Implementation Support Tool";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripButton tsbHandFlat;
		private System.Windows.Forms.ToolStripButton tsbZoomIn;
		private System.Windows.Forms.ToolStripButton tsbZoomOut;
		private System.Windows.Forms.ToolStripButton tsbNONE;
		private System.Windows.Forms.ToolStripButton tsbEdit;
		private System.Windows.Forms.ToolStripButton tsbPrevExtend;
		private System.Windows.Forms.ToolStripButton tsbNextExtend;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabelLat;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabelLong;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripMenuItem conflictDetectingToolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem intersectingTracksWithCommonPointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem intersectingTracksWithoutCommonPointToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nonintersectingSIDAndSTARToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem conceptualDesignToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem departureRoutesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem arrivalRoutesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem enrouteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lateralSeparationToolStripMenuItem;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ToolStripMenuItem viewPrijectionToolStripMenuItem;
		public Controls.ViewControl viewControl1;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.ToolStripMenuItem unitSettingsMenuItem;
		private System.Windows.Forms.ToolStripButton tsbMeasureDistance;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabelInfo;
	}
}

