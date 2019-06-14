Option Strict Off

Imports ESRI.ArcGIS.Geometry
Imports System.Collections.Generic
Imports ESRI.ArcGIS.Carto

Friend Class TrackConstructor
    Private _startPrjPnt As IPoint
    Private _startPntPrjAngle As Double
    Private _targetPrjPnt As IPoint
    Private _targetPntPrjAngle As Double
    Private _divergencePnt As IPoint
    Private _convergencePnt As IPoint
    Private _divergenceAngle As Double
    Private _convergenceAngle As Double

    Private _trackStepCentrelinePntColl As IPointCollection
    Private _trackStepCentreline As IPolyline
    Private _trackStepPntColl As IPointCollection
    Private _trackStepPolygon As IPolygon
    Private _trackStepCentrelineElement As IElement
    Private _trackStepPolygonElement As IElement

    Private _trackStepMapElements As List(Of TrackStepMapElement)
    Private _trackSteps As List(Of TrackStep)
    Public Property trackSteps() As List(Of TrackStep)
        Get
            Return _trackSteps
        End Get
        Set(ByVal value As List(Of TrackStep))
            _trackSteps = value
        End Set
    End Property
    Private _trackSteps_2 As List(Of TrackStep_2)
    Public Property trackSteps_2() As List(Of TrackStep_2)
        Get
            Return _trackSteps_2
        End Get
        Set(ByVal value As List(Of TrackStep_2))
            _trackSteps_2 = value
        End Set
    End Property

    Private _trackStepObstacles As List(Of ObstacleAr)
    Public Property trackStepObstacles() As List(Of ObstacleAr)
        Get
            Return _trackStepObstacles
        End Get
        Set(ByVal value As List(Of ObstacleAr))
            _trackStepObstacles = value
        End Set
    End Property
    Private _initialStartPrjPnt As IPoint
    Private _initialPntPrjAngle As Double
    Private _allVisualReferencePoints As List(Of VisualReferencePoint)
    'Private _allVisualReferencePoints As List(Of Aran.Aim.Features.AeronauticalGroundLight)
    Private _divergenceSegmentVisualReferencePoints As List(Of VisualReferencePoint)
    Public Property DivergenceSegmentVisualReferencePoints() As List(Of VisualReferencePoint)
        Get
            Return _divergenceSegmentVisualReferencePoints
        End Get
        Set(ByVal value As List(Of VisualReferencePoint))
            _divergenceSegmentVisualReferencePoints = value
        End Set
    End Property

    'Private _divergenceSegmentVisualReferencePoints As List(Of Aim.Features.AeronauticalGroundLight)
    'Public Property DivergenceSegmentVisualReferencePoints() As List(Of Aim.Features.AeronauticalGroundLight)
    '    Get
    '        Return _divergenceSegmentVisualReferencePoints
    '    End Get
    '    Set(ByVal value As List(Of Aim.Features.AeronauticalGroundLight))
    '        _divergenceSegmentVisualReferencePoints = value
    '    End Set
    'End Property

    Private _convergenceSegmentVisualReferencePoints As List(Of VisualReferencePoint)
    Public Property ConvergenceSegmentVisualReferencePoints() As List(Of VisualReferencePoint)
        Get
            Return _convergenceSegmentVisualReferencePoints
        End Get
        Set(ByVal value As List(Of VisualReferencePoint))
            _convergenceSegmentVisualReferencePoints = value
        End Set
    End Property

    'Private _convergenceSegmentVisualReferencePoints As List(Of Aim.Features.AeronauticalGroundLight)
    'Public Property ConvergenceSegmentVisualReferencePoints() As List(Of Aim.Features.AeronauticalGroundLight)
    '    Get
    '        Return _convergenceSegmentVisualReferencePoints
    '    End Get
    '    Set(ByVal value As List(Of Aim.Features.AeronauticalGroundLight))
    '        _convergenceSegmentVisualReferencePoints = value
    '    End Set
    'End Property

    Private _startTargetAngleDiff As Double
    Public Property startTargetAngleDiff() As Double
        Get
            Return _startTargetAngleDiff
        End Get
        Set(ByVal value As Double)
            _startTargetAngleDiff = value
        End Set
    End Property

    Private _startPntElement As IElement
    Public Property startPntElement() As IElement
        Get
            Return _startPntElement
        End Get
        Set(ByVal value As IElement)
            _startPntElement = value
        End Set
    End Property
    Private _targetPntElement As IElement
    Public Property targetPntElement() As IElement
        Get
            Return _targetPntElement
        End Get
        Set(ByVal value As IElement)
            _targetPntElement = value
        End Set
    End Property
    Private _divergencePolyElement As IElement
    Public Property divergencePolyElement() As IElement
        Get
            Return _divergencePolyElement
        End Get
        Set(ByVal value As IElement)
            _divergencePolyElement = value
        End Set
    End Property
    Private _convergencePolyElement As IElement
    Public Property convergencePolyElement() As IElement
        Get
            Return _convergencePolyElement
        End Get
        Set(ByVal value As IElement)
            _convergencePolyElement = value
        End Set
    End Property
    Private _divergenceVisRefPntElement As IElement
    Public Property divergenceVisRefPntElement() As IElement
        Get
            Return _divergenceVisRefPntElement
        End Get
        Set(ByVal value As IElement)
            _divergenceVisRefPntElement = value
        End Set
    End Property
    Private _divergenceVisRefPntIndx As Integer
    Public Property divergenceVisRefPntIndx() As Integer
        Get
            Return _divergenceVisRefPntIndx
        End Get
        Set(ByVal value As Integer)
            _divergenceVisRefPntIndx = value
        End Set
    End Property
    Private _convergenceVisRefPntElement As IElement
    Public Property convergenceVisRefPntElement() As IElement
        Get
            Return _convergenceVisRefPntElement
        End Get
        Set(ByVal value As IElement)
            _convergenceVisRefPntElement = value
        End Set
    End Property
    Private _convergenceVisRefPntIndx As Integer
    Public Property convergenceVisRefPntIndx() As Integer
        Get
            Return _convergenceVisRefPntIndx
        End Get
        Set(ByVal value As Integer)
            _convergenceVisRefPntIndx = value
        End Set
    End Property
    'Private _pickableAreaElement As IElement
    'Public Property pickableAreaElement() As IElement
    '    Get
    '        Return _pickableAreaElement
    '    End Get
    '    Set(ByVal value As IElement)
    '        _pickableAreaElement = value
    '    End Set
    'End Property
    Private _pickableAreaElements As IGroupElement
    Public Property pickableAreaElements() As IGroupElement
        Get
            Return _pickableAreaElements
        End Get
        Set(ByVal value As IGroupElement)
            _pickableAreaElements = value
        End Set
    End Property


    Private _color As Integer = Color.Black.ToArgb
    Private _pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
    Public Property pGraphics() As ESRI.ArcGIS.Carto.IGraphicsContainer
        Get
            Return _pGraphics
        End Get
        Set(ByVal value As ESRI.ArcGIS.Carto.IGraphicsContainer)
            _pGraphics = value
        End Set
    End Property
    Private _showTrackBuffer As Boolean
    Public Property showTrackBuffer() As Boolean
        Get
            Return _showTrackBuffer
        End Get
        Set(ByVal value As Boolean)
            _showTrackBuffer = value
        End Set
    End Property
    Private _trackHighestObstacleElement As IElement


    Dim pTopo As ITopologicalOperator2

    Private _maneuveringAreaPntColl As IPointCollection
    Private _obstacleList() As ObstacleAr
    Private _maneuveringAreaRelatOper As ESRI.ArcGIS.Geometry.IRelationalOperator
    Private _corridorSemiWidth As Double
    Private _bufferWidth As Double
    Private _maxVisibilityDistance As Double
    Private _bankAngle As Double
    Private _TAS As Double
    Private _turnRadius As Double
    Private _maxConvergenceAngle As Double
    Private _maxDivergenceAngle As Double
    Private _minPrePntTime As Double
    Private _minPrePntDist As Double
    Private _maxConvergenceDist As Double
    Private _maxDivergenceDist As Double
    Private _maxDistanceToTargetPnt As Double
    Private _bankEstablishDistance As Double
    Private _finalSegmentLength As Double
    Private _selectedRWY As RWYType
    Private _rwydata() As RWYType
    Private _MOC As Double
    Private _minOCH As Double
    Private _rightBoxOCH As Double
    Private _leftBoxOCH As Double
    Private _rightBoxHighestObstacleIndex As Integer
    Private _leftBoxHighestObstacleIndex As Integer

    Private _trackOCH As Double
    Public Property trackOCH() As Double
        Get
            Return _trackOCH
        End Get
        Set(value As Double)

        End Set
    End Property


    Dim _initialSegmentCentrelinePntColl As IPointCollection = Nothing
    Dim _initialSegmentCentreline As IPolyline = Nothing
    Dim _initialSegmentCentrelineElement As IElement = Nothing
    Dim _initialSegmentPntColl As IPointCollection = Nothing
    Dim _initialSegmentPolygon As IPolygon = Nothing
    Dim _initialSegmentPolygonElement As IElement = Nothing

    Dim _intermediateSegmentCentrelinePntColl As IPointCollection = Nothing
    Dim _intermediateSegmentCentreline As IPolyline = Nothing
    Dim _intermediateSegmentCentrelineElement As IElement = Nothing
    Dim _intermediateSegmentPntColl As IPointCollection = Nothing
    Dim _intermediateSegmentPolygon As IPolygon = Nothing
    Dim _intermediateSegmentPolygonElement As IElement = Nothing

    Dim _finalSegmentCentrelinePntColl As IPointCollection = Nothing
    Dim _finalSegmentCentreline As IPolyline = Nothing
    Dim _finalSegmentCentrelineElement As IElement = Nothing
    Dim _finalSegmentPntColl As IPointCollection = Nothing
    Dim _finalSegmentPolygon As IPolygon = Nothing
    Dim _finalSegmentPolygonElement As IElement = Nothing


    Dim leftPolygon As IPolygon
    Dim rightPolygon As IPolygon

    Dim isNewTHRPnt As Boolean = False

    Public Sub New()
        _trackSteps = New List(Of TrackStep)
        _trackStepObstacles = New List(Of ObstacleAr)
        _trackStepMapElements = New List(Of TrackStepMapElement)
        '_trackStepMapElements2 = New List(Of TrackStepMapElement2)
        _pGraphics = GetActiveView().GraphicsContainer
    End Sub

    Public Sub SetParams(convexPoly As IPointCollection, obstacleList() As ObstacleAr, corridorSemiWidth As Double,
                         maxVisibilityDist As Double, minManeuverDist As Double, bankAngle As Double, TAS As Double, turnRadius As Double,
                         maxConvergenceAngle As Double, maxDivergenceAngle As Double, maxConvergenceDist As Double, maxDivergenceDist As Double,
                         initialDirectionAngle As Double, initialStartPnt As IPoint, bankEstablishDist As Double, finalSegmentLength As Double,
                         selectedRWY As RWYType, RWYDATA() As RWYType, MOC As Double, minOCH As Double, rightBoxOCH As Double, leftBoxOCH As Double,
                         rightBoxHighestObstacleIndex As Integer, leftBoxHighestObstacleIndex As Integer, showBuffer As Boolean)
        _maneuveringAreaPntColl = convexPoly
        _obstacleList = obstacleList
        _corridorSemiWidth = corridorSemiWidth
        _maxVisibilityDistance = maxVisibilityDist
        _bankAngle = bankAngle
        _TAS = TAS
        _turnRadius = turnRadius
        _maxConvergenceAngle = maxConvergenceAngle
        _maxDivergenceAngle = maxDivergenceAngle
        _selectedRWY = selectedRWY
        _minPrePntDist = Math.Round(_minPrePntTime * _TAS * 1000 / 3600, 0)
        _maxConvergenceDist = maxConvergenceDist
        _maxDivergenceDist = maxDivergenceDist
        _initialPntPrjAngle = initialDirectionAngle
        _initialStartPrjPnt = initialStartPnt
        _bankEstablishDistance = bankEstablishDist
        _finalSegmentLength = finalSegmentLength
        _rwydata = RWYDATA
        _MOC = MOC
        _minOCH = minOCH
        _rightBoxOCH = rightBoxOCH - _MOC
        _leftBoxOCH = leftBoxOCH - _MOC
        _rightBoxHighestObstacleIndex = rightBoxHighestObstacleIndex
        _leftBoxHighestObstacleIndex = leftBoxHighestObstacleIndex
        _showTrackBuffer = showBuffer
        '_allVisualReferencePoints = VisualManoeuvringDBTool.GetVisualReferencePoints()

        _allVisualReferencePoints = VisualManoeuvringDBTool.GetVisualReferencePoints()


        Dim tempDist As Double
        For i As Integer = 0 To _rwydata.Length - 1
            Dim found As Boolean = False
            For j As Integer = 0 To _allVisualReferencePoints.Count - 1
                'tempDist = DistanceCalculator.CalcDistance(_rwydata(i).pPtGeo(eRWY.PtTHR).X, _rwydata(i).pPtGeo(eRWY.PtTHR).Y, _allVisualReferencePoints(j).gShape.X, _allVisualReferencePoints(j).gShape.Y)
                tempDist = DistanceCalculator.CalcDistance(_rwydata(i).pPtGeo(eRWY.PtTHR).X, _rwydata(i).pPtGeo(eRWY.PtTHR).Y, _allVisualReferencePoints(j).gShape.X, _allVisualReferencePoints(j).gShape.Y)
                If tempDist < 1 Then
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                CType(_rwydata(i).pPtGeo(eRWY.PtTHR), IZAware).ZAware = True
                'VisualManoeuvringDBTool.AddVisualReferencePoint(New VisualReferencePoint(_rwydata(i).pPtGeo(eRWY.PtTHR), _rwydata(i).pPtPrj(eRWY.PtTHR), "THR " + _rwydata(i).Name, "THR", _rwydata(i).Name))
                VisualManoeuvringDBTool.CreateVisualReferencePoint("THR " + _rwydata(i).Name, _rwydata(i).Name, _rwydata(i).pPtGeo(eRWY.PtTHR))
                isNewTHRPnt = True
            End If
        Next
        If isNewTHRPnt Then
            VisualManoeuvringDBTool.InsertVisualReferencePoints()
        End If

        _startPrjPnt = _initialStartPrjPnt
        _startPntPrjAngle = _initialPntPrjAngle
    End Sub

    Public Sub TargetPointDoneButtonClicked(targetPrjPnt As IPoint, targetPntPrjAngle As Double, isFinalStep As Boolean)
        If _trackSteps.Count = 0 Then
            _startPrjPnt = _initialStartPrjPnt
            _startPntPrjAngle = _initialPntPrjAngle
        Else
            _startPrjPnt = _trackSteps(_trackSteps.Count - 1).TargetPrjPoint
            _startPntPrjAngle = _trackSteps(_trackSteps.Count - 1).TargetPointPrjAngle
        End If

        _targetPrjPnt = targetPrjPnt
        _targetPntPrjAngle = targetPntPrjAngle
        _startTargetAngleDiff = Math.Abs(_startPntPrjAngle - _targetPntPrjAngle)

        '_allVisualReferencePoints = VisualManoeuvringDBTool.GetVisualReferencePoints()
        _allVisualReferencePoints = VisualManoeuvringDBTool.GetVisualReferencePoints()
        _divergenceSegmentVisualReferencePoints = New List(Of VisualReferencePoint)
        _convergenceSegmentVisualReferencePoints = New List(Of VisualReferencePoint)
        getDivergenceVisualReferencePoints()
        getConvergenceVisualReferencePoints()

        Dim angleDiff As Double
        Dim distToPnt As Double
        Dim distToPntProj As Double

        For i As Integer = 0 To _divergenceSegmentVisualReferencePoints.Count - 1
            angleDiff = Math.Abs(Functions.ReturnAngleInDegrees(_startPrjPnt, _divergenceSegmentVisualReferencePoints(i).pShape) - _startPntPrjAngle)
            distToPnt = Math.Sqrt(Math.Pow(_startPrjPnt.X - _divergenceSegmentVisualReferencePoints(i).pShape.X, 2) +
                                  Math.Pow(_startPrjPnt.Y - _divergenceSegmentVisualReferencePoints(i).pShape.Y, 2))
            distToPntProj = distToPnt * Math.Cos(Functions.DegToRad(angleDiff))
            _divergenceSegmentVisualReferencePoints(i).ProjPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, distToPntProj)
        Next

        For i As Integer = 0 To _convergenceSegmentVisualReferencePoints.Count - 1
            angleDiff = Math.Abs(Functions.ReturnAngleInDegrees(_targetPrjPnt, _convergenceSegmentVisualReferencePoints(i).pShape) - Modulus(_targetPntPrjAngle - 180, 360))
            distToPnt = Math.Sqrt(Math.Pow(_targetPrjPnt.X - _convergenceSegmentVisualReferencePoints(i).pShape.X, 2) +
                                  Math.Pow(_targetPrjPnt.Y - _convergenceSegmentVisualReferencePoints(i).pShape.Y, 2))
            distToPntProj = distToPnt * Math.Cos(Functions.DegToRad(angleDiff))
            _convergenceSegmentVisualReferencePoints(i).ProjPnt = Functions.PointAlongPlane(_targetPrjPnt, Modulus(_targetPntPrjAngle - 180, 360), distToPntProj)
        Next

        If _startPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_startPntElement)
            _startPntElement = Nothing
        End If
        _startPntElement = Functions.DrawPoint(_startPrjPnt, 255, True)
        If _targetPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_targetPntElement)
            _targetPntElement = Nothing
        End If
        _targetPntElement = Functions.DrawPoint(_targetPrjPnt, 255, True)
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
    End Sub

    Public Sub RemoveButtonCkicked()
        If _divergencePolyElement IsNot Nothing Then
            _pGraphics.DeleteElement(_divergencePolyElement)
            _divergencePolyElement = Nothing
        End If
        If _divergenceVisRefPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_divergenceVisRefPntElement)
            _divergenceVisRefPntElement = Nothing
        End If
        If _convergencePolyElement IsNot Nothing Then
            _pGraphics.DeleteElement(_convergencePolyElement)
            _convergencePolyElement = Nothing
        End If
        If _convergenceVisRefPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_convergenceVisRefPntElement)
            _convergenceVisRefPntElement = Nothing
        End If
        If _startPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_startPntElement)
            _startPntElement = Nothing
        End If
        If _targetPntElement IsNot Nothing Then
            _pGraphics.DeleteElement(_targetPntElement)
            _targetPntElement = Nothing
        End If
        If _trackStepMapElements.Count > 0 Then
            If _trackStepMapElements(_trackStepMapElements.Count - 1).initialSegmentCentrelineElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).initialSegmentCentrelineElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).initialSegmentPolygonElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).initialSegmentPolygonElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).intermediateSegmentCentrelineElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).intermediateSegmentCentrelineElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).intermediateSegmentPolygonElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).intermediateSegmentPolygonElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).finalSegmentCentrelineElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).finalSegmentCentrelineElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).finalSegmentPolygonElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).finalSegmentPolygonElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).startPntElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).startPntElement)
            End If
            If _trackStepMapElements(_trackStepMapElements.Count - 1).targetPntElement IsNot Nothing Then
                _pGraphics.DeleteElement(_trackStepMapElements(_trackStepMapElements.Count - 1).targetPntElement)
            End If

            _trackStepMapElements.RemoveAt(_trackStepMapElements.Count - 1)
            _trackSteps.RemoveAt(_trackSteps.Count - 1)
        End If
        If _trackHighestObstacleElement IsNot Nothing Then
            _pGraphics.DeleteElement(_trackHighestObstacleElement)
            _trackHighestObstacleElement = Nothing
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        If _trackHighestObstacleElement IsNot Nothing Then
            _pGraphics.DeleteElement(_trackHighestObstacleElement)
            _trackHighestObstacleElement = Nothing
        End If
        _trackOCH = CalculateTrackOCH()
    End Sub

    Private Sub getDivergenceVisualReferencePoints()
        If _divergencePolyElement IsNot Nothing Then
            _pGraphics.DeleteElement(_divergencePolyElement)
            _divergencePolyElement = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

        Dim distToPoly As Double
        Dim pnt As IPoint
        Dim pntColl As IPointCollection = New Polygon
        Dim farthestPnt As IPoint = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, _startPrjPnt, _startPntPrjAngle, distToPoly, False)

        pnt = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, _startPntPrjAngle, distToPoly)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, _startPntPrjAngle + 90, 2 * _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, Modulus(_startPntPrjAngle - 180, 360), distToPoly)
        pntColl.AddPoint(pnt)


        Dim poly As IPolygon = CType(pntColl, IPolygon)
        _divergencePolyElement = Functions.DrawPolygon(poly, 255, , True)



        pTopo = CType(poly, ITopologicalOperator2)
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        Dim relatOper As IRelationalOperator = CType(pntColl, IRelationalOperator)

        For i As Integer = 0 To _allVisualReferencePoints.Count - 1
            If relatOper.Contains(_allVisualReferencePoints(i).pShape) Then
                _divergenceSegmentVisualReferencePoints.Add(_allVisualReferencePoints(i))
            End If
            'If relatOper.Contains(ToPrj(_allVisualReferencePoints(i).Location.Geo)) Then
            '    _divergenceSegmentVisualReferencePoints.Add(_allVisualReferencePoints(i))
            'End If
        Next
    End Sub

    Private Sub getConvergenceVisualReferencePoints()
        If _convergencePolyElement IsNot Nothing Then
            _pGraphics.DeleteElement(_convergencePolyElement)
            _convergencePolyElement = Nothing
            GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

        Dim pnt As IPoint
        Dim pntColl As IPointCollection = New Polygon

        pnt = Functions.PointAlongPlane(_targetPrjPnt, Modulus(_targetPntPrjAngle - 90, 360), _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, Modulus(_targetPntPrjAngle - 180, 360), _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, _targetPntPrjAngle + 90, 2 * _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        pnt = Functions.PointAlongPlane(pnt, _targetPntPrjAngle, _maxVisibilityDistance)
        pntColl.AddPoint(pnt)

        Dim poly As IPolygon = CType(pntColl, IPolygon)
        _convergencePolyElement = Functions.DrawPolygon(poly, 255, , True)

        Dim pTopo As ITopologicalOperator2

        pTopo = CType(poly, ITopologicalOperator2)
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        Dim relatOper As IRelationalOperator = CType(pntColl, IRelationalOperator)

        For i As Integer = 0 To _allVisualReferencePoints.Count - 1
            If relatOper.Contains(_allVisualReferencePoints(i).pShape) Then
                _convergenceSegmentVisualReferencePoints.Add(_allVisualReferencePoints(i))
            End If
        Next
    End Sub

    Public Sub GetSelectableConvergencePoints(isFinalStep As Boolean, ByRef pntNames As List(Of String))
        Dim tempDist As Double
        If isFinalStep Then
            For i As Integer = 0 To _convergenceSegmentVisualReferencePoints.Count - 1
                If _convergenceSegmentVisualReferencePoints(i).Name.Contains("THR") Then
                    tempDist = DistanceCalculator.CalcDistance(_convergenceSegmentVisualReferencePoints(i).gShape.X, _convergenceSegmentVisualReferencePoints(i).gShape.Y, _selectedRWY.pPtGeo(eRWY.PtTHR).X, _selectedRWY.pPtGeo(eRWY.PtTHR).Y)
                    If tempDist < 1 Then
                        '_convergenceSegmentVisualReferencePoints(i).ProjPnt = Functions.PointAlongPlane(_convergenceSegmentVisualReferencePoints(i).pShape, Modulus(Functions.Azt2DirPrj(_convergenceSegmentVisualReferencePoints(i).pShape, _selectedRWY.TrueBearing) - 180, 360), _finalSegmentLength)
                        pntNames.Add(_convergenceSegmentVisualReferencePoints(i).Name)
                        Exit For
                    End If
                End If
            Next
        Else
            For i As Integer = 0 To _convergenceSegmentVisualReferencePoints.Count - 1
                'If _convergenceSegmentVisualReferencePoints(i).Type = "THR" Then
                '    tempDist = DistanceCalculator.CalcDistance(_convergenceSegmentVisualReferencePoints(i).gShape.X, _convergenceSegmentVisualReferencePoints(i).gShape.Y, _selectedRWY.pPtGeo(eRWY.PtTHR).X, _selectedRWY.pPtGeo(eRWY.PtTHR).Y)
                '    If tempDist < 1 Then
                '        _convergenceSegmentVisualReferencePoints(i).ProjPnt = Functions.PointAlongPlane(_convergenceSegmentVisualReferencePoints(i).pShape, Modulus(Functions.Azt2DirPrj(_convergenceSegmentVisualReferencePoints(i).pShape, _selectedRWY.TrueBearing) - 180, 360), _finalSegmentLength)
                '    End If
                'End If
                pntNames.Add(_convergenceSegmentVisualReferencePoints(i).Name)
            Next
        End If
    End Sub

    Public Sub AddStepButtonClicked(isOnCirclingBox As Boolean, isFinalStep As Boolean, convergencePntName As String, divergencePntSelectedIndx As Integer)
        For i As Integer = 0 To _convergenceSegmentVisualReferencePoints.Count - 1
            If _convergenceSegmentVisualReferencePoints(i).Name = convergencePntName Then
                Dim tempDist As Double = DistanceCalculator.CalcDistance(_convergenceSegmentVisualReferencePoints(i).gShape.X, _convergenceSegmentVisualReferencePoints(i).gShape.Y, _selectedRWY.pPtGeo(eRWY.PtTHR).X, _selectedRWY.pPtGeo(eRWY.PtTHR).Y)
                If _convergenceSegmentVisualReferencePoints(i).Name.Contains("THR") AndAlso isFinalStep AndAlso tempDist < 1 Then
                    _convergenceSegmentVisualReferencePoints(i).ProjPnt = Functions.PointAlongPlane(_convergenceSegmentVisualReferencePoints(i).pShape, Modulus(Functions.Azt2DirPrj(_convergenceSegmentVisualReferencePoints(i).pShape, _selectedRWY.TrueBearing) - 180, 360), _finalSegmentLength)
                End If
                Dim divergencePnt As IPoint
                Dim convergencePnt As IPoint
                Dim side As Integer
                Dim intermediateSegmentAngle As Double
                If isOnCirclingBox And _startTargetAngleDiff > 178 And _startTargetAngleDiff < 182 Then
                    Dim angleDiff As Double
                    Dim distToPnt As Double

                    angleDiff = Math.Abs(Functions.ReturnAngleInDegrees(_startPrjPnt, _convergenceSegmentVisualReferencePoints(i).ProjPnt) - _startPntPrjAngle)
                    distToPnt = Math.Sqrt(Math.Pow(_convergenceSegmentVisualReferencePoints(i).ProjPnt.X - _startPrjPnt.X, 2) + Math.Pow(_convergenceSegmentVisualReferencePoints(i).ProjPnt.Y - _startPrjPnt.Y, 2))
                    Dim distToDivergencePnt As Double = distToPnt * Math.Cos(Functions.DegToRad(angleDiff))
                    divergencePnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, distToDivergencePnt)

                    side = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, _targetPrjPnt)
                    If side < 0 Then
                        convergencePnt = Functions.PointAlongPlane(divergencePnt, Modulus(_startPntPrjAngle + 90, 360), 2 * _turnRadius)
                    ElseIf side > 0 Then
                        convergencePnt = Functions.PointAlongPlane(divergencePnt, Modulus(_startPntPrjAngle - 90, 360), 2 * _turnRadius)
                    End If
                    intermediateSegmentAngle = Functions.ReturnAngleInDegrees(divergencePnt, convergencePnt)
                    _trackStepMapElements.Add(DrawStep(_startPrjPnt, divergencePnt, convergencePnt, _targetPrjPnt, _startPntPrjAngle, -1, _targetPntPrjAngle, side, isOnCirclingBox, isFinalStep, _convergenceSegmentVisualReferencePoints(i).Name))
                Else
                    If isFinalStep AndAlso _startTargetAngleDiff > 90 AndAlso _startTargetAngleDiff < 270 AndAlso _convergenceSegmentVisualReferencePoints(i).Name = _trackSteps(_trackSteps.Count - 1).ConvergenceVisualReferencePoint.Name Then
                        If _startTargetAngleDiff <> 180 Then
                            Dim angleDiff As Double = Math.Abs(_startPntPrjAngle - Modulus(_targetPntPrjAngle - 180, 360))
                            Dim distToPnt As Double = _turnRadius / Math.Tan(DegToRad(angleDiff) / 2)
                            divergencePnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, distToPnt)
                            convergencePnt = Functions.PointAlongPlane(_targetPrjPnt, Modulus(_targetPntPrjAngle - 180, 360), distToPnt)
                            intermediateSegmentAngle = Functions.ReturnAngleInDegrees(divergencePnt, convergencePnt)
                            If _startTargetAngleDiff < 180 Then
                                side = -1
                                _trackStepMapElements.Add(DrawStep(_startPrjPnt, divergencePnt, convergencePnt, _targetPrjPnt, _startPntPrjAngle, -1, _targetPntPrjAngle, side, isOnCirclingBox, isFinalStep, _convergenceSegmentVisualReferencePoints(i).Name))
                            ElseIf _startTargetAngleDiff > 180 Then
                                side = 1
                                _trackStepMapElements.Add(DrawStep(_startPrjPnt, divergencePnt, convergencePnt, _targetPrjPnt, _startPntPrjAngle, -1, _targetPntPrjAngle, side, isOnCirclingBox, isFinalStep, _convergenceSegmentVisualReferencePoints(i).Name))
                            Else
                                'Write some logic how to decide which side to take
                            End If
                        Else

                        End If
                        intermediateSegmentAngle = Functions.ReturnAngleInDegrees(divergencePnt, convergencePnt)
                        side = 0
                    Else
                        side = 0
                        divergencePnt = _divergenceSegmentVisualReferencePoints(divergencePntSelectedIndx).ProjPnt
                        convergencePnt = _convergenceSegmentVisualReferencePoints(i).ProjPnt
                        intermediateSegmentAngle = Functions.ReturnAngleInDegrees(_divergenceSegmentVisualReferencePoints(divergencePntSelectedIndx).ProjPnt, _convergenceSegmentVisualReferencePoints(i).ProjPnt)
                        _trackStepMapElements.Add(DrawStep(_startPrjPnt, divergencePnt, convergencePnt,
                                             _targetPrjPnt, _startPntPrjAngle, intermediateSegmentAngle, _targetPntPrjAngle, 0, isOnCirclingBox, isFinalStep, _convergenceSegmentVisualReferencePoints(i).Name))
                        
                    End If
                End If

                If Not _showTrackBuffer Then
                    DeleteTrackBuffer()
                End If

                pTopo.IsKnownSimple_2 = False
                pTopo = CType(_initialSegmentPolygon, ITopologicalOperator2)
                pTopo.Simplify()
                pTopo = CType(_intermediateSegmentPolygon, ITopologicalOperator2)
                pTopo.Simplify()
                pTopo = CType(_finalSegmentPolygon, ITopologicalOperator2)
                pTopo.Simplify()

                Dim tempTrackStep As TrackStep = New TrackStep()
                tempTrackStep.StartGeoPoint = CType(Functions.ToGeo(_startPrjPnt), IPoint)
                tempTrackStep.StartPointGeoAngle = Dir2Azt(_startPrjPnt, _startPntPrjAngle)
                tempTrackStep.StartPrjPoint = _startPrjPnt
                tempTrackStep.StartPointPrjAngle = _startPntPrjAngle
                tempTrackStep.TargetGeoPoint = CType(Functions.ToGeo(_targetPrjPnt), IPoint)
                tempTrackStep.TargetPointGeoAngle = Dir2Azt(_targetPrjPnt, _targetPntPrjAngle)
                tempTrackStep.TargetPrjPoint = _targetPrjPnt
                tempTrackStep.TargetPointPrjAngle = _targetPntPrjAngle

                tempTrackStep.DivergenceAngle = Math.Abs(_startPntPrjAngle - intermediateSegmentAngle)
                If tempTrackStep.DivergenceAngle > 180 Then
                    tempTrackStep.DivergenceAngle = 360 - tempTrackStep.DivergenceAngle
                End If
                tempTrackStep.ConvergenceAngle = Math.Abs(intermediateSegmentAngle - _targetPntPrjAngle)
                If tempTrackStep.ConvergenceAngle > 180 Then
                    tempTrackStep.ConvergenceAngle = 360 - tempTrackStep.ConvergenceAngle
                End If
				tempTrackStep.DistBetweenManeuvers = Functions.ReturnDistanceFromGeomInMeters(divergencePnt, convergencePnt)
				If _divergenceVisRefPntIndx > -1 Then
					tempTrackStep.DivergenceVisualReferencePoint = _divergenceSegmentVisualReferencePoints(_divergenceVisRefPntIndx)
				Else
					tempTrackStep.DivergenceVisualReferencePoint = Nothing
				End If
				If _convergenceVisRefPntIndx > -1 Then
					tempTrackStep.ConvergenceVisualReferencePoint = _convergenceSegmentVisualReferencePoints(_convergenceVisRefPntIndx)
				Else
					tempTrackStep.ConvergenceVisualReferencePoint = Nothing
				End If
				tempTrackStep.initialSegmentCentrelinePointCollection = _initialSegmentCentrelinePntColl
				tempTrackStep.intermediateSegmentCentrelinePointCollection = _intermediateSegmentCentrelinePntColl
				tempTrackStep.finalSegmentCentrelinePointCollection = _finalSegmentCentrelinePntColl
				tempTrackStep.initialSegmentPolygon = _initialSegmentPolygon
				tempTrackStep.intermediateSegmentPolygon = _intermediateSegmentPolygon
				tempTrackStep.finalSegmentPolygon = _finalSegmentPolygon
				tempTrackStep.usingCirclingBox = side
				If isFinalStep Then
					tempTrackStep.isFinalStep = True
				Else
					tempTrackStep.isFinalStep = False
				End If
				_trackSteps.Add(tempTrackStep)

				Dim tempTrackStepObstacle As ObstacleAr
				Dim segmentRelatOper As IRelationalOperator

				For k As Integer = 0 To _obstacleList.Length - 1
					tempTrackStepObstacle = New ObstacleAr()
					segmentRelatOper = CType(_trackSteps(_trackSteps.Count - 1).initialSegmentPolygon, IRelationalOperator)
					If segmentRelatOper.Contains(_obstacleList(k).pGeomPrj) Then
						_obstacleList(k).stepNumber = _trackSteps.Count
						tempTrackStepObstacle = _obstacleList(k)
						_trackStepObstacles.Add(tempTrackStepObstacle)
						Continue For
					End If
					segmentRelatOper = CType(_trackSteps(_trackSteps.Count - 1).intermediateSegmentPolygon, IRelationalOperator)
					If segmentRelatOper.Contains(_obstacleList(k).pGeomPrj) Then
						_obstacleList(k).stepNumber = _trackSteps.Count
						tempTrackStepObstacle = _obstacleList(k)
						_trackStepObstacles.Add(tempTrackStepObstacle)
						Continue For
					End If
					segmentRelatOper = CType(_trackSteps(_trackSteps.Count - 1).finalSegmentPolygon, IRelationalOperator)
					If segmentRelatOper.Contains(_obstacleList(k).pGeomPrj) Then
						_obstacleList(k).stepNumber = _trackSteps.Count
						tempTrackStepObstacle = _obstacleList(k)
						_trackStepObstacles.Add(tempTrackStepObstacle)
						Continue For
					End If
				Next
			End If
		Next

		If _divergencePolyElement IsNot Nothing Then
			_pGraphics.DeleteElement(_divergencePolyElement)
			_divergencePolyElement = Nothing
		End If
		If _convergencePolyElement IsNot Nothing Then
			_pGraphics.DeleteElement(_convergencePolyElement)
			_convergencePolyElement = Nothing
		End If
		If _divergenceVisRefPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_divergenceVisRefPntElement)
			_divergenceVisRefPntElement = Nothing
		End If
		If _convergenceVisRefPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_convergenceVisRefPntElement)
			_convergenceVisRefPntElement = Nothing
		End If
		If _trackSteps.Count = 0 Then
			_startPrjPnt = _initialStartPrjPnt
			_startPntPrjAngle = _initialPntPrjAngle
		Else
			_startPrjPnt = _trackSteps(_trackSteps.Count - 1).TargetPrjPoint
			_startPntPrjAngle = _trackSteps(_trackSteps.Count - 1).TargetPointPrjAngle
		End If

		_trackOCH = CalculateTrackOCH()
	End Sub

	Public Sub AddStepButtonClicked_2(targetPnt As IPoint, targetPntAngle As Double, divPnt As IPoint, divAngle As Double, convPnt As IPoint, convAngle As Double)
		_targetPrjPnt = targetPnt
		_targetPntPrjAngle = targetPntAngle
		_divergencePnt = divPnt
		_divergenceAngle = divAngle
		_convergencePnt = convPnt
		_convergenceAngle = convAngle

		Dim tempTrackStep As TrackStep_2 = New TrackStep_2()
		tempTrackStep.startPrjPnt = _startPrjPnt
		tempTrackStep.targetPrjPnt = _targetPrjPnt
		tempTrackStep.divergencePrjPnt = _divergencePnt
		tempTrackStep.convergencePrjPnt = _convergencePnt
		tempTrackStep.divergenceAngle = _divergenceAngle
		tempTrackStep.convergenceAngle = _convergenceAngle

		tempTrackStep.trackStepCentreline = _trackStepCentreline
		tempTrackStep.trackStepPolygon = _trackStepPolygon

		_trackSteps_2.Add(tempTrackStep)

		If _trackSteps.Count = 0 Then
			_startPrjPnt = _initialStartPrjPnt
			_startPntPrjAngle = _initialPntPrjAngle
		Else
			_startPrjPnt = _trackSteps(_trackSteps.Count - 1).TargetPrjPoint
			_startPntPrjAngle = _trackSteps(_trackSteps.Count - 1).TargetPointPrjAngle
		End If
	End Sub

	Private Function DrawStep(startPnt As IPoint, initialSegmentDivergencePnt As IPoint, finalSegmentConvergencePnt As IPoint, targetPnt As IPoint,
		  startPntAngle As Double, intermediateSegmentAngle As Double, targetPntAngle As Double, side As Integer, isOnCirclingBox As Boolean, isFinalStep As Boolean, convergencePntName As String) As TrackStepMapElement
		Dim pnt1 As IPoint
		Dim pnt2 As IPoint
		Dim pnt3 As IPoint
		Dim pnt4 As IPoint
		Dim centPnt As IPoint

		Dim _initialSegmentLength As Double = Functions.ReturnDistanceFromGeomInMeters(startPnt, initialSegmentDivergencePnt)
		Dim _intermediateSegmentLength As Double
		Dim _finalSegmentLength As Double = Functions.ReturnDistanceFromGeomInMeters(finalSegmentConvergencePnt, targetPnt)
		Dim divergenceAngle As Double
		Dim convergenceAngle As Double
		Dim distBetweenManeuvers As Double
		Dim _usingBox As Integer = 0

		'Draw initial segment
		_initialSegmentCentrelinePntColl = Nothing
		_initialSegmentCentreline = Nothing
		_initialSegmentCentrelineElement = Nothing
		_initialSegmentPntColl = Nothing
		_initialSegmentPolygon = Nothing
		_initialSegmentPolygonElement = Nothing

		_initialSegmentCentrelinePntColl = New Polyline()
		_initialSegmentCentrelinePntColl.AddPoint(startPnt)
		_initialSegmentCentrelinePntColl.AddPoint(initialSegmentDivergencePnt)

		_initialSegmentCentreline = CType(_initialSegmentCentrelinePntColl, IPolyline)
		_initialSegmentCentrelineElement = Functions.DrawPolyLine(_initialSegmentCentreline, _color, 2.0, True)

		pnt1 = Functions.PointAlongPlane(startPnt, Modulus(startPntAngle - 90, 360), _corridorSemiWidth)
		pnt2 = Functions.PointAlongPlane(pnt1, startPntAngle, _initialSegmentLength)
		pnt3 = Functions.PointAlongPlane(pnt2, startPntAngle + 90, 2 * _corridorSemiWidth)
		pnt4 = Functions.PointAlongPlane(pnt3, Modulus(startPntAngle - 180, 360), _initialSegmentLength)
		centPnt = Functions.PointAlongPlane(pnt2, startPntAngle + 90, _corridorSemiWidth)
		_initialSegmentPntColl = New Polygon()
		_initialSegmentPntColl.AddPoint(pnt1)
		_initialSegmentPntColl.AddPoint(pnt2)
		_initialSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt2, pnt3, 1))
		_initialSegmentPntColl.AddPoint(pnt3)
		_initialSegmentPntColl.AddPoint(pnt4)

		_initialSegmentPolygon = CType(_initialSegmentPntColl, IPolygon)
		_initialSegmentPolygonElement = Functions.DrawPolygon(_initialSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)


		'Draw intermediate segment
		_intermediateSegmentCentrelinePntColl = Nothing
		_intermediateSegmentCentreline = Nothing
		_intermediateSegmentCentrelineElement = Nothing
		_intermediateSegmentPntColl = Nothing
		_intermediateSegmentPolygon = Nothing
		_intermediateSegmentPolygonElement = Nothing

		If isOnCirclingBox AndAlso _startTargetAngleDiff > 178 AndAlso _startTargetAngleDiff < 182 Then
			_intermediateSegmentCentrelinePntColl = New Polyline()
			_intermediateSegmentCentrelinePntColl.AddPoint(initialSegmentDivergencePnt)

			If side > 0 Then
				centPnt = Functions.PointAlongPlane(initialSegmentDivergencePnt, Modulus(startPntAngle - 90, 360), _turnRadius)
				_intermediateSegmentCentrelinePntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, initialSegmentDivergencePnt, finalSegmentConvergencePnt, -1))
			Else
				centPnt = Functions.PointAlongPlane(initialSegmentDivergencePnt, startPntAngle + 90, _turnRadius)
				_intermediateSegmentCentrelinePntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, initialSegmentDivergencePnt, finalSegmentConvergencePnt, 1))
			End If
			_intermediateSegmentCentrelinePntColl.AddPoint(finalSegmentConvergencePnt)
			_intermediateSegmentCentreline = CType(_intermediateSegmentCentrelinePntColl, IPolyline)
			_intermediateSegmentCentrelineElement = Functions.DrawPolyLine(_intermediateSegmentCentreline, _color, 2.0, True)

			_intermediateSegmentPntColl = New Polygon()
			If side > 0 Then
				pnt1 = Functions.PointAlongPlane(initialSegmentDivergencePnt, startPntAngle + 90, _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPoint(pnt1)
				centPnt = Functions.PointAlongPlane(pnt1, Modulus(startPntAngle - 90, 360), _corridorSemiWidth + _turnRadius)
				pnt2 = Functions.PointAlongPlane(pnt1, Modulus(startPntAngle - 90, 360), 2 * _corridorSemiWidth + 2 * _turnRadius)
				_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, -1))
				_intermediateSegmentPntColl.AddPoint(pnt2)
				pnt3 = Functions.PointAlongPlane(pnt2, startPntAngle + 90, 2 * _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPoint(pnt3)
				centPnt = Functions.PointAlongPlane(pnt3, startPntAngle + 90, _turnRadius - _corridorSemiWidth)
				pnt4 = Functions.PointAlongPlane(pnt3, startPntAngle + 90, 2 * _turnRadius - _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt3, pnt4, 1))
				_intermediateSegmentPntColl.AddPoint(pnt4)
				_usingBox = 1
			ElseIf side < 0 Then
				pnt1 = Functions.PointAlongPlane(initialSegmentDivergencePnt, Modulus(startPntAngle - 90, 360), _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPoint(pnt1)
				centPnt = Functions.PointAlongPlane(pnt1, startPntAngle + 90, _corridorSemiWidth + _turnRadius)
				pnt2 = Functions.PointAlongPlane(pnt1, startPntAngle + 90, 2 * _corridorSemiWidth + 2 * _turnRadius)
				_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, 1))
				_intermediateSegmentPntColl.AddPoint(pnt2)
				pnt3 = Functions.PointAlongPlane(pnt2, Modulus(startPntAngle - 90, 360), 2 * _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPoint(pnt3)
				centPnt = Functions.PointAlongPlane(pnt3, Modulus(startPntAngle - 90, 360), _turnRadius - _corridorSemiWidth)
				pnt4 = Functions.PointAlongPlane(pnt3, Modulus(startPntAngle - 90, 360), 2 * _turnRadius - _corridorSemiWidth)
				_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt3, pnt4, -1))
				_intermediateSegmentPntColl.AddPoint(pnt4)
				_usingBox = -1
			End If
			_intermediateSegmentPolygon = CType(_intermediateSegmentPntColl, IPolygon)
			_intermediateSegmentPolygonElement = Functions.DrawPolygon(_intermediateSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
			divergenceAngle = 90
			convergenceAngle = 90
		Else
			If isFinalStep AndAlso _startTargetAngleDiff > 90 AndAlso _startTargetAngleDiff < 270 AndAlso convergencePntName = _trackSteps(trackSteps.Count - 1).ConvergenceVisualReferencePoint.Name Then
				_intermediateSegmentCentrelinePntColl = New Polyline()
				_intermediateSegmentCentrelinePntColl.AddPoint(initialSegmentDivergencePnt)

				If side < 0 Then
					If _startTargetAngleDiff = 180 Then
					Else
						centPnt = Functions.PointAlongPlane(initialSegmentDivergencePnt, startPntAngle + 90, _turnRadius)
						_intermediateSegmentCentrelinePntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, initialSegmentDivergencePnt, finalSegmentConvergencePnt, 1))
					End If
				ElseIf side > 0 Then
					If _startTargetAngleDiff = 180 Then
					Else
						centPnt = Functions.PointAlongPlane(initialSegmentDivergencePnt, Modulus(startPntAngle - 90, 360), _turnRadius)
						_intermediateSegmentCentrelinePntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, initialSegmentDivergencePnt, finalSegmentConvergencePnt, -1))
					End If
				End If
				_intermediateSegmentCentrelinePntColl.AddPoint(finalSegmentConvergencePnt)
				_intermediateSegmentCentreline = CType(_intermediateSegmentCentrelinePntColl, IPolyline)
				_intermediateSegmentCentrelineElement = Functions.DrawPolyLine(_intermediateSegmentCentreline, _color, 2.0, True)

				_intermediateSegmentPntColl = New Polygon()
				If side < 0 Then
					If _startTargetAngleDiff = 180 Then

					Else
						pnt1 = Functions.PointAlongPlane(initialSegmentDivergencePnt, Modulus(startPntAngle - 90, 360), _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPoint(pnt1)
						centPnt = Functions.PointAlongPlane(pnt1, startPntAngle + 90, _corridorSemiWidth + _turnRadius)
						pnt2 = Functions.PointAlongPlane(finalSegmentConvergencePnt, targetPntAngle - 90, _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, 1))
						_intermediateSegmentPntColl.AddPoint(pnt2)
						pnt3 = Functions.PointAlongPlane(pnt2, targetPntAngle + 90, 2 * _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPoint(pnt3)
						centPnt = Functions.PointAlongPlane(pnt3, targetPntAngle + 90, _turnRadius - _corridorSemiWidth)
						pnt4 = Functions.PointAlongPlane(pnt1, startPntAngle + 90, 2 * _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt3, pnt4, -1))
						_intermediateSegmentPntColl.AddPoint(pnt4)
					End If
				ElseIf side > 0 Then
					If _startTargetAngleDiff = 180 Then

					Else
						pnt1 = Functions.PointAlongPlane(initialSegmentDivergencePnt, startPntAngle + 90, _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPoint(pnt1)
						centPnt = Functions.PointAlongPlane(pnt1, Modulus(startPntAngle - 90, 360), _corridorSemiWidth + _turnRadius)
						pnt2 = Functions.PointAlongPlane(finalSegmentConvergencePnt, targetPntAngle + 90, _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt1, pnt2, -1))
						_intermediateSegmentPntColl.AddPoint(pnt2)
						pnt3 = Functions.PointAlongPlane(pnt2, targetPntAngle - 90, 2 * _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPoint(pnt3)
						centPnt = Functions.PointAlongPlane(pnt3, targetPntAngle - 90, _turnRadius - _corridorSemiWidth)
						pnt4 = Functions.PointAlongPlane(pnt1, startPntAngle - 90, 2 * _corridorSemiWidth)
						_intermediateSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt3, pnt4, 1))
						_intermediateSegmentPntColl.AddPoint(pnt4)
					End If
				End If
				_intermediateSegmentPolygon = CType(_intermediateSegmentPntColl, IPolygon)
				_intermediateSegmentPolygonElement = Functions.DrawPolygon(_intermediateSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
				divergenceAngle = 90
				convergenceAngle = 90
			Else
				_intermediateSegmentLength = Functions.ReturnDistanceFromGeomInMeters(initialSegmentDivergencePnt, finalSegmentConvergencePnt)
				_intermediateSegmentCentrelinePntColl = New Polyline()
				_intermediateSegmentCentrelinePntColl.AddPoint(initialSegmentDivergencePnt)
				_intermediateSegmentCentrelinePntColl.AddPoint(finalSegmentConvergencePnt)

				_intermediateSegmentCentreline = CType(_intermediateSegmentCentrelinePntColl, IPolyline)
				_intermediateSegmentCentrelineElement = Functions.DrawPolyLine(_intermediateSegmentCentreline, _color, 2.0, True)

				pnt1 = Functions.PointAlongPlane(initialSegmentDivergencePnt, Modulus(intermediateSegmentAngle - 90, 360), _corridorSemiWidth)
				pnt2 = Functions.PointAlongPlane(pnt1, intermediateSegmentAngle, _intermediateSegmentLength)
				pnt3 = Functions.PointAlongPlane(pnt2, intermediateSegmentAngle + 90, 2 * _corridorSemiWidth)
				pnt4 = Functions.PointAlongPlane(pnt3, Modulus(intermediateSegmentAngle - 180, 360), _intermediateSegmentLength)
				_intermediateSegmentPntColl = New Polygon()
				_intermediateSegmentPntColl.AddPoint(pnt1)
				_intermediateSegmentPntColl.AddPoint(pnt2)
				_intermediateSegmentPntColl.AddPoint(pnt3)
				_intermediateSegmentPntColl.AddPoint(pnt4)

				_intermediateSegmentPolygon = CType(_intermediateSegmentPntColl, IPolygon)
				_intermediateSegmentPolygonElement = Functions.DrawPolygon(_intermediateSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)

				divergenceAngle = Math.Abs(intermediateSegmentAngle - startPntAngle)
				convergenceAngle = Math.Abs(intermediateSegmentAngle - targetPntAngle)
			End If
		End If


		'Draw final segment
		_finalSegmentCentrelinePntColl = Nothing
		_finalSegmentCentreline = Nothing
		_finalSegmentCentrelineElement = Nothing
		_finalSegmentPntColl = Nothing
		_finalSegmentPolygon = Nothing
		_finalSegmentPolygonElement = Nothing

		_finalSegmentCentrelinePntColl = New Polyline()
		_finalSegmentCentrelinePntColl.AddPoint(finalSegmentConvergencePnt)
		_finalSegmentCentrelinePntColl.AddPoint(targetPnt)

		_finalSegmentCentreline = CType(_finalSegmentCentrelinePntColl, IPolyline)
		_finalSegmentCentrelineElement = Functions.DrawPolyLine(_finalSegmentCentreline, _color, 2.0, True)

		pnt1 = Functions.PointAlongPlane(finalSegmentConvergencePnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
		pnt2 = Functions.PointAlongPlane(pnt1, targetPntAngle, _finalSegmentLength)
		pnt3 = Functions.PointAlongPlane(pnt2, targetPntAngle + 90, 2 * _corridorSemiWidth)
		pnt4 = Functions.PointAlongPlane(pnt3, Modulus(targetPntAngle - 180, 360), _finalSegmentLength)
		_finalSegmentPntColl = New Polygon()
		_finalSegmentPntColl.AddPoint(pnt1)
		_finalSegmentPntColl.AddPoint(pnt2)
		_finalSegmentPntColl.AddPoint(pnt3)
		_finalSegmentPntColl.AddPoint(pnt4)
		centPnt = Functions.PointAlongPlane(pnt4, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
		_finalSegmentPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(centPnt, pnt4, pnt1, 1))

		_finalSegmentPolygon = CType(_finalSegmentPntColl, IPolygon)
		_finalSegmentPolygonElement = Functions.DrawPolygon(_finalSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)

		Dim startPntElement As IElement
		Dim targetPntElement As IElement
		Dim trackStepMapElement As TrackStepMapElement

		startPntElement = Functions.DrawPoint(startPnt, 255, True)
		targetPntElement = Functions.DrawPoint(targetPnt, 255, True)
		distBetweenManeuvers = Functions.ReturnDistanceFromGeomInMeters(initialSegmentDivergencePnt, finalSegmentConvergencePnt)

		trackStepMapElement = New TrackStepMapElement()
		trackStepMapElement.startPntElement = startPntElement
		trackStepMapElement.targetPntElement = targetPntElement
		trackStepMapElement.initialSegmentPolygon = _initialSegmentPolygon
		trackStepMapElement.initialSegmentPolygonElement = _initialSegmentPolygonElement
		trackStepMapElement.initialSegmentCentrelineElement = _initialSegmentCentrelineElement
		trackStepMapElement.intermediateSegmentPolygon = _intermediateSegmentPolygon
		trackStepMapElement.intermediateSegmentPolygonElement = _intermediateSegmentPolygonElement
		trackStepMapElement.intermediateSegmentCentrelineElement = _intermediateSegmentCentrelineElement
		trackStepMapElement.finalSegmentPolygon = _finalSegmentPolygon
		trackStepMapElement.finalSegmentPolygonElement = _finalSegmentPolygonElement
		trackStepMapElement.finalSegmentCentrelineElement = _finalSegmentCentrelineElement

		Return trackStepMapElement
	End Function

	Public Sub RemoveTrack()
		If _divergencePolyElement IsNot Nothing Then
			_pGraphics.DeleteElement(_divergencePolyElement)
		End If
		If _divergenceVisRefPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_divergenceVisRefPntElement)
		End If
		If _convergencePolyElement IsNot Nothing Then
			_pGraphics.DeleteElement(_convergencePolyElement)
		End If
		If _convergenceVisRefPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_convergenceVisRefPntElement)
		End If
		If _startPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_startPntElement)
		End If
		If _targetPntElement IsNot Nothing Then
			_pGraphics.DeleteElement(_targetPntElement)
		End If
		If _trackStepMapElements.Count > 0 Then
			For i As Integer = 0 To _trackSteps.Count - 1
				If _trackStepMapElements(i).startPntElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).startPntElement)
				End If
				If _trackStepMapElements(i).initialSegmentCentrelineElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).initialSegmentCentrelineElement)
				End If
				If _trackStepMapElements(i).initialSegmentPolygonElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).initialSegmentPolygonElement)
				End If
				If _trackStepMapElements(i).intermediateSegmentCentrelineElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).intermediateSegmentCentrelineElement)
				End If
				If _trackStepMapElements(i).intermediateSegmentPolygonElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).intermediateSegmentPolygonElement)
				End If
				If _trackStepMapElements(i).finalSegmentCentrelineElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).finalSegmentCentrelineElement)
				End If
				If _trackStepMapElements(i).finalSegmentPolygonElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).finalSegmentPolygonElement)
				End If
				If _trackStepMapElements(i).targetPntElement IsNot Nothing Then
					_pGraphics.DeleteElement(_trackStepMapElements(i).targetPntElement)
				End If
			Next
		End If
		If _trackHighestObstacleElement IsNot Nothing Then
			_pGraphics.DeleteElement(_trackHighestObstacleElement)
		End If

		If _pickableAreaElements IsNot Nothing Then
			For i As Integer = 0 To _pickableAreaElements.ElementCount - 1
				_pGraphics.DeleteElement(_pickableAreaElements.Element(i))
			Next
			_pickableAreaElements = Nothing
		End If
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

		For i As Integer = 0 To trackSteps.Count - 1
			trackSteps.RemoveAt(trackSteps.Count - 1)
		Next
		For i As Integer = 0 To trackStepObstacles.Count - 1
			trackStepObstacles.RemoveAt(trackStepObstacles.Count - 1)
		Next
		_trackOCH = 0


	End Sub

	Private Class TrackStepMapElement

		Private _startPntElement As IElement
		Public Property startPntElement() As IElement
			Get
				Return _startPntElement
			End Get
			Set(ByVal value As IElement)
				_startPntElement = value
			End Set
		End Property
		Private _targetPntElement As IElement
		Public Property targetPntElement() As IElement
			Get
				Return _targetPntElement
			End Get
			Set(ByVal value As IElement)
				_targetPntElement = value
			End Set
		End Property
		'Private _initialSegmentPntColl As IPointCollection
		'Public Property initialSegmentPntColl() As IPointCollection
		'    Get
		'        Return _initialSegmentPntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _initialSegmentPntColl = value
		'    End Set
		'End Property
		'Private _initialSegmentPolygon As IPolygon
		'Public Property initialSegmentPolygon() As IPolygon
		'    Get
		'        Return _initialSegmentPolygon
		'    End Get
		'    Set(ByVal value As IPolygon)
		'        _initialSegmentPolygon = value
		'    End Set
		'End Property
		Private _initialSegmentPolygon As IPolygon
		Public Property initialSegmentPolygon() As IPolygon
			Get
				Return _initialSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_initialSegmentPolygon = value
			End Set
		End Property
		Private _initialSegmentPolygonElement As IElement
		Public Property initialSegmentPolygonElement() As IElement
			Get
				Return _initialSegmentPolygonElement
			End Get
			Set(ByVal value As IElement)
				_initialSegmentPolygonElement = value
			End Set
		End Property
		'Private _initialSegmentCetrelinePntColl As IPointCollection
		'Public Property initialSegmentCentrelinePntColl() As IPointCollection
		'    Get
		'        Return _initialSegmentCetrelinePntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _initialSegmentCetrelinePntColl = value
		'    End Set
		'End Property
		'Private _initialSegmentCentreline As IPolyline
		'Public Property initialSegmentCentreline() As IPolyline
		'    Get
		'        Return _initialSegmentCentreline
		'    End Get
		'    Set(ByVal value As IPolyline)
		'        _initialSegmentCentreline = value
		'    End Set
		'End Property
		Private _initialSegmentCentrelineElement As IElement
		Public Property initialSegmentCentrelineElement() As IElement
			Get
				Return _initialSegmentCentrelineElement
			End Get
			Set(ByVal value As IElement)
				_initialSegmentCentrelineElement = value
			End Set
		End Property
		'Private _intermediateSegmentPntColl As IPointCollection
		'Public Property intermediateSegmentPntColl() As IPointCollection
		'    Get
		'        Return _intermediateSegmentPntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _intermediateSegmentPntColl = value
		'    End Set
		'End Property
		'Private _intermediateSegmentPolygon As IPolygon
		'Public Property intermediateSegmentPolygon() As IPolygon
		'    Get
		'        Return _intermediateSegmentPolygon
		'    End Get
		'    Set(ByVal value As IPolygon)
		'        _intermediateSegmentPolygon = value
		'    End Set
		'End Property
		Private _intermediateSegmentPolygon As IPolygon
		Public Property intermediateSegmentPolygon() As IPolygon
			Get
				Return _intermediateSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_intermediateSegmentPolygon = value
			End Set
		End Property
		Private _intermediateSegmentPolygonElement As IElement
		Public Property intermediateSegmentPolygonElement() As IElement
			Get
				Return _intermediateSegmentPolygonElement
			End Get
			Set(ByVal value As IElement)
				_intermediateSegmentPolygonElement = value
			End Set
		End Property
		'Private _intermediateSegmentCentrelinePntColl As IPointCollection
		'Public Property intermediateSegmentCentrelinePntColl() As IPointCollection
		'    Get
		'        Return _intermediateSegmentCentrelinePntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _intermediateSegmentCentrelinePntColl = value
		'    End Set
		'End Property
		'Private _intermediateSegmentCentreline As IPolyline
		'Public Property intermediateSegmentCentreline() As IPolyline
		'    Get
		'        Return _intermediateSegmentCentreline
		'    End Get
		'    Set(ByVal value As IPolyline)
		'        _intermediateSegmentCentreline = value
		'    End Set
		'End Property
		Private _intermediateSegmentCentrelineElement As IElement
		Public Property intermediateSegmentCentrelineElement() As IElement
			Get
				Return _intermediateSegmentCentrelineElement
			End Get
			Set(ByVal value As IElement)
				_intermediateSegmentCentrelineElement = value
			End Set
		End Property
		'Private _finalSegmentPntColl As IPointCollection
		'Public Property finalSegmentPntColl() As IPointCollection
		'    Get
		'        Return _finalSegmentPntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _finalSegmentPntColl = value
		'    End Set
		'End Property
		'Private _finalSegmentPolygon As IPolygon
		'Public Property finalSegmentPolygon() As IPolygon
		'    Get
		'        Return _finalSegmentPolygon
		'    End Get
		'    Set(ByVal value As IPolygon)
		'        _finalSegmentPolygon = value
		'    End Set
		'End Property
		Private _finalSegmentPolygon As IPolygon
		Public Property finalSegmentPolygon() As IPolygon
			Get
				Return _finalSegmentPolygon
			End Get
			Set(ByVal value As IPolygon)
				_finalSegmentPolygon = value
			End Set
		End Property
		Private _finalSegmentPolygonElement As IElement
		Public Property finalSegmentPolygonElement() As IElement
			Get
				Return _finalSegmentPolygonElement
			End Get
			Set(ByVal value As IElement)
				_finalSegmentPolygonElement = value
			End Set
		End Property
		'Private _finalSegmentCentrelinePntColl As IPointCollection
		'Public Property finalSegmentCentrelinePntColl() As IPointCollection
		'    Get
		'        Return _finalSegmentCentrelinePntColl
		'    End Get
		'    Set(ByVal value As IPointCollection)
		'        _finalSegmentCentrelinePntColl = value
		'    End Set
		'End Property
		'Private _finalSegmentCentreline As IPolyline
		'Public Property finalSegmentCentreline() As IPolyline
		'    Get
		'        Return _finalSegmentCentreline
		'    End Get
		'    Set(ByVal value As IPolyline)
		'        _finalSegmentCentreline = value
		'    End Set
		'End Property
		Private _finalSegmentCentrelineElement As IElement
		Public Property finalSegmentCentrelineElement() As IElement
			Get
				Return _finalSegmentCentrelineElement
			End Get
			Set(ByVal value As IElement)
				_finalSegmentCentrelineElement = value
			End Set
		End Property

	End Class

	Public Function CalculateTrackOCH() As Double
		Dim I As Integer
		Dim J As Integer

		Dim relatOper As IRelationalOperator
		Dim maxHeight As Double = -100000
		Dim maxIndex As Integer = -1


		For I = 0 To _trackStepMapElements.Count - 1
			relatOper = CType(_trackStepMapElements(I).initialSegmentPolygon, IRelationalOperator)
			For J = 0 To _obstacleList.Length - 1
				If relatOper.Contains(_obstacleList(J).pGeomPrj) Then
					If _obstacleList(J).Height > maxHeight Then
						maxHeight = _obstacleList(J).Height
						maxIndex = J
					End If
				End If
			Next
			relatOper = CType(_trackStepMapElements(I).intermediateSegmentPolygon, IRelationalOperator)
			For J = 0 To _obstacleList.Length - 1
				If relatOper.Contains(_obstacleList(J).pGeomPrj) Then
					If _obstacleList(J).Height > maxHeight Then
						maxHeight = _obstacleList(J).Height
						maxIndex = J
					End If
				End If
			Next
			relatOper = CType(_trackStepMapElements(I).finalSegmentPolygon, IRelationalOperator)
			For J = 0 To _obstacleList.Length - 1
				If relatOper.Contains(_obstacleList(J).pGeomPrj) Then
					If _obstacleList(J).Height > maxHeight Then
						maxHeight = _obstacleList(J).Height
						maxIndex = J
					End If
				End If
			Next
			If _trackSteps(I).usingCirclingBox > 0 Then
				If _rightBoxOCH > maxHeight Then
					maxHeight = _rightBoxOCH
					maxIndex = _rightBoxHighestObstacleIndex
				End If
			ElseIf _trackSteps(I).usingCirclingBox < 0 Then
				If _leftBoxOCH > maxHeight Then
					maxHeight = _leftBoxOCH
					maxIndex = _leftBoxHighestObstacleIndex
				End If
			End If
		Next

		Dim OCH As Double = maxHeight + _MOC
		If OCH < _minOCH Then
			OCH = _minOCH
		Else
			If _trackHighestObstacleElement IsNot Nothing Then
				_pGraphics.DeleteElement(_trackHighestObstacleElement)
			End If
			If _trackStepMapElements.Count > 0 Then
				_trackHighestObstacleElement = Functions.DrawPointWithText(_obstacleList(maxIndex).pGeomPrj, _obstacleList(maxIndex).Name, 255, True)
			End If
		End If

		Return OCH
	End Function

	Public Function CalcualteTrackLength() As Double
		Dim I As Integer
		Dim trackLength As Double = 0
		For I = 0 To _trackSteps.Count - 1
			trackLength += CType(_trackSteps(I).initialSegmentCentrelinePointCollection, IPolyline).Length
			trackLength += CType(_trackSteps(I).intermediateSegmentCentrelinePointCollection, IPolyline).Length
			trackLength += CType(_trackSteps(I).finalSegmentCentrelinePointCollection, IPolyline).Length
		Next
		Return Math.Round(trackLength)
	End Function

	Public Function CalculateFinalSegmentDescentGradient() As Double
		Dim gradient As Double = 0
		If trackSteps.Count > 0 AndAlso trackSteps(trackSteps.Count - 1).isFinalStep Then
			Dim tempDist As Double = CType(trackSteps(trackSteps.Count - 1).finalSegmentCentrelinePointCollection, IPolyline).Length
			If tempDist < _finalSegmentLength Then
				gradient = Math.Round(CalculateTrackOCH() / tempDist * 100, 1)
			Else
				gradient = Math.Round(CalculateTrackOCH() / _finalSegmentLength * 100, 1)
				'If gradient > maxGradient Then
				'    gradient = Math.Round(CalculateTrackOCH() / tempDist * 100, 1)
				'End If
			End If
		End If
		Return gradient
	End Function

	Public Function CalculateDownwindLegLength() As Double
		Dim downwindLegLength As Double = 0
		Dim angleDiff As Double
		Dim distToPnt As Double
		Dim distToPntProj As Double

		Dim tempPnt As IPoint

		For i As Integer = 0 To trackSteps.Count - 1
			If trackSteps(i).usingCirclingBox <> 0 Then
				tempPnt = Functions.PointAlongPlane(_selectedRWY.pPtPrj(eRWY.PtTHR), Modulus(trackSteps(i).StartPointPrjAngle - 180, 360), _finalSegmentLength)
				angleDiff = Math.Abs(Functions.ReturnAngleInDegrees(trackSteps(i).StartPrjPoint, tempPnt) - trackSteps(i).StartPointPrjAngle)
				distToPnt = Math.Sqrt(Math.Pow(trackSteps(i).StartPrjPoint.X - tempPnt.X, 2) +
										  Math.Pow(trackSteps(i).StartPrjPoint.Y - tempPnt.Y, 2))
				distToPntProj = distToPnt * Math.Cos(Functions.DegToRad(angleDiff))
				downwindLegLength = Functions.ReturnDistanceFromGeomInMeters(trackSteps(i).StartPrjPoint, Functions.PointAlongPlane(trackSteps(i).StartPrjPoint, trackSteps(i).StartPointPrjAngle, distToPntProj))
			End If
		Next
		Return downwindLegLength
	End Function

	Public Sub DeleteTrackBuffer()
		For i As Integer = 0 To _trackStepMapElements.Count - 1
			If _trackStepMapElements(i).initialSegmentPolygonElement IsNot Nothing Then
				pGraphics.DeleteElement(_trackStepMapElements(i).initialSegmentPolygonElement)
				_trackStepMapElements(i).initialSegmentPolygonElement = Nothing
			End If
			If _trackStepMapElements(i).intermediateSegmentPolygonElement IsNot Nothing Then
				pGraphics.DeleteElement(_trackStepMapElements(i).intermediateSegmentPolygonElement)
				_trackStepMapElements(i).intermediateSegmentPolygonElement = Nothing
			End If
			If _trackStepMapElements(i).finalSegmentPolygonElement IsNot Nothing Then
				pGraphics.DeleteElement(_trackStepMapElements(i).finalSegmentPolygonElement)
				_trackStepMapElements(i).finalSegmentPolygonElement = Nothing
			End If
		Next
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	End Sub

	Public Sub RedrawTrackBuffer()
		For i As Integer = 0 To _trackStepMapElements.Count - 1
			If _trackStepMapElements(i).initialSegmentPolygonElement Is Nothing Then
				_trackStepMapElements(i).initialSegmentPolygonElement = Functions.DrawPolygon(_trackStepMapElements(i).initialSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
			End If
			If _trackStepMapElements(i).intermediateSegmentPolygonElement Is Nothing Then
				_trackStepMapElements(i).intermediateSegmentPolygonElement = Functions.DrawPolygon(_trackStepMapElements(i).intermediateSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
			End If
			If _trackStepMapElements(i).finalSegmentPolygonElement Is Nothing Then
				_trackStepMapElements(i).finalSegmentPolygonElement = Functions.DrawPolygon(_trackStepMapElements(i).finalSegmentPolygon, _color, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True)
			End If
		Next
		GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
	End Sub

	Public Sub ConstructPickableArea(ByRef leftPoly As IGeometry, ByRef rightPoly As IGeometry, turnLeftGE180deg As Boolean, turnRightGE180deg As Boolean)
		Dim cutLine As IPointCollection = New Polyline
		Dim pntColl As IPointCollection
		Dim pnt1 As IPoint
		Dim pnt2 As IPoint
		Dim pnt3 As IPoint
		Dim pnt4 As IPoint
		Dim pnt5 As IPoint
		Dim pnt6 As IPoint
		Dim pnt7 As IPoint
		Dim pnt8 As IPoint
		Dim pnt9 As IPoint
		Dim tempPnt As IPoint
		Dim tempPnt2 As IPoint
		Dim tempAngle As Double
		Dim _distToPoly As Double

		If _trackSteps.Count = 0 Then
			_startPrjPnt = _initialStartPrjPnt
			_startPntPrjAngle = _initialPntPrjAngle
		Else
			_startPrjPnt = _trackSteps(_trackSteps.Count - 1).TargetPrjPoint
			_startPntPrjAngle = _trackSteps(_trackSteps.Count - 1).TargetPointPrjAngle
		End If

		If _pickableAreaElements IsNot Nothing Then
			For i As Integer = 0 To _pickableAreaElements.ElementCount - 1
				pGraphics.DeleteElement(_pickableAreaElements.Element(i))
			Next
			_pickableAreaElements = Nothing
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Dim poly As IPolygon = CType(_maneuveringAreaPntColl, IPolygon)
		pTopo = CType(_maneuveringAreaPntColl, ITopologicalOperator2)
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		Dim relatOper As IRelationalOperator = CType(_maneuveringAreaPntColl, IRelationalOperator)
		_pickableAreaElements = New GroupElement

		If turnLeftGE180deg Then
			If relatOper.Contains(_startPrjPnt) Then
				tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, _startPrjPnt, _startPntPrjAngle, _distToPoly, True)
			Else
				tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, _startPrjPnt, _startPntPrjAngle, _distToPoly, False)
			End If

			If _distToPoly >= angleStabilizationDist(90) + _corridorSemiWidth Then
				tempPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _distToPoly - _turnRadius - _corridorSemiWidth)
				tempPnt2 = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle + 90, 360), 2 * _turnRadius)
				If relatOper.Contains(tempPnt2) Then
					pnt1 = Functions.PointAlongPlane(tempPnt2, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
					If relatOper.Contains(pnt1) Then
						pnt2 = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt1, Modulus(_startPntPrjAngle - 180, 360), _distToPoly, True)
					Else
						pnt2 = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt1, Modulus(_startPntPrjAngle - 180, 360), _distToPoly, False)
					End If
					pnt3 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle - 90, 360), 2 * _corridorSemiWidth)
					pnt4 = Functions.PointAlongPlane(pnt1, Modulus(_startPntPrjAngle - 90, 360), 2 * _corridorSemiWidth)
					pntColl = New Polygon
					pntColl.AddPoint(pnt1)
					pntColl.AddPoint(pnt2)
					pntColl.AddPoint(pnt3)
					pntColl.AddPoint(pnt4)
					leftPoly = CType(pntColl, Polygon)
					_pickableAreaElements.AddElement(Functions.DrawPolygon(leftPoly, Color.LightGray.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True))

					tempAngle = 2 * RadToDeg(Math.Atan(_turnRadius / Functions.ReturnDistanceFromGeomInMeters(_startPrjPnt, tempPnt)))
					pnt2 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + tempAngle + 90, 360), _corridorSemiWidth)
					If relatOper.Contains(pnt2) Then
						pnt1 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle + tempAngle + 180, 360), 2500)
						If relatOper.Contains(pnt1) Then
							'pnt3 = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _corridorSemiWidth)
							'pnt4 = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle - 90, 360), 2500)
							pnt3 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle + tempAngle - 90, 360), 2 * _corridorSemiWidth)
							pnt4 = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle + tempAngle - 180, 360), 2500)

							pntColl = New Polygon
							pntColl.AddPoint(pnt1)
							pntColl.AddPoint(pnt2)
							pntColl.AddPoint(_startPrjPnt)
							pntColl.AddPoint(pnt3)
							pntColl.AddPoint(pnt4)
							rightPoly = CType(pntColl, Polygon)
							_pickableAreaElements.AddElement(Functions.DrawPolygon(rightPoly, Color.LightGray.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True))
						End If
					End If
				End If
			End If
		ElseIf turnRightGE180deg Then
			If relatOper.Contains(_startPrjPnt) Then
				tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, _startPrjPnt, _startPntPrjAngle, _distToPoly, True)
			Else
				tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, _startPrjPnt, _startPntPrjAngle, _distToPoly, False)
			End If

			If _distToPoly >= angleStabilizationDist(90) + _corridorSemiWidth Then
				tempPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _distToPoly - _turnRadius - _corridorSemiWidth)
				tempPnt2 = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle - 90, 360), 2 * _turnRadius)
				tempAngle = Modulus(_startPntPrjAngle - 180, 360)

				If relatOper.Contains(tempPnt2) Then
					pnt1 = Functions.PointAlongPlane(tempPnt2, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
					If relatOper.Contains(pnt1) Then
						pnt2 = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt1, Modulus(_startPntPrjAngle - 180, 360), _distToPoly, True)
					Else
						pnt2 = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt1, Modulus(_startPntPrjAngle - 180, 360), _distToPoly, False)
					End If
					pnt3 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle + 90, 360), 2 * _corridorSemiWidth)
					pnt4 = Functions.PointAlongPlane(pnt1, Modulus(_startPntPrjAngle + 90, 360), 2 * _corridorSemiWidth)
					pntColl = New Polygon
					pntColl.AddPoint(pnt1)
					pntColl.AddPoint(pnt2)
					pntColl.AddPoint(pnt3)
					pntColl.AddPoint(pnt4)
					rightPoly = CType(pntColl, Polygon)
					_pickableAreaElements.AddElement(Functions.DrawPolygon(rightPoly, Color.LightGray.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True))


					tempAngle = 2 * RadToDeg(Math.Atan(_turnRadius / Functions.ReturnDistanceFromGeomInMeters(_startPrjPnt, tempPnt)))
					pnt2 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - tempAngle - 90, 360), _corridorSemiWidth)
					If relatOper.Contains(pnt2) Then
						pnt1 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle - tempAngle - 180, 360), 2500)
						If relatOper.Contains(pnt1) Then
							'pnt3 = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _corridorSemiWidth)
							'pnt4 = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle + 90, 360), 2500)
							pnt3 = Functions.PointAlongPlane(pnt2, Modulus(_startPntPrjAngle - tempAngle + 90, 360), 2 * _corridorSemiWidth)
							pnt4 = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle - tempAngle + 180, 360), 2500)

							pntColl = New Polygon
							pntColl.AddPoint(pnt1)
							pntColl.AddPoint(pnt2)
							pntColl.AddPoint(_startPrjPnt)
							pntColl.AddPoint(pnt3)
							pntColl.AddPoint(pnt4)
							leftPoly = CType(pntColl, Polygon)
							_pickableAreaElements.AddElement(Functions.DrawPolygon(leftPoly, Color.LightGray.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True))
						End If
					End If
				End If
			End If
		Else
			tempPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _bankEstablishDistance)
			If Not relatOper.Contains(tempPnt) Then
				Return
			End If

			pnt5 = tempPnt
			tempPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, angleStabilizationDist(90))
			tempPnt = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle + 90, 360), angleStabilizationDist(90) - _bankEstablishDistance)
			pnt4 = tempPnt
			If relatOper.Contains(pnt4) Then
				tempPnt = Functions.PointAlongPlane(pnt4, Modulus(_startPntPrjAngle + 90, 360), 2 * _bankEstablishDistance)
				pnt3 = tempPnt
				If relatOper.Contains(pnt3) Then
					tempPnt = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle + 90, 360), angleStabilizationDist(45) - _bankEstablishDistance)
					tempPnt = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle + 90 + 45, 360), angleStabilizationDist(45) - _bankEstablishDistance)
					pnt2 = tempPnt
					If relatOper.Contains(pnt2) Then
						tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt2, Modulus(_startPntPrjAngle + 90 + 45, 360), _distToPoly, True)
						pnt1 = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle + 90 + 45, 360), 1000)
					End If
				End If
			End If

			tempPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, angleStabilizationDist(90))
			tempPnt = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle - 90, 360), angleStabilizationDist(90) - _bankEstablishDistance)
			pnt6 = tempPnt
			If relatOper.Contains(pnt6) Then
				tempPnt = Functions.PointAlongPlane(pnt6, Modulus(_startPntPrjAngle - 90, 360), 2 * _bankEstablishDistance)
				pnt7 = tempPnt
				If relatOper.Contains(pnt7) Then
					tempPnt = Functions.PointAlongPlane(pnt7, Modulus(_startPntPrjAngle - 90, 360), angleStabilizationDist(45) - _bankEstablishDistance)
					tempPnt = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle - 90 - 45, 360), angleStabilizationDist(45) - _bankEstablishDistance)
					pnt8 = tempPnt
					If relatOper.Contains(pnt8) Then
						tempPnt = Functions.LinePolygonIntersect(_maneuveringAreaPntColl, pnt8, Modulus(_startPntPrjAngle - 90 - 45, 360), _distToPoly, True)
						pnt9 = Functions.PointAlongPlane(tempPnt, Modulus(_startPntPrjAngle - 90 - 45, 360), 1000)
					End If
				End If
			End If

			If pnt1 IsNot Nothing Then
				cutLine.AddPoint(pnt1)
			End If
			If pnt2 IsNot Nothing Then
				cutLine.AddPoint(pnt2)
				tempPnt = Functions.PointAlongPlane(pnt3, Modulus(_startPntPrjAngle - 180, 360), _turnRadius)
				cutLine.AddPointCollection(Functions.CreateArcPolylinePrj(tempPnt, pnt2, pnt3, -1))
			End If
			If pnt3 IsNot Nothing Then
				cutLine.AddPoint(pnt3)
			End If
			If pnt4 IsNot Nothing Then
				cutLine.AddPoint(pnt4)
				tempPnt = Functions.PointAlongPlane(pnt4, Modulus(_startPntPrjAngle - 180, 360), _turnRadius)
				cutLine.AddPointCollection(Functions.CreateArcPolylinePrj(tempPnt, pnt4, pnt5, -1))
			End If
			If pnt5 IsNot Nothing Then
				cutLine.AddPoint(pnt5)
			End If
			If pnt6 IsNot Nothing Then
				tempPnt = Functions.PointAlongPlane(pnt6, Modulus(_startPntPrjAngle - 180, 360), _turnRadius)
				cutLine.AddPointCollection(Functions.CreateArcPolylinePrj(tempPnt, pnt5, pnt6, -1))
				cutLine.AddPoint(pnt6)
			End If
			If pnt7 IsNot Nothing Then
				cutLine.AddPoint(pnt7)
			End If
			If pnt8 IsNot Nothing Then
				tempPnt = Functions.PointAlongPlane(pnt7, Modulus(_startPntPrjAngle - 180, 360), _turnRadius)
				cutLine.AddPointCollection(Functions.CreateArcPolylinePrj(tempPnt, pnt7, pnt8, -1))
				cutLine.AddPoint(pnt8)
			End If
			If pnt9 IsNot Nothing Then
				cutLine.AddPoint(pnt9)
			End If

			leftPoly = New Polygon
			rightPoly = New Polygon
			pTopo.Cut(CType(cutLine, Global.ESRI.ArcGIS.Geometry.IPolyline), leftPoly, rightPoly)
			_pickableAreaElements.AddElement(Functions.DrawPolygon(leftPoly, Color.LightGray.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal, True))
		End If
		leftPolygon = leftPoly
		rightPolygon = rightPoly
	End Sub

	Public Sub DeletePickableArea()
		If _pickableAreaElements IsNot Nothing Then
			For i As Integer = 0 To _pickableAreaElements.ElementCount - 1
				pGraphics.DeleteElement(_pickableAreaElements.Element(i))
			Next
			_pickableAreaElements = Nothing
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Sub

	Public Sub GetAngleRange_old(targetPnt As IPoint, ByRef minAngle As Integer, ByRef maxAngle As Integer)
		minAngle = -1
		maxAngle = -1
		Dim firstTurnPnt As IPoint
		Dim tempPnt As IPoint
		Dim tempPnt2 As IPoint
		Dim tempSide As Integer
		Dim tempDist As Double
		Dim tempDist2 As Double
		Dim tempAngle As Double
		Dim tempAngle2 As Double
		Dim firstTurnAngle As Double
		Dim secondTurnAngle As Double
		Dim firstTurnDist As Double
		Dim secondTurnDist As Double
		Dim tangentDist As Double
		Dim rightStartCircleCentrePnt As IPoint = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _turnRadius)
		Dim leftStartCirclecentrePnt As IPoint = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _turnRadius)

		tempSide = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
		If tempSide = 0 Then
			tempDist = Functions.ReturnDistanceFromGeomInMeters(_startPrjPnt, targetPnt)
			If tempDist = 0 Then
				minAngle = _startPntPrjAngle
				maxAngle = _startPntPrjAngle
			Else
				tempDist2 = (tempDist - angleStabilizationDist(45)) * Math.Sin(DegToRad(45))
				If tempDist2 >= _turnRadius Then
					firstTurnAngle = RadToDeg(Math.Asin(_turnRadius / tempDist))
					tangentDist = tempDist * Math.Cos(DegToRad(firstTurnAngle))
					firstTurnDist = angleStabilizationDist(firstTurnAngle)
					If tangentDist >= firstTurnDist + angleStabilizationDist(90) Then
						tempAngle = 90 - firstTurnAngle
						minAngle = Modulus(_startPntPrjAngle - tempAngle, 360)
						maxAngle = Modulus(_startPntPrjAngle + tempAngle, 360)
					Else

					End If
				Else

				End If
			End If
		ElseIf tempSide < 1 Then
			firstTurnAngle = getTurnAngle(_startPrjPnt, _startPntPrjAngle, targetPnt, -1, -1)
			If firstTurnAngle <= 45 Then
				firstTurnDist = angleStabilizationDist(firstTurnAngle)
				firstTurnPnt = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, firstTurnDist)
				tempPnt = Functions.PointAlongPlane(targetPnt, Modulus(_startPntPrjAngle - firstTurnAngle - 90, 360), _turnRadius)
				tempDist = Functions.ReturnDistanceFromGeomInMeters(firstTurnPnt, tempPnt)
				If tempDist >= angleStabilizationDist(firstTurnAngle) + angleStabilizationDist(90) Then
					maxAngle = Modulus(_startPntPrjAngle - firstTurnAngle + 90, 360)
				Else

				End If


				tempPnt = Functions.PointAlongPlane(targetPnt, Modulus(_startPntPrjAngle + 90, 360), _turnRadius)
				tempSide = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, tempPnt)
				If tempSide = 0 Then
					minAngle = Modulus(_startPntPrjAngle - 90, 360)
				ElseIf tempSide < 1 Then
					tempAngle2 = Math.Abs(_startPntPrjAngle - Functions.ReturnAngleInDegrees(rightStartCircleCentrePnt, targetPnt))
					For tempAngle = 45 To Math.Ceiling(tempAngle2)
						tempPnt = Functions.PointAlongPlane(targetPnt, Modulus(_startPntPrjAngle - tempAngle + 90, 360), _turnRadius)
						firstTurnPnt = Functions.LineLineIntersect(_startPrjPnt, _startPntPrjAngle, tempPnt, Modulus(_startPntPrjAngle - tempAngle + 180, 360))
						If Functions.ReturnDistanceFromGeomInMeters(_startPrjPnt, firstTurnPnt) >= angleStabilizationDist(tempAngle) Then
							If Functions.ReturnDistanceFromGeomInMeters(firstTurnPnt, tempPnt) >= angleStabilizationDist(tempAngle) + angleStabilizationDist(90) Then
								minAngle = Modulus(_startPntPrjAngle - tempAngle - 90, 360)
								Exit For
							End If
						End If
					Next
					If minAngle < 0 Then
						minAngle = Modulus(_startPntPrjAngle - 90, 360)	'Questionable
					End If
				Else

					tempAngle = getTurnAngle(Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, _bankEstablishDistance), _startPntPrjAngle, targetPnt, -1, 1)
					minAngle = Modulus(_startPntPrjAngle + tempAngle - 90, 360)
				End If
			Else

			End If
		Else

		End If
	End Sub

	Private Function angleStabilizationDist(angle As Double) As Double
		Return _turnRadius * Math.Tan(DegToRad(angle / 2)) + _bankEstablishDistance	'added 5 sec flight distance for bank establishing (stabilization distance)
	End Function

	Private Function getTurnAngle(startPnt As IPoint, startAngle As Double, targetPnt As IPoint, side1 As Integer, side2 As Integer) As Double
		Dim centrePnt As IPoint
		If side2 < 0 Then
			centrePnt = Functions.PointAlongPlane(startPnt, Modulus(_startPntPrjAngle - 90, 360), _turnRadius)
		Else
			centrePnt = Functions.PointAlongPlane(startPnt, Modulus(_startPntPrjAngle + 90, 360), _turnRadius)
		End If

		Dim tempAngle As Double = Functions.ReturnAngleInDegrees(centrePnt, targetPnt)
		Dim tempDist As Double = Functions.ReturnDistanceFromGeomInMeters(centrePnt, targetPnt)

		If side1 < 0 Then
			If side2 < 0 Then
				tempAngle = Math.Abs(Math.Abs(_startPntPrjAngle + 90 - tempAngle) - RadToDeg(Math.Asin(2 * _turnRadius / tempDist)))
			Else
				tempAngle = Math.Abs(Math.Abs(_startPntPrjAngle - 90 - tempAngle) - RadToDeg(Math.Asin(2 * _turnRadius / tempDist)))
			End If

		Else
			If side2 < 0 Then
				tempAngle = Math.Abs(Math.Abs(_startPntPrjAngle + 90 - tempAngle) - RadToDeg(Math.Asin(2 * _turnRadius / tempDist)))
			Else
				tempAngle = Math.Abs(Math.Abs(_startPntPrjAngle - 90 - tempAngle) - RadToDeg(Math.Asin(2 * _turnRadius / tempDist)))
			End If
		End If

		Return tempAngle
	End Function

	Public Sub GetAngleRange(targetPnt As IPoint, ByRef minAngle As Double, ByRef maxAngle As Double)
		minAngle = -1
		maxAngle = -1

		Dim tempSide As Integer
		Dim tempDist As Double
		Dim tempPnt As IPoint
		Dim firstTurnPnt1 As IPoint
		Dim firstTurnAngle1 As Double
		Dim firstTurnPnt2 As IPoint
		Dim firstTurnAngle2 As Double

		tempSide = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
		If tempSide = 0 Then
			'Do it at the end
		ElseIf tempSide < 1 Then
			'Max angle
			tempPnt = Functions.PointAlongPlane(targetPnt, Modulus(_startPntPrjAngle - 45 - 90, 360), angleStabilizationDist(90))
			firstTurnPnt1 = Functions.PointAlongPlane(_startPrjPnt, _startPntPrjAngle, angleStabilizationDist(45))
			tempSide = Functions.SideDef(firstTurnPnt1, Modulus(_startPntPrjAngle - 45, 360), tempPnt)
			If tempSide = 0 Then
				'Do it at the end
			ElseIf tempSide < 1 Then
			Else
				firstTurnAngle1 = getTurnAngle(_startPrjPnt, _startPntPrjAngle, targetPnt, -1, -1)
				tempDist = Functions.ReturnDistanceFromGeomInMeters(firstTurnPnt1, tempPnt)
				If tempDist >= angleStabilizationDist(firstTurnAngle1) + angleStabilizationDist(90) Then
					maxAngle = Modulus(_startPntPrjAngle - firstTurnAngle1 + 90, 360)
				Else
					'Smth difficult?
				End If
			End If

			'Min angle

		Else

		End If



	End Sub

    Public Sub GetAngleRange_light(targetPnt As IPoint, ByRef minAngle As Double, ByRef maxAngle As Double, isLeftGE180turn As Boolean, isRightGE180turn As Boolean)
        Dim tempSide As Integer
        tempSide = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
        If isLeftGE180turn Then
            pTopo = CType(leftPolygon, ITopologicalOperator2)
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()
            Dim relatOper As IRelationalOperator = CType(leftPolygon, IRelationalOperator)
            If relatOper.Contains(targetPnt) Then
                minAngle = Modulus(_startPntPrjAngle + 179, 360)
                maxAngle = Modulus(_startPntPrjAngle + 181, 360)
            Else
                minAngle = Modulus(trackSteps(trackSteps.Count - 1).StartPointPrjAngle + 179, 360)
                maxAngle = Modulus(trackSteps(trackSteps.Count - 1).StartPointPrjAngle + 181, 360)
            End If
        ElseIf isRightGE180turn Then
            pTopo = CType(rightPolygon, ITopologicalOperator2)
            pTopo.IsKnownSimple_2 = False
            pTopo.Simplify()
            Dim relatOper As IRelationalOperator = CType(rightPolygon, IRelationalOperator)
            If relatOper.Contains(targetPnt) Then
                minAngle = Modulus(_startPntPrjAngle - 181, 360)
                maxAngle = Modulus(_startPntPrjAngle - 179, 360)
            Else
                minAngle = Modulus(trackSteps(trackSteps.Count - 1).StartPointPrjAngle - 181, 360)
                maxAngle = Modulus(trackSteps(trackSteps.Count - 1).StartPointPrjAngle - 179, 360)
            End If
        Else
            Dim tempAngle As Double
            Dim tempPnt As IPoint

            If tempSide = eSide.OnLine Then
                minAngle = Modulus(_startPntPrjAngle - 90, 360)
                maxAngle = Modulus(_startPntPrjAngle + 90, 360)
            ElseIf tempSide = eSide.Left Then
                tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle + 45, 360), targetPnt)
                If tempSide = eSide.OnLine Then
                    minAngle = Modulus(_startPntPrjAngle + 45 - 90, 360)
                    maxAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                ElseIf tempSide = eSide.Left Then
                    tempPnt = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), angleStabilizationDist(45) + angleStabilizationDist(90))
                    tempSide = Functions.SideDef(tempPnt, Modulus(_startPntPrjAngle + 45 + 90, 360), targetPnt)
                    If tempSide = eSide.OnLine Then
                        tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), targetPnt)
                        If tempSide = eSide.OnLine Then
                            minAngle = Modulus(_startPntPrjAngle + 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                        ElseIf tempSide = eSide.Left Then
                            minAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                        Else 'right
                            minAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                            maxAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                        End If
                    ElseIf tempSide = eSide.Left Then
                        tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), targetPnt)
                        If tempSide = eSide.OnLine Then
                            minAngle = Modulus(_startPntPrjAngle + 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle + 90, 360)
                        ElseIf tempSide = eSide.Left Then
                            'Impossible
                        Else 'right
                            minAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                            maxAngle = Modulus(_startPntPrjAngle + 90, 360)
                        End If
                    Else 'right
                        minAngle = Functions.ReturnAngleInDegrees(tempPnt, targetPnt)
                        maxAngle = Modulus(_startPntPrjAngle + 45 + 90, 360)
                    End If
                Else 'right
                    tempAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                    minAngle = Modulus(tempAngle - 90, 360)
                    maxAngle = Modulus(tempAngle + 90, 360)
                End If
            Else 'right
                tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), targetPnt)
                If tempSide = eSide.OnLine Then
                    minAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                    maxAngle = Modulus(_startPntPrjAngle - 45 + 90, 360)
                ElseIf tempSide = eSide.Left Then
                    tempAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                    minAngle = Modulus(tempAngle - 90, 360)
                    maxAngle = Modulus(tempAngle + 90, 360)
                Else 'right
                    tempPnt = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), angleStabilizationDist(45) + angleStabilizationDist(90))
                    tempSide = Functions.SideDef(tempPnt, Modulus(_startPntPrjAngle - 45 - 90, 360), targetPnt)
                    If tempSide = eSide.OnLine Then
                        tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), targetPnt)
                        If tempSide = eSide.OnLine Then
                            minAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle - 90, 360)
                        ElseIf tempSide = eSide.Left Then
                            minAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                        Else 'right
                            minAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                            maxAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                        End If
                    ElseIf tempSide = eSide.Left Then
                        minAngle = Functions.ReturnAngleInDegrees(tempPnt, targetPnt)
                        maxAngle = Modulus(_startPntPrjAngle - 45 - 90, 360)
                    Else 'right
                        tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), targetPnt)
                        If tempSide = eSide.OnLine Then
                            minAngle = Modulus(_startPntPrjAngle - 90, 360)
                            maxAngle = Modulus(_startPntPrjAngle - 90, 360)
                        ElseIf tempSide = eSide.Left Then
                            minAngle = Modulus(_startPntPrjAngle - 90, 360)
                            maxAngle = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
                        Else 'right
                            'Impossible
                        End If
                    End If
                End If
            End If
        End If

        'minAngle = 90
        'maxAngle = 91

    End Sub

    Public Sub GetDivergenceConvergenceAngles(targetPnt As IPoint, targetAngle As Double, ByRef divAngle As Double, ByRef convAngle As Double)
        Dim tempAngle As Double = Functions.ReturnAngleInDegrees(_startPrjPnt, targetPnt)
        tempAngle = Math.Abs(_startPntPrjAngle - tempAngle)
        If tempAngle > 180 Then
            divAngle = 360 - tempAngle
        End If
        If divAngle > 90 Then
            divAngle = 45
        Else
            divAngle = tempAngle
        End If

        Dim tempSide As Integer = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
        If tempSide = eSide.OnLine Then
            tempAngle = _startPntPrjAngle
        ElseIf tempSide = eSide.Left Then
            tempAngle = Modulus(_startPntPrjAngle + divAngle, 360)
        Else
            tempAngle = Modulus(_startPntPrjAngle - divAngle, 360)
        End If

        convAngle = Math.Abs(targetAngle - tempAngle)
        If convAngle > 180 Then
            convAngle = 360 - convAngle
        End If

        divAngle = Math.Round(divAngle, 0)
        convAngle = Math.Round(convAngle, 0)
    End Sub

    Public Sub DrawStep_2(targetPnt As IPoint, targetPntAngle As Double, divAngle As Double, convAngle As Double, ByRef divPnt As IPoint, ByRef convPnt As IPoint)
        If _trackStepCentrelineElement IsNot Nothing Then
            pGraphics.DeleteElement(_trackStepCentrelineElement)
            _trackStepCentrelineElement = Nothing

        End If
        If _trackStepPolygonElement IsNot Nothing Then
            pGraphics.DeleteElement(_trackStepPolygonElement)
            _trackStepPolygonElement = Nothing
        End If
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        Dim tempPnt As IPoint
        Dim pnt1 As IPoint
        Dim pnt2 As IPoint
        Dim pnt3 As IPoint
        Dim pnt4 As IPoint
        Dim pnt5 As IPoint
        Dim pnt6 As IPoint
        Dim pnt7 As IPoint
        Dim pnt8 As IPoint
        Dim pnt9 As IPoint
        Dim pnt10 As IPoint

        _trackStepCentrelinePntColl = New Polyline
        Dim tempAngle As Double

        Dim tempSide As Integer = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
        If divPnt Is Nothing AndAlso convPnt Is Nothing Then
            divPnt = _startPrjPnt
            If tempSide = eSide.OnLine Then
                convPnt = targetPnt
            ElseIf tempSide = eSide.Left Then
                tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle + 45, 360), targetPnt)
                If tempSide = eSide.Left Then
                    convPnt = Functions.LineLineIntersect(_startPrjPnt, Modulus(_startPntPrjAngle + 45, 360), targetPnt, targetPntAngle)
                Else
                    convPnt = targetPnt
                End If
            Else
                tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), targetPnt)
                If tempSide = eSide.Right Then
                    convPnt = Functions.LineLineIntersect(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), targetPnt, targetPntAngle)
                Else
                    convPnt = targetPnt
                End If
            End If

            Functions.DrawPoint(divPnt, 255, True)
            Functions.DrawPoint(convPnt, 255, True)



        ElseIf divPnt Is Nothing Then
            If tempSide = eSide.OnLine Then
                divPnt = _startPrjPnt
            ElseIf tempSide = eSide.Left Then
                tempAngle = Modulus(_startPntPrjAngle + divAngle, 360)
                tempAngle = targetPntAngle - tempAngle
                If tempAngle > 180 Then
                    tempAngle = tempAngle - 360
                ElseIf tempAngle < -180 Then
                    tempAngle = tempAngle + 360
                End If

                If tempAngle < 0 Then
                    divPnt = Functions.LineLineIntersect(_startPrjPnt, _startPntPrjAngle, convPnt, Modulus(targetPntAngle + convAngle + 180, 360))
                ElseIf tempAngle > 0 Then
                    divPnt = Functions.LineLineIntersect(_startPrjPnt, _startPntPrjAngle, convPnt, Modulus(targetPntAngle - convAngle + 180, 360))
                End If
            Else
                tempAngle = Modulus(_startPntPrjAngle - divAngle, 360)
                tempAngle = targetPntAngle - tempAngle
                If tempAngle > 180 Then
                    tempAngle = tempAngle - 360
                ElseIf tempAngle < -180 Then
                    tempAngle = tempAngle + 360
                End If

                If tempAngle < 0 Then
                    divPnt = Functions.LineLineIntersect(_startPrjPnt, _startPntPrjAngle, convPnt, Modulus(targetPntAngle + convAngle + 180, 360))
                Else
                    divPnt = Functions.LineLineIntersect(_startPrjPnt, _startPntPrjAngle, convPnt, Modulus(targetPntAngle - convAngle + 180, 360))
                End If
            End If
            'Functions.DrawPoint(divPnt, Color.DarkGreen.ToArgb, True)
        Else 'If convPnt Is Nothing Then
            If tempSide = eSide.OnLine Then
                convPnt = targetPnt
            ElseIf tempSide = eSide.Left Then
                convPnt = Functions.LineLineIntersect(divPnt, Modulus(_startPntPrjAngle + divAngle, 360), targetPnt, targetPntAngle)
            Else
                convPnt = Functions.LineLineIntersect(divPnt, Modulus(_startPntPrjAngle - divAngle, 360), targetPnt, targetPntAngle)
            End If
            'Functions.DrawPoint(convPnt, Color.Purple.ToArgb, True)
        End If

        _trackStepCentrelinePntColl.AddPoint(_startPrjPnt)
        _trackStepCentrelinePntColl.AddPoint(divPnt)
        _trackStepCentrelinePntColl.AddPoint(convPnt)
        _trackStepCentrelinePntColl.AddPoint(targetPnt)

        _trackStepCentrelineElement = Functions.DrawPolyLine(CType(_trackStepCentrelinePntColl, Polyline), 255, 2.0, True)


        'Draw Buffer polygon
        tempSide = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
        If tempSide = eSide.OnLine Then

        ElseIf tempSide = eSide.Left Then
            tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle + 45, 360), targetPnt)
            If tempSide = eSide.OnLine Then
                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.LineLineIntersect(pnt1, _startPntPrjAngle, pnt3, targetPntAngle)
                pnt4 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt5 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + divAngle - 90, 360), _corridorSemiWidth)
                pnt6 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt7 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPoint(pnt3)
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt5, pnt6, -1))
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPoint(pnt7)
            ElseIf tempSide = eSide.Left Then
                tempPnt = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle + divAngle + 90, 360), _corridorSemiWidth)

                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.LineLineIntersect(pnt1, _startPntPrjAngle, tempPnt, Modulus(_startPntPrjAngle + divAngle, 360))
                pnt4 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.LineLineIntersect(pnt2, Modulus(_startPntPrjAngle + divAngle, 360), pnt4, targetPntAngle)
                pnt5 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt6 = Functions.PointAlongPlane(convPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt7 = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle + divAngle - 90, 360), _corridorSemiWidth)
                pnt8 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + divAngle - 90, 360), _corridorSemiWidth)
                pnt9 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt10 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPoint(pnt3)
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(convPnt, pnt6, pnt7, -1))
                _trackStepPntColl.AddPoint(pnt7)
                _trackStepPntColl.AddPoint(pnt8)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt8, pnt9, -1))
                _trackStepPntColl.AddPoint(pnt9)
                _trackStepPntColl.AddPoint(pnt10)
            Else
                tempPnt = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle + divAngle + 90, 360), _corridorSemiWidth)

                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle + divAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.LineLineIntersect(pnt1, _startPntPrjAngle, pnt3, Modulus(_startPntPrjAngle + divAngle, 360))
                pnt4 = Functions.PointAlongPlane(convPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt5 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt6 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt8 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + divAngle - 90, 360), _corridorSemiWidth)
                pnt7 = Functions.LineLineIntersect(pnt6, targetPntAngle, pnt8, Modulus(_startPntPrjAngle + divAngle, 360))
                pnt9 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt10 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPoint(pnt3)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(convPnt, pnt3, pnt4, -1))
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPoint(pnt7)
                _trackStepPntColl.AddPoint(pnt8)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt8, pnt9, -1))
                _trackStepPntColl.AddPoint(pnt9)
                _trackStepPntColl.AddPoint(pnt10)
            End If
        Else
            tempSide = Functions.SideDef(_startPrjPnt, Modulus(_startPntPrjAngle - 45, 360), targetPnt)
            If tempSide = eSide.OnLine Then
                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90 - divAngle, 360), _corridorSemiWidth)
                pnt4 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt5 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt7 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt6 = Functions.LineLineIntersect(pnt7, _startPntPrjAngle, pnt5, targetPntAngle)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt2, pnt3, -1))
                _trackStepPntColl.AddPoint(pnt3)
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPoint(pnt7)
            ElseIf tempSide = eSide.Left Then
                tempPnt = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle - divAngle - 90, 360), _corridorSemiWidth)

                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90 - divAngle, 360), _corridorSemiWidth)
                pnt5 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt4 = Functions.LineLineIntersect(pnt3, Modulus(_startPntPrjAngle - divAngle, 360), pnt5, targetPntAngle)
                pnt6 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt7 = Functions.PointAlongPlane(convPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt8 = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle - divAngle - 90, 360), _corridorSemiWidth)
                pnt10 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt9 = Functions.LineLineIntersect(pnt8, Modulus(_startPntPrjAngle - divAngle, 360), pnt10, _startPntPrjAngle)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt2, pnt3, -1))
                _trackStepPntColl.AddPoint(pnt3)
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPoint(pnt7)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(convPnt, pnt7, pnt8, -1))
                _trackStepPntColl.AddPoint(pnt8)
                _trackStepPntColl.AddPoint(pnt9)
                _trackStepPntColl.AddPoint(pnt10)
            Else
                tempPnt = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle - divAngle - 90, 360), _corridorSemiWidth)

                pnt1 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt2 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90, 360), _corridorSemiWidth)
                pnt3 = Functions.PointAlongPlane(divPnt, Modulus(_startPntPrjAngle + 90 - divAngle, 360), _corridorSemiWidth)
                pnt4 = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle + 90 - divAngle, 360), _corridorSemiWidth)
                pnt5 = Functions.PointAlongPlane(convPnt, Modulus(_startPntPrjAngle - divAngle - convAngle + 90, 360), _corridorSemiWidth)
                pnt6 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle + 90, 360), _corridorSemiWidth)
                pnt7 = Functions.PointAlongPlane(targetPnt, Modulus(targetPntAngle - 90, 360), _corridorSemiWidth)
                pnt8 = Functions.LineLineIntersect(tempPnt, Modulus(_startPntPrjAngle - divAngle, 360), pnt7, targetPntAngle)
                pnt10 = Functions.PointAlongPlane(_startPrjPnt, Modulus(_startPntPrjAngle - 90, 360), _corridorSemiWidth)
                pnt9 = Functions.LineLineIntersect(tempPnt, Modulus(_startPntPrjAngle - divAngle, 360), pnt10, _startPntPrjAngle)

                _trackStepPntColl = New Polygon
                _trackStepPntColl.AddPoint(pnt1)
                _trackStepPntColl.AddPoint(pnt2)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(divPnt, pnt2, pnt3, -1))
                _trackStepPntColl.AddPoint(pnt4)
                _trackStepPntColl.AddPoint(pnt5)
                _trackStepPntColl.AddPoint(pnt6)
                _trackStepPntColl.AddPoint(pnt7)
                _trackStepPntColl.AddPointCollection(Functions.CreateArcPolylinePrj(convPnt, pnt7, pnt8, -1))
                _trackStepPntColl.AddPoint(pnt8)
                _trackStepPntColl.AddPoint(pnt9)
                _trackStepPntColl.AddPoint(pnt10)
            End If
        End If

        _trackStepPolygon = CType(_trackStepPntColl, IPolygon)
        _trackStepPolygonElement = Functions.DrawPolygon(_trackStepPolygon, Color.DarkGreen.ToArgb, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross, True)
        GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)


    End Sub

    Public Function GetDivergenceAngle(targetPnt As IPoint, targetPntAngle As Double, convergencePnt As IPoint, convergenceAngle As Double) As Double
        Dim divAngle As Double = -1
        Dim tempAngle As Double

        Dim tempSide As Integer = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)

        If tempSide = eSide.OnLine Then
            divAngle = 0
        ElseIf tempSide = eSide.Left Then
            tempAngle = targetPntAngle - _startPntPrjAngle
            If tempAngle > 180 Then
                tempAngle = tempAngle - 360
            ElseIf tempAngle < -180 Then
                tempAngle = tempAngle + 360
            End If

            If tempAngle < 0 Then
                divAngle = Math.Abs(_startPntPrjAngle - Modulus(targetPntAngle - convergenceAngle, 360))
                If divAngle > 180 Then
                    divAngle = 360 - divAngle
                End If
            Else
                divAngle = Math.Abs(_startPntPrjAngle - Modulus(targetPntAngle + convergenceAngle, 360))
                If divAngle > 180 Then
                    divAngle = 360 - divAngle
                End If
            End If
        Else
            tempAngle = targetPntAngle - _startPntPrjAngle
            If tempAngle > 180 Then
                tempAngle = tempAngle - 360
            ElseIf tempAngle < -180 Then
                tempAngle = tempAngle + 360
            End If

            If tempAngle < 0 Then
                divAngle = Math.Abs(_startPntPrjAngle - Modulus(targetPntAngle + convergenceAngle, 360))
                If divAngle > 180 Then
                    divAngle = 360 - divAngle
                End If
            Else
                divAngle = Math.Abs(_startPntPrjAngle - Modulus(targetPntAngle - convergenceAngle, 360))
                If divAngle > 180 Then
                    divAngle = 360 - divAngle
                End If
            End If
        End If
        Return divAngle
    End Function

    Public Function GetConvergenceAngle(targetPnt As IPoint, targetPntAngle As Double, divergenceAngle As Double) As Double
        Dim convAngle As Double = -1
        Dim tempSide As Integer = Functions.SideDef(_startPrjPnt, _startPntPrjAngle, targetPnt)
        If tempSide = eSide.OnLine Then
            convAngle = Math.Abs(targetPntAngle - _startPntPrjAngle)
        ElseIf tempSide = eSide.Left Then
            convAngle = Math.Abs(targetPntAngle - Modulus(_startPntPrjAngle + divergenceAngle, 360))
            If convAngle > 180 Then
                convAngle = 360 - convAngle
            End If
        Else
            convAngle = Math.Abs(targetPntAngle - Modulus(_startPntPrjAngle - divergenceAngle, 360))
            If convAngle > 180 Then
                convAngle = 360 - convAngle
            End If
        End If
        Return convAngle
    End Function

End Class
