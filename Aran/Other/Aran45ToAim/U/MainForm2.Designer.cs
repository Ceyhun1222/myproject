namespace Aran45ToAixm
{
    partial class MainForm2
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
			this.ui_openMdbFileButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.ui_mdbFileTB = new System.Windows.Forms.TextBox();
			this.ui_featuresDGV = new System.Windows.Forms.DataGridView();
			this.ui_colName51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_colName45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ui_convertButton = new System.Windows.Forms.Button();
			this.ui_checkCRCChB = new System.Windows.Forms.CheckBox();
			this.ui_progressBar = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.ui_featuresDGV)).BeginInit();
			this.SuspendLayout();
			// 
			// ui_openMdbFileButton
			// 
			this.ui_openMdbFileButton.Location = new System.Drawing.Point(3, 3);
			this.ui_openMdbFileButton.Name = "ui_openMdbFileButton";
			this.ui_openMdbFileButton.Size = new System.Drawing.Size(112, 23);
			this.ui_openMdbFileButton.TabIndex = 13;
			this.ui_openMdbFileButton.Text = "Open MDB File";
			this.ui_openMdbFileButton.UseVisualStyleBackColor = true;
			this.ui_openMdbFileButton.Click += new System.EventHandler(this.OpenMdbFile_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 359);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "MDB File:";
			// 
			// ui_mdbFileTB
			// 
			this.ui_mdbFileTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_mdbFileTB.Location = new System.Drawing.Point(3, 375);
			this.ui_mdbFileTB.Name = "ui_mdbFileTB";
			this.ui_mdbFileTB.ReadOnly = true;
			this.ui_mdbFileTB.Size = new System.Drawing.Size(503, 20);
			this.ui_mdbFileTB.TabIndex = 15;
			// 
			// ui_featuresDGV
			// 
			this.ui_featuresDGV.AllowUserToAddRows = false;
			this.ui_featuresDGV.AllowUserToDeleteRows = false;
			this.ui_featuresDGV.AllowUserToResizeColumns = false;
			this.ui_featuresDGV.AllowUserToResizeRows = false;
			this.ui_featuresDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_featuresDGV.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ui_featuresDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ui_featuresDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colName51,
            this.ui_colName45,
            this.ui_colCount});
			this.ui_featuresDGV.Location = new System.Drawing.Point(3, 32);
			this.ui_featuresDGV.MultiSelect = false;
			this.ui_featuresDGV.Name = "ui_featuresDGV";
			this.ui_featuresDGV.ReadOnly = true;
			this.ui_featuresDGV.RowHeadersVisible = false;
			this.ui_featuresDGV.RowTemplate.Height = 30;
			this.ui_featuresDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ui_featuresDGV.Size = new System.Drawing.Size(503, 322);
			this.ui_featuresDGV.TabIndex = 16;
			// 
			// ui_colName51
			// 
			this.ui_colName51.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ui_colName51.HeaderText = "Name (5.1)";
			this.ui_colName51.Name = "ui_colName51";
			this.ui_colName51.ReadOnly = true;
			// 
			// ui_colName45
			// 
			this.ui_colName45.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ui_colName45.HeaderText = "Name (4.5)";
			this.ui_colName45.Name = "ui_colName45";
			this.ui_colName45.ReadOnly = true;
			// 
			// ui_colCount
			// 
			this.ui_colCount.HeaderText = "Row Count";
			this.ui_colCount.Name = "ui_colCount";
			this.ui_colCount.ReadOnly = true;
			this.ui_colCount.Width = 120;
			// 
			// ui_convertButton
			// 
			this.ui_convertButton.Location = new System.Drawing.Point(121, 3);
			this.ui_convertButton.Name = "ui_convertButton";
			this.ui_convertButton.Size = new System.Drawing.Size(112, 23);
			this.ui_convertButton.TabIndex = 17;
			this.ui_convertButton.Text = "Convert";
			this.ui_convertButton.UseVisualStyleBackColor = true;
			this.ui_convertButton.Click += new System.EventHandler(this.Convert_Click);
			// 
			// ui_checkCRCChB
			// 
			this.ui_checkCRCChB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_checkCRCChB.AutoSize = true;
			this.ui_checkCRCChB.Checked = true;
			this.ui_checkCRCChB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ui_checkCRCChB.Location = new System.Drawing.Point(424, 9);
			this.ui_checkCRCChB.Name = "ui_checkCRCChB";
			this.ui_checkCRCChB.Size = new System.Drawing.Size(82, 17);
			this.ui_checkCRCChB.TabIndex = 18;
			this.ui_checkCRCChB.Text = "Check CRC";
			this.ui_checkCRCChB.UseVisualStyleBackColor = true;
			// 
			// ui_progressBar
			// 
			this.ui_progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_progressBar.Location = new System.Drawing.Point(3, 401);
			this.ui_progressBar.Name = "ui_progressBar";
			this.ui_progressBar.Size = new System.Drawing.Size(503, 23);
			this.ui_progressBar.TabIndex = 19;
			this.ui_progressBar.Visible = false;
			// 
			// MainForm2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(518, 427);
			this.Controls.Add(this.ui_progressBar);
			this.Controls.Add(this.ui_checkCRCChB);
			this.Controls.Add(this.ui_convertButton);
			this.Controls.Add(this.ui_featuresDGV);
			this.Controls.Add(this.ui_mdbFileTB);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ui_openMdbFileButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm2";
			this.Text = "Aran 4.5 to AIM 5.1";
			((System.ComponentModel.ISupportInitialize)(this.ui_featuresDGV)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_openMdbFileButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ui_mdbFileTB;
		private System.Windows.Forms.DataGridView ui_featuresDGV;
		private System.Windows.Forms.Button ui_convertButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName51;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colName45;
		private System.Windows.Forms.DataGridViewTextBoxColumn ui_colCount;
		private System.Windows.Forms.CheckBox ui_checkCRCChB;
        private System.Windows.Forms.ProgressBar ui_progressBar;
    }
}