Option Strict Off
Option Explicit On

Module PolyCut
	
	Private Structure TIntersections
		Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim IntersectIndex As Integer
		Dim PointIndex As Integer
		Dim PredIndex As Integer
		Dim Dist As Double
	End Structure
	
	Private Structure TPointsHeap
		Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
		Dim IsIntersection As Boolean
		Dim IntersectIndex As Integer
		Dim UsedFlag As Integer
	End Structure
	
	Private Const Epsilon As Double = 0.0000000001
	
	Public Function ClipByLine(ByRef pClipPolygon As ESRI.ArcGIS.Geometry.Polygon, ByRef pCutLine As ESRI.ArcGIS.Geometry.Polyline, ByRef pLeft As ESRI.ArcGIS.Geometry.Polygon, ByRef pRight As ESRI.ArcGIS.Geometry.Polygon, ByRef pUnspecified As ESRI.ArcGIS.Geometry.Polygon) As Short
		'Dim iRings As Integer
		Dim Side1 As Integer
		Dim side As Integer
		Dim Cnt As Integer
		Dim UBd As Integer
		'Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer

		Dim Down As Double
		Dim UpA As Double
		Dim UpB As Double
		Dim mA As Double
		Dim mB As Double

		Dim dXl As Double
		Dim dYl As Double
		Dim dXs As Double
		Dim dYs As Double
		Dim dX As Double
		Dim dY As Double

		Dim pIntersectionPoints() As TIntersections
		Dim pPointsHeap() As TPointsHeap

		Dim pRingVertices As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFormedPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pTestPoly As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pIUnspec As ESRI.ArcGIS.Geometry.IGeometryCollection
		'Dim pGeomCol As IGeometryCollection
		Dim pProxy As ESRI.ArcGIS.Geometry.IProximityOperator

		'Dim pExteriorRing() As ESRI.ArcGIS.Geometry.IRing
		'Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon2

		Dim pExteriorRings As ESRI.ArcGIS.Geometry.IGeometryBag
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon4

		Dim CutLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pRing As ESRI.ArcGIS.Geometry.IRing

		Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
		Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtX As ESRI.ArcGIS.Geometry.IPoint

		pClone = pClipPolygon
		pPolygon = pClone.Clone

		pTopoOper = pPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		pLeft = New ESRI.ArcGIS.Geometry.Polygon
		pRight = New ESRI.ArcGIS.Geometry.Polygon
		pUnspecified = New ESRI.ArcGIS.Geometry.Polygon
		pIUnspec = pUnspecified

		pTestPoly = New ESRI.ArcGIS.Geometry.Polygon
		CutLine = pCutLine
		ClipByLine = 0

		'    Set pGeomCol = pPolygon

		'iRings = pPolygon.ExteriorRingCount
		'If iRings > 0 Then
		'	ReDim pExteriorRing(iRings - 1)
		'Else
		'	Exit Function
		'End If

		'pPolygon.QueryExteriorRingsEx(iRings, pExteriorRing(0))
		pExteriorRings = pPolygon.ExteriorRingBag

		ptFrom = CutLine.FromPoint
		ptTo = CutLine.ToPoint

		dXl = ptTo.X - ptFrom.X
		dYl = ptTo.Y - ptFrom.Y

		Dim exteriorRingsEnum As ESRI.ArcGIS.Geometry.IEnumGeometry

		exteriorRingsEnum = DirectCast(pExteriorRings, ESRI.ArcGIS.Geometry.IEnumGeometry)
		exteriorRingsEnum.Reset()

		pRing = DirectCast(exteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)

		While Not pRing Is Nothing
			'For I = 0 To iRings - 1
			'	'' Getting each ring with its Index
			'	pRing = pExteriorRing(I)
			'        Set pRing = pGeomCol.Geometry(I)

			If pRing.IsExterior Then
				pProxy = ptFrom
			Else
				pProxy = ptTo
			End If

			pRingVertices = pRing

			N = pRingVertices.PointCount - 1
			If N >= 0 Then
				ReDim pIntersectionPoints(N - 1)

				L = -1
				Do
					L = L + 1
					If L >= N Then Exit Do
					side = WhichSide(pCutLine, pRingVertices.Point(L))
				Loop While side = 0

				Cnt = 0
				' '' GetIntersectPointOfTwoLines
				For J = 0 To N - 1
					K = (L + 1) Mod N
					pt1 = pRingVertices.Point(K)

					Side1 = WhichSide(pCutLine, pt1)
					If Side1 * side < 0 Then
						pt0 = pRingVertices.Point(L)

						side = Side1
						dXs = pt1.X - pt0.X
						dYs = pt1.Y - pt0.Y
						Down = dYl * dXs - dXl * dYs

						If System.Math.Abs(Down) > Epsilon Then
							dX = pt0.X - ptFrom.X
							dY = pt0.Y - ptFrom.Y

							UpA = dXl * dY - dYl * dX
							UpB = dXs * dY - dYs * dX

							mA = UpA / Down
							mB = UpB / Down

							If (mA >= 0.0 And mA < 1.0) And (mB >= 0.0 And mB < 1.0) Then

								PtX = New ESRI.ArcGIS.Geometry.Point
								PtX.PutCoords(pt0.X + mA * dXs, pt0.Y + mA * dYs)

								pIntersectionPoints(Cnt).pPoint = PtX
								pIntersectionPoints(Cnt).Dist = pProxy.ReturnDistance(PtX)
								pIntersectionPoints(Cnt).PredIndex = L
								Cnt = Cnt + 1
							End If
						End If
					End If
					L = K
				Next J

				If Cnt > 0 Then
					pTestPoly.RemoveGeometries(0, pTestPoly.GeometryCount)
					pTestPoly.AddGeometry(pRing)

					pProxy = pTestPoly

					If pProxy.ReturnDistance(ptTo) <> 0.0 Then
						UBd = Cnt - 1
					Else
						UBd = Cnt - 2
					End If

					If pProxy.ReturnDistance(ptFrom) = 0.0 Then
						pIntersectionPoints(0) = pIntersectionPoints(UBd)
						UBd = UBd - 1
					End If

					If UBd > 0 Then
						ReDim Preserve pIntersectionPoints(UBd)
						ReDim pPointsHeap(N + UBd)

						ArrangeVertices(pIntersectionPoints, pRingVertices, pPointsHeap)
						FormPolygons(pIntersectionPoints, pPointsHeap, pCutLine, pLeft, pRight, pUnspecified)

						ClipByLine = 3
					End If
					pProxy = ptFrom
				Else
					pIUnspec.AddGeometry(pRing)
				End If
			End If
			pRing = DirectCast(exteriorRingsEnum.Next(), ESRI.ArcGIS.Geometry.IRing)
		End While

		Dim pArea As ESRI.ArcGIS.Geometry.IArea

		If ClipByLine <> 0 Then
			pFormedPolygon = pLeft
			If (pFormedPolygon.PointCount < 3) And (pFormedPolygon.PointCount > 0) Then pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount)
			pArea = pLeft
			If pArea.Area >= 0.5 Then
				pTopoOper = pLeft
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
			Else
				pLeft = New ESRI.ArcGIS.Geometry.Polygon
			End If

			pFormedPolygon = pUnspecified
			If (pFormedPolygon.PointCount < 3) And (pFormedPolygon.PointCount > 0) Then pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount)

			pArea = pUnspecified
			If pArea.Area >= 0.5 Then
				pTopoOper = pUnspecified
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
			Else
				pUnspecified = New ESRI.ArcGIS.Geometry.Polygon
			End If

			pFormedPolygon = pRight
			If (pFormedPolygon.PointCount < 3) And (pFormedPolygon.PointCount > 0) Then pFormedPolygon.RemovePoints(0, pFormedPolygon.PointCount)
			pArea = pRight
			If pArea.Area >= 0.5 Then
				pTopoOper = pRight
				pTopoOper.IsKnownSimple_2 = False
				pTopoOper.Simplify()
			Else
				pRight = New ESRI.ArcGIS.Geometry.Polygon
			End If
		End If
	End Function
	
	Public Function ClipByPoly(ByRef pClipLine As ESRI.ArcGIS.Geometry.Polyline, ByRef pCutPolygon As ESRI.ArcGIS.Geometry.Polygon) As ESRI.ArcGIS.Geometry.Polyline
		Dim iRings As Integer
		Dim Side1 As Integer
		Dim side As Integer
		Dim Cnt As Integer
		Dim UBd As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
        Dim N As Integer
		
		Dim Down As Double
		Dim UpA As Double
		Dim UpB As Double
		Dim mA As Double
		Dim mB As Double
		
		Dim dXl As Double
		Dim dYl As Double
		Dim dXs As Double
		Dim dYs As Double
		Dim dX As Double
		Dim dY As Double
		
		Dim pIntersectionPoints() As TIntersections
		Dim pGeomCol As ESRI.ArcGIS.Geometry.IGeometryCollection
		
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pRingVertices As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pProxy As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon2
		Dim pCutLine As ESRI.ArcGIS.Geometry.IPolyline
		Dim pPline As ESRI.ArcGIS.Geometry.IPath
		Dim pClone As ESRI.ArcGIS.esriSystem.IClone
		Dim pRing As ESRI.ArcGIS.Geometry.IRing
		
		Dim ptFrom As ESRI.ArcGIS.Geometry.IPoint
		Dim ptTo As ESRI.ArcGIS.Geometry.IPoint
		Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
		Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
		Dim PtX As ESRI.ArcGIS.Geometry.IPoint

		pClone = pCutPolygon
		pPolygon = pClone.Clone

		pTopoOper = pPolygon
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		ClipByPoly = New ESRI.ArcGIS.Geometry.Polyline

		pCutLine = pClipLine

		pGeomCol = pPolygon
		iRings = pGeomCol.GeometryCount
		'    iRings = pPolygon.ExteriorRingCount

		If iRings = 0 Then
			'        ReDim pExteriorRing(iRings - 1)
			'    Else
			Exit Function
		End If

		'    pPolygon.QueryExteriorRingsEx iRings, pExteriorRing(0)

		ptFrom = pCutLine.FromPoint
		ptTo = pCutLine.ToPoint

		dXl = ptTo.X - ptFrom.X
		dYl = ptTo.Y - ptFrom.Y

		pProxy = ptFrom
		Cnt = 0

		For I = 0 To iRings - 1
			'' Getting each ring with its Index
			'        Set pRing = pExteriorRing(I)
			pRing = pGeomCol.Geometry(I)
			pRingVertices = pRing

			N = pRingVertices.PointCount - 1
			If N >= 0 Then
				ReDim Preserve pIntersectionPoints(N + Cnt)

				L = -1
				Do
					L = L + 1
					side = WhichSide(pCutLine, pRingVertices.Point(L))
				Loop While side = 0

				' '' GetIntersectPointOfTwoLines
				For J = 0 To N - 1
					K = (L + 1) Mod N
					pt1 = pRingVertices.Point(K)

					Side1 = WhichSide(pCutLine, pt1)
					If Side1 * side < 0 Then
						pt0 = pRingVertices.Point(L)

						side = Side1
						dXs = pt1.X - pt0.X
						dYs = pt1.Y - pt0.Y
						Down = dYl * dXs - dXl * dYs

						If System.Math.Abs(Down) > Epsilon Then
							dX = pt0.X - ptFrom.X
							dY = pt0.Y - ptFrom.Y

							UpA = dXl * dY - dYl * dX
							UpB = dXs * dY - dYs * dX

							Down = 1.0 / Down
							mA = UpA * Down
							mB = UpB * Down

							If (mA >= 0.0 And mA < 1.0) And (mB >= 0.0 And mB < 1.0) Then
								PtX = New ESRI.ArcGIS.Geometry.Point
								PtX.PutCoords(pt0.X + mA * dXs, pt0.Y + mA * dYs)

								pIntersectionPoints(Cnt).pPoint = PtX
								pIntersectionPoints(Cnt).Dist = pProxy.ReturnDistance(PtX)
								Cnt = Cnt + 1
							End If
						End If
					End If
					L = K
				Next J
			End If
		Next I
		
		pProxy = pPolygon
		
        If pProxy.ReturnDistance(ptTo) = 0 Then
            PtX = New ESRI.ArcGIS.Geometry.Point
            PtX.PutCoords(ptTo.X, ptTo.Y)
            pIntersectionPoints(Cnt).pPoint = PtX
			pIntersectionPoints(Cnt).Dist = 0.0
            Cnt = Cnt + 1
        End If
		
        If pProxy.ReturnDistance(ptFrom) = 0 Then
            PtX = New ESRI.ArcGIS.Geometry.Point
            PtX.PutCoords(ptFrom.X, ptFrom.Y)
            pIntersectionPoints(Cnt).pPoint = PtX
            pIntersectionPoints(Cnt).Dist = pCutLine.Length
            Cnt = Cnt + 1
        End If
		
		UBd = Cnt - 1
		
		If UBd > 0 Then
			pGeomCol = ClipByPoly
			ReDim Preserve pIntersectionPoints(UBd)
			SortByDist(pIntersectionPoints, UBd)
			For I = 0 To UBd Step 2
				pPline = New ESRI.ArcGIS.Geometry.Path
                pPline.FromPoint = pIntersectionPoints(I).pPoint
                pPline.ToPoint = pIntersectionPoints(I + 1).pPoint
                pGeomCol.AddGeometry(pPline)
			Next 
		End If
	End Function
	
	Private Sub ArrangeVertices(ByRef pInterSecPoints() As TIntersections, ByRef pVertexCol As ESRI.ArcGIS.Geometry.IPointCollection, ByRef pPointsHeap() As TPointsHeap)
		Dim CurItem As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim N As Integer
		Dim M As Integer
		
		N = UBound(pInterSecPoints) + 1
		M = pVertexCol.PointCount - 1
		
		SortByDist(pInterSecPoints, N - 1)
		
		For I = 0 To N - 1
			pInterSecPoints(I).IntersectIndex = I
		Next I
		
		SortByIndex(pInterSecPoints, N - 1)
		CurItem = 0
		
		For I = 0 To N - 1
			J = (I + 1) Mod N
			
			pPointsHeap(CurItem).pPoint = pInterSecPoints(I).pPoint
			pPointsHeap(CurItem).IsIntersection = True
			pPointsHeap(CurItem).IntersectIndex = pInterSecPoints(I).IntersectIndex
			pInterSecPoints(I).PointIndex = CurItem
			CurItem = CurItem + 1
			
			K = (pInterSecPoints(I).PredIndex + 1) Mod M
			L = (pInterSecPoints(J).PredIndex + 1) Mod M
			While K <> L
				pPointsHeap(CurItem).pPoint = pVertexCol.Point(K)
				pPointsHeap(CurItem).IsIntersection = False
				CurItem = CurItem + 1
				K = (K + 1) Mod M
			End While
		Next I
		
		SortByDist(pInterSecPoints, N - 1)
	End Sub
	
	Private Sub FormPolygons(ByRef pIntersectionPoints() As TIntersections, ByRef pPointsHeap() As TPointsHeap, ByRef pCutLine As ESRI.ArcGIS.Geometry.Polyline, ByRef pLeft As ESRI.ArcGIS.Geometry.Polygon, ByRef pRight As ESRI.ArcGIS.Geometry.Polygon, ByRef pUnspecified As ESRI.ArcGIS.Geometry.Polygon)
		
		Dim StartIndex As Integer
		Dim EndIndex As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim M As Integer
		
		Dim pPoints As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRing As ESRI.ArcGIS.Geometry.IRing
		
		Dim pIUnspec As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pIRight As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pILeft As ESRI.ArcGIS.Geometry.IGeometryCollection
		
		Dim bUnspec As Boolean
		Dim Trigger As Boolean
		
		pILeft = pLeft
		pIRight = pRight
		pIUnspec = pUnspecified
		
		N = UBound(pPointsHeap)
		M = UBound(pIntersectionPoints)
		
		For J = 0 To M - 1 Step 2
			K = J + 1
			
			StartIndex = pIntersectionPoints(J).PointIndex
			EndIndex = pIntersectionPoints(K).PointIndex
			
			I = StartIndex
			K = (I + 1) Mod (N + 1)
			
			bUnspec = False
			Trigger = False
			
			pRing = New ESRI.ArcGIS.Geometry.Ring
			pPoints = pRing
			
			If (pPointsHeap(I).UsedFlag And 1) = 0 Then
				While I <> EndIndex
					If pPointsHeap(I).IsIntersection Then
						pPoints.AddPoint(pPointsHeap(I).pPoint)

						If Trigger Then
							K = I - 1
							If K < 0 Then
								K = K + N + 1
							Else
								K = K Mod (N + 1)
							End If

							If WhichSide(pCutLine, pPointsHeap(K).pPoint) > 0 Then
								pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 1
								K = pPointsHeap(I).IntersectIndex - 1
								If K < 0 Then K = K + M + 1
							Else
								pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 2
								K = pPointsHeap(I).IntersectIndex + 1
								bUnspec = True
							End If

							K = K Mod (M + 1)
							I = (pIntersectionPoints(K).PointIndex)
						Else
							pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 1
							I = (I + 1) Mod (N + 1)
						End If

						Trigger = Not Trigger
					Else
						pPoints.AddPoint(pPointsHeap(I).pPoint)
						I = (I + 1) Mod (N + 1)
					End If
				End While
				
				pPoints.AddPoint(pPointsHeap(I).pPoint)
				pRing.Close()
				
				If bUnspec Then
                    pIUnspec.AddGeometry(pRing)
				Else
                    pILeft.AddGeometry(pRing)
				End If
			End If
			
			pRing = New ESRI.ArcGIS.Geometry.Ring
			pPoints = pRing
			
			I = StartIndex
			bUnspec = False
			Trigger = False
			
			K = I - 1
			If K < 0 Then
				K = K + N + 1
			Else
				K = K Mod (N + 1)
			End If
			
			If (pPointsHeap(I).UsedFlag And 2) = 0 Then
				While I <> EndIndex
					If pPointsHeap(I).IsIntersection Then
						pPoints.AddPoint(pPointsHeap(I).pPoint)
						
						If Trigger Then
							K = (I + 1) Mod (N + 1)
							
                            If WhichSide(pCutLine, pPointsHeap(K).pPoint) < 0 Then
                                pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 2
                                K = pPointsHeap(I).IntersectIndex - 1
                                If K < 0 Then K = K + M + 1
                            Else
                                K = pPointsHeap(I).IntersectIndex + 1
                                pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 1
                                bUnspec = True
                            End If
							
							K = K Mod (M + 1)
							I = (pIntersectionPoints(K).PointIndex)
						Else
							pPointsHeap(I).UsedFlag = pPointsHeap(I).UsedFlag Or 2
							I = I - 1
							If I < 0 Then I = I + N + 1
							I = I Mod (N + 1)
						End If
						
						Trigger = Not Trigger
					Else
						pPoints.AddPoint(pPointsHeap(I).pPoint)
						
						I = I - 1
						If I < 0 Then
							I = I + N + 1
						Else
							I = I Mod (N + 1)
						End If
					End If
				End While
				
				pPoints.AddPoint(pPointsHeap(I).pPoint)
				pRing.Close()
                pRing.ReverseOrientation()
				
				If bUnspec Then
                    pIUnspec.AddGeometry(pRing)
				Else
                    pIRight.AddGeometry(pRing)
				End If
			End If
		Next J
	End Sub
	
	Private Function WhichSide(ByRef CutLine As ESRI.ArcGIS.Geometry.IPolyline, ByRef ptCheck As ESRI.ArcGIS.Geometry.IPoint) As Integer
		Dim dX0 As Double
		Dim dY0 As Double
		Dim dX1 As Double
		Dim dY1 As Double
		Dim X As Double
		
        dX0 = ptCheck.X - CutLine.FromPoint.X
        dY0 = ptCheck.Y - CutLine.FromPoint.Y
		
        dX1 = ptCheck.X - CutLine.ToPoint.X
        dY1 = ptCheck.Y - CutLine.ToPoint.Y
		
		X = dX0 * dY1 - dY0 * dX1
		'    If Abs(x) > Epsilon Then
		WhichSide = System.Math.Sign(X)
		'    Else
		'        WhichSide = 0
		'    End If
	End Function
	
	Private Sub QSortByDist(ByRef A() As TIntersections, ByRef iLo As Integer, ByRef iHi As Integer)
		Dim T As TIntersections
		Dim Mid_Renamed As Double
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A((Lo + Hi) / 2).Dist
		Do
			While A(Lo).Dist < Mid_Renamed
				Lo = Lo + 1
			End While
			While A(Hi).Dist > Mid_Renamed
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

		If (Hi > iLo) Then QSortByDist(A, iLo, Hi)
		If (Lo < iHi) Then QSortByDist(A, Lo, iHi)
	End Sub
	
	Private Sub SortByDist(ByRef A() As TIntersections, ByRef N As Integer)
		'Dim tempInterPt As TIntersections
		'Dim I As Long
		'Dim J As Long
		
		'For I = 0 To N - 1
		'    For J = I + 1 To N
		'        If (pInterSecPoints(I).Dist > pInterSecPoints(J).Dist) Then
		'            tempInterPt = pInterSecPoints(J)
		'            pInterSecPoints(J) = pInterSecPoints(I)
		'            pInterSecPoints(I) = tempInterPt
		'        End If
		'    Next J
		'Next I
		
		Dim Lo As Integer
		Dim Hi As Integer
		
		Lo = LBound(A)
		Hi = N
		
		If (Lo = Hi) Then Return
		QSortByDist(A, Lo, Hi)
	End Sub
	
	Private Sub QSortByIndex(ByRef A() As TIntersections, ByRef iLo As Integer, ByRef iHi As Integer)
		Dim T As TIntersections
		Dim Mid_Renamed As Integer
		Dim Lo As Integer
		Dim Hi As Integer

		Lo = iLo
		Hi = iHi
		Mid_Renamed = A((Lo + Hi) / 2).PredIndex
		Do
			While A(Lo).PredIndex < Mid_Renamed
				Lo = Lo + 1
			End While
			While A(Hi).PredIndex > Mid_Renamed
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

		If (Hi > iLo) Then QSortByIndex(A, iLo, Hi)
		If (Lo < iHi) Then QSortByIndex(A, Lo, iHi)
	End Sub
	
	Private Sub SortByIndex(ByRef A() As TIntersections, ByRef N As Integer)
		'Dim tempInterPt As TIntersections
		'Dim I As Long
		'Dim J As Long
		
		'For I = 0 To N - 1
		'    For J = I + 1 To N
		'        If (pInterSecPoints(I).PredIndex > pInterSecPoints(J).PredIndex) Then
		'            tempInterPt = pInterSecPoints(J)
		'            pInterSecPoints(J) = pInterSecPoints(I)
		'            pInterSecPoints(I) = tempInterPt
		'        End If
		'    Next J
		'Next I
		Dim Lo As Integer
		Dim Hi As Integer
		
		Lo = LBound(A)
		Hi = N
		
		If (Lo = Hi) Then Return
		QSortByIndex(A, Lo, Hi)
	End Sub

End Module