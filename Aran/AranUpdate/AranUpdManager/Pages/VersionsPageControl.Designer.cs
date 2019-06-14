namespace AranUpdateManager
{
    partial class VersionsPageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStrip toolStrip1;
            System.Windows.Forms.ToolStripButton toolStripButton1;
            System.Windows.Forms.ToolStrip toolStrip2;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_setUserGroupTSB = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ui_versionDGV = new System.Windows.Forms.DataGridView();
            this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colReleasedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colIsActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ui_versionRTB = new System.Windows.Forms.RichTextBox();
            this.ui_versionUserFilterByVersionRB = new System.Windows.Forms.RadioButton();
            this.ui_versionUserNoFilterRB = new System.Windows.Forms.RadioButton();
            this.ui_versionUserGrDGV = new System.Windows.Forms.DataGridView();
            this.ui_colUGName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colUGDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colUGNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            toolStrip1.SuspendLayout();
            toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_versionDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_versionUserGrDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripButton1,
            this.ui_setUserGroupTSB});
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(335, 36);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = global::AranUpdateManager.Properties.Resources.add_24;
            toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Margin = new System.Windows.Forms.Padding(4);
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(126, 28);
            toolStripButton1.Text = "Add New Version";
            toolStripButton1.ToolTipText = "Add New Version";
            toolStripButton1.Click += new System.EventHandler(this.AddNewVersion_Click);
            // 
            // ui_setUserGroupTSB
            // 
            this.ui_setUserGroupTSB.Enabled = false;
            this.ui_setUserGroupTSB.Image = global::AranUpdateManager.Properties.Resources.download_24;
            this.ui_setUserGroupTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_setUserGroupTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_setUserGroupTSB.Margin = new System.Windows.Forms.Padding(4);
            this.ui_setUserGroupTSB.Name = "ui_setUserGroupTSB";
            this.ui_setUserGroupTSB.Size = new System.Drawing.Size(110, 28);
            this.ui_setUserGroupTSB.Text = "Set UserGroup";
            this.ui_setUserGroupTSB.ToolTipText = "Edit Group";
            this.ui_setUserGroupTSB.Click += new System.EventHandler(this.SetUserGroup_Click);
            // 
            // toolStrip2
            // 
            toolStrip2.AutoSize = false;
            toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new System.Drawing.Size(338, 36);
            toolStrip2.TabIndex = 3;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(173, 33);
            this.toolStripLabel1.Text = "User group version history:";
            this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ui_versionDGV
            // 
            this.ui_versionDGV.AllowUserToAddRows = false;
            this.ui_versionDGV.AllowUserToDeleteRows = false;
            this.ui_versionDGV.AllowUserToResizeRows = false;
            this.ui_versionDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_versionDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_versionDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_versionDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName,
            this.ui_colReleasedDate,
            this.ui_colIsActual});
            this.ui_versionDGV.Location = new System.Drawing.Point(0, 39);
            this.ui_versionDGV.MultiSelect = false;
            this.ui_versionDGV.Name = "ui_versionDGV";
            this.ui_versionDGV.ReadOnly = true;
            this.ui_versionDGV.RowHeadersVisible = false;
            this.ui_versionDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_versionDGV.Size = new System.Drawing.Size(335, 540);
            this.ui_versionDGV.TabIndex = 1;
            this.ui_versionDGV.VirtualMode = true;
            this.ui_versionDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VersionDGV_CellFormatting);
            this.ui_versionDGV.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.VersionDGV_CellValueNeeded);
            this.ui_versionDGV.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.VersionDGV_ColumnHeaderMouseClick);
            this.ui_versionDGV.CurrentCellChanged += new System.EventHandler(this.VersionDGV_CurrentCellChanged);
            // 
            // ui_colName
            // 
            this.ui_colName.HeaderText = "Name";
            this.ui_colName.Name = "ui_colName";
            this.ui_colName.ReadOnly = true;
            this.ui_colName.Width = 120;
            // 
            // ui_colReleasedDate
            // 
            this.ui_colReleasedDate.HeaderText = "Released Date";
            this.ui_colReleasedDate.Name = "ui_colReleasedDate";
            this.ui_colReleasedDate.ReadOnly = true;
            this.ui_colReleasedDate.Width = 80;
            // 
            // ui_colIsActual
            // 
            this.ui_colIsActual.HeaderText = "Is Actual";
            this.ui_colIsActual.Name = "ui_colIsActual";
            this.ui_colIsActual.ReadOnly = true;
            this.ui_colIsActual.Width = 60;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.ui_versionDGV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1075, 579);
            this.splitContainer1.SplitterDistance = 335;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ui_versionRTB);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ui_versionUserFilterByVersionRB);
            this.splitContainer2.Panel2.Controls.Add(this.ui_versionUserNoFilterRB);
            this.splitContainer2.Panel2.Controls.Add(this.ui_versionUserGrDGV);
            this.splitContainer2.Panel2.Controls.Add(toolStrip2);
            this.splitContainer2.Size = new System.Drawing.Size(736, 579);
            this.splitContainer2.SplitterDistance = 394;
            this.splitContainer2.TabIndex = 1;
            // 
            // ui_versionRTB
            // 
            this.ui_versionRTB.BackColor = System.Drawing.SystemColors.Window;
            this.ui_versionRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_versionRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_versionRTB.Location = new System.Drawing.Point(0, 0);
            this.ui_versionRTB.Name = "ui_versionRTB";
            this.ui_versionRTB.ReadOnly = true;
            this.ui_versionRTB.Size = new System.Drawing.Size(394, 579);
            this.ui_versionRTB.TabIndex = 0;
            this.ui_versionRTB.Text = "";
            // 
            // ui_versionUserFilterByVersionRB
            // 
            this.ui_versionUserFilterByVersionRB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_versionUserFilterByVersionRB.AutoSize = true;
            this.ui_versionUserFilterByVersionRB.Checked = true;
            this.ui_versionUserFilterByVersionRB.Location = new System.Drawing.Point(248, 10);
            this.ui_versionUserFilterByVersionRB.Name = "ui_versionUserFilterByVersionRB";
            this.ui_versionUserFilterByVersionRB.Size = new System.Drawing.Size(75, 17);
            this.ui_versionUserFilterByVersionRB.TabIndex = 5;
            this.ui_versionUserFilterByVersionRB.TabStop = true;
            this.ui_versionUserFilterByVersionRB.Text = "By Version";
            this.ui_versionUserFilterByVersionRB.UseVisualStyleBackColor = true;
            this.ui_versionUserFilterByVersionRB.CheckedChanged += new System.EventHandler(this.VersionUserFilterByVersion_CheckedChanged);
            // 
            // ui_versionUserNoFilterRB
            // 
            this.ui_versionUserNoFilterRB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_versionUserNoFilterRB.AutoSize = true;
            this.ui_versionUserNoFilterRB.Location = new System.Drawing.Point(204, 10);
            this.ui_versionUserNoFilterRB.Name = "ui_versionUserNoFilterRB";
            this.ui_versionUserNoFilterRB.Size = new System.Drawing.Size(36, 17);
            this.ui_versionUserNoFilterRB.TabIndex = 4;
            this.ui_versionUserNoFilterRB.Text = "All";
            this.ui_versionUserNoFilterRB.UseVisualStyleBackColor = false;
            this.ui_versionUserNoFilterRB.CheckedChanged += new System.EventHandler(this.VersionUserNoFilter_CheckedChanged);
            // 
            // ui_versionUserGrDGV
            // 
            this.ui_versionUserGrDGV.AllowUserToAddRows = false;
            this.ui_versionUserGrDGV.AllowUserToDeleteRows = false;
            this.ui_versionUserGrDGV.AllowUserToResizeRows = false;
            this.ui_versionUserGrDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_versionUserGrDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_versionUserGrDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colUGName,
            this.ui_colUGDate,
            this.ui_colUGNote});
            this.ui_versionUserGrDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_versionUserGrDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_versionUserGrDGV.MultiSelect = false;
            this.ui_versionUserGrDGV.Name = "ui_versionUserGrDGV";
            this.ui_versionUserGrDGV.ReadOnly = true;
            this.ui_versionUserGrDGV.RowHeadersVisible = false;
            this.ui_versionUserGrDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_versionUserGrDGV.Size = new System.Drawing.Size(338, 579);
            this.ui_versionUserGrDGV.TabIndex = 2;
            // 
            // ui_colUGName
            // 
            this.ui_colUGName.DataPropertyName = "UserGroup";
            this.ui_colUGName.HeaderText = "User Group";
            this.ui_colUGName.Name = "ui_colUGName";
            this.ui_colUGName.ReadOnly = true;
            this.ui_colUGName.Width = 140;
            // 
            // ui_colUGDate
            // 
            this.ui_colUGDate.DataPropertyName = "DateTime";
            dataGridViewCellStyle2.Format = "yyyy-MM-dd HH:mm";
            dataGridViewCellStyle2.NullValue = null;
            this.ui_colUGDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.ui_colUGDate.HeaderText = "Date";
            this.ui_colUGDate.Name = "ui_colUGDate";
            this.ui_colUGDate.ReadOnly = true;
            this.ui_colUGDate.Width = 120;
            // 
            // ui_colUGNote
            // 
            this.ui_colUGNote.DataPropertyName = "Note";
            this.ui_colUGNote.HeaderText = "Note";
            this.ui_colUGNote.Name = "ui_colUGNote";
            this.ui_colUGNote.ReadOnly = true;
            this.ui_colUGNote.Width = 160;
            // 
            // VersionsPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "VersionsPageControl";
            this.Size = new System.Drawing.Size(1075, 579);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_versionDGV)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_versionUserGrDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_versionDGV;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox ui_versionRTB;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView ui_versionUserGrDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colReleasedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colIsActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUGName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUGDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colUGNote;
        private System.Windows.Forms.ToolStripButton ui_setUserGroupTSB;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.RadioButton ui_versionUserFilterByVersionRB;
        private System.Windows.Forms.RadioButton ui_versionUserNoFilterRB;
    }
}
