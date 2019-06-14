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
            System.Windows.Forms.Panel panel2;
            System.Windows.Forms.Panel panel4;
            this.ui_fileNameTB = new System.Windows.Forms.TextBox();
            this.ui_selFileNameButton = new System.Windows.Forms.Button();
            this.ui_exportTypeAllFeaturesRB = new System.Windows.Forms.RadioButton();
            this.ui_exportTypeSelFeaturesRB = new System.Windows.Forms.RadioButton();
            this.ui_clearList = new System.Windows.Forms.Button();
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ui_FeaturesDGV = new System.Windows.Forms.DataGridView();
            this.ui_colFeatureType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ui_colIdentifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(57, 13);
            label1.TabIndex = 0;
            label1.Text = "File Name:";
            // 
            // panel1
            // 
            panel1.Controls.Add(this.ui_fileNameTB);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(this.ui_selFileNameButton);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(453, 32);
            panel1.TabIndex = 0;
            // 
            // ui_fileNameTB
            // 
            this.ui_fileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_fileNameTB.Location = new System.Drawing.Point(72, 3);
            this.ui_fileNameTB.Name = "ui_fileNameTB";
            this.ui_fileNameTB.ReadOnly = true;
            this.ui_fileNameTB.Size = new System.Drawing.Size(349, 20);
            this.ui_fileNameTB.TabIndex = 1;
            this.ui_fileNameTB.TextChanged += new System.EventHandler(this.FileNameTB_TextChanged);
            // 
            // ui_selFileNameButton
            // 
            this.ui_selFileNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_selFileNameButton.AutoSize = true;
            this.ui_selFileNameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_selFileNameButton.Location = new System.Drawing.Point(424, 1);
            this.ui_selFileNameButton.Name = "ui_selFileNameButton";
            this.ui_selFileNameButton.Size = new System.Drawing.Size(26, 23);
            this.ui_selFileNameButton.TabIndex = 2;
            this.ui_selFileNameButton.Text = "...";
            this.ui_selFileNameButton.UseVisualStyleBackColor = true;
            this.ui_selFileNameButton.Click += new System.EventHandler(this.SelectFile_Click);
            // 
            // panel2
            // 
            panel2.Controls.Add(this.ui_exportTypeAllFeaturesRB);
            panel2.Controls.Add(this.ui_exportTypeSelFeaturesRB);
            panel2.Location = new System.Drawing.Point(3, 41);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(248, 25);
            panel2.TabIndex = 1;
            // 
            // ui_exportTypeAllFeaturesRB
            // 
            this.ui_exportTypeAllFeaturesRB.AutoSize = true;
            this.ui_exportTypeAllFeaturesRB.Checked = true;
            this.ui_exportTypeAllFeaturesRB.Location = new System.Drawing.Point(3, 3);
            this.ui_exportTypeAllFeaturesRB.Name = "ui_exportTypeAllFeaturesRB";
            this.ui_exportTypeAllFeaturesRB.Size = new System.Drawing.Size(80, 17);
            this.ui_exportTypeAllFeaturesRB.TabIndex = 5;
            this.ui_exportTypeAllFeaturesRB.TabStop = true;
            this.ui_exportTypeAllFeaturesRB.Text = "All Features";
            this.ui_exportTypeAllFeaturesRB.UseVisualStyleBackColor = true;
            this.ui_exportTypeAllFeaturesRB.CheckedChanged += new System.EventHandler(this.ExportType_CheckedChanged);
            // 
            // ui_exportTypeSelFeaturesRB
            // 
            this.ui_exportTypeSelFeaturesRB.AutoSize = true;
            this.ui_exportTypeSelFeaturesRB.Location = new System.Drawing.Point(119, 3);
            this.ui_exportTypeSelFeaturesRB.Name = "ui_exportTypeSelFeaturesRB";
            this.ui_exportTypeSelFeaturesRB.Size = new System.Drawing.Size(111, 17);
            this.ui_exportTypeSelFeaturesRB.TabIndex = 6;
            this.ui_exportTypeSelFeaturesRB.TabStop = true;
            this.ui_exportTypeSelFeaturesRB.Text = "Selected Features";
            this.ui_exportTypeSelFeaturesRB.UseVisualStyleBackColor = true;
            this.ui_exportTypeSelFeaturesRB.CheckedChanged += new System.EventHandler(this.ExportType_CheckedChanged);
            // 
            // panel4
            // 
            panel4.Controls.Add(this.ui_clearList);
            panel4.Controls.Add(this.ui_cancelButton);
            panel4.Controls.Add(this.ui_okButton);
            panel4.Location = new System.Drawing.Point(3, 351);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(453, 30);
            panel4.TabIndex = 3;
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
            this.ui_cancelButton.Location = new System.Drawing.Point(375, 3);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.ui_cancelButton.TabIndex = 3;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_okButton
            // 
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(294, 3);
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
            this.flowLayoutPanel1.Controls.Add(panel2);
            this.flowLayoutPanel1.Controls.Add(this.ui_FeaturesDGV);
            this.flowLayoutPanel1.Controls.Add(panel4);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(459, 384);
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
            this.ui_colIdentifier});
            this.ui_FeaturesDGV.Location = new System.Drawing.Point(3, 72);
            this.ui_FeaturesDGV.Name = "ui_FeaturesDGV";
            this.ui_FeaturesDGV.ReadOnly = true;
            this.ui_FeaturesDGV.RowHeadersWidth = 21;
            this.ui_FeaturesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_FeaturesDGV.Size = new System.Drawing.Size(453, 273);
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
            // FeatureExporterForm
            // 
            this.AcceptButton = this.ui_okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(466, 397);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureExporterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Feature Exporter";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_FeaturesDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_fileNameTB;
        private System.Windows.Forms.Button ui_selFileNameButton;
        private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.Button ui_okButton;
        private System.Windows.Forms.RadioButton ui_exportTypeAllFeaturesRB;
        private System.Windows.Forms.RadioButton ui_exportTypeSelFeaturesRB;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DataGridView ui_FeaturesDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colFeatureType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ui_colIdentifier;
        private System.Windows.Forms.Button ui_clearList;
    }
}