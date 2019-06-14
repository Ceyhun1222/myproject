using System.Windows.Forms;
namespace ARENA.VisualComponents
{
    partial class StringProperty
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
            this.PropertyValueTxtBx.Location = new System.Drawing.Point(193, 6);
            this.PropertyValueTxtBx.Multiline = true;
            this.PropertyValueTxtBx.Name = "PropertyValueTxtBx";
            this.PropertyValueTxtBx.Size = new System.Drawing.Size(259, 23);
            this.PropertyValueTxtBx.TabIndex = 1;
            this.PropertyValueTxtBx.TextChanged += new System.EventHandler(this.PropertyValueTxtBx_TextChanged);
            this.PropertyValueTxtBx.Validated += new System.EventHandler(this.Property_Value_Validated);
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(0, 0);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(181, 36);
            this.Property_Name.TabIndex = 0;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Click += new System.EventHandler(this.Property_Name_Click);
            // 
            // StringProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueTxtBx);
            this.Controls.Add(this.Property_Name);
            this.Name = "StringProperty";
            this.Size = new System.Drawing.Size(455, 35);
            this.Load += new System.EventHandler(this.StringProperty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializeComponent(bool mandatoryFlag)
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
            this.PropertyValueTxtBx.Location = new System.Drawing.Point(225, 7);
            this.PropertyValueTxtBx.Multiline = true;
            this.PropertyValueTxtBx.Name = "PropertyValueTxtBx";
            this.PropertyValueTxtBx.Size = new System.Drawing.Size(301, 26);
            this.PropertyValueTxtBx.TabIndex = 1;
            this.PropertyValueTxtBx.TextChanged += new System.EventHandler(this.PropertyValueTxtBx_TextChanged);
            this.PropertyValueTxtBx.Validated += new System.EventHandler(this.Property_Value_Validated);
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(0, 0);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(211, 42);
            this.Property_Name.TabIndex = 0;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Click += new System.EventHandler(this.Property_Name_Click);
            // 
            // StringProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueTxtBx);
            this.Controls.Add(this.Property_Name);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "StringProperty";
            this.Size = new System.Drawing.Size(531, 40);
            this.Load += new System.EventHandler(this.StringProperty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializeComponent(bool mandatoryFlag, string toolTipTextString)
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
            this.PropertyValueTxtBx.Location = new System.Drawing.Point(225, 7);
            this.PropertyValueTxtBx.Multiline = true;
            this.PropertyValueTxtBx.Name = "PropertyValueTxtBx";
            this.PropertyValueTxtBx.Size = new System.Drawing.Size(301, 26);
            this.PropertyValueTxtBx.TabIndex = 1;

            ToolTip toolTip1 = new ToolTip();
            toolTip1.IsBalloon = true;
            toolTip1.AutoPopDelay = 3000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 500;
            toolTip1.ShowAlways = true;
            toolTip1.Active = true;
            toolTip1.SetToolTip(this.PropertyValueTxtBx, toolTipTextString);

            this.PropertyValueTxtBx.TextChanged += new System.EventHandler(this.PropertyValueTxtBx_TextChanged);
            this.PropertyValueTxtBx.Validated += new System.EventHandler(this.Property_Value_Validated);
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(0, 0);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(211, 42);
            this.Property_Name.TabIndex = 0;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Click += new System.EventHandler(this.Property_Name_Click);
            // 
            // StringProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueTxtBx);
            this.Controls.Add(this.Property_Name);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "StringProperty";
            this.Size = new System.Drawing.Size(531, 40);
            this.Load += new System.EventHandler(this.StringProperty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label Property_Name;
        public System.Windows.Forms.TextBox PropertyValueTxtBx;


    }
}
