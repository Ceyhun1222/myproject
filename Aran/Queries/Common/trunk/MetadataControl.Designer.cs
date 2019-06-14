namespace Aran.Queries.Common
{
    partial class MetadataControl
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

        #region Component Designer generated code

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
			this.ui_featureTSMDCont.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_featureTSMDCont.Location = new System.Drawing.Point(0, 0);
			this.ui_featureTSMDCont.Name = "ui_featureTSMDCont";
			this.ui_featureTSMDCont.Size = new System.Drawing.Size(456, 373);
			this.ui_featureTSMDCont.TabIndex = 0;
			// 
			// MetadataControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ui_featureTSMDCont);
			this.Name = "MetadataControl";
			this.Size = new System.Drawing.Size(456, 373);
			this.ResumeLayout(false);

        }

        #endregion

		private Aim.FmdEditor.FeatureTSMDControl ui_featureTSMDCont;

	}
}
