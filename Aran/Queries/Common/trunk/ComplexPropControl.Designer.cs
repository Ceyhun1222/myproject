namespace Aran.Queries.Common
{
    partial class ComplexPropControl
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
            this.mainGrb = new System.Windows.Forms.GroupBox ();
            this.propNameLinkLabel = new System.Windows.Forms.LinkLabel ();
            this.mainGrb.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // mainGrb
            // 
            this.mainGrb.Controls.Add (this.propNameLinkLabel);
            this.mainGrb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainGrb.Location = new System.Drawing.Point (0, 0);
            this.mainGrb.Name = "mainGrb";
            this.mainGrb.Size = new System.Drawing.Size (120, 44);
            this.mainGrb.TabIndex = 0;
            this.mainGrb.TabStop = false;
            // 
            // propNameLinkLabel
            // 
            this.propNameLinkLabel.AutoSize = true;
            this.propNameLinkLabel.Location = new System.Drawing.Point (15, 16);
            this.propNameLinkLabel.Name = "propNameLinkLabel";
            this.propNameLinkLabel.Size = new System.Drawing.Size (89, 13);
            this.propNameLinkLabel.TabIndex = 0;
            this.propNameLinkLabel.TabStop = true;
            this.propNameLinkLabel.Text = "<Property Name>";
            this.propNameLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler (this.propNameLinkLabel_LinkClicked);
            // 
            // ComplexPropControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add (this.mainGrb);
            this.Name = "ComplexPropControl";
            this.Size = new System.Drawing.Size (120, 44);
            this.mainGrb.ResumeLayout (false);
            this.mainGrb.PerformLayout ();
            this.ResumeLayout (false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mainGrb;
        private System.Windows.Forms.LinkLabel propNameLinkLabel;
    }
}
