namespace AIP.DataSet
{
    partial class DataSetViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSetViewer));
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarLabel1 = new Telerik.WinControls.UI.CommandBarLabel();
            this.lbl_Count = new Telerik.WinControls.UI.CommandBarLabel();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.lbl_Hash = new Telerik.WinControls.UI.CommandBarLabel();
            this.pnl_Menu = new Telerik.WinControls.UI.RadPanel();
            this.btn_HideComplex = new Telerik.WinControls.UI.RadButton();
            this.wb_bar = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsLineWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsLineWaitingBarIndicatorElement();
            this.rgv_DataSet = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cbx_DataSet = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Menu)).BeginInit();
            this.pnl_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_HideComplex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wb_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_DataSet.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.radCommandBar1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnl_Menu, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rgv_DataSet, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 579);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCommandBar1.Location = new System.Drawing.Point(3, 550);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(244, 26);
            this.radCommandBar1.TabIndex = 1;
            this.radCommandBar1.Text = "radCommandBar1";
            this.radCommandBar1.ThemeName = "Office2013Light";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarLabel1,
            this.lbl_Count,
            this.commandBarSeparator1,
            this.lbl_Hash});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement1.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.commandBarStripElement1.ShowHorizontalLine = true;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElement1.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // commandBarLabel1
            // 
            this.commandBarLabel1.DisplayName = "commandBarLabel1";
            this.commandBarLabel1.Name = "commandBarLabel1";
            this.commandBarLabel1.Text = "Total: ";
            // 
            // lbl_Count
            // 
            this.lbl_Count.DisplayName = "commandBarLabel2";
            this.lbl_Count.Name = "lbl_Count";
            this.lbl_Count.Text = "";
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // lbl_Hash
            // 
            this.lbl_Hash.BackColor = System.Drawing.SystemColors.Window;
            this.lbl_Hash.DisplayName = "commandBarLabel2";
            this.lbl_Hash.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_Hash.Name = "lbl_Hash";
            this.lbl_Hash.Text = "";
            // 
            // pnl_Menu
            // 
            this.pnl_Menu.Controls.Add(this.btn_HideComplex);
            this.pnl_Menu.Controls.Add(this.wb_bar);
            this.pnl_Menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Menu.Location = new System.Drawing.Point(253, 3);
            this.pnl_Menu.Name = "pnl_Menu";
            this.pnl_Menu.Size = new System.Drawing.Size(591, 44);
            this.pnl_Menu.TabIndex = 0;
            this.pnl_Menu.ThemeName = "Office2013Light";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnl_Menu.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btn_HideComplex
            // 
            this.btn_HideComplex.Location = new System.Drawing.Point(3, 21);
            this.btn_HideComplex.Name = "btn_HideComplex";
            this.btn_HideComplex.Size = new System.Drawing.Size(159, 23);
            this.btn_HideComplex.TabIndex = 7;
            this.btn_HideComplex.Text = "Hide list type`s columns";
            this.btn_HideComplex.ThemeName = "Office2013Light";
            this.btn_HideComplex.Visible = false;
            this.btn_HideComplex.Click += new System.EventHandler(this.btn_HideComplex_Click);
            // 
            // wb_bar
            // 
            this.wb_bar.Dock = System.Windows.Forms.DockStyle.Right;
            this.wb_bar.Location = new System.Drawing.Point(385, 0);
            this.wb_bar.Name = "wb_bar";
            this.wb_bar.Size = new System.Drawing.Size(206, 44);
            this.wb_bar.TabIndex = 5;
            this.wb_bar.Text = "radWaitingBar1";
            this.wb_bar.WaitingIndicators.Add(this.dotsLineWaitingBarIndicatorElement1);
            this.wb_bar.WaitingSpeed = 80;
            this.wb_bar.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsLine;
            // 
            // dotsLineWaitingBarIndicatorElement1
            // 
            this.dotsLineWaitingBarIndicatorElement1.Name = "dotsLineWaitingBarIndicatorElement1";
            this.dotsLineWaitingBarIndicatorElement1.Text = "Loading...";
            this.dotsLineWaitingBarIndicatorElement1.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rgv_DataSet
            // 
            this.rgv_DataSet.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.rgv_DataSet, 2);
            this.rgv_DataSet.Cursor = System.Windows.Forms.Cursors.Default;
            this.rgv_DataSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgv_DataSet.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rgv_DataSet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rgv_DataSet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rgv_DataSet.Location = new System.Drawing.Point(3, 53);
            // 
            // 
            // 
            this.rgv_DataSet.MasterTemplate.AllowAddNewRow = false;
            this.rgv_DataSet.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgv_DataSet.Name = "rgv_DataSet";
            this.rgv_DataSet.ReadOnly = true;
            this.rgv_DataSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rgv_DataSet.Size = new System.Drawing.Size(841, 491);
            this.rgv_DataSet.TabIndex = 3;
            this.rgv_DataSet.Text = "radGridView1";
            this.rgv_DataSet.ThemeName = "Office2013Light";
            this.rgv_DataSet.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.rgv_DataSet_CellDoubleClick);
            this.rgv_DataSet.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.rgv_DataSet_ContextMenuOpening);
            this.rgv_DataSet.DataBindingComplete += new Telerik.WinControls.UI.GridViewBindingCompleteEventHandler(this.rgv_DataSet_DataBindingComplete);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.cbx_DataSet);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(244, 44);
            this.radPanel1.TabIndex = 4;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radLabel1
            // 
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radLabel1.Location = new System.Drawing.Point(0, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(244, 19);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "Select feature to analyze";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // cbx_DataSet
            // 
            this.cbx_DataSet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbx_DataSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_DataSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_DataSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbx_DataSet.FormattingEnabled = true;
            this.cbx_DataSet.Location = new System.Drawing.Point(0, 20);
            this.cbx_DataSet.Name = "cbx_DataSet";
            this.cbx_DataSet.Size = new System.Drawing.Size(244, 24);
            this.cbx_DataSet.TabIndex = 2;
            this.cbx_DataSet.SelectedIndexChanged += new System.EventHandler(this.cbx_DataSet_SelectedIndexChanged);
            // 
            // DataSetViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 579);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataSetViewer";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Data Set Viewer";
            this.ThemeName = "Office2013Light";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataSetViewer_FormClosing);
            this.Load += new System.EventHandler(this.DataSetManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Menu)).EndInit();
            this.pnl_Menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_HideComplex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wb_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_DataSet.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadPanel pnl_Menu;
        private System.Windows.Forms.ComboBox cbx_DataSet;
        private Telerik.WinControls.UI.RadGridView rgv_DataSet;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarLabel commandBarLabel1;
        private Telerik.WinControls.UI.CommandBarLabel lbl_Count;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadWaitingBar wb_bar;
        private Telerik.WinControls.UI.DotsLineWaitingBarIndicatorElement dotsLineWaitingBarIndicatorElement1;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarLabel lbl_Hash;
        private Telerik.WinControls.UI.RadButton btn_HideComplex;
    }
}
