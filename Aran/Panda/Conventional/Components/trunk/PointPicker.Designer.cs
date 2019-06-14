namespace ChoosePointNS
{
	partial class PointPicker
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PointPicker));
			this.gbFinalPoint = new System.Windows.Forms.GroupBox();
			this.rbDMS = new System.Windows.Forms.RadioButton();
			this.rbDD = new System.Windows.Forms.RadioButton();
			this.chkByClick = new System.Windows.Forms.CheckBox();
			this.lbLon = new System.Windows.Forms.Label();
			this.lbLat = new System.Windows.Forms.Label();
			this.DD_DMSLon = new ChoosePointNS.DD_DMS();
			this.DD_DMSLat = new ChoosePointNS.DD_DMS();
			this.gbFinalPoint.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbFinalPoint
			// 
			this.gbFinalPoint.Controls.Add(this.rbDMS);
			this.gbFinalPoint.Controls.Add(this.rbDD);
			this.gbFinalPoint.Controls.Add(this.DD_DMSLon);
			this.gbFinalPoint.Controls.Add(this.DD_DMSLat);
			this.gbFinalPoint.Controls.Add(this.chkByClick);
			this.gbFinalPoint.Controls.Add(this.lbLon);
			this.gbFinalPoint.Controls.Add(this.lbLat);
			this.gbFinalPoint.Location = new System.Drawing.Point(0, 0);
			this.gbFinalPoint.Name = "gbFinalPoint";
			this.gbFinalPoint.Size = new System.Drawing.Size(257, 107);
			this.gbFinalPoint.TabIndex = 16;
			this.gbFinalPoint.TabStop = false;
			// 
			// rbDMS
			// 
			this.rbDMS.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.rbDMS.AutoSize = true;
			this.rbDMS.Location = new System.Drawing.Point(150, 12);
			this.rbDMS.Name = "rbDMS";
			this.rbDMS.Size = new System.Drawing.Size(49, 17);
			this.rbDMS.TabIndex = 2;
			this.rbDMS.Text = "DMS";
			this.rbDMS.UseVisualStyleBackColor = true;
			// 
			// rbDD
			// 
			this.rbDD.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.rbDD.AutoSize = true;
			this.rbDD.Checked = true;
			this.rbDD.Location = new System.Drawing.Point(34, 12);
			this.rbDD.Name = "rbDD";
			this.rbDD.Size = new System.Drawing.Size(41, 17);
			this.rbDD.TabIndex = 1;
			this.rbDD.TabStop = true;
			this.rbDD.Text = "DD";
			this.rbDD.UseVisualStyleBackColor = true;
			this.rbDD.CheckedChanged += new System.EventHandler(this.rbDD_CheckedChanged);
			// 
			// chkByClick
			// 
			this.chkByClick.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.chkByClick.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkByClick.Image = ((System.Drawing.Image)(resources.GetObject("chkByClick.Image")));
			this.chkByClick.Location = new System.Drawing.Point(98, 12);
			this.chkByClick.Name = "chkByClick";
			this.chkByClick.Size = new System.Drawing.Size(24, 25);
			this.chkByClick.TabIndex = 5;
			this.chkByClick.UseVisualStyleBackColor = true;
			this.chkByClick.CheckedChanged += new System.EventHandler(this.chkkByClick_CheckedChanged);
			// 
			// lbLon
			// 
			this.lbLon.AutoSize = true;
			this.lbLon.Location = new System.Drawing.Point(6, 79);
			this.lbLon.Name = "lbLon";
			this.lbLon.Size = new System.Drawing.Size(28, 13);
			this.lbLon.TabIndex = 7;
			this.lbLon.Text = "Lon:";
			// 
			// lbLat
			// 
			this.lbLat.AutoSize = true;
			this.lbLat.Location = new System.Drawing.Point(6, 48);
			this.lbLat.Name = "lbLat";
			this.lbLat.Size = new System.Drawing.Size(25, 13);
			this.lbLat.TabIndex = 6;
			this.lbLat.Text = "Lat:";
			// 
			// DD_DMSLon
			// 
			this.DD_DMSLon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_DMSLon.DDAccuracy = 0;
			this.DD_DMSLon.DMSAccuracy = 0;
			this.DD_DMSLon.IsDD = true;
			this.DD_DMSLon.IsX = true;
			this.DD_DMSLon.Location = new System.Drawing.Point(39, 73);
			this.DD_DMSLon.Name = "DD_DMSLon";
			this.DD_DMSLon.ReadOnly = false;
			this.DD_DMSLon.Size = new System.Drawing.Size(211, 26);
			this.DD_DMSLon.TabIndex = 4;
			this.DD_DMSLon.Value = 0D;
			this.DD_DMSLon.ValueChanged += new System.EventHandler(this.DD_DMSLon_ValueChanged);
			// 
			// DD_DMSLat
			// 
			this.DD_DMSLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_DMSLat.DDAccuracy = 0;
			this.DD_DMSLat.DMSAccuracy = 0;
			this.DD_DMSLat.IsDD = true;
			this.DD_DMSLat.IsX = false;
			this.DD_DMSLat.Location = new System.Drawing.Point(39, 41);
			this.DD_DMSLat.Name = "DD_DMSLat";
			this.DD_DMSLat.ReadOnly = false;
			this.DD_DMSLat.Size = new System.Drawing.Size(211, 26);
			this.DD_DMSLat.TabIndex = 3;
			this.DD_DMSLat.Value = 0D;
			this.DD_DMSLat.ValueChanged += new System.EventHandler(this.DD_DMSLat_ValueChanged);
			// 
			// PointPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbFinalPoint);
			this.Name = "PointPicker";
			this.Size = new System.Drawing.Size(258, 111);
			this.gbFinalPoint.ResumeLayout(false);
			this.gbFinalPoint.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbFinalPoint;
		private System.Windows.Forms.RadioButton rbDMS;
		private System.Windows.Forms.RadioButton rbDD;
		private DD_DMS DD_DMSLon;
		private DD_DMS DD_DMSLat;
		private System.Windows.Forms.CheckBox chkByClick;
		private System.Windows.Forms.Label lbLon;
		private System.Windows.Forms.Label lbLat;
	}
}
