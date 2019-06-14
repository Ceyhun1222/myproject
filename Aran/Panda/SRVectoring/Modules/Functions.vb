Option Strict Off
Option Explicit On
Option Compare Text
Imports System.Runtime.InteropServices

Module Functions

	Public EnabledColor As Color = SystemColors.Window
	Public DisabledColor As Color = SystemColors.ButtonFace

	Public WritableColor As Color = SystemColors.Window
	Public ReadOnlyColor As Color = SystemColors.ButtonFace

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function AppendMenu(hMenu As IntPtr, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function InsertMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
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

	Public Sub SetReadOnly(ByRef ctrl As System.Windows.Forms.Control, ByVal isReadOnly As Boolean)
		ctrl.Enabled = Not isReadOnly
		If ctrl.Enabled Then
			ctrl.BackColor = IIf(isReadOnly, ReadOnlyColor, WritableColor)
		Else
			ctrl.BackColor = DisabledColor
		End If
		'    If TypeOf ctrl Is ComboBox Then ctrl.Style = IIf(isReadOnly, 1, 2)
	End Sub

	Public Sub EnableControl(ByRef ctrl As System.Windows.Forms.Control, ByVal vEnable As Boolean)
		ctrl.Enabled = vEnable

		If Not ((TypeOf ctrl Is System.Windows.Forms.Label) Or (TypeOf ctrl Is System.Windows.Forms.RadioButton)) Then
			ctrl.BackColor = IIf(vEnable, EnabledColor, DisabledColor)
		End If

		'    If TypeOf ctrl Is TextBox Then ctrl.BackColor = IIf(vEnable, EnabledColor, DisabledColor)
	End Sub

	Public Sub ShowErrorMessage(message As String, Optional ByVal isError As Boolean = True)
		MessageBox.Show(message, ModuleName, MessageBoxButtons.OK, IIf(isError, MessageBoxIcon.Error, MessageBoxIcon.Warning))
	End Sub

	Public Function ConvertDistance(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * DistanceConverter(DistanceUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding - 0.4999999) * DistanceConverter(DistanceUnit).Rounding
			Case eRoundMode.NERAEST ', eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding) * DistanceConverter(DistanceUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding + 0.4999999) * DistanceConverter(DistanceUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertHeight(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * HeightConverter(HeightUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding - 0.4999999) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.NERAEST ', eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding + 0.4999999) * HeightConverter(HeightUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertSpeed(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * SpeedConverter(SpeedUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding - 0.4999999) * SpeedConverter(SpeedUnit).Rounding
			Case eRoundMode.NERAEST ', eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding) * SpeedConverter(SpeedUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding + 0.4999999) * SpeedConverter(SpeedUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertDSpeed(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NERAEST) As Double
		If (RoundMode < 0) Or (RoundMode > 3) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * DSpeedConverter(HeightUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding - 0.4999999) * DSpeedConverter(HeightUnit).Rounding
			Case eRoundMode.NERAEST ', eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding) * DSpeedConverter(HeightUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding + 0.4999999) * DSpeedConverter(HeightUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function DeConvertDistance(ByVal Val_Renamed As Double) As Double
		DeConvertDistance = Val_Renamed / DistanceConverter(DistanceUnit).Multiplier
	End Function

	Public Function DeConvertHeight(ByVal Val_Renamed As Double) As Double
		DeConvertHeight = Val_Renamed / HeightConverter(HeightUnit).Multiplier
	End Function

	Public Function DeConvertSpeed(ByVal Val_Renamed As Double) As Double
		DeConvertSpeed = Val_Renamed / SpeedConverter(SpeedUnit).Multiplier
	End Function

	Public Function DeConvertDSpeed(ByVal Val_Renamed As Double) As Double
		DeConvertDSpeed = Val_Renamed / DSpeedConverter(HeightUnit).Multiplier
	End Function

	Public Function Dir2Azt(ByVal ptPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal dir_Renamed As Double) As Double
		Dim resD As Double
		Dim resI As Double
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint

		Pt11 = ToGeo(ptPrj)
		Pt10 = ToGeo(PointAlongPlane(ptPrj, dir_Renamed, 10.0))

		ReturnGeodesicAzimuth(Pt11.X, Pt11.Y, Pt10.X, Pt10.Y, resD, resI)
		Dir2Azt = resD
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

		Azt2Dir = ReturnAngleInDegrees(Pt10, Pt11)
	End Function

	Public Function ToGeo(ByRef pPrjGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		pClone = pPrjGeom
		ToGeo = pClone.Clone
		ToGeo.SpatialReference = pSpRefPrj
		ToGeo.Project(pSpRefShp)
	End Function

	Public Function ToPrj(ByRef pGeoGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		pClone = pGeoGeom
		ToPrj = pClone.Clone
		ToPrj.SpatialReference = pSpRefShp
		ToPrj.Project(pSpRefPrj)
	End Function

	Sub TextBoxFloat(ByRef KeyAscii As Short, ByRef BoxText As String)
		Dim N As Integer
		Dim sDecSep As String
		Dim DecSep As Integer

		sDecSep = Mid(CStr(1.1), 2, 1)
		DecSep = Asc(sDecSep)

		If KeyAscii < 32 Then Exit Sub

		If ((KeyAscii < Asc("0") Or KeyAscii > Asc("9")) And KeyAscii <> DecSep) Then
			KeyAscii = 0
		Else
			If KeyAscii = DecSep Then
				N = InStr(BoxText, sDecSep)
				If (N <> 0) Then KeyAscii = 0
			End If
		End If
	End Sub

	Function RealMode(ByRef X As Double, ByRef base As Integer) As Double
		Dim dX As Double
		Dim N As Short
		N = Fix(X)
		dX = X - N
		X = N Mod base + dX

		If (X < 0.0) Then X = X + base
		If (X > base / 2) Then X = X - base

		RealMode = X
	End Function

	Sub DD2DMS(ByRef xDeg As Double, ByRef xMin As Double, ByRef xSec As Double, ByRef Sign As Integer, ByRef nMod As Integer)
		Dim X As Double
		Dim dX As Double
		X = System.Math.Abs(xDeg) * Sign
		X = System.Math.Round(X, 10)

		If (nMod > 0) Then X = RealMode(X, nMod)

		Sign = System.Math.Sign(X)

		X = System.Math.Abs(X)
		xDeg = Fix(X)
		dX = (X - xDeg) * 60
		dX = System.Math.Round(dX, 8)
		xMin = Fix(dX)
		xSec = (dX - xMin) * 60
		xSec = System.Math.Round(xSec, 6)
	End Sub

	Sub DMS2Str(ByRef xDeg As Double, ByRef xMin As Double, ByVal xSec As Double, ByRef yDeg As Double, ByRef yMin As Double, ByVal ySec As Double, ByRef Xstr As String, ByRef Ystr As String, ByRef LonSide As String, ByRef LatSide As String)
		Dim xDegStr As String
		Dim xMinStr As String
		Dim xSecStr As String
		Dim yDegStr As String
		Dim yMinStr As String
		Dim ySecStr As String
		Dim DecSep As String

		DecSep = Mid(CStr(1.1), 2, 1)

		If (xDeg < 10) Then
			xDegStr = "00" + CStr(xDeg)
		ElseIf (xDeg < 100) Then
			xDegStr = "0" + CStr(xDeg)
		Else
			xDegStr = CStr(xDeg)
		End If
		xDegStr = xDegStr + "°"

		If (xMin < 10) Then
			xMinStr = "0" + CStr(xMin)
		Else
			xMinStr = CStr(xMin)
		End If
		xMinStr = xMinStr + "'"
		xSec = System.Math.Round(xSec * 100) / 100

		If (xSec < 10) Then
			xSecStr = "0" + CStr(Fix(xSec))
		Else
			xSecStr = CStr(Fix(xSec))
		End If
		xSecStr = xSecStr + DecSep

		xSec = System.Math.Round((xSec - Fix(xSec)) * 100)

		If (xSec < 10) Then
			xSecStr = xSecStr + "0" + CStr(xSec)
		Else
			xSecStr = xSecStr + CStr(xSec)
		End If
		xSecStr = xSecStr + "''" + LonSide
		Xstr = xDegStr + xMinStr + xSecStr


		If (yDeg < 10) Then
			yDegStr = "0" + CStr(yDeg)
		Else
			yDegStr = CStr(yDeg)
		End If
		yDegStr = yDegStr + "°"

		If (yMin < 10) Then
			yMinStr = "0" + CStr(yMin)
		Else
			yMinStr = CStr(yMin)
		End If
		yMinStr = yMinStr + "'"

		ySec = System.Math.Round(ySec * 100) / 100

		If (ySec < 10) Then
			ySecStr = "0" + CStr(Fix(ySec))
		Else
			ySecStr = CStr(Fix(ySec))
		End If
		ySecStr = ySecStr + DecSep
		ySec = System.Math.Round((ySec - Fix(ySec)) * 100)

		If (ySec < 10) Then
			ySecStr = ySecStr + "0" + CStr(ySec)
		Else
			ySecStr = ySecStr & CStr(ySec)
		End If
		ySecStr = ySecStr + "''" + LatSide
		Ystr = yDegStr + yMinStr + ySecStr
	End Sub

	Sub DD2Str(ByVal xDeg As Double, ByVal yDeg As Double, ByRef Xstr As String, ByRef Ystr As String, ByRef LonSide As String, ByRef LatSide As String)
		Dim xMin As Double
		Dim xSec As Double
		Dim yMin As Double
		Dim ySec As Double

		Dim xSign As Short
		Dim ySign As Short

		xSign = System.Math.Sign(xDeg)
		ySign = System.Math.Sign(yDeg)

		DD2DMS(xDeg, xMin, xSec, xSign, 360)
		DD2DMS(yDeg, yMin, ySec, ySign, 0)

		DMS2Str(xDeg, xMin, xSec, yDeg, yMin, ySec, Xstr, Ystr, LonSide, LatSide)
	End Sub

	Public Function IAS2TAS(ByRef IAS As Double, ByRef h As Double, ByRef dT As Double) As Double
		IAS2TAS = IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * h) / ((288.0 - 0.006496 * h) ^ 2.628)
	End Function

	Public Function Bank2Radius(ByRef Bank As Double, ByRef V As Double) As Double
		Dim Rv As Double
		Rv = 6.355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
		If (Rv > 0.003) Then Rv = 0.003
		If (Rv > 0) Then Return V / (20.0 * PI * Rv)
		Return -1
	End Function

	Public Function Radius2Bank(ByRef R As Double, ByRef V As Double) As Double
		If (R > 0.0) Then Return RadToDeg(System.Math.Atan(V * V / (20.0 * R * 6.355)))
		Return -1
	End Function

	'Function DistToSpeed(ByRef Dist As Double) As Double
	'	Dim I As Integer
	'	Dim fTmp As Double
	'	Dim SpeedList() As Double = {356.0, 370.0, 387.0, 404.0, 424.0, 441.0, 452.0, 459.0, 467.0, 472.0, 478.0, 483.0, 487.0, 491.0, 493.0, 494.0, 498.0, 502.0, 504.0, 511.0, 515.0, 519.0, 524.0, 526.0, 530.0}

	'       fTmp = Dist / 1852.0
	'	I = System.Math.Round(fTmp + 0.4999999)
	'	If I > 24 Then I = 24
	'	If I < 0 Then I = 0
	'       DistToSpeed = SpeedList(I)
	'End Function

	'Function HeightToBank(ByRef h As Double) As Double
	'	If h > 914.4 Then Return 25.0
	'	If h > 304.8 Then Return 20.0
	'	Return 15.0
	'End Function

	'   Public Function ArcSin(ByVal X As Double) As Double
	'       If System.Math.Abs(X) >= 1.0 Then
	'           If X > 0.0 Then
	'               ArcSin = 0.5 * PI
	'           Else
	'               ArcSin = -0.5 * PI
	'           End If
	'       Else
	'           ArcSin = System.Math.Atan(X / System.Math.Sqrt(-X * X + 1.0))
	'       End If
	'   End Function

	'Public Function ArcCos(ByVal X As Double) As Double
	'       If System.Math.Abs(X) >= 1.0 Then
	'           ArcCos = 0.0
	'       Else
	'           ArcCos = System.Math.Atan(-X / System.Math.Sqrt(-X * X + 1.0)) + 0.5 * PI
	'       End If
	'End Function

	'Public Function Max(ByVal X As Double, ByVal Y As Double) As Double
	'	If X > Y Then
	'		Max = X
	'	Else
	'		Max = Y
	'	End If
	'End Function

	'Public Function Min(ByVal X As Double, ByVal Y As Double) As Double
	'	If X < Y Then
	'		Min = X
	'	Else
	'		Min = Y
	'	End If
	'End Function

	Public Function CreatePrjCircle(ByRef pPoint1 As ESRI.ArcGIS.Geometry.IPoint, ByRef R As Double, Optional ByVal N As Integer = 360) As ESRI.ArcGIS.Geometry.Polygon
		Dim I As Integer
		Dim dA As Double
		Dim iInRad As Double

		Dim pt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pt = New ESRI.ArcGIS.Geometry.Point
		pPolygon = New ESRI.ArcGIS.Geometry.Polygon
		dA = 360.0 * DegToRadValue / N

		N = N - 1
		For I = 0 To N
			iInRad = DegToRad(I)
			pt.X = pPoint1.X + R * System.Math.Cos(iInRad)
			pt.Y = pPoint1.Y + R * System.Math.Sin(iInRad)
			pPolygon.AddPoint(pt)
		Next I

		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return pPolygon
	End Function

	Function CreateArcPrj(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim I As Integer
		Dim J As Integer

		Dim R As Double
		Dim dX As Double
		Dim dY As Double
		Dim daz As Double
		Dim AztTo As Double
		Dim iInRad As Double
		Dim AngStep As Double
		Dim AztFrom As Double
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim pt As ESRI.ArcGIS.Geometry.IPoint

		ptCur = New ESRI.ArcGIS.Geometry.Point
		pt = New ESRI.ArcGIS.Geometry.Point
		CreateArcPrj = New ESRI.ArcGIS.Geometry.Polygon

		dX = ptFrom.X - ptCnt.X
		dY = ptFrom.Y - ptCnt.Y
		R = System.Math.Sqrt(dX * dX + dY * dY)

		AztFrom = RadToDeg(Math.Atan2(dY, dX))
		AztFrom = Modulus(AztFrom)

		AztTo = RadToDeg(Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X))
		AztTo = Modulus(AztTo)

		daz = Modulus((AztTo - AztFrom) * ClWise)
		AngStep = 1

		I = Math.Round(daz / AngStep)
		If (I < 1) Then
			I = 1
		ElseIf (I < 5) Then
			I = 5
		ElseIf (I < 10) Then
			I = 10
		End If

		AngStep = daz / I

		CreateArcPrj.AddPoint(ptFrom)
		For J = 1 To I - 1
			iInRad = DegToRad(AztFrom + J * AngStep * ClWise)
			pt.X = ptCnt.X + R * System.Math.Cos(iInRad)
			pt.Y = ptCnt.Y + R * System.Math.Sin(iInRad)
			CreateArcPrj.AddPoint(pt)
		Next J
		CreateArcPrj.AddPoint(ptTo)
	End Function

	Function CreateArcPrj2(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.Path
		Dim I As Integer
		Dim J As Integer
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim pt As ESRI.ArcGIS.Geometry.IPoint
		Dim R As Double
		Dim AngStep As Double
		Dim dX As Double
		Dim dY As Double
		Dim AztFrom As Double
		Dim AztTo As Double
		Dim iInRad As Double
		Dim daz As Double

		ptCur = New ESRI.ArcGIS.Geometry.Point
		pt = New ESRI.ArcGIS.Geometry.Point

		Dim pCol As ESRI.ArcGIS.Geometry.IPointCollection
		pCol = New ESRI.ArcGIS.Geometry.Path

		dX = ptFrom.X - ptCnt.X
		dY = ptFrom.Y - ptCnt.Y
		R = System.Math.Sqrt(dX * dX + dY * dY)

		AztFrom = RadToDeg(Math.Atan2(dY, dX))
		AztFrom = Modulus(AztFrom, 360.0)

		AztTo = RadToDeg(Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X))
		AztTo = Modulus(AztTo, 360.0)

		daz = Modulus((AztTo - AztFrom) * ClWise, 360.0)
		AngStep = 1
		I = daz / AngStep
		If (I < 10) Then I = 10
		AngStep = daz / I

		pCol.AddPoint(ptFrom)
		For J = 1 To I - 1
			iInRad = DegToRad(AztFrom + J * AngStep * ClWise)
			pt.X = ptCnt.X + R * System.Math.Cos(iInRad)
			pt.Y = ptCnt.Y + R * System.Math.Sin(iInRad)
			pCol.AddPoint(pt)
		Next J
		pCol.AddPoint(ptTo)

		CreateArcPrj2 = pCol
	End Function

	Function CreateArcPrj_ByArc(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef side As Integer) As ESRI.ArcGIS.Geometry.ISegment
		Dim ClWise As Boolean
		Dim isMinor As Boolean
		Dim ha As Double
		Dim Rds As Double

		Dim ptMid As ESRI.ArcGIS.Geometry.IPoint
		Dim pCircularArc As ESRI.ArcGIS.Geometry.IConstructCircularArc

		Rds = ReturnDistanceInMeters(ptCnt, ptFrom)
		isMinor = (Modulus(side * (ptTo.M - ptFrom.M)) > 180.0)

		ha = Modulus(-side * (ptTo.M - ptFrom.M), 360.0)
		'Set ptMid = PointAlongPlane(ptCnt, ptFrom.M - (side * ha / 2), Rds)
		ptMid = PointAlongPlane(ptCnt, ReturnAngleInDegrees(ptCnt, ptFrom) - side * (ha / 2), Rds)

		'DrawPoint ptMid
		ClWise = (side = 1)

		pCircularArc = New ESRI.ArcGIS.Geometry.CircularArc
		'pCircularArc.ConstructEndPointsRadius ptFrom, ptTo, Not ClWise, Rds, isMinor
		pCircularArc.ConstructThreePoints(ptFrom, ptMid, ptTo, True)
		CreateArcPrj_ByArc = pCircularArc
	End Function

	Function CreateArcPrj_ByArc_2(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef side As Integer) As ESRI.ArcGIS.Geometry.ISegment

		Dim ClWise As Boolean
		Dim Rds As Double
		Dim isMinor As Boolean
		Rds = ReturnDistanceInMeters(ptCnt, ptFrom)
		isMinor = (Modulus(side * (ptTo.M - ptFrom.M), 360.0) > 180.0)

		ClWise = (side = 1)

		Dim pCircularArc As ESRI.ArcGIS.Geometry.IConstructCircularArc
		pCircularArc = New ESRI.ArcGIS.Geometry.CircularArc
		pCircularArc.ConstructEndPointsRadius(ptFrom, ptTo, Not ClWise, Rds, isMinor)
		CreateArcPrj_ByArc_2 = pCircularArc
	End Function

	Function SpiralTouchAngle(ByRef r0 As Double, ByRef E As Double, ByRef aztNominal As Double, ByRef AztTouch As Double, ByRef TurnDir As Integer) As Double
		Dim I As Integer
		Dim TurnAngle As Double
		Dim TouchAngle As Double
		Dim D As Double
		Dim delta As Double
		Dim DegE As Double

		TouchAngle = Modulus((AztTouch - aztNominal) * TurnDir)
		TouchAngle = DegToRad(TouchAngle)
		TurnAngle = TouchAngle
		DegE = RadToDeg(E)

		For I = 0 To 9
			D = DegE / (r0 + DegE * TurnAngle)
			delta = (TurnAngle - TouchAngle - System.Math.Atan(D)) / (2.0 - 1.0 / (D * D + 1.0))
			TurnAngle = TurnAngle - delta
			If (System.Math.Abs(delta) < radEps) Then
				Exit For
			End If
		Next I

		SpiralTouchAngle = Modulus(RadToDeg(TurnAngle))
	End Function

	Public Function ReturnAngleInDegrees(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint) As Double
		ReturnAngleInDegrees = Modulus(RadToDeg(Math.Atan2(ptOut.Y - ptFrom.Y, ptOut.X - ptFrom.X)))
	End Function

	Public Function ReturnDistanceInMeters(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptOut.X - ptFrom.X
		fdY = ptOut.Y - ptFrom.Y
		ReturnDistanceInMeters = System.Math.Sqrt(fdX * fdX + fdY * fdY)
	End Function

	Function SubtractAngles(ByVal X As Double, ByVal Y As Double) As Double
		X = Modulus(X)
		Y = Modulus(Y)
		SubtractAngles = Modulus(X - Y)
		If SubtractAngles > 180.0 Then SubtractAngles = 360.0 - SubtractAngles
	End Function

	Function SubtractAnglesWithSign(ByVal StRad As Double, ByVal EndRad As Double, ByRef Turn As Integer) As Double
		SubtractAnglesWithSign = Modulus((EndRad - StRad) * Turn)
		If SubtractAnglesWithSign > 180.0 Then
			SubtractAnglesWithSign = SubtractAnglesWithSign - 360.0
		End If
	End Function

	'   Public Function Quadric(ByRef A As Double, ByRef B As Double, ByRef C As Double, ByRef x0 As Double, ByRef x1 As Double) As Integer
	'       Dim D As Double
	'       Dim fTmp As Double

	'	D = B * B - 4.0 * A * C
	'	If D < 0.0 Then
	'		Quadric = 0
	'	ElseIf (D = 0.0) Or (A = 0.0) Then
	'		Quadric = 1
	'		If A = 0.0 Then
	'			x0 = -C / B
	'		Else
	'			x0 = -0.5 * B / A
	'		End If
	'	Else
	'		Quadric = 2
	'		D = System.Math.Sqrt(D)
	'		fTmp = 0.5 / A
	'		If fTmp > 0.0 Then
	'			x0 = (-B - D) * fTmp
	'			x1 = (-B + D) * fTmp
	'		Else
	'			x0 = (-B + D) * fTmp
	'			x1 = (-B - D) * fTmp
	'		End If
	'	End If
	'End Function

	'Public Function CutNavPoly(ByRef poly As ESRI.ArcGIS.Geometry.Polygon, ByRef CutLine As ESRI.ArcGIS.Geometry.IPolyline, ByRef side As Integer) As ESRI.ArcGIS.Geometry.Polygon ', Optional bDraw As Boolean = False
	'    Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'    Dim pUnspecified As ESRI.ArcGIS.Geometry.IPointCollection
	'    Dim pGeo As ESRI.ArcGIS.Geometry.IGeometry
	'    Dim pRight As ESRI.ArcGIS.Geometry.Polygon
	'    Dim pLeft As ESRI.ArcGIS.Geometry.Polygon
	'    Dim pLine As ESRI.ArcGIS.Geometry.ILine

	'    Dim dir_Renamed As Double

	'    pLine = New ESRI.ArcGIS.Geometry.Line
	'    pLine.FromPoint = CutLine.FromPoint
	'    pLine.ToPoint = CutLine.ToPoint

	'    ClipByLine(poly, CutLine, pLeft, pRight, pUnspecified)
	'    'If bDraw Then
	'    '    DrawPolygon pRight, 255
	'    '    DrawPolygon pUnspecified, 0
	'    '    DrawPolygon pLeft, RGB(0, 0, 255)
	'    '    DrawPolyLine CutLine, 255, 2
	'    '    DrawPolygon Poly, RGB(0, 255, 0)
	'    'End If
	'    '    Set pGeo = pUnspecified
	'    '    Set pArea = pUnspecified
	'    '    Dir = pUnspecified.PointCount
	'    '    Dir = pArea.Area

	'    CutNavPoly = New ESRI.ArcGIS.Geometry.Polygon
	'    dir_Renamed = RadToDeg(pLine.Angle)

	'    If side > 0 Then
	'        pGeo = pLeft
	'        If Not pGeo.IsEmpty Then
	'            pGeo = pUnspecified
	'            If Not pGeo.IsEmpty Then '            If pArea.Area > 0.5 Then
	'                If SideDef(pLine.FromPoint, dir_Renamed, pUnspecified.Point(0)) < 0 Then
	'                    pTopoOper = pLeft
	'                    CutNavPoly = pTopoOper.Union(pUnspecified)
	'                End If
	'            End If
	'            CutNavPoly = pLeft
	'        Else
	'            pGeo = pUnspecified
	'            If Not pGeo.IsEmpty Then
	'                If SideDef(pLine.FromPoint, dir_Renamed, pUnspecified.Point(0)) < 0 Then
	'                    CutNavPoly = pUnspecified
	'                End If
	'            End If
	'        End If
	'    Else
	'        pGeo = pRight
	'        If Not pGeo.IsEmpty Then
	'            pGeo = pUnspecified
	'            If Not pGeo.IsEmpty Then '       If pArea.Area > 0.5 Then
	'                If SideDef(pLine.FromPoint, dir_Renamed, pUnspecified.Point(0)) > 0 Then
	'                    pTopoOper = pRight
	'                    CutNavPoly = pTopoOper.Union(pUnspecified)
	'                End If
	'            End If
	'            CutNavPoly = pRight '        If Not pGeo.IsEmpty Then
	'        Else
	'            pGeo = pUnspecified
	'            If Not pGeo.IsEmpty Then
	'                If SideDef(pLine.FromPoint, dir_Renamed, pUnspecified.Point(0)) > 0 Then
	'                    CutNavPoly = pUnspecified
	'                End If
	'            End If
	'        End If
	'    End If
	'    pTopoOper = CutNavPoly
	'    pTopoOper.IsKnownSimple_2 = False
	'    pTopoOper.Simplify()
	'End Function

	'    Public Sub CutPoly(ByRef poly As ESRI.ArcGIS.Geometry.Polygon, ByRef CutLine As ESRI.ArcGIS.Geometry.Polyline, ByRef side As Integer)
	'		Dim Geocollect As ESRI.ArcGIS.Geometry.IGeometryCollection
	'        Dim Topo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'        'Dim Topo1 As ITopologicalOperator
	'        Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
	'        Dim Cutter As ESRI.ArcGIS.Geometry.IPolyline
	'        Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
	'        Dim TmpPolygon As ESRI.ArcGIS.Geometry.IPolygon2

	'        Dim SRC As Integer

	'        Dim tmpAzt As Double
	'        Dim dist0 As Double
	'        Dim Dist1 As Double
	'        Dim Dist As Double
	'        Dim GIx As Integer
	'        Dim Ix As Integer
	'        '=================================
	'        Dim pLeft As ESRI.ArcGIS.Geometry.Polygon
	'        Dim pRight As ESRI.ArcGIS.Geometry.Polygon
	'        Dim pUnspecified As ESRI.ArcGIS.Geometry.Polygon
	'        '=================================
	'        On Error GoTo ErrorHandler
	'        'SRC = 0
	'        'Dim pPolygon As IPolygon
	'        'Set pPolygon = Poly
	'        'DrawPolygon pPolygon, 0
	'        Cutter = CutLine

	'        tmpAzt = ReturnAngleInDegrees(Cutter.FromPoint, Cutter.ToPoint)
	'        Dist = ReturnDistanceInMeters(Cutter.FromPoint, Cutter.ToPoint)

	'        Topo = Cutter
	'        Topo.IsKnownSimple_2 = False
	'        Topo.Simplify()

	'        Topo = poly
	'        Topo.IsKnownSimple_2 = False
	'        Topo.Simplify()

	'        'Set TmpPolygon = Topo
	'		'TmpPolygon.Generalize 20.0

	'        '=============================
	'        'Cutter.ReverseOrientation
	'        'DrawPolygon Poly, RGB(255, 255, 0)
	'        'DrawPolyLine Cutter, 255

	'        'Exit Sub
	'        'Set PtTmp = New Point
	'		'PtTmp.PutCoords Cutter.ToPoint.x + 10.0, Cutter.ToPoint.Y
	'        'Cutter.ToPoint = PtTmp
	'        'Set Topo = Cutter
	'        'Topo.Simplify
	'        'Set Topo = Poly
	'        '=============================

	'        'Exit Sub
	'        'Set Topo = pPolygon
	'        'Topo.Simplify
	'        'Set TmpPoly = Topo.Intersect(Topo1, 2)

	'        'SRC = 1
	'        'Set TmpPoly = Topo.Intersect(Cutter, 2)

	'        tmpPoly = ClipByPoly(Cutter, poly)
	'        'SRC = 2

	'        If tmpPoly.PointCount <> 0 Then
	'            Geocollect = tmpPoly
	'            If Geocollect.GeometryCount > 1 Then
	'                For Ix = 0 To Geocollect.GeometryCount - 1
	'                    tmpPoly = Geocollect.Geometry(Ix)
	'                    dist0 = ReturnDistanceInMeters(tmpPoly.Point(0), Cutter.FromPoint)
	'                    Dist1 = ReturnDistanceInMeters(tmpPoly.Point(1), Cutter.FromPoint)
	'                    If dist0 > Dist1 Then dist0 = Dist1

	'                    If Dist > dist0 Then
	'                        GIx = Ix
	'                        Dist = dist0
	'                    End If
	'                Next Ix

	'                tmpPoly = Geocollect.Geometry(GIx)
	'                Dist = ReturnDistanceInMeters(tmpPoly.Point(0), Cutter.FromPoint)
	'                dist0 = ReturnDistanceInMeters(tmpPoly.Point(1), Cutter.FromPoint)

	'                If Dist < dist0 Then Dist = dist0
	'				ptTmp = PointAlongPlane(Cutter.FromPoint, tmpAzt, Dist + 5.0)
	'                Cutter.ToPoint = ptTmp
	'            End If

	'            '    If Side < 0 Then
	'            '        Topo.Cut Cutter, TmpPoly, Poly
	'            '    Else
	'            '        Topo.Cut Cutter, Poly, TmpPoly
	'            '    End If
	'            'SRC = 3
	'            ClipByLine(poly, Cutter, pLeft, pRight, pUnspecified)
	'            'SRC = 4

	'            If side < 0 Then
	'                poly = pRight
	'            Else
	'                poly = pLeft
	'            End If

	'        End If

	'        On Error GoTo 0

	'        Exit Sub

	'ErrorHandler:  ' Error-handling routine.

	'        DrawPolyLine(Cutter, 255)
	'        DrawPolygon(poly, RGB(255, 255, 0))
	'        MsgBox(" ErrNum = " & CStr(Err.Number) & ": " & Err.Description)

	'        '    MsgBox "Ошибка среды 'ArcMap' фирмы 'ESRI'!!! ErrNum= " + CStr(Err.Number) + "  " + Err.Description
	'        '    MsgBox "Ошибка среды 'ArcMap'. ErrNum = " + CStr(Err.Number) + ": " + Err.Description
	'        '    MsgBox "Ошибка среды 'ArcMap'. ErrNum= " + CStr(SRC) + "  " + Err.Description
	'        '    Exit Sub
	'        '    If Err.Number = -2147220943 Then
	'        '
	'		'        Set ptTmp = PointAlongPlane(Cutter.ToPoint, tmpAzt + 90.0 * side, 0.01)
	'        '        Cutter.ToPoint = ptTmp
	'        '' '        DrawPolyLine Cutter, 0
	'        '        Resume      ' Resume execution at same line that caused the error.
	'        '    Else 'If Err.Number <> -2147220968 Then
	'        '        MsgBox "Ошибка среды 'ArcMap' фирмы 'ESRI' !!!"
	'        ''        If Err.Number <> -2147220968 Then
	'        ''            DrawPolygon Poly, RGB(255, 255, 0)
	'        ''            DrawPolyLine Cutter, 255
	'        ''        End If
	'        '        Exit Sub
	'        '        Resume Next ' Resume execution at next line that caused the error.
	'        '    End If
	'    End Sub

	Public Function LineLineIntersect(ByRef pPt1 As ESRI.ArcGIS.Geometry.Point, ByRef dir1 As Double, ByRef pPt2 As ESRI.ArcGIS.Geometry.Point, ByRef dir2 As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim pConstructor As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = pPt1
		LineLineIntersect = pClone.Clone
		pConstructor = LineLineIntersect
		pConstructor.ConstructAngleIntersection(pPt1, DegToRad(dir1), pPt2, DegToRad(dir2))
	End Function

	Public Function CircleVectorIntersect(ByRef PtCent As ESRI.ArcGIS.Geometry.IPoint, ByVal Radius As Double, ByRef ptVect As ESRI.ArcGIS.Geometry.IPoint, ByVal DirVect As Double, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim DistCnt2Vect As Double
		Dim D As Double
		Dim Constr As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = PtCent
		ptTmp = pClone.Clone
		Constr = ptTmp

		Constr.ConstructAngleIntersection(PtCent, DegToRad(DirVect + 90.0), ptVect, DegToRad(DirVect))
		CircleVectorIntersect = -1.0

		If ptTmp.IsEmpty Then
			ptRes = New ESRI.ArcGIS.Geometry.Point
			Exit Function
		End If

		DistCnt2Vect = ReturnDistanceInMeters(PtCent, ptTmp)

		If DistCnt2Vect < Radius Then
			D = System.Math.Sqrt(Radius * Radius - DistCnt2Vect * DistCnt2Vect)
			ptRes = PointAlongPlane(ptTmp, DirVect, D)
			CircleVectorIntersect = D 'ReturnDistanceInMeters(ptRes, ptTmp)
		Else
			ptRes = New ESRI.ArcGIS.Geometry.Point
		End If
	End Function

	'Public Function CircleCircleIntersect(ByRef PtCent1 As ESRI.ArcGIS.Geometry.IPoint, ByVal R1 As Double, ByRef PtCent2 As ESRI.ArcGIS.Geometry.IPoint, ByVal R2 As Double, ByVal TurnDir As Integer, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
	'    Dim Dist As Double
	'    Dim DistOver1 As Double
	'    Dim h As Double
	'    Dim A As Double
	'    Dim dX As Double
	'    Dim dY As Double

	'    Dist = ReturnDistanceInMeters(PtCent1, PtCent2)
	'    ptRes = New ESRI.ArcGIS.Geometry.Point

	'    If (Dist <= R1 + R2) And (Dist > 0.0) Then
	'        DistOver1 = 1.0 / Dist
	'        A = 0.5 * DistOver1 * (R1 * R1 + Dist * Dist - R2 * R2)
	'        h = System.Math.Sqrt(R1 * R1 - A * A)

	'        dX = DistOver1 * (PtCent2.X - PtCent1.X)
	'        dY = DistOver1 * (PtCent2.Y - PtCent1.Y)

	'        ptRes.X = PtCent1.X + TurnDir * h * dY + A * dX
	'        ptRes.Y = PtCent1.Y + TurnDir * h * dX - A * dY
	'        CircleCircleIntersect = h
	'    Else
	'        CircleCircleIntersect = -1.0
	'    End If
	'End Function

	'Function FindCommonTochCircle(ByRef pPolygon As ESRI.ArcGIS.Geometry.IPolygon, ByRef fAxis As Double, ByRef fTuchDist As Double, ByRef iTurnDir As Integer, ByRef pResPt As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.Point
	'    Dim pProximityoperator As ESRI.ArcGIS.Geometry.IProximityOperator
	'    Dim pConstructPoint As ESRI.ArcGIS.Geometry.IConstructPoint
	'    Dim fDist As Double

	'    Dim fdX As Double
	'    Dim fdY As Double
	'    Dim pTuchPt As ESRI.ArcGIS.Geometry.IPoint
	'    Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint

	'    Dim I As Integer
	'    Dim J As Integer

	'    pProximityoperator = pPolygon
	'    pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)

	'    fdX = pTuchPt.X - pResPt.X
	'    fdY = pTuchPt.Y - pResPt.Y
	'    fDist = System.Math.Sqrt(fdX * fdX + fdY * fdY)

	'    I = 0
	'    J = 0

	'    Do While System.Math.Abs(fDist - fTuchDist) > 0.25
	'        If CircleVectorIntersect(pTuchPt, fTuchDist, pResPt, fAxis, pPtTmp) > 0.0 Then
	'            pResPt.PutCoords(pPtTmp.X, pPtTmp.Y)
	'            J = 0
	'            I = I + 1
	'        Else
	'            pConstructPoint = pPtTmp
	'pConstructPoint.ConstructAngleIntersection(pResPt, DegToRad(fAxis), pTuchPt, DegToRad(fAxis + 90.0))
	'            pResPt.PutCoords(pPtTmp.X, pPtTmp.Y)
	'            J = J + 1
	'        End If

	'        pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)
	'        'DrawPoint pTuchPt, 0
	'        'DrawPolygon CreatePrjCircle(pTuchPt, fTuchDist)

	'        If (I > 10) Or (J > 5) Then
	'            FindCommonTochCircle = pTuchPt
	'            Exit Function
	'        End If

	'        fdX = pTuchPt.X - pResPt.X
	'        fdY = pTuchPt.Y - pResPt.Y
	'        fDist = System.Math.Sqrt(fdX * fdX + fdY * fdY)
	'    Loop

	'    FindCommonTochCircle = pTuchPt
	'End Function

	Public Function SideDef(ByRef PtInLine As ESRI.ArcGIS.Geometry.IPoint, ByRef LineAngle As Double, ByRef PtOutLine As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim Angle12 As Double
		Dim dAngle As Double
		Dim AbsdAngle As Double
		Dim dX As Double
		Dim dY As Double

		dX = PtOutLine.X - PtInLine.X
		dY = PtOutLine.Y - PtInLine.Y
		If dX * dX + dY * dY = 0.0 Then Return 0

		Angle12 = RadToDeg(Math.Atan2(dY, dX))

		AbsdAngle = Modulus(System.Math.Abs(LineAngle - Angle12), 180.0)
		If (AbsdAngle = 0.0) Or (AbsdAngle = 180.0) Then Return 0

		dAngle = Modulus(LineAngle - Angle12)
		If (dAngle < 180.0) Then Return 1

		Return -1
	End Function

	Public Function SideFrom2Angle(ByVal Angle0 As Double, ByVal Angle1 As Double) As Integer
		Dim dAngle As Double

		dAngle = SubtractAngles(Angle0, Angle1)

		If (180.0 - dAngle < degEps) Or (dAngle < degEps) Then
			SideFrom2Angle = 0
			Exit Function
		End If

		dAngle = Modulus(Angle1 - Angle0)

		If (dAngle < 180.0) Then
			SideFrom2Angle = 1
		Else
			SideFrom2Angle = -1
		End If
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

	Sub CreateWindSpiral(ByRef pt As ESRI.ArcGIS.Geometry.IPoint, ByRef aztNom As Double, ByRef AztSt As Double, ByRef AztEnd As Double, ByRef r0 As Double, ByRef coef As Double, ByRef TurnDir As Integer, ByRef pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection)
		Dim dAlpha As Double
		Dim N As Integer
		Dim I As Integer
		Dim TurnAng As Double
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim azt0 As Double
		Dim R As Double
		Dim dPhi As Double
		Dim dPhi0 As Double
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint

		ptCnt = PointAlongPlane(pt, aztNom + 90.0 * TurnDir, r0)

		If SubtractAngles(aztNom, AztEnd) < degEps Then AztEnd = aztNom

		dPhi0 = (AztSt - aztNom) * TurnDir
		dPhi0 = Modulus(dPhi0, 360.0)

		If (dPhi0 < 0.001) Then
			dPhi0 = 0.0
		Else
			dPhi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, TurnDir)
		End If

		dPhi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, TurnDir)

		TurnAng = dPhi - dPhi0

		azt0 = aztNom + (dPhi0 - 90.0) * TurnDir
		azt0 = Modulus(azt0, 360.0)

		If (TurnAng < 0.0) Then Exit Sub

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

		ptCur = New ESRI.ArcGIS.Geometry.Point
		For I = 0 To N
			R = r0 + (dAlpha * coef * I) + dPhi0 * coef
			ptCur = PointAlongPlane(ptCnt, azt0 + (I * dAlpha) * TurnDir, R)
			'    DrawPoint ptCur, 233

			pPointCollection.AddPoint(ptCur)
		Next I
		'Dim pCurve As ICurve
		'Dim pPolyLine As IPointCollection
		'Set pPolyLine = New Polyline
		'
		'Set pPolyLine = pPointCollection
		'Set pCurve = pPolyLine
	End Sub

	Public Function RayPolylineIntersect(ByVal pPolyLine As ESRI.ArcGIS.Geometry.Polyline, ByVal RayPt As ESRI.ArcGIS.Geometry.Point, ByVal RayDir As Double, ByRef InterPt As ESRI.ArcGIS.Geometry.IPoint) As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim D As Double
		Dim dMin As Double
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pLine.FromPoint = RayPt
		dMin = 5000000.0
		pLine.ToPoint = PointAlongPlane(RayPt, RayDir, dMin)

		pTopo = pPolyLine
		pPoints = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
		N = pPoints.PointCount

		RayPolylineIntersect = N > 0

		If N = 0 Then Exit Function

		If N = 1 Then
			InterPt = pPoints.Point(0)
		Else
			For I = 0 To N - 1
				D = ReturnDistanceInMeters(RayPt, pPoints.Point(I))
				If D < dMin Then
					dMin = D
					InterPt = pPoints.Point(I)
				End If
			Next
		End If
	End Function

	Function AngleInSector(ByVal angle As Double, ByVal X As Double, ByVal Y As Double) As Boolean
		Dim Sector As Double
		Dim Sub1 As Double
		Dim Sub2 As Double

		Sector = Modulus(Y - X)
		Sub1 = Modulus(angle - X)
		Sub2 = Modulus(Y - angle)

		AngleInSector = Not (Sub1 + Sub2 > Sector + degEps)
	End Function

	Public Function DrawPointWithText(ByRef pPoint As ESRI.ArcGIS.Geometry.Point, ByRef sText As String, Optional ByRef Color As Integer = -1, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
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
		DrawPointWithText = pCommonElement

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pCommonElement, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	Public Function DrawPoint(ByRef pPoint As ESRI.ArcGIS.Geometry.Point, Optional ByRef Color As Integer = -1, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
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

		DrawPoint = pElementofpPoint

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementofpPoint, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	Function DrawLine(ByRef pLine As ESRI.ArcGIS.Geometry.Line, Optional ByRef Color As Integer = -1, Optional ByRef Width As Double = 1.0, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
		Dim pLineElement As ESRI.ArcGIS.Carto.ILineElement
		Dim pElementOfpLine As ESRI.ArcGIS.Carto.IElement
		Dim pLineSym As ESRI.ArcGIS.Display.ISimpleLineSymbol
		Dim pRGB As ESRI.ArcGIS.Display.IRgbColor

		pLineElement = New ESRI.ArcGIS.Carto.LineElement
		pElementOfpLine = pLineElement

		pElementOfpLine.Geometry = pLine

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

		pLineElement.Symbol = pLineSym
		DrawLine = pElementOfpLine

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementOfpLine, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	Public Function DrawPolyLine(ByRef pLine As ESRI.ArcGIS.Geometry.Polyline, Optional ByRef Color As Integer = -1, Optional ByRef Width As Double = 1.0, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement

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
		pElementOfpLine.Geometry = pLine

		pLineElement.Symbol = pLineSym
		DrawPolyLine = pElementOfpLine

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementOfpLine, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	Public Function DrawPolygon(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon, Optional ByRef Color As Integer = -1, Optional ByRef drawFlg As Boolean = True) As ESRI.ArcGIS.Carto.IElement
		Dim pRGB As ESRI.ArcGIS.Display.IRgbColor
		Dim pFillSym As ESRI.ArcGIS.Display.ISimpleFillSymbol
		Dim pFillShpElement As ESRI.ArcGIS.Carto.IFillShapeElement
		Dim pLineSimbol As ESRI.ArcGIS.Display.ILineSymbol

		Dim pElementofPoly As ESRI.ArcGIS.Carto.IElement

		pRGB = New ESRI.ArcGIS.Display.RgbColor
		pFillSym = New ESRI.ArcGIS.Display.SimpleFillSymbol
		pFillShpElement = New ESRI.ArcGIS.Carto.PolygonElement

		pElementofPoly = pFillShpElement
		pElementofPoly.Geometry = pPolygon

		If Color <> -1 Then
			pRGB.RGB = Color
		Else
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
		End If

		pFillSym.Color = pRGB
		pFillSym.Style = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull 'esriSFSDiagonalCross

		pLineSimbol = New ESRI.ArcGIS.Display.SimpleLineSymbol
		pLineSimbol.Color = pRGB
		pLineSimbol.Width = 1
		pFillSym.Outline = pLineSimbol

		pFillShpElement.Symbol = pFillSym
		DrawPolygon = pElementofPoly

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementofPoly, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If
	End Function

	'Function DrawSectorPrj(PtCnt As IPoint, ptFrom As IPoint,  ptTo As IPoint, ClWise As Long) As IPointCollection
	Function DrawSectorPrj(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef R As Double, ByRef stDir As Double, ByRef endDir As Double, ByRef ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
		ptFrom = PointAlongPlane(ptCnt, stDir, R)
		ptTo = PointAlongPlane(ptCnt, endDir, R)
		DrawSectorPrj = CreateArcPrj(ptCnt, ptFrom, ptTo, ClWise)
		DrawSectorPrj.AddPoint(ptCnt)
		DrawSectorPrj.AddPoint(ptCnt, 0)
	End Function

	Public Sub ClearScr()
		Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
		Dim pElement As ESRI.ArcGIS.Carto.IElement
		Dim pActiveView As ESRI.ArcGIS.Carto.IActiveView
		pActiveView = GetActiveView()
		pGraphics = pActiveView.GraphicsContainer

		On Error GoTo Err_Renamed

		pGraphics.Reset()
		pElement = pGraphics.Next

		While Not pElement Is Nothing
			If pElement.Locked Then pGraphics.DeleteElement(pElement)

			'    If (pElement.Geometry.GeometryType = esriGeometryPoint) Or _
			''        (pElement.Geometry.GeometryType = esriGeometryPolygon) Or _
			''        (pElement.Geometry.GeometryType = esriGeometryPolyline) Then
			'        pGraphics.DeleteElement pElement
			'    End If
			pElement = pGraphics.Next
		End While

		pActiveView.Refresh()
		Return

Err_Renamed:
		On Error GoTo 0
	End Sub

	Public Sub GetIntData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As Integer, ByVal size As Integer)
		Dim I As Integer
		Dim E As Double

		E = 1
		Vara = 0

		For I = 0 To size - 1
			Vara = Data(Index + I) * E + Vara
			E = E * 256
		Next I

		Index = Index + size
	End Sub

	Public Sub GetStrData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As String, ByVal size As Integer)
		Dim I As Integer
		Vara = ""

		For I = 0 To size - 1
			Vara = Vara & Chr(Data(Index + I))
		Next I

		Index = Index + size
	End Sub

	Public Sub GetData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As Object, ByVal size As Integer)
		Dim I As Integer
		ReDim Vara(size)
		For I = 0 To size - 1

			Vara(I) = Data(Index + I)
		Next I
		Index = Index + size
	End Sub

	Public Sub GetDoubleData(ByRef Data() As Byte, ByRef Index As Integer, ByRef Vara As Double, ByVal size As Integer)
		Dim I As Integer
		Dim Sign As Integer

		Dim mantissa As Double
		Dim exponent As Integer

		mantissa = 0
		exponent = 0

		For I = 0 To size - 3
			mantissa = Data(Index + I) + mantissa / 256.0
		Next I

		mantissa = CShort(CShort(CShort(Data(Index + size - 2) And 15) + 16) + mantissa / 256.0) / CShort(16.0)

		exponent = CShort(Data(Index + size - 2) And 240) / CShort(16.0)
		exponent = Data(Index + size - 1) * 16.0 + exponent

		If mantissa = 1 And exponent = 0 Then
			Vara = 0.0
			Index = Index + size
			Exit Sub
		End If

		Sign = exponent And 2048
		exponent = CShort(exponent And 2047) - 1023
		If exponent > 0 Then
			For I = 1 To exponent
				mantissa = mantissa * 2.0
			Next I
		ElseIf exponent < 0 Then
			For I = -1 To exponent Step -1
				mantissa = mantissa / 2.0
			Next I
		End If

		If Sign <> 0 Then mantissa = -mantissa
		Vara = mantissa
		Index = Index + size
	End Sub

	Function DegToRad(ByVal X As Double) As Double
		DegToRad = X * DegToRadValue
	End Function

	Function RadToDeg(ByVal X As Double) As Double
		RadToDeg = X * RadToDegValue
	End Function

	Function OpenTableFromFile(ByRef pTable As ESRI.ArcGIS.Geodatabase.ITable, ByRef sFolderName As String, ByRef sFileName As String) As Boolean
		On Error GoTo EH

		Dim pFact As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory
		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace
		Dim pFeatWs As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace

		Dim plDocument As ESRI.ArcGIS.Framework.IDocument
		Dim sPath As String
		Dim L As Integer
		Dim Pos As Integer

		OpenTableFromFile = True
		sPath = sFolderName

		pFact = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory
		pWorkspace = pFact.OpenFromFile(sPath, GetApplicationHWnd())
		pFeatWs = pWorkspace
		pTable = pFeatWs.OpenTable(sFileName)

		Exit Function

EH:
		OpenTableFromFile = False
		ErrorStr = Err.Number & "  " & Err.Description
		MsgBox(Err.Number & "  " & Err.Description)
	End Function
	'
	Public Function Point2LineDistancePrj(ByVal pt As ESRI.ArcGIS.Geometry.IPoint, ByVal ptLine As ESRI.ArcGIS.Geometry.IPoint, ByVal Azt As Double) As Double
		Dim cosx As Double
		Dim siny As Double
		Azt = DegToRad(Azt)

		cosx = System.Math.Cos(Azt)
		siny = System.Math.Sin(Azt)
		Point2LineDistancePrj = System.Math.Abs((pt.Y - ptLine.Y) * cosx - (pt.X - ptLine.X) * siny)
	End Function

	Public Function LineVectIntersect(ByRef pPt1 As ESRI.ArcGIS.Geometry.IPoint, ByRef pPt2 As ESRI.ArcGIS.Geometry.IPoint, ByRef ptVect As ESRI.ArcGIS.Geometry.IPoint, ByRef Azt As Double, ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim Az As Double
		Dim SinAz As Double
		Dim CosAz As Double
		Dim UaDenom As Double
		Dim UaNumer As Double
		Dim Ua As Double

		Az = DegToRad(Azt)
		SinAz = System.Math.Sin(Az)
		CosAz = System.Math.Cos(Az)

		ptRes = New ESRI.ArcGIS.Geometry.Point

		UaDenom = SinAz * (pPt2.X - pPt1.X) - CosAz * (pPt2.Y - pPt1.Y)
		If UaDenom = 0.0 Then
			LineVectIntersect = -2
			Exit Function
		End If

		UaNumer = CosAz * (pPt1.Y - ptVect.Y) - SinAz * (pPt1.X - ptVect.X)

		Ua = UaNumer / UaDenom
		If Ua < 0.0 Then
			LineVectIntersect = -1
		ElseIf Ua > 1.0 Then
			LineVectIntersect = 1
		Else
			LineVectIntersect = 0
		End If

		ptRes.PutCoords(pPt1.X + Ua * (pPt2.X - pPt1.X), pPt1.Y + Ua * (pPt2.Y - pPt1.Y))
	End Function

	Public Function FixToTouchSpiral(ByRef ptBase As ESRI.ArcGIS.Geometry.IPoint, ByRef coef0 As Double, ByRef TurnR As Double, ByRef TurnDir As Integer, ByRef Theta As Double, ByRef FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef DepCourse As Double) As Double
		Dim R As Double
		Dim x1 As Double
		Dim y1 As Double
		Dim Theta0 As Double
		Dim Theta1 As Double
		Dim Theta1New As Double
		Dim fTmp1 As Double
		Dim fTmp2 As Double
		Dim coef As Double
		Dim Dist As Double
		Dim FixTheta As Double
		Dim I As Integer
		Dim PtCntSpiral As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
		Dim dTheta As Double
		Dim CntTheta As Double

		FixToTouchSpiral = -1000

		coef = RadToDeg(coef0)
		Theta0 = Modulus(90.0 * TurnDir + DepCourse + 180.0, 360.0)
		PtCntSpiral = PointAlongPlane(ptBase, DepCourse + 90.0 * TurnDir, TurnR)
		Dist = ReturnDistanceInMeters(PtCntSpiral, FixPnt)
		FixTheta = ReturnAngleInDegrees(PtCntSpiral, FixPnt)
		dTheta = Modulus((FixTheta - Theta0) * TurnDir, 360.0)
		R = TurnR + dTheta * coef0
		If Dist < R Then
			Exit Function
		End If

		x1 = FixPnt.X - PtCntSpiral.X
		y1 = FixPnt.Y - PtCntSpiral.Y
		CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, TurnDir)
		CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
		'===============================Variant Firdowsy ==================================
		Dim F As Double
		Dim F1 As Double
		Dim SinT As Double
		Dim CosT As Double

		Theta1 = CntTheta
		For I = 0 To 20
			dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
			SinT = System.Math.Sin(DegToRad(Theta1))
			CosT = System.Math.Cos(DegToRad(Theta1))
			R = TurnR + dTheta * coef0
			F = R * R - (y1 * R + x1 * coef * TurnDir) * SinT - (x1 * R - y1 * coef * TurnDir) * CosT
			F1 = 2 * R * coef * TurnDir - (y1 * R + 2 * x1 * coef * TurnDir) * CosT + (x1 * R - 2 * y1 * coef * TurnDir) * SinT
			Theta1New = Theta1 - RadToDeg(F / F1)

			fTmp1 = SubtractAngles(Theta1New, Theta1)
			If fTmp1 < 0.0001 Then
				Theta1 = Theta1New
				Exit For
			End If
			Theta1 = Theta1New
		Next I

		dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
		R = TurnR + dTheta * coef0

		ptOut = PointAlongPlane(PtCntSpiral, Theta1, R)
		fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
		CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
		CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
		fTmp2 = SubtractAngles(CntTheta, Theta1)

		If fTmp2 < 0.0001 Then
			FixToTouchSpiral = fTmp1
			Exit Function
		End If

#If test Then
        Exit Function
        '=============================11==================================
        Dim Theta11 As Double
        Dim Theta12 As Double
        Dim Theta21 As Double
        Dim Theta22 As Double
        Dim Theta2New As Double
        Dim SinCoef As Double
        Dim CosCoef As Double
        Dim A As Double
        Dim B As Double
        Dim C As Double
        Dim D As Double
        Dim sin1 As Double
        Dim sin2 As Double

        Dim dThetaNew As Double
        Dim SolFlag11 As Boolean
        Dim SolFlag12 As Boolean
        Dim SolFlag21 As Boolean
        Dim SolFlag22 As Boolean

        Theta1 = CntTheta
        SolFlag11 = False

        For I = 0 To 20
            dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
            'SinCoef*SinX+CosCoef*CosX = 1
            R = TurnR + dTheta * coef0
            SinCoef = (y1 + TurnDir * coef * x1 / R) / R
            CosCoef = (x1 - TurnDir * coef * y1 / R) / R
            'a*x2 + b*x + c = 0
            A = SinCoef * SinCoef + CosCoef * CosCoef
            B = -2 * SinCoef
            C = 1 - CosCoef * CosCoef
            D = B * B - 4.0 * A * C
            If D < 0.0 Then
                Theta1New = -D * System.Math.Sign(-B * A) * 90.0
            Else
                sin1 = (-B + System.Math.Sqrt(D)) / (2.0 * A)
                Theta1New = RadToDeg(ArcSin(sin1))
            End If

            fTmp1 = SubtractAngles(Theta1New, Theta1)
            If fTmp1 < 0.0001 Then
                SolFlag11 = True
                Theta11 = Theta1
                Exit For
            End If
            Theta1 = Theta1New
        Next I
        '=============================12==================================
        Theta1 = CntTheta
        SolFlag12 = False

        For I = 0 To 20
            dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
            'SinCoef*SinX+CosCoef*CosX = 1
            R = TurnR + dTheta * coef0
            SinCoef = (y1 + TurnDir * coef * x1 / R) / R
            CosCoef = (x1 - TurnDir * coef * y1 / R) / R
            'a*x2 + b*x + c = 0
            A = SinCoef * SinCoef + CosCoef * CosCoef
            B = -2 * SinCoef
            C = 1 - CosCoef * CosCoef
            D = B * B - 4.0 * A * C
            If D < 0.0 Then
                Theta1New = 180.0 + D * System.Math.Sign(-B * A) * 90.0
            Else
                sin1 = (-B + System.Math.Sqrt(D)) / (2.0 * A)
                Theta1New = 180.0 - RadToDeg(ArcSin(sin1))
            End If

            fTmp1 = SubtractAngles(Theta1New, Theta1)
            If fTmp1 < 0.0001 Then
                SolFlag12 = True
                Theta12 = Theta1
                Exit For
            End If
            Theta1 = Theta1New
        Next I
        '=============================21==================================
        Theta1 = CntTheta
        SolFlag21 = False

        For I = 0 To 20
            dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
            'SinCoef*SinX+CosCoef*CosX = 1
            R = TurnR + dTheta * coef0
            SinCoef = (y1 + TurnDir * coef * x1 / R) / R
            CosCoef = (x1 - TurnDir * coef * y1 / R) / R
            'a*x2 + b*x + c = 0
            A = SinCoef * SinCoef + CosCoef * CosCoef
            B = -2 * SinCoef
            C = 1 - CosCoef * CosCoef
            D = B * B - 4.0 * A * C
            If D < 0.0 Then
                Theta1New = -D * System.Math.Sign(-B * A) * 90.0
            Else
                sin1 = (-B - System.Math.Sqrt(D)) / (2.0 * A)
                Theta1New = RadToDeg(ArcSin(sin1))
            End If

            fTmp1 = SubtractAngles(Theta1New, Theta1)
            If fTmp1 < 0.0001 Then
                SolFlag21 = True
                Theta21 = Theta1
                Exit For
            End If
            Theta1 = Theta1New
        Next I
        '=============================22==================================
        Theta1 = CntTheta + 180.0
        SolFlag22 = False

        For I = 0 To 20
            dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
            'SinCoef*SinX+CosCoef*CosX = 1
            R = TurnR + dTheta * coef0
            SinCoef = (y1 + TurnDir * coef * x1 / R) / R
            CosCoef = (x1 - TurnDir * coef * y1 / R) / R
            'a*x2 + b*x + c = 0
            A = SinCoef * SinCoef + CosCoef * CosCoef
            B = -2 * SinCoef
            C = 1 - CosCoef * CosCoef
            D = B * B - 4.0 * A * C
            If D < 0.0 Then
                Theta1New = 180.0 + D * System.Math.Sign(-B * A) * 90.0
            Else
                sin1 = (-B - System.Math.Sqrt(D)) / (2.0 * A)
                Theta1New = 180.0 - RadToDeg(ArcSin(sin1))
            End If

            fTmp1 = SubtractAngles(Theta1New, Theta1)
            If fTmp1 < 0.0001 Then
                SolFlag22 = True
                Theta22 = Theta1
                Exit For
            End If
            Theta1 = Theta1New
        Next I
        '================================11=====================================
        If SolFlag11 Then
            dTheta = Modulus((Theta11 - Theta0) * TurnDir, 360.0)
            R = TurnR + dTheta * coef0
            ptOut = PointAlongPlane(PtCntSpiral, Theta11, R)
            fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
            CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
            CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
            fTmp2 = SubtractAngles(CntTheta, Theta11)
            If fTmp2 < 0.0001 Then
                FixToTouchSpiral = fTmp1
                Exit Function
            End If
        End If
        '================================12=====================================
        If SolFlag12 Then
            dTheta = Modulus((Theta12 - Theta0) * TurnDir, 360.0)
            R = TurnR + dTheta * coef0
            ptOut = PointAlongPlane(PtCntSpiral, Theta12, R)
            fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
            CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
            CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
            fTmp2 = SubtractAngles(CntTheta, Theta12)
            If fTmp2 < 0.0001 Then
                FixToTouchSpiral = fTmp1
                Exit Function
            End If
        End If
        '================================21=====================================
        If SolFlag21 Then
            dTheta = Modulus((Theta21 - Theta0) * TurnDir, 360.0)
            R = TurnR + dTheta * coef0
            ptOut = PointAlongPlane(PtCntSpiral, Theta21, R)
            fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
            CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
            CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
            fTmp2 = SubtractAngles(CntTheta, Theta21)
            If fTmp2 < 0.0001 Then
                FixToTouchSpiral = fTmp1
                Exit Function
            End If
        End If
        '================================22=====================================
        If SolFlag22 Then
            dTheta = Modulus((Theta22 - Theta0) * TurnDir, 360.0)
            R = TurnR + dTheta * coef0
            ptOut = PointAlongPlane(PtCntSpiral, Theta22, R)
            fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
            CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
            CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)
            fTmp2 = SubtractAngles(CntTheta, Theta22)
            If fTmp2 < 0.0001 Then
                FixToTouchSpiral = fTmp1
                Exit Function
            End If
        End If
#End If
	End Function

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

	Public Function PolygonIntersection(ByRef pPoly1 As ESRI.ArcGIS.Geometry.Polygon, ByRef pPoly2 As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTmpPoly0 As ESRI.ArcGIS.Geometry.Polygon
		Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.Polygon

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

	Function RemoveFars(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon, ByRef pPoint As ESRI.ArcGIS.Geometry.Point) As ESRI.ArcGIS.Geometry.Polygon
		Dim Geocollect As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim lCollect As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim OutDist As Double
		Dim tmpDist As Double
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim I As Integer
		Dim N As Integer

		pClone = pPolygon
		RemoveFars = pClone.Clone
		Geocollect = RemoveFars
		N = Geocollect.GeometryCount
		lCollect = New ESRI.ArcGIS.Geometry.Polygon

		If N > 1 Then
			pProxi = pPoint
			OutDist = 20000000000.0

			For I = 0 To N - 1
				lCollect.AddGeometry(Geocollect.Geometry(I))

				tmpDist = pProxi.ReturnDistance(lCollect)
				If OutDist > tmpDist Then
					OutDist = tmpDist
				End If
				lCollect.RemoveGeometries(0, 1)
			Next I

			I = 0
			While I < N
				lCollect.AddGeometry(Geocollect.Geometry(I))
				tmpDist = pProxi.ReturnDistance(lCollect)
				If OutDist < tmpDist Then
					Geocollect.RemoveGeometries(I, 1)
					N = N - 1
				Else
					I = I + 1
				End If
				lCollect.RemoveGeometries(0, 1)
			End While
		End If
	End Function

	Function RemoveSmalls(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim Geocollect As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim OutArea As Double
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pArea As ESRI.ArcGIS.Geometry.IArea
		Dim I As Integer
		Dim N As Integer

		pClone = pPolygon
		RemoveSmalls = pClone.Clone
		Geocollect = RemoveSmalls
		N = Geocollect.GeometryCount

		If N > 1 Then
			OutArea = 0.0

			For I = 0 To N - 1
				pArea = Geocollect.Geometry(I)
				If pArea.Area > OutArea Then
					OutArea = pArea.Area
				End If
			Next I

			I = 0
			While I < N
				pArea = Geocollect.Geometry(I)
				If pArea.Area < OutArea Then
					Geocollect.RemoveGeometries(I, 1)
					N = N - 1
				Else
					I = I + 1
				End If
			End While
		End If
	End Function

	Function RemoveHoles(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim NewPolygon As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pInteriorRing As ESRI.ArcGIS.Geometry.IRing
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim I As Integer

		pClone = pPolygon
		'Set NewPolygon = pClone.Clone

		RemoveHoles = pClone.Clone
		NewPolygon = RemoveHoles

		I = 0
		While I < NewPolygon.GeometryCount
			pInteriorRing = NewPolygon.Geometry(I)
			If Not pInteriorRing.IsExterior Then
				NewPolygon.RemoveGeometries(I, 1)
			Else
				I = I + 1
			End If
		End While

		pTopoOper = NewPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
		'Set RemoveHoles = NewPolygon
	End Function

	Function RemoveAgnails(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer

		Dim N As Integer

		Dim dl As Double
		Dim dX0 As Double
		Dim dY0 As Double
		Dim dX1 As Double
		Dim dY1 As Double

		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPGone As ESRI.ArcGIS.Geometry.IPolygon2
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = pPolygon
		pPoly = pClone.Clone

		pPGone = pPoly
		pPGone.Close()

		N = pPoly.PointCount - 1

		If N <= 3 Then
			RemoveAgnails = pPoly
			Exit Function
		End If

		pPoly.RemovePoints(N, 1)

		J = 0
		Do While J < N
			If N < 4 Then Exit Do

			K = (J + 1) Mod N
			L = (J + 2) Mod N

			dX0 = pPoly.Point(K).X - pPoly.Point(J).X
			dY0 = pPoly.Point(K).Y - pPoly.Point(J).Y

			dX1 = pPoly.Point(L).X - pPoly.Point(K).X
			dY1 = pPoly.Point(L).Y - pPoly.Point(K).Y

			dl = dX1 * dX1 + dY1 * dY1

			If dl < 0.00001 Then
				pPoly.RemovePoints(K, 1)
				N = N - 1
				If J >= N Then J = N - 1
			ElseIf (dY0 <> 0.0) Then
				If dY1 <> 0.0 Then
					If System.Math.Abs(dX0 / dY0 - dX1 / dY1) < 0.0001 Then
						pPoly.RemovePoints(K, 1)
						N = N - 1
						J = (J - 2) Mod N
						If J < 0 Then J = 0 'J = J + N
					Else
						J = J + 1
					End If
				Else
					J = J + 1
				End If
			ElseIf (dX0 <> 0.0) Then
				If dX1 <> 0.0 Then
					If System.Math.Abs(dY0 / dX0 - dY1 / dX1) < 0.0001 Then
						pPoly.RemovePoints(K, 1)
						N = N - 1
						J = (J - 2) Mod N
						If J < 0 Then J = 0 'J = J + N
					Else
						J = J + 1
					End If
				Else
					J = J + 1
				End If
			Else
				J = J + 1
			End If
		Loop

		pPGone = pPoly
		pPGone.Close()

		pTopo = pPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		RemoveAgnails = pPoly
	End Function

	Public Function ReArrangePolygon(ByRef pPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef PtDerL As ESRI.ArcGIS.Geometry.IPoint, ByRef CLDir As Double, Optional ByRef bFlag As Boolean = False) As ESRI.ArcGIS.Geometry.IPointCollection
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
		Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection

		pPoly = New ESRI.ArcGIS.Geometry.Polyline
		pPoly.AddPointCollection(pPolygon)

		pPoly.RemovePoints(0, 1)

		N = pPoly.PointCount

		dm = Point2LineDistancePrj(pPoly.Point(0), PtDerL, CLDir + 90.0) * SideDef(PtDerL, CLDir, pPoly.Point(0))

		iStart = -1
		If dm < 0 Then iStart = 0

		For I = 0 To N - 1
			dl = Point2LineDistancePrj(pPoly.Point(I), PtDerL, CLDir + 90.0) * SideDef(PtDerL, CLDir, pPoly.Point(I))
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
			J = (I + 1) Mod (N - 1)
			dX1 = pPoly.Point(J).X - pPoly.Point(I).X
			dY1 = pPoly.Point(J).Y - pPoly.Point(I).Y
			dl = ReturnDistanceInMeters(pPoly.Point(J), pPoly.Point(I))

			If dl < distEps Then
				pPoly.RemovePoints(I, 1)
				N = N - 1
				J = (I + 1) Mod N
				If I <= iStart Then
					iStart = iStart - 1
				End If
				dX1 = dX0
				dY1 = dY0
			ElseIf (dY0 <> 0.0) And (I <> iStart) Then
				If dY1 <> 0.0 Then
					If System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001 Then
						pPoly.RemovePoints(I, 1)
						N = N - 1
						J = (I + 1) Mod N
						If I <= iStart Then
							iStart = iStart - 1
						End If
						dX1 = dX0
						dY1 = dY0
					Else
						I = I + 1
					End If
				Else
					I = I + 1
				End If
			ElseIf (dX0 <> 0.0) And (I <> iStart) Then
				If dX1 <> 0.0 Then
					If System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001 Then
						pPoly.RemovePoints(I, 1)
						N = N - 1
						J = (I + 1) Mod N
						If I <= iStart Then
							iStart = iStart - 1
						End If
						dX1 = dX0
						dY1 = dY0
					Else
						I = I + 1
					End If
				Else
					I = I + 1
				End If
			Else
				I = I + 1
			End If
			dX0 = dX1
			dY0 = dY1
		Loop

		N = pPoly.PointCount
		ReArrangePolygon = New ESRI.ArcGIS.Geometry.Polygon

		For I = N - 1 To 0 Step -1
			J = ((I + iStart) Mod N)
			ReArrangePolygon.AddPoint(pPoly.Point(J))
		Next

		'DrawPolygon ReArrangePolygon, 255

		'Set pPoly = New Polyline
		'pPoly.re
		'pPoly.ReverseOrientation
	End Function

	Function CalcTrajectoryFromMultiPoint(ByRef MultiPoint As ESRI.ArcGIS.Geometry.IPointCollection) As ESRI.ArcGIS.Geometry.IPointCollection

		'Dim ii As Long
		'
		'For ii = 0 To MultiPoint.PointCount - 1
		'    MessageBox FrmManevreN.hWnd, "Drawing point N:" + CStr(ii), "", 0
		'    DrawPoint MultiPoint.Point(ii)
		'Next ii

		CalcTrajectoryFromMultiPoint = CalcTrajectoryFromMultiPoint_ByPart(MultiPoint)
		Exit Function
#If test Then
        Dim ptConstr As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim TmpLine As ESRI.ArcGIS.Geometry.IPolyline

        Dim FromPt As ESRI.ArcGIS.Geometry.IPoint
        Dim CntPt As ESRI.ArcGIS.Geometry.IPoint
        Dim ToPt As ESRI.ArcGIS.Geometry.IPoint

        Dim CenAng As Double
        Dim fTmp As Double
        Dim fE As Double

        Dim side As Integer
        Dim I As Integer
        Dim N As Integer

        CntPt = New ESRI.ArcGIS.Geometry.Point
        ptConstr = CntPt
        CalcTrajectoryFromMultiPoint = New ESRI.ArcGIS.Geometry.Polyline
        fE = DegToRadValue * 0.5

        N = MultiPoint.PointCount - 2
        CalcTrajectoryFromMultiPoint.AddPoint(MultiPoint.Point(0))

        For I = 0 To N
            FromPt = MultiPoint.Point(I)
            ToPt = MultiPoint.Point(I + 1)
            fTmp = DegToRadValue * (FromPt.M - ToPt.M)

            If (System.Math.Abs(System.Math.Sin(fTmp)) <= fE) And (System.Math.Cos(fTmp) > 0.0) Then
                CalcTrajectoryFromMultiPoint.AddPoint(ToPt)
            Else
                If System.Math.Abs(System.Math.Sin(fTmp)) > fE Then
                    ptConstr.ConstructAngleIntersection(FromPt, DegToRadValue * (Modulus(FromPt.M + 90.0, 360.0)), ToPt, DegToRadValue * (Modulus(ToPt.M + 90.0, 360.0)))
                Else
                    CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y))
                End If
                '        DrawPoint CntPt, RGB(0, 0, 255)
                side = SideDef(FromPt, (FromPt.M), ToPt)
                CalcTrajectoryFromMultiPoint.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -side))
            End If
            '    DrawPoint CntPt, RGB(0, 255, 0)
        Next
#End If
	End Function

	Function CalcTrajectoryFromMultiPoint_ByPart(ByRef MultiPoint As ESRI.ArcGIS.Geometry.IPointCollection) As ESRI.ArcGIS.Geometry.IPolyline
		Dim ptConstr As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pGeometryCollection As ESRI.ArcGIS.Geometry.IGeometryCollection

		Dim FromPt As ESRI.ArcGIS.Geometry.IPoint
		Dim CntPt As ESRI.ArcGIS.Geometry.IPoint
		Dim ToPt As ESRI.ArcGIS.Geometry.IPoint

		Dim fTmp As Double
		Dim fE As Double

		Dim side As Integer
		Dim I As Integer
		Dim N As Integer

		CntPt = New ESRI.ArcGIS.Geometry.Point
		ptConstr = CntPt



		fE = DegToRadValue * 0.5

		N = MultiPoint.PointCount - 2
		pGeometryCollection = New ESRI.ArcGIS.Geometry.Polyline

		Dim pPath As ESRI.ArcGIS.Geometry.ISegmentCollection
		Dim pPolyL As ESRI.ArcGIS.Geometry.IGeometryCollection

		pPolyL = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To N
			pPath = New ESRI.ArcGIS.Geometry.Path
			FromPt = MultiPoint.Point(I)
			ToPt = MultiPoint.Point(I + 1)
			fTmp = DegToRadValue * (FromPt.M - ToPt.M)

			If (System.Math.Abs(System.Math.Sin(fTmp)) <= fE) And (System.Math.Cos(fTmp) > 0.0) Then
				pPath.AddSegment(GetLine(FromPt, ToPt))
			Else
				If System.Math.Abs(System.Math.Sin(fTmp)) > fE Then
					ptConstr.ConstructAngleIntersection(FromPt, DegToRadValue * (Modulus(FromPt.M + 90.0, 360.0)), ToPt, DegToRadValue * (Modulus(ToPt.M + 90.0, 360.0)))
				Else
					CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y))
				End If
				side = SideDef(FromPt, (FromPt.M), ToPt)
				'pPath.AddSegment CreateArcPrj_ByArc(CntPt, FromPt, ToPt, side)
				pPath = CreateArcPrj2(CntPt, FromPt, ToPt, -side)
			End If
			pPolyL.AddGeometry(pPath)
		Next

		CalcTrajectoryFromMultiPoint_ByPart = pPolyL
	End Function

	Public Function GetLine(ByRef ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByRef ptTo As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.ILine
		Dim pLine As ESRI.ArcGIS.Geometry.ILine
		pLine = New ESRI.ArcGIS.Geometry.Line
		pLine.PutCoords(ptFrom, ptTo)

		GetLine = pLine
	End Function

	Public Function CreateBasePoints(ByRef pPolygone As ESRI.ArcGIS.Geometry.IPointCollection, ByRef K1K1 As ESRI.ArcGIS.Geometry.IPolyline, ByRef lDepDir As Double, ByRef lTurnDir As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim bFlg As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim side As Integer

		bFlg = False
		N = pPolygone.PointCount
		tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
		CreateBasePoints = New ESRI.ArcGIS.Geometry.Polygon

		If lTurnDir > 0 Then
			For I = 0 To N - 1
				side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
				If (side < 0) Then
					If bFlg Then
						CreateBasePoints.AddPoint(pPolygone.Point(I))
					Else
						tmpPoly.AddPoint(pPolygone.Point(I))
					End If
				ElseIf Not bFlg Then
					bFlg = True
					CreateBasePoints.AddPoint(K1K1.FromPoint)
					CreateBasePoints.AddPoint(K1K1.ToPoint)
				End If
			Next
		Else
			For I = N - 1 To 0 Step -1
				side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
				If (side < 0) Then
					If bFlg Then
						CreateBasePoints.AddPoint(pPolygone.Point(I))
					Else
						tmpPoly.AddPoint(pPolygone.Point(I))
					End If
				ElseIf Not bFlg Then
					bFlg = True
					CreateBasePoints.AddPoint(K1K1.ToPoint)
					CreateBasePoints.AddPoint(K1K1.FromPoint)
				End If
			Next
		End If

		CreateBasePoints.AddPointCollection(tmpPoly)
	End Function

	Public Function TurnToFixPrj(ByRef PtSt As ESRI.ArcGIS.Geometry.IPoint, ByRef TurnR As Double, ByRef TurnDir As Integer, ByRef FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef OutDir As Double) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint

		Dim DeltaAngle As Double
		Dim DirFx2Cnt As Double
		Dim DistFx2Cnt As Double
		Dim dirCur As Double

		dirCur = PtSt.M

		TurnToFixPrj = New ESRI.ArcGIS.Geometry.Multipoint
		ptCnt = PointAlongPlane(PtSt, dirCur + 90.0 * TurnDir, TurnR)

		DistFx2Cnt = ReturnDistanceInMeters(ptCnt, FixPnt)

		If DistFx2Cnt < TurnR Then
			TurnR = DistFx2Cnt
			Exit Function
		End If

		DirFx2Cnt = ReturnAngleInDegrees(ptCnt, FixPnt)

		OutDir = DirFx2Cnt

		DeltaAngle = -RadToDeg(Math.Acos(TurnR / DistFx2Cnt)) * TurnDir

		pPt1 = PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR)
		pPt1.M = ReturnAngleInDegrees(pPt1, FixPnt)

		TurnToFixPrj.AddPoint(PtSt)
		TurnToFixPrj.AddPoint(pPt1)
	End Function

	Public Function ReturnPolygonPartAsPolyline(ByRef pPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef PtDerL As ESRI.ArcGIS.Geometry.IPoint, ByRef CLDir As Double, ByRef Turn As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim I As Integer
		Dim N As Integer
		Dim side As Integer
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection

		'Set pTmpPoly = ReArrangePolygon(pPolygon, PtDerL, CLDir)
		pTmpPoly = RemoveAgnails(pPolygon)
		pTmpPoly = ReArrangePolygon(pTmpPoly, PtDerL, CLDir)

		ReturnPolygonPartAsPolyline = New ESRI.ArcGIS.Geometry.Polyline
		N = pTmpPoly.PointCount - 1

		For I = 0 To N
			side = SideDef(PtDerL, CLDir, pTmpPoly.Point(I))
			If side = Turn Then
				ReturnPolygonPartAsPolyline.AddPoint(pTmpPoly.Point(I))
			End If
		Next

		If Turn < 0 Then
			pLine = ReturnPolygonPartAsPolyline
			pLine.ReverseOrientation()
		End If
	End Function

	Function CalcTouchByFixDir(ByRef PtSt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptFIX As ESRI.ArcGIS.Geometry.IPoint, ByRef TurnR As Double, ByRef dirCur As Double, ByRef DirFix As Double, ByRef TurnDir As Integer, ByRef TurnDir2 As Integer, ByRef SnapAngle As Double, ByRef dDir As Double, ByRef FlyBy As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Constructor As ESRI.ArcGIS.Geometry.IConstructPoint

		Dim PtCnt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint

		Dim pPt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt3 As ESRI.ArcGIS.Geometry.IPoint

		Dim SideD As Integer
		Dim SideT As Integer

		Dim DeltaAngle As Double
		Dim DeltaDist As Double
		Dim distToTmp As Double
		Dim dirToTmp As Double

		Dim OutDir As Double
		Dim OutDir0 As Double
		Dim OutDir1 As Double
		Dim Dist As Double

		If SubtractAngles(dirCur, DirFix) < 0.5 Then DirFix = dirCur

		CalcTouchByFixDir = New ESRI.ArcGIS.Geometry.Multipoint
		PtCnt1 = PointAlongPlane(PtSt, dirCur + 90.0 * TurnDir, TurnR)
		PtSt.M = dirCur

		OutDir0 = Modulus(DirFix - SnapAngle * TurnDir, 360.0)
		OutDir1 = Modulus(DirFix + SnapAngle * TurnDir, 360.0)

		Pt10 = PointAlongPlane(PtCnt1, OutDir0 - 90.0 * TurnDir, TurnR)
		Pt11 = PointAlongPlane(PtCnt1, OutDir1 - 90.0 * TurnDir, TurnR)

		SideT = SideDef(Pt10, DirFix, ptFIX)
		SideD = SideDef(Pt10, DirFix, PtCnt1)

		If SideT * SideD < 0 Then
			pPt1 = Pt10
			OutDir = OutDir0
		Else
			pPt1 = Pt11
			OutDir = OutDir1
		End If

		pPt1.M = OutDir

		FlyBy = New ESRI.ArcGIS.Geometry.Point
		Constructor = FlyBy

		Constructor.ConstructAngleIntersection(pPt1, DegToRad(OutDir), ptFIX, DegToRad(DirFix))

		Dist = ReturnDistanceInMeters(pPt1, FlyBy)

		dirToTmp = ReturnAngleInDegrees(ptFIX, FlyBy)
		distToTmp = ReturnDistanceInMeters(ptFIX, FlyBy)

		TurnDir2 = -AnglesSideDef(OutDir, DirFix)

		If TurnDir2 < 0 Then
			DeltaAngle = Modulus(180.0 + DirFix - OutDir, 360.0)
		ElseIf TurnDir2 > 0 Then
			DeltaAngle = Modulus(OutDir - 180.0 - DirFix, 360.0)
		End If

		DeltaAngle = 0.5 * DeltaAngle
		DeltaDist = TurnR / System.Math.Tan(DegToRad(DeltaAngle))

		dDir = Dist - DeltaDist

		If DeltaDist <= Dist Then
			pPt2 = PointAlongPlane(FlyBy, OutDir - 180.0, DeltaDist)
			pPt3 = PointAlongPlane(FlyBy, DirFix, DeltaDist)
		Else
			pPt2 = PointAlongPlane(FlyBy, OutDir, DeltaDist)
			pPt3 = PointAlongPlane(FlyBy, DirFix - 180.0, DeltaDist)
		End If

		pPt2.M = OutDir
		pPt3.M = DirFix

		CalcTouchByFixDir.AddPoint(PtSt)
		CalcTouchByFixDir.AddPoint(pPt1)
		CalcTouchByFixDir.AddPoint(pPt2)
		CalcTouchByFixDir.AddPoint(pPt3)
	End Function

	'Public Sub SaveMxDocument(ByRef FileName As String, Optional ByRef ClearGraphics As Boolean = True)
	'    Dim pGxLayer As ESRI.ArcGIS.Catalog.IGxLayer
	'    Dim pGxFile As ESRI.ArcGIS.Catalog.IGxFile
	'    Dim pLyr As ESRI.ArcGIS.Carto.ILayer
	'    Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer

	'    pGraphics = GetActiveView().GraphicsContainer

	'    pLyr = pMap.ActiveGraphicsLayer
	'    pLyr.Name_2 = "PANDA Chart"
	'    pLyr.SpatialReference = pSpRefPrj

	'    pGxLayer = New ESRI.ArcGIS.Catalog.GxLayer
	'    pGxFile = pGxLayer

	'    pGxFile.Path = FileName & "_Chart.lyr"

	'    pGxLayer.Layer = pLyr

	'    pGraphics.DeleteAllElements()

	'    pGxLayer = New ESRI.ArcGIS.Catalog.GxLayer
	'    pGxFile = pGxLayer

	'    pGxFile.Path = FileName & "_Chart.lyr"

	'    pMap.AddLayer(pGxLayer.Layer)
	'    Application.SaveAsDocument(FileName & "_Chart.mxd", True)
	'    pMap.DeleteLayer(pGxLayer.Layer)
	'End Sub

	Public Function ShowSaveDialog(ByRef FileName As String, ByRef FileTitle As String) As Boolean
		Dim pos As Integer
		Dim pos2 As Integer
		Dim ProjectPath As String
		Dim SaveDlg As New System.Windows.Forms.SaveFileDialog()

		ProjectPath = GetMapFileName()

		pos = InStrRev(ProjectPath, "\")
		pos2 = InStrRev(ProjectPath, ".")

		FileName = ""
		FileTitle = ""
		SaveDlg.DefaultExt = ""
		SaveDlg.InitialDirectory = Left(ProjectPath, pos)
		SaveDlg.Title = My.Resources.str0502
		SaveDlg.FileName = Left(ProjectPath, pos2 - 1) + ".htm"
		SaveDlg.Filter = "Text files (*.txt)|*.txt|PANDA Report Files (*.htm)|*.htm|PANDA Report Files (*.html)|*.html|All files (*.*)|*.*"

		If SaveDlg.ShowDialog() <> DialogResult.OK Then Return False

		FileName = SaveDlg.FileName
		FileTitle = SaveDlg.FileName

		pos = InStrRev(FileName, ".")
		pos2 = InStrRev(ProjectPath, "\")

		If (pos > 0) Then FileName = Left(FileName, pos - 1)

		If (pos2 > 0) Then FileTitle = Right(FileTitle, pos2 + 1)
		pos2 = InStrRev(FileTitle, ".")
		If (pos2 > 0) Then FileTitle = Left(FileTitle, pos2 - 1)
		Return True
	End Function


	Private Function CreateLEGFeatureClass(ByRef pFeatureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace, ByRef className As String) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit
		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit
		Dim pGeomDef As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit
		Dim ShapeFieldName As String

		On Error GoTo EH

		ShapeFieldName = "Shape"
		' Add the Fields to the class the OID and Shape are compulsory

		pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields
		'=======================================OID

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field

		pFieldEdit.Name_2 = "OID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
		pFieldEdit.AliasName_2 = "Object ID"
		pFieldEdit.Editable_2 = False
		pFieldEdit.IsNullable_2 = False

		pFieldsEdit.AddField(pFieldEdit)

		'=======================================SHAPE
		pGeomDef = New ESRI.ArcGIS.Geodatabase.GeometryDef

		pGeomDef.AvgNumPoints_2 = 2
		pGeomDef.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline
		pGeomDef.GridCount_2 = 1
		pGeomDef.GridSize_2(0) = 1000
		pGeomDef.HasM_2 = True
		pGeomDef.HasZ_2 = True
		pGeomDef.SpatialReference_2 = pSpRefShp


		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = ShapeFieldName
			.AliasName_2 = ShapeFieldName
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
			.Editable_2 = True
			.IsNullable_2 = False
			.GeometryDef_2 = pGeomDef
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'=======================================NAME
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "NAME"
			.AliasName_2 = "NAME"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 12
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================NO_SEQ
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "NO_SEQ"
			.AliasName_2 = "NO_SEQ"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
			.Editable_2 = True
			.IsNullable_2 = False
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_PHASE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_PHASE"
			.AliasName_2 = "CODE_PHASE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_TYPE"
			.AliasName_2 = "CODE_TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 2
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_COURSE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_COURSE"
			.AliasName_2 = "VAL_COURSE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 8
			.Precision_2 = 4
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_TYPE_COURSE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_TYPE_COURSE"
			.AliasName_2 = "CODE_TYPE_COURSE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 4
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_DIR_TURN
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_DIR_TURN"
			.AliasName_2 = "CODE_DIR_TURN"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_TURN_VALID
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_TURN_VALID"
			.AliasName_2 = "CODE_TURN_VALID"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_DESCR_DIST_VER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_DESCR_DIST_VER"
			.AliasName_2 = "CODE_DESCR_DIST_VER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_DIST_VER_UPPER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_DIST_VER_UPPER"
			.AliasName_2 = "CODE_DIST_VER_UPPER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_DIST_VER_UPPER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_DIST_VER_UPPER"
			.AliasName_2 = "VAL_DIST_VER_UPPER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 12
			.Precision_2 = 8
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================UOM_DIST_VER_UPPER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "UOM_DIST_VER_UPPER"
			.AliasName_2 = "UOM_DIST_VER_UPPER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 2
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_DIST_VER_LOWER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_DIST_VER_LOWER"
			.AliasName_2 = "CODE_DIST_VER_LOWER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_DIST_VER_LOWER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_DIST_VER_LOWER"
			.AliasName_2 = "VAL_DIST_VER_LOWER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 12
			.Precision_2 = 8
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================UOM_DIST_VER_LOWER
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "UOM_DIST_VER_LOWER"
			.AliasName_2 = "UOM_DIST_VER_LOWER"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 2
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_VER_ANGLE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_VER_ANGLE"
			.AliasName_2 = "VAL_VER_ANGLE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 9
			.Precision_2 = 6
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_SPEED_LIMIT
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_SPEED_LIMIT"
			.AliasName_2 = "VAL_SPEED_LIMIT"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 10
			.Precision_2 = 8
			.Scale_2 = 1
		End With

		pFieldsEdit.AddField(pFieldEdit)

		'=======================================UOM_SPEED
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "UOM_SPEED"
			.AliasName_2 = "UOM_SPEED"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 10
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_SPEED_REF
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_SPEED_REF"
			.AliasName_2 = "CODE_SPEED_REF"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 4
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_DIST
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_DIST"
			.AliasName_2 = "VAL_DIST"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 12
			.Precision_2 = 8
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_DUR
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_DUR"
			.AliasName_2 = "VAL_DUR"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 10
			.Precision_2 = 8
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================UOM_DUR
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "UOM_DUR"
			.AliasName_2 = "UOM_DUR"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_THETA
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_THETA"
			.AliasName_2 = "VAL_THETA"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 8
			.Precision_2 = 3
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_RHO
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_RHO"
			.AliasName_2 = "VAL_RHO"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 12
			.Precision_2 = 8
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================VAL_BANK_ANGLE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "VAL_BANK_ANGLE"
			.AliasName_2 = "VAL_BANK_ANGLE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 7
			.Precision_2 = 3
			.Scale_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================UOM_DIST_HORZ
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "UOM_DIST_HORZ"
			.AliasName_2 = "UOM_DIST_HORZ"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 2
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_REP_ATC
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_REP_ATC"
			.AliasName_2 = "CODE_REP_ATC"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 1
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================CODE_ROLE_FIX
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "CODE_ROLE_FIX"
			.AliasName_2 = "CODE_ROLE_FIX"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 4
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================TXT_RMK
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TXT_RMK"
			.AliasName_2 = "TXT_RMK"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 5000
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================TRAKING_FACILITY
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TRAKING_FACILITY"
			.AliasName_2 = "TRAKING_FACILITY"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================TRAKING_TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TRAKING_TYPE"
			.AliasName_2 = "TRAKING_TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================INTERSECTING_FACILITY
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "INTERSECTING_FACILITY"
			.AliasName_2 = "INTERSECTING_FACILITY"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================INTERSECTING_TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "INTERSECTING_TYPE"
			.AliasName_2 = "INTERSECTING_TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 3
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================INTERSECTING_TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "PROC_TYPE"
			.AliasName_2 = "PROC_TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 4
		End With
		pFieldsEdit.AddField(pFieldEdit)

		''=======================================SHAPE_Length
		'    Set pFieldEdit = New esriGeoDatabase.Field
		'    With pFieldEdit
		'        .Name_2 = "SHAPE_Length"
		'        .AliasName_2 = "SHAPE_Length"
		'        .Type_2 = esriFieldTypeDouble
		'        .Editable_2 = True
		'        .IsNullable_2 = False
		'        .Length_2 = 6
		'        .Precision_2 = 6
		'        .Scale_2 = 1
		'    End With
		'    pFieldsEdit.AddField pFieldEdit

		'=======================================INPUTDATE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "INPUTDATE"
			.AliasName_2 = "INPUTDATE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate
			.Editable_2 = True
			.IsNullable_2 = False
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'=======================================T_TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "T_TYPE"
			.AliasName_2 = "T_TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger
			.Editable_2 = True
			.IsNullable_2 = False
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'=======================================Radius_M
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "Radius_M"
			.AliasName_2 = "Radius_M"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 6
			.Precision_2 = 6
			.Scale_2 = 0
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'=======================================

		Dim pUid As ESRI.ArcGIS.esriSystem.IUID
		pFields = pFieldsEdit
		pUid = New ESRI.ArcGIS.esriSystem.UID
		pUid.Value = "esriCore.Feature"
		CreateLEGFeatureClass = pFeatureWorkspace.CreateFeatureClass(className, pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, ShapeFieldName, "")
		Exit Function
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, MsgBoxStyle.Information, "createAccessWorkspace")
	End Function

	Public Sub AddShapeFile(ByRef pFeatureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace, ByRef pFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass)
		Dim I As Integer
		Dim sName As String
		Dim bLayerExist As Boolean
		Dim pFeatureLayer As ESRI.ArcGIS.Carto.IFeatureLayer

		sName = pFeatureClass.AliasName
		bLayerExist = False

		For I = 0 To pMap.LayerCount - 1
			If pMap.Layer(I).Name_2 = sName Then
				bLayerExist = True
				Exit For
			End If
		Next I

		If Not bLayerExist Then
			'Create a new FeatureLayer and assign a shapefile to it
			pFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayer
			pFeatureLayer.FeatureClass = pFeatureClass
			pFeatureLayer.Name_2 = sName
			'Add the FeatureLayer to the focus map
			pMap.AddLayer(pFeatureLayer)
		End If
	End Sub

	Public Function OpenLEGFeatureClass(ByRef pFeatWs As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		OpenLEGFeatureClass = Nothing
		On Error Resume Next
		OpenLEGFeatureClass = pFeatWs.OpenFeatureClass("LEGs")
		On Error GoTo EH
		If OpenLEGFeatureClass Is Nothing Then ') And bCreateNew
			OpenLEGFeatureClass = CreateLEGFeatureClass(pFeatWs, "LEGs")
			AddShapeFile(pFeatWs, OpenLEGFeatureClass)
		End If

		'    Set OpenLEGFeatureClass = pFeatWs
		Exit Function
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, MsgBoxStyle.Information, "OpenLEGFeatureClass")
	End Function

	Public Function OpenLEGWorkspace(ByRef FileName As String) As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pWorkspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory
		Dim L As Integer
		Dim Pos As Integer
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
		FileName = FileNameForCreate & ".mdb"

		'Create a new AccessWorkspaceFactory object and open a shapefile database
		pWorkspaceFactory = New ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory
		On Error Resume Next
		OpenLEGWorkspace = pWorkspaceFactory.OpenFromFile(Location & FileName, 0)
		On Error GoTo EH

		'Create a new AccessWorkspaceFactory object and open a shapefile database
		Dim pWorkspaceName As ESRI.ArcGIS.Geodatabase.IWorkspaceName
		If OpenLEGWorkspace Is Nothing Then

			'        Set pWorkspaceName = pWorkspaceFactory.Create(Location, FileNameForCreate, Nothing, 0)
			pWorkspaceName = pWorkspaceFactory.Create(Location, FileName, Nothing, 0)

			pWorkspaceFactory = pWorkspaceName.WorkspaceFactory
			OpenLEGWorkspace = pWorkspaceFactory.OpenFromFile(Location & FileName, 0)
			pWorkspaceName = Nothing
		End If
		Exit Function
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, MsgBoxStyle.Information, )
	End Function

	Function PointAlongPlane(ByRef ptGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		dirAngle = DegToRad(dirAngle)
		pClone = ptGeo
		PointAlongPlane = pClone.Clone

		PointAlongPlane.PutCoords(ptGeo.X + Dist * System.Math.Cos(dirAngle), ptGeo.Y + Dist * System.Math.Sin(dirAngle))
	End Function

	Public Sub shall_SortfSort(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).fSort > TempVal.fSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortfSortD(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).fSort < TempVal.fSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSort(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).sSort > TempVal.sSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub

	Public Sub shall_SortsSortD(ByRef obsArray() As TypeDefinitions.ObstacleType)
		Dim TempVal As TypeDefinitions.ObstacleType
		Dim GapSize, I, CurPos As Integer
		Dim LastRow, FirstRow, NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray(I)
				Do While obsArray(CurPos - GapSize).sSort < TempVal.sSort
					obsArray(CurPos) = obsArray(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray(CurPos) = TempVal
			Next I
		Loop Until GapSize = 1
	End Sub
End Module