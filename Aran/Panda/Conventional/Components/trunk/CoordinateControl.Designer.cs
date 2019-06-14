namespace ChoosePointNS
{
    partial class CoordinateControl
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
            this.lbLon = new System.Windows.Forms.Label();
            this.lbLat = new System.Windows.Forms.Label();
            this.ui_flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_textPanel = new System.Windows.Forms.Panel();
            this.ui_coordPanel = new System.Windows.Forms.Panel();
            this.ui_latDDMS = new ChoosePointNS.DD_DMS();
            this.ui_lonDDMS = new ChoosePointNS.DD_DMS();
            this.ui_flowLayoutPanel.SuspendLayout();
            this.ui_textPanel.SuspendLayout();
            this.ui_coordPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLon
            // 
            this.lbLon.AutoSize = true;
            this.lbLon.Location = new System.Drawing.Point(-1, 34);
            this.lbLon.Name = "lbLon";
            this.lbLon.Size = new System.Drawing.Size(28, 13);
            this.lbLon.TabIndex = 11;
            this.lbLon.Text = "Lon:";
            // 
            // lbLat
            // 
            this.lbLat.AutoSize = true;
            this.lbLat.Location = new System.Drawing.Point(-1, 7);
            this.lbLat.Name = "lbLat";
            this.lbLat.Size = new System.Drawing.Size(25, 13);
            this.lbLat.TabIndex = 10;
            this.lbLat.Text = "Lat:";
            // 
            // ui_flowLayoutPanel
            // 
            this.ui_flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_flowLayoutPanel.Controls.Add(this.ui_textPanel);
            this.ui_flowLayoutPanel.Controls.Add(this.ui_coordPanel);
            this.ui_flowLayoutPanel.Location = new System.Drawing.Point(3, -4);
            this.ui_flowLayoutPanel.Name = "ui_flowLayoutPanel";
            this.ui_flowLayoutPanel.Size = new System.Drawing.Size(258, 63);
            this.ui_flowLayoutPanel.TabIndex = 12;
            this.ui_flowLayoutPanel.WrapContents = false;
            // 
            // ui_textPanel
            // 
            this.ui_textPanel.Controls.Add(this.lbLat);
            this.ui_textPanel.Controls.Add(this.lbLon);
            this.ui_textPanel.Location = new System.Drawing.Point(3, 3);
            this.ui_textPanel.Name = "ui_textPanel";
            this.ui_textPanel.Size = new System.Drawing.Size(26, 57);
            this.ui_textPanel.TabIndex = 0;
            // 
            // ui_coordPanel
            // 
            this.ui_coordPanel.Controls.Add(this.ui_latDDMS);
            this.ui_coordPanel.Controls.Add(this.ui_lonDDMS);
            this.ui_coordPanel.Location = new System.Drawing.Point(35, 3);
            this.ui_coordPanel.Name = "ui_coordPanel";
            this.ui_coordPanel.Size = new System.Drawing.Size(230, 57);
            this.ui_coordPanel.TabIndex = 1;
            // 
            // ui_latDDMS
            // 
            this.ui_latDDMS.DDAccuracy = 0;
            this.ui_latDDMS.DMSAccuracy = 0;
            this.ui_latDDMS.IsDD = true;
            this.ui_latDDMS.IsX = false;
            this.ui_latDDMS.Location = new System.Drawing.Point(3, 3);
            this.ui_latDDMS.Name = "ui_latDDMS";
            this.ui_latDDMS.ReadOnly = false;
            this.ui_latDDMS.Size = new System.Drawing.Size(211, 26);
            this.ui_latDDMS.TabIndex = 8;
            this.ui_latDDMS.Value = 0D;
            this.ui_latDDMS.ValueChanged += new System.EventHandler(this.LatLon_ValueChanged);
            // 
            // ui_lonDDMS
            // 
            this.ui_lonDDMS.DDAccuracy = 0;
            this.ui_lonDDMS.DMSAccuracy = 0;
            this.ui_lonDDMS.IsDD = true;
            this.ui_lonDDMS.IsX = true;
            this.ui_lonDDMS.Location = new System.Drawing.Point(3, 32);
            this.ui_lonDDMS.Name = "ui_lonDDMS";
            this.ui_lonDDMS.ReadOnly = false;
            this.ui_lonDDMS.Size = new System.Drawing.Size(211, 26);
            this.ui_lonDDMS.TabIndex = 9;
            this.ui_lonDDMS.Value = 0D;
            this.ui_lonDDMS.ValueChanged += new System.EventHandler(this.LatLon_ValueChanged);
            // 
            // CoordinateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_flowLayoutPanel);
            this.Name = "CoordinateControl";
            this.Size = new System.Drawing.Size(254, 60);
            this.ui_flowLayoutPanel.ResumeLayout(false);
            this.ui_textPanel.ResumeLayout(false);
            this.ui_textPanel.PerformLayout();
            this.ui_coordPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DD_DMS ui_lonDDMS;
        private DD_DMS ui_latDDMS;
        private System.Windows.Forms.Label lbLon;
        private System.Windows.Forms.Label lbLat;
        private System.Windows.Forms.FlowLayoutPanel ui_flowLayoutPanel;
        private System.Windows.Forms.Panel ui_textPanel;
        private System.Windows.Forms.Panel ui_coordPanel;
    }
}
