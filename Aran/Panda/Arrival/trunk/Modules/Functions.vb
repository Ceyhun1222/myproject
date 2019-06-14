Option Strict Off
Option Explicit On
Option Compare Text

Imports System.Collections.Generic
Imports System.Data.OleDb
Imports System.Linq
Imports System.Runtime.InteropServices
Imports Aran.Aim
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Enums
Imports Aran.Aim.Features
Imports ESRI.ArcGIS.Geometry

Module Functions

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function GetSystemMenu(hWnd As IntPtr, bRevert As Boolean) As IntPtr
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function AppendMenu(hMenu As IntPtr, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	<DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
	Public Function InsertMenu(hMenu As IntPtr, uPosition As Integer, uFlags As Integer, uIDNewItem As Integer, lpNewItem As String) As Boolean
	End Function

	Public Sub LockControl(ByRef pControl As System.Windows.Forms.Control, ByVal Locked As Boolean)
		pControl.Enabled = Not Locked
		If Locked Then
			pControl.BackColor = System.Drawing.SystemColors.Control
		Else
			pControl.BackColor = System.Drawing.SystemColors.Window
		End If
	End Sub

	Public Sub ShowErrorMessage(message As String, Optional ByVal isError As Boolean = True)
		MessageBox.Show(message, ModuleName, MessageBoxButtons.OK, IIf(isError, MessageBoxIcon.Error, MessageBoxIcon.Warning))
	End Sub

	Public Function ConvertDistance(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.CEIL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * DistanceConverter(DistanceUnit).Multiplier
				'Case eRoundMode.rmFLOOR
				'Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding - 0.4999) * DistanceConverter(DistanceUnit).Rounding
				'Case eRoundMode.rmCEIL
				'Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding + 0.4999) * DistanceConverter(DistanceUnit).Rounding
				'Case eRoundMode.rmNERAEST
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * DistanceConverter(DistanceUnit).Multiplier / DistanceConverter(DistanceUnit).Rounding) * DistanceConverter(DistanceUnit).Rounding
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertHeight(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * HeightConverter(HeightUnit).Multiplier
				'Case eRoundMode.rmFLOOR
				'Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding - 0.4999) * HeightConverter(HeightUnit).Rounding
				'Case eRoundMode.rmCEIL
				'Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding + 0.4999) * HeightConverter(HeightUnit).Rounding
				'Case eRoundMode.rmNERAEST
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
			Case eRoundMode.SPECIAL
				If HeightUnit = 0 Then
					Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / 50.0) * 50.0
				ElseIf HeightUnit = 1 Then
					Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / 100.0) * 100.0
				Else
					Return System.Math.Round(Val_Renamed * HeightConverter(HeightUnit).Multiplier / HeightConverter(HeightUnit).Rounding) * HeightConverter(HeightUnit).Rounding
				End If
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertSpeed(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * SpeedConverter(SpeedUnit).Multiplier
				'Case eRoundMode.rmFLOOR
				'Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding - 0.4999) * SpeedConverter(SpeedUnit).Rounding
				'Case eRoundMode.rmCEIL
				'Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding + 0.4999) * SpeedConverter(SpeedUnit).Rounding
				'Case eRoundMode.rmNERAEST
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / SpeedConverter(SpeedUnit).Rounding) * SpeedConverter(SpeedUnit).Rounding
			Case eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * SpeedConverter(SpeedUnit).Multiplier / 5.0) * 5.0
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertDSpeed(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * DSpeedConverter(HeightUnit).Multiplier
				'Case eRoundMode.rmFLOOR
				'Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding - 0.4999) * DSpeedConverter(HeightUnit).Rounding
				'Case eRoundMode.rmCEIL
				'Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding + 0.4999) * DSpeedConverter(HeightUnit).Rounding
				'Case eRoundMode.rmNERAEST
			Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / DSpeedConverter(HeightUnit).Rounding) * DSpeedConverter(HeightUnit).Rounding
			Case eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * DSpeedConverter(HeightUnit).Multiplier / 5.0) * 5.0
		End Select
		Return Val_Renamed
	End Function

	Public Function ConvertAngle(ByVal Val_Renamed As Double, Optional ByVal RoundMode As eRoundMode = eRoundMode.NEAREST) As Double
		If (RoundMode < eRoundMode.NONE) Or (RoundMode > eRoundMode.SPECIAL) Then RoundMode = eRoundMode.NONE
		Select Case RoundMode
			Case eRoundMode.NONE
				Return Val_Renamed * AngleConverter(AngleUnit).Multiplier
			Case eRoundMode.FLOOR
				Return System.Math.Round(Val_Renamed * AngleConverter(AngleUnit).Multiplier / AngleConverter(AngleUnit).Rounding - 0.4999) * AngleConverter(AngleUnit).Rounding
			Case eRoundMode.CEIL
				Return System.Math.Round(Val_Renamed * AngleConverter(AngleUnit).Multiplier / AngleConverter(AngleUnit).Rounding + 0.4999) * AngleConverter(AngleUnit).Rounding
			Case eRoundMode.NEAREST
				Return System.Math.Round(Val_Renamed * AngleConverter(AngleUnit).Multiplier / AngleConverter(AngleUnit).Rounding) * AngleConverter(AngleUnit).Rounding
			'Case eRoundMode.FLOOR, eRoundMode.CEIL, eRoundMode.NEAREST
			'	Return System.Math.Round(Val_Renamed * AngleConverter(AngleUnit).Multiplier / AngleConverter(AngleUnit).Rounding) * AngleConverter(AngleUnit).Rounding
			Case eRoundMode.SPECIAL
				Return System.Math.Round(Val_Renamed * AngleConverter(AngleUnit).Multiplier / 5.0) * 5.0
		End Select
		Return Val_Renamed
	End Function

	Public Function DeConvertDistance(ByVal Val_Renamed As Double) As Double
		Return Val_Renamed / DistanceConverter(DistanceUnit).Multiplier
	End Function

	Public Function DeConvertHeight(ByVal Val_Renamed As Double) As Double
		Return Val_Renamed / HeightConverter(HeightUnit).Multiplier
	End Function

	Public Function DeConvertSpeed(ByVal Val_Renamed As Double) As Double
		Return Val_Renamed / SpeedConverter(SpeedUnit).Multiplier
	End Function

	Public Function DeConvertDSpeed(ByVal Val_Renamed As Double) As Double
		Return Val_Renamed / DSpeedConverter(HeightUnit).Multiplier
	End Function

	'Public Function DeConvertAngle(ByVal Val_Renamed As Double) As Double

	Public Function DegreeToString(ByVal X As Double, ByVal Mode As Degree2StringMode) As String
		Dim xDeg As Double
		Dim xMin As Double
		Dim xIMin As Double
		Dim xSec As Double
		Dim lSign As Boolean = False
		Dim sSign As String
		Dim sTmp As String
		Dim sResult As String

		sResult = ""
		sSign = ""

		If Mode = Degree2StringMode.DMSLat Then
			lSign = Math.Sign(X) < 0
			If lSign Then X = -X

			xDeg = System.Math.Floor(X)
			xMin = (X - xDeg) * 60.0
			xIMin = System.Math.Floor(xMin)
			xSec = (xMin - xIMin) * 60.0
			If (xSec >= 60.0) Then
				xSec = 0.0
				xIMin += 1
			End If

			If (xIMin >= 60.0) Then
				xIMin = 0.0
				xDeg += 1
			End If

			sTmp = xDeg.ToString("00")
			sResult = sTmp + "°"

			sTmp = xIMin.ToString("00")
			sResult = sResult + sTmp + "'"

			sTmp = xSec.ToString("00.00")
			sResult = sResult + sTmp + """"

			Return sResult + IIf(X > 0, "N", "S")
		End If

		If Mode = Degree2StringMode.DMSLon Then
			X = NativeMethods.Modulus(X)
			lSign = X > 180.0
			If lSign Then X = 360.0 - X

			xDeg = System.Math.Floor(X)
			xMin = (X - xDeg) * 60.0
			xIMin = System.Math.Floor(xMin)
			xSec = (xMin - xIMin) * 60.0
			If (xSec >= 60.0) Then
				xSec = 0.0
				xIMin += 1
			End If

			If (xIMin >= 60.0) Then
				xIMin = 0.0
				xDeg += 1
			End If

			sTmp = xDeg.ToString("000")
			sResult = sTmp + "°"

			sTmp = xIMin.ToString("00")
			sResult = sResult + sTmp + "'"

			sTmp = xSec.ToString("00.00")
			sResult = sResult + sTmp + """"

			Return sResult + IIf(X > 0, "E", "W")
		End If

		If (System.Math.Sign(X) < 0) Then sSign = "-"
		X = NativeMethods.Modulus(System.Math.Abs(X))

		Select Case Mode
			Case Degree2StringMode.DD
				Return sSign + X.ToString("#0.00##") + "°"
			Case Degree2StringMode.DM

				xDeg = System.Math.Floor(X)
				xMin = (X - xDeg) * 60.0
				If (xMin >= 60) Then
					X += 1
					xMin = 0
				End If

				sResult = sSign + xDeg.ToString() + "°"

				sTmp = xMin.ToString("00.00##")
				Return sResult + sTmp + "'"
			Case Degree2StringMode.DMS
				If (System.Math.Sign(X) < 0) Then sSign = "-"
				X = NativeMethods.Modulus(System.Math.Abs(X))

				xDeg = System.Math.Floor(X)
				xMin = (X - xDeg) * 60.0
				xIMin = System.Math.Floor(xMin)
				xSec = System.Math.Round((xMin - xIMin) * 60.0, 2)
				If (xSec >= 60.0) Then
					xSec = 0.0
					xIMin += 1
				End If

				If (xIMin >= 60.0) Then
					xIMin = 0.0
					xDeg += 1
				End If

				sResult = sSign + xDeg.ToString() + "°"

				sTmp = xIMin.ToString("00")
				sResult = sResult + sTmp + "'"

				sTmp = xSec.ToString("00.00")
				Return sResult + sTmp + """"
		End Select

		Return sResult
	End Function

	Public Sub FillDomainControl(ByRef control As DomainUpDown, ByVal startVal As Integer, endVal As Integer)
		Dim i As Integer

		control.Items.Clear()
		For i = startVal To endVal
			control.Items.Add(Modulus(i))
		Next

		'If control.Items.Count > 0 Then control.SelectedIndex = 0
	End Sub

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

	'Public Function GetCategoryByProcedureName(ByVal ProcedureName As String) As Integer
	'	Dim L As Integer

	'	L = Len(ProcedureName)

	'	Select Case Mid(ProcedureName, L, 1)
	'		Case "A"
	'			Return 0
	'		Case "B"
	'			Return 1
	'		Case "C"
	'			Return 2
	'		Case "D"
	'			Return 3
	'		Case "E"
	'			Return 4
	'		Case "H"
	'			Return 5
	'	End Select
	'	Return 3
	'End Function

	Public Function ToGeo(ByVal pPrjGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pClone = pPrjGeom
		ToGeo = pClone.Clone
		ToGeo.SpatialReference = pSpRefPrj
		ToGeo.Project(pSpRefGeo)

		If ToGeo.GeometryType = esriGeometryType.esriGeometryPolygon Then
			pTopoOper = ToGeo
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If
	End Function

	Public Function ToPrj(ByVal pGeoGeom As ESRI.ArcGIS.Geometry.IGeometry) As ESRI.ArcGIS.Geometry.IGeometry
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pClone = pGeoGeom
		ToPrj = pClone.Clone
		ToPrj.SpatialReference = pSpRefGeo
		ToPrj.Project(pSpRefPrj)

		If ToPrj.GeometryType = esriGeometryType.esriGeometryPolygon Then
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

	Function Azt2DirPrj(ByVal ptPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal Azt As Double) As Double
		Dim ptGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint
		Dim ResX As Double
		Dim ResY As Double

		ptGeo = ToGeo(ptPrj)
		PointAlongGeodesic(ptGeo.X, ptGeo.Y, 10.0, Azt, ResX, ResY)

		Pt10 = New ESRI.ArcGIS.Geometry.Point
		Pt10.PutCoords(ResX, ResY)

		Pt11 = ToPrj(Pt10)
		Return ReturnAngleInDegrees(ptPrj, Pt11)
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

	Public Function Dir2AztGeo(ByVal ptGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal Dir_Renamed As Double) As Double
		Dim ptPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim resD As Double
		Dim resI As Double
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint

		ptPrj = ToPrj(ptGeo)
		Pt10 = ToGeo(PointAlongPlane(ptPrj, Dir_Renamed, 10.0))

		ReturnGeodesicAzimuth(ptGeo.X, ptGeo.Y, Pt10.X, Pt10.Y, resD, resI)
		Return resD
	End Function

	Function PointAlongPlane(ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal dirAngle As Double, ByVal Dist As Double) As ESRI.ArcGIS.Geometry.IPoint
		PointAlongPlane = New ESRI.ArcGIS.Geometry.Point
		dirAngle = DegToRadValue * dirAngle
		PointAlongPlane.PutCoords(ptFrom.X + Dist * System.Math.Cos(dirAngle), ptFrom.Y + Dist * System.Math.Sin(dirAngle))
	End Function

	' ==================================
	Function LocalToPrj(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal X As Double, Optional ByVal Y As Double = 0.0) As ESRI.ArcGIS.Geometry.Point
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim Xnew As Double = center.X + X * CosA - Y * SinA
		Dim Ynew As Double = center.Y + X * SinA + Y * CosA

		Dim Result As New ESRI.ArcGIS.Geometry.Point
		Result.PutCoords(Xnew, Ynew)
		Result.Z = center.Z
		Result.M = center.M
		Return Result
	End Function

	Function LocalToPrj(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal ptPrj As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.Point
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim Xnew As Double = center.X + ptPrj.X * CosA - ptPrj.Y * SinA
		Dim Ynew As Double = center.Y + ptPrj.X * SinA + ptPrj.Y * CosA

		Dim Result As New ESRI.ArcGIS.Geometry.Point
		Result.PutCoords(Xnew, Ynew)
		Result.Z = center.Z
		Result.M = center.M
		Return Result
	End Function
	' ==================================
	Function PrjToLocal(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal x As Double, ByVal y As Double) As ESRI.ArcGIS.Geometry.Point
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim dX As Double = x - center.X
		Dim dY As Double = y - center.Y
		Dim Xnew As Double = dX * CosA + dY * SinA
		Dim Ynew As Double = -dX * SinA + dY * CosA

		Dim result As New ESRI.ArcGIS.Geometry.Point
		result.PutCoords(Xnew, Ynew)
		result.Z = center.Z
		Return result
	End Function

	Sub PrjToLocal(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal x As Double, ByVal y As Double, ByRef resX As Double, ByRef resY As Double)
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim dX As Double = x - center.X
		Dim dY As Double = y - center.Y

		resX = dX * CosA + dY * SinA
		resY = -dX * SinA + dY * CosA
	End Sub

	Function PrjToLocal(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal ptPrj As IPoint) As ESRI.ArcGIS.Geometry.Point
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim dX As Double = ptPrj.X - center.X
		Dim dY As Double = ptPrj.Y - center.Y
		Dim Xnew As Double = dX * CosA + dY * SinA
		Dim Ynew As Double = -dX * SinA + dY * CosA

		Dim result As New ESRI.ArcGIS.Geometry.Point
		result.PutCoords(Xnew, Ynew)
		result.Z = ptPrj.Z
		Return result
	End Function

	Sub PrjToLocal(ByVal center As ESRI.ArcGIS.Geometry.IPoint, ByVal dirInDeg As Double, ByVal ptPrj As ESRI.ArcGIS.Geometry.IPoint, ByRef resX As Double, ByRef resY As Double)
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim dX As Double = ptPrj.X - center.X
		Dim dY As Double = ptPrj.Y - center.Y

		resX = dX * CosA + dY * SinA
		resY = -dX * SinA + dY * CosA
	End Sub

	' ==================================

	Public Function IAS2TAS(ByVal IAS As Double, ByVal H As Double, ByVal Dt As Double) As Double
		Return IAS * 171233.0 * System.Math.Sqrt(288.0 + Dt - 0.006496 * H) / ((288.0 - 0.006496 * H) ^ 2.628)
	End Function

	Public Function Bank2Radius(ByVal Bank As Double, ByVal V As Double) As Double
		Dim Rv As Double

		Rv = 6.355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)

		If (Rv > 0.003) Then Rv = 0.003
		If (Rv > 0.0) Then Return V / (20.0 * PI * Rv)

		Return -1.0
	End Function

	Public Function Radius2Bank(ByVal R As Double, ByVal V As Double) As Double
		If (R > 0.0) Then Return RadToDeg(System.Math.Atan(V * V / (20.0 * R * 6.355)))

		Return -1.0
	End Function

	Public Function CalcMAPtDistD(ByVal H As Double, ByVal Categoty As Integer) As Double
		Dim fTAS As Double

		fTAS = IAS2TAS(3.6 * cVmaInter.Values(Categoty), H, arISAmax.Value) + 3.6 * arNearTerrWindSp.Value
		Return 0.277777777777778 * fTAS * arMAPilotToleran.Value
	End Function

	Public Function CalcMAPtDistX(ByVal H As Double, ByVal Categoty As Integer) As Double
		Dim fTAS As Double

		fTAS = IAS2TAS(3.6 * cVmaInter.Values(Categoty), H, arISAmax.Value) + 3.6 * arNearTerrWindSp.Value
		Return 0.277777777777778 * fTAS * arSOCdelayTime.Value
	End Function

	Public Function ArcSin(ByVal X As Double) As Double
		If System.Math.Abs(X) >= 1.0 Then
			If X > 0.0 Then Return 0.5 * PI
			Return -0.5 * PI
		End If

		Return System.Math.Atan(X / System.Math.Sqrt(-X * X + 1.0))
	End Function

	Public Function ArcCos(ByVal X As Double) As Double
		If System.Math.Abs(X) >= 1.0 Then Return 0.0

		Return System.Math.Atan(-X / System.Math.Sqrt(-X * X + 1.0)) + 0.5 * PI
	End Function

	Public Function Max(ByVal X As Double, ByVal Y As Double) As Double
		If X > Y Then Return X
		Return Y
	End Function

	Public Function Min(ByVal X As Double, ByVal Y As Double) As Double
		If X < Y Then Return X
		Return Y
	End Function

	Public Function Det(ByVal X0 As Double, ByVal Y0 As Double, ByVal X1 As Double, ByVal Y1 As Double) As Double
		Return X0 * Y1 - X1 * Y0
	End Function

	Public Function StartPointDist(ByVal A As D3DPolygone, ByVal B As D3DPolygone, ByVal hFAP As Double, ByVal FAPDist As Double) As Double
		Dim D As Double
		Dim dX As Double

		D = Det(A.Plane.A, A.Plane.B, B.Plane.A, B.Plane.B)
		If D = 0.0 Then Return -1.0

		dX = Det(-(A.Plane.D + A.Plane.C * hFAP), A.Plane.B, -(B.Plane.D + B.Plane.C * hFAP), B.Plane.B)

		Return System.Math.Abs(dX / D) - FAPDist
	End Function

	Function LinePolygonIntersect(ByVal poly As IPointCollection, ByVal ptVector As IPoint, ByVal dir As Double, ByRef distToPoly As Double, Optional ByVal findNearest As Boolean = False) As ESRI.ArcGIS.Geometry.IPoint
		Dim n As Integer = poly.PointCount

		distToPoly = Double.NaN
		If (n < 2) Then
			Return Nothing
		End If

		Dim PE As IPoint = poly.Point(0)
		Dim dirInRadian As Double = Functions.DegToRad(dir)

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim X1 As Double = (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA
		Dim Y1 As Double = -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA

		Dim HaveIntersection As Boolean = False
		Dim result As IPoint = Nothing

		For i As Integer = 1 To n + 1
			Dim X0 As Double = X1
			Dim Y0 As Double = Y1

			Dim j As Integer = i And (0 - Convert.ToInt32(i < n))
			PE = poly.Point(j)
			X1 = (PE.X - ptVector.X) * CosA + (PE.Y - ptVector.Y) * SinA
			Y1 = -(PE.X - ptVector.X) * SinA + (PE.Y - ptVector.Y) * CosA

			If ((Y0 * Y1 > 0) Or ((X0 < 0) And (X1 < 0))) Then
				Continue For
			End If

			Dim dXE As Double = X1 - X0
			Dim dYE As Double = Y1 - Y0
			Dim x As Double

			If (System.Math.Abs(dYE) < EpsilonDistance) Then
				x = X0
			Else
				x = X0 - Y0 * dXE / dYE
			End If
			If ((Not HaveIntersection) Or (findNearest And (x < distToPoly)) Or ((Not findNearest) And (x > distToPoly))) Then
				distToPoly = x
			End If
			HaveIntersection = True
		Next

		If (HaveIntersection) Then
			result = PointAlongPlane(ptVector, dir, distToPoly)
		End If
		Return result
	End Function

	Public Function IntersectPlanesAtHeight(ByVal A As D3DPolygone, ByVal B As D3DPolygone, ByVal fHeight As Double) As ESRI.ArcGIS.Geometry.IPoint ', ByVal ptCenter As Point, ByVal xDir As Double
		Dim D As Double
		Dim dX As Double
		Dim dY As Double

		IntersectPlanesAtHeight = New ESRI.ArcGIS.Geometry.Point

		D = Det(A.Plane.A, A.Plane.B, B.Plane.A, B.Plane.B)
		If D = 0.0 Then Exit Function

		dX = Det(-(A.Plane.D + A.Plane.C * fHeight), A.Plane.B, -(B.Plane.D + B.Plane.C * fHeight), B.Plane.B)
		dY = Det(A.Plane.A, -(A.Plane.D + A.Plane.C * fHeight), B.Plane.A, -(B.Plane.D + B.Plane.C * fHeight))

		IntersectPlanesAtHeight.X = dX / D
		IntersectPlanesAtHeight.Y = dY / D

		'    Set pTransform = IntersectPlanesAtHeight
		'    pTransform.Move ptCenter.X, ptCenter.Y
		'    pTransform.Rotate ptCenter, DegToRad(xDir)
	End Function

	Public Function CalcEquipotentialPoint(ByVal A As D3DPolygone, ByVal pRefPt As ESRI.ArcGIS.Geometry.Point, ByVal Dist As Double) As ESRI.ArcGIS.Geometry.IPoint ', ByVal ptCenter As IPoint, ByVal xDir As Double
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim D As Double
		pClone = pRefPt

		ptTmp = pClone.Clone
		CalcEquipotentialPoint = pClone.Clone

		D = A.Plane.A * A.Plane.A + A.Plane.B * A.Plane.B
		If D = 0.0 Then Exit Function
		D = Dist / System.Math.Sqrt(D)

		CalcEquipotentialPoint.X = ptTmp.X + A.Plane.B * D
		CalcEquipotentialPoint.Y = ptTmp.Y - A.Plane.A * D
	End Function

	Public Function CreateReducedTIA(ByVal OASPlanes() As D3DPolygone, ByVal dl As Double, ByVal ptCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal xDir As Double) As ESRI.ArcGIS.Geometry.IPolygon
		Dim pt300_L As ESRI.ArcGIS.Geometry.IPoint
		Dim pt300_R As ESRI.ArcGIS.Geometry.IPoint

		Dim pt300t_L As ESRI.ArcGIS.Geometry.IPoint
		Dim pt300t_R As ESRI.ArcGIS.Geometry.IPoint

		Dim pLineL As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLineR As ESRI.ArcGIS.Geometry.IPolyline

		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		Dim dX As Double
		Dim dY As Double

		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pt300_L = IntersectPlanesAtHeight(OASPlanes(eOAS.XlPlane), OASPlanes(eOAS.YlPlane), 300.0)
		pClone = pt300_L

		pt300_R = pClone.Clone
		pt300_R.Y = -pt300_L.Y

		pt300t_L = CalcEquipotentialPoint(OASPlanes(eOAS.YlPlane), pt300_L, dl)

		pClone = pt300t_L
		pt300t_R = pClone.Clone
		pt300t_R.Y = -pt300t_L.Y

		pLineL = IntersectPlanes(OASPlanes(eOAS.XlPlane).Plane, OASPlanes(eOAS.YlPlane).Plane, 0.0, 300.0)

		dX = pt300t_L.X - pt300_L.X
		dY = pt300t_L.Y - pt300_L.Y

		pTransform = pLineL
		pTransform.Move(dX, dY)

		pClone = pLineL
		pLineR = pClone.Clone

		pPtTmp = pLineR.FromPoint
		pPtTmp.Y = -pPtTmp.Y
		pLineR.FromPoint = pPtTmp

		pPtTmp = pLineR.ToPoint
		pPtTmp.Y = -pPtTmp.Y
		pLineR.ToPoint = pPtTmp

		pPointCollection = New ESRI.ArcGIS.Geometry.Polygon

		pPointCollection.AddPoint(pt300_L)
		pPointCollection.AddPoint(pt300t_L)

		pPointCollection.AddPoint(pLineL.ToPoint)
		pPointCollection.AddPoint(pLineL.FromPoint)
		'====================================================
		pPointCollection.AddPoint(pLineR.FromPoint)
		pPointCollection.AddPoint(pLineR.ToPoint)
		pPointCollection.AddPoint(pt300t_R)
		pPointCollection.AddPoint(pt300_R)

		pTopo = pPointCollection
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pTransform = pPointCollection
		pTransform.Move(ptCenter.X, ptCenter.Y)
		pTransform.Rotate(ptCenter, DegToRad(xDir))
		CreateReducedTIA = pPointCollection
	End Function

	Public Function IntersectPlanes(ByVal PlaneA As D3DPlane, ByVal PlaneB As D3DPlane, ByVal hMin As Double, ByVal hMax As Double) As ESRI.ArcGIS.Geometry.IPolyline
		Dim D As Double
		Dim dX As Double
		Dim dY As Double
		Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint

		IntersectPlanes = New ESRI.ArcGIS.Geometry.Polyline

		D = Det(PlaneA.A, PlaneA.B, PlaneB.A, PlaneB.B)
		If D = 0.0 Then Exit Function

		dX = Det(-(PlaneA.D + PlaneA.C * hMin), PlaneA.B, -(PlaneB.D + PlaneB.C * hMin), PlaneB.B)
		dY = Det(PlaneA.A, -(PlaneA.D + PlaneA.C * hMin), PlaneB.A, -(PlaneB.D + PlaneB.C * hMin))
		pt0 = New ESRI.ArcGIS.Geometry.Point
		pt0.X = dX / D
		pt0.Y = dY / D

		dX = Det(-(PlaneA.D + PlaneA.C * hMax), PlaneA.B, -(PlaneB.D + PlaneB.C * hMax), PlaneB.B)
		dY = Det(PlaneA.A, -(PlaneA.D + PlaneA.C * hMax), PlaneB.A, -(PlaneB.D + PlaneB.C * hMax))

		pt1 = New ESRI.ArcGIS.Geometry.Point
		pt1.X = dX / D
		pt1.Y = dY / D

		IntersectPlanes.FromPoint = pt0
		IntersectPlanes.ToPoint = pt1
	End Function

	Sub CreateNavaidZone(ByVal NavFacil As NavaidData, ByVal dirAngle As Double, ByVal ptTHRprj As IPoint,
	 ByVal Ss As Double, ByVal Vs As Double, ByVal FrontLen As Double, ByVal BackLen As Double,
	 ByRef LPolygon As IPointCollection, ByRef RPolygon As IPointCollection, ByRef PrimPolygon As IPointCollection)     ', Optional ByVal drawPolys As Boolean = False)

		Dim BaseLength As Double
		Dim ILSDir As Double
		Dim Alpha As Double
		Dim Betta As Double
		Dim d0 As Double
		Dim d1 As Double

		Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt4 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt5 As ESRI.ArcGIS.Geometry.IPoint
		Dim lOASPlanes(8) As D3DPolygone

		Dim Xlf As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Xrt As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pZPlane As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPlane01 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPlane02 As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		If LPolygon.PointCount > 0 Then LPolygon.RemovePoints(0, LPolygon.PointCount)
		If RPolygon.PointCount > 0 Then RPolygon.RemovePoints(0, RPolygon.PointCount)
		If PrimPolygon.PointCount > 0 Then PrimPolygon.RemovePoints(0, PrimPolygon.PointCount)

		If NavFacil.TypeCode = eNavaidType.LLZ Then
			ILSDir = Azt2Dir(NavFacil.pPtGeo, NavFacil.pPtGeo.M)
			OAS_DATABase(NavFacil.LLZ_THR, 3.0, 0.025, 1, NavFacil.GP_RDH, Ss, Vs, lOASPlanes)
			CreateOASPlanes(ptTHRprj, ILSDir, 300.0, lOASPlanes, 1) ', drawPolys

			LPolygon = lOASPlanes(eOAS.YlPlane).Poly
			RPolygon = lOASPlanes(eOAS.YrPlane).Poly

			pt0 = PointAlongPlane(NavFacil.pPtPrj, ILSDir, 10.0 * FrontLen)

			Xlf = ReArrangePolygon(lOASPlanes(eOAS.XlPlane).Poly, pt0, ILSDir)
			Xrt = ReArrangePolygon(lOASPlanes(eOAS.XrPlane).Poly, pt0, ILSDir + 180.0)

			pt1 = New ESRI.ArcGIS.Geometry.Point
			pt2 = New ESRI.ArcGIS.Geometry.Point

			pt0 = PointAlongPlane(NavFacil.pPtPrj, ILSDir + 180.0, FrontLen)

			Alpha = Math.Atan2(Xlf.Point(0).Y - Xlf.Point(Xlf.PointCount - 1).Y, Xlf.Point(0).X - Xlf.Point(Xlf.PointCount - 1).X)
			pConstruct = pt1
			pConstruct.ConstructAngleIntersection(Xlf.Point(0), Alpha, pt0, DegToRad(ILSDir + 90.0))

			Betta = Math.Atan2(Xrt.Point(Xrt.PointCount - 2).Y - Xrt.Point(Xrt.PointCount - 1).Y, Xrt.Point(Xrt.PointCount - 2).X - Xrt.Point(Xrt.PointCount - 1).X)

			pConstruct = pt2
			pConstruct.ConstructAngleIntersection(Xrt.Point(Xrt.PointCount - 2), Betta, pt0, DegToRad(ILSDir + 90.0))

			pPlane01 = New ESRI.ArcGIS.Geometry.Polygon
			pPlane01.AddPoint(pt1)
			pPlane01.AddPoint(pt2)
			pPlane01.AddPoint(Xrt.Point(Xrt.PointCount - 1))
			pPlane01.AddPoint(Xlf.Point(Xlf.PointCount - 1))

			pTopo = pPlane01
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pPlane02 = pTopo.Union(lOASPlanes(eOAS.ZeroPlane).Poly)
			'=======================================================================
			pZPlane = ReArrangePolygon(lOASPlanes(eOAS.ZPlane).Poly, ptTHRprj, ILSDir)

			pt0 = PointAlongPlane(NavFacil.pPtPrj, ILSDir, BackLen)

			pConstruct = pt1
			pConstruct.ConstructAngleIntersection(pZPlane.Point(1), DegToRad(ILSDir - arMA_SplayAngle.Value), pt0, DegToRad(ILSDir - 90.0))

			pConstruct = pt2
			pConstruct.ConstructAngleIntersection(pZPlane.Point(2), DegToRad(ILSDir + arMA_SplayAngle.Value), pt0, DegToRad(ILSDir - 90.0))

			pPlane01 = New ESRI.ArcGIS.Geometry.Polygon

			pPlane01.AddPoint(pZPlane.Point(0))
			pPlane01.AddPoint(pZPlane.Point(1))
			pPlane01.AddPoint(pt1)
			pPlane01.AddPoint(pt2)
			pPlane01.AddPoint(pZPlane.Point(2))
			pPlane01.AddPoint(pZPlane.Point(3))

			pTopo = pPlane01
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			PrimPolygon = pTopo.Union(pPlane02)
			pTopo = PrimPolygon
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		Else
			If NavFacil.TypeCode = eNavaidType.NDB Then
				BaseLength = NDB.InitWidth * 0.5
				Alpha = NDB.SplayAngle
			ElseIf (NavFacil.TypeCode = eNavaidType.VOR) Or (NavFacil.TypeCode = eNavaidType.TACAN) Then
				BaseLength = 0.5 * VOR.InitWidth
				Alpha = VOR.SplayAngle
			Else
				Return
			End If

			d0 = FrontLen / System.Math.Cos(DegToRad(Alpha))
			d1 = BackLen / System.Math.Cos(DegToRad(Alpha))

			Betta = 0.5 * System.Math.Tan(DegToRad(Alpha))
			Betta = System.Math.Atan(Betta)
			Betta = RadToDeg(Betta)

			'==========LeftPolygon
			pt0 = PointAlongPlane(NavFacil.pPtPrj, dirAngle + 90.0, BaseLength)
			pt3 = PointAlongPlane(NavFacil.pPtPrj, dirAngle + 90.0, 0.5 * BaseLength)

			pt1 = PointAlongPlane(pt0, dirAngle + Alpha, d1)
			pt2 = PointAlongPlane(pt3, dirAngle + Betta, d1)

			LPolygon.AddPoint(pt0)
			LPolygon.AddPoint(pt1)
			LPolygon.AddPoint(pt2)
			LPolygon.AddPoint(pt3)

			If d1 > 0.0 Then
				Pt4 = PointAlongPlane(pt3, dirAngle + 180.0 - Betta, d0)
				pt5 = PointAlongPlane(pt0, dirAngle + 180.0 - Alpha, d0)
				LPolygon.AddPoint(Pt4)
				LPolygon.AddPoint(pt5)
				PrimPolygon.AddPoint(Pt4)
			End If

			PrimPolygon.AddPoint(pt3)
			PrimPolygon.AddPoint(pt2)

			'==========RightPolygon
			pt0 = PointAlongPlane(NavFacil.pPtPrj, dirAngle - 90.0, 0.5 * BaseLength)
			pt3 = PointAlongPlane(NavFacil.pPtPrj, dirAngle - 90.0, BaseLength)
			pt1 = PointAlongPlane(pt0, dirAngle - Betta, d1)
			pt2 = PointAlongPlane(pt3, dirAngle - Alpha, d1)

			RPolygon.AddPoint(pt0)
			RPolygon.AddPoint(pt1)
			RPolygon.AddPoint(pt2)
			RPolygon.AddPoint(pt3)

			If d1 > 0.0 Then
				Pt4 = PointAlongPlane(pt3, dirAngle + 180.0 + Alpha, d0)
				pt5 = PointAlongPlane(pt0, dirAngle + 180.0 + Betta, d0)
				RPolygon.AddPoint(Pt4)
				RPolygon.AddPoint(pt5)
			End If

			PrimPolygon.AddPoint(pt1)
			PrimPolygon.AddPoint(pt0)

			If d1 > 0.0 Then PrimPolygon.AddPoint(pt5)
		End If
	End Sub

	Public Function CreatePrjCircle(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, Optional ByVal N As Integer = 360) As ESRI.ArcGIS.Geometry.IPointCollection
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

	Function CreateArcPolylinePrj(ByVal ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint, ByVal ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim I As Integer
		Dim J As Integer
		Dim Pt As ESRI.ArcGIS.Geometry.IPoint
		Dim R As Double
		Dim AngStep As Double
		Dim dX As Double
		Dim dY As Double
		Dim daz As Double
		Dim AztTo As Double
		Dim AztFrom As Double
		Dim iInRad As Double

		Pt = New ESRI.ArcGIS.Geometry.Point
		CreateArcPolylinePrj = New ESRI.ArcGIS.Geometry.Polyline

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

		If (I < 1) Then
			I = 1
		ElseIf (I < 5) Then
			I = 5
		ElseIf (I < 10) Then
			I = 10
		End If

		AngStep = daz / I

		CreateArcPolylinePrj.AddPoint(ptFrom)
		For J = 1 To I - 1
			iInRad = DegToRad(AztFrom + J * AngStep * ClWise)
			Pt.X = ptCnt.X + R * System.Math.Cos(iInRad)
			Pt.Y = ptCnt.Y + R * System.Math.Sin(iInRad)
			CreateArcPolylinePrj.AddPoint(Pt)
		Next J
		CreateArcPolylinePrj.AddPoint(ptTo)
	End Function

	Function CreateArcPrj(ByVal ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint, ByVal ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim I As Integer
		Dim J As Integer
		Dim Pt As ESRI.ArcGIS.Geometry.IPoint
		Dim R As Double
		Dim AngStep As Double
		Dim dX As Double
		Dim dY As Double
		Dim AztFrom As Double
		Dim AztTo As Double
		Dim iInRad As Double
		Dim daz As Double

		Pt = New ESRI.ArcGIS.Geometry.Point
		CreateArcPrj = New ESRI.ArcGIS.Geometry.Polygon

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

		If I < 1 Then
			I = 1
		ElseIf I < 5 Then
			I = 5
		ElseIf I < 10 Then
			I = 10
		End If
		AngStep = daz / I

		CreateArcPrj.AddPoint(ptFrom)
		For J = 1 To I - 1
			iInRad = DegToRad(AztFrom + J * AngStep * ClWise)
			Pt.X = ptCnt.X + R * System.Math.Cos(iInRad)
			Pt.Y = ptCnt.Y + R * System.Math.Sin(iInRad)
			CreateArcPrj.AddPoint(Pt)
		Next J
		CreateArcPrj.AddPoint(ptTo)

	End Function

	Function SpiralTouchAngle(ByVal r0 As Double, ByVal E As Double, ByVal aztNominal As Double, ByVal AztTouch As Double, ByVal TurnDir As Integer) As Double
		Dim I As Integer
		Dim D As Double
		Dim DegE As Double
		Dim delta As Double
		Dim result As Double
		Dim TouchAngle As Double

		DegE = RadToDeg(E)

		TouchAngle = DegToRad(Modulus((AztTouch - aztNominal) * TurnDir, 360.0))
		result = TouchAngle

		For I = 0 To 9
			D = DegE / (r0 + DegE * result)
			delta = (result - TouchAngle - System.Math.Atan(D)) / (2.0 - 1.0 / (D * D + 1.0))
			result = result - delta
			If (System.Math.Abs(delta) < radEps) Then Exit For
		Next I

		Return Modulus(RadToDeg(result), 360.0)
	End Function

	Public Function ReturnAngleInDegrees(ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return Modulus(RadToDeg(Math.Atan2(fdY, fdX)), 360.0)
	End Function

	'Public Function ReturnDistanceFromGeomInMeters(ByVal pFromGeom As ESRI.ArcGIS.Geometry.IGeometry, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
	'	If pFromGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
	'		Return ReturnDistanceInMeters(pFromGeom, ptTo)
	'	End If

	'	Dim pProxy As IProximityOperator		
	'	pProxy = pFromGeom
	'	Return pProxy.ReturnDistance(ptTo)
	'End Function

	Public Function ReturnSquareDistanceInMeters(ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
		Dim fdX, fdY As Double
		fdX = ptTo.X - ptFrom.X
		fdY = ptTo.Y - ptFrom.Y
		Return fdX * fdX + fdY * fdY
	End Function

	Public Function ReturnDistanceInMeters(ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint) As Double
		Return System.Math.Sqrt(ReturnSquareDistanceInMeters(ptFrom, ptTo))
	End Function

	Public Function CalcHorisontalAccuracy(ByVal ptFix As ESRI.ArcGIS.Geometry.IPoint, ByVal GuidanceNav As NavaidData, ByVal IntersectNav As NavaidData) As Double
		Dim sqrt1_2 As Double = 0.5 * Math.Sqrt(2.0)

		Dim fTmp As Double
		Dim IntersectDir As Double
		Dim GuidDir As Double
		Dim GuidDist As Double

		Dim result As Double
		Dim LNavNav As Double
		Dim dNavNav As Double
		Dim sqRoot As Double
		Dim recip As Double

		Dim dX3dY3 As Double
		Dim dX3dT1 As Double
		Dim dX3dL As Double
		Dim dX3dD As Double

		Dim dY3dX3 As Double
		Dim dY3dL As Double
		Dim dY3dT1 As Double
		Dim dY3dT2 As Double
		Dim X3 As Double
		Dim Y3 As Double

		Dim ctT1T2 As Double
		Dim ted1 As Double
		Dim ted2 As Double
		Dim sigL2 As Double
		Dim sigT2 As Double
		Dim sigD2 As Double

		Dim sinT1 As Double
		Dim cosT1 As Double

		Dim sigY3_2 As Double
		Dim sigY3 As Double
		Dim sigX3_2 As Double
		Dim sigX3 As Double

		If (GuidanceNav.TypeCode = eNavaidType.DME) Or (IntersectNav.Identifier = Guid.Empty) Then
			Return 0
		End If

		GuidDir = ReturnAngleInDegrees(GuidanceNav.pPtPrj, ptFix)
		LNavNav = ReturnDistanceInMeters(GuidanceNav.pPtPrj, IntersectNav.pPtPrj)

		If LNavNav < distEps * distEps Then
			sigL2 = 0.5 * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy)
			dNavNav = GuidDir
		Else
			dNavNav = ReturnAngleInDegrees(GuidanceNav.pPtPrj, IntersectNav.pPtPrj)

			Dim dX As Double = IntersectNav.pPtPrj.X - GuidanceNav.pPtPrj.X
			Dim dY As Double = IntersectNav.pPtPrj.Y - GuidanceNav.pPtPrj.Y

			Dim sigX As Double
			Dim sigY As Double

			sigX = 0.5 * dX * dX / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy)
			sigY = 0.5 * dY * dY / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy)

			sigL2 = sigX + sigY

			'============================
			'Dim sigGuidX As Double
			'Dim sigGuidY As Double

			'Dim sigInterX As Double
			'Dim sigInterY As Double

			'sigGuidX = 0.5 * (dX * dX / (LNavNav * LNavNav)) * GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy
			'sigGuidY = 0.5 * (dY * dY / (LNavNav * LNavNav)) * GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy

			'sigInterX = 0.5 * (dX * dX / (LNavNav * LNavNav)) * IntersectNav.HorAccuracy * IntersectNav.HorAccuracy
			'sigInterY = 0.5 * (dY * dY / (LNavNav * LNavNav)) * IntersectNav.HorAccuracy * IntersectNav.HorAccuracy

			'sigL2 = sigGuidX + sigGuidY + sigInterX + sigInterY
		End If

		sigT2 = _settings.AnglePrecision * DegToRadValue
		sigT2 = sigT2 * sigT2

		ted1 = SubtractAngles(dNavNav, GuidDir) * DegToRadValue
		dY3dX3 = Math.Tan(ted1)

		If IntersectNav.TypeCode = eNavaidType.DME Then
			sigD2 = DeConvertDistance(_settings.DistancePrecision)
			sigD2 = sigD2 * sigD2

			GuidDist = ReturnDistanceInMeters(IntersectNav.pPtPrj, ptFix)
			sinT1 = Math.Sin(ted1)
			cosT1 = Math.Cos(ted1)

			sqRoot = Math.Sqrt(GuidDist * GuidDist - LNavNav * LNavNav * sinT1 * sinT1)
			recip = 1.0 / sqRoot

			dX3dL = cosT1 * cosT1 + LNavNav * cosT1 * sinT1 * sinT1 * recip '(14)
			dX3dD = GuidDist * cosT1 * recip '(15)
			dX3dT1 = 2.0 * LNavNav * cosT1 * sinT1 + sinT1 * sqRoot + cosT1 * cosT1 * sinT1 * LNavNav * LNavNav * recip '(16)

			sigX3_2 = dX3dL * dX3dL * sigL2 + dX3dD * dX3dD * sigD2 + dX3dT1 * dX3dT1 * sigT2
			sigX3 = Math.Sqrt(sigX3_2) '(17)

			X3 = LNavNav * cosT1 * cosT1 + cosT1 * sqRoot '(13)
			dY3dT1 = X3 / (cosT1 * cosT1)

			sigY3 = Math.Sqrt(dY3dX3 * dY3dX3 * sigX3_2 + dY3dT1 * dY3dT1 * sigT2)
		Else
			IntersectDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, ptFix)
			ted2 = SubtractAngles(dNavNav, IntersectDir) * DegToRadValue

			dX3dY3 = 1.0 / dY3dX3            '	(7)
			ctT1T2 = dX3dY3 + 1.0 / Math.Tan(ted2)

			dY3dL = 1.0 / ctT1T2
			Y3 = LNavNav * dY3dL

			fTmp = Math.Sin(ted1) * ctT1T2
			dY3dT1 = -LNavNav / (fTmp * fTmp)

			fTmp = Math.Sin(ted1)
			dX3dT1 = -Y3 / (fTmp * fTmp)            '	(8)

			fTmp = Math.Sin(ted2) * ctT1T2
			dY3dT2 = -LNavNav / (fTmp * fTmp)

			sigY3_2 = dY3dL * dY3dL * sigL2 + dY3dT1 * dY3dT1 * sigT2 + dY3dT2 * dY3dT2 * sigT2
			sigY3 = Math.Sqrt(sigY3_2)
			sigX3 = Math.Sqrt(dX3dY3 * dX3dY3 * sigY3_2 + dX3dT1 * dX3dT1 * sigT2)
		End If

		result = Math.Sqrt(sigX3 * sigX3 + sigY3 * sigY3)
		Return result
	End Function

	Public Sub SaveFixAccurasyInfo(reportFile As ReportFile, ptFix As IPoint, FixRole As String, GuidanceNav As NavaidData, IntersectNav As NavaidData, Optional isFinal As Boolean = False)
		'Fix Role				{ FAF, If... }
		'Calculated horizontal accuracy at FIX - in meters

		'Guidance Navaid – Call Sign And Type (RIA VOR)
		'Guidance Navaid Horizontal accuracy - in meters
		'Distance From Guidance Navaid to FIX – in user defined units

		'Intersecting Navaid – Call Sign And Type (RIA VOR)
		'Intersecting Navaid Horizontal accuracy - in meters
		'Distance From Intersecting Navaid to FIX – in user defined units

		If GuidanceNav.Identifier = Guid.Empty Then Return
		If IntersectNav.TypeCode = eNavaidType.NONE Then Return

		If GuidanceNav.TypeCode = eNavaidType.DME Then Return
		If IntersectNav.IntersectionType = eIntersectionType.OnNavaid Then Return

		Dim HorAccuracy As Double = CalcHorisontalAccuracy(ptFix, GuidanceNav, IntersectNav)

		reportFile.Param("Fix Role", FixRole)
		reportFile.Param("Calculated horizontal accuracy at FIX", HorAccuracy.ToString("0.00"), "meters")
		reportFile.WriteMessage()

		reportFile.Param("Guidance Navaid", GuidanceNav.CallSign + "/" + GuidanceNav.TypeCode.ToString())
		reportFile.Param("Guidance Navaid Horizontal accuracy", GuidanceNav.HorAccuracy.ToString("0.00"), "meters")

		Dim distance As Double = ReturnDistanceInMeters(ptFix, GuidanceNav.pPtPrj)
		reportFile.Param("Distance From Guidance Navaid to FIX", ConvertDistance(distance), DistanceConverter(DistanceUnit).Unit)
		reportFile.WriteMessage()

		reportFile.Param("Intersecting Navaid", IntersectNav.CallSign + "/" + IntersectNav.TypeCode.ToString())
		reportFile.Param("Intersecting Navaid Horizontal accuracy", IntersectNav.HorAccuracy.ToString("0.00"), "meters")

		distance = ReturnDistanceInMeters(ptFix, IntersectNav.pPtPrj)
		reportFile.Param("Distance From Intersecting Navaid to FIX", ConvertDistance(distance), DistanceConverter(DistanceUnit).Unit)

		If Not isFinal Then
			reportFile.WriteMessage("=================================================")
			reportFile.WriteMessage()
		End If
	End Sub

	Function SubtractAngles(ByVal X As Double, ByVal Y As Double) As Double
		SubtractAngles = Modulus(X - Y, 360.0)
		If SubtractAngles > 180.0 Then SubtractAngles = 360.0 - SubtractAngles
	End Function

	Function SubtractAnglesWithSign(ByVal StRad As Double, ByVal EndRad As Double, ByVal Turn As Integer) As Double
		SubtractAnglesWithSign = Modulus((EndRad - StRad) * Turn, 360.0)
		If SubtractAnglesWithSign > 180.0 Then
			SubtractAnglesWithSign = SubtractAnglesWithSign - 360.0
		End If
	End Function

	Public Function Quadric(ByVal A As Double, ByVal B As Double, ByVal C As Double, ByRef X0 As Double, ByRef X1 As Double) As Integer
		Dim D As Double
		Dim fTmp As Double

		D = B * B - 4 * A * C
		If D < 0.0 Then
			Quadric = 0
		ElseIf (D = 0.0) Or (A = 0.0) Then
			Quadric = 1
			If A = 0.0 Then
				X0 = -C / B
			Else
				X0 = -0.5 * B / A
			End If
		Else
			Quadric = 2
			fTmp = 0.5 / A
			If fTmp > 0 Then
				X0 = (-B - System.Math.Sqrt(D)) * fTmp
				X1 = (-B + System.Math.Sqrt(D)) * fTmp
			Else
				X0 = (-B + System.Math.Sqrt(D)) * fTmp
				X1 = (-B - System.Math.Sqrt(D)) * fTmp
			End If
		End If

	End Function

	Public Function CutNavPoly(ByVal Poly As ESRI.ArcGIS.Geometry.Polygon, ByVal CutLine As ESRI.ArcGIS.Geometry.IPolyline, ByVal Side As Integer) As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pUnspecified As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pRight As ESRI.ArcGIS.Geometry.Polygon
		Dim pLeft As ESRI.ArcGIS.Geometry.Polygon
		Dim pLine As ESRI.ArcGIS.Geometry.ILine
		Dim Dir_Renamed As Double

		pLine = New ESRI.ArcGIS.Geometry.Line
		pLine.FromPoint = CutLine.FromPoint
		pLine.ToPoint = CutLine.ToPoint

		ClipByLine(Poly, CutLine, pLeft, pRight, pUnspecified)
		CutNavPoly = New ESRI.ArcGIS.Geometry.Polygon
		Dir_Renamed = RadToDeg(pLine.Angle)

		If Side > 0 Then
			pGeo = pLeft
			If Not pGeo.IsEmpty() Then
				pGeo = pUnspecified
				If Not pGeo.IsEmpty() Then '            If pArea.Area > 0.5 Then
					If SideDef(pLine.FromPoint, Dir_Renamed, pUnspecified.Point(0)) < 0 Then
						pTopoOper = pLeft
						CutNavPoly = pTopoOper.Union(pUnspecified)
					End If
				End If
				CutNavPoly = pLeft
			Else
				pGeo = pUnspecified
				If Not pGeo.IsEmpty() Then
					If SideDef(pLine.FromPoint, Dir_Renamed, pUnspecified.Point(0)) < 0 Then
						CutNavPoly = pUnspecified
					End If
				End If
			End If
		Else
			pGeo = pRight
			If Not pGeo.IsEmpty() Then
				pGeo = pUnspecified
				If Not pGeo.IsEmpty() Then '       If pArea.Area > 0.5 Then
					If SideDef(pLine.FromPoint, Dir_Renamed, pUnspecified.Point(0)) > 0 Then
						pTopoOper = pRight
						CutNavPoly = pTopoOper.Union(pUnspecified)
					End If
				End If
				CutNavPoly = pRight '        If Not pGeo.IsEmpty() Then
			Else
				pGeo = pUnspecified
				If Not pGeo.IsEmpty() Then
					If SideDef(pLine.FromPoint, Dir_Renamed, pUnspecified.Point(0)) > 0 Then
						CutNavPoly = pUnspecified
					End If
				End If
			End If
		End If

		pTopoOper = CutNavPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
	End Function

	Public Sub CutPoly(ByRef Poly As ESRI.ArcGIS.Geometry.Polygon, ByVal CutLine As ESRI.ArcGIS.Geometry.Polyline, ByVal Side As Integer)
		Dim Geocollect As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Cutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim tmpAzt As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim Dist As Double
		Dim GIx As Integer
		Dim Ix As Integer
		'=================================
		Dim pLeft As ESRI.ArcGIS.Geometry.Polygon
		Dim pRight As ESRI.ArcGIS.Geometry.Polygon
		Dim pUnspecified As ESRI.ArcGIS.Geometry.Polygon
		'=================================
		On Error GoTo ErrorHandler

		Cutter = CutLine

		tmpAzt = ReturnAngleInDegrees(Cutter.FromPoint, Cutter.ToPoint)
		Dist = ReturnDistanceInMeters(Cutter.FromPoint, Cutter.ToPoint)

		pTopo = Poly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()
		tmpPoly = ClipByPoly(Cutter, Poly)

		If tmpPoly.PointCount <> 0 Then
			Geocollect = tmpPoly
			If Geocollect.GeometryCount > 1 Then
				For Ix = 0 To Geocollect.GeometryCount - 1
					tmpPoly = Geocollect.Geometry(Ix)
					Dist0 = ReturnDistanceInMeters(tmpPoly.Point(0), Cutter.FromPoint)
					Dist1 = ReturnDistanceInMeters(tmpPoly.Point(1), Cutter.FromPoint)
					If Dist0 > Dist1 Then Dist0 = Dist1

					If Dist > Dist0 Then
						GIx = Ix
						Dist = Dist0
					End If
				Next Ix

				tmpPoly = Geocollect.Geometry(GIx)
				Dist = ReturnDistanceInMeters(tmpPoly.Point(0), Cutter.FromPoint)
				Dist0 = ReturnDistanceInMeters(tmpPoly.Point(1), Cutter.FromPoint)

				If Dist < Dist0 Then Dist = Dist0
				ptTmp = PointAlongPlane(Cutter.FromPoint, tmpAzt, Dist + 5.0)
				Cutter.ToPoint = ptTmp
			End If

			ClipByLine(Poly, Cutter, pLeft, pRight, pUnspecified)

			If Side < 0 Then
				Poly = pRight
			Else
				Poly = pLeft
			End If
		End If

		On Error GoTo 0

		Return

ErrorHandler:  ' Error-handling routine.
		DrawPolyLine(Cutter, 255)
		DrawPolygon(Poly, RGB(255, 255, 0))
		MessageBox.Show(". ErrNum = " + CStr(Err.Number) + ": " + Err.Description)

		'If Err.Number = -2147220943 Then
		'	ptTmp = PointAlongPlane(Cutter.ToPoint, tmpAzt + 90.0 * Side, 0.01)
		'	Cutter.ToPoint = ptTmp
		'	Resume	' Resume execution at same line that caused the error.
		'Else ' If Err.Number <> -2147220968 Then
		'	MsgBox("Ошибка среды 'ArcMap' фирмы 'ESRI' !!!")
		'	Return
		'	Resume Next	' Resume execution at next line that caused the error.
		'End If
	End Sub

	Public Function LineLineIntersect(ByVal pt1 As ESRI.ArcGIS.Geometry.Point, ByVal Dir1 As Double, ByVal pt2 As ESRI.ArcGIS.Geometry.Point, ByVal Dir2 As Double) As ESRI.ArcGIS.Geometry.Point
		Dim Constructor As ESRI.ArcGIS.Geometry.IConstructPoint
		LineLineIntersect = New ESRI.ArcGIS.Geometry.Point
		Constructor = LineLineIntersect
		Constructor.ConstructAngleIntersection(pt1, DegToRad(Dir1), pt2, DegToRad(Dir2))
	End Function

	Public Function CircleVectorIntersect(ByVal PtCent As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByVal ptVect As ESRI.ArcGIS.Geometry.IPoint, ByVal DirVect As Double, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim DistCnt2Vect As Double
		Dim D As Double
		Dim Constr As ESRI.ArcGIS.Geometry.IConstructPoint

		ptTmp = New ESRI.ArcGIS.Geometry.Point
		Constr = ptTmp

		Constr.ConstructAngleIntersection(PtCent, DegToRad(DirVect + 90.0), ptVect, DegToRad(DirVect))

		DistCnt2Vect = ReturnDistanceInMeters(PtCent, ptTmp)

		If DistCnt2Vect < R Then
			D = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect)
			ptRes = PointAlongPlane(ptTmp, DirVect, D)
			CircleVectorIntersect = D ' ReturnDistanceInMeters(ptRes, ptTmp)
		Else
			CircleVectorIntersect = 0.0
			ptRes = New ESRI.ArcGIS.Geometry.Point
		End If

	End Function

	Public Function CircleCircleIntersect(ByVal PtCent1 As ESRI.ArcGIS.Geometry.IPoint, ByVal R1 As Double, ByVal PtCent2 As ESRI.ArcGIS.Geometry.IPoint, ByVal R2 As Double, ByVal TurnDir As Integer, Optional ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint = Nothing) As Double
		Dim Dist As Double
		Dim H As Double
		Dim A As Double
		Dim X2 As Double
		Dim Y2 As Double

		Dist = ReturnDistanceInMeters(PtCent1, PtCent2)
		ptRes = New ESRI.ArcGIS.Geometry.Point
		If Dist <= R1 + R2 Then
			A = (R1 * R1 - R2 * R2 + Dist * Dist) / (2 * Dist)
			H = System.Math.Sqrt(R1 * R1 - A * A)
			X2 = PtCent1.X + A * (PtCent2.X - PtCent1.X) / Dist
			Y2 = PtCent1.Y + A * (PtCent2.Y - PtCent1.Y) / Dist

			ptRes.X = X2 + TurnDir * H * (PtCent2.Y - PtCent1.Y) / Dist
			ptRes.Y = Y2 - TurnDir * H * (PtCent2.X - PtCent1.X) / Dist
			CircleCircleIntersect = H
		Else
			CircleCircleIntersect = -1.0
		End If
	End Function

	Function FindCommonTochCircle(ByVal pPolygon As ESRI.ArcGIS.Geometry.IPolygon, ByVal fAxis As Double,
	   ByVal fTuchDist As Double, ByVal iTurnDir As Integer, ByRef pResPt As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.Point
		Dim pProximityoperator As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pConstructPoint As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim fDist As Double

		Dim fdX As Double
		Dim fdY As Double
		Dim pTuchPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim I As Integer
		Dim J As Integer

		pProximityoperator = pPolygon
		pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)

		fdX = pTuchPt.X - pResPt.X
		fdY = pTuchPt.Y - pResPt.Y
		fDist = System.Math.Sqrt(fdX * fdX + fdY * fdY)

		I = 0
		J = 0

		Do While System.Math.Abs(fDist - fTuchDist) > 0.25

			'    If fDist <= fTuchDist Then
			'        CircleVectorIntersect pTuchPt, fTuchDist, ptLine, fAxis, pPtTmp
			If CircleVectorIntersect(pTuchPt, fTuchDist, pResPt, fAxis, pPtTmp) > 0.0 Then
				pResPt.PutCoords(pPtTmp.X, pPtTmp.Y)
				J = 0
				I = I + 1
			Else
				pConstructPoint = pPtTmp
				pConstructPoint.ConstructAngleIntersection(pResPt, DegToRad(fAxis), pTuchPt, DegToRad(fAxis + 90.0))
				pResPt.PutCoords(pPtTmp.X, pPtTmp.Y)
				J = J + 1
			End If

			pTuchPt = pProximityoperator.ReturnNearestPoint(pResPt, ESRI.ArcGIS.Geometry.esriSegmentExtension.esriNoExtension)

			If (I > 10) Or (J > 5) Then Return pTuchPt

			fdX = pTuchPt.X - pResPt.X
			fdY = pTuchPt.Y - pResPt.Y
			fDist = System.Math.Sqrt(fdX * fdX + fdY * fdY)
		Loop

		Return pTuchPt
	End Function

	'Public Function SideDefEps(ByVal PtInLine As ESRI.ArcGIS.Geometry.IPoint, ByVal LineAngle As Double, ByVal PtTest As ESRI.ArcGIS.Geometry.IPoint) As Integer
	'	Dim Angle12 As Double
	'	Dim dAngle As Double
	'	Dim fDist As Double
	'	Dim fdX As Double
	'	Dim fdY As Double

	'	SideDefEps = 0

	'	fdX = PtTest.X - PtInLine.X
	'	fdY = PtTest.Y - PtInLine.Y

	'	fDist = fdY * fdY + fdX * fdX
	'	If fDist < distEps * distEps Then Exit Function

	'	Angle12 = RadToDeg(ATan2(fdY, fdX))
	'	dAngle = Modulus(LineAngle - Angle12, 360.0)

	'	If dAngle > degEps Then
	'		If (dAngle < 180.0 - degEps) Then
	'			SideDefEps = 1
	'		ElseIf (dAngle > 180.0 + degEps) Then
	'			SideDefEps = -1
	'		End If
	'	End If
	'End Function

	Public Function SideDef(ByVal PtInLine As ESRI.ArcGIS.Geometry.IPoint, ByVal LineAngle As Double, ByVal PtTest As ESRI.ArcGIS.Geometry.IPoint) As eSide
		Dim Angle12 As Double
		Dim dAngle As Double
		Dim fDist As Double
		Dim fdX As Double
		Dim fdY As Double

		fdX = PtTest.X - PtInLine.X
		fdY = PtTest.Y - PtInLine.Y
		fDist = fdY * fdY + fdX * fdX

		If fDist < distEps * distEps Then Return eSide.OnLine

		Angle12 = RadToDeg(Math.Atan2(fdY, fdX))
		dAngle = Modulus(LineAngle - Angle12, 360.0)

		'If (dAngle < degEps) Or (Math.Abs(dAngle - 180.0) < degEps) Then Return eSide.OnLine
		If dAngle = 0.0 Then Return eSide.OnLine

		If (dAngle < 180.0) Then Return eSide.Right '- degEps Sag

		'ElseIf (dAngle > 180.0) Then  '+ degEps Sol
		Return eSide.Left
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

	Sub CreateWindSpiral(ByVal Pt As ESRI.ArcGIS.Geometry.IPoint, ByVal aztNom As Double, ByVal AztSt As Double,
	   ByVal AztEnd As Double, ByVal r0 As Double, ByVal coef As Double, ByVal TurnDir As Integer, ByRef pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection)
		Dim N As Integer
		Dim I As Integer

		Dim R As Double
		Dim azt0 As Double
		Dim dPhi As Double
		Dim dPhi0 As Double
		Dim dAlpha As Double
		Dim TurnAng As Double

		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint

		ptCnt = PointAlongPlane(Pt, aztNom + 90.0 * TurnDir, r0)

		If SubtractAngles(aztNom, AztEnd) < degEps Then AztEnd = aztNom

		dPhi0 = (AztSt - aztNom) * TurnDir
		dPhi0 = Modulus(dPhi0, 360.0)

		If (dPhi0 < 0.001) Then
			dPhi0 = 0.0
		Else
			dPhi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, TurnDir)
		End If

		'DrawPolygon pPointCollection, 0

		dPhi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, TurnDir)

		TurnAng = dPhi - dPhi0

		azt0 = aztNom + (dPhi0 - 90.0) * TurnDir
		azt0 = Modulus(azt0, 360.0)

		If (TurnAng < 0.0) Then Return

		dAlpha = 1.0
		N = CInt(Math.Ceiling(TurnAng / dAlpha))

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
			pPointCollection.AddPoint(ptCur)
		Next I
	End Sub

	Public Function RayPolylineIntersect(ByVal pPolyline As ESRI.ArcGIS.Geometry.Polyline, ByVal RayPt As ESRI.ArcGIS.Geometry.Point, ByVal RayDir As Double, ByRef InterPt As ESRI.ArcGIS.Geometry.IPoint) As Boolean
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

		pTopo = pPolyline
		pPoints = pTopo.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
		N = pPoints.PointCount

		If N = 0 Then Return False

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

		Return True
	End Function

	Function AngleInSector(ByVal angle As Double, ByVal X As Double, ByVal Y As Double) As Boolean
		X = Modulus(X)
		Y = Modulus(Y)
		angle = Modulus(angle)

		If (X > Y) Then
			If ((angle >= X) Or (angle <= Y)) Then Return True
		Else
			If ((angle >= X) And (angle <= Y)) Then Return True
		End If

		Return False
	End Function

	Function AngleInInterval(Ang As Double, inter As Interval) As Boolean

		If inter.Left = -2 Then Return False

		If inter.Right = -1 Then
			If System.Math.Round(inter.Left, 1) = System.Math.Round(Ang, 1) Then Return True
			Return False
		End If

		inter.Left = NativeMethods.Modulus(inter.Left)
		inter.Right = NativeMethods.Modulus(inter.Right)
		Ang = NativeMethods.Modulus(Ang)

		If inter.Left > inter.Right Then
			If (Ang >= inter.Left) Or (Ang <= inter.Right) Then Return True
		Else
			If (Ang >= inter.Left) And (Ang <= inter.Right) Then Return True
		End If

		Return False
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

	Public Function DrawPolygon(ByRef pPolygon As ESRI.ArcGIS.Geometry.Polygon, Optional ByVal FillColor As Integer = -1, Optional ByVal FillStyle As ESRI.ArcGIS.Display.esriSimpleFillStyle = ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSNull, Optional ByVal drawFlg As Boolean = True, Optional ByVal OutlineWidth As Integer = 1, Optional ByRef OutlineColor As Integer = -2) As ESRI.ArcGIS.Carto.IElement
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
		pFillSym.Style = FillStyle

		pLineSym = New ESRI.ArcGIS.Display.SimpleLineSymbol

		If OutlineColor = -1 Then
			pRGB.Red = System.Math.Round(255 * Rnd())
			pRGB.Green = System.Math.Round(255 * Rnd())
			pRGB.Blue = System.Math.Round(255 * Rnd())
			'ElseIf OutlineColor = -2 Then
		ElseIf OutlineColor >= 0 Then
			pRGB.RGB = OutlineColor
		End If

		pLineSym.Width = OutlineWidth
		pLineSym.Color = pRGB
		pFillSym.Outline = pLineSym

		pElementofPoly = pFillShpElement
		pElementofPoly.Geometry = pPolygon

		pFillShpElement.Symbol = pFillSym

		If drawFlg Then
			Dim pGraphics As ESRI.ArcGIS.Carto.IGraphicsContainer
			pGraphics = GetActiveView().GraphicsContainer
			pGraphics.AddElement(pElementofPoly, 0)
			GetActiveView().PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
		End If

		Return pElementofPoly
	End Function

	Public Function CreateSectorPrj(ByVal pPtCenter As ESRI.ArcGIS.Geometry.IPoint, ByVal R As Double, ByVal stDir As Double, ByVal endDir As Double, ByVal ClWise As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint

		ptFrom = PointAlongPlane(pPtCenter, stDir, R)
		ptTo = PointAlongPlane(pPtCenter, endDir, R)

		CreateSectorPrj = CreateArcPrj(pPtCenter, ptFrom, ptTo, ClWise)
		CreateSectorPrj.AddPoint(pPtCenter)
		CreateSectorPrj.AddPoint(pPtCenter, 0)
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

	Function DegToRad(ByVal X As Double) As Double
		DegToRad = X * DegToRadValue
	End Function

	Function RadToDeg(ByVal X As Double) As Double
		RadToDeg = X * RadToDegValue
	End Function

	Function OpenTableFromFile(ByRef pTable As ESRI.ArcGIS.Geodatabase.ITable, ByVal sFolderName As String, ByVal sFileName As String) As Boolean
		On Error GoTo EH

		Dim pFact As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory
		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace
		Dim pFeatWs As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim sPath As String

		sPath = sFolderName

		pFact = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory
		pWorkspace = pFact.OpenFromFile(sPath, GetApplicationHWnd())
		pFeatWs = pWorkspace
		pTable = pFeatWs.OpenTable(sFileName)

		Return True
EH:
		ErrorStr = Err.Number + "  " + Err.Description
		'    msgbox Err.Number + "  " + Err.Description
		Return False
	End Function

	Sub SortIntervals(ByRef Intervals() As Interval, Optional ByVal RightSide As Boolean = False)
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim Tmp As Interval

		N = UBound(Intervals)

		For I = 0 To N - 1
			For J = I + 1 To N
				If RightSide Then
					If Intervals(I).Right > Intervals(J).Right Then
						Tmp = Intervals(I)
						Intervals(I) = Intervals(J)
						Intervals(J) = Tmp
					End If
				Else
					If Intervals(I).Left > Intervals(J).Left Then
						Tmp = Intervals(I)
						Intervals(I) = Intervals(J)
						Intervals(J) = Tmp
					End If
				End If
			Next J
		Next I
	End Sub

	Private Sub SortByDist(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = 0.0
		Mid_Renamed = A.Parts((Lo + Hi) / 2).Dist
		Do
			While A.Parts(Lo).Dist < Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).Dist > Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByDist(A, iLo, Hi)
		If (Lo < iHi) Then SortByDist(A, Lo, iHi)
	End Sub

	Private Sub SortByTurnDist(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).TurnDistL

		Do
			While A.Parts(Lo).TurnDistL < Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).TurnDistL > Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByTurnDist(A, iLo, Hi)
		If (Lo < iHi) Then SortByTurnDist(A, Lo, iHi)
	End Sub

	Private Sub SortByReqH(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).ReqH

		Do
			While A.Parts(Lo).ReqH > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).ReqH < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByReqH(A, iLo, Hi)
		If (Lo < iHi) Then SortByReqH(A, Lo, iHi)
	End Sub

	Private Sub SortByReqOCH(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).ReqOCH

		Do
			While A.Parts(Lo).ReqOCH > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).ReqOCH < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByReqOCH(A, iLo, Hi)
		If (Lo < iHi) Then SortByReqOCH(A, Lo, iHi)
	End Sub

	Private Sub SortByMoc(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).MOC

		Do
			While A.Parts(Lo).MOC > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).MOC < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByMoc(A, iLo, Hi)
		If (Lo < iHi) Then SortByMoc(A, Lo, iHi)
	End Sub

	Private Sub SortByEffectiveHeight(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).EffectiveHeight

		Do
			While A.Parts(Lo).EffectiveHeight > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).EffectiveHeight < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByEffectiveHeight(A, iLo, Hi)
		If (Lo < iHi) Then SortByEffectiveHeight(A, Lo, iHi)
	End Sub

	Private Sub SortByHPenetrate(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).hPenet

		Do
			While A.Parts(Lo).hPenet > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).hPenet < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByHPenetrate(A, iLo, Hi)
		If (Lo < iHi) Then SortByHPenetrate(A, Lo, iHi)
	End Sub

	'Private Sub SortByfTmp(ByRef A() As ObstacleAr, ByVal iLo As Integer, ByVal iHi As Integer)
	'	Dim T As ObstacleAr
	'	Dim Mid_Renamed As Double
	'	Dim Lo As Integer
	'	Dim Hi As Integer

	'	Lo = iLo
	'	Hi = iHi
	'	Mid_Renamed = A((Lo + Hi) / 2).fTmp

	'	Do
	'		While A(Lo).fTmp < Mid_Renamed
	'			Lo = Lo + 1
	'		End While

	'		While A(Hi).fTmp > Mid_Renamed
	'			Hi = Hi - 1
	'		End While

	'		If (Lo <= Hi) Then
	'			T = A(Lo)
	'			A(Lo) = A(Hi)
	'			A(Hi) = T
	'			Lo = Lo + 1
	'			Hi = Hi - 1
	'		End If
	'	Loop Until Lo > Hi

	'	If (Hi > iLo) Then SortByfTmp(A, iLo, Hi)
	'	If (Lo < iHi) Then SortByfTmp(A, Lo, iHi)
	'End Sub

	Private Sub SortByfSort(ByRef A As ObstacleContainer, ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleData
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A.Parts((Lo + Hi) / 2).fSort

		Do
			While A.Parts(Lo).fSort > Mid_Renamed
				Lo = Lo + 1
			End While
			While A.Parts(Hi).fSort < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A.Parts(Lo)
				A.Parts(Lo) = A.Parts(Hi)
				A.Parts(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortByfSort(A, iLo, Hi)
		If (Lo < iHi) Then SortByfSort(A, Lo, iHi)
	End Sub

	Public Sub Sort(ByRef A As ObstacleContainer, ByVal SortIx As Integer)
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = LBound(A.Parts)
		Hi = UBound(A.Parts)

		If (Lo >= Hi) Then Return

		Select Case SortIx
			Case 0
				SortByDist(A, Lo, Hi)
			Case 1
				SortByTurnDist(A, Lo, Hi)
			Case 2
				SortByReqH(A, Lo, Hi)
			Case 3
				SortByReqOCH(A, Lo, Hi)
			Case 4
				SortByEffectiveHeight(A, Lo, Hi)
			Case 5
				SortByHPenetrate(A, Lo, Hi)
				'Case 4
				'	SortByfTmp(A, Lo, Hi)
			Case 6
				SortByMoc(A, Lo, Hi)
			Case 100
				SortByfSort(A, Lo, Hi)
				'    Case 101
				'        SortBysSort A, Lo, Hi
		End Select
	End Sub

	Public Sub SortMSAByReqH(ByRef A As ObstacleMSA(), ByVal iLo As Integer, ByVal iHi As Integer)
		Dim T As ObstacleMSA
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A((Lo + Hi) / 2).ReqH

		Do
			While A(Lo).ReqH > Mid_Renamed
				Lo = Lo + 1
			End While
			While A(Hi).ReqH < Mid_Renamed
				Hi = Hi - 1
			End While
			If (Lo <= Hi) Then
				T = A(Lo)
				A(Lo) = A(Hi)
				A(Hi) = T
				Lo = Lo + 1
				Hi = Hi - 1
			End If
		Loop Until Lo > Hi

		If (Hi > iLo) Then SortMSAByReqH(A, iLo, Hi)
		If (Lo < iHi) Then SortMSAByReqH(A, Lo, iHi)
	End Sub


	Public Sub shall_SortfSort(ByRef obsArray As ObstacleContainer)
		Dim TempVal As ObstacleData
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray.Parts)
		LastRow = UBound(obsArray.Parts)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray.Parts(I)
				Do While obsArray.Parts(CurPos - GapSize).fSort > TempVal.fSort
					obsArray.Parts(CurPos) = obsArray.Parts(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray.Parts(CurPos) = TempVal
			Next I
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortfSortD(ByRef obsArray As ObstacleContainer)
		Dim TempVal As ObstacleData
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray.Parts)
		LastRow = UBound(obsArray.Parts)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray.Parts(I)
				Do While obsArray.Parts(CurPos - GapSize).fSort < TempVal.fSort
					obsArray.Parts(CurPos) = obsArray.Parts(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray.Parts(CurPos) = TempVal
			Next I
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortsSort(ByRef obsArray As ObstacleContainer)
		Dim TempVal As ObstacleData
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray.Parts)
		LastRow = UBound(obsArray.Parts)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray.Parts(I)
				Do While obsArray.Parts(CurPos - GapSize).sSort > TempVal.sSort
					obsArray.Parts(CurPos) = obsArray.Parts(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray.Parts(CurPos) = TempVal
			Next I
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortsSortD(ByRef obsArray As ObstacleContainer)
		Dim TempVal As ObstacleData
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray.Parts)
		LastRow = UBound(obsArray.Parts)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

		Do
			GapSize = GapSize * 3 + 1
		Loop Until GapSize > NumRows

		Do
			GapSize = GapSize \ 3
			For I = (GapSize + FirstRow) To LastRow
				CurPos = I
				TempVal = obsArray.Parts(I)
				Do While obsArray.Parts(CurPos - GapSize).sSort < TempVal.sSort
					obsArray.Parts(CurPos) = obsArray.Parts(CurPos - GapSize)
					CurPos = CurPos - GapSize
					If (CurPos - GapSize) < FirstRow Then Exit Do
				Loop
				obsArray.Parts(CurPos) = TempVal
			Next I
		Loop Until GapSize <= 1
	End Sub

	' MSA =================
	Public Sub shall_SortfSort(ByRef obsArray() As ObstacleMSA)
		Dim TempVal As ObstacleMSA
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

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
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortfSortD(ByRef obsArray() As ObstacleMSA)
		Dim TempVal As ObstacleMSA
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

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
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortsSort(ByRef obsArray() As ObstacleMSA)
		Dim TempVal As ObstacleMSA
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

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
		Loop Until GapSize <= 1
	End Sub

	Public Sub shall_SortsSortD(ByRef obsArray() As ObstacleMSA)
		Dim TempVal As ObstacleMSA
		Dim GapSize As Integer
		Dim I As Integer
		Dim CurPos As Integer
		Dim LastRow As Integer
		Dim FirstRow As Integer
		Dim NumRows As Integer

		FirstRow = LBound(obsArray)
		LastRow = UBound(obsArray)
		NumRows = LastRow - FirstRow + 1

		If NumRows = 0 Then Return

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
		Loop Until GapSize <= 1
	End Sub

	'Public Sub ShellSort(ByRef Arr() As Double, ByVal N As Long)
	'Dim C As Boolean
	'Dim E As Long
	'Dim G As Long
	'Dim I As Long
	'Dim J As Long
	'Dim Tmp As Double

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

	Public Function ReturnNearestPoint(ByVal pGeometry As ESRI.ArcGIS.Geometry.IGeometry, ByVal pLinePt As ESRI.ArcGIS.Geometry.IPoint, ByVal direction As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim I As Integer
		Dim N As Integer
		Dim CurrDist As Double
		Dim Distance As Double
		Dim pPolyline As IPolyline
		Dim pIntersections As IPointCollection
		Dim pTopo As ITopologicalOperator2

		Dim pPoints As IPointCollection
		Dim Result As ESRI.ArcGIS.Geometry.IPoint

		Result = Nothing

		pPoints = pGeometry

		pPolyline = New Polyline()
		pPolyline.FromPoint = PointAlongPlane(pLinePt, direction, 1000000.0)
		pPolyline.ToPoint = PointAlongPlane(pLinePt, direction, -1000000.0)
		pTopo = pPolyline

		pIntersections = pTopo.Intersect(pGeometry, esriGeometryDimension.esriGeometry0Dimension)
		pPoints.AddPointCollection(pIntersections)

		pIntersections = pTopo.Intersect(pGeometry, esriGeometryDimension.esriGeometry1Dimension)
		pPoints.AddPointCollection(pIntersections)

		N = pPoints.PointCount - 1
		Distance = Double.MaxValue

		For I = 0 To N
			CurrDist = Point2LineDistancePrj(pPoints.Point(I), pLinePt, direction)
			If CurrDist < Distance Then
				Distance = CurrDist
				Result = pPoints.Point(I)
			End If
		Next

		Return Result
	End Function

	'Public Function Geometry2LineDistancePrj(ByVal pGeometry As ESRI.ArcGIS.Geometry.IGeometry, ByVal pLinePt As ESRI.ArcGIS.Geometry.IPoint, ByVal direction As Double) As Double
	'	If pGeometry.GeometryType = esriGeometryType.esriGeometryPoint Then Return Point2LineDistancePrj(pGeometry, pLinePt, direction)

	'	Dim pPolyline As IPolyline
	'	Dim pProxy As IProximityOperator

	'	pPolyline = New Polyline()		Point2LineDistancePrj
	'	pPolyline.FromPoint = PointAlongPlane(pLinePt, direction, 1000000.0)
	'	pPolyline.ToPoint = PointAlongPlane(pLinePt, direction, -1000000.0)
	'	pProxy = pPolyline

	'	Return pProxy.ReturnDistance(pGeometry)
	'End Function

	Public Function Point2LineDistanceSigned(ByVal pInPt As ESRI.ArcGIS.Geometry.IPoint, ByVal pLinePt As ESRI.ArcGIS.Geometry.IPoint, ByVal direction As Double) As Double
		Dim CosA As Double
		Dim SinA As Double
		Dim dX As Double
		Dim dY As Double

		direction = DegToRad(direction)
		CosA = System.Math.Cos(direction)
		SinA = System.Math.Sin(direction)
		dX = pInPt.X - pLinePt.X
		dY = pInPt.Y - pLinePt.Y

		Return dY * CosA - dX * SinA
	End Function

	Public Function Point2LineDistancePrj(ByVal pInPt As ESRI.ArcGIS.Geometry.IPoint, ByVal pLinePt As ESRI.ArcGIS.Geometry.IPoint, ByVal direction As Double) As Double
		Dim CosA As Double
		Dim SinA As Double
		Dim dX As Double
		Dim dY As Double

		direction = DegToRad(direction)
		CosA = System.Math.Cos(direction)
		SinA = System.Math.Sin(direction)
		dX = pInPt.X - pLinePt.X
		dY = pInPt.Y - pLinePt.Y

		Return System.Math.Abs(dY * CosA - dX * SinA)
	End Function

	Sub GetObstInRange(ByRef ObstSource As ObstacleContainer, ByRef ObstDest As ObstacleContainer, ByVal Range As Double)
		Dim I As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer

		N = UBound(ObstSource.Parts)

		ReDim ObstDest.Parts(N)
		If N < 0 Then Return

		M = UBound(ObstSource.Obstacles)
		ReDim ObstDest.Obstacles(M)
		For I = 0 To M
			ObstSource.Obstacles(I).NIx = -1
		Next

		K = -1
		L = -1
		For I = 0 To N
			If ObstSource.Parts(I).Dist > Range Then Exit For
			K += 1
			ObstDest.Parts(K) = ObstSource.Parts(I)

			If ObstSource.Obstacles(ObstSource.Parts(I).Owner).NIx < 0 Then
				L += 1
				ObstDest.Obstacles(L) = ObstSource.Obstacles(ObstSource.Parts(I).Owner)
				ObstDest.Obstacles(L).PartsNum = 0
				ReDim ObstDest.Obstacles(L).Parts(ObstSource.Obstacles(ObstSource.Parts(I).Owner).PartsNum - 1)
				ObstSource.Obstacles(ObstSource.Parts(I).Owner).NIx = L
			End If

			ObstDest.Parts(K).Owner = ObstSource.Obstacles(ObstSource.Parts(I).Owner).NIx
			ObstDest.Parts(K).Index = ObstDest.Obstacles(ObstDest.Parts(K).Owner).PartsNum
			ObstDest.Obstacles(ObstDest.Parts(K).Owner).Parts(ObstDest.Parts(K).Index) = K
			ObstDest.Obstacles(ObstDest.Parts(K).Owner).PartsNum += 1
		Next I

		If K < 0 Then
			ReDim ObstDest.Parts(-1)
			ReDim ObstDest.Obstacles(-1)
		Else
			ReDim Preserve ObstDest.Parts(K)
			ReDim Preserve ObstDest.Obstacles(L)
		End If
	End Sub

	'Public Function LineVectIntersect(ByRef pt1 As ESRI.ArcGIS.Geometry.IPoint, ByRef pt2 As ESRI.ArcGIS.Geometry.IPoint, ByRef pt3 As ESRI.ArcGIS.Geometry.IPoint, ByRef azt As Double, ByRef ptRes As ESRI.ArcGIS.Geometry.IPoint) As Integer
	'	Dim Az As Double
	'	Dim SinAz As Double
	'	Dim CosAz As Double
	'	Dim UaDenom As Double
	'	Dim UaNumer As Double
	'	Dim Ua As Double

	'	Az = DegToRad(azt)
	'	SinAz = System.Math.Sin(Az)
	'	CosAz = System.Math.Cos(Az)

	'	ptRes = New ESRI.ArcGIS.Geometry.Point

	'	UaDenom = SinAz * (pt2.X - pt1.X) - CosAz * (pt2.Y - pt1.Y)
	'	If UaDenom = 0.0 Then Return -2

	'	UaNumer = CosAz * (pt1.Y - pt3.Y) - SinAz * (pt1.X - pt3.X)

	'	Ua = UaNumer / UaDenom
	'	ptRes.PutCoords(pt1.X + Ua * (pt2.X - pt1.X), pt1.Y + Ua * (pt2.Y - pt1.Y))

	'	If Ua < 0.0 Then Return -1
	'	If Ua > 1.0 Then Return 1
	'	Return 0
	'End Function

	Public Function FixToTouchSpiral(ByVal ptBase As ESRI.ArcGIS.Geometry.IPoint, ByVal coef0 As Double,
	   ByVal TurnR As Double, ByVal TurnDir As Integer, ByVal Theta As Double,
	   ByVal FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByVal DepCourse As Double) As Double

		Dim I As Integer

		Dim R As Double
		Dim F As Double
		Dim F1 As Double
		Dim X1 As Double
		Dim Y1 As Double
		Dim SinT As Double
		Dim CosT As Double
		Dim coef As Double
		Dim Dist As Double
		Dim fTmp As Double
		Dim dTheta As Double
		Dim result As Double
		Dim Theta0 As Double
		Dim Theta1 As Double
		Dim CntTheta As Double
		Dim FixTheta As Double
		Dim Theta1New As Double

		Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntSpiral As ESRI.ArcGIS.Geometry.IPoint

		coef = RadToDeg(coef0)
		Theta0 = Modulus(90.0 * TurnDir + DepCourse + 180.0)
		PtCntSpiral = PointAlongPlane(ptBase, DepCourse + 90.0 * TurnDir, TurnR)
		Dist = ReturnDistanceInMeters(PtCntSpiral, FixPnt)
		FixTheta = ReturnAngleInDegrees(PtCntSpiral, FixPnt)
		dTheta = Modulus((FixTheta - Theta0) * TurnDir)

		R = TurnR + dTheta * coef0
		If Dist < R Then Return -1000.0

		X1 = FixPnt.X - PtCntSpiral.X
		Y1 = FixPnt.Y - PtCntSpiral.Y
		CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, TurnDir)
		CntTheta = Modulus(Theta0 + CntTheta * TurnDir)
		'===============================Variant Firdowsy ==================================

		Theta1 = CntTheta
		For I = 0 To 20
			dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
			SinT = System.Math.Sin(DegToRad(Theta1))
			CosT = System.Math.Cos(DegToRad(Theta1))
			R = TurnR + dTheta * coef0
			F = R * R - (Y1 * R + X1 * coef * TurnDir) * SinT - (X1 * R - Y1 * coef * TurnDir) * CosT
			F1 = 2 * R * coef * TurnDir - (Y1 * R + 2 * X1 * coef * TurnDir) * CosT + (X1 * R - 2 * Y1 * coef * TurnDir) * SinT

			Theta1New = Theta1 - RadToDeg(F / F1)
			fTmp = SubtractAngles(Theta1New, Theta1)
			Theta1 = Theta1New

			If fTmp < 0.0001 Then Exit For
		Next I

		dTheta = Modulus((Theta1 - Theta0) * TurnDir, 360.0)
		R = TurnR + dTheta * coef0
		ptOut = PointAlongPlane(PtCntSpiral, Theta1, R)

		result = ReturnAngleInDegrees(ptOut, FixPnt)
		CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, result, TurnDir)
		CntTheta = Modulus(Theta0 + CntTheta * TurnDir, 360.0)

		If SubtractAngles(CntTheta, Theta1) < 0.0001 Then Return result

		Return -1000.0
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
		    SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
		    CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
		    'a*x2 + b*X + c = 0
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
		    SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
		    CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
		    'a*x2 + b*X + c = 0
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
		    SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
		    CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
		    'a*x2 + b*X + c = 0
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
		    SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
		    CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
		    'a*x2 + b*X + c = 0
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

	Public Function LowHighIntersection(ByVal A As LowHigh, ByVal B As LowHigh, ByRef C As LowHigh) As Integer
		If (A.Low > B.High) Or (A.High < B.Low) Then Return 0

		If A.High < B.High Then
			C.High = A.High
		Else
			C.High = B.High
		End If

		If A.Low > B.Low Then
			C.Low = A.Low
		Else
			C.Low = B.Low
		End If

		Return 1
	End Function

	Public Function LowHighDifference(ByVal A As LowHigh, ByVal B As LowHigh) As LowHigh()
		Dim Res() As LowHigh

		ReDim Res(0)

		If (B.Low = B.High) Or (B.High < A.Low) Or (A.High < B.Low) Then
			Res(0) = A
		ElseIf (A.Low < B.Low) And (A.High > B.High) Then
			ReDim Res(1)
			Res(0).Low = A.Low
			Res(0).High = B.Low
			Res(1).Low = B.High
			Res(1).High = A.High
		ElseIf A.High > B.High Then
			Res(0).Low = B.High
			Res(0).High = A.High
		ElseIf (A.Low < B.Low) Then
			Res(0).Low = A.Low
			Res(0).High = B.Low
		Else
			ReDim Res(-1)
		End If

		Return Res
	End Function

	Public Function IntervalsDifference(ByVal A As Interval, ByVal B As Interval) As Interval()
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

	Public Function AngleIntervalIntersection(ByVal A As Interval, ByVal B As Interval, ByRef C As Interval) As Integer
		Dim fDelta0 As Double
		Dim fDelta1 As Double
		Dim fDelta2 As Double
		Dim fDelta3 As Double
		Dim fDelta4 As Double
		Dim fDelta5 As Double

		fDelta0 = Modulus(A.Right - B.Left, 360.0)
		fDelta1 = Modulus(A.Right - A.Left, 360.0)
		fDelta2 = Modulus(A.Right - B.Right, 360.0)

		fDelta3 = Modulus(B.Right - A.Left, 360.0)
		fDelta4 = Modulus(B.Right - B.Left, 360.0)
		fDelta5 = Modulus(B.Right - A.Right, 360.0)

		AngleIntervalIntersection = 1

		If fDelta0 <= fDelta1 Then
			fDelta1 = Modulus(B.Right - B.Left, 360.0)
			If fDelta1 <= fDelta0 Then
				C = B
			Else
				C.Left = B.Left
				C.Right = A.Right
			End If
		ElseIf fDelta2 <= fDelta1 Then
			C.Left = A.Left
			C.Right = B.Right
		ElseIf fDelta3 <= fDelta4 Then
			If fDelta1 <= fDelta3 Then
				C = A
			Else
				C.Left = A.Left
				C.Right = B.Right
			End If
		ElseIf fDelta5 <= fDelta4 Then
			C.Left = B.Left
			C.Right = A.Right
		Else
			AngleIntervalIntersection = 0
		End If
	End Function

	Public Function IntervalsIntersection(A As Interval, B As Interval, C As Interval) As Long
		Dim Result As Interval
		C.Tag = 0
		IntervalsIntersection = C.Tag
		If (A.Left > B.Right) Or (A.Right < B.Left) Then Exit Function

		If A.Right < B.Right Then
			Result.Right = A.Right
		Else
			Result.Right = B.Right
		End If

		If A.Left > B.Left Then
			Result.Left = A.Left
		Else
			Result.Left = B.Left
		End If

		C = Result
		C.Tag = 1
		IntervalsIntersection = C.Tag
	End Function

	Function CalcDMEInterval(ForFIX As StepDownFIX, ByVal maxLegLenght As Double, ByVal pPtNAV As IPoint, ByVal fTurnR As Double, ByVal TurnDir As Long, ByVal InOrOut As Long) As Interval
		Dim L As Double
		Dim L0 As Double
		Dim L1 As Double
		Dim R1 As Double
		Dim R2 As Double
		Dim R11 As Double
		Dim R12 As Double
		Dim R21 As Double
		Dim R22 As Double
		Dim Phi As Double
		Dim fTmp As Double
		Dim CosA1 As Double
		Dim CosA2 As Double
		Dim Bheta As Double
		Dim minR1R2 As Double
		Dim maxR1R2 As Double

		Dim pPtTmp As IPoint
		Dim ResInterval As Interval

		ResInterval.Left = -1.0#
		ResInterval.Right = -1.0#
		ResInterval.Tag = -1
		CosA1 = -2.0#
		CosA2 = 1.0#

		If ForFIX.TurnDir <> 0 Then
			pPtTmp = ForFIX.pPtStart
		Else
			pPtTmp = PointAlongPlane(ForFIX.pPtPrj, ForFIX.InDir, ForFIX.Length)
		End If

		Phi = Functions.ReturnAngleInDegrees(pPtTmp, pPtNAV)
		L = Functions.ReturnDistanceInMeters(pPtTmp, pPtNAV)

		Bheta = ForFIX.InDir - Phi + 180.0#

		R12 = -1000000.0# * (InOrOut * fTurnR - InOrOut * TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - InOrOut * fTurnR          'If Cos(A1) = -0.5

		If R12 < 0.0# Then
			R22 = 1000000.0# * (InOrOut * fTurnR - InOrOut * TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - InOrOut * fTurnR       'If Cos(A2) = 1
			R21 = CosA2 * (InOrOut * fTurnR - InOrOut * TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - InOrOut * fTurnR          'If Cos(A2) = 1

			maxR1R2 = Max(R21, R22)
			minR1R2 = Min(R21, R22)
		Else
			R11 = CosA1 * (InOrOut * fTurnR - InOrOut * TurnDir * L * Math.Sin(GlobalVars.DegToRadValue * Bheta)) - InOrOut * fTurnR          'If Cos(A1) = -0.5

			maxR1R2 = Max(R11, R12)
			minR1R2 = Min(R11, R12)
		End If

		'If (B > -90) And (B < 90) Then
		'End If

		If (minR1R2 < rDMEMax) And (maxR1R2 > rDMEMin) Then
			R2 = Min(maxR1R2, rDMEMax)
			R1 = Max(minR1R2, rDMEMin)


			L0 = InOrOut * Functions.ReturnDistanceInMeters(ForFIX.pPtPrj, pPtNAV) * SideDef(pPtNAV, ForFIX.InDir + 90.0#, pPtTmp)
			If L0 < 0 Then L0 = rDMEMin

			pPtTmp = PointAlongPlane(ForFIX.pPtPrj, ForFIX.InDir + 180.0#, maxLegLenght)
			'DrawPointWithText pPtTmp, "PtMax"

			L1 = InOrOut * Functions.ReturnDistanceInMeters(pPtTmp, pPtNAV) * SideDef(pPtNAV, ForFIX.InDir + 90.0#, pPtTmp)
			If L1 < 0 Then L1 = rDMEMin

			If SideDef(ForFIX.pPtPrj, ForFIX.InDir - 90.0#, pPtNAV) < 0 Then
				If InOrOut < 0 Then
					If R1 < L0 Then R1 = L0
					If R2 > L1 Then R2 = L1
				Else
					If R1 < L1 Then R1 = L1
					If R2 > L0 Then R2 = L0
				End If
			Else
				If InOrOut < 0 Then
					If R1 < L0 Then R1 = L0
					If R2 > L1 Then R2 = L1
				Else
					If R1 < L1 Then R1 = L1
					If R2 > L0 Then R2 = L0
				End If
			End If

			If InOrOut > 0 Then
				fTmp = L - fTurnR * 1.5
				If R2 > fTmp Then R2 = fTmp
				If Math.Cos(DegToRadValue * Bheta) < 0.0# Then R2 = R1
			Else
				fTmp = L + fTurnR * 1.5
				If R1 < fTmp Then R1 = fTmp
			End If

			If R1 < R2 Then
				ResInterval.Left = R1
				ResInterval.Right = R2
				ResInterval.Tag = 1 '(TurnDir + 1) / 2 + (InOrOut + 1)
			End If
		End If
		CalcDMEInterval = ResInterval
	End Function

	Function CalcNavInterval(ByRef ForFIX As StepDownFIX, ByVal maxLegLenght As Double, ByVal pPtNAV As ESRI.ArcGIS.Geometry.IPoint, ByVal fTurnR As Double, ByVal TurnDir As Integer) As Interval
		Dim fRefX As Double
		Dim fRefY As Double
		Dim maxTurn As Double
		Dim dReserve As Double
		Dim refDirection As Double
		Dim CotanMaxTurn As Double

		Dim dist120 As Double
		Dim fToIntersect120 As Double

		Dim D As Double
		Dim denom As Double
		Dim distMn As Double
		Dim distMx As Double
		Dim dirMin As Double
		Dim dirMax As Double
		Dim alpha_1 As Double
		Dim alpha_2 As Double
		Dim TurnAngle1 As Double
		Dim TurnAngle2 As Double
		Dim fDistToIntersect1 As Double
		Dim fDistToIntersect2 As Double

		Dim bHaveSolution As Boolean
		Dim ResInterval As Interval
		Dim ptMin As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax As ESRI.ArcGIS.Geometry.IPoint

		TurnDir = -TurnDir

		refDirection = ForFIX.InDir + 180.0#
		maxTurn = GlobalVars.DegToRadValue * GlobalVars.MaxInterceptAngle

		ResInterval.Left = -1.0#
		ResInterval.Right = -1.0#
		ResInterval.Tag = -1
		CotanMaxTurn = 1.0# / System.Math.Tan(maxTurn)

		If ForFIX.TurnDir <> 0 Then
			dReserve = -ReturnDistanceInMeters(ForFIX.pPtStart, ForFIX.pPtPrj)
		Else
			dReserve = ForFIX.Length
		End If

		PrjToLocal(ForFIX.pPtPrj, refDirection, pPtNAV, fRefX, fRefY)
		fRefX = fRefX + dReserve

		fToIntersect120 = fRefX - TurnDir * fRefY * CotanMaxTurn
		dist120 = fTurnR * System.Math.Tan(GlobalVars.DegToRadValue * 60.0#)

		If System.Math.Abs(fRefY) < 1 Then ' If System.Math.Abs(fRefY) < distEps Then
			If (fRefX < 0.0#) Then
				CalcNavInterval = ResInterval
				Exit Function
			End If
			ResInterval.Tag = 0

			If TurnDir > 0 Then
				ResInterval.Left = ForFIX.InDir
				ResInterval.Right = ForFIX.InDir + Min(2.0# * GlobalVars.RadToDegValue * Math.Atan2(fRefX, fTurnR), GlobalVars.MaxInterceptAngle)
			Else
				ResInterval.Left = ForFIX.InDir - Min(2.0# * GlobalVars.RadToDegValue * Math.Atan2(fRefX, fTurnR), GlobalVars.MaxInterceptAngle)
				ResInterval.Right = ForFIX.InDir
			End If

			CalcNavInterval = ResInterval
			Exit Function
		End If

		bHaveSolution = False

		If TurnDir * fRefY > 0.0# Then 'eyni teref
			denom = TurnDir * fRefY - 2.0# * fTurnR
			D = fRefX * fRefX + TurnDir * fRefY * denom

			If (D < 0.0#) Then
				CalcNavInterval = ResInterval
				Exit Function
			End If

			D = System.Math.Sqrt(D)

			alpha_1 = 2.0# * Math.Atan2(-fRefX - D, denom)
			alpha_2 = 2.0# * Math.Atan2(-fRefX + D, denom)

			fDistToIntersect1 = fRefX - fRefY * TurnDir / System.Math.Tan(alpha_1)
			fDistToIntersect2 = fRefX - fRefY * TurnDir / System.Math.Tan(alpha_2)

			If (fDistToIntersect1 <= 0.0#) And (fDistToIntersect2 <= 0.0#) Then
				CalcNavInterval = ResInterval
				Exit Function
			End If

			If fDistToIntersect1 * fDistToIntersect2 < 0.0# Then
				distMn = Max(fDistToIntersect1, fDistToIntersect2)
				distMx = Min(fToIntersect120, 0.2 * GlobalVars.MaxNAVDist + dReserve)
			Else
				distMn = Min(fDistToIntersect1, fDistToIntersect2)
				distMx = Min(fToIntersect120, Max(fDistToIntersect1, fDistToIntersect2))
			End If

			If distMx > maxLegLenght + dReserve Then distMx = maxLegLenght + dReserve
			If distMn < dReserve Then distMn = dReserve
			bHaveSolution = distMx > distMn
		Else ' eks teref
			denom = -fRefY * TurnDir
			D = System.Math.Sqrt(fRefX * fRefX + denom * (denom + 2.0# * fTurnR))

			alpha_1 = 2.0# * Math.Atan2(-fRefX - D, denom)
			alpha_2 = 2.0# * Math.Atan2(-fRefX + D, denom)

			fDistToIntersect1 = fRefX + fRefY * TurnDir / System.Math.Tan(alpha_1)
			fDistToIntersect2 = fRefX + fRefY * TurnDir / System.Math.Tan(alpha_2)

			If (fDistToIntersect1 <= 0.0#) And (fDistToIntersect2 <= 0.0#) Then
				CalcNavInterval = ResInterval
				Exit Function
			End If

			If (fDistToIntersect1 * fDistToIntersect2 < 0.0#) Then
				distMn = Max(Max(fDistToIntersect1, fDistToIntersect2), fToIntersect120)
			Else
				distMn = Max(Min(fDistToIntersect1, fDistToIntersect2), fToIntersect120)
			End If

			If distMn < dReserve Then distMn = dReserve
			distMx = Min(0.2 * GlobalVars.MaxNAVDist, maxLegLenght) + dReserve

			bHaveSolution = distMx > distMn
		End If

		If bHaveSolution Then
			ptMin = LocalToPrj(ForFIX.pPtPrj, refDirection, distMn - dReserve)
			ptMax = LocalToPrj(ForFIX.pPtPrj, refDirection, distMx - dReserve)

			dirMin = ReturnAngleInDegrees(ptMin, pPtNAV)
			TurnAngle1 = TurnDir * (dirMin - ForFIX.InDir)
			If Modulus(TurnAngle1) > 180.0# Then dirMin = dirMin + 180.0#

			dirMax = ReturnAngleInDegrees(ptMax, pPtNAV)
			TurnAngle2 = TurnDir * (dirMax - ForFIX.InDir)
			If Modulus(TurnAngle2) > 180.0# Then dirMax = dirMax + 180.0#

			If AnglesSideDef(dirMin, dirMax) < 0 Then
				ResInterval.Left = dirMin
				ResInterval.Right = dirMax
			Else
				ResInterval.Left = dirMax
				ResInterval.Right = dirMin
			End If

			ResInterval.Tag = 0
		End If

		CalcNavInterval = ResInterval
	End Function

	Function CalcLeaveArcInterval(ByRef NextFIX As StepDownFIX, ByRef maxLegLenght As Double, ByRef pPtNAV As ESRI.ArcGIS.Geometry.IPoint, ByRef fTurnR As Double, ByRef TurnDir As eSide) As List(Of Interval)
		Dim NavSide As Integer
		Dim ExitSide As Integer
		Dim L As Double
		Dim Theta As Double
		Dim denom As Double
		Dim MinOut As Double
		Dim MaxOut As Double

		Dim minAngle As Double
		Dim maxAngle As Double

		Dim minInter0 As Double
		Dim maxInter0 As Double

		Dim minInter1 As Double
		Dim maxInter1 As Double

		Dim DMERadius As Double
		Dim minLegAngle As Double
		Dim maxLegAngle As Double

		Dim ToRange As Interval
		Dim Result As List(Of Interval)
		Dim FromRange As Interval
		Dim permissibleRange As Interval

		Dim pPtMin As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtMax As ESRI.ArcGIS.Geometry.IPoint
		Dim ArcDir As eSide

		Dim interval1 As Interval
		Dim interval2 As Interval
		Result = New List(Of Interval)

		ArcDir = NextFIX.ArcDir
		ExitSide = ArcDir * TurnDir

		DMERadius = ReturnDistanceInMeters(NextFIX.pPtPrj, NextFIX.GuidanceNav.pPtPrj)
		L = Functions.ReturnDistanceInMeters(pPtNAV, NextFIX.GuidanceNav.pPtPrj)
		Theta = Functions.ReturnAngleInDegrees(pPtNAV, NextFIX.GuidanceNav.pPtPrj)
		NavSide = IIf(DMERadius > L, -1, 1)

		If NextFIX.TurnDir <> 0 Then
			denom = NavSide * (L - (DMERadius + ExitSide * fTurnR))
			If denom < fTurnR Then
				Return Result
			End If

			minLegAngle = -RadToDeg(ArcSin(fTurnR / denom))
		Else
			minLegAngle = 0 'RadToDeg(NextFIX.Length / DMERadius)
		End If

		minLegAngle = ReturnAngleInDegrees(NextFIX.GuidanceNav.pPtPrj, NextFIX.pPtStart) - ArcDir * minLegAngle
		maxLegAngle = ReturnAngleInDegrees(NextFIX.GuidanceNav.pPtPrj, NextFIX.pPtPrj) + ArcDir * RadToDeg(maxLegLenght / DMERadius)

		If DMERadius < L Then
			If ExitSide > 0 Then
				Return Result
			End If

			minAngle = RadToDeg(ArcSin(0.5 * DMERadius / L))
			maxAngle = RadToDeg(ArcSin(DMERadius / L))

			If ArcDir > 0 Then
				minInter0 = Theta + 180.0# - 90.0# + maxAngle
				maxInter0 = Theta + 180.0# + 30.0# - minAngle

				minInter1 = Theta + 180.0# + 90.0# - maxAngle
				maxInter1 = Theta + 180.0# - 150.0# + minAngle
			Else
				maxInter0 = Theta + 180.0# + 90.0# - maxAngle
				minInter0 = Theta + 180.0# - 30.0# + minAngle

				maxInter1 = Theta + 180.0# - 90.0# + maxAngle
				minInter1 = Theta + 180.0# + 150.0# - minAngle
			End If

			FromRange.Left = minInter0
			FromRange.Right = maxInter0

			ToRange.Left = minInter1
			ToRange.Right = maxInter1

			If ArcDir > 0 Then
				permissibleRange.Left = minLegAngle
				permissibleRange.Right = maxLegAngle
			Else
				permissibleRange.Left = maxLegAngle
				permissibleRange.Right = minLegAngle
			End If


			interval1.Tag = AngleIntervalIntersection(FromRange, permissibleRange, interval1) - 1
			If interval1.Tag > -1 Then
				pPtMin = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, interval1.Left, DMERadius)
				pPtMax = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, interval1.Right, DMERadius)

				interval1.Left = ReturnAngleInDegrees(pPtNAV, pPtMax)
				interval1.Right = ReturnAngleInDegrees(pPtNAV, pPtMin)
			End If

			interval2.Tag = AngleIntervalIntersection(ToRange, permissibleRange, interval2) - 1
			If interval2.Tag > -1 Then
				pPtMin = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, interval2.Left, DMERadius)
				pPtMax = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, interval2.Right, DMERadius)

				interval2.Left = ReturnAngleInDegrees(pPtMin, pPtNAV)
				interval2.Right = ReturnAngleInDegrees(pPtMax, pPtNAV)
			End If

			Result.Add(interval1)
			Result.Add(interval2)
			Return Result
		End If


		pPtMin = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, minLegAngle, DMERadius)
		pPtMax = PointAlongPlane(NextFIX.GuidanceNav.pPtPrj, maxLegAngle, DMERadius)

		MinOut = ReturnAngleInDegrees(pPtMin, pPtNAV) + 90.0# * (ExitSide + 1)
		MaxOut = ReturnAngleInDegrees(pPtMax, pPtNAV) + 90.0# * (ExitSide + 1)

		If ArcDir > 0 Then
			permissibleRange.Left = MinOut
			permissibleRange.Right = MaxOut
		Else
			permissibleRange.Left = MaxOut
			permissibleRange.Right = MinOut
		End If

		If 0.5 * DMERadius < L Then
			minAngle = RadToDeg(ArcSin(0.5 * DMERadius / L))

			interval1.Left = Theta - minAngle + 90 * (1 - ArcDir)
			interval1.Right = Theta + minAngle + 90 * (1 + ArcDir)
		Else

			interval1 = permissibleRange
			interval1.Tag = 1
			Result.Add(interval1)
			Return Result
		End If

		interval1.Tag = AngleIntervalIntersection(interval1, permissibleRange, interval1) - 1
		Result.Add(interval1)
		Return Result
	End Function

	Private Function CalcNomPos(ByVal ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByVal Xs As Double, ByVal Ys As Double, ByVal d0 As Double, ByVal BaseHeight As Double, ByVal fRefAltitude As Double, ByVal PDG As Double, ByVal AheadBehindSide As Integer, ByVal NearSide As Integer) As Double
		Dim dNomPosDer As Double
		Dim dNomPosDME As Double
		Dim dOldPosDME As Double
		Dim nSign As Double
		Dim nSqr As Double
		Dim hMax As Double
		Dim I As Integer

		I = 0
		dNomPosDME = d0 + NearSide * DME.MinimalError
		hMax = 0.0

		Do
			nSqr = dNomPosDME * dNomPosDME - Ys * Ys
			nSign = System.Math.Sign(nSqr)

			dNomPosDer = Xs + AheadBehindSide * nSign * System.Math.Sqrt(System.Math.Abs(nSqr)) 'dNomPosDer = Xs + AheadBehindSide * System.Math.Sqrt(nSqr)
			hMax = dNomPosDer * PDG + BaseHeight + fRefAltitude - ptDMEprj.Z
			dOldPosDME = dNomPosDME
			dNomPosDME = (d0 + NearSide * DME.MinimalError) / (1.0 - NearSide * DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)))

			I = I + 1
			If I > 5 Then Exit Do
		Loop While System.Math.Abs(dOldPosDME - dNomPosDME) > distEps

		Return dNomPosDME
	End Function

	Private Function CalcDMERange(ByVal ptBasePrj As ESRI.ArcGIS.Geometry.Point, ByVal BaseHeight As Double, ByVal fRefAltitude As Double, ByVal NomDir As Double, ByVal PDG As Double, ByVal ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByVal KKhMin As ESRI.ArcGIS.Geometry.IPolyline, ByVal KKhMax As ESRI.ArcGIS.Geometry.IPolyline) As Interval
		Dim Side As Integer
		Dim d0 As Double
		Dim d1 As Double
		Dim Ys As Double
		Dim Xs As Double

		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim LeftRightSide As Integer
		Dim AheadBehindSide As Integer

		AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90.0, ptDMEprj)
		LeftRightSide = SideDef(ptBasePrj, NomDir, ptDMEprj)

		Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90.0) * SideDef(ptBasePrj, NomDir + 90.0, ptDMEprj)
		Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir)

		If AheadBehindSide < 0 Then
			If LeftRightSide > 0 Then
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint)

				Side = SideDef(KKhMax.FromPoint, NomDir, ptDMEprj)
				If Side < 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.FromPoint, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint)
				End If
			Else
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint)

				Side = SideDef(KKhMax.ToPoint, NomDir, ptDMEprj)
				If Side > 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.ToPoint, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint)
				End If
			End If
		Else
			If LeftRightSide > 0 Then
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint)

				Side = SideDef(KKhMin.FromPoint, NomDir, ptDMEprj)
				If Side < 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint)
				End If
			Else
				d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint)

				Side = SideDef(KKhMin.ToPoint, NomDir, ptDMEprj)
				If Side > 0 Then
					d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0)
				Else
					d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint)
				End If
			End If
		End If

		Dist0 = CalcNomPos(ptDMEprj, Xs, Ys, d0, BaseHeight, fRefAltitude, PDG, AheadBehindSide, 1)
		Dist1 = CalcNomPos(ptDMEprj, Xs, Ys, d1, BaseHeight, fRefAltitude, PDG, AheadBehindSide, -1)

		CalcDMERange.Left = Dist0
		CalcDMERange.Right = Dist1
	End Function

	Public Function GetValidNavs(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon, ByVal fRefAlt As Double, ByVal hMin As Double, ByVal hMax As Double, ByVal OCH As Double, ByVal NomDir As Double, ByVal PDG As Double, ByVal ptBase As ESRI.ArcGIS.Geometry.IPoint, ByRef pFIXAreaPolygon As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByVal GuidType As eNavaidType = eNavaidType.NONE, Optional ByVal GuidNav As ESRI.ArcGIS.Geometry.IPoint = Nothing, Optional ByVal bCobvert As Boolean = True) As NavaidData()
		Dim I As Integer
		Dim J As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim ii As Integer
		Dim jj As Integer

		Dim nNav As Integer
		Dim Side As Integer
		Dim LeftRightSide As Integer
		Dim AheadBehindSide As Integer
		Dim AheadBehindKKhMax As Integer
		Dim K As eNavaidType

		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double
		Dim Xs As Double
		Dim Ys As Double
		Dim fTmp As Double
		Dim dMin As Double
		Dim dMax As Double

		Dim ERange As Double
		Dim azt_Far As Double
		Dim azt_Near As Double
		Dim OCHequip As Double
		Dim InterToler As Double
		Dim TrackToler As Double
		Dim Dir_MinL2MaxR As Double
		Dim Dir_MinR2MaxL As Double
		Dim ValidNavs() As NavaidData

		Dim IntrH As Interval
		Dim Intr23 As Interval
		Dim Intr55 As Interval
		Dim IntrRes() As Interval
		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval

		Dim ptFar As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNear As ESRI.ArcGIS.Geometry.IPoint

		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint

		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint

		Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim KKhMin As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMax As ESRI.ArcGIS.Geometry.IPolyline
		Dim Cutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMinDME As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMaxDME As ESRI.ArcGIS.Geometry.IPolyline

		Dim pPolygon1 As ESRI.ArcGIS.Geometry.IPointCollection

		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim Clone As ESRI.ArcGIS.esriSystem.IClone

		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		If GuidType > eNavaidType.NONE Then
			If (GuidType = eNavaidType.VOR) Or (GuidType = eNavaidType.TACAN) Then
				TrackToler = VOR.TrackingTolerance
			ElseIf GuidType = eNavaidType.NDB Then
				TrackToler = NDB.TrackingTolerance
			ElseIf GuidType = eNavaidType.LLZ Then
				TrackToler = LLZ.TrackingTolerance
			End If
			pPolygon1 = New ESRI.ArcGIS.Geometry.Polygon
			pPolygon1.AddPoint(GuidNav)
			pPolygon1.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, 3.0 * MaxModelRadius))
			pPolygon1.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, 3.0 * MaxModelRadius))
			pPolygon1.AddPoint(GuidNav)
			'    If GuidType <> 3 Then
			pPolygon1.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, 3.0 * MaxModelRadius))
			pPolygon1.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, 3.0 * MaxModelRadius))
			pPolygon1.AddPoint(GuidNav)
			'    End If
		Else
			Clone = pPolygon
			pPolygon1 = Clone.Clone
		End If

		pTopoOper = pPolygon1
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pRelational = pPolygon1

		Clone = pPolygon1
		pFIXAreaPolygon = Clone.Clone

		dMin = (hMin - ptBase.Z) / PDG '0# '(hMin - arAbv_Treshold.Value) / PDG
		dMax = (hMax - ptBase.Z) / PDG

		Cutter = New ESRI.ArcGIS.Geometry.Polyline

		IntrH.Left = dMin
		IntrH.Right = dMax

		ptTmp = PointAlongPlane(ptBase, NomDir, dMin)

		Cutter.FromPoint = PointAlongPlane(ptTmp, NomDir - 90.0, MaxModelRadius)
		Cutter.ToPoint = PointAlongPlane(ptTmp, NomDir + 90.0, MaxModelRadius)

		'DrawPolyLine Cutter, 255, 2         '"""""

		pFIXAreaPolygon = CutNavPoly(pFIXAreaPolygon, Cutter, -1)
		KKhMin = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If KKhMin.IsEmpty() Then
			KKhMin.FromPoint = ptTmp
			KKhMin.ToPoint = ptTmp
		ElseIf SideDef(ptTmp, NomDir, KKhMin.FromPoint) < 0 Then
			KKhMin.ReverseOrientation()
		End If

		ptTmp = KKhMin.FromPoint
		ptTmp.M = 0
		KKhMin.FromPoint = ptTmp

		ptTmp = KKhMin.ToPoint
		ptTmp.M = 0
		KKhMin.ToPoint = ptTmp

		ptTmp = PointAlongPlane(ptBase, NomDir, dMax)
		Cutter.FromPoint = PointAlongPlane(ptTmp, NomDir - 90.0, MaxModelRadius)
		Cutter.ToPoint = PointAlongPlane(ptTmp, NomDir + 90.0, MaxModelRadius)

		pFIXAreaPolygon = CutNavPoly(pFIXAreaPolygon, Cutter, 1)

		'DrawPolygon(pFIXAreaPolygon, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSHorizontal)
		'Application.DoEvents()

		KKhMax = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If KKhMax.IsEmpty() Then
			KKhMax.FromPoint = ptTmp
			KKhMax.ToPoint = ptTmp
		ElseIf SideDef(ptTmp, NomDir, KKhMax.FromPoint) < 0 Then
			KKhMax.ReverseOrientation()
		End If

		ptTmp = KKhMin.FromPoint
		ptTmp.M = ReturnAngleInDegrees(KKhMin.FromPoint, KKhMax.FromPoint)
		KKhMin.FromPoint = ptTmp

		ptTmp = KKhMin.ToPoint
		ptTmp.M = ReturnAngleInDegrees(KKhMin.ToPoint, KKhMax.ToPoint)
		KKhMin.ToPoint = ptTmp

		'DrawPolyLine(KKhMin, 0, 2)
		'DrawPolyLine(KKhMax, 255, 2)
		'Application.DoEvents()

		ptNearD = New ESRI.ArcGIS.Geometry.Point
		Construct = ptNearD
		Construct.ConstructAngleIntersection(ptBase, DegToRad(NomDir), KKhMin.ToPoint, DegToRad(NomDir + 90.0))

		ptFarD = New ESRI.ArcGIS.Geometry.Point
		Construct = ptFarD
		Construct.ConstructAngleIntersection(ptBase, DegToRad(NomDir), KKhMax.ToPoint, DegToRad(NomDir + 90.0))

		Dir_MinL2MaxR = ReturnAngleInDegrees(KKhMin.ToPoint, KKhMax.FromPoint)
		Dir_MinR2MaxL = ReturnAngleInDegrees(KKhMin.FromPoint, KKhMax.ToPoint)

		OCHequip = OCH + fRefAlt

		J = 0
		I = -1

		ReDim ValidNavs(nNav - 1)

		For I = 0 To UBound(NavaidList)
			ValidNavs(J) = NavaidList(I)

			ptFNav = NavaidList(I).pPtGeo
			ptFNavPrj = NavaidList(I).pPtPrj
			K = NavaidList(I).TypeCode

			LeftRightSide = SideDef(ptBase, NomDir, ptFNavPrj)
			AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90.0, ptFNavPrj)
			AheadBehindKKhMax = SideDef(KKhMax.FromPoint, NomDir + 90.0, ptFNavPrj)

			If (K = eNavaidType.VOR) Or (K = eNavaidType.NDB) Or (K = eNavaidType.TACAN) Then
				If pRelational.Contains(ptFNavPrj) Then Continue For

				If K = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance
					ERange = NDB.Range
				Else
					InterToler = VOR.IntersectingTolerance
					ERange = VOR.Range
				End If

				Side = SideDef(KKhMax.FromPoint, Dir_MinL2MaxR, ptFNavPrj)
				If Side * LeftRightSide < 0 Then Continue For
				Side = SideDef(KKhMax.ToPoint, Dir_MinR2MaxL, ptFNavPrj)
				If Side * LeftRightSide < 0 Then Continue For

				If LeftRightSide > 0 Then '   LeftSide
					Side = SideDef(KKhMin.FromPoint, KKhMin.FromPoint.M, ptFNavPrj)
					If Side < 0 Then Continue For

					If AheadBehindSide < 0 Then 'RightSide
						ptNear = KKhMin.FromPoint
						ptFar = KKhMax.ToPoint
					ElseIf AheadBehindKKhMax < 0 Then
						ptNear = KKhMin.ToPoint
						ptFar = KKhMax.ToPoint
					Else
						ptNear = KKhMin.ToPoint
						ptFar = KKhMax.FromPoint
					End If
				Else
					Side = SideDef(KKhMin.ToPoint, KKhMin.ToPoint.M, ptFNavPrj)
					If Side > 0 Then Continue For

					If AheadBehindSide < 0 Then
						ptNear = KKhMin.ToPoint
						ptFar = KKhMax.FromPoint
					ElseIf AheadBehindKKhMax < 0 Then
						ptNear = KKhMin.FromPoint
						ptFar = KKhMax.FromPoint
					Else
						ptNear = KKhMin.FromPoint
						ptFar = KKhMax.ToPoint
					End If
				End If

				'            If ERange < ReturnDistanceInMeters(ptFNavPrj, ptFar) Then Continue For

				azt_Far = ReturnAngleInDegrees(ptFNavPrj, ptFar)
				azt_Near = ReturnAngleInDegrees(ptFNavPrj, ptNear)

				'            If (azt_Near - azt_Far) * LeftRightSide < 2.0 * InterToler Then Continue For
				If SubtractAngles(azt_Near, azt_Far) < 2.0 * InterToler Then Continue For

				ReDim ValidNavs(J).ValMax(0)
				ReDim ValidNavs(J).ValMin(0)

				ValidNavs(J).ValCnt = LeftRightSide
				If bCobvert Then
					If LeftRightSide > 0 Then
						ValidNavs(J).ValMax(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Far + InterToler) - 0.4999)
						ValidNavs(J).ValMin(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Near - InterToler) + 0.4999)
					Else
						ValidNavs(J).ValMin(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Far - InterToler) + 0.4999)
						ValidNavs(J).ValMax(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Near + InterToler) - 0.4999)
					End If
				Else
					If LeftRightSide > 0 Then
						ValidNavs(J).ValMax(0) = azt_Far + InterToler
						ValidNavs(J).ValMin(0) = azt_Near - InterToler
					Else
						ValidNavs(J).ValMin(0) = azt_Far - InterToler
						ValidNavs(J).ValMax(0) = azt_Near + InterToler
					End If
				End If

				If SubtractAngles(ValidNavs(J).ValMax(0) + InterToler, ValidNavs(J).ValMin(0) - InterToler) < InterToler Then
					Continue For
				End If

				ValidNavs(J).IntersectionType = eIntersectionType.ByAngle
				J = J + 1
			End If
		Next I

		'========================================================================================
		For I = 0 To UBound(DMEList)
			ValidNavs(J) = DMEList(I)

			ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			LeftRightSide = SideDef(ptBase, NomDir, ptFNavPrj)

			AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90.0, ptFNavPrj)
			AheadBehindKKhMax = SideDef(KKhMax.FromPoint, NomDir + 90.0, ptFNavPrj)

			fTmp = 0.0
			If LeftRightSide > 0 Then
				If AheadBehindSide < 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMin.ToPoint)
				ElseIf AheadBehindKKhMax > 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.ToPoint)
				End If
			Else
				If AheadBehindSide < 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMin.FromPoint)
				ElseIf AheadBehindKKhMax > 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.FromPoint)
				End If
			End If

			If fTmp > DME.Range Then Continue For '   Range checking

			If LeftRightSide <> 0 Then
				ptMin23 = New ESRI.ArcGIS.Geometry.Point
				ptMax23 = New ESRI.ArcGIS.Geometry.Point
				Construct = ptMin23
				Construct.ConstructAngleIntersection(ptBase, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value))
				Construct = ptMax23
				Construct.ConstructAngleIntersection(ptBase, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value))
			Else
				ptMin23 = ptFNavPrj
				ptMax23 = ptFNavPrj
			End If

			'DrawPointWithText(ptMin23, "ptMin23")
			'DrawPointWithText(ptMax23, "ptMax23")
			'While True
			'	Application.DoEvents()
			'End While

			Intr23.Left = Point2LineDistancePrj(ptBase, ptMin23, NomDir + 90.0) * SideDef(ptBase, NomDir + 90.0, ptMin23)
			Intr23.Right = Point2LineDistancePrj(ptBase, ptMax23, NomDir + 90.0) * SideDef(ptBase, NomDir + 90.0, ptMax23)
			IntrRes = IntervalsDifference(IntrH, Intr23)

			Xs = Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir + 90.0) * SideDef(ptBase, NomDir + 90.0, ptFNavPrj)
			Ys = Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir)

			fTmp = 1.0 / System.Math.Tan(DegToRad(DME.SlantAngle))
			fTmp = fTmp * fTmp

			A = PDG * PDG - fTmp
			B = 2.0 * ((OCHequip - ptFNavPrj.Z) * PDG + Xs * fTmp)
			C = (OCHequip - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
			D = B * B - 4 * A * C

			If D > 0.0 Then
				If A > 0 Then
					Intr55.Left = 0.5 * (-B - System.Math.Sqrt(D)) / A
					Intr55.Right = 0.5 * (-B + System.Math.Sqrt(D)) / A
				Else
					Intr55.Left = 0.5 * (-B + System.Math.Sqrt(D)) / A
					Intr55.Right = 0.5 * (-B - System.Math.Sqrt(D)) / A
				End If

				N = UBound(IntrRes)
				ReDim IntrRes1(-1)

				For ii = 0 To N
					IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)

					If UBound(IntrRes1) < 0 Then
						IntrRes1 = IntrRes2
					Else
						L = UBound(IntrRes1)
						M = UBound(IntrRes2)
						If M >= 0 Then
							ReDim Preserve IntrRes1(L + M + 1)

							For jj = 0 To M
								IntrRes1(jj + L + 1) = IntrRes2(jj)
							Next jj
						End If
					End If
				Next ii

				IntrRes = IntrRes1
			End If

			N = UBound(IntrRes)

			ii = 0
			If N >= 0 Then
				Do
					If IntrRes(ii).Left = IntrRes(ii).Right Then
						For jj = ii To N - 1
							IntrRes(jj) = IntrRes(jj + 1)
						Next jj
						N = N - 1
					Else
						ii = ii + 1
					End If
				Loop While ii < N - 1
			End If

			ii = 0
			While ii < N - 1
				If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
					IntrRes(ii).Right = IntrRes(ii + 1).Right
					For jj = ii + 1 To N - 1
						IntrRes(jj) = IntrRes(jj + 1)
					Next jj
					N = N - 1
				Else
					ii = ii + 1
				End If
			End While

			If N < 0 Then Continue For

			ReDim IntrRes1(N)
			M = 0

			For ii = 0 To N
				If System.Math.Abs(IntrRes(ii).Left - IntrRes(ii).Right) > 4 * DME.MinimalError Then
					ptNearD = PointAlongPlane(ptBase, NomDir, IntrRes(ii).Left)
					ptFarD = PointAlongPlane(ptBase, NomDir, IntrRes(ii).Right)

					Cutter.FromPoint = PointAlongPlane(ptNearD, NomDir - 90.0, MaxModelRadius)
					Cutter.ToPoint = PointAlongPlane(ptNearD, NomDir + 90.0, MaxModelRadius)

					KKhMinDME = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

					If KKhMinDME.IsEmpty() Then
						KKhMinDME.FromPoint = ptFarD
						KKhMinDME.ToPoint = ptFarD
					ElseIf SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0 Then
						KKhMinDME.ReverseOrientation()
					End If

					Cutter.FromPoint = PointAlongPlane(ptFarD, NomDir - 90.0, MaxModelRadius)
					Cutter.ToPoint = PointAlongPlane(ptFarD, NomDir + 90.0, MaxModelRadius)

					KKhMaxDME = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

					If KKhMaxDME.IsEmpty() Then
						KKhMaxDME.FromPoint = ptFarD
						KKhMaxDME.ToPoint = ptFarD
					ElseIf SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0 Then
						KKhMaxDME.ReverseOrientation()
					End If

					IntrRes1(M) = CalcDMERange(ptBase, OCH, fRefAlt, NomDir, PDG, ptFNavPrj, KKhMinDME, KKhMaxDME)
					If IntrRes1(M).Left < IntrRes1(M).Right And (IntrRes1(M).Right - IntrRes1(M).Left > 4 * DME.MinimalError) Then
						M = M + 1
						ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90.0, ptFNavPrj)
					End If
				End If
			Next ii

			M = M - 1
			If M < 0 Then Continue For


			If M > 0 Then ValidNavs(J).ValCnt = 0

			ReDim ValidNavs(J).ValMax(M)
			ReDim ValidNavs(J).ValMin(M)

			For ii = 0 To M
				ValidNavs(J).ValMin(ii) = System.Math.Round(IntrRes1(ii).Left + 0.4999)
				ValidNavs(J).ValMax(ii) = System.Math.Round(IntrRes1(ii).Right - 0.4999)
			Next ii

			If ValidNavs(J).ValMax(0) >= ValidNavs(J).ValMin(0) Then
				ValidNavs(J).IntersectionType = eIntersectionType.ByDistance
				J = J + 1
			End If
		Next I

		'Set pGroupElem = New GroupElement
		'pGroupElem.AddElement DrawPolygon(pFIXAreaPolygon, RGB(195, 195, 195), False)
		'Set FIXElem = pGroupElem
		'msgbox "1-       J = " + CStr(J)

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	Public Function GetValidFAPNavs(ByVal PtTHR As ESRI.ArcGIS.Geometry.IPoint, ByVal MaxDist As Double, ByVal NomDir As Double, ByVal ptFAP As ESRI.ArcGIS.Geometry.IPoint, ByVal hFAP As Double, ByVal GuidType As eNavaidType, ByVal GuidNav As ESRI.ArcGIS.Geometry.IPoint) As NavaidData()
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer

		Dim ValidNavs() As NavaidData
		Dim nNav As Integer
		Dim Side As Integer
		Dim AheadBehindSide As Integer

		Dim dMin As Double
		Dim dMax As Double
		Dim Dist0 As Double
		Dim d0 As Double
		Dim fTmp As Double
		Dim Hequ As Double

		Dim TrackToler As Double

		Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim DirNAV2FAF As Double
		Dim Alpha As Double
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim p4Poly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pGeomCollection1 As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPLine As IPointCollection
		Dim pNominalLine As IPolyline
		'===========================================================================
		If (GuidType = eNavaidType.VOR) Or (GuidType = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
			fTmp = VOR.Range
		ElseIf GuidType = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
			fTmp = NDB.Range
		ElseIf GuidType = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
			fTmp = LLZ.Range
		End If

		nNav = UBound(DMEList)

		If nNav < 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		ptTmp = New ESRI.ArcGIS.Geometry.Point

		p4Poly = New ESRI.ArcGIS.Geometry.Polygon
		pGeomCollection1 = p4Poly

		AheadBehindSide = SideDef(ptFAP, NomDir + 90.0, GuidNav)
		pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		pGuidPoly.AddPoint(GuidNav)

		If AheadBehindSide > 0 Then
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, fTmp))
		ElseIf AheadBehindSide < 0 Then
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp))
		Else
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(GuidNav)
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp))
		End If
		pGuidPoly.AddPoint(GuidNav)

		pTopoOper = pGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
		'DrawPolygon pGuidPoly

		pNominalLine = New Polyline()
		pNominalLine.FromPoint = PointAlongPlane(PtTHR, NomDir + 180.0, 2.0 * MaxDist)
		pNominalLine.ToPoint = PointAlongPlane(PtTHR, NomDir, 2.0 * MaxDist)

		ReDim ValidNavs(nNav)
		J = 0
		'========================================================================================

		For I = 0 To nNav
			ValidNavs(J) = DMEList(I)

			ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			'LeftRightSide = SideDef(ptFAP, NomDir, ptFNavPrj)
			DirNAV2FAF = ReturnAngleInDegrees(ptFNavPrj, ptFAP)
			d0 = ReturnDistanceInMeters(ptFNavPrj, ptFAP)
			'=========================
			fTmp = SubtractAngles(DirNAV2FAF, NomDir)
			If fTmp > 90.0 Then fTmp = 180.0 - fTmp
			If fTmp > arTP_by_DME_div.Value Then Continue For

			If d0 + 0.5 * arFAFTolerance.Value > DME.Range Then Continue For

			Hequ = hFAP + PtTHR.Z - ptFNavPrj.Z
			Alpha = RadToDeg(System.Math.Atan(Hequ / d0))
			If Alpha > 90.0 - DME.SlantAngle Then Continue For

			Dist0 = System.Math.Sqrt(Hequ * Hequ + d0 * d0)

			dMin = (Dist0 - DME.MinimalError) / (1.0 + DME.ErrorScalingUp)
			dMax = (Dist0 + DME.MinimalError) / (1.0 - DME.ErrorScalingUp)

			Side = SideDef(ptFAP, NomDir + 90.0, ptFNavPrj)

			If Side < 0 Then
				CircleVectorIntersect(ptFNavPrj, dMin, ptFAP, NomDir, ptTmp)
				If ptTmp.IsEmpty() Then Continue For
				If ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * arFAFTolerance.Value Then Continue For

				CircleVectorIntersect(ptFNavPrj, dMax, ptFAP, NomDir, ptTmp)
				If ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * arFAFTolerance.Value Then Continue For
				fTmp = ReturnDistanceInMeters(ptFNavPrj, GuidNav)
			Else
				CircleVectorIntersect(ptFNavPrj, dMin, ptFAP, NomDir + 180.0, ptTmp)
				If ptTmp.IsEmpty() Then Continue For
				If ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * arFAFTolerance.Value Then Continue For

				CircleVectorIntersect(ptFNavPrj, dMax, ptFAP, NomDir + 180.0, ptTmp)
				If ReturnDistanceInMeters(ptFAP, ptTmp) > 0.5 * arFAFTolerance.Value Then Continue For
			End If

			pCircle = CreatePrjCircle(ptFNavPrj, dMax)
			pTmpPoly = CreatePrjCircle(ptFNavPrj, dMin)
			pTopoOper = pCircle
			pCircle = pTopoOper.Difference(pTmpPoly)

			pTopoOper = pCircle
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pTmpPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			pGeomCollection = pTmpPoly

			If pGeomCollection1.GeometryCount > 0 Then
				pGeomCollection1.RemoveGeometries(0, pGeomCollection1.GeometryCount)
			End If

			If pTmpPoly.ExteriorRingCount > 1 Then
				pProxi = ptFAP
				For K = 0 To pGeomCollection.GeometryCount - 1
					pGeomCollection1.RemoveGeometries(0, pGeomCollection1.GeometryCount)
					pGeomCollection1.AddGeometry(pGeomCollection.Geometry(K))
					If pProxi.ReturnDistance(pGeomCollection1) = 0.0 Then Exit For
				Next K
			Else
				pGeomCollection1.AddGeometry(pGeomCollection.Geometry(0))
			End If

			pTopoOper = p4Poly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			Dim bUseful As Boolean = True

			pPLine = pTopoOper.Intersect(pNominalLine, esriGeometryDimension.esriGeometry1Dimension)

			For K = 0 To pPLine.PointCount - 1
				If ReturnDistanceInMeters(ptFAP, pPLine.Point(K)) > arFAFTolerance.Value Then
					bUseful = False
					Exit For
				End If
			Next K
			If Not bUseful Then Continue For

			ValidNavs(J).IntersectionType = eIntersectionType.ByDistance
			J = J + 1
		Next I
		'========================================================================================
		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	Public Function GetValidFAFNavs(ByVal ptFicTHR As ESRI.ArcGIS.Geometry.IPoint, ByVal MaxDist As Double, ByVal NomDir As Double, ByVal PtFAF As ESRI.ArcGIS.Geometry.IPoint, ByVal hFAF As Double, ByVal GuidType As Integer, ByVal GuidNav As ESRI.ArcGIS.Geometry.IPoint) As NavaidData()
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer

		Dim nNav As Integer
		Dim Side As Integer
		Dim AheadBehindSide As Integer

		Dim ValidNavs() As NavaidData

		Dim dMin As Double
		Dim dMax As Double
		Dim Dist0 As Double

		Dim d0 As Double
		Dim fTmp As Double
		Dim Hequ As Double

		Dim InterToler As Double
		Dim TrackToler As Double
		Dim FixToler As Double

		Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim DirNAV2FAF As Double
		Dim Alpha As Double
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim p4Poly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pCircle As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGeomCollection As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pGeomCollection1 As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pPLine As IPointCollection
		Dim pNominalLine As IPolyline
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		'===========================================================================
		If (GuidType = eNavaidType.VOR) Or (GuidType = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
			fTmp = VOR.Range
		ElseIf GuidType = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
			fTmp = NDB.Range
		ElseIf GuidType = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
			fTmp = LLZ.Range
		End If

		ptTmp = New ESRI.ArcGIS.Geometry.Point

		p4Poly = New ESRI.ArcGIS.Geometry.Polygon
		pGeomCollection1 = p4Poly

		AheadBehindSide = SideDef(PtFAF, NomDir + 90.0, GuidNav)
		pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		pGuidPoly.AddPoint(GuidNav)

		If AheadBehindSide > 0 Then
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, fTmp))
		ElseIf AheadBehindSide > 0 Then
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp))
		Else
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, fTmp))
			pGuidPoly.AddPoint(GuidNav)
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, fTmp))
			pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, fTmp))
		End If
		pGuidPoly.AddPoint(GuidNav)

		pTopoOper = pGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pNominalLine = New Polyline()
		pNominalLine.FromPoint = PointAlongPlane(ptFicTHR, NomDir + 180.0, 2 * MaxDist)
		pNominalLine.ToPoint = PointAlongPlane(ptFicTHR, NomDir, 2 * MaxDist)


		ReDim ValidNavs(nNav - 1)
		J = 0

		For I = 0 To UBound(NavaidList)
			ValidNavs(J) = NavaidList(I)
			K = NavaidList(I).TypeCode

			If (K = eNavaidType.VOR) Or (K = eNavaidType.NDB) Or (K = eNavaidType.TACAN) Then
				ptFNavPrj = NavaidList(I).pPtPrj

				DirNAV2FAF = ReturnAngleInDegrees(ptFNavPrj, PtFAF)
				d0 = ReturnDistanceInMeters(ptFNavPrj, PtFAF)

				If K = eNavaidType.NDB Then
					If d0 < NDB.OnNAVRadius Then Continue For
					InterToler = NDB.IntersectingTolerance
				Else
					If d0 < VOR.OnNAVRadius Then Continue For
					InterToler = VOR.IntersectingTolerance
				End If

				fTmp = SubtractAngles(DirNAV2FAF, NomDir)
				If fTmp < 3.0 * InterToler Then Continue For
				If fTmp > 180.0 - 3.0 * InterToler Then Continue For

				FixToler = d0 * System.Math.Sin(DegToRad(InterToler)) / System.Math.Sin(DegToRad(fTmp + (1 + 2 * CShort(fTmp <= 90.0)) * InterToler))
				If FixToler > arFAFTolerance.Value Then Continue For

				ValidNavs(J).IntersectionType = eIntersectionType.ByAngle
				J = J + 1
			End If
		Next I

		For I = 0 To UBound(DMEList)
			ValidNavs(J) = DMEList(I)
			ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			'LeftRightSide = SideDef(PtFAF, NomDir, ptFNavPrj)
			DirNAV2FAF = ReturnAngleInDegrees(ptFNavPrj, PtFAF)
			d0 = ReturnDistanceInMeters(ptFNavPrj, PtFAF)

			fTmp = SubtractAngles(DirNAV2FAF, NomDir)
			If fTmp > 90.0 Then fTmp = 180.0 - fTmp
			If fTmp > arTP_by_DME_div.Value Then Continue For

			If d0 + arFAFTolerance.Value > DME.Range Then Continue For

			Hequ = hFAF + ptFicTHR.Z - ptFNavPrj.Z
			Alpha = RadToDeg(System.Math.Atan(Hequ / d0))
			If Alpha > 90.0 - DME.SlantAngle Then Continue For

			Dist0 = System.Math.Sqrt(Hequ * Hequ + d0 * d0)

			dMin = (Dist0 - DME.MinimalError) / (1.0 + DME.ErrorScalingUp)
			dMax = (Dist0 + DME.MinimalError) / (1.0 - DME.ErrorScalingUp)

			Side = SideDef(PtFAF, NomDir + 90.0, ptFNavPrj)

			If Side < 0 Then
				CircleVectorIntersect(ptFNavPrj, dMin, PtFAF, NomDir, ptTmp)
				If ptTmp.IsEmpty() Then Continue For
				If ReturnDistanceInMeters(PtFAF, ptTmp) > arFAFTolerance.Value Then Continue For

				CircleVectorIntersect(ptFNavPrj, dMax, PtFAF, NomDir, ptTmp)
				If ReturnDistanceInMeters(PtFAF, ptTmp) > arFAFTolerance.Value Then Continue For
				fTmp = ReturnDistanceInMeters(ptFNavPrj, GuidNav)
			Else
				CircleVectorIntersect(ptFNavPrj, dMin, PtFAF, NomDir + 180.0, ptTmp)
				If ptTmp.IsEmpty() Then Continue For
				If ReturnDistanceInMeters(PtFAF, ptTmp) > arFAFTolerance.Value Then Continue For

				CircleVectorIntersect(ptFNavPrj, dMax, PtFAF, NomDir + 180.0, ptTmp)
				If ReturnDistanceInMeters(PtFAF, ptTmp) > arFAFTolerance.Value Then Continue For
			End If

			pCircle = CreatePrjCircle(ptFNavPrj, dMax)
			pTmpPoly = CreatePrjCircle(ptFNavPrj, dMin)
			pTopoOper = pCircle
			pCircle = pTopoOper.Difference(pTmpPoly)

			pTopoOper = pCircle
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pTmpPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			pGeomCollection = pTmpPoly

			If pGeomCollection1.GeometryCount > 0 Then
				pGeomCollection1.RemoveGeometries(0, pGeomCollection1.GeometryCount)
			End If

			If pTmpPoly.ExteriorRingCount > 1 Then
				pProxi = PtFAF
				For K = 0 To pGeomCollection.GeometryCount - 1
					pGeomCollection1.RemoveGeometries(0, pGeomCollection1.GeometryCount)
					pGeomCollection1.AddGeometry(pGeomCollection.Geometry(K))
					If pProxi.ReturnDistance(pGeomCollection1) = 0.0 Then Exit For
				Next K
			Else
				pGeomCollection1.AddGeometry(pGeomCollection.Geometry(0))
			End If

			pTopoOper = p4Poly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pPLine = pTopoOper.Intersect(pNominalLine, esriGeometryDimension.esriGeometry1Dimension)
			For K = 0 To pPLine.PointCount - 1
				If ReturnDistanceInMeters(PtFAF, pPLine.Point(K)) > arFAFTolerance.Value Then Continue For
			Next K

			ValidNavs(J).IntersectionType = eIntersectionType.ByDistance
			J = J + 1
		Next I

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	Public Function GetValidIFNavs(ByVal PtFAF As ESRI.ArcGIS.Geometry.IPoint, ByVal fRefH As Double, ByVal minDist As Double, ByVal MaxDist As Double, ByVal NomDir As Double, ByVal fPDG As Double, ByVal GuidType As eNavaidType, ByVal GuidNav As ESRI.ArcGIS.Geometry.IPoint) As NavaidData()
		Dim AheadBehindSide As Integer
		Dim LeftRightSide As Integer
		Dim nNav As Integer
		Dim Side As Integer
		Dim ii As Integer
		Dim jj As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As eNavaidType
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer

		Dim ValidNavs() As NavaidData
		Dim fMinDMEDist As Double
		Dim InterToler As Double
		Dim TrackToler As Double
		Dim azt_Near As Double
		Dim azt_Far As Double
		Dim Hequip As Double
		Dim Dist0 As Double
		Dim Dist1 As Double
		Dim Betta As Double
		Dim fTmp As Double
		Dim d0 As Double
		Dim d1 As Double
		Dim Xs As Double
		Dim Ys As Double
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double

		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval
		Dim IntrRes() As Interval
		Dim Intr3700 As Interval
		Dim Intr23 As Interval
		Dim Intr55 As Interval
		Dim IntrH As Interval

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim KKhMinDME As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMaxDME As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMax As ESRI.ArcGIS.Geometry.IPolyline

		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNear As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFar As ESRI.ArcGIS.Geometry.IPoint

		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		If (GuidType = eNavaidType.VOR) Or (GuidType = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
		ElseIf GuidType = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
		ElseIf GuidType = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
		End If

		pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		'If GuidType <> eNavaidTupes.CodeLLZ Then
		pGuidPoly.AddPoint(GuidNav)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler, 3.0 * MaxModelRadius))
		'End If
		pGuidPoly.AddPoint(GuidNav)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(GuidNav)

		pTopoOper = pGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		'DrawPointWithText(PtFAF, "-FAF")
		'While True
		'System.Windows.Forms.Application.DoEvents()
		'End While

		ptFar = PointAlongPlane(PtFAF, NomDir + 180.0, MaxDist)
		ptNear = PointAlongPlane(PtFAF, NomDir + 180.0, minDist)

		'DrawPointWithText(ptFar, "-Far")
		'DrawPointWithText(ptNear, "-Near")
		'While True
		'	System.Windows.Forms.Application.DoEvents()
		'End While

		KKhMax = New ESRI.ArcGIS.Geometry.Polyline
		ptTmp = New ESRI.ArcGIS.Geometry.Point
		Construct = ptTmp

		Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir - TrackToler), ptFar, DegToRad(NomDir + 90.0))
		KKhMax.FromPoint = ptTmp

		Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir + TrackToler), ptFar, DegToRad(NomDir + 90.0))
		KKhMax.ToPoint = ptTmp

		If SideDef(KKhMax.ToPoint, NomDir, KKhMax.FromPoint) > 0 Then
			KKhMax.ReverseOrientation()
		End If

		KKhMaxDME = New ESRI.ArcGIS.Geometry.Polyline

		I = -1
		J = 0
		ReDim ValidNavs(nNav - 1)
		pProxi = pGuidPoly

		For I = 0 To UBound(NavaidList)
			ValidNavs(J) = NavaidList(I)
			ptFNav = NavaidList(I).pPtGeo
			ptFNavPrj = NavaidList(I).pPtPrj

			If pProxi.ReturnDistance(ptFNavPrj) = 0.0 Then Continue For

			K = NavaidList(I).TypeCode

			LeftRightSide = SideDef(ptNear, NomDir + 180.0, ptFNavPrj)
			AheadBehindSide = SideDef(ptNear, NomDir - 90.0, ptFNavPrj) 'ptFar

			If (K = eNavaidType.VOR) Or (K = eNavaidType.NDB) Or (K = eNavaidType.TACAN) Then
				If K = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance
				Else
					InterToler = VOR.IntersectingTolerance
				End If

				Side = SideDef(KKhMax.FromPoint, NomDir - LeftRightSide * 90.0, ptFNavPrj)
				If Side < 0 Then
					ptFarD = KKhMax.ToPoint
				Else
					ptFarD = KKhMax.FromPoint
				End If

				'If ERange < ReturnDistanceInMeters(ptFNavPrj, ptFarD) Then Continue For
				If NavaidList(I).Range < ReturnDistanceInMeters(ptFNavPrj, ptFarD) Then Continue For

				azt_Far = ReturnAngleInDegrees(ptFNavPrj, ptFarD)
				azt_Near = ReturnAngleInDegrees(ptFNavPrj, ptNear)

				If SubtractAngles(azt_Near, azt_Far) < 2.0 * InterToler Then Continue For

				D = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)
				If RadToDeg(System.Math.Atan(arIFTolerance.Value / D)) < InterToler Then Continue For

				Betta = 0.5 * (ArcCos(2.0 * D * System.Math.Sin(DegToRad(InterToler)) / arIFTolerance.Value - System.Math.Cos(DegToRad(InterToler))) - DegToRad(InterToler))
				Construct = ptTmp

				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + 90.0) + Betta)
				'DrawPoint ptTmp, RGB(0, 0, 255)
				If SideDef(PtFAF, NomDir + 90.0, ptTmp) > 0 Then
					Dist0 = 0.0
				Else
					Dist0 = ReturnDistanceInMeters(PtFAF, ptTmp)
				End If

				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + 90.0) - Betta)
				'DrawPoint ptTmp, RGB(0, 0, 255)

				If SideDef(PtFAF, NomDir + 90.0, ptTmp) > 0 Then
					Dist1 = 0.0
				Else
					Dist1 = ReturnDistanceInMeters(PtFAF, ptTmp)
				End If

				If Dist1 < Dist0 Then
					fTmp = Dist1
					Dist1 = Dist0
					Dist0 = fTmp
				End If

				If Dist1 = 0 Then Continue For
				'        If Dist1 < minDist Then Continue For
				'        If Dist0 > MaxDist Then Continue For

				d0 = minDist
				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(azt_Far + LeftRightSide * InterToler))

				d1 = ReturnDistanceInMeters(PtFAF, ptTmp)
				If d0 < Dist0 Then d0 = Dist0
				If d1 > Dist1 Then d1 = Dist1
				If d1 < d0 Then Continue For

				ptNearD = PointAlongPlane(PtFAF, NomDir + 180.0, d0)
				ptFarD = PointAlongPlane(PtFAF, NomDir + 180.0, d1)

				azt_Far = ReturnAngleInDegrees(ptFNavPrj, ptFarD)
				azt_Near = ReturnAngleInDegrees(ptFNavPrj, ptNearD)

				ReDim ValidNavs(J).ValMax(0)
				ReDim ValidNavs(J).ValMin(0)

				ValidNavs(J).ValCnt = LeftRightSide

				'        ValidNavs(J).ValMax(0) = Round(Dir2Azt(ptFNavPrj, azt_Far) - 0.4999)
				'        ValidNavs(J).ValMin(0) = Round(Dir2Azt(ptFNavPrj, azt_Near) + 0.4999)
				'        If SubtractAngles(ValidNavs(J).ValMax(0), ValidNavs(J).ValMin(0)) < InterToler Then
				'            Continue For
				'        End If

				If LeftRightSide > 0 Then
					ValidNavs(J).ValMax(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Far) - 0.4999)
					ValidNavs(J).ValMin(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Near) + 0.4999)
				Else
					ValidNavs(J).ValMin(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Far) + 0.4999)
					ValidNavs(J).ValMax(0) = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Near) - 0.4999)
				End If
				If SubtractAngles(ValidNavs(J).ValMax(0) + InterToler, ValidNavs(J).ValMin(0) - InterToler) < InterToler Then
					Continue For
				End If

				ValidNavs(J).IntersectionType = eIntersectionType.ByAngle
				J = J + 1
			End If
		Next I

		Hequip = PtFAF.Z + fRefH

		For I = 0 To UBound(DMEList)
			ValidNavs(J) = DMEList(I)
			ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			LeftRightSide = SideDef(ptNear, NomDir + 180.0, ptFNavPrj)
			AheadBehindSide = SideDef(ptNear, NomDir - 90.0, ptFNavPrj) 'ptFar

			IntrH.Left = minDist
			IntrH.Right = MaxDist

			If AheadBehindSide < 0 Then
				fTmp = ReturnDistanceInMeters(ptFNavPrj, ptNear)
			ElseIf LeftRightSide > 0 Then
				fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.ToPoint)
			Else
				fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.FromPoint)
			End If

			If fTmp > DME.Range Then Continue For '   Range checking

			If LeftRightSide <> 0 Then
				ptMin23 = New ESRI.ArcGIS.Geometry.Point
				ptMax23 = New ESRI.ArcGIS.Geometry.Point
				Construct = ptMin23
				Construct.ConstructAngleIntersection(PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value))
				Construct = ptMax23
				Construct.ConstructAngleIntersection(PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value))
			Else
				ptMin23 = ptFNavPrj
				ptMax23 = ptFNavPrj
			End If

			Intr23.Left = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90.0) * SideDef(PtFAF, NomDir - 90.0, ptMin23)
			Intr23.Right = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90.0) * SideDef(PtFAF, NomDir - 90.0, ptMax23)

			fTmp = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir + TrackToler) + 5.0
			D = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir - TrackToler) + 5.0
			If D < fTmp Then D = fTmp
			fMinDMEDist = (DME.MinimalError + D) / (1.0 - DME.ErrorScalingUp)

			If CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir + 180.0, ptMin23) > 0 Then
				CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir, ptMax23)
				A = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90.0) * SideDef(PtFAF, NomDir - 90.0, ptMax23)
				B = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90.0) * SideDef(PtFAF, NomDir - 90.0, ptMin23)
				If Intr23.Left > A Then Intr23.Left = A
				If Intr23.Right < B Then Intr23.Right = B
			End If

			IntrRes = IntervalsDifference(IntrH, Intr23)
			If UBound(IntrRes) < 0 Then Continue For

			'        fTmp = CircleVectorIntersect(ptFNavPrj, maxDist, ptFAF, NomDir + 180.0, ptTmp)

			Xs = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir + 90.0) * SideDef(PtFAF, NomDir - 90.0, ptFNavPrj)
			Ys = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)

			'Hequip1 = Hequip - ptFNavPrj.Z
			'If Hequip1 < 0 Then Hequip1 = 0
			'======================= 3700 m ==============================================================
			A = 1.0 + arImDescent_Max.Value * arImDescent_Max.Value
			B = 2.0 * arImDescent_Max.Value * Hequip - Xs
			C = Hequip * Hequip + Xs * Xs + Ys * Ys - ((arIFTolerance.Value * System.Math.Cos(DegToRad(arTP_by_DME_div.Value)) - DME.MinimalError) / DME.ErrorScalingUp) ^ 2
			D = B * B - 4.0 * A * C

			If D <= 0.0 Then Continue For

			D = System.Math.Sqrt(D)
			If A > 0 Then
				Intr3700.Left = 0.5 * (-B - D) / A
				Intr3700.Right = 0.5 * (-B + D) / A
			Else
				Intr3700.Left = 0.5 * (-B + D) / A
				Intr3700.Right = 0.5 * (-B - D) / A
			End If

			If IntrH.Left < Intr3700.Left Then IntrH.Left = Intr3700.Left
			If IntrH.Right > Intr3700.Right Then IntrH.Right = Intr3700.Right

			If IntrH.Left >= IntrH.Right Then Continue For
			'========================= 55 deg ==================================================================
			fTmp = 1.0 / System.Math.Tan(DegToRad(DME.SlantAngle))
			fTmp = fTmp * fTmp

			A = arImDescent_Max.Value * arImDescent_Max.Value - fTmp
			B = 2.0 * ((Hequip - ptFNavPrj.Z) * arImDescent_Max.Value + Xs * fTmp)
			C = (Hequip - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
			D = B * B - 4.0 * A * C

			If D > 0.0 Then
				D = System.Math.Sqrt(D)

				If A > 0 Then
					Intr55.Left = 0.5 * (-B - D) / A
					Intr55.Right = 0.5 * (-B + D) / A
				Else
					Intr55.Left = 0.5 * (-B + D) / A
					Intr55.Right = 0.5 * (-B - D) / A
				End If

				'Set ptTmp = PointAlongPlane(ptFAF, NomDir + 180.0, Intr55.Left)
				'DrawPoint ptTmp, 0
				'Set ptTmp = PointAlongPlane(ptFAF, NomDir + 180.0, Intr55.Right)
				'DrawPoint ptTmp, 255

				N = UBound(IntrRes)
				ReDim IntrRes1(-1)

				For ii = 0 To N
					IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)

					If UBound(IntrRes1) < 0 Then
						IntrRes1 = IntrRes2
					Else
						L = UBound(IntrRes1)
						M = UBound(IntrRes2)
						If M >= 0 Then
							ReDim Preserve IntrRes1(L + M + 1)

							For jj = 0 To M
								IntrRes1(jj + L + 1) = IntrRes2(jj)
							Next jj
						End If
					End If
				Next ii

				IntrRes = IntrRes1
			End If

			N = UBound(IntrRes)

			ii = 0
			If N >= 0 Then
				Do
					If IntrRes(ii).Left = IntrRes(ii).Right Then
						For jj = ii To N - 1
							IntrRes(jj) = IntrRes(jj + 1)
						Next jj
						N = N - 1
					Else
						ii = ii + 1
					End If
				Loop While ii < N - 1
			End If

			ii = 0
			While ii < N - 1
				If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
					IntrRes(ii).Right = IntrRes(ii + 1).Right
					For jj = ii + 1 To N - 1
						IntrRes(jj) = IntrRes(jj + 1)
					Next jj
					N = N - 1
				Else
					ii = ii + 1
				End If
			End While

			If N < 0 Then Continue For

			ReDim IntrRes1(N)
			M = 0

			For ii = 0 To N
				ptNearD = PointAlongPlane(PtFAF, NomDir + 180.0, IntrRes(ii).Left)
				ptFarD = PointAlongPlane(PtFAF, NomDir + 180.0, IntrRes(ii).Right)
				d1 = ReturnDistanceInMeters(ptNearD, ptFNavPrj)
				KKhMinDME = New ESRI.ArcGIS.Geometry.Polyline
				KKhMinDME.FromPoint = ptNearD
				KKhMinDME.ToPoint = ptNearD
				'DrawPointWithText ptFarD, "ptFarD", 255
				'DrawPointWithText ptNearD, "ptNearD", 0

				KKhMaxDME.FromPoint = PointAlongPlane(ptFarD, NomDir - 90.0, arIFHalfWidth.Value)
				KKhMaxDME.ToPoint = PointAlongPlane(ptFarD, NomDir + 90.0, arIFHalfWidth.Value)

				KKhMaxDME = pTopoOper.Intersect(KKhMaxDME, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
				If KKhMaxDME.IsEmpty() Then
					KKhMaxDME.FromPoint = ptFarD
					KKhMaxDME.ToPoint = ptFarD
				ElseIf SideDef(ptFarD, NomDir + 180.0, KKhMaxDME.FromPoint) < 0 Then
					KKhMaxDME.ReverseOrientation()
				End If

				IntrRes1(M) = CalcDMERange(PtFAF, PtFAF.Z, fRefH, NomDir + 180.0, arImDescent_Max.Value, ptFNavPrj, KKhMinDME, KKhMaxDME)
				If ii = 0 Then
					If AheadBehindSide < 0 Then
						If IntrRes1(M).Left > d1 Then
							IntrRes1(M).Left = d1
						End If
					Else
						If IntrRes1(M).Right < d1 Then
							IntrRes1(M).Right = d1
						End If
					End If
				End If

				If IntrRes1(M).Left < IntrRes1(M).Right Then
					M = M + 1
					ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90.0, ptFNavPrj)
				End If
			Next ii

			M = M - 1
			If M < 0 Then Continue For

			If M > 0 Then ValidNavs(J).ValCnt = 0

			ReDim ValidNavs(J).ValMax(M)
			ReDim ValidNavs(J).ValMin(M)

			For ii = 0 To M
				ValidNavs(J).ValMin(ii) = System.Math.Round(IntrRes1(ii).Left + 0.4999)
				ValidNavs(J).ValMax(ii) = Int(IntrRes1(ii).Right)
			Next ii

			If ValidNavs(J).ValMax(0) < ValidNavs(J).ValMin(0) Then Continue For

			ValidNavs(J).IntersectionType = eIntersectionType.ByDistance
			J = J + 1
		Next I

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	Public Function GetValidMAPtNavs(ByVal PtFAF As ESRI.ArcGIS.Geometry.IPoint, ByVal minDist As Double, ByVal MaxDist As Double, ByVal NomDir As Double, ByVal OCH As Double, ByVal GuidNav As NavaidData) As NavaidData()
		Dim ValidNavs() As NavaidData
		Dim AheadBehindKKhMax As Integer
		Dim AheadBehindSide As Integer
		Dim LeftRightSide As Integer

		Dim Side As Integer
		Dim nNav As Integer
		Dim ii As Integer
		Dim jj As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As eNavaidType
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer

		Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint

		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint

		Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNear As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFar As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Dim Dir_MinL2MaxR As Double
		Dim Dir_MinR2MaxL As Double
		Dim fMinDMEDist As Double
		Dim InterToler As Double
		Dim TrackToler As Double
		Dim AztNear As Double
		Dim AztFar As Double
		Dim fTmp As Double
		Dim Xs As Double
		Dim Ys As Double
		Dim A As Double
		Dim B As Double
		Dim C As Double
		Dim D As Double

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim Construct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim Intr23 As Interval
		Dim Intr55 As Interval
		Dim IntrH As Interval

		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval
		Dim IntrRes() As Interval

		Dim KKhMinDME As ESRI.ArcGIS.Geometry.IPolyline
		Dim Cutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMin As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMax As ESRI.ArcGIS.Geometry.IPolyline
		'===========================================================================
		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		AheadBehindSide = SideDef(PtFAF, NomDir + 90.0, GuidNav.pPtPrj)
		pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		pGuidPoly.AddPoint(GuidNav.pPtPrj)

		KKhMinDME = New ESRI.ArcGIS.Geometry.Polyline

		If (GuidNav.TypeCode = eNavaidType.VOR) Or (GuidNav.TypeCode = eNavaidType.TACAN) Then
			TrackToler = VOR.TrackingTolerance
		ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
		ElseIf GuidNav.TypeCode = eNavaidType.LLZ Then
			TrackToler = LLZ.TrackingTolerance
		End If

		pGuidPoly.AddPoint(GuidNav.pPtPrj)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(GuidNav.pPtPrj)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler + 180.0, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler + 180.0, 3.0 * MaxModelRadius))
		pGuidPoly.AddPoint(GuidNav.pPtPrj)

		pTopoOper = pGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		ptFar = PointAlongPlane(PtFAF, NomDir, minDist)
		ptNear = PointAlongPlane(PtFAF, NomDir, MaxDist)
		'=============================================================================================
		Cutter = New ESRI.ArcGIS.Geometry.Polyline

		Cutter.FromPoint = PointAlongPlane(ptFar, NomDir - 90.0, MaxModelRadius)
		Cutter.ToPoint = PointAlongPlane(ptFar, NomDir + 90.0, MaxModelRadius)

		KKhMin = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If KKhMin.IsEmpty() Then
			KKhMin.FromPoint = ptFar
			KKhMin.ToPoint = ptFar
		ElseIf SideDef(ptFar, NomDir, KKhMin.FromPoint) < 0 Then
			KKhMin.ReverseOrientation()
		End If

		Cutter.FromPoint = PointAlongPlane(ptNear, NomDir - 90.0, MaxModelRadius)
		Cutter.ToPoint = PointAlongPlane(ptNear, NomDir + 90.0, MaxModelRadius)

		KKhMax = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If KKhMax.IsEmpty() Then
			KKhMax.FromPoint = ptNear
			KKhMax.ToPoint = ptNear
		ElseIf SideDef(ptNear, NomDir, KKhMax.FromPoint) < 0 Then
			KKhMax.ReverseOrientation()
		End If

		Dir_MinL2MaxR = ReturnAngleInDegrees(KKhMin.ToPoint, KKhMax.FromPoint)
		Dir_MinR2MaxL = ReturnAngleInDegrees(KKhMin.FromPoint, KKhMax.ToPoint)
		'=============================================================================================
		pProxi = pGuidPoly
		ptTmp = New ESRI.ArcGIS.Geometry.Point

		J = 0
		I = -1

		ReDim ValidNavs(nNav - 1)

		For I = 0 To UBound(NavaidList)
			ValidNavs(J) = NavaidList(I)
			ptFNav = NavaidList(I).pPtGeo
			ptFNavPrj = NavaidList(I).pPtPrj
			K = NavaidList(I).TypeCode

			LeftRightSide = SideDef(ptFar, NomDir, ptFNavPrj)
			AheadBehindSide = SideDef(ptFar, NomDir + 90.0, ptFNavPrj)
			AheadBehindKKhMax = SideDef(ptNear, NomDir + 90.0, ptFNavPrj)

			If (K = eNavaidType.VOR) Or (K = eNavaidType.NDB) Or (K = eNavaidType.TACAN) Then
				If pProxi.ReturnDistance(ptFNavPrj) = 0.0 Then Continue For

				If K = eNavaidType.NDB Then
					InterToler = NDB.IntersectingTolerance
				Else
					InterToler = VOR.IntersectingTolerance
				End If

				Side = SideDef(KKhMax.FromPoint, Dir_MinL2MaxR, ptFNavPrj)
				If Side * LeftRightSide < 0 Then Continue For
				Side = SideDef(KKhMax.ToPoint, Dir_MinR2MaxL, ptFNavPrj)
				If Side * LeftRightSide < 0 Then Continue For

				If LeftRightSide > 0 Then
					If AheadBehindSide < 0 Then
						ptNearD = KKhMin.FromPoint
						ptFarD = KKhMax.ToPoint
					ElseIf AheadBehindKKhMax < 0 Then
						ptNearD = KKhMin.ToPoint
						ptFarD = KKhMax.ToPoint
					Else
						ptNearD = KKhMin.ToPoint
						ptFarD = KKhMax.FromPoint
					End If
				Else
					If AheadBehindSide < 0 Then
						ptNearD = KKhMin.ToPoint
						ptFarD = KKhMax.FromPoint
					ElseIf AheadBehindKKhMax < 0 Then
						ptNearD = KKhMin.FromPoint
						ptFarD = KKhMax.FromPoint
					Else
						ptNearD = KKhMin.FromPoint
						ptFarD = KKhMax.ToPoint
					End If
				End If

				'        If ERange < ReturnDistanceInMeters(ptFNavPrj, ptFar) Then Continue For

				AztFar = ReturnAngleInDegrees(ptFNavPrj, ptFarD)
				AztNear = ReturnAngleInDegrees(ptFNavPrj, ptNearD)

				If SubtractAngles(AztNear, AztFar) < 2.0 * InterToler Then Continue For

				ReDim ValidNavs(J).ValMax(0)
				ReDim ValidNavs(J).ValMin(0)

				ValidNavs(J).ValCnt = LeftRightSide
				If LeftRightSide > 0 Then
					pConstruct = ptTmp
					pConstruct.ConstructAngleIntersection(ptFNavPrj, DegToRad(AztFar + InterToler), PtFAF, DegToRad(NomDir))
					ValidNavs(J).ValMin(0) = System.Math.Round(ReturnDistanceInMeters(ptTmp, PtFAF) - 0.4999)

					pConstruct.ConstructAngleIntersection(ptFNavPrj, DegToRad(AztNear - InterToler), PtFAF, DegToRad(NomDir))
					ValidNavs(J).ValMax(0) = System.Math.Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
				Else
					pConstruct = ptTmp
					pConstruct.ConstructAngleIntersection(ptFNavPrj, DegToRad(AztFar - InterToler), PtFAF, DegToRad(NomDir))
					ValidNavs(J).ValMin(0) = System.Math.Round(ReturnDistanceInMeters(ptTmp, PtFAF) - 0.4999)

					pConstruct.ConstructAngleIntersection(ptFNavPrj, DegToRad(AztNear + InterToler), PtFAF, DegToRad(NomDir))
					ValidNavs(J).ValMax(0) = System.Math.Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
				End If

				ValidNavs(J).IntersectionType = eIntersectionType.ByAngle
				J = J + 1
			End If
		Next I


		For I = 0 To UBound(DMEList)
			ValidNavs(J) = DMEList(I)
			ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			IntrH.Left = minDist
			IntrH.Right = MaxDist
			'=========================================================================================
			If LeftRightSide > 0 Then
				If AheadBehindSide < 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMin.ToPoint)
				Else
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.ToPoint)
				End If
			Else
				If AheadBehindSide < 0 Then
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMin.FromPoint)
				Else
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.FromPoint)
				End If
			End If

			If fTmp > DME.Range Then Continue For '   Range checking

			If LeftRightSide <> 0 Then
				ptMin23 = New ESRI.ArcGIS.Geometry.Point
				ptMax23 = New ESRI.ArcGIS.Geometry.Point
				Construct = ptMin23
				Construct.ConstructAngleIntersection(PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * arTP_by_DME_div.Value))
				Construct = ptMax23
				Construct.ConstructAngleIntersection(PtFAF, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * arTP_by_DME_div.Value))
			Else
				ptMin23 = ptFNavPrj
				ptMax23 = ptFNavPrj
			End If
			'=========================================================================================

			Intr23.Left = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90.0) * SideDef(PtFAF, NomDir + 90.0, ptMin23)
			Intr23.Right = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90.0) * SideDef(PtFAF, NomDir + 90.0, ptMax23)

			fTmp = Point2LineDistancePrj(ptFNavPrj, GuidNav.pPtPrj, NomDir + TrackToler) + 5.0
			D = Point2LineDistancePrj(ptFNavPrj, GuidNav.pPtPrj, NomDir - TrackToler) + 5.0
			If D < fTmp Then D = fTmp
			fMinDMEDist = (DME.MinimalError + D) / (1.0 - DME.ErrorScalingUp)
			'        If fMinDMEDist < InitWidth Then fMinDMEDist = InitWidth

			If CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir + 180.0, ptMin23) > 0 Then
				CircleVectorIntersect(ptFNavPrj, fMinDMEDist, PtFAF, NomDir, ptMax23)
				A = Point2LineDistancePrj(PtFAF, ptMin23, NomDir + 90.0) * SideDef(PtFAF, NomDir + 90.0, ptMin23)
				B = Point2LineDistancePrj(PtFAF, ptMax23, NomDir + 90.0) * SideDef(PtFAF, NomDir + 90.0, ptMax23)
				If Intr23.Left > A Then Intr23.Left = A
				If Intr23.Right < B Then Intr23.Right = B
			End If

			IntrRes = IntervalsDifference(IntrH, Intr23)
			If UBound(IntrRes) < 0 Then Continue For

			Xs = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir + 90.0) * SideDef(PtFAF, NomDir + 90.0, ptFNavPrj)
			Ys = Point2LineDistancePrj(ptFNavPrj, PtFAF, NomDir)
			'================================== SlantAngle =================================================
			fTmp = 1.0 / System.Math.Tan(DegToRad(DME.SlantAngle))
			fTmp = fTmp * fTmp

			A = arImDescent_Max.Value * arImDescent_Max.Value - fTmp
			B = 2.0 * ((OCH - ptFNavPrj.Z) * arImDescent_Max.Value + Xs * fTmp)
			C = (OCH - ptFNavPrj.Z) ^ 2 - (Xs * Xs + Ys * Ys) * fTmp
			D = B * B - 4.0 * A * C

			If D > 0.0 Then
				If A > 0 Then
					Intr55.Left = 0.5 * (-B - System.Math.Sqrt(D)) / A
					Intr55.Right = 0.5 * (-B + System.Math.Sqrt(D)) / A
				Else
					Intr55.Left = 0.5 * (-B + System.Math.Sqrt(D)) / A
					Intr55.Right = 0.5 * (-B - System.Math.Sqrt(D)) / A
				End If

				N = UBound(IntrRes)
				ReDim IntrRes1(-1)

				For ii = 0 To N
					IntrRes2 = IntervalsDifference(IntrRes(ii), Intr55)

					If UBound(IntrRes1) < 0 Then
						IntrRes1 = IntrRes2
					Else
						L = UBound(IntrRes1)
						M = UBound(IntrRes2)
						If M >= 0 Then
							ReDim Preserve IntrRes1(L + M + 1)

							For jj = 0 To M
								IntrRes1(jj + L + 1) = IntrRes2(jj)
							Next jj
						End If
					End If
				Next ii

				IntrRes = IntrRes1
			End If

			N = UBound(IntrRes)

			ii = 0
			If N >= 0 Then
				Do
					If IntrRes(ii).Left = IntrRes(ii).Right Then
						For jj = ii To N - 1
							IntrRes(jj) = IntrRes(jj + 1)
						Next jj
						N = N - 1
					Else
						ii = ii + 1
					End If
				Loop While ii < N - 1
			End If

			ii = 0
			While ii < N - 1
				If IntrRes(ii).Right = IntrRes(ii + 1).Left Then
					IntrRes(ii).Right = IntrRes(ii + 1).Right
					For jj = ii + 1 To N - 1
						IntrRes(jj) = IntrRes(jj + 1)
					Next jj
					N = N - 1
				Else
					ii = ii + 1
				End If
			End While

			If N < 0 Then Continue For

			'        ReDim IntrRes1(N)
			ReDim ValidNavs(J).ValMax(N)
			ReDim ValidNavs(J).ValMin(N)

			M = 0
			'===========================================================================================
			For ii = 0 To N
				ptNearD = PointAlongPlane(PtFAF, NomDir, IntrRes(ii).Left)
				'               Set ptFarD = PointAlongPlane(PtFAF, NomDir, IntrRes(ii).Right)

				Cutter.FromPoint = PointAlongPlane(ptNearD, NomDir - 90.0, 3.0 * MaxModelRadius)
				Cutter.ToPoint = PointAlongPlane(ptNearD, NomDir + 90.0, 3.0 * MaxModelRadius)

				KKhMinDME = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
				If KKhMinDME.IsEmpty() Then
					KKhMinDME.FromPoint = ptNearD
					KKhMinDME.ToPoint = ptNearD
				ElseIf SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0 Then
					KKhMinDME.ReverseOrientation()
				End If

				If IntrRes(M).Left < IntrRes(M).Right Then
					ValidNavs(J).ValMax(M) = System.Math.Round(IntrRes(M).Right - 0.4999)
					ValidNavs(J).ValMin(M) = System.Math.Round(IntrRes(M).Left + 0.4999)
					M = M + 1
					ValidNavs(J).ValCnt = SideDef(KKhMinDME.FromPoint, NomDir + 90.0, ptFNavPrj)
				End If
			Next ii

			M = M - 1
			If M < 0 Then Continue For

			If M > 0 Then ValidNavs(J).ValCnt = 0
			ReDim Preserve ValidNavs(J).ValMax(M)
			ReDim Preserve ValidNavs(J).ValMin(M)

			'        If M > 0 Then
			'            CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir, ptTmp
			'            ValidNavs(J).ValMin(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) - 0.4999)
			'
			'            CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir, ptTmp
			'            ValidNavs(J).ValMax(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
			'
			'            CircleVectorIntersect ptFNavPrj, IntrRes1(1).Left, PtFAF, NomDir, ptTmp
			'            ValidNavs(J).ValMin(1) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
			'
			'            CircleVectorIntersect ptFNavPrj, IntrRes1(1).Right, PtFAF, NomDir, ptTmp
			'            ValidNavs(J).ValMax(1) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
			'        Else
			''            If ValidNavs(J).ValCnt < 0 Then
			'            If AheadBehindSide < 0 Then
			'                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir, ptTmp
			'                ValidNavs(J).ValMin(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) - 0.4999)
			'
			'                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir, ptTmp
			'                ValidNavs(J).ValMax(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
			'            Else
			'                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Left, PtFAF, NomDir + 180.0, ptTmp
			'                ValidNavs(J).ValMin(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) - 0.4999)
			'
			'                CircleVectorIntersect ptFNavPrj, IntrRes1(0).Right, PtFAF, NomDir + 180.0, ptTmp
			'                ValidNavs(J).ValMax(0) = Round(ReturnDistanceInMeters(ptTmp, PtFAF) + 0.4999)
			'            End If
			'        End If

			ValidNavs(J).IntersectionType = eIntersectionType.ByDistance
			J = J + 1
		Next I

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	Public Function PolygonIntersection(ByVal pPoly1 As ESRI.ArcGIS.Geometry.Polygon, ByVal pPoly2 As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTmpPoly0 As ESRI.ArcGIS.Geometry.Polygon
		Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.Polygon
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone

		pClone = pPoly1
		pTmpPoly0 = pClone.Clone

		pTopo = pTmpPoly0
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		pClone = pPoly2
		pTmpPoly1 = pClone.Clone
		pTopo = pTmpPoly1
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		PolygonIntersection = pTopo.Intersect(pTmpPoly0, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
	End Function

	'Public Function PolygonIntersection1(ByVal pPoly1 As ESRI.ArcGIS.Geometry.Polygon, ByVal pPoly2 As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
	'	Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'	Dim pTmpPoly0 As ESRI.ArcGIS.Geometry.Polygon
	'	Dim pTmpPoly1 As ESRI.ArcGIS.Geometry.Polygon

	'	pTopo = pPoly2
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	pTopo = pPoly1
	'	pTopo.IsKnownSimple_2 = False
	'	pTopo.Simplify()

	'	On Error Resume Next
	'	PolygonIntersection1 = pTopo.Intersect(pPoly2, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
	'	If Err.Number = 0 Then Exit Function

	'	Err.Clear()
	'	pTmpPoly0 = pTopo.Union(pPoly2)
	'	pTmpPoly1 = pTopo.SymmetricDifference(pPoly2)

	'	pTopo = pTmpPoly0
	'	PolygonIntersection1 = pTopo.Difference(pTmpPoly1)
	'	If Err.Number = 0 Then Exit Function
	'	PolygonIntersection1 = pPoly2
	'End Function

	Function RemoveFars(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon, ByVal pPoint As ESRI.ArcGIS.Geometry.Point) As ESRI.ArcGIS.Geometry.Polygon
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
				tmpDist = pProxi.ReturnDistance(DirectCast(lCollect, IGeometry))
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

	Function RemoveSmalls(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
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

	Function RemoveHoles(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim NewPolygon As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pInteriorRing As ESRI.ArcGIS.Geometry.IRing
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim I As Integer

		pClone = pPolygon

		NewPolygon = pClone.Clone

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

		Return NewPolygon
	End Function

	Function RemoveAgnails(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polygon
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

		pTopo = pPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		N = pPoly.PointCount - 1

		If N <= 3 Then Return pPoly

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

	Public Function ReArrangePolygon(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon, ByVal PtDerL As ESRI.ArcGIS.Geometry.IPoint, ByVal CLDir As Double, Optional ByVal bFlag As Boolean = False) As ESRI.ArcGIS.Geometry.Polygon
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
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPt As ESRI.ArcGIS.Geometry.IPoint
		Dim Result As ESRI.ArcGIS.Geometry.IPointCollection

		pClone = pPolygon
		pTmpPoly = pClone.Clone

		pTopoOper = pTmpPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		If pTmpPoly.PointCount <= 3 Then
			Return New ESRI.ArcGIS.Geometry.Polygon
		End If

		pPoly = New ESRI.ArcGIS.Geometry.Polyline
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
				N = N - 1
				J = Modulus(I + 1, N)
				If I <= iStart Then iStart = iStart - 1

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
						If I <= iStart Then iStart = iStart - 1
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
		Result = New ESRI.ArcGIS.Geometry.Polygon

		For I = N - 1 To 0 Step -1
			J = Modulus(I + iStart, N)
			Result.AddPoint(pPoly.Point(J))
		Next

		Return Result
	End Function

	Function CalcTrajectoryFromMultiPoint(ByVal MultiPoint As ESRI.ArcGIS.Geometry.IPointCollection) As ESRI.ArcGIS.Geometry.Polyline
		Dim ptConstr As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim FromPt As ESRI.ArcGIS.Geometry.IPoint
		Dim CntPt As ESRI.ArcGIS.Geometry.IPoint
		Dim ToPt As ESRI.ArcGIS.Geometry.IPoint

		Dim fTmp As Double
		Dim fE As Double

		Dim Side As Integer
		Dim I As Integer
		Dim N As Integer

		Dim pPolyline As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pPath As ESRI.ArcGIS.Geometry.IPointCollection

		CntPt = New ESRI.ArcGIS.Geometry.Point
		ptConstr = CntPt
		fE = 0.5 * DegToRadValue

		Dim Result As ESRI.ArcGIS.Geometry.Polyline

		Result = New ESRI.ArcGIS.Geometry.Polyline
		pPolyline = Result

		N = MultiPoint.PointCount - 2

		For I = 0 To N
			FromPt = MultiPoint.Point(I)
			ToPt = MultiPoint.Point(I + 1)
			fTmp = DegToRadValue * (FromPt.M - ToPt.M)

			If (System.Math.Abs(System.Math.Sin(fTmp)) <= fE) And (System.Math.Cos(fTmp) > 0.0) Then
				pPath = New ESRI.ArcGIS.Geometry.Path
				pPath.AddPoint(FromPt)
				pPath.AddPoint(ToPt)
				pPolyline.AddGeometry(pPath)
			Else
				If System.Math.Abs(System.Math.Sin(fTmp)) > fE Then
					ptConstr.ConstructAngleIntersection(FromPt, DegToRadValue * (FromPt.M + 90.0), ToPt, DegToRadValue * (ToPt.M + 90.0))
				Else
					CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y))
				End If

				Side = SideDef(FromPt, FromPt.M, ToPt)

				pPath = New ESRI.ArcGIS.Geometry.Path
				pPath.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -Side))
				pPolyline.AddGeometry(pPath)
			End If
		Next

		Return Result
	End Function

	Public Function CreateBasePoints(ByVal pPolygone As ESRI.ArcGIS.Geometry.IPointCollection, ByVal K1K1 As ESRI.ArcGIS.Geometry.IPolyline, ByVal lDepDir As Double, ByVal lTurnDir As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim bFlg As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim Side As Integer

		bFlg = False
		N = pPolygone.PointCount
		tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
		CreateBasePoints = New ESRI.ArcGIS.Geometry.Polygon

		If lTurnDir > 0 Then
			For I = 0 To N - 1
				Side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
				If (Side < 0) Then
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
				Side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
				If (Side < 0) Then
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

	Public Function TurnToFixPrj(ByVal PtSt As ESRI.ArcGIS.Geometry.Point, ByVal TurnR As Double, ByVal TurnDir As Integer, ByVal FixPnt As ESRI.ArcGIS.Geometry.IPoint) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim DeltaAngle As Double
		Dim DirFx2Cnt As Double
		Dim DistFx2Cnt As Double
		Dim DirCur As Double

		DirCur = PtSt.M

		TurnToFixPrj = New ESRI.ArcGIS.Geometry.Multipoint
		ptCnt = PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR)

		DistFx2Cnt = ReturnDistanceInMeters(ptCnt, FixPnt)

		If DistFx2Cnt < TurnR Then
			TurnR = DistFx2Cnt
			Exit Function
		End If

		DirFx2Cnt = ReturnAngleInDegrees(ptCnt, FixPnt)
		DeltaAngle = -RadToDeg(ArcCos(TurnR / DistFx2Cnt)) * TurnDir

		pt1 = PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR)

		pt1.M = ReturnAngleInDegrees(pt1, FixPnt)

		TurnToFixPrj.AddPoint(PtSt)
		TurnToFixPrj.AddPoint(pt1)
	End Function

	Public Function ReturnPolygonPartAsPolyline(ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon, ByVal PtDerL As ESRI.ArcGIS.Geometry.IPoint, ByVal CLDir As Double, ByVal Turn As Integer) As ESRI.ArcGIS.Geometry.Polyline
		Dim I As Integer
		Dim N As Integer
		Dim Side As Integer
		Dim pPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Result As ESRI.ArcGIS.Geometry.IPointCollection

		Result = New ESRI.ArcGIS.Geometry.Polyline

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

		If Turn < 0 Then
			pLine = Result
			pLine.ReverseOrientation()
		End If

		Return Result
	End Function

	'==========================================================================================================================================================
	Public Sub CreateOFZPlanes(ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double, ByVal H As Double, ByRef OFZPlanes() As D3DPolygone)
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt4 As ESRI.ArcGIS.Geometry.IPoint

		Dim I As Integer
		Dim N As Integer

		pt1 = PointAlongPlane(ptLHPrj, ArDir + 180.0, (H - 27.0 + 1.2) * 50.0)
		pt2 = PointAlongPlane(ptLHPrj, ArDir + 180.0, 60.0)
		pt3 = PointAlongPlane(ptLHPrj, ArDir, 1800.0)
		Pt4 = PointAlongPlane(ptLHPrj, ArDir, (H + 59.94) * 30.03)

		pt2.Z = 0.0
		pt3.Z = 0.0

		N = UBound(OFZPlanes)

		For I = 0 To N
			OFZPlanes(I).Poly = New ESRI.ArcGIS.Geometry.Polygon
		Next I

		'=================================================================
		OFZPlanes(0).Poly.AddPoint(PointAlongPlane(pt2, ArDir - 90.0, 60.0))
		OFZPlanes(0).Poly.AddPoint(PointAlongPlane(pt2, ArDir + 90.0, 60.0))
		OFZPlanes(0).Poly.AddPoint(PointAlongPlane(pt3, ArDir + 90.0, 60.0))
		OFZPlanes(0).Poly.AddPoint(PointAlongPlane(pt3, ArDir - 90.0, 60.0))

		OFZPlanes(0).Plane.pPt = pt2
		OFZPlanes(0).Plane.X = 0.0
		OFZPlanes(0).Plane.Y = 0.0
		OFZPlanes(0).Plane.Z = 1.0

		OFZPlanes(0).Plane.A = 0.0
		OFZPlanes(0).Plane.B = 0.0
		OFZPlanes(0).Plane.C = -1.0
		OFZPlanes(0).Plane.D = 0.0

		'=================================================================
		OFZPlanes(1).Poly.AddPoint(PointAlongPlane(pt1, ArDir - 90.0, 60.0))
		OFZPlanes(1).Poly.AddPoint(PointAlongPlane(pt1, ArDir + 90.0, 60.0))
		OFZPlanes(1).Poly.AddPoint(OFZPlanes(0).Poly.Point(1))
		OFZPlanes(1).Poly.AddPoint(OFZPlanes(0).Poly.Point(0))

		OFZPlanes(1).Plane.pPt = pt2
		OFZPlanes(1).Plane.A = 0.02
		OFZPlanes(1).Plane.B = 0.0
		OFZPlanes(1).Plane.C = -1.0
		OFZPlanes(1).Plane.D = -1.2

		'=================================================================
		OFZPlanes(2).Poly.AddPoint(OFZPlanes(1).Poly.Point(1))
		OFZPlanes(2).Poly.AddPoint(PointAlongPlane(pt1, ArDir + 90.0, 3.003 * (21.18 - 1.2 + 27.0)))
		OFZPlanes(2).Poly.AddPoint(PointAlongPlane(pt2, ArDir + 90.0, 3.003 * (H + 21.18 - 0.02 * 60.0)))
		OFZPlanes(2).Poly.AddPoint(OFZPlanes(1).Poly.Point(2))

		OFZPlanes(2).Plane.pPt = OFZPlanes(2).Poly.Point(3)
		OFZPlanes(2).Plane.pPt.Z = 0.0

		OFZPlanes(2).Plane.A = 0.02
		OFZPlanes(2).Plane.B = -0.333
		OFZPlanes(2).Plane.C = -1.0
		OFZPlanes(2).Plane.D = -21.18

		'=================================================================
		OFZPlanes(3).Poly.AddPoint(OFZPlanes(0).Poly.Point(1))
		OFZPlanes(3).Poly.AddPoint(OFZPlanes(2).Poly.Point(2))
		OFZPlanes(3).Poly.AddPoint(PointAlongPlane(Pt4, ArDir + 90.0, 3.003 * (H + 19.98)))
		OFZPlanes(3).Poly.AddPoint(OFZPlanes(0).Poly.Point(2))

		OFZPlanes(3).Plane.pPt = OFZPlanes(3).Poly.Point(3)
		OFZPlanes(3).Plane.pPt.Z = 0.0

		OFZPlanes(3).Plane.A = 0.0
		OFZPlanes(3).Plane.B = -0.333
		OFZPlanes(3).Plane.C = -1.0
		OFZPlanes(3).Plane.D = -19.98

		'=================================================================
		OFZPlanes(4).Poly.AddPoint(OFZPlanes(3).Poly.Point(3))
		OFZPlanes(4).Poly.AddPoint(OFZPlanes(3).Poly.Point(2))
		OFZPlanes(4).Poly.AddPoint(PointAlongPlane(Pt4, ArDir - 90.0, 3.003 * (H + 19.98)))
		OFZPlanes(4).Poly.AddPoint(OFZPlanes(0).Poly.Point(3))

		OFZPlanes(4).Plane.pPt = OFZPlanes(4).Poly.Point(3)
		OFZPlanes(4).Plane.pPt.Z = 0.0

		OFZPlanes(4).Plane.A = -0.0333
		OFZPlanes(4).Plane.B = 0.0
		OFZPlanes(4).Plane.C = -1.0
		OFZPlanes(4).Plane.D = -59.94

		'=================================================================
		OFZPlanes(5).Poly.AddPoint(OFZPlanes(0).Poly.Point(0))
		OFZPlanes(5).Poly.AddPoint(OFZPlanes(0).Poly.Point(3))
		OFZPlanes(5).Poly.AddPoint(OFZPlanes(4).Poly.Point(2))
		OFZPlanes(5).Poly.AddPoint(PointAlongPlane(pt2, ArDir - 90.0, 3.003 * (H + 21.18 - 0.02 * 60.0)))

		OFZPlanes(5).Plane.pPt = OFZPlanes(5).Poly.Point(0)
		OFZPlanes(5).Plane.pPt.Z = 0.0

		OFZPlanes(5).Plane.A = 0.0
		OFZPlanes(5).Plane.B = 0.333
		OFZPlanes(5).Plane.C = -1.0
		OFZPlanes(5).Plane.D = -19.98

		'=================================================================
		OFZPlanes(6).Poly.AddPoint(OFZPlanes(1).Poly.Point(0))
		OFZPlanes(6).Poly.AddPoint(OFZPlanes(1).Poly.Point(3))
		OFZPlanes(6).Poly.AddPoint(OFZPlanes(5).Poly.Point(3))
		OFZPlanes(6).Poly.AddPoint(PointAlongPlane(pt1, ArDir - 90.0, 3.003 * (21.18 - 1.2 + 27.0)))

		OFZPlanes(6).Plane.pPt = OFZPlanes(6).Poly.Point(1)
		OFZPlanes(6).Plane.pPt.Z = 0.0

		OFZPlanes(6).Plane.A = 0.02
		OFZPlanes(6).Plane.B = 0.333
		OFZPlanes(6).Plane.C = -1.0
		OFZPlanes(6).Plane.D = -21.18

		'=================================================================
		For I = 0 To N - 1
			pTopo = OFZPlanes(N).Poly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTopo = OFZPlanes(I).Poly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
			OFZPlanes(N).Poly = pTopo.Union(OFZPlanes(N).Poly)
		Next I
	End Sub

	Public Sub CreateILSPlanes(ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double, ByRef ILSPlanes() As D3DPolygone)
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim ptD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptH As ESRI.ArcGIS.Geometry.IPoint
		Dim PtG As ESRI.ArcGIS.Geometry.IPoint
		Dim ptF As ESRI.ArcGIS.Geometry.IPoint
		Dim ptE As ESRI.ArcGIS.Geometry.IPoint
		Dim ptA As ESRI.ArcGIS.Geometry.IPoint

		Dim I As Integer
		Dim N As Integer
		Dim fAlpha As Double
		Dim fAztPl2 As Double
		Dim fDistPl2 As Double

		ptD = PointAlongPlane(ptLHPrj, ArDir + 180.0, 12660.0)
		ptH = PointAlongPlane(ptLHPrj, ArDir + 180.0, 3060.0)
		PtG = PointAlongPlane(ptLHPrj, ArDir + 180, 60.0)
		ptF = PointAlongPlane(ptLHPrj, ArDir, 900.0)
		ptE = PointAlongPlane(ptLHPrj, ArDir, 2700.0)
		ptA = PointAlongPlane(ptLHPrj, ArDir, 12900.0)

		ptF.Z = 0.0
		PtG.Z = 0.0

		N = UBound(ILSPlanes)

		For I = 0 To N
			ILSPlanes(I).Poly = New ESRI.ArcGIS.Geometry.Polygon
		Next I

		'=================================================================
		ILSPlanes(0).Poly.AddPoint(PointAlongPlane(ptF, ArDir - 90.0, 150.0))
		ILSPlanes(0).Poly.AddPoint(PointAlongPlane(PtG, ArDir - 90.0, 150.0))
		ILSPlanes(0).Poly.AddPoint(PointAlongPlane(PtG, ArDir + 90.0, 150.0))
		ILSPlanes(0).Poly.AddPoint(PointAlongPlane(ptF, ArDir + 90.0, 150.0))

		ILSPlanes(0).Plane.pPt = ptF
		ILSPlanes(0).Plane.X = 0.0
		ILSPlanes(0).Plane.Y = 0.0
		ILSPlanes(0).Plane.Z = 1.0

		ILSPlanes(0).Plane.A = 0.0
		ILSPlanes(0).Plane.B = 0.0
		ILSPlanes(0).Plane.C = -1.0
		ILSPlanes(0).Plane.D = 0.0

		'=================================================================
		ILSPlanes(1).Poly.AddPoint(ILSPlanes(0).Poly.Point(1))
		ILSPlanes(1).Poly.AddPoint(PointAlongPlane(ptH, ArDir - 90.0, 600.0))
		ILSPlanes(1).Poly.AddPoint(PointAlongPlane(ptH, ArDir + 90.0, 600.0))
		ILSPlanes(1).Poly.AddPoint(ILSPlanes(0).Poly.Point(2))

		ILSPlanes(1).Plane.pPt = ptH
		ILSPlanes(1).Plane.A = 0.02
		ILSPlanes(1).Plane.B = 0.0
		ILSPlanes(1).Plane.C = -1.0
		ILSPlanes(1).Plane.D = -1.2

		'=================================================================
		ILSPlanes(2).Poly.AddPoint(ILSPlanes(1).Poly.Point(1))
		ILSPlanes(2).Poly.AddPoint(PointAlongPlane(ptD, ArDir - 90.0, 2040.0))
		ILSPlanes(2).Poly.AddPoint(PointAlongPlane(ptD, ArDir + 90.0, 2040.0))
		ILSPlanes(2).Poly.AddPoint(ILSPlanes(1).Poly.Point(2))

		ILSPlanes(2).Plane.pPt = ptD
		ILSPlanes(2).Plane.pPt.Z = 0.0

		ILSPlanes(2).Plane.A = 0.025
		ILSPlanes(2).Plane.B = 0.0
		ILSPlanes(2).Plane.C = -1.0
		ILSPlanes(2).Plane.D = -16.5

		'=================================================================
		ILSPlanes(3).Poly.AddPoint(ILSPlanes(2).Poly.Point(3))
		ILSPlanes(3).Poly.AddPoint(ILSPlanes(2).Poly.Point(2))
		ILSPlanes(3).Poly.AddPoint(PointAlongPlane(ptH, ArDir + 90.0, 2278.0))

		ILSPlanes(3).Plane.pPt = ptH
		ILSPlanes(3).Plane.pPt.Z = 0.0

		ILSPlanes(3).Plane.A = 0.00355
		ILSPlanes(3).Plane.B = -0.143
		ILSPlanes(3).Plane.C = -1.0
		ILSPlanes(3).Plane.D = -36.66

		'=================================================================
		ILSPlanes(11).Poly.AddPoint(ILSPlanes(2).Poly.Point(0))
		ILSPlanes(11).Poly.AddPoint(PointAlongPlane(ptH, ArDir - 90.0, 2278.0))
		ILSPlanes(11).Poly.AddPoint(ILSPlanes(2).Poly.Point(1))

		ILSPlanes(11).Plane.pPt = ptH

		ILSPlanes(11).Plane.A = 0.00355
		ILSPlanes(11).Plane.B = 0.143
		ILSPlanes(11).Plane.C = -1.0
		ILSPlanes(11).Plane.D = -36.66

		'=================================================================
		ILSPlanes(4).Poly.AddPoint(ILSPlanes(1).Poly.Point(3))
		ILSPlanes(4).Poly.AddPoint(ILSPlanes(1).Poly.Point(2))
		ILSPlanes(4).Poly.AddPoint(ILSPlanes(3).Poly.Point(2))
		ILSPlanes(4).Poly.AddPoint(PointAlongPlane(PtG, ArDir + 90.0, 2248.0))

		ILSPlanes(4).Plane.pPt = PtG

		ILSPlanes(4).Plane.A = -0.00145
		ILSPlanes(4).Plane.B = -0.143
		ILSPlanes(4).Plane.C = -1.0
		ILSPlanes(4).Plane.D = -21.36

		'=================================================================
		ILSPlanes(10).Poly.AddPoint(ILSPlanes(1).Poly.Point(0))
		ILSPlanes(10).Poly.AddPoint(PointAlongPlane(PtG, ArDir - 90.0, 2248.0))
		ILSPlanes(10).Poly.AddPoint(ILSPlanes(11).Poly.Point(1))
		ILSPlanes(10).Poly.AddPoint(ILSPlanes(1).Poly.Point(1))

		ILSPlanes(10).Plane.pPt = PtG

		ILSPlanes(10).Plane.A = -0.00145
		ILSPlanes(10).Plane.B = 0.143
		ILSPlanes(10).Plane.C = -1.0
		ILSPlanes(10).Plane.D = -21.36

		'=================================================================
		ILSPlanes(5).Poly.AddPoint(ILSPlanes(0).Poly.Point(3))
		ILSPlanes(5).Poly.AddPoint(ILSPlanes(0).Poly.Point(2))
		ILSPlanes(5).Poly.AddPoint(ILSPlanes(4).Poly.Point(3))
		ILSPlanes(5).Poly.AddPoint(PointAlongPlane(ptE, ArDir + 90.0, 2248.0))
		ILSPlanes(5).Poly.AddPoint(PointAlongPlane(ptE, ArDir + 90.0, 465.0))

		ILSPlanes(5).Plane.pPt = ptE

		ILSPlanes(5).Plane.A = 0.0
		ILSPlanes(5).Plane.B = -0.143
		ILSPlanes(5).Plane.C = -1.0
		ILSPlanes(5).Plane.D = -21.45

		'=================================================================
		ILSPlanes(9).Poly.AddPoint(PointAlongPlane(ptE, ArDir - 90.0, 465.0))
		ILSPlanes(9).Poly.AddPoint(PointAlongPlane(ptE, ArDir - 90.0, 2248.0))
		ILSPlanes(9).Poly.AddPoint(ILSPlanes(10).Poly.Point(1))
		ILSPlanes(9).Poly.AddPoint(ILSPlanes(0).Poly.Point(1))
		ILSPlanes(9).Poly.AddPoint(ILSPlanes(0).Poly.Point(0))

		ILSPlanes(9).Plane.pPt = ptE

		ILSPlanes(9).Plane.A = 0.0
		ILSPlanes(9).Plane.B = 0.143
		ILSPlanes(9).Plane.C = -1.0
		ILSPlanes(9).Plane.D = -21.45

		'=================================================================
		ILSPlanes(6).Poly.AddPoint(ILSPlanes(5).Poly.Point(4))
		ILSPlanes(6).Poly.AddPoint(ILSPlanes(5).Poly.Point(3))
		ILSPlanes(6).Poly.AddPoint(PointAlongPlane(ptA, ArDir + 90.0, 3015.0))

		ILSPlanes(6).Plane.pPt = ptA

		ILSPlanes(6).Plane.A = 0.01075
		ILSPlanes(6).Plane.B = -0.143
		ILSPlanes(6).Plane.C = -1.0
		ILSPlanes(6).Plane.D = 7.58

		'=================================================================
		ILSPlanes(8).Poly.AddPoint(ILSPlanes(9).Poly.Point(0))
		ILSPlanes(8).Poly.AddPoint(PointAlongPlane(ptA, ArDir - 90.0, 3015.0))
		ILSPlanes(8).Poly.AddPoint(ILSPlanes(9).Poly.Point(1))

		ILSPlanes(8).Plane.pPt = ptA

		ILSPlanes(8).Plane.A = 0.01075
		ILSPlanes(8).Plane.B = 0.143
		ILSPlanes(8).Plane.C = -1.0
		ILSPlanes(8).Plane.D = 7.58

		'=================================================================
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(0).Poly.Point(0))
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(0).Poly.Point(3))
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(6).Poly.Point(0))
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(6).Poly.Point(2))
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(8).Poly.Point(1))
		ILSPlanes(7).Poly.AddPoint(ILSPlanes(8).Poly.Point(0))

		ILSPlanes(7).Plane.pPt = ptA

		ILSPlanes(7).Plane.A = -0.025
		ILSPlanes(7).Plane.B = 0.0
		ILSPlanes(7).Plane.C = -1.0
		ILSPlanes(7).Plane.D = -22.5

		'=================================================================
		fAlpha = System.Math.Atan(1440.0 / 9600.0)
		fDistPl2 = (18500.0 - 3060.0) / System.Math.Cos(fAlpha)

		fAztPl2 = ReturnAngleInDegrees(ILSPlanes(2).Poly.Point(0), ILSPlanes(2).Poly.Point(1))
		ILSPlanes(2).Poly.ReplacePoints(1, 1, 1, PointAlongPlane(ILSPlanes(2).Poly.Point(0), fAztPl2, fDistPl2))

		fAztPl2 = ReturnAngleInDegrees(ILSPlanes(2).Poly.Point(3), ILSPlanes(2).Poly.Point(2))
		ILSPlanes(2).Poly.ReplacePoints(2, 1, 1, PointAlongPlane(ILSPlanes(2).Poly.Point(3), fAztPl2, fDistPl2))

		For I = 0 To N - 1
			pTopo = ILSPlanes(N).Poly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			pTopo = ILSPlanes(I).Poly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			ILSPlanes(N).Poly = pTopo.Union(ILSPlanes(N).Poly)
		Next I
	End Sub

	Public Sub CreateOASPlanes(ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double, ByVal hMax As Double, ByRef OASPlanes() As D3DPolygone, ByVal ILSCategory As Integer) ', Optional bDraw As Boolean = False)
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer
		Dim M As Integer
		Dim hCons As Double

		Dim ResLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pFrmPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pToPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTransform As ESRI.ArcGIS.Geometry.ITransform2D
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		If ILSCategory = 0 Then
			hCons = hMax
		ElseIf ILSCategory = 1 Then
			hCons = arHOASPlaneCat1
		Else
			hCons = arHOASPlaneCat23
		End If

		pPolyline = New ESRI.ArcGIS.Geometry.Polyline
		pFrmPoint = New ESRI.ArcGIS.Geometry.Point
		pToPoint = New ESRI.ArcGIS.Geometry.Point

		N = UBound(OASPlanes)

		For I = 0 To N
			OASPlanes(I).Poly = New ESRI.ArcGIS.Geometry.Polygon
		Next I

		For I = eOAS.XlPlane To eOAS.YrPlane
			ResLine = IntersectPlanes(OASPlanes(I).Plane, OASPlanes(I + 1).Plane, 0.0, hCons)

			pFrmPoint.PutCoords(ResLine.FromPoint.X + ptLHPrj.X, ResLine.FromPoint.Y + ptLHPrj.Y)
			pPolyline.FromPoint = pFrmPoint

			pToPoint.PutCoords(ResLine.ToPoint.X + ptLHPrj.X, ResLine.ToPoint.Y + ptLHPrj.Y)
			pPolyline.ToPoint = pToPoint

			pTransform = pPolyline
			pTransform.Rotate(ptLHPrj, DegToRad(ArDir + 180.0))

			OASPlanes(0).Poly.AddPoint(pPolyline.FromPoint)
			OASPlanes(N).Poly.AddPoint(pPolyline.ToPoint)
		Next I

		For I = 0 To 3
			J = 1 + (I + 4) Mod 6
			K = 1 + (I + 5) Mod 6

			ResLine = IntersectPlanes(OASPlanes(J).Plane, OASPlanes(K).Plane, 0.0, hMax)

			pFrmPoint.PutCoords(ResLine.FromPoint.X + ptLHPrj.X, ResLine.FromPoint.Y + ptLHPrj.Y)
			pPolyline.FromPoint = pFrmPoint

			pToPoint.PutCoords(ResLine.ToPoint.X + ptLHPrj.X, ResLine.ToPoint.Y + ptLHPrj.Y)
			pPolyline.ToPoint = pToPoint

			pTransform = pPolyline
			pTransform.Rotate(ptLHPrj, DegToRad(ArDir + 180.0))

			OASPlanes(0).Poly.AddPoint(pPolyline.FromPoint)
			OASPlanes(N).Poly.AddPoint(pPolyline.ToPoint)
		Next I

		M = OASPlanes(0).Poly.PointCount
		J = 6
		For I = 1 To 6
			K = J Mod M
			L = (J + M - 1) Mod M

			OASPlanes(I).Poly.AddPoint(OASPlanes(0).Poly.Point(K))
			OASPlanes(I).Poly.AddPoint(OASPlanes(0).Poly.Point(L))
			OASPlanes(I).Poly.AddPoint(OASPlanes(N).Poly.Point(L))
			OASPlanes(I).Poly.AddPoint(OASPlanes(N).Poly.Point(K))
			OASPlanes(I).Plane.pPt = OASPlanes(0).Poly.Point(L)

			J = J + 1
			If J = 4 Then J = 5
			If J = 8 Then J = 1
		Next I

		For I = 0 To N
			pTopo = OASPlanes(I).Poly
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()
		Next I

		ResLine = Nothing
	End Sub

	Function MaxObstacleHeightInPoly(ByVal ObstSrcList As ObstacleContainer, ByRef OutObstList() As ObstacleMSA, ByVal MOC As Double, ByVal pPoly As ESRI.ArcGIS.Geometry.IPolygon, ByRef Index As Integer) As Double
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim MaxHeight As Double
		Dim pProxiOperator As ESRI.ArcGIS.Geometry.IProximityOperator

		Index = -1
		N = UBound(ObstSrcList.Obstacles)

		If pPoly.IsEmpty() Or (N < 0) Then
			ReDim OutObstList(-1)
			Return 0.0
		End If

		ReDim OutObstList(N)

		pProxiOperator = pPoly

		MaxHeight = 0.0
		J = -1

		For I = 0 To N
			If pProxiOperator.ReturnDistance(ObstSrcList.Obstacles(I).pGeomPrj) = 0 Then
				J = J + 1
				OutObstList(J).pGeomGeo = ObstSrcList.Obstacles(I).pGeomGeo
				OutObstList(J).pGeomPrj = ObstSrcList.Obstacles(I).pGeomPrj

				OutObstList(J).TypeName = ObstSrcList.Obstacles(I).TypeName
				OutObstList(J).UnicalName = ObstSrcList.Obstacles(I).UnicalName

				OutObstList(J).ID = ObstSrcList.Obstacles(I).ID
				OutObstList(J).Identifier = ObstSrcList.Obstacles(I).Identifier

				OutObstList(J).Height = ObstSrcList.Obstacles(I).Height 'pPtGeo.Z - RefHeight
				OutObstList(J).ReqH = OutObstList(J).Height + MOC

				OutObstList(J).Index = ObstSrcList.Obstacles(I).Index
				If OutObstList(J).Height > MaxHeight Then
					Index = J
					MaxHeight = OutObstList(J).Height
				End If
			End If
		Next I

		If J >= 0 Then
			ReDim Preserve OutObstList(J)
		Else
			ReDim OutObstList(-1)
		End If

		Return MaxHeight
	End Function

	Sub CalcArObstaclesMOC2(ByVal ObstSource As ObstacleContainer, ByRef WorkObstList As ObstacleContainer, ByVal pBasePolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByVal pBufferPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef iMax As Integer)   'As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		'Dim pFullProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pBufferProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim hMax As Double
		Dim Dist As Double
		Dim MaxReqH As Double
		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer

		iMax = -1

		M = UBound(ObstSource.Obstacles)

		If (M < 0) Or (pBufferPolygon.PointCount < 1) Then
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
			Return '-1
		End If

		ReDim WorkObstList.Obstacles(M)

		N = Math.Max(UBound(ObstSource.Parts), M)
		C = N
		ReDim WorkObstList.Parts(N)

		pBaseProxi = pBasePolygon
		pBufferProxi = pBufferPolygon
		hMax = -9999.0
		K = -1
		L = -1

		For I = 0 To M
			pCurrGeom = ObstSource.Obstacles(I).pGeomPrj
			Dist = pBufferProxi.ReturnDistance(pCurrGeom)
			If Dist > 0.0 Then Continue For
			L += 1
			WorkObstList.Obstacles(L) = ObstSource.Obstacles(I)

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			ReDim WorkObstList.Obstacles(L).Parts(P)

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve WorkObstList.Parts(C)
				End If

				WorkObstList.Parts(K).pPtPrj = pCurrPt
				WorkObstList.Parts(K).Owner = L
				WorkObstList.Parts(K).Height = WorkObstList.Obstacles(L).Height
				WorkObstList.Parts(K).Index = J
				WorkObstList.Obstacles(L).Parts(J) = K

				Dist = pBaseProxi.ReturnDistance(pCurrPt)
				WorkObstList.Parts(K).Flags = -CInt(Dist = 0.0)
				WorkObstList.Parts(K).fTmp = 1.0

				If WorkObstList.Parts(K).Flags = 0 Then
					WorkObstList.Parts(K).fTmp = (arHoldingBuffer.Value - Dist) / arHoldingBuffer.Value
				End If

				WorkObstList.Parts(K).MOC = WorkObstList.Parts(K).fTmp * arIASegmentMOC.Value
				WorkObstList.Parts(K).ReqH = WorkObstList.Parts(K).Height + WorkObstList.Parts(K).MOC
				If WorkObstList.Parts(K).ReqH > MaxReqH Then MaxReqH = WorkObstList.Parts(K).ReqH

				If WorkObstList.Parts(K).ReqH > hMax Then
					hMax = WorkObstList.Parts(K).ReqH
					iMax = K
				End If
			Next
		Next

		If K >= 0 Then
			ReDim Preserve WorkObstList.Parts(K)
			ReDim Preserve WorkObstList.Obstacles(L)
		Else
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
		End If
		'Return P
	End Sub

	Sub CalcArObstaclesMOC(ByVal ObstSource As ObstacleContainer, ByRef WorkObstList As ObstacleContainer, ByVal pBasePolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByVal pBufferPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef iMax As Integer)   'As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		'Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		'Dim pFullProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pBufferProxi As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim pBufferRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pBaseRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		Dim hMax As Double
		Dim Dist As Double
		Dim MaxReqH As Double
		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer

		iMax = -1

		M = UBound(ObstSource.Obstacles)

		If (M < 0) Or (pBufferPolygon.PointCount < 1) Then
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
			Return '-1
		End If

		ReDim WorkObstList.Obstacles(M)

		N = Math.Max(UBound(ObstSource.Parts), M)
		C = N
		ReDim WorkObstList.Parts(N)

		pBaseProxi = pBasePolygon
		pBufferProxi = pBufferPolygon
		hMax = -9999.0
		K = -1
		L = -1

		pBaseRelation = pBasePolygon
		pBufferRelation = pBufferPolygon


		For I = 0 To M
			pCurrGeom = ObstSource.Obstacles(I).pGeomPrj

			If pBufferRelation.Disjoint(pCurrGeom) Then Continue For

			L += 1
			WorkObstList.Obstacles(L) = ObstSource.Obstacles(I)

			pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				If (pBaseRelation.Disjoint(pCurrGeom) = False) Then
					Dim pTmpCollection As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint
					If (pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon) Then
						pTmpCollection = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry2Dimension)
					ElseIf (pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolyline) Then
						pTmpCollection = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry1Dimension)
					End If

					If (pTmpCollection.PointCount > 0) Then
						pObstPoints.AddPoint(pTmpCollection.Point(0))
					End If
				ElseIf (pBaseRelation.Disjoint(pBufferPolygon) = False) Then
					Dim pTmpCollection As IPointCollection = New ESRI.ArcGIS.Geometry.Multipoint
					If (pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon) Then
						pTmpCollection = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry2Dimension)
					ElseIf (pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolyline) Then
						pTmpCollection = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry1Dimension)
					End If

					Dim minDistance As Double = Double.MaxValue
					Dim minObs As IPoint
					For kk As Integer = 0 To pTmpCollection.PointCount - 1
						pCurrPt = pTmpCollection.Point(kk)
						Dist = pBaseProxi.ReturnDistance(pCurrPt)
						If (Dist < minDistance) Then
							minObs = pCurrPt
							minDistance = Dist
						End If
					Next
					If (minObs IsNot Nothing) Then
						pObstPoints.AddPoint(minObs)
					End If
				End If
			End If

			P = pObstPoints.PointCount - 1
			ReDim WorkObstList.Obstacles(L).Parts(P)

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve WorkObstList.Parts(C)
				End If

				WorkObstList.Parts(K).pPtPrj = pCurrPt
				WorkObstList.Parts(K).Owner = L
				WorkObstList.Parts(K).Height = WorkObstList.Obstacles(L).Height
				WorkObstList.Parts(K).Index = J
				WorkObstList.Obstacles(L).Parts(J) = K

				Dist = pBaseProxi.ReturnDistance(pCurrPt)
				WorkObstList.Parts(K).Flags = -CInt(Dist = 0.0)
				WorkObstList.Parts(K).fTmp = 1.0

				If WorkObstList.Parts(K).Flags = 0 Then
					WorkObstList.Parts(K).fTmp = (arHoldingBuffer.Value - Dist) / arHoldingBuffer.Value
				End If

				WorkObstList.Parts(K).MOC = WorkObstList.Parts(K).fTmp * arIASegmentMOC.Value
				WorkObstList.Parts(K).ReqH = WorkObstList.Parts(K).Height + WorkObstList.Parts(K).MOC
				If WorkObstList.Parts(K).ReqH > MaxReqH Then MaxReqH = WorkObstList.Parts(K).ReqH

				If WorkObstList.Parts(K).ReqH > hMax Then
					hMax = WorkObstList.Parts(K).ReqH
					iMax = K
				End If
			Next
		Next

		If K >= 0 Then
			ReDim Preserve WorkObstList.Parts(K)
			ReDim Preserve WorkObstList.Obstacles(L)
		Else
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
		End If
		'Return P
	End Sub

	Public Function CalcSpiralStartPoint(ByVal LinePoint As ESRI.ArcGIS.Geometry.IPointCollection, ByVal detObs As ObstacleData, ByVal coef As Double, ByVal r0 As Double, ByVal ArDir As Double, ByVal TurnDir As Integer) As Interval
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp2 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCurr As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTurn As ESRI.ArcGIS.Geometry.IPoint
		Dim BasePoints() As ESRI.ArcGIS.Geometry.IPoint
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pConstructor As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pLine As ESRI.ArcGIS.Geometry.ILine

		Dim I As Integer
		Dim N As Integer

		Dim iMin As Integer
		Dim Side As Integer
		Dim Side1 As Integer
		Dim Offset As Integer

		Dim hT As Double
		Dim Dist As Double
		Dim fTmp As Double
		Dim hTMin As Double
		Dim Alpha As Double
		Dim Theta As Double
		Dim dAlpha As Double
		Dim MaxTheta As Double
		Dim ASinAlpha As Double

		Offset = 0

		pLine = New ESRI.ArcGIS.Geometry.Line
		pProxi = pLine

		N = LinePoint.PointCount - Offset

		ReDim BasePoints(N)

		For I = 0 To N - 1
			BasePoints(I) = LinePoint.Point(I + Offset)
			If I = N - 1 Then
				BasePoints(I).M = BasePoints(I - 1).M
			Else
				BasePoints(I).M = ReturnAngleInDegrees(LinePoint.Point(I + Offset), LinePoint.Point(I + Offset + 1))
			End If
		Next

		ptCnt = New ESRI.ArcGIS.Geometry.Point
		pConstructor = ptCnt

		ptTurn = Nothing

		hTMin = MaxModelRadius
		iMin = -1

		'DrawPolyLine LinePoint, 0

		MaxTheta = SpiralTouchAngle(r0, coef, ArDir, ArDir + 90.0 * TurnDir, TurnDir)
		If MaxTheta > 180.0 Then MaxTheta = 360.0 - MaxTheta

		For I = 0 To N - 2
			ptCurr = detObs.pPtPrj

			Side = SideDef(BasePoints(I), BasePoints(I).M, ptCurr)
			Alpha = ArDir + 90.0 * Side
			If System.Math.Abs(System.Math.Sin(DegToRad(Alpha - BasePoints(I).M))) > degEps Then
				dAlpha = SubtractAngles(Alpha, BasePoints(I).M)
				ptTmp = PointAlongPlane(BasePoints(I), ArDir - 90.0 * Side, r0)

				Dist = Point2LineDistancePrj(ptCurr, ptTmp, BasePoints(I).M)
				Side1 = SideDef(ptTmp, BasePoints(I).M, ptCurr)

				Theta = 0.5 * MaxTheta
				Do
					fTmp = Theta
					ASinAlpha = Dist / (r0 + Theta * coef)
					If System.Math.Abs(ASinAlpha) <= 1.0 Then
						Theta = dAlpha - RadToDeg(Side1 * TurnDir * ArcSin(ASinAlpha))
					Else
						Theta = MaxTheta
						Exit Do
					End If
				Loop While System.Math.Abs(fTmp - Theta) > degEps

				fTmp = System.Math.Sin(DegToRad(Theta)) * (r0 + Theta * coef)

				If Theta > MaxTheta Then
					hT = System.Math.Sin(DegToRad(MaxTheta)) * (r0 + MaxTheta * coef)
					fTmp = (hT - fTmp)
					Theta = MaxTheta
				Else
					hT = fTmp
					fTmp = 0.0
				End If

				ptTmp2 = PointAlongPlane(ptCurr, ArDir + 180.0, hT + fTmp)
				pConstructor.ConstructAngleIntersection(ptTmp2, DegToRad(ArDir + 90.0), ptTmp, DegToRad(BasePoints(I).M))

				ptTmp = PointAlongPlane(ptCnt, ArDir - TurnDir * 90.0, r0)

				pLine.FromPoint = BasePoints(I)
				pLine.ToPoint = BasePoints(I + 1)

				fTmp = pProxi.ReturnDistance(ptTmp)

				If fTmp < distEps Then
					If hT < hTMin Then
						hTMin = hT
						iMin = I
						ptTurn = ptTmp
						ptTurn.M = Theta
						ptTurn.Z = detObs.Dist - hTMin
						If (ptTurn.Z < 0.0) Then
							ptTurn.Z = 0.0
						End If
					End If
				End If
			End If
			'Next K
		Next I

		If iMin > -1 Then
			CalcSpiralStartPoint.Left = ptTurn.Z
			CalcSpiralStartPoint.Right = ptTurn.M
		Else
			CalcSpiralStartPoint.Left = -9999.0
			CalcSpiralStartPoint.Right = -9999.0
		End If
	End Function

	Sub RemoveSeamPoints(ByRef pPoints As IPointCollection)
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim pCurrPt As IPoint
		Dim fDist As Double
		N = pPoints.PointCount
		J = 0
		While J < N - 1
			pCurrPt = pPoints.Point(J)
			I = J + 1
			While I < N
				fDist = ReturnDistanceInMeters(pCurrPt, pPoints.Point(I))
				If fDist < distEps Then
					pPoints.RemovePoints(I, 1)
					N -= 1
				Else
					I += 1
				End If
			End While
			J += 1
		End While
	End Sub

	Sub RemoveSeamPoints(ByRef pPoints As IPointCollection, ByVal ptTHR As ESRI.ArcGIS.Geometry.IPoint, ByVal Direction As Double)
		Dim I As Integer
		Dim N As Integer

		Dim X As Double
		Dim Y As Double
		Dim Xmin As Double
		Dim Ymin As Double
		Dim fTmp As Double

		Dim pCurrPt As IPoint
		Dim pMinXpt As IPoint
		Dim pMinYpt As IPoint

		N = pPoints.PointCount

		For I = 0 To N - 1
			pCurrPt = pPoints.Point(I)
			PrjToLocal(ptTHR, Direction, pCurrPt, X, Y)

			If I = 0 Then
				Xmin = Math.Abs(X)
				Ymin = Math.Abs(Y)
				pMinXpt = pCurrPt
				pMinYpt = pCurrPt
			Else
				fTmp = Math.Abs(X)
				If Xmin > fTmp Then
					Xmin = fTmp
					pMinXpt = pCurrPt
				End If

				fTmp = Math.Abs(Y)
				If Ymin > fTmp Then
					Ymin = fTmp
					pMinYpt = pCurrPt
				End If
			End If
		Next

		pPoints.RemovePoints(0, N)
		pPoints.AddPoint(pMinXpt)
		If ReturnDistanceInMeters(pMinXpt, pMinYpt) > EpsilonDistance Then pPoints.AddPoint(pMinYpt)
	End Sub

	Function CreateObstacleParts(ByRef ObstList As ObstacleContainer, ByVal pBaseArea As ESRI.ArcGIS.Geometry.IPolygon, Optional ByVal pBufferArea As ESRI.ArcGIS.Geometry.IPolygon = Nothing) As Integer
		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim P As Integer

		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator

		N = UBound(ObstList.Obstacles)

		If (N < 0) Or (pBufferArea Is Nothing And pBaseArea Is Nothing) Then
			ReDim ObstList.Parts(-1)
			Return -1
		End If

		K = -1
		C = 10 * N
		ReDim ObstList.Parts(C)

		For I = 0 To N
			pCurrGeom = ObstList.Obstacles(I).pGeomPrj
			pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint

			If Not pBufferArea Is Nothing Then
				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
					pRelational = pBufferArea
					If pRelational.Contains(pCurrGeom) Then pObstPoints.AddPoint(pCurrGeom)
				Else
					pTopoOper = pCurrGeom
					pTmpPoints = pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
					pTmpPoints = pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				End If
			End If

			If Not pBaseArea Is Nothing Then
				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
					pRelational = pBaseArea
					If pRelational.Contains(pCurrGeom) Then pObstPoints.AddPoint(pCurrGeom)
				Else
					pTopoOper = pCurrGeom
					pTmpPoints = pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					pTmpPoints = pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				End If
			End If

			RemoveSeamPoints(pObstPoints)

			P = pObstPoints.PointCount - 1

			ReDim ObstList.Obstacles(I).Parts(P)

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve ObstList.Parts(C)
				End If

				ObstList.Parts(K).pPtPrj = pCurrPt
				ObstList.Parts(K).Owner = I
				ObstList.Parts(K).Height = ObstList.Obstacles(I).Height
				ObstList.Parts(K).Index = J
				ObstList.Obstacles(I).Parts(J) = K
			Next
		Next

		If K >= 0 Then
			ReDim Preserve ObstList.Parts(K)
		Else
			ReDim ObstList.Parts(-1)
		End If

		Return K
	End Function

	Function CalcArObstaclesNavMOC(ByVal ObstSrcList As ObstacleContainer, ByRef WorkObstList As ObstacleContainer, ByVal ptNAV As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTHR As ESRI.ArcGIS.Geometry.IPoint, ByVal Direction As Double, ByVal pBasePolygon As ESRI.ArcGIS.Geometry.IPolygon, ByVal pBufferPolygon As ESRI.ArcGIS.Geometry.IPolygon, ByVal Var As Double, ByRef iMax As Integer) As Integer
		Dim pBufferRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pBaseRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint

		Dim p2DistPoly As ESRI.ArcGIS.Geometry.IPolygon

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pLine1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline

		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Dim MOCValue As Double
		Dim fDist As Double
		Dim hMax As Double

		Dim Side0 As Integer
		Dim Side1 As Integer

		Dim I As Integer
		Dim J As Integer
		Dim C As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer
		Dim M As Integer
		Dim P As Integer

		M = UBound(ObstSrcList.Obstacles)
		pBufferPolygon.SpatialReference = pSpRefPrj
		pBasePolygon.SpatialReference = pSpRefPrj
		iMax = -1

		If (M < 0) Or (pBufferPolygon.IsEmpty) Then
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
			Return -1
		End If

		'DrawPolygon(pBufferPolygon, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
		'DrawPolygon(pBasePolygon, RGB(0, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
		'Application.DoEvents()

		Select Case Math.Round(Var)
			Case 0
				MOCValue = arFASeg_FAF_MOC.Value
			Case 1
				MOCValue = arFASegmentMOC.Value
			Case 2
				MOCValue = arMA_InterMOC.Value
			Case 3
				MOCValue = arMA_FinalMOC.Value
			Case 4
				MOCValue = arFASeg_FAF_MOC.Value
			Case Else
				MOCValue = Var
		End Select

		ReDim WorkObstList.Obstacles(M)

		N = Math.Max(UBound(ObstSrcList.Parts), M)
		C = N
		ReDim WorkObstList.Parts(C)

		pTopoOper = pBufferPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		p2DistPoly = pTopoOper.Difference(pBasePolygon)
		pTopoOper = p2DistPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pBaseRelation = pBasePolygon
		pBufferRelation = pBufferPolygon

		K = -1
		L = -1
		hMax = -9999.0

		pLine1 = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To M
			pCurrGeom = ObstSrcList.Obstacles(I).pGeomPrj
			If pBufferRelation.Disjoint(pCurrGeom) Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				'If ObstSrcList.Obstacles(I).UnicalName = "TG0133OB" Then
				'	P = 0
				'End If

				pObstPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pBufferPolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pBasePolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints, ptTHR, Direction)
			End If

			P = pObstPoints.PointCount - 1
			If P < 0 Then Continue For

			L += 1
			WorkObstList.Obstacles(L) = ObstSrcList.Obstacles(I)
			WorkObstList.Obstacles(L).PartsNum = P + 1
			ReDim WorkObstList.Obstacles(L).Parts(P)

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve WorkObstList.Parts(C)
				End If

				WorkObstList.Parts(K).pPtPrj = pCurrPt
				WorkObstList.Parts(K).Owner = L
				WorkObstList.Parts(K).Height = WorkObstList.Obstacles(L).Height
				WorkObstList.Parts(K).Index = J
				WorkObstList.Obstacles(L).Parts(J) = K

				If Var > 1 Then
					WorkObstList.Parts(K).Dist = Point2LineDistancePrj(pCurrPt, ptNAV, Direction - 90.0)
				End If

				If Not pBaseRelation.Disjoint(pCurrPt) Then
					WorkObstList.Parts(K).Flags = 1
					WorkObstList.Parts(K).MOC = MOCValue
					WorkObstList.Parts(K).fTmp = 1.0
				Else
					WorkObstList.Parts(K).Flags = 0

					Side0 = SideDef(ptNAV, Direction, pCurrPt)
					fDist = Point2LineDistancePrj(pCurrPt, ptNAV, Direction)

					pLine1.FromPoint = PointAlongPlane(pCurrPt, Direction + 90.0 * Side0, fDist)
					pLine1.ToPoint = PointAlongPlane(pCurrPt, Direction - 90.0 * Side0, 1000000.0)

					'DrawPolyLine(pLine1, 255, 2)
					'DrawPointWithText(pCurrPt, "31")
					'DrawPolygon(pBufferPolygon, RGB(255, 255, 0), ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
					'While True
					'	Application.DoEvents()
					'End While

					pTopoOper = pBufferPolygon

					pLine = pTopoOper.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
					Side1 = SideDef(pLine.FromPoint, Direction, pLine.ToPoint)
					If Side0 * Side1 < 0 Then pLine.ReverseOrientation()

					'DrawPolyLine(pLine, 0, 2)
					'Application.DoEvents()

					pTopoOper = pBasePolygon
					pLine1 = pTopoOper.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
					Side1 = SideDef(pLine1.FromPoint, Direction, pLine1.ToPoint)
					If Side0 * Side1 < 0 Then pLine1.ReverseOrientation()

					pLine.FromPoint = pLine1.ToPoint
					WorkObstList.Parts(K).fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length
					WorkObstList.Parts(K).MOC = MOCValue * WorkObstList.Parts(K).fTmp
				End If

				WorkObstList.Parts(K).ReqH = WorkObstList.Parts(K).Height + WorkObstList.Parts(K).MOC
				If WorkObstList.Parts(K).ReqH > hMax Then
					hMax = WorkObstList.Parts(K).ReqH
					iMax = K
				End If
			Next
		Next

		If K >= 0 Then
			ReDim Preserve WorkObstList.Parts(K)
			ReDim Preserve WorkObstList.Obstacles(L)
		Else
			ReDim WorkObstList.Parts(-1)
			ReDim WorkObstList.Obstacles(-1)
		End If

		pLine1 = Nothing
		pLine = Nothing
		Return K
	End Function

	Sub GetFinalObstacles(ByVal ObstacleList As ObstacleContainer, ByRef ObstacleFinalMOCList As ObstacleContainer, ByVal BaseArea As ESRI.ArcGIS.Geometry.IPointCollection, ByVal SecPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByVal SecPoly0 As ESRI.ArcGIS.Geometry.IPointCollection, ByVal DistPoly As ESRI.ArcGIS.Geometry.IGeometry, ByVal TurnAngle As Double, ByVal ArDir As Double, ByVal OutDir As Double, ByVal ptNavPrj As ESRI.ArcGIS.Geometry.Point, ByRef FixPntPrj As ESRI.ArcGIS.Geometry.Point, ByRef iMax As Integer) 'As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pLine1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pSecondProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pSecond0Proxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pDistProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		'Dim pSecondReleation As ESRI.ArcGIS.Geometry.IRelationalOperator

		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Dim D As Double
		Dim dcX As Double
		Dim dcY As Double
		Dim hMax As Double
		Dim MOCValue As Double

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer
		Dim Side0 As Integer
		Dim Side1 As Integer

		M = UBound(ObstacleList.Obstacles)
		iMax = -1

		If (M < 0) Or (BaseArea.PointCount < 1) Then
			ReDim ObstacleFinalMOCList.Parts(-1)
			ReDim ObstacleFinalMOCList.Obstacles(-1)
			Return '-1
		End If

		ReDim ObstacleFinalMOCList.Obstacles(M)
		N = Math.Max(UBound(ObstacleList.Parts), M)

		C = N
		ReDim ObstacleFinalMOCList.Parts(C)

		pBaseProxi = BaseArea
		pSecondProxi = SecPoly
		'pSecondReleation = SecPoly
		pSecond0Proxi = SecPoly0
		pDistProxi = DistPoly

		K = -1
		L = -1
		hMax = -9999.0

		If TurnAngle > arMATurnTrshAngl.Value Then
			MOCValue = arMA_FinalMOC.Value
		Else
			MOCValue = arMA_InterMOC.Value
		End If

		pLine1 = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			D = pBaseProxi.ReturnDistance(pCurrGeom)

			If D <> 0.0 Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				pTmpPoints = pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			If P < 0 Then Continue For

			L += 1
			ObstacleFinalMOCList.Obstacles(L) = ObstacleList.Obstacles(I)
			ObstacleFinalMOCList.Obstacles(L).PartsNum = P + 1
			ReDim ObstacleFinalMOCList.Obstacles(L).Parts(P)

			For J = 0 To P
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve ObstacleFinalMOCList.Parts(C)
				End If

				pCurrPt = pObstPoints.Point(J)

				ObstacleFinalMOCList.Parts(K).pPtPrj = pCurrPt
				ObstacleFinalMOCList.Parts(K).Owner = L
				ObstacleFinalMOCList.Parts(K).Height = ObstacleFinalMOCList.Obstacles(L).Height
				ObstacleFinalMOCList.Parts(K).Index = J
				ObstacleFinalMOCList.Obstacles(L).Parts(J) = K

				ObstacleFinalMOCList.Parts(K).Flags = 0
				ObstacleFinalMOCList.Parts(K).fTmp = 1.0

				pGeometry = SecPoly0
				If Not pGeometry.IsEmpty() Then
					D = pSecond0Proxi.ReturnDistance(pCurrPt)
				Else
					D = 10.0
				End If

				'pSecondReleation = SecPoly0
				If D = 0.0 Then 'And Not pSecondReleation.Disjoint(pCurrPt) Then
					ObstacleFinalMOCList.Parts(K).Flags = 2

					pTopoOper = SecPoly0

					PrjToLocal(ptNavPrj, ArDir, pCurrPt, dcX, dcY)
					Side0 = Math.Sign(dcY)

					pLine1.FromPoint = LocalToPrj(ptNavPrj, ArDir, dcX)
					pLine1.ToPoint = LocalToPrj(ptNavPrj, ArDir, dcX, 10.0 * Side0 * MaxModelRadius)

					'Side0 = SideDef(ptNavPrj, ArDir, pCurrPt)
					'dcX = Point2LineDistancePrj(pCurrPt, ptNavPrj, ArDir)

					'pLine1.FromPoint = PointAlongPlane(pCurrPt, ArDir + 90.0 * Side0, dcX)
					'pLine1.ToPoint = PointAlongPlane(pCurrPt, ArDir - 90.0 * Side0, 10.0 * MaxModelRadius)

					'DrawPolygon(SecPoly0, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
					'DrawPolyLine(pLine1, -1, 2)
					'DrawPointWithText(pCurrPt, "obst")
					'While True
					'	Application.DoEvents()
					'End While

					pLine = pTopoOper.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
					If pLine.IsEmpty Then
						ObstacleFinalMOCList.Parts(K).fTmp = 1.0
						ObstacleFinalMOCList.Parts(K).Flags = 0
					Else
						Side1 = SideDef(pLine.FromPoint, ArDir, pLine.ToPoint)
						If Side0 = Side1 Then pLine.ReverseOrientation()

						ObstacleFinalMOCList.Parts(K).fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length
						If ObstacleFinalMOCList.Parts(K).fTmp > 1.0 Then
							ObstacleFinalMOCList.Parts(K).fTmp = 1.0
							ObstacleFinalMOCList.Parts(K).Flags = 0
						End If
					End If
				Else
					pGeometry = SecPoly
					'pSecondReleation = SecPoly

					If pGeometry.IsEmpty() Then 'Or Not pSecondReleation.Contains(pCurrPt) Then
						D = 10.0
					Else
						D = pSecondProxi.ReturnDistance(pCurrPt)
					End If

					If D = 0.0 Then
						ObstacleFinalMOCList.Parts(K).Flags = 1
						pTopoOper = SecPoly

						PrjToLocal(FixPntPrj, OutDir, pCurrPt, dcX, dcY)
						Side0 = Math.Sign(dcY)

						pLine1.FromPoint = LocalToPrj(FixPntPrj, OutDir, dcX)
						pLine1.ToPoint = LocalToPrj(FixPntPrj, OutDir, dcX, 10.0 * Side0 * MaxModelRadius)

						'DrawPointWithText(pCurrPt, ObstacleFinalMOCList.Obstacles(L).UnicalName)
						'DrawPolygon(SecPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
						'DrawPolyLine(pLine1, -1, 2)

						'While True
						'	Application.DoEvents()
						'End While
						Try
							pLine = pTopoOper.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

							Side1 = SideDef(pLine.FromPoint, OutDir, pLine.ToPoint)

							If Side0 = Side1 Then pLine.ReverseOrientation()

							ObstacleFinalMOCList.Parts(K).fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length
							If ObstacleFinalMOCList.Parts(K).fTmp > 1.0 Then ObstacleFinalMOCList.Parts(K).fTmp = 1.0
						Catch
						End Try
					End If
				End If

				ObstacleFinalMOCList.Parts(K).MOC = MOCValue * ObstacleFinalMOCList.Parts(K).fTmp
				ObstacleFinalMOCList.Parts(K).ReqH = ObstacleFinalMOCList.Parts(K).Height + ObstacleFinalMOCList.Parts(K).MOC
				ObstacleFinalMOCList.Parts(K).Dist = pDistProxi.ReturnDistance(pCurrPt)
				'AvDist += ObstacleFinalMOCList.Parts(K).Dist

				If ObstacleFinalMOCList.Parts(K).ReqH > hMax Then
					hMax = ObstacleFinalMOCList.Parts(K).ReqH
					iMax = K
				End If
			Next

		Next

		If K >= 0 Then
			ReDim Preserve ObstacleFinalMOCList.Obstacles(L)
			ReDim Preserve ObstacleFinalMOCList.Parts(K)
		Else
			ReDim ObstacleFinalMOCList.Obstacles(-1)
			ReDim ObstacleFinalMOCList.Parts(-1)
		End If

		Return 'iMax
	End Sub

	Function GetIntermedObstacles(ByVal ObstacleList As ObstacleContainer, ByRef ObstacleImList As ObstacleContainer, ByVal FullArea As ESRI.ArcGIS.Geometry.IPointCollection, ByVal SecPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByVal HalfFAFWidth As Double, ByVal minIntLen As Double, ByVal ArDir As Double, ByVal hFAF As Double, ByVal NavType As Integer, ByVal ptFAFprj As ESRI.ArcGIS.Geometry.Point, Optional ByVal bKeepAll As Boolean = False) As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFullProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim D As Double
		Dim fTmp As Double
		Dim AvDist As Double
		Dim MOCValue As Double

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer

		M = UBound(ObstacleList.Obstacles)

		If (M < 0) Or (FullArea.PointCount < 1) Then
			ReDim ObstacleImList.Parts(-1)
			ReDim ObstacleImList.Obstacles(-1)
			Return -1
		End If

		ReDim ObstacleImList.Obstacles(M)
		N = Math.Max(UBound(ObstacleList.Parts), M)
		C = 10 * N
		ReDim ObstacleImList.Parts(C)

		pFullProxi = FullArea

		'pSecondProxi = SecPoly
		'pLine = New ESRI.ArcGIS.Geometry.Polyline

		MOCValue = arISegmentMOC.Value
		L = -1
		K = -1

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			D = pFullProxi.ReturnDistance(pCurrGeom)

			If D <> 0.0 Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				pTmpPoints = pTopoOper.Intersect(FullArea, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(FullArea, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(FullArea, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			If P < 0 Then Continue For

			L += 1
			ObstacleImList.Obstacles(L) = ObstacleList.Obstacles(I)
			ObstacleImList.Obstacles(L).PartsNum = P + 1
			ReDim ObstacleImList.Obstacles(L).Parts(P)

			For J = 0 To P
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve ObstacleImList.Parts(C)
				End If

				pCurrPt = pObstPoints.Point(J)

				ObstacleImList.Parts(K).pPtPrj = pCurrPt
				ObstacleImList.Parts(K).Owner = L
				ObstacleImList.Parts(K).Height = ObstacleImList.Obstacles(L).Height
				ObstacleImList.Parts(K).Index = J
				ObstacleImList.Obstacles(L).Parts(J) = K


				ObstacleImList.Parts(K).fTmp = 1.0
				ObstacleImList.Parts(K).Dist = Point2LineDistancePrj(pCurrPt, ptFAFprj, ArDir + 90.0)
				ObstacleImList.Parts(K).DistStar = Point2LineDistancePrj(pCurrPt, ptFAFprj, ArDir)
				AvDist += ObstacleImList.Parts(K).Dist

				If minIntLen > ObstacleImList.Parts(K).Dist Then
					If NavType = eNavaidType.LLZ Then
						fTmp = 2.0 * (arIFHalfWidth.Value - HalfFAFWidth - minIntLen * (ObstacleImList.Parts(K).DistStar - HalfFAFWidth) / ObstacleImList.Parts(K).Dist) / arIFHalfWidth.Value
					Else
						fTmp = 2.0 * (1.0 - ObstacleImList.Parts(K).DistStar / (HalfFAFWidth + ObstacleImList.Parts(K).Dist / minIntLen * (arIFHalfWidth.Value - HalfFAFWidth)))
					End If
				Else
					fTmp = 2.0 * (1.0 - ObstacleImList.Parts(K).DistStar / arIFHalfWidth.Value)
				End If

				If fTmp <= 0 Then Exit For

				If fTmp >= 1.0 Then
					ObstacleImList.Parts(K).MOC = MOCValue
					ObstacleImList.Parts(K).Flags = 1
					ObstacleImList.Parts(K).fTmp = 1.0
				Else 'If fTmp > 0.0 Then
					ObstacleImList.Parts(K).MOC = fTmp * MOCValue
					ObstacleImList.Parts(K).Flags = 0
					ObstacleImList.Parts(K).fTmp = fTmp
				End If

				ObstacleImList.Parts(K).ReqH = ObstacleImList.Parts(K).Height + ObstacleImList.Parts(K).MOC

				If (ObstacleImList.Parts(K).ReqH > hFAF) Or bKeepAll Then
				Else
					K -= 1
				End If
			Next
		Next I

		'DrawPolygon(FullArea, RGB(0, 255, 127), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)
		'Application.DoEvents()

		If K >= 0 Then
			ReDim Preserve ObstacleImList.Obstacles(L)
			ReDim Preserve ObstacleImList.Parts(K)
		Else
			ReDim ObstacleImList.Obstacles(-1)
			ReDim ObstacleImList.Parts(-1)
		End If

		Return K
	End Function

	Public Function GetIntermObstacleList(ByVal ObstacleList As ObstacleContainer, ByRef OutObstList As ObstacleContainer, ByVal pPlane As ESRI.ArcGIS.Geometry.IPolygon, ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double) As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer

		M = UBound(ObstacleList.Obstacles)

		If (M < 0) Or pPlane.IsEmpty() Then
			ReDim OutObstList.Obstacles(-1)
			ReDim OutObstList.Parts(-1)
			Return 0
		End If

		ReDim OutObstList.Obstacles(M)

		N = Math.Max(UBound(ObstacleList.Parts), M)
		C = N
		ReDim OutObstList.Parts(C)

		pRelation = pPlane
		L = -1
		K = -1

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			If pRelation.Disjoint(pCurrGeom) Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(pPlane, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(pPlane, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pPlane, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			If P < 0 Then Continue For

			L = L + 1
			OutObstList.Obstacles(L) = ObstacleList.Obstacles(I)
			OutObstList.Obstacles(L).PartsNum = P + 1
			ReDim OutObstList.Obstacles(L).Parts(P)

			For J = 0 To P
				K += 1
				If K >= C Then
					C += N
					ReDim Preserve OutObstList.Parts(C)
				End If

				pCurrPt = pObstPoints.Point(J)

				OutObstList.Parts(K).pPtPrj = pCurrPt
				OutObstList.Parts(K).Owner = L
				OutObstList.Parts(K).Index = J
				OutObstList.Parts(K).Flags = 0
				OutObstList.Obstacles(L).Parts(J) = K

				'pZv = ObstacleList.Obstacles(I).pGeomPrj
				'OutObstList.Obstacles(L).Height = pZv.ZMax - ptLHPrj.Z

				OutObstList.Parts(K).Height = OutObstList.Obstacles(L).Height
				'OutObstList.Parts(K).Height = OutObstList.Parts(K).pPtPrj.Z - ptLHPrj.Z

				OutObstList.Parts(K).Dist = Point2LineDistancePrj(OutObstList.Parts(K).pPtPrj, ptLHPrj, ArDir - 90.0)
				OutObstList.Parts(K).DistStar = Point2LineDistancePrj(OutObstList.Parts(K).pPtPrj, ptLHPrj, ArDir)
				OutObstList.Parts(K).ReqH = OutObstList.Parts(J).Height + arMA_FinalMOC.Value
			Next J
		Next I

		If K >= 0 Then
			ReDim Preserve OutObstList.Obstacles(L)
			ReDim Preserve OutObstList.Parts(K)
		Else
			ReDim OutObstList.Obstacles(-1)
			ReDim OutObstList.Parts(-1)
		End If

		Return K - 1
	End Function

	Function GetMAPtObstacles(ByVal ObstacleList As ObstacleContainer, ByRef WorkObstacleList As ObstacleContainer, ByVal PtFAF As ESRI.ArcGIS.Geometry.IPoint, ByVal pFarPoint As IPoint, ByVal PtNAV As ESRI.ArcGIS.Geometry.IPoint, ByVal NavType As Integer, ByVal Dir_Renamed As Double, ByVal pFullPolygon As ESRI.ArcGIS.Geometry.IPointCollection) As Integer
		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		'Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer

		Dim dCL As Double
		Dim dNav As Double
		Dim Dist As Double
		Dim Toler As Double
		Dim NavArea As Double
		Dim ignorDist As Double
		Dim InitWidth As Double

		M = UBound(ObstacleList.Obstacles)

		If (M < 0) Or (pFullPolygon.PointCount < 1) Then
			ReDim WorkObstacleList.Parts(-1)
			ReDim WorkObstacleList.Obstacles(-1)
			Return -1
		End If

		If (NavType = eNavaidType.VOR) Or (NavType = eNavaidType.TACAN) Then
			InitWidth = 0.5 * VOR.InitWidth
			Toler = VOR.SplayAngle
		ElseIf NavType = eNavaidType.NDB Then
			InitWidth = 0.5 * NDB.InitWidth
			Toler = NDB.SplayAngle
		ElseIf NavType = eNavaidType.LLZ Then
			InitWidth = -1.0
			Toler = -1.0
		Else
			ReDim WorkObstacleList.Parts(-1)
			ReDim WorkObstacleList.Obstacles(-1)
			Return -1
		End If

		ReDim WorkObstacleList.Obstacles(M)

		N = Math.Max(UBound(ObstacleList.Parts), M)
		C = N
		ReDim WorkObstacleList.Parts(C)

		pBaseProxi = pFullPolygon
		ignorDist = PtFAF.Z / arFixMaxIgnorGrd.Value

		K = -1
		L = -1

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			If pBaseProxi.ReturnDistance(pCurrGeom) <> 0.0 Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(pFullPolygon, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(pFullPolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pFullPolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			If P < 0 Then Continue For

			L += 1
			WorkObstacleList.Obstacles(L) = ObstacleList.Obstacles(I)
			ReDim WorkObstacleList.Obstacles(L).Parts(P)
			WorkObstacleList.Obstacles(L).PartsNum = P + 1

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				Dist = Point2LineDistancePrj(pCurrPt, pFarPoint, Dir_Renamed - 90.0)
				If (WorkObstacleList.Obstacles(L).Height <= arFixMaxIgnorGrd.Value * (ignorDist - Dist)) Then Continue For

				K += 1
				If K >= C Then
					C += N
					ReDim Preserve WorkObstacleList.Parts(C)
				End If

				WorkObstacleList.Parts(K).pPtPrj = pCurrPt
				WorkObstacleList.Parts(K).Owner = L
				WorkObstacleList.Parts(K).Height = WorkObstacleList.Obstacles(L).Height
				WorkObstacleList.Parts(K).Index = J
				WorkObstacleList.Obstacles(L).Parts(J) = K

				WorkObstacleList.Parts(K).Dist = Point2LineDistancePrj(pCurrPt, PtFAF, Dir_Renamed - 90.0)

				dNav = Point2LineDistancePrj(pCurrPt, PtNAV, Dir_Renamed - 90.0)
				dCL = Point2LineDistancePrj(pCurrPt, PtNAV, Dir_Renamed)
				NavArea = InitWidth + System.Math.Tan(DegToRad(Toler)) * dNav

				WorkObstacleList.Parts(K).fTmp = 2.0 * (NavArea - dCL) / NavArea
				If WorkObstacleList.Parts(K).fTmp >= 1.0 Then
					WorkObstacleList.Parts(K).Flags = 1
					WorkObstacleList.Parts(K).fTmp = 1.0
				Else
					WorkObstacleList.Parts(K).Flags = 0
				End If

				'Else
			Next
		Next

		If K >= 0 Then
			ReDim Preserve WorkObstacleList.Obstacles(L)
			ReDim Preserve WorkObstacleList.Parts(K)
		Else
			ReDim WorkObstacleList.Obstacles(-1)
			ReDim WorkObstacleList.Parts(-1)
		End If

		Return K
	End Function

	Public Function AnaliseObstacles(ByVal ObstacleList As ObstacleContainer, ByRef OutObstList As ObstacleContainer, ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double, ByVal Planes() As D3DPolygone) As Integer
		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim O As Integer
		Dim P As Integer
		Dim Result As Integer

		Dim X As Double
		Dim Y As Double
		Dim Z As Double

		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		'Dim pClone As ArcGIS.esriSystem.IClone

		Dim pZv As ESRI.ArcGIS.Geometry.IZ
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRelationFull As ESRI.ArcGIS.Geometry.IRelationalOperator

		M = UBound(ObstacleList.Obstacles)

		If M < 0 Then
			ReDim OutObstList.Obstacles(-1)
			ReDim OutObstList.Parts(-1)
			Return 0
		End If

		P = UBound(Planes)
		pRelationFull = Planes(P).Poly

		ReDim OutObstList.Obstacles(M)

		C = 10 * M
		ReDim OutObstList.Parts(C)

		K = -1
		L = -1
		Result = 0

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			'pTopoOper = pCurrGeom
			'If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
			'	pTopoOper.IsKnownSimple_2 = False
			'	pTopoOper.Simplify()
			'End If

			If pCurrGeom.IsEmpty Or pRelationFull.Disjoint(pCurrGeom) Then
				Continue For
			End If

			pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint()

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints.AddPoint(pCurrGeom)
			Else

				For O = 0 To P - 1
					pRelation = Planes(O).Poly
					If pRelation.Disjoint(pCurrGeom) Then Continue For

					'DrawPolygon(Planes(O).Poly, 0, ArcGIS.Display.esriSimpleFillStyle.esriSFSBackwardDiagonal)
					'DrawPolygon(pCurrGeom, 255, ArcGIS.Display.esriSimpleFillStyle.esriSFSForwardDiagonal)
					'Application.DoEvents()
					pTopoOper = pCurrGeom
					'pTopoOper.IsKnownSimple_2 = False
					'pTopoOper.Simplify()

					pTmpPoints = pTopoOper.Intersect(Planes(O).Poly, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					pTmpPoints = pTopoOper.Intersect(Planes(O).Poly, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(Planes(O).Poly, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				Next O

				RemoveSeamPoints(pObstPoints)
			End If

			N = pObstPoints.PointCount - 1
			If N < 0 Then Continue For

			L += 1
			OutObstList.Obstacles(L) = ObstacleList.Obstacles(I)
			OutObstList.Obstacles(L).PartsNum = N + 1
			ReDim OutObstList.Obstacles(L).Parts(N)

			If ObstacleList.Obstacles(I).pGeomPrj.GeometryType = esriGeometryType.esriGeometryPoint Then
				OutObstList.Obstacles(L).Height = CType(ObstacleList.Obstacles(I).pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z - ptLHPrj.Z
			Else
				pZv = ObstacleList.Obstacles(I).pGeomPrj
				OutObstList.Obstacles(L).Height = pZv.ZMax - ptLHPrj.Z
			End If

			For J = 0 To N
				'pClone = pObstPoints.Point(J)
				pCurrPt = pObstPoints.Point(J) 'pClone.Clone

				K += 1
				If K >= C Then
					C += M
					ReDim Preserve OutObstList.Parts(C)
				End If

				OutObstList.Parts(K).pPtPrj = pCurrPt
				OutObstList.Parts(K).Owner = L
				OutObstList.Parts(K).Height = OutObstList.Obstacles(L).Height
				OutObstList.Parts(K).Index = J
				OutObstList.Obstacles(L).Parts(J) = K

				PrjToLocal(ptLHPrj, ArDir + 180.0, OutObstList.Parts(K).pPtPrj, X, Y)

				'X = Point2LineDistancePrj(OutObstList.Parts(K).pPtPrj, ptLHPrj, ArDir - 90.0) * SideDef(ptLHPrj, ArDir - 90.0, OutObstList.Parts(K).pPtPrj)
				'Y = Point2LineDistancePrj(OutObstList.Parts(K).pPtPrj, ptLHPrj, ArDir) * SideDef(ptLHPrj, ArDir, OutObstList.Parts(K).pPtPrj)
				OutObstList.Parts(K).Dist = X
				OutObstList.Parts(K).DistStar = Y

				For O = 0 To P - 1
					pRelation = Planes(O).Poly
					If pRelation.Disjoint(OutObstList.Parts(K).pPtPrj) Then Continue For

					Z = Planes(O).Plane.A * X + Planes(O).Plane.B * Y + Planes(O).Plane.D

					'OutObstList.Parts(K).AreaIndex = O
					OutObstList.Parts(K).fTmp = Z
					OutObstList.Parts(K).hPenet = OutObstList.Parts(K).Height - Z

					OutObstList.Parts(K).Plane = O
					OutObstList.Parts(K).minZPlane = Z
					OutObstList.Parts(K).Flags = 0

					If OutObstList.Parts(K).hPenet > 0.0 Then Result += 1
					Exit For
				Next O
			Next J
		Next I

		If K >= 0 Then
			ReDim Preserve OutObstList.Obstacles(L)
			ReDim Preserve OutObstList.Parts(K)
		Else
			ReDim OutObstList.Obstacles(-1)
			ReDim OutObstList.Parts(-1)
		End If

		Return Result
	End Function

	Public Function AnaliseCat23Obstacles(ByRef ObstList As ObstacleContainer, ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal ArDir As Double, ByVal Planes() As D3DPolygone) As Integer
		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		'Dim O As Integer
		Dim P As Integer
		Dim P1 As Integer
		Dim Plans As Integer
		Dim Result As Integer

		Dim X As Double
		Dim Y As Double
		Dim Z As Double

		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pRelationFull As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		M = UBound(ObstList.Obstacles)
		If M < 0 Then Return 0

		K = UBound(ObstList.Parts)
		C = Math.Max(K, M)
		N = C

		If K >= 0 Then
			ReDim Preserve ObstList.Parts(C)
		Else
			ReDim ObstList.Parts(C)
		End If

		Plans = UBound(Planes)
		pRelationFull = Planes(Plans).Poly
		Result = 0

		For I = 0 To M
			pCurrGeom = ObstList.Obstacles(I).pGeomPrj

			If pRelationFull.Disjoint(pCurrGeom) Then Continue For
			pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint()

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom
				For J = 0 To Plans - 1
					pRelation = Planes(J).Poly
					If pRelation.Disjoint(pCurrGeom) Then Continue For

					pTmpPoints = pTopoOper.Intersect(Planes(J).Poly, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					pTmpPoints = pTopoOper.Intersect(Planes(J).Poly, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(Planes(J).Poly, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				Next J

				RemoveSeamPoints(pObstPoints)
			End If

			Dim pCurrPt As IPoint
			Dim fDist As Double

			P1 = ObstList.Obstacles(I).PartsNum - 1
			P = pObstPoints.PointCount

			J = 0
			While J < P
				For L = 0 To P1
					pCurrPt = ObstList.Parts(ObstList.Obstacles(I).Parts(L)).pPtPrj
					fDist = ReturnDistanceInMeters(pCurrPt, pObstPoints.Point(J))
					If fDist < distEps Then
						pObstPoints.RemovePoints(J, 1)
						P -= 1
					Else
						J += 1
					End If
					If J >= P Then Exit For
				Next
				'P = pObstPoints.PointCount
				If P <= 0 Then Exit While

				ReDim Preserve ObstList.Obstacles(I).Parts(P1 + P)

				For J = 1 To P
					pCurrPt = pObstPoints.Point(J - 1)
					K += 1
					If K >= C Then
						C += N
						ReDim Preserve ObstList.Parts(C)
					End If

					ObstList.Parts(K).pPtPrj = pCurrPt
					ObstList.Parts(K).Owner = I
					ObstList.Parts(K).Height = ObstList.Obstacles(I).Height
					ObstList.Parts(K).Index = ObstList.Obstacles(I).PartsNum
					ObstList.Obstacles(I).Parts(ObstList.Parts(K).Index) = K
					ObstList.Obstacles(I).PartsNum += 1

					X = Point2LineDistancePrj(pCurrPt, ptLHPrj, ArDir - 90.0) * SideDef(ptLHPrj, ArDir - 90.0, pCurrPt)
					Y = Point2LineDistancePrj(pCurrPt, ptLHPrj, ArDir) * SideDef(ptLHPrj, ArDir, pCurrPt)

					ObstList.Parts(K).Dist = X
					ObstList.Parts(K).DistStar = Y
				Next

				Exit While
			End While
		Next I
		ReDim Preserve ObstList.Parts(K)

		For I = 0 To K
			For P = 0 To Plans - 1
				pRelation = Planes(P).Poly

				If pRelation.Disjoint(ObstList.Parts(I).pPtPrj) Then Continue For
				Z = Planes(P).Plane.A * ObstList.Parts(I).Dist + Planes(P).Plane.B * ObstList.Parts(I).DistStar + Planes(P).Plane.D

				'ObstList.Parts(I).AreaIndex = P Or 32
				ObstList.Parts(I).fTmp = Z
				ObstList.Parts(I).hPenet = ObstList.Parts(I).Height - Z

				ObstList.Parts(I).Plane = P Or 32
				ObstList.Parts(I).minZPlane = Z

				If ObstList.Parts(I).hPenet > 0.0 Then Result += 1
				Exit For
			Next P
		Next I

		Return Result
	End Function

	Public Function AnaliseTerminalObstacles(ByRef ObstacleList As ObstacleContainer, ByRef pPolygon As ESRI.ArcGIS.Geometry.IPolygon) As Double
		Dim Zgeom As Double
		Dim MaxH As Double
		Dim I As Integer
		Dim N As Integer
		Dim pIZ As ESRI.ArcGIS.Geometry.IZ
		Dim pRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		N = UBound(ObstacleList.Obstacles)

		If (N < 0) Or pPolygon.IsEmpty() Then Return 0.0

		MaxH = 0.0
		pRelation = pPolygon

		For I = 0 To N
			If Not pRelation.Disjoint(ObstacleList.Obstacles(I).pGeomPrj) Then
				If ObstacleList.Obstacles(I).pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
					Zgeom = CType(ObstacleList.Obstacles(I).pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z
				Else
					pIZ = ObstacleList.Obstacles(I).pGeomPrj
					Zgeom = pIZ.ZMax
				End If
				If MaxH < Zgeom Then MaxH = Zgeom
			End If
		Next
		Return MaxH
	End Function

	Public Function CalcHTMA(ByRef ObstacleList As ObstacleContainer, TurnFixPnt As ESRI.ArcGIS.Geometry.IPoint, ByVal fEnrouteMOC As Double, ByVal fRefHeight As Double, ByVal fMissAprPDG As Double) As Double
		Dim MaxDh As Double
		Dim fDist As Double
		Dim Result As Double
		Dim fDistToARP As Double

		Dim pTmpPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pModelPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pUnionPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pModelPoly = CreatePrjCircle(CurrADHP.pPtPrj, RModel)
		fDistToARP = ReturnDistanceInMeters(TurnFixPnt, CurrADHP.pPtPrj)

		Result = fRefHeight
		MaxDh = fRefHeight + fEnrouteMOC

		Do While MaxDh > Result
			Result = MaxDh
			fDist = (Result - TurnFixPnt.Z) / fMissAprPDG
			pTmpPoly = CreatePrjCircle(TurnFixPnt, fDist)

			If fDistToARP + fDist > RModel Then
				pTopo = pModelPoly
				pUnionPoly = pTopo.Union(pTmpPoly)

				pTopo = pUnionPoly
				pTopo.IsKnownSimple_2 = False
				pTopo.Simplify()

				GetObstaclesByPoly(ObstacleList, pUnionPoly, fRefHeight)
			End If

			MaxDh = AnaliseTerminalObstacles(ObstacleList, pTmpPoly) + fEnrouteMOC
		Loop

		CalcHTMA = Result - fRefHeight
	End Function

	Function Intersect2Lines(ByVal A0 As Double, ByVal B0 As Double, ByVal C0 As Double, ByVal a1 As Double, ByVal b1 As Double, ByVal C1 As Double) As ESRI.ArcGIS.Geometry.IPoint
		Dim D As Double
		Dim X As Double
		Dim Y As Double

		Intersect2Lines = New ESRI.ArcGIS.Geometry.Point
		D = A0 * b1 - a1 * B0
		If D = 0.0 Then Exit Function

		X = (B0 * C1 - b1 * C0) / D
		Y = (C0 * a1 - C1 * A0) / D
		Intersect2Lines.PutCoords(X, Y)
	End Function

	Sub GetLineComponents(ByVal PlaneA As D3DPlane, ByVal PlaneB As D3DPlane, ByRef LineA As Double, ByRef LineB As Double, ByRef LineC As Double)
		LineA = PlaneA.C * PlaneB.A - PlaneA.A * PlaneB.C
		LineB = PlaneA.C * PlaneB.B - PlaneA.B * PlaneB.C
		LineC = PlaneA.C * PlaneB.D - PlaneA.D * PlaneB.C
	End Sub

	Sub GetReducedTurnAreaObstacles(ByVal ObstacleList As ObstacleContainer, ByRef ObstacleTurnAreaMOCList As ObstacleContainer, ByVal BaseArea As ESRI.ArcGIS.Geometry.IPointCollection, ByVal ZNRPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal pDistPolygon As ESRI.ArcGIS.Geometry.IPolygon, ByVal KKLine As ESRI.ArcGIS.Geometry.IPolyline, ByVal TurnAngle As Double, ByVal TNH As Double, ByVal MAPDG As Double, ByVal FixType As Integer, ByVal ptNavPrj As ESRI.ArcGIS.Geometry.Point, ByVal SecPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByVal OutAzt As Double, ByVal TurnDir As Integer, ByVal EarlyDir As Double, ByVal ArDir As Double, ByVal ptLHPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal OASPlanes() As D3DPolygone, ByRef iMax As Integer) 'As Integer
		Dim pXPlane As D3DPlane
		Dim pYPlane As D3DPlane
		Dim pWPlane As D3DPlane

		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry

		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection

		Dim ptXWIntersect As ESRI.ArcGIS.Geometry.IPoint
		Dim ptKYIntersect As ESRI.ArcGIS.Geometry.IPoint

		Dim ptOWIntersect As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOXIntersect As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOYIntersect As ESRI.ArcGIS.Geometry.IPoint

		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pCutLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pTmpPolygonL As ESRI.ArcGIS.Geometry.IPolygon
		Dim pTmpPolygonR As ESRI.ArcGIS.Geometry.IPolygon
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pDistProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTopoOper2 As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pBaseRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pSecondRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pYPlaneRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		Dim LineA As Double
		Dim LineB As Double
		Dim LineC As Double

		Dim DistX As Double
		Dim DistY As Double

		Dim PlaneA As Double
		Dim PlaneB As Double
		Dim PlaneC As Double

		Dim DistStar As Double
		Dim ArDirInRad As Double
		Dim EarlyLocal As Double

		Dim D As Double
		Dim dcX As Double
		Dim dcY As Double
		Dim SinA As Double
		Dim CosA As Double
		Dim hMax As Double
		Dim fTmp As Double
		Dim dMin As Double
		Dim dMax As Double
		Dim MinPDG As Double
		Dim MOCValue As Double

		Dim C As Integer
		Dim E As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer
		Dim iEnd As Integer
		Dim iStart As Integer
		Dim Side0 As Integer
		Dim Side1 As Integer

		Dim bPassFlg As Boolean
		Dim bAdded As Boolean

		M = UBound(ObstacleList.Obstacles)

		If (M < 0) Or (BaseArea.PointCount < 1) Then
			ReDim ObstacleTurnAreaMOCList.Obstacles(-1)
			ReDim ObstacleTurnAreaMOCList.Parts(-1)
			Return '-1
		End If

		ReDim ObstacleTurnAreaMOCList.Obstacles(M)

		N = Math.Max(UBound(ObstacleList.Parts), M)
		C = N
		ReDim ObstacleTurnAreaMOCList.Parts(C)

		pBaseRelation = BaseArea

		If FixType >= 0 Then
			pSecondRelation = SecPoly

			pTopoOper2 = SecPoly
			pTopoOper2.IsKnownSimple_2 = False
			pTopoOper2.Simplify()
		End If

		'    If TurnAngle > 15 Then
		If TurnAngle > arMATurnTrshAngl.Value Then
			MOCValue = arMA_FinalMOC.Value
		Else
			MOCValue = arMA_InterMOC.Value
		End If

		pLine = New ESRI.ArcGIS.Geometry.Polyline
		pLine.FromPoint = PointAlongPlane(KKLine.FromPoint, ArDir + 90.0, 100000.0)
		pLine.ToPoint = PointAlongPlane(KKLine.FromPoint, ArDir - 90.0, 100000.0)
		pTopoOper = ZNRPoly
		pTopoOper.Cut(pLine, pTmpPolygonL, pPointCollection)

		'DrawPolygon pPointCollection, 128
		pTopoOper = pPointCollection
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		ArDirInRad = DegToRad(ArDir + 180.0)
		SinA = System.Math.Sin(-ArDirInRad)
		CosA = System.Math.Cos(-ArDirInRad)

		'=======================================================

		If TurnAngle > 75.0 Then
			pDistProxi = pDistPolygon
			pYPlaneRelation = pTopoOper.Intersect(OASPlanes(eOAS.ZPlane + TurnDir).Poly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
			MinPDG = MAPDG 'Min(MAPDG, OASPlanes(eOAS.WPlane).Plane.A)
			'pYPlane =====================================
			pYPlane = OASPlanes(eOAS.ZPlane + TurnDir).Plane
			fTmp = pYPlane.A * CosA + pYPlane.B * SinA
			pYPlane.B = -pYPlane.A * SinA + pYPlane.B * CosA
			pYPlane.A = fTmp
			pYPlane.D = pYPlane.D - (pYPlane.A * ptLHPrj.X + pYPlane.B * ptLHPrj.Y)
		Else
			pDistProxi = KKLine

			EarlyLocal = DegToRad(EarlyDir)
			PlaneA = -System.Math.Sin(EarlyLocal)
			PlaneB = System.Math.Cos(EarlyLocal)
			MinPDG = MAPDG
			'pXPlane =====================================
			pXPlane = OASPlanes(eOAS.ZPlane - 2 * TurnDir).Plane
			fTmp = pXPlane.A * CosA + pXPlane.B * SinA
			pXPlane.B = -pXPlane.A * SinA + pXPlane.B * CosA
			pXPlane.A = fTmp
			pXPlane.D = pXPlane.D - (pXPlane.A * ptLHPrj.X + pXPlane.B * ptLHPrj.Y)
			'pYPlane =====================================
			pYPlane = OASPlanes(eOAS.ZPlane - TurnDir).Plane
			fTmp = pYPlane.A * CosA + pYPlane.B * SinA
			pYPlane.B = -pYPlane.A * SinA + pYPlane.B * CosA
			pYPlane.A = fTmp
			pYPlane.D = pYPlane.D - (pYPlane.A * ptLHPrj.X + pYPlane.B * ptLHPrj.Y)
			'pWPlane =====================================
			pWPlane = OASPlanes(eOAS.WPlane).Plane
			fTmp = pWPlane.A * CosA + pWPlane.B * SinA
			pWPlane.B = -pWPlane.A * SinA + pWPlane.B * CosA
			pWPlane.A = fTmp
			pWPlane.D = pWPlane.D - (pWPlane.A * ptLHPrj.X + pWPlane.B * ptLHPrj.Y)
			'=====================================
			GetLineComponents(pXPlane, pWPlane, LineA, LineB, LineC)

			ptXWIntersect = Intersect2Lines(pXPlane.A, pXPlane.B, pXPlane.C * TNH + pXPlane.D, pWPlane.A, pWPlane.B, pWPlane.C * TNH + pWPlane.D)
			'        Set ptXYIntersect = Intersect2Lines(pXPlane.A, pXPlane.B, pXPlane.C * TNH + pXPlane.D, pYPlane.A, pYPlane.B, pYPlane.C * TNH + pYPlane.D)

			If TurnDir > 0 Then
				ptKYIntersect = KKLine.ToPoint
			Else
				ptKYIntersect = KKLine.FromPoint
			End If

			pLine = IntersectPlanes(pXPlane, pWPlane, -1000.0, TNH + 1000.0)

			pTopoOper.Cut(pLine, pTmpPolygonL, pTmpPolygonR)

			If TurnDir > 0 Then
				pTopoOper = pTmpPolygonL
			Else
				pTopoOper = pTmpPolygonR
			End If

			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pYPlaneRelation = pTopoOper.Union(pDistPolygon)
		End If

		pTopoOper = pYPlaneRelation
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		K = -1
		L = -1
		hMax = 0.0
		iMax = -1

		pCutLine = New ESRI.ArcGIS.Geometry.Polyline

		'For I = 0 To M
		'	ObstacleList.Obstacles(I).NIx = -1
		K = -1
		L = -1

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj
			If pBaseRelation.Disjoint(pCurrGeom) Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				pTmpPoints = pTopoOper.Intersect(OASPlanes(eOAS.ZPlane + TurnDir).Poly, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(OASPlanes(eOAS.ZPlane + TurnDir).Poly, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(OASPlanes(eOAS.ZPlane + TurnDir).Poly, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				pTmpPoints = pTopoOper.Intersect(pDistPolygon, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(pDistPolygon, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(pDistPolygon, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				pTmpPoints = pTopoOper.Intersect(ZNRPoly, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(ZNRPoly, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(ZNRPoly, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				If FixType >= 0 Then
					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				End If

				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			bAdded = False

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				If TurnAngle > 75.0 Then
					If Not pYPlaneRelation.Disjoint(pCurrPt) Then
						fTmp = pYPlane.A * pCurrPt.X + pYPlane.B * pCurrPt.Y + pYPlane.D
						bPassFlg = (Not pBaseRelation.Disjoint(pCurrPt)) And (ObstacleList.Obstacles(I).Height > fTmp)
					Else
						bPassFlg = Not pBaseRelation.Disjoint(pCurrPt)
					End If
				Else
					bPassFlg = (Not pBaseRelation.Disjoint(pCurrPt)) And (pYPlaneRelation.Disjoint(pCurrPt))
				End If

				If Not bPassFlg Then Continue For

				If TurnAngle > 75.0 Then
					DistStar = pDistProxi.ReturnDistance(pCurrPt)
				Else
					If SideDef(ptKYIntersect, EarlyDir, pCurrPt) = TurnDir Then
						DistStar = pDistProxi.ReturnDistance(pCurrPt)
					Else
						PlaneC = -(pCurrPt.X * PlaneA + pCurrPt.Y * PlaneB)

						If SideDef(ptXWIntersect, EarlyDir, pCurrPt) = -TurnDir Then
							ptOWIntersect = Intersect2Lines(LineA, LineB, LineC, PlaneA, PlaneB, PlaneC)
							DistStar = ReturnDistanceInMeters(pCurrPt, ptOWIntersect)
						Else
							ptOXIntersect = Intersect2Lines(pXPlane.A, pXPlane.B, pXPlane.C * TNH + pXPlane.D, PlaneA, PlaneB, PlaneC)
							ptOYIntersect = Intersect2Lines(pYPlane.A, pYPlane.B, pYPlane.C * TNH + pYPlane.D, PlaneA, PlaneB, PlaneC)

							DistX = ReturnDistanceInMeters(pCurrPt, ptOXIntersect)
							DistY = ReturnDistanceInMeters(pCurrPt, ptOYIntersect)
							D = DistX
							If D < DistY Then D = DistY
							DistStar = D
						End If
					End If
				End If

				If DistStar <= 0.0 Then Continue For

				If Not bAdded Then
					bAdded = True
					L += 1
					ObstacleTurnAreaMOCList.Obstacles(L) = ObstacleList.Obstacles(I)
					ObstacleTurnAreaMOCList.Obstacles(L).PartsNum = 0
					ReDim ObstacleTurnAreaMOCList.Obstacles(L).Parts(P)
				End If

				K += 1
				If K >= C Then
					C += N
					ReDim Preserve ObstacleTurnAreaMOCList.Parts(C)
				End If

				ObstacleTurnAreaMOCList.Parts(K).pPtPrj = pCurrPt
				ObstacleTurnAreaMOCList.Parts(K).Owner = L
				ObstacleTurnAreaMOCList.Parts(K).Height = ObstacleTurnAreaMOCList.Obstacles(L).Height
				ObstacleTurnAreaMOCList.Parts(K).Index = ObstacleTurnAreaMOCList.Obstacles(L).PartsNum
				ObstacleTurnAreaMOCList.Obstacles(L).Parts(ObstacleTurnAreaMOCList.Parts(K).Index) = K
				ObstacleTurnAreaMOCList.Obstacles(L).PartsNum += 1

				ObstacleTurnAreaMOCList.Parts(K).DistStar = DistStar
				ObstacleTurnAreaMOCList.Parts(K).Flags = 0
				ObstacleTurnAreaMOCList.Parts(K).fTmp = 1.0

				If (FixType >= 0) Then
					If pSecondRelation.Contains(pCurrPt) Then
						ObstacleTurnAreaMOCList.Parts(K).Flags = 1

						PrjToLocal(ptNavPrj, OutAzt, pCurrPt, dcX, dcY)
						Side0 = Math.Sign(dcY)

						pCutLine.FromPoint = LocalToPrj(ptNavPrj, OutAzt, dcX)
						pCutLine.ToPoint = LocalToPrj(ptNavPrj, OutAzt, dcX, 10.0 * Side0 * MaxModelRadius)

						'Side0 = SideDef(ptNavPrj, OutAzt, pCurrPt)
						'dCL = Point2LineDistancePrj(pCurrPt, ptNavPrj, OutAzt)

						'pCutLine.FromPoint = PointAlongPlane(pCurrPt, OutAzt + 90.0 * Side0, dCL)
						'pCutLine.ToPoint = PointAlongPlane(pCurrPt, OutAzt - 90.0 * Side0, 10.0 * MaxModelRadius)

						'DrawPolyLine(pCutLine, -1, 1)
						'DrawPolygon(pYPlaneRelation, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross)

						'DrawPolygon(pTopoOper, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)

						'While True
						'Application.DoEvents()
						'End While

						pLine = pTopoOper2.Intersect(pCutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

						pPointCollection = pLine
						If pPointCollection.PointCount > 2 Then
							iStart = 0
							iEnd = 0
							dMin = Point2LineDistancePrj(pPointCollection.Point(0), ptNavPrj, OutAzt)
							dMax = dMin

							For E = 1 To pPointCollection.PointCount - 1
								D = Point2LineDistancePrj(pPointCollection.Point(E), ptNavPrj, OutAzt)
								If D > dMax Then
									iEnd = E
									dMax = D
								End If

								If D < dMin Then
									iStart = E
									dMin = D
								End If
							Next E

							pLine = New ESRI.ArcGIS.Geometry.Polyline
							pLine.FromPoint = pPointCollection.Point(iStart)
							pLine.ToPoint = pPointCollection.Point(iEnd)
							pPointCollection = Nothing
						Else
							Side1 = SideDef(pLine.FromPoint, OutAzt, pLine.ToPoint)
							If Side0 = Side1 Then pLine.ReverseOrientation()
						End If

						ObstacleTurnAreaMOCList.Parts(K).fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length
					End If
				End If

				ObstacleTurnAreaMOCList.Parts(K).MOC = MOCValue * ObstacleTurnAreaMOCList.Parts(K).fTmp
				ObstacleTurnAreaMOCList.Parts(K).ReqH = ObstacleTurnAreaMOCList.Parts(K).Height + ObstacleTurnAreaMOCList.Parts(K).MOC
				ObstacleTurnAreaMOCList.Parts(K).EffectiveHeight = TNH + ObstacleTurnAreaMOCList.Parts(K).DistStar * MinPDG
				ObstacleTurnAreaMOCList.Parts(K).hPenet = ObstacleTurnAreaMOCList.Parts(K).ReqH - ObstacleTurnAreaMOCList.Parts(K).EffectiveHeight
				ObstacleTurnAreaMOCList.Parts(K).Dist = ObstacleTurnAreaMOCList.Parts(K).DistStar

				If ObstacleTurnAreaMOCList.Parts(K).hPenet > hMax Then
					hMax = ObstacleTurnAreaMOCList.Parts(K).hPenet
					iMax = K
				End If
			Next J

			If bAdded Then
				ReDim Preserve ObstacleTurnAreaMOCList.Obstacles(L).Parts(ObstacleTurnAreaMOCList.Obstacles(L).PartsNum - 1)
			End If
		Next I

		If K >= 0 Then
			ReDim Preserve ObstacleTurnAreaMOCList.Parts(K)
			ReDim Preserve ObstacleTurnAreaMOCList.Obstacles(L)
		Else
			ReDim ObstacleTurnAreaMOCList.Parts(-1)
			ReDim ObstacleTurnAreaMOCList.Obstacles(-1)
		End If
		'Return K
	End Sub

	Sub GetTurnAreaObstacles(ByVal ObstacleList As ObstacleContainer, ByRef ObstacleTurnAreaMOCList As ObstacleContainer, ByVal BaseArea As ESRI.ArcGIS.Geometry.IPointCollection, ByVal DistPoly As ESRI.ArcGIS.Geometry.IGeometry, ByVal TurnAngle As Integer, ByVal TNH As Double, ByVal MAPDG As Double, ByVal NavType As Integer, ByVal ptNavPrj As ESRI.ArcGIS.Geometry.Point, ByVal SecPoly As ESRI.ArcGIS.Geometry.IPointCollection, ByVal OutAzt As Double, ByRef iMax As Integer) 'As Integer
		Dim bAdded As Boolean

		Dim C As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim P As Integer
		Dim iEnd As Integer
		Dim Side0 As Integer
		Dim Side1 As Integer
		Dim iStart As Integer

		Dim D As Double
		Dim dcY As Double
		Dim dcX As Double
		Dim hMax As Double
		Dim dMin As Double
		Dim dMax As Double
		Dim MOCValue As Double
		Dim DistStar As Double

		Dim pCurrPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pCurrGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim pTmpPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pObstPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pDistProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pBaseRelation As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pSecondRelation As ESRI.ArcGIS.Geometry.IRelationalOperator

		M = UBound(ObstacleList.Obstacles)
		iMax = -1

		If (M < 0) Or (BaseArea.PointCount < 1) Then
			ReDim ObstacleTurnAreaMOCList.Obstacles(-1)
			ReDim ObstacleTurnAreaMOCList.Parts(-1)
			Return '-1
		End If

		ReDim ObstacleTurnAreaMOCList.Obstacles(M)

		N = Math.Max(UBound(ObstacleList.Parts), M)
		C = N
		ReDim ObstacleTurnAreaMOCList.Parts(C)

		pBaseRelation = BaseArea
		pSecondRelation = SecPoly

		If NavType >= 0 Then
			pTopoOper = SecPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
		End If

		pDistProxi = DistPoly

		K = -1
		L = -1
		hMax = 0.0

		'If TurnAngle > arMATurnTrshAngl.Value Then
		If TurnAngle > 15 Then
			MOCValue = arMA_FinalMOC.Value
		Else
			MOCValue = arMA_InterMOC.Value
		End If

		pLine = New ESRI.ArcGIS.Geometry.Polyline

		For I = 0 To M
			pCurrGeom = ObstacleList.Obstacles(I).pGeomPrj

			If pBaseRelation.Disjoint(pCurrGeom) Then Continue For

			If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPoint Then
				pObstPoints = New ESRI.ArcGIS.Geometry.Multipoint
				pObstPoints.AddPoint(pCurrGeom)
			Else
				pTopoOper = pCurrGeom

				pObstPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry0Dimension)
				pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(BaseArea, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If

				If NavType >= 0 Then
					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry0Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry1Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)

					If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon Then
						pTmpPoints = pTopoOper.Intersect(SecPoly, esriGeometryDimension.esriGeometry2Dimension)
						pObstPoints.AddPointCollection(pTmpPoints)
					End If
				End If

				pTmpPoints = pTopoOper.Intersect(DistPoly, esriGeometryDimension.esriGeometry0Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				pTmpPoints = pTopoOper.Intersect(DistPoly, esriGeometryDimension.esriGeometry1Dimension)
				pObstPoints.AddPointCollection(pTmpPoints)

				If pCurrGeom.GeometryType = esriGeometryType.esriGeometryPolygon And DistPoly.GeometryType = esriGeometryType.esriGeometryPolygon Then
					pTmpPoints = pTopoOper.Intersect(DistPoly, esriGeometryDimension.esriGeometry2Dimension)
					pObstPoints.AddPointCollection(pTmpPoints)
				End If
				RemoveSeamPoints(pObstPoints)
			End If

			P = pObstPoints.PointCount - 1
			bAdded = False

			For J = 0 To P
				pCurrPt = pObstPoints.Point(J)
				DistStar = pDistProxi.ReturnDistance(pCurrPt)

				If DistStar <= 0.0 Then Continue For

				If Not bAdded Then
					L += 1
					ObstacleTurnAreaMOCList.Obstacles(L) = ObstacleList.Obstacles(I)
					ObstacleTurnAreaMOCList.Obstacles(L).PartsNum = 0
					ReDim ObstacleTurnAreaMOCList.Obstacles(L).Parts(P)
					bAdded = True
				End If

				K += 1
				If K >= C Then
					C += N
					ReDim Preserve ObstacleTurnAreaMOCList.Parts(C)
				End If

				ObstacleTurnAreaMOCList.Parts(K).pPtPrj = pCurrPt
				ObstacleTurnAreaMOCList.Parts(K).Owner = L
				ObstacleTurnAreaMOCList.Parts(K).Height = ObstacleTurnAreaMOCList.Obstacles(L).Height
				ObstacleTurnAreaMOCList.Parts(K).Index = ObstacleTurnAreaMOCList.Obstacles(L).PartsNum
				ObstacleTurnAreaMOCList.Obstacles(L).Parts(ObstacleTurnAreaMOCList.Parts(K).Index) = K
				ObstacleTurnAreaMOCList.Obstacles(L).PartsNum += 1

				ObstacleTurnAreaMOCList.Parts(K).DistStar = DistStar
				ObstacleTurnAreaMOCList.Parts(K).Flags = 0
				ObstacleTurnAreaMOCList.Parts(K).fTmp = 1.0

				If (NavType >= 0) Then
					If pSecondRelation.Contains(pCurrPt) Then
						ObstacleTurnAreaMOCList.Parts(K).Flags = 1

						PrjToLocal(ptNavPrj, OutAzt, pCurrPt, dcX, dcY)
						Side0 = Math.Sign(dcY)

						pLine.FromPoint = LocalToPrj(ptNavPrj, OutAzt, dcX)
						pLine.ToPoint = LocalToPrj(ptNavPrj, OutAzt, dcX, 10.0 * Side0 * MaxModelRadius)

						'Side0 = SideDef(ptNavPrj, OutAzt, pCurrPt)
						'dcY = Point2LineDistancePrj(pCurrPt, ptNavPrj, OutAzt)
						'pLine.FromPoint = PointAlongPlane(pCurrPt, OutAzt + 90.0 * Side0, dcY)
						'pLine.ToPoint = PointAlongPlane(pCurrPt, OutAzt - 90.0 * Side0, 10.0 * MaxModelRadius)

						pTopoOper = SecPoly
						pLine = pTopoOper.Intersect(pLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

						pPointCollection = pLine
						If pPointCollection.PointCount > 2 Then
							iStart = 0
							iEnd = 0
							dMin = Point2LineDistancePrj(pPointCollection.Point(0), ptNavPrj, OutAzt)
							dMax = dMin

							For L = 1 To pPointCollection.PointCount - 1
								D = Point2LineDistancePrj(pPointCollection.Point(L), ptNavPrj, OutAzt)
								If D > dMax Then
									iEnd = L
									dMax = D
								End If
								If D < dMin Then
									iStart = L
									dMin = D
								End If
							Next L
							pLine = New ESRI.ArcGIS.Geometry.Polyline
							pLine.FromPoint = pPointCollection.Point(iStart)
							pLine.ToPoint = pPointCollection.Point(iEnd)

							pPointCollection = Nothing
						Else
							Side1 = SideDef(pLine.FromPoint, OutAzt, pLine.ToPoint)
							If Side0 = Side1 Then pLine.ReverseOrientation()
						End If

						ObstacleTurnAreaMOCList.Parts(K).fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length
						'                ObstacleTurnAreaMOCList(J).Dist = pDistProxi.ReturnDistance(ptCur)
					End If
				End If

				ObstacleTurnAreaMOCList.Parts(K).MOC = MOCValue * ObstacleTurnAreaMOCList.Parts(K).fTmp
				ObstacleTurnAreaMOCList.Parts(K).ReqH = ObstacleTurnAreaMOCList.Parts(K).Height + ObstacleTurnAreaMOCList.Parts(K).MOC
				ObstacleTurnAreaMOCList.Parts(K).EffectiveHeight = TNH + ObstacleTurnAreaMOCList.Parts(K).DistStar * MAPDG
				ObstacleTurnAreaMOCList.Parts(K).hPenet = ObstacleTurnAreaMOCList.Parts(K).ReqH - ObstacleTurnAreaMOCList.Parts(K).EffectiveHeight
				ObstacleTurnAreaMOCList.Parts(K).Dist = ObstacleTurnAreaMOCList.Parts(K).DistStar

				If ObstacleTurnAreaMOCList.Parts(K).hPenet > hMax Then
					hMax = ObstacleTurnAreaMOCList.Parts(K).hPenet
					iMax = K
				End If
			Next J

			If bAdded Then
				ReDim Preserve ObstacleTurnAreaMOCList.Obstacles(L).Parts(ObstacleTurnAreaMOCList.Obstacles(L).PartsNum - 1)
			End If
		Next I

		If K >= 0 Then
			ReDim Preserve ObstacleTurnAreaMOCList.Parts(K)
			ReDim Preserve ObstacleTurnAreaMOCList.Obstacles(L)
		Else
			ReDim ObstacleTurnAreaMOCList.Parts(-1)
			ReDim ObstacleTurnAreaMOCList.Obstacles(-1)
		End If

		Return 'K
	End Sub

	Public Sub CalcEffectiveHeights(ByRef ObstList As ObstacleContainer, ByVal fGPAngle As Double, ByVal fMAPDG As Double, ByVal fRDH As Double, Optional ByVal ILSObs As Boolean = False)
		Dim I As Integer
		Dim N As Integer

		Dim fTmp As Double
		Dim CoTanZ As Double
		Dim TanGPA As Double
		Dim CoTanGPA As Double
		Dim ZSurfaceOrigin As Double

		CoTanZ = 1.0 / fMAPDG

		TanGPA = System.Math.Tan(DegToRad(fGPAngle))
		CoTanGPA = 1.0 / TanGPA

		ZSurfaceOrigin = OASZOrigin
		If fGPAngle > MaxRefGPAngle Then
			ZSurfaceOrigin = (OASZOrigin + 500.0 * (fGPAngle - MaxRefGPAngle))
		End If

		N = UBound(ObstList.Parts)

		For I = 0 To N
			If ILSObs Then
				If ObstList.Parts(I).Dist < 0 Then
					ObstList.Parts(I).ReqH = fRDH
				Else
					ObstList.Parts(I).ReqH = ObstList.Parts(I).Dist * TanGPA + fRDH
				End If
			Else
				Select Case ObstList.Parts(I).Plane
					Case eOAS.ZeroPlane
						ObstList.Parts(I).ReqH = fRDH
					Case eOAS.ZPlane
						ObstList.Parts(I).ReqH = 0.0
					Case eOAS.WPlane, eOAS.XlPlane, eOAS.XrPlane
						ObstList.Parts(I).ReqH = OASPlanes(ObstList.Parts(I).Plane).Plane.A * ObstList.Parts(I).Dist + OASPlanes(ObstList.Parts(I).Plane).Plane.B * ObstList.Parts(I).DistStar + OASPlanes(ObstList.Parts(I).Plane).Plane.D + arISegmentMOC.Value
					Case eOAS.YlPlane, eOAS.YrPlane
						If ObstList.Parts(I).Dist < 0 Then
							ObstList.Parts(I).ReqH = fRDH
						Else
							ObstList.Parts(I).ReqH = ObstList.Parts(I).Dist * TanGPA + fRDH
						End If
				End Select
			End If

			If ObstList.Parts(I).ReqH < arISegmentMOC.Value Then ObstList.Parts(I).ReqH = arISegmentMOC.Value

			fTmp = (ObstList.Parts(I).Height * CoTanZ + (ZSurfaceOrigin + ObstList.Parts(I).Dist)) / (CoTanZ + CoTanGPA)
			ObstList.Parts(I).EffectiveHeight = fTmp

			'    If ObstList(I).Dist < -ZSurfaceOrigin Then
			If ObstList.Parts(I).EffectiveHeight < ObstList.Parts(I).Height Then
				ObstList.Parts(I).Flags = 1
			Else
				ObstList.Parts(I).Flags = 0
			End If
		Next I
	End Sub

	Sub OAS_DATABase(ByVal LLZ2THRDist As Double, ByVal GPAngle As Double, ByVal MisAprGr As Double, ByVal ILSCategory As Integer, ByVal RDH As Double, ByVal Ss As Double, ByVal St As Double, ByRef OASPlanes() As D3DPolygone)
		Dim sTabName As String
		Dim sMisAprGr As String
		Dim sGPAngle As String
		Dim CatOffset As Integer
		Dim sLLZ2THRDist As String

		Dim fTmp0 As Double
		Dim fTmp1 As Double
		Dim P As Double
		Dim CwCorr As Double
		Dim Cw_Corr As Double
		Dim CxCorr As Double
		Dim CyCorr As Double
		Dim RDHCorr As Double

		fTmp0 = GPAngle
		If fTmp0 > MaxRefGPAngle Then fTmp0 = MaxRefGPAngle

		sGPAngle = CStr(System.Math.Round(fTmp0 * 10.0 - 0.4999))

		If LLZ2THRDist < 2000.0 Then LLZ2THRDist = 2000.0
		If LLZ2THRDist > 4500.0 Then LLZ2THRDist = 4500.0
		If LLZ2THRDist > 4400.0 Then
			sLLZ2THRDist = CStr(System.Math.Round(0.01 * LLZ2THRDist - 0.4999) * 100.0)
		Else
			sLLZ2THRDist = CStr(System.Math.Round(0.005 * LLZ2THRDist - 0.4999) * 200.0)
		End If

		sTabName = sGPAngle + "_" + sLLZ2THRDist

		sMisAprGr = CStr(System.Math.Round(MisAprGr * 1000.0))

		CatOffset = 3 * (ILSCategory - 1)
		'If RDH > arAbv_Treshold.Value Then RDH = arAbv_Treshold.Value
		RDHCorr = RDH - arAbv_Treshold.Value

		CwCorr = St - 6.0 - RDHCorr
		Cw_Corr = St - 6.0 - RDHCorr

		OASPlanes(eOAS.ZeroPlane).Plane.A = 0.0
		OASPlanes(eOAS.ZeroPlane).Plane.B = 0.0
		OASPlanes(eOAS.ZeroPlane).Plane.C = -1.0
		OASPlanes(eOAS.ZeroPlane).Plane.D = 0.0

		OASPlanes(eOAS.CommonPlane).Plane.A = 0.0
		OASPlanes(eOAS.CommonPlane).Plane.B = 0.0
		OASPlanes(eOAS.CommonPlane).Plane.C = -1.0
		OASPlanes(eOAS.CommonPlane).Plane.D = 300.0

		Dim conn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ConstDir + "\plans.mdb;Jet OLEDB:Database Password=test")
		Dim pCommand As OleDbCommand
		Dim dr As OleDbDataReader

		conn.Open()

		'Try
		'Catch ex As Exception
		'	Console.WriteLine(ex.Message)
		'End Try


		pCommand = conn.CreateCommand()

		'' 'Region "W"
		pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'W'"
		dr = pCommand.ExecuteReader()

		If dr.Read() Then
			OASPlanes(eOAS.WPlane).Plane.A = Convert.ToDouble(dr(1 + CatOffset))
			OASPlanes(eOAS.WPlane).Plane.B = -Convert.ToDouble(dr(2 + CatOffset))
			OASPlanes(eOAS.WPlane).Plane.C = -1.0
			OASPlanes(eOAS.WPlane).Plane.D = Convert.ToDouble(dr(3 + CatOffset)) - CwCorr

			If GPAngle > MaxRefGPAngle Then
				OASPlanes(eOAS.WPlane).Plane.A = OASPlanes(eOAS.WPlane).Plane.A + 0.0092 * (GPAngle - MaxRefGPAngle)
				OASPlanes(eOAS.WPlane).Plane.D = -6.45 - CwCorr
			End If
		Else
			dr.Close()
			Throw New Exception("Plans mdb is invalid. 'W' not found.")
		End If
		dr.Close()

		'' 'Region "X"
		pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'X'"
		dr = pCommand.ExecuteReader()

		If dr.Read() Then
			OASPlanes(eOAS.XlPlane).Plane.A = Convert.ToDouble(dr(1 + CatOffset))
			OASPlanes(eOAS.XlPlane).Plane.B = -Convert.ToDouble(dr(2 + CatOffset))
			OASPlanes(eOAS.XlPlane).Plane.C = -1.0

			fTmp0 = -St / OASPlanes(eOAS.XlPlane).Plane.B
			fTmp1 = Ss - (St - 3) / OASPlanes(eOAS.XlPlane).Plane.B
			CxCorr = Max(fTmp0, fTmp1)

			fTmp0 = -6 / OASPlanes(eOAS.XlPlane).Plane.B
			fTmp1 = 30 - 3 / OASPlanes(eOAS.XlPlane).Plane.B
			P = CxCorr - Max(fTmp0, fTmp1)

			CxCorr = -P * OASPlanes(eOAS.XlPlane).Plane.B - RDHCorr
			OASPlanes(eOAS.XlPlane).Plane.D = Convert.ToDouble(dr(3 + CatOffset)) - CxCorr
		Else
			dr.Close()
			Throw New Exception("Plans mdb is invalid. 'X' not found.")
		End If
		dr.Close()

		'' 'Region "Y"

		pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'Y" + sMisAprGr + "'"
		dr = pCommand.ExecuteReader()

		If dr.Read() Then
			OASPlanes(eOAS.YlPlane).Plane.A = Convert.ToDouble(dr(1 + CatOffset))
			OASPlanes(eOAS.YlPlane).Plane.B = -Convert.ToDouble(dr(2 + CatOffset))
			OASPlanes(eOAS.YlPlane).Plane.C = -1.0

			'fTmp0 = -St / OASPlanes(eOAS.YlPlane).Plane.B
			'fTmp1 = Ss - (St - 3) / OASPlanes(eOAS.YlPlane).Plane.B
			'CyCorr = Max(fTmp0, fTmp1)
			'
			'fTmp0 = -6 / OASPlanes(eOAS.YlPlane).Plane.B
			'fTmp1 = 30 - 3 / OASPlanes(eOAS.YlPlane).Plane.B
			'Py = CyCorr - Max(fTmp0, fTmp1)

			'CyCorr = -Py * OASPlanes(eOAS.YlPlane).Plane.B - RDHCorr

			CyCorr = -P * OASPlanes(eOAS.YlPlane).Plane.B - RDHCorr
			OASPlanes(eOAS.YlPlane).Plane.D = Convert.ToDouble(dr(3 + CatOffset)) - CyCorr
		Else
			dr.Close()
			Throw New Exception("Plans mdb is invalid. 'Y" + sMisAprGr + "'" + " not found.")
		End If
		dr.Close()

		'' 'Region "Z"

		pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'Z" + sMisAprGr + "'"
		dr = pCommand.ExecuteReader()

		If dr.Read() Then
			OASPlanes(eOAS.ZPlane).Plane.A = Convert.ToDouble(dr(1 + CatOffset))
			OASPlanes(eOAS.ZPlane).Plane.B = -Convert.ToDouble(dr(2 + CatOffset))
			OASPlanes(eOAS.ZPlane).Plane.C = -1.0
			OASPlanes(eOAS.ZPlane).Plane.D = Convert.ToDouble(dr(3 + CatOffset))

			If GPAngle > MaxRefGPAngle Then
				OASPlanes(eOAS.ZPlane).Plane.D = OASPlanes(eOAS.ZPlane).Plane.A * (OASZOrigin + 500.0 * (GPAngle - MaxRefGPAngle))
			End If
		Else
			dr.Close()
			Throw New Exception("Plans mdb is invalid. 'Z" + sMisAprGr + "'" + " not found.")
		End If
		dr.Close()

		OASPlanes(eOAS.YrPlane).Plane.A = OASPlanes(eOAS.YlPlane).Plane.A
		OASPlanes(eOAS.YrPlane).Plane.B = -OASPlanes(eOAS.YlPlane).Plane.B
		OASPlanes(eOAS.YrPlane).Plane.C = -1.0
		OASPlanes(eOAS.YrPlane).Plane.D = OASPlanes(eOAS.YlPlane).Plane.D

		OASPlanes(eOAS.XrPlane).Plane.A = OASPlanes(eOAS.XlPlane).Plane.A
		OASPlanes(eOAS.XrPlane).Plane.B = -OASPlanes(eOAS.XlPlane).Plane.B
		OASPlanes(eOAS.XrPlane).Plane.C = -1.0
		OASPlanes(eOAS.XrPlane).Plane.D = OASPlanes(eOAS.XlPlane).Plane.D

		'' 'Region "W*"

		If ILSCategory = 3 Then
			pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'W*'"
			dr = pCommand.ExecuteReader()

			If dr.Read() Then
				OASPlanes(eOAS.WsPlane).Plane.A = Convert.ToDouble(dr(1 + CatOffset))
				OASPlanes(eOAS.WsPlane).Plane.B = -Convert.ToDouble(dr(2 + CatOffset))
				OASPlanes(eOAS.WsPlane).Plane.C = -1.0
				OASPlanes(eOAS.WsPlane).Plane.D = Convert.ToDouble(dr(3 + CatOffset)) - Cw_Corr
			Else
				dr.Close()
				Throw New Exception("Plans mdb is invalid. 'W*' not found.")
			End If
			dr.Close()
		End If

		conn.Close()
	End Sub

#If False Then
		Dim dbsGlis As dao.Database
		Dim rstGlis As dao.Recordset

		dbsGlis = DAODBEngine_definst.OpenDatabase(InstallDir + "\plans.mdb", dao.RecordsetTypeEnum.dbOpenSnapshot, False, ";Pwd=test")
		rstGlis = dbsGlis.OpenRecordset(sTabName, dao.RecordsetTypeEnum.dbOpenDynaset)

		CatOffset = 3 * (ILSCategory - 1)
		'If RDH > arAbv_Treshold.Value Then RDH = arAbv_Treshold.Value
		RDHCorr = RDH - arAbv_Treshold.Value

		CwCorr = St - 6.0 - RDHCorr
		Cw_Corr = St - 6.0 - RDHCorr

		OASPlanes(eOAS.ZeroPlane).Plane.A = 0.0
		OASPlanes(eOAS.ZeroPlane).Plane.B = 0.0
		OASPlanes(eOAS.ZeroPlane).Plane.C = -1.0
		OASPlanes(eOAS.ZeroPlane).Plane.D = 0.0

		OASPlanes(eOAS.CommonPlane).Plane.A = 0.0
		OASPlanes(eOAS.CommonPlane).Plane.B = 0.0
		OASPlanes(eOAS.CommonPlane).Plane.C = -1.0
		OASPlanes(eOAS.CommonPlane).Plane.D = 300.0

		rstGlis.FindFirst("ID = 'W'")
		OASPlanes(eOAS.WPlane).Plane.A = rstGlis.Fields(1 + CatOffset).Value
		OASPlanes(eOAS.WPlane).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
		OASPlanes(eOAS.WPlane).Plane.C = -1.0
		OASPlanes(eOAS.WPlane).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CwCorr

		If GPAngle > MaxRefGPAngle Then
			OASPlanes(eOAS.WPlane).Plane.A = OASPlanes(eOAS.WPlane).Plane.A + 0.0092 * (GPAngle - MaxRefGPAngle)
			OASPlanes(eOAS.WPlane).Plane.D = -6.45 - CwCorr
		End If

		rstGlis.FindFirst("ID = 'X'")
		OASPlanes(eOAS.XlPlane).Plane.A = rstGlis.Fields(1 + CatOffset).Value
		OASPlanes(eOAS.XlPlane).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
		OASPlanes(eOAS.XlPlane).Plane.C = -1.0

		fTmp0 = -St / OASPlanes(eOAS.XlPlane).Plane.B
		fTmp1 = Ss - (St - 3) / OASPlanes(eOAS.XlPlane).Plane.B
		CxCorr = Max(fTmp0, fTmp1)

		fTmp0 = -6 / OASPlanes(eOAS.XlPlane).Plane.B
		fTmp1 = 30 - 3 / OASPlanes(eOAS.XlPlane).Plane.B
		P = CxCorr - Max(fTmp0, fTmp1)

		CxCorr = -P * OASPlanes(eOAS.XlPlane).Plane.B - RDHCorr
		OASPlanes(eOAS.XlPlane).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CxCorr

		rstGlis.FindFirst("ID = 'Y" + sMisAprGr + "'")
		OASPlanes(eOAS.YlPlane).Plane.A = rstGlis.Fields(1 + CatOffset).Value
		OASPlanes(eOAS.YlPlane).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
		OASPlanes(eOAS.YlPlane).Plane.C = -1.0

		'    fTmp0 = -St / OASPlanes(eOAS.YlPlane).Plane.B
		'    fTmp1 = Ss - (St - 3) / OASPlanes(eOAS.YlPlane).Plane.B
		'    CyCorr = Max(fTmp0, fTmp1)
		'
		'    fTmp0 = -6 / OASPlanes(eOAS.YlPlane).Plane.B
		'    fTmp1 = 30 - 3 / OASPlanes(eOAS.YlPlane).Plane.B
		'    Py = CyCorr - Max(fTmp0, fTmp1)

		'    CyCorr = -Py * OASPlanes(eOAS.YlPlane).Plane.B - RDHCorr

		CyCorr = -P * OASPlanes(eOAS.YlPlane).Plane.B - RDHCorr
		OASPlanes(eOAS.YlPlane).Plane.D = rstGlis.Fields(3 + CatOffset).Value - CyCorr

		rstGlis.FindFirst("ID = '" + "Z" + sMisAprGr + "'")
		OASPlanes(eOAS.ZPlane).Plane.A = rstGlis.Fields(1 + CatOffset).Value
		OASPlanes(eOAS.ZPlane).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
		OASPlanes(eOAS.ZPlane).Plane.C = -1.0
		OASPlanes(eOAS.ZPlane).Plane.D = rstGlis.Fields(3 + CatOffset).Value

		If GPAngle > MaxRefGPAngle Then
			OASPlanes(eOAS.ZPlane).Plane.D = OASPlanes(eOAS.ZPlane).Plane.A * (OASZOrigin + 500.0 * (GPAngle - MaxRefGPAngle))
		End If

		OASPlanes(eOAS.YrPlane).Plane.A = OASPlanes(eOAS.YlPlane).Plane.A
		OASPlanes(eOAS.YrPlane).Plane.B = -OASPlanes(eOAS.YlPlane).Plane.B
		OASPlanes(eOAS.YrPlane).Plane.C = -1.0
		OASPlanes(eOAS.YrPlane).Plane.D = OASPlanes(eOAS.YlPlane).Plane.D

		OASPlanes(eOAS.XrPlane).Plane.A = OASPlanes(eOAS.XlPlane).Plane.A
		OASPlanes(eOAS.XrPlane).Plane.B = -OASPlanes(eOAS.XlPlane).Plane.B
		OASPlanes(eOAS.XrPlane).Plane.C = -1.0
		OASPlanes(eOAS.XrPlane).Plane.D = OASPlanes(eOAS.XlPlane).Plane.D

		If ILSCategory = 3 Then
			rstGlis.FindFirst("ID = 'W*'")
			OASPlanes(eOAS.WsPlane).Plane.A = rstGlis.Fields(1 + CatOffset).Value
			OASPlanes(eOAS.WsPlane).Plane.B = -rstGlis.Fields(2 + CatOffset).Value
			OASPlanes(eOAS.WsPlane).Plane.C = -1.0
			OASPlanes(eOAS.WsPlane).Plane.D = rstGlis.Fields(3 + CatOffset).Value - Cw_Corr
		End If

		rstGlis.Close()
		dbsGlis.Close()
#End If

	Function CalcTouchByFixDir(ByVal PtSt As ESRI.ArcGIS.Geometry.IPoint, ByVal ptFIX As ESRI.ArcGIS.Geometry.IPoint, ByVal TurnR As Double, ByVal DirCur As Double, ByVal DirFix As Double, ByVal TurnDir As Integer, ByRef TurnDir2 As Integer, ByVal SnapAngle As Double, ByRef dDir As Double, Optional ByRef FlyBy As ESRI.ArcGIS.Geometry.IPoint = Nothing) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim Constructor As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim PtCnt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt10 As ESRI.ArcGIS.Geometry.IPoint
		Dim Pt11 As ESRI.ArcGIS.Geometry.IPoint

		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt3 As ESRI.ArcGIS.Geometry.IPoint

		Dim SideD As Integer
		Dim SideT As Integer

		Dim DeltaAngle As Double
		Dim DeltaDist As Double
		Dim OutDir As Double
		Dim OutDir0 As Double
		Dim OutDir1 As Double
		Dim Dist As Double

		FlyBy = New ESRI.ArcGIS.Geometry.Point
		CalcTouchByFixDir = New ESRI.ArcGIS.Geometry.Multipoint

		If SubtractAngles(DirCur, DirFix) < 0.5 Then
			DirFix = DirCur
			If ReturnDistanceInMeters(ptFIX, PtSt) < distEps Then
				FlyBy.PutCoords(ptFIX.X, ptFIX.Y)
				CalcTouchByFixDir.AddPoint(PtSt)
				CalcTouchByFixDir.AddPoint(PtSt)
				Exit Function
			End If
		End If

		PtCnt1 = PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR)
		PtSt.M = DirCur

		OutDir0 = Modulus(DirFix - SnapAngle * TurnDir, 360.0)
		OutDir1 = Modulus(DirFix + SnapAngle * TurnDir, 360.0)

		Pt10 = PointAlongPlane(PtCnt1, OutDir0 - 90.0 * TurnDir, TurnR)
		Pt11 = PointAlongPlane(PtCnt1, OutDir1 - 90.0 * TurnDir, TurnR)

		SideT = SideDef(Pt10, DirFix, ptFIX)
		SideD = SideDef(Pt10, DirFix, PtCnt1)

		If SideT * SideD < 0 Then
			pt1 = Pt10
			OutDir = OutDir0
		Else
			pt1 = Pt11
			OutDir = OutDir1
		End If

		pt1.M = OutDir

		Constructor = FlyBy

		Constructor.ConstructAngleIntersection(pt1, DegToRad(OutDir), ptFIX, DegToRad(DirFix))

		Dist = ReturnDistanceInMeters(pt1, FlyBy)

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
			pt2 = PointAlongPlane(FlyBy, OutDir - 180.0, DeltaDist)
			pt3 = PointAlongPlane(FlyBy, DirFix, DeltaDist)
		Else
			pt2 = PointAlongPlane(FlyBy, OutDir, DeltaDist)
			pt3 = PointAlongPlane(FlyBy, DirFix - 180.0, DeltaDist)
		End If

		pt2.M = OutDir
		pt3.M = DirFix

		CalcTouchByFixDir.AddPoint(PtSt)
		CalcTouchByFixDir.AddPoint(pt1)
		CalcTouchByFixDir.AddPoint(pt2)
		CalcTouchByFixDir.AddPoint(pt3)
	End Function

	'Function LatNorm(ByVal Y As Double) As String
	'    If (Y < 0.0) Then
	'        LatNorm = "S"
	'        Y = -Y
	'    Else
	'        LatNorm = "N"
	'    End If
	'End Function
	'
	'Function LonNorm(ByVal X As Double) As String
	'    If (X > 180.0) Then
	'        LonNorm = "W"
	'        X = 360.0 - X
	'    Else
	'        LonNorm = "E"
	'    End If
	'End Function

	Public Sub FillTurnParams(ByVal ptFrom As ESRI.ArcGIS.Geometry.IPoint, ByVal ptTo As ESRI.ArcGIS.Geometry.IPoint, ByRef TrackPoint As ReportPoint)
		Dim fE As Double
		Dim fTmp As Double
		Dim ptCenter As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCenterGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pGeometry As ESRI.ArcGIS.Geometry.IGeometry
		Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline
		Dim pConstructPoint As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection

		fE = DegToRad(0.5)
		fTmp = DegToRad(ptFrom.M - ptTo.M)

		If (System.Math.Abs(System.Math.Sin(fTmp)) <= fE) And (System.Math.Cos(fTmp) > 0.0) Then
			TrackPoint.Radius = NO_DATA_VALUE
			TrackPoint.Turn = 0
			TrackPoint.TurnAngle = 0
			TrackPoint.TurnArcLen = 0
			TrackPoint.CenterLat = ""
			TrackPoint.CenterLon = ""
		Else
			ptCenter = New ESRI.ArcGIS.Geometry.Point
			ptCenterGeo = New ESRI.ArcGIS.Geometry.Point

			If System.Math.Abs(System.Math.Sin(fTmp)) > fE Then
				pConstructPoint = ptCenter
				pConstructPoint.ConstructAngleIntersection(ptFrom, DegToRad(ptFrom.M + 90.0), ptTo, DegToRad(ptTo.M + 90.0))
			Else
				ptCenter.PutCoords(0.5 * (ptFrom.X + ptTo.X), 0.5 * (ptFrom.Y + ptTo.Y))
			End If

			ptCenterGeo.PutCoords(ptCenter.X, ptCenter.Y)
			pGeometry = ptCenterGeo
			pGeometry.SpatialReference = pSpRefPrj
			pGeometry.Project(pSpRefGeo)

			TrackPoint.Radius = ReturnDistanceInMeters(ptCenter, ptFrom)
			TrackPoint.Turn = SideDef(ptFrom, (ptFrom.M), ptTo)
			TrackPoint.TurnAngle = System.Math.Round(Modulus((ptFrom.M - ptTo.M) * TrackPoint.Turn, 360.0), 2)

			TrackPoint.CenterLat = DegreeToString(ptCenterGeo.Y, Degree2StringMode.DMSLat)
			TrackPoint.CenterLon = DegreeToString(ptCenterGeo.X, Degree2StringMode.DMSLon)

			pPointCollection = New ESRI.ArcGIS.Geometry.Polyline
			pPointCollection.AddPoint(ptFrom)
			pPointCollection.AddPoint(ptTo)

			pPolyline = CalcTrajectoryFromMultiPoint(pPointCollection)
			TrackPoint.TurnArcLen = CDbl(CStr(System.Math.Round(pPolyline.Length)))

			ptCenter = Nothing
			ptCenterGeo = Nothing

			pPointCollection = Nothing
			pPolyline = Nothing
		End If
	End Sub

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
		SaveDlg.Title = My.Resources.str00156
		SaveDlg.FileName = Left(ProjectPath, pos2 - 1) + ".htm"
		SaveDlg.Filter = "Text files (*.txt)|*.txt|PANDA Report Files (*.htm)|*.htm|PANDA Report Files (*.html)|*.html|All files (*.*)|*.*"

		If SaveDlg.ShowDialog() <> DialogResult.OK Then Return False

		FileName = SaveDlg.FileName
		'FileTitle = SaveDlg.FileName

		pos = InStrRev(FileName, ".")
		If (pos > 0) Then FileName = Left(FileName, pos - 1)

		FileTitle = FileName
		pos2 = InStrRev(FileTitle, "\")
		If (pos2 > 0) Then FileTitle = FileTitle.Substring(pos2)

		'pos2 = InStrRev(FileTitle, ".")
		'If (pos2 > 0) Then FileTitle = Left(FileTitle, pos2 - 1)

		Return True
	End Function

	Public Sub SafeDeleteElement(graphicElement As ArcGIS.Carto.IElement)
		If graphicElement Is Nothing Then Return

		Try
			GlobalVars.GetActiveView().GraphicsContainer.DeleteElement(graphicElement)
		Catch
			Try
				If TypeOf (graphicElement) Is ESRI.ArcGIS.Carto.GroupElement Then
					Dim i As Integer
					Dim pGroupElement As ESRI.ArcGIS.Carto.IGroupElement = graphicElement

					For i = 0 To pGroupElement.ElementCount - 1
						If Not (pGroupElement.Element(i) Is Nothing) Then
							GlobalVars.GetActiveView().GraphicsContainer.DeleteElement(pGroupElement.Element(i))
						End If
					Next
				End If
			Catch
			End Try
		End Try
	End Sub

	Public Function PriorPostFixTolerance(ByVal pPolygon As IPointCollection, ByVal pPtPrj As IPoint, ByVal fDir As Double, ByRef PriorDist As Double, ByRef PostDist As Double) As Boolean
		Dim I, N As Integer
		Dim fDist As Double
		Dim fMinDist As Double
		Dim fMaxDist As Double

		Dim pCurrPt As IPoint
		Dim pCutterPolyline As IPolyline
		Dim pIntersection As IPointCollection
		Dim pTopological As ITopologicalOperator2

		pCutterPolyline = New Polyline()
		pCutterPolyline.FromPoint = PointAlongPlane(pPtPrj, fDir, 1000000.0)
		pCutterPolyline.ToPoint = PointAlongPlane(pPtPrj, fDir + 180.0, 1000000.0)
		pTopological = pPolygon

		PriorDist = -1.0
		PostDist = -1.0

		Try
			pIntersection = pTopological.Intersect(pCutterPolyline, esriGeometryDimension.esriGeometry0Dimension)
		Catch
			Return False
		End Try

		N = pIntersection.PointCount
		If N = 0 Then Return False

		pCurrPt = pIntersection.Point(0)
		fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt)
		fMinDist = fDist
		fMaxDist = fDist

		For I = 1 To N - 1
			pCurrPt = pIntersection.Point(I)
			fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt)
			If fDist < fMinDist Then fMinDist = fDist
			If fDist > fMaxDist Then fMaxDist = fDist
		Next

		PostDist = fMinDist
		PriorDist = fMaxDist
		Return True
	End Function

	Public Sub SetComboDroppedWidth(ByRef cb As System.Windows.Forms.ComboBox, ByVal cbWidth As Integer)
		SendMessage(cb.Handle.ToInt32, CBS_DROPDOWNLIST, 0, 0)
		SendMessage(cb.Handle.ToInt32, CB_SETDROPPEDWIDTH, cbWidth, 0)
	End Sub

	Public Function GetUnicalObstales(ByRef obstacleList As ObstacleContainer) As Dictionary(Of Guid, Integer)
		Dim dictionary As IDictionary(Of Guid, Integer) = New Dictionary(Of Guid, Integer)
		Dim N As Integer = UBound(obstacleList.Parts)
		Dim I As Integer
		Dim obstacleID As Guid

		For I = 0 To N
			obstacleID = obstacleList.Obstacles(obstacleList.Parts(I).Owner).Identifier
			If Not dictionary.ContainsKey(obstacleID) Then
				dictionary.Add(obstacleID, I)
			End If
		Next

		Return dictionary
	End Function

	Public Sub SortListView(ByVal columnIndex As Integer, ByVal lvSorter As ListViewColumnSorter, ByVal listview As ListView)
		If (columnIndex <> lvSorter.ColumntToSort) Then
			listview.Columns(lvSorter.ColumntToSort).ImageIndex = 2
			lvSorter.ColumntToSort = columnIndex

			lvSorter.Order = SortOrder.Ascending
			listview.Columns(lvSorter.ColumntToSort).ImageIndex = 0
		ElseIf (lvSorter.Order = SortOrder.Ascending) Then
			lvSorter.Order = SortOrder.Descending
			listview.Columns(lvSorter.ColumntToSort).ImageIndex = 1
		Else
			lvSorter.Order = SortOrder.Ascending
			listview.Columns(lvSorter.ColumntToSort).ImageIndex = 0
		End If

		listview.Sort()
	End Sub

	'Public Sub SaveTrackPoints(ByVal FileName As String, ByVal pTransition As ProcedureTransition)
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim N As Integer
	'	Dim M As Integer
	'	Dim FileNum As Integer
	'	Dim phaseNames() As String = {"RWY", "COMMON", "EN_ROUTE", "APPROACH", "FINAL", "MISSED", "MISSED_P", "MISSED_S", "ENGINE_OUT", "OTHER"}

	'	Dim pTrajectory As Curve
	'	Dim pLineStringSegment As LineString
	'	Dim pPoint As ElevatedPoint

	'	FileName = FileName + "_" + phaseNames(pTransition.type) + "TransitionPoints.txt"

	'	FileNum = FreeFile()
	'	FileOpen(FileNum, FileName, OpenMode.Output)

	'	pTrajectory = pTransition.trajectory
	'	N = pTrajectory.CurveSegmentList.Count

	'	'		Dim K As Long
	'	'		Dim L As Long

	'	For I = 0 To N - 1
	'		pCurveSegment = pTrajectory.CurveSegmentList(I)
	'		pLineStringSegment = pCurveSegment.lineStringSegment()
	'		M = pLineStringSegment.pointList.Count()

	'		For J = 0 To M - 1
	'			'pPart = pPolyline.GetItem(J)
	'			'L = pPart.Count
	'			'For K = 0 To L - 1

	'			pPoint = pCurveSegment.lineStringSegment.pointList(J)
	'			PrintLine(FileNum, CStr(pPoint.x) + " " + CStr(pPoint.y))

	'			'Next K
	'		Next J
	'	Next I

	'	FileClose(FileNum)
	'End Sub

	Public Sub AddObstacles(ostacles As ObstacleContainer, mUomVDistance As UomDistanceVertical, ByRef pPrimProtectedArea As ObstacleAssessmentArea, ByRef pSecProtectedArea As ObstacleAssessmentArea)
		Dim I As Integer
		Dim J As Integer

		Dim pDistance As ValDistance
		Dim pDistanceVertical As ValDistanceVertical
		'added by agshin
		'sort by ReqH

		Functions.Sort(ostacles, 2)
		Dim saveCount As Int32 = Math.Min(ostacles.Obstacles.Length, 5)

		For I = 0 To ostacles.Obstacles.Length - 1
			If (saveCount = 0) Then Exit For
			Dim obs As Obstruction = New Obstruction
			obs.VerticalStructureObstruction = New FeatureRef(ostacles.Obstacles(I).Identifier)

			Dim MinimumAltitude As Double = 0
			Dim RequiredClearance As Double = 0
			Dim isPrimary As Integer = 0

			For J = 0 To ostacles.Obstacles(I).PartsNum - 1
				MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts(ostacles.Obstacles(I).Parts(J)).ReqH)
				RequiredClearance = Math.Max(RequiredClearance, ostacles.Parts(ostacles.Obstacles(I).Parts(J)).MOC)
				If ((ostacles.Parts(ostacles.Obstacles(I).Parts(J)).Flags And 1) = 1) Then
					isPrimary = isPrimary Or 1
				Else
					isPrimary = isPrimary Or 2
				End If
			Next

			'ReqH
			pDistanceVertical = New ValDistanceVertical()
			pDistanceVertical.Uom = mUomVDistance
			pDistanceVertical.Value = ConvertHeight(MinimumAltitude, eRoundMode.NEAREST)
			obs.MinimumAltitude = pDistanceVertical

			'MOC
			pDistance = New ValDistance()
			pDistance.Uom = UomDistance.M
			pDistance.Value = RequiredClearance
			obs.RequiredClearance = pDistance

			If ((isPrimary And 1) <> 0) Then
				pPrimProtectedArea.SignificantObstacle.Add(obs)
			End If

			If ((isPrimary And 2) <> 0) Then
				pSecProtectedArea.SignificantObstacle.Add(obs)
			End If

			saveCount -= 1
		Next
	End Sub

	Public Function GetDesignatedPointDescription(point As IPoint) As String
		Dim designatedPointsFeature As List(Of Feature) = pObjectDir.GetFeatureList(FeatureType.DesignatedPoint)
		Dim aixmPoint As AixmPoint = ESRIPointToAixmPoint(ToGeo(point))

		For Each designatedPointFeature As DesignatedPoint In designatedPointsFeature
			If aixmPoint.Geo.Equals2D(designatedPointFeature.Location.Geo) Then
				Return designatedPointFeature.Name
			End If
		Next

		Return Nothing
	End Function
End Module