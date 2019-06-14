namespace UMLInfo
{
    partial class PropForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle ();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle ();
            this.closeButton = new System.Windows.Forms.Button ();
            this.propDGV = new System.Windows.Forms.DataGridView ();
            this.docTextBox = new System.Windows.Forms.TextBox ();
            this.label1 = new System.Windows.Forms.Label ();
            this.label2 = new System.Windows.Forms.Label ();
            this.restrictionTextBox = new System.Windows.Forms.TextBox ();
            ((System.ComponentModel.ISupportInitialize) (this.propDGV)).BeginInit ();
            this.SuspendLayout ();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point (764, 477);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size (75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler (this.closeButton_Click);
            // 
            // propDGV
            // 
            this.propDGV.AllowUserToAddRows = false;
            this.propDGV.AllowUserToOrderColumns = true;
            this.propDGV.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.propDGV.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.propDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.propDGV.ColumnHeadersHeight = 30;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.propDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.propDGV.Location = new System.Drawing.Point (12, 12);
            this.propDGV.Name = "propDGV";
            this.propDGV.ReadOnly = true;
            this.propDGV.RowHeadersVisible = false;
            this.propDGV.RowHeadersWidth = 25;
            this.propDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.propDGV.Size = new System.Drawing.Size (827, 386);
            this.propDGV.TabIndex = 3;
            this.propDGV.CurrentCellChanged += new System.EventHandler (this.propDGV_CurrentCellChanged);
            // 
            // docTextBox
            // 
            this.docTextBox.AcceptsReturn = true;
            this.docTextBox.AcceptsTab = true;
            this.docTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.docTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.docTextBox.Location = new System.Drawing.Point (12, 422);
            this.docTextBox.Multiline = true;
            this.docTextBox.Name = "docTextBox";
            this.docTextBox.ReadOnly = true;
            this.docTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.docTextBox.Size = new System.Drawing.Size (522, 49);
            this.docTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point (12, 406);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size (82, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Documentation:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point (540, 406);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size (60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Restriction:";
            // 
            // restrictionTextBox
            // 
            this.restrictionTextBox.AcceptsReturn = true;
            this.restrictionTextBox.AcceptsTab = true;
            this.restrictionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.restrictionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.restrictionTextBox.Location = new System.Drawing.Point (540, 422);
            this.restrictionTextBox.Multiline = true;
            this.restrictionTextBox.Name = "restrictionTextBox";
            this.restrictionTextBox.ReadOnly = true;
            this.restrictionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.restrictionTextBox.Size = new System.Drawing.Size (299, 49);
            this.restrictionTextBox.TabIndex = 6;
            // 
            // PropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size (851, 512);
            this.Controls.Add (this.label2);
            this.Controls.Add (this.restrictionTextBox);
            this.Controls.Add (this.label1);
            this.Controls.Add (this.docTextBox);
            this.Controls.Add (this.propDGV);
            this.Controls.Add (this.closeButton);
            this.Name = "PropForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.PropForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.propDGV)).EndInit ();
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.DataGridView propDGV;
        private System.Windows.Forms.TextBox docTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox restrictionTextBox;
    }
}