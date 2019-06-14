namespace Aran.PANDA.Conventional.Racetrack
{
    partial class FormReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReport));
            this.dataGridReport = new System.Windows.Forms.DataGridView();
            this.dgvTxtBxClmnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnGeomType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnHAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnVAcc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnElevation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnMOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnReq_H = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTxtBxClmnPenetrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsStLblAltitude = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsStLblCnt = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridReport)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridReport
            // 
            this.dataGridReport.AllowUserToAddRows = false;
            this.dataGridReport.AllowUserToDeleteRows = false;
            this.dataGridReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTxtBxClmnName,
            this.dgvTxtBxClmnGeomType,
            this.dgvTxtBxClmnArea,
            this.dgvTxtBxClmnHAcc,
            this.dgvTxtBxClmnVAcc,
            this.dgvTxtBxClmnElevation,
            this.dgvTxtBxClmnMOC,
            this.dgvTxtBxClmnReq_H,
            this.dgvTxtBxClmnPenetrate});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridReport.Location = new System.Drawing.Point(0, 0);
            this.dataGridReport.MultiSelect = false;
            this.dataGridReport.Name = "dataGridReport";
            this.dataGridReport.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridReport.Size = new System.Drawing.Size(720, 425);
            this.dataGridReport.TabIndex = 0;
            this.dataGridReport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridReport.ColumnRemoved += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridReport_ColumnRemoved);
            this.dataGridReport.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridReport_RowEnter);
            this.dataGridReport.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridReport_RowPrePaint);
            // 
            // dgvTxtBxClmnName
            // 
            this.dgvTxtBxClmnName.FillWeight = 70.50198F;
            this.dgvTxtBxClmnName.HeaderText = "Name";
            this.dgvTxtBxClmnName.Name = "dgvTxtBxClmnName";
            this.dgvTxtBxClmnName.ReadOnly = true;
            // 
            // dgvTxtBxClmnGeomType
            // 
            this.dgvTxtBxClmnGeomType.FillWeight = 70.50198F;
            this.dgvTxtBxClmnGeomType.HeaderText = "Geometry Type";
            this.dgvTxtBxClmnGeomType.Name = "dgvTxtBxClmnGeomType";
            this.dgvTxtBxClmnGeomType.ReadOnly = true;
            // 
            // dgvTxtBxClmnArea
            // 
            this.dgvTxtBxClmnArea.FillWeight = 70.50198F;
            this.dgvTxtBxClmnArea.HeaderText = "Area";
            this.dgvTxtBxClmnArea.Name = "dgvTxtBxClmnArea";
            this.dgvTxtBxClmnArea.ReadOnly = true;
            // 
            // dgvTxtBxClmnHAcc
            // 
            this.dgvTxtBxClmnHAcc.FillWeight = 70.50198F;
            this.dgvTxtBxClmnHAcc.HeaderText = "HAcc";
            this.dgvTxtBxClmnHAcc.Name = "dgvTxtBxClmnHAcc";
            this.dgvTxtBxClmnHAcc.ReadOnly = true;
            // 
            // dgvTxtBxClmnVAcc
            // 
            this.dgvTxtBxClmnVAcc.FillWeight = 70.50198F;
            this.dgvTxtBxClmnVAcc.HeaderText = "VAcc";
            this.dgvTxtBxClmnVAcc.Name = "dgvTxtBxClmnVAcc";
            this.dgvTxtBxClmnVAcc.ReadOnly = true;
            // 
            // dgvTxtBxClmnElevation
            // 
            this.dgvTxtBxClmnElevation.FillWeight = 70.50198F;
            this.dgvTxtBxClmnElevation.HeaderText = "Elevation";
            this.dgvTxtBxClmnElevation.Name = "dgvTxtBxClmnElevation";
            this.dgvTxtBxClmnElevation.ReadOnly = true;
            // 
            // dgvTxtBxClmnMOC
            // 
            this.dgvTxtBxClmnMOC.FillWeight = 70.50198F;
            this.dgvTxtBxClmnMOC.HeaderText = "MOC";
            this.dgvTxtBxClmnMOC.Name = "dgvTxtBxClmnMOC";
            this.dgvTxtBxClmnMOC.ReadOnly = true;
            // 
            // dgvTxtBxClmnReq_H
            // 
            this.dgvTxtBxClmnReq_H.HeaderText = "Req_Altitude";
            this.dgvTxtBxClmnReq_H.Name = "dgvTxtBxClmnReq_H";
            this.dgvTxtBxClmnReq_H.ReadOnly = true;
            // 
            // dgvTxtBxClmnPenetrate
            // 
            this.dgvTxtBxClmnPenetrate.FillWeight = 70.50198F;
            this.dgvTxtBxClmnPenetrate.HeaderText = "Penetrate";
            this.dgvTxtBxClmnPenetrate.Name = "dgvTxtBxClmnPenetrate";
            this.dgvTxtBxClmnPenetrate.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Location";
            this.dataGridViewTextBoxColumn1.HeaderText = "Location";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 409;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsStLblAltitude,
            this.toolStripStatusLabel2,
            this.tsStLblCnt});
            this.statusStrip1.Location = new System.Drawing.Point(0, 462);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(720, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(145, 19);
            this.toolStripStatusLabel1.Text = "Altitude above of navaid : ";
            // 
            // tsStLblAltitude
            // 
            this.tsStLblAltitude.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsStLblAltitude.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsStLblAltitude.Name = "tsStLblAltitude";
            this.tsStLblAltitude.Size = new System.Drawing.Size(122, 19);
            this.tsStLblAltitude.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(289, 19);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Count:";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tsStLblCnt
            // 
            this.tsStLblCnt.Name = "tsStLblCnt";
            this.tsStLblCnt.Size = new System.Drawing.Size(118, 19);
            this.tsStLblCnt.Text = "toolStripStatusLabel3";
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSave.Image = global::Aran.PANDA.Conventional.Racetrack.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(637, 431);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 485;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.Location = new System.Drawing.Point(67, 435);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 20);
            this.txtSearch.TabIndex = 487;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 438);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 486;
            this.label1.Text = "Search :";
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 486);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridReport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReport";
            this.Text = "Report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormReport_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridReport)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.DataGridView dataGridReport;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel tsStLblAltitude;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripStatusLabel tsStLblCnt;
        public System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnGeomType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnHAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnVAcc;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnElevation;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnMOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnReq_H;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxClmnPenetrate;
    }
}