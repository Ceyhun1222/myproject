Imports Telerik.Examples.WinControls.Editors.ComboBox
Imports Telerik.WinControls
Imports Telerik.WinControls.Enumerations

Namespace Telerik.Examples.WinControls.Buttons.ToggleSwitch
    Partial Public Class Form1
        Inherits EditorExampleBaseForm
        Public Sub New()
            InitializeComponent()
            Me.radTimePicker1.TimePickerElement.ShowSpinButtons = False
            AddHandler Me.radToggleSwitchReminder.ValueChanged, AddressOf Me.radToggleSwitchReminder_ValueChanged
            AddHandler Me.radToggleSwitchRecurrence.ValueChanged, AddressOf Me.radToggleSwitchRecurrence_ValueChanged
            AddHandler Me.radSpinEditorSwitchElasticity.ValueChanged, AddressOf Me.radSpinEditorSwitchElasticity_ValueChanged
            AddHandler Me.radSpinEditorThumbTickness.ValueChanged, AddressOf Me.radSpinEditorThumbTickness_ValueChanged
            AddHandler Me.radCheckBoxAllowAnimation.ToggleStateChanged, AddressOf Me.radCheckBoxAllowAnimation_ToggleStateChanged
            AddHandler Me.radCheckBoxRightToLeft.ToggleStateChanged, AddressOf Me.radCheckBoxRightToLeft_ToggleStateChanged
            AddHandler Me.radButtonCreateAppointment.Click, AddressOf Me.radButtonCreateAppointment_Click
            AddHandler Me.radSpinEditorAnimationFrames.ValueChanged, AddressOf Me.radSpinEditorAnimationFrames_ValueChanged
        End Sub

        Private Sub radToggleSwitchReminder_ValueChanged(sender As Object, e As EventArgs)
            Me.radDateTimePicker1.Enabled = Me.radToggleSwitchReminder.Value
            Me.radTimePicker1.Enabled = Me.radToggleSwitchReminder.Value
        End Sub

        Private Sub radToggleSwitchRecurrence_ValueChanged(sender As Object, e As EventArgs)
            Me.radDropDownListRecurrence.Enabled = Me.radToggleSwitchRecurrence.Value
        End Sub

        Private Sub radSpinEditorSwitchElasticity_ValueChanged(sender As Object, e As EventArgs)
            Me.radToggleSwitchReminder.SwitchElasticity = CDbl(Me.radSpinEditorSwitchElasticity.Value)
            Me.radToggleSwitchRecurrence.SwitchElasticity = CDbl(Me.radSpinEditorSwitchElasticity.Value)
            Me.radToggleSwitchPlaySound.SwitchElasticity = CDbl(Me.radSpinEditorSwitchElasticity.Value)
        End Sub

        Private Sub radSpinEditorThumbTickness_ValueChanged(sender As Object, e As EventArgs)
            Me.radToggleSwitchReminder.ThumbTickness = CInt(Me.radSpinEditorThumbTickness.Value)
            Me.radToggleSwitchRecurrence.ThumbTickness = CInt(Me.radSpinEditorThumbTickness.Value)
            Me.radToggleSwitchPlaySound.ThumbTickness = CInt(Me.radSpinEditorThumbTickness.Value)
        End Sub

        Private Sub radCheckBoxAllowAnimation_ToggleStateChanged(sender As Object, args As UI.StateChangedEventArgs)
            Me.radToggleSwitchReminder.AllowAnimation = Me.radCheckBoxAllowAnimation.ToggleState = ToggleState.[On]
            Me.radToggleSwitchRecurrence.AllowAnimation = Me.radCheckBoxAllowAnimation.ToggleState = ToggleState.[On]
            Me.radToggleSwitchPlaySound.AllowAnimation = Me.radCheckBoxAllowAnimation.ToggleState = ToggleState.[On]
        End Sub

        Private Sub radCheckBoxRightToLeft_ToggleStateChanged(sender As Object, args As UI.StateChangedEventArgs)
            Me.radToggleSwitchReminder.RightToLeft = If(Me.radCheckBoxRightToLeft.ToggleState = ToggleState.[On], System.Windows.Forms.RightToLeft.Yes, System.Windows.Forms.RightToLeft.No)
            Me.radToggleSwitchRecurrence.RightToLeft = If(Me.radCheckBoxRightToLeft.ToggleState = ToggleState.[On], System.Windows.Forms.RightToLeft.Yes, System.Windows.Forms.RightToLeft.No)
            Me.radToggleSwitchPlaySound.RightToLeft = If(Me.radCheckBoxRightToLeft.ToggleState = ToggleState.[On], System.Windows.Forms.RightToLeft.Yes, System.Windows.Forms.RightToLeft.No)
        End Sub

        Private Sub radButtonCreateAppointment_Click(sender As Object, e As EventArgs)
            RadMessageBox.Instance.ThemeName = Me.CurrentThemeName
            RadMessageBox.Show("Appointment:" + Environment.NewLine + radTextBoxControlNewAppointment.Text + Environment.NewLine + "Successfully created.")
        End Sub

        Private Sub radSpinEditorAnimationFrames_ValueChanged(sender As Object, e As EventArgs)
            Me.radToggleSwitchReminder.AnimationFrames = CInt(Me.radSpinEditorAnimationFrames.Value)
            Me.radToggleSwitchRecurrence.AnimationFrames = CInt(Me.radSpinEditorAnimationFrames.Value)
            Me.radToggleSwitchPlaySound.AnimationFrames = CInt(Me.radSpinEditorAnimationFrames.Value)
        End Sub

    End Class
End Namespace