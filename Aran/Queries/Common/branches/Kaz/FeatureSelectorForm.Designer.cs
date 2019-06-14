namespace Aran.Queries.Common
{
    partial class FeatureSelectorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSelectorForm));
			this.ui_okButton = new System.Windows.Forms.Button();
			this.ui_cancelButton = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.ui_mainPanel = new System.Windows.Forms.Panel();
			this.ui_dgv = new System.Windows.Forms.DataGridView();
			this.ui_topPanel = new System.Windows.Forms.Panel();
			this.ui_featureTypeComboBox = new System.Windows.Forms.ComboBox();
			this.ui_featureTypeLabel = new System.Windows.Forms.Label();
			this.btnBack = new System.Windows.Forms.Button();
			this.ui_mainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
			this.ui_topPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ui_okButton
			// 
			this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_okButton.Enabled = false;
			this.ui_okButton.Image = ((System.Drawing.Image)(resources.GetObject("ui_okButton.Image")));
			this.ui_okButton.Location = new System.Drawing.Point(310, 321);
			this.ui_okButton.Name = "ui_okButton";
			this.ui_okButton.Size = new System.Drawing.Size(65, 28);
			this.ui_okButton.TabIndex = 0;
			this.ui_okButton.UseVisualStyleBackColor = true;
			this.ui_okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// ui_cancelButton
			// 
			this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ui_cancelButton.Location = new System.Drawing.Point(381, 321);
			this.ui_cancelButton.Name = "ui_cancelButton";
			this.ui_cancelButton.Size = new System.Drawing.Size(65, 28);
			this.ui_cancelButton.TabIndex = 1;
			this.ui_cancelButton.Text = "Cancel";
			this.ui_cancelButton.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
			this.btnNext.Location = new System.Drawing.Point(192, 311);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(65, 28);
			this.btnNext.TabIndex = 5;
			this.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnNext.UseCompatibleTextRendering = true;
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// ui_mainPanel
			// 
			this.ui_mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ui_mainPanel.Controls.Add(this.ui_dgv);
			this.ui_mainPanel.Controls.Add(this.ui_topPanel);
			this.ui_mainPanel.Location = new System.Drawing.Point(3, 10);
			this.ui_mainPanel.Name = "ui_mainPanel";
			this.ui_mainPanel.Size = new System.Drawing.Size(443, 307);
			this.ui_mainPanel.TabIndex = 4;
			// 
			// ui_dgv
			// 
			this.ui_dgv.AllowUserToAddRows = false;
			this.ui_dgv.AllowUserToDeleteRows = false;
			this.ui_dgv.AllowUserToResizeRows = false;
			this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ui_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ui_dgv.Location = new System.Drawing.Point(0, 32);
			this.ui_dgv.Name = "ui_dgv";
			this.ui_dgv.ReadOnly = true;
			this.ui_dgv.RowHeadersVisible = false;
			this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ui_dgv.Size = new System.Drawing.Size(443, 275);
			this.ui_dgv.TabIndex = 2;
			this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.dgv_CurrentCellChanged);
			// 
			// ui_topPanel
			// 
			this.ui_topPanel.Controls.Add(this.ui_featureTypeComboBox);
			this.ui_topPanel.Controls.Add(this.ui_featureTypeLabel);
			this.ui_topPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ui_topPanel.Location = new System.Drawing.Point(0, 0);
			this.ui_topPanel.Name = "ui_topPanel";
			this.ui_topPanel.Size = new System.Drawing.Size(443, 32);
			this.ui_topPanel.TabIndex = 0;
			// 
			// ui_featureTypeComboBox
			// 
			this.ui_featureTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ui_featureTypeComboBox.FormattingEnabled = true;
			this.ui_featureTypeComboBox.Location = new System.Drawing.Point(82, 3);
			this.ui_featureTypeComboBox.Name = "ui_featureTypeComboBox";
			this.ui_featureTypeComboBox.Size = new System.Drawing.Size(193, 21);
			this.ui_featureTypeComboBox.TabIndex = 1;
			this.ui_featureTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ui_featureTypeComboBox_SelectedIndexChanged);
			// 
			// ui_featureTypeLabel
			// 
			this.ui_featureTypeLabel.AutoSize = true;
			this.ui_featureTypeLabel.Location = new System.Drawing.Point(3, 6);
			this.ui_featureTypeLabel.Name = "ui_featureTypeLabel";
			this.ui_featureTypeLabel.Size = new System.Drawing.Size(73, 13);
			this.ui_featureTypeLabel.TabIndex = 0;
			this.ui_featureTypeLabel.Text = "Feature Type:";
			// 
			// btnBack
			// 
			this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
			this.btnBack.Location = new System.Drawing.Point(121, 311);
			this.btnBack.Name = "btnBack";
			this.btnBack.Size = new System.Drawing.Size(65, 28);
			this.btnBack.TabIndex = 7;
			this.btnBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnBack.UseVisualStyleBackColor = true;
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// FeatureSelectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ui_cancelButton;
			this.ClientSize = new System.Drawing.Size(449, 351);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.ui_mainPanel);
			this.Controls.Add(this.ui_cancelButton);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.ui_okButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FeatureSelectorForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Feature";
			this.Load += new System.EventHandler(this.FeatureSelectorForm_Load);
			this.ui_mainPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
			this.ui_topPanel.ResumeLayout(false);
			this.ui_topPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_okButton;
		private System.Windows.Forms.Button ui_cancelButton;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Panel ui_mainPanel;
		private System.Windows.Forms.DataGridView ui_dgv;
		private System.Windows.Forms.Panel ui_topPanel;
		private System.Windows.Forms.ComboBox ui_featureTypeComboBox;
		private System.Windows.Forms.Label ui_featureTypeLabel;
		private System.Windows.Forms.Button btnBack;
    }
}