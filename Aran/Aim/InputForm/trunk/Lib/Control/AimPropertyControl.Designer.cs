namespace Aran.Aim.InputFormLib
{
    partial class AimPropertyControl
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
            this.ui_propNameLabel = new System.Windows.Forms.Label();
            this.ui_propValueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ui_propNameLabel
            // 
            this.ui_propNameLabel.AutoSize = true;
            this.ui_propNameLabel.Location = new System.Drawing.Point(3, 7);
            this.ui_propNameLabel.Name = "ui_propNameLabel";
            this.ui_propNameLabel.Size = new System.Drawing.Size(89, 13);
            this.ui_propNameLabel.TabIndex = 1;
            this.ui_propNameLabel.Text = "<Property Name>";
            // 
            // ui_propValueLabel
            // 
            this.ui_propValueLabel.AutoSize = true;
            this.ui_propValueLabel.Location = new System.Drawing.Point(199, 7);
            this.ui_propValueLabel.Name = "ui_propValueLabel";
            this.ui_propValueLabel.Size = new System.Drawing.Size(88, 13);
            this.ui_propValueLabel.TabIndex = 2;
            this.ui_propValueLabel.Text = "<Property Value>";
            // 
            // AimPropertyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_propValueLabel);
            this.Controls.Add(this.ui_propNameLabel);
            this.Name = "AimPropertyControl";
            this.Size = new System.Drawing.Size(499, 29);
            this.MouseEnter += new System.EventHandler(this.AimPropertyControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.AimPropertyControl_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ui_propNameLabel;
        private System.Windows.Forms.Label ui_propValueLabel;
    }
}
