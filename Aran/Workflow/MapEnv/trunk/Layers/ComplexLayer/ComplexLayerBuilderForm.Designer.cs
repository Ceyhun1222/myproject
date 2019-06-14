namespace MapEnv.ComplexLayer
{
    partial class ComplexLayerBuilderForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComplexLayerBuilderForm));
            this.ui_tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ui_mainAreaPanel = new System.Windows.Forms.Panel();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_prevButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_selectBaseOnFeatTSBI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_splitter = new System.Windows.Forms.ToolStripSeparator();
            this.saveLinkInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadLinkRelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_layerNameTB = new System.Windows.Forms.TextBox();
            this.lineControl1 = new MapEnv.Controls.LineControl();
            this.ui_featureTypeSelector = new MapEnv.Controls.FeatureTypeSelector();
            this.ui_layerTreeContElementHos = new System.Windows.Forms.Integration.ElementHost();
            this.ui_layerTreeCont = new MapEnv.ComplexLayer.CompLayerTreeControl();
            label1 = new System.Windows.Forms.Label();
            this.ui_tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ui_mainAreaPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_tabControl
            // 
            this.ui_tabControl.Controls.Add(this.tabPage1);
            this.ui_tabControl.Controls.Add(this.tabPage2);
            this.ui_tabControl.Location = new System.Drawing.Point(8, 8);
            this.ui_tabControl.Name = "ui_tabControl";
            this.ui_tabControl.SelectedIndex = 0;
            this.ui_tabControl.Size = new System.Drawing.Size(628, 297);
            this.ui_tabControl.TabIndex = 0;
            this.ui_tabControl.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ui_featureTypeSelector);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(620, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ui_layerTreeContElementHos);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(620, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // ui_mainAreaPanel
            // 
            this.ui_mainAreaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainAreaPanel.Controls.Add(this.ui_tabControl);
            this.ui_mainAreaPanel.Location = new System.Drawing.Point(4, 4);
            this.ui_mainAreaPanel.Name = "ui_mainAreaPanel";
            this.ui_mainAreaPanel.Size = new System.Drawing.Size(656, 343);
            this.ui_mainAreaPanel.TabIndex = 1;
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(583, 370);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 5;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nextButton.Enabled = false;
            this.ui_nextButton.Location = new System.Drawing.Point(502, 370);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(75, 23);
            this.ui_nextButton.TabIndex = 4;
            this.ui_nextButton.Text = "Next >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_prevButton
            // 
            this.ui_prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_prevButton.Location = new System.Drawing.Point(421, 370);
            this.ui_prevButton.Name = "ui_prevButton";
            this.ui_prevButton.Size = new System.Drawing.Size(75, 23);
            this.ui_prevButton.TabIndex = 3;
            this.ui_prevButton.Text = "< Back";
            this.ui_prevButton.UseVisualStyleBackColor = true;
            this.ui_prevButton.Visible = false;
            this.ui_prevButton.Click += new System.EventHandler(this.Prev_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.AutoSize = true;
            this.button1.Image = global::MapEnv.Properties.Resources.settings_24;
            this.button1.Location = new System.Drawing.Point(12, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 33);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_selectBaseOnFeatTSBI,
            this.ui_splitter,
            this.saveLinkInfoToolStripMenuItem,
            this.loadLinkRelationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(194, 76);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // ui_selectBaseOnFeatTSBI
            // 
            this.ui_selectBaseOnFeatTSBI.Name = "ui_selectBaseOnFeatTSBI";
            this.ui_selectBaseOnFeatTSBI.Size = new System.Drawing.Size(193, 22);
            this.ui_selectBaseOnFeatTSBI.Text = "Select Base On Feature";
            this.ui_selectBaseOnFeatTSBI.Visible = false;
            this.ui_selectBaseOnFeatTSBI.Click += new System.EventHandler(this.SelectBaseOnFeature_Click);
            // 
            // ui_splitter
            // 
            this.ui_splitter.Name = "ui_splitter";
            this.ui_splitter.Size = new System.Drawing.Size(190, 6);
            this.ui_splitter.Visible = false;
            // 
            // saveLinkInfoToolStripMenuItem
            // 
            this.saveLinkInfoToolStripMenuItem.Name = "saveLinkInfoToolStripMenuItem";
            this.saveLinkInfoToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.saveLinkInfoToolStripMenuItem.Text = "Save Feature Relation";
            this.saveLinkInfoToolStripMenuItem.Click += new System.EventHandler(this.SaveFeatureRelation_Click);
            // 
            // loadLinkRelationToolStripMenuItem
            // 
            this.loadLinkRelationToolStripMenuItem.Name = "loadLinkRelationToolStripMenuItem";
            this.loadLinkRelationToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.loadLinkRelationToolStripMenuItem.Text = "Load Feature Relation";
            this.loadLinkRelationToolStripMenuItem.Click += new System.EventHandler(this.LoadFeatureRelation_Click);
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(77, 376);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(67, 13);
            label1.TabIndex = 8;
            label1.Text = "Layer Name:";
            // 
            // ui_layerNameTB
            // 
            this.ui_layerNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_layerNameTB.Location = new System.Drawing.Point(150, 373);
            this.ui_layerNameTB.Name = "ui_layerNameTB";
            this.ui_layerNameTB.Size = new System.Drawing.Size(189, 20);
            this.ui_layerNameTB.TabIndex = 9;
            // 
            // lineControl1
            // 
            this.lineControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineControl1.Location = new System.Drawing.Point(-15, 353);
            this.lineControl1.Name = "lineControl1";
            this.lineControl1.Size = new System.Drawing.Size(695, 8);
            this.lineControl1.TabIndex = 3;
            // 
            // ui_featureTypeSelector
            // 
            this.ui_featureTypeSelector.Location = new System.Drawing.Point(17, 11);
            this.ui_featureTypeSelector.Name = "ui_featureTypeSelector";
            this.ui_featureTypeSelector.Size = new System.Drawing.Size(573, 244);
            this.ui_featureTypeSelector.TabIndex = 0;
            this.ui_featureTypeSelector.TypeSelected += new System.EventHandler(this.FeatureTypeSelector_TypeSelected);
            // 
            // ui_layerTreeContElementHos
            // 
            this.ui_layerTreeContElementHos.BackColor = System.Drawing.SystemColors.Control;
            this.ui_layerTreeContElementHos.Location = new System.Drawing.Point(6, 13);
            this.ui_layerTreeContElementHos.Name = "ui_layerTreeContElementHos";
            this.ui_layerTreeContElementHos.Size = new System.Drawing.Size(589, 237);
            this.ui_layerTreeContElementHos.TabIndex = 0;
            this.ui_layerTreeContElementHos.Child = this.ui_layerTreeCont;
            // 
            // ComplexLayerBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 403);
            this.Controls.Add(this.ui_layerNameTB);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lineControl1);
            this.Controls.Add(this.ui_nextButton);
            this.Controls.Add(this.ui_prevButton);
            this.Controls.Add(this.ui_mainAreaPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(613, 316);
            this.Name = "ComplexLayerBuilderForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Form_Load);
            this.ui_tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ui_mainAreaPanel.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl ui_tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel ui_mainAreaPanel;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_prevButton;
        private Controls.FeatureTypeSelector ui_featureTypeSelector;
        private System.Windows.Forms.Integration.ElementHost ui_layerTreeContElementHos;
        private CompLayerTreeControl ui_layerTreeCont;
        private Controls.LineControl lineControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ui_selectBaseOnFeatTSBI;
        private System.Windows.Forms.ToolStripSeparator ui_splitter;
        private System.Windows.Forms.ToolStripMenuItem saveLinkInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadLinkRelationToolStripMenuItem;
        private System.Windows.Forms.TextBox ui_layerNameTB;
    }
}