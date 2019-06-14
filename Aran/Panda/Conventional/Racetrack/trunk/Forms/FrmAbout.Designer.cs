namespace Aran.PANDA.Conventional.Racetrack.Forms
{
	partial class FormAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ( );
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ( )
		{
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCopyrightLeftPart = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblBuildDate = new System.Windows.Forms.Label();
            this.lblCopyrightEnd = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Aran.PANDA.Conventional.Racetrack.Properties.Resources.risk_final_tf;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(286, 57);
            this.pictureBox1.TabIndex = 538;
            this.pictureBox1.TabStop = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(193, 82);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(51, 13);
            this.label.TabIndex = 542;
            this.label.Text = "Version : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 541;
            this.label2.Text = "Coventional Holding";
            // 
            // lblCopyrightLeftPart
            // 
            this.lblCopyrightLeftPart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyrightLeftPart.AutoSize = true;
            this.lblCopyrightLeftPart.Location = new System.Drawing.Point(5, 155);
            this.lblCopyrightLeftPart.Name = "lblCopyrightLeftPart";
            this.lblCopyrightLeftPart.Size = new System.Drawing.Size(90, 13);
            this.lblCopyrightLeftPart.TabIndex = 543;
            this.lblCopyrightLeftPart.Text = "Copyright© 2001-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 544;
            this.label3.Text = "Build date : ";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(247, 82);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(28, 13);
            this.lblVersion.TabIndex = 545;
            this.lblVersion.Text = "___ ";
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.Location = new System.Drawing.Point(247, 107);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(25, 13);
            this.lblBuildDate.TabIndex = 546;
            this.lblBuildDate.Text = "___";
            // 
            // lblCopyrightEnd
            // 
            this.lblCopyrightEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyrightEnd.AutoSize = true;
            this.lblCopyrightEnd.Location = new System.Drawing.Point(120, 155);
            this.lblCopyrightEnd.Name = "lblCopyrightEnd";
            this.lblCopyrightEnd.Size = new System.Drawing.Size(193, 13);
            this.lblCopyrightEnd.TabIndex = 547;
            this.lblCopyrightEnd.Text = "R.I.S.K. Company. All  Rights Reserved";
            // 
            // lblYear
            // 
            this.lblYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(92, 155);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(31, 13);
            this.lblYear.TabIndex = 548;
            this.lblYear.Text = "____";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 177);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.lblCopyrightEnd);
            this.Controls.Add(this.lblBuildDate);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCopyrightLeftPart);
            this.Controls.Add(this.label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowIcon = false;
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAbout_FormClosing);
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblCopyrightLeftPart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblBuildDate;
        private System.Windows.Forms.Label lblCopyrightEnd;
        private System.Windows.Forms.Label lblYear;
    }
}