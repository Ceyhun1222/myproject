using Aran.Controls;
namespace MapEnv
{
    partial class SimpleLayerBuilderForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_pageTextLabel = new System.Windows.Forms.Label();
            this.ui_containerPanel = new System.Windows.Forms.Panel();
            this.ui_hiddenTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ui_featureTypesPanel = new System.Windows.Forms.Panel();
            this.ui_featureTypeSelector = new MapEnv.Controls.FeatureTypeSelector();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ui_featureStyleControl = new MapEnv.FeatureStyleControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ui_filterControl = new Aran.Controls.FilterControl();
            this.ui_layerNameTB = new System.Windows.Forms.TextBox();
            this.ui_layerNameLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.ui_containerPanel.SuspendLayout();
            this.ui_hiddenTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ui_featureTypesPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_prevButton
            // 
            this.ui_prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_prevButton.Location = new System.Drawing.Point(481, 571);
            this.ui_prevButton.Name = "ui_prevButton";
            this.ui_prevButton.Size = new System.Drawing.Size(75, 23);
            this.ui_prevButton.TabIndex = 0;
            this.ui_prevButton.Text = "< Back";
            this.ui_prevButton.UseVisualStyleBackColor = true;
            this.ui_prevButton.Visible = false;
            this.ui_prevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nextButton.Enabled = false;
            this.ui_nextButton.Location = new System.Drawing.Point(562, 571);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(75, 23);
            this.ui_nextButton.TabIndex = 1;
            this.ui_nextButton.Text = "Next >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(643, 571);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 2;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            this.ui_cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Location = new System.Drawing.Point(12, 562);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(706, 2);
            this.label1.TabIndex = 3;
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
            this.panel1.Size = new System.Drawing.Size(761, 48);
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
            // ui_containerPanel
            // 
            this.ui_containerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_containerPanel.Controls.Add(this.ui_hiddenTabControl);
            this.ui_containerPanel.Location = new System.Drawing.Point(3, 44);
            this.ui_containerPanel.Name = "ui_containerPanel";
            this.ui_containerPanel.Size = new System.Drawing.Size(713, 515);
            this.ui_containerPanel.TabIndex = 7;
            // 
            // ui_hiddenTabControl
            // 
            this.ui_hiddenTabControl.Controls.Add(this.tabPage1);
            this.ui_hiddenTabControl.Controls.Add(this.tabPage2);
            this.ui_hiddenTabControl.Controls.Add(this.tabPage3);
            this.ui_hiddenTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_hiddenTabControl.Location = new System.Drawing.Point(0, 0);
            this.ui_hiddenTabControl.Name = "ui_hiddenTabControl";
            this.ui_hiddenTabControl.SelectedIndex = 0;
            this.ui_hiddenTabControl.Size = new System.Drawing.Size(713, 515);
            this.ui_hiddenTabControl.TabIndex = 8;
            this.ui_hiddenTabControl.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ui_featureTypesPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(705, 489);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // ui_featureTypesPanel
            // 
            this.ui_featureTypesPanel.Controls.Add(this.ui_featureTypeSelector);
            this.ui_featureTypesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featureTypesPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_featureTypesPanel.Name = "ui_featureTypesPanel";
            this.ui_featureTypesPanel.Size = new System.Drawing.Size(699, 483);
            this.ui_featureTypesPanel.TabIndex = 7;
            this.ui_featureTypesPanel.Visible = false;
            // 
            // ui_featureTypeSelector
            // 
            this.ui_featureTypeSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featureTypeSelector.Location = new System.Drawing.Point(0, 0);
            this.ui_featureTypeSelector.Name = "ui_featureTypeSelector";
            this.ui_featureTypeSelector.Size = new System.Drawing.Size(699, 483);
            this.ui_featureTypeSelector.TabIndex = 12;
            this.ui_featureTypeSelector.TypeSelected += new System.EventHandler(this.FeatureTypeSelector_TypeSelected);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ui_featureStyleControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(858, 532);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // ui_featureStyleControl
            // 
            this.ui_featureStyleControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featureStyleControl.Location = new System.Drawing.Point(3, 3);
            this.ui_featureStyleControl.Name = "ui_featureStyleControl";
            this.ui_featureStyleControl.Size = new System.Drawing.Size(852, 526);
            this.ui_featureStyleControl.TabIndex = 8;
            this.ui_featureStyleControl.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ui_filterControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(858, 532);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // ui_filterControl
            // 
            this.ui_filterControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_filterControl.LoadFeatureListByDependHandler = null;
            this.ui_filterControl.Location = new System.Drawing.Point(3, 3);
            this.ui_filterControl.Name = "ui_filterControl";
            this.ui_filterControl.Size = new System.Drawing.Size(852, 526);
            this.ui_filterControl.TabIndex = 5;
            this.ui_filterControl.Visible = false;
            // 
            // ui_layerNameTB
            // 
            this.ui_layerNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_layerNameTB.Location = new System.Drawing.Point(82, 573);
            this.ui_layerNameTB.Name = "ui_layerNameTB";
            this.ui_layerNameTB.Size = new System.Drawing.Size(144, 20);
            this.ui_layerNameTB.TabIndex = 11;
            this.ui_layerNameTB.TextChanged += new System.EventHandler(this.uiEvents_layerNameTB_TextChanged);
            this.ui_layerNameTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uiEvents_layerNameTB_KeyDown);
            // 
            // ui_layerNameLabel
            // 
            this.ui_layerNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_layerNameLabel.AutoSize = true;
            this.ui_layerNameLabel.Location = new System.Drawing.Point(9, 576);
            this.ui_layerNameLabel.Name = "ui_layerNameLabel";
            this.ui_layerNameLabel.Size = new System.Drawing.Size(67, 13);
            this.ui_layerNameLabel.TabIndex = 10;
            this.ui_layerNameLabel.Text = "Layer Name:";
            // 
            // SimpleLayerBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(728, 606);
            this.Controls.Add(this.ui_layerNameLabel);
            this.Controls.Add(this.ui_layerNameTB);
            this.Controls.Add(this.ui_containerPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_nextButton);
            this.Controls.Add(this.ui_prevButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(496, 385);
            this.Name = "SimpleLayerBuilderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Layer Builder Wizard";
            this.Load += new System.EventHandler(this.LayerBuilderForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ui_containerPanel.ResumeLayout(false);
            this.ui_hiddenTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ui_featureTypesPanel.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_prevButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Label label1;
        private FilterControl ui_filterControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel ui_containerPanel;
        private System.Windows.Forms.Label ui_pageTextLabel;
        private System.Windows.Forms.Panel ui_featureTypesPanel;
        private System.Windows.Forms.TextBox ui_layerNameTB;
        private System.Windows.Forms.Label ui_layerNameLabel;
        private FeatureStyleControl ui_featureStyleControl;
        private Controls.FeatureTypeSelector ui_featureTypeSelector;
        private System.Windows.Forms.TabControl ui_hiddenTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
    }
}