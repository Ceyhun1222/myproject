Imports Telerik.Examples.WinControls.Editors.ComboBox

Namespace Telerik.Examples.WinControls.Buttons.RepeatButton
	''' <summary>
	''' Main class for the repeat button example
	''' </summary>
	Partial Public Class Form1
		Inherits EditorExampleBaseForm
		Public Sub New()
			InitializeComponent()

			Me.radProgressBar1.Text = ""
		End Sub

		Private Sub radRepeatButton3_ButtonClick(ByVal sender As Object, ByVal e As EventArgs)
			If Me.radProgressBar1.Value1 < Me.radProgressBar1.Maximum Then
				Me.radProgressBar1.Value1 += 1
			Else
				Me.radProgressBar1.Value1 = Me.radProgressBar1.Minimum
			End If
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radRepeatButton3.ButtonClick, AddressOf radRepeatButton3_ButtonClick
		End Sub
	End Class
End Namespace