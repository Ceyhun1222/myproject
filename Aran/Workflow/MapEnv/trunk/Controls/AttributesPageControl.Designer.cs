namespace MapEnv
{
    partial class AttributesPageControl
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
            this.ui_geomTypeLabel = new System.Windows.Forms.Label();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_contextMSdgv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_zoomToTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ui_rowCountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.ui_contextMSdgv.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_geomTypeLabel
            // 
            this.ui_geomTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_geomTypeLabel.AutoSize = true;
            this.ui_geomTypeLabel.Location = new System.Drawing.Point(4, 419);
            this.ui_geomTypeLabel.Name = "ui_geomTypeLabel";
            this.ui_geomTypeLabel.Size = new System.Drawing.Size(88, 13);
            this.ui_geomTypeLabel.TabIndex = 3;
            this.ui_geomTypeLabel.Text = "<GeometryType>";
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.AllowUserToOrderColumns = true;
            this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Location = new System.Drawing.Point(2, 2);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.Size = new System.Drawing.Size(916, 410);
            this.ui_dgv.TabIndex = 2;
            // 
            // ui_contextMSdgv
            // 
            this.ui_contextMSdgv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ui_zoomToTSMI});
            this.ui_contextMSdgv.Name = "ui_contextMSdgv";
            this.ui_contextMSdgv.Size = new System.Drawing.Size(124, 26);
            this.ui_contextMSdgv.Click += new System.EventHandler(this.ZoomToTSMI_Click);
            // 
            // ui_zoomToTSMI
            // 
            this.ui_zoomToTSMI.Name = "ui_zoomToTSMI";
            this.ui_zoomToTSMI.Size = new System.Drawing.Size(123, 22);
            this.ui_zoomToTSMI.Text = "Zoom To";
            // 
            // ui_rowCountLabel
            // 
            this.ui_rowCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_rowCountLabel.Location = new System.Drawing.Point(774, 419);
            this.ui_rowCountLabel.Name = "ui_rowCountLabel";
            this.ui_rowCountLabel.Size = new System.Drawing.Size(142, 13);
            this.ui_rowCountLabel.TabIndex = 4;
            this.ui_rowCountLabel.Text = "Count:  0";
            this.ui_rowCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AttributesPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_rowCountLabel);
            this.Controls.Add(this.ui_geomTypeLabel);
            this.Controls.Add(this.ui_dgv);
            this.Name = "AttributesPageControl";
            this.Size = new System.Drawing.Size(921, 440);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ui_contextMSdgv.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ui_geomTypeLabel;
        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.ContextMenuStrip ui_contextMSdgv;
        private System.Windows.Forms.ToolStripMenuItem ui_zoomToTSMI;
        private System.Windows.Forms.Label ui_rowCountLabel;
    }
}
