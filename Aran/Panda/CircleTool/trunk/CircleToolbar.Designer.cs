namespace Aran.PANDA.CircleTool
{
	partial class CircleToolbar
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
			this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsBtn15nm = new System.Windows.Forms.ToolStripButton();
			this.tsBtn30nm = new System.Windows.Forms.ToolStripButton();
			this.ToolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ToolStrip1
			// 
			this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtn15nm,
            this.tsBtn30nm});
			this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
			this.ToolStrip1.Name = "ToolStrip1";
			this.ToolStrip1.Size = new System.Drawing.Size(305, 25);
			this.ToolStrip1.TabIndex = 1;
			// 
			// tsBtn15nm
			// 
			this.tsBtn15nm.CheckOnClick = true;
			this.tsBtn15nm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtn15nm.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtn15nm.Name = "tsBtn15nm";
			this.tsBtn15nm.Size = new System.Drawing.Size(49, 22);
			this.tsBtn15nm.Text = "15 N.M";
			this.tsBtn15nm.Click += new System.EventHandler(this.tsBtn15nm_Click);
			// 
			// tsBtn30nm
			// 
			this.tsBtn30nm.CheckOnClick = true;
			this.tsBtn30nm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsBtn30nm.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsBtn30nm.Name = "tsBtn30nm";
			this.tsBtn30nm.Size = new System.Drawing.Size(49, 22);
			this.tsBtn30nm.Text = "30 N.M";
			this.tsBtn30nm.Click += new System.EventHandler(this.tsBtn30nm_Click);
			// 
			// CircleToolbar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(305, 29);
			this.ControlBox = false;
			this.Controls.Add(this.ToolStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CircleToolbar";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "CircleToolbar";
			this.ToolStrip1.ResumeLayout(false);
			this.ToolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.ToolStrip ToolStrip1;
		private System.Windows.Forms.ToolStripButton tsBtn30nm;
		private System.Windows.Forms.ToolStripButton tsBtn15nm;
	}
}