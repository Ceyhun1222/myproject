namespace Aran.Aim.FeatureInfo
{
    partial class FeatureInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureInfoForm));
            this.ui_elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.ui_featureContainerCont = new Aran.Aim.FeatureInfo.FeatureContainerControl();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ui_elementHost
            // 
            this.ui_elementHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_elementHost.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.ui_elementHost.Location = new System.Drawing.Point(0, 0);
            this.ui_elementHost.Name = "ui_elementHost";
            this.ui_elementHost.Size = new System.Drawing.Size(423, 376);
            this.ui_elementHost.TabIndex = 0;
            this.ui_elementHost.Child = this.ui_featureContainerCont;
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_closeButton.Location = new System.Drawing.Point(344, 379);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 27);
            this.ui_closeButton.TabIndex = 1;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_okButton.Location = new System.Drawing.Point(263, 379);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 27);
            this.ui_okButton.TabIndex = 2;
            this.ui_okButton.Text = "<ok>";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Visible = false;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // FeatureInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 409);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(this.ui_elementHost);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FeatureInfoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feature Info";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost ui_elementHost;
        private FeatureContainerControl ui_featureContainerCont;
        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.Button ui_okButton;
    }
}