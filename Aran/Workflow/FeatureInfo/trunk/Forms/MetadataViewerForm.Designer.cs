namespace Aran.Aim.FeatureInfo
{
	partial class MetadataViewerForm
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
			this.ui_featureTSMDCont = new Aran.Aim.FmdEditor.FeatureTSMDControl();
			this.SuspendLayout();
			// 
			// ui_featureTSMDCont
			// 
			this.ui_featureTSMDCont.Location = new System.Drawing.Point(2, 3);
			this.ui_featureTSMDCont.Name = "ui_featureTSMDCont";
			this.ui_featureTSMDCont.ReadOnly = true;
			this.ui_featureTSMDCont.Size = new System.Drawing.Size(454, 370);
			this.ui_featureTSMDCont.TabIndex = 0;
			// 
			// MetadataViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(458, 376);
			this.Controls.Add(this.ui_featureTSMDCont);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MetadataViewerForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Metadata Viewer";
			this.ResumeLayout(false);

		}

		#endregion

		private FmdEditor.FeatureTSMDControl ui_featureTSMDCont;
	}
}