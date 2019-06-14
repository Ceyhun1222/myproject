Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Diagnostics
Imports Telerik.WinControls
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Editors.SpellChecker
    Partial Public Class Form1
        Inherits ExternalProcessForm
        Protected Overrides Function GetExecutablePath() As String
            Return "\..\..\SpellCheckAsYouType\Bin\SpellCheckAsYouType.exe"


        End Function
    End Class
End Namespace