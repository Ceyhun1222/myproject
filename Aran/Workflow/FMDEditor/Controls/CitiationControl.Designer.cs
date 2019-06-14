namespace Aran.Aim.FmdEditor
{
	partial class CitiationControl
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
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label3;
			this.ui_dateTypeCB = new System.Windows.Forms.ComboBox();
			this.ui_titleTB = new System.Windows.Forms.TextBox();
			this.ui_processCertificationTB = new System.Windows.Forms.TextBox();
			this.ui_dateTimePicker = new Aran.Aim.FmdEditor.NullableDateTimePicker();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label4 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(8, 7);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(30, 13);
			label1.TabIndex = 0;
			label1.Text = "Title:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(8, 33);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(106, 13);
			label2.TabIndex = 1;
			label2.Text = "Process Certification:";
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.ui_dateTimePicker);
			groupBox1.Controls.Add(this.ui_dateTypeCB);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(label3);
			groupBox1.Location = new System.Drawing.Point(42, 56);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(257, 79);
			groupBox1.TabIndex = 4;
			groupBox1.TabStop = false;
			groupBox1.Text = "Date";
			// 
			// ui_dateTypeCB
			// 
			this.ui_dateTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_dateTypeCB.FormattingEnabled = true;
			this.ui_dateTypeCB.Location = new System.Drawing.Point(109, 46);
			this.ui_dateTypeCB.Name = "ui_dateTypeCB";
			this.ui_dateTypeCB.Size = new System.Drawing.Size(137, 21);
			this.ui_dateTypeCB.TabIndex = 8;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(12, 49);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(60, 13);
			label4.TabIndex = 7;
			label4.Text = "Date Type:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(12, 22);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(33, 13);
			label3.TabIndex = 5;
			label3.Text = "Date:";
			// 
			// ui_titleTB
			// 
			this.ui_titleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_titleTB.Location = new System.Drawing.Point(120, 4);
			this.ui_titleTB.Name = "ui_titleTB";
			this.ui_titleTB.Size = new System.Drawing.Size(179, 20);
			this.ui_titleTB.TabIndex = 2;
			// 
			// ui_processCertificationTB
			// 
			this.ui_processCertificationTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_processCertificationTB.Location = new System.Drawing.Point(120, 30);
			this.ui_processCertificationTB.Name = "ui_processCertificationTB";
			this.ui_processCertificationTB.Size = new System.Drawing.Size(179, 20);
			this.ui_processCertificationTB.TabIndex = 3;
			// 
			// ui_dateTimePicker
			// 
			this.ui_dateTimePicker.CustomFormat = "yyyy-MM-dd";
			this.ui_dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.ui_dateTimePicker.Location = new System.Drawing.Point(109, 19);
			this.ui_dateTimePicker.Name = "ui_dateTimePicker";
			this.ui_dateTimePicker.ShowCheckBox = true;
			this.ui_dateTimePicker.Size = new System.Drawing.Size(137, 20);
			this.ui_dateTimePicker.TabIndex = 9;
			// 
			// CitiationControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(groupBox1);
			this.Controls.Add(this.ui_processCertificationTB);
			this.Controls.Add(this.ui_titleTB);
			this.Controls.Add(label2);
			this.Controls.Add(label1);
			this.Name = "CitiationControl";
			this.Size = new System.Drawing.Size(307, 143);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ui_titleTB;
		private System.Windows.Forms.TextBox ui_processCertificationTB;
		private System.Windows.Forms.ComboBox ui_dateTypeCB;
		private NullableDateTimePicker ui_dateTimePicker;
	}
}
