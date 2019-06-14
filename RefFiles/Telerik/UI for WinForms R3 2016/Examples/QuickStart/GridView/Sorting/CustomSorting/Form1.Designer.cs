namespace Telerik.Examples.WinControls.GridView.Sorting.CustomSorting
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.radGridView = new Telerik.WinControls.UI.RadGridView();
            this.customersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nwindRadGridView = new Telerik.Examples.WinControls.DataSources.NorthwindDataSet();
            this.customersTableAdapter = new Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwindRadGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Location = new System.Drawing.Point(1446, 1);
            // 
            // 
            // 
            this.settingsPanel.RootElement.ForeColor = System.Drawing.Color.Black;
            this.settingsPanel.Size = new System.Drawing.Size(200, 747);
            this.settingsPanel.ThemeName = "ControlDefault";
            // 
            // radGridView
            // 
            this.radGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.radGridView.ForeColor = System.Drawing.Color.Black;
            this.radGridView.Location = new System.Drawing.Point(12, 11);
            // 
            // radGridView
            // 
            this.radGridView.MasterTemplate.AllowAddNewRow = false;
            this.radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView.MasterTemplate.Caption = null;
            gridViewTextBoxColumn1.FieldName = "CustomerID";
            gridViewTextBoxColumn1.HeaderText = "CustomerID";
            gridViewTextBoxColumn1.IsAutoGenerated = true;
            gridViewTextBoxColumn1.Name = "CustomerID";
            gridViewTextBoxColumn1.Width = 204;
            gridViewTextBoxColumn2.FieldName = "CompanyName";
            gridViewTextBoxColumn2.HeaderText = "CompanyName";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "CompanyName";
            gridViewTextBoxColumn2.Width = 204;
            gridViewTextBoxColumn3.FieldName = "Address";
            gridViewTextBoxColumn3.HeaderText = "Address";
            gridViewTextBoxColumn3.IsAutoGenerated = true;
            gridViewTextBoxColumn3.Name = "Address";
            gridViewTextBoxColumn3.Width = 204;
            gridViewTextBoxColumn4.FieldName = "City";
            gridViewTextBoxColumn4.HeaderText = "City";
            gridViewTextBoxColumn4.IsAutoGenerated = true;
            gridViewTextBoxColumn4.Name = "City";
            gridViewTextBoxColumn4.Width = 204;
            gridViewTextBoxColumn5.FieldName = "Region";
            gridViewTextBoxColumn5.HeaderText = "Region";
            gridViewTextBoxColumn5.IsAutoGenerated = true;
            gridViewTextBoxColumn5.Name = "Region";
            gridViewTextBoxColumn5.Width = 204;
            gridViewTextBoxColumn6.FieldName = "Country";
            gridViewTextBoxColumn6.HeaderText = "Country";
            gridViewTextBoxColumn6.IsAutoGenerated = true;
            gridViewTextBoxColumn6.Name = "Country";
            gridViewTextBoxColumn6.Width = 204;
            gridViewTextBoxColumn7.FieldName = "Phone";
            gridViewTextBoxColumn7.HeaderText = "Phone";
            gridViewTextBoxColumn7.IsAutoGenerated = true;
            gridViewTextBoxColumn7.Name = "Phone";
            gridViewTextBoxColumn7.Width = 205;
            this.radGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7});
            this.radGridView.MasterTemplate.DataSource = this.customersBindingSource;
            this.radGridView.Name = "radGridView";
            // 
            // 
            // 
            this.radGridView.RootElement.ForeColor = System.Drawing.Color.Black;
            this.radGridView.Size = new System.Drawing.Size(1619, 725);
            this.radGridView.TabIndex = 0;
            this.radGridView.Text = "radGridView1";
          
            // 
            // customersBindingSource
            // 
            this.customersBindingSource.DataMember = "Customers";
            this.customersBindingSource.DataSource = this.nwindRadGridView;
            // 
            // nwindRadGridView
            // 
            this.nwindRadGridView.DataSetName = "NwindRadGridView";
            this.nwindRadGridView.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGridView);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1647, 749);

            this.Controls.SetChildIndex(this.radGridView, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwindRadGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView radGridView;
		private System.Windows.Forms.BindingSource customersBindingSource;
		private Telerik.Examples.WinControls.DataSources.NorthwindDataSet nwindRadGridView;
        private Telerik.Examples.WinControls.DataSources.NorthwindDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
    }
}