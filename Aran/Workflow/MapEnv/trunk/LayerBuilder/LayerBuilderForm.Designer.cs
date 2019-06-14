using Aran.Controls;
namespace MapEnv
{
    partial class LayerBuilderForm
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
            this.ui_prevButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_featureTypesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_pageTextLabel = new System.Windows.Forms.Label();
            this.ui_sheetPanel = new System.Windows.Forms.Panel();
            this.ui_featureStyleControl = new MapEnv.FeatureStyleControl();
            this.ui_featureTypesPanel = new System.Windows.Forms.Panel();
            this.ui_layerNameTB = new System.Windows.Forms.TextBox();
            this.ui_layerNameLabel = new System.Windows.Forms.Label();
            this.ui_layerTypeCB = new System.Windows.Forms.ComboBox();
            this.ui_layerTypeLabel = new System.Windows.Forms.Label();
            this.ui_filterControl = new Aran.Controls.FilterControl();
            this.panel1.SuspendLayout();
            this.ui_sheetPanel.SuspendLayout();
            this.ui_featureTypesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_prevButton
            // 
            this.ui_prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_prevButton.Location = new System.Drawing.Point(312, 449);
            this.ui_prevButton.Name = "ui_prevButton";
            this.ui_prevButton.Size = new System.Drawing.Size(75, 23);
            this.ui_prevButton.TabIndex = 0;
            this.ui_prevButton.Text = "< Back";
            this.ui_prevButton.UseVisualStyleBackColor = true;
            this.ui_prevButton.Visible = false;
            this.ui_prevButton.Click += new System.EventHandler(this.uiEvents_prevButton_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nextButton.Enabled = false;
            this.ui_nextButton.Location = new System.Drawing.Point(393, 449);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(75, 23);
            this.ui_nextButton.TabIndex = 1;
            this.ui_nextButton.Text = "Next >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Click += new System.EventHandler(this.uiEvents_nextButton_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(474, 449);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 2;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.uiEvent_cancelButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(12, 440);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(537, 2);
            this.label1.TabIndex = 3;
            // 
            // ui_featureTypesListView
            // 
            this.ui_featureTypesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featureTypesListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_featureTypesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ui_featureTypesListView.HideSelection = false;
            this.ui_featureTypesListView.Location = new System.Drawing.Point(3, 30);
            this.ui_featureTypesListView.Name = "ui_featureTypesListView";
            this.ui_featureTypesListView.Size = new System.Drawing.Size(523, 68);
            this.ui_featureTypesListView.TabIndex = 4;
            this.ui_featureTypesListView.UseCompatibleStateImageBehavior = false;
            this.ui_featureTypesListView.View = System.Windows.Forms.View.List;
            this.ui_featureTypesListView.SelectedIndexChanged += new System.EventHandler(this.uiEvents_featureTypesListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 160;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ui_pageTextLabel);
            this.panel1.Location = new System.Drawing.Point(-9, -8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 48);
            this.panel1.TabIndex = 6;
            // 
            // ui_pageTextLabel
            // 
            this.ui_pageTextLabel.AutoSize = true;
            this.ui_pageTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_pageTextLabel.Location = new System.Drawing.Point(17, 16);
            this.ui_pageTextLabel.Name = "ui_pageTextLabel";
            this.ui_pageTextLabel.Size = new System.Drawing.Size(149, 16);
            this.ui_pageTextLabel.TabIndex = 0;
            this.ui_pageTextLabel.Text = "Select Feature Type";
            // 
            // ui_sheetPanel
            // 
            this.ui_sheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_sheetPanel.Controls.Add(this.ui_featureStyleControl);
            this.ui_sheetPanel.Controls.Add(this.ui_featureTypesPanel);
            this.ui_sheetPanel.Controls.Add(this.ui_filterControl);
            this.ui_sheetPanel.Location = new System.Drawing.Point(5, 46);
            this.ui_sheetPanel.Name = "ui_sheetPanel";
            this.ui_sheetPanel.Size = new System.Drawing.Size(542, 391);
            this.ui_sheetPanel.TabIndex = 7;
            // 
            // ui_featureStyleControl
            // 
            this.ui_featureStyleControl.Location = new System.Drawing.Point(257, 140);
            this.ui_featureStyleControl.Name = "ui_featureStyleControl";
            this.ui_featureStyleControl.Size = new System.Drawing.Size(286, 415);
            this.ui_featureStyleControl.TabIndex = 8;
            // 
            // ui_featureTypesPanel
            // 
            this.ui_featureTypesPanel.Controls.Add(this.ui_layerNameTB);
            this.ui_featureTypesPanel.Controls.Add(this.ui_layerNameLabel);
            this.ui_featureTypesPanel.Controls.Add(this.ui_layerTypeCB);
            this.ui_featureTypesPanel.Controls.Add(this.ui_layerTypeLabel);
            this.ui_featureTypesPanel.Controls.Add(this.ui_featureTypesListView);
            this.ui_featureTypesPanel.Location = new System.Drawing.Point(10, 12);
            this.ui_featureTypesPanel.Name = "ui_featureTypesPanel";
            this.ui_featureTypesPanel.Size = new System.Drawing.Size(529, 101);
            this.ui_featureTypesPanel.TabIndex = 7;
            this.ui_featureTypesPanel.Visible = false;
            // 
            // ui_layerNameTB
            // 
            this.ui_layerNameTB.Location = new System.Drawing.Point(76, 3);
            this.ui_layerNameTB.Name = "ui_layerNameTB";
            this.ui_layerNameTB.Size = new System.Drawing.Size(144, 20);
            this.ui_layerNameTB.TabIndex = 11;
            this.ui_layerNameTB.TextChanged += new System.EventHandler(this.uiEvents_layerNameTB_TextChanged);
            this.ui_layerNameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiEvents_layerNameTB_KeyDown);
            // 
            // ui_layerNameLabel
            // 
            this.ui_layerNameLabel.AutoSize = true;
            this.ui_layerNameLabel.Location = new System.Drawing.Point(3, 5);
            this.ui_layerNameLabel.Name = "ui_layerNameLabel";
            this.ui_layerNameLabel.Size = new System.Drawing.Size(67, 13);
            this.ui_layerNameLabel.TabIndex = 10;
            this.ui_layerNameLabel.Text = "Layer Name:";
            // 
            // ui_layerTypeCB
            // 
            this.ui_layerTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_layerTypeCB.FormattingEnabled = true;
            this.ui_layerTypeCB.Location = new System.Drawing.Point(344, 2);
            this.ui_layerTypeCB.Name = "ui_layerTypeCB";
            this.ui_layerTypeCB.Size = new System.Drawing.Size(156, 21);
            this.ui_layerTypeCB.TabIndex = 9;
            this.ui_layerTypeCB.SelectedIndexChanged += new System.EventHandler(this.uiEvents_LayerTypeCB_SelectedIndexChanged);
            // 
            // ui_layerTypeLabel
            // 
            this.ui_layerTypeLabel.AutoSize = true;
            this.ui_layerTypeLabel.Location = new System.Drawing.Point(275, 6);
            this.ui_layerTypeLabel.Name = "ui_layerTypeLabel";
            this.ui_layerTypeLabel.Size = new System.Drawing.Size(63, 13);
            this.ui_layerTypeLabel.TabIndex = 8;
            this.ui_layerTypeLabel.Text = "Layer Type:";
            // 
            // ui_filterControl
            // 
            this.ui_filterControl.FeatureDescription = null;
            this.ui_filterControl.FillDataGridColumnsHandler = null;
            this.ui_filterControl.LoadFeatureListByDependHandler = null;
            this.ui_filterControl.Location = new System.Drawing.Point(16, 137);
            this.ui_filterControl.Name = "ui_filterControl";
            this.ui_filterControl.SetDataGridRowHandler = null;
            this.ui_filterControl.Size = new System.Drawing.Size(241, 368);
            this.ui_filterControl.TabIndex = 5;
            this.ui_filterControl.Visible = false;
            // 
            // LayerBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(559, 484);
            this.Controls.Add(this.ui_sheetPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_nextButton);
            this.Controls.Add(this.ui_prevButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayerBuilderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Layer Builder Wizard";
            this.Load += new System.EventHandler(this.LayerBuilderForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ui_sheetPanel.ResumeLayout(false);
            this.ui_featureTypesPanel.ResumeLayout(false);
            this.ui_featureTypesPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_prevButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView ui_featureTypesListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private FilterControl ui_filterControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel ui_sheetPanel;
        private System.Windows.Forms.Label ui_pageTextLabel;
        private System.Windows.Forms.Panel ui_featureTypesPanel;
        private System.Windows.Forms.ComboBox ui_layerTypeCB;
        private System.Windows.Forms.Label ui_layerTypeLabel;
        private System.Windows.Forms.TextBox ui_layerNameTB;
        private System.Windows.Forms.Label ui_layerNameLabel;
        private FeatureStyleControl ui_featureStyleControl;
    }
}