namespace Aran.Exporter.Gdb
{
    partial class ExporterForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button button2;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.RadioButton radioButton2;
            System.Windows.Forms.RadioButton radioButton1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExporterForm));
            this.ui_kButton = new System.Windows.Forms.Button();
            this.ui_featuresCLB = new System.Windows.Forms.CheckedListBox();
            this.ui_propsTV = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ui_checkAll = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButton2 = new System.Windows.Forms.RadioButton();
            radioButton1 = new System.Windows.Forms.RadioButton();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(3, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(73, 16);
            label1.TabIndex = 0;
            label1.Text = "Features:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(3, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(84, 16);
            label2.TabIndex = 3;
            label2.Text = "Properties:";
            // 
            // button2
            // 
            button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            button2.Location = new System.Drawing.Point(752, 521);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox1
            // 
            groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new System.Drawing.Point(6, 503);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(235, 48);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Exported Database Type:";
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(145, 23);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(67, 17);
            radioButton2.TabIndex = 1;
            radioButton2.Text = "File GDB";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += new System.EventHandler(this.ExpDbTypeFileGDB_CheckedChanged);
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new System.Drawing.Point(23, 23);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(99, 17);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "Personal (MDB)";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += new System.EventHandler(this.ExpDbTypePersonal_CheckedChanged);
            // 
            // ui_kButton
            // 
            this.ui_kButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_kButton.Enabled = false;
            this.ui_kButton.Location = new System.Drawing.Point(671, 521);
            this.ui_kButton.Name = "ui_kButton";
            this.ui_kButton.Size = new System.Drawing.Size(75, 23);
            this.ui_kButton.TabIndex = 5;
            this.ui_kButton.Text = "OK";
            this.ui_kButton.UseVisualStyleBackColor = true;
            this.ui_kButton.Click += new System.EventHandler(this.OK_Click);
            // 
            // ui_featuresCLB
            // 
            this.ui_featuresCLB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_featuresCLB.FormattingEnabled = true;
            this.ui_featuresCLB.Location = new System.Drawing.Point(2, 33);
            this.ui_featuresCLB.Name = "ui_featuresCLB";
            this.ui_featuresCLB.Size = new System.Drawing.Size(293, 454);
            this.ui_featuresCLB.TabIndex = 1;
            this.ui_featuresCLB.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FeaturesCLB_ItemCheck);
            this.ui_featuresCLB.SelectedIndexChanged += new System.EventHandler(this.Features_SelectedIndexChanged);
            // 
            // ui_propsTV
            // 
            this.ui_propsTV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_propsTV.CheckBoxes = true;
            this.ui_propsTV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ui_propsTV.ItemHeight = 22;
            this.ui_propsTV.Location = new System.Drawing.Point(0, 36);
            this.ui_propsTV.Name = "ui_propsTV";
            this.ui_propsTV.Size = new System.Drawing.Size(535, 458);
            this.ui_propsTV.TabIndex = 2;
            this.ui_propsTV.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.PropsTreeView_BeforeCheck);
            this.ui_propsTV.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.PropsTreeView_AfterCheck);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ui_featuresCLB);
            this.splitContainer1.Panel1.Controls.Add(label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ui_checkAll);
            this.splitContainer1.Panel2.Controls.Add(label2);
            this.splitContainer1.Panel2.Controls.Add(this.ui_propsTV);
            this.splitContainer1.Size = new System.Drawing.Size(837, 494);
            this.splitContainer1.SplitterDistance = 298;
            this.splitContainer1.TabIndex = 7;
            // 
            // ui_checkAll
            // 
            this.ui_checkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ui_checkAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.ui_checkAll.AutoSize = true;
            this.ui_checkAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ui_checkAll.Location = new System.Drawing.Point(464, 7);
            this.ui_checkAll.Name = "ui_checkAll";
            this.ui_checkAll.Size = new System.Drawing.Size(62, 23);
            this.ui_checkAll.TabIndex = 9;
            this.ui_checkAll.Text = "Check All";
            this.ui_checkAll.UseVisualStyleBackColor = true;
            this.ui_checkAll.CheckedChanged += new System.EventHandler(this.CheckAll_CheckedChanged);
            // 
            // ExporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 556);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(button2);
            this.Controls.Add(this.ui_kButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExporterForm";
            this.Text = "Exporter";
            this.Load += new System.EventHandler(this.ExporterForm_Load);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox ui_featuresCLB;
        private System.Windows.Forms.TreeView ui_propsTV;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button ui_kButton;
        private System.Windows.Forms.CheckBox ui_checkAll;


    }
}