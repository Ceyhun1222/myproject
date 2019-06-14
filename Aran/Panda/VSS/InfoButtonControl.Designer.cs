namespace Aran.PANDA.Vss
{
    partial class InfoButtonControl
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
            if (disposing && (components != null)) {
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
            this.components = new System.ComponentModel.Container();
            this.ui_infoButton = new System.Windows.Forms.Button();
            this.ui_mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ui_infoButton
            // 
            this.ui_infoButton.BackgroundImage = global::Aran.PANDA.Vss.Properties.Resources.info_20;
            this.ui_infoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ui_infoButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_infoButton.FlatAppearance.BorderSize = 0;
            this.ui_infoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ui_infoButton.Location = new System.Drawing.Point(0, 0);
            this.ui_infoButton.Name = "ui_infoButton";
            this.ui_infoButton.Size = new System.Drawing.Size(22, 22);
            this.ui_infoButton.TabIndex = 3;
            this.ui_mainToolTip.SetToolTip(this.ui_infoButton, "More Information");
            this.ui_infoButton.UseVisualStyleBackColor = true;
            this.ui_infoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // InfoButtonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_infoButton);
            this.Name = "InfoButtonControl";
            this.Size = new System.Drawing.Size(22, 22);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ui_infoButton;
        private System.Windows.Forms.ToolTip ui_mainToolTip;
    }
}
