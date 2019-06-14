using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Queries;
using Aran.Queries.Panda_2;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IPandaSpecializedQPI pObjectDir;

		public static ValDistance CreateValDistanceType(UomDistance uom, double value)
		{
			ValDistance res = new ValDistance();
			res.Uom = uom;
			res.Value = value;
			return res;
		}

		//public static ValDistanceVertical CreateValAltitudeType(UomDistanceVertical uom, double value)
		//{
		//	ValDistanceVertical res = new ValDistanceVertical();
		//	res.Uom = uom;
		//	res.Value = value;
		//	return res;
		//}

		//public static int FillADHPList(ref ADHPType[] ADHPList, Guid organ, bool CheckILS = false)
		//{
		//	int i, n;

		//	Descriptor pName;
		//	List<Descriptor> pADHPNameList;

		//	pADHPNameList = pObjectDir.GetAirportHeliportList(organ, CheckILS);	//, AirportHeliportFields_Designator + AirportHeliportFields_Id + AirportHeliportFields_ElevatedPoint

		//	if (pADHPNameList == null)
		//	{
		//		ADHPList = new ADHPType[0];
		//		return -1;
		//	}

		//	n = pADHPNameList.Count;
		//	ADHPList = new ADHPType[n];

		//	if (n == 0)
		//		return -1;

		//	for (i = 0; i < n; i++)
		//	{
		//		pName = pADHPNameList[i];
		//		ADHPList[i].Name = pName.Name;
		//		ADHPList[i].Identifier = pName.Identifier;

		//		//if CheckILS 
		//		//	pAIXMILSList = pObjectDir.GetILSNavaidEquipmentList(pName.Identifier)
		//		//	T = 0
		//		//	if pAIXMILSList.Count > 0 
		//		//		For J = 0 To pAIXMILSList.Count - 1
		//		//			pAIXMNAVEq = pAIXMILSList(J)

		//		//			if (pAIXMNAVEq is Localizer) 
		//		//				T = T Or 1
		//		//			else if (pAIXMNAVEq is Glidepath) 
		//		//				T = T Or 2
		//		//			End if
		//		//			if T = 3  Exit For
		//		//		Next J
		//		//	End if

		//		//	ADHPList(I).Index = T
		//		//else
		//		//	ADHPList(I).Index = I
		//		//End if
		//		ADHPList[i].index = i;
		//	}

		//	return pADHPNameList.Count - 1;
		//}

		public static int FillADHPFields(ref ADHPType CurrADHP)
		{
			AirportHeliport pADHP;

			ESRI.ArcGIS.Geometry.IRelationalOperator pRelational;
			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;

			if (CurrADHP.pPtGeo != null)
				return 0;

			pADHP = pObjectDir.GetFeature(FeatureType.AirportHeliport, CurrADHP.Identifier) as Aran.Aim.Features.AirportHeliport;
			//CurrADHP.pAirportHeliport = pADHP;
			if (pADHP == null)
				return -1;

			pPtGeo = Converters.ARANPointToESRIPoint(pADHP.ARP.Geo);
			//    pPtGeo.X = pADHP.ARP.pos.DoubleList(0)
			//    pPtGeo.Y = pADHP.ARP.pos.DoubleList(1)

			//    if pADHP.ARP.pos.DoubleList.Count >= 3 
			//        pPtGeo.Z = pADHP.ARP.pos.DoubleList(2)
			//    else
			pPtGeo.Z = ConverterToSI.Convert(pADHP.ARP.Elevation, 0);
			//    End if

			pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
			if (pPtPrj.IsEmpty)
				return -1;

			pRelational = (ESRI.ArcGIS.Geometry.IRelationalOperator)GlobalVars.p_LicenseRect;
			if (!pRelational.Contains(pPtPrj))
				return -1;

			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			if (pADHP.MagneticVariation == null)
				CurrADHP.MagVar = 0.0;
			else
				CurrADHP.MagVar = pADHP.MagneticVariation.Value;

			//CurADHP.Elev = ConverterToSI.ConvertToSI(pElevPoint.Elevation, 0.0)
			//CurADHP.pPtGeo.Z = CurADHP.Elev
			//CurADHP.pPtPrj.Z = CurADHP.Elev

			//CurADHP.MinTMA = ConverterToSI.Convert(pADHP.transitionAltitude, 2500.0)
			//CurADHP.TransitionAltitude = ConverterToSI.ConvertToSI(ah.TransitionAltitude)

			CurrADHP.ISAtC = 15.0;	//ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}

		//static RunwayCentrelinePoint GetNearestPoint(CLPoint[] CLPointArray, ESRI.ArcGIS.Geometry.IPoint pPtGeo, double MaxDist = 5.0)
		//{
		//	ESRI.ArcGIS.Geometry.IPoint ptA = (ESRI.ArcGIS.Geometry.IPoint)Functions.ToPrj(pPtGeo);
		//	RunwayCentrelinePoint result = null;
		//	double fDist = MaxDist;

		//	for (int i = 0; i < CLPointArray.Length; i++)
		//		if (CLPointArray[i].pPtPrj != null && !CLPointArray[i].pPtPrj.IsEmpty)
		//		{
		//			double fTmp = Functions.ReturnDistanceInMeters(ptA, CLPointArray[i].pPtPrj);

		//			if (fTmp < fDist)
		//			{
		//				result = CLPointArray[i].pCLPoint;
		//				fDist = fTmp;
		//			}
		//		}

		//	return result;
		//}

#if Old
		public static int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
		{
			int iRwyNum, i, j, k;
			double ResX, ResY, fTmp, TrueBearing;
			//bool bRwyNum;

			Descriptor pName;
			CLPoint[] CLPointArray;
			List<Descriptor> pAIXMRWYList;
			List<RunwayDirection> pRwyDRList;

			ElevatedPoint pElevatedPoint;
			List<RunwayCentrelinePoint> pCenterLinePointList;

			Runway pRunway;
			RunwayDirection pRwyDirection;
			RunwayDirection pRwyDirectinPair;
			RunwayCentrelinePoint pRunwayCenterLinePoint;

			pAIXMRWYList = pObjectDir.GetRunwayList(Owner.Identifier);
			//, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

			iRwyNum = -1;
			if (pAIXMRWYList.Count == 0)
			{
				RWYList = new RWYType[0];
				return -1;
			}

			RWYList = new RWYType[2 * pAIXMRWYList.Count];

			//ResX = 0;
			//ResY = 0;
			//TrueBearing = 0;
			//fTmp = 0;

			for (i = 0; i < pAIXMRWYList.Count; i++)
			{
				pName = pAIXMRWYList[i];
				pRunway = (Runway)pObjectDir.GetFeature(FeatureType.Runway, pName.Identifier);
				pRwyDRList = pObjectDir.GetRunwayDirectionList(pRunway.Identifier);
				//bRwyNum = true;

				//if (pRwyDRList.Count == 2){

				for (j = 0; j < pRwyDRList.Count; j++)
				{
					iRwyNum++;

					//RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
					//if (RWYList[iRwyNum].Length < 0)
					//{
					//    iRwyNum--;
					//    break;
					//}

					pRwyDirection = pRwyDRList[j];
					pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);

					RWYList[iRwyNum].Initialize();
					//RWYList[iRwyNum].pRunwayDir = pRwyDirection;
					RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = null;

					CLPointArray = new CLPoint[pCenterLinePointList.Count];
					ESRI.ArcGIS.Geometry.IPoint ptDTresh = null;
					double fLDA = 0.0, fTORA = 0.0, fTODA = 0.0;

					for (k = 0; k < pCenterLinePointList.Count; k++)
					{
						pRunwayCenterLinePoint = pCenterLinePointList[k];

						if (pRunwayCenterLinePoint.Role != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;

							CLPointArray[k].pCLPoint = pRunwayCenterLinePoint;
							CLPointArray[k].pPtGeo = Converters.AixmPointToESRIPoint(pElevatedPoint);
							CLPointArray[k].pPtPrj = (IPoint)Functions.ToPrj(CLPointArray[k].pPtGeo);

							switch (pRunwayCenterLinePoint.Role.Value)//Select Case pRunwayCenterLinePoint.Role.Value
							{
								case CodeRunwayPointRole.START:
									RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);

									List<RunwayDeclaredDistance> pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;
									for (int L = 0; L < pDeclaredDistList.Count; L++)
									{
										RunwayDeclaredDistance pDirDeclaredDist = pDeclaredDistList[L];
										if (pDirDeclaredDist.DeclaredValue.Count > 0)
										{
											if (pDirDeclaredDist.Type == Aran.Aim.Enums.CodeDeclaredDistance.LDA)
												fLDA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, 0);
											else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TORA)
												fTORA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, 0);
											else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TODA)
												fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, 0);
										}
									}

									break;
								case CodeRunwayPointRole.THR:
									RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
								case CodeRunwayPointRole.END:
									RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
								case CodeRunwayPointRole.DISTHR:
									ptDTresh = CLPointArray[k].pPtGeo;
									ptDTresh.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
							}
						}
					}

					if (RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] == null && ptDTresh != null)
						RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = ptDTresh;

					for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtEnd; ek++)
					{
						if (RWYList[iRwyNum].pPtGeo[ek] == null)
						{
							//if (ek == eRWY.PtTHR)	continue;
							iRwyNum--;
							//bRwyNum = false;
							goto NextI;
						}
					}

					if (pRwyDirection.TrueBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
					else if (pRwyDirection.MagneticBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value - Owner.MagVar;
					else
					{
						NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, out TrueBearing, out fTmp);
						RWYList[iRwyNum].TrueBearing = TrueBearing;
					}

					//==============================================================================================================
					double dDT = fTORA - fLDA;
					if (dDT > 0.0)
					{
						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart]);
						if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = Converters.AixmPointToESRIPoint(pElevatedPoint);//ARANPointToESRIPoint(pElevatedPoint.Geo);
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
						}
						else
						{
							NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, dDT, RWYList[iRwyNum].TrueBearing + 180.0, out ResX, out ResY);

							RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = new ESRI.ArcGIS.Geometry.Point();
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart].PutCoords(ResX, ResY);

							pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart], 10000.0);
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

					//==============================================================================================================
					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);
					if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
					{
						pElevatedPoint = pRunwayCenterLinePoint.Location;
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = Converters.AixmPointToESRIPoint(pElevatedPoint);	//ARANPointToESRIPoint(pElevatedPoint.Geo)
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
					}
					else
					{
						NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, fLDA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = new ESRI.ArcGIS.Geometry.Point();
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);

						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd], 10000.0);
						if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
						}
						else
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = Owner.Elev;
					}

					RWYList[iRwyNum].ClearWay = fTODA - fTORA;
					if (RWYList[iRwyNum].ClearWay < 0.0)
						RWYList[iRwyNum].ClearWay = 0.0;

					if (RWYList[iRwyNum].ClearWay > 0.0)
					{
						NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);
					}

					//==============================================================================================================
					//RWYList[iRwyNum].ClearWay = 0.0;

					//pRunwayProtectAreaList = pObjectDir.GetRunwayProtectAreaList(pRwyDirection.Identifier);
					//for (k = 0; k < pRunwayProtectAreaList.Count; k++)
					//{
					//    pRunwayProtectArea = pRunwayProtectAreaList[k];
					//    pAirportHeliportProtectionArea = pRunwayProtectArea;
					//    if (pRunwayProtectArea.Type.Value == CodeRunwayProtectionArea.CWY && pAirportHeliportProtectionArea.Length != null)
					//    {
					//        RWYList[iRwyNum].ClearWay = ConverterToSI.Convert(pAirportHeliportProtectionArea.Length, 0.0);
					//        break;
					//    }
					//}

					//if (RWYList[iRwyNum].ClearWay > 0.0)
					//{
					//    NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
					//    RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);
					//}
					//*************************************************************************

					for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtEnd; ek++)
					{
						RWYList[iRwyNum].pPtPrj[ek] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[ek]) as IPoint;
						if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
						{
							iRwyNum--;
							break;
						}

						RWYList[iRwyNum].pPtGeo[ek].M = RWYList[iRwyNum].TrueBearing;
						RWYList[iRwyNum].pPtPrj[ek].M = Functions.Azt2Dir(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
					}

					RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
					if (RWYList[iRwyNum].Length < 0.0)
						RWYList[iRwyNum].Length = Functions.ReturnDistanceInMeters(RWYList[iRwyNum].pPtPrj[eRWY.PtEnd], RWYList[iRwyNum].pPtPrj[eRWY.PtStart]);

					RWYList[iRwyNum].Identifier = pRwyDirection.Identifier;
					RWYList[iRwyNum].Name = pRwyDirection.Designator;
					RWYList[iRwyNum].ADHP_ID = Owner.Identifier;
					//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

					pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
					RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
					RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;
				}			//}
			NextI: ;
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}
#endif

		public static int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
		{
			Descriptor pName;
			List<Descriptor> pAIXMRWYList;
			List<RunwayDirection> pRwyDRList;
			ElevatedPoint pElevatedPoint;
			List<RunwayCentrelinePoint> pCenterLinePointList;

			Runway pRunway;
			RunwayDirection pRwyDirection;
			RunwayDirection pRwyDirectinPair;
			RunwayCentrelinePoint pRunwayCenterLinePoint;

			//AirportHeliportProtectionArea pAirportHeliportProtectionArea;
			//List<RunwayProtectArea> pRunwayProtectAreaList;
			//RunwayProtectArea pRunwayProtectArea;

			double fLDA, fTORA, fTODA, fDTHR;
			Guid pTmpRunwayCenterLinePointID = Guid.Empty;
			pAIXMRWYList = pObjectDir.GetRunwayList(Owner.Identifier);	//, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

			if (pAIXMRWYList.Count == 0)
			{
				RWYList = new RWYType[0];
				return -1;
			}

			RWYList = new RWYType[2 * pAIXMRWYList.Count];

			int iRwyNum = -1;

			for (int i = 0; i < pAIXMRWYList.Count; i++)
			{
				pName = pAIXMRWYList[i];
				pRunway = (Runway)pObjectDir.GetFeature(FeatureType.Runway, pName.Identifier);

				pRwyDRList = pObjectDir.GetRunwayDirectionList(pRunway.Identifier);

				for (int j = 0; j < pRwyDRList.Count; j++)
				{
					iRwyNum++;

					RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);

					if (RWYList[iRwyNum].Length < 0)
					{
						iRwyNum--;
						continue;
					}

					pRwyDirection = pRwyDRList[j];
					RWYList[iRwyNum].Initialize();
					List<RunwayDeclaredDistance> pDeclaredDistList = null;
					double PtEndZ = 0.0;

					ESRI.ArcGIS.Geometry.IPoint ptDTresh = null;

					pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
					for (int k = 0; k < pCenterLinePointList.Count; k++)
					{
						pRunwayCenterLinePoint = pCenterLinePointList[k];
						pElevatedPoint = pRunwayCenterLinePoint.Location;


						if (pElevatedPoint == null || pRunwayCenterLinePoint.Role == null)
							continue;

						switch (pRunwayCenterLinePoint.Role.Value)
						{
							case CodeRunwayPointRole.START:
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = Converters.AixmPointToESRIPoint(pElevatedPoint);
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								RWYList[iRwyNum].pSignificantPointID[eRWY.PtStart] = pRunwayCenterLinePoint.Identifier;
								RWYList[iRwyNum].StartHorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0);

								pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

								break;
							case CodeRunwayPointRole.THR:
								RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = Converters.AixmPointToESRIPoint(pElevatedPoint);
								RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								RWYList[iRwyNum].pSignificantPointID[eRWY.PtTHR] = pRunwayCenterLinePoint.Identifier;

								break;
							case CodeRunwayPointRole.DISTHR:
								ptDTresh = Converters.AixmPointToESRIPoint(pElevatedPoint);
								ptDTresh.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								pTmpRunwayCenterLinePointID = pRunwayCenterLinePoint.Identifier;

								break;
							case CodeRunwayPointRole.END:
								RWYList[iRwyNum].pPtGeo[eRWY.PtDER] = Converters.AixmPointToESRIPoint(pElevatedPoint);
								PtEndZ = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								//RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
								RWYList[iRwyNum].pSignificantPointID[eRWY.PtDER] = pRunwayCenterLinePoint.Identifier;

								break;
						}
					}

					if (ptDTresh != null)       //RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] == null &&
					{
						RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = ptDTresh;
						RWYList[iRwyNum].pSignificantPointID[eRWY.PtTHR] = pTmpRunwayCenterLinePointID;
					}

					//for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtDER; ek++)
					//{
					//	if (RWYList[iRwyNum].pPtGeo[ek] == null)
					//	{
					//		//if (ek == eRWY.PtTHR)									continue;
					//		iRwyNum--;
					//		goto NextI;
					//	}
					//}

					if (RWYList[iRwyNum].pPtGeo[eRWY.PtStart] == null || RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] == null || RWYList[iRwyNum].pPtGeo[eRWY.PtDER] == null)
					{
						iRwyNum--;
						continue;
					}

					if (pRwyDirection.TrueBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
					else if (pRwyDirection.MagneticBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value - Owner.MagVar;
					else
					{
						double TrueBearing, fTmp;
						NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.PtDER].X, RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Y, out TrueBearing, out fTmp);
						RWYList[iRwyNum].TrueBearing = TrueBearing;
					}

					double fTODAacc = 0;
					fLDA = fTORA = fTODA = fDTHR = -1;

					if (pDeclaredDistList != null)
					{
						for (int k = 0; k < pDeclaredDistList.Count; k++)
						{
							RunwayDeclaredDistance pDirDeclaredDist = pDeclaredDistList[k];
							if (pDirDeclaredDist.DeclaredValue.Count > 0)
							{
								if (pDirDeclaredDist.Type == CodeDeclaredDistance.LDA)
									fLDA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fLDA);
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TORA)
									fTORA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fTORA);
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TODA)
								{
									fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fTODA);
									fTODAacc = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].DistanceAccuracy, 0);
								}
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.DTHR)
									fDTHR = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fDTHR);
							}
						}
					}

					//List<RunwayDeclaredDistance> GetDeclaredDistance(Guid RCLPIdentifier);
					//DeclaredDistance pDirDeclaredDist;
					//pDeclaredDistList  = pObjectDir.g GetDeclaredDistanceList(pRunway.Identifier);

					if (fTODA < 0.0 )
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
						iRwyNum--;
						continue;
					}

					RWYList[iRwyNum].TODA = fTODA;
					RWYList[iRwyNum].TODAAccuracy = fTODAacc;

					double ResX, ResY;
					NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].TODA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);

					RWYList[iRwyNum].pPtGeo[eRWY.PtDER] = new ESRI.ArcGIS.Geometry.Point();
					RWYList[iRwyNum].pPtGeo[eRWY.PtDER].PutCoords(ResX, ResY);

					RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Z = PtEndZ;				// RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z;
					RWYList[iRwyNum].ClearWay = fTODA - fTORA;


					for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtDER; ek++)
					{
						RWYList[iRwyNum].pPtPrj[ek] = (IPoint)Functions.ToPrj(RWYList[iRwyNum].pPtGeo[ek]);
						if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
						{
							iRwyNum--;
							goto NextI;
						}

						RWYList[iRwyNum].pPtGeo[ek].M = RWYList[iRwyNum].TrueBearing;
						RWYList[iRwyNum].pPtPrj[ek].M = Functions.Azt2Dir(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
					}

					RWYList[iRwyNum].Identifier = pRwyDirection.Identifier;
					RWYList[iRwyNum].Name = pRwyDirection.Designator;
					RWYList[iRwyNum].ADHP_ID = Owner.Identifier;
					//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

					pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
					RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
					RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;
				NextI: ;
				}
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		//public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
		//{
		//	int i, n;

		//	n = NavaidList.GetLength(0);

		//	for (i = 0; i < n; i++)
		//		if ((NavaidList[i].TypeCode == eNavaidType.LLZ) && (NavaidList[i].CallSign == ILS.CallSign))
		//			return i;

		//	System.Array.Resize<NavaidType>(ref NavaidList, n + 1);

		//	NavaidList[n].pPtGeo = ILS.pPtGeo;
		//	NavaidList[n].pPtPrj = ILS.pPtPrj;
		//	NavaidList[n].Name = ILS.CallSign;
		//	NavaidList[n].NAV_Ident = ILS.NAV_Ident;
		//	NavaidList[n].Identifier = ILS.Identifier;

		//	NavaidList[n].CallSign = ILS.CallSign;

		//	NavaidList[n].MagVar = ILS.MagVar;
		//	NavaidList[n].TypeCode = eNavaidType.LLZ;
		//	NavaidList[n].Range = 40000.0;
		//	NavaidList[n].index = ILS.index;
		//	NavaidList[n].PairNavaidIndex = -1;

		//	NavaidList[n].GPAngle = ILS.GPAngle;
		//	NavaidList[n].GP_RDH = ILS.GP_RDH;

		//	NavaidList[n].Course = ILS.Course;
		//	NavaidList[n].LLZ_THR = ILS.LLZ_THR;
		//	NavaidList[n].SecWidth = ILS.SecWidth;
		//	NavaidList[n].AngleWidth = ILS.AngleWidth;
		//	NavaidList[n].Tag = 0;
		//	//NavaidList[n].pFeature = ILS.pFeature;

		//	return n + 1;
		//}

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

		//public static long GetILSByName(string ProcName, ADHPType ADHP, ref ILSType ILS)
		//{
		//	char chr;
		//	int i, n, pos;
		//	string RWYName;
		//	RWYType[] RWYList;

		//	pos = ProcName.IndexOf("RWY");

		//	if (pos <= 0)
		//		return 0;

		//	pos += 3;
		//	RWYName = "";

		//	chr = ProcName.ToCharArray(pos, 1)[0];
		//	while (chr != ' ')
		//	{
		//		RWYName = RWYName + chr;
		//		pos++;
		//		chr = ProcName.ToCharArray(pos, 1)[0];
		//	}

		//	//    ADHP.ID = OwnerID
		//	//    if FillADHPFields(ADHP) < 0  Exit Function

		//	n = FillRWYList(out RWYList, ADHP);

		//	for (i = 0; i < n; i++)
		//		if (RWYList[i].Name == RWYName)
		//			return GetILS(RWYList[i], ref ILS, ADHP);

		//	return 0;

		//	//    if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		//}

		//public static long GetILS(RWYType RWY, ref ILSType ILS, ADHPType Owner)
		//{
		//	int i, j;
		//	double dX, dY;

		//	Navaid pNavaid;
		//	List<NavaidComponent> pAIXMNAVEqList;

		//	NavaidEquipment pAIXMNAVEq;
		//	Localizer pAIXMLocalizer;
		//	Glidepath pAIXMGlidepath;
		//	ElevatedPoint pElevPoint;

		//	ILS.index = 0;

		//	//if RWY.ILSID = ""  Exit Function

		//	pNavaid = pObjectDir.GetILSNavaid(RWY.Identifier);
		//	if (pNavaid == null)
		//		return 0;

		//	pAIXMNAVEqList = pNavaid.NavaidEquipment;
		//	if (pAIXMNAVEqList.Count == 0)
		//		return 0;

		//	ILS.Category = 4;
		//	ILS.NAV_Ident = pNavaid.Identifier;

		//	if (pNavaid.SignalPerformance != null)
		//		ILS.Category = (int)pNavaid.SignalPerformance.Value + 1;

		//	if (ILS.Category > 3)
		//		ILS.Category = 1;

		//	ILS.RWY_ID = RWY.Identifier;

		//	pAIXMLocalizer = null;
		//	pAIXMGlidepath = null;

		//	j = 0;

		//	for (i = 0; i < pAIXMNAVEqList.Count; i++)
		//	{
		//		pAIXMNAVEq = (NavaidEquipment)pAIXMNAVEqList[i].TheNavaidEquipment.GetFeature();

		//		if (pAIXMNAVEq is Localizer)
		//		{
		//			pAIXMLocalizer = (Localizer)pAIXMNAVEq;
		//			j |= 1;
		//		}
		//		else if (pAIXMNAVEq is Glidepath)
		//		{
		//			pAIXMGlidepath = (Glidepath)pAIXMNAVEq;
		//			j |= 2;
		//		}

		//		if (j == 3)
		//			break;
		//	}

		//	//    Set pAIXMNAVEq = pAIXMNAVEqList.Item(0).TheNavaidEquipment
		//	//    Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(0).TheNavaidEquipment).Cast()

		//	//    if (pAIXMNAVEq is LocalizerCom) 
		//	//        Set pAIXMLocalizer = pAIXMNAVEq
		//	//    else if (pAIXMNAVEq is GlidepathCom) 
		//	//        Set pAIXMGlidepath = pAIXMNAVEq
		//	//    End if

		//	//    if pAIXMNAVEqList.Count > 1 
		//	//        Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(1).TheNavaidEquipment).Cast()
		//	//        'Set pAIXMNAVEq = pAIXMNAVEqList.Item(1).TheNavaidEquipment
		//	//        if (pAIXMNAVEq is LocalizerCom) 
		//	//            Set pAIXMLocalizer = pAIXMNAVEq
		//	//        else if (pAIXMNAVEq is GlidepathCom) 
		//	//            Set pAIXMGlidepath = pAIXMNAVEq
		//	//        End if
		//	//    End if

		//	if (pAIXMLocalizer != null)
		//	{
		//		pAIXMNAVEq = (NavaidEquipment)pAIXMLocalizer;
		//		pElevPoint = pAIXMNAVEq.Location;

		//		if (pAIXMNAVEq.MagneticVariation != null)
		//			ILS.MagVar = (double)pAIXMNAVEq.MagneticVariation.Value;
		//		else if (pAIXMLocalizer.TrueBearing != null && pAIXMLocalizer.MagneticBearing != null)
		//			ILS.MagVar = (double)(pAIXMLocalizer.MagneticBearing.Value - pAIXMLocalizer.TrueBearing.Value);
		//		else
		//			ILS.MagVar = Owner.MagVar;

		//		if (pAIXMLocalizer.TrueBearing != null)
		//			ILS.Course = pAIXMLocalizer.TrueBearing.Value;
		//		else if (pAIXMLocalizer.MagneticBearing != null)
		//			ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
		//		else
		//			goto NoLocalizer;

		//		//			ILS.Course = Modulus(ILS.Course + 180.0, 360.0)

		//		ILS.pPtGeo = Converters.ARANPointToESRIPoint(pElevPoint.Geo);
		//		ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.PtTHR].Z);
		//		ILS.pPtGeo.M = ILS.Course;

		//		ILS.pPtPrj = Functions.ToPrj(ILS.pPtGeo) as IPoint;

		//		if (ILS.pPtPrj.IsEmpty)
		//			return 0;

		//		ILS.pPtPrj.M = Functions.Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M);

		//		dX = RWY.pPtPrj[eRWY.PtTHR].X - ILS.pPtPrj.X;
		//		dY = RWY.pPtPrj[eRWY.PtTHR].Y - ILS.pPtPrj.Y;
		//		ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

		//		if (pAIXMLocalizer.WidthCourse != null)
		//		{
		//			ILS.AngleWidth = (double)pAIXMLocalizer.WidthCourse.Value;
		//			ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(Functions.DegToRad(ILS.AngleWidth));

		//			ILS.index = 1;
		//			ILS.Identifier = pAIXMNAVEq.Identifier;
		//			ILS.CallSign = pAIXMNAVEq.Designator;	//pNavaid.Designator '

		//			//        ID = pAIXMNAVEq.Identifier
		//			//        CallSign = pAIXMNAVEq.Designator
		//			//    else
		//			//        Exit Function
		//		}
		//	}
		//NoLocalizer:

		//	if (pAIXMGlidepath != null)
		//	{
		//		pAIXMNAVEq = pAIXMGlidepath;

		//		if (pAIXMGlidepath.Slope != null)
		//		{
		//			ILS.GPAngle = pAIXMGlidepath.Slope.Value;

		//			if (pAIXMGlidepath.Rdh != null)
		//			{
		//				ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, ILS.pPtGeo.Z);
		//				ILS.index = ILS.index | 2;

		//				if (ILS.index == 2)
		//				{
		//					ILS.Identifier = pAIXMNAVEq.Identifier;
		//					ILS.CallSign = pAIXMNAVEq.Designator;
		//				}
		//			}
		//		}
		//	}

		//	return ILS.index;
		//}

		public static void FillNavaidList(out NavaidType[] NavaidList, out NavaidType[] DMEList, ADHPType CurrADHP, double Radius)
		{
			int i, j, iNavaidNum, iDMENum;

			double fDist;

			eNavaidType NavTypeCode;
			List<Navaid> pNavaidList;

			Navaid pNavaid;
			NavaidEquipment AixmNavaidEquipment;

			IPolygon pPolygon;
			IPoint pPtGeo;
			IPoint pPtPrj;

			MultiPolygon pARANPolygon;
			ElevatedPoint pElevPoint;

			pPolygon = (ESRI.ArcGIS.Geometry.IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, Radius);
			pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(pPolygon) as IPolygon);

			pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

			// .GetNavaidEquipmentList (pARANPolygon)
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

			for (j = 0; j < pNavaidList.Count; j++)
			{
				pNavaid = pNavaidList[j];
				for (i = 0; i < pNavaid.NavaidEquipment.Count; i++)
				{
					if (pNavaid.NavaidEquipment[i] == null)
						continue;

					if (pNavaid.NavaidEquipment[i].TheNavaidEquipment == null)
						continue;

					AixmNavaidEquipment = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();
					if (AixmNavaidEquipment == null)
						continue;

					if (AixmNavaidEquipment.NavaidEquipmentType == NavaidEquipmentType.VOR)
						NavTypeCode = eNavaidType.VOR;
					else if (AixmNavaidEquipment.NavaidEquipmentType == NavaidEquipmentType.DME)
						NavTypeCode = eNavaidType.DME;
					else if (AixmNavaidEquipment.NavaidEquipmentType == NavaidEquipmentType.NDB)
						NavTypeCode = eNavaidType.NDB;
					else if (AixmNavaidEquipment.NavaidEquipmentType == NavaidEquipmentType.TACAN)
						NavTypeCode = eNavaidType.TACAN;
					else
						continue;

					//if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.VOR)
					//	NavTypeCode = eNavaidType.VOR;
					//else if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.DME)
					//	NavTypeCode = eNavaidType.DME;
					//else if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.NDB)
					//	NavTypeCode = eNavaidType.NDB;
					//else if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.TACAN)
					//	NavTypeCode = eNavaidType.TACAN;
					//else
					//	continue;

					pElevPoint = AixmNavaidEquipment.Location;
					if (pElevPoint == null)
						continue;

					pPtGeo = Converters.ARANPointToESRIPoint(pElevPoint.Geo);
					pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev);
					pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;

					if (pPtPrj.IsEmpty)
						continue;

					if (NavTypeCode == eNavaidType.DME)
					{
						iDMENum++;

						DMEList[iDMENum].pPtGeo = pPtGeo;
						DMEList[iDMENum].pPtPrj = pPtPrj;
						DMEList[iDMENum].HorAccuracy = ConverterToSI.Convert(pElevPoint.HorizontalAccuracy, 0);

						if (AixmNavaidEquipment.MagneticVariation != null)
							DMEList[iDMENum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
						else
							DMEList[iDMENum].MagVar = CurrADHP.MagVar;

						if (((DME)AixmNavaidEquipment).Displace != null)
							DMEList[iDMENum].Disp = ConverterToSI.Convert(((DME)AixmNavaidEquipment).Displace, 0);
						else
							DMEList[iDMENum].Disp = 0.0;

						DMEList[iDMENum].Range = 350000.0; //DME.Range
						DMEList[iDMENum].PairNavaidIndex = -1;

						DMEList[iDMENum].Name = AixmNavaidEquipment.Name;

						DMEList[iDMENum].NAV_Ident = pNavaid.Identifier;
						DMEList[iDMENum].Identifier = AixmNavaidEquipment.Identifier;
						DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

						DMEList[iDMENum].TypeCode = NavTypeCode;
						DMEList[iDMENum].index = iNavaidNum + iDMENum;
					}
					else
					{
						iNavaidNum++;

						NavaidList[iNavaidNum].pPtGeo = pPtGeo;
						NavaidList[iNavaidNum].pPtPrj = pPtPrj;
						NavaidList[iNavaidNum].HorAccuracy = ConverterToSI.Convert(pElevPoint.HorizontalAccuracy, 0);

						if (AixmNavaidEquipment.MagneticVariation != null)
							NavaidList[iNavaidNum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
						else
							NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar;

						if (NavTypeCode == eNavaidType.NDB)
							NavaidList[iNavaidNum].Range = 350000.0;		//NDB.Range
						else
							NavaidList[iNavaidNum].Range = 350000.0;		//VOR.Range

						NavaidList[iNavaidNum].PairNavaidIndex = -1;

						NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name;

						NavaidList[iNavaidNum].NAV_Ident = pNavaid.Identifier;
						NavaidList[iNavaidNum].Identifier = AixmNavaidEquipment.Identifier;
						NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator;

						NavaidList[iNavaidNum].TypeCode = NavTypeCode;
						NavaidList[iNavaidNum].index = iNavaidNum + iDMENum;
					}
				}
			}

			for (j = 0; j <= iNavaidNum; j++)
				for (i = 0; i <= iDMENum; i++)
				{
					fDist = Functions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
					if (fDist <= 2.0)
					{
						NavaidList[j].PairNavaidIndex = i;
						DMEList[i].PairNavaidIndex = j;
						break;
					}
				}

			System.Array.Resize<NavaidType>(ref NavaidList, iNavaidNum + 1);
			System.Array.Resize<NavaidType>(ref DMEList, iDMENum + 1);
		}

		public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, ADHPType CurrADHP, double radius)
		{
			int iWPTNum, i, j, n;

			eNavaidType NavTypeCode;

			List<DesignatedPoint> AIXMWPTList;
			List<Navaid> pNavaidList;

			Navaid pNavaid;
			NavaidEquipment AIXMNAVEq;
			DesignatedPoint AIXMWPT;

			IPolygon pPolygon;
			MultiPolygon pARANPolygon;

			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;

			pPolygon = (ESRI.ArcGIS.Geometry.IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, radius);
			pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(pPolygon) as IPolygon);

			AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon);
			pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

			n = AIXMWPTList.Count + 2 * pNavaidList.Count;
			if (n == 0)
			{
				WPTList = new WPT_FIXType[0];
				return -1;
			}

			iWPTNum = -1;
			WPTList = new WPT_FIXType[n];

			for (i = 0; i < AIXMWPTList.Count; i++)
			{
				AIXMWPT = AIXMWPTList[i];
				if (AIXMWPT.Designator == null)
					continue;

				pPtGeo = Converters.ARANPointToESRIPoint(AIXMWPT.Location.Geo);
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
				WPTList[iWPTNum].HorAccuracy = ConverterToSI.Convert(AIXMWPT.Location.HorizontalAccuracy, 0);

				WPTList[iWPTNum].Name = AIXMWPT.Designator;
				WPTList[iWPTNum].CallSign = AIXMWPT.Designator;
				WPTList[iWPTNum].NAV_Ident = AIXMWPT.Identifier;
				WPTList[iWPTNum].Identifier = AIXMWPT.Identifier;

				//WPTList[iWPTNum].pFeature = AIXMWPT;

				WPTList[iWPTNum].TypeCode = eNavaidType.NONE;
			}

			//======================================================================

			for (j = 0; j < pNavaidList.Count; j++)
			{
				pNavaid = pNavaidList[j];
				for (i = 0; i < pNavaid.NavaidEquipment.Count; i++)
				{
					if (pNavaid.NavaidEquipment[i] == null)
						continue;

					if (pNavaid.NavaidEquipment[i].TheNavaidEquipment == null)
						continue;

					AIXMNAVEq = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();
					if (AIXMNAVEq == null || AIXMNAVEq.Location == null)
						continue;

					if (AIXMNAVEq.NavaidEquipmentType == NavaidEquipmentType.VOR)
						NavTypeCode = eNavaidType.VOR;
					else if (AIXMNAVEq.NavaidEquipmentType == NavaidEquipmentType.NDB)
						NavTypeCode = eNavaidType.NDB;
					else if (AIXMNAVEq.NavaidEquipmentType == NavaidEquipmentType.TACAN)
						NavTypeCode = eNavaidType.TACAN;
					else
						continue;

					//if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.VOR)
					//	NavTypeCode = eNavaidType.VOR;
					//else if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.NDB)
					//	NavTypeCode = eNavaidType.NDB;
					//else if (pNavaid.NavaidEquipment[i].TheNavaidEquipment.Type == NavaidEquipmentType.TACAN)
					//	NavTypeCode = eNavaidType.TACAN;
					//else
					//	continue;

					pPtGeo = Converters.ARANPointToESRIPoint(AIXMNAVEq.Location.Geo);
					pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, CurrADHP.Elev);

					pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
					if (pPtPrj.IsEmpty)
						continue;

					iWPTNum++;

					WPTList[iWPTNum].pPtGeo = pPtGeo;
					WPTList[iWPTNum].pPtPrj = pPtPrj;
					WPTList[iWPTNum].HorAccuracy = ConverterToSI.Convert(AIXMNAVEq.Location.HorizontalAccuracy, 0);

					if (AIXMNAVEq.MagneticVariation != null)
						WPTList[iWPTNum].MagVar = (double)AIXMNAVEq.MagneticVariation.Value;
					else
						WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

					WPTList[iWPTNum].Name = AIXMNAVEq.Designator;
					WPTList[iWPTNum].NAV_Ident = pNavaid.Identifier;
					WPTList[iWPTNum].Identifier = AIXMNAVEq.Identifier;

					WPTList[iWPTNum].TypeCode = NavTypeCode;
				}
			}
			//======================================================================
			iWPTNum++;
			System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

			System.Array.Sort<WPT_FIXType>(WPTList,
				(delegate (WPT_FIXType a, WPT_FIXType b)
				{
					return a.ToString().CompareTo(b.ToString());
				}));

			return iWPTNum;
		}

		public static double GetObstListInPoly(out ObstacleContainer ObstList, IPoint ptCenter, double radius, double fRefHeight = 0.0)
		{
			if (GlobalVars.settings.AnnexObstalce)
				return DBModule.GetAnnexObstacle(out ObstList, GlobalVars.CurrADHP.Identifier, ptCenter, fRefHeight);
			else
				return DBModule.GetObstaclesByDist(out ObstList, ptCenter, radius, fRefHeight);
		}

		public static double GetObstaclesByDist(out ObstacleContainer obstList, IPoint ptCenter, double radius, double fRefHeight)
		{
			IPolygon pPoly = Functions.CreatePrjCircle(ptCenter, radius);

			MultiPolygon pARANPolygon = Converters.ESRIPolygonToARANPolygon((IPolygon)Functions.ToGeo(pPoly));
			List<VerticalStructure> VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);

			//Functions.DrawPolygon(pPoly, -1, ESRI.ArcGIS.Display.esriSimpleFillStyle.esriSFSDiagonalCross);
			//System.Windows.Forms.Application.DoEvents();

			return ProcessObstacles(out obstList, VerticalStructureList, ptCenter, fRefHeight);
		}

		//public static double GetObstListInPoly(out ObstacleContainer ObstList, IPolygon pPoly, IPoint ptCenter, double fRefHeight = 0.0)

		public static double GetAnnexObstacle(out ObstacleContainer obstList, Guid Identifier, IPoint ptCenter, double fRefHeight)
		{
			List<VerticalStructure> VerticalStructureList = pObjectDir.GetAnnexVerticalStructureList(Identifier);
			return ProcessObstacles(out obstList, VerticalStructureList, ptCenter, fRefHeight);
		}

		public static double ProcessObstacles(out ObstacleContainer ObstList, List<VerticalStructure> VerticalStructureList, IPoint ptCenter, double fRefHeight)
		{
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;
			ElevatedCurve pElevatedCurve;
			ElevatedSurface pElevatedSurface;

			int n = VerticalStructureList.Count;
			ObstList.Parts = new ObstacleData[0];
			ObstList.Obstacles = new Obstacle[n];

			if (n == 0)
				return 0.0;

			ESRI.ArcGIS.Geometry.IZ pZv;
			ESRI.ArcGIS.Geometry.IZAware pZAware;
			ESRI.ArcGIS.Geometry.ITopologicalOperator pTopop;
			ESRI.ArcGIS.Geometry.ITopologicalOperator2 pTopo;
			ESRI.ArcGIS.Geometry.IProximityOperator pProxy = (ESRI.ArcGIS.Geometry.IProximityOperator)ptCenter;

			int C = n;
			int k = -1;

			double MaxDist, Z, HorAccuracy, VertAccuracy;
			MaxDist = Z = HorAccuracy = VertAccuracy = 0.0;
			ESRI.ArcGIS.Geometry.IGeometry pGeomGeo = null, pGeomPrj;

			for (int i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];
				int m = AixmObstacle.Part.Count;

				for (int j = 0; j < m; j++)
				{
					if (AixmObstacle.Part[j].HorizontalProjection == null)
						continue;

					VerticalStructurePart ObstaclePart = AixmObstacle.Part[j];

					switch (ObstaclePart.HorizontalProjection.Choice)
					{
						case VerticalStructurePartGeometryChoice.ElevatedPoint:
							pElevatedPoint = ObstaclePart.HorizontalProjection.Location;
							if (pElevatedPoint == null) continue;
							if (pElevatedPoint.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

							pGeomGeo = Converters.AixmPointToESRIPoint(pElevatedPoint);
							Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent;
							if (pElevatedCurve == null) continue;
							if (pElevatedCurve.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

							pGeomGeo = ConvertToEsriGeom.FromMultiLineString(pElevatedCurve.Geo);
							Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							//continue;

							pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
							if (pElevatedSurface == null) continue;
							if (pElevatedSurface.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

							pGeomGeo = ConvertToEsriGeom.FromMultiPolygon(pElevatedSurface.Geo);
							Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
							break;
						default:
							continue;
					}

					pGeomGeo.SpatialReference = GlobalVars.pSpRefShp;
					pGeomPrj = Functions.ToPrj(pGeomGeo);
					if (pGeomPrj.IsEmpty) continue;

					if (VertAccuracy > 0.0)
						Z += VertAccuracy;

					if (false) //(HorAccuracy > 0.0)//HorAccuracy = 0.0;
					{
						if (pGeomPrj.GeometryType == esriGeometryType.esriGeometryPoint && (HorAccuracy <= 2.0))
							pGeomPrj = Functions.CreatePrjCircle((IPoint)pGeomPrj, HorAccuracy, 18);
						else
						{
							pTopop = (ITopologicalOperator)pGeomPrj;
							pTopop.Simplify();
							pGeomPrj = pTopop.Buffer(HorAccuracy);

							pTopo = (ITopologicalOperator2)pGeomPrj;
							pTopo.IsKnownSimple_2 = false;
							pTopo.Simplify();
						}

						pGeomGeo = Functions.ToGeo(pGeomPrj);
					}

					pZAware = (IZAware)pGeomGeo;
					pZAware.ZAware = true;

					pZAware = (IZAware)pGeomPrj;
					pZAware.ZAware = true;

					if (pGeomGeo.GeometryType == esriGeometryType.esriGeometryPoint)
					{
						(pGeomGeo as ESRI.ArcGIS.Geometry.IPoint).Z = Z;
						(pGeomPrj as ESRI.ArcGIS.Geometry.IPoint).Z = Z;
					}
					else
					{
						pZv = (IZ)pGeomGeo;
						pZv.SetConstantZ(Z);

						pZv = (IZ)pGeomPrj;
						pZv.SetConstantZ(Z);
					}

					k++;

					if (k >= C)
					{
						C += n;
						Array.Resize<Obstacle>(ref ObstList.Obstacles, C);
					}

					ObstList.Obstacles[k].pGeomGeo = pGeomGeo;
					ObstList.Obstacles[k].pGeomPrj = pGeomPrj;
					ObstList.Obstacles[k].UnicalName = AixmObstacle.Name;
					if (AixmObstacle.Type == null)
						ObstList.Obstacles[k].TypeName = "";
					else
						ObstList.Obstacles[k].TypeName = AixmObstacle.Type.ToString();

					ObstList.Obstacles[k].Identifier = AixmObstacle.Identifier;
					ObstList.Obstacles[k].ID = AixmObstacle.Id;

					ObstList.Obstacles[k].HorAccuracy = HorAccuracy;
					ObstList.Obstacles[k].VertAccuracy = VertAccuracy;

					ObstList.Obstacles[k].Height = Z - fRefHeight;
					ObstList.Obstacles[k].Index = k;

					double fDist = pProxy.ReturnDistance(pGeomPrj);
					if (fDist > MaxDist)
						MaxDist = fDist;
					//}
					//catch
					//{
					//}
				}
			}

			k++;
			System.Array.Resize<Obstacle>(ref ObstList.Obstacles, k);

			return MaxDist;
		}

		//======================================================
		public static Feature CreateDesignatedPoint(IPoint pPtPrj, string Name = "COORD", double fDir = -1000.0, double horAccuracy = 0.10)
		{
			WPT_FIXType WptFIX = new WPT_FIXType();
			DesignatedPoint pFixDesignatedPoint;
			double fMinDist = 10000.0;
			int n = GlobalVars.WPTList.Length;

			for (int i = 0; i < n; i++)
			{
				double fDist = Functions.ReturnDistanceInMeters(pPtPrj, GlobalVars.WPTList[i].pPtPrj);

				if (fDist < fMinDist)
				{
					fMinDist = fDist;
					WptFIX = GlobalVars.WPTList[i];

					if (fMinDist == 0.0)
						break;
				}
			}

			bool bExist = fMinDist <= 25.0;

			if (!bExist && fMinDist <= 100.0 && fDir != -1000.0)
			{
				double fDirToPt = Functions.ReturnAngleInDegrees(pPtPrj, WptFIX.pPtPrj);
				bExist = Functions.SubtractAngles(fDir, fDirToPt) < 0.1;
			}

			if (bExist)
				return WptFIX.GetFeature();

			pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature<DesignatedPoint>();
			if (Name == "")
				Name = "COORD";
			pFixDesignatedPoint.Designator = Name;
			pFixDesignatedPoint.Name = Name;

			pFixDesignatedPoint.Location = Converters.ESRIPointToAixmPoint(Functions.ToGeo(pPtPrj) as IPoint);
			if (horAccuracy > 0.0)
				pFixDesignatedPoint.Location.HorizontalAccuracy = CreateValDistanceType(UomDistance.M, horAccuracy);

			//pFixDesignatedPoint.Note;
			//pFixDesignatedPoint.Tag;
			pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.DESIGNED;
			return pFixDesignatedPoint;
		}

		public static AngleIndication CreateAngleIndication(Double Angle, Aran.Aim.Enums.CodeBearing AngleType, SignificantPoint pSignificantPoint)
		{
			AngleIndication pAngleIndication;

			pAngleIndication = DBModule.pObjectDir.CreateFeature<AngleIndication>();
			pAngleIndication.Angle = Angle;
			pAngleIndication.AngleType = AngleType;
			pAngleIndication.PointChoice = pSignificantPoint;

			return pAngleIndication;
		}

		public static DistanceIndication CreateDistanceIndication(Double Distance, UomDistance Uom, SignificantPoint pSignificantPoint)
		{
			DistanceIndication pDistanceIndication;
			ValDistance pDistance;

			pDistanceIndication = DBModule.pObjectDir.CreateFeature<DistanceIndication>();

			pDistance = new ValDistance();
			pDistance.Uom = Uom;
			pDistance.Value = Distance;

			pDistanceIndication.Distance = pDistance;
			pDistanceIndication.PointChoice = pSignificantPoint;

			return pDistanceIndication;
		}

		private static bool isOpen = false;

		public static void InitModule()	//As String
		{
			if (!isOpen)
			{
				Aran.Aim.Data.DbProvider dbPro = (Aran.Aim.Data.DbProvider)GlobalVars.gAranEnv.DbProvider;

				pObjectDir = PandaSQPIFactory.Create();
				Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;

				pObjectDir.Open(dbPro);

				var terrainDataReaderHandler = GlobalVars.gAranEnv.CommonData.GetObject("terrainDataReader") as TerrainDataReaderEventHandler;
				if (terrainDataReaderHandler != null)
					pObjectDir.TerrainDataReader += terrainDataReaderHandler;

				GlobalVars.UserName = dbPro.CurrentUser.Name;

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
