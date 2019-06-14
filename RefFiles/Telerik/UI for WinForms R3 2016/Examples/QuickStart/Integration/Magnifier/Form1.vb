Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.Integration.Magnifier
	Partial Public Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			FillMagnifierWithButtons()

			Me.radPanel1.Text = ""
			radToggleButton1.ToggleState = ToggleState.On
		End Sub

		Protected Overrides Function GetExampleDefaultTheme() As String
			Return "OfficeGlass"
		End Function

		Private Sub FillMagnifierWithButtons()
			For i As Integer = 0 To 48
				Dim button As New RadButtonElement(i.ToString())

				button.StretchHorizontally = False
				button.StretchVertically = False
				button.Alignment = ContentAlignment.MiddleCenter
				button.TextAlignment = ContentAlignment.MiddleCenter
				button.TextElement.Parent.Alignment = ContentAlignment.MiddleCenter
				button.MinSize = New Size(33, 33)
				Me.magnifier1.Items.Add(button)
			Next i
		End Sub

		Private Sub radToggleButton1_ToggleStateChanged(ByVal sender As Object, ByVal args As StateChangedEventArgs)
			Dim rand As New Random()

			For Each item As RadItem In Me.magnifier1.Items
				If args.ToggleState = ToggleState.On Then
					item.Opacity = CDbl(rand.Next(100) + 10) / 100R
				Else
					item.Opacity = 1
				End If
			Next item
		End Sub

		Private Sub radSpinEditor1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
			Me.magnifier1.ZoomFactor = CSng(Me.radSpinEditor1.Value)
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radToggleButton1.ToggleStateChanged, AddressOf radToggleButton1_ToggleStateChanged
			AddHandler radSpinEditor1.ValueChanged, AddressOf radSpinEditor1_ValueChanged
		End Sub
	End Class
End Namespace
