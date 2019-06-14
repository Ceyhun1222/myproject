namespace MapEnv.Forms
{
    partial class RasterPropertyForm
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
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_fileNameTB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(8, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(85, 13);
            label1.TabIndex = 1;
            label1.Text = "Raster File Path:";
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(340, 76);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 0;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_fileNameTB
            // 
            this.ui_fileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_fileNameTB.Location = new System.Drawing.Point(11, 25);
            this.ui_fileNameTB.Name = "ui_fileNameTB";
            this.ui_fileNameTB.ReadOnly = true;
            this.ui_fileNameTB.Size = new System.Drawing.Size(404, 20);
            this.ui_fileNameTB.TabIndex = 2;
            // 
            // RasterPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 111);
            this.Controls.Add(this.ui_fileNameTB);
            this.Controls.Add(label1);
            this.Controls.Add(this.ui_closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RasterPropertyForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Raster Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.TextBox ui_fileNameTB;
    }
}