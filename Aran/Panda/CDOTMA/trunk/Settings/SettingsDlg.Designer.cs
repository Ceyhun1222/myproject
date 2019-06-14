namespace CDOTMA
{
	partial class SettingsDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDlg));
			this.gbLanguage = new System.Windows.Forms.GroupBox();
			this.comboBox5 = new System.Windows.Forms.ComboBox();
			this.gbPrecision = new System.Windows.Forms.GroupBox();
			this.textBox6 = new System.Windows.Forms.TextBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.gbUnits = new System.Windows.Forms.GroupBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.gbParameter = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnApplay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.gbLanguage.SuspendLayout();
			this.gbPrecision.SuspendLayout();
			this.gbUnits.SuspendLayout();
			this.gbParameter.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbLanguage
			// 
			this.gbLanguage.Controls.Add(this.comboBox5);
			this.gbLanguage.Location = new System.Drawing.Point(309, 1);
			this.gbLanguage.Name = "gbLanguage";
			this.gbLanguage.Size = new System.Drawing.Size(89, 90);
			this.gbLanguage.TabIndex = 23;
			this.gbLanguage.TabStop = false;
			this.gbLanguage.Text = "Language";
			this.gbLanguage.Visible = false;
			// 
			// comboBox5
			// 
			this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.Items.AddRange(new object[] {
            "English",
            "Potuguese",
            "Russian"});
			this.comboBox5.Location = new System.Drawing.Point(6, 19);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(77, 21);
			this.comboBox5.TabIndex = 1;
			this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
			// 
			// gbPrecision
			// 
			this.gbPrecision.Controls.Add(this.textBox6);
			this.gbPrecision.Controls.Add(this.textBox5);
			this.gbPrecision.Controls.Add(this.textBox4);
			this.gbPrecision.Controls.Add(this.textBox3);
			this.gbPrecision.Controls.Add(this.textBox2);
			this.gbPrecision.Controls.Add(this.textBox1);
			this.gbPrecision.Location = new System.Drawing.Point(210, 1);
			this.gbPrecision.Name = "gbPrecision";
			this.gbPrecision.Size = new System.Drawing.Size(93, 192);
			this.gbPrecision.TabIndex = 22;
			this.gbPrecision.TabStop = false;
			this.gbPrecision.Text = "Precision";
			// 
			// textBox6
			// 
			this.textBox6.Location = new System.Drawing.Point(6, 158);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new System.Drawing.Size(81, 20);
			this.textBox6.TabIndex = 5;
			this.textBox6.Leave += new System.EventHandler(this.textBox6_Leave);
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(6, 130);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(81, 20);
			this.textBox5.TabIndex = 4;
			this.textBox5.Leave += new System.EventHandler(this.textBox5_Leave);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(6, 102);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(81, 20);
			this.textBox4.TabIndex = 3;
			this.textBox4.Leave += new System.EventHandler(this.textBox4_Leave);
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(6, 74);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(81, 20);
			this.textBox3.TabIndex = 2;
			this.textBox3.Leave += new System.EventHandler(this.textBox3_Leave);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(6, 47);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(81, 20);
			this.textBox2.TabIndex = 1;
			this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(6, 20);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(81, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
			// 
			// gbUnits
			// 
			this.gbUnits.Controls.Add(this.button2);
			this.gbUnits.Controls.Add(this.button1);
			this.gbUnits.Controls.Add(this.comboBox4);
			this.gbUnits.Controls.Add(this.comboBox3);
			this.gbUnits.Controls.Add(this.comboBox2);
			this.gbUnits.Controls.Add(this.comboBox1);
			this.gbUnits.Location = new System.Drawing.Point(104, 1);
			this.gbUnits.Name = "gbUnits";
			this.gbUnits.Size = new System.Drawing.Size(100, 192);
			this.gbUnits.TabIndex = 21;
			this.gbUnits.TabStop = false;
			this.gbUnits.Text = "Units";
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.button2.Location = new System.Drawing.Point(10, 156);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(77, 21);
			this.button2.TabIndex = 5;
			this.button2.Text = "%";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Enabled = false;
			this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.button1.Location = new System.Drawing.Point(10, 127);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(77, 21);
			this.button1.TabIndex = 4;
			this.button1.Text = "°";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// comboBox4
			// 
			this.comboBox4.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox4.Enabled = false;
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Items.AddRange(new object[] {
            "meter/min",
            "feet/min"});
			this.comboBox4.Location = new System.Drawing.Point(10, 102);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(77, 21);
			this.comboBox4.TabIndex = 3;
			// 
			// comboBox3
			// 
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Items.AddRange(new object[] {
            "kM/h",
            "knot"});
			this.comboBox3.Location = new System.Drawing.Point(10, 74);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(77, 21);
			this.comboBox3.TabIndex = 2;
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "meter",
            "feet"});
			this.comboBox2.Location = new System.Drawing.Point(10, 47);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(77, 21);
			this.comboBox2.TabIndex = 1;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "kM",
            "N.m."});
			this.comboBox1.Location = new System.Drawing.Point(10, 20);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(77, 21);
			this.comboBox1.TabIndex = 0;
			// 
			// gbParameter
			// 
			this.gbParameter.Controls.Add(this.label6);
			this.gbParameter.Controls.Add(this.label5);
			this.gbParameter.Controls.Add(this.label4);
			this.gbParameter.Controls.Add(this.label3);
			this.gbParameter.Controls.Add(this.label2);
			this.gbParameter.Controls.Add(this.label1);
			this.gbParameter.Location = new System.Drawing.Point(4, 1);
			this.gbParameter.Name = "gbParameter";
			this.gbParameter.Size = new System.Drawing.Size(94, 192);
			this.gbParameter.TabIndex = 20;
			this.gbParameter.TabStop = false;
			this.gbParameter.Text = "Parameter";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new System.Drawing.Point(6, 161);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(50, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "Gradient:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(6, 133);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(37, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Angle:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(6, 105);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Descent rate:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(6, 77);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Speed:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(6, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "ELEV/HGT:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(6, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Distance:";
			// 
			// btnApplay
			// 
			this.btnApplay.Enabled = false;
			this.btnApplay.Location = new System.Drawing.Point(316, 166);
			this.btnApplay.Name = "btnApplay";
			this.btnApplay.Size = new System.Drawing.Size(75, 23);
			this.btnApplay.TabIndex = 29;
			this.btnApplay.Text = "&Apply";
			this.btnApplay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(316, 138);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 28;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(316, 109);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 27;
			this.btnOK.Text = "&OK";
			// 
			// SettingsDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(403, 196);
			this.Controls.Add(this.btnApplay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.gbLanguage);
			this.Controls.Add(this.gbPrecision);
			this.Controls.Add(this.gbUnits);
			this.Controls.Add(this.gbParameter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Unit settings";
			this.gbLanguage.ResumeLayout(false);
			this.gbPrecision.ResumeLayout(false);
			this.gbPrecision.PerformLayout();
			this.gbUnits.ResumeLayout(false);
			this.gbParameter.ResumeLayout(false);
			this.gbParameter.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbLanguage;
		private System.Windows.Forms.ComboBox comboBox5;
		private System.Windows.Forms.GroupBox gbPrecision;
		private System.Windows.Forms.TextBox textBox6;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox gbUnits;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox comboBox4;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.GroupBox gbParameter;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnApplay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}