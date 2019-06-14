namespace AIP.GUI.Forms
{
    partial class LanguageManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageManager));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lm_rgv = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.cbx_Lang = new System.Windows.Forms.ComboBox();
            this.btn_Install = new Telerik.WinControls.UI.RadButton();
            this.btn_Upgrade = new Telerik.WinControls.UI.RadButton();
            this.btn_Export = new Telerik.WinControls.UI.RadButton();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lm_rgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lm_rgv.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Install)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Upgrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Export)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lm_rgv, 0, 1);
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
            // lm_rgv
            // 
            this.lm_rgv.AutoSizeRows = true;
            this.lm_rgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lm_rgv.Location = new System.Drawing.Point(3, 43);
            // 
            // 
            // 
            this.lm_rgv.MasterTemplate.AllowAddNewRow = false;
            this.lm_rgv.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.lm_rgv.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.lm_rgv.Name = "lm_rgv";
            this.lm_rgv.Size = new System.Drawing.Size(891, 462);
            this.lm_rgv.TabIndex = 0;
            this.lm_rgv.Text = "radGridView1";
            this.lm_rgv.ThemeName = "Office2013Light";
            this.lm_rgv.CellEditorInitialized += new Telerik.WinControls.UI.GridViewCellEventHandler(this.lm_rgv_CellEditorInitialized);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.cbx_Lang);
            this.radPanel1.Controls.Add(this.btn_Install);
            this.radPanel1.Controls.Add(this.btn_Upgrade);
            this.radPanel1.Controls.Add(this.btn_Export);
            this.radPanel1.Controls.Add(this.btn_Save);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(891, 34);
            this.radPanel1.TabIndex = 1;
            // 
            // cbx_Lang
            // 
            this.cbx_Lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Lang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_Lang.FormattingEnabled = true;
            this.cbx_Lang.Location = new System.Drawing.Point(10, 10);
            this.cbx_Lang.Name = "cbx_Lang";
            this.cbx_Lang.Size = new System.Drawing.Size(121, 21);
            this.cbx_Lang.TabIndex = 4;
            this.cbx_Lang.SelectedIndexChanged += new System.EventHandler(this.cbx_Lang_SelectedIndexChanged);
            // 
            // btn_Install
            // 
            this.btn_Install.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Install.Location = new System.Drawing.Point(355, 0);
            this.btn_Install.Name = "btn_Install";
            this.btn_Install.Size = new System.Drawing.Size(158, 34);
            this.btn_Install.TabIndex = 3;
            this.btn_Install.Text = "New Installation";
            this.btn_Install.ThemeName = "Office2013Light";
            this.btn_Install.Click += new System.EventHandler(this.btn_Install_Click);
            // 
            // btn_Upgrade
            // 
            this.btn_Upgrade.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Upgrade.Location = new System.Drawing.Point(513, 0);
            this.btn_Upgrade.Name = "btn_Upgrade";
            this.btn_Upgrade.Size = new System.Drawing.Size(158, 34);
            this.btn_Upgrade.TabIndex = 4;
            this.btn_Upgrade.Text = "Upgrade";
            this.btn_Upgrade.ThemeName = "Office2013Light";
            this.btn_Upgrade.Click += new System.EventHandler(this.btn_Upgrade_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Export.Location = new System.Drawing.Point(671, 0);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(110, 34);
            this.btn_Export.TabIndex = 3;
            this.btn_Export.Text = "Export to file";
            this.btn_Export.ThemeName = "Office2013Light";
            this.btn_Export.Visible = false;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Save.Location = new System.Drawing.Point(781, 0);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(110, 34);
            this.btn_Save.TabIndex = 2;
            this.btn_Save.Text = "Save";
            this.btn_Save.ThemeName = "Office2013Light";
            this.btn_Save.Click += new System.EventHandler(this.SaveClick);
            // 
            // LanguageManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 508);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LanguageManager";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "LanguageManager";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.LanguageManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lm_rgv.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lm_rgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Install)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Upgrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Export)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView lm_rgv;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton btn_Export;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private Telerik.WinControls.UI.RadButton btn_Install;
        private System.Windows.Forms.ComboBox cbx_Lang;
        private Telerik.WinControls.UI.RadButton btn_Upgrade;
    }
}
