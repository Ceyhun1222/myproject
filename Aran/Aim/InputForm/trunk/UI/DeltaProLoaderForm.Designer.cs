namespace Aran.Aim.InputForm
{
	partial class DeltaProLoaderForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.Windows.Forms.Button button1;
			System.Windows.Forms.Button button2;
			this.ui_dgv = new System.Windows.Forms.DataGridView();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			button1.Location = new System.Drawing.Point(426, 376);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(93, 25);
			button1.TabIndex = 1;
			button1.Text = "&OK";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(this.OK_Click);
			// 
			// button2
			// 
			button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			button2.Location = new System.Drawing.Point(525, 376);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(93, 25);
			button2.TabIndex = 2;
			button2.Text = "&Cancel";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// ui_dgv
			// 
			this.ui_dgv.AllowUserToAddRows = false;
			this.ui_dgv.AllowUserToDeleteRows = false;
			this.ui_dgv.AllowUserToOrderColumns = true;
			this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ui_dgv.Location = new System.Drawing.Point(5, 5);
			this.ui_dgv.MultiSelect = false;
			this.ui_dgv.Name = "ui_dgv";
			this.ui_dgv.ReadOnly = true;
			this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ui_dgv.Size = new System.Drawing.Size(613, 365);
			this.ui_dgv.TabIndex = 0;
			// 
			// DeltaProLoaderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(625, 408);
			this.Controls.Add(button2);
			this.Controls.Add(button1);
			this.Controls.Add(this.ui_dgv);
			this.MaximizeBox = false;
			this.Name = "DeltaProLoaderForm";
			this.ShowIcon = false;
			this.Text = "Delta Project File Loader";
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView ui_dgv;
	}
}