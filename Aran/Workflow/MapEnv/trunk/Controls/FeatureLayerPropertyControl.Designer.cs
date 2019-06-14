namespace MapEnv
{
    partial class FeatureLayerPropertyControl
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
            this.ui_mainTabControl = new System.Windows.Forms.TabControl();
            this.ui_styleTabPage = new System.Windows.Forms.TabPage();
            this.ui_styleControl = new MapEnv.FeatureStyleControl();
            this.ui_filterTabPage = new System.Windows.Forms.TabPage();
            this.ui_filterControl = new MapEnv.FilterControl();
            this.ui_saveButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_mainTabControl.SuspendLayout();
            this.ui_styleTabPage.SuspendLayout();
            this.ui_filterTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_mainTabControl
            // 
            this.ui_mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.ui_mainTabControl.Controls.Add(this.ui_styleTabPage);
            this.ui_mainTabControl.Controls.Add(this.ui_filterTabPage);
            this.ui_mainTabControl.Location = new System.Drawing.Point(3, 3);
            this.ui_mainTabControl.Name = "ui_mainTabControl";
            this.ui_mainTabControl.SelectedIndex = 0;
            this.ui_mainTabControl.Size = new System.Drawing.Size(407, 412);
            this.ui_mainTabControl.TabIndex = 0;
            // 
            // ui_styleTabPage
            // 
            this.ui_styleTabPage.Controls.Add(this.ui_styleControl);
            this.ui_styleTabPage.Location = new System.Drawing.Point(4, 25);
            this.ui_styleTabPage.Name = "ui_styleTabPage";
            this.ui_styleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_styleTabPage.Size = new System.Drawing.Size(399, 383);
            this.ui_styleTabPage.TabIndex = 0;
            this.ui_styleTabPage.Text = "Style";
            this.ui_styleTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_styleControl
            // 
            this.ui_styleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_styleControl.Location = new System.Drawing.Point(3, 3);
            this.ui_styleControl.Name = "ui_styleControl";
            this.ui_styleControl.Size = new System.Drawing.Size(393, 377);
            this.ui_styleControl.TabIndex = 0;
            // 
            // ui_filterTabPage
            // 
            this.ui_filterTabPage.Controls.Add(this.ui_filterControl);
            this.ui_filterTabPage.Location = new System.Drawing.Point(4, 25);
            this.ui_filterTabPage.Name = "ui_filterTabPage";
            this.ui_filterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_filterTabPage.Size = new System.Drawing.Size(399, 383);
            this.ui_filterTabPage.TabIndex = 1;
            this.ui_filterTabPage.Text = "Filter";
            this.ui_filterTabPage.UseVisualStyleBackColor = true;
            // 
            // ui_filterControl
            // 
            this.ui_filterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_filterControl.Location = new System.Drawing.Point(3, 3);
            this.ui_filterControl.Name = "ui_filterControl";
            this.ui_filterControl.Size = new System.Drawing.Size(393, 377);
            this.ui_filterControl.TabIndex = 0;
            // 
            // ui_saveButton
            // 
            this.ui_saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_saveButton.Location = new System.Drawing.Point(250, 421);
            this.ui_saveButton.Name = "ui_saveButton";
            this.ui_saveButton.Size = new System.Drawing.Size(75, 23);
            this.ui_saveButton.TabIndex = 0;
            this.ui_saveButton.Text = "Save";
            this.ui_saveButton.UseVisualStyleBackColor = true;
            this.ui_saveButton.Click += new System.EventHandler(this.ui_saveButton_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.Location = new System.Drawing.Point(331, 421);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 1;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.ui_cancelButton_Click);
            // 
            // FeatureLayerPropertyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_saveButton);
            this.Controls.Add(this.ui_mainTabControl);
            this.Name = "FeatureLayerPropertyControl";
            this.Size = new System.Drawing.Size(413, 456);
            this.ui_mainTabControl.ResumeLayout(false);
            this.ui_styleTabPage.ResumeLayout(false);
            this.ui_filterTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ui_mainTabControl;
        private System.Windows.Forms.TabPage ui_styleTabPage;
        private System.Windows.Forms.TabPage ui_filterTabPage;
        private System.Windows.Forms.Button ui_saveButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private FeatureStyleControl ui_styleControl;
        private FilterControl ui_filterControl;
    }
}
