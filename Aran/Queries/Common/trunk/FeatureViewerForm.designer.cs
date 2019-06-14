namespace Aran.Queries.Viewer
{
    partial class FeatureViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureViewerForm));
            this.ui_featuresTreeView = new System.Windows.Forms.TreeView();
            this.featuresIL = new System.Windows.Forms.ImageList(this.components);
            this.ui_mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.featureDetailsPanel = new System.Windows.Forms.Panel();
            this.ui_topToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.ui_applyTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_backTSB = new System.Windows.Forms.ToolStripButton();
            this.ui_gotoFeatureTSB = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.addNewCompPropsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_mainContainerPanel = new System.Windows.Forms.Panel();
            this.bottomButtonContainer = new System.Windows.Forms.Panel();
            this.btnApplyAll = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.ui_scrollableFlow = new Aran.Queries.Common.ScrollableFlow();
            this.setDBEntityNullContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_validTimePanel = new System.Windows.Forms.Panel();
            this.validTimeCancelButton = new System.Windows.Forms.Button();
            this.validTimeOkButton = new System.Windows.Forms.Button();
            this.ui_airacCycleCont = new Aran.Controls.Airac.AiracCycleControl();
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).BeginInit();
            this.ui_mainSplitContainer.Panel1.SuspendLayout();
            this.ui_mainSplitContainer.Panel2.SuspendLayout();
            this.ui_mainSplitContainer.SuspendLayout();
            this.ui_topToolStrip.SuspendLayout();
            this.ui_mainContainerPanel.SuspendLayout();
            this.bottomButtonContainer.SuspendLayout();
            this.setDBEntityNullContextMenu.SuspendLayout();
            this.ui_validTimePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_featuresTreeView
            // 
            this.ui_featuresTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featuresTreeView.HideSelection = false;
            this.ui_featuresTreeView.ImageIndex = 0;
            this.ui_featuresTreeView.ImageList = this.featuresIL;
            this.ui_featuresTreeView.Location = new System.Drawing.Point(0, 0);
            this.ui_featuresTreeView.Name = "ui_featuresTreeView";
            this.ui_featuresTreeView.SelectedImageIndex = 0;
            this.ui_featuresTreeView.Size = new System.Drawing.Size(223, 484);
            this.ui_featuresTreeView.TabIndex = 0;
            this.ui_featuresTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.featuresTreeView_AfterSelect);
            this.ui_featuresTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.featuresTreeView_NodeMouseClick);
            this.ui_featuresTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.featuresTreeView_MouseDown);
            // 
            // featuresIL
            // 
            this.featuresIL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("featuresIL.ImageStream")));
            this.featuresIL.TransparentColor = System.Drawing.Color.Transparent;
            this.featuresIL.Images.SetKeyName(0, "new_aixm_feature.png");
            this.featuresIL.Images.SetKeyName(1, "old_aixm_feature.png");
            this.featuresIL.Images.SetKeyName(2, "aixm_object.png");
            this.featuresIL.Images.SetKeyName(3, "new_aixm_feature_list.png");
            this.featuresIL.Images.SetKeyName(4, "old_aixm_feature_list.png");
            this.featuresIL.Images.SetKeyName(5, "aixm_object_list.png");
            this.featuresIL.Images.SetKeyName(6, "shortcut_overlay.png");
            // 
            // ui_mainSplitContainer
            // 
            this.ui_mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ui_mainSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.ui_mainSplitContainer.Name = "ui_mainSplitContainer";
            // 
            // ui_mainSplitContainer.Panel1
            // 
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_featuresTreeView);
            // 
            // ui_mainSplitContainer.Panel2
            // 
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.featureDetailsPanel);
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.ui_topToolStrip);
            this.ui_mainSplitContainer.Size = new System.Drawing.Size(535, 484);
            this.ui_mainSplitContainer.SplitterDistance = 223;
            this.ui_mainSplitContainer.TabIndex = 1;
            // 
            // featureDetailsPanel
            // 
            this.featureDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featureDetailsPanel.Location = new System.Drawing.Point(0, 44);
            this.featureDetailsPanel.Name = "featureDetailsPanel";
            this.featureDetailsPanel.Size = new System.Drawing.Size(308, 440);
            this.featureDetailsPanel.TabIndex = 0;
            // 
            // ui_topToolStrip
            // 
            this.ui_topToolStrip.AutoSize = false;
            this.ui_topToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ui_topToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ui_topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.ui_applyTSB,
            this.ui_backTSB,
            this.ui_gotoFeatureTSB});
            this.ui_topToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ui_topToolStrip.Name = "ui_topToolStrip";
            this.ui_topToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.ui_topToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ui_topToolStrip.Size = new System.Drawing.Size(308, 44);
            this.ui_topToolStrip.TabIndex = 1;
            this.ui_topToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::Aran.Queries.Common.Properties.Resources.close;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripButton2.Size = new System.Drawing.Size(64, 41);
            this.toolStripButton2.Text = "Close";
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButton2.Click += new System.EventHandler(this.cancel2Button_Click);
            // 
            // ui_applyTSB
            // 
            this.ui_applyTSB.Image = global::Aran.Queries.Common.Properties.Resources.apply;
            this.ui_applyTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ui_applyTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_applyTSB.Name = "ui_applyTSB";
            this.ui_applyTSB.Size = new System.Drawing.Size(64, 41);
            this.ui_applyTSB.Text = "Apply";
            this.ui_applyTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ui_applyTSB.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // ui_backTSB
            // 
            this.ui_backTSB.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ui_backTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_backTSB.Image")));
            this.ui_backTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_backTSB.Name = "ui_backTSB";
            this.ui_backTSB.Size = new System.Drawing.Size(60, 41);
            this.ui_backTSB.Text = "Back";
            this.ui_backTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ui_backTSB.Visible = false;
            this.ui_backTSB.Click += new System.EventHandler(this.ui_backTSB_Click);
            // 
            // ui_gotoFeatureTSB
            // 
            this.ui_gotoFeatureTSB.Image = ((System.Drawing.Image)(resources.GetObject("ui_gotoFeatureTSB.Image")));
            this.ui_gotoFeatureTSB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ui_gotoFeatureTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_gotoFeatureTSB.Name = "ui_gotoFeatureTSB";
            this.ui_gotoFeatureTSB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ui_gotoFeatureTSB.Size = new System.Drawing.Size(109, 28);
            this.ui_gotoFeatureTSB.Text = "Go To Feature";
            this.ui_gotoFeatureTSB.Visible = false;
            this.ui_gotoFeatureTSB.Click += new System.EventHandler(this.ui_gotoFeatureTSB_Click);
            // 
            // addNewCompPropsContextMenu
            // 
            this.addNewCompPropsContextMenu.Name = "addNewCompPropsContextMenu";
            this.addNewCompPropsContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // ui_mainContainerPanel
            // 
            this.ui_mainContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainContainerPanel.Controls.Add(this.bottomButtonContainer);
            this.ui_mainContainerPanel.Controls.Add(this.ui_mainSplitContainer);
            this.ui_mainContainerPanel.Controls.Add(this.ui_scrollableFlow);
            this.ui_mainContainerPanel.Location = new System.Drawing.Point(247, 6);
            this.ui_mainContainerPanel.Name = "ui_mainContainerPanel";
            this.ui_mainContainerPanel.Size = new System.Drawing.Size(545, 576);
            this.ui_mainContainerPanel.TabIndex = 4;
            // 
            // bottomButtonContainer
            // 
            this.bottomButtonContainer.Controls.Add(this.btnApplyAll);
            this.bottomButtonContainer.Controls.Add(this.cancelButton);
            this.bottomButtonContainer.Controls.Add(this.okButton);
            this.bottomButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomButtonContainer.Location = new System.Drawing.Point(0, 550);
            this.bottomButtonContainer.Name = "bottomButtonContainer";
            this.bottomButtonContainer.Size = new System.Drawing.Size(545, 26);
            this.bottomButtonContainer.TabIndex = 4;
            // 
            // btnApplyAll
            // 
            this.btnApplyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyAll.Location = new System.Drawing.Point(308, 0);
            this.btnApplyAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnApplyAll.Size = new System.Drawing.Size(74, 23);
            this.btnApplyAll.TabIndex = 2;
            this.btnApplyAll.Text = "Apply all";
            this.btnApplyAll.UseVisualStyleBackColor = true;
            this.btnApplyAll.Visible = false;
            this.btnApplyAll.Click += new System.EventHandler(this.btnApplyAll_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(467, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.OK_Cancel_Button_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(388, 0);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OK_Cancel_Button_Click);
            // 
            // ui_scrollableFlow
            // 
            this.ui_scrollableFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_scrollableFlow.Location = new System.Drawing.Point(3, 493);
            this.ui_scrollableFlow.Margin = new System.Windows.Forms.Padding(4);
            this.ui_scrollableFlow.Name = "ui_scrollableFlow";
            this.ui_scrollableFlow.Size = new System.Drawing.Size(535, 51);
            this.ui_scrollableFlow.TabIndex = 3;
            // 
            // setDBEntityNullContextMenu
            // 
            this.setDBEntityNullContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.openFeatureToolStripMenuItem});
            this.setDBEntityNullContextMenu.Name = "setDBEntityNullContextMenu";
            this.setDBEntityNullContextMenu.Size = new System.Drawing.Size(146, 48);
            this.setDBEntityNullContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.setDBEntityNullContextMenu_Opening);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // openFeatureToolStripMenuItem
            // 
            this.openFeatureToolStripMenuItem.Name = "openFeatureToolStripMenuItem";
            this.openFeatureToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.openFeatureToolStripMenuItem.Text = "Open Feature";
            this.openFeatureToolStripMenuItem.Visible = false;
            this.openFeatureToolStripMenuItem.Click += new System.EventHandler(this.openFeatureToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Valid Time:";
            // 
            // ui_validTimePanel
            // 
            this.ui_validTimePanel.Controls.Add(this.validTimeCancelButton);
            this.ui_validTimePanel.Controls.Add(this.validTimeOkButton);
            this.ui_validTimePanel.Controls.Add(this.label1);
            this.ui_validTimePanel.Controls.Add(this.ui_airacCycleCont);
            this.ui_validTimePanel.Location = new System.Drawing.Point(7, 6);
            this.ui_validTimePanel.Name = "ui_validTimePanel";
            this.ui_validTimePanel.Size = new System.Drawing.Size(234, 103);
            this.ui_validTimePanel.TabIndex = 7;
            // 
            // validTimeCancelButton
            // 
            this.validTimeCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.validTimeCancelButton.Location = new System.Drawing.Point(147, 68);
            this.validTimeCancelButton.Name = "validTimeCancelButton";
            this.validTimeCancelButton.Size = new System.Drawing.Size(75, 23);
            this.validTimeCancelButton.TabIndex = 8;
            this.validTimeCancelButton.Text = "Cancel";
            this.validTimeCancelButton.UseVisualStyleBackColor = true;
            this.validTimeCancelButton.Click += new System.EventHandler(this.ValidTimeCancel_Click);
            // 
            // validTimeOkButton
            // 
            this.validTimeOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.validTimeOkButton.Location = new System.Drawing.Point(68, 68);
            this.validTimeOkButton.Name = "validTimeOkButton";
            this.validTimeOkButton.Size = new System.Drawing.Size(75, 23);
            this.validTimeOkButton.TabIndex = 7;
            this.validTimeOkButton.Text = "OK";
            this.validTimeOkButton.UseVisualStyleBackColor = true;
            this.validTimeOkButton.Click += new System.EventHandler(this.ValidTimeOk_Click);
            // 
            // ui_airacCycleCont
            // 
            this.ui_airacCycleCont.DateTimeFormat = "yyyy - MM - dd  HH:mm";
            this.ui_airacCycleCont.Location = new System.Drawing.Point(11, 27);
            this.ui_airacCycleCont.Margin = new System.Windows.Forms.Padding(4);
            this.ui_airacCycleCont.Name = "ui_airacCycleCont";
            this.ui_airacCycleCont.SelectionMode = Aran.AranEnvironment.AiracSelectionMode.Airac;
            this.ui_airacCycleCont.Size = new System.Drawing.Size(208, 21);
            this.ui_airacCycleCont.TabIndex = 5;
            this.ui_airacCycleCont.Value = new System.DateTime(2014, 10, 16, 0, 0, 0, 0);
            // 
            // FeatureViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 594);
            this.Controls.Add(this.ui_validTimePanel);
            this.Controls.Add(this.ui_mainContainerPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FeatureViewerForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Feature Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FeatureViewerForm_FormClosing);
            this.ui_mainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).EndInit();
            this.ui_mainSplitContainer.ResumeLayout(false);
            this.ui_topToolStrip.ResumeLayout(false);
            this.ui_topToolStrip.PerformLayout();
            this.ui_mainContainerPanel.ResumeLayout(false);
            this.bottomButtonContainer.ResumeLayout(false);
            this.setDBEntityNullContextMenu.ResumeLayout(false);
            this.ui_validTimePanel.ResumeLayout(false);
            this.ui_validTimePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView ui_featuresTreeView;
        private System.Windows.Forms.SplitContainer ui_mainSplitContainer;
        private System.Windows.Forms.Panel featureDetailsPanel;
        private System.Windows.Forms.ImageList featuresIL;
        private System.Windows.Forms.ToolTip toolTip1;
        private Common.ScrollableFlow ui_scrollableFlow;
        private System.Windows.Forms.ContextMenuStrip addNewCompPropsContextMenu;
        private System.Windows.Forms.Panel ui_mainContainerPanel;
        private System.Windows.Forms.Panel bottomButtonContainer;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ToolStrip ui_topToolStrip;
        private System.Windows.Forms.ToolStripButton ui_applyTSB;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ContextMenuStrip setDBEntityNullContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ui_backTSB;
        private System.Windows.Forms.ToolStripMenuItem openFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ui_gotoFeatureTSB;
        private Controls.Airac.AiracCycleControl ui_airacCycleCont;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel ui_validTimePanel;
        private System.Windows.Forms.Button validTimeCancelButton;
        private System.Windows.Forms.Button validTimeOkButton;
        private System.Windows.Forms.Button btnApplyAll;
    }
}