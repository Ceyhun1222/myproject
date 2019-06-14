namespace AIP.GUI.Forms
{
    partial class FileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileManager));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fm_rgv = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btn_ChartNumbers = new Telerik.WinControls.UI.RadButton();
            this.btn_History = new Telerik.WinControls.UI.RadButton();
            this.cbx_Lang = new Telerik.WinControls.UI.RadDropDownList();
            this.cbx_Section = new Telerik.WinControls.UI.RadDropDownList();
            this.btn_New = new Telerik.WinControls.UI.RadButton();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ChartNumbers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_History)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbx_Lang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbx_Section)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_New)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(897, 508);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // fm_rgv
            // 
            this.fm_rgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fm_rgv.Location = new System.Drawing.Point(3, 51);
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
            this.fm_rgv.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.fm_rgv_CellDoubleClick);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.btn_ChartNumbers);
            this.radPanel1.Controls.Add(this.btn_History);
            this.radPanel1.Controls.Add(this.cbx_Lang);
            this.radPanel1.Controls.Add(this.cbx_Section);
            this.radPanel1.Controls.Add(this.btn_New);
            this.radPanel1.Controls.Add(this.btn_Save);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(891, 42);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(143, 0);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(63, 18);
            this.radLabel2.TabIndex = 10;
            this.radLabel2.Text = "AIP Section";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(55, 18);
            this.radLabel1.TabIndex = 9;
            this.radLabel1.Text = "Language";
            // 
            // btn_ChartNumbers
            // 
            this.btn_ChartNumbers.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_ChartNumbers.Location = new System.Drawing.Point(491, 0);
            this.btn_ChartNumbers.Name = "btn_ChartNumbers";
            this.btn_ChartNumbers.Size = new System.Drawing.Size(100, 42);
            this.btn_ChartNumbers.TabIndex = 3;
            this.btn_ChartNumbers.Text = "Chart Numbers";
            this.btn_ChartNumbers.ThemeName = "Office2013Light";
            this.btn_ChartNumbers.Click += new System.EventHandler(this.btn_ChartNumbers_Click);
            // 
            // btn_History
            // 
            this.btn_History.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_History.Location = new System.Drawing.Point(591, 0);
            this.btn_History.Name = "btn_History";
            this.btn_History.Size = new System.Drawing.Size(100, 42);
            this.btn_History.TabIndex = 8;
            this.btn_History.Text = "History";
            this.btn_History.ThemeName = "Office2013Light";
            this.btn_History.Click += new System.EventHandler(this.btn_History_Click);
            // 
            // cbx_Lang
            // 
            this.cbx_Lang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cbx_Lang.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cbx_Lang.Location = new System.Drawing.Point(9, 20);
            this.cbx_Lang.Name = "cbx_Lang";
            this.cbx_Lang.Size = new System.Drawing.Size(125, 21);
            this.cbx_Lang.TabIndex = 7;
            this.cbx_Lang.ThemeName = "Office2013Light";
            this.cbx_Lang.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.btn_Filter_Click);
            // 
            // cbx_Section
            // 
            this.cbx_Section.Location = new System.Drawing.Point(140, 20);
            this.cbx_Section.Name = "cbx_Section";
            this.cbx_Section.Size = new System.Drawing.Size(125, 21);
            this.cbx_Section.TabIndex = 6;
            this.cbx_Section.ThemeName = "Office2013Light";
            this.cbx_Section.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.btn_Filter_Click);
            // 
            // btn_New
            // 
            this.btn_New.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_New.Location = new System.Drawing.Point(691, 0);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(100, 42);
            this.btn_New.TabIndex = 3;
            this.btn_New.Text = "New file";
            this.btn_New.ThemeName = "Office2013Light";
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Save.Location = new System.Drawing.Point(791, 0);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(100, 42);
            this.btn_Save.TabIndex = 2;
            this.btn_Save.Text = "Save order";
            this.btn_Save.ThemeName = "Office2013Light";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // FileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 508);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileManager";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "File Manager";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.LanguageManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ChartNumbers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_History)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbx_Lang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbx_Section)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_New)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView fm_rgv;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton btn_ChartNumbers;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private Telerik.WinControls.UI.RadButton btn_New;
        private Telerik.WinControls.UI.RadDropDownList cbx_Lang;
        private Telerik.WinControls.UI.RadDropDownList cbx_Section;
        private Telerik.WinControls.UI.RadButton btn_History;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
