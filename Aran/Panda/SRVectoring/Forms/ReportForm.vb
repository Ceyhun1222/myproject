Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic

Friend Class CReportForm
	Inherits System.Windows.Forms.Form

	Private pPointElem As ESRI.ArcGIS.Carto.IElement

	Private SortF1 As Integer
	Private SortF2 As Integer
	Private LObstaclesPage1() As TypeDefinitions.ObstacleType
	Private LObstaclesPage2() As TypeDefinitions.ObstacleType

	Public SegmentCnt As Short
	Private CurSegment As Short

	Private Ix1ID As String
	Private Ix2ID As String

	Private SegmentData() As SegmentDataType
	Private ReportBtn As System.Windows.Forms.CheckBox

	Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()

		SortF1 = 0
		SortF2 = 0

		ReDim LObstaclesPage1(-1)
		ReDim LObstaclesPage2(-1)
		ReDim SegmentData(-1)
		SegmentCnt = -1

		'MultiPage1.TabVisible(0) = False
		'Me.Text = My.Resources.str30200
		'MultiPage1.TabCaption(0) = My.Resources.str30201
		'MultiPage1.TabCaption(1) = My.Resources.str30202

		'Label1.Text = My.Resources.str30210
		''Label2.Text = My.Resources.str30211
		'SaveBtn.Text = My.Resources.str30002
		'CloseBtn.Text = My.Resources.str30001
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

	Public Sub SetSegmentObst(ByRef FIXObstacles() As TypeDefinitions.ObstacleType, ByRef HPrevFIX As Double, Optional ByRef FIXIx As Integer = -1)
		Dim n As Integer = FIXObstacles.Length
		ReDim SegmentData(SegmentCnt).FIXObstacles(n - 1)

		Array.Copy(FIXObstacles, SegmentData(SegmentCnt).FIXObstacles, FIXObstacles.Length)

		SegmentData(SegmentCnt).FIXIx = FIXIx
		SegmentData(SegmentCnt).HPrevFIX = HPrevFIX
		FillPage2(FIXObstacles, FIXIx)

		UpDown1.Value = SegmentCnt
	End Sub

	Public Sub AddSegment(ByRef FIXObstacles() As TypeDefinitions.ObstacleType, ByRef HPrevFIX As Double, Optional ByRef FIXIx As Integer = -1)
		SegmentCnt = SegmentCnt + 1
		If UBound(SegmentData) < 0 Then
			ReDim SegmentData(SegmentCnt)
		Else
			ReDim Preserve SegmentData(SegmentCnt)
		End If

		UpDown1.Maximum = SegmentCnt
		SetSegmentObst(FIXObstacles, HPrevFIX, FIXIx)
	End Sub

	Public Sub DeleteSegment(ByVal N As Integer)
		Dim I As Integer
		SegmentCnt = SegmentCnt - N
		If SegmentCnt >= 0 Then
			For I = SegmentCnt To UBound(LObstaclesPage2)
				LObstaclesPage2(I).pPtPrj = Nothing
				LObstaclesPage2(I).pPtGeo = Nothing
			Next I

			ReDim Preserve SegmentData(SegmentCnt)

			UpDown1.Maximum = SegmentCnt

			If UpDown1.Value > UpDown1.Maximum Then UpDown1.Value = UpDown1.Maximum

			FillPage2(SegmentData(UpDown1.Maximum).FIXObstacles, SegmentData(UpDown1.Maximum).FIXIx)
		Else
			For I = 0 To UBound(LObstaclesPage2)
				LObstaclesPage2(I).pPtPrj = Nothing
				LObstaclesPage2(I).pPtGeo = Nothing
			Next I

			ListView1.Items.Clear()
			ListView2.Items.Clear()

			ReDim LObstaclesPage1(-1)
			ReDim LObstaclesPage2(-1)
			ReDim SegmentData(-1)
			SegmentCnt = -1
		End If
	End Sub

	Public Sub FillPage1(ByRef Obstacles() As TypeDefinitions.ObstacleType, ByRef HPrevFIX As Double, Optional ByRef Ix As Integer = -1)
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		ListView1.Items.Clear()

		N = UBound(Obstacles)
		ReDim LObstaclesPage1(N)

		If N < 0 Then Exit Sub

		If Ix >= 0 Then
			Ix1ID = Obstacles(Ix).ID
		Else
			Ix1ID = ""
		End If

		'If HPrevFIX > -10000.0 Then
		'    Label2.Caption = CStr(Round(HPrevFIX - 0.4999999))
		'Else
		'    Label2.Caption = ""
		'End If

		For I = 0 To N
			LObstaclesPage1(I) = Obstacles(I)

			itmX = ListView1.Items.Add(Obstacles(I).Name)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles(I).ID))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).Height))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).MOC))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).ReqH))))


			If I = Ix Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
				For J = 1 To 4
					itmX.SubItems.Item(J).Font = itmX.Font
					itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
				Next J
			End If

			'itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next I

		If SortF1 <> 0 Then
			SortF1 = -SortF1
			ListView1_ColumnClick(ListView1, New System.Windows.Forms.ColumnClickEventArgs(System.Math.Abs(SortF1) - 1))
		End If

		'MultiPage1.SelectedIndex = 1
		'If ReportBtn.Checked And (Not Me.Visible) Then Show(s_Win32Window)
	End Sub

	Public Sub FillPage2(ByRef Obstacles() As TypeDefinitions.ObstacleType, Optional ByRef Ix As Integer = -1)
		Dim N As Integer
		Dim I As Integer
		Dim J As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		ListView2.Items.Clear()

		N = UBound(Obstacles)
		ReDim LObstaclesPage2(N)

		If N < 0 Then Exit Sub

		If Ix >= 0 Then
			Ix2ID = Obstacles(Ix).ID
		Else
			Ix2ID = ""
		End If

		For I = 0 To N
			LObstaclesPage2(I) = Obstacles(I)

			itmX = ListView2.Items.Add(Obstacles(I).Name)

			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles(I).ID))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).Height))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).MOC))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles(I).ReqH))))

			If I = Ix Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
				For J = 1 To 4
					itmX.SubItems.Item(J).Font = itmX.Font
					itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
				Next J
			End If
			'itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next I

		'MultiPage1.Value = 2
		'If ReportBtn Then Show 0

		If SortF2 <> 0 Then
			SortF2 = -SortF2
			ListView2_ColumnClick(ListView2, New System.Windows.Forms.ColumnClickEventArgs(System.Math.Abs(SortF2) - 1))
		End If

		'MultiPage1.SelectedIndex = 2
		'If ReportBtn.Checked And (Not Me.Visible) Then Show(s_Win32Window)
	End Sub

	Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
		If ListView1.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = ListView1.SelectedItems.Item(0)
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		If (UBound(LObstaclesPage1) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		pPointElem = DrawPointWithText(LObstaclesPage1(Item.Index).pPtPrj, LObstaclesPage1(Item.Index).ID, 255)
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView2.SelectedIndexChanged
		If ListView2.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = ListView2.SelectedItems.Item(0)

		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		If (UBound(LObstaclesPage2) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		pPointElem = DrawPointWithText(LObstaclesPage2(Item.Index).pPtPrj, LObstaclesPage2(Item.Index).ID, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView1_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView1.Columns(eventArgs.Column)
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(LObstaclesPage1)

		'ListView1.Sorting = SortOrder.None

		If System.Math.Abs(SortF1) - 1 = ColumnHeader.Index Then
			SortF1 = -SortF1
		Else
			If SortF1 <> 0 Then ListView1.Columns.Item(System.Math.Abs(SortF1) - 1).ImageIndex = -1
			SortF1 = ColumnHeader.Index + 1
		End If

		If SortF1 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If


		If (ColumnHeader.Index >= 2) Then
			For I = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage1(I).fSort = LObstaclesPage1(I).Height
					Case 3
						LObstaclesPage1(I).fSort = LObstaclesPage1(I).MOC
					Case 4
						LObstaclesPage1(I).fSort = LObstaclesPage1(I).ReqH
				End Select
			Next I

			'    Sort (LObstaclesPage1, 100)
			If SortF1 > 0 Then
				shall_SortfSort(LObstaclesPage1)
			Else
				shall_SortfSortD(LObstaclesPage1)
			End If
		Else
			For I = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						LObstaclesPage1(I).sSort = LObstaclesPage1(I).Name
					Case 1
						LObstaclesPage1(I).sSort = LObstaclesPage1(I).ID
				End Select
			Next I

			If SortF1 > 0 Then
				shall_SortsSort(LObstaclesPage1)
			Else
				shall_SortsSortD(LObstaclesPage1)
			End If
		End If

		For I = 0 To N
			itmX = ListView1.Items.Item(I)
			itmX.Text = LObstaclesPage1(I).Name

			itmX.SubItems(1).Text = LObstaclesPage1(I).ID
			itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage1(I).Height))
			itmX.SubItems(3).Text = CStr(ConvertHeight(LObstaclesPage1(I).MOC))
			itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage1(I).ReqH))

			If LObstaclesPage1(I).ID = Ix1ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For J = 1 To 4
				itmX.SubItems.Item(J).Font = itmX.Font
				itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
			Next J
			'itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next I

		If ReportBtn.Checked Then ListView1_SelectedIndexChanged(ListView1, New System.EventArgs)

	End Sub

	Private Sub ListView2_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView2.ColumnClick
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim ColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(eventArgs.Column)
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(LObstaclesPage2)
		'ListView2.Sorting = SortOrder.None

		If System.Math.Abs(SortF2) - 1 = ColumnHeader.Index Then
			SortF2 = -SortF2
		Else
			If SortF2 <> 0 Then ListView2.Columns.Item(System.Math.Abs(SortF2) - 1).ImageIndex = -1
			SortF2 = ColumnHeader.Index + 1
		End If

		If SortF2 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		If (ColumnHeader.Index >= 2) Then
			For I = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).Height
					Case 3
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).MOC
					Case 4
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).ReqH
				End Select
			Next I

			If SortF2 > 0 Then
				shall_SortfSort(LObstaclesPage2)
			Else
				shall_SortfSortD(LObstaclesPage2)
			End If
		Else
			For I = 0 To N
				Select Case ColumnHeader.Index
					Case 0
						LObstaclesPage2(I).sSort = LObstaclesPage2(I).Name
					Case 1
						LObstaclesPage2(I).sSort = LObstaclesPage2(I).ID
				End Select
			Next I

			If SortF2 > 0 Then
				shall_SortsSort(LObstaclesPage2)
			Else
				shall_SortsSortD(LObstaclesPage2)
			End If
		End If


		For I = 0 To N
			itmX = ListView2.Items.Item(I)
			itmX.Text = LObstaclesPage2(I).Name

			itmX.SubItems(1).Text = LObstaclesPage2(I).ID
			itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage2(I).Height))
			itmX.SubItems(3).Text = CStr(ConvertHeight(LObstaclesPage2(I).MOC))
			itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage2(I).ReqH))

			If LObstaclesPage2(I).ID = Ix2ID Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)
			Else
				itmX.Font = New Font(itmX.Font, FontStyle.Regular)
				itmX.ForeColor = Color.FromArgb(0)
			End If

			For J = 1 To 4
				itmX.SubItems.Item(J).Font = itmX.Font
				itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
			Next J
			'itmX.SubItems.Item(4).Font = New Font(itmX.SubItems.Item(4).Font, FontStyle.Bold)
		Next I

		ListView2_SelectedIndexChanged(ListView2, New System.EventArgs)
	End Sub

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		Static PreviousTab As Short = MultiPage1.SelectedIndex()
		Label1.Visible = MultiPage1.SelectedIndex > 0
		UpDown1.Visible = MultiPage1.SelectedIndex > 0
		UpDown1.Visible = MultiPage1.SelectedIndex > 0

		If MultiPage1.SelectedIndex = 0 Then
			ListView2_SelectedIndexChanged(ListView2, New System.EventArgs)
		Else
			ListView1_SelectedIndexChanged(ListView1, New System.EventArgs)
		End If

		PreviousTab = MultiPage1.SelectedIndex()
	End Sub

	Private Sub UpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpDown1.ValueChanged
		If (UpDown1.Value >= 0) And (UpDown1.Value <= SegmentCnt) Then
			CurSegment = UpDown1.Value
			FillPage1(SegmentData(UpDown1.Value).FIXObstacles, SegmentData(UpDown1.Value).HPrevFIX, SegmentData(UpDown1.Value).FIXIx)
		End If
	End Sub

	Private Sub ReportForm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			CloseBtn_ClickEvent(CloseBtn, New EventArgs())
		End If
		eventArgs.Cancel = Cancel
	End Sub

	Private Sub ReportForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim I As Integer
		Dim N As Integer

		pPointElem = Nothing
		N = UBound(LObstaclesPage1)
		For I = 0 To N
			LObstaclesPage1(I).pPtPrj = Nothing
			LObstaclesPage1(I).pPtGeo = Nothing
		Next I

		N = UBound(LObstaclesPage2)
		For I = 0 To N
			LObstaclesPage2(I).pPtPrj = Nothing
			LObstaclesPage2(I).pPtGeo = Nothing
		Next I

		Erase LObstaclesPage1
		Erase LObstaclesPage2
	End Sub

	Public Sub SaveProtocol(ByRef RepFileName As String, ByRef RepFileTitle As String)

		Dim ProtRep As New ReportFile
		Dim I As Integer
		Dim N As Integer

		'    Set ProtRep.ThrPtPrj = FictTHR
		ProtRep.RefHeight = 0.0

		ProtRep.OpenFile(RepFileName & "_Protocol", My.Resources.str0503)

		ProtRep.WriteMessage(My.Resources.str0504)
		ProtRep.WriteMessage()
		ProtRep.WriteMessage(RepFileTitle)
		ProtRep.WriteParam(My.Resources.str0211, CStr(Today) & " - " & CStr(TimeOfDay))

		ProtRep.WriteMessage()
		ProtRep.WriteMessage()

		N = UBound(SegmentData)

		For I = 0 To N
			ProtRep.WriteMessage("Segment " & CStr(I + 1))
			ProtRep.WriteMessage()
			ProtRep.WriteObstData(SegmentData(I).FIXObstacles, -1)
		Next

		ProtRep.CloseFile()
		ProtRep = Nothing
	End Sub

	Private Sub CloseBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportBtn.CheckState = False
		Me.Hide()
	End Sub

	Private Sub SaveBtn_ClickEvent(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
		Dim FileNum As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim M As Integer
		Dim TmpLen As Integer
		Dim maxLen As Integer
		Dim HeadersLen() As Integer
		Dim lListView As System.Windows.Forms.ListView

		Dim StrOut As String
		Dim HeadersText() As String
		Dim tmpStr As String
		Dim hdrX As System.Windows.Forms.ColumnHeader
		Dim itmX As System.Windows.Forms.ListViewItem

		lListView = ListView1

		If SaveDialog1.ShowDialog() <> Windows.Forms.DialogResult.OK Then Return
		'UPGRADE_ISSUE: MSComDlg.CommonDialog property CommonDialog1.CancelError was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'

		FileNum = FreeFile()
		FileOpen(FileNum, SaveDialog1.FileName, OpenMode.Output)

		PrintLine(FileNum, Text)
		'        Print #FileNum, Chr(9) + MultiPage1.Pages.Item(MultiPage1.Value).Caption
		PrintLine(FileNum)

		N = lListView.Columns.Count
		ReDim HeadersText(N + 1)
		ReDim HeadersLen(N + 1)

		maxLen = 0
		For I = 1 To N
			'UPGRADE_WARNING: Lower bound of collection lListView.ColumnHeaders has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			hdrX = lListView.Columns.Item(I)
			HeadersText(I) = """" & hdrX.Text & """"
			HeadersLen(I) = Len(HeadersText(I))
			If HeadersLen(I) > maxLen Then
				maxLen = HeadersLen(I)
			End If
		Next I

		StrOut = ""
		For I = 1 To N
			J = maxLen - HeadersLen(I)
			M = J / 2
			If I < N Then
				tmpStr = Space(M) & "|"
			Else
				tmpStr = ""
			End If
			StrOut = StrOut & Space(J - M) & HeadersText(I) & tmpStr
		Next I

		PrintLine(FileNum, StrOut)
		StrOut = ""
		tmpStr = New String("-", maxLen) & "+"
		For I = 1 To N - 1
			StrOut = StrOut & tmpStr
		Next I
		StrOut = StrOut & New String("-", maxLen)

		PrintLine(FileNum, StrOut)

		M = lListView.Items.Count
		For I = 1 To M
			'UPGRADE_WARNING: Lower bound of collection lListView.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
			itmX = lListView.Items.Item(I)
			TmpLen = Len(itmX.Text)
			If TmpLen > maxLen Then
				StrOut = VB.Left(itmX.Text, maxLen - 1) & "*"
			Else
				StrOut = Space(maxLen - TmpLen) & itmX.Text
			End If

			For J = 1 To N - 1
				'UPGRADE_WARNING: Lower bound of collection itmX has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
				tmpStr = itmX.SubItems(J).Text

				TmpLen = Len(tmpStr)

				If TmpLen > maxLen Then
					tmpStr = VB.Left(tmpStr, maxLen - 1) & "*"
				Else
					If J < N - 1 Or TmpLen > 0 Then
						tmpStr = Space(maxLen - TmpLen) & tmpStr
					End If
				End If

				StrOut = StrOut & "|" & tmpStr
			Next J
			PrintLine(FileNum, StrOut)
		Next I

		FileClose(FileNum)
	End Sub

	Sub SetReportBtn(ByRef Btn As System.Windows.Forms.CheckBox)
		ReportBtn = Btn
	End Sub

	'Sub SetVisible(ByVal I As Long)
	'Dim J As Long
	'    If i < 0 Then
	'        For j = 0 To MultiPage1.Pages.Count - 1
	'            MultiPage1.Pages(j).Visible = True
	'        Next j
	'    ElseIf i < MultiPage1.Pages.Count Then
	'        MultiPage1.Pages(i).Visible = True
	'    End If
	'End Sub

	'Sub SetUnVisible(ByVal I As Long)
	'Dim J As Long
	'    If (i < 0) Then
	'        For j = 0 To MultiPage1.Pages.Count - 1
	'            MultiPage1.Pages(j).Visible = False
	'        Next j
	'    ElseIf (i < MultiPage1.Pages.Count) Then
	'        MultiPage1.Pages(i).Visible = False
	'    End If
	'End Sub

End Class