namespace Aran.PANDA.Vss
{
    partial class ReportForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colHSurface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colHPenetrate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colNeededSlope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ui_dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ui_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName,
            this.ui_colX,
            this.ui_colHSurface,
            this.ui_colHeight,
            this.ui_colHPenetrate,
            this.ui_colNeededSlope});
            this.ui_dgv.Location = new System.Drawing.Point(2, 2);
            this.ui_dgv.MultiSelect = false;
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_dgv.Size = new System.Drawing.Size(591, 387);
            this.ui_dgv.TabIndex = 0;
            this.ui_dgv.VirtualMode = true;
            this.ui_dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.ui_dgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DGV_CellValueNeeded);
            this.ui_dgv.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_ColumnHeaderMouseClick);
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            // 
            // ui_colName
            // 
            this.ui_colName.FillWeight = 54.05405F;
            this.ui_colName.HeaderText = "Name";
            this.ui_colName.Name = "ui_colName";
            this.ui_colName.ReadOnly = true;
            this.ui_colName.Width = 120;
            // 
            // ui_colX
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.NullValue = null;
            this.ui_colX.DefaultCellStyle = dataGridViewCellStyle3;
            this.ui_colX.HeaderText = "X";
            this.ui_colX.Name = "ui_colX";
            this.ui_colX.ReadOnly = true;
            // 
            // ui_colHSurface
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = null;
            this.ui_colHSurface.DefaultCellStyle = dataGridViewCellStyle4;
            this.ui_colHSurface.FillWeight = 237.8378F;
            this.ui_colHSurface.HeaderText = "HSurface";
            this.ui_colHSurface.Name = "ui_colHSurface";
            this.ui_colHSurface.ReadOnly = true;
            // 
            // ui_colHeight
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ui_colHeight.DefaultCellStyle = dataGridViewCellStyle5;
            this.ui_colHeight.FillWeight = 54.05405F;
            this.ui_colHeight.HeaderText = "Height";
            this.ui_colHeight.Name = "ui_colHeight";
            this.ui_colHeight.ReadOnly = true;
            // 
            // ui_colHPenetrate
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ui_colHPenetrate.DefaultCellStyle = dataGridViewCellStyle6;
            this.ui_colHPenetrate.FillWeight = 54.05405F;
            this.ui_colHPenetrate.HeaderText = "HPenetrate";
            this.ui_colHPenetrate.Name = "ui_colHPenetrate";
            this.ui_colHPenetrate.ReadOnly = true;
            // 
            // ui_colNeededSlope
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ui_colNeededSlope.DefaultCellStyle = dataGridViewCellStyle7;
            this.ui_colNeededSlope.HeaderText = "Required VPA °";
            this.ui_colNeededSlope.Name = "ui_colNeededSlope";
            this.ui_colNeededSlope.ReadOnly = true;
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(509, 395);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 2;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 425);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(this.ui_dgv);
            this.MaximizeBox = false;
            this.Name = "ReportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report";
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colHSurface;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colHPenetrate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colNeededSlope;
    }
}