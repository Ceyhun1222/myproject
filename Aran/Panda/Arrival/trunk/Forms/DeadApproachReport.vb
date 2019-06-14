Option Strict Off
Option Explicit On
Imports System.Collections.Generic

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CDeadApproachReport
	Inherits System.Windows.Forms.Form

	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private pGeomElem As ESRI.ArcGIS.Carto.IElement
	Private SortF1 As Integer
	Private SortF2 As Integer
	Private SortF3 As Integer

	Private fTreshold1 As Double
	Private fTreshold2 As Double
	Private Ix3ID As Long

	Private LObstaclesPage1 As ObstacleContainer
	Private LObstaclesPage2 As ObstacleContainer
	Private LObstaclesPage3 As ObstacleContainer

	Private ReportBtn As System.Windows.Forms.CheckBox
	Private HelpContextID As Long = 13400

	Private lvwColumnSorter1 As ListViewColumnSorter
	Private lvwColumnSorter2 As ListViewColumnSorter
	Private lvwColumnSorter3 As ListViewColumnSorter

	Public Sub New()
		'MyBase.New()
		' This call is required by the Windows Form Designer.
		InitializeComponent()

		ReDim LObstaclesPage1.Obstacles(-1)
		ReDim LObstaclesPage1.Parts(-1)
		ReDim LObstaclesPage2.Obstacles(-1)
		ReDim LObstaclesPage2.Parts(-1)
		ReDim LObstaclesPage3.Obstacles(-1)
		ReDim LObstaclesPage3.Parts(-1)

		Text = My.Resources.str00815
		SaveBtn.Text = My.Resources.str30002
		CloseBtn.Text = My.Resources.str30001

		MultiPage1.TabPages.Item(0).Text = My.Resources.str50221
		MultiPage1.TabPages.Item(1).Text = My.Resources.str50222
		MultiPage1.TabPages.Item(2).Text = My.Resources.str50223
		MultiPage1.SelectedIndex = 0

		ListView1.Columns.Item(0).Text = "Type"   'type
		ListView1.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView1.Columns.Item(2).Text = My.Resources.str50226 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Elevation
		ListView1.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC
		ListView1.Columns.Item(4).Text = My.Resources.str50228 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. Elev
		ListView1.Columns.Item(5).Text = My.Resources.str30042 'Area

		ListView2.Columns.Item(0).Text = "Type"   'type
		ListView2.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView2.Columns.Item(2).Text = My.Resources.str50226 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Elevation
		ListView2.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC
		ListView2.Columns.Item(4).Text = My.Resources.str50228 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. Elev
		ListView2.Columns.Item(5).Text = My.Resources.str30042 'Area

		ListView3.Columns.Item(0).Text = "Type"   'type
		ListView3.Columns.Item(1).Text = My.Resources.str30011 'name
		ListView3.Columns.Item(2).Text = My.Resources.str50226 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Elevation
		ListView3.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'MOC
		ListView3.Columns.Item(4).Text = My.Resources.str50228 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")" 'Req. Elev
		ListView3.Columns.Item(5).Text = My.Resources.str30042 'Area

		lvwColumnSorter1 = New ListViewColumnSorter()
		lvwColumnSorter2 = New ListViewColumnSorter()
		lvwColumnSorter3 = New ListViewColumnSorter()

		ListView1.ListViewItemSorter = lvwColumnSorter1
		ListView2.ListViewItemSorter = lvwColumnSorter2
		ListView3.ListViewItemSorter = lvwColumnSorter3

		ListView1.View = View.Details
		ListView2.View = View.Details
		ListView3.View = View.Details
		'SetFormParented(Handle.ToInt32)
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

	Private Sub DeadApproachReport_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		If eventArgs.CloseReason = System.Windows.Forms.CloseReason.UserClosing Then
			eventArgs.Cancel = True
			CloseBtn_Click(CloseBtn, New EventArgs())
		End If
	End Sub

	'Private Sub UserForm_Terminate()
	'	Dim I As Integer
	'	Dim N As Integer

	'	pPointElem = Nothing
	'	N = UBound(LObstaclesPage1)
	'	For I = 0 To N
	'		LObstaclesPage1(I).pPtPrj = Nothing
	'		LObstaclesPage1(I).pPtGeo = Nothing
	'	Next I

	'	N = UBound(LObstaclesPage2)

	'	For I = 0 To N
	'		LObstaclesPage2(I).pPtPrj = Nothing
	'		LObstaclesPage2(I).pPtGeo = Nothing
	'	Next I

	'	N = UBound(LObstaclesPage3)
	'	For I = 0 To N
	'		LObstaclesPage3(I).pPtPrj = Nothing
	'		LObstaclesPage3(I).pPtGeo = Nothing
	'	Next I
	'	Erase LObstaclesPage1
	'	Erase LObstaclesPage2
	'	Erase LObstaclesPage3
	'End Sub

	Private Sub CloseBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CloseBtn.Click
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		ReportBtn.Checked = False
		Hide()
	End Sub

	Private Sub SaveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
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

		If SaveDlg.ShowDialog() <> DialogResult.OK Then Return
		FileNum = FreeFile()
		FileOpen(FileNum, SaveDlg.FileName, OpenMode.Output)

		PrintLine(FileNum, Text)
		'        Print #FileNum, Chr(9) + MultiPage1.Pages.Item(MultiPage1.Value).Caption
		PrintLine(FileNum)

		Select Case MultiPage1.SelectedIndex
			Case 0
				lListView = ListView1
			Case 1
				lListView = ListView2
			Case 2
				lListView = ListView3
			Case Else
				Return
		End Select

		N = lListView.Columns.Count
		ReDim HeadersText(N + 1)
		ReDim HeadersLen(N + 1)

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
				tmpStr = ""
			End If
			StrOut = StrOut + Space(J - M) + HeadersText(I) + tmpStr
		Next I

		PrintLine(FileNum, StrOut)
		StrOut = ""
		tmpStr = New String("-", maxLen) + "+"
		For I = 1 To N - 1
			StrOut = StrOut + tmpStr
		Next I
		StrOut = StrOut + New String("-", maxLen)

		PrintLine(FileNum, StrOut)

		M = lListView.Items.Count
		For I = 0 To M - 1
			itmX = lListView.Items.Item(I)
			TmpLen = Len(itmX.Text)
			If TmpLen > maxLen Then
				StrOut = Strings.Left(itmX.Text, maxLen - 1) + "*"
			Else
				StrOut = Space(maxLen - TmpLen) + itmX.Text
			End If

			For J = 1 To N - 1
				tmpStr = itmX.SubItems(J).Text

				TmpLen = Len(tmpStr)

				If TmpLen > maxLen Then
					tmpStr = Strings.Left(tmpStr, maxLen - 1) + "*"
				Else
					If J < N - 1 Or TmpLen > 0 Then
						tmpStr = Space(maxLen - TmpLen) + tmpStr
					End If
				End If

				StrOut = StrOut + "|" + tmpStr
			Next J
			PrintLine(FileNum, StrOut)
		Next I
		FileClose(FileNum)
	End Sub

	Sub SetReportBtn(ByRef Btn As System.Windows.Forms.CheckBox)
		ReportBtn = Btn
	End Sub

	Private Sub CDeadApproachReport_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
	End Sub

	Public Sub FillPage1(ByRef Obstacles As ObstacleContainer, Optional ByRef fTreshold As Double = NO_DATA_VALUE)
		Dim I As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(Obstacles.Parts)
		M = UBound(Obstacles.Obstacles)

		ReDim LObstaclesPage1.Parts(N)
		ReDim LObstaclesPage1.Obstacles(M)

		ListView1.BeginUpdate()
		ListView1.Items.Clear()

		If N < 0 Then Return
		For I = 0 To M
			LObstaclesPage1.Obstacles(I) = Obstacles.Obstacles(I)
		Next

		For I = 0 To N
			Obstacles.Parts(I).fSort = Obstacles.Parts(I).ReqH
		Next I

		shall_SortfSortD(Obstacles)

		Dim pColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(4)
		pColumnHeader.ImageIndex = 1

		fTreshold1 = fTreshold

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage1)
		N = unicalObstacleList.Count
		Dim z As Integer = -1
		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			LObstaclesPage1.Parts(I) = Obstacles.Parts(I)
			itmX = ListView1.Items.Add(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName)))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))))

			If (Obstacles.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If Obstacles.Parts(I).ReqH > fTreshold1 Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)

				For K = 1 To 5
					itmX.SubItems.Item(K).Font = itmX.Font
					itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
				Next K
			End If
			itmX.Tag = I
		Next

		ListView1.EndUpdate()

		lblCountNumber.Text = ListView1.Items.Count
		'MultiPage1.Value = 1
		'If ReportBtn Then Show 0
	End Sub

	Public Sub FillPage2(ByRef Obstacles As ObstacleContainer, Optional ByVal fTreshold As Double = NO_DATA_VALUE)
		Dim I As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(Obstacles.Parts)
		M = UBound(Obstacles.Obstacles)

		ReDim LObstaclesPage2.Parts(N)
		ReDim LObstaclesPage2.Obstacles(M)

		If N < 0 Then Return
		For I = 0 To M
			LObstaclesPage2.Obstacles(I) = Obstacles.Obstacles(I)
		Next

		fTreshold2 = fTreshold

		For I = 0 To N
			Obstacles.Parts(I).fSort = Obstacles.Parts(I).ReqH
		Next I

		shall_SortfSortD(Obstacles)

		ListView2.BeginUpdate()
		ListView2.Items.Clear()

		Dim pColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(4)
		pColumnHeader.ImageIndex = 1

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage2)
		N = unicalObstacleList.Count
		Dim z As Integer = -1
		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			LObstaclesPage2.Parts(I) = Obstacles.Parts(I)
			itmX = ListView2.Items.Add(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName)))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))))

			If (Obstacles.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If Obstacles.Parts(I).ReqH > fTreshold2 Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)

				For K = 1 To 5
					itmX.SubItems.Item(K).Font = itmX.Font
					itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
				Next K
			End If
			itmX.Tag = I
		Next

		ListView2.EndUpdate()
		lblCountNumber.Text = ListView2.Items.Count
		'MultiPage1.Value = 2
		'If ReportBtn Then Show 0
	End Sub

	Public Sub RecalcPage2(ByVal fTreshold As Double)
		Dim pColumnHeader As System.Windows.Forms.ColumnHeader
		fTreshold2 = fTreshold

		If SortF2 <> 0 Then
			pColumnHeader = ListView2.Columns.Item(System.Math.Abs(SortF2))
			SortF2 = -SortF2
			ListView2_ColumnClick(ListView2, New System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index))
		End If
	End Sub

	Public Sub FillPage3(ByRef Obstacles As ObstacleContainer, Optional ByVal Ix As Integer = -1)
		Dim I As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		N = UBound(Obstacles.Parts)
		M = UBound(Obstacles.Obstacles)

		ReDim LObstaclesPage3.Parts(N)
		ReDim LObstaclesPage3.Obstacles(M)

		If N < 0 Then Return

		ListView2.BeginUpdate()
		ListView2.Items.Clear()

		If Ix >= 0 Then
			Ix3ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix3ID = -1
		End If

		For I = 0 To M
			LObstaclesPage3.Obstacles(I) = Obstacles.Obstacles(I)
		Next

		For I = 0 To N
			Obstacles.Parts(I).fSort = Obstacles.Parts(I).ReqH
		Next I

		shall_SortfSortD(Obstacles)

		Dim pColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(4)
		pColumnHeader.ImageIndex = 1

		ListView3.BeginUpdate()
		ListView3.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(LObstaclesPage3)
		N = unicalObstacleList.Count
		Dim z As Integer = -1
		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value

			LObstaclesPage3.Parts(I) = Obstacles.Parts(I)
			itmX = ListView3.Items.Add(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName)))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))))

			If (Obstacles.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If I = Ix Then
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				itmX.ForeColor = Color.FromArgb(&HFF0000)

				For K = 1 To 5
					itmX.SubItems.Item(K).ForeColor = itmX.ForeColor
					itmX.SubItems.Item(K).Font = itmX.Font
				Next K
			End If
			itmX.Tag = I
		Next

		ListView3.EndUpdate()

		lblCountNumber.Text = ListView3.Items.Count
		'MultiPage1.Value = 2
		'If ReportBtn Then Show 0
	End Sub

	Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage1.Obstacles) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage1.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage1.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage1.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage1.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0


		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage2.Obstacles) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage2.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage2.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage2.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage2.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView3.SelectedIndexChanged
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		If sender.SelectedItems.Count = 0 Then Return

		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage3.Obstacles) < 0) Or (Item Is Nothing) Or (Not Visible) Then Return

		Dim Index As Integer
		Index = LObstaclesPage3.Parts(Item.Tag).Owner

		Dim pGeometry As ArcGIS.Geometry.IGeometry
		Dim pPtTmp As ArcGIS.Geometry.IPoint
		pGeometry = LObstaclesPage3.Obstacles(Index).pGeomPrj
		pPtTmp = LObstaclesPage3.Parts(Item.Tag).pPtPrj

		If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pGeomElem = DrawPolyLine(pGeometry, 255, 2)
			pGeomElem.Locked = True
		ElseIf pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pGeomElem = DrawPolygon(pGeometry, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage3.Obstacles(Index).UnicalName, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub ListView1_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
		SortListView(eventArgs.Column, lvwColumnSorter1, ListView1)
	End Sub

	Private Sub ListView2_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView2.ColumnClick
		SortListView(eventArgs.Column, lvwColumnSorter2, ListView2)
	End Sub

	Private Sub ListView3_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView3.ColumnClick
		SortListView(eventArgs.Column, lvwColumnSorter3, ListView3)
	End Sub

	Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
		If MultiPage1.SelectedIndex = 0 Then
			ListView1_SelectedIndexChanged(ListView1, New System.EventArgs)
			lblCountNumber.Text = ListView1.Items.Count
		ElseIf MultiPage1.SelectedIndex = 1 Then
			ListView2_SelectedIndexChanged(ListView2, New System.EventArgs)
			lblCountNumber.Text = ListView2.Items.Count
		Else
			ListView2_SelectedIndexChanged(ListView3, New System.EventArgs) ' ListView3.FocusedItem
			lblCountNumber.Text = ListView3.Items.Count
		End If
	End Sub

End Class
