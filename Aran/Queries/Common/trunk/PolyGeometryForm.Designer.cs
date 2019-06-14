namespace Aran.Queries.Common
{
    partial class PolyGeometryForm
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
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_sessionGeomButton = new System.Windows.Forms.Button();
            this.ui_sessionGeomMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ui_okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.SuspendLayout();
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
            this.ui_colX,
            this.ui_colZ});
            this.ui_dgv.Location = new System.Drawing.Point(5, 5);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ui_dgv.Size = new System.Drawing.Size(424, 292);
            this.ui_dgv.TabIndex = 2;
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
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(355, 303);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 3;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_sessionGeomButton
            // 
            this.ui_sessionGeomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_sessionGeomButton.Location = new System.Drawing.Point(5, 303);
            this.ui_sessionGeomButton.Name = "ui_sessionGeomButton";
            this.ui_sessionGeomButton.Size = new System.Drawing.Size(124, 23);
            this.ui_sessionGeomButton.TabIndex = 4;
            this.ui_sessionGeomButton.Text = "Session Geometries";
            this.ui_sessionGeomButton.UseVisualStyleBackColor = true;
            this.ui_sessionGeomButton.Click += new System.EventHandler(this.SessionGeoms_Click);
            // 
            // ui_sessionGeomMenu
            // 
            this.ui_sessionGeomMenu.Name = "ui_sessionGeomMenu";
            this.ui_sessionGeomMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Location = new System.Drawing.Point(274, 303);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 6;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // PolyGeometryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 332);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_sessionGeomButton);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(this.ui_dgv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PolyGeometryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geometry";
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colZ;
        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.Button ui_sessionGeomButton;
        private System.Windows.Forms.ContextMenuStrip ui_sessionGeomMenu;
        private System.Windows.Forms.Button ui_okButton;
    }
}