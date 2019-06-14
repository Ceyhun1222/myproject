namespace SpreadConvertion
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.loadCustomDocumentButton = new Telerik.WinControls.UI.RadButton();
            this.loadSampleDocumentButton = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.fileNameLabel = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.fileExtensionsDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.saveButton = new Telerik.WinControls.UI.RadButton();
            this.telerikMetroTheme1 = new Telerik.WinControls.Themes.TelerikMetroTheme();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadCustomDocumentButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadSampleDocumentButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileNameLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileExtensionsDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::SpreadConvertion.Properties.Resources.CustomDocumentImage;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(262, 200);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "- OR - ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = global::SpreadConvertion.Properties.Resources.SampleDocumentImage;
            this.pictureBox2.Location = new System.Drawing.Point(313, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(262, 200);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // loadCustomDocumentButton
            // 
            this.loadCustomDocumentButton.Location = new System.Drawing.Point(62, 216);
            this.loadCustomDocumentButton.Name = "loadCustomDocumentButton";
            this.loadCustomDocumentButton.Size = new System.Drawing.Size(146, 24);
            this.loadCustomDocumentButton.TabIndex = 3;
            this.loadCustomDocumentButton.Text = "Load Custom Document";
            this.loadCustomDocumentButton.ThemeName = "TelerikMetro";
            this.loadCustomDocumentButton.Click += new System.EventHandler(this.loadCustomDocumentButton_Click);
            // 
            // loadSampleDocumentButton
            // 
            this.loadSampleDocumentButton.Location = new System.Drawing.Point(377, 216);
            this.loadSampleDocumentButton.Name = "loadSampleDocumentButton";
            this.loadSampleDocumentButton.Size = new System.Drawing.Size(146, 24);
            this.loadSampleDocumentButton.TabIndex = 4;
            this.loadSampleDocumentButton.Text = "Load Sample Document";
            this.loadSampleDocumentButton.ThemeName = "TelerikMetro";
            this.loadSampleDocumentButton.Click += new System.EventHandler(this.loadSampleDocumentButton_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(0, 252);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(31, 18);
            this.radLabel1.TabIndex = 5;
            this.radLabel1.Text = "File: ";
            this.radLabel1.ThemeName = "TelerikMetro";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.Location = new System.Drawing.Point(37, 252);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(2, 2);
            this.fileNameLabel.TabIndex = 6;
            this.fileNameLabel.ThemeName = "TelerikMetro";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel2.Location = new System.Drawing.Point(0, 284);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(64, 18);
            this.radLabel2.TabIndex = 7;
            this.radLabel2.Text = "Extension: ";
            this.radLabel2.ThemeName = "TelerikMetro";
            // 
            // fileExtensionsDropDownList
            // 
            this.fileExtensionsDropDownList.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem1.Text = "Xlsx";
            radListDataItem2.Text = "Csv";
            radListDataItem3.Text = "Txt";
            this.fileExtensionsDropDownList.Items.Add(radListDataItem1);
            this.fileExtensionsDropDownList.Items.Add(radListDataItem2);
            this.fileExtensionsDropDownList.Items.Add(radListDataItem3);
            this.fileExtensionsDropDownList.Location = new System.Drawing.Point(70, 284);
            this.fileExtensionsDropDownList.Name = "fileExtensionsDropDownList";
            this.fileExtensionsDropDownList.Size = new System.Drawing.Size(50, 24);
            this.fileExtensionsDropDownList.TabIndex = 8;
            this.fileExtensionsDropDownList.ThemeName = "TelerikMetro";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(0, 320);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(575, 24);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Save";
            this.saveButton.ThemeName = "TelerikMetro";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(575, 345);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.fileExtensionsDropDownList);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.loadSampleDocumentButton);
            this.Controls.Add(this.loadCustomDocumentButton);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Convert Documents";
            this.ThemeName = "TelerikMetro";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadCustomDocumentButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadSampleDocumentButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileNameLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileExtensionsDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Telerik.WinControls.UI.RadButton loadCustomDocumentButton;
        private Telerik.WinControls.UI.RadButton loadSampleDocumentButton;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel fileNameLabel;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadDropDownList fileExtensionsDropDownList;
        private Telerik.WinControls.UI.RadButton saveButton;
        private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
    }
}

