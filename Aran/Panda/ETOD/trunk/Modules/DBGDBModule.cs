//3186801021

using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Converters;
using Aran.Geometries;
using Aran.Aim.CAWProvider;

using Aran.Queries;
using Aran.Queries.Panda_2;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;

using TestAranGdbProvider;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBGDBModule
	{
		public static GDBProvider pGDBProvider;

		public static int FillOrganisationList(out OrganisationType[] OrganisationList)
		{
			OrganisationList = GlobalVars.InitArray<OrganisationType>(0);
			List<OrganisationAuthority> pOrganisationList = pGDBProvider.GetOrganisationList();

			if (pOrganisationList == null)
				return -1;

			int n = pOrganisationList.Count;

			if (n == 0)
				return -1;

			System.Array.Resize(ref OrganisationList, n);

			for (int i = 0; i < n; i++)
			{
				OrganisationList[i].pOrganisationAuthority = pOrganisationList[i];
				OrganisationList[i].Identifier = pOrganisationList[i].Identifier;
				OrganisationList[i].Name = pOrganisationList[i].Name;
				OrganisationList[i].DbID = pOrganisationList[i].Id;
				OrganisationList[i].Index = i;
			}

			return n;
		}

		public static int FillADHPList(out ADHPType[] ADHPList, Guid organ, bool CheckILS = false)
		{
			//ESRI.ArcGIS.Geometry.IRelationalOperator pRelational;
			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;
			List<AirportHeliport> pADHPList;

			ADHPList = GlobalVars.InitArray<ADHPType>(0);
			pADHPList = pGDBProvider.GetAirportList(System.Guid.Empty);

			if (pADHPList == null)
				return -1;

			int n = pADHPList.Count;

			if (n == 0)
				return -1;

			System.Array.Resize(ref ADHPList, n);

			for (int i = 0; i < n; i++)
			{
				ADHPList[i].Name = pADHPList[i].Name;
				ADHPList[i].ID = pADHPList[i].Identifier;
				ADHPList[i].pAirportHeliport = pADHPList[i];

				pPtGeo = ConvertToEsriGeom.FromPoint(pADHPList[i].ARP.Geo);

				//    pPtGeo.X = pADHP.ARP.pos.DoubleList(0)
				//    pPtGeo.Y = pADHP.ARP.pos.DoubleList(1)

				//    if pADHP.ARP.pos.DoubleList.Count >= 3 
				//        pPtGeo.Z = pADHP.ARP.pos.DoubleList(2)
				//    else
				pPtGeo.Z = ConverterToSI.Convert(pADHPList[i].FieldElevation, 0);
				//    End if

				pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
				if (pPtPrj.IsEmpty)
					return -1;

				//pRelational = (ESRI.ArcGIS.Geometry.IRelationalOperator)GlobalVars.p_LicenseRect;
				//if (!pRelational.Contains(pPtPrj))
				//	return -1;

				ADHPList[i].pPtGeo = pPtGeo;
				ADHPList[i].pPtPrj = pPtPrj;
				ADHPList[i].OrgID = pADHPList[i].ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

				if (pADHPList[i].MagneticVariation == null)
					ADHPList[i].MagVar = 0.0;
				else
					ADHPList[i].MagVar = pADHPList[i].MagneticVariation.Value;

				//    CurADHP.Elev = pConverter.ConvertToSI(pElevPoint.elevation, 0.0)
				//    CurADHP.pPtGeo.Z = CurADHP.Elev
				//    CurADHP.pPtPrj.Z = CurADHP.Elev

				//CurADHP.MinTMA = pConverter.Convert(pADHP.transitionAltitude, 2500.0)
				//            CurADHP.TransitionAltitude = pConverter.ConvertToSI(ah.TransitionAltitude)

				ADHPList[i].ISAtC = ConverterToSI.Convert(pADHPList[i].ReferenceTemperature, 15.0);
				//            CurADHP.ReferenceTemperature = pConverter.ConvertToSI(ah.ReferenceTemperature)

				ADHPList[i].TransitionLevel = ConverterToSI.Convert(pADHPList[i].TransitionLevel, 2500.0);
				ADHPList[i].WindSpeed = 56.0;
				ADHPList[i].Index = i;
			}
			return n;
		}

		public static int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
		{
			List<Runway> pAIXMRWYList;
			List<RunwayDirection> pRwyDRList;
			List<RunwayCentrelinePoint> pCenterLinePointList;
			//List<RunwayProtectArea> pRunwayProtectAreaList;
			//Descriptor pName;
			ElevatedPoint pElevatedPoint;

			Runway pRunway;
			RunwayDirection pRwyDirection;
			RunwayDirection pRwyDirectinPair;
			//RunwayProtectArea pRunwayProtectArea;
			RunwayCentrelinePoint pRunwayCenterLinePoint;

			//AirportHeliportProtectionArea pAirportHeliportProtectionArea;
			IPoint ptDTreshGeo, ptDTreshPrj;
			//RWYCLPoint[] CLPointArray;

			RunwayCentrelinePoint pDTreshCenterLinePoint;
			RunwayDeclaredDistance pDirDeclaredDist;
			List<RunwayDeclaredDistance> pDeclaredDistList;
			double fLDA, fTORA, fTODA, dDT;

			pAIXMRWYList = pGDBProvider.GetRunwayList(Owner.ID);
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

				pRunway = pAIXMRWYList[i];
				pRwyDRList = pGDBProvider.GetRunwayDirectionList(pRunway.Identifier);

				if (pRwyDRList.Count == 2)
				{
					for (int j = 0; j < 2; j++)
					{
						bool error = false;
						iRwyNum++;

						RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
						if (RWYList[iRwyNum].Length < 0.0)
						{
							iRwyNum--;
							if ((iRwyNum & 1) == 0)
								iRwyNum--;
							break;
						}

						pRwyDirection = pRwyDRList[j];
						pCenterLinePointList = pGDBProvider.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
						if (pCenterLinePointList.Count == 0)
						{
							iRwyNum--;
							if ((iRwyNum & 1) == 0)
								iRwyNum--;
							break;
						}

						RWYList[iRwyNum].Initialize();
						RWYList[iRwyNum].pRunwayDir = pRwyDirection;
						RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = null;
						ptDTreshGeo = ptDTreshPrj = null;
						pDTreshCenterLinePoint = null;

						RWYList[iRwyNum].CLPointArray = new RWYCLPoint[pCenterLinePointList.Count];
						fLDA = fTORA = fTODA = 0.0;

						for (int k = 0; k < pCenterLinePointList.Count; k++)
						{
							pRunwayCenterLinePoint = pCenterLinePointList[k];
							pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].CLPointArray[k].pCLPoint = pRunwayCenterLinePoint;
							RWYList[iRwyNum].CLPointArray[k].pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
							RWYList[iRwyNum].CLPointArray[k].pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
							RWYList[iRwyNum].CLPointArray[k].pPtPrj = Functions.ToPrj(RWYList[iRwyNum].CLPointArray[k].pPtGeo) as IPoint;

							if (pRunwayCenterLinePoint.Role != null)
							{
								switch (pRunwayCenterLinePoint.Role.Value)
								{
									case Aran.Aim.Enums.CodeRunwayPointRole.START:
										for (int l = 0; l < pDeclaredDistList.Count; l++)
										{
											pDirDeclaredDist = pDeclaredDistList[l];
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

										RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = RWYList[iRwyNum].CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtStart] = RWYList[iRwyNum].CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtStart] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.THR:
										RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = RWYList[iRwyNum].CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtTHR] = RWYList[iRwyNum].CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtTHR] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.END:
										RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = RWYList[iRwyNum].CLPointArray[k].pPtGeo;
										RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = RWYList[iRwyNum].CLPointArray[k].pPtPrj;
										RWYList[iRwyNum].pSignificantPoint[eRWY.PtEnd] = pRunwayCenterLinePoint;
										break;
									case Aran.Aim.Enums.CodeRunwayPointRole.DISTHR:
										ptDTreshGeo = RWYList[iRwyNum].CLPointArray[k].pPtGeo;
										ptDTreshPrj = RWYList[iRwyNum].CLPointArray[k].pPtPrj;
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

						for (eRWY rwypt = eRWY.PtStart; rwypt <= eRWY.PtEnd; rwypt++)
						{
							if (RWYList[iRwyNum].pPtGeo[rwypt] == null)
							{
								error = true;
								iRwyNum--;
								if ((iRwyNum & 1) == 0)
									iRwyNum--;
								break;
							}
						}
						if (error)
							break;

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

							pRunwayCenterLinePoint = Functions.GetNearestPoint(ref RWYList[iRwyNum].CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart]);
							if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
							{
								pElevatedPoint = pRunwayCenterLinePoint.Location;

								RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
							}
							else
							{
								pRunwayCenterLinePoint = Functions.GetNearestPoint(ref RWYList[iRwyNum].CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart], 10000.0);
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
						pRunwayCenterLinePoint = Functions.GetNearestPoint(ref RWYList[iRwyNum].CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);
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

							pRunwayCenterLinePoint = Functions.GetNearestPoint(ref RWYList[iRwyNum].CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd], 10000.0);
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
						RWYList[iRwyNum].NameID = System.Convert.ToInt32(RWYList[iRwyNum].Name.Substring(0, 2));

						RWYList[iRwyNum].ADHP_ID = Owner.ID;
						//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

						pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
						RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
						RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;

						//RWYList[iRwyNum].ClearWay = 0.0;
						//pRunwayProtectAreaList = pGDBProvider.GetRunwayProtectAreaList(pRwyDirection.Identifier);
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
						//    NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, ref ResX, ref ResY);
						//    RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);
						//}

						for (eRWY rwypt = eRWY.PtStart; rwypt <= eRWY.PtEnd; rwypt++)
						{
							//RWYList[iRwyNum].pPtPrj[k] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[k]) as IPoint;
							if (RWYList[iRwyNum].pPtPrj[rwypt].IsEmpty)
							{
								error = true;
								iRwyNum--;
								if ((iRwyNum & 1) == 0)
									iRwyNum--;
								break;
							}

							RWYList[iRwyNum].pPtGeo[rwypt].M = RWYList[iRwyNum].TrueBearing;
							RWYList[iRwyNum].pPtPrj[rwypt].M = Functions.Azt2Dir(RWYList[iRwyNum].pPtGeo[rwypt], RWYList[iRwyNum].TrueBearing);
						}
						if (error)
							break;
					}
				}
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		public static List<ObstacleType> GetObstaclesOrgID(Guid orgID)
		{
			double HorAccuracy, VertAccuracy, Elev;

			List<VerticalStructure> VerticalStructureList;
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;
			ElevatedCurve pElevatedCurve;
			ElevatedSurface pElevatedSurface;

			ObstacleType tmpObstacle;

			ESRI.ArcGIS.Geometry.IGeometry pGeoGeo;
			ESRI.ArcGIS.Geometry.IGeometry pGeoPrj;

			ESRI.ArcGIS.Geometry.IPoint pPtGeo;
			ESRI.ArcGIS.Geometry.IPoint pPtPrj;

			ESRI.ArcGIS.Geometry.IPolyline pLnGeo;
			ESRI.ArcGIS.Geometry.IPolyline pLnPrj;

			ESRI.ArcGIS.Geometry.IPolygon pPgnGeo;
			ESRI.ArcGIS.Geometry.IPolygon pPgnPrj;
			IZAware pZAware;
			IZ pZ;

			List<ObstacleType> result = new List<ObstacleType>();

			VerticalStructureList = pGDBProvider.GetVerticalStructureList(orgID);
			int n = VerticalStructureList.Count;

			if (n == 0)
				return result;

			int j = -1;

			for (int i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];

				for (int k = 0; k < AixmObstacle.Part.Count; k++)
				{
					if (AixmObstacle.Part[k].HorizontalProjection == null)
						continue;

					switch (AixmObstacle.Part[k].HorizontalProjection.Choice)
					{
						case VerticalStructurePartGeometryChoice.ElevatedPoint:
							pElevatedPoint = AixmObstacle.Part[k].HorizontalProjection.Location;
							if (pElevatedPoint == null) continue;
							if (pElevatedPoint.Elevation == null) continue;

							Elev = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);	// +pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, 0); 
							pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
							pZAware = pPtGeo as IZAware;
							pZAware.ZAware = true;
							pPtGeo.Z = Elev;

							pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;
							if (pPtPrj.IsEmpty)
								continue;
							pGeoGeo = pPtGeo;
							pGeoPrj = pPtPrj;

							HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							pElevatedCurve = AixmObstacle.Part[k].HorizontalProjection.LinearExtent;
							if (pElevatedCurve == null) continue;
							if (pElevatedCurve.Elevation == null) continue;

							Elev = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);	// +pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, 0));
							pLnGeo = ConvertToEsriGeom.FromMultiLineString(pElevatedCurve.Geo);
							pZAware = pLnGeo as IZAware;
							pZAware.ZAware = true;
							pZ = pLnGeo as IZ;
							pZ.SetConstantZ(Elev);

							pLnPrj = Functions.ToPrj(pLnGeo) as IPolyline;
							if (pLnPrj.IsEmpty)
								continue;
							pGeoGeo = pLnGeo;
							pGeoPrj = pLnPrj;

							HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							pElevatedSurface = AixmObstacle.Part[k].HorizontalProjection.SurfaceExtent;
							if (pElevatedSurface == null) continue;
							if (pElevatedSurface.Elevation == null) continue;

							Elev = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
							pPgnGeo = ConvertToEsriGeom.FromMultiPolygon(pElevatedSurface.Geo);
							pZAware = pPgnGeo as IZAware;
							pZAware.ZAware = true;
							pZ = pPgnGeo as IZ;
							pZ.SetConstantZ(Elev);

							pPgnPrj = Functions.ToPrj(pPgnGeo) as IPolygon;
							if (pPgnPrj.IsEmpty)
								continue;
							pGeoGeo = pPgnGeo;
							pGeoPrj = pPgnPrj;

							HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

							break;
						default:
							continue;
					}

					tmpObstacle = new ObstacleType();

					j++;
					tmpObstacle.pGeoGeo = pGeoGeo;
					tmpObstacle.pGeoPrj = pGeoPrj;
					tmpObstacle.Name = AixmObstacle.Name;

					tmpObstacle.PartName = AixmObstacle.Part[k].Type.ToString();
					tmpObstacle.Identifier = AixmObstacle.Identifier;
					tmpObstacle.ID = AixmObstacle.Id;

					tmpObstacle.HorAccuracy = HorAccuracy;
					tmpObstacle.VertAccuracy = VertAccuracy;

					tmpObstacle.Elevation = Elev;
					tmpObstacle.Height = ConverterToSI.Convert(AixmObstacle.Part[0].VerticalExtent, -9999);
					tmpObstacle.index = j;
					tmpObstacle.Group = i;
					result.Add(tmpObstacle);
				}
			}

			return result;
		}

		//public static int GetArObstaclesByPoly(out ObstacleType[] ObstacleList, Guid orgID, ESRI.ArcGIS.Geometry.IPolygon pPoly)
		//{
		//    List<VerticalStructure> VerticalStructureList;
		//    VerticalStructure AixmObstacle;
		//    ElevatedPoint pElevatedPoint;
		//    //Aran.Geometries.MultiPolygon pARANPolygon;
		//    ConvertToSI pConverter;
		//    IPoint pPtGeo;
		//    IPoint pPtPrj;
		//    IRelationalOperator relation = pPoly as IRelationalOperator;

		//    //pARANPolygon = ConvertFromEsriGeom.ESRIGeometryToARANGeometry(Functions.ToGeo(pPoly))
		//    //					as Aran.Geometries.MultiPolygon;

		//    VerticalStructureList = pGDBProvider.GetVerticalStructureList(orgID);
		//    int n = VerticalStructureList.Count;
		//    ObstacleList = new ObstacleType[n];

		//    if (n == 0)
		//        return -1;

		//    pConverter = new ConvertToSI();

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

		//        for (int k = 0; k < AixmObstacle.Part.Count; k++)
		//        {
				
		//        }

		//        if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//            pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

		//        if (pElevatedPoint == null) continue;
		//        if (pElevatedPoint.Elevation == null) continue;

		//        pPtGeo = ConvertToEsriGeom.FromPoint(pElevatedPoint.Geo);
		//        pPtGeo.Z = pConverter.Convert(pElevatedPoint.Elevation, -9999.0);
		//        pPtPrj = Functions.ToPrj(pPtGeo) as IPoint;

		//        if (pPtPrj.IsEmpty)
		//            continue;

		//        if (!relation.Contains(pPtPrj))
		//            continue;

		//        j++;
		//        ObstacleList[j].pPtGeo = pPtGeo;
		//        ObstacleList[j].pPtPrj = pPtPrj;
		//        ObstacleList[j].Name = AixmObstacle.Name;
		//        ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//        ObstacleList[j].ID = AixmObstacle.Id;

		//        ObstacleList[j].HorAccuracy = pConverter.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//        ObstacleList[j].VertAccuracy = pConverter.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//        ObstacleList[j].Elevation = ObstacleList[j].pPtGeo.Z;
		//        ObstacleList[j].Height = pConverter.Convert(AixmObstacle.Part[0].VerticalExtent, -9999);
		//        ObstacleList[j].index = i;
		//    }
		//    j++;
		//    System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//    return j;
		//}

		public static int GetAirspaceList(Guid orgID, out Airspace[] AirspaceList)
		{
			List<Airspace> pAirspaceList = pGDBProvider.GetAirspaceList(orgID);

			int n = pAirspaceList.Count;
			AirspaceList = new Airspace[n];

			if (n == 0)
				return -1;
			int j = 0;

			for (int i = 0; i < n; i++)
			{
				if (pAirspaceList[i].Type != null && (pAirspaceList[i].Type == CodeAirspace.TMA ||
					pAirspaceList[i].Type == CodeAirspace.P || pAirspaceList[i].Type == CodeAirspace.FIR))
				{
					AirspaceList[j] = pAirspaceList[i];
					j++;
				}
			}

			System.Array.Resize<Airspace>(ref AirspaceList, j);
			return j;
		}

		public static List<Taxiway> GetTaxiwayList(Guid airportID)
		{
			return pGDBProvider.GetTaxiwayList(airportID);
		}

		public static List<TaxiwayElement> GetTaxiwayElementList(Guid taxiwayID)
		{
			return pGDBProvider.GetTaxiwayElementList(taxiwayID);
		}

		public static List<Apron> GetApronList(Guid airportID)
		{
			return pGDBProvider.GetApronList(airportID);
		}

		public static List<ApronElement> GetApronElementList(Guid apronID)
		{
			return pGDBProvider.GetApronElementList(apronID);
		}

		private static bool isOpen = false;

		public static void InitModule()	//string
		{
			if (!isOpen)
			{
				pGDBProvider = new GDBProvider();
				pGDBProvider.Open();
				isOpen = true;
			}
		}

		public static void CloseDB()
		{
			if (isOpen)
			{
				isOpen = false;
			}
		}
	}
}
