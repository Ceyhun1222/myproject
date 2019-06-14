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
			this.featuresTreeView = new System.Windows.Forms.TreeView();
			this.featuresIL = new System.Windows.Forms.ImageList(this.components);
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.featureDetailsPanel = new System.Windows.Forms.Panel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.ui_closeTSB = new System.Windows.Forms.ToolStripButton();
			this.ui_applyTSB = new System.Windows.Forms.ToolStripButton();
			this.ui_backTSB = new System.Windows.Forms.ToolStripButton();
			this.ui_gotoFeatureTSB = new System.Windows.Forms.ToolStripButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.addNewCompPropsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mainContainerPanel = new System.Windows.Forms.Panel();
			this.bottomButtonContainer = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.setDBEntityNullContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.scrollableFlow1 = new Aran.Queries.Common.ScrollableFlow();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.mainContainerPanel.SuspendLayout();
			this.bottomButtonContainer.SuspendLayout();
			this.setDBEntityNullContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// featuresTreeView
			// 
			this.featuresTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.featuresTreeView.HideSelection = false;
			this.featuresTreeView.ImageIndex = 0;
			this.featuresTreeView.ImageList = this.featuresIL;
			this.featuresTreeView.Location = new System.Drawing.Point(0, 0);
			this.featuresTreeView.Name = "featuresTreeView";
			this.featuresTreeView.SelectedImageIndex = 0;
			this.featuresTreeView.Size = new System.Drawing.Size(223, 404);
			this.featuresTreeView.TabIndex = 0;
			this.featuresTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.featuresTreeView_AfterSelect);
			this.featuresTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.featuresTreeView_NodeMouseClick);
			this.featuresTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.featuresTreeView_MouseDown);
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
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.featuresTreeView);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.featureDetailsPanel);
			this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
			this.splitContainer1.Size = new System.Drawing.Size(789, 404);
			this.splitContainer1.SplitterDistance = 223;
			this.splitContainer1.TabIndex = 1;
			// 
			// featureDetailsPanel
			// 
			this.featureDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.featureDetailsPanel.Location = new System.Drawing.Point(0, 44);
			this.featureDetailsPanel.Name = "featureDetailsPanel";
			this.featureDetailsPanel.Size = new System.Drawing.Size(562, 360);
			this.featureDetailsPanel.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_closeTSB,
            this.ui_applyTSB,
            this.ui_backTSB,
            this.ui_gotoFeatureTSB});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.toolStrip1.Size = new System.Drawing.Size(562, 44);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// ui_closeTSB
			// 
			this.ui_closeTSB.Image = global::Aran.Queries.Common.Properties.Resources.close;
			this.ui_closeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ui_closeTSB.Name = "ui_closeTSB";
			this.ui_closeTSB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.ui_closeTSB.Size = new System.Drawing.Size(61, 41);
			this.ui_closeTSB.Text = "Close";
			this.ui_closeTSB.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.ui_closeTSB.Visible = false;
			this.ui_closeTSB.Click += new System.EventHandler(this.cancel2Button_Click);
			// 
			// ui_applyTSB
			// 
			this.ui_applyTSB.Image = global::Aran.Queries.Common.Properties.Resources.apply;
			this.ui_applyTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ui_applyTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ui_applyTSB.Name = "ui_applyTSB";
			this.ui_applyTSB.Size = new System.Drawing.Size(60, 41);
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
			this.ui_backTSB.Size = new System.Drawing.Size(57, 41);
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
			this.ui_gotoFeatureTSB.Size = new System.Drawing.Size(104, 41);
			this.ui_gotoFeatureTSB.Text = "Go To Feature";
			this.ui_gotoFeatureTSB.Visible = false;
			this.ui_gotoFeatureTSB.Click += new System.EventHandler(this.ui_gotoFeatureTSB_Click);
			// 
			// addNewCompPropsContextMenu
			// 
			this.addNewCompPropsContextMenu.Name = "addNewCompPropsContextMenu";
			this.addNewCompPropsContextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// mainContainerPanel
			// 
			this.mainContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainContainerPanel.Controls.Add(this.bottomButtonContainer);
			this.mainContainerPanel.Controls.Add(this.splitContainer1);
			this.mainContainerPanel.Controls.Add(this.scrollableFlow1);
			this.mainContainerPanel.Location = new System.Drawing.Point(0, 1);
			this.mainContainerPanel.Name = "mainContainerPanel";
			this.mainContainerPanel.Size = new System.Drawing.Size(799, 496);
			this.mainContainerPanel.TabIndex = 4;
			// 
			// bottomButtonContainer
			// 
			this.bottomButtonContainer.Controls.Add(this.cancelButton);
			this.bottomButtonContainer.Controls.Add(this.okButton);
			this.bottomButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomButtonContainer.Location = new System.Drawing.Point(0, 470);
			this.bottomButtonContainer.Name = "bottomButtonContainer";
			this.bottomButtonContainer.Size = new System.Drawing.Size(799, 26);
			this.bottomButtonContainer.TabIndex = 4;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(721, 0);
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
			this.okButton.Location = new System.Drawing.Point(642, 0);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.OK_Cancel_Button_Click);
			// 
			// setDBEntityNullContextMenu
			// 
			this.setDBEntityNullContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.openFeatureToolStripMenuItem});
			this.setDBEntityNullContextMenu.Name = "setDBEntityNullContextMenu";
			this.setDBEntityNullContextMenu.Size = new System.Drawing.Size(142, 48);
			this.setDBEntityNullContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.setDBEntityNullContextMenu_Opening);
			// 
			// clearToolStripMenuItem
			// 
			this.clearToolStripMenuItem.Image = global::Aran.Queries.Common.Properties.Resources.edit_clear;
			this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
			this.clearToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.clearToolStripMenuItem.Text = "Clear";
			this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
			// 
			// openFeatureToolStripMenuItem
			// 
			this.openFeatureToolStripMenuItem.Name = "openFeatureToolStripMenuItem";
			this.openFeatureToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.openFeatureToolStripMenuItem.Text = "Open Feature";
			this.openFeatureToolStripMenuItem.Visible = false;
			this.openFeatureToolStripMenuItem.Click += new System.EventHandler(this.openFeatureToolStripMenuItem_Click);
			// 
			// scrollableFlow1
			// 
			this.scrollableFlow1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.scrollableFlow1.Location = new System.Drawing.Point(3, 413);
			this.scrollableFlow1.Name = "scrollableFlow1";
			this.scrollableFlow1.Size = new System.Drawing.Size(789, 51);
			this.scrollableFlow1.TabIndex = 3;
			// 
			// FeatureViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 498);
			this.Controls.Add(this.mainContainerPanel);
			this.Name = "FeatureViewerForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Feature Viewer";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.mainContainerPanel.ResumeLayout(false);
			this.bottomButtonContainer.ResumeLayout(false);
			this.setDBEntityNullContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView featuresTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel featureDetailsPanel;
        private System.Windows.Forms.ImageList featuresIL;
        private System.Windows.Forms.ToolTip toolTip1;
        private Common.ScrollableFlow scrollableFlow1;
        private System.Windows.Forms.ContextMenuStrip addNewCompPropsContextMenu;
        private System.Windows.Forms.Panel mainContainerPanel;
        private System.Windows.Forms.Panel bottomButtonContainer;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ui_applyTSB;
        private System.Windows.Forms.ToolStripButton ui_closeTSB;
        private System.Windows.Forms.ContextMenuStrip setDBEntityNullContextMenu;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ui_backTSB;
        private System.Windows.Forms.ToolStripMenuItem openFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton ui_gotoFeatureTSB;
    }
}