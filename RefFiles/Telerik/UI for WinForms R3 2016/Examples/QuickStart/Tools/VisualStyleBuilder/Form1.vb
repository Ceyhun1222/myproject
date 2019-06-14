Imports System.IO
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Tools.VisualStyleBuilder
    Public Class Form1

        Protected Overrides Function GetExecutablePath() As String
            If File.Exists(Application.StartupPath + "\..\..\..\Bin\Release\VisualStyleBuilder.exe") Then
                Return "\..\..\..\..\Bin\Release\VisualStyleBuilder.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe") Then
                Return "\..\..\..\..\Bin\ReleaseTrial\VisualStyleBuilder.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\Debug\VisualStyleBuilder.exe") Then
                Return "\..\..\..\Bin\Debug\VisualStyleBuilder.exe"
            ElseIf File.Exists(Application.StartupPath + "\..\..\..\Bin\VisualStyleBuilder.exe") Then
                Return "\..\..\..\Bin\VisualStyleBuilder.exe"
            Else
                RadMessageBox.Show("Could not locate executable!", "Error!", MessageBoxButtons.OK, RadMessageIcon.[Error])
            End If

            Return String.Empty
        End Function
    End Class
End Namespace
