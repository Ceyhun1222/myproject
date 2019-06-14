Imports Telerik.WinControls.UI
Imports System.Threading
Imports System.IO
Imports System.Text

Namespace Telerik.QuickStart.WinControls
    Friend NotInheritable Class Program
        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        Private Sub New()
        End Sub
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New MainForm())
        End Sub
    End Class
End Namespace