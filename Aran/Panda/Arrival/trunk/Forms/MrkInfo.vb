Option Strict Off
Option Explicit On

Friend Class CMrkInfoForm
	Inherits System.Windows.Forms.Form

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		ListView1.Columns.Item(0).Text = "Name"
		ListView1.Columns.Item(1).Text = "ID"
		ListView1.Columns.Item(2).Text = "CallSign"
		ListView1.Columns.Item(3).Text = "DistFromTHR (" + DistanceConverter(DistanceUnit).Unit + ")"
		ListView1.Columns.Item(4).Text = "Height (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(5).Text = "Altitude (" + HeightConverter(HeightUnit).Unit + ")"

		'SetFormParented hwnd
	End Sub

	Private Sub SaveTabAsHTML(ByRef FileNum As Integer)
		Dim I As Integer
		Dim J As Integer

		Dim ColorStr As String
		Dim ParaStr As String
		Dim FontStr As String
		Dim EndFace As String
		Dim Face As String

		Dim lListView As System.Windows.Forms.ListView
		Dim itmsX As System.Windows.Forms.ListView.ListViewItemCollection

		lListView = ListView1

		ParaStr = "<p></p>"

		itmsX = lListView.Items

		PrintLine(FileNum, "<Table border=1>")
		PrintLine(FileNum, "<Tr>")
		For I = 0 To lListView.Columns.Count - 1
			PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + "><b>" + CStr(lListView.Columns.Item(I).Text) + "</b></Td>")
		Next
		PrintLine(FileNum, "</Tr>")

		For I = 0 To itmsX.Count - 1 'lListView.ListItems.Count-1
			'    Print #FileNum,"<Tr>"

			If System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = 0 Then
				ColorStr = "000000"
			ElseIf System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = 255 Then
				ColorStr = "FF0000"
			ElseIf System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor) = RGB(0, 0, 255) Then
				ColorStr = "0000FF"
			Else
				ColorStr = CStr(Not System.Drawing.ColorTranslator.ToOle(itmsX.Item(I).ForeColor))
			End If

			If itmsX.Item(I).Font.Bold Then
				Face = "<b>"
				EndFace = "</b>"
			Else
				Face = ""
				EndFace = ""
			End If
			FontStr = "<Font Color=" + Chr(34) + ColorStr + Chr(34) + ">" + Face

			PrintLine(FileNum, "<Tr><Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + itmsX.Item(I).Text + EndFace + "</Td>")

			For J = 1 To lListView.Columns.Count - 1
				PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + ">" + FontStr + itmsX.Item(I).SubItems(J).Text + EndFace + "</Td>")
			Next

			PrintLine(FileNum, "</Tr>")
		Next

		PrintLine(FileNum, "</Table>")
		PrintLine(FileNum, ParaStr)
	End Sub

	Private Sub SaveTab(ByRef FileNum As Integer)
		Dim N As Integer
		Dim M As Integer
		Dim I As Integer
		Dim J As Integer
		Dim TmpLen As Integer
		Dim maxLen As Integer
		Dim HeadersLen() As Integer
		Dim StrOut As String
		Dim HeadersText() As String

		Dim lListView As System.Windows.Forms.ListView
		Dim tmpStr As String
		Dim hdrX As System.Windows.Forms.ColumnHeader
		Dim itmX As System.Windows.Forms.ListViewItem

		lListView = ListView1

		PrintLine(FileNum, Chr(9) + Text)
		PrintLine(FileNum)

		N = lListView.Columns.Count
		ReDim HeadersText(N - 1)
		ReDim HeadersLen(N - 1)

		maxLen = 0
		For I = 0 To N - 1
			hdrX = lListView.Columns.Item(I)
			HeadersText(I) = """" + hdrX.Text + """"
			HeadersLen(I) = Len(HeadersText(I))
			If HeadersLen(I) > maxLen Then
				maxLen = HeadersLen(I)
			End If
		Next I

		StrOut = ""
		For I = 0 To N - 1
			J = maxLen - HeadersLen(I)
			M = J / 2
			If I < N Then
				tmpStr = Space(M) + "|"
			Else
				tmpStr = "|"
			End If
			StrOut = StrOut + Space(J - M) + HeadersText(I) + tmpStr
		Next I

		PrintLine(FileNum, StrOut)
		StrOut = ""
		tmpStr = New String("-", maxLen) + "+"
		For I = 1 To N
			StrOut = StrOut + tmpStr
		Next I
		'StrOut = StrOut + New String("-", maxLen)

		PrintLine(FileNum, StrOut)

		M = lListView.Items.Count
		For I = 0 To M - 1
			itmX = lListView.Items.Item(I)
			TmpLen = Len(itmX.Text)
			If TmpLen > maxLen Then
				StrOut = Microsoft.VisualBasic.Left(itmX.Text, maxLen - 1) + "*"
			Else
				StrOut = Space(maxLen - TmpLen) + itmX.Text
			End If

			For J = 1 To N - 1
				tmpStr = itmX.SubItems(J).Text

				TmpLen = Len(tmpStr)

				If TmpLen > maxLen Then
					tmpStr = Microsoft.VisualBasic.Left(tmpStr, maxLen - 1) + "*"
				Else
					If J < N - 1 Or TmpLen > 0 Then
						tmpStr = Space(maxLen - TmpLen) + tmpStr
					End If
				End If

				StrOut = StrOut + "|" + tmpStr
			Next J
			PrintLine(FileNum, StrOut + "|")
		Next I

		PrintLine(FileNum)
	End Sub

	Sub ShowMrkInfo(ByRef MkrList() As MKRType)
		Dim N As Integer
		Dim I As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(MkrList)

		ListView1.Items.Clear()
		For I = 0 To N
			itmX = ListView1.Items.Add(MkrList(I).Name)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, MkrList(I).Identifier.ToString()))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(MkrList(I).CallSign)))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(MkrList(I).DistFromTHR, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(MkrList(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(MkrList(I).Altitude, eRoundMode.NEAREST))))

			'MkrList(I).AxisBearing
			'MkrList(I).Class
			'MkrList(I).Frequency
			'MkrList(I).DistFromCL
			'MkrList(I).ILS_ID
			'MkrList(I).NDB_ID
		Next

		ShowDialog()
	End Sub

	Private Sub SaveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim L As Integer
		Dim pos As Integer
		Dim FileNum As Integer
		Dim sExt As String
		Dim bHtml As Boolean

		If ComDlgSave.ShowDialog() <> Windows.Forms.DialogResult.OK Then Return

		L = Len(ComDlgSave.FileName)
		pos = InStrRev(ComDlgSave.FileName, ".")
		If pos <> 0 Then
			sExt = Microsoft.VisualBasic.Right(ComDlgSave.FileName, L - pos)
			bHtml = (UCase(sExt) = "HTM") Or (UCase(sExt) = "HTML")
		Else
			bHtml = ComDlgSave.FilterIndex > 1
		End If

		FileNum = FreeFile()
		FileOpen(FileNum, ComDlgSave.FileName, OpenMode.Output)
		If bHtml Then
			SaveTabAsHTML(FileNum)
		Else
			SaveTab(FileNum)
		End If

		FileClose(FileNum)
	End Sub

	Private Sub CloseBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		Me.Close()
	End Sub
End Class