namespace CDOTMA.CoordinatSystems
{
	partial class ProjectionDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectionDlg));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApplay = new System.Windows.Forms.Button();
			this.btnChange = new System.Windows.Forms.Button();
			this.grbGCS = new System.Windows.Forms.GroupBox();
			this.txbGCS = new System.Windows.Forms.TextBox();
			this.grbLU = new System.Windows.Forms.GroupBox();
			this.txbMetPerUnit = new System.Windows.Forms.TextBox();
			this.cmbLUName = new System.Windows.Forms.ComboBox();
			this.lblMetPerUnit = new System.Windows.Forms.Label();
			this.lblLUName = new System.Windows.Forms.Label();
			this.grbProjection = new System.Windows.Forms.GroupBox();
			this.dgvPrjParams = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmbPrName = new System.Windows.Forms.ComboBox();
			this.lblPrName = new System.Windows.Forms.Label();
			this.lblGnrName = new System.Windows.Forms.Label();
			this.txbGenName = new System.Windows.Forms.TextBox();
			this.grbGCS.SuspendLayout();
			this.grbLU.SuspendLayout();
			this.grbProjection.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvPrjParams)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(128, 461);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 24;
			this.btnOK.Text = "&OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(209, 461);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 25;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnApplay
			// 
			this.btnApplay.Enabled = false;
			this.btnApplay.Location = new System.Drawing.Point(291, 461);
			this.btnApplay.Name = "btnApplay";
			this.btnApplay.Size = new System.Drawing.Size(75, 23);
			this.btnApplay.TabIndex = 26;
			this.btnApplay.Text = "&Apply";
			this.btnApplay.UseVisualStyleBackColor = true;
			// 
			// btnChange
			// 
			this.btnChange.Enabled = false;
			this.btnChange.Location = new System.Drawing.Point(272, 21);
			this.btnChange.Name = "btnChange";
			this.btnChange.Size = new System.Drawing.Size(75, 23);
			this.btnChange.TabIndex = 27;
			this.btnChange.Text = "C&hange...";
			this.btnChange.UseVisualStyleBackColor = true;
			// 
			// grbGCS
			// 
			this.grbGCS.Controls.Add(this.txbGCS);
			this.grbGCS.Controls.Add(this.btnChange);
			this.grbGCS.Location = new System.Drawing.Point(9, 321);
			this.grbGCS.Name = "grbGCS";
			this.grbGCS.Size = new System.Drawing.Size(357, 129);
			this.grbGCS.TabIndex = 28;
			this.grbGCS.TabStop = false;
			this.grbGCS.Text = "Geographic Coordinate System";
			// 
			// txbGCS
			// 
			this.txbGCS.BackColor = System.Drawing.SystemColors.Window;
			this.txbGCS.Location = new System.Drawing.Point(10, 21);
			this.txbGCS.Multiline = true;
			this.txbGCS.Name = "txbGCS";
			this.txbGCS.ReadOnly = true;
			this.txbGCS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txbGCS.Size = new System.Drawing.Size(248, 98);
			this.txbGCS.TabIndex = 28;
			this.txbGCS.Text = resources.GetString("txbGCS.Text");
			this.txbGCS.WordWrap = false;
			// 
			// grbLU
			// 
			this.grbLU.Controls.Add(this.txbMetPerUnit);
			this.grbLU.Controls.Add(this.cmbLUName);
			this.grbLU.Controls.Add(this.lblMetPerUnit);
			this.grbLU.Controls.Add(this.lblLUName);
			this.grbLU.Location = new System.Drawing.Point(9, 225);
			this.grbLU.Name = "grbLU";
			this.grbLU.Size = new System.Drawing.Size(357, 86);
			this.grbLU.TabIndex = 29;
			this.grbLU.TabStop = false;
			this.grbLU.Text = "Linear Unit";
			// 
			// txbMetPerUnit
			// 
			this.txbMetPerUnit.Location = new System.Drawing.Point(128, 53);
			this.txbMetPerUnit.Name = "txbMetPerUnit";
			this.txbMetPerUnit.Size = new System.Drawing.Size(219, 20);
			this.txbMetPerUnit.TabIndex = 3;
			this.txbMetPerUnit.Text = "1";
			this.txbMetPerUnit.Validating += new System.ComponentModel.CancelEventHandler(this.txbMetPerUnit_Validating);
			// 
			// cmbLUName
			// 
			this.cmbLUName.FormattingEnabled = true;
			this.cmbLUName.Items.AddRange(new object[] {
            "Meter",
            "KM"});
			this.cmbLUName.Location = new System.Drawing.Point(128, 21);
			this.cmbLUName.Name = "cmbLUName";
			this.cmbLUName.Size = new System.Drawing.Size(219, 21);
			this.cmbLUName.TabIndex = 2;
			this.cmbLUName.Text = "Meter";
			this.cmbLUName.SelectedIndexChanged += new System.EventHandler(this.cmbLUName_SelectedIndexChanged);
			// 
			// lblMetPerUnit
			// 
			this.lblMetPerUnit.AutoSize = true;
			this.lblMetPerUnit.Location = new System.Drawing.Point(10, 52);
			this.lblMetPerUnit.Name = "lblMetPerUnit";
			this.lblMetPerUnit.Size = new System.Drawing.Size(80, 13);
			this.lblMetPerUnit.TabIndex = 1;
			this.lblMetPerUnit.Text = "Meters per unit:";
			// 
			// lblLUName
			// 
			this.lblLUName.AutoSize = true;
			this.lblLUName.Location = new System.Drawing.Point(10, 21);
			this.lblLUName.Name = "lblLUName";
			this.lblLUName.Size = new System.Drawing.Size(38, 13);
			this.lblLUName.TabIndex = 0;
			this.lblLUName.Text = "Name:";
			// 
			// grbProjection
			// 
			this.grbProjection.Controls.Add(this.dgvPrjParams);
			this.grbProjection.Controls.Add(this.cmbPrName);
			this.grbProjection.Controls.Add(this.lblPrName);
			this.grbProjection.Location = new System.Drawing.Point(9, 43);
			this.grbProjection.Name = "grbProjection";
			this.grbProjection.Size = new System.Drawing.Size(357, 172);
			this.grbProjection.TabIndex = 30;
			this.grbProjection.TabStop = false;
			this.grbProjection.Text = "Projection";
			// 
			// dgvPrjParams
			// 
			this.dgvPrjParams.AllowUserToAddRows = false;
			this.dgvPrjParams.AllowUserToDeleteRows = false;
			this.dgvPrjParams.AllowUserToResizeColumns = false;
			this.dgvPrjParams.AllowUserToResizeRows = false;
			this.dgvPrjParams.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvPrjParams.ColumnHeadersHeight = 20;
			this.dgvPrjParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvPrjParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.dgvPrjParams.Location = new System.Drawing.Point(10, 53);
			this.dgvPrjParams.Name = "dgvPrjParams";
			this.dgvPrjParams.RowHeadersVisible = false;
			this.dgvPrjParams.RowTemplate.Height = 18;
			this.dgvPrjParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvPrjParams.Size = new System.Drawing.Size(337, 108);
			this.dgvPrjParams.TabIndex = 4;
			this.dgvPrjParams.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrjParams_CellEndEdit);
			// 
			// Column1
			// 
			this.Column1.HeaderText = "Parameter";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column1.Width = 150;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Value";
			this.Column2.Name = "Column2";
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column2.Width = 150;
			// 
			// cmbPrName
			// 
			this.cmbPrName.FormattingEnabled = true;
			this.cmbPrName.Items.AddRange(new object[] {
            "Transverse_Mercator"});
			this.cmbPrName.Location = new System.Drawing.Point(128, 22);
			this.cmbPrName.Name = "cmbPrName";
			this.cmbPrName.Size = new System.Drawing.Size(219, 21);
			this.cmbPrName.TabIndex = 3;
			this.cmbPrName.Text = "Transverse_Mercator";
			this.cmbPrName.SelectedIndexChanged += new System.EventHandler(this.cmbPrName_SelectedIndexChanged);
			// 
			// lblPrName
			// 
			this.lblPrName.AutoSize = true;
			this.lblPrName.Location = new System.Drawing.Point(10, 22);
			this.lblPrName.Name = "lblPrName";
			this.lblPrName.Size = new System.Drawing.Size(38, 13);
			this.lblPrName.TabIndex = 0;
			this.lblPrName.Text = "Name:";
			// 
			// lblGnrName
			// 
			this.lblGnrName.AutoSize = true;
			this.lblGnrName.BackColor = System.Drawing.Color.Transparent;
			this.lblGnrName.Location = new System.Drawing.Point(14, 13);
			this.lblGnrName.Name = "lblGnrName";
			this.lblGnrName.Size = new System.Drawing.Size(38, 13);
			this.lblGnrName.TabIndex = 31;
			this.lblGnrName.Text = "Name:";
			// 
			// txbGenName
			// 
			this.txbGenName.Location = new System.Drawing.Point(137, 13);
			this.txbGenName.Name = "txbGenName";
			this.txbGenName.Size = new System.Drawing.Size(219, 20);
			this.txbGenName.TabIndex = 32;
			this.txbGenName.Text = "New_Projected_Coordinate_System";
			// 
			// ProjectionBox
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(375, 490);
			this.Controls.Add(this.txbGenName);
			this.Controls.Add(this.lblGnrName);
			this.Controls.Add(this.grbProjection);
			this.Controls.Add(this.grbLU);
			this.Controls.Add(this.grbGCS);
			this.Controls.Add(this.btnApplay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectionBox";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Projected Coordinate System";
			this.grbGCS.ResumeLayout(false);
			this.grbGCS.PerformLayout();
			this.grbLU.ResumeLayout(false);
			this.grbLU.PerformLayout();
			this.grbProjection.ResumeLayout(false);
			this.grbProjection.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvPrjParams)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApplay;
		private System.Windows.Forms.Button btnChange;
		private System.Windows.Forms.GroupBox grbGCS;
		private System.Windows.Forms.TextBox txbGCS;
		private System.Windows.Forms.GroupBox grbLU;
		private System.Windows.Forms.ComboBox cmbLUName;
		private System.Windows.Forms.Label lblMetPerUnit;
		private System.Windows.Forms.Label lblLUName;
		private System.Windows.Forms.TextBox txbMetPerUnit;
		private System.Windows.Forms.GroupBox grbProjection;
		private System.Windows.Forms.ComboBox cmbPrName;
		private System.Windows.Forms.Label lblPrName;
		private System.Windows.Forms.DataGridView dgvPrjParams;
		private System.Windows.Forms.Label lblGnrName;
		private System.Windows.Forms.TextBox txbGenName;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
	}
}
