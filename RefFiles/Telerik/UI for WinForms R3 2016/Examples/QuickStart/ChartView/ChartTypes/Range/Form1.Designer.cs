namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Range
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Telerik.WinControls.UI.CartesianArea cartesianArea2 = new Telerik.WinControls.UI.CartesianArea();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.radChartView1 = new Telerik.WinControls.UI.RadChartView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radDropDownListChartType = new Telerik.WinControls.UI.RadDropDownList();
            this.orientationCheckBox = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBoxSpline = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListChartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orientationCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxSpline)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radCheckBoxSpline);
            this.settingsPanel.Controls.Add(this.orientationCheckBox);
            this.settingsPanel.Controls.Add(this.radDropDownListChartType);
            this.settingsPanel.Controls.Add(this.radLabel2);
            this.settingsPanel.Controls.Add(this.radLabel1);
            this.settingsPanel.Location = new System.Drawing.Point(834, 1);
            this.settingsPanel.Size = new System.Drawing.Size(812, 883);
            this.settingsPanel.Controls.SetChildIndex(this.radLabel1, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radLabel2, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radDropDownListChartType, 0);
            this.settingsPanel.Controls.SetChildIndex(this.orientationCheckBox, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radCheckBoxSpline, 0);
            // 
            // radChartView1
            // 
            this.radChartView1.AreaDesign = cartesianArea2;
            this.radChartView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radChartView1.Location = new System.Drawing.Point(0, 0);
            this.radChartView1.MinimumSize = new System.Drawing.Size(550, 320);
            this.radChartView1.Name = "radChartView1";
            // 
            // 
            // 
            this.radChartView1.RootElement.MinSize = new System.Drawing.Size(550, 320);
            this.radChartView1.ShowGrid = false;
            this.radChartView1.ShowToolTip = true;
            this.radChartView1.Size = new System.Drawing.Size(1871, 1086);
            this.radChartView1.TabIndex = 1;
            this.radChartView1.Text = "radChartView1";
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel1.Location = new System.Drawing.Point(10, 45);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(62, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Series type:";
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radLabel2.Location = new System.Drawing.Point(10, 116);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(106, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "Change Orientation:";
            // 
            // radDropDownListChartType
            // 
            this.radDropDownListChartType.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radDropDownListChartType.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem3.Text = "Range";
            radListDataItem4.Text = "RangeBar";
            this.radDropDownListChartType.Items.Add(radListDataItem3);
            this.radDropDownListChartType.Items.Add(radListDataItem4);
            this.radDropDownListChartType.Location = new System.Drawing.Point(10, 66);
            this.radDropDownListChartType.Name = "radDropDownListChartType";
            this.radDropDownListChartType.Size = new System.Drawing.Size(792, 20);
            this.radDropDownListChartType.TabIndex = 2;
            // 
            // orientationCheckBox
            // 
            this.orientationCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.orientationCheckBox.Location = new System.Drawing.Point(10, 137);
            this.orientationCheckBox.Name = "orientationCheckBox";
            this.orientationCheckBox.Size = new System.Drawing.Size(72, 18);
            this.orientationCheckBox.TabIndex = 4;
            this.orientationCheckBox.Text = "Horizontal";
            // 
            // radCheckBoxSpline
            // 
            this.radCheckBoxSpline.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radCheckBoxSpline.Enabled = false;
            this.radCheckBoxSpline.Location = new System.Drawing.Point(10, 92);
            this.radCheckBoxSpline.Name = "radCheckBoxSpline";
            this.radCheckBoxSpline.Size = new System.Drawing.Size(51, 18);
            this.radCheckBoxSpline.TabIndex = 5;
            this.radCheckBoxSpline.Text = "Spline";
            // 
            // Form1
            // 
            this.AutoScrollMinSize = new System.Drawing.Size(550, 320);
            this.Controls.Add(this.radChartView1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1881, 1096);
            this.Controls.SetChildIndex(this.themePanel, 0);
            this.Controls.SetChildIndex(this.radChartView1, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.themePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radChartView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownListChartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orientationCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBoxSpline)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private Telerik.WinControls.UI.RadChartView radChartView1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadCheckBox orientationCheckBox;
        private Telerik.WinControls.UI.RadDropDownList radDropDownListChartType;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadCheckBox radCheckBoxSpline;
	}
}
