Option Strict Off

Imports System.Collections.Generic
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry

Friend Class VisualManoeuvringReport
    Private _isClosed As Boolean = False
    Public Property isClosed() As Boolean
        Get
            Return _isClosed
        End Get
        Set(ByVal value As Boolean)
            '_isClosed = value
        End Set
    End Property

    Private LVisualReferencePoints As List(Of VisualReferencePoint)
    Private LCirclingAreaObstacles As ObstacleAr()
    Private LleftCirclingBoxObstacles As ObstacleAr()
    Private LrightCirclingBoxObstacles As ObstacleAr()
    Private LTrackSteps As List(Of TrackStep)
    Private LTrackStepObstacles As ObstacleAr()

    Private pointElement As IElement
    Dim pPointElem As IElement
    Dim pInitLineElem As IElement
    Dim pIntermLineElem As IElement
    Dim pFinLineElem As IElement
    Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
    Private ReportBtn As System.Windows.Forms.CheckBox

    Private SortF2 As Integer
    Private SortF3 As Integer
    Private SortF4 As Integer
    Private SortF5 As Integer


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SortF2 = 0
    End Sub

    Public Sub FillPageReferensePoints(ByRef VisRefPnts As List(Of VisualReferencePoint))
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        lstVw_ReferencePoints.Items.Clear()

        N = VisRefPnts.Count
        LVisualReferencePoints = VisRefPnts

        For I = 0 To N - 1
            'LVisRefPnts(I).gShape = VisRefPnts(I).gShape
            'LVisRefPnts(I).pShape = VisRefPnts(I).pShape
            'LVisRefPnts(I).name = VisRefPnts(I).name
            'LVisRefPnts(I).type = VisRefPnts(I).type
            'LVisRefPnts(I).description = VisRefPnts(I).description

            itmX = lstVw_ReferencePoints.Items.Add(VisRefPnts(I).Name)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VisRefPnts(I).Type))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, VisRefPnts(I).Description))
        Next
    End Sub

    Public Sub FillPageCirclingAreaObstacles(ByRef CirclingAreaObs As ObstacleAr(), MOC As Double, Optional ByRef PrecFlg As Integer = -1, Optional ByRef UseILSPlanes As Boolean = False)
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem


        lstVw_CirclingAreaObstacles.Items.Clear()
        N = UBound(CirclingAreaObs)
        ReDim LCirclingAreaObstacles(N)
        If N < 0 Then Return

        For I = 0 To N
            LCirclingAreaObstacles(I).Name = CirclingAreaObs(I).Name
            LCirclingAreaObstacles(I).Height = ConvertHeight(CirclingAreaObs(I).Height, eRoundMode.rmNERAEST)
            LCirclingAreaObstacles(I).MOC = ConvertHeight(MOC, eRoundMode.rmNERAEST)
            LCirclingAreaObstacles(I).ReqH = CirclingAreaObs(I).Height + MOC

            itmX = lstVw_CirclingAreaObstacles.Items.Add(CirclingAreaObs(I).Name)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CirclingAreaObs(I).Height, eRoundMode.rmNERAEST))))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(MOC, eRoundMode.rmNERAEST))))
            itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(CirclingAreaObs(I).Height, eRoundMode.rmNERAEST) + MOC)))
        Next

        If SortF2 <> 0 Then
            SortF2 = -SortF2
            lstVw_CirclingAreaObstacles_ColumnClick(lstVw_CirclingAreaObstacles, New System.Windows.Forms.ColumnClickEventArgs(System.Math.Abs(SortF2) - 1))
        End If
    End Sub

    Public Sub FillPageLeftCirclingBoxObstacles(ByRef leftCirclingBoxObs As List(Of ObstacleAr), MOC As Double)
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        lstVw_LeftCirclingBoxObstacles.Items.Clear()

        N = leftCirclingBoxObs.Count - 1
        ReDim LleftCirclingBoxObstacles(N)
        If N < 0 Then Return

        For I = 0 To N
            LleftCirclingBoxObstacles(I).Name = leftCirclingBoxObs(I).Name
            LleftCirclingBoxObstacles(I).Height = ConvertHeight(leftCirclingBoxObs(I).Height, eRoundMode.rmNERAEST)
            LleftCirclingBoxObstacles(I).MOC = ConvertHeight(MOC, eRoundMode.rmNERAEST)
            LleftCirclingBoxObstacles(I).ReqH = leftCirclingBoxObs(I).Height + MOC

            itmX = lstVw_LeftCirclingBoxObstacles.Items.Add(leftCirclingBoxObs(I).Name)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(leftCirclingBoxObs(I).Height, eRoundMode.rmNERAEST))))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(MOC)))
            itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(leftCirclingBoxObs(I).Height, eRoundMode.rmNERAEST) + MOC)))
        Next

        If SortF3 <> 0 Then
            SortF3 = -SortF3
            lstVw_LeftCirclingBoxObstacles_ColumnClick(lstVw_LeftCirclingBoxObstacles, New System.Windows.Forms.ColumnClickEventArgs(System.Math.Abs(SortF3) - 1))
        End If
    End Sub

    Public Sub FillPageRightCirclingBoxObstacles(ByRef rightCirclingBoxObs As List(Of ObstacleAr), MOC As Double)
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        lstVw_RightCirclingBoxObstacles.Items.Clear()

        N = rightCirclingBoxObs.Count - 1
        ReDim LrightCirclingBoxObstacles(N)
        If N < 0 Then Return

        For I = 0 To N
            LrightCirclingBoxObstacles(I).Name = rightCirclingBoxObs(I).Name
            LrightCirclingBoxObstacles(I).Height = ConvertHeight(rightCirclingBoxObs(I).Height, eRoundMode.rmNERAEST)
            LrightCirclingBoxObstacles(I).MOC = ConvertHeight(MOC, eRoundMode.rmNERAEST)
            LrightCirclingBoxObstacles(I).ReqH = rightCirclingBoxObs(I).Height + MOC

            itmX = lstVw_RightCirclingBoxObstacles.Items.Add(rightCirclingBoxObs(I).Name)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(rightCirclingBoxObs(I).Height, eRoundMode.rmNERAEST))))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(MOC)))
            itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(rightCirclingBoxObs(I).Height, eRoundMode.rmNERAEST) + MOC)))
        Next

        If SortF4 <> 0 Then
            SortF4 = -SortF4
            lstVw_RightCirclingBoxObstacles_ColumnClick(lstVw_RightCirclingBoxObstacles, New System.Windows.Forms.ColumnClickEventArgs(System.Math.Abs(SortF4) - 1))
        End If
    End Sub

    Public Sub FillPageTrackSteps(ByRef trackSteps As List(Of TrackStep), maxDivergenceAngle As Double, maxConvergenceAngle As Double, minDistBetweenManeuvers As Double, maxVisibilityDistacne As Double)
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        lstVw_TrackSteps.Items.Clear()

        N = trackSteps.Count
        LTrackSteps = trackSteps

        For I = 0 To N - 1
            itmX = lstVw_TrackSteps.Items.Add("Step " & I + 1)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(trackSteps(I).DivergenceAngle))))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(trackSteps(I).ConvergenceAngle))))
            itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(Math.Round(trackSteps(I).DistBetweenManeuvers))))
            If trackSteps(I).DivergenceVisualReferencePoint IsNot Nothing Then
                itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, trackSteps(I).DivergenceVisualReferencePoint.Name))
            Else
                itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "-"))
            End If
            If trackSteps(I).ConvergenceVisualReferencePoint IsNot Nothing Then
                itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, trackSteps(I).ConvergenceVisualReferencePoint.Name))
            Else
                itmX.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, "-"))
            End If


            If trackSteps(I).usingCirclingBox = 0 Then
                If Math.Round(trackSteps(I).DivergenceAngle) > maxDivergenceAngle Then
                    itmX.Font = New Font(itmX.Font, FontStyle.Bold)
                    itmX.ForeColor = Color.FromArgb(&HFF0000)
                    'itmX.SubItems.Item(1).Font = itmX.Font
                    'itmX.SubItems.Item(1).ForeColor = itmX.ForeColor
                    'itmX.SubItems.Item(1).ForeColor = Color.FromArgb(&HFF0000)
                End If
                If Math.Round(trackSteps(I).ConvergenceAngle) > maxConvergenceAngle Then
                    itmX.Font = New Font(itmX.Font, FontStyle.Bold)
                    itmX.ForeColor = Color.FromArgb(&HFF0000)
                    'itmX.SubItems.Item(2).Font = itmX.Font
                    'itmX.SubItems.Item(2).ForeColor = itmX.ForeColor
                    'itmX.SubItems.Item(0).ForeColor = Color.FromArgb(&HFF0000)
                    'itmX.SubItems.Item(2).ForeColor = Color.FromArgb(&HFF0000)
                End If
                If Math.Round(trackSteps(I).DistBetweenManeuvers) < minDistBetweenManeuvers Then
                    itmX.Font = New Font(itmX.Font, FontStyle.Bold)
                    itmX.ForeColor = Color.FromArgb(&HFF0000)
                    'itmX.SubItems.Item(3).Font = itmX.Font
                    'itmX.SubItems.Item(3).ForeColor = itmX.ForeColor
                    'itmX.SubItems.Item(3).ForeColor = Color.FromArgb(&HFF0000)
                End If
                If Math.Round(trackSteps(I).DistBetweenManeuvers) > maxVisibilityDistacne Then
                    itmX.Font = New Font(itmX.Font, FontStyle.Bold)
                    itmX.ForeColor = Color.FromArgb(&HFF0000)
                End If
            End If
        Next
    End Sub

    Public Sub FillPageTrackObstacles(ByRef trackStepObs As List(Of ObstacleAr), MOC As Double)
        Dim N As Integer
        Dim I As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        lstVw_TrackObstacles.Items.Clear()

        N = trackStepObs.Count - 1
        ReDim LTrackStepObstacles(N)
        If N < 0 Then Return

        For I = 0 To N
            LTrackStepObstacles(I).Name = trackStepObs(I).Name
            LTrackStepObstacles(I).Height = ConvertHeight(trackStepObs(I).Height, eRoundMode.rmNERAEST)
            LTrackStepObstacles(I).MOC = ConvertHeight(MOC, eRoundMode.rmNERAEST)
            LTrackStepObstacles(I).ReqH = trackStepObs(I).Height + MOC
            LTrackStepObstacles(I).stepNumber = trackStepObs(I).stepNumber

            itmX = lstVw_TrackObstacles.Items.Add(trackStepObs(I).Name)
            itmX.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(trackStepObs(I).stepNumber)))
            itmX.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(trackStepObs(I).Height, CType(2, eRoundMode)))))
            itmX.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(MOC)))
            itmX.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(ConvertHeight(trackStepObs(I).Height, CType(2, eRoundMode)) + MOC)))
        Next
    End Sub

    Public Sub FillPageTrackStatistics(trackOCH As Double, trackLength As Double, finalSegmentLength As Double, descentGradient As Double, downwindLegLenth As Double)
        txtBox_trackOCH.Text = CStr(trackOCH)
        txtBox_trackLength.Text = CStr(trackLength)
        'txtBox_descentGradient.Text = CStr(Math.Round(trackOCH / finalSegmentLength * 100, 1))
        txtBox_descentGradient.Text = CStr(descentGradient)
        txtBox_downwindLegLength.Text = CStr(Math.Round(downwindLegLenth))
    End Sub

    Private Sub lstVw_VisRefPnts_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_ReferencePoints.SelectedIndexChanged
        If lstVw_ReferencePoints.SelectedItems.Count = 0 Then Return
        Dim Item As System.Windows.Forms.ListViewItem = lstVw_ReferencePoints.SelectedItems.Item(0)

        If (LVisualReferencePoints.Count = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
        pGraphics = GetActiveView().GraphicsContainer
        On Error Resume Next
        If pPointElem IsNot Nothing Then
            pGraphics.DeleteElement(pPointElem)
            pPointElem = Nothing
        End If
        On Error GoTo 0

        pPointElem = DrawPointWithText(LVisualReferencePoints(Item.Index).pShape, LVisualReferencePoints(Item.Index).Name, 255)
        pPointElem.Locked = True
    End Sub

    Private Sub lstVw_TrackSteps_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_TrackSteps.SelectedIndexChanged
        If lstVw_TrackSteps.SelectedItems.Count = 0 Then Return
        Dim Item As System.Windows.Forms.ListViewItem = lstVw_TrackSteps.SelectedItems.Item(0)
        If (LTrackSteps.Count = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
        pGraphics = GetActiveView().GraphicsContainer
        On Error Resume Next
        If pInitLineElem IsNot Nothing Then
            pGraphics.DeleteElement(pInitLineElem)
            pInitLineElem = Nothing
        End If
        If pIntermLineElem IsNot Nothing Then
            pGraphics.DeleteElement(pIntermLineElem)
            pIntermLineElem = Nothing
        End If
        If pFinLineElem IsNot Nothing Then
            pGraphics.DeleteElement(pFinLineElem)
            pFinLineElem = Nothing
        End If
        On Error GoTo 0

        pInitLineElem = DrawPolyLine(CType(LTrackSteps(Item.Index).initialSegmentCentrelinePointCollection, IPolyline), 255, 2.0, True)
        pIntermLineElem = DrawPolyLine(CType(LTrackSteps(Item.Index).intermediateSegmentCentrelinePointCollection, IPolyline), 255, 2.0, True)
        pFinLineElem = DrawPolyLine(CType(LTrackSteps(Item.Index).finalSegmentCentrelinePointCollection, IPolyline), 255, 2.0, True)
    End Sub

    Private Sub lstVw_TrackObstacles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_TrackObstacles.SelectedIndexChanged
        If lstVw_TrackObstacles.SelectedItems.Count = 0 Then Return
        Dim Item As System.Windows.Forms.ListViewItem = lstVw_TrackObstacles.SelectedItems.Item(0)

        If (LTrackStepObstacles.Length = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
        pGraphics = GetActiveView().GraphicsContainer
        On Error Resume Next
        If pPointElem IsNot Nothing Then
            pGraphics.DeleteElement(pPointElem)
            pPointElem = Nothing
        End If
        On Error GoTo 0

		pPointElem = DrawPointWithText(LTrackStepObstacles(Item.Index).pGeomPrj, LTrackStepObstacles(Item.Index).ID, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub VisualManoeuvringReport_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		pGraphics = GetActiveView().GraphicsContainer
		If pPointElem IsNot Nothing Then
			pGraphics.DeleteElement(pPointElem)
		End If
		If pInitLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pInitLineElem)
		End If
		If pIntermLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pIntermLineElem)
		End If
		If pFinLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pFinLineElem)
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		_isClosed = True
	End Sub

	Private Sub tbCntrl_VisualManoeuvring_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles tbCntrl_VisualManoeuvring.SelectedIndexChanged
		pGraphics = GetActiveView().GraphicsContainer
		If pPointElem IsNot Nothing Then
			pGraphics.DeleteElement(pPointElem)
			pPointElem = Nothing
		End If
		If pInitLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pInitLineElem)
			pInitLineElem = Nothing
		End If
		If pIntermLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pIntermLineElem)
			pIntermLineElem = Nothing
		End If
		If pFinLineElem IsNot Nothing Then
			pGraphics.DeleteElement(pFinLineElem)
			pFinLineElem = Nothing
		End If
	End Sub

	Sub SetBtn(ByRef Btn As System.Windows.Forms.CheckBox)
		'ReportBtn = Btn
	End Sub

	Private Sub CloseBtn_Click(sender As System.Object, e As System.EventArgs) Handles btn_Close.Click
		Me.Close()
	End Sub

	Private Sub lstVw_LeftCirclingBoxObstacles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_LeftCirclingBoxObstacles.SelectedIndexChanged
		If lstVw_LeftCirclingBoxObstacles.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = lstVw_LeftCirclingBoxObstacles.SelectedItems.Item(0)
		If (LleftCirclingBoxObstacles.Length = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If pPointElem IsNot Nothing Then
			pGraphics.DeleteElement(pPointElem)
			pPointElem = Nothing
		End If
		On Error GoTo 0

		pPointElem = DrawPointWithText(LleftCirclingBoxObstacles(Item.Index).pGeomPrj, LleftCirclingBoxObstacles(Item.Index).ID, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub lstVw_RightCirclingBoxObstacles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_RightCirclingBoxObstacles.SelectedIndexChanged
		If lstVw_RightCirclingBoxObstacles.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = lstVw_RightCirclingBoxObstacles.SelectedItems.Item(0)
		If (LrightCirclingBoxObstacles.Length = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If pPointElem IsNot Nothing Then
			pGraphics.DeleteElement(pPointElem)
			pPointElem = Nothing
		End If
		On Error GoTo 0

		pPointElem = DrawPointWithText(LrightCirclingBoxObstacles(Item.Index).pGeomPrj, LrightCirclingBoxObstacles(Item.Index).ID, 255)
		pPointElem.Locked = True
	End Sub

	Private Sub lstVw_CirclingAreaObstacles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstVw_CirclingAreaObstacles.SelectedIndexChanged
		If lstVw_CirclingAreaObstacles.SelectedItems.Count = 0 Then Return
		Dim Item As System.Windows.Forms.ListViewItem = lstVw_CirclingAreaObstacles.SelectedItems.Item(0)
		If (LCirclingAreaObstacles.Length = 0) Or (Item Is Nothing) Or (Not Visible) Then Return
		pGraphics = GetActiveView().GraphicsContainer
		On Error Resume Next
		If pPointElem IsNot Nothing Then
			pGraphics.DeleteElement(pPointElem)
			pPointElem = Nothing
		End If
		On Error GoTo 0

		pPointElem = DrawPointWithText(LCirclingAreaObstacles(Item.Index).pGeomPrj, LCirclingAreaObstacles(Item.Index).ID, 255)
		pPointElem.Locked = True
	End Sub

    Private Sub lstVw_CirclingAreaObstacles_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lstVw_CirclingAreaObstacles.ColumnClick
        Dim I As Integer
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lstVw_CirclingAreaObstacles.Columns(e.Column)
        Dim J As Integer
        Dim N As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        N = UBound(LCirclingAreaObstacles)

        If System.Math.Abs(SortF2) - 1 = ColumnHeader.Index Then
            SortF2 = -SortF2
        Else
            If SortF2 <> 0 Then lstVw_CirclingAreaObstacles.Columns.Item(System.Math.Abs(SortF2) - 1).ImageIndex = -1
            SortF2 = ColumnHeader.Index + 1
        End If

        If SortF2 > 0 Then
            ColumnHeader.ImageIndex = 0
        Else
            ColumnHeader.ImageIndex = 1
        End If

        If (ColumnHeader.Index >= 1) Then
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 1
                        LCirclingAreaObstacles(I).fSort = LCirclingAreaObstacles(I).Height
                    Case 2
                        LCirclingAreaObstacles(I).fSort = LCirclingAreaObstacles(I).MOC
                    Case 3
                        LCirclingAreaObstacles(I).fSort = LCirclingAreaObstacles(I).ReqH
                End Select
            Next I

            If SortF2 > 0 Then
                shall_SortfSort(LCirclingAreaObstacles)
            Else
                shall_SortfSortD(LCirclingAreaObstacles)
            End If
        Else
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 0
                        LCirclingAreaObstacles(I).sSort = LCirclingAreaObstacles(I).Name
                End Select
            Next I

            If SortF2 > 0 Then
                shall_SortsSort(LCirclingAreaObstacles)
            Else
                shall_SortsSortD(LCirclingAreaObstacles)
            End If
        End If

        For I = 0 To N
            itmX = lstVw_CirclingAreaObstacles.Items.Item(I)
            itmX.Text = LCirclingAreaObstacles(I).Name
            itmX.SubItems(1).Text = CStr(ConvertHeight(LCirclingAreaObstacles(I).Height, 2))
            itmX.SubItems(2).Text = CStr(ConvertHeight(LCirclingAreaObstacles(I).MOC, 2))
            itmX.SubItems(3).Text = CStr(ConvertHeight(LCirclingAreaObstacles(I).ReqH, 2))

            For J = 1 To 3 '11
                itmX.SubItems.Item(J).Font = itmX.Font
                itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
            Next J
        Next I
    End Sub

    Private Sub lstVw_LeftCirclingBoxObstacles_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lstVw_LeftCirclingBoxObstacles.ColumnClick
        Dim I As Integer
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lstVw_LeftCirclingBoxObstacles.Columns(e.Column)
        Dim J As Integer
        Dim N As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        N = UBound(LleftCirclingBoxObstacles)

        If System.Math.Abs(SortF3) - 1 = ColumnHeader.Index Then
            SortF3 = -SortF3
        Else
            If SortF3 <> 0 Then lstVw_LeftCirclingBoxObstacles.Columns.Item(System.Math.Abs(SortF3) - 1).ImageIndex = -1
            SortF3 = ColumnHeader.Index + 1
        End If

        If SortF3 > 0 Then
            ColumnHeader.ImageIndex = 0
        Else
            ColumnHeader.ImageIndex = 1
        End If

        If (ColumnHeader.Index >= 1) Then
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 1
                        LleftCirclingBoxObstacles(I).fSort = LleftCirclingBoxObstacles(I).Height
                    Case 2
                        LleftCirclingBoxObstacles(I).fSort = LleftCirclingBoxObstacles(I).MOC
                    Case 3
                        LleftCirclingBoxObstacles(I).fSort = LleftCirclingBoxObstacles(I).ReqH
                End Select
            Next I

            If SortF3 > 0 Then
                shall_SortfSort(LleftCirclingBoxObstacles)
            Else
                shall_SortfSortD(LleftCirclingBoxObstacles)
            End If
        Else
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 0
                        LleftCirclingBoxObstacles(I).sSort = LleftCirclingBoxObstacles(I).Name
                End Select
            Next I

            If SortF3 > 0 Then
                shall_SortsSort(LleftCirclingBoxObstacles)
            Else
                shall_SortsSortD(LleftCirclingBoxObstacles)
            End If
        End If

        For I = 0 To N
            itmX = lstVw_LeftCirclingBoxObstacles.Items.Item(I)
            itmX.Text = LleftCirclingBoxObstacles(I).Name
            itmX.SubItems(1).Text = CStr(ConvertHeight(LleftCirclingBoxObstacles(I).Height, 2))
            itmX.SubItems(2).Text = CStr(ConvertHeight(LleftCirclingBoxObstacles(I).MOC, 2))
            itmX.SubItems(3).Text = CStr(ConvertHeight(LleftCirclingBoxObstacles(I).ReqH, 2))

            For J = 1 To 3 '11
                itmX.SubItems.Item(J).Font = itmX.Font
                itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
            Next J
        Next I
    End Sub

    Private Sub lstVw_RightCirclingBoxObstacles_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lstVw_RightCirclingBoxObstacles.ColumnClick
        Dim I As Integer
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lstVw_RightCirclingBoxObstacles.Columns(e.Column)
        Dim J As Integer
        Dim N As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        N = UBound(LrightCirclingBoxObstacles)

        If System.Math.Abs(SortF4) - 1 = ColumnHeader.Index Then
            SortF4 = -SortF4
        Else
            If SortF4 <> 0 Then lstVw_RightCirclingBoxObstacles.Columns.Item(System.Math.Abs(SortF4) - 1).ImageIndex = -1
            SortF4 = ColumnHeader.Index + 1
        End If

        If SortF4 > 0 Then
            ColumnHeader.ImageIndex = 0
        Else
            ColumnHeader.ImageIndex = 1
        End If

        If (ColumnHeader.Index >= 1) Then
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 1
                        LrightCirclingBoxObstacles(I).fSort = LrightCirclingBoxObstacles(I).Height
                    Case 2
                        LrightCirclingBoxObstacles(I).fSort = LrightCirclingBoxObstacles(I).MOC
                    Case 3
                        LrightCirclingBoxObstacles(I).fSort = LrightCirclingBoxObstacles(I).ReqH
                End Select
            Next I

            If SortF4 > 0 Then
                shall_SortfSort(LrightCirclingBoxObstacles)
            Else
                shall_SortfSortD(LrightCirclingBoxObstacles)
            End If
        Else
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 0
                        LrightCirclingBoxObstacles(I).sSort = LrightCirclingBoxObstacles(I).Name
                End Select
            Next I

            If SortF4 > 0 Then
                shall_SortsSort(LrightCirclingBoxObstacles)
            Else
                shall_SortsSortD(LrightCirclingBoxObstacles)
            End If
        End If

        For I = 0 To N
            itmX = lstVw_RightCirclingBoxObstacles.Items.Item(I)
            itmX.Text = LrightCirclingBoxObstacles(I).Name
            itmX.SubItems(1).Text = CStr(ConvertHeight(LrightCirclingBoxObstacles(I).Height, 2))
            itmX.SubItems(2).Text = CStr(ConvertHeight(LrightCirclingBoxObstacles(I).MOC, 2))
            itmX.SubItems(3).Text = CStr(ConvertHeight(LrightCirclingBoxObstacles(I).ReqH, 2))

            For J = 1 To 3 '11
                itmX.SubItems.Item(J).Font = itmX.Font
                itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
            Next J
        Next I
    End Sub

    Private Sub lstVw_TrackObstacles_ColumnClick(sender As System.Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lstVw_TrackObstacles.ColumnClick
        Dim I As Integer
        Dim ColumnHeader As System.Windows.Forms.ColumnHeader = lstVw_TrackObstacles.Columns(e.Column)
        Dim J As Integer
        Dim N As Integer
        Dim itmX As System.Windows.Forms.ListViewItem

        N = UBound(LTrackStepObstacles)

        If System.Math.Abs(SortF5) - 1 = ColumnHeader.Index Then
            SortF5 = -SortF5
        Else
            If SortF5 <> 0 Then lstVw_TrackObstacles.Columns.Item(System.Math.Abs(SortF5) - 1).ImageIndex = -1
            SortF5 = ColumnHeader.Index + 1
        End If

        If SortF5 > 0 Then
            ColumnHeader.ImageIndex = 0
        Else
            ColumnHeader.ImageIndex = 1
        End If

        If (ColumnHeader.Index >= 1) Then
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 1
                        LTrackStepObstacles(I).fSort = LTrackStepObstacles(I).stepNumber
                    Case 2
                        LTrackStepObstacles(I).fSort = LTrackStepObstacles(I).Height
                    Case 3
                        LTrackStepObstacles(I).fSort = LTrackStepObstacles(I).MOC
                    Case 4
                        LTrackStepObstacles(I).fSort = LTrackStepObstacles(I).ReqH
                End Select
            Next I

            If SortF5 > 0 Then
                shall_SortfSort(LTrackStepObstacles)
            Else
                shall_SortfSortD(LTrackStepObstacles)
            End If
        Else
            For I = 0 To N
                Select Case ColumnHeader.Index
                    Case 0
                        LTrackStepObstacles(I).sSort = LTrackStepObstacles(I).Name
                End Select
            Next I

            If SortF5 > 0 Then
                shall_SortsSort(LTrackStepObstacles)
            Else
                shall_SortsSortD(LTrackStepObstacles)
            End If
        End If

        For I = 0 To N
            itmX = lstVw_TrackObstacles.Items.Item(I)
            itmX.Text = LTrackStepObstacles(I).Name
            itmX.SubItems(1).Text = CStr(LTrackStepObstacles(I).stepNumber)
            itmX.SubItems(2).Text = CStr(ConvertHeight(LTrackStepObstacles(I).Height, 2))
            itmX.SubItems(3).Text = CStr(ConvertHeight(LTrackStepObstacles(I).MOC, 2))
            itmX.SubItems(4).Text = CStr(ConvertHeight(LTrackStepObstacles(I).ReqH, 2))

            For J = 1 To 4 '11
                itmX.SubItems.Item(J).Font = itmX.Font
                itmX.SubItems.Item(J).ForeColor = itmX.ForeColor
            Next J
        Next I
    End Sub
End Class