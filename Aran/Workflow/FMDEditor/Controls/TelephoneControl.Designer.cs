namespace Aran.Aim.FmdEditor
{
	partial class TelephoneControl
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
			System.Windows.Forms.Label label3;
			this.ui_codeCB = new System.Windows.Forms.ComboBox();
			this.ui_numberTB = new System.Windows.Forms.TextBox();
			this.ui_descriptionTB = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(5, 8);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 13);
			label1.TabIndex = 0;
			label1.Text = "Code:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(5, 35);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(47, 13);
			label2.TabIndex = 1;
			label2.Text = "Number:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(5, 61);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(63, 13);
			label3.TabIndex = 2;
			label3.Text = "Description:";
			// 
			// ui_codeCB
			// 
			this.ui_codeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_codeCB.FormattingEnabled = true;
			this.ui_codeCB.Location = new System.Drawing.Point(74, 5);
			this.ui_codeCB.Name = "ui_codeCB";
			this.ui_codeCB.Size = new System.Drawing.Size(121, 21);
			this.ui_codeCB.TabIndex = 3;
			// 
			// ui_numberTB
			// 
			this.ui_numberTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_numberTB.Location = new System.Drawing.Point(74, 32);
			this.ui_numberTB.Name = "ui_numberTB";
			this.ui_numberTB.Size = new System.Drawing.Size(230, 20);
			this.ui_numberTB.TabIndex = 4;
			// 
			// ui_descriptionTB
			// 
			this.ui_descriptionTB.AcceptsReturn = true;
			this.ui_descriptionTB.AcceptsTab = true;
			this.ui_descriptionTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_descriptionTB.Location = new System.Drawing.Point(74, 58);
			this.ui_descriptionTB.Multiline = true;
			this.ui_descriptionTB.Name = "ui_descriptionTB";
			this.ui_descriptionTB.Size = new System.Drawing.Size(230, 107);
			this.ui_descriptionTB.TabIndex = 5;
			// 
			// TelephoneControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ui_descriptionTB);
			this.Controls.Add(this.ui_numberTB);
			this.Controls.Add(this.ui_codeCB);
			this.Controls.Add(label3);
			this.Controls.Add(label2);
			this.Controls.Add(label1);
			this.Name = "TelephoneControl";
			this.Size = new System.Drawing.Size(310, 170);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox ui_codeCB;
		private System.Windows.Forms.TextBox ui_numberTB;
		private System.Windows.Forms.TextBox ui_descriptionTB;
	}
}
