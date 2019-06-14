Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Diagram.LinearProcess
    Partial Public Class Form1
        Inherits ExternalExampleHostForm


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
        Public Overrides ReadOnly Property ExampleName As String
            Get
                Return "LinearProcess"
            End Get
        End Property
    End Class
End Namespace