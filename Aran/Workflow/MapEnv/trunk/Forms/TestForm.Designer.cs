namespace MapEnv
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.ui_esriPageLayoutControl = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.ui_esriToolbarControl = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriPageLayoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriToolbarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_esriPageLayoutControl
            // 
            this.ui_esriPageLayoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_esriPageLayoutControl.Location = new System.Drawing.Point(3, 70);
            this.ui_esriPageLayoutControl.Name = "ui_esriPageLayoutControl";
            this.ui_esriPageLayoutControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriPageLayoutControl.OcxState")));
            this.ui_esriPageLayoutControl.Size = new System.Drawing.Size(707, 371);
            this.ui_esriPageLayoutControl.TabIndex = 0;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(570, -8);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // ui_esriToolbarControl
            // 
            this.ui_esriToolbarControl.Location = new System.Drawing.Point(3, 2);
            this.ui_esriToolbarControl.Name = "ui_esriToolbarControl";
            this.ui_esriToolbarControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriToolbarControl.OcxState")));
            this.ui_esriToolbarControl.Size = new System.Drawing.Size(393, 28);
            this.ui_esriToolbarControl.TabIndex = 6;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(3, 36);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(393, 28);
            this.axToolbarControl1.TabIndex = 7;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 501);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.ui_esriToolbarControl);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.ui_esriPageLayoutControl);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriPageLayoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriToolbarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxPageLayoutControl ui_esriPageLayoutControl;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl ui_esriToolbarControl;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
    }
}