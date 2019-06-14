namespace SpreadGridIntegration
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.headerRowColorBox = new Telerik.WinControls.UI.RadColorBox();
            this.groupHeaderColorBox = new Telerik.WinControls.UI.RadColorBox();
            this.dataRowColorBox = new Telerik.WinControls.UI.RadColorBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.exportButton = new Telerik.WinControls.UI.RadButton();
            this.exportFormatDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerRowColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupHeaderColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportFormatDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.BackColor = System.Drawing.Color.White;
            this.radGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.radGridView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowColumnChooser = false;
            this.radGridView1.MasterTemplate.AllowColumnReorder = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AllowRowResize = false;
            this.radGridView1.MasterTemplate.ShowGroupedColumns = true;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radGridView1.Size = new System.Drawing.Size(661, 354);
            this.radGridView1.TabIndex = 0;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "TelerikMetro";
            // 
            // headerRowColorBox
            // 
            this.headerRowColorBox.Location = new System.Drawing.Point(154, 402);
            this.headerRowColorBox.Name = "headerRowColorBox";
            this.headerRowColorBox.Size = new System.Drawing.Size(139, 24);
            this.headerRowColorBox.TabIndex = 1;
            this.headerRowColorBox.Text = "radColorBox1";
            this.headerRowColorBox.ThemeName = "TelerikMetro";
            // 
            // groupHeaderColorBox
            // 
            this.groupHeaderColorBox.Location = new System.Drawing.Point(154, 432);
            this.groupHeaderColorBox.Name = "groupHeaderColorBox";
            this.groupHeaderColorBox.Size = new System.Drawing.Size(139, 24);
            this.groupHeaderColorBox.TabIndex = 2;
            this.groupHeaderColorBox.Text = "radColorBox2";
            this.groupHeaderColorBox.ThemeName = "TelerikMetro";
            // 
            // dataRowColorBox
            // 
            this.dataRowColorBox.Location = new System.Drawing.Point(154, 462);
            this.dataRowColorBox.Name = "dataRowColorBox";
            this.dataRowColorBox.Size = new System.Drawing.Size(139, 24);
            this.dataRowColorBox.TabIndex = 3;
            this.dataRowColorBox.Text = "radColorBox3";
            this.dataRowColorBox.ThemeName = "TelerikMetro";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(0, 406);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(106, 16);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "Header background";
            this.radLabel1.ThemeName = "TelerikMetro";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(0, 436);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(138, 16);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "Group header background";
            this.radLabel2.ThemeName = "TelerikMetro";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(0, 466);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(91, 16);
            this.radLabel3.TabIndex = 6;
            this.radLabel3.Text = "Row background";
            this.radLabel3.ThemeName = "TelerikMetro";
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(0, 505);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(661, 24);
            this.exportButton.TabIndex = 7;
            this.exportButton.Text = "Export";
            this.exportButton.ThemeName = "TelerikMetro";
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // exportFormatDropDownList
            // 
            this.exportFormatDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.exportFormatDropDownList.Location = new System.Drawing.Point(154, 374);
            this.exportFormatDropDownList.Name = "exportFormatDropDownList";
            this.exportFormatDropDownList.Size = new System.Drawing.Size(139, 24);
            this.exportFormatDropDownList.TabIndex = 8;
            this.exportFormatDropDownList.Text = "radDropDownList1";
            this.exportFormatDropDownList.ThemeName = "TelerikMetro";
            this.exportFormatDropDownList.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.exportFormatDropDownList_SelectedIndexChanged);
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(0, 376);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(78, 16);
            this.radLabel4.TabIndex = 9;
            this.radLabel4.Text = "Export Format";
            this.radLabel4.ThemeName = "TelerikMetro";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(662, 530);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.exportFormatDropDownList);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dataRowColorBox);
            this.Controls.Add(this.groupHeaderColorBox);
            this.Controls.Add(this.headerRowColorBox);
            this.Controls.Add(this.radGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Spread Grid Integration";
            this.ThemeName = "TelerikMetro";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerRowColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupHeaderColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportFormatDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadColorBox headerRowColorBox;
        private Telerik.WinControls.UI.RadColorBox groupHeaderColorBox;
        private Telerik.WinControls.UI.RadColorBox dataRowColorBox;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadButton exportButton;
        private Telerik.WinControls.UI.RadDropDownList exportFormatDropDownList;
        private Telerik.WinControls.UI.RadLabel radLabel4;
    }
}

