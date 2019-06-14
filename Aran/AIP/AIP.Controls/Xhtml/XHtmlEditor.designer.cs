namespace AIP.BaseLib
{
    partial class XHtmlEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        internal void InitializeComponent()
        {
            this.Editor = new Telerik.WinControls.UI.RadRichTextEditor();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.btn_Bold = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.btn_Italic = new Telerik.WinControls.UI.CommandBarToggleButton();
            this.btn_Symbols = new Telerik.WinControls.UI.CommandBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
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
            this.Editor.SelectionStroke = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.Editor.Size = new System.Drawing.Size(394, 64);
            this.Editor.TabIndex = 2;
            this.Editor.ThemeName = "Office2013Light";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.20926F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.79074F));
            this.tableLayoutPanel1.Controls.Add(this.radCommandBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Editor, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 100);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.BackColor = System.Drawing.SystemColors.Menu;
            this.tableLayoutPanel1.SetColumnSpan(this.radCommandBar1, 2);
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Margin = new System.Windows.Forms.Padding(0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(400, 32);
            this.radCommandBar1.TabIndex = 7;
            this.radCommandBar1.ThemeName = "Office2013Light";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.BackColor2 = System.Drawing.SystemColors.Menu;
            this.commandBarRowElement1.BackColor4 = System.Drawing.SystemColors.Menu;
            this.commandBarRowElement1.BorderInnerColor = System.Drawing.SystemColors.Menu;
            this.commandBarRowElement1.BorderInnerColor2 = System.Drawing.SystemColors.Menu;
            this.commandBarRowElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarRowElement1.Margin = new System.Windows.Forms.Padding(0);
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            this.commandBarRowElement1.Text = "";
            this.commandBarRowElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.BackColor = System.Drawing.SystemColors.Menu;
            this.commandBarStripElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btn_Bold,
            this.btn_Italic,
            this.commandBarSeparator1,
            this.btn_Symbols});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement1.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.commandBarStripElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElement1.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // btn_Bold
            // 
            this.btn_Bold.BackColor = System.Drawing.SystemColors.Menu;
            this.btn_Bold.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Bold.DisplayName = "commandBarToggleButton1";
            this.btn_Bold.DrawText = false;
            this.btn_Bold.Image = global::AIP.BaseLib.Properties.Resources.text_bold;
            this.btn_Bold.Name = "btn_Bold";
            this.btn_Bold.Text = "Bold";
            this.btn_Bold.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Bold.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // btn_Italic
            // 
            this.btn_Italic.BackColor = System.Drawing.SystemColors.Menu;
            this.btn_Italic.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Italic.DisplayName = "commandBarToggleButton1";
            this.btn_Italic.Image = global::AIP.BaseLib.Properties.Resources.text_italic;
            this.btn_Italic.Name = "btn_Italic";
            this.btn_Italic.Text = "";
            this.btn_Italic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btn_Italic.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.ToggleStateChanged);
            // 
            // btn_Symbols
            // 
            this.btn_Symbols.BackColor = System.Drawing.SystemColors.Menu;
            this.btn_Symbols.DisplayName = "commandBarButton1";
            this.btn_Symbols.Image = global::AIP.BaseLib.Properties.Resources.text_symbols;
            this.btn_Symbols.Name = "btn_Symbols";
            this.btn_Symbols.Text = "";
            this.btn_Symbols.Click += new System.EventHandler(this.Button_Click);
            // 
            // XHtmlEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "XHtmlEditor";
            this.Size = new System.Drawing.Size(400, 100);
            this.Load += new System.EventHandler(this.RadForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadRichTextEditor Editor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarToggleButton btn_Italic;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        internal Telerik.WinControls.UI.CommandBarToggleButton btn_Bold;
        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.WinControls.UI.CommandBarButton btn_Symbols;
    }
}
