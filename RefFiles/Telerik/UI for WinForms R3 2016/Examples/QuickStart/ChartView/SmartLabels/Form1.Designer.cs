namespace Telerik.Examples.WinControls.ChartView.SmartLabels
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
            Telerik.WinControls.UI.CartesianArea cartesianArea2 = new Telerik.WinControls.UI.CartesianArea();
            this.radCheckBox1 = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox2 = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox3 = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox4 = new Telerik.WinControls.UI.RadCheckBox();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radSpinEditor1 = new Telerik.WinControls.UI.RadSpinEditor();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.customTableLayoutPanel1 = new Telerik.Examples.WinControls.ChartView.FirstLook.CustomTableLayoutPanel();
            this.radChartView1 = new Telerik.WinControls.UI.RadChartView();
            this.radChartView2 = new Telerik.WinControls.UI.RadChartView();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            this.customTableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView2)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupBox1);
            this.settingsPanel.Location = new System.Drawing.Point(1380, 3);
            this.settingsPanel.Size = new System.Drawing.Size(171, 493);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox1, 0);
            // 
            // radCheckBox1
            // 
            this.radCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBox1.Location = new System.Drawing.Point(5, 48);
            this.radCheckBox1.Name = "radCheckBox1";
            this.radCheckBox1.Size = new System.Drawing.Size(126, 18);
            this.radCheckBox1.TabIndex = 1;
            this.radCheckBox1.Text = "Draw connecting line";
            this.radCheckBox1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // radCheckBox2
            // 
            this.radCheckBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBox2.Location = new System.Drawing.Point(5, 72);
            this.radCheckBox2.Name = "radCheckBox2";
            this.radCheckBox2.Size = new System.Drawing.Size(115, 18);
            this.radCheckBox2.TabIndex = 1;
            this.radCheckBox2.Text = "Draw labels border";
            this.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // radCheckBox3
            // 
            this.radCheckBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBox3.Location = new System.Drawing.Point(5, 120);
            this.radCheckBox3.Name = "radCheckBox3";
            this.radCheckBox3.Size = new System.Drawing.Size(124, 18);
            this.radCheckBox3.TabIndex = 1;
            this.radCheckBox3.Text = "Sync labels line color";
            this.radCheckBox3.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // radCheckBox4
            // 
            this.radCheckBox4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBox4.Location = new System.Drawing.Point(5, 96);
            this.radCheckBox4.Name = "radCheckBox4";
            this.radCheckBox4.Size = new System.Drawing.Size(140, 18);
            this.radCheckBox4.TabIndex = 1;
            this.radCheckBox4.Text = "Sync labels border color";
            this.radCheckBox4.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.Controls.Add(this.radSpinEditor1);
            this.radGroupBox1.Controls.Add(this.radDropDownList1);
            this.radGroupBox1.Controls.Add(this.radCheckBox4);
            this.radGroupBox1.Controls.Add(this.radCheckBox3);
            this.radGroupBox1.Controls.Add(this.radCheckBox2);
            this.radGroupBox1.Controls.Add(this.radCheckBox1);
            this.radGroupBox1.HeaderText = "Settings";
            this.radGroupBox1.Location = new System.Drawing.Point(10, 3);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(151, 201);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "Settings";
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel1.Location = new System.Drawing.Point(5, 145);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(88, 18);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "Distance to label";
            // 
            // radSpinEditor1
            // 
            this.radSpinEditor1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radSpinEditor1.Location = new System.Drawing.Point(5, 169);
            this.radSpinEditor1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.radSpinEditor1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.radSpinEditor1.Name = "radSpinEditor1";
            this.radSpinEditor1.Size = new System.Drawing.Size(141, 20);
            this.radSpinEditor1.TabIndex = 3;
            this.radSpinEditor1.TabStop = false;
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.radDropDownList1.Location = new System.Drawing.Point(5, 22);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(141, 20);
            this.radDropDownList1.TabIndex = 2;
            this.radDropDownList1.Text = "radDropDownList1";
            // 
            // customTableLayoutPanel1
            // 
            this.customTableLayoutPanel1.ColumnCount = 2;
            this.customTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.customTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.customTableLayoutPanel1.Controls.Add(this.radChartView1, 0, 0);
            this.customTableLayoutPanel1.Controls.Add(this.radChartView2, 1, 0);
            this.customTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.customTableLayoutPanel1.MinimumSize = new System.Drawing.Size(500, 350);
            this.customTableLayoutPanel1.Name = "customTableLayoutPanel1";
            this.customTableLayoutPanel1.RowCount = 1;
            this.customTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.customTableLayoutPanel1.Size = new System.Drawing.Size(1158, 695);
            this.customTableLayoutPanel1.TabIndex = 1;
            // 
            // radChartView1
            // 
            this.radChartView1.AreaDesign = cartesianArea1;
            this.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChartView1.Location = new System.Drawing.Point(3, 3);
            this.radChartView1.Name = "radChartView1";
            this.radChartView1.ShowGrid = false;
            this.radChartView1.Size = new System.Drawing.Size(573, 689);
            this.radChartView1.TabIndex = 0;
            this.radChartView1.Text = "radChartView1";
            this.radChartView1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            // 
            // radChartView2
            // 
            this.radChartView2.AreaDesign = cartesianArea2;
            this.radChartView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChartView2.Location = new System.Drawing.Point(582, 3);
            this.radChartView2.Name = "radChartView2";
            this.radChartView2.ShowGrid = false;
            this.radChartView2.Size = new System.Drawing.Size(573, 689);
            this.radChartView2.TabIndex = 0;
            this.radChartView2.Text = "radChartView2";
            this.radChartView2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMinSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.customTableLayoutPanel1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1168, 705);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.customTableLayoutPanel1, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            this.customTableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FirstLook.CustomTableLayoutPanel customTableLayoutPanel1;
        private Telerik.WinControls.UI.RadChartView radChartView1;
        private Telerik.WinControls.UI.RadChartView radChartView2;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox2;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox3;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox4;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadSpinEditor radSpinEditor1;

    }
}