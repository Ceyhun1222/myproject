using System;
using System.Collections.Generic;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GML;
using AIXM;
using AIXM.Features;
using AIXM.Features.Geometry;
using ARANDB;

namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IObjectDirectory pObjectDir;

		#region Convert GML to ESRI

		public static IPoint GmlPointToIPoint(IGMLPoint gmlPoint)
		{
			IPoint gmlPointToIPointReturn = new ESRI.ArcGIS.Geometry.Point();
			gmlPointToIPointReturn.PutCoords(gmlPoint.X, gmlPoint.Y);
			return gmlPointToIPointReturn;
		}

		public static GMLPolygon CreateGMLPoly(IPoint ptCenterGeo, double Radius)
		{
			IGMLPoint pGMLPoint = new GMLPoint();
			pGMLPoint.X = ptCenterGeo.X;
			pGMLPoint.Y = ptCenterGeo.Y;

			IGMLRing pGMLRing = new GMLRing();
			IGMLPolygon pGMLPoly = new GMLPolygon();

			pGMLRing.Type = RingType.Circle;
			pGMLRing.CentrePoint = pGMLPoint;
			pGMLRing.Radius = Radius;

			pGMLPoly.Add(pGMLRing);

			return (GML.GMLPolygon)pGMLPoly;
		}

		public static GMLPolygon CreateGMLPolyP(IPoint ptCenterPRJ, double Radius)
		{
			return  CreateGMLPoly(Functions.ToGeo(ptCenterPRJ) as IPoint, Radius);
		}

		public static AIXMCurve ConvertToAIXMCurve(IPolyline pPolyGEO)
		{
			AIXM.Features.Geometry.AIXMCurve convertToAIXMCurveReturn = null;
			convertToAIXMCurveReturn = new AIXMCurve();
			convertToAIXMCurveReturn.Polyline = ConvertToGMLPolyline(pPolyGEO);
			return convertToAIXMCurveReturn;
		}

		public static GMLPolyline ConvertToGMLPolyline(IPolyline pPolyGEO)
		{
			IGeometryCollection pGeometryCollection = (IGeometryCollection)pPolyGEO;
			int M = pGeometryCollection.GeometryCount;

			IGMLPolyline pGMLPolyline = new GMLPolyline();

			for (int j = 0; j < M; j++)
			{
				IGMLPart pGMLPart = new GMLPart();
				IPointCollection pPointCollection = ((ESRI.ArcGIS.Geometry.IPointCollection)(pGeometryCollection.get_Geometry(j)));
				int N = pPointCollection.PointCount;
				for (int i = 0; i < N; i++)
				{
					IGMLPoint pGMLPoint = new GMLPoint();
					IPoint ptTmp = ((ESRI.ArcGIS.Geometry.IPoint)(Functions.ToGeo(pPointCollection.get_Point(i))));
					pGMLPoint.PutCoord(ptTmp.X, ptTmp.Y);
					pGMLPart.Add(pGMLPoint);
				}

				pGMLPolyline.Add(pGMLPart);
			}

			return (GMLPolyline)pGMLPolyline;
		}

		public static GMLPolygon ConvertToGMLPolygon(IPolygon pPolyGEO)
		{
			if (pPolyGEO == null)
				return null;

			int N = pPolyGEO.ExteriorRingCount;
			if (N == 0)
				return null;

			IRing[] pExteriorRing = new ESRI.ArcGIS.Geometry.IRing[N];

			pPolyGEO.QueryExteriorRings(ref pExteriorRing[0]);

			IGMLPolygon pGMLPoly = new GMLPolygon();

			for (int i = 0; i < N; i++)
			{
				IGMLRing pGMLRing = new GMLRing();
				pGMLRing.Type = RingType.PointSeq;

				IPointCollection pPointCol = ((ESRI.ArcGIS.Geometry.IPointCollection)(pExteriorRing[i]));
				int M = pPointCol.PointCount;
				double dX = System.Math.Abs(pPointCol.get_Point(0).X - pPointCol.get_Point(M - 1).X);
				double dY = System.Math.Abs(pPointCol.get_Point(0).Y - pPointCol.get_Point(M - 1).Y);
				if (dX * dY < 0.001)
					M --;

				for (int j = 0; j < M; j++)
				{
					IGMLPoint pGMLPoint = new GMLPoint();
					IPoint pPoint = pPointCol.get_Point(j);
					pGMLPoint.X = pPoint.X;
					pGMLPoint.Y = pPoint.Y;
					pGMLRing.Add(pGMLPoint);
				}
				pGMLPoly.Add(pGMLRing);
			}

			return (GMLPolygon)pGMLPoly;
		}

		public static GMLPolygon ConvertToGMLPolygonP(IPolygon pPolyPRJ)
		{
			return ConvertToGMLPolygon(Functions.ToGeo(pPolyPRJ) as IPolygon);
		}

		#endregion

		public static int FillADHPList(out ADHPData[] ADHPList, IObjectList icaoPrefixList, bool CheckILS = false)
		{
			IUnicalName pName = null;
			INavaidEquipmentList pAIXMILSList = null;
			INavaidEquipment pAIXMNAVEq = null;

			IUnicalNameList pADHPList = pObjectDir.GetAirportHeliportList(icaoPrefixList,
					AirportHeliportFields.Designator | AirportHeliportFields.Id | AirportHeliportFields.ElevatedPoint);

			ADHPList = new ADHPData[pADHPList.Count];

			if (pADHPList.Count == 0)
				return -1;

			for (int i = 0; i < pADHPList.Count; i++)
			{
				pName = pADHPList.GetItem(i);
				ADHPList[i].Name = pName.Tag;
				ADHPList[i].ID = pName.Id;

				if (CheckILS)
				{
					pAIXMILSList = pObjectDir.GetILSNavaidEquipmentList(pName.Id);
					int t = 0;
					if (pAIXMILSList.Count > 0)
					{
						for (int j = 0; j <= pAIXMILSList.Count - 1; j++)
						{
							pAIXMNAVEq = pAIXMILSList.GetItem(j);

							if ((pAIXMNAVEq is ILocalizer))
								t |= 1;
							else if ((pAIXMNAVEq is IGlidepath))
								t |= 2;

							if (t == 3)
								break;
						}
					}

					ADHPList[i].index = t;
				}
				else
					ADHPList[i].index = i;
			}

			return pADHPList.Count - 1;
		}

		public static int FillADHPFields(ref ADHPData CurADHP)
		{
			if (CurADHP.pPtGeo != null)
				return 0;

			IAirportHeliport pADHP = pObjectDir.GetAirportHeliport(CurADHP.ID);
			if (pADHP == null)
				return -1;

			IElevatedPoint pElevPoint = pADHP.ElevatedPoint;
			IGMLPoint pGMLPoint = pElevPoint;
			IPoint pPtGeo = GmlPointToIPoint(pGMLPoint);
			IPoint pPtPrj = (IPoint)Functions.ToPrj(pPtGeo);

			if (pPtPrj.IsEmpty)
				return -1;

			//IRelationalOperator pRelational = (IRelationalOperator)GlobalVars.p_LicenseRect;
			//if (! pRelational.Contains(pPtPrj))
			//	return -1;

			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();

			CurADHP.pPtGeo = pPtGeo;
			CurADHP.pPtPrj = pPtPrj;

			CurADHP.OrgID = pADHP.OrganisationId;

			if (pADHP.MagneticVariation == null)
				CurADHP.MagVar = 0.0;
			else
				CurADHP.MagVar = pADHP.MagneticVariation.Value;

			CurADHP.Elev = pConverter.ConvertToSI(pElevPoint.Elevation, 0.0);
			CurADHP.pPtGeo.Z = CurADHP.Elev;
			CurADHP.pPtPrj.Z = CurADHP.Elev;

			CurADHP.ISAtC = pConverter.ConvertToSI(pADHP.ReferenceTemperature, 15.0);
			CurADHP.TransitionLevel = pConverter.ConvertToSI(pADHP.TransitionLevel, 2500.0);
			CurADHP.WindSpeed = 56.0;

			return 1;
		}

		public static int FillRWYList(out RWYData[] RWYList, ADHPData Owner)
		{
			IUnicalNameList pAIXMRWYList = pObjectDir.GetRunwayList(Owner.ID,
				RunwayFields.Designator | RunwayFields.Id | RunwayFields.Length | RunwayFields.Profile | RunwayFields.Type);

			if (pAIXMRWYList.Count == 0)
			{
				RWYList = new RWYData[0];
				return -1;
			}

			RWYList = new RWYData[2 * pAIXMRWYList.Count];
			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();

			int iRwyNum = -1;

			for (int i = 0; i <= pAIXMRWYList.Count - 1; i++)
			{
				IUnicalName pName = pAIXMRWYList.GetItem(i);
				IRunway pRunway = pObjectDir.GetRunway(pName.Id);
				IRunwayDirectionList pRwyDRList = pObjectDir.GetRunwayDirectionList(pName.Id);
				CLPoint[] CLPointArray;

				for (int j = 0; j < pRwyDRList.Count; j++)
				{
					iRwyNum++;

					RWYList[iRwyNum].Length = pConverter.ConvertToSI(pRunway.Length, -9999.0);
					if (RWYList[iRwyNum].Length < 0)
					{
						iRwyNum--;
						continue;
					}

					IRunwayDirection pRwyDirection = pRwyDRList.GetItem(j);
					IRunwayCentrelinePointList pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Id);

					if (pCenterLinePointList.Count == 0)
					{
						iRwyNum--;
						continue;
					}

					RWYList[iRwyNum].Initialize();
					RWYList[iRwyNum].pRunwayDir = pRwyDirection;

					IRunwayCentrelinePoint pRunwayCenterLinePoint;
					IElevatedPointList pElevatedPointList;
					IGMLPoint pGMLPoint;
					IElevatedPoint pElevPoint;

					CLPointArray = new CLPoint[pCenterLinePointList.Count];

					RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = null;

					for (int k = 0; k < pCenterLinePointList.Count; k++)
					{
						pRunwayCenterLinePoint = pCenterLinePointList.GetItem(k);
						pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;

						if (pElevatedPointList.Count >= 1)
						{
							pGMLPoint = pElevatedPointList.GetItem(0);
							pElevPoint = ((AIXM.Features.Geometry.IElevatedPoint)(pGMLPoint));
							CLPointArray[k].pCLPoint = pRunwayCenterLinePoint;
							pGMLPoint = pElevatedPointList.GetItem(0);
							pElevPoint = pGMLPoint as IElevatedPoint;

							CLPointArray[k].pPtGeo = GmlPointToIPoint(pGMLPoint);
							CLPointArray[k].pPtPrj = Functions.ToPrj(CLPointArray[k].pPtGeo) as IPoint;

							if (pRunwayCenterLinePoint.Role == AIXM.DataTypes.Enums.RunwayPointRoleType.THR)
							{
								RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = CLPointArray[k].pPtGeo;
								RWYList[iRwyNum].pPtPrj[eRWY.PtTHR] = CLPointArray[k].pPtPrj;

								RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
								RWYList[iRwyNum].pPtPrj[eRWY.PtTHR].Z = RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Z;
								RWYList[iRwyNum].ptID[eRWY.PtTHR] = pElevPoint.Id;
							}
						}
					}

					if (RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] == null)
					{
						iRwyNum--;
						continue;
					}

					if (pRwyDirection.TrueBearing != null && ! double.IsInfinity( pRwyDirection.TrueBearing.Value ))
						RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
					else if (pRwyDirection.MagneticBearing != null && !double.IsInfinity(pRwyDirection.MagneticBearing.Value))
						RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value + Owner.MagVar;
					else
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " RWY direction: Bearing not defined.");
						iRwyNum--;
						continue;

						//NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y,
						//    RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Y, ref TrueBearing, ref fTmp);
						//RWYList[iRwyNum].TrueBearing = TrueBearing;
					}

					double dDT, fASDA, fLDA, fTORA, fTODA, fDTHR;
					fLDA = fDTHR = 0;
					fASDA = fTORA = fTODA = -1;

					IRunwayDirDeclaredDistList pDeclaredDistList = pObjectDir.GetDeclaredDistance(pRwyDirection.Id);

					for (int K = 0; K < pDeclaredDistList.Count; K++)
					{
						RunwayDirDeclaredDist pDirDeclaredDist = pDeclaredDistList.GetItem(K);
						if (pDirDeclaredDist.CodeType == DeclaredDistanceType.LDA)
							fLDA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, fLDA);
						else if (pDirDeclaredDist.CodeType == DeclaredDistanceType.DPLM)
							fDTHR = pConverter.ConvertToSI(pDirDeclaredDist.Distance, fDTHR);
						else if (pDirDeclaredDist.CodeType == DeclaredDistanceType.TORA)
							fTORA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, fTORA);
						else if (pDirDeclaredDist.CodeType == DeclaredDistanceType.TODA)
							fTODA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, fTODA);
						else if (pDirDeclaredDist.CodeType == DeclaredDistanceType.ASDA)
							fASDA = pConverter.ConvertToSI(pDirDeclaredDist.Distance, fASDA);
					}

					if (fTODA < 0)
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
						iRwyNum--;
						continue;
					}
					if (fTORA < 0)
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TORA not defined.");
						iRwyNum--;
						continue;
					}
					if (fASDA < 0)
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: ASDA not defined.");
						iRwyNum--;
						continue;
					}

					RWYList[iRwyNum].TODA = fTODA;
					RWYList[iRwyNum].TORA = fTORA;
					RWYList[iRwyNum].ASDA = fASDA;

					//==============================================================================================================
					double ResX, ResY;

					dDT = fTORA - fLDA;

					if (dDT > 0)
					{
						NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X,
							RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, dDT, RWYList[iRwyNum].TrueBearing + 180.0, out ResX, out ResY);

						RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = new Point();
						RWYList[iRwyNum].pPtGeo[eRWY.PtStart].PutCoords(ResX, ResY);

						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart]);
						if (pRunwayCenterLinePoint != null)
						{
							pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
							pGMLPoint = pElevatedPointList.GetItem(0);
							pElevPoint = pGMLPoint as IElevatedPoint;

							RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = GmlPointToIPoint(pGMLPoint);
							RWYList[iRwyNum].pPtPrj[eRWY.PtStart] = Functions.ToPrj(CLPointArray[i].pPtGeo) as IPoint;
							RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
							RWYList[iRwyNum].ptID[eRWY.PtStart] = pElevPoint.Id;
						}
						else
						{
							pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtStart], 10000.0);
							if (pRunwayCenterLinePoint != null)
							{
								pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
								pElevPoint = pElevatedPointList.GetItem(0);
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
							}
							else
								RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = Owner.Elev;

							RWYList[iRwyNum].ptID[eRWY.PtStart] = "0";
						}
					}
					else
					{
						RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = RWYList[iRwyNum].pPtGeo[eRWY.PtTHR];
						RWYList[iRwyNum].ptID[eRWY.PtStart] = RWYList[iRwyNum].ptID[eRWY.PtTHR];
					}
					RWYList[iRwyNum].pPtPrj[eRWY.PtStart] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtStart]) as IPoint;
					//==============================================================================================================

					NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Y, fLDA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = new Point();
					RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].PutCoords(ResX, ResY);

					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]);
					if (pRunwayCenterLinePoint != null)
					{
						pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
						pGMLPoint = pElevatedPointList.GetItem(0);
						pElevPoint = pGMLPoint as IElevatedPoint;

						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd] = GmlPointToIPoint(pGMLPoint);
						RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]) as IPoint;
						RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
						RWYList[iRwyNum].ptID[eRWY.PtEnd] = pElevPoint.Id;
					}
					else
					{
						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtEnd], 10000.0);
						if (pRunwayCenterLinePoint != null)
						{
							pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
							pElevPoint = pElevatedPointList.GetItem(0);
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
						}
						else
							RWYList[iRwyNum].pPtGeo[eRWY.PtEnd].Z = Owner.Elev;

						RWYList[iRwyNum].ptID[eRWY.PtEnd] = "0";
					}

					RWYList[iRwyNum].pPtPrj[eRWY.PtEnd] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtEnd]) as IPoint;
					//==============================================================================================================

					NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].TODA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.PtDER] = new Point();
					RWYList[iRwyNum].pPtGeo[eRWY.PtDER].PutCoords(ResX, ResY);

					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtDER]);
					if (pRunwayCenterLinePoint != null)
					{
						pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
						pGMLPoint = pElevatedPointList.GetItem(0);
						pElevPoint = pGMLPoint as IElevatedPoint;

						RWYList[iRwyNum].pPtGeo[eRWY.PtDER] = GmlPointToIPoint(pGMLPoint);
						RWYList[iRwyNum].pPtPrj[eRWY.PtDER] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtDER]) as IPoint;
						RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
						RWYList[iRwyNum].ptID[eRWY.PtDER] = pElevPoint.Id;
					}
					else
					{
						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.PtDER], 10000.0);
						if (pRunwayCenterLinePoint != null)
						{
							pElevatedPointList = pRunwayCenterLinePoint.ElevatedPointList;
							pElevPoint = pElevatedPointList.GetItem(0);
							RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Z = pConverter.ConvertToSI(pElevPoint.Elevation, Owner.Elev);
						}
						else
							RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Z = Owner.Elev;

						RWYList[iRwyNum].ptID[eRWY.PtDER] = "0";
					}

					RWYList[iRwyNum].pPtPrj[eRWY.PtDER] = Functions.ToPrj(RWYList[iRwyNum].pPtGeo[eRWY.PtDER]) as IPoint;

					//==============================================================================================================

					for (eRWY K = eRWY.PtStart; K <= eRWY.PtEnd; K++)
					{
						if (RWYList[iRwyNum].pPtGeo[K] == null || RWYList[iRwyNum].pPtPrj[K].IsEmpty)
						{
							iRwyNum--;
							goto NextJ;
						}
						RWYList[iRwyNum].pPtGeo[K].M = RWYList[iRwyNum].TrueBearing;
						RWYList[iRwyNum].pPtPrj[K].M = Functions.Azt2Dir(RWYList[iRwyNum].pPtGeo[K], RWYList[iRwyNum].TrueBearing);
					}

					RWYList[iRwyNum].ID = pRwyDirection.Id;
					RWYList[iRwyNum].Name = pRwyDirection.Designator;
					RWYList[iRwyNum].ADHP_ID = Owner.ID;
					RWYList[iRwyNum].ILSID = pRwyDirection.ILS_ID;

					//IRunwayDirection pRwyDirectinPair = pRwyDRList.GetItem((j + 1) % 2);
					//RWYList[iRwyNum].PairID = pRwyDirectinPair.Id;
					//RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;
				NextJ: ;
				}
			}
			//}

			if (iRwyNum >= 0)
				Array.Resize<RWYData>(ref RWYList, iRwyNum + 1);
			else
				RWYList = new RWYData[0];

			return iRwyNum;
		}

		public static int GetILS(RWYData RWY, ref ILSData ILS, ADHPData Owner)
		{
			INavaid pAIXMNavaid = null;
			INavaidEquipmentList pAIXMNAVEqList = null;
			INavaidEquipment pAIXMNAVEq = null;
			ILocalizer pAIXMLocalizer = null;
			IGlidepath pAIXMGlidepath = null;
			IGMLPoint pGMLPoint = null;
			IElevatedPoint pElevPoint = null;

			if (RWY.ILSID == "")
				return -1;

			pAIXMNavaid = pObjectDir.GetILSNavaid(RWY.ILSID);
			pAIXMNAVEqList = pAIXMNavaid.NavaidEquipmentList;
			if (pAIXMNAVEqList.Count < 2)
				return -1;

			ILS.Category = System.Convert.ToInt32(pAIXMNavaid.LandingCategory) + 1;
			if (ILS.Category > 3)
				ILS.Category = 3;

			ILS.RWY_ID = RWY.ID;
			ILS.index = 0;

			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();
			for (int i = 0; i < pAIXMNAVEqList.Count; i++)
			{
				pAIXMNAVEq = pAIXMNAVEqList.GetItem(i);
				if (pAIXMNAVEq is ILocalizer)
					pAIXMLocalizer = (AIXM.Features.ILocalizer)pAIXMNAVEq;
				else if (pAIXMNAVEq is IGlidepath)
					pAIXMGlidepath = (AIXM.Features.IGlidepath)pAIXMNAVEq;
			}

			if (pAIXMLocalizer != null)
			{
				pAIXMNAVEq = pAIXMLocalizer;
				pElevPoint = pAIXMNAVEq.ElevatedPoint;

				if (pAIXMNAVEq.MagneticVariation != null)
					ILS.MagVar = pAIXMNAVEq.MagneticVariation.Value;
				else
					ILS.MagVar = Owner.MagVar;

				if (pAIXMLocalizer.TrueBearing != null)
					ILS.Course = pAIXMLocalizer.TrueBearing.Value;
				else if (pAIXMLocalizer.MagneticBearing != null)
					ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
				else
					return -1;

				pGMLPoint = pElevPoint;

				ILS.pPtGeo = GmlPointToIPoint(pGMLPoint);
				ILS.pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.Elevation, RWY.pPtGeo[eRWY.PtTHR].Z);
				ILS.pPtGeo.M = ILS.Course;

				ILS.pPtPrj = (IPoint)Functions.ToPrj(ILS.pPtGeo);
				if (ILS.pPtPrj.IsEmpty)
					return -1;

				ILS.pPtPrj.M = Functions.Azt2Dir(ILS.pPtGeo, ILS.pPtGeo.M);

				double dX = RWY.pPtPrj[eRWY.PtTHR].X - ILS.pPtPrj.X;
				double dY = RWY.pPtPrj[eRWY.PtTHR].Y - ILS.pPtPrj.Y;
				ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

				if (pAIXMLocalizer.WidthCourse != null)
				{
					ILS.AngleWidth = pAIXMLocalizer.WidthCourse.Value;
					ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(Functions.DegToRad(ILS.AngleWidth));
				}
				else
					return -1;

				ILS.index = ILS.index | 1;
			}
			else
				return -1;

			if (pAIXMGlidepath != null)
			{
				pAIXMNAVEq = pAIXMGlidepath;

				if (pAIXMGlidepath.Slope != null)
					ILS.GPAngle = pAIXMGlidepath.Slope.Value;
				else
					return -1;

				if (pAIXMGlidepath.Rdh != null)
					ILS.GP_RDH = pConverter.ConvertToSI(pAIXMGlidepath.Rdh, ILS.pPtGeo.Z);
				else
					return -1;

				ILS.ID = pAIXMNAVEq.Id;
				ILS.CallSign = pAIXMNAVEq.Designator;

				ILS.index = ILS.index | 2;
			}

			if (ILS.index < 3)
				return -1;

			return 0;
		}

		public static int FillNavaidList(out NavaidData[] NavaidList, out NavaidData[] DMEList, ADHPData CurrADHP, double Radius)
		{
			eNavaidClass NavTypeCode;

			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();
			IGMLPolygon pGMLPoly = CreateGMLPoly(CurrADHP.pPtGeo, Radius);
			INavaidEquipmentList NavaidDataList = pObjectDir.GetNavaidEquipmentList(pGMLPoly);

			NavaidList = new NavaidData[NavaidDataList.Count];
			DMEList = new NavaidData[NavaidDataList.Count];

			if (NavaidDataList.Count == 0)
				return 0;

			int iNavaidNum = -1;
			int iDMENum = -1;

			for (int i = 0; i < NavaidDataList.Count; i++)
			{
				INavaidEquipment AixmNavaidEquipment = NavaidDataList.GetItem(i);

				if (AixmNavaidEquipment is IVOR)
					NavTypeCode = eNavaidClass.CodeVOR;
				else if (AixmNavaidEquipment is IDME)
					NavTypeCode = eNavaidClass.CodeDME;
				else if (AixmNavaidEquipment is INDB)
					NavTypeCode = eNavaidClass.CodeNDB;
				else if (AixmNavaidEquipment is ITACAN)
					NavTypeCode = eNavaidClass.CodeTACAN;
				else
					continue;

				IElevatedPoint pElevPoint = AixmNavaidEquipment.ElevatedPoint;
				IGMLPoint pGMLPoint = pElevPoint;

				IPoint pPtGeo = GmlPointToIPoint(pGMLPoint);
				pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.Elevation, CurrADHP.Elev);
				IPoint pPtPrj = (IPoint)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					continue;

				if (NavTypeCode == eNavaidClass.CodeDME)
				{
					iDMENum ++;

					DMEList[iDMENum].pPtGeo = pPtGeo;
					DMEList[iDMENum].pPtPrj = pPtPrj;

					if (AixmNavaidEquipment.MagneticVariation != null)
						DMEList[iDMENum].MagVar = AixmNavaidEquipment.MagneticVariation.Value;
					else
						DMEList[iDMENum].MagVar = CurrADHP.MagVar;

					DMEList[iDMENum].Range = 350000.0; // DME.Range
					DMEList[iDMENum].PairNavaidIndex = -1;

					DMEList[iDMENum].Name = AixmNavaidEquipment.Name;
					DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

					if (DMEList[iDMENum].Name == "")
						DMEList[iDMENum].Name = DMEList[iDMENum].CallSign;

					DMEList[iDMENum].ID = AixmNavaidEquipment.Id;

					DMEList[iDMENum].TypeCode = NavTypeCode;
					//DMEList[iDMENum].TypeName = NavTypeName;
				}
				else
				{
					iNavaidNum ++;

					NavaidList[iNavaidNum].pPtGeo = pPtGeo;
					NavaidList[iNavaidNum].pPtPrj = pPtPrj;

					if (AixmNavaidEquipment.MagneticVariation != null)
						NavaidList[iNavaidNum].MagVar = AixmNavaidEquipment.MagneticVariation.Value;
					else
						NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar;

					if (NavTypeCode == eNavaidClass.CodeNDB)
						NavaidList[iNavaidNum].Range = 350000.0; // NDB.Range
					else
						NavaidList[iNavaidNum].Range = 350000.0; // VOR.Range

					NavaidList[iNavaidNum].PairNavaidIndex = -1;

					NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name;
					NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator;

					if (NavaidList[iNavaidNum].Name == "")
						NavaidList[iNavaidNum].Name = NavaidList[iNavaidNum].CallSign;

					NavaidList[iNavaidNum].ID = AixmNavaidEquipment.Id;

					NavaidList[iNavaidNum].TypeCode = NavTypeCode;
					//NavaidList[iNavaidNum].TypeName = NavTypeName;
				}
			}

			for (int j = 0; j <= iNavaidNum; j++)
			{
				for (int i = 0; i <= iDMENum; i++)
				{
					double fDist = Functions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
					if (fDist <= 2.0)
					{
						NavaidList[j].PairNavaidIndex = i;
						DMEList[i].PairNavaidIndex = j;
						break;
					}
				}
			}

			if (iNavaidNum > -1)
				Array.Resize<NavaidData>(ref NavaidList, iNavaidNum + 1);
			else
				NavaidList = new NavaidData[0];

			if (iDMENum > -1)
				System.Array.Resize<NavaidData>(ref DMEList, iDMENum + 1);
			else
				DMEList = new NavaidData[0];

			return iNavaidNum + iDMENum + 2;
		}

		public static int FillWPT_FIXList(out WPT_FIXData[] WPTList, ADHPData CurrADHP, double Radius)
		{
			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();
			IGMLPolygon pGMLPoly = CreateGMLPoly(CurrADHP.pPtGeo, Radius);

			IObjectList AIXMWPTList = (IObjectList)pObjectDir.GetDesignatedPointList(pGMLPoly);
			IObjectList AIXMNAVList = (IObjectList)pObjectDir.GetNavaidEquipmentList(pGMLPoly);

			int n = AIXMWPTList.Count + AIXMNAVList.Count - 1;
			if (n < 0)
			{
				WPTList = new WPT_FIXData[0];
				return -1;
			}

			IPoint pPtGeo, pPtPrj;
			WPTList = new WPT_FIXData[n + 1];
			int iWPTNum = -1;

			for (int i = 0; i < AIXMWPTList.Count; i++)
			{
				IDesignatedPoint AIXMWPT = ((AIXM.Features.IDesignatedPoint)(AIXMWPTList.GetItem(i)));
				IGMLPoint pGMLPoint = AIXMWPT.Point;

				pPtGeo = GmlPointToIPoint(pGMLPoint);
				pPtGeo.Z = 0.0;

				pPtPrj = (IPoint)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					continue;

				iWPTNum++;

				WPTList[iWPTNum].pPtGeo = pPtGeo;
				WPTList[iWPTNum].pPtPrj = pPtPrj;
				WPTList[iWPTNum].MagVar = CurrADHP.MagVar;

				WPTList[iWPTNum].Name = AIXMWPT.Designator;
				WPTList[iWPTNum].ID = AIXMWPT.Id;

				WPTList[iWPTNum].TypeCode = eNavaidClass.CodeNONE;
			}
			// ======================================================================

			for (int i = 0; i < AIXMNAVList.Count; i++)
			{
				INavaidEquipment AIXMNAVEq = ((AIXM.Features.INavaidEquipment)(AIXMNAVList.GetItem(i)));

				IElevatedPoint pElevPoint = AIXMNAVEq.ElevatedPoint;
				IGMLPoint pGMLPoint = pElevPoint;

				pPtGeo = GmlPointToIPoint(pGMLPoint);
				pPtGeo.Z = pConverter.ConvertToSI(pElevPoint.Elevation, CurrADHP.Elev);
				pPtPrj = (IPoint)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					continue;

				eNavaidClass NavTypeCode;
				//string NavTypeName;

				if ((AIXMNAVEq is IVOR))
				{
					NavTypeCode = eNavaidClass.CodeVOR;
					//NavTypeName = "VOR";
				}
				else if ((AIXMNAVEq is INDB))
				{
					NavTypeCode = eNavaidClass.CodeNDB;
					//NavTypeName = "NDB";
				}
				else if ((AIXMNAVEq is ITACAN))
				{
					NavTypeCode = eNavaidClass.CodeTACAN;
					//NavTypeName = "TACAN";
				}
				else
					continue;

				iWPTNum++;

				WPTList[iWPTNum].pPtGeo = pPtGeo;
				WPTList[iWPTNum].pPtPrj = pPtPrj;

				if (AIXMNAVEq.MagneticVariation != null)
					WPTList[iWPTNum].MagVar = AIXMNAVEq.MagneticVariation.Value;
				else
					WPTList[iWPTNum].MagVar = CurrADHP.MagVar;

				WPTList[iWPTNum].Name = AIXMNAVEq.Designator;
				WPTList[iWPTNum].ID = AIXMNAVEq.Id;

				WPTList[iWPTNum].TypeCode = NavTypeCode;
			}
			// ======================================================================
			if (iWPTNum < 0)
				WPTList = new WPT_FIXData[0];
			else
				System.Array.Resize<WPT_FIXData>(ref WPTList, iWPTNum + 1);

			return iWPTNum + 1;
		}

		public static int GetObstacles(out ObstacleData[] ObstacleList, IPoint ptCenter, double MaxDist, double fRefHeight)
		{
			IPolygon pPoly = Functions.CreateCirclePrj(ptCenter, MaxDist);
			IGMLPolygon pGMLPoly = ConvertToGMLPolygonP(pPoly);
			//IGMLPolygon pGMLPoly = CreateGMLPolyP(ptCenter, MaxDist);

			IVerticalStructureList VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly);
			int n = VerticalStructureList.Count;
			ObstacleList = new ObstacleData[n];

			if (n <= 0)
				return 0;

			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();
			IProximityOperator pProxiOperator = (IProximityOperator)ptCenter;
			int j = 0;

			for (int i = 0; i < n; i++)
			{
				IVerticalStructure AixmObstacle = VerticalStructureList.GetItem(i);
				IElevatedPoint pElevatedPoint = AixmObstacle.ElevatedPoint;
				if (pElevatedPoint.Elevation == null)
					continue;

				IGMLPoint pGMLPoint = pElevatedPoint;
				IPoint pPtGeo = GmlPointToIPoint(pGMLPoint);
				pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.Elevation, -9999.0);

				IPoint pPtPrj = (IPoint)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					continue;

				ObstacleList[j].pPtGeo = pPtGeo;
				ObstacleList[j].pPtPrj = pPtPrj;
				ObstacleList[j].Name = AixmObstacle.Name;
				ObstacleList[j].ID = AixmObstacle.Id;

				IAixmPoint pAixmPoint = pElevatedPoint.AsIAixmPoint;
				ObstacleList[j].HorAccuracy = pConverter.ConvertToSI(pAixmPoint.HorizontalAccuracy, 0.0);
				ObstacleList[j].VertAccuracy = pConverter.ConvertToSI(pElevatedPoint.VerticalAccuracy, 0.0);

				ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
				ObstacleList[j].X = pProxiOperator.ReturnDistance(ObstacleList[j].pPtPrj);
				ObstacleList[j].index = i;
				j++;
			}

			System.Array.Resize<ObstacleData>(ref ObstacleList, j);
			//if (j > 0)
			//    System.Array.Resize<ObstacleType>(ref ObstacleList, j);
			//else
			//    ObstacleList = new ObstacleType[0];

			return j;
		}

		public static int GetObstListInPoly(out ObstacleData[] ObstList, IPolygon pPoly, double fRefHeight)
		{
			IGMLPolygon pGMLPoly = ConvertToGMLPolygonP(pPoly);
			IVerticalStructureList VerticalStructureList = pObjectDir.GetVerticalStructureList(pGMLPoly);
			int n = VerticalStructureList.Count;
			ObstList = new ObstacleData[n];

			if (n <= 0)
				return 0;

			Converter.IAixmConvertType pConverter = new Converter.AixmConvertType();
			int j = 0;

			for (int i = 0; i < n; i++)
			{
				IVerticalStructure AixmObstacle = VerticalStructureList.GetItem(i);
				IElevatedPoint pElevatedPoint = AixmObstacle.ElevatedPoint;
				if (pElevatedPoint.Elevation == null)
					continue;

				IGMLPoint pGMLPoint = pElevatedPoint;
				IPoint pPtGeo = GmlPointToIPoint(pGMLPoint);
				pPtGeo.Z = pConverter.ConvertToSI(pElevatedPoint.Elevation, -9999.0);

				IPoint pPtPrj = ((ESRI.ArcGIS.Geometry.IPoint)(Functions.ToPrj(pPtGeo)));

				if (pPtPrj.IsEmpty)
					continue;

				ObstList[j].pPtGeo = pPtGeo;
				ObstList[j].pPtPrj = pPtPrj;

				IAixmPoint pAixmPoint = pElevatedPoint.AsIAixmPoint;
				ObstList[j].HorAccuracy = pConverter.ConvertToSI(pAixmPoint.HorizontalAccuracy, 0.0);
				ObstList[j].VertAccuracy = pConverter.ConvertToSI(pElevatedPoint.VerticalAccuracy, 0.0);

				ObstList[j].Name = AixmObstacle.Name;
				ObstList[j].ID = AixmObstacle.Id;
				ObstList[j].Height = ObstList[j].pPtGeo.Z - fRefHeight;
				ObstList[j].index = i;

				j++;
			}

			System.Array.Resize<ObstacleData>(ref ObstList, j);
			//if (j >= 0)
			//    System.Array.Resize<ObstacleType>(ref ObstList, j);
			//else
			//    ObstList = new ObstacleType[0];

			return j;
		}

		public static IWorkspace GetADHPWorkspace()
		{
			int I = 0;
			IMxDocument mxDoc = null;
			IMap map = null;
			IFeatureLayer fL = null;
			IDataset ds = null;

			mxDoc = (IMxDocument)GlobalVars.Application.Document;
			map = mxDoc.FocusMap;

			for (I = 0; I <= map.LayerCount - 1; I++)
			{
				if (map.get_Layer(I) is IFeatureLayer)
				{
					if (map.get_Layer(I).Name == "ADHP")
					{
						fL = ((ESRI.ArcGIS.Carto.IFeatureLayer)(map.get_Layer(I)));
						ds = ((ESRI.ArcGIS.Geodatabase.IDataset)(fL.FeatureClass));
						return ds.Workspace;
					}
				}
			}

			return null;
		}

		public static IRunwayCentrelinePoint GetNearestPoint(CLPoint[] CLPointArray, IPoint pPtGeo, double MaxDist = 5.0)
		{
			IRunwayCentrelinePoint rv = null;
			double dist = MaxDist;
			IPoint ptA = Functions.ToPrj(pPtGeo) as IPoint;

			for (int i = 0; i < CLPointArray.Length; i++)
			{
				if (CLPointArray[i].pPtPrj != null)
				{
					double temp = Functions.ReturnDistanceInMeters(ptA, CLPointArray[i].pPtPrj);
					if (temp < dist)
					{
						rv = CLPointArray[i].pCLPoint;
						dist = temp;
					}
				}
			}

			return rv;
		}

		public static string InitModule()
		{
			ObjectDirectoryFactory odf = new ObjectDirectoryFactory();
			pObjectDir = odf.Create("", "Conv45.AranObjectDirectory.dll");

			pObjectDir.Map = GlobalVars.pMap;
			string mapFileName = GlobalVars.GetMapFileName();
			pObjectDir.ConnectUsingRNAVSettings(mapFileName);
			return pObjectDir.GetCurrentUserName();
		}

		public static void Terminate()
		{
			pObjectDir = null;
		}
	}
}
