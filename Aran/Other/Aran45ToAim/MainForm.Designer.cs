namespace Aran45ToAixm
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ui_mdbFileNameTB = new System.Windows.Forms.TextBox();
            this.ui_selectMDBFileButton = new System.Windows.Forms.Button();
            this.ui_progressPanel = new System.Windows.Forms.Panel();
            this.ui_progressInfoLabel = new System.Windows.Forms.Label();
            this.ui_progressBar = new System.Windows.Forms.ProgressBar();
            this.ui_openFileButton = new System.Windows.Forms.Button();
            this.ui_featuresChLB = new System.Windows.Forms.CheckedListBox();
            this.ui_convertButton = new System.Windows.Forms.Button();
            this.ui_progressPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "MDB File name:";
            // 
            // ui_mdbFileNameTB
            // 
            this.ui_mdbFileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_mdbFileNameTB.Location = new System.Drawing.Point(12, 25);
            this.ui_mdbFileNameTB.Name = "ui_mdbFileNameTB";
            this.ui_mdbFileNameTB.Size = new System.Drawing.Size(399, 20);
            this.ui_mdbFileNameTB.TabIndex = 4;
            this.ui_mdbFileNameTB.TextChanged += new System.EventHandler(this.FileName_TextChanged);
            // 
            // ui_selectMDBFileButton
            // 
            this.ui_selectMDBFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_selectMDBFileButton.Location = new System.Drawing.Point(417, 23);
            this.ui_selectMDBFileButton.Name = "ui_selectMDBFileButton";
            this.ui_selectMDBFileButton.Size = new System.Drawing.Size(31, 23);
            this.ui_selectMDBFileButton.TabIndex = 5;
            this.ui_selectMDBFileButton.Text = "...";
            this.ui_selectMDBFileButton.UseVisualStyleBackColor = true;
            this.ui_selectMDBFileButton.Click += new System.EventHandler(this.SelectMDBFile_Click);
            // 
            // ui_progressPanel
            // 
            this.ui_progressPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_progressPanel.Controls.Add(this.ui_progressInfoLabel);
            this.ui_progressPanel.Controls.Add(this.ui_progressBar);
            this.ui_progressPanel.Location = new System.Drawing.Point(12, 208);
            this.ui_progressPanel.Name = "ui_progressPanel";
            this.ui_progressPanel.Size = new System.Drawing.Size(436, 54);
            this.ui_progressPanel.TabIndex = 8;
            this.ui_progressPanel.Visible = false;
            // 
            // ui_progressInfoLabel
            // 
            this.ui_progressInfoLabel.AutoSize = true;
            this.ui_progressInfoLabel.Location = new System.Drawing.Point(6, 5);
            this.ui_progressInfoLabel.Name = "ui_progressInfoLabel";
            this.ui_progressInfoLabel.Size = new System.Drawing.Size(37, 13);
            this.ui_progressInfoLabel.TabIndex = 1;
            this.ui_progressInfoLabel.Text = "<Info>";
            // 
            // ui_progressBar
            // 
            this.ui_progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_progressBar.Location = new System.Drawing.Point(3, 21);
            this.ui_progressBar.Name = "ui_progressBar";
            this.ui_progressBar.Size = new System.Drawing.Size(430, 23);
            this.ui_progressBar.TabIndex = 0;
            // 
            // ui_openFileButton
            // 
            this.ui_openFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_openFileButton.Enabled = false;
            this.ui_openFileButton.Location = new System.Drawing.Point(373, 51);
            this.ui_openFileButton.Name = "ui_openFileButton";
            this.ui_openFileButton.Size = new System.Drawing.Size(75, 23);
            this.ui_openFileButton.TabIndex = 12;
            this.ui_openFileButton.Text = "Open";
            this.ui_openFileButton.UseVisualStyleBackColor = true;
            this.ui_openFileButton.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // ui_featuresChLB
            // 
            this.ui_featuresChLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_featuresChLB.FormattingEnabled = true;
            this.ui_featuresChLB.Location = new System.Drawing.Point(12, 52);
            this.ui_featuresChLB.Name = "ui_featuresChLB";
            this.ui_featuresChLB.Size = new System.Drawing.Size(202, 139);
            this.ui_featuresChLB.TabIndex = 13;
            this.ui_featuresChLB.SelectedIndexChanged += new System.EventHandler(this.FeaturesChLB_SelectedIndexChanged);
            // 
            // ui_convertButton
            // 
            this.ui_convertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_convertButton.Enabled = false;
            this.ui_convertButton.Location = new System.Drawing.Point(370, 174);
            this.ui_convertButton.Name = "ui_convertButton";
            this.ui_convertButton.Size = new System.Drawing.Size(75, 23);
            this.ui_convertButton.TabIndex = 14;
            this.ui_convertButton.Text = "Convert";
            this.ui_convertButton.UseVisualStyleBackColor = true;
            this.ui_convertButton.Click += new System.EventHandler(this.Convert_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 274);
            this.Controls.Add(this.ui_convertButton);
            this.Controls.Add(this.ui_featuresChLB);
            this.Controls.Add(this.ui_openFileButton);
            this.Controls.Add(this.ui_progressPanel);
            this.Controls.Add(this.ui_selectMDBFileButton);
            this.Controls.Add(this.ui_mdbFileNameTB);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(294, 171);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Aran 4.5 To 5.1";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ui_progressPanel.ResumeLayout(false);
            this.ui_progressPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ui_mdbFileNameTB;
        private System.Windows.Forms.Button ui_selectMDBFileButton;
        private System.Windows.Forms.Panel ui_progressPanel;
        private System.Windows.Forms.ProgressBar ui_progressBar;
        private System.Windows.Forms.Label ui_progressInfoLabel;
        private System.Windows.Forms.Button ui_openFileButton;
        private System.Windows.Forms.CheckedListBox ui_featuresChLB;
        private System.Windows.Forms.Button ui_convertButton;
    }
}

