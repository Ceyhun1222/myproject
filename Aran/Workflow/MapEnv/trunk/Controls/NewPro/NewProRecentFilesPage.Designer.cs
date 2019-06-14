namespace MapEnv
{
    partial class NewProRecentFilesPage
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
            if (disposing && (components != null)) {
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
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearRecentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_thumbPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_thumbPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToResizeColumns = false;
            this.ui_dgv.AllowUserToResizeRows = false;
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.ui_dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.ui_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dgv.Location = new System.Drawing.Point(0, 0);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_dgv.Size = new System.Drawing.Size(462, 307);
            this.ui_dgv.TabIndex = 0;
            this.ui_dgv.VirtualMode = true;
            this.ui_dgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.DGV_CellValueNeeded);
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "#";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 30;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "File Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Directory";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearRecentFilesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(167, 26);
            // 
            // clearRecentFilesToolStripMenuItem
            // 
            this.clearRecentFilesToolStripMenuItem.Name = "clearRecentFilesToolStripMenuItem";
            this.clearRecentFilesToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.clearRecentFilesToolStripMenuItem.Text = "Clear Recent Files";
            this.clearRecentFilesToolStripMenuItem.Click += new System.EventHandler(this.ClearRecentFilesContextMenuItem_Click);
            // 
            // ui_thumbPictureBox
            // 
            this.ui_thumbPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.ui_thumbPictureBox.Location = new System.Drawing.Point(462, 0);
            this.ui_thumbPictureBox.Name = "ui_thumbPictureBox";
            this.ui_thumbPictureBox.Size = new System.Drawing.Size(208, 307);
            this.ui_thumbPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ui_thumbPictureBox.TabIndex = 1;
            this.ui_thumbPictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(468, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 91);
            this.label1.TabIndex = 2;
            // 
            // NewProRecentFilesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ui_dgv);
            this.Controls.Add(this.ui_thumbPictureBox);
            this.Name = "NewProRecentFilesPage";
            this.Size = new System.Drawing.Size(670, 307);
            this.Load += new System.EventHandler(this.NewProRecentFilesPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_thumbPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.PictureBox ui_thumbPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearRecentFilesToolStripMenuItem;
    }
}
