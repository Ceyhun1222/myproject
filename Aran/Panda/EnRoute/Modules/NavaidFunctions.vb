Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS

<System.Runtime.InteropServices.ComVisibleAttribute(False)> Module NavaidFunctions
	Public NavTypeNames() As String = {"VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX", "MKR"}

	'Function OnNAVShift(ByRef NavType As Integer, ByRef Hrel As Double) As Double
	'	If (NavType = eNavaidType.CodeVOR) Or (NavType = eNavaidType.CodeTACAN) Then
	'		OnNAVShift = VOR.OnNAVRadius / OverHeadToler.Value * Hrel * System.Math.Tan(DegToRad(VOR.ConeAngle))
	'	ElseIf NavType = eNavaidType.CodeNDB Then
	'		OnNAVShift = NDB.OnNAVRadius / OverHeadToler.Value * Hrel * System.Math.Tan(DegToRad(NDB.ConeAngle))
	'	Else ' (NavType = eNavaidType.CodeLLZ) Then
	'		OnNAVShift = -1000000.0
	'	End If
	'End Function

	Function GetNavTypeName(NavaidType As eNavaidType) As String
		If NavaidType = eNavaidType.NONE Then
			Return "WPT"
		Else
			Return NavTypeNames(NavaidType)
		End If
	End Function

	Function OpenTableFromFile(ByRef pTable As Geodatabase.ITable, ByVal sFolderName As String, ByVal sFileName As String) As Boolean
		On Error GoTo EH

		Dim pFact As Geodatabase.IWorkspaceFactory
		Dim pWorkspace As Geodatabase.IWorkspace
		Dim pFeatWs As Geodatabase.IFeatureWorkspace
		Dim sPath As String

		sPath = sFolderName

		pFact = New DataSourcesFile.ShapefileWorkspaceFactory
		pWorkspace = pFact.OpenFromFile(sPath, GetApplicationHWnd())
		pFeatWs = pWorkspace
		pTable = pFeatWs.OpenTable(sFileName)

		Return True
EH:
		MessageBox.Show(Err.Number + "  " + Err.Description, "Open Table", MessageBoxButtons.OK, MessageBoxIcon.Error)
		'ErrorStr = Err.Number + "  " + Err.Description
		Return False
	End Function

	Private Function CalcNomPos(ByVal ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByVal Xs As Double, ByVal Ys As Double, ByVal D0 As Double, ByVal BaseHeight As Double, ByVal fRefAltitude As Double, ByVal PDG As Double, ByVal AheadBehindSide As Integer, ByVal NearSide As Integer) As Double
		Dim dNomPosDer As Double
		Dim dNomPosDME As Double
		Dim dOldPosDME As Double
		Dim nSign As Double
		Dim nSqr As Double
		Dim hMax As Double
		Dim I As Integer

		I = 0
		dNomPosDME = D0 + NearSide * DME.MinimalError
		hMax = 0.0

		Do
			nSqr = dNomPosDME * dNomPosDME - Ys * Ys
			nSign = System.Math.Sign(nSqr)

			dNomPosDer = Xs + AheadBehindSide * nSign * System.Math.Sqrt(System.Math.Abs(nSqr))
			hMax = dNomPosDer * PDG + BaseHeight - ptDMEprj.Z + fRefAltitude
			dOldPosDME = dNomPosDME
			dNomPosDME = (D0 + NearSide * DME.MinimalError) / (1.0 - NearSide * DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)))

			I += 1
			If I > 5 Then Exit Do
		Loop While System.Math.Abs(dOldPosDME - dNomPosDME) > distEps

		Return dNomPosDME
	End Function

	Private Function CalcDMERange(ByVal ptBasePrj As ESRI.ArcGIS.Geometry.Point, ByVal BaseHeight As Double, ByVal fRefAltitude As Double, ByVal NomDir As Double, ByVal PDG As Double, ByVal ptDMEprj As ESRI.ArcGIS.Geometry.IPoint, ByVal KKhMin As ESRI.ArcGIS.Geometry.IPolyline, ByVal KKhMax As ESRI.ArcGIS.Geometry.IPolyline) As Interval
		Dim Side As Integer
		Dim LeftRightSide As Integer
		Dim AheadBehindSide As Integer

		Dim D0 As Double
		Dim D1 As Double
		Dim Ys As Double
		Dim Xs As Double
		Dim dist0 As Double
		Dim Dist1 As Double

		AheadBehindSide = SideDef(KKhMin.FromPoint, NomDir + 90.0, ptDMEprj)
		LeftRightSide = SideDef(ptBasePrj, NomDir, ptDMEprj)

		Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90.0) * SideDef(ptBasePrj, NomDir + 90.0, ptDMEprj)
		Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir)

		If AheadBehindSide < 0 Then
			If LeftRightSide > 0 Then
				D0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint)

				Side = SideDef(KKhMax.FromPoint, NomDir, ptDMEprj)
				If Side < 0 Then
					D1 = Point2LineDistancePrj(ptDMEprj, KKhMax.FromPoint, NomDir + 90.0)
				Else
					D1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint)
				End If
			Else
				D0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint)

				Side = SideDef(KKhMax.ToPoint, NomDir, ptDMEprj)
				If Side > 0 Then
					D1 = Point2LineDistancePrj(ptDMEprj, KKhMax.ToPoint, NomDir + 90.0)
				Else
					D1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint)
				End If
			End If
		Else
			If LeftRightSide > 0 Then
				D0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint)

				Side = SideDef(KKhMin.FromPoint, NomDir, ptDMEprj)
				If Side < 0 Then
					D1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0)
				Else
					D1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint)
				End If
			Else
				D0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint)

				Side = SideDef(KKhMin.ToPoint, NomDir, ptDMEprj)
				If Side > 0 Then
					D1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0)
				Else
					D1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint)
				End If
			End If
		End If

		dist0 = CalcNomPos(ptDMEprj, Xs, Ys, D0, BaseHeight, fRefAltitude, PDG, AheadBehindSide, 1)
		Dist1 = CalcNomPos(ptDMEprj, Xs, Ys, D1, BaseHeight, fRefAltitude, PDG, AheadBehindSide, -1)

		CalcDMERange.Left = dist0
		CalcDMERange.Right = Dist1
	End Function

	Public Function GetValidEnRouteNavs(ByRef GuidNav As NavaidType, ByRef hFIX As Double, ByRef pTolerPolyline As ESRI.ArcGIS.Geometry.IPolyline, ByRef MaxToler As Double) As NavaidType()
		Dim ValidNavs() As NavaidType
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim L As Integer
		Dim M As Integer
		Dim N As Integer
		Dim nN As Integer
		Dim ii As Integer
		Dim jj As Integer
		Dim nNav As Integer
		Dim LeftRightSide As Integer
		Dim AheadBehindSide As Integer
		Dim AheadBehindKKhMax As Integer

		Dim fTmp As Double
		Dim SinA2 As Double
		Dim coef0 As Double
		Dim Coef1 As Double
		Dim Coef2 As Double
		Dim NomDir As Double
		Dim AztFar As Double
		Dim SinA2_1 As Double
		Dim SinA2_2 As Double
		Dim fRConus As Double
		Dim AztNear As Double
		Dim CotToler As Double
		Dim fFarDist As Double
		Dim fNearDist As Double
		Dim InterToler As Double
		Dim fConeAngle As Double
		Dim TrackToler1 As Double
		Dim fConeAngle1 As Double
		Dim TrackRange1 As Double
		Dim fNavAbeamDist As Double
		Dim fNavAlongDist As Double

		Dim pDMEConus As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPoly1 As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pProxi1 As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pProxiLine As ESRI.ArcGIS.Geometry.IProximityOperator

		Dim pGeomCollect As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint
		Dim pPath As ESRI.ArcGIS.Geometry.IPath
		Dim pGeom As ESRI.ArcGIS.Geometry.IGeometry

		'===========================================================================
		Dim IntrH As Interval
		Dim Intr23 As Interval
		Dim Intr55 As Interval

		Dim IntrRes() As Interval
		Dim IntrRes1() As Interval
		Dim IntrRes2() As Interval

		Dim Cutter As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMin1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMax1 As ESRI.ArcGIS.Geometry.IPolyline

		Dim KKhMinDME1 As ESRI.ArcGIS.Geometry.IPolyline
		Dim KKhMaxDME1 As ESRI.ArcGIS.Geometry.IPolyline

		'===========================================================================
		Dim ptTmp As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFarD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMin23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptMax23 As ESRI.ArcGIS.Geometry.IPoint
		Dim ptNearD As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		'Dim pInterPoly As ESRI.ArcGIS.Geometry.IPointCollection
		'Dim Geom As IGeometry

		Dim GuidRange As Double

		Dim minDist1 As Double
		Dim maxDist1 As Double
		Dim fCorrAngle As Double

		Dim fDist1 As Double
		Dim fDist2 As Double
		Dim fDist3 As Double
		Dim fMaxDist As Double
		Dim pLine As ESRI.ArcGIS.Geometry.IPolyline

		'===========================================================================
		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If
		ReDim ValidNavs(nNav - 1)
		'===========================================================================

		pGeomCollect = pTolerPolyline
		NomDir = ReturnAngleInDegrees(pTolerPolyline.FromPoint, pTolerPolyline.ToPoint)

		If GuidNav.TypeCode = eNavaidType.VOR Then
			TrackToler1 = VOR.EnRouteTrackingToler
			fConeAngle1 = VOR.ConeAngle
		ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
			TrackToler1 = NDB.EnRouteTrackingToler
			fConeAngle1 = NDB.ConeAngle
		End If

		TrackRange1 = GuidNav.Range / System.Math.Cos(DegToRad(TrackToler1))
		fRConus = System.Math.Tan(DegToRad(fConeAngle1)) * hFIX

		pGuidPoly1 = New ESRI.ArcGIS.Geometry.Polygon
		pGuidPoly1.AddPoint(GuidNav.pPtPrj)
		pGuidPoly1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler1, 100.0 * TrackRange1))
		pGuidPoly1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler1, 100.0 * TrackRange1))
		pGuidPoly1.AddPoint(GuidNav.pPtPrj)
		pGuidPoly1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler1 + 180.0, 100.0 * TrackRange1))
		pGuidPoly1.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler1 + 180.0, 100.0 * TrackRange1))
		pGuidPoly1.AddPoint(GuidNav.pPtPrj)

		pTopoOper = pGuidPoly1
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()
		pProxi1 = pGuidPoly1

		Cutter = New ESRI.ArcGIS.Geometry.Polyline
		pPath = pGeomCollect.Geometry(0)

		Cutter.FromPoint = PointAlongPlane(pPath.FromPoint, NomDir + 90.0, 100.0 * TrackRange1)
		Cutter.ToPoint = PointAlongPlane(pPath.FromPoint, NomDir - 90.0, 100.0 * TrackRange1)

		pGeom = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

		If pGeom.IsEmpty Then
			KKhMin1 = New ESRI.ArcGIS.Geometry.Polyline
			KKhMin1.FromPoint = pPath.FromPoint
			KKhMin1.ToPoint = pPath.FromPoint
		Else
			KKhMin1 = pGeom
			If SideDef(KKhMin1.ToPoint, NomDir, KKhMin1.FromPoint) < 0 Then KKhMin1.ReverseOrientation()
		End If

		ptTmp = pPath.ToPoint
		'If bTwoGuid And (pGeomCollect.GeometryCount = 1) Then
		'    Set ptTmp = PointAlongPlane(GuidNav1.ptPrj, NomDir, COPDist)
		'Else
		'    Set ptTmp = pPath.ToPoint
		'End If

		Cutter.FromPoint = PointAlongPlane(ptTmp, NomDir + 90.0, 100.0 * TrackRange1)
		Cutter.ToPoint = PointAlongPlane(ptTmp, NomDir - 90.0, 100.0 * TrackRange1)

		pGeom = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
		If pGeom.IsEmpty Then
			KKhMax1 = New ESRI.ArcGIS.Geometry.Polyline
			KKhMax1.FromPoint = pPath.ToPoint
			KKhMax1.ToPoint = pPath.ToPoint
		Else
			KKhMax1 = pGeom
			If SideDef(KKhMax1.ToPoint, NomDir, KKhMax1.FromPoint) < 0 Then KKhMax1.ReverseOrientation()
		End If

		minDist1 = ReturnDistanceInMeters(GuidNav.pPtPrj, pPath.FromPoint)
		maxDist1 = ReturnDistanceInMeters(GuidNav.pPtPrj, pPath.ToPoint)

		fCorrAngle = 0.0
		If minDist1 > maxDist1 Then fCorrAngle = 180.0

		'===========================================================================

		pProxiLine = pTolerPolyline
		'pInterPoly = New ESRI.ArcGIS.Geometry.Polygon
		nN = UBound(NavaidList)
		J = 0

		For I = 0 To nN
			ptFNavPrj = NavaidList(I).pPtPrj

			GuidRange = Math.Min(System.Math.Sqrt(hFIX) * 4130.0, NavaidList(I).Range)
			'        GuidRange = NavaidList(I).Range '* 1000#
			If GuidRange < pProxiLine.ReturnDistance(ptFNavPrj) Then Continue For

			LeftRightSide = SideDef(pTolerPolyline.FromPoint, NomDir, ptFNavPrj)
			AheadBehindSide = SideDef(pTolerPolyline.FromPoint, NomDir + 90.0, ptFNavPrj) '+
			AheadBehindKKhMax = SideDef(pTolerPolyline.ToPoint, NomDir + 90.0, ptFNavPrj) '-

			'        If K > eNavaidType.CodeNDB Then GoTo NextNAV

			If (K = eNavaidType.VOR) Then
				InterToler = VOR.EnRouteInterToler
				fConeAngle = VOR.ConeAngle
			Else
				InterToler = NDB.EnRouteInterToler
				fConeAngle = NDB.ConeAngle
			End If

			fRConus = System.Math.Tan(DegToRad(fConeAngle)) * hFIX
			If (pProxi1.ReturnDistance(ptFNavPrj) < fRConus) Then Continue For

			pDMEConus = CreatePrjCircle(ptFNavPrj, GuidRange)
			Cutter.FromPoint = GuidNav.pPtPrj

			If LeftRightSide < 0 Then
				If minDist1 > maxDist1 Then
					Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler1 + 180.0, 10.0 * TrackRange1)
				Else
					Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler1, 10.0 * TrackRange1)
				End If
			Else
				If minDist1 > maxDist1 Then
					Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler1 + 180.0, 10.0 * TrackRange1)
				Else
					Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler1, 10.0 * TrackRange1)
				End If
			End If

			pTopoOper = pDMEConus
			pGeom = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)
			'ii = -1

			If (pGeom.IsEmpty) Or (pProxi1.ReturnDistance(ptFNavPrj) < fRConus) Then Continue For
			pLine = pGeom
			If SideDef(pLine.FromPoint, NomDir + 90.0, pLine.ToPoint) < 0 Then pLine.ReverseOrientation()

			fNavAbeamDist = Point2LineDistancePrj(ptFNavPrj, GuidNav.pPtPrj, NomDir)
			fNavAlongDist = Point2LineDistancePrj(ptFNavPrj, GuidNav.pPtPrj, NomDir + 90.0) * SideDef(GuidNav.pPtPrj, NomDir + 90.0, ptFNavPrj)

			CotToler = 1.0 / System.Math.Tan(DegToRad(InterToler))

			Coef1 = (1.0 + CotToler * CotToler)
			coef0 = fNavAbeamDist / MaxToler
			Coef2 = 1.0 + 2.0 * CotToler * coef0

			fTmp = Coef2 * Coef2 - 4.0 * Coef1 * coef0 * coef0

			If fTmp < 0 Then Continue For

			SinA2_1 = 0.5 * (1.0 + 2.0 * CotToler * coef0 + System.Math.Sqrt(fTmp)) / Coef1
			SinA2_2 = 0.5 * (1.0 + 2.0 * CotToler * coef0 - System.Math.Sqrt(fTmp)) / Coef1
			SinA2 = Math.Max(Math.Min(SinA2_1, SinA2_2), 0.3)

			If LeftRightSide > 0 Then
				If AheadBehindSide < 0 Then
					ptNearD = KKhMin1.FromPoint
					ptFarD = KKhMax1.ToPoint
				ElseIf AheadBehindKKhMax < 0 Then
					ptNearD = KKhMin1.ToPoint
					ptFarD = KKhMax1.ToPoint
				Else
					ptNearD = KKhMin1.ToPoint
					ptFarD = KKhMax1.FromPoint
				End If
			Else
				If AheadBehindSide < 0 Then
					ptNearD = KKhMin1.ToPoint
					ptFarD = KKhMax1.FromPoint
				ElseIf AheadBehindKKhMax < 0 Then
					ptNearD = KKhMin1.FromPoint
					ptFarD = KKhMax1.FromPoint
				Else
					ptNearD = KKhMin1.FromPoint
					ptFarD = KKhMax1.ToPoint
				End If
			End If

			fNearDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ptNearD)
			fFarDist = ReturnDistanceInMeters(GuidNav.pPtPrj, ptFarD)

			If minDist1 > maxDist1 Then
				fTmp = ReturnDistanceInMeters(GuidNav.pPtPrj, pLine.FromPoint)
				If fNearDist > fTmp Then fNearDist = fTmp

				fTmp = fNavAlongDist - System.Math.Sqrt(1.0 - SinA2) / System.Math.Sqrt(SinA2) * fNavAbeamDist
				If fNearDist > -fTmp Then fNearDist = -fTmp

				fTmp = ReturnDistanceInMeters(GuidNav.pPtPrj, pLine.ToPoint)
				If fFarDist < fTmp Then fFarDist = fTmp

				fTmp = fNavAlongDist + System.Math.Sqrt(1.0 - SinA2) / System.Math.Sqrt(SinA2) * fNavAbeamDist
				If fFarDist < -fTmp Then fFarDist = -fTmp

				If fFarDist > fNearDist Then Continue For

				ptNearD = PointAlongPlane(GuidNav.pPtPrj, NomDir + 180.0, fNearDist)
				ptFarD = PointAlongPlane(GuidNav.pPtPrj, NomDir + 180.0, fFarDist)
			Else
				fTmp = ReturnDistanceInMeters(GuidNav.pPtPrj, pLine.FromPoint)
				If fNearDist < fTmp Then fNearDist = fTmp

				fTmp = fNavAlongDist - System.Math.Sqrt(1.0 - SinA2) / System.Math.Sqrt(SinA2) * fNavAbeamDist
				If fNearDist < fTmp Then fNearDist = fTmp

				fTmp = ReturnDistanceInMeters(GuidNav.pPtPrj, pLine.ToPoint)
				If fFarDist > fTmp Then fFarDist = fTmp

				fTmp = fNavAlongDist + System.Math.Sqrt(1.0 - SinA2) / System.Math.Sqrt(SinA2) * fNavAbeamDist
				If fFarDist > fTmp Then fFarDist = fTmp

				If fFarDist < fNearDist Then Continue For

				ptNearD = PointAlongPlane(GuidNav.pPtPrj, NomDir, fNearDist)
				ptFarD = PointAlongPlane(GuidNav.pPtPrj, NomDir, fFarDist)
			End If
			AztFar = ReturnAngleInDegrees(ptFNavPrj, ptFarD)
			AztNear = ReturnAngleInDegrees(ptFNavPrj, ptNearD)

			If SubtractAngles(AztNear, AztFar) < 2.0 * InterToler Then Continue For

			ValidNavs(J) = NavaidList(I)
			ReDim ValidNavs(J).ValMax(0)
			ReDim ValidNavs(J).ValMin(0)

			ValidNavs(J).ValCnt = LeftRightSide

			If LeftRightSide > 0 Then
				ValidNavs(J).ValMin(0) = Modulus(AztFar + InterToler, 360.0)
				ValidNavs(J).ValMax(0) = Modulus(AztNear - InterToler, 360.0)
			Else
				ValidNavs(J).ValMin(0) = Modulus(AztFar - InterToler, 360.0)
				ValidNavs(J).ValMax(0) = Modulus(AztNear + InterToler, 360.0)
			End If

			J += 1
		Next I

		nN = UBound(DMEList)
		For I = 0 To nN
			ptFNavPrj = DMEList(I).pPtPrj

			GuidRange = Math.Min(System.Math.Sqrt(hFIX) * 4130.0, DMEList(I).Range)
			'        GuidRange = DMEList(I).Range '* 1000#
			If GuidRange < pProxiLine.ReturnDistance(ptFNavPrj) Then Continue For


			LeftRightSide = SideDef(pTolerPolyline.FromPoint, NomDir, ptFNavPrj)
			AheadBehindSide = SideDef(pTolerPolyline.FromPoint, NomDir + 90.0, ptFNavPrj) '+
			AheadBehindKKhMax = SideDef(pTolerPolyline.ToPoint, NomDir + 90.0, ptFNavPrj) '-

			If minDist1 > maxDist1 Then
				IntrH.Left = maxDist1
				IntrH.Right = minDist1
			Else
				IntrH.Left = minDist1
				IntrH.Right = maxDist1
			End If
			'=========================================================================================
			If LeftRightSide <> 0 Then
				ptMin23 = New ESRI.ArcGIS.Geometry.Point
				ptMax23 = New ESRI.ArcGIS.Geometry.Point
				If minDist1 > maxDist1 Then
					pConstruct = ptMin23
					pConstruct.ConstructAngleIntersection(GuidNav.pPtPrj, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * DME.TP_div))

					pConstruct = ptMax23
					pConstruct.ConstructAngleIntersection(GuidNav.pPtPrj, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * DME.TP_div))
				Else
					pConstruct = ptMin23
					pConstruct.ConstructAngleIntersection(GuidNav.pPtPrj, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - LeftRightSide * DME.TP_div))

					pConstruct = ptMax23
					pConstruct.ConstructAngleIntersection(GuidNav.pPtPrj, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir + LeftRightSide * DME.TP_div))
				End If
			Else
				ptMin23 = ptFNavPrj
				ptMax23 = ptFNavPrj
			End If
			'=========================================================================================
			Intr23.Left = ReturnDistanceInMeters(GuidNav.pPtPrj, ptMin23) * SideDef(GuidNav.pPtPrj, NomDir + fCorrAngle + 90.0, ptMin23)
			Intr23.Right = ReturnDistanceInMeters(GuidNav.pPtPrj, ptMax23) * SideDef(GuidNav.pPtPrj, NomDir + fCorrAngle + 90.0, ptMax23)

			IntrRes = IntervalsDifference(IntrH, Intr23)
			If UBound(IntrRes) < 0 Then Continue For
			If IntrRes(0).Left > GuidRange Then Continue For

			'================================== SlantAngle =================================================

			fRConus = (hFIX - ptFNavPrj.Z) * System.Math.Tan(DegToRad(DME.SlantAngle))

			fMaxDist = Point2LineDistancePrj(GuidNav.pPtPrj, ptFNavPrj, NomDir + TrackToler1)
			fDist1 = Point2LineDistancePrj(GuidNav.pPtPrj, ptFNavPrj, NomDir - TrackToler1)
			If fDist1 > fMaxDist Then fMaxDist = fDist1
			If fRConus > fMaxDist Then fMaxDist = fRConus

			If SideDef(GuidNav.pPtPrj, NomDir + 90.0, ptFNavPrj) < 0 Then
				fDist1 = ReturnDistanceInMeters(GuidNav.pPtPrj, ptFNavPrj)
				If fDist1 > fMaxDist Then fMaxDist = fDist1
			End If

			pDMEConus = CreatePrjCircle(ptFNavPrj, fMaxDist + 1.0)
			Cutter.FromPoint = GuidNav.pPtPrj

			If LeftRightSide > 0 Then
				Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler1 + fCorrAngle, 10.0 * TrackRange1)
			Else
				Cutter.ToPoint = PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler1 + fCorrAngle, 10.0 * TrackRange1)
			End If

			pTopoOper = pDMEConus
			pGeom = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

			If Not pGeom.IsEmpty Then
				pLine = pGeom
				If SideDef(pLine.FromPoint, NomDir + 90.0, pLine.ToPoint) < 0 Then pLine.ReverseOrientation()

				fDist1 = Point2LineDistancePrj(GuidNav.pPtPrj, pLine.FromPoint, NomDir)
				fDist2 = Point2LineDistancePrj(GuidNav.pPtPrj, pLine.ToPoint, NomDir)

				fDist3 = Point2LineDistancePrj(GuidNav.pPtPrj, ptFNavPrj, NomDir)
				fTmp = Point2LineDistancePrj(GuidNav.pPtPrj, ptFNavPrj, NomDir + 90.0)

				If fDist3 <= fDist1 Then
					Intr55.Left = fTmp - fMaxDist
				Else
					Intr55.Left = Point2LineDistancePrj(GuidNav.pPtPrj, pLine.FromPoint, NomDir + 90.0)
				End If

				If fDist3 <= fDist2 Then
					Intr55.Right = fTmp + fMaxDist
				Else
					Intr55.Right = Point2LineDistancePrj(GuidNav.pPtPrj, pLine.ToPoint, NomDir + 90.0)
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

			'===========================================================================================
			If N < 0 Then Continue For

			ReDim IntrRes1(N)
			M = 0
			pTopoOper = pGuidPoly1

			For ii = 0 To N
				ptNearD = PointAlongPlane(GuidNav.pPtPrj, NomDir + fCorrAngle, IntrRes(ii).Left)
				ptFarD = PointAlongPlane(GuidNav.pPtPrj, NomDir + fCorrAngle, IntrRes(ii).Right)

				Cutter.FromPoint = PointAlongPlane(ptNearD, NomDir - 90.0 + fCorrAngle, 100.0 * TrackRange1)
				Cutter.ToPoint = PointAlongPlane(ptNearD, NomDir + 90.0 + fCorrAngle, 100.0 * TrackRange1)

				KKhMinDME1 = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

				If SideDef(ptNearD, NomDir + fCorrAngle, KKhMinDME1.FromPoint) < 0 Then KKhMinDME1.ReverseOrientation()

				Cutter.FromPoint = PointAlongPlane(ptFarD, NomDir - 90.0, 100.0 * TrackRange1)
				Cutter.ToPoint = PointAlongPlane(ptFarD, NomDir + 90.0, 100.0 * TrackRange1)

				KKhMaxDME1 = pTopoOper.Intersect(Cutter, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension)

				If SideDef(ptFarD, NomDir + fCorrAngle, KKhMaxDME1.FromPoint) < 0 Then KKhMaxDME1.ReverseOrientation()

				IntrRes1(M) = CalcDMERange(GuidNav.pPtPrj, hFIX, GuidNav.pPtPrj.Z, NomDir + fCorrAngle, 0.0, ptFNavPrj, KKhMinDME1, KKhMaxDME1)

				If IntrRes1(M).Left < GuidRange Then
					If IntrRes1(M).Left < IntrRes1(M).Right Then
						If IntrRes1(M).Right > GuidRange Then IntrRes1(M).Right = GuidRange
						ValidNavs(J).ValCnt = SideDef(KKhMinDME1.FromPoint, NomDir + 90.0, ptFNavPrj)
						M += 1
					End If
				End If
			Next ii

			M -= 1
			If M < 0 Then Continue For
			'=========================================================================================
			ValidNavs(J) = DMEList(I)

			ReDim ValidNavs(J).ValMax(1)
			ReDim ValidNavs(J).ValMin(1)

			If M >= 0 Then
				If M > 0 Then
					ValidNavs(J).ValCnt = 0
					ValidNavs(J).ValMin(0) = System.Math.Round(IntrRes1(0).Left - 0.4999999)
					ValidNavs(J).ValMax(0) = System.Math.Round(IntrRes1(0).Right + 0.4999999)
					ValidNavs(J).ValMin(1) = System.Math.Round(IntrRes1(1).Left + 0.4999999)
					ValidNavs(J).ValMax(1) = System.Math.Round(IntrRes1(1).Right + 0.4999999)
				Else
					If ValidNavs(J).ValCnt < 0 Then
						ValidNavs(J).ValMin(0) = System.Math.Round(IntrRes1(0).Left + 0.4999999)
						ValidNavs(J).ValMax(0) = System.Math.Round(IntrRes1(0).Right - 0.4999999)
					Else
						ValidNavs(J).ValMin(0) = System.Math.Round(IntrRes1(0).Left + 0.4999999)
						ValidNavs(J).ValMax(0) = System.Math.Round(IntrRes1(0).Right - 0.4999999)
					End If
				End If
			End If

			ReDim Preserve ValidNavs(J).ValMin(M)
			ReDim Preserve ValidNavs(J).ValMax(M)

			J += 1
		Next I

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	'Public Function GetValidEnRouteNavs1(ByRef ptFix As ESRI.ArcGIS.Geometry.IPoint, ByRef NomDir As Double, ByRef hFIX As Double, ByRef GuidNav As TypeDefinitions.FIXableNavaidType) As TypeDefinitions.FIXableNavaidType()
	'	Dim ValidNavs() As TypeDefinitions.FIXableNavaidType
	'	Dim Side As Integer
	'	Dim nNav As Integer
	'	Dim ii As Integer
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim K As Integer
	'	Dim N As Integer
	'	Dim nN As Integer

	'	Dim GuidRange As Double
	'	Dim InterToler As Double
	'	Dim TrackToler As Double
	'	Dim TrackRange As Double
	'	Dim ValMax As Double
	'	Dim ValMin As Double
	'	Dim InterDir As Double
	'	Dim D As Double
	'	Dim fDist As Double
	'	Dim fRConus As Double
	'	Dim fConeAngle As Double

	'	Dim pGuidConus As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pInterConus As ESRI.ArcGIS.Geometry.IPointCollection

	'	Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint

	'	Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
	'	Dim pInterPoly As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pGuidPolyLine As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim pFIXPoly As ESRI.ArcGIS.Geometry.IPointCollection

	'	Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
	'	Dim pProxi1 As ESRI.ArcGIS.Geometry.IProximityOperator
	'	'===========================================================================

	'	nNav = UBound(NavaidList) + UBound(DMEList) + 2
	'	If nNav = 0 Then
	'		ReDim ValidNavs(-1)
	'		Return ValidNavs
	'	End If

	'	ReDim ValidNavs(nNav - 1)

	'	pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
	'	pGuidPolyLine = New ESRI.ArcGIS.Geometry.Polyline

	'	If GuidNav.TypeCode = eNavaidType.CodeVOR Then
	'		TrackToler = VOR.EnRouteTrackingToler
	'		fConeAngle = VOR.ConeAngle
	'	ElseIf GuidNav.TypeCode = eNavaidType.CodeNDB Then
	'		TrackToler = NDB.EnRouteTrackingToler
	'		fConeAngle = NDB.ConeAngle
	'	End If

	'	TrackRange = GuidNav.Range / System.Math.Cos(DegToRad(TrackToler))
	'	fRConus = System.Math.Tan(DegToRad(fConeAngle)) * hFIX
	'	pGuidConus = CreatePrjCircle(GuidNav.ptPrj, fRConus)

	'	pGuidPoly.AddPoint(GuidNav.ptPrj)
	'	pGuidPoly.AddPoint(PointAlongPlane(GuidNav.ptPrj, NomDir - TrackToler, 10.0 * TrackRange))
	'	pGuidPoly.AddPoint(PointAlongPlane(GuidNav.ptPrj, NomDir + TrackToler, 10.0 * TrackRange))
	'	pGuidPoly.AddPoint(GuidNav.ptPrj)
	'	pGuidPoly.AddPoint(PointAlongPlane(GuidNav.ptPrj, NomDir - TrackToler + 180.0, 10.0 * TrackRange))
	'	pGuidPoly.AddPoint(PointAlongPlane(GuidNav.ptPrj, NomDir + TrackToler + 180.0, 10.0 * TrackRange))
	'	pGuidPoly.AddPoint(GuidNav.ptPrj)

	'	pGuidPolyLine.AddPoint(pGuidPoly.Point(1))
	'	pGuidPolyLine.AddPoint(pGuidPoly.Point(4))
	'	pGuidPolyLine.AddPoint(pGuidPoly.Point(5))
	'	pGuidPolyLine.AddPoint(pGuidPoly.Point(2))
	'	pGuidPolyLine.AddPoint(pGuidPoly.Point(1))

	'	pTopoOper = pGuidPoly
	'	pTopoOper.IsKnownSimple_2 = False
	'	pTopoOper.Simplify()

	'	'=============================================================================================

	'	J = 0
	'	pInterPoly = New ESRI.ArcGIS.Geometry.Polygon
	'	pProxi = pGuidPoly

	'	nN = UBound(NavaidList)
	'	For I = 0 To nN
	'		ptFNav = NavaidList(I).ptGeo
	'		ptFNavPrj = NavaidList(I).ptPrj

	'		GuidRange = Min(System.Math.Sqrt(hFIX) * 4130.0, NavaidList(I).Range)
	'		'        GuidRange = NavaidList(I).Range '* 1000#

	'		fDist = ReturnDistanceInMeters(ptFNavPrj, ptFix)
	'		If GuidRange < fDist Then Continue For

	'		If pProxi.ReturnDistance(ptFNavPrj) < distEps Then Continue For

	'		InterDir = ReturnAngleInDegrees(ptFNavPrj, ptFix)

	'		If (K = eNavaidType.CodeVOR) Then
	'			InterToler = VOR.EnRouteInterToler
	'			fConeAngle = VOR.ConeAngle
	'		Else
	'			InterToler = NDB.EnRouteInterToler
	'			fConeAngle = NDB.ConeAngle
	'		End If

	'		TrackRange = GuidRange / System.Math.Cos(DegToRad(TrackToler))

	'		If pInterPoly.PointCount > 0 Then pInterPoly.RemovePoints(0, pInterPoly.PointCount)

	'		pInterPoly.AddPoint(ptFNavPrj)
	'		pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir - TrackToler, TrackRange))
	'		pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir + TrackToler, TrackRange))
	'		pInterPoly.AddPoint(ptFNavPrj)
	'		pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir - TrackToler + 180.0, TrackRange))
	'		pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir + TrackToler + 180.0, TrackRange))
	'		pInterPoly.AddPoint(ptFNavPrj)

	'		pTopoOper = pInterPoly
	'		pTopoOper.IsKnownSimple_2 = False
	'		pTopoOper.Simplify()

	'		pProxi1 = pInterPoly
	'		If pProxi1.ReturnDistance(GuidNav.ptPrj) < distEps Then Continue For

	'		pFIXPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)
	'		If pProxi1.ReturnDistance(pGuidConus) < distEps Then Continue For

	'		fRConus = System.Math.Tan(DegToRad(fConeAngle)) * hFIX
	'		pInterConus = CreatePrjCircle(ptFNavPrj, fRConus)
	'		If pProxi1.ReturnDistance(pInterConus) < distEps Then Continue For

	'		N = pFIXPoly.PointCount - 1
	'		ValMax = -1.0
	'		ValMin = -1.0

	'		For ii = 0 To N
	'			D = Point2LineDistancePrj(pFIXPoly.Point(ii), ptFix, NomDir - 90.0)
	'			Side = SideDef(ptFix, NomDir - 90.0, pFIXPoly.Point(ii))
	'			If Side < 0 Then
	'				If ValMax < D Then ValMax = D
	'			Else
	'				If ValMin < D Then ValMin = D
	'			End If
	'		Next ii

	'		ReDim ValidNavs(J).ValMax(0)
	'		ReDim ValidNavs(J).ValMin(0)

	'		ValidNavs(J).ValMax(0) = ValMax
	'		ValidNavs(J).ValMin(0) = ValMin

	'		ValidNavs(J).ptGeo = ptFNav
	'		ValidNavs(J).ptPrj = ptFNavPrj
	'		ValidNavs(J).CallSign = NavaidList(I).CallSign
	'		ValidNavs(J).ID = NavaidList(I).ID
	'		ValidNavs(J).Name = NavaidList(I).Name
	'		ValidNavs(J).Index = I
	'		ValidNavs(J).MagVar = NavaidList(I).MagVar

	'		ValidNavs(J).TypeCode = NavaidList(I).TypeCode
	'		'ValidNavs(J).PairTypeCode = NavaidList(I).PairTypeCode
	'		ValidNavs(J).Range = NavaidList(I).Range

	'		J += 1
	'	Next I

	'	'''''''''''''''' DME
	'	nN = UBound(DMEList)

	'	For I = 0 To nN
	'		ptFNav = DMEList(I).ptGeo
	'		ptFNavPrj = DMEList(I).ptPrj

	'		GuidRange = Min(System.Math.Sqrt(hFIX) * 4130.0, DMEList(I).Range)
	'		'        GuidRange = DMEList(I).Range '* 1000#
	'		fDist = ReturnDistanceInMeters(ptFNavPrj, ptFix)

	'		If GuidRange < fDist Then Continue For

	'		D = hFIX / System.Math.Tan(DegToRad(90.0 - DME.SlantAngle))
	'		If D >= fDist Then Continue For '55
	'		TrackRange = GuidRange / System.Math.Cos(DegToRad(DME.TP_div)) '23

	'		If pInterPoly.PointCount > 0 Then pInterPoly.RemovePoints(0, pInterPoly.PointCount)
	'		pInterPoly.AddPoint(ptFix)
	'		pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir - DME.TP_div, TrackRange))
	'		pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir + DME.TP_div, TrackRange))
	'		pInterPoly.AddPoint(ptFix)
	'		pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir - DME.TP_div + 180.0, TrackRange))
	'		pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir + DME.TP_div + 180.0, TrackRange))
	'		pInterPoly.AddPoint(ptFix)

	'		pTopoOper = pInterPoly
	'		pTopoOper.IsKnownSimple_2 = False
	'		pTopoOper.Simplify()
	'		pProxi1 = pInterPoly
	'		If pProxi1.ReturnDistance(ptFNavPrj) <> 0.0 Then Continue For '23

	'		D = System.Math.Sqrt(fDist * fDist + hFIX * hFIX) * DME.ErrorScalingUp + DME.MinimalError
	'		fRConus = fDist - D
	'		pInterConus = CreatePrjCircle(ptFNavPrj, fRConus)

	'		pProxi1 = pGuidPolyLine
	'		If pProxi1.ReturnDistance(pInterConus) <> 0.0 Then Continue For
	'=========================================================================================
	'		ReDim ValidNavs(J).ValMax(0)
	'		ReDim ValidNavs(J).ValMin(0)
	'		ValidNavs(J).ValMax(0) = fDist
	'		ValidNavs(J).ValMin(0) = fDist

	'		ValidNavs(J).ptGeo = ptFNav
	'		ValidNavs(J).ptPrj = ptFNavPrj

	'		ValidNavs(J).CallSign = DMEList(I).CallSign
	'		ValidNavs(J).ID = DMEList(I).ID
	'		ValidNavs(J).Name = DMEList(I).Name
	'		ValidNavs(J).Index = I
	'		ValidNavs(J).MagVar = DMEList(I).MagVar

	'		ValidNavs(J).TypeCode = DMEList(I).TypeCode
	'		'ValidNavs(J).PairTypeCode = DMEList(I).PairTypeCode
	'		ValidNavs(J).Range = DMEList(I).Range

	'		J += 1
	'	Next I

	'	If J > 0 Then
	'		ReDim Preserve ValidNavs(J - 1)
	'	Else
	'		ReDim ValidNavs(-1)
	'	End If

	'	Return ValidNavs
	'End Function

	Public Function GetValidEnRouteInterNavs(ByRef ptFix As ESRI.ArcGIS.Geometry.IPoint, ByRef NomDir As Double, ByRef hFIX As Double, ByRef GuidNav As TypeDefinitions.NavaidType, ByRef MaxToler As Double) As TypeDefinitions.NavaidType()
		Dim ValidNavs() As TypeDefinitions.NavaidType
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim ii As Integer
		Dim nN As Integer
		Dim Side As Integer
		Dim nNav As Integer

		Dim D As Double
		Dim fDist As Double
		Dim ValMax As Double
		Dim ValMin As Double
		Dim fRConus As Double
		Dim InterDir As Double
		Dim fRIConus As Double
		Dim GuidRange As Double
		Dim InterToler As Double
		Dim TrackToler As Double
		Dim TrackRange As Double
		Dim fConeAngle As Double

		'Dim ptFNav As ESRI.ArcGIS.Geometry.IPoint
		Dim ptFNavPrj As ESRI.ArcGIS.Geometry.IPoint
		'Dim pGuidConus As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pInterConus As ESRI.ArcGIS.Geometry.IPointCollection

		Dim pTopoOper As ESRI.ArcGIS.Geometry.ITopologicalOperator2
		Dim pInterPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPolyLineL As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pGuidPolyLineR As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pFIXPoly As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pRings As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pTestRings As ESRI.ArcGIS.Geometry.IGeometryCollection
		Dim pProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pProxi1 As ESRI.ArcGIS.Geometry.IProximityOperator
		'===========================================================================

		nNav = UBound(NavaidList) + UBound(DMEList) + 2

		If nNav = 0 Then
			ReDim ValidNavs(-1)
			Return ValidNavs
		End If

		ReDim ValidNavs(nNav - 1)
		'===========================================================================

		pGuidPoly = New ESRI.ArcGIS.Geometry.Polygon
		pGuidPolyLineL = New ESRI.ArcGIS.Geometry.Polyline
		pGuidPolyLineR = New ESRI.ArcGIS.Geometry.Polyline

		If GuidNav.TypeCode = eNavaidType.VOR Then
			TrackToler = VOR.EnRouteTrackingToler
			fConeAngle = VOR.ConeAngle
		ElseIf GuidNav.TypeCode = eNavaidType.NDB Then
			TrackToler = NDB.EnRouteTrackingToler
			fConeAngle = NDB.ConeAngle
		End If

		TrackRange = GuidNav.Range / System.Math.Cos(DegToRad(TrackToler))
		fRConus = System.Math.Tan(DegToRad(fConeAngle)) * (hFIX - GuidNav.pPtPrj.Z)
		'pGuidConus = CreatePrjCircle(GuidNav.pPtPrj, fRConus)

		pGuidPoly.AddPoint(GuidNav.pPtPrj)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler, 10.0 * TrackRange))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler, 10.0 * TrackRange))
		pGuidPoly.AddPoint(GuidNav.pPtPrj)
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir - TrackToler + 180.0, 10.0 * TrackRange))
		pGuidPoly.AddPoint(PointAlongPlane(GuidNav.pPtPrj, NomDir + TrackToler + 180.0, 10.0 * TrackRange))
		pGuidPoly.AddPoint(GuidNav.pPtPrj)

		pGuidPolyLineL.AddPoint(pGuidPoly.Point(1))
		pGuidPolyLineL.AddPoint(pGuidPoly.Point(4))

		pGuidPolyLineR.AddPoint(pGuidPoly.Point(5))
		pGuidPolyLineR.AddPoint(pGuidPoly.Point(2))
		'pGuidPolyLine.AddPoint pGuidPoly.Point(1)

		pTopoOper = pGuidPoly
		pTopoOper.IsKnownSimple_2 = False
		pTopoOper.Simplify()

		'=============================================================================================

		pProxi = pGuidPoly
		pInterPoly = New ESRI.ArcGIS.Geometry.Polygon

		J = 0
		nN = UBound(NavaidList)

		For I = 0 To nN
			'ptFNav = NavaidList(I).pPtGeo
			ptFNavPrj = NavaidList(I).pPtPrj

			GuidRange = Math.Min(System.Math.Sqrt(hFIX) * 4130.0, NavaidList(I).Range)
			'GuidRange = NavaidList(I).Range

			fDist = ReturnDistanceInMeters(ptFNavPrj, ptFix)
			If GuidRange < fDist Then Continue For

			InterDir = ReturnAngleInDegrees(ptFNavPrj, ptFix)

			If K = eNavaidType.VOR Then
				InterToler = VOR.EnRouteInterToler
				fConeAngle = VOR.ConeAngle
				'TrackToler = VOR.EnRouteTrackingToler
			Else
				InterToler = NDB.EnRouteInterToler
				fConeAngle = NDB.ConeAngle
				'TrackToler = NDB.EnRouteTrackingToler
			End If

			fRIConus = System.Math.Tan(DegToRad(fConeAngle)) * (hFIX - ptFNavPrj.Z)
			If pProxi.ReturnDistance(ptFNavPrj) < fRIConus Then Continue For

			TrackRange = GuidRange / System.Math.Cos(DegToRad(InterToler)) 'TrackToler

			If pInterPoly.PointCount > 0 Then pInterPoly.RemovePoints(0, pInterPoly.PointCount)
			pInterPoly.AddPoint(ptFNavPrj)
			pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir - InterToler, 10.0 * TrackRange))
			pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir + InterToler, 10.0 * TrackRange))
			pInterPoly.AddPoint(ptFNavPrj)
			pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir - InterToler + 180.0, 10.0 * TrackRange))
			pInterPoly.AddPoint(PointAlongPlane(ptFNavPrj, InterDir + InterToler + 180.0, 10.0 * TrackRange))
			pInterPoly.AddPoint(ptFNavPrj)

			pTopoOper = pInterPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()

			pProxi1 = pInterPoly
			If pProxi1.ReturnDistance(GuidNav.pPtPrj) < fRConus Then Continue For
			pFIXPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)

			'        If pProxi1.ReturnDistance(pGuidConus) < distEps Then GoTo NextNAV
			'        Set pInterConus = CreateCirclePrj(ptFNavPrj, fRIConus)
			'        If pProxi1.ReturnDistance(pInterConus) < distEps Then GoTo NextNAV

			N = pFIXPoly.PointCount - 1
			ValMax = -1.0
			ValMin = -1.0

			For ii = 0 To N
				D = Point2LineDistancePrj(pFIXPoly.Point(ii), ptFix, NomDir - 90.0)
				Side = SideDef(ptFix, NomDir - 90.0, pFIXPoly.Point(ii))
				If Side < 0 Then
					If ValMax < D Then ValMax = D
				Else
					If ValMin < D Then ValMin = D
				End If
			Next ii

			If ValMin > MaxToler Then Continue For

			ValidNavs(J) = NavaidList(I)

			ReDim ValidNavs(J).ValMax(0)
			ReDim ValidNavs(J).ValMin(0)

			ValidNavs(J).ValMax(0) = ValMax
			ValidNavs(J).ValMin(0) = ValMin

			J += 1
		Next I

		nN = UBound(DMEList)
		For I = 0 To nN
			'ptFNav = DMEList(I).pPtGeo
			ptFNavPrj = DMEList(I).pPtPrj

			GuidRange = Math.Min(System.Math.Sqrt(hFIX) * 4130.0, DMEList(I).Range)
			GuidRange = DMEList(I).Range

			fDist = ReturnDistanceInMeters(ptFNavPrj, ptFix)
			If GuidRange < fDist Then Continue For

			D = (hFIX - ptFNavPrj.Z) / System.Math.Tan(DegToRad(90.0 - DME.SlantAngle))
			If D >= fDist Then Continue For '55
			TrackRange = GuidRange / System.Math.Cos(DegToRad(DME.TP_div)) '23

			If pInterPoly.PointCount > 0 Then pInterPoly.RemovePoints(0, pInterPoly.PointCount)
			pInterPoly.AddPoint(ptFix)
			pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir - DME.TP_div, TrackRange))
			pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir + DME.TP_div, TrackRange))
			pInterPoly.AddPoint(ptFix)
			pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir - DME.TP_div + 180.0, TrackRange))
			pInterPoly.AddPoint(PointAlongPlane(ptFix, NomDir + DME.TP_div + 180.0, TrackRange))
			pInterPoly.AddPoint(ptFix)

			pTopoOper = pInterPoly
			pTopoOper.IsKnownSimple_2 = False
			pTopoOper.Simplify()
			pProxi1 = pInterPoly
			If pProxi1.ReturnDistance(ptFNavPrj) <> 0.0 Then Continue For '23

			D = System.Math.Sqrt(fDist * fDist + (hFIX - ptFNavPrj.Z) * (hFIX - ptFNavPrj.Z)) * DME.ErrorScalingUp + DME.MinimalError
			pInterConus = CreatePrjCircle(ptFNavPrj, fDist - D)

			pProxi1 = pGuidPolyLineL
			If pProxi1.ReturnDistance(pInterConus) <> 0.0 Then Continue For
			pProxi1 = pGuidPolyLineR
			If pProxi1.ReturnDistance(pInterConus) <> 0.0 Then Continue For

			'=========================================================================================
			pFIXPoly = CreatePrjCircle(ptFNavPrj, fDist + D)

			pTopoOper = pFIXPoly
			pFIXPoly = pTopoOper.Difference(pInterConus)

			pTopoOper = pFIXPoly
			pFIXPoly = pTopoOper.Intersect(pGuidPoly, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry2Dimension)

			pRings = pFIXPoly
			pTestRings = pInterPoly
			pProxi1 = pInterPoly
			'        If pInterPoly.PointCount > 0 Then pInterPoly.RemovePoints 0, pInterPoly.PointCount
			ii = 0

			While ii < pRings.GeometryCount
				pTestRings.RemoveGeometries(0, pTestRings.GeometryCount)
				pTestRings.AddGeometry(pRings.Geometry(ii))
				If pProxi1.ReturnDistance(ptFix) <> 0.0 Then
					pRings.RemoveGeometries(ii, 1)
				Else
					ii += 1
				End If
			End While

			N = pFIXPoly.PointCount - 1
			ValMax = -1.0
			ValMin = -1.0

			For ii = 0 To N
				D = Point2LineDistancePrj(pFIXPoly.Point(ii), ptFix, NomDir - 90.0)
				Side = SideDef(ptFix, NomDir - 90.0, pFIXPoly.Point(ii))
				If Side < 0 Then
					If ValMax < D Then ValMax = D
				Else
					If ValMin < D Then ValMin = D
				End If
			Next ii

			If ValMin > MaxToler Then Continue For
			'=========================================================================================
			ValidNavs(J) = DMEList(I)

			ReDim ValidNavs(J).ValMax(0)
			ReDim ValidNavs(J).ValMin(0)
			ValidNavs(J).ValMax(0) = fDist
			ValidNavs(J).ValMin(0) = fDist

			J += 1
		Next I

		If J > 0 Then
			ReDim Preserve ValidNavs(J - 1)
		Else
			ReDim ValidNavs(-1)
		End If

		Return ValidNavs
	End Function

	'Public Function FixToTouchSpiral(ByRef ptBase As ESRI.ArcGIS.Geometry.IPoint, ByRef coef0 As Double, ByRef TurnR As Double, ByRef Turndir As Integer, ByRef Theta As Double, ByRef FixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef DepCourse As Double) As Double
	'	Dim R As Double
	'	Dim X1 As Double
	'	Dim Y1 As Double
	'	Dim Theta0 As Double
	'	Dim Theta1 As Double
	'       Dim Theta1New As Double
	'       Dim fTmp1 As Double
	'	Dim fTmp2 As Double
	'       Dim coef As Double
	'	Dim Dist As Double
	'	Dim FixTheta As Double
	'	Dim I As Integer
	'	Dim PtCntSpiral As ESRI.ArcGIS.Geometry.IPoint
	'	Dim ptOut As ESRI.ArcGIS.Geometry.IPoint
	'	Dim dTheta As Double
	'       Dim CntTheta As Double

	'	FixToTouchSpiral = -1000

	'	coef = RadToDeg(coef0)
	'	Theta0 = Modulus(90# * Turndir + DepCourse + 180#, 360#)
	'	PtCntSpiral = PointAlongPlane(ptBase, DepCourse + 90# * Turndir, TurnR)
	'	Dist = ReturnDistanceInMeters(PtCntSpiral, FixPnt)
	'	FixTheta = ReturnAngleInDegrees(PtCntSpiral, FixPnt)
	'	dTheta = Modulus((FixTheta - Theta0) * Turndir, 360#)
	'	R = TurnR + dTheta * coef0
	'	If Dist < R Then
	'		Exit Function
	'	End If

	'	X1 = FixPnt.X - PtCntSpiral.X
	'	Y1 = FixPnt.Y - PtCntSpiral.Y
	'	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, Turndir)
	'	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'	'===============================Variant Firdowsy ==================================
	'	Dim f As Double
	'	Dim F1 As Double
	'	Dim SinT As Double
	'	Dim CosT As Double

	'	Theta1 = CntTheta
	'	For I = 0 To 20
	'		dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'		SinT = System.Math.Sin(DegToRad(Theta1))
	'		CosT = System.Math.Cos(DegToRad(Theta1))
	'		R = TurnR + dTheta * coef0
	'		f = R * R - (Y1 * R + X1 * coef * Turndir) * SinT - (X1 * R - Y1 * coef * Turndir) * CosT
	'		F1 = 2 * R * coef * Turndir - (Y1 * R + 2 * X1 * coef * Turndir) * CosT + (X1 * R - 2 * Y1 * coef * Turndir) * SinT
	'		Theta1New = Theta1 - RadToDeg(f / F1)

	'		fTmp1 = SubtractAngles(Theta1New, Theta1)
	'		If fTmp1 < 0.0001 Then
	'			Theta1 = Theta1New
	'			Exit For
	'		End If
	'		Theta1 = Theta1New
	'	Next I

	'	dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'	R = TurnR + dTheta * coef0

	'	ptOut = PointAlongPlane(PtCntSpiral, Theta1, R)
	'	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
	'	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, Turndir)
	'	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'	fTmp2 = SubtractAngles(CntTheta, Theta1)

	'	If fTmp2 < 0.0001 Then FixToTouchSpiral = fTmp1

	'       '''Exit Function
	'       ''''=============================11==================================
	'       '''Theta1 = CntTheta
	'       '''SolFlag11 = False

	'       '''For I = 0 To 20
	'       '''	dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'       '''	'SinCoef*SinX+CosCoef*CosX = 1
	'       '''	R = TurnR + dTheta * coef0
	'       '''	SinCoef = (Y1 + Turndir * coef * X1 / R) / R
	'       '''	CosCoef = (X1 - Turndir * coef * Y1 / R) / R
	'       '''	'a*x2 + b*x + c = 0
	'       '''	A = SinCoef * SinCoef + CosCoef * CosCoef
	'       '''	B = -2 * SinCoef
	'       '''	C = 1 - CosCoef * CosCoef
	'       '''	D = B * B - 4# * A * C
	'       '''	If D < 0# Then
	'       '''		Theta1New = -D * System.Math.Sign(-B * A) * 90#
	'       '''	Else
	'       '''		sin1 = (-B + System.Math.Sqrt(D)) / (2# * A)
	'       '''		Theta1New = RadToDeg(ArcSin(sin1))
	'       '''	End If

	'       '''	fTmp1 = SubtractAngles(Theta1New, Theta1)
	'       '''	If fTmp1 < 0.0001 Then
	'       '''		SolFlag11 = True
	'       '''		Theta11 = Theta1
	'       '''		Exit For
	'       '''	End If
	'       '''	Theta1 = Theta1New
	'       '''Next I
	'       ''''=============================12==================================
	'       '''Theta1 = CntTheta
	'       '''SolFlag12 = False

	'       '''For I = 0 To 20
	'       '''	dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'       '''	'SinCoef*SinX+CosCoef*CosX = 1
	'       '''	R = TurnR + dTheta * coef0
	'       '''	SinCoef = (Y1 + Turndir * coef * X1 / R) / R
	'       '''	CosCoef = (X1 - Turndir * coef * Y1 / R) / R
	'       '''	'a*x2 + b*x + c = 0
	'       '''	A = SinCoef * SinCoef + CosCoef * CosCoef
	'       '''	B = -2 * SinCoef
	'       '''	C = 1 - CosCoef * CosCoef
	'       '''	D = B * B - 4# * A * C
	'       '''	If D < 0# Then
	'       '''		Theta1New = 180# + D * System.Math.Sign(-B * A) * 90#
	'       '''	Else
	'       '''		sin1 = (-B + System.Math.Sqrt(D)) / (2# * A)
	'       '''		Theta1New = 180# - RadToDeg(ArcSin(sin1))
	'       '''	End If

	'       '''	fTmp1 = SubtractAngles(Theta1New, Theta1)
	'       '''	If fTmp1 < 0.0001 Then
	'       '''		SolFlag12 = True
	'       '''		Theta12 = Theta1
	'       '''		Exit For
	'       '''	End If
	'       '''	Theta1 = Theta1New
	'       '''Next I
	'       ''''=============================21==================================
	'       '''Theta1 = CntTheta
	'       '''SolFlag21 = False

	'       '''For I = 0 To 20
	'       '''	dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'       '''	'SinCoef*SinX+CosCoef*CosX = 1
	'       '''	R = TurnR + dTheta * coef0
	'       '''	SinCoef = (Y1 + Turndir * coef * X1 / R) / R
	'       '''	CosCoef = (X1 - Turndir * coef * Y1 / R) / R
	'       '''	'a*x2 + b*x + c = 0
	'       '''	A = SinCoef * SinCoef + CosCoef * CosCoef
	'       '''	B = -2 * SinCoef
	'       '''	C = 1 - CosCoef * CosCoef
	'       '''	D = B * B - 4# * A * C
	'       '''	If D < 0# Then
	'       '''		Theta1New = -D * System.Math.Sign(-B * A) * 90#
	'       '''	Else
	'       '''		sin1 = (-B - System.Math.Sqrt(D)) / (2# * A)
	'       '''		Theta1New = RadToDeg(ArcSin(sin1))
	'       '''	End If

	'       '''	fTmp1 = SubtractAngles(Theta1New, Theta1)
	'       '''	If fTmp1 < 0.0001 Then
	'       '''		SolFlag21 = True
	'       '''		Theta21 = Theta1
	'       '''		Exit For
	'       '''	End If
	'       '''	Theta1 = Theta1New
	'       '''Next I
	'       ''''=============================22==================================
	'       '''Theta1 = CntTheta + 180#
	'       '''SolFlag22 = False

	'       '''For I = 0 To 20
	'       '''	dTheta = Modulus((Theta1 - Theta0) * Turndir, 360#)
	'       '''	'SinCoef*SinX+CosCoef*CosX = 1
	'       '''	R = TurnR + dTheta * coef0
	'       '''	SinCoef = (Y1 + Turndir * coef * X1 / R) / R
	'       '''	CosCoef = (X1 - Turndir * coef * Y1 / R) / R
	'       '''	'a*x2 + b*x + c = 0
	'       '''	A = SinCoef * SinCoef + CosCoef * CosCoef
	'       '''	B = -2 * SinCoef
	'       '''	C = 1 - CosCoef * CosCoef
	'       '''	D = B * B - 4# * A * C
	'       '''	If D < 0# Then
	'       '''		Theta1New = 180# + D * System.Math.Sign(-B * A) * 90#
	'       '''	Else
	'       '''		sin1 = (-B - System.Math.Sqrt(D)) / (2# * A)
	'       '''		Theta1New = 180# - RadToDeg(ArcSin(sin1))
	'       '''	End If

	'       '''	fTmp1 = SubtractAngles(Theta1New, Theta1)
	'       '''	If fTmp1 < 0.0001 Then
	'       '''		SolFlag22 = True
	'       '''		Theta22 = Theta1
	'       '''		Exit For
	'       '''	End If
	'       '''	Theta1 = Theta1New
	'       '''Next I
	'       ''''================================11=====================================
	'       '''If SolFlag11 Then
	'       '''	dTheta = Modulus((Theta11 - Theta0) * Turndir, 360#)
	'       '''	R = TurnR + dTheta * coef0
	'       '''	ptOut = PointAlongPlane(PtCntSpiral, Theta11, R)
	'       '''	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
	'       '''	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, Turndir)
	'       '''	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'       '''	fTmp2 = SubtractAngles(CntTheta, Theta11)
	'       '''	If fTmp2 < 0.0001 Then
	'       '''		FixToTouchSpiral = fTmp1
	'       '''		Exit Function
	'       '''	End If
	'       '''End If
	'       ''''================================12=====================================
	'       '''If SolFlag12 Then
	'       '''	dTheta = Modulus((Theta12 - Theta0) * Turndir, 360#)
	'       '''	R = TurnR + dTheta * coef0
	'       '''	ptOut = PointAlongPlane(PtCntSpiral, Theta12, R)
	'       '''	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
	'       '''	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, Turndir)
	'       '''	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'       '''	fTmp2 = SubtractAngles(CntTheta, Theta12)
	'       '''	If fTmp2 < 0.0001 Then
	'       '''		FixToTouchSpiral = fTmp1
	'       '''		Exit Function
	'       '''	End If
	'       '''End If
	'       ''''================================21=====================================
	'       '''If SolFlag21 Then
	'       '''	dTheta = Modulus((Theta21 - Theta0) * Turndir, 360#)
	'       '''	R = TurnR + dTheta * coef0
	'       '''	ptOut = PointAlongPlane(PtCntSpiral, Theta21, R)
	'       '''	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
	'       '''	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, Turndir)
	'       '''	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'       '''	fTmp2 = SubtractAngles(CntTheta, Theta21)
	'       '''	If fTmp2 < 0.0001 Then
	'       '''		FixToTouchSpiral = fTmp1
	'       '''		Exit Function
	'       '''	End If
	'       '''End If
	'       ''''================================22=====================================
	'       '''If SolFlag22 Then
	'       '''	dTheta = Modulus((Theta22 - Theta0) * Turndir, 360#)
	'       '''	R = TurnR + dTheta * coef0
	'       '''	ptOut = PointAlongPlane(PtCntSpiral, Theta22, R)
	'       '''	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
	'       '''	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, Turndir)
	'       '''	CntTheta = Modulus(Theta0 + CntTheta * Turndir, 360#)
	'       '''	fTmp2 = SubtractAngles(CntTheta, Theta22)
	'       '''	If fTmp2 < 0.0001 Then
	'       '''		FixToTouchSpiral = fTmp1
	'       '''		Exit Function
	'       '''	End If
	'       '''End If
	'End Function

	'Function CalcTouchByRadius(ByRef PtSt As ESRI.ArcGIS.Geometry.IPoint, ByRef ptEnd As ESRI.ArcGIS.Geometry.IPoint, ByRef R As Double, ByRef AztCur As Double, ByRef Turndir As Integer, ByRef ptOut As ESRI.ArcGIS.Geometry.IPoint, ByRef grad As Double, ByRef H As Double) As Boolean
	'	Dim RSp As Double
	'	Dim E2 As Double
	'	Dim resx As Double
	'	Dim resy As Double
	'	Dim C As Double
	'	Dim Y As Double
	'	Dim sy As Double
	'	Dim Sy2 As Double
	'	Dim Rm As Double
	'	Dim Bet As Double
	'	Dim Az3 As Double
	'	Dim ARad As Double
	'	Dim azt1 As Double
	'	Dim azt2 As Double
	'	Dim aztTemp As Double
	'	Dim deltaAz As Double
	'	Dim FWG As Double
	'	Dim L As Double
	'	Dim Dist As Double

	'	Dim r0 As ESRI.ArcGIS.Geometry.IPoint
	'       Dim I As Integer
	'	Dim J As Integer

	'	CalcTouchByRadius = False
	'	RSp = pSpheroid.SemiMajorAxis
	'	FWG = pSpheroid.Flattening
	'	E2 = FWG * (2 - FWG)
	'	J = PointAlongGeodesic(PtSt.X, PtSt.Y, R, AztCur + (90 * Turndir), resx, resy)
	'	r0 = New ESRI.ArcGIS.Geometry.Point
	'	r0.PutCoords(resx, resy)
	'	C = ReturnGeodesicDistance(resx, resy, ptEnd.X, ptEnd.Y)

	'	If (C < R) Then
	'		Dist = ReturnGeodesicDistance(PtSt.X, PtSt.Y, ptEnd.X, ptEnd.Y)
	'		ReturnGeodesicAzimuth(PtSt.X, PtSt.Y, ptEnd.X, ptEnd.Y, azt1, azt2)
	'		PointAlongGeodesic(PtSt.X, PtSt.Y, 0.5 * Dist, azt1, resx, resy)
	'		Calc2VectIntersect(resx, resy, azt1 + 90#, PtSt.X, PtSt.Y, AztCur + 90#, resx, resy)
	'		Dist = ReturnGeodesicDistance(PtSt.X, PtSt.Y, resx, resy)

	'		MessageBox.Show(My.Resources.str504 + vbCrLf + My.Resources.str505+ vbCrLf + My.Resources.str506 + CStr(Fix(Dist) - 1) + My.Resources.str507)
	'		R = 0
	'		Exit Function
	'	End If

	'	Y = r0.Y * Pi / 180
	'	sy = System.Math.Sin(Y)
	'	Sy2 = sy * sy
	'	Rm = (System.Math.Sqrt(1 - E2) / (1 - E2 * Sy2)) * RSp
	'	C = C / Rm
	'	Rm = R / Rm
	'	Bet = System.Math.Tan(Rm) / System.Math.Tan(C)
	'	Bet = System.Math.Atan(-Bet / System.Math.Sqrt(-Bet * Bet + 1)) + 2 * System.Math.Atan(1)
	'	J = ReturnGeodesicAzimuth(r0.X, r0.Y, ptEnd.X, ptEnd.Y, Az3, resx)
	'	Bet = RadToDeg(Bet) ' * 180 / PI
	'	If (C < Rm) Then
	'		MessageBox.Show("Error")
	'		Exit Function
	'	End If

	'	ARad = Az3 - (Bet * Turndir)
	'	J = PointAlongGeodesic(r0.X, r0.Y, R, ARad, resx, resy)

	'	For I = 0 To 10
	'		J = ReturnGeodesicAzimuth(resx, resy, r0.X, r0.Y, azt1, aztTemp)
	'		J = ReturnGeodesicAzimuth(resx, resy, ptEnd.X, ptEnd.Y, azt2, aztTemp)
	'		deltaAz = System.Math.Abs(azt1 - azt2)
	'		If (deltaAz > 180#) Then
	'			deltaAz = 360# - deltaAz
	'		End If
	'		deltaAz = (deltaAz - 90#) * Turndir
	'		If (System.Math.Abs(deltaAz) < 0.00000001) Then
	'			Exit For
	'		End If
	'		ARad = ARad - deltaAz
	'		J = PointAlongGeodesic(r0.X, r0.Y, R, ARad, resx, resy)
	'	Next I

	'	ptOut.PutCoords(resx, resy)
	'	J = ReturnGeodesicAzimuth(r0.X, r0.Y, resx, resy, azt2, aztTemp)
	'	J = ReturnGeodesicAzimuth(r0.X, r0.Y, PtSt.X, PtSt.Y, azt1, aztTemp)
	'	deltaAz = (azt2 - azt1) * Turndir

	'	deltaAz = Modulus(deltaAz, 360#)
	'	'If (deltaAz < 0) Then deltaAz = deltaAz + 360
	'	L = R * DegToRad(deltaAz)
	'	H = L * grad * 0.01

	'	CalcTouchByRadius = True
	'End Function

	'Sub CreateNavaidZone(ByRef ptNav As ESRI.ArcGIS.Geometry.IPoint, ByRef NavType As Integer, ByRef DirAngle As Double, ByRef Multiplicity As Double, ByRef LPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef RPolygon As ESRI.ArcGIS.Geometry.IPointCollection, ByRef PrimPolygon As ESRI.ArcGIS.Geometry.IPointCollection, Optional ByRef HalfPast As Boolean = False)
	'	Dim pt0 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt1 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt2 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt3 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt4 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pt5 As ESRI.ArcGIS.Geometry.IPoint
	'	Dim dZone As Double
	'	Dim BaseLength As Double
	'	Dim Alpha As Double
	'	Dim Betta As Double
	'	Dim D0 As Double

	'	If NavType = eNavaidType.CodeNDB Then
	'		BaseLength = NDB.InitWidth * 0.5
	'		Alpha = NDB.SplayAngle
	'		dZone = NDB.Range * Multiplicity
	'	ElseIf NavType = eNavaidType.CodeVOR Then
	'		BaseLength = 0.5 * VOR.InitWidth
	'		Alpha = VOR.SplayAngle
	'		dZone = VOR.Range * Multiplicity
	'	Else
	'		Exit Sub
	'	End If

	'	D0 = dZone / System.Math.Cos(DegToRad(Alpha))
	'	Betta = 0.5 * System.Math.Tan(DegToRad(Alpha))
	'	Betta = System.Math.Atan(Betta)
	'	Betta = RadToDeg(Betta)

	'	If LPolygon.PointCount > 0 Then LPolygon.RemovePoints(0, LPolygon.PointCount)
	'	If RPolygon.PointCount > 0 Then RPolygon.RemovePoints(0, RPolygon.PointCount)
	'	If PrimPolygon.PointCount > 0 Then PrimPolygon.RemovePoints(0, PrimPolygon.PointCount)

	'	'==========LeftPolygon
	'	pt0 = PointAlongPlane(ptNav, DirAngle + 90.0 , BaseLength)
	'	pt3 = PointAlongPlane(ptNav, DirAngle + 90.0 , 0.5 * BaseLength)
	'	pt1 = PointAlongPlane(pt0, DirAngle + Alpha, D0)
	'	pt2 = PointAlongPlane(pt3, DirAngle + Betta, D0)
	'	pt4 = PointAlongPlane(pt3, DirAngle + 180.0  - Betta, D0)
	'	pt5 = PointAlongPlane(pt0, DirAngle + 180.0  - Alpha, D0)

	'	LPolygon.AddPoint(pt0)
	'	LPolygon.AddPoint(pt1)
	'	LPolygon.AddPoint(pt2)
	'	LPolygon.AddPoint(pt3)
	'	If Not HalfPast Then
	'		LPolygon.AddPoint(pt4)
	'		LPolygon.AddPoint(pt5)
	'	End If

	'	If Not HalfPast Then
	'		PrimPolygon.AddPoint(pt4)
	'	End If
	'	PrimPolygon.AddPoint(pt3)
	'	PrimPolygon.AddPoint(pt2)

	'	'==========RightPolygon
	'	pt0 = PointAlongPlane(ptNav, DirAngle - 90.0 , 0.5 * BaseLength)
	'	pt3 = PointAlongPlane(ptNav, DirAngle - 90.0 , BaseLength)
	'	pt1 = PointAlongPlane(pt0, DirAngle - Betta, D0)
	'	pt2 = PointAlongPlane(pt3, DirAngle - Alpha, D0)
	'	pt4 = PointAlongPlane(pt3, DirAngle + 180.0  + Alpha, D0)
	'	pt5 = PointAlongPlane(pt0, DirAngle + 180.0  + Betta, D0)

	'	RPolygon.AddPoint(pt0)
	'	RPolygon.AddPoint(pt1)
	'	RPolygon.AddPoint(pt2)
	'	RPolygon.AddPoint(pt3)
	'	If Not HalfPast Then
	'		RPolygon.AddPoint(pt4)
	'		RPolygon.AddPoint(pt5)
	'	End If
	'	'RPolygon.AddPoint Pt0

	'	PrimPolygon.AddPoint(pt1)
	'	PrimPolygon.AddPoint(pt0)
	'	If Not HalfPast Then
	'		PrimPolygon.AddPoint(pt5)
	'	End If
	'	'PrimPolygon.AddPoint PrimPolygon.Point(0)
	'End Sub

	'Public Function CreateBasePoints(ByVal pPolygone As ESRI.ArcGIS.Geometry.IPointCollection, ByVal K1K1 As ESRI.ArcGIS.Geometry.IPolyline, ByVal lDepDir As Double, ByVal lTurnDir As Integer) As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim tmpPoly As ESRI.ArcGIS.Geometry.IPointCollection
	'	Dim bFlg As Boolean
	'	Dim I As Integer
	'	Dim N As Integer
	'	Dim Side As Integer

	'	bFlg = False
	'	N = pPolygone.PointCount
	'	tmpPoly = New ESRI.ArcGIS.Geometry.Polyline
	'	CreateBasePoints = New ESRI.ArcGIS.Geometry.Polygon

	'	If lTurnDir > 0 Then
	'		For I = 0 To N - 1
	'			Side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
	'			If (Side < 0) Then
	'				If bFlg Then
	'					CreateBasePoints.AddPoint(pPolygone.Point(I))
	'				Else
	'					tmpPoly.AddPoint(pPolygone.Point(I))
	'				End If
	'			ElseIf Not bFlg Then
	'				bFlg = True
	'				CreateBasePoints.AddPoint(K1K1.FromPoint)
	'				CreateBasePoints.AddPoint(K1K1.ToPoint)
	'			End If
	'		Next
	'	Else
	'		For I = N - 1 To 0 Step -1
	'			Side = SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.Point(I))
	'			If (Side < 0) Then
	'				If bFlg Then
	'					CreateBasePoints.AddPoint(pPolygone.Point(I))
	'				Else
	'					tmpPoly.AddPoint(pPolygone.Point(I))
	'				End If
	'			ElseIf Not bFlg Then
	'				bFlg = True
	'				CreateBasePoints.AddPoint(K1K1.ToPoint)
	'				CreateBasePoints.AddPoint(K1K1.FromPoint)
	'			End If
	'		Next
	'	End If

	'	CreateBasePoints.AddPointCollection(tmpPoly)
	'End Function

End Module