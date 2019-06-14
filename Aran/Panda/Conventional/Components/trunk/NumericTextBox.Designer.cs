namespace ChoosePointNS
{
	partial class NumericTextBox
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
			this.tbNumricText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbNumricText
			// 
			this.tbNumricText.Location = new System.Drawing.Point(0, 0);
			this.tbNumricText.Name = "tbNumricText";
			this.tbNumricText.Size = new System.Drawing.Size(100, 20);
			this.tbNumricText.TabIndex = 0;
			this.tbNumricText.TextChanged += new System.EventHandler(this.tbNumricText_TextChanged);
			this.tbNumricText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumricText_KeyPress);
			this.tbNumricText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbNumricText_KeyUp);
			this.tbNumricText.Leave += new System.EventHandler(this.tbNumricText_Leave);
			this.tbNumricText.Validating += new System.ComponentModel.CancelEventHandler(this.tbNumricText_Validating);
			// 
			// NumericTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tbNumricText);
			this.Name = "NumericTextBox";
			this.Size = new System.Drawing.Size(103, 23);
			this.Resize += new System.EventHandler(this.NumericTextBox_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbNumricText;
	}
}
