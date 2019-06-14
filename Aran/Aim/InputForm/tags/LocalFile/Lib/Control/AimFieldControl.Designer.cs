namespace Aran.Aim.InputFormLib
{
    partial class AimFieldControl
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
            this.ui_leftPanel = new System.Windows.Forms.Panel();
            this.ui_rightPanel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.ui_midPanel = new System.Windows.Forms.Panel();
            this.ui_leftPanel.SuspendLayout();
            this.ui_rightPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ui_propNameLabel
            // 
            this.ui_propNameLabel.AutoSize = true;
            this.ui_propNameLabel.Location = new System.Drawing.Point(15, 15);
            this.ui_propNameLabel.Name = "ui_propNameLabel";
            this.ui_propNameLabel.Size = new System.Drawing.Size(89, 13);
            this.ui_propNameLabel.TabIndex = 0;
            this.ui_propNameLabel.Text = "<Property Name>";
            // 
            // ui_leftPanel
            // 
            this.ui_leftPanel.Controls.Add(this.ui_propNameLabel);
            this.ui_leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ui_leftPanel.Location = new System.Drawing.Point(0, 0);
            this.ui_leftPanel.Name = "ui_leftPanel";
            this.ui_leftPanel.Size = new System.Drawing.Size(203, 46);
            this.ui_leftPanel.TabIndex = 1;
            // 
            // ui_rightPanel
            // 
            this.ui_rightPanel.Controls.Add(this.button1);
            this.ui_rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ui_rightPanel.Location = new System.Drawing.Point(433, 0);
            this.ui_rightPanel.Name = "ui_rightPanel";
            this.ui_rightPanel.Size = new System.Drawing.Size(55, 46);
            this.ui_rightPanel.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "clear";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ui_midPanel
            // 
            this.ui_midPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_midPanel.Location = new System.Drawing.Point(203, 0);
            this.ui_midPanel.Name = "ui_midPanel";
            this.ui_midPanel.Size = new System.Drawing.Size(230, 46);
            this.ui_midPanel.TabIndex = 3;
            // 
            // AimFieldControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_midPanel);
            this.Controls.Add(this.ui_rightPanel);
            this.Controls.Add(this.ui_leftPanel);
            this.Name = "AimFieldControl";
            this.Size = new System.Drawing.Size(488, 46);
            this.ui_leftPanel.ResumeLayout(false);
            this.ui_leftPanel.PerformLayout();
            this.ui_rightPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ui_propNameLabel;
        private System.Windows.Forms.Panel ui_leftPanel;
        private System.Windows.Forms.Panel ui_rightPanel;
        private System.Windows.Forms.Panel ui_midPanel;
        private System.Windows.Forms.Button button1;
    }
}
