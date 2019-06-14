namespace Aran.PANDA.RNAV.SGBAS
{
	partial class InfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.lblInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtInfo
			// 
			this.txtInfo.BackColor = System.Drawing.SystemColors.Info;
			this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtInfo.Location = new System.Drawing.Point(1, 1);
			this.txtInfo.Multiline = true;
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ReadOnly = true;
			this.txtInfo.Size = new System.Drawing.Size(273, 182);
			this.txtInfo.TabIndex = 3;
			this.txtInfo.WordWrap = false;
			// 
			// lblInfo
			// 
			this.lblInfo.AutoSize = true;
			this.lblInfo.BackColor = System.Drawing.Color.Transparent;
			this.lblInfo.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblInfo.Location = new System.Drawing.Point(1, 1);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblInfo.Size = new System.Drawing.Size(0, 13);
			this.lblInfo.TabIndex = 2;
			this.lblInfo.Visible = false;
			// 
			// InfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.ClientSize = new System.Drawing.Size(304, 194);
			this.Controls.Add(this.txtInfo);
			this.Controls.Add(this.lblInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InfoForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Info";
			this.Deactivate += new System.EventHandler(this.InfoForm_Deactivate);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtInfo;
		private System.Windows.Forms.Label lblInfo;

	}
}