Option Strict Off
Option Explicit On
Imports System.Collections.Generic

<System.Runtime.InteropServices.ComVisible(False)> Friend Class CInitialReportsFrm
    Inherits System.Windows.Forms.Form

    Private pPointElem As ESRI.ArcGIS.Carto.IElement
    Private pGeomElem As ESRI.ArcGIS.Carto.IElement

    Private SortF1 As Integer
    Private SortF2 As Integer
    Public SegmUbnd As Integer = -1

    Private Ix1ID As Long
    Private Ix2ID As Long
    Private LObstaclesPage1 As ObstacleContainer
    Private LObstaclesPage2 As ObstacleContainer

    Private HelpContextID As Long = 12400
    Private ReportBtn As System.Windows.Forms.CheckBox
    Private bFormInitialised As Boolean = False

    Private lvwColumnSorter1 As ListViewColumnSorter
    Private lvwColumnSorter2 As ListViewColumnSorter

    Public Sub SetSegmentObst(ByRef FIXObstacles As ObstacleContainer, ByRef HPrevFIX As Double, Optional ByRef FIXIx As Integer = -1)
        Dim M As Integer
        Dim N As Integer

        M = UBound(FIXObstacles.Obstacles)
        N = UBound(FIXObstacles.Parts)

        ReDim SegmentData(SegmUbnd).FIXObstacles.Obstacles(M)
        ReDim SegmentData(SegmUbnd).FIXObstacles.Parts(N)
        Array.Copy(FIXObstacles.Obstacles, SegmentData(SegmUbnd).FIXObstacles.Obstacles, M + 1)
        Array.Copy(FIXObstacles.Parts, SegmentData(SegmUbnd).FIXObstacles.Parts, N + 1)

        SegmentData(SegmUbnd).FIXIx = FIXIx
        SegmentData(SegmUbnd).HPrevFIX = HPrevFIX

        If UpDown1.Value <> SegmUbnd Then
            UpDown1.Value = SegmUbnd
        Else
            FillPage1(SegmentData(UpDown1.Value).FIXObstacles, SegmentData(UpDown1.Value).HPrevFIX, SegmentData(UpDown1.Value).FIXIx)
        End If
    End Sub

    Public Sub AddSegment(ByRef FIXObstacles As ObstacleContainer, ByRef HPrevFIX As Double, Optional ByRef FIXIx As Integer = -1)
        SegmUbnd = SegmUbnd + 1
        If UBound(SegmentData) < 0 Then
            ReDim SegmentData(SegmUbnd)
        Else
            ReDim Preserve SegmentData(SegmUbnd)
        End If

        UpDown1.Maximum = SegmUbnd
        SetSegmentObst(FIXObstacles, HPrevFIX, FIXIx)
    End Sub

    Public Sub DeleteSegment()
        SegmUbnd = SegmUbnd - 1
        If SegmUbnd >= 0 Then
            ReDim Preserve SegmentData(SegmUbnd)
            UpDown1.Maximum = SegmUbnd
        Else
            ReDim LObstaclesPage1.Parts(-1)
            ReDim LObstaclesPage2.Parts(-1)
            ReDim SegmentData(-1)
            SegmUbnd = -1

            ListView1.Items.Clear()
        End If
    End Sub

    Public Sub ClearSegments()

        ReDim LObstaclesPage1.Parts(-1)
        ReDim LObstaclesPage2.Parts(-1)
        ReDim SegmentData(-1)
        SegmUbnd = -1

        ListView1.Items.Clear()
    End Sub

    Public Sub FillPage1(ByRef Obstacles As ObstacleContainer, ByRef HPrevFIX As Double, Optional ByRef Ix As Integer = -1)
        Dim I As Integer
        Dim K As Integer
        Dim M As Integer
        Dim N As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        N = UBound(Obstacles.Parts)
        M = UBound(Obstacles.Obstacles)

        ReDim LObstaclesPage1.Parts(N)
        ReDim LObstaclesPage1.Obstacles(M)

        If N < 0 Then
            Ix1ID = -1
            Label3.Text = ""

            ListView1.BeginUpdate()
            ListView1.Items.Clear()
            ListView1.EndUpdate()
            Return
        End If

        If Ix >= 0 Then
            Ix1ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
        Else
            Ix1ID = -1
        End If

        If HPrevFIX > -10000.0 Then
            Label3.Text = CStr(ConvertHeight(HPrevFIX, eRoundMode.NEAREST))
        Else
            Label3.Text = ""
        End If

        For I = 0 To M
            LObstaclesPage1.Obstacles(I) = Obstacles.Obstacles(I)
        Next

        For I = 0 To N
            Obstacles.Parts(I).fSort = Obstacles.Parts(I).ReqH
        Next I

        shall_SortfSortD(Obstacles)

        Dim pColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(5)
        pColumnHeader.ImageIndex = 1
        ListView1.BeginUpdate()
        ListView1.Items.Clear()

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)
		N = unicalObstacleList.Count
		Dim z As Integer = -1
		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value

			LObstaclesPage1.Parts(I) = Obstacles.Parts(I)
			itmX = ListView1.Items.Add(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(100.0 * Obstacles.Parts(I).fTmp + 0.04999999, 1))))
			itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(Obstacles.Parts(I).Dist, eRoundMode.NEAREST))))

			If (Obstacles.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			If I = Ix Then
				itmX.ForeColor = Color.Red
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				For K = 1 To 7
					itmX.SubItems.Item(K).ForeColor = Color.Red
					itmX.SubItems.Item(K).Font = New Font(itmX.SubItems.Item(K).Font, FontStyle.Bold)
				Next K
			End If
			itmX.Tag = I
		Next

		ListView1.EndUpdate()

		If SortF1 = 0 Then
			SortF1 = 1
		Else
			SortF1 = -SortF1
		End If

		pColumnHeader = ListView1.Columns.Item(System.Math.Abs(SortF1) - 1)
		'ListView1_ColumnClick(ListView1, New System.Windows.Forms.ColumnClickEventArgs(pColumnHeader.Index))

		'End If

		'If ReportBtn Then Show 0
	End Sub

	Public Sub FillPage2(ByRef Obstacles As ObstacleContainer, Optional ByRef Ix As Integer = -1)
		Dim I As Integer
		Dim M As Integer
		Dim N As Integer
		Dim K As Integer
		Dim itmX As System.Windows.Forms.ListViewItem

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)

		ReDim LObstaclesPage2.Obstacles(M)
		ReDim LObstaclesPage2.Parts(N)

		ListView2.BeginUpdate()
		ListView2.Items.Clear()

		If N < 0 Then
			ListView2.EndUpdate()
			Return
		End If

		If Ix >= 0 Then
			Ix2ID = Obstacles.Obstacles(Obstacles.Parts(Ix).Owner).ID
		Else
			Ix2ID = -1
		End If

		For I = 0 To M
			LObstaclesPage2.Obstacles(I) = Obstacles.Obstacles(I)
		Next

		For I = 0 To N
			Obstacles.Parts(I).fSort = Obstacles.Parts(I).ReqH
		Next I

		shall_SortfSortD(Obstacles)

		Dim pColumnHeader As System.Windows.Forms.ColumnHeader = ListView2.Columns(4)
		pColumnHeader.ImageIndex = 1

		Dim unicalObstacleList As IDictionary(Of Guid, Integer) = GetUnicalObstales(Obstacles)
		N = unicalObstacleList.Count
		Dim z As Integer = -1
		For Each item As KeyValuePair(Of Guid, Integer) In unicalObstacleList
			I = item.Value
			LObstaclesPage2.Parts(I) = Obstacles.Parts(I)
			itmX = ListView2.Items.Add(Obstacles.Obstacles(Obstacles.Parts(I).Owner).TypeName)

			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Obstacles.Obstacles(Obstacles.Parts(I).Owner).UnicalName))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).Height, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).MOC, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportHeight(Obstacles.Parts(I).ReqH, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(System.Math.Round(100.0 * Obstacles.Parts(I).fTmp + 0.04999999, 1))))
			itmX.SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(Obstacles.Parts(I).Dist, eRoundMode.NEAREST))))

			If (Obstacles.Parts(I).Flags And 1) = 1 Then
				itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Primary"))
			Else
				itmX.SubItems.Insert(7, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "Secondary"))
			End If

			itmX.SubItems.Insert(8, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(Obstacles.Parts(I).Rmin, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(9, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(Obstacles.Parts(I).Rmax, eRoundMode.NEAREST))))
			itmX.SubItems.Insert(10, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertReportDistance(Obstacles.Parts(I).TurnDistL, eRoundMode.NEAREST))))
			If I = Ix Then
				itmX.ForeColor = Color.Red
				itmX.Font = New Font(itmX.Font, FontStyle.Bold)
				For K = 1 To 10 '12
					itmX.SubItems.Item(K).ForeColor = Color.Red
					itmX.SubItems.Item(K).Font = New Font(itmX.SubItems.Item(K).Font, FontStyle.Bold)
				Next K
			End If
			itmX.Tag = I
		Next

		ListView2.EndUpdate()

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

    Private Sub ListView1_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
       SortListView(eventArgs.Column,lvwColumnSorter1,Listview1)
    End Sub

    Private Sub ListView2_ColumnClick(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.ColumnClickEventArgs) Handles ListView2.ColumnClick
      SortListView(eventArgs.Column,lvwColumnSorter2,Listview2)
    End Sub

    Private Sub MultiPage1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MultiPage1.SelectedIndexChanged
        Label1.Visible = MultiPage1.SelectedIndex > 0
        UpDown1.Visible = MultiPage1.SelectedIndex > 0

        If MultiPage1.SelectedIndex = 0 Then
            ListView1_SelectedIndexChanged(ListView1, New System.EventArgs)
        Else
            ListView2_SelectedIndexChanged(ListView2, New System.EventArgs)
        End If
    End Sub

    Private Sub UpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles UpDown1.ValueChanged
        If Not bFormInitialised Then Return

        If (UpDown1.Value >= 0) And (UpDown1.Value <= SegmUbnd) Then
            FillPage1(SegmentData(UpDown1.Value).FIXObstacles, SegmentData(UpDown1.Value).HPrevFIX, SegmentData(UpDown1.Value).FIXIx)
        End If
    End Sub

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        SegmUbnd = -1
        bFormInitialised = True
        ReDim LObstaclesPage1.Parts(-1)
        ReDim LObstaclesPage2.Parts(-1)
        ReDim LObstaclesPage1.Obstacles(-1)
        ReDim LObstaclesPage2.Obstacles(-1)
        ReDim SegmentData(-1)

        Text = My.Resources.str30200
        MultiPage1.TabPages.Item(0).Text = My.Resources.str30201
        MultiPage1.TabPages.Item(1).Text = My.Resources.str30202
        MultiPage1.SelectedIndex = 0

        ListView1.Columns.Item(0).Text = "Type"   'type
        ListView1.Columns.Item(1).Text = My.Resources.str30011 'name
        ListView1.Columns.Item(2).Text = My.Resources.str50226 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'Height
        ListView1.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'MOC
        ListView1.Columns.Item(4).Text = My.Resources.str40032 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'Req. H
        ListView1.Columns.Item(5).Text = My.Resources.str40033 + " (%)" 'Req PDG
        ListView1.Columns.Item(6).Text = My.Resources.str40035 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Dist
        ListView1.Columns.Item(7).Text = My.Resources.str40034 'Area

        ListView2.Columns.Item(0).Text = "Type"   'type
        ListView2.Columns.Item(1).Text = My.Resources.str30011 'name
        ListView2.Columns.Item(2).Text = My.Resources.str50226 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'Height
        ListView2.Columns.Item(3).Text = My.Resources.str30036 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'MOC
        ListView2.Columns.Item(4).Text = My.Resources.str40032 + " (" + ReportHeightConverter(ReportHeightUnit).Unit + ")"  'Req. H
        ListView2.Columns.Item(5).Text = My.Resources.str40033 + " (%)" 'Req. PDG

        ListView2.Columns.Item(6).Text = My.Resources.str40035 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")" 'Dist
        ListView2.Columns.Item(7).Text = My.Resources.str40034 'Area

        '    ListView2.ColumnHeaders(8).Text = LoadResString(30042) + " (" + DistanceConverter(DistanceUnit).Unit + ")"'RMin
        '    ListView2.ColumnHeaders(9).Text = LoadResString(30042) + " (" + DistanceConverter(DistanceUnit).Unit + ")" 'RMax (Xs)m
        ListView2.Columns.Item(10).Text = My.Resources.str40036 + " (" + ReportDistanceConverter(ReportDistanceUnit).Unit + ")"  'Turn Dist m

        Label1.Text = My.Resources.str30210
        Label2.Text = My.Resources.str30211 + "(" + ReportHeightConverter(ReportHeightUnit).Unit + ")"

        SaveBtn.Text = My.Resources.str30002
        CloseBtn.Text = My.Resources.str30001

        lvwColumnSorter1 = New ListViewColumnSorter()
        lvwColumnSorter2 = New ListViewColumnSorter()

        ListView1.ListViewItemSorter = lvwColumnSorter1
        ListView2.ListViewItemSorter = lvwColumnSorter2

        ListView1.View = View.Details
        ListView2.View = View.Details
        'SetFormParented(Handle.ToInt32)
    End Sub

    Private Sub InitialReportsFrm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If eventArgs.CloseReason = System.Windows.Forms.CloseReason.UserClosing Then
            eventArgs.Cancel = True
            CloseBtn_Click(CloseBtn, New EventArgs())
        End If
    End Sub

	Private Sub InitialReportsFrm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		Dim I As Integer
		Dim N As Integer

		pPointElem = Nothing
		N = UBound(LObstaclesPage1.Obstacles)
		For I = 0 To N
			LObstaclesPage1.Obstacles(I).pGeomPrj = Nothing
			LObstaclesPage1.Obstacles(I).pGeomGeo = Nothing
		Next I
		Erase LObstaclesPage1.Obstacles

		N = UBound(LObstaclesPage2.Obstacles)
		For I = 0 To N
			LObstaclesPage2.Obstacles(I).pGeomPrj = Nothing
			LObstaclesPage2.Obstacles(I).pGeomGeo = Nothing
		Next I
		Erase LObstaclesPage2.Obstacles
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

    Private Sub SaveTabAsHTML(ByRef lListView As System.Windows.Forms.ListView, ByRef FileNum As Integer)
        Dim I As Integer
        Dim J As Integer
        Dim ColorStr As String
        Dim ParaStr As String
        Dim FontStr As String
        Dim EndFace As String
        Dim Face As String
        Dim itmsX As System.Windows.Forms.ListView.ListViewItemCollection

        ParaStr = "<p></p>"
        itmsX = lListView.Items

        PrintLine(FileNum, "<Table border=1>")
        PrintLine(FileNum, "<Tr>")
        For I = 0 To lListView.Columns.Count - 1
            PrintLine(FileNum, "<Td Width=" + Chr(34) + "9%" + Chr(34) + "><b>" + CStr(lListView.Columns.Item(I).Text) + "</b></Td>")
        Next
        PrintLine(FileNum, "</Tr>")

        For I = 0 To itmsX.Count - 1 'lListView.ListItems.Count
            'Print #FileNum,"<Tr>"
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

    Private Sub SaveTab(ByRef lListView As System.Windows.Forms.ListView, ByRef FileNum As Integer)
        Dim I As Integer
        Dim J As Integer
        Dim N As Integer
        Dim M As Integer
        Dim TmpLen As Integer
        Dim maxLen As Integer
        Dim HeadersLen() As Integer

        Dim StrOut As String
        Dim HeadersText() As String
        Dim tmpStr As String
        Dim hdrX As System.Windows.Forms.ColumnHeader
        Dim itmX As System.Windows.Forms.ListViewItem

        PrintLine(FileNum, Text)
        '      Print #FileNum, Chr(9) + MultiPage1.Pages.Item(MultiPage1.Value).Caption
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
    End Sub

    Private Sub SaveBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SaveBtn.Click
        Dim L As Integer
        Dim pos As Integer
        Dim FileNum As Integer
        Dim sExt As String
        Dim bHtml As Boolean
        Dim lListView As System.Windows.Forms.ListView

        If SaveDlg.ShowDialog() <> DialogResult.OK Then Return

        L = Len(SaveDlg.FileName)
        pos = InStrRev(SaveDlg.FileName, ".")
        If pos <> 0 Then
            sExt = Strings.Right(SaveDlg.FileName, L - pos)
            bHtml = (UCase(sExt) = "HTM") Or (UCase(sExt) = "HTML")
        Else
            bHtml = SaveDlg.FilterIndex > 1
        End If

        FileNum = FreeFile()
        FileOpen(FileNum, SaveDlg.FileName, OpenMode.Output)

        Select Case MultiPage1.SelectedIndex
            Case 0
                lListView = ListView2
            Case 1
                lListView = ListView1
            Case Else
                Return
        End Select

        If bHtml Then
            SaveTabAsHTML(lListView, FileNum)
        Else
            SaveTab(lListView, FileNum)
        End If

        FileClose(FileNum)
    End Sub

    Sub SetReportBtn(ByRef Btn As System.Windows.Forms.CheckBox)
        ReportBtn = Btn
    End Sub

	'Sub SetVisible(ByVal I As Long)
	'Dim J As Long
	'    If i < 0 Then
	'        For J = 0 To MultiPage1.Pages.Count - 1
	'            MultiPage1.Pages(J).Visible = True
	'        Next J
	'    ElseIf i < MultiPage1.Pages.Count Then
	'        MultiPage1.Pages(i).Visible = True
	'    End If
	'End Sub

	'Sub SetUnVisible(ByVal I As Long)
	'Dim J As Long
	'    If (i < 0) Then
	'        For J = 0 To MultiPage1.Pages.Count - 1
	'            MultiPage1.Pages(J).Visible = False
	'        Next J
	'    ElseIf (i < MultiPage1.Pages.Count) Then
	'        MultiPage1.Pages(i).Visible = False
	'    End If
	'End Sub

	Private Sub CInitialReportsFrm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F1 Then
			HtmlHelp(0, HelpFile, HH_HELP_CONTEXT, HelpContextID)
			e.Handled = True
		End If
	End Sub

End Class