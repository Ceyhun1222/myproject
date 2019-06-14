namespace ChoosePointNS
{
	partial class SignificantPointBox
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbMain = new System.Windows.Forms.GroupBox ();
            this.lbOrganisation = new System.Windows.Forms.Label ();
            this.lbAirport = new System.Windows.Forms.Label ();
            this.lbSignificantPoint = new System.Windows.Forms.Label ();
            this.cbOrganisation = new System.Windows.Forms.ComboBox ();
            this.cbAirport = new System.Windows.Forms.ComboBox ();
            this.cbSignificantPoint = new System.Windows.Forms.ComboBox ();
            this.cbPointsList = new System.Windows.Forms.ComboBox ();
            this.gbMain.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // gbMain
            // 
            this.gbMain.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom ) 
            | System.Windows.Forms.AnchorStyles.Left ) 
            | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.gbMain.Controls.Add ( this.lbOrganisation );
            this.gbMain.Controls.Add ( this.lbAirport );
            this.gbMain.Controls.Add ( this.lbSignificantPoint );
            this.gbMain.Location = new System.Drawing.Point ( 3, 0 );
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size ( 222, 136 );
            this.gbMain.TabIndex = 1;
            this.gbMain.TabStop = false;
            // 
            // lbOrganisation
            // 
            this.lbOrganisation.AutoSize = true;
            this.lbOrganisation.Location = new System.Drawing.Point ( 6, 21 );
            this.lbOrganisation.Name = "lbOrganisation";
            this.lbOrganisation.Size = new System.Drawing.Size ( 69, 13 );
            this.lbOrganisation.TabIndex = 6;
            this.lbOrganisation.Text = "Organisation:";
            // 
            // lbAirport
            // 
            this.lbAirport.AutoSize = true;
            this.lbAirport.Location = new System.Drawing.Point ( 6, 48 );
            this.lbAirport.Name = "lbAirport";
            this.lbAirport.Size = new System.Drawing.Size ( 40, 13 );
            this.lbAirport.TabIndex = 7;
            this.lbAirport.Text = "Airport:";
            // 
            // lbSignificantPoint
            // 
            this.lbSignificantPoint.Location = new System.Drawing.Point ( 6, 69 );
            this.lbSignificantPoint.Name = "lbSignificantPoint";
            this.lbSignificantPoint.Size = new System.Drawing.Size ( 69, 27 );
            this.lbSignificantPoint.TabIndex = 8;
            this.lbSignificantPoint.Text = "Significant Point :";
            // 
            // cbOrganisation
            // 
            this.cbOrganisation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrganisation.FormattingEnabled = true;
            this.cbOrganisation.Location = new System.Drawing.Point ( 89, 16 );
            this.cbOrganisation.Name = "cbOrganisation";
            this.cbOrganisation.Size = new System.Drawing.Size ( 121, 21 );
            this.cbOrganisation.TabIndex = 2;
            this.cbOrganisation.SelectedIndexChanged += new System.EventHandler ( this.cbOrganisation_SelectedIndexChanged );
            // 
            // cbAirport
            // 
            this.cbAirport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAirport.FormattingEnabled = true;
            this.cbAirport.Location = new System.Drawing.Point ( 89, 45 );
            this.cbAirport.Name = "cbAirport";
            this.cbAirport.Size = new System.Drawing.Size ( 121, 21 );
            this.cbAirport.TabIndex = 3;
            this.cbAirport.SelectedIndexChanged += new System.EventHandler ( this.cbAerodrome_SelectedIndexChanged );
            // 
            // cbSignificantPoint
            // 
            this.cbSignificantPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSignificantPoint.FormattingEnabled = true;
            this.cbSignificantPoint.Location = new System.Drawing.Point ( 89, 73 );
            this.cbSignificantPoint.Name = "cbSignificantPoint";
            this.cbSignificantPoint.Size = new System.Drawing.Size ( 121, 21 );
            this.cbSignificantPoint.TabIndex = 4;
            this.cbSignificantPoint.SelectedIndexChanged += new System.EventHandler ( this.cbSignificantPoint_SelectedIndexChanged );
            // 
            // cbPointsList
            // 
            this.cbPointsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPointsList.FormattingEnabled = true;
            this.cbPointsList.Location = new System.Drawing.Point ( 89, 102 );
            this.cbPointsList.Name = "cbPointsList";
            this.cbPointsList.Size = new System.Drawing.Size ( 121, 21 );
            this.cbPointsList.TabIndex = 5;
            this.cbPointsList.SelectedIndexChanged += new System.EventHandler ( this.cbPointsList_SelectedIndexChanged );
            // 
            // SignificantPointBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.cbOrganisation );
            this.Controls.Add ( this.cbAirport );
            this.Controls.Add ( this.cbSignificantPoint );
            this.Controls.Add ( this.cbPointsList );
            this.Controls.Add ( this.gbMain );
            this.Name = "SignificantPointBox";
            this.Size = new System.Drawing.Size ( 228, 143 );
            this.gbMain.ResumeLayout ( false );
            this.gbMain.PerformLayout ();
            this.ResumeLayout ( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox gbMain;
		private System.Windows.Forms.ComboBox cbOrganisation;
		private System.Windows.Forms.ComboBox cbAirport;
		private System.Windows.Forms.ComboBox cbSignificantPoint;
		private System.Windows.Forms.ComboBox cbPointsList;
		private System.Windows.Forms.Label lbOrganisation;
		private System.Windows.Forms.Label lbAirport;
		private System.Windows.Forms.Label lbSignificantPoint;
	}
}
