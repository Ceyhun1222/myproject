namespace Aran.Aim.FeatureInfo
{
    partial class FeatureInfoTransparentForm
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
            this.ui_mainPanel = new System.Windows.Forms.Panel();
            this.ui_wpfElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.ui_featureContainerCont = new Aran.Aim.FeatureInfo.FeatureContainerControl();
            this.ui_mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_mainPanel
            // 
            this.ui_mainPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ui_mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_mainPanel.Controls.Add(this.ui_wpfElementHost);
            this.ui_mainPanel.Location = new System.Drawing.Point(441, 137);
            this.ui_mainPanel.Name = "ui_mainPanel";
            this.ui_mainPanel.Size = new System.Drawing.Size(401, 439);
            this.ui_mainPanel.TabIndex = 1;
            // 
            // ui_wpfElementHost
            // 
            this.ui_wpfElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_wpfElementHost.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_wpfElementHost.Location = new System.Drawing.Point(0, 0);
            this.ui_wpfElementHost.Name = "ui_wpfElementHost";
            this.ui_wpfElementHost.Size = new System.Drawing.Size(399, 437);
            this.ui_wpfElementHost.TabIndex = 3;
            this.ui_wpfElementHost.Child = this.ui_featureContainerCont;
            // 
            // FeatureInfoTransparentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 588);
            this.Controls.Add(this.ui_mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FeatureInfoTransparentForm";
            this.ShowInTaskbar = false;
            this.Text = "FeatureInfoForm";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.ui_mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ui_mainPanel;
        private System.Windows.Forms.Integration.ElementHost ui_wpfElementHost;
        private FeatureContainerControl ui_featureContainerCont;
    }
}