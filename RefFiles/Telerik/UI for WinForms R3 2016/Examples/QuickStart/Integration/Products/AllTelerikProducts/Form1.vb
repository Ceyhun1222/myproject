Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Integration.Products.AllTelerikProducts
    Partial Public Class Form1
        Inherits ExternalProcessForm
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Function GetExecutablePath() As String
            Return "http://www.telerik.com/demos/"
        End Function
    End Class
End Namespace