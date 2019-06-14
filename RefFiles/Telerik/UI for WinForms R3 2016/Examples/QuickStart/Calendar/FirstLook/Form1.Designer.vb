Namespace Telerik.Examples.WinControls.Calendar.FirstLook
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim radListDataItem1 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem2 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem3 As New Telerik.WinControls.UI.RadListDataItem()
			Dim radListDataItem4 As New Telerik.WinControls.UI.RadListDataItem()
			Me.radCalendar1 = New Telerik.WinControls.UI.RadCalendar()
			Me.cbTitleFormat = New Telerik.WinControls.UI.RadDropDownList()
			Me.lbRenderingDirection = New Telerik.WinControls.UI.RadLabel()
			Me.radCheckBox3 = New Telerik.WinControls.UI.RadCheckBox()
			Me.chShowSelector = New Telerik.WinControls.UI.RadCheckBox()
			Me.chRowHeader = New Telerik.WinControls.UI.RadCheckBox()
			Me.chColumnHeader = New Telerik.WinControls.UI.RadCheckBox()
			Me.lbTitleFormat = New Telerik.WinControls.UI.RadLabel()
			Me.radCheckBox2 = New Telerik.WinControls.UI.RadCheckBox()
			Me.chNavigationButtons = New Telerik.WinControls.UI.RadCheckBox()
			Me.chFastNavigationButtons = New Telerik.WinControls.UI.RadCheckBox()
			Me.chViewHeader = New Telerik.WinControls.UI.RadCheckBox()
			Me.chAllowFishEye = New Telerik.WinControls.UI.RadCheckBox()
			Me.nudHeaderHeight = New Telerik.WinControls.UI.RadSpinEditor()
			Me.nudHeaderWidth = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radioButton16 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radioButton15 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			CType(Me.radCalendar1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.cbTitleFormat, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lbRenderingDirection, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chShowSelector, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chRowHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chColumnHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lbTitleFormat, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chNavigationButtons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chFastNavigationButtons, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.chAllowFishEye, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.nudHeaderHeight, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.nudHeaderWidth, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radioButton16, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radioButton15, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox2.SuspendLayout()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radGroupBox2)
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Location = New Point(930, 1)
			Me.settingsPanel.Size = New Size(200, 830)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
			' 
			' radCalendar1
			' 
			Me.radCalendar1.AllowColumnHeaderSelectors = True
			Me.radCalendar1.AllowFishEye = True
			Me.radCalendar1.AllowMultipleSelect = True
			Me.radCalendar1.AllowMultipleView = True
			Me.radCalendar1.AllowRowHeaderSelectors = True
			Me.radCalendar1.AllowViewSelector = True
			Me.radCalendar1.BackColor = Color.FromArgb((CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))), (CInt(Fix((CByte(248))))))
			Me.radCalendar1.DayNameFormat = Telerik.WinControls.UI.DayNameFormat.Full
			Me.radCalendar1.FocusedDate = New Date(2013, 3, 11, 0, 0, 0, 0)
			Me.radCalendar1.ForeColor = Color.Black
			Me.radCalendar1.Location = New Point(0, 0)
			Me.radCalendar1.Name = "radCalendar1"
			Me.radCalendar1.ShowFastNavigationButtons = False
			Me.radCalendar1.ShowFooter = True
			Me.radCalendar1.ShowRowHeaders = True
			Me.radCalendar1.ShowViewSelector = True
			Me.radCalendar1.Size = New Size(640, 438)
			Me.radCalendar1.TabIndex = 0
			Me.radCalendar1.Text = "radCalendar1"
            Me.radCalendar1.ZoomFactor = 1.2F
            Me.radCalendar1.CalendarElement.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
			' 
			' cbTitleFormat
			' 
			Me.cbTitleFormat.DropDownSizingMode = (CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode))
			radListDataItem1.Text = "MMMM yyyy"
			radListDataItem1.TextWrap = True
			radListDataItem2.Text = "MM/yy"
			radListDataItem2.TextWrap = True
			radListDataItem3.Text = "MM-yy"
			radListDataItem3.TextWrap = True
			radListDataItem4.Text = "MMM yyyy"
			radListDataItem4.TextWrap = True
			Me.cbTitleFormat.Items.Add(radListDataItem1)
			Me.cbTitleFormat.Items.Add(radListDataItem2)
			Me.cbTitleFormat.Items.Add(radListDataItem3)
			Me.cbTitleFormat.Items.Add(radListDataItem4)
			Me.cbTitleFormat.Location = New Point(14, 267)
			Me.cbTitleFormat.MaxDropDownItems = 4
			Me.cbTitleFormat.Name = "cbTitleFormat"
			' 
			' 
			' 
			Me.cbTitleFormat.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.cbTitleFormat.Size = New Size(93, 20)
			Me.cbTitleFormat.TabIndex = 1
            Me.cbTitleFormat.Text = "MMMM yyyy"
            Me.cbTitleFormat.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			' 
			' lbRenderingDirection
			' 
			Me.lbRenderingDirection.Location = New Point(11, 26)
			Me.lbRenderingDirection.Name = "lbRenderingDirection"
			Me.lbRenderingDirection.Size = New Size(135, 18)
			Me.lbRenderingDirection.TabIndex = 4
			Me.lbRenderingDirection.Text = "Select rendering direction"
			Me.lbRenderingDirection.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' radCheckBox3
			' 
			Me.radCheckBox3.Location = New Point(13, 153)
			Me.radCheckBox3.Name = "radCheckBox3"
			Me.radCheckBox3.Size = New Size(79, 18)
			Me.radCheckBox3.TabIndex = 19
			Me.radCheckBox3.Text = "Right to left"
			' 
			' chShowSelector
			' 
			Me.chShowSelector.Location = New Point(13, 72)
			Me.chShowSelector.Name = "chShowSelector"
			Me.chShowSelector.Size = New Size(90, 18)
			Me.chShowSelector.TabIndex = 8
			Me.chShowSelector.Text = "Show selector"
			Me.chShowSelector.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' chRowHeader
			' 
			Me.chRowHeader.Location = New Point(13, 26)
			Me.chRowHeader.Name = "chRowHeader"
			Me.chRowHeader.Size = New Size(107, 18)
			Me.chRowHeader.TabIndex = 6
			Me.chRowHeader.Text = "Show row header"
			Me.chRowHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' chColumnHeader
			' 
			Me.chColumnHeader.Location = New Point(13, 49)
			Me.chColumnHeader.Name = "chColumnHeader"
			Me.chColumnHeader.Size = New Size(129, 18)
			Me.chColumnHeader.TabIndex = 7
			Me.chColumnHeader.Text = "Show column  header"
			Me.chColumnHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' lbTitleFormat
			' 
			Me.lbTitleFormat.Location = New Point(14, 246)
			Me.lbTitleFormat.Name = "lbTitleFormat"
			Me.lbTitleFormat.Size = New Size(66, 18)
			Me.lbTitleFormat.TabIndex = 6
			Me.lbTitleFormat.Text = "Title format:"
			Me.lbTitleFormat.TextAlignment = ContentAlignment.MiddleCenter
			' 
			' radCheckBox2
			' 
			Me.radCheckBox2.Location = New Point(13, 222)
			Me.radCheckBox2.Name = "radCheckBox2"
			Me.radCheckBox2.Size = New Size(124, 18)
			Me.radCheckBox2.TabIndex = 18
			Me.radCheckBox2.Text = "Show Navigation Bar"
			Me.radCheckBox2.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On

			' 
			' chNavigationButtons
			' 
			Me.chNavigationButtons.Location = New Point(13, 95)
			Me.chNavigationButtons.Name = "chNavigationButtons"
			Me.chNavigationButtons.Size = New Size(145, 18)
			Me.chNavigationButtons.TabIndex = 9
			Me.chNavigationButtons.Text = "Show navigation buttons"
			Me.chNavigationButtons.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' chFastNavigationButtons
			' 
			Me.chFastNavigationButtons.Location = New Point(13, 118)
			Me.chFastNavigationButtons.Name = "chFastNavigationButtons"
			Me.chFastNavigationButtons.Size = New Size(132, 33)
			Me.chFastNavigationButtons.TabIndex = 10
			Me.chFastNavigationButtons.Text = "Show fast navigation " & vbCrLf & "buttons"
			' 
			' chViewHeader
			' 
			Me.chViewHeader.Location = New Point(13, 176)
			Me.chViewHeader.Name = "chViewHeader"
			Me.chViewHeader.Size = New Size(111, 18)
			Me.chViewHeader.TabIndex = 11
			Me.chViewHeader.Text = "Show view header"
			Me.chViewHeader.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			Me.chViewHeader.Visible = False
			' 
			' chAllowFishEye
			' 
			Me.chAllowFishEye.Location = New Point(13, 199)
			Me.chAllowFishEye.Name = "chAllowFishEye"
			Me.chAllowFishEye.Size = New Size(92, 18)
			Me.chAllowFishEye.TabIndex = 12
			Me.chAllowFishEye.Text = "Allow fish eye "
			Me.chAllowFishEye.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' nudHeaderHeight
			' 
			Me.nudHeaderHeight.Location = New Point(14, 159)
			Me.nudHeaderHeight.Minimum = New Decimal(New Integer() { 17, 0, 0, 0})
			Me.nudHeaderHeight.Name = "nudHeaderHeight"
			' 
			' 
			' 
			Me.nudHeaderHeight.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.nudHeaderHeight.Size = New Size(132, 20)
			Me.nudHeaderHeight.TabIndex = 20
			Me.nudHeaderHeight.TabStop = False
			Me.nudHeaderHeight.Value = New Decimal(New Integer() { 17, 0, 0, 0})
			' 
			' nudHeaderWidth
			' 
			Me.nudHeaderWidth.Location = New Point(14, 113)
			Me.nudHeaderWidth.Minimum = New Decimal(New Integer() { 17, 0, 0, 0})
			Me.nudHeaderWidth.Name = "nudHeaderWidth"
			' 
			' 
			' 
			Me.nudHeaderWidth.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
			Me.nudHeaderWidth.Size = New Size(132, 20)
			Me.nudHeaderWidth.TabIndex = 20
			Me.nudHeaderWidth.TabStop = False
			Me.nudHeaderWidth.Value = New Decimal(New Integer() { 17, 0, 0, 0})
			' 
			' radioButton16
			' 
			Me.radioButton16.Location = New Point(14, 47)
			Me.radioButton16.Name = "radioButton16"
			Me.radioButton16.Size = New Size(98, 18)
			Me.radioButton16.TabIndex = 21
			Me.radioButton16.Text = "Render In Rows"
			' 
			' radioButton15
			' 
			Me.radioButton15.Location = New Point(14, 69)
			Me.radioButton15.Name = "radioButton15"
			Me.radioButton15.Size = New Size(115, 18)
			Me.radioButton15.TabIndex = 21
			Me.radioButton15.Text = "Render In Columns"
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupBox1.Anchor = AnchorStyles.Top
			Me.radGroupBox1.Controls.Add(Me.cbTitleFormat)
			Me.radGroupBox1.Controls.Add(Me.chRowHeader)
			Me.radGroupBox1.Controls.Add(Me.chFastNavigationButtons)
			Me.radGroupBox1.Controls.Add(Me.lbTitleFormat)
			Me.radGroupBox1.Controls.Add(Me.chViewHeader)
			Me.radGroupBox1.Controls.Add(Me.chNavigationButtons)
			Me.radGroupBox1.Controls.Add(Me.chColumnHeader)
			Me.radGroupBox1.Controls.Add(Me.radCheckBox3)
			Me.radGroupBox1.Controls.Add(Me.chAllowFishEye)
			Me.radGroupBox1.Controls.Add(Me.chShowSelector)
			Me.radGroupBox1.Controls.Add(Me.radCheckBox2)
			Me.radGroupBox1.FooterText = ""
			Me.radGroupBox1.HeaderText = "Calendar Settings"
			Me.radGroupBox1.Location = New Point(10, 6)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupBox1.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupBox1.Size = New Size(180, 297)
			Me.radGroupBox1.TabIndex = 0
			Me.radGroupBox1.Text = "Calendar Settings"
			' 
			' radGroupBox2
			' 
			Me.radGroupBox2.AccessibleRole = AccessibleRole.Grouping
			Me.radGroupBox2.Anchor = AnchorStyles.Top
			Me.radGroupBox2.Controls.Add(Me.radLabel2)
			Me.radGroupBox2.Controls.Add(Me.radLabel1)
			Me.radGroupBox2.Controls.Add(Me.radioButton16)
			Me.radGroupBox2.Controls.Add(Me.radioButton15)
			Me.radGroupBox2.Controls.Add(Me.lbRenderingDirection)
			Me.radGroupBox2.Controls.Add(Me.nudHeaderHeight)
			Me.radGroupBox2.Controls.Add(Me.nudHeaderWidth)
			Me.radGroupBox2.FooterText = ""
			Me.radGroupBox2.HeaderText = "Rendering Options"
			Me.radGroupBox2.Location = New Point(10, 309)
			Me.radGroupBox2.Name = "radGroupBox2"
			Me.radGroupBox2.Padding = New Padding(10, 20, 10, 10)
			' 
			' 
			' 
			Me.radGroupBox2.RootElement.Padding = New Padding(10, 20, 10, 10)
			Me.radGroupBox2.Size = New Size(180, 192)
			Me.radGroupBox2.TabIndex = 1
			Me.radGroupBox2.Text = "Rendering Options"
			' 
			' radLabel2
			' 
			Me.radLabel2.Location = New Point(14, 138)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New Size(79, 18)
			Me.radLabel2.TabIndex = 2
			Me.radLabel2.Text = "Header Height"
			' 
			' radLabel1
			' 
			Me.radLabel1.Location = New Point(14, 92)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New Size(76, 18)
			Me.radLabel1.TabIndex = 2
			Me.radLabel1.Text = "Header Width"
			' 
			' Form1
			' 
			Me.Controls.Add(Me.radCalendar1)
			Me.Name = "Form1"
			Me.Size = New Size(1308, 506)
			Me.Controls.SetChildIndex(Me.radCalendar1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			CType(Me.radCalendar1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.cbTitleFormat, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lbRenderingDirection, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox3, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chShowSelector, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chRowHeader, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chColumnHeader, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lbTitleFormat, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chNavigationButtons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chFastNavigationButtons, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.chAllowFishEye, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.nudHeaderHeight, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.nudHeaderWidth, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radioButton16, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radioButton15, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			CType(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox2.ResumeLayout(False)
			Me.radGroupBox2.PerformLayout()
			CType(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radCalendar1 As Telerik.WinControls.UI.RadCalendar
		Private cbTitleFormat As Telerik.WinControls.UI.RadDropDownList
		Private lbRenderingDirection As Telerik.WinControls.UI.RadLabel
		Private lbTitleFormat As Telerik.WinControls.UI.RadLabel
		Private chRowHeader As Telerik.WinControls.UI.RadCheckBox
		Private chColumnHeader As Telerik.WinControls.UI.RadCheckBox
		Private chShowSelector As Telerik.WinControls.UI.RadCheckBox
		Private chNavigationButtons As Telerik.WinControls.UI.RadCheckBox
		Private chFastNavigationButtons As Telerik.WinControls.UI.RadCheckBox
		Private chViewHeader As Telerik.WinControls.UI.RadCheckBox
		Private chAllowFishEye As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox2 As Telerik.WinControls.UI.RadCheckBox
		Private radCheckBox3 As Telerik.WinControls.UI.RadCheckBox
		Private nudHeaderHeight As Telerik.WinControls.UI.RadSpinEditor
		Private nudHeaderWidth As Telerik.WinControls.UI.RadSpinEditor
		Private radioButton16 As Telerik.WinControls.UI.RadRadioButton
		Private radioButton15 As Telerik.WinControls.UI.RadRadioButton
		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
	End Class
End Namespace