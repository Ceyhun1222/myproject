Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMapUI


Public Class TapFunctions

    Public Declare Sub AboutBox Lib "MathFunctions.dll" Alias "_AboutBox@0" ()
    Public Declare Sub InitAll Lib "MathFunctions.dll" Alias "_InitAll@0" ()
    Public Declare Sub InitEllipsoid Lib "MathFunctions.dll" Alias "_InitEllipsoid@16" (ByVal EquatorialRadius As Double, ByVal InverseFlattening As Double)
    Public Declare Sub SetInverseFlattening Lib "MathFunctions.dll" Alias "_SetInverseFlattening@8" (ByVal InverseFlattening As Double)
    Public Declare Sub SetEquatorialRadius Lib "MathFunctions.dll" Alias "_SetEquatorialRadius@8" (ByVal EquatorialRadius As Double)
    Public Declare Sub InitProjection Lib "MathFunctions.dll" Alias "_InitProjection@40" (ByVal Lm0 As Double, ByVal Lp0 As Double, ByVal Sc As Double, ByVal Efalse As Double, ByVal Nfalse As Double)
    Public Declare Function ATan2 Lib "MathFunctions.dll" Alias "_ATan2@16" (ByVal Y As Double, ByVal X As Double) As Double
    Public Declare Function Modulus Lib "MathFunctions.dll" Alias "_Modulus@16" (ByVal X As Double, Optional ByVal Y As Double = 360.0#) As Double
    Public Declare Function ReturnGeodesicDistance Lib "MathFunctions.dll" Alias "_ReturnGeodesicDistance@32" (ByVal x0 As Double, ByVal Y0 As Double, ByVal x1 As Double, ByVal y1 As Double) As Double
    Public Declare Function DistFromPointToLine Lib "MathFunctions.dll" Alias "_DistFromPointToLine@52" (ByVal xPt As Double, ByVal yPt As Double, ByVal xLn As Double, ByVal yLn As Double, ByVal Azimuth As Double, ByRef xres As Double, ByRef yres As Double, ByRef azimuthres As Double) As Double
    Public Declare Function TriangleBy2PointAndAngle Lib "MathFunctions.dll" Alias "_TriangleBy2PointAndAngle@56" (ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal angle As Double, ByVal h As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
    Public Declare Function ExcludeAreaCreate Lib "MathFunctions.dll" Alias "_ExcludeAreaCreate@64" (ByVal x1 As Double, ByVal y1 As Double, ByVal phi1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal phi2 As Double, ByVal h As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
    Public Declare Function CreatePrevDMEOrbitalFixZone Lib "MathFunctions.dll" Alias "_CreatePrevDMEOrbitalFixZone@76" (ByVal xDME As Double, ByVal yDME As Double, ByVal xVORNDB As Double, ByVal yVORNDB As Double, ByRef Phi As Double, ByRef A As Double, ByRef h As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
    Public Declare Function DMEcircles Lib "MathFunctions.dll" Alias "_DMEcircles@60" (ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal Alpha As Double, ByRef r As Double, ByRef x1res As Double, ByRef y1res As Double, ByRef x2res As Double, ByRef y2res As Double) As Integer
    Public Declare Function CreatePrevDMEFixZone Lib "MathFunctions.dll" Alias "_CreatePrevDMEFixZone@76" (ByVal xDME As Double, ByVal yDME As Double, ByVal toleranceDME As Double, ByVal xVORNDB As Double, ByVal yVORNDB As Double, ByVal A As Double, ByVal h As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
    Public Declare Function CreateFixZone Lib "MathFunctions.dll" Alias "_CreateFixZone@80" (ByVal x1 As Double, ByVal y1 As Double, ByVal phi1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal phi2 As Double, ByVal h As Double, ByVal xFix As Double, ByVal yFix As Double, ByRef xArray As Double, ByRef yArray As Double) As Integer
    Public Declare Function CreatePrevFixZone Lib "MathFunctions.dll" Alias "_CreatePrevFixZone@76" (ByVal P1x As Double, ByVal P1y As Double, ByVal P2x As Double, ByVal P2y As Double, ByVal P2phi As Double, ByVal A As Double, ByVal h As Double, ByRef pCnt As Integer, ByRef x1Array As Double, ByRef y1Array As Double, ByRef x2Array As Double, ByRef y2Array As Double) As Integer
    Public Declare Function EnterToCircle Lib "MathFunctions.dll" Alias "_EnterToCircle@76" (ByVal x0 As Double, ByVal Y0 As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal curAzimuth As Double, ByVal Flag As Integer, ByVal rRMP As Double, ByVal rTouch As Double, ByRef xTouch As Double, ByRef yTouch As Double, ByRef xTurn As Double, ByRef yTurn As Double) As Integer
    Public Declare Function OutFromTurn Lib "MathFunctions.dll" Alias "_OutFromTurn@64" (ByVal x0 As Double, ByVal Y0 As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal Radius As Double, ByVal Azimuth As Double, ByVal Flag As Integer, ByRef resx As Double, ByRef resy As Double, ByRef resAzimuth As Double) As Integer
    Public Declare Function CalcByCourseDistance Lib "MathFunctions.dll" Alias "_CalcByCourseDistance@64" (ByVal x0 As Double, ByVal Y0 As Double, ByVal Azt As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal Dist As Double, ByRef x0res As Double, ByRef y0res As Double, ByRef x1res As Double, ByRef y1res As Double) As Integer
    Public Declare Function Calc2VectIntersect Lib "MathFunctions.dll" Alias "_Calc2VectIntersect@56" (ByVal x0 As Double, ByVal Y0 As Double, ByVal azimuth0 As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal azimuth1 As Double, ByRef resx As Double, ByRef resy As Double) As Integer
    Public Declare Function Calc2DistIntersects Lib "MathFunctions.dll" Alias "_Calc2DistIntersects@64" (ByVal x0 As Double, ByVal Y0 As Double, ByVal dist0 As Double, ByVal x1 As Double, ByVal y1 As Double, ByVal Dist1 As Double, ByRef xres0 As Double, ByRef yres0 As Double, ByRef xres1 As Double, ByRef yres1 As Double) As Integer
    Public Declare Function PointAlongGeodesic Lib "MathFunctions.dll" Alias "_PointAlongGeodesic@40" (ByVal X As Double, ByVal Y As Double, ByVal Dist As Double, ByVal Azimuth As Double, ByRef resx As Double, ByRef resy As Double) As Integer
    Public Declare Function ReturnGeodesicAzimuth Lib "MathFunctions.dll" Alias "_ReturnGeodesicAzimuth@40" (ByVal x0 As Double, ByVal Y0 As Double, ByVal x1 As Double, ByVal y1 As Double, ByRef DirectAzimuth As Double, ByRef InverseAzimuth As Double) As Integer
    Public Declare Sub GeographicToProjection Lib "MathFunctions.dll" Alias "_GeographicToProjection@24" (ByVal X As Double, ByVal Y As Double, ByRef resx As Double, ByRef resy As Double)
    Public Declare Sub ProjectionToGeographic Lib "MathFunctions.dll" Alias "_ProjectionToGeographic@24" (ByVal X As Double, ByVal Y As Double, ByRef resx As Double, ByRef resy As Double)


    Public pSpheroid As ESRI.ArcGIS.Geometry.ISpheroid
    Public pSpRefPrj As ESRI.ArcGIS.Geometry.ISpatialReference
    Public pSpRefShp As ESRI.ArcGIS.Geometry.ISpatialReference
    Public pPCS As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem

    Public Const DegToRadValue As Double = Math.PI / 180.0#
    Public Const RadToDegValue As Double = 180.0# / Math.PI
    Private Const Epsilon As Double = 0.0000000001

    Public Const mEps As Double = 0.000000000001
    Public Const distEps As Double = 0.0001
    Public Const PDGEps As Double = 0.0001
    Public Const degEps As Double = 1.0# / 36000.0#
    Public Const radEps As Double = degEps * DegToRadValue

    Private Structure TIntersections
        Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
        Dim IntersectIndex As Integer
        Dim PointIndex As Integer
        Dim PredIndex As Integer
        Dim Dist As Double
    End Structure

    Private Structure TPointsHeap
        Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
        Dim IsIntersection As Boolean
        Dim IntersectIndex As Integer
        Dim UsedFlag As Integer
    End Structure

    Structure Interval
        'UPGRADE_NOTE: Left was upgraded to Left_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim Left_Renamed As Double
        'UPGRADE_NOTE: Right was upgraded to Right_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim Right_Renamed As Double
        Dim Tag As Integer
    End Structure

    Public Function PointAlongPlane(ByVal ptGeo As IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As IPoint
        dirAngle = DegToRad(dirAngle)
        Dim pClone As IClone
        pClone = ptGeo
        PointAlongPlane = pClone.Clone
        'Set PointAlongPlane = New Point
        PointAlongPlane.PutCoords(ptGeo.X + Dist * Math.Cos(dirAngle), ptGeo.Y + Dist * Math.Sin(dirAngle))
    End Function


    Public Function DegToRad(ByVal X As Double) As Double
        DegToRad = X * (Math.PI / 180)
    End Function

    Function RadToDeg(ByVal X As Double) As Double
        RadToDeg = X * (180 / Math.PI)
    End Function


    Public Function Dir2Azimuth(ByVal ptPrj As IPoint, ByVal dir As Double) As Double
        Dim resD As Double
        Dim resI As Double
        Dim Pt10 As IPoint
        Dim Pt11 As IPoint
        Dim i As Long


        Pt11 = ToGeo(ptPrj)
        Pt10 = ToGeo(PointAlongPlane(ptPrj, dir, 10.0#))

        i = ReturnGeodesicAzimuth(Pt11.X, Pt11.Y, Pt10.X, Pt10.Y, resD, resI)
        Dir2Azimuth = resD
    End Function

    Function Azt2Direction(ByVal ptGeo As IPoint, ByVal Azt As Double) As Double
        Dim pGeo As IGeometry
        Dim pClone As IClone
        Dim pLine As ILine
        Dim resx As Double
        Dim resy As Double

        Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
        'Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint
        Dim j As Long

        '=================================================================================
        'Set Pt11 = ToGeo(ptGeo)

        j = PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0#, Azt, resx, resy)

        pClone = ptGeo
        Pt10 = pClone.Clone

        Pt10.PutCoords(resx, resy)

        pLine = New ESRI.ArcGIS.Geometry.Line

        pLine.PutCoords(ptGeo, Pt10)

        pGeo = pLine
        pGeo.SpatialReference = pSpRefShp
        pGeo.Project(pSpRefPrj)

        Azt2Direction = Modulus(RadToDeg(pLine.Angle), 360.0#)
        Pt10 = Nothing
        pLine = Nothing

    End Function

    Public Function ToGeo(ByVal pGeo As IGeometry) As IGeometry
        Dim pClone As IClone
        pClone = pGeo
        ToGeo = pClone.Clone
        ToGeo.SpatialReference = pSpRefPrj
        ToGeo.Project(pSpRefShp)
    End Function

    Public Function ToProject(ByVal pGeo As IGeometry) As IGeometry
        Dim pClone As IClone
        pClone = pGeo
        ToProject = pClone.Clone
        ToProject.SpatialReference = pSpRefShp
        ToProject.Project(pSpRefPrj)
    End Function

    Public Function IAS2TAS(ByVal IAS As Double, ByVal h As Double, ByVal dT As Double) As Double
        IAS2TAS = IAS * 171233.0# * Math.Sqrt(288.0# + dT - 0.006496 * h) / ((288.0# - 0.006496 * h) ^ 2.628)
    End Function

    Public Function Bank2Radius(ByVal Bank As Double, ByVal V As Double) As Double

        Dim Rv As Double

        Rv = 6.355 * Math.Tan(DegToRad(Bank)) / (Math.PI * V)

        If (Rv > 0.003) Then
            Rv = 0.003
        End If

        If (Rv > 0) Then
            Bank2Radius = V / (20.0# * Math.PI * Rv)
        Else
            Bank2Radius = -1
        End If

    End Function

    Public Function Radius2Bank(ByVal r As Double, ByVal V As Double) As Double

        Radius2Bank = 0.0#

        If (r > 0.0#) Then
            Radius2Bank = RadToDeg(Math.Atan(V * V / (20.0# * r * 6.355)))
        Else
            Radius2Bank = -1
        End If

    End Function

    Function DistToSpeed(ByVal Dist As Double) As Double
        Dim SpeedList() ' As Double
        'ReDim SpeedList(0 To 25)
        Dim i As Long
        Dim fTmp As Double
        SpeedList = New Object() {356.0#, 370.0#, 387.0#, 404.0#, 424.0#, 441.0#, 452.0#, 459.0#, 467.0#, 472.0#, 478.0#, 483.0#, 487.0#, 491.0#, 493.0#, 494.0#, 498.0#, 502.0#, 504.0#, 511.0#, 515.0#, 519.0#, 524.0#, 526.0#, 530.0#}
        fTmp = Dist / 1852.0#
        i = Math.Round(fTmp + 0.4999999)
        If i > 24 Then i = 24
        If i < 0 Then i = 0
        DistToSpeed = SpeedList(i)
    End Function

    Function HeightToBank(ByVal h As Double) As Double
        If h > 914.4 Then
            HeightToBank = 25.0#
        ElseIf h > 304.8 Then
            HeightToBank = 20.0#
        Else
            HeightToBank = 15.0#
        End If
    End Function

    Public Function ArcSin(ByVal X As Double) As Double
        If Math.Abs(X) >= 1.0# Then
            If X > 0.0# Then
                ArcSin = 0.5 * Math.PI
            Else
                ArcSin = -0.5 * Math.PI
            End If
        Else
            ArcSin = Math.Atan(X / Math.Sqrt(-X * X + 1.0#))
        End If
    End Function

    Public Function ArcCos(ByVal X As Double) As Double
        If Math.Abs(X) >= 1.0# Then
            ArcCos = 0.0#
        Else
            ArcCos = Math.Atan(-X / Math.Sqrt(-X * X + 1.0#)) + 0.5 * Math.PI
        End If
    End Function

    Public Function max(ByVal X As Double, ByVal y As Double) As Double
        If X > y Then
            max = X
        Else
            max = y
        End If
    End Function

    Public Function min(ByVal X As Double, ByVal y As Double) As Double
        If X < y Then
            min = X
        Else
            min = y
        End If
    End Function

    Public Function CreatePrjCircle(ByVal pPoint1 As IPoint, ByVal r As Double) As IPointCollection
        Dim i As Long
        Dim iInRad As Double
        Dim pt As IPoint
        Dim pPolygon As IPointCollection
        Dim pTopo As ITopologicalOperator2

        pt = New Point
        pPolygon = New Polygon
        'toRad = Pi / 180#
        For i = 0 To 359
            iInRad = DegToRad(i)
            pt.X = pPoint1.X + r * Math.Cos(iInRad)
            pt.Y = pPoint1.Y + r * Math.Sin(iInRad)
            pPolygon.AddPoint(pt)
        Next i


        pTopo = pPolygon
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        CreatePrjCircle = pPolygon
    End Function

    Function CreateArcPrj(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection

        Dim i As Integer
        Dim j As Integer
        Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
        Dim pt As ESRI.ArcGIS.Geometry.IPoint
        Dim r As Double
        Dim AngStep As Double
        Dim dX As Double
        Dim dY As Double
        Dim AztFrom As Double
        Dim AztTo As Double
        Dim iInRad As Double
        'Dim pGeo As ESRI.ArcGIS.Geometry.IGeometry

        ptCur = New ESRI.ArcGIS.Geometry.Point
        pt = New ESRI.ArcGIS.Geometry.Point
        CreateArcPrj = New ESRI.ArcGIS.Geometry.Polygon

        dX = ptFrom.X - ptCnt.X
        dY = ptFrom.Y - ptCnt.Y
        r = System.Math.Sqrt(dX * dX + dY * dY)

        AztFrom = RadToDeg(ATan2(dY, dX))
        AztFrom = Modulus(AztFrom, 360.0#)

        AztTo = RadToDeg(ATan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X))
        AztTo = Modulus(AztTo, 360.0#)



        Dim daz As Double
        'Dim saz As Short

        daz = Modulus((AztTo - AztFrom) * ClWise, 360.0#)
        AngStep = 1
        i = daz / AngStep
        If (i < 10) Then i = 10
        AngStep = daz / i

        CreateArcPrj.AddPoint(ptFrom)
        For j = 1 To i - 1
            iInRad = DegToRad(AztFrom + j * AngStep * ClWise)
            pt.X = ptCnt.X + r * System.Math.Cos(iInRad)
            pt.Y = ptCnt.Y + r * System.Math.Sin(iInRad)
            CreateArcPrj.AddPoint(pt)
        Next j
        CreateArcPrj.AddPoint(ptTo)

    End Function

    Function CreateArcPrj2(ByVal ptCnt As IPoint, ByVal ptFrom As IPoint, _
    ByVal ptTo As IPoint, ByVal ClWise As Long) As Path

        Dim i As Long
        Dim j As Long
        Dim ptCur As IPoint
        Dim pt As IPoint
        Dim r As Double
        Dim AngStep As Double
        Dim dX As Double
        Dim dY As Double
        Dim AztFrom As Double
        Dim AztTo As Double
        Dim iInRad As Double
        'Dim pGeo As IGeometry

        ptCur = New Point
        pt = New Point

        Dim pcol As IPointCollection
        pcol = New Path

        dX = ptFrom.X - ptCnt.X
        dY = ptFrom.Y - ptCnt.Y
        r = Math.Sqrt(dX * dX + dY * dY)

        AztFrom = RadToDeg(ATan2(dY, dX))
        AztFrom = Modulus(AztFrom, 360.0#)

        AztTo = RadToDeg(ATan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X))
        AztTo = Modulus(AztTo, 360.0#)

        Dim daz As Double
        'Dim saz As Integer

        daz = Modulus((AztTo - AztFrom) * ClWise, 360.0#)
        AngStep = 1
        i = daz / AngStep
        If (i < 10) Then i = 10
        AngStep = daz / i

        pcol.AddPoint(ptFrom)
        For j = 1 To i - 1
            iInRad = DegToRad(AztFrom + j * AngStep * ClWise)
            pt.X = ptCnt.X + r * Math.Cos(iInRad)
            pt.Y = ptCnt.Y + r * Math.Sin(iInRad)
            pcol.AddPoint(pt)
        Next j
        pcol.AddPoint(ptTo)

        CreateArcPrj2 = pcol

    End Function

    Function CreateArcPrj_ByArc(ByVal ptCnt As IPoint, ByVal ptFrom As IPoint, _
    ByVal ptTo As IPoint, ByVal side As Long) As ISegment

        Dim ClWise As Boolean
        Dim Rds As Double
        Dim isMinor As Boolean
        Rds = ReturnDistanceAsMeter(ptCnt, ptFrom)
        isMinor = (Modulus(side * (ptTo.M - ptFrom.M), 360.0#) > 180.0#)

        Dim ha As Double
        ha = Modulus(-side * (ptTo.M - ptFrom.M), 360.0#)
        Dim ptMid As IPoint
        'Set ptMid = PointAlongPlane(ptCnt, ptFrom.M - (side * ha / 2), Rds)
        ptMid = PointAlongPlane(ptCnt, ReturnAngleAsDegree(ptCnt, ptFrom) - side * (ha / 2), Rds)

        'DrawPoint ptMid
        ClWise = (side = 1)

        Dim pCircularArc As IConstructCircularArc
        pCircularArc = New CircularArc
        'pCircularArc.ConstructEndPointsRadius ptFrom, ptTo, Not ClWise, Rds, isMinor
        pCircularArc.ConstructThreePoints(ptFrom, ptMid, ptTo, True)
        CreateArcPrj_ByArc = pCircularArc

    End Function

    Function CreateArcPrj_ByArc_2(ByVal ptCnt As IPoint, ByVal ptFrom As IPoint, _
    ByVal ptTo As IPoint, ByVal side As Long) As ISegment

        Dim ClWise As Boolean
        Dim Rds As Double
        Dim isMinor As Boolean
        Rds = ReturnDistanceAsMeter(ptCnt, ptFrom)
        isMinor = (Modulus(side * (ptTo.M - ptFrom.M), 360.0#) > 180.0#)

        ClWise = (side = 1)

        Dim pCircularArc As IConstructCircularArc
        pCircularArc = New CircularArc
        pCircularArc.ConstructEndPointsRadius(ptFrom, ptTo, Not ClWise, Rds, isMinor)
        CreateArcPrj_ByArc_2 = pCircularArc

    End Function

    Function SpiralTouchAngle(ByVal r0 As Double, ByVal E As Double, _
    ByVal aztNominal As Double, ByVal AztTouch As Double, ByVal turnDir As Long) As Double

        Dim i As Long
        Dim TurnAngle As Double
        Dim TouchAngle As Double
        Dim d As Double
        Dim delta As Double
        Dim DegE As Double

        TouchAngle = Modulus((AztTouch - aztNominal) * turnDir, 360.0#)
        TouchAngle = DegToRad(TouchAngle)
        TurnAngle = TouchAngle
        DegE = RadToDeg(E)

        For i = 0 To 9
            d = DegE / (r0 + DegE * TurnAngle)
            delta = (TurnAngle - TouchAngle - Math.Atan(d)) / (2.0# - 1.0# / (d * d + 1.0#))
            TurnAngle = TurnAngle - delta
            If (Math.Abs(delta) < radEps) Then
                Exit For
            End If
        Next i
        SpiralTouchAngle = RadToDeg(TurnAngle)

        If SpiralTouchAngle < 0.0# Then
            SpiralTouchAngle = Modulus(SpiralTouchAngle, 360.0#)
        End If

    End Function

    Public Function ReturnAngleAsDegree(ByVal ptFrom As IPoint, ByVal ptOut As IPoint) As Double
        Dim fdX As Double, fdY As Double
        fdX = ptOut.X - ptFrom.X
        fdY = ptOut.Y - ptFrom.Y
        ReturnAngleAsDegree = Modulus(RadToDeg(ATan2(fdY, fdX)), 360.0#)
    End Function

    Public Function ReturnDistanceAsMeter(ByVal ptFrom As IPoint, ByVal ptOut As IPoint) As Double
        Dim fdX As Double, fdY As Double
        fdX = ptOut.X - ptFrom.X
        fdY = ptOut.Y - ptFrom.Y
        ReturnDistanceAsMeter = Math.Sqrt(fdX * fdX + fdY * fdY)
    End Function

    Function SubtractAngles(ByVal X As Double, ByVal y As Double) As Double
        X = Modulus(X, 360.0#)
        y = Modulus(y, 360.0#)
        SubtractAngles = Modulus(X - y, 360.0#)
        If SubtractAngles > 180.0# Then SubtractAngles = 360.0# - SubtractAngles
    End Function

    Function SubtractAnglesWithSign(ByVal StRad As Double, ByVal EndRad As Double, ByVal Turn As Long) As Double
        SubtractAnglesWithSign = Modulus((EndRad - StRad) * Turn, 360.0#)
        If SubtractAnglesWithSign > 180.0# Then
            SubtractAnglesWithSign = SubtractAnglesWithSign - 360.0#
        End If
    End Function

    Public Function Quadric(ByVal A As Double, ByVal B As Double, ByVal C As Double, ByVal x0 As Double, ByVal x1 As Double) As Long
        Dim d As Double
        Dim fTmp As Double

        d = B * B - 4 * A * C
        If d < 0.0# Then
            Quadric = 0
        ElseIf (d = 0.0#) Or (A = 0.0#) Then
            Quadric = 1
            If A = 0.0# Then
                x0 = -C / B
            Else
                x0 = -0.5 * B / A
            End If
        Else
            Quadric = 2
            fTmp = 0.5 / A
            If fTmp > 0 Then
                x0 = (-B - Math.Sqrt(d)) * fTmp
                x1 = (-B + Math.Sqrt(d)) * fTmp
            Else
                x0 = (-B + Math.Sqrt(d)) * fTmp
                x1 = (-B - Math.Sqrt(d)) * fTmp
            End If
        End If

    End Function

    Public Function LineLineIntersect(ByVal Pt1 As ESRI.ArcGIS.Geometry.IPoint, ByVal dir1 As Double, ByVal pt2 As ESRI.ArcGIS.Geometry.IPoint, ByVal dir2 As Double) As ESRI.ArcGIS.Geometry.IPoint
        Dim Constructor As IConstructPoint

        LineLineIntersect = New ESRI.ArcGIS.Geometry.Point
        Constructor = LineLineIntersect
        Constructor.ConstructAngleIntersection(Pt1, DegToRad(dir1), pt2, DegToRad(dir2))
    End Function

    Public Function CircleVectorIntersect(ByVal ptCent As ESRI.ArcGIS.Geometry.IPoint, ByVal r As Double, ByVal ptVect As ESRI.ArcGIS.Geometry.IPoint, ByVal DirVect As Double, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
        Dim DistCnt2Vect As Double
        'Dim fTmp As Double
        Dim d As Double
        Dim Constr As ESRI.ArcGIS.Geometry.IConstructPoint

        ptTmp = New ESRI.ArcGIS.Geometry.Point
        Constr = ptTmp

        'DrawPoint PtCent, 255
        'DrawPoint ptVect, 0

        Constr.ConstructAngleIntersection(ptCent, DegToRad(DirVect + 90.0#), ptVect, DegToRad(DirVect))

        DistCnt2Vect = ReturnDistanceAsMeter(ptCent, ptTmp)

        If DistCnt2Vect < r Then
            d = System.Math.Sqrt(r * r - DistCnt2Vect * DistCnt2Vect)
            ptRes = PointAlongPlane(ptTmp, DirVect, d)
            CircleVectorIntersect = d 'ReturnDistanceAsMeter(ptRes, ptTmp)
        Else
            CircleVectorIntersect = -1.0#
            ptRes = New ESRI.ArcGIS.Geometry.Point
        End If

    End Function

    Public Function CircleCircleIntersect(ByRef PtCent1 As ESRI.ArcGIS.Geometry.IPoint, ByRef R1 As Double, ByRef PtCent2 As ESRI.ArcGIS.Geometry.IPoint, ByRef R2 As Double, ByRef turnDir As Integer, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
        Dim Dist As Double
        Dim h As Double
        Dim A As Double
        Dim x2 As Double
        Dim y2 As Double

        Dist = ReturnDistanceAsMeter(PtCent1, PtCent2)
        ptRes = New ESRI.ArcGIS.Geometry.Point
        If Dist <= R1 + R2 Then
            A = (R1 * R1 - R2 * R2 + Dist * Dist) / (2 * Dist)
            h = System.Math.Sqrt(R1 * R1 - A * A)
            x2 = PtCent1.X + A * (PtCent2.X - PtCent1.X) / Dist
            y2 = PtCent1.Y + A * (PtCent2.Y - PtCent1.Y) / Dist

            ptRes.X = x2 + turnDir * h * (PtCent2.Y - PtCent1.Y) / Dist
            ptRes.Y = y2 - turnDir * h * (PtCent2.X - PtCent1.X) / Dist
            CircleCircleIntersect = h
        Else
            CircleCircleIntersect = -1.0#
        End If
    End Function


    Public Function SideDef(ByVal PtInLine As IPoint, ByVal LineAngle As Double, ByVal PtOutLine As IPoint) As Long
        Dim Angle12 As Double
        Dim dAngle As Double

        Angle12 = ReturnAngleAsDegree(PtInLine, PtOutLine)
        dAngle = LineAngle - Angle12
        dAngle = Modulus(dAngle, 360.0#)

        If (dAngle = 0.0#) Or (dAngle = 180.0#) Then
            SideDef = 0
            Exit Function
        End If

        If (dAngle < 180.0#) Then
            SideDef = 1
        Else
            SideDef = -1
        End If

    End Function

    Public Function SideFrom2Angle(ByVal Angle0 As Double, ByVal Angle1 As Double) As Long
        Dim dAngle As Double

        dAngle = SubtractAngles(Angle0, Angle1)

        If (180.0# - dAngle < degEps) Or (dAngle < degEps) Then
            SideFrom2Angle = 0
            Exit Function
        End If

        dAngle = Modulus(Angle1 - Angle0, 360.0#)

        If (dAngle < 180.0#) Then
            SideFrom2Angle = 1
        Else
            SideFrom2Angle = -1
        End If

    End Function

    Function AnglesSideDef(ByVal X As Double, ByVal y As Double) As Long
        Dim Z As Double
        Z = Modulus(X - y, 360.0#)
        If Z = 0.0# Then
            AnglesSideDef = 0
        ElseIf Z > 180.0# Then
            AnglesSideDef = -1
        ElseIf Z < 180.0# Then
            AnglesSideDef = 1
        Else
            AnglesSideDef = 2
        End If
    End Function

    Sub CreateWindSpiral(ByVal pt As IPoint, ByVal aztNom As Double, ByVal AztSt As Double, ByVal AztEnd As Double, _
    ByVal r0 As Double, ByVal coef As Double, _
    ByVal turnDir As Long, ByVal pPointCollection As IPointCollection)
        Dim dAlpha As Double
        Dim N As Long
        Dim i As Long
        Dim TurnAng As Double
        Dim ptCur As IPoint
        Dim azt0 As Double
        Dim r As Double
        Dim dPhi As Double
        Dim dPhi0 As Double
        Dim ptCnt As IPoint

        ptCnt = PointAlongPlane(pt, aztNom + 90.0# * turnDir, r0)
        'DrawPoint ptCnt, 0
        'DrawPoint Pt, 255

        If SubtractAngles(aztNom, AztEnd) < degEps Then
            AztEnd = aztNom
        End If

        dPhi0 = (AztSt - aztNom) * turnDir
        dPhi0 = Modulus(dPhi0, 360.0#)

        If (dPhi0 < 0.001) Then
            dPhi0 = 0.0#
        Else
            dPhi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, turnDir)
        End If

        'DrawPolygon pPointCollection, 0

        dPhi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, turnDir)

        TurnAng = dPhi - dPhi0

        azt0 = aztNom + (dPhi0 - 90.0#) * turnDir
        azt0 = Modulus(azt0, 360.0#)

        If (TurnAng < 0.0#) Then
            Exit Sub
        End If

        dAlpha = 1.0#
        N = TurnAng / dAlpha
        If (N < 10) Then
            N = 10
        End If

        dAlpha = TurnAng / N

        ptCur = New Point
        For i = 0 To N
            r = r0 + (dAlpha * coef * i) + dPhi0 * coef
            ptCur = PointAlongPlane(ptCnt, azt0 + (i * dAlpha) * turnDir, r)
            '    DrawPoint ptCur, 233

            pPointCollection.AddPoint(ptCur)
        Next i

    End Sub

    Public Function RayPolylineIntersect(ByVal pPolyLine As Polyline, ByVal RayPt As Point, ByVal RayDir As Double, ByVal InterPt As IPoint) As Boolean
        Dim i As Long
        Dim N As Long
        Dim d As Double
        Dim dMin As Double
        Dim pLine As IPolyline
        Dim pPoints As IPointCollection
        Dim pTopo As ITopologicalOperator

        pLine = New Polyline
        pLine.FromPoint = RayPt
        dMin = 5000000.0#
        pLine.ToPoint = PointAlongPlane(RayPt, RayDir, dMin)

        pTopo = pPolyLine
        pPoints = pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension)
        N = pPoints.PointCount

        RayPolylineIntersect = N > 0

        If N = 0 Then
            Exit Function
        End If

        If N = 1 Then
            InterPt = pPoints.Point(0)
        Else
            For i = 0 To N - 1
                d = ReturnDistanceAsMeter(RayPt, pPoints.Point(i))
                If d < dMin Then
                    dMin = d
                    InterPt = pPoints.Point(i)
                End If
            Next
        End If

    End Function

    Function AngleInSector(ByVal angle As Double, ByVal X As Double, ByVal y As Double) As Boolean
        Dim Sector As Double
        Dim Sub1 As Double
        Dim Sub2 As Double

        Sector = SubtractAngles(X, y)
        Sub1 = SubtractAngles(X, angle)
        Sub2 = SubtractAngles(angle, y)

        AngleInSector = Not (Sub1 + Sub2 > Sector + degEps)

    End Function

    Public Function DrawPointWithText(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByRef pPoint As ESRI.ArcGIS.Geometry.Point, ByRef sText As String, Optional ByRef FontSize As Double = 10, Optional ByRef Color As Integer = -1, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
        Dim pMarkerShpElement As ESRI.ArcGIS.Carto.IMarkerElement
        Dim pElementofpPoint As ESRI.ArcGIS.Carto.IElement
        Dim pMarkerSym As ESRI.ArcGIS.Display.ISimpleMarkerSymbol

        Dim pTextElement As ESRI.ArcGIS.Carto.ITextElement
        Dim pElementOfText As ESRI.ArcGIS.Carto.IElement
        Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
        Dim pCommonElement As ESRI.ArcGIS.Carto.IElement
        Dim pTextSymbol As ESRI.ArcGIS.Display.ITextSymbol

        pTextElement = New ESRI.ArcGIS.Carto.TextElement
        pElementOfText = pTextElement

        pTextSymbol = New ESRI.ArcGIS.Display.TextSymbol
        pTextSymbol.HorizontalAlignment = ESRI.ArcGIS.Display.esriTextHorizontalAlignment.esriTHALeft
        pTextSymbol.VerticalAlignment = ESRI.ArcGIS.Display.esriTextVerticalAlignment.esriTVABottom
        pTextSymbol.Size = FontSize


        pTextElement.Text = sText
        pTextElement.ScaleText = False
        pTextElement.Symbol = pTextSymbol

        'UPGRADE_WARNING: Couldn't resolve default property of object pPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementOfText.Geometry = pPoint

        pMarkerShpElement = New ESRI.ArcGIS.Carto.MarkerElement

        pElementofpPoint = pMarkerShpElement
        'UPGRADE_WARNING: Couldn't resolve default property of object pPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementofpPoint.Geometry = pPoint

        pRGB = New ESRI.ArcGIS.Display.RgbColor
        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        pMarkerSym = New ESRI.ArcGIS.Display.SimpleMarkerSymbol
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerSym.Color = pRGB
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym.size. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerSym.Size = 6
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerShpElement.Symbol = pMarkerSym

        pGroupElement = New ESRI.ArcGIS.Carto.GroupElement
        pGroupElement.AddElement(pElementofpPoint)
        'UPGRADE_WARNING: Couldn't resolve default property of object pTextElement. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pGroupElement.AddElement(pTextElement)

        pCommonElement = pGroupElement
        DrawPointWithText = pCommonElement

        If drawFlg Then
            CurrentActiveView.GraphicsContainer.AddElement(pCommonElement, 0)
            ' Refresh_PartialActiveView()
            'If G.Application Is Nothing Then
            '    G.CurrentActiveView.Refresh()
            'Else
            '    G.CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
            'End If
            CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Function

    Public Function DrawPoint(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByVal pPoint As ESRI.ArcGIS.Geometry.IPoint, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
        Dim pMarkerShpElement As ESRI.ArcGIS.Carto.IMarkerElement
        Dim pElementofpPoint As ESRI.ArcGIS.Carto.IElement
        Dim pMarkerSym As ESRI.ArcGIS.Display.ISimpleMarkerSymbol
        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor

        pMarkerShpElement = New ESRI.ArcGIS.Carto.MarkerElement

        pElementofpPoint = pMarkerShpElement
        'UPGRADE_WARNING: Couldn't resolve default property of object pPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementofpPoint.Geometry = pPoint

        pRGB = New ESRI.ArcGIS.Display.RgbColor
        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        pMarkerSym = New ESRI.ArcGIS.Display.SimpleMarkerSymbol
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerSym.Color = pRGB
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym.size. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerSym.Size = 8
        'UPGRADE_WARNING: Couldn't resolve default property of object pMarkerSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pMarkerShpElement.Symbol = pMarkerSym

        DrawPoint = pElementofpPoint

        If drawFlg Then
            CurrentActiveView.GraphicsContainer.AddElement(pElementofpPoint, 0)
            'Refresh_PartialActiveView()

            CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If
    End Function

    Function DrawLine(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByRef pLine As ESRI.ArcGIS.Geometry.Line, Optional ByRef Color As Integer = -1, Optional ByRef width As Double = 1.0#, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
        Dim pLineElement As ESRI.ArcGIS.Carto.ILineElement
        Dim pElementOfpLine As ESRI.ArcGIS.Carto.IElement
        Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
        'Dim pGeometry As IGeometry

        pLineElement = New ESRI.ArcGIS.Carto.LineElement
        pElementOfpLine = pLineElement
        'Set pGeometry = pLine

        'UPGRADE_WARNING: Couldn't resolve default property of object pLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementOfpLine.Geometry = pLine

        pRGB = New ESRI.ArcGIS.Display.RgbColor
        pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol

        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Color = pRGB
        pLineSym.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid
        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.width. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Width = width

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineElement.Symbol = pLineSym
        DrawLine = pElementOfpLine

        If drawFlg Then
            CurrentActiveView.GraphicsContainer.AddElement(pElementOfpLine, 0)
            'Refresh_PartialActiveView()

            CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Function

    Function DrawLayoutLine(ByRef pDocument As ESRI.ArcGIS.ArcMapUI.IMxDocument, ByRef pLine As ESRI.ArcGIS.Geometry.Line, Optional ByRef Color As Integer = -1, Optional ByRef width As Double = 1.0#, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement

        Dim pLineElement As ESRI.ArcGIS.Carto.ILineElement
        Dim pElementOfpLine As ESRI.ArcGIS.Carto.IElement
        Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
        'Dim pGeometry As IGeometry

        pLineElement = New ESRI.ArcGIS.Carto.LineElement
        pElementOfpLine = pLineElement
        'Set pGeometry = pLine

        'Dim pPolyLine As IPolyline
        'pPolyLine.FromPoint =

        'UPGRADE_WARNING: Couldn't resolve default property of object pLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementOfpLine.Geometry = pLine

        pRGB = New ESRI.ArcGIS.Display.RgbColor
        pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol

        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Color = pRGB
        pLineSym.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid
        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.width. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Width = width

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineElement.Symbol = pLineSym
        DrawLayoutLine = pElementOfpLine

        Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
        If drawFlg Then
            pGraphics = pDocument.PageLayout '.ActivatedView.GraphicsContainer
            pGraphics.AddElement(pElementOfpLine, 0)
            pDocument.ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Function

    Public Function DrawPolyLine(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByVal pLine As ESRI.ArcGIS.Geometry.Polyline, Optional ByVal Color As Integer = -1, Optional ByVal width As Double = 1.0#, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement

        Dim pLineElement As ESRI.ArcGIS.Carto.ILineElement
        Dim pElementOfpLine As ESRI.ArcGIS.Carto.IElement
        Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor

        pRGB = New ESRI.ArcGIS.Display.RgbColor


        pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol

        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Color = pRGB
        pLineSym.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid
        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym.width. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSym.Width = width

        pLineElement = New ESRI.ArcGIS.Carto.LineElement

        pElementOfpLine = pLineElement
        'UPGRADE_WARNING: Couldn't resolve default property of object pLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementOfpLine.Geometry = pLine

        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineElement.Symbol = pLineSym
        DrawPolyLine = pElementOfpLine

        If drawFlg Then
            CurrentActiveView.GraphicsContainer.AddElement(pElementOfpLine, 0)
            'Refresh_PartialActiveView()
            CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Function

    Public Function DrawPolygon(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon, Optional ByRef Color As Integer = -1, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement

        Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
        Dim pFillSym As ESRI.ArcGIS.Display.ISimpleFillSymbol
        Dim pFillShpElement As ESRI.ArcGIS.Carto.IFillShapeElement
        Dim pLineSimbol As ESRI.ArcGIS.Display.ILineSymbol

        Dim pElementofPoly As ESRI.ArcGIS.Carto.IElement

        pRGB = New ESRI.ArcGIS.Display.RgbColor
        pFillSym = New ESRI.ArcGIS.Display.SimpleFillSymbol
        pFillShpElement = New ESRI.ArcGIS.Carto.PolygonElement

        pElementofPoly = pFillShpElement
        'UPGRADE_WARNING: Couldn't resolve default property of object pPolygon. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pElementofPoly.Geometry = pPolygon

        If Color <> -1 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object pRGB.RGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pRGB.RGB = Color
        Else
            pRGB.Red = System.Math.Round(255 * Rnd())
            pRGB.Green = System.Math.Round(255 * Rnd())
            pRGB.Blue = System.Math.Round(255 * Rnd())
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object pFillSym.Color. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pFillSym.Color = pRGB
        pFillSym.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull 'esriSFSDiagonalCross

        pLineSimbol = New ESRI.ArcGIS.Display.SimpleLineSymbol
        'UPGRADE_WARNING: Couldn't resolve default property of object pRGB. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pLineSimbol.Color = pRGB
        pLineSimbol.Width = 1
        'UPGRADE_WARNING: Couldn't resolve default property of object pFillSym.Outline. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object pLineSimbol. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pFillSym.Outline = pLineSimbol

        'UPGRADE_WARNING: Couldn't resolve default property of object pFillSym. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pFillShpElement.Symbol = pFillSym
        DrawPolygon = pElementofPoly

        If drawFlg Then
            CurrentActiveView.GraphicsContainer.AddElement(pElementofPoly, 0)
            'Refresh_PartialActiveView()
            CurrentActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        End If

    End Function

    Function DrawSectorPrj(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef r As Double, ByRef stDir As Double, ByRef endDir As Double, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
        'Function DrawSectorPrj(PtCnt As IPoint, ptFrom As IPoint, _
        ''            ptTo As IPoint, ClWise As Long) As IPointCollection
        Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
        ptFrom = PointAlongPlane(ptCnt, stDir, r)
        ptTo = PointAlongPlane(ptCnt, endDir, r)
        DrawSectorPrj = CreateArcPrj(ptCnt, ptFrom, ptTo, ClWise)
        DrawSectorPrj.AddPoint(ptCnt)
        DrawSectorPrj.AddPoint(ptCnt, 0)
    End Function


    Public Sub GetIntData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara As Long, ByVal size As Long)
        Dim i As Long
        Dim E As Double
        'Dim Sign As Long
        E = 1
        Vara = 0

        For i = 0 To size - 1
            Vara = Data(index + i) * E + Vara
            E = E * 256
        Next i

        index = index + size
    End Sub

    Public Sub GetStrData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara As String, ByVal size As Long)
        Dim i As Long
        Vara = ""

        For i = 0 To size - 1
            Vara = Vara + Chr(Data(index + i))
        Next i

        index = index + size
    End Sub

    Public Sub GetDoubleData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara As Double, ByVal size As Long)
        Dim i As Long
        Dim Sign As Long

        Dim mantissa As Double
        Dim exponent As Long
        'Dim lTmp As Long

        mantissa = 0
        exponent = 0

        For i = 0 To size - 3
            mantissa = Data(index + i) + mantissa / 256.0#
        Next i

        mantissa = (((Data(index + size - 2) And 15) + 16) + mantissa / 256.0#) / 16.0#

        exponent = (Data(index + size - 2) And 240) / 16.0#
        exponent = Data(index + size - 1) * 16.0# + exponent

        If mantissa = 1 And exponent = 0 Then
            Vara = 0.0#
            index = index + size
            Exit Sub
        End If

        Sign = exponent And 2048
        exponent = (exponent And 2047) - 1023
        If exponent > 0 Then
            For i = 1 To exponent
                mantissa = mantissa * 2.0#
            Next i
        ElseIf exponent < 0 Then
            For i = -1 To exponent Step -1
                mantissa = mantissa / 2.0#
            Next i
        End If

        If Sign <> 0 Then mantissa = -mantissa
        Vara = mantissa
        index = index + size
    End Sub

    Public Function Point2LineDistancePrj(ByVal pt As IPoint, ByVal ptLine As IPoint, ByVal Azt As Double) As Double
        Dim cosx, siny As Double
        Azt = DegToRad(Azt)
        cosx = Math.Cos(Azt)
        siny = Math.Sin(Azt)
        Point2LineDistancePrj = Math.Abs((pt.Y - ptLine.Y) * cosx - (pt.X - ptLine.X) * siny)
        'k = 0
    End Function

    Public Function LineVectIntersect(ByVal Pt1 As IPoint, ByVal pt2 As IPoint, ByVal ptVect As IPoint, ByVal Azt As Double, ByVal ptRes As IPoint) As Long
        Dim Az As Double
        Dim SinAz As Double
        Dim CosAz As Double
        Dim UaDenom As Double
        Dim UaNumer As Double
        Dim Ua As Double

        Az = DegToRad(Azt)
        SinAz = Math.Sin(Az)
        CosAz = Math.Cos(Az)

        ptRes = New Point

        UaDenom = SinAz * (pt2.X - Pt1.X) - CosAz * (pt2.Y - Pt1.Y)
        If UaDenom = 0.0# Then
            LineVectIntersect = -2
            Exit Function
        End If

        UaNumer = CosAz * (Pt1.Y - ptVect.Y) - SinAz * (Pt1.X - ptVect.X)

        Ua = UaNumer / UaDenom
        If Ua < 0.0# Then
            LineVectIntersect = -1
        ElseIf Ua > 1.0# Then
            LineVectIntersect = 1
        Else
            LineVectIntersect = 0
        End If

        ptRes.PutCoords(Pt1.X + Ua * (pt2.X - Pt1.X), Pt1.Y + Ua * (pt2.Y - Pt1.Y))

    End Function

    Public Function IntervalsDifference(ByRef A As Interval, ByRef B As Interval) As Interval()
        Dim Res() As Interval

        ReDim Res(0)

        If (B.Left_Renamed = B.Right_Renamed) Or (B.Right_Renamed < A.Left_Renamed) Or (A.Right_Renamed < B.Left_Renamed) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object Res(0). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Res(0) = A
        ElseIf (A.Left_Renamed < B.Left_Renamed) And (A.Right_Renamed > B.Right_Renamed) Then
            ReDim Res(1)
            Res(0).Left_Renamed = A.Left_Renamed
            Res(0).Right_Renamed = B.Left_Renamed
            Res(1).Left_Renamed = B.Right_Renamed
            Res(1).Right_Renamed = A.Right_Renamed
        ElseIf A.Right_Renamed > B.Right_Renamed Then
            Res(0).Left_Renamed = B.Right_Renamed
            Res(0).Right_Renamed = A.Right_Renamed
        ElseIf (A.Left_Renamed < B.Left_Renamed) Then
            Res(0).Left_Renamed = A.Left_Renamed
            Res(0).Right_Renamed = B.Left_Renamed
        Else
            'UPGRADE_ISSUE: Declaration type not supported: Array with upper bound less than zero. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="934BD4FF-1FF9-47BD-888F-D411E47E78FA"'
            ReDim Res(0 To 0)
        End If

        IntervalsDifference = Microsoft.VisualBasic.Compatibility.VB6.CopyArray(Res)
    End Function

    Public Function PolygonIntersection(ByVal pPoly1 As Polygon, ByVal pPoly2 As Polygon) As Polygon
        Dim pTopo As ITopologicalOperator2
        Dim pTmpPoly0 As Polygon
        Dim pTmpPoly1 As Polygon

        pTopo = pPoly2
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        pTopo = pPoly1
        pTopo.IsKnownSimple_2 = False
        pTopo.Simplify()

        On Error Resume Next
        PolygonIntersection = pTopo.Intersect(pPoly2, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
        If Err.Number = 0 Then Exit Function
        Err.Clear()
        pTmpPoly0 = pTopo.Union(pPoly2)
        '    DrawPolygon pTmpPoly0, 0
        pTmpPoly1 = pTopo.SymmetricDifference(pPoly2)
        '    DrawPolygon pTmpPoly1, RGB(255, 0, 255)
        pTopo = pTmpPoly0
        PolygonIntersection = pTopo.Difference(pTmpPoly1)
        If Err.Number = 0 Then Exit Function
        PolygonIntersection = pPoly2
    End Function

    Function RemoveFars(ByVal pPolygon As Polygon, ByVal pPoint As Point) As Polygon
        Dim Geocollect As IGeometryCollection
        Dim lCollect As IGeometryCollection
        Dim pProxi As IProximityOperator
        Dim OutDist As Double
        Dim tmpDist As Double
        Dim pClone As IClone
        Dim i As Long
        Dim N As Long

        pClone = pPolygon
        RemoveFars = pClone.Clone
        Geocollect = RemoveFars
        N = Geocollect.GeometryCount
        lCollect = New Polygon

        If N > 1 Then
            pProxi = pPoint
            OutDist = 20000000000.0#

            For i = 0 To N - 1
                lCollect.AddGeometry(Geocollect.Geometry(i))

                tmpDist = pProxi.ReturnDistance(lCollect)
                If OutDist > tmpDist Then
                    OutDist = tmpDist
                End If
                lCollect.RemoveGeometries(0, 1)
            Next i

            i = 0
            While i < N
                lCollect.AddGeometry(Geocollect.Geometry(i))
                tmpDist = pProxi.ReturnDistance(lCollect)
                If OutDist < tmpDist Then
                    Geocollect.RemoveGeometries(i, 1)
                    N = N - 1
                Else
                    i = i + 1
                End If
                lCollect.RemoveGeometries(0, 1)
            End While
        End If

    End Function

    Function RemoveSmalls(ByVal pPolygon As Polygon) As Polygon
        Dim Geocollect As IGeometryCollection
        Dim OutArea As Double
        Dim pClone As IClone
        Dim pArea As IArea
        Dim i As Long
        Dim N As Long

        pClone = pPolygon
        RemoveSmalls = pClone.Clone
        Geocollect = RemoveSmalls
        N = Geocollect.GeometryCount

        If N > 1 Then
            OutArea = 0.0#

            For i = 0 To N - 1
                pArea = Geocollect.Geometry(i)
                If pArea.Area > OutArea Then
                    OutArea = pArea.Area
                End If
            Next i

            i = 0
            While i < N
                pArea = Geocollect.Geometry(i)
                If pArea.Area < OutArea Then
                    Geocollect.RemoveGeometries(i, 1)
                    N = N - 1
                Else
                    i = i + 1
                End If
            End While
        End If

    End Function

    Function RemoveHoles(ByVal pPolygon As Polygon) As Polygon
        Dim pTopoOper As ITopologicalOperator2
        Dim NewPolygon As IGeometryCollection
        Dim pInteriorRing As IRing
        Dim pClone As IClone
        Dim i As Long

        pClone = pPolygon
        'Set NewPolygon = pClone.Clone

        RemoveHoles = pClone.Clone
        NewPolygon = RemoveHoles

        i = 0
        While i < NewPolygon.GeometryCount
            pInteriorRing = NewPolygon.Geometry(i)
            If Not pInteriorRing.IsExterior Then
                NewPolygon.RemoveGeometries(i, 1)
            Else
                i = i + 1
            End If
        End While

        pTopoOper = NewPolygon
        pTopoOper.IsKnownSimple_2 = False
        pTopoOper.Simplify()
        'Set RemoveHoles = NewPolygon

    End Function

 
    Public Function GetLine(ByVal ptFrom As IPoint, ByVal ptTo As IPoint) As ILine
        Dim pLine As ILine
        pLine = New ESRI.ArcGIS.Geometry.Line
        pLine.PutCoords(ptFrom, ptTo)

        GetLine = pLine
    End Function

    Public Function CreateBasePoints(ByVal pPolygone As IPointCollection, ByVal K1K1 As IPolyline, ByVal lDepDir As Double, ByVal lTurnDir As Long) As IPointCollection
        Dim tmpPoly As IPointCollection
        Dim bFlg As Boolean
        Dim i As Long
        Dim N As Long
        'Dim M As Long
        Dim side As Long

        bFlg = False
        N = pPolygone.PointCount
        tmpPoly = New Polyline
        CreateBasePoints = New Polygon

        If lTurnDir > 0 Then
            For i = 0 To N - 1
                side = SideDef(K1K1.FromPoint, lDepDir + 90.0#, pPolygone.Point(i))
                If (side < 0) Then
                    If bFlg Then
                        CreateBasePoints.AddPoint(pPolygone.Point(i))
                    Else
                        tmpPoly.AddPoint(pPolygone.Point(i))
                    End If
                ElseIf Not bFlg Then
                    bFlg = True
                    CreateBasePoints.AddPoint(K1K1.FromPoint)
                    CreateBasePoints.AddPoint(K1K1.ToPoint)
                End If
            Next
        Else
            For i = N - 1 To 0 Step -1
                side = SideDef(K1K1.FromPoint, lDepDir + 90.0#, pPolygone.Point(i))
                If (side < 0) Then
                    If bFlg Then
                        CreateBasePoints.AddPoint(pPolygone.Point(i))
                    Else
                        tmpPoly.AddPoint(pPolygone.Point(i))
                    End If
                ElseIf Not bFlg Then
                    bFlg = True
                    CreateBasePoints.AddPoint(K1K1.ToPoint)
                    CreateBasePoints.AddPoint(K1K1.FromPoint)
                End If
            Next
        End If

        'DrawPolygon CreateBasePoints, 0
        CreateBasePoints.AddPointCollection(tmpPoly)
        'DrawPolygon CreateBasePoints, 0

    End Function

    Public Function TurnToFixPrj(ByVal PtSt As IPoint, ByVal TurnR As Double, ByVal turnDir As Long, ByVal FixPnt As IPoint) As IPointCollection

        Dim ptCnt As IPoint
        'Dim ptTmp As IPoint
        Dim Pt1 As IPoint
        'Dim pt2 As IPoint
        Dim DeltaAngle As Double
        Dim DirFx2Cnt As Double
        Dim DistFx2Cnt As Double
        Dim dirCur As Double

        dirCur = PtSt.M

        TurnToFixPrj = New Multipoint
        ptCnt = PointAlongPlane(PtSt, dirCur + 90.0# * turnDir, TurnR)


        DistFx2Cnt = ReturnDistanceAsMeter(ptCnt, FixPnt)

        If DistFx2Cnt < TurnR Then
            TurnR = DistFx2Cnt
            Exit Function
        End If

        DirFx2Cnt = ReturnAngleAsDegree(ptCnt, FixPnt)

        'OutDir = DirFx2Cnt

        DeltaAngle = -RadToDeg(ArcCos(TurnR / DistFx2Cnt)) * turnDir

        Pt1 = PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR)

        Pt1.M = ReturnAngleAsDegree(Pt1, FixPnt)

        TurnToFixPrj.AddPoint(PtSt)
        TurnToFixPrj.AddPoint(Pt1)

    End Function

    Function CalcTouchByFixDir(ByVal PtSt As IPoint, ByVal ptFIX As IPoint, ByVal TurnR As Double, ByVal dirCur As Double, _
    ByVal DirFix As Double, ByVal turnDir As Long, ByVal TurnDir2 As Long, ByVal SnapAngle As Double, ByVal dDir As Double, ByVal FlyBy As IPoint) As IPointCollection

        Dim Constructor As IConstructPoint

        Dim PtCnt1 As IPoint
        'Dim PtCnt2 As IPoint
        'Dim ptTmp As IPoint
        Dim Pt1 As IPoint
        Dim Pt10 As IPoint
        Dim Pt11 As IPoint

        Dim pt2 As IPoint
        Dim pt3 As IPoint

        Dim SideD As Long
        Dim SideT As Long
        'Dim Side As Long

        Dim DeltaAngle As Double
        Dim DeltaDist As Double
        Dim distToTmp As Double
        Dim dirToTmp As Double
        'Dim dirFromTmp As Double

        'Dim dirFxCnt As Double
        'Dim TouchDir As Double
        Dim OutDir As Double
        Dim OutDir0 As Double
        Dim OutDir1 As Double
        Dim Dist As Double

        If SubtractAngles(dirCur, DirFix) < 0.5 Then
            DirFix = dirCur
        End If

        CalcTouchByFixDir = New Multipoint
        PtCnt1 = PointAlongPlane(PtSt, dirCur + 90.0# * turnDir, TurnR)
        PtSt.M = dirCur

        OutDir0 = Modulus(DirFix - SnapAngle * turnDir, 360.0#)
        OutDir1 = Modulus(DirFix + SnapAngle * turnDir, 360.0#)

        Pt10 = PointAlongPlane(PtCnt1, OutDir0 - 90.0# * turnDir, TurnR)
        Pt11 = PointAlongPlane(PtCnt1, OutDir1 - 90.0# * turnDir, TurnR)

        SideT = SideDef(Pt10, DirFix, ptFIX)
        SideD = SideDef(Pt10, DirFix, PtCnt1)

        If SideT * SideD < 0 Then
            Pt1 = Pt10
            OutDir = OutDir0
        Else
            Pt1 = Pt11
            OutDir = OutDir1
        End If

        Pt1.M = OutDir

        FlyBy = New Point
        Constructor = FlyBy

        Constructor.ConstructAngleIntersection(Pt1, DegToRad(OutDir), ptFIX, DegToRad(DirFix))

        Dist = ReturnDistanceAsMeter(Pt1, FlyBy)

        dirToTmp = ReturnAngleAsDegree(ptFIX, FlyBy)
        distToTmp = ReturnDistanceAsMeter(ptFIX, FlyBy)

        TurnDir2 = -AnglesSideDef(OutDir, DirFix)

        If TurnDir2 < 0 Then
            DeltaAngle = Modulus(180.0# + DirFix - OutDir, 360.0#)
        ElseIf TurnDir2 > 0 Then
            DeltaAngle = Modulus(OutDir - 180.0# - DirFix, 360.0#)
        End If

        DeltaAngle = 0.5 * DeltaAngle
        DeltaDist = TurnR / Math.Tan(DegToRad(DeltaAngle))

        dDir = Dist - DeltaDist

        If DeltaDist <= Dist Then
            pt2 = PointAlongPlane(FlyBy, OutDir - 180.0#, DeltaDist)
            pt3 = PointAlongPlane(FlyBy, DirFix, DeltaDist)
        Else
            pt2 = PointAlongPlane(FlyBy, OutDir, DeltaDist)
            pt3 = PointAlongPlane(FlyBy, DirFix - 180.0#, DeltaDist)
        End If

        pt2.M = OutDir
        pt3.M = DirFix

        CalcTouchByFixDir.AddPoint(PtSt)
        CalcTouchByFixDir.AddPoint(Pt1)
        CalcTouchByFixDir.AddPoint(pt2)
        CalcTouchByFixDir.AddPoint(pt3)

    End Function

    Public Function ClipByLine(ByRef pClipPolygon As ESRI.ArcGIS.Geometry.Polygon, ByRef pCutLine As ESRI.ArcGIS.Geometry.Polyline, ByRef pLeft As ESRI.ArcGIS.Geometry.Polygon, ByRef pRight As ESRI.ArcGIS.Geometry.Polygon, ByRef pUnspecified As ESRI.ArcGIS.Geometry.Polygon) As Short
        Dim iRings As Integer
        Dim Side1 As Integer
        Dim side As Integer
        Dim Cnt As Integer
        Dim UBd As Integer
        Dim i As Integer
        Dim j As Integer
        Dim K As Integer
        Dim L As Integer
        'Dim M As Integer
        Dim N As Integer

        Dim Down As Double
        Dim UpA As Double
        Dim UpB As Double
        Dim mA As Double
        Dim mB As Double

        Dim dXl As Double
        Dim dYl As Double
        Dim dXs As Double
        Dim dYs As Double
        Dim dX As Double
        Dim dY As Double

        Dim pIntersectionPoints() As TIntersections
        Dim pPointsHeap() As TPointsHeap

        Dim pRingVertices As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
        Dim pTestPoly As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pIUnspec As ESRI.ArcGIS.Geometry.IGeometryCollection
        'Dim pGeomCol As IGeometryCollection
        Dim pProxy As ESRI.ArcGIS.Geometry.IProximityOperator
        Dim pExteriorRing() As ESRI.ArcGIS.Geometry.IRing
        Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon2
        Dim CutLine As ESRI.ArcGIS.Geometry.IPolyline
        Dim pClone As ESRI.ArcGIS.esriSystem.IClone
        Dim pRing As ESRI.ArcGIS.Geometry.IRing

        Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
        Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
        Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
        Dim Pt1 As ESRI.ArcGIS.Geometry.IPoint
        Dim PtX As ESRI.ArcGIS.Geometry.IPoint

        pClone = pClipPolygon
        pPolygon = pClone.Clone

        pTopoOper = pPolygon
        pTopoOper.IsKnownSimple_2 = False
        'UPGRADE_WARNING: Couldn't resolve default property of object pTopoOper.Simplify. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        pTopoOper.Simplify()

        pLeft = New ESRI.ArcGIS.Geometry.Polygon
        pRight = New ESRI.ArcGIS.Geometry.Polygon
        pUnspecified = New ESRI.ArcGIS.Geometry.Polygon
        pIUnspec = pUnspecified

        pTestPoly = New ESRI.ArcGIS.Geometry.Polygon
        CutLine = pCutLine
        ClipByLine = 0

        '    Set pGeomCol = pPolygon
        '    iRings = pGeomCol.GeometryCount
        'UPGRADE_WARNING: Couldn't resolve default property of object pPolygon.ExteriorRingCount. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        iRings = pPolygon.ExteriorRingCount
        If iRings > 0 Then
            ReDim pExteriorRing(iRings - 1)
        Else
            Exit Function
        End If

        pPolygon.QueryExteriorRingsEx(iRings, pExteriorRing(0))

        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.FromPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ptFrom = CutLine.FromPoint
        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.ToPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        ptTo = CutLine.ToPoint

        dXl = ptTo.X - ptFrom.X
        dYl = ptTo.Y - ptFrom.Y


        For i = 0 To iRings - 1
            '' Getting each ring with its index
            pRing = pExteriorRing(i)
            '        Set pRing = pGeomCol.Geometry(I)

            If pRing.IsExterior Then
                pProxy = ptFrom
            Else
                pProxy = ptTo
            End If

            pRingVertices = pRing

            N = pRingVertices.PointCount - 1
            If N >= 0 Then
                ReDim pIntersectionPoints(N - 1)

                L = -1
                Do
                    L = L + 1
                    If L >= N Then Exit Do
                    'UPGRADE_WARNING: Couldn't resolve default property of object pCutLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    side = WhichSide(pCutLine, pRingVertices.Point(L))
                Loop While side = 0

                Cnt = 0
                ' GetIntersectPointOfTwoLines
                For j = 0 To N - 1
                    K = (L + 1) Mod N
                    Pt1 = pRingVertices.Point(K)

                    'UPGRADE_WARNING: Couldn't resolve default property of object pCutLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    Side1 = WhichSide(pCutLine, Pt1)
                    If Side1 * side < 0 Then
                        pt0 = pRingVertices.Point(L)

                        side = Side1
                        dXs = Pt1.X - pt0.X
                        dYs = Pt1.Y - pt0.Y
                        Down = dYl * dXs - dXl * dYs

                        If System.Math.Abs(Down) > Epsilon Then
                            dX = pt0.X - ptFrom.X
                            dY = pt0.Y - ptFrom.Y

                            UpA = dXl * dY - dYl * dX
                            UpB = dXs * dY - dYs * dX

                            mA = UpA / Down
                            mB = UpB / Down

                            If (mA >= 0.0# And mA < 1.0#) And (mB >= 0.0# And mB < 1.0#) Then

                                PtX = New ESRI.ArcGIS.Geometry.Point
                                PtX.PutCoords(pt0.X + mA * dXs, pt0.Y + mA * dYs)

                                pIntersectionPoints(Cnt).pPoint = PtX
                                'UPGRADE_WARNING: Couldn't resolve default property of object PtX. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                pIntersectionPoints(Cnt).Dist = pProxy.ReturnDistance(PtX)
                                pIntersectionPoints(Cnt).PredIndex = L
                                Cnt = Cnt + 1
                            End If
                        End If
                    End If
                    L = K
                Next j

                If Cnt > 0 Then
                    pTestPoly.RemoveGeometries(0, pTestPoly.GeometryCount)
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pTestPoly.AddGeometry(pRing)

                    pProxy = pTestPoly

                    'UPGRADE_WARNING: Couldn't resolve default property of object ptTo. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If pProxy.ReturnDistance(ptTo) <> 0.0# Then
                        UBd = Cnt - 1
                    Else
                        UBd = Cnt - 2
                    End If

                    'UPGRADE_WARNING: Couldn't resolve default property of object ptFrom. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If pProxy.ReturnDistance(ptFrom) = 0.0# Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object pIntersectionPoints(0). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        pIntersectionPoints(0) = pIntersectionPoints(UBd)
                        UBd = UBd - 1
                    End If

                    If UBd > 0 Then
                        ReDim Preserve pIntersectionPoints(UBd)
                        ReDim pPointsHeap(N + UBd)

                        ArrangeVertices(pIntersectionPoints, pRingVertices, pPointsHeap)
                        FormPolygons(pIntersectionPoints, pPointsHeap, pCutLine, pLeft, pRight, pUnspecified)

                        ClipByLine = 3
                    End If
                    pProxy = ptFrom
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pIUnspec.AddGeometry(pRing)
                End If
            End If
        Next i

        If ClipByLine <> 0 Then
            pTopoOper = pLeft
            pTopoOper.IsKnownSimple_2 = False
            'UPGRADE_WARNING: Couldn't resolve default property of object pTopoOper.Simplify. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pTopoOper.Simplify()

            pTopoOper = pUnspecified
            pTopoOper.IsKnownSimple_2 = False
            'UPGRADE_WARNING: Couldn't resolve default property of object pTopoOper.Simplify. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pTopoOper.Simplify()

            pTopoOper = pRight
            pTopoOper.IsKnownSimple_2 = False
            'UPGRADE_WARNING: Couldn't resolve default property of object pTopoOper.Simplify. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            pTopoOper.Simplify()
        End If

    End Function

    Private Function WhichSide(ByRef CutLine As ESRI.ArcGIS.Geometry.IPolyline, ByRef ptCheck As ESRI.ArcGIS.Geometry.IPoint) As Integer
        Dim dX0 As Double
        Dim dY0 As Double
        Dim dX1 As Double
        Dim dY1 As Double
        Dim X As Double

        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.FromPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dX0 = ptCheck.X - CutLine.FromPoint.X
        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.FromPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dY0 = ptCheck.Y - CutLine.FromPoint.Y

        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.ToPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dX1 = ptCheck.X - CutLine.ToPoint.X
        'UPGRADE_WARNING: Couldn't resolve default property of object CutLine.ToPoint. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dY1 = ptCheck.Y - CutLine.ToPoint.Y

        X = dX0 * dY1 - dY0 * dX1
        '    If Abs(x) > Epsilon Then
        WhichSide = System.Math.Sign(X)
        '    Else
        '        WhichSide = 0
        '    End If

    End Function

    Private Sub ArrangeVertices(ByRef pInterSecPoints() As TIntersections, ByRef pVertexCol As ESRI.ArcGIS.Geometry.IPointCollection, ByRef pPointsHeap() As TPointsHeap)
        Dim CurItem As Integer
        Dim i As Integer
        Dim j As Integer
        Dim K As Integer
        Dim L As Integer
        Dim N As Integer
        Dim M As Integer

        N = UBound(pInterSecPoints) + 1
        M = pVertexCol.PointCount - 1

        SortByDist(pInterSecPoints, N - 1)

        For i = 0 To N - 1
            pInterSecPoints(i).IntersectIndex = i
        Next i

        SortByIndex(pInterSecPoints, N - 1)
        CurItem = 0

        For i = 0 To N - 1
            j = (i + 1) Mod N

            pPointsHeap(CurItem).pPoint = pInterSecPoints(i).pPoint
            pPointsHeap(CurItem).IsIntersection = True
            pPointsHeap(CurItem).IntersectIndex = pInterSecPoints(i).IntersectIndex
            pInterSecPoints(i).PointIndex = CurItem
            CurItem = CurItem + 1

            K = (pInterSecPoints(i).PredIndex + 1) Mod M
            L = (pInterSecPoints(j).PredIndex + 1) Mod M
            While K <> L
                pPointsHeap(CurItem).pPoint = pVertexCol.Point(K)
                pPointsHeap(CurItem).IsIntersection = False
                CurItem = CurItem + 1
                K = (K + 1) Mod M
            End While
        Next i

        SortByDist(pInterSecPoints, N - 1)
    End Sub

    Private Sub FormPolygons(ByRef pIntersectionPoints() As TIntersections, ByRef pPointsHeap() As TPointsHeap, ByRef pCutLine As ESRI.ArcGIS.Geometry.Polyline, ByRef pLeft As ESRI.ArcGIS.Geometry.Polygon, ByRef pRight As ESRI.ArcGIS.Geometry.Polygon, ByRef pUnspecified As ESRI.ArcGIS.Geometry.Polygon)

        Dim StartIndex As Integer
        Dim EndIndex As Integer
        Dim i As Integer
        Dim j As Integer
        Dim K As Integer
        Dim N As Integer
        Dim M As Integer

        Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
        Dim pRing As ESRI.ArcGIS.Geometry.IRing

        Dim pIUnspec As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pIRight As ESRI.ArcGIS.Geometry.IGeometryCollection
        Dim pILeft As ESRI.ArcGIS.Geometry.IGeometryCollection

        Dim bUnspec As Boolean
        Dim Trigger As Boolean

        pILeft = pLeft
        pIRight = pRight
        pIUnspec = pUnspecified

        N = UBound(pPointsHeap)
        M = UBound(pIntersectionPoints)

        For j = 0 To M - 1 Step 2
            K = j + 1

            StartIndex = pIntersectionPoints(j).PointIndex
            EndIndex = pIntersectionPoints(K).PointIndex

            i = StartIndex
            K = (i + 1) Mod (N + 1)

            bUnspec = False
            Trigger = False

            pRing = New ESRI.ArcGIS.Geometry.Ring
            pPoints = pRing

            If (pPointsHeap(i).UsedFlag And 1) = 0 Then
                While i <> EndIndex

                    If pPointsHeap(i).IsIntersection Then
                        pPoints.AddPoint(pPointsHeap(i).pPoint)

                        If Trigger Then
                            K = i - 1
                            If K < 0 Then
                                K = K + N + 1
                            Else
                                K = K Mod (N + 1)
                            End If

                            'UPGRADE_WARNING: Couldn't resolve default property of object pCutLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If WhichSide(pCutLine, pPointsHeap(K).pPoint) > 0 Then
                                pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 1
                                K = pPointsHeap(i).IntersectIndex - 1
                                If K < 0 Then K = K + M + 1
                            Else
                                pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 2
                                K = pPointsHeap(i).IntersectIndex + 1
                                bUnspec = True
                            End If

                            K = K Mod (M + 1)
                            i = (pIntersectionPoints(K).PointIndex)
                        Else
                            pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 1
                            i = (i + 1) Mod (N + 1)
                        End If

                        Trigger = Not Trigger
                    Else
                        pPoints.AddPoint(pPointsHeap(i).pPoint)
                        i = (i + 1) Mod (N + 1)
                    End If
                End While

                pPoints.AddPoint(pPointsHeap(i).pPoint)
                pRing.Close()

                If bUnspec Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pIUnspec.AddGeometry(pRing)
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pILeft.AddGeometry(pRing)
                End If
            End If

            pRing = New ESRI.ArcGIS.Geometry.Ring
            pPoints = pRing

            i = StartIndex
            bUnspec = False
            Trigger = False

            K = i - 1
            If K < 0 Then
                K = K + N + 1
            Else
                K = K Mod (N + 1)
            End If

            If (pPointsHeap(i).UsedFlag And 2) = 0 Then
                While i <> EndIndex
                    If pPointsHeap(i).IsIntersection Then
                        pPoints.AddPoint(pPointsHeap(i).pPoint)

                        If Trigger Then
                            K = (i + 1) Mod (N + 1)

                            'UPGRADE_WARNING: Couldn't resolve default property of object pCutLine. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            If WhichSide(pCutLine, pPointsHeap(K).pPoint) < 0 Then
                                pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 2
                                K = pPointsHeap(i).IntersectIndex - 1
                                If K < 0 Then K = K + M + 1
                            Else
                                K = pPointsHeap(i).IntersectIndex + 1
                                pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 1
                                bUnspec = True
                            End If

                            K = K Mod (M + 1)
                            i = (pIntersectionPoints(K).PointIndex)
                        Else
                            pPointsHeap(i).UsedFlag = pPointsHeap(i).UsedFlag Or 2
                            i = i - 1
                            If i < 0 Then i = i + N + 1
                            i = i Mod (N + 1)
                        End If

                        Trigger = Not Trigger
                    Else
                        pPoints.AddPoint(pPointsHeap(i).pPoint)

                        i = i - 1
                        If i < 0 Then
                            i = i + N + 1
                        Else
                            i = i Mod (N + 1)
                        End If
                    End If
                End While

                pPoints.AddPoint(pPointsHeap(i).pPoint)
                pRing.Close()
                'UPGRADE_WARNING: Couldn't resolve default property of object pRing.ReverseOrientation. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                pRing.ReverseOrientation()

                If bUnspec Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pIUnspec.AddGeometry(pRing)
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object pRing. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    pIRight.AddGeometry(pRing)
                End If
            End If
        Next j

    End Sub

    Private Sub SortByDist(ByRef A() As TIntersections, ByRef N As Integer)
        'Dim tempInterPt As TIntersections
        'Dim I As Long
        'Dim J As Long

        'For I = 0 To N - 1
        '    For J = I + 1 To N
        '        If (pInterSecPoints(I).Dist > pInterSecPoints(J).Dist) Then
        '            tempInterPt = pInterSecPoints(J)
        '            pInterSecPoints(J) = pInterSecPoints(I)
        '            pInterSecPoints(I) = tempInterPt
        '        End If
        '    Next J
        'Next I

        Dim Lo As Short
        Dim Hi As Short

        Lo = LBound(A)
        Hi = N

        If (Lo = Hi) Then Exit Sub
        QSortByDist(A, Lo, Hi)



    End Sub

    Private Sub SortByIndex(ByRef A() As TIntersections, ByRef N As Integer)
        'Dim tempInterPt As TIntersections
        'Dim I As Long
        'Dim J As Long

        'For I = 0 To N - 1
        '    For J = I + 1 To N
        '        If (pInterSecPoints(I).PredIndex > pInterSecPoints(J).PredIndex) Then
        '            tempInterPt = pInterSecPoints(J)
        '            pInterSecPoints(J) = pInterSecPoints(I)
        '            pInterSecPoints(I) = tempInterPt
        '        End If
        '    Next J
        'Next I
        Dim Lo As Short
        Dim Hi As Short

        Lo = LBound(A)
        Hi = N

        If (Lo = Hi) Then Exit Sub
        QSortByIndex(A, Lo, Hi)

    End Sub

    Private Sub QSortByDist(ByRef A() As TIntersections, ByRef iLo As Short, ByRef iHi As Short)
        Dim t As TIntersections
        'UPGRADE_NOTE: Mid was upgraded to Mid_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim Mid_Renamed As Double
        Dim Lo As Short
        Dim Hi As Short

        Lo = iLo
        Hi = iHi
        Mid_Renamed = A((Lo + Hi) / 2).Dist
        Do
            While A(Lo).Dist < Mid_Renamed
                Lo = Lo + 1
            End While
            While A(Hi).Dist > Mid_Renamed
                Hi = Hi - 1
            End While
            If (Lo <= Hi) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object t. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                t = A(Lo)
                'UPGRADE_WARNING: Couldn't resolve default property of object A(Lo). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                A(Lo) = A(Hi)
                'UPGRADE_WARNING: Couldn't resolve default property of object A(Hi). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                A(Hi) = t
                Lo = Lo + 1
                Hi = Hi - 1
            End If
        Loop Until Lo > Hi

        If (Hi > iLo) Then QSortByDist(A, iLo, Hi)
        If (Lo < iHi) Then QSortByDist(A, Lo, iHi)
    End Sub

    Private Sub QSortByIndex(ByRef A() As TIntersections, ByRef iLo As Short, ByRef iHi As Short)
        Dim t As TIntersections
        'UPGRADE_NOTE: Mid was upgraded to Mid_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim Mid_Renamed As Short
        Dim Lo As Short
        Dim Hi As Short

        Lo = iLo
        Hi = iHi
        Mid_Renamed = A((Lo + Hi) / 2).PredIndex
        Do
            While A(Lo).PredIndex < Mid_Renamed
                Lo = Lo + 1
            End While
            While A(Hi).PredIndex > Mid_Renamed
                Hi = Hi - 1
            End While
            If (Lo <= Hi) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object t. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                t = A(Lo)
                'UPGRADE_WARNING: Couldn't resolve default property of object A(Lo). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                A(Lo) = A(Hi)
                'UPGRADE_WARNING: Couldn't resolve default property of object A(Hi). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                A(Hi) = t
                Lo = Lo + 1
                Hi = Hi - 1
            End If
        Loop Until Lo > Hi

        If (Hi > iLo) Then QSortByIndex(A, iLo, Hi)
        If (Lo < iHi) Then QSortByIndex(A, Lo, iHi)
    End Sub

    Public Function Calc_CF_Trajectory(ByVal CurrentActiveView As ESRI.ArcGIS.Carto.IActiveView, ByVal CurPnt As IPoint, ByVal FixPnt As IPoint, ByVal CourseToFix As Double, ByVal Side As Long, ByVal radius As Double) As IPointCollection

        Dim PtInter As Point
        Dim Pt1 As Point, Pt2 As Point
        Dim Theta As Double
        Dim d1 As Double, d2 As Double
        Dim l As Long

        Dim NomTrack As IPointCollection
        NomTrack = New Multipoint


        PtInter = LineLineIntersect(CurPnt, CurPnt.M, FixPnt, CourseToFix - 180)
        'DrawPointWithText(CurrentActiveView, PtInter, "PtInter", 8, 125, True)

        d1 = ReturnDistanceAsMeter(PtInter, CurPnt)
        d2 = ReturnDistanceAsMeter(PtInter, FixPnt)


        'добавлено 11 апреля 2014 (проверка, когда средство не вписывается в указанный курс)

        Theta = ReturnAngleAsDegree(FixPnt, PtInter)

        If (Math.Abs(Modulus(CurPnt.M, 360) - Modulus(CourseToFix, 360)) <= 5) Or (SubtractAngles(Theta, CourseToFix) > 90) Then
            CurPnt.M = CourseToFix
            NomTrack.AddPoint(CurPnt)

            FixPnt.M = CourseToFix
            NomTrack.AddPoint(CurPnt)

            Calc_CF_Trajectory = NomTrack

            Return Calc_CF_Trajectory

        End If


        If d2 > d1 Then d1 = d2
        'DrawPointWithText(CurrentActiveView, FixPnt, "FixPnt", 8, 125, True)
        'DrawPointWithText(CurrentActiveView, CurPnt, "CurPnt", 8, 125, True)


        l = SideDef(CurPnt, CurPnt.M + 90, PtInter)

        If (l = -1) Then

            Pt1 = PointAlongPlane(PtInter, CurPnt.M, d1)
            'DrawPointWithText(CurrentActiveView, Pt1, "Pt1", 8, 125, True)

            NomTrack.AddPoint(CurPnt)
            Pt1.M = CurPnt.M
            NomTrack.AddPoint(Pt1)

            Pt2 = PointAlongPlane(PtInter, CourseToFix + 180, d1)
            Pt2.M = CourseToFix
            'DrawPointWithText(CurrentActiveView, Pt2, "Pt2", 8, 125, True)

            NomTrack.AddPoint(Pt2)

            FixPnt.M = CourseToFix
            NomTrack.AddPoint(FixPnt)

        End If

        'If Math.Abs(Modulus(CurPnt.M, 360) - Modulus(CourseToFix, 360)) < 180 - 0.5 Then
        If (l = 1) Then

            Pt1 = PointAlongPlane(PtInter, CurPnt.M + 180, d1)
            'DrawPointWithText(CurrentActiveView, Pt1, "Pt1", 8, 125, True)

            NomTrack.AddPoint(CurPnt)
            Pt1.M = CurPnt.M
            NomTrack.AddPoint(Pt1)

            Pt2 = PointAlongPlane(PtInter, CourseToFix, d1)
            Pt2.M = CourseToFix
            NomTrack.AddPoint(Pt2)
            'DrawPointWithText(CurrentActiveView, Pt2, "Pt2", 8, 125, True)


            FixPnt.M = CourseToFix
            NomTrack.AddPoint(FixPnt)

        End If

        If Math.Abs(Modulus(CurPnt.M, 360) - Modulus(CourseToFix, 360) - 180) <= 0.5 Then
            l = SideDef(CurPnt, CurPnt.M + Side * 90, FixPnt)

            If l <= 0 Then
                Pt1 = PointAlongPlane(CurPnt, CurPnt.M + Side * 90, 2 * radius)
                Pt1.M = CourseToFix
                NomTrack.AddPoint(Pt1)
                FixPnt.M = CourseToFix
                NomTrack.AddPoint(FixPnt)

            Else
                Pt1 = PointAlongPlane(FixPnt, CourseToFix + Side * 90, 2 * radius)
                Pt1.M = CurPnt.M
                NomTrack.AddPoint(Pt1)
                FixPnt.M = CourseToFix
                NomTrack.AddPoint(FixPnt)
            End If
        End If





        Calc_CF_Trajectory = NomTrack
    End Function

    Public Function Calc_FB_Trajectory(ByVal V As Double, ByVal Theta As Double, ByVal B1 As Double, ByVal c As Double, _
                                       ByVal CurPnt As IPoint, ByVal Side As Long) As IPointCollection
        ' EPT - Earliest turning point, LPT - Latest turning point, ASW - Area semi-width
        ' EPT & LPT abs distance from Current point in meter
        c = 0

        Dim NomTrack As IPointCollection
        NomTrack = New Multipoint

        Dim L1 As Double, L2 As Double
        Dim R1 As Double
        Dim CalcMinDistFB As Double



        Theta = DegToRadValue * Theta
        R1 = Bank2Radius(B1, V)

        L1 = R1 * Math.Tan(Theta / 2.0#)
        L2 = c * V / 3.6

        CalcMinDistFB = L1 + L2

        Theta = RadToDegValue * Theta
        'Alpha = RadToDegValue * Alpha


        Dim PtTmp As IPoint
        Dim PtTmp1 As IPoint
        'Dim fTmp As Double
        Dim MTmp As Double

        MTmp = CurPnt.M

        PtTmp = PointAlongPlane(CurPnt, MTmp - 180.0#, L1)
        PtTmp.M = MTmp
        NomTrack.AddPoint(PtTmp)        'Point1

        PtTmp1 = PointAlongPlane(CurPnt, MTmp - Theta * Side, L1)
        PtTmp1.M = MTmp - Theta * Side
        NomTrack.AddPoint(PtTmp1)        'Point2
        PtTmp = PointAlongPlane(PtTmp1, PtTmp1.M, L2)
        PtTmp.M = PtTmp1.M
        NomTrack.AddPoint(PtTmp)        'Point3



        ''Dim NomPoly As IPolyline
        'NomPoly = CalcTrajectoryFromMultiPoint(NomTrack)



        Calc_FB_Trajectory = NomTrack
    End Function

    Public Function Calc_FO_Trajectory(ByVal V As Double, ByVal Alpha As Double, ByVal Theta As Double, ByVal B1 As Double, ByVal B2 As Double, ByVal c As Double, _
                                    ByVal CurPnt As IPoint, ByVal Side As Long, Optional ByVal FixPnt As IPoint = Nothing) As IPointCollection
        ' EPT - Earliest turning point, LPT - Latest turning point, ASW - Area semi-width
        ' EPT & LPT abs distance from Current point in meter

        Dim L1 As Double, L2 As Double, L3 As Double, L4 As Double, L5 As Double
        Dim R1 As Double, R2 As Double
        Dim CalcMinDistFO As Double
        Dim NomTrack As IPointCollection
        NomTrack = New Multipoint




        Theta = DegToRadValue * Theta
        Alpha = DegToRadValue * Alpha
        R1 = Bank2Radius(B1, V)
        R2 = Bank2Radius(B2, V)

        L1 = R1 * Math.Sin(Theta)
        L2 = R1 * Math.Cos(Theta) * Math.Tan(Alpha)
        'L3 = R1 * (1 / Sin(Alpha) - 2 * Cos(Theta) / Cos(Alpha)) 'Bag PANS OPS
        L3 = R1 * (1.0# - Math.Cos(Theta) / Math.Cos(Alpha)) / Math.Sin(Alpha)
        L4 = R2 * Math.Tan(Alpha / 2)

        If L3 * Math.Cos(Alpha) < L4 Then
            Alpha = DegToRad(Math.Floor((RadToDeg(ArcCos((R2 + R1 * Math.Cos(Theta)) / (R1 + R2))))))
            L2 = R1 * Math.Cos(Theta) * Math.Tan(Alpha)
            L3 = R1 * (1.0 - Math.Cos(Theta) / Math.Cos(Alpha)) / Math.Sin(Alpha)
            L4 = R2 * Math.Tan(0.5 * Alpha)
        End If






        L5 = c * V / 3.6
        CalcMinDistFO = L1 + L2 + L3 + L4 + L5
        'Calc Nominal Track
        'Side = 1  Right
        'Side = -1 Left

        Dim PtTmp As IPoint
        'Dim PtTmp1 As IPoint
        Dim fTmp As Double
        Dim MTmp As Double

        ' 04.2014
        Dim DistanceToFix As Double
        If Not FixPnt Is Nothing Then
            DistanceToFix = ReturnDistanceAsMeter(CurPnt, FixPnt)
        Else
            DistanceToFix = CalcMinDistFO
        End If

        If DistanceToFix < CalcMinDistFO Then
            PtTmp = CurPnt
            PtTmp.M = ReturnAngleAsDegree(CurPnt, FixPnt)
            NomTrack.AddPoint(PtTmp)


            PtTmp = FixPnt
            PtTmp.M = ReturnAngleAsDegree(CurPnt, FixPnt)
            NomTrack.AddPoint(PtTmp)

            Return NomTrack
        End If


        ' end 04.2014


        Theta = RadToDegValue * Theta
        Alpha = RadToDegValue * Alpha




        MTmp = CurPnt.M

        NomTrack.AddPoint(CurPnt)        'Point1

        PtTmp = PointAlongPlane(CurPnt, MTmp - Side * 90.0#, R1)
        PtTmp = PointAlongPlane(PtTmp, MTmp - Side * 90.0# + 180 - (Theta + Alpha) * Side, R1)

        PtTmp.M = MTmp - (Theta + Alpha) * Side
        NomTrack.AddPoint(PtTmp)        'Point2

        fTmp = L3 * Math.Cos(DegToRadValue * Alpha) - L4
        PtTmp = PointAlongPlane(PtTmp, MTmp - (Theta + Alpha) * Side, fTmp)
        PtTmp.M = NomTrack.Point(1).M
        NomTrack.AddPoint(PtTmp)        'Point3

        PtTmp = PointAlongPlane(CurPnt, CurPnt.M - Theta * Side, CalcMinDistFO - L5)
        PtTmp.M = CurPnt.M - Theta * Side
        NomTrack.AddPoint(PtTmp)        'Point4

        PtTmp = PointAlongPlane(NomTrack.Point(3), NomTrack.Point(3).M, L5)
        PtTmp.M = NomTrack.Point(3).M
        NomTrack.AddPoint(PtTmp)        'Point5


        'NomPoly = CalcTrajectoryFromMultiPoint(NomTrack)

        'Dim pCircle As IPolygon
        'pCircle = CreatePrjCircle(CurPnt, CalcMinDistFO)


        Calc_FO_Trajectory = NomTrack
    End Function

    Function Get_DMEArc_ptTmp(ByVal CurPt As IPoint, ByVal ptNav As IPoint, ByVal OutDir As Double, ByVal ptDME As IPoint, ByVal RDME As Double, ByVal TurnR As Double, ByVal turnSide As Long, ByVal CurDMEClockWise As Long) As IPointCollection

        Dim ptCnt As IPoint
        Dim ptTmp As IPoint
        Dim fTmp As Double
        Dim fTmp1 As Double
        Dim fDir As Double
        Dim SideNav As Long
        Dim pPointCollection As IPointCollection
        pPointCollection = New Multipoint
        pPointCollection.AddPoint(CurPt)

        Get_DMEArc_ptTmp = Nothing
        SideNav = SideDef(ptNav, OutDir, ptDME)
        fTmp1 = Point2LineDistancePrj(ptDME, ptNav, OutDir)
        fTmp = fTmp1 - TurnR * SideNav * turnSide
        ptTmp = PointAlongPlane(ptNav, OutDir + 90.0# * SideNav, -TurnR * SideNav * turnSide)

        fTmp = CircleVectorIntersect(ptDME, RDME - TurnR * CurDMEClockWise * turnSide, ptTmp, OutDir + (CurDMEClockWise * turnSide + 1) * 90.0#, ptTmp)
        If fTmp = -1 Then
            Get_DMEArc_ptTmp = Nothing
            Exit Function
        End If

        fDir = ReturnAngleAsDegree(ptDME, ptTmp)
        ptTmp = PointAlongPlane(ptDME, fDir, RDME)
        ptTmp.M = Modulus(fDir - 90.0# * CurDMEClockWise)
        pPointCollection.AddPoint(ptTmp)

        fDir = ReturnAngleAsDegree(ptDME, ptTmp)

        ptCnt = PointAlongPlane(ptTmp, ptTmp.M - 90.0# * turnSide, TurnR)
        ptTmp = PointAlongPlane(ptCnt, OutDir + 90.0# * turnSide, TurnR)
        ptTmp.M = OutDir

        pPointCollection.AddPoint(ptTmp)

        fDir = ReturnAngleAsDegree(ptDME, ptTmp)

        Get_DMEArc_ptTmp = pPointCollection

    End Function

    Function Intercept_DMEArc(ByVal ptNav As IPoint, ByVal OutDir As Double, ByVal CurPt As IPoint, ByVal R As Double, ByVal Side As Long, ByVal RDME As Double) As IPointCollection

        'Dim ptCnt As IPoint
        Dim ptTmp As IPoint
        'Dim ptTo As IPoint
        Dim ptDME As IPoint


        Dim fTmp As Double
        Dim fTmp1 As Double
        ' Dim Rtmp As Double
        Dim fDir As Double
        Dim SideNav As Long
        Dim CurDMEClockWise As Long
        Dim turnSide As Long
        'Dim RDME As Double
        Dim TurnR As Double


        Dim pPointCollection As IPointCollection
        pPointCollection = New Multipoint
        pPointCollection.AddPoint(CurPt) '.ptPrj

        CurDMEClockWise = Side
        turnSide = -Side
        'RDME = R
        TurnR = R
        fTmp = ReturnDistanceAsMeter(CurPt, ptNav)

        If (fTmp < R + RDME) Then
            Intercept_DMEArc = Nothing
            Exit Function
        End If

        ptTmp = PointAlongPlane(CurPt, CurPt.M, fTmp - (R + RDME))
        ptTmp.M = CurPt.M
        pPointCollection.AddPoint(ptTmp)

        Intercept_DMEArc = Nothing
        ptDME = PointAlongPlane(CurPt, CurPt.M - 90 * Side, R)

        SideNav = SideDef(ptNav, OutDir, ptDME)
        fTmp1 = Point2LineDistancePrj(ptDME, ptNav, OutDir)
        fTmp = fTmp1 - TurnR * SideNav * turnSide
        ptTmp = PointAlongPlane(ptNav, OutDir + 90.0# * SideNav, -TurnR * SideNav * turnSide)

        fTmp = CircleVectorIntersect(ptDME, RDME - TurnR * CurDMEClockWise * turnSide, _
            ptTmp, OutDir + (CurDMEClockWise * turnSide + 1) * 90.0#, ptTmp)

        If fTmp = -1 Then Exit Function

        fDir = ReturnAngleAsDegree(ptDME, ptTmp)
        ptTmp = PointAlongPlane(ptDME, fDir, RDME)

        'ptTmp.M = Modulus(fDir - 90.0# * CurDMEClockWise)
        ptTmp.M = Modulus(fDir + 90.0# * CurDMEClockWise)
        pPointCollection.AddPoint(ptTmp)
        pPointCollection.AddPoint(ptTmp)

        'In_Dir = ptTmp.M

        'ptCnt = PointAlongPlane(ptTmp, ptTmp.M - 90.0# * turnSide, TurnR)
        'ptTmp = PointAlongPlane(ptCnt, OutDir + 90.0# * turnSide, TurnR)

        'ptTmp.M = OutDir
        'pPointCollection.AddPoint(ptTmp)

        'ptNav.M = OutDir
        'pPointCollection.AddPoint(ptNav)

        Intercept_DMEArc = pPointCollection

    End Function

    Function CalcDMEInterception(ByVal fRDME As Double, ByVal ptDME As IPoint, ByVal ptFrom As IPoint, ByVal TurnR As Double, ByVal turnSide As Long) As IPointCollection


        Dim pPointCollection As IPointCollection
        Dim ptBase As IPoint
        Dim ptTmp As IPoint
        Dim ptTo As IPoint
        Dim DMETurn As Long
        Dim fSqrVal As Double
        Dim fTmp As Double
        Dim Fl As Double
        Dim fS As Double
        Dim X As Long

        pPointCollection = New Multipoint
        pPointCollection.AddPoint(ptFrom)

        DMETurn = SideDef(ptFrom, ptFrom.M, ptDME)

        If DMETurn = 0 Then DMETurn = 1
        ptBase = LineLineIntersect(ptFrom, ptFrom.M, ptDME, ptFrom.M + 90.0#)
        Fl = Point2LineDistancePrj(ptFrom, ptDME, ptFrom.M)
        fTmp = Fl - TurnR * DMETurn * turnSide

        X = 1 ' IIf(OptionButton402(0).value, 1, -1) проверка до или после средства
        fSqrVal = (fRDME + (X * TurnR)) * (fRDME + (X * TurnR)) - fTmp * fTmp

        If Math.Abs(fSqrVal) < 0.0001 Then fSqrVal = 0.0#
        If fSqrVal < 0 Then
            CalcDMEInterception = Nothing '"С данным радиусом невозможно выйти на данную точку"
            Exit Function
        End If

        fS = Math.Sqrt(fSqrVal)
        ptTmp = PointAlongPlane(ptBase, ptFrom.M + ((0.5 * X + 0.5) * 180.0#), fS)

        ptTmp.M = ptFrom.M
        ptTmp.Z = ptFrom.Z
        pPointCollection.AddPoint(ptTmp)

        ptTo = PointAlongPlane(ptTmp, ptTmp.M - 90.0# * turnSide, TurnR)

        ' If OptionButton402(0).value Then
        fTmp = ReturnAngleAsDegree(ptTo, ptDME)
        '  Else
        ' fTmp = ReturnAngleAsDegree(ptDME, ptTo)
        ' End If

        '  If IsTurnGreateThanNormal(ptTmp.M, Modulus(fTmp - 90.0# * turnSide, 360.0#)) Then Exit Function

        ptTo = PointAlongPlane(ptTo, fTmp, TurnR)
        ptTo.M = Modulus(fTmp - 90.0# * turnSide, 360.0#)
        pPointCollection.AddPoint(ptTo)

        CalcDMEInterception = pPointCollection

    End Function

    Function Procedure_45_180_OR_80_260(ByVal ptTurn As IPoint, ByVal fDir As Double, ByVal r As Double, _
    ByRef pcol As IPointCollection, ByVal turnSide As Long, ByVal is45_180 As Boolean) As Boolean

        Procedure_45_180_OR_80_260 = False
        Dim A As Double
        Dim d As Double
        'Dim ptTmp As IPoint
        Dim PtTmp1 As IPoint
        Dim PtTmp2 As IPoint
        Dim ptTmp3 As IPoint
        Dim ptTmp4 As IPoint
        Dim turnDir As Double
        Dim tan_a_2 As Double

        A = IIf(is45_180, 45.0#, 80.0#)
        tan_a_2 = Math.Tan(DegToRad(A / 2.0#))
        If tan_a_2 = 0 Then
            Exit Function
        End If

        pcol = New Multipoint

        pcol.AddPoint(ptTurn)
        pcol.Point(pcol.PointCount - 1).M = fDir

        turnDir = fDir - (turnSide * A)
        d = r * tan_a_2


        PtTmp1 = PointAlongPlane(ptTurn, fDir, d)
        PtTmp2 = PointAlongPlane(PtTmp1, turnDir, d)

        pcol.AddPoint(PtTmp2)
        pcol.Point(pcol.PointCount - 1).M = turnDir

        d = r / tan_a_2
        ptTmp3 = PointAlongPlane(PtTmp1, turnDir, d)

        pcol.AddPoint(ptTmp3)
        pcol.Point(pcol.PointCount - 1).M = turnDir

        ptTmp4 = PointAlongPlane(PtTmp1, fDir, d)

        pcol.AddPoint(ptTmp4)
        pcol.Point(pcol.PointCount - 1).M = Modulus(fDir + 180.0#, 360.0#)

        Dim pClone As IClone
        pClone = ptTurn
        Dim pt As IPoint
        pt = pClone.Clone()

        pcol.AddPoint(pt)
        pcol.Point(pcol.PointCount - 1).M = pcol.Point(pcol.PointCount - 2).M

        Procedure_45_180_OR_80_260 = True

    End Function

End Class
