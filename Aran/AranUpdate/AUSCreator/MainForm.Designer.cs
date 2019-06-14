namespace AUSCreator
{
    partial class MainForm
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_versionNameTB = new System.Windows.Forms.TextBox();
            this.ui_releaseDatePiker = new System.Windows.Forms.DateTimePicker();
            this.ui_changesRTB = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ui_bin7zFileTB = new System.Windows.Forms.TextBox();
            this.ui_sel7zBinFileButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 11);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(101, 17);
            label1.TabIndex = 0;
            label1.Text = "Version Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(255, 11);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(98, 17);
            label2.TabIndex = 2;
            label2.Text = "Release Date:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(17, 76);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(117, 17);
            label3.TabIndex = 4;
            label3.Text = "Changes ( RTF ):";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(499, 11);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(77, 17);
            label4.TabIndex = 7;
            label4.Text = "7z Bin File:";
            // 
            // ui_versionNameTB
            // 
            this.ui_versionNameTB.Location = new System.Drawing.Point(20, 31);
            this.ui_versionNameTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_versionNameTB.Name = "ui_versionNameTB";
            this.ui_versionNameTB.Size = new System.Drawing.Size(211, 22);
            this.ui_versionNameTB.TabIndex = 1;
            // 
            // ui_releaseDatePiker
            // 
            this.ui_releaseDatePiker.CustomFormat = "yyyy - MM - dd";
            this.ui_releaseDatePiker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ui_releaseDatePiker.Location = new System.Drawing.Point(259, 31);
            this.ui_releaseDatePiker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_releaseDatePiker.Name = "ui_releaseDatePiker";
            this.ui_releaseDatePiker.Size = new System.Drawing.Size(211, 22);
            this.ui_releaseDatePiker.TabIndex = 3;
            // 
            // ui_changesRTB
            // 
            this.ui_changesRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ui_changesRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_changesRTB.Location = new System.Drawing.Point(0, 0);
            this.ui_changesRTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_changesRTB.Name = "ui_changesRTB";
            this.ui_changesRTB.Size = new System.Drawing.Size(961, 496);
            this.ui_changesRTB.TabIndex = 5;
            this.ui_changesRTB.Text = "";
            this.ui_changesRTB.TextChanged += new System.EventHandler(this.ChangesRTB_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ui_changesRTB);
            this.panel1.Location = new System.Drawing.Point(20, 96);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 498);
            this.panel1.TabIndex = 6;
            // 
            // ui_bin7zFileTB
            // 
            this.ui_bin7zFileTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_bin7zFileTB.Location = new System.Drawing.Point(503, 31);
            this.ui_bin7zFileTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_bin7zFileTB.Name = "ui_bin7zFileTB";
            this.ui_bin7zFileTB.Size = new System.Drawing.Size(437, 22);
            this.ui_bin7zFileTB.TabIndex = 8;
            // 
            // ui_sel7zBinFileButton
            // 
            this.ui_sel7zBinFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_sel7zBinFileButton.AutoSize = true;
            this.ui_sel7zBinFileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_sel7zBinFileButton.Location = new System.Drawing.Point(954, 28);
            this.ui_sel7zBinFileButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_sel7zBinFileButton.Name = "ui_sel7zBinFileButton";
            this.ui_sel7zBinFileButton.Size = new System.Drawing.Size(30, 27);
            this.ui_sel7zBinFileButton.TabIndex = 9;
            this.ui_sel7zBinFileButton.Text = "...";
            this.ui_sel7zBinFileButton.UseVisualStyleBackColor = true;
            this.ui_sel7zBinFileButton.Click += new System.EventHandler(this.Select7zBinFile_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(20, 613);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 11;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Create_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(884, 613);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 12;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Close_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(128, 613);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 13;
            this.button3.Text = "Open";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Open_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 661);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ui_sel7zBinFileButton);
            this.Controls.Add(this.ui_bin7zFileTB);
            this.Controls.Add(label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(label3);
            this.Controls.Add(this.ui_releaseDatePiker);
            this.Controls.Add(label2);
            this.Controls.Add(this.ui_versionNameTB);
            this.Controls.Add(label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Aran Update Source Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_versionNameTB;
        private System.Windows.Forms.DateTimePicker ui_releaseDatePiker;
        private System.Windows.Forms.RichTextBox ui_changesRTB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox ui_bin7zFileTB;
        private System.Windows.Forms.Button ui_sel7zBinFileButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

