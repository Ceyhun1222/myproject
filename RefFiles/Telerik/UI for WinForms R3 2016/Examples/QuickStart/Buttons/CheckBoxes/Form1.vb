Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.Examples.WinControls.Editors.ComboBox

Namespace Telerik.Examples.WinControls.Buttons.CheckBoxes
	Partial Public Class Form1
		Inherits EditorExampleBaseForm
		Public Sub New()
			InitializeComponent()

			Me.radCheckBox1.Font = New Font(New FontFamily("Arial"), 10.0f, GraphicsUnit.Point)
			Me.radCheckBox2.Font = New Font(New FontFamily("Arial"), 12.0f, GraphicsUnit.Point)
			Me.radCheckBox3.Font = New Font(New FontFamily("Arial"), 14.0f, GraphicsUnit.Point)
		End Sub


		Protected Overrides Sub WireEvents()
			AddHandler radCheckBox3.ToggleStateChanged, AddressOf radCheckBox1_ToggleStateChanged
			AddHandler radCheckBox2.ToggleStateChanged, AddressOf radCheckBox1_ToggleStateChanged
			AddHandler radCheckBox1.ToggleStateChanged, AddressOf radCheckBox1_ToggleStateChanged
		End Sub

		Private Sub radCheckBox1_ToggleStateChanged(ByVal sender As Object, ByVal args As Telerik.WinControls.UI.StateChangedEventArgs)
			Me.radTextBoxEvents.Text += String.Format("{0} toggled" & Environment.NewLine, (TryCast(sender, RadCheckBox)).Text)
			Me.radTextBoxEvents.SelectionStart = Me.radTextBoxEvents.Text.Length
			Me.radTextBoxEvents.ScrollToCaret()
		End Sub


	End Class
End Namespace