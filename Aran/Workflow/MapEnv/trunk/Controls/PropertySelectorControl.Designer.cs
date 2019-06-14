namespace MapEnv
{
    partial class PropertySelectorControl
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
            this.ui_propNameTB = new System.Windows.Forms.TextBox();
            this.ui_propNameSelectorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ui_propNameTB
            // 
            this.ui_propNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_propNameTB.Location = new System.Drawing.Point(3, 3);
            this.ui_propNameTB.Name = "ui_propNameTB";
            this.ui_propNameTB.ReadOnly = true;
            this.ui_propNameTB.Size = new System.Drawing.Size(283, 20);
            this.ui_propNameTB.TabIndex = 6;
            // 
            // ui_propNameSelectorButton
            // 
            this.ui_propNameSelectorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_propNameSelectorButton.Enabled = false;
            this.ui_propNameSelectorButton.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ui_propNameSelectorButton.Location = new System.Drawing.Point(292, 2);
            this.ui_propNameSelectorButton.Name = "ui_propNameSelectorButton";
            this.ui_propNameSelectorButton.Size = new System.Drawing.Size(29, 23);
            this.ui_propNameSelectorButton.TabIndex = 5;
            this.ui_propNameSelectorButton.Text = "▼";
            this.ui_propNameSelectorButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ui_propNameSelectorButton.UseVisualStyleBackColor = true;
            this.ui_propNameSelectorButton.Click += new System.EventHandler(this.uiEvents_propNameSelectorButton_Click);
            // 
            // PropertySelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ui_propNameTB);
            this.Controls.Add(this.ui_propNameSelectorButton);
            this.Name = "PropertySelectorControl";
            this.Size = new System.Drawing.Size(324, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_propNameTB;
        private System.Windows.Forms.Button ui_propNameSelectorButton;
    }
}
