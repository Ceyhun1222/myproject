Option Strict Off
Option Explicit On

Imports System.IO

Friend Class ReportFile

	Const ReportFileExt As String = ".htm"

	Public lListView As System.Windows.Forms.ListView
	Private pFileName As String
	Private _stwr As StreamWriter = Nothing

	Public Sub OpenFile(ByVal Name As String, ByVal ReportTitle As String)

		If (Name = "") Then Return
		If Not (_stwr Is Nothing) Then Return

		pFileName = Name + ReportFileExt

		_stwr = New StreamWriter(pFileName, False, System.Text.Encoding.UTF8)

		_stwr.WriteLine("<html>")
		_stwr.WriteLine("<head>")
		_stwr.WriteLine("<title>PANDA - " + ReportTitle + "</title>")
		_stwr.WriteLine("<meta http-equiv=""content-type"" content=""text/html; charset=utf-8"" />")
		_stwr.WriteLine("<style>")
		_stwr.WriteLine("body {font-family: Arial, Sans-Serif; font-size:12;}")
		_stwr.WriteLine("table {font-family: Arial, Sans-Serif; font-size:12;}")
		_stwr.WriteLine("</style>")
		_stwr.WriteLine("</head>")
		_stwr.WriteLine("<body>")
	End Sub

	Public Sub CloseFile()
		If _stwr Is Nothing Then Return

		_stwr.WriteLine("</body>")
		_stwr.WriteLine("</html>")

		_stwr.Flush()
		_stwr.Dispose()
		_stwr = Nothing
	End Sub


	Public Sub WriteText(ByRef Text As String)
		_stwr.WriteLine(Text)
	End Sub

	Public Sub WriteMessage(Optional ByRef Message As String = "")
		Dim temp As String

		temp = Message
		'If (Message <> "") Then temp = "<b>" + Message + "</b>"
		_stwr.WriteLine(System.Net.WebUtility.HtmlEncode(temp) + "<br>")
	End Sub

	Public Sub WriteParam(ByRef ParamName As String, ByRef ParamValue As String, Optional ByRef ParamUnit As String = "")
		If (ParamName = "") Or (ParamValue = "") Then Return

		System.Net.WebUtility.HtmlEncode(ParamName, _stwr)
		_stwr.WriteLine(": ")
		System.Net.WebUtility.HtmlEncode(ParamValue, _stwr)

		If (ParamUnit <> "") Then System.Net.WebUtility.HtmlEncode(" " + ParamUnit, _stwr)
		_stwr.WriteLine("<br>")
	End Sub

	Public Sub WriteTab(Optional ByRef TabComment As String = "")
		Dim n As Integer
		Dim m As Integer
		Dim i As Integer
		Dim j As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		If (lListView Is Nothing) Then Return
		If (lListView.Items.Count = 0) Then Return

		If (TabComment <> "") Then
			WriteMessage(TabComment)
			WriteMessage()
		End If

		_stwr.WriteLine("<table border='1' cellspacing='0' cellpadding='1'>")

		n = lListView.Columns.Count
		m = lListView.Items.Count

		_stwr.WriteLine("<tr>")
		For i = 1 To n
			_stwr.WriteLine("<td><b>" & lListView.Columns.Item(i).Text & "</b></td>")
		Next i
		_stwr.WriteLine("</tr>")

		For i = 1 To m
			itmX = lListView.Items.Item(i)

			_stwr.WriteLine("<tr>")
			_stwr.WriteLine("<td>" & itmX.Text & "</td>")

			For j = 1 To n - 1
				_stwr.WriteLine("<td>" & itmX.SubItems(j).Text & "</td>")
			Next j
			_stwr.WriteLine("</tr>")
		Next i

		_stwr.WriteLine("</table>")
		_stwr.WriteLine("<br><br>")
	End Sub

	Public Sub WriteImage(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
		If (ImageName = "") Then Return

		If (ImageComment <> "") Then
			WriteMessage(ImageComment)
			WriteMessage()
		End If

		_stwr.WriteLine("<img src='" & ImageName & "' border='0'>")
	End Sub

	Public Sub WriteImageLink(ByRef ImageName As String, Optional ByRef ImageComment As String = "")
		If (ImageName = "") Then Return
		_stwr.WriteLine("<a href='" & ImageName & "'>" & ImageComment & "</a>")
	End Sub
End Class