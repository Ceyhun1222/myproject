Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS
Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Friend Class CReportForm
	Inherits Form
	Private pPointElem As Carto.IElement

	Private SortF2 As Integer

	Private LSegmentsArray1() As TypeDefinitions.SegmentInfo
	Private LObstaclesPage2() As TypeDefinitions.ObstacleType
	Private DObstID As Guid

	Public SegmentCnt As Short
	Private CurSegment As Short
	Private HelpContextID As Long
	Private pReportBtn As CheckBox

	Public Sub New()
		MyBase.New()
		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		pPointElem = Nothing
		SegmentCnt = -1
		'SortF1 = 0
		SortF2 = 0

		ListView1.Columns.Item(0).Text = My.Resources.str2000
		ListView1.Columns.Item(1).Text = My.Resources.str2001
		ListView1.Columns.Item(2).Text = My.Resources.str2002
		ListView1.Columns.Item(3).Text = My.Resources.str2003
		ListView1.Columns.Item(4).Text = My.Resources.str2004 + " (" + DistanceConverter(DistanceUnit).Unit + ")"
		'    ListView1.ColumnHeaders(5).Text = LoadResString(2005)

		ListView1.Columns.Item(5).Text = My.Resources.str2006 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(6).Text = My.Resources.str2007 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(7).Text = My.Resources.str2008 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(8).Text = My.Resources.str2018 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(9).Text = My.Resources.str2019 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView1.Columns.Item(10).Text = My.Resources.str2020

		ListView2.Columns.Item(0).Text = My.Resources.str2021
		ListView2.Columns.Item(1).Text = My.Resources.str2022
		ListView2.Columns.Item(2).Text = My.Resources.str2023 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView2.Columns.Item(3).Text = My.Resources.str2024 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView2.Columns.Item(4).Text = My.Resources.str2025 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView2.Columns.Item(5).Text = My.Resources.str2026 + " (" + DistanceConverter(DistanceUnit).Unit + ")"
		ListView2.Columns.Item(6).Text = My.Resources.str2030 + " (" + HeightConverter(HeightUnit).Unit + ")"
		ListView2.Columns.Item(7).Text = My.Resources.str2027

		tabControl1.SelectedIndex = 0
	End Sub

	Private Sub ReportForm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As FormClosingEventArgs) Handles Me.FormClosing
		If eventArgs.CloseReason = CloseReason.UserClosing Then
			eventArgs.Cancel = True
			CloseBtn_Click(Nothing, Nothing)
		End If
	End Sub


	Private Sub ReportForm_KeyUp(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
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

	Protected Overrides Sub WndProc(ByRef m As Windows.Forms.Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

	Public Sub DeleteSegment()
		SegmentCnt = SegmentCnt - 1
		If SegmentCnt >= 0 Then
			ReDim Preserve SegmentData(SegmentCnt)
			UpDown1.Maximum = SegmentCnt
		Else
			'ReDim LObstaclesPage1(-1)
			ReDim LObstaclesPage2(-1)
			ReDim SegmentData(-1)
			SegmentCnt = -1
		End If
	End Sub

	Public Sub AddSegment(ByRef pSegment As TypeDefinitions.SegmentInfo, ByRef pObstArray() As TypeDefinitions.ObstacleType)
		Dim N As Integer
		SegmentCnt = SegmentCnt + 1

		If UBound(SegmentData) < 0 Then
			ReDim SegmentData(SegmentCnt)
		Else
			ReDim Preserve SegmentData(SegmentCnt)
		End If

		SegmentData(SegmentCnt).Segment = pSegment
		N = UBound(pObstArray)
		ReDim SegmentData(SegmentCnt).Obstacles(N)
		Array.Copy(pObstArray, SegmentData(SegmentCnt).Obstacles, N + 1)

		UpDown1.Maximum = SegmentCnt

		If UpDown1.Value = SegmentCnt Then
			UpDown1_ValueChanged(Nothing, Nothing)
		Else
			UpDown1.Value = SegmentCnt
		End If
	End Sub

	Public Sub FillReport(ByRef pSegmentsArray() As TypeDefinitions.SegmentInfo)
		Dim N As Integer
		Dim I As Integer
		Dim fTmp As Double
		Dim itmX As ListViewItem

		ListView1.Items.Clear()

		N = UBound(pSegmentsArray)
		ReDim LSegmentsArray1(N)
		Array.Copy(pSegmentsArray, LSegmentsArray1, N + 1)

		For I = 0 To N - 2
			itmX = ListView1.Items.Add(CStr(I))

			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LSegmentsArray1(I).StartFIX.Name))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, LSegmentsArray1(I).EndFIX.Name))

			fTmp = Dir2Azt(LSegmentsArray1(I).StartFIX.pPtPrj, LSegmentsArray1(I).fDirection)
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(fTmp, 1))))

			fTmp = ReturnDistanceInMeters(LSegmentsArray1(I).StartFIX.pPtPrj, LSegmentsArray1(I).EndFIX.pPtPrj)
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(fTmp, eRoundMode.NEAREST))))

			'itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(LSegmentsArray1(I).fTurnAngle, eRoundMode.FLOOR))))

			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LSegmentsArray1(I).fHInterS, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LSegmentsArray1(I).fHInterE, eRoundMode.NEAREST))))

			fTmp = Math.Max(LSegmentsArray1(I).fHGuidE, LSegmentsArray1(I).fHGuidS)
			itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(fTmp, eRoundMode.NEAREST))))

			itmX.SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LSegmentsArray1(I).DominantObstacle.ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LSegmentsArray1(I).fMOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(10, New ListViewItem.ListViewSubItem(Nothing, LSegmentsArray1(I).DominantObstacle.UnicalName))
		Next I
	End Sub

	Public Sub FillPage2(ByRef pObstArray() As TypeDefinitions.ObstacleType)
		Dim N As Integer
		Dim I As Integer
		Dim J As Integer

		Dim itmX As ListViewItem
		Dim IsDominant As Boolean
		Dim ItemForeColor As Color
		Dim ItemFontStyle As FontStyle

		ListView2.Items.Clear()

		N = UBound(pObstArray)
		ReDim LObstaclesPage2(N)

		If N < 0 Then Return

		For I = 0 To N
			LObstaclesPage2(I) = pObstArray(I)
			itmX = ListView2.Items.Add(LObstaclesPage2(I).TypeName)

			itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage2(I).UnicalName))
			itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(LObstaclesPage2(I).Dist, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).MCA, eRoundMode.NEAREST))))

			If LObstaclesPage2(I).Prima = 1 Then
				itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			IsDominant = LObstaclesPage2(I).Identifier = DObstID

			ItemForeColor = IIf(IsDominant, Color.Red, Color.Black)
			ItemFontStyle = IIf(IsDominant, FontStyle.Bold, FontStyle.Regular)

			itmX.Font = New Font(itmX.Font, ItemFontStyle)
			itmX.ForeColor = ItemForeColor
			For J = 1 To 7
				itmX.SubItems.Item(J).Font = New Font(itmX.SubItems.Item(J).Font, ItemFontStyle)
				itmX.SubItems.Item(J).ForeColor = ItemForeColor
			Next J
		Next I

		If SortF2 <> 0 Then
			Dim pColumnHeader As ColumnHeader
			pColumnHeader = ListView2.Columns.Item(System.Math.Abs(SortF2) - 1)
			SortF2 = -SortF2
			ListView2_ColumnClick(ListView2, New ColumnClickEventArgs(pColumnHeader.Index))
		End If
	End Sub

	Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
		Dim pGraphics As Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage2) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		pPointElem = DrawPointWithText(LObstaclesPage2(Item.Index).ptPrj, LObstaclesPage2(Item.Index).Identifier.ToString(), 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView2_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As ColumnClickEventArgs) Handles ListView2.ColumnClick
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim ColumnHeader As ColumnHeader = ListView2.Columns(eventArgs.Column)
		Dim itmX As ListViewItem
		Dim IsDominant As Boolean
		Dim ItemForeColor As Color
		Dim ItemFontStyle As FontStyle

		ListView2.Sorting = SortOrder.None
		N = UBound(LObstaclesPage2)

		If System.Math.Abs(SortF2) - 1 = ColumnHeader.Index Then
			SortF2 = -SortF2
		Else
			If SortF2 <> 0 Then ListView2.Columns.Item(System.Math.Abs(SortF2) - 1).ImageIndex = 2
			SortF2 = ColumnHeader.Index + 1
		End If

		If SortF2 > 0 Then
			ColumnHeader.ImageIndex = 0
		Else
			ColumnHeader.ImageIndex = 1
		End If

		If (ColumnHeader.Index >= 2) And (ColumnHeader.Index < 7) Then
			For I = 0 To N
				Select Case ColumnHeader.Index
					Case 2
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).Height
					Case 3
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).MOC
					Case 4
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).ReqH
					Case 5
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).Dist
					Case 6
						LObstaclesPage2(I).fSort = LObstaclesPage2(I).MCA
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
						LObstaclesPage2(I).sSort = LObstaclesPage2(I).TypeName
					Case 1
						LObstaclesPage2(I).sSort = LObstaclesPage2(I).UnicalName
					Case 7
						If LObstaclesPage2(I).Prima = 1 Then
							LObstaclesPage2(I).sSort = "Primary"
						Else
							LObstaclesPage2(I).sSort = "Secondary"
						End If
				End Select
			Next I

			If SortF2 > 0 Then
				shall_SortsSort(LObstaclesPage2)
			Else
				shall_SortsSortD(LObstaclesPage2)
			End If
		End If

		For I = N + 2 To ListView2.Items.Count
			ListView2.Items.RemoveAt(1)
		Next I

		For I = 0 To N
			If ListView2.Items.Count <= I Then
				itmX = ListView2.Items.Add(LObstaclesPage2(I).TypeName)
			Else
				itmX = ListView2.Items.Item(I)
				itmX.Text = LObstaclesPage2(I).TypeName
			End If

			If itmX.SubItems.Count > 1 Then
				itmX.SubItems(1).Text = LObstaclesPage2(I).UnicalName
			Else
				itmX.SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, LObstaclesPage2(I).UnicalName))
			End If

			If itmX.SubItems.Count > 2 Then
				itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage2(I).Height))
			Else
				itmX.SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).Height))))
			End If

			If itmX.SubItems.Count > 3 Then
				itmX.SubItems(3).Text = CStr(ConvertHeight(LObstaclesPage2(I).MOC))
			Else
				itmX.SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).MOC))))
			End If

			If itmX.SubItems.Count > 4 Then
				itmX.SubItems(4).Text = CStr(ConvertHeight(LObstaclesPage2(I).ReqH, 2))
			Else
				itmX.SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).ReqH, eRoundMode.NEAREST))))
			End If

			If itmX.SubItems.Count > 5 Then
				itmX.SubItems(5).Text = CStr(ConvertDistance(LObstaclesPage2(I).Dist, 2))
			Else
				itmX.SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertDistance(LObstaclesPage2(I).Dist, eRoundMode.NEAREST))))
			End If

			If itmX.SubItems.Count > 6 Then
				itmX.SubItems(6).Text = CStr(ConvertHeight(LObstaclesPage2(I).MCA, 2))
			Else
				itmX.SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage2(I).MCA, eRoundMode.NEAREST))))
			End If

			If LObstaclesPage2(I).Prima = 1 Then
				If itmX.SubItems.Count > 7 Then
					itmX.SubItems(7).Text = "Primary"
				Else
					itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, "Primary"))
				End If
			Else
				If itmX.SubItems.Count > 7 Then
					itmX.SubItems(7).Text = "Secondary"
				Else
					itmX.SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, "Secondary"))
				End If
			End If

			IsDominant = LObstaclesPage2(I).Identifier = DObstID
			ItemForeColor = IIf(IsDominant, Color.Red, Color.Black)
			ItemFontStyle = IIf(IsDominant, FontStyle.Bold, FontStyle.Regular)

			itmX.Font = New Font(itmX.Font, ItemFontStyle)
			itmX.ForeColor = ItemForeColor
			For J = 1 To 7
				itmX.SubItems.Item(J).Font = New Font(itmX.SubItems.Item(J).Font, ItemFontStyle)
				itmX.SubItems.Item(J).ForeColor = ItemForeColor
			Next J
		Next I

		ListView2_SelectedIndexChanged(ListView2, New System.EventArgs())
	End Sub

	Private Sub tabControl1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tabControl1.SelectedIndexChanged
		'Static PreviousTab As Short = tabControl1.SelectedIndex()
		If tabControl1.SelectedIndex = 1 Then
			Label1.Visible = True
			'TextBox1.Visible = True
			UpDown1.Visible = True
		Else
			Label1.Visible = False
			'TextBox1.Visible = False
			UpDown1.Visible = False
		End If
		'PreviousTab = tabControl1.SelectedIndex()
	End Sub

	Private Sub UpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles UpDown1.ValueChanged
		If UpDown1.Value < 0 Then
			UpDown1.Value = 0
		ElseIf UpDown1.Value > SegmentCnt Then
			UpDown1.Value = SegmentCnt
		Else
			CurSegment = UpDown1.Value
			DObstID = SegmentData(CurSegment).Segment.DominantObstacle.Identifier
			FillPage2(SegmentData(CurSegment).Obstacles)
		End If
		'If (UpDown1.Value >= 0) And (UpDown1.Value <= SegmentCnt) Then
		'	CurSegment = UpDown1.Value
		'	DObstID = SegmentData(CurSegment).Segment.DominantObstacle.ID
		'	FillPage2(SegmentData(CurSegment).Obstacles)
		'End If
	End Sub

	'Private Sub UpDown1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpDown1.Click
	'	If (UpDown1.Value >= 0) And (UpDown1.Value <= SegmentCnt) Then
	'		CurSegment = UpDown1.Value
	'		DObstID = SegmentData(CurSegment).Segment.DominantObstacle.ID
	'		FillPage2(SegmentData(CurSegment).Obstacles)
	'	End If
	'End Sub

	Public Sub SetReportBtn(ByRef pBtn As CheckBox)
		pReportBtn = pBtn
	End Sub

	Private Sub CloseBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		On Error Resume Next
		If Not pPointElem Is Nothing Then GetActiveView().GraphicsContainer.DeleteElement(pPointElem)
		On Error GoTo 0
		GetActiveView().PartialRefresh(Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		pReportBtn.Checked = False
		Me.Hide()
	End Sub
End Class