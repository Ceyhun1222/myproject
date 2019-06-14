namespace UIMetadatEditor
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
            this.ui_classInfoDGV = new System.Windows.Forms.DataGridView();
            this.ui_Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_Column4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ui_Column5 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ui_showOnlyFeaturesCheckBox = new System.Windows.Forms.CheckBox();
            this.ui_saveButton = new System.Windows.Forms.Button();
            this.ui_showPropertiesCheckBox = new System.Windows.Forms.CheckBox();
            this.ui_mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_propPanel = new System.Windows.Forms.Panel();
            this.ui_descFormatTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_propInfoLabel = new System.Windows.Forms.Label();
            this.ui_propInfoDGV = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_classInfoCountStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_propInfoCountStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ui_quickSearchTextBox = new System.Windows.Forms.TextBox();
            this.ui_backButton = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.ui_propInfoColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_propInfoColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_propInfoColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_propInfoColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ui_classInfoDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).BeginInit();
            this.ui_mainSplitContainer.Panel1.SuspendLayout();
            this.ui_mainSplitContainer.Panel2.SuspendLayout();
            this.ui_mainSplitContainer.SuspendLayout();
            this.ui_propPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_propInfoDGV)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_classInfoDGV
            // 
            this.ui_classInfoDGV.AllowUserToAddRows = false;
            this.ui_classInfoDGV.AllowUserToDeleteRows = false;
            this.ui_classInfoDGV.AllowUserToOrderColumns = true;
            this.ui_classInfoDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_classInfoDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ui_classInfoDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_classInfoDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_Column1,
            this.ui_Column2,
            this.ui_Column3,
            this.ui_Column4,
            this.ui_Column5});
            this.ui_classInfoDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_classInfoDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_classInfoDGV.MultiSelect = false;
            this.ui_classInfoDGV.Name = "ui_classInfoDGV";
            this.ui_classInfoDGV.RowHeadersWidth = 25;
            this.ui_classInfoDGV.Size = new System.Drawing.Size(467, 372);
            this.ui_classInfoDGV.TabIndex = 0;
            this.ui_classInfoDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ui_classInfoDGV_CellContentClick);
            this.ui_classInfoDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ui_mainDGV_CellEndEdit);
            this.ui_classInfoDGV.CurrentCellChanged += new System.EventHandler(this.ui_classInfoDGV_CurrentCellChanged);
            // 
            // ui_Column1
            // 
            this.ui_Column1.HeaderText = "Name";
            this.ui_Column1.Name = "ui_Column1";
            this.ui_Column1.ReadOnly = true;
            this.ui_Column1.Width = 200;
            // 
            // ui_Column2
            // 
            this.ui_Column2.HeaderText = "Type";
            this.ui_Column2.Name = "ui_Column2";
            this.ui_Column2.ReadOnly = true;
            this.ui_Column2.Width = 160;
            // 
            // ui_Column3
            // 
            this.ui_Column3.HeaderText = "Caption";
            this.ui_Column3.Name = "ui_Column3";
            this.ui_Column3.Width = 200;
            // 
            // ui_Column4
            // 
            this.ui_Column4.DisplayStyleForCurrentCellOnly = true;
            this.ui_Column4.HeaderText = "Depend Feature";
            this.ui_Column4.Name = "ui_Column4";
            this.ui_Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ui_Column4.Width = 160;
            // 
            // ui_Column5
            // 
            this.ui_Column5.HeaderText = "Parent";
            this.ui_Column5.Name = "ui_Column5";
            this.ui_Column5.ReadOnly = true;
            this.ui_Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ui_showOnlyFeaturesCheckBox
            // 
            this.ui_showOnlyFeaturesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_showOnlyFeaturesCheckBox.AutoSize = true;
            this.ui_showOnlyFeaturesCheckBox.Location = new System.Drawing.Point(6, 413);
            this.ui_showOnlyFeaturesCheckBox.Name = "ui_showOnlyFeaturesCheckBox";
            this.ui_showOnlyFeaturesCheckBox.Size = new System.Drawing.Size(121, 17);
            this.ui_showOnlyFeaturesCheckBox.TabIndex = 1;
            this.ui_showOnlyFeaturesCheckBox.Text = "Show Only Features";
            this.ui_showOnlyFeaturesCheckBox.UseVisualStyleBackColor = true;
            this.ui_showOnlyFeaturesCheckBox.CheckedChanged += new System.EventHandler(this.ui_showOnlyFeaturesCheckBox_CheckedChanged);
            // 
            // ui_saveButton
            // 
            this.ui_saveButton.Location = new System.Drawing.Point(6, 6);
            this.ui_saveButton.Name = "ui_saveButton";
            this.ui_saveButton.Size = new System.Drawing.Size(45, 23);
            this.ui_saveButton.TabIndex = 2;
            this.ui_saveButton.Text = "Save";
            this.ui_saveButton.UseVisualStyleBackColor = true;
            this.ui_saveButton.Click += new System.EventHandler(this.ui_saveButton_Click);
            // 
            // ui_showPropertiesCheckBox
            // 
            this.ui_showPropertiesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_showPropertiesCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_showPropertiesCheckBox.AutoSize = true;
            this.ui_showPropertiesCheckBox.Location = new System.Drawing.Point(831, 6);
            this.ui_showPropertiesCheckBox.Name = "ui_showPropertiesCheckBox";
            this.ui_showPropertiesCheckBox.Size = new System.Drawing.Size(94, 23);
            this.ui_showPropertiesCheckBox.TabIndex = 3;
            this.ui_showPropertiesCheckBox.Text = "Show Properties";
            this.ui_showPropertiesCheckBox.UseVisualStyleBackColor = true;
            this.ui_showPropertiesCheckBox.CheckedChanged += new System.EventHandler(this.ui_showPropertiesCheckBox_CheckedChanged);
            // 
            // ui_mainSplitContainer
            // 
            this.ui_mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.ui_mainSplitContainer.Location = new System.Drawing.Point(6, 35);
            this.ui_mainSplitContainer.Name = "ui_mainSplitContainer";
            // 
            // ui_mainSplitContainer.Panel1
            // 
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_classInfoDGV);
            // 
            // ui_mainSplitContainer.Panel2
            // 
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.ui_propPanel);
            this.ui_mainSplitContainer.Size = new System.Drawing.Size(921, 372);
            this.ui_mainSplitContainer.SplitterDistance = 467;
            this.ui_mainSplitContainer.TabIndex = 4;
            // 
            // ui_propPanel
            // 
            this.ui_propPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ui_propPanel.Controls.Add(this.ui_descFormatTextBox);
            this.ui_propPanel.Controls.Add(this.label1);
            this.ui_propPanel.Controls.Add(this.ui_propInfoLabel);
            this.ui_propPanel.Controls.Add(this.ui_propInfoDGV);
            this.ui_propPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_propPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_propPanel.Name = "ui_propPanel";
            this.ui_propPanel.Size = new System.Drawing.Size(450, 372);
            this.ui_propPanel.TabIndex = 0;
            // 
            // ui_descFormatTextBox
            // 
            this.ui_descFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_descFormatTextBox.Location = new System.Drawing.Point(113, 30);
            this.ui_descFormatTextBox.Name = "ui_descFormatTextBox";
            this.ui_descFormatTextBox.Size = new System.Drawing.Size(330, 20);
            this.ui_descFormatTextBox.TabIndex = 4;
            this.ui_descFormatTextBox.TextChanged += new System.EventHandler(this.ui_descFormatTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Description Format:";
            // 
            // ui_propInfoLabel
            // 
            this.ui_propInfoLabel.AutoSize = true;
            this.ui_propInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_propInfoLabel.Location = new System.Drawing.Point(12, 10);
            this.ui_propInfoLabel.Name = "ui_propInfoLabel";
            this.ui_propInfoLabel.Size = new System.Drawing.Size(105, 13);
            this.ui_propInfoLabel.TabIndex = 2;
            this.ui_propInfoLabel.Text = "<PropInfo Name>";
            // 
            // ui_propInfoDGV
            // 
            this.ui_propInfoDGV.AllowUserToAddRows = false;
            this.ui_propInfoDGV.AllowUserToDeleteRows = false;
            this.ui_propInfoDGV.AllowUserToOrderColumns = true;
            this.ui_propInfoDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_propInfoDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_propInfoDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ui_propInfoDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_propInfoDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_propInfoColumn1,
            this.ui_propInfoColumn2,
            this.ui_propInfoColumn3,
            this.ui_propInfoColumn4});
            this.ui_propInfoDGV.Location = new System.Drawing.Point(0, 56);
            this.ui_propInfoDGV.MultiSelect = false;
            this.ui_propInfoDGV.Name = "ui_propInfoDGV";
            this.ui_propInfoDGV.RowHeadersWidth = 25;
            this.ui_propInfoDGV.Size = new System.Drawing.Size(446, 311);
            this.ui_propInfoDGV.TabIndex = 1;
            this.ui_propInfoDGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ui_propInfoDGV_CellEndEdit);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ui_classInfoCountStatusLabel,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.ui_propInfoCountStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 435);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(939, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(94, 17);
            this.toolStripStatusLabel1.Text = "ClassInfo Count:";
            // 
            // ui_classInfoCountStatusLabel
            // 
            this.ui_classInfoCountStatusLabel.Name = "ui_classInfoCountStatusLabel";
            this.ui_classInfoCountStatusLabel.Size = new System.Drawing.Size(13, 17);
            this.ui_classInfoCountStatusLabel.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabel2.Text = "        ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(112, 17);
            this.toolStripStatusLabel3.Text = "PropertyInfo Count:";
            // 
            // ui_propInfoCountStatusLabel
            // 
            this.ui_propInfoCountStatusLabel.Name = "ui_propInfoCountStatusLabel";
            this.ui_propInfoCountStatusLabel.Size = new System.Drawing.Size(13, 17);
            this.ui_propInfoCountStatusLabel.Text = "0";
            // 
            // ui_quickSearchTextBox
            // 
            this.ui_quickSearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_quickSearchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_quickSearchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.ui_quickSearchTextBox.Location = new System.Drawing.Point(647, 8);
            this.ui_quickSearchTextBox.Name = "ui_quickSearchTextBox";
            this.ui_quickSearchTextBox.Size = new System.Drawing.Size(178, 20);
            this.ui_quickSearchTextBox.TabIndex = 6;
            this.ui_quickSearchTextBox.Text = "Quick Search";
            this.ui_quickSearchTextBox.Enter += new System.EventHandler(this.ui_quickSearchTextBox_Enter);
            this.ui_quickSearchTextBox.Leave += new System.EventHandler(this.ui_quickSearchTextBox_Leave);
            // 
            // ui_backButton
            // 
            this.ui_backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_backButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_backButton.Location = new System.Drawing.Point(592, 6);
            this.ui_backButton.Name = "ui_backButton";
            this.ui_backButton.Size = new System.Drawing.Size(49, 23);
            this.ui_backButton.TabIndex = 7;
            this.ui_backButton.Text = "<";
            this.ui_backButton.UseVisualStyleBackColor = true;
            this.ui_backButton.Click += new System.EventHandler(this.ui_backButton_Click);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(57, 6);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(75, 23);
            this.testButton.TabIndex = 8;
            this.testButton.Text = "Test";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // ui_propInfoColumn1
            // 
            this.ui_propInfoColumn1.HeaderText = "Name";
            this.ui_propInfoColumn1.Name = "ui_propInfoColumn1";
            this.ui_propInfoColumn1.ReadOnly = true;
            // 
            // ui_propInfoColumn2
            // 
            this.ui_propInfoColumn2.HeaderText = "Type";
            this.ui_propInfoColumn2.Name = "ui_propInfoColumn2";
            this.ui_propInfoColumn2.ReadOnly = true;
            this.ui_propInfoColumn2.Width = 60;
            // 
            // ui_propInfoColumn3
            // 
            this.ui_propInfoColumn3.HeaderText = "Caption";
            this.ui_propInfoColumn3.Name = "ui_propInfoColumn3";
            this.ui_propInfoColumn3.Width = 140;
            // 
            // ui_propInfoColumn4
            // 
            this.ui_propInfoColumn4.HeaderText = "Show GridView";
            this.ui_propInfoColumn4.Name = "ui_propInfoColumn4";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 457);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.ui_backButton);
            this.Controls.Add(this.ui_quickSearchTextBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ui_mainSplitContainer);
            this.Controls.Add(this.ui_showPropertiesCheckBox);
            this.Controls.Add(this.ui_saveButton);
            this.Controls.Add(this.ui_showOnlyFeaturesCheckBox);
            this.MinimumSize = new System.Drawing.Size(500, 38);
            this.Name = "MainForm";
            this.Text = "UI Metadata Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_classInfoDGV)).EndInit();
            this.ui_mainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).EndInit();
            this.ui_mainSplitContainer.ResumeLayout(false);
            this.ui_propPanel.ResumeLayout(false);
            this.ui_propPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_propInfoDGV)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_classInfoDGV;
        private System.Windows.Forms.CheckBox ui_showOnlyFeaturesCheckBox;
        private System.Windows.Forms.Button ui_saveButton;
        private System.Windows.Forms.CheckBox ui_showPropertiesCheckBox;
        private System.Windows.Forms.SplitContainer ui_mainSplitContainer;
        private System.Windows.Forms.Panel ui_propPanel;
        private System.Windows.Forms.DataGridView ui_propInfoDGV;
        private System.Windows.Forms.Label ui_propInfoLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel ui_classInfoCountStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel ui_propInfoCountStatusLabel;
        private System.Windows.Forms.TextBox ui_quickSearchTextBox;
        private System.Windows.Forms.Button ui_backButton;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.TextBox ui_descFormatTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_Column3;
        private System.Windows.Forms.DataGridViewComboBoxColumn ui_Column4;
        private System.Windows.Forms.DataGridViewLinkColumn ui_Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_propInfoColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_propInfoColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_propInfoColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ui_propInfoColumn4;
    }
}

