namespace MapEnv
{
    partial class SubQuerySelectorForm
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
            System.Windows.Forms.Label label1;
            this.ui_cancelButton = new System.Windows.Forms.Button ();
            this.ui_okButton = new System.Windows.Forms.Button ();
            this.ui_featureTypeCB = new System.Windows.Forms.ComboBox ();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel ();
            this.ui_featureTypePanel = new System.Windows.Forms.Panel ();
            this.ui_propertyPanel = new System.Windows.Forms.Panel ();
            this.ui_compPropSelControl = new MapEnv.PropertySelectorControl ();
            this.ui_absFeaturePanel = new System.Windows.Forms.Panel ();
            this.ui_absFeaturesCLB = new System.Windows.Forms.CheckedListBox ();
            this.ui_label2 = new System.Windows.Forms.Label ();
            label1 = new System.Windows.Forms.Label ();
            this.flowLayoutPanel1.SuspendLayout ();
            this.ui_featureTypePanel.SuspendLayout ();
            this.ui_propertyPanel.SuspendLayout ();
            this.ui_absFeaturePanel.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point (5, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size (70, 13);
            label1.TabIndex = 0;
            label1.Text = "FeatureType:";
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point (249, 257);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size (75, 23);
            this.ui_cancelButton.TabIndex = 3;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point (168, 257);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size (75, 23);
            this.ui_okButton.TabIndex = 2;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler (this.Ok_Click);
            // 
            // ui_featureTypeCB
            // 
            this.ui_featureTypeCB.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featureTypeCB.FormattingEnabled = true;
            this.ui_featureTypeCB.Location = new System.Drawing.Point (3, 23);
            this.ui_featureTypeCB.Name = "ui_featureTypeCB";
            this.ui_featureTypeCB.Size = new System.Drawing.Size (313, 21);
            this.ui_featureTypeCB.Sorted = true;
            this.ui_featureTypeCB.TabIndex = 1;
            this.ui_featureTypeCB.TextChanged += new System.EventHandler (this.FeatureType_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add (this.ui_featureTypePanel);
            this.flowLayoutPanel1.Controls.Add (this.ui_propertyPanel);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point (7, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size (325, 242);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // ui_featureTypePanel
            // 
            this.ui_featureTypePanel.AutoSize = true;
            this.ui_featureTypePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_featureTypePanel.Controls.Add (this.ui_featureTypeCB);
            this.ui_featureTypePanel.Controls.Add (label1);
            this.ui_featureTypePanel.Location = new System.Drawing.Point (3, 3);
            this.ui_featureTypePanel.Name = "ui_featureTypePanel";
            this.ui_featureTypePanel.Size = new System.Drawing.Size (319, 47);
            this.ui_featureTypePanel.TabIndex = 5;
            // 
            // ui_propertyPanel
            // 
            this.ui_propertyPanel.AutoSize = true;
            this.ui_propertyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_propertyPanel.Controls.Add (this.ui_compPropSelControl);
            this.ui_propertyPanel.Controls.Add (this.ui_absFeaturePanel);
            this.ui_propertyPanel.Controls.Add (this.ui_label2);
            this.ui_propertyPanel.Location = new System.Drawing.Point (3, 56);
            this.ui_propertyPanel.Name = "ui_propertyPanel";
            this.ui_propertyPanel.Size = new System.Drawing.Size (316, 183);
            this.ui_propertyPanel.TabIndex = 6;
            // 
            // ui_compPropSelControl
            // 
            this.ui_compPropSelControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_compPropSelControl.ClassInfo = null;
            this.ui_compPropSelControl.Location = new System.Drawing.Point (3, 26);
            this.ui_compPropSelControl.Name = "ui_compPropSelControl";
            this.ui_compPropSelControl.Size = new System.Drawing.Size (310, 28);
            this.ui_compPropSelControl.TabIndex = 9;
            this.ui_compPropSelControl.Value = null;
            this.ui_compPropSelControl.AfterSelect += new MapEnv.PropertySelectedEventHandler (this.CompPropSel_AfterSelect);
            this.ui_compPropSelControl.ValueChanged += new System.EventHandler (this.CompPropSel_ValueChanged);
            // 
            // ui_absFeaturePanel
            // 
            this.ui_absFeaturePanel.Controls.Add (this.ui_absFeaturesCLB);
            this.ui_absFeaturePanel.Location = new System.Drawing.Point (5, 58);
            this.ui_absFeaturePanel.Name = "ui_absFeaturePanel";
            this.ui_absFeaturePanel.Size = new System.Drawing.Size (308, 122);
            this.ui_absFeaturePanel.TabIndex = 2;
            this.ui_absFeaturePanel.Visible = false;
            // 
            // ui_absFeaturesCLB
            // 
            this.ui_absFeaturesCLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_absFeaturesCLB.FormattingEnabled = true;
            this.ui_absFeaturesCLB.Location = new System.Drawing.Point (0, 0);
            this.ui_absFeaturesCLB.Name = "ui_absFeaturesCLB";
            this.ui_absFeaturesCLB.Size = new System.Drawing.Size (308, 122);
            this.ui_absFeaturesCLB.TabIndex = 1;
            this.ui_absFeaturesCLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler (this.AbsFeatures_ItemCheck);
            // 
            // ui_label2
            // 
            this.ui_label2.AutoSize = true;
            this.ui_label2.Location = new System.Drawing.Point (5, 10);
            this.ui_label2.Name = "ui_label2";
            this.ui_label2.Size = new System.Drawing.Size (80, 13);
            this.ui_label2.TabIndex = 8;
            this.ui_label2.Text = "Property Name:";
            // 
            // SubQuerySelectorForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size (336, 292);
            this.Controls.Add (this.flowLayoutPanel1);
            this.Controls.Add (this.ui_okButton);
            this.Controls.Add (this.ui_cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubQuerySelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Feature Type";
            this.Load += new System.EventHandler (this.SubQuerySelectorForm_Load);
            this.flowLayoutPanel1.ResumeLayout (false);
            this.flowLayoutPanel1.PerformLayout ();
            this.ui_featureTypePanel.ResumeLayout (false);
            this.ui_featureTypePanel.PerformLayout ();
            this.ui_propertyPanel.ResumeLayout (false);
            this.ui_propertyPanel.PerformLayout ();
            this.ui_absFeaturePanel.ResumeLayout (false);
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.ComboBox ui_featureTypeCB;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel ui_featureTypePanel;
        private System.Windows.Forms.Panel ui_propertyPanel;
        private PropertySelectorControl ui_compPropSelControl;
        private System.Windows.Forms.Label ui_label2;
        private System.Windows.Forms.Panel ui_absFeaturePanel;
        private System.Windows.Forms.CheckedListBox ui_absFeaturesCLB;
    }
}