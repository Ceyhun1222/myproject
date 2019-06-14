
Namespace Telerik.Examples.WinControls.Scheduler.CustomWorkTime
	Partial Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
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
			Dim dateTimeInterval1 As New Telerik.WinControls.UI.DateTimeInterval()
			Dim schedulerDailyPrintStyle1 As New Telerik.WinControls.UI.SchedulerDailyPrintStyle()
			Me.radGroupBox1 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radDateTimePicker2 = New Telerik.WinControls.UI.RadDateTimePicker()
			Me.radLabel2 = New Telerik.WinControls.UI.RadLabel()
			Me.radDateTimePicker1 = New Telerik.WinControls.UI.RadDateTimePicker()
			Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
			Me.radScheduler1 = New Telerik.WinControls.UI.RadScheduler()
			Me.radSchedulerNavigator1 = New Telerik.WinControls.UI.RadSchedulerNavigator()
			Me.radGroupBox2 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radDropDownList2 = New Telerik.WinControls.UI.RadDropDownList()
			Me.radDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
			Me.radLabel3 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel4 = New Telerik.WinControls.UI.RadLabel()
			Me.radGroupBox3 = New Telerik.WinControls.UI.RadGroupBox()
			Me.radSpinEditor3 = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radSpinEditor4 = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel7 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel8 = New Telerik.WinControls.UI.RadLabel()
			Me.radSpinEditor2 = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radSpinEditor1 = New Telerik.WinControls.UI.RadSpinEditor()
			Me.radLabel5 = New Telerik.WinControls.UI.RadLabel()
			Me.radLabel6 = New Telerik.WinControls.UI.RadLabel()
			Me.radButton1 = New Telerik.WinControls.UI.RadButton()
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.settingsPanel.SuspendLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox1.SuspendLayout()
			DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSchedulerNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox2.SuspendLayout()
			DirectCast(Me.radDropDownList2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radGroupBox3.SuspendLayout()
			DirectCast(Me.radSpinEditor3, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditor4, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditor2, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Controls.Add(Me.radButton1)
			Me.settingsPanel.Controls.Add(Me.radGroupBox3)
			Me.settingsPanel.Controls.Add(Me.radGroupBox2)
			Me.settingsPanel.Controls.Add(Me.radGroupBox1)
			Me.settingsPanel.Location = New System.Drawing.Point(1056, 12)
			Me.settingsPanel.Size = New System.Drawing.Size(217, 1240)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox1, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox2, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radGroupBox3, 0)
			Me.settingsPanel.Controls.SetChildIndex(Me.radButton1, 0)
			' 
			' radGroupBox1
			' 
			Me.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox1.Controls.Add(Me.radDateTimePicker2)
			Me.radGroupBox1.Controls.Add(Me.radLabel2)
			Me.radGroupBox1.Controls.Add(Me.radDateTimePicker1)
			Me.radGroupBox1.Controls.Add(Me.radLabel1)
			Me.radGroupBox1.HeaderText = "WorkTime Settings"
			Me.radGroupBox1.Location = New System.Drawing.Point(10, 48)
			Me.radGroupBox1.Name = "radGroupBox1"
			Me.radGroupBox1.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
			Me.radGroupBox1.Size = New System.Drawing.Size(197, 132)
			Me.radGroupBox1.TabIndex = 5
			Me.radGroupBox1.Text = "WorkTime Settings"
			' 
			' radDateTimePicker2
			' 
			Me.radDateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDateTimePicker2.CustomFormat = "HH:mm"
			Me.radDateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.[Custom]
			Me.radDateTimePicker2.Location = New System.Drawing.Point(5, 95)
			Me.radDateTimePicker2.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
				0)
			Me.radDateTimePicker2.Name = "radDateTimePicker2"
			Me.radDateTimePicker2.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
				0)
			Me.radDateTimePicker2.ShowUpDown = True
			Me.radDateTimePicker2.Size = New System.Drawing.Size(187, 20)
			Me.radDateTimePicker2.TabIndex = 3
			Me.radDateTimePicker2.TabStop = False
			Me.radDateTimePicker2.Text = "16:45"
			Me.radDateTimePicker2.Value = New System.DateTime(2010, 1, 18, 16, 45, 7, _
				444)
			' 
			' radLabel2
			' 
			Me.radLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel2.Location = New System.Drawing.Point(5, 74)
			Me.radLabel2.Name = "radLabel2"
			Me.radLabel2.Size = New System.Drawing.Size(20, 18)
			Me.radLabel2.TabIndex = 2
			Me.radLabel2.Text = "To:"
			' 
			' radDateTimePicker1
			' 
			Me.radDateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDateTimePicker1.CustomFormat = "HH:mm"
			Me.radDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Custom]
			Me.radDateTimePicker1.Location = New System.Drawing.Point(5, 45)
			Me.radDateTimePicker1.MinDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
				0)
			Me.radDateTimePicker1.Name = "radDateTimePicker1"
			Me.radDateTimePicker1.NullDate = New System.DateTime(1900, 1, 1, 0, 0, 0, _
				0)
			Me.radDateTimePicker1.ShowUpDown = True
			Me.radDateTimePicker1.Size = New System.Drawing.Size(187, 20)
			Me.radDateTimePicker1.TabIndex = 1
			Me.radDateTimePicker1.TabStop = False
			Me.radDateTimePicker1.Text = "16:44"
			Me.radDateTimePicker1.Value = New System.DateTime(2010, 1, 18, 16, 44, 48, _
				954)
			' 
			' radLabel1
			' 
			Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel1.Location = New System.Drawing.Point(5, 24)
			Me.radLabel1.Name = "radLabel1"
			Me.radLabel1.Size = New System.Drawing.Size(33, 18)
			Me.radLabel1.TabIndex = 0
			Me.radLabel1.Text = "From:"
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
			schedulerDailyPrintStyle1.DateEndRange = New System.DateTime(2014, 6, 15, 0, 0, 0, _
				0)
			schedulerDailyPrintStyle1.DateHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold)
			schedulerDailyPrintStyle1.DateStartRange = New System.DateTime(2014, 6, 10, 0, 0, 0, _
				0)
			schedulerDailyPrintStyle1.PageHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Bold)
			Me.radScheduler1.PrintStyle = schedulerDailyPrintStyle1
			Me.radScheduler1.Size = New System.Drawing.Size(1274, 928)
			Me.radScheduler1.TabIndex = 1
			Me.radScheduler1.Text = "radScheduler1"
			' 
			' radSchedulerNavigator1
			' 
			Me.radSchedulerNavigator1.AssociatedScheduler = Nothing
			Me.radSchedulerNavigator1.DateFormat = "yyyy/MM/dd"
			Me.radSchedulerNavigator1.Dock = System.Windows.Forms.DockStyle.Top
			Me.radSchedulerNavigator1.Location = New System.Drawing.Point(0, 0)
			Me.radSchedulerNavigator1.MinimumSize = New System.Drawing.Size(400, 74)
			Me.radSchedulerNavigator1.Name = "radSchedulerNavigator1"
			Me.radSchedulerNavigator1.NavigationStepType = Telerik.WinControls.UI.NavigationStepTypes.Day
			' 
			' 
			' 
			Me.radSchedulerNavigator1.RootElement.MinSize = New System.Drawing.Size(400, 74)
			Me.radSchedulerNavigator1.RootElement.StretchVertically = False
			Me.radSchedulerNavigator1.Size = New System.Drawing.Size(1274, 77)
			Me.radSchedulerNavigator1.TabIndex = 2
			Me.radSchedulerNavigator1.Text = "radSchedulerNavigator1"
			' 
			' radGroupBox2
			' 
			Me.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox2.Controls.Add(Me.radDropDownList2)
			Me.radGroupBox2.Controls.Add(Me.radDropDownList1)
			Me.radGroupBox2.Controls.Add(Me.radLabel3)
			Me.radGroupBox2.Controls.Add(Me.radLabel4)
			Me.radGroupBox2.HeaderText = "WorkWeek Settings"
			Me.radGroupBox2.Location = New System.Drawing.Point(10, 197)
			Me.radGroupBox2.Name = "radGroupBox2"
			Me.radGroupBox2.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
			Me.radGroupBox2.Size = New System.Drawing.Size(197, 132)
			Me.radGroupBox2.TabIndex = 6
			Me.radGroupBox2.Text = "WorkWeek Settings"
			' 
			' radDropDownList2
			' 
			Me.radDropDownList2.AllowShowFocusCues = False
			Me.radDropDownList2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownList2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownList2.Location = New System.Drawing.Point(5, 99)
			Me.radDropDownList2.Name = "radDropDownList2"
			Me.radDropDownList2.Size = New System.Drawing.Size(187, 20)
			Me.radDropDownList2.TabIndex = 4
			Me.radDropDownList2.Text = "radDropDownList2"
			' 
			' radDropDownList1
			' 
			Me.radDropDownList1.AllowShowFocusCues = False
			Me.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownList1.Location = New System.Drawing.Point(5, 48)
			Me.radDropDownList1.Name = "radDropDownList1"
			Me.radDropDownList1.Size = New System.Drawing.Size(187, 20)
			Me.radDropDownList1.TabIndex = 3
			Me.radDropDownList1.Text = "radDropDownList1"
			' 
			' radLabel3
			' 
			Me.radLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel3.Location = New System.Drawing.Point(5, 74)
			Me.radLabel3.Name = "radLabel3"
			Me.radLabel3.Size = New System.Drawing.Size(20, 18)
			Me.radLabel3.TabIndex = 2
			Me.radLabel3.Text = "To:"
			' 
			' radLabel4
			' 
			Me.radLabel4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel4.Location = New System.Drawing.Point(5, 24)
			Me.radLabel4.Name = "radLabel4"
			Me.radLabel4.Size = New System.Drawing.Size(33, 18)
			Me.radLabel4.TabIndex = 0
			Me.radLabel4.Text = "From:"
			' 
			' radGroupBox3
			' 
			Me.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
			Me.radGroupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radGroupBox3.Controls.Add(Me.radSpinEditor3)
			Me.radGroupBox3.Controls.Add(Me.radSpinEditor4)
			Me.radGroupBox3.Controls.Add(Me.radLabel7)
			Me.radGroupBox3.Controls.Add(Me.radLabel8)
			Me.radGroupBox3.Controls.Add(Me.radSpinEditor2)
			Me.radGroupBox3.Controls.Add(Me.radSpinEditor1)
			Me.radGroupBox3.Controls.Add(Me.radLabel5)
			Me.radGroupBox3.Controls.Add(Me.radLabel6)
			Me.radGroupBox3.HeaderText = "Ruler Settings"
			Me.radGroupBox3.Location = New System.Drawing.Point(10, 350)
			Me.radGroupBox3.Name = "radGroupBox3"
			Me.radGroupBox3.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
			Me.radGroupBox3.Size = New System.Drawing.Size(197, 233)
			Me.radGroupBox3.TabIndex = 7
			Me.radGroupBox3.Text = "Ruler Settings"
			' 
			' radSpinEditor3
			' 
			Me.radSpinEditor3.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditor3.Location = New System.Drawing.Point(5, 197)
			Me.radSpinEditor3.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
			Me.radSpinEditor3.Name = "radSpinEditor3"
			Me.radSpinEditor3.Size = New System.Drawing.Size(187, 20)
			Me.radSpinEditor3.TabIndex = 8
			Me.radSpinEditor3.TabStop = False

			' 
			' radSpinEditor4
			' 
			Me.radSpinEditor4.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditor4.Location = New System.Drawing.Point(5, 147)
			Me.radSpinEditor4.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
			Me.radSpinEditor4.Name = "radSpinEditor4"
			Me.radSpinEditor4.Size = New System.Drawing.Size(187, 20)
			Me.radSpinEditor4.TabIndex = 7
			Me.radSpinEditor4.TabStop = False

			' 
			' radLabel7
			' 
			Me.radLabel7.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel7.Location = New System.Drawing.Point(5, 173)
			Me.radLabel7.Name = "radLabel7"
			Me.radLabel7.Size = New System.Drawing.Size(92, 18)
			Me.radLabel7.TabIndex = 6
			Me.radLabel7.Text = "EndScaleMinutes:"
			' 
			' radLabel8
			' 
			Me.radLabel8.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel8.Location = New System.Drawing.Point(5, 123)
			Me.radLabel8.Name = "radLabel8"
			Me.radLabel8.Size = New System.Drawing.Size(52, 18)
			Me.radLabel8.TabIndex = 5
			Me.radLabel8.Text = "EndScale:"
			' 
			' radSpinEditor2
			' 
			Me.radSpinEditor2.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditor2.Location = New System.Drawing.Point(5, 97)
			Me.radSpinEditor2.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
			Me.radSpinEditor2.Name = "radSpinEditor2"
			Me.radSpinEditor2.Size = New System.Drawing.Size(187, 20)
			Me.radSpinEditor2.TabIndex = 4
			Me.radSpinEditor2.TabStop = False

			' 
			' radSpinEditor1
			' 
			Me.radSpinEditor1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radSpinEditor1.Location = New System.Drawing.Point(5, 47)
			Me.radSpinEditor1.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
			Me.radSpinEditor1.Name = "radSpinEditor1"
			Me.radSpinEditor1.Size = New System.Drawing.Size(187, 20)
			Me.radSpinEditor1.TabIndex = 3
			Me.radSpinEditor1.TabStop = False

			' 
			' radLabel5
			' 
			Me.radLabel5.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel5.Location = New System.Drawing.Point(5, 73)
			Me.radLabel5.Name = "radLabel5"
			Me.radLabel5.Size = New System.Drawing.Size(97, 18)
			Me.radLabel5.TabIndex = 2
			Me.radLabel5.Text = "StartScaleMinutes:"
			' 
			' radLabel6
			' 
			Me.radLabel6.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radLabel6.Location = New System.Drawing.Point(5, 23)
			Me.radLabel6.Name = "radLabel6"
			Me.radLabel6.Size = New System.Drawing.Size(57, 18)
			Me.radLabel6.TabIndex = 0
			Me.radLabel6.Text = "StartScale:"
			' 
			' radButton1
			' 
			Me.radButton1.Anchor = System.Windows.Forms.AnchorStyles.Top
			Me.radButton1.Location = New System.Drawing.Point(10, 606)
			Me.radButton1.Name = "radButton1"
			Me.radButton1.Size = New System.Drawing.Size(197, 24)
			Me.radButton1.TabIndex = 8
			Me.radButton1.Text = "Show WorkTime Exceptions"

			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.radScheduler1)
			Me.Controls.Add(Me.radSchedulerNavigator1)
			Me.Name = "Form1"
			Me.Size = New System.Drawing.Size(1284, 1015)
			Me.Controls.SetChildIndex(Me.themePanel, 0)
			Me.Controls.SetChildIndex(Me.radSchedulerNavigator1, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			Me.Controls.SetChildIndex(Me.radScheduler1, 0)
			DirectCast(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			Me.settingsPanel.ResumeLayout(False)
			Me.settingsPanel.PerformLayout()
			DirectCast(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox1.ResumeLayout(False)
			Me.radGroupBox1.PerformLayout()
			DirectCast(Me.radDateTimePicker2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radScheduler1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSchedulerNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radGroupBox2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox2.ResumeLayout(False)
			Me.radGroupBox2.PerformLayout()
			DirectCast(Me.radDropDownList2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel3, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel4, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radGroupBox3.ResumeLayout(False)
			Me.radGroupBox3.PerformLayout()
			DirectCast(Me.radSpinEditor3, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditor4, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel7, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel8, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditor2, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radSpinEditor1, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel5, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radLabel6, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private radGroupBox1 As Telerik.WinControls.UI.RadGroupBox
		Private radDateTimePicker2 As Telerik.WinControls.UI.RadDateTimePicker
		Private radLabel2 As Telerik.WinControls.UI.RadLabel
		Private radDateTimePicker1 As Telerik.WinControls.UI.RadDateTimePicker
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private radScheduler1 As Telerik.WinControls.UI.RadScheduler
		Private radSchedulerNavigator1 As Telerik.WinControls.UI.RadSchedulerNavigator
		Private radGroupBox2 As Telerik.WinControls.UI.RadGroupBox
		Private radLabel3 As Telerik.WinControls.UI.RadLabel
		Private radLabel4 As Telerik.WinControls.UI.RadLabel
		Private radDropDownList2 As Telerik.WinControls.UI.RadDropDownList
		Private radDropDownList1 As Telerik.WinControls.UI.RadDropDownList
		Private radGroupBox3 As Telerik.WinControls.UI.RadGroupBox
		Private radSpinEditor1 As Telerik.WinControls.UI.RadSpinEditor
		Private radLabel5 As Telerik.WinControls.UI.RadLabel
		Private radLabel6 As Telerik.WinControls.UI.RadLabel
		Private radSpinEditor2 As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditor3 As Telerik.WinControls.UI.RadSpinEditor
		Private radSpinEditor4 As Telerik.WinControls.UI.RadSpinEditor
		Private radLabel7 As Telerik.WinControls.UI.RadLabel
		Private radLabel8 As Telerik.WinControls.UI.RadLabel
		Private radButton1 As Telerik.WinControls.UI.RadButton

	End Class
End Namespace
