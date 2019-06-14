namespace AIP.GUI.Forms
{
    partial class AbbrevVersion
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AbbrevVersion));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fm_rgv = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.fm_rgv, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(897, 508);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // fm_rgv
            // 
            this.fm_rgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fm_rgv.Location = new System.Drawing.Point(3, 43);
            // 
            // 
            // 
            this.fm_rgv.MasterTemplate.AllowAddNewRow = false;
            this.fm_rgv.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.fm_rgv.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.fm_rgv.Name = "fm_rgv";
            this.fm_rgv.Size = new System.Drawing.Size(891, 462);
            this.fm_rgv.TabIndex = 0;
            this.fm_rgv.Text = "radGridView1";
            this.fm_rgv.ThemeName = "Office2013Light";
            this.fm_rgv.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.fm_rgv_CellFormatting);
            this.fm_rgv.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.fm_rgv_CellDoubleClick);
            // 
            // radPanel1
            // 
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(891, 34);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.ThemeName = "Office2013Light";
            // 
            // AbbrevVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 508);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AbbrevVersion";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Abbreviation History";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.LanguageManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView fm_rgv;
        private Telerik.WinControls.UI.RadPanel radPanel1;
    }
}
