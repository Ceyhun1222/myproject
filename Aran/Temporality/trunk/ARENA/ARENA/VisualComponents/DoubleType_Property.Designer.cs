namespace ARENA.VisualComponents
{
    partial class DoubleType_Property
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
            this.PropertyValueTxtBx = new System.Windows.Forms.TextBox();
            this.Property_Name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PropertyValueTxtBx
            // 
            this.PropertyValueTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyValueTxtBx.Location = new System.Drawing.Point(225, 12);
            this.PropertyValueTxtBx.Multiline = true;
            this.PropertyValueTxtBx.Name = "PropertyValueTxtBx";
            this.PropertyValueTxtBx.Size = new System.Drawing.Size(188, 29);
            this.PropertyValueTxtBx.TabIndex = 3;
            this.PropertyValueTxtBx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PropertyValueTxtBx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Property_Value_KeyPress);
            this.PropertyValueTxtBx.Validated += new System.EventHandler(this.Property_Value_Validated);
            // 
            // Property_Name
            // 
            this.Property_Name.Dock = System.Windows.Forms.DockStyle.Left;
            this.Property_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.Image = global::ARENA.Properties.Resources.EditingStartEditing32;
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(0, 0);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(218, 58);
            this.Property_Name.TabIndex = 2;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DoubleType_Property
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueTxtBx);
            this.Controls.Add(this.Property_Name);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "DoubleType_Property";
            this.Size = new System.Drawing.Size(443, 58);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Property_Name;
        public System.Windows.Forms.TextBox PropertyValueTxtBx;


    }
}
