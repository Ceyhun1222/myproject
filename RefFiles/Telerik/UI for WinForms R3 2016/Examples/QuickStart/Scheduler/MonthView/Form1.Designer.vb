Namespace Telerik.Examples.WinControls.Scheduler.MonthView
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class Form1
        Inherits QuickStart.WinControls.ExamplesForm

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

#Region "Windows Form Designer generated code"

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim dateTimeInterval1 As New Telerik.WinControls.UI.DateTimeInterval()
            Dim schedulerDailyPrintStyle1 As New Telerik.WinControls.UI.SchedulerDailyPrintStyle()
            Me.radScheduler1 = New Telerik.WinControls.UI.RadScheduler()
            Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
            Me.checkBoxCellsOverflow = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxAppointmentsScrolling = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxEnableWeeksHeader = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowWeeksHeader = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxVerticalScroll = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
            Me.panel2 = New System.Windows.Forms.Panel()
            Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel8 = New Telerik.WinControls.UI.RadLabel()
            Me.dropDownWorkWeekEnd = New Telerik.WinControls.UI.RadDropDownList()
            Me.dropDownWorkWeekStart = New Telerik.WinControls.UI.RadDropDownList()
            Me.radLabel6 = New Telerik.WinControls.UI.RadLabel()
            Me.spinEditorWeekCount = New Telerik.WinControls.UI.RadSpinEditor()
            Me.checkBoxShowWeekend = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxShowFullMonth = New Telerik.WinControls.UI.RadCheckBox()
            Me.radGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
            Me.trackBarColumnSize = New Telerik.WinControls.UI.RadTrackBar()
            Me.trackBarRowSize = New Telerik.WinControls.UI.RadTrackBar()
            Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
            Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
            Me.panel1 = New System.Windows.Forms.Panel()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.spinEditorAppointmentSpacing = New Telerik.WinControls.UI.RadSpinEditor()
            Me.checkBoxAutoSizeAppointments = New Telerik.WinControls.UI.RadCheckBox()
            Me.checkBoxExactTimeRendering = New Telerik.WinControls.UI.RadCheckBox()
            Me.timePickerRulerStart = New Telerik.WinControls.UI.RadTimePicker()
            Me.timePickerRulerEnd = New Telerik.WinControls.UI.RadTimePicker()
            Me.buttonBackToMonthView = New Telerik.WinControls.UI.RadButton()
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox1.SuspendLayout()
            DirectCast(Me.checkBoxCellsOverflow, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxAppointmentsScrolling, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxEnableWeeksHeader, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowWeeksHeader, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxVerticalScroll, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox2.SuspendLayout()
            Me.panel2.SuspendLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownWorkWeekEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.dropDownWorkWeekStart, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorWeekCount, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowWeekend, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxShowFullMonth, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupBox3.SuspendLayout()
            DirectCast(Me.trackBarColumnSize, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.trackBarRowSize, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panel1.SuspendLayout()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.spinEditorAppointmentSpacing, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxAutoSizeAppointments, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.checkBoxExactTimeRendering, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerRulerStart, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.timePickerRulerEnd, System.ComponentModel.ISupportInitialize).BeginInit()
            DirectCast(Me.buttonBackToMonthView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            ' 
            ' settingsPanel
            ' 
            Me.settingsPanel.Controls.Add(Me.buttonBackToMonthView)
            Me.settingsPanel.Controls.Add(Me.radGroupBox3)
            Me.settingsPanel.Controls.Add(Me.radGroupBox2)
            Me.settingsPanel.Controls.Add(Me.radGroupBox1)
            Me.settingsPanel.Location = New System.Drawing.Point(1065, 80)
            Me.settingsPanel.Size = New System.Drawing.Size(230, 719)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox3, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.buttonBackToMonthView, 0)
            ' 
            ' radScheduler1
            ' 
            dateTimeInterval1.[End] = New System.DateTime(CLng(0))
            dateTimeInterval1.Start = New System.DateTime(CLng(0))
            Me.radScheduler1.AccessibleInterval = dateTimeInterval1
            Me.radScheduler1.ActiveViewType = Telerik.WinControls.UI.SchedulerViewType.Month
            Me.radScheduler1.Culture = New System.Globalization.CultureInfo("en-US")
            Me.radScheduler1.DataSource = Nothing
            Me.radScheduler1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.radScheduler1.Location = New System.Drawing.Point(0, 0)
            Me.radScheduler1.Name = "radScheduler1"
            schedulerDailyPrintStyle1.AppointmentFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
            schedulerDailyPrintStyle1.DateEndRange = New System.DateTime(2014, 6, 15, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle1.DateHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold)
            schedulerDailyPrintStyle1.DateStartRange = New System.DateTime(2014, 6, 10, 0, 0, 0, _
                0)
            schedulerDailyPrintStyle1.PageHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 22.0F, System.Drawing.FontStyle.Bold)
            Me.radScheduler1.PrintStyle = schedulerDailyPrintStyle1
            Me.radScheduler1.Size = New System.Drawing.Size(1531, 990)
            Me.radScheduler1.TabIndex = 2
            Me.radScheduler1.Text = "radScheduler1"
            ' 
            ' radGroupBox1
            ' 
            Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox1.Controls.Add(Me.checkBoxCellsOverflow)
            Me.radGroupBox1.Controls.Add(Me.checkBoxAppointmentsScrolling)
            Me.radGroupBox1.Controls.Add(Me.checkBoxEnableWeeksHeader)
            Me.radGroupBox1.Controls.Add(Me.checkBoxShowWeeksHeader)
            Me.radGroupBox1.Controls.Add(Me.checkBoxVerticalScroll)
            Me.radGroupBox1.HeaderText = "Visual settings"
            Me.radGroupBox1.Location = New System.Drawing.Point(10, 33)
            Me.radGroupBox1.Name = "radGroupBox1"
            Me.radGroupBox1.Size = New System.Drawing.Size(210, 143)
            Me.radGroupBox1.TabIndex = 1
            Me.radGroupBox1.Text = "Visual settings"
            ' 
            ' checkBoxCellsOverflow
            ' 
            Me.checkBoxCellsOverflow.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxCellsOverflow.Location = New System.Drawing.Point(5, 117)
            Me.checkBoxCellsOverflow.Name = "checkBoxCellsOverflow"
            Me.checkBoxCellsOverflow.Size = New System.Drawing.Size(161, 18)
            Me.checkBoxCellsOverflow.TabIndex = 4
            Me.checkBoxCellsOverflow.Text = "Enable cells overflow button"
            ' 
            ' checkBoxAppointmentsScrolling
            ' 
            Me.checkBoxAppointmentsScrolling.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxAppointmentsScrolling.Location = New System.Drawing.Point(5, 93)
            Me.checkBoxAppointmentsScrolling.Name = "checkBoxAppointmentsScrolling"
            Me.checkBoxAppointmentsScrolling.Size = New System.Drawing.Size(171, 18)
            Me.checkBoxAppointmentsScrolling.TabIndex = 3
            Me.checkBoxAppointmentsScrolling.Text = "Enable row scrollbars"
            ' 
            ' checkBoxEnableWeeksHeader
            ' 
            Me.checkBoxEnableWeeksHeader.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxEnableWeeksHeader.Location = New System.Drawing.Point(5, 69)
            Me.checkBoxEnableWeeksHeader.Name = "checkBoxEnableWeeksHeader"
            Me.checkBoxEnableWeeksHeader.Size = New System.Drawing.Size(125, 18)
            Me.checkBoxEnableWeeksHeader.TabIndex = 2
            Me.checkBoxEnableWeeksHeader.Text = "Enable weeks header"
            ' 
            ' checkBoxShowWeeksHeader
            ' 
            Me.checkBoxShowWeeksHeader.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowWeeksHeader.Location = New System.Drawing.Point(5, 45)
            Me.checkBoxShowWeeksHeader.Name = "checkBoxShowWeeksHeader"
            Me.checkBoxShowWeeksHeader.Size = New System.Drawing.Size(119, 18)
            Me.checkBoxShowWeeksHeader.TabIndex = 1
            Me.checkBoxShowWeeksHeader.Text = "Show weeks header"
            ' 
            ' checkBoxVerticalScroll
            ' 
            Me.checkBoxVerticalScroll.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxVerticalScroll.Location = New System.Drawing.Point(5, 21)
            Me.checkBoxVerticalScroll.Name = "checkBoxVerticalScroll"
            Me.checkBoxVerticalScroll.Size = New System.Drawing.Size(132, 18)
            Me.checkBoxVerticalScroll.TabIndex = 0
            Me.checkBoxVerticalScroll.Text = "Show vertical scrollbar"
            ' 
            ' radGroupBox2
            ' 
            Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox2.Controls.Add(Me.panel2)
            Me.radGroupBox2.Controls.Add(Me.checkBoxShowWeekend)
            Me.radGroupBox2.Controls.Add(Me.checkBoxShowFullMonth)
            Me.radGroupBox2.HeaderText = "View settings"
            Me.radGroupBox2.Location = New System.Drawing.Point(10, 182)
            Me.radGroupBox2.Name = "radGroupBox2"
            Me.radGroupBox2.Size = New System.Drawing.Size(210, 145)
            Me.radGroupBox2.TabIndex = 2
            Me.radGroupBox2.Text = "View settings"
            ' 
            ' panel2
            ' 
            Me.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.panel2.Controls.Add(Me.radLabel7)
            Me.panel2.Controls.Add(Me.radLabel8)
            Me.panel2.Controls.Add(Me.dropDownWorkWeekEnd)
            Me.panel2.Controls.Add(Me.dropDownWorkWeekStart)
            Me.panel2.Controls.Add(Me.radLabel6)
            Me.panel2.Controls.Add(Me.spinEditorWeekCount)
            Me.panel2.Location = New System.Drawing.Point(5, 22)
            Me.panel2.Name = "panel2"
            Me.panel2.Size = New System.Drawing.Size(200, 78)
            Me.panel2.TabIndex = 4
            ' 
            ' radLabel7
            ' 
            Me.radLabel7.Location = New System.Drawing.Point(0, 28)
            Me.radLabel7.Name = "radLabel7"
            Me.radLabel7.Size = New System.Drawing.Size(100, 18)
            Me.radLabel7.TabIndex = 25
            Me.radLabel7.Text = "Working week end"
            ' 
            ' radLabel8
            ' 
            Me.radLabel8.Location = New System.Drawing.Point(0, 3)
            Me.radLabel8.Name = "radLabel8"
            Me.radLabel8.Size = New System.Drawing.Size(103, 18)
            Me.radLabel8.TabIndex = 24
            Me.radLabel8.Text = "Working week start"
            ' 
            ' dropDownWorkWeekEnd
            ' 
            Me.dropDownWorkWeekEnd.AllowShowFocusCues = False
            Me.dropDownWorkWeekEnd.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.dropDownWorkWeekEnd.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.dropDownWorkWeekEnd.Location = New System.Drawing.Point(121, 28)
            Me.dropDownWorkWeekEnd.Name = "dropDownWorkWeekEnd"
            Me.dropDownWorkWeekEnd.Size = New System.Drawing.Size(78, 20)
            Me.dropDownWorkWeekEnd.TabIndex = 23
            Me.dropDownWorkWeekEnd.Text = "radDropDownList3"
            ' 
            ' dropDownWorkWeekStart
            ' 
            Me.dropDownWorkWeekStart.AllowShowFocusCues = False
            Me.dropDownWorkWeekStart.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.dropDownWorkWeekStart.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.dropDownWorkWeekStart.Location = New System.Drawing.Point(121, 2)
            Me.dropDownWorkWeekStart.Name = "dropDownWorkWeekStart"
            Me.dropDownWorkWeekStart.Size = New System.Drawing.Size(78, 20)
            Me.dropDownWorkWeekStart.TabIndex = 22
            Me.dropDownWorkWeekStart.Text = "radDropDownList2"
            ' 
            ' radLabel6
            ' 
            Me.radLabel6.Location = New System.Drawing.Point(0, 55)
            Me.radLabel6.Name = "radLabel6"
            Me.radLabel6.Size = New System.Drawing.Size(66, 18)
            Me.radLabel6.TabIndex = 21
            Me.radLabel6.Text = "Week count"
            ' 
            ' spinEditorWeekCount
            ' 
            Me.spinEditorWeekCount.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.spinEditorWeekCount.Location = New System.Drawing.Point(121, 54)
            Me.spinEditorWeekCount.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
            Me.spinEditorWeekCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.spinEditorWeekCount.Name = "spinEditorWeekCount"
            Me.spinEditorWeekCount.Size = New System.Drawing.Size(78, 20)
            Me.spinEditorWeekCount.TabIndex = 20
            Me.spinEditorWeekCount.TabStop = False
            Me.spinEditorWeekCount.Value = New Decimal(New Integer() {3, 0, 0, 0})
            ' 
            ' checkBoxShowWeekend
            ' 
            Me.checkBoxShowWeekend.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowWeekend.Location = New System.Drawing.Point(5, 121)
            Me.checkBoxShowWeekend.Name = "checkBoxShowWeekend"
            Me.checkBoxShowWeekend.Size = New System.Drawing.Size(95, 18)
            Me.checkBoxShowWeekend.TabIndex = 3
            Me.checkBoxShowWeekend.Text = "Show weekend"
            ' 
            ' checkBoxShowFullMonth
            ' 
            Me.checkBoxShowFullMonth.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxShowFullMonth.Location = New System.Drawing.Point(5, 102)
            Me.checkBoxShowFullMonth.Name = "checkBoxShowFullMonth"
            Me.checkBoxShowFullMonth.Size = New System.Drawing.Size(102, 18)
            Me.checkBoxShowFullMonth.TabIndex = 2
            Me.checkBoxShowFullMonth.Text = "Show full month"
            ' 
            ' radGroupBox3
            ' 
            Me.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupBox3.Controls.Add(Me.trackBarColumnSize)
            Me.radGroupBox3.Controls.Add(Me.trackBarRowSize)
            Me.radGroupBox3.Controls.Add(Me.radLabel3)
            Me.radGroupBox3.Controls.Add(Me.radLabel2)
            Me.radGroupBox3.Controls.Add(Me.panel1)
            Me.radGroupBox3.Controls.Add(Me.checkBoxAutoSizeAppointments)
            Me.radGroupBox3.Controls.Add(Me.checkBoxExactTimeRendering)
            Me.radGroupBox3.HeaderText = "Sizing options"
            Me.radGroupBox3.Location = New System.Drawing.Point(10, 333)
            Me.radGroupBox3.Name = "radGroupBox3"
            Me.radGroupBox3.Size = New System.Drawing.Size(210, 261)
            Me.radGroupBox3.TabIndex = 3
            Me.radGroupBox3.Text = "Sizing options"
            ' 
            ' trackBarColumnSize
            ' 
            Me.trackBarColumnSize.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.trackBarColumnSize.LabelStyle = Telerik.WinControls.UI.TrackBarLabelStyle.BottomRight
            Me.trackBarColumnSize.LargeTickFrequency = 1
            Me.trackBarColumnSize.Location = New System.Drawing.Point(5, 202)
            Me.trackBarColumnSize.Maximum = 10.0F
            Me.trackBarColumnSize.Minimum = 1.0F
            Me.trackBarColumnSize.Name = "trackBarColumnSize"
            Me.trackBarColumnSize.Size = New System.Drawing.Size(200, 55)
            Me.trackBarColumnSize.TabIndex = 6
            Me.trackBarColumnSize.Text = "radTrackBar2"
            Me.trackBarColumnSize.Value = 1.0F
            ' 
            ' trackBarRowSize
            ' 
            Me.trackBarRowSize.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.trackBarRowSize.LabelStyle = Telerik.WinControls.UI.TrackBarLabelStyle.BottomRight
            Me.trackBarRowSize.LargeTickFrequency = 1
            Me.trackBarRowSize.Location = New System.Drawing.Point(5, 114)
            Me.trackBarRowSize.Maximum = 10.0F
            Me.trackBarRowSize.Minimum = 1.0F
            Me.trackBarRowSize.Name = "trackBarRowSize"
            Me.trackBarRowSize.Size = New System.Drawing.Size(200, 55)
            Me.trackBarRowSize.TabIndex = 0
            Me.trackBarRowSize.Text = "radTrackBar1"
            Me.trackBarRowSize.Value = 1.0F
            ' 
            ' radLabel3
            ' 
            Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel3.Location = New System.Drawing.Point(5, 178)
            Me.radLabel3.Name = "radLabel3"
            Me.radLabel3.Size = New System.Drawing.Size(107, 18)
            Me.radLabel3.TabIndex = 5
            Me.radLabel3.Text = "Current column size:"
            ' 
            ' radLabel2
            ' 
            Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel2.Location = New System.Drawing.Point(5, 92)
            Me.radLabel2.Name = "radLabel2"
            Me.radLabel2.Size = New System.Drawing.Size(89, 18)
            Me.radLabel2.TabIndex = 4
            Me.radLabel2.Text = "Current row size:"
            ' 
            ' panel1
            ' 
            Me.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.panel1.Controls.Add(Me.radLabel1)
            Me.panel1.Controls.Add(Me.spinEditorAppointmentSpacing)
            Me.panel1.Location = New System.Drawing.Point(5, 61)
            Me.panel1.Name = "panel1"
            Me.panel1.Size = New System.Drawing.Size(200, 25)
            Me.panel1.TabIndex = 3
            ' 
            ' radLabel1
            ' 
            Me.radLabel1.Location = New System.Drawing.Point(0, 4)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(114, 18)
            Me.radLabel1.TabIndex = 2
            Me.radLabel1.Text = "Appointment spacing"
            ' 
            ' spinEditorAppointmentSpacing
            ' 
            Me.spinEditorAppointmentSpacing.Location = New System.Drawing.Point(130, 3)
            Me.spinEditorAppointmentSpacing.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.spinEditorAppointmentSpacing.Name = "spinEditorAppointmentSpacing"
            Me.spinEditorAppointmentSpacing.Size = New System.Drawing.Size(67, 20)
            Me.spinEditorAppointmentSpacing.TabIndex = 3
            Me.spinEditorAppointmentSpacing.TabStop = False
            ' 
            ' checkBoxAutoSizeAppointments
            ' 
            Me.checkBoxAutoSizeAppointments.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxAutoSizeAppointments.Location = New System.Drawing.Point(5, 40)
            Me.checkBoxAutoSizeAppointments.Name = "checkBoxAutoSizeAppointments"
            Me.checkBoxAutoSizeAppointments.Size = New System.Drawing.Size(139, 18)
            Me.checkBoxAutoSizeAppointments.TabIndex = 1
            Me.checkBoxAutoSizeAppointments.Text = "Auto size appointments"
            ' 
            ' checkBoxExactTimeRendering
            ' 
            Me.checkBoxExactTimeRendering.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.checkBoxExactTimeRendering.Location = New System.Drawing.Point(5, 21)
            Me.checkBoxExactTimeRendering.Name = "checkBoxExactTimeRendering"
            Me.checkBoxExactTimeRendering.Size = New System.Drawing.Size(123, 18)
            Me.checkBoxExactTimeRendering.TabIndex = 0
            Me.checkBoxExactTimeRendering.Text = "Exact time rendering"
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
            ' buttonBackToMonthView
            ' 
            Me.buttonBackToMonthView.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.buttonBackToMonthView.Location = New System.Drawing.Point(10, 33)
            Me.buttonBackToMonthView.Name = "buttonBackToMonthView"
            Me.buttonBackToMonthView.Size = New System.Drawing.Size(210, 24)
            Me.buttonBackToMonthView.TabIndex = 4
            Me.buttonBackToMonthView.Text = "Back to Month View"
            Me.buttonBackToMonthView.Visible = False
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radScheduler1)
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1541, 1000)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radScheduler1, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox1.ResumeLayout(False)
            Me.radGroupBox1.PerformLayout()
            DirectCast(Me.checkBoxCellsOverflow, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxAppointmentsScrolling, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxEnableWeeksHeader, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowWeeksHeader, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxVerticalScroll, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox2.ResumeLayout(False)
            Me.radGroupBox2.PerformLayout()
            Me.panel2.ResumeLayout(False)
            Me.panel2.PerformLayout()
            DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownWorkWeekEnd, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.dropDownWorkWeekStart, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorWeekCount, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowWeekend, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxShowFullMonth, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupBox3.ResumeLayout(False)
            Me.radGroupBox3.PerformLayout()
            DirectCast(Me.trackBarColumnSize, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.trackBarRowSize, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.panel1.ResumeLayout(False)
            Me.panel1.PerformLayout()
            DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.spinEditorAppointmentSpacing, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxAutoSizeAppointments, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.checkBoxExactTimeRendering, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerRulerStart, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.timePickerRulerEnd, System.ComponentModel.ISupportInitialize).EndInit()
            DirectCast(Me.buttonBackToMonthView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Friend WithEvents radGroupBox3 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
        Friend WithEvents radScheduler1 As Telerik.WinControls.UI.RadScheduler
        Friend WithEvents radLabel3 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel2 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents panel1 As System.Windows.Forms.Panel
        Friend WithEvents radLabel1 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents spinEditorAppointmentSpacing As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents checkBoxAutoSizeAppointments As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxExactTimeRendering As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxCellsOverflow As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxAppointmentsScrolling As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxEnableWeeksHeader As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxShowWeeksHeader As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxVerticalScroll As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents trackBarColumnSize As Telerik.WinControls.UI.RadTrackBar
        Friend WithEvents trackBarRowSize As Telerik.WinControls.UI.RadTrackBar
        Friend WithEvents checkBoxShowWeekend As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents checkBoxShowFullMonth As Telerik.WinControls.UI.RadCheckBox
        Friend WithEvents timePickerRulerStart As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents timePickerRulerEnd As Telerik.WinControls.UI.RadTimePicker
        Friend WithEvents panel2 As System.Windows.Forms.Panel
        Friend WithEvents radLabel6 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents spinEditorWeekCount As Telerik.WinControls.UI.RadSpinEditor
        Friend WithEvents radLabel7 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents radLabel8 As Telerik.WinControls.UI.RadLabel
        Friend WithEvents dropDownWorkWeekEnd As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents dropDownWorkWeekStart As Telerik.WinControls.UI.RadDropDownList
        Friend WithEvents buttonBackToMonthView As Telerik.WinControls.UI.RadButton
    End Class
End Namespace
