Option Strict Off
Option Explicit On

Friend Class CExcludeObstFrm
	Inherits System.Windows.Forms.Form

	Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
	Private pPointElem As ESRI.ArcGIS.Carto.IElement
	Private pGeomElem As ESRI.ArcGIS.Carto.IElement
	Private LObstaclesPage1 As ObstacleContainer
	Private SortF1 As Integer

	Private DlgResult As Integer

	Public Sub New()
		MyBase.New()
		' This call is required by the designer.
		InitializeComponent()

		' Add any initialization after the InitializeComponent() call.
		ListView1.Columns.Item(0).Text = My.Resources.str30011	'name
		ListView1.Columns.Item(1).Text = My.Resources.str50225	'id
		ListView1.Columns.Item(2).Text = My.Resources.str30050	'H Abv DER
	End Sub

	Public Function ExcludeObstacles(ByRef Obstacles As ObstacleContainer, _PrecisionFrm As CPrecisionFrm) As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim M As Integer
		Dim N As Integer
		Dim itmX As System.Windows.Forms.ListViewItem
		Dim pColumnHeader As System.Windows.Forms.ColumnHeader

		ExcludeObstacles = False
		ListView1.Items.Clear()

		pGraphics = GetActiveView().GraphicsContainer

		M = UBound(Obstacles.Obstacles)
		N = UBound(Obstacles.Parts)

		ReDim LObstaclesPage1.Obstacles(M)
		ReDim LObstaclesPage1.Parts(N)
		If M < 0 Then Exit Function

		For I = 0 To N
			LObstaclesPage1.Parts(I) = Obstacles.Parts(I)
		Next

		For I = 0 To M
			LObstaclesPage1.Obstacles(I) = Obstacles.Obstacles(I)
			itmX = ListView1.Items.Add(Obstacles.Obstacles(I).TypeName)
			itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Obstacles.Obstacles(I).UnicalName)))
			itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(Obstacles.Obstacles(I).Height, eRoundMode.NEAREST))))
			itmX.Checked = Obstacles.Obstacles(I).IgnoredByUser
		Next I

		SortF1 = -1


		'If SortF1 <> 0 Then
		'    Set pColumnHeader = ListView1.ColumnHeaders(Abs(SortF1))
		'    SortF1 = -SortF1
		'    ListView1_ColumnClick pColumnHeader
		'End If

		DlgResult = 0

		Me.ShowDialog(_PrecisionFrm)

		If DlgResult > 0 Then
			For I = 0 To M
				itmX = ListView1.Items.Item(I)
				Obstacles.Obstacles(I).IgnoredByUser = itmX.Checked
			Next
			ExcludeObstacles = True
		End If

		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		On Error GoTo 0
	End Function

	Private Sub ListView1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView1.SelectedIndexChanged
		If sender.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = sender.SelectedItems.Item(0)
		If (UBound(LObstaclesPage1.Obstacles) < 0) Or (Item Is Nothing) Then Return

		Dim pPtTmp As ArcGIS.Geometry.IPoint
		Dim pCurve As ArcGIS.Geometry.ICurve
		Dim pArea As ArcGIS.Geometry.IArea

		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If Not pPointElem Is Nothing Then pGraphics.DeleteElement(pPointElem)
		If Not pGeomElem Is Nothing Then pGraphics.DeleteElement(pGeomElem)
		On Error GoTo 0

		If LObstaclesPage1.Obstacles(Item.Index).pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
			pPtTmp = LObstaclesPage1.Obstacles(Item.Index).pGeomPrj
		ElseIf LObstaclesPage1.Obstacles(Item.Index).pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pCurve = LObstaclesPage1.Obstacles(Item.Index).pGeomPrj
			pPtTmp = New ArcGIS.Geometry.Point()
			pCurve.QueryPoint(ArcGIS.Geometry.esriSegmentExtension.esriNoExtension, 0.5, True, pPtTmp)
			pGeomElem = DrawPolyLine(pCurve, 255, 2)
			pGeomElem.Locked = True
		Else 'If LObstaclesPage11(Item.Index).pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pArea = LObstaclesPage1.Obstacles(Item.Index).pGeomPrj
			pPtTmp = New ArcGIS.Geometry.Point()
			pArea.QueryLabelPoint(pPtTmp)
			pGeomElem = DrawPolygon(LObstaclesPage1.Obstacles(Item.Index).pGeomPrj, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
			pGeomElem.Locked = True
		End If

		pPointElem = DrawPointWithText(pPtTmp, LObstaclesPage1.Obstacles(Item.Index).UnicalName, 255)
		pPointElem.Locked = True 'RGB(0, 0, 255)
	End Sub

	Private Sub ListView1_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
		'Dim I As Integer
		'Dim N As Integer
		'Dim M As Integer
		'Dim itmX As System.Windows.Forms.ListViewItem
		'Dim ZoneNames() As String = {My.Resources.str820, My.Resources.str30007}
		'Dim ColumnHdr As System.Windows.Forms.ColumnHeader = ListView1.Columns(e.Column)

		''ZoneNames = Array("No", "Yes")

		'ListView1.Sorting = SortOrder.None
		'N = UBound(LObstaclesPage1.Obstacles)

		'If System.Math.Abs(SortF1) - 1 = ColumnHdr.Index Then
		'	SortF1 = -SortF1
		'Else
		'	If SortF1 <> 0 Then ListView1.Columns.Item(System.Math.Abs(SortF1) - 1).ImageIndex = -1
		'	SortF1 = ColumnHdr.Index + 1
		'End If


		'If SortF1 > 0 Then
		'	ColumnHdr.ImageIndex = 0
		'Else
		'	ColumnHdr.ImageIndex = 1
		'End If


		'If (ColumnHdr.Index = 2) Then
		'	For I = 0 To N
		'		LObstaclesPage1.Obstacles(I).fSort = LObstaclesPage1.Obstacles(I).Height
		'	Next I

		'	If SortF1 > 0 Then
		'		shall_SortfSort(LObstaclesPage1)
		'	Else
		'		shall_SortfSortD(LObstaclesPage1)
		'	End If
		'Else
		'	For I = 0 To N
		'		Select Case ColumnHdr.Index
		'			Case 0
		'				LObstaclesPage1.Obstacles(I).sSort = LObstaclesPage1.Obstacles(I).Name
		'			Case 1
		'				LObstaclesPage1.Obstacles(I).sSort = LObstaclesPage1.Obstacles(I).ID
		'		End Select
		'	Next I

		'	If SortF1 > 0 Then
		'		shall_SortsSort(LObstaclesPage1)
		'	Else
		'		shall_SortsSortD(LObstaclesPage1)
		'	End If
		'End If

		'M = ListView1.Items.Count
		'For I = N + 1 To M - 1
		'	ListView1.Items.RemoveAt(1)
		'Next I

		'For I = 0 To N
		'	M = ListView1.Items.Count

		'	If M <= I Then
		'		itmX = ListView1.Items.Add(LObstaclesPage1.Obstacles(I).Name)
		'	Else
		'		itmX = ListView1.Items.Item(I)
		'		itmX.Text = LObstaclesPage1.Obstacles(I).Name
		'	End If

		'	If itmX.SubItems.Count > 1 Then
		'		itmX.SubItems(1).Text = LObstaclesPage1.Obstacles(I).ID
		'	Else
		'		itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, LObstaclesPage1.Obstacles(I).ID))
		'	End If

		'	If itmX.SubItems.Count > 2 Then
		'		itmX.SubItems(2).Text = CStr(ConvertHeight(LObstaclesPage1.Obstacles(I).Height, eRoundMode.rmNERAEST))
		'	Else
		'		itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(LObstaclesPage1.Obstacles(I).Height, eRoundMode.rmNERAEST))))
		'	End If

		'	itmX.Checked = LObstaclesPage1.Obstacles(I).IgnoredByUser
		'Next I

		'ListView1_SelectedIndexChanged(ListView1, New System.EventArgs())
	End Sub

	Private Sub OkBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OKbtn.Click
		DlgResult = 1
		Hide()
	End Sub

	Private Sub CancelBtn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelBtn.Click
		DlgResult = 0
		Hide()
	End Sub

End Class