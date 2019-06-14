namespace AControls
{
	partial class CheckBoxList
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose ( );
			}
			base.Dispose ( disposing );
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ( )
		{
			this.chckBxAll = new System.Windows.Forms.CheckBox();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtBxSearch = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// chckBxAll
			// 
			this.chckBxAll.AutoSize = true;
			this.chckBxAll.Location = new System.Drawing.Point(5, 25);
			this.chckBxAll.Name = "chckBxAll";
			this.chckBxAll.Size = new System.Drawing.Size(15, 14);
			this.chckBxAll.TabIndex = 9;
			this.chckBxAll.UseVisualStyleBackColor = true;
			this.chckBxAll.CheckedChanged += new System.EventHandler(this.chckBxAll_CheckedChanged);
			// 
			// dataGridView
			// 
			this.dataGridView.AllowUserToAddRows = false;
			this.dataGridView.AllowUserToDeleteRows = false;
			this.dataGridView.AllowUserToOrderColumns = true;
			this.dataGridView.AllowUserToResizeColumns = false;
			this.dataGridView.AllowUserToResizeRows = false;
			this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.dataGridView.Location = new System.Drawing.Point(0, 22);
			this.dataGridView.MultiSelect = false;
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowHeadersVisible = false;
			this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView.Size = new System.Drawing.Size(180, 68);
			this.dataGridView.TabIndex = 8;
			this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
			this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_RowEnter);
			// 
			// Column1
			// 
			this.Column1.HeaderText = "";
			this.Column1.Name = "Column1";
			this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.Column1.Width = 20;
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column2.HeaderText = "Items";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			// 
			// txtBxSearch
			// 
			this.txtBxSearch.AllowDrop = true;
			this.txtBxSearch.BackColor = System.Drawing.SystemColors.Window;
			this.txtBxSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtBxSearch.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.txtBxSearch.Location = new System.Drawing.Point(0, 0);
			this.txtBxSearch.Name = "txtBxSearch";
			this.txtBxSearch.Size = new System.Drawing.Size(180, 20);
			this.txtBxSearch.TabIndex = 11;
			this.txtBxSearch.Text = "Search";
			this.txtBxSearch.TextChanged += new System.EventHandler(this.txtBxSearch_TextChanged);
			this.txtBxSearch.Enter += new System.EventHandler(this.txtBxSearch_Enter);
			this.txtBxSearch.Leave += new System.EventHandler(this.txtBxSearch_Leave);
			// 
			// CheckBoxList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chckBxAll);
			this.Controls.Add(this.txtBxSearch);
			this.Controls.Add(this.dataGridView);
			this.MinimumSize = new System.Drawing.Size(180, 90);
			this.Name = "CheckBoxList";
			this.Size = new System.Drawing.Size(180, 90);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chckBxAll;
		private System.Windows.Forms.DataGridView dataGridView;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.TextBox txtBxSearch;
	}
}
