namespace SigmaCallout
{
	partial class FormMorse
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
            this.btnClose = new System.Windows.Forms.Button();
            this.nmrcUpDwnShiftOnX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nmrcUpDwnShiftOnY = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nmrcUpDwnLeading = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnShiftOnX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnShiftOnY)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnLeading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(118, 124);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 25);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // nmrcUpDwnShiftOnX
            // 
            this.nmrcUpDwnShiftOnX.Location = new System.Drawing.Point(64, 22);
            this.nmrcUpDwnShiftOnX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nmrcUpDwnShiftOnX.Name = "nmrcUpDwnShiftOnX";
            this.nmrcUpDwnShiftOnX.Size = new System.Drawing.Size(51, 20);
            this.nmrcUpDwnShiftOnX.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "X axis";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Y axis";
            // 
            // nmrcUpDwnShiftOnY
            // 
            this.nmrcUpDwnShiftOnY.Location = new System.Drawing.Point(64, 48);
            this.nmrcUpDwnShiftOnY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nmrcUpDwnShiftOnY.Name = "nmrcUpDwnShiftOnY";
            this.nmrcUpDwnShiftOnY.Size = new System.Drawing.Size(51, 20);
            this.nmrcUpDwnShiftOnY.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nmrcUpDwnShiftOnY);
            this.groupBox1.Controls.Add(this.nmrcUpDwnShiftOnX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(131, 74);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shift coordinates";
            this.groupBox1.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Leading";
            this.label4.Visible = false;
            // 
            // nmrcUpDwnLeading
            // 
            this.nmrcUpDwnLeading.DecimalPlaces = 1;
            this.nmrcUpDwnLeading.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmrcUpDwnLeading.Location = new System.Drawing.Point(76, 11);
            this.nmrcUpDwnLeading.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmrcUpDwnLeading.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nmrcUpDwnLeading.Name = "nmrcUpDwnLeading";
            this.nmrcUpDwnLeading.Size = new System.Drawing.Size(51, 20);
            this.nmrcUpDwnLeading.TabIndex = 34;
            this.nmrcUpDwnLeading.Visible = false;
            // 
            // FormMorse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 162);
            this.Controls.Add(this.nmrcUpDwnLeading);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(190, 190);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(190, 190);
            this.Name = "FormMorse";
            this.ShowIcon = false;
            this.Text = "Morse element settings";
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnShiftOnX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnShiftOnY)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnLeading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.NumericUpDown nmrcUpDwnShiftOnX;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nmrcUpDwnShiftOnY;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nmrcUpDwnLeading;
	}
}