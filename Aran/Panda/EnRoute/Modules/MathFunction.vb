Option Strict Off
Option Explicit On

Imports ArcGeometry = ESRI.ArcGIS.Geometry

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module MathFunction
	Function LocalToPrj(ByVal center As ArcGeometry.IPoint, ByVal dirInDeg As Double, ByVal X As Double, Optional ByVal Y As Double = 0.0) As ArcGeometry.Point
		Dim dirInRadian As Double
		Dim SinA As Double
		Dim CosA As Double
		Dim Xnew As Double
		Dim Ynew As Double
		Dim result As ArcGeometry.Point

		dirInRadian = GlobalVars.DegToRadValue * dirInDeg
		SinA = Math.Sin(dirInRadian)
		CosA = Math.Cos(dirInRadian)
		Xnew = center.X + X * CosA - Y * SinA
		Ynew = center.Y + X * SinA + Y * CosA

		result = New ArcGeometry.Point
		result.PutCoords(Xnew, Ynew)
		Return result
	End Function

	Sub PrjToLocal(ByVal center As ArcGeometry.IPoint, ByVal dirInDeg As Double, ByVal ptPrj As ArcGeometry.IPoint, ByRef resX As Double, ByRef resY As Double)
		Dim dirInRadian As Double = GlobalVars.DegToRadValue * dirInDeg

		Dim SinA As Double = Math.Sin(dirInRadian)
		Dim CosA As Double = Math.Cos(dirInRadian)
		Dim dX As Double = ptPrj.X - center.X
		Dim dY As Double = ptPrj.Y - center.Y

		resX = dX * CosA + dY * SinA
		resY = -dX * SinA + dY * CosA
	End Sub

	'Function RealMode(ByVal X As Double, ByVal base As Short) As Double
	'	Dim dX As Double
	'	Dim N As Short

	'	N = Fix(X)
	'	dX = X - N
	'	X = N Mod base + dX

	'	If (X < 0.0) Then X = X + base
	'	If (X > base / 2) Then X = X - base

	'	Return X
	'End Function

	Public Function SideDef(ByVal PtInLine As ArcGeometry.IPoint, ByVal LineAngle As Double, ByVal PtTest As ArcGeometry.IPoint) As Integer
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

		If dAngle > 0.0 Then
			If (dAngle < 180.0) Then '- degEps Sag
				Return eSide.Right
			ElseIf (dAngle > 180.0) Then  '+ degEps Sol
				Return eSide.Left
			End If
		End If

		Return eSide.OnLine
	End Function

	Public Function SideFrom2Angle(ByVal Angle0 As Double, ByVal Angle1 As Double) As Integer
		Dim dAngle As Double

		dAngle = SubtractAngles(Angle0, Angle1)
		If (180.0 - dAngle < degEps) Or (dAngle < degEps) Then Return 0

		dAngle = Modulus(Angle1 - Angle0, 360.0)
		If (dAngle < 180.0) Then Return 1

		Return -1
	End Function

	Public Function IAS2TAS(ByRef IAS As Double, ByRef H As Double, ByRef dT As Double) As Double
		Return IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * H) / ((288.0 - 0.006496 * H) ^ 2.628)
	End Function

	Public Function Bank2Radius(ByRef Bank As Double, ByRef V As Double) As Double
		Dim Rv As Double

		Rv = 6.355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)

		If (Rv > 0.003) Then Rv = 0.003
		If (Rv > 0.0) Then Return V / (20.0 * PI * Rv)

		Return -1.0
	End Function

	Public Function Radius2Bank(ByRef R As Double, ByRef V As Double) As Double
		If (R > 0.0) Then Return RadToDeg(System.Math.Atan(V * V / (20.0 * R * 6.355)))
		Return -1
	End Function

	'Public Function Quadric(ByRef A As Double, ByRef B As Double, ByRef C As Double, ByRef X0 As Double, ByRef X1 As Double) As Integer
	'	Dim D As Double
	'	Dim fTmp As Double

	'	D = B * B - 4 * A * C
	'	If D < 0.0 Then
	'		Quadric = 0
	'	ElseIf (D = 0.0) Or (A = 0.0) Then
	'		Quadric = 1
	'		If A = 0.0 Then
	'			X0 = -C / B
	'		Else
	'			X0 = -0.5 * B / A
	'		End If
	'	Else
	'		Quadric = 2
	'		fTmp = 0.5 / A
	'		If fTmp > 0 Then
	'			X0 = (-B - System.Math.Sqrt(D)) * fTmp
	'			X1 = (-B + System.Math.Sqrt(D)) * fTmp
	'		Else
	'			X0 = (-B + System.Math.Sqrt(D)) * fTmp
	'			X1 = (-B - System.Math.Sqrt(D)) * fTmp
	'		End If
	'	End If
	'End Function

	'Function ArcSin(ByVal X As Double) As Double
	'	If System.Math.Abs(X) >= 1.0 Then
	'		ArcSin = 0.5 * System.Math.Sign(X) * PI
	'	Else
	'		ArcSin = System.Math.Atan(X / System.Math.Sqrt(1.0 - X * X))
	'	End If
	'End Function

	'Function ArcCos(ByVal X As Double) As Double
	'	If System.Math.Abs(X) >= 1.0 Then
	'		ArcCos = 0.0
	'	Else
	'		ArcCos = System.Math.Atan(-X / System.Math.Sqrt(1.0 - X * X)) + 0.5 * PI
	'	End If
	'End Function

	Function SubtractAngles(ByVal X As Double, ByVal Y As Double) As Double
		X = Modulus(X, 360.0)
		Y = Modulus(Y, 360.0)
		SubtractAngles = Modulus(X - Y, 360.0)
		If SubtractAngles > 180.0 Then SubtractAngles = 360.0 - SubtractAngles
	End Function

	'Function SubtractAnglesWithSign(ByVal StRad As Double, ByVal EndRad As Double, ByRef Turn As Integer) As Double
	'	SubtractAnglesWithSign = Modulus((EndRad - StRad) * Turn, 360.0)
	'	If SubtractAnglesWithSign > 180.0 Then
	'		SubtractAnglesWithSign = SubtractAnglesWithSign - 360.0
	'	End If
	'End Function

	Function AngleInSector(ByVal Angle As Double, ByVal X As Double, ByVal Y As Double) As Boolean
		X = Modulus(X)
		Y = Modulus(Y)
		Angle = Modulus(Angle)

		If (X > Y) Then
			If ((Angle >= X) Or (Angle <= Y)) Then Return True
		Else
			If ((Angle >= X) And (Angle <= Y)) Then Return True
		End If

		Return False
	End Function

	Function AnglesSideDef(ByVal X As Double, ByVal Y As Double) As Integer
		Dim z As Double
		z = Modulus(X - Y, 360.0)
		If z = 0.0 Then
			AnglesSideDef = 0
		ElseIf z > 180.0 Then
			AnglesSideDef = -1
		ElseIf z < 180.0 Then
			AnglesSideDef = 1
		Else
			AnglesSideDef = 2
		End If
	End Function

	Function DegToRad(ByVal X As Double) As Double
		DegToRad = X * DegToRadValue
	End Function

	Function RadToDeg(ByVal X As Double) As Double
		RadToDeg = X * RadToDegValue
	End Function

	'Function CeilPDG(ByVal X As Double) As Double
	'	CeilPDG = System.Math.Round(X + 0.004999999, 4)		'CeilPDG = -Int(-X * 1000) * 0.001
	'End Function

	'Function CalcZeroDTNAH(ByRef dTNA As Double, ByRef PDGznr As Double, ByRef PDGzr As Double, ByRef Alpha As Double, ByRef Beta As Double, ByRef dr As Double) As SquareSolutionArea
	'	Dim Res1IsSolution As Boolean
	'	Dim Res2IsSolution As Boolean
	'	Dim balans0 As Double
	'	Dim balans1 As Double

	'	Dim A As Double
	'	Dim B As Double
	'	Dim C As Double
	'	Dim D As Double
	'	Dim M As Double
	'	Dim N As Double

	'	Dim Cos30 As Double
	'	Dim CosAlpha As Double
	'	Dim SinAlpha As Double
	'	Dim X0 As Double
	'	Dim X1 As Double
	'	Dim X2 As Double
	'	Dim Y1 As Double
	'	Dim y2 As Double
	'	Dim Epsilon As Double

	'	CalcZeroDTNAH.Solutions = 0
	'	Epsilon = 0.0001
	'	Cos30 = System.Math.Cos(DegToRad(Beta))

	'	CosAlpha = System.Math.Cos(DegToRad(Alpha))
	'	SinAlpha = System.Math.Sin(DegToRad(Alpha))
	'	X0 = dr * CosAlpha * Cos30

	'	N = dTNA
	'	M = (dTNA + PDGzr * dr) / PDGzr

	'	A = (PDGznr * Cos30) ^ 2.0 - PDGzr ^ 2.0
	'	B = 2.0 * ((PDGzr - PDGznr * Cos30 / CosAlpha) * PDGzr * X0 - PDGznr * N * Cos30 ^ 2)
	'	C = (N * Cos30) ^ 2 + 2.0 * N * PDGzr * X0 * Cos30 / CosAlpha
	'	D = B * B - 4.0 * A * C

	'	If D < 0 Then
	'		CalcZeroDTNAH.Second = 0.0
	'		CalcZeroDTNAH.Solutions = 0
	'		Exit Function
	'	End If

	'	X1 = (-B - System.Math.Sqrt(D)) / (2 * A)
	'	X2 = (-B + System.Math.Sqrt(D)) / (2 * A)

	'	If X1 > X2 Then
	'		Y1 = X1
	'		X1 = X2
	'		X2 = Y1
	'	End If

	'	If (X1 > X0) Then
	'		'    y1 = dR * SinAlpha
	'		'    balans0 = N - x0 * PDGznr - (y1 - dR) * PDGzr
	'		'    x1 = x0
	'		Res1IsSolution = False
	'	Else
	'		Y1 = System.Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X1 + X1 * X1 / (Cos30 * Cos30))
	'		balans0 = N - X1 * PDGznr - (Y1 - dr) * PDGzr
	'		Res1IsSolution = balans0 >= -Epsilon

	'		If balans0 > Epsilon Then X1 = X1 + balans0 / PDGznr
	'	End If

	'	If (X2 > X0) Then
	'		X2 = X0
	'		y2 = dr * SinAlpha
	'	Else
	'		y2 = System.Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X2 + X2 * X2 / (Cos30 * Cos30))
	'	End If

	'	balans1 = N - X2 * PDGznr - (y2 - dr) * PDGzr

	'	Res2IsSolution = balans1 >= -Epsilon

	'	If (balans1 > Epsilon) Then X2 = X2 + balans1 / PDGznr

	'	'Dim balTemp As Double
	'	'balTemp = N - x2 * PDGznr - (y2 - dR) * PDGzr

	'	If Res1IsSolution And Res2IsSolution Then
	'		If X1 >= 0 Then
	'			CalcZeroDTNAH.First = X1
	'			CalcZeroDTNAH.Solutions = 1
	'		End If
	'		CalcZeroDTNAH.Second = X2
	'		CalcZeroDTNAH.Solutions = CalcZeroDTNAH.Solutions Or 2
	'	ElseIf Res1IsSolution Then
	'		CalcZeroDTNAH.Second = X1
	'		CalcZeroDTNAH.Solutions = 2
	'	ElseIf Res2IsSolution Then
	'		CalcZeroDTNAH.Second = X2
	'		CalcZeroDTNAH.Solutions = 2
	'	Else
	'		CalcZeroDTNAH.Second = 0.0
	'		CalcZeroDTNAH.Solutions = 0
	'	End If
	'End Function

End Module