using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;
using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]

	public enum RunwayPointRoleType
	{
		ABEAM_ELEVATION,
		ABEAM_GLIDESLOPE,
		ABEAM_PAR,
		ABEAM_RER,
		ABEAM_TDR,
		DISTHR,
		END,
		LAHSO,
		MID,
		START,
		START_RUN,
		TDZ,
		THR
	}

	public class IRunwayCentrelinePoint
	{
		List<IPoint> ElevatedPointList;
		string Id;
		RunwayPointRoleType Role;
		string RunwayDirectionId;
	}

	public static class SS_DBModule
	{
		//Public Const AIRPORT_LayerName = "AIRPORT"
		//    Public Const AIRName = "NAME"
		//    Public Const AIRICAO_ID = "ICAO_ID"
		//    Public Const AIRElev = "Elev_M"
		//    Public Const AIRMagVar = "Var"
		//    Public Const AIRMinTMA = "Min_TMA"
		//    Public Const AIRWindSpeed = "WindSpeed"
		//    Public Const AIRISAtC = "ISA_C"

		//Public Const RWY_LayerName = "RWY"
		//    Public Const RWYADHP_ID = "ICAO_ID"
		//    Public Const RWYADHP = "AIRPORT"
		//    Public Const RWY_ID = "RWY_ID"
		//    Public Const RWYCode = "CODE"
		//    Public Const RWYClass = "CLASS"
		//    Public Const RWYPair_ID = "PAIR_ID"
		//    Public Const RWYElev = "ELEV_M"
		//    Public Const RWYGeo = "GEO"
		//    Public Const RWYClearWay = "CLEARWAY"
		//    Public Const RWYDplm = "DPLM_M"
		//    Public Const RWYLength = "Length_M"
		//    Public Const RWYWidth = "WIDTH_M"

		//	Public Const NAVAID_LayerName = "NAVAID"
		//	Public Const OBSTACLE_LayerName = "OBSTACLE"
		//	Public Const WPT_FIX_LayerName = "WPT_FIX"
		//	Public Const WARNING_LayerName = "WARNING"
		//	Public Const MSA_LayerName = "MSA"
		//	Public Const ILS_LayerName = "ILS"

		struct CLPoint
		{
			public IRunwayCentrelinePoint pCLPoint;
			public IPoint pPtGeo;
			public IPoint pPtPrj;
		}
		static IFeatureLayer pADHPLayer;
		static IFeatureClass pADHPFeatureClass;

		public static int FillADHPList(ref ADHPType[] ADHPList, Guid organ, bool CheckILS = false)
		{
			int I;
			int N;
			bool bLayerFound = false;

			IQueryFilter pFilter;
			ICursor pCursor;
			IRow pRow;
			pADHPLayer = null;

			ADHPList = GlobalVars.InitArray<ADHPType>(0);

			for (I = 0; I < GlobalVars.pMap.LayerCount; I++)
			{
				if (GlobalVars.pMap.Layer[I].Name == "AIRPORT")
				{
					pADHPLayer = GlobalVars.pMap.Layer[I] as IFeatureLayer;
					bLayerFound = true;
					break;
				}
			}
			/*===================*/
			if (!bLayerFound)
				return -1;

			pADHPFeatureClass = pADHPLayer.FeatureClass;

			pFilter = new QueryFilter();
			pFilter.WhereClause = pADHPFeatureClass.OIDFieldName + ">=0";

			N = pADHPFeatureClass.FeatureCount(pFilter);
			if (N == 0)
				return -1;

			System.Array.Resize(ref ADHPList, N);

			int iNameField, iIDField;

			iNameField = pADHPFeatureClass.FindField("Name");
			iIDField = pADHPFeatureClass.FindField("Identifer");

			pCursor = (ICursor)pADHPFeatureClass.Search(pFilter, true);
			pRow = pCursor.NextRow();


			for (I = 0; I < N; I++)
			{
				ADHPList[I].Name = pRow.Value[iNameField].ToString();
				ADHPList[I].ID = (Guid)pRow.Value[iIDField];
				ADHPList[I].Index = I;
				pRow = pCursor.NextRow();
			}

			return N - 1;
		}

		public int FillADHPFields(ref ADHPType CurADHP)
		{
			//IRelationalOperator pRelational;
			IPoint pPtGeo;
			IPoint pPtPrj;


			if (CurADHP.pPtGeo != null)
				return 0;

			IQueryFilter pFilter;
			ICursor pCursor;
			IRow pRow;

			pFilter = new QueryFilter();
			pFilter.WhereClause = "Identifer=" + CurADHP.ID;

			//if (pADHPFeatureClass.FeatureCount(pFilter) == 0)
			//	return -1;

			pCursor = (ICursor)pADHPFeatureClass.Search(pFilter, true);
			pRow = pCursor.NextRow();
			if (pRow == null)
				return -1;

			int iShape = pADHPFeatureClass.FindField(pADHPFeatureClass.ShapeFieldName);

			pPtGeo = pRow.Value[iShape] as IPoint;

			pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;

			if (pPtPrj.IsEmpty)
				return -1;

			//pRelational = P_LicenseRect;
			//if(!pRelational.Contains(pPtPrj))
			//    return -1;

			//pConverter = new AixmConvertType();

			CurADHP.pPtGeo = pPtGeo;
			CurADHP.pPtPrj = pPtPrj;

			int iOrganisationId = pADHPFeatureClass.FindField("OrganisationId");
			int iMagneticVariation = pADHPFeatureClass.FindField("MagneticVariation");
			int iElevation = pADHPFeatureClass.FindField("Elevation");
			int iReferenceTemperature = pADHPFeatureClass.FindField("ReferenceTemperature");
			int iTransitionLevel = pADHPFeatureClass.FindField("TransitionLevel");

			CurADHP.OrgID = (System.Guid)pRow.Value[iOrganisationId];

			object vObject = pRow.Value[iMagneticVariation];
			try
			{
				CurADHP.MagVar = System.Convert.ToDouble(vObject);
			}
			catch
			{
				CurADHP.MagVar = 0.0;
			}

			vObject = pRow.Value[iElevation];
			try
			{
				CurADHP.Elev = System.Convert.ToDouble(vObject);
			}
			catch
			{
				CurADHP.Elev = 0.0;
			}

			CurADHP.pPtGeo.Z = CurADHP.Elev;
			CurADHP.pPtPrj.Z = CurADHP.Elev;

			vObject = pRow.Value[iReferenceTemperature];
			try
			{
				CurADHP.ISAtC = System.Convert.ToDouble(vObject);
			}
			catch
			{
				CurADHP.ISAtC = 15.0;
			}

			//CurADHP.ReferenceTemperature = pConverter.ConvertToSI(ah.ReferenceTemperature)
			//CurADHP.TransitionAltitude = pConverter.ConvertToSI(pADHP.TransitionAltitude, 2500#)
			//CurADHP.MinTMA = pConverter.ConvertToSI(pADHP.TransitionAltitude, 2500#)

			vObject = pRow.Value[iTransitionLevel];
			try
			{
				CurADHP.TransitionLevel = System.Convert.ToDouble(vObject);
			}
			catch
			{
				CurADHP.TransitionLevel = 2500.0;
			}

			CurADHP.WindSpeed = 56.0;
			return 1;
		}

		IRunwayCentrelinePoint GetNearestPoint(ref CLPoint[] CLPointArray, IPoint pPtGeo, double MaxDist = 5.0)
		{
			double fTmp;
			double fDist = MaxDist;
			IPoint ptA = Functions.ToPrj(pPtGeo) as IPoint;
			IRunwayCentrelinePoint result = null;
			int n = CLPointArray.Length;



			for (int i = 0; i < n; i++)
			{
				if (CLPointArray[i].pPtPrj != null)
				{
					fTmp = Functions.ReturnDistanceInMeters(ptA, CLPointArray[i].pPtPrj);
					if (fTmp < fDist)
					{
						result = CLPointArray[i].pCLPoint;
						fDist = fTmp;
					}
				}
			}
			return result;
		}

		//        public int FillRWYList(ref RWYType[] RWYList, ADHPType Owner)
		//        {
		//            Dim iRwyNum As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim K As Long
		//            Dim ResX As Double
		//            Dim ResY As Double
		//            Dim dDT As Double
		//            Dim fTmp As Double
		//            Dim TrueBearing As Double

		//            Dim pName As IUnicalName
		//            Dim pAIXMRWYList As IUnicalNameList
		//            Dim pRwyDRList As IRunwayDirectionList
		//            Dim pCenterLinePointList As IRunwayCentrelinePointList
		//            Dim pElevatedPointList As IElevatedPointList
		//            Dim pRunwayProtectAreaList As IRunwayProtectAreaList

		//            Dim pRunway As IRunway
		//            Dim pRwyDirection As IRunwayDirection
		//            Dim pRwyDirectinPair As IRunwayDirection
		//            Dim pRunwayProtectArea As IRunwayProtectArea
		//            Dim pRunwayCenterLinePoint As IRunwayCentrelinePoint

		//            Dim pGMLPoint As IGMLPoint
		//            Dim pElevPoint As IElevatedPoint
		//            Dim pConverter As IAixmConvertType
		//            Dim pAirportHeliportProtectionArea As IAirportHeliportProtectionArea

		//            Dim CLPointArray() As CLPoint
		//            Dim pDirDeclaredDist As RunwayDirDeclaredDist
		//            Dim pDeclaredDistList As IRunwayDirDeclaredDistList
		//            Dim fLDA As Double
		//            Dim fTORA As Double
		//            Dim fTODA As Double

		//            pAIXMRWYList = pObjectDir.GetRunwayList(Owner.ID, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type)
		//            pConverter = New AixmConvertType

		//            If pAIXMRWYList.Count = 0 Then
		//        ReDim RWYList(-1 To -1)
		//                FillRWYList = -1
		//                Exit Function
		//            End If

		//            ReDim RWYList(0 To 2 * pAIXMRWYList.Count - 1)
		//            pConverter = New AixmConvertType
		//            iRwyNum = -1

		//            For I = 0 To pAIXMRWYList.Count - 1
		//                pName = pAIXMRWYList.GetItem(I)
		//                pRunway = pObjectDir.GetRunway(pName.ID)
		//                pRwyDRList = pObjectDir.GetRunwayDirectionList(pName.ID)

		//                If pRwyDRList.Count = 2 Then
		//                    For J = 0 To 1
		//                        iRwyNum = iRwyNum + 1

		//                        RWYList(iRwyNum).Length = pConverter.ConvertToSI(pRunway.Length, -9999.0#)
		//                        If RWYList(iRwyNum).Length < 0 Then
		//                            iRwyNum = iRwyNum - 1
		//                            GoTo NextI
		//                        End If

		//                        pRwyDirection = pRwyDRList.GetItem(J)
		//                        pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.ID)

		//                        If pCenterLinePointList.Count = 0 Then
		//                            iRwyNum = iRwyNum - 1
		//                            GoTo NextI
		//                        End If

		//                        RWYList(iRwyNum).pPtGeo(PtTHR) = Nothing

		//                        ReDim CLPointArray(0 To pCenterLinePointList.Count - 1)
		//                        For K = 0 To pCenterLinePointList.Count - 1
		//                            pRunwayCenterLinePoint = pCenterLinePointList.GetItem(K)
		//                            pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList

		//                            If pElevatedPointList.Count >= 1 Then
		//                                CLPointArray(K).pCLPoint = pRunwayCenterLinePoint
		//                                pGMLPoint = pElevatedPointList.GetItem(0)
		//                                pElevPoint = pGMLPoint

		//                                CLPointArray(K).pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                                CLPointArray(K).pPtPrj = ToPrj(CLPointArray(K).pPtGeo)

		//                                If pRunwayCenterLinePoint.Role = RunwayPointRoleType_THR Then
		//                                    RWYList(iRwyNum).pPtGeo(PtTHR) = CLPointArray(K).pPtGeo
		//                                    RWYList(iRwyNum).pPtPrj(PtTHR) = CLPointArray(K).pPtPrj

		//                                    RWYList(iRwyNum).pPtGeo(PtTHR).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                                    RWYList(iRwyNum).pPtPrj(PtTHR).Z = RWYList(iRwyNum).pPtGeo(PtTHR).Z
		//                                End If

		//                                //'                        Select Case pRunwayCenterLinePoint.Role
		//                                //'                        Case RunwayPointRoleType_START
		//                                //'                            Set RWYList(iRwyNum).pPtGeo(PtStart) = GmlPointToIPoint(pGMLPoint)
		//                                //'                            RWYList(iRwyNum).pPtGeo(PtStart).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                                //'                        Case RunwayPointRoleType_THR
		//                                //'                            Set RWYList(iRwyNum).pPtGeo(PtTHR) = GmlPointToIPoint(pGMLPoint)
		//                                //'                            RWYList(iRwyNum).pPtGeo(PtTHR).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                                //'                        Case RunwayPointRoleType_END
		//                                //'                            Set RWYList(iRwyNum).pPtGeo(PtEnd) = GmlPointToIPoint(pGMLPoint)
		//                                //'                            RWYList(iRwyNum).pPtGeo(PtEnd).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                                //'
		//                                //'                        End Select
		//                            End If
		//                        Next K

		//                        If RWYList(iRwyNum).pPtGeo(PtTHR) Is Nothing Then
		//                            iRwyNum = iRwyNum - 1
		//                            GoTo NextI
		//                        End If

		//                        If Not pRwyDirection.TrueBearing Is Nothing Then
		//                            RWYList(iRwyNum).TrueBearing = pRwyDirection.TrueBearing
		//                        ElseIf Not pRwyDirection.MagneticBearing Is Nothing Then
		//                            RWYList(iRwyNum).TrueBearing = pRwyDirection.MagneticBearing + Owner.MagVar
		//                        Else
		//                            ReturnGeodesicAzimuth(RWYList(iRwyNum).pPtGeo(PtStart).X, RWYList(iRwyNum).pPtGeo(PtStart).Y, RWYList(iRwyNum).pPtGeo(ptEnd).X, RWYList(iRwyNum).pPtGeo(ptEnd).Y, TrueBearing, fTmp)
		//                            RWYList(iRwyNum).TrueBearing = TrueBearing
		//                        End If

		//                        pDeclaredDistList = pObjectDir.GetDeclaredDistance(pRwyDirection.ID)

		//                        For K = 0 To pDeclaredDistList.Count - 1
		//                            pDirDeclaredDist = pDeclaredDistList.GetItem(K)
		//                            If pDirDeclaredDist.CodeType = ARANDB_DeclaredDistanceType_LDA Then
		//                                fLDA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, 0)
		//                            ElseIf pDirDeclaredDist.CodeType = ARANDB_DeclaredDistanceType_TORA Then
		//                                fTORA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, 0)
		//                            ElseIf pDirDeclaredDist.CodeType = ARANDB_DeclaredDistanceType_TODA Then
		//                                fTODA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, 0)
		//                            End If
		//                        Next
		//                        //==============================================================================================================
		//                        dDT = fTORA - fLDA
		//                        If dDT > 0 Then
		//                            PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(PtTHR).X, RWYList(iRwyNum).pPtGeo(PtTHR).Y, dDT, RWYList(iRwyNum).TrueBearing + 180.0#, ResX, ResY)
		//                            RWYList(iRwyNum).pPtGeo(PtStart) = New Point
		//                            RWYList(iRwyNum).pPtGeo(PtStart).PutCoords(ResX, ResY)

		//                            pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(PtStart))
		//                            If Not (pRunwayCenterLinePoint Is Nothing) Then
		//                                pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList
		//                                pGMLPoint = pElevatedPointList.GetItem(0)
		//                                pElevPoint = pGMLPoint

		//                                RWYList(iRwyNum).pPtGeo(PtStart) = GmlPointToIPoint(pGMLPoint)
		//                                RWYList(iRwyNum).pPtPrj(PtStart) = ToPrj(CLPointArray(I).pPtGeo)
		//                                RWYList(iRwyNum).pPtGeo(PtStart).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                            Else
		//                                pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(PtStart), 10000.0#)
		//                                If Not (pRunwayCenterLinePoint Is Nothing) Then
		//                                    pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList
		//                                    pElevPoint = pElevatedPointList.GetItem(0)
		//                                    RWYList(iRwyNum).pPtGeo(PtStart).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                                Else
		//                                    RWYList(iRwyNum).pPtGeo(PtStart).Z = Owner.Elev
		//                                End If
		//                            End If
		//                        Else
		//                            RWYList(iRwyNum).pPtGeo(PtStart) = RWYList(iRwyNum).pPtGeo(PtTHR)
		//                        End If
		//                        RWYList(iRwyNum).pPtPrj(PtStart) = ToPrj(RWYList(iRwyNum).pPtGeo(PtStart))
		//                        //==============================================================================================================
		//                        PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(PtTHR).X, RWYList(iRwyNum).pPtGeo(PtTHR).Y, fLDA, RWYList(iRwyNum).TrueBearing, ResX, ResY)
		//                        RWYList(iRwyNum).pPtGeo(ptEnd) = New Point
		//                        RWYList(iRwyNum).pPtGeo(ptEnd).PutCoords(ResX, ResY)

		//                        pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(ptEnd))
		//                        If Not (pRunwayCenterLinePoint Is Nothing) Then
		//                            pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList
		//                            pGMLPoint = pElevatedPointList.GetItem(0)
		//                            pElevPoint = pGMLPoint

		//                            RWYList(iRwyNum).pPtGeo(ptEnd) = GmlPointToIPoint(pGMLPoint)
		//                            RWYList(iRwyNum).pPtPrj(ptEnd) = ToPrj(CLPointArray(I).pPtGeo)
		//                            RWYList(iRwyNum).pPtGeo(ptEnd).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)

		//                        Else
		//                            pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList(iRwyNum).pPtGeo(ptEnd), 10000.0#)
		//                            If Not (pRunwayCenterLinePoint Is Nothing) Then
		//                                pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList
		//                                pElevPoint = pElevatedPointList.GetItem(0)
		//                                RWYList(iRwyNum).pPtGeo(ptEnd).Z = pConverter.ConvertToSI(pElevPoint.elevation, Owner.Elev)
		//                            Else
		//                                RWYList(iRwyNum).pPtGeo(ptEnd).Z = Owner.Elev
		//                            End If


		//                        End If

		//                        RWYList(iRwyNum).ClearWay = fTODA - fTORA
		//                        If RWYList(iRwyNum).ClearWay < 0.0# Then RWYList(iRwyNum).ClearWay = 0.0#

		//                        If RWYList(iRwyNum).ClearWay > 0.0# Then
		//                            PointAlongGeodesic(RWYList(iRwyNum).pPtGeo(ptEnd).X, RWYList(iRwyNum).pPtGeo(ptEnd).Y, RWYList(iRwyNum).ClearWay, RWYList(iRwyNum).TrueBearing, ResX, ResY)
		//                            RWYList(iRwyNum).pPtGeo(ptEnd).PutCoords(ResX, ResY)

		//                        End If

		//                        RWYList(iRwyNum).pPtPrj(ptEnd) = ToPrj(RWYList(iRwyNum).pPtGeo(ptEnd))


		//                        //==============================================================================================================
		//                        //RWYList(iRwyNum).pPtGeo (PtTHR)

		//                        For K = PtStart To ptEnd
		//                            If RWYList(iRwyNum).pPtGeo(K) Is Nothing Then
		//                                iRwyNum = iRwyNum - 1
		//                                GoTo NextI
		//                            End If
		//                        Next K

		//                        RWYList(iRwyNum).ID = pRwyDirection.ID
		//                        RWYList(iRwyNum).Name = pRwyDirection.Designator
		//                        RWYList(iRwyNum).ADHP_ID = Owner.ID
		//                        RWYList(iRwyNum).ILSID = pRwyDirection.ILS_ID

		//                        pRwyDirectinPair = pRwyDRList.GetItem((J + 1) Mod 2)
		//                        RWYList(iRwyNum).PairID = pRwyDirectinPair.ID
		//                        RWYList(iRwyNum).PairName = pRwyDirectinPair.Designator

		//                        //                RWYList(iRwyNum).ClearWay = 0#
		//                        //                Set pRunwayProtectAreaList = pObjectDir.GetRunwayProtectAreaList(pRwyDirection.ID)
		//                        //                For K = 0 To pRunwayProtectAreaList.Count - 1
		//                        //                    Set pRunwayProtectArea = pRunwayProtectAreaList.GetItem(K)
		//                        //                    Set pAirportHeliportProtectionArea = pRunwayProtectArea
		//                        //                    If (pRunwayProtectArea.Type = RunwayProtectionAreaType_CWY) And (Not pAirportHeliportProtectionArea.Length Is Nothing) Then
		//                        //                        RWYList(iRwyNum).ClearWay = pConverter.ConvertToSI(pAirportHeliportProtectionArea.Length, 0#)
		//                        //                        Exit For
		//                        //                    End If
		//                        //                Next K
		//                        //
		//                        //                If RWYList(iRwyNum).ClearWay > 0# Then
		//                        //                    PointAlongGeodesic RWYList(iRwyNum).pPtGeo(PtEnd).X, RWYList(iRwyNum).pPtGeo(PtEnd).Y, RWYList(iRwyNum).ClearWay, RWYList(iRwyNum).TrueBearing, ResX, ResY
		//                        //                    RWYList(iRwyNum).pPtGeo(PtEnd).PutCoords ResX, ResY

		//                        //                End If

		//                        For K = PtStart To ptEnd
		//                            //Set RWYList(iRwyNum).pPtPrj(K) = ToPrj(RWYList(iRwyNum).pPtGeo(K))


		//                            If RWYList(iRwyNum).pPtPrj(K).IsEmpty Then
		//                                iRwyNum = iRwyNum - 1
		//                                GoTo NextI
		//                            End If

		//                            RWYList(iRwyNum).pPtGeo(K).M = RWYList(iRwyNum).TrueBearing
		//                            RWYList(iRwyNum).pPtPrj(K).M = Azt2Dir(RWYList(iRwyNum).pPtGeo(K), RWYList(iRwyNum).TrueBearing)
		//                        Next K
		//NextI:
		//                    Next J
		//                End If
		//            Next I

		//            FillRWYList = iRwyNum

		//            If iRwyNum >= 0 Then
		//                ReDim Preserve RWYList(0 To iRwyNum)
		//            Else
		//        ReDim RWYList(-1 To -1)
		//            End If
		//        End Function

		//        Function AddILSToNavList(ILS As ILSType, NavaidList() As NavaidType) As Long
		//            Dim I As Long
		//            Dim N As Long
		//            Dim bFound As Boolean

		//            AddILSToNavList = -1
		//            N = UBound(NavaidList)
		//            bFound = False
		//            For I = 0 To N
		//                bFound = (NavaidList(I).TypeCode = CodeLLZ) And (NavaidList(I).CallSign = ILS.CallSign)
		//                If bFound Then
		//                    AddILSToNavList = I
		//                    Exit Function
		//                End If
		//            Next I

		//            N = N + 1
		//            ReDim Preserve NavaidList(0 To N)

		//            NavaidList(N).pPtGeo = ILS.pPtGeo
		//            NavaidList(N).pPtPrj = ILS.pPtPrj
		//            NavaidList(N).Name = ILS.CallSign
		//            NavaidList(N).ID = ILS.ID
		//            NavaidList(N).CallSign = ILS.CallSign

		//            NavaidList(N).MagVar = ILS.MagVar
		//            NavaidList(N).TypeName = "LLZ"
		//            NavaidList(N).TypeCode = CodeLLZ
		//            NavaidList(N).Range = 40000.0#
		//            NavaidList(N).Index = ILS.Index
		//            NavaidList(N).PairNavaidIndex = -1

		//            NavaidList(N).GPAngle = ILS.GPAngle
		//            NavaidList(N).GP_RDH = ILS.GP_RDH

		//            NavaidList(N).Course = ILS.Course
		//            NavaidList(N).LLZ_THR = ILS.LLZ_THR
		//            NavaidList(N).SecWidth = ILS.SecWidth
		//            NavaidList(N).AngleWidth = ILS.AngleWidth
		//            NavaidList(N).Tag = 0
		//            AddILSToNavList = N
		//        End Function

		//        Function GetRWYByProcedureName(ProcName As String, ADHP As ADHPType, RWY As RWYType) As Boolean
		//            Dim I As Long
		//            Dim N As Long
		//            Dim pos As Long

		//            Dim RWYList() As RWYType
		//            Dim bRWYFound As Boolean

		//            Dim RWYName As String
		//Dim Char As String

		//            GetRWYByProcedureName = False

		//            pos = InStr(1, ProcName, "RWY", 1)
		//            If pos <= 0 Then Exit Function

		//            pos = pos + 3
		//            RWYName = ""

		//    Char = Mid(ProcName, pos, 1)
		//    Do While Char <> " "
		//        RWYName = RWYName + Char
		//                pos = pos + 1
		//        Char = Mid(ProcName, pos, 1)
		//            Loop

		//            N = FillRWYList(RWYList, ADHP)

		//            bRWYFound = False
		//            For I = 0 To N
		//        Char = RWYList(I).Name
		//                bRWYFound = RWYList(I).Name = RWYName
		//                If bRWYFound Then
		//                    GetRWYByProcedureName = True
		//                    RWY = RWYList(I)
		//                    Exit For
		//                End If
		//            Next I
		//        End Function

		//        Function GetILSByName(ProcName As String, ADHP As ADHPType, ILS As ILSType) As Long
		//            Dim I As Long
		//            Dim N As Long
		//            Dim pos As Long

		//            Dim RWYList() As RWYType
		//            Dim RWY As RWYType
		//            Dim bRWYFound As Boolean

		//            Dim RWYName As String
		//Dim Char As String

		//            GetILSByName = 0

		//            pos = InStr(1, ProcName, "RWY", 1)
		//            If pos <= 0 Then Exit Function

		//            pos = pos + 3
		//            RWYName = ""

		//    Char = Mid(ProcName, pos, 1)
		//    Do While Char <> " "
		//        RWYName = RWYName + Char
		//                pos = pos + 1
		//        Char = Mid(ProcName, pos, 1)
		//            Loop

		//            //    ADHP.ID = OwnerID
		//            //    If FillADHPFields(ADHP) < 0 Then Exit Function

		//            N = FillRWYList(RWYList, ADHP)

		//            bRWYFound = False
		//            For I = 0 To N
		//        Char = RWYList(I).Name
		//                bRWYFound = RWYList(I).Name = RWYName
		//                If bRWYFound Then
		//                    RWY = RWYList(I)
		//                    Exit For
		//                End If
		//            Next I

		//            If Not bRWYFound Then Exit Function

		//            GetILSByName = GetILS(RWY, ILS, ADHP)
		//            //	If ILS.CallSign <> ILSCallSign Then GetILSByName = -1
		//        End Function

		//        Public Function GetILS(RWY As RWYType, ILS As ILSType, Owner As ADHPType) As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long
		//            Dim pAIXMNavaid As INavaid
		//            Dim pAIXMNAVEqList As INavaidEquipmentList
		//            Dim pAIXMNAVEq As INavaidEquipment
		//            Dim pAIXMLocalizer As ILocalizer
		//            Dim pAIXMGlidepath As IGlidepath
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pElevPoint As IElevatedPoint
		//            Dim pConverter As IAixmConvertType
		//            Dim dX As Double
		//            Dim dY As Double
		//            Dim fTmp As Double
		//            Dim ID As String
		//            Dim CallSign As String

		//            ILS.Index = 0
		//            GetILS = 0
		//            If RWY.ILSID = "" Then Exit Function

		//            pAIXMNavaid = pObjectDir.GetILSNavaid(RWY.ILSID)
		//            pAIXMNAVEqList = pAIXMNavaid.NavaidEquipmentList
		//            If pAIXMNAVEqList.Count = 0 Then Exit Function

		//            ILS.Category = pAIXMNavaid.LandingCategory + 1
		//            If ILS.Category > 3 Then ILS.Category = 3
		//            ILS.RWY_ID = RWY.ID

		//            pConverter = New AixmConvertType

		//            J = 0
		//            For I = 0 To pAIXMNAVEqList.Count - 1
		//                pAIXMNAVEq = pAIXMNAVEqList.GetItem(I)
		//                If (TypeOf pAIXMNAVEq Is ILocalizer) Then
		//                    J = J Or 1
		//                    pAIXMLocalizer = pAIXMNAVEq
		//                ElseIf (TypeOf pAIXMNAVEq Is IGlidepath) Then
		//                    pAIXMGlidepath = pAIXMNAVEq
		//                    J = J Or 2
		//                End If
		//                If J = 3 Then Exit For
		//            Next I

		//            If Not pAIXMLocalizer Is Nothing Then
		//                pAIXMNAVEq = pAIXMLocalizer
		//                pElevPoint = pAIXMNAVEq.ElevatedPoint

		//                If Not pAIXMNAVEq.MagneticVariation Is Nothing Then
		//                    ILS.MagVar = pAIXMNAVEq.MagneticVariation.Value
		//                Else
		//                    ILS.MagVar = Owner.MagVar
		//                End If

		//                If Not pAIXMLocalizer.TrueBearing Is Nothing Then
		//                    ILS.Course = pAIXMLocalizer.TrueBearing.Value
		//                ElseIf Not pAIXMLocalizer.MagneticBearing Is Nothing Then
		//                    ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar
		//                Else
		//                    Exit Function
		//                End If

		//                pGMLPoint = pElevPoint
		//                //Set ILS.pPtGeo = pGMLPoint.Tag

		//                ILS.pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                ILS.pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.elevation, RWY.pPtGeo(PtTHR).Z)
		//                ILS.pPtGeo.M = ILS.Course

		//                ILS.pPtPrj = ToPrj(ILS.pPtGeo)
		//                If ILS.pPtPrj.IsEmpty Then Exit Function
		//                ILS.pPtPrj.M = Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M)

		//                dX = RWY.pPtPrj(PtTHR).X - ILS.pPtPrj.X
		//                dY = RWY.pPtPrj(PtTHR).Y - ILS.pPtPrj.Y
		//                ILS.LLZ_THR = Sqr(dX * dX + dY * dY)

		//                If Not pAIXMLocalizer.WidthCourse Is Nothing Then
		//                    ILS.AngleWidth = pAIXMLocalizer.WidthCourse.Value
		//                    ILS.SecWidth = 2.0# * ILS.LLZ_THR * Tan(DegToRad(0.5 * ILS.AngleWidth))
		//                Else
		//                    GoTo ExitLocalizer
		//                End If

		//                ILS.Index = ILS.Index Or 1
		//                ILS.ID = pAIXMNAVEq.ID
		//                ILS.CallSign = pAIXMNAVEq.Designator
		//                //        ID = pAIXMNAVEq.ID
		//                //        CallSign = pAIXMNAVEq.Designator

		//                //    Else
		//                //        Exit Function
		//            End If
		//ExitLocalizer:

		//            If Not pAIXMGlidepath Is Nothing Then
		//                pAIXMNAVEq = pAIXMGlidepath

		//                If Not pAIXMGlidepath.Slope Is Nothing Then
		//                    ILS.GPAngle = pAIXMGlidepath.Slope.Value
		//                Else
		//                    GoTo ExitGlidepath
		//                End If

		//                If Not pAIXMGlidepath.RDH Is Nothing Then
		//                    fTmp = -9999
		//                    If Not (ILS.pPtGeo Is Nothing) Then
		//                        If Not ILS.pPtGeo.IsEmpty Then fTmp = ILS.pPtGeo.Z
		//                    End If

		//                    If fTmp = -9999 Then
		//                        fTmp = Owner.Elev
		//                    End If

		//                    ILS.GP_RDH = pConverter.ConvertToSI(pAIXMGlidepath.RDH, fTmp)
		//                Else
		//                    GoTo ExitGlidepath
		//                End If

		//                ID = pAIXMNAVEq.ID
		//                CallSign = pAIXMNAVEq.Designator
		//                //        ILS.ID = pAIXMNAVEq.ID
		//                //        ILS.CallSign = pAIXMNAVEq.Designator
		//                ILS.Index = ILS.Index Or 2
		//            End If
		//ExitGlidepath:

		//            If ILS.Index = 2 Then
		//                ILS.ID = ID
		//                ILS.CallSign = CallSign
		//            End If
		//            GetILS = ILS.Index
		//        End Function

		//        Public Function FillNavaidList(NavaidList() As NavaidType, ByRef DMEList() As NavaidType, CurrADHP As ADHPType, Radius As Double) As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim NavTypeCode As Long
		//            Dim NavTypeName As String
		//            Dim iNavaidNum As Long
		//            Dim iDMENum As Long
		//            Dim fDist As Double

		//            Dim NavaidDataList As INavaidEquipmentList
		//            //    Dim ILSDataList         As INavaidEquipmentList
		//            Dim AIXMNavaid As INavaid
		//            Dim AixmNavaidEquipment As INavaidEquipment
		//            //    Dim pAIXMLocalizer      As ILocalizer
		//            //    Dim pAIXMGlidepath      As IGlidepath

		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pElevPoint As IElevatedPoint
		//            Dim pConverter As IAixmConvertType
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint
		//            Dim CurrDate As Double //Date
		//            Dim pContour As Isogons.Contour

		//            pGMLPoly = CreateGMLCircle(CurrADHP.pPtGeo, Radius)

		//            NavaidDataList = pObjectDir.GetNavaidEquipmentList(pGMLPoly)
		//            //    Set ILSDataList = pObjectDir.GetILSNavaidEquipmentList(CurrADHP.ID)

		//            If (NavaidDataList.Count = 0) Then	   //And (ILSDataList.Count = 0)
		//        ReDim NavaidList(-1 To -1)
		//                Exit Function
		//            End If

		//            pConverter = New AixmConvertType
		//            pContour = New Isogons.Contour

		//            CurrDate = Year(Now) + (Month(Now) - 1 + Day(Now) / 30) / 12

		//            ReDim NavaidList(0 To NavaidDataList.Count - 1)	//+ ILSDataList.Count
		//            ReDim DMEList(0 To NavaidDataList.Count - 1)

		//            iNavaidNum = -1
		//            iDMENum = -1

		//            For I = 0 To NavaidDataList.Count - 1
		//                AixmNavaidEquipment = NavaidDataList.GetItem(I)

		//                If (TypeOf AixmNavaidEquipment Is IVOR) Then
		//                    NavTypeCode = CodeVOR
		//                    NavTypeName = "VOR"
		//                ElseIf (TypeOf AixmNavaidEquipment Is IDME) Then
		//                    NavTypeCode = CodeDME
		//                    NavTypeName = "DME"
		//                ElseIf (TypeOf AixmNavaidEquipment Is INDB) Then
		//                    NavTypeCode = CodeNDB
		//                    NavTypeName = "NDB"
		//                ElseIf (TypeOf AixmNavaidEquipment Is ITACAN) Then
		//                    NavTypeCode = CodeTACAN
		//                    NavTypeName = "Tacan"
		//                Else
		//                    GoTo NextNav
		//                End If

		//                pElevPoint = AixmNavaidEquipment.ElevatedPoint
		//                pGMLPoint = pElevPoint

		//                //Set pPtGeo = pGMLPoint.Tag
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.elevation, CurrADHP.Elev)
		//                pPtPrj = ToPrj(pPtGeo)

		//                If pPtPrj.IsEmpty Then GoTo NextNav

		//                If NavTypeCode = CodeDME Then
		//                    iDMENum = iDMENum + 1

		//                    DMEList(iDMENum).pPtGeo = pPtGeo
		//                    DMEList(iDMENum).pPtPrj = pPtPrj

		//                    If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
		//                        DMEList(iDMENum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
		//                    Else
		//                        DMEList(iDMENum).MagVar = pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate) 'CurrADHP.MagVar
		//                    End If

		//                    DMEList(iDMENum).Range = 350000.0 //DME.Range
		//                    DMEList(iDMENum).PairNavaidIndex = -1

		//                    DMEList(iDMENum).Name = AixmNavaidEquipment.Name
		//                    DMEList(iDMENum).ID = AixmNavaidEquipment.ID
		//                    DMEList(iDMENum).CallSign = AixmNavaidEquipment.Designator

		//                    DMEList(iDMENum).TypeCode = NavTypeCode
		//                    DMEList(iDMENum).TypeName = NavTypeName
		//                    DMEList(iDMENum).Index = I
		//                Else
		//                    iNavaidNum = iNavaidNum + 1

		//                    NavaidList(iNavaidNum).pPtGeo = pPtGeo
		//                    NavaidList(iNavaidNum).pPtPrj = pPtPrj

		//                    If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
		//                        NavaidList(iNavaidNum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
		//                    Else
		//                        NavaidList(iNavaidNum).MagVar = pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate)  'CurrADHP.MagVar
		//                    End If

		//                    If NavTypeCode = CodeNDB Then
		//                        NavaidList(iNavaidNum).Range = 350000.0; //NDB.Range
		//                    Else
		//                        NavaidList(iNavaidNum).Range = 350000.0; //VOR.Range
		//                    End If
		//                    NavaidList(iNavaidNum).PairNavaidIndex = -1

		//                    NavaidList(iNavaidNum).Name = AixmNavaidEquipment.Name
		//                    NavaidList(iNavaidNum).ID = AixmNavaidEquipment.ID
		//                    NavaidList(iNavaidNum).CallSign = AixmNavaidEquipment.Designator

		//                    NavaidList(iNavaidNum).TypeCode = NavTypeCode
		//                    NavaidList(iNavaidNum).TypeName = NavTypeName
		//                    NavaidList(iNavaidNum).Index = I
		//                End If
		//NextNav:
		//            Next I

		//            For J = 0 To iNavaidNum
		//                For I = 0 To iDMENum
		//                    fDist = ReturnDistanceInMeters(NavaidList(J).pPtPrj, DMEList(I).pPtPrj)
		//                    If (fDist <= 10.0#) And (LCase(NavaidList(J).CallSign) = LCase(DMEList(I).CallSign)) Then
		//                        NavaidList(J).PairNavaidIndex = I
		//                        DMEList(I).PairNavaidIndex = J
		//                        GoTo NextNavEq
		//                    End If
		//                Next I
		//NextNavEq:
		//            Next J

		//            //'    For I = 0 To ILSDataList.Count - 1
		//            //'        Set AixmNavaidEquipment = ILSDataList.GetItem(I)
		//            //'
		//            //'        If Not (TypeOf AixmNavaidEquipment Is ILocalizer) Then GoTo NextILS
		//            //'
		//            //'        Set pElevPoint = AixmNavaidEquipment.ElevatedPoint
		//            //'        Set pGMLPoint = pElevPoint
		//            //'        Set pAIXMLocalizer = AixmNavaidEquipment
		//            //'
		//            //'        If pAIXMLocalizer.TrueBearing Is Nothing Then GoTo NextILS
		//            //'
		//            //'        Set pPtGeo = GmlPointToIPoint(pGMLPoint)
		//            //'        pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.elevation, CurrADHP.Elev)
		//            //'        pPtGeo.M = pAIXMLocalizer.TrueBearing
		//            //'
		//            //'        Set pPtPrj = ToPrj(pPtGeo)
		//            //'        If pPtPrj.IsEmpty Then GoTo NextILS
		//            //'
		//            //'        pPtPrj.M = Azt2Dir(pPtGeo, pPtGeo.M)
		//            //''=====================================================
		//            //'
		//            //'        iNavaidNum = iNavaidNum + 1
		//            //'
		//            //'        Set NavaidList(iNavaidNum).pPtGeo = pPtGeo
		//            //'        Set NavaidList(iNavaidNum).pPtPrj = pPtPrj
		//            //'
		//            //'        NavaidList(iNavaidNum).Course = pPtGeo.M
		//            //''        dX = RWY.pPtPrj(ptTHR).X - pPtPrj.X
		//            //''        dY = RWY.pPtPrj(ptTHR).Y - pPtPrj.Y
		//            //''        NavaidList(iNavaidNum).LLZ_THR = Sqr(dX * dX + dY * dY)
		//            //'
		//            //'        If Not pAIXMLocalizer.WidthCourse Is Nothing Then
		//            //'            NavaidList(iNavaidNum).AngleWidth = pAIXMLocalizer.WidthCourse
		//            //''           NavaidList(iNavaidNum).SecWidth = NavaidList(iNavaidNum).LLZ_THR * Tan(DegToRad(NavaidList(iNavaidNum).AngleWidth))  'NavaidList(iNavaidNum).SecWidth = pAIXMLocalizer.WidthCourse
		//            //'        End If
		//            //''=====================================================
		//            //'
		//            //'        If (Not AixmNavaidEquipment.MagneticVariation Is Nothing) Then
		//            //'            NavaidList(iNavaidNum).MagVar = AixmNavaidEquipment.MagneticVariation.Value
		//            //'        Else
		//            //'            NavaidList(iNavaidNum).MagVar = CurrADHP.MagVar
		//            //'        End If
		//            //'
		//            //'        NavaidList(iNavaidNum).Name = AixmNavaidEquipment.Name
		//            //'        NavaidList(iNavaidNum).ID = AixmNavaidEquipment.ID
		//            //'        NavaidList(iNavaidNum).CallSign = AixmNavaidEquipment.Designator
		//            //'
		//            //'        NavaidList(iNavaidNum).Range = 35000#
		//            //'        NavaidList(iNavaidNum).TypeCode = CodeLLZ
		//            //'        NavaidList(iNavaidNum).TypeName = "ILS"
		//            //'
		//            //''    GP_RDH = 0
		//            //'
		//            //''    GPAngle = 0
		//            //'
		//            //'NextILS:
		//            //'    Next I

		//            If iNavaidNum > -1 Then
		//                ReDim Preserve NavaidList(0 To iNavaidNum)
		//            Else
		//        ReDim NavaidList(-1 To -1)
		//            End If

		//            If iDMENum > -1 Then
		//                ReDim Preserve DMEList(0 To iDMENum)
		//            Else
		//        ReDim DMEList(-1 To -1)
		//            End If
		//        End Function

		//        Public Function FillWPT_FIXList(WPTList() As WPT_FIXType, CurrADHP As ADHPType, Radius As Double) As Long
		//            Dim iWPTNum As Long
		//            Dim I As Long
		//            Dim N As Long
		//            Dim NavTypeCode As Long
		//            Dim NavTypeName As String

		//            Dim pConverter As IAixmConvertType
		//            Dim pName As IUnicalName

		//            Dim AIXMWPTList As IObjectList
		//            Dim AIXMNAVList As IObjectList
		//            Dim ElevatedPointList As IObjectList

		//            Dim AIXMWPT As IDesignatedPoint
		//            Dim AIXMNAVEq As INavaidEquipment

		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pElevPoint As IElevatedPoint
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint

		//            pGMLPoly = CreateGMLCircle(CurrADHP.pPtGeo, Radius)

		//            AIXMWPTList = pObjectDir.GetDesignatedPointList(pGMLPoly)
		//            AIXMNAVList = pObjectDir.GetNavaidEquipmentList(pGMLPoly)

		//            N = AIXMWPTList.Count + AIXMNAVList.Count - 1
		//            If N < 0 Then
		//        ReDim WPTList(-1 To -1)
		//                FillWPT_FIXList = -1
		//                Exit Function
		//            End If

		//            Dim CurrDate As Double //Date
		//            Dim pContour As Isogons.Contour

		//            pConverter = New AixmConvertType
		//            pContour = New Isogons.Contour

		//            CurrDate = Year(Now) + (Month(Now) - 1 + Day(Now) / 30) / 12

		//            iWPTNum = -1

		//            ReDim WPTList(0 To N)

		//            For I = 0 To AIXMWPTList.Count - 1
		//                AIXMWPT = AIXMWPTList.GetItem(I)
		//                pGMLPoint = AIXMWPT.Point

		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = CurrADHP.pPtGeo.Z + 300.0#
		//                pPtPrj = ToPrj(pPtGeo)

		//                If pPtPrj.IsEmpty Then GoTo NextWPT
		//                iWPTNum = iWPTNum + 1

		//                WPTList(iWPTNum).MagVar = pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate)

		//                //CurrADHP.MagVar

		//                WPTList(iWPTNum).pPtGeo = pPtGeo
		//                WPTList(iWPTNum).pPtPrj = pPtPrj

		//                WPTList(iWPTNum).Name = AIXMWPT.Designator
		//                WPTList(iWPTNum).ID = AIXMWPT.ID

		//                WPTList(iWPTNum).TypeName = "WPT"
		//                WPTList(iWPTNum).TypeCode = CodeNONE
		//NextWPT:
		//            Next I
		//            //======================================================================

		//            For I = 0 To AIXMNAVList.Count - 1
		//                AIXMNAVEq = AIXMNAVList.GetItem(I)

		//                pElevPoint = AIXMNAVEq.ElevatedPoint
		//                pGMLPoint = pElevPoint

		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.elevation, CurrADHP.Elev)

		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextNav

		//                If (TypeOf AIXMNAVEq Is IVOR) Then
		//                    NavTypeCode = CodeVOR
		//                    NavTypeName = "VOR"
		//                ElseIf (TypeOf AIXMNAVEq Is INDB) Then
		//                    NavTypeCode = CodeNDB
		//                    NavTypeName = "NDB"
		//                ElseIf (TypeOf AIXMNAVEq Is ITACAN) Then
		//                    NavTypeCode = CodeTACAN
		//                    NavTypeName = "TACAN"
		//                Else
		//                    GoTo NextNav
		//                End If

		//                iWPTNum = iWPTNum + 1

		//                WPTList(iWPTNum).pPtGeo = pPtGeo
		//                WPTList(iWPTNum).pPtPrj = pPtPrj

		//                If Not AIXMNAVEq.MagneticVariation Is Nothing Then
		//                    WPTList(iWPTNum).MagVar = AIXMNAVEq.MagneticVariation
		//                Else
		//                    WPTList(iWPTNum).MagVar = pContour.wmm(pPtGeo.X, pPtGeo.Y, pPtGeo.Z, CurrDate) 'CurrADHP.MagVar
		//                End If

		//                WPTList(iWPTNum).Name = AIXMNAVEq.Designator
		//                WPTList(iWPTNum).ID = AIXMNAVEq.ID

		//                WPTList(iWPTNum).TypeName = NavTypeName
		//                WPTList(iWPTNum).TypeCode = NavTypeCode
		//NextNav:
		//            Next I
		//            //======================================================================
		//            If iWPTNum < 0 Then
		//        ReDim WPTList(-1 To -1)
		//            Else
		//                ReDim Preserve WPTList(0 To iWPTNum)
		//            End If

		//            //    Set pContour = Nothing
		//            //    Set pConverter = Nothing

		//            FillWPT_FIXList = iWPTNum + 1
		//        End Function

		//        Public Function GetObstListInPoly(ObstList() As ObstacleAr, pPoly As Polygon) As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long
		//            Dim VerticalStructureList As IVerticalStructureList
		//            Dim AixmObstacle As IVerticalStructure
		//            Dim pElevatedPoint As IElevatedPoint
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pAixmPoint As IAixmPoint
		//            Dim pConverter As IAixmConvertType
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint

		//            GetObstListInPoly = -1

		//            pGMLPoly = ConvertToGMLPolyP(pPoly)
		//            VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly)
		//            N = VerticalStructureList.Count - 1

		//            If N < 0 Then
		//        ReDim ObstList(-1 To -1)
		//                Exit Function
		//            End If

		//            pConverter = New AixmConvertType
		//            ReDim ObstList(0 To N)
		//            J = -1

		//            For I = 0 To N
		//                AixmObstacle = VerticalStructureList.GetItem(I)
		//                pElevatedPoint = AixmObstacle.ElevatedPoint
		//                If pElevatedPoint.elevation Is Nothing Then GoTo NextI

		//                pGMLPoint = pElevatedPoint
		//                //Set pPtGeo = pGMLPoint.Tag
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.elevation, -9999.0#)

		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextI

		//                J = J + 1

		//                ObstList(J).pPtGeo = pPtGeo
		//                ObstList(J).pPtPrj = pPtPrj

		//                pAixmPoint = pElevatedPoint.AsIAixmPoint
		//                ObstList(J).HorAccuracy = pConverter.ConvertToSI(pAixmPoint.HorizontalAccuracy, 0.0#)
		//                ObstList(J).VertAccuracy = pConverter.ConvertToSI(pElevatedPoint.VerticalAccuracy, 0.0#)

		//                ObstList(J).Name = AixmObstacle.Name
		//                ObstList(J).ID = AixmObstacle.ID
		//                ObstList(J).Height = ObstList(J).pPtGeo.Z
		//                ObstList(J).Index = I
		//NextI:
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve ObstList(0 To J)
		//                GetObstListInPoly = J
		//            Else
		//        ReDim ObstList(-1 To -1)
		//            End If
		//        End Function

		//        Public Function GetArObstaclesByPoly(ObstacleList() As ObstacleAr, pPoly As IPolygon, fRefHeight As Double) As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long
		//            Dim VerticalStructureList As IVerticalStructureList
		//            Dim AixmObstacle As IVerticalStructure
		//            Dim pElevatedPoint As IElevatedPoint
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pConverter As IAixmConvertType
		//            //Dim pProxiOperator          As IProximityOperator
		//            Dim pAixmPoint As IAixmPoint
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint

		//            GetArObstaclesByPoly = -1

		//            pGMLPoly = ConvertToGMLPolyP(pPoly)
		//            VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly)
		//            N = VerticalStructureList.Count - 1

		//            If N < 0 Then
		//        ReDim ObstacleList(-1 To -1)
		//                Exit Function
		//            End If

		//            pConverter = New AixmConvertType
		//            ReDim ObstacleList(0 To N)
		//            //Set pProxiOperator = CurrADHP.pPtPrj
		//            J = -1

		//            For I = 0 To N
		//                AixmObstacle = VerticalStructureList.GetItem(I)
		//                pElevatedPoint = AixmObstacle.ElevatedPoint

		//                If pElevatedPoint.elevation Is Nothing Then GoTo NextI

		//                pGMLPoint = pElevatedPoint
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.elevation, -9999.0#)

		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextI

		//                J = J + 1
		//                ObstacleList(J).pPtGeo = pPtGeo
		//                ObstacleList(J).pPtPrj = pPtPrj
		//                ObstacleList(J).Name = AixmObstacle.Name
		//                ObstacleList(J).ID = AixmObstacle.ID

		//                pAixmPoint = pElevatedPoint.AsIAixmPoint
		//                ObstacleList(J).HorAccuracy = pConverter.ConvertToSI(pAixmPoint.HorizontalAccuracy, 0.0#)
		//                ObstacleList(J).VertAccuracy = pConverter.ConvertToSI(pElevatedPoint.VerticalAccuracy, 0.0#)

		//                ObstacleList(J).Height = ObstacleList(J).pPtGeo.Z - fRefHeight
		//                //ObstacleList(J).Dist = pProxiOperator.ReturnDistance(ObstacleList(J).pPtPrj)
		//                ObstacleList(J).Index = I
		//NextI:
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve ObstacleList(0 To J)
		//                GetArObstaclesByPoly = J
		//                Sort(ObstacleList, 0)
		//            Else
		//        ReDim ObstacleList(-1 To -1)
		//            End If
		//        End Function

		//        Public Function GetArObstacles(ObstacleList() As ObstacleAr, CurrADHP As ADHPType, MaxDist As Double, fRefHeight As Double) As Long
		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long
		//            Dim VerticalStructureList As IVerticalStructureList
		//            Dim AixmObstacle As IVerticalStructure
		//            Dim pElevatedPoint As IElevatedPoint
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pConverter As IAixmConvertType
		//            Dim pProxiOperator As IProximityOperator
		//            Dim pAixmPoint As IAixmPoint
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint

		//            GetArObstacles = -1

		//            pGMLPoly = CreateGMLCircle(CurrADHP.pPtGeo, MaxDist)
		//            VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly)
		//            N = VerticalStructureList.Count - 1

		//            If N < 0 Then
		//        ReDim ObstacleList(-1 To -1)
		//                Exit Function
		//            End If

		//            pConverter = New AixmConvertType
		//            ReDim ObstacleList(0 To N)
		//            pProxiOperator = CurrADHP.pPtPrj
		//            J = -1

		//            For I = 0 To N
		//                AixmObstacle = VerticalStructureList.GetItem(I)
		//                pElevatedPoint = AixmObstacle.ElevatedPoint
		//                If pElevatedPoint.elevation Is Nothing Then GoTo NextI

		//                pGMLPoint = pElevatedPoint
		//                'Set pPtGeo = pGMLPoint.Tag
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.elevation, -9999.0#)

		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextI

		//                J = J + 1
		//                ObstacleList(J).pPtGeo = pPtGeo
		//                ObstacleList(J).pPtPrj = pPtPrj
		//                ObstacleList(J).Name = AixmObstacle.Name
		//                ObstacleList(J).ID = AixmObstacle.ID

		//                pAixmPoint = pElevatedPoint.AsIAixmPoint
		//                ObstacleList(J).HorAccuracy = pConverter.ConvertToSI(pAixmPoint.HorizontalAccuracy, 0.0#)
		//                ObstacleList(J).VertAccuracy = pConverter.ConvertToSI(pElevatedPoint.VerticalAccuracy, 0.0#)

		//                ObstacleList(J).Height = ObstacleList(J).pPtGeo.Z - fRefHeight
		//                ObstacleList(J).Dist = pProxiOperator.ReturnDistance(ObstacleList(J).pPtPrj)
		//                ObstacleList(J).Index = I
		//NextI:
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve ObstacleList(0 To J)
		//                GetArObstacles = J
		//                Sort(ObstacleList, 0)
		//            Else
		//        ReDim ObstacleList(-1 To -1)
		//            End If
		//        End Function

		//        Public Function FillSegObstList(LastPoint As StepDownFIX, _
		//         fRefHeight As Double, IAF_FullAreaPoly As Polygon, IAF_BaseAreaPoly As Polygon, _
		//         ObstList() As ObstacleAr) As Long

		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long

		//            Dim FIXHeight As Double
		//            Dim fAlpha As Double
		//            Dim fDist As Double
		//            Dim fDir As Double
		//            Dim fTmp As Double
		//            Dim fL As Double

		//            Dim pBaseProxi As IProximityOperator
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint
		//            Dim GuidNav As NavaidType

		//            Dim ObsR As Double
		//            Dim ObsMOC As Double
		//            Dim ObsDist As Double
		//            Dim ObsCLDist As Double
		//            Dim ObsHeight As Double

		//            Dim VerticalStructureList As IVerticalStructureList
		//            Dim AixmObstacle As IVerticalStructure
		//            Dim pElevatedPoint As IElevatedPoint
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pConverter As IAixmConvertType

		//            FillSegObstList = -1
		//    ReDim ObstList(-1 To -1)

		//            pGMLPoly = ConvertToGMLPolyP(IAF_FullAreaPoly)
		//            VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly)
		//            N = VerticalStructureList.Count - 1

		//            If N < 0 Then Exit Function

		//            pConverter = New AixmConvertType
		//            ReDim ObstList(0 To N)

		//            pBaseProxi = IAF_BaseAreaPoly

		//            FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		//            fDir = LastPoint.InDir
		//            GuidNav = LastPoint.GuidNav

		//            fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		//            fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)
		//            J = -1
		//            For I = 0 To N
		//                AixmObstacle = VerticalStructureList.GetItem(I)
		//                pElevatedPoint = AixmObstacle.ElevatedPoint
		//                If pElevatedPoint.elevation Is Nothing Then GoTo NextI

		//                pGMLPoint = pElevatedPoint

		//                'Set pPtGeo = pGMLPoint.Tag
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.elevation, -9999.0#)
		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextI
		//                J = J + 1

		//                ObstList(J).pPtGeo = pPtGeo
		//                ObstList(J).pPtPrj = pPtPrj

		//                fDist = pBaseProxi.ReturnDistance(ObstList(J).pPtPrj)

		//                ObstList(J).Height = ObstList(J).pPtGeo.Z - fRefHeight
		//                ObstList(J).Flags = -CInt(fDist = 0.0#)
		//                ObsMOC = arIASegmentMOC.Value * (1.0# - 2 * fDist / arIFHalfWidth.Value)

		//                If FIXHeight <= ObstList(J).Height Then
		//                    ObsR = arIFHalfWidth.Value
		//                Else
		//                    ObsR = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObstList(J).Height) / arIASegmentMOC.Value 'MOC '
		//                End If

		//                If ObsR > arIFHalfWidth.Value Then ObsR = arIFHalfWidth.Value

		//                If GuidNav.TypeCode <> 1 Then
		//                    ObstList(J).Dist = Point2LineDistancePrj(ObstList(J).pPtPrj, LastPoint.pPtPrj, fDir + 90.0#)
		//                    ObstList(J).CLDist = Point2LineDistancePrj(ObstList(J).pPtPrj, LastPoint.pPtPrj, fDir)
		//                Else
		//                    ObstList(J).Dist = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ObstList(J).pPtPrj))) * fL
		//                    ObstList(J).CLDist = Abs(fL - ReturnDistanceInMeters(ObstList(J).pPtPrj, GuidNav.pPtPrj))
		//                End If

		//                ObstList(J).MOC = ObsMOC
		//                ObstList(J).ReqH = ObstList(J).Height + ObsMOC
		//                ObstList(J).hPent = ObstList(J).ReqH - FIXHeight
		//                ObstList(J).fTmp = ObstList(J).hPent / ObstList(J).Dist

		//                ObstList(J).Name = AixmObstacle.Name
		//                ObstList(J).ID = AixmObstacle.ID
		//                ObstList(J).Index = I
		//NextI:
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve ObstList(0 To J)
		//                FillSegObstList = J
		//            Else
		//        ReDim ObstList(-1 To -1)
		//            End If
		//        End Function

		//        Public Function CalcIFProhibitions(LastPoint As StepDownFIX, _
		//         fRefHeight As Double, IAF_FullAreaPoly As Polygon, IAF_BaseAreaPoly As Polygon, _
		//         ObstList() As ObstacleAr, ProhibitionSectors() As IFProhibitionSector) As Long

		//            Dim I As Long
		//            Dim J As Long
		//            Dim N As Long

		//            Dim FIXHeight As Double
		//            Dim fAlpha As Double

		//            Dim fDist As Double
		//            Dim fDir As Double
		//            Dim fTmp As Double

		//            Dim fL As Double

		//            Dim pBaseProxi As IProximityOperator
		//            Dim GuidNav As NavaidType
		//            Dim pPtGeo As IPoint
		//            Dim pPtPrj As IPoint

		//            Dim ObsR As Double
		//            Dim ObsMOC As Double
		//            Dim ObsDist As Double
		//            Dim ObsCLDist As Double
		//            Dim ObsHeight As Double

		//            Dim VerticalStructureList As IVerticalStructureList
		//            Dim AixmObstacle As IVerticalStructure
		//            Dim pElevatedPoint As IElevatedPoint
		//            Dim pGMLPoint As IGMLPoint
		//            Dim pGMLPoly As IGMLPolygon
		//            Dim pConverter As IAixmConvertType

		//            CalcIFProhibitions = -1
		//    ReDim ObstList(-1 To -1)
		//    ReDim ProhibitionSectors(-1 To -1)

		//            pGMLPoly = ConvertToGMLPolyP(IAF_FullAreaPoly)
		//            VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly)
		//            N = VerticalStructureList.Count - 1

		//            If N < 0 Then Exit Function

		//            pConverter = New AixmConvertType
		//            ReDim ObstList(0 To N)
		//            ReDim ProhibitionSectors(0 To N)

		//            pBaseProxi = IAF_BaseAreaPoly

		//            FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		//            fDir = LastPoint.InDir
		//            GuidNav = LastPoint.GuidNav

		//            fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		//            fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)

		//            J = -1

		//            For I = 0 To N
		//                AixmObstacle = VerticalStructureList.GetItem(I)
		//                pElevatedPoint = AixmObstacle.ElevatedPoint
		//                If pElevatedPoint.elevation Is Nothing Then GoTo NextI

		//                pGMLPoint = pElevatedPoint

		//                'Set pPtGeo = pGMLPoint.Tag
		//                pPtGeo = GmlPointToIPoint(pGMLPoint)
		//                pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.elevation, -9999.0#)
		//                pPtPrj = ToPrj(pPtGeo)
		//                If pPtPrj.IsEmpty Then GoTo NextI

		//                fDist = pBaseProxi.ReturnDistance(pPtPrj)
		//                ObsMOC = arIASegmentMOC.Value * (1.0# - 2.0# * fDist / arIFHalfWidth.Value)
		//                ObsHeight = pPtGeo.Z - fRefHeight

		//                If FIXHeight - ObsHeight >= ObsMOC Then GoTo NextI

		//                If FIXHeight <= ObsHeight Then
		//                    ObsR = arIFHalfWidth.Value
		//                Else
		//                    ObsR = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObsHeight) / arIASegmentMOC.Value	'MOC '
		//                End If

		//                If ObsR > arIFHalfWidth.Value Then ObsR = arIFHalfWidth.Value

		//                If GuidNav.TypeCode <> 1 Then
		//                    ObsDist = Point2LineDistancePrj(pPtPrj, LastPoint.pPtPrj, fDir + 90.0#)
		//                    ObsCLDist = Point2LineDistancePrj(pPtPrj, LastPoint.pPtPrj, fDir)
		//                Else
		//                    ObsDist = DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, pPtPrj))) * fL
		//                    ObsCLDist = Abs(fL - ReturnDistanceInMeters(pPtPrj, GuidNav.pPtPrj))
		//                End If

		//                If (ObsR > 0.5 * arIFHalfWidth.Value) And (ObsCLDist <= ObsR) Then
		//                    J = J + 1
		//                    ObstList(J).pPtGeo = pPtGeo
		//                    ObstList(J).pPtPrj = pPtPrj

		//                    ObstList(J).Dist = ObsDist
		//                    ObstList(J).CLDist = ObsCLDist
		//                    ObstList(J).Height = ObsHeight
		//                    ObstList(J).Flags = -CInt(fDist = 0.0#)

		//                    ObstList(J).Name = AixmObstacle.Name
		//                    ObstList(J).ID = AixmObstacle.ID

		//                    ObstList(J).MOC = ObsMOC
		//                    ObstList(J).ReqH = ObsHeight + ObsMOC
		//                    ObstList(J).hPent = ObstList(J).ReqH - FIXHeight
		//                    ObstList(J).fTmp = ObstList(J).hPent / ObsDist
		//                    ObstList(J).Index = J

		//                    ProhibitionSectors(J).MOC = ObsMOC
		//                    ProhibitionSectors(J).rObs = ObsR
		//                    ProhibitionSectors(J).ObsArea = CreatePrjCircle(pPtPrj, ObsR)

		//                    ProhibitionSectors(J).DirToObst = ReturnAngleInDegrees(LastPoint.pPtPrj, pPtPrj)
		//                    ProhibitionSectors(J).DistToObst = ReturnDistanceInMeters(LastPoint.pPtPrj, pPtPrj)

		//                    '            If ObsR < ProhibitionSectors(J).DistToObst Then
		//                    '                fTmp = ArcSin(rObs / ProhibitionSectors(J).DistToObst)
		//                    '                ProhibitionSectors(J).AlphaFrom = Round(Modulus(ProhibitionSectors(J).DirToObst - RadToDeg(fTmp), 360#) - 0.4999)
		//                    '                ProhibitionSectors(J).AlphaTo = Round(Modulus(ProhibitionSectors(J).DirToObst + RadToDeg(fTmp), 360#) + 0.4999)
		//                    '            Else
		//                    '                ProhibitionSectors(J).AlphaFrom = 0
		//                    '                ProhibitionSectors(J).AlphaTo = 360
		//                    '            End If

		//                    ProhibitionSectors(J).dHObst = ObsHeight - FIXHeight + ObsMOC
		//                    ProhibitionSectors(J).Index = I
		//                End If
		//NextI:
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve ObstList(0 To J)
		//                ReDim Preserve ProhibitionSectors(0 To J)
		//                CalcIFProhibitions = J
		//            Else
		//        ReDim ObstList(-1 To -1)
		//        ReDim ProhibitionSectors(-1 To -1)
		//            End If
		//        End Function

		//        Private Function CreateMSASectorPrj(ptCntGeo As IPoint, Sector As SectorMSA) As Polygon
		//            Dim I As Long
		//            Dim N As Long

		//            Dim AngleFrom As Double
		//            Dim AngleTo As Double
		//            Dim dAngle As Double
		//            Dim AngStep As Long

		//            Dim iInRad As Double
		//            Dim CosI As Double
		//            Dim SinI As Double

		//            Dim ptCntPrj As IPoint
		//            Dim ptInner As IPoint
		//            Dim ptOuter As IPoint

		//            Dim pPolygon As IPointCollection
		//            Dim pTopo As ITopologicalOperator2

		//            pPolygon = New Polygon
		//            ptCntPrj = ToPrj(ptCntGeo)
		//            ptInner = New Point
		//            ptOuter = New Point

		//            AngleTo = Azt2Dir(ptCntGeo, Sector.FromAngle)
		//            AngleFrom = Azt2Dir(ptCntGeo, Sector.ToAngle)

		//            dAngle = Modulus(AngleTo - AngleFrom, 360.0#)
		//            AngStep = 1

		//            N = Round(dAngle / AngStep)

		//            If (N < 1) Then
		//                N = 1
		//            ElseIf (N < 5) Then
		//                N = 5
		//            ElseIf (N < 10) Then
		//                N = 10
		//            End If

		//            AngStep = dAngle / N

		//            For I = 0 To N
		//                iInRad = DegToRad(AngleFrom + I * AngStep)
		//                CosI = Cos(iInRad)
		//                SinI = Sin(iInRad)

		//                ptInner.X = ptCntPrj.X + Sector.InnerDist * CosI
		//                ptInner.Y = ptCntPrj.Y + Sector.InnerDist * SinI

		//                ptOuter.X = ptCntPrj.X + Sector.OuterDist * CosI
		//                ptOuter.Y = ptCntPrj.Y + Sector.OuterDist * SinI

		//                pPolygon.AddPoint(ptInner)
		//                pPolygon.AddPoint(ptOuter, 0)
		//            Next I

		//            pTopo = pPolygon
		//            pTopo.IsKnownSimple = False
		//            pTopo.Simplify()

		//            CreateMSASectorPrj = pPolygon
		//        End Function

		//        Public Function GetMSASectors(Nav As NavaidType, MSAList() As MSAType) As Boolean
		//            Dim I As Long
		//            Dim J As Long
		//            Dim K As Long
		//            Dim N As Long
		//            Dim M As Long
		//            Dim fTmp As Double
		//            Dim FromAngle As Double
		//            Dim ToAngle As Double

		//            Dim ptCenter As IPoint

		//            Dim SAAList As ISafeAltitudeAreaList
		//            Dim SAASectorList As ISafeAltitudeAreaSectorList
		//            Dim SectorDefinition As ICircleSector

		//            Dim SafeAltitudeArea As ISafeAltitudeArea
		//            Dim SafeAltitudeSector As ISafeAltitudeAreaSector
		//            Dim pConverter As IAixmConvertType

		//            GetMSASectors = False

		//            SAAList = pObjectDir.GetSafeAltitudeAreaList(Nav.ID)
		//            N = SAAList.Count

		//            If N = 0 Then
		//        ReDim MSAList(-1 To -1)
		//                Exit Function
		//            End If

		//            ReDim MSAList(0 To N - 1)
		//            pConverter = New AixmConvertType
		//            J = -1
		//            For I = 0 To N - 1
		//                SafeAltitudeArea = SAAList.GetItem(I)

		//                If SafeAltitudeArea.SafeAreaType = SafeAltitudeType_MSA Then
		//                    SAASectorList = pObjectDir.GetSafeAltitudeAreaSectorList(SafeAltitudeArea.ID)
		//                    M = SAASectorList.Count
		//                    If M > 0 Then
		//                        J = J + 1
		//                        MSAList(J).Name = SafeAltitudeArea.ID
		//                        ReDim MSAList(J).Sectors(0 To M - 1)

		//                        'Set ptCenter = SafeAltitudeArea.CentrePoint.AsIGMLPoint.Tag
		//                        'Set ptCenter = GmlPointToIPoint(SafeAltitudeArea.CentrePoint.AsIGMLPoint)

		//                        For K = 0 To M - 1
		//                            SafeAltitudeSector = SAASectorList.GetItem(K)
		//                            SectorDefinition = SafeAltitudeSector.SectorDefinition

		//                            FromAngle = SectorDefinition.FromAngle
		//                            ToAngle = SectorDefinition.ToAngle

		//                            If SectorDefinition.AngleDirectionReference = DirectionReferenceType_TO Then
		//                                FromAngle = Modulus(FromAngle + 180.0#, 360)
		//                                ToAngle = Modulus(ToAngle + 180.0#, 360)
		//                            End If

		//                            If SectorDefinition.ArcDirection = ArcDirectionType_CCA Then
		//                                fTmp = FromAngle
		//                                FromAngle = ToAngle
		//                                ToAngle = fTmp
		//                            End If

		//                            If SectorDefinition.AngleType <> BearingType_TRUE Then

		//                            End If

		//                            MSAList(J).Sectors(K).LowerLimit = pConverter.ConvertToSI(SectorDefinition.LowerLimit, 0.0#)
		//                            MSAList(J).Sectors(K).UpperLimit = pConverter.ConvertToSI(SectorDefinition.UpperLimit, 0.0#)

		//                            MSAList(J).Sectors(K).InnerDist = pConverter.ConvertToSI(SectorDefinition.InnerDistance, 0.0#)
		//                            MSAList(J).Sectors(K).OuterDist = pConverter.ConvertToSI(SectorDefinition.OuterDistance, 19000.0#)
		//                            MSAList(J).Sectors(K).FromAngle = FromAngle
		//                            MSAList(J).Sectors(K).ToAngle = ToAngle
		//                            MSAList(J).Sectors(K).AbsAngle = Modulus(ToAngle - FromAngle, 360.0#)
		//                            'MSAList(J).Sectors(K).AbsAngle = SubtractAngles(FromAngle, ToAngle)
		//                            '                   Set MSAList(J).Sectors(K).Sector = CreateMSASectorPrj(ptCenter, MSAList(J).Sectors(K))
		//                        Next K
		//                    End If
		//                End If
		//            Next I

		//            If J >= 0 Then
		//                ReDim Preserve MSAList(0 To J)
		//                GetMSASectors = True
		//            Else
		//        ReDim MSAList(-1 To -1)
		//            End If

		//        End Function

		//        Public Function GetADHPWorkspace() As IWorkspace
		//            Dim I As Long
		//            Dim map As IMap
		//            Dim fL As IFeatureLayer
		//            Dim ds As IDataset

		//            map = GetMap()

		//            For I = 0 To map.LayerCount - 1
		//                If TypeOf map.Layer(I) Is IFeatureLayer Then
		//                    If map.Layer(I).Name = "ADHP" Then
		//                        fL = map.Layer(I)
		//                        ds = fL.FeatureClass
		//                        GetADHPWorkspace = ds.Workspace
		//                        Exit Function
		//                    End If
		//                End If
		//            Next I

		//            GetADHPWorkspace = Nothing
		//        End Function

		//        Public Function InitModule() As String
		//            Dim mapFileName As String
		//            pObjectDir = New ARANObjectDirectory.ARANObjectDirectory
		//            pObjectDir.map = pMap
		//            mapFileName = GetMapFileName()
		//            pObjectDir.ConnectUsingRNAVSettings(mapFileName)
		//            InitModule = pObjectDir.GetCurrentUserName
		//        End Function

		//        Sub Terminate()
		//            pObjectDir = Nothing
		//        End Sub

		//    }
	}
}