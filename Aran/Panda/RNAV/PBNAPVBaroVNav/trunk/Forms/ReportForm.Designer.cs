namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.tsIntermediateApproach = new System.Windows.Forms.TabPage();
			this.listView2 = new System.Windows.Forms.ListView();
			this._column_20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tsAPVSegment = new System.Windows.Forms.TabPage();
			this.listView1 = new System.Windows.Forms.ListView();
			this._column_9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._column_19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.mainTabControl = new System.Windows.Forms.TabControl();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.helpBtn = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.SaveDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.tsIntermediateApproach.SuspendLayout();
			this.tsAPVSegment.SuspendLayout();
			this.mainTabControl.SuspendLayout();
			this.Panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.imageList1.Images.SetKeyName(0, "");
			this.imageList1.Images.SetKeyName(1, "");
			this.imageList1.Images.SetKeyName(2, "space.ico");
			this.imageList1.Images.SetKeyName(3, "");
			// 
			// tsIntermediateApproach
			// 
			this.tsIntermediateApproach.Controls.Add(this.listView2);
			this.tsIntermediateApproach.ImageIndex = 5;
			this.tsIntermediateApproach.Location = new System.Drawing.Point(4, 22);
			this.tsIntermediateApproach.Name = "tsIntermediateApproach";
			this.tsIntermediateApproach.Size = new System.Drawing.Size(787, 455);
			this.tsIntermediateApproach.TabIndex = 2;
			this.tsIntermediateApproach.Text = "Intermediate approach";
			// 
			// listView2
			// 
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._column_20,
            this._column_21,
            this._column_22,
            this._column_23,
            this._column_24,
            this._column_25,
            this._column_26,
            this._column_27});
			this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			this.listView2.HideSelection = false;
			this.listView2.Location = new System.Drawing.Point(0, 0);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(787, 455);
			this.listView2.SmallImageList = this.imageList1;
			this.listView2.TabIndex = 0;
			this.listView2.Tag = 2;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Details;
			this.listView2.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView2_ColumnClick);
			this.listView2.SelectedIndexChanged += new System.EventHandler(this.listViews_SelectedIndexChanged);
			// 
			// _column_20
			// 
			this._column_20.Name = "_column_20";
			this._column_20.Text = "Type";
			// 
			// _column_21
			// 
			this._column_21.Name = "_column_21";
			this._column_21.Text = "Name";
			// 
			// _column_22
			// 
			this._column_22.Name = "_column_22";
			this._column_22.Text = "H Abv. Tresh.";
			// 
			// _column_23
			// 
			this._column_23.Name = "_column_23";
			this._column_23.Text = "MOC";
			// 
			// _column_24
			// 
			this._column_24.Name = "_column_24";
			this._column_24.Text = "Req. OCH";
			// 
			// _column_25
			// 
			this._column_25.Name = "_column_25";
			this._column_25.Text = "X";
			// 
			// _column_26
			// 
			this._column_26.Name = "_column_26";
			this._column_26.Text = "Y";
			// 
			// _column_27
			// 
			this._column_27.Name = "_column_27";
			this._column_27.Text = "Area";
			// 
			// tsAPVSegment
			// 
			this.tsAPVSegment.Controls.Add(this.listView1);
			this.tsAPVSegment.ImageIndex = 1;
			this.tsAPVSegment.Location = new System.Drawing.Point(4, 22);
			this.tsAPVSegment.Name = "tsAPVSegment";
			this.tsAPVSegment.Size = new System.Drawing.Size(787, 455);
			this.tsAPVSegment.TabIndex = 1;
			this.tsAPVSegment.Text = "APV Segment";
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._column_9,
            this._column_10,
            this._column_11,
            this._column_12,
            this.columnHeader1,
            this._column_13,
            this._column_14,
            this._column_15,
            this.columnHeader2,
            this._column_16,
            this._column_17,
            this._column_18,
            this._column_19});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(787, 455);
			this.listView1.SmallImageList = this.imageList1;
			this.listView1.TabIndex = 0;
			this.listView1.Tag = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listViews_SelectedIndexChanged);
			// 
			// _column_9
			// 
			this._column_9.Name = "_column_9";
			this._column_9.Text = "Type";
			// 
			// _column_10
			// 
			this._column_10.Name = "_column_10";
			this._column_10.Text = "Name";
			// 
			// _column_11
			// 
			this._column_11.Name = "_column_11";
			this._column_11.Text = "H surface";
			// 
			// _column_12
			// 
			this._column_12.Name = "_column_12";
			this._column_12.Text = "H Abv. Tresh.";
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Effective height";
			// 
			// _column_13
			// 
			this._column_13.Name = "_column_13";
			this._column_13.Text = "Penetration";
			// 
			// _column_14
			// 
			this._column_14.Name = "_column_14";
			this._column_14.Text = "Secondary area coefficient";
			// 
			// _column_15
			// 
			this._column_15.Name = "_column_15";
			this._column_15.Text = "Req. OCH";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "PANS-OPS OCH";
			// 
			// _column_16
			// 
			this._column_16.Name = "_column_16";
			this._column_16.Text = "X";
			// 
			// _column_17
			// 
			this._column_17.Name = "_column_17";
			this._column_17.Text = "Y";
			// 
			// _column_18
			// 
			this._column_18.Name = "_column_18";
			this._column_18.Text = "Surface";
			// 
			// _column_19
			// 
			this._column_19.Name = "_column_19";
			this._column_19.Text = "Area";
			// 
			// mainTabControl
			// 
			this.mainTabControl.Controls.Add(this.tsAPVSegment);
			this.mainTabControl.Controls.Add(this.tsIntermediateApproach);
			this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTabControl.Location = new System.Drawing.Point(0, 0);
			this.mainTabControl.Name = "mainTabControl";
			this.mainTabControl.SelectedIndex = 0;
			this.mainTabControl.Size = new System.Drawing.Size(795, 481);
			this.mainTabControl.TabIndex = 2;
			this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
			// 
			// Panel1
			// 
			this.Panel1.Controls.Add(this.helpBtn);
			this.Panel1.Controls.Add(this.closeButton);
			this.Panel1.Controls.Add(this.saveButton);
			this.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.Panel1.Location = new System.Drawing.Point(0, 481);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(795, 50);
			this.Panel1.TabIndex = 15;
			// 
			// helpBtn
			// 
			this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.helpBtn.Cursor = System.Windows.Forms.Cursors.Default;
			this.helpBtn.Image = ((System.Drawing.Image)(resources.GetObject("helpBtn.Image")));
			this.helpBtn.Location = new System.Drawing.Point(516, 11);
			this.helpBtn.Name = "helpBtn";
			this.helpBtn.Size = new System.Drawing.Size(25, 25);
			this.helpBtn.TabIndex = 16;
			this.helpBtn.UseVisualStyleBackColor = false;
			this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
			this.closeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.closeButton.Location = new System.Drawing.Point(667, 11);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(116, 25);
			this.closeButton.TabIndex = 15;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
			this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveButton.Location = new System.Drawing.Point(546, 11);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(116, 25);
			this.saveButton.TabIndex = 14;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// SaveDialog1
			// 
			this.SaveDialog1.DefaultExt = "txt";
			this.SaveDialog1.Filter = "PANDA Report File|*.htm|All Files (*.*)|*.*";
			// 
			// ReportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(795, 531);
			this.Controls.Add(this.mainTabControl);
			this.Controls.Add(this.Panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ReportForm";
			this.ShowInTaskbar = false;
			this.Text = "ReportForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportForm_FormClosing);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ReportForm_KeyUp);
			this.tsIntermediateApproach.ResumeLayout(false);
			this.tsAPVSegment.ResumeLayout(false);
			this.mainTabControl.ResumeLayout(false);
			this.Panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ImageList imageList1;

		private System.Windows.Forms.TabControl mainTabControl;
		private System.Windows.Forms.TabPage tsIntermediateApproach;
		private System.Windows.Forms.TabPage tsAPVSegment;
		public System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader _column_9;
		private System.Windows.Forms.ColumnHeader _column_10;
		private System.Windows.Forms.ColumnHeader _column_11;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader _column_12;
		private System.Windows.Forms.ColumnHeader _column_13;
		private System.Windows.Forms.ColumnHeader _column_14;
		private System.Windows.Forms.ColumnHeader _column_15;
		private System.Windows.Forms.ColumnHeader _column_16;
		private System.Windows.Forms.ColumnHeader _column_17;
		private System.Windows.Forms.ColumnHeader _column_18;
		private System.Windows.Forms.ColumnHeader _column_19;
		public System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.ColumnHeader _column_20;
		private System.Windows.Forms.ColumnHeader _column_21;
		private System.Windows.Forms.ColumnHeader _column_22;
		private System.Windows.Forms.ColumnHeader _column_23;
		private System.Windows.Forms.ColumnHeader _column_24;
		private System.Windows.Forms.ColumnHeader _column_25;
		private System.Windows.Forms.ColumnHeader _column_26;
		private System.Windows.Forms.ColumnHeader _column_27;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel Panel1;
		private System.Windows.Forms.Button helpBtn;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.SaveFileDialog SaveDialog1;
	}
}