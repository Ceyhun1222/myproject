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
            this.ui_cancelButton = new System.Windows.Forms.Button();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            this.ui_mainPanel = new System.Windows.Forms.Panel();
            this.ui_topPanel = new System.Windows.Forms.Panel();
            this.ui_featureTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ui_featureTypeLabel = new System.Windows.Forms.Label();
            this.ui_backButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.ui_mainPanel.SuspendLayout();
            this.ui_topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_cancelButton
            // 
            this.ui_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_cancelButton.Location = new System.Drawing.Point(421, 356);
            this.ui_cancelButton.Name = "ui_cancelButton";
            this.ui_cancelButton.Size = new System.Drawing.Size(65, 28);
            this.ui_cancelButton.TabIndex = 1;
            this.ui_cancelButton.Text = "Cancel";
            this.ui_cancelButton.UseVisualStyleBackColor = true;
            // 
            // ui_dgv
            // 
            this.ui_dgv.AllowUserToAddRows = false;
            this.ui_dgv.AllowUserToDeleteRows = false;
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dgv.Location = new System.Drawing.Point(0, 32);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.RowHeadersVisible = false;
            this.ui_dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ui_dgv.Size = new System.Drawing.Size(474, 306);
            this.ui_dgv.TabIndex = 2;
            this.ui_dgv.CurrentCellChanged += new System.EventHandler(this.dgv_CurrentCellChanged);
            // 
            // ui_mainPanel
            // 
            this.ui_mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mainPanel.Controls.Add(this.ui_dgv);
            this.ui_mainPanel.Controls.Add(this.ui_topPanel);
            this.ui_mainPanel.Location = new System.Drawing.Point(12, 12);
            this.ui_mainPanel.Name = "ui_mainPanel";
            this.ui_mainPanel.Size = new System.Drawing.Size(474, 338);
            this.ui_mainPanel.TabIndex = 3;
            // 
            // ui_topPanel
            // 
            this.ui_topPanel.Controls.Add(this.ui_featureTypeComboBox);
            this.ui_topPanel.Controls.Add(this.ui_featureTypeLabel);
            this.ui_topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ui_topPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_topPanel.Name = "ui_topPanel";
            this.ui_topPanel.Size = new System.Drawing.Size(474, 32);
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
            this.ui_featureTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.FeatureType_SelectedIndexChanged);
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
            // ui_backButton
            // 
            this.ui_backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_backButton.Enabled = false;
            this.ui_backButton.Location = new System.Drawing.Point(12, 356);
            this.ui_backButton.Name = "ui_backButton";
            this.ui_backButton.Size = new System.Drawing.Size(65, 28);
            this.ui_backButton.TabIndex = 9;
            this.ui_backButton.Text = "Back";
            this.ui_backButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ui_backButton.UseVisualStyleBackColor = true;
            this.ui_backButton.Click += new System.EventHandler(this.Back_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_nextButton.Enabled = false;
            this.ui_nextButton.Location = new System.Drawing.Point(83, 356);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(65, 28);
            this.ui_nextButton.TabIndex = 8;
            this.ui_nextButton.Text = "Next";
            this.ui_nextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ui_nextButton.UseCompatibleTextRendering = true;
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_okButton
            // 
            this.ui_okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_okButton.Enabled = false;
            this.ui_okButton.Location = new System.Drawing.Point(350, 356);
            this.ui_okButton.Name = "ui_okButton";
            this.ui_okButton.Size = new System.Drawing.Size(65, 28);
            this.ui_okButton.TabIndex = 10;
            this.ui_okButton.Text = "OK";
            this.ui_okButton.UseVisualStyleBackColor = true;
            this.ui_okButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // FeatureSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_cancelButton;
            this.ClientSize = new System.Drawing.Size(498, 391);
            this.Controls.Add(this.ui_backButton);
            this.Controls.Add(this.ui_mainPanel);
            this.Controls.Add(this.ui_cancelButton);
            this.Controls.Add(this.ui_okButton);
            this.Controls.Add(this.ui_nextButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureSelectorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Feature";
            this.Load += new System.EventHandler(this.FeatureSelectorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ui_mainPanel.ResumeLayout(false);
            this.ui_topPanel.ResumeLayout(false);
            this.ui_topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button ui_cancelButton;
        private System.Windows.Forms.DataGridView ui_dgv;
        private System.Windows.Forms.Panel ui_mainPanel;
        private System.Windows.Forms.Panel ui_topPanel;
        private System.Windows.Forms.ComboBox ui_featureTypeComboBox;
        private System.Windows.Forms.Label ui_featureTypeLabel;
		private System.Windows.Forms.Button ui_backButton;
		private System.Windows.Forms.Button ui_nextButton;
		private System.Windows.Forms.Button ui_okButton;
    }
}