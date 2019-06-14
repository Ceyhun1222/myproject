Imports System.ComponentModel
Imports System.Text
Imports System.IO
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Integration.MovieLab
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			Dim executablePath As String = Application.StartupPath & "\..\..\MovieLab\Bin\MovieLab.exe"
			If File.Exists(executablePath) Then
				Dim proc As New ProcessStartInfo(executablePath)
				proc.WorkingDirectory = Path.GetDirectoryName(executablePath)
				Process.Start(proc)
			Else
				RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.Error)
			End If
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			Me.Close()
		End Sub
	End Class
End Namespace
