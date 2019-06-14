Imports System.Reflection
Imports System.IO
Namespace Telerik.Examples.WinControls.PdfViewer.FirstLook
    Public Class Form1



        Protected Overrides Sub OnLoad(e As System.EventArgs)
            MyBase.OnLoad(e)
            InitializeComponent()
            Dim stream As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Sample.pdf")
            Me.radPdfViewer1.LoadDocument(stream)
        End Sub
    End Class
End Namespace
