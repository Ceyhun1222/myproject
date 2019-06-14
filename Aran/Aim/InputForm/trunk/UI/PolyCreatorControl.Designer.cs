namespace Aran.Aim.InputForm
{
    partial class PolyCreatorControl
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
            System.Windows.Forms.LinkLabel linkLabel1;
            System.Windows.Forms.LinkLabel linkLabel2;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Button ui_importButton;
            System.Windows.Forms.ToolStripMenuItem fromTextFileToolStripMenuItem;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PolyCreatorControl));
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.ui_pathLabel = new System.Windows.Forms.Label();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_partRingCB = new System.Windows.Forms.ComboBox();
            this.ui_backButton = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ui_okButton = new System.Windows.Forms.Button();
            this.ui_partOrRingGrBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ui_byClickChB = new System.Windows.Forms.CheckBox();
            this.ui_importContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_fromDeltaProTSMI = new System.Windows.Forms.ToolStripMenuItem();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            linkLabel2 = new System.Windows.Forms.LinkLabel();
            groupBox1 = new System.Windows.Forms.GroupBox();
            ui_importButton = new System.Windows.Forms.Button();
            fromTextFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.ui_partOrRingGrBox.SuspendLayout();
            this.ui_importContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new System.Drawing.Point(9, 20);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(51, 13);
            linkLabel1.TabIndex = 7;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Add New";
            linkLabel1.Click += new System.EventHandler(this.AddNewPartRing_Click);
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new System.Drawing.Point(8, 40);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new System.Drawing.Size(84, 13);
            linkLabel2.TabIndex = 8;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "Remove Current";
            linkLabel2.Click += new System.EventHandler(this.RemoveCurrPartRing_Click);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.linkLabel4);
            groupBox1.Controls.Add(this.linkLabel3);
            groupBox1.Location = new System.Drawing.Point(71, 87);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(122, 44);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Selected Coordinate";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(51, 19);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(47, 13);
            this.linkLabel4.TabIndex = 1;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Remove";
            this.linkLabel4.Click += new System.EventHandler(this.RemoveSelCoord_Click);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(20, 19);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(25, 13);
            this.linkLabel3.TabIndex = 0;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Edit";
            this.linkLabel3.Click += new System.EventHandler(this.EditCoord_Click);
            // 
            // ui_importButton
            // 
            ui_importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            ui_importButton.Location = new System.Drawing.Point(3, 276);
            ui_importButton.Name = "ui_importButton";
            ui_importButton.Size = new System.Drawing.Size(93, 26);
            ui_importButton.TabIndex = 11;
            ui_importButton.Text = "Import From";
            ui_importButton.UseVisualStyleBackColor = true;
            ui_importButton.Click += new System.EventHandler(this.ImportFrom_Click);
            // 
            // fromTextFileToolStripMenuItem
            // 
            fromTextFileToolStripMenuItem.Name = "fromTextFileToolStripMenuItem";
            fromTextFileToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            fromTextFileToolStripMenuItem.Text = "Text File";
            // 
            // ui_pathLabel
            // 
            this.ui_pathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_pathLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ui_pathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_pathLabel.Location = new System.Drawing.Point(-2, 229);
            this.ui_pathLabel.Name = "ui_pathLabel";
            this.ui_pathLabel.Padding = new System.Windows.Forms.Padding(4);
            this.ui_pathLabel.Size = new System.Drawing.Size(503, 43);
            this.ui_pathLabel.TabIndex = 1;
            this.ui_pathLabel.Text = "Item1 -> Item2";
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colY,
            this.ui_colX});
            this.ui_dgv.Location = new System.Drawing.Point(198, 4);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_dgv.Size = new System.Drawing.Size(298, 218);
            this.ui_dgv.TabIndex = 2;
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            // 
            // ui_colY
            // 
            this.ui_colY.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colY.HeaderText = "Latitude";
            this.ui_colY.Name = "ui_colY";
            this.ui_colY.ReadOnly = true;
            // 
            // ui_colX
            // 
            this.ui_colX.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colX.HeaderText = "Longitude";
            this.ui_colX.Name = "ui_colX";
            this.ui_colX.ReadOnly = true;
            // 
            // ui_partRingCB
            // 
            this.ui_partRingCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_partRingCB.FormattingEnabled = true;
            this.ui_partRingCB.Location = new System.Drawing.Point(97, 31);
            this.ui_partRingCB.Name = "ui_partRingCB";
            this.ui_partRingCB.Size = new System.Drawing.Size(77, 21);
            this.ui_partRingCB.TabIndex = 5;
            this.ui_partRingCB.SelectedIndexChanged += new System.EventHandler(this.PartRing_SelectedIndexChanged);
            // 
            // ui_backButton
            // 
            this.ui_backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_backButton.ImageIndex = 0;
            this.ui_backButton.ImageList = this.imageList1;
            this.ui_backButton.Location = new System.Drawing.Point(336, 276);
            this.ui_backButton.Name = "ui_backButton";
            this.ui_backButton.Size = new System.Drawing.Size(77, 26);
            this.ui_backButton.TabIndex = 6;
            this.ui_backButton.Text = "Back";
            this.ui_backButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_backButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ui_backButton.UseVisualStyleBackColor = true;
            this.ui_backButton.Click += new System.EventHandler(this.Back_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "back_24.png");
            this.imageList1.Images.SetKeyName(1, "dialog_apply.png");
            this.imageList1.Images.SetKeyName(2, "edit_icon_24x24.png");
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.ImageIndex = 1;
            this.ui_okButton.ImageList = this.imageList1;
            this.ui_okButton.Location = new System.Drawing.Point(419, 276);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(77, 26);
            this.ui_okButton.TabIndex = 7;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_okButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_partOrRingGrBox
            // 
            this.ui_partOrRingGrBox.Controls.Add(linkLabel2);
            this.ui_partOrRingGrBox.Controls.Add(linkLabel1);
            this.ui_partOrRingGrBox.Controls.Add(this.label1);
            this.ui_partOrRingGrBox.Controls.Add(this.ui_partRingCB);
            this.ui_partOrRingGrBox.Location = new System.Drawing.Point(6, 8);
            this.ui_partOrRingGrBox.Name = "ui_partOrRingGrBox";
            this.ui_partOrRingGrBox.Size = new System.Drawing.Size(187, 68);
            this.ui_partOrRingGrBox.TabIndex = 8;
            this.ui_partOrRingGrBox.TabStop = false;
            this.ui_partOrRingGrBox.Text = "<Part Or Ring>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Current Index:";
            // 
            // ui_byClickChB
            // 
            this.ui_byClickChB.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_byClickChB.FlatAppearance.CheckedBackColor = System.Drawing.Color.SkyBlue;
            this.ui_byClickChB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_byClickChB.ImageIndex = 2;
            this.ui_byClickChB.ImageList = this.imageList1;
            this.ui_byClickChB.Location = new System.Drawing.Point(71, 148);
            this.ui_byClickChB.Name = "ui_byClickChB";
            this.ui_byClickChB.Size = new System.Drawing.Size(122, 30);
            this.ui_byClickChB.TabIndex = 10;
            this.ui_byClickChB.Text = "By Click";
            this.ui_byClickChB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_byClickChB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ui_byClickChB.UseVisualStyleBackColor = true;
            this.ui_byClickChB.CheckedChanged += new System.EventHandler(this.ByClick_CheckedChanged);
            // 
            // ui_importContextMenu
            // 
            this.ui_importContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fromTextFileToolStripMenuItem,
            this.ui_fromDeltaProTSMI});
            this.ui_importContextMenu.Name = "ui_importContextMenu";
            this.ui_importContextMenu.Size = new System.Drawing.Size(171, 70);
            // 
            // ui_fromDeltaProTSMI
            // 
            this.ui_fromDeltaProTSMI.Name = "ui_fromDeltaProTSMI";
            this.ui_fromDeltaProTSMI.Size = new System.Drawing.Size(170, 22);
            this.ui_fromDeltaProTSMI.Text = "DELTA Project File";
            this.ui_fromDeltaProTSMI.Visible = false;
            this.ui_fromDeltaProTSMI.Click += new System.EventHandler(this.FromDeltaPro_Click);
            // 
            // PolyCreatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(ui_importButton);
            this.Controls.Add(this.ui_byClickChB);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.ui_partOrRingGrBox);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_backButton);
            this.Controls.Add(this.ui_dgv);
            this.Controls.Add(this.ui_pathLabel);
            this.Name = "PolyCreatorControl";
            this.Size = new System.Drawing.Size(500, 308);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ui_partOrRingGrBox.ResumeLayout(false);
            this.ui_partOrRingGrBox.PerformLayout();
            this.ui_importContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ui_pathLabel;
		private System.Windows.Forms.DataGridView ui_dgv;
		private System.Windows.Forms.ComboBox ui_partRingCB;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colY;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colX;
		private System.Windows.Forms.Button ui_backButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Button ui_okButton;
		private System.Windows.Forms.GroupBox ui_partOrRingGrBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel4;
		private System.Windows.Forms.LinkLabel linkLabel3;
		private System.Windows.Forms.CheckBox ui_byClickChB;
		private System.Windows.Forms.ContextMenuStrip ui_importContextMenu;
		private System.Windows.Forms.ToolStripMenuItem ui_fromDeltaProTSMI;
    }
}
