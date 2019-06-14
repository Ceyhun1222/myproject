namespace Aran.Queries.Common
{
	partial class DocInfoForm
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
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.ui_infoTB = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.ui_infoTB);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(463, 84);
			this.panel1.TabIndex = 1;
			// 
			// ui_infoTB
			// 
			this.ui_infoTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_infoTB.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.ui_infoTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ui_infoTB.Location = new System.Drawing.Point(7, 5);
			this.ui_infoTB.Multiline = true;
			this.ui_infoTB.Name = "ui_infoTB";
			this.ui_infoTB.ReadOnly = true;
			this.ui_infoTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ui_infoTB.Size = new System.Drawing.Size(453, 72);
			this.ui_infoTB.TabIndex = 1;
			this.ui_infoTB.Text = "Property Description";
			// 
			// DocInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.ClientSize = new System.Drawing.Size(463, 84);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DocInfoForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "DocInfoForm";
			this.Deactivate += new System.EventHandler(this.DocInfoForm_Deactivate);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox ui_infoTB;

	}
}