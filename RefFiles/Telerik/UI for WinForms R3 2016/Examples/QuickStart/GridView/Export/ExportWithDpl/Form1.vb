﻿Imports System
Imports System.Linq
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.GridView.Export.ExportWithDpl
    Partial Public Class Form1
        Inherits ExternalExampleHostForm
        Private ReadOnly ExternalExampleName As String = "GridView"
        Public Sub New(themeName As String)
            Me.ThemeName = themeName
        End Sub
        Protected Overrides Function GetExecutablePath() As String
            Return "\..\..\ExportWithDpl\bin\ExportWithDpl.exe"
        End Function
        Protected Overrides Function GetEntryPointAsString() As String
            Return "ExportWithDpl." & ExternalExampleName & ".Form1"
        End Function
        Protected Overrides Function GetExternalProcessArguments(excutablePath As String) As String
            Return [String].Format("{0} {1}", ExternalExampleName, If([String].IsNullOrEmpty(Me.ThemeName), "TelerikMetro", Me.ThemeName))
        End Function

        Protected Overrides ReadOnly Property CanOpenMultipleInstances() As Boolean
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace