Option Strict Off
Option Explicit On
Option Compare Text
Imports System.Runtime.InteropServices

Module Utils

	Public Declare Sub ShowPandaBox Lib "MathFunctions.dll" Alias "_ShowPandaBox@4" (ByVal hWnd As Integer)
	Public Declare Sub HidePandaBox Lib "MathFunctions.dll" Alias "_HidePandaBox@0" ()
	Public Declare Sub InitEllipsoid Lib "MathFunctions.dll" Alias "_InitEllipsoid@16" (ByVal EquatorialRadius As Double, ByVal InverseFlattening As Double)
	Public Declare Sub InitProjection Lib "MathFunctions.dll" Alias "_InitProjection@40" (ByVal Lm0 As Double, ByVal Lp0 As Double, ByVal Sc As Double, ByVal Efalse As Double, ByVal Nfalse As Double)
	Public Declare Function PointAlongGeodesic Lib "MathFunctions.dll" Alias "_PointAlongGeodesic@40" (ByVal X As Double, ByVal Y As Double, ByVal Dist As Double, ByVal Azimuth As Double, ByRef resx As Double, ByRef resy As Double) As Integer
	Public Declare Function ReturnGeodesicAzimuth Lib "MathFunctions.dll" Alias "_ReturnGeodesicAzimuth@40" (ByVal x0 As Double, ByVal y0 As Double, ByVal X1 As Double, ByVal Y1 As Double, ByRef DirectAzimuth As Double, ByRef InverseAzimuth As Double) As Integer
	Public Declare Function Modulus Lib "MathFunctions.dll" Alias "_Modulus@16" (ByVal X As Double, Optional ByVal Y As Double = 360.0) As Double

	Public Declare Function SetThreadLocale Lib "kernel32.dll" (ByVal dwLangID As Integer) As Boolean
	Public Declare Function HtmlHelp Lib "hhctrl.ocx" Alias "HtmlHelpA" (ByVal hwndCaller As Integer, ByVal pszFile As String, ByVal uCommand As Integer, ByVal dwData As Integer) As Integer

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function AppendMenu(hMenu As IntPtr, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function InsertMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	Public Sub ShowErrorMessage(message As String, Optional ByVal isError As Boolean = True)
		MessageBox.Show(message, ModuleName, MessageBoxButtons.OK, IIf(isError, MessageBoxIcon.Error, MessageBoxIcon.Warning))
	End Sub

	Public Function ConvertDistance(ByVal value As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
		Select Case RoundMode
			Case 0
				Return value * DistanceConverter(DistanceUnit).Multiplier
			Case 1
				Return System.Math.Round(value * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding - 0.4999999) * DistanceConverter(DistanceUnit).Rounding
			Case 2
				Return System.Math.Round(value * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding) * DistanceConverter(DistanceUnit).Rounding
			Case 3
				Return System.Math.Round(value * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding + 0.4999999) * DistanceConverter(DistanceUnit).Rounding
		End Select
	End Function

	Public Function ConvertHeight(ByVal value As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL_CEIL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return value * HeightConverter(HeightUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding - 0.4999999) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.NERAEST
				Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding + 0.4999999) * HeightConverter(HeightUnit).Rounding

			Case eRoundMode.SPECIAL_FLOOR
				If HeightUnit = 0 Then
					Return System.Math.Floor(value * HeightConverter(HeightUnit).Multiplier / 50.0) * 50.0
				ElseIf HeightUnit = 1 Then
					Return System.Math.Floor(value * HeightConverter(HeightUnit).Multiplier / 100.0) * 100.0
				Else
					Return System.Math.Floor(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
				End If

			Case eRoundMode.SPECIAL_NERAEST
				If HeightUnit = 0 Then
					Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / 50.0) * 50.0
				ElseIf HeightUnit = 1 Then
					Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / 100.0) * 100.0
				Else
					Return System.Math.Round(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
				End If

			Case eRoundMode.SPECIAL_CEIL
				If HeightUnit = 0 Then
					Return System.Math.Ceiling(value * HeightConverter(HeightUnit).Multiplier / 50.0) * 50.0
				ElseIf HeightUnit = 1 Then
					Return System.Math.Ceiling(value * HeightConverter(HeightUnit).Multiplier / 100.0) * 100.0
				Else
					Return System.Math.Ceiling(value * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
				End If
		End Select
	End Function

	Public Function ConvertSpeed(ByVal value As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
		Select Case RoundMode
			Case 0
				Return value * SpeedConverter(SpeedUnit).Multiplier
			Case 1
				Return System.Math.Round(value * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding - 0.4999999) * SpeedConverter(SpeedUnit).Rounding
			Case 2
				Return System.Math.Round(value * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding) * SpeedConverter(SpeedUnit).Rounding
			Case 3
				Return System.Math.Round(value * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding + 0.4999999) * SpeedConverter(SpeedUnit).Rounding
		End Select
	End Function

	Public Function ConvertDSpeed(ByVal value As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = 0
		Select Case RoundMode
			Case 0
				Return value * DSpeedConverter(HeightUnit).Multiplier
			Case 1
				Return System.Math.Round(value * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding - 0.4999999) * DSpeedConverter(HeightUnit).Rounding
			Case 2
				Return System.Math.Round(value * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding) * DSpeedConverter(HeightUnit).Rounding
			Case 3
				Return System.Math.Round(value * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding + 0.4999999) * DSpeedConverter(HeightUnit).Rounding
		End Select
	End Function

	Public Function DeConvertDistance(ByVal value As Double) As Double
		Return value / DistanceConverter(DistanceUnit).Multiplier
	End Function

	Public Function DeConvertHeight(ByVal value As Double) As Double
		Return value / HeightConverter(HeightUnit).Multiplier
	End Function

	Public Function DeConvertSpeed(ByVal value As Double) As Double
		Return value / SpeedConverter(SpeedUnit).Multiplier
	End Function

	Public Function DeConvertDSpeed(ByVal value As Double) As Double
		Return value / DSpeedConverter(HeightUnit).Multiplier
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

	Function DegToRad(ByRef X As Double) As Double
		Return X * DegToRadValue
	End Function

	Function RadToDeg(ByRef X As Double) As Double
		Return X * RadToDegValue
	End Function

	Function SubtractAngles(ByVal X As Double, ByVal Y As Double) As Double
		SubtractAngles = Modulus(X - Y, 360.0)
		If SubtractAngles > 180.0 Then SubtractAngles = 360.0 - SubtractAngles
	End Function

	Public Function ToGeo(ByVal pPrjGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = pPrjGeom
		ToGeo = pClone.Clone
		ToGeo.SpatialReference = pSpRefPrj
		ToGeo.Project(pSpRefShp)

		If ToGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
			pTopoOper = ToGeo
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If
	End Function

	Public Function ToPrj(ByVal pGeoGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = pGeoGeom
		ToPrj = pClone.Clone
		ToPrj.SpatialReference = pSpRefShp
		ToPrj.Project(pSpRefPrj)

		If ToPrj.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
			pTopoOper = ToPrj
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If
	End Function

	Function Azt2Dir(ByVal ptGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal Azt As Double) As Double
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint
		Dim ResX As Double
		Dim ResY As Double

		PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, ResX, ResY)

		Pt10 = New ESRI.ArcGIS.Geometry.Point
		Pt10.PutCoords(ResX, ResY)

		Pt11 = ToPrj(Pt10)
		Pt10 = ToPrj(ptGeo)

		Return ReturnAngleInDegrees(Pt10, Pt11)
	End Function

	Public Function Dir2Azt(ByVal ptPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal Dir_Renamed As Double) As Double
		Dim resD As Double
		Dim resI As Double
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint

		Pt10 = ToGeo(PointAlongPlane(ptPrj, Dir_Renamed, 10.0))
		Pt11 = ToGeo(ptPrj)

		ReturnGeodesicAzimuth(Pt11.X, Pt11.Y, Pt10.X, Pt10.Y, resD, resI)
		Return resD
	End Function

	Function PointAlongPlane(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef dirAngle As Double, ByRef Dist As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim dirInRad As Double
		dirInRad = DegToRad(dirAngle)
		PointAlongPlane = New ESRI.ArcGIS.Geometry.Point
		PointAlongPlane.PutCoords(ptFrom.X + Dist * System.Math.Cos(dirInRad), ptFrom.Y + Dist * System.Math.Sin(dirInRad))
	End Function

	Public Function ReturnAngleInDegrees(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return Modulus(RadToDegValue * Math.Atan2(fdY, fdX), 360.0)
	End Function

	Public Function ReturnDistanceInMeters(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return System.Math.Sqrt(fdX * fdX + fdY * fdY)
	End Function

	Public Function SideDef(ByRef PtInLine As ESRI.ArcGIS.Geometry.IPoint, ByRef LineAngle As Double, ByRef TestPt As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim Angle12 As Double
		Dim dAngle As Double
		Dim fDist As Double

		fDist = ReturnDistanceInMeters(PtInLine, TestPt)
		If fDist < distEps Then Return 0

		Angle12 = ReturnAngleInDegrees(PtInLine, TestPt)
		dAngle = Modulus(LineAngle - Angle12, 360.0)

		If (dAngle = 0.0) Or (dAngle = 180.0) Then Return 0
		If (dAngle < 180.0) Then Return 1
		Return -1
	End Function

	Public Function CartesianToPolar(ByVal ptCenter As ESRI.ArcGIS.Geometry.IPoint, ptCartesian As ESRI.ArcGIS.Geometry.IPoint) As PolarPoint
		Dim dX As Double
		Dim dY As Double
		Dim result As PolarPoint

		dX = ptCartesian.X - ptCenter.X
		dY = ptCartesian.Y - ptCenter.Y

		'result = New ESRI.ArcGIS.Geometry.Point
		result.R = Math.Sqrt(dX * dX + dY * dY)
		result.a = RadToDegValue * Math.Atan2(dY, dX)
		If result.a < 0 Then result.a += 360.0
		Return result
	End Function

	Public Function CartesianToPolar(ptCartesian As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.IPoint
		Dim result As ESRI.ArcGIS.Geometry.IPoint

		result = New ESRI.ArcGIS.Geometry.Point
		result.X = Math.Sqrt(ptCartesian.X * ptCartesian.X + ptCartesian.Y * ptCartesian.Y)
		result.Y = RadToDegValue * Math.Atan2(ptCartesian.Y, ptCartesian.X)
		Return result
	End Function

	Public Function PolarToCartesian(ptPolar As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.IPoint
		Dim Theta As Double
		Dim result As ESRI.ArcGIS.Geometry.IPoint

		Theta = DegToRadValue * ptPolar.Y
		result = New ESRI.ArcGIS.Geometry.Point

		result.X = ptPolar.X * Math.Cos(Theta)
		result.Y = ptPolar.X * Math.Sin(Theta)
		Return result
	End Function

	Public Sub PartObstacle(ByVal ptCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal Radius As Double, ByVal pCircleProxi As ESRI.ArcGIS.Geometry.IProximityOperator, ByRef Obstacle As ObstacleType)
		Dim I As Integer
		Dim N As Integer
		Dim ptCurr As PolarPoint
		Dim ptMin As PolarPoint
		Dim ptMax As PolarPoint
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		pProxi = Obstacle.pGeomPrj
		Obstacle.MinDist = pProxi.ReturnDistance(ptCenter)

		If Obstacle.pGeomPrj.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
			ptCurr = CartesianToPolar(ptCenter, Obstacle.pGeomPrj)
			Obstacle.FromPoint = ptCurr
			Obstacle.ToPoint = ptCurr
			Obstacle.MaxDist = Obstacle.MinDist
		Else
			Obstacle.MinDist = Radius - pCircleProxi.ReturnDistance(Obstacle.pGeomPrj)

			pPointCollection = Obstacle.pGeomPrj
			N = pPointCollection.PointCount

			ptCurr = CartesianToPolar(ptCenter, pPointCollection.Point(0))
			ptMin = ptCurr
			ptMax = ptCurr

			For I = 1 To N - 1
				ptCurr = CartesianToPolar(ptCenter, pPointCollection.Point(I))

				If AnglesSideDef(ptCurr.a, ptMin.a) < 0 Then
					ptMin = ptCurr
				ElseIf AnglesSideDef(ptCurr.a, ptMax.a) = 1 Then
					ptMax = ptCurr
				End If
			Next
			Obstacle.FromPoint = ptMin
			Obstacle.ToPoint = ptMax
		End If
	End Sub

	'Public Sub ClassifyObstacles(ByRef Obstacles() As ObstacleType, ByVal FullPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal PrimaPoly As ESRI.ArcGIS.Geometry.IPolygon)
	'	Dim i As Integer
	'	Dim n As Integer
	'	Dim pPrimaRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

	'pPrimaRelation = PrimaPoly

	'n = Obstacles.Length

	'For i = 0 To n - 1
	'Obstacles(i).iFlag = 0
	'	If Not pPrimaRelation.Disjoint(Obstacles(i).pGeomPrj) Then
	'		Obstacles(i).iFlag = 1
	'	End If
	'Next
	'End Sub

	Public Function AngleInSector(ByVal Angle As Double, ByVal X As Double, ByVal Y As Double) As Boolean
		X = Modulus(X)
		Y = Modulus(Y)
		Angle = Modulus(Angle)

		If X > Y Then
			If (Angle >= X) Or (Angle <= Y) Then Return True
		Else
			If (Angle >= X) And (Angle <= Y) Then Return True
		End If

		Return False
	End Function

	Public Function AngleInSectorExc(ByVal testAngle As Integer, ByVal FromAngle As Integer, ByVal ToAngle As Integer) As Boolean
		While FromAngle < 0
			FromAngle += 360
		End While

		While FromAngle >= 360
			FromAngle -= 360
		End While

		While ToAngle < 0
			ToAngle += 360
		End While

		While ToAngle >= 360
			ToAngle -= 360
		End While

		While testAngle < 0
			testAngle += 360
		End While

		While testAngle >= 360
			testAngle -= 360
		End While

		If FromAngle > ToAngle Then
			If (testAngle > FromAngle) Or (testAngle < ToAngle) Then Return True
		Else
			If (testAngle > FromAngle) And (testAngle < ToAngle) Then Return True
		End If

		Return False
	End Function

	Public Function SideFrom2Angle(ByVal Angle0 As Double, ByVal Angle1 As Double) As Integer
		Dim dAngle As Double

		dAngle = SubtractAngles(Angle0, Angle1)
		If (180.0 - dAngle < degEps) Or (dAngle < degEps) Then Return 0

		dAngle = Modulus(Angle1 - Angle0, 360.0)
		If (dAngle < 180.0) Then Return 1

		Return -1
	End Function

	Function AnglesSideDef(ByVal X As Double, ByVal Y As Double) As Integer
		Dim Z As Double
		Z = Modulus(X - Y, 360.0)

		If Z = 0.0 Then
			AnglesSideDef = 0
		ElseIf Z > 180.0 Then
			AnglesSideDef = -1
		ElseIf Z < 180.0 Then
			AnglesSideDef = 1
		Else
			AnglesSideDef = 2
		End If
	End Function

	Public Function CreatePrjCircle(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, Optional ByVal N As Integer = 360) As ESRI.ArcGIS.Geometry.Polygon
		Dim I As Integer
		Dim iInRad As Double
		Dim dA As Double

		Dim Pt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Pt = New ESRI.ArcGIS.Geometry.Point
		pPolygon = New ESRI.ArcGIS.Geometry.Polygon
		dA = 360.0 * DegToRadValue / N

		N = N - 1
		For I = 0 To N
			iInRad = I * dA
			Pt.X = pPtCenter.X + R * System.Math.Cos(iInRad)
			Pt.Y = pPtCenter.Y + R * System.Math.Sin(iInRad)
			pPolygon.AddPoint(Pt)
		Next I

		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return pPolygon
	End Function

	Public Function CreateCircleBorder(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, Optional ByVal N As Integer = 360) As ESRI.ArcGIS.Geometry.Polyline
		Dim I As Integer
		Dim iInRad As Double
		Dim dA As Double

		Dim Pt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolyline As ESRI.ArcGIS.Geometry.IPointCollection

		Pt = New ESRI.ArcGIS.Geometry.Point
		pPolyline = New ESRI.ArcGIS.Geometry.Polyline
		dA = 360.0 * DegToRadValue / N

		'N = N - 1
		For I = 0 To N
			iInRad = I * dA
			Pt.X = pPtCenter.X + R * System.Math.Cos(iInRad)
			Pt.Y = pPtCenter.Y + R * System.Math.Sin(iInRad)
			pPolyline.AddPoint(Pt)
		Next I

		Return pPolyline
	End Function

	Public Function CreateCirclePoly(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal InnerDist As Double, ByVal OuterDist As Double) As ESRI.ArcGIS.Geometry.Polygon
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInnerPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pPolygon = CreatePrjCircle(pPtCenter, OuterDist)

		If InnerDist > distEps Then
			pInnerPoints = CreatePrjCircle(pPtCenter, InnerDist)
			pTopoOper = pPolygon
			pPolygon = pTopoOper.Difference(pInnerPoints)

			pTopoOper = pPolygon
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If

		Return pPolygon
	End Function

	Public Function CreateArcPrj(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.Polygon
		Dim n As Integer
		Dim i As Integer

		Dim R As Double
		Dim dX As Double
		Dim dY As Double
		Dim daz As Double
		Dim AztTo As Double
		Dim IinRad As Double
		Dim AngStep As Double
		Dim AztFrom As Double

		Dim Result As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint

		Result = New ESRI.ArcGIS.Geometry.Polygon

		dX = ptFrom.X - ptCnt.X
		dY = ptFrom.Y - ptCnt.Y
		R = System.Math.Sqrt(dX * dX + dY * dY)

		AztFrom = Modulus(RadToDegValue * Math.Atan2(dY, dX))
		AztTo = Modulus(RadToDegValue * Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X))
		daz = Modulus((AztTo - AztFrom) * ClWise)

		AngStep = 1.0 ' /// 
		n = Math.Round(daz / AngStep)

		If (n < 1) Then
			n = 1
		ElseIf (n < 5) Then
			n = 5
		ElseIf (n < 10) Then
			n = 10
		End If

		AngStep = daz / n

		Result.AddPoint(ptFrom)
		ptCur = New ESRI.ArcGIS.Geometry.Point
		For i = 1 To n - 1
			IinRad = DegToRadValue * (AztFrom + i * AngStep * ClWise)
			ptCur.X = ptCnt.X + R * System.Math.Cos(IinRad)
			ptCur.Y = ptCnt.Y + R * System.Math.Sin(IinRad)
			Result.AddPoint(ptCur)
		Next i
		Result.AddPoint(ptTo)

		Return Result
	End Function

	Public Function CreateSectorPoly(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal InnerDist As Double, ByVal OuterDist As Double, ByVal FromAngle As Double, ByVal ToAngle As Double) As ESRI.ArcGIS.Geometry.Polygon
		Dim pPt0 As ESRI.ArcGIS.Geometry.Point
		Dim pPt1 As ESRI.ArcGIS.Geometry.Point
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInnerPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pPt0 = PointAlongPlane(pPtCenter, FromAngle, OuterDist)
		pPt1 = PointAlongPlane(pPtCenter, ToAngle, OuterDist)
		pPolygon = CreateArcPrj(pPtCenter, pPt0, pPt1, -1)

		If InnerDist < distEps Then
			pPolygon.AddPoint(pPtCenter)
			pPolygon.AddPoint(pPtCenter, 0)
		Else
			pPt0 = PointAlongPlane(pPtCenter, FromAngle, InnerDist)
			pPt1 = PointAlongPlane(pPtCenter, ToAngle, InnerDist)
			pInnerPoints = CreateArcPrj(pPtCenter, pPt1, pPt0, 1)
			pPolygon.AddPointCollection(pInnerPoints)
		End If

		pTopoOper = pPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		Return pPolygon
	End Function

	Public Function MSAvalue(ByRef ReqH As Double) As Double
		Dim MSARoundThreshold As Double
		Dim InvMSARoundThreshold As Double

		If HeightUnit = 0 Then
			MSARoundThreshold = 50.0
			InvMSARoundThreshold = 0.02
		Else
			MSARoundThreshold = 100.0
			InvMSARoundThreshold = 0.01
		End If

		Return System.Math.Round(ConvertHeight(ReqH, 0) * InvMSARoundThreshold + 0.4999999) * MSARoundThreshold
	End Function

	Public Function DrawObstacle(ByVal Obstacle As ObstacleType, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
		'Dim pElementColl As ESRI.ArcGIS.Carto.IElementCollection

		Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement
		Dim pPtGroupElement As ESRI.ArcGIS.Carto.IGroupElement
		Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry

		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurve As ESRI.ArcGIS.Geometry.ICurve
		Dim pArea As ESRI.ArcGIS.Geometry.IArea
		Dim I As Integer

		pGeometry = Obstacle.pGeomPrj

		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement

		If pGeometry.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
			pPtTmp = pGeometry
		ElseIf pGeometry.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
			pCurve = pGeometry
			pPtTmp = New ESRI.ArcGIS.Geometry.Point()
			pCurve.QueryPoint(ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension, 0.5, True, pPtTmp)
			pGroupElement.AddElement(DrawPolyLine(pGeometry, Color, 2, False))
		Else 'If pGeometry.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
			pArea = pGeometry
			pPtTmp = New ESRI.ArcGIS.Geometry.Point()
			pArea.QueryLabelPoint(pPtTmp)
			pGroupElement.AddElement(DrawPolygon(pGeometry, Color, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross, False))
		End If

		pPtGroupElement = DrawPointWithText(pPtTmp, Obstacle.UnicalName, Color, False)
		For I = 0 To pPtGroupElement.ElementCount - 1
			pGroupElement.AddElement(pPtGroupElement.Element(I))
		Next

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pGroupElement, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pGroupElement
	End Function

	Public Function DrawPoint(ByVal pPoint As ESRI.ArcGIS.Geometry.Point, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
		Dim pMarkerShpElement As ESRI.ArcGIS.Carto.IMarkerElement
		Dim pElementofpPoint As ESRI.ArcGIS.Carto.IElement
		Dim pMarkerSym As ESRI.ArcGIS.Display.ISimpleMarkerSymbol
		Dim pRGB As ESRI.ArcGIS.Display.IRgbColor

		pMarkerShpElement = New ESRI.ArcGIS.Carto.MarkerElement

		pElementofpPoint = pMarkerShpElement
		pElementofpPoint.Geometry = pPoint

		pRGB = New ESRI.ArcGIS.Display.RgbColor
		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pMarkerSym = New ESRI.ArcGIS.Display.SimpleMarkerSymbol
		pMarkerSym.Color = pRGB
		pMarkerSym.Size = 8
		pMarkerShpElement.Symbol = pMarkerSym

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementofpPoint, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementofpPoint
	End Function

	Public Function DrawPointWithText(ByVal pPoint As ESRI.ArcGIS.Geometry.Point, ByVal sText As String, Optional ByVal Color As Integer = -1, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
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

		pTextElement.Text = sText
		pTextElement.ScaleText = False
		pTextElement.Symbol = pTextSymbol

		pElementOfText.Geometry = pPoint

		pMarkerShpElement = New ESRI.ArcGIS.Carto.MarkerElement

		pElementofpPoint = pMarkerShpElement
		pElementofpPoint.Geometry = pPoint

		pRGB = New ESRI.ArcGIS.Display.RgbColor
		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pMarkerSym = New ESRI.ArcGIS.Display.SimpleMarkerSymbol
		pMarkerSym.Color = pRGB
		pMarkerSym.Size = 6
		pMarkerShpElement.Symbol = pMarkerSym

		pGroupElement = New ESRI.ArcGIS.Carto.GroupElement
		pGroupElement.AddElement(pElementofpPoint)
		pGroupElement.AddElement(pTextElement)

		pCommonElement = pGroupElement

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pCommonElement, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
		Return pCommonElement
	End Function

	Public Sub DrawLine(ByRef FromPoint As ESRI.ArcGIS.Geometry.IPoint, ByRef ToPoint As ESRI.ArcGIS.Geometry.IPoint, ByRef pActiveView As ESRI.ArcGIS.Carto.IActiveView)
		Dim myPolyLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim myLineSymbol As ESRI.ArcGIS.Display.ISimpleLineSymbol
		Dim pSymbol As ESRI.ArcGIS.Display.ISymbol
		Dim pRGBColor As ESRI.ArcGIS.Display.IRgbColor

		myPolyLine = New ESRI.ArcGIS.Geometry.Polyline

		myLineSymbol = New ESRI.ArcGIS.Display.SimpleLineSymbol
		myLineSymbol.Width = 1

		pRGBColor = New ESRI.ArcGIS.Display.RgbColor

		pRGBColor.Red = 223
		pRGBColor.Green = 223
		pRGBColor.Blue = 223

		myLineSymbol.Color = pRGBColor
		pSymbol = myLineSymbol
		pSymbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPXOrPen

		myPolyLine.FromPoint = FromPoint
		myPolyLine.ToPoint = ToPoint

		pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache)
		pActiveView.ScreenDisplay.SetSymbol(pSymbol)
		pActiveView.ScreenDisplay.DrawPolyline(myPolyLine)
		pActiveView.ScreenDisplay.FinishDrawing()
	End Sub

	Public Function DrawPolyLine(ByVal pPolyline As ESRI.ArcGIS.Geometry.Polyline, Optional ByVal Color As Integer = -1, Optional ByVal Width As Double = 1.0, Optional ByVal drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
		Dim pLineElement As ESRI.ArcGIS.Carto.ILineElement
		Dim pElementOfpLine As ESRI.ArcGIS.Carto.IElement
		Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
		Dim pRGB As ESRI.ArcGIS.Display.IRgbColor

		pRGB = New ESRI.ArcGIS.Display.RgbColor
		pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol

		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pLineSym.Color = pRGB
		pLineSym.Style = ESRI.ArcGIS.Display.esriSimpleLineStyle.esriSLSSolid
		pLineSym.Width = Width

		pLineElement = New ESRI.ArcGIS.Carto.LineElement

		pElementOfpLine = pLineElement
		pElementOfpLine.Geometry = pPolyline

		pLineElement.Symbol = pLineSym

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementOfpLine, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementOfpLine
	End Function

	Public Function DrawPolygon(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon, Optional ByVal FillColor As Integer = -1, Optional ByVal drawFS As ESRI.ArcGIS.Display.esriSimpleFillStyle = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross, Optional ByVal drawFlg As Boolean = True, Optional ByVal OutlineWidth As Integer = 1, Optional ByRef OutlineColor As Integer = -1) As ESRI.ArcGIS.Carto.IElement
		Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
		Dim pFillSym As ESRI.ArcGIS.Display.ISimpleFillSymbol
		Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
		Dim pFillShpElement As ESRI.ArcGIS.Carto.IFillShapeElement
		Dim pElementofPoly As ESRI.ArcGIS.Carto.IElement

		pRGB = New ESRI.ArcGIS.Display.RgbColor
		If FillColor <> -1 Then
			pRGB.RGB = FillColor
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pFillSym = New ESRI.ArcGIS.Display.SimpleFillSymbol
		pFillShpElement = New ESRI.ArcGIS.Carto.PolygonElement

		pFillSym.Color = pRGB
		pFillSym.Style = drawFS

		If OutlineColor <> -1 Then
			pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol
			pRGB.RGB = OutlineColor

			pLineSym.Width = OutlineWidth
			pLineSym.Color = pRGB

			pFillSym.Outline = pLineSym
		End If

		pElementofPoly = pFillShpElement
		pElementofPoly.Geometry = pPolygon

		pFillShpElement.Symbol = pFillSym
		DrawPolygon = pElementofPoly

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementofPoly, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	Public Sub shall_SortfSort(ByRef obsArray() As ObstacleType)
		Dim TempVal As ObstacleType
		Dim GapSize, i, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For i = (GapSize + FirstRow) To LastRow
				CurPos = i
				TempVal = obsArray(i)
				Do While obsArray(CurPos - GapSize).fSort > TempVal.fSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next i
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortfSortD(ByRef obsArray() As ObstacleType)
		Dim TempVal As ObstacleType
		Dim GapSize, i, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For i = (GapSize + FirstRow) To LastRow
				CurPos = i
				TempVal = obsArray(i)
				Do While obsArray(CurPos - GapSize).fSort < TempVal.fSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next i
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSort(ByRef obsArray() As ObstacleType)
		Dim TempVal As ObstacleType
		Dim GapSize, i, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For i = (GapSize + FirstRow) To LastRow
				CurPos = i
				TempVal = obsArray(i)
				Do While obsArray(CurPos - GapSize).sSort > TempVal.sSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next i
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSortD(ByRef obsArray() As ObstacleType)
		Dim TempVal As ObstacleType
		Dim GapSize, i, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For i = (GapSize + FirstRow) To LastRow
				CurPos = i
				TempVal = obsArray(i)
				Do While obsArray(CurPos - GapSize).sSort < TempVal.sSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next i
		Loop Until GapSize = 1
	End Sub

	Sub TextBoxInteger(ByRef KeyChar As Char, ByRef BoxText As String)
		If KeyChar < Chr(32) Then Return
		If (KeyChar < "0" Or KeyChar > "9") Then KeyChar = Chr(0)
	End Sub

	Function HmaxInInnerSector(ByRef ObstacleList() As ObstacleType, ByVal SecI As Integer, Sector As MSASectorType, Navaid As NavaidType) As Integer
		Dim i As Integer

		Dim pInnerRelation As ESRI.ArcGIS.Geometry.IRelationalOperator = Sector.InnerBuffer
		Dim pOuterRelation As ESRI.ArcGIS.Geometry.IRelationalOperator = Sector.OuterBuffer

		Dim hMax As Double = -9999
		Dim Result As Integer = -1
		Dim n As Integer = UBound(ObstacleList)

		'===============================================================
		For i = 0 To n
			Dim InInner As Boolean = False
			Dim InOuter As Boolean = False

			ObstacleList(i).iSector(SecI) = (ObstacleList(i).iSector(SecI) And 12)

			If ObstacleList(i).iSector(SecI) > WholeSector Then
				InInner = Not pInnerRelation.Disjoint(ObstacleList(i).pGeomPrj)
				InOuter = Not pOuterRelation.Disjoint(ObstacleList(i).pGeomPrj)
				'===============================================================
				If InInner Then
					ObstacleList(i).iSector(SecI) = ObstacleList(i).iSector(SecI) Or InnerSector
					If ObstacleList(i).Height > hMax Then
						hMax = ObstacleList(i).Height
						Result = i
					End If
				End If

				If InOuter Then
					ObstacleList(i).iSector(SecI) = ObstacleList(i).iSector(SecI) Or OuterSector
				End If
			End If
		Next i

		Return Result
	End Function

	Public Function GetTAAObstacles(ByRef Obstacles() As ObstacleType, ByVal pPoly As ESRI.ArcGIS.Geometry.IPolygon) As Integer
		Dim i As Integer
		Dim j As Integer
		Dim n As Integer
		Dim result As Integer

		Dim dist As Double
		Dim h As Double = -10000.0
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		pProxi = pPoly
		n = ObstacleList.Length - 1
		ReDim Obstacles(n)
		j = -1
		result = -1

		For i = 0 To n
			dist = pProxi.ReturnDistance(ObstacleList(i).pGeomPrj)
			If dist <= ObstacleList(i).HorAccuracy Then
				j += 1
				Obstacles(j) = ObstacleList(i)

				If Obstacles(j).Height > h Then
					h = Obstacles(j).Height
					result = j
				End If
			End If
		Next

		ReDim Preserve Obstacles(j)
		Return result
	End Function
End Module
