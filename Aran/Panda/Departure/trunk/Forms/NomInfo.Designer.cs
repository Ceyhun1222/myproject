using System;
using System.Windows.Forms;

namespace Aran.PANDA.Departure
{
	partial class CNomInfo
	{
		[System.Diagnostics.DebuggerNonUserCode()]
		public CNomInfo()
			: base()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// Form overrides dispose to clean up the component list.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();
			
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public ToolTip ToolTip1;
		public TextBox Text1;
		public TextBox Text2;
		public Label Label2;
		public Label Label3;
		public Label Label4;
		public Label Label5;
		public GroupBox Frame2;

		#region '"Windows Form Designer generated code "'

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// NOTE: The following procedure is required by the Windows Form Designer
		/// It can be modified using the Windows Form Designer.
		/// Do not modify it using the code editor.
		/// </summary>


		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CNomInfo));
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.Frame2 = new System.Windows.Forms.GroupBox();
			this.Text1 = new System.Windows.Forms.TextBox();
			this.Text2 = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.Frame2.SuspendLayout();
			this.SuspendLayout();
			// 
			// Frame2
			// 
			this.Frame2.Controls.Add(this.Text1);
			this.Frame2.Controls.Add(this.Text2);
			this.Frame2.Controls.Add(this.Label2);
			this.Frame2.Controls.Add(this.Label3);
			this.Frame2.Controls.Add(this.Label4);
			this.Frame2.Controls.Add(this.Label5);
			this.Frame2.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Frame2.Location = new System.Drawing.Point(0, 0);
			this.Frame2.Name = "Frame2";
			this.Frame2.Padding = new System.Windows.Forms.Padding(0);
			this.Frame2.Size = new System.Drawing.Size(217, 86);
			this.Frame2.TabIndex = 0;
			this.Frame2.TabStop = false;
			//
			//Text1
			//
			this.Text1.AcceptsReturn = true;

			this.Text1.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Text1.Location = new System.Drawing.Point(106, 21);
			this.Text1.MaxLength = 0;
			this.Text1.Name = "Text1";
			this.Text1.ReadOnly = true;
			this.Text1.Size = new System.Drawing.Size(67, 20);
			this.Text1.TabIndex = 2;
			//
			//Text2
			//
			this.Text2.AcceptsReturn = true;

			this.Text2.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Text2.Location = new System.Drawing.Point(106, 52);
			this.Text2.MaxLength = 0;
			this.Text2.Name = "Text2";
			this.Text2.ReadOnly = true;
			this.Text2.Size = new System.Drawing.Size(67, 20);
			this.Text2.TabIndex = 1;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label2.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label2.Location = new System.Drawing.Point(12, 24);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(96, 14);
			this.Label2.TabIndex = 6;
			this.Label2.Text = "Nom. track length :";
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label3.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label3.Location = new System.Drawing.Point(12, 53);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(93, 14);
			this.Label3.TabIndex = 5;
			this.Label3.Text = "Reached altitude :";
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label4.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label4.Location = new System.Drawing.Point(182, 22);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(20, 14);
			this.Label4.TabIndex = 4;
			this.Label4.Text = "km";
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
			this.Label5.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label5.Location = new System.Drawing.Point(182, 53);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(26, 14);
			this.Label5.TabIndex = 3;
			this.Label5.Text = "feet";
			//
			//CNomInfo
			//
			this.Deactivate += new System.EventHandler(CNomInfoForm_Deactivate);
			this.ClientSize = new System.Drawing.Size(220, 95);
			this.Controls.Add(this.Frame2);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Font = new System.Drawing.Font("Arial", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Name = "CNomInfo";
			this.ShowInTaskbar = false;
			this.Text = "Form1";
			this.Frame2.ResumeLayout(false);
			this.Frame2.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion
	}
}

 
