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
Imports Aran.AranEnvironment
Imports Aran
Imports ESRI

Module DBModule
	Public pObjectDir As IPandaSpecializedQPI

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

	Public Function FillADHPList(ByRef ADHPList() As ADHPType, organ As Guid, Optional CheckILS As Boolean = False) As Integer
		Dim I As Integer
		Dim N As Integer

		Dim pName As Descriptor
		Dim pADHPNameList As List(Of Descriptor)

		ReDim ADHPList(-1)

		pADHPNameList = pObjectDir.GetAirportHeliportList(organ, CheckILS)	', AirportHeliportFields_Designator + AirportHeliportFields_Id + AirportHeliportFields_ElevatedPoint

		If pADHPNameList Is Nothing Then Return -1

		N = pADHPNameList.Count - 1

		If N < 0 Then Return -1

		ReDim ADHPList(N)

		For I = 0 To pADHPNameList.Count - 1
			pName = pADHPNameList(I)
			ADHPList(I).Name = pName.Name
			ADHPList(I).Identifier = pName.Identifier

			'If CheckILS Then
			'	pAIXMILSList = pObjectDir.GetILSNavaidEquipmentList(pName.Identifer)
			'	T = 0
			'	If pAIXMILSList.Count > 0 Then
			'		For J = 0 To pAIXMILSList.Count - 1
			'			pAIXMNAVEq = pAIXMILSList(J)

			'			If (TypeOf pAIXMNAVEq Is Localizer) Then
			'				T = T Or 1
			'			ElseIf (TypeOf pAIXMNAVEq Is Glidepath) Then
			'				T = T Or 2
			'			End If
			'			If T = 3 Then Exit For
			'		Next J
			'	End If

			'	ADHPList(I).Index = T
			'Else
			'	ADHPList(I).Index = I
			'End If
			ADHPList(I).Index = I
		Next I

		Return pADHPNameList.Count - 1
	End Function

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

		CurrADHP.Name = pADHP.Designator
		CurrADHP.pPtGeo = pPtGeo
		CurrADHP.pPtPrj = pPtPrj
		CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier

		If pADHP.MagneticVariation Is Nothing Then
			CurrADHP.MagVar = 0.0
		Else
			CurrADHP.MagVar = pADHP.MagneticVariation.Value
		End If

		'CurrADHP.ISAtC = fAirportISAtC.Value
		CurrADHP.Elev = pPtGeo.Z
		CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0)
		'CurrADHP.WindSpeed = 3.6 * depWS.Value

		Return 1
	End Function

	Function GetNearestPoint(CLPointArray() As CLPoint, ByVal pPtGeo As ESRI.ArcGIS.Geometry.IPoint, Optional ByVal MaxDist As Double = 5.0) As RunwayCentrelinePoint
		Dim I As Long
		Dim fTmp As Double
		Dim fDist As Double
		Dim ptA As ESRI.ArcGIS.Geometry.IPoint

		GetNearestPoint = Nothing
		fDist = MaxDist
		ptA = ToPrj(pPtGeo)

		For I = 0 To UBound(CLPointArray)
			If Not (CLPointArray(I).pPtPrj Is Nothing) Then
				If Not CLPointArray(I).pPtPrj.IsEmpty Then
					fTmp = ReturnDistanceInMeters(ptA, CLPointArray(I).pPtPrj)
					If fTmp < fDist Then
						GetNearestPoint = CLPointArray(I).pCLPoint
						fDist = fTmp
					End If
				End If
			End If
		Next
	End Function

	'Public Function FillRWYList(ByRef RWYList() As RWYType, ByVal Owner As ADHPType, Optional ByVal Precision As Boolean = False) As Integer
	'	Dim iRwyNum As Integer
	'	Dim I As Integer
	'	Dim J As Integer
	'	Dim K As Integer
	'	Dim L As Integer
	'	Dim M As Integer

	'	Dim ResX As Double
	'	Dim ResY As Double
	'	Dim fTmp As Double
	'	Dim TrueBearing As Double

	'	Dim pName As Descriptor
	'	Dim pElevatedPoint As ElevatedPoint
	'	Dim pAIXMRWYList As List(Of Descriptor)
	'	Dim pRwyDRList As List(Of RunwayDirection)
	'	Dim pCenterLinePointList As List(Of RunwayCentrelinePoint)

	'	Dim pRunway As Runway
	'	Dim pRwyDirection As RunwayDirection
	'	Dim pRwyDirectinPair As RunwayDirection
	'	Dim pRunwayCenterLinePoint As RunwayCentrelinePoint
	'	Dim ptDTresh As ESRI.ArcGIS.Geometry.IPoint

	'	Dim CLPointArray() As CLPoint
	'	Dim pDirDeclaredDist As RunwayDeclaredDistance
	'	Dim pDeclaredDistList As List(Of RunwayDeclaredDistance)
	'	Dim fLDA As Double
	'	Dim fTORA As Double
	'	Dim fTODA As Double
	'	Dim dDT As Double
	'	Dim bRwyNum As Boolean

	'	pAIXMRWYList = pObjectDir.GetRunwayList(Owner.pAirportHeliport.Identifier)	', RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

	'	If pAIXMRWYList.Count = 0 Then
	'		ReDim RWYList(-1)
	'		Return -1
	'	End If

	'	ReDim RWYList(2 * pAIXMRWYList.Count - 1)
	'	iRwyNum = -1

	'	For I = 0 To pAIXMRWYList.Count - 1
	'		pName = pAIXMRWYList.Item(I)
	'		pRunway = pObjectDir.GetFeature(FeatureType.Runway, pName.Identifier)
	'		pRwyDRList = pObjectDir.GetRunwayDirectionList(pRunway.Identifier)
	'		bRwyNum = True

	'		If (pRwyDRList.Count = 2) Or (Precision And (pRwyDRList.Count = 1)) Then
	'			M = pRwyDRList.Count

	'			For J = 0 To M - 1
	'				If Precision Then bRwyNum = True
	'				iRwyNum = iRwyNum + 1

	'				'RWYList(iRwyNum).Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0)
	'				'If RWYList(iRwyNum).Length < 0 Then
	'				'	iRwyNum = iRwyNum - 1
	'				'	If Precision Then
	'				'		Continue For
	'				'	Else
	'				'		Exit For
	'				'	End If
	'				'End If

	'				pRwyDirection = pRwyDRList.Item(J)
	'				pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier)

	'				If pCenterLinePointList.Count = 0 Then
	'					iRwyNum = iRwyNum - 1
	'					If Precision Then
	'						Continue For
	'					Else
	'						Exit For
	'					End If
	'				End If

	'				RWYList(iRwyNum).Initialize()
	'				RWYList(iRwyNum).pRunwayDir = pRwyDirection
	'				RWYList(iRwyNum).pPtGeo(eRWY.PtTHR) = Nothing

	'				ptDTresh = Nothing
	'				ReDim CLPointArray(pCenterLinePointList.Count - 1)
	'				fLDA = 0.0
	'				fTORA = 0.0
	'				fTODA = 0.0

	'				For K = 0 To pCenterLinePointList.Count - 1
	'					pRunwayCenterLinePoint = pCenterLinePointList.Item(K)

	'					If (Not pRunwayCenterLinePoint.Role Is Nothing) And (Not pRunwayCenterLinePoint.Location Is Nothing) Then

	'						pElevatedPoint = pRunwayCenterLinePoint.Location
	'						CLPointArray(K).pCLPoint = pRunwayCenterLinePoint
	'						CLPointArray(K).pPtGeo = ARANPointToESRIPoint(pElevatedPoint.Geo)
	'						CLPointArray(K).pPtPrj = ToPrj(CLPointArray(K).pPtGeo)

	'						Select Case pRunwayCenterLinePoint.Role.Value
	'							Case Aran.Aim.Enums.CodeRunwayPointRole.START
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtStart) = CLPointArray(K).pPtGeo
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtStart).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)

	'								pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance
	'								For L = 0 To pDeclaredDistList.Count - 1
	'									pDirDeclaredDist = pDeclaredDistList(L)
	'									If pDirDeclaredDist.DeclaredValue.Count > 0 Then
	'										If pDirDeclaredDist.Type = Aran.Aim.Enums.CodeDeclaredDistance.LDA Then
	'											fLDA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue(0).Distance, 0)
	'										ElseIf pDirDeclaredDist.Type = CodeDeclaredDistance.TORA Then
	'											fTORA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue(0).Distance, 0)
	'										ElseIf pDirDeclaredDist.Type = CodeDeclaredDistance.TODA Then
	'											fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue(0).Distance, 0)
	'										End If
	'									End If
	'								Next

	'							Case Aran.Aim.Enums.CodeRunwayPointRole.THR
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtTHR) = CLPointArray(K).pPtGeo
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtTHR).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'							Case Aran.Aim.Enums.CodeRunwayPointRole.END
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtEnd) = CLPointArray(K).pPtGeo
	'								RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'							Case Aran.Aim.Enums.CodeRunwayPointRole.DISTHR
	'								ptDTresh = CLPointArray(K).pPtGeo
	'								ptDTresh.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'						End Select
	'					End If
	'				Next K

	'				If (RWYList(iRwyNum).pPtGeo(eRWY.PtTHR) Is Nothing) And Not (ptDTresh Is Nothing) Then
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtTHR) = ptDTresh
	'				End If

	'				For K = eRWY.PtStart To eRWY.PtEnd
	'					If RWYList(iRwyNum).pPtGeo(K) Is Nothing Then
	'						iRwyNum = iRwyNum - 1
	'						bRwyNum = False
	'						Exit For
	'					End If
	'				Next K

	'				If Not bRwyNum Then
	'					If Precision Then
	'						Continue For
	'					Else
	'						Exit For
	'					End If
	'				End If

	'				If Not pRwyDirection.TrueBearing Is Nothing Then
	'					RWYList(iRwyNum).TrueBearing = pRwyDirection.TrueBearing.Value
	'				ElseIf Not pRwyDirection.MagneticBearing Is Nothing Then
	'					RWYList(iRwyNum).TrueBearing = pRwyDirection.MagneticBearing.Value + Owner.MagVar
	'				Else
	'					ReturnGeodesicAzimuth(RWYList(iRwyNum).pPtGeo(eRWY.PtStart).X, RWYList(iRwyNum).pPtGeo(eRWY.PtStart).Y, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).X, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Y, TrueBearing, fTmp)
	'					RWYList(iRwyNum).TrueBearing = TrueBearing
	'				End If
	'				'//*************************************************************************
	'				'pDeclaredDistList = pObjectDir.GetDeclaredDistance(pRwyDirection.Id)
	'				'For K = 0 To pDeclaredDistList.Count - 1
	'				'	pDirDeclaredDist = pDeclaredDistList.GetItem(K)
	'				'	If pDirDeclaredDist.CodeType = Aran.Aim.Enums.CodeDeclaredDistance.LDA Then
	'				'		fLDA = pConverter.ConverterToSI(pDirDeclaredDist.Distance, 0)
	'				'	ElseIf pDirDeclaredDist.CodeType = CodeDeclaredDistance.TORA Then
	'				'		fTORA = pConverter.ConverterToSI(pDirDeclaredDist.Distance, 0)
	'				'	ElseIf pDirDeclaredDist.CodeType = CodeDeclaredDistance.TODA Then
	'				'		fTODA = pConverter.ConverterToSI(pDirDeclaredDist.Distance, 0)
	'				'	End If
	'				'Next
	'				'==============================================================================================================
	'				dDT = fTORA - fLDA
	'				If dDT > 0 Then
	'					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(eRWY.PtStart))
	'					If Not (pRunwayCenterLinePoint Is Nothing) And Not (pRunwayCenterLinePoint.Location Is Nothing) Then
	'						pElevatedPoint = pRunwayCenterLinePoint.Location
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtStart) = ARANPointToESRIPoint(pElevatedPoint.Geo)
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtStart).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'					Else
	'						PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(eRWY.PtTHR).X, RWYList(iRwyNum).pPtGeo(eRWY.PtTHR).Y, dDT, RWYList(iRwyNum).TrueBearing + 180.0, ResX, ResY)
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtStart) = New Point
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtStart).PutCoords(ResX, ResY)

	'						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(eRWY.PtStart), 10000.0)
	'						If Not (pRunwayCenterLinePoint Is Nothing) And Not (pRunwayCenterLinePoint.Location Is Nothing) Then
	'							pElevatedPoint = pRunwayCenterLinePoint.Location
	'							RWYList(iRwyNum).pPtGeo(eRWY.PtStart).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'						Else
	'							RWYList(iRwyNum).pPtGeo(eRWY.PtStart).Z = Owner.Elev
	'						End If
	'					End If
	'				Else
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtStart) = RWYList(iRwyNum).pPtGeo(eRWY.PtTHR)
	'				End If

	'				'==============================================================================================================
	'				pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd))
	'				If Not (pRunwayCenterLinePoint Is Nothing) And Not (pRunwayCenterLinePoint.Location Is Nothing) Then
	'					pElevatedPoint = pRunwayCenterLinePoint.Location
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtEnd) = ARANPointToESRIPoint(pElevatedPoint.Geo)
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'				Else
	'					PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(eRWY.PtTHR).X, RWYList(iRwyNum).pPtGeo(eRWY.PtTHR).Y, fLDA, RWYList(iRwyNum).TrueBearing, ResX, ResY)
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtEnd) = New Point
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).PutCoords(ResX, ResY)

	'					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd), 10000.0#)
	'					If Not (pRunwayCenterLinePoint Is Nothing) And Not (pRunwayCenterLinePoint.Location Is Nothing) Then
	'						pElevatedPoint = pRunwayCenterLinePoint.Location
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev)
	'					Else
	'						RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Z = Owner.Elev
	'					End If
	'				End If

	'				RWYList(iRwyNum).ClearWay = fTODA - fTORA
	'				If RWYList(iRwyNum).ClearWay < 0.0 Then RWYList(iRwyNum).ClearWay = 0.0

	'				If RWYList(iRwyNum).ClearWay > 0.0 Then
	'					PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).X, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Y, RWYList(iRwyNum).ClearWay, RWYList(iRwyNum).TrueBearing, ResX, ResY)
	'					RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).PutCoords(ResX, ResY)
	'				End If

	'				'==============================================================================================================
	'				'RWYList(iRwyNum).ClearWay = 0.0
	'				'pRunwayProtectAreaList = pObjectDir.GetRunwayProtectAreaList(pRwyDirection.Identifier)
	'				'For K = 0 To pRunwayProtectAreaList.Count - 1
	'				'	pRunwayProtectArea = pRunwayProtectAreaList.Item(K)
	'				'	pAirportHeliportProtectionArea = pRunwayProtectArea
	'				'	If (pRunwayProtectArea.Type.Value = Aran.Aim.Enums.CodeRunwayProtectionArea.CWY) And (Not pAirportHeliportProtectionArea.Length Is Nothing) Then
	'				'		RWYList(iRwyNum).ClearWay = pConverter.Convert(pAirportHeliportProtectionArea.Length, 0.0)
	'				'		Exit For
	'				'	End If
	'				'Next

	'				'If RWYList(iRwyNum).ClearWay > 0.0 Then
	'				'	PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).X, RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).Y, RWYList(iRwyNum).ClearWay, RWYList(iRwyNum).TrueBearing, ResX, ResY)
	'				'	RWYList(iRwyNum).pPtGeo(eRWY.PtEnd).PutCoords(ResX, ResY)
	'				'End If
	'				'//*************************************************************************

	'				For K = eRWY.PtStart To eRWY.PtEnd
	'					RWYList(iRwyNum).pPtPrj(K) = ToPrj(RWYList(iRwyNum).pPtGeo(K))

	'					If RWYList(iRwyNum).pPtPrj(K).IsEmpty() Then
	'						iRwyNum = iRwyNum - 1
	'						bRwyNum = False
	'						Exit For
	'					End If

	'					RWYList(iRwyNum).pPtGeo(K).M = RWYList(iRwyNum).TrueBearing
	'					RWYList(iRwyNum).pPtPrj(K).M = Azt2Dir(RWYList(iRwyNum).pPtGeo(K), RWYList(iRwyNum).TrueBearing)
	'				Next K

	'				If Not bRwyNum Then
	'					If Precision Then
	'						Continue For
	'					Else
	'						Exit For
	'					End If
	'				End If

	'				RWYList(iRwyNum).Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0)
	'				If RWYList(iRwyNum).Length < 0 Then
	'					RWYList(iRwyNum).Length = ReturnDistanceInMeters(RWYList(iRwyNum).pPtPrj(eRWY.PtEnd), RWYList(iRwyNum).pPtPrj(eRWY.PtStart))
	'				End If

	'				RWYList(iRwyNum).Identifier = pRwyDirection.Identifier
	'				RWYList(iRwyNum).Name = pRwyDirection.Designator
	'				RWYList(iRwyNum).ADHP_Ident = Owner.Identifier
	'				'RWYList(iRwyNum).ILSID = pRwyDirection .ILS_ID

	'				pRwyDirectinPair = pRwyDRList.Item((J + 1) Mod 2)
	'				RWYList(iRwyNum).Pair_Ident = pRwyDirectinPair.Identifier
	'				RWYList(iRwyNum).PairName = pRwyDirectinPair.Designator

	'			Next J
	'		End If
	'	Next I

	'	If iRwyNum >= 0 Then
	'		ReDim Preserve RWYList(iRwyNum)
	'	Else
	'		ReDim RWYList(-1)
	'	End If

	'	Return iRwyNum
	'End Function

	Function AddILSToNavList(ByVal ILS As NavaidType, ByRef NavaidList() As NavaidType) As Integer
		Dim I As Integer
		Dim N As Integer

		N = UBound(NavaidList)
		For I = 0 To N
			If (NavaidList(I).TypeCode = eNavaidType.LLZ) And (NavaidList(I).CallSign = ILS.CallSign) Then Return I
		Next I

		N = N + 1
		ReDim Preserve NavaidList(N)

		NavaidList(N) = ILS
		NavaidList(N).Name = ILS.CallSign
		NavaidList(N).TypeCode = eNavaidType.LLZ
		NavaidList(N).Range = 40000.0
		NavaidList(N).PairNavaidIndex = -1
		NavaidList(N).Tag = 0

		Return N
	End Function

	'Function GetILSByName(ByVal ProcName As String, ByVal ADHP As ADHPType, ByRef ILS As NavaidType) As Integer
	'	Dim I As Integer
	'	Dim N As Integer
	'	Dim L As Integer
	'	Dim pos As Integer

	'	Dim RWYList() As RWYType
	'	Dim RWY As RWYType
	'	Dim bRWYFound As Boolean

	'	Dim RWYName As String
	'	Dim Chr As Char

	'	GetILSByName = 0

	'	pos = InStr(1, ProcName, "RWY", 1)
	'	If pos <= 0 Then Exit Function

	'	pos = pos + 3
	'	RWYName = ""
	'	L = ProcName.Length()

	'	Do While pos <= L
	'		Chr = Mid(ProcName, pos, 1)
	'		If Not IsNumeric(Chr) Then
	'			If Chr = "R" Or Chr = "L" Then
	'				RWYName = RWYName + Chr
	'			End If
	'			Exit Do
	'		End If
	'		RWYName = RWYName + Chr
	'		pos = pos + 1
	'	Loop

	'	'    ADHP.ID = OwnerID
	'	'    If FillADHPFields(ADHP) < 0 Then Exit Function

	'	N = FillRWYList(RWYList, ADHP)

	'	bRWYFound = False
	'	For I = 0 To N
	'		bRWYFound = RWYList(I).Name = RWYName
	'		If bRWYFound Then
	'			RWY = RWYList(I)
	'			Exit For
	'		End If
	'	Next I

	'	If Not bRWYFound Then Exit Function

	'	GetILSByName = GetILS(RWY, ILS, ADHP)
	'	'    If ILS.CallSign <> ILSCallSign Then GetILSByName = -1
	'End Function

	'	Public Function GetILSByNavEq(pNavaid As Navaid, ByRef ILS As NavaidType) As Integer
	'		Dim I As Integer
	'		Dim MkrCnt As Integer
	'		Dim pNAVEqList As List(Of NavaidComponent)

	'		Dim pAIXMNAVEq As NavaidEquipment
	'		Dim pAIXMLocalizer As Localizer
	'		Dim pAIXMGlidepath As Glidepath
	'		Dim pElevPoint As ElevatedPoint
	'		Dim Identifier As Guid
	'		Dim CallSign As String

	'		ILS.Index = 0

	'		pNAVEqList = pNavaid.NavaidEquipment
	'		If pNAVEqList.Count = 0 Then Return 0

	'		ILS.Category = 4
	'		ILS.pFeature = pNavaid
	'		ILS.TypeCode = eNavaidType.LLZ

	'		If Not pNavaid.SignalPerformance Is Nothing Then
	'			ILS.Category = pNavaid.SignalPerformance.Value + 1
	'		End If

	'		If ILS.Category > 3 Then ILS.Category = 1

	'		MkrCnt = 0
	'		For I = 0 To pNAVEqList.Count - 1
	'			pAIXMNAVEq = pNAVEqList(I).TheNavaidEquipment.GetFeature2()

	'			If (TypeOf pAIXMNAVEq Is Localizer) Then
	'				pAIXMLocalizer = DirectCast(pAIXMNAVEq, Localizer)
	'			ElseIf (TypeOf pAIXMNAVEq Is Glidepath) Then
	'				pAIXMGlidepath = DirectCast(pAIXMNAVEq, Glidepath)
	'			ElseIf (TypeOf pAIXMNAVEq Is MarkerBeacon) Then
	'				GetMRK(ILS.MkrList(MkrCnt), pAIXMNAVEq)
	'				MkrCnt = MkrCnt + 1
	'			End If
	'		Next I

	'		If MkrCnt > 0 Then
	'			ReDim Preserve ILS.MkrList(MkrCnt - 1)
	'		Else
	'			ReDim ILS.MkrList(-1)
	'		End If

	'		If Not pAIXMLocalizer Is Nothing Then
	'			pAIXMNAVEq = pAIXMLocalizer
	'			pElevPoint = pAIXMNAVEq.Location

	'			If Not pAIXMNAVEq.MagneticVariation Is Nothing Then
	'				ILS.MagVar = pAIXMNAVEq.MagneticVariation.Value
	'			ElseIf (Not pAIXMLocalizer.TrueBearing Is Nothing) And (Not pAIXMLocalizer.MagneticBearing Is Nothing) Then
	'				ILS.MagVar = pAIXMLocalizer.TrueBearing.Value - pAIXMLocalizer.MagneticBearing.Value
	'			Else
	'				GoTo NoLocalizer
	'			End If

	'			If Not pAIXMLocalizer.TrueBearing Is Nothing Then
	'				ILS.Course = pAIXMLocalizer.TrueBearing.Value
	'			ElseIf Not pAIXMLocalizer.MagneticBearing Is Nothing Then
	'				ILS.Course = pAIXMLocalizer.MagneticBearing.Value + ILS.MagVar
	'			Else
	'				GoTo NoLocalizer
	'			End If

	'			ILS.pPtGeo = ARANPointToESRIPoint(pElevPoint.Geo)
	'			ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, -9999)
	'			ILS.pPtGeo.M = ILS.Course

	'			ILS.pPtPrj = ToPrj(ILS.pPtGeo)
	'			If ILS.pPtPrj.IsEmpty() Then GoTo NoLocalizer
	'			ILS.pPtPrj.M = Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M)

	'			If Not pAIXMLocalizer.WidthCourse Is Nothing Then
	'				ILS.AngleWidth = pAIXMLocalizer.WidthCourse.Value

	'				ILS.Index = 1
	'				ILS.Identifier = pAIXMNAVEq.Identifier
	'				ILS.CallSign = pAIXMNAVEq.Designator
	'			End If
	'		End If
	'NoLocalizer:

	'		If Not pAIXMGlidepath Is Nothing Then
	'			pAIXMNAVEq = pAIXMGlidepath

	'			If Not pAIXMGlidepath.Slope Is Nothing Then
	'				ILS.GPAngle = pAIXMGlidepath.Slope.Value

	'				If Not pAIXMGlidepath.Rdh Is Nothing Then

	'					ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, -9999)

	'					If ILS.GP_RDH >= 0 Then
	'						Identifier = pAIXMNAVEq.Identifier
	'						CallSign = pAIXMNAVEq.Designator
	'						ILS.Index = ILS.Index Or 2
	'					End If
	'				End If
	'			End If
	'		End If

	'		For I = 0 To MkrCnt - 1
	'			ILS.MkrList(I).DistFromCL = Point2LineDistancePrj(ILS.pPtPrj, ILS.MkrList(I).pPtPrj, ILS.Course)
	'			ILS.MkrList(I).ILS_ID = ILS.Identifier
	'		Next

	'		If ILS.Index = 2 Then
	'			ILS.Identifier = Identifier
	'			ILS.CallSign = CallSign
	'		End If

	'		Return ILS.Index
	'	End Function

	'	Public Function GetILS(ByVal RWY As RWYType, ByRef ILS As NavaidType, ByVal Owner As ADHPType) As Integer
	'		Dim I As Integer
	'		Dim MkrCnt As Integer
	'		Dim pNavaid As Navaid
	'		Dim pAIXMNAVEqList As List(Of NavaidComponent)

	'		Dim pAIXMNAVEq As NavaidEquipment
	'		Dim pAIXMLocalizer As Localizer
	'		Dim pAIXMGlidepath As Glidepath
	'		Dim pElevPoint As ElevatedPoint
	'		Dim dX As Double
	'		Dim dY As Double
	'		Dim Identifier As Guid
	'		Dim CallSign As String

	'		ILS.Index = 0

	'		pNavaid = pObjectDir.GetILSNavaid(RWY.pRunwayDir.Identifier)
	'		If pNavaid Is Nothing Then Return 0

	'		pAIXMNAVEqList = pNavaid.NavaidEquipment
	'		If pAIXMNAVEqList.Count = 0 Then Return 0

	'		ILS.Category = 4
	'		ILS.pFeature = pNavaid
	'		ILS.TypeCode = eNavaidType.LLZ

	'		If Not pNavaid.SignalPerformance Is Nothing Then
	'			ILS.Category = pNavaid.SignalPerformance.Value + 1
	'		End If

	'		If ILS.Category > 3 Then ILS.Category = 1

	'		ILS.RWY_Ident = RWY.Identifier

	'		MkrCnt = 0
	'		ReDim ILS.MkrList(pAIXMNAVEqList.Count - 1)

	'		For I = 0 To pAIXMNAVEqList.Count - 1
	'			pAIXMNAVEq = pAIXMNAVEqList(I).TheNavaidEquipment.GetFeature2()

	'			If (TypeOf pAIXMNAVEq Is Localizer) Then
	'				pAIXMLocalizer = DirectCast(pAIXMNAVEq, Localizer)
	'			ElseIf (TypeOf pAIXMNAVEq Is Glidepath) Then
	'				pAIXMGlidepath = DirectCast(pAIXMNAVEq, Glidepath)
	'			ElseIf (TypeOf pAIXMNAVEq Is MarkerBeacon) Then
	'				GetMRK(ILS.MkrList(MkrCnt), pAIXMNAVEq)
	'				MkrCnt = MkrCnt + 1
	'			End If
	'		Next I

	'		If MkrCnt > 0 Then
	'			ReDim Preserve ILS.MkrList(MkrCnt - 1)
	'		Else
	'			ReDim ILS.MkrList(-1)
	'		End If

	'		If Not pAIXMLocalizer Is Nothing Then
	'			pAIXMNAVEq = pAIXMLocalizer
	'			pElevPoint = pAIXMNAVEq.Location

	'			If Not pAIXMNAVEq.MagneticVariation Is Nothing Then
	'				ILS.MagVar = pAIXMNAVEq.MagneticVariation.Value
	'			ElseIf (Not pAIXMLocalizer.TrueBearing Is Nothing) And (Not pAIXMLocalizer.MagneticBearing Is Nothing) Then
	'				ILS.MagVar = pAIXMLocalizer.TrueBearing.Value - pAIXMLocalizer.MagneticBearing.Value
	'			Else
	'				ILS.MagVar = Owner.MagVar
	'			End If

	'			If Not pAIXMLocalizer.TrueBearing Is Nothing Then
	'				ILS.Course = pAIXMLocalizer.TrueBearing.Value
	'			ElseIf Not pAIXMLocalizer.MagneticBearing Is Nothing Then
	'				ILS.Course = pAIXMLocalizer.MagneticBearing.Value + ILS.MagVar
	'			Else
	'				GoTo NoLocalizer
	'			End If

	'			ILS.pPtGeo = ARANPointToESRIPoint(pElevPoint.Geo)
	'			ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo(eRWY.PtTHR).Z)
	'			ILS.pPtGeo.M = ILS.Course

	'			ILS.pPtPrj = ToPrj(ILS.pPtGeo)
	'			If ILS.pPtPrj.IsEmpty() Then GoTo NoLocalizer
	'			ILS.pPtPrj.M = Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M)

	'			dX = RWY.pPtPrj(eRWY.PtTHR).X - ILS.pPtPrj.X
	'			dY = RWY.pPtPrj(eRWY.PtTHR).Y - ILS.pPtPrj.Y
	'			ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY)

	'			If Not pAIXMLocalizer.WidthCourse Is Nothing Then
	'				ILS.AngleWidth = pAIXMLocalizer.WidthCourse.Value
	'				ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(DegToRad(ILS.AngleWidth))

	'				ILS.Index = 1
	'				ILS.Identifier = pAIXMNAVEq.Identifier
	'				ILS.CallSign = pAIXMNAVEq.Designator
	'			End If
	'		End If
	'NoLocalizer:

	'		If ILS.Index = 1 Then
	'			For I = 0 To MkrCnt - 1
	'				ILS.MkrList(I).DistFromCL = Point2LineDistancePrj(ILS.pPtPrj, ILS.MkrList(I).pPtPrj, ILS.Course)
	'				ILS.MkrList(I).ILS_ID = ILS.Identifier
	'			Next
	'		End If

	'		If Not pAIXMGlidepath Is Nothing Then
	'			pAIXMNAVEq = pAIXMGlidepath

	'			If Not pAIXMGlidepath.Slope Is Nothing Then
	'				ILS.GPAngle = pAIXMGlidepath.Slope.Value

	'				If Not pAIXMGlidepath.Rdh Is Nothing Then

	'					ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, -9999)

	'					If ILS.GP_RDH >= 0 Then
	'						ILS.Index = ILS.Index Or 2
	'						If ILS.Index = 2 Then
	'							ILS.Identifier = Identifier
	'							ILS.CallSign = CallSign
	'						End If
	'					End If
	'				End If
	'			End If
	'		End If

	'		Return ILS.Index
	'	End Function

	'Public Function GetMRK(ByRef Mkr As MKRType, ByRef AixmNavaidEquipment As NavaidEquipment) As Boolean
	'	Dim AIXMMarker As MarkerBeacon
	'	Dim pElevPoint As ElevatedPoint
	'	Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
	'	Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

	'	If Not (TypeOf AixmNavaidEquipment Is MarkerBeacon) Then Return False

	'	AIXMMarker = AixmNavaidEquipment

	'	pElevPoint = AixmNavaidEquipment.Location
	'	If pElevPoint Is Nothing Then Return False

	'	pPtGeo = AIXMPointToESRIPoint(pElevPoint)
	'	pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev)
	'	pPtPrj = ToPrj(pPtGeo)

	'	If pPtPrj.IsEmpty() Then Return False

	'	Mkr.pPtGeo = pPtGeo
	'	Mkr.pPtPrj = pPtPrj

	'	If Not AIXMMarker.AxisBearing Is Nothing Then
	'		Mkr.AxisBearing = AIXMMarker.AxisBearing.Value
	'	End If

	'	Mkr.Name = AixmNavaidEquipment.Name
	'	Mkr.Identifier = AixmNavaidEquipment.Identifier
	'	Mkr.CallSign = AixmNavaidEquipment.Designator

	'	Return True
	'End Function

	Public Sub FillNavaidList(ByRef NavaidList() As NavaidType, ByRef DMEList() As NavaidType, ByRef TACANList() As NavaidType, ByRef CurrADHP As ADHPType, ByVal Radius As Double)
		Dim I As Integer
		Dim J As Integer
		Dim NavTypeCode As Integer
		Dim iNavaidNum As Integer
		Dim iDMENum As Integer
		Dim iTACANNum As Integer
		Dim index As Integer

		Dim fqvRadius As Double
		Dim fDist As Double

		Dim pNavaidList As List(Of Navaid)
		Dim pNavaid As Navaid
		Dim AixmNavaidEquipment As NavaidEquipment
		Dim pAIXMDME As DME

		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pARANPolygon As Aran.Geometries.MultiPolygon

		Dim pElevPoint As ElevatedPoint
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		pPolygon = CreatePrjCircle(CurrADHP.pPtPrj, Radius)

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))

		pNavaidList = pObjectDir.GetNavaidList(pARANPolygon)

		'pNavaidList = pObjectDir.GetNavaidList()

		If (pNavaidList.Count = 0) Then	'And (ILSDataList.Count = 0)
			ReDim NavaidList(-1)
			ReDim DMEList(-1)
			ReDim TACANList(-1)
			Return
		End If

		ReDim NavaidList(pNavaidList.Count - 1)	'+ ILSDataList.Count
		ReDim DMEList(pNavaidList.Count - 1)
		ReDim TACANList(pNavaidList.Count - 1)

		' .GetNavaidEquipmentList (pARANPolygon)
		' Set ILSDataList = pObjectDir.GetILSNavaidEquipmentList(CurrADHP.ID)

		fqvRadius = Radius * Radius

		iNavaidNum = -1
		iDMENum = -1
		iTACANNum = -1
		index = -1

		For J = 0 To pNavaidList.Count - 1

			pNavaid = pNavaidList.Item(J)

			For I = 0 To pNavaid.NavaidEquipment.Count - 1
				If pNavaid.NavaidEquipment(I).TheNavaidEquipment Is Nothing Then
					Continue For
				End If

				AixmNavaidEquipment = pNavaid.NavaidEquipment(I).TheNavaidEquipment.GetFeature2()

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
				pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev)
				pPtPrj = ToPrj(pPtGeo)

				If pPtPrj.IsEmpty() Then Continue For

				'dX = pPtPrj.X - CurrADHP.pPtPrj.X
				'dY = pPtPrj.Y - CurrADHP.pPtPrj.Y
				'If dX * dX + dY * dY > fqvRadius Then Continue For

				If NavTypeCode = eNavaidType.DME Then
					iDMENum = iDMENum + 1

					DMEList(iDMENum).pPtGeo = pPtGeo
					DMEList(iDMENum).pPtPrj = pPtPrj

					If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
						DMEList(iDMENum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
					Else
						DMEList(iDMENum).MagVar = CurrADHP.MagVar
					End If

					pAIXMDME = DirectCast(AixmNavaidEquipment, DME)

					If (Not pAIXMDME.Displace Is Nothing) Then
						DMEList(iDMENum).Disp = ConverterToSI.Convert(pAIXMDME.Displace, 0)
					Else
						DMEList(iDMENum).Disp = 0
					End If

					DMEList(iDMENum).Range = 350000.0 'DME.Range
					DMEList(iDMENum).PairNavaidIndex = -1

					DMEList(iDMENum).Name = AixmNavaidEquipment.Name
					DMEList(iDMENum).Identifier = AixmNavaidEquipment.Identifier
					DMEList(iDMENum).NAV_Ident = pNavaid.Identifier
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
						TACANList(iTACANNum).MagVar = CurrADHP.MagVar		'pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate) '
					End If

					TACANList(iTACANNum).Range = 350000.0 'TACAN.Range
					TACANList(iTACANNum).PairNavaidIndex = -1

					TACANList(iTACANNum).Name = AixmNavaidEquipment.Name
					TACANList(iTACANNum).Identifier = AixmNavaidEquipment.Identifier
					TACANList(iTACANNum).NAV_Ident = pNavaid.Identifier
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
						NavaidList(iNavaidNum).MagVar = CurrADHP.MagVar
					End If

					If NavTypeCode = eNavaidType.NDB Then
						NavaidList(iNavaidNum).Range = 350000.0	'NDB.Range
					Else
						NavaidList(iNavaidNum).Range = 350000.0	'VOR.Range
					End If
					NavaidList(iNavaidNum).PairNavaidIndex = -1

					NavaidList(iNavaidNum).Name = AixmNavaidEquipment.Name
					NavaidList(iNavaidNum).Identifier = AixmNavaidEquipment.Identifier
					NavaidList(iNavaidNum).NAV_Ident = pNavaid.Identifier
					NavaidList(iNavaidNum).CallSign = AixmNavaidEquipment.Designator

					NavaidList(iNavaidNum).TypeCode = NavTypeCode

					index += 1
					NavaidList(iNavaidNum).Index = index

					'NavaidList(iNavaidNum).pFeature = pNavaid
				End If
			Next I
		Next J

		For J = 0 To iNavaidNum
			For I = 0 To iDMENum
				fDist = ReturnDistanceInMeters(NavaidList(J).pPtPrj, DMEList(I).pPtPrj)
				If fDist <= 2.0 Then
					NavaidList(J).PairNavaidIndex = I
					DMEList(I).PairNavaidIndex = J
					Exit For
				End If
			Next I
		Next J

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
		'
		''    GPAngle = 0
		'
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
	End Sub

	Public Function FillWPT_FIXList(ByRef WPTList() As WPT_FIXType, ByVal CurrADHP As ADHPType, ByVal Radius As Double) As Integer
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

		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pARANPolygon As Aran.Geometries.MultiPolygon
		Dim pElevPoint As ElevatedPoint

		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		pPolygon = CreatePrjCircle(CurrADHP.pPtPrj, Radius)

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))

		AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon)


		' ''*****************
		' ''Begin => ForDebug
		' ''*****************

		'For Each dp As DesignatedPoint In AIXMWPTList
		'    If dp.Identifier = Guid.Parse("03e58320-d4e1-4ebf-88f6-d58b38c15a10") Then

		'        Dim esriPoint As ESRI.ArcGIS.Geometry.IPoint = ConvertToEsriGeom.FromPoint(dp.Location.Geo)

		'        DrawPolygon(pPolygon, 255)
		'        DrawPoint(ToPrj(esriPoint), RGB(0, 255, 0))

		'        Dim b As Boolean = pARANPolygon.IsPointInside(dp.Location.Geo)

		'        If b Then

		'            MessageBox.Show("OK")
		'        End If
		'    End If
		'Next

		' ''*****************
		' ''End
		' ''*****************

		'Set AIXMNAVList = pObjectDir.GetNavaidEquipmentList(pARANPolygon)
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

			If AIXMWPT.Location Is Nothing Then Continue For

			pPtGeo = AIXMPointToESRIPoint(AIXMWPT.Location)
			pPtGeo.Z = CurrADHP.pPtGeo.Z + 300.0
			pPtPrj = ToPrj(pPtGeo)

			If pPtPrj.IsEmpty() Then Continue For
			If AIXMWPT.Designator = Nothing Then Continue For

			iWPTNum = iWPTNum + 1

			'If Not AIXMWPT.magneticVariation Is Nothing Then
			'	WPTList(iWPTNum).MagVar = AIXMWPT.magneticVariation.Value
			'Else
			'	WPTList(iWPTNum).MagVar = CurrADHP.MagVar
			'End If
			WPTList(iWPTNum).MagVar = 0.0 'CurrADHP.MagVar

			WPTList(iWPTNum).pPtGeo = pPtGeo
			WPTList(iWPTNum).pPtPrj = pPtPrj

			WPTList(iWPTNum).Name = AIXMWPT.Designator

			WPTList(iWPTNum).Identifier = AIXMWPT.Identifier

			'WPTList(iWPTNum).pFeature = AIXMWPT
			WPTList(iWPTNum).TypeCode = eNavaidType.NONE
		Next I

		'======================================================================

		For J = 0 To pNavaidList.Count - 1
			pNavaid = pNavaidList.Item(J)

			For I = 0 To pNavaid.NavaidEquipment.Count - 1

				If pNavaid.NavaidEquipment(I).TheNavaidEquipment Is Nothing Then Continue For

				AIXMNAVEq = pNavaid.NavaidEquipment(I).TheNavaidEquipment.GetFeature2()

				If AIXMNAVEq Is Nothing Then Continue For

				pElevPoint = AIXMNAVEq.Location
				If pElevPoint Is Nothing Then Continue For

				pPtGeo = AIXMPointToESRIPoint(pElevPoint)
				pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev)

				pPtPrj = ToPrj(pPtGeo)
				If pPtPrj.IsEmpty() Then Continue For

				If (TypeOf AIXMNAVEq Is Aran.Aim.Features.VOR) Then
					NavTypeCode = eNavaidType.VOR
				ElseIf (TypeOf AIXMNAVEq Is Aran.Aim.Features.NDB) Then
					NavTypeCode = eNavaidType.NDB
				ElseIf (TypeOf AIXMNAVEq Is Aran.Aim.Features.TACAN) Then
					NavTypeCode = eNavaidType.TACAN
				Else
					Continue For
				End If

				iWPTNum = iWPTNum + 1

				WPTList(iWPTNum).pPtGeo = pPtGeo
				WPTList(iWPTNum).pPtPrj = pPtPrj

				If Not AIXMNAVEq.MagneticVariation Is Nothing Then
					WPTList(iWPTNum).MagVar = AIXMNAVEq.MagneticVariation.Value
				Else
					WPTList(iWPTNum).MagVar = 0.0 'CurrADHP.MagVar
				End If

				WPTList(iWPTNum).Name = AIXMNAVEq.Designator
				WPTList(iWPTNum).Identifier = AIXMNAVEq.Identifier
				WPTList(iWPTNum).NAV_Ident = pNavaid.Identifier

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

	Public Function FillObstacleList(ByRef ObstacleList() As ObstacleType, ByVal CurrADHP As ADHPType, ByVal Radius As Double, Optional ByVal fRefHeight As Double = 0.0) As Integer
		Dim C As Integer
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
		Dim pElevatedCurve As ElevatedCurve
		Dim pElevatedSurface As ElevatedSurface

		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pARANPolygon As Aran.Geometries.MultiPolygon
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry

		Dim pZv As ESRI.ArcGIS.Geometry.IZ
		Dim pZAware As ESRI.ArcGIS.Geometry.IZAware

		pPolygon = CreatePrjCircle(CurrADHP.pPtPrj, Radius)

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1

		ReDim ObstacleList(N)

		If N < 0 Then Return -1

		C = N
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
					Case VerticalStructurePartGeometryChoice.ElevatedCurve
						pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent
						If pElevatedCurve Is Nothing Then Continue For
						If pElevatedCurve.Elevation Is Nothing Then Continue For

						HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0)
						VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0)

						pGeomGeo = AIXMCurveToESRIPolyline(pElevatedCurve)
						Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0)
					Case VerticalStructurePartGeometryChoice.ElevatedSurface
						'Continue For
						pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent
						If pElevatedSurface Is Nothing Then Continue For
						If pElevatedSurface.Elevation Is Nothing Then Continue For

						HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0)
						VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0)

						pGeomGeo = AIXMSurfaceToESRIPolygon(pElevatedSurface)
						Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0)
					Case Else
						Continue For
				End Select

				pGeomGeo.SpatialReference = pSpRefShp
				pGeomPrj = ToPrj(pGeomGeo)
				If pGeomPrj.IsEmpty() Then Continue For

				If VertAccuracy > 0.0 Then Z += VertAccuracy

				'HorAccuracy = 0.0
				'If HorAccuracy > 0.0 Then
				'	If pGeomPrj.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint And (HorAccuracy <= 2.0) Then
				'		pGeomPrj = CreatePrjCircle(pGeomPrj, HorAccuracy, 18)
				'	Else
				'		pTopop = pGeomPrj
				'		pTopop.Simplify()
				'		pGeomPrj = pTopop.Buffer(HorAccuracy)

				'		pTopo = pGeomPrj
				'		pTopo.IsKnownSimple_2 = False
				'		pTopo.Simplify()
				'	End If

				'	pGeomGeo = ToGeo(pGeomPrj)
				'End If

				pZAware = pGeomGeo
				pZAware.ZAware = True

				pZAware = pGeomPrj
				pZAware.ZAware = True

				If pGeomGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
					CType(pGeomGeo, ESRI.ArcGIS.Geometry.IPoint).Z = Z
					CType(pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z = Z
				Else
					pZv = pGeomGeo
					pZv.SetConstantZ(Z)

					pZv = pGeomPrj
					pZv.SetConstantZ(Z)
				End If

				K += 1
				If K > C Then
					C += N
					ReDim Preserve ObstacleList(C)
				End If

				ObstacleList(K).pGeomGeo = pGeomGeo
				ObstacleList(K).pGeomPrj = pGeomPrj

				ObstacleList(K).UnicalName = AixmObstacle.Name
				If AixmObstacle.Type Is Nothing Then
					ObstacleList(K).TypeName = ""
				Else
					ObstacleList(K).TypeName = AixmObstacle.Type.ToString()
				End If

				'ObstacleList(K).ID = AixmObstacle.Id
				ObstacleList(K).Identifier = AixmObstacle.Identifier

				ObstacleList(K).HorAccuracy = HorAccuracy
				ObstacleList(K).VertAccuracy = VertAccuracy

				ObstacleList(K).Height = Z - fRefHeight
				ObstacleList(K).Index = K
			Next J
		Next I

		If K >= 0 Then
			ReDim Preserve ObstacleList(K)
		Else
			ReDim ObstacleList(-1)
		End If

		Return K
	End Function

	Public Function GetObstaclesByPoly(ByRef ObstacleList() As ObstacleType, ByVal pPoly As ESRI.ArcGIS.Geometry.IPolygon, Optional ByVal fRefHeight As Double = 0.0) As Integer
		Dim C As Integer
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
		Dim pElevatedCurve As ElevatedCurve
		Dim pElevatedSurface As ElevatedSurface

		Dim pARANPolygon As Aran.Geometries.MultiPolygon
		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry

		Dim pZv As ESRI.ArcGIS.Geometry.IZ
		Dim pZAware As ESRI.ArcGIS.Geometry.IZAware
		Dim pTopop As ESRI.ArcGIS.Geometry.ITopologicalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		'DrawPolygon(pPoly, RGB(127, 0, 255), ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross) '????????????????????????????

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPoly))

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1

		ReDim ObstacleList(N)

		If N < 0 Then Return -1

		C = N
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
					Case VerticalStructurePartGeometryChoice.ElevatedCurve
						pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent
						If pElevatedCurve Is Nothing Then Continue For
						If pElevatedCurve.Elevation Is Nothing Then Continue For

						HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0)
						VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0)

						pGeomGeo = AIXMCurveToESRIPolyline(pElevatedCurve)
						Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0)
					Case VerticalStructurePartGeometryChoice.ElevatedSurface
						'Continue For
						pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent
						If pElevatedSurface Is Nothing Then Continue For
						If pElevatedSurface.Elevation Is Nothing Then Continue For

						HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0)
						VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0)

						pGeomGeo = AIXMSurfaceToESRIPolygon(pElevatedSurface)
						Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0)
					Case Else
						Continue For
				End Select

				pGeomGeo.SpatialReference = pSpRefShp
				pGeomPrj = ToPrj(pGeomGeo)
				If pGeomPrj.IsEmpty() Then Continue For

				If VertAccuracy > 0.0 Then Z += VertAccuracy

				HorAccuracy = 0.0
				If HorAccuracy > 0.0 Then
					If pGeomPrj.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint And (HorAccuracy <= 2.0) Then
						pGeomPrj = CreatePrjCircle(pGeomPrj, HorAccuracy, 18)
					Else
						pTopop = pGeomPrj
						pTopop.Simplify()
						pGeomPrj = pTopop.Buffer(HorAccuracy)

						pTopo = pGeomPrj
						pTopo.IsKnownSimple_2 = False
						pTopo.Simplify()
					End If

					pGeomGeo = ToGeo(pGeomPrj)
				End If

				pZAware = pGeomGeo
				pZAware.ZAware = True

				pZAware = pGeomPrj
				pZAware.ZAware = True

				If pGeomGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
					CType(pGeomGeo, ESRI.ArcGIS.Geometry.IPoint).Z = Z
					CType(pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z = Z
				Else
					pZv = pGeomGeo
					pZv.SetConstantZ(Z)

					pZv = pGeomPrj
					pZv.SetConstantZ(Z)
				End If

				K += 1
				If K > C Then
					C += N
					ReDim Preserve ObstacleList(C)
				End If

				ObstacleList(K).pGeomGeo = pGeomGeo
				ObstacleList(K).pGeomPrj = pGeomPrj

				ObstacleList(K).UnicalName = AixmObstacle.Name
				If AixmObstacle.Type Is Nothing Then
					ObstacleList(K).TypeName = ""
				Else
					ObstacleList(K).TypeName = AixmObstacle.Type.ToString()
				End If

				'ObstacleList(K).ID = AixmObstacle.Id
				ObstacleList(K).Identifier = AixmObstacle.Identifier

				ObstacleList(K).HorAccuracy = HorAccuracy
				ObstacleList(K).VertAccuracy = VertAccuracy

				ObstacleList(K).Height = Z - fRefHeight
				ObstacleList(K).Index = K
				'ObstacleList(K).VerticalStructure = AixmObstacle
			Next J
		Next I

		If K >= 0 Then
			ReDim Preserve ObstacleList(K)
		Else
			ReDim ObstacleList(-1)
		End If

		Return K
	End Function

	Public Function GetObstaclesByDist(ByRef ObstacleList() As ObstacleType, ByVal pPtPrj As ArcGIS.Geometry.IPoint, ByVal MaxDist As Double, Optional ByVal fRefHeight As Double = 0.0) As Integer
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim M As Integer
		Dim C As Integer

		Dim Z As Double
		Dim HorAccuracy As Double
		Dim VertAccuracy As Double

		Dim VerticalStructureList As List(Of VerticalStructure)
		Dim AixmObstacle As VerticalStructure
		Dim ObstaclePart As VerticalStructurePart

		Dim pElevatedPoint As ElevatedPoint
		Dim pElevatedCurve As ElevatedCurve
		Dim pElevatedSurface As ElevatedSurface

		Dim pGeomGeo As ESRI.ArcGIS.Geometry.IGeometry
		Dim pGeomPrj As ESRI.ArcGIS.Geometry.IGeometry

		Dim pZv As ESRI.ArcGIS.Geometry.IZ
		Dim pZAware As ESRI.ArcGIS.Geometry.IZAware
		Dim pTopop As ESRI.ArcGIS.Geometry.ITopologicalOperator
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		Dim pPolygon As ESRI.ArcGIS.Geometry.IPolygon
		Dim pARANPolygon As MultiPolygon

		Dim pReleation As ESRI.ArcGIS.Geometry.IRelationalOperator
		'=============================================================================
		pPolygon = CreatePrjCircle(pPtPrj, MaxDist)
		pReleation = pPolygon

		'pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pPolygon))
		'=============================================================================

		Dim pExtendPoly As ESRI.ArcGIS.Geometry.IPolygon
		Dim pCurrPt As ArcGIS.Geometry.IPoint
		Dim pTmpPoints As ArcGIS.Geometry.IPointCollection

		pExtendPoly = New ESRI.ArcGIS.Geometry.Polygon()
		pTmpPoints = pExtendPoly

		pCurrPt = New ESRI.ArcGIS.Geometry.Point

		pCurrPt.PutCoords(pPtPrj.X - MaxDist, pPtPrj.Y - MaxDist)
		pTmpPoints.AddPoint(pCurrPt)

		pCurrPt.PutCoords(pPtPrj.X - MaxDist, pPtPrj.Y + MaxDist)
		pTmpPoints.AddPoint(pCurrPt)

		pCurrPt.PutCoords(pPtPrj.X + MaxDist, pPtPrj.Y + MaxDist)
		pTmpPoints.AddPoint(pCurrPt)

		pCurrPt.PutCoords(pPtPrj.X + MaxDist, pPtPrj.Y - MaxDist)
		pTmpPoints.AddPoint(pCurrPt)

		pExtendPoly.Close()
		pTopo = pExtendPoly
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		'DrawPolygon(pExtendPoly, -1, ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
		'Application.DoEvents()

		pARANPolygon = ESRIGeometryToARANGeometry(ToGeo(pExtendPoly))
		'=============================================================================

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1


		ReDim ObstacleList(-1)

		If N < 0 Then
			ReDim ObstacleList(-1)
			Return -1
		End If

		ReDim ObstacleList(N)

		C = N
		K = -1

		For I = 0 To N
			AixmObstacle = VerticalStructureList.Item(I)
			Try
				M = AixmObstacle.Part.Count - 1
				For J = 0 To M
					ObstaclePart = AixmObstacle.Part(J)
					If ObstaclePart.HorizontalProjection Is Nothing Then Continue For

					Select Case ObstaclePart.HorizontalProjection.Choice
						Case VerticalStructurePartGeometryChoice.ElevatedPoint
							'Continue For

							pElevatedPoint = ObstaclePart.HorizontalProjection.Location
							If pElevatedPoint Is Nothing Then Continue For
							If pElevatedPoint.Elevation Is Nothing Then Continue For

							HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
							VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0)

							pGeomGeo = AIXMPointToESRIPoint(pElevatedPoint)
							Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0)
						Case VerticalStructurePartGeometryChoice.ElevatedCurve
							'Continue For
							pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent
							If pElevatedCurve Is Nothing Then Continue For
							If pElevatedCurve.Elevation Is Nothing Then Continue For

							HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0)
							VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0)

							pGeomGeo = AIXMCurveToESRIPolyline(pElevatedCurve)
							Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0)
						Case VerticalStructurePartGeometryChoice.ElevatedSurface
							'Continue For

							pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent
							If pElevatedSurface Is Nothing Then Continue For
							If pElevatedSurface.Elevation Is Nothing Then Continue For

							HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0)
							VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0)

							pGeomGeo = AIXMSurfaceToESRIPolygon(pElevatedSurface)
							Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0)
						Case Else
							Continue For
					End Select

					pGeomGeo.SpatialReference = pSpRefShp
					pGeomPrj = ToPrj(pGeomGeo)
					If pGeomPrj.IsEmpty() Then Continue For
					If pReleation.Disjoint(pGeomPrj) Then Continue For

					If VertAccuracy > 0.0 Then Z += VertAccuracy

					HorAccuracy = 0.0
					If HorAccuracy > 0.0 Then
						If pGeomPrj.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPoint And (HorAccuracy <= 2.0) Then
							pGeomPrj = CreatePrjCircle(pGeomPrj, HorAccuracy, 18)
						Else
							pTopop = pGeomPrj
							pTopop.Simplify()
							pGeomPrj = pTopop.Buffer(HorAccuracy)

							pTopo = pGeomPrj
							pTopo.IsKnownSimple_2 = False
							pTopo.Simplify()
						End If

						pGeomGeo = ToGeo(pGeomPrj)
					End If

					pZAware = pGeomGeo
					pZAware.ZAware = True

					pZAware = pGeomPrj
					pZAware.ZAware = True

					If pGeomGeo.GeometryType = ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
						CType(pGeomGeo, ESRI.ArcGIS.Geometry.IPoint).Z = Z
						CType(pGeomPrj, ESRI.ArcGIS.Geometry.IPoint).Z = Z
					Else
						pZv = pGeomGeo
						pZv.SetConstantZ(Z)

						pZv = pGeomPrj
						pZv.SetConstantZ(Z)
					End If

					K += 1
					'DrawPolygon(pGeomPrj, , ArcGIS.Display.esriSimpleFillStyle.esriSFSCross)
					'Application.DoEvents()

					If K > C Then
						C += N
						ReDim Preserve ObstacleList(C)
					End If

					ObstacleList(K).pGeomGeo = pGeomGeo
					ObstacleList(K).pGeomPrj = pGeomPrj

					ObstacleList(K).UnicalName = AixmObstacle.Name
					If AixmObstacle.Type Is Nothing Then
						ObstacleList(K).TypeName = ""
					Else
						ObstacleList(K).TypeName = AixmObstacle.Type.ToString()
					End If

					ObstacleList(K).Identifier = AixmObstacle.Identifier

					ObstacleList(K).HorAccuracy = HorAccuracy
					ObstacleList(K).VertAccuracy = VertAccuracy

					ObstacleList(K).Height = Z - fRefHeight
					ObstacleList(K).Index = K
					'ObstacleList(K).VerticalStructure = AixmObstacle
				Next J
			Catch exc As Exception
				MessageBox.Show("GetObstaclesByDist encountred error on obstacle " + AixmObstacle.Name + "." + vbCrLf + exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try
		Next I

		If K >= 0 Then
			ReDim Preserve ObstacleList(K)
		Else
			ReDim ObstacleList(-1)
		End If

		Return K
	End Function

	Public Function FillAIXMMSAList(ByRef MSAList() As MSAType, ByRef NavList() As NavaidType) As Integer
		Dim i As Integer
		Dim j As Integer
		Dim k As Integer
		Dim n As Integer
		Dim iMSANum As Integer
		Dim iSecNum As Integer

		Dim pSafeAltitudeAreaList As List(Of SafeAltitudeArea)
		Dim pSafeAltitudeArea As SafeAltitudeArea

		Dim pSafeAltitudeAreaSectorList As List(Of SafeAltitudeAreaSector)
		Dim pSafeAltitudeAreaSector As SafeAltitudeAreaSector

		Dim pSectorDefinition As CircleSector

		Dim rMax As Double
		Dim RMSA As Double
		Dim MaxHeight As Double

		n = UBound(NavList)
		ReDim MSAList(n)

		If n < 0 Then Return -1

		iMSANum = -1

		For i = 0 To n
			pSafeAltitudeAreaList = pObjectDir.GetSafeAltitudeAreaList(NavList(i).GetFeatureRef().Identifier)

			For j = 0 To pSafeAltitudeAreaList.Count - 1
				pSafeAltitudeArea = pSafeAltitudeAreaList(j)
				pSafeAltitudeAreaSectorList = pSafeAltitudeArea.Sector 'pObjectDir.GetSafeAltitudeAreaSector List(pSafeAltitudeArea.Id)

				If pSafeAltitudeAreaSectorList.Count > 0 Then
					iMSANum = iMSANum + 1

					MSAList(iMSANum).SectorsNum = pSafeAltitudeAreaSectorList.Count
					MSAList(iMSANum).Navaid = NavList(i)
					MSAList(iMSANum).Name = NavList(i).CallSign

					rMax = 0
					MaxHeight = -10000
					ReDim MSAList(iMSANum).Sectors(MSAList(iMSANum).SectorsNum - 1)
					iSecNum = -1

					For k = 0 To pSafeAltitudeAreaSectorList.Count - 1
						pSafeAltitudeAreaSector = pSafeAltitudeAreaSectorList(k)
						pSectorDefinition = pSafeAltitudeAreaSector.SectorDefinition

						If (Not pSectorDefinition.UpperLimit Is Nothing) Or (Not pSectorDefinition.LowerLimit Is Nothing) Then
							If pSectorDefinition.UpperLimit Is Nothing Then pSectorDefinition.UpperLimit = pSectorDefinition.LowerLimit
							If pSectorDefinition.LowerLimit Is Nothing Then pSectorDefinition.LowerLimit = pSectorDefinition.UpperLimit

							iSecNum = iSecNum + 1
							RMSA = ConverterToSI.Convert(pSectorDefinition.OuterDistance, arMSARange)

							If rMax < RMSA Then rMax = RMSA
							MSAList(iMSANum).Sectors(iSecNum).OuterDist = RMSA
							MSAList(iMSANum).Sectors(iSecNum).InnerDist = ConverterToSI.Convert(pSectorDefinition.InnerDistance, 0)

							MSAList(iMSANum).Sectors(iSecNum).LowerLimit = ConverterToSI.Convert(pSectorDefinition.LowerLimit, 0.0)
							MSAList(iMSANum).Sectors(iSecNum).UpperLimit = ConverterToSI.Convert(pSectorDefinition.UpperLimit, 0.0)
							MSAList(iMSANum).Sectors(iSecNum).BufferWidth = ConverterToSI.Convert(pSafeAltitudeAreaSector.BufferWidth, arBufferMSA.Value)

							If MaxHeight < MSAList(iMSANum).Sectors(iSecNum).LowerLimit Then MaxHeight = MSAList(iMSANum).Sectors(iSecNum).LowerLimit

							If pSectorDefinition.AngleDirectionReference = CodeDirectionReference.FROM Then
								MSAList(iMSANum).Sectors(iSecNum).FromDir = pSectorDefinition.FromAngle.Value + 180.0
								MSAList(iMSANum).Sectors(iSecNum).ToDir = pSectorDefinition.ToAngle.Value + 180.0
							Else
								MSAList(iMSANum).Sectors(iSecNum).FromDir = pSectorDefinition.FromAngle.Value
								MSAList(iMSANum).Sectors(iSecNum).ToDir = pSectorDefinition.ToAngle.Value
							End If

							If pSectorDefinition.AngleType = CodeBearing.MAG Then
								MSAList(iMSANum).Sectors(iSecNum).FromDir = MSAList(iMSANum).Sectors(iSecNum).FromDir + NavList(i).MagVar
								MSAList(iMSANum).Sectors(iSecNum).ToDir = MSAList(iMSANum).Sectors(iSecNum).ToDir + NavList(i).MagVar
							End If

							MSAList(iMSANum).Sectors(iSecNum).SectorPoly = ToPrj(AIXMSurfaceToESRIPolygon(pSafeAltitudeAreaSector.Extent))
							'MSAList(iMSANum).Sectors(iSecNum).SectorPoly = CreateMSASectorPrj(NavList(i).pPtGeo, MSAList(iMSANum).Sectors(iSecNum))
						End If
					Next k

					If iSecNum >= 0 Then
						ReDim Preserve MSAList(iMSANum).Sectors(iSecNum)
						MSAList(iMSANum).MaxRadius = rMax
						MSAList(iMSANum).MaxHeight = MaxHeight
					Else
						iMSANum = iMSANum - 1
					End If
				End If
			Next j
		Next i
		'======================================================================

		If iMSANum < 0 Then
			ReDim MSAList(-1)
			Return -1
		End If

		ReDim Preserve MSAList(iMSANum)
		Return iMSANum + 1
	End Function

	Public Function GetMSASectors(ByVal Nav As NavaidType, ByRef MSAList() As MSAType) As Boolean
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim M As Integer
		Dim N As Integer

		Dim fTmp As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim SAAList As List(Of SafeAltitudeArea)
		Dim SAASectorList As List(Of SafeAltitudeAreaSector)
		Dim SectorDefinition As CircleSector

		Dim SafeAltitudeArea As SafeAltitudeArea
		Dim SafeAltitudeSector As SafeAltitudeAreaSector
		Dim pSignificantPoint As SignificantPoint

		SAAList = pObjectDir.GetSafeAltitudeAreaList(Nav.GetSignificantPoint().NavaidSystem.Identifier)
		N = SAAList.Count

		ReDim MSAList(N - 1)
		If N = 0 Then Return False

		J = -1

		For I = 0 To N - 1
			SafeAltitudeArea = SAAList.Item(I)

			If SafeAltitudeArea.SafeAreaType.Value = Enums.CodeSafeAltitude.MSA Then
				SAASectorList = SafeAltitudeArea.Sector
				M = SAASectorList.Count
				If M > 0 Then
					J = J + 1
					MSAList(J).Identifier = SafeAltitudeArea.Identifier
					ReDim MSAList(J).Sectors(M - 1)

					pSignificantPoint = SafeAltitudeArea.CentrePoint
					If pSignificantPoint.Choice = SignificantPointChoice.Navaid Then
						'pNavaid = pSignificantPoint.NavaidSystem.GetFeature(Of Navaid)()
						'For L = 0 To pNavaid.NavaidEquipment.Count - 1
						'	pNAVEq = pNavaid.NavaidEquipment(L).TheNavaidEquipment.GetFeature(Of NavaidEquipment)()
						'	If (pNAVEq.NavaidEquipmentType = NavaidEquipmentType.NDB) Or _
						'	 (pNAVEq.NavaidEquipmentType = NavaidEquipmentType.TACAN) Or _
						'	 (pNAVEq.NavaidEquipmentType = NavaidEquipmentType.VOR) Then
						'		ptCenter = AixmPointToESRIPoint(pNAVEq.Location)
						'		Exit For
						'	End If
						'Next
					ElseIf pSignificantPoint.Choice = SignificantPointChoice.AirportHeliport Then
						'pADHP = pSignificantPoint.AirportReferencePoint.GetFeature(Of AirportHeliport)()
						'ptCenter = AixmPointToESRIPoint(pNAVEq.Location)
					ElseIf pSignificantPoint.Choice = SignificantPointChoice.DesignatedPoint Then
						'pDesign = pSignificantPoint.FixDesignatedPoint.GetFeature(Of DesignatedPoint)()
						'ptCenter = AixmPointToESRIPoint(pNAVEq.Location)
					Else
						Continue For
					End If

					For K = 0 To M - 1
						SafeAltitudeSector = SAASectorList.Item(K)
						SectorDefinition = SafeAltitudeSector.SectorDefinition

						FromAngle = SectorDefinition.FromAngle.Value
						ToAngle = SectorDefinition.ToAngle.Value

						If SectorDefinition.AngleDirectionReference.Value = Aran.Aim.Enums.CodeDirectionReference.TO Then
							FromAngle = FromAngle + 180.0
							ToAngle = ToAngle + 180.0
						End If

						If SectorDefinition.ArcDirection.Value = CodeArcDirection.CCA Then
							fTmp = FromAngle
							FromAngle = ToAngle
							ToAngle = fTmp
						End If

						If SectorDefinition.AngleType.Value = CodeBearing.MAG Then
							FromAngle += Nav.MagVar
							ToAngle += Nav.MagVar
						End If

						MSAList(J).Sectors(K).LowerLimit = ConverterToSI.Convert(SectorDefinition.LowerLimit, 0.0)
						MSAList(J).Sectors(K).UpperLimit = ConverterToSI.Convert(SectorDefinition.UpperLimit, 0.0)

						MSAList(J).Sectors(K).InnerDist = ConverterToSI.Convert(SectorDefinition.InnerDistance, 0.0)
						MSAList(J).Sectors(K).OuterDist = ConverterToSI.Convert(SectorDefinition.OuterDistance, MinDistanceSect)
						MSAList(J).Sectors(K).FromDir = FromAngle
						MSAList(J).Sectors(K).ToDir = ToAngle
						MSAList(J).Sectors(K).AbsAngle = SubtractAngles(FromAngle, ToAngle)
						'				MSAList(J).Sectors(K).Sector = CreateMSASectorPrj(ptCenter, MSAList(J).Sectors(K))
					Next K
				End If
			End If
		Next I

		If J >= 0 Then
			ReDim Preserve MSAList(J)
		Else
			ReDim MSAList(-1)
		End If

		Return J >= 0
	End Function

	Private Function CreateMSASectorPrj(ByVal ptCntGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal Sector As MSASectorType) As ESRI.ArcGIS.Geometry.IPolygon
		Dim I As Integer
		Dim N As Integer

		Dim AngleFrom As Double
		Dim AngleTo As Double
		Dim dAngle As Double
		Dim AngStep As Integer

		Dim iInRad As Double
		Dim CosI As Double
		Dim SinI As Double

		Dim ptCntPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim ptInner As ESRI.ArcGIS.Geometry.IPoint
		Dim ptOuter As ESRI.ArcGIS.Geometry.IPoint

		Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		pPolygon = New ESRI.ArcGIS.Geometry.Polygon

		ptCntPrj = ToPrj(ptCntGeo)
		ptInner = New ESRI.ArcGIS.Geometry.Point
		ptOuter = New ESRI.ArcGIS.Geometry.Point

		AngStep = 1.0

		If Sector.ToDir - Sector.FromDir > 359.999 Then
			AngleFrom = 0.0
			N = 360
		Else
			AngleTo = Azt2Dir(ptCntGeo, Sector.FromDir + 180.0)
			AngleFrom = Azt2Dir(ptCntGeo, Sector.ToDir + 180.0)

			dAngle = Modulus(AngleTo - AngleFrom)
			N = System.Math.Round(dAngle / AngStep)

			If (N < 1) Then
				N = 1
			ElseIf (N < 5) Then
				N = 5
			ElseIf (N < 10) Then
				N = 10
			End If

			AngStep = dAngle / N
		End If

		For I = 0 To N
			iInRad = DegToRadValue * (AngleFrom + I * AngStep)
			CosI = System.Math.Cos(iInRad)
			SinI = System.Math.Sin(iInRad)

			ptInner.X = ptCntPrj.X + Sector.InnerDist * CosI
			ptInner.Y = ptCntPrj.Y + Sector.InnerDist * SinI

			ptOuter.X = ptCntPrj.X + Sector.OuterDist * CosI
			ptOuter.Y = ptCntPrj.Y + Sector.OuterDist * SinI

			pPolygon.AddPoint(ptInner)
			pPolygon.AddPoint(ptOuter, 0)
		Next I

		pTopo = pPolygon
		pTopo.IsKnownSimple_2 = False
		pTopo.Simplify()

		Return pPolygon
	End Function
	'===========================================================================================================================

	'Public Function CreateDesignatedPoint(ByVal pPtPrj As ESRI.ArcGIS.Geometry.IPoint, Optional ByVal Name As String = "COORD", Optional ByVal fDirTreshold As Double = -1000.0) As DesignatedPoint
	'	Dim pFixDesignatedPoint As DesignatedPoint
	'	Dim fMinDist As Double
	'	Dim fDist As Double
	'	Dim fDirToPt As Double

	'	Dim I As Long
	'	Dim N As Long
	'	Dim bExist As Boolean
	'	Dim WptFIX As WPT_FIXType

	'	fMinDist = 10000.0
	'	N = UBound(WPTList)

	'	If N >= 0 Then
	'		For I = 0 To N
	'			fDist = ReturnDistanceInMeters(pPtPrj, WPTList(I).pPtPrj)

	'			If (fDist < fMinDist) And (WPTList(I).TypeCode = eNavaidType.NONE) Then
	'				fMinDist = fDist
	'				WptFIX = WPTList(I)
	'			End If
	'		Next I
	'	End If

	'	bExist = fMinDist <= 10.0

	'	If (Not bExist) And (fMinDist <= 100.0) And (fDirTreshold <> -1000.0) Then
	'		fDirToPt = ReturnAngleInDegrees(pPtPrj, WptFIX.pPtPrj)
	'		bExist = SubtractAngles(fDirTreshold, fDirToPt) < 0.1
	'	End If

	'	If bExist Then
	'		Return WptFIX.pFeature
	'	End If

	'	pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature(Of DesignatedPoint)()
	'	pFixDesignatedPoint.Designator = "COORD"
	'	pFixDesignatedPoint.Name = Name

	'	pFixDesignatedPoint.Location = ESRIPointToAixmPoint(ToGeo(pPtPrj))
	'	'pFixDesignatedPoint.Note
	'	'pFixDesignatedPoint.Tag
	'	pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.DESIGNED
	'	Return pFixDesignatedPoint
	'End Function

	'Public Function CreateAngleIndication(ByVal Angle As Double, ByVal AngleType As Aran.Aim.Enums.CodeBearing, ByVal pSignificantPoint As SignificantPoint) As AngleIndication
	'	Dim pAngleIndication As AngleIndication

	'	pAngleIndication = DBModule.pObjectDir.CreateFeature(Of AngleIndication)()
	'	pAngleIndication.Angle = Angle
	'	pAngleIndication.AngleType = AngleType
	'	pAngleIndication.PointChoice = pSignificantPoint

	'	Return pAngleIndication
	'End Function

	'Public Function CreateDistanceIndication(ByVal Distance As Double, ByVal Uom As UomDistance, ByVal pSignificantPoint As SignificantPoint) As DistanceIndication
	'	Dim pDistanceIndication As DistanceIndication
	'	Dim pDistance As ValDistance

	'	pDistanceIndication = DBModule.pObjectDir.CreateFeature(Of DistanceIndication)()

	'	pDistance = New ValDistance
	'	pDistance.Uom = Uom
	'	pDistance.Value = Distance

	'	pDistanceIndication.Distance = pDistance
	'	pDistanceIndication.PointChoice = pSignificantPoint

	'	Return pDistanceIndication
	'End Function

	Private isOpen As Boolean = False

	Public Function InitModule() As String
		Dim dbPro As Aran.Aim.Data.DbProvider = gAranEnv.DbProvider

		If (Not isOpen) Then
			pObjectDir = PandaSQPIFactory.Create()
			Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir
			pObjectDir.Open(dbPro)

			Dim tdrObj As Object = gAranEnv.CommonData.GetObject("terrainDataReader")
			Dim terrainDataReader As TerrainDataReaderEventHandler = CTypeDynamic(Of TerrainDataReaderEventHandler)(tdrObj)
			AddHandler pObjectDir.TerrainDataReader, terrainDataReader

			isOpen = True
			UserName = dbPro.CurrentUser.Name
		End If

		Return (dbPro.CurrentUser.Name)
	End Function

	Public Sub CloseDB()
		If (isOpen) Then
			'pObjectDir.Close()
			isOpen = False
		End If
	End Sub
End Module
