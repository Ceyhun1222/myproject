Imports System.ComponentModel
Imports System.Linq
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports System.Reflection
Imports System.IO

Namespace Telerik.Examples.WinControls.PdfViewer
	Partial Public Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()
		End Sub

		Protected Overrides Sub OnLoad(ByVal e As EventArgs)
			MyBase.OnLoad(e)

			Dim stream As Stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Sample.pdf")
			Me.radPdfViewer1.LoadDocument(stream)
		End Sub

		Protected Overrides Sub WireEvents()
		End Sub
	End Class
End Namespace
