Friend Class VisualManoeuvringForm
    Private VMReportFrm As VisualManoeuvringReport
    Private currentPage As Integer
    Private RWYDATA() As RWYType
    Private fRefHeight As Double
    Private MSAObstacleList() As ObstacleMSA
    Private pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
    Private ObstacleList() As ObstacleAr

    'Pages
    Private _CirclingAreaPage As CirclingAreaPage
    Private _RWYNavaidPage As RWYNavaidPage
    Private _PrescribedTrackPage As PrescribedTrackPage

    Private PageLabel(2) As System.Windows.Forms.Label
    Private bFormInitialised As Boolean = False

	Private _visManReport As VisualManoeuvringReport

    Public Property visManReport() As VisualManoeuvringReport
        Get
            Return _visManReport
        End Get
        Set(ByVal value As VisualManoeuvringReport)
            _visManReport = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        PageLabel(0) = lbl_CirclingAreaPage
        PageLabel(1) = lbl_RWYNavaidPage
        PageLabel(2) = lbl_PrescribedTrackPage
    End Sub

    Private Sub VisualManoeuvringForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim I As Integer
        Dim N As Integer
        bFormInitialised = True
        btn_ShowPanel.Checked = False
        'ArrivalProfile = New CArrivalProfile
        VMReportFrm = New VisualManoeuvringReport
        VMReportFrm.SetBtn(btn_Report)

        pGraphics = GetActiveView().GraphicsContainer

        ReDim RWYDATA(-1)
        'ReDim RWYIndex(-1)
        ReDim MSAObstacleList(-1)

        RModel = MaxModelRadius

		'SetFormParented(Handle.ToInt32)

        fRefHeight = CurrADHP.pPtGeo.Z

        FillRWYList(RWYDATA, CurrADHP)
        N = UBound(RWYDATA)
        If N < 0 Then
            'gAranEnv.ShowError(My.Resources.str300)
            MsgBox(My.Resources.str300, MsgBoxStyle.Critical, "PANDA")
            Me.Close()
            Return
        End If

        fRefHeight = CurrADHP.pPtGeo.Z
        'ReDim RWYIndex(N)

		GetArObstaclesByDist(ObstacleList, CurrADHP, RModel, fRefHeight)

        _CirclingAreaPage = New CirclingAreaPage
        _CirclingAreaPage.RWYDATA = RWYDATA
        _CirclingAreaPage.ObstacleList = ObstacleList
        _RWYNavaidPage = New RWYNavaidPage
        _RWYNavaidPage.RWYDATA = RWYDATA
        _PrescribedTrackPage = New PrescribedTrackPage
        WorkPanel.Controls.Add(_CirclingAreaPage)
        WorkPanel.Controls.Add(_RWYNavaidPage)
        WorkPanel.Controls.Add(_PrescribedTrackPage)
        For I = 0 To WorkPanel.Controls.Count - 1
            WorkPanel.Controls(I).Dock = DockStyle.Fill
            If I = 0 Then
                WorkPanel.Controls(I).Show()
            Else
                WorkPanel.Controls(I).Hide()
            End If
        Next
        btn_Prev.Enabled = False
        FocusStepCaption(0)
    End Sub

    Private Sub btn_Next_Click(sender As System.Object, e As System.EventArgs) Handles btn_Next.Click
        Select Case currentPage
            Case 0
                WorkPanel.Controls(0).Hide()
                WorkPanel.Controls(1).Show()
                _RWYNavaidPage.Category = _CirclingAreaPage.cmbBox_AircraftCat.SelectedIndex
                _RWYNavaidPage.txtBox_ARCSemiSpan.Text = CStr(arSemiSpan.Values(_CirclingAreaPage.cmbBox_AircraftCat.SelectedIndex))
                _RWYNavaidPage.txtBox_ARCVeticalDimension.Text = CStr(arVerticalSize.Values(_CirclingAreaPage.cmbBox_AircraftCat.SelectedIndex))

                _RWYNavaidPage.cmbBox_TrueBRGRange.Items.Clear()
                Dim N As Integer = UBound(_RWYNavaidPage.Solutions)
                For I As Integer = 0 To N
                    If _RWYNavaidPage.Solutions(I).Low = _RWYNavaidPage.Solutions(I).High Then
                        _RWYNavaidPage.cmbBox_TrueBRGRange.Items.Add(CStr(_RWYNavaidPage.Solutions(I).Low))
                    Else
                        _RWYNavaidPage.cmbBox_TrueBRGRange.Items.Add(CStr(_RWYNavaidPage.Solutions(I).Low) & "-" & CStr(_RWYNavaidPage.Solutions(I).High))
                    End If
                Next I
                _RWYNavaidPage.cmbBox_TrueBRGRange.SelectedIndex = 0
                currentPage = 1
                btn_Prev.Enabled = True
            Case 1
                _PrescribedTrackPage.SetParams(_CirclingAreaPage.ConvexPoly, _RWYNavaidPage.fRefHeight, _CirclingAreaPage.Category, _CirclingAreaPage.TAS, _RWYNavaidPage.InitialDirectionAngle, _RWYNavaidPage.SelectedRWY, _RWYNavaidPage.RWYDATA, _RWYNavaidPage.FinalNav, _CirclingAreaPage.ADElevation)
                WorkPanel.Controls(1).Hide()
                WorkPanel.Controls(2).Show()
                currentPage = 2
                btn_Next.Enabled = False
                btn_Report.Enabled = True
        End Select
        FocusStepCaption((currentPage))
    End Sub

    Private Sub btn_Prev_Click(sender As System.Object, e As System.EventArgs) Handles btn_Prev.Click
        Select Case currentPage
            Case 1
                If _RWYNavaidPage.FASegmElement IsNot Nothing Then pGraphics.DeleteElement(_RWYNavaidPage.FASegmElement)
                If _RWYNavaidPage.LeftPolyElement IsNot Nothing Then pGraphics.DeleteElement(_RWYNavaidPage.LeftPolyElement)
                If _RWYNavaidPage.RightPolyElement IsNot Nothing Then pGraphics.DeleteElement(_RWYNavaidPage.RightPolyElement)
                If _RWYNavaidPage.PrimePolyElement IsNot Nothing Then pGraphics.DeleteElement(_RWYNavaidPage.PrimePolyElement)

                WorkPanel.Controls(1).Hide()
                WorkPanel.Controls(0).Show()
                currentPage = 0
                btn_Prev.Enabled = False
            Case 2
                If _PrescribedTrackPage.visManTrackConstructor IsNot Nothing Then _PrescribedTrackPage.visManTrackConstructor.Close()
                If _PrescribedTrackPage.newAddVisualReferencePoints IsNot Nothing Then _PrescribedTrackPage.newAddVisualReferencePoints.Close()
                _PrescribedTrackPage.DeleteCirclingBoxes()
                WorkPanel.Controls(2).Hide()
                WorkPanel.Controls(1).Show()
                currentPage = 1
                btn_Next.Enabled = True
                btn_Report.Enabled = False
        End Select
        FocusStepCaption((currentPage))
    End Sub

    Private Sub btn_Cancel_Click(sender As System.Object, e As System.EventArgs) Handles btn_Cancel.Click
        Me.Close()
    End Sub

    Private Sub VisualManoeuvringForm_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        DBModule.CloseDB()

        Dim I As Integer
        Dim J As Integer

        'If Not bUnloadByOk Then ClearScr()

        'NonPrecReportFrm.Close()
        'ArrivalProfile.Close()
        CloseLog()
        CurrCmd = 0

        'IntermediateBaseAreaElem = Nothing
        'IntermediateSecAreaElem = Nothing
        'FinalBaseAreaElem = Nothing
        'FinalSecAreaElem = Nothing
        If _RWYNavaidPage.RightPolyElement IsNot Nothing Then
            pGraphics.DeleteElement(_RWYNavaidPage.RightPolyElement)
            _RWYNavaidPage.RightPolyElement = Nothing
        End If
        If _RWYNavaidPage.PrimePolyElement IsNot Nothing Then
            pGraphics.DeleteElement(_RWYNavaidPage.PrimePolyElement)
            _RWYNavaidPage.PrimePolyElement = Nothing
        End If
        If _RWYNavaidPage.LeftPolyElement IsNot Nothing Then
            pGraphics.DeleteElement(_RWYNavaidPage.LeftPolyElement)
            _RWYNavaidPage.LeftPolyElement = Nothing
        End If
        If _RWYNavaidPage.FASegmElement IsNot Nothing Then
            pGraphics.DeleteElement(_RWYNavaidPage.FASegmElement)
            _RWYNavaidPage.FASegmElement = Nothing
        End If
        If _CirclingAreaPage.VisualAreaElement IsNot Nothing Then
            pGraphics.DeleteElement(_CirclingAreaPage.VisualAreaElement)
            _CirclingAreaPage.VisualAreaElement = Nothing
        End If
        GetActiveView().Refresh()

        For I = 0 To UBound(RWYDATA)
            For J = eRWY.PtStart To eRWY.PtEnd
                RWYDATA(I).pPtGeo(J) = Nothing
                RWYDATA(I).pPtPrj(J) = Nothing
            Next J
        Next I
        Erase RWYDATA
        Erase _CirclingAreaPage.RWYDATA
        Erase _RWYNavaidPage.RWYDATA
        'Erase RWYIndex

        For I = 0 To UBound(MSAObstacleList)
			MSAObstacleList(I).pGeomGeo = Nothing
			MSAObstacleList(I).pGeomPrj = Nothing
        Next I
        Erase MSAObstacleList

        _PrescribedTrackPage.DeleteCirclingBoxes()
        If _PrescribedTrackPage.visManTrackConstructor IsNot Nothing AndAlso Not _PrescribedTrackPage.visManTrackConstructor.isClosed Then
            _PrescribedTrackPage.visManTrackConstructor.Close()
        End If

        If _PrescribedTrackPage.newAddVisualReferencePoints IsNot Nothing AndAlso Not _PrescribedTrackPage.newAddVisualReferencePoints.isClosed Then
            _PrescribedTrackPage.newAddVisualReferencePoints.Close()
        End If

        _CirclingAreaPage = Nothing
        _RWYNavaidPage = Nothing
        _PrescribedTrackPage = Nothing
    End Sub

    Private Sub FocusStepCaption(ByRef StIndex As Integer)
        Dim I As Integer

        For I = 0 To 2
            PageLabel(I).ForeColor = System.Drawing.Color.FromArgb(&HC0C0C0)
            PageLabel(I).Font = New Font(PageLabel(I).Font, FontStyle.Regular)
        Next

        PageLabel(StIndex).ForeColor = System.Drawing.Color.FromArgb(&HFF8000)
        PageLabel(StIndex).Font = New Font(PageLabel(StIndex).Font, FontStyle.Bold)

        'Me.Text = My.Resources.str1 & " " & My.Resources.str818 & "  [" & MultiPage1.TabPages.Item(StIndex).Text & "]"
    End Sub

    Private Sub btn_ShowPanel_CheckStateChanged(sender As System.Object, e As System.EventArgs) Handles btn_ShowPanel.CheckStateChanged
        If Not bFormInitialised Then Return

        If btn_ShowPanel.Checked Then
            Me.Width = 775
			btn_ShowPanel.Image = Arrival.My.Resources.bmpHIDE_INFO
		Else
			Me.Width = 595
			btn_ShowPanel.Image = Arrival.My.Resources.bmpSHOW_INFO
        End If

        If btn_Next.Enabled Then
            btn_Next.Focus()
        Else
            btn_Prev.Focus()
        End If
    End Sub

    Private Sub btn_Report_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles btn_Report.CheckedChanged
        If _visManReport Is Nothing OrElse _visManReport.isClosed Then
            _visManReport = New VisualManoeuvringReport
            _visManReport.FillPageReferensePoints(VisualManoeuvringDBTool.GetVisualReferencePoints())
            _visManReport.FillPageCirclingAreaObstacles(_PrescribedTrackPage.ConvexPolyObstacleList, _PrescribedTrackPage._MOC)
            _visManReport.FillPageLeftCirclingBoxObstacles(_PrescribedTrackPage.leftCirclingBoxObstacles, _PrescribedTrackPage._MOC)
            _visManReport.FillPageRightCirclingBoxObstacles(_PrescribedTrackPage.rightCirclingBoxObstacles, _PrescribedTrackPage._MOC)
            If _PrescribedTrackPage IsNot Nothing Then
                If _PrescribedTrackPage.visManTrackConstructor IsNot Nothing Then
                    If _PrescribedTrackPage.visManTrackConstructor.trackConstructor IsNot Nothing Then
                        _visManReport.FillPageTrackSteps(_PrescribedTrackPage.visManTrackConstructor.trackConstructor.trackSteps, _PrescribedTrackPage._maxDivergenceAngle, _PrescribedTrackPage._maxConvergenceAngle, _PrescribedTrackPage._minManeuverDistance, _PrescribedTrackPage._maxVisibilityDistance)
                        _visManReport.FillPageTrackObstacles(_PrescribedTrackPage.visManTrackConstructor.trackConstructor.trackStepObstacles, _PrescribedTrackPage._MOC)
                        _visManReport.FillPageTrackStatistics(_PrescribedTrackPage.visManTrackConstructor.trackConstructor.trackOCH, _PrescribedTrackPage.visManTrackConstructor.trackConstructor.CalcualteTrackLength(), _PrescribedTrackPage._finalSegmentLength, _PrescribedTrackPage.visManTrackConstructor.trackConstructor.CalculateFinalSegmentDescentGradient(), _PrescribedTrackPage.visManTrackConstructor.trackConstructor.CalculateDownwindLegLength())
                    End If
                End If
            End If
            _visManReport.Show(s_Win32Window)
        End If
    End Sub
End Class