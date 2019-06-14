Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.Geometry

Module Shablons
	
	Sub ChangeSpiralStartParam(ByVal E As Double, ByVal StR0 As Double, ByVal StRadial As Double, ByRef NewStR0 As Double, ByRef NewStRadial As Double, ByVal Turn As Integer, ByVal turnChange As Integer)
		Dim TurnAng As Double
		
		TurnAng = Modulus((NewStRadial - StRadial) * turnChange, 360.0)

		If TurnAng >= 180.0 Then
			turnChange = -turnChange
			TurnAng = Modulus((NewStRadial - StRadial) * turnChange, 360.0)
		End If

		NewStR0 = StR0 + E * TurnAng * Turn * turnChange

		If NewStR0 < 0.0 Then
			TurnAng = StR0 / E
			NewStR0 = 0.0
		End If
		NewStRadial = Modulus(TurnAng * turnChange + StRadial, 360.0)
	End Sub

	'Public Function SpiralTouchToPoint(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef E As Double, ByRef r0 As Double, ByRef AztStRad As Double, ByRef Turn As Integer, ByRef pPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef SpiralIntercept As Integer, ByRef SolPnt As ESRI.ArcGIS.Geometry.IPoint) As Integer
	'	Dim DistToPnt As Double
	'	Dim AztToPnt As Double
	'	Dim SpAngle As Double
	'	Dim fTmp As Double
	'	Dim DegE As Double
	'	Dim dPhi As Double
	'	Dim Xsp As Double
	'	Dim Ysp As Double
	'	Dim Phi As Double
	'	Dim R As Double
	'	Dim I As Integer
	'	'if SpiralIntercept = 1 Point to spiral else spiral to Point

	'	AztToPnt = ReturnAngleInDegrees(ptCnt, pPnt)
	'	DistToPnt = ReturnDistanceInMeters(ptCnt, pPnt)
	'	DegE = RadToDeg(E)

	'	'DrawPoint pPnt, 0

	'	Phi = Modulus((AztToPnt - AztStRad) * Turn, 360.0)

	'	If (Phi > 180.0) And (r0 = 0.0) Then
	'		Phi = Phi - 360.0
	'	End If

	'	R = r0 + E * Phi

	'	If System.Math.Abs(R - DistToPnt) < distEps Then
	'		SpiralTouchToPoint = 1
	'		SolPnt = pPnt
	'		Exit Function
	'	ElseIf R > DistToPnt Then
	'		SpiralTouchToPoint = 0
	'		Exit Function
	'	End If

	'	fTmp = Modulus(AztToPnt + 90.0 * (1 + SpiralIntercept), 360.0)
	'	'Set ptSp = New Point

	'	For I = 0 To 30
	'		Phi = fTmp
	'		SpAngle = SpiralTouchAngle(r0, E, AztStRad + 90.0 * Turn, Phi, Turn)
	'		R = r0 + E * SpAngle
	'		fTmp = Modulus(AztStRad + SpAngle * Turn, 360.0)
	'		'Set ptSp = PointAlongPlane(PtCnt, fTmp, R)
	'		'DrawPoint ptSp, 255

	'		fTmp = DegToRad(fTmp)
	'		Xsp = ptCnt.X + R * System.Math.Cos(fTmp)
	'		Ysp = ptCnt.Y + R * System.Math.Sin(fTmp)
	'		fTmp = RadToDeg(ATan2(pPnt.Y - Ysp, pPnt.X - Xsp))
	'		fTmp = Modulus(fTmp + 90.0 * (1 + SpiralIntercept), 360.0)

	'		dPhi = SubtractAngles(Phi, fTmp)

	'		'ptSp.PutCoords Xsp, Ysp
	'		'DrawPoint ptSp, RGB(0, 0, 255)

	'		If dPhi < degEps Then
	'			SolPnt = New ESRI.ArcGIS.Geometry.Point
	'			SolPnt.PutCoords(Xsp, Ysp)
	'			SpiralTouchToPoint = 1
	'			Exit Function
	'		End If
	'	Next I

	'	SpiralTouchToPoint = 0
	'End Function

	Public Function SpiralTouchToFix(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef E As Double, ByRef r0 As Double, ByRef AztStRad As Double, ByRef Turn As Integer, ByRef FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef SpiralIntercept As Integer, ByRef Axis As Double) As Double
		Dim I As Integer
		Dim J As Integer
		Dim R As Double
		Dim F As Double
		Dim f_ As Double
		Dim Xa As Double
		Dim Ya As Double
		Dim Xsp As Double
		Dim Ysp As Double
		Dim Phi As Double
		Dim Phi0 As Double
		Dim fTmp As Double
		Dim SinA As Double
		Dim CosA As Double
		Dim dPhi As Double
		Dim SpAngle As Double
		Dim AztToFix As Double
		Dim DistToFix As Double

		'if SpiralIntercept = 1 FIX to spiral else spiral to FIX
		AztToFix = ReturnAngleInDegrees(ptCnt, FixPnt)
		DistToFix = ReturnDistanceInMeters(ptCnt, FixPnt)

		'SpiralTouchToFix = 0.0
		'Phi0 = Modulus(AztToFix, 360.0)

		'Phi0 = Modulus(AztToFix + Turn * SpiralIntercept * 90.0, 360.0)
		fTmp = Modulus((AztToFix - AztStRad) * Turn, 360.0)

		If (fTmp > 180.0) And (r0 = 0.0) Then
			fTmp = fTmp - 360.0
		End If

		R = r0 + E * fTmp '* turn
		If System.Math.Abs(R - DistToFix) < distEps Then
			SpiralTouchToFix = AztToFix
			Exit Function
		End If

		If R < DistToFix Then
			Phi0 = Modulus(AztToFix + 90.0 * (1 + SpiralIntercept), 360.0)

			For I = 0 To 30
				Phi = Phi0
				SpAngle = SpiralTouchAngle(r0, E, AztStRad + 90.0 * Turn, Phi, Turn)
				R = r0 + E * SpAngle

				'Set SolPnt = PointAlongPlane(PtCnt, Phi, R)
				'DrawPoint SolPnt, 255

				SpiralTouchToFix = Modulus(AztStRad + SpAngle * Turn, 360.0)

				fTmp = DegToRad(SpiralTouchToFix)
				Xsp = ptCnt.X + R * System.Math.Cos(fTmp)
				Ysp = ptCnt.Y + R * System.Math.Sin(fTmp)
				fTmp = RadToDeg(Math.Atan2(FixPnt.Y - Ysp, FixPnt.X - Xsp))

				Phi0 = Modulus(fTmp + 90.0 * (1 + SpiralIntercept), 360.0)

				'SolPnt.PutCoords Xsp, Ysp
				'DrawPoint SolPnt, RGB(0, 0, 255)

				dPhi = SubtractAngles(Phi, Phi0)

				If dPhi < degEps Then
					'            SpiralTouchToFix = fTmp
					'            Set SolPnt = New Point
					'            SolPnt.PutCoords Xsp, Ysp
					'            SpiralTouchToPoint = 1
					Exit Function
				End If
			Next I
		Else
			CosA = System.Math.Cos(DegToRad(Axis + 90.0 * Turn))
			SinA = System.Math.Sin(DegToRad(Axis + 90.0 * Turn))
			Xa = DistToFix * System.Math.Cos(DegToRad(AztToFix))
			Ya = DistToFix * System.Math.Sin(DegToRad(AztToFix))
			Phi0 = AztToFix
			For J = 0 To 20
				fTmp = Modulus((Phi0 - AztStRad) * Turn, 360.0)
				R = r0 + E * fTmp '* turn
				F = R * (System.Math.Sin(DegToRad(Phi0)) * CosA - System.Math.Cos(DegToRad(Phi0)) * SinA) + Xa * SinA - Ya * CosA
				f_ = RadToDeg(E) * Turn * (System.Math.Sin(DegToRad(Phi0)) * CosA - System.Math.Cos(DegToRad(Phi0)) * SinA) + R * (System.Math.Cos(DegToRad(Phi0)) * CosA + System.Math.Sin(DegToRad(Phi0)) * SinA)
				Phi = Phi0 - RadToDeg(F / f_)
				If System.Math.Abs(System.Math.Sin(DegToRad(Phi - Phi0))) < 0.001 Then
					SpiralTouchToFix = Modulus(Phi, 360.0)
					Exit Function
				Else
					Phi0 = Phi
				End If
			Next J
		End If

		SpiralTouchToFix = Modulus(Phi, 360.0)
	End Function

	Sub CreateSpiralBy2Radial(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef r0 As Double, ByRef AztStRad As Double, ByRef AztEndRad As Double, ByRef E As Double, ByRef Turn As Integer, ByRef pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection)
		Dim dAlpha As Double
		Dim TurnAng As Double
		Dim R As Double
		Dim N As Integer
		Dim I As Integer
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint

		TurnAng = Modulus((AztEndRad - AztStRad) * Turn, 360.0)
		dAlpha = 1.0
		N = TurnAng / dAlpha
		If N < 5 Then
			N = 5
		ElseIf N < 10 Then
			N = 10
		End If

		dAlpha = TurnAng / N
		ptCur = New ESRI.ArcGIS.Geometry.Point
		For I = 0 To N
			R = r0 + (dAlpha * E * I) ' + dphi0 * coef
			ptCur = PointAlongPlane(ptCnt, AztStRad + (I * dAlpha) * Turn, R)
			pPointCollection.AddPoint(ptCur)
		Next I
	End Sub

	'Public Function TouchTo2Spiral(PtCnt1 As IPoint, r10 As Double, e1 As Double, AztSt1 As Double, _
	''                                PtCnt2 As IPoint, r20 As Double, E2 As Double, AztSt2 As Double, _
	''                                Turn As Long, CntAngle1 As Double, CntAngle2 As Double) As Long
	'Dim phi1 As Double
	'Dim phi2 As Double
	'Dim phi10 As Double
	'Dim phi20 As Double
	'Dim AztR1E As Double
	'Dim AztR2E As Double
	'Dim AztO1O2 As Double
	'Dim DistO1O2 As Double
	'Dim R1 As Double
	'Dim R2 As Double
	'Dim f As Double
	'Dim f_ As Double
	'Dim I As Long
	'Dim J As Long
	'Dim fTmp As Double
	'Dim pt1 As IPoint
	'Dim pt2 As IPoint
	'Dim AztTouch As Double
	'Dim AztTouch1 As Double
	'Dim TurnAngle As Double
	'
	'AztO1O2 = ReturnAngleInDegrees(PtCnt1, PtCnt2)
	'DistO1O2 = ReturnDistanceInMeters(PtCnt1, PtCnt2)
	'phi10 = Modulus(AztO1O2 - 90.0 * Turn, 360.0)
	'For J = 0 To 20
	'    fTmp = Modulus((phi10 - AztSt1) * Turn, 360.0)
	'    R1 = r10 + e1 * fTmp
	'    AztR1E = RadToDeg(Atn(RadToDeg(e1 * Turn) / R1))
	'    phi20 = phi10
	'    For I = 0 To 20
	'        fTmp = Modulus((phi20 - AztSt2) * Turn, 360.0)
	'        R2 = r20 + E2 * fTmp
	'        AztR2E = RadToDeg(Atn(RadToDeg(E2 * Turn) / R2))
	'        f = phi20 - phi10 + AztR1E - AztR2E
	'        fTmp = RadToDeg(E2) * RadToDeg(E2)
	'        f_ = 1.0 + fTmp / (R2 * R2 + fTmp)
	'        phi2 = phi20 - f / f_
	'        If Abs(Sin(DegToRad(f / f_))) < degEps Then
	'            Exit For
	'        Else
	'            phi20 = phi2
	'        End If
	'    Next I
	'    phi2 = Modulus(phi2, 360.0)
	'    fTmp = (R1 * Cos(DegToRad(AztR1E)) - R2 * Cos(DegToRad(AztR2E))) / DistO1O2
	'    If Abs(fTmp) > 1 Then
	'        TouchTo2Spiral = 0
	'        Exit Function
	'    End If
	'    f = phi10 - AztO1O2 - AztR1E + Turn * RadToDeg(ArcCos(fTmp))
	'    f_ = 1.0 - Turn * RadToDeg(e1 * Cos(DegToRad(AztR1E)) - E2 * Cos(DegToRad(AztR2E))) / DistO1O2
	'    phi1 = phi10 - f / f_
	'    phi1 = Modulus(phi1, 360.0)
	'
	'    If Abs(Sin(DegToRad(f / f_))) < degEps Then
	'        CntAngle1 = phi1
	'        CntAngle2 = phi2
	'        TouchTo2Spiral = 1
	'        Exit Function
	'    Else
	'        phi10 = phi1
	'    End If
	'Next J
	'End Function

	Public Function TouchTo2Spiral(ByRef PtCnt1 As ESRI.ArcGIS.Geometry.IPoint, ByVal r10 As Double, ByVal E1 As Double, ByVal AztSt1 As Double, ByRef PtCnt2 As ESRI.ArcGIS.Geometry.IPoint, ByVal r20 As Double, ByVal E2 As Double, ByVal AztSt2 As Double, ByVal Turn As Integer, ByRef CntAngle1 As Double, ByRef CntAngle2 As Double) As Integer
		Dim bOutOfSpiral As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer

		Dim F As Double
		Dim F_ As Double
		Dim R1 As Double
		Dim R2 As Double
		Dim fTmp As Double
		Dim phi1 As Double
		Dim phi2 As Double
		Dim phi10 As Double
		Dim phi20 As Double
		Dim AztR1E As Double
		Dim AztR2E As Double
		Dim AztO1O2 As Double
		Dim Distance As Double
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

		'Set sp0 = New Polyline
		'Set sp1 = New Polyline

		'CreateSpiralBy2Radial PtCnt1, r10, AztSt1, AztSt1 + 359.99 * Turn, E1, Turn, sp0
		'CreateSpiralBy2Radial PtCnt2, r20, AztSt2, AztSt2 + 359.99 * Turn, E2, Turn, sp1

		AztO1O2 = ReturnAngleInDegrees(PtCnt1, PtCnt2)
		'DistO1O2 = ReturnDistanceInMeters(PtCnt1, PtCnt2)

		bOutOfSpiral = False

		For K = 0 To 10
			phi10 = Modulus(AztO1O2 - (90.0 + 10.0 * K) * Turn, 360.0)
			fTmp = Modulus((phi10 - AztSt1) * Turn, 360.0)
			R1 = r10 + E1 * fTmp
			pt1 = PointAlongPlane(PtCnt1, phi10, R1)

			phi20 = ReturnAngleInDegrees(PtCnt2, pt1)
			Distance = ReturnDistanceInMeters(PtCnt2, pt1)
			fTmp = Modulus((phi20 - AztSt2) * Turn, 360.0)
			R2 = r20 + E2 * fTmp

			If R2 < Distance Then
				phi20 = phi10
				bOutOfSpiral = True
				Exit For
			End If

		Next K

		If Not bOutOfSpiral Then
			TouchTo2Spiral = 0
			Exit Function
		End If

		For J = 0 To 30
			fTmp = Modulus((phi10 - AztSt1) * Turn, 360.0)
			R1 = r10 + E1 * fTmp
			AztR1E = RadToDeg(System.Math.Atan(RadToDeg(E1 * Turn) / R1))

			For I = 0 To 20
				fTmp = Modulus((phi20 - AztSt2) * Turn, 360.0)
				R2 = r20 + E2 * fTmp
				AztR2E = RadToDeg(System.Math.Atan(RadToDeg(E2 * Turn) / R2))
				F = phi20 - phi10 + AztR1E - AztR2E
				fTmp = RadToDeg(E2) * RadToDeg(E2)
				f_ = 1.0 + fTmp / (R2 * R2 + fTmp)
				phi2 = phi20 - F / f_

				If System.Math.Abs(System.Math.Sin(DegToRad(F / f_))) < degEps Then
					Exit For
				Else
					phi20 = phi2
				End If
			Next I

			pt1 = PointAlongPlane(PtCnt1, phi1, R1)
			pt2 = PointAlongPlane(PtCnt2, phi2, R2)
			fTmp = ReturnAngleInDegrees(pt1, pt2)
			fTmp = SubtractAnglesWithSign(phi1 + (90.0 * Turn - AztR1E), fTmp, Turn)
			phi1 = phi10 + fTmp * Turn
			If System.Math.Abs(fTmp) < (degEps * 50.0) Then
				CntAngle1 = Modulus(phi1, 360.0)
				CntAngle2 = Modulus(phi2, 360.0)
				TouchTo2Spiral = 1
				Exit Function
			Else
				phi10 = phi1
			End If
		Next J

		TouchTo2Spiral = 0

	End Function

	'Public Function TouchTo2Spiral1(ByRef PtCnt1 As ESRI.ArcGIS.Geometry.IPoint, ByRef r10 As Double, ByRef E1 As Double, ByRef AztSt1 As Double, ByRef PtCnt2 As ESRI.ArcGIS.Geometry.IPoint, ByRef r20 As Double, ByRef E2 As Double, ByRef AztSt2 As Double, ByRef Turn As Integer, ByRef CntAngle1 As Double, ByRef CntAngle2 As Double) As Integer
	'	Dim sp0 As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim sp1 As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim InterPoints As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim InterPoints0 As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim CutLine As ESRI.ArcGIS.Geometry.IPointCollection

	'	Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'	Dim tmpPoint As ESRI.ArcGIS.Geometry.IPoint
	'	Dim StRad As Double
	'	Dim Distance As Double
	'	Dim StartPoint As ESRI.ArcGIS.Geometry.IPoint

	'	Dim Rad0 As Double
	'	Dim Rad1 As Double
	'	Dim pPoint0 As ESRI.ArcGIS.Geometry.IPoint

	'	Dim phi1 As Double
	'	Dim phi2 As Double
	'	Dim phi10 As Double
	'	Dim phi20 As Double
	'	Dim AztR1E As Double
	'	Dim AztR2E As Double
	'	Dim AztO1O2 As Double
	'	Dim DistO1O2 As Double
	'	Dim R1 As Double
	'	Dim R2 As Double
	'	Dim F As Double
	'	Dim f_ As Double
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim fTmp As Double
	'	Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt2 As ESRI.ArcGIS.Geometry.IPoint

	'	sp0 = New ESRI.ArcGIS.Geometry.Polyline
	'	sp1 = New ESRI.ArcGIS.Geometry.Polyline

	'	CreateSpiralBy2Radial(PtCnt1, r10, AztSt1, AztSt1 + 359.99 * Turn, E1, Turn, sp0)
	'	CreateSpiralBy2Radial(PtCnt2, r20, AztSt2, AztSt2 + 359.99 * Turn, E2, Turn, sp1)

	'	pTopo = sp0
	'	InterPoints = pTopo.Intersect(sp1, esriGeometryDimension.esriGeometry0Dimension)
	'	StartPoint = Nothing

	'	'DrawPolyLine sp0, 255
	'	'DrawPolyLine sp1, 0
	'	'
	'	'DrawPoint PtCnt1, 255
	'	'DrawPoint PtCnt2, 0

	'	If InterPoints.PointCount >= 2 Then	'hall var
	'		For I = 0 To InterPoints.PointCount - 1
	'			StRad = ReturnAngleInDegrees(PtCnt2, InterPoints.Point(I)) - Turn
	'			Rad1 = r20 + E2 * Modulus(Turn * (StRad - AztSt2), 360.0)
	'			tmpPoint = PointAlongPlane(PtCnt2, StRad, Rad1)
	'			'    DrawPoint tmpPoint, 0

	'			Distance = ReturnDistanceInMeters(PtCnt1, tmpPoint)
	'			StRad = ReturnAngleInDegrees(PtCnt1, tmpPoint)
	'			Rad0 = r10 + E1 * Modulus(Turn * (StRad - AztSt1), 360.0)
	'			If Rad0 >= Distance Then 'hall var
	'				StRad = ReturnAngleInDegrees(PtCnt1, InterPoints.Point(I)) - Turn
	'				Rad0 = r10 + E1 * Modulus(Turn * (StRad - AztSt1), 360.0)
	'				StartPoint = PointAlongPlane(PtCnt1, StRad, Rad0)
	'				Exit For
	'			End If
	'		Next I

	'		If StartPoint Is Nothing Then
	'			Return 0
	'		End If

	'		'    DrawPoint StartPoint, 255
	'		'    If Rad0 < Distance Then    'hall 1
	'		'        StRad = ReturnAngleInDegrees(PtCnt1, InterPoints.Point(1)) - Turn
	'		'        Rad0 = r10 + e1 * Modulus(Turn * (StRad - AztSt1), 360.0)
	'		'        Set StartPoint = PointAlongPlane(PtCnt1, StRad, Rad0)
	'		'    Else                        'hall 0
	'		'        StRad = ReturnAngleInDegrees(PtCnt1, InterPoints.Point(0)) - Turn
	'		'        Rad0 = r10 + e1 * Modulus(Turn * (StRad - AztSt1), 360.0)
	'		'        Set StartPoint = PointAlongPlane(PtCnt1, StRad, Rad0)
	'		'    End If
	'		'    DrawPoint StartPoint, 0
	'		'    Set StartPoint = InterPoints0.Point(0)
	'	ElseIf InterPoints.PointCount <= 1 Then	 'ola biler
	'		If InterPoints.PointCount > 0 Then 'ola biler
	'			StRad = ReturnAngleInDegrees(PtCnt2, InterPoints.Point(0)) - Turn
	'			Distance = ReturnDistanceInMeters(PtCnt2, InterPoints.Point(0)) * 2.0
	'			CutLine = New ESRI.ArcGIS.Geometry.Polyline
	'			CutLine.AddPoint(PtCnt2)
	'			CutLine.AddPoint(PointAlongPlane(PtCnt2, StRad, Distance))

	'			'    DrawPolyLine CutLine, 255
	'			'    DrawPolyLine sp0, 255

	'			pTopo = sp0
	'			InterPoints0 = pTopo.Intersect(CutLine, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry0Dimension)
	'			StartPoint = Nothing

	'			If InterPoints0.PointCount > 0 Then
	'				If InterPoints0.PointCount > 1 Then
	'					Rad0 = ReturnDistanceInMeters(PtCnt2, InterPoints0.Point(0))
	'					Rad1 = ReturnDistanceInMeters(PtCnt2, InterPoints0.Point(1))

	'					If Rad0 > Rad1 Then
	'						StartPoint = InterPoints0.Point(0)
	'					Else
	'						StartPoint = InterPoints0.Point(1)
	'					End If
	'				Else
	'					Rad0 = ReturnDistanceInMeters(PtCnt2, InterPoints.Point(0))
	'					Rad1 = ReturnDistanceInMeters(PtCnt2, InterPoints0.Point(0))
	'					If Rad1 >= Rad0 Then 'hall var
	'						StartPoint = InterPoints0.Point(0)
	'					End If
	'				End If

	'				'            StRad = ReturnAngleInDegrees(PtCnt1, InterPoints0.Point(0))
	'				'            Distance = ReturnDistanceInMeters(PtCnt1, InterPoints0.Point(0))
	'				'
	'				'            Rad0 = r10 + e1 * Modulus(Turn * (StRad - AztSt1), 360.0)
	'				'            Rad1 = r10 + e1 * SubtractAnglesWithSign(StRad, AztSt1, Turn)
	'				'
	'				'            If Rad0 >= Distance Then    'hall var
	'				'                Set StartPoint = InterPoints0.Point(0)
	'				'            End If
	'			End If
	'		End If

	'		If StartPoint Is Nothing Then
	'			AztO1O2 = ReturnAngleInDegrees(PtCnt1, PtCnt2)
	'			DistO1O2 = ReturnDistanceInMeters(PtCnt1, PtCnt2)
	'			phi10 = Modulus(AztO1O2 - 90.0 * Turn, 360.0)
	'			phi20 = phi10

	'			For J = 0 To 30
	'				fTmp = Modulus((phi10 - AztSt1) * Turn, 360.0)
	'				R1 = r10 + E1 * fTmp
	'				AztR1E = RadToDeg(System.Math.Atan(RadToDeg(E1 * Turn) / R1))

	'				For I = 0 To 20
	'					fTmp = Modulus((phi20 - AztSt2) * Turn, 360.0)
	'					R2 = r20 + E2 * fTmp
	'					AztR2E = RadToDeg(System.Math.Atan(RadToDeg(E2 * Turn) / R2))
	'					F = phi20 - phi10 + AztR1E - AztR2E
	'					fTmp = RadToDeg(E2) * RadToDeg(E2)
	'					f_ = 1.0 + fTmp / (R2 * R2 + fTmp)
	'					phi2 = phi20 - F / f_

	'					If System.Math.Abs(System.Math.Sin(DegToRad(F / f_))) < degEps Then
	'						Exit For
	'					Else
	'						phi20 = phi2
	'					End If
	'				Next I

	'				pt1 = PointAlongPlane(PtCnt1, phi1, R1)
	'				pt2 = PointAlongPlane(PtCnt2, phi2, R2)
	'				fTmp = ReturnAngleInDegrees(pt1, pt2)
	'				fTmp = SubtractAnglesWithSign(phi1 + (90.0 * Turn - AztR1E), fTmp, Turn)
	'				phi1 = phi10 + fTmp * Turn
	'				If System.Math.Abs(fTmp) < degEps Then
	'					CntAngle1 = Modulus(phi1, 360.0)
	'					CntAngle2 = Modulus(phi2, 360.0)
	'					Return 1
	'				Else
	'					phi10 = phi1
	'				End If
	'			Next J

	'			Return 0
	'		End If
	'	End If

	'	For I = 0 To 20
	'		If SpiralTouchToPoint(PtCnt2, E2, r20, AztSt2, Turn, StartPoint, 1, pPoint0) <> 0 Then
	'			Rad0 = ReturnAngleInDegrees(StartPoint, pPoint0)
	'			SpiralTouchToPoint(PtCnt1, E1, r10, AztSt1, Turn, pPoint0, -1, StartPoint)

	'			Rad1 = ReturnAngleInDegrees(StartPoint, pPoint0)
	'			If System.Math.Abs(System.Math.Sin(DegToRad(Rad0 - Rad1))) < radEps Then
	'				'        DrawPolyLine CutLine, 0
	'				'            Rad0 = ReturnAngleInDegrees(PtCnt1, StartPoint)
	'				'            Rad1 = ReturnAngleInDegrees(PtCnt2, pPoint0)
	'				'            CntAngle1 = Modulus((Rad0 - AztSt1) * Turn, 360.0)
	'				'            CntAngle2 = Modulus((Rad1 - AztSt2) * Turn, 360.0)
	'				CntAngle1 = ReturnAngleInDegrees(PtCnt1, StartPoint)
	'				CntAngle2 = ReturnAngleInDegrees(PtCnt2, pPoint0)
	'				Return 1
	'			End If
	'		Else
	'			Return 0
	'		End If
	'	Next I
	'	Return 0
	'End Function

	Function OnNavaidFIXTolerArea(ByVal Nav As NavaidData, ByVal Aztin As Double, ByVal AbsH As Double, ByRef pTolerArea As ESRI.ArcGIS.Geometry.Polygon) As Double
		Dim Result As Double
		Dim pTopo As ITopologicalOperator2

		If Nav.TypeCode = eNavaidType.NDB Then
			Result = NDBFIXTolerArea(Nav.pPtPrj, Aztin, AbsH, pTolerArea)
		ElseIf (Nav.TypeCode = eNavaidType.VOR) Or (Nav.TypeCode = eNavaidType.TACAN) Then
			Result = VORFIXTolerArea(Nav.pPtPrj, Aztin, AbsH, pTolerArea)
		Else
			Return -1
		End If

		pTopo = pTolerArea
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return Result
	End Function

	Function VORFIXTolerArea(ByVal ptVor As ESRI.ArcGIS.Geometry.IPoint, ByVal Aztin As Double, ByVal AbsH As Double, ByRef TolerArea As ESRI.ArcGIS.Geometry.IPointCollection) As Double
		Dim R As Double
		Dim fTmpH As Double
		Dim ptBase1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptBase2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt4 As ESRI.ArcGIS.Geometry.IPoint

		fTmpH = AbsH - ptVor.Z
		R = fTmpH * System.Math.Tan(DegToRad(VOR.ConeAngle))
		TolerArea = New ESRI.ArcGIS.Geometry.Polygon

		ptBase1 = PointAlongPlane(ptVor, Aztin - 90.0 - VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH)
		CircleVectorIntersect(ptVor, R, ptBase1, Aztin - VOR.TrackAccuracy, pt1)
		CircleVectorIntersect(ptVor, R, ptBase1, 180.0 + Aztin + VOR.TrackAccuracy, pt2)

		ptBase2 = PointAlongPlane(ptVor, Aztin + 90.0 + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH)
		CircleVectorIntersect(ptVor, R, ptBase2, 180.0 + Aztin - VOR.TrackAccuracy, pt3)
		CircleVectorIntersect(ptVor, R, ptBase2, Aztin + VOR.TrackAccuracy, pt4)

		TolerArea.AddPoint(pt1)
		TolerArea.AddPoint(pt2)
		TolerArea.AddPoint(pt3)
		TolerArea.AddPoint(pt4)

		Return R
	End Function

	Function NDBFIXTolerArea(ByVal ptNDB As ESRI.ArcGIS.Geometry.IPoint, ByVal Aztin As Double, ByVal AbsH As Double, ByRef TolerArea As ESRI.ArcGIS.Geometry.IPointCollection) As Double
		Dim R As Double
		Dim qN As Double
		Dim fTmpH As Double
		Dim ptBase1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptBase2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt4 As ESRI.ArcGIS.Geometry.IPoint

		fTmpH = AbsH - ptNDB.Z
		R = fTmpH * System.Math.Tan(DegToRad(NDB.ConeAngle))
		TolerArea = New ESRI.ArcGIS.Geometry.Polygon
		qN = R * System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy))

		ptBase1 = PointAlongPlane(ptNDB, Aztin - 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(DegToRad(NDB.TrackAccuracy)))
		CircleVectorIntersect(ptNDB, R, ptBase1, Aztin - NDB.TrackAccuracy, pt1)
		CircleVectorIntersect(ptNDB, R, ptBase1, 180.0 + Aztin - NDB.TrackAccuracy, pt2)

		ptBase2 = PointAlongPlane(ptNDB, Aztin + 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(DegToRad(NDB.TrackAccuracy)))
		CircleVectorIntersect(ptNDB, R, ptBase2, 180.0 + Aztin + NDB.TrackAccuracy, pt3)
		CircleVectorIntersect(ptNDB, R, ptBase2, Aztin + NDB.TrackAccuracy, pt4)

		TolerArea.AddPoint(pt1)
		TolerArea.AddPoint(pt2)
		TolerArea.AddPoint(pt3)
		TolerArea.AddPoint(pt4)

		Return R
	End Function

	Public Function CreateReckoningToleranceArea(ByVal T As Double, ByVal Axis As Double, ByVal hAbs As Double, ByVal IAS As Double, ByVal dISA As Double, ByVal PtNAV As ESRI.ArcGIS.Geometry.IPoint, ByVal NavType As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim A As Double
		Dim V As Double
		Dim w As Double
		Dim OA As Double
		Dim OA1 As Double
		Dim OA2 As Double
		Dim ConeAngle As Double
		Dim TrackingTolerance As Double

		If NavType = eNavaidType.NDB Then
			ConeAngle = NDB.ConeAngle
			TrackingTolerance = NDB.TrackingTolerance
		Else
			ConeAngle = VOR.ConeAngle
			TrackingTolerance = VOR.TrackingTolerance
		End If

		A = System.Math.Tan(DegToRad(ConeAngle)) * (hAbs - PtNAV.Z)

		V = 0.277777777777778 * IAS2TAS(IAS, hAbs, dISA)
		w = 0.277777777777778 * (0.012 * hAbs + 87.0)

		'T = T + 16#

		OA = V * T
		OA1 = (V - w) * (T - 10.0) - A
		OA2 = (V + w) * (T + 10.0) + A

		CreateReckoningToleranceArea = New ESRI.ArcGIS.Geometry.Multipoint

		'DrawPoint ptNav, 255

		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(PtNAV, Axis - TrackingTolerance, OA2))
		'DrawPoint CreateReckoningToleranceArea.Point(0), 0

		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(PtNAV, Axis - TrackingTolerance, OA1))
		'DrawPoint CreateReckoningToleranceArea.Point(1), 0

		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(PtNAV, Axis + TrackingTolerance, OA1))
		'DrawPoint CreateReckoningToleranceArea.Point(2), 0

		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(PtNAV, Axis + TrackingTolerance, OA2))
		'DrawPoint CreateReckoningToleranceArea.Point(3), 0

		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(PtNAV, Axis, OA))
		'DrawPoint CreateReckoningToleranceArea.Point(4), 0
	End Function

	Function Shablon45_180(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal Axis As Double, ByVal Turn As Integer, ByVal cat As Integer, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef ptTurn45_180 As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Ix As Integer) As Integer
		'Вычисление требуемых параметров
		Dim R As Double			' 1
		Dim Rv As Double		' 2
		Dim H As Double			' 3
		Dim w As Double			' 4
		Dim w_ As Double		' 5
		Dim E As Double			' 6
		Dim ab As Double		' 7
		Dim cd As Double		' 8
		Dim cd1 As Double		' 9
		Dim cd2 As Double		'10
		Dim fTAS As Double		'11
		Dim v3600 As Double		'12

		Dim fTmp As Double
		Dim TurnAng As Double
		Dim TouchAC As Double
		Dim TouchCE2 As Double

		Dim PtB As IPoint
		Dim PtC As IPoint
		Dim ptD As IPoint
		Dim PtE2 As IPoint
		Dim PtE3 As IPoint
		Dim PtE4 As IPoint
		Dim ptTmp As IPoint
		Dim PtTmp0 As IPoint
		Dim PtTmp1 As IPoint
		Dim PtTmp2 As IPoint
		Dim ptInter As IPoint

		fTAS = IAS2TAS(IAS, AbsH, dISA)
		v3600 = fTAS / 3600.0
		R = 943.27 / fTAS
		If (R > 3.0) Then R = 3.0

		Rv = 1000.0 * fTAS / (62.83 * R)
		H = AbsH / 1000.0
		w = 12.0 * H + 87.0
		w_ = w / 3.6
		E = w_ / R

		v3600 = v3600 * 1000.0
		ab = 5.0 * v3600
		'====================

		cd = (arT45_180.Values(cat) - 5.0 - 45.0 / R) * v3600
		cd1 = cd - 5.0 * v3600
		cd2 = cd + 15.0 * v3600
		'cd3 = cd1
		'cd4 = cd2

		PtB = PointAlongPlane(ptNavPrj, Axis, ab)				'Point b
		PtTmp0 = PointAlongPlane(PtB, Axis - Turn * 90.0, Rv)

		PtC = PointAlongPlane(PtTmp0, Axis + Turn * 45.0, Rv)	'Point c
		ptD = PointAlongPlane(PtC, Axis - Turn * 45.0, cd)		'Point d
		'=========================== Traektory ================================
		ptTurn45_180 = New ESRI.ArcGIS.Geometry.Multipoint

		ptTurn45_180.AddPoint(PtB)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis

		ptTurn45_180.AddPoint(PtC)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis - Turn * 45.0

		ptTurn45_180.AddPoint(ptD)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis - Turn * 45.0

		ptTmp = LineLineIntersect(PtB, Axis, PtC, Axis - Turn * 45.0)
		fTmp = ReturnDistanceInMeters(ptTmp, ptD)

		ptTurn45_180.AddPoint(PointAlongPlane(ptTmp, Axis, fTmp))
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis + 180.0

		'======================================================================
		'Control Points
		Dim ptD2 As IPoint
		Dim PtD3 As IPoint
		Dim PtD4 As IPoint
		Dim Aztin As Double

		Aztin = Axis - Turn * 45.0
		PtD3 = PointAlongPlane(PtC, Aztin + Turn * 5.0, cd1) 'Point d3
		ptD2 = PointAlongPlane(PtC, Aztin - Turn * 5.0, cd2) 'Point d2
		PtD4 = PointAlongPlane(PtC, Aztin + Turn * 5.0, cd2) 'Point d4

		PtE2 = PointAlongPlane(ptD2, Aztin + 90.0 * Turn, Rv)	'Point e2
		PtE3 = PointAlongPlane(PtD3, Aztin + 90.0 * Turn, Rv)	'Point e3
		PtE4 = PointAlongPlane(PtD4, Aztin + 90.0 * Turn, Rv)	'Point e4

		'Radius
		Dim Wc As Double
		Dim We2 As Double
		Dim We3 As Double
		Dim We4 As Double

		Wc = 5 * w_ + 45 * E
		We2 = Rv + (arT45_180.Values(cat) + 15.0) * w_
		We3 = Rv + (arT45_180.Values(cat) - 5.0) * w_
		We4 = We2

		Dim aztWc As Double
		Dim aztWe2 As Double
		Dim aztWe3 As Double
		Dim aztWe4 As Double

		aztWc = Modulus(Axis + 180.0 + 45.0 * Turn, 360.0)
		aztWe2 = Modulus(Aztin + 90.0 * Turn + 180.0, 360.0)
		aztWe3 = aztWe2
		aztWe4 = aztWe2

		Dim Solution As Integer

		Dim Ec As Double
		Dim Rtmp As Double
		Dim Rtmp1 As Double
		Dim AztAc As Double
		Dim Rad2Touch0 As Double
		Dim Rad2Touch1 As Double
		Dim Rad2Touch2 As Double

		Ec = E
		'AztAc = ReturnAngleInDegrees(ptNavPrj, PtC)
		Shablon = New Polygon
		Shablon.AddPoint(ptNavPrj)

		AztAc = ReturnAngleInDegrees(PtE2, PtC)
		fTmp = AztAc

		ChangeSpiralStartParam(Ec, Wc, aztWc, Rtmp1, fTmp, Turn, -Turn)
		Wc = Rtmp1
		aztWc = fTmp
		Rad2Touch0 = SpiralTouchToFix(PtC, Ec, Wc, aztWc, Turn, ptNavPrj, 1, Axis)

		ChangeSpiralStartParam(E, We2, aztWe2, Rtmp, AztAc, Turn, -Turn)

		Solution = TouchTo2Spiral(PtC, Rtmp1, Ec, fTmp, PtE2, Rtmp, E, AztAc, Turn, Rad2Touch1, Rad2Touch2)
		If Solution = 0 Then
			AztAc = ReturnAngleInDegrees(ptNavPrj, PtE2)
			ChangeSpiralStartParam(E, We2, aztWe2, Rtmp, AztAc, Turn, -Turn)
			Rad2Touch2 = SpiralTouchToFix(PtE2, E, We2, aztWe2, Turn, ptNavPrj, 1, Axis)
		Else
			TurnAng = SubtractAnglesWithSign(Rad2Touch0, Rad2Touch1, Turn)

			If TurnAng < 0 Then
				TurnAng = SubtractAnglesWithSign(aztWc, Rad2Touch0, Turn)
				PtTmp0 = PointAlongPlane(PtC, Rad2Touch0, Wc + Ec * TurnAng)

				TouchAC = ReturnAngleInDegrees(ptNavPrj, PtTmp0)
				TurnAng = SubtractAnglesWithSign(aztWc, Rad2Touch1, Turn)
				PtTmp1 = PointAlongPlane(PtC, Rad2Touch1, Wc + Ec * TurnAng)

				TurnAng = SubtractAnglesWithSign(aztWe2, Rad2Touch2, Turn)
				PtTmp2 = PointAlongPlane(PtE2, Rad2Touch2, We2 + E * TurnAng) '* turn)

				fTmp = ReturnDistanceInMeters(PtTmp0, PtTmp1)
				If fTmp > distEps Then
					TouchCE2 = ReturnAngleInDegrees(PtTmp1, PtTmp2)
					ptInter = LineLineIntersect(ptNavPrj, TouchAC, PtTmp1, TouchCE2)
					Shablon.AddPoint(ptInter)
				Else
					Shablon.AddPoint(PtTmp0)
				End If
			Else
				fTmp = SubtractAnglesWithSign(aztWc, Rad2Touch0, Turn)
				If fTmp < 0 Then
					ChangeSpiralStartParam(E, Wc, aztWc, fTmp, Rad2Touch0, Turn, -Turn)
				Else
					ChangeSpiralStartParam(E, Wc, aztWc, fTmp, Rad2Touch0, Turn, Turn)
				End If
				'        CreateSpiralBy2Radial PtC, Wc, aztWc, Rad2Touch1, E, Turn, Shablon
				CreateSpiralBy2Radial(PtC, fTmp, Rad2Touch0, Rad2Touch1, E, Turn, Shablon)
			End If
		End If

		Ix = Shablon.PointCount

		Solution = TouchTo2Spiral(PtE2, We2, E, aztWe2, PtE4, We4, E, aztWe4, Turn, Rad2Touch0, Rad2Touch1)
		If Solution = 0 Then
			MessageBox.Show("Wrong parameters of template 45 - 180")
			Return 0
		End If

		fTmp = SubtractAnglesWithSign(aztWe2, Rad2Touch2, Turn)
		If fTmp < 0 Then
			ChangeSpiralStartParam(E, We2, aztWe2, fTmp, Rad2Touch2, Turn, -Turn)
		Else
			ChangeSpiralStartParam(E, We2, aztWe2, fTmp, Rad2Touch2, Turn, Turn)
		End If
		CreateSpiralBy2Radial(PtE2, fTmp, Rad2Touch2, Rad2Touch0, E, Turn, Shablon)

		Solution = TouchTo2Spiral(PtE4, We4, E, aztWe4, PtE3, We3, E, aztWe3, Turn, Rad2Touch0, Rad2Touch2)
		If Solution = 0 Then
			MessageBox.Show("Wrong parameters of template 45 - 180")
			Return 0
		End If

		ChangeSpiralStartParam(E, We4, aztWe4, fTmp, Rad2Touch1, Turn, -Turn)
		CreateSpiralBy2Radial(PtE4, fTmp, Rad2Touch1, Rad2Touch0, E, Turn, Shablon)
		Rad2Touch0 = SpiralTouchToFix(PtE3, E, We3, aztWe3, Turn, ptNavPrj, -1, Axis)

		fTmp = We3 + E * Modulus((Rad2Touch2 - aztWe3) * Turn, 360.0)
		CreateSpiralBy2Radial(PtE3, fTmp, Rad2Touch2, Rad2Touch0, E, Turn, Shablon)

		Shablon.AddPoint(ptNavPrj)
		Return 1
	End Function

	Function Shablon80_260(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal Axis As Double, ByVal Turn As Integer, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef ptTurn80_260 As ESRI.ArcGIS.Geometry.IPointCollection) As Integer
		Dim R As Double			' 1
		Dim H As Double			' 2
		Dim w As Double			' 3
		Dim E As Double			' 4
		Dim Rs As Double		' 5
		Dim w_ As Double		' 6
		Dim ab As Double		' 7
		Dim de As Double		' 8 = d1e1 = d2e2
		Dim fTAS As Double		' 9
		Dim v3600 As Double		'10

		fTAS = IAS2TAS(IAS, AbsH, dISA)
		v3600 = fTAS / 3.6
		R = 943.27 / fTAS
		If (R > 3.0) Then R = 3.0

		Rs = 1000.0 * fTAS / (62.83 * R)
		H = AbsH / 1000.0
		w = 12 * H + 87
		w_ = w / 3.6
		E = w_ / R

		ab = 5.0 * v3600
		de = 10.0 * v3600

		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		PtStart = ptNavPrj
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim PtB As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptE As ESRI.ArcGIS.Geometry.IPoint
		PtB = PointAlongPlane(PtStart, Axis, ab) 'Point b
		ptCnt = PointAlongPlane(PtB, Axis - Turn * 90.0, Rs)
		ptD = PointAlongPlane(ptCnt, Axis + Turn * (90.0 - 80.0), Rs) 'Point d
		ptE = PointAlongPlane(ptD, Axis - Turn * 80.0, de)	'Point e

		'Control Points
		Dim ptD1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD2 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtE1 As ESRI.ArcGIS.Geometry.IPoint	'd3
		Dim PtE2 As ESRI.ArcGIS.Geometry.IPoint	'd4

		'Dim Aztin As Double
		'Aztin = Axis - Turn * 80.0
		ptD1 = PointAlongPlane(ptCnt, Axis + Turn * (90.0 - 80.0 + 5.0), Rs)	'Point d1
		ptD2 = PointAlongPlane(ptCnt, Axis + Turn * (90.0 - 80.0 - 5.0), Rs)	'Point d2
		PtE1 = PointAlongPlane(ptD1, Axis + Turn * (90.0 - 80.0 + 5.0) - Turn * 90.0, de)	'Point e1
		PtE2 = PointAlongPlane(ptD2, Axis + Turn * (90.0 - 80.0 - 5.0) - Turn * 90.0, de)	'Point e2

		Dim PtF1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtF2 As ESRI.ArcGIS.Geometry.IPoint
		PtF1 = PointAlongPlane(PtE1, Axis + Turn * (90.0 - 80.0 + 5.0), Rs)	'Point f1
		PtF2 = PointAlongPlane(PtE2, Axis + Turn * (90.0 - 80.0 - 5.0), Rs)	'Point f2
		'=========================== Traektory ================================
		Dim fTmp As Double

		ptTurn80_260 = New ESRI.ArcGIS.Geometry.Multipoint


		ptTurn80_260.AddPoint(PtB)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis

		ptTurn80_260.AddPoint(ptD)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis - Turn * 80.0

		'=============Adding 29.11.2004
		ptTurn80_260.AddPoint(ptE)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis - Turn * 80.0

		ptTmp = LineLineIntersect(PtB, Axis, ptE, Axis - Turn * 80.0)
		fTmp = ReturnDistanceInMeters(ptTmp, ptE)
		'====================End Adding=============
		'============Comment 29.11.2004
		'Set ptTmp = LineLineIntersect(PtB, Axis, ptD, Axis - Turn * 80#)
		'fTmp = ReturnDistanceInMeters(ptTmp, ptD)
		'=============End Comment========

		ptTurn80_260.AddPoint(PointAlongPlane(ptTmp, Axis, fTmp))
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis + 180.0

		'========================================================================
		Dim Solution As Integer
		Dim AztAF As Double
		Dim Rtmp As Double
		Dim Rtmp1 As Double
		Dim RF1St As Double
		Dim RF2St As Double
		Dim AztF1St As Double
		Dim AztF2St As Double
		Dim AztFSt As Double
		Dim AztFEnd As Double
		Dim AztF2F1 As Double
		Dim Rad2Touch1 As Double
		Dim Rad2Touch2 As Double

		AztAF = ReturnAngleInDegrees(PtF2, ptNavPrj)
		RF2St = Rs + 15.0 * w_ + 85.0 * E
		AztF2St = ReturnAngleInDegrees(PtF2, PtE2)
		RF1St = Rs + 15.0 * w_ + 75.0 * E
		AztF1St = ReturnAngleInDegrees(PtF1, PtE1)

		ChangeSpiralStartParam(E, RF2St, AztF2St, Rtmp, AztAF, Turn, -Turn)
		AztFSt = SpiralTouchToFix(PtF2, E, Rtmp, AztAF, Turn, ptNavPrj, 1, Axis)
		AztF2F1 = ReturnAngleInDegrees(PtF2, PtF1)

		fTmp = AztF2F1
		ChangeSpiralStartParam(E, RF2St, AztF2St, Rtmp, fTmp, Turn, -Turn)

		Rtmp1 = Rtmp
		ChangeSpiralStartParam(E, RF1St, AztF1St, Rtmp, AztF2F1, Turn, -Turn)

		Solution = TouchTo2Spiral(PtF2, Rtmp1, E, fTmp, PtF1, Rtmp, E, AztF2F1, Turn, Rad2Touch1, Rad2Touch2)
		If Solution = 0 Then
			MessageBox.Show("Wrong parameters of template 80 - 260")
			Return 0
		End If
		Shablon = New ESRI.ArcGIS.Geometry.Polygon

		Shablon.AddPoint(ptNavPrj)
		'ChangeSpiralStartParam E, RF2St, AztF2St, Rtmp, AztF2StNew, turn, -turn
		Dim TurnAng As Double
		TurnAng = SubtractAnglesWithSign(AztF2St, AztFSt, Turn)
		Dim RStNew As Double
		RStNew = RF2St + TurnAng * E
		CreateSpiralBy2Radial(PtF2, RStNew, AztFSt, Rad2Touch1, E, Turn, Shablon)

		'Secondary Spiral
		AztAF = ReturnAngleInDegrees(ptNavPrj, PtF1)
		ChangeSpiralStartParam(E, RF1St, AztF1St, Rtmp, AztAF, Turn, Turn)
		AztFEnd = SpiralTouchToFix(PtF1, E, Rtmp, AztAF, Turn, ptNavPrj, -1, Axis)

		'turnAng = SubtractAnglesWithSign(AztF1St, Rad2Touch2, turn)
		TurnAng = Modulus((Rad2Touch2 - AztF1St) * Turn, 360.0)
		RStNew = RF1St + TurnAng * E

		CreateSpiralBy2Radial(PtF1, RStNew, Rad2Touch2, AztFEnd, E, Turn, Shablon)
		Shablon.AddPoint(ptNavPrj)
		'DrawPolygon Shablon, 0

		Return 1
	End Function

	Function ReversalShablonByDistance(ByVal ptCenterPrj As IPoint, ByVal ptDMEPrj As IPoint, ByVal NavType As eNavaidType, ByRef T As Double, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal Axis As Double, ByVal Alpha As Double, ByVal Turn As Integer, ByRef Shablon As IPointCollection, ByRef NavArea As IPointCollection, ByRef MPtCollection As IPointCollection) As Integer
		Dim bFlg As Boolean
		Dim Solution As Integer
		'Вычисление требуемых параметров
		Dim fTAS As Double	' 1
		Dim v3600 As Double	' 2
		Dim R As Double		' 3
		Dim Rv As Double	' 4
		Dim w As Double		' 5
		Dim w_ As Double	' 6
		Dim E As Double		' 7
		Dim Phi As Double	' 8
		Dim L As Double		' 9
		Dim Lsl As Double	'10

		Dim ab1 As Double	'11
		Dim ab2 As Double	'12
		Dim ab3 As Double	'13
		Dim ab4 As Double	'14
		Dim D As Double		'15
		Dim N3l As Double	'16

		Dim dh As Double
		Dim Rr As Double
		Dim ffTmp As Double
		Dim DMEToler As Double
		Dim AztToFix As Double
		Dim DistToFix As Double

		Dim RStSp As Double
		Dim RStSp0 As Double
		Dim NewStR As Double
		Dim RadStSp As Double
		Dim RadStSp0 As Double
		Dim NewStRad As Double

		Dim TouchRad1 As Double
		Dim TouchRad2 As Double
		Dim TurnAngle As Double
		Dim TouchAngl As Double

		Dim AztAC2 As Double
		Dim AztC5C3 As Double
		Dim AztC2C4 As Double

		Dim fTmp As Double
		Dim fTmp0 As Double
		Dim RadC3 As Double
		Dim RadC5 As Double
		Dim NomTrack As Double
		Dim TrackToler As Double
		Dim AdjustCourse As Double

		Dim Ptk As IPoint
		Dim pt1 As IPoint
		Dim pt2 As IPoint
		Dim pt3 As IPoint
		'Dim Pt4 As IPoint
		Dim Ptb2 As IPoint
		Dim Ptb3 As IPoint
		Dim Ptb4 As IPoint
		Dim PtC2 As IPoint
		Dim PtC3 As IPoint
		Dim PtC4 As IPoint
		Dim PtC5 As IPoint
		Dim ptTmp As IPoint
		Dim PtCntIn As IPoint
		Dim NavAreaSt As IPoint
		Dim ptInter As IConstructPoint

		fTAS = IAS2TAS(IAS, AbsH, dISA)
		v3600 = fTAS / 3.6

		R = 943.27 / fTAS
		If (R > 3.0) Then R = 3.0

		Rv = 1000.0 * fTAS / (62.83 * R)
		w = 0.012 * AbsH + 87.0
		w_ = w / 3.6
		E = w_ / R

		D = RadToDeg(ArcSin(w / fTAS))
		N3l = 11.0 * v3600

		If NavType = eNavaidType.VOR Then
			TrackToler = VOR.TrackingTolerance
		Else
			TrackToler = NDB.TrackingTolerance
		End If

		Do
			L = 60.0 * v3600 * T

			dh = AbsH - ptDMEPrj.Z
			Lsl = Math.Sqrt(L * L + dh * dh)
			DMEToler = DME.MinimalError + DME.ErrorScalingUp * Lsl

			ab1 = Lsl - DMEToler + 5 * (v3600 - w_)
			ab2 = Lsl + DMEToler + 11 * (v3600 + w_)

			ab3 = ab1
			ab4 = ab2

			'Конец вычисления требуемых параметров
			T = L / (60.0 * v3600)

			If fTAS <= 315.0 Then
				Phi = 36.0 / T
			Else
				Phi = 0.116 * fTAS / T
			End If

			'Nominal Track ===================

			MPtCollection = New ESRI.ArcGIS.Geometry.Multipoint
			pt1 = New Point
			pt1.PutCoords(ptCenterPrj.X, ptCenterPrj.Y)
			pt1.Z = ptCenterPrj.Z
			pt1.M = Axis - Turn * Phi
			MPtCollection.AddPoint(pt1)

			pt2 = PointAlongPlane(pt1, pt1.M, L)
			pt2.M = pt1.M
			MPtCollection.AddPoint(pt2)

			pt3 = PointAlongPlane(pt1, Axis, L)
			pt3.M = Axis + 180.0
			MPtCollection.AddPoint(pt3)

			'Pt4 = New Point
			'Pt4.PutCoords(ptNavPrj.X, ptNavPrj.Y)
			'Pt4.M = Axis + 180.0
			'MPtCollection.AddPoint(Pt4)
			'================================

			'Защита входа
			'fTmp = axis - (Phi + dAlpha) * turn
			AdjustCourse = Modulus(Axis - (Phi + Alpha) * Turn, 360.0)
			If NavType = eNavaidType.VOR Then
				VORFIXTolerArea(ptCenterPrj, AdjustCourse, AbsH, NavArea)
			Else
				NDBFIXTolerArea(ptCenterPrj, AdjustCourse, AbsH, NavArea)
			End If

			If Turn = -1 Then 'Правый
				NavAreaSt = NavArea.Point(3)
			Else
				NavAreaSt = NavArea.Point(0)
			End If

			PtCntIn = PointAlongPlane(NavAreaSt, AdjustCourse, N3l)
			PtCntIn = PointAlongPlane(PtCntIn, AdjustCourse + 90.0 * Turn, Rv)

			NewStRad = ReturnAngleInDegrees(PtCntIn, NavAreaSt)
			RadStSp0 = Modulus(AdjustCourse - 90.0 * Turn, 360.0)
			RStSp0 = Rv + 11.0 * w_

			ChangeSpiralStartParam(E, RStSp0, RadStSp0, NewStR, NewStRad, Turn, -Turn)
			TouchRad1 = SpiralTouchToFix(PtCntIn, E, NewStR, NewStRad, Turn, NavAreaSt, 1, Axis)
			TurnAngle = SubtractAnglesWithSign(RadStSp0, TouchRad1, Turn)

			NewStR = RStSp0 + E * TurnAngle
			NewStRad = TouchRad1

			NomTrack = Axis - Turn * Phi
			Ptb2 = PointAlongPlane(ptCenterPrj, NomTrack - TrackToler * Turn, ab2)
			Ptb3 = PointAlongPlane(ptCenterPrj, NomTrack + TrackToler * Turn, ab3)
			Ptb4 = PointAlongPlane(ptCenterPrj, NomTrack + TrackToler * Turn, ab4)

			PtC2 = PointAlongPlane(Ptb2, NomTrack + 90.0 * Turn, Rv)
			PtC4 = PointAlongPlane(Ptb4, NomTrack + 90.0 * Turn, Rv)

			RStSp = Rv
			RadStSp = Modulus(NomTrack + 90.0 * Turn + 180.0, 360.0)

			AztAC2 = ReturnAngleInDegrees(PtC2, PtCntIn)
			fTmp = AztAC2

			ChangeSpiralStartParam(E, NewStR, NewStRad, RStSp0, AztAC2, Turn, -Turn)
			ChangeSpiralStartParam(E, RStSp, RadStSp, fTmp0, fTmp, Turn, -Turn)

			Solution = TouchTo2Spiral(PtCntIn, RStSp0, E, AztAC2, PtC2, fTmp0, E, fTmp, Turn, RadC5, RadC3)

			If Solution = 0 Then
				MessageBox.Show("ERROR ON 'ReversalShablon'")
				'MessageBox.Show("Параметры шаблона разворота на посадочный курс неправильны")
				Return 0
			End If

			Shablon = New ESRI.ArcGIS.Geometry.Polygon
			Shablon.AddPoint(ptCenterPrj)
			Shablon.AddPoint(NavAreaSt)

			CreateSpiralBy2Radial(PtCntIn, NewStR, NewStRad, RadC5, E, Turn, Shablon)

			TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
			RStSp = RStSp + E * TurnAngle
			RadStSp = RadC3

			AztC2C4 = ReturnAngleInDegrees(PtC2, PtC4)
			TouchAngl = Modulus(Axis - (90.0 + D) * Turn + 180.0, 360.0)

			bFlg = AnglesSideDef(TouchAngl, AztC2C4) = Turn

			If bFlg Then
				TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, AztC2C4, Turn)
				fTmp = Rv + TouchRad1 * E
				TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90.0) * Turn, 360.0)
				ptTmp = PointAlongPlane(PtC4, TouchRad1, fTmp)
			Else
				TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
				fTmp = Rv + TouchRad1 * E
				TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90.0) * Turn, 360.0)
				ptTmp = PointAlongPlane(PtC2, TouchRad1, fTmp)
			End If

			CreateSpiralBy2Radial(PtC2, RStSp, RadStSp, TouchRad1, E, Turn, Shablon)

			If bFlg Then
				RStSp = fTmp

				TouchRad2 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
				TouchRad2 = Modulus(NomTrack + (TouchRad2 - 90.0) * Turn, 360.0)

				CreateSpiralBy2Radial(PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, Shablon)
			End If

			'sp = New Polygon()
			'CreateSpiralBy2Radial(PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, sp)
			'DrawPolygon(Shablon, 255)

			ptInter = New ESRI.ArcGIS.Geometry.Point
			ptInter.ConstructAngleIntersection(ptCenterPrj, DegToRad(Axis), ptTmp, DegToRad(TouchAngl))
			Ptk = ptInter

			PtC5 = PointAlongPlane(Ptk, Axis + 180.0, Rv)
			PtC3 = PointAlongPlane(Ptb3, NomTrack + 90.0 * Turn, Rv)
			'DrawPointWithText(PtC3, "PtC3", RGB(0, 0, 255))
			'DrawPointWithText(PtC5, "PtC5", RGB(0, 0, 255))

			'DrawPointWithText(Ptk, "Ptk", RGB(0, 0, 255))
			'DrawPointWithText(Ptb2, "Ptb2", RGB(0, 0, 255))
			'DrawPointWithText(Ptb3, "Ptb3", RGB(0, 0, 255))
			'DrawPointWithText(Ptb4, "Ptb4", RGB(0, 0, 255))
			'Application.DoEvents()

			RStSp = Rv + 190.0 * E
			RadStSp = Modulus(NomTrack - 90.0 * Turn + 190.0 * Turn, 360.0)

			AztC5C3 = ReturnAngleInDegrees(PtC3, PtC5)
			ChangeSpiralStartParam(E, RStSp, RadStSp, NewStR, AztC5C3, Turn, -Turn)
			Solution = TouchTo2Spiral(PtC5, Rv, E, Axis, PtC3, NewStR, E, AztC5C3, Turn, RadC5, RadC3)
			If Solution = 0 Then
				MessageBox.Show("ERROR ON 'ReversalShablon'")
				'MessageBox.Show("Парамеры шаблона разворота на посадочный курс неправильны")
				Return 0
			End If

			CreateSpiralBy2Radial(PtC5, Rv, Axis, RadC5, E, Turn, Shablon)
			'DrawPolygon(Shablon, 255)


			TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
			NewStR = RStSp + E * TurnAngle

			fTmp = ReturnAngleInDegrees(ptCenterPrj, PtC3)

			ChangeSpiralStartParam(E, NewStR, RadC3, fTmp0, fTmp, Turn, -Turn)

			DistToFix = ReturnDistanceInMeters(PtC3, ptCenterPrj)
			AztToFix = ReturnAngleInDegrees(PtC3, ptCenterPrj)

			ffTmp = Modulus((AztToFix - fTmp) * Turn, 360.0)
			If (ffTmp > 180.0) And (fTmp0 = 0.0) Then ffTmp = ffTmp - 360.0

			Rr = fTmp0 + E * ffTmp '* turn

			T = T + 0.5
		Loop While Rr >= DistToFix

		T = T - 0.5

		TouchRad1 = SpiralTouchToFix(PtC3, E, fTmp0, fTmp, Turn, ptCenterPrj, -1, NomTrack + 10 * Turn)
		CreateSpiralBy2Radial(PtC3, NewStR, RadC3, TouchRad1, E, Turn, Shablon)
		Shablon.AddPoint(ptCenterPrj)

		Return 1
	End Function

	Function ReversalShablon(ByVal ptNavPrj As IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByRef T As Double, ByVal Axis As Double, ByVal Alpha As Double, ByVal Turn As Integer, ByVal NavType As eNavaidType, ByRef Shablon As IPointCollection, ByRef NavArea As IPointCollection, ByRef MPtCollection As IPointCollection) As Integer
		Dim bFlg As Boolean
		Dim Solution As Integer
		'Вычисление требуемых параметров
		Dim fTAS As Double	' 1
		Dim v3600 As Double	' 2
		Dim R As Double		' 3
		Dim Rv As Double	' 4
		Dim w As Double		' 5
		Dim w_ As Double	' 6
		Dim E As Double		' 7
		Dim Phi As Double	' 8
		Dim zN As Double	' 9
		Dim t_ As Double	'10
		Dim L As Double		'11

		Dim ab1 As Double	'12
		Dim ab2 As Double	'13
		Dim ab3 As Double	'14
		Dim ab4 As Double	'15
		Dim D As Double		'16
		Dim N3l As Double	'17

		Dim Rr As Double
		Dim ffTmp As Double
		Dim AztToFix As Double
		Dim DistToFix As Double

		Dim RStSp As Double
		Dim RStSp0 As Double
		Dim NewStR As Double
		Dim RadStSp As Double
		Dim RadStSp0 As Double
		Dim NewStRad As Double

		Dim TouchRad1 As Double
		Dim TouchRad2 As Double
		Dim TurnAngle As Double
		Dim TouchAngl As Double

		Dim AztAC2 As Double
		Dim AztC5C3 As Double
		Dim AztC2C4 As Double

		Dim fTmp As Double
		Dim fTmp0 As Double
		Dim RadC3 As Double
		Dim RadC5 As Double
		Dim NomTrack As Double
		Dim TrackToler As Double
		Dim AdjustCourse As Double

		Dim Ptk As IPoint
		Dim pt1 As IPoint
		Dim pt2 As IPoint
		Dim pt3 As IPoint
		'Dim Pt4 As IPoint
		Dim Ptb2 As IPoint
		Dim Ptb3 As IPoint
		Dim Ptb4 As IPoint
		Dim PtC2 As IPoint
		Dim PtC3 As IPoint
		Dim PtC4 As IPoint
		Dim PtC5 As IPoint
		Dim ptTmp As IPoint
		Dim PtCntIn As IPoint
		Dim NavAreaSt As IPoint
		Dim ptInter As IConstructPoint

		fTAS = IAS2TAS(IAS, AbsH, dISA)
		v3600 = fTAS / 3.6

		R = 943.27 / fTAS
		If (R > 3.0) Then R = 3.0

		Rv = 1000.0 * fTAS / (62.83 * R)
		w = 0.012 * AbsH + 87.0
		w_ = w / 3.6
		E = w_ / R

		If NavType = eNavaidType.NDB Then
			zN = (AbsH - ptNavPrj.Z) * System.Math.Tan(DegToRad(NDB.ConeAngle))
			TrackToler = NDB.TrackingTolerance
		Else
			zN = (AbsH - ptNavPrj.Z) * System.Math.Tan(DegToRad(VOR.ConeAngle))
			TrackToler = VOR.TrackingTolerance
		End If

		D = RadToDeg(ArcSin(w / fTAS))
		N3l = 11.0 * v3600

		Do
			t_ = 60.0 * T
			L = v3600 * t_

			ab1 = (t_ - 5.0) * (v3600 - w_) - zN
			ab2 = (t_ + 21.0) * (v3600 + w_) + zN

			ab3 = ab1
			ab4 = ab2

			'Конец вычисления требуемых параметров

			If fTAS <= 315.0 Then
				Phi = 36.0 / T
			Else
				Phi = 0.116 * fTAS / T
			End If

			AdjustCourse = Modulus(Axis - (Phi + Alpha) * Turn, 360.0)
			If NavType = eNavaidType.VOR Then
				VORFIXTolerArea(ptNavPrj, AdjustCourse, AbsH, NavArea)
			Else
				NDBFIXTolerArea(ptNavPrj, AdjustCourse, AbsH, NavArea)
			End If

			'Nominal Track ===================

			MPtCollection = New ESRI.ArcGIS.Geometry.Multipoint
			pt1 = New Point
			pt1.PutCoords(ptNavPrj.X, ptNavPrj.Y)
			pt1.Z = ptNavPrj.Z
			pt1.M = Axis - Turn * Phi
			MPtCollection.AddPoint(pt1)

			pt2 = PointAlongPlane(pt1, pt1.M, L)
			pt2.M = pt1.M
			MPtCollection.AddPoint(pt2)

			pt3 = PointAlongPlane(pt1, Axis, L)
			pt3.M = Axis + 180.0
			MPtCollection.AddPoint(pt3)

			'Pt4 = New Point
			'Pt4.PutCoords(ptNavPrj.X, ptNavPrj.Y)
			'Pt4.M = Axis + 180.0
			'MPtCollection.AddPoint(Pt4)
			'================================
			'Защита входа
			'fTmp = axis - (Phi + dAlpha) * turn

			If Turn = -1 Then 'Правый
				NavAreaSt = NavArea.Point(3)
			Else
				NavAreaSt = NavArea.Point(0)
			End If

			PtCntIn = PointAlongPlane(NavAreaSt, AdjustCourse, N3l)
			PtCntIn = PointAlongPlane(PtCntIn, AdjustCourse + 90.0 * Turn, Rv)

			NewStRad = ReturnAngleInDegrees(PtCntIn, NavAreaSt)
			RadStSp0 = Modulus(AdjustCourse - 90.0 * Turn, 360.0)
			RStSp0 = Rv + 11.0 * w_

			ChangeSpiralStartParam(E, RStSp0, RadStSp0, NewStR, NewStRad, Turn, -Turn)
			TouchRad1 = SpiralTouchToFix(PtCntIn, E, NewStR, NewStRad, Turn, NavAreaSt, 1, Axis)
			TurnAngle = SubtractAnglesWithSign(RadStSp0, TouchRad1, Turn)

			NewStR = RStSp0 + E * TurnAngle
			NewStRad = TouchRad1

			NomTrack = Axis - Turn * Phi
			Ptb2 = PointAlongPlane(ptNavPrj, NomTrack - TrackToler * Turn, ab2)
			Ptb3 = PointAlongPlane(ptNavPrj, NomTrack + TrackToler * Turn, ab3)
			Ptb4 = PointAlongPlane(ptNavPrj, NomTrack + TrackToler * Turn, ab4)

			PtC2 = PointAlongPlane(Ptb2, NomTrack + 90.0 * Turn, Rv)
			PtC4 = PointAlongPlane(Ptb4, NomTrack + 90.0 * Turn, Rv)

			RStSp = Rv
			RadStSp = Modulus(NomTrack + 90.0 * Turn + 180.0, 360.0)

			AztAC2 = ReturnAngleInDegrees(PtC2, PtCntIn)
			fTmp = AztAC2

			ChangeSpiralStartParam(E, NewStR, NewStRad, RStSp0, AztAC2, Turn, -Turn)
			ChangeSpiralStartParam(E, RStSp, RadStSp, fTmp0, fTmp, Turn, -Turn)

			Solution = TouchTo2Spiral(PtCntIn, RStSp0, E, AztAC2, PtC2, fTmp0, E, fTmp, Turn, RadC5, RadC3)

			If Solution = 0 Then
				MessageBox.Show("ERROR ON 'ReversalShablon'")
				'MessageBox.Show("Параметры шаблона разворота на посадочный курс неправильны")
				Return 0
			End If

			Shablon = New ESRI.ArcGIS.Geometry.Polygon
			Shablon.AddPoint(ptNavPrj)
			Shablon.AddPoint(NavAreaSt)

			CreateSpiralBy2Radial(PtCntIn, NewStR, NewStRad, RadC5, E, Turn, Shablon)

			TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
			RStSp = RStSp + E * TurnAngle
			RadStSp = RadC3

			AztC2C4 = ReturnAngleInDegrees(PtC2, PtC4)
			TouchAngl = Modulus(Axis - (90.0 + D) * Turn + 180.0, 360.0)

			bFlg = AnglesSideDef(TouchAngl, AztC2C4) = Turn

			If bFlg Then
				TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, AztC2C4, Turn)
				fTmp = Rv + TouchRad1 * E
				TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90.0) * Turn, 360.0)
				ptTmp = PointAlongPlane(PtC4, TouchRad1, fTmp)
			Else
				TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
				fTmp = Rv + TouchRad1 * E
				TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90.0) * Turn, 360.0)
				ptTmp = PointAlongPlane(PtC2, TouchRad1, fTmp)
			End If

			CreateSpiralBy2Radial(PtC2, RStSp, RadStSp, TouchRad1, E, Turn, Shablon)

			If bFlg Then
				RStSp = fTmp

				TouchRad2 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
				TouchRad2 = Modulus(NomTrack + (TouchRad2 - 90.0) * Turn, 360.0)

				CreateSpiralBy2Radial(PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, Shablon)
			End If

			'sp = New Polygon()
			'CreateSpiralBy2Radial(PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, sp)
			'DrawPolygon(Shablon, 255)

			ptInter = New ESRI.ArcGIS.Geometry.Point
			ptInter.ConstructAngleIntersection(ptNavPrj, DegToRad(Axis), ptTmp, DegToRad(TouchAngl))
			Ptk = ptInter

			'DrawPoint(Ptk, RGB(0, 0, 255))

			PtC5 = PointAlongPlane(Ptk, Axis + 180.0, Rv)
			PtC3 = PointAlongPlane(Ptb3, NomTrack + 90.0 * Turn, Rv)
			RStSp = Rv + 190.0 * E
			RadStSp = Modulus(NomTrack - 90.0 * Turn + 190.0 * Turn, 360.0)

			AztC5C3 = ReturnAngleInDegrees(PtC3, PtC5)
			ChangeSpiralStartParam(E, RStSp, RadStSp, NewStR, AztC5C3, Turn, -Turn)
			Solution = TouchTo2Spiral(PtC5, Rv, E, Axis, PtC3, NewStR, E, AztC5C3, Turn, RadC5, RadC3)
			If Solution = 0 Then
				MessageBox.Show("ERROR ON 'ReversalShablon'")
				'MessageBox.Show("Парамеры шаблона разворота на посадочный курс неправильны")
				Return 0
			End If

			CreateSpiralBy2Radial(PtC5, Rv, Axis, RadC5, E, Turn, Shablon)
			'DrawPolygon(Shablon, 255)

			TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
			NewStR = RStSp + E * TurnAngle

			fTmp = ReturnAngleInDegrees(ptNavPrj, PtC3)

			ChangeSpiralStartParam(E, NewStR, RadC3, fTmp0, fTmp, Turn, -Turn)

			DistToFix = ReturnDistanceInMeters(PtC3, ptNavPrj)
			AztToFix = ReturnAngleInDegrees(PtC3, ptNavPrj)

			ffTmp = Modulus((AztToFix - fTmp) * Turn, 360.0)
			If (ffTmp > 180.0) And (fTmp0 = 0.0) Then ffTmp = ffTmp - 360.0

			Rr = fTmp0 + E * ffTmp '* turn

			T = T + 0.5
		Loop While Rr >= DistToFix

		T = T - 0.5

		TouchRad1 = SpiralTouchToFix(PtC3, E, fTmp0, fTmp, Turn, ptNavPrj, -1, NomTrack + 10 * Turn)
		CreateSpiralBy2Radial(PtC3, NewStR, RadC3, TouchRad1, E, Turn, Shablon)
		Shablon.AddPoint(ptNavPrj)

		Return 1
	End Function

	Function HoldingShablon(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal NavType As eNavaidType, ByRef IAS As Double, ByRef AbsH As Double, ByRef dISA As Double, ByRef T As Double, ByRef Axis As Double, ByRef Turn As Integer, ByRef HoldingArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Line3 As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Turn180 As ESRI.ArcGIS.Geometry.IPointCollection) As Integer
		'Вычисление требуемых параметров
		Dim fTAS As Double		' 1
		Dim v3600 As Double		' 2
		Dim Rv As Double		' 3
		Dim Rs As Double		' 4
		Dim H As Double			' 5
		Dim w As Double			' 6
		Dim w_ As Double		' 7
		Dim E45 As Double		' 8
		Dim E As Double			' 9
		Dim t_ As Double		'10
		Dim L As Double			'11
		Dim ab As Double		'12
		Dim ac As Double		'13
		Dim gi1 As Double		'14
		Dim gi2 As Double		'15
		Dim xe As Double		'16
		Dim ye As Double		'17

		Dim PtB As ESRI.ArcGIS.Geometry.IPoint
		Dim PtC As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntB As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntC As ESRI.ArcGIS.Geometry.IPoint
		Dim PtG As ESRI.ArcGIS.Geometry.IPoint
		Dim PtI1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtI2 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtI3 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtI4 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntI2 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntI4 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntI1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtCntI3 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtN3 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtN4_ As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint

		Shablon = New ESRI.ArcGIS.Geometry.Polygon
		HoldingArea = New ESRI.ArcGIS.Geometry.Multipoint
		Line3 = New ESRI.ArcGIS.Geometry.Multipoint
		Turn180 = New ESRI.ArcGIS.Geometry.Polyline

		fTAS = IAS2TAS(IAS, AbsH, dISA)
		v3600 = 0.277777777777778 * fTAS
		Rv = 943.27 / fTAS
		If (Rv > 3.0) Then Rv = 3.0

		Rs = 1000.0 * fTAS / (62.83 * Rv)
		H = AbsH / 1000.0
		w = 12.0 * H + 87.0

		w_ = 0.277777777777778 * w

		E = w_ / Rv
		E45 = 45.0 * E
		t_ = 60.0 * T

		L = v3600 * t_
		ab = 5.0 * v3600
		ac = 11.0 * v3600
		gi1 = (t_ - 5.0) * v3600
		gi2 = (t_ + 21.0) * v3600

		xe = 2.0 * Rs + (t_ + 15.0) * v3600 + (26.0 + 195.0 / Rv + t_) * w_
		ye = 11.0 * v3600 * System.Math.Cos(DegToRad(20.0)) + Rs * System.Math.Sin(DegToRad(20.0)) + Rs + (t_ + 15.0) * v3600 * System.Math.Tan(DegToRad(5.0)) + (26.0 + 125.0 / Rv + t_) * w_

		'Конец вычисления требуемых параметров
		'Вычисление центров спиралей

		PtB = PointAlongPlane(ptNavPrj, Axis + 180.0, ab)
		PtC = PointAlongPlane(ptNavPrj, Axis + 180.0, ac)
		PtCntB = PointAlongPlane(PtB, Axis - 90.0 * Turn, Rs)
		PtCntC = PointAlongPlane(PtC, Axis - 90.0 * Turn, Rs)
		PtG = PointAlongPlane(PtC, Axis - 90.0 * Turn, 2.0 * Rs)

		PtI1 = PointAlongPlane(PtG, Axis - 5.0 * Turn, gi1)
		PtI2 = PointAlongPlane(PtG, Axis - 5.0 * Turn, gi2)
		PtI3 = PointAlongPlane(PtG, Axis + 5.0 * Turn, gi1)
		PtI4 = PointAlongPlane(PtG, Axis + 5.0 * Turn, gi2)
		PtCntI1 = PointAlongPlane(PtI1, Axis + 90.0 * Turn, Rs)
		PtCntI2 = PointAlongPlane(PtI2, Axis + 90.0 * Turn, Rs)
		PtCntI3 = PointAlongPlane(PtI3, Axis + 90.0 * Turn, Rs)
		PtCntI4 = PointAlongPlane(PtI4, Axis + 90.0 * Turn, Rs)

		PtN3 = PointAlongPlane(PtI3, Axis + 90.0 * Turn, 2.0 * Rs)
		'Конец вычисления центров спиралей

		'===========HoldingArea Nom Track
		HoldingArea.AddPoint(ptNavPrj) 'PtC
		HoldingArea.Point(0).M = Axis + 180.0

		HoldingArea.AddPoint(PointAlongPlane(ptNavPrj, Axis + 180.0 + 90.0 * Turn, 2.0 * Rs))	'PtG '
		HoldingArea.Point(1).M = Axis

		HoldingArea.AddPoint(PointAlongPlane(HoldingArea.Point(1), Axis, L))
		HoldingArea.Point(2).M = Axis

		HoldingArea.AddPoint(PointAlongPlane(HoldingArea.Point(2), Axis + 90.0 * Turn, 2.0 * Rs))
		HoldingArea.Point(3).M = Axis + 180.0

		HoldingArea.AddPoint(HoldingArea.Point(0))

		'Коней вычисления центров спиралей
		'===========Параметры спиралей
		Dim R0C As Double
		Dim Rad0C As Double
		Dim R0I1 As Double
		Dim Rad0I1 As Double
		Dim R0I2 As Double
		Dim Rad0I2 As Double
		Dim R0I3 As Double
		Dim Rad0I3 As Double
		Dim R0I4 As Double
		Dim Rad0I4 As Double
		Dim RN3 As Double
		Dim R0N4 As Double

		R0C = Rs + 11.0 * w_
		Rad0C = Axis + 90.0 * Turn
		R0I1 = Rs + (t_ + 6.0) * w_ + 4.0 * E45
		Rad0I1 = Axis - 90.0 * Turn
		R0I2 = R0I1 + 14.0 * w_
		Rad0I2 = Rad0I1
		R0I3 = R0I1
		Rad0I3 = Rad0I1
		R0I4 = R0I2
		Rad0I4 = Rad0I1
		RN3 = (t_ + 6.0) * w_ + 8.0 * E45
		'================= Line3 ===========================
		Dim Solution As Integer
		Dim Rad1Tmp As Double
		Dim Rad2Tmp As Double
		Dim R1Tmp As Double
		Dim R2Tmp As Double

		Dim I As Integer
		Dim CntAng1 As Double
		Dim CntAng2 As Double
		Dim TouchRes As Integer

		TouchRes = TouchTo2Spiral(PtG, R0C - Rs, 0.0, 0.0, PtI3, R0I3 - Rs, 0.0, 0.0, -Turn, CntAng1, CntAng2)

		If TouchRes > 0 Then
			Line3.AddPoint(PointAlongPlane(PtG, CntAng1, R0C - Rs))
			Line3.AddPoint(PointAlongPlane(PtI3, CntAng2, R0I3 - Rs))
			Line3.Point(0).M = ReturnAngleInDegrees(Line3.Point(0), Line3.Point(1))
			Line3.Point(1).M = Line3.Point(0).M
		End If

		TouchRes = TouchTo2Spiral(PtI3, R0I3 - Rs, 0.0, 0.0, PtI4, R0I4 - Rs, 0.0, 0.0, -Turn, CntAng1, CntAng2)
		If TouchRes > 0 Then
			I = Line3.PointCount
			Line3.AddPoint(PointAlongPlane(PtI3, CntAng1, R0I3 - Rs))
			Line3.AddPoint(PointAlongPlane(PtI4, CntAng2, R0I4 - Rs))
			Line3.Point(I).M = ReturnAngleInDegrees(Line3.Point(I), Line3.Point(I + 1))
			Line3.Point(I + 1).M = Line3.Point(I).M
		End If

		Line3.AddPoint(PointAlongPlane(PtI4, Axis + 180.0 - 90.0 * Turn, R0I4 - Rs))
		Line3.Point(Line3.PointCount - 1).M = Axis + 180.0
		'=====================================Turn180
		Dim R0B As Double
		Dim Rad0B As Double
		Dim RadBEnd As Double
		Dim RadBstr As Double
		Dim RadCend As Double
		Dim TrackToler As Double
		Dim TouchRad As Double

		R0B = Rs + 5.0 * w_ + 4.0 * E45
		Rad0B = Modulus(Rad0C + 180.0, 360.0)

		Rad1Tmp = ReturnAngleInDegrees(PtCntB, PtCntC)
		Rad2Tmp = Rad1Tmp

		ChangeSpiralStartParam(E, R0C, Rad0C, R1Tmp, Rad1Tmp, Turn, Turn)
		ChangeSpiralStartParam(E, R0B, Rad0B, R2Tmp, Rad2Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, Turn, RadCend, RadBstr)
		'Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, Turn, RadCend, RadBstr)

		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			Return 0
		End If

		Turn180.AddPoint(ptNavPrj)

		R1Tmp = R0C + E * Modulus(Turn * (RadCend - Rad0C), 360.0)
		Rad1Tmp = Modulus(RadCend + Turn * (90.0 - RadToDeg(System.Math.Atan(RadToDeg(E) / R1Tmp))), 360.0)

		If NavType = eNavaidType.NDB Then
			TrackToler = NDB.TrackingTolerance
		Else
			TrackToler = VOR.TrackingTolerance
		End If
		TouchRad = Modulus(Axis + Turn * TrackToler, 360.0)

		'If AnglesSideDef(TouchRad, Rad1Tmp) = Turn Then
		If AnglesSideDef(Rad1Tmp, TouchRad) = Turn Then
			Rad2Tmp = Modulus(Rad0C + 90.0 * Turn, 360.0)
			RadBEnd = SpiralTouchAngle(R0C, E, Rad2Tmp, TouchRad, Turn)
			RadBEnd = Modulus(Rad0C + RadBEnd * Turn, 360.0)
			CreateSpiralBy2Radial(PtCntC, R0C, Rad0C, RadBEnd, E, Turn, Turn180)
		Else
			Rad2Tmp = Modulus(Rad0B + 90.0 * Turn, 360.0)
			RadBEnd = SpiralTouchAngle(R0B, E, Rad2Tmp, TouchRad, Turn)

			RadBEnd = Modulus(Rad0B + RadBEnd * Turn, 360.0)
			CreateSpiralBy2Radial(PtCntC, R0C, Rad0C, RadCend, E, Turn, Turn180)

			If AnglesSideDef(Rad0B, RadBstr) = Turn Then
				R0B = R0B - E * SubtractAngles(Rad0B, RadBstr)
			Else
				R0B = R0B + E * SubtractAngles(Rad0B, RadBstr)
			End If
			CreateSpiralBy2Radial(PtCntB, R0B, RadBstr, RadBEnd, E, Turn, Turn180)
		End If

		'===================================================
		Dim fTmp As Double
		Dim RadCSt As Double
		Dim TurnAng As Double
		Dim RadI1St As Double
		Dim RadI2St As Double
		Dim RadI3St As Double
		Dim RadI4St As Double
		Dim RadN4St As Double
		Dim RadI1End As Double
		Dim RadI2End As Double
		Dim RadI3End As Double
		Dim RadI4End As Double
		Dim RadN4End As Double

		Dim bN3flg As Boolean
		Dim PtSt As ESRI.ArcGIS.Geometry.IPoint

		Dim Ra As Double
		Dim RadaSt As Double
		Dim RadaEnd As Double
		Dim baflg As Boolean

		Rad1Tmp = ReturnAngleInDegrees(PtCntI1, PtCntC) - 45.0 * Turn
		ChangeSpiralStartParam(E, R0I1, Rad0I1, R1Tmp, Rad1Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtCntC, R0C, E, Rad0C, PtCntI1, R1Tmp, E, Rad1Tmp, Turn, RadCend, RadI1St)

		'DrawPolyLine Turn180, 0, 2
		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			'    msgbox "Параметры шаблона ипподрома неправильны"
			Return 0
		End If

		Rad2Tmp = ReturnAngleInDegrees(PtCntI2, PtCntI1)
		ChangeSpiralStartParam(E, R0I2, Rad0I2, R2Tmp, Rad2Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtCntI1, R1Tmp, E, Rad1Tmp, PtCntI2, R2Tmp, E, Rad2Tmp, Turn, RadI1End, RadI2St)
		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			'    msgbox "Параметры шаблона ипподрома неправильны"
			Return 0
		End If

		Rad1Tmp = ReturnAngleInDegrees(PtCntI4, PtCntI2)
		Rad2Tmp = Rad1Tmp
		ChangeSpiralStartParam(E, R0I2, Rad0I2, R1Tmp, Rad1Tmp, Turn, -Turn)
		ChangeSpiralStartParam(E, R0I4, Rad0I4, R2Tmp, Rad2Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtCntI2, R1Tmp, E, Rad1Tmp, PtCntI4, R2Tmp, E, Rad2Tmp, Turn, RadI2End, RadI4St)
		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			'    msgbox "Параметры шаблона ипподрома неправильны"
			Return 0
		End If

		Rad1Tmp = ReturnAngleInDegrees(PtCntI3, PtCntI4)
		Rad2Tmp = Rad1Tmp
		ChangeSpiralStartParam(E, R0I4, Rad0I4, R1Tmp, Rad1Tmp, Turn, -Turn)
		ChangeSpiralStartParam(E, R0I3, Rad0I3, R2Tmp, Rad2Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtCntI4, R1Tmp, E, Rad1Tmp, PtCntI3, R2Tmp, E, Rad2Tmp, Turn, RadI4End, RadI3St)
		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			'    msgbox "Параметры шаблона ипподрома неправильны"
			Return 0
		End If

		TurnAng = Modulus((RadI4End - Rad0I4) * Turn, 360.0)
		R1Tmp = R0I4 + E * TurnAng
		ptTmp = PointAlongPlane(PtCntI4, RadI4End, R1Tmp)
		fTmp = RadToDeg(System.Math.Atan(RadToDeg(E) / R1Tmp))
		Rad1Tmp = RadI4End + (180.0 - fTmp) * Turn
		PtN4_ = PointAlongPlane(ptTmp, Rad1Tmp, R1Tmp - Rs)
		RadN4St = Modulus(Rad1Tmp + 180.0, 360.0)

		R0N4 = R1Tmp - Rs

		Solution = TouchTo2Spiral(PtN4_, R0N4, 0.0, RadN4St, PtN3, RN3, 0.0, Rad2Tmp, Turn, RadN4End, RadI3St)

		bN3flg = Solution <> 0

		If Not bN3flg Then
			PtN3 = PtN4_
			RN3 = R0N4
		End If

		'DrawPoint PtN4_, 0
		'DrawPoint PtN3, 255
		'DrawPolygon CreatePrjCircle(PtN4_, R0N4), 0
		'DrawPolygon CreatePrjCircle(PtN3, RN3), 255

		Rad2Tmp = ReturnAngleInDegrees(PtCntC, PtN3)
		ChangeSpiralStartParam(E, R0C, Rad0C, R2Tmp, Rad2Tmp, Turn, -Turn)
		Solution = TouchTo2Spiral(PtN3, RN3, 0.0, Rad1Tmp, PtCntC, R2Tmp, E, Rad2Tmp, Turn, RadI3End, RadCSt)

		baflg = Solution = 0

		If baflg Then
			RadaSt = Axis + 180.0

			CircleVectorIntersect(PtN3, RN3, ptNavPrj, RadaSt, ptTmp)
			RadI3End = ReturnAngleInDegrees(PtN3, ptTmp)
			Ra = ReturnDistanceInMeters(ptNavPrj, ptTmp)

			Solution = TouchTo2Spiral(ptNavPrj, Ra, 0.0, RadaSt, PtCntC, R2Tmp, E, Rad2Tmp, Turn, RadaEnd, RadCSt)
		End If

		If Solution = 0 Then
			MessageBox.Show("ERROR ON 'HoldingShablon'")
			'    msgbox "Параметры шаблона ипподрома неправильны"
			Return 0
		End If

		If Not bN3flg Then
			RadN4End = RadI3End
		End If

		If AnglesSideDef(RadI3St, RadI3End) = Turn Then
			RadI3St = RadI3End
		End If

		TurnAng = SubtractAnglesWithSign(Rad0C, RadCSt, Turn)
		R1Tmp = R0C + E * TurnAng
		PtSt = PointAlongPlane(PtCntC, RadCSt, R1Tmp)
		CreateSpiralBy2Radial(PtCntC, R1Tmp, RadCSt, RadCend, E, Turn, Shablon)

		TurnAng = SubtractAnglesWithSign(Rad0I1, RadI1St, Turn)
		R1Tmp = R0I1 + E * TurnAng
		CreateSpiralBy2Radial(PtCntI1, R1Tmp, RadI1St, RadI1End, E, Turn, Shablon)

		TurnAng = SubtractAnglesWithSign(Rad0I2, RadI2St, Turn)
		R1Tmp = R0I2 + E * TurnAng
		CreateSpiralBy2Radial(PtCntI2, R1Tmp, RadI2St, RadI2End, E, Turn, Shablon)

		TurnAng = SubtractAnglesWithSign(Rad0I4, RadI4St, Turn)
		R1Tmp = R0I4 + E * TurnAng
		CreateSpiralBy2Radial(PtCntI4, R1Tmp, RadI4St, RadI4End, E, Turn, Shablon)

		CreateSpiralBy2Radial(PtN4_, R0N4, RadN4St, RadN4End, 0.0, Turn, Shablon)

		If bN3flg Then
			CreateSpiralBy2Radial(PtN3, RN3, RadI3St, RadI3End, 0.0, Turn, Shablon)
		End If

		If baflg Then
			CreateSpiralBy2Radial(ptNavPrj, Ra, RadaSt, RadaEnd, 0.0, Turn, Shablon)
		End If

		Shablon.AddPoint(PtSt)

		Dim pTransform2D As ESRI.ArcGIS.Geometry.ITransform2D
		Dim pGeom As ESRI.ArcGIS.Geometry.IGeometry
		Dim ptE As ESRI.ArcGIS.Geometry.IPoint
		Dim dXmin As Double
		Dim dYmin As Double
		Dim dXmax As Double
		Dim dYmax As Double

		pTransform2D = Shablon
		pTransform2D.Rotate(ptNavPrj, DegToRad(-Axis))
		pGeom = Shablon
		pGeom.Envelope.QueryCoords(dXmin, dYmin, dXmax, dYmax)
		ptE = New ESRI.ArcGIS.Geometry.Point

		If Turn < 0 Then
			ptE.PutCoords(dXmax - xe, dYmin + ye)
		Else
			ptE.PutCoords(dXmax - xe, dYmax - ye)
		End If

		Shablon.AddPoint(ptNavPrj)
		Shablon.AddPoint(ptE)

		Shablon.AddPoint(ptNavPrj, 0)
		Shablon.AddPoint(ptE, 0)

		pTransform2D.Rotate(ptNavPrj, DegToRad(Axis))
		Return 1
	End Function

End Module