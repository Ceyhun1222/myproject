Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.DocumentsProcessing.WordConvertion
    Partial Public Class Form1
        Inherits ExternalExampleHostForm
        Public Sub New(themeName As String)
            Me.ThemeName = themeName
        End Sub

        Protected Overrides Function GetExecutablePath() As String
            Return "\..\..\DocumentsProcessing\WordConvertion\bin\WordConvertion.exe"
        End Function

    End Class
End Namespace