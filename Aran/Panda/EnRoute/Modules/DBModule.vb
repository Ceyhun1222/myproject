Option Strict Off
Option Explicit On

Imports System.Collections.Generic
Imports Aran.Aim
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Features
Imports Aran.Aim.Enums
Imports Aran.Converters
Imports Aran.Geometries
Imports Aran.Aim.Data
Imports Aran.Aim.Data.Filters
Imports Aran.Queries.Panda_2
Imports Aran.Queries
Imports System.Runtime.InteropServices

<ComVisibleAttribute(False)> Module DBModule

	Public pObjectDir As IPandaSpecializedQPI

	Public Function FillADHPFields(ByRef CurrADHP As ADHPType) As Integer
		Dim pADHP As AirportHeliport

		Dim pRelational As ESRI.ArcGIS.Geometry.IRelationalOperator
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		If Not CurrADHP.pPtGeo Is Nothing Then Return 0

		pADHP = pObjectDir.GetFeature(Aran.Aim.FeatureType.AirportHeliport, CurrADHP.Identifier)

		CurrADHP.pAirportHeliport = pADHP

		If pADHP Is Nothing Then
			Throw New AranEnvironment.AranException("Error on GetFeature [FillADHP - Identifier: " + CurrADHP.Identifier.ToString() + "]")
			Return -1
		End If

		If pADHP.ARP Is Nothing Then
			Throw New AranEnvironment.AranException(
				String.Format("Current Airport ({0}) Point is Empty", pADHP.Designator), AranEnvironment.ExceptionType.Warning)
		End If

		pPtGeo = ARANPointToESRIPoint(pADHP.ARP.Geo)
		pPtGeo.Z = ConverterToSI.Convert(pADHP.FieldElevation, 0)

		pPtPrj = ToPrj(pPtGeo)
		If pPtPrj.IsEmpty() Then
			ShowErrorMessage("Error on Geom Project [FillADHP - Identifier: " + CurrADHP.Identifier.ToString() + "]")
			Return -1
		End If

		pRelational = P_LicenseRect
		If Not pRelational.Contains(pPtPrj) Then
			ShowErrorMessage("Internal Error: #512-1 [FillADHP]")
			Return -1
		End If

		CurrADHP.pPtGeo = pPtGeo
		CurrADHP.pPtPrj = pPtPrj
		CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier

		If pADHP.MagneticVariation Is Nothing Then
			CurrADHP.MagVar = 0.0
		Else
			CurrADHP.MagVar = pADHP.MagneticVariation.Value
		End If

		'CurrADHP.ISAtC = fAirportISAtC.Value   'pADHP.ReferenceTemperature
		'CurrADHP.WindSpeed = 3.6 * depWS.Value  'pADHP.
		CurrADHP.Elev = pPtGeo.Z
		CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0)

		Return 1

	End Function

	Public Function FillNavaidList(ByRef NavaidList() As NavaidType, ByRef DMEList() As NavaidType, ByRef TACANList() As NavaidType, ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon) As Long
		Dim NavTypeCode As Integer
		Dim iNavaidNum As Integer
		Dim iTACANNum As Integer
		Dim iDMENum As Integer
		Dim index As Integer
		Dim I As Integer
		Dim J As Integer

		Dim AixmNavaidEquipment As NavaidEquipment
		Dim pNavaidList As List(Of Navaid)
		Dim pNavaid As Navaid
		Dim pAIXMDME As DME

		Dim pARANPolygon As Aran.Geometries.MultiPolygon

		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim pElevPoint As ElevatedPoint

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))
		pNavaidList = pObjectDir.GetNavaidList(pARANPolygon)

		'pNavaidList = pObjectDir.GetNavaidList()

		If (pNavaidList.Count = 0) Then	'And (ILSDataList.Count = 0)
			ReDim NavaidList(-1)
			ReDim TACANList(-1)
			ReDim DMEList(-1)
			Return 0
		End If

		ReDim NavaidList(pNavaidList.Count - 1)	'+ ILSDataList.Count
		ReDim DMEList(pNavaidList.Count - 1)
		ReDim TACANList(pNavaidList.Count - 1)

		' .GetNavaidEquipmentList (pARANPolygon)
		' Set ILSDataList = pObjectDir.GetILSNavaidEquipmentList(CurrADHP.ID)

		iNavaidNum = -1
		iTACANNum = -1
		iDMENum = -1
		index = -1

		For J = 0 To pNavaidList.Count - 1
			pNavaid = pNavaidList.Item(J)
			For I = 0 To pNavaid.NavaidEquipment.Count - 1
				If pNavaid.NavaidEquipment(I).TheNavaidEquipment Is Nothing Then Continue For

				AixmNavaidEquipment = pNavaid.NavaidEquipment(I).TheNavaidEquipment.GetFeature()
				If AixmNavaidEquipment Is Nothing Then Continue For

				Select Case AixmNavaidEquipment.NavaidEquipmentType
					Case NavaidEquipmentType.VOR
						NavTypeCode = eNavaidType.VOR
					Case NavaidEquipmentType.DME
						NavTypeCode = eNavaidType.DME
					Case NavaidEquipmentType.NDB
						NavTypeCode = eNavaidType.NDB
					Case NavaidEquipmentType.TACAN
						NavTypeCode = eNavaidType.TACAN
					Case Else
						Continue For
				End Select

				pElevPoint = AixmNavaidEquipment.Location
				If pElevPoint Is Nothing Then Continue For

				pPtGeo = AIXMPointToESRIPoint(pElevPoint)
				pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, 0.0)
				pPtPrj = ToPrj(pPtGeo)

				If pPtPrj.IsEmpty() Then Continue For

				If NavTypeCode = eNavaidType.DME Then
					iDMENum = iDMENum + 1

					DMEList(iDMENum).pPtGeo = pPtGeo
					DMEList(iDMENum).pPtPrj = pPtPrj

					If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
						DMEList(iDMENum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
					Else
						DMEList(iDMENum).MagVar = 0.0
					End If

					pAIXMDME = DirectCast(AixmNavaidEquipment, DME)

					If (Not pAIXMDME.Displace Is Nothing) Then
						DMEList(iDMENum).Disp = ConverterToSI.Convert(pAIXMDME.Displace, 0)
					Else
						DMEList(iDMENum).Disp = 0.0
					End If

					DMEList(iDMENum).Range = 450000.0 'DME.Range
					'DMEList(iDMENum).PairNavaidIndex = -1

					DMEList(iDMENum).Name = AixmNavaidEquipment.Name
					DMEList(iDMENum).NAV_Ident = pNavaid.Identifier
					DMEList(iDMENum).Identifier = AixmNavaidEquipment.Identifier
					DMEList(iDMENum).CallSign = AixmNavaidEquipment.Designator

					DMEList(iDMENum).TypeCode = NavTypeCode

					index += 1
					DMEList(iDMENum).Index = index

					'DMEList(iDMENum).pFeature = pNavaid
				ElseIf NavTypeCode = eNavaidType.TACAN Then
					iTACANNum = iTACANNum + 1

					TACANList(iTACANNum).pPtGeo = pPtGeo
					TACANList(iTACANNum).pPtPrj = pPtPrj

					If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
						TACANList(iTACANNum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
					Else
						TACANList(iTACANNum).MagVar = 0.0		'pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate) '
					End If

					TACANList(iTACANNum).Range = 450000.0 'TACAN.Range
					'TACANList(iTACANNum).PairNavaidIndex = -1

					TACANList(iTACANNum).Name = AixmNavaidEquipment.Name
					TACANList(iTACANNum).NAV_Ident = pNavaid.Identifier
					TACANList(iTACANNum).Identifier = AixmNavaidEquipment.Identifier
					TACANList(iTACANNum).CallSign = AixmNavaidEquipment.Designator

					TACANList(iTACANNum).TypeCode = NavTypeCode
					index += 1
					TACANList(iTACANNum).Index = index
					'TACANList(iTACANNum).pFeature = pNavaid

					iNavaidNum = iNavaidNum + 1
					NavaidList(iNavaidNum) = TACANList(iTACANNum)

					iDMENum = iDMENum + 1
					DMEList(iDMENum) = TACANList(iTACANNum)
				Else
					iNavaidNum = iNavaidNum + 1

					NavaidList(iNavaidNum).pPtGeo = pPtGeo
					NavaidList(iNavaidNum).pPtPrj = pPtPrj

					If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
						NavaidList(iNavaidNum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
					Else
						NavaidList(iNavaidNum).MagVar = 0.0
					End If

					If NavTypeCode = eNavaidType.NDB Then
						NavaidList(iNavaidNum).Range = 450000.0	'NDB.Range
					Else
						NavaidList(iNavaidNum).Range = 450000.0	'VOR.Range
					End If
					'NavaidList(iNavaidNum).PairNavaidIndex = -1

					NavaidList(iNavaidNum).Name = AixmNavaidEquipment.Name
					NavaidList(iNavaidNum).NAV_Ident = pNavaid.Identifier
					NavaidList(iNavaidNum).Identifier = AixmNavaidEquipment.Identifier

					NavaidList(iNavaidNum).CallSign = AixmNavaidEquipment.Designator

					NavaidList(iNavaidNum).TypeCode = NavTypeCode

					index += 1
					NavaidList(iNavaidNum).Index = index

					'NavaidList(iNavaidNum).pFeature = pNavaid
				End If
			Next I
		Next J

		'For J = 0 To iNavaidNum
		'	For I = 0 To iDMENum
		'		fDist = ReturnDistanceInMeters(NavaidList(J).pPtPrj, DMEList(I).pPtPrj)
		'		If fDist <= 2.0 Then
		'			NavaidList(J).PairNavaidIndex = I
		'			DMEList(I).PairNavaidIndex = J
		'			Exit For
		'		End If
		'	Next I
		'Next J

		'    For I = 0 To ILSDataList.Count - 1
		'        Set AixmNavaidEquipment = ILSDataList.Item(I)
		'
		'        If Not (TypeOf AixmNavaidEquipment Is ILocalizer) Then Continue For
		'
		'        Set pElevPoint = AixmNavaidEquipment.ElevatedPoint
		'        Set pGMLPoint = pElevPoint
		'        Set pAIXMLocalizer = AixmNavaidEquipment
		'
		'        If pAIXMLocalizer.TrueBearing Is Nothing Then Continue For

		'        Set pPtGeo = pGMLPoint.Tag
		'        pPtGeo.Z = pConverter.Convert(pElevPoint.elevation, CurrADHP.Elev)
		'        pPtGeo.M = pAIXMLocalizer.TrueBearing
		'
		'        Set pPtPrj = ToPrj(pPtGeo)
		'        If pPtPrj.IsEmpty() Then Continue For
		'
		'        pPtPrj.M = Azt2Dir(pPtGeo, pPtGeo.M)
		''=====================================================
		'
		'        iNavaidNum = iNavaidNum + 1
		'
		'        Set NavaidList(iNavaidNum).pPtGeo = pPtGeo
		'        Set NavaidList(iNavaidNum).pPtPrj = pPtPrj
		'
		'        NavaidList(iNavaidNum).Course = pPtGeo.M
		''        dX = RWY.pPtPrj(eRWY.PtTHR).X - pPtPrj.X
		''        dY = RWY.pPtPrj(eRWY.PtTHR).Y - pPtPrj.Y
		''        NavaidList(iNavaidNum).LLZ_THR = Sqr(dX * dX + dY * dY)
		'
		'        If Not pAIXMLocalizer.WidthCourse Is Nothing Then
		'            NavaidList(iNavaidNum).AngleWidth = pAIXMLocalizer.WidthCourse
		''           NavaidList(iNavaidNum).SecWidth = NavaidList(iNavaidNum).LLZ_THR * Tan(DegToRad(NavaidList(iNavaidNum).AngleWidth))  'NavaidList(iNavaidNum).SecWidth = pAIXMLocalizer.WidthCourse
		'        End If
		''=====================================================
		'
		'        If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
		'            NavaidList(iNavaidNum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
		'        Else
		'            NavaidList(iNavaidNum).MagVar = CurrADHP.MagVar
		'        End If
		'
		'        NavaidList(iNavaidNum).Name = AixmNavaidEquipment.Name
		'        NavaidList(iNavaidNum).ID = AixmNavaidEquipment.identifier
		'        NavaidList(iNavaidNum).CallSign = AixmNavaidEquipment.Designator
		'
		'        NavaidList(iNavaidNum).Range = 35000#
		'        NavaidList(iNavaidNum).TypeCode = CodeLLZ
		'        NavaidList(iNavaidNum).TypeName = "ILS"
		'
		''    GP_RDH = 0

		''    GPAngle = 0

		'    Next I

		If iNavaidNum > -1 Then
			ReDim Preserve NavaidList(iNavaidNum)
		Else
			ReDim NavaidList(-1)
		End If

		If iDMENum > -1 Then
			ReDim Preserve DMEList(iDMENum)
		Else
			ReDim DMEList(-1)
		End If

		If iTACANNum > -1 Then
			ReDim Preserve TACANList(iTACANNum)
		Else
			ReDim TACANList(-1)
		End If

		Return index + 1
	End Function

	Public Function FillWPT_FIXList(ByRef WPTList() As NavaidType, ByVal pPolygon As ESRI.ArcGIS.Geometry.Polygon) As Integer
		Dim iWPTNum As Integer
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer
		Dim NavTypeCode As Integer

		Dim AIXMWPTList As List(Of DesignatedPoint)

		Dim pNavaidList As List(Of Navaid)

		Dim AIXMWPT As DesignatedPoint

		Dim pNavaid As Navaid
		Dim AIXMNAVEq As NavaidEquipment

		Dim pARANPolygon As Aran.Geometries.MultiPolygon

		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint


		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))

		AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon)
		pNavaidList = pObjectDir.GetNavaidList(pARANPolygon)

		N = AIXMWPTList.Count + 2 * pNavaidList.Count - 1
		If N < 0 Then
			ReDim WPTList(-1)
			FillWPT_FIXList = -1
			Exit Function
		End If

		iWPTNum = -1
		ReDim WPTList(N)

		For I = 0 To AIXMWPTList.Count - 1
			AIXMWPT = AIXMWPTList.Item(I)

			pPtGeo = AIXMPointToESRIPoint(AIXMWPT.Location)
			pPtGeo.Z = 0.0

			pPtPrj = ToPrj(pPtGeo)

			If pPtPrj.IsEmpty() Then Continue For
			If AIXMWPT.Designator = Nothing Then Continue For

			iWPTNum = iWPTNum + 1

			WPTList(iWPTNum).MagVar = 0.0 'CurrADHP.MagVar

			WPTList(iWPTNum).pPtGeo = pPtGeo
			WPTList(iWPTNum).pPtPrj = pPtPrj

			WPTList(iWPTNum).Name = AIXMWPT.Designator
			WPTList(iWPTNum).CallSign = WPTList(iWPTNum).Name

			WPTList(iWPTNum).Identifier = AIXMWPT.Identifier

			'WPTList(iWPTNum).pFeature = AIXMWPT
			WPTList(iWPTNum).TypeCode = eNavaidType.NONE
		Next I

		'======================================================================

		For J = 0 To pNavaidList.Count - 1
			pNavaid = pNavaidList.Item(J)

			For I = 0 To pNavaid.NavaidEquipment.Count - 1

				If pNavaid.NavaidEquipment(I).TheNavaidEquipment Is Nothing Then
					Continue For
				End If

				AIXMNAVEq = pNavaid.NavaidEquipment(I).TheNavaidEquipment.GetFeature()

				If AIXMNAVEq Is Nothing Then Continue For

				If (TypeOf AIXMNAVEq Is Aran.Aim.Features.VOR) Then
					NavTypeCode = eNavaidType.VOR
				ElseIf (TypeOf AIXMNAVEq Is Aran.Aim.Features.NDB) Then
					NavTypeCode = eNavaidType.NDB
				ElseIf (TypeOf AIXMNAVEq Is Aran.Aim.Features.TACAN) Then
					NavTypeCode = eNavaidType.TACAN
				Else
					Continue For
				End If

				If AIXMNAVEq.Location Is Nothing Then Continue For

				pPtGeo = AIXMPointToESRIPoint(AIXMNAVEq.Location)
				pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, 0)

				pPtPrj = ToPrj(pPtGeo)
				If pPtPrj.IsEmpty() Then Continue For

				iWPTNum = iWPTNum + 1

				WPTList(iWPTNum).pPtGeo = pPtGeo
				WPTList(iWPTNum).pPtPrj = pPtPrj

				If Not AIXMNAVEq.MagneticVariation Is Nothing Then
					WPTList(iWPTNum).MagVar = AIXMNAVEq.MagneticVariation.Value
				Else
					WPTList(iWPTNum).MagVar = 0.0 'CurrADHP.MagVar
				End If

				WPTList(iWPTNum).Name = AIXMNAVEq.Designator
				WPTList(iWPTNum).NAV_Ident = pNavaid.Identifier
				WPTList(iWPTNum).Identifier = AIXMNAVEq.Identifier

				'WPTList(iWPTNum).pFeature = pNavaid

				If NavTypeCode = eNavaidType.NDB Then
					WPTList(iWPTNum).Range = 450000.0  'NDB.Range
				Else
					WPTList(iWPTNum).Range = 450000.0  'VOR.Range
				End If

				WPTList(iWPTNum).TypeCode = NavTypeCode
			Next I
		Next J
		'======================================================================
		If iWPTNum < 0 Then
			ReDim WPTList(-1)
		Else
			ReDim Preserve WPTList(iWPTNum)
		End If

		Return iWPTNum + 1
	End Function

	Public Function FillObstacleList(ByRef Obstacles() As TypeDefinitions.ObstacleType, ByRef pPoly As ESRI.ArcGIS.Geometry.Polygon, Optional ByVal fRefHeight As Double = 0.0) As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer

		Dim Z As Double
		Dim HorAccuracy As Double
		Dim VertAccuracy As Double

		Dim VerticalStructureList As List(Of VerticalStructure)
		Dim ObstaclePart As VerticalStructurePart
		Dim AixmObstacle As VerticalStructure

		Dim pElevatedPoint As ElevatedPoint
		'Dim pElevatedCurve As ElevatedCurve
		'Dim pElevatedSurface As ElevatedSurface

		Dim pARANPolygon As Aran.Geometries.MultiPolygon
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IPoint

		'Dim pZv As ESRI.ArcGIS.Geometry.IZ
		Dim pZAware As ESRI.ArcGIS.Geometry.IZAware
		'Dim pTopop As ESRI.ArcGIS.Geometry.ITopologicalOperator
		'Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPoly))

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1
		ReDim Obstacles(N)

		If N < 0 Then Return -1

		K = -1

		For I = 0 To N
			AixmObstacle = VerticalStructureList.Item(I)
			M = AixmObstacle.Part.Count - 1

			For J = 0 To M
				ObstaclePart = AixmObstacle.Part(J)
				If ObstaclePart.HorizontalProjection Is Nothing Then Continue For

				Select Case ObstaclePart.HorizontalProjection.Choice
					Case VerticalStructurePartGeometryChoice.ElevatedPoint
						pElevatedPoint = ObstaclePart.HorizontalProjection.Location
						If pElevatedPoint Is Nothing Then Continue For
						If pElevatedPoint.Elevation Is Nothing Then Continue For

						HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
						VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0)

						pGeomGeo = AIXMPointToESRIPoint(pElevatedPoint)
						Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0)
						'Case VerticalStructurePartGeometryChoice.ElevatedCurve
						'	Continue For
						'	pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent
						'	If pElevatedCurve Is Nothing Then Continue For
						'	If pElevatedCurve.Elevation Is Nothing Then Continue For

						'	HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0)
						'	VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0)

						'	pGeomGeo = AIXMCurveToESRIPolyline(pElevatedCurve)
						'	Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0)
						'Case VerticalStructurePartGeometryChoice.ElevatedSurface
						'	Continue For
						'	pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent
						'	If pElevatedSurface Is Nothing Then Continue For
						'	If pElevatedSurface.Elevation Is Nothing Then Continue For

						'	HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0)
						'	VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0)

						'	pGeomGeo = AIXMSurfaceToESRIPolygon(pElevatedSurface)
						'	Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0)
					Case Else
						Continue For
				End Select

				pGeomGeo.SpatialReference = pSpRefShp
				pGeomPrj = ToPrj(pGeomGeo)
				If pGeomPrj.IsEmpty() Then Continue For

				If VertAccuracy > 0.0 Then Z += VertAccuracy

				'HorAccuracy = 0.0
				'If HorAccuracy > 0.0 Then
				'If pGeomPrj.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint And (HorAccuracy <= 2.0) Then
				'pGeomPrj = CreatePrjCircle(pGeomPrj, HorAccuracy, 18)
				'Else
				'	pTopop = pGeomPrj
				'	pTopop.Simplify()
				'	pGeomPrj = pTopop.Buffer(HorAccuracy)

				'	pTopo = pGeomPrj
				'	pTopo.IsKnownSimple_2 = False
				'	pTopo.Simplify()
				'End If

				'pGeomGeo = ToGeo(pGeomPrj)
				'End If

				pZAware = pGeomGeo
				pZAware.ZAware = True

				pZAware = pGeomPrj
				pZAware.ZAware = True
				pGeomGeo.Z = Z
				pGeomPrj.Z = Z

				'If pGeomGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
				'	CType(pGeomGeo, ESRI.ArcGIS.Geometry.IPoint).Z = Z
				'	CType(pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z = Z
				'Else
				'	pZv = pGeomGeo
				'	pZv.SetConstantZ(Z)

				'	pZv = pGeomPrj
				'	pZv.SetConstantZ(Z)
				'End If

				K += 1
				'If K > C Then
				'	C += N
				'	ReDim Preserve Obstacles(C)
				'End If

				Obstacles(K).ptGeo = pGeomGeo
				Obstacles(K).ptPrj = pGeomPrj
				Obstacles(K).UnicalName = AixmObstacle.Name

				If AixmObstacle.Type Is Nothing Then
					Obstacles(K).TypeName = ""
				Else
					Obstacles(K).TypeName = AixmObstacle.Type.ToString()
				End If

				'Obstacles(K).Identifier = AixmObstacle.Id
				Obstacles(K).Identifier = AixmObstacle.Identifier

				'Obstacles(K).HorAccuracy = HorAccuracy
				'Obstacles(K).VertAccuracy = VertAccuracy
				Obstacles(K).pFeature = AixmObstacle

				Obstacles(K).Height = Z - fRefHeight
				Obstacles(K).Index = K
			Next J
		Next I

		If K >= 0 Then
			ReDim Preserve Obstacles(K)
		Else
			ReDim Obstacles(-1)
		End If

		Return K
	End Function

	Public Function CreateValDistanceType(ByVal uom As UomDistance, ByVal value As Double) As ValDistance
		Dim res As New ValDistance
		res.Uom = uom
		res.Value = CDec(value)
		Return res
	End Function

	Public Function CreateValAltitudeType(ByVal uom As UomDistanceVertical, ByVal Value As Double) As ValDistanceVertical
		Dim res As New ValDistanceVertical
		res.Uom = uom
		res.Value = CStr(Value)
		Return res
	End Function

	Public Function CreateDesignatedPoint(ByVal pPtPrj As ESRI.ArcGIS.Geometry.IPoint, Optional ByVal Name As String = "COORD", Optional ByVal fDirTreshold As Double = -1000.0, Optional ByVal fDistTreshold As Double = 0.1) As DesignatedPoint
		Dim pFixDesignatedPoint As DesignatedPoint
		Dim fMinDist As Double
		Dim fDist As Double
		Dim fDirToPt As Double

		Dim I As Long
		Dim N As Long
		Dim bExist As Boolean
		Dim WptFIX As NavaidType = New NavaidType()

		fMinDist = 10000.0
		N = UBound(WPTList)

		If N >= 0 Then
			For I = 0 To N
				fDist = ReturnDistanceInMeters(pPtPrj, WPTList(I).pPtPrj)

				If (fDist < fMinDist) And (WPTList(I).TypeCode = eNavaidType.NONE) Then
					fMinDist = fDist
					WptFIX = WPTList(I)
				End If
			Next I
		End If

		bExist = fMinDist <= fDistTreshold

		If (Not bExist) And (fMinDist <= 100.0) And (fDirTreshold <> -1000.0) Then
			fDirToPt = ReturnAngleInDegrees(pPtPrj, WptFIX.pPtPrj)
			bExist = SubtractAngles(fDirTreshold, fDirToPt) < 0.1
		End If

		If bExist Then
			Return WptFIX.GetFeature()
		End If

		pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature(Of DesignatedPoint)()
		pFixDesignatedPoint.Designator = "COORD"
		pFixDesignatedPoint.Name = Name

		pFixDesignatedPoint.Location = ESRIPointToAixmPoint(ToGeo(pPtPrj))
		'pFixDesignatedPoint.Note
		'pFixDesignatedPoint.Tag
		pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.DESIGNED
		Return pFixDesignatedPoint
	End Function

	Public Function CreateAngleIndication(ByVal Angle As Double, ByVal AngleType As Aran.Aim.Enums.CodeBearing, ByVal pSignificantPoint As SignificantPoint) As AngleIndication
		Dim pAngleIndication As AngleIndication

		pAngleIndication = DBModule.pObjectDir.CreateFeature(Of AngleIndication)()
		pAngleIndication.Angle = Angle
		pAngleIndication.AngleType = AngleType
		pAngleIndication.PointChoice = pSignificantPoint

		Return pAngleIndication
	End Function

	Public Function CreateDistanceIndication(ByVal Distance As Double, ByVal Uom As UomDistance, ByVal pSignificantPoint As SignificantPoint) As DistanceIndication
		Dim pDistanceIndication As DistanceIndication
		Dim pDistance As ValDistance

		pDistanceIndication = DBModule.pObjectDir.CreateFeature(Of DistanceIndication)()

		pDistance = New ValDistance
		pDistance.Uom = Uom
		pDistance.Value = Distance

		pDistanceIndication.Distance = pDistance
		pDistanceIndication.PointChoice = pSignificantPoint

		Return pDistanceIndication
	End Function

	'Private isOpen As Boolean = False

	Public Function InitModule() As String
		Dim dbPro As Aran.Aim.Data.DbProvider = gAranEnv.DbProvider

		'If (Not isOpen) Then
		pObjectDir = PandaSQPIFactory.Create()
		Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir
        pObjectDir.Open(dbPro)

        Dim tdrObj As Object = gAranEnv.CommonData.GetObject("terrainDataReader")
        Dim terrainDataReader As TerrainDataReaderEventHandler = CTypeDynamic(Of TerrainDataReaderEventHandler)(tdrObj)
        If Not terrainDataReader Is Nothing Then
            AddHandler pObjectDir.TerrainDataReader, terrainDataReader
        End If

		'isOpen = True
		'UserName = dbPro.CurrentUser.Name
		'End If

		Return dbPro.CurrentUser.Name
	End Function

	'Public Sub CloseDB()
	'	If (isOpen) Then
	'		'pObjectDir.Close()
	'		isOpen = False
	'	End If
	'End Sub
End Module