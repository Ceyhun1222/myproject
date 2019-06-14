Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.Zip

Namespace Telerik.Examples.WinControls.ZipLibrary.FirstLook
	Public Partial Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

			For i As Integer = 1 To 7
				Dim item As ListViewDataItem = New ListViewDataItem("sample text file " & i & ".txt")
				item.CheckState = Telerik.WinControls.Enumerations.ToggleState.On
				checkedListView.Items.Add(item)
			Next i
		End Sub

		Private Sub saveZipFileButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveZipFileButton.Click
			Dim dialog As SaveFileDialog = New SaveFileDialog()
            dialog.Filter = "Zip File | *.zip"
            dialog.FileName = "sample zip file.zip"
			Dim dialogResult As DialogResult = dialog.ShowDialog()

			If dialogResult = System.Windows.Forms.DialogResult.OK Then
				Using stream As Stream = dialog.OpenFile()
					Using zipArchive As ZipArchive = New ZipArchive(stream, ZipArchiveMode.Create, False,Nothing)
						For Each file As ListViewDataItem In checkedListView.CheckedItems
							Using entry As ZipArchiveEntry = zipArchive.CreateEntry(file.Text)
								Using ms As MemoryStream = CreateNewFile(file.Text)
									Dim entryStream As Stream = entry.Open()
									Telerik.WinControls.Zip.Extensions.ZipFile.CopyStreamTo(ms, entryStream)
								End Using
							End Using
						Next file
					End Using
				End Using
			End If
		End Sub

		Private Sub readZipFileButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles readZipFileButton.Click
			Dim dialog As OpenFileDialog = New OpenFileDialog()
			dialog.Filter = "Zip File | *.zip"
			Dim dialogResult As DialogResult = dialog.ShowDialog()

			If dialogResult = System.Windows.Forms.DialogResult.OK Then
				Dim stream As Stream = dialog.OpenFile()
				Using zipArchive As ZipArchive = New ZipArchive(stream, ZipArchiveMode.Read, False,Nothing)
					radGridView1.DataSource = zipArchive.Entries
					For Each asd As ZipArchiveEntry In zipArchive.Entries

					Next asd
				End Using
			End If
		End Sub

		Private Sub clearZipFileButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles clearZipFileButton.Click
			radGridView1.DataSource = Nothing
		End Sub

		Private Function CreateNewFile(ByVal file As String) As MemoryStream
			Dim stream As MemoryStream = New MemoryStream()
			Dim writer As StreamWriter = New StreamWriter(stream, New UTF8Encoding())
			writer.Write(file)
			writer.Flush()
			stream.Seek(0, SeekOrigin.Begin)
			Return stream
		End Function
	End Class
End Namespace