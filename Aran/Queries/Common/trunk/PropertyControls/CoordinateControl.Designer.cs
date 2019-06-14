namespace Aran.Queries.Common
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.ui_xDDMS = new ChoosePointNS.DD_DMS();
            this.ui_yDDMS = new ChoosePointNS.DD_DMS();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(5, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 13);
            label1.TabIndex = 1;
            label1.Text = "Latitude:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(5, 37);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(57, 13);
            label2.TabIndex = 3;
            label2.Text = "Longitude:";
            // 
            // ui_xDDMS
            // 
            this.ui_xDDMS.DDAccuracy = 6;
            this.ui_xDDMS.DMSAccuracy = 4;
            this.ui_xDDMS.IsDD = true;
            this.ui_xDDMS.IsX = true;
            this.ui_xDDMS.Location = new System.Drawing.Point(65, 31);
            this.ui_xDDMS.Name = "ui_xDDMS";
            this.ui_xDDMS.Size = new System.Drawing.Size(225, 26);
            this.ui_xDDMS.TabIndex = 2;
            this.ui_xDDMS.Value = 0D;
            this.ui_xDDMS.ValueChanged += new System.EventHandler(this.DDMS_ValueChanged);
            // 
            // ui_yDDMS
            // 
            this.ui_yDDMS.DDAccuracy = 6;
            this.ui_yDDMS.DMSAccuracy = 4;
            this.ui_yDDMS.IsDD = true;
            this.ui_yDDMS.IsX = false;
            this.ui_yDDMS.Location = new System.Drawing.Point(65, -1);
            this.ui_yDDMS.Name = "ui_yDDMS";
            this.ui_yDDMS.Size = new System.Drawing.Size(225, 26);
            this.ui_yDDMS.TabIndex = 0;
            this.ui_yDDMS.Value = 0D;
            this.ui_yDDMS.ValueChanged += new System.EventHandler(this.DDMS_ValueChanged);
            // 
            // CoordinateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.ui_xDDMS);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_yDDMS);
            this.Controls.Add(label2);
            this.Name = "CoordinateControl";
            this.Size = new System.Drawing.Size(293, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoosePointNS.DD_DMS ui_yDDMS;
        private ChoosePointNS.DD_DMS ui_xDDMS;
    }
}
