Imports System.Reflection

Public Class AboutForm
	Public Sub New()
		InitializeComponent()

		Dim currentAssem As Assembly = Assembly.GetExecutingAssembly()

		lbllVersion.Text = "Version :  " + currentAssem.GetName().Version.ToString()
		lblVersionDate.Text = "Build date : " + RetrieveLinkerTimestamp().ToString("MM.dd.yyyy")

		Dim attrib As Object = currentAssem.GetCustomAttribute(GetType(AssemblyTitleAttribute))
		Me.Text = "About " + CType(attrib, AssemblyTitleAttribute).Title

		attrib = currentAssem.GetCustomAttribute(GetType(AssemblyDescriptionAttribute))
		lblDescription.Text = CType(attrib, AssemblyDescriptionAttribute).Description

		attrib = currentAssem.GetCustomAttribute(GetType(AssemblyCopyrightAttribute))
		lblCopyRight.Text = CType(attrib, AssemblyCopyrightAttribute).Copyright
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Close()
	End Sub
End Class