namespace AIP.BaseLib.Airac
{
    partial class AiracCycleDateTime
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ui_maskedTB = new System.Windows.Forms.MaskedTextBox();
            this.ui_selButton = new System.Windows.Forms.Button();
            this.ui_airacOrCustomLabel = new System.Windows.Forms.Label();
            this.ui_utcLabel = new System.Windows.Forms.Label();
            this.ui_maskedTB.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_maskedTB
            // 
            this.ui_maskedTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_maskedTB.Controls.Add(this.ui_selButton);
            this.ui_maskedTB.Controls.Add(this.ui_utcLabel);
            this.ui_maskedTB.Controls.Add(this.ui_airacOrCustomLabel);
            this.ui_maskedTB.Location = new System.Drawing.Point(2, 1);
            this.ui_maskedTB.Margin = new System.Windows.Forms.Padding(46, 3, 30, 3);
            this.ui_maskedTB.Mask = "00 - 00 - 0000";
            this.ui_maskedTB.Name = "ui_maskedTB";
            this.ui_maskedTB.Size = new System.Drawing.Size(232, 20);
            this.ui_maskedTB.TabIndex = 5;
            this.ui_maskedTB.Validating += new System.ComponentModel.CancelEventHandler(this.MaskedTB_Validating);
            // 
            // ui_selButton
            // 
            this.ui_selButton.BackgroundImage = global::AIP.BaseLib.Properties.Resources.calendar_16;
            this.ui_selButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ui_selButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.ui_selButton.FlatAppearance.BorderSize = 0;
            this.ui_selButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_selButton.Location = new System.Drawing.Point(208, 0);
            this.ui_selButton.Name = "ui_selButton";
            this.ui_selButton.Size = new System.Drawing.Size(20, 16);
            this.ui_selButton.TabIndex = 5;
            this.ui_selButton.Text = "...";
            this.ui_selButton.UseVisualStyleBackColor = true;
            this.ui_selButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // ui_airacOrCustomLabel
            // 
            this.ui_airacOrCustomLabel.AutoSize = true;
            this.ui_airacOrCustomLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ui_airacOrCustomLabel.Location = new System.Drawing.Point(112, 1);
            this.ui_airacOrCustomLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ui_airacOrCustomLabel.Name = "ui_airacOrCustomLabel";
            this.ui_airacOrCustomLabel.Size = new System.Drawing.Size(39, 13);
            this.ui_airacOrCustomLabel.TabIndex = 5;
            this.ui_airacOrCustomLabel.Text = "AIRAC";
            this.ui_airacOrCustomLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ui_utcLabel
            // 
            this.ui_utcLabel.AutoSize = true;
            this.ui_utcLabel.BackColor = System.Drawing.Color.Transparent;
            this.ui_utcLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ui_utcLabel.Location = new System.Drawing.Point(177, 1);
            this.ui_utcLabel.Name = "ui_utcLabel";
            this.ui_utcLabel.Size = new System.Drawing.Size(29, 13);
            this.ui_utcLabel.TabIndex = 5;
            this.ui_utcLabel.Text = "UTC";
            // 
            // AiracCycleDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_maskedTB);
            this.Name = "AiracCycleDateTime";
            this.Size = new System.Drawing.Size(236, 21);
            this.ui_maskedTB.ResumeLayout(false);
            this.ui_maskedTB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox ui_maskedTB;
        private System.Windows.Forms.Button ui_selButton;
        private System.Windows.Forms.Label ui_airacOrCustomLabel;
        private System.Windows.Forms.Label ui_utcLabel;
    }
}
