namespace Aran.Aim.FeatureInfo
{
	partial class FeatureHistoryForm
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
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.featureHistoryControlW1 = new Aran.Aim.FeatureInfo.FeatureHistoryControlW();
			this.SuspendLayout();
			// 
			// elementHost1
			// 
			this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementHost1.Location = new System.Drawing.Point(0, 0);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(703, 431);
			this.elementHost1.TabIndex = 0;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = this.featureHistoryControlW1;
			// 
			// FeatureHistoryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 431);
			this.Controls.Add(this.elementHost1);
			this.MaximizeBox = false;
			this.Name = "FeatureHistoryForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Feature History";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private FeatureHistoryControlW featureHistoryControlW1;
	}
}