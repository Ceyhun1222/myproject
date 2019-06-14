namespace Aran.Panda.VisualManoeuvring.Forms
{
    partial class Parameters
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Parameters));
            this.lstVw_Parameters = new System.Windows.Forms.ListView();
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lstVw_Parameters
            // 
            this.lstVw_Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVw_Parameters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17});
            this.lstVw_Parameters.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstVw_Parameters.Font = new System.Drawing.Font("Arial", 8F);
            this.lstVw_Parameters.FullRowSelect = true;
            this.lstVw_Parameters.GridLines = true;
            this.lstVw_Parameters.HideSelection = false;
            this.lstVw_Parameters.Location = new System.Drawing.Point(1, 1);
            this.lstVw_Parameters.Name = "lstVw_Parameters";
            this.lstVw_Parameters.Size = new System.Drawing.Size(595, 330);
            this.lstVw_Parameters.TabIndex = 5;
            this.lstVw_Parameters.UseCompatibleStateImageBehavior = false;
            this.lstVw_Parameters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Parameter";
            this.columnHeader15.Width = 250;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Value";
            this.columnHeader16.Width = 170;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "UoM";
            this.columnHeader17.Width = 170;
            // 
            // Parameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 332);
            this.Controls.Add(this.lstVw_Parameters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Parameters";
            this.Text = "Parameters";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Parameters_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView lstVw_Parameters;
        internal System.Windows.Forms.ColumnHeader columnHeader15;
        internal System.Windows.Forms.ColumnHeader columnHeader16;
        internal System.Windows.Forms.ColumnHeader columnHeader17;
    }
}