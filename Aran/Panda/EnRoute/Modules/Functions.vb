Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS
Imports System.Runtime.InteropServices
Imports ArcGeometry = ESRI.ArcGIS.Geometry
Imports System.IO

<ComVisibleAttribute(False)> Module Functions

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function AppendMenu(hMenu As IntPtr, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function InsertMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	Public Function ToGeo(ByVal pPrjGeom As Geometry.IGeometry) As Geometry.IGeometry
		Dim pClone As esriSystem.IClone
		Dim pTopoOper As Geometry.ITopologicalOperator2

		pClone = pPrjGeom
		ToGeo = pClone.Clone
		ToGeo.SpatialReference = pSpRefPrj
		ToGeo.Project(pSpRefShp)

		If ToGeo.GeometryType = Geometry.esriGeometryType.esriGeometryPolygon Then
			pTopoOper = ToGeo
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If
	End Function

	Public Function ToPrj(ByVal pGeoGeom As Geometry.IGeometry) As Geometry.IGeometry
		Dim pClone As esriSystem.IClone
		Dim pTopoOper As Geometry.ITopologicalOperator2

		pClone = pGeoGeom
		ToPrj = pClone.Clone
		ToPrj.SpatialReference = pSpRefShp
		ToPrj.Project(pSpRefPrj)

		If ToPrj.GeometryType = Geometry.esriGeometryType.esriGeometryPolygon Then
			pTopoOper = ToPrj
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If
	End Function

	Function Azt2Dir(ByVal ptGeo As Geometry.IPoint, ByVal Azt As Double) As Double
		Dim Pt10 As Geometry.IPoint
		Dim Pt11 As Geometry.IPoint
		Dim ResX As Double
		Dim ResY As Double

		PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, ResX, ResY)

		Pt10 = New Geometry.Point
		Pt10.PutCoords(ResX, ResY)

		Pt11 = ToPrj(Pt10)
		Pt10 = ToPrj(ptGeo)

		Return ReturnAngleInDegrees(Pt10, Pt11)
	End Function

	'Function Azt2DirPrj(ByVal ptPrj As Geometry.IPoint, ByVal Azt As Double) As Double
	'	Dim ptGeo As Geometry.IPoint
	'	Dim Pt10 As Geometry.IPoint
	'	Dim Pt11 As Geometry.IPoint
	'	Dim ResX As Double
	'	Dim ResY As Double

	'	ptGeo = ToGeo(ptPrj)
	'	PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, ResX, ResY)

	'	Pt10 = New Geometry.Point
	'	Pt10.PutCoords(ResX, ResY)

	'	Pt11 = ToPrj(Pt10)
	'	Return ReturnAngleInDegrees(ptPrj, Pt11)
	'End Function

	Public Function Dir2Azt(ByVal ptPrj As Geometry.IPoint, ByVal Dir_Renamed As Double) As Double
		Dim resD As Double
		Dim resI As Double
		Dim Pt10 As Geometry.IPoint
		Dim Pt11 As Geometry.IPoint

		Pt10 = ToGeo(PointAlongPlane(ptPrj, Dir_Renamed, 10.0))
		Pt11 = ToGeo(ptPrj)

		ReturnGeodesicAzimuth(Pt11.X, Pt11.Y, Pt10.X, Pt10.Y, resD, resI)
		Return resD
	End Function

	'Public Function Dir2AztGeo(ByVal ptGeo As Geometry.IPoint, ByVal Dir_Renamed As Double) As Double
	'	Dim ptPrj As Geometry.IPoint
	'	Dim resD As Double
	'	Dim resI As Double
	'	Dim Pt10 As Geometry.IPoint

	'	ptPrj = ToPrj(ptGeo)
	'	Pt10 = ToGeo(PointAlongPlane(ptPrj, Dir_Renamed, 10.0))

	'	ReturnGeodesicAzimuth(ptGeo.X, ptGeo.Y, Pt10.X, Pt10.Y, resD, resI)
	'	Return resD
	'End Function

	Public Function UseLineSymbolFromStyleGallery(ByRef Name As String) As Display.ILineSymbol
		Dim pStyleGallery As Display.IStyleGallery
		Dim pEnumStyleGall As Display.IEnumStyleGalleryItem
		Dim pStyleItem As Display.IStyleGalleryItem
		Dim pLineSym As Display.ILineSymbol
		Dim pStylePath As String

		pStyleGallery = GetStyleGallery()
		pStylePath = "ESRI.Style"

		pEnumStyleGall = pStyleGallery.Items("Line Symbols", pStylePath, "")
		pEnumStyleGall.Reset()
		pStyleItem = pEnumStyleGall.Next

		'Loop through and access each marker
		Do While Not pStyleItem Is Nothing
			pLineSym = pStyleItem.Item
			If pStyleItem.Name = Name Then Return pLineSym

			pStyleItem = pEnumStyleGall.Next
		Loop

		Return Nothing
	End Function

	Public Function RetrieveLinkerTimestamp() As DateTime
		Const c_PeHeaderOffset As Integer = 60
		Const c_LinkerTimestampOffset As Integer = 8

		Dim filePath As String = System.Reflection.Assembly.GetCallingAssembly().Location
		Dim b() As Byte
		Dim s As System.IO.Stream = Nothing

		ReDim b(2047)

		Try
			s = New System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)
			s.Read(b, 0, 2048)
		Finally
			If Not (s Is Nothing) Then s.Close()
		End Try

		Dim i As Integer = System.BitConverter.ToInt32(b, c_PeHeaderOffset)
		Dim secondsSince1970 As Integer = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset)

		Dim dt As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)

		dt = dt.AddSeconds(secondsSince1970)
		dt = dt.ToLocalTime()
		Return dt
	End Function

	Public Function GetEnrouteObstacles(ByRef ObstacleList() As ObstacleType, ByRef EnrouteObstaclesList() As ObstacleType, ByRef PrimPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByRef SecPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByRef fMOCValue As Double, ByRef fCurrDir As Double, ByRef pMeasurePoint As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim iMax As Integer

		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim hMax As Double

		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pContourLine As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pPrimProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pSecondProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pContourProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pmca As Double

		N = UBound(ObstacleList)

		If (N < 0) Or (SecPoly.PointCount < 1) Then
			ReDim EnrouteObstaclesList(-1)
			Return -1
		End If

		ReDim EnrouteObstaclesList(N)

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pContourLine = New ESRI.ArcGIS.Geometry.Polyline
		pContourLine.AddPointCollection(SecPoly)

		pPrimProxi = PrimPoly
		pSecondProxi = SecPoly
		pContourProxi = pContourLine

		iMax = -1
		J = -1
		hMax = -10

		pmca = 0.0246868250539957 '150.0 * 0.3048 / 1852.0

		For I = 0 To N
			ptCur = ObstacleList(I).ptPrj

			If pSecondProxi.ReturnDistance(ptCur) = 0.0 Then
				J += 1
				EnrouteObstaclesList(J) = ObstacleList(I)

				If pPrimProxi.ReturnDistance(ptCur) = 0.0 Then
					EnrouteObstaclesList(J).Prima = 1
					EnrouteObstaclesList(J).fTmp = 1.0
				Else
					EnrouteObstaclesList(J).Prima = 0

					pLine.ToPoint = ptCur
					ptTmp = pPrimProxi.ReturnNearestPoint(ptCur, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)
					pLine.FromPoint = ptTmp
					fDist1 = pLine.Length

					ptTmp = pContourProxi.ReturnNearestPoint(ptCur, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)

					pLine.FromPoint = ptTmp
					fDist2 = pLine.Length

					EnrouteObstaclesList(J).fTmp = fDist2 / (fDist1 + fDist2)
				End If

				EnrouteObstaclesList(J).MOC = fMOCValue * EnrouteObstaclesList(J).fTmp
				EnrouteObstaclesList(J).ReqH = EnrouteObstaclesList(J).Height + EnrouteObstaclesList(J).MOC
				If EnrouteObstaclesList(J).ReqH > hMax Then
					hMax = EnrouteObstaclesList(J).ReqH
					iMax = J
				End If

				EnrouteObstaclesList(J).Dist = Point2LineDistancePrj(ptCur, pMeasurePoint, fCurrDir - 90) * SideDef(ptCur, fCurrDir - 90.0, pMeasurePoint)
				EnrouteObstaclesList(J).MCA = EnrouteObstaclesList(J).ReqH

				If EnrouteObstaclesList(J).Dist > 0 Then
					EnrouteObstaclesList(J).MCA = EnrouteObstaclesList(J).ReqH - EnrouteObstaclesList(J).Dist * pmca
				End If
			End If
		Next

		If J >= 0 Then
			ReDim Preserve EnrouteObstaclesList(J)
		Else
			ReDim EnrouteObstaclesList(-1)
		End If

		Return iMax
	End Function

	Public Function IntersectPolyLines(ByVal pPoly1 As ArcGeometry.Polyline, ByVal pPoly2 As ArcGeometry.Polyline, ByRef Result As ArcGeometry.IPointCollection) As Integer
		Dim pTopo As ArcGeometry.ITopologicalOperator2
		pTopo = pPoly1

		Result = pTopo.Intersect(pPoly2, ArcGeometry.esriGeometryDimension.esriGeometry0Dimension)
		Return Result.PointCount
	End Function

	Public Sub CalcTolerance(ByVal ptFix As ArcGeometry.IPoint, ByVal pFIXPoly As ArcGeometry.IPointCollection, ByVal Direction As Double, ByRef NearToler As Double, ByRef FarToler As Double)
		Dim I As Integer
		Dim N As Integer
		Dim D As Double

		FarToler = 0.0
		NearToler = 0.0
		N = pFIXPoly.PointCount - 1

		For I = 0 To N
			D = Point2LineDistancePrj(ptFix, pFIXPoly.Point(I), Direction + 90.0)
			If SideDef(ptFix, Direction + 90.0, pFIXPoly.Point(I)) > 0 Then
				If D > FarToler Then FarToler = D
			Else
				If D > NearToler Then NearToler = D
			End If
		Next I
	End Sub

	Public Sub ChangePoints(ByRef pPolygon As ArcGeometry.IPointCollection, ByVal pPolyline As ArcGeometry.IPolyline, ByVal ptNav As ArcGeometry.Point, ByVal Direction As Double)
		Dim I As Integer
		Dim N As Integer
		Dim Side As Integer
		Dim fAngle As Double
		Dim eAngle As Double
		Dim pTopo As ArcGeometry.ITopologicalOperator2

		N = pPolygon.PointCount - 1

		eAngle = ReturnAngleInDegrees(pPolyline.FromPoint, pPolyline.ToPoint)
		For I = 0 To N
			fAngle = ReturnAngleInDegrees(pPolyline.FromPoint, pPolygon.Point(I))

			If (System.Math.Abs(fAngle - eAngle) < degEps) Then
				Side = SideDef(ptNav, Direction, pPolygon.Point(I))

				If (Side < 0) Then pPolygon.ReplacePoints(I, 1, 1, pPolyline.FromPoint)
				If (Side > 0) Then pPolygon.ReplacePoints(I, 1, 1, pPolyline.ToPoint)
			End If
		Next I

		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
	End Sub

	Public Function CalcNavInterval(ByVal ptStart As ArcGeometry.IPoint, ByVal fDir As Double, ByVal maxLegLenght As Double, ByVal dReserve As Double, ByVal fTurnR As Double, ByVal NAV As NavaidType, ByVal TurnDir As Long) As LowHigh
		Dim D As Double
		Dim denom As Double
		Dim fRefX As Double
		Dim fRefY As Double
		Dim distMn As Double
		Dim distMx As Double
		Dim dirMin As Double
		Dim dirMax As Double
		Dim maxTurn As Double
		Dim alpha_1 As Double
		Dim alpha_2 As Double
		Dim TurnAngle1 As Double
		Dim TurnAngle2 As Double
		Dim CotanMaxTurn As Double
		Dim fToIntersect120 As Double
		Dim fDistToIntersect1 As Double
		Dim fDistToIntersect2 As Double

		Dim bHaveSolution As Boolean
		Dim ptMin As ArcGeometry.IPoint
		Dim ptMax As ArcGeometry.IPoint

		Dim Result As LowHigh

		maxTurn = GlobalVars.DegToRadValue * GlobalVars.MaxInterceptAngle

		Result.Low = -1.0   'разворот на средство не допускается
		Result.High = -1.0  'разворот на средство не допускается
		Result.Tag = 0

		CotanMaxTurn = 1.0 / Math.Tan(maxTurn)

		'dReserve = -ReturnDistanceInMeters(ForFix.ptD0, ForFix.StartFIX.ptPrj)

		PrjToLocal(ptStart, fDir, NAV.pPtPrj, fRefX, fRefY)
		fRefX = fRefX + dReserve

		fToIntersect120 = fRefX - TurnDir * fRefY * CotanMaxTurn
		'dist120 = fTurnR * Math.Tan(GlobalVars.DegToRadValue * 60.0)

		If Math.Abs(fRefY) < distEps Then
			If (fRefX < 0.0) Then Return Result
			Result.Tag = 1

			If TurnDir > 0 Then
				Result.Low = fDir
				Result.High = fDir + Math.Min(2.0 * GlobalVars.RadToDegValue * Math.Atan2(fRefX, fTurnR), GlobalVars.MaxInterceptAngle)
			Else
				Result.Low = fDir - Math.Min(2.0 * GlobalVars.RadToDegValue * Math.Atan2(fRefX, fTurnR), GlobalVars.MaxInterceptAngle)
				Result.High = fDir
			End If

			Return Result
		End If

		bHaveSolution = False

		If TurnDir * fRefY > 0.0 Then       'eyni teref
			denom = TurnDir * fRefY - 2.0 * fTurnR
			D = fRefX * fRefX + TurnDir * fRefY * denom

			If (D < 0.0) Then Return Result

			D = Math.Sqrt(D)

			alpha_1 = 2.0 * Math.Atan2(-fRefX - D, denom)
			alpha_2 = 2.0 * Math.Atan2(-fRefX + D, denom)

			fDistToIntersect1 = fRefX - fRefY * TurnDir / Math.Tan(alpha_1)
			fDistToIntersect2 = fRefX - fRefY * TurnDir / Math.Tan(alpha_2)

			If (fDistToIntersect1 <= 0.0) And (fDistToIntersect2 <= 0.0) Then Return Result

			If fDistToIntersect1 * fDistToIntersect2 < 0.0 Then
				distMn = Math.Max(fDistToIntersect1, fDistToIntersect2)
				distMx = Math.Min(fToIntersect120, maxLegLenght + dReserve)
			Else
				distMn = Math.Min(fDistToIntersect1, fDistToIntersect2)
				distMx = Math.Min(fToIntersect120, Math.Max(fDistToIntersect1, fDistToIntersect2))
			End If

			If distMx > maxLegLenght + dReserve Then distMx = maxLegLenght + dReserve
			If distMn < dReserve Then distMn = dReserve
			bHaveSolution = distMx > distMn
		Else                        ' eks teref
			denom = -fRefY * TurnDir
			D = Math.Sqrt(fRefX * fRefX + denom * (denom + 2.0 * fTurnR))

			alpha_1 = 2.0 * Math.Atan2(-fRefX - D, denom)
			alpha_2 = 2.0 * Math.Atan2(-fRefX + D, denom)

			fDistToIntersect1 = fRefX + fRefY * TurnDir / Math.Tan(alpha_1)
			fDistToIntersect2 = fRefX + fRefY * TurnDir / Math.Tan(alpha_2)

			If (fDistToIntersect1 <= 0.0) And (fDistToIntersect2 <= 0.0) Then Return Result

			If (fDistToIntersect1 * fDistToIntersect2 < 0.0) Then
				distMn = Math.Max(Math.Max(fDistToIntersect1, fDistToIntersect2), fToIntersect120)
			Else
				distMn = Math.Max(Math.Min(fDistToIntersect1, fDistToIntersect2), fToIntersect120)
			End If

			If distMn < dReserve Then distMn = dReserve
			distMx = maxLegLenght + dReserve

			bHaveSolution = distMx > distMn
		End If

		If bHaveSolution Then
			ptMin = LocalToPrj(ptStart, fDir, distMn - dReserve)
			ptMax = LocalToPrj(ptStart, fDir, distMx - dReserve)

			dirMin = ReturnAngleInDegrees(ptMin, NAV.pPtPrj)
			TurnAngle1 = TurnDir * (dirMin - fDir)
			If Modulus(TurnAngle1) > 180.0 Then dirMin = dirMin + 180.0

			dirMax = ReturnAngleInDegrees(ptMax, NAV.pPtPrj)
			TurnAngle2 = TurnDir * (dirMax - fDir)
			If Modulus(TurnAngle2) > 180.0 Then dirMax = dirMax + 180.0

			If AnglesSideDef(dirMin, dirMax) < 0 Then
				Result.Low = dirMin
				Result.High = dirMax
			Else
				Result.Low = dirMax
				Result.High = dirMin
			End If

			Result.Tag = 1
		End If

		CalcNavInterval = Result
	End Function


	'Public Function LowHighInsert(ByRef A() As LowHigh, ByRef B As LowHigh) As LowHigh()
	'	Dim I As Integer
	'	Dim J As Integer

	'	Dim N As Integer
	'	Dim M As Integer
	'	Dim res() As LowHigh

	'	N = UBound(A)
	'	I = 0

	'	While (I <= N) And (A(I).High < B.Low)
	'		I = I + 1
	'	End While

	'	If I > N Then
	'		If N < 0 Then
	'			ReDim res(I)
	'		Else
	'			ReDim Preserve res(I)
	'		End If

	'		res(I) = B
	'	ElseIf A(I).Low > B.High Then 
	'		ReDim Preserve res(N + 1)
	'		For J = N + 1 To I + 1 Step -1
	'			res(J) = res(J - 1)
	'		Next J
	'		res(I) = B
	'	ElseIf A(I).Low > B.Low Then 

	'	ElseIf A(I).High < B.High Then 
	'		res(I).High = B.High
	'		If A(I).Low > B.Low Then
	'			res(I).Low = B.Low
	'		End If

	'		If I < N Then
	'			J = I + 1
	'			While (J <= N) And (A(J).High < B.High)
	'				J = J + 1
	'			End While

	'			If J <= N Then
	'				If A(J).Low < B.High Then
	'					M = J - I
	'					res(I).High = A(J).High
	'				Else
	'					M = J - I - 1
	'				End If
	'			Else
	'				M = J - I - 1
	'			End If

	'			If M > 0 Then
	'				For J = I + 1 To N - M
	'					res(J) = res(J + M)
	'				Next J
	'				ReDim Preserve res(N - M)
	'			End If
	'		End If
	'	Else

	'	End If

	'	Return res
	'   End Function

	'Public Function LowHighDifference(ByRef A As LowHigh, ByRef B As LowHigh) As LowHigh()
	'	Dim res() As LowHigh

	'	ReDim res(0)

	'	If (B.Low = B.High) Or (B.High < A.Low) Or (A.High < B.Low) Then
	'		res(0) = A
	'	ElseIf (A.Low < B.Low) And (A.High > B.High) Then 
	'		ReDim res(1)
	'		res(0).Low = A.Low
	'		res(0).High = B.Low
	'		res(1).Low = B.High
	'		res(1).High = A.High
	'	ElseIf A.High > B.High Then 
	'		res(0).Low = B.High
	'		res(0).High = A.High
	'	ElseIf (A.Low < B.Low) Then 
	'		res(0).Low = A.Low
	'		res(0).High = B.Low
	'	Else
	'		ReDim res(-1)
	'       End If

	'	Return res
	'   End Function

	'Sub LowHighSort(ByRef A() As LowHigh, Optional ByVal Descent As Boolean = False)
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim N As Integer
	'	Dim Tmp As LowHigh

	'	N = UBound(A)

	'	If Descent Then
	'		For I = 0 To N - 1
	'			For J = I + 1 To N
	'				If A(I).High > A(J).High Then
	'					Tmp = A(I)
	'					A(I) = A(J)
	'					A(J) = Tmp
	'				End If
	'			Next J
	'		Next I
	'	Else
	'		For I = 0 To N - 1
	'			For J = I + 1 To N
	'				If A(I).Low > A(J).Low Then
	'					Tmp = A(I)
	'					A(I) = A(J)
	'					A(J) = Tmp
	'				End If
	'			Next J
	'		Next I
	'	End If

	'End Sub

	'Sub SortIntervals(ByRef Intervals() As Interval, Optional ByVal Descent As Boolean = False)
	'	Dim I As Long
	'	Dim J As Long
	'	Dim N As Long
	'	Dim Tmp As Interval

	'	N = UBound(Intervals)

	'	For I = 0 To N - 1
	'		For J = I + 1 To N
	'			If Descent Then
	'				If Intervals(I).Right > Intervals(J).Right Then
	'					Tmp = Intervals(I)
	'					Intervals(I) = Intervals(J)
	'					Intervals(J) = Tmp
	'				End If
	'			Else
	'				If Intervals(I).Left > Intervals(J).Left Then
	'					Tmp = Intervals(I)
	'					Intervals(I) = Intervals(J)
	'					Intervals(J) = Tmp
	'				End If
	'			End If
	'		Next J
	'	Next I
	'End Sub

	'Public Function LowHighIntersection(ByRef A As LowHigh, ByRef B As LowHigh, ByRef C As LowHigh) As Integer
	'	If (A.Low > B.High) Or (A.High < B.Low) Then Return 0

	'	If A.High < B.High Then
	'		C.High = A.High
	'	Else
	'		C.High = B.High
	'	End If

	'	If A.Low > B.Low Then
	'		C.Low = A.Low
	'	Else
	'		C.Low = B.Low
	'	End If

	'	Return 1
	'End Function

	Public Function IntervalsDifference(ByRef A As Interval, ByRef B As Interval) As Interval()
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
			ReDim Res(-1)
		End If

		Return Res
	End Function

	Public Function ShowSaveDialog(ByRef FileName As String, ByRef FileTitle As String) As Boolean
		Dim ProjectDir As String
		Dim ProjectPath As String
		Dim SaveDlg As New System.Windows.Forms.SaveFileDialog()

		ProjectPath = GetMapFileName()
		ProjectDir = Path.GetDirectoryName(ProjectPath)
		If (FileTitle = "") Then FileTitle = Path.GetFileNameWithoutExtension(ProjectPath)

		SaveDlg.DefaultExt = ""
		SaveDlg.InitialDirectory = ProjectDir 'Left(ProjectPath, pos)
		SaveDlg.Title = My.Resources.str0508

		SaveDlg.FileName = Path.Combine(ProjectDir, FileTitle + ".htm")   '	Left(ProjectPath, pos2 - 1) + ".htm"
		SaveDlg.Filter = "Text files (*.txt)|*.txt|PANDA Report Files (*.htm)|*.htm|PANDA Report Files (*.html)|*.html|All files (*.*)|*.*"

		FileTitle = ""
		FileName = ""

		If SaveDlg.ShowDialog() <> DialogResult.OK Then
			SaveDlg.Dispose()
			Return False
		End If

		FileTitle = Path.GetFileNameWithoutExtension(SaveDlg.FileName)
		FileName = Path.GetDirectoryName(Path.GetFullPath(SaveDlg.FileName)) + "\" + FileTitle

		SaveDlg.Dispose()

		Return True
	End Function

	Public Sub ShowErrorMessage(message As String, Optional ByVal isError As Boolean = True)
		MessageBox.Show(message, ModuleName, MessageBoxButtons.OK, IIf(isError, MessageBoxIcon.Error, MessageBoxIcon.Warning))
	End Sub

	'Public Function AnaliseObstacles(ByVal InObstList() As ObstacleType, ByRef OutObstList() As ObstacleType
	''                ptLHprj As IPoint, ArDir As Double, Planes() As D3DPolygone) As Long
	'Dim I As Long
	'Dim J As Long
	'Dim K As Long
	'Dim N As Long
	'Dim M As Long
	'Dim fDist As Double
	'Dim X As Double
	'Dim Y As Double
	'Dim z As Double
	'
	'Dim pRelationFull As IRelationalOperator
	'Dim pRelation As IRelationalOperator
	'
	'N = UBound(InObstList)
	'AnaliseObstacles = 0
	'
	'If N < 0 Then
	'    ReDim OutObstList(-1)
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
	'                z = Planes(J).Plane.A * X + _
	''                    Planes(J).Plane.B * Y + _
	''                    Planes(J).Plane.D
	'
	'                OutObstList(K).Dist = X
	'                OutObstList(K).DistStar = Y
	'                OutObstList(K).fTmp = z
	'                OutObstList(K).Prima = J
	'                OutObstList(K).Height = InObstList(I).ptPrj.z - ptLHprj.z
	'                OutObstList(K).hPent = OutObstList(K).Height - z
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
	'    ReDim OutObstList(-1)
	'End If
	'End Function

	Public Sub GetIntData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As Integer, ByVal size As Integer)
		Dim I As Integer
		Dim E As Double

		E = 1
		Vara = 0

		For I = 0 To size - 1
			Vara = Data(Index + I) * E + Vara
			E = E * 256
		Next I

		Index += size
	End Sub

	Public Sub GetStrData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As String, ByVal size As Integer)
		Dim I As Integer
		Vara = ""

		For I = 0 To size - 1
			Vara = Vara + Chr(Data(Index + I))
		Next I

		Index += size
	End Sub

	Public Sub GetData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As Object, ByVal size As Integer)
		Dim I As Integer
		ReDim Vara(size)
		For I = 0 To size - 1
			Vara(I) = Data(Index + I)
		Next I
		Index += size
	End Sub

	Public Sub GetDoubleData(ByRef data() As Byte, ByRef index As UInteger, ByRef Vara As Double, ByVal size As Integer)
		Dim I As Integer
		Dim Sign As Integer

		Dim mantissa As Double
		Dim exponent As Integer

		mantissa = 0
		exponent = 0

		For I = 0 To size - 3
			mantissa = data(index + I) + 0.00390625 * mantissa
		Next I

		mantissa = 0.0625 * (((data(index + size - 2) And 15) + 16) + 0.00390625 * mantissa)

		exponent = 0.0625 * (data(index + size - 2) And 240)
		exponent = data(index + size - 1) * 16.0 + exponent

		If (mantissa = 1) And (exponent = 0) Then
			Vara = 0.0
			index = index + size
			Return
		End If

		Sign = exponent And 2048
		exponent = (exponent And 2047) - 1023
		If exponent > 0 Then
			For I = 1 To exponent
				mantissa = mantissa * 2.0
			Next I
		ElseIf exponent < 0 Then
			For I = -1 To exponent Step -1
				mantissa = 0.5 * mantissa
			Next I
		End If

		If Sign <> 0 Then mantissa = -mantissa
		Vara = mantissa
		index = index + size
	End Sub

End Module