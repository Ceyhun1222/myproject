namespace Telerik.Examples.WinControls.ChartView.Printing
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
            this.buttonPrint = new Telerik.WinControls.UI.RadButton();
            this.buttonPrintPreview = new Telerik.WinControls.UI.RadButton();
            this.buttonPrintSettings = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.radButtonExport = new Telerik.WinControls.UI.RadButton();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.radSpinEditorWidth = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radSpinEditorHeight = new Telerik.WinControls.UI.RadSpinEditor();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrintPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrintSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupBox2);
            this.settingsPanel.Controls.Add(this.radGroupBox1);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsPanel.Location = new System.Drawing.Point(1585, 0);
            this.settingsPanel.Size = new System.Drawing.Size(286, 1086);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox1, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox2, 0);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPrint.Location = new System.Drawing.Point(5, 21);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(256, 24);
            this.buttonPrint.TabIndex = 1;
            this.buttonPrint.Text = "Print";
            // 
            // buttonPrintPreview
            // 
            this.buttonPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPrintPreview.Location = new System.Drawing.Point(5, 51);
            this.buttonPrintPreview.Name = "buttonPrintPreview";
            this.buttonPrintPreview.Size = new System.Drawing.Size(256, 24);
            this.buttonPrintPreview.TabIndex = 1;
            this.buttonPrintPreview.Text = "Print Preview";
            // 
            // buttonPrintSettings
            // 
            this.buttonPrintSettings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonPrintSettings.Location = new System.Drawing.Point(5, 81);
            this.buttonPrintSettings.Name = "buttonPrintSettings";
            this.buttonPrintSettings.Size = new System.Drawing.Size(256, 24);
            this.buttonPrintSettings.TabIndex = 1;
            this.buttonPrintSettings.Text = "Print Settings";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupBox1.Controls.Add(this.buttonPrint);
            this.radGroupBox1.Controls.Add(this.buttonPrintSettings);
            this.radGroupBox1.Controls.Add(this.buttonPrintPreview);
            this.radGroupBox1.HeaderText = "Printing";
            this.radGroupBox1.Location = new System.Drawing.Point(10, 32);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(266, 118);
            this.radGroupBox1.TabIndex = 2;
            this.radGroupBox1.Text = "Printing";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupBox2.Controls.Add(this.radSpinEditorHeight);
            this.radGroupBox2.Controls.Add(this.radSpinEditorWidth);
            this.radGroupBox2.Controls.Add(this.radButtonExport);
            this.radGroupBox2.Controls.Add(this.radLabel3);
            this.radGroupBox2.Controls.Add(this.radLabel2);
            this.radGroupBox2.Controls.Add(this.radLabel1);
            this.radGroupBox2.Controls.Add(this.radDropDownList1);
            this.radGroupBox2.HeaderText = "Export";
            this.radGroupBox2.Location = new System.Drawing.Point(10, 166);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(266, 214);
            this.radGroupBox2.TabIndex = 3;
            this.radGroupBox2.Text = "Export";
            // 
            // radButtonExport
            // 
            this.radButtonExport.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radButtonExport.Location = new System.Drawing.Point(5, 179);
            this.radButtonExport.Name = "radButtonExport";
            this.radButtonExport.Size = new System.Drawing.Size(256, 24);
            this.radButtonExport.TabIndex = 6;
            this.radButtonExport.Text = "Export";
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel2.Location = new System.Drawing.Point(5, 74);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(35, 18);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "Width";
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel1.Location = new System.Drawing.Point(5, 22);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(73, 18);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Image format";
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.radDropDownList1.Location = new System.Drawing.Point(5, 47);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(256, 20);
            this.radDropDownList1.TabIndex = 0;
            this.radDropDownList1.Text = "radDropDownList1";
            // 
            // radSpinEditorWidth
            // 
            this.radSpinEditorWidth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radSpinEditorWidth.Location = new System.Drawing.Point(5, 99);
            this.radSpinEditorWidth.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.radSpinEditorWidth.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.radSpinEditorWidth.Name = "radSpinEditorWidth";
            this.radSpinEditorWidth.Size = new System.Drawing.Size(256, 20);
            this.radSpinEditorWidth.TabIndex = 7;
            this.radSpinEditorWidth.TabStop = false;
            this.radSpinEditorWidth.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // radLabel3
            // 
            this.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel3.Location = new System.Drawing.Point(5, 125);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(39, 18);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "Height";
            // 
            // radSpinEditorHeight
            // 
            this.radSpinEditorHeight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radSpinEditorHeight.Location = new System.Drawing.Point(5, 150);
            this.radSpinEditorHeight.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.radSpinEditorHeight.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.radSpinEditorHeight.Name = "radSpinEditorHeight";
            this.radSpinEditorHeight.Size = new System.Drawing.Size(256, 20);
            this.radSpinEditorHeight.TabIndex = 7;
            this.radSpinEditorHeight.TabStop = false;
            this.radSpinEditorHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1881, 1096);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrintPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPrintSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton buttonPrint;
        private Telerik.WinControls.UI.RadButton buttonPrintPreview;
        private Telerik.WinControls.UI.RadButton buttonPrintSettings;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadButton radButtonExport;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadSpinEditor radSpinEditorHeight;
        private Telerik.WinControls.UI.RadSpinEditor radSpinEditorWidth;
        private Telerik.WinControls.UI.RadLabel radLabel3;
    }
}