Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Editors.ComboBox
	''' <summary>
	''' This is a base control for all RadComboBox examples.
	''' </summary>
	Partial Public Class EditorExampleBaseForm
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			Me.radPanelDemoHolder.PanelElement.PanelFill.Visibility = ElementVisibility.Collapsed
			Me.radPanelDemoHolder.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed

			Me.AutoScaleMode = AutoScaleMode.None
		End Sub

		''' <summary>
		''' Resets the location of the panel that holds the example so that it
		''' always resides in the middle of the available space.
		''' </summary>
		''' <param name="e"></param>
		Protected Overrides Sub OnResize(ByVal e As EventArgs)
			If Me.radPanelDemoHolder IsNot Nothing Then
				'float xCoord = ((float)(this.Width - this.radPanelDemoHolder.Width)) / 2;
				'float yCoord = ((float)(this.Height - this.radPanelDemoHolder.Height)) / 2;

				Me.radPanelDemoHolder.Location = Point.Empty
			End If

			MyBase.OnResize(e)
		End Sub
	End Class
End Namespace
