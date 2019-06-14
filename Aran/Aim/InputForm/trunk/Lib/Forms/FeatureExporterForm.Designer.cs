namespace Aran.Aim.InputFormLib
{
    partial class FeatureExporterForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Panel panel4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.GroupBox groupBox1;
            this.ui_write3DisExistsChB = new System.Windows.Forms.CheckBox();
            this.ui_effectiveDateTB = new System.Windows.Forms.TextBox();
            this.ui_loadFeatAllVersion = new System.Windows.Forms.CheckBox();
            this.chckBxIncludeFeatRefs = new System.Windows.Forms.CheckBox();
            this.ui_writeExtensionChB = new System.Windows.Forms.CheckBox();
            this.ui_fileNameTB = new System.Windows.Forms.TextBox();
            this.ui_selFileNameButton = new System.Windows.Forms.Button();
            this.ui_expAsSeperateButton = new System.Windows.Forms.Button();
            this.ui_clearList = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_FeaturesDGV = new System.Windows.Forms.DataGridView();
            this.ui_colFeatureType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colIdentifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_srsNameCB = new System.Windows.Forms.ComboBox();
            this.ui_exportTypeSelFeaturesRB = new System.Windows.Forms.RadioButton();
            this.ui_exportTypeAllFeaturesRB = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            panel4 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).BeginInit();
            groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(26, 5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(57, 13);
            label1.TabIndex = 0;
            label1.Text = "File Name:";
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(this.ui_srsNameCB);
            panel1.Controls.Add(this.ui_write3DisExistsChB);
            panel1.Controls.Add(this.ui_effectiveDateTB);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(this.ui_loadFeatAllVersion);
            panel1.Controls.Add(this.chckBxIncludeFeatRefs);
            panel1.Controls.Add(this.ui_writeExtensionChB);
            panel1.Controls.Add(this.ui_fileNameTB);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(this.ui_selFileNameButton);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(647, 97);
            panel1.TabIndex = 0;
            // 
            // ui_write3DisExistsChB
            // 
            this.ui_write3DisExistsChB.AutoSize = true;
            this.ui_write3DisExistsChB.Checked = true;
            this.ui_write3DisExistsChB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_write3DisExistsChB.Location = new System.Drawing.Point(482, 52);
            this.ui_write3DisExistsChB.Name = "ui_write3DisExistsChB";
            this.ui_write3DisExistsChB.Size = new System.Drawing.Size(158, 17);
            this.ui_write3DisExistsChB.TabIndex = 8;
            this.ui_write3DisExistsChB.Text = "Write 3D coordinate if exists";
            this.ui_write3DisExistsChB.UseVisualStyleBackColor = true;
            // 
            // ui_effectiveDateTB
            // 
            this.ui_effectiveDateTB.Location = new System.Drawing.Point(89, 29);
            this.ui_effectiveDateTB.Name = "ui_effectiveDateTB";
            this.ui_effectiveDateTB.ReadOnly = true;
            this.ui_effectiveDateTB.Size = new System.Drawing.Size(175, 20);
            this.ui_effectiveDateTB.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(5, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 13);
            label2.TabIndex = 6;
            label2.Text = "Effective Date:";
            // 
            // ui_loadFeatAllVersion
            // 
            this.ui_loadFeatAllVersion.AutoSize = true;
            this.ui_loadFeatAllVersion.Checked = true;
            this.ui_loadFeatAllVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_loadFeatAllVersion.Location = new System.Drawing.Point(482, 75);
            this.ui_loadFeatAllVersion.Name = "ui_loadFeatAllVersion";
            this.ui_loadFeatAllVersion.Size = new System.Drawing.Size(141, 17);
            this.ui_loadFeatAllVersion.TabIndex = 5;
            this.ui_loadFeatAllVersion.Text = "Load Feature All Version";
            this.ui_loadFeatAllVersion.UseVisualStyleBackColor = true;
            // 
            // chckBxIncludeFeatRefs
            // 
            this.chckBxIncludeFeatRefs.AutoSize = true;
            this.chckBxIncludeFeatRefs.Location = new System.Drawing.Point(482, 29);
            this.chckBxIncludeFeatRefs.Name = "chckBxIncludeFeatRefs";
            this.chckBxIncludeFeatRefs.Size = new System.Drawing.Size(137, 17);
            this.chckBxIncludeFeatRefs.TabIndex = 4;
            this.chckBxIncludeFeatRefs.Text = "Include related features";
            this.chckBxIncludeFeatRefs.UseVisualStyleBackColor = true;
            // 
            // ui_writeExtensionChB
            // 
            this.ui_writeExtensionChB.AutoSize = true;
            this.ui_writeExtensionChB.Checked = true;
            this.ui_writeExtensionChB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ui_writeExtensionChB.Location = new System.Drawing.Point(482, 8);
            this.ui_writeExtensionChB.Name = "ui_writeExtensionChB";
            this.ui_writeExtensionChB.Size = new System.Drawing.Size(108, 17);
            this.ui_writeExtensionChB.TabIndex = 3;
            this.ui_writeExtensionChB.Text = "Writer Extensions";
            this.ui_writeExtensionChB.UseVisualStyleBackColor = true;
            // 
            // ui_fileNameTB
            // 
            this.ui_fileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_fileNameTB.Location = new System.Drawing.Point(89, 1);
            this.ui_fileNameTB.Name = "ui_fileNameTB";
            this.ui_fileNameTB.ReadOnly = true;
            this.ui_fileNameTB.Size = new System.Drawing.Size(345, 20);
            this.ui_fileNameTB.TabIndex = 1;
            this.ui_fileNameTB.TextChanged += new System.EventHandler(this.FileNameTB_TextChanged);
            // 
            // ui_selFileNameButton
            // 
            this.ui_selFileNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_selFileNameButton.AutoSize = true;
            this.ui_selFileNameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_selFileNameButton.Location = new System.Drawing.Point(438, 0);
            this.ui_selFileNameButton.Name = "ui_selFileNameButton";
            this.ui_selFileNameButton.Size = new System.Drawing.Size(26, 23);
            this.ui_selFileNameButton.TabIndex = 2;
            this.ui_selFileNameButton.Text = "...";
            this.ui_selFileNameButton.UseVisualStyleBackColor = true;
            this.ui_selFileNameButton.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // panel4
            // 
            panel4.Controls.Add(this.ui_expAsSeperateButton);
            panel4.Controls.Add(this.ui_clearList);
            panel4.Controls.Add(this.ui_cancelButton);
            panel4.Controls.Add(this.ui_okButton);
            panel4.Location = new System.Drawing.Point(3, 405);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(647, 30);
            panel4.TabIndex = 3;
            // 
            // ui_expAsSeperateButton
            // 
            this.ui_expAsSeperateButton.Location = new System.Drawing.Point(89, 3);
            this.ui_expAsSeperateButton.Name = "ui_expAsSeperateButton";
            this.ui_expAsSeperateButton.Size = new System.Drawing.Size(130, 23);
            this.ui_expAsSeperateButton.TabIndex = 6;
            this.ui_expAsSeperateButton.Text = "Export as seperate file";
            this.ui_expAsSeperateButton.UseVisualStyleBackColor = true;
            this.ui_expAsSeperateButton.Visible = false;
            this.ui_expAsSeperateButton.Click += new System.EventHandler(this.ExportAsSeperate_Click);
            // 
            // ui_clearList
            // 
            this.ui_clearList.Location = new System.Drawing.Point(0, 3);
            this.ui_clearList.Name = "ui_clearList";
            this.ui_clearList.Size = new System.Drawing.Size(83, 23);
            this.ui_clearList.TabIndex = 5;
            this.ui_clearList.Text = "Clear List";
            this.ui_clearList.UseVisualStyleBackColor = true;
            this.ui_clearList.Visible = false;
            this.ui_clearList.Click += new System.EventHandler(this.ClearList_Click);
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(564, 3);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 3;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(483, 3);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(75, 23);
            this.ui_okButton.TabIndex = 4;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(panel1);
            this.flowLayoutPanel1.Controls.Add(this.ui_FeaturesDGV);
            this.flowLayoutPanel1.Controls.Add(panel4);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(653, 438);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ui_FeaturesDGV
            // 
            this.ui_FeaturesDGV.AllowUserToAddRows = false;
            this.ui_FeaturesDGV.AllowUserToDeleteRows = false;
            this.ui_FeaturesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_FeaturesDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_FeaturesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_FeaturesDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ui_colFeatureType,
            this.ui_colIdentifier,
            this.ui_colDesc});
            this.ui_FeaturesDGV.Location = new System.Drawing.Point(3, 106);
            this.ui_FeaturesDGV.Name = "ui_FeaturesDGV";
            this.ui_FeaturesDGV.ReadOnly = true;
            this.ui_FeaturesDGV.RowHeadersWidth = 21;
            this.ui_FeaturesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_FeaturesDGV.Size = new System.Drawing.Size(647, 293);
            this.ui_FeaturesDGV.TabIndex = 11;
            this.ui_FeaturesDGV.Visible = false;
            // 
            // ui_colFeatureType
            // 
            this.ui_colFeatureType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colFeatureType.HeaderText = "Feature Type";
            this.ui_colFeatureType.Name = "ui_colFeatureType";
            this.ui_colFeatureType.ReadOnly = true;
            // 
            // ui_colIdentifier
            // 
            this.ui_colIdentifier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colIdentifier.HeaderText = "Identifier";
            this.ui_colIdentifier.Name = "ui_colIdentifier";
            this.ui_colIdentifier.ReadOnly = true;
            // 
            // ui_colDesc
            // 
            this.ui_colDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ui_colDesc.HeaderText = "Description";
            this.ui_colDesc.Name = "ui_colDesc";
            this.ui_colDesc.ReadOnly = true;
            // 
            // ui_srsNameCB
            // 
            this.ui_srsNameCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ui_srsNameCB.FormattingEnabled = true;
            this.ui_srsNameCB.Location = new System.Drawing.Point(89, 60);
            this.ui_srsNameCB.Name = "ui_srsNameCB";
            this.ui_srsNameCB.Size = new System.Drawing.Size(175, 21);
            this.ui_srsNameCB.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(32, 64);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(51, 13);
            label3.TabIndex = 10;
            label3.Text = "srsName:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.ui_exportTypeAllFeaturesRB);
            groupBox1.Controls.Add(this.ui_exportTypeSelFeaturesRB);
            groupBox1.Location = new System.Drawing.Point(298, 21);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(134, 63);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            // 
            // ui_exportTypeSelFeaturesRB
            // 
            this.ui_exportTypeSelFeaturesRB.AutoSize = true;
            this.ui_exportTypeSelFeaturesRB.Location = new System.Drawing.Point(13, 37);
            this.ui_exportTypeSelFeaturesRB.Name = "ui_exportTypeSelFeaturesRB";
            this.ui_exportTypeSelFeaturesRB.Size = new System.Drawing.Size(111, 17);
            this.ui_exportTypeSelFeaturesRB.TabIndex = 6;
            this.ui_exportTypeSelFeaturesRB.TabStop = true;
            this.ui_exportTypeSelFeaturesRB.Text = "Selected Features";
            this.ui_exportTypeSelFeaturesRB.UseVisualStyleBackColor = true;
            this.ui_exportTypeSelFeaturesRB.CheckedChanged += new System.EventHandler(this.ExportType_CheckedChanged);
            // 
            // ui_exportTypeAllFeaturesRB
            // 
            this.ui_exportTypeAllFeaturesRB.AutoSize = true;
            this.ui_exportTypeAllFeaturesRB.Checked = true;
            this.ui_exportTypeAllFeaturesRB.Location = new System.Drawing.Point(13, 14);
            this.ui_exportTypeAllFeaturesRB.Name = "ui_exportTypeAllFeaturesRB";
            this.ui_exportTypeAllFeaturesRB.Size = new System.Drawing.Size(80, 17);
            this.ui_exportTypeAllFeaturesRB.TabIndex = 5;
            this.ui_exportTypeAllFeaturesRB.TabStop = true;
            this.ui_exportTypeAllFeaturesRB.Text = "All Features";
            this.ui_exportTypeAllFeaturesRB.UseVisualStyleBackColor = true;
            this.ui_exportTypeAllFeaturesRB.CheckedChanged += new System.EventHandler(this.ExportType_CheckedChanged);
            // 
            // FeatureExporterForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(659, 445);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureExporterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feature Exporter";
            this.Load += new System.EventHandler(this.FeatureExporterForm_Load);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_fileNameTB;
        private System.Windows.Forms.Button ui_selFileNameButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridView ui_FeaturesDGV;
        private System.Windows.Forms.Button ui_clearList;
        private System.Windows.Forms.CheckBox ui_writeExtensionChB;
		private System.Windows.Forms.CheckBox chckBxIncludeFeatRefs;
		private System.Windows.Forms.CheckBox ui_loadFeatAllVersion;
        private System.Windows.Forms.Button ui_expAsSeperateButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colFeatureType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colIdentifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colDesc;
        private System.Windows.Forms.TextBox ui_effectiveDateTB;
        private System.Windows.Forms.CheckBox ui_write3DisExistsChB;
        private System.Windows.Forms.ComboBox ui_srsNameCB;
        private System.Windows.Forms.RadioButton ui_exportTypeAllFeaturesRB;
        private System.Windows.Forms.RadioButton ui_exportTypeSelFeaturesRB;
    }
}