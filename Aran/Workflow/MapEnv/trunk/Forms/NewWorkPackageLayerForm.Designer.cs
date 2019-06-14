namespace MapEnv
{
    partial class NewWorkPackageLayerForm
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
            if (disposing && (components != null)) {
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
            System.Windows.Forms.TabPage tabPage1;
            System.Windows.Forms.TabPage tabPage2;
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_wpDgv = new System.Windows.Forms.DataGridView();
            this.ui_wpColId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_wpColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_wpColState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_wpColEffectiveDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_wpColFeatCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_featuresDgv = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_featureTypeStylesPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_pageTextLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_prevButton = new System.Windows.Forms.Button();
            this.ui_hiddenTabControl = new System.Windows.Forms.TabControl();
            this.ui_pageContainerPanel = new System.Windows.Forms.Panel();
            this.ui_featTypesStyleControl = new MapEnv.FeatureTypesStyleControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_wpDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_featuresDgv)).BeginInit();
            tabPage2.SuspendLayout();
            this.ui_featureTypeStylesPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ui_hiddenTabControl.SuspendLayout();
            this.ui_pageContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(this.splitContainer1);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(690, 499);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_wpDgv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_featuresDgv);
            this.splitContainer1.Size = new System.Drawing.Size(684, 493);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 3;
            // 
            // ui_wpDgv
            // 
            this.ui_wpDgv.AllowUserToAddRows = false;
            this.ui_wpDgv.AllowUserToDeleteRows = false;
            this.ui_wpDgv.AllowUserToResizeRows = false;
            this.ui_wpDgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_wpDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_wpDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_wpColId,
            this.ui_wpColName,
            this.ui_wpColState,
            this.ui_wpColEffectiveDate,
            this.ui_wpColFeatCount});
            this.ui_wpDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_wpDgv.Location = new System.Drawing.Point(0, 0);
            this.ui_wpDgv.MultiSelect = false;
            this.ui_wpDgv.Name = "ui_wpDgv";
            this.ui_wpDgv.ReadOnly = true;
            this.ui_wpDgv.RowHeadersVisible = false;
            this.ui_wpDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_wpDgv.Size = new System.Drawing.Size(684, 243);
            this.ui_wpDgv.TabIndex = 1;
            this.ui_wpDgv.VirtualMode = true;
            this.ui_wpDgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.WpDgv_CellValueNeeded);
            this.ui_wpDgv.CurrentCellChanged += new System.EventHandler(this.WpDgv_CurrentCellChanged);
            // 
            // ui_wpColId
            // 
            this.ui_wpColId.HeaderText = "ID";
            this.ui_wpColId.Name = "ui_wpColId";
            this.ui_wpColId.ReadOnly = true;
            this.ui_wpColId.Width = 60;
            // 
            // ui_wpColName
            // 
            this.ui_wpColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_wpColName.HeaderText = "Name";
            this.ui_wpColName.Name = "ui_wpColName";
            this.ui_wpColName.ReadOnly = true;
            // 
            // ui_wpColState
            // 
            this.ui_wpColState.HeaderText = "State";
            this.ui_wpColState.Name = "ui_wpColState";
            this.ui_wpColState.ReadOnly = true;
            this.ui_wpColState.Width = 120;
            // 
            // ui_wpColEffectiveDate
            // 
            this.ui_wpColEffectiveDate.HeaderText = "Effective Date";
            this.ui_wpColEffectiveDate.Name = "ui_wpColEffectiveDate";
            this.ui_wpColEffectiveDate.ReadOnly = true;
            this.ui_wpColEffectiveDate.Width = 140;
            // 
            // ui_wpColFeatCount
            // 
            this.ui_wpColFeatCount.HeaderText = "Feature Count";
            this.ui_wpColFeatCount.Name = "ui_wpColFeatCount";
            this.ui_wpColFeatCount.ReadOnly = true;
            this.ui_wpColFeatCount.Width = 60;
            // 
            // ui_featuresDgv
            // 
            this.ui_featuresDgv.AllowUserToAddRows = false;
            this.ui_featuresDgv.AllowUserToDeleteRows = false;
            this.ui_featuresDgv.AllowUserToResizeRows = false;
            this.ui_featuresDgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_featuresDgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_featuresDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn5});
            this.ui_featuresDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featuresDgv.Location = new System.Drawing.Point(0, 0);
            this.ui_featuresDgv.MultiSelect = false;
            this.ui_featuresDgv.Name = "ui_featuresDgv";
            this.ui_featuresDgv.ReadOnly = true;
            this.ui_featuresDgv.RowHeadersVisible = false;
            this.ui_featuresDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_featuresDgv.Size = new System.Drawing.Size(684, 246);
            this.ui_featuresDgv.TabIndex = 2;
            this.ui_featuresDgv.VirtualMode = true;
            this.ui_featuresDgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.FeaturesDgv_CellValueNeeded);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Identifier";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Feature Type";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(this.ui_featureTypeStylesPanel);
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(690, 499);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            // 
            // ui_featureTypeStylesPanel
            // 
            this.ui_featureTypeStylesPanel.Controls.Add(this.ui_featTypesStyleControl);
            this.ui_featureTypeStylesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featureTypeStylesPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_featureTypeStylesPanel.Name = "ui_featureTypeStylesPanel";
            this.ui_featureTypeStylesPanel.Size = new System.Drawing.Size(684, 493);
            this.ui_featureTypeStylesPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ui_pageTextLabel);
            this.panel1.Location = new System.Drawing.Point(-4, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 48);
            this.panel1.TabIndex = 7;
            // 
            // ui_pageTextLabel
            // 
            this.ui_pageTextLabel.AutoSize = true;
            this.ui_pageTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_pageTextLabel.Location = new System.Drawing.Point(17, 16);
            this.ui_pageTextLabel.Name = "ui_pageTextLabel";
            this.ui_pageTextLabel.Size = new System.Drawing.Size(122, 16);
            this.ui_pageTextLabel.TabIndex = 0;
            this.ui_pageTextLabel.Text = "Work Packages:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(9, 582);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(686, 2);
            this.label1.TabIndex = 11;
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(624, 592);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 10;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nextButton.Location = new System.Drawing.Point(543, 592);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(75, 23);
            this.ui_nextButton.TabIndex = 9;
            this.ui_nextButton.Text = "Next >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_prevButton
            // 
            this.ui_prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_prevButton.Location = new System.Drawing.Point(462, 592);
            this.ui_prevButton.Name = "ui_prevButton";
            this.ui_prevButton.Size = new System.Drawing.Size(75, 23);
            this.ui_prevButton.TabIndex = 8;
            this.ui_prevButton.Text = "< Back";
            this.ui_prevButton.UseVisualStyleBackColor = true;
            this.ui_prevButton.Visible = false;
            this.ui_prevButton.Click += new System.EventHandler(this.Back_Click);
            // 
            // ui_hiddenTabControl
            // 
            this.ui_hiddenTabControl.Controls.Add(tabPage1);
            this.ui_hiddenTabControl.Controls.Add(tabPage2);
            this.ui_hiddenTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_hiddenTabControl.Location = new System.Drawing.Point(0, 0);
            this.ui_hiddenTabControl.Name = "ui_hiddenTabControl";
            this.ui_hiddenTabControl.SelectedIndex = 0;
            this.ui_hiddenTabControl.Size = new System.Drawing.Size(698, 525);
            this.ui_hiddenTabControl.TabIndex = 12;
            // 
            // ui_pageContainerPanel
            // 
            this.ui_pageContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_pageContainerPanel.Controls.Add(this.ui_hiddenTabControl);
            this.ui_pageContainerPanel.Location = new System.Drawing.Point(3, 48);
            this.ui_pageContainerPanel.Name = "ui_pageContainerPanel";
            this.ui_pageContainerPanel.Size = new System.Drawing.Size(698, 525);
            this.ui_pageContainerPanel.TabIndex = 13;
            // 
            // ui_featTypesStyleControl
            // 
            this.ui_featTypesStyleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featTypesStyleControl.Location = new System.Drawing.Point(0, 0);
            this.ui_featTypesStyleControl.Name = "ui_featTypesStyleControl";
            this.ui_featTypesStyleControl.SelectedFeatureType = null;
            this.ui_featTypesStyleControl.Size = new System.Drawing.Size(684, 493);
            this.ui_featTypesStyleControl.TabIndex = 0;
            // 
            // NewWorkPackageLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 627);
            this.Controls.Add(this.ui_pageContainerPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_nextButton);
            this.Controls.Add(this.ui_prevButton);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewWorkPackageLayerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "New WorkPackage Layer";
            this.Load += new System.EventHandler(this.NewWorkPackageLayerForm_Load);
            tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_wpDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_featuresDgv)).EndInit();
            tabPage2.ResumeLayout(false);
            this.ui_featureTypeStylesPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ui_hiddenTabControl.ResumeLayout(false);
            this.ui_pageContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_wpDgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColEffectiveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColFeatCount;
        private System.Windows.Forms.DataGridView ui_featuresDgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ui_pageTextLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_prevButton;
        private System.Windows.Forms.TabControl ui_hiddenTabControl;
        private System.Windows.Forms.Panel ui_pageContainerPanel;
        private System.Windows.Forms.Panel ui_featureTypeStylesPanel;
        private FeatureTypesStyleControl ui_featTypesStyleControl;

    }
}