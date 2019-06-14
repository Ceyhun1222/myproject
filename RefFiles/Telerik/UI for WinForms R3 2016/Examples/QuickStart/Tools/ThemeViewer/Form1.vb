Imports System.IO
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Tools.ThemeViewer
    Partial Public Class Form1

        Protected Overrides Function GetExecutablePath() As String
            If File.Exists(Application.StartupPath + "\..\..\..\Bin\Release\ThemeViewer.exe") Then
                Return "\..\..\..\..\Bin\Release\ThemeViewer.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\ReleaseTrial\ThemeViewer.exe") Then
                Return "\..\..\..\..\Bin\ReleaseTrial\ThemeViewer.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\Debug\ThemeViewer.exe") Then
                Return "\..\..\..\Bin\Debug\ThemeViewer.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\ThemeViewer.exe") Then
                Return "\..\..\..\Bin\ThemeViewer.exe"
            Else
                RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.[Error])
            End If

            Return String.Empty
        End Function
    End Class
End Namespace
