

namespace ARENA.TOCLayerView
{
    partial class TOCLayerFilter
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
            if (disposing && (components != null))
            {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TOCLayerFilter));
            this.contextMenuDummy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showOnMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshTreeViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortTreeViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.procTABToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FeatureTreeView = new System.Windows.Forms.TreeView();
            this.TreeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuDummy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuDummy
            // 
            this.contextMenuDummy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnMapToolStripMenuItem,
            this.deleteSelectedObjectToolStripMenuItem,
            this.refreshTreeViewToolStripMenuItem,
            this.sortTreeViewToolStripMenuItem,
            this.cloneObjectToolStripMenuItem,
            this.clearSelectionToolStripMenuItem,
            this.procTABToolStripMenuItem});
            this.contextMenuDummy.Name = "contextMenuDummy";
            this.contextMenuDummy.Size = new System.Drawing.Size(182, 180);
            this.contextMenuDummy.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuDummy_Opening);
            // 
            // showOnMapToolStripMenuItem
            // 
            this.showOnMapToolStripMenuItem.Name = "showOnMapToolStripMenuItem";
            this.showOnMapToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showOnMapToolStripMenuItem.Text = "Show on map";
            this.showOnMapToolStripMenuItem.Click += new System.EventHandler(this.showOnMapToolStripMenuItem_Click);
            // 
            // deleteSelectedObjectToolStripMenuItem
            // 
            this.deleteSelectedObjectToolStripMenuItem.Enabled = false;
            this.deleteSelectedObjectToolStripMenuItem.Name = "deleteSelectedObjectToolStripMenuItem";
            this.deleteSelectedObjectToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteSelectedObjectToolStripMenuItem.Text = "Delete selected object";
            this.deleteSelectedObjectToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedObjectToolStripMenuItem_Click);
            // 
            // refreshTreeViewToolStripMenuItem
            // 
            this.refreshTreeViewToolStripMenuItem.Name = "refreshTreeViewToolStripMenuItem";
            this.refreshTreeViewToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.refreshTreeViewToolStripMenuItem.Text = "Refresh TreeView";
            this.refreshTreeViewToolStripMenuItem.Click += new System.EventHandler(this.refreshTreeViewToolStripMenuItem_Click);
            // 
            // sortTreeViewToolStripMenuItem
            // 
            this.sortTreeViewToolStripMenuItem.Name = "sortTreeViewToolStripMenuItem";
            this.sortTreeViewToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.sortTreeViewToolStripMenuItem.Text = "Sort TreeView";
            this.sortTreeViewToolStripMenuItem.Click += new System.EventHandler(this.sortTreeViewToolStripMenuItem_Click);
            // 
            // cloneObjectToolStripMenuItem
            // 
            this.cloneObjectToolStripMenuItem.Enabled = false;
            this.cloneObjectToolStripMenuItem.Name = "cloneObjectToolStripMenuItem";
            this.cloneObjectToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.cloneObjectToolStripMenuItem.Text = "Clone object";
            this.cloneObjectToolStripMenuItem.Click += new System.EventHandler(this.cloneObjectToolStripMenuItem_Click);
            // 
            // clearSelectionToolStripMenuItem
            // 
            this.clearSelectionToolStripMenuItem.Name = "clearSelectionToolStripMenuItem";
            this.clearSelectionToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.clearSelectionToolStripMenuItem.Text = "Clear selection";
            this.clearSelectionToolStripMenuItem.Click += new System.EventHandler(this.clearSelectionToolStripMenuItem_Click);
            // 
            // procTABToolStripMenuItem
            // 
            this.procTABToolStripMenuItem.Name = "procTABToolStripMenuItem";
            this.procTABToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.procTABToolStripMenuItem.Text = "Tabulation data";
            this.procTABToolStripMenuItem.Click += new System.EventHandler(this.procTABToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FeatureTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Panel2.Controls.Add(this.linkLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(224, 300);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 3;
            // 
            // FeatureTreeView
            // 
            this.FeatureTreeView.ContextMenuStrip = this.contextMenuDummy;
            this.FeatureTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FeatureTreeView.HideSelection = false;
            this.FeatureTreeView.ImageIndex = 0;
            this.FeatureTreeView.ImageList = this.TreeViewImageList;
            this.FeatureTreeView.Location = new System.Drawing.Point(0, 0);
            this.FeatureTreeView.Name = "FeatureTreeView";
            this.FeatureTreeView.SelectedImageIndex = 0;
            this.FeatureTreeView.Size = new System.Drawing.Size(224, 168);
            this.FeatureTreeView.TabIndex = 1;
            this.FeatureTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FeatureTreeView_AfterSelect);
            this.FeatureTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FeatureTreeView_KeyDown);
            this.FeatureTreeView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FeatureTreeView_KeyPress);
            // 
            // TreeViewImageList
            // 
            this.TreeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeViewImageList.ImageStream")));
            this.TreeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.TreeViewImageList.Images.SetKeyName(0, "GenericButtonGreen16.png");
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(224, 53);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            this.propertyGrid1.SelectedObjectsChanged += new System.EventHandler(this.propertyGrid1_SelectedObjectsChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.linkLabel1.Location = new System.Drawing.Point(0, 53);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(224, 17);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Quick search";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(224, 58);
            this.panel2.TabIndex = 5;
            this.panel2.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(190, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Identifier = ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search In";
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(62, 30);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(123, 21);
            this.comboBox2.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "",
            "Airport",
            "Navaid",
            "WayPoint",
            "VerticalStructure",
            "Airspaces",
            "Enroutes",
            "IAP",
            "SID",
            "STAR"});
            this.comboBox1.Location = new System.Drawing.Point(62, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(123, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // TOCLayerFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TOCLayerFilter";
            this.Size = new System.Drawing.Size(224, 300);
            this.contextMenuDummy.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuDummy;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView FeatureTreeView;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem showOnMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedObjectToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ImageList TreeViewImageList;
        private System.Windows.Forms.ToolStripMenuItem refreshTreeViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortTreeViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem procTABToolStripMenuItem;
    }
}
