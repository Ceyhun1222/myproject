﻿namespace Telerik.Examples.WinControls.ChartView.NullValues
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
            Telerik.WinControls.UI.CartesianArea cartesianArea1 = new Telerik.WinControls.UI.CartesianArea();
            this.tableLayoutPanel1 = new CustomTableLayoutPanel();
            this.tableLayoutPanel2 = new CustomTableLayoutPanel();
            this.leftButton = new Telerik.WinControls.UI.RadButton();
            this.rightButton = new Telerik.WinControls.UI.RadButton();
            this.radListView1 = new Telerik.WinControls.UI.RadListView();
            this.tableLayoutPanel3 = new CustomTableLayoutPanel();
            this.radChartView1 = new Telerik.WinControls.UI.RadChartView();
            this.radPanorama1 = new Telerik.WinControls.UI.RadPanorama();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.radCheckBox1 = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radCheckBox1);
            this.settingsPanel.Controls.Add(this.radDropDownList1);
            this.settingsPanel.Controls.SetChildIndex(this.radDropDownList1, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radCheckBox1, 0);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1072, 971);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tableLayoutPanel2.Controls.Add(this.leftButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rightButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.radListView1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 179F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1066, 179);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // leftButton
            // 
            this.leftButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leftButton.Location = new System.Drawing.Point(3, 3);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(15, 173);
            this.leftButton.TabIndex = 0;
            this.leftButton.Text = "▼";
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rightButton.Location = new System.Drawing.Point(1047, 3);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(16, 173);
            this.rightButton.TabIndex = 1;
            this.rightButton.Text = "▼";
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // radListView1
            // 
            this.radListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radListView1.Location = new System.Drawing.Point(24, 3);
            this.radListView1.Name = "radListView1";
            this.radListView1.Size = new System.Drawing.Size(1017, 173);
            this.radListView1.TabIndex = 2;
            this.radListView1.Text = "radListView1";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.radChartView1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.radPanorama1, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 188);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1066, 780);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // radChartView1
            // 
            this.radChartView1.AreaDesign = cartesianArea1;
            this.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChartView1.Location = new System.Drawing.Point(3, 3);
            this.radChartView1.Name = "radChartView1";
            this.radChartView1.ShowGrid = false;
            this.radChartView1.Size = new System.Drawing.Size(846, 774);
            this.radChartView1.TabIndex = 0;
            this.radChartView1.Text = "radChartView1";
            // 
            // radPanorama1
            // 
            this.radPanorama1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanorama1.Location = new System.Drawing.Point(855, 3);
            this.radPanorama1.Name = "radPanorama1";
            this.radPanorama1.Size = new System.Drawing.Size(208, 774);
            this.radPanorama1.TabIndex = 1;
            this.radPanorama1.Text = "radPanorama1";
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.AllowShowFocusCues = false;
            this.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radDropDownList1.Location = new System.Drawing.Point(10, 65);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(210, 20);
            this.radDropDownList1.TabIndex = 0;
            this.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // radCheckBox1
            // 
            this.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBox1.Location = new System.Drawing.Point(10, 103);
            this.radCheckBox1.Name = "radCheckBox1";
            this.radCheckBox1.Size = new System.Drawing.Size(82, 18);
            this.radCheckBox1.TabIndex = 0;
            this.radCheckBox1.Text = "Show Labels";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1082, 981);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanorama1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Telerik.WinControls.UI.RadButton leftButton;
        private Telerik.WinControls.UI.RadButton rightButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Telerik.WinControls.UI.RadChartView radChartView1;
        private Telerik.WinControls.UI.RadPanorama radPanorama1;
        private Telerik.WinControls.UI.RadListView radListView1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox1;
    }
}