namespace AIP.DataSet
{
    partial class DataSetManager
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
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.tvProps = new Telerik.WinControls.UI.RadTreeView();
            this.btn_SaveXml = new Telerik.WinControls.UI.RadButton();
            this.btn_OpenXml = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.cbx_DataSet = new System.Windows.Forms.ComboBox();
            this.btn_Exit = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.tvProps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_SaveXml)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OpenXml)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Exit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tvProps
            // 
            this.tvProps.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.tvProps.CheckBoxes = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tvProps, 2);
            this.tvProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProps.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tvProps.Location = new System.Drawing.Point(3, 35);
            this.tvProps.Name = "tvProps";
            this.tvProps.Size = new System.Drawing.Size(841, 541);
            this.tvProps.SpacingBetweenNodes = -1;
            this.tvProps.TabIndex = 1;
            this.tvProps.Text = "tvProps";
            this.tvProps.NodeCheckedChanged += new Telerik.WinControls.UI.TreeNodeCheckedEventHandler(this.tvProps_NodeCheckedChanged);
            // 
            // btn_SaveXml
            // 
            this.btn_SaveXml.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_SaveXml.Location = new System.Drawing.Point(198, 0);
            this.btn_SaveXml.Name = "btn_SaveXml";
            this.btn_SaveXml.Size = new System.Drawing.Size(153, 26);
            this.btn_SaveXml.TabIndex = 2;
            this.btn_SaveXml.Text = "Save Configuration";
            this.btn_SaveXml.ThemeName = "Office2013Light";
            this.btn_SaveXml.Click += new System.EventHandler(this.btn_SaveXml_Click);
            // 
            // btn_OpenXml
            // 
            this.btn_OpenXml.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_OpenXml.Location = new System.Drawing.Point(45, 0);
            this.btn_OpenXml.Name = "btn_OpenXml";
            this.btn_OpenXml.Size = new System.Drawing.Size(153, 26);
            this.btn_OpenXml.TabIndex = 3;
            this.btn_OpenXml.Text = "Load Configuration";
            this.btn_OpenXml.ThemeName = "Office2013Light";
            this.btn_OpenXml.Click += new System.EventHandler(this.btn_OpenXml_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 337F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tvProps, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbx_DataSet, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 579);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.btn_OpenXml);
            this.radPanel1.Controls.Add(this.btn_SaveXml);
            this.radPanel1.Controls.Add(this.btn_Exit);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(340, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(504, 26);
            this.radPanel1.TabIndex = 0;
            // 
            // cbx_DataSet
            // 
            this.cbx_DataSet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbx_DataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_DataSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_DataSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbx_DataSet.FormattingEnabled = true;
            this.cbx_DataSet.Location = new System.Drawing.Point(3, 3);
            this.cbx_DataSet.Name = "cbx_DataSet";
            this.cbx_DataSet.Size = new System.Drawing.Size(246, 24);
            this.cbx_DataSet.TabIndex = 2;
            this.cbx_DataSet.SelectedIndexChanged += new System.EventHandler(this.cbx_DataSet_SelectedIndexChanged);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Exit.Location = new System.Drawing.Point(351, 0);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(153, 26);
            this.btn_Exit.TabIndex = 3;
            this.btn_Exit.Text = "Close";
            this.btn_Exit.ThemeName = "Office2013Light";
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // DataSetManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 579);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DataSetManager";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "DataSet Manager";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DataSetManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tvProps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_SaveXml)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OpenXml)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Exit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.WinControls.UI.RadTreeView tvProps;
        private Telerik.WinControls.UI.RadButton btn_SaveXml;
        private Telerik.WinControls.UI.RadButton btn_OpenXml;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.ComboBox cbx_DataSet;
        private Telerik.WinControls.UI.RadButton btn_Exit;
    }
}
