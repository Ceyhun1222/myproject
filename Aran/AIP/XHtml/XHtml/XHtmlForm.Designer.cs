namespace XHTML
{
    partial class XHtmlForm
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
            this.Editor = new Telerik.WinControls.UI.RadRichTextEditor();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.btn_Bold = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.btn_Italic = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.btn_ul = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.btn_ol = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.btn_Table = new Telerik.WinControls.UI.CommandBarButton();
            this.btn_Link = new Telerik.WinControls.UI.CommandBarButton();
            this.btn_OK = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.Editor.AutoScroll = true;
            this.Editor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(189)))), ((int)(((byte)(232)))));
            this.Editor.CaretWidth = float.NaN;
            this.tableLayoutPanel1.SetColumnSpan(this.Editor, 2);
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Location = new System.Drawing.Point(3, 33);
            this.Editor.Name = "Editor";
            this.Editor.SelectionFill = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(78)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.Editor.Size = new System.Drawing.Size(938, 496);
            this.Editor.TabIndex = 2;
            this.Editor.TextChanged += new System.EventHandler(this.radRichTextEditor1_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.20926F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.79074F));
            this.tableLayoutPanel1.Controls.Add(this.btn_Cancel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.radCommandBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Editor, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_OK, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.13544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.864564F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(944, 569);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Cancel.Location = new System.Drawing.Point(186, 538);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(110, 24);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Cancel";
            // 
            // radCommandBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.radCommandBar1, 2);
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(3, 3);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(938, 30);
            this.radCommandBar1.TabIndex = 7;
            this.radCommandBar1.Text = "radCommandBar1";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            this.commandBarRowElement1.Text = "";
            this.commandBarRowElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btn_Bold,
            this.btn_Italic,
            this.commandBarSeparator1,
            this.btn_ul,
            this.btn_ol,
            this.commandBarSeparator2,
            this.btn_Table,
            this.btn_Link});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            this.commandBarStripElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // btn_Bold
            // 
            this.btn_Bold.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Bold.DisplayName = "commandBarToggleButton1";
            this.btn_Bold.DrawText = false;
            this.btn_Bold.Image = global::XHTML.Properties.Resources.text_bold;
            this.btn_Bold.Name = "btn_Bold";
            this.btn_Bold.Text = "Bold";
            this.btn_Bold.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Bold.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // btn_Italic
            // 
            this.btn_Italic.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Italic.DisplayName = "commandBarToggleButton1";
            this.btn_Italic.Image = global::XHTML.Properties.Resources.text_italic;
            this.btn_Italic.Name = "btn_Italic";
            this.btn_Italic.Text = "commandBarToggleButton1";
            this.btn_Italic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Italic.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // btn_ul
            // 
            this.btn_ul.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_ul.DisplayName = "commandBarToggleButton1";
            this.btn_ul.Image = global::XHTML.Properties.Resources.ul;
            this.btn_ul.Name = "btn_ul";
            this.btn_ul.Text = "commandBarToggleButton1";
            this.btn_ul.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_ul.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // btn_ol
            // 
            this.btn_ol.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_ol.DisplayName = "commandBarToggleButton2";
            this.btn_ol.Image = global::XHTML.Properties.Resources.ol;
            this.btn_ol.Name = "btn_ol";
            this.btn_ol.Text = "commandBarToggleButton2";
            this.btn_ol.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_ol.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator2.DisplayName = "commandBarSeparator2";
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.commandBarSeparator2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // btn_Table
            // 
            this.btn_Table.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Table.DisplayName = "commandBarButton1";
            this.btn_Table.Image = global::XHTML.Properties.Resources.table_add;
            this.btn_Table.Name = "btn_Table";
            this.btn_Table.Text = "commandBarButton1";
            this.btn_Table.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Table.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn_Link
            // 
            this.btn_Link.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Link.DisplayName = "commandBarButton1";
            this.btn_Link.Image = global::XHTML.Properties.Resources.link;
            this.btn_Link.Name = "btn_Link";
            this.btn_Link.Text = "commandBarButton1";
            this.btn_Link.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Link.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_OK.Location = new System.Drawing.Point(658, 538);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(110, 24);
            this.btn_OK.TabIndex = 8;
            this.btn_OK.Text = "Ok";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // XHtmlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 569);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "XHtmlForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "XHTML Editor";
            this.Load += new System.EventHandler(this.RadForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadRichTextEditor Editor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarToggleButton btn_Bold;
        private Telerik.WinControls.UI.CommandBarToggleButton btn_Italic;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarToggleButton btn_ul;
        private Telerik.WinControls.UI.CommandBarToggleButton btn_ol;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
        private Telerik.WinControls.UI.CommandBarButton btn_Table;
        private Telerik.WinControls.UI.CommandBarButton btn_Link;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private Telerik.WinControls.UI.RadButton btn_OK;
    }
}