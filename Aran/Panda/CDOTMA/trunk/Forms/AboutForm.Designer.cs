namespace CDOTMA
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.Button1 = new System.Windows.Forms.Button();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblCopyRight = new System.Windows.Forms.Label();
			this.lblVersionDate = new System.Windows.Forms.Label();
			this.lbllVersion = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// Button1
			// 
			this.Button1.Location = new System.Drawing.Point(347, 91);
			this.Button1.Name = "Button1";
			this.Button1.Size = new System.Drawing.Size(75, 23);
			this.Button1.TabIndex = 17;
			this.Button1.Text = "&OK";
			this.Button1.UseVisualStyleBackColor = true;
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// PictureBox1
			// 
			this.PictureBox1.BackColor = System.Drawing.SystemColors.Window;
			this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
			this.PictureBox1.Location = new System.Drawing.Point(12, 10);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(197, 43);
			this.PictureBox1.TabIndex = 12;
			this.PictureBox1.TabStop = false;
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(9, 74);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(60, 13);
			this.lblDescription.TabIndex = 13;
			this.lblDescription.Text = "Description";
			// 
			// lblCopyRight
			// 
			this.lblCopyRight.AutoSize = true;
			this.lblCopyRight.Location = new System.Drawing.Point(9, 96);
			this.lblCopyRight.Name = "lblCopyRight";
			this.lblCopyRight.Size = new System.Drawing.Size(51, 13);
			this.lblCopyRight.TabIndex = 16;
			this.lblCopyRight.Text = "Copyright";
			// 
			// lblVersionDate
			// 
			this.lblVersionDate.AutoSize = true;
			this.lblVersionDate.Location = new System.Drawing.Point(241, 40);
			this.lblVersionDate.Name = "lblVersionDate";
			this.lblVersionDate.Size = new System.Drawing.Size(66, 13);
			this.lblVersionDate.TabIndex = 15;
			this.lblVersionDate.Text = "Version date";
			// 
			// lbllVersion
			// 
			this.lbllVersion.AutoSize = true;
			this.lbllVersion.Location = new System.Drawing.Point(241, 10);
			this.lbllVersion.Name = "lbllVersion";
			this.lbllVersion.Size = new System.Drawing.Size(42, 13);
			this.lbllVersion.TabIndex = 14;
			this.lbllVersion.Text = "Version";
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(430, 125);
			this.Controls.Add(this.Button1);
			this.Controls.Add(this.PictureBox1);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblCopyRight);
			this.Controls.Add(this.lblVersionDate);
			this.Controls.Add(this.lbllVersion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.Button Button1;
		internal System.Windows.Forms.PictureBox PictureBox1;
		internal System.Windows.Forms.Label lblDescription;
		internal System.Windows.Forms.Label lblCopyRight;
		internal System.Windows.Forms.Label lblVersionDate;
		internal System.Windows.Forms.Label lbllVersion;
	}
}