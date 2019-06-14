namespace AIP.GUI.Forms
{
    partial class CompareHTML
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareHTML));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.fm_rgv = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.ChoosenFIlesPath = new Telerik.WinControls.UI.RadLabel();
            this.btn_Choose = new Telerik.WinControls.UI.RadButton();
            this.btn_Update = new Telerik.WinControls.UI.RadButton();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.segmentedRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).BeginInit();
            this.fm_rgv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChoosenFIlesPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Choose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Update)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.radGroupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fm_rgv, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(897, 508);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // fm_rgv
            // 
            this.fm_rgv.Controls.Add(this.radWaitingBar1);
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
            this.fm_rgv.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.fm_rgv_ContextMenuOpening);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.btn_Choose);
            this.radPanel1.Controls.Add(this.btn_Update);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.btn_Save);
            this.radPanel1.Controls.Add(this.ChoosenFIlesPath);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(4, 4);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(883, 34);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.ThemeName = "Office2013Light";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // ChoosenFIlesPath
            // 
            this.ChoosenFIlesPath.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChoosenFIlesPath.Location = new System.Drawing.Point(206, 2);
            this.ChoosenFIlesPath.MinimumSize = new System.Drawing.Size(200, 30);
            this.ChoosenFIlesPath.Name = "ChoosenFIlesPath";
            // 
            // 
            // 
            this.ChoosenFIlesPath.RootElement.MinSize = new System.Drawing.Size(200, 30);
            this.ChoosenFIlesPath.Size = new System.Drawing.Size(200, 30);
            this.ChoosenFIlesPath.TabIndex = 6;
            this.ChoosenFIlesPath.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.ChoosenFIlesPath.TextWrap = false;
            // 
            // btn_Choose
            // 
            this.btn_Choose.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Choose.Location = new System.Drawing.Point(0, 0);
            this.btn_Choose.MinimumSize = new System.Drawing.Size(200, 30);
            this.btn_Choose.Name = "btn_Choose";
            this.btn_Choose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.btn_Choose.RootElement.MinSize = new System.Drawing.Size(200, 30);
            this.btn_Choose.Size = new System.Drawing.Size(200, 34);
            this.btn_Choose.TabIndex = 5;
            this.btn_Choose.Text = "Select Comparing eAIP folder";
            this.btn_Choose.ThemeName = "Office2013Light";
            this.btn_Choose.Click += new System.EventHandler(this.btn_Choose_Click);
            // 
            // btn_Update
            // 
            this.btn_Update.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Update.Location = new System.Drawing.Point(683, 0);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(100, 34);
            this.btn_Update.TabIndex = 4;
            this.btn_Update.Text = "Compare eAIPs";
            this.btn_Update.ThemeName = "Office2013Light";
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(0, 0);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(2, 2);
            this.radLabel2.TabIndex = 0;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(0, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(2, 2);
            this.radLabel1.TabIndex = 1;
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Save.Location = new System.Drawing.Point(783, 0);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(100, 34);
            this.btn_Save.TabIndex = 2;
            this.btn_Save.Text = "Open Selected ";
            this.btn_Save.ThemeName = "Office2013Light";
            this.btn_Save.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radPanel1);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox1.HeaderText = "";
            this.radGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.radGroupBox1.Size = new System.Drawing.Size(891, 42);
            this.radGroupBox1.TabIndex = 7;
            this.radGroupBox1.ThemeName = "Office2013Light";
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radWaitingBar1.Location = new System.Drawing.Point(0, 0);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(891, 462);
            this.radWaitingBar1.TabIndex = 1;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.Visible = false;
            this.radWaitingBar1.WaitingIndicators.Add(this.segmentedRingWaitingBarIndicatorElement1);
            this.radWaitingBar1.WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            this.radWaitingBar1.WaitingSpeed = 20;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.SegmentedRing;
            // 
            // segmentedRingWaitingBarIndicatorElement1
            // 
            this.segmentedRingWaitingBarIndicatorElement1.Name = "segmentedRingWaitingBarIndicatorElement1";
            // 
            // CompareHTML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 508);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompareHTML";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Comparison Manager";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.LanguageManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fm_rgv)).EndInit();
            this.fm_rgv.ResumeLayout(false);
            this.fm_rgv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChoosenFIlesPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Choose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Update)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGridView fm_rgv;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btn_Update;
        private Telerik.WinControls.UI.RadButton btn_Choose;
        private Telerik.WinControls.UI.RadLabel ChoosenFIlesPath;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement segmentedRingWaitingBarIndicatorElement1;
    }
}
