namespace MapEnv
{
    partial class NewProPluginPage
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
            System.Windows.Forms.Label pluginsLabel;
            System.Windows.Forms.Label featLayerLabel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Panel panel1;
            this.ui_pluginsCLB = new System.Windows.Forms.CheckedListBox();
            this.ui_featLayersLB = new System.Windows.Forms.ListBox();
            this.ui_selSpatialRefButton = new System.Windows.Forms.Button();
            this.ui_spatialRefTB = new System.Windows.Forms.TextBox();
            this.ui_spatialRefLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ui_effectiveDateTimePicker = new Aran.Controls.Airac.AiracCycleControl();
            this.ui_checkAllChB = new System.Windows.Forms.CheckBox();
            pluginsLabel = new System.Windows.Forms.Label();
            featLayerLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pluginsLabel
            // 
            pluginsLabel.AutoSize = true;
            pluginsLabel.Location = new System.Drawing.Point(11, 9);
            pluginsLabel.Name = "pluginsLabel";
            pluginsLabel.Size = new System.Drawing.Size(44, 13);
            pluginsLabel.TabIndex = 0;
            pluginsLabel.Text = "Plugins:";
            // 
            // featLayerLabel
            // 
            featLayerLabel.AutoSize = true;
            featLayerLabel.Location = new System.Drawing.Point(270, 9);
            featLayerLabel.Name = "featLayerLabel";
            featLayerLabel.Size = new System.Drawing.Size(80, 13);
            featLayerLabel.TabIndex = 3;
            featLayerLabel.Text = "Feature Layers:";
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 197);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 13);
            label2.TabIndex = 16;
            label2.Text = "Effective Date:";
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            panel1.BackColor = System.Drawing.SystemColors.Window;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(this.ui_pluginsCLB);
            panel1.Location = new System.Drawing.Point(10, 26);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(256, 161);
            panel1.TabIndex = 18;
            // 
            // ui_pluginsCLB
            // 
            this.ui_pluginsCLB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_pluginsCLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_pluginsCLB.FormattingEnabled = true;
            this.ui_pluginsCLB.Location = new System.Drawing.Point(0, 0);
            this.ui_pluginsCLB.Name = "ui_pluginsCLB";
            this.ui_pluginsCLB.Size = new System.Drawing.Size(254, 159);
            this.ui_pluginsCLB.TabIndex = 1;
            this.ui_pluginsCLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.PluginsCLB_ItemCheck);
            this.ui_pluginsCLB.SelectedIndexChanged += new System.EventHandler(this.Plugins_SelectedIndexChanged);
            // 
            // ui_featLayersLB
            // 
            this.ui_featLayersLB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_featLayersLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featLayersLB.FormattingEnabled = true;
            this.ui_featLayersLB.Location = new System.Drawing.Point(0, 0);
            this.ui_featLayersLB.Name = "ui_featLayersLB";
            this.ui_featLayersLB.Size = new System.Drawing.Size(188, 159);
            this.ui_featLayersLB.TabIndex = 2;
            // 
            // ui_selSpatialRefButton
            // 
            this.ui_selSpatialRefButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_selSpatialRefButton.Location = new System.Drawing.Point(432, 222);
            this.ui_selSpatialRefButton.Name = "ui_selSpatialRefButton";
            this.ui_selSpatialRefButton.Size = new System.Drawing.Size(29, 23);
            this.ui_selSpatialRefButton.TabIndex = 6;
            this.ui_selSpatialRefButton.Text = "...";
            this.ui_selSpatialRefButton.UseVisualStyleBackColor = true;
            this.ui_selSpatialRefButton.Click += new System.EventHandler(this.SelectSpatialRef_Click);
            // 
            // ui_spatialRefTB
            // 
            this.ui_spatialRefTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_spatialRefTB.Location = new System.Drawing.Point(110, 224);
            this.ui_spatialRefTB.Name = "ui_spatialRefTB";
            this.ui_spatialRefTB.ReadOnly = true;
            this.ui_spatialRefTB.Size = new System.Drawing.Size(317, 20);
            this.ui_spatialRefTB.TabIndex = 5;
            // 
            // ui_spatialRefLabel
            // 
            this.ui_spatialRefLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_spatialRefLabel.AutoSize = true;
            this.ui_spatialRefLabel.Location = new System.Drawing.Point(11, 227);
            this.ui_spatialRefLabel.Name = "ui_spatialRefLabel";
            this.ui_spatialRefLabel.Size = new System.Drawing.Size(95, 13);
            this.ui_spatialRefLabel.TabIndex = 4;
            this.ui_spatialRefLabel.Text = "Spatial Reference:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ui_featLayersLB);
            this.panel2.Location = new System.Drawing.Point(272, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 161);
            this.panel2.TabIndex = 19;
            // 
            // ui_effectiveDateTimePicker
            // 
            this.ui_effectiveDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_effectiveDateTimePicker.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_effectiveDateTimePicker.Location = new System.Drawing.Point(110, 192);
            this.ui_effectiveDateTimePicker.Name = "ui_effectiveDateTimePicker";
            this.ui_effectiveDateTimePicker.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_effectiveDateTimePicker.Size = new System.Drawing.Size(215, 25);
            this.ui_effectiveDateTimePicker.TabIndex = 20;
            this.ui_effectiveDateTimePicker.Value = new System.DateTime(2014, 9, 10, 0, 0, 0, 0);
            // 
            // ui_checkAllChB
            // 
            this.ui_checkAllChB.AutoSize = true;
            this.ui_checkAllChB.Location = new System.Drawing.Point(193, 9);
            this.ui_checkAllChB.Name = "ui_checkAllChB";
            this.ui_checkAllChB.Size = new System.Drawing.Size(71, 17);
            this.ui_checkAllChB.TabIndex = 21;
            this.ui_checkAllChB.Text = "Check All";
            this.ui_checkAllChB.UseVisualStyleBackColor = true;
            this.ui_checkAllChB.CheckedChanged += new System.EventHandler(this.CheckAll_CheckedChanged);
            // 
            // NewProPluginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_effectiveDateTimePicker);
            this.Controls.Add(this.panel2);
            this.Controls.Add(panel1);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_selSpatialRefButton);
            this.Controls.Add(this.ui_spatialRefTB);
            this.Controls.Add(this.ui_spatialRefLabel);
            this.Controls.Add(featLayerLabel);
            this.Controls.Add(pluginsLabel);
            this.Controls.Add(this.ui_checkAllChB);
            this.Name = "NewProPluginPage";
            this.Size = new System.Drawing.Size(472, 256);
            panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox ui_pluginsCLB;
        private System.Windows.Forms.ListBox ui_featLayersLB;
        private System.Windows.Forms.Button ui_selSpatialRefButton;
        private System.Windows.Forms.TextBox ui_spatialRefTB;
        private System.Windows.Forms.Label ui_spatialRefLabel;
        private System.Windows.Forms.Panel panel2;
        private Aran.Controls.Airac.AiracCycleControl ui_effectiveDateTimePicker;
        private System.Windows.Forms.CheckBox ui_checkAllChB;
    }
}
