namespace MapEnv
{
    partial class LogsForm
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
            this.ui_linesTB = new System.Windows.Forms.TextBox();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ui_linesTB
            // 
            this.ui_linesTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_linesTB.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_linesTB.Location = new System.Drawing.Point(12, 12);
            this.ui_linesTB.Multiline = true;
            this.ui_linesTB.Name = "ui_linesTB";
            this.ui_linesTB.Size = new System.Drawing.Size(366, 204);
            this.ui_linesTB.TabIndex = 0;
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(303, 234);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(75, 23);
            this.ui_closeButton.TabIndex = 1;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.ui_closeButton_Click);
            // 
            // LogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ui_closeButton;
            this.ClientSize = new System.Drawing.Size(390, 269);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(this.ui_linesTB);
            this.Name = "LogsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Logs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_linesTB;
        private System.Windows.Forms.Button ui_closeButton;
    }
}