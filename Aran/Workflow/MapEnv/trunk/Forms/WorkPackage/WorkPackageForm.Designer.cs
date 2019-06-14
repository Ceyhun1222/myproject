namespace MapEnv
{
    partial class WorkPackageForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button button1;
            this.ui_clearWpButton = new System.Windows.Forms.Button();
            this.ui_currWorkPackageTB = new System.Windows.Forms.TextBox();
            this.ui_mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ui_showFeaturesButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ui_featuresLB = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_dgv = new System.Windows.Forms.DataGridView();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 16);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(155, 17);
            label1.TabIndex = 0;
            label1.Text = "Current Work Package:";
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button1.Location = new System.Drawing.Point(485, 10);
            button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(30, 27);
            button1.TabIndex = 2;
            button1.Text = "...";
            this.ui_mainToolTip.SetToolTip(button1, "Select Work Package");
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.SelectWorkPackage_Click);
            // 
            // ui_clearWpButton
            // 
            this.ui_clearWpButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ui_clearWpButton.Enabled = false;
            this.ui_clearWpButton.Image = global::MapEnv.Properties.Resources.close_2_24;
            this.ui_clearWpButton.Location = new System.Drawing.Point(521, 10);
            this.ui_clearWpButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_clearWpButton.Name = "ui_clearWpButton";
            this.ui_clearWpButton.Size = new System.Drawing.Size(35, 28);
            this.ui_clearWpButton.TabIndex = 3;
            this.ui_mainToolTip.SetToolTip(this.ui_clearWpButton, "Clear Work Package");
            this.ui_clearWpButton.UseVisualStyleBackColor = true;
            this.ui_clearWpButton.Click += new System.EventHandler(this.ClearWorkPackage_Click);
            // 
            // ui_currWorkPackageTB
            // 
            this.ui_currWorkPackageTB.Location = new System.Drawing.Point(173, 12);
            this.ui_currWorkPackageTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_currWorkPackageTB.Name = "ui_currWorkPackageTB";
            this.ui_currWorkPackageTB.ReadOnly = true;
            this.ui_currWorkPackageTB.Size = new System.Drawing.Size(308, 22);
            this.ui_currWorkPackageTB.TabIndex = 1;
            // 
            // ui_showFeaturesButton
            // 
            this.ui_showFeaturesButton.Location = new System.Drawing.Point(3, 57);
            this.ui_showFeaturesButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_showFeaturesButton.Name = "ui_showFeaturesButton";
            this.ui_showFeaturesButton.Size = new System.Drawing.Size(136, 28);
            this.ui_showFeaturesButton.TabIndex = 4;
            this.ui_showFeaturesButton.Text = "Show Features";
            this.ui_showFeaturesButton.UseVisualStyleBackColor = true;
            this.ui_showFeaturesButton.Click += new System.EventHandler(this.ShowFeatures_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(612, 57);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(185, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "Save Work Package";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveWorkPackage_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(801, 57);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(181, 28);
            this.button3.TabIndex = 6;
            this.button3.Text = "Discard Work Package";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.DiscardWorkPackage_Click);
            // 
            // ui_featuresLB
            // 
            this.ui_featuresLB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_featuresLB.FormattingEnabled = true;
            this.ui_featuresLB.ItemHeight = 16;
            this.ui_featuresLB.Location = new System.Drawing.Point(0, 0);
            this.ui_featuresLB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_featuresLB.Name = "ui_featuresLB";
            this.ui_featuresLB.Size = new System.Drawing.Size(285, 597);
            this.ui_featuresLB.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 92);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_featuresLB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_dgv);
            this.splitContainer1.Size = new System.Drawing.Size(985, 597);
            this.splitContainer1.SplitterDistance = 285;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 8;
            // 
            // ui_dgv
            // 
            this.ui_dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ui_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ui_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ui_dgv.Location = new System.Drawing.Point(0, 0);
            this.ui_dgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ui_dgv.Name = "ui_dgv";
            this.ui_dgv.ReadOnly = true;
            this.ui_dgv.Size = new System.Drawing.Size(695, 597);
            this.ui_dgv.TabIndex = 0;
            // 
            // WorkPackageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 692);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ui_showFeaturesButton);
            this.Controls.Add(this.ui_clearWpButton);
            this.Controls.Add(button1);
            this.Controls.Add(this.ui_currWorkPackageTB);
            this.Controls.Add(label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "WorkPackageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work Packages";
            this.Load += new System.EventHandler(this.WorkPackageForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ui_dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ui_currWorkPackageTB;
        private System.Windows.Forms.ToolTip ui_mainToolTip;
        private System.Windows.Forms.Button ui_clearWpButton;
        private System.Windows.Forms.Button ui_showFeaturesButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox ui_featuresLB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView ui_dgv;
    }
}