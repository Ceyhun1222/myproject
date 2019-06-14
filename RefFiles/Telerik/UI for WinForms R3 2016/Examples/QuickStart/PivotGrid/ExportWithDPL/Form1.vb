Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.PivotGrid.ExportWithDPL
    Partial Public Class Form1
        Inherits ExternalExampleHostForm

        Private ReadOnly ExternalExampleName As String = "PivotGrid"
        Public Sub New(themeName As String)
            Me.ThemeName = themeName
        End Sub
        Protected Overrides Function GetExecutablePath() As String
            Return "\..\..\ExportWithDpl\bin\ExportWithDpl.exe"
        End Function
        Protected Overrides Function GetEntryPointAsString() As String
            Return "ExportWithDpl." & ExternalExampleName & ".Form1"
        End Function
        Protected Overrides Function GetExternalProcessArguments(ByVal excutablePath As String) As String
            If String.IsNullOrEmpty(Me.ThemeName) Then
                Return String.Format("{0} {1}", ExternalExampleName, "TelerikMetro")
            Else
                Return String.Format("{0} {1}", ExternalExampleName, Me.ThemeName)
            End If
        End Function

        Protected Overrides ReadOnly Property CanOpenMultipleInstances() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace