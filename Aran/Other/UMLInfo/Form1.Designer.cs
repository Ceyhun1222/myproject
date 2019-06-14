namespace UMLInfo
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.assocDGV = new System.Windows.Forms.DataGridView();
            this.Role1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Supplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cardinality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Containment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Supplier_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cardinality_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Containment_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowCountlabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.assocDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // assocDGV
            // 
            this.assocDGV.AllowUserToAddRows = false;
            this.assocDGV.AllowUserToOrderColumns = true;
            this.assocDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.assocDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.assocDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.assocDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.assocDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Role1,
            this.Label,
            this.Supplier,
            this.Cardinality,
            this.Containment,
            this.Role2,
            this.Label_2,
            this.Supplier_2,
            this.Cardinality_2,
            this.Containment_2});
            this.assocDGV.Location = new System.Drawing.Point(12, -4);
            this.assocDGV.Name = "assocDGV";
            this.assocDGV.ReadOnly = true;
            this.assocDGV.Size = new System.Drawing.Size(578, 262);
            this.assocDGV.TabIndex = 3;
            // 
            // Role1
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Role1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Role1.HeaderText = "Role1";
            this.Role1.Name = "Role1";
            this.Role1.ReadOnly = true;
            this.Role1.Width = 30;
            // 
            // Label
            // 
            this.Label.HeaderText = "Label";
            this.Label.Name = "Label";
            this.Label.ReadOnly = true;
            this.Label.Width = 150;
            // 
            // Supplier
            // 
            this.Supplier.HeaderText = "Supplier";
            this.Supplier.Name = "Supplier";
            this.Supplier.ReadOnly = true;
            // 
            // Cardinality
            // 
            this.Cardinality.HeaderText = "Cardinality";
            this.Cardinality.Name = "Cardinality";
            this.Cardinality.ReadOnly = true;
            this.Cardinality.Width = 50;
            // 
            // Containment
            // 
            this.Containment.HeaderText = "Containment";
            this.Containment.Name = "Containment";
            this.Containment.ReadOnly = true;
            this.Containment.Width = 50;
            // 
            // Role2
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Role2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Role2.HeaderText = "Role2";
            this.Role2.Name = "Role2";
            this.Role2.ReadOnly = true;
            this.Role2.Width = 30;
            // 
            // Label_2
            // 
            this.Label_2.HeaderText = "Label-2";
            this.Label_2.Name = "Label_2";
            this.Label_2.ReadOnly = true;
            this.Label_2.Width = 150;
            // 
            // Supplier_2
            // 
            this.Supplier_2.HeaderText = "Supplier-2";
            this.Supplier_2.Name = "Supplier_2";
            this.Supplier_2.ReadOnly = true;
            // 
            // Cardinality_2
            // 
            this.Cardinality_2.HeaderText = "Cardinality_2";
            this.Cardinality_2.Name = "Cardinality_2";
            this.Cardinality_2.ReadOnly = true;
            this.Cardinality_2.Width = 50;
            // 
            // Containment_2
            // 
            this.Containment_2.HeaderText = "Containment-2";
            this.Containment_2.Name = "Containment_2";
            this.Containment_2.ReadOnly = true;
            this.Containment_2.Width = 50;
            // 
            // rowCountlabel
            // 
            this.rowCountlabel.AutoSize = true;
            this.rowCountlabel.Location = new System.Drawing.Point(12, 270);
            this.rowCountlabel.Name = "rowCountlabel";
            this.rowCountlabel.Size = new System.Drawing.Size(0, 13);
            this.rowCountlabel.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 292);
            this.Controls.Add(this.rowCountlabel);
            this.Controls.Add(this.assocDGV);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.assocDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView assocDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Label;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cardinality;
        private System.Windows.Forms.DataGridViewTextBoxColumn Containment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Label_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Supplier_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cardinality_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Containment_2;
        private System.Windows.Forms.Label rowCountlabel;
    }
}