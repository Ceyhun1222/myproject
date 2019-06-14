namespace AIP.GUI.Forms
{
    partial class AICForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AICForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radDataEntry1 = new Telerik.WinControls.UI.RadDataEntry();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEdit = new Telerik.WinControls.UI.RadButton();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).BeginInit();
            this.radDataEntry1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 604F));
            this.tableLayoutPanel1.Controls.Add(this.radDataEntry1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.530612F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.46939F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(614, 539);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radDataEntry1
            // 
            this.radDataEntry1.AllowAutomaticScrollToControl = false;
            this.radDataEntry1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radDataEntry1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radDataEntry1.Location = new System.Drawing.Point(3, 38);
            this.radDataEntry1.Name = "radDataEntry1";
            // 
            // radDataEntry1.PanelContainer
            // 
            this.radDataEntry1.PanelContainer.Size = new System.Drawing.Size(606, 496);
            this.radDataEntry1.Size = new System.Drawing.Size(608, 498);
            this.radDataEntry1.TabIndex = 5;
            this.radDataEntry1.Text = "radDataEntry1";
            this.radDataEntry1.ThemeName = "Office2013Light";
            this.radDataEntry1.EditorInitializing += new Telerik.WinControls.UI.EditorInitializingEventHandler(this.radDataEntry1_EditorInitializing);
            this.radDataEntry1.ItemInitialized += new Telerik.WinControls.UI.ItemInitializedEventHandler(this.radDataEntry1_ItemInitialized);
            this.radDataEntry1.BindingCreating += new Telerik.WinControls.UI.BindingCreatingEventHandler(this.radDataEntry1_BindingCreating);
            this.radDataEntry1.BindingCreated += new Telerik.WinControls.UI.BindingCreatedEventHandler(this.radDataEntry1_BindingCreated);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btn_Save);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 29);
            this.panel1.TabIndex = 6;
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnEdit.Location = new System.Drawing.Point(0, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(156, 29);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit Circular";
            this.btnEdit.ThemeName = "Office2013Light";
            this.btnEdit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Save.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_Save.Location = new System.Drawing.Point(498, 0);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(110, 29);
            this.btn_Save.TabIndex = 4;
            this.btn_Save.Text = "Save";
            this.btn_Save.ThemeName = "Office2013Light";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // AICForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(614, 539);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AICForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Circular form";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileManagerForm_FormClosing);
            this.Load += new System.EventHandler(this.UniFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radDataEntry1)).EndInit();
            this.radDataEntry1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private Telerik.WinControls.UI.RadDataEntry radDataEntry1;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadButton btnEdit;
    }
}
