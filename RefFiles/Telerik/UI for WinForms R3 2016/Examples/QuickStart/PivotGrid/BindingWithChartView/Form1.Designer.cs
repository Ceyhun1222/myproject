namespace Telerik.Examples.WinControls.PivotGrid.BindingWithChartView
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
            this.radPivotGrid1 = new Telerik.WinControls.UI.RadPivotGrid();
            this.radChartView1 = new Telerik.WinControls.UI.RadChartView();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.checkBoxDelayUpdates = new Telerik.WinControls.UI.RadCheckBox();
            this.checkBoxSelectionOnly = new Telerik.WinControls.UI.RadCheckBox();
            this.checkBoxColumnSubTotals = new Telerik.WinControls.UI.RadCheckBox();
            this.checkBoxRowSubTotals = new Telerik.WinControls.UI.RadCheckBox();
            this.checkBoxColumnGrandTotals = new Telerik.WinControls.UI.RadCheckBox();
            this.checkBoxRowGrandTotals = new Telerik.WinControls.UI.RadCheckBox();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGroupBox3 = new Telerik.WinControls.UI.RadGroupBox();
            this.radioRows = new Telerik.WinControls.UI.RadRadioButton();
            this.radioColumns = new Telerik.WinControls.UI.RadRadioButton();
            this.radioBarSeries = new Telerik.WinControls.UI.RadRadioButton();
            this.radioLineSeries = new Telerik.WinControls.UI.RadRadioButton();
            this.radioAreaSeries = new Telerik.WinControls.UI.RadRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPivotGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDelayUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxSelectionOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxColumnSubTotals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRowSubTotals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxColumnGrandTotals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRowGrandTotals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).BeginInit();
            this.radGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioBarSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioLineSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioAreaSeries)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsPanel.Controls.Add(this.radGroupBox2);
            this.settingsPanel.Controls.Add(this.radGroupBox1);
            this.settingsPanel.Controls.Add(this.radGroupBox3);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsPanel.Location = new System.Drawing.Point(795, 0);
            this.settingsPanel.Size = new System.Drawing.Size(286, 832);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox3, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox1, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox2, 0);
            // 
            // pivot
            // 
            this.radPivotGrid1.AllowFieldsDragDrop = false;
            this.radPivotGrid1.AllowGroupFiltering = false; 
            this.radPivotGrid1.AllowHorizontalResize = false;
            this.radPivotGrid1.AllowContextMenu = false;
            this.radPivotGrid1.AllowVerticalResize = false;
            this.radPivotGrid1.ColumnHeaderHeight = 30;
            this.radPivotGrid1.ColumnWidth = 100;
            this.radPivotGrid1.ChartDataProvider.IncludeRowGrandTotals = true;
            this.radPivotGrid1.ChartDataProvider.IncludeColumnGrandTotals = true;
            this.radPivotGrid1.DataMember = null;
            this.radPivotGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPivotGrid1.FilterAreaHeight = 30;
            this.radPivotGrid1.Location = new System.Drawing.Point(0, 0);
            this.radPivotGrid1.Name = "pivot";
            this.radPivotGrid1.RowHeight = 25;
            this.radPivotGrid1.Size = new System.Drawing.Size(795, 225);
            this.radPivotGrid1.TabIndex = 1;
            this.radPivotGrid1.Text = "radPivotGrid1";
            // 
            // radChartView1
            // 
            this.radChartView1.LegendTitle = "Legend";
            this.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChartView1.Location = new System.Drawing.Point(0, 366);
            this.radChartView1.Name = "radChartView1";
            this.radChartView1.Size = new System.Drawing.Size(795, 466);
            this.radChartView1.ShowTitle = true;
            this.radChartView1.Title = "Pivot Data Visualization";
            this.radChartView1.ShowLegend = true;
            this.radChartView1.TabIndex = 2;
            this.radChartView1.Text = "radChartView1";
            this.radChartView1.View.Margin = new System.Windows.Forms.Padding(10, 10, 0, 0);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.checkBoxRowGrandTotals);
            this.radGroupBox1.Controls.Add(this.checkBoxColumnGrandTotals);
            this.radGroupBox1.Controls.Add(this.checkBoxRowSubTotals);
            this.radGroupBox1.Controls.Add(this.checkBoxColumnSubTotals);
            this.radGroupBox1.Controls.Add(this.checkBoxSelectionOnly);
            this.radGroupBox1.Controls.Add(this.checkBoxDelayUpdates);
            this.radGroupBox1.FooterImageIndex = -1;
            this.radGroupBox1.FooterImageKey = "";
            this.radGroupBox1.HeaderImageIndex = -1;
            this.radGroupBox1.HeaderImageKey = "";
            this.radGroupBox1.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox1.HeaderText = "Binding options";
            this.radGroupBox1.Location = new System.Drawing.Point(78, 2);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.radGroupBox1.Size = new System.Drawing.Size(185, 175);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "Binding options";
            // 
            // checkBoxDelayUpdates
            // 
            this.checkBoxDelayUpdates.Location = new System.Drawing.Point(11, 22);
            this.checkBoxDelayUpdates.Name = "checkBoxDelayUpdates";
            this.checkBoxDelayUpdates.Size = new System.Drawing.Size(86, 18);
            this.checkBoxDelayUpdates.TabIndex = 0;
            this.checkBoxDelayUpdates.Text = "Delay update";
            // 
            // checkBoxSelectionOnly
            // 
            this.checkBoxSelectionOnly.Location = new System.Drawing.Point(11, 46);
            this.checkBoxSelectionOnly.Name = "checkBoxSelectionOnly";
            this.checkBoxSelectionOnly.Size = new System.Drawing.Size(90, 18);
            this.checkBoxSelectionOnly.TabIndex = 0;
            this.checkBoxSelectionOnly.Text = "Selection only";
            // 
            // checkBoxColumnSubTotals
            // 
            this.checkBoxColumnSubTotals.Location = new System.Drawing.Point(11, 70);
            this.checkBoxColumnSubTotals.Name = "checkBoxColumnSubTotals";
            this.checkBoxColumnSubTotals.Size = new System.Drawing.Size(150, 18);
            this.checkBoxColumnSubTotals.TabIndex = 0;
            this.checkBoxColumnSubTotals.Text = "Include column sub-totals";
            // 
            // checkBoxRowSubTotals
            // 
            this.checkBoxRowSubTotals.Location = new System.Drawing.Point(11, 94);
            this.checkBoxRowSubTotals.Name = "checkBoxRowSubTotals";
            this.checkBoxRowSubTotals.Size = new System.Drawing.Size(132, 18);
            this.checkBoxRowSubTotals.TabIndex = 0;
            this.checkBoxRowSubTotals.Text = "Include row sub-totals";
            // 
            // checkBoxColumnGrandTotals
            // 
            this.checkBoxColumnGrandTotals.Location = new System.Drawing.Point(11, 118);
            this.checkBoxColumnGrandTotals.Name = "checkBoxColumnGrandTotals";
            this.checkBoxColumnGrandTotals.Size = new System.Drawing.Size(160, 18);
            this.checkBoxColumnGrandTotals.TabIndex = 0;
            this.checkBoxColumnGrandTotals.Text = "Include column grand totals";
            // 
            // checkBoxRowGrandTotals
            // 
            this.checkBoxRowGrandTotals.Location = new System.Drawing.Point(11, 142);
            this.checkBoxRowGrandTotals.Name = "checkBoxRowGrandTotals";
            this.checkBoxRowGrandTotals.Size = new System.Drawing.Size(142, 18);
            this.checkBoxRowGrandTotals.TabIndex = 0;
            this.checkBoxRowGrandTotals.Text = "Include row grand totals";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.radioColumns);
            this.radGroupBox2.Controls.Add(this.radioRows);
            this.radGroupBox2.FooterImageIndex = -1;
            this.radGroupBox2.FooterImageKey = "";
            this.radGroupBox2.HeaderImageIndex = -1;
            this.radGroupBox2.HeaderImageKey = "";
            this.radGroupBox2.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox2.HeaderText = "Generate series from";
            this.radGroupBox2.Location = new System.Drawing.Point(78, 184);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.radGroupBox2.Size = new System.Drawing.Size(185, 76);
            this.radGroupBox2.TabIndex = 2;
            this.radGroupBox2.Text = "Generate series from";
            // 
            // radGroupBox3
            // 
            this.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox3.Controls.Add(this.radioAreaSeries);
            this.radGroupBox3.Controls.Add(this.radioLineSeries);
            this.radGroupBox3.Controls.Add(this.radioBarSeries);
            this.radGroupBox3.FooterImageIndex = -1;
            this.radGroupBox3.FooterImageKey = "";
            this.radGroupBox3.HeaderImageIndex = -1;
            this.radGroupBox3.HeaderImageKey = "";
            this.radGroupBox3.HeaderMargin = new System.Windows.Forms.Padding(0);
            this.radGroupBox3.HeaderText = "Generated series type";
            this.radGroupBox3.Location = new System.Drawing.Point(78, 266);
            this.radGroupBox3.Name = "radGroupBox3";
            this.radGroupBox3.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.radGroupBox3.Size = new System.Drawing.Size(185, 96);
            this.radGroupBox3.TabIndex = 2;
            this.radGroupBox3.Text = "Generated series type";
            // 
            // radioRows
            // 
            this.radioRows.Location = new System.Drawing.Point(11, 22);
            this.radioRows.Name = "radioRows";
            this.radioRows.Size = new System.Drawing.Size(110, 18);
            this.radioRows.TabIndex = 0;
            this.radioRows.Text = "Rows";
            // 
            // radioColumns
            // 
            this.radioColumns.Location = new System.Drawing.Point(11, 46);
            this.radioColumns.Name = "radioColumns";
            this.radioColumns.Size = new System.Drawing.Size(110, 18);
            this.radioColumns.TabIndex = 0;
            this.radioColumns.Text = "Columns";
            // 
            // radioBarSeries
            // 
            this.radioBarSeries.Location = new System.Drawing.Point(11, 21);
            this.radioBarSeries.Name = "radioBarSeries";
            this.radioBarSeries.Size = new System.Drawing.Size(110, 18);
            this.radioBarSeries.TabIndex = 0;
            this.radioBarSeries.Text = "Bar";
            // 
            // radioLineSeries
            // 
            this.radioLineSeries.Location = new System.Drawing.Point(11, 45);
            this.radioLineSeries.Name = "radioLineSeries";
            this.radioLineSeries.Size = new System.Drawing.Size(110, 18);
            this.radioLineSeries.TabIndex = 0;
            this.radioLineSeries.Text = "Line";
            // 
            // radioAreaSeries
            // 
            this.radioAreaSeries.Location = new System.Drawing.Point(11, 69);
            this.radioAreaSeries.Name = "radioAreaSeries";
            this.radioAreaSeries.Size = new System.Drawing.Size(110, 18);
            this.radioAreaSeries.TabIndex = 0;
            this.radioAreaSeries.Text = "Area";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radChartView1);
            this.Controls.Add(this.radPivotGrid1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1081, 832);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            this.Controls.SetChildIndex(this.radPivotGrid1, 0);
            this.Controls.SetChildIndex(this.radChartView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPivotGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxDelayUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxSelectionOnly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxColumnSubTotals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRowSubTotals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxColumnGrandTotals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRowGrandTotals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).EndInit();
            this.radGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioBarSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioLineSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioAreaSeries)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPivotGrid radPivotGrid1;
        private Telerik.WinControls.UI.RadChartView radChartView1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadCheckBox checkBoxSelectionOnly;
        private Telerik.WinControls.UI.RadCheckBox checkBoxDelayUpdates;
        private Telerik.WinControls.UI.RadCheckBox checkBoxRowSubTotals;
        private Telerik.WinControls.UI.RadCheckBox checkBoxColumnSubTotals;
        private Telerik.WinControls.UI.RadCheckBox checkBoxColumnGrandTotals;
        private Telerik.WinControls.UI.RadCheckBox checkBoxRowGrandTotals;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox3;
        private Telerik.WinControls.UI.RadRadioButton radioRows;
        private Telerik.WinControls.UI.RadRadioButton radioColumns;
        private Telerik.WinControls.UI.RadRadioButton radioLineSeries;
        private Telerik.WinControls.UI.RadRadioButton radioBarSeries;
        private Telerik.WinControls.UI.RadRadioButton radioAreaSeries;
    }
}