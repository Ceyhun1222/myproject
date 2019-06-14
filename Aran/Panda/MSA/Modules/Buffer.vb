Option Strict Off
Option Explicit On

Module Buffer

	Private CirclePoints(360) As D2Point
	Private SavedBuffValue As Double

	Public Sub InitModule()
		Dim I As Integer
		SavedBuffValue = 0.0

		For I = 0 To 360
			CirclePoints(I).X = 0.0
			CirclePoints(I).Y = 0.0
		Next I
	End Sub

	Private Sub CalcCirclePoints(ByVal Buff As Double)
		Dim I As Integer
		Dim IinRad As Double

		For I = 0 To 360
			IinRad = I * DegToRadValue
			CirclePoints(I).X = Buff * System.Math.Cos(IinRad)
			CirclePoints(I).Y = Buff * System.Math.Sin(IinRad)
		Next I
	End Sub

	Public Function ArcBuf(ByRef ptCnt As ESRI.ArcGIS.Geometry.IPoint, ByRef InnerR As Double, ByRef OuterR As Double, ByRef BuffValue As Double, ByRef FromAngle As Double, ByRef ToAngle As Double) As ESRI.ArcGIS.Geometry.Polygon
		Dim I As Integer
		Dim KStop As Integer
		Dim KStart As Integer

		Dim H As Double
		Dim dA As Double
		Dim dX As Double
		Dim dY As Double
		Dim SinA As Double
		Dim CosA As Double
		Dim MaxR As Double
		Dim MinR As Double
		Dim InRad As Double
		Dim fDist As Double
		Dim fhInvD As Double
		Dim fAngle As Double
		Dim Angle1_1 As Double

		Dim pPtX As D2Point
		Dim pPtStop As D2Point
		Dim Side1(3) As D2Point
		Dim Side2(3) As D2Point
		Dim pPt0Stop As D2Point
		Dim pPtStart As D2Point
		Dim pPt0Start As D2Point

		Dim pPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pPolyline As ESRI.ArcGIS.Geometry.IPolyline
		Dim pInnerPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pOuterRing As ESRI.ArcGIS.Geometry.IPointCollection	'Ring
		Dim pInnerRing As ESRI.ArcGIS.Geometry.IPointCollection	'Ring
		Dim pPolygon As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		If BuffValue <> SavedBuffValue Then
			CalcCirclePoints(BuffValue)
			SavedBuffValue = BuffValue
		End If

		MaxR = BuffValue + OuterR
		MinR = InnerR - BuffValue

		pPt = New ESRI.ArcGIS.Geometry.Point
		pInnerPoly = New ESRI.ArcGIS.Geometry.Polyline
		pPolyline = pInnerPoly

		Angle1_1 = System.Math.Round(FromAngle + 0.4999)
		KStart = Modulus(Angle1_1)
		KStop = System.Math.Round(ToAngle - 0.4999)

		pPolygon = New ESRI.ArcGIS.Geometry.Polygon
		pOuterRing = New ESRI.ArcGIS.Geometry.Ring

		If (Modulus(ToAngle - Angle1_1) > 359.0) Or (Modulus(ToAngle - FromAngle) = 0.0) Then
			If MinR > 0 Then
				pInnerRing = New ESRI.ArcGIS.Geometry.Ring
				For I = 0 To 359
					InRad = DegToRadValue * I
					SinA = System.Math.Sin(InRad)
					CosA = System.Math.Cos(InRad)
					pPt.X = ptCnt.X + MaxR * CosA
					pPt.Y = ptCnt.Y + MaxR * SinA
					pOuterRing.AddPoint(pPt)

					pPt.X = ptCnt.X + MinR * CosA
					pPt.Y = ptCnt.Y + MinR * SinA
					pInnerPoly.AddPoint(pPt)
				Next
				pPolyline.ReverseOrientation()
				pInnerRing.AddPointCollection(pInnerPoly)
				pPolygon.AddGeometry(pOuterRing)
				pPolygon.AddGeometry(pInnerRing)
			Else
				For I = 0 To 359
					InRad = DegToRadValue * I
					pPt.X = ptCnt.X + MaxR * System.Math.Cos(InRad)
					pPt.Y = ptCnt.Y + MaxR * System.Math.Sin(InRad)
					pOuterRing.AddPoint(pPt)
				Next
				pPolygon.AddGeometry(pOuterRing)
			End If


			pPt = Nothing
			pTopo = pPolygon
			pTopo.IsKnownSimple_2 = False
			pTopo.Simplify()

			ArcBuf = pPolygon
			Exit Function
		End If

		KStart = Modulus(System.Math.Round(FromAngle + 0.4999))
		KStop = System.Math.Round(ToAngle - 0.4999)

		If MinR > 0 Then
			If KStop < KStart Then
				For I = KStart To 359
					InRad = DegToRadValue * I
					SinA = System.Math.Sin(InRad)
					CosA = System.Math.Cos(InRad)
					pPt.X = ptCnt.X + MaxR * CosA
					pPt.Y = ptCnt.Y + MaxR * SinA
					pOuterRing.AddPoint(pPt)

					pPt.X = ptCnt.X + MinR * CosA
					pPt.Y = ptCnt.Y + MinR * SinA
					pInnerPoly.AddPoint(pPt)
				Next

				For I = 0 To KStop
					InRad = DegToRadValue * I
					SinA = System.Math.Sin(InRad)
					CosA = System.Math.Cos(InRad)
					pPt.X = ptCnt.X + MaxR * CosA
					pPt.Y = ptCnt.Y + MaxR * SinA
					pOuterRing.AddPoint(pPt)

					pPt.X = ptCnt.X + MinR * CosA
					pPt.Y = ptCnt.Y + MinR * SinA
					pInnerPoly.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					InRad = DegToRadValue * I
					SinA = System.Math.Sin(InRad)
					CosA = System.Math.Cos(InRad)
					pPt.X = ptCnt.X + MaxR * CosA
					pPt.Y = ptCnt.Y + MaxR * SinA
					pOuterRing.AddPoint(pPt)

					pPt.X = ptCnt.X + MinR * CosA
					pPt.Y = ptCnt.Y + MinR * SinA
					pInnerPoly.AddPoint(pPt)
				Next
			End If

			pPolyline.ReverseOrientation()
		Else
			If KStop < KStart Then
				For I = KStart To 359
					InRad = DegToRadValue * I
					pPt.X = ptCnt.X + MaxR * System.Math.Cos(InRad)
					pPt.Y = ptCnt.Y + MaxR * System.Math.Sin(InRad)
					pOuterRing.AddPoint(pPt)
				Next

				For I = 0 To KStop
					InRad = DegToRadValue * I
					pPt.X = ptCnt.X + MaxR * System.Math.Cos(InRad)
					pPt.Y = ptCnt.Y + MaxR * System.Math.Sin(InRad)
					pOuterRing.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					InRad = DegToRadValue * I
					pPt.X = ptCnt.X + MaxR * System.Math.Cos(InRad)
					pPt.Y = ptCnt.Y + MaxR * System.Math.Sin(InRad)
					pOuterRing.AddPoint(pPt)
				Next
			End If
		End If

		InRad = DegToRadValue * FromAngle
		CosA = System.Math.Cos(InRad)
		SinA = System.Math.Sin(InRad)
		pPtStart.X = ptCnt.X + OuterR * CosA
		pPtStart.Y = ptCnt.Y + OuterR * SinA

		If MinR > 0 Then
			pPt0Start.X = ptCnt.X + InnerR * CosA
			pPt0Start.Y = ptCnt.Y + InnerR * SinA
		End If

		InRad = DegToRadValue * ToAngle
		CosA = System.Math.Cos(InRad)
		SinA = System.Math.Sin(InRad)
		pPtStop.X = ptCnt.X + CosA * OuterR
		pPtStop.Y = ptCnt.Y + SinA * OuterR

		If MinR > 0 Then
			pPt0Stop.X = ptCnt.X + CosA * InnerR
			pPt0Stop.Y = ptCnt.Y + SinA * InnerR
		End If

		dA = Modulus(ToAngle - FromAngle)

		dX = pPtStop.X - pPtStart.X
		dY = pPtStop.Y - pPtStart.Y
		fDist = System.Math.Sqrt(dX * dX + dY * dY)

		'======================================================
		Dim numer1, denom, numer2 As Double
		Dim dx1, dx0, dx2 As Double
		Dim dy1, dy0, dy2 As Double
		Dim ua, ub As Double

		If (dA <= 180.0 - degEps) Or (fDist > 2 * BuffValue) Then
			InRad = DegToRadValue * (FromAngle - 90.0)
			CosA = System.Math.Cos(InRad)
			SinA = System.Math.Sin(InRad)

			Side1(0).X = pPtStart.X + BuffValue * CosA
			Side1(0).Y = pPtStart.Y + BuffValue * SinA

			If MinR > 0 Then
				Side1(1).X = pPt0Start.X + BuffValue * CosA
				Side1(1).Y = pPt0Start.Y + BuffValue * SinA
			Else
				Side1(1).X = ptCnt.X + BuffValue * CosA
				Side1(1).Y = ptCnt.Y + BuffValue * SinA
			End If
			'======================================================

			InRad = DegToRadValue * (ToAngle + 90.0)
			CosA = System.Math.Cos(InRad)
			SinA = System.Math.Sin(InRad)

			Side2(0).X = pPtStop.X + BuffValue * CosA
			Side2(0).Y = pPtStop.Y + BuffValue * SinA

			If MinR > 0 Then
				Side2(1).X = pPt0Stop.X + BuffValue * CosA
				Side2(1).Y = pPt0Stop.Y + BuffValue * SinA
			Else
				Side2(1).X = ptCnt.X + BuffValue * CosA
				Side2(1).Y = ptCnt.Y + BuffValue * SinA
			End If
			'======================================================

			KStart = Modulus(System.Math.Round(ToAngle + 0.4999))
			KStop = System.Math.Round(Modulus(ToAngle + 90.0) - 0.4999)

			If KStop < KStart Then
				For I = KStart To 359
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next

				For I = 0 To KStop
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next
			End If
			'======================================================
			pPt.PutCoords(Side2(0).X, Side2(0).Y)
			pOuterRing.AddPoint(pPt)

			fDist = 0
			If MinR > 0 Then
				dX = pPt0Stop.X - pPt0Start.X
				dY = pPt0Stop.Y - pPt0Start.Y
				fDist = System.Math.Sqrt(dX * dX + dY * dY)
			End If

			If (dA <= 180.0 - degEps) Or (fDist > 2 * BuffValue) Then
				pPt.PutCoords(Side2(1).X, Side2(1).Y)
				pOuterRing.AddPoint(pPt)

				If MinR > 0 Then
					KStart = System.Math.Round(Modulus(ToAngle + 90.0) + 0.4999)
					KStop = Modulus(System.Math.Round(ToAngle + 180 - 0.4999))

					If KStop < KStart Then
						For I = KStart To 359
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pOuterRing.AddPoint(pPt)
						Next

						For I = 0 To KStop
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pOuterRing.AddPoint(pPt)
						Next
					Else
						For I = KStart To KStop
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pOuterRing.AddPoint(pPt)
						Next
					End If

					pOuterRing.AddPointCollection(pInnerPoly)

					KStart = Modulus(System.Math.Round(FromAngle - 180.0 + 0.4999))
					KStop = System.Math.Round(Modulus(FromAngle - 90.0) - 0.4999)

					If KStop < KStart Then
						For I = KStart To 359
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pOuterRing.AddPoint(pPt)
						Next

						For I = 0 To KStop
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pOuterRing.AddPoint(pPt)
						Next
					Else
						For I = KStart To KStop
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pOuterRing.AddPoint(pPt)
						Next
					End If
				Else
					KStart = Modulus(System.Math.Round(ToAngle + 90.0 + 0.4999))
					KStop = Modulus(System.Math.Round(FromAngle - 90.0 - 0.4999))
					If KStop < KStart Then
						For I = KStart To 359
							pPt.X = CirclePoints(I).X + ptCnt.X
							pPt.Y = CirclePoints(I).Y + ptCnt.Y
							pOuterRing.AddPoint(pPt)
						Next

						For I = 0 To KStop
							pPt.X = CirclePoints(I).X + ptCnt.X
							pPt.Y = CirclePoints(I).Y + ptCnt.Y
							pOuterRing.AddPoint(pPt)
						Next
					Else
						For I = KStart To KStop
							pPt.X = CirclePoints(I).X + ptCnt.X
							pPt.Y = CirclePoints(I).Y + ptCnt.Y
							pOuterRing.AddPoint(pPt)
						Next
					End If
				End If
				pPt.PutCoords(Side1(1).X, Side1(1).Y)
				pOuterRing.AddPoint(pPt)
			Else
				dx1 = Side1(1).X - Side1(0).X
				dx2 = Side2(1).X - Side2(0).X

				dy1 = Side1(1).Y - Side1(0).Y
				dy2 = Side2(1).Y - Side2(0).Y
				denom = dy2 * dx1 - dx2 * dy1

				If (System.Math.Abs(denom) > Eps) Then
					dx0 = Side1(0).X - Side2(0).X
					dy0 = Side1(0).Y - Side2(0).Y

					dx1 = Side1(1).X - Side1(0).X
					dy1 = Side1(1).Y - Side1(0).Y

					If (System.Math.Abs(dx1) > System.Math.Abs(dx2)) Then
						numer1 = dx2 * dy0 - dy2 * dx0
						ua = numer1 / denom
						dx0 = (ua * dx1) + 1.0

						pPtX.X = Side1(0).X + dx0
						pPtX.Y = Side1(0).Y + dx0 * dy1 / dx1
					Else
						numer2 = dx1 * dy0 - dy1 * dx0
						ub = numer2 / denom
						dx0 = (ub * dx2) + 1.0

						pPtX.X = Side2(0).X + dx0
						pPtX.Y = Side2(0).Y + dx0 * dy2 / dx2
					End If
				Else
					pPtX = Side2(1)
				End If

				pPt.PutCoords(pPtX.X, pPtX.Y)
				pOuterRing.AddPoint(pPt)

				If MinR > 0 Then
					H = System.Math.Sqrt(BuffValue * BuffValue - 0.25 * fDist * fDist)
					fhInvD = H / fDist

					pPtX.X = 0.5 * (pPt0Start.X + pPt0Stop.X) + fhInvD * dY
					pPtX.Y = 0.5 * (pPt0Start.Y + pPt0Stop.Y) - fhInvD * dX

					fAngle = Modulus(RadToDegValue * (Math.Atan2(pPtX.Y - pPt0Stop.Y, pPtX.X - pPt0Stop.X)))
					dA = Modulus(ToAngle - fAngle - 180)
					'======================================================
					KStop = Modulus(System.Math.Round(FromAngle + dA + 180 - 0.4999))
					KStart = Modulus(System.Math.Round(FromAngle + 180 + 0.4999))

					If KStop < KStart Then
						For I = KStart To 359
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pInnerPoly.AddPoint(pPt)
						Next

						For I = 0 To KStop
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pInnerPoly.AddPoint(pPt)
						Next
					Else
						For I = KStart To KStop
							pPt.X = CirclePoints(I).X + pPt0Start.X
							pPt.Y = CirclePoints(I).Y + pPt0Start.Y
							pInnerPoly.AddPoint(pPt)
						Next
					End If
					'======================================================
					pPt.PutCoords(pPtX.X, pPtX.Y)
					pInnerPoly.AddPoint(pPt)
					'======================================================
					KStart = Modulus(System.Math.Round(fAngle - 0.4999))
					KStop = Modulus(System.Math.Round(ToAngle + 180 + 0.4999))

					If KStop < KStart Then
						For I = KStart To 359
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pInnerPoly.AddPoint(pPt)
						Next

						For I = 0 To KStop
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pInnerPoly.AddPoint(pPt)
						Next
					Else
						For I = KStart To KStop
							pPt.X = CirclePoints(I).X + pPt0Stop.X
							pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
							pInnerPoly.AddPoint(pPt)
						Next
					End If

					pInnerRing = New ESRI.ArcGIS.Geometry.Ring
					pInnerRing.AddPointCollection(pInnerPoly)
					pPolygon.AddGeometry(pInnerRing)
				End If
			End If

			pPt.PutCoords(Side1(0).X, Side1(0).Y)
			pOuterRing.AddPoint(pPt)

			'======================================================
			KStart = Modulus(System.Math.Round(FromAngle - 90.0 + 0.4999))
			KStop = System.Math.Round(FromAngle - 0.4999)

			If KStop < KStart Then
				For I = KStart To 359
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next

				For I = 0 To KStop
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next
			End If
			'======================================================
		Else
			H = System.Math.Sqrt(BuffValue * BuffValue - 0.25 * fDist * fDist)
			fhInvD = H / fDist
			pPtX.X = 0.5 * (pPtStart.X + pPtStop.X) - fhInvD * dY
			pPtX.Y = 0.5 * (pPtStart.Y + pPtStop.Y) + fhInvD * dX

			fAngle = Modulus(RadToDegValue * (Math.Atan2(pPtX.Y - pPtStop.Y, pPtX.X - pPtStop.X)))

			dA = Modulus(fAngle - ToAngle)
			'======================================================
			KStart = Modulus(System.Math.Round(ToAngle + 0.4999))
			KStop = System.Math.Round(fAngle - 0.4999)

			If KStop < KStart Then
				For I = KStart To 359
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next

				For I = 0 To KStop
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					pPt.X = CirclePoints(I).X + pPtStop.X
					pPt.Y = CirclePoints(I).Y + pPtStop.Y
					pOuterRing.AddPoint(pPt)
				Next
			End If

			'======================================================
			pPt.PutCoords(pPtX.X, pPtX.Y)
			pOuterRing.AddPoint(pPt)
			'======================================================
			KStart = Modulus(System.Math.Round(FromAngle - dA + 0.4999))
			KStop = System.Math.Round(FromAngle - 0.4999)

			If KStop < KStart Then
				For I = KStart To 359
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next

				For I = 0 To KStop
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next
			Else
				For I = KStart To KStop
					pPt.X = CirclePoints(I).X + pPtStart.X
					pPt.Y = CirclePoints(I).Y + pPtStart.Y
					pOuterRing.AddPoint(pPt)
				Next
			End If

			If MinR > 0 Then
				dX = pPt0Stop.X - pPt0Start.X
				dY = pPt0Stop.Y - pPt0Start.Y
				fDist = System.Math.Sqrt(dX * dX + dY * dY)

				H = System.Math.Sqrt(BuffValue * BuffValue - 0.25 * fDist * fDist)
				fhInvD = H / fDist

				pPtX.X = 0.5 * (pPt0Start.X + pPt0Stop.X) + fhInvD * dY
				pPtX.Y = 0.5 * (pPt0Start.Y + pPt0Stop.Y) - fhInvD * dX

				fAngle = Modulus(RadToDegValue * (Math.Atan2(pPtX.Y - pPt0Stop.Y, pPtX.X - pPt0Stop.X)))
				dA = Modulus(ToAngle - fAngle - 180.0)
				'======================================================
				KStop = Modulus(System.Math.Round(FromAngle + dA + 180.0 - 0.4999))
				KStart = Modulus(System.Math.Round(FromAngle + 180 + 0.4999))

				If KStop < KStart Then
					For I = KStart To 359
						pPt.X = CirclePoints(I).X + pPt0Start.X
						pPt.Y = CirclePoints(I).Y + pPt0Start.Y
						pInnerPoly.AddPoint(pPt)
					Next

					For I = 0 To KStop
						pPt.X = CirclePoints(I).X + pPt0Start.X
						pPt.Y = CirclePoints(I).Y + pPt0Start.Y
						pInnerPoly.AddPoint(pPt)
					Next
				Else
					For I = KStart To KStop
						pPt.X = CirclePoints(I).X + pPt0Start.X
						pPt.Y = CirclePoints(I).Y + pPt0Start.Y
						pInnerPoly.AddPoint(pPt)
					Next
				End If
				'======================================================
				pPt.PutCoords(pPtX.X, pPtX.Y)
				pInnerPoly.AddPoint(pPt)
				'======================================================
				KStart = Modulus(System.Math.Round(fAngle - 0.4999))
				KStop = Modulus(System.Math.Round(ToAngle + 180 + 0.4999))

				If KStop < KStart Then
					For I = KStart To 359
						pPt.X = CirclePoints(I).X + pPt0Stop.X
						pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
						pInnerPoly.AddPoint(pPt)
					Next

					For I = 0 To KStop
						pPt.X = CirclePoints(I).X + pPt0Stop.X
						pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
						pInnerPoly.AddPoint(pPt)
					Next
				Else
					For I = KStart To KStop
						pPt.X = CirclePoints(I).X + pPt0Stop.X
						pPt.Y = CirclePoints(I).Y + pPt0Stop.Y
						pInnerPoly.AddPoint(pPt)
					Next
				End If

				pInnerRing = New ESRI.ArcGIS.Geometry.Ring
				pInnerRing.AddPointCollection(pInnerPoly)
				pPolygon.AddGeometry(pInnerRing)
			End If
		End If
		'======================================================

		pPolygon.AddGeometry(pOuterRing)

		pPt = Nothing
		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		ArcBuf = pPolygon
	End Function
End Module