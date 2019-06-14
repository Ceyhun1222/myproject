namespace Aran.PANDA.Vss
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
            if (disposing && (components != null)) {
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
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.GroupBox groupBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ui_pageContainerPanel = new System.Windows.Forms.Panel();
            this.ui_closeButton = new System.Windows.Forms.Button();
            this.ui_nextButton = new System.Windows.Forms.Button();
            this.ui_backButton = new System.Windows.Forms.Button();
            this.ui_reportChB = new System.Windows.Forms.CheckBox();
            panel1 = new System.Windows.Forms.Panel();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.Controls.Add(groupBox1);
            panel1.Location = new System.Drawing.Point(8, 488);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(534, 13);
            panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            groupBox1.Location = new System.Drawing.Point(-3, 1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(541, 18);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // ui_pageContainerPanel
            // 
            this.ui_pageContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_pageContainerPanel.Location = new System.Drawing.Point(8, 8);
            this.ui_pageContainerPanel.Name = "ui_pageContainerPanel";
            this.ui_pageContainerPanel.Size = new System.Drawing.Size(538, 470);
            this.ui_pageContainerPanel.TabIndex = 1;
            // 
            // ui_closeButton
            // 
            this.ui_closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_closeButton.Location = new System.Drawing.Point(465, 509);
            this.ui_closeButton.Name = "ui_closeButton";
            this.ui_closeButton.Size = new System.Drawing.Size(77, 25);
            this.ui_closeButton.TabIndex = 3;
            this.ui_closeButton.Text = "Close";
            this.ui_closeButton.UseVisualStyleBackColor = true;
            this.ui_closeButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ui_nextButton
            // 
            this.ui_nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_nextButton.Location = new System.Drawing.Point(368, 509);
            this.ui_nextButton.Name = "ui_nextButton";
            this.ui_nextButton.Size = new System.Drawing.Size(77, 25);
            this.ui_nextButton.TabIndex = 4;
            this.ui_nextButton.Text = "Next  >";
            this.ui_nextButton.UseVisualStyleBackColor = true;
            this.ui_nextButton.Visible = false;
            this.ui_nextButton.Click += new System.EventHandler(this.Next_Click);
            // 
            // ui_backButton
            // 
            this.ui_backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_backButton.Location = new System.Drawing.Point(287, 509);
            this.ui_backButton.Name = "ui_backButton";
            this.ui_backButton.Size = new System.Drawing.Size(77, 25);
            this.ui_backButton.TabIndex = 5;
            this.ui_backButton.Text = "<  Back";
            this.ui_backButton.UseVisualStyleBackColor = true;
            this.ui_backButton.Visible = false;
            this.ui_backButton.Click += new System.EventHandler(this.Back_Click);
            // 
            // ui_reportChB
            // 
            this.ui_reportChB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ui_reportChB.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_reportChB.Location = new System.Drawing.Point(8, 510);
            this.ui_reportChB.Name = "ui_reportChB";
            this.ui_reportChB.Size = new System.Drawing.Size(83, 24);
            this.ui_reportChB.TabIndex = 6;
            this.ui_reportChB.Text = "Report";
            this.ui_reportChB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ui_reportChB.UseVisualStyleBackColor = true;
            this.ui_reportChB.CheckedChanged += new System.EventHandler(this.Report_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 544);
            this.Controls.Add(this.ui_reportChB);
            this.Controls.Add(this.ui_backButton);
            this.Controls.Add(this.ui_nextButton);
            this.Controls.Add(this.ui_closeButton);
            this.Controls.Add(panel1);
            this.Controls.Add(this.ui_pageContainerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visual Segment Surface";
            this.Load += new System.EventHandler(this.MainForm_Load);
            panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ui_pageContainerPanel;
        private System.Windows.Forms.Button ui_closeButton;
        private System.Windows.Forms.Button ui_nextButton;
        private System.Windows.Forms.Button ui_backButton;
        private System.Windows.Forms.CheckBox ui_reportChB;
    }
}