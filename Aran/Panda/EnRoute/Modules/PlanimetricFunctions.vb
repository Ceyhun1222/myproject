Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module PlanimetricFunctions

	Public Function Point2LineDistancePrj(ByVal pInPt As Geometry.IPoint, ByVal pLinePt As Geometry.IPoint, ByVal direction As Double) As Double
		Dim CosA As Double
		Dim SinA As Double
		Dim dX As Double
		Dim dY As Double

		direction = DegToRadValue * direction
		CosA = System.Math.Cos(direction)
		SinA = System.Math.Sin(direction)

		dX = pInPt.X - pLinePt.X
		dY = pInPt.Y - pLinePt.Y

		Return System.Math.Abs(dY * CosA - dX * SinA)
	End Function

	Public Function CircleVectorIntersect(ByVal PtCent As Geometry.IPoint, ByVal R As Double, ByVal ptVect As Geometry.IPoint, ByVal DirVect As Double, Optional ByRef ptRes As Geometry.IPoint = Nothing, Optional ByVal bDirect As Boolean = True) As Double
		Dim ptTmp As Geometry.IPoint
		Dim DistCnt2Vect As Double
		Dim D As Double
		Dim Constr As Geometry.IConstructPoint

		ptTmp = New Geometry.Point
		Constr = ptTmp

		Constr.ConstructAngleIntersection(PtCent, DegToRad(DirVect + 90.0), ptVect, DegToRad(DirVect))
		DistCnt2Vect = ReturnDistanceInMeters(PtCent, ptTmp)

		If DistCnt2Vect < R Then
			D = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect)
			If bDirect Then
				ptRes = PointAlongPlane(ptTmp, DirVect, D)
			Else
				ptRes = PointAlongPlane(ptTmp, DirVect + 180.0, D)
			End If

			Return ReturnDistanceInMeters(ptRes, ptTmp)
		End If

		If DistCnt2Vect = R Then
			If SideDef(PtCent, DirVect, ptTmp) = 1 Then
				ptRes = PointAlongPlane(PtCent, DirVect - 90.0, R)
				Return R
			End If
			If SideDef(PtCent, DirVect, ptTmp) = -1 Then
				ptRes = PointAlongPlane(PtCent, DirVect + 90.0, R)
				Return R
			End If
		End If

		ptRes = New Geometry.Point
		Return 0.0
	End Function

	'Public Function CircleVectorIntersect0(ByVal PtCent As Geometry.IPoint, ByVal R As Double, ByVal ptVect As Geometry.IPoint, ByVal DirVect As Double, Optional ByRef ptRes As Geometry.IPoint = Nothing) As Double
	'	Dim ptTmp As Geometry.IPoint
	'	Dim DistCnt2Vect As Double
	'	Dim D As Double
	'	Dim Constr As Geometry.IConstructPoint

	'	ptTmp = New Geometry.Point
	'	Constr = ptTmp

	'	Constr.ConstructAngleIntersection(PtCent, DegToRad(DirVect + 90.0), ptVect, DegToRad(DirVect))

	'	DistCnt2Vect = ReturnDistanceInMeters(PtCent, ptTmp)

	'	If DistCnt2Vect < R Then
	'		D = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect)
	'		ptRes = PointAlongPlane(ptTmp, DirVect, D)
	'		Return D ' ReturnDistanceInMeters(ptRes, ptTmp)
	'	End If

	'	ptRes = New Geometry.Point
	'	Return 0.0
	'End Function

	'Public Function RayPolylineIntersect(ByVal pPolyline As Geometry.Polyline, ByVal RayPt As Geometry.Point, ByVal RayDir As Double, ByRef InterPt As Geometry.IPoint) As Boolean
	'	Dim I As Integer
	'	Dim N As Integer
	'	Dim D As Double
	'	Dim dMin As Double
	'	Dim pLine As Geometry.IPolyline
	'	Dim pPoints As Geometry.IPointCollection
	'	Dim pTopo As Geometry.ITopologicalOperator

	'	pLine = New Geometry.Polyline
	'	pLine.FromPoint = RayPt
	'	dMin = 5000000.0
	'	pLine.ToPoint = PointAlongPlane(RayPt, RayDir, dMin)

	'	pTopo = pPolyline
	'	pPoints = pTopo.Intersect(pLine, Geometry.esriGeometryDimension.esriGeometry0Dimension)
	'	N = pPoints.PointCount

	'	If N = 0 Then Return False

	'	If N = 1 Then
	'		InterPt = pPoints.Point(0)
	'	Else
	'		For I = 0 To N - 1
	'			D = ReturnDistanceInMeters(RayPt, pPoints.Point(I))
	'			If D < dMin Then
	'				dMin = D
	'				InterPt = pPoints.Point(I)
	'			End If
	'		Next
	'	End If

	'	Return True
	'End Function

	'Public Function LineLineIntersect(ByVal pt1 As Geometry.Point, ByVal Dir1 As Double, ByVal pt2 As Geometry.Point, ByVal Dir2 As Double) As Geometry.Point
	'	Dim Constructor As Geometry.IConstructPoint
	'	Constructor = New Geometry.Point
	'	Constructor.ConstructAngleIntersection(pt1, DegToRad(Dir1), pt2, DegToRad(Dir2))
	'	Return Constructor
	'End Function

	'Public Function DistanceToCircleBound(ByVal PtCent As Geometry.IPoint, ByVal R As Double, ByVal ptCurr As Geometry.IPoint, ByVal Direct As Double) As Double
	'	Dim DirCnt2Pnt As Double
	'	Dim DistCnt2Pnt As Double
	'	Dim D As Double

	'	DirCnt2Pnt = ReturnAngleInDegrees(PtCent, ptCurr)
	'	DistCnt2Pnt = ReturnDistanceInMeters(PtCent, ptCurr)

	'	If DistCnt2Pnt < R Then
	'		D = DistCnt2Pnt * System.Math.Cos(DegToRad(Direct - DirCnt2Pnt))
	'		Return R - D
	'	End If

	'	Return 0.0
	'End Function

	Public Function ReturnDistanceInMeters(ByVal ptFrom As Geometry.IPoint, ByVal ptTo As Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return System.Math.Sqrt(fdX * fdX + fdY * fdY)
	End Function

	Public Function ReturnAngleInDegrees(ByVal ptFrom As Geometry.IPoint, ByVal ptTo As Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return Modulus(RadToDeg(Math.Atan2(fdY, fdX)), 360.0)
	End Function

	Function PointAlongPlane(ByVal ptFrom As Geometry.IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As Geometry.IPoint
		PointAlongPlane = New Geometry.Point
		dirAngle = DegToRadValue * dirAngle
		PointAlongPlane.PutCoords(ptFrom.X + Dist * System.Math.Cos(dirAngle), ptFrom.Y + Dist * System.Math.Sin(dirAngle))
	End Function

	Public Function GetSymmetricPoint(ByRef PtInLine As Geometry.IPoint, ByRef LineAngle As Double, ByRef PtOrig As Geometry.IPoint) As Geometry.Point
		Dim fDist As Double
		Dim Side As Integer

		fDist = Point2LineDistancePrj(PtOrig, PtInLine, LineAngle)
		Side = SideDef(PtInLine, LineAngle, PtOrig)
		GetSymmetricPoint = PointAlongPlane(PtOrig, LineAngle + Side * 90.0, 2.0 * fDist)
	End Function

	'Function CalcTrajectoryFromMultiPoint(ByVal MultiPoint As Geometry.IPointCollection) As Geometry.Polyline
	'	Dim ptConstr As Geometry.IConstructPoint
	'	Dim FromPt As Geometry.IPoint
	'	Dim CntPt As Geometry.IPoint
	'	Dim ToPt As Geometry.IPoint

	'	Dim fTmp As Double
	'	Dim fE As Double

	'	Dim Side As Integer
	'	Dim I As Integer
	'	Dim N As Integer

	'	Dim pPolyline As Geometry.IGeometryCollection
	'	Dim pPath As Geometry.IPointCollection

	'	CntPt = New Geometry.Point
	'	ptConstr = CntPt
	'	fE = 0.5 * DegToRadValue

	'	CalcTrajectoryFromMultiPoint = New Geometry.Polyline
	'	pPolyline = CalcTrajectoryFromMultiPoint

	'	N = MultiPoint.PointCount - 2

	'	For I = 0 To N
	'		FromPt = MultiPoint.Point(I)
	'		ToPt = MultiPoint.Point(I + 1)
	'		fTmp = DegToRadValue * (FromPt.M - ToPt.M)

	'		If (System.Math.Abs(System.Math.Sin(fTmp)) <= fE) And (System.Math.Cos(fTmp) > 0.0) Then
	'			pPath = New Geometry.Path
	'			pPath.AddPoint(FromPt)
	'			pPath.AddPoint(ToPt)
	'			pPolyline.AddGeometry(pPath)
	'		Else
	'			If System.Math.Abs(System.Math.Sin(fTmp)) > fE Then
	'				ptConstr.ConstructAngleIntersection(FromPt, DegToRadValue * (FromPt.M + 90.0), ToPt, DegToRadValue * (ToPt.M + 90.0))
	'			Else
	'				CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y))
	'			End If

	'			Side = SideDef(FromPt, FromPt.M, ToPt)

	'			pPath = New Geometry.Path
	'			pPath.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -Side))
	'			pPolyline.AddGeometry(pPath)
	'		End If
	'	Next
	'End Function

	'Sub CreateGuidanceTrapeze(ByRef pt As Geometry.IPoint, ByRef azt As Double, ByRef BaseDist As Double, ByRef Alpha As Double, ByRef pPolygonTotal As Geometry.IPointCollection, ByRef pPolygonBase As Geometry.IPointCollection)
	'	Dim pt1 As Geometry.IPoint
	'	Dim pt2 As Geometry.IPoint
	'	Dim pt3 As Geometry.IPoint
	'	Dim pt4 As Geometry.IPoint
	'	Dim resx As Double
	'	Dim resy As Double
	'	Dim azt12 As Double
	'	Dim azt21 As Double
	'	Dim Dist As Double
	'	Dim MaxW As Double
	'	Dim pGeo As Geometry.IGeometry
	'	Dim J As Integer

	'	Dim pTopoOper As Geometry.ITopologicalOperator
	'	pt1 = New Geometry.Point
	'	pt2 = New Geometry.Point
	'	pt3 = New Geometry.Point
	'	pt4 = New Geometry.Point
	'	J = PointAlongGeodesic(pt.X, pt.Y, BaseDist, azt - 90, resx, resy)
	'	pt1.PutCoords(resx, resy)
	'	J = PointAlongGeodesic(pt.X, pt.Y, BaseDist, azt + 90, resx, resy)
	'	pt2.PutCoords(resx, resy)

	'	MaxW = 18600.0
	'	Dist = 0.5 * MaxW / System.Math.Tan(DegToRad(Alpha))
	'	J = PointAlongGeodesic(pt1.X, pt1.Y, Dist, azt - Alpha, resx, resy)
	'	pt3.PutCoords(resx, resy)
	'	J = PointAlongGeodesic(pt2.X, pt2.Y, Dist, azt + Alpha, resx, resy)
	'	pt4.PutCoords(resx, resy)

	'	pPolygonTotal = New Geometry.Polygon
	'	pPolygonTotal.AddPoint(pt1)
	'	pPolygonTotal.AddPoint(pt3)
	'	pPolygonTotal.AddPoint(pt4)
	'	pPolygonTotal.AddPoint(pt2)
	'	pPolygonTotal.AddPoint(pt1)

	'	pGeo = pPolygonTotal
	'	pGeo.SpatialReference = pSpRefShp
	'	pGeo.Project(pSpRefPrj)
	'	pPolygonTotal = pGeo
	'	pTopoOper = pPolygonTotal
	'	pTopoOper.Simplify()

	'	J = ReturnGeodesicAzimuth(pt1.X, pt1.Y, pt2.X, pt2.Y, azt12, azt21)
	'	Dist = ReturnGeodesicDistance(pt1.X, pt1.Y, pt2.X, pt2.Y)
	'	Dist = Dist / 4
	'	J = PointAlongGeodesic(pt1.X, pt1.Y, Dist, azt12, resx, resy)
	'	pt1.PutCoords(resx, resy)
	'	J = PointAlongGeodesic(pt2.X, pt2.Y, Dist, azt21, resx, resy)
	'	pt2.PutCoords(resx, resy)

	'	J = ReturnGeodesicAzimuth(pt3.X, pt3.Y, pt4.X, pt4.Y, azt12, azt21)
	'	Dist = ReturnGeodesicDistance(pt3.X, pt3.Y, pt4.X, pt4.Y)
	'	Dist = Dist / 4
	'	J = PointAlongGeodesic(pt3.X, pt3.Y, Dist, azt12, resx, resy)
	'	pt3.PutCoords(resx, resy)
	'	J = PointAlongGeodesic(pt4.X, pt4.Y, Dist, azt21, resx, resy)
	'	pt4.PutCoords(resx, resy)

	'	pPolygonBase = New Geometry.Polygon
	'	pPolygonBase.AddPoint(pt1)
	'	pPolygonBase.AddPoint(pt3)
	'	pPolygonBase.AddPoint(pt4)
	'	pPolygonBase.AddPoint(pt2)
	'	pPolygonBase.AddPoint(pt1)
	'	pGeo = pPolygonBase
	'	pGeo.SpatialReference = pSpRefShp
	'	pGeo.Project(pSpRefPrj)
	'	pPolygonBase = pGeo
	'	pTopoOper = pPolygonBase
	'	pTopoOper.Simplify()
	'End Sub

	Function SpiralTouchAngle(ByRef r0 As Double, ByRef E As Double, ByRef NominalDir As Double, ByRef TouchDir As Double, ByRef TurnTo As Integer) As Double
		Dim I As Integer
		Dim D As Double
		Dim DegE As Double
		Dim delta As Double
		Dim TurnAngle As Double
		Dim TouchAngle As Double

		TouchAngle = DegToRadValue * Modulus((TouchDir - NominalDir) * TurnTo, 360.0)
		TurnAngle = TouchAngle
		DegE = RadToDeg(E)

		For I = 0 To 9
			D = DegE / (r0 + DegE * TurnAngle)
			delta = (TurnAngle - TouchAngle - System.Math.Atan(D)) / (2.0 - 1.0 / (D * D + 1.0))
			TurnAngle = TurnAngle - delta

			If (System.Math.Abs(delta) < radEps) Then Exit For
		Next I

		Return Modulus(RadToDegValue * TurnAngle, 360.0)
	End Function

	'Function SpiralTouchAngleNew(ByRef r0 As Double, ByRef coef0 As Double, ByRef aztNominal As Double, ByRef AztTouch As Double, ByRef Turndir As Integer) As Double
	'	Dim I As Integer
	'	Dim TurnAngle As Double
	'	Dim TouchAngle As Double
	'	Dim delta As Double
	'	Dim coef As Double
	'	Dim Theta0 As Double
	'	Dim f As Double
	'	Dim dF As Double
	'	Dim TanAlpha As Double
	'	Dim R As Double
	'	Dim SinTheta As Double
	'	Dim CosTheta As Double
	'	Dim turnAnglenew As Double
	'	Theta0 = Modulus(aztNominal - 90.0 * Turndir, 360.0)

	'	TouchAngle = Modulus(AztTouch - Theta0 * Turndir, 360.0)
	'	TouchAngle = DegToRad(TouchAngle)
	'	TanAlpha = System.Math.Tan(TouchAngle)
	'	TurnAngle = DegToRad(Modulus((AztTouch - aztNominal) * Turndir, 360.0))
	'	coef = RadToDeg(coef0)

	'	For I = 0 To 9
	'		R = r0 + coef * TurnAngle
	'		SinTheta = System.Math.Sin(TurnAngle)
	'		CosTheta = System.Math.Cos(TurnAngle)
	'		f = R * CosTheta + coef * SinTheta + (R * SinTheta - coef * CosTheta) * TanAlpha
	'		dF = -R * SinTheta + 2.0 * coef * CosTheta + (R * CosTheta + 2 * coef * SinTheta) * TanAlpha
	'		turnAnglenew = TurnAngle - f / dF
	'		'd = coef / (r0 + coef * turnAngle)
	'		'delta = (turnAngle - TouchAngle - Atn(d)) / (2# - 1# / (d * d + 1#))
	'		delta = Modulus(TurnAngle - turnAnglenew, 360.0)
	'		If (delta > DegToRad(180.0)) Then
	'			delta = DegToRad(360.0) - delta
	'		End If
	'		If (System.Math.Abs(delta) < radEps) Then
	'			Exit For
	'		End If
	'		TurnAngle = Modulus(turnAnglenew, 2 * PI)
	'	Next I
	'	SpiralTouchAngleNew = RadToDeg(TurnAngle)

	'	return Modulus(SpiralTouchAngleNew, 360.0)

	'End Function

	Sub CreateWindSpiral(ByVal pt As Geometry.IPoint, ByVal NomDir As Double, ByVal StartDir As Double, ByVal EndDir As Double, ByVal r0 As Double, ByVal coef As Double, ByVal TurnTo As Integer, ByRef pPointCollection As Geometry.IPointCollection)
		Dim I As Integer
		Dim N As Integer

		Dim TurnAng As Double
		Dim dAlpha As Double
		Dim dPhi0 As Double
		Dim azt0 As Double
		Dim dPhi As Double
		Dim R As Double

		Dim ptCur As Geometry.IPoint
		Dim ptCnt As Geometry.IPoint

		ptCnt = PointAlongPlane(pt, NomDir + 90.0 * TurnTo, r0)
		'DrawPoint ptCnt, 0
		'DrawPoint Pt, 255

		If SubtractAngles(NomDir, EndDir) < degEps Then EndDir = NomDir

		dPhi0 = (StartDir - NomDir) * TurnTo
		dPhi0 = Modulus(dPhi0, 360.0)

		If (dPhi0 < 0.001) Then
			dPhi0 = 0.0
		Else
			dPhi0 = SpiralTouchAngle(r0, coef, NomDir, StartDir, TurnTo)
		End If

		'DrawPolygon pPointCollection, 0

		dPhi = SpiralTouchAngle(r0, coef, NomDir, EndDir, TurnTo)

		TurnAng = dPhi - dPhi0

		azt0 = NomDir + (dPhi0 - 90.0) * TurnTo
		azt0 = Modulus(azt0, 360.0)

		If (TurnAng < 0.0) Then Return

		dAlpha = 1.0
		N = TurnAng / dAlpha

		If (N < 1) Then
			N = 1
		ElseIf (N < 5) Then
			N = 5
		ElseIf (N < 10) Then
			N = 10
		End If

		dAlpha = TurnAng / N

		ptCur = New Geometry.Point
		For I = 0 To N
			R = r0 + (dAlpha * coef * I) + dPhi0 * coef
			ptCur = PointAlongPlane(ptCnt, azt0 + (I * dAlpha) * TurnTo, R)
			pPointCollection.AddPoint(ptCur)
		Next I
	End Sub

	'Function WindSpiralByTurnAngle(ByRef pt As Geometry.IPoint, ByRef StartDir As Double, ByRef TurnAng As Double, ByRef r0 As Double, ByRef coef As Double, ByRef TurnTo As Integer) As Geometry.Polygon
	'	Dim Result As Geometry.IPointCollection
	'	Dim I As Integer
	'	Dim N As Integer

	'	Dim R As Double
	'	Dim azt0 As Double
	'	Dim dAlpha As Double

	'	Dim ptCur As Geometry.IPoint
	'	Dim ptCnt As Geometry.IPoint

	'	Result = New Geometry.Polygon

	'	If (TurnAng < 0.0) Then Return Result

	'	ptCnt = PointAlongPlane(pt, StartDir + 90.0 * TurnTo, r0)
	'	azt0 = StartDir - 90.0 * TurnTo

	'	dAlpha = 1.0
	'	N = TurnAng / dAlpha
	'	If (N < 1) Then
	'		N = 1
	'	ElseIf (N < 5) Then
	'		N = 5
	'	ElseIf (N < 10) Then
	'		N = 10
	'	End If

	'	dAlpha = TurnAng / N

	'	ptCur = New Geometry.Point
	'	For I = 0 To N
	'		R = r0 + (dAlpha * coef * I)
	'		ptCur = PointAlongPlane(ptCnt, azt0 + I * TurnTo * dAlpha, R)
	'		WindSpiralByTurnAngle.AddPoint(ptCur)
	'	Next I

	'	Return Result
	'End Function

	'Sub CreateFIXZoneBy2VOR(ByVal Vor1 As Geometry.IPoint, ByVal phi1 As Double, ByVal Vor2 As Geometry.IPoint, ByVal phi2 As Double, ByVal FIXPoint As Geometry.IPoint, ByRef pPolygon As Geometry.IPolygon)
	'	Dim I As Integer
	'	Dim J As Integer

	'	Dim Xa(3) As Double
	'	Dim Ya(3) As Double
	'	Dim H As Double

	'	Dim pTopoOper As Geometry.ITopologicalOperator2
	'	Dim triangle1 As Geometry.IPointCollection
	'	Dim triangle2 As Geometry.IPointCollection
	'	Dim ptCur As Geometry.IPoint

	'	ptCur = New Geometry.Point()
	'	triangle1 = New Geometry.Polygon()
	'	triangle2 = New Geometry.Polygon()

	'	I = TriangleBy2PointAndAngle(Vor1.X, Vor1.Y, FIXPoint.X, FIXPoint.Y, phi1, H, Xa(0), Ya(0))

	'	For J = 0 To I - 1
	'		ptCur.PutCoords(Xa(J), Ya(J))
	'		triangle1.AddPoint(ptCur)
	'	Next J

	'	I = TriangleBy2PointAndAngle(Vor2.X, Vor2.Y, FIXPoint.X, FIXPoint.Y, phi1, H, Xa(0), Ya(0))

	'	For J = 0 To I - 1
	'		ptCur.PutCoords(Xa(J), Ya(J))
	'		triangle2.AddPoint(ptCur)
	'	Next J

	'	pTopoOper = triangle2
	'	pTopoOper.IsKnownSimple_2 = False
	'	pTopoOper.Simplify()

	'	pTopoOper = triangle1
	'	pTopoOper.IsKnownSimple_2 = False
	'	pTopoOper.Simplify()

	'	pPolygon = pTopoOper.Intersect(triangle2, Geometry.esriGeometryDimension.esriGeometry2Dimension)
	'End Sub

	'Public Function LineVectIntersect(ByRef pt1 As Geometry.IPoint, ByRef pt2 As Geometry.IPoint, ByRef pt3 As Geometry.IPoint, ByRef azt As Double, ByRef ptRes As Geometry.IPoint) As Integer
	'	Dim Az As Double
	'	Dim SinAz As Double
	'	Dim CosAz As Double
	'	Dim UaDenom As Double
	'	Dim UaNumer As Double
	'	Dim Ua As Double

	'	Az = DegToRad(azt)
	'	SinAz = System.Math.Sin(Az)
	'	CosAz = System.Math.Cos(Az)

	'	ptRes = New Geometry.Point

	'	UaDenom = SinAz * (pt2.X - pt1.X) - CosAz * (pt2.Y - pt1.Y)
	'	If UaDenom = 0.0 Then Return -2

	'	UaNumer = CosAz * (pt1.Y - pt3.Y) - SinAz * (pt1.X - pt3.X)

	'	Ua = UaNumer / UaDenom
	'	ptRes.PutCoords(pt1.X + Ua * (pt2.X - pt1.X), pt1.Y + Ua * (pt2.Y - pt1.Y))

	'	If Ua < 0.0 Then Return -1
	'	If Ua > 1.0 Then Return 1
	'	Return 0
	'End Function

	'Public Function PolygonDifference(ByRef Source As Geometry.Polygon, ByRef Subtractor As Geometry.Polygon) As Geometry.Polygon
	'	Dim pTopo As Geometry.ITopologicalOperator2

	'	pTopo = Source
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	pTopo = Source
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	On Error Resume Next

	'	PolygonDifference = pTopo.Difference(Subtractor)
	'	If Err.Number = 0 Then Exit Function

	'	PolygonDifference = Subtractor
	'End Function

	'Public Function PolygonIntersection(ByRef pPoly1 As Geometry.Polygon, ByRef pPoly2 As Geometry.Polygon) As Geometry.Polygon
	'	Dim pTopo As Geometry.ITopologicalOperator2
	'	Dim pTmpPoly0 As Geometry.Polygon
	'	Dim pTmpPoly1 As Geometry.Polygon

	'	pTopo = pPoly2
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	pTopo = pPoly1
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	On Error Resume Next

	'	PolygonIntersection = pTopo.Intersect(pPoly2, Geometry.esriGeometryDimension.esriGeometry2Dimension)
	'	If Err.Number = 0 Then Exit Function
	'	Err.Clear()

	'	pTmpPoly0 = pTopo.Union(pPoly2)
	'	pTmpPoly1 = pTopo.SymmetricDifference(pPoly2)

	'	pTopo = pTmpPoly0
	'	PolygonIntersection = pTopo.Difference(pTmpPoly1)

	'	If Err.Number = 0 Then Exit Function
	'	PolygonIntersection = pPoly2
	'End Function

	Public Function CreatePrjCircle(ByVal pPoint1 As Geometry.IPoint, ByVal R As Double, Optional ByVal N As Integer = 360) As Geometry.IPointCollection
		Dim I As Integer
		Dim iInRad As Double
		Dim dA As Double

		Dim Pt As Geometry.IPoint
		Dim pPolygon As Geometry.IPointCollection
		Dim pTopo As Geometry.ITopologicalOperator2

		Pt = New Geometry.Point
		pPolygon = New Geometry.Polygon
		dA = 360.0 * DegToRadValue / N

		N = N - 1
		For I = 0 To N
			iInRad = I * dA
			Pt.X = pPoint1.X + R * System.Math.Cos(iInRad)
			Pt.Y = pPoint1.Y + R * System.Math.Sin(iInRad)
			pPolygon.AddPoint(Pt)
		Next I

		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return pPolygon
	End Function

	'Function CreateArcPrj(ByVal ptCnt As Geometry.IPoint, ByVal ptFrom As Geometry.IPoint, ByVal ptTo As Geometry.IPoint, ByVal ClWise As Integer) As Geometry.Polygon
	'	Dim I As Integer
	'	Dim J As Integer

	'	Dim R As Double
	'	Dim dX As Double
	'	Dim dY As Double
	'	Dim dAz As Double
	'	Dim ToDir As Double
	'	Dim FromDir As Double

	'	Dim iInRad As Double
	'	Dim AngStep As Double

	'	Dim pt As Geometry.IPoint
	'	Dim Result As Geometry.IPointCollection

	'	pt = New Geometry.Point()
	'	Result = New Geometry.Polygon()

	'	dX = ptFrom.X - ptCnt.X
	'	dY = ptFrom.Y - ptCnt.Y
	'	R = System.Math.Sqrt(dX * dX + dY * dY)

	'	FromDir = Modulus(RadToDeg(Math.Atan2(dY, dX)), 360.0)
	'	ToDir = Modulus(RadToDeg(Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X)), 360.0)

	'	dAz = Modulus((ToDir - FromDir) * ClWise, 360.0)
	'	I = dAz

	'	If I < 1 Then
	'		I = 1
	'	ElseIf I < 5 Then
	'		I = 5
	'	ElseIf I < 10 Then
	'		I = 10
	'	End If
	'	AngStep = dAz / I

	'	Result.AddPoint(ptFrom)
	'	For J = 1 To I - 1
	'		iInRad = DegToRad(FromDir + J * AngStep * ClWise)
	'		pt.X = ptCnt.X + R * System.Math.Cos(iInRad)
	'		pt.Y = ptCnt.Y + R * System.Math.Sin(iInRad)
	'		Result.AddPoint(pt)
	'	Next J

	'	Result.AddPoint(ptTo)
	'	Return Result
	'End Function

	'Public Function CreateSectorPrj(ByVal pPtCenter As Geometry.IPoint, ByVal R As Double, ByVal stDir As Double, ByVal endDir As Double, ByVal ClWise As Integer) As Geometry.IPointCollection
	'	Dim ptFrom As Geometry.IPoint
	'	Dim ptTo As Geometry.IPoint

	'	ptFrom = PointAlongPlane(pPtCenter, stDir, R)
	'	ptTo = PointAlongPlane(pPtCenter, endDir, R)

	'	CreateSectorPrj = CreateArcPrj(pPtCenter, ptFrom, ptTo, ClWise)
	'	CreateSectorPrj.AddPoint(pPtCenter)
	'	CreateSectorPrj.AddPoint(pPtCenter, 0)
	'End Function

	Public Function ReArrangePolygon(ByVal pPolygon As Geometry.Polygon, ByVal PtDerL As Geometry.IPoint, ByVal CLDir As Double, Optional ByVal bFlag As Boolean = False) As Geometry.Polygon
		Dim I As Integer
		Dim J As Integer

		Dim N As Integer
		Dim iStart As Integer

		Dim dl As Double
		Dim dm As Double

		Dim dX0 As Double
		Dim dY0 As Double

		Dim dX1 As Double
		Dim dY1 As Double

		Dim pPt As Geometry.IPoint
		Dim pClone As esriSystem.IClone
		Dim pPoly As Geometry.IPointCollection
		Dim Result As Geometry.IPointCollection
		Dim pTmpPoly As Geometry.IPointCollection
		Dim pTopoOper As Geometry.ITopologicalOperator2

		pClone = pPolygon
		pTmpPoly = pClone.Clone

		pTopoOper = pTmpPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		Result = New Geometry.Polygon

		If pTmpPoly.PointCount <= 3 Then Return Result

		pPoly = New Geometry.Polyline()
		pPoly.AddPointCollection(pTmpPoly)

		pPoly.RemovePoints(0, 1)
		N = pPoly.PointCount

		pPt = PointAlongPlane(PtDerL, CLDir + 180.0, 30000.0)
		dm = Point2LineDistancePrj(pPoly.Point(0), pPt, CLDir + 90.0) * SideDef(pPt, CLDir, pPoly.Point(0))

		iStart = -1
		If dm < 0 Then iStart = 0

		For I = 1 To N - 1
			dl = Point2LineDistancePrj(pPoly.Point(I), pPt, CLDir + 90.0) * SideDef(pPt, CLDir, pPoly.Point(I))
			If (dl < 0.0) And ((dl > dm) Or (dm >= 0.0)) Then
				dm = dl
				iStart = I
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
		dY0 = pPoly.Point(1).Y - pPoly.Point(0).Y
		I = 1
		Do While I < N
			J = Modulus(I + 1, N)
			dX1 = pPoly.Point(J).X - pPoly.Point(I).X
			dY1 = pPoly.Point(J).Y - pPoly.Point(I).Y
			dl = ReturnDistanceInMeters(pPoly.Point(J), pPoly.Point(I))

			If dl < distEps Then
				pPoly.RemovePoints(I, 1)
				N -= 1
				J = Modulus(I + 1, N)
				If I <= iStart Then iStart -= 1

				dX1 = dX0
				dY1 = dY0
			ElseIf (dY0 <> 0.0) And (I <> iStart) Then
				If dY1 <> 0.0 Then
					If System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001 Then
						pPoly.RemovePoints(I, 1)
						N -= 1
						J = (I + 1) Mod N
						If I <= iStart Then
							iStart -= 1
						End If
						dX1 = dX0
						dY1 = dY0
					Else
						I += 1
					End If
				Else
					I += 1
				End If
			ElseIf (dX0 <> 0.0) And (I <> iStart) Then
				If dX1 <> 0.0 Then
					If System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001 Then
						pPoly.RemovePoints(I, 1)
						N -= 1
						J = (I + 1) Mod N
						If I <= iStart Then iStart -= 1
						dX1 = dX0
						dY1 = dY0
					Else
						I += 1
					End If
				Else
					I += 1
				End If
			Else
				I += 1
			End If
			dX0 = dX1
			dY0 = dY1
		Loop

		N = pPoly.PointCount

		For I = N - 1 To 0 Step -1
			J = Modulus(I + iStart, N)
			Result.AddPoint(pPoly.Point(J))
		Next

		Return Result
	End Function

	Public Function ReturnPolygonPartAsPolyline(ByVal pPolygon As Geometry.Polygon, ByVal PtDerL As Geometry.IPoint, ByVal CLDir As Double, ByVal Turn As Integer) As Geometry.Polyline
		Dim I As Integer
		Dim N As Integer
		Dim Side As Integer
		Dim pPt As Geometry.IPoint
		Dim pLine As Geometry.IPolyline
		Dim pTmpPoly As Geometry.IPointCollection
		Dim Result As Geometry.IPointCollection

		Result = New Geometry.Polyline()
		pLine = Result

		pTmpPoly = RemoveAgnails(pPolygon)
		pTmpPoly = ReArrangePolygon(pTmpPoly, PtDerL, CLDir)

		N = pTmpPoly.PointCount - 1

		pPt = PointAlongPlane(PtDerL, CLDir + 180.0, 30000.0)
		For I = 0 To N
			Side = SideDef(pPt, CLDir, pTmpPoly.Point(I))
			If Side = Turn Then
				Result.AddPoint(pTmpPoly.Point(I))
			End If
		Next

		If Turn < 0 Then pLine.ReverseOrientation()
		Return pLine
	End Function

	Function RemoveAgnails(ByVal pPolygon As Geometry.Polygon) As Geometry.Polygon
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer

		Dim dl As Double
		Dim dX0 As Double
		Dim dY0 As Double
		Dim dX1 As Double
		Dim dY1 As Double

		Dim pTopo As Geometry.ITopologicalOperator2
		Dim Result As Geometry.IPointCollection
		Dim pPGone As Geometry.IPolygon2
		Dim pClone As esriSystem.IClone

		pClone = pPolygon
		Result = pClone.Clone

		pPGone = Result
		pPGone.Close()

		pTopo = Result
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		N = Result.PointCount - 1

		If N <= 3 Then Return Result

		Result.RemovePoints(N, 1)

		J = 0
		Do While J < N
			If N < 4 Then Exit Do

			K = (J + 1) Mod N
			L = (J + 2) Mod N

			dX0 = Result.Point(K).X - Result.Point(J).X
			dY0 = Result.Point(K).Y - Result.Point(J).Y

			dX1 = Result.Point(L).X - Result.Point(K).X
			dY1 = Result.Point(L).Y - Result.Point(K).Y

			dl = dX1 * dX1 + dY1 * dY1

			If dl < 0.00001 Then
				Result.RemovePoints(K, 1)
				N -= 1
				If J >= N Then J = N - 1
			ElseIf (dY0 <> 0.0) Then
				If dY1 <> 0.0 Then
					If System.Math.Abs(dX0 / dY0 - dX1 / dY1) < 0.0001 Then
						Result.RemovePoints(K, 1)
						N -= 1
						J = (J - 2) Mod N
						If J < 0 Then J = 0 'J = J + N
					Else
						J += 1
					End If
				Else
					J += 1
				End If
			ElseIf (dX0 <> 0.0) Then
				If dX1 <> 0.0 Then
					If System.Math.Abs(dY0 / dX0 - dY1 / dX1) < 0.0001 Then
						Result.RemovePoints(K, 1)
						N -= 1
						J = (J - 2) Mod N
						If J < 0 Then J = 0 'J = J + N
					Else
						J += 1
					End If
				Else
					J += 1
				End If
			Else
				J += 1
			End If
		Loop

		pPGone = Result
		pPGone.Close()

		pTopo = Result
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return Result
	End Function

	'Sub OrientPolygon(ByVal pPolygon As Geometry.IPointCollection, ByVal pFix As Geometry.IPoint, ByVal CourseDir As Double, ByRef TurnTo As Integer)
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim N As Integer
	'	Dim iMax As Integer
	'	Dim clockWise As Integer

	'	Dim dir12 As Double
	'	Dim dir21 As Double
	'	Dim dir121 As Double
	'	Dim dir211 As Double
	'	Dim AztArray() As Double

	'	Dim PtArray() As Geometry.IPoint

	'	N = pPolygon.PointCount - 1
	'	ReDim PtArray(N)
	'	ReDim AztArray(N)

	'	J = ReturnGeodesicAzimuth(pFix.X, pFix.Y, pPolygon.Point(0).X, pPolygon.Point(0).Y, dir12, dir21)
	'	J = ReturnGeodesicAzimuth(pFix.X, pFix.Y, pPolygon.Point(1).X, pPolygon.Point(1).Y, dir121, dir211)
	'	dir21 = dir121 - dir12

	'	If (dir21 < 0) Then
	'		clockWise = -1
	'	Else
	'		clockWise = 1
	'	End If

	'	If (System.Math.Abs(dir21) > 180.0) Then clockWise = -clockWise

	'	For I = 0 To N - 1
	'		PtArray(I) = pPolygon.Point(I)
	'		J = ReturnGeodesicAzimuth(pFix.X, pFix.Y, PtArray(I).X, PtArray(I).Y, dir12, dir21)
	'		dir21 = (dir12 - CourseDir) * TurnTo
	'		If (dir21 < 0) Then dir21 = dir21 + 360

	'		AztArray(I) = dir21
	'	Next I

	'	clockWise = clockWise * TurnTo
	'	iMax = 0
	'	dir21 = AztArray(0)
	'	For I = 1 To N - 1
	'		If (AztArray(I) > dir21) Then
	'			iMax = I
	'			dir21 = AztArray(I)
	'		End If
	'	Next I

	'	pPolygon.UpdatePoint(0, PtArray(iMax))
	'	For I = 1 To N - 1
	'		iMax = iMax + clockWise
	'		iMax = Modulus(iMax, N)
	'		pPolygon.UpdatePoint(I, PtArray(iMax))
	'	Next I
	'End Sub

	'Function RemoveFars(ByVal pPolygon As Geometry.Polygon, ByVal pPoint As Geometry.Point) As Geometry.Polygon
	'	Dim Geocollect As Geometry.IGeometryCollection
	'	Dim lCollect As Geometry.IGeometryCollection
	'	Dim pProxi As Geometry.IProximityOperator
	'	Dim OutDist As Double
	'	Dim tmpDist As Double
	'	Dim pClone As esriSystem.IClone
	'	Dim I As Integer
	'	Dim N As Integer

	'	pClone = pPolygon
	'	RemoveFars = pClone.Clone
	'	Geocollect = RemoveFars
	'	N = Geocollect.GeometryCount
	'	lCollect = New Geometry.Polygon

	'	If N > 1 Then
	'		pProxi = pPoint
	'		OutDist = 20000000000.0

	'		For I = 0 To N - 1
	'			lCollect.AddGeometry(Geocollect.Geometry(I))

	'			tmpDist = pProxi.ReturnDistance(lCollect)
	'			If OutDist > tmpDist Then
	'				OutDist = tmpDist
	'			End If
	'			lCollect.RemoveGeometries(0, 1)
	'		Next I

	'		I = 0
	'		While I < N
	'			lCollect.AddGeometry(Geocollect.Geometry(I))
	'			tmpDist = pProxi.ReturnDistance(DirectCast(lCollect, IGeometry))
	'			If OutDist < tmpDist Then
	'				Geocollect.RemoveGeometries(I, 1)
	'				N = N - 1
	'			Else
	'				I = I + 1
	'			End If
	'			lCollect.RemoveGeometries(0, 1)
	'		End While
	'	End If
	'End Function

	'Function RemoveSmalls(ByVal pPolygon As Geometry.Polygon) As Geometry.Polygon
	'	Dim I As Integer
	'	Dim N As Integer
	'	Dim OutArea As Double

	'	Dim Geocollect As Geometry.IGeometryCollection
	'	Dim pClone As esriSystem.IClone
	'	Dim Result As Geometry.Polygon
	'	Dim pArea As Geometry.IArea

	'	pClone = pPolygon
	'	Result = pClone.Clone
	'	Geocollect = Result
	'	N = Geocollect.GeometryCount

	'	If N > 1 Then
	'		OutArea = 0.0

	'		For I = 0 To N - 1
	'			pArea = Geocollect.Geometry(I)
	'			If pArea.Area > OutArea Then
	'				OutArea = pArea.Area
	'			End If
	'		Next I

	'		I = 0
	'		While I < N
	'			pArea = Geocollect.Geometry(I)
	'			If pArea.Area < OutArea Then
	'				Geocollect.RemoveGeometries(I, 1)
	'				N -= 1
	'			Else
	'				I += 1
	'			End If
	'		End While
	'	End If

	'	Return Result
	'End Function

	'Function RemoveHoles(ByVal pPolygon As Geometry.Polygon) As Geometry.Polygon
	'	Dim I As Integer
	'	Dim pTopoOper As Geometry.ITopologicalOperator2
	'	Dim Result As Geometry.IGeometryCollection
	'	Dim pInteriorRing As Geometry.IRing
	'	Dim pClone As esriSystem.IClone

	'	pClone = pPolygon
	'	Result = pClone.Clone

	'	I = 0
	'	While I < Result.GeometryCount
	'		pInteriorRing = Result.Geometry(I)
	'		If Not pInteriorRing.IsExterior Then
	'			Result.RemoveGeometries(I, 1)
	'		Else
	'			I += 1
	'		End If
	'	End While

	'	pTopoOper = Result
	'	pTopoOper.IsKnownSimple_2 = False
	'	pTopoOper.Simplify()

	'	Return Result
	'End Function
End Module