namespace MapEnv
{
    partial class FeatureTypesStyleControl
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
            if (disposing && (components != null)) {
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
            this.ui_featureStyleControl = new MapEnv.FeatureStyleControl();
            this.ui_featTypesLB = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ui_featureStyleControl
            // 
            this.ui_featureStyleControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featureStyleControl.Location = new System.Drawing.Point(266, 3);
            this.ui_featureStyleControl.Name = "ui_featureStyleControl";
            this.ui_featureStyleControl.Size = new System.Drawing.Size(345, 404);
            this.ui_featureStyleControl.TabIndex = 1;
            // 
            // ui_featTypesLB
            // 
            this.ui_featTypesLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_featTypesLB.FormattingEnabled = true;
            this.ui_featTypesLB.Location = new System.Drawing.Point(3, 3);
            this.ui_featTypesLB.Name = "ui_featTypesLB";
            this.ui_featTypesLB.Size = new System.Drawing.Size(257, 394);
            this.ui_featTypesLB.TabIndex = 2;
            this.ui_featTypesLB.SelectedIndexChanged += new System.EventHandler(this.FeatureTypes_SelectedIndexChanged);
            // 
            // FeatureTypesStyleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_featureStyleControl);
            this.Controls.Add(this.ui_featTypesLB);
            this.Name = "FeatureTypesStyleControl";
            this.Size = new System.Drawing.Size(614, 413);
            this.ResumeLayout(false);

        }

        #endregion

        private FeatureStyleControl ui_featureStyleControl;
        private System.Windows.Forms.ListBox ui_featTypesLB;
    }
}
