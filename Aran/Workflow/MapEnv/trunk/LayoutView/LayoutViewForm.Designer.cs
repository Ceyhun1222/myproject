namespace MapEnv.LayoutView
{
    partial class LayoutViewForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem printSetupToolStripMenuItem;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.ToolStripButton toolStripButton1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutViewForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ui_esriLayoutControl = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.ui_esriLayoutToolbarControl = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.ui_esriMapToolbarControl = new ESRI.ArcGIS.Controls.AxToolbarControl();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriLayoutControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriLayoutToolbarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapToolbarControl)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 17);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(55, 17);
            label1.TabIndex = 12;
            label1.Text = "Layout:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(356, 16);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 17);
            label2.TabIndex = 13;
            label2.Text = "Map:";
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            printToolStripMenuItem.Text = "Print";
            printToolStripMenuItem.Click += new System.EventHandler(this.Print_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            printPreviewToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            printPreviewToolStripMenuItem.Text = "Print Preview";
            printPreviewToolStripMenuItem.Click += new System.EventHandler(this.PrintPreview_Click);
            // 
            // printSetupToolStripMenuItem
            // 
            printSetupToolStripMenuItem.Name = "printSetupToolStripMenuItem";
            printSetupToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            printSetupToolStripMenuItem.Text = "Page Setup";
            printSetupToolStripMenuItem.Click += new System.EventHandler(this.PageSetup_Click);
            // 
            // panel1
            // 
            panel1.Controls.Add(this.toolStrip1);
            panel1.Location = new System.Drawing.Point(715, 5);
            panel1.Margin = new System.Windows.Forms.Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(264, 38);
            panel1.TabIndex = 16;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(5, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(259, 38);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            printToolStripMenuItem,
            printPreviewToolStripMenuItem,
            printSetupToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::MapEnv.Properties.Resources.print_16;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(73, 35);
            this.toolStripDropDownButton1.Text = "Print";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Image = global::MapEnv.Properties.Resources.arcmap_16;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(141, 35);
            toolStripButton1.Text = "Open in ArcMap";
            toolStripButton1.Click += new System.EventHandler(this.OpenInArcMap_Click);
            // 
            // ui_esriLayoutControl
            // 
            this.ui_esriLayoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_esriLayoutControl.Location = new System.Drawing.Point(1, 40);
            this.ui_esriLayoutControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_esriLayoutControl.Name = "ui_esriLayoutControl";
            this.ui_esriLayoutControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriLayoutControl.OcxState")));
            this.ui_esriLayoutControl.Size = new System.Drawing.Size(1174, 630);
            this.ui_esriLayoutControl.TabIndex = 9;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(850, 5);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(4);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 8;
            // 
            // ui_esriLayoutToolbarControl
            // 
            this.ui_esriLayoutToolbarControl.Location = new System.Drawing.Point(47, 6);
            this.ui_esriLayoutToolbarControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_esriLayoutToolbarControl.Name = "ui_esriLayoutToolbarControl";
            this.ui_esriLayoutToolbarControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriLayoutToolbarControl.OcxState")));
            this.ui_esriLayoutToolbarControl.Size = new System.Drawing.Size(372, 28);
            this.ui_esriLayoutToolbarControl.TabIndex = 10;
            // 
            // ui_esriMapToolbarControl
            // 
            this.ui_esriMapToolbarControl.Location = new System.Drawing.Point(427, 9);
            this.ui_esriMapToolbarControl.Margin = new System.Windows.Forms.Padding(4);
            this.ui_esriMapToolbarControl.Name = "ui_esriMapToolbarControl";
            this.ui_esriMapToolbarControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("ui_esriMapToolbarControl.OcxState")));
            this.ui_esriMapToolbarControl.Size = new System.Drawing.Size(289, 28);
            this.ui_esriMapToolbarControl.TabIndex = 11;
            // 
            // LayoutViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 674);
            this.Controls.Add(panel1);
            this.Controls.Add(this.ui_esriMapToolbarControl);
            this.Controls.Add(this.ui_esriLayoutToolbarControl);
            this.Controls.Add(this.ui_esriLayoutControl);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LayoutViewForm";
            this.Text = "Layout View";
            this.Load += new System.EventHandler(this.LayoutViewForm_Load);
            panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriLayoutControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriLayoutToolbarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ui_esriMapToolbarControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxPageLayoutControl ui_esriLayoutControl;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl ui_esriLayoutToolbarControl;
        private ESRI.ArcGIS.Controls.AxToolbarControl ui_esriMapToolbarControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
    }
}