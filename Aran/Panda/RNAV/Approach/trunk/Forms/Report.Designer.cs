namespace Aran.PANDA.RNAV.Approach
{
	partial class Report
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.tsCircklingArea = new System.Windows.Forms.TabPage();
			this.dataGridView01 = new System.Windows.Forms.DataGridView();
			this.tsIntermediateApproach = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCount = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.txtColumn01_01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtColumn01_02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.fltColumn01_05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainTabControl.SuspendLayout();
			this.tsCircklingArea.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView01)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.tsCircklingArea);
			this.mainTabControl.Controls.Add(this.tsIntermediateApproach);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(870, 425);
			this.mainTabControl.TabIndex = 3;
			// 
			// tsCircklingArea
			// 
			this.tsCircklingArea.Controls.Add(this.dataGridView01);
			this.tsCircklingArea.ImageIndex = 1;
			this.tsCircklingArea.Location = new System.Drawing.Point(4, 22);
			this.tsCircklingArea.Name = "tsCircklingArea";
			this.tsCircklingArea.Size = new System.Drawing.Size(862, 399);
			this.tsCircklingArea.TabIndex = 1;
			this.tsCircklingArea.Text = "Circkling area";
			// 
			// dataGridView01
			// 
			this.dataGridView01.AllowUserToAddRows = false;
			this.dataGridView01.AllowUserToDeleteRows = false;
			this.dataGridView01.AllowUserToResizeRows = false;
			this.dataGridView01.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dataGridView01.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView01.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtColumn01_01,
            this.txtColumn01_02,
            this.fltColumn01_03,
            this.fltColumn01_04,
            this.fltColumn01_05});
			this.dataGridView01.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView01.Location = new System.Drawing.Point(0, 0);
			this.dataGridView01.MultiSelect = false;
			this.dataGridView01.Name = "dataGridView01";
			this.dataGridView01.ReadOnly = true;
			this.dataGridView01.RowHeadersVisible = false;
			this.dataGridView01.RowTemplate.DividerHeight = 1;
			this.dataGridView01.RowTemplate.ReadOnly = true;
			this.dataGridView01.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView01.Size = new System.Drawing.Size(862, 399);
			this.dataGridView01.TabIndex = 16;
			this.dataGridView01.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView01_RowEnter);
			this.dataGridView01.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
			// 
			// tsIntermediateApproach
			// 
			this.tsIntermediateApproach.ImageIndex = 5;
			this.tsIntermediateApproach.Location = new System.Drawing.Point(4, 22);
			this.tsIntermediateApproach.Name = "tsIntermediateApproach";
			this.tsIntermediateApproach.Size = new System.Drawing.Size(862, 399);
			this.tsIntermediateApproach.TabIndex = 2;
			this.tsIntermediateApproach.Text = "Intermediate approach";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblCount);
			this.panel1.Controls.Add(this.numericUpDown1);
			this.panel1.Controls.Add(this.HelpBtn);
			this.panel1.Controls.Add(this.closeButton);
			this.panel1.Controls.Add(this.saveButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 425);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(870, 33);
			this.panel1.TabIndex = 4;
			// 
			// lblCount
			// 
			this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblCount.AutoSize = true;
			this.lblCount.Location = new System.Drawing.Point(9, 11);
			this.lblCount.Name = "lblCount";
			this.lblCount.Size = new System.Drawing.Size(41, 13);
			this.lblCount.TabIndex = 233;
			this.lblCount.Text = "Count :";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown1.Location = new System.Drawing.Point(443, 7);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
			this.numericUpDown1.TabIndex = 232;
			this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown1.Visible = false;
			// 
			// HelpBtn
			// 
			this.HelpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.HelpBtn.BackColor = System.Drawing.SystemColors.Control;
			this.HelpBtn.Cursor = System.Windows.Forms.Cursors.Default;
			this.HelpBtn.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HelpBtn.ForeColor = System.Drawing.SystemColors.ControlText;
			this.HelpBtn.Image = ((System.Drawing.Image)(resources.GetObject("HelpBtn.Image")));
			this.HelpBtn.Location = new System.Drawing.Point(581, 5);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(25, 25);
			this.HelpBtn.TabIndex = 231;
			this.HelpBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.HelpBtn.UseVisualStyleBackColor = false;
			this.HelpBtn.Visible = false;
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
			this.closeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.closeButton.Location = new System.Drawing.Point(746, 5);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(116, 25);
			this.closeButton.TabIndex = 230;
			this.closeButton.Text = "Close";
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
			this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveButton.Location = new System.Drawing.Point(625, 5);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(116, 25);
			this.saveButton.TabIndex = 229;
			this.saveButton.Text = "Save";
			// 
			// txtColumn01_01
			// 
			this.txtColumn01_01.HeaderText = "Type name";
			this.txtColumn01_01.Name = "txtColumn01_01";
			this.txtColumn01_01.ReadOnly = true;
			// 
			// txtColumn01_02
			// 
			this.txtColumn01_02.HeaderText = "Name";
			this.txtColumn01_02.Name = "txtColumn01_02";
			this.txtColumn01_02.ReadOnly = true;
			// 
			// fltColumn01_03
			// 
			this.fltColumn01_03.HeaderText = "H abv. ARP";
			this.fltColumn01_03.Name = "fltColumn01_03";
			this.fltColumn01_03.ReadOnly = true;
			// 
			// fltColumn01_04
			// 
			this.fltColumn01_04.HeaderText = "MOC";
			this.fltColumn01_04.Name = "fltColumn01_04";
			this.fltColumn01_04.ReadOnly = true;
			// 
			// fltColumn01_05
			// 
			this.fltColumn01_05.HeaderText = "Req. H";
			this.fltColumn01_05.Name = "fltColumn01_05";
			this.fltColumn01_05.ReadOnly = true;
			// 
			// Report
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(870, 458);
			this.Controls.Add(this.mainTabControl);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Report";
			this.ShowInTaskbar = false;
			this.Text = "Report";
			this.mainTabControl.ResumeLayout(false);
			this.tsCircklingArea.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView01)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		public System.Windows.Forms.Button HelpBtn;
		public System.Windows.Forms.Button closeButton;
		public System.Windows.Forms.Button saveButton;

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage tsCircklingArea;
		private System.Windows.Forms.TabPage tsIntermediateApproach;

		private System.Windows.Forms.DataGridView dataGridView01;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn01_01;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn01_02;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_03;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_04;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn01_05;
	}
}