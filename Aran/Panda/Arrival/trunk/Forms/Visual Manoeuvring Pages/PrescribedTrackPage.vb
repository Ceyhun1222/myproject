Option Strict Off

Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports System.Collections.Generic

Friend Class PrescribedTrackPage
    Private _rightBoxPolygon As IPolygon
    Private _rightBoxPolygonElement As IElement
    Private _rightBoxCentreline As IPolyline
    Private _rightBoxCentrelineElement As IElement
    Private _rightBoxPntColl As IPointCollection

    Private _leftBoxPolygon As IPolygon
    Private _leftBoxPolygonElement As IElement
    Private _leftBoxCentreline As IPolyline
    Private _leftBoxCentrelineElement As IElement
    Private _leftBoxPntColl As IPointCollection

    Private _finalSegmentPolygon As IPolygon
    Private _finalSegmentElement As IElement
    Private _finalSegmentCentreline As IPolyline
    Private _finalSegmentCentrelineElement As IElement
    Private _finalSegmentPntColl As IPointCollection


    Private _maneuveringAreaPntColl As IPointCollection
    Private _maneuveringAreaRelatOper As IRelationalOperator
    Private _cat As Integer
    Private _TAS As Double
    Private _initialDirectionAngle As Double
    Private _selectedRWY As RWYType
    Private _rwyData() As RWYType
    Private _finalSegmentIAS As Double
    Private _minFinalSegmentIAS As Double
    Private _maxFinalSegmentIAS As Double
    Private _finalSegmentTAS As Double
    Private _finalSegmentTime As Double
    Public _finalSegmentLength As Double

    Private _corridorSemiWidth As Double
    Private _turnRadius As Double
    Public _MOC As Double
    Private _minOCH As Double
    Public _minVisibilityDistance As Double
    Private _maxDescentGradient As Double
    Public _maxVisibilityDistance As Double
    Public _maxDivergenceAngle As Double
    Public _maxConvergenceAngle As Double
    Private _bankEstablishTime As Double
    Private _bankEstablishDistance As Double
    Private _bankAngle As Double
    Private _initialStartPnt As IPoint
    Private _ahElev As Double
    Private _ISA As Double

    Private _maxDivergenceDistance As Double
    Private _maxConvergenceDistance As Double
    Public _minManeuverDistance As Double

    Private _circlingBoxObstacles As List(Of CirclingBoxObstacle)
    Public Property circlingBoxObstacles() As List(Of CirclingBoxObstacle)
        Get
            Return _circlingBoxObstacles
        End Get
        Set(ByVal value As List(Of CirclingBoxObstacle))
            _circlingBoxObstacles = value
        End Set
    End Property
    Private _leftCirclingBoxObstacles As List(Of ObstacleAr)
    Public Property leftCirclingBoxObstacles() As List(Of ObstacleAr)
        Get
            Return _leftCirclingBoxObstacles
        End Get
        Set(ByVal value As List(Of ObstacleAr))
            _leftCirclingBoxObstacles = value
        End Set
    End Property
    Private _rightCirclingBoxObstacles As List(Of ObstacleAr)
    Public Property rightCirclingBoxObstacles() As List(Of ObstacleAr)
        Get
            Return _rightCirclingBoxObstacles
        End Get
        Set(ByVal value As List(Of ObstacleAr))
            _rightCirclingBoxObstacles = value
        End Set
    End Property


    Private _rightBoxOCH As Double
    Public Property rightBoxOCH() As Double
        Get
            Return _rightBoxOCH
        End Get
        Set(ByVal value As Double)
            _rightBoxOCH = value
        End Set
    End Property
    Private _leftBoxOCH As Double
    Public Property leftBoxOCH() As Double
        Get
            Return _leftBoxOCH
        End Get
        Set(ByVal value As Double)
            _leftBoxOCH = value
        End Set
    End Property
    Private _rightBoxHighestObstacleIndex As Integer
    Private _leftBoxHighestObstacleIndex As Integer
    Private _bestCirclingBox As Integer

    Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

    Private _newAddVisualReferencePoints As AddVisualReferencePoints
    Public Property newAddVisualReferencePoints() As AddVisualReferencePoints
        Get
            Return _newAddVisualReferencePoints
        End Get
        Set(ByVal value As AddVisualReferencePoints)
            _newAddVisualReferencePoints = value
        End Set
    End Property
    Private _visManTrackConstructor As VisualManoeuvringTrackConstructor
    Public Property visManTrackConstructor() As VisualManoeuvringTrackConstructor
        Get
            Return _visManTrackConstructor
        End Get
        Set(ByVal value As VisualManoeuvringTrackConstructor)
            _visManTrackConstructor = value
        End Set
    End Property
    Private _visManReport As VisualManoeuvringReport
    Public Property visManReport() As VisualManoeuvringReport
        Get
            Return _visManReport
        End Get
        Set(ByVal value As VisualManoeuvringReport)
            _visManReport = value
        End Set
    End Property

    Private _ConvexPoly As IPolygon

    Private _convexPolyObstacleList As ObstacleAr()
    Public Property ConvexPolyObstacleList() As ObstacleAr()
        Get
            Return _convexPolyObstacleList
        End Get
        Set(ByVal value As ObstacleAr())
            _convexPolyObstacleList = value
        End Set
    End Property


    Public Sub SetParams(convexPoly As IPointCollection, fRefHeight As Double, Cat As Integer, TAS As Double, initialDirectionAngle As Double,
                         SelectedRWY As RWYType, RWYDATA() As RWYType, finalNav As NavaidType, AH_Elev As Double)

        lbl_meterSign1.Text = DistanceConverter(DistanceUnit).Unit
        lbl_kmhSign2.Text = SpeedConverter(SpeedUnit).Unit
        lbl_meterSign9.Text = HeightConverter(HeightUnit).Unit
        lbl_meterSign10.Text = HeightConverter(HeightUnit).Unit
        lbl_meterSign8.Text = DistanceConverter(DistanceUnit).Unit
        lbl_meterSign2.Text = DistanceConverter(DistanceUnit).Unit
        lbl_meterSign6.Text = DistanceConverter(DistanceUnit).Unit
        lbl_meterSign7.Text = DistanceConverter(DistanceUnit).Unit

        _cat = Cat
        pGraphics = GetActiveView().GraphicsContainer
        Select Case _cat
            Case 0 'Cat A
                _corridorSemiWidth = 1400
                _MOC = 90
                _minOCH = 120
                _minVisibilityDistance = 1900
                _maxDescentGradient = 200
                _finalSegmentIAS = 185
                _minFinalSegmentIAS = 130
                _maxFinalSegmentIAS = 185
            Case 1 'Cat B
                _corridorSemiWidth = 1500
                _MOC = 90
                _minOCH = 150
                _minVisibilityDistance = 2800
                _maxDescentGradient = 200
                _finalSegmentIAS = 240
                _minFinalSegmentIAS = 155
                _maxFinalSegmentIAS = 240
            Case 2 'Cat C
                _corridorSemiWidth = 1800
                _MOC = 120
                _minOCH = 180
                _minVisibilityDistance = 3700
                _maxDescentGradient = 305
                _finalSegmentIAS = 295
                _minFinalSegmentIAS = 215
                _maxFinalSegmentIAS = 295
            Case 3 'Cat D
                _corridorSemiWidth = 2100
                _MOC = 120
                _minOCH = 210
                _minVisibilityDistance = 4600
                _maxDescentGradient = 305
                _finalSegmentIAS = 345
                _minFinalSegmentIAS = 240
                _maxFinalSegmentIAS = 345
            Case 4 'Cat E
                _corridorSemiWidth = 2600
                _MOC = 150
                _minOCH = 240
                _minVisibilityDistance = 6500
                _maxDescentGradient = 305
                _finalSegmentIAS = 425
                _minFinalSegmentIAS = 285
                _maxFinalSegmentIAS = 425
        End Select
        _ConvexPoly = CType(convexPoly, Global.ESRI.ArcGIS.Geometry.IPolygon)
        GetArObstaclesByPoly(_convexPolyObstacleList, _ConvexPoly, fRefHeight) 'Time consuming task!
        _initialDirectionAngle = Functions.Azt2DirPrj(finalNav.pPtPrj, initialDirectionAngle)
        _maneuveringAreaPntColl = convexPoly
        _maneuveringAreaRelatOper = CType(_maneuveringAreaPntColl, IRelationalOperator)
        _TAS = TAS
        _ISA = 15
        _ahElev = AH_Elev
        _finalSegmentTime = 30
        _finalSegmentTAS = Functions.IAS2TAS(_finalSegmentIAS, _ahElev, _ISA + 15)
        _finalSegmentLength = Math.Round((_finalSegmentTime * _finalSegmentTAS * 1000 / 3600), 0)
        _selectedRWY = SelectedRWY
        _bankAngle = 25
        _turnRadius = Math.Round(Functions.Bank2Radius(_bankAngle, _TAS), 0)
        _maxVisibilityDistance = _minVisibilityDistance
        _maxDivergenceAngle = 45
        _maxConvergenceAngle = 90
        _bankEstablishTime = 5
        _bankEstablishDistance = Math.Round((_TAS * 1000 / 3600) * _bankEstablishTime, 0)
        _rwyData = RWYDATA

        Dim _distToPoly As Double
        If _maneuveringAreaRelatOper.Contains(finalNav.pPtPrj) Then
            _initialStartPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, finalNav.pPtPrj, Modulus(_initialDirectionAngle - 180, 360), _distToPoly, True)
        Else
            _initialStartPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, finalNav.pPtPrj, _initialDirectionAngle, _distToPoly, True)
            If _initialStartPnt Is Nothing Then
                _initialDirectionAngle = Modulus(_initialDirectionAngle - 180, 360)
                _initialStartPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, finalNav.pPtPrj, _initialDirectionAngle, _distToPoly, True)
            End If
        End If

        '_circlingBoxObstacles = New List(Of CirclingBoxObstacle)
        _leftCirclingBoxObstacles = New List(Of ObstacleAr)
        _rightCirclingBoxObstacles = New List(Of ObstacleAr)

        'If lbl_kmhSign2.Text.Equals("km/h") Then
        '    nmrcUpDown_FinalSegmentIAS.Minimum = _minFinalSegmentIAS
        '    nmrcUpDown_FinalSegmentIAS.Maximum = _maxFinalSegmentIAS
        'Else
        '    nmrcUpDown_FinalSegmentIAS.Minimum = KMH2KT(_minFinalSegmentIAS)
        '    nmrcUpDown_FinalSegmentIAS.Maximum = KMH2KT(_maxFinalSegmentIAS)
        'End If

        nmrcUpDown_FinalSegmentIAS.Minimum = ConvertSpeed(_minFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST)
        nmrcUpDown_FinalSegmentIAS.Maximum = ConvertSpeed(_maxFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST)

        nmrcUpDown_FinalSegmentTime.Minimum = 30
        nmrcUpDown_FinalSegmentTime.Maximum = 45

        txtBox_maxDivergenceAngle.Text = CStr(_maxDivergenceAngle)
        txtBox_maxConvergenceAngle.Text = CStr(_maxConvergenceAngle)
        txtBox_bankEstablishTime.Text = CStr(_bankEstablishTime)

        'If (lbl_meterSign1.Text.Equals("m")) Then
        '    txtBox_bankEstablishDistance.Text = CStr(_bankEstablishDistance)
        '    txtBox_minManeuverDistance.Text = CStr(_minManeuverDistance)
        '    txtBox_maxVisibilityDistance.Text = CStr(_maxVisibilityDistance)

        '    txtBox_finalSegmentLength.Text = CStr(_finalSegmentLength)
        '    txtBox_corridorSemiWidth.Text = CStr(_corridorSemiWidth)
        'Else
        '    txtBox_bankEstablishDistance.Text = CStr(M2NM(_bankEstablishDistance))
        '    txtBox_minManeuverDistance.Text = CStr(M2NM(_minManeuverDistance))
        '    txtBox_maxVisibilityDistance.Text = CStr(M2NM(_maxVisibilityDistance))

        '    txtBox_finalSegmentLength.Text = CStr(M2NM(_finalSegmentLength))
        '    txtBox_corridorSemiWidth.Text = CStr(M2NM(_corridorSemiWidth))
        'End If
        txtBox_bankEstablishDistance.Text = CStr(ConvertDistance(_bankEstablishDistance, eRoundMode.rmNONE))
        txtBox_minManeuverDistance.Text = CStr(ConvertDistance(_minManeuverDistance, eRoundMode.rmNONE))
        txtBox_maxVisibilityDistance.Text = CStr(ConvertDistance(_maxVisibilityDistance, eRoundMode.rmNONE))
        txtBox_finalSegmentLength.Text = CStr(ConvertDistance(_finalSegmentLength, eRoundMode.rmNONE))
        txtBox_corridorSemiWidth.Text = CStr(ConvertDistance(_corridorSemiWidth, eRoundMode.rmNONE))


        'If lbl_kmhSign2.Text.Equals("km/h") Then
        '    'lbl_finalSegmentIASrange.Text = "(" & _minFinalSegmentIAS & " - " & _maxFinalSegmentIAS & " km/h)"
        '    'nmrcUpDown_FinalSegmentIAS.Value = CDec(_finalSegmentIAS)
        '    lbl_finalSegmentIASrange.Text = "(" & ConvertSpeed(_minFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST) & " - " & ConvertSpeed(_maxFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST) & SpeedConverter(SpeedUnit).Unit
        '    nmrcUpDown_FinalSegmentIAS.Value = CDec(_finalSegmentIAS)
        'Else
        '    'lbl_finalSegmentIASrange.Text = "(" & KMH2KT(_minFinalSegmentIAS) & " - " & KMH2KT(_maxFinalSegmentIAS) & " Kt)"
        '    'nmrcUpDown_FinalSegmentIAS.Value = CDec(KMH2KT(_finalSegmentIAS))
        '    lbl_finalSegmentIASrange.Text = "(" & ConvertSpeed(_minFinalSegmentIAS, eRoundMode.rmNONE) & " - " & ConvertSpeed(_maxFinalSegmentIAS, eRoundMode.rmNONE) & " Kt)"
        '    nmrcUpDown_FinalSegmentIAS.Value = CDec(ConvertSpeed(_finalSegmentIAS, eRoundMode.rmNONE))
        'End If

        lbl_finalSegmentIASrange.Text = "(" & ConvertSpeed(_minFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST) & " - " & ConvertSpeed(_maxFinalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST) & SpeedConverter(SpeedUnit).Unit & ")"
        nmrcUpDown_FinalSegmentIAS.Value = CDec(ConvertSpeed(_finalSegmentIAS * 0.277777777777778, eRoundMode.rmNERAEST))

        nmrcUpDown_FinalSegmentTime.Value = CDec(_finalSegmentTime)

        'If lbl_meterSign9.Text.Equals("m") Then
        '    txtBox_rightBoxOCH.Text = CStr(Math.Round(rightBoxOCH, 0))
        '    txtBox_leftBoxOCH.Text = CStr(Math.Round(leftBoxOCH, 0))
        'Else
        '    txtBox_rightBoxOCH.Text = CStr(Meter2Feet(rightBoxOCH))
        '    txtBox_leftBoxOCH.Text = CStr(Meter2Feet(leftBoxOCH))
        'End If

        txtBox_rightBoxOCH.Text = CStr(ConvertHeight(rightBoxOCH, eRoundMode.rmNONE))
        txtBox_leftBoxOCH.Text = CStr(ConvertHeight(leftBoxOCH, eRoundMode.rmNONE))

    End Sub

    Private Sub DrawCirclingBoxes()
        Dim pntColl As IPointCollection
        Dim dirRWYTrueBearing As Double = Functions.Azt2DirPrj(_selectedRWY.pPtPrj(eRWY.PtTHR), _selectedRWY.TrueBearing)
        Dim pnt1 As IPoint
        Dim pnt2 As IPoint
        Dim pnt3 As IPoint
        Dim pnt4 As IPoint
        Dim tempPnt As IPoint
        Dim tempPnt2 As IPoint
        Dim centPnt As IPoint
		Dim distBetweenThrEnd As Double = Functions.ReturnDistanceFromGeomInMeters(_selectedRWY.pPtPrj(eRWY.PtTHR), _selectedRWY.pPtPrj(eRWY.PtEnd))

        'Draw final segment        
        _finalSegmentPolygon = Nothing
        _finalSegmentElement = Nothing
        _finalSegmentCentreline = Nothing
        _finalSegmentCentrelineElement = Nothing
        _finalSegmentPntColl = Nothing

        pnt1 = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing + 90, _corridorSemiWidth)
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 180, _finalSegmentLength)
        pnt3 = Functions.PointAlongPlane(pnt2, dirRWYTrueBearing - 90, 2 * _corridorSemiWidth)
        pnt4 = Functions.PointAlongPlane(pnt3, dirRWYTrueBearing, _finalSegmentLength)
        tempPnt = pnt3

        _finalSegmentPntColl = New Polygon()
        _finalSegmentPntColl.AddPoint(pnt1)
        _finalSegmentPntColl.AddPoint(pnt2)
        _finalSegmentPntColl.AddPoint(pnt3)
        _finalSegmentPntColl.AddPoint(pnt4)

        _finalSegmentPolygon = CType(_finalSegmentPntColl, IPolygon)
        If chkBox_showFinalSegment.Checked Then
            _finalSegmentElement = Functions.DrawPolygon(_finalSegmentPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
        End If

        pntColl = New Polyline()
        pntColl.AddPoint(_selectedRWY.pPtPrj(eRWY.PtTHR))
        tempPnt2 = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing - 180, _finalSegmentLength)
        pntColl.AddPoint(tempPnt2)

        _finalSegmentCentreline = CType(pntColl, IPolyline)
        If chkBox_showFinalSegmentCetreline.Checked Then
            _finalSegmentCentrelineElement = Functions.DrawPolyLine(_finalSegmentCentreline, Color.Green.ToArgb, 1.0, True)
        End If


        'Draw right box
        _rightBoxPolygon = Nothing
        _rightBoxPolygonElement = Nothing
        _rightBoxCentreline = Nothing
        _rightBoxCentrelineElement = Nothing
        _rightBoxPntColl = Nothing

        _rightBoxPntColl = New Polygon()

        pnt1 = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing + 90, _corridorSemiWidth)
        _rightBoxPntColl.AddPoint(pnt1)

        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 180, _finalSegmentLength)
        _rightBoxPntColl.AddPoint(pnt2)

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 90, 2 * _corridorSemiWidth + 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 90, _corridorSemiWidth + _turnRadius)
        _rightBoxPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, 1))

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing, distBetweenThrEnd + _finalSegmentLength)
        _rightBoxPntColl.AddPoint(pnt2)

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing + 90, 2 * _corridorSemiWidth + 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing + 90, _corridorSemiWidth + _turnRadius)
        _rightBoxPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, 1))

        _rightBoxPolygon = CType(_rightBoxPntColl, IPolygon)


        pntColl = New Polyline()

        tempPnt = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing - 180, _finalSegmentLength)
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing - 90, 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing - 90, _turnRadius)
        pntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, tempPnt, tempPnt2, 1))

        tempPnt = tempPnt2
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing, distBetweenThrEnd + _finalSegmentLength)
        pntColl.AddPoint(tempPnt2)

        tempPnt = tempPnt2
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing + 90, 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing + 90, _turnRadius)
        pntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, tempPnt, tempPnt2, 1))

        _rightBoxCentreline = CType(pntColl, IPolyline)


        'Draw left box
        _leftBoxPolygon = Nothing
        _leftBoxPolygonElement = Nothing
        _leftBoxCentreline = Nothing
        _leftBoxCentrelineElement = Nothing
        _leftBoxPntColl = Nothing

        _leftBoxPntColl = New Polygon()

        pnt1 = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing - 90, _corridorSemiWidth)
        _leftBoxPntColl.AddPoint(pnt1)

        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 180, _finalSegmentLength)
        _leftBoxPntColl.AddPoint(pnt2)

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing + 90, 2 * _corridorSemiWidth + 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing + 90, _corridorSemiWidth + _turnRadius)
        _leftBoxPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, -1))

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing, distBetweenThrEnd + _finalSegmentLength)
        _leftBoxPntColl.AddPoint(pnt2)

        pnt1 = pnt2
        pnt2 = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 90, 2 * _corridorSemiWidth + 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(pnt1, dirRWYTrueBearing - 90, _corridorSemiWidth + _turnRadius)
        _leftBoxPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, -1))

        _leftBoxPolygon = CType(_leftBoxPntColl, IPolygon)


        pntColl = New Polyline()
        tempPnt = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), dirRWYTrueBearing - 180, _finalSegmentLength)
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing + 90, 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing + 90, _turnRadius)
        pntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, tempPnt, tempPnt2, -1))

        tempPnt = tempPnt2
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing, distBetweenThrEnd + _finalSegmentLength)
        pntColl.AddPoint(tempPnt2)

        tempPnt = tempPnt2
        tempPnt2 = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing - 90, 2 * _turnRadius)
        centPnt = Functions.PointAlongPlane(tempPnt, dirRWYTrueBearing - 90, _turnRadius)
        pntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, tempPnt, tempPnt2, -1))

        _leftBoxCentreline = CType(pntColl, IPolyline)


        Dim pTopo As ITopologicalOperator2

        pTopo = CType(_leftBoxPolygon, ITopologicalOperator2)
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTopo = CType(_rightBoxPolygon, ITopologicalOperator2)
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        _rightBoxOCH = calculateCirclingBoxOCH(_rightBoxPntColl, _rightBoxHighestObstacleIndex, 1)
        _leftBoxOCH = calculateCirclingBoxOCH(_leftBoxPntColl, _leftBoxHighestObstacleIndex, -1)


        _bestCirclingBox = 0
        If rightBoxOCH < leftBoxOCH Then
            _bestCirclingBox = 1
        ElseIf leftBoxOCH < rightBoxOCH Then
            _bestCirclingBox = -1
        End If

        If chkBox_showRightBox.Checked Then
            If _bestCirclingBox >= 0 Then
                _rightBoxPolygonElement = Functions.DrawPolygon(_rightBoxPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
            Else
                _rightBoxPolygonElement = Functions.DrawPolygon(_rightBoxPolygon, Color.Purple.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
            End If
        End If

        If chkBox_showRightBoxCentreline.Checked Then
            If _bestCirclingBox >= 0 Then
                _rightBoxCentrelineElement = Functions.DrawPolyLine(_rightBoxCentreline, Color.Green.ToArgb, 1.0, True)
            Else
                _rightBoxCentrelineElement = Functions.DrawPolyLine(_rightBoxCentreline, Color.Purple.ToArgb, 1.0, True)
            End If
        End If

        If chkBox_showLeftBox.Checked Then
            If _bestCirclingBox <= 0 Then
                _leftBoxPolygonElement = Functions.DrawPolygon(_leftBoxPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
            Else
                _leftBoxPolygonElement = Functions.DrawPolygon(_leftBoxPolygon, Color.Purple.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
            End If
        End If

        If chkbox_showLeftBoxCetreline.Checked Then
            If _bestCirclingBox <= 0 Then
                _leftBoxCentrelineElement = Functions.DrawPolyLine(_leftBoxCentreline, Color.Green.ToArgb, 1.0, True)
            Else
                _leftBoxCentrelineElement = Functions.DrawPolyLine(_leftBoxCentreline, Color.Purple.ToArgb, 1.0, True)
            End If
        End If
    End Sub

    Public Sub DeleteCirclingBoxes()
        If _finalSegmentElement IsNot Nothing Then
            pGraphics.DeleteElement(_finalSegmentElement)
            _finalSegmentElement = Nothing
        End If
        If _finalSegmentCentrelineElement IsNot Nothing Then
            pGraphics.DeleteElement(_finalSegmentCentrelineElement)
            _finalSegmentCentrelineElement = Nothing
        End If
        If _rightBoxPolygonElement IsNot Nothing Then
            pGraphics.DeleteElement(_rightBoxPolygonElement)
            _rightBoxPolygonElement = Nothing
        End If
        If _rightBoxCentrelineElement IsNot Nothing Then
            pGraphics.DeleteElement(_rightBoxCentrelineElement)
            _rightBoxCentrelineElement = Nothing
        End If
        If _leftBoxPolygonElement IsNot Nothing Then
            pGraphics.DeleteElement(_leftBoxPolygonElement)
            _leftBoxPolygonElement = Nothing
        End If
        If _leftBoxCentrelineElement IsNot Nothing Then
            pGraphics.DeleteElement(_leftBoxCentrelineElement)
            _leftBoxCentrelineElement = Nothing
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Function calculateCirclingBoxOCH(circlingBoxPntColl As IPointCollection, ByRef maxHeightObstacleIndex As Integer, circlingBoxSide As Integer) As Double
        Dim _circlingBoxRelatOper As IRelationalOperator = CType(circlingBoxPntColl, IRelationalOperator)
        Dim maxHeight As Double = -100000
        'Dim tempCirclingBoxObstacle As CirclingBoxObstacle

        maxHeightObstacleIndex = 0

        For I As Integer = 0 To _convexPolyObstacleList.Length - 1
			If _circlingBoxRelatOper.Contains(_convexPolyObstacleList(I).pGeomPrj) Then
				'tempCirclingBoxObstacle = New CirclingBoxObstacle
				'tempCirclingBoxObstacle.Obstacle = _convexPolyObstacleList(I)
				'tempCirclingBoxObstacle.circlingBoxSide = circlingBoxSide
				'_circlingBoxObstacles.Add(tempCirclingBoxObstacle)
				If circlingBoxSide = 1 Then
					_rightCirclingBoxObstacles.Add(_convexPolyObstacleList(I))
				Else 'circlingBoxSide = -1
					_leftCirclingBoxObstacles.Add(_convexPolyObstacleList(I))
				End If

				If _convexPolyObstacleList(I).Height > maxHeight Then
					maxHeight = _convexPolyObstacleList(I).Height
					maxHeightObstacleIndex = I
				End If
			End If
        Next

        Dim OCH As Double = maxHeight + _MOC
        If OCH < _minOCH Then
            OCH = _minOCH
        End If
        Return OCH
    End Function

    Private Sub MaxDivergenceAngleTxtBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_maxDivergenceAngle.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            MaxDivergenceAngleTxtBox_Validating(txtBox_maxDivergenceAngle, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_maxDivergenceAngle.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub MaxDivergenceAngleTxtBox_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_maxDivergenceAngle.Validating
        If Not IsNumeric(txtBox_maxDivergenceAngle.Text) Then Return
        If CDbl(txtBox_maxDivergenceAngle.Text) > 45 Then
            txtBox_maxDivergenceAngle.Text = CStr(45)
        End If
        If CDbl(txtBox_maxDivergenceAngle.Text) < 0 Then
            txtBox_maxDivergenceAngle.Text = CStr(0)
        End If
    End Sub

    Private Sub MaxConvergenceAngleTxtBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_maxConvergenceAngle.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            MaxConvergenceAngleTxtBox_Validating(txtBox_maxConvergenceAngle, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_maxConvergenceAngle.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub MaxConvergenceAngleTxtBox_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_maxConvergenceAngle.Validating
        If Not IsNumeric(txtBox_maxConvergenceAngle.Text) Then Return
        If CDbl(txtBox_maxConvergenceAngle.Text) > 90 Then
            txtBox_maxConvergenceAngle.Text = CStr(90)
        End If
        If CDbl(txtBox_maxConvergenceAngle.Text) < 0 Then
            txtBox_maxConvergenceAngle.Text = CStr(0)
        End If
    End Sub

    Private Sub StabilizationTimeTxtBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_bankEstablishTime.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            StabilizationTimeTxtBox_Validating(txtBox_bankEstablishTime, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_bankEstablishTime.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub StabilizationTimeTxtBox_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_bankEstablishTime.Validating
        If Not IsNumeric(txtBox_bankEstablishTime.Text) Then Return
        If CDbl(txtBox_bankEstablishTime.Text) > 10 Then
            txtBox_bankEstablishTime.Text = CStr(10)
        End If
    End Sub

    Private Sub MaxVisibilityDistanceTxtBox_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtBox_maxVisibilityDistance.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            MaxVisibilityDistanceTxtBox_Validating(txtBox_maxVisibilityDistance, New System.ComponentModel.CancelEventArgs(False))
        Else
            TextBoxFloat(KeyAscii, (txtBox_maxVisibilityDistance.Text))
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub MaxVisibilityDistanceTxtBox_Validating(ByVal eventSender As System.Object, ByVal eventArgs As System.ComponentModel.CancelEventArgs) Handles txtBox_maxVisibilityDistance.Validating
        If Not IsNumeric(txtBox_maxVisibilityDistance.Text) Then Return
        Dim dist As Double = 0
        'If (lbl_meterSign1.Text.Equals("m")) Then
        '    dist = CDbl(txtBox_maxVisibilityDistance.Text)
        '    If dist < _minVisibilityDistance Then
        '        txtBox_maxVisibilityDistance.Text = CStr(_minVisibilityDistance)
        '    End If
        'Else
        '    dist = Feet2Meter(CDbl(txtBox_maxVisibilityDistance.Text))
        '    If dist < _minVisibilityDistance Then
        '        txtBox_maxVisibilityDistance.Text = CStr(Meter2Feet(_minVisibilityDistance))
        '    End If
        'End If

        dist = DeConvertDistance(CDbl(txtBox_maxVisibilityDistance.Text))
        If dist < _minVisibilityDistance Then
            txtBox_maxVisibilityDistance.Text = CStr(ConvertDistance(_minVisibilityDistance, eRoundMode.rmNONE))
        End If
    End Sub

    Private Sub txtBox_maxDivergenceAngle_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBox_maxDivergenceAngle.TextChanged
        _maxDivergenceAngle = CDbl(txtBox_maxDivergenceAngle.Text)
        _maxDivergenceDistance = Math.Sqrt(Math.Pow(_turnRadius, 2) * (1 - Math.Cos(DegToRad(_maxDivergenceAngle))) / (1 + Math.Cos(DegToRad(_maxDivergenceAngle))))
        _minManeuverDistance = Math.Round(_maxDivergenceDistance + _maxConvergenceDistance + 2 * _bankEstablishDistance, 0)
        'If (lbl_meterSign2.Text.Equals("m")) Then
        '    txtBox_minManeuverDistance.Text = CStr(_minManeuverDistance)
        'Else
        '    txtBox_minManeuverDistance.Text = CStr(M2NM(_minManeuverDistance))
        'End If

        txtBox_minManeuverDistance.Text = CStr(ConvertDistance(_minManeuverDistance, eRoundMode.rmNONE))
    End Sub

    Private Sub txtBox_maxConvergenceAngle_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBox_maxConvergenceAngle.TextChanged
        _maxConvergenceAngle = CDbl(txtBox_maxConvergenceAngle.Text)
        _maxConvergenceDistance = Math.Sqrt(Math.Pow(_turnRadius, 2) * (1 - Math.Cos(DegToRad(_maxConvergenceAngle))) / (1 + Math.Cos(DegToRad(_maxConvergenceAngle))))
        _minManeuverDistance = Math.Round(_maxDivergenceDistance + _maxConvergenceDistance + _bankEstablishDistance, 0)
        'If (lbl_meterSign2.Text.Equals("m")) Then
        '    txtBox_minManeuverDistance.Text = CStr(_minManeuverDistance)
        'Else
        '    txtBox_minManeuverDistance.Text = CStr(M2NM(_minManeuverDistance))
        'End If

        txtBox_minManeuverDistance.Text = CStr(ConvertDistance(_minManeuverDistance, eRoundMode.rmNONE))
    End Sub

    Private Sub txtBox_bankEstablishTime_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBox_bankEstablishTime.TextChanged
        _bankEstablishTime = CDbl(txtBox_bankEstablishTime.Text)
        _bankEstablishDistance = Math.Round((_TAS * 1000 / 3600) * _bankEstablishTime, 0)
        _minManeuverDistance = Math.Round(_maxDivergenceDistance + _maxConvergenceDistance + _bankEstablishDistance, 0)

        'If (lbl_meterSign8.Text.Equals("m")) Then
        '    txtBox_minManeuverDistance.Text = CStr(_minManeuverDistance)
        '    txtBox_bankEstablishDistance.Text = CStr(_bankEstablishDistance)
        'Else
        '    txtBox_minManeuverDistance.Text = CStr(M2NM(_minManeuverDistance))
        '    txtBox_bankEstablishDistance.Text = CStr(M2NM(_bankEstablishDistance))
        'End If

        txtBox_minManeuverDistance.Text = CStr(ConvertDistance(_minManeuverDistance, eRoundMode.rmNONE))
        txtBox_bankEstablishDistance.Text = CStr(ConvertDistance(_bankEstablishDistance, eRoundMode.rmNONE))
    End Sub

    Private Sub txtBox_maxVisibilityDistance_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBox_maxVisibilityDistance.TextChanged
        'If (lbl_meterSign1.Text.Equals("m")) Then
        '    _maxVisibilityDistance = CDbl(txtBox_maxVisibilityDistance.Text)
        'Else
        '    _maxVisibilityDistance = NM2M(CDbl(txtBox_maxVisibilityDistance.Text))
        'End If

        _maxVisibilityDistance = CDbl(ConvertDistance(DeConvertDistance(CDbl(txtBox_maxVisibilityDistance.Text)), eRoundMode.rmNONE))
    End Sub

    Private Sub chkBox_showRightBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showRightBox.CheckedChanged
        If chkBox_showRightBox.Checked Then
            If _rightBoxPolygonElement Is Nothing And _rightBoxPolygon IsNot Nothing Then
                If _bestCirclingBox >= 0 Then
                    _rightBoxPolygonElement = Functions.DrawPolygon(_rightBoxPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
                Else
                    _rightBoxPolygonElement = Functions.DrawPolygon(_rightBoxPolygon, Color.Purple.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
                End If
            End If
        Else
            If _rightBoxPolygonElement IsNot Nothing Then
                pGraphics.DeleteElement(_rightBoxPolygonElement)
                _rightBoxPolygonElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub chkBox_showLeftBox_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showLeftBox.CheckedChanged
        If chkBox_showLeftBox.Checked Then
            If _leftBoxPolygonElement Is Nothing And _leftBoxPolygon IsNot Nothing Then
                If _bestCirclingBox <= 0 Then
                    _leftBoxPolygonElement = Functions.DrawPolygon(_leftBoxPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
                Else
                    _leftBoxPolygonElement = Functions.DrawPolygon(_leftBoxPolygon, Color.Purple.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
                End If
            End If
        Else
            If _leftBoxPolygonElement IsNot Nothing Then
                pGraphics.DeleteElement(_leftBoxPolygonElement)
                _leftBoxPolygonElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub chkBox_showFinalSegment_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showFinalSegment.CheckedChanged
        If chkBox_showFinalSegment.Checked Then
            If _finalSegmentElement Is Nothing And _finalSegmentPolygon IsNot Nothing Then
                _finalSegmentElement = Functions.DrawPolygon(_finalSegmentPolygon, Color.Green.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
            End If
        Else
            If _finalSegmentElement IsNot Nothing Then
                pGraphics.DeleteElement(_finalSegmentElement)
                _finalSegmentElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub chkBox_showRightBoxCentreline_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showRightBoxCentreline.CheckedChanged
        If chkBox_showRightBoxCentreline.Checked Then
            If _rightBoxCentrelineElement Is Nothing And _rightBoxCentreline IsNot Nothing Then
                If _bestCirclingBox >= 0 Then
                    _rightBoxCentrelineElement = Functions.DrawPolyLine(_rightBoxCentreline, Color.Green.ToArgb, 1.0, True)
                Else
                    _rightBoxCentrelineElement = Functions.DrawPolyLine(_rightBoxCentreline, Color.Purple.ToArgb, 1.0, True)
                End If
            End If
        Else
            If _rightBoxCentrelineElement IsNot Nothing Then
                pGraphics.DeleteElement(_rightBoxCentrelineElement)
                _rightBoxCentrelineElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub chkbox_showLeftBoxCetreline_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkbox_showLeftBoxCetreline.CheckedChanged
        If chkbox_showLeftBoxCetreline.Checked Then
            If _leftBoxCentrelineElement Is Nothing And _leftBoxCentreline IsNot Nothing Then
                If _bestCirclingBox <= 0 Then
                    _leftBoxCentrelineElement = Functions.DrawPolyLine(_leftBoxCentreline, Color.Green.ToArgb, 1.0, True)
                Else
                    _leftBoxCentrelineElement = Functions.DrawPolyLine(_leftBoxCentreline, Color.Purple.ToArgb, 1.0, True)
                End If
            End If
        Else
            If _leftBoxCentrelineElement IsNot Nothing Then
                pGraphics.DeleteElement(_leftBoxCentrelineElement)
                _leftBoxCentrelineElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub chkBox_showFinalSegmentCetreline_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showFinalSegmentCetreline.CheckedChanged
        If chkBox_showFinalSegmentCetreline.Checked Then
            If _finalSegmentCentrelineElement Is Nothing And _finalSegmentCentreline IsNot Nothing Then
                _finalSegmentCentrelineElement = Functions.DrawPolyLine(_finalSegmentCentreline, Color.Green.ToArgb, 1.0, True)
            End If
        Else
            If _finalSegmentCentrelineElement IsNot Nothing Then
                pGraphics.DeleteElement(_finalSegmentCentrelineElement)
                _finalSegmentCentrelineElement = Nothing
            End If
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Private Sub btn_addVisRefPnts_Click(sender As System.Object, e As System.EventArgs) Handles btn_addVisRefPnts.Click
        If _newAddVisualReferencePoints Is Nothing OrElse _newAddVisualReferencePoints.isClosed Then
            _newAddVisualReferencePoints = New AddVisualReferencePoints
            _newAddVisualReferencePoints.Show(s_Win32Window)
        End If
    End Sub

    Private Sub btn_drawTrack_Click(sender As System.Object, e As System.EventArgs) Handles btn_drawTrack.Click
        If _visManTrackConstructor Is Nothing OrElse _visManTrackConstructor.isClosed Then
            _visManTrackConstructor = New VisualManoeuvringTrackConstructor(_maneuveringAreaPntColl)
            _visManTrackConstructor.trackConstructor.SetParams(_maneuveringAreaPntColl, _convexPolyObstacleList, _corridorSemiWidth,
                                                               _maxVisibilityDistance, _minManeuverDistance, _bankAngle, _TAS, _turnRadius,
                                                               _maxConvergenceAngle, _maxDivergenceAngle, _maxConvergenceDistance, _maxDivergenceDistance,
                                                               Math.Round(_initialDirectionAngle, 0), _initialStartPnt, _bankEstablishDistance, _finalSegmentLength, _selectedRWY,
                                                               _rwyData, _MOC, _minOCH, rightBoxOCH, leftBoxOCH, _rightBoxHighestObstacleIndex, _leftBoxHighestObstacleIndex, chkBox_showTrackBuffer.Checked)
            _visManTrackConstructor.Show(s_Win32Window)
        End If
    End Sub

    Private Sub btn_VisManReport_Click(sender As System.Object, e As System.EventArgs) Handles btn_VisManReport.Click
        If _visManReport Is Nothing OrElse _visManReport.isClosed Then
            _visManReport = New VisualManoeuvringReport
            _visManReport.FillPageReferensePoints(VisualManoeuvringDBTool.GetVisualReferencePoints())
            _visManReport.FillPageCirclingAreaObstacles(_convexPolyObstacleList, _MOC)
            _visManReport.FillPageLeftCirclingBoxObstacles(_leftCirclingBoxObstacles, _MOC)
            _visManReport.FillPageRightCirclingBoxObstacles(_rightCirclingBoxObstacles, _MOC)
            If _visManTrackConstructor IsNot Nothing Then
                If _visManTrackConstructor.trackConstructor IsNot Nothing Then
                    _visManReport.FillPageTrackSteps(_visManTrackConstructor.trackConstructor.trackSteps, _maxDivergenceAngle, _maxConvergenceAngle, _minManeuverDistance, _maxVisibilityDistance)
                    _visManReport.FillPageTrackObstacles(_visManTrackConstructor.trackConstructor.trackStepObstacles, _MOC)
                    _visManReport.FillPageTrackStatistics(_visManTrackConstructor.trackConstructor.trackOCH, _visManTrackConstructor.trackConstructor.CalcualteTrackLength(), _finalSegmentLength, _visManTrackConstructor.trackConstructor.CalculateFinalSegmentDescentGradient(), _visManTrackConstructor.trackConstructor.CalculateDownwindLegLength())
                End If
            End If
            _visManReport.Show(s_Win32Window)
        End If
    End Sub

    Private Sub chkBox_showTrackBuffer_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBox_showTrackBuffer.CheckedChanged
        If _visManTrackConstructor IsNot Nothing AndAlso _visManTrackConstructor.trackConstructor IsNot Nothing Then
            If chkBox_showTrackBuffer.Checked Then
                _visManTrackConstructor.trackConstructor.RedrawTrackBuffer()
                _visManTrackConstructor.trackConstructor.showTrackBuffer = True
            Else
                _visManTrackConstructor.trackConstructor.DeleteTrackBuffer()
                _visManTrackConstructor.trackConstructor.showTrackBuffer = False
            End If
        End If
    End Sub

    Private Sub nmrcUpDown_FinalSegmentIAS_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_FinalSegmentIAS.ValueChanged
        'If lbl_kmhSign2.Text.Equals("km/h") Then
        '    _finalSegmentIAS = nmrcUpDown_FinalSegmentIAS.Value
        'Else
        '    _finalSegmentIAS = KT2KMH(nmrcUpDown_FinalSegmentIAS.Value)
        'End If

        _finalSegmentIAS = ConvertSpeed(DeConvertSpeed(nmrcUpDown_FinalSegmentIAS.Value), eRoundMode.rmNERAEST)

        _finalSegmentTAS = Functions.IAS2TAS(_finalSegmentIAS, _ahElev, _ISA + 15)
        _finalSegmentLength = Math.Round((_finalSegmentTime * _finalSegmentTAS * 1000 / 3600), 0)
        txtBox_finalSegmentLength.Text = CStr(_finalSegmentLength)
        DeleteCirclingBoxes()
        DrawCirclingBoxes()
    End Sub

    Private Sub nmrcUpDown_FinalSegmentTime_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nmrcUpDown_FinalSegmentTime.ValueChanged
        _finalSegmentTime = nmrcUpDown_FinalSegmentTime.Value
        _finalSegmentLength = Math.Round((_finalSegmentTime * _finalSegmentTAS * 1000 / 3600), 0)
        txtBox_finalSegmentLength.Text = CStr(_finalSegmentLength)
        DeleteCirclingBoxes()
        DrawCirclingBoxes()
    End Sub

    'Private Function KMH2KT(kmh As Double) As Double
    '    Return Math.Round(kmh * 1.852, 0)
    'End Function

    'Private Function KT2KMH(Kt As Double) As Double
    '    Return Math.Round(Kt / 1.852, 0)
    'End Function

    'Private Function Meter2Feet(m As Double) As Double
    '    Return Math.Round(m / 1852, 3)
    'End Function

    'Private Function Feet2Meter(ft As Double) As Double
    '    Return Math.Round(ft * 1852, 0)
    'End Function

    'Private Function KM2NM(km As Double) As Double
    '    Return Math.Round(km * 1.852, 3)
    'End Function

    'Private Function NM2KM(NM As Double) As Double
    '    Return Math.Round(NM / 1.852, 3)
    'End Function

    'Private Function M2NM(m As Double) As Double
    '    Return KM2NM(m / 1000)
    'End Function

    'Private Function NM2M(NM As Double) As Double
    '    Return NM2KM(NM) * 1000
    'End Function
End Class
