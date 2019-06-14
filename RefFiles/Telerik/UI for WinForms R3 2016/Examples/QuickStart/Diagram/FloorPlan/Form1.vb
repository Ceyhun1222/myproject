Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Diagram.FloorPlan
    Partial Public Class Form1
        Inherits ExternalExampleHostForm
        Private ReadOnly ExternalExampleName As String = "FloorPlan"

        Public Sub New()
        End Sub
        Public Sub New(themeName As String)
            Me.ThemeName = themeName
        End Sub

        Protected Overrides Function GetEntryPointAsString() As String
            Return "DiagramFirstLook.Form1"
        End Function

        Protected Overrides Function GetExecutablePath() As String
            Return "\..\..\Diagram\DiagramFirstLook\bin\DiagramFirstLook.exe"
        End Function

        Protected Overrides Function GetExternalProcessArguments(excutablePath As String) As String
            Return [String].Format("{0} {1}", If([String].IsNullOrEmpty(Me.ThemeName), "TelerikMetro", Me.ThemeName), ExternalExampleName)
        End Function

        Public Overrides ReadOnly Property ExampleName() As String
            Get
                Return ExternalExampleName
            End Get
        End Property
    End Class
End Namespace