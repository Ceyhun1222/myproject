namespace Telerik.Examples.WinControls.Calendar.FirstLook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.radCalendar1 = new Telerik.WinControls.UI.RadCalendar();
            this.cbTitleFormat = new Telerik.WinControls.UI.RadDropDownList();
            this.lbRenderingDirection = new Telerik.WinControls.UI.RadLabel();
            this.radCheckBox3 = new Telerik.WinControls.UI.RadCheckBox();
            this.chShowSelector = new Telerik.WinControls.UI.RadCheckBox();
            this.chRowHeader = new Telerik.WinControls.UI.RadCheckBox();
            this.chColumnHeader = new Telerik.WinControls.UI.RadCheckBox();
            this.lbTitleFormat = new Telerik.WinControls.UI.RadLabel();
            this.radCheckBox2 = new Telerik.WinControls.UI.RadCheckBox();
            this.chNavigationButtons = new Telerik.WinControls.UI.RadCheckBox();
            this.chFastNavigationButtons = new Telerik.WinControls.UI.RadCheckBox();
            this.chViewHeader = new Telerik.WinControls.UI.RadCheckBox();
            this.chAllowFishEye = new Telerik.WinControls.UI.RadCheckBox();
            this.nudHeaderHeight = new Telerik.WinControls.UI.RadSpinEditor();
            this.nudHeaderWidth = new Telerik.WinControls.UI.RadSpinEditor();
            this.radioButton16 = new Telerik.WinControls.UI.RadRadioButton();
            this.radioButton15 = new Telerik.WinControls.UI.RadRadioButton();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTitleFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbRenderingDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chShowSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chRowHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chColumnHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbTitleFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chNavigationButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFastNavigationButtons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chViewHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chAllowFishEye)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButton16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButton15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.radGroupBox2);
            this.settingsPanel.Controls.Add(this.radGroupBox1);
            this.settingsPanel.Location = new System.Drawing.Point(930, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 830);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox1, 0);
            this.settingsPanel.Controls.SetChildIndex(this.radGroupBox2, 0);
            // 
            // radCalendar1
            // 
            this.radCalendar1.AllowColumnHeaderSelectors = true;
            this.radCalendar1.AllowFishEye = true;
            this.radCalendar1.AllowMultipleSelect = true;
            this.radCalendar1.AllowMultipleView = true;
            this.radCalendar1.AllowRowHeaderSelectors = true;
            this.radCalendar1.AllowViewSelector = true;
            this.radCalendar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.radCalendar1.DayNameFormat = Telerik.WinControls.UI.DayNameFormat.Full;
            this.radCalendar1.FocusedDate = new System.DateTime(2013, 3, 11, 0, 0, 0, 0);
            this.radCalendar1.ForeColor = System.Drawing.Color.Black;
            this.radCalendar1.Location = new System.Drawing.Point(0, 0);
            this.radCalendar1.Name = "radCalendar1";
            this.radCalendar1.ShowFastNavigationButtons = false;
            this.radCalendar1.ShowFooter = true;
            this.radCalendar1.ShowRowHeaders = true;
            this.radCalendar1.ShowViewSelector = true;
            this.radCalendar1.Size = new System.Drawing.Size(640, 438);
            this.radCalendar1.TabIndex = 0;
            this.radCalendar1.Text = "radCalendar1";
            this.radCalendar1.ZoomFactor = 1.2F;
            this.radCalendar1.CalendarElement.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // cbTitleFormat
            // 
            this.cbTitleFormat.DropDownSizingMode = ((Telerik.WinControls.UI.SizingMode)((Telerik.WinControls.UI.SizingMode.RightBottom | Telerik.WinControls.UI.SizingMode.UpDown)));
            radListDataItem1.Text = "MMMM yyyy";
            radListDataItem1.TextWrap = true;
            radListDataItem2.Text = "MM/yy";
            radListDataItem2.TextWrap = true;
            radListDataItem3.Text = "MM-yy";
            radListDataItem3.TextWrap = true;
            radListDataItem4.Text = "MMM yyyy";
            radListDataItem4.TextWrap = true;
            this.cbTitleFormat.Items.Add(radListDataItem1);
            this.cbTitleFormat.Items.Add(radListDataItem2);
            this.cbTitleFormat.Items.Add(radListDataItem3);
            this.cbTitleFormat.Items.Add(radListDataItem4);
            this.cbTitleFormat.Location = new System.Drawing.Point(14, 267);
            this.cbTitleFormat.MaxDropDownItems = 4;
            this.cbTitleFormat.Name = "cbTitleFormat";
            // 
            // 
            // 
            this.cbTitleFormat.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cbTitleFormat.Size = new System.Drawing.Size(93, 20);
            this.cbTitleFormat.TabIndex = 1;
            this.cbTitleFormat.Text = "MMMM yyyy";
            this.cbTitleFormat.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            // 
            // lbRenderingDirection
            // 
            this.lbRenderingDirection.Location = new System.Drawing.Point(11, 26);
            this.lbRenderingDirection.Name = "lbRenderingDirection";
            this.lbRenderingDirection.Size = new System.Drawing.Size(135, 18);
            this.lbRenderingDirection.TabIndex = 4;
            this.lbRenderingDirection.Text = "Select rendering direction";
            this.lbRenderingDirection.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radCheckBox3
            // 
            this.radCheckBox3.Location = new System.Drawing.Point(13, 153);
            this.radCheckBox3.Name = "radCheckBox3";
            this.radCheckBox3.Size = new System.Drawing.Size(79, 18);
            this.radCheckBox3.TabIndex = 19;
            this.radCheckBox3.Text = "Right to left";
            // 
            // chShowSelector
            // 
            this.chShowSelector.Location = new System.Drawing.Point(13, 72);
            this.chShowSelector.Name = "chShowSelector";
            this.chShowSelector.Size = new System.Drawing.Size(90, 18);
            this.chShowSelector.TabIndex = 8;
            this.chShowSelector.Text = "Show selector";
            this.chShowSelector.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // chRowHeader
            // 
            this.chRowHeader.Location = new System.Drawing.Point(13, 26);
            this.chRowHeader.Name = "chRowHeader";
            this.chRowHeader.Size = new System.Drawing.Size(107, 18);
            this.chRowHeader.TabIndex = 6;
            this.chRowHeader.Text = "Show row header";
            this.chRowHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // chColumnHeader
            // 
            this.chColumnHeader.Location = new System.Drawing.Point(13, 49);
            this.chColumnHeader.Name = "chColumnHeader";
            this.chColumnHeader.Size = new System.Drawing.Size(129, 18);
            this.chColumnHeader.TabIndex = 7;
            this.chColumnHeader.Text = "Show column  header";
            this.chColumnHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // lbTitleFormat
            // 
            this.lbTitleFormat.Location = new System.Drawing.Point(14, 246);
            this.lbTitleFormat.Name = "lbTitleFormat";
            this.lbTitleFormat.Size = new System.Drawing.Size(66, 18);
            this.lbTitleFormat.TabIndex = 6;
            this.lbTitleFormat.Text = "Title format:";
            this.lbTitleFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radCheckBox2
            // 
            this.radCheckBox2.Location = new System.Drawing.Point(13, 222);
            this.radCheckBox2.Name = "radCheckBox2";
            this.radCheckBox2.Size = new System.Drawing.Size(124, 18);
            this.radCheckBox2.TabIndex = 18;
            this.radCheckBox2.Text = "Show Navigation Bar";
            this.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;

            // 
            // chNavigationButtons
            // 
            this.chNavigationButtons.Location = new System.Drawing.Point(13, 95);
            this.chNavigationButtons.Name = "chNavigationButtons";
            this.chNavigationButtons.Size = new System.Drawing.Size(145, 18);
            this.chNavigationButtons.TabIndex = 9;
            this.chNavigationButtons.Text = "Show navigation buttons";
            this.chNavigationButtons.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // chFastNavigationButtons
            // 
            this.chFastNavigationButtons.Location = new System.Drawing.Point(13, 118);
            this.chFastNavigationButtons.Name = "chFastNavigationButtons";
            this.chFastNavigationButtons.Size = new System.Drawing.Size(132, 33);
            this.chFastNavigationButtons.TabIndex = 10;
            this.chFastNavigationButtons.Text = "Show fast navigation \r\nbuttons";
            // 
            // chViewHeader
            // 
            this.chViewHeader.Location = new System.Drawing.Point(13, 176);
            this.chViewHeader.Name = "chViewHeader";
            this.chViewHeader.Size = new System.Drawing.Size(111, 18);
            this.chViewHeader.TabIndex = 11;
            this.chViewHeader.Text = "Show view header";
            this.chViewHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.chViewHeader.Visible = false;
            // 
            // chAllowFishEye
            // 
            this.chAllowFishEye.Location = new System.Drawing.Point(13, 199);
            this.chAllowFishEye.Name = "chAllowFishEye";
            this.chAllowFishEye.Size = new System.Drawing.Size(92, 18);
            this.chAllowFishEye.TabIndex = 12;
            this.chAllowFishEye.Text = "Allow fish eye ";
            this.chAllowFishEye.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // nudHeaderHeight
            // 
            this.nudHeaderHeight.Location = new System.Drawing.Point(14, 159);
            this.nudHeaderHeight.Minimum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudHeaderHeight.Name = "nudHeaderHeight";
            // 
            // 
            // 
            this.nudHeaderHeight.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.nudHeaderHeight.Size = new System.Drawing.Size(132, 20);
            this.nudHeaderHeight.TabIndex = 20;
            this.nudHeaderHeight.TabStop = false;
            this.nudHeaderHeight.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // nudHeaderWidth
            // 
            this.nudHeaderWidth.Location = new System.Drawing.Point(14, 113);
            this.nudHeaderWidth.Minimum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.nudHeaderWidth.Name = "nudHeaderWidth";
            // 
            // 
            // 
            this.nudHeaderWidth.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.nudHeaderWidth.Size = new System.Drawing.Size(132, 20);
            this.nudHeaderWidth.TabIndex = 20;
            this.nudHeaderWidth.TabStop = false;
            this.nudHeaderWidth.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // radioButton16
            // 
            this.radioButton16.Location = new System.Drawing.Point(14, 47);
            this.radioButton16.Name = "radioButton16";
            this.radioButton16.Size = new System.Drawing.Size(98, 18);
            this.radioButton16.TabIndex = 21;
            this.radioButton16.Text = "Render In Rows";
            // 
            // radioButton15
            // 
            this.radioButton15.Location = new System.Drawing.Point(14, 69);
            this.radioButton15.Name = "radioButton15";
            this.radioButton15.Size = new System.Drawing.Size(115, 18);
            this.radioButton15.TabIndex = 21;
            this.radioButton15.Text = "Render In Columns";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupBox1.Controls.Add(this.cbTitleFormat);
            this.radGroupBox1.Controls.Add(this.chRowHeader);
            this.radGroupBox1.Controls.Add(this.chFastNavigationButtons);
            this.radGroupBox1.Controls.Add(this.lbTitleFormat);
            this.radGroupBox1.Controls.Add(this.chViewHeader);
            this.radGroupBox1.Controls.Add(this.chNavigationButtons);
            this.radGroupBox1.Controls.Add(this.chColumnHeader);
            this.radGroupBox1.Controls.Add(this.radCheckBox3);
            this.radGroupBox1.Controls.Add(this.chAllowFishEye);
            this.radGroupBox1.Controls.Add(this.chShowSelector);
            this.radGroupBox1.Controls.Add(this.radCheckBox2);
            this.radGroupBox1.FooterText = "";
            this.radGroupBox1.HeaderText = "Calendar Settings";
            this.radGroupBox1.Location = new System.Drawing.Point(10, 6);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupBox1.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupBox1.Size = new System.Drawing.Size(180, 297);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.Text = "Calendar Settings";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.radGroupBox2.Controls.Add(this.radLabel2);
            this.radGroupBox2.Controls.Add(this.radLabel1);
            this.radGroupBox2.Controls.Add(this.radioButton16);
            this.radGroupBox2.Controls.Add(this.radioButton15);
            this.radGroupBox2.Controls.Add(this.lbRenderingDirection);
            this.radGroupBox2.Controls.Add(this.nudHeaderHeight);
            this.radGroupBox2.Controls.Add(this.nudHeaderWidth);
            this.radGroupBox2.FooterText = "";
            this.radGroupBox2.HeaderText = "Rendering Options";
            this.radGroupBox2.Location = new System.Drawing.Point(10, 309);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            // 
            // 
            // 
            this.radGroupBox2.RootElement.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.radGroupBox2.Size = new System.Drawing.Size(180, 192);
            this.radGroupBox2.TabIndex = 1;
            this.radGroupBox2.Text = "Rendering Options";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(14, 138);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(79, 18);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "Header Height";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(14, 92);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(76, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Header Width";
            // 
            // Form1
            // 
            this.Controls.Add(this.radCalendar1);
            this.Name = "Form1";
            this.Size = new System.Drawing.Size(1308, 506);
            this.Controls.SetChildIndex(this.radCalendar1, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTitleFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbRenderingDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chShowSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chRowHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chColumnHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbTitleFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chNavigationButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFastNavigationButtons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chViewHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chAllowFishEye)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeaderWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButton16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioButton15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.UI.RadCalendar radCalendar1;
		private Telerik.WinControls.UI.RadDropDownList cbTitleFormat;
        private Telerik.WinControls.UI.RadLabel lbRenderingDirection;
		private Telerik.WinControls.UI.RadLabel lbTitleFormat;
		private Telerik.WinControls.UI.RadCheckBox chRowHeader;
		private Telerik.WinControls.UI.RadCheckBox chColumnHeader;
		private Telerik.WinControls.UI.RadCheckBox chShowSelector;
		private Telerik.WinControls.UI.RadCheckBox chNavigationButtons;
		private Telerik.WinControls.UI.RadCheckBox chFastNavigationButtons;
		private Telerik.WinControls.UI.RadCheckBox chViewHeader;
        private Telerik.WinControls.UI.RadCheckBox chAllowFishEye;
		private Telerik.WinControls.UI.RadCheckBox radCheckBox2;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox3;
        private Telerik.WinControls.UI.RadSpinEditor nudHeaderHeight;
        private Telerik.WinControls.UI.RadSpinEditor nudHeaderWidth;
        private Telerik.WinControls.UI.RadRadioButton radioButton16;
        private Telerik.WinControls.UI.RadRadioButton radioButton15;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
	}
}