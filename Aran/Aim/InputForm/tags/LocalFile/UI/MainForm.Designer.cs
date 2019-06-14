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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_NavigationPanel = new System.Windows.Forms.Panel();
            this.ui_navFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_FeatureTypesPanel = new System.Windows.Forms.Panel();
            this.ui_quickSearchTextBox = new System.Windows.Forms.TextBox();
            this.ui_FeatureNameLabel = new System.Windows.Forms.Label();
            this.ui_FeatureTypesDGV = new System.Windows.Forms.DataGridView();
            this.ui_FeatureNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_FeaturesPanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ui_newTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_editTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_nextTSB = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ui_interpretationComboBox = new System.Windows.Forms.ComboBox();
            this.ui_effectiveDateLabel = new System.Windows.Forms.Label();
            this.ui_effectiveDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ui_interpretationLabel = new System.Windows.Forms.Label();
            this.ui_FeaturesDGV = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ui_fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ui_statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_NavigationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_MainSplitContainer)).BeginInit();
            this.ui_MainSplitContainer.Panel1.SuspendLayout();
            this.ui_MainSplitContainer.Panel2.SuspendLayout();
            this.ui_MainSplitContainer.SuspendLayout();
            this.ui_FeatureTypesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeatureTypesDGV)).BeginInit();
            this.ui_FeaturesPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_NavigationPanel
            // 
            this.ui_NavigationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_NavigationPanel.Controls.Add(this.ui_navFlowLayoutPanel);
            this.ui_NavigationPanel.Location = new System.Drawing.Point(12, 27);
            this.ui_NavigationPanel.Name = "ui_NavigationPanel";
            this.ui_NavigationPanel.Size = new System.Drawing.Size(1120, 28);
            this.ui_NavigationPanel.TabIndex = 0;
            // 
            // ui_navFlowLayoutPanel
            // 
            this.ui_navFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_navFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_navFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_navFlowLayoutPanel.Name = "ui_navFlowLayoutPanel";
            this.ui_navFlowLayoutPanel.Size = new System.Drawing.Size(1120, 28);
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
            this.ui_MainSplitContainer.Location = new System.Drawing.Point(12, 67);
            this.ui_MainSplitContainer.Name = "ui_MainSplitContainer";
            // 
            // ui_MainSplitContainer.Panel1
            // 
            this.ui_MainSplitContainer.Panel1.Controls.Add(this.ui_FeatureTypesPanel);
            // 
            // ui_MainSplitContainer.Panel2
            // 
            this.ui_MainSplitContainer.Panel2.Controls.Add(this.ui_FeaturesPanel);
            this.ui_MainSplitContainer.Size = new System.Drawing.Size(1120, 417);
            this.ui_MainSplitContainer.SplitterDistance = 250;
            this.ui_MainSplitContainer.TabIndex = 1;
            // 
            // ui_FeatureTypesPanel
            // 
            this.ui_FeatureTypesPanel.Controls.Add(this.ui_quickSearchTextBox);
            this.ui_FeatureTypesPanel.Controls.Add(this.ui_FeatureNameLabel);
            this.ui_FeatureTypesPanel.Controls.Add(this.ui_FeatureTypesDGV);
            this.ui_FeatureTypesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_FeatureTypesPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_FeatureTypesPanel.Name = "ui_FeatureTypesPanel";
            this.ui_FeatureTypesPanel.Size = new System.Drawing.Size(250, 417);
            this.ui_FeatureTypesPanel.TabIndex = 2;
            // 
            // ui_quickSearchTextBox
            // 
            this.ui_quickSearchTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ui_quickSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_quickSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.ui_quickSearchTextBox.Location = new System.Drawing.Point(0, 397);
            this.ui_quickSearchTextBox.Name = "ui_quickSearchTextBox";
            this.ui_quickSearchTextBox.Size = new System.Drawing.Size(250, 20);
            this.ui_quickSearchTextBox.TabIndex = 7;
            this.ui_quickSearchTextBox.Text = "Quick Search";
            this.ui_quickSearchTextBox.Enter += new System.EventHandler(this.ui_quickFeatTypeSearchTB_Enter);
            this.ui_quickSearchTextBox.Leave += new System.EventHandler(this.ui_quickFeatTypeSearchTB_Leave);
            // 
            // ui_FeatureNameLabel
            // 
            this.ui_FeatureNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_FeatureNameLabel.AutoSize = true;
            this.ui_FeatureNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_FeatureNameLabel.Location = new System.Drawing.Point(4, 5);
            this.ui_FeatureNameLabel.Name = "ui_FeatureNameLabel";
            this.ui_FeatureNameLabel.Size = new System.Drawing.Size(88, 13);
            this.ui_FeatureNameLabel.TabIndex = 1;
            this.ui_FeatureNameLabel.Text = "Feature Types";
            this.ui_FeatureNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.ui_FeatureTypesDGV.Size = new System.Drawing.Size(250, 370);
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
            this.ui_FeaturesPanel.Controls.Add(this.toolStrip1);
            this.ui_FeaturesPanel.Controls.Add(this.groupBox1);
            this.ui_FeaturesPanel.Controls.Add(this.ui_FeaturesDGV);
            this.ui_FeaturesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_FeaturesPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_FeaturesPanel.Name = "ui_FeaturesPanel";
            this.ui_FeaturesPanel.Size = new System.Drawing.Size(866, 417);
            this.ui_FeaturesPanel.TabIndex = 11;
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
            this.ui_newTSB,
            this.ui_editTSB,
            this.ui_nextTSB});
            this.toolStrip1.Location = new System.Drawing.Point(296, 11);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(566, 44);
            this.toolStrip1.TabIndex = 16;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ui_newTSB
            // 
            this.ui_newTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_newTSB.Image")));
            this.ui_newTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_newTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_newTSB.Name = "ui_newTSB";
            this.ui_newTSB.Size = new System.Drawing.Size(59, 41);
            this.ui_newTSB.Text = "New";
            this.ui_newTSB.Click += new System.EventHandler(this.ui_newButton_Click);
            // 
            // ui_editTSB
            // 
            this.ui_editTSB.Enabled = false;
            this.ui_editTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_editTSB.Image")));
            this.ui_editTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_editTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_editTSB.Name = "ui_editTSB";
            this.ui_editTSB.Size = new System.Drawing.Size(55, 41);
            this.ui_editTSB.Text = "Edit";
            this.ui_editTSB.Click += new System.EventHandler(this.ui_showPropButton_Click);
            // 
            // ui_nextTSB
            // 
            this.ui_nextTSB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ui_nextTSB.Enabled = false;
            this.ui_nextTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_nextTSB.Image")));
            this.ui_nextTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_nextTSB.Name = "ui_nextTSB";
            this.ui_nextTSB.Size = new System.Drawing.Size(59, 41);
            this.ui_nextTSB.Text = "Next";
            this.ui_nextTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ui_nextTSB.Click += new System.EventHandler(this.ui_nextButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ui_interpretationComboBox);
            this.groupBox1.Controls.Add(this.ui_effectiveDateLabel);
            this.groupBox1.Controls.Add(this.ui_effectiveDateTimePicker);
            this.groupBox1.Controls.Add(this.ui_interpretationLabel);
            this.groupBox1.Location = new System.Drawing.Point(3, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 60);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // ui_interpretationComboBox
            // 
            this.ui_interpretationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_interpretationComboBox.FormattingEnabled = true;
            this.ui_interpretationComboBox.Location = new System.Drawing.Point(8, 30);
            this.ui_interpretationComboBox.Name = "ui_interpretationComboBox";
            this.ui_interpretationComboBox.Size = new System.Drawing.Size(129, 21);
            this.ui_interpretationComboBox.TabIndex = 7;
            this.ui_interpretationComboBox.SelectedIndexChanged += new System.EventHandler(this.ui_interpretationComboBox_SelectedIndexChanged);
            // 
            // ui_effectiveDateLabel
            // 
            this.ui_effectiveDateLabel.AutoSize = true;
            this.ui_effectiveDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_effectiveDateLabel.Location = new System.Drawing.Point(144, 13);
            this.ui_effectiveDateLabel.Name = "ui_effectiveDateLabel";
            this.ui_effectiveDateLabel.Size = new System.Drawing.Size(93, 13);
            this.ui_effectiveDateLabel.TabIndex = 8;
            this.ui_effectiveDateLabel.Text = "Effective Date:";
            // 
            // ui_effectiveDateTimePicker
            // 
            this.ui_effectiveDateTimePicker.CustomFormat = "yyyy - MM - dd";
            this.ui_effectiveDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ui_effectiveDateTimePicker.Location = new System.Drawing.Point(147, 30);
            this.ui_effectiveDateTimePicker.Name = "ui_effectiveDateTimePicker";
            this.ui_effectiveDateTimePicker.Size = new System.Drawing.Size(131, 20);
            this.ui_effectiveDateTimePicker.TabIndex = 9;
            // 
            // ui_interpretationLabel
            // 
            this.ui_interpretationLabel.AutoSize = true;
            this.ui_interpretationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_interpretationLabel.Location = new System.Drawing.Point(5, 13);
            this.ui_interpretationLabel.Name = "ui_interpretationLabel";
            this.ui_interpretationLabel.Size = new System.Drawing.Size(87, 13);
            this.ui_interpretationLabel.TabIndex = 6;
            this.ui_interpretationLabel.Text = "Interpretation:";
            // 
            // ui_FeaturesDGV
            // 
            this.ui_FeaturesDGV.AllowUserToAddRows = false;
            this.ui_FeaturesDGV.AllowUserToDeleteRows = false;
            this.ui_FeaturesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_FeaturesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_FeaturesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_FeaturesDGV.Location = new System.Drawing.Point(0, 65);
            this.ui_FeaturesDGV.Name = "ui_FeaturesDGV";
            this.ui_FeaturesDGV.ReadOnly = true;
            this.ui_FeaturesDGV.Size = new System.Drawing.Size(866, 352);
            this.ui_FeaturesDGV.TabIndex = 10;
            this.ui_FeaturesDGV.CurrentCellChanged += new System.EventHandler(this.ui_FeaturesDGV_CurrentCellChanged);
            this.ui_FeaturesDGV.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.ui_FeaturesDGV_RowsAdded);
            this.ui_FeaturesDGV.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.ui_FeaturesDGV_RowsRemoved);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_fileToolStripMenuItem,
            this.ui_helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1144, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ui_fileToolStripMenuItem
            // 
            this.ui_fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_exportToolStripMenuItem,
            this.toolStripMenuItem2,
            this.ui_exitToolStripMenuItem});
            this.ui_fileToolStripMenuItem.Name = "ui_fileToolStripMenuItem";
            this.ui_fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.ui_fileToolStripMenuItem.Text = "&File";
            // 
            // ui_exportToolStripMenuItem
            // 
            this.ui_exportToolStripMenuItem.Name = "ui_exportToolStripMenuItem";
            this.ui_exportToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.ui_exportToolStripMenuItem.Text = "Export";
            this.ui_exportToolStripMenuItem.Click += new System.EventHandler(this.ui_exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 6);
            // 
            // ui_exitToolStripMenuItem
            // 
            this.ui_exitToolStripMenuItem.Name = "ui_exitToolStripMenuItem";
            this.ui_exitToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.ui_exitToolStripMenuItem.Text = "&Exit";
            this.ui_exitToolStripMenuItem.Click += new System.EventHandler(this.ui_exitToolStripMenuItem_Click);
            // 
            // ui_helpToolStripMenuItem
            // 
            this.ui_helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_aboutToolStripMenuItem});
            this.ui_helpToolStripMenuItem.Name = "ui_helpToolStripMenuItem";
            this.ui_helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.ui_helpToolStripMenuItem.Text = "&Help";
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
            this.ui_statusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1144, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ui_statusLabel1
            // 
            this.ui_statusLabel1.Name = "ui_statusLabel1";
            this.ui_statusLabel1.Size = new System.Drawing.Size(92, 17);
            this.ui_statusLabel1.Text = "Feature count: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 509);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ui_MainSplitContainer);
            this.Controls.Add(this.ui_NavigationPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Input Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ui_NavigationPanel;
        private System.Windows.Forms.SplitContainer ui_MainSplitContainer;
        private System.Windows.Forms.DataGridView ui_FeatureTypesDGV;
        private System.Windows.Forms.Label ui_FeatureNameLabel;
        private System.Windows.Forms.DateTimePicker ui_effectiveDateTimePicker;
        private System.Windows.Forms.Label ui_effectiveDateLabel;
        private System.Windows.Forms.ComboBox ui_interpretationComboBox;
        private System.Windows.Forms.Label ui_interpretationLabel;
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
        private System.Windows.Forms.ToolStripStatusLabel ui_statusLabel1;
        private System.Windows.Forms.TextBox ui_quickSearchTextBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ui_newTSB;
        private System.Windows.Forms.ToolStripButton ui_editTSB;
        private System.Windows.Forms.ToolStripButton ui_nextTSB;
    }
}

