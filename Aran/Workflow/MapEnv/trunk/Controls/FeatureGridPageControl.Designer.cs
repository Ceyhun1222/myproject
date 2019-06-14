namespace MapEnv
{
    partial class FeatureGridPageControl
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
			this.ui_dgv = new System.Windows.Forms.DataGridView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.ui_selectAllChB = new System.Windows.Forms.CheckBox();
			this.ui_featCountLab = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ui_dgv
			// 
			this.ui_dgv.AllowUserToAddRows = false;
			this.ui_dgv.AllowUserToDeleteRows = false;
			this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ui_dgv.Location = new System.Drawing.Point(3, 34);
			this.ui_dgv.Name = "ui_dgv";
			this.ui_dgv.ReadOnly = true;
			this.ui_dgv.RowHeadersWidth = 25;
			this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ui_dgv.Size = new System.Drawing.Size(649, 293);
			this.ui_dgv.TabIndex = 1;
			this.ui_dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
			this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.DGV_CurrentCellChanged);
			this.ui_dgv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_MouseUp);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStrip1.Size = new System.Drawing.Size(655, 31);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image = global::MapEnv.Properties.Resources.preview_24;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(76, 28);
			this.toolStripButton1.Text = "Preview";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// ui_selectAllChB
			// 
			this.ui_selectAllChB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_selectAllChB.Appearance = System.Windows.Forms.Appearance.Button;
			this.ui_selectAllChB.AutoSize = true;
			this.ui_selectAllChB.Location = new System.Drawing.Point(586, 3);
			this.ui_selectAllChB.Name = "ui_selectAllChB";
			this.ui_selectAllChB.Size = new System.Drawing.Size(61, 23);
			this.ui_selectAllChB.TabIndex = 3;
			this.ui_selectAllChB.Text = "Select All";
			this.ui_selectAllChB.UseVisualStyleBackColor = true;
			this.ui_selectAllChB.Visible = false;
			this.ui_selectAllChB.CheckedChanged += new System.EventHandler(this.SelectAllFeature_CheckedChanged);
			// 
			// ui_featCountLab
			// 
			this.ui_featCountLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ui_featCountLab.AutoSize = true;
			this.ui_featCountLab.Location = new System.Drawing.Point(6, 339);
			this.ui_featCountLab.Name = "ui_featCountLab";
			this.ui_featCountLab.Size = new System.Drawing.Size(86, 13);
			this.ui_featCountLab.TabIndex = 4;
			this.ui_featCountLab.Text = "Feature Count: 0";
			// 
			// FeatureGridPageControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ui_featCountLab);
			this.Controls.Add(this.ui_selectAllChB);
			this.Controls.Add(this.ui_dgv);
			this.Controls.Add(this.toolStrip1);
			this.Name = "FeatureGridPageControl";
			this.Size = new System.Drawing.Size(655, 358);
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
		private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.CheckBox ui_selectAllChB;
		private System.Windows.Forms.Label ui_featCountLab;
    }
}
