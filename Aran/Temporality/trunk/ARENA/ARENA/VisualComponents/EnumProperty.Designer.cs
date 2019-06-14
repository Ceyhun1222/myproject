using System.Windows.Forms;
namespace ARENA.VisualComponents
{
    partial class EnumProperty
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
            this.Property_Name = new System.Windows.Forms.Label();
            this.PropertyValueCmbBx = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(1, -1);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(181, 36);
            this.Property_Name.TabIndex = 2;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropertyValueCmbBx
            // 
            this.PropertyValueCmbBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyValueCmbBx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.PropertyValueCmbBx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PropertyValueCmbBx.FormattingEnabled = true;
            this.PropertyValueCmbBx.Location = new System.Drawing.Point(193, 6);
            this.PropertyValueCmbBx.Name = "PropertyValueCmbBx";
            this.PropertyValueCmbBx.Size = new System.Drawing.Size(259, 21);
            this.PropertyValueCmbBx.TabIndex = 3;
            this.PropertyValueCmbBx.SelectedIndexChanged += new System.EventHandler(this.PropertyValueCmbBx_SelectedIndexChanged);
            // 
            // EnumProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueCmbBx);
            this.Controls.Add(this.Property_Name);
            this.Name = "EnumProperty";
            this.Size = new System.Drawing.Size(455, 35);
            this.ResumeLayout(false);

        }

        private void InitializeComponent(bool mandatoryFlag)
        {
            this.Property_Name = new System.Windows.Forms.Label();
            this.PropertyValueCmbBx = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            if (mandatoryFlag) 
                this.Property_Name.ForeColor =  System.Drawing.Color.DarkRed;
            else
                this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));

            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(1, -1);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(181, 36);
            this.Property_Name.TabIndex = 2;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropertyValueCmbBx
            // 
            this.PropertyValueCmbBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyValueCmbBx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.PropertyValueCmbBx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PropertyValueCmbBx.FormattingEnabled = true;
            this.PropertyValueCmbBx.Location = new System.Drawing.Point(193, 6);
            this.PropertyValueCmbBx.Name = "PropertyValueCmbBx";
            this.PropertyValueCmbBx.Size = new System.Drawing.Size(259, 21);
            this.PropertyValueCmbBx.TabIndex = 3;
            this.PropertyValueCmbBx.SelectedIndexChanged += new System.EventHandler(this.PropertyValueCmbBx_SelectedIndexChanged);
            // 
            // EnumProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueCmbBx);
            this.Controls.Add(this.Property_Name);
            this.Name = "EnumProperty";
            this.Size = new System.Drawing.Size(455, 35);
            this.ResumeLayout(false);

        }

        private void InitializeComponent(bool mandatoryFlag, string toolTipTextString)
        {
            this.Property_Name = new System.Windows.Forms.Label();
            this.PropertyValueCmbBx = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Property_Name
            // 
            this.Property_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Property_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Property_Name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Property_Name.Location = new System.Drawing.Point(1, -1);
            this.Property_Name.Name = "Property_Name";
            this.Property_Name.Size = new System.Drawing.Size(181, 36);
            this.Property_Name.TabIndex = 2;
            this.Property_Name.Text = "label1";
            this.Property_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropertyValueCmbBx
            // 
            this.PropertyValueCmbBx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.PropertyValueCmbBx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PropertyValueCmbBx.FormattingEnabled = true;
            this.PropertyValueCmbBx.Location = new System.Drawing.Point(193, 6);
            this.PropertyValueCmbBx.Name = "PropertyValueCmbBx";
            this.PropertyValueCmbBx.Size = new System.Drawing.Size(259, 21);
            this.PropertyValueCmbBx.TabIndex = 3;
            this.PropertyValueCmbBx.SelectedIndexChanged += new System.EventHandler(this.PropertyValueCmbBx_SelectedIndexChanged);
            // 
            // EnumProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PropertyValueCmbBx);
            this.Controls.Add(this.Property_Name);
            this.Name = "EnumProperty";
            this.Size = new System.Drawing.Size(455, 35);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Property_Name;
        public System.Windows.Forms.ComboBox PropertyValueCmbBx;
        private string Property_Type;
    }
}
