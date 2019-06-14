namespace Aran.PANDA.RNAV.EnRoute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.chbForward = new System.Windows.Forms.CheckBox();
            this.chbBackward = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.dataGridView03 = new System.Windows.Forms.DataGridView();
            this.txtColumn03_01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtColumn03_02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fltColumn03_09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtColumn03_10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblObsCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView03)).BeginInit();
            this.SuspendLayout();
            // 
            // chbForward
            // 
            this.chbForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbForward.AutoSize = true;
            this.chbForward.Checked = true;
            this.chbForward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbForward.Location = new System.Drawing.Point(83, 576);
            this.chbForward.Name = "chbForward";
            this.chbForward.Size = new System.Drawing.Size(64, 17);
            this.chbForward.TabIndex = 218;
            this.chbForward.Text = "Forward";
            this.chbForward.UseVisualStyleBackColor = true;
            this.chbForward.CheckedChanged += new System.EventHandler(this.chbForward_CheckedChanged);
            // 
            // chbBackward
            // 
            this.chbBackward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbBackward.AutoSize = true;
            this.chbBackward.Checked = true;
            this.chbBackward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbBackward.Location = new System.Drawing.Point(169, 576);
            this.chbBackward.Name = "chbBackward";
            this.chbBackward.Size = new System.Drawing.Size(74, 17);
            this.chbBackward.TabIndex = 219;
            this.chbBackward.Text = "Backward";
            this.chbBackward.UseVisualStyleBackColor = true;
            this.chbBackward.CheckedChanged += new System.EventHandler(this.chbForward_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(358, 573);
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
            this.numericUpDown1.TabIndex = 217;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpBtn.BackColor = System.Drawing.SystemColors.Control;
            this.HelpBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.HelpBtn.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HelpBtn.Image = ((System.Drawing.Image)(resources.GetObject("HelpBtn.Image")));
            this.HelpBtn.Location = new System.Drawing.Point(496, 571);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(25, 25);
            this.HelpBtn.TabIndex = 123;
            this.HelpBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpBtn.UseVisualStyleBackColor = false;
            this.HelpBtn.Visible = false;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
            this.closeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.closeButton.Location = new System.Drawing.Point(661, 571);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(116, 25);
            this.closeButton.TabIndex = 122;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveButton.Location = new System.Drawing.Point(540, 571);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(116, 25);
            this.saveButton.TabIndex = 121;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // dataGridView03
            // 
            this.dataGridView03.AllowUserToAddRows = false;
            this.dataGridView03.AllowUserToDeleteRows = false;
            this.dataGridView03.AllowUserToResizeRows = false;
            this.dataGridView03.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView03.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView03.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView03.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView03.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtColumn03_01,
            this.txtColumn03_02,
            this.fltColumn03_03,
            this.fltColumn03_04,
            this.fltColumn03_05,
            this.fltColumn03_06,
            this.fltColumn03_07,
            this.fltColumn03_08,
            this.fltColumn03_09,
            this.txtColumn03_10});
            this.dataGridView03.Location = new System.Drawing.Point(1, 36);
            this.dataGridView03.MultiSelect = false;
            this.dataGridView03.Name = "dataGridView03";
            this.dataGridView03.ReadOnly = true;
            this.dataGridView03.RowHeadersVisible = false;
            this.dataGridView03.RowTemplate.DividerHeight = 1;
            this.dataGridView03.RowTemplate.ReadOnly = true;
            this.dataGridView03.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView03.Size = new System.Drawing.Size(784, 525);
            this.dataGridView03.TabIndex = 15;
            this.dataGridView03.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView03_ColumnHeaderMouseClick);
            this.dataGridView03.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView03_RowEnter);
            this.dataGridView03.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // txtColumn03_01
            // 
            this.txtColumn03_01.HeaderText = "Name";
            this.txtColumn03_01.Name = "txtColumn03_01";
            this.txtColumn03_01.ReadOnly = true;
            this.txtColumn03_01.Width = 177;
            // 
            // txtColumn03_02
            // 
            this.txtColumn03_02.HeaderText = "ID";
            this.txtColumn03_02.Name = "txtColumn03_02";
            this.txtColumn03_02.ReadOnly = true;
            // 
            // fltColumn03_03
            // 
            this.fltColumn03_03.HeaderText = "X";
            this.fltColumn03_03.Name = "fltColumn03_03";
            this.fltColumn03_03.ReadOnly = true;
            // 
            // fltColumn03_04
            // 
            this.fltColumn03_04.HeaderText = "Y";
            this.fltColumn03_04.Name = "fltColumn03_04";
            this.fltColumn03_04.ReadOnly = true;
            // 
            // fltColumn03_05
            // 
            this.fltColumn03_05.HeaderText = "Elevation";
            this.fltColumn03_05.Name = "fltColumn03_05";
            this.fltColumn03_05.ReadOnly = true;
            // 
            // fltColumn03_06
            // 
            this.fltColumn03_06.HeaderText = "MOC";
            this.fltColumn03_06.Name = "fltColumn03_06";
            this.fltColumn03_06.ReadOnly = true;
            // 
            // fltColumn03_07
            // 
            this.fltColumn03_07.HeaderText = "MOCA";
            this.fltColumn03_07.Name = "fltColumn03_07";
            this.fltColumn03_07.ReadOnly = true;
            // 
            // fltColumn03_08
            // 
            this.fltColumn03_08.HeaderText = "Hor. accuracy";
            this.fltColumn03_08.Name = "fltColumn03_08";
            this.fltColumn03_08.ReadOnly = true;
            // 
            // fltColumn03_09
            // 
            this.fltColumn03_09.HeaderText = "Ver. accuracy";
            this.fltColumn03_09.Name = "fltColumn03_09";
            this.fltColumn03_09.ReadOnly = true;
            // 
            // txtColumn03_10
            // 
            this.txtColumn03_10.HeaderText = "Area";
            this.txtColumn03_10.Name = "txtColumn03_10";
            this.txtColumn03_10.ReadOnly = true;
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "htm";
            this.saveDialog.Filter = "\"PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 577);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 220;
            this.label1.Text = "Count :";
            // 
            // lblObsCount
            // 
            this.lblObsCount.AutoSize = true;
            this.lblObsCount.Location = new System.Drawing.Point(40, 578);
            this.lblObsCount.Name = "lblObsCount";
            this.lblObsCount.Size = new System.Drawing.Size(13, 13);
            this.lblObsCount.TabIndex = 221;
            this.lblObsCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 222;
            this.label2.Text = "Search By ID :";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(93, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(173, 20);
            this.txtSearch.TabIndex = 223;
            this.txtSearch.Enter += new System.EventHandler(this.btnSearch_Click);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(281, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(86, 20);
            this.btnSearch.TabIndex = 224;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 603);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblObsCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView03);
            this.Controls.Add(this.chbBackward);
            this.Controls.Add(this.chbForward);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(740, 420);
            this.Name = "ReportForm";
            this.ShowInTaskbar = false;
            this.Text = "En-route report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReportForm_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReportForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView03)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chbForward;
		private System.Windows.Forms.CheckBox chbBackward;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		public System.Windows.Forms.Button HelpBtn;
		public System.Windows.Forms.Button closeButton;
		public System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.DataGridView dataGridView03;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn03_01;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn03_02;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_03;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_04;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_05;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_06;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_07;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_08;
		private System.Windows.Forms.DataGridViewTextBoxColumn fltColumn03_09;
		private System.Windows.Forms.DataGridViewTextBoxColumn txtColumn03_10;
		private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblObsCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
    }
}