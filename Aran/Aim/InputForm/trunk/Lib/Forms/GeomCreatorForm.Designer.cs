namespace Aran.Aim.InputFormLib
{
    partial class GeomCreatorForm
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
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ui_groupDGV = new System.Windows.Forms.DataGridView();
            this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.ui_geomTypeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_changeGeomTypeTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_toolStrip = new System.Windows.Forms.ToolStrip();
            this.ui_newPolygonTSMI = new System.Windows.Forms.ToolStripButton();
            this.ui_newPolylineTSMI = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ui_removeGroupTSMI = new System.Windows.Forms.ToolStripButton();
            this.ui_removePointTSMI = new System.Windows.Forms.ToolStripButton();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_pointCountLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).BeginInit();
            this.ui_mainSplitContainer.Panel1.SuspendLayout();
            this.ui_mainSplitContainer.Panel2.SuspendLayout();
            this.ui_mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_groupDGV)).BeginInit();
            this.ui_geomTypeContextMenu.SuspendLayout();
            this.ui_toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 279);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 13);
            label1.TabIndex = 5;
            label1.Text = "Point Count:";
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colY,
            this.ui_colX,
            this.ui_colZ});
            this.ui_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dgv.Location = new System.Drawing.Point(0, 0);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ui_dgv.Size = new System.Drawing.Size(337, 236);
            this.ui_dgv.TabIndex = 1;
            this.ui_dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            this.ui_dgv.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DGV_RowsAdded);
            this.ui_dgv.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DGV_RowsRemoved);
            // 
            // ui_colY
            // 
            this.ui_colY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colY.HeaderText = "Latitude";
            this.ui_colY.Name = "ui_colY";
            // 
            // ui_colX
            // 
            this.ui_colX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colX.HeaderText = "Longitude";
            this.ui_colX.Name = "ui_colX";
            // 
            // ui_colZ
            // 
            this.ui_colZ.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colZ.HeaderText = "Z";
            this.ui_colZ.Name = "ui_colZ";
            // 
            // ui_mainSplitContainer
            // 
            this.ui_mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainSplitContainer.Location = new System.Drawing.Point(2, 33);
            this.ui_mainSplitContainer.Name = "ui_mainSplitContainer";
            // 
            // ui_mainSplitContainer.Panel1
            // 
            this.ui_mainSplitContainer.Panel1.Controls.Add(this.ui_groupDGV);
            // 
            // ui_mainSplitContainer.Panel2
            // 
            this.ui_mainSplitContainer.Panel2.Controls.Add(this.ui_dgv);
            this.ui_mainSplitContainer.Size = new System.Drawing.Size(496, 236);
            this.ui_mainSplitContainer.SplitterDistance = 155;
            this.ui_mainSplitContainer.TabIndex = 2;
            // 
            // ui_groupDGV
            // 
            this.ui_groupDGV.AllowUserToAddRows = false;
            this.ui_groupDGV.AllowUserToDeleteRows = false;
            this.ui_groupDGV.AllowUserToResizeRows = false;
            this.ui_groupDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_groupDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_groupDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName,
            this.ui_colImage});
            this.ui_groupDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_groupDGV.Location = new System.Drawing.Point(0, 0);
            this.ui_groupDGV.MultiSelect = false;
            this.ui_groupDGV.Name = "ui_groupDGV";
            this.ui_groupDGV.RowHeadersVisible = false;
            this.ui_groupDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ui_groupDGV.Size = new System.Drawing.Size(155, 236);
            this.ui_groupDGV.TabIndex = 2;
            this.ui_groupDGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GroupDGV_CellValueChanged);
            this.ui_groupDGV.CurrentCellChanged += new System.EventHandler(this.GroupDGV_CurrentCellChanged);
            // 
            // ui_colName
            // 
            this.ui_colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colName.HeaderText = "Name";
            this.ui_colName.Name = "ui_colName";
            // 
            // ui_colImage
            // 
            this.ui_colImage.ContextMenuStrip = this.ui_geomTypeContextMenu;
            this.ui_colImage.HeaderText = "Type";
            this.ui_colImage.Name = "ui_colImage";
            this.ui_colImage.ReadOnly = true;
            this.ui_colImage.Width = 60;
            // 
            // ui_geomTypeContextMenu
            // 
            this.ui_geomTypeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_changeGeomTypeTSMI});
            this.ui_geomTypeContextMenu.Name = "ui_geomTypeContextMenu";
            this.ui_geomTypeContextMenu.Size = new System.Drawing.Size(200, 26);
            // 
            // ui_changeGeomTypeTSMI
            // 
            this.ui_changeGeomTypeTSMI.Name = "ui_changeGeomTypeTSMI";
            this.ui_changeGeomTypeTSMI.Size = new System.Drawing.Size(199, 22);
            this.ui_changeGeomTypeTSMI.Text = "Change Geometry Type";
            this.ui_changeGeomTypeTSMI.Click += new System.EventHandler(this.ChangeGeomType_Click);
            // 
            // ui_toolStrip
            // 
            this.ui_toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ui_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_newPolygonTSMI,
            this.ui_newPolylineTSMI,
            this.toolStripSeparator1,
            this.ui_removeGroupTSMI,
            this.ui_removePointTSMI});
            this.ui_toolStrip.Location = new System.Drawing.Point(0, 0);
            this.ui_toolStrip.Name = "ui_toolStrip";
            this.ui_toolStrip.Size = new System.Drawing.Size(503, 25);
            this.ui_toolStrip.TabIndex = 3;
            // 
            // ui_newPolygonTSMI
            // 
            this.ui_newPolygonTSMI.Image = global::Aran.Aim.InputFormLib.Properties.Resources.polygon_16;
            this.ui_newPolygonTSMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_newPolygonTSMI.Name = "ui_newPolygonTSMI";
            this.ui_newPolygonTSMI.Size = new System.Drawing.Size(98, 22);
            this.ui_newPolygonTSMI.Text = "New Polygon";
            this.ui_newPolygonTSMI.Click += new System.EventHandler(this.NewPolygon_Click);
            // 
            // ui_newPolylineTSMI
            // 
            this.ui_newPolylineTSMI.Image = global::Aran.Aim.InputFormLib.Properties.Resources.polyline_16;
            this.ui_newPolylineTSMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_newPolylineTSMI.Name = "ui_newPolylineTSMI";
            this.ui_newPolylineTSMI.Size = new System.Drawing.Size(96, 22);
            this.ui_newPolylineTSMI.Text = "New Polyline";
            this.ui_newPolylineTSMI.Click += new System.EventHandler(this.NewPolyline_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ui_removeGroupTSMI
            // 
            this.ui_removeGroupTSMI.Enabled = false;
            this.ui_removeGroupTSMI.Image = global::Aran.Aim.InputFormLib.Properties.Resources.cancel_24;
            this.ui_removeGroupTSMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_removeGroupTSMI.Name = "ui_removeGroupTSMI";
            this.ui_removeGroupTSMI.Size = new System.Drawing.Size(106, 22);
            this.ui_removeGroupTSMI.Text = "Remove Group";
            // 
            // ui_removePointTSMI
            // 
            this.ui_removePointTSMI.Enabled = false;
            this.ui_removePointTSMI.Image = global::Aran.Aim.InputFormLib.Properties.Resources.cancel_24;
            this.ui_removePointTSMI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ui_removePointTSMI.Name = "ui_removePointTSMI";
            this.ui_removePointTSMI.Size = new System.Drawing.Size(101, 22);
            this.ui_removePointTSMI.Text = "Remove Point";
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(423, 275);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 4;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_pointCountLabel
            // 
            this.ui_pointCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_pointCountLabel.AutoSize = true;
            this.ui_pointCountLabel.Location = new System.Drawing.Point(74, 278);
            this.ui_pointCountLabel.Name = "ui_pointCountLabel";
            this.ui_pointCountLabel.Size = new System.Drawing.Size(13, 13);
            this.ui_pointCountLabel.TabIndex = 6;
            this.ui_pointCountLabel.Text = "0";
            // 
            // GeomCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 303);
            this.Controls.Add(this.ui_pointCountLabel);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(this.ui_toolStrip);
            this.Controls.Add(this.ui_mainSplitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GeomCreatorForm";
            this.Text = "Geometry Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeomCreatorForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ui_mainSplitContainer.Panel1.ResumeLayout(false);
            this.ui_mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_mainSplitContainer)).EndInit();
            this.ui_mainSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_groupDGV)).EndInit();
            this.ui_geomTypeContextMenu.ResumeLayout(false);
            this.ui_toolStrip.ResumeLayout(false);
            this.ui_toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colZ;
        private System.Windows.Forms.SplitContainer ui_mainSplitContainer;
        private System.Windows.Forms.DataGridView ui_groupDGV;
        private System.Windows.Forms.ToolStrip ui_toolStrip;
        private System.Windows.Forms.ToolStripButton ui_newPolygonTSMI;
        private System.Windows.Forms.ToolStripButton ui_newPolylineTSMI;
        private System.Windows.Forms.ToolStripButton ui_removeGroupTSMI;
        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ui_removePointTSMI;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
        private System.Windows.Forms.DataGridViewImageColumn ui_colImage;
        private System.Windows.Forms.ContextMenuStrip ui_geomTypeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ui_changeGeomTypeTSMI;
        private System.Windows.Forms.Label ui_pointCountLabel;
    }
}