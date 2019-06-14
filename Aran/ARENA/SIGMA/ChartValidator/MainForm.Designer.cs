namespace ChartValidator
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgViewEnroute = new System.Windows.Forms.DataGridView();
			this.dgvTxtBxColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvTxtBxColDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.zoomToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblCount = new System.Windows.Forms.Label();
			this.tbCntrlMain = new System.Windows.Forms.TabControl();
			this.tabPageEnroute = new System.Windows.Forms.TabPage();
			this.tabPageRouteSegment = new System.Windows.Forms.TabPage();
			this.dgViewRouteSegment = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageNavaid = new System.Windows.Forms.TabPage();
			this.dgViewNavaid = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageAirspace = new System.Windows.Forms.TabPage();
			this.dgViewAirspace = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageHolding = new System.Windows.Forms.TabPage();
			this.dgViewHolding = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageWayPoint = new System.Windows.Forms.TabPage();
			this.dgViewWayPoint = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageSID = new System.Windows.Forms.TabPage();
			this.dgViewSID = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageDepartureLeg = new System.Windows.Forms.TabPage();
			this.dgViewDepartureLeg = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lblDescription = new System.Windows.Forms.Label();
			this.btnInfo = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgViewEnroute)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.tbCntrlMain.SuspendLayout();
			this.tabPageEnroute.SuspendLayout();
			this.tabPageRouteSegment.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewRouteSegment)).BeginInit();
			this.tabPageNavaid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewNavaid)).BeginInit();
			this.tabPageAirspace.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewAirspace)).BeginInit();
			this.tabPageHolding.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewHolding)).BeginInit();
			this.tabPageWayPoint.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewWayPoint)).BeginInit();
			this.tabPageSID.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewSID)).BeginInit();
			this.tabPageDepartureLeg.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgViewDepartureLeg)).BeginInit();
			this.SuspendLayout();
			// 
			// dgViewEnroute
			// 
			this.dgViewEnroute.AllowUserToAddRows = false;
			this.dgViewEnroute.AllowUserToDeleteRows = false;
			this.dgViewEnroute.AllowUserToOrderColumns = true;
			this.dgViewEnroute.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewEnroute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewEnroute.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTxtBxColName,
            this.dgvTxtBxColDesc});
			this.dgViewEnroute.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewEnroute.Location = new System.Drawing.Point(3, 3);
			this.dgViewEnroute.MultiSelect = false;
			this.dgViewEnroute.Name = "dgViewEnroute";
			this.dgViewEnroute.RowHeadersVisible = false;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgViewEnroute.RowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dgViewEnroute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewEnroute.Size = new System.Drawing.Size(547, 274);
			this.dgViewEnroute.TabIndex = 0;
			this.dgViewEnroute.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewEnroute_RowEnter);
			// 
			// dgvTxtBxColName
			// 
			this.dgvTxtBxColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvTxtBxColName.FillWeight = 55F;
			this.dgvTxtBxColName.HeaderText = "Name";
			this.dgvTxtBxColName.Name = "dgvTxtBxColName";
			this.dgvTxtBxColName.ReadOnly = true;
			// 
			// dgvTxtBxColDesc
			// 
			this.dgvTxtBxColDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvTxtBxColDesc.FillWeight = 65F;
			this.dgvTxtBxColDesc.HeaderText = "Details";
			this.dgvTxtBxColDesc.Name = "dgvTxtBxColDesc";
			this.dgvTxtBxColDesc.ReadOnly = true;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(124, 26);
			// 
			// zoomToToolStripMenuItem
			// 
			this.zoomToToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("zoomToToolStripMenuItem.Image")));
			this.zoomToToolStripMenuItem.Name = "zoomToToolStripMenuItem";
			this.zoomToToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
			this.zoomToToolStripMenuItem.Text = "Zoom To";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(492, 315);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(69, 25);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblCount
			// 
			this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblCount.AutoSize = true;
			this.lblCount.Location = new System.Drawing.Point(32, 320);
			this.lblCount.Name = "lblCount";
			this.lblCount.Size = new System.Drawing.Size(83, 13);
			this.lblCount.TabIndex = 2;
			this.lblCount.Text = "Warning count :";
			// 
			// tbCntrlMain
			// 
			this.tbCntrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCntrlMain.Controls.Add(this.tabPageEnroute);
			this.tbCntrlMain.Controls.Add(this.tabPageRouteSegment);
			this.tbCntrlMain.Controls.Add(this.tabPageNavaid);
			this.tbCntrlMain.Controls.Add(this.tabPageAirspace);
			this.tbCntrlMain.Controls.Add(this.tabPageHolding);
			this.tbCntrlMain.Controls.Add(this.tabPageWayPoint);
			this.tbCntrlMain.Controls.Add(this.tabPageSID);
			this.tbCntrlMain.Controls.Add(this.tabPageDepartureLeg);
			this.tbCntrlMain.Location = new System.Drawing.Point(1, 3);
			this.tbCntrlMain.Name = "tbCntrlMain";
			this.tbCntrlMain.SelectedIndex = 0;
			this.tbCntrlMain.Size = new System.Drawing.Size(561, 306);
			this.tbCntrlMain.TabIndex = 3;
			this.tbCntrlMain.SelectedIndexChanged += new System.EventHandler(this.tbCntrlMain_SelectedIndexChanged);
			// 
			// tabPageEnroute
			// 
			this.tabPageEnroute.Controls.Add(this.dgViewEnroute);
			this.tabPageEnroute.Location = new System.Drawing.Point(4, 22);
			this.tabPageEnroute.Name = "tabPageEnroute";
			this.tabPageEnroute.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageEnroute.Size = new System.Drawing.Size(553, 280);
			this.tabPageEnroute.TabIndex = 0;
			this.tabPageEnroute.Text = "Enroute";
			this.tabPageEnroute.UseVisualStyleBackColor = true;
			// 
			// tabPageRouteSegment
			// 
			this.tabPageRouteSegment.Controls.Add(this.dgViewRouteSegment);
			this.tabPageRouteSegment.Location = new System.Drawing.Point(4, 22);
			this.tabPageRouteSegment.Name = "tabPageRouteSegment";
			this.tabPageRouteSegment.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRouteSegment.Size = new System.Drawing.Size(553, 280);
			this.tabPageRouteSegment.TabIndex = 1;
			this.tabPageRouteSegment.Text = "Route Segment";
			this.tabPageRouteSegment.UseVisualStyleBackColor = true;
			// 
			// dgViewRouteSegment
			// 
			this.dgViewRouteSegment.AllowUserToAddRows = false;
			this.dgViewRouteSegment.AllowUserToDeleteRows = false;
			this.dgViewRouteSegment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgViewRouteSegment.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewRouteSegment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewRouteSegment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
			this.dgViewRouteSegment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewRouteSegment.Location = new System.Drawing.Point(3, 3);
			this.dgViewRouteSegment.MultiSelect = false;
			this.dgViewRouteSegment.Name = "dgViewRouteSegment";
			this.dgViewRouteSegment.RowHeadersVisible = false;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgViewRouteSegment.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.dgViewRouteSegment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewRouteSegment.Size = new System.Drawing.Size(547, 274);
			this.dgViewRouteSegment.TabIndex = 1;
			this.dgViewRouteSegment.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewRouteSegment_RowEnter);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.FillWeight = 24.31206F;
			this.dataGridViewTextBoxColumn1.HeaderText = "Name";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.FillWeight = 65.49461F;
			this.dataGridViewTextBoxColumn2.HeaderText = "Details";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			// 
			// tabPageNavaid
			// 
			this.tabPageNavaid.Controls.Add(this.dgViewNavaid);
			this.tabPageNavaid.Location = new System.Drawing.Point(4, 22);
			this.tabPageNavaid.Name = "tabPageNavaid";
			this.tabPageNavaid.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageNavaid.Size = new System.Drawing.Size(553, 280);
			this.tabPageNavaid.TabIndex = 2;
			this.tabPageNavaid.Text = "Navaid";
			this.tabPageNavaid.UseVisualStyleBackColor = true;
			// 
			// dgViewNavaid
			// 
			this.dgViewNavaid.AllowUserToAddRows = false;
			this.dgViewNavaid.AllowUserToDeleteRows = false;
			this.dgViewNavaid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgViewNavaid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewNavaid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewNavaid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
			this.dgViewNavaid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewNavaid.Location = new System.Drawing.Point(3, 3);
			this.dgViewNavaid.MultiSelect = false;
			this.dgViewNavaid.Name = "dgViewNavaid";
			this.dgViewNavaid.RowHeadersVisible = false;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgViewNavaid.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dgViewNavaid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewNavaid.Size = new System.Drawing.Size(547, 274);
			this.dgViewNavaid.TabIndex = 2;
			this.dgViewNavaid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewNavaid_RowEnter);
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.FillWeight = 24.31206F;
			this.dataGridViewTextBoxColumn3.HeaderText = "Name";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.FillWeight = 65.49461F;
			this.dataGridViewTextBoxColumn4.HeaderText = "Details";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			// 
			// tabPageAirspace
			// 
			this.tabPageAirspace.Controls.Add(this.dgViewAirspace);
			this.tabPageAirspace.Location = new System.Drawing.Point(4, 22);
			this.tabPageAirspace.Name = "tabPageAirspace";
			this.tabPageAirspace.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageAirspace.Size = new System.Drawing.Size(553, 280);
			this.tabPageAirspace.TabIndex = 3;
			this.tabPageAirspace.Text = "Airspace";
			this.tabPageAirspace.UseVisualStyleBackColor = true;
			// 
			// dgViewAirspace
			// 
			this.dgViewAirspace.AllowUserToAddRows = false;
			this.dgViewAirspace.AllowUserToDeleteRows = false;
			this.dgViewAirspace.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgViewAirspace.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewAirspace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewAirspace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
			this.dgViewAirspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewAirspace.Location = new System.Drawing.Point(3, 3);
			this.dgViewAirspace.MultiSelect = false;
			this.dgViewAirspace.Name = "dgViewAirspace";
			this.dgViewAirspace.RowHeadersVisible = false;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgViewAirspace.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgViewAirspace.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewAirspace.Size = new System.Drawing.Size(547, 274);
			this.dgViewAirspace.TabIndex = 2;
			this.dgViewAirspace.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewAirspace_RowEnter);
			// 
			// dataGridViewTextBoxColumn5
			// 
			this.dataGridViewTextBoxColumn5.FillWeight = 24.31206F;
			this.dataGridViewTextBoxColumn5.HeaderText = "Name";
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn6
			// 
			this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn6.FillWeight = 65.49461F;
			this.dataGridViewTextBoxColumn6.HeaderText = "Details";
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			// 
			// tabPageHolding
			// 
			this.tabPageHolding.Controls.Add(this.dgViewHolding);
			this.tabPageHolding.Location = new System.Drawing.Point(4, 22);
			this.tabPageHolding.Name = "tabPageHolding";
			this.tabPageHolding.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageHolding.Size = new System.Drawing.Size(553, 280);
			this.tabPageHolding.TabIndex = 4;
			this.tabPageHolding.Text = "Holding";
			this.tabPageHolding.UseVisualStyleBackColor = true;
			// 
			// dgViewHolding
			// 
			this.dgViewHolding.AllowUserToAddRows = false;
			this.dgViewHolding.AllowUserToDeleteRows = false;
			this.dgViewHolding.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgViewHolding.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewHolding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewHolding.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
			this.dgViewHolding.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewHolding.Location = new System.Drawing.Point(3, 3);
			this.dgViewHolding.MultiSelect = false;
			this.dgViewHolding.Name = "dgViewHolding";
			this.dgViewHolding.RowHeadersVisible = false;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgViewHolding.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dgViewHolding.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewHolding.Size = new System.Drawing.Size(547, 274);
			this.dgViewHolding.TabIndex = 2;
			this.dgViewHolding.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewHolding_RowEnter);
			// 
			// dataGridViewTextBoxColumn7
			// 
			this.dataGridViewTextBoxColumn7.FillWeight = 24.31206F;
			this.dataGridViewTextBoxColumn7.HeaderText = "Name";
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn8
			// 
			this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn8.FillWeight = 65.49461F;
			this.dataGridViewTextBoxColumn8.HeaderText = "Details";
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			// 
			// tabPageWayPoint
			// 
			this.tabPageWayPoint.Controls.Add(this.dgViewWayPoint);
			this.tabPageWayPoint.Location = new System.Drawing.Point(4, 22);
			this.tabPageWayPoint.Name = "tabPageWayPoint";
			this.tabPageWayPoint.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageWayPoint.Size = new System.Drawing.Size(553, 280);
			this.tabPageWayPoint.TabIndex = 5;
			this.tabPageWayPoint.Text = "Way Point";
			this.tabPageWayPoint.UseVisualStyleBackColor = true;
			// 
			// dgViewWayPoint
			// 
			this.dgViewWayPoint.AllowUserToAddRows = false;
			this.dgViewWayPoint.AllowUserToDeleteRows = false;
			this.dgViewWayPoint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgViewWayPoint.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewWayPoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewWayPoint.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
			this.dgViewWayPoint.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewWayPoint.Location = new System.Drawing.Point(3, 3);
			this.dgViewWayPoint.MultiSelect = false;
			this.dgViewWayPoint.Name = "dgViewWayPoint";
			this.dgViewWayPoint.RowHeadersVisible = false;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgViewWayPoint.RowsDefaultCellStyle = dataGridViewCellStyle6;
			this.dgViewWayPoint.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewWayPoint.Size = new System.Drawing.Size(547, 274);
			this.dgViewWayPoint.TabIndex = 2;
			this.dgViewWayPoint.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewWayPoint_RowEnter);
			// 
			// dataGridViewTextBoxColumn9
			// 
			this.dataGridViewTextBoxColumn9.FillWeight = 24.31206F;
			this.dataGridViewTextBoxColumn9.HeaderText = "Name";
			this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			this.dataGridViewTextBoxColumn9.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn10
			// 
			this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn10.FillWeight = 65.49461F;
			this.dataGridViewTextBoxColumn10.HeaderText = "Details";
			this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			this.dataGridViewTextBoxColumn10.ReadOnly = true;
			// 
			// tabPageSID
			// 
			this.tabPageSID.Controls.Add(this.dgViewSID);
			this.tabPageSID.Location = new System.Drawing.Point(4, 22);
			this.tabPageSID.Name = "tabPageSID";
			this.tabPageSID.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSID.Size = new System.Drawing.Size(553, 280);
			this.tabPageSID.TabIndex = 6;
			this.tabPageSID.Text = "Procedure";
			this.tabPageSID.UseVisualStyleBackColor = true;
			// 
			// dgViewSID
			// 
			this.dgViewSID.AllowUserToAddRows = false;
			this.dgViewSID.AllowUserToDeleteRows = false;
			this.dgViewSID.AllowUserToOrderColumns = true;
			this.dgViewSID.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewSID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewSID.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
			this.dgViewSID.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewSID.Location = new System.Drawing.Point(3, 3);
			this.dgViewSID.MultiSelect = false;
			this.dgViewSID.Name = "dgViewSID";
			this.dgViewSID.RowHeadersVisible = false;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgViewSID.RowsDefaultCellStyle = dataGridViewCellStyle7;
			this.dgViewSID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewSID.Size = new System.Drawing.Size(547, 274);
			this.dgViewSID.TabIndex = 1;
			this.dgViewSID.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewSID_RowEnter);
			// 
			// dataGridViewTextBoxColumn11
			// 
			this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn11.FillWeight = 55F;
			this.dataGridViewTextBoxColumn11.HeaderText = "Name";
			this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			this.dataGridViewTextBoxColumn11.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn12
			// 
			this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn12.FillWeight = 65F;
			this.dataGridViewTextBoxColumn12.HeaderText = "Details";
			this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			this.dataGridViewTextBoxColumn12.ReadOnly = true;
			// 
			// tabPageDepartureLeg
			// 
			this.tabPageDepartureLeg.Controls.Add(this.dgViewDepartureLeg);
			this.tabPageDepartureLeg.Location = new System.Drawing.Point(4, 22);
			this.tabPageDepartureLeg.Name = "tabPageDepartureLeg";
			this.tabPageDepartureLeg.Size = new System.Drawing.Size(553, 280);
			this.tabPageDepartureLeg.TabIndex = 7;
			this.tabPageDepartureLeg.Text = "Leg";
			this.tabPageDepartureLeg.UseVisualStyleBackColor = true;
			// 
			// dgViewDepartureLeg
			// 
			this.dgViewDepartureLeg.AllowUserToAddRows = false;
			this.dgViewDepartureLeg.AllowUserToDeleteRows = false;
			this.dgViewDepartureLeg.AllowUserToOrderColumns = true;
			this.dgViewDepartureLeg.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgViewDepartureLeg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgViewDepartureLeg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
			this.dgViewDepartureLeg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgViewDepartureLeg.Location = new System.Drawing.Point(0, 0);
			this.dgViewDepartureLeg.MultiSelect = false;
			this.dgViewDepartureLeg.Name = "dgViewDepartureLeg";
			this.dgViewDepartureLeg.RowHeadersVisible = false;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgViewDepartureLeg.RowsDefaultCellStyle = dataGridViewCellStyle8;
			this.dgViewDepartureLeg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgViewDepartureLeg.Size = new System.Drawing.Size(553, 280);
			this.dgViewDepartureLeg.TabIndex = 1;
			this.dgViewDepartureLeg.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgViewDepartureLeg_RowEnter);
			// 
			// dataGridViewTextBoxColumn13
			// 
			this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn13.FillWeight = 55F;
			this.dataGridViewTextBoxColumn13.HeaderText = "Name";
			this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			this.dataGridViewTextBoxColumn13.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn14
			// 
			this.dataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn14.FillWeight = 65F;
			this.dataGridViewTextBoxColumn14.HeaderText = "Details";
			this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			this.dataGridViewTextBoxColumn14.ReadOnly = true;
			// 
			// lblDescription
			// 
			this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(199, 314);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(145, 26);
			this.lblDescription.TabIndex = 4;
			this.lblDescription.Text = "Calculated values are on the \r\nleft side of drop (/) symbol";
			// 
			// btnInfo
			// 
			this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInfo.BackgroundImage")));
			this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnInfo.Location = new System.Drawing.Point(1, 315);
			this.btnInfo.Name = "btnInfo";
			this.btnInfo.Size = new System.Drawing.Size(25, 25);
			this.btnInfo.TabIndex = 5;
			this.btnInfo.UseVisualStyleBackColor = true;
			this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(421, 315);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(69, 25);
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(350, 315);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(69, 25);
			this.button1.TabIndex = 7;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(572, 352);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnInfo);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.tbCntrlMain);
			this.Controls.Add(this.lblCount);
			this.Controls.Add(this.btnOk);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(580, 380);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Validation Report ";
			((System.ComponentModel.ISupportInitialize)(this.dgViewEnroute)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.tbCntrlMain.ResumeLayout(false);
			this.tabPageEnroute.ResumeLayout(false);
			this.tabPageRouteSegment.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewRouteSegment)).EndInit();
			this.tabPageNavaid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewNavaid)).EndInit();
			this.tabPageAirspace.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewAirspace)).EndInit();
			this.tabPageHolding.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewHolding)).EndInit();
			this.tabPageWayPoint.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewWayPoint)).EndInit();
			this.tabPageSID.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewSID)).EndInit();
			this.tabPageDepartureLeg.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgViewDepartureLeg)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgViewEnroute;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.TabControl tbCntrlMain;
		private System.Windows.Forms.TabPage tabPageEnroute;
		private System.Windows.Forms.TabPage tabPageRouteSegment;
		private System.Windows.Forms.TabPage tabPageNavaid;
		private System.Windows.Forms.TabPage tabPageAirspace;
		private System.Windows.Forms.TabPage tabPageHolding;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem zoomToToolStripMenuItem;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TabPage tabPageWayPoint;
		private System.Windows.Forms.DataGridView dgViewRouteSegment;
		private System.Windows.Forms.DataGridView dgViewNavaid;
		private System.Windows.Forms.DataGridView dgViewAirspace;
		private System.Windows.Forms.DataGridView dgViewHolding;
		private System.Windows.Forms.DataGridView dgViewWayPoint;
		private System.Windows.Forms.Button btnInfo;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxColName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvTxtBxColDesc;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
		private System.Windows.Forms.TabPage tabPageSID;
		private System.Windows.Forms.DataGridView dgViewSID;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
		private System.Windows.Forms.TabPage tabPageDepartureLeg;
		private System.Windows.Forms.DataGridView dgViewDepartureLeg;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.Button button1;
	}
}