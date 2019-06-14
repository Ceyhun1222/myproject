Namespace Telerik.Examples.WinControls.Scheduler.DayView

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class Form1
        Inherits Telerik.QuickStart.WinControls.ExamplesForm

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.


#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim dateTimeInterval1 As New Telerik.WinControls.UI.DateTimeInterval()
            Dim schedulerDailyPrintStyle1 As New Telerik.WinControls.UI.SchedulerDailyPrintStyle()
            Me.radScheduler1 = New Telerik.WinControls.UI.RadScheduler()
            Me.radSchedulerNavigator1 = New Telerik.WinControls.UI.RadSchedulerNavigator()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.panel1 = New System.Windows.Forms.Panel()
            Me.radLabel6 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.dropDownRangeFactor = New Telerik.WinControls.UI.RadDropDownList()
            Me.spinEditorRulerWidth = New Telerik.WinControls.UI.RadSpinEditor()
            Me.spinEditorPointerWidth = New Telerik.WinControls.UI.RadSpinEditor()
            Me.spinEditorScaleSize = New Telerik.WinControls.UI.RadSpinEditor()
            Me.timePickerRulerEnd = New Telerik.WinControls.UI.RadTimePicker()
            Me.timePickerRulerStart = New Telerik.WinControls.UI.RadTimePicker()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.panel2 = New System.Windows.Forms.Panel()
            Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel8 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel9 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel10 = New Telerik.WinControls.UI.RadLabel()
            Me.dropDownWorkWeekEnd = New Telerik.WinControls.UI.RadDropDownList()
            Me.dropDownWorkWeekStart = New Telerik.WinControls.UI.RadDropDownList()
            Me.timePickerWorkTimeEnd = New Telerik.WinControls.UI.RadTimePicker()
            Me.timePickerWorkTimeStart = New Telerik.WinControls.UI.RadTimePicker()
            Me.radGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
            Me.panel3 = New System.Windows.Forms.Panel()
            Me.radLabel13 = New Telerik.WinControls.UI.RadLabel()
            Me.spinEditorAllDayMaxHeight = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabel11 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel12 = New Telerik.WinControls.UI.RadLabel()
            Me.spinEditorAllDayHeight = New Telerik.WinControls.UI.RadSpinEditor()
            Me.spinEditorAppointmentSpacing = New Telerik.WinControls.UI.RadSpinEditor()
            Me.checkBoxExactTimeRendering = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowHeader = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowAllDayArea = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowRuler = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowWeekend = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBox4 = New Telerik.WinControls.UI.RadGroupBox()
            Me.radTrackBar1 = New Telerik.WinControls.UI.RadTrackBar()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radSchedulerNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            Me.panel1.SuspendLayout()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownRangeFactor, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorRulerWidth, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorPointerWidth, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorScaleSize, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerRulerEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerRulerStart, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            Me.panel2.SuspendLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel9, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel10, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownWorkWeekEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownWorkWeekStart, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerWorkTimeEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerWorkTimeStart, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox3.SuspendLayout()
            Me.panel3.SuspendLayout()
            DirectCast(Me.radLabel13, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorAllDayMaxHeight, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel11, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel12, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorAllDayHeight, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorAppointmentSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxExactTimeRendering, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowHeader, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowAllDayArea, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowRuler, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowWeekend, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox4.SuspendLayout()
            DirectCast(Me.radTrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.radGroupBox4)
            Me.settingsPanel.Controls.Add(Me.radGroupBox3)
            Me.settingsPanel.Controls.Add(Me.radGroupBox2)
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(838, 37)
            Me.settingsPanel.Size = New System.Drawing.Size(230, 713)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox3, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox4, 0)
            ' 
            ' radScheduler1
            ' 
            dateTimeInterval1.[End] = New System.DateTime(CLng(0))
            dateTimeInterval1.Start = New System.DateTime(CLng(0))
            Me.radScheduler1.AccessibleInterval = dateTimeInterval1
            Me.radScheduler1.Culture = New System.Globalization.CultureInfo("en-US")
            Me.radScheduler1.DataSource = Nothing
            Me.radScheduler1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radScheduler1.Location = New System.Drawing.Point(0, 77)
            Me.radScheduler1.Name = "radScheduler1"
            schedulerDailyPrintStyle1.AppointmentFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            schedulerDailyPrintStyle1.DateEndRange = New System.DateTime(2014, 6, 14, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle1.DateHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold)
            schedulerDailyPrintStyle1.DateStartRange = New System.DateTime(2014, 6, 9, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle1.PageHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 22.0F, System.Drawing.FontStyle.Bold)
            Me.radScheduler1.PrintStyle = schedulerDailyPrintStyle1
            Me.radScheduler1.Size = New System.Drawing.Size(1487, 913)
            Me.radScheduler1.TabIndex = 2
            Me.radScheduler1.Text = "radScheduler1"
            ' 
            ' radSchedulerNavigator1
            ' 
            Me.radSchedulerNavigator1.AssociatedScheduler = Me.radScheduler1
            Me.radSchedulerNavigator1.DateFormat = "yyyy/MM/dd"
            Me.radSchedulerNavigator1.Dock = System.Windows.Forms.DockStyle.Top
            Me.radSchedulerNavigator1.Location = New System.Drawing.Point(0, 0)
            Me.radSchedulerNavigator1.Name = "radSchedulerNavigator1"
            Me.radSchedulerNavigator1.NavigationStepType = Telerik.WinControls.UI.NavigationStepTypes.Day
            ' 
            ' 
            ' 
            Me.radSchedulerNavigator1.RootElement.StretchVertically = False
            Me.radSchedulerNavigator1.Size = New System.Drawing.Size(1487, 77)
            Me.radSchedulerNavigator1.TabIndex = 3
            Me.radSchedulerNavigator1.Text = "radSchedulerNavigator1"
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.panel1)
            Me.radGroupBox1.HeaderText = "Ruler options"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 40)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(210, 179)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Ruler options"
            ' 
            ' panel1
            ' 
            Me.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.panel1.Controls.Add(Me.radLabel6)
            Me.panel1.Controls.Add(Me.radLabel5)
            Me.panel1.Controls.Add(Me.radLabel4)
            Me.panel1.Controls.Add(Me.radLabel3)
            Me.panel1.Controls.Add(Me.radLabel2)
            Me.panel1.Controls.Add(Me.radLabel1)
            Me.panel1.Controls.Add(Me.dropDownRangeFactor)
            Me.panel1.Controls.Add(Me.spinEditorRulerWidth)
            Me.panel1.Controls.Add(Me.spinEditorPointerWidth)
            Me.panel1.Controls.Add(Me.spinEditorScaleSize)
            Me.panel1.Controls.Add(Me.timePickerRulerEnd)
            Me.panel1.Controls.Add(Me.timePickerRulerStart)
            Me.panel1.Location = New System.Drawing.Point(5, 18)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(200, 164)
            Me.panel1.TabIndex = 9
            ' 
            ' radLabel6
            ' 
            Me.radLabel6.Location = New System.Drawing.Point(3, 134)
            Me.radLabel6.Name = "radLabel6"
            Me.radLabel6.Size = New System.Drawing.Size(73, 18)
            Me.radLabel6.TabIndex = 19
            Me.radLabel6.Text = "Pointer width"
            ' 
            ' radLabel5
            ' 
            Me.radLabel5.Location = New System.Drawing.Point(3, 108)
            Me.radLabel5.Name = "radLabel5"
            Me.radLabel5.Size = New System.Drawing.Size(63, 18)
            Me.radLabel5.TabIndex = 18
            Me.radLabel5.Text = "Ruler width"
            ' 
            ' radLabel4
            ' 
            Me.radLabel4.Location = New System.Drawing.Point(3, 81)
            Me.radLabel4.Name = "radLabel4"
            Me.radLabel4.Size = New System.Drawing.Size(70, 18)
            Me.radLabel4.TabIndex = 17
            Me.radLabel4.Text = "Range factor"
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Location = New System.Drawing.Point(3, 56)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(54, 18)
            Me.radLabel3.TabIndex = 16
            Me.radLabel3.Text = "Scale size"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Location = New System.Drawing.Point(3, 30)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(54, 18)
            Me.radLabel2.TabIndex = 15
            Me.radLabel2.Text = "Ruler end"
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(3, 4)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(57, 18)
            Me.radLabel1.TabIndex = 14
            Me.radLabel1.Text = "Ruler start"
            ' 
            ' dropDownRangeFactor
            ' 
            Me.dropDownRangeFactor.AllowShowFocusCues = False
            Me.dropDownRangeFactor.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.dropDownRangeFactor.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.dropDownRangeFactor.Location = New System.Drawing.Point(90, 81)
            Me.dropDownRangeFactor.Name = "dropDownRangeFactor"
            Me.dropDownRangeFactor.Size = New System.Drawing.Size(107, 20)
            Me.dropDownRangeFactor.TabIndex = 13
            Me.dropDownRangeFactor.Text = "radDropDownList1"
            ' 
            ' spinEditorRulerWidth
            ' 
            Me.spinEditorRulerWidth.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorRulerWidth.Location = New System.Drawing.Point(90, 107)
            Me.spinEditorRulerWidth.Name = "spinEditorRulerWidth"
            Me.spinEditorRulerWidth.Size = New System.Drawing.Size(107, 20)
            Me.spinEditorRulerWidth.TabIndex = 12
            Me.spinEditorRulerWidth.TabStop = False
            ' 
            ' spinEditorPointerWidth
            ' 
            Me.spinEditorPointerWidth.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorPointerWidth.Location = New System.Drawing.Point(90, 133)
            Me.spinEditorPointerWidth.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.spinEditorPointerWidth.Name = "spinEditorPointerWidth"
            Me.spinEditorPointerWidth.Size = New System.Drawing.Size(107, 20)
            Me.spinEditorPointerWidth.TabIndex = 11
            Me.spinEditorPointerWidth.TabStop = False
            ' 
            ' spinEditorScaleSize
            ' 
            Me.spinEditorScaleSize.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorScaleSize.Location = New System.Drawing.Point(90, 55)
            Me.spinEditorScaleSize.Minimum = New Decimal(New Integer() {21, 0, 0, 0})
            Me.spinEditorScaleSize.Name = "spinEditorScaleSize"
            Me.spinEditorScaleSize.Size = New System.Drawing.Size(107, 20)
            Me.spinEditorScaleSize.TabIndex = 10
            Me.spinEditorScaleSize.TabStop = False
            Me.spinEditorScaleSize.Value = New Decimal(New Integer() {22, 0, 0, 0})
            ' 
            ' timePickerRulerEnd
            ' 
            Me.timePickerRulerEnd.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.timePickerRulerEnd.Location = New System.Drawing.Point(90, 29)
            Me.timePickerRulerEnd.Name = "timePickerRulerEnd"
            Me.timePickerRulerEnd.Size = New System.Drawing.Size(107, 20)
            Me.timePickerRulerEnd.TabIndex = 9
            Me.timePickerRulerEnd.TabStop = False
            Me.timePickerRulerEnd.Text = "radTimePicker2"
            Me.timePickerRulerEnd.Value = New System.DateTime(2014, 6, 9, 14, 58, 13, _
                0)
            ' 
            ' timePickerRulerStart
            ' 
            Me.timePickerRulerStart.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.timePickerRulerStart.Location = New System.Drawing.Point(90, 3)
            Me.timePickerRulerStart.Name = "timePickerRulerStart"
            Me.timePickerRulerStart.Size = New System.Drawing.Size(107, 20)
            Me.timePickerRulerStart.TabIndex = 8
            Me.timePickerRulerStart.TabStop = False
            Me.timePickerRulerStart.Text = "radTimePicker1"
            Me.timePickerRulerStart.Value = New System.DateTime(2014, 6, 9, 14, 58, 13, _
                0)
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox2.Controls.Add(Me.panel2)
            Me.radGroupBox2.HeaderText = "Working time options"
            Me.radGroupBox2.Location = New System.Drawing.Point(10, 225)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Size = New System.Drawing.Size(210, 131)
            Me.radGroupBox2.TabIndex = 2
            Me.radGroupBox2.Text = "Working time options"
            ' 
            ' panel2
            ' 
            Me.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.panel2.Controls.Add(Me.radLabel7)
            Me.panel2.Controls.Add(Me.radLabel8)
            Me.panel2.Controls.Add(Me.radLabel9)
            Me.panel2.Controls.Add(Me.radLabel10)
            Me.panel2.Controls.Add(Me.dropDownWorkWeekEnd)
            Me.panel2.Controls.Add(Me.dropDownWorkWeekStart)
            Me.panel2.Controls.Add(Me.timePickerWorkTimeEnd)
            Me.panel2.Controls.Add(Me.timePickerWorkTimeStart)
            Me.panel2.Location = New System.Drawing.Point(5, 21)
            Me.panel2.Name = "panel2"
            Me.panel2.Size = New System.Drawing.Size(200, 113)
            Me.panel2.TabIndex = 9
            ' 
            ' radLabel7
            ' 
            Me.radLabel7.Location = New System.Drawing.Point(3, 83)
            Me.radLabel7.Name = "radLabel7"
            Me.radLabel7.Size = New System.Drawing.Size(100, 18)
            Me.radLabel7.TabIndex = 21
            Me.radLabel7.Text = "Working week end"
            ' 
            ' radLabel8
            ' 
            Me.radLabel8.Location = New System.Drawing.Point(3, 58)
            Me.radLabel8.Name = "radLabel8"
            Me.radLabel8.Size = New System.Drawing.Size(103, 18)
            Me.radLabel8.TabIndex = 20
            Me.radLabel8.Text = "Working week start"
            ' 
            ' radLabel9
            ' 
            Me.radLabel9.Location = New System.Drawing.Point(3, 32)
            Me.radLabel9.Name = "radLabel9"
            Me.radLabel9.Size = New System.Drawing.Size(80, 18)
            Me.radLabel9.TabIndex = 19
            Me.radLabel9.Text = "Work time end"
            ' 
            ' radLabel10
            ' 
            Me.radLabel10.Location = New System.Drawing.Point(3, 6)
            Me.radLabel10.Name = "radLabel10"
            Me.radLabel10.Size = New System.Drawing.Size(84, 18)
            Me.radLabel10.TabIndex = 18
            Me.radLabel10.Text = "Work time start"
            ' 
            ' dropDownWorkWeekEnd
            ' 
            Me.dropDownWorkWeekEnd.AllowShowFocusCues = False
            Me.dropDownWorkWeekEnd.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.dropDownWorkWeekEnd.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.dropDownWorkWeekEnd.Location = New System.Drawing.Point(120, 83)
            Me.dropDownWorkWeekEnd.Name = "dropDownWorkWeekEnd"
            Me.dropDownWorkWeekEnd.Size = New System.Drawing.Size(78, 20)
            Me.dropDownWorkWeekEnd.TabIndex = 13
            Me.dropDownWorkWeekEnd.Text = "radDropDownList3"
            ' 
            ' dropDownWorkWeekStart
            ' 
            Me.dropDownWorkWeekStart.AllowShowFocusCues = False
            Me.dropDownWorkWeekStart.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.dropDownWorkWeekStart.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.dropDownWorkWeekStart.Location = New System.Drawing.Point(120, 57)
            Me.dropDownWorkWeekStart.Name = "dropDownWorkWeekStart"
            Me.dropDownWorkWeekStart.Size = New System.Drawing.Size(78, 20)
            Me.dropDownWorkWeekStart.TabIndex = 12
            Me.dropDownWorkWeekStart.Text = "radDropDownList2"
            ' 
            ' timePickerWorkTimeEnd
            ' 
            Me.timePickerWorkTimeEnd.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.timePickerWorkTimeEnd.Location = New System.Drawing.Point(103, 31)
            Me.timePickerWorkTimeEnd.Name = "timePickerWorkTimeEnd"
            Me.timePickerWorkTimeEnd.Size = New System.Drawing.Size(95, 20)
            Me.timePickerWorkTimeEnd.TabIndex = 11
            Me.timePickerWorkTimeEnd.TabStop = False
            Me.timePickerWorkTimeEnd.Text = "radTimePicker3"
            Me.timePickerWorkTimeEnd.Value = New System.DateTime(2014, 6, 9, 14, 58, 13, _
                0)
            ' 
            ' timePickerWorkTimeStart
            ' 
            Me.timePickerWorkTimeStart.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.timePickerWorkTimeStart.Location = New System.Drawing.Point(103, 5)
            Me.timePickerWorkTimeStart.Name = "timePickerWorkTimeStart"
            Me.timePickerWorkTimeStart.Size = New System.Drawing.Size(95, 20)
            Me.timePickerWorkTimeStart.TabIndex = 10
            Me.timePickerWorkTimeStart.TabStop = False
            Me.timePickerWorkTimeStart.Text = "radTimePicker4"
            Me.timePickerWorkTimeStart.Value = New System.DateTime(2014, 6, 9, 14, 58, 13, _
                0)
            ' 
            ' radGroupBox3
            ' 
            Me.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox3.Controls.Add(Me.panel3)
            Me.radGroupBox3.Controls.Add(Me.checkBoxExactTimeRendering)
            Me.radGroupBox3.Controls.Add(Me.checkBoxShowHeader)
            Me.radGroupBox3.Controls.Add(Me.checkBoxShowAllDayArea)
            Me.radGroupBox3.Controls.Add(Me.checkBoxShowRuler)
            Me.radGroupBox3.Controls.Add(Me.checkBoxShowWeekend)
            Me.radGroupBox3.HeaderText = "General options"
            Me.radGroupBox3.Location = New System.Drawing.Point(10, 362)
            Me.radGroupBox3.Name = "radGroupBox3"
            Me.radGroupBox3.Size = New System.Drawing.Size(210, 227)
            Me.radGroupBox3.TabIndex = 3
            Me.radGroupBox3.Text = "General options"
            ' 
            ' panel3
            ' 
            Me.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.panel3.Controls.Add(Me.radLabel13)
            Me.panel3.Controls.Add(Me.spinEditorAllDayMaxHeight)
            Me.panel3.Controls.Add(Me.radLabel11)
            Me.panel3.Controls.Add(Me.radLabel12)
            Me.panel3.Controls.Add(Me.spinEditorAllDayHeight)
            Me.panel3.Controls.Add(Me.spinEditorAppointmentSpacing)
            Me.panel3.Location = New System.Drawing.Point(5, 141)
            Me.panel3.Name = "panel3"
            Me.panel3.Size = New System.Drawing.Size(200, 81)
            Me.panel3.TabIndex = 9
            ' 
            ' radLabel13
            ' 
            Me.radLabel13.Location = New System.Drawing.Point(3, 30)
            Me.radLabel13.Name = "radLabel13"
            Me.radLabel13.Size = New System.Drawing.Size(123, 18)
            Me.radLabel13.TabIndex = 19
            Me.radLabel13.Text = "All day area max height"
            ' 
            ' spinEditorAllDayMaxHeight
            ' 
            Me.spinEditorAllDayMaxHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorAllDayMaxHeight.Location = New System.Drawing.Point(140, 29)
            Me.spinEditorAllDayMaxHeight.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
            Me.spinEditorAllDayMaxHeight.Name = "spinEditorAllDayMaxHeight"
            Me.spinEditorAllDayMaxHeight.Size = New System.Drawing.Size(58, 20)
            Me.spinEditorAllDayMaxHeight.TabIndex = 18
            Me.spinEditorAllDayMaxHeight.TabStop = False
            ' 
            ' radLabel11
            ' 
            Me.radLabel11.Location = New System.Drawing.Point(3, 56)
            Me.radLabel11.Name = "radLabel11"
            Me.radLabel11.Size = New System.Drawing.Size(119, 18)
            Me.radLabel11.TabIndex = 17
            Me.radLabel11.Text = "Appointments spacing"
            ' 
            ' radLabel12
            ' 
            Me.radLabel12.Location = New System.Drawing.Point(3, 4)
            Me.radLabel12.Name = "radLabel12"
            Me.radLabel12.Size = New System.Drawing.Size(100, 18)
            Me.radLabel12.TabIndex = 16
            Me.radLabel12.Text = "All day area height"
            ' 
            ' spinEditorAllDayHeight
            ' 
            Me.spinEditorAllDayHeight.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorAllDayHeight.Location = New System.Drawing.Point(140, 3)
            Me.spinEditorAllDayHeight.Name = "spinEditorAllDayHeight"
            Me.spinEditorAllDayHeight.Size = New System.Drawing.Size(58, 20)
            Me.spinEditorAllDayHeight.TabIndex = 8
            Me.spinEditorAllDayHeight.TabStop = False
            ' 
            ' spinEditorAppointmentSpacing
            ' 
            Me.spinEditorAppointmentSpacing.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorAppointmentSpacing.Location = New System.Drawing.Point(140, 55)
            Me.spinEditorAppointmentSpacing.Maximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.spinEditorAppointmentSpacing.Name = "spinEditorAppointmentSpacing"
            Me.spinEditorAppointmentSpacing.Size = New System.Drawing.Size(58, 20)
            Me.spinEditorAppointmentSpacing.TabIndex = 7
            Me.spinEditorAppointmentSpacing.TabStop = False
            ' 
            ' checkBoxExactTimeRendering
            ' 
            Me.checkBoxExactTimeRendering.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxExactTimeRendering.Location = New System.Drawing.Point(5, 117)
            Me.checkBoxExactTimeRendering.Name = "checkBoxExactTimeRendering"
            Me.checkBoxExactTimeRendering.Size = New System.Drawing.Size(123, 18)
            Me.checkBoxExactTimeRendering.TabIndex = 4
            Me.checkBoxExactTimeRendering.Text = "Exact time rendering"
            ' 
            ' checkBoxShowHeader
            ' 
            Me.checkBoxShowHeader.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowHeader.Location = New System.Drawing.Point(5, 93)
            Me.checkBoxShowHeader.Name = "checkBoxShowHeader"
            Me.checkBoxShowHeader.Size = New System.Drawing.Size(151, 18)
            Me.checkBoxShowHeader.TabIndex = 3
            Me.checkBoxShowHeader.Text = "Show the DayView header"
            ' 
            ' checkBoxShowAllDayArea
            ' 
            Me.checkBoxShowAllDayArea.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowAllDayArea.Location = New System.Drawing.Point(5, 69)
            Me.checkBoxShowAllDayArea.Name = "checkBoxShowAllDayArea"
            Me.checkBoxShowAllDayArea.Size = New System.Drawing.Size(126, 18)
            Me.checkBoxShowAllDayArea.TabIndex = 2
            Me.checkBoxShowAllDayArea.Text = "Show the AllDay area"
            ' 
            ' checkBoxShowRuler
            ' 
            Me.checkBoxShowRuler.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowRuler.Location = New System.Drawing.Point(5, 45)
            Me.checkBoxShowRuler.Name = "checkBoxShowRuler"
            Me.checkBoxShowRuler.Size = New System.Drawing.Size(93, 18)
            Me.checkBoxShowRuler.TabIndex = 1
            Me.checkBoxShowRuler.Text = "Show the ruler"
            ' 
            ' checkBoxShowWeekend
            ' 
            Me.checkBoxShowWeekend.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowWeekend.Location = New System.Drawing.Point(5, 21)
            Me.checkBoxShowWeekend.Name = "checkBoxShowWeekend"
            Me.checkBoxShowWeekend.Size = New System.Drawing.Size(114, 18)
            Me.checkBoxShowWeekend.TabIndex = 0
            Me.checkBoxShowWeekend.Text = "Show the weekend"
            ' 
            ' radGroupBox4
            ' 
            Me.radGroupBox4.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox4.Controls.Add(Me.radTrackBar1)
            Me.radGroupBox4.HeaderText = "Resize the selected column"
            Me.radGroupBox4.Location = New System.Drawing.Point(10, 595)
            Me.radGroupBox4.Name = "radGroupBox4"
            Me.radGroupBox4.Size = New System.Drawing.Size(210, 84)
            Me.radGroupBox4.TabIndex = 4
            Me.radGroupBox4.Text = "Resize the selected column"
            ' 
            ' radTrackBar1
            ' 
            Me.radTrackBar1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radTrackBar1.LabelStyle = Telerik.WinControls.UI.TrackBarLabelStyle.BottomRight
            Me.radTrackBar1.LargeTickFrequency = 1
            Me.radTrackBar1.Location = New System.Drawing.Point(5, 21)
            Me.radTrackBar1.Maximum = 10.0F
            Me.radTrackBar1.Minimum = 1.0F
            Me.radTrackBar1.Name = "radTrackBar1"
            Me.radTrackBar1.Size = New System.Drawing.Size(200, 55)
            Me.radTrackBar1.TabIndex = 0
            Me.radTrackBar1.Text = "radTrackBar1"
            Me.radTrackBar1.Value = 1.0F
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radScheduler1)
            Me.Controls.Add(Me.radSchedulerNavigator1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1497, 1000)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radSchedulerNavigator1, 0)
            Me.Controls.SetChildIndex(Me.radScheduler1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radSchedulerNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.panel1.ResumeLayout(False)
            Me.panel1.PerformLayout()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownRangeFactor, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorRulerWidth, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorPointerWidth, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorScaleSize, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerRulerEnd, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerRulerStart, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.panel2.ResumeLayout(False)
            Me.panel2.PerformLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel9, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel10, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownWorkWeekEnd, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownWorkWeekStart, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerWorkTimeEnd, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerWorkTimeStart, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox3.ResumeLayout(False)
            Me.radGroupBox3.PerformLayout()
            Me.panel3.ResumeLayout(False)
            Me.panel3.PerformLayout()
            DirectCast(Me.radLabel13, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorAllDayMaxHeight, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel11, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel12, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorAllDayHeight, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorAppointmentSpacing, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxExactTimeRendering, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowHeader, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowAllDayArea, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowRuler, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowWeekend, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox4.ResumeLayout(False)
            Me.radGroupBox4.PerformLayout()
            DirectCast(Me.radTrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Friend WithEvents radScheduler1 As Telerik.WinControls.UI.RadScheduler
        Friend WithEvents radSchedulerNavigator1 As Telerik.WinControls.UI.RadSchedulerNavigator
        Friend WithEvents radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radGroupBox3 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents checkBoxShowHeader As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxShowAllDayArea As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxShowRuler As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxShowWeekend As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents spinEditorAllDayHeight As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents spinEditorAppointmentSpacing As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents checkBoxExactTimeRendering As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents panel1 As System.Windows.Forms.Panel
        Friend WithEvents dropDownRangeFactor As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents spinEditorRulerWidth As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents spinEditorPointerWidth As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents spinEditorScaleSize As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents timePickerRulerEnd As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents timePickerRulerStart As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents radLabel6 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel5 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel4 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel3 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel2 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel1 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents panel2 As System.Windows.Forms.Panel
        Friend WithEvents radLabel7 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel8 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel9 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel10 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents dropDownWorkWeekEnd As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents dropDownWorkWeekStart As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents timePickerWorkTimeEnd As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents timePickerWorkTimeStart As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents panel3 As System.Windows.Forms.Panel
        Friend WithEvents radLabel11 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel12 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel13 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents spinEditorAllDayMaxHeight As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents radGroupBox4 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radTrackBar1 As Telerik.WinControls.UI.RadTrackBar

    End Class
End Namespace
