namespace EOSID
{
	partial class LegsInfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LegsInfoForm));
			this.ListView001 = new System.Windows.Forms.ListView();
			this.txtHeader_01 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.txtHeader_02 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_03 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_04 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_05 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_06 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_07 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.fltHeader_08 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.txtHeader_09 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.txtHeader_10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// ListView001
			// 
			this.ListView001.BackColor = System.Drawing.SystemColors.Window;
			this.ListView001.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.txtHeader_01,
            this.txtHeader_02,
            this.fltHeader_03,
            this.fltHeader_04,
            this.fltHeader_05,
            this.fltHeader_06,
            this.fltHeader_07,
            this.fltHeader_08,
            this.txtHeader_09,
            this.txtHeader_10});
			resources.ApplyResources(this.ListView001, "ListView001");
			this.ListView001.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ListView001.FullRowSelect = true;
			this.ListView001.GridLines = true;
			this.ListView001.Name = "ListView001";
			this.ListView001.UseCompatibleStateImageBehavior = false;
			this.ListView001.View = System.Windows.Forms.View.Details;
			this.ListView001.SelectedIndexChanged += new System.EventHandler(this.ListView001_SelectedIndexChanged);
			// 
			// txtHeader_01
			// 
			resources.ApplyResources(this.txtHeader_01, "txtHeader_01");
			// 
			// txtHeader_02
			// 
			resources.ApplyResources(this.txtHeader_02, "txtHeader_02");
			// 
			// fltHeader_03
			// 
			resources.ApplyResources(this.fltHeader_03, "fltHeader_03");
			// 
			// fltHeader_04
			// 
			resources.ApplyResources(this.fltHeader_04, "fltHeader_04");
			// 
			// fltHeader_05
			// 
			resources.ApplyResources(this.fltHeader_05, "fltHeader_05");
			// 
			// fltHeader_06
			// 
			resources.ApplyResources(this.fltHeader_06, "fltHeader_06");
			// 
			// fltHeader_07
			// 
			resources.ApplyResources(this.fltHeader_07, "fltHeader_07");
			// 
			// fltHeader_08
			// 
			resources.ApplyResources(this.fltHeader_08, "fltHeader_08");
			// 
			// txtHeader_09
			// 
			resources.ApplyResources(this.txtHeader_09, "txtHeader_09");
			// 
			// txtHeader_10
			// 
			resources.ApplyResources(this.txtHeader_10, "txtHeader_10");
			// 
			// LegsInfoForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ListView001);
			this.Name = "LegsInfoForm";
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LegsInfoForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.ListView ListView001;
		private System.Windows.Forms.ColumnHeader txtHeader_01;
		private System.Windows.Forms.ColumnHeader txtHeader_02;
		private System.Windows.Forms.ColumnHeader fltHeader_03;
		private System.Windows.Forms.ColumnHeader fltHeader_05;
		private System.Windows.Forms.ColumnHeader fltHeader_07;
		private System.Windows.Forms.ColumnHeader fltHeader_08;
		private System.Windows.Forms.ColumnHeader txtHeader_09;
		private System.Windows.Forms.ColumnHeader txtHeader_10;
		private System.Windows.Forms.ColumnHeader fltHeader_04;
		private System.Windows.Forms.ColumnHeader fltHeader_06;

	}
}