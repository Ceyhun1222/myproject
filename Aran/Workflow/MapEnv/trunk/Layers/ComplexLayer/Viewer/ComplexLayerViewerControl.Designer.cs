﻿namespace MapEnv.ComplexLayer
{
    partial class ComplexLayerViewerControl
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
            this.ui_elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.ui_complexLayerViewerW = new MapEnv.ComplexLayer.ComplexLayerViewerW();
            this.SuspendLayout();
            // 
            // ui_elementHost
            // 
            this.ui_elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_elementHost.Location = new System.Drawing.Point(0, 0);
            this.ui_elementHost.Name = "ui_elementHost";
            this.ui_elementHost.Size = new System.Drawing.Size(343, 395);
            this.ui_elementHost.TabIndex = 0;
            this.ui_elementHost.Text = "elementHost1";
            this.ui_elementHost.Child = this.ui_complexLayerViewerW;
            // 
            // ComplexLayerViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_elementHost);
            this.Name = "ComplexLayerViewerControl";
            this.Size = new System.Drawing.Size(343, 395);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost ui_elementHost;
        private ComplexLayerViewerW ui_complexLayerViewerW;
    }
}
