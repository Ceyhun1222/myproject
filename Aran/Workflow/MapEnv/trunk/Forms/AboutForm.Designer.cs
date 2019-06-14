namespace MapEnv
{
    partial class AboutForm
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
            System.Windows.Forms.Label label2;
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_versionTB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            label1.BackColor = System.Drawing.SystemColors.Window;
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(-2, -2);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            label1.Size = new System.Drawing.Size(416, 65);
            label1.TabIndex = 1;
            label1.Text = "IAIM Environment";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label2.Location = new System.Drawing.Point(130, 42);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(51, 15);
            label2.TabIndex = 2;
            label2.Text = "Version:";
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ui_closeButton.Location = new System.Drawing.Point(324, 117);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 0;
            this.ui_closeButton.Text = "&Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            // 
            // ui_versionTB
            // 
            this.ui_versionTB.AutoSize = true;
            this.ui_versionTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_versionTB.Location = new System.Drawing.Point(184, 42);
            this.ui_versionTB.Name = "ui_versionTB";
            this.ui_versionTB.Size = new System.Drawing.Size(36, 15);
            this.ui_versionTB.TabIndex = 3;
            this.ui_versionTB.Text = "<x.x>";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MapEnv.Properties.Resources.risk_final_tf;
            this.pictureBox1.Location = new System.Drawing.Point(12, 83);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(286, 57);
            this.pictureBox1.TabIndex = 539;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.ui_closeButton;
            this.ClientSize = new System.Drawing.Size(411, 152);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ui_versionTB);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "IAIM Environment - About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.Label ui_versionTB;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}