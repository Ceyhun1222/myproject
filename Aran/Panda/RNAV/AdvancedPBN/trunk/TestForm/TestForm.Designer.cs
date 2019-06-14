namespace Aran.PANDA.RNAV.SGBAS.Forms
{
	partial class TestForm
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
			this.Frame0103 = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.TextBox0106 = new System.Windows.Forms.TextBox();
			this.ListView0101 = new System.Windows.Forms.ListView();
			this._ListView0101_ColumnHeader_1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._ListView0101_ColumnHeader_2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._ListView0101_ColumnHeader_3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._Label0101_10 = new System.Windows.Forms.Label();
			this._Label0101_6 = new System.Windows.Forms.Label();
			this._Label0101_3 = new System.Windows.Forms.Label();
			this.Frame0103.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// Frame0103
			// 
			this.Frame0103.BackColor = System.Drawing.SystemColors.Control;
			this.Frame0103.Controls.Add(this.label12);
			this.Frame0103.Controls.Add(this.numericUpDown1);
			this.Frame0103.Controls.Add(this.TextBox0106);
			this.Frame0103.Controls.Add(this.ListView0101);
			this.Frame0103.Controls.Add(this._Label0101_10);
			this.Frame0103.Controls.Add(this._Label0101_6);
			this.Frame0103.Controls.Add(this._Label0101_3);
			this.Frame0103.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Frame0103.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Frame0103.Location = new System.Drawing.Point(12, 12);
			this.Frame0103.Name = "Frame0103";
			this.Frame0103.Padding = new System.Windows.Forms.Padding(0);
			this.Frame0103.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.Frame0103.Size = new System.Drawing.Size(446, 190);
			this.Frame0103.TabIndex = 33;
			this.Frame0103.TabStop = false;
			this.Frame0103.Text = "Intermediate approach segment placement with no OCH increase";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.BackColor = System.Drawing.SystemColors.Control;
			this.label12.Cursor = System.Windows.Forms.Cursors.Default;
			this.label12.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label12.Location = new System.Drawing.Point(201, 29);
			this.label12.Name = "label12";
			this.label12.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label12.Size = new System.Drawing.Size(11, 14);
			this.label12.TabIndex = 256;
			this.label12.Text = "°";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.DecimalPlaces = 1;
			this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.numericUpDown1.Location = new System.Drawing.Point(146, 26);
			this.numericUpDown1.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(49, 20);
			this.numericUpDown1.TabIndex = 255;
			this.numericUpDown1.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
			// 
			// TextBox0106
			// 
			this.TextBox0106.AcceptsReturn = true;
			this.TextBox0106.BackColor = System.Drawing.SystemColors.Control;
			this.TextBox0106.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.TextBox0106.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextBox0106.ForeColor = System.Drawing.SystemColors.WindowText;
			this.TextBox0106.Location = new System.Drawing.Point(352, 26);
			this.TextBox0106.MaxLength = 0;
			this.TextBox0106.Name = "TextBox0106";
			this.TextBox0106.ReadOnly = true;
			this.TextBox0106.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TextBox0106.Size = new System.Drawing.Size(67, 20);
			this.TextBox0106.TabIndex = 36;
			// 
			// ListView0101
			// 
			this.ListView0101.BackColor = System.Drawing.SystemColors.Window;
			this.ListView0101.CheckBoxes = true;
			this.ListView0101.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._ListView0101_ColumnHeader_1,
            this._ListView0101_ColumnHeader_2,
            this._ListView0101_ColumnHeader_3});
			this.ListView0101.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ListView0101.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ListView0101.FullRowSelect = true;
			this.ListView0101.GridLines = true;
			this.ListView0101.LabelWrap = false;
			this.ListView0101.Location = new System.Drawing.Point(6, 59);
			this.ListView0101.Name = "ListView0101";
			this.ListView0101.Size = new System.Drawing.Size(432, 124);
			this.ListView0101.TabIndex = 98;
			this.ListView0101.UseCompatibleStateImageBehavior = false;
			this.ListView0101.View = System.Windows.Forms.View.Details;
			// 
			// _ListView0101_ColumnHeader_1
			// 
			this._ListView0101_ColumnHeader_1.Text = "FAP-IF, min";
			this._ListView0101_ColumnHeader_1.Width = 255;
			// 
			// _ListView0101_ColumnHeader_2
			// 
			this._ListView0101_ColumnHeader_2.Text = "FAP-IF, max";
			this._ListView0101_ColumnHeader_2.Width = 255;
			// 
			// _ListView0101_ColumnHeader_3
			// 
			this._ListView0101_ColumnHeader_3.Text = "To Avoid Obstacle";
			this._ListView0101_ColumnHeader_3.Width = 255;
			// 
			// _Label0101_10
			// 
			this._Label0101_10.AutoSize = true;
			this._Label0101_10.BackColor = System.Drawing.SystemColors.Control;
			this._Label0101_10.Cursor = System.Windows.Forms.Cursors.Default;
			this._Label0101_10.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._Label0101_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Label0101_10.Location = new System.Drawing.Point(423, 29);
			this._Label0101_10.Name = "_Label0101_10";
			this._Label0101_10.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._Label0101_10.Size = new System.Drawing.Size(20, 14);
			this._Label0101_10.TabIndex = 234;
			this._Label0101_10.Text = "km";
			// 
			// _Label0101_6
			// 
			this._Label0101_6.BackColor = System.Drawing.SystemColors.Control;
			this._Label0101_6.Cursor = System.Windows.Forms.Cursors.Default;
			this._Label0101_6.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._Label0101_6.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Label0101_6.Location = new System.Drawing.Point(234, 22);
			this._Label0101_6.Name = "_Label0101_6";
			this._Label0101_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._Label0101_6.Size = new System.Drawing.Size(116, 29);
			this._Label0101_6.TabIndex = 35;
			this._Label0101_6.Text = "Minimum length of the track:";
			// 
			// _Label0101_3
			// 
			this._Label0101_3.AutoSize = true;
			this._Label0101_3.BackColor = System.Drawing.SystemColors.Control;
			this._Label0101_3.Cursor = System.Windows.Forms.Cursors.Default;
			this._Label0101_3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._Label0101_3.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Label0101_3.Location = new System.Drawing.Point(8, 29);
			this._Label0101_3.Name = "_Label0101_3";
			this._Label0101_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this._Label0101_3.Size = new System.Drawing.Size(130, 14);
			this._Label0101_3.TabIndex = 33;
			this._Label0101_3.Text = "Magnitude of turn over IF:";
			this._Label0101_3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 450);
			this.Controls.Add(this.Frame0103);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.Frame0103.ResumeLayout(false);
			this.Frame0103.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.GroupBox Frame0103;
		public System.Windows.Forms.Label label12;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		public System.Windows.Forms.TextBox TextBox0106;
		public System.Windows.Forms.ListView ListView0101;
		public System.Windows.Forms.ColumnHeader _ListView0101_ColumnHeader_1;
		public System.Windows.Forms.ColumnHeader _ListView0101_ColumnHeader_2;
		public System.Windows.Forms.ColumnHeader _ListView0101_ColumnHeader_3;
		public System.Windows.Forms.Label _Label0101_10;
		public System.Windows.Forms.Label _Label0101_6;
		public System.Windows.Forms.Label _Label0101_3;
	}
}