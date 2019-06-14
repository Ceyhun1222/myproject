Namespace Telerik.Examples.WinControls.Buttons.ToggleSwitch
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

#Region "Component Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Dim RadListDataItem1 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem2 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem3 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Dim RadListDataItem4 As Telerik.WinControls.UI.RadListDataItem = New Telerik.WinControls.UI.RadListDataItem()
            Me.radLabelNewAppointment = New Telerik.WinControls.UI.RadLabel()
            Me.radTextBoxControlNewAppointment = New Telerik.WinControls.UI.RadTextBoxControl()
            Me.radLabelReminder = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelReminderDate = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelReminderTime = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelRecurrence = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelRecurrenceDate = New Telerik.WinControls.UI.RadLabel()
            Me.radLabelPlaySound = New Telerik.WinControls.UI.RadLabel()
            Me.radToggleSwitchReminder = New Telerik.WinControls.UI.RadToggleSwitch()
            Me.radToggleSwitchRecurrence = New Telerik.WinControls.UI.RadToggleSwitch()
            Me.radToggleSwitchPlaySound = New Telerik.WinControls.UI.RadToggleSwitch()
            Me.radDateTimePicker1 = New Telerik.WinControls.UI.RadDateTimePicker()
            Me.radTimePicker1 = New Telerik.WinControls.UI.RadTimePicker()
            Me.radDropDownListRecurrence = New Telerik.WinControls.UI.RadDropDownList()
            Me.radButtonCreateAppointment = New Telerik.WinControls.UI.RadButton()
            Me.radCheckBoxAllowAnimation = New Telerik.WinControls.UI.RadCheckBox()
            Me.radCheckBoxRightToLeft = New Telerik.WinControls.UI.RadCheckBox()
            Me.radLabelSwitchElasticity = New Telerik.WinControls.UI.RadLabel()
            Me.radSpinEditorSwitchElasticity = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabelThumbTickness = New Telerik.WinControls.UI.RadLabel()
            Me.radSpinEditorThumbTickness = New Telerik.WinControls.UI.RadSpinEditor()
            Me.radLabelAnimationFrames = New Telerik.WinControls.UI.RadLabel()
            Me.radSpinEditorAnimationFrames = New Telerik.WinControls.UI.RadSpinEditor()
            CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanelDemoHolder.SuspendLayout()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelNewAppointment, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTextBoxControlNewAppointment, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelReminder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelReminderDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelReminderTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelRecurrence, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelRecurrenceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelPlaySound, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radToggleSwitchReminder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radToggleSwitchRecurrence, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radToggleSwitchPlaySound, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownListRecurrence, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radButtonCreateAppointment, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxAllowAnimation, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radCheckBoxRightToLeft, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelSwitchElasticity, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorSwitchElasticity, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelThumbTickness, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorThumbTickness, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabelAnimationFrames, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radSpinEditorAnimationFrames, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'radPanelDemoHolder
            '
            Me.radPanelDemoHolder.Controls.Add(Me.radButtonCreateAppointment)
            Me.radPanelDemoHolder.Controls.Add(Me.radDropDownListRecurrence)
            Me.radPanelDemoHolder.Controls.Add(Me.radTimePicker1)
            Me.radPanelDemoHolder.Controls.Add(Me.radDateTimePicker1)
            Me.radPanelDemoHolder.Controls.Add(Me.radToggleSwitchPlaySound)
            Me.radPanelDemoHolder.Controls.Add(Me.radToggleSwitchRecurrence)
            Me.radPanelDemoHolder.Controls.Add(Me.radToggleSwitchReminder)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelPlaySound)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelRecurrenceDate)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelRecurrence)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelReminderTime)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelReminderDate)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelReminder)
            Me.radPanelDemoHolder.Controls.Add(Me.radTextBoxControlNewAppointment)
            Me.radPanelDemoHolder.Controls.Add(Me.radLabelNewAppointment)
            Me.radPanelDemoHolder.Size = New System.Drawing.Size(377, 451)
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radSpinEditorAnimationFrames)
            Me.settingsPanel.Controls.Add(Me.radLabelAnimationFrames)
            Me.settingsPanel.Controls.Add(Me.radSpinEditorThumbTickness)
            Me.settingsPanel.Controls.Add(Me.radLabelThumbTickness)
            Me.settingsPanel.Controls.Add(Me.radSpinEditorSwitchElasticity)
            Me.settingsPanel.Controls.Add(Me.radLabelSwitchElasticity)
            Me.settingsPanel.Controls.Add(Me.radCheckBoxRightToLeft)
            Me.settingsPanel.Controls.Add(Me.radCheckBoxAllowAnimation)
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxAllowAnimation, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radCheckBoxRightToLeft, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabelSwitchElasticity, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorSwitchElasticity, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabelThumbTickness, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorThumbTickness, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabelAnimationFrames, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radSpinEditorAnimationFrames, 0)
            '
            'themePanel
            '
            Me.themePanel.Location = New System.Drawing.Point(922, 1)
            Me.themePanel.Size = New System.Drawing.Size(230, 598)
            '
            'radLabelNewAppointment
            '
            Me.radLabelNewAppointment.Font = New System.Drawing.Font("Segoe UI", 14.0!)
            Me.radLabelNewAppointment.Location = New System.Drawing.Point(-5, 24)
            Me.radLabelNewAppointment.Name = "radLabelNewAppointment"
            Me.radLabelNewAppointment.Size = New System.Drawing.Size(162, 29)
            Me.radLabelNewAppointment.TabIndex = 0
            Me.radLabelNewAppointment.Text = "New appointment"
            '
            'radTextBoxControlNewAppointment
            '
            Me.radTextBoxControlNewAppointment.Location = New System.Drawing.Point(1, 67)
            Me.radTextBoxControlNewAppointment.Name = "radTextBoxControlNewAppointment"
            Me.radTextBoxControlNewAppointment.Size = New System.Drawing.Size(301, 20)
            Me.radTextBoxControlNewAppointment.TabIndex = 1
            Me.radTextBoxControlNewAppointment.Text = "Name"
            '
            'radLabelReminder
            '
            Me.radLabelReminder.Font = New System.Drawing.Font("Segoe UI", 10.25!)
            Me.radLabelReminder.Location = New System.Drawing.Point(-3, 103)
            Me.radLabelReminder.Name = "radLabelReminder"
            Me.radLabelReminder.Size = New System.Drawing.Size(67, 22)
            Me.radLabelReminder.TabIndex = 2
            Me.radLabelReminder.Text = "Reminder"
            '
            'radLabelReminderDate
            '
            Me.radLabelReminderDate.Font = New System.Drawing.Font("Segoe UI", 8.75!)
            Me.radLabelReminderDate.Location = New System.Drawing.Point(-3, 137)
            Me.radLabelReminderDate.Name = "radLabelReminderDate"
            Me.radLabelReminderDate.Size = New System.Drawing.Size(31, 19)
            Me.radLabelReminderDate.TabIndex = 3
            Me.radLabelReminderDate.Text = "Date"
            '
            'radLabelReminderTime
            '
            Me.radLabelReminderTime.Font = New System.Drawing.Font("Segoe UI", 8.75!)
            Me.radLabelReminderTime.Location = New System.Drawing.Point(-2, 187)
            Me.radLabelReminderTime.Name = "radLabelReminderTime"
            Me.radLabelReminderTime.Size = New System.Drawing.Size(32, 19)
            Me.radLabelReminderTime.TabIndex = 4
            Me.radLabelReminderTime.Text = "Time"
            '
            'radLabelRecurrence
            '
            Me.radLabelRecurrence.Font = New System.Drawing.Font("Segoe UI", 10.25!)
            Me.radLabelRecurrence.Location = New System.Drawing.Point(-3, 249)
            Me.radLabelRecurrence.Name = "radLabelRecurrence"
            Me.radLabelRecurrence.Size = New System.Drawing.Size(76, 22)
            Me.radLabelRecurrence.TabIndex = 5
            Me.radLabelRecurrence.Text = "Recurrence"
            '
            'radLabelRecurrenceDate
            '
            Me.radLabelRecurrenceDate.Font = New System.Drawing.Font("Segoe UI", 8.75!)
            Me.radLabelRecurrenceDate.Location = New System.Drawing.Point(-3, 277)
            Me.radLabelRecurrenceDate.Name = "radLabelRecurrenceDate"
            Me.radLabelRecurrenceDate.Size = New System.Drawing.Size(31, 19)
            Me.radLabelRecurrenceDate.TabIndex = 6
            Me.radLabelRecurrenceDate.Text = "Date"
            '
            'radLabelPlaySound
            '
            Me.radLabelPlaySound.Font = New System.Drawing.Font("Segoe UI", 10.25!)
            Me.radLabelPlaySound.Location = New System.Drawing.Point(-3, 337)
            Me.radLabelPlaySound.Name = "radLabelPlaySound"
            Me.radLabelPlaySound.Size = New System.Drawing.Size(75, 22)
            Me.radLabelPlaySound.TabIndex = 7
            Me.radLabelPlaySound.Text = "Play sound"
            '
            'radToggleSwitchReminder
            '
            Me.radToggleSwitchReminder.Location = New System.Drawing.Point(252, 105)
            Me.radToggleSwitchReminder.Name = "radToggleSwitchReminder"
            Me.radToggleSwitchReminder.Size = New System.Drawing.Size(50, 20)
            Me.radToggleSwitchReminder.TabIndex = 8
            Me.radToggleSwitchReminder.Text = "radToggleSwitch1"
            '
            'radToggleSwitchRecurrence
            '
            Me.radToggleSwitchRecurrence.Location = New System.Drawing.Point(252, 251)
            Me.radToggleSwitchRecurrence.Name = "radToggleSwitchRecurrence"
            Me.radToggleSwitchRecurrence.Size = New System.Drawing.Size(50, 20)
            Me.radToggleSwitchRecurrence.TabIndex = 9
            Me.radToggleSwitchRecurrence.Text = "radToggleSwitch2"
            '
            'radToggleSwitchPlaySound
            '
            Me.radToggleSwitchPlaySound.Location = New System.Drawing.Point(252, 339)
            Me.radToggleSwitchPlaySound.Name = "radToggleSwitchPlaySound"
            Me.radToggleSwitchPlaySound.Size = New System.Drawing.Size(50, 20)
            Me.radToggleSwitchPlaySound.TabIndex = 10
            Me.radToggleSwitchPlaySound.Text = "radToggleSwitch3"
            Me.radToggleSwitchPlaySound.Value = False
            '
            'radDateTimePicker1
            '
            Me.radDateTimePicker1.CustomFormat = "dd\/MM\/yyyy"
            Me.radDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.radDateTimePicker1.Location = New System.Drawing.Point(1, 158)
            Me.radDateTimePicker1.Name = "radDateTimePicker1"
            Me.radDateTimePicker1.Size = New System.Drawing.Size(301, 20)
            Me.radDateTimePicker1.TabIndex = 11
            Me.radDateTimePicker1.TabStop = False
            Me.radDateTimePicker1.Text = "11/06/2015"
            Me.radDateTimePicker1.Value = New Date(2015, 6, 11, 11, 15, 0, 0)
            '
            'radTimePicker1
            '
            Me.radTimePicker1.Location = New System.Drawing.Point(1, 208)
            Me.radTimePicker1.MaxValue = New Date(9999, 12, 31, 23, 59, 59, 0)
            Me.radTimePicker1.MinValue = New Date(CType(0, Long))
            Me.radTimePicker1.Name = "radTimePicker1"
            Me.radTimePicker1.Size = New System.Drawing.Size(301, 20)
            Me.radTimePicker1.TabIndex = 12
            Me.radTimePicker1.TabStop = False
            Me.radTimePicker1.Text = "radTimePicker1"
            Me.radTimePicker1.Value = New Date(2015, 6, 11, 11, 15, 0, 0)
            '
            'radDropDownListRecurrence
            '
            RadListDataItem1.Text = "Every hour"
            RadListDataItem2.Selected = True
            RadListDataItem2.Text = "Every day"
            RadListDataItem3.Text = "Every week"
            RadListDataItem4.Text = "Every month"
            Me.radDropDownListRecurrence.Items.Add(RadListDataItem1)
            Me.radDropDownListRecurrence.Items.Add(RadListDataItem2)
            Me.radDropDownListRecurrence.Items.Add(RadListDataItem3)
            Me.radDropDownListRecurrence.Items.Add(RadListDataItem4)
            Me.radDropDownListRecurrence.Location = New System.Drawing.Point(1, 298)
            Me.radDropDownListRecurrence.Name = "radDropDownListRecurrence"
            Me.radDropDownListRecurrence.Size = New System.Drawing.Size(301, 20)
            Me.radDropDownListRecurrence.TabIndex = 13
            Me.radDropDownListRecurrence.Text = "Every day"
            '
            'radButtonCreateAppointment
            '
            Me.radButtonCreateAppointment.Location = New System.Drawing.Point(0, 384)
            Me.radButtonCreateAppointment.Name = "radButtonCreateAppointment"
            Me.radButtonCreateAppointment.Size = New System.Drawing.Size(110, 24)
            Me.radButtonCreateAppointment.TabIndex = 14
            Me.radButtonCreateAppointment.Text = "Create"
            '
            'radCheckBoxAllowAnimation
            '
            Me.radCheckBoxAllowAnimation.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxAllowAnimation.CheckState = System.Windows.Forms.CheckState.Checked
            Me.radCheckBoxAllowAnimation.Location = New System.Drawing.Point(10, 34)
            Me.radCheckBoxAllowAnimation.Name = "radCheckBoxAllowAnimation"
            Me.radCheckBoxAllowAnimation.Size = New System.Drawing.Size(103, 18)
            Me.radCheckBoxAllowAnimation.TabIndex = 1
            Me.radCheckBoxAllowAnimation.Text = "Allow Animation"
            Me.radCheckBoxAllowAnimation.ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
            '
            'radCheckBoxRightToLeft
            '
            Me.radCheckBoxRightToLeft.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radCheckBoxRightToLeft.Location = New System.Drawing.Point(10, 59)
            Me.radCheckBoxRightToLeft.Name = "radCheckBoxRightToLeft"
            Me.radCheckBoxRightToLeft.Size = New System.Drawing.Size(84, 18)
            Me.radCheckBoxRightToLeft.TabIndex = 2
            Me.radCheckBoxRightToLeft.Text = "Right To Left"
            '
            'radLabelSwitchElasticity
            '
            Me.radLabelSwitchElasticity.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelSwitchElasticity.Location = New System.Drawing.Point(10, 84)
            Me.radLabelSwitchElasticity.Name = "radLabelSwitchElasticity"
            Me.radLabelSwitchElasticity.Size = New System.Drawing.Size(84, 18)
            Me.radLabelSwitchElasticity.TabIndex = 3
            Me.radLabelSwitchElasticity.Text = "Switch Elasticity"
            '
            'radSpinEditorSwitchElasticity
            '
            Me.radSpinEditorSwitchElasticity.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorSwitchElasticity.DecimalPlaces = 2
            Me.radSpinEditorSwitchElasticity.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
            Me.radSpinEditorSwitchElasticity.Location = New System.Drawing.Point(10, 102)
            Me.radSpinEditorSwitchElasticity.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.radSpinEditorSwitchElasticity.Name = "radSpinEditorSwitchElasticity"
            Me.radSpinEditorSwitchElasticity.Size = New System.Drawing.Size(180, 20)
            Me.radSpinEditorSwitchElasticity.TabIndex = 4
            Me.radSpinEditorSwitchElasticity.TabStop = False
            Me.radSpinEditorSwitchElasticity.ThousandsSeparator = True
            Me.radSpinEditorSwitchElasticity.Value = New Decimal(New Integer() {5, 0, 0, 65536})
            '
            'radLabelThumbTickness
            '
            Me.radLabelThumbTickness.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelThumbTickness.Location = New System.Drawing.Point(10, 129)
            Me.radLabelThumbTickness.Name = "radLabelThumbTickness"
            Me.radLabelThumbTickness.Size = New System.Drawing.Size(85, 18)
            Me.radLabelThumbTickness.TabIndex = 5
            Me.radLabelThumbTickness.Text = "Thumb Tickness"
            '
            'radSpinEditorThumbTickness
            '
            Me.radSpinEditorThumbTickness.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorThumbTickness.Location = New System.Drawing.Point(10, 147)
            Me.radSpinEditorThumbTickness.Maximum = New Decimal(New Integer() {35, 0, 0, 0})
            Me.radSpinEditorThumbTickness.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.radSpinEditorThumbTickness.Name = "radSpinEditorThumbTickness"
            Me.radSpinEditorThumbTickness.Size = New System.Drawing.Size(180, 20)
            Me.radSpinEditorThumbTickness.TabIndex = 1
            Me.radSpinEditorThumbTickness.TabStop = False
            Me.radSpinEditorThumbTickness.Value = New Decimal(New Integer() {20, 0, 0, 0})
            '
            'radLabelAnimationFrames
            '
            Me.radLabelAnimationFrames.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabelAnimationFrames.Location = New System.Drawing.Point(10, 174)
            Me.radLabelAnimationFrames.Name = "radLabelAnimationFrames"
            Me.radLabelAnimationFrames.Size = New System.Drawing.Size(96, 18)
            Me.radLabelAnimationFrames.TabIndex = 6
            Me.radLabelAnimationFrames.Text = "Animation Frames"
            '
            'radSpinEditorAnimationFrames
            '
            Me.radSpinEditorAnimationFrames.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radSpinEditorAnimationFrames.Location = New System.Drawing.Point(10, 193)
            Me.radSpinEditorAnimationFrames.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.radSpinEditorAnimationFrames.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
            Me.radSpinEditorAnimationFrames.Name = "radSpinEditorAnimationFrames"
            Me.radSpinEditorAnimationFrames.Size = New System.Drawing.Size(180, 20)
            Me.radSpinEditorAnimationFrames.TabIndex = 7
            Me.radSpinEditorAnimationFrames.TabStop = False
            Me.radSpinEditorAnimationFrames.Value = New Decimal(New Integer() {20, 0, 0, 0})
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1513, 917)
            CType(Me.radPanelDemoHolder, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanelDemoHolder.ResumeLayout(False)
            Me.radPanelDemoHolder.PerformLayout()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelNewAppointment, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTextBoxControlNewAppointment, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelReminder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelReminderDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelReminderTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelRecurrence, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelRecurrenceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelPlaySound, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radToggleSwitchReminder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radToggleSwitchRecurrence, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radToggleSwitchPlaySound, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDateTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radTimePicker1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownListRecurrence, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radButtonCreateAppointment, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxAllowAnimation, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radCheckBoxRightToLeft, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelSwitchElasticity, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorSwitchElasticity, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelThumbTickness, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorThumbTickness, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabelAnimationFrames, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radSpinEditorAnimationFrames, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radDropDownListRecurrence As Telerik.WinControls.UI.RadDropDownList
        Private radTimePicker1 As Telerik.WinControls.UI.RadTimePicker
        Private radDateTimePicker1 As Telerik.WinControls.UI.RadDateTimePicker
        Private radToggleSwitchPlaySound As Telerik.WinControls.UI.RadToggleSwitch
        Private radToggleSwitchRecurrence As Telerik.WinControls.UI.RadToggleSwitch
        Private radToggleSwitchReminder As Telerik.WinControls.UI.RadToggleSwitch
        Private radLabelPlaySound As Telerik.WinControls.UI.RadLabel
        Private radLabelRecurrenceDate As Telerik.WinControls.UI.RadLabel
        Private radLabelRecurrence As Telerik.WinControls.UI.RadLabel
        Private radLabelReminderTime As Telerik.WinControls.UI.RadLabel
        Private radLabelReminderDate As Telerik.WinControls.UI.RadLabel
        Private radLabelReminder As Telerik.WinControls.UI.RadLabel
        Private radTextBoxControlNewAppointment As Telerik.WinControls.UI.RadTextBoxControl
        Private radLabelNewAppointment As Telerik.WinControls.UI.RadLabel
        Private radButtonCreateAppointment As Telerik.WinControls.UI.RadButton
        Private radCheckBoxRightToLeft As Telerik.WinControls.UI.RadCheckBox
        Private radCheckBoxAllowAnimation As Telerik.WinControls.UI.RadCheckBox
        Private radLabelSwitchElasticity As Telerik.WinControls.UI.RadLabel
        Private radSpinEditorSwitchElasticity As Telerik.WinControls.UI.RadSpinEditor
        Private radSpinEditorThumbTickness As Telerik.WinControls.UI.RadSpinEditor
        Private radLabelThumbTickness As Telerik.WinControls.UI.RadLabel
        Private radSpinEditorAnimationFrames As Telerik.WinControls.UI.RadSpinEditor
        Private radLabelAnimationFrames As Telerik.WinControls.UI.RadLabel

    End Class
End Namespace
