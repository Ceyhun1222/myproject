namespace Aran.Aim.InputForm
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
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_NavigationPanel = new System.Windows.Forms.Panel();
            this.ui_navFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_FeatureTypesPanel = new System.Windows.Forms.Panel();
            this.ui_quickSearchTextBox = new System.Windows.Forms.TextBox();
            this.ui_FeatureTypesDGV = new System.Windows.Forms.DataGridView();
            this.ui_FeatureNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_FeaturesPanel = new System.Windows.Forms.Panel();
            this.ui_searchPanel = new System.Windows.Forms.Panel();
            this.ui_closeFindPanelButton = new System.Windows.Forms.Button();
            this.ui_findNextButton = new System.Windows.Forms.Button();
            this.ui_searchTB = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_loadingPictureBox = new System.Windows.Forms.PictureBox();
            this.ui_FeaturesDGV = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ui_nextTSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.ui_nextButtonSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ui_refreshTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_newTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_viewTSSB = new System.Windows.Forms.ToolStripSplitButton();
            this.ui_viewTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_viewHistoryTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_editCorrTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_editNewSeqTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_deleteTSMI = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ui_effectiveDateTimePicker = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_effectiveDateLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ui_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_importTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.fromAIXMMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_toolsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_findFeatureTypeTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_quickSearchTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_optionsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_featTypesByClassifiedTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ui_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_addForExpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_addForExportingTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_NavigationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_MainSplitContainer)).BeginInit();
            this.ui_MainSplitContainer.Panel1.SuspendLayout();
            this.ui_MainSplitContainer.Panel2.SuspendLayout();
            this.ui_MainSplitContainer.SuspendLayout();
            this.ui_FeatureTypesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeatureTypesDGV)).BeginInit();
            this.ui_FeaturesPanel.SuspendLayout();
            this.ui_searchPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_loadingPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.ui_addForExpContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_NavigationPanel
            // 
            this.ui_NavigationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_NavigationPanel.Controls.Add(this.ui_navFlowLayoutPanel);
            this.ui_NavigationPanel.Location = new System.Drawing.Point(12, 31);
            this.ui_NavigationPanel.Name = "ui_NavigationPanel";
            this.ui_NavigationPanel.Size = new System.Drawing.Size(1110, 33);
            this.ui_NavigationPanel.TabIndex = 0;
            // 
            // ui_navFlowLayoutPanel
            // 
            this.ui_navFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_navFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_navFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_navFlowLayoutPanel.Name = "ui_navFlowLayoutPanel";
            this.ui_navFlowLayoutPanel.Size = new System.Drawing.Size(1110, 33);
            this.ui_navFlowLayoutPanel.TabIndex = 0;
            this.ui_navFlowLayoutPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.ui_navFlowLayoutPanel_ControlAdded);
            this.ui_navFlowLayoutPanel.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.ui_navFlowLayoutPanel_ControlRemoved);
            // 
            // ui_MainSplitContainer
            // 
            this.ui_MainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_MainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ui_MainSplitContainer.Location = new System.Drawing.Point(12, 70);
            this.ui_MainSplitContainer.Name = "ui_MainSplitContainer";
            // 
            // ui_MainSplitContainer.Panel1
            // 
            this.ui_MainSplitContainer.Panel1.Controls.Add(this.ui_FeatureTypesPanel);
            // 
            // ui_MainSplitContainer.Panel2
            // 
            this.ui_MainSplitContainer.Panel2.Controls.Add(this.ui_FeaturesPanel);
            this.ui_MainSplitContainer.Size = new System.Drawing.Size(1110, 447);
            this.ui_MainSplitContainer.SplitterDistance = 250;
            this.ui_MainSplitContainer.TabIndex = 1;
            // 
            // ui_FeatureTypesPanel
            // 
            this.ui_FeatureTypesPanel.Controls.Add(this.ui_quickSearchTextBox);
            this.ui_FeatureTypesPanel.Controls.Add(this.ui_FeatureTypesDGV);
            this.ui_FeatureTypesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_FeatureTypesPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_FeatureTypesPanel.Name = "ui_FeatureTypesPanel";
            this.ui_FeatureTypesPanel.Size = new System.Drawing.Size(250, 447);
            this.ui_FeatureTypesPanel.TabIndex = 2;
            // 
            // ui_quickSearchTextBox
            // 
            this.ui_quickSearchTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_quickSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_quickSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.ui_quickSearchTextBox.Location = new System.Drawing.Point(0, 0);
            this.ui_quickSearchTextBox.Name = "ui_quickSearchTextBox";
            this.ui_quickSearchTextBox.Size = new System.Drawing.Size(250, 20);
            this.ui_quickSearchTextBox.TabIndex = 7;
            this.ui_quickSearchTextBox.Text = "Quick Search";
            this.ui_quickSearchTextBox.Enter += new System.EventHandler(this.ui_quickFeatTypeSearchTB_Enter);
            this.ui_quickSearchTextBox.Leave += new System.EventHandler(this.ui_quickFeatTypeSearchTB_Leave);
            // 
            // ui_FeatureTypesDGV
            // 
            this.ui_FeatureTypesDGV.AllowUserToAddRows = false;
            this.ui_FeatureTypesDGV.AllowUserToDeleteRows = false;
            this.ui_FeatureTypesDGV.AllowUserToResizeColumns = false;
            this.ui_FeatureTypesDGV.AllowUserToResizeRows = false;
            this.ui_FeatureTypesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_FeatureTypesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_FeatureTypesDGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ui_FeatureTypesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_FeatureTypesDGV.ColumnHeadersVisible = false;
            this.ui_FeatureTypesDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_FeatureNameColumn});
            this.ui_FeatureTypesDGV.Location = new System.Drawing.Point(0, 21);
            this.ui_FeatureTypesDGV.MultiSelect = false;
            this.ui_FeatureTypesDGV.Name = "ui_FeatureTypesDGV";
            this.ui_FeatureTypesDGV.ReadOnly = true;
            this.ui_FeatureTypesDGV.RowHeadersVisible = false;
            this.ui_FeatureTypesDGV.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ui_FeatureTypesDGV.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ui_FeatureTypesDGV.RowTemplate.Height = 30;
            this.ui_FeatureTypesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_FeatureTypesDGV.Size = new System.Drawing.Size(250, 425);
            this.ui_FeatureTypesDGV.TabIndex = 0;
            // 
            // ui_FeatureNameColumn
            // 
            this.ui_FeatureNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_FeatureNameColumn.HeaderText = "Feature Name";
            this.ui_FeatureNameColumn.Name = "ui_FeatureNameColumn";
            this.ui_FeatureNameColumn.ReadOnly = true;
            // 
            // ui_FeaturesPanel
            // 
            this.ui_FeaturesPanel.Controls.Add(this.ui_searchPanel);
            this.ui_FeaturesPanel.Controls.Add(this.panel1);
            this.ui_FeaturesPanel.Controls.Add(this.toolStrip1);
            this.ui_FeaturesPanel.Controls.Add(this.groupBox1);
            this.ui_FeaturesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_FeaturesPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_FeaturesPanel.Name = "ui_FeaturesPanel";
            this.ui_FeaturesPanel.Size = new System.Drawing.Size(856, 447);
            this.ui_FeaturesPanel.TabIndex = 11;
            // 
            // ui_searchPanel
            // 
            this.ui_searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_searchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_searchPanel.Controls.Add(this.ui_closeFindPanelButton);
            this.ui_searchPanel.Controls.Add(this.ui_findNextButton);
            this.ui_searchPanel.Controls.Add(this.ui_searchTB);
            this.ui_searchPanel.Location = new System.Drawing.Point(532, 67);
            this.ui_searchPanel.Name = "ui_searchPanel";
            this.ui_searchPanel.Size = new System.Drawing.Size(297, 37);
            this.ui_searchPanel.TabIndex = 19;
            this.ui_searchPanel.Visible = false;
            this.ui_searchPanel.Leave += new System.EventHandler(this.SearchPanel_Leave);
            // 
            // ui_closeFindPanelButton
            // 
            this.ui_closeFindPanelButton.AutoSize = true;
            this.ui_closeFindPanelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_closeFindPanelButton.Font = new System.Drawing.Font("Webdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ui_closeFindPanelButton.Location = new System.Drawing.Point(262, 3);
            this.ui_closeFindPanelButton.Name = "ui_closeFindPanelButton";
            this.ui_closeFindPanelButton.Size = new System.Drawing.Size(30, 27);
            this.ui_closeFindPanelButton.TabIndex = 5;
            this.ui_closeFindPanelButton.Text = "r";
            this.ui_closeFindPanelButton.UseVisualStyleBackColor = true;
            this.ui_closeFindPanelButton.Click += new System.EventHandler(this.CloseFindPanelLabel_Click);
            // 
            // ui_findNextButton
            // 
            this.ui_findNextButton.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.ui_findNextButton.Location = new System.Drawing.Point(232, 3);
            this.ui_findNextButton.Name = "ui_findNextButton";
            this.ui_findNextButton.Size = new System.Drawing.Size(30, 27);
            this.ui_findNextButton.TabIndex = 4;
            this.ui_findNextButton.Text = "q";
            this.ui_findNextButton.UseVisualStyleBackColor = true;
            this.ui_findNextButton.Click += new System.EventHandler(this.FindNext_Click);
            // 
            // ui_searchTB
            // 
            this.ui_searchTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_searchTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_searchTB.Location = new System.Drawing.Point(3, 6);
            this.ui_searchTB.Name = "ui_searchTB";
            this.ui_searchTB.Size = new System.Drawing.Size(226, 22);
            this.ui_searchTB.TabIndex = 0;
            this.ui_searchTB.TextChanged += new System.EventHandler(this.SearchTB_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ui_loadingPictureBox);
            this.panel1.Controls.Add(this.ui_FeaturesDGV);
            this.panel1.Location = new System.Drawing.Point(4, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(849, 389);
            this.panel1.TabIndex = 20;
            // 
            // ui_loadingPictureBox
            // 
            this.ui_loadingPictureBox.BackColor = System.Drawing.Color.White;
            this.ui_loadingPictureBox.Image = global::Aran.Aim.InputForm.Properties.Resources.loadinfo;
            this.ui_loadingPictureBox.Location = new System.Drawing.Point(302, 146);
            this.ui_loadingPictureBox.Name = "ui_loadingPictureBox";
            this.ui_loadingPictureBox.Size = new System.Drawing.Size(153, 92);
            this.ui_loadingPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ui_loadingPictureBox.TabIndex = 17;
            this.ui_loadingPictureBox.TabStop = false;
            this.ui_loadingPictureBox.Visible = false;
            // 
            // ui_FeaturesDGV
            // 
            this.ui_FeaturesDGV.AllowUserToAddRows = false;
            this.ui_FeaturesDGV.AllowUserToDeleteRows = false;
            this.ui_FeaturesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_FeaturesDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_FeaturesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_FeaturesDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_FeaturesDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_FeaturesDGV.Name = "ui_FeaturesDGV";
            this.ui_FeaturesDGV.ReadOnly = true;
            this.ui_FeaturesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_FeaturesDGV.Size = new System.Drawing.Size(847, 387);
            this.ui_FeaturesDGV.TabIndex = 10;
            this.ui_FeaturesDGV.CurrentCellChanged += new System.EventHandler(this.ui_FeaturesDGV_CurrentCellChanged);
            this.ui_FeaturesDGV.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ui_FeaturesDGV_RowsAdded);
            this.ui_FeaturesDGV.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.ui_FeaturesDGV_RowsRemoved);
            this.ui_FeaturesDGV.SelectionChanged += new System.EventHandler(this.FeaturesDGV_SelectionChanged);
            this.ui_FeaturesDGV.SizeChanged += new System.EventHandler(this.FeaturesDGV_SizeChanged);
            this.ui_FeaturesDGV.VisibleChanged += new System.EventHandler(this.FeaturesDGV_VisibleChanged);
            this.ui_FeaturesDGV.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ui_FeaturesDGV_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_nextTSB,
            this.toolStripLabel2,
            this.ui_nextButtonSeparator,
            this.toolStripLabel1,
            this.ui_refreshTSB,
            this.ui_newTSB,
            this.ui_viewTSSB,
            this.ui_editCorrTSB,
            this.ui_editNewSeqTSB,
            this.ui_deleteTSMI});
            this.toolStrip1.Location = new System.Drawing.Point(5, 5);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(2);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(570, 47);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ui_nextTSB
            // 
            this.ui_nextTSB.Enabled = false;
            this.ui_nextTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_nextTSB.Image")));
            this.ui_nextTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_nextTSB.Name = "ui_nextTSB";
            this.ui_nextTSB.Padding = new System.Windows.Forms.Padding(2);
            this.ui_nextTSB.Size = new System.Drawing.Size(39, 40);
            this.ui_nextTSB.Text = "Next";
            this.ui_nextTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_nextTSB.Click += new System.EventHandler(this.ui_nextButton_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.AutoSize = false;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(10, 41);
            // 
            // ui_nextButtonSeparator
            // 
            this.ui_nextButtonSeparator.Name = "ui_nextButtonSeparator";
            this.ui_nextButtonSeparator.Size = new System.Drawing.Size(6, 43);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(10, 41);
            // 
            // ui_refreshTSB
            // 
            this.ui_refreshTSB.Image = global::Aran.Aim.InputForm.Properties.Resources.refresh_24;
            this.ui_refreshTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_refreshTSB.Name = "ui_refreshTSB";
            this.ui_refreshTSB.Padding = new System.Windows.Forms.Padding(2);
            this.ui_refreshTSB.Size = new System.Drawing.Size(54, 40);
            this.ui_refreshTSB.Text = "Refresh";
            this.ui_refreshTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_refreshTSB.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // ui_newTSB
            // 
            this.ui_newTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_newTSB.Image")));
            this.ui_newTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_newTSB.Name = "ui_newTSB";
            this.ui_newTSB.Padding = new System.Windows.Forms.Padding(2);
            this.ui_newTSB.Size = new System.Drawing.Size(39, 40);
            this.ui_newTSB.Text = "New";
            this.ui_newTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_newTSB.Click += new System.EventHandler(this.ui_newButton_Click);
            // 
            // ui_viewTSSB
            // 
            this.ui_viewTSSB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_viewTSMI,
            this.ui_viewHistoryTSMI});
            this.ui_viewTSSB.Image = global::Aran.Aim.InputForm.Properties.Resources.view_24;
            this.ui_viewTSSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_viewTSSB.Name = "ui_viewTSSB";
            this.ui_viewTSSB.Padding = new System.Windows.Forms.Padding(2);
            this.ui_viewTSSB.Size = new System.Drawing.Size(52, 40);
            this.ui_viewTSSB.Text = "View";
            this.ui_viewTSSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_viewTSSB.ButtonClick += new System.EventHandler(this.View_Click);
            // 
            // ui_viewTSMI
            // 
            this.ui_viewTSMI.Image = global::Aran.Aim.InputForm.Properties.Resources.view_24;
            this.ui_viewTSMI.Name = "ui_viewTSMI";
            this.ui_viewTSMI.Size = new System.Drawing.Size(140, 22);
            this.ui_viewTSMI.Text = "View";
            this.ui_viewTSMI.Click += new System.EventHandler(this.View_Click);
            // 
            // ui_viewHistoryTSMI
            // 
            this.ui_viewHistoryTSMI.Image = global::Aran.Aim.InputForm.Properties.Resources.view_hist_32;
            this.ui_viewHistoryTSMI.Name = "ui_viewHistoryTSMI";
            this.ui_viewHistoryTSMI.Size = new System.Drawing.Size(140, 22);
            this.ui_viewHistoryTSMI.Text = "View History";
            this.ui_viewHistoryTSMI.Click += new System.EventHandler(this.ViewHistory_Click);
            // 
            // ui_editCorrTSB
            // 
            this.ui_editCorrTSB.Enabled = false;
            this.ui_editCorrTSB.Image = global::Aran.Aim.InputForm.Properties.Resources.edit_blue_24;
            this.ui_editCorrTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_editCorrTSB.Name = "ui_editCorrTSB";
            this.ui_editCorrTSB.Size = new System.Drawing.Size(98, 40);
            this.ui_editCorrTSB.Text = "Edit - Correction";
            this.ui_editCorrTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_editCorrTSB.ToolTipText = "Edit As Correction";
            this.ui_editCorrTSB.Click += new System.EventHandler(this.EditAs_Correction_Click);
            // 
            // ui_editNewSeqTSB
            // 
            this.ui_editNewSeqTSB.Enabled = false;
            this.ui_editNewSeqTSB.Image = global::Aran.Aim.InputForm.Properties.Resources.edit_green_24;
            this.ui_editNewSeqTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_editNewSeqTSB.Name = "ui_editNewSeqTSB";
            this.ui_editNewSeqTSB.Size = new System.Drawing.Size(91, 40);
            this.ui_editNewSeqTSB.Text = "Edit - New Seq.";
            this.ui_editNewSeqTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_editNewSeqTSB.ToolTipText = "Edit As New Sequence";
            this.ui_editNewSeqTSB.Click += new System.EventHandler(this.EditAs_NewSequence_Click);
            // 
            // ui_deleteTSMI
            // 
            this.ui_deleteTSMI.Enabled = false;
            this.ui_deleteTSMI.Image = global::Aran.Aim.InputForm.Properties.Resources.delete_icon_24;
            this.ui_deleteTSMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_deleteTSMI.Name = "ui_deleteTSMI";
            this.ui_deleteTSMI.Padding = new System.Windows.Forms.Padding(2);
            this.ui_deleteTSMI.Size = new System.Drawing.Size(98, 40);
            this.ui_deleteTSMI.Text = "Decom. Feature";
            this.ui_deleteTSMI.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ui_deleteTSMI.ToolTipText = "Decomission Feature";
            this.ui_deleteTSMI.Click += new System.EventHandler(this.Delete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ui_effectiveDateTimePicker);
            this.groupBox1.Controls.Add(this.ui_effectiveDateLabel);
            this.groupBox1.Location = new System.Drawing.Point(625, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 51);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // ui_effectiveDateTimePicker
            // 
            this.ui_effectiveDateTimePicker.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_effectiveDateTimePicker.Location = new System.Drawing.Point(7, 25);
            this.ui_effectiveDateTimePicker.Name = "ui_effectiveDateTimePicker";
            this.ui_effectiveDateTimePicker.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_effectiveDateTimePicker.Size = new System.Drawing.Size(208, 25);
            this.ui_effectiveDateTimePicker.TabIndex = 0;
            this.ui_effectiveDateTimePicker.Value = new System.DateTime(((long)(0)));
            this.ui_effectiveDateTimePicker.ValueChanged += new System.EventHandler(this.EffectiveDate_ValueChanged);
            // 
            // ui_effectiveDateLabel
            // 
            this.ui_effectiveDateLabel.AutoSize = true;
            this.ui_effectiveDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_effectiveDateLabel.Location = new System.Drawing.Point(6, 11);
            this.ui_effectiveDateLabel.Name = "ui_effectiveDateLabel";
            this.ui_effectiveDateLabel.Size = new System.Drawing.Size(93, 13);
            this.ui_effectiveDateLabel.TabIndex = 8;
            this.ui_effectiveDateLabel.Text = "Effective Date:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_fileToolStripMenuItem,
            this.ui_toolsTSMI,
            this.ui_helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1134, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ui_fileToolStripMenuItem
            // 
            this.ui_fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_importTSMI,
            this.ui_exportToolStripMenuItem,
            this.toolStripMenuItem2,
            this.ui_exitToolStripMenuItem});
            this.ui_fileToolStripMenuItem.Name = "ui_fileToolStripMenuItem";
            this.ui_fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.ui_fileToolStripMenuItem.Text = "&File";
            // 
            // ui_importTSMI
            // 
            this.ui_importTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromAIXMMessageToolStripMenuItem});
            this.ui_importTSMI.Name = "ui_importTSMI";
            this.ui_importTSMI.Size = new System.Drawing.Size(110, 22);
            this.ui_importTSMI.Text = "Import";
            // 
            // fromAIXMMessageToolStripMenuItem
            // 
            this.fromAIXMMessageToolStripMenuItem.Name = "fromAIXMMessageToolStripMenuItem";
            this.fromAIXMMessageToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.fromAIXMMessageToolStripMenuItem.Text = "From AIXM Message";
            this.fromAIXMMessageToolStripMenuItem.Click += new System.EventHandler(this.ImportFromXml_Click);
            // 
            // ui_exportToolStripMenuItem
            // 
            this.ui_exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toXmlToolStripMenuItem});
            this.ui_exportToolStripMenuItem.Name = "ui_exportToolStripMenuItem";
            this.ui_exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.ui_exportToolStripMenuItem.Text = "Export";
            // 
            // toXmlToolStripMenuItem
            // 
            this.toXmlToolStripMenuItem.Name = "toXmlToolStripMenuItem";
            this.toXmlToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.toXmlToolStripMenuItem.Text = "AIXM Messages";
            this.toXmlToolStripMenuItem.Click += new System.EventHandler(this.ExportToXml_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(107, 6);
            // 
            // ui_exitToolStripMenuItem
            // 
            this.ui_exitToolStripMenuItem.Name = "ui_exitToolStripMenuItem";
            this.ui_exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.ui_exitToolStripMenuItem.Text = "&Exit";
            this.ui_exitToolStripMenuItem.Click += new System.EventHandler(this.ui_exitToolStripMenuItem_Click);
            // 
            // ui_toolsTSMI
            // 
            this.ui_toolsTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_findFeatureTypeTSMI,
            this.ui_quickSearchTSMI,
            this.ui_optionsTSMI,
            this.ui_featTypesByClassifiedTSMI});
            this.ui_toolsTSMI.Name = "ui_toolsTSMI";
            this.ui_toolsTSMI.Size = new System.Drawing.Size(48, 20);
            this.ui_toolsTSMI.Text = "Tools";
            // 
            // ui_findFeatureTypeTSMI
            // 
            this.ui_findFeatureTypeTSMI.Name = "ui_findFeatureTypeTSMI";
            this.ui_findFeatureTypeTSMI.Size = new System.Drawing.Size(218, 22);
            this.ui_findFeatureTypeTSMI.Text = "Find FeatureType";
            this.ui_findFeatureTypeTSMI.Click += new System.EventHandler(this.FindFeatureType_Click);
            // 
            // ui_quickSearchTSMI
            // 
            this.ui_quickSearchTSMI.Name = "ui_quickSearchTSMI";
            this.ui_quickSearchTSMI.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.ui_quickSearchTSMI.Size = new System.Drawing.Size(218, 22);
            this.ui_quickSearchTSMI.Text = "Quick Search";
            this.ui_quickSearchTSMI.Click += new System.EventHandler(this.QuickSearch_Click);
            // 
            // ui_optionsTSMI
            // 
            this.ui_optionsTSMI.Name = "ui_optionsTSMI";
            this.ui_optionsTSMI.Size = new System.Drawing.Size(218, 22);
            this.ui_optionsTSMI.Text = "Options";
            this.ui_optionsTSMI.Click += new System.EventHandler(this.Options_Click);
            // 
            // ui_featTypesByClassifiedTSMI
            // 
            this.ui_featTypesByClassifiedTSMI.Checked = true;
            this.ui_featTypesByClassifiedTSMI.CheckOnClick = true;
            this.ui_featTypesByClassifiedTSMI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_featTypesByClassifiedTSMI.Name = "ui_featTypesByClassifiedTSMI";
            this.ui_featTypesByClassifiedTSMI.Size = new System.Drawing.Size(218, 22);
            this.ui_featTypesByClassifiedTSMI.Text = "FeaturesTypes by Classified";
            this.ui_featTypesByClassifiedTSMI.CheckedChanged += new System.EventHandler(this.FeatTypesByClassified_CheckedChanged);
            // 
            // ui_helpToolStripMenuItem
            // 
            this.ui_helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_aboutToolStripMenuItem});
            this.ui_helpToolStripMenuItem.Name = "ui_helpToolStripMenuItem";
            this.ui_helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.ui_helpToolStripMenuItem.Text = "&Help";
            this.ui_helpToolStripMenuItem.Visible = false;
            // 
            // ui_aboutToolStripMenuItem
            // 
            this.ui_aboutToolStripMenuItem.Name = "ui_aboutToolStripMenuItem";
            this.ui_aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.ui_aboutToolStripMenuItem.Text = "&About";
            this.ui_aboutToolStripMenuItem.Click += new System.EventHandler(this.ui_aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 520);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1134, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ui_statusLabel
            // 
            this.ui_statusLabel.Name = "ui_statusLabel";
            this.ui_statusLabel.Size = new System.Drawing.Size(92, 17);
            this.ui_statusLabel.Text = "Feature count: 0";
            // 
            // ui_addForExpContextMenu
            // 
            this.ui_addForExpContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_addForExportingTSMI});
            this.ui_addForExpContextMenu.Name = "ui_addForExpContextMenu";
            this.ui_addForExpContextMenu.Size = new System.Drawing.Size(168, 26);
            // 
            // ui_addForExportingTSMI
            // 
            this.ui_addForExportingTSMI.Name = "ui_addForExportingTSMI";
            this.ui_addForExportingTSMI.Size = new System.Drawing.Size(167, 22);
            this.ui_addForExportingTSMI.Text = "Add for Exporting";
            this.ui_addForExportingTSMI.Click += new System.EventHandler(this.AddForExporting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 542);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ui_MainSplitContainer);
            this.Controls.Add(this.ui_NavigationPanel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "MainForm";
            this.Text = "AIM Data Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ui_NavigationPanel.ResumeLayout(false);
            this.ui_MainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_MainSplitContainer)).EndInit();
            this.ui_MainSplitContainer.ResumeLayout(false);
            this.ui_FeatureTypesPanel.ResumeLayout(false);
            this.ui_FeatureTypesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeatureTypesDGV)).EndInit();
            this.ui_FeaturesPanel.ResumeLayout(false);
            this.ui_searchPanel.ResumeLayout(false);
            this.ui_searchPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_loadingPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ui_addForExpContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ui_NavigationPanel;
        private System.Windows.Forms.SplitContainer ui_MainSplitContainer;
        private System.Windows.Forms.DataGridView ui_FeatureTypesDGV;
        private System.Windows.Forms.Label ui_effectiveDateLabel;
        private System.Windows.Forms.DataGridView ui_FeaturesDGV;
        private System.Windows.Forms.Panel ui_FeatureTypesPanel;
        private System.Windows.Forms.Panel ui_FeaturesPanel;
        private System.Windows.Forms.FlowLayoutPanel ui_navFlowLayoutPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_FeatureNameColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ui_fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ui_exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ui_statusLabel;
        private System.Windows.Forms.TextBox ui_quickSearchTextBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ui_newTSB;
		private System.Windows.Forms.ToolStripMenuItem toXmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_importTSMI;
		private System.Windows.Forms.ToolStripButton ui_deleteTSMI;
        private System.Windows.Forms.ToolStripMenuItem fromAIXMMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ui_toolsTSMI;
        private System.Windows.Forms.ToolStripMenuItem ui_optionsTSMI;
		private System.Windows.Forms.PictureBox ui_loadingPictureBox;
        private System.Windows.Forms.ContextMenuStrip ui_addForExpContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ui_addForExportingTSMI;
		private System.Windows.Forms.ToolStripSplitButton ui_viewTSSB;
		private System.Windows.Forms.ToolStripMenuItem ui_viewHistoryTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_viewTSMI;
		private System.Windows.Forms.ToolStripMenuItem ui_findFeatureTypeTSMI;
		private System.Windows.Forms.Panel ui_searchPanel;
		private System.Windows.Forms.Button ui_closeFindPanelButton;
		private System.Windows.Forms.Button ui_findNextButton;
		private System.Windows.Forms.TextBox ui_searchTB;
		private System.Windows.Forms.ToolStripMenuItem ui_quickSearchTSMI;
		private System.Windows.Forms.ToolStripButton ui_nextTSB;
		private System.Windows.Forms.ToolStripSeparator ui_nextButtonSeparator;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripButton ui_refreshTSB;
		private System.Windows.Forms.ToolStripButton ui_editNewSeqTSB;
		private System.Windows.Forms.ToolStripButton ui_editCorrTSB;
		private System.Windows.Forms.Panel panel1;
        private Aran.Controls.Airac.AiracCycleControl ui_effectiveDateTimePicker;
        private System.Windows.Forms.ToolStripMenuItem ui_featTypesByClassifiedTSMI;
    }
}

