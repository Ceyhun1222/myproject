namespace Aran.PANDA.RNAV.Enroute.VD
{
	partial class ReportsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsForm));
			this.dataGridView01 = new System.Windows.Forms.DataGridView();
			this.txtColumn01_01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtColumn01_02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtColumn01_10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.numericUpDown01 = new System.Windows.Forms.NumericUpDown();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView01)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown01)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView01
			// 
			this.dataGridView01.AllowUserToAddRows = false;
			this.dataGridView01.AllowUserToDeleteRows = false;
			this.dataGridView01.AllowUserToResizeRows = false;
			this.dataGridView01.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView01.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dataGridView01.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dataGridView01.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView01.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtColumn01_01,
            this.txtColumn01_02,
            this.fltColumn01_03,
            this.fltColumn01_04,
            this.fltColumn01_05,
            this.fltColumn01_06,
            this.fltColumn01_07,
            this.fltColumn01_08,
            this.fltColumn01_09,
            this.txtColumn01_10});
			this.dataGridView01.Location = new System.Drawing.Point(1, 6);
			this.dataGridView01.MultiSelect = false;
			this.dataGridView01.Name = "dataGridView01";
			this.dataGridView01.ReadOnly = true;
			this.dataGridView01.RowHeadersVisible = false;
			this.dataGridView01.RowTemplate.DividerHeight = 1;
			this.dataGridView01.RowTemplate.ReadOnly = true;
			this.dataGridView01.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView01.Size = new System.Drawing.Size(979, 589);
			this.dataGridView01.TabIndex = 220;
			this.dataGridView01.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView01_RowEnter);
			this.dataGridView01.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
			// 
			// txtColumn03_01
			// 
			this.txtColumn01_01.HeaderText = "Name";
			this.txtColumn01_01.Name = "txtColumn01_01";
			this.txtColumn01_01.ReadOnly = true;
			this.txtColumn01_01.Width = 177;
			// 
			// txtColumn03_02
			// 
			this.txtColumn01_02.HeaderText = "ID";
			this.txtColumn01_02.Name = "txtColumn01_02";
			this.txtColumn01_02.ReadOnly = true;
			// 
			// fltColumn03_03
			// 
			this.fltColumn01_03.HeaderText = "X";
			this.fltColumn01_03.Name = "fltColumn01_03";
			this.fltColumn01_03.ReadOnly = true;
			// 
			// fltColumn03_04
			// 
			this.fltColumn01_04.HeaderText = "Y";
			this.fltColumn01_04.Name = "fltColumn01_04";
			this.fltColumn01_04.ReadOnly = true;
			// 
			// fltColumn03_05
			// 
			this.fltColumn01_05.HeaderText = "Elevation";
			this.fltColumn01_05.Name = "fltColumn01_05";
			this.fltColumn01_05.ReadOnly = true;
			// 
			// fltColumn03_06
			// 
			this.fltColumn01_06.HeaderText = "MOC";
			this.fltColumn01_06.Name = "fltColumn01_06";
			this.fltColumn01_06.ReadOnly = true;
			// 
			// fltColumn03_07
			// 
			this.fltColumn01_07.HeaderText = "MOCA";
			this.fltColumn01_07.Name = "fltColumn01_07";
			this.fltColumn01_07.ReadOnly = true;
			// 
			// fltColumn03_08
			// 
			this.fltColumn01_08.HeaderText = "Hor. accuracy";
			this.fltColumn01_08.Name = "fltColumn01_08";
			this.fltColumn01_08.ReadOnly = true;
			// 
			// fltColumn03_09
			// 
			this.fltColumn01_09.HeaderText = "Ver. accuracy";
			this.fltColumn01_09.Name = "fltColumn01_09";
			this.fltColumn01_09.ReadOnly = true;
			// 
			// txtColumn03_10
			// 
			this.txtColumn01_10.HeaderText = "Area";
			this.txtColumn01_10.Name = "txtColumn01_10";
			this.txtColumn01_10.ReadOnly = true;
			// 
			// numericUpDown01
			// 
			this.numericUpDown01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown01.Location = new System.Drawing.Point(550, 603);
			this.numericUpDown01.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown01.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown01.Name = "numericUpDown01";
			this.numericUpDown01.Size = new System.Drawing.Size(120, 20);
			this.numericUpDown01.TabIndex = 224;
			this.numericUpDown01.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown01.ValueChanged += new System.EventHandler(this.numericUpDown01_ValueChanged);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.HelpBtn.BackColor = System.Drawing.SystemColors.Control;
			this.HelpBtn.Cursor = System.Windows.Forms.Cursors.Default;
			this.HelpBtn.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.HelpBtn.Image = ((System.Drawing.Image)(resources.GetObject("HelpBtn.Image")));
			this.HelpBtn.Location = new System.Drawing.Point(688, 601);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(25, 25);
			this.HelpBtn.TabIndex = 223;
			this.HelpBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.HelpBtn.UseVisualStyleBackColor = false;
			this.HelpBtn.Visible = false;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
			this.closeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.closeButton.Location = new System.Drawing.Point(853, 601);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(116, 25);
			this.closeButton.TabIndex = 222;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
			this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveButton.Location = new System.Drawing.Point(732, 601);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(116, 25);
			this.saveButton.TabIndex = 221;
			this.saveButton.Text = "Save";
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// ReportsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(981, 636);
			this.Controls.Add(this.dataGridView01);
			this.Controls.Add(this.numericUpDown01);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.saveButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ReportsForm";
			this.Text = "ReportsForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReportForm_FormClosed);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReportForm_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView01)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown01)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView01;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn01_01;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn01_02;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_03;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_04;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_05;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_06;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_07;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_08;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_09;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn01_10;
		private System.Windows.Forms.NumericUpDown numericUpDown01;
		public System.Windows.Forms.Button HelpBtn;
		public System.Windows.Forms.Button closeButton;
		public System.Windows.Forms.Button saveButton;
	}
}