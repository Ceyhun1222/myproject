//3186801021

using System.Collections.Generic;
using System;


using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
//using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Aim.Enums;

using Aran.Converters;
using Aran.Geometries;
using Aran.Queries.Panda_2;
using Aran.Queries;

using Aran.Aim.CAWProvider;

//using ESRI.ArcGIS.ArcMapUI;
//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Geometry;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IPandaSpecializedQPI pObjectDir;

		//Delib.Specialized. DelibObjectDirectory
		//public pAixmGDBCom As DelibObjectDirectory.AixmGDBCom

		//public Function CreateGMLCircle(ptCenterGeo As IPoint, Radius As Double) As IPolygon
		//    Dim pGMLPoint               As IGMLPoint
		//    Dim pGMLRing                As IGMLRing
		//    Dim pGMLPoly                As IGMLPolygon
		//
		//    Set pGMLPoint = New gmlPoint
		//    pGMLPoint.X = ptCenterGeo.X
		//    pGMLPoint.Y = ptCenterGeo.Y
		//
		//    Set pGMLRing = New GMLRing
		//    Set pGMLPoly = New GMLPolygon
		//
		//    pGMLRing.Type = RingType_Circle
		//    Set pGMLRing.centrePoint = pGMLPoint
		//    pGMLRing.Radius = Radius
		//
		//    pGMLPoly.Add pGMLRing
		//
		//    Set CreateGMLCircle = pGMLPoly
		//End Function

		//public Function CreateGMLCircleP(ptCenterPRJ As IPoint, Radius As Double) As GMLPolygon
		//    Dim ptCenterGeo As IPoint
		//    Set ptCenterGeo = Functions.ToGeo(ptCenterPRJ)
		//    Set CreateGMLCircleP = CreateGMLCircle(ptCenterGeo, Radius)
		//End Function

		//public Function ConvertToGMLPoly(pPolyGEO As IPolygon) As GMLPolygon
		//    Dim I               As Long
		//    Dim J               As Long
		//    Dim N               As Long
		//    Dim M               As Long
		//    Dim dX              As Double
		//    Dim dY              As Double
		//
		//    Dim pPoly           As IPolygon
		//    Dim pTopoOp         As ITopologicalOperator2
		//    Dim pRing           As IRing
		//    Dim pPoint          As IPoint
		//    Dim pPointCol       As IPointCollection
		//    Dim pExteriorRing() As IRing
		//
		//    Dim pGMLPoint       As IGMLPoint
		//    Dim pGMLRing        As IGMLRing
		//    Dim pGMLPoly        As IGMLPolygon
		//
		//    Set ConvertToGMLPoly = Nothing
		//    if pPolyGEO is Nothing  Exit Function
		//
		//    Set pGMLPoly = New GMLPolygon
		//    Set ConvertToGMLPoly = pGMLPoly
		//
		//    N = pPolyGEO.ExteriorRingCount
		//    if N = 0  Exit Function
		//
		//    ReDim pExteriorRing(0 To N - 1)
		//
		//    pPolyGEO.QueryExteriorRings pExteriorRing(0)
		//
		//    For I = 0 To N - 1
		//        Set pGMLRing = New GMLRing
		//        pGMLRing.Type = RingType_PointSeq
		//
		//        Set pPointCol = pExteriorRing(I)
		//        M = pPointCol.PointCount
		//        dX = Abs(pPointCol.Point(0).X - pPointCol.Point(M - 1).X)
		//        dY = Abs(pPointCol.Point(0).Y - pPointCol.Point(M - 1).Y)
		//        if dX * dY < 0.001  M = M - 1
		//
		//        For J = 0 To M - 1
		//            Set pGMLPoint = New gmlPoint
		//            Set pPoint = pPointCol.Point(J)
		//            pGMLPoint.X = pPoint.X
		//            pGMLPoint.Y = pPoint.Y
		//            pGMLRing.Add pGMLPoint
		//        Next J
		//        pGMLPoly.Add pGMLRing
		//    Next
		//
		//    Set ConvertToGMLPoly = pGMLPoly
		//End Function

		//public Function ConvertToGMLPolyP(pPolyPRJ As IPolygon) As GMLPolygon
		//    Dim pPolyGEO As IPolygon
		//    Set pPolyGEO = Functions.ToGeo(pPolyPRJ)
		//    Set ConvertToGMLPolyP = ConvertToGMLPoly(pPolyGEO)
		//End Function

		//public Function GmlPointToIPoint(gmlPoint As IGMLPoint) As IPoint
		//    Dim Result As IPoint
		//    Set Result = New Point
		//    Result.PutCoords gmlPoint.X, gmlPoint.Y
		//    Set GmlPointToIPoint = Result
		//End Function

		public static ValDistance CreateValDistanceType(UomDistance uom, double value)
		{
			ValDistance res = new ValDistance();
			res.Uom = uom;
			res.Value = value;
			return res;
		}

		public static ValDistanceVertical CreateValAltitudeType(UomDistanceVertical uom, double value)
		{
			ValDistanceVertical res = new ValDistanceVertical();
			res.Uom = uom;
			res.Value = value;
			return res;
		}

		public static int FillADHPList(ref ADHPType[] ADHPList, System.Guid organ, bool CheckILS = false)
		{
			ADHPList = GlobalVars.InitArray<ADHPType>(0);

			List<Descriptor> pADHPNameList = pObjectDir.GetAirportHeliportList(organ, CheckILS);

			if (pADHPNameList == null)
				return -1;

			int n = pADHPNameList.Count;

			if (n == 0)
				return -1;

			System.Array.Resize(ref ADHPList, n);

			for (int i = 0; i < n; i++)
			{
				Descriptor pName = pADHPNameList[i];
				ADHPList[i].Name = pName.Name;
				ADHPList[i].ID = pName.Identifier;

				//if CheckILS 
				//	pAIXMILSList = pObjectDir.GetILSNavaidEquipmentList(pName.Identifer)
				//	T = 0
				//	if pAIXMILSList.Count > 0 
				//		For J = 0 To pAIXMILSList.Count - 1
				//			pAIXMNAVEq = pAIXMILSList(J)

				//			if (pAIXMNAVEq is Localizer) 
				//				T = T Or 1
				//			else if (pAIXMNAVEq is Glidepath) 
				//				T = T Or 2
				//			End if
				//			if T = 3  Exit For
				//		Next J
				//	End if

				//	ADHPList(I).Index = T
				//else
				//	ADHPList(I).Index = I
				//End if
				ADHPList[i].Index = i;
			}

			return pADHPNameList.Count - 1;
		}

		public static int FillADHPFields(ref ADHPType CurrADHP)
		{
			AirportHeliport pADHP;

			IRelationalOperator pRelational;
			IPoint pPtGeo;
			IPoint pPtPrj;

			if (CurrADHP.pPtGeo != null)
				return 0;

			pADHP = (AirportHeliport)pObjectDir.GetFeature(FeatureType.AirportHeliport, CurrADHP.ID);
			CurrADHP.pAirportHeliport = pADHP;
			if (pADHP == null)
				return -1;

			pPtGeo = ConvertToEsriGeom.FromPoint(pADHP.ARP.Geo);

			//    pPtGeo.X = pADHP.ARP.pos.DoubleList(0)
			//    pPtGeo.Y = pADHP.ARP.pos.DoubleList(1)

			//    if pADHP.ARP.pos.DoubleList.Count >= 3 
			//        pPtGeo.Z = pADHP.ARP.pos.DoubleList(2)
			//    else
			pPtGeo.Z = ConverterToSI.Convert(pADHP.FieldElevation, 0);
			//    End if

			pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
			if (pPtPrj.IsEmpty)
				return -1;

			pRelational = (IRelationalOperator)GlobalVars.p_LicenseRect;
			if (!pRelational.Contains(pPtPrj))
				return -1;

			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			if (pADHP.MagneticVariation == null)
				CurrADHP.MagVar = 0.0;
			else
				CurrADHP.MagVar = pADHP.MagneticVariation.Value;

			//	CurADHP.Elev = pConverter.ConvertToSI(pElevPoint.elevation, 0.0)
			//	CurADHP.pPtGeo.Z = CurADHP.Elev
			//	CurADHP.pPtPrj.Z = CurADHP.Elev
			//	CurADHP.MinTMA = pConverter.Convert(pADHP.transitionAltitude, 2500.0)
			//	CurADHP.TransitionAltitude = pConverter.ConvertToSI(ah.TransitionAltitude)

			CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);
			//	CurADHP.ReferenceTemperature = pConverter.ConvertToSI(ah.ReferenceTemperature)

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}

		public static int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
		{
			List<Descriptor> pAIXMRWYList;
			List<RunwayDirection> pRwyDRList;
			List<RunwayCentrelinePoint> pCenterLinePointList;
			//List<RunwayProtectArea> pRunwayProtectAreaList;
			Descriptor pName;
			ElevatedPoint pElevatedPoint;

			Runway pRunway;
			RunwayDirection pRwyDirection;
			RunwayDirection pRwyDirectinPair;
			//RunwayProtectArea pRunwayProtectArea;
			RunwayCentrelinePoint pRunwayCenterLinePoint;

			//AirportHeliportProtectionArea pAirportHeliportProtectionArea;
			IPoint ptDTreshGeo, ptDTreshPrj;
			RWYCLPoint[] CLPointArray;

			RunwayCentrelinePoint pDTreshCenterLinePoint;
			RunwayDeclaredDistance pDirDeclaredDist;
			List<RunwayDeclaredDistance> pDeclaredDistList;
			double fLDA, fTORA, fTODA, dDT;

			pAIXMRWYList = pObjectDir.GetRunwayList(Owner.ID);

			if (pAIXMRWYList.Count == 0)
			{
				RWYList = new RWYType[0];
				return -1;
			}

			RWYList = GlobalVars.InitArray<RWYType>(2 * pAIXMRWYList.Count);

			double ResX = 0.0, ResY = 0.0,
				fTmp = 0.0, TrueBearing = 0.0;
			int iRwyNum = -1;

			for (int i = 0; i < pAIXMRWYList.Count; i++)
			{
				pName = pAIXMRWYList[i];
				pRunway = (Runway)pObjectDir.GetFeature(FeatureType.Runway, pName.Identifier);
				pRwyDRList = pObjectDir.GetRunwayDirectionList(pRunway.Identifier);

				if (pRwyDRList.Count == 2)
				{
					for (int j = 0; j < 2; j++)
					{
						iRwyNum++;

						RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
						if (RWYList[iRwyNum].Length < 0.0)
						{
							iRwyNum--;
							break;
						}

						pRwyDirection = pRwyDRList[j];
						pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
						if (pCenterLinePointList.Count == 0)
						{
							iRwyNum--;
							break;
						}

						RWYList[iRwyNum].Initialize();
						RWYList[iRwyNum].pRunwayDir = pRwyDirection;
						RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = null;
						ptDTreshGeo = ptDTreshPrj = null;
						pDTreshCenterLinePoint = null;

						CLPointArray = new RWYCLPoint[pCenterLinePointList.Count];
						fLDA = fTORA = fTODA = 0.0;

						for (int k = 0; k < pCenterLinePointList.Count; k++)
						{
							pRunwayCenterLinePoint = pCenterLinePointList[k];
							pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

							for (int l = 0; l < pDeclaredDistList.Count; l++)
							{
								pDirDeclaredDist = pDeclaredDistList[l];
								if (pDirDeclaredDist.DeclaredValue.Count > 0)
								{
									if (pDirDeclaredDist.Type == Aran.Aim.Enums.CodeDeclaredDistance.LDA)
										fLDA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0], 0);
									else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TORA)
										fTORA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0], 0);
									else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TODA)
										fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0], 0);
								}
							}

							if (pRunwayCenterLinePoint.Role != null)
							{
								pElevatedPoint = pRunwayCenterLinePoint.Location;
								CLPointArray[k].pCLPoint = pRunwayCenterLinePoint;
								CLPointArray[k].pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
								CLPointArray[k].pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								CLPointArray[k].pPtPrj = Functions.ToPrj(CLPointArray[k].pPtGeo) as IPoint;

								switch (pRunwayCenterLinePoint.Role.Value)
								{
									case Aran.Aim.Enums.CodeRunwayPointRole.START:
										RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtStart] = CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtStart] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.THR:
										RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtTHR] = CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtTHR] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.END:
										RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtEnd] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.DISTHR:
										ptDTreshGeo = CLPointArray[k].pPtGeo;
										ptDTreshPrj = CLPointArray[k].pPtPrj;
										pDTreshCenterLinePoint = pRunwayCenterLinePoint;
										break;
								}
							}
						}

						if ((RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] == null) && (ptDTreshGeo != null))
						{
							RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = ptDTreshGeo;
							RWYList[iRwyNum].pPtPrj[eRWY.PtTHR] = ptDTreshPrj;
							RWYList[iRwyNum].pSignificantPoint[eRWY.PtTHR] = pDTreshCenterLinePoint;
						}

						for (eRWY k = eRWY.PtStart; k <= eRWY.PtEnd; k++)
						{
							if (RWYList[iRwyNum].pPtGeo[k] == null)
							{
								iRwyNum--;
								break;
							}
						}

						if (pRwyDirection.TrueBearing != null)
							RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
						else if (pRwyDirection.MagneticBearing != null)
							RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value - Owner.MagVar;
						else
						{
							NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, ref TrueBearing, ref fTmp);
							RWYList[iRwyNum].TrueBearing = TrueBearing;
						}

						//==============================================================================================================
						dDT = fTORA - fLDA;
						if (dDT > 0.0)
						{
							NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, dDT, RWYList[iRwyNum].TrueBearing + 180.0, ref ResX, ref ResY);

							RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = new ESRI.ArcGIS.Geometry.Point();
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart].PutCoords(ResX, ResY);

							pRunwayCenterLinePoint = Functions.GetNearestPoint(ref CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart]);
							if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
							{
								pElevatedPoint = pRunwayCenterLinePoint.Location;

								RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
							}
							else
							{
								pRunwayCenterLinePoint = Functions.GetNearestPoint(ref CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart], 10000.0);
								if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
								{
									pElevatedPoint = pRunwayCenterLinePoint.Location;
									RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								}
								else
									RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = Owner.Elev;
							}
						}
						else
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = RWYList[iRwyNum].pPtGeo[eRWY.PtTHR];

						RWYList[iRwyNum].pPtPrj[eRWY.PtStart] = (IPoint)Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtStart]);

						//==============================================================================================================
						pRunwayCenterLinePoint = Functions.GetNearestPoint(ref CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);
						if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
						}
						else
						{
							NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, fLDA, RWYList[iRwyNum].TrueBearing, ref ResX, ref ResY);
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = new ESRI.ArcGIS.Geometry.Point();
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);

							pRunwayCenterLinePoint = Functions.GetNearestPoint(ref CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd], 10000.0);
							if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
							{
								pElevatedPoint = pRunwayCenterLinePoint.Location;
								RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
							}
							else
								RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = Owner.Elev;
						}

						RWYList[iRwyNum].ClearWay = fTODA - fTORA;
						if (RWYList[iRwyNum].ClearWay < 0.0) RWYList[iRwyNum].ClearWay = 0.0;

						if (RWYList[iRwyNum].ClearWay > 0.0)
						{
							NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, ref ResX, ref ResY);
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);
						}

						RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = (IPoint)Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);

						//*************************************************************************

						RWYList[iRwyNum].ID = pRwyDirection.Identifier;
						RWYList[iRwyNum].Name = pRwyDirection.Designator;
						RWYList[iRwyNum].ADHP_ID = Owner.ID;
						//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

						pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
						RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
						RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;

						//RWYList[iRwyNum].ClearWay = 0.0;
						//pRunwayProtectAreaList = pObjectDir.GetRunwayProtectAreaList(pRwyDirection.Identifier);
						//for (K = 0; K < pRunwayProtectAreaList.Count; K++)
						//{
						//    pRunwayProtectArea = pRunwayProtectAreaList[K];
						//    pAirportHeliportProtectionArea = pRunwayProtectArea;
						//    if (pRunwayProtectArea.Type.Value == Aran.Aim.Enums.CodeRunwayProtectionArea.CWY && pAirportHeliportProtectionArea.Length != null)
						//    {
						//        RWYList[iRwyNum].ClearWay = pConverter.Convert(pAirportHeliportProtectionArea.Length, 0.0);
						//        break;
						//    }
						//}

						//if (RWYList[iRwyNum].ClearWay > 0.0)
						//{
						//	NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, ref ResX, ref ResY);
						//	RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);
						//	RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = (IPoint)Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);
						//}

						for (eRWY k = eRWY.PtStart; k <= eRWY.PtEnd; k++)
						{
							//RWYList[iRwyNum].pPtPrj[k] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[k]) as IPoint;
							if (RWYList[iRwyNum].pPtPrj[k].IsEmpty)
							{
								iRwyNum--;
								break;
							}

							RWYList[iRwyNum].pPtGeo[k].M = RWYList[iRwyNum].TrueBearing;
							RWYList[iRwyNum].pPtPrj[k].M = Functions.Azt2Dir(RWYList[iRwyNum].pPtGeo[k], RWYList[iRwyNum].TrueBearing);
						}
					}
				}
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
		{
			int n = NavaidList.GetLength(0);

			for (int i = 0; i < n; i++)
				if ((NavaidList[i].TypeCode == eNavaidClass.CodeLLZ) && (NavaidList[i].CallSign == ILS.CallSign))
					return i;

			System.Array.Resize<NavaidType>(ref NavaidList, n + 1);

			NavaidList[n].pPtGeo = ILS.pPtGeo;
			NavaidList[n].pPtPrj = ILS.pPtPrj;
			NavaidList[n].Name = ILS.CallSign;
			NavaidList[n].ID = ILS.ID;
			NavaidList[n].CallSign = ILS.CallSign;

			NavaidList[n].MagVar = ILS.MagVar;
			NavaidList[n].TypeName_Renamed = "LLZ";
			NavaidList[n].TypeCode = eNavaidClass.CodeLLZ;
			NavaidList[n].Range = 40000.0;
			NavaidList[n].index = ILS.Index;
			NavaidList[n].PairNavaidIndex = -1;

			NavaidList[n].GPAngle = ILS.GPAngle;
			NavaidList[n].GP_RDH = ILS.GP_RDH;

			NavaidList[n].Course = ILS.Course;
			NavaidList[n].LLZ_THR = ILS.LLZ_THR;
			NavaidList[n].SecWidth = ILS.SecWidth;
			NavaidList[n].AngleWidth = ILS.AngleWidth;
			NavaidList[n].Tag = 0;
			NavaidList[n].pSignificantPoint = ILS.pSignificantPoint;

			return n + 1;
		}

		//Function GetRWYByProcedureName(ByVal ProcName As String, ByVal ADHP As ADHPType, ByRef RWY As RWYType) As Boolean
		//	Dim I As Integer
		//	Dim N As Integer
		//	Dim pos As Integer

		//	Dim RWYList() As RWYType
		//	Dim bRWYFound As Boolean

		//	Dim RWYName As String
		//	Dim Char_Renamed As String

		//	GetRWYByProcedureName = False

		//	pos = InStr(1, ProcName, "RWY", 1)
		//	if pos <= 0  Exit Function

		//	pos = pos + 3
		//	RWYName = ""

		//	Char_Renamed = Mid(ProcName, pos, 1)
		//	Do While Char_Renamed <> " "
		//		RWYName = RWYName & Char_Renamed
		//		pos = pos + 1
		//		Char_Renamed = Mid(ProcName, pos, 1)
		//	Loop

		//	N = FillRWYList(RWYList, ADHP)

		//	bRWYFound = False
		//	For I = 0 To N
		//		Char_Renamed = RWYList(I).Name
		//		bRWYFound = RWYList(I).Name = RWYName
		//		if bRWYFound 
		//			GetRWYByProcedureName = True
		//			RWY = RWYList(I)
		//			Exit For
		//		End if
		//	Next I
		//End Function

		public static int GetILSByName(string ProcName, ADHPType ADHP, ref ILSType ILS)
		{
			RWYType[] RWYList;
			int pos = ProcName.IndexOf("RWY");

			if (pos <= 0)
				return 0;

			pos += 3;
			string RWYName = "";
			char chr = ProcName.ToCharArray(pos, 1)[0];

			while (chr != ' ')
			{
				RWYName = RWYName + chr;
				pos++;
				chr = ProcName.ToCharArray(pos, 1)[0];
			}

			//ADHP.ID = OwnerID
			//if FillADHPFields(ADHP) < 0  Exit Function

			int n = FillRWYList(out RWYList, ADHP);

			for (int i = 0; i < n; i++)
				if (RWYList[i].Name == RWYName)
					return GetILS(RWYList[i], ref ILS, ADHP);

			return 0;
			//if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		}

		public static int GetILS(RWYType RWY, ref ILSType ILS, ADHPType Owner)
		{
			Navaid pNavaidCom;
			List<NavaidComponent> pAIXMNAVEqList;

			NavaidEquipment pAIXMNAVEq;
			Localizer pAIXMLocalizer = null;
			Glidepath pAIXMGlidepath = null;

			ElevatedPoint pElevPoint;

			ILS.Index = 0;

			//if RWY.ILSID = ""  Exit Function

			pNavaidCom = pObjectDir.GetILSNavaid(RWY.pRunwayDir.Identifier);
			if (pNavaidCom == null)
				return 0;

			pAIXMNAVEqList = pNavaidCom.NavaidEquipment;
			if (pAIXMNAVEqList.Count == 0)
				return 0;

			ILS.Category = 4;
			ILS.pSignificantPoint = pNavaidCom;

			if (pNavaidCom.SignalPerformance != null)
				ILS.Category = (int)pNavaidCom.SignalPerformance.Value + 1;

			if (ILS.Category > 3)
				ILS.Category = 1;

			ILS.RWY_ID = RWY.ID;

			int j = 0;
			//CallSign = "";

			for (int i = 0; i < pAIXMNAVEqList.Count; i++)
			{
				pAIXMNAVEq = (NavaidEquipment)pAIXMNAVEqList[i].TheNavaidEquipment.GetFeature();

				if (pAIXMNAVEq is Localizer)
				{
					pAIXMLocalizer = (Localizer)pAIXMNAVEq;
					j = j | 1;
				}
				else if (pAIXMNAVEq is Glidepath)
				{
					pAIXMGlidepath = (Glidepath)pAIXMNAVEq;
					j = j | 2;
				}
				if (j == 3)
					break;
			}

			//    Set pAIXMNAVEq = pAIXMNAVEqList.Item(0).theNavaidEquipment
			//    Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(0).theNavaidEquipment).Cast()

			//    if (pAIXMNAVEq is LocalizerCom) 
			//        Set pAIXMLocalizer = pAIXMNAVEq
			//    else if (pAIXMNAVEq is GlidepathCom) 
			//        Set pAIXMGlidepath = pAIXMNAVEq
			//    End if

			//    if pAIXMNAVEqList.Count > 1 
			//        Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(1).theNavaidEquipment).Cast()
			//        'Set pAIXMNAVEq = pAIXMNAVEqList.Item(1).theNavaidEquipment
			//        if (pAIXMNAVEq is LocalizerCom) 
			//            Set pAIXMLocalizer = pAIXMNAVEq
			//        else if (pAIXMNAVEq is GlidepathCom) 
			//            Set pAIXMGlidepath = pAIXMNAVEq
			//        End if
			//    End if

			if (pAIXMLocalizer != null)
			{
				pAIXMNAVEq = (NavaidEquipment)pAIXMLocalizer;
				pElevPoint = pAIXMNAVEq.Location;

				if (pAIXMNAVEq.MagneticVariation != null)
					ILS.MagVar = pAIXMNAVEq.MagneticVariation.Value;
				else if (pAIXMLocalizer.TrueBearing != null && pAIXMLocalizer.MagneticBearing != null)
					ILS.MagVar = pAIXMLocalizer.MagneticBearing.Value - pAIXMLocalizer.TrueBearing.Value;
				else
					ILS.MagVar = Owner.MagVar;


				if (pAIXMLocalizer.TrueBearing != null)
					ILS.Course = pAIXMLocalizer.TrueBearing.Value;
				else if (pAIXMLocalizer.MagneticBearing != null)
					ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
				else
					goto NoLocalizer;

				//			ILS.course = Modulus(ILS.Course + 180.0, 360.0)

				ILS.pPtGeo = ConvertToEsriGeom.FromPoint(pElevPoint.Geo);
				ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.PtTHR].Z);
				ILS.pPtGeo.M = ILS.Course;

				ILS.pPtPrj = Functions.ToPrj(ILS.pPtGeo) as IPoint;

				if (ILS.pPtPrj.IsEmpty)
					return 0;

				ILS.pPtPrj.M = Functions.Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M);

				double dX = RWY.pPtPrj[eRWY.PtTHR].X - ILS.pPtPrj.X;
				double dY = RWY.pPtPrj[eRWY.PtTHR].Y - ILS.pPtPrj.Y;
				ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

				if (pAIXMLocalizer.WidthCourse != null)
				{
					ILS.AngleWidth = (double)pAIXMLocalizer.WidthCourse.Value;
					ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(Functions.DegToRad(ILS.AngleWidth));

					ILS.Index = 1;
					ILS.ID = pAIXMNAVEq.Identifier;
					ILS.CallSign = pAIXMNAVEq.Designator;

					//        ID = pAIXMNAVEq.identifier
					//        CallSign = pAIXMNAVEq.Designator
					//    else
					//        Exit Function
				}
			}
		NoLocalizer:

			if (pAIXMGlidepath != null)
			{
				pAIXMNAVEq = pAIXMGlidepath;

				if (pAIXMGlidepath.Slope != null)
				{
					ILS.GPAngle = (double)pAIXMGlidepath.Slope.Value;

					if (pAIXMGlidepath.Rdh != null)
					{
						ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, ILS.pPtGeo.Z);
						ILS.Index = ILS.Index | 2;

						if (ILS.Index == 2)
						{
							ILS.ID = pAIXMNAVEq.Identifier;
							ILS.CallSign = pAIXMNAVEq.Designator;
						}
					}
				}
			}
			return ILS.Index;
		}

		public static void FillNavaidList(out NavaidType[] NavaidList, out NavaidType[] DMEList, ADHPType CurrADHP, double Radius)
		{
			eNavaidClass NavTypeCode;
			string NavTypeName;

			int iNavaidNum;
			int iDMENum;
			double fDist;
			List<Navaid> pNavaidList;

			Navaid pNavaid;
			NavaidEquipment AixmNavaidEquipment;

			ESRI.ArcGIS.Geometry.IPolygon pPolygon;
			Aran.Geometries.MultiPolygon pARANPolygon;

			ElevatedPoint pElevPoint;

			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;

			pPolygon = (ESRI.ArcGIS.Geometry.IPolygon)Functions.ToGeo(Functions.CreatePrjCircle(CurrADHP.pPtPrj, Radius) as IGeometry);
			pARANPolygon = (MultiPolygon)ConvertFromEsriGeom.ToPolygonGeo(pPolygon);

			pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

			// .GetNavaidEquipmentList (pDelibPolygon)
			// Set ILSDataList = pObjectDir.GetILSNavaidEquipmentList(CurrADHP.ID)


			if (pNavaidList.Count == 0) 	//And (ILSDataList.Count = 0)
			{
				NavaidList = new NavaidType[0];
				DMEList = new NavaidType[0];
				return;
			}

			NavaidList = new NavaidType[pNavaidList.Count];		//+ ILSDataList.Count
			DMEList = new NavaidType[pNavaidList.Count];		//+ ILSDataList.Count

			iNavaidNum = -1;
			iDMENum = -1;

			for (int j = 0; j < pNavaidList.Count; j++)
			{
				pNavaid = pNavaidList[j];
				for (int i = 0; i < pNavaid.NavaidEquipment.Count; i++)
				{
					AixmNavaidEquipment = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

					if (AixmNavaidEquipment is VOR)
					{
						NavTypeCode = eNavaidClass.CodeVOR;
						NavTypeName = "VOR";
					}
					else if (AixmNavaidEquipment is DME)
					{
						NavTypeCode = eNavaidClass.CodeDME;
						NavTypeName = "DME";
					}
					else if (AixmNavaidEquipment is NDB)
					{
						NavTypeCode = eNavaidClass.CodeNDB;
						NavTypeName = "NDB";
					}
					else if (AixmNavaidEquipment is TACAN)
					{
						NavTypeCode = eNavaidClass.CodeTACAN;
						NavTypeName = "Tacan";
					}
					else
						continue;

					pElevPoint = AixmNavaidEquipment.Location;

					pPtGeo = ConvertToEsriGeom.FromPoint(pElevPoint.Geo);
					pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev);
					pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;

					if (pPtPrj.IsEmpty)
						continue;

					if (NavTypeCode == eNavaidClass.CodeDME)
					{
						iDMENum++;

						DMEList[iDMENum].pPtGeo = pPtGeo;
						DMEList[iDMENum].pPtPrj = pPtPrj;

						if (AixmNavaidEquipment.MagneticVariation != null)
							DMEList[iDMENum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
						else
							DMEList[iDMENum].MagVar = CurrADHP.MagVar;

						DMEList[iDMENum].Range = 350000.0; //DME.Range
						DMEList[iDMENum].PairNavaidIndex = -1;

						DMEList[iDMENum].Name = AixmNavaidEquipment.Name;
						DMEList[iDMENum].ID = AixmNavaidEquipment.Identifier;
						DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

						DMEList[iDMENum].TypeCode = NavTypeCode;
						DMEList[iDMENum].TypeName_Renamed = NavTypeName;
						DMEList[iDMENum].index = iNavaidNum + iDMENum;

						DMEList[iDMENum].pSignificantPoint = pNavaid;
					}
					else
					{
						iNavaidNum++;

						NavaidList[iNavaidNum].pPtGeo = pPtGeo;
						NavaidList[iNavaidNum].pPtPrj = pPtPrj;

						if (AixmNavaidEquipment.MagneticVariation != null)
							NavaidList[iNavaidNum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
						else
							NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar;

						if (NavTypeCode == eNavaidClass.CodeNDB)
							NavaidList[iNavaidNum].Range = 350000.0;		//NDB.Range
						else
							NavaidList[iNavaidNum].Range = 350000.0;		//VOR.Range

						NavaidList[iNavaidNum].PairNavaidIndex = -1;

						NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name;
						NavaidList[iNavaidNum].ID = AixmNavaidEquipment.Identifier;
						NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator;

						NavaidList[iNavaidNum].TypeCode = NavTypeCode;
						NavaidList[iNavaidNum].TypeName_Renamed = NavTypeName;
						NavaidList[iNavaidNum].index = iNavaidNum + iDMENum;

						NavaidList[iNavaidNum].pSignificantPoint = pNavaid;
					}
				}
			}

			for (int j = 0; j <= iNavaidNum; j++)
				for (int i = 0; i <= iDMENum; i++)
				{
					fDist = Functions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
					if (fDist <= 2.0)
					{
						NavaidList[j].PairNavaidIndex = i;
						DMEList[i].PairNavaidIndex = j;
						break;
					}
				}

			//    For I = 0 To ILSDataList.Count - 1
			//        Set AixmNavaidEquipment = ILSDataList.Item(I)
			//
			//        if Not (AixmNavaidEquipment is ILocalizer)  continue For
			//
			//        Set pElevPoint = AixmNavaidEquipment.ElevatedPoint
			//        Set pGMLPoint = pElevPoint
			//        Set pAIXMLocalizer = AixmNavaidEquipment
			//
			//        if pAIXMLocalizer.TrueBearing is Nothing  continue For
			//
			//        Set pPtGeo = pGMLPoint.Tag
			//        pPtGeo.Z = pConverter.Convert(pElevPoint.elevation, CurrADHP.Elev)
			//        pPtGeo.M = pAIXMLocalizer.TrueBearing
			//
			//        Set pPtPrj = Functions.ToPrj(pPtGeo)
			//        if pPtPrj.IsEmpty()  continue For
			//
			//        pPtPrj.M = Functions.Azt2Dir(pPtGeo, pPtGeo.M)
			//'=====================================================
			//
			//        iNavaidNum = iNavaidNum + 1
			//
			//        Set NavaidList[iNavaidNum].pPtGeo = pPtGeo
			//        Set NavaidList[iNavaidNum].pPtPrj = pPtPrj
			//
			//        NavaidList[iNavaidNum].course = pPtGeo.M
			//'        dX = RWY.pPtPrj(eRWY.PtTHR).X - pPtPrj.X
			//'        dY = RWY.pPtPrj(eRWY.PtTHR).Y - pPtPrj.Y
			//'        NavaidList[iNavaidNum].LLZ_THR = Sqr(dX * dX + dY * dY)
			//
			//        if Not pAIXMLocalizer.WidthCourse is Nothing 
			//            NavaidList[iNavaidNum].AngleWidth = pAIXMLocalizer.WidthCourse
			//'           NavaidList[iNavaidNum].SecWidth = NavaidList[iNavaidNum].LLZ_THR * Tan(Functions.DegToRad(NavaidList[iNavaidNum].AngleWidth))  'NavaidList[iNavaidNum].SecWidth = pAIXMLocalizer.WidthCourse
			//        End if
			//'=====================================================
			//
			//        if (Not AixmNavaidEquipment.MagneticVariation is Nothing) 
			//            NavaidList[iNavaidNum].MagVar = AixmNavaidEquipment.MagneticVariation.Value
			//        else
			//            NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar
			//        End if
			//
			//        NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name
			//        NavaidList[iNavaidNum].ID = AixmNavaidEquipment.identifier
			//        NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator
			//
			//        NavaidList[iNavaidNum].Range = 35000#
			//        NavaidList[iNavaidNum].TypeCode = CodeLLZ
			//        NavaidList[iNavaidNum].TypeName = "ILS"
			//
			//'    GP_RDH = 0
			//
			//'    GPAngle = 0
			//
			//    Next I

			System.Array.Resize<NavaidType>(ref NavaidList, iNavaidNum + 1);
			System.Array.Resize<NavaidType>(ref DMEList, iDMENum + 1);
		}

		public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, ADHPType CurrADHP, double radius)
		{
			eNavaidClass NavTypeCode;
			string NavTypeName;

			List<DesignatedPoint> AIXMWPTList;
			List<Navaid> pNavaidComList;

			Navaid pNavaidCom;
			NavaidEquipment AIXMNAVEq;
			DesignatedPoint AIXMWPT;

			ESRI.ArcGIS.Geometry.IPolygon pPolygon;
			Aran.Geometries.MultiPolygon pARANPolygon;

			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;

			pPolygon = (IPolygon)Functions.ToGeo((IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, radius));
			pARANPolygon = ConvertFromEsriGeom.ToGeometry(pPolygon) as Aran.Geometries.MultiPolygon;

			AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon);
			//Set AIXMNAVList = pObjectDir.GetNavaidEquipmentList(pDelibPolygon)
			pNavaidComList = pObjectDir.GetNavaidList(pARANPolygon);

			int n = AIXMWPTList.Count + 2 * pNavaidComList.Count;
			if (n == 0)
			{
				WPTList = new WPT_FIXType[0];
				return -1;
			}

			int iWPTNum = -1;

			WPTList = new WPT_FIXType[n];

			for (int i = 0; i < AIXMWPTList.Count; i++)
			{
				AIXMWPT = AIXMWPTList[i];

				pPtGeo = ConvertToEsriGeom.FromPoint(AIXMWPT.Location.Geo);
				pPtGeo.Z = CurrADHP.pPtGeo.Z + 300.0;
				pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;

				if (pPtPrj.IsEmpty)
					continue;
				iWPTNum++;

				//if Not AIXMWPT.MagneticVariation is Nothing 
				//	WPTList[iWPTNum].MagVar = AIXMWPT.MagneticVariation.Value
				//else
				//	WPTList[iWPTNum].MagVar = CurrADHP.MagVar
				//End if
				WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

				WPTList[iWPTNum].pPtGeo = pPtGeo;
				WPTList[iWPTNum].pPtPrj = pPtPrj;

				WPTList[iWPTNum].Name = AIXMWPT.Designator;
				WPTList[iWPTNum].ID = AIXMWPT.Identifier;

				WPTList[iWPTNum].pSignificantPoint = AIXMWPT;
				WPTList[iWPTNum].TypeName_Renamed = "WPT";
				WPTList[iWPTNum].TypeCode = eNavaidClass.CodeNONE;
			}

			//======================================================================

			for (int j = 0; j < pNavaidComList.Count; j++)
			{
				pNavaidCom = pNavaidComList[j];
				for (int i = 0; i < pNavaidCom.NavaidEquipment.Count; i++)
				{
					AIXMNAVEq = (NavaidEquipment)pNavaidCom.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

					//    For I = 0 To AIXMNAVList.Count - 1
					//        Set AIXMNAVEq = AIXMNAVList.Item(I).Cast()

					//        Set pElevPoint = AIXMNAVEq.Location
					//        Set pGMLPoint = pElevPoint

					pPtGeo = ConvertToEsriGeom.FromPoint(AIXMNAVEq.Location.Geo);
					pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, CurrADHP.Elev);

					pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
					if (pPtPrj.IsEmpty)
						continue;

					if (AIXMNAVEq is VOR)
					{
						NavTypeCode = eNavaidClass.CodeVOR;
						NavTypeName = "VOR";
					}
					else if (AIXMNAVEq is NDB)
					{
						NavTypeCode = eNavaidClass.CodeNDB;
						NavTypeName = "NDB";
					}
					else if (AIXMNAVEq is TACAN)
					{
						NavTypeCode = eNavaidClass.CodeTACAN;
						NavTypeName = "TACAN";
					}
					else
						continue;

					iWPTNum++;

					WPTList[iWPTNum].pPtGeo = pPtGeo;
					WPTList[iWPTNum].pPtPrj = pPtPrj;

					if (AIXMNAVEq.MagneticVariation != null)
						WPTList[iWPTNum].MagVar = AIXMNAVEq.MagneticVariation.Value;
					else
						WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

					WPTList[iWPTNum].Name = AIXMNAVEq.Designator;
					WPTList[iWPTNum].ID = AIXMNAVEq.Identifier;

					WPTList[iWPTNum].pSignificantPoint = pNavaidCom;

					WPTList[iWPTNum].TypeName_Renamed = NavTypeName;
					WPTList[iWPTNum].TypeCode = NavTypeCode;
				}
			}
			//======================================================================
			iWPTNum++;
			System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

			return iWPTNum;
		}

		//public static int GetArObstaclesByPoly(ref ObstacleType[] ObstacleList, ESRI.ArcGIS.Geometry.IPolygon pPoly, double fRefHeight)
		//{
		//    List<VerticalStructure> VerticalStructureList;
		//    VerticalStructure AixmObstacle;
		//    ElevatedPoint pElevatedPoint;
		//    Aran.Geometries.MultiPolygon pARANPolygon;
		//    ConvertToSI pConverter;
		//    ESRI.ArcGIS.Geometry.IPoint pPtGeo;
		//    ESRI.ArcGIS.Geometry.IPoint pPtPrj;

		//    pARANPolygon = ConvertFromEsriGeom.ESRIGeometryToARANGeometry(Functions.ToGeo(pPoly))
		//                        as Aran.Geometries.MultiPolygon;

		//    VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//    int n = VerticalStructureList.Count;
		//    ObstacleList = new ObstacleType[0];

		//    if (n == 0)
		//        return -1;

		//    pConverter = new ConvertToSI();

		//    System.Array.Resize<ObstacleType>(ref ObstacleList, n);
		//    //pProxiOperator = CurrADHP.pPtPrj;
		//    int j = -1;

		//    for (int i = 0; i < n; i++)
		//    {
		//        AixmObstacle = VerticalStructureList[i];

		//        if (AixmObstacle.Part.Count == 0)
		//            continue;
		//        if (AixmObstacle.Part[0].HorizontalProjection == null)
		//            continue;

		//        pElevatedPoint = null;
		//        if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//            pElevatedPoint = (ElevatedPoint)AixmObstacle.Part[0].HorizontalProjection.Location;

		//        if (pElevatedPoint == null) continue;
		//        if (pElevatedPoint.Elevation == null) continue;

		//        pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
		//        pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation, -9999.0);	// +pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, 0);

		//        pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
		//        if (pPtPrj.IsEmpty)
		//            continue;

		//        j++;
		//        ObstacleList[j].pPtGeo = pPtGeo;
		//        ObstacleList[j].pPtPrj = pPtPrj;
		//        ObstacleList[j].Name = AixmObstacle.Name;
		//        ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//        ObstacleList[j].ID = AixmObstacle.Id;

		//        ObstacleList[j].HorAccuracy = pConverter.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//        ObstacleList[j].VertAccuracy = pConverter.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//        ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
		//        //ObstacleList[J].Dist = pProxiOperator.ReturnDistance(ObstacleList[J].pPtPrj);
		//        ObstacleList[j].index = i;
		//    }
		//    j++;
		//    System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//    return j;
		//}

		////public static int GetHDObstacles(ref ObstacleHd[] ObstacleList, ADHPType CurrADHP, double MaxDist, double fRefHeight)
		////{
		////    ESRI.ArcGIS.Geometry.IPolygon pPoly;
		////    pPoly = (ESRI.ArcGIS.Geometry.IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, MaxDist);

		////    return GetArObstaclesByPoly(ref ObstacleList, pPoly, fRefHeight);
		////}

		//static public int GetHDObstacles(ref ObstacleType[] ObstacleList, ADHPType CurrADHP, double MaxDist, double fRefHeight)
		//{
		//    List<VerticalStructure> VerticalStructureList;
		//    VerticalStructure AixmObstacle;
		//    ElevatedPoint pElevatedPoint;

		//    ConvertToSI pConverter;
		//    ESRI.ArcGIS.Geometry.IProximityOperator pProxiOperator;

		//    ESRI.ArcGIS.Geometry.IPoint pPtGeo;
		//    ESRI.ArcGIS.Geometry.IPoint pPtPrj;

		//    ESRI.ArcGIS.Geometry.IPolygon pPolygon;
		//    Aran.Geometries.MultiPolygon pARANPolygon;

		//    pPolygon = (ESRI.ArcGIS.Geometry.IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, MaxDist);

		//    pARANPolygon = ConvertFromEsriGeom.ESRIGeometryToARANGeometry(Functions.ToGeo(pPolygon))
		//                        as Aran.Geometries.MultiPolygon;

		//    VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//    int n = VerticalStructureList.Count;

		//    ObstacleList = new ObstacleType[0];

		//    if (n == 0)
		//        return -1;

		//    pConverter = new ConvertToSI();

		//    System.Array.Resize<ObstacleType>(ref ObstacleList, n);
		//    pProxiOperator = ((ESRI.ArcGIS.Geometry.IProximityOperator)(CurrADHP.pPtPrj));
		//    int j = -1;

		//    for (int i = 0; i < n; i++)
		//    {
		//        AixmObstacle = VerticalStructureList[i];

		//        if (AixmObstacle.Part.Count == 0) continue;
		//        if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

		//        pElevatedPoint = null;
		//        if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//            pElevatedPoint = (ElevatedPoint)AixmObstacle.Part[0].HorizontalProjection.Location;

		//        if (pElevatedPoint == null) continue;
		//        if (pElevatedPoint.Elevation == null) continue;

		//        pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
		//        pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation, -9999.0);		// +pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, 0);

		//        pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
		//        if (pPtPrj.IsEmpty) continue;

		//        j++;
		//        ObstacleList[j].pPtGeo = pPtGeo;
		//        ObstacleList[j].pPtPrj = pPtPrj;
		//        ObstacleList[j].Name = AixmObstacle.Name;
		//        ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//        ObstacleList[j].ID = AixmObstacle.Id;

		//        ObstacleList[j].HorAccuracy = pConverter.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//        ObstacleList[j].VertAccuracy = pConverter.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//        ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
		//        //ObstacleList[j].Dist = pProxiOperator.ReturnDistance(ObstacleList[j].pPtPrj);
		//        ObstacleList[j].index = i;
		//    }
		//    j++;
		//    System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//    return j;
		//}

		//public static int GetObstListInPoly(ref ObstacleType[] ObstList, IPolygon pPoly)
		//{
		//    List<VerticalStructure> VerticalStructureList;
		//    VerticalStructure AixmObstacle;
		//    ElevatedPoint pElevatedPoint;

		//    Aran.Geometries.MultiPolygon pARANPolygon;

		//    ConvertToSI pConverter;
		//    ESRI.ArcGIS.Geometry.IPoint pPtGeo;
		//    ESRI.ArcGIS.Geometry.IPoint pPtPrj;


		//    pARANPolygon = ConvertFromEsriGeom.ESRIGeometryToARANGeometry(Functions.ToGeo(pPoly))
		//                        as Aran.Geometries.MultiPolygon;

		//    VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//    int n = VerticalStructureList.Count;

		//    ObstList = new ObstacleType[0];

		//    if (n == 0)
		//        return -1;

		//    pConverter = new ConvertToSI();

		//    System.Array.Resize<ObstacleType>(ref ObstList, n);
		//    int j = -1;

		//    for (int i = 0; i < n; i++)
		//    {
		//        AixmObstacle = VerticalStructureList[i];

		//        if (AixmObstacle.Part.Count == 0) continue;
		//        if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

		//        pElevatedPoint = null;
		//        if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//            pElevatedPoint = (ElevatedPoint)AixmObstacle.Part[0].HorizontalProjection.Location;

		//        if (pElevatedPoint == null) continue;
		//        if (pElevatedPoint.Elevation == null) continue;

		//        pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
		//        pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation, -9999.0);		// +pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, 0);

		//        pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
		//        if (pPtPrj.IsEmpty) continue;

		//        j++;

		//        ObstList[j].pPtGeo = pPtGeo;
		//        ObstList[j].pPtPrj = pPtPrj;

		//        ObstList[j].HorAccuracy = pConverter.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//        ObstList[j].VertAccuracy = pConverter.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//        ObstList[j].Name = AixmObstacle.Name;
		//        ObstList[j].Identifier = AixmObstacle.Identifier;
		//        ObstList[j].ID = AixmObstacle.Id;
		//        ObstList[j].Height = ObstList[j].pPtGeo.Z;
		//        ObstList[j].index = i;
		//    }
		//    j++;
		//    System.Array.Resize<ObstacleType>(ref ObstList, j);
		//    return j;
		//}

		/*
	public int FillSegObstList(ByVal LastPoint As StepDownFIX, ByVal fRefHeight As Double, ByVal IAF_FullAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal IAF_BaseAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByRef ObstList() As ObstacleAr)
{
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim FIXHeight As Double
		Dim fAlpha As Double
		Dim fDist As Double
		Dim fDir As Double
		Dim fL As Double

		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint
		Dim GuidNav As NavaidType

		Dim ObsR As Double
		Dim ObsMOC As Double

		Dim VerticalStructureList As List<VerticalStructure)
		Dim AixmObstacle As VerticalStructure
		Dim pElevatedPoint As ElevatedPoint
		Dim pConverter As ConvertToSI
		Dim pDelibPolygon As ElevatedSurface

		ReDim ObstList(-1)

		pDelibPolygon = Delib.ESRIConverter.ESRIConversionHelper.EsriGeometryToGeometryObject(Functions.ToGeo(IAF_FullAreaPoly), True)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pDelibPolygon)
		N = VerticalStructureList.Count - 1

		if N < 0  Return -1

		pConverter = New ConvertToSI
		ReDim ObstList(N)

		pBaseProxi = IAF_BaseAreaPoly

		FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		fDir = LastPoint.InDir
		GuidNav = LastPoint.GuidNav

		fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)
		J = -1
		For I = 0 To N
			AixmObstacle = VerticalStructureList.Item(I)

			if AixmObstacle.Part.Count = 0  continue For
			if AixmObstacle.Part.Item(0).horizontalProjection is Nothing  continue For

			pElevatedPoint = Nothing
			if AixmObstacle.Part.Item(0).horizontalProjection.geometryType = Delib.Classes.Enums.GeometryType.Point 
				pElevatedPoint = AixmObstacle.Part.Item(0).horizontalProjection
			End if

			if pElevatedPoint is Nothing  continue For
			if pElevatedPoint.Elevation is Nothing  continue For

			pPtGeo = Delib.ESRIConverter.ESRIConversionHelper.GeometryObjectToEsriGeometry(pElevatedPoint)
			pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation.elevation, -9999.0) + pConverter.Convert(AixmObstacle.Part(0).verticalExtent, 0)

			pPtPrj = Functions.ToPrj(pPtGeo)
			if pPtPrj.IsEmpty()  continue For

			J = J + 1

			ObstList[J].pPtGeo = pPtGeo
			ObstList[J].pPtPrj = pPtPrj

			ObstList[J].HorAccuracy = pConverter.Convert(pElevatedPoint.horizontalAccuracy, 0.0)
			ObstList[J].VertAccuracy = pConverter.Convert(pElevatedPoint.Elevation.verticalAccuracy, 0.0)

			fDist = pBaseProxi.ReturnDistance(ObstList[J].pPtPrj)

			ObstList[J].Height = ObstList[J].pPtGeo.Z - fRefHeight
			ObstList[J].Flags = -CShort(fDist = 0.0)
			ObsMOC = arIASegmentMOC.Value * (1.0 - 2 * fDist / arIFHalfWidth.Value)

			if FIXHeight <= ObstList[J].Height 
				ObsR = arIFHalfWidth.Value
			else
				ObsR = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObstList[J].Height) / arIASegmentMOC.Value 'MOC '
			End if

			if ObsR > arIFHalfWidth.Value  ObsR = arIFHalfWidth.Value

			if GuidNav.TypeCode <> 1 
				ObstList[J].Dist = Point2LineDistancePrj(ObstList[J].pPtPrj, LastPoint.pPtPrj, fDir + 90.0)
				ObstList[J].CLDist = Point2LineDistancePrj(ObstList[J].pPtPrj, LastPoint.pPtPrj, fDir)
			else
				ObstList[J].Dist = Functions.DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ObstList[J].pPtPrj))) * fL
				ObstList[J].CLDist = System.Math.Abs(fL - ReturnDistanceInMeters(ObstList[J].pPtPrj, GuidNav.pPtPrj))
			End if

			ObstList[J].MOC = ObsMOC
			ObstList[J].ReqH = ObstList[J].Height + ObsMOC
			ObstList[J].hPent = ObstList[J].ReqH - FIXHeight
			ObstList[J].fTmp = ObstList[J].hPent / ObstList[J].Dist

			ObstList[J].Name = AixmObstacle.name
			ObstList[J].Identifier = AixmObstacle.identifier
			ObstList[J].ID = AixmObstacle.Designator
			ObstList[J].Index = I
		Next I

		if J >= 0 
			ReDim Preserve ObstList[J]
		else
			ReDim ObstList(-1)
		End if

		Return J
}

	public int CalcIFProhibitions(ByVal LastPoint As StepDownFIX, ByVal fRefHeight As Double, ByVal IAF_FullAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal IAF_BaseAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByRef ObstList() As ObstacleAr, ByRef ProhibitionSectors() As IFProhibitionSector)
{
		Dim I As Integer
		Dim J As Integer
		Dim N As Integer

		Dim FIXHeight As Double
		Dim fAlpha As Double

		Dim fDist As Double
		Dim fDir As Double
		Dim fL As Double

		Dim pBaseProxi As ESRI.ArcGIS.Geometry.IProximityOperator
		Dim GuidNav As NavaidType
		Dim pPtGeo As ESRI.ArcGIS.Geometry.IPoint
		Dim pPtPrj As ESRI.ArcGIS.Geometry.IPoint

		Dim ObsR As Double
		Dim ObsMOC As Double
		Dim ObsDist As Double
		Dim ObsCLDist As Double
		Dim ObsHeight As Double

		Dim VerticalStructureList As List<VerticalStructure)
		Dim AixmObstacle As VerticalStructure
		Dim pElevatedPoint As ElevatedPoint
		Dim pDelibPolygon As ElevatedSurface
		Dim pConverter As ConvertToSI

		ReDim ObstList(-1)
		ReDim ProhibitionSectors(-1)

		pDelibPolygon = Delib.ESRIConverter.ESRIConversionHelper.EsriGeometryToGeometryObject(Functions.ToGeo(IAF_FullAreaPoly), True)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pDelibPolygon)
		N = VerticalStructureList.Count - 1

		if N < 0  Return -1

		pConverter = New ConvertToSI
		ReDim ObstList(N)
		ReDim ProhibitionSectors(N)

		pBaseProxi = IAF_BaseAreaPoly

		FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		fDir = LastPoint.InDir
		GuidNav = LastPoint.GuidNav

		fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)

		J = -1

		For I = 0 To N
			AixmObstacle = VerticalStructureList.Item(I)

			if AixmObstacle.Part.Count = 0  continue For
			if AixmObstacle.Part.Item(0).horizontalProjection is Nothing  continue For

			pElevatedPoint = Nothing
			if AixmObstacle.Part.Item(0).horizontalProjection.geometryType = Delib.Classes.Enums.GeometryType.Point 
				pElevatedPoint = AixmObstacle.Part.Item(0).horizontalProjection
			End if

			if pElevatedPoint is Nothing  continue For
			if pElevatedPoint.Elevation is Nothing  continue For

			pPtGeo = Delib.ESRIConverter.ESRIConversionHelper.GeometryObjectToEsriGeometry(pElevatedPoint)
			pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation.elevation, -9999.0) + pConverter.Convert(AixmObstacle.Part(0).verticalExtent, 0)

			pPtPrj = Functions.ToPrj(pPtGeo)
			if pPtPrj.IsEmpty()  continue For

			fDist = pBaseProxi.ReturnDistance(pPtPrj)
			ObsMOC = arIASegmentMOC.Value * (1.0 - 2.0 * fDist / arIFHalfWidth.Value)
			ObsHeight = pPtGeo.Z - fRefHeight

			if FIXHeight - ObsHeight >= ObsMOC  continue For

			if FIXHeight <= ObsHeight 
				ObsR = arIFHalfWidth.Value
			else
				ObsR = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObsHeight) / arIASegmentMOC.Value	'MOC '
			End if

			if ObsR > arIFHalfWidth.Value  ObsR = arIFHalfWidth.Value

			if GuidNav.TypeCode <> 1 
				ObsDist = Point2LineDistancePrj(pPtPrj, LastPoint.pPtPrj, fDir + 90.0)
				ObsCLDist = Point2LineDistancePrj(pPtPrj, LastPoint.pPtPrj, fDir)
			else
				ObsDist = Functions.DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, pPtPrj))) * fL
				ObsCLDist = System.Math.Abs(fL - ReturnDistanceInMeters(pPtPrj, GuidNav.pPtPrj))
			End if

			if (ObsR > 0.5 * arIFHalfWidth.Value) And (ObsCLDist <= ObsR) 
				J = J + 1
				ObstList[J].pPtGeo = pPtGeo
				ObstList[J].pPtPrj = pPtPrj

				ObstList[J].HorAccuracy = pConverter.Convert(pElevatedPoint.horizontalAccuracy, 0.0)
				ObstList[J].VertAccuracy = pConverter.Convert(pElevatedPoint.Elevation.verticalAccuracy, 0.0)

				ObstList[J].Dist = ObsDist
				ObstList[J].CLDist = ObsCLDist
				ObstList[J].Height = ObsHeight
				ObstList[J].Flags = -CShort(fDist = 0.0)

				ObstList[J].Name = AixmObstacle.name
				ObstList[J].Identifier = AixmObstacle.identifier
				ObstList[J].ID = AixmObstacle.Designator

				ObstList[J].MOC = ObsMOC
				ObstList[J].ReqH = ObsHeight + ObsMOC
				ObstList[J].hPent = ObstList[J].ReqH - FIXHeight
				ObstList[J].fTmp = ObstList[J].hPent / ObsDist
				ObstList[J].Index = J

				ProhibitionSectors(J).MOC = ObsMOC
				ProhibitionSectors(J).rObs = ObsR
				ProhibitionSectors(J).ObsArea = CreatePrjCircle(pPtPrj, ObsR)

				ProhibitionSectors(J).DirToObst = ReturnAngleInDegrees(LastPoint.pPtPrj, pPtPrj)
				ProhibitionSectors(J).DistToObst = ReturnDistanceInMeters(LastPoint.pPtPrj, pPtPrj)

				//            if ObsR < ProhibitionSectors(J).DistToObst 
				//                fTmp = ArcSin(rObs / ProhibitionSectors(J).DistToObst)
				//                ProhibitionSectors(J).AlphaFrom = Round(Modulus(ProhibitionSectors(J).DirToObst - RadToDeg(fTmp), 360.0) - 0.4999)
				//                ProhibitionSectors(J).AlphaTo = Round(Modulus(ProhibitionSectors(J).DirToObst + RadToDeg(fTmp), 360.0) + 0.4999)
				//            else
				//                ProhibitionSectors(J).AlphaFrom = 0
				//                ProhibitionSectors(J).AlphaTo = 360
				//            End if

				ProhibitionSectors(J).dHObst = ObsHeight - FIXHeight + ObsMOC
				ProhibitionSectors(J).Index = I
			End if
		Next I

		if J >= 0 
			ReDim Preserve ObstList[J]
			ReDim Preserve ProhibitionSectors(J)
		else
			ReDim ObstList(-1)
			ReDim ProhibitionSectors(-1)
		End if

		Return J
}

	public bool GetMSASectors(ByVal Nav As NavaidType, ByRef MSAList() As MSAType)
{
		Dim I As Integer
		Dim J As Integer
		Dim K As Integer
		Dim N As Integer
		Dim M As Integer
		Dim fTmp As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim SAAList As List<Delib.Classes.Features.Procedures.SafeAltitudeArea)
		Dim SAASectorList As List<Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector)
		Dim SectorDefinition As Delib.Classes.Objects.Procedure.CircleSector

		Dim SafeAltitudeArea As Delib.Classes.Features.Procedures.SafeAltitudeArea
		Dim SafeAltitudeSector As Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector
		Dim pConverter As ConvertToSI

		SAAList = pObjectDir.GetSafeAltitudeAreaList(Nav.pSignificantPoint)
		N = SAAList.Count

		if N = 0 
			ReDim MSAList(-1)
			Return False
		End if

		ReDim MSAList(N - 1)
		pConverter = New ConvertToSI
		J = -1

		Dim pSignificantPoint As ISignificantPoint
		Dim pNAVEq As NavaidEquipment
		Dim pADHP As AirportHeliport
		Dim pDesign As DesignatedPoint
		For I = 0 To N - 1
			SafeAltitudeArea = SAAList.Item(I)

			if SafeAltitudeArea.safeAreaType.Value = Delib.Classes.Codes.SafeAltitudeType.MSA 

				SAASectorList = SafeAltitudeArea.sector
				//Set SAASectorList = pObjectDir.GetSafeAltitudeAreaSectorList(SafeAltitudeArea.Identifier)
				M = SAASectorList.Count
			if m> 0 then
				J = J + 1
				MSAList(J).Name = SafeAltitudeArea.identifier
				ReDim MSAList(J).Sectors(M - 1)

				//            Set pFeaturePoint = pObjectDir.GetFeature(SafeAltitudeArea.CentrePoint.featurePoint).Cast()
				//            pFeaturePoint.


				pSignificantPoint = SafeAltitudeArea.centrePoint
				if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.Navaid 
					pNAVEq = pSignificantPoint
					//ptCenter = Delib.ESRIConverter.ESRIConversionHelper.GeometryObjectToEsriGeometry(pNAVEq.location)
				else if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.AirportHeliport 
					pADHP = pSignificantPoint
					//ptCenter = Delib.ESRIConverter.ESRIConversionHelper.GeometryObjectToEsriGeometry(pADHP.arp)
				else if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.DesignatedPoint 
					pDesign = pSignificantPoint
					//ptCenter = Delib.ESRIConverter.ESRIConversionHelper.GeometryObjectToEsriGeometry(pDesign.location)
				else
					continue For
				End if

				For K = 0 To M - 1
					SafeAltitudeSector = SAASectorList.Item(K)
					SectorDefinition = SafeAltitudeSector.sectorDefinition

					FromAngle = SectorDefinition.fromAngle.Value
					ToAngle = SectorDefinition.toAngle.Value

					if SectorDefinition.angleDirectionReference.Value = Delib.Classes.Codes.DirectionReferenceType.TO 
						FromAngle = FromAngle + 180.0
						ToAngle = ToAngle + 180.0
					End if

					if SectorDefinition.arcDirection.Value = Delib.Classes.Codes.ArcDirectionType.CCA 
						fTmp = FromAngle
						FromAngle = ToAngle
						ToAngle = fTmp
					End if

					if SectorDefinition.angleType.Value <> Delib.Classes.Codes.BearingType.TRUE 

					End if

					MSAList(J).Sectors(K).LowerLimit = pConverter.Convert(SectorDefinition.lowerLimit, 0.0)
					MSAList(J).Sectors(K).UpperLimit = pConverter.Convert(SectorDefinition.upperLimit, 0.0)

					MSAList(J).Sectors(K).InnerDist = pConverter.Convert(SectorDefinition.innerDistance, 0.0)
					MSAList(J).Sectors(K).OuterDist = pConverter.Convert(SectorDefinition.outerDistance, 19000.0)
					MSAList(J).Sectors(K).FromAngle = FromAngle
					MSAList(J).Sectors(K).ToAngle = ToAngle
					MSAList(J).Sectors(K).AbsAngle = SubtractAngles(FromAngle, ToAngle)

					//                Set MSAList(J).Sectors(K).Sector = CreateMSASectorPrj(ptCenter, MSAList(J).Sectors(K))
				Next K
			End if
			End if
		Next I

		if J >= 0 
			ReDim Preserve MSAList(J)
		else
			ReDim MSAList(-1)
		End if

		Return False
}
		*/
		//Private Function CreateMSASectorPrj(ByVal ptCntGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal Sector As SectorMSA) As ESRI.ArcGIS.Geometry.IPolygon
		//	Dim I As Integer
		//	Dim N As Integer

		//	Dim AngleFrom As Double
		//	Dim AngleTo As Double
		//	Dim dAngle As Double
		//	Dim AngStep As Integer

		//	Dim iInRad As Double
		//	Dim CosI As Double
		//	Dim SinI As Double

		//	Dim ptCntPrj As ESRI.ArcGIS.Geometry.IPoint
		//	Dim ptInner As ESRI.ArcGIS.Geometry.IPoint
		//	Dim ptOuter As ESRI.ArcGIS.Geometry.IPoint

		//	Dim pPolygon As ESRI.ArcGIS.Geometry.IPointCollection
		//	Dim pTopo As ESRI.ArcGIS.Geometry.ITopologicalOperator2

		//	pPolygon = New ESRI.ArcGIS.Geometry.Polygon
		//	ptCntPrj = Functions.ToPrj(ptCntGeo)
		//	ptInner = New ESRI.ArcGIS.Geometry.Point
		//	ptOuter = New ESRI.ArcGIS.Geometry.Point

		//	AngleTo = Functions.Azt2Dir(ptCntGeo, Sector.FromAngle)
		//	AngleFrom = Functions.Azt2Dir(ptCntGeo, Sector.ToAngle)

		//	dAngle = Modulus(AngleTo - AngleFrom, 360.0)
		//	AngStep = 1

		//	N = System.Math.Round(dAngle / AngStep)

		//	if (N < 1) 
		//		N = 1
		//	else if (N < 5) 
		//		N = 5
		//	else if (N < 10) 
		//		N = 10
		//	End if

		//	AngStep = dAngle / N

		//	For I = 0 To N
		//		iInRad = Functions.DegToRad(AngleFrom + I * AngStep)
		//		CosI = System.Math.Cos(iInRad)
		//		SinI = System.Math.Sin(iInRad)

		//		ptInner.X = ptCntPrj.X + Sector.InnerDist * CosI
		//		ptInner.Y = ptCntPrj.Y + Sector.InnerDist * SinI

		//		ptOuter.X = ptCntPrj.X + Sector.OuterDist * CosI
		//		ptOuter.Y = ptCntPrj.Y + Sector.OuterDist * SinI

		//		pPolygon.AddPoint(ptInner)
		//		pPolygon.AddPoint(ptOuter, 0)
		//	Next I

		//	pTopo = pPolygon
		//	pTopo.IsKnownSimple_2 = False
		//	pTopo.Simplify()

		//	Return pPolygon
		//End Function

		//public Function GetADHPWorkspace() As ESRI.ArcGIS.Geodatabase.IWorkspace
		//	Dim I As Integer
		//	Dim Map As ESRI.ArcGIS.Carto.IMap
		//	Dim fL As ESRI.ArcGIS.Carto.IFeatureLayer
		//	Dim ds As ESRI.ArcGIS.Geodatabase.IDataset

		//	Map = GetMap()

		//	For I = 0 To Map.LayerCount - 1
		//		if Map.Layer(I) is ESRI.ArcGIS.Carto.IFeatureLayer 
		//			if Map.Layer(I).Name = "ADHP" 
		//				fL = Map.Layer(I)
		//				ds = fL.FeatureClass
		//				Return ds.Workspace
		//			End if
		//		End if
		//	Next I

		//	Return Nothing
		//End Function

		private static bool isOpen = false;

		public static void InitModule()
		{
			if (!isOpen)
			{
				pObjectDir = PandaSQPIFactory.Create();
				Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;
				pObjectDir.Open((IDbProvider)GlobalVars.gAranEnv.DbProvider);

				isOpen = true;
			}
		}

		public static void CloseDB()
		{
			if (isOpen)
			{
				//pObjectDir.Close();
				isOpen = false;
			}
		}
	}
}
