namespace MapEnv.Controls
{
    partial class FeatureTypeSelector
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
            this.ui_listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ui_layerTypeCB = new System.Windows.Forms.ComboBox();
            this.ui_layerTypeLabel = new System.Windows.Forms.Label();
            this.ui_topPanel = new System.Windows.Forms.Panel();
            this.ui_quickSearchTB = new System.Windows.Forms.TextBox();
            this.ui_topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_listView
            // 
            this.ui_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ui_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_listView.HideSelection = false;
            this.ui_listView.Location = new System.Drawing.Point(0, 28);
            this.ui_listView.MultiSelect = false;
            this.ui_listView.Name = "ui_listView";
            this.ui_listView.ShowGroups = false;
            this.ui_listView.Size = new System.Drawing.Size(600, 418);
            this.ui_listView.TabIndex = 0;
            this.ui_listView.UseCompatibleStateImageBehavior = false;
            this.ui_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Feature Type";
            this.columnHeader1.Width = 200;
            // 
            // ui_layerTypeCB
            // 
            this.ui_layerTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_layerTypeCB.FormattingEnabled = true;
            this.ui_layerTypeCB.Location = new System.Drawing.Point(72, 3);
            this.ui_layerTypeCB.Name = "ui_layerTypeCB";
            this.ui_layerTypeCB.Size = new System.Drawing.Size(156, 21);
            this.ui_layerTypeCB.TabIndex = 11;
            this.ui_layerTypeCB.SelectedIndexChanged += new System.EventHandler(this.LayerType_SelectedIndexChanged);
            // 
            // ui_layerTypeLabel
            // 
            this.ui_layerTypeLabel.AutoSize = true;
            this.ui_layerTypeLabel.Location = new System.Drawing.Point(3, 6);
            this.ui_layerTypeLabel.Name = "ui_layerTypeLabel";
            this.ui_layerTypeLabel.Size = new System.Drawing.Size(63, 13);
            this.ui_layerTypeLabel.TabIndex = 10;
            this.ui_layerTypeLabel.Text = "Layer Type:";
            // 
            // ui_topPanel
            // 
            this.ui_topPanel.Controls.Add(this.ui_quickSearchTB);
            this.ui_topPanel.Controls.Add(this.ui_layerTypeCB);
            this.ui_topPanel.Controls.Add(this.ui_layerTypeLabel);
            this.ui_topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_topPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_topPanel.Name = "ui_topPanel";
            this.ui_topPanel.Size = new System.Drawing.Size(600, 28);
            this.ui_topPanel.TabIndex = 12;
            // 
            // ui_quickSearchTB
            // 
            this.ui_quickSearchTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_quickSearchTB.Location = new System.Drawing.Point(457, 4);
            this.ui_quickSearchTB.Name = "ui_quickSearchTB";
            this.ui_quickSearchTB.Size = new System.Drawing.Size(140, 20);
            this.ui_quickSearchTB.TabIndex = 12;
            this.ui_quickSearchTB.TextChanged += new System.EventHandler(this.QuickSearch_TextChanged);
            // 
            // FeatureTypeSelector_OLD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_listView);
            this.Controls.Add(this.ui_topPanel);
            this.Name = "FeatureTypeSelector_OLD";
            this.Size = new System.Drawing.Size(600, 446);
            this.ui_topPanel.ResumeLayout(false);
            this.ui_topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ui_listView;
        private System.Windows.Forms.ComboBox ui_layerTypeCB;
        private System.Windows.Forms.Label ui_layerTypeLabel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel ui_topPanel;
        private System.Windows.Forms.TextBox ui_quickSearchTB;

    }
}
