Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports System.Management

Namespace Telerik.Examples.WinControls.RichTextEditor.DocumentProtection
	Public Partial Class Form1
        Inherits ExternalExampleHostForm
        Public Sub New(themeName As String)
            Me.ThemeName = themeName
        End Sub
		Private ReadOnly ExternalExampleName As String = "DocumentProtection"
        Protected Overrides Function GetEntryPointAsString() As String
            Return "RichTextEditor." & ExternalExampleName & ".Form1"
        End Function

		Protected Overrides Function GetExecutablePath() As String
			Return "\..\..\RichTextEditor\bin\RichTextEditor.exe"
		End Function

		Protected Overrides Function GetExternalProcessArguments(ByVal excutablePath As String) As String
			If String.IsNullOrEmpty(Me.ThemeName) Then
				Return String.Format("{0} {1}", ExternalExampleName,"TelerikMetro")
			Else
				Return String.Format("{0} {1}", ExternalExampleName,Me.ThemeName)
			End If
		End Function

		Protected Overrides ReadOnly Property CanOpenMultipleInstances() As Boolean
			Get
				Return True
			End Get
		End Property
	End Class
End Namespace
