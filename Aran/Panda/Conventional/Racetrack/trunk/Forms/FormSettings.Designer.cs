namespace Aran.PANDA.Conventional.Racetrack.Forms
{
	partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.chckBxNominalTrack = new System.Windows.Forms.CheckBox();
            this.chckBxBuffers = new System.Windows.Forms.CheckBox();
            this.chckBxSector = new System.Windows.Forms.CheckBox();
            this.chckBxToleranceArea = new System.Windows.Forms.CheckBox();
            this.chckBxShablon = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chckBxTest = new System.Windows.Forms.CheckBox();
            this.lblFixToleranceDist = new System.Windows.Forms.Label();
            this.nmrcUpDownFixTolDist = new System.Windows.Forms.NumericUpDown();
            this.lblFixTolDistUnit = new System.Windows.Forms.Label();
            this.lblLowLimUnit = new System.Windows.Forms.Label();
            this.nmrcUpDwnLowLimHldngPattern = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chckBxSaveSecndPnt = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDownFixTolDist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnLowLimHldngPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // chckBxNominalTrack
            // 
            this.chckBxNominalTrack.AutoSize = true;
            this.chckBxNominalTrack.Location = new System.Drawing.Point(17, 24);
            this.chckBxNominalTrack.Name = "chckBxNominalTrack";
            this.chckBxNominalTrack.Size = new System.Drawing.Size(114, 17);
            this.chckBxNominalTrack.TabIndex = 13;
            this.chckBxNominalTrack.Text = "Nominal Trajectory";
            this.chckBxNominalTrack.UseVisualStyleBackColor = true;
            this.chckBxNominalTrack.CheckedChanged += new System.EventHandler(this.chckBxNominalTrack_CheckedChanged);
            // 
            // chckBxBuffers
            // 
            this.chckBxBuffers.AutoSize = true;
            this.chckBxBuffers.Location = new System.Drawing.Point(17, 126);
            this.chckBxBuffers.Name = "chckBxBuffers";
            this.chckBxBuffers.Size = new System.Drawing.Size(59, 17);
            this.chckBxBuffers.TabIndex = 12;
            this.chckBxBuffers.Text = "Buffers";
            this.chckBxBuffers.UseVisualStyleBackColor = true;
            this.chckBxBuffers.Visible = false;
            this.chckBxBuffers.CheckedChanged += new System.EventHandler(this.chckBxBuffers_CheckedChanged);
            // 
            // chckBxSector
            // 
            this.chckBxSector.AutoSize = true;
            this.chckBxSector.Location = new System.Drawing.Point(17, 160);
            this.chckBxSector.Name = "chckBxSector";
            this.chckBxSector.Size = new System.Drawing.Size(74, 17);
            this.chckBxSector.TabIndex = 11;
            this.chckBxSector.Text = "Entry area";
            this.chckBxSector.UseVisualStyleBackColor = true;
            this.chckBxSector.Visible = false;
            this.chckBxSector.CheckedChanged += new System.EventHandler(this.chckBxSector_CheckedChanged);
            // 
            // chckBxToleranceArea
            // 
            this.chckBxToleranceArea.AutoSize = true;
            this.chckBxToleranceArea.Location = new System.Drawing.Point(17, 92);
            this.chckBxToleranceArea.Name = "chckBxToleranceArea";
            this.chckBxToleranceArea.Size = new System.Drawing.Size(99, 17);
            this.chckBxToleranceArea.TabIndex = 10;
            this.chckBxToleranceArea.Text = "Tolerance Area";
            this.chckBxToleranceArea.UseVisualStyleBackColor = true;
            this.chckBxToleranceArea.CheckedChanged += new System.EventHandler(this.chckBxToleranceArea_CheckedChanged);
            // 
            // chckBxShablon
            // 
            this.chckBxShablon.AutoSize = true;
            this.chckBxShablon.Location = new System.Drawing.Point(17, 58);
            this.chckBxShablon.Name = "chckBxShablon";
            this.chckBxShablon.Size = new System.Drawing.Size(70, 17);
            this.chckBxShablon.TabIndex = 9;
            this.chckBxShablon.Text = "Template";
            this.chckBxShablon.UseVisualStyleBackColor = true;
            this.chckBxShablon.CheckedChanged += new System.EventHandler(this.chckBxShablon_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(206, 232);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chckBxShablon);
            this.groupBox1.Controls.Add(this.chckBxToleranceArea);
            this.groupBox1.Controls.Add(this.chckBxNominalTrack);
            this.groupBox1.Controls.Add(this.chckBxBuffers);
            this.groupBox1.Controls.Add(this.chckBxSector);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 117);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select the ones you want to be visible";
            // 
            // chckBxTest
            // 
            this.chckBxTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chckBxTest.AutoSize = true;
            this.chckBxTest.Location = new System.Drawing.Point(29, 233);
            this.chckBxTest.Name = "chckBxTest";
            this.chckBxTest.Size = new System.Drawing.Size(95, 17);
            this.chckBxTest.TabIndex = 16;
            this.chckBxTest.Text = "Is Test version";
            this.chckBxTest.UseVisualStyleBackColor = true;
            this.chckBxTest.Visible = false;
            this.chckBxTest.CheckedChanged += new System.EventHandler(this.chckBxTest_CheckedChanged);
            // 
            // lblFixToleranceDist
            // 
            this.lblFixToleranceDist.AutoSize = true;
            this.lblFixToleranceDist.Location = new System.Drawing.Point(30, 163);
            this.lblFixToleranceDist.Name = "lblFixToleranceDist";
            this.lblFixToleranceDist.Size = new System.Drawing.Size(94, 26);
            this.lblFixToleranceDist.TabIndex = 18;
            this.lblFixToleranceDist.Text = "Maximum fix \r\ntolerance distance";
            this.lblFixToleranceDist.Visible = false;
            // 
            // nmrcUpDownFixTolDist
            // 
            this.nmrcUpDownFixTolDist.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nmrcUpDownFixTolDist.Location = new System.Drawing.Point(140, 169);
            this.nmrcUpDownFixTolDist.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nmrcUpDownFixTolDist.Name = "nmrcUpDownFixTolDist";
            this.nmrcUpDownFixTolDist.Size = new System.Drawing.Size(68, 20);
            this.nmrcUpDownFixTolDist.TabIndex = 19;
            this.nmrcUpDownFixTolDist.Visible = false;
            this.nmrcUpDownFixTolDist.ValueChanged += new System.EventHandler(this.nmrcUpDownFixTolDist_ValueChanged);
            // 
            // lblFixTolDistUnit
            // 
            this.lblFixTolDistUnit.AutoSize = true;
            this.lblFixTolDistUnit.Location = new System.Drawing.Point(214, 172);
            this.lblFixTolDistUnit.Name = "lblFixTolDistUnit";
            this.lblFixTolDistUnit.Size = new System.Drawing.Size(35, 13);
            this.lblFixTolDistUnit.TabIndex = 20;
            this.lblFixTolDistUnit.Text = "label1";
            this.lblFixTolDistUnit.Visible = false;
            // 
            // lblLowLimUnit
            // 
            this.lblLowLimUnit.AutoSize = true;
            this.lblLowLimUnit.Location = new System.Drawing.Point(214, 137);
            this.lblLowLimUnit.Name = "lblLowLimUnit";
            this.lblLowLimUnit.Size = new System.Drawing.Size(35, 13);
            this.lblLowLimUnit.TabIndex = 23;
            this.lblLowLimUnit.Text = "label1";
            // 
            // nmrcUpDwnLowLimHldngPattern
            // 
            this.nmrcUpDwnLowLimHldngPattern.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nmrcUpDwnLowLimHldngPattern.Location = new System.Drawing.Point(140, 135);
            this.nmrcUpDwnLowLimHldngPattern.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.nmrcUpDwnLowLimHldngPattern.Name = "nmrcUpDwnLowLimHldngPattern";
            this.nmrcUpDwnLowLimHldngPattern.Size = new System.Drawing.Size(68, 20);
            this.nmrcUpDwnLowLimHldngPattern.TabIndex = 22;
            this.nmrcUpDwnLowLimHldngPattern.ValueChanged += new System.EventHandler(this.nmrcUpDwnLowLimHldngPattern_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 26);
            this.label2.TabIndex = 21;
            this.label2.Text = "Lower limit \r\nHolding Pattern";
            // 
            // chckBxSaveSecndPnt
            // 
            this.chckBxSaveSecndPnt.AutoSize = true;
            this.chckBxSaveSecndPnt.Location = new System.Drawing.Point(29, 202);
            this.chckBxSaveSecndPnt.Name = "chckBxSaveSecndPnt";
            this.chckBxSaveSecndPnt.Size = new System.Drawing.Size(132, 17);
            this.chckBxSaveSecndPnt.TabIndex = 24;
            this.chckBxSaveSecndPnt.Text = "Save secondary  point";
            this.chckBxSaveSecndPnt.UseVisualStyleBackColor = true;
            this.chckBxSaveSecndPnt.CheckedChanged += new System.EventHandler(this.chckBxSaveSecndPnt_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 261);
            this.Controls.Add(this.chckBxSaveSecndPnt);
            this.Controls.Add(this.lblLowLimUnit);
            this.Controls.Add(this.nmrcUpDwnLowLimHldngPattern);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFixTolDistUnit);
            this.Controls.Add(this.nmrcUpDownFixTolDist);
            this.Controls.Add(this.lblFixToleranceDist);
            this.Controls.Add(this.chckBxTest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(305, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(305, 300);
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDownFixTolDist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcUpDwnLowLimHldngPattern)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chckBxNominalTrack;
		private System.Windows.Forms.CheckBox chckBxBuffers;
		private System.Windows.Forms.CheckBox chckBxSector;
		private System.Windows.Forms.CheckBox chckBxToleranceArea;
		private System.Windows.Forms.CheckBox chckBxShablon;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chckBxTest;
		private System.Windows.Forms.Label lblFixToleranceDist;
		private System.Windows.Forms.NumericUpDown nmrcUpDownFixTolDist;
		private System.Windows.Forms.Label lblFixTolDistUnit;
		private System.Windows.Forms.Label lblLowLimUnit;
		private System.Windows.Forms.NumericUpDown nmrcUpDwnLowLimHldngPattern;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chckBxSaveSecndPnt;
    }
}