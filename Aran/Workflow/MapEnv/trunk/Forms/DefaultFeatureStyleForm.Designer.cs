namespace MapEnv
{
    partial class DefaultFeatureStyleForm
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
            MapEnv.Controls.LineControl lineControl1;
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ui_addFeatTypeTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_restoreSettingsTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_removeTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_featTypesStyleControl = new MapEnv.FeatureTypesStyleControl();
            lineControl1 = new MapEnv.Controls.LineControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lineControl1
            // 
            lineControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            lineControl1.Location = new System.Drawing.Point(-21, 508);
            lineControl1.Name = "lineControl1";
            lineControl1.Size = new System.Drawing.Size(741, 8);
            lineControl1.TabIndex = 0;
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(611, 521);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 1;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(530, 521);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 2;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.Save_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_addFeatTypeTSB,
            this.ui_restoreSettingsTSB,
            this.ui_removeTSB});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(698, 27);
            this.toolStrip1.TabIndex = 1;
            // 
            // ui_addFeatTypeTSB
            // 
            this.ui_addFeatTypeTSB.Image = global::MapEnv.Properties.Resources.add_16;
            this.ui_addFeatTypeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_addFeatTypeTSB.Name = "ui_addFeatTypeTSB";
            this.ui_addFeatTypeTSB.Size = new System.Drawing.Size(119, 24);
            this.ui_addFeatTypeTSB.Text = "Add Feature Type";
            this.ui_addFeatTypeTSB.Click += new System.EventHandler(this.AddFeatureType_Click);
            // 
            // ui_restoreSettingsTSB
            // 
            this.ui_restoreSettingsTSB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ui_restoreSettingsTSB.Image = global::MapEnv.Properties.Resources.restore_16;
            this.ui_restoreSettingsTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_restoreSettingsTSB.Name = "ui_restoreSettingsTSB";
            this.ui_restoreSettingsTSB.Size = new System.Drawing.Size(152, 24);
            this.ui_restoreSettingsTSB.Text = "Restore Default Settings";
            this.ui_restoreSettingsTSB.Click += new System.EventHandler(this.RestoreSettings_Click);
            // 
            // ui_removeTSB
            // 
            this.ui_removeTSB.Enabled = false;
            this.ui_removeTSB.Image = global::MapEnv.Properties.Resources.remove_16;
            this.ui_removeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_removeTSB.Name = "ui_removeTSB";
            this.ui_removeTSB.Size = new System.Drawing.Size(70, 24);
            this.ui_removeTSB.Text = "Remove";
            this.ui_removeTSB.Click += new System.EventHandler(this.RemoveFeatureType_Click);
            // 
            // ui_featTypesStyleControl
            // 
            this.ui_featTypesStyleControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featTypesStyleControl.Location = new System.Drawing.Point(0, 28);
            this.ui_featTypesStyleControl.Name = "ui_featTypesStyleControl";
            this.ui_featTypesStyleControl.SelectedFeatureType = null;
            this.ui_featTypesStyleControl.Size = new System.Drawing.Size(698, 491);
            this.ui_featTypesStyleControl.TabIndex = 3;
            this.ui_featTypesStyleControl.SelectedFeatureTypeChanged += new System.EventHandler(this.FeatTypesStyleControl_SelectedFeatureTypeChanged);
            // 
            // DefaultFeatureStyleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 553);
            this.Controls.Add(lineControl1);
            this.Controls.Add(this.ui_featTypesStyleControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(538, 470);
            this.Name = "DefaultFeatureStyleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Default Feature Styles";
            this.Load += new System.EventHandler(this.DefaultFeatureStyleForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ui_addFeatTypeTSB;
        private System.Windows.Forms.ToolStripButton ui_removeTSB;
        private FeatureTypesStyleControl ui_featTypesStyleControl;
        private System.Windows.Forms.ToolStripButton ui_restoreSettingsTSB;
    }
}