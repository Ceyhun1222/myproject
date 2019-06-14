namespace Holding
{
    partial class frmHoldingReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridReport = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Elevation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Altitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MOC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequriedH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.obstacleReportBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.holdingReportBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblCount = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.obstacleReportBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingReportBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridReport
            // 
            this.dataGridReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridReport.AutoGenerateColumns = false;
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
            this.Id,
            this.Name,
            this.Elevation,
            this.Altitude,
            this.MOC,
            this.RequriedH});
            this.dataGridReport.DataSource = this.obstacleReportBindingSource1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridReport.Location = new System.Drawing.Point(0, 0);
            this.dataGridReport.Name = "dataGridReport";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridReport.Size = new System.Drawing.Size(449, 238);
            this.dataGridReport.TabIndex = 0;
            this.dataGridReport.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridReport_CellContentClick);
            this.dataGridReport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridReport.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridReport_RowEnter);
            this.dataGridReport.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridReport_RowPrePaint);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            // 
            // Name
            // 
            this.Name.DataPropertyName = "Name";
            this.Name.HeaderText = "Name";
            this.Name.Name = "Name";
            // 
            // Elevation
            // 
            this.Elevation.DataPropertyName = "Elevation";
            this.Elevation.HeaderText = "Elevation (m)";
            this.Elevation.Name = "Elevation";
            // 
            // Altitude
            // 
            this.Altitude.DataPropertyName = "Altitude";
            this.Altitude.HeaderText = "Altitude (m)";
            this.Altitude.Name = "Altitude";
            // 
            // MOC
            // 
            this.MOC.DataPropertyName = "Moc";
            this.MOC.HeaderText = "Moc (m)";
            this.MOC.Name = "MOC";
            // 
            // RequriedH
            // 
            this.RequriedH.DataPropertyName = "Req_H";
            this.RequriedH.HeaderText = "Req_H (m)";
            this.RequriedH.Name = "RequriedH";
            // 
            // obstacleReportBindingSource1
            // 
            this.obstacleReportBindingSource1.DataMember = "ObstacleReport";
            this.obstacleReportBindingSource1.DataSource = this.holdingReportBindingSource;
            // 
            // holdingReportBindingSource
            // 
            this.holdingReportBindingSource.DataSource = typeof(Holding.HoldingReport);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(12, 255);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(41, 13);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "Count :";
            // 
            // txtCount
            // 
            this.txtCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCount.Enabled = false;
            this.txtCount.Location = new System.Drawing.Point(59, 253);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(100, 20);
            this.txtCount.TabIndex = 2;
            // 
            // frmHoldingReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 278);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.dataGridReport);
            this.Text = "Report";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHoldingReport_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.obstacleReportBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holdingReportBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridReport;
        private System.Windows.Forms.BindingSource holdingReportBindingSource;
        private System.Windows.Forms.BindingSource obstacleReportBindingSource1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Elevation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Altitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequriedH;
    }
}