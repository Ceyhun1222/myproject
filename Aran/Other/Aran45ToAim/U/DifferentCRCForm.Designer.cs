namespace Aran45ToAixm.U
{
	partial class DifferentCRCForm
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
			this.ui_dgv = new System.Windows.Forms.DataGridView();
			this.ui_continueButton = new System.Windows.Forms.Button();
			this.ui_stopButton = new System.Windows.Forms.Button();
			this.ui_saveReportButton = new System.Windows.Forms.Button();
			this.ui_colMid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_colSourceCRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_newCRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// ui_dgv
			// 
			this.ui_dgv.AllowUserToAddRows = false;
			this.ui_dgv.AllowUserToDeleteRows = false;
			this.ui_dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ui_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colMid,
            this.ui_colName,
            this.ui_colSourceCRC,
            this.ui_newCRC});
			this.ui_dgv.Location = new System.Drawing.Point(6, 12);
			this.ui_dgv.MultiSelect = false;
			this.ui_dgv.Name = "ui_dgv";
			this.ui_dgv.ReadOnly = true;
			this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ui_dgv.Size = new System.Drawing.Size(586, 373);
			this.ui_dgv.TabIndex = 0;
			// 
			// ui_continueButton
			// 
			this.ui_continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ui_continueButton.Location = new System.Drawing.Point(6, 391);
			this.ui_continueButton.Name = "ui_continueButton";
			this.ui_continueButton.Size = new System.Drawing.Size(178, 23);
			this.ui_continueButton.TabIndex = 1;
			this.ui_continueButton.Text = "Continue without these Features";
			this.ui_continueButton.UseVisualStyleBackColor = true;
			this.ui_continueButton.Click += new System.EventHandler(this.Continue_Click);
			// 
			// ui_stopButton
			// 
			this.ui_stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_stopButton.Location = new System.Drawing.Point(484, 391);
			this.ui_stopButton.Name = "ui_stopButton";
			this.ui_stopButton.Size = new System.Drawing.Size(106, 23);
			this.ui_stopButton.TabIndex = 2;
			this.ui_stopButton.Text = "Stop Converting";
			this.ui_stopButton.UseVisualStyleBackColor = true;
			this.ui_stopButton.Click += new System.EventHandler(this.Stop_Click);
			// 
			// ui_saveReportButton
			// 
			this.ui_saveReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_saveReportButton.Location = new System.Drawing.Point(372, 391);
			this.ui_saveReportButton.Name = "ui_saveReportButton";
			this.ui_saveReportButton.Size = new System.Drawing.Size(106, 23);
			this.ui_saveReportButton.TabIndex = 3;
			this.ui_saveReportButton.Text = "Save Report";
			this.ui_saveReportButton.UseVisualStyleBackColor = true;
			this.ui_saveReportButton.Click += new System.EventHandler(this.SaveReport_Click);
			// 
			// ui_colMid
			// 
			this.ui_colMid.HeaderText = "MID";
			this.ui_colMid.Name = "ui_colMid";
			this.ui_colMid.ReadOnly = true;
			this.ui_colMid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ui_colName
			// 
			this.ui_colName.HeaderText = "Name";
			this.ui_colName.Name = "ui_colName";
			this.ui_colName.ReadOnly = true;
			this.ui_colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// ui_colSourceCRC
			// 
			this.ui_colSourceCRC.HeaderText = "Source CRC";
			this.ui_colSourceCRC.Name = "ui_colSourceCRC";
			this.ui_colSourceCRC.ReadOnly = true;
			this.ui_colSourceCRC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ui_colSourceCRC.Width = 160;
			// 
			// ui_newCRC
			// 
			this.ui_newCRC.HeaderText = "CRC";
			this.ui_newCRC.Name = "ui_newCRC";
			this.ui_newCRC.ReadOnly = true;
			this.ui_newCRC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ui_newCRC.Width = 160;
			// 
			// DifferentCRCForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 421);
			this.Controls.Add(this.ui_saveReportButton);
			this.Controls.Add(this.ui_stopButton);
			this.Controls.Add(this.ui_continueButton);
			this.Controls.Add(this.ui_dgv);
			this.MaximizeBox = false;
			this.Name = "DifferentCRCForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CRC Validation";
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView ui_dgv;
		private System.Windows.Forms.Button ui_continueButton;
		private System.Windows.Forms.Button ui_stopButton;
		private System.Windows.Forms.Button ui_saveReportButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colMid;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colSourceCRC;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_newCRC;
	}
}