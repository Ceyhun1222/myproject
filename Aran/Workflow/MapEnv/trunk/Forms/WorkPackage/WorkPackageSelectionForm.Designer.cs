namespace MapEnv
{
    partial class WorkPackageSelectionForm
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
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_wpDgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_featuresDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 1);
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
            this.splitContainer1.Size = new System.Drawing.Size(751, 488);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 4;
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
            this.ui_wpDgv.Size = new System.Drawing.Size(751, 239);
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
            this.ui_featuresDgv.Size = new System.Drawing.Size(751, 245);
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
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(579, 501);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(79, 27);
            this.ui_okButton.TabIndex = 5;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(663, 501);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(79, 27);
            this.ui_cancelButton.TabIndex = 6;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // WorkPackageSelectionForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(754, 540);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkPackageSelectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Work Package";
            this.Load += new System.EventHandler(this.WorkPackageSelectionForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_wpDgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_featuresDgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView ui_wpDgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColEffectiveDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_wpColFeatCount;
        private System.Windows.Forms.DataGridView ui_featuresDgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.Button ui_cancelButton;
    }
}