Attribute VB_Name = "Functions"
Option Explicit
Option Base 0
Option Compare Text

Public Function LoadResString(ByVal strID As Long) As String
    Dim str As String
    Dim nCopy As Long
    Dim i As Long
    Dim bX() As Byte

    str = String(4096, vbNullChar)
    
    If Not Application Is Nothing Then App_hInstance = App.HINSTANCE
    
    nCopy = GetStringFromTable(App_hInstance, strID, str, 4096, GetThreadLocale())
    
    If nCopy >= 0 Then
        LoadResString = Left(str, nCopy)
    Else
        bX = StrConv(str, vbFromUnicode)   ' Convert string.
        nCopy = Len(str)
        For i = 0 To UBound(bX)
            If bX(i) = 0 Then
                nCopy = i
                Exit For
            End If
        Next
        LoadResString = Left(str, nCopy)
    End If
End Function

Public Function ConvertDistance(ByVal val As Double, ByVal RoundMode As Long) As Double
If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
Select Case RoundMode
    Case 0
        ConvertDistance = val * DistanceConverter(DistanceUnit).Multiplier
    Case 1
        ConvertDistance = Round(val * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding - 0.4999999) * DistanceConverter(DistanceUnit).Rounding
    Case 2
        ConvertDistance = Round(val * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding) * DistanceConverter(DistanceUnit).Rounding
    Case 3
        ConvertDistance = Round(val * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding + 0.4999999) * DistanceConverter(DistanceUnit).Rounding
End Select
End Function

Public Function ConvertHeight(ByVal val As Double, ByVal RoundMode As Long) As Double
If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
Select Case RoundMode
    Case 0
        ConvertHeight = val * HeightConverter(HeightUnit).Multiplier
    Case 1
        ConvertHeight = Round(val * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding - 0.4999999) * HeightConverter(HeightUnit).Rounding
    Case 2
        ConvertHeight = Round(val * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
    Case 3
        ConvertHeight = Round(val * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding + 0.4999999) * HeightConverter(HeightUnit).Rounding
End Select
End Function

Public Function ConvertSpeed(ByVal val As Double, ByVal RoundMode As Long) As Double
If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
Select Case RoundMode
    Case 0
        ConvertSpeed = val * SpeedConverter(SpeedUnit).Multiplier
    Case 1
        ConvertSpeed = Round(val * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding - 0.4999999) * SpeedConverter(SpeedUnit).Rounding
    Case 2
        ConvertSpeed = Round(val * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding) * SpeedConverter(SpeedUnit).Rounding
    Case 3
        ConvertSpeed = Round(val * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding + 0.4999999) * SpeedConverter(SpeedUnit).Rounding
End Select
End Function

Public Function ConvertDSpeed(ByVal val As Double, ByVal RoundMode As Long) As Double
If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
Select Case RoundMode
    Case 0
        ConvertDSpeed = val * DSpeedConverter(HeightUnit).Multiplier
    Case 1
        ConvertDSpeed = Round(val * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding - 0.4999999) * DSpeedConverter(HeightUnit).Rounding
    Case 2
        ConvertDSpeed = Round(val * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding) * DSpeedConverter(HeightUnit).Rounding
    Case 3
        ConvertDSpeed = Round(val * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding + 0.4999999) * DSpeedConverter(HeightUnit).Rounding
End Select
End Function

Public Function DeConvertDistance(ByVal val As Double) As Double
    DeConvertDistance = val / DistanceConverter(DistanceUnit).Multiplier
End Function

Public Function DeConvertHeight(ByVal val As Double) As Double
    DeConvertHeight = val / HeightConverter(HeightUnit).Multiplier
End Function

Public Function DeConvertSpeed(ByVal val As Double) As Double
    DeConvertSpeed = val / SpeedConverter(SpeedUnit).Multiplier
End Function

Public Function DeConvertDSpeed(ByVal val As Double) As Double
    DeConvertDSpeed = val / DSpeedConverter(HeightUnit).Multiplier
End Function

'Public Sub ptToStr(Pt As IPoint, sLat As String, sLon As String)
'Dim xStr1 As String
'Dim yStr1 As String
'Dim LonSide As String
'Dim LatSide As String
'
'    If Pt.X > 0 Then
'        LonSide = "E"
'    Else
'        LonSide = "W"
'    End If
'
'    If Pt.Y > 0 Then
'        LatSide = "N"
'    Else
'        LatSide = "S"
'    End If
'
'    PandaExt.DD2Str Pt.X, Pt.Y, xStr1, yStr1, LonSide, LatSide
'    sLat = yStr1
'    sLon = xStr1
'End Sub
'
'Public Sub ptToStrPrj(Pt As IPoint, sLat As String, sLon As String)
'Dim xStr1 As String
'Dim yStr1 As String
'Dim LonSide As String
'Dim LatSide As String
'Dim pGeom As IGeometry
'Dim ptTmp As IPoint
'
'    Set ptTmp = New Point
'    ptTmp.PutCoords Pt.X, Pt.Y
'    Set pGeom = ptTmp
'    Set pGeom.SpatialReference = pSpRefPrj
'    pGeom.Project pSpRefShp
'
'    If ptTmp.X > 0 Then
'        LonSide = "E"
'    Else
'        LonSide = "W"
'    End If
'
'    If ptTmp.Y > 0 Then
'        LatSide = "N"
'    Else
'        LatSide = "S"
'    End If
'
'    PandaExt.DD2Str ptTmp.X, ptTmp.Y, xStr1, yStr1, LonSide, LatSide
'    Set ptTmp = Nothing
'    sLat = yStr1
'    sLon = xStr1
'End Sub
'
'Public Function MyDD2Str(X As Double, Mode As Integer) As String
'Dim xDeg As Double
'Dim xMin As Double
'Dim xIMin As Double
'Dim xSec As Double
'Dim lSign As Long
'Dim sSign As String
'Dim sTmp As String
'
'lSign = Sgn(X)
'X = Abs(X)
'X = Modulus(X, 360#)
'
'sSign = ""
'If lSign < 0 Then sSign = "-"
'    xDeg = Int(X)
'
'Select Case Mode
'Case 0 ' DD
'    MyDD2Str = sSign + str(Round(X, 6)) + "°"
'Case 1 ' DDMM
'    sTmp = sSign + str(xDeg) + "°"
'    xMin = Round((X - xDeg) * 60#, 4)
'    MyDD2Str = sTmp + str(xMin) + "'"
'Case 2 To 4 ' DDMMSS
'    If Mode <> 2 Then sSign = ""
'    xDeg = Int(X)
'    sTmp = sSign + CStr(xDeg) + "°"
'    xMin = (X - xDeg) * 60#
'    xIMin = Int(xMin)
'    sTmp = sTmp + CStr(xIMin) + "'"
'    xSec = (xMin - xIMin) * 60#
'    MyDD2Str = sTmp + CStr(Round(xSec, 2)) + """"
'
'    If Mode = 3 Then
'        If (lSign > 0) Then
'            MyDD2Str = MyDD2Str + "N"
'        Else
'            MyDD2Str = MyDD2Str + "S"
'        End If
'    ElseIf Mode = 4 Then
'        If (lSign > 0) Then
'            MyDD2Str = MyDD2Str + "E"
'        Else
'            MyDD2Str = MyDD2Str + "W"
'        End If
'    End If
'End Select
'End Function

Public Function Dir2Azimuth(ByVal ptPrj As IPoint, ByVal dir As Double) As Double
Dim resD As Double
Dim resI As Double
Dim Pt10 As IPoint
Dim Pt11 As IPoint
Dim i As Long


Set Pt11 = ToGeo(ptPrj)
Set Pt10 = ToGeo(PointAlongPlane(ptPrj, dir, 10#))

i = ReturnGeodesicAzimuth(Pt11.X, Pt11.y, Pt10.X, Pt10.y, resD, resI)
Dir2Azimuth = resD
End Function

Function Azt2Direction(ByVal ptGeo As IPoint, ByVal Azt As Double) As Double
Dim pGeo As IGeometry
Dim pClone As IClone
Dim pLine As ILine
Dim resx As Double
Dim resy As Double

Dim Pt10 As IPoint
Dim Pt11 As IPoint
Dim j As Long

'=================================================================================
'Set Pt11 = ToGeo(ptGeo)

j = PointAlongGeodesic(ptGeo.X, ptGeo.y, 10#, Azt, resx, resy)

Set pClone = ptGeo
Set Pt10 = pClone.Clone

Pt10.PutCoords resx, resy

Set pLine = New esriGeometry.Line
pLine.PutCoords ptGeo, Pt10

Set pGeo = pLine
Set pGeo.SpatialReference = pSpRefShp
pGeo.Project pSpRefPrj

Azt2Direction = Modulus(RadToDeg(pLine.angle), 360#)
Set Pt10 = Nothing
Set pLine = Nothing

End Function

Public Function ToGeo(pGeo As IGeometry) As IGeometry
Dim pClone As IClone
    Set pClone = pGeo
    Set ToGeo = pClone.Clone
    Set ToGeo.SpatialReference = pSpRefPrj
    ToGeo.Project pSpRefShp
End Function

Public Function ToProject(pGeo As IGeometry) As IGeometry
Dim pClone As IClone
    Set pClone = pGeo
    Set ToProject = pClone.Clone
    Set ToProject.SpatialReference = pSpRefShp
    ToProject.Project pSpRefPrj
End Function

Public Function IAS2TAS(IAS As Double, h As Double, dT As Double) As Double
    IAS2TAS = IAS * 171233# * Sqr(288# + dT - 0.006496 * h) / ((288# - 0.006496 * h) ^ 2.628)
End Function

Public Function Bank2Radius(Bank As Double, V As Double) As Double

    Dim Rv As Double
    
    Rv = 6.355 * Tan(DegToRad(Bank)) / (Pi * V)
    
    If (Rv > 0.003) Then
        Rv = 0.003
    End If
    
    If (Rv > 0) Then
        Bank2Radius = V / (20# * Pi * Rv)
    Else
        Bank2Radius = -1
    End If

End Function

Public Function Radius2Bank(r As Double, V As Double) As Double

    Radius2Bank = 0#
    
    If (r > 0#) Then
        Radius2Bank = RadToDeg(Atn(V * V / (20# * r * 6.355)))
    Else
        Radius2Bank = -1
    End If

End Function

Function DistToSpeed(Dist As Double) As Double
Dim SpeedList() ' As Double
'ReDim SpeedList(0 To 25)
Dim i As Long
Dim fTmp As Double
SpeedList = Array(356#, 370#, 387#, 404#, 424#, 441#, 452#, 459#, 467#, 472#, 478#, _
                  483#, 487#, 491#, 493#, 494#, 498#, 502#, 504#, 511#, 515#, 519#, 524#, 526#, 530#)
fTmp = Dist / 1852#
i = Round(fTmp + 0.4999999)
If i > 24 Then i = 24
If i < 0 Then i = 0
DistToSpeed = SpeedList(i)
End Function

Function HeightToBank(h As Double) As Double
    If h > 914.4 Then
        HeightToBank = 25#
    ElseIf h > 304.8 Then
        HeightToBank = 20#
    Else
        HeightToBank = 15#
    End If
End Function

'Public Function CalcMAPtDistD(H As Double, Categoty As Long) As Double
'Dim fTAS As Double
'
'fTAS = IAS2TAS(3.6 * cVmaInter.Values(Categoty), H, arISAmax.Value) + 3.6 * arNearTerrWindSp.Value
'CalcMAPtDistD = 0.277777777777778 * fTAS * arPilotTolerance.Value
'
'End Function
'
'Public Function CalcMAPtDistX(H As Double, Categoty As Long) As Double
'Dim fTAS As Double
'
'fTAS = IAS2TAS(3.6 * cVmaInter.Values(Categoty), H, arISAmax.Value) + 3.6 * arNearTerrWindSp.Value
'CalcMAPtDistX = 0.277777777777778 * fTAS * arSOCdelayTime.Value
'
'End Function

Public Function ArcSin(ByVal X As Double) As Double
    If Abs(X) >= 1# Then
        If X > 0# Then
            ArcSin = 0.5 * Pi
        Else
            ArcSin = -0.5 * Pi
        End If
    Else
        ArcSin = Atn(X / Sqr(-X * X + 1#))
    End If
End Function

Public Function ArcCos(ByVal X As Double) As Double
    If Abs(X) >= 1# Then
        ArcCos = 0#
    Else
        ArcCos = Atn(-X / Sqr(-X * X + 1#)) + 0.5 * Pi
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

'Public Function Det(X0 As Double, Y0 As Double, _
'                X1 As Double, Y1 As Double) As Double
'    Det = X0 * Y1 - X1 * Y0
'End Function

'Public Function IntersectPlanes(A As D3DPolygone, B As D3DPolygone, ByVal Hmin As Double, ByVal hMax As Double) As IPolyline
'Dim D As Double
'Dim dX As Double
'Dim dY As Double
'
'Dim pt0 As IPoint
'Dim pt1 As IPoint
'
'    Set IntersectPlanes = New Polyline
'
'    D = Det(A.Plane.A, A.Plane.B, B.Plane.A, B.Plane.B)
'    If D = 0# Then Exit Function
'
'    dX = Det(-(A.Plane.D + A.Plane.C * Hmin), A.Plane.B, -(B.Plane.D + B.Plane.C * Hmin), B.Plane.B)
'    dY = Det(A.Plane.A, -(A.Plane.D + A.Plane.C * Hmin), B.Plane.A, -(B.Plane.D + B.Plane.C * Hmin))
'    Set pt0 = New Point
'    pt0.X = dX / D
'    pt0.Y = dY / D
'
'    dX = Det(-(A.Plane.D + A.Plane.C * hMax), A.Plane.B, -(B.Plane.D + B.Plane.C * hMax), B.Plane.B)
'    dY = Det(A.Plane.A, -(A.Plane.D + A.Plane.C * hMax), B.Plane.A, -(B.Plane.D + B.Plane.C * hMax))
'    Set pt1 = New Point
'    pt1.X = dX / D
'    pt1.Y = dY / D
'
'    IntersectPlanes.FromPoint = pt0
'    IntersectPlanes.ToPoint = pt1
'End Function

'Sub CreateNavaidZone(NavFacil As NavaidData, DirAngle As Double, ptTHRprj As Point, _
'    Ss As Double, Vs As Double, FrontLen As Double, BackLen As Double, _
'    LPolygon As IPointCollection, RPolygon As IPointCollection, PrimPolygon As IPointCollection) ', Optional ByVal drawPolys As Boolean = False)
'Dim BaseLength As Double
'Dim ILSDir As Double
'Dim Alpha As Double
'Dim Betta As Double
'Dim hMax As Double
'Dim d0 As Double
'Dim d1 As Double
'Dim I As Long
'
'Dim pt0 As IPoint
'Dim pt1 As IPoint
'Dim pt2 As IPoint
'Dim pt3 As IPoint
'Dim Pt4 As IPoint
'Dim pt5 As IPoint
'Dim lOASPlanes(8) As D3DPolygone
'
'If LPolygon.PointCount > 0 Then LPolygon.RemovePoints 0, LPolygon.PointCount
'If RPolygon.PointCount > 0 Then RPolygon.RemovePoints 0, RPolygon.PointCount
'If PrimPolygon.PointCount > 0 Then PrimPolygon.RemovePoints 0, PrimPolygon.PointCount
'
'If NavFacil.NavTypeCode = 3 Then
'    Dim Xlf As IPointCollection
'    Dim Xrt As IPointCollection
'    Dim pZPlane As IPointCollection
'    Dim pPlane01 As IPointCollection
'    Dim pPlane02 As IPointCollection
'
'    Dim pConstruct As IConstructPoint
'    Dim pLine As ILine
'    Dim pTopo As ITopologicalOperator2
'
'    ILSDir = Azt2Direction(NavFacil.Pt, NavFacil.Pt.M + 180#)
'    OAS_DATABase NavFacil.LLZ_THR, 3#, 0.025, 1, NavFacil.GP_RDH, Ss, Vs, lOASPlanes
'    CreateOASPlanes ptTHRprj, ILSDir, 300#, lOASPlanes, 1 ', drawPolys
'
'    Set LPolygon = lOASPlanes(YlPlane).Poly
'    Set RPolygon = lOASPlanes(YrPlane).Poly
'
'    Set pt0 = PointAlongPlane(NavFacil.ptPrj, ILSDir, 10# * FrontLen)
'
'    Set Xlf = ReArrangePolygon(lOASPlanes(XlPlane).Poly, pt0, ILSDir)
'    Set Xrt = ReArrangePolygon(lOASPlanes(XrPlane).Poly, pt0, ILSDir + 180#)
'
'    Set pt1 = New Point
'    Set pt2 = New Point
'    Set pLine = New esriGeometry.Line
'
'    Set pt0 = PointAlongPlane(NavFacil.ptPrj, ILSDir + 180#, FrontLen)
'
'    pLine.FromPoint = Xlf.Point(Xlf.PointCount - 1)
'    pLine.ToPoint = Xlf.Point(0)
'
'    Set pConstruct = pt1
'    pConstruct.ConstructAngleIntersection pLine.ToPoint, pLine.angle, pt0, DegToRad(ILSDir + 90#)
'
'    pLine.FromPoint = Xrt.Point(Xrt.PointCount - 1)
'    pLine.ToPoint = Xrt.Point(Xrt.PointCount - 2)
'
'    Set pConstruct = pt2
'    pConstruct.ConstructAngleIntersection pLine.ToPoint, pLine.angle, pt0, DegToRad(ILSDir + 90#)
'
'    Set pPlane01 = New Polygon
'    pPlane01.AddPoint pt1
'    pPlane01.AddPoint pt2
'    pPlane01.AddPoint pLine.FromPoint
'    pPlane01.AddPoint Xlf.Point(Xlf.PointCount - 1)
'
'    Set pTopo = pPlane01
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'
'    Set pPlane02 = pTopo.Union(lOASPlanes(ZeroPlane).Poly)
''=======================================================================
'    Set pZPlane = ReArrangePolygon(lOASPlanes(ZPlane).Poly, ptTHRprj, ILSDir)
'
'    Set pt0 = PointAlongPlane(NavFacil.ptPrj, ILSDir, BackLen)
'
'    pLine.FromPoint = pZPlane.Point(0)
'    pLine.ToPoint = pZPlane.Point(1)
'
'    Set pConstruct = pt1
'    pConstruct.ConstructAngleIntersection pZPlane.Point(1), pLine.angle - DegToRad(arMA_SplayAngle.Value), pt0, DegToRad(ILSDir + 90#)
'
'    pLine.FromPoint = pZPlane.Point(3)
'    pLine.ToPoint = pZPlane.Point(2)
'
'    Set pConstruct = pt2
'    pConstruct.ConstructAngleIntersection pZPlane.Point(2), pLine.angle + DegToRad(arMA_SplayAngle.Value), pt0, DegToRad(ILSDir + 90#)
'
'    Set pPlane01 = New Polygon
'
'    pPlane01.AddPoint pZPlane.Point(0)
'    pPlane01.AddPoint pZPlane.Point(1)
'    pPlane01.AddPoint pt1
'    pPlane01.AddPoint pt2
'    pPlane01.AddPoint pZPlane.Point(2)
'    pPlane01.AddPoint pZPlane.Point(3)
'
'    Set pTopo = pPlane01
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'
'    Set PrimPolygon = pTopo.Union(pPlane02)
'    Set pTopo = PrimPolygon
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
''===========================================================================================
'Else
'    If NavFacil.NavTypeCode = 2 Then
'        BaseLength = NDB.InitWidth * 0.5
'        Alpha = NDB.SplayAngle
'    ElseIf NavFacil.NavTypeCode = 0 Then
'        BaseLength = 0.5 * VOR.InitWidth
'        Alpha = VOR.SplayAngle
'    Else
'        Exit Sub
'    End If
'
'    d0 = FrontLen / Cos(DegToRad(Alpha))
'    d1 = BackLen / Cos(DegToRad(Alpha))
'
'    Betta = 0.5 * Tan(DegToRad(Alpha))
'    Betta = Atn(Betta)
'    Betta = RadToDeg(Betta)
'
''==========LeftPolygon
'    Set pt0 = PointAlongPlane(NavFacil.ptPrj, DirAngle + 90#, BaseLength)
'    Set pt3 = PointAlongPlane(NavFacil.ptPrj, DirAngle + 90#, 0.5 * BaseLength)
'
'    Set pt1 = PointAlongPlane(pt0, DirAngle + Alpha, d1)
'    Set pt2 = PointAlongPlane(pt3, DirAngle + Betta, d1)
'
'    LPolygon.AddPoint pt0
'    LPolygon.AddPoint pt1
'    LPolygon.AddPoint pt2
'    LPolygon.AddPoint pt3
'
'    If d1 > 0# Then
'        Set Pt4 = PointAlongPlane(pt3, DirAngle + 180# - Betta, d0)
'        Set pt5 = PointAlongPlane(pt0, DirAngle + 180# - Alpha, d0)
'        LPolygon.AddPoint Pt4
'        LPolygon.AddPoint pt5
'        PrimPolygon.AddPoint Pt4
'    End If
'    PrimPolygon.AddPoint pt3
'    PrimPolygon.AddPoint pt2
'
''==========RightPolygon
'    Set pt0 = PointAlongPlane(NavFacil.ptPrj, DirAngle - 90#, 0.5 * BaseLength)
'    Set pt3 = PointAlongPlane(NavFacil.ptPrj, DirAngle - 90#, BaseLength)
'    Set pt1 = PointAlongPlane(pt0, DirAngle - Betta, d1)
'    Set pt2 = PointAlongPlane(pt3, DirAngle - Alpha, d1)
'
'    RPolygon.AddPoint pt0
'    RPolygon.AddPoint pt1
'    RPolygon.AddPoint pt2
'    RPolygon.AddPoint pt3
'
'    If d1 > 0# Then
'        Set Pt4 = PointAlongPlane(pt3, DirAngle + 180# + Alpha, d0)
'        Set pt5 = PointAlongPlane(pt0, DirAngle + 180# + Betta, d0)
'        RPolygon.AddPoint Pt4
'        RPolygon.AddPoint pt5
'    End If
'
'    PrimPolygon.AddPoint pt1
'    PrimPolygon.AddPoint pt0
'    If d1 > 0# Then PrimPolygon.AddPoint pt5
'End If
'
'End Sub

Public Function CreatePrjCircle(pPoint1 As IPoint, r As Double) As IPointCollection
Dim i As Long
Dim iInRad As Double
Dim pt As IPoint
Dim pPolygon As IPointCollection
Dim pTopo As ITopologicalOperator2

Set pt = New Point
Set pPolygon = New Polygon
'toRad = Pi / 180#
For i = 0 To 359
    iInRad = DegToRad(i)
    pt.X = pPoint1.X + r * Cos(iInRad)
    pt.y = pPoint1.y + r * Sin(iInRad)
    pPolygon.AddPoint pt
Next i

Set pTopo = pPolygon
pTopo.IsKnownSimple = False
pTopo.Simplify

Set CreatePrjCircle = pPolygon
End Function

Function CreateArcPrj(ptCnt As IPoint, ptFrom As IPoint, _
            ptTo As IPoint, ClWise As Long) As IPointCollection
            
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
    Dim pGeo As IGeometry
    
    Set ptCur = New Point
    Set pt = New Point
    Set CreateArcPrj = New Polygon
    
    dX = ptFrom.X - ptCnt.X
    dY = ptFrom.y - ptCnt.y
    r = Sqr(dX * dX + dY * dY)
    
    AztFrom = RadToDeg(ATan2(dY, dX))
    AztFrom = Modulus(AztFrom, 360#)
    
    AztTo = RadToDeg(ATan2(ptTo.y - ptCnt.y, ptTo.X - ptCnt.X))
    AztTo = Modulus(AztTo, 360#)
    
    Dim daz As Double
    Dim saz As Integer
    
    daz = Modulus((AztTo - AztFrom) * ClWise, 360#)
    AngStep = 1
    i = daz / AngStep
    If (i < 10) Then i = 10
    AngStep = daz / i
    
    CreateArcPrj.AddPoint ptFrom
    For j = 1 To i - 1
        iInRad = DegToRad(AztFrom + j * AngStep * ClWise)
        pt.X = ptCnt.X + r * Cos(iInRad)
        pt.y = ptCnt.y + r * Sin(iInRad)
        CreateArcPrj.AddPoint pt
    Next j
    CreateArcPrj.AddPoint ptTo

End Function

Function CreateArcPrj2(ptCnt As IPoint, ptFrom As IPoint, _
            ptTo As IPoint, ClWise As Long) As Path
    
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
    Dim pGeo As IGeometry
    
    Set ptCur = New Point
    Set pt = New Point
    
    Dim pcol As IPointCollection
    Set pcol = New Path
    
    dX = ptFrom.X - ptCnt.X
    dY = ptFrom.y - ptCnt.y
    r = Sqr(dX * dX + dY * dY)
    
    AztFrom = RadToDeg(ATan2(dY, dX))
    AztFrom = Modulus(AztFrom, 360#)
    
    AztTo = RadToDeg(ATan2(ptTo.y - ptCnt.y, ptTo.X - ptCnt.X))
    AztTo = Modulus(AztTo, 360#)
    
    Dim daz As Double
    Dim saz As Integer
    
    daz = Modulus((AztTo - AztFrom) * ClWise, 360#)
    AngStep = 1
    i = daz / AngStep
    If (i < 10) Then i = 10
    AngStep = daz / i
    
    pcol.AddPoint ptFrom
    For j = 1 To i - 1
        iInRad = DegToRad(AztFrom + j * AngStep * ClWise)
        pt.X = ptCnt.X + r * Cos(iInRad)
        pt.y = ptCnt.y + r * Sin(iInRad)
        pcol.AddPoint pt
    Next j
    pcol.AddPoint ptTo
    
    Set CreateArcPrj2 = pcol

End Function

Function CreateArcPrj_ByArc(ptCnt As IPoint, ptFrom As IPoint, _
            ptTo As IPoint, side As Long) As ISegment
            
    Dim ClWise As Boolean
    Dim Rds As Double
    Dim isMinor As Boolean
    Rds = ReturnDistanceAsMeter(ptCnt, ptFrom)
    isMinor = (Modulus(side * (ptTo.M - ptFrom.M), 360#) > 180#)
    
    Dim ha As Double
    ha = Modulus(-side * (ptTo.M - ptFrom.M), 360#)
    Dim ptMid As IPoint
    'Set ptMid = PointAlongPlane(ptCnt, ptFrom.M - (side * ha / 2), Rds)
    Set ptMid = PointAlongPlane(ptCnt, ReturnAngleAsDegree(ptCnt, ptFrom) - side * (ha / 2), Rds)
    
    'DrawPoint ptMid
    ClWise = (side = 1)
    
    Dim pCircularArc As IConstructCircularArc
    Set pCircularArc = New CircularArc
    'pCircularArc.ConstructEndPointsRadius ptFrom, ptTo, Not ClWise, Rds, isMinor
    pCircularArc.ConstructThreePoints ptFrom, ptMid, ptTo, True
    Set CreateArcPrj_ByArc = pCircularArc

End Function

Function CreateArcPrj_ByArc_2(ptCnt As IPoint, ptFrom As IPoint, _
            ptTo As IPoint, side As Long) As ISegment
            
    Dim ClWise As Boolean
    Dim Rds As Double
    Dim isMinor As Boolean
    Rds = ReturnDistanceAsMeter(ptCnt, ptFrom)
    isMinor = (Modulus(side * (ptTo.M - ptFrom.M), 360#) > 180#)
    
    ClWise = (side = 1)
    
    Dim pCircularArc As IConstructCircularArc
    Set pCircularArc = New CircularArc
    pCircularArc.ConstructEndPointsRadius ptFrom, ptTo, Not ClWise, Rds, isMinor
    Set CreateArcPrj_ByArc_2 = pCircularArc

End Function

Function SpiralTouchAngle(r0 As Double, E As Double, _
        aztNominal As Double, AztTouch As Double, turnDir As Long) As Double

Dim i As Long
Dim TurnAngle As Double
Dim TouchAngle As Double
Dim d As Double
Dim delta As Double
Dim DegE As Double

    TouchAngle = Modulus((AztTouch - aztNominal) * turnDir, 360#)
    TouchAngle = DegToRad(TouchAngle)
    TurnAngle = TouchAngle
    DegE = RadToDeg(E)

    For i = 0 To 9
        d = DegE / (r0 + DegE * TurnAngle)
        delta = (TurnAngle - TouchAngle - Atn(d)) / (2# - 1# / (d * d + 1#))
        TurnAngle = TurnAngle - delta
        If (Abs(delta) < radEps) Then
            Exit For
        End If
    Next i
SpiralTouchAngle = RadToDeg(TurnAngle)

If SpiralTouchAngle < 0# Then
    SpiralTouchAngle = Modulus(SpiralTouchAngle, 360#)
End If

End Function

'Public Function CalcSpiralStartPoint(LinePoint As IPointCollection, _
'        detObs As ObstacleAr, coef As Double, r0 As Double, _
'        ArDir As Double, TurnDir As Long) As Interval
'
'Dim ptTmp As IPoint
'Dim PtTmp1 As IPoint
'Dim PtTmp2 As IPoint
'Dim ptCnt As IPoint
'Dim pConstructor As IConstructPoint
'Dim pProxi As IProximityOperator
'Dim BasePoints() As IPoint
'Dim dAlpha As Double
'Dim MaxTheta As Double
'Dim Alpha As Double
'Dim Theta As Double
'Dim hT As Double
'Dim hTMin As Double
'Dim Dist As Double
'Dim fTmp As Double
'Dim pLine As ILine
'Dim Offset As Long
'Dim N As Long
'Dim M As Long
'Dim I As Long
'Dim iMin As Long
'Dim IL As Long
'Dim ir As Long
'Dim Side1 As Long
'Dim Side As Long
'Dim PtTurn As IPoint
'Dim ASinAlpha As Double
'
''If bDerFlg Then
''    Offset = 1
''Else
'    Offset = 0
''End If
'
'Set pLine = New esriGeometry.Line
'Set pProxi = pLine
'
'N = LinePoint.PointCount - Offset
'
'ReDim BasePoints(N)
'
'For I = 0 To N - 1
'    Set BasePoints(I) = LinePoint.Point(I + Offset)
'    If I = N - 1 Then
'        BasePoints(I).M = BasePoints(I - 1).M
'    Else
'        BasePoints(I).M = ReturnAngleAsDegree(LinePoint.Point(I + Offset), LinePoint.Point(I + Offset + 1))
'    End If
'Next
'
'Set ptCnt = New Point
'Set pConstructor = ptCnt
'
'Set PtTurn = Nothing
'
'hTMin = R
'iMin = -1
'
''DrawPolyLine LinePoint, 0
'
'MaxTheta = SpiralTouchAngle(r0, coef, ArDir, ArDir + 90# * TurnDir, TurnDir)
'If MaxTheta > 180# Then MaxTheta = 360# - MaxTheta
'
'For I = 0 To N - 2
'    Side = SideDef(BasePoints(I), BasePoints(I).M, detObs.ptPrj)
'    Alpha = ArDir + 90# * Side
'
'    dAlpha = SubtractAngles(Alpha, BasePoints(I).M)
'    Set ptTmp = PointAlongPlane(BasePoints(I), ArDir - 90# * Side, r0)
'
'    Dist = Point2LineDistancePrj(detObs.ptPrj, ptTmp, BasePoints(I).M)
'    Side1 = SideDef(ptTmp, BasePoints(I).M, detObs.ptPrj)
'
'    Theta = 0.5 * MaxTheta
'    Do
'        fTmp = Theta
'        ASinAlpha = Dist / (r0 + Theta * coef)
'        If Abs(ASinAlpha) <= 1# Then
'            Theta = dAlpha - RadToDeg(Side1 * TurnDir * ArcSin(ASinAlpha))
'        Else
'            Theta = MaxTheta
'            Exit Do
'        End If
'    Loop While Abs(fTmp - Theta) > degEps
'
'    fTmp = Sin(DegToRad(Theta)) * (r0 + Theta * coef)
'
'    If Theta > MaxTheta Then
'        hT = Sin(DegToRad(MaxTheta)) * (r0 + MaxTheta * coef)
'        fTmp = (hT - fTmp)
'        Theta = MaxTheta
'    Else
'        hT = fTmp
'        fTmp = 0#
'    End If
'
''    DrawPoint detObs.ptPrj, 255
''    DrawPoint BasePoints(0), 0
''    DrawPoint BasePoints(1), 255
''    DrawPoint BasePoints(2), 0
''    DrawPoint BasePoints(3), 255
''    DrawPoint BasePoints(4), 0
'
'    Set PtTmp2 = PointAlongPlane(detObs.ptPrj, ArDir + 180#, hT + fTmp)
'    pConstructor.ConstructAngleIntersection PtTmp2, DegToRad(ArDir + 90#), ptTmp, DegToRad(BasePoints(I).M)
'
''    DrawPoint detObs.ptPrj, 255
''    DrawPoint PtCnt, RGB(255, 0, 255)
''    DrawPoint PtTmp2, 0
'
'    Set ptTmp = PointAlongPlane(ptCnt, ArDir - TurnDir * 90#, r0)
''    DrawPoint ptTmp, 0
'
'    pLine.FromPoint = BasePoints(I)
'    pLine.ToPoint = BasePoints(I + 1)
'
'    fTmp = pProxi.ReturnDistance(ptTmp)
'
'    If fTmp < distEps Then
'        If hT < hTMin Then
'            hTMin = hT
'            iMin = I
'            Set PtTurn = ptTmp
'            PtTurn.M = Theta
'            PtTurn.Z = detObs.Dist - hTMin
'            If (PtTurn.Z < 0#) Then
'                PtTurn.Z = 0#
'            End If
'        End If
'    End If
'Next I
'
'If iMin > -1 Then
'    CalcSpiralStartPoint.Left = PtTurn.Z
'    CalcSpiralStartPoint.Right = PtTurn.M
'Else
'    CalcSpiralStartPoint.Left = -9999#
'    CalcSpiralStartPoint.Right = -9999#
'End If
'
'End Function

Public Function ReturnAngleAsDegree(ptFrom As IPoint, ptOut As IPoint) As Double
Dim fdX As Double, fdY As Double
    fdX = ptOut.X - ptFrom.X
    fdY = ptOut.y - ptFrom.y
    ReturnAngleAsDegree = Modulus(RadToDeg(ATan2(fdY, fdX)), 360#)
End Function

Public Function ReturnDistanceAsMeter(ptFrom As IPoint, ptOut As IPoint) As Double
Dim fdX As Double, fdY As Double
    fdX = ptOut.X - ptFrom.X
    fdY = ptOut.y - ptFrom.y
    ReturnDistanceAsMeter = Sqr(fdX * fdX + fdY * fdY)
End Function

Function SubtractAngles(ByVal X As Double, ByVal y As Double) As Double
    X = Modulus(X, 360#)
    y = Modulus(y, 360#)
    SubtractAngles = Modulus(X - y, 360#)
    If SubtractAngles > 180# Then SubtractAngles = 360# - SubtractAngles
End Function

Function SubtractAnglesWithSign(ByVal StRad As Double, ByVal EndRad As Double, Turn As Long) As Double
    SubtractAnglesWithSign = Modulus((EndRad - StRad) * Turn, 360#)
    If SubtractAnglesWithSign > 180# Then
        SubtractAnglesWithSign = SubtractAnglesWithSign - 360#
    End If
End Function

Public Function Quadric(A As Double, B As Double, C As Double, x0 As Double, x1 As Double) As Long
Dim d As Double
Dim fTmp As Double

d = B * B - 4 * A * C
If d < 0# Then
    Quadric = 0
ElseIf (d = 0#) Or (A = 0#) Then
    Quadric = 1
    If A = 0# Then
        x0 = -C / B
    Else
        x0 = -0.5 * B / A
    End If
Else
    Quadric = 2
    fTmp = 0.5 / A
    If fTmp > 0 Then
        x0 = (-B - Sqr(d)) * fTmp
        x1 = (-B + Sqr(d)) * fTmp
    Else
        x0 = (-B + Sqr(d)) * fTmp
        x1 = (-B - Sqr(d)) * fTmp
    End If
End If

End Function

Public Function CutNavPoly(poly As Polygon, CutLine As IPolyline, side As Long) As Polygon ', Optional bDraw As Boolean = False
Dim pTopoOper As ITopologicalOperator2
Dim pUnspecified As IPointCollection
Dim pArea As IArea
Dim pGeo As IGeometry
Dim pRight As Polygon
Dim pLeft As Polygon
Dim pLine As ILine
Dim dir As Double

    Set pLine = New esriGeometry.Line
    pLine.FromPoint = CutLine.FromPoint
    pLine.ToPoint = CutLine.ToPoint

    ClipByLine poly, CutLine, pLeft, pRight, pUnspecified
'If bDraw Then
'    DrawPolygon pRight, 255
'    DrawPolygon pUnspecified, 0
'    DrawPolygon pLeft, RGB(0, 0, 255)
'    DrawPolyLine CutLine, 255, 2
'    DrawPolygon Poly, RGB(0, 255, 0)
'End If
'    Set pGeo = pUnspecified
'    Set pArea = pUnspecified
'    Dir = pUnspecified.PointCount
'    Dir = pArea.Area

    Set CutNavPoly = New Polygon
    dir = RadToDeg(pLine.angle)

    If side > 0 Then
        Set pGeo = pLeft
        If Not pGeo.IsEmpty Then
            Set pGeo = pUnspecified
            If Not pGeo.IsEmpty Then '            If pArea.Area > 0.5 Then
                If SideDef(pLine.FromPoint, dir, pUnspecified.Point(0)) < 0 Then
                    Set pTopoOper = pLeft
                    Set CutNavPoly = pTopoOper.Union(pUnspecified)
                End If
            End If
            Set CutNavPoly = pLeft
        Else
            Set pGeo = pUnspecified
            If Not pGeo.IsEmpty Then
                If SideDef(pLine.FromPoint, dir, pUnspecified.Point(0)) < 0 Then
                    Set CutNavPoly = pUnspecified
                End If
            End If
        End If
    Else
        Set pGeo = pRight
        If Not pGeo.IsEmpty Then
            Set pGeo = pUnspecified
            If Not pGeo.IsEmpty Then '       If pArea.Area > 0.5 Then
                If SideDef(pLine.FromPoint, dir, pUnspecified.Point(0)) > 0 Then
                    Set pTopoOper = pRight
                    Set CutNavPoly = pTopoOper.Union(pUnspecified)
                End If
            End If
            Set CutNavPoly = pRight '        If Not pGeo.IsEmpty Then
        Else
            Set pGeo = pUnspecified
            If Not pGeo.IsEmpty Then
                If SideDef(pLine.FromPoint, dir, pUnspecified.Point(0)) > 0 Then
                    Set CutNavPoly = pUnspecified
                End If
            End If
        End If
    End If
    Set pTopoOper = CutNavPoly
    pTopoOper.IsKnownSimple = False
    pTopoOper.Simplify
End Function

Public Sub CutPoly(poly As Polygon, CutLine As Polyline, side As Long)

Dim Geocollect As IGeometryCollection
Dim Topo As ITopologicalOperator2
'Dim Topo1 As ITopologicalOperator
Dim tmpPoly As IPointCollection
Dim Cutter As IPolyline
Dim ptTmp As IPoint
Dim TmpPolygon As IPolygon2

Dim SRC As Long

Dim tmpAzt As Double
Dim dist0 As Double
Dim Dist1 As Double
Dim Dist As Double
Dim GIx As Long
Dim Ix As Long
'=================================
Dim pLeft As Polygon
Dim pRight As Polygon
Dim pUnspecified As Polygon
'=================================
On Error GoTo ErrorHandler
'SRC = 0
'Dim pPolygon As IPolygon
'Set pPolygon = Poly
'DrawPolygon pPolygon, 0
Set Cutter = CutLine

tmpAzt = ReturnAngleAsDegree(Cutter.FromPoint, Cutter.ToPoint)
Dist = ReturnDistanceAsMeter(Cutter.FromPoint, Cutter.ToPoint)

Set Topo = Cutter
Topo.IsKnownSimple = False
Topo.Simplify

Set Topo = poly
Topo.IsKnownSimple = False
Topo.Simplify

'Set TmpPolygon = Topo
'TmpPolygon.Generalize 20#

'=============================
'Cutter.ReverseOrientation
'DrawPolygon Poly, RGB(255, 255, 0)
'DrawPolyLine Cutter, 255

'Exit Sub
'Set PtTmp = New Point
'PtTmp.PutCoords Cutter.ToPoint.x + 10#, Cutter.ToPoint.Y
'Cutter.ToPoint = PtTmp
'Set Topo = Cutter
'Topo.Simplify
'Set Topo = Poly
'=============================

'Exit Sub
'Set Topo = pPolygon
'Topo.Simplify
'Set TmpPoly = Topo.Intersect(Topo1, 2)

'SRC = 1
'Set TmpPoly = Topo.Intersect(Cutter, 2)
Set tmpPoly = ClipByPoly(Cutter, poly)
'SRC = 2

If tmpPoly.PointCount <> 0 Then
    Set Geocollect = tmpPoly
    If Geocollect.GeometryCount > 1 Then
        For Ix = 0 To Geocollect.GeometryCount - 1
            Set tmpPoly = Geocollect.Geometry(Ix)
            dist0 = ReturnDistanceAsMeter(tmpPoly.Point(0), Cutter.FromPoint)
            Dist1 = ReturnDistanceAsMeter(tmpPoly.Point(1), Cutter.FromPoint)
            If dist0 > Dist1 Then dist0 = Dist1

            If Dist > dist0 Then
                GIx = Ix
                Dist = dist0
            End If
        Next Ix

        Set tmpPoly = Geocollect.Geometry(GIx)
        Dist = ReturnDistanceAsMeter(tmpPoly.Point(0), Cutter.FromPoint)
        dist0 = ReturnDistanceAsMeter(tmpPoly.Point(1), Cutter.FromPoint)

        If Dist < dist0 Then Dist = dist0
        Set ptTmp = PointAlongPlane(Cutter.FromPoint, tmpAzt, Dist + 5#)
        Cutter.ToPoint = ptTmp
    End If

'    If Side < 0 Then
'        Topo.Cut Cutter, TmpPoly, Poly
'    Else
'        Topo.Cut Cutter, Poly, TmpPoly
'    End If
'SRC = 3
    ClipByLine poly, Cutter, pLeft, pRight, pUnspecified
'SRC = 4

    If side < 0 Then
        Set poly = pRight
    Else
        Set poly = pLeft
    End If

End If

On Error GoTo 0

Exit Sub

ErrorHandler:    ' Error-handling routine.
    DrawPolyLine Cutter, 255
    DrawPolygon poly, RGB(255, 255, 0)
'    MsgDialog.ShowMessage "Ошибка среды 'ArcMap' фирмы 'ESRI'!!! ErrNum= " + CStr(Err.Number) + "  " + Err.Description
    MsgDialog.ShowMessage "Ошибка среды 'ArcMap'. ErrNum = " + CStr(Err.Number) + ": " + Err.Description
'    MsgDialog.ShowMessage "Ошибка среды 'ArcMap'. ErrNum= " + CStr(SRC) + "  " + Err.Description
    Exit Sub

    If Err.Number = -2147220943 Then

        Set ptTmp = PointAlongPlane(Cutter.ToPoint, tmpAzt + 90# * side, 0.01)
        Cutter.ToPoint = ptTmp
''        DrawPolyLine Cutter, 0
        Resume      ' Resume execution at same line that caused the error.
    Else 'If Err.Number <> -2147220968 Then
        MsgDialog.ShowMessage "Ошибка среды 'ArcMap' фирмы 'ESRI' !!!"
'        If Err.Number <> -2147220968 Then
'            DrawPolygon Poly, RGB(255, 255, 0)
'            DrawPolyLine Cutter, 255
'        End If
        Exit Sub
        Resume Next ' Resume execution at next line that caused the error.
    End If
            
End Sub

Public Function LineLineIntersect(Pt1 As Point, dir1 As Double, pt2 As Point, dir2 As Double) As Point
Dim Constructor As IConstructPoint

Set LineLineIntersect = New Point
Set Constructor = LineLineIntersect
Constructor.ConstructAngleIntersection Pt1, DegToRad(dir1), pt2, DegToRad(dir2)
End Function

Public Function CircleVectorIntersect(ptCent As IPoint, r As Double, ptVect As IPoint, DirVect As Double, Optional ptRes As IPoint) As Double
Dim ptTmp As IPoint
Dim DistCnt2Vect As Double
Dim fTmp As Double
Dim d As Double
Dim Constr As IConstructPoint

Set ptTmp = New Point
Set Constr = ptTmp

'DrawPoint PtCent, 255
'DrawPoint ptVect, 0

Constr.ConstructAngleIntersection ptCent, DegToRad(DirVect + 90#), ptVect, DegToRad(DirVect)

DistCnt2Vect = ReturnDistanceAsMeter(ptCent, ptTmp)

If DistCnt2Vect < r Then
    d = Sqr(r * r - DistCnt2Vect * DistCnt2Vect)
    Set ptRes = PointAlongPlane(ptTmp, DirVect, d)
    CircleVectorIntersect = d 'ReturnDistanceAsMeter(ptRes, ptTmp)
Else
    CircleVectorIntersect = -1#
    Set ptRes = New Point
End If

End Function

Public Function CircleCircleIntersect(PtCent1 As IPoint, R1 As Double, PtCent2 As IPoint, R2 As Double, turnDir As Long, Optional ptRes As IPoint) As Double
Dim Dist As Double
Dim h As Double
Dim A As Double
Dim x2 As Double
Dim y2 As Double

Dist = ReturnDistanceAsMeter(PtCent1, PtCent2)
Set ptRes = New Point
If Dist <= R1 + R2 Then
    A = (R1 * R1 - R2 * R2 + Dist * Dist) / (2 * Dist)
    h = Sqr(R1 * R1 - A * A)
    x2 = PtCent1.X + A * (PtCent2.X - PtCent1.X) / Dist
    y2 = PtCent1.y + A * (PtCent2.y - PtCent1.y) / Dist

    ptRes.X = x2 + turnDir * h * (PtCent2.y - PtCent1.y) / Dist
    ptRes.y = y2 - turnDir * h * (PtCent2.X - PtCent1.X) / Dist
    CircleCircleIntersect = h
Else
    CircleCircleIntersect = -1#
End If
End Function

Function FindCommonTochCircle(pPolygon As IPolygon, fAxis As Double, fTuchDist As Double, iTurnDir As Long, pResPt As IPoint) As Point
Dim pTopo As ITopologicalOperator2
Dim pProximityoperator As IProximityOperator
Dim pConstructPoint As IConstructPoint
Dim pCircle As IPointCollection
Dim fDist As Double
Dim fDir As Double

Dim fdX As Double
Dim fdY As Double
Dim fXp2 As Double
Dim fYp2 As Double
Dim fRx As Double
Dim fRy As Double
Dim fA As Double
Dim fH As Double
Dim pTuchPt As IPoint
Dim pPtTmp As IPoint

Dim i As Long
Dim j As Long

'DrawPolygon pPolygon
'DrawPolyLine pPolyline

Set pProximityoperator = pPolygon
Set pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, esriNoExtension)

fdX = pTuchPt.X - pResPt.X
fdY = pTuchPt.y - pResPt.y
fDist = Sqr(fdX * fdX + fdY * fdY)

i = 0
j = 0

Do While Abs(fDist - fTuchDist) > 0.25

'    If fDist <= fTuchDist Then
'        CircleVectorIntersect pTuchPt, fTuchDist, ptLine, fAxis, pPtTmp
    If CircleVectorIntersect(pTuchPt, fTuchDist, pResPt, fAxis, pPtTmp) > 0# Then
        pResPt.PutCoords pPtTmp.X, pPtTmp.y
        j = 0
        i = i + 1
    Else
        Set pConstructPoint = pPtTmp
        pConstructPoint.ConstructAngleIntersection pResPt, DegToRad(fAxis), pTuchPt, DegToRad(fAxis + 90#)
        pResPt.PutCoords pPtTmp.X, pPtTmp.y
        j = j + 1
    End If

'DrawPoint pResPt, 255
    Set pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, esriNoExtension)
'DrawPoint pTuchPt, 0
'DrawPolygon CreatePrjCircle(pTuchPt, fTuchDist)

    If (i > 10) Or (j > 5) Then
        Set FindCommonTochCircle = pTuchPt
        Exit Function
    End If

    fdX = pTuchPt.X - pResPt.X
    fdY = pTuchPt.y - pResPt.y
    fDist = Sqr(fdX * fdX + fdY * fdY)
Loop

Set FindCommonTochCircle = pTuchPt

End Function

Public Function SideDef(PtInLine As IPoint, LineAngle As Double, PtOutLine As IPoint) As Long
Dim Angle12 As Double
Dim dAngle As Double

Angle12 = ReturnAngleAsDegree(PtInLine, PtOutLine)
dAngle = LineAngle - Angle12
dAngle = Modulus(dAngle, 360#)

If (dAngle = 0#) Or (dAngle = 180#) Then
    SideDef = 0
    Exit Function
End If

If (dAngle < 180#) Then
    SideDef = 1
Else
    SideDef = -1
End If

End Function

Public Function SideFrom2Angle(ByVal Angle0 As Double, ByVal Angle1 As Double) As Long
Dim dAngle As Double

dAngle = SubtractAngles(Angle0, Angle1)

If (180# - dAngle < degEps) Or (dAngle < degEps) Then
    SideFrom2Angle = 0
    Exit Function
End If

dAngle = Modulus(Angle1 - Angle0, 360#)

If (dAngle < 180#) Then
    SideFrom2Angle = 1
Else
    SideFrom2Angle = -1
End If

End Function

Function AnglesSideDef(ByVal X As Double, ByVal y As Double) As Long
Dim Z As Double
    Z = Modulus(X - y, 360#)
    If Z = 0# Then
        AnglesSideDef = 0
    ElseIf Z > 180# Then
        AnglesSideDef = -1
    ElseIf Z < 180# Then
        AnglesSideDef = 1
    Else
        AnglesSideDef = 2
    End If
End Function

Sub CreateWindSpiral(pt As IPoint, aztNom As Double, AztSt As Double, AztEnd As Double, _
                    r0 As Double, coef As Double, _
                    turnDir As Long, pPointCollection As IPointCollection)
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

Set ptCnt = PointAlongPlane(pt, aztNom + 90# * turnDir, r0)
'DrawPoint ptCnt, 0
'DrawPoint Pt, 255

If SubtractAngles(aztNom, AztEnd) < degEps Then
    AztEnd = aztNom
End If

dPhi0 = (AztSt - aztNom) * turnDir
dPhi0 = Modulus(dPhi0, 360#)

If (dPhi0 < 0.001) Then
    dPhi0 = 0#
Else
    dPhi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, turnDir)
End If

'DrawPolygon pPointCollection, 0

dPhi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, turnDir)

TurnAng = dPhi - dPhi0

azt0 = aztNom + (dPhi0 - 90#) * turnDir
azt0 = Modulus(azt0, 360#)

If (TurnAng < 0#) Then
    Exit Sub
End If

dAlpha = 1#
N = TurnAng / dAlpha
If (N < 10) Then
    N = 10
End If
    
dAlpha = TurnAng / N

Set ptCur = New Point
For i = 0 To N
    r = r0 + (dAlpha * coef * i) + dPhi0 * coef
    Set ptCur = PointAlongPlane(ptCnt, azt0 + (i * dAlpha) * turnDir, r)
'    DrawPoint ptCur, 233

    pPointCollection.AddPoint ptCur
Next i
'Dim pCurve As ICurve
'Dim pPolyLine As IPointCollection
'Set pPolyLine = New Polyline
'
'Set pPolyLine = pPointCollection
'Set pCurve = pPolyLine

End Sub

Public Function RayPolylineIntersect(ByVal pPolyLine As Polyline, ByVal RayPt As Point, ByVal RayDir As Double, InterPt As IPoint) As Boolean
Dim i As Long
Dim N As Long
Dim d As Double
Dim dMin As Double
Dim pLine As IPolyline
Dim pPoints As IPointCollection
Dim pTopo As ITopologicalOperator

Set pLine = New Polyline
pLine.FromPoint = RayPt
dMin = 5000000#
pLine.ToPoint = PointAlongPlane(RayPt, RayDir, dMin)

Set pTopo = pPolyLine
Set pPoints = pTopo.Intersect(pLine, esriGeometry0Dimension)
N = pPoints.PointCount

RayPolylineIntersect = N > 0

If N = 0 Then
    Exit Function
End If

If N = 1 Then
    Set InterPt = pPoints.Point(0)
Else
    For i = 0 To N - 1
        d = ReturnDistanceAsMeter(RayPt, pPoints.Point(i))
        If d < dMin Then
            dMin = d
            Set InterPt = pPoints.Point(i)
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

Public Function DrawPointWithText(pPoint As Point, sText As String, Optional Color As Long = -1, Optional drawFlg As Boolean = True) As IElement
Dim pRGB As IRgbColor
Dim pMarkerShpElement As IMarkerElement
Dim pElementofpPoint As IElement
Dim pMarkerSym As ISimpleMarkerSymbol

Dim pTextElement As ITextElement
Dim pElementOfText As IElement
Dim pGroupElement As IGroupElement
Dim pCommonElement As IElement
Dim pTextSymbol As ITextSymbol

Set pTextElement = New TextElement
Set pElementOfText = pTextElement

Set pTextSymbol = New TextSymbol
pTextSymbol.HorizontalAlignment = esriTHALeft
pTextSymbol.VerticalAlignment = esriTVABottom

pTextElement.Text = sText
pTextElement.ScaleText = False
pTextElement.symbol = pTextSymbol

pElementOfText.Geometry = pPoint
  
Set pMarkerShpElement = New MarkerElement
 
Set pElementofpPoint = pMarkerShpElement
pElementofpPoint.Geometry = pPoint
 
Set pRGB = New RgbColor
If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If
 
Set pMarkerSym = New SimpleMarkerSymbol
pMarkerSym.Color = pRGB
pMarkerSym.size = 6
pMarkerShpElement.symbol = pMarkerSym

Set pGroupElement = New GroupElement
pGroupElement.AddElement pElementofpPoint
pGroupElement.AddElement pTextElement

Set pCommonElement = pGroupElement
Set DrawPointWithText = pCommonElement

    If drawFlg Then
        CurrentActiveView.GraphicsContainer.AddElement pCommonElement, 0
        Refresh_PartialActiveView
    End If
    
End Function

Public Function DrawPoint(pPoint As Point, Optional Color As Long = -1, Optional drawFlg As Boolean = True) As IElement
Dim pMarkerShpElement As IMarkerElement
Dim pElementofpPoint As IElement
Dim pMarkerSym As ISimpleMarkerSymbol
Dim pRGB As IRgbColor

Set pMarkerShpElement = New MarkerElement

Set pElementofpPoint = pMarkerShpElement
pElementofpPoint.Geometry = pPoint

Set pRGB = New RgbColor
If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If

Set pMarkerSym = New SimpleMarkerSymbol
pMarkerSym.Color = pRGB
pMarkerSym.size = 8
pMarkerShpElement.symbol = pMarkerSym

Set DrawPoint = pElementofpPoint

    If drawFlg Then
        CurrentActiveView.GraphicsContainer.AddElement pElementofpPoint, 0
        Refresh_PartialActiveView
    End If
End Function

Function DrawLine(pLine As esriGeometry.Line, Optional Color As Long = -1, Optional width As Double = 1#, Optional drawFlg As Boolean = True) As IElement
Dim pLineElement As ILineElement
Dim pElementOfpLine As IElement
Dim pLineSym As ISimpleLineSymbol
Dim pRGB As IRgbColor
'Dim pGeometry As IGeometry

Set pLineElement = New LineElement
Set pElementOfpLine = pLineElement
'Set pGeometry = pLine

pElementOfpLine.Geometry = pLine

Set pRGB = New RgbColor
Set pLineSym = New SimpleLineSymbol

If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If

pLineSym.Color = pRGB
pLineSym.Style = esriSLSSolid
pLineSym.width = width

pLineElement.symbol = pLineSym
Set DrawLine = pElementOfpLine

    If drawFlg Then
        CurrentActiveView.GraphicsContainer.AddElement pElementOfpLine, 0
        Refresh_PartialActiveView
    End If

End Function

Function DrawLayoutLine(pLine As esriGeometry.Line, Optional Color As Long = -1, Optional width As Double = 1#, Optional drawFlg As Boolean = True) As IElement

Dim pLineElement As ILineElement
Dim pElementOfpLine As IElement
Dim pLineSym As ISimpleLineSymbol
Dim pRGB As IRgbColor
'Dim pGeometry As IGeometry

Set pLineElement = New LineElement
Set pElementOfpLine = pLineElement
'Set pGeometry = pLine

'Dim pPolyLine As IPolyline
'pPolyLine.FromPoint =

pElementOfpLine.Geometry = pLine

Set pRGB = New RgbColor
Set pLineSym = New SimpleLineSymbol

If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If

pLineSym.Color = pRGB
pLineSym.Style = esriSLSSolid
pLineSym.width = width

pLineElement.symbol = pLineSym
Set DrawLayoutLine = pElementOfpLine

If drawFlg Then
    Dim pGraphics As IGraphicsContainer
    Set pGraphics = pDocument.PageLayout '.ActivatedView.GraphicsContainer
    pGraphics.AddElement pElementOfpLine, 0
    pDocument.ActiveView.PartialRefresh esriViewGraphics, Nothing, Nothing
End If

End Function

Public Function DrawPolyLine(pLine As Polyline, Optional Color As Long = -1, Optional width As Double = 1#, Optional drawFlg As Boolean = True) As IElement

Dim pLineElement As ILineElement
Dim pElementOfpLine As IElement
Dim pLineSym As ISimpleLineSymbol
Dim pRGB As IRgbColor

Set pRGB = New RgbColor


Set pLineSym = New SimpleLineSymbol

If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If

pLineSym.Color = pRGB
pLineSym.Style = esriSLSSolid
pLineSym.width = width

Set pLineElement = New LineElement

Set pElementOfpLine = pLineElement
pElementOfpLine.Geometry = pLine

pLineElement.symbol = pLineSym
Set DrawPolyLine = pElementOfpLine

    If drawFlg Then
        CurrentActiveView.GraphicsContainer.AddElement pElementOfpLine, 0
        Refresh_PartialActiveView
    End If
    
End Function

Public Function DrawPolygon(pPolygon As Polygon, Optional Color As Long = -1, Optional drawFlg As Boolean = True) As IElement

Dim pRGB As IRgbColor
Dim pFillSym As ISimpleFillSymbol
Dim pFillShpElement As IFillShapeElement
Dim pLineSimbol As ILineSymbol

Dim pElementofPoly As IElement

Set pRGB = New RgbColor
Set pFillSym = New SimpleFillSymbol
Set pFillShpElement = New PolygonElement

Set pElementofPoly = pFillShpElement
pElementofPoly.Geometry = pPolygon

If Color <> -1 Then
    pRGB.RGB = Color
Else
    pRGB.Red = Round(255 * Rnd)
    pRGB.Green = Round(255 * Rnd)
    pRGB.Blue = Round(255 * Rnd)
End If

pFillSym.Color = pRGB
pFillSym.Style = esriSFSNull 'esriSFSDiagonalCross

Set pLineSimbol = New SimpleLineSymbol
pLineSimbol.Color = pRGB
pLineSimbol.width = 1
pFillSym.Outline = pLineSimbol

pFillShpElement.symbol = pFillSym
Set DrawPolygon = pElementofPoly

    If drawFlg Then
        CurrentActiveView.GraphicsContainer.AddElement pElementofPoly, 0
        Refresh_PartialActiveView
    End If

End Function

Function DrawSectorPrj(ptCnt As IPoint, r As Double, stDir As Double, _
        endDir As Double, ClWise As Long) As IPointCollection
'Function DrawSectorPrj(PtCnt As IPoint, ptFrom As IPoint, _
'            ptTo As IPoint, ClWise As Long) As IPointCollection
Dim ptFrom As IPoint
Dim ptTo As IPoint
    Set ptFrom = PointAlongPlane(ptCnt, stDir, r)
    Set ptTo = PointAlongPlane(ptCnt, endDir, r)
    Set DrawSectorPrj = CreateArcPrj(ptCnt, ptFrom, ptTo, ClWise)
    DrawSectorPrj.AddPoint ptCnt
    DrawSectorPrj.AddPoint ptCnt, 0
End Function

'''Public Sub ClearScr()
'''Dim pGraphics As IGraphicsContainer
'''Dim pDoc As IMxDocument
'''Dim pElement As IElement
'''Set pDoc = Application.Document
'''Set pGraphics = pDoc.ActivatedView.GraphicsContainer
'''
'''On Error GoTo Err
'''
'''pGraphics.Reset
'''Set pElement = pGraphics.Next
'''
'''While Not pElement Is Nothing
'''    If pElement.Locked Then pGraphics.DeleteElement pElement
'''
''''    If (pElement.Geometry.GeometryType = esriGeometryPoint) Or _
''''        (pElement.Geometry.GeometryType = esriGeometryPolygon) Or _
''''        (pElement.Geometry.GeometryType = esriGeometryPolyline) Then
''''        pGraphics.DeleteElement pElement
''''    End If
'''    Set pElement = pGraphics.Next
'''Wend
'''
'''pDoc.ActivatedView.Refresh
'''Exit Sub
'''
'''Err:
'''On Error GoTo 0
'''End Sub

Public Sub GetIntData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara As Long, ByVal size As Long)
Dim i As Long
Dim E As Double
Dim Sign As Long
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

Public Sub GetData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara, ByVal size As Long)
Dim i As Long
ReDim Vara(size)
    For i = 0 To size - 1
        Vara(i) = Data(index + i)
    Next i
    index = index + size
End Sub

Public Sub GetDoubleData(ByRef Data() As Byte, ByRef index As Long, ByRef Vara As Double, ByVal size As Long)
Dim i As Long
Dim Sign As Long

Dim mantissa As Double
Dim exponent As Long
Dim lTmp As Long

    mantissa = 0
    exponent = 0

    For i = 0 To size - 3
        mantissa = Data(index + i) + mantissa / 256#
    Next i

    mantissa = (((Data(index + size - 2) And 15) + 16) + mantissa / 256#) / 16#

    exponent = (Data(index + size - 2) And 240) / 16#
    exponent = Data(index + size - 1) * 16# + exponent

    If mantissa = 1 And exponent = 0 Then
        Vara = 0#
        index = index + size
        Exit Sub
    End If

    Sign = exponent And 2048
    exponent = (exponent And 2047) - 1023
    If exponent > 0 Then
        For i = 1 To exponent
            mantissa = mantissa * 2#
        Next i
    ElseIf exponent < 0 Then
        For i = -1 To exponent Step -1
            mantissa = mantissa / 2#
        Next i
    End If

    If Sign <> 0 Then mantissa = -mantissa
    Vara = mantissa
    index = index + size
End Sub

Sub CheckRepidNameInput(LayerName As String, ItemName As String, _
                        FieldName As String, CheckN As Boolean, _
                        Optional NavType As String = "", _
                        Optional CheckNav As Boolean = False)

Dim pFeatureLayer As IFeatureLayer
Dim pFeatureClass As IFeatureClass
Dim pTable As ITable
Dim i As Long
Dim N As Long
Dim pFieldName As Long
Dim pFieldType As Long
Dim bFlag As Boolean

Dim pFilter As IQueryFilter
Dim pCursor As ICursor
Dim pRow As IRow

For i = 0 To pMap.LayerCount - 1
    If pMap.Layer(i).Name = LayerName Then
        Set pFeatureLayer = pMap.Layer(i)
        bFlag = True
        Exit For
    End If
Next i

If Not bFlag Then
    MsgDialog.ShowMessage LayerName + "Theme Not Found"
    Exit Sub
End If

Set pFeatureClass = pFeatureLayer.FeatureClass
Set pTable = pFeatureClass

Set pFilter = New QueryFilter
pFilter.WhereClause = pTable.OIDFieldName + ">=0"
N = pTable.rowCount(pFilter)

Set pCursor = pTable.Search(pFilter, True)
pFieldName = pCursor.FindField(FieldName)

CheckN = False

Set pRow = pCursor.NextRow
Do While Not pRow Is Nothing
    If (pRow.value(pFieldName) = ItemName) Then
        If CheckNav Then
            pFieldType = pCursor.FindField("Type")
            If (pRow.value(pFieldType) = NavType) Then
                CheckN = True
                MsgDialog.ShowMessage "Объект с данным названием уже существует в базе данных." + _
                "Измените " + FieldName + " объекта.", vbExclamation
                Set pFilter = Nothing
                Exit Sub
            End If
        Else
            CheckN = True
            MsgDialog.ShowMessage "Объект с данным названием уже существует в базе данных." + _
            "Измените " + FieldName + " объекта.", vbExclamation
            Set pFilter = Nothing
            Exit Sub
        End If
    End If
    Set pRow = pCursor.NextRow
Loop
Set pFilter = Nothing

End Sub

Function DegToRad(ByVal X As Double) As Double
    DegToRad = X * DegToRadValue
End Function

Function RadToDeg(ByVal X As Double) As Double
    RadToDeg = X * RadToDegValue
End Function

Function OpenTableFromFile(pTable As ITable, sFolderName As String, sFileName As String) As Boolean
On Error GoTo EH

Dim pFact As IWorkspaceFactory
Dim pWorkspace As IWorkspace
Dim pFeatWs As IFeatureWorkspace

Dim plDocument As IDocument
Dim sPath As String
Dim L As Long
Dim Pos As Long

OpenTableFromFile = True
sPath = sFolderName

Set pFact = New ShapefileWorkspaceFactory
Set pWorkspace = pFact.OpenFromFile(sPath, 0)
Set pFeatWs = pWorkspace
Set pTable = pFeatWs.OpenTable(sFileName)

Exit Function

EH:
    OpenTableFromFile = False
    ErrorStr = Err.Number & "  " & Err.Description
    'MsgDialog.ShowMessage Err.Number & "  " & Err.Description
End Function

'Sub SortIntervals(Intervals() As Interval, Optional RightSide = False)
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim Tmp As Interval
'
'N = UBound(Intervals)
'
'For I = 0 To N - 1
'    For J = I + 1 To N
'        If RightSide Then
'            If Intervals(I).Right > Intervals(J).Right Then
'                Tmp = Intervals(I)
'                Intervals(I) = Intervals(J)
'                Intervals(J) = Tmp
'            End If
'        Else
'            If Intervals(I).Left > Intervals(J).Left Then
'                Tmp = Intervals(I)
'                Intervals(I) = Intervals(J)
'                Intervals(J) = Tmp
'            End If
'        End If
'    Next J
'Next I
'
'End Sub
'
'Private Sub SortByDist(A() As ObstacleAr, iLo As Integer, iHi As Integer)
'Dim T As ObstacleAr
'Dim Mid As Double
'Dim Lo As Integer
'Dim Hi As Integer
'
'    Lo = iLo
'    Hi = iHi
'    Mid = A((Lo + Hi) / 2).Dist
'    Do
'      While A(Lo).Dist < Mid
'        Lo = Lo + 1
'      Wend
'      While A(Hi).Dist > Mid
'         Hi = Hi - 1
'      Wend
'      If (Lo <= Hi) Then
'        T = A(Lo)
'        A(Lo) = A(Hi)
'        A(Hi) = T
'        Lo = Lo + 1
'        Hi = Hi - 1
'      End If
'    Loop Until Lo > Hi
'
'    If (Hi > iLo) Then SortByDist A, iLo, Hi
'    If (Lo < iHi) Then SortByDist A, Lo, iHi
'End Sub
'
'Private Sub SortByTurnDist(A() As ObstacleAr, iLo As Integer, iHi As Integer)
'Dim T As ObstacleAr
'Dim Mid As Double
'Dim Lo As Integer
'Dim Hi As Integer
'
'    Lo = iLo
'    Hi = iHi
'    Mid = A((Lo + Hi) / 2).TurnDistL
'
'    Do
'      While A(Lo).TurnDistL < Mid
'        Lo = Lo + 1
'      Wend
'      While A(Hi).TurnDistL > Mid
'         Hi = Hi - 1
'      Wend
'      If (Lo <= Hi) Then
'        T = A(Lo)
'        A(Lo) = A(Hi)
'        A(Hi) = T
'        Lo = Lo + 1
'        Hi = Hi - 1
'      End If
'    Loop Until Lo > Hi
'
'    If (Hi > iLo) Then SortByTurnDist A, iLo, Hi
'    If (Lo < iHi) Then SortByTurnDist A, Lo, iHi
'End Sub
'
'Private Sub SortByReqH(A() As ObstacleAr, iLo As Integer, iHi As Integer)
'Dim T As ObstacleAr
'Dim Mid As Double
'Dim Lo As Integer
'Dim Hi As Integer
'
'    Lo = iLo
'    Hi = iHi
'    Mid = A((Lo + Hi) / 2).ReqH
'
'    Do
'      While A(Lo).ReqH > Mid
'        Lo = Lo + 1
'      Wend
'      While A(Hi).ReqH < Mid
'         Hi = Hi - 1
'      Wend
'      If (Lo <= Hi) Then
'        T = A(Lo)
'        A(Lo) = A(Hi)
'        A(Hi) = T
'        Lo = Lo + 1
'        Hi = Hi - 1
'      End If
'    Loop Until Lo > Hi
'
'    If (Hi > iLo) Then SortByReqH A, iLo, Hi
'    If (Lo < iHi) Then SortByReqH A, Lo, iHi
'End Sub
'
'Private Sub SortByfSort(A() As ObstacleAr, iLo As Integer, iHi As Integer)
'Dim T As ObstacleAr
'Dim Mid As Double
'Dim Lo As Integer
'Dim Hi As Integer
'
'    Lo = iLo
'    Hi = iHi
'    Mid = A((Lo + Hi) / 2).fSort
'
'    Do
'      While A(Lo).fSort > Mid
'        Lo = Lo + 1
'      Wend
'      While A(Hi).fSort < Mid
'         Hi = Hi - 1
'      Wend
'      If (Lo <= Hi) Then
'        T = A(Lo)
'        A(Lo) = A(Hi)
'        A(Hi) = T
'        Lo = Lo + 1
'        Hi = Hi - 1
'      End If
'    Loop Until Lo > Hi
'
'    If (Hi > iLo) Then SortByfSort A, iLo, Hi
'    If (Lo < iHi) Then SortByfSort A, Lo, iHi
'End Sub
'
'Public Sub Sort(A() As ObstacleAr, SortIx As Long)
'
'Dim Lo As Integer
'Dim Hi As Integer
'
'Lo = LBound(A)
'Hi = UBound(A)
'
'If (Lo >= Hi) Then Exit Sub
'
'Select Case SortIx
'Case 0
'    SortByDist A, Lo, Hi
'Case 1
'    SortByTurnDist A, Lo, Hi
'Case 2
'    SortByReqH A, Lo, Hi
'Case 100
'    SortByfSort A, Lo, Hi
'End Select
'
'End Sub
'
'Public Sub shall_SortfSort(obsArray() As ObstacleAr)
'Dim TempVal As ObstacleAr
'Dim I As Long, GapSize As Long, CurPos As Long
'Dim FirstRow As Long, LastRow As Long, NumRows As Long
'
'    FirstRow = LBound(obsArray)
'    LastRow = UBound(obsArray)
'    NumRows = LastRow - FirstRow + 1
'
'    Do
'        GapSize = GapSize * 3 + 1
'    Loop Until GapSize > NumRows
'
'    Do
'        GapSize = GapSize \ 3
'        For I = (GapSize + FirstRow) To LastRow
'            CurPos = I
'            TempVal = obsArray(I)
'            Do While obsArray(CurPos - GapSize).fSort > TempVal.fSort
'                obsArray(CurPos) = obsArray(CurPos - GapSize)
'                CurPos = CurPos - GapSize
'                If (CurPos - GapSize) < FirstRow Then Exit Do
'            Loop
'            obsArray(CurPos) = TempVal
'        Next I
'    Loop Until GapSize = 1
'End Sub
'
'Public Sub shall_SortfSortD(obsArray() As ObstacleAr)
'Dim TempVal As ObstacleAr
'Dim I As Long, GapSize As Long, CurPos As Long
'Dim FirstRow As Long, LastRow As Long, NumRows As Long
'
'    FirstRow = LBound(obsArray)
'    LastRow = UBound(obsArray)
'    NumRows = LastRow - FirstRow + 1
'
'    Do
'        GapSize = GapSize * 3 + 1
'    Loop Until GapSize > NumRows
'
'    Do
'        GapSize = GapSize \ 3
'        For I = (GapSize + FirstRow) To LastRow
'            CurPos = I
'            TempVal = obsArray(I)
'            Do While obsArray(CurPos - GapSize).fSort < TempVal.fSort
'                obsArray(CurPos) = obsArray(CurPos - GapSize)
'                CurPos = CurPos - GapSize
'                If (CurPos - GapSize) < FirstRow Then Exit Do
'            Loop
'            obsArray(CurPos) = TempVal
'        Next I
'    Loop Until GapSize = 1
'End Sub
'
'Public Sub shall_SortsSort(obsArray() As ObstacleAr)
'Dim TempVal As ObstacleAr
'Dim I As Long, GapSize As Long, CurPos As Long
'Dim FirstRow As Long, LastRow As Long, NumRows As Long
'
'    FirstRow = LBound(obsArray)
'    LastRow = UBound(obsArray)
'    NumRows = LastRow - FirstRow + 1
'
'    Do
'        GapSize = GapSize * 3 + 1
'    Loop Until GapSize > NumRows
'
'    Do
'        GapSize = GapSize \ 3
'        For I = (GapSize + FirstRow) To LastRow
'            CurPos = I
'            TempVal = obsArray(I)
'            Do While obsArray(CurPos - GapSize).sSort > TempVal.sSort
'                obsArray(CurPos) = obsArray(CurPos - GapSize)
'                CurPos = CurPos - GapSize
'                If (CurPos - GapSize) < FirstRow Then Exit Do
'            Loop
'            obsArray(CurPos) = TempVal
'        Next I
'    Loop Until GapSize = 1
'End Sub
'
'Public Sub shall_SortsSortD(obsArray() As ObstacleAr)
'Dim TempVal As ObstacleAr
'Dim I As Long, GapSize As Long, CurPos As Long
'Dim FirstRow As Long, LastRow As Long, NumRows As Long
'
'    FirstRow = LBound(obsArray)
'    LastRow = UBound(obsArray)
'    NumRows = LastRow - FirstRow + 1
'
'    Do
'        GapSize = GapSize * 3 + 1
'    Loop Until GapSize > NumRows
'
'    Do
'        GapSize = GapSize \ 3
'        For I = (GapSize + FirstRow) To LastRow
'            CurPos = I
'            TempVal = obsArray(I)
'            Do While obsArray(CurPos - GapSize).sSort < TempVal.sSort
'                obsArray(CurPos) = obsArray(CurPos - GapSize)
'                CurPos = CurPos - GapSize
'                If (CurPos - GapSize) < FirstRow Then Exit Do
'            Loop
'            obsArray(CurPos) = TempVal
'        Next I
'    Loop Until GapSize = 1
'End Sub

'Public Sub ShellSort(ByRef Arr() As Double, ByVal N As Long)
'Dim C As Boolean
'Dim E As Long
'Dim G As Long
'Dim I As Long
'Dim J As Long
'Dim Tmp As Double
'
'    N = UBound(Arr)
'    G = (N + 1) \ 2
'    Do
'        I = G
'        Do
'            J = I - G
'            C = True
'            Do
'                If Arr(J) <= Arr(J + G) Then
'                    C = False
'                Else
'                    Tmp = Arr(J)
'                    Arr(J) = Arr(J + G)
'                    Arr(J + G) = Tmp
'                End If
'                J = J - 1
'            Loop Until Not (J >= 0 And C)
'            I = I + 1
'        Loop Until Not I <= N
'        G = G \ 2
'    Loop Until Not G > 0
'End Sub
'
Public Function Point2LineDistancePrj(ByVal pt As IPoint, ByVal ptLine As IPoint, ByVal Azt As Double) As Double
Dim cosx, siny As Double
Azt = DegToRad(Azt)
cosx = Cos(Azt)
siny = Sin(Azt)
Point2LineDistancePrj = Abs((pt.y - ptLine.y) * cosx - (pt.X - ptLine.X) * siny)
'k = 0
End Function

'Sub GetObstInRange(ObstSource() As ObstacleAr, ObstDest() As ObstacleAr, Range As Double)
'Dim N As Long
'Dim I As Long
'Dim J As Long
'Dim PDG As Double
'
'    N = UBound(ObstSource)
'    If N >= 0 Then
'        ReDim ObstDest(N)
'    Else
'        ReDim ObstDest(-1 To -1)
'        Exit Sub
'    End If
'
'    J = -1
'    For I = 0 To N
'        If ObstSource(I).Dist > Range Then Exit For
'        J = J + 1
'        ObstDest(J) = ObstSource(I)
'    Next I
'
'    If J < 0 Then
'        ReDim ObstDest(-1 To -1)
'    Else
'        ReDim Preserve ObstDest(J)
'    End If
'
'End Sub

Public Function LineVectIntersect(Pt1 As IPoint, pt2 As IPoint, ptVect As IPoint, Azt As Double, ptRes As IPoint) As Long
Dim Az As Double
Dim SinAz As Double
Dim CosAz As Double
Dim UaDenom As Double
Dim UaNumer As Double
Dim Ua As Double

Az = DegToRad(Azt)
SinAz = Sin(Az)
CosAz = Cos(Az)

Set ptRes = New Point

UaDenom = SinAz * (pt2.X - Pt1.X) - CosAz * (pt2.y - Pt1.y)
If UaDenom = 0# Then
    LineVectIntersect = -2
    Exit Function
End If

UaNumer = CosAz * (Pt1.y - ptVect.y) - SinAz * (Pt1.X - ptVect.X)

Ua = UaNumer / UaDenom
If Ua < 0# Then
    LineVectIntersect = -1
ElseIf Ua > 1# Then
    LineVectIntersect = 1
Else
    LineVectIntersect = 0
End If

ptRes.PutCoords Pt1.X + Ua * (pt2.X - Pt1.X), Pt1.y + Ua * (pt2.y - Pt1.y)

End Function

Public Function FixToTouchSpiral(ptBase As IPoint, coef0 As Double, TurnR As Double, _
                                turnDir As Long, Theta As Double, FixPnt As IPoint, DepCourse As Double) As Double
Dim r As Double
Dim x1 As Double
Dim y1 As Double
Dim Theta0 As Double
Dim Theta1 As Double
Dim Theta11 As Double
Dim Theta12 As Double
Dim Theta21 As Double
Dim Theta22 As Double
Dim Theta1New As Double
Dim Theta2New As Double
Dim SinCoef As Double
Dim CosCoef As Double
Dim A As Double
Dim B As Double
Dim C As Double
Dim d As Double
Dim fTmp1 As Double
Dim fTmp2 As Double
Dim sin1 As Double
Dim sin2 As Double
Dim coef As Double
Dim Dist As Double
Dim FixTheta As Double
Dim i As Long
Dim PtCntSpiral As IPoint
Dim ptOut As IPoint
Dim dTheta As Double
Dim dThetaNew As Double
Dim CntTheta As Double
Dim SolFlag11 As Boolean
Dim SolFlag12 As Boolean
Dim SolFlag21 As Boolean
Dim SolFlag22 As Boolean

FixToTouchSpiral = -1000

coef = RadToDeg(coef0)
Theta0 = Modulus(90# * turnDir + DepCourse + 180#, 360#)
Set PtCntSpiral = PointAlongPlane(ptBase, DepCourse + 90# * turnDir, TurnR)
Dist = ReturnDistanceAsMeter(PtCntSpiral, FixPnt)
FixTheta = ReturnAngleAsDegree(PtCntSpiral, FixPnt)
dTheta = Modulus((FixTheta - Theta0) * turnDir, 360#)
r = TurnR + dTheta * coef0
If Dist < r Then
    Exit Function
End If

x1 = FixPnt.X - PtCntSpiral.X
y1 = FixPnt.y - PtCntSpiral.y
CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, turnDir)
CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
'===============================Variant Firdowsy ==================================
Dim F As Double
Dim F1 As Double
Dim SinT As Double
Dim CosT As Double

Theta1 = CntTheta
For i = 0 To 20
    dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
    SinT = Sin(DegToRad(Theta1))
    CosT = Cos(DegToRad(Theta1))
    r = TurnR + dTheta * coef0
    F = r * r - (y1 * r + x1 * coef * turnDir) * SinT - (x1 * r - y1 * coef * turnDir) * CosT
    F1 = 2 * r * coef * turnDir - (y1 * r + 2 * x1 * coef * turnDir) * CosT + _
        (x1 * r - 2 * y1 * coef * turnDir) * SinT
    Theta1New = Theta1 - RadToDeg(F / F1)
    
    fTmp1 = SubtractAngles(Theta1New, Theta1)
    If fTmp1 < 0.0001 Then
        Theta1 = Theta1New
        Exit For
    End If
    Theta1 = Theta1New
Next i

dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
r = TurnR + dTheta * coef0

Set ptOut = PointAlongPlane(PtCntSpiral, Theta1, r)
fTmp1 = ReturnAngleAsDegree(ptOut, FixPnt)
CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, turnDir)
CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
fTmp2 = SubtractAngles(CntTheta, Theta1)

If fTmp2 < 0.0001 Then
    FixToTouchSpiral = fTmp1
    Exit Function
End If

Exit Function
'=============================11==================================
Theta1 = CntTheta
SolFlag11 = False

For i = 0 To 20
    dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
    'SinCoef*SinX+CosCoef*CosX = 1
    r = TurnR + dTheta * coef0
    SinCoef = (y1 + turnDir * coef * x1 / r) / r
    CosCoef = (x1 - turnDir * coef * y1 / r) / r
    'a*x2 + b*x + c = 0
    A = SinCoef * SinCoef + CosCoef * CosCoef
    B = -2 * SinCoef
    C = 1 - CosCoef * CosCoef
    d = B * B - 4# * A * C
    If d < 0# Then
        Theta1New = -d * Sgn(-B * A) * 90#
    Else
        sin1 = (-B + Sqr(d)) / (2# * A)
        Theta1New = RadToDeg(ArcSin(sin1))
    End If

    fTmp1 = SubtractAngles(Theta1New, Theta1)
    If fTmp1 < 0.0001 Then
        SolFlag11 = True
        Theta11 = Theta1
        Exit For
    End If
    Theta1 = Theta1New
Next i
'=============================12==================================
Theta1 = CntTheta
SolFlag12 = False

For i = 0 To 20
    dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
    'SinCoef*SinX+CosCoef*CosX = 1
    r = TurnR + dTheta * coef0
    SinCoef = (y1 + turnDir * coef * x1 / r) / r
    CosCoef = (x1 - turnDir * coef * y1 / r) / r
    'a*x2 + b*x + c = 0
    A = SinCoef * SinCoef + CosCoef * CosCoef
    B = -2 * SinCoef
    C = 1 - CosCoef * CosCoef
    d = B * B - 4# * A * C
    If d < 0# Then
        Theta1New = 180# + d * Sgn(-B * A) * 90#
    Else
        sin1 = (-B + Sqr(d)) / (2# * A)
        Theta1New = 180# - RadToDeg(ArcSin(sin1))
    End If

    fTmp1 = SubtractAngles(Theta1New, Theta1)
    If fTmp1 < 0.0001 Then
        SolFlag12 = True
        Theta12 = Theta1
        Exit For
    End If
    Theta1 = Theta1New
Next i
'=============================21==================================
Theta1 = CntTheta
SolFlag21 = False

For i = 0 To 20
    dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
    'SinCoef*SinX+CosCoef*CosX = 1
    r = TurnR + dTheta * coef0
    SinCoef = (y1 + turnDir * coef * x1 / r) / r
    CosCoef = (x1 - turnDir * coef * y1 / r) / r
    'a*x2 + b*x + c = 0
    A = SinCoef * SinCoef + CosCoef * CosCoef
    B = -2 * SinCoef
    C = 1 - CosCoef * CosCoef
    d = B * B - 4# * A * C
    If d < 0# Then
        Theta1New = -d * Sgn(-B * A) * 90#
    Else
        sin1 = (-B - Sqr(d)) / (2# * A)
        Theta1New = RadToDeg(ArcSin(sin1))
    End If

    fTmp1 = SubtractAngles(Theta1New, Theta1)
    If fTmp1 < 0.0001 Then
        SolFlag21 = True
        Theta21 = Theta1
        Exit For
    End If
    Theta1 = Theta1New
Next i
'=============================22==================================
Theta1 = CntTheta + 180#
SolFlag22 = False

For i = 0 To 20
    dTheta = Modulus((Theta1 - Theta0) * turnDir, 360#)
    'SinCoef*SinX+CosCoef*CosX = 1
    r = TurnR + dTheta * coef0
    SinCoef = (y1 + turnDir * coef * x1 / r) / r
    CosCoef = (x1 - turnDir * coef * y1 / r) / r
    'a*x2 + b*x + c = 0
    A = SinCoef * SinCoef + CosCoef * CosCoef
    B = -2 * SinCoef
    C = 1 - CosCoef * CosCoef
    d = B * B - 4# * A * C
    If d < 0# Then
        Theta1New = 180# + d * Sgn(-B * A) * 90#
    Else
        sin1 = (-B - Sqr(d)) / (2# * A)
        Theta1New = 180# - RadToDeg(ArcSin(sin1))
    End If
    
    fTmp1 = SubtractAngles(Theta1New, Theta1)
    If fTmp1 < 0.0001 Then
        SolFlag22 = True
        Theta22 = Theta1
        Exit For
    End If
    Theta1 = Theta1New
Next i
'================================11=====================================
If SolFlag11 Then
    dTheta = Modulus((Theta11 - Theta0) * turnDir, 360#)
    r = TurnR + dTheta * coef0
    Set ptOut = PointAlongPlane(PtCntSpiral, Theta11, r)
    fTmp1 = ReturnAngleAsDegree(ptOut, FixPnt)
    CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, turnDir)
    CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
    fTmp2 = SubtractAngles(CntTheta, Theta11)
    If fTmp2 < 0.0001 Then
        FixToTouchSpiral = fTmp1
        Exit Function
    End If
End If
'================================12=====================================
If SolFlag12 Then
    dTheta = Modulus((Theta12 - Theta0) * turnDir, 360#)
    r = TurnR + dTheta * coef0
    Set ptOut = PointAlongPlane(PtCntSpiral, Theta12, r)
    fTmp1 = ReturnAngleAsDegree(ptOut, FixPnt)
    CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, turnDir)
    CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
    fTmp2 = SubtractAngles(CntTheta, Theta12)
    If fTmp2 < 0.0001 Then
        FixToTouchSpiral = fTmp1
        Exit Function
    End If
End If
'================================21=====================================
If SolFlag21 Then
    dTheta = Modulus((Theta21 - Theta0) * turnDir, 360#)
    r = TurnR + dTheta * coef0
    Set ptOut = PointAlongPlane(PtCntSpiral, Theta21, r)
    fTmp1 = ReturnAngleAsDegree(ptOut, FixPnt)
    CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, turnDir)
    CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
    fTmp2 = SubtractAngles(CntTheta, Theta21)
    If fTmp2 < 0.0001 Then
        FixToTouchSpiral = fTmp1
        Exit Function
    End If
End If
'================================22=====================================
If SolFlag22 Then
    dTheta = Modulus((Theta22 - Theta0) * turnDir, 360#)
    r = TurnR + dTheta * coef0
    Set ptOut = PointAlongPlane(PtCntSpiral, Theta22, r)
    fTmp1 = ReturnAngleAsDegree(ptOut, FixPnt)
    CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, turnDir)
    CntTheta = Modulus(Theta0 + CntTheta * turnDir, 360#)
    fTmp2 = SubtractAngles(CntTheta, Theta22)
    If fTmp2 < 0.0001 Then
        FixToTouchSpiral = fTmp1
        Exit Function
    End If
End If

End Function

'Sub LowHighSort(A() As LowHigh, Optional Descent = False)
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim Tmp As LowHigh
'
'N = UBound(A)
'
'If Descent Then
'    For I = 0 To N - 1
'        For J = I + 1 To N
'            If A(I).High > A(J).High Then
'                Tmp = A(I)
'                A(I) = A(J)
'                A(J) = Tmp
'            End If
'        Next J
'    Next I
'Else
'    For I = 0 To N - 1
'        For J = I + 1 To N
'            If A(I).Low > A(J).Low Then
'                Tmp = A(I)
'                A(I) = A(J)
'                A(J) = Tmp
'            End If
'        Next J
'    Next I
'End If
'
'End Sub
'
'Public Function LowHighIntersection(A As LowHigh, B As LowHigh, C As LowHigh) As Long
'
'    If (A.Low > B.High) Or (A.High < B.Low) Then
'        LowHighIntersection = 0
'    Else
'        If A.High < B.High Then
'            C.High = A.High
'        Else
'            C.High = B.High
'        End If
'
'        If A.Low > B.Low Then
'            C.Low = A.Low
'        Else
'            C.Low = B.Low
'        End If
'        LowHighIntersection = 1
'    End If
'
'End Function
'
'Public Function LowHighDifference(A As LowHigh, B As LowHigh) As LowHigh()
'Dim Res() As LowHigh
'
'ReDim Res(0)
'
'If (B.Low = B.High) Or (B.High < A.Low) Or (A.High < B.Low) Then
'    Res(0) = A
'ElseIf (A.Low < B.Low) And (A.High > B.High) Then
'    ReDim Res(1)
'    Res(0).Low = A.Low
'    Res(0).High = B.Low
'    Res(1).Low = B.High
'    Res(1).High = A.High
'ElseIf A.High > B.High Then
'    Res(0).Low = B.High
'    Res(0).High = A.High
'ElseIf (A.Low < B.Low) Then
'    Res(0).Low = A.Low
'    Res(0).High = B.Low
'Else
'    ReDim Res(-1 To -1)
'End If
'
'LowHighDifference = Res
'End Function
'
Public Function IntervalsDifference(A As Interval, B As Interval) As Interval()
    Dim Res() As Interval
    
    ReDim Res(0)
    
    If (B.Left = B.Right) Or (B.Right < A.Left) Or (A.Right < B.Left) Then
        Res(0) = A
    ElseIf (A.Left < B.Left) And (A.Right > B.Right) Then
        ReDim Res(1)
        Res(0).Left = A.Left
        Res(0).Right = B.Left
        Res(1).Left = B.Right
        Res(1).Right = A.Right
    ElseIf A.Right > B.Right Then
        Res(0).Left = B.Right
        Res(0).Right = A.Right
    ElseIf (A.Left < B.Left) Then
        Res(0).Left = A.Left
        Res(0).Right = B.Left
    Else
        ReDim Res(-1 To -1)
    End If
    
    IntervalsDifference = Res
End Function

'Private Function CalcNomPos(Xs As Double, Ys As Double, RefZ As Double, PDG As Double, OCH As Double, d0 As Double, AheadBehindSide As Long, NearSide As Long, ptDMEprj As IPoint, ptSOCPrj As IPoint) As Double
'Dim Rx As Double
'Dim dNomPosDer As Double
'Dim dNomPosDME As Double
'Dim dOldPosDME As Double
'Dim hMax As Double
'Dim I As Long
'
'    I = 0
'    dNomPosDME = d0 + NearSide * DME.MinimalError
'    hMax = 0#
'
'    Do
'        dNomPosDer = Xs + AheadBehindSide * Sqr(dNomPosDME * dNomPosDME - Ys * Ys)
'        hMax = dNomPosDer * PDG + OCH - ptDMEprj.Z + RefZ
'        dOldPosDME = dNomPosDME
'        dNomPosDME = (d0 + NearSide * DME.MinimalError) / (1# - NearSide * DME.ErrorScalingUp * Sqr(1# + hMax * hMax / (dNomPosDer * dNomPosDer)))
'
'        I = I + 1
'        If I > 5 Then Exit Do
'    Loop While Abs(dOldPosDME - dNomPosDME) > distEps
'
'    CalcNomPos = dNomPosDME
'End Function
'
'Private Function CalcDMERange(ByVal ptSOCPrj As IPoint, ByVal ptBasePrj As Point, _
'    ByVal NomDir As Double, ByVal RefZ As Double, ByVal PDG As Double, _
'    ByVal OCH As Double, ByVal ptDMEprj As IPoint, ByVal KKhMin As IPolyline, _
'    ByVal KKhMax As IPolyline) As Interval
'
'Dim Side As Long
'Dim d0 As Double
'Dim d1 As Double
'Dim Ys As Double
'Dim Xs As Double
'
'Dim dH As Double
'Dim Rx As Double
'Dim X As Double
'Dim Hpdg As Double
'Dim dMax As Double
'Dim dMin As Double
'Dim dist0 As Double
'Dim Dist1 As Double
'Dim LeftRightSide As Long
'Dim AheadBehindSide As Long
'Dim ptOnCirc As IPoint
'
'    AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90#, ptDMEprj)
'    LeftRightSide = SideDef(ptBasePrj, NomDir, ptDMEprj)
'
'    Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90#) * SideDef(ptSOCPrj, NomDir + 90#, ptDMEprj)
'    Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir)
'
'    If AheadBehindSide < 0 Then
'        If LeftRightSide > 0 Then
'            d0 = ReturnDistanceAsMeter(ptDMEprj, KKhMin.ToPoint)
'
'            Side = SideDef(KKhMax.FromPoint, NomDir, ptDMEprj)
'            If Side < 0 Then
'                d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.FromPoint, NomDir + 90#)
'            Else
'                d1 = ReturnDistanceAsMeter(ptDMEprj, KKhMax.FromPoint)
'            End If
'        Else
'            d0 = ReturnDistanceAsMeter(ptDMEprj, KKhMin.FromPoint)
'
'            Side = SideDef(KKhMax.ToPoint, NomDir, ptDMEprj)
'            If Side > 0 Then
'                d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.ToPoint, NomDir + 90#)
'            Else
'                d1 = ReturnDistanceAsMeter(ptDMEprj, KKhMax.ToPoint)
'            End If
'        End If
'    Else
'        If LeftRightSide > 0 Then
'            d0 = ReturnDistanceAsMeter(ptDMEprj, KKhMax.ToPoint)
'
'            Side = SideDef(KKhMin.FromPoint, NomDir, ptDMEprj)
'            If Side < 0 Then
'                d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90#)
'            Else
'                d1 = ReturnDistanceAsMeter(ptDMEprj, KKhMin.FromPoint)
'            End If
'        Else
'            d0 = ReturnDistanceAsMeter(ptDMEprj, KKhMax.FromPoint)
'
'            Side = SideDef(KKhMin.ToPoint, NomDir, ptDMEprj)
'            If Side > 0 Then
'                d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90#)
'            Else
'                d1 = ReturnDistanceAsMeter(ptDMEprj, KKhMin.ToPoint)
'            End If
'        End If
'    End If
'
'    dist0 = CalcNomPos(Xs, Ys, RefZ, PDG, OCH, d0, AheadBehindSide, 1, ptDMEprj, ptSOCPrj)
'    Dist1 = CalcNomPos(Xs, Ys, RefZ, PDG, OCH, d1, AheadBehindSide, -1, ptDMEprj, ptSOCPrj)
'
''DrawPoint ptSOCPrj, RGB(0, 255, 255)
''DrawPoint ptDMEprj, RGB(255, 0, 255)
'    CalcDMERange.Left = dist0
'    CalcDMERange.Right = Dist1
'End Function
'
'Public Function GetValidNavs(pPolygon As Polygon, fRefH As Double, NomDir As Double, Hmin As Double, _
'        hMax As Double, OCH As Double, PDG As Double, PtSOC As IPoint, _
'        pFIXAreaPolygon As IPointCollection, Optional GuidType As Long = -1, _
'        Optional GuidNav As IPoint) As FIXableNavaid()
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim L As Long
'Dim M As Long
'Dim N As Long
'Dim ii As Long
'Dim jj As Long
'Dim iNavShape As Long
'Dim iNavCall As Long
'Dim iNavType As Long
'Dim iNavCode As Long
'Dim iNavRange As Long
'
'Dim ValidNavs() As FIXableNavaid
'Dim nNav As Long
'Dim Side As Long
'Dim LeftRightSide As Long
'Dim AheadBehindSide As Long
'Dim AheadBehindKKhMax As Long
'Dim RightCut As Boolean
'
'Dim ERange As Double
'Dim dMin As Double
'Dim dMax As Double
'Dim dist0 As Double
'Dim Dist1 As Double
'Dim d0 As Double
'Dim d1 As Double
'Dim fTmp As Double
'Dim OCHequip As Double
'
'Dim DMECase As Long
'
'Dim Dir_MinL2MaxR As Double
'Dim Dir_MinR2MaxL As Double
'
'Dim InterToler As Double
'Dim TrackToler As Double
'Dim IntrH As Interval
'Dim Intr23 As Interval
'Dim Intr55 As Interval
'Dim IntrRes() As Interval
'Dim IntrRes1() As Interval
'Dim IntrRes2() As Interval
'
'Dim Intr As Interval
'
'Dim ptFNav As IPoint
'Dim ptFNavPrj As IPoint
'
'Dim ptBase As IPoint
'Dim ptFar As IPoint
'Dim ptNear As IPoint
'
'Dim ptFarD As IPoint
'Dim ptNearD As IPoint
'
'Dim ptMin23 As IPoint
'Dim ptMax23 As IPoint
'
'Dim ptTmp As IPoint
'Dim PtTmp1 As IPoint
'
'Dim azt_Far As Double
'Dim azt_Near As Double
'Dim Geom As IGeometry
'Dim KKhMin As IPolyline
'Dim KKhMax As IPolyline
'Dim KKhMinDME As IPolyline
'Dim KKhMaxDME As IPolyline
'
'Dim pPolygon1 As IPointCollection
'
'Dim pFilter As IQueryFilter
'Dim pCursor As ICursor
'Dim pNavRow As IRow
'Dim Cutter As IPointCollection
'
'Dim Construct As IConstructPoint
'Dim pTopoOper As ITopologicalOperator2
'Dim Clone As IClone
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pNAVTable.OIDFieldName + ">=0"
'
'nNav = pNAVTable.RowCount(pFilter)
'If nNav = 0 Then
'    ReDim ValidNavs(-1 To -1)
'    Set pFilter = Nothing
'    GetValidNavs = ValidNavs
'    Exit Function
'End If
'
'If GuidType > -1 Then
'    If GuidType = 0 Then
'        TrackToler = VOR.TrackingTolerance
'    ElseIf GuidType = 2 Then
'        TrackToler = NDB.TrackingTolerance
'    ElseIf GuidType = 3 Then
'        TrackToler = LLZ.TrackingTolerance
'    End If
'    Set pPolygon1 = New Polygon
'    pPolygon1.AddPoint GuidNav
'    pPolygon1.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler, 3# * R)
'    pPolygon1.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler, 3# * R)
'    pPolygon1.AddPoint GuidNav
''    If GuidType <> 3 Then
'        pPolygon1.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler + 180#, 3# * R)
'        pPolygon1.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler + 180#, 3# * R)
'        pPolygon1.AddPoint GuidNav
''    End If
'Else
'    Set Clone = pPolygon
'    Set pPolygon1 = Clone.Clone
'End If
'
'Set pTopoOper = pPolygon1
'pTopoOper.IsKnownSimple = False
'pTopoOper.Simplify
'
'Set Clone = pPolygon1
'Set pFIXAreaPolygon = Clone.Clone
'
'dMin = (Hmin - PtSOC.Z) / PDG '0# '(hMin - arAbv_Treshold.Value) / PDG
'dMax = (hMax - PtSOC.Z) / PDG
'
'Set ptBase = PtSOC
'Set Cutter = New Polyline
'
'IntrH.Left = dMin
'IntrH.Right = dMax
'
'Set ptTmp = PointAlongPlane(ptBase, NomDir, dMin)
'
'Cutter.AddPoint PointAlongPlane(ptTmp, NomDir - 90#, R)
'Cutter.AddPoint PointAlongPlane(ptTmp, NomDir + 90#, R)
'
''DrawPolygon pFIXAreaPolygon, 0      '"""""
''DrawPolyLine Cutter, 255, 2         '"""""
'
'Set pFIXAreaPolygon = CutNavPoly(pFIXAreaPolygon, Cutter, -1)
'Set KKhMin = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'If KKhMin.IsEmpty Then
'    KKhMin.FromPoint = ptTmp
'    KKhMin.ToPoint = ptTmp
'End If
'
'Set ptTmp = KKhMin.FromPoint
'ptTmp.M = 0
'KKhMin.FromPoint = ptTmp
'
'Set ptTmp = KKhMin.ToPoint
'ptTmp.M = 0
'KKhMin.ToPoint = ptTmp
'
'If SideDef(ptTmp, NomDir, KKhMin.FromPoint) < 0 Then
'    KKhMin.ReverseOrientation
'End If
'
'Cutter.RemovePoints 0, 2
'
'Set ptTmp = PointAlongPlane(ptBase, NomDir, dMax)
'Cutter.AddPoint PointAlongPlane(ptTmp, NomDir - 90#, R)
'Cutter.AddPoint PointAlongPlane(ptTmp, NomDir + 90#, R)
'
'Set pFIXAreaPolygon = CutNavPoly(pFIXAreaPolygon, Cutter, 1)
'
'Set KKhMax = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'If KKhMax.IsEmpty Then
'    KKhMax.FromPoint = ptTmp
'    KKhMax.ToPoint = ptTmp
'ElseIf SideDef(ptTmp, NomDir, KKhMax.FromPoint) < 0 Then
'    KKhMax.ReverseOrientation
'End If
'
'Set ptTmp = KKhMin.FromPoint
'ptTmp.M = ReturnAngleAsDegree(KKhMin.FromPoint, KKhMax.FromPoint)
'KKhMin.FromPoint = ptTmp
'
'Set ptTmp = KKhMin.ToPoint
'ptTmp.M = ReturnAngleAsDegree(KKhMin.ToPoint, KKhMax.ToPoint)
'KKhMin.ToPoint = ptTmp
'
'Set ptNearD = New Point
'Set Construct = ptNearD
'Construct.ConstructAngleIntersection ptBase, DegToRad(NomDir), KKhMin.ToPoint, DegToRad(NomDir + 90#)
'
'Set ptFarD = New Point
'Set Construct = ptFarD
'Construct.ConstructAngleIntersection ptBase, DegToRad(NomDir), KKhMax.ToPoint, DegToRad(NomDir + 90#)
'
'Dir_MinL2MaxR = ReturnAngleAsDegree(KKhMin.ToPoint, KKhMax.FromPoint)
'Dir_MinR2MaxL = ReturnAngleAsDegree(KKhMin.FromPoint, KKhMax.ToPoint)
'
'Set pCursor = pNAVTable.Search(pFilter, False)
'iNavShape = pCursor.FindField("Shape")
'iNavCall = pCursor.FindField("CallSign")
'iNavType = pCursor.FindField("Type")
'iNavCode = pCursor.FindField("Code")
'iNavRange = pCursor.FindField("Range_Km")
'
'OCHequip = OCH + fRefH
'
'J = 0
'I = -1
'
'ReDim ValidNavs(0 To nNav - 1)
'Set pNavRow = pCursor.NextRow
'Do While Not pNavRow Is Nothing
'    I = I + 1
'    Set ptFNav = pNavRow.Value(iNavShape)
'    Set ptFNavPrj = New Point
'    ptFNavPrj.PutCoords ptFNav.X, ptFNav.Y
'    ptFNavPrj.Z = ptFNav.Z
'
'    Set Geom = ptFNavPrj
'    Set Geom.SpatialReference = pSpRefShp
'    Geom.Project pSpRefPrj
'
'    K = pNavRow.Value(iNavCode)
'
'    LeftRightSide = SideDef(ptBase, NomDir, ptFNavPrj)
'
'    AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90#, ptFNavPrj)
'    AheadBehindKKhMax = SideDef(KKhMax.FromPoint, NomDir + 90#, ptFNavPrj)
'
'    If (K = 0) Or (K = 2) Then
'        If (K = 0) Then
'            InterToler = VOR.IntersectingTolerance
'            ERange = VOR.Range
'        Else
'            InterToler = NDB.IntersectingTolerance
'            ERange = NDB.Range
'        End If
'
'        Side = SideDef(KKhMax.FromPoint, Dir_MinL2MaxR, ptFNavPrj)
'        If Side * LeftRightSide < 0 Then GoTo NextI
'        Side = SideDef(KKhMax.ToPoint, Dir_MinR2MaxL, ptFNavPrj)
'        If Side * LeftRightSide < 0 Then GoTo NextI
'
'        If LeftRightSide > 0 Then
'            Side = SideDef(KKhMin.FromPoint, KKhMin.FromPoint.M, ptFNavPrj)
'            If Side < 0 Then GoTo NextI
'
'            If AheadBehindSide < 0 Then
'                Set ptNear = KKhMin.FromPoint
'                Set ptFar = KKhMax.ToPoint
'            ElseIf AheadBehindKKhMax < 0 Then
'                Set ptNear = KKhMin.ToPoint
'                Set ptFar = KKhMax.ToPoint
'            Else
'                Set ptNear = KKhMin.ToPoint
'                Set ptFar = KKhMax.FromPoint
'            End If
'        Else
'            Side = SideDef(KKhMin.ToPoint, KKhMin.ToPoint.M, ptFNavPrj)
'            If Side > 0 Then GoTo NextI
'
'            If AheadBehindSide < 0 Then
'                Set ptNear = KKhMin.ToPoint
'                Set ptFar = KKhMax.FromPoint
'            ElseIf AheadBehindKKhMax < 0 Then
'                Set ptNear = KKhMin.FromPoint
'                Set ptFar = KKhMax.FromPoint
'            Else
'                Set ptNear = KKhMin.FromPoint
'                Set ptFar = KKhMax.ToPoint
'            End If
'        End If
'
'        If ERange < ReturnDistanceAsMeter(ptFNavPrj, ptFar) Then GoTo NextI
'
'        azt_Far = ReturnAngleAsDegree(ptFNavPrj, ptFar)
'        azt_Near = ReturnAngleAsDegree(ptFNavPrj, ptNear)
'
'        If SubtractAngles(azt_Near, azt_Far) < 2# * InterToler Then
'            GoTo NextI
'        End If
'
'        ReDim ValidNavs(J).ValMax(0)
'        ReDim ValidNavs(J).ValMin(0)
'
'        ValidNavs(J).ValCnt = LeftRightSide
'        If LeftRightSide > 0 Then
'            ValidNavs(J).ValMax(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Far + InterToler) - 0.4999999)
'            ValidNavs(J).ValMin(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Near - InterToler) + 0.4999999)
'        Else
'            ValidNavs(J).ValMin(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Far - InterToler) + 0.4999999)
'            ValidNavs(J).ValMax(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Near + InterToler) - 0.4999999)
'        End If
'
'        If SubtractAngles(ValidNavs(J).ValMax(0) + InterToler, ValidNavs(J).ValMin(0) - InterToler) < InterToler Then
'            GoTo NextI
'        End If
'    ElseIf K = 1 Then       '   DME
'        fTmp = 0#
'        If LeftRightSide > 0 Then
'            If AheadBehindSide < 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMin.ToPoint)
'            ElseIf AheadBehindKKhMax > 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.ToPoint)
'            End If
'        Else
'            If AheadBehindSide < 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMin.FromPoint)
'            ElseIf AheadBehindKKhMax > 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.FromPoint)
'            End If
'        End If
'
'        If fTmp > DME.Range Then GoTo NextI '   Range checking
'
'        If LeftRightSide <> 0 Then
'            Set ptMin23 = New Point
'            Set ptMax23 = New Point
'            Set Construct = ptMin23
'            Construct.ConstructAngleIntersection ptBase, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value)
'            Set Construct = ptMax23
'            Construct.ConstructAngleIntersection ptBase, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value)
'        Else
'            Set ptMin23 = ptFNavPrj
'            Set ptMax23 = ptFNavPrj
'        End If
'
'        Intr23.Left = Point2LineDistancePrj(ptBase, ptMin23, NomDir + 90#) * SideDef(PtSOC, NomDir + 90#, ptMin23)
'        Intr23.Right = Point2LineDistancePrj(ptBase, ptMax23, NomDir + 90#) * SideDef(PtSOC, NomDir + 90#, ptMax23)
'
'        IntrRes = IntervalsDifference(IntrH, Intr23)
'
'Dim Xs As Double
'Dim Ys As Double
'Dim A As Double
'Dim B As Double
'Dim C As Double
'Dim D As Double
'
'        Xs = Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir + 90#) * SideDef(ptBase, NomDir + 90#, ptFNavPrj)
'        Ys = Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir)
'
'        fTmp = 1# / Tan(DegToRad(DME.SlantAngle))
'        fTmp = fTmp * fTmp
'
'        A = PDG * PDG - fTmp
'        B = 2# * ((OCHequip - ptFNavPrj.Z) * PDG + Xs * fTmp)
'        C = (OCHequip - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
'        D = B * B - 4 * A * C
'
'        If D > 0# Then
'            If A > 0 Then
'                Intr55.Left = 0.5 * (-B - Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B + Sqr(D)) / A
'            Else
'                Intr55.Left = 0.5 * (-B + Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B - Sqr(D)) / A
'            End If
'
'            N = UBound(IntrRes)
'            ReDim IntrRes1(-1 To -1)
'
'            For ii = 0 To N
'                IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)
'
'                If UBound(IntrRes1) < 0 Then
'                    IntrRes1 = IntrRes2
'                Else
'                    L = UBound(IntrRes1)
'                    M = UBound(IntrRes2)
'                    If M >= 0 Then
'                        ReDim Preserve IntrRes1(L + M + 1)
'
'                        For jj = 0 To M
'                            IntrRes1(jj + L + 1) = IntrRes2(jj)
'                        Next jj
'                    End If
'                End If
'            Next ii
'
'            IntrRes = IntrRes1
'        End If
'
'        N = UBound(IntrRes)
'
'        ii = 0
'        If N >= 0 Then
'        Do
'            If IntrRes(ii).Left = IntrRes(ii).Right Then
'                For jj = ii To N - 1
'                    IntrRes(jj) = IntrRes(jj + 1)
'                Next jj
'                N = N - 1
'            Else
'                ii = ii + 1
'            End If
'        Loop While ii < N - 1
'        End If
'
'        ii = 0
'        While ii < N - 1
'            If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
'                IntrRes(ii).Right = IntrRes(ii + 1).Right
'                For jj = ii + 1 To N - 1
'                    IntrRes(jj) = IntrRes(jj + 1)
'                Next jj
'                N = N - 1
'            Else
'                ii = ii + 1
'            End If
'        Wend
'
'        If N < 0 Then GoTo NextI
'        If Cutter.PointCount > 0 Then
'            Cutter.RemovePoints 0, Cutter.PointCount
'        End If
'
'        ReDim IntrRes1(0 To N)
'        M = 0
'
'        For ii = 0 To N
'            If Abs(IntrRes(ii).Left - IntrRes(ii).Right) > 2 * DME.MinimalError Then
'                Set ptNearD = PointAlongPlane(ptBase, NomDir, IntrRes(ii).Left)
'                Set ptFarD = PointAlongPlane(ptBase, NomDir, IntrRes(ii).Right)
'
'                Cutter.AddPoint PointAlongPlane(ptNearD, NomDir - 90#, R)
'                Cutter.AddPoint PointAlongPlane(ptNearD, NomDir + 90#, R)
'
'                Set KKhMinDME = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'                If KKhMinDME.IsEmpty Then
'                    KKhMinDME.FromPoint = ptFarD
'                    KKhMinDME.ToPoint = ptFarD
'                ElseIf SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0 Then
'                    KKhMinDME.ReverseOrientation
'                End If
'
'                Cutter.RemovePoints 0, 2
'
'                Cutter.AddPoint PointAlongPlane(ptFarD, NomDir - 90#, R)
'                Cutter.AddPoint PointAlongPlane(ptFarD, NomDir + 90#, R)
'
'                Set KKhMaxDME = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'                If KKhMaxDME.IsEmpty Then
'                    KKhMaxDME.FromPoint = ptFarD
'                    KKhMaxDME.ToPoint = ptFarD
'                ElseIf SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0 Then
'                    KKhMaxDME.ReverseOrientation
'                End If
'                Cutter.RemovePoints 0, 2
'
'                IntrRes1(M) = CalcDMERange(PtSOC, ptBase, NomDir, fRefH, PDG, OCH, ptFNavPrj, KKhMinDME, KKhMaxDME)
'                If IntrRes1(M).Left < IntrRes1(M).Right Then
'                    M = M + 1
'                    ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
'                End If
'            End If
'        Next ii
'
'        M = M - 1
'        If M < 0 Then
'            GoTo NextI
'        End If
'
'        If M > 0 Then ValidNavs(J).ValCnt = 0
'
'        ReDim ValidNavs(J).ValMax(M)
'        ReDim ValidNavs(J).ValMin(M)
'
'        For ii = 0 To M
'            ValidNavs(J).ValMin(ii) = Round(IntrRes1(ii).Left + 0.4999999)
'            ValidNavs(J).ValMax(ii) = Round(IntrRes1(ii).Right - 0.4999999)
'        Next ii
'
'        If ValidNavs(J).ValMax(0) < ValidNavs(J).ValMin(0) Then
'            GoTo NextI
'        End If
'    Else
'        GoTo NextI
'    End If
'
'    ValidNavs(J).Name = pNavRow.Value(iNavCall)
'    ValidNavs(J).NavTypeName = pNavRow.Value(iNavType)
'    ValidNavs(J).Range = pNavRow.Value(iNavRange) * 1000#
'    ValidNavs(J).NavTypeCode = K
'    Set ValidNavs(J).Ptgeo = ptFNav
'    Set ValidNavs(J).ptPrj = ptFNavPrj
'    J = J + 1
'NextI:
'    Set pNavRow = pCursor.NextRow
'Loop
'
''Set pGroupElem = New GroupElement
''pGroupElem.AddElement DrawPolygon(pFIXAreaPolygon, RGB(195, 195, 195), False)
''Set FIXElem = pGroupElem
''MsgDialog.ShowMessage "1-       J = " + CStr(J)
'
'If J > 0 Then
'    ReDim Preserve ValidNavs(0 To J - 1)
'Else
'    ReDim ValidNavs(-1 To -1)
'End If
'
'Set pFilter = Nothing
'GetValidNavs = ValidNavs
'End Function

'Public Function GetValidFAFNavs(ptFicTHR As IPoint, MaxDist As Double, NomDir As Double, PtFAF As IPoint, hFAF As Double, _
'        GuidType As Long, GuidNav As IPoint) As FIXableNavaid()
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim iNavShape As Long
'Dim iNavCall As Long
'Dim iNavType As Long
'Dim iNavCode As Long
'Dim iNavRange As Long
'
'Dim LeftRightSide As Long
'Dim ValidNavs() As FIXableNavaid
'Dim nNav As Long
'Dim Side As Long
'Dim AheadBehindSide As Long
'Dim Heq As Double
'
'Dim dMin As Double
'Dim dMax As Double
'Dim dist0 As Double
'Dim Dist1 As Double
'Dim d0 As Double
'Dim d1 As Double
'Dim fTmp As Double
'Dim Hequ As Double
'
'Dim InterToler As Double
'Dim TrackToler As Double
'
'Dim ptFNav As IPoint
'Dim ptFNavPrj As IPoint
'Dim ptTmp As IPoint
'
'Dim DirNAV2FAF As Double
'Dim Alpha As Double
'Dim Geom As IGeometry
'Dim pTmpPoly As IPolygon
'Dim p4Poly As IPointCollection
'Dim pCircle As IPointCollection
'Dim pGuidPoly As IPointCollection
'Dim pGeomCollection As IGeometryCollection
'Dim pGeomCollection1 As IGeometryCollection
'Dim pProxi As IProximityOperator
'
'Dim pCursor As ICursor
'Dim pFilter As IQueryFilter
'Dim pNavRow As IRow
'Dim Construct As IConstructPoint
'Dim pTopoOper As ITopologicalOperator2
''===========================================================================
'If GuidType = 0 Then
'    TrackToler = VOR.TrackingTolerance
'    fTmp = VOR.Range
'ElseIf GuidType = 2 Then
'    TrackToler = NDB.TrackingTolerance
'    fTmp = NDB.Range
'ElseIf GuidType = 3 Then
'    TrackToler = LLZ.TrackingTolerance
'    fTmp = LLZ.Range
'End If
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pNAVTable.OIDFieldName + ">=0"
'
'nNav = pNAVTable.RowCount(pFilter)
'If nNav = 0 Then
'    ReDim ValidNavs(-1 To -1)
'    GetValidFAFNavs = ValidNavs
'    Set pCursor = Nothing
'    Exit Function
'End If
'
'Set pCursor = pNAVTable.Search(pFilter, False)
'iNavShape = pCursor.FindField("Shape")
'iNavCall = pCursor.FindField("CallSign")
'iNavType = pCursor.FindField("Type")
'iNavCode = pCursor.FindField("Code")
'iNavRange = pCursor.FindField("Range_Km")
'
'Set ptTmp = New Point
'Set Construct = ptTmp
'
'Set p4Poly = New Polygon
'Set pGeomCollection1 = p4Poly
'
'AheadBehindSide = SideDef(PtFAF, NomDir + 90#, GuidNav)
'Set pGuidPoly = New Polygon
'pGuidPoly.AddPoint GuidNav
'
'If AheadBehindSide > 0 Then
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler + 180#, fTmp)
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler + 180#, fTmp)
'ElseIf AheadBehindSide > 0 Then
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp)
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp)
'Else
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler + 180#, fTmp)
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler + 180#, fTmp)
'
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp)
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp)
'End If
'Set pTopoOper = pGuidPoly
'pTopoOper.IsKnownSimple = False
'pTopoOper.Simplify
'
'I = -1
'J = 0
'
'ReDim ValidNavs(0 To nNav - 1)
'Set pNavRow = pCursor.NextRow
'Do While Not pNavRow Is Nothing
'    I = I + 1
'    Set ptFNav = pNavRow.Value(iNavShape)
'    Set ptFNavPrj = New Point
'
'    ptFNavPrj.PutCoords ptFNav.X, ptFNav.Y
'    ptFNavPrj.Z = ptFNav.Z
'
'    Set Geom = ptFNavPrj
'    Set Geom.SpatialReference = pSpRefShp
'    Geom.Project pSpRefPrj
'
'    K = pNavRow.Value(iNavCode)
'
'    LeftRightSide = SideDef(PtFAF, NomDir, ptFNavPrj)
'    DirNAV2FAF = ReturnAngleAsDegree(ptFNavPrj, PtFAF)
'    d0 = ReturnDistanceAsMeter(ptFNavPrj, PtFAF)
'
'    If (K = 0) Or (K = 2) Then
'        If (K = 0) Then
'            InterToler = VOR.IntersectingTolerance
'        Else
'            InterToler = NDB.IntersectingTolerance
'        End If
'
'        Set pProxi = pGuidPoly
'        If pProxi.ReturnDistance(ptFNavPrj) = 0# Then GoTo NextI
'
'        Construct.ConstructAngleIntersection ptFNavPrj, DegToRad(DirNAV2FAF + InterToler), PtFAF, DegToRad(NomDir)
'
'        If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'
'        Set Construct = ptTmp
'        Construct.ConstructAngleIntersection ptFNavPrj, DegToRad(DirNAV2FAF - InterToler), PtFAF, DegToRad(NomDir)
'
'        If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'
'        K = 0.5 * (AheadBehindSide + 1) + (LeftRightSide + 1)
'        Select Case K
'        Case 0
'            Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir - TrackToler), ptFNavPrj, DegToRad(DirNAV2FAF + InterToler)
'        Case 1
'            Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir + TrackToler), ptFNavPrj, DegToRad(DirNAV2FAF - InterToler)
'        Case 2
'            Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir + TrackToler), ptFNavPrj, DegToRad(DirNAV2FAF + InterToler)
'        Case 3
'            Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir - TrackToler), ptFNavPrj, DegToRad(DirNAV2FAF + InterToler)
'        End Select
'        If Point2LineDistancePrj(ptFicTHR, ptTmp, NomDir + 90#) > MaxDist Then GoTo NextI
'    ElseIf K = 1 Then
'        fTmp = SubtractAngles(DirNAV2FAF, NomDir)
'        If fTmp > 90# Then fTmp = 180# - fTmp
'        If fTmp > arTP_by_DME_div.Value Then GoTo NextI
'
'        If d0 + arFAFTolerance.Value > DME.Range Then GoTo NextI
'
'        Hequ = hFAF + ptFicTHR.Z - ptFNavPrj.Z
'        Alpha = RadToDeg(Atn(Hequ / d0))
'        If Alpha > 90# - DME.SlantAngle Then GoTo NextI
'
'        dist0 = Sqr(Hequ * Hequ + d0 * d0)
'
'        dMin = (dist0 - DME.MinimalError) / (1# + DME.ErrorScalingUp)
'        dMax = (dist0 + DME.MinimalError) / (1# - DME.ErrorScalingUp)
'
'        Side = SideDef(PtFAF, NomDir + 90#, ptFNavPrj)
'
'        If Side < 0 Then
'            CircleVectorIntersect ptFNavPrj, dMin, PtFAF, NomDir, ptTmp
'            If ptTmp.IsEmpty Then GoTo NextI
'            If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'
'            CircleVectorIntersect ptFNavPrj, dMax, PtFAF, NomDir, ptTmp
'            If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'            fTmp = ReturnDistanceAsMeter(ptFNavPrj, GuidNav)
'        Else
'            CircleVectorIntersect ptFNavPrj, dMin, PtFAF, NomDir + 180#, ptTmp
'            If ptTmp.IsEmpty Then GoTo NextI
'            If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'
'            CircleVectorIntersect ptFNavPrj, dMax, PtFAF, NomDir + 180#, ptTmp
'            If ReturnDistanceAsMeter(PtFAF, ptTmp) > arFAFTolerance.Value Then GoTo NextI
'        End If
'
'        Set pCircle = CreatePrjCircle(ptFNavPrj, dMax)
'        Set pTmpPoly = CreatePrjCircle(ptFNavPrj, dMin)
'        Set pTopoOper = pCircle
'        Set pCircle = pTopoOper.Difference(pTmpPoly)
'
'        Set pTopoOper = pCircle
'        pTopoOper.IsKnownSimple = False
'        pTopoOper.Simplify
'
'        Set pTmpPoly = pTopoOper.Intersect(pGuidPoly, esriGeometry2Dimension)
'        Set pGeomCollection = pTmpPoly
'
'        If pGeomCollection1.GeometryCount > 0 Then
'            pGeomCollection1.RemoveGeometries 0, pGeomCollection1.GeometryCount
'        End If
'
'        If pTmpPoly.ExteriorRingCount > 1 Then
'            Set pProxi = PtFAF
'            For K = 0 To pGeomCollection.GeometryCount - 1
'                pGeomCollection1.RemoveGeometries 0, pGeomCollection1.GeometryCount
'                pGeomCollection1.AddGeometry pGeomCollection.Geometry(K)
'                If pProxi.ReturnDistance(pGeomCollection1) = 0# Then Exit For
'            Next K
'        Else
'            pGeomCollection1.AddGeometry pGeomCollection.Geometry(0)
'        End If
'
'        For K = 0 To p4Poly.PointCount - 1
'            If Point2LineDistancePrj(ptFicTHR, p4Poly.Point(K), NomDir + 90#) > MaxDist Then GoTo NextI
'        Next K
'        K = 1
'    End If
'
'    ValidNavs(J).Name = pNavRow.Value(iNavCall)
'    ValidNavs(J).NavTypeName = pNavRow.Value(iNavType)
'    ValidNavs(J).NavTypeCode = pNavRow.Value(iNavCode)
'    ValidNavs(J).Range = pNavRow.Value(iNavRange) * 1000#
'    Set ValidNavs(J).Ptgeo = ptFNav
'    Set ValidNavs(J).ptPrj = ptFNavPrj
'    J = J + 1
'NextI:
'    Set pNavRow = pCursor.NextRow
'Loop
'
'If J > 0 Then
'    ReDim Preserve ValidNavs(0 To J - 1)
'Else
'    ReDim ValidNavs(-1 To -1)
'End If
'
'Set pCursor = Nothing
'GetValidFAFNavs = ValidNavs
'End Function
'
'Public Function GetValidIFNavs(PtFAF As IPoint, fRefH As Double, minDist As Double, MaxDist As Double, NomDir As Double, _
'        GuidType As Long, GuidNav As IPoint) As FIXableNavaid()
'Dim AheadBehindKKhMax As Long
'Dim AheadBehindSide As Long
'Dim LeftRightSide As Long
'Dim iNavRange As Long
'Dim iNavShape As Long
'Dim iNavCall As Long
'Dim iNavType As Long
'Dim iNavCode As Long
'Dim DMECase As Long
'Dim nNav As Long
'Dim Side As Long
'Dim ii As Long
'Dim jj As Long
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim L As Long
'Dim M As Long
'Dim N As Long
'
'Dim ValidNavs() As FIXableNavaid
'Dim RightCut As Boolean
'
'Dim Dir_MinL2MaxR As Double
'Dim Dir_MinR2MaxL As Double
'Dim fMinDMEDist As Double
'Dim InterToler As Double
'Dim TrackToler As Double
'Dim azt_Near As Double
'Dim azt_Far As Double
'Dim ERange As Double
'Dim Hequip As Double
'Dim dist0 As Double
'Dim Dist1 As Double
'Dim Betta As Double
'Dim fTmp As Double
'Dim d0 As Double
'Dim d1 As Double
'Dim Xs As Double
'Dim Ys As Double
'Dim A As Double
'Dim B As Double
'Dim C As Double
'Dim D As Double
'
'Dim IntrRes1() As Interval
'Dim IntrRes2() As Interval
'Dim IntrRes() As Interval
'Dim Intr3700 As Interval
'Dim Intr23 As Interval
'Dim Intr55 As Interval
'Dim IntrH As Interval
'Dim Intr As Interval
'
''Dim pFIXAreaPolygon As IPointCollection
'Dim pTopoOper As ITopologicalOperator2
'Dim pGuidPoly As IPointCollection
'Dim pPolygon1 As IPointCollection
'Dim Construct As IConstructPoint
'Dim pProxi As IProximityOperator
'Dim Cutter As IPointCollection
'Dim KKhMinDME As IPolyline
'Dim KKhMaxDME As IPolyline
'Dim KKhMin As IPolyline
'Dim KKhMax As IPolyline
'Dim Geom As IGeometry
'
'Dim ptFNavPrj As IPoint
'Dim ptMin23 As IPoint
'Dim ptMax23 As IPoint
'Dim ptNearD As IPoint
'Dim ptFarD As IPoint
'Dim ptNear As IPoint
'Dim ptFNav As IPoint
'Dim PtTmp1 As IPoint
'Dim ptTmp As IPoint
'Dim ptFar As IPoint
'
'Dim pFilter As IQueryFilter
'Dim pCursor As ICursor
'Dim pNavRow As IRow
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pNAVTable.OIDFieldName + ">=0"
'
'nNav = pNAVTable.RowCount(pFilter)
'If nNav = 0 Then
'    ReDim ValidNavs(-1 To -1)
'    GetValidIFNavs = ValidNavs
'    Set pCursor = Nothing
'    Exit Function
'End If
'
'If GuidType = 0 Then
'    TrackToler = VOR.TrackingTolerance
'ElseIf GuidType = 2 Then
'    TrackToler = NDB.TrackingTolerance
'ElseIf GuidType = 3 Then
'    TrackToler = LLZ.TrackingTolerance
'End If
'
'Set pGuidPoly = New Polygon
''If GuidType <> 3 Then
'    pGuidPoly.AddPoint GuidNav
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler, 3# * R)
'    pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler, 3# * R)
''End If
'pGuidPoly.AddPoint GuidNav
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler + 180#, 3# * R)
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler + 180#, 3# * R)
'pGuidPoly.AddPoint GuidNav
'
'Set pTopoOper = pGuidPoly
'pTopoOper.IsKnownSimple = False
'pTopoOper.Simplify
'
'Set ptFar = PointAlongPlane(PtFAF, NomDir + 180#, MaxDist)
'Set ptNear = PointAlongPlane(PtFAF, NomDir + 180#, minDist)
'
'Set KKhMax = New Polyline
'Set ptTmp = New Point
'Set Construct = ptTmp
'
'Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir - TrackToler), ptFar, DegToRad(NomDir + 90#)
'KKhMax.FromPoint = ptTmp
'
'Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir + TrackToler), ptFar, DegToRad(NomDir + 90#)
'KKhMax.ToPoint = ptTmp
'
'If SideDef(KKhMax.ToPoint, NomDir, KKhMax.FromPoint) > 0 Then
'    KKhMax.ReverseOrientation
'End If
'
'Set Cutter = New Polyline
'Set KKhMaxDME = New Polyline
'
'Set pCursor = pNAVTable.Search(pFilter, False)
'
'iNavShape = pCursor.FindField("Shape")
'iNavCall = pCursor.FindField("CallSign")
'iNavType = pCursor.FindField("Type")
'iNavCode = pCursor.FindField("Code")
'iNavRange = pCursor.FindField("Range_Km")
'
'Hequip = PtFAF.Z + fRefH
'
'I = -1
'J = 0
'ReDim ValidNavs(0 To nNav - 1)
'Set pNavRow = pCursor.NextRow
'Do While Not pNavRow Is Nothing
'    I = I + 1
'    Set ptFNav = pNavRow.Value(iNavShape)
'    Set ptFNavPrj = New Point
'
'    ptFNavPrj.PutCoords ptFNav.X, ptFNav.Y
'    ptFNavPrj.Z = ptFNav.Z
'
'    Set Geom = ptFNavPrj
'    Set Geom.SpatialReference = pSpRefShp
'    Geom.Project pSpRefPrj
'
'    K = pNavRow.Value(iNavCode)
'
'    LeftRightSide = SideDef(ptNear, NomDir + 180#, ptFNavPrj)
'    AheadBehindSide = SideDef(ptNear, NomDir - 90#, ptFNavPrj) 'ptFar
'
'    If (K = 0) Or (K = 2) Then
'        If (K = 0) Then
'            InterToler = VOR.IntersectingTolerance
'            ERange = VOR.Range
'        Else
'            InterToler = NDB.IntersectingTolerance
'            ERange = NDB.Range
'        End If
'
'        Set Construct = ptTmp
'
'        Set pProxi = pGuidPoly
'        If pProxi.ReturnDistance(ptFNavPrj) = 0# Then GoTo NextI
'
'        Side = SideDef(KKhMax.FromPoint, NomDir - LeftRightSide * 90#, ptFNavPrj)
'        If Side < 0 Then
'            Set ptFarD = KKhMax.ToPoint
'        Else
'            Set ptFarD = KKhMax.FromPoint
'        End If
'
'        If ERange < ReturnDistanceAsMeter(ptFNavPrj, ptFarD) Then GoTo NextI
'
'        azt_Far = ReturnAngleAsDegree(ptFNavPrj, ptFarD)
'        azt_Near = ReturnAngleAsDegree(ptFNavPrj, ptNear)
'
'        If SubtractAngles(azt_Near, azt_Far) < 2# * InterToler Then
'            GoTo NextI
'        End If
'
'        D = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)
'        If RadToDeg(Atn(arIFTolerance.Value / D)) < InterToler Then GoTo NextI
'
'        Betta = 0.5 * (ArcCos(2# * D * Sin(DegToRad(InterToler)) / arIFTolerance.Value - Cos(DegToRad(InterToler))) - DegToRad(InterToler))
'
'        Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + 90#) + Betta
''DrawPoint ptTmp, RGB(0, 0, 255)
'        If SideDef(PtFAF, NomDir + 90#, ptTmp) > 0 Then
'            dist0 = 0#
'        Else
'            dist0 = ReturnDistanceAsMeter(PtFAF, ptTmp)
'        End If
'
'        Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + 90#) - Betta
''DrawPoint ptTmp, RGB(0, 0, 255)
'
'        If SideDef(PtFAF, NomDir + 90#, ptTmp) > 0 Then
'            Dist1 = 0#
'        Else
'            Dist1 = ReturnDistanceAsMeter(PtFAF, ptTmp)
'        End If
'
'        If Dist1 < dist0 Then
'            fTmp = Dist1
'            Dist1 = dist0
'            dist0 = fTmp
'        End If
'
'        d0 = minDist
'        Construct.ConstructAngleIntersection GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(azt_Far + LeftRightSide * InterToler)
'        d1 = ReturnDistanceAsMeter(PtFAF, ptTmp)
'        If d0 < dist0 Then d0 = dist0
'        If d1 > Dist1 Then d1 = Dist1
'        If d1 < d0 Then GoTo NextI
'
'        Set ptNearD = PointAlongPlane(PtFAF, NomDir + 180#, d0)
'        Set ptFarD = PointAlongPlane(PtFAF, NomDir + 180#, d1)
'
'        azt_Far = ReturnAngleAsDegree(ptFNavPrj, ptFarD)
'        azt_Near = ReturnAngleAsDegree(ptFNavPrj, ptNearD)
'
'        ReDim ValidNavs(J).ValMax(0)
'        ReDim ValidNavs(J).ValMin(0)
'
'        ValidNavs(J).ValCnt = LeftRightSide
'
''        ValidNavs(J).ValMax(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Far) - 0.4999999)
''        ValidNavs(J).ValMin(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Near) + 0.4999999)
''
''        If SubtractAngles(ValidNavs(J).ValMax(0), ValidNavs(J).ValMin(0)) < InterToler Then
''            GoTo NextI
''        End If
'        If LeftRightSide > 0 Then
'            ValidNavs(J).ValMax(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Far) - 0.4999999)
'            ValidNavs(J).ValMin(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Near) + 0.4999999)
'        Else
'            ValidNavs(J).ValMin(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Far) + 0.4999999)
'            ValidNavs(J).ValMax(0) = Round(Dir2Azimuth(ptFNavPrj, azt_Near) - 0.4999999)
'        End If
'        If SubtractAngles(ValidNavs(J).ValMax(0) + InterToler, ValidNavs(J).ValMin(0) - InterToler) < InterToler Then
'            GoTo NextI
'        End If
'    ElseIf K = 1 Then       '   DME
''    arImHorSegLen
'        IntrH.Left = minDist
'        IntrH.Right = MaxDist
'
'        If AheadBehindSide < 0 Then
'            fTmp = ReturnDistanceAsMeter(ptFNavPrj, ptNear)
'        Else
'            If LeftRightSide > 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.ToPoint)
'            Else
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.FromPoint)
'            End If
'        End If
'
'        If fTmp > DME.Range Then GoTo NextI '   Range checking
'
'        If LeftRightSide <> 0 Then
'            Set ptMin23 = New Point
'            Set ptMax23 = New Point
'            Set Construct = ptMin23
'            Construct.ConstructAngleIntersection PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value)
'            Set Construct = ptMax23
'            Construct.ConstructAngleIntersection PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value)
'        Else
'            Set ptMin23 = ptFNavPrj
'            Set ptMax23 = ptFNavPrj
'        End If
'
'        Intr23.Left = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90#) * SideDef(PtFAF, NomDir - 90#, ptMin23)
'        Intr23.Right = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90#) * SideDef(PtFAF, NomDir - 90#, ptMax23)
'
'        fTmp = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir + TrackToler) + 5#
'        D = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir - TrackToler) + 5#
'        If D < fTmp Then D = fTmp
'        fMinDMEDist = (DME.MinimalError + D) / (1# - DME.ErrorScalingUp)
'
'        If CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir + 180#, ptMin23) >= 0 Then
'            CircleVectorIntersect ptFNavPrj, fMinDMEDist, PtFAF, NomDir, ptMax23
'            A = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90#) * SideDef(PtFAF, NomDir - 90#, ptMax23)
'            B = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90#) * SideDef(PtFAF, NomDir - 90#, ptMin23)
'            If Intr23.Left > A Then Intr23.Left = A
'            If Intr23.Right < B Then Intr23.Right = B
'        End If
'
'        IntrRes = IntervalsDifference(IntrH, Intr23)
'        If UBound(IntrRes) < 0 Then GoTo NextI
'
''        fTmp = CircleVectorIntersect(ptFNavPrj, maxDist, ptFAF, NomDir + 180#, ptTmp)
'
'        Xs = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir + 90#) * SideDef(PtFAF, NomDir - 90#, ptFNavPrj)
'        Ys = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)
''======================= 3700 ==============================================================
'        A = 1# + arImDescent_Max.Value * arImDescent_Max.Value
'        B = 2# * arImDescent_Max.Value * Hequip - Xs
'        C = Hequip * Hequip + Xs * Xs + Ys * Ys - ((arIFTolerance.Value * Cos(DegToRad(arTP_by_DME_div.Value)) - DME.MinimalError) / DME.ErrorScalingUp) ^ 2
'        D = B * B - 4# * A * C
'
'        If D > 0# Then
'            D = Sqr(D)
'            If A > 0 Then
'                Intr3700.Left = 0.5 * (-B - D) / A
'                Intr3700.Right = 0.5 * (-B + D) / A
'            Else
'                Intr3700.Left = 0.5 * (-B + D) / A
'                Intr3700.Right = 0.5 * (-B - D) / A
'            End If
'            If IntrH.Left < Intr3700.Left Then IntrH.Left = Intr3700.Left
'            If IntrH.Right > Intr3700.Right Then IntrH.Right = Intr3700.Right
'        Else
'            GoTo NextI
'        End If
'        If IntrH.Left >= IntrH.Right Then GoTo NextI
''===========================================================================================
'        fTmp = 1# / Tan(DegToRad(DME.SlantAngle))
'        fTmp = fTmp * fTmp
'
'        A = arImDescent_Max.Value * arImDescent_Max.Value - fTmp
'        B = 2# * ((Hequip - ptFNavPrj.Z) * arImDescent_Max.Value + Xs * fTmp)
'        C = (Hequip - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
'        D = B * B - 4# * A * C
'
'        If D > 0# Then
'            If A > 0 Then
'                Intr55.Left = 0.5 * (-B - Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B + Sqr(D)) / A
'            Else
'                Intr55.Left = 0.5 * (-B + Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B - Sqr(D)) / A
'            End If
'
''Set ptTmp = PointAlongPlane(ptFAF, NomDir + 180#, Intr55.Left)
''DrawPoint ptTmp, 0
''Set ptTmp = PointAlongPlane(ptFAF, NomDir + 180#, Intr55.Right)
''DrawPoint ptTmp, 255
'
'            N = UBound(IntrRes)
'            ReDim IntrRes1(-1 To -1)
'
'            For ii = 0 To N
'                IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)
'
'                If UBound(IntrRes1) < 0 Then
'                    IntrRes1 = IntrRes2
'                Else
'                    L = UBound(IntrRes1)
'                    M = UBound(IntrRes2)
'                    If M >= 0 Then
'                        ReDim Preserve IntrRes1(L + M + 1)
'
'                        For jj = 0 To M
'                            IntrRes1(jj + L + 1) = IntrRes2(jj)
'                        Next jj
'                    End If
'                End If
'            Next ii
'
'            IntrRes = IntrRes1
'        End If
'
'        N = UBound(IntrRes)
'
'        ii = 0
'        If N >= 0 Then
'            Do
'                If IntrRes(ii).Left = IntrRes(ii).Right Then
'                    For jj = ii To N - 1
'                        IntrRes(jj) = IntrRes(jj + 1)
'                    Next jj
'                    N = N - 1
'                Else
'                    ii = ii + 1
'                End If
'            Loop While ii < N - 1
'        End If
'
'        ii = 0
'        While ii < N - 1
'            If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
'                IntrRes(ii).Right = IntrRes(ii + 1).Right
'                For jj = ii + 1 To N - 1
'                    IntrRes(jj) = IntrRes(jj + 1)
'                Next jj
'                N = N - 1
'            Else
'                ii = ii + 1
'            End If
'        Wend
'
'        If N < 0 Then GoTo NextI
'
'        ReDim IntrRes1(0 To N)
'        M = 0
'
'        For ii = 0 To N
'            Set ptNearD = PointAlongPlane(PtFAF, NomDir + 180#, IntrRes(ii).Left)
'            Set ptFarD = PointAlongPlane(PtFAF, NomDir + 180#, IntrRes(ii).Right)
'            d1 = ReturnDistanceAsMeter(ptNearD, ptFNavPrj)
'            Set KKhMinDME = New Polyline
'            KKhMinDME.FromPoint = ptNearD
'            KKhMinDME.ToPoint = ptNearD
''DrawPoint ptFarD, 255
''DrawPoint ptNearD, 0
'
'            KKhMaxDME.FromPoint = PointAlongPlane(ptFarD, NomDir - 90#, arIFHalfWidth.Value)
'            KKhMaxDME.ToPoint = PointAlongPlane(ptFarD, NomDir + 90#, arIFHalfWidth.Value)
'
'            Set KKhMaxDME = pTopoOper.Intersect(KKhMaxDME, esriGeometry1Dimension)
'            If KKhMaxDME.IsEmpty Then
'                KKhMaxDME.FromPoint = ptFarD
'                KKhMaxDME.ToPoint = ptFarD
'            ElseIf SideDef(ptFarD, NomDir + 180#, KKhMaxDME.FromPoint) < 0 Then
'                KKhMaxDME.ReverseOrientation
'            End If
'
'            IntrRes1(M) = CalcDMERange(PtFAF, PtFAF, NomDir + 180#, fRefH, arImDescent_Max.Value, Hequip, ptFNavPrj, KKhMinDME, KKhMaxDME)
'            If ii = 0 Then
'                If AheadBehindSide < 0 Then
'                    If IntrRes1(M).Left > d1 Then
'                        IntrRes1(M).Left = d1
'                    End If
'                Else
'                    If IntrRes1(M).Right < d1 Then
'                        IntrRes1(M).Right = d1
'                    End If
'                End If
'            End If
'
'            If IntrRes1(M).Left < IntrRes1(M).Right Then
'                M = M + 1
'                ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
'            End If
'        Next ii
'
'        M = M - 1
'        If M < 0 Then
'            GoTo NextI
'        End If
'
'        If M > 0 Then ValidNavs(J).ValCnt = 0
'
'        ReDim ValidNavs(J).ValMax(M)
'        ReDim ValidNavs(J).ValMin(M)
'
'        For ii = 0 To M
'            ValidNavs(J).ValMin(ii) = Round(IntrRes1(ii).Left + 0.4999999)
'            ValidNavs(J).ValMax(ii) = Int(IntrRes1(ii).Right)
'        Next ii
'
'        If ValidNavs(J).ValMax(0) < ValidNavs(J).ValMin(0) Then
'            GoTo NextI
'        End If
'    Else
'        GoTo NextI
'    End If
'
'    ValidNavs(J).Name = pNavRow.Value(iNavCall)
'    ValidNavs(J).NavTypeName = pNavRow.Value(iNavType)
'    ValidNavs(J).Range = pNavRow.Value(iNavRange) * 1000#
'    ValidNavs(J).NavTypeCode = K
'    Set ValidNavs(J).Ptgeo = ptFNav
'    Set ValidNavs(J).ptPrj = ptFNavPrj
'
'    J = J + 1
'NextI:
'    Set pNavRow = pCursor.NextRow
'Loop
'
'If J > 0 Then
'    ReDim Preserve ValidNavs(J - 1)
'Else
'    ReDim ValidNavs(-1 To -1)
'End If
'
'GetValidIFNavs = ValidNavs
'End Function
'
'Public Function GetValidMAPtNavs(PtFAF As IPoint, minDist As Double, MaxDist As Double, _
'    NomDir As Double, OCH As Double, Category As Long, GuidType As Long, GuidNav As IPoint) As FIXableNavaid()
'
'Dim ValidNavs() As FIXableNavaid
'
'Dim AheadBehindKKhMax As Long
'Dim AheadBehindSide As Long
'Dim LeftRightSide As Long
'Dim iNavShape As Long
'Dim iNavCall As Long
'Dim iNavType As Long
'Dim iNavCode As Long
'Dim iNavRange As Long
'
'Dim Side As Long
'Dim nNav As Long
'Dim ii As Long
'Dim jj As Long
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim L As Long
'Dim M As Long
'Dim N As Long
'
'Dim pGuidPoly As IPointCollection
'Dim ptFNavPrj As IPoint
'Dim ptMin23 As IPoint
'Dim ptMax23 As IPoint
'
'Dim ptNearD As IPoint
'Dim ptFarD As IPoint
'
'Dim ptFNav As IPoint
'Dim ptNear As IPoint
'Dim ptFar As IPoint
'Dim ptTmp As IPoint
'
'Dim Geom As IGeometry
'
'Dim pFilter As IQueryFilter
'Dim pCursor As ICursor
'Dim pNavRow As IRow
'
'Dim Dir_MinL2MaxR As Double
'Dim Dir_MinR2MaxL As Double
'Dim fMinDMEDist As Double
'Dim InterToler As Double
'Dim TrackToler As Double
'Dim AztNear As Double
'Dim AztFar As Double
'Dim fTmp As Double
'Dim Xs As Double
'Dim Ys As Double
'Dim A As Double
'Dim B As Double
'Dim C As Double
'Dim D As Double
'
'Dim pTopoOper As ITopologicalOperator2
'Dim pConstruct As IConstructPoint
'Dim Construct As IConstructPoint
'Dim pProxi As IProximityOperator
'
'Dim Intr23 As Interval
'Dim Intr55 As Interval
'Dim IntrH As Interval
'Dim Intr As Interval
'
'Dim IntrRes1() As Interval
'Dim IntrRes2() As Interval
'Dim IntrRes() As Interval
'
'Dim KKhMinDME As IPolyline
'Dim KKhMaxDME As IPolyline
'Dim Cutter As IPolyline
'Dim KKhMin As IPolyline
'Dim KKhMax As IPolyline
''===========================================================================
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pNAVTable.OIDFieldName + ">=0"
'
'nNav = pNAVTable.RowCount(pFilter)
'If nNav = 0 Then
'    ReDim ValidNavs(-1 To -1)
'    GetValidMAPtNavs = ValidNavs
'    Set pCursor = Nothing
'    Exit Function
'End If
'
'AheadBehindSide = SideDef(PtFAF, NomDir + 90#, GuidNav)
'Set pGuidPoly = New Polygon
'pGuidPoly.AddPoint GuidNav
'
'Set KKhMinDME = New Polyline
'Set KKhMaxDME = New Polyline
'
'If GuidType = 0 Then
'    TrackToler = VOR.TrackingTolerance
'ElseIf GuidType = 2 Then
'    TrackToler = NDB.TrackingTolerance
'ElseIf GuidType = 3 Then
'    TrackToler = LLZ.TrackingTolerance
'End If
'
'pGuidPoly.AddPoint GuidNav
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler, 3# * R)
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler, 3# * R)
'pGuidPoly.AddPoint GuidNav
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir - TrackToler + 180#, 3# * R)
'pGuidPoly.AddPoint PointAlongPlane(GuidNav, NomDir + TrackToler + 180#, 3# * R)
'pGuidPoly.AddPoint GuidNav
'
'Set pTopoOper = pGuidPoly
'pTopoOper.IsKnownSimple = False
'pTopoOper.Simplify
'
'Set ptFar = PointAlongPlane(PtFAF, NomDir, minDist)
'Set ptNear = PointAlongPlane(PtFAF, NomDir, MaxDist)
''=============================================================================================
'Set Cutter = New Polyline
'
'Cutter.FromPoint = PointAlongPlane(ptFar, NomDir - 90#, R)
'Cutter.ToPoint = PointAlongPlane(ptFar, NomDir + 90#, R)
'
'Set KKhMin = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'If KKhMin.IsEmpty Then
'    KKhMin.FromPoint = ptFar
'    KKhMin.ToPoint = ptFar
'ElseIf SideDef(ptFar, NomDir, KKhMin.FromPoint) < 0 Then
'    KKhMin.ReverseOrientation
'End If
'
'Cutter.FromPoint = PointAlongPlane(ptNear, NomDir - 90#, R)
'Cutter.ToPoint = PointAlongPlane(ptNear, NomDir + 90#, R)
'
'Set KKhMax = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'
'If KKhMax.IsEmpty Then
'    KKhMax.FromPoint = ptNear
'    KKhMax.ToPoint = ptNear
'ElseIf SideDef(ptNear, NomDir, KKhMax.FromPoint) < 0 Then
'    KKhMax.ReverseOrientation
'End If
'
'Dir_MinL2MaxR = ReturnAngleAsDegree(KKhMin.ToPoint, KKhMax.FromPoint)
'Dir_MinR2MaxL = ReturnAngleAsDegree(KKhMin.FromPoint, KKhMax.ToPoint)
''=============================================================================================
'Set pProxi = pGuidPoly
'Set ptTmp = New Point
'
'Set pCursor = pNAVTable.Search(pFilter, False)
'iNavShape = pCursor.FindField("Shape")
'iNavCall = pCursor.FindField("CallSign")
'iNavType = pCursor.FindField("Type")
'iNavCode = pCursor.FindField("Code")
'iNavRange = pCursor.FindField("Range_Km")
'
'J = 0
'I = -1
'
'ReDim ValidNavs(0 To nNav - 1)
'Set pNavRow = pCursor.NextRow
'Do While Not pNavRow Is Nothing
'    I = I + 1
'    Set ptFNav = pNavRow.Value(iNavShape)
'    Set ptFNavPrj = New Point
'
'    ptFNavPrj.PutCoords ptFNav.X, ptFNav.Y
'    ptFNavPrj.Z = ptFNav.Z
'
'    Set Geom = ptFNavPrj
'    Set Geom.SpatialReference = pSpRefShp
'    Geom.Project pSpRefPrj
'
'    K = pNavRow.Value(iNavCode)
'
'    LeftRightSide = SideDef(ptFar, NomDir, ptFNavPrj)
'    AheadBehindSide = SideDef(ptFar, NomDir + 90#, ptFNavPrj)
'    AheadBehindKKhMax = SideDef(ptNear, NomDir + 90#, ptFNavPrj)
'
'    If (K = 0) Or (K = 2) Then
'        If pProxi.ReturnDistance(ptFNavPrj) = 0# Then GoTo NextI
'
'        If (K = 0) Then
'            InterToler = VOR.IntersectingTolerance
'        Else
'            InterToler = NDB.IntersectingTolerance
'        End If
'
'        Side = SideDef(KKhMax.FromPoint, Dir_MinL2MaxR, ptFNavPrj)
'        If Side * LeftRightSide < 0 Then GoTo NextI
'        Side = SideDef(KKhMax.ToPoint, Dir_MinR2MaxL, ptFNavPrj)
'        If Side * LeftRightSide < 0 Then GoTo NextI
'
'        If LeftRightSide > 0 Then
'            If AheadBehindSide < 0 Then
'                Set ptNearD = KKhMin.FromPoint
'                Set ptFarD = KKhMax.ToPoint
'            ElseIf AheadBehindKKhMax < 0 Then
'                Set ptNearD = KKhMin.ToPoint
'                Set ptFarD = KKhMax.ToPoint
'            Else
'                Set ptNearD = KKhMin.ToPoint
'                Set ptFarD = KKhMax.FromPoint
'            End If
'        Else
'            If AheadBehindSide < 0 Then
'                Set ptNearD = KKhMin.ToPoint
'                Set ptFarD = KKhMax.FromPoint
'            ElseIf AheadBehindKKhMax < 0 Then
'                Set ptNearD = KKhMin.FromPoint
'                Set ptFarD = KKhMax.FromPoint
'            Else
'                Set ptNearD = KKhMin.FromPoint
'                Set ptFarD = KKhMax.ToPoint
'            End If
'        End If
'
''        If ERange < ReturnDistanceAsMeter(ptFNavPrj, ptFar) Then GoTo NextI
'
'        AztFar = ReturnAngleAsDegree(ptFNavPrj, ptFarD)
'        AztNear = ReturnAngleAsDegree(ptFNavPrj, ptNearD)
'
'        If SubtractAngles(AztNear, AztFar) < 2# * InterToler Then GoTo NextI
'
'        ReDim ValidNavs(J).ValMax(0)
'        ReDim ValidNavs(J).ValMin(0)
'
'        ValidNavs(J).ValCnt = LeftRightSide
'        If LeftRightSide > 0 Then
'            Set pConstruct = ptTmp
'            pConstruct.ConstructAngleIntersection ptFNavPrj, DegToRad(AztFar + InterToler), PtFAF, DegToRad(NomDir)
'            ValidNavs(J).ValMin(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) - 0.4999999)
'
'            pConstruct.ConstructAngleIntersection ptFNavPrj, DegToRad(AztNear - InterToler), PtFAF, DegToRad(NomDir)
'            ValidNavs(J).ValMax(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
'        Else
'            Set pConstruct = ptTmp
'            pConstruct.ConstructAngleIntersection ptFNavPrj, DegToRad(AztFar - InterToler), PtFAF, DegToRad(NomDir)
'            ValidNavs(J).ValMin(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) - 0.4999999)
'
'            pConstruct.ConstructAngleIntersection ptFNavPrj, DegToRad(AztNear + InterToler), PtFAF, DegToRad(NomDir)
'            ValidNavs(J).ValMax(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
'        End If
'    ElseIf K = 1 Then       '   DME
'        IntrH.Left = minDist
'        IntrH.Right = MaxDist
''=========================================================================================
'        If LeftRightSide > 0 Then
'            If AheadBehindSide < 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMin.ToPoint)
'            Else
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.ToPoint)
'            End If
'        Else
'            If AheadBehindSide < 0 Then
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMin.FromPoint)
'            Else
'                fTmp = ReturnDistanceAsMeter(ptFNavPrj, KKhMax.FromPoint)
'            End If
'        End If
'
'        If fTmp > DME.Range Then GoTo NextI '   Range checking
'
'        If LeftRightSide <> 0 Then
'            Set ptMin23 = New Point
'            Set ptMax23 = New Point
'            Set Construct = ptMin23
'            Construct.ConstructAngleIntersection PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value)
'            Set Construct = ptMax23
'            Construct.ConstructAngleIntersection PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value)
'        Else
'            Set ptMin23 = ptFNavPrj
'            Set ptMax23 = ptFNavPrj
'        End If
''=========================================================================================
'
'        Intr23.Left = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90#) * SideDef(PtFAF, NomDir + 90#, ptMin23)
'        Intr23.Right = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90#) * SideDef(PtFAF, NomDir + 90#, ptMax23)
'
'        fTmp = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir + TrackToler) + 5#
'        D = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir - TrackToler) + 5#
'        If D < fTmp Then D = fTmp
'        fMinDMEDist = (DME.MinimalError + D) / (1# - DME.ErrorScalingUp)
'
'        If CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir + 180#, ptMin23) >= 0 Then
'            CircleVectorIntersect ptFNavPrj, fMinDMEDist, PtFAF, NomDir, ptMax23
'            A = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90#) * SideDef(PtFAF, NomDir + 90#, ptMin23)
'            B = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90#) * SideDef(PtFAF, NomDir + 90#, ptMax23)
'            If Intr23.Left > A Then Intr23.Left = A
'            If Intr23.Right < B Then Intr23.Right = B
'        End If
'
'        IntrRes = IntervalsDifference(IntrH, Intr23)
'        If UBound(IntrRes) < 0 Then GoTo NextI
'
'        Xs = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir + 90#) * SideDef(PtFAF, NomDir + 90#, ptFNavPrj)
'        Ys = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)
''================================== SlantAngle =================================================
'        fTmp = 1# / Tan(DegToRad(DME.SlantAngle))
'        fTmp = fTmp * fTmp
'
'        A = arImDescent_Max.Value * arImDescent_Max.Value - fTmp
'        B = 2# * ((OCH - ptFNavPrj.Z) * arImDescent_Max.Value + Xs * fTmp)
'        C = (OCH - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
'        D = B * B - 4# * A * C
'
'        If D > 0# Then
'            If A > 0 Then
'                Intr55.Left = 0.5 * (-B - Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B + Sqr(D)) / A
'            Else
'                Intr55.Left = 0.5 * (-B + Sqr(D)) / A
'                Intr55.Right = 0.5 * (-B - Sqr(D)) / A
'            End If
'
'            N = UBound(IntrRes)
'            ReDim IntrRes1(-1 To -1)
'
'            For ii = 0 To N
'                IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)
'
'                If UBound(IntrRes1) < 0 Then
'                    IntrRes1 = IntrRes2
'                Else
'                    L = UBound(IntrRes1)
'                    M = UBound(IntrRes2)
'                    If M >= 0 Then
'                        ReDim Preserve IntrRes1(L + M + 1)
'
'                        For jj = 0 To M
'                            IntrRes1(jj + L + 1) = IntrRes2(jj)
'                        Next jj
'                    End If
'                End If
'            Next ii
'
'            IntrRes = IntrRes1
'        End If
'
'        N = UBound(IntrRes)
'
'        ii = 0
'        If N >= 0 Then
'            Do
'                If IntrRes(ii).Left = IntrRes(ii).Right Then
'                    For jj = ii To N - 1
'                        IntrRes(jj) = IntrRes(jj + 1)
'                    Next jj
'                    N = N - 1
'                Else
'                    ii = ii + 1
'                End If
'            Loop While ii < N - 1
'        End If
'
'        ii = 0
'        While ii < N - 1
'            If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
'                IntrRes(ii).Right = IntrRes(ii + 1).Right
'                For jj = ii + 1 To N - 1
'                    IntrRes(jj) = IntrRes(jj + 1)
'                Next jj
'                N = N - 1
'            Else
'                ii = ii + 1
'            End If
'        Wend
'
'        If N < 0 Then GoTo NextI
'
''        ReDim IntrRes1(0 To N)
'        ReDim ValidNavs(J).ValMax(0 To N)
'        ReDim ValidNavs(J).ValMin(0 To N)
'
'        M = 0
''===========================================================================================
'        For ii = 0 To N
'            Set ptNearD = PointAlongPlane(PtFAF, NomDir, IntrRes(ii).Left)
''            Set ptFarD = PointAlongPlane(PtFAF, NomDir, IntrRes(ii).Right)
'
'            Cutter.FromPoint = PointAlongPlane(ptNearD, NomDir - 90#, 3# * R)
'            Cutter.ToPoint = PointAlongPlane(ptNearD, NomDir + 90#, 3# * R)
'
'            Set KKhMinDME = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
'            If KKhMinDME.IsEmpty Then
'                KKhMinDME.FromPoint = ptNearD
'                KKhMinDME.ToPoint = ptNearD
'            ElseIf SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0 Then
'                KKhMinDME.ReverseOrientation
'            End If
'
''            Cutter.FromPoint = PointAlongPlane(ptFarD, NomDir - 90#, 3# * R)
''            Cutter.ToPoint = PointAlongPlane(ptFarD, NomDir + 90#, 3# * R)
''
''            Set KKhMaxDME = pTopoOper.Intersect(Cutter, esriGeometry1Dimension)
''            If KKhMaxDME.IsEmpty Then
''                KKhMaxDME.FromPoint = ptFarD
''                KKhMaxDME.ToPoint = ptFarD
''            ElseIf SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0 Then
''                KKhMaxDME.ReverseOrientation
''            End If
''
''            IntrRes1(M) = CalcDMERange(PtFAF, PtFAF, NomDir, RWY2.Z, 0#, OCH, ptFNavPrj, KKhMinDME, KKhMaxDME)
''            If IntrRes1(M).Left < IntrRes1(M).Right Then
''                M = M + 1
''                ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
''            End If
'            If IntrRes(M).Left < IntrRes(M).Right Then
'                ValidNavs(J).ValMax(M) = Round(IntrRes(M).Right - 0.4999999)
'                ValidNavs(J).ValMin(M) = Round(IntrRes(M).Left + 0.4999999)
'                M = M + 1
'                ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90#, ptFNavPrj)
'            End If
'        Next ii
'
'        M = M - 1
'        If M < 0 Then
'            GoTo NextI
'        End If
'
'        If M > 0 Then ValidNavs(J).ValCnt = 0
'        ReDim Preserve ValidNavs(J).ValMax(M)
'        ReDim Preserve ValidNavs(J).ValMin(M)
'
''        If M > 0 Then
''            CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir, ptTmp
''            ValidNavs(J).ValMin(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) - 0.4999999)
''
''            CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir, ptTmp
''            ValidNavs(J).ValMax(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
''
''            CircleVectorIntersect ptFNavPrj, IntrRes1(1).Left, PtFAF, NomDir, ptTmp
''            ValidNavs(J).ValMin(1) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
''
''            CircleVectorIntersect ptFNavPrj, IntrRes1(1).Right, PtFAF, NomDir, ptTmp
''            ValidNavs(J).ValMax(1) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
''        Else
'''            If ValidNavs(J).ValCnt < 0 Then
''            If AheadBehindSide < 0 Then
''                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir, ptTmp
''                ValidNavs(J).ValMin(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) - 0.4999999)
''
''                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir, ptTmp
''                ValidNavs(J).ValMax(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
''            Else
''                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir + 180#, ptTmp
''                ValidNavs(J).ValMin(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) - 0.4999999)
''
''                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir + 180#, ptTmp
''                ValidNavs(J).ValMax(0) = Round(ReturnDistanceAsMeter(ptTmp, PtFAF) + 0.4999999)
''            End If
''        End If
'    Else
'        GoTo NextI
'    End If
'
'    ValidNavs(J).Name = pNavRow.Value(iNavCall)
'    ValidNavs(J).NavTypeName = pNavRow.Value(iNavType)
'    ValidNavs(J).Range = pNavRow.Value(iNavRange) * 1000#
'    ValidNavs(J).NavTypeCode = K
'    Set ValidNavs(J).Ptgeo = ptFNav
'    Set ValidNavs(J).ptPrj = ptFNavPrj
'
'    J = J + 1
'NextI:
'    Set pNavRow = pCursor.NextRow
'Loop
'
'If J > 0 Then
'    ReDim Preserve ValidNavs(J - 1)
'Else
'    ReDim ValidNavs(-1 To -1)
'End If
'
'GetValidMAPtNavs = ValidNavs
'
'End Function

Public Function PolygonIntersection(pPoly1 As Polygon, pPoly2 As Polygon) As Polygon
Dim pTopo As ITopologicalOperator2
Dim pTmpPoly0 As Polygon
Dim pTmpPoly1 As Polygon

Set pTopo = pPoly2
pTopo.IsKnownSimple = False
pTopo.Simplify

Set pTopo = pPoly1
pTopo.IsKnownSimple = False
pTopo.Simplify

On Error Resume Next
    Set PolygonIntersection = pTopo.Intersect(pPoly2, esriGeometry2Dimension)
If Err.Number = 0 Then Exit Function
    Err.Clear
    Set pTmpPoly0 = pTopo.Union(pPoly2)
'    DrawPolygon pTmpPoly0, 0
    Set pTmpPoly1 = pTopo.SymmetricDifference(pPoly2)
'    DrawPolygon pTmpPoly1, RGB(255, 0, 255)
    Set pTopo = pTmpPoly0
    Set PolygonIntersection = pTopo.Difference(pTmpPoly1)
If Err.Number = 0 Then Exit Function
    Set PolygonIntersection = pPoly2
End Function

Function RemoveFars(pPolygon As Polygon, pPoint As Point) As Polygon
Dim Geocollect As IGeometryCollection
Dim lCollect As IGeometryCollection
Dim pProxi As IProximityOperator
Dim OutDist As Double
Dim tmpDist As Double
Dim pClone As IClone
Dim i As Long
Dim N As Long

Set pClone = pPolygon
Set RemoveFars = pClone.Clone
Set Geocollect = RemoveFars
N = Geocollect.GeometryCount
Set lCollect = New Polygon

If N > 1 Then
    Set pProxi = pPoint
    OutDist = 20000000000#

    For i = 0 To N - 1
        lCollect.AddGeometry Geocollect.Geometry(i)

        tmpDist = pProxi.ReturnDistance(lCollect)
        If OutDist > tmpDist Then
            OutDist = tmpDist
        End If
        lCollect.RemoveGeometries 0, 1
    Next i

    i = 0
    While i < N
        lCollect.AddGeometry Geocollect.Geometry(i)
        tmpDist = pProxi.ReturnDistance(lCollect)
        If OutDist < tmpDist Then
            Geocollect.RemoveGeometries i, 1
            N = N - 1
        Else
            i = i + 1
        End If
        lCollect.RemoveGeometries 0, 1
    Wend
End If

End Function

Function RemoveSmalls(pPolygon As Polygon) As Polygon
Dim Geocollect As IGeometryCollection
Dim OutArea As Double
Dim pClone As IClone
Dim pArea As IArea
Dim i As Long
Dim N As Long

Set pClone = pPolygon
Set RemoveSmalls = pClone.Clone
Set Geocollect = RemoveSmalls
N = Geocollect.GeometryCount

If N > 1 Then
    OutArea = 0#

    For i = 0 To N - 1
        Set pArea = Geocollect.Geometry(i)
        If pArea.Area > OutArea Then
            OutArea = pArea.Area
        End If
    Next i

    i = 0
    While i < N
        Set pArea = Geocollect.Geometry(i)
        If pArea.Area < OutArea Then
            Geocollect.RemoveGeometries i, 1
            N = N - 1
        Else
            i = i + 1
        End If
    Wend
End If

End Function

Function RemoveHoles(pPolygon As Polygon) As Polygon
Dim pTopoOper As ITopologicalOperator2
Dim NewPolygon As IGeometryCollection
Dim pInteriorRing As IRing
Dim pClone As IClone
Dim i As Long

Set pClone = pPolygon
'Set NewPolygon = pClone.Clone

Set RemoveHoles = pClone.Clone
Set NewPolygon = RemoveHoles

i = 0
While i < NewPolygon.GeometryCount
    Set pInteriorRing = NewPolygon.Geometry(i)
    If Not pInteriorRing.IsExterior Then
        NewPolygon.RemoveGeometries i, 1
    Else
        i = i + 1
    End If
Wend

Set pTopoOper = NewPolygon
pTopoOper.IsKnownSimple = False
pTopoOper.Simplify
'Set RemoveHoles = NewPolygon

End Function

Function RemoveAgnails(pPolygon As Polygon) As Polygon
Dim i As Long
Dim j As Long
Dim K As Long
Dim L As Long

Dim N As Long
Dim Rings As Long

Dim dl As Double
Dim dX0 As Double
Dim dY0 As Double
Dim dX1 As Double
Dim dY1 As Double

Dim Geocollect As IGeometryCollection
Dim pTopo As ITopologicalOperator2
Dim pPoly As IPointCollection
Dim pPGone As IPolygon2
Dim pClone As IClone

Set pClone = pPolygon
Set pPoly = pClone.Clone

Set pPGone = pPoly
pPGone.Close

N = pPoly.PointCount - 1

If N <= 3 Then
    Set RemoveAgnails = pPoly
    Exit Function
End If

pPoly.RemovePoints N, 1

j = 0
Do While j < N
    If N < 4 Then Exit Do

    K = (j + 1) Mod N
    L = (j + 2) Mod N

    dX0 = pPoly.Point(K).X - pPoly.Point(j).X
    dY0 = pPoly.Point(K).y - pPoly.Point(j).y
    
    dX1 = pPoly.Point(L).X - pPoly.Point(K).X
    dY1 = pPoly.Point(L).y - pPoly.Point(K).y

    dl = dX1 * dX1 + dY1 * dY1

    If dl < 0.00001 Then
        pPoly.RemovePoints K, 1
        N = N - 1
        If j >= N Then j = N - 1
    ElseIf (dY0 <> 0#) Then
        If dY1 <> 0# Then
            If Abs(dX0 / dY0 - dX1 / dY1) < 0.0001 Then
                pPoly.RemovePoints K, 1
                N = N - 1
                j = (j - 2) Mod N
                If j < 0 Then j = 0 'J = J + N
            Else
                 j = j + 1
           End If
        Else
            j = j + 1
        End If
    ElseIf (dX0 <> 0#) Then
        If dX1 <> 0# Then
            If Abs(dY0 / dX0 - dY1 / dX1) < 0.0001 Then
                pPoly.RemovePoints K, 1
                N = N - 1
                j = (j - 2) Mod N
                If j < 0 Then j = 0 'J = J + N
            Else
                j = j + 1
            End If
        Else
            j = j + 1
        End If
    Else
        j = j + 1
    End If
Loop

Set pPGone = pPoly
pPGone.Close

Set pTopo = pPoly
pTopo.IsKnownSimple = False
pTopo.Simplify

Set RemoveAgnails = pPoly

End Function

Public Function ReArrangePolygon(pPolygon As IPointCollection, PtDerL As IPoint, CLDir As Double, Optional bFlag As Boolean = False) As IPointCollection
Dim i As Long
Dim j As Long

Dim N As Long
'Dim Nl As Long
'Dim Nr As Long
Dim iStart As Long
Dim side As Long
Dim bDone As Boolean

Dim dl As Double
Dim dm As Double

Dim dX0 As Double
Dim dY0 As Double

Dim dX1 As Double
Dim dY1 As Double
Dim pPoly As IPointCollection

Set pPoly = New Polyline
pPoly.AddPointCollection pPolygon

pPoly.RemovePoints 0, 1

N = pPoly.PointCount

dm = Point2LineDistancePrj(pPoly.Point(0), PtDerL, CLDir + 90#) * SideDef(PtDerL, CLDir, pPoly.Point(0))

iStart = -1
If dm < 0 Then iStart = 0

For i = 0 To N - 1
    dl = Point2LineDistancePrj(pPoly.Point(i), PtDerL, CLDir + 90#) * SideDef(PtDerL, CLDir, pPoly.Point(i))
    If (dl < 0#) And ((dl > dm) Or (dm >= 0#)) Then
        dm = dl
        iStart = i
    End If
Next

If bFlag Then
    If iStart = 0 Then
        iStart = N - 1
    Else
        iStart = iStart - 1
    End If
End If

dX0 = pPoly.Point(1).X - pPoly.Point(0).X
dY0 = pPoly.Point(1).y - pPoly.Point(0).y
i = 1
Do While i < N
    j = (i + 1) Mod (N - 1)
    dX1 = pPoly.Point(j).X - pPoly.Point(i).X
    dY1 = pPoly.Point(j).y - pPoly.Point(i).y
    dl = ReturnDistanceAsMeter(pPoly.Point(j), pPoly.Point(i))

    If dl < distEps Then
        pPoly.RemovePoints i, 1
        N = N - 1
        j = (i + 1) Mod N
        If i <= iStart Then
            iStart = iStart - 1
        End If
        dX1 = dX0
        dY1 = dY0
    ElseIf (dY0 <> 0#) And (i <> iStart) Then
        If dY1 <> 0# Then
            If Abs(Abs(dX0 / dY0) - Abs(dX1 / dY1)) < 0.00001 Then
                pPoly.RemovePoints i, 1
                N = N - 1
                j = (i + 1) Mod N
                If i <= iStart Then
                    iStart = iStart - 1
                End If
                dX1 = dX0
                dY1 = dY0
            Else
                i = i + 1
            End If
        Else
            i = i + 1
        End If
    ElseIf (dX0 <> 0#) And (i <> iStart) Then
        If dX1 <> 0# Then
            If Abs(Abs(dY0 / dX0) - Abs(dY1 / dX1)) < 0.00001 Then
                pPoly.RemovePoints i, 1
                N = N - 1
                j = (i + 1) Mod N
                If i <= iStart Then
                    iStart = iStart - 1
                End If
                dX1 = dX0
                dY1 = dY0
            Else
                i = i + 1
            End If
        Else
            i = i + 1
        End If
    Else
        i = i + 1
    End If
    dX0 = dX1
    dY0 = dY1
Loop

N = pPoly.PointCount
Set ReArrangePolygon = New Polygon

For i = N - 1 To 0 Step -1
    j = ((i + iStart) Mod N)
    ReArrangePolygon.AddPoint pPoly.Point(j)
Next

'DrawPolygon ReArrangePolygon, 255

'Set pPoly = New Polyline
'pPoly.re
'pPoly.ReverseOrientation

End Function


'Function CalcTrajectoryFromMultiPoint(MultiPoint As IPointCollection) As IPointCollection
'
'Dim ptConstr As IConstructPoint
'Dim TmpLine As IPolyline
'
'Dim FromPt As IPoint
'Dim CntPt As IPoint
'Dim ToPt As IPoint
'
'Dim CenAng As Double
'Dim fTmp As Double
'Dim fE As Double
'
'Dim side As Long
'Dim I As Long
'Dim N As Long
'
'Set CntPt = New Point
'Set ptConstr = CntPt
'Set CalcTrajectoryFromMultiPoint = New Polyline
'Dim DegToRadValue As Double
'DegToRadValue = PI / 180#
'fE = DegToRadValue * 0.5
'
'N = MultiPoint.PointCount - 2
'CalcTrajectoryFromMultiPoint.AddPoint MultiPoint.Point(0)
'
'For I = 0 To N
'    Set FromPt = MultiPoint.Point(I)
'    Set ToPt = MultiPoint.Point(I + 1)
'    fTmp = DegToRadValue * (FromPt.M - ToPt.M)
'
'    If (Abs(Sin(fTmp)) <= fE) And (Cos(fTmp) > 0#) Then
'        CalcTrajectoryFromMultiPoint.AddPoint ToPt
'    Else
'        If Abs(Sin(fTmp)) > fE Then
'            ptConstr.ConstructAngleIntersection FromPt, DegToRadValue * (Modulus(FromPt.M + 90#, 360#)), ToPt, DegToRadValue * (Modulus(ToPt.M + 90#, 360#))
'        Else
'            CntPt.PutCoords 0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.y + ToPt.y)
'        End If
''        DrawPoint CntPt, RGB(0, 0, 255)
'        side = SideDef(FromPt, FromPt.M, ToPt)
'        CalcTrajectoryFromMultiPoint.AddPointCollection CreateArcPrj(CntPt, FromPt, ToPt, -side)
'    End If
''    DrawPoint CntPt, RGB(0, 255, 0)
'Next
'
'End Function

Function CalcTrajectoryFromMultiPoint(MultiPoint As IPointCollection) As IPointCollection

'Dim ii As Long
'
'For ii = 0 To MultiPoint.PointCount - 1
'    MessageBox FrmManevreN.hWnd, "Drawing point N:" + CStr(ii), "", 0
'    DrawPoint MultiPoint.Point(ii)
'Next ii

Set CalcTrajectoryFromMultiPoint = CalcTrajectoryFromMultiPoint_ByPart(MultiPoint)
Exit Function

Dim ptConstr As IConstructPoint
Dim TmpLine As IPolyline

Dim FromPt As IPoint
Dim CntPt As IPoint
Dim ToPt As IPoint

Dim CenAng As Double
Dim fTmp As Double
Dim fE As Double

Dim side As Long
Dim i As Long
Dim N As Long

Set CntPt = New Point
Set ptConstr = CntPt
Set CalcTrajectoryFromMultiPoint = New Polyline
Dim DegToRadValue As Double
DegToRadValue = Pi / 180#
fE = DegToRadValue * 0.5

N = MultiPoint.PointCount - 2
CalcTrajectoryFromMultiPoint.AddPoint MultiPoint.Point(0)

For i = 0 To N
    Set FromPt = MultiPoint.Point(i)
    Set ToPt = MultiPoint.Point(i + 1)
    fTmp = DegToRadValue * (FromPt.M - ToPt.M)

    If (Abs(Sin(fTmp)) <= fE) And (Cos(fTmp) > 0#) Then
        CalcTrajectoryFromMultiPoint.AddPoint ToPt
    Else
        If Abs(Sin(fTmp)) > fE Then
            ptConstr.ConstructAngleIntersection FromPt, DegToRadValue * (Modulus(FromPt.M + 90#, 360#)), ToPt, DegToRadValue * (Modulus(ToPt.M + 90#, 360#))
        Else
            CntPt.PutCoords 0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.y + ToPt.y)
        End If
'        DrawPoint CntPt, RGB(0, 0, 255)
        side = SideDef(FromPt, FromPt.M, ToPt)
        CalcTrajectoryFromMultiPoint.AddPointCollection CreateArcPrj(CntPt, FromPt, ToPt, -side)
    End If
'    DrawPoint CntPt, RGB(0, 255, 0)
Next

End Function

Function CalcTrajectoryFromMultiPoint_ByPart(MultiPoint As IPointCollection) As IPolyline

    On Error GoTo EH

    Dim ptConstr As IConstructPoint
    Dim TmpLine As IPolyline
    Dim pGeometryCollection As IGeometryCollection

    Dim FromPt As IPoint
    Dim CntPt As IPoint
    Dim ToPt As IPoint

    Dim CenAng As Double
    Dim fTmp As Double
    Dim fE As Double

    Dim side As Long
    Dim i As Long
    Dim N As Long

    Set CntPt = New Point
    Set ptConstr = CntPt
    Dim DegToRadValue As Double
    DegToRadValue = Pi / 180#
    fE = DegToRadValue * 0.5

    N = MultiPoint.PointCount - 2
    Set pGeometryCollection = New Polyline

    Dim pPath As ISegmentCollection
    Dim pPolyL As IGeometryCollection
    
    Set pPolyL = New Polyline

    For i = 0 To N
        Set pPath = New Path
        Set FromPt = MultiPoint.Point(i)
        Set ToPt = MultiPoint.Point(i + 1)
        fTmp = DegToRadValue * (FromPt.M - ToPt.M)

        If (Abs(Sin(fTmp)) <= fE) And (Cos(fTmp) > 0#) Then
            pPath.AddSegment GetLine(FromPt, ToPt)
        Else
            If Abs(Sin(fTmp)) > fE Then
                ptConstr.ConstructAngleIntersection FromPt, DegToRadValue * (Modulus(FromPt.M + 90#, 360#)), ToPt, DegToRadValue * (Modulus(ToPt.M + 90#, 360#))
            Else
                CntPt.PutCoords 0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.y + ToPt.y)
            End If
            side = SideDef(FromPt, FromPt.M, ToPt)
            'pPath.AddSegment CreateArcPrj_ByArc(CntPt, FromPt, ToPt, side)
            Set pPath = CreateArcPrj2(CntPt, FromPt, ToPt, -side)
        End If
        pPolyL.AddGeometry pPath
    Next
        
    Set CalcTrajectoryFromMultiPoint_ByPart = pPolyL
EH:
    If Err.Number <> 0 Then
        ShowError Err.Description
    End If

End Function

Public Function GetLine(ptFrom As IPoint, ptTo As IPoint) As ILine
    Dim pLine As ILine
    Set pLine = New esriGeometry.Line
    pLine.PutCoords ptFrom, ptTo
    
    Set GetLine = pLine
End Function

Public Function CreateBasePoints(pPolygone As IPointCollection, K1K1 As IPolyline, lDepDir As Double, lTurnDir As Long) As IPointCollection
Dim tmpPoly As IPointCollection
Dim bFlg As Boolean
Dim i As Long
Dim N As Long
Dim M As Long
Dim side As Long

bFlg = False
N = pPolygone.PointCount
Set tmpPoly = New Polyline
Set CreateBasePoints = New Polygon

If lTurnDir > 0 Then
    For i = 0 To N - 1
        side = SideDef(K1K1.FromPoint, lDepDir + 90#, pPolygone.Point(i))
        If (side < 0) Then
            If bFlg Then
                CreateBasePoints.AddPoint pPolygone.Point(i)
            Else
                tmpPoly.AddPoint pPolygone.Point(i)
            End If
        ElseIf Not bFlg Then
            bFlg = True
            CreateBasePoints.AddPoint K1K1.FromPoint
            CreateBasePoints.AddPoint K1K1.ToPoint
        End If
    Next
Else
    For i = N - 1 To 0 Step -1
        side = SideDef(K1K1.FromPoint, lDepDir + 90#, pPolygone.Point(i))
        If (side < 0) Then
            If bFlg Then
                CreateBasePoints.AddPoint pPolygone.Point(i)
            Else
                tmpPoly.AddPoint pPolygone.Point(i)
            End If
        ElseIf Not bFlg Then
            bFlg = True
            CreateBasePoints.AddPoint K1K1.ToPoint
            CreateBasePoints.AddPoint K1K1.FromPoint
        End If
    Next
End If

'DrawPolygon CreateBasePoints, 0
CreateBasePoints.AddPointCollection tmpPoly
'DrawPolygon CreateBasePoints, 0

End Function

Public Function TurnToFixPrj(PtSt As IPoint, TurnR As Double, turnDir As Long, FixPnt As IPoint, OutDir As Double) As IPointCollection

Dim ptCnt As IPoint
Dim ptTmp As IPoint
Dim Pt1 As IPoint
'Dim pt2 As IPoint
Dim DeltaAngle As Double
Dim DirFx2Cnt As Double
Dim DistFx2Cnt As Double
Dim dirCur As Double

dirCur = PtSt.M

Set TurnToFixPrj = New MultiPoint
Set ptCnt = PointAlongPlane(PtSt, dirCur + 90# * turnDir, TurnR)


DistFx2Cnt = ReturnDistanceAsMeter(ptCnt, FixPnt)

If DistFx2Cnt < TurnR Then
    TurnR = DistFx2Cnt
    Exit Function
End If

DirFx2Cnt = ReturnAngleAsDegree(ptCnt, FixPnt)

OutDir = DirFx2Cnt

DeltaAngle = -RadToDeg(ArcCos(TurnR / DistFx2Cnt)) * turnDir

Set Pt1 = PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR)

Pt1.M = ReturnAngleAsDegree(Pt1, FixPnt)

TurnToFixPrj.AddPoint PtSt
TurnToFixPrj.AddPoint Pt1

End Function

Public Function ReturnPolygonPartAsPolyline(pPolygon As IPointCollection, PtDerL As IPoint, CLDir As Double, Turn As Long) As IPointCollection
Dim i As Long
Dim N As Long
Dim side As Long
Dim pLine As IPolyline
Dim pTmpPoly As IPointCollection

'Set pTmpPoly = ReArrangePolygon(pPolygon, PtDerL, CLDir)
Set pTmpPoly = RemoveAgnails(pPolygon)
Set pTmpPoly = ReArrangePolygon(pTmpPoly, PtDerL, CLDir)

Set ReturnPolygonPartAsPolyline = New Polyline
N = pTmpPoly.PointCount - 1

For i = 0 To N
    side = SideDef(PtDerL, CLDir, pTmpPoly.Point(i))
    If side = Turn Then
        ReturnPolygonPartAsPolyline.AddPoint pTmpPoly.Point(i)
    End If
Next

If Turn < 0 Then
    Set pLine = ReturnPolygonPartAsPolyline
    pLine.ReverseOrientation
End If

End Function

'==========================================================================================================================================================
'Public Sub CreateILS23Planes(ptLHprj As IPoint, ArDir As Double, H As Double, ILSPlanes() As D3DPolygone)
'
'Dim pTopo As ITopologicalOperator2
'Dim pt1 As IPoint
'Dim pt2 As IPoint
'Dim pt3 As IPoint
'Dim Pt4 As IPoint
'
'Dim I As Long
'Dim N As Long
'
'Set pt1 = PointAlongPlane(ptLHprj, ArDir + 180#, (H - 27# + 1.2) * 50#)
'Set pt2 = PointAlongPlane(ptLHprj, ArDir + 180#, 60#)
'Set pt3 = PointAlongPlane(ptLHprj, ArDir, 1800#)
'Set Pt4 = PointAlongPlane(ptLHprj, ArDir, (H + 59.94) * 30.03)
'
'pt2.Z = 0#
'pt3.Z = 0#
'
'N = UBound(ILSPlanes)
'
'For I = 0 To N
'    Set ILSPlanes(I).Poly = New Polygon
'Next I
'
''=================================================================
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(pt2, ArDir - 90#, 60#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(pt2, ArDir + 90#, 60#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(pt3, ArDir + 90#, 60#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(pt3, ArDir - 90#, 60#)
'
'Set ILSPlanes(0).Plane.Pt = pt2
'ILSPlanes(0).Plane.X = 0#
'ILSPlanes(0).Plane.Y = 0#
'ILSPlanes(0).Plane.Z = 1#
'
'ILSPlanes(0).Plane.A = 0#
'ILSPlanes(0).Plane.B = 0#
'ILSPlanes(0).Plane.C = -1#
'ILSPlanes(0).Plane.D = 0#
'
''=================================================================
'ILSPlanes(1).Poly.AddPoint PointAlongPlane(pt1, ArDir - 90#, 60#)
'ILSPlanes(1).Poly.AddPoint PointAlongPlane(pt1, ArDir + 90#, 60#)
'ILSPlanes(1).Poly.AddPoint ILSPlanes(0).Poly.Point(1)
'ILSPlanes(1).Poly.AddPoint ILSPlanes(0).Poly.Point(0)
'
'Set ILSPlanes(1).Plane.Pt = pt2
'ILSPlanes(1).Plane.A = 0.02
'ILSPlanes(1).Plane.B = 0#
'ILSPlanes(1).Plane.C = -1#
'ILSPlanes(1).Plane.D = -1.2
'
''=================================================================
'ILSPlanes(2).Poly.AddPoint ILSPlanes(1).Poly.Point(1)
'ILSPlanes(2).Poly.AddPoint PointAlongPlane(pt1, ArDir + 90#, 3.003 * (21.18 - 1.2 + 27#))
'ILSPlanes(2).Poly.AddPoint PointAlongPlane(pt2, ArDir + 90#, 3.003 * (H + 21.18 - 0.02 * 60#))
'ILSPlanes(2).Poly.AddPoint ILSPlanes(1).Poly.Point(2)
'
'Set ILSPlanes(2).Plane.Pt = ILSPlanes(2).Poly.Point(3)
'ILSPlanes(2).Plane.Pt.Z = 0#
'
'ILSPlanes(2).Plane.A = 0.02
'ILSPlanes(2).Plane.B = -0.333
'ILSPlanes(2).Plane.C = -1#
'ILSPlanes(2).Plane.D = -21.18
'
''=================================================================
'ILSPlanes(3).Poly.AddPoint ILSPlanes(0).Poly.Point(1)
'ILSPlanes(3).Poly.AddPoint ILSPlanes(2).Poly.Point(2)
'ILSPlanes(3).Poly.AddPoint PointAlongPlane(Pt4, ArDir + 90#, 3.003 * (H + 19.98))
'ILSPlanes(3).Poly.AddPoint ILSPlanes(0).Poly.Point(2)
'
'Set ILSPlanes(3).Plane.Pt = ILSPlanes(3).Poly.Point(3)
'ILSPlanes(3).Plane.Pt.Z = 0#
'
'ILSPlanes(3).Plane.A = 0#
'ILSPlanes(3).Plane.B = -0.333
'ILSPlanes(3).Plane.C = -1#
'ILSPlanes(3).Plane.D = -19.98
'
''=================================================================
'ILSPlanes(4).Poly.AddPoint ILSPlanes(3).Poly.Point(3)
'ILSPlanes(4).Poly.AddPoint ILSPlanes(3).Poly.Point(2)
'ILSPlanes(4).Poly.AddPoint PointAlongPlane(Pt4, ArDir - 90#, 3.003 * (H + 19.98))
'ILSPlanes(4).Poly.AddPoint ILSPlanes(0).Poly.Point(3)
'
'Set ILSPlanes(4).Plane.Pt = ILSPlanes(4).Poly.Point(3)
'ILSPlanes(4).Plane.Pt.Z = 0#
'
'ILSPlanes(4).Plane.A = -0.0333
'ILSPlanes(4).Plane.B = 0#
'ILSPlanes(4).Plane.C = -1#
'ILSPlanes(4).Plane.D = -59.94
'
''=================================================================
'ILSPlanes(5).Poly.AddPoint ILSPlanes(0).Poly.Point(0)
'ILSPlanes(5).Poly.AddPoint ILSPlanes(0).Poly.Point(3)
'ILSPlanes(5).Poly.AddPoint ILSPlanes(4).Poly.Point(2)
'ILSPlanes(5).Poly.AddPoint PointAlongPlane(pt2, ArDir - 90#, 3.003 * (H + 21.18 - 0.02 * 60#))
'
'Set ILSPlanes(5).Plane.Pt = ILSPlanes(5).Poly.Point(0)
'ILSPlanes(5).Plane.Pt.Z = 0#
'
'ILSPlanes(5).Plane.A = 0#
'ILSPlanes(5).Plane.B = 0.333
'ILSPlanes(5).Plane.C = -1#
'ILSPlanes(5).Plane.D = -19.98
'
''=================================================================
'ILSPlanes(6).Poly.AddPoint ILSPlanes(1).Poly.Point(0)
'ILSPlanes(6).Poly.AddPoint ILSPlanes(1).Poly.Point(3)
'ILSPlanes(6).Poly.AddPoint ILSPlanes(5).Poly.Point(3)
'ILSPlanes(6).Poly.AddPoint PointAlongPlane(pt1, ArDir - 90#, 3.003 * (21.18 - 1.2 + 27#))
'
'Set ILSPlanes(6).Plane.Pt = ILSPlanes(6).Poly.Point(1)
'ILSPlanes(6).Plane.Pt.Z = 0#
'
'ILSPlanes(6).Plane.A = 0.02
'ILSPlanes(6).Plane.B = 0.333
'ILSPlanes(6).Plane.C = -1#
'ILSPlanes(6).Plane.D = -21.18
'
''=================================================================
'For I = 0 To N - 1
'    Set pTopo = ILSPlanes(N).Poly
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'
'    Set pTopo = ILSPlanes(I).Poly
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'    Set ILSPlanes(N).Poly = pTopo.Union(ILSPlanes(N).Poly)
''   DrawPolygon pTopo
'Next I
'
'End Sub
'
'Public Sub CreateILSPlanes(ptLHprj As IPoint, ArDir As Double, ILSPlanes() As D3DPolygone)
'
'Dim pTopo As ITopologicalOperator2
'Dim ptD As IPoint
'Dim ptH As IPoint
'Dim PtG As IPoint
'Dim ptF As IPoint
'Dim ptE As IPoint
'Dim ptA As IPoint
'
'Dim I As Long
'Dim N As Long
'Dim fAlpha As Double
'Dim fAztPl2 As Double
'Dim fDistPl2 As Double
'
'Set ptD = PointAlongPlane(ptLHprj, ArDir + 180#, 12660#)
'Set ptH = PointAlongPlane(ptLHprj, ArDir + 180#, 3060#)
'Set PtG = PointAlongPlane(ptLHprj, ArDir + 180, 60#)
'Set ptF = PointAlongPlane(ptLHprj, ArDir, 900#)
'Set ptE = PointAlongPlane(ptLHprj, ArDir, 2700#)
'Set ptA = PointAlongPlane(ptLHprj, ArDir, 12900#)
'
'ptF.Z = 0#
'PtG.Z = 0#
'
'N = UBound(ILSPlanes)
'
'For I = 0 To N
'    Set ILSPlanes(I).Poly = New Polygon
'Next I
'
''=================================================================
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(ptF, ArDir - 90#, 150#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(PtG, ArDir - 90#, 150#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(PtG, ArDir + 90#, 150#)
'ILSPlanes(0).Poly.AddPoint PointAlongPlane(ptF, ArDir + 90#, 150#)
'
'Set ILSPlanes(0).Plane.Pt = ptF
'ILSPlanes(0).Plane.X = 0#
'ILSPlanes(0).Plane.Y = 0#
'ILSPlanes(0).Plane.Z = 1#
'
'ILSPlanes(0).Plane.A = 0#
'ILSPlanes(0).Plane.B = 0#
'ILSPlanes(0).Plane.C = -1#
'ILSPlanes(0).Plane.D = 0#
'
''=================================================================
'ILSPlanes(1).Poly.AddPoint ILSPlanes(0).Poly.Point(1)
'ILSPlanes(1).Poly.AddPoint PointAlongPlane(ptH, ArDir - 90#, 600#)
'ILSPlanes(1).Poly.AddPoint PointAlongPlane(ptH, ArDir + 90#, 600#)
'ILSPlanes(1).Poly.AddPoint ILSPlanes(0).Poly.Point(2)
'
'Set ILSPlanes(1).Plane.Pt = ptH
'ILSPlanes(1).Plane.A = 0.02
'ILSPlanes(1).Plane.B = 0#
'ILSPlanes(1).Plane.C = -1#
'ILSPlanes(1).Plane.D = -1.2
'
''=================================================================
'ILSPlanes(2).Poly.AddPoint ILSPlanes(1).Poly.Point(1)
'ILSPlanes(2).Poly.AddPoint PointAlongPlane(ptD, ArDir - 90#, 2040#)
'ILSPlanes(2).Poly.AddPoint PointAlongPlane(ptD, ArDir + 90#, 2040#)
'ILSPlanes(2).Poly.AddPoint ILSPlanes(1).Poly.Point(2)
'
'Set ILSPlanes(2).Plane.Pt = ptD
'ILSPlanes(2).Plane.Pt.Z = 0#
'
'ILSPlanes(2).Plane.A = 0.025
'ILSPlanes(2).Plane.B = 0#
'ILSPlanes(2).Plane.C = -1#
'ILSPlanes(2).Plane.D = -16.5
'
''=================================================================
'ILSPlanes(3).Poly.AddPoint ILSPlanes(2).Poly.Point(3)
'ILSPlanes(3).Poly.AddPoint ILSPlanes(2).Poly.Point(2)
'ILSPlanes(3).Poly.AddPoint PointAlongPlane(ptH, ArDir + 90#, 2278#)
'
'Set ILSPlanes(3).Plane.Pt = ptH
'ILSPlanes(3).Plane.Pt.Z = 0#
'
'ILSPlanes(3).Plane.A = 0.00355
'ILSPlanes(3).Plane.B = -0.143
'ILSPlanes(3).Plane.C = -1#
'ILSPlanes(3).Plane.D = -36.66
'
''=================================================================
'ILSPlanes(11).Poly.AddPoint ILSPlanes(2).Poly.Point(0)
'ILSPlanes(11).Poly.AddPoint PointAlongPlane(ptH, ArDir - 90#, 2278#)
'ILSPlanes(11).Poly.AddPoint ILSPlanes(2).Poly.Point(1)
'
'Set ILSPlanes(11).Plane.Pt = ptH
'
'ILSPlanes(11).Plane.A = 0.00355
'ILSPlanes(11).Plane.B = 0.143
'ILSPlanes(11).Plane.C = -1#
'ILSPlanes(11).Plane.D = -36.66
'
''=================================================================
'ILSPlanes(4).Poly.AddPoint ILSPlanes(1).Poly.Point(3)
'ILSPlanes(4).Poly.AddPoint ILSPlanes(1).Poly.Point(2)
'ILSPlanes(4).Poly.AddPoint ILSPlanes(3).Poly.Point(2)
'ILSPlanes(4).Poly.AddPoint PointAlongPlane(PtG, ArDir + 90#, 2248#)
'
'Set ILSPlanes(4).Plane.Pt = PtG
'
'ILSPlanes(4).Plane.A = -0.00145
'ILSPlanes(4).Plane.B = -0.143
'ILSPlanes(4).Plane.C = -1#
'ILSPlanes(4).Plane.D = -21.36
'
''=================================================================
'ILSPlanes(10).Poly.AddPoint ILSPlanes(1).Poly.Point(0)
'ILSPlanes(10).Poly.AddPoint PointAlongPlane(PtG, ArDir - 90#, 2248#)
'ILSPlanes(10).Poly.AddPoint ILSPlanes(11).Poly.Point(1)
'ILSPlanes(10).Poly.AddPoint ILSPlanes(1).Poly.Point(1)
'
'Set ILSPlanes(10).Plane.Pt = PtG
'
'ILSPlanes(10).Plane.A = -0.00145
'ILSPlanes(10).Plane.B = 0.143
'ILSPlanes(10).Plane.C = -1#
'ILSPlanes(10).Plane.D = -21.36
'
''=================================================================
'ILSPlanes(5).Poly.AddPoint ILSPlanes(0).Poly.Point(3)
'ILSPlanes(5).Poly.AddPoint ILSPlanes(0).Poly.Point(2)
'ILSPlanes(5).Poly.AddPoint ILSPlanes(4).Poly.Point(3)
'ILSPlanes(5).Poly.AddPoint PointAlongPlane(ptE, ArDir + 90#, 2248#)
'ILSPlanes(5).Poly.AddPoint PointAlongPlane(ptE, ArDir + 90#, 465#)
'
'Set ILSPlanes(5).Plane.Pt = ptE
'
'ILSPlanes(5).Plane.A = 0#
'ILSPlanes(5).Plane.B = -0.143
'ILSPlanes(5).Plane.C = -1#
'ILSPlanes(5).Plane.D = -21.45
'
''=================================================================
'ILSPlanes(9).Poly.AddPoint PointAlongPlane(ptE, ArDir - 90#, 465#)
'ILSPlanes(9).Poly.AddPoint PointAlongPlane(ptE, ArDir - 90#, 2248#)
'ILSPlanes(9).Poly.AddPoint ILSPlanes(10).Poly.Point(1)
'ILSPlanes(9).Poly.AddPoint ILSPlanes(0).Poly.Point(1)
'ILSPlanes(9).Poly.AddPoint ILSPlanes(0).Poly.Point(0)
'
'Set ILSPlanes(9).Plane.Pt = ptE
'
'ILSPlanes(9).Plane.A = 0#
'ILSPlanes(9).Plane.B = 0.143
'ILSPlanes(9).Plane.C = -1#
'ILSPlanes(9).Plane.D = -21.45
'
''=================================================================
'ILSPlanes(6).Poly.AddPoint ILSPlanes(5).Poly.Point(4)
'ILSPlanes(6).Poly.AddPoint ILSPlanes(5).Poly.Point(3)
'ILSPlanes(6).Poly.AddPoint PointAlongPlane(ptA, ArDir + 90#, 3015#)
'
'Set ILSPlanes(6).Plane.Pt = ptA
'
'ILSPlanes(6).Plane.A = 0.01075
'ILSPlanes(6).Plane.B = -0.143
'ILSPlanes(6).Plane.C = -1#
'ILSPlanes(6).Plane.D = 7.58
'
''=================================================================
'ILSPlanes(8).Poly.AddPoint ILSPlanes(9).Poly.Point(0)
'ILSPlanes(8).Poly.AddPoint PointAlongPlane(ptA, ArDir - 90#, 3015#)
'ILSPlanes(8).Poly.AddPoint ILSPlanes(9).Poly.Point(1)
'
'Set ILSPlanes(8).Plane.Pt = ptA
'
'ILSPlanes(8).Plane.A = 0.01075
'ILSPlanes(8).Plane.B = 0.143
'ILSPlanes(8).Plane.C = -1#
'ILSPlanes(8).Plane.D = 7.58
'
''=================================================================
'ILSPlanes(7).Poly.AddPoint ILSPlanes(0).Poly.Point(0)
'ILSPlanes(7).Poly.AddPoint ILSPlanes(0).Poly.Point(3)
'ILSPlanes(7).Poly.AddPoint ILSPlanes(6).Poly.Point(0)
'ILSPlanes(7).Poly.AddPoint ILSPlanes(6).Poly.Point(2)
'ILSPlanes(7).Poly.AddPoint ILSPlanes(8).Poly.Point(1)
'ILSPlanes(7).Poly.AddPoint ILSPlanes(8).Poly.Point(0)
'
'Set ILSPlanes(7).Plane.Pt = ptA
'
'ILSPlanes(7).Plane.A = -0.025
'ILSPlanes(7).Plane.B = 0#
'ILSPlanes(7).Plane.C = -1#
'ILSPlanes(7).Plane.D = -22.5
'
''=================================================================
'fAlpha = Atn(1440# / 9600#)
'fDistPl2 = (18500# - 3060#) / Cos(fAlpha)
'
'fAztPl2 = ReturnAngleAsDegree(ILSPlanes(2).Poly.Point(0), ILSPlanes(2).Poly.Point(1))
'ILSPlanes(2).Poly.ReplacePoints 1, 1, 1, PointAlongPlane(ILSPlanes(2).Poly.Point(0), fAztPl2, fDistPl2)
'
'fAztPl2 = ReturnAngleAsDegree(ILSPlanes(2).Poly.Point(3), ILSPlanes(2).Poly.Point(2))
'ILSPlanes(2).Poly.ReplacePoints 2, 1, 1, PointAlongPlane(ILSPlanes(2).Poly.Point(3), fAztPl2, fDistPl2)
'
'For I = 0 To N - 1
'    Set pTopo = ILSPlanes(N).Poly
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'
'    Set pTopo = ILSPlanes(I).Poly
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'
'    Set ILSPlanes(N).Poly = pTopo.Union(ILSPlanes(N).Poly)
''    DrawPolygon ILSPlanes(i).Poly
'Next I
'
''DrawPolygon ILSPlanes(N).Poly, 0
'
'End Sub
'
'Public Sub CreateOASPlanes(ptLHprj As IPoint, ArDir As Double, _
'    ByVal hMax As Double, OASPlanes() As D3DPolygone, ILSCategory As Long) ', Optional bDraw As Boolean = False)
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim L As Long
'Dim N As Long
'Dim M As Long
'Dim hCons As Double
'
'Dim ResLine(8) As IPolyline
'Dim pFrmPoint As IPoint
'Dim pToPoint As IPoint
'Dim pPolyline As IPolyline
'Dim pPolyLine1 As IPolyline
'Dim pTransform As ITransform2D
'Dim pTopo As ITopologicalOperator2
'
'If ILSCategory = 1 Then
'    hCons = arHOASPlaneCat1
'Else
'    hCons = arHOASPlaneCat23
'End If
'
'Set pPolyline = New Polyline
'Set pFrmPoint = New Point
'Set pToPoint = New Point
'
'N = UBound(OASPlanes)
'
'For I = 0 To N
'    Set OASPlanes(I).Poly = New Polygon
'Next I
'
'For I = 2 To 5
'    J = I + 1
'    Set ResLine(I) = IntersectPlanes(OASPlanes(I), OASPlanes(I + 1), 0#, hCons)
''If bDraw And I = 2 Then
''    MsgDialog.ShowMessage "ResLine(I).FromPoint.X = " + CStr(ResLine(I).FromPoint.X) + "     ResLine(I).FromPoint.Y = " + CStr(ResLine(I).FromPoint.Y)
''End If
''MsgDialog.ShowMessage "pt0.X = " + CStr(IntersectPlanes.FromPoint.X) + "     pt0.Y = " + CStr(IntersectPlanes.FromPoint.Y)
'
'    pFrmPoint.PutCoords ResLine(I).FromPoint.X + ptLHprj.X, ResLine(I).FromPoint.Y + ptLHprj.Y
'    pPolyline.FromPoint = pFrmPoint
'
''If bDraw And I = 2 Then
''    MsgDialog.ShowMessage "ResLine(I).FromPoint.X = " + CStr(ResLine(I).FromPoint.X) + "     ResLine(I).FromPoint.Y = " + CStr(ResLine(I).FromPoint.Y)
''End If
''If Draw And (I = 2) Then
''    MsgDialog.ShowMessage "ResLine(I).FromPoint.X + ptLHprj.X = " + CStr(ResLine(I).FromPoint.X) + "+" + CStr(ptLHprj.X) + "=" + CStr(pPoint.X) + "    ResLine(I).FromPoint.Y + ptLHprj.Y = " + CStr(ResLine(I).FromPoint.Y) + "+" + CStr(ptLHprj.Y) + "=" + CStr(pPoint.Y)
''    MsgDialog.ShowMessage "ResLine(I).FromPoint.X + ptLHprj.X = " + CStr(ResLine(I).FromPoint.X) + "+" + CStr(ptLHprj.X) + "=" + CStr(pPolyline.FromPoint.X) + "   ResLine(I).FromPoint.Y + ptLHprj.Y = " + CStr(ResLine(I).FromPoint.Y) + "+" + CStr(ptLHprj.Y) + "=" + CStr(pPolyline.FromPoint.Y)
''End If
'
'    pToPoint.PutCoords ResLine(I).ToPoint.X + ptLHprj.X, ResLine(I).ToPoint.Y + ptLHprj.Y
'    pPolyline.ToPoint = pToPoint
''If Draw And (I = 2) Then
''    MsgDialog.ShowMessage "ResLine(I).ToPoint.X + ptLHprj.X = " + CStr(ResLine(I).ToPoint.X) + "+" + CStr(ptLHprj.X) + "=" + CStr(pPoint.X) + " ResLine(I).ToPoint.Y + ptLHprj.Y = " + CStr(ResLine(I).ToPoint.Y) + "+" + CStr(ptLHprj.Y) + "=" + CStr(pPoint.Y)
''    MsgDialog.ShowMessage "ResLine(I).ToPoint.X + ptLHprj.X = " + CStr(ResLine(I).ToPoint.X) + "+" + CStr(ptLHprj.X) + "=" + CStr(pPolyline.ToPoint.X) + " ResLine(I).ToPoint.Y + ptLHprj.Y = " + CStr(ResLine(I).ToPoint.Y) + "+" + CStr(ptLHprj.Y) + "=" + CStr(pPolyline.ToPoint.Y)
''End If
'
''If bDraw And I = 2 Then
''    DrawPoint pFrmPoint, 255
''    DrawPolyLine pPolyline, 0, 2
''End If
'
'    Set pTransform = pPolyline
'    pTransform.Rotate ptLHprj, DegToRad(ArDir + 180#)
'
''If bDraw And I = 2 Then
''    DrawPolyLine pPolyline, 255, 2
''End If
'
'    OASPlanes(0).Poly.AddPoint pPolyline.FromPoint
'    OASPlanes(N).Poly.AddPoint pPolyline.ToPoint
'Next I
'
'For I = 0 To 3
'    J = 1 + (I + 4) Mod 6
'    K = 1 + (I + 5) Mod 6
'    L = (I + 6) Mod 8
'
'    Set ResLine(L) = IntersectPlanes(OASPlanes(J), OASPlanes(K), 0#, hMax)
'
'    pFrmPoint.PutCoords ResLine(L).FromPoint.X + ptLHprj.X, ResLine(L).FromPoint.Y + ptLHprj.Y
'    pPolyline.FromPoint = pFrmPoint
'
'    pToPoint.PutCoords ResLine(L).ToPoint.X + ptLHprj.X, ResLine(L).ToPoint.Y + ptLHprj.Y
'    pPolyline.ToPoint = pToPoint
'
'    Set pTransform = pPolyline
'    pTransform.Rotate ptLHprj, DegToRad(ArDir + 180#)
'
'    OASPlanes(0).Poly.AddPoint pPolyline.FromPoint
'    OASPlanes(N).Poly.AddPoint pPolyline.ToPoint
'Next I
'
'M = OASPlanes(0).Poly.PointCount
'J = 6
'For I = 1 To 6
'    K = J Mod M
'    L = (J + M - 1) Mod M
'
'    OASPlanes(I).Poly.AddPoint OASPlanes(0).Poly.Point(K)
'    OASPlanes(I).Poly.AddPoint OASPlanes(0).Poly.Point(L)
'    OASPlanes(I).Poly.AddPoint OASPlanes(N).Poly.Point(L)
'    OASPlanes(I).Poly.AddPoint OASPlanes(N).Poly.Point(K)
'    Set OASPlanes(I).Plane.Pt = OASPlanes(0).Poly.Point(L)
'
'    J = J + 1
'    If J = 4 Then J = 5
'    If J = 8 Then J = 1
'Next I
'
''If ILSCategory = 3 Then
''    OASPlanes(N - 1).Poly.AddPoint
''End If
'
'For I = 0 To N
'    Set pTopo = OASPlanes(I).Poly
'    pTopo.IsKnownSimple = False
'    pTopo.Simplify
'Next I
'
'For I = 0 To 8
'    Set ResLine(I) = Nothing
'Next I
'
'End Sub
'
'Private Function GetObstacles(ObstList() As ObstacleMSA, ptCenter As IPoint, MaxDist As Double) As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
''Dim iID As Long
'Dim iNAME As Long
'Dim iObsShape As Long
'
'Dim pRow As IRow
'Dim pCursor As ICursor
'Dim pFilter As IQueryFilter
'
'Dim pGeo As IGeometry
'Dim pProxiOperator As IProximityOperator
'
'GetObstacles = 0
'ReDim ObstList(-1 To -1)
'
'If MaxDist = 0 Then Exit Function
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pObsTable.OIDFieldName + ">=0"
'
'N = pObsTable.RowCount(pFilter) - 1
'If N < 0 Then
'    Set pFilter = Nothing
'    Exit Function
'End If
'
'Set pCursor = pObsTable.Search(pFilter, False)
'
'iObsShape = pCursor.FindField("Shape")
'iNAME = pCursor.FindField("Name")
'
'ReDim ObstList(0 To N)
'
'J = 0
'I = -1
'Set pProxiOperator = ptCenter
'Set pRow = pCursor.NextRow
'
'Do While Not pRow Is Nothing
'    I = I + 1
'    Set ObstList(J).Ptgeo = pRow.Value(iObsShape)
'
'    Set ObstList(J).ptPrj = New Point
'    ObstList(J).ptPrj.PutCoords ObstList(J).Ptgeo.X, ObstList(J).Ptgeo.Y
'
'    Set pGeo = ObstList(J).ptPrj
'    Set pGeo.SpatialReference = pSpRefShp
'    pGeo.Project pSpRefPrj
'
'    If pProxiOperator.ReturnDistance(ObstList(J).ptPrj) <= MaxDist Then
'        ObstList(J).ptPrj.Z = ObstList(J).Ptgeo.Z
'        ObstList(J).ptPrj.M = ObstList(J).Ptgeo.M
'        ObstList(J).Name = pRow.Value(iNAME)
'        ObstList(J).Height = ObstList(J).Ptgeo.Z
'        ObstList(J).Index = I
'        J = J + 1
'    End If
'    Set pRow = pCursor.NextRow
'Loop
'
'If J > 0 Then
'    ReDim Preserve ObstList(J - 1)
'    GetObstacles = J
'Else
'    ReDim ObstList(-1 To -1)
'End If
'
'Set pFilter = Nothing
'
'End Function

'Sub GetWorkObstacleList11111111111(CurObstacle() As ObstacleMSA, BuffObstacle() As ObstacleMSA, WorkObstacle() As ObstacleMSA, PtNAV As Point, bMax As Long, wMax As Long)
'Dim I As Long
'Dim J As Long
'Dim K As Long
'
'Dim N As Long
'Dim hbMax As Double
'Dim hwMax As Double
'Dim fTmp As Double
'Dim Proxi As IProximityOperator
'
'hbMax = -9999#
'hwMax = -9999#
'bMax = -1
'wMax = -1
'
'N = UBound(CurObstacle)
'
'If N < 0 Then
'    ReDim BuffObstacle(-1 To -1)
'    ReDim WorkObstacle(-1 To -1)
'    Exit Sub
'End If
'
'ReDim WorkObstacle(0 To N)
'ReDim BuffObstacle(0 To N)
'Set Proxi = PtNAV
'
'J = 0
'K = 0
'
'For I = 0 To N
'    fTmp = Proxi.ReturnDistance(CurObstacle(I).ptPrj)
'    If fTmp <= arBufferMSA.Value Then
'        BuffObstacle(K) = CurObstacle(I)
'        If BuffObstacle(K).Height > hbMax Then
'            hbMax = BuffObstacle(K).Height
'            bMax = K
'        End If
'        K = K + 1
'    Else
'        WorkObstacle(J) = CurObstacle(I)
'        If WorkObstacle(J).Height > hwMax Then
'            hwMax = WorkObstacle(J).Height
'            wMax = J
'        End If
'        J = J + 1
'    End If
'Next I
'
'If K > 0 Then
'    ReDim Preserve BuffObstacle(K - 1)
'Else
'    ReDim BuffObstacle(-1 To -1)
'End If
'
'If J > 0 Then
'    ReDim Preserve WorkObstacle(J - 1)
'Else
'    ReDim WorkObstacle(-1 To -1)
'End If
'
'End Sub

'Function MaxObstacleHeightInPoly(ObstList() As ObstacleMSA, pPoly As IPolygon, RefHeight As Double, index As Long) As Double
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim iID As Long
'Dim iNAME As Long
'Dim iObsShape As Long
''Dim iOIDList() As Long
'Dim MaxHeight As Double
'
'Dim pFilter As IQueryFilter
'Dim pRow As IRow
'Dim pCursor As ICursor
'
'Dim pGeo As IGeometry
'Dim pProxiOperator As IProximityOperator
'
'MaxObstacleHeightInPoly = 0#
'ReDim ObstList(-1 To -1)
'index = -1
'
'If pPoly.IsEmpty Then Exit Function
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pObsTable.OIDFieldName + ">=0"
'
'N = pObsTable.RowCount(pFilter) - 1
'If N < 0 Then
'    Set pFilter = Nothing
'    Exit Function
'End If
'
'Set pCursor = pObsTable.Search(pFilter, False)
'iObsShape = pCursor.FindField("Shape")
'iNAME = pCursor.FindField("Name")
'iID = pCursor.FindField("ID_OBS")
'
'ReDim ObstList(0 To N)
'
'J = 0
'I = -1
'
'MaxHeight = 0#
'Set pProxiOperator = pPoly
'Set pRow = pCursor.NextRow
'
'Do While Not pRow Is Nothing
'    I = I + 1
'    Set ObstList(J).Ptgeo = pRow.Value(iObsShape)
'
'    Set ObstList(J).ptPrj = New Point
'    ObstList(J).ptPrj.PutCoords ObstList(J).Ptgeo.X, ObstList(J).Ptgeo.Y
'
'    Set pGeo = ObstList(J).ptPrj
'    Set pGeo.SpatialReference = pSpRefShp
'    pGeo.Project pSpRefPrj
'
'    If pProxiOperator.ReturnDistance(ObstList(J).ptPrj) = 0 Then
'        ObstList(J).ptPrj.Z = ObstList(J).Ptgeo.Z
'        ObstList(J).ptPrj.M = ObstList(J).Ptgeo.M
'        ObstList(J).Name = pRow.Value(iNAME)
'        ObstList(J).ID = pRow.Value(iID)
'        ObstList(J).Height = ObstList(J).Ptgeo.Z - RefHeight
'        ObstList(J).index = I
'        If ObstList(J).Height > MaxHeight Then
'            index = J
'            MaxHeight = ObstList(J).Height
'        End If
'        J = J + 1
'    End If
'    Set pRow = pCursor.NextRow
'Loop
'
'MaxObstacleHeightInPoly = MaxHeight
'
'If J > 0 Then
'    ReDim Preserve ObstList(J - 1)
'Else
'    ReDim ObstList(-1 To -1)
'End If
'
'Set pFilter = Nothing
'
'End Function

'Function CalcArObstaclesMOC(PtInList() As ObstacleAr, PtWorkList() As ObstacleAr, pBasePolygon As IPointCollection, pBufferPolygon As IPointCollection) As Long
'Dim ptCur As IPoint
'Dim pBaseProxi As IProximityOperator
'Dim pBufferProxi As IProximityOperator
'
'Dim hMax As Double
'Dim MOCValue As Double
'Dim iMax As Double
'Dim D As Double
'
'Dim N As Long
'Dim I As Long
'Dim J As Long
'
''PtInList()
''PtWorkList()
'
'CalcArObstaclesMOC = -1
'N = UBound(PtInList)
'
'If (N < 0) Or (pBufferPolygon.PointCount < 1) Then
'    ReDim PtWorkList(-1 To -1)
'    Exit Function
'End If
'
'ReDim PtWorkList(0 To N)
'
'Set pBaseProxi = pBasePolygon
'Set pBufferProxi = pBufferPolygon
'J = 0
'hMax = -9999#
'iMax = -1
'
'MOCValue = arIASegmentMOC.Value
'
'For I = 0 To N
'    Set ptCur = PtInList(I).ptPrj
'
'    D = pBufferProxi.ReturnDistance(ptCur)
'
'    If D = 0# Then
'        PtWorkList(J) = PtInList(I)
'        D = pBaseProxi.ReturnDistance(ptCur)
'        PtWorkList(J).Flags = -CInt(D = 0#)
'        PtWorkList(J).fTmp = 1#
'
'        If PtWorkList(J).Flags = 0 Then
'            PtWorkList(J).fTmp = (arHoldingBuffer.Value - D) / arHoldingBuffer.Value
'        End If
'
'        PtWorkList(J).MOC = PtWorkList(J).fTmp * arIASegmentMOC.Value
'        PtWorkList(J).ReqH = PtWorkList(J).Height + PtWorkList(J).MOC
'
'        If PtWorkList(J).ReqH > hMax Then
'            hMax = PtWorkList(J).ReqH
'            iMax = J
'        End If
'        J = J + 1
'    End If
'Next
'CalcArObstaclesMOC = iMax
'
'If J > 0 Then
'    ReDim Preserve PtWorkList(J - 1)
'Else
'    ReDim PtWorkList(-1 To -1)
'End If
'
'End Function
'
'Function CalcArObstaclesNavMOC(PtInList() As ObstacleAr, PtWorkList() As ObstacleAr, PtNAV As IPoint, Dir As Double, pBasePolygon As IPointCollection, pBufferPolygon As IPointCollection, Var As Double) As Long
'Dim pBufferProxi As IProximityOperator
'Dim pBaseProxi As IProximityOperator
'Dim ptCur As IPoint
'
'Dim pTopoOper As ITopologicalOperator2
'Dim pLine1 As IPolyline
'Dim pLine As IPolyline
'
'Dim MOCValue As Double
'Dim fDist As Double
'Dim hMax As Double
'
'Dim Side0 As Long
'Dim Side1 As Long
'Dim iMax As Long
'Dim N As Long
'Dim I As Long
'Dim J As Long
'
'CalcArObstaclesNavMOC = -1
'N = UBound(PtInList)
'
'If (N < 0) Or (pBufferPolygon.PointCount < 1) Then
'    ReDim PtWorkList(-1 To -1)
'    Exit Function
'End If
'
'ReDim PtWorkList(0 To N)
'
'Set pBaseProxi = pBasePolygon
'Set pBufferProxi = pBufferPolygon
'J = 0
'hMax = -9999#
'iMax = -1
'
'Select Case Var
'Case 0
'    MOCValue = arFASeg_FAF_MOC.Value
'Case 1
'    MOCValue = arFASegmentMOC.Value
'Case 2
'    MOCValue = arMA_InterMOC.Value
'Case 3
'    MOCValue = arMA_FinalMOC.Value
'Case 4
'    MOCValue = arFASeg_FAF_MOC.Value
'Case Else
'    MOCValue = Var
'End Select
'
'Set pLine1 = New Polyline
'
'For I = 0 To N
'    Set ptCur = PtInList(I).ptPrj
'
'    fDist = pBufferProxi.ReturnDistance(ptCur)
'
'    If fDist = 0# Then
'        PtWorkList(J) = PtInList(I)
'        fDist = pBaseProxi.ReturnDistance(ptCur)
'        If fDist = 0# Then
'            PtWorkList(J).Flags = 1
'        Else
'            PtWorkList(J).Flags = 0
'        End If
'
'        fDist = Point2LineDistancePrj(ptCur, PtNAV, Dir - 90#)
'        If Var > 1 Then PtWorkList(J).Dist = fDist
'
'        If PtWorkList(J).Flags <> 0 Then
'            PtWorkList(J).MOC = MOCValue
'            PtWorkList(J).fTmp = 1#
'        Else
'            Side0 = SideDef(PtNAV, Dir, ptCur)
'            fDist = Point2LineDistancePrj(ptCur, PtNAV, Dir)
'
'            pLine1.FromPoint = PointAlongPlane(ptCur, Dir + 90# * Side0, fDist)
'            pLine1.ToPoint = PointAlongPlane(ptCur, Dir - 90# * Side0, 1000000#)
'
'            Set pTopoOper = pBufferPolygon
'            Set pLine = pTopoOper.Intersect(pLine1, esriGeometry1Dimension)
'            Side1 = SideDef(pLine.FromPoint, Dir, pLine.ToPoint)
'            If Side0 * Side1 < 0 Then pLine.ReverseOrientation
'
'            Set pTopoOper = pBasePolygon
'            Set pLine1 = pTopoOper.Intersect(pLine, esriGeometry1Dimension)
'            Side1 = SideDef(pLine1.FromPoint, Dir, pLine1.ToPoint)
'            If Side0 * Side1 < 0 Then pLine1.ReverseOrientation
'
'            pLine.FromPoint = pLine1.ToPoint
'            PtWorkList(J).fTmp = ReturnDistanceAsMeter(ptCur, pLine.ToPoint) / pLine.Length
'            PtWorkList(J).MOC = MOCValue * PtWorkList(J).fTmp
'        End If
'
'        PtWorkList(J).ReqH = PtWorkList(J).Height + PtWorkList(J).MOC
'        If PtWorkList(J).ReqH > hMax Then
'            hMax = PtWorkList(J).ReqH
'            iMax = J
'        End If
'        J = J + 1
'    End If
'Next
'CalcArObstaclesNavMOC = iMax
'
'If J > 0 Then
'    ReDim Preserve PtWorkList(J - 1)
'Else
'    ReDim PtWorkList(-1 To -1)
'End If
'
'Set pLine1 = Nothing
'Set pLine = Nothing
'
'End Function
'
'Function GetFinalObstacles(ObstacleList() As ObstacleAr, ObstacleFinalMOCList() As ObstacleAr, _
'    BaseArea As IPointCollection, SecPoly As IPointCollection, SecPoly0 As IPointCollection, _
'            DistPoly As IGeometry, TurnAngle As Double, ArDir As Double, OutAzt As Double, _
'            ptNavPrj As Point, FixPntPrj As Point) As Long
'
'Dim ptCur As IPoint
'Dim pLine As IPolyline
'Dim pLine1 As IPolyline
'Dim pGeometry As IGeometry
'Dim pTopoOper As ITopologicalOperator
'Dim pBaseProxi As IProximityOperator
'Dim pSecondProxi As IProximityOperator
'Dim pSecond0Proxi As IProximityOperator
'Dim pDistProxi As IProximityOperator
'
'Dim D As Double
'Dim dCL As Double
'Dim hMax As Double
'Dim MOCValue As Double
'
'Dim N As Long
'Dim I As Long
'Dim J As Long
'Dim iMax As Long
'Dim Side0 As Long
'Dim Side1 As Long
'
'GetFinalObstacles = -1
'N = UBound(ObstacleList)
'
'If (N < 0) Or (BaseArea.PointCount < 1) Then
'    ReDim ObstacleFinalMOCList(-1 To -1)
'    Exit Function
'End If
'
'ReDim ObstacleFinalMOCList(0 To N)
'Set pBaseProxi = BaseArea
'Set pSecondProxi = SecPoly
'Set pSecond0Proxi = SecPoly0
'Set pDistProxi = DistPoly
'
'J = 0
'hMax = -9999#
'iMax = -1
'
'If TurnAngle > arMATurnTrshAngl.Value Then
'    MOCValue = arMA_FinalMOC.Value
'Else
'    MOCValue = arMA_InterMOC.Value
'End If
'
'Set pLine1 = New Polyline
'
'For I = 0 To N
'    Set ptCur = ObstacleList(I).ptPrj
'
'    D = pBaseProxi.ReturnDistance(ptCur)
'
'    If D = 0# Then
'        ObstacleFinalMOCList(J) = ObstacleList(I)
'        ObstacleFinalMOCList(J).Flags = 0
'        ObstacleFinalMOCList(J).fTmp = 1#
'
'        Set pGeometry = SecPoly0
'        If Not pGeometry.IsEmpty Then
'            D = pSecond0Proxi.ReturnDistance(ptCur)
'        Else
'            D = 10#
'        End If
'
'        If D = 0# Then
'            ObstacleFinalMOCList(J).Flags = 2
'
'            Set pTopoOper = SecPoly0
'            Side0 = SideDef(ptNavPrj, ArDir, ptCur)
'            dCL = Point2LineDistancePrj(ptCur, ptNavPrj, ArDir)
'
'            pLine1.FromPoint = PointAlongPlane(ptCur, ArDir + 90# * Side0, dCL)
'            pLine1.ToPoint = PointAlongPlane(ptCur, ArDir - 90# * Side0, 10# * R)
'
'            Set pLine = pTopoOper.Intersect(pLine1, esriGeometry1Dimension)
'
'
'            Side1 = SideDef(pLine.FromPoint, ArDir, pLine.ToPoint)
'
'            If Side0 * Side1 < 0 Then
'                pLine.ReverseOrientation
'            End If
'
'            ObstacleFinalMOCList(J).fTmp = ReturnDistanceAsMeter(ptCur, pLine.ToPoint) / pLine.Length
'            If ObstacleFinalMOCList(J).fTmp > 1# Then ObstacleFinalMOCList(J).fTmp = 1#
'        Else
'            Set pGeometry = SecPoly
'            If Not pGeometry.IsEmpty Then
'                D = pSecondProxi.ReturnDistance(ptCur)
'            Else
'                D = 10#
'            End If
'
'            If D = 0# Then
'                ObstacleFinalMOCList(J).Flags = 1
'                Set pTopoOper = SecPoly
'                Side0 = SideDef(FixPntPrj, OutAzt, ptCur)
'                dCL = Point2LineDistancePrj(ptCur, FixPntPrj, OutAzt)
'
'                pLine1.FromPoint = PointAlongPlane(ptCur, OutAzt + 90# * Side0, dCL)
'                pLine1.ToPoint = PointAlongPlane(ptCur, OutAzt - 90# * Side0, 10# * R)
'                Set pLine = pTopoOper.Intersect(pLine1, esriGeometry1Dimension)
'
'                Side1 = SideDef(pLine.FromPoint, OutAzt, pLine.ToPoint)
'
'                If Side0 * Side1 < 0 Then pLine.ReverseOrientation
'
'                ObstacleFinalMOCList(J).fTmp = ReturnDistanceAsMeter(ptCur, pLine.ToPoint) / pLine.Length
'                If ObstacleFinalMOCList(J).fTmp > 1# Then ObstacleFinalMOCList(J).fTmp = 1#
'            End If
'        End If
'
'        ObstacleFinalMOCList(J).MOC = MOCValue * ObstacleFinalMOCList(J).fTmp
'        ObstacleFinalMOCList(J).ReqH = ObstacleFinalMOCList(J).Height + ObstacleFinalMOCList(J).MOC
'        ObstacleFinalMOCList(J).Dist = pDistProxi.ReturnDistance(ptCur)
'        If ObstacleFinalMOCList(J).ReqH > hMax Then
'            hMax = ObstacleFinalMOCList(J).ReqH
'            iMax = J
'        End If
'        J = J + 1
'    End If
'Next
'
'GetFinalObstacles = iMax
'
'If J > 0 Then
'    ReDim Preserve ObstacleFinalMOCList(J - 1)
'Else
'    ReDim ObstacleFinalMOCList(-1 To -1)
'End If
'
'End Function
'
'Function GetIntermedObstacles(ObstacleList() As ObstacleAr, ObstacleImList() As ObstacleAr, _
'            FullArea As IPointCollection, SecPoly As IPointCollection, HalfFAFWidth As Double, _
'            minIntLen As Double, ArDir As Double, hFAF As Double, NavType As Long, _
'            ptFAFprj As Point, Optional bKeepAll As Boolean = False) As Long
'
'Dim ptCur As IPoint
'Dim pLine As IPolyline
'Dim pGeometry As IGeometry
'Dim pTopoOper As ITopologicalOperator
'Dim pFullProxi As IProximityOperator
'Dim pSecondProxi As IProximityOperator
'
'Dim D As Double
'Dim fTmp As Double
'Dim MOCValue As Double
'
'Dim N As Long
'Dim I As Long
'Dim J As Long
'Dim Side0 As Long
'Dim Side1 As Long
'
'GetIntermedObstacles = -1
'N = UBound(ObstacleList)
'
'If (N < 0) Or (FullArea.PointCount < 1) Then
'    ReDim ObstacleImList(-1 To -1)
'    Exit Function
'End If
'
'ReDim ObstacleImList(0 To N)
'Set pFullProxi = FullArea
'Set pSecondProxi = SecPoly
'
'MOCValue = arISegmentMOC.Value
'Set pLine = New Polyline
'J = 0
'
'For I = 0 To N
'    Set ptCur = ObstacleList(I).ptPrj
'
'    D = pFullProxi.ReturnDistance(ptCur)
'
'    If D = 0# Then
'        ObstacleImList(J) = ObstacleList(I)
'        ObstacleImList(J).fTmp = 1#
'        ObstacleImList(J).Dist = Point2LineDistancePrj(ptCur, ptFAFprj, ArDir + 90#)
'        ObstacleImList(J).DistStar = Point2LineDistancePrj(ptCur, ptFAFprj, ArDir)
'
'        If minIntLen > ObstacleImList(J).Dist Then
'            If NavType = 3 Then
'                fTmp = 2# * (arIFHalfWidth.Value - HalfFAFWidth - minIntLen * (ObstacleImList(J).DistStar - HalfFAFWidth) / ObstacleImList(J).Dist) / arIFHalfWidth.Value
'            Else
'                fTmp = 2# * (1# - ObstacleImList(J).DistStar / (HalfFAFWidth + ObstacleImList(J).Dist / minIntLen * (arIFHalfWidth.Value - HalfFAFWidth)))
'            End If
'        Else
'            fTmp = 2# * (1# - ObstacleImList(J).DistStar / arIFHalfWidth.Value)
'        End If
'
'        If fTmp >= 1# Then
'            ObstacleImList(J).MOC = MOCValue
'            ObstacleImList(J).Flags = True
'            ObstacleImList(J).fTmp = 1#
'        ElseIf fTmp > 0# Then
'            ObstacleImList(J).MOC = fTmp * MOCValue
'            ObstacleImList(J).Flags = False
'            ObstacleImList(J).fTmp = fTmp
'        Else
'            GoTo continue
'        End If
'
'        ObstacleImList(J).ReqH = ObstacleImList(J).Height + ObstacleImList(J).MOC
'        If (ObstacleImList(J).ReqH > hFAF) Or bKeepAll Then J = J + 1
'continue:
'    End If
'Next I
'
'GetIntermedObstacles = J - 1
'
'If J > 0 Then
'    ReDim Preserve ObstacleImList(J - 1)
'Else
'    ReDim ObstacleImList(-1 To -1)
'End If
'
'End Function
'
'Public Function GetIntermObstacleList(InObstList() As ObstacleAr, OutObstList() As ObstacleAr, pPlane As IPolygon, ptLHprj As IPoint, ArDir As Double) As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim pRelation As IRelationalOperator
'
'N = UBound(InObstList)
'GetIntermObstacleList = 0
'
'If (N < 0) Or pPlane.IsEmpty Then
'    ReDim OutObstList(-1 To -1)
'    Exit Function
'End If
'
'ReDim OutObstList(0 To N)
'
'Set pRelation = pPlane
'J = 0
'
'For I = 0 To N
'    If pRelation.Contains(InObstList(I).ptPrj) Then
'        OutObstList(J) = InObstList(I)
'        OutObstList(J).Height = InObstList(I).ptPrj.Z - ptLHprj.Z
'        OutObstList(J).Dist = Point2LineDistancePrj(InObstList(I).ptPrj, ptLHprj, ArDir - 90#)
'        OutObstList(J).DistStar = Point2LineDistancePrj(InObstList(I).ptPrj, ptLHprj, ArDir)
''        OutObstList(J).ReqH = OutObstList(J).Height + arMA_FinalMOC.Value
'        J = J + 1
'    End If
'Next I
'
'If J > 0 Then
'    ReDim Preserve OutObstList(0 To J - 1)
'Else
'    ReDim OutObstList(-1 To -1)
'End If
'
'GetIntermObstacleList = J - 1
'
'End Function
'
'Function GetMAPtObstacles(PtInList() As ObstacleAr, PtWorkList() As ObstacleAr, _
'        PtFAF As IPoint, pFarPoint, PtNAV As IPoint, NavType As Long, Dir As Double, _
'        pFullPolygon As IPointCollection) As Long
'Dim pBaseProxi As IProximityOperator
'Dim ptCur As IPoint
'
'Dim InitWidth As Double
'Dim NavArea As Double
'Dim Toler As Double
'Dim Dist As Double
'Dim dNav As Double
'Dim dCL As Double
'Dim L As Double
'
'Dim N As Long
'Dim I As Long
'Dim J As Long
'
'GetMAPtObstacles = -1
'N = UBound(PtInList)
'
'If (N < 0) Or (pFullPolygon.PointCount < 1) Then
'    ReDim PtWorkList(-1 To -1)
'    Exit Function
'End If
'
'If NavType = 0 Then
'    InitWidth = 0.5 * VOR.InitWidth
'    Toler = VOR.SplayAngle
'ElseIf NavType = 2 Then
'    InitWidth = 0.5 * NDB.InitWidth
'    Toler = NDB.SplayAngle
'Else
'    ReDim PtWorkList(-1 To -1)
'    Exit Function
'End If
'
'Set pBaseProxi = pFullPolygon
'ReDim PtWorkList(0 To N)
'
'J = 0
'L = PtFAF.Z / arFixMaxIgnorGrd.Value
'
'For I = 0 To N
'    Set ptCur = PtInList(I).ptPrj
'
'    If pBaseProxi.ReturnDistance(ptCur) = 0# Then
'        Dist = Point2LineDistancePrj(ptCur, pFarPoint, Dir - 90#)
'        If (PtInList(I).Height > arFixMaxIgnorGrd.Value * (L - Dist)) Then
'            PtWorkList(J) = PtInList(I)
'            PtWorkList(J).Dist = Point2LineDistancePrj(ptCur, PtFAF, Dir - 90#)
'
'            dNav = Point2LineDistancePrj(ptCur, PtNAV, Dir - 90#)
'            dCL = Point2LineDistancePrj(ptCur, PtNAV, Dir)
'            NavArea = InitWidth + Tan(DegToRad(Toler)) * dNav
'
'            PtWorkList(J).fTmp = 2# * (NavArea - dCL) / NavArea
'            If PtWorkList(J).fTmp >= 1# Then
'                PtWorkList(J).Flags = 1
'                PtWorkList(J).fTmp = 1#
'            Else
'                PtWorkList(J).Flags = 0
'            End If
'            J = J + 1
''        Else
'
'        End If
'    End If
'Next
'GetMAPtObstacles = J - 1
'
'If J > 0 Then
'    ReDim Preserve PtWorkList(J - 1)
'Else
'    ReDim PtWorkList(-1 To -1)
'End If
'
'End Function
'
'Function GetArObstacles(ObstList() As ObstacleAr, ptCenter As IPoint, MaxDist As Double, fRefHeight As Double) As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim iID As Long
'
'Dim iShapeField As Long
'Dim iNameField As Long
'Dim iHeightField As Long
'Dim iIDField As Long
'
'Dim pRow As IRow
'Dim pCursor As ICursor
'Dim pFilter As IQueryFilter
'
'Dim pGeo As IGeometry
'Dim pProxiOperator As IProximityOperator
'
'GetArObstacles = 0
'ReDim ObstList(-1 To -1)
'
'If MaxDist = 0 Then Exit Function
'
'Set pFilter = New QueryFilter
'pFilter.WhereClause = pObsTable.OIDFieldName + ">=0"
'
'N = pObsTable.RowCount(pFilter) - 1
'If N < 0 Then
'    Set pFilter = Nothing
'    Exit Function
'End If
'
'Set pCursor = pObsTable.Search(pFilter, False)
'
'iShapeField = pCursor.FindField("Shape")
'iHeightField = pCursor.FindField("Height_M")
'iIDField = pCursor.FindField("ID_OBS")
'iNameField = pCursor.FindField("Name")
'
''=============================================
'ReDim ObstList(0 To N)
'Set pProxiOperator = ptCenter
'Set pRow = pCursor.NextRow
'
'J = 0
'I = -1
'
'Do While Not pRow Is Nothing
'    I = I + 1
'
'    Set ObstList(J).ptGeo = pRow.Value(iShapeField)
'
'    Set ObstList(J).ptPrj = New Point
'    ObstList(J).ptPrj.PutCoords ObstList(J).ptGeo.X, ObstList(J).ptGeo.Y
'
'    Set pGeo = ObstList(J).ptPrj
'    Set pGeo.SpatialReference = pSpRefShp
'    pGeo.Project pSpRefPrj
'
'    If pProxiOperator.ReturnDistance(ObstList(J).ptPrj) <= MaxDist Then
'        ObstList(J).ptPrj.Z = ObstList(J).ptGeo.Z
'        ObstList(J).ptPrj.M = ObstList(J).ptGeo.M
'        ObstList(J).Name = pRow.Value(iNameField)
'        ObstList(J).Height = ObstList(J).ptGeo.Z - fRefHeight
'        ObstList(J).ID = pRow.Value(iIDField)
'        ObstList(J).index = I
'        J = J + 1
'    End If
'    Set pRow = pCursor.NextRow
'Loop
''=============================================
'GetArObstacles = J
'Set pFilter = Nothing
'
'If J > 0 Then
'    ReDim Preserve ObstList(J - 1)
'    Sort ObstList, 0
'Else
'    ReDim ObstList(-1 To -1)
'End If
'
'End Function

'Public Function AnaliseObstacles(InObstList() As ObstacleAr, OutObstList() As ObstacleAr, _
'                ptLHprj As IPoint, ArDir As Double, Planes() As D3DPolygone) As Long
'Dim I As Long
'Dim J As Long
'Dim K As Long
'Dim N As Long
'Dim M As Long
'
'Dim X As Double
'Dim Y As Double
'Dim Z As Double
'
'Dim pRelationFull As IRelationalOperator
'Dim pRelation As IRelationalOperator
'
'N = UBound(InObstList)
'AnaliseObstacles = 0
'
'If N < 0 Then
'    ReDim OutObstList(-1 To -1)
'    Exit Function
'End If
'
'ReDim OutObstList(0 To N)
'
'M = UBound(Planes)
'Set pRelationFull = Planes(M).Poly
'K = 0
'
'For I = 0 To N
'    If pRelationFull.Contains(InObstList(I).ptPrj) Then
'        For J = 0 To M - 1
'            Set pRelation = Planes(J).Poly
'
'            If pRelation.Contains(InObstList(I).ptPrj) Then
'                OutObstList(K) = InObstList(I)
'
'                X = Point2LineDistancePrj(InObstList(I).ptPrj, ptLHprj, ArDir - 90#) * SideDef(ptLHprj, ArDir - 90#, InObstList(I).ptPrj)
'                Y = Point2LineDistancePrj(InObstList(I).ptPrj, ptLHprj, ArDir) * SideDef(ptLHprj, ArDir, InObstList(I).ptPrj)
'                Z = Planes(J).Plane.A * X + _
'                    Planes(J).Plane.B * Y + _
'                    Planes(J).Plane.D
'
'                OutObstList(K).Dist = X
'                OutObstList(K).DistStar = Y
'                OutObstList(K).fTmp = Z
'                OutObstList(K).Flags = J
'                OutObstList(K).Height = InObstList(I).ptPrj.Z - ptLHprj.Z
'                OutObstList(K).hPent = OutObstList(K).Height - Z
'                If OutObstList(K).hPent > 0# Then AnaliseObstacles = AnaliseObstacles + 1
'                K = K + 1
'                Exit For
'            End If
'        Next J
'    End If
'Next I
'
'If K > 0 Then
'    ReDim Preserve OutObstList(0 To K - 1)
'Else
'    ReDim OutObstList(-1 To -1)
'End If
'
'End Function
'
'Public Function AnaliseCat23Obstacles(ObstList() As ObstacleAr, Planes() As D3DPolygone) As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
'Dim M As Long
'Dim Z As Double
'
'Dim pRelationFull As IRelationalOperator
'Dim pRelation As IRelationalOperator
'
'N = UBound(ObstList)
'AnaliseCat23Obstacles = 0
'
'If N < 0 Then Exit Function
'
'M = UBound(Planes)
'Set pRelationFull = Planes(M).Poly
'
'For I = 0 To N
'    If pRelationFull.Contains(ObstList(I).ptPrj) Then
'        For J = 0 To M - 1
'            Set pRelation = Planes(J).Poly
'
'            If pRelation.Contains(ObstList(I).ptPrj) Then
'                Z = Planes(J).Plane.A * ObstList(I).Dist + _
'                    Planes(J).Plane.B * ObstList(I).DistStar + _
'                    Planes(J).Plane.D
'
'                ObstList(I).fTmp = Z
'                ObstList(I).hPent = ObstList(I).Height - Z
'                ObstList(I).Flags = J + 100
'                If ObstList(I).hPent > 0# Then AnaliseCat23Obstacles = AnaliseCat23Obstacles + 1
'                Exit For
'            End If
'        Next J
'    End If
'Next I
'
'End Function
'
'Function GetTurnAreaObstacles(ObstacleList() As ObstacleAr, ObstacleTurnAreaMOCList() As ObstacleAr, _
'    BaseArea As IPointCollection, DistPoly As IGeometry, TurnFlg As Boolean, TNH As Double, _
'    MAPDG As Double, NavType As Long, ptNavPrj As Point, SecPoly As IPointCollection, OutAzt As Double) As Long
'
'Dim ptCur As IPoint
'Dim pLine As IPolyline
'Dim pGeometry As IGeometry
'Dim pTopoOper As ITopologicalOperator2
'
'Dim pBaseRelation As IRelationalOperator
'Dim pSecondRelation As IRelationalOperator
'Dim pDistProxi As IProximityOperator
'
'Dim dCL As Double
'Dim hMax As Double
'Dim MOCValue As Double
'
'Dim N As Long
'Dim I As Long
'Dim J As Long
'Dim iMax As Long
'Dim Side0 As Long
'Dim Side1 As Long
'
'GetTurnAreaObstacles = -1
'N = UBound(ObstacleList)
'
''DrawPolygon BaseArea, 0
''DrawPolygon SecPoly, 255
''DrawPolygon DistPoly, RGB(255, 0, 255)
'
'If (N < 0) Or (BaseArea.PointCount < 1) Then
'    ReDim ObstacleTurnAreaMOCList(-1 To -1)
'    Exit Function
'End If
'
'ReDim ObstacleTurnAreaMOCList(0 To N)
'Set pBaseRelation = BaseArea
'Set pSecondRelation = SecPoly
'Set pTopoOper = SecPoly
'
'If NavType >= 0 Then
'    pTopoOper.IsKnownSimple = False
'    pTopoOper.Simplify
'End If
'
'Set pDistProxi = DistPoly
'
'J = 0
'hMax = 0#
'iMax = -1
'
'If TurnFlg Then
'    MOCValue = arMA_FinalMOC.Value
'Else
'    MOCValue = arMA_InterMOC.Value
'End If
'
'Set pLine = New Polyline
'
'For I = 0 To N
'    Set ptCur = ObstacleList(I).ptPrj
'    If pBaseRelation.Contains(ptCur) Then
'        ObstacleTurnAreaMOCList(J) = ObstacleList(I)
'        ObstacleTurnAreaMOCList(J).DistStar = pDistProxi.ReturnDistance(ptCur)
'        If ObstacleTurnAreaMOCList(J).DistStar > 0# Then
'            ObstacleTurnAreaMOCList(J).Flags = 0
'            ObstacleTurnAreaMOCList(J).fTmp = 1#
'
'            If NavType >= 0 Then
'                If pSecondRelation.Contains(ptCur) Then
'                    ObstacleTurnAreaMOCList(J).Flags = 1
'
'                    Side0 = SideDef(ptNavPrj, OutAzt, ptCur)
'                    dCL = Point2LineDistancePrj(ptCur, ptNavPrj, OutAzt)
'
'                    pLine.FromPoint = PointAlongPlane(ptCur, OutAzt + 90# * Side0, dCL)
'                    pLine.ToPoint = PointAlongPlane(ptCur, OutAzt - 90# * Side0, 10# * R)
'
'                    Set pLine = pTopoOper.Intersect(pLine, esriGeometry1Dimension)
'
'                    Side1 = SideDef(pLine.FromPoint, OutAzt, pLine.ToPoint)
'
'                    If Side0 * Side1 < 0 Then
'                        pLine.ReverseOrientation
'                    End If
'                    ObstacleTurnAreaMOCList(J).fTmp = ReturnDistanceAsMeter(ptCur, pLine.ToPoint) / pLine.Length
'    '                ObstacleTurnAreaMOCList(J).Dist = pDistProxi.ReturnDistance(ptCur)
'                End If
'            End If
'
'            ObstacleTurnAreaMOCList(J).MOC = MOCValue * ObstacleTurnAreaMOCList(J).fTmp
'            ObstacleTurnAreaMOCList(J).ReqH = ObstacleTurnAreaMOCList(J).Height + ObstacleTurnAreaMOCList(J).MOC
'            ObstacleTurnAreaMOCList(J).EffectiveHeight = TNH + ObstacleTurnAreaMOCList(J).DistStar * MAPDG
'            ObstacleTurnAreaMOCList(J).hPent = ObstacleTurnAreaMOCList(J).ReqH - ObstacleTurnAreaMOCList(J).EffectiveHeight
'            ObstacleTurnAreaMOCList(J).Dist = ObstacleTurnAreaMOCList(J).DistStar
'
'            If ObstacleTurnAreaMOCList(J).hPent > hMax Then
'                hMax = ObstacleTurnAreaMOCList(J).hPent
'                iMax = J
'            End If
'            J = J + 1
'        End If
'    End If
'Next
'
'GetTurnAreaObstacles = iMax
'
'If J > 0 Then
'    ReDim Preserve ObstacleTurnAreaMOCList(J - 1)
'Else
'    ReDim ObstacleTurnAreaMOCList(-1 To -1)
'End If
'
'End Function
'
'Public Sub CalcEffectiveHeights(ObstList() As ObstacleAr, fGPAngle As Double, _
'    fMAPDG As Double, fRDH As Double, Optional ILSObs As Boolean = False)
'
'Dim I As Long
'Dim N As Long
'Dim fTmp As Double
'Dim CoTanZ As Double
'Dim TanGPA As Double
'Dim CoTanGPA As Double
'
'CoTanZ = 1# / fMAPDG
'
'TanGPA = Tan(DegToRad(fGPAngle))
'CoTanGPA = 1# / TanGPA
'
'N = UBound(ObstList)
'
'For I = 0 To N
'    fTmp = (ObstList(I).Height * CoTanZ + (arOverHeadToler.Value + ObstList(I).Dist)) / (CoTanZ + CoTanGPA)
'
'    ObstList(I).EffectiveHeight = fTmp
'
'    If ObstList(I).Dist < -arOverHeadToler.Value Then
'        ObstList(I).Index = 1
'    Else
'        ObstList(I).Index = 0
'
'        If ILSObs Then
'            If ObstList(I).Dist < 0 Then
'                ObstList(I).ReqH = fRDH
'            Else
'                ObstList(I).ReqH = ObstList(I).Dist * TanGPA + fRDH
'            End If
'        Else
'            Select Case ObstList(I).Flags
'            Case ZeroPlane
'                ObstList(I).ReqH = fRDH
'            Case ZPlane
'                ObstList(I).ReqH = 0#
'            Case WPlane, XlPlane, XrPlane
'                ObstList(I).ReqH = OASPlanes(ObstList(I).Flags).Plane.A * ObstList(I).Dist + _
'                    OASPlanes(ObstList(I).Flags).Plane.B * ObstList(I).DistStar + _
'                    OASPlanes(ObstList(I).Flags).Plane.D + arISegmentMOC.Value
'            Case YlPlane, YrPlane
'                If ObstList(I).Dist < 0 Then
'                    ObstList(I).ReqH = fRDH
'                Else
'                    ObstList(I).ReqH = ObstList(I).Dist * TanGPA + fRDH
'                End If
'            End Select
'        End If
'
'        If ObstList(I).ReqH < arISegmentMOC.Value Then
'            ObstList(I).ReqH = arISegmentMOC.Value
'        End If
'    End If
'Next I
'
'End Sub
'
'Function OAS_DATABase(ByVal LLZ2THRDist As Double, ByVal GPAngle As Double, _
'                ByVal MisAprGr As Double, ByVal ILSCategory As Long, RDH As Double, _
'                Ss As Double, St As Double, OASPlanes() As D3DPolygone) As Boolean
'Dim sTabName As String
'Dim sMisAprGr As String
'Dim sGPAngle As String
'Dim iLLZ2THRDist As Long
'Dim CatOffset As Long
'Dim sLLZ2THRDist As String
'
'Dim fTmp0 As Double
'Dim fTmp1 As Double
'Dim CwCorr As Double
'Dim Cw_Corr As Double
'Dim CxCorr As Double
'Dim CyCorr As Double
'Dim RDHCorr As Double
'
'Dim dbsGlis As DAO.Database
'Dim rstGlis As DAO.Recordset
'
'sGPAngle = CStr(Round(GPAngle * 10# - 0.4999999))
'
'If LLZ2THRDist < 2000# Then LLZ2THRDist = 2000#
'If LLZ2THRDist > 4500# Then LLZ2THRDist = 4500#
'If LLZ2THRDist > 4400# Then
'    sLLZ2THRDist = CStr(Round(0.01 * LLZ2THRDist - 0.4999999) * 100)
'Else
'    sLLZ2THRDist = CStr(Round(0.005 * LLZ2THRDist - 0.4999999) * 200)
'End If
'
'sTabName = sGPAngle + "_" + sLLZ2THRDist
'
'sMisAprGr = CStr(Round(MisAprGr * 1000#))
'
'Set dbsGlis = DAO.OpenDatabase(InstallDir & "\plans.mdb", dbOpenSnapshot, False, ";Pwd=test")
'Set rstGlis = dbsGlis.OpenRecordset(sTabName, dbOpenDynaset)
'
'CatOffset = 3 * (ILSCategory - 1)
''If RDH > arAbv_Treshold.Value Then RDH = arAbv_Treshold.Value
'RDHCorr = RDH - arAbv_Treshold.Value
'
'CwCorr = St - 6# - RDHCorr
'Cw_Corr = St - 6# - RDHCorr
'
'OASPlanes(0).Plane.A = 0#
'OASPlanes(0).Plane.B = 0#
'OASPlanes(0).Plane.C = -1#
'OASPlanes(0).Plane.D = 0#
'
'OASPlanes(8).Plane.A = 0#
'OASPlanes(8).Plane.B = 0#
'OASPlanes(8).Plane.C = -1#
'OASPlanes(8).Plane.D = 300#
'
''rstGlis.Restartable
'
'    rstGlis.FindFirst "ID = 'W'"
'    OASPlanes(1).Plane.A = rstGlis.Fields(1 + CatOffset).Value
'    OASPlanes(1).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
'    OASPlanes(1).Plane.C = -1#
'    OASPlanes(1).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CwCorr
'
'    rstGlis.FindFirst "ID = 'X'"
'    OASPlanes(2).Plane.A = rstGlis.Fields(1 + CatOffset).Value
'    OASPlanes(2).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
'    OASPlanes(2).Plane.C = -1#
'
'    fTmp0 = -St / OASPlanes(2).Plane.B
'    fTmp1 = Ss - (St - 3) / OASPlanes(2).Plane.B
'    CxCorr = Max(fTmp0, fTmp1)
'
'    fTmp0 = -6 / OASPlanes(2).Plane.B
'    fTmp1 = 30 - 3 / OASPlanes(2).Plane.B
'    CxCorr = CxCorr - Max(fTmp0, fTmp1)
'
'    CxCorr = -CxCorr * OASPlanes(2).Plane.B - RDHCorr
'    OASPlanes(2).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CxCorr
'
'    rstGlis.FindFirst "ID = 'Y" + sMisAprGr + "'"
'    OASPlanes(3).Plane.A = rstGlis.Fields(1 + CatOffset).Value
'    OASPlanes(3).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
'    OASPlanes(3).Plane.C = -1#
'
'    fTmp0 = -St / OASPlanes(3).Plane.B
'    fTmp1 = Ss - (St - 3) / OASPlanes(3).Plane.B
'    CyCorr = Max(fTmp0, fTmp1)
'
'    fTmp0 = -6 / OASPlanes(3).Plane.B
'    fTmp1 = 30 - 3 / OASPlanes(3).Plane.B
'    CyCorr = CyCorr - Max(fTmp0, fTmp1)
'
'    CyCorr = -CyCorr * OASPlanes(3).Plane.B - RDHCorr
'    OASPlanes(3).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CyCorr
'
'    rstGlis.FindFirst "ID = '" & "Z" & sMisAprGr & "'"
'    OASPlanes(4).Plane.A = rstGlis.Fields(1 + CatOffset).Value
'    OASPlanes(4).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
'    OASPlanes(4).Plane.C = -1#
'    OASPlanes(4).Plane.D = rstGlis.Fields(3 + CatOffset).Value
'
'OASPlanes(5).Plane.A = OASPlanes(3).Plane.A
'OASPlanes(5).Plane.B = -OASPlanes(3).Plane.B
'OASPlanes(5).Plane.C = -1#
'OASPlanes(5).Plane.D = OASPlanes(3).Plane.D
'
'OASPlanes(6).Plane.A = OASPlanes(2).Plane.A
'OASPlanes(6).Plane.B = -OASPlanes(2).Plane.B
'OASPlanes(6).Plane.C = -1#
'OASPlanes(6).Plane.D = OASPlanes(2).Plane.D
'
'If ILSCategory = 3 Then
'    rstGlis.FindFirst "ID = 'W*'"
'    OASPlanes(7).Plane.A = rstGlis.Fields(1 + CatOffset).Value
'    OASPlanes(7).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
'    OASPlanes(7).Plane.C = -1#
'    OASPlanes(7).Plane.D = rstGlis.Fields(3 + CatOffset).Value - Cw_Corr
'End If
'
'rstGlis.Close
'dbsGlis.Close
'
'End Function
'
Function CalcTouchByFixDir(PtSt As IPoint, ptFIX As IPoint, TurnR As Double, dirCur As Double, _
DirFix As Double, turnDir As Long, TurnDir2 As Long, SnapAngle As Double, dDir As Double, FlyBy As IPoint) As IPointCollection

Dim Constructor As IConstructPoint

Dim PtCnt1 As IPoint
Dim PtCnt2 As IPoint
Dim ptTmp As IPoint
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
Dim dirFromTmp As Double

Dim dirFxCnt As Double
Dim TouchDir As Double
Dim OutDir As Double
Dim OutDir0 As Double
Dim OutDir1 As Double
Dim Dist As Double

If SubtractAngles(dirCur, DirFix) < 0.5 Then
    DirFix = dirCur
End If

Set CalcTouchByFixDir = New MultiPoint
Set PtCnt1 = PointAlongPlane(PtSt, dirCur + 90# * turnDir, TurnR)
PtSt.M = dirCur

OutDir0 = Modulus(DirFix - SnapAngle * turnDir, 360#)
OutDir1 = Modulus(DirFix + SnapAngle * turnDir, 360#)

Set Pt10 = PointAlongPlane(PtCnt1, OutDir0 - 90# * turnDir, TurnR)
Set Pt11 = PointAlongPlane(PtCnt1, OutDir1 - 90# * turnDir, TurnR)

SideT = SideDef(Pt10, DirFix, ptFIX)
SideD = SideDef(Pt10, DirFix, PtCnt1)

If SideT * SideD < 0 Then
    Set Pt1 = Pt10
    OutDir = OutDir0
Else
    Set Pt1 = Pt11
    OutDir = OutDir1
End If

Pt1.M = OutDir

Set FlyBy = New Point
Set Constructor = FlyBy

Constructor.ConstructAngleIntersection Pt1, DegToRad(OutDir), ptFIX, DegToRad(DirFix)

Dist = ReturnDistanceAsMeter(Pt1, FlyBy)

dirToTmp = ReturnAngleAsDegree(ptFIX, FlyBy)
distToTmp = ReturnDistanceAsMeter(ptFIX, FlyBy)

TurnDir2 = -AnglesSideDef(OutDir, DirFix)

If TurnDir2 < 0 Then
    DeltaAngle = Modulus(180# + DirFix - OutDir, 360#)
ElseIf TurnDir2 > 0 Then
    DeltaAngle = Modulus(OutDir - 180# - DirFix, 360#)
End If

DeltaAngle = 0.5 * DeltaAngle
DeltaDist = TurnR / Tan(DegToRad(DeltaAngle))

dDir = Dist - DeltaDist

If DeltaDist <= Dist Then
    Set pt2 = PointAlongPlane(FlyBy, OutDir - 180#, DeltaDist)
    Set pt3 = PointAlongPlane(FlyBy, DirFix, DeltaDist)
Else
    Set pt2 = PointAlongPlane(FlyBy, OutDir, DeltaDist)
    Set pt3 = PointAlongPlane(FlyBy, DirFix - 180#, DeltaDist)
End If

pt2.M = OutDir
pt3.M = DirFix

CalcTouchByFixDir.AddPoint PtSt
CalcTouchByFixDir.AddPoint Pt1
CalcTouchByFixDir.AddPoint pt2
CalcTouchByFixDir.AddPoint pt3

End Function

'''Public Function FillNavaidList() As Long
'''Dim CoopNavField As Long
'''Dim NavNameField As Long
'''Dim NavTypeField As Long
'''Dim NavCodeField As Long
'''Dim NavShpField As Long
'''Dim MagVarField As Long
'''Dim RangeField As Long
'''Dim nRep_Nav As Long
'''Dim I As Long
'''Dim J As Long
'''Dim K As Long
'''
'''Dim pGeo As IGeometry
'''Dim pRow As IRow
'''Dim pCursor As ICursor
'''Dim pFilter As IQueryFilter
'''
'''Set pFilter = New QueryFilter
'''pFilter.WhereClause = pNAVTable.OIDFieldName + ">=0"
'''
'''nRep_Nav = pNAVTable.RowCount(pFilter)
'''FillNavaidList = nRep_Nav
'''
'''If nRep_Nav = 0 Then
'''    ReDim NavaidList(-1 To -1)
'''    ReDim DMEList(-1 To -1)
'''    NavCount = -1
'''Else
'''    ReDim NavaidList(0 To nRep_Nav - 1)
'''    ReDim DMEList(0 To nRep_Nav - 1)
'''
'''    Set pCursor = pNAVTable.Search(pFilter, False)
'''    CoopNavField = pCursor.FindField("CoopNav")
'''    NavNameField = pCursor.FindField("CallSign")
'''    NavTypeField = pCursor.FindField("Type")
'''    NavCodeField = pCursor.FindField("Code")
'''    NavShpField = pCursor.FindField("Shape")
'''    MagVarField = pCursor.FindField("Var")
'''    RangeField = pCursor.FindField("Range_Km")
'''
'''    J = 0
'''    K = 0
'''    I = -1
'''
'''    Set pRow = pCursor.NextRow
'''    Do While Not pRow Is Nothing
'''        I = I + 1
'''        NavaidList(J).NavTypecode = pRow.Value(NavCodeField)
'''
'''        If (NavaidList(J).NavTypecode = 0) Or (NavaidList(J).NavTypecode = 2) Then
'''
'''            Set NavaidList(J).ptGeo = pRow.Value(NavShpField)
'''            Set NavaidList(J).ptPrj = New Point
'''            NavaidList(J).ptPrj.PutCoords NavaidList(J).ptGeo.X, NavaidList(J).ptGeo.y
'''            NavaidList(J).ptPrj.Z = NavaidList(J).ptGeo.Z
'''            NavaidList(J).ptPrj.M = NavaidList(J).ptGeo.M
'''
'''            Set pGeo = NavaidList(J).ptPrj
'''            Set pGeo.SpatialReference = pSpRefShp
'''            pGeo.Project pSpRefPrj
'''
'''            NavaidList(J).Name = pRow.Value(NavNameField)
'''            NavaidList(J).NavTypeName = pRow.Value(NavTypeField)
'''            NavaidList(J).MagVar = pRow.Value(MagVarField)
'''            NavaidList(J).Range = pRow.Value(RangeField) * 1000#
'''            NavaidList(J).CoopNav = pRow.Value(CoopNavField)
'''            NavaidList(J).index = I
'''
'''            J = J + 1
'''        ElseIf NavaidList(J).NavTypecode = 1 Then
'''            DMEList(K).NavTypecode = 1
'''            Set DMEList(K).ptGeo = pRow.Value(NavShpField)
'''            Set DMEList(K).ptPrj = New Point
'''            DMEList(K).ptPrj.PutCoords DMEList(K).ptGeo.X, DMEList(K).ptGeo.y
'''            DMEList(K).ptPrj.Z = DMEList(K).ptGeo.Z
'''            DMEList(K).ptPrj.M = DMEList(K).ptGeo.M
'''
'''            Set pGeo = DMEList(K).ptPrj
'''            Set pGeo.SpatialReference = pSpRefShp
'''            pGeo.Project pSpRefPrj
'''
'''            DMEList(K).Name = pRow.Value(NavNameField)
'''            DMEList(K).NavTypeName = pRow.Value(NavTypeField)
''''            DMEList(K).MagVar = pRow.Value(MagVarField)
'''            DMEList(K).Range = pRow.Value(RangeField) * 1000#
'''            DMEList(K).CoopNav = pRow.Value(CoopNavField)
'''            DMEList(K).index = I
'''
'''            K = K + 1
'''        End If
'''        Set pRow = pCursor.NextRow
'''    Loop
'''
'''    If J = 0 Then
'''        ReDim NavaidList(-1 To -1)
'''        NavCount = -1
'''    Else
'''        ReDim Preserve NavaidList(J - 1)
'''        NavCount = J
'''    End If
'''
'''    If K = 0 Then
'''        ReDim DMEList(-1 To -1)
'''    Else
'''        ReDim Preserve DMEList(K - 1)
'''    End If
'''End If
''''========================
'''pFilter.WhereClause = pILSTable.OIDFieldName + ">=0"
'''nRep_Nav = pILSTable.RowCount(pFilter)
'''
'''If nRep_Nav > 0 Then
'''
'''Dim iCourse As Long
'''Dim iGP_RDH As Long
'''Dim iLLZ_THR As Long
'''Dim iSEC_WIDTH As Long
'''Dim pPC As IPointCollection
'''
'''    Set pCursor = pILSTable.Search(pFilter, False)
'''
'''    NavShpField = pCursor.FindField("Shape")
'''    NavNameField = pCursor.FindField("CallSign")
'''    iCourse = pCursor.FindField("Course")
'''    iGP_RDH = pCursor.FindField("GP_RDH")
'''    iLLZ_THR = pCursor.FindField("LLZ_THR")
'''    iSEC_WIDTH = pCursor.FindField("SEC_WIDTH")
'''    RangeField = pCursor.FindField("Range_Km")
'''
'''    K = UBound(NavaidList)
'''    If K < 0 Then
'''        ReDim NavaidList(0 To nRep_Nav - 1)
'''        J = 0
'''    Else
'''        ReDim Preserve NavaidList(0 To K + nRep_Nav)
'''        J = K + 1
'''    End If
'''
'''    I = -1
'''    Set pRow = pCursor.NextRow
'''
'''    Do While Not pRow Is Nothing
'''        I = I + 1
'''        Set pPC = pRow.Value(NavShpField)
'''        Set NavaidList(J).ptGeo = pPC.Point(0)
'''        Set NavaidList(J).ptPrj = New Point
'''        NavaidList(J).ptPrj.PutCoords NavaidList(J).ptGeo.X, NavaidList(J).ptGeo.y
'''        NavaidList(J).ptPrj.Z = NavaidList(J).ptGeo.Z
'''        NavaidList(J).ptPrj.M = NavaidList(J).ptGeo.M
'''
'''        Set pGeo = NavaidList(J).ptPrj
'''        Set pGeo.SpatialReference = pSpRefShp
'''        pGeo.Project pSpRefPrj
'''
'''        NavaidList(J).Name = pRow.Value(NavNameField)
'''        NavaidList(J).Course = pRow.Value(iCourse)
'''        NavaidList(J).GP_RDH = pRow.Value(iGP_RDH)
'''        NavaidList(J).LLZ_THR = pRow.Value(iLLZ_THR)
'''        NavaidList(J).Sec_Width = pRow.Value(iSEC_WIDTH)
'''        NavaidList(J).Range = LLZ.Range
'''
'''        NavaidList(J).MagVar = MagVar
'''        NavaidList(J).NavTypeName = "LLZ"
'''        NavaidList(J).NavTypecode = 3
'''        NavaidList(J).index = I
'''
'''        J = J + 1
'''        Set pRow = pCursor.NextRow
'''    Loop
'''
'''    If J = 0 Then
'''        ReDim NavaidList(-1 To -1)
'''        NavCount = -1
'''    Else
'''        ReDim Preserve NavaidList(0 To J - 1)
'''        NavCount = J
'''   End If
'''End If
'''
'''Set pFilter = Nothing
'''
'''End Function
'''
''''Private Function CreateAPP_FIXFeatureClass(pFeatureWorkspace As IFeatureWorkspace, className As String) As IFeatureClass
''''Dim pFieldsEdit As IFieldsEdit
''''Dim pFields As IFields
''''Dim pFieldEdit As IFieldEdit
''''Dim pGeomDef As IGeometryDefEdit
''''Dim ShapeFieldName As String
''''
''''On Error GoTo EH
''''
''''ShapeFieldName = "Shape"
''''  ' Add the Fields to the class the OID and Shape are compulsory
''''
''''  Set pFieldsEdit = New esriGeoDatabase.Fields
'''''=======================================OID
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "OID"
''''        .AliasName = "Object ID"
''''        .Type = esriFieldTypeOID
''''        .Editable = False
''''        .IsNullable = False
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================SHAPE
''''    Set pGeomDef = New GeometryDef
''''    With pGeomDef
''''        .AvgNumPoints = 1
''''        .GeometryType = esriGeometryPoint
''''        .GridCount = 1
''''        .GridSize(0) = 1000
''''        .HasM = True
''''        .HasZ = True
''''        Set .SpatialReference = pSpRefShp
''''    End With
''''
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = ShapeFieldName
''''        .AliasName = ShapeFieldName
''''        .Type = esriFieldTypeGeometry
''''        .Editable = True
''''        .IsNullable = False
''''        Set .GeometryDef = pGeomDef
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================NAME
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "NAME"
''''        .AliasName = "NAME"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 8
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================TYPE
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "TYPE"
''''        .AliasName = "TYPE"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 5
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================LAT
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "LAT"
''''        .AliasName = "LAT"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 16
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================LON
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "LON"
''''        .AliasName = "LON"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 16
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================ELEV_M
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "ELEV_M"
''''        .AliasName = "ELEV_M"
''''        .Type = esriFieldTypeInteger
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 5
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the PROC NAME field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "PROC_NAME"
''''        .AliasName = "PROC_Name"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 32
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the PROC Type field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "PROC_Type"
''''        .AliasName = "PROC_Type"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 16
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the Homing Nav field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "GuidNav"
''''        .AliasName = "GuidNav"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 5
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the TypeOfHomingNav field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "TypeOfGuidNav"
''''        .AliasName = "TypeOfGuidNav"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 5
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the Intersect Nav field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "InterNav"
''''        .AliasName = "InterNav"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = True
''''        .Length = 5
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
'''''=======================================
'''''' create the TypeOfIntersectNav field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "TypeOfInterNav"
''''        .AliasName = "TypeOfInterNav"
''''        .Type = esriFieldTypeString
''''        .Editable = True
''''        .IsNullable = False
''''        .Length = 12
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================INPUTDATE
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    With pFieldEdit
''''        .Name = "INPUTDATE"
''''        .AliasName = "INPUTDATE"
''''        .Type = esriFieldTypeDate
''''        .Editable = True
''''        .IsNullable = False
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
'''''=======================================
''''Dim pUid As IUID
''''    Set pFields = pFieldsEdit
''''    Set pUid = New uID
''''    pUid.Value = "esriGeoDatabase.Feature"
''''    Set CreateAPP_FIXFeatureClass = pFeatureWorkspace.CreateFeatureClass(className, pFields, Nothing, Nothing, esriFTSimple, ShapeFieldName, "")
''''    Exit Function
''''EH:
''''    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, , , "createDefsTable"
''''End Function
''''
''''Public Function createDefsTable(featWorkspace As IFeatureWorkspace) As ITable
''''Dim pFields As IFields
''''Dim pCLSID As uID
''''Dim pCLSEXT As uID
''''Dim ConfigWord As String
''''
''''  On Error GoTo EH
''''
''''  Set createDefsTable = Nothing
''''  If featWorkspace Is Nothing Then Exit Function
''''
''''    Set pCLSID = Nothing
''''    Set pCLSID = New uID
''''
''''    '' determine the appropriate geometry type corresponding the the feature type
''''    pCLSID.Value = "esriGeoDatabase.Object"
''''
''''  ' establish a fields collection
''''
''''Dim pFieldsEdit As esriGeoDatabase.IFieldsEdit
''''    Set pFieldsEdit = New esriGeoDatabase.Fields
''''
'''''Dim pField As IField
''''Dim pFieldEdit As IFieldEdit
''''
''''
''''    ''
''''    '' create the object id field
''''    ''
''''
''''    Set pFieldEdit = New esriGeoDatabase.Field
'''''    Set pField = New Field
''''    With pFieldEdit
''''        .Name = "OBJECTID"
''''        .AliasName = "OBJECTID"
''''        .Type = esriFieldTypeOID
''''    End With
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the WPT Name field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "WPT_Name"
''''    pFieldEdit.AliasName = "WPT_Name"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 8
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the PROC Type field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "PROC_Type"
''''    pFieldEdit.AliasName = "PROC_Type"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 16
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the PROC NAME field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "PROC_NAME"
''''    pFieldEdit.AliasName = "PROC_Name"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 32
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the H Min field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "H_Min"
''''    pFieldEdit.AliasName = "H_Min"
''''    pFieldEdit.Type = esriFieldTypeSmallInteger
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the H Max field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "H_Max"
''''    pFieldEdit.AliasName = "H_Max"
''''    pFieldEdit.Type = esriFieldTypeSmallInteger
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the H Units field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "H_Units"
''''    pFieldEdit.AliasName = "H_Units"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 8
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the Homing Nav field
''''    ''
'''' '   Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "GuidanceNav"
''''    pFieldEdit.AliasName = "GuidanceNav"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 8
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the TypeOfHomingNav field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "TypeOfGuidNav"
''''    pFieldEdit.AliasName = "TypeOfGuidNav"
''''    pFieldEdit.Type = esriFieldTypeSmallInteger
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the Intersect Nav field
''''    ''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "IntersectNav"
''''    pFieldEdit.AliasName = "IntersectNav"
''''    pFieldEdit.Type = esriFieldTypeString
''''    pFieldEdit.Length = 8
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    ''
''''    '' create the TypeOfIntersectNav field
''''    ''
''''
'''''    Set pField = New Field
''''    Set pFieldEdit = New esriGeoDatabase.Field
''''    pFieldEdit.Name = "TypeOfIntersectNav"
''''    pFieldEdit.AliasName = "TypeOfIntersectNav"
''''    pFieldEdit.Type = esriFieldTypeSmallInteger
''''    pFieldEdit.IsNullable = False
''''    pFieldsEdit.AddField pFieldEdit
''''
''''    Set pFields = pFieldsEdit
''''
''''  ' establish the class extension
''''    Set pCLSEXT = Nothing
''''    Set createDefsTable = featWorkspace.CreateTable("Definitions", pFields, pCLSID, _
''''                             pCLSEXT, ConfigWord)
''''  Exit Function
''''EH:
''''    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, , , "createDefsTable"
''''End Function
''''
''''Public Sub AddShapeFile(pFeatureWorkspace As IFeatureWorkspace, pFeatureClass As IFeatureClass)
''''Dim I As Long
''''Dim sName As String
''''Dim bLayerExist As Boolean
''''Dim pFeatureLayer As IFeatureLayer
''''
''''    sName = pFeatureClass.AliasName
''''    bLayerExist = False
''''
''''    For I = 0 To pMap.LayerCount - 1
''''        If pMap.Layer(I).Name = sName Then
''''            bLayerExist = True
''''            Exit For
''''        End If
''''    Next I
''''
''''    If Not bLayerExist Then
'''''Create a new FeatureLayer and assign a shapefile to it
''''        Set pFeatureLayer = New FeatureLayer
''''        Set pFeatureLayer.FeatureClass = pFeatureClass
''''        pFeatureLayer.Name = sName
'''''Add the FeatureLayer to the focus map
''''        pMap.AddLayer pFeatureLayer
''''    End If
''''End Sub
''''
''''Public Function OpenResultWorkspace(bCreateNew As Boolean) As IWorkspace
''''Dim L As Long
''''Dim pos As Long
''''
''''Dim FileName As String
''''Dim Location As String
''''Dim FileNameForCreate As String
''''Dim plDocument As IDocument
''''Dim pFact As IWorkspaceFactory
''''Dim pFeatWs As IFeatureWorkspace
''''
''''    Set OpenResultWorkspace = Nothing
''''    Set plDocument = pDocument
''''    FileName = plDocument.VBProject.FileName
''''    L = Len(FileName)
''''    pos = InStrRev(FileName, "\")
''''
''''    If pos <> 0 Then
''''        Location = VBA.Left(FileName, pos)
''''        FileName = VBA.Right(FileName, L - pos)
''''    Else
''''        Location = "\"
''''    End If
''''
''''    pos = InStrRev(FileName, ".")
''''    FileNameForCreate = VBA.Left(FileName, pos - 1)
''''    FileName = FileNameForCreate + ".mdb"
''''
''''    Set pFact = New AccessWorkspaceFactory
''''On Error Resume Next
''''    Set OpenResultWorkspace = pFact.OpenFromFile(Location + FileName, 0)
''''On Error GoTo EH
''''    If (OpenResultWorkspace Is Nothing) And bCreateNew Then
''''        Dim pWorkspaceName As IWorkspaceName
''''        Dim pFeatureLayer As IFeatureLayer
''''
''''        Set pWorkspaceName = pFact.Create(Location, FileNameForCreate, Nothing, 0)
''''        Set pFact = pWorkspaceName.WorkspaceFactory
''''        Set OpenResultWorkspace = pFact.OpenFromFile(Location + FileName, 0)
''''    End If
''''
''''    If bCreateNew Then
''''        Dim pTable As ITable
''''
''''        Set pTable = Nothing
''''        Set pFeatWs = OpenResultWorkspace
''''On Error Resume Next
''''        Set pTable = pFeatWs.OpenTable("Definitions")
''''On Error GoTo 0
''''        If pTable Is Nothing Then createDefsTable OpenResultWorkspace
''''    End If
''''
''''    Exit Function
''''EH:
''''    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, , "createAccessWorkspace"
''''End Function
''''
''''Public Function OpenAPP_FIXWorkspace(bCreateNew As Boolean, APP_FIXFeatureClass As IFeatureClass) As IFeatureWorkspace
''''Dim pFeatWs As IFeatureWorkspace
''''Dim pDataset As IDataset
''''
''''    Set OpenAPP_FIXWorkspace = Nothing
''''    Set pDataset = pAIRFeatureClass
''''    Set pFeatWs = pDataset.Workspace
''''
''''    Set APP_FIXFeatureClass = Nothing
''''On Error Resume Next
''''    Set APP_FIXFeatureClass = pFeatWs.OpenFeatureClass("APP_FIX")
''''On Error GoTo EH
''''    If (APP_FIXFeatureClass Is Nothing) And bCreateNew Then
''''        Set APP_FIXFeatureClass = CreateAPP_FIXFeatureClass(pFeatWs, "APP_FIX")
''''    End If
''''    If Not (APP_FIXFeatureClass Is Nothing) Then AddShapeFile pFeatWs, APP_FIXFeatureClass
''''
''''    Set OpenAPP_FIXWorkspace = pFeatWs
''''    Exit Function
''''EH:
''''    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, "CreateAPP_FIXWorkspace"
''''End Function
''''
''''Public Function CheckName(pCheckTable As ITable, CheckItem As String) As Boolean
''''Dim pQueryFilter As IQueryFilter
''''Dim pCursor As ICursor
''''Dim pRow As IRow
''''
''''CheckName = False
''''
''''Set pQueryFilter = New QueryFilter
''''pQueryFilter.SubFields = "NAME"
''''
''''pQueryFilter.WhereClause = "NAME ='" + CheckItem + "'"
''''Set pCursor = pCheckTable.Search(pQueryFilter, False)
''''Set pRow = pCursor.NextRow
''''CheckName = Not (pRow Is Nothing)
''''
''''Set pQueryFilter = Nothing
''''End Function
'''
'Function LatNorm(Y As Double) As String
'    If (Y < 0#) Then
'        LatNorm = "S"
'        Y = -Y
'    Else
'        LatNorm = "N"
'    End If
'End Function
'
'Function LonNorm(X As Double) As String
'    If (X > 180#) Then
'        LonNorm = "W"
'        X = 360# - X
'    Else
'        LonNorm = "E"
'    End If
'End Function

'Public Sub FillTurnParams(ptFrom As IPoint, ptTo As IPoint, TrackPoint As ReportPoint)
'Dim fTmp As Double
'Dim fE As Double
'Dim ptCenter As IPoint
'Dim pGeometry As IGeometry
'Dim ptCenterGeo As IPoint
'Dim pConstructPoint As IConstructPoint
'
'Dim pPointCollection As IPointCollection
'Dim pPolyline As IPolyline
'
'fE = DegToRad(0.5)
'fTmp = DegToRad(ptFrom.M - ptTo.M)
'
'If (Abs(Sin(fTmp)) <= fE) And (Cos(fTmp) > 0#) Then
'    TrackPoint.Raidus = -1
'    TrackPoint.Turn = 0
'    TrackPoint.TurnAngle = ""
'    TrackPoint.TurnArcLen = ""
'    TrackPoint.CenterLat = ""
'    TrackPoint.CenterLon = ""
'Else
'    Set ptCenter = New Point
'    Set ptCenterGeo = New Point
'
'    If Abs(Sin(fTmp)) > fE Then
'        Set pConstructPoint = ptCenter
'        pConstructPoint.ConstructAngleIntersection ptFrom, DegToRad(ptFrom.M + 90#), ptTo, DegToRad(ptTo.M + 90#)
'    Else
'        ptCenter.PutCoords 0.5 * (ptFrom.X + ptTo.X), 0.5 * (ptFrom.Y + ptTo.Y)
'    End If
'
'    ptCenterGeo.PutCoords ptCenter.X, ptCenter.Y
'    Set pGeometry = ptCenterGeo
'    Set pGeometry.SpatialReference = pSpRefPrj
'    pGeometry.Project pSpRefShp
'
'    TrackPoint.Raidus = Round(ReturnDistanceAsMeter(ptCenter, ptFrom))
'    TrackPoint.Turn = SideDef(ptFrom, ptFrom.M, ptTo)
'    TrackPoint.TurnAngle = Round(Modulus((ptFrom.M - ptTo.M) * TrackPoint.Turn, 360#), 2)
'
'    TrackPoint.CenterLat = MyDD2Str(ptCenterGeo.Y, 3)
'    TrackPoint.CenterLon = MyDD2Str(ptCenterGeo.X, 4)
'
'    Set pPointCollection = New Polyline
'    pPointCollection.AddPoint ptFrom
'    pPointCollection.AddPoint ptTo
'
'    Set pPolyline = CalcTrajectoryFromMultiPoint(pPointCollection)
'    TrackPoint.TurnArcLen = CStr(Round(pPolyline.Length))
'
'    Set ptCenter = Nothing
'    Set ptCenterGeo = Nothing
'
'    Set pPointCollection = Nothing
'    Set pPolyline = Nothing
'End If
'
'End Sub

'''Public Sub SaveMxDocument(FileName As String, Optional ClearGraphics As Boolean = True)
'''Dim pGxLayer As IGxLayer
'''Dim pGxFile As IGxFile
'''Dim pLyr As ILayer
'''Dim pGraphics As IGraphicsContainer
'''
'''    'Set pGraphics = pDocument.ActivatedView.GraphicsContainer
'''    Set pGraphics = CurrentActiveView.GraphicsContainer
'''
'''    Set pLyr = pMap.ActiveGraphicsLayer
'''    pLyr.Name = "PANDA Chart"
'''    Set pLyr.SpatialReference = pSpRefPrj
'''
'''    Set pGxLayer = New GxLayer
'''    Set pGxFile = pGxLayer
'''
'''    pGxFile.Path = FileName + "_Chart.lyr"
'''
'''    Set pGxLayer.Layer = pLyr
'''
'''    pGraphics.DeleteAllElements
'''
'''    Set pGxLayer = New GxLayer
'''    Set pGxFile = pGxLayer
'''
'''    pGxFile.Path = FileName + "_Chart.lyr"
'''
'''    pMap.AddLayer pGxLayer.Layer
'''    Application.SaveAsDocument FileName + "_Chart.mxd", True
'''    pMap.DeleteLayer pGxLayer.Layer
'''End Sub

'Public Function FillSegObstList(LastPoint As StepDownFIX, _
'    fRefHeight As Double, IAF_FullAreaPoly As Polygon, IAF_BaseAreaPoly As Polygon, _
'    ObstList() As ObstacleAr) As Long
''StepDownFIXs (StepDownsNum - 1), fRefHeight, IAF_IIAreaPoly, IAF_IAreaPoly, SegObstList
'Dim iObsShape As Long
'Dim iNAME As Long
'Dim iID As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
'
'Dim FIXHeight As Double
'Dim fAlpha As Double
''Dim fIADir As Double
'Dim fDist As Double
'Dim fDir As Double
'Dim fTmp As Double
'Dim rObs As Double
'Dim MOC As Double
'Dim fL As Double
'
'Dim pQueryFilter As IQueryFilter
'Dim pCursor As ICursor
'Dim pRow As IRow
''Dim ptFictIF As IPoint
'Dim pFullRelation As IRelationalOperator
'Dim pBaseProxi As IProximityOperator
'Dim pGeo As IGeometry
'Dim GuidNav As NavaidData
'
'FillSegObstList = -1
'ReDim ObstList(-1 To -1)
'
'Set pQueryFilter = New QueryFilter
'pQueryFilter.SubFields = "Shape, ID_OBS, Name"
'pQueryFilter.WhereClause = pObsTable.OIDFieldName + ">=0"
'
'N = pObsTable.RowCount(pQueryFilter) - 1
'If N < 0 Then
'    Exit Function
'    Set pQueryFilter = Nothing
'End If
'
'ReDim ObstList(0 To N)
'
'Set pCursor = pObsTable.Search(pQueryFilter, False)
'
'iID = pCursor.FindField("ID_OBS")
'iObsShape = pCursor.FindField("Shape")
'iNAME = pCursor.FindField("Name")
'
'Set pFullRelation = IAF_FullAreaPoly
'Set pBaseProxi = IAF_BaseAreaPoly
'
'FIXHeight = LastPoint.ptPrj.Z - fRefHeight
''fIADir = LastPoint.InDir
'fDir = LastPoint.InDir
'GuidNav = LastPoint.GuidNav
'
'fL = ReturnDistanceAsMeter(GuidNav.ptPrj, LastPoint.ptPrj)
'fAlpha = ReturnAngleAsDegree(GuidNav.ptPrj, LastPoint.ptPrj)
'
'Set pRow = pCursor.NextRow
'
'I = 0
'J = 0
'
'While Not pRow Is Nothing
'
''                    ObstList(J).ID = pRow.Value(iID)
''                    ObstList(J).Name = pRow.Value(iName)
''    If (ObstList(J).ID = "709") Or (ObstList(J).ID = "1278") Then
''        I = I
''    End If
'
'    Set ObstList(J).ptGeo = pRow.Value(iObsShape)
'    Set ObstList(J).ptPrj = New Point
'    ObstList(J).ptPrj.PutCoords ObstList(J).ptGeo.X, ObstList(J).ptGeo.Y
'
'    Set pGeo = ObstList(J).ptPrj
'    Set pGeo.SpatialReference = pSpRefShp
'    pGeo.Project pSpRefPrj
'
'    If pFullRelation.Contains(ObstList(J).ptPrj) Then
'        fDist = pBaseProxi.ReturnDistance(ObstList(J).ptPrj)
'
'        ObstList(J).Height = ObstList(J).ptGeo.Z - fRefHeight
'        ObstList(J).Flags = -CInt(fDist = 0#)
'        MOC = arIASegmentMOC.Value * (1# - 2 * fDist / arIFHalfWidth.Value)
'
''        If FIXHeight - ObstList(J).Height < MOC Then
'            If FIXHeight <= ObstList(J).Height Then
'                rObs = arIFHalfWidth.Value
'            Else
'                rObs = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObstList(J).Height) / arIASegmentMOC.Value 'MOC '
'            End If
'
'            If rObs > arIFHalfWidth.Value Then rObs = arIFHalfWidth.Value
'
'            If GuidNav.NavTypeCode <> 1 Then
'                ObstList(J).Dist = Point2LineDistancePrj(ObstList(J).ptPrj, LastPoint.ptPrj, fDir + 90#)
'                ObstList(J).CLDist = Point2LineDistancePrj(ObstList(J).ptPrj, LastPoint.ptPrj, fDir)
'            Else
'                ObstList(J).Dist = DegToRad(SubtractAngles(fAlpha, ReturnAngleAsDegree(GuidNav.ptPrj, ObstList(J).ptPrj))) * fL
'                ObstList(J).CLDist = Abs(fL - ReturnDistanceAsMeter(ObstList(J).ptPrj, GuidNav.ptPrj))
'            End If
'
''            If (rObs > 0.5 * arIFHalfWidth.Value) And (ObstList(J).CLDist <= rObs) Then
'                ObstList(J).ptPrj.Z = ObstList(J).ptGeo.Z
'                ObstList(J).ptPrj.M = ObstList(J).ptGeo.M
'                ObstList(J).ID = pRow.Value(iID)
'                ObstList(J).Name = pRow.Value(iNAME)
'                ObstList(J).Index = I
'
'                ObstList(J).MOC = MOC
'                ObstList(J).ReqH = ObstList(J).Height + MOC
'                ObstList(J).hPent = ObstList(J).ReqH - FIXHeight
'                ObstList(J).fTmp = ObstList(J).hPent / ObstList(J).Dist
'
''                ProhibitionSectors(J).MOC = MOC
''                ProhibitionSectors(J).rObs = rObs
''                Set ProhibitionSectors(J).ObsArea = CreatePrjCircle(ObstList(J).ptPrj, rObs)
'
''                ProhibitionSectors(J).DirToObst = ReturnAngleAsDegree(LastPoint.ptPrj, ObstList(J).ptPrj)
' '               ProhibitionSectors(J).DistToObst = ReturnDistanceAsMeter(LastPoint.ptPrj, ObstList(J).ptPrj)
'
''                    If rObs < ProhibitionSectors(J).DistToObst Then
''                        fTmp = ArcSin(rObs / ProhibitionSectors(J).DistToObst)
''                        ProhibitionSectors(J).AlphaFrom = Round(Modulus(ProhibitionSectors(J).DirToObst - RadToDeg(fTmp), 360#) - 0.4999999)
''                        ProhibitionSectors(J).AlphaTo = Round(Modulus(ProhibitionSectors(J).DirToObst + RadToDeg(fTmp), 360#) + 0.4999999)
''                    Else
''                        ProhibitionSectors(J).AlphaFrom = 0
''                        ProhibitionSectors(J).AlphaTo = 360
''                    End If
'
''                ProhibitionSectors(J).dHObst = ObstList(J).Height - FIXHeight + MOC
''                ProhibitionSectors(J).Index = I
'                J = J + 1
''            End If
''        End If
'    End If
'
'    I = I + 1
'    Set pRow = pCursor.NextRow
'Wend
'
'If J > 0 Then
'    ReDim Preserve ObstList(J - 1)
'    FillSegObstList = J - 1
'Else
'    ReDim ObstList(-1 To -1)
'End If
'Set pQueryFilter = Nothing
'
'End Function

'Public Function CalcIFProhibitions(LastPoint As StepDownFIX, _
'    fRefHeight As Double, IAF_FullAreaPoly As Polygon, IAF_BaseAreaPoly As Polygon, _
'    ObstList() As ObstacleAr, ProhibitionSectors() As IFProhibitionSector) As Long
'
'Dim iObsShape As Long
'Dim iNAME As Long
'Dim iID As Long
'Dim I As Long
'Dim J As Long
'Dim N As Long
'
'Dim FIXHeight As Double
'Dim fAlpha As Double
''Dim fIADir As Double
'Dim fDist As Double
'Dim fDir As Double
'Dim fTmp As Double
'Dim rObs As Double
'Dim MOC As Double
'Dim fL As Double
'
'Dim pQueryFilter As IQueryFilter
'Dim pCursor As ICursor
'Dim pRow As IRow
''Dim ptFictIF As IPoint
'Dim pFullRelation As IRelationalOperator
'Dim pBaseProxi As IProximityOperator
'Dim pGeo As IGeometry
'Dim GuidNav As NavaidData
'
'CalcIFProhibitions = -1
'ReDim ObstList(-1 To -1)
'ReDim ProhibitionSectors(-1 To -1)
'
'
'Set pQueryFilter = New QueryFilter
'pQueryFilter.SubFields = "Shape, ID_OBS, Name"
'pQueryFilter.WhereClause = pObsTable.OIDFieldName + ">=0"
'
'N = pObsTable.RowCount(pQueryFilter) - 1
'If N < 0 Then
'    Set pQueryFilter = Nothing
'    Exit Function
'End If
'
'ReDim ObstList(0 To N)
'ReDim ProhibitionSectors(0 To N)
'
'Set pFullRelation = IAF_FullAreaPoly
'Set pBaseProxi = IAF_BaseAreaPoly
'I = 0
'J = 0
'
'FIXHeight = LastPoint.ptPrj.Z - fRefHeight
''fIADir = LastPoint.InDir
'fDir = LastPoint.InDir
'GuidNav = LastPoint.GuidNav
'
'fL = ReturnDistanceAsMeter(GuidNav.ptPrj, LastPoint.ptPrj)
'fAlpha = ReturnAngleAsDegree(GuidNav.ptPrj, LastPoint.ptPrj)
'
'
'Set pCursor = pObsTable.Search(pQueryFilter, False)
'iID = pCursor.FindField("ID_OBS")
'iObsShape = pCursor.FindField("Shape")
'iNAME = pCursor.FindField("Name")
'
'Set pRow = pCursor.NextRow
'While Not pRow Is Nothing
'
''                    ObstList(J).ID = pRow.Value(iID)
''                    ObstList(J).Name = pRow.Value(iName)
''    If (ObstList(J).ID = "709") Or (ObstList(J).ID = "1278") Then
''        I = I
''    End If
'
'    Set ObstList(J).ptGeo = pRow.Value(iObsShape)
'    Set ObstList(J).ptPrj = New Point
'    ObstList(J).ptPrj.PutCoords ObstList(J).ptGeo.X, ObstList(J).ptGeo.Y
'
'    Set pGeo = ObstList(J).ptPrj
'    Set pGeo.SpatialReference = pSpRefShp
'    pGeo.Project pSpRefPrj
'
'    If pFullRelation.Contains(ObstList(J).ptPrj) Then
'        fDist = pBaseProxi.ReturnDistance(ObstList(J).ptPrj)
'
'        ObstList(J).Height = ObstList(J).ptGeo.Z - fRefHeight
'        ObstList(J).Flags = -CInt(fDist = 0#)
'        MOC = arIASegmentMOC.Value * (1# - 2 * fDist / arIFHalfWidth.Value)
'
'        If FIXHeight - ObstList(J).Height < MOC Then
'            If FIXHeight <= ObstList(J).Height Then
'                rObs = arIFHalfWidth.Value
'            Else
'                rObs = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObstList(J).Height) / arIASegmentMOC.Value 'MOC '
'            End If
'
'            If rObs > arIFHalfWidth.Value Then rObs = arIFHalfWidth.Value
'
'            If GuidNav.NavTypeCode <> 1 Then
'                ObstList(J).Dist = Point2LineDistancePrj(ObstList(J).ptPrj, LastPoint.ptPrj, fDir + 90#)
'                ObstList(J).CLDist = Point2LineDistancePrj(ObstList(J).ptPrj, LastPoint.ptPrj, fDir)
'            Else
'                ObstList(J).Dist = DegToRad(SubtractAngles(fAlpha, ReturnAngleAsDegree(GuidNav.ptPrj, ObstList(J).ptPrj))) * fL
'                ObstList(J).CLDist = Abs(fL - ReturnDistanceAsMeter(ObstList(J).ptPrj, GuidNav.ptPrj))
'            End If
'
'            If (rObs > 0.5 * arIFHalfWidth.Value) And (ObstList(J).CLDist <= rObs) Then
'                ObstList(J).ptPrj.Z = ObstList(J).ptGeo.Z
'                ObstList(J).ptPrj.M = ObstList(J).ptGeo.M
'                ObstList(J).ID = pRow.Value(iID)
'                ObstList(J).Name = pRow.Value(iNAME)
'                ObstList(J).Index = J
'
'                ObstList(J).MOC = MOC
'                ObstList(J).ReqH = ObstList(J).Height + MOC
'                ObstList(J).hPent = ObstList(J).ReqH - FIXHeight
'                ObstList(J).fTmp = ObstList(J).hPent / ObstList(J).Dist
'
'                ProhibitionSectors(J).MOC = MOC
'                ProhibitionSectors(J).rObs = rObs
'                Set ProhibitionSectors(J).ObsArea = CreatePrjCircle(ObstList(J).ptPrj, rObs)
'
'                ProhibitionSectors(J).DirToObst = ReturnAngleAsDegree(LastPoint.ptPrj, ObstList(J).ptPrj)
'                ProhibitionSectors(J).DistToObst = ReturnDistanceAsMeter(LastPoint.ptPrj, ObstList(J).ptPrj)
'
''                    If rObs < ProhibitionSectors(J).DistToObst Then
''                        fTmp = ArcSin(rObs / ProhibitionSectors(J).DistToObst)
''                        ProhibitionSectors(J).AlphaFrom = Round(Modulus(ProhibitionSectors(J).DirToObst - RadToDeg(fTmp), 360#) - 0.4999999)
''                        ProhibitionSectors(J).AlphaTo = Round(Modulus(ProhibitionSectors(J).DirToObst + RadToDeg(fTmp), 360#) + 0.4999999)
''                    Else
''                        ProhibitionSectors(J).AlphaFrom = 0
''                        ProhibitionSectors(J).AlphaTo = 360
''                    End If
'
'                ProhibitionSectors(J).dHObst = ObstList(J).Height - FIXHeight + MOC
'                ProhibitionSectors(J).Index = I
'                J = J + 1
'            End If
'        End If
'    End If
'
'    I = I + 1
'    Set pRow = pCursor.NextRow
'Wend
'
'If J > 0 Then
'    ReDim Preserve ObstList(J - 1)
'    ReDim Preserve ProhibitionSectors(J - 1)
'    CalcIFProhibitions = J - 1
'Else
'    ReDim ObstList(-1 To -1)
'    ReDim ProhibitionSectors(-1 To -1)
'End If
'Set pQueryFilter = Nothing
'
'End Function
'

Private Function Create_ADT_Table(pFeatureWorkspace As IFeatureWorkspace, tableName As String) As ITable

    Dim pFieldsEdit As IFieldsEdit
    Dim pFieldEdit As IFieldEdit
    
    Dim strConfigWord As String
    strConfigWord = ""
    
On Error GoTo EH

  Set pFieldsEdit = New esriGeoDatabase.Fields
  
'=======================================OID, Identity
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "OID"
        .Type = esriFieldTypeOID
        .AliasName = "Object ID"
        .Editable = False
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================LegID, Long
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "LegID"
        .AliasName = "LegID"
        .Type = esriFieldTypeInteger
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================Coordinate, String
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "Coordinate"
        .AliasName = "Coordinate"
        .Type = esriFieldTypeString
        .length = 100
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================FixPoint, String
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "FixPoint"
        .AliasName = "FixPoint"
        .Type = esriFieldTypeString
        .length = 50
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================FixPointText, String
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "FixPointText"
        .AliasName = "FixPointText"
        .Type = esriFieldTypeString
        .length = 100
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================Checked, String
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "Checked"
        .AliasName = "Checked"
        .Type = esriFieldType.esriFieldTypeSmallInteger
        .length = 100
        .Editable = True
        .DefaultValue = 1
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================GeoX, double
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "GeoX"
        .AliasName = "GeoX"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 4
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================GeoY, double
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "GeoY"
        .AliasName = "GeoY"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 4
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================PointZ, double
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "PointZ"
        .AliasName = "PointZ"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 4
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================PointM, double
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "PointM"
        .AliasName = "PointM"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 4
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================

    Dim pUid As IUID
    Dim pFields As IFields
    
    Set pFields = pFieldsEdit
    Set pUid = New uID
    pUid.value = "esriCore.Feature"
    
    Set Create_ADT_Table = pFeatureWorkspace.CreateTable(tableName, pFields, Nothing, Nothing, strConfigWord)
EH:
    If Err.Number <> 0 Then
        MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, "createAccessWorkspace"
    End If
    
End Function

Private Function CreateLEGFeatureClass(pFeatureWorkspace As IFeatureWorkspace, className As String) As IFeatureClass
Dim pFieldsEdit As IFieldsEdit
Dim pFields As IFields
Dim pFieldEdit As IFieldEdit
Dim pGeomDef As IGeometryDefEdit
Dim ShapeFieldName As String

On Error GoTo EH

ShapeFieldName = "Shape"
  ' Add the Fields to the class the OID and Shape are compulsory

  Set pFieldsEdit = New esriGeoDatabase.Fields
'=======================================OID

    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "OID"
        .Type = esriFieldTypeOID
        .AliasName = "Object ID"
        .Editable = False
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================SHAPE
    Set pGeomDef = New GeometryDef
    With pGeomDef
        .AvgNumPoints = 2
        .GeometryType = esriGeometryPolyline
        .GridCount = 1
        .GridSize(0) = 1000
        .HasM = True
        .HasZ = True
        Set .SpatialReference = CopySpatialReference(pSpRefShp)
    End With

    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = ShapeFieldName
        .AliasName = ShapeFieldName
        .Type = esriFieldTypeGeometry
        .Editable = True
        .IsNullable = False
        Set .GeometryDef = pGeomDef
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================NAME
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "NAME"
        .AliasName = "NAME"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = False
        .length = 12
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================NO_SEQ
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "NO_SEQ"
        .AliasName = "NO_SEQ"
        .Type = esriFieldTypeSmallInteger
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_PHASE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_PHASE"
        .AliasName = "CODE_PHASE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_TYPE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_TYPE"
        .AliasName = "CODE_TYPE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = False
        .length = 2
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_COURSE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_COURSE"
        .AliasName = "VAL_COURSE"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 4
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
  
'=======================================CODE_TYPE_COURSE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_TYPE_COURSE"
        .AliasName = "CODE_TYPE_COURSE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 4
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_DIR_TURN
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_DIR_TURN"
        .AliasName = "CODE_DIR_TURN"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_TURN_VALID
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_TURN_VALID"
        .AliasName = "CODE_TURN_VALID"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_DESCR_DIST_VER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_DESCR_DIST_VER"
        .AliasName = "CODE_DESCR_DIST_VER"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_DIST_VER_UPPER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_DIST_VER_UPPER"
        .AliasName = "CODE_DIST_VER_UPPER"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit
    
'=======================================VAL_DIST_VER_UPPER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_DIST_VER_UPPER"
        .AliasName = "VAL_DIST_VER_UPPER"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 12
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit
    
'=======================================UOM_DIST_VER_UPPER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "UOM_DIST_VER_UPPER"
        .AliasName = "UOM_DIST_VER_UPPER"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 2
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_DIST_VER_LOWER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_DIST_VER_LOWER"
        .AliasName = "CODE_DIST_VER_LOWER"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_DIST_VER_LOWER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_DIST_VER_LOWER"
        .AliasName = "VAL_DIST_VER_LOWER"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 12
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================UOM_DIST_VER_LOWER
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "UOM_DIST_VER_LOWER"
        .AliasName = "UOM_DIST_VER_LOWER"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 2
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_VER_ANGLE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_VER_ANGLE"
        .AliasName = "VAL_VER_ANGLE"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 9
        .Precision = 6
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_SPEED_LIMIT
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_SPEED_LIMIT"
        .AliasName = "VAL_SPEED_LIMIT"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 10
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================UOM_SPEED
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "UOM_SPEED"
        .AliasName = "UOM_SPEED"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 10
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_SPEED_REF
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_SPEED_REF"
        .AliasName = "CODE_SPEED_REF"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 4
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_DIST
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_DIST"
        .AliasName = "VAL_DIST"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 12
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_DUR
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_DUR"
        .AliasName = "VAL_DUR"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 10
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================UOM_DUR
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "UOM_DUR"
        .AliasName = "UOM_DUR"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_THETA
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_THETA"
        .AliasName = "VAL_THETA"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 8
        .Precision = 3
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_RHO
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_RHO"
        .AliasName = "VAL_RHO"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 12
        .Precision = 8
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================VAL_BANK_ANGLE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "VAL_BANK_ANGLE"
        .AliasName = "VAL_BANK_ANGLE"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 7
        .Precision = 3
        .Scale = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================UOM_DIST_HORZ
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "UOM_DIST_HORZ"
        .AliasName = "UOM_DIST_HORZ"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 2
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_REP_ATC
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_REP_ATC"
        .AliasName = "CODE_REP_ATC"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 1
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================CODE_ROLE_FIX
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "CODE_ROLE_FIX"
        .AliasName = "CODE_ROLE_FIX"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 4
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================TXT_RMK
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "TXT_RMK"
        .AliasName = "TXT_RMK"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 5000
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================TRAKING_FACILITY
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "TRAKING_FACILITY"
        .AliasName = "TRAKING_FACILITY"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================TRAKING_TYPE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "TRAKING_TYPE"
        .AliasName = "TRAKING_TYPE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================INTERSECTING_FACILITY
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "INTERSECTING_FACILITY"
        .AliasName = "INTERSECTING_FACILITY"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================INTERSECTING_TYPE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "INTERSECTING_TYPE"
        .AliasName = "INTERSECTING_TYPE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 3
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================INTERSECTING_TYPE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "PROC_TYPE"
        .AliasName = "PROC_TYPE"
        .Type = esriFieldTypeString
        .Editable = True
        .IsNullable = True
        .length = 4
    End With
    pFieldsEdit.AddField pFieldEdit

''=======================================SHAPE_Length
'    Set pFieldEdit = New esriGeoDatabase.Field
'    With pFieldEdit
'        .Name = "SHAPE_Length"
'        .AliasName = "SHAPE_Length"
'        .Type = esriFieldTypeDouble
'        .Editable = True
'        .IsNullable = False
'        .Length = 6
'        .Precision = 6
'        .Scale = 1
'    End With
'    pFieldsEdit.AddField pFieldEdit

'=======================================INPUTDATE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "INPUTDATE"
        .AliasName = "INPUTDATE"
        .Type = esriFieldTypeDate
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit

'=======================================T_TYPE
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "T_TYPE"
        .AliasName = "T_TYPE"
        .Type = esriFieldTypeInteger
        .Editable = True
        .IsNullable = False
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================Radius_M
    Set pFieldEdit = New esriGeoDatabase.Field
    With pFieldEdit
        .Name = "Radius_M"
        .AliasName = "Radius_M"
        .Type = esriFieldTypeDouble
        .Editable = True
        .IsNullable = True
        .length = 6
        .Precision = 6
        .Scale = 0
    End With
    pFieldsEdit.AddField pFieldEdit
'=======================================

Dim pUid As IUID
    Set pFields = pFieldsEdit
    Set pUid = New uID
    pUid.value = "esriCore.Feature"
    Set CreateLEGFeatureClass = pFeatureWorkspace.CreateFeatureClass(className, pFields, Nothing, Nothing, esriFTSimple, ShapeFieldName, "")
    Exit Function
EH:
    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, "createAccessWorkspace"
End Function

Public Sub AddShapeFile(pFeatureWorkspace As IFeatureWorkspace, pFeatureClass As IFeatureClass)
Dim i As Long
Dim sName As String
Dim bLayerExist As Boolean
Dim pFeatureLayer As IFeatureLayer

    sName = pFeatureClass.AliasName
    bLayerExist = False

    For i = 0 To pMap.LayerCount - 1
        If pMap.Layer(i).Name = sName Then
            bLayerExist = True
            Exit For
        End If
    Next i

    If Not bLayerExist Then
'Create a new FeatureLayer and assign a shapefile to it
        Set pFeatureLayer = New FeatureLayer
        Set pFeatureLayer.FeatureClass = pFeatureClass
        pFeatureLayer.Name = sName
'Add the FeatureLayer to the focus map
        pMap.AddLayer pFeatureLayer
    End If
End Sub

Public Function Open_ADT_Table(pFeatWs As IFeatureWorkspace) As ITable
    Set Open_ADT_Table = Nothing
On Error Resume Next
    Set Open_ADT_Table = pFeatWs.OpenTable("ADT")
On Error GoTo EH
    If Open_ADT_Table Is Nothing Then
        Set Open_ADT_Table = Create_ADT_Table(pFeatWs, "ADT")
    End If
EH:
    If Err.Number <> 0 Then
        MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, "OpenLEGFeatureClass"
    End If
End Function

Public Function OpenLEGFeatureClass(pFeatWs As IFeatureWorkspace) As IFeatureClass
    Set OpenLEGFeatureClass = Nothing
On Error Resume Next
    Set OpenLEGFeatureClass = pFeatWs.OpenFeatureClass("LEGs")
On Error GoTo EH
    If OpenLEGFeatureClass Is Nothing Then ') And bCreateNew
        Set OpenLEGFeatureClass = CreateLEGFeatureClass(pFeatWs, "LEGs")
        AddShapeFile pFeatWs, OpenLEGFeatureClass
    End If

'    Set OpenLEGFeatureClass = pFeatWs
    Exit Function
EH:
    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, "OpenLEGFeatureClass"
End Function

Public Function Open_Workspace(FileName As String) As IFeatureWorkspace
Dim pWorkspaceFactory As IWorkspaceFactory
Dim L As Long
Dim Pos As Long
Dim Location As String
Dim FileNameForCreate As String

    L = Len(FileName)
    Pos = InStrRev(FileName, "\")

    If Pos <> 0 Then
        Location = Left(FileName, Pos)
        FileName = Right(FileName, L - Pos)
    Else
        Location = "\"
    End If

    Pos = InStrRev(FileName, ".")
    FileNameForCreate = Left(FileName, Pos - 1)
    FileName = FileNameForCreate + ".mdb"

'Create a new AccessWorkspaceFactory object and open a shapefile database
    Set pWorkspaceFactory = New AccessWorkspaceFactory
On Error Resume Next
    Set Open_Workspace = pWorkspaceFactory.OpenFromFile(Location + FileName, 0)
On Error GoTo EH

'Create a new AccessWorkspaceFactory object and open a shapefile database
    If Open_Workspace Is Nothing Then
        Dim pWorkspaceName As IWorkspaceName

'        Set pWorkspaceName = pWorkspaceFactory.Create(Location, FileNameForCreate, Nothing, 0)
        Set pWorkspaceName = pWorkspaceFactory.Create(Location, FileName, Nothing, 0)

        Set pWorkspaceFactory = pWorkspaceName.WorkspaceFactory
        Set Open_Workspace = pWorkspaceFactory.OpenFromFile(Location + FileName, 0)
        Set pWorkspaceName = Nothing
    End If
    Exit Function
EH:
    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, , "OpenLEGWorkspace"
End Function

Public Function OpenLEGWorkspace(FileName As String) As IFeatureWorkspace
Dim pWorkspaceFactory As IWorkspaceFactory
Dim L As Long
Dim Pos As Long
Dim Location As String
Dim FileNameForCreate As String

    L = Len(FileName)
    Pos = InStrRev(FileName, "\")

    If Pos <> 0 Then
        Location = Left(FileName, Pos)
        FileName = Right(FileName, L - Pos)
    Else
        Location = "\"
    End If

    Pos = InStrRev(FileName, ".")
    FileNameForCreate = Left(FileName, Pos - 1)
    FileName = FileNameForCreate + ".mdb"

'Create a new AccessWorkspaceFactory object and open a shapefile database
    Set pWorkspaceFactory = New AccessWorkspaceFactory
On Error Resume Next
    Set OpenLEGWorkspace = pWorkspaceFactory.OpenFromFile(Location + FileName, 0)
On Error GoTo EH

'Create a new AccessWorkspaceFactory object and open a shapefile database
    If OpenLEGWorkspace Is Nothing Then
        Dim pWorkspaceName As IWorkspaceName

'        Set pWorkspaceName = pWorkspaceFactory.Create(Location, FileNameForCreate, Nothing, 0)
        Set pWorkspaceName = pWorkspaceFactory.Create(Location, FileName, Nothing, 0)

        Set pWorkspaceFactory = pWorkspaceName.WorkspaceFactory
        Set OpenLEGWorkspace = pWorkspaceFactory.OpenFromFile(Location + FileName, 0)
        Set pWorkspaceName = Nothing
    End If
    Exit Function
EH:
    MsgDialog.ShowMessage CStr(Err.Number) + " :" + Err.Description, vbInformation, , "OpenLEGWorkspace"
End Function


Function PointAlongPlane(ptGeo As IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As IPoint
    dirAngle = DegToRad(dirAngle)
    Dim pClone As IClone
    Set pClone = ptGeo
    Set PointAlongPlane = pClone.Clone
    'Set PointAlongPlane = New Point
    PointAlongPlane.PutCoords ptGeo.X + Dist * Cos(dirAngle), ptGeo.y + Dist * Sin(dirAngle)

End Function

