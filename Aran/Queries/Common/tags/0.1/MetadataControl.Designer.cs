namespace Aran.Queries.Common
{
    partial class MetadataControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.ui_showInWebBrowserButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ui_showInWebBrowserButton
            // 
            this.ui_showInWebBrowserButton.Enabled = false;
            this.ui_showInWebBrowserButton.Location = new System.Drawing.Point(12, 17);
            this.ui_showInWebBrowserButton.Name = "ui_showInWebBrowserButton";
            this.ui_showInWebBrowserButton.Size = new System.Drawing.Size(140, 23);
            this.ui_showInWebBrowserButton.TabIndex = 0;
            this.ui_showInWebBrowserButton.Text = "Show in web browser";
            this.ui_showInWebBrowserButton.UseVisualStyleBackColor = true;
            this.ui_showInWebBrowserButton.Click += new System.EventHandler(this.ui_showInWebBrowserButton_Click);
            // 
            // MetadataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_showInWebBrowserButton);
            this.Name = "MetadataControl";
            this.Size = new System.Drawing.Size(307, 271);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_showInWebBrowserButton;
    }
}
