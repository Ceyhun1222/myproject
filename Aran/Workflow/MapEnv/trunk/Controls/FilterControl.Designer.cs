namespace MapEnv
{
    partial class FilterControl
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
            this.ui_operChoiceComboBox = new System.Windows.Forms.ComboBox();
            this.ui_label1 = new System.Windows.Forms.Label();
            this.ui_opersTabControl = new System.Windows.Forms.TabControl();
            this.ui_comparasonTabPage = new System.Windows.Forms.TabPage();
            this.ui_comparasonPanel = new System.Windows.Forms.Panel();
            this.ui_compOperTypeFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_aimPropertyControl = new Aran.Controls.AimPropertyControl();
            this.ui_label3 = new System.Windows.Forms.Label();
            this.ui_label2 = new System.Windows.Forms.Label();
            this.ui_logicTabPage = new System.Windows.Forms.TabPage();
            this.ui_logicPanel = new System.Windows.Forms.Panel();
            this.ui_spatialTabPage = new System.Windows.Forms.TabPage();
            this.ui_spatialPanel = new System.Windows.Forms.Panel();
            this.ui_spatiFilterChoiceContainerPanel = new System.Windows.Forms.Panel();
            this.ui_spatFilterDistUomCB = new System.Windows.Forms.ComboBox();
            this.ui_spatFilterDistValueNUD = new System.Windows.Forms.NumericUpDown();
            this.ui_spatFilterDistLabel = new System.Windows.Forms.Label();
            this.ui_spatFilterCoordTB = new System.Windows.Forms.TextBox();
            this.ui_spatFilterCoordLabel = new System.Windows.Forms.Label();
            this.ui_withinRB = new System.Windows.Forms.RadioButton();
            this.ui_dwithinRB = new System.Windows.Forms.RadioButton();
            this.ui_geoPropNameCB = new System.Windows.Forms.ComboBox();
            this.ui_geoPropNameLabel = new System.Windows.Forms.Label();
            this.ui_layerFilterTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ui_featureLayersCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ui_compPropSelControl = new MapEnv.PropertySelectorControl();
            this.ui_layerFilterPropSelCont = new MapEnv.PropertySelectorControl();
            this.ui_opersTabControl.SuspendLayout();
            this.ui_comparasonTabPage.SuspendLayout();
            this.ui_comparasonPanel.SuspendLayout();
            this.ui_logicTabPage.SuspendLayout();
            this.ui_spatialTabPage.SuspendLayout();
            this.ui_spatialPanel.SuspendLayout();
            this.ui_spatiFilterChoiceContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_spatFilterDistValueNUD)).BeginInit();
            this.ui_layerFilterTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_operChoiceComboBox
            // 
            this.ui_operChoiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_operChoiceComboBox.FormattingEnabled = true;
            this.ui_operChoiceComboBox.Location = new System.Drawing.Point(66, 3);
            this.ui_operChoiceComboBox.Name = "ui_operChoiceComboBox";
            this.ui_operChoiceComboBox.Size = new System.Drawing.Size(99, 21);
            this.ui_operChoiceComboBox.TabIndex = 1;
            this.ui_operChoiceComboBox.SelectedIndexChanged += new System.EventHandler(this.OperChoice_SelectedIndexChanged);
            // 
            // ui_label1
            // 
            this.ui_label1.AutoSize = true;
            this.ui_label1.Location = new System.Drawing.Point(4, 6);
            this.ui_label1.Name = "ui_label1";
            this.ui_label1.Size = new System.Drawing.Size(56, 13);
            this.ui_label1.TabIndex = 0;
            this.ui_label1.Text = "Operation:";
            // 
            // ui_opersTabControl
            // 
            this.ui_opersTabControl.Controls.Add(this.ui_comparasonTabPage);
            this.ui_opersTabControl.Controls.Add(this.ui_logicTabPage);
            this.ui_opersTabControl.Controls.Add(this.ui_spatialTabPage);
            this.ui_opersTabControl.Controls.Add(this.ui_layerFilterTabPage);
            this.ui_opersTabControl.Location = new System.Drawing.Point(2, 30);
            this.ui_opersTabControl.Name = "ui_opersTabControl";
            this.ui_opersTabControl.SelectedIndex = 0;
            this.ui_opersTabControl.Size = new System.Drawing.Size(489, 241);
            this.ui_opersTabControl.TabIndex = 2;
            this.ui_opersTabControl.Visible = false;
            // 
            // ui_comparasonTabPage
            // 
            this.ui_comparasonTabPage.Controls.Add(this.ui_comparasonPanel);
            this.ui_comparasonTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_comparasonTabPage.Name = "ui_comparasonTabPage";
            this.ui_comparasonTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_comparasonTabPage.Size = new System.Drawing.Size(481, 215);
            this.ui_comparasonTabPage.TabIndex = 0;
            this.ui_comparasonTabPage.Text = "Comparason";
            // 
            // ui_comparasonPanel
            // 
            this.ui_comparasonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_comparasonPanel.Controls.Add(this.ui_compPropSelControl);
            this.ui_comparasonPanel.Controls.Add(this.ui_compOperTypeFlowPanel);
            this.ui_comparasonPanel.Controls.Add(this.ui_aimPropertyControl);
            this.ui_comparasonPanel.Controls.Add(this.ui_label3);
            this.ui_comparasonPanel.Controls.Add(this.ui_label2);
            this.ui_comparasonPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_comparasonPanel.Name = "ui_comparasonPanel";
            this.ui_comparasonPanel.Size = new System.Drawing.Size(475, 220);
            this.ui_comparasonPanel.TabIndex = 0;
            this.ui_comparasonPanel.Visible = false;
            // 
            // ui_compOperTypeFlowPanel
            // 
            this.ui_compOperTypeFlowPanel.Location = new System.Drawing.Point(8, 71);
            this.ui_compOperTypeFlowPanel.Name = "ui_compOperTypeFlowPanel";
            this.ui_compOperTypeFlowPanel.Size = new System.Drawing.Size(319, 67);
            this.ui_compOperTypeFlowPanel.TabIndex = 3;
            // 
            // ui_aimPropertyControl
            // 
            this.ui_aimPropertyControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_aimPropertyControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_aimPropertyControl.FillDataGridColumnsHandler = null;
            this.ui_aimPropertyControl.LoadFeatureListByDependHandler = null;
            this.ui_aimPropertyControl.Location = new System.Drawing.Point(89, 144);
            this.ui_aimPropertyControl.Name = "ui_aimPropertyControl";
            this.ui_aimPropertyControl.PropInfo = null;
            this.ui_aimPropertyControl.SetDataGridRowHandler = null;
            this.ui_aimPropertyControl.Size = new System.Drawing.Size(375, 26);
            this.ui_aimPropertyControl.TabIndex = 5;
            // 
            // ui_label3
            // 
            this.ui_label3.AutoSize = true;
            this.ui_label3.Location = new System.Drawing.Point(4, 149);
            this.ui_label3.Name = "ui_label3";
            this.ui_label3.Size = new System.Drawing.Size(37, 13);
            this.ui_label3.TabIndex = 4;
            this.ui_label3.Text = "Value:";
            // 
            // ui_label2
            // 
            this.ui_label2.AutoSize = true;
            this.ui_label2.Location = new System.Drawing.Point(3, 14);
            this.ui_label2.Name = "ui_label2";
            this.ui_label2.Size = new System.Drawing.Size(80, 13);
            this.ui_label2.TabIndex = 0;
            this.ui_label2.Text = "Property Name:";
            // 
            // ui_logicTabPage
            // 
            this.ui_logicTabPage.Controls.Add(this.ui_logicPanel);
            this.ui_logicTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_logicTabPage.Name = "ui_logicTabPage";
            this.ui_logicTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_logicTabPage.Size = new System.Drawing.Size(481, 215);
            this.ui_logicTabPage.TabIndex = 1;
            this.ui_logicTabPage.Text = "Logic";
            // 
            // ui_logicPanel
            // 
            this.ui_logicPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_logicPanel.Name = "ui_logicPanel";
            this.ui_logicPanel.Size = new System.Drawing.Size(338, 215);
            this.ui_logicPanel.TabIndex = 1;
            this.ui_logicPanel.Visible = false;
            // 
            // ui_spatialTabPage
            // 
            this.ui_spatialTabPage.Controls.Add(this.ui_spatialPanel);
            this.ui_spatialTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_spatialTabPage.Name = "ui_spatialTabPage";
            this.ui_spatialTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_spatialTabPage.Size = new System.Drawing.Size(481, 215);
            this.ui_spatialTabPage.TabIndex = 2;
            this.ui_spatialTabPage.Text = "Spatial";
            // 
            // ui_spatialPanel
            // 
            this.ui_spatialPanel.Controls.Add(this.ui_spatiFilterChoiceContainerPanel);
            this.ui_spatialPanel.Controls.Add(this.ui_withinRB);
            this.ui_spatialPanel.Controls.Add(this.ui_dwithinRB);
            this.ui_spatialPanel.Controls.Add(this.ui_geoPropNameCB);
            this.ui_spatialPanel.Controls.Add(this.ui_geoPropNameLabel);
            this.ui_spatialPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_spatialPanel.Name = "ui_spatialPanel";
            this.ui_spatialPanel.Size = new System.Drawing.Size(466, 215);
            this.ui_spatialPanel.TabIndex = 1;
            this.ui_spatialPanel.Visible = false;
            // 
            // ui_spatiFilterChoiceContainerPanel
            // 
            this.ui_spatiFilterChoiceContainerPanel.Controls.Add(this.ui_spatFilterDistUomCB);
            this.ui_spatiFilterChoiceContainerPanel.Controls.Add(this.ui_spatFilterDistValueNUD);
            this.ui_spatiFilterChoiceContainerPanel.Controls.Add(this.ui_spatFilterDistLabel);
            this.ui_spatiFilterChoiceContainerPanel.Controls.Add(this.ui_spatFilterCoordTB);
            this.ui_spatiFilterChoiceContainerPanel.Controls.Add(this.ui_spatFilterCoordLabel);
            this.ui_spatiFilterChoiceContainerPanel.Location = new System.Drawing.Point(120, 47);
            this.ui_spatiFilterChoiceContainerPanel.Name = "ui_spatiFilterChoiceContainerPanel";
            this.ui_spatiFilterChoiceContainerPanel.Size = new System.Drawing.Size(328, 159);
            this.ui_spatiFilterChoiceContainerPanel.TabIndex = 4;
            // 
            // ui_spatFilterDistUomCB
            // 
            this.ui_spatFilterDistUomCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_spatFilterDistUomCB.FormattingEnabled = true;
            this.ui_spatFilterDistUomCB.Location = new System.Drawing.Point(244, 43);
            this.ui_spatFilterDistUomCB.Name = "ui_spatFilterDistUomCB";
            this.ui_spatFilterDistUomCB.Size = new System.Drawing.Size(70, 21);
            this.ui_spatFilterDistUomCB.TabIndex = 4;
            // 
            // ui_spatFilterDistValueNUD
            // 
            this.ui_spatFilterDistValueNUD.DecimalPlaces = 4;
            this.ui_spatFilterDistValueNUD.Location = new System.Drawing.Point(81, 43);
            this.ui_spatFilterDistValueNUD.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.ui_spatFilterDistValueNUD.Name = "ui_spatFilterDistValueNUD";
            this.ui_spatFilterDistValueNUD.Size = new System.Drawing.Size(157, 20);
            this.ui_spatFilterDistValueNUD.TabIndex = 3;
            // 
            // ui_spatFilterDistLabel
            // 
            this.ui_spatFilterDistLabel.AutoSize = true;
            this.ui_spatFilterDistLabel.Location = new System.Drawing.Point(14, 45);
            this.ui_spatFilterDistLabel.Name = "ui_spatFilterDistLabel";
            this.ui_spatFilterDistLabel.Size = new System.Drawing.Size(52, 13);
            this.ui_spatFilterDistLabel.TabIndex = 2;
            this.ui_spatFilterDistLabel.Text = "Distance:";
            // 
            // ui_spatFilterCoordTB
            // 
            this.ui_spatFilterCoordTB.Location = new System.Drawing.Point(81, 11);
            this.ui_spatFilterCoordTB.Name = "ui_spatFilterCoordTB";
            this.ui_spatFilterCoordTB.Size = new System.Drawing.Size(233, 20);
            this.ui_spatFilterCoordTB.TabIndex = 1;
            // 
            // ui_spatFilterCoordLabel
            // 
            this.ui_spatFilterCoordLabel.AutoSize = true;
            this.ui_spatFilterCoordLabel.Location = new System.Drawing.Point(14, 14);
            this.ui_spatFilterCoordLabel.Name = "ui_spatFilterCoordLabel";
            this.ui_spatFilterCoordLabel.Size = new System.Drawing.Size(61, 13);
            this.ui_spatFilterCoordLabel.TabIndex = 0;
            this.ui_spatFilterCoordLabel.Text = "Coordinate:";
            // 
            // ui_withinRB
            // 
            this.ui_withinRB.AutoSize = true;
            this.ui_withinRB.Enabled = false;
            this.ui_withinRB.Location = new System.Drawing.Point(15, 70);
            this.ui_withinRB.Name = "ui_withinRB";
            this.ui_withinRB.Size = new System.Drawing.Size(55, 17);
            this.ui_withinRB.TabIndex = 3;
            this.ui_withinRB.Text = "Within";
            this.ui_withinRB.UseVisualStyleBackColor = true;
            // 
            // ui_dwithinRB
            // 
            this.ui_dwithinRB.AutoSize = true;
            this.ui_dwithinRB.Checked = true;
            this.ui_dwithinRB.Location = new System.Drawing.Point(15, 47);
            this.ui_dwithinRB.Name = "ui_dwithinRB";
            this.ui_dwithinRB.Size = new System.Drawing.Size(63, 17);
            this.ui_dwithinRB.TabIndex = 2;
            this.ui_dwithinRB.TabStop = true;
            this.ui_dwithinRB.Text = "DWithin";
            this.ui_dwithinRB.UseVisualStyleBackColor = true;
            // 
            // ui_geoPropNameCB
            // 
            this.ui_geoPropNameCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_geoPropNameCB.FormattingEnabled = true;
            this.ui_geoPropNameCB.Location = new System.Drawing.Point(120, 11);
            this.ui_geoPropNameCB.Name = "ui_geoPropNameCB";
            this.ui_geoPropNameCB.Size = new System.Drawing.Size(328, 21);
            this.ui_geoPropNameCB.TabIndex = 1;
            // 
            // ui_geoPropNameLabel
            // 
            this.ui_geoPropNameLabel.AutoSize = true;
            this.ui_geoPropNameLabel.Location = new System.Drawing.Point(11, 14);
            this.ui_geoPropNameLabel.Name = "ui_geoPropNameLabel";
            this.ui_geoPropNameLabel.Size = new System.Drawing.Size(103, 13);
            this.ui_geoPropNameLabel.TabIndex = 0;
            this.ui_geoPropNameLabel.Text = "Geo Property Name:";
            // 
            // ui_layerFilterTabPage
            // 
            this.ui_layerFilterTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.ui_layerFilterTabPage.Controls.Add(this.panel1);
            this.ui_layerFilterTabPage.Location = new System.Drawing.Point(4, 22);
            this.ui_layerFilterTabPage.Name = "ui_layerFilterTabPage";
            this.ui_layerFilterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ui_layerFilterTabPage.Size = new System.Drawing.Size(481, 215);
            this.ui_layerFilterTabPage.TabIndex = 3;
            this.ui_layerFilterTabPage.Text = "Layer Filter";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ui_layerFilterPropSelCont);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ui_featureLayersCB);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(463, 204);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Feature Layers";
            // 
            // ui_featureLayersCB
            // 
            this.ui_featureLayersCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_featureLayersCB.FormattingEnabled = true;
            this.ui_featureLayersCB.Location = new System.Drawing.Point(89, 48);
            this.ui_featureLayersCB.Name = "ui_featureLayersCB";
            this.ui_featureLayersCB.Size = new System.Drawing.Size(350, 21);
            this.ui_featureLayersCB.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Property Name:";
            // 
            // ui_compPropSelControl
            // 
            this.ui_compPropSelControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_compPropSelControl.ClassInfo = null;
            this.ui_compPropSelControl.Location = new System.Drawing.Point(0, 30);
            this.ui_compPropSelControl.Name = "ui_compPropSelControl";
            this.ui_compPropSelControl.Size = new System.Drawing.Size(472, 28);
            this.ui_compPropSelControl.TabIndex = 7;
            this.ui_compPropSelControl.Value = null;
            this.ui_compPropSelControl.AfterSelect += new MapEnv.PropertySelectedEventHandler(this.CompPropSel_AfterSelect);
            this.ui_compPropSelControl.ValueChanged += new System.EventHandler(this.CompPropSel_ValueChanged);
            // 
            // ui_layerFilterPropSelCont
            // 
            this.ui_layerFilterPropSelCont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_layerFilterPropSelCont.ClassInfo = null;
            this.ui_layerFilterPropSelCont.Location = new System.Drawing.Point(89, 14);
            this.ui_layerFilterPropSelCont.Name = "ui_layerFilterPropSelCont";
            this.ui_layerFilterPropSelCont.Size = new System.Drawing.Size(350, 28);
            this.ui_layerFilterPropSelCont.TabIndex = 8;
            this.ui_layerFilterPropSelCont.Value = null;
            this.ui_layerFilterPropSelCont.AfterSelect += new MapEnv.PropertySelectedEventHandler(this.LayerFilterPropSel_AfterSelect);
            this.ui_layerFilterPropSelCont.ValueChanged += new System.EventHandler(this.LayerFilterPropSel_ValueChanged);
            // 
            // FilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_opersTabControl);
            this.Controls.Add(this.ui_label1);
            this.Controls.Add(this.ui_operChoiceComboBox);
            this.Name = "FilterControl";
            this.Size = new System.Drawing.Size(493, 278);
            this.Load += new System.EventHandler(this.FilterControl_Load);
            this.ui_opersTabControl.ResumeLayout(false);
            this.ui_comparasonTabPage.ResumeLayout(false);
            this.ui_comparasonPanel.ResumeLayout(false);
            this.ui_comparasonPanel.PerformLayout();
            this.ui_logicTabPage.ResumeLayout(false);
            this.ui_spatialTabPage.ResumeLayout(false);
            this.ui_spatialPanel.ResumeLayout(false);
            this.ui_spatialPanel.PerformLayout();
            this.ui_spatiFilterChoiceContainerPanel.ResumeLayout(false);
            this.ui_spatiFilterChoiceContainerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_spatFilterDistValueNUD)).EndInit();
            this.ui_layerFilterTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ui_operChoiceComboBox;
        private System.Windows.Forms.Label ui_label1;
        private System.Windows.Forms.TabControl ui_opersTabControl;
        private System.Windows.Forms.TabPage ui_comparasonTabPage;
        private System.Windows.Forms.TabPage ui_logicTabPage;
        private System.Windows.Forms.TabPage ui_spatialTabPage;
        private System.Windows.Forms.Panel ui_comparasonPanel;
        private System.Windows.Forms.Panel ui_logicPanel;
        private System.Windows.Forms.Panel ui_spatialPanel;
        private System.Windows.Forms.Label ui_label2;
        private System.Windows.Forms.Label ui_label3;
        private Aran.Controls.AimPropertyControl ui_aimPropertyControl;
        private System.Windows.Forms.FlowLayoutPanel ui_compOperTypeFlowPanel;
        private System.Windows.Forms.ToolTip ui_toolTip;
        private System.Windows.Forms.TabPage ui_layerFilterTabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ui_featureLayersCB;
        private PropertySelectorControl ui_compPropSelControl;
        private PropertySelectorControl ui_layerFilterPropSelCont;
        private System.Windows.Forms.Label ui_geoPropNameLabel;
        private System.Windows.Forms.ComboBox ui_geoPropNameCB;
        private System.Windows.Forms.RadioButton ui_withinRB;
        private System.Windows.Forms.RadioButton ui_dwithinRB;
        private System.Windows.Forms.Panel ui_spatiFilterChoiceContainerPanel;
        private System.Windows.Forms.Label ui_spatFilterCoordLabel;
        private System.Windows.Forms.TextBox ui_spatFilterCoordTB;
        private System.Windows.Forms.NumericUpDown ui_spatFilterDistValueNUD;
        private System.Windows.Forms.Label ui_spatFilterDistLabel;
        private System.Windows.Forms.ComboBox ui_spatFilterDistUomCB;
    }
}
