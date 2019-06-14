Option Strict Off
Option Explicit On
Module Shablons
	
	Function VORFIXTolerArea(ByRef ptVor As ESRI.ArcGIS.Geometry.IPoint, ByRef Aztin As Double, ByRef AbsH As Double, ByRef TolerArea As ESRI.ArcGIS.Geometry.IPointCollection) As Double
		Dim R As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim fTmp As Double
		Dim fTmpH As Double
		
		fTmpH = AbsH - ptVor.Z
		R = fTmpH * System.Math.Tan(DegToRad(VOR.ConeAngle))
		TolerArea = New ESRI.ArcGIS.Geometry.Polygon
		
		ptTmp = PointAlongPlane(ptVor, Aztin - (90# + VOR.TrackAccuracy), VOR.LateralDeviationCoef * fTmpH)
		fTmp = CircleVectorIntersect(ptVor, R, ptTmp, Aztin - VOR.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		fTmp = CircleVectorIntersect(ptVor, R, ptTmp, 180# + Aztin - VOR.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		ptTmp = PointAlongPlane(ptVor, Aztin + 90# + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH)
		fTmp = CircleVectorIntersect(ptVor, R, ptTmp, 180# + Aztin + VOR.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		
		fTmp = CircleVectorIntersect(ptVor, R, ptTmp, Aztin + VOR.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		
		VORFIXTolerArea = R
	End Function
	
	Function NDBFIXTolerArea(ByRef ptNDB As ESRI.ArcGIS.Geometry.IPoint, ByRef Aztin As Double, ByRef AbsH As Double, ByRef TolerArea As ESRI.ArcGIS.Geometry.IPointCollection) As Double
		Dim R As Double
		Dim qN As Double
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		Dim fTmp As Double
		Dim fTmpH As Double
		
		fTmpH = AbsH - ptNDB.Z
		R = fTmpH * System.Math.Tan(DegToRad(NDB.ConeAngle))
		
		TolerArea = New ESRI.ArcGIS.Geometry.Polygon
		
		qN = R * System.Math.Sin(DegToRad(NDB.Entry2ConeAccuracy))
		ptTmp = PointAlongPlane(ptNDB, Aztin - 90#, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(DegToRad(NDB.TrackAccuracy)))
		fTmp = CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - NDB.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		fTmp = CircleVectorIntersect(ptNDB, R, ptTmp, 180# + Aztin - NDB.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		ptTmp = PointAlongPlane(ptNDB, Aztin + 90#, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(DegToRad(NDB.TrackAccuracy)))
		fTmp = CircleVectorIntersect(ptNDB, R, ptTmp, 180# + Aztin + NDB.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		fTmp = CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + NDB.TrackAccuracy, ptCur)
		TolerArea.AddPoint(ptCur)
		NDBFIXTolerArea = R
	End Function
	
	Sub ChangeSpiralStartParam(ByRef E As Double, ByRef StR0 As Double, ByRef StRadial As Double, ByRef NewStR0 As Double, ByRef NewStRadial As Double, ByRef Turn As Integer, ByRef turnChange As Integer)
		Dim TurnAng As Double
		
		TurnAng = Modulus((NewStRadial - StRadial) * turnChange, 360#)
		NewStR0 = StR0 + E * TurnAng * Turn * turnChange
		If NewStR0 < 0# Then
			TurnAng = StR0 / E
			NewStR0 = 0#
			NewStRadial = Modulus(TurnAng * turnChange + StRadial, 360#)
		End If
	End Sub
	
	Public Function SpiralTouchToPoint(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef E As Double, ByRef r0 As Double, ByRef AztStRad As Double, ByRef Turn As Integer, ByRef pPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef SpiralIntercept As Integer, ByRef SolPnt As ESRI.ArcGIS.Geometry.IPoint) As Integer
        Dim DistToPnt As Double
		Dim AztToPnt As Double
        Dim SpAngle As Double
        Dim fTmp As Double
		Dim DegE As Double
		Dim dPhi As Double
		Dim Xsp As Double
		Dim Ysp As Double
		Dim Phi As Double
		Dim R As Double
		
        Dim I As Integer
		'if SpiralIntercept = 1 Point to spiral else spiral to Point
		
		AztToPnt = ReturnAngleInDegrees(ptCnt, pPnt)
		DistToPnt = ReturnDistanceInMeters(ptCnt, pPnt)
		DegE = RadToDeg(E)
		
		'DrawPoint pPnt, 0
		
		Phi = Modulus((AztToPnt - AztStRad) * Turn, 360#)
		
		If (Phi > 180#) And (r0 = 0#) Then
			Phi = Phi - 360#
		End If
		
		R = r0 + E * Phi
		
		If System.Math.Abs(R - DistToPnt) < distEps Then
			SpiralTouchToPoint = 1
			SolPnt = pPnt
			Exit Function
		ElseIf R > DistToPnt Then 
			SpiralTouchToPoint = 0
			Exit Function
		End If
		
		fTmp = Modulus(AztToPnt + 90# * (1 + SpiralIntercept), 360#)
		'Set ptSp = New Point
		
		For I = 0 To 30
			Phi = fTmp
			SpAngle = SpiralTouchAngle(r0, E, AztStRad + 90# * Turn, Phi, Turn)
			R = r0 + E * SpAngle
			fTmp = Modulus(AztStRad + SpAngle * Turn, 360#)
			'Set ptSp = PointAlongPlane(PtCnt, fTmp, R)
			'DrawPoint ptSp, 255
			
			fTmp = DegToRad(fTmp)
			Xsp = ptCnt.X + R * System.Math.Cos(fTmp)
			Ysp = ptCnt.Y + R * System.Math.Sin(fTmp)
			fTmp = RadToDeg(Math.Atan2(pPnt.Y - Ysp, pPnt.X - Xsp))
			fTmp = Modulus(fTmp + 90# * (1 + SpiralIntercept), 360#)
			
			dPhi = SubtractAngles(Phi, fTmp)
			
			'ptSp.PutCoords Xsp, Ysp
			'DrawPoint ptSp, RGB(0, 0, 255)
			
			If dPhi < degEps Then
				SolPnt = New ESRI.ArcGIS.Geometry.Point
				SolPnt.PutCoords(Xsp, Ysp)
				SpiralTouchToPoint = 1
				Exit Function
			End If
		Next I
		
		SpiralTouchToPoint = 0
		
	End Function
	
	Public Function SpiralTouchToFix(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef E As Double, ByRef r0 As Double, ByRef AztStRad As Double, ByRef Turn As Integer, ByRef FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef SpiralIntercept As Integer, ByRef Axis As Double) As Double
        Dim AztToFix As Double
		Dim DistToFix As Double
		Dim Phi As Double
		Dim Phi0 As Double
		Dim R As Double
		Dim fTmp As Double
		Dim I As Integer
		Dim J As Integer

		Dim AztEndRad0 As Double

		Dim F As Double
		Dim f_ As Double

		Dim Xa As Double
		Dim Ya As Double
		Dim SinA As Double
		Dim CosA As Double

		Dim RadE As Double
		Dim SpAngle As Double
		Dim Xsp As Double
		Dim Ysp As Double
        Dim dPhi As Double
		
		'if SpiralIntercept = 1 FIX to spiral else spiral to FIX
		AztToFix = ReturnAngleInDegrees(ptCnt, FixPnt)
		DistToFix = ReturnDistanceInMeters(ptCnt, FixPnt)
		AztEndRad0 = AztToFix
		
		'SpiralTouchToFix = 0#
		'Phi0 = Modulus(AztToFix, 360#)
		
		'Phi0 = Modulus(AztToFix + Turn * SpiralIntercept * 90#, 360#)
		fTmp = Modulus((AztToFix - AztStRad) * Turn, 360#)
		
		If (fTmp > 180#) And (r0 = 0#) Then
			fTmp = fTmp - 360#
		End If
		
		R = r0 + E * fTmp '* turn
		If System.Math.Abs(R - DistToFix) < distEps Then
			SpiralTouchToFix = AztToFix
			Exit Function
		End If
		

		If R < DistToFix Then
			RadE = RadToDeg(E)
			Phi0 = Modulus(AztToFix + 90# * (1 + SpiralIntercept), 360#)
			
			For I = 0 To 30
				Phi = Phi0
				SpAngle = SpiralTouchAngle(r0, E, AztStRad + 90# * Turn, Phi, Turn)
				R = r0 + E * SpAngle
				
				'Set SolPnt = PointAlongPlane(PtCnt, Phi, R)
				'DrawPoint SolPnt, 255
				
				SpiralTouchToFix = Modulus(AztStRad + SpAngle * Turn, 360#)
				
				fTmp = DegToRad(SpiralTouchToFix)
				Xsp = ptCnt.X + R * System.Math.Cos(fTmp)
				Ysp = ptCnt.Y + R * System.Math.Sin(fTmp)
				fTmp = RadToDeg(Math.Atan2(FixPnt.Y - Ysp, FixPnt.X - Xsp))
				
				Phi0 = Modulus(fTmp + 90# * (1 + SpiralIntercept), 360#)
				
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
			CosA = System.Math.Cos(DegToRad(Axis + 90# * Turn))
			SinA = System.Math.Sin(DegToRad(Axis + 90# * Turn))
			Xa = DistToFix * System.Math.Cos(DegToRad(AztToFix))
			Ya = DistToFix * System.Math.Sin(DegToRad(AztToFix))
			Phi0 = AztToFix
			For J = 0 To 20
				fTmp = Modulus((Phi0 - AztStRad) * Turn, 360#)
				R = r0 + E * fTmp '* turn
				F = R * (System.Math.Sin(DegToRad(Phi0)) * CosA - System.Math.Cos(DegToRad(Phi0)) * SinA) + Xa * SinA - Ya * CosA
				f_ = RadToDeg(E) * Turn * (System.Math.Sin(DegToRad(Phi0)) * CosA - System.Math.Cos(DegToRad(Phi0)) * SinA) + R * (System.Math.Cos(DegToRad(Phi0)) * CosA + System.Math.Sin(DegToRad(Phi0)) * SinA)
				Phi = Phi0 - RadToDeg(F / f_)
				If System.Math.Abs(System.Math.Sin(DegToRad(Phi - Phi0))) < 0.001 Then
					SpiralTouchToFix = Modulus(Phi, 360#)
					Exit Function
				Else
					Phi0 = Phi
				End If
			Next J
		End If
		
		SpiralTouchToFix = Modulus(Phi, 360#)
	End Function
	
	Sub CreateSpiralBy2Radial(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef r0 As Double, ByRef AztStRad As Double, ByRef AztEndRad As Double, ByRef E As Double, ByRef Turn As Integer, ByRef pPointCollection As ESRI.ArcGIS.Geometry.IPointCollection)
		Dim dAlpha As Double
		Dim TurnAng As Double
		Dim R As Double
		Dim N As Integer
		Dim I As Integer
		Dim ptCur As ESRI.ArcGIS.Geometry.IPoint
		
		TurnAng = Modulus((AztEndRad - AztStRad) * Turn, 360#)
		dAlpha = 1#
		N = TurnAng / dAlpha
		If (N < 10) Then
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
	'Dim pPt1 As IPoint
	'Dim pPt2 As IPoint
	'Dim AztTouch As Double
	'Dim AztTouch1 As Double
	'Dim TurnAngle As Double
	'
	'AztO1O2 = ReturnAngleInDegrees(PtCnt1, PtCnt2)
	'DistO1O2 = ReturnDistanceInMeters(PtCnt1, PtCnt2)
	'phi10 = Modulus(AztO1O2 - 90# * Turn, 360#)
	'For J = 0 To 20
	'    fTmp = Modulus((phi10 - AztSt1) * Turn, 360#)
	'    R1 = r10 + e1 * fTmp
	'    AztR1E = RadToDeg(Atn(RadToDeg(e1 * Turn) / R1))
	'    phi20 = phi10
	'    For I = 0 To 20
	'        fTmp = Modulus((phi20 - AztSt2) * Turn, 360#)
	'        R2 = r20 + E2 * fTmp
	'        AztR2E = RadToDeg(Atn(RadToDeg(E2 * Turn) / R2))
	'        f = phi20 - phi10 + AztR1E - AztR2E
	'        fTmp = RadToDeg(E2) * RadToDeg(E2)
	'        f_ = 1# + fTmp / (R2 * R2 + fTmp)
	'        phi2 = phi20 - f / f_
	'        If Abs(Sin(DegToRad(f / f_))) < degEps Then
	'            Exit For
	'        Else
	'            phi20 = phi2
	'        End If
	'    Next I
	'    phi2 = Modulus(phi2, 360#)
	'    fTmp = (R1 * Cos(DegToRad(AztR1E)) - R2 * Cos(DegToRad(AztR2E))) / DistO1O2
	'    If Abs(fTmp) > 1 Then
	'        TouchTo2Spiral = 0
	'        Exit Function
	'    End If
	'    f = phi10 - AztO1O2 - AztR1E + Turn * RadToDeg(ArcCos(fTmp))
	'    f_ = 1# - Turn * RadToDeg(e1 * Cos(DegToRad(AztR1E)) - E2 * Cos(DegToRad(AztR2E))) / DistO1O2
	'    phi1 = phi10 - f / f_
	'    phi1 = Modulus(phi1, 360#)
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
		Dim Distance As Double
		Dim phi1 As Double
		Dim phi2 As Double
		Dim phi10 As Double
		Dim phi20 As Double
		Dim AztR1E As Double
		Dim AztR2E As Double
		Dim AztO1O2 As Double
		Dim R1 As Double
		Dim R2 As Double
		Dim F As Double
		Dim f_ As Double
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim fTmp As Double
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim bOutOfSpiral As Boolean
		
		AztO1O2 = ReturnAngleInDegrees(PtCnt1, PtCnt2)
		
		bOutOfSpiral = False
		
		For K = 0 To 10
			phi10 = Modulus(AztO1O2 - (90# + 10# * K) * Turn, 360#)
			fTmp = Modulus((phi10 - AztSt1) * Turn, 360#)
			R1 = r10 + E1 * fTmp
			pPt1 = PointAlongPlane(PtCnt1, phi10, R1)
			
			phi20 = ReturnAngleInDegrees(PtCnt2, pPt1)
			Distance = ReturnDistanceInMeters(PtCnt2, pPt1)
			fTmp = Modulus((phi20 - AztSt2) * Turn, 360#)
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
			fTmp = Modulus((phi10 - AztSt1) * Turn, 360#)
			R1 = r10 + E1 * fTmp
			AztR1E = RadToDeg(System.Math.Atan(RadToDeg(E1 * Turn) / R1))
			
			For I = 0 To 20
				fTmp = Modulus((phi20 - AztSt2) * Turn, 360#)
				R2 = r20 + E2 * fTmp
				AztR2E = RadToDeg(System.Math.Atan(RadToDeg(E2 * Turn) / R2))
				F = phi20 - phi10 + AztR1E - AztR2E
				fTmp = RadToDeg(E2) * RadToDeg(E2)
				f_ = 1# + fTmp / (R2 * R2 + fTmp)
				phi2 = phi20 - F / f_
				
				If System.Math.Abs(System.Math.Sin(DegToRad(F / f_))) < degEps Then
					Exit For
				Else
					phi20 = phi2
				End If
			Next I
			
			pPt1 = PointAlongPlane(PtCnt1, phi1, R1)
			pPt2 = PointAlongPlane(PtCnt2, phi2, R2)
			fTmp = ReturnAngleInDegrees(pPt1, pPt2)
			fTmp = SubtractAnglesWithSign(phi1 + (90# * Turn - AztR1E), fTmp, Turn)
			phi1 = phi10 + fTmp * Turn
			If System.Math.Abs(fTmp) < (degEps * 50) Then
				CntAngle1 = Modulus(phi1, 360#)
				CntAngle2 = Modulus(phi2, 360#)
				TouchTo2Spiral = 1
				Exit Function
			Else
				phi10 = phi1
			End If
		Next J
		
		TouchTo2Spiral = 0
		
	End Function
	
	Public Function CreateReckoningToleranceArea(ByVal t As Double, ByVal Axis As Double, ByVal hAbs As Double, ByVal IAS As Double, ByVal dISA As Double, ByVal ptNav As ESRI.ArcGIS.Geometry.IPoint, ByVal NavType As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
		Dim A As Double
		Dim V As Double
		Dim w As Double
		Dim OA As Double
		Dim OA1 As Double
		Dim OA2 As Double
		Dim ConeAngle As Double
		Dim TrackingTolerance As Double
		
		If NavType = 0 Then
			ConeAngle = VOR.ConeAngle
			TrackingTolerance = VOR.TrackingTolerance
		Else
			ConeAngle = NDB.ConeAngle
			TrackingTolerance = NDB.TrackingTolerance
		End If
		
		A = System.Math.Tan(DegToRad(ConeAngle)) * (hAbs - ptNav.Z)
		
		V = 0.277777777777778 * IAS2TAS(IAS, hAbs, dISA)
		w = 0.277777777777778 * (0.012 * hAbs + 87#)
		
		'T = T + 16#
		
		OA = V * t
		OA1 = (V - w) * (t - 10#) - A
		OA2 = (V + w) * (t + 10#) + A
		
		CreateReckoningToleranceArea = New ESRI.ArcGIS.Geometry.Multipoint
		
		'DrawPoint ptNav, 255
		
		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(ptNav, Axis - TrackingTolerance, OA2))
		'DrawPoint CreateReckoningToleranceArea.Point(0), 0
		
		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(ptNav, Axis - TrackingTolerance, OA1))
		'DrawPoint CreateReckoningToleranceArea.Point(1), 0
		
		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(ptNav, Axis + TrackingTolerance, OA1))
		'DrawPoint CreateReckoningToleranceArea.Point(2), 0
		
		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(ptNav, Axis + TrackingTolerance, OA2))
		'DrawPoint CreateReckoningToleranceArea.Point(3), 0
		
		CreateReckoningToleranceArea.AddPoint(PointAlongPlane(ptNav, Axis, OA))
		'DrawPoint CreateReckoningToleranceArea.Point(4), 0
		
	End Function
	
	Function Shablon80_260(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal Axis As Double, ByVal Bank As Double, ByVal Turn As Integer, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef ptTurn80_260 As ESRI.ArcGIS.Geometry.IPointCollection) As Integer
		
		Dim K As Double ' 1
		Dim V As Double ' 2
		Dim v3600 As Double ' 3
		Dim R As Double ' 4
		Dim Rv As Double ' 5
		Dim h As Double ' 6
		Dim w As Double ' 7
		Dim w_ As Double ' 8
		Dim E As Double ' 9
		Dim ab As Double '10
		Dim de As Double '11 = d1e1 = d2e2
		Shablon80_260 = 0
		
		K = IAS2TAS(IAS, AbsH, dISA)
		V = K '* IAS
		v3600 = V / 3.6
		'R = 943.27 / V
		R = 6355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
		If (R > 3#) Then R = 3#
		
		Rv = 1000# * V / (62.83 * R)
		h = AbsH / 1000#
		w = 12 * h + 87
		w_ = w / 3.6
		E = w_ / R
		ab = 5# * v3600
		
		de = 10# * v3600
		
		Dim PtStart As ESRI.ArcGIS.Geometry.IPoint
		PtStart = ptNavPrj
		
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptCnt As ESRI.ArcGIS.Geometry.IPoint
		Dim PtB As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptE As ESRI.ArcGIS.Geometry.IPoint
		PtB = PointAlongPlane(PtStart, Axis, ab) 'Point b
		
		ptCnt = PointAlongPlane(PtB, Axis - Turn * 90#, Rv)
		ptD = PointAlongPlane(ptCnt, Axis + Turn * (90# - 80#), Rv) 'Point D
		ptE = PointAlongPlane(ptD, Axis - Turn * 80#, de) 'Point e
		
		'Control Points
		Dim ptD1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD2 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtE1 As ESRI.ArcGIS.Geometry.IPoint 'd3
		Dim PtE2 As ESRI.ArcGIS.Geometry.IPoint 'd4
		Dim Aztin As Double
		
		Aztin = Axis - Turn * 80#
		ptD1 = PointAlongPlane(ptCnt, Axis + Turn * (90# - 80# + 5#), Rv) 'Point d1
		ptD2 = PointAlongPlane(ptCnt, Axis + Turn * (90# - 80# - 5#), Rv) 'Point d2
		PtE1 = PointAlongPlane(ptD1, Axis + Turn * (90# - 80# + 5#) - Turn * 90#, de) 'Point e1
		PtE2 = PointAlongPlane(ptD2, Axis + Turn * (90# - 80# - 5#) - Turn * 90#, de) 'Point e2
		
		Dim PtF1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtF2 As ESRI.ArcGIS.Geometry.IPoint
		PtF1 = PointAlongPlane(PtE1, Axis + Turn * (90# - 80# + 5#), Rv) 'Point f1
		PtF2 = PointAlongPlane(PtE2, Axis + Turn * (90# - 80# - 5#), Rv) 'Point f2
		'=========================== Traektory ================================
		'Dim ptTmp As IPoint
		Dim fTmp As Double
		
		ptTurn80_260 = New ESRI.ArcGIS.Geometry.Multipoint
		
		ptTmp = New ESRI.ArcGIS.Geometry.Point
		ptTmp.PutCoords(ptNavPrj.X, ptNavPrj.Y)
		
		ptTurn80_260.AddPoint(ptTmp)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis
		
		ptTurn80_260.AddPoint(PtB)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis
		
		'ptTurn80_260.AddPoint PtC
		'ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = axis - Turn * 45#
		
		ptTurn80_260.AddPoint(ptD)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis - Turn * 80#
		
		'=============Adding 29.11.2004
		ptTurn80_260.AddPoint(ptE)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis - Turn * 80#
		
        ptTmp = LineLineIntersect(PtB, Axis, ptE, Axis - Turn * 80.0#)
		fTmp = ReturnDistanceInMeters(ptTmp, ptE)
		'====================End Adding=============
		'============Comment 29.11.2004
		'Set ptTmp = LineLineIntersect(PtB, Axis, ptD, Axis - Turn * 80#)
		'fTmp = ReturnDistanceInMeters(ptTmp, ptD)
		'=============End Comment========
		
		ptTurn80_260.AddPoint(PointAlongPlane(ptTmp, Axis, fTmp))
		
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis + 180#
		
		'fTmp = Atn(DegToRad(22.5)) * fTmp
		
		'DrawPoint ptTmp, 255
		
		ptTurn80_260.AddPoint(ptTmp)
		ptTurn80_260.Point(ptTurn80_260.PointCount - 1).M = Axis + 180#
		
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
		
		AztAF = ReturnAngleInDegrees(ptNavPrj, PtF2)
		RF2St = Rv + 15# * w_ + 85# * E
		AztF2St = ReturnAngleInDegrees(PtF2, PtE2)
		RF1St = Rv + 15# * w_ + 75# * E
		AztF1St = ReturnAngleInDegrees(PtF1, PtE1)
		
		ChangeSpiralStartParam(E, RF2St, AztF2St, Rtmp, AztAF, Turn, -Turn)
		
		AztFSt = SpiralTouchToFix(PtF2, E, Rtmp, AztAF, Turn, ptNavPrj, 1, Axis)
		AztF2F1 = ReturnAngleInDegrees(PtF2, PtF1)
		
		fTmp = AztF2F1
		ChangeSpiralStartParam(E, RF2St, AztF2St, Rtmp1, fTmp, Turn, -Turn)
		'Rtmp1 = Rtmp
		ChangeSpiralStartParam(E, RF1St, AztF1St, Rtmp, AztF2F1, Turn, -Turn)
		
		Solution = TouchTo2Spiral(PtF2, Rtmp1, E, fTmp, PtF1, Rtmp, E, AztF2F1, Turn, Rad2Touch1, Rad2Touch2)
		If Solution = 0 Then
			MsgBox("Парамеры шаблона 80 - 260 неправильны")
			Exit Function
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
		TurnAng = Modulus((Rad2Touch2 - AztF1St) * Turn, 360#)
		RStNew = RF1St + TurnAng * E
		CreateSpiralBy2Radial(PtF1, RStNew, Rad2Touch2, AztFEnd, E, Turn, Shablon)
		'DrawPolygon Shablon, 255
		'DrawPolygon Shablon, 0
		
		Shablon.AddPoint(ptNavPrj)
		'DrawPolygon Shablon, 0
		
		Shablon80_260 = 1
	End Function
	
	Function Shablon45_180(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal Axis As Double, ByVal Bank As Double, ByVal Turn As Integer, ByVal Cat As Integer, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef ptTurn45_180 As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Ix As Integer) As Integer
		
		'Вычисление требуемых параметров
		Dim K As Double ' 1
		Dim V As Double ' 2
		Dim v3600 As Double ' 3
		Dim R As Double ' 4
		Dim Rv As Double ' 5
		Dim h As Double ' 6
		Dim w As Double ' 7
		Dim w_ As Double ' 8
		Dim E As Double ' 9
		Dim ab As Double '10
		Dim cd As Double '10
		Dim cd1 As Double '10
		Dim cd2 As Double '10

		Shablon45_180 = 0
		
		K = IAS2TAS(IAS, AbsH, dISA)
		V = K '* IAS
		v3600 = V / 3600#
		
		'R = 943.27 / V
		R = 6355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
		If (R > 3#) Then R = 3#
		Rv = 1000# * V / (62.83 * R)
		h = AbsH / 1000#
		w = 12 * h + 87
		w_ = w / 3.6
		E = w_ / R
		ab = 5# * v3600 * 1000#
		
		'====================
        Dim PtTmp0 As ESRI.ArcGIS.Geometry.IPoint
        Dim fTmp As Double
		
		v3600 = v3600 * 1000#
		cd = (arT45_180.Values(Cat) - 5# - 45# / R) * v3600
		cd1 = cd - 5# * v3600
		'cd3 = cd1
		cd2 = cd + 15# * v3600
		'cd4 = cd2
		Dim PtB As ESRI.ArcGIS.Geometry.IPoint
		Dim PtC As ESRI.ArcGIS.Geometry.IPoint
		Dim ptD As ESRI.ArcGIS.Geometry.IPoint

		PtB = PointAlongPlane(ptNavPrj, Axis, ab) 'Point b
		PtTmp0 = PointAlongPlane(PtB, Axis - Turn * 90#, Rv)
		
		PtC = PointAlongPlane(PtTmp0, Axis + Turn * 45#, Rv) 'Point c
		ptD = PointAlongPlane(PtC, Axis - Turn * 45#, cd) 'Point D
		
		'=========================== Traektory ================================
		
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		
		ptTurn45_180 = New ESRI.ArcGIS.Geometry.Multipoint
		
		ptTmp = New ESRI.ArcGIS.Geometry.Point
		ptTmp.PutCoords(ptNavPrj.X, ptNavPrj.Y)
		
		ptTurn45_180.AddPoint(ptTmp)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis
		
		ptTurn45_180.AddPoint(PtB)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis
		
		ptTurn45_180.AddPoint(PtC)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis - Turn * 45#
		
		ptTurn45_180.AddPoint(ptD)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis - Turn * 45#
		
        ptTmp = LineLineIntersect(PtB, Axis, PtC, Axis - Turn * 45.0#)
		fTmp = ReturnDistanceInMeters(ptTmp, ptD)
		
		ptTurn45_180.AddPoint(PointAlongPlane(ptTmp, Axis, fTmp))
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis + 180#
		
		'fTmp = Atn(DegToRad(22.5)) * fTmp
		
		ptTurn45_180.AddPoint(ptTmp)
		ptTurn45_180.Point(ptTurn45_180.PointCount - 1).M = Axis + 180#
		
		
		Shablon45_180 = 1
		Exit Function
		
		
		
		
		''''''For fTmp = 0 To ptTurn45_180.PointCount - 1
		''''''    DrawPoint ptTurn45_180.Point(fTmp), 0
		''''''Next fTmp
		''''''======================================================================
		''''''Control Points
		'''''Dim ptD1 As IPoint
		'''''Dim ptD2 As IPoint
		'''''Dim PtD3 As IPoint
		'''''Dim PtD4 As IPoint
		'''''Dim Aztin As Double
		'''''Aztin = Axis - Turn * 45#
		'''''Set ptD1 = PointAlongPlane(PtC, Aztin - Turn * 5#, cd1) 'Point d1
		'''''Set PtD3 = PointAlongPlane(PtC, Aztin + Turn * 5#, cd1) 'Point d3
		'''''Set ptD2 = PointAlongPlane(PtC, Aztin - Turn * 5#, cd2) 'Point d2
		'''''Set PtD4 = PointAlongPlane(PtC, Aztin + Turn * 5#, cd2) 'Point d4
		'''''
		'''''Set PtE2 = PointAlongPlane(ptD2, Aztin + 90# * Turn, Rv) 'Point e2
		'''''Set PtE3 = PointAlongPlane(PtD3, Aztin + 90# * Turn, Rv) 'Point e3
		'''''Set PtE4 = PointAlongPlane(PtD4, Aztin + 90# * Turn, Rv) 'Point e4
		''''''Radius
		'''''Dim Wc As Double
		'''''Dim We2 As Double
		'''''Dim We3 As Double
		'''''Dim We4 As Double
		'''''Wc = 5 * w_ + 45 * E
		'''''We2 = Rv + (arT45_180.Values(Cat) + 15#) * w_
		'''''We3 = Rv + (arT45_180.Values(Cat) - 5#) * w_
		'''''We4 = We2
		'''''Dim aztWc As Double
		'''''Dim aztWe2 As Double
		'''''Dim aztWe3 As Double
		'''''Dim aztWe4 As Double
		'''''aztWc = Modulus(Axis + 180# + 45# * Turn, 360#)
		'''''aztWe2 = Modulus(Aztin + 90# * Turn + 180#, 360#)
		'''''aztWe3 = aztWe2
		'''''aztWe4 = aztWe2
		'''''
		'''''Dim Rad2Touch0 As Double
		'''''Dim Rad2Touch1 As Double
		'''''Dim Rad2Touch2 As Double
		'''''Dim Solution As Long
		'''''Dim AztAc As Double
		'''''Dim Rtmp As Double
		'''''Dim Rtmp1 As Double
		'''''Dim Ec As Double
		'''''
		'''''Ec = E
		'''''AztAc = ReturnAngleInDegrees(ptNavPrj, PtC)
		'''''Set Shablon = New Polygon
		'''''Shablon.AddPoint ptNavPrj
		'''''
		''''''Rad2Touch0 = SpiralTouchToFix(PtC, Ec, Rtmp, AztAc, Turn, ptNavPrj, 1, Axis)
		'''''
		'''''AztAc = ReturnAngleInDegrees(PtC, PtE2)
		'''''fTmp = AztAc
		'''''
		'''''ChangeSpiralStartParam Ec, Wc, aztWc, Rtmp1, fTmp, Turn, -Turn
		'''''Wc = Rtmp1
		'''''aztWc = fTmp
		''''''ChangeSpiralStartParam Ec, Wc, aztWc, Rtmp, AztAc, Turn, -Turn
		''''''Wc = Rtmp
		''''''aztWc = AztAc
		'''''Rad2Touch0 = SpiralTouchToFix(PtC, Ec, Wc, aztWc, Turn, ptNavPrj, 1, Axis)
		'''''
		''''''Set SmallSpir = New Polygon
		''''''CreateSpiralBy2Radial PtC, Rtmp1, fTmp, fTmp + 355# * Turn, Ec, Turn, SmallSpir
		''''''DrawPolygon SmallSpir, RGB(0, 0, 255)
		''''''DrawPoint PtC, 0
		'''''
		''''''DrawPolygon CreatePrjCircle(PtC, Wc), RGB(0, 0, 255)
		'''''''DrawPoint PtC, 0
		'''''
		'''''ChangeSpiralStartParam E, We2, aztWe2, Rtmp, AztAc, Turn, -Turn
		'''''
		''''''Set SmallSpir = New Polygon
		''''''CreateSpiralBy2Radial PtE2, Rtmp, AztAc, AztAc + 250# * Turn, E, Turn, SmallSpir
		''''''DrawPolygon SmallSpir, RGB(0, 0, 255)
		''''''DrawPoint PtE2, 0
		'''''
		'''''''DrawPolygon CreatePrjCircle(PtE2, We2), RGB(0, 0, 255)
		'''''''DrawPoint PtE2, 0
		'''''
		''''''Solution = TouchTo2Spiral(PtC, Wc, Ec, aztWc, PtE2, We2, E, aztWe2, Turn, Rad2Touch1, Rad2Touch2)
		'''''Solution = TouchTo2Spiral(PtC, Rtmp1, Ec, fTmp, PtE2, Rtmp, E, AztAc, Turn, Rad2Touch1, Rad2Touch2)
		'''''If Solution = 0 Then
		'''''    AztAc = ReturnAngleInDegrees(ptNavPrj, PtE2)
		'''''    ChangeSpiralStartParam E, We2, aztWe2, Rtmp, AztAc, Turn, -Turn
		'''''    Rad2Touch2 = SpiralTouchToFix(PtE2, E, We2, aztWe2, Turn, ptNavPrj, 1, Axis)
		''''''    Rad2Touch2 = SpiralTouchToFix(PtE2, E, Rtmp, AztAc, Turn, ptNavPrj, 1, Axis)
		'''''
		''''''    MsgBox "Парамеры шаблона 45 - 180 неправильны"
		''''''    Exit Function
		'''''
		''''''Set SmallSpir = New Polygon
		''''''CreateSpiralBy2Radial PtC, Wc, aztWc, aztWc + 120# * Turn, Ec, Turn, SmallSpir
		''''''DrawPolygon SmallSpir, 0
		''''''
		''''''Set SmallSpir = New Polygon
		''''''CreateSpiralBy2Radial PtE2, We2, aztWe2, aztWe2 + 120# * Turn, E, Turn, SmallSpir
		''''''DrawPolygon SmallSpir, 0
		'''''Else
		'''''    TurnAng = SubtractAnglesWithSign(Rad2Touch0, Rad2Touch1, Turn)
		'''''
		'''''    If TurnAng < 0 Then
		'''''        TurnAng = SubtractAnglesWithSign(aztWc, Rad2Touch0, Turn)
		'''''        Set PtTmp0 = PointAlongPlane(PtC, Rad2Touch0, Wc + Ec * TurnAng)
		'''''
		'''''        TouchAC = ReturnAngleInDegrees(ptNavPrj, PtTmp0)
		'''''        TurnAng = SubtractAnglesWithSign(aztWc, Rad2Touch1, Turn)
		'''''        Set PtTmp1 = PointAlongPlane(PtC, Rad2Touch1, Wc + Ec * TurnAng)
		'''''
		'''''        TurnAng = SubtractAnglesWithSign(aztWe2, Rad2Touch2, Turn)
		'''''        Set PtTmp2 = PointAlongPlane(PtE2, Rad2Touch2, We2 + E * TurnAng) '* turn)
		''''''DrawPoint PtTmp0, 0
		''''''DrawPoint PtTmp1, 255
		''''''DrawPoint PtTmp2, 255# * 256
		'''''
		'''''        fTmp = ReturnDistanceInMeters(PtTmp0, PtTmp1)
		'''''        If fTmp > distEps Then
		'''''            TouchCE2 = ReturnAngleInDegrees(PtTmp1, PtTmp2)
		'''''            Set ptInter = LineLineIntersect(ptNavPrj, TouchAC, PtTmp1, TouchCE2)
		'''''            Shablon.AddPoint ptInter
		''''''DrawPoint ptInter, 255# * 256 * 256
		'''''        Else
		'''''            Shablon.AddPoint PtTmp0
		'''''        End If
		''''''DrawPoint PtC, (255# * 256 + 255#) * 256
		'''''    Else
		'''''        fTmp = SubtractAnglesWithSign(aztWc, Rad2Touch0, Turn)
		'''''        If fTmp < 0 Then
		'''''            ChangeSpiralStartParam E, Wc, aztWc, fTmp, Rad2Touch0, Turn, -Turn
		'''''        Else
		'''''            ChangeSpiralStartParam E, Wc, aztWc, fTmp, Rad2Touch0, Turn, Turn
		'''''        End If
		''''''        CreateSpiralBy2Radial PtC, Wc, aztWc, Rad2Touch1, E, Turn, Shablon
		'''''        CreateSpiralBy2Radial PtC, fTmp, Rad2Touch0, Rad2Touch1, E, Turn, Shablon
		'''''    End If
		'''''End If
		'''''
		'''''Ix = Shablon.PointCount
		'''''
		'''''Solution = TouchTo2Spiral(PtE2, We2, E, aztWe2, PtE4, We4, E, aztWe4, Turn, Rad2Touch0, Rad2Touch1)
		'''''If Solution = 0 Then
		'''''    MsgBox "Парамеры шаблона 45 - 180 неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		''''''fTmp = ChangeSpiralStartParam(E, We2, aztWe2, Rad2Touch2, turn, -turn)
		'''''fTmp = SubtractAnglesWithSign(aztWe2, Rad2Touch2, Turn)
		'''''If fTmp < 0 Then
		'''''    ChangeSpiralStartParam E, We2, aztWe2, fTmp, Rad2Touch2, Turn, -Turn
		'''''Else
		'''''    ChangeSpiralStartParam E, We2, aztWe2, fTmp, Rad2Touch2, Turn, Turn
		'''''End If
		'''''CreateSpiralBy2Radial PtE2, fTmp, Rad2Touch2, Rad2Touch0, E, Turn, Shablon
		''''''DrawPolygon Shablon, 255
		''''''DrawPoint PtC, 0
		''''''DrawPoint PtE2, 255
		'''''Solution = TouchTo2Spiral(PtE4, We4, E, aztWe4, PtE3, We3, E, aztWe3, Turn, Rad2Touch0, Rad2Touch2)
		'''''If Solution = 0 Then
		'''''    MsgBox "Парамеры шаблона 45 - 180 неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		''''''fTmp = ChangeSpiralStartParam(E, We4, aztWe4, Rad2Touch1, turn)
		'''''fTmp = SubtractAnglesWithSign(aztWe4, Rad2Touch1, Turn)
		'''''If fTmp < 0 Then
		'''''    ChangeSpiralStartParam E, We4, aztWe4, fTmp, Rad2Touch1, Turn, -Turn
		'''''Else
		'''''    ChangeSpiralStartParam E, We4, aztWe4, fTmp, Rad2Touch1, Turn, Turn
		'''''End If
		'''''CreateSpiralBy2Radial PtE4, fTmp, Rad2Touch1, Rad2Touch0, E, Turn, Shablon
		'''''
		'''''Rad2Touch0 = SpiralTouchToFix(PtE3, E, We3, aztWe3, Turn, ptNavPrj, -1, Axis)
		''''''fTmp = ChangeSpiralStartParam(E, We3, aztWe3, Rad2Touch2, turn)
		'''''ChangeSpiralStartParam E, We3, aztWe3, fTmp, Rad2Touch2, Turn, Turn
		'''''
		'''''CreateSpiralBy2Radial PtE3, fTmp, Rad2Touch2, Rad2Touch0, E, Turn, Shablon
		'''''Shablon.AddPoint ptNavPrj
		''''''DrawPolygon Shablon, 255
		'''''
		'''''Shablon45_180 = 1
	End Function
	
	Function ReversalShablon(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByRef t As Double, ByVal Axis As Double, ByVal Alpha As Double, ByVal Bank As Double, ByVal Turn As Integer, ByVal NavType As Integer, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef pFixArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef MPtCollection As ESRI.ArcGIS.Geometry.IPointCollection) As Integer
		'Вычисление требуемых параметров
		Dim K As Double ' 1
		Dim V As Double ' 2
		Dim v3600 As Double ' 3
		Dim R As Double ' 4
		Dim Rv As Double ' 5
		Dim h As Double ' 6
		Dim w As Double ' 7
		Dim w_ As Double ' 8
		Dim E As Double ' 9
		Dim Phi As Double '10
		Dim zN As Double '11
		Dim t_ As Double '12
		Dim L As Double '13
		Dim ab1 As Double '14
		Dim ab2 As Double '15
		Dim ab3 As Double '14_
		Dim ab4 As Double '15_
		Dim D As Double '20
		Dim N3l As Double '21
		Dim TrackToler As Double
		ReversalShablon = 0
		
		'Do
		K = IAS2TAS(IAS, AbsH, dISA)
		V = K '* IAS
		v3600 = V / 3.6
		
		'    R = 943.27 / V
		R = 6355 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
		If (R > 3#) Then R = 3#
		Rv = 1000# * V / (62.83 * R)
		
		h = AbsH
		w = 0.012 * h + 87#
		w_ = w / 3.6
		E = w_ / R
		
		If V <= 315# Then
			Phi = 36# / t
		Else
			Phi = 0.116 * V / t
		End If
		
		'MessageBox FrmManevreN.hWnd, CStr(Phi), "", 0
		
		If NavType = 2 Then
			zN = h * System.Math.Tan(DegToRad(NDB.ConeAngle))
			TrackToler = NDB.TrackingTolerance
		Else
			zN = h * System.Math.Tan(DegToRad(VOR.ConeAngle))
			TrackToler = VOR.TrackingTolerance
		End If
		
		t_ = 60# * t
		L = v3600 * t_
		ab1 = (t_ - 5#) * (v3600 - w_) - zN
		ab3 = ab1
		ab2 = (t_ + 21#) * (v3600 + w_) + zN
		ab4 = ab2
		D = RadToDeg(Math.Asin(w / V))
		N3l = 11# * v3600
		
		'Конец вычисления требуемых параметров

        Dim NomTrack As Double
        Dim AdjustCourse As Double

		NomTrack = Axis - Turn * Phi
		
		AdjustCourse = Modulus(Axis - (Phi + Alpha) * Turn, 360#)
		If NavType = 0 Then
			VORFIXTolerArea(ptNavPrj, AdjustCourse, h, pFixArea)
		Else
			NDBFIXTolerArea(ptNavPrj, AdjustCourse, h, pFixArea)
		End If
		
		
		'DrawPoint PointAlongPlane(ptNavPrj, NomTrack, 8000#), 255
		'DrawPolygon pFixArea, 255
		
		'Set pFixPoints = ReArrangePolygon(pFixArea, ptNavPrj, NomTrack)
		
		'DrawPolygon pFixArea, 255
		'DrawPolygon pFixPoints, 0
		
		'Nominal Track===================
		Dim pPt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt2 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt3 As ESRI.ArcGIS.Geometry.IPoint
		Dim pPt4 As ESRI.ArcGIS.Geometry.IPoint
		pPt4 = New ESRI.ArcGIS.Geometry.Point
		'    Dim MPtCollection As IPointCollection
		MPtCollection = New ESRI.ArcGIS.Geometry.Multipoint
		pPt1 = ptNavPrj
		pPt1.M = Axis - Turn * Phi
		MPtCollection.AddPoint(pPt1)
		
		
		pPt2 = PointAlongPlane(pPt1, pPt1.M, L)
		pPt2.M = pPt1.M

        MPtCollection.AddPoint(pPt2)
		pPt3 = PointAlongPlane(pPt1, Axis, L)
		pPt3.M = Axis + 180#

        MPtCollection.AddPoint(pPt3)
		pPt4.PutCoords(ptNavPrj.X, ptNavPrj.Y)
		pPt4.M = Axis + 180#
		MPtCollection.AddPoint(pPt4)
		
		'    Dim TracPoly As IPointCollection
		'    Set TracPoly = New Polyline
		'    Set TracPoly = CalcTrajectoryFromMultiPoint(MPtCollection)
		'    DrawPolyLine TracPoly, 0
		'================================
		ReversalShablon = 1

#If test Then
		Exit Function
        '================================
        Dim NewStR As Double
        Dim NewStRad As Double
        Dim fTmp As Double
        Dim fTmp0 As Double
        Dim NavArea As ESRI.ArcGIS.Geometry.IPointCollection
        Dim AztAC2 As Double
        Dim AztC5C3 As Double
        Dim AztC2C4 As Double
        Dim Ptb1 As ESRI.ArcGIS.Geometry.IPoint
        Dim Ptb2 As ESRI.ArcGIS.Geometry.IPoint
        Dim Ptb3 As ESRI.ArcGIS.Geometry.IPoint
		Dim Ptb4 As ESRI.ArcGIS.Geometry.IPoint
        Dim ptC2 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtC3 As ESRI.ArcGIS.Geometry.IPoint
        Dim PtC4 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtC5 As ESRI.ArcGIS.Geometry.IPoint
        Dim ptInter As ESRI.ArcGIS.Geometry.IConstructPoint
        Dim Ptk As ESRI.ArcGIS.Geometry.IPoint
        Dim pFixPoints As ESRI.ArcGIS.Geometry.IPointCollection
        Dim RadC3 As Double
        Dim RadC5 As Double
		Dim RadStSp0 As Double
		Dim RStSp0 As Double
		Dim RadStSp As Double
		Dim RStSp As Double
        Dim TouchRad2 As Double
        Dim TurnAngle As Double
        Dim TouchRad1 As Double
        Dim TouchAngl As Double
        Dim Solution As Integer

		'Защита входа
		Dim PtCntIn As ESRI.ArcGIS.Geometry.IPoint
		Dim NavAreaSt As ESRI.ArcGIS.Geometry.IPoint
		'fTmp = axis - (Phi + dAlpha) * turn
		If Turn = -1 Then 'Правый
			NavAreaSt = pFixArea.Point(3)
		Else
			NavAreaSt = pFixArea.Point(0)
		End If
		
		PtCntIn = PointAlongPlane(NavAreaSt, AdjustCourse, N3l)
		PtCntIn = PointAlongPlane(PtCntIn, AdjustCourse + 90# * Turn, Rv)
		
		'DrawPoint PtCntIn, 0
		'DrawPoint PtCntIn, 0
		NewStRad = ReturnAngleInDegrees(NavAreaSt, PtCntIn)
		RadStSp0 = Modulus(AdjustCourse - 90# * Turn, 360#)
		RStSp0 = Rv + 11# * w_
		ChangeSpiralStartParam(E, RStSp0, RadStSp0, NewStR, NewStRad, Turn, -Turn)
		TouchRad1 = SpiralTouchToFix(PtCntIn, E, NewStR, NewStRad, Turn, NavAreaSt, 1, Axis)
		TurnAngle = SubtractAnglesWithSign(RadStSp0, TouchRad1, Turn)
		NewStR = RStSp0 + E * TurnAngle
		NewStRad = TouchRad1
		
		If Turn < 0 Then
			Ptb1 = pFixPoints.Point(3)
			Ptb2 = pFixPoints.Point(2)
			Ptb3 = pFixPoints.Point(0)
			Ptb4 = pFixPoints.Point(1)
		Else
			Ptb1 = pFixPoints.Point(0)
			Ptb2 = pFixPoints.Point(1)
			Ptb3 = pFixPoints.Point(3)
			Ptb4 = pFixPoints.Point(2)
		End If
		
		'    Set Ptb1 = PointAlongPlane(ptNavPrj, NomTrack - TrackToler * Turn, ab1)
		'    Set Ptb2 = PointAlongPlane(ptNavPrj, NomTrack - TrackToler * Turn, ab2)
		'    Set Ptb3 = PointAlongPlane(ptNavPrj, NomTrack + TrackToler * Turn, ab3)
		'    Set Ptb4 = PointAlongPlane(ptNavPrj, NomTrack + TrackToler * Turn, ab4)
		
		ptC2 = PointAlongPlane(Ptb2, NomTrack + 90# * Turn, Rv)
		PtC4 = PointAlongPlane(Ptb4, NomTrack + 90# * Turn, Rv)
		
		RStSp = Rv
		RadStSp = Modulus(NomTrack + 90# * Turn + 180#, 360#)
		AztAC2 = ReturnAngleInDegrees(PtCntIn, ptC2)
		fTmp = AztAC2
		ChangeSpiralStartParam(E, NewStR, NewStRad, RStSp0, AztAC2, Turn, -Turn)
		ChangeSpiralStartParam(E, RStSp, RadStSp, fTmp0, fTmp, Turn, -Turn)
		
		Solution = TouchTo2Spiral(PtCntIn, RStSp0, E, AztAC2, ptC2, fTmp0, E, fTmp, Turn, RadC5, RadC3)
		If Solution = 0 Then
			MsgBox("Парамеры шаблона разворота на посадочный курс неправильны")
			Exit Function
		End If
		Shablon = New ESRI.ArcGIS.Geometry.Polygon
		Shablon.AddPoint(ptNavPrj)
		Shablon.AddPoint(NavAreaSt)
		CreateSpiralBy2Radial(PtCntIn, NewStR, NewStRad, RadC5, E, Turn, Shablon)
		TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
		RStSp = RStSp + E * TurnAngle
		RadStSp = RadC3
		'    DrawPolygon Shablon, 255
		'AztAC2 = ReturnAngleInDegrees(ptNavPrj, PtC2)
		'ChangeSpiralStartParam E, RStSp, RadStSp, NewStR, NewStRad, turn, -turn
		'TouchRad1 = SpiralTouchToFix(PtC2, E, NewStR, NewStRad, turn, ptNavPrj, 1, axis)
		'TurnAngle = SubtractAnglesWithSign(RadStSp, TouchRad1, turn)
		'NewStR = RStSp + E * TurnAngle
		'NewStRad = pEllipsoidalMath.Modulus(RadStSp + TurnAngle, 360#)
		AztC2C4 = ReturnAngleInDegrees(ptC2, PtC4)
		TouchAngl = Modulus(Axis - (90# + D) * Turn + 180#, 360#)
		Dim bFlg As Boolean
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		
		'        RStSp = Rv + Modulus(NomTrack + (TouchRad1 - 90#) * Turn, 360#) * E
		'    '    TouchRad2 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
		'        TouchRad2 = SpiralTouchAngle(RStSp, E, Modulus(NomTrack + TouchRad1 * Turn, 360#), TouchAngl, Turn)
		'        TouchRad2 = Modulus(NomTrack + TouchRad1 * Turn + (TouchRad2 - 90#) * Turn, 360#)
		
		bFlg = AnglesSideDef(TouchAngl, AztC2C4) = Turn
		
		If bFlg Then
			TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, AztC2C4, Turn)
			fTmp = Rv + TouchRad1 * E
			TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90#) * Turn, 360#)
			ptTmp = PointAlongPlane(PtC4, TouchRad1, fTmp)
		Else
			TouchRad1 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
			fTmp = Rv + TouchRad1 * E
			TouchRad1 = Modulus(NomTrack + (TouchRad1 - 90#) * Turn, 360#)
			ptTmp = PointAlongPlane(ptC2, TouchRad1, fTmp)
		End If
		
		'    DrawPoint ptTmp, 0
		'    DrawPoint PtC4, 255
		'    DrawPoint PtC2, 128
		
		CreateSpiralBy2Radial(ptC2, RStSp, RadStSp, TouchRad1, E, Turn, Shablon)
		'    Shablon.AddPoint Ptk
		'    DrawPolygon Shablon, 255
		
		Dim sp As ESRI.ArcGIS.Geometry.IPointCollection
		'Set sp = CreatePrjCircle(PtC4, Rv + 100# * E)
		'DrawPolygon sp, 0
		
		If bFlg Then
			RStSp = fTmp
			'        TouchRad2 = SpiralTouchAngle(RStSp, E, TouchRad1 + 90 * Turn, TouchAngl, Turn)
			'        TouchRad2 = Modulus(TouchRad1 + TouchRad2 * Turn, 360#)
			
			TouchRad2 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
			TouchRad2 = Modulus(NomTrack + (TouchRad2 - 90#) * Turn, 360#)
			
			CreateSpiralBy2Radial(PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, Shablon)
			
			'    Set Ptk = PointAlongPlane(PtC4, Modulus(NomTrack + (TouchRad1 - 90#) * Turn, 360#), RStSp)
			'    DrawPoint Ptk, RGB(0, 0, 255)
			'    DrawPoint PtC4, 255
			
			'Set sp = CreatePrjCircle(PtC4, RStSp)
			'DrawPolygon Shablon, 255
			
			''        RStSp = Rv + Modulus(NomTrack - 90# * Turn, 360#) * E
			'    '    TouchRad2 = SpiralTouchAngle(Rv, E, NomTrack, TouchAngl, Turn)
			
			'        TouchRad2 = Modulus(NomTrack + TouchRad1 * Turn + (TouchAngl - 90#) * Turn, 360#)
			'        CreateSpiralBy2Radial PtC4, RStSp, TouchRad2, TouchRad1, E, Turn, Shablon
			'=========================================
			'        RStSp = Rv + Modulus(NomTrack + (TouchRad1 - 90#) * Turn, 360#) * E
			'        TouchRad2 = SpiralTouchAngle(RStSp, E, Modulus(NomTrack + TouchRad1 * Turn, 360#), TouchAngl, Turn)
			'        TouchRad2 = Modulus(NomTrack + TouchRad1 * Turn + (TouchRad2 - 90#) * Turn, 360#)
			'        CreateSpiralBy2Radial PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, Shablon
		End If
		
		'Set sp = New Polygon
		'CreateSpiralBy2Radial PtC4, RStSp, TouchRad1, TouchRad2, E, Turn, sp
		'    DrawPolygon Shablon, 255
		
		ptInter = New ESRI.ArcGIS.Geometry.Point
		ptInter.ConstructAngleIntersection(ptNavPrj, DegToRad(Axis), ptTmp, DegToRad(TouchAngl))
		Ptk = ptInter
		
		'    DrawPoint Ptk, RGB(0, 0, 255)
		
		PtC5 = PointAlongPlane(Ptk, Axis + 180#, Rv)
		PtC3 = PointAlongPlane(Ptb3, NomTrack + 90# * Turn, Rv)
		RStSp = Rv + 190# * E
		RadStSp = Modulus(NomTrack - 90# * Turn + 190# * Turn, 360#)
		AztC5C3 = ReturnAngleInDegrees(PtC5, PtC3)
		ChangeSpiralStartParam(E, RStSp, RadStSp, NewStR, AztC5C3, Turn, -Turn)
		Solution = TouchTo2Spiral(PtC5, Rv, E, Axis, PtC3, NewStR, E, AztC5C3, Turn, RadC5, RadC3)
		If Solution = 0 Then
			MsgBox("Парамеры шаблона разворота на посадочный курс неправильны")
			Exit Function
		End If
		
		CreateSpiralBy2Radial(PtC5, Rv, Axis, RadC5, E, Turn, Shablon)
		'    DrawPolygon Shablon, 255
		
		TurnAngle = SubtractAnglesWithSign(RadStSp, RadC3, Turn)
		NewStR = RStSp + E * TurnAngle
		
		fTmp = ReturnAngleInDegrees(ptNavPrj, PtC3)
		
		ChangeSpiralStartParam(E, NewStR, RadC3, fTmp0, fTmp, Turn, -Turn)
		'    TouchRad1 = SpiralTouchToFix(PtC3, E, fTmp0, fTmp, Turn, ptNavPrj, -1, NomTrack + 90# * Turn)
		Dim DistToFix As Double
		Dim AztToFix As Double
		Dim ffTmp As Double
		Dim Rr As Double
		
		DistToFix = ReturnDistanceInMeters(PtC3, ptNavPrj)
		AztToFix = ReturnAngleInDegrees(PtC3, ptNavPrj)
		
		ffTmp = Modulus((AztToFix - fTmp) * Turn, 360#)
		If (ffTmp > 180#) And (fTmp0 = 0#) Then
			ffTmp = ffTmp - 360#
		End If
		Rr = fTmp0 + E * ffTmp '* turn
		
		'    T = T + 0.5
		'Loop While False 'Rr >= DistToFix
		'T = T - 0.5
		
		TouchRad1 = SpiralTouchToFix(PtC3, E, fTmp0, fTmp, Turn, ptNavPrj, -1, NomTrack + 10 * Turn)
		'If SubtractAngles(TouchRad1, RadC3) > 90# Then
		'    TouchRad1 = RadC3 + 90# * Turn
		'End If
		
		CreateSpiralBy2Radial(PtC3, NewStR, RadC3, TouchRad1, E, Turn, Shablon)
		Shablon.AddPoint(ptNavPrj)
		
		'DrawPolygon Shablon, 255
		
		'Dim lCircle As IPointCollection
		'Dim mCircle As IPointCollection
		'Dim nCircle As IPointCollection
		'Set lCircle = CreatePrjCircle(Ptl, Wl * 1000#)
		'Set mCircle = CreatePrjCircle(ptM, Wm * 1000#)
		'Set nCircle = CreatePrjCircle(Ptn, Wn * 1000#)
		'
		'DrawPolygon lCircle, RGB(255, 0, 0)
		'DrawPolygon mCircle, RGB(0, 255, 0)
		'DrawPolygon nCircle, RGB(0, 0, 255)
		'
		'dCircle.AddPointCollection lCircle
		'dCircle.AddPointCollection mCircle
		'dCircle.AddPointCollection nCircle
		'Set TopoOper = dCircle
		'Dim FullGeo As IGeometry
		'TopoOper.Simplify
		'Set FullGeo = TopoOper.ConvexHull
		'DrawPolygon FullGeo, RGB(0, 255, 0)
		'================================
		ReversalShablon = 1
#End If
	End Function
	
	Function HoldingShablon(ByVal ptNavPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal IAS As Double, ByVal AbsH As Double, ByVal dISA As Double, ByVal t As Double, ByVal Axis As Double, ByVal Bank As Double, ByVal Turn As Integer, ByVal NavType As Integer, ByRef HoldingArea As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Shablon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Line3 As ESRI.ArcGIS.Geometry.IPointCollection, ByRef Turn180 As ESRI.ArcGIS.Geometry.IPointCollection, ByVal IsByTime As Boolean, Optional ByRef radial As Double = -1) As Integer
		'Вычисление требуемых параметров
		Dim K As Double ' 1
		Dim V As Double ' 2
		Dim v3600 As Double ' 3
		Dim R As Double ' 4
		Dim Rv As Double ' 5
		Dim h As Double ' 6
		Dim w As Double ' 7
		Dim w_ As Double ' 8
		Dim E45 As Double ' 9
		Dim E As Double ' 9
		Dim t_ As Double '12
		Dim L As Double '13
		Dim ab As Double '14
		Dim ac As Double '14_
		Dim gi1 As Double '15
		Dim gi2 As Double '15_

		Dim xe As Double '15_
		Dim ye As Double '20
		
		'Set Shablon = New MultiPoint
		Shablon = New ESRI.ArcGIS.Geometry.Polygon
		HoldingArea = New ESRI.ArcGIS.Geometry.Multipoint
		Line3 = New ESRI.ArcGIS.Geometry.Multipoint
		Turn180 = New ESRI.ArcGIS.Geometry.Polyline
		HoldingShablon = 0
		
		K = IAS2TAS(IAS, AbsH, dISA)
		V = K '* IAS
		v3600 = 0.277777777777778 * V
		
		
		'R = 943.27 / V
        R = 6355.0 * System.Math.Tan(DegToRad(Bank)) / (PI * V)
        If (R > 3.0) Then R = 3.0
        Rv = 1000.0 * V / (62.83 * R)
		
		'------------------------------------
		Dim ptT1 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptT2 As ESRI.ArcGIS.Geometry.IPoint
		If radial <> -1 And t = -1 Then
            ptT1 = PointAlongPlane(ptNavPrj, Axis + 90.0, 2.0 * Rv)
            ptT2 = LineLineIntersect(ptT1, Axis, ptNavPrj, radial)
            t = ReturnDistanceInMeters(ptT2, ptNavPrj) / (60.0 * 0.277777777777778 * K)
		End If
		'------------------------------------
		'Rv = -1
		'If (R > 0.003) Then R = 0.003
		'If (R > 0) Then Rv = V / (20# * PI * R)
		
		
        h = AbsH / 1000.0
        w = 12.0 * h + 87.0
		
		w_ = 0.277777777777778 * w
		
		E = w_ / R
        E45 = 45.0 * E
        t_ = 60.0 * t
		
		L = v3600 * t_
		
        ab = 5.0 * v3600
        ac = 11.0 * v3600
        gi1 = (t_ - 5.0) * v3600
        gi2 = (t_ + 21.0) * v3600
		
        xe = 2.0 * Rv + (t_ + 15.0) * v3600 + (26.0 + 195.0 / R + t_) * w_
        ye = 11.0 * v3600 * System.Math.Cos(DegToRad(20.0)) + Rv * System.Math.Sin(DegToRad(20.0)) + Rv + (t_ + 15.0) * v3600 * System.Math.Tan(DegToRad(5.0)) + (21.0 + 125.0 / R + t_) * w_
		
		'-----------------------------------
		If Not IsByTime Then
			If L < 2# * Rv Then
				Exit Function
			End If
			'L = Sqr((L * L) - (4# * Rv * Rv)) + ac
			L = System.Math.Sqrt((L * L) - (4# * Rv * Rv))
		End If
		'-----------------------------------
		
		'Конец вычисления требуемых параметров
		'Вычисление центров спиралей
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

		PtB = PointAlongPlane(ptNavPrj, Axis + 180#, ab)
		
		'Set PtC = PointAlongPlane(ptNavPrj, Axis + 180#, ac)
		PtC = PointAlongPlane(ptNavPrj, Axis + 180#, 0#)
		
		PtCntB = PointAlongPlane(PtB, Axis - 90# * Turn, Rv)
		PtCntC = PointAlongPlane(PtC, Axis - 90# * Turn, Rv)
		PtG = PointAlongPlane(PtC, Axis - 90# * Turn, 2# * Rv)
		PtI1 = PointAlongPlane(PtG, Axis - 5# * Turn, gi1)
		PtI2 = PointAlongPlane(PtG, Axis - 5# * Turn, gi2)
		PtI3 = PointAlongPlane(PtG, Axis + 5# * Turn, gi1)
		PtI4 = PointAlongPlane(PtG, Axis + 5# * Turn, gi2)
		PtCntI2 = PointAlongPlane(PtI2, Axis + 90# * Turn, Rv)
		PtCntI4 = PointAlongPlane(PtI4, Axis + 90# * Turn, Rv)
		PtCntI1 = PointAlongPlane(PtI1, Axis + 90# * Turn, Rv)
		PtCntI3 = PointAlongPlane(PtI3, Axis + 90# * Turn, Rv)
		PtN3 = PointAlongPlane(PtI3, Axis + 90# * Turn, 2# * Rv)
		'===========HoldingArea
		HoldingArea.AddPoint(PtC)
		HoldingArea.Point(0).M = Axis + 180#
		
		HoldingArea.AddPoint(PtG) 'PointAlongPlane(PtC, axis + 180# + 90# * Turn, 2# * Rv)
		HoldingArea.Point(1).M = Axis
		
		HoldingArea.AddPoint(PointAlongPlane(HoldingArea.Point(1), Axis, L))
		HoldingArea.Point(2).M = Axis
		
		HoldingArea.AddPoint(PointAlongPlane(HoldingArea.Point(2), Axis + 90# * Turn, 2# * Rv))
		HoldingArea.Point(3).M = Axis + 180#
		
		HoldingArea.AddPoint(HoldingArea.Point(0))
		
		HoldingShablon = 1
		
		'Exit Function
		
		''''''Коней вычисления центров спиралей
		''''''===========Параметры спиралей
		'''''Dim R0C As Double
		'''''Dim Rad0C As Double
		'''''Dim R0I1 As Double
		'''''Dim Rad0I1 As Double
		'''''Dim R0I2 As Double
		'''''Dim Rad0I2 As Double
		'''''Dim R0I3 As Double
		'''''Dim Rad0I3 As Double
		'''''Dim R0I4 As Double
		'''''Dim Rad0I4 As Double
		'''''Dim RN3 As Double
		'''''Dim RadN3 As Double
		'''''Dim R0N4 As Double
		'''''
		'''''R0C = Rv + 11# * w_
		'''''Rad0C = Axis + 90# * Turn
		'''''R0I1 = Rv + (t_ + 6#) * w_ + 4# * E45
		'''''Rad0I1 = Axis - 90# * Turn
		'''''R0I2 = R0I1 + 14# * w_
		'''''Rad0I2 = Rad0I1
		'''''R0I3 = R0I1
		'''''Rad0I3 = Rad0I1
		'''''R0I4 = R0I2
		'''''Rad0I4 = Rad0I1
		'''''RN3 = (t_ + 6#) * w_ + 8# * E45
		'''''RadN3 = Axis + 90# * Turn
		''''''================= Line3 ===========================
		'''''Dim Solution As Long
		'''''Dim Rad1Tmp As Double
		'''''Dim Rad2Tmp As Double
		'''''Dim R1Tmp As Double
		'''''Dim R2Tmp As Double
		'''''
		'''''Dim I As Long
		'''''Dim CntAng1 As Double
		'''''Dim CntAng2 As Double
		'''''Dim TouchRes As Long
		'''''
		'''''TouchRes = TouchTo2Spiral(PtG, R0C - Rv, 0#, 0#, PtI3, R0I3 - Rv, 0#, 0#, -Turn, CntAng1, CntAng2)
		'''''
		'''''If TouchRes > 0 Then
		'''''    Line3.AddPoint PointAlongPlane(PtG, CntAng1, R0C - Rv)
		'''''    Line3.AddPoint PointAlongPlane(PtI3, CntAng2, R0I3 - Rv)
		'''''    Line3.Point(0).M = ReturnAngleInDegrees(Line3.Point(0), Line3.Point(1))
		'''''    Line3.Point(1).M = Line3.Point(0).M
		'''''End If
		'''''
		'''''TouchRes = TouchTo2Spiral(PtI3, R0I3 - Rv, 0#, 0#, PtI4, R0I4 - Rv, 0#, 0#, -Turn, CntAng1, CntAng2)
		'''''If TouchRes > 0 Then
		'''''    I = Line3.PointCount
		'''''    Line3.AddPoint PointAlongPlane(PtI3, CntAng1, R0I3 - Rv)
		'''''    Line3.AddPoint PointAlongPlane(PtI4, CntAng2, R0I4 - Rv)
		'''''    Line3.Point(I).M = ReturnAngleInDegrees(Line3.Point(I), Line3.Point(I + 1))
		'''''    Line3.Point(I + 1).M = Line3.Point(I).M
		'''''End If
		'''''
		'''''Line3.AddPoint PointAlongPlane(PtI4, Axis + 180# - 90# * Turn, R0I4 - Rv)
		'''''Line3.Point(Line3.PointCount - 1).M = Axis + 180#
		''''''=====================================Turn180
		'''''Dim R0B As Double
		'''''Dim Rad0B As Double
		'''''Dim RadBEnd As Double
		'''''Dim RadBstr As Double
		'''''Dim RadCend As Double
		'''''Dim TrackToler As Double
		'''''Dim TouchRad As Double
		''''''Dim PtCnt As IPoint
		''''''Dim AztStRad As Double
		''''''Dim Turn As Long
		''''''Dim FixPnt As IPoint
		''''''Dim SpiralIntercept As Long
		''''''Dim axis As Double
		'''''
		'''''R0B = Rv + 5# * w_ + 4# * E45
		'''''Rad0B = Modulus(Rad0C + 180#, 360#)
		'''''
		'''''Rad1Tmp = ReturnAngleInDegrees(PtCntB, PtCntC)
		'''''Rad2Tmp = Rad1Tmp
		'''''
		'''''ChangeSpiralStartParam E, R0C, Rad0C, R1Tmp, Rad1Tmp, Turn, Turn
		'''''ChangeSpiralStartParam E, R0B, Rad0B, R2Tmp, Rad2Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, Turn, RadCend, RadBstr)
		''''''Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, Turn, RadCend, RadBstr)
		'''''
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''Turn180.AddPoint ptNavPrj
		'''''
		'''''R1Tmp = R0C + E * Modulus(Turn * (RadCend - Rad0C), 360#)
		'''''Rad1Tmp = Modulus(RadCend + Turn * (90 - RadToDeg(Atn(RadToDeg(E) / R1Tmp))), 360#)
		'''''
		'''''If NavType = 2 Then
		'''''    TrackToler = NDB.TrackingTolerance
		'''''Else
		'''''    TrackToler = VOR.TrackingTolerance
		'''''End If
		'''''TouchRad = Modulus(Axis + Turn * TrackToler, 360#)
		'''''
		''''''If AnglesSideDef(TouchRad, Rad1Tmp) = Turn Then
		'''''If AnglesSideDef(Rad1Tmp, TouchRad) = Turn Then
		'''''    Rad2Tmp = Modulus(Rad0C + 90# * Turn, 360#)
		'''''    RadBEnd = SpiralTouchAngle(R0C, E, Rad2Tmp, TouchRad, Turn)
		'''''    RadBEnd = Modulus(Rad0C + RadBEnd * Turn, 360#)
		'''''    CreateSpiralBy2Radial PtCntC, R0C, Rad0C, RadBEnd, E, Turn, Turn180
		'''''Else
		'''''    Rad2Tmp = Modulus(Rad0B + 90 * Turn, 360#)
		'''''    RadBEnd = SpiralTouchAngle(R0B, E, Rad2Tmp, TouchRad, Turn)
		'''''
		'''''    RadBEnd = Modulus(Rad0B + RadBEnd * Turn, 360#)
		'''''    CreateSpiralBy2Radial PtCntC, R0C, Rad0C, RadCend, E, Turn, Turn180
		'''''
		'''''    If AnglesSideDef(Rad0B, RadBstr) = Turn Then
		'''''        R0B = R0B - E * SubtractAngles(Rad0B, RadBstr)
		'''''    Else
		'''''        R0B = R0B + E * SubtractAngles(Rad0B, RadBstr)
		'''''    End If
		'''''    CreateSpiralBy2Radial PtCntB, R0B, RadBstr, RadBEnd, E, Turn, Turn180
		'''''End If
		'''''
		''''''===================================================
		'''''Dim AztTmp As Double
		'''''Dim RadCSt As Double
		''''''Dim RadCend As Double
		'''''Dim RadI1St As Double
		'''''Dim RadI1End As Double
		'''''Dim RadI2St As Double
		'''''Dim RadI2End As Double
		'''''Dim RadI3St As Double
		'''''Dim RadI3End As Double
		'''''Dim RadI4St As Double
		'''''Dim RadI4End As Double
		'''''Dim RadN4St As Double
		'''''Dim RadN4End As Double
		'''''
		'''''Dim TurnAng As Double
		'''''Dim PtSt As IPoint
		'''''Dim fTmp As Double
		'''''Dim bN3flg As Boolean
		'''''
		'''''Rad1Tmp = ReturnAngleInDegrees(PtCntI1, PtCntC) - 45# * Turn
		'''''ChangeSpiralStartParam E, R0I1, Rad0I1, R1Tmp, Rad1Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtCntC, R0C, E, Rad0C, PtCntI1, R1Tmp, E, Rad1Tmp, Turn, RadCend, RadI1St)
		'''''
		''''''DrawPolyLine Turn180, 0, 2
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''Rad2Tmp = ReturnAngleInDegrees(PtCntI2, PtCntI1)
		'''''ChangeSpiralStartParam E, R0I2, Rad0I2, R2Tmp, Rad2Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtCntI1, R1Tmp, E, Rad1Tmp, PtCntI2, R2Tmp, E, Rad2Tmp, Turn, RadI1End, RadI2St)
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''Rad1Tmp = ReturnAngleInDegrees(PtCntI4, PtCntI2)
		'''''Rad2Tmp = Rad1Tmp
		'''''ChangeSpiralStartParam E, R0I2, Rad0I2, R1Tmp, Rad1Tmp, Turn, -Turn
		'''''ChangeSpiralStartParam E, R0I4, Rad0I4, R2Tmp, Rad2Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtCntI2, R1Tmp, E, Rad1Tmp, PtCntI4, R2Tmp, E, Rad2Tmp, Turn, RadI2End, RadI4St)
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''Rad1Tmp = ReturnAngleInDegrees(PtCntI3, PtCntI4)
		'''''Rad2Tmp = Rad1Tmp
		'''''ChangeSpiralStartParam E, R0I4, Rad0I4, R1Tmp, Rad1Tmp, Turn, -Turn
		'''''ChangeSpiralStartParam E, R0I3, Rad0I3, R2Tmp, Rad2Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtCntI4, R1Tmp, E, Rad1Tmp, PtCntI3, R2Tmp, E, Rad2Tmp, Turn, RadI4End, RadI3St)
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''TurnAng = Modulus((RadI4End - Rad0I4) * Turn, 360#)
		'''''R1Tmp = R0I4 + E * TurnAng
		'''''Set ptTmp = PointAlongPlane(PtCntI4, RadI4End, R1Tmp)
		'''''fTmp = RadToDeg(Atn(RadToDeg(E) / R1Tmp))
		'''''Rad1Tmp = RadI4End + (180# - fTmp) * Turn
		'''''Set PtN4_ = PointAlongPlane(ptTmp, Rad1Tmp, R1Tmp - Rv)
		'''''RadN4St = Modulus(Rad1Tmp + 180#, 360#)
		'''''
		'''''R0N4 = R1Tmp - Rv
		'''''
		'''''Solution = TouchTo2Spiral(PtN4_, R0N4, 0#, RadN4St, PtN3, RN3, 0#, Rad2Tmp, Turn, RadN4End, RadI3St)
		'''''
		'''''bN3flg = Solution <> 0
		'''''
		'''''If Not bN3flg Then
		'''''    Set PtN3 = PtN4_
		'''''    RN3 = R0N4
		'''''End If
		'''''
		''''''DrawPoint PtN4_, 0
		''''''DrawPoint PtN3, 255
		''''''DrawPolygon CreatePrjCircle(PtN4_, R0N4), 0
		''''''DrawPolygon CreatePrjCircle(PtN3, RN3), 255
		'''''
		'''''Rad2Tmp = ReturnAngleInDegrees(PtCntC, PtN3)
		'''''ChangeSpiralStartParam E, R0C, Rad0C, R2Tmp, Rad2Tmp, Turn, -Turn
		'''''Solution = TouchTo2Spiral(PtN3, RN3, 0#, Rad1Tmp, PtCntC, R2Tmp, E, Rad2Tmp, Turn, RadI3End, RadCSt)
		'''''
		'''''If Solution = 0 Then
		'''''    MsgBox "Параметры шаблона ипподрома неправильны"
		'''''    Exit Function
		'''''End If
		'''''
		'''''If Not bN3flg Then
		'''''    RadN4End = RadI3End
		'''''End If
		'''''
		'''''If AnglesSideDef(RadI3St, RadI3End) = Turn Then
		'''''    RadI3St = RadI3End
		'''''End If
		'''''
		'''''TurnAng = SubtractAnglesWithSign(Rad0C, RadCSt, Turn)
		'''''R1Tmp = R0C + E * TurnAng
		'''''Set PtSt = PointAlongPlane(PtCntC, RadCSt, R1Tmp)
		'''''CreateSpiralBy2Radial PtCntC, R1Tmp, RadCSt, RadCend, E, Turn, Shablon
		'''''
		'''''TurnAng = SubtractAnglesWithSign(Rad0I1, RadI1St, Turn)
		'''''R1Tmp = R0I1 + E * TurnAng
		'''''CreateSpiralBy2Radial PtCntI1, R1Tmp, RadI1St, RadI1End, E, Turn, Shablon
		'''''
		'''''TurnAng = SubtractAnglesWithSign(Rad0I2, RadI2St, Turn)
		'''''R1Tmp = R0I2 + E * TurnAng
		'''''CreateSpiralBy2Radial PtCntI2, R1Tmp, RadI2St, RadI2End, E, Turn, Shablon
		'''''
		'''''TurnAng = SubtractAnglesWithSign(Rad0I4, RadI4St, Turn)
		'''''R1Tmp = R0I4 + E * TurnAng
		'''''CreateSpiralBy2Radial PtCntI4, R1Tmp, RadI4St, RadI4End, E, Turn, Shablon
		'''''
		'''''CreateSpiralBy2Radial PtN4_, R0N4, RadN4St, RadN4End, 0#, Turn, Shablon
		''''''DrawPolygon Shablon, 255
		'''''
		'''''If bN3flg Then
		'''''    CreateSpiralBy2Radial PtN3, RN3, RadI3St, RadI3End, 0#, Turn, Shablon
		'''''End If
		'''''
		'''''Shablon.AddPoint PtSt
		'''''
		'''''Dim pTransform2D As ITransform2D
		'''''Dim pGeom As IGeometry
		'''''Dim ptE As IPoint
		'''''Dim dXmin As Double
		'''''Dim dYmin As Double
		'''''Dim dXmax As Double
		'''''Dim dYmax As Double
		'''''
		'''''Set pTransform2D = Shablon
		'''''pTransform2D.Rotate ptNavPrj, DegToRad(-Axis)
		'''''Set pGeom = Shablon
		'''''pGeom.Envelope.QueryCoords dXmin, dYmin, dXmax, dYmax
		'''''Set ptE = New Point
		'''''
		'''''If Turn < 0 Then
		'''''    ptE.PutCoords dXmax - xe, dYmin + ye
		'''''Else
		'''''    ptE.PutCoords dXmax - xe, dYmax - ye
		'''''End If
		'''''
		'''''Shablon.AddPoint ptNavPrj
		'''''Shablon.AddPoint ptE
		'''''
		'''''Shablon.AddPoint ptNavPrj, 0
		'''''Shablon.AddPoint ptE, 0
		'''''
		'''''pTransform2D.Rotate ptNavPrj, DegToRad(Axis)
		'''''HoldingShablon = 1
		
	End Function
End Module