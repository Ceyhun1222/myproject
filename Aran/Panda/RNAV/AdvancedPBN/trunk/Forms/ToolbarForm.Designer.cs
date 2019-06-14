namespace Aran.PANDA.RNAV.SGBAS
{
	partial class ToolbarForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolbarForm));
			this.RNAVToolStrip = new System.Windows.Forms.ToolStrip();
			this.tsBtnOASCat1 = new System.Windows.Forms.ToolStripButton();
			this.tsBtnBasicILS = new System.Windows.Forms.ToolStripButton();
			this.tsBtnSBASOAS = new System.Windows.Forms.ToolStripButton();
			this.tsBtnOFZ = new System.Windows.Forms.ToolStripButton();
			this.RNAVToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// RNAVToolStrip
			// 
			this.RNAVToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.RNAVToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnOASCat1,
            this.tsBtnBasicILS,
            this.tsBtnSBASOAS,
            this.tsBtnOFZ});
			this.RNAVToolStrip.Location = new System.Drawing.Point(0, 0);
			this.RNAVToolStrip.Name = "RNAVToolStrip";
			this.RNAVToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.RNAVToolStrip.Size = new System.Drawing.Size(401, 25);
			this.RNAVToolStrip.TabIndex = 1;
			this.RNAVToolStrip.Text = "RNAV Approach";
			// 
			// tsBtnOASCat1
			// 
			this.tsBtnOASCat1.Checked = true;
			this.tsBtnOASCat1.CheckOnClick = true;
			this.tsBtnOASCat1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsBtnOASCat1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtnOASCat1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnOASCat1.Name = "tsBtnOASCat1";
			this.tsBtnOASCat1.Size = new System.Drawing.Size(62, 22);
			this.tsBtnOASCat1.Text = "OAS cat 1";
			this.tsBtnOASCat1.Click += new System.EventHandler(this.tsBtnOASCat1_Click);
			// 
			// tsBtnBasicILS
			// 
			this.tsBtnBasicILS.Checked = true;
			this.tsBtnBasicILS.CheckOnClick = true;
			this.tsBtnBasicILS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsBtnBasicILS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtnBasicILS.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnBasicILS.Name = "tsBtnBasicILS";
			this.tsBtnBasicILS.Size = new System.Drawing.Size(56, 22);
			this.tsBtnBasicILS.Text = "Basic ILS";
			this.tsBtnBasicILS.Click += new System.EventHandler(this.tsBtnBasicILS_Click);
			// 
			// tsBtnSBASOAS
			// 
			this.tsBtnSBASOAS.Checked = true;
			this.tsBtnSBASOAS.CheckOnClick = true;
			this.tsBtnSBASOAS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsBtnSBASOAS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtnSBASOAS.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnSBASOAS.Name = "tsBtnSBASOAS";
			this.tsBtnSBASOAS.Size = new System.Drawing.Size(64, 22);
			this.tsBtnSBASOAS.Text = "SBAS OAS";
			this.tsBtnSBASOAS.Click += new System.EventHandler(this.tsBtnSBASOAS_Click);
			// 
			// tsBtnOFZ
			// 
			this.tsBtnOFZ.Checked = true;
			this.tsBtnOFZ.CheckOnClick = true;
			this.tsBtnOFZ.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsBtnOFZ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtnOFZ.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtnOFZ.Name = "tsBtnOFZ";
			this.tsBtnOFZ.Size = new System.Drawing.Size(33, 22);
			this.tsBtnOFZ.Text = "OFZ";
			this.tsBtnOFZ.Click += new System.EventHandler(this.tsBtnOFZ_Click);
			// 
			// ToolbarForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(401, 27);
			this.ControlBox = false;
			this.Controls.Add(this.RNAVToolStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ToolbarForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "RNAV Approach Toolbar";
			this.RNAVToolStrip.ResumeLayout(false);
			this.RNAVToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.ToolStrip RNAVToolStrip;

		private System.Windows.Forms.ToolStripButton tsBtnOASCat1;
		private System.Windows.Forms.ToolStripButton tsBtnBasicILS;
		internal System.Windows.Forms.ToolStripButton tsBtnSBASOAS;
		internal System.Windows.Forms.ToolStripButton tsBtnOFZ;
	}
}