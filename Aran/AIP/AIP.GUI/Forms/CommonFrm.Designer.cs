namespace AIP.GUI.Forms
{
    partial class CommonFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonFrm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.radDataEntry1 = new Telerik.WinControls.UI.RadDataEntry();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.MainTabControl = new Telerik.WinControls.UI.RadPageView();
            this.PV1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).BeginInit();
            this.radDataEntry1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainTabControl)).BeginInit();
            this.MainTabControl.SuspendLayout();
            this.PV1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 604F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Save, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radDataEntry1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.530612F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.46939F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(604, 520);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Save.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Save.Location = new System.Drawing.Point(3, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(110, 27);
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "Save";
            this.btn_Save.ThemeName = "Office2013Light";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // radDataEntry1
            // 
            this.radDataEntry1.AllowAutomaticScrollToControl = false;
            this.radDataEntry1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radDataEntry1.Location = new System.Drawing.Point(3, 36);
            this.radDataEntry1.Name = "radDataEntry1";
            // 
            // radDataEntry1.PanelContainer
            // 
            this.radDataEntry1.PanelContainer.Size = new System.Drawing.Size(564, 479);
            this.radDataEntry1.Size = new System.Drawing.Size(566, 481);
            this.radDataEntry1.TabIndex = 5;
            this.radDataEntry1.Text = "radDataEntry1";
            this.radDataEntry1.ThemeName = "Office2013Light";
            this.radDataEntry1.EditorInitializing += new Telerik.WinControls.UI.EditorInitializingEventHandler(this.radDataEntry1_EditorInitializing);
            this.radDataEntry1.ItemInitialized += new Telerik.WinControls.UI.ItemInitializedEventHandler(this.radDataEntry1_ItemInitialized);
            this.radDataEntry1.BindingCreating += new Telerik.WinControls.UI.BindingCreatingEventHandler(this.radDataEntry1_BindingCreating);
            this.radDataEntry1.BindingCreated += new Telerik.WinControls.UI.BindingCreatedEventHandler(this.radDataEntry1_BindingCreated);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.PV1);
            this.MainTabControl.DefaultPage = this.PV1;
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedPage = this.PV1;
            this.MainTabControl.Size = new System.Drawing.Size(614, 539);
            this.MainTabControl.TabIndex = 0;
            this.MainTabControl.ThemeName = "Office2013Light";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.MainTabControl.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            // 
            // PV1
            // 
            this.PV1.Controls.Add(this.tableLayoutPanel1);
            this.PV1.ItemSize = new System.Drawing.SizeF(10F, 10F);
            this.PV1.Location = new System.Drawing.Point(5, 14);
            this.PV1.Name = "PV1";
            this.PV1.Size = new System.Drawing.Size(604, 520);
            // 
            // CommonFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(614, 539);
            this.Controls.Add(this.MainTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommonFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Common form";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CommonFrm_FormClosing);
            this.Load += new System.EventHandler(this.UniFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).EndInit();
            this.radDataEntry1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainTabControl)).EndInit();
            this.MainTabControl.ResumeLayout(false);
            this.PV1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private Telerik.WinControls.UI.RadDataEntry radDataEntry1;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.WinControls.UI.RadPageView MainTabControl;
        private Telerik.WinControls.UI.RadPageViewPage PV1;
    }
}
