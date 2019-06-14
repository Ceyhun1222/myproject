namespace MapEnv
{
    partial class FeatureStyleControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label ui_symbCatPropNameLabel;
            this.ui_symbCatGroupBox = new System.Windows.Forms.GroupBox();
            this.ui_symbCatPropNameCB = new System.Windows.Forms.ComboBox();
            this.ui_removeSymCatButton = new System.Windows.Forms.Button();
            this.ui_addSymCatButton = new System.Windows.Forms.Button();
            this.ui_symbCatValsListView = new System.Windows.Forms.ListView();
            this.ui_featureTypeLabel = new System.Windows.Forms.Label();
            this.ui_featureTypeTB = new System.Windows.Forms.TextBox();
            this.ui_textSymbolLabel = new System.Windows.Forms.Label();
            this.ui_textPropLabel = new System.Windows.Forms.Label();
            this.ui_geoPropLabel = new System.Windows.Forms.Label();
            this.ui_geoPropComboBox = new System.Windows.Forms.ComboBox();
            this.ui_textSymbolButton = new System.Windows.Forms.Button();
            this.ui_addShapeInfoButton = new System.Windows.Forms.Button();
            this.ui_removeShapeInfoButton = new System.Windows.Forms.Button();
            this.ui_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ui_shapeInfoLV = new System.Windows.Forms.ListView();
            this.ui_textPropCB = new System.Windows.Forms.ComboBox();
            ui_symbCatPropNameLabel = new System.Windows.Forms.Label();
            this.ui_symbCatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_symbCatPropNameLabel
            // 
            ui_symbCatPropNameLabel.AutoSize = true;
            ui_symbCatPropNameLabel.Location = new System.Drawing.Point(6, 16);
            ui_symbCatPropNameLabel.Name = "ui_symbCatPropNameLabel";
            ui_symbCatPropNameLabel.Size = new System.Drawing.Size(80, 13);
            ui_symbCatPropNameLabel.TabIndex = 0;
            ui_symbCatPropNameLabel.Text = "Property Name:";
            // 
            // ui_symbCatGroupBox
            // 
            this.ui_symbCatGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_symbCatGroupBox.Controls.Add(this.ui_symbCatPropNameCB);
            this.ui_symbCatGroupBox.Controls.Add(this.ui_removeSymCatButton);
            this.ui_symbCatGroupBox.Controls.Add(this.ui_addSymCatButton);
            this.ui_symbCatGroupBox.Controls.Add(this.ui_symbCatValsListView);
            this.ui_symbCatGroupBox.Controls.Add(ui_symbCatPropNameLabel);
            this.ui_symbCatGroupBox.Enabled = false;
            this.ui_symbCatGroupBox.Location = new System.Drawing.Point(3, 175);
            this.ui_symbCatGroupBox.Name = "ui_symbCatGroupBox";
            this.ui_symbCatGroupBox.Size = new System.Drawing.Size(505, 190);
            this.ui_symbCatGroupBox.TabIndex = 5;
            this.ui_symbCatGroupBox.TabStop = false;
            this.ui_symbCatGroupBox.Text = "Symbol Categories:";
            // 
            // ui_symbCatPropNameCB
            // 
            this.ui_symbCatPropNameCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_symbCatPropNameCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_symbCatPropNameCB.DropDownWidth = 440;
            this.ui_symbCatPropNameCB.FormattingEnabled = true;
            this.ui_symbCatPropNameCB.ItemHeight = 13;
            this.ui_symbCatPropNameCB.Location = new System.Drawing.Point(9, 30);
            this.ui_symbCatPropNameCB.Name = "ui_symbCatPropNameCB";
            this.ui_symbCatPropNameCB.Size = new System.Drawing.Size(433, 21);
            this.ui_symbCatPropNameCB.TabIndex = 23;
            this.ui_symbCatPropNameCB.SelectedIndexChanged += new System.EventHandler(this.SymbCatPropNameCB_SelectedIndexChanged);
            // 
            // ui_removeSymCatButton
            // 
            this.ui_removeSymCatButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_removeSymCatButton.Image = global::MapEnv.Properties.Resources.remove_16;
            this.ui_removeSymCatButton.Location = new System.Drawing.Point(474, 27);
            this.ui_removeSymCatButton.Name = "ui_removeSymCatButton";
            this.ui_removeSymCatButton.Size = new System.Drawing.Size(26, 26);
            this.ui_removeSymCatButton.TabIndex = 6;
            this.ui_toolTip.SetToolTip(this.ui_removeSymCatButton, "Remove Selected Category Symbol");
            this.ui_removeSymCatButton.UseVisualStyleBackColor = true;
            this.ui_removeSymCatButton.Click += new System.EventHandler(this.RemoveSymCat_Click);
            // 
            // ui_addSymCatButton
            // 
            this.ui_addSymCatButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_addSymCatButton.Enabled = false;
            this.ui_addSymCatButton.Image = global::MapEnv.Properties.Resources.add_16;
            this.ui_addSymCatButton.Location = new System.Drawing.Point(447, 27);
            this.ui_addSymCatButton.Name = "ui_addSymCatButton";
            this.ui_addSymCatButton.Size = new System.Drawing.Size(26, 26);
            this.ui_addSymCatButton.TabIndex = 5;
            this.ui_toolTip.SetToolTip(this.ui_addSymCatButton, "Add Category Symbol");
            this.ui_addSymCatButton.UseVisualStyleBackColor = true;
            this.ui_addSymCatButton.Click += new System.EventHandler(this.AddSymCat_Click);
            // 
            // ui_symbCatValsListView
            // 
            this.ui_symbCatValsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_symbCatValsListView.LabelEdit = true;
            this.ui_symbCatValsListView.Location = new System.Drawing.Point(4, 54);
            this.ui_symbCatValsListView.MultiSelect = false;
            this.ui_symbCatValsListView.Name = "ui_symbCatValsListView";
            this.ui_symbCatValsListView.ShowItemToolTips = true;
            this.ui_symbCatValsListView.Size = new System.Drawing.Size(494, 130);
            this.ui_symbCatValsListView.TabIndex = 2;
            this.ui_symbCatValsListView.UseCompatibleStateImageBehavior = false;
            this.ui_symbCatValsListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.SymbCatValsListView_AfterLabelEdit);
            this.ui_symbCatValsListView.SelectedIndexChanged += new System.EventHandler(this.SymbCatValsListView_SelectedIndexChanged);
            this.ui_symbCatValsListView.DoubleClick += new System.EventHandler(this.SymbCatValsListView_DoubleClick);
            // 
            // ui_featureTypeLabel
            // 
            this.ui_featureTypeLabel.AutoSize = true;
            this.ui_featureTypeLabel.Location = new System.Drawing.Point(7, 6);
            this.ui_featureTypeLabel.Name = "ui_featureTypeLabel";
            this.ui_featureTypeLabel.Size = new System.Drawing.Size(73, 13);
            this.ui_featureTypeLabel.TabIndex = 0;
            this.ui_featureTypeLabel.Text = "Feature Type:";
            // 
            // ui_featureTypeTB
            // 
            this.ui_featureTypeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featureTypeTB.Location = new System.Drawing.Point(86, 3);
            this.ui_featureTypeTB.Name = "ui_featureTypeTB";
            this.ui_featureTypeTB.ReadOnly = true;
            this.ui_featureTypeTB.Size = new System.Drawing.Size(416, 20);
            this.ui_featureTypeTB.TabIndex = 1;
            // 
            // ui_textSymbolLabel
            // 
            this.ui_textSymbolLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textSymbolLabel.AutoSize = true;
            this.ui_textSymbolLabel.Location = new System.Drawing.Point(393, 381);
            this.ui_textSymbolLabel.Name = "ui_textSymbolLabel";
            this.ui_textSymbolLabel.Size = new System.Drawing.Size(44, 13);
            this.ui_textSymbolLabel.TabIndex = 5;
            this.ui_textSymbolLabel.Text = "Symbol:";
            // 
            // ui_textPropLabel
            // 
            this.ui_textPropLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_textPropLabel.AutoSize = true;
            this.ui_textPropLabel.Location = new System.Drawing.Point(4, 381);
            this.ui_textPropLabel.Name = "ui_textPropLabel";
            this.ui_textPropLabel.Size = new System.Drawing.Size(73, 13);
            this.ui_textPropLabel.TabIndex = 2;
            this.ui_textPropLabel.Text = "Text Property:";
            // 
            // ui_geoPropLabel
            // 
            this.ui_geoPropLabel.AutoSize = true;
            this.ui_geoPropLabel.Location = new System.Drawing.Point(4, 37);
            this.ui_geoPropLabel.Name = "ui_geoPropLabel";
            this.ui_geoPropLabel.Size = new System.Drawing.Size(72, 13);
            this.ui_geoPropLabel.TabIndex = 0;
            this.ui_geoPropLabel.Text = "Geo Property:";
            // 
            // ui_geoPropComboBox
            // 
            this.ui_geoPropComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_geoPropComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_geoPropComboBox.DropDownWidth = 440;
            this.ui_geoPropComboBox.FormattingEnabled = true;
            this.ui_geoPropComboBox.ItemHeight = 13;
            this.ui_geoPropComboBox.Location = new System.Drawing.Point(12, 53);
            this.ui_geoPropComboBox.Name = "ui_geoPropComboBox";
            this.ui_geoPropComboBox.Size = new System.Drawing.Size(433, 21);
            this.ui_geoPropComboBox.TabIndex = 20;
            this.ui_geoPropComboBox.SelectedIndexChanged += new System.EventHandler(this.GeoPropComboBox_SelectedIndexChanged);
            // 
            // ui_textSymbolButton
            // 
            this.ui_textSymbolButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textSymbolButton.Location = new System.Drawing.Point(441, 380);
            this.ui_textSymbolButton.Name = "ui_textSymbolButton";
            this.ui_textSymbolButton.Size = new System.Drawing.Size(61, 53);
            this.ui_textSymbolButton.TabIndex = 19;
            this.ui_textSymbolButton.UseVisualStyleBackColor = true;
            this.ui_textSymbolButton.Click += new System.EventHandler(this.TextSymbol_Click);
            // 
            // ui_addShapeInfoButton
            // 
            this.ui_addShapeInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_addShapeInfoButton.Enabled = false;
            this.ui_addShapeInfoButton.Image = global::MapEnv.Properties.Resources.add_16;
            this.ui_addShapeInfoButton.Location = new System.Drawing.Point(450, 51);
            this.ui_addShapeInfoButton.Name = "ui_addShapeInfoButton";
            this.ui_addShapeInfoButton.Size = new System.Drawing.Size(26, 26);
            this.ui_addShapeInfoButton.TabIndex = 3;
            this.ui_toolTip.SetToolTip(this.ui_addShapeInfoButton, "Add Symbol");
            this.ui_addShapeInfoButton.UseVisualStyleBackColor = true;
            this.ui_addShapeInfoButton.Click += new System.EventHandler(this.AddShapeInfo_Click);
            // 
            // ui_removeShapeInfoButton
            // 
            this.ui_removeShapeInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_removeShapeInfoButton.Enabled = false;
            this.ui_removeShapeInfoButton.Image = global::MapEnv.Properties.Resources.remove_16;
            this.ui_removeShapeInfoButton.Location = new System.Drawing.Point(477, 51);
            this.ui_removeShapeInfoButton.Name = "ui_removeShapeInfoButton";
            this.ui_removeShapeInfoButton.Size = new System.Drawing.Size(26, 26);
            this.ui_removeShapeInfoButton.TabIndex = 4;
            this.ui_toolTip.SetToolTip(this.ui_removeShapeInfoButton, "Remove Selected Symbol");
            this.ui_removeShapeInfoButton.UseVisualStyleBackColor = true;
            this.ui_removeShapeInfoButton.Click += new System.EventHandler(this.RemoveShapeInfo_Click);
            // 
            // ui_shapeInfoLV
            // 
            this.ui_shapeInfoLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_shapeInfoLV.HideSelection = false;
            this.ui_shapeInfoLV.Location = new System.Drawing.Point(7, 82);
            this.ui_shapeInfoLV.MultiSelect = false;
            this.ui_shapeInfoLV.Name = "ui_shapeInfoLV";
            this.ui_shapeInfoLV.ShowItemToolTips = true;
            this.ui_shapeInfoLV.Size = new System.Drawing.Size(496, 87);
            this.ui_shapeInfoLV.TabIndex = 21;
            this.ui_shapeInfoLV.UseCompatibleStateImageBehavior = false;
            this.ui_shapeInfoLV.SelectedIndexChanged += new System.EventHandler(this.ShapeInfoLV_SelectedIndexChanged);
            this.ui_shapeInfoLV.DoubleClick += new System.EventHandler(this.ShapeInfoLV_DoubleClick);
            // 
            // ui_textPropCB
            // 
            this.ui_textPropCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_textPropCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_textPropCB.DropDownWidth = 440;
            this.ui_textPropCB.FormattingEnabled = true;
            this.ui_textPropCB.ItemHeight = 13;
            this.ui_textPropCB.Location = new System.Drawing.Point(7, 397);
            this.ui_textPropCB.Name = "ui_textPropCB";
            this.ui_textPropCB.Size = new System.Drawing.Size(427, 21);
            this.ui_textPropCB.TabIndex = 22;
            this.ui_textPropCB.SelectedIndexChanged += new System.EventHandler(this.TextPropCB_SelectedIndexChanged);
            // 
            // FeatureStyleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_textPropCB);
            this.Controls.Add(this.ui_shapeInfoLV);
            this.Controls.Add(this.ui_geoPropComboBox);
            this.Controls.Add(this.ui_symbCatGroupBox);
            this.Controls.Add(this.ui_featureTypeLabel);
            this.Controls.Add(this.ui_removeShapeInfoButton);
            this.Controls.Add(this.ui_featureTypeTB);
            this.Controls.Add(this.ui_geoPropLabel);
            this.Controls.Add(this.ui_addShapeInfoButton);
            this.Controls.Add(this.ui_textPropLabel);
            this.Controls.Add(this.ui_textSymbolLabel);
            this.Controls.Add(this.ui_textSymbolButton);
            this.Name = "FeatureStyleControl";
            this.Size = new System.Drawing.Size(511, 448);
            this.Load += new System.EventHandler(this.FeatureStyleControl_Load);
            this.ui_symbCatGroupBox.ResumeLayout(false);
            this.ui_symbCatGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ui_featureTypeLabel;
        private System.Windows.Forms.TextBox ui_featureTypeTB;
        private System.Windows.Forms.Label ui_textSymbolLabel;
        private System.Windows.Forms.Label ui_textPropLabel;
        private System.Windows.Forms.Label ui_geoPropLabel;
        private System.Windows.Forms.ListView ui_symbCatValsListView;
        private System.Windows.Forms.Button ui_addShapeInfoButton;
        private System.Windows.Forms.Button ui_removeShapeInfoButton;
        private System.Windows.Forms.Button ui_textSymbolButton;
        private System.Windows.Forms.ToolTip ui_toolTip;
        private System.Windows.Forms.ComboBox ui_geoPropComboBox;
        private System.Windows.Forms.ListView ui_shapeInfoLV;
        private System.Windows.Forms.Button ui_removeSymCatButton;
        private System.Windows.Forms.Button ui_addSymCatButton;
        private System.Windows.Forms.GroupBox ui_symbCatGroupBox;
        private System.Windows.Forms.ComboBox ui_textPropCB;
        private System.Windows.Forms.ComboBox ui_symbCatPropNameCB;
    }
}
