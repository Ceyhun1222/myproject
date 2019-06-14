Imports System.ComponentModel
Imports System.Text
Imports Telerik.WinControls
Imports System.IO
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Integration.BugTracker
	Partial Public Class Form1
		Inherits ExternalProcessForm
		Protected Overrides Function GetExecutablePath() As String
			Return "\..\..\BugTracker\Bin\BugTracker.exe"
		End Function
	End Class
End Namespace
