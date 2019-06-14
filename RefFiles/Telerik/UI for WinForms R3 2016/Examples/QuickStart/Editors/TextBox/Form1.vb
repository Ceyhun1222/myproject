Imports Telerik.Examples.WinControls.Editors.ComboBox

Namespace Telerik.Examples.WinControls.Editors.TextBox
	''' <summary>
	''' example form         
	''' </summary>
	Partial Public Class Form1
		Inherits EditorExampleBaseForm
		Public Sub New()
			InitializeComponent()
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radTxtDemo2.TextChanging, AddressOf radTextBox2_TextChanging
			AddHandler radTxtNullText.TextChanged, AddressOf radTxtNullText_TextChanged
		End Sub

		Private Sub textBox1_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
			radTxtDemo1.NullText = Me.radTxtNullText.Text
		End Sub

		Private Sub radTextBox2_TextChanging(ByVal sender As Object, ByVal e As Telerik.WinControls.TextChangingEventArgs)
			e.Cancel = Me.radCheckCancel.Checked
			Me.radLblOldValue.Text = "Old Value: " & e.OldValue
			Me.radLblNewValue.Text = "New Value: " & e.NewValue
		End Sub

		Private Sub radTxtNullText_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
			radTxtDemo1.NullText = Me.radTxtNullText.Text
			radTxtDemo2.NullText = Me.radTxtNullText.Text
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Me.radTxtDemo2.AcceptsReturn = True
		End Sub
	End Class
End Namespace