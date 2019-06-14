using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;
using Aran.Queries.Panda_2;
using Aran.Queries;
using System.Diagnostics;

namespace Aran.PANDA.CRMWall
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IPandaSpecializedQPI pObjectDir;

		//public static void OpenTableFromFile(out ITable pTable, string sFolderName, string sFileName)
		//{
		//    IWorkspaceFactory pFact = new ShapefileWorkspaceFactory();
		//    IWorkspace pWorkspace = pFact.OpenFromFile(sFolderName, GlobalVars.GetApplicationHWnd());
		//    pTable = ((IFeatureWorkspace)(pWorkspace)).OpenTable(sFileName);
		//}

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

		public static int FillADHPList(ref ADHPType[] ADHPList, Guid organ, bool CheckILS = false)
		{
			int i, n;

			Descriptor pName;
			List<Descriptor> pADHPNameList;

			ADHPList = new ADHPType[0];

			pADHPNameList = pObjectDir.GetAirportHeliportList(organ, CheckILS);	//, AirportHeliportFields_Designator + AirportHeliportFields_Id + AirportHeliportFields_ElevatedPoint

			if (pADHPNameList == null)
				return -1;

			n = pADHPNameList.Count;

			if (n == 0)
				return -1;

			System.Array.Resize(ref ADHPList, n);

			for (i = 0; i < n; i++)
			{
				pName = pADHPNameList[i];
				ADHPList[i].Name = pName.Name;
				ADHPList[i].Identifier = pName.Identifier;

				//if CheckILS 
				//	pAIXMILSList = pObjectDir.GetILSNavaidEquipmentList(pName.Identifier)
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

				ADHPList[i].index = i;
			}

			return pADHPNameList.Count - 1;
		}

		public static int FillADHPFields(ref ADHPType CurrADHP)
		{
			if (CurrADHP.pPtGeo != null)
				return 0;

			AirportHeliport pADHP = pObjectDir.GetFeature(FeatureType.AirportHeliport, CurrADHP.Identifier) as Aran.Aim.Features.AirportHeliport;
			CurrADHP.pAirportHeliport = pADHP;
			if (pADHP == null)
				return -1;

			CurrADHP.Identifier = pADHP.Identifier;

			Point pPtGeo = pADHP.ARP.Geo;
			pPtGeo.Z = ConverterToSI.Convert(pADHP.ARP.Elevation, 0);

			Point pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

			if (pPtPrj.IsEmpty)
				return -1;
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.LineSymbol pLineSym = new Aran.AranEnvironment.Symbols.LineSymbol();
			//pLineSym.Color = ARANFunctions.RGB(255, 0, 0);
			//pLineSym.Width = 2;

			//Aran.AranEnvironment.Symbols.FillSymbol pFillSym = new Aran.AranEnvironment.Symbols.FillSymbol();
			//pFillSym.Style = Aran.AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross;	//Aran.AranEnvironment.Symbols.eFillStyle.sfsNull;
			//pFillSym.Color = 255;
			//pFillSym.Outline = pLineSym;	//	pRedLineSymbol

			//GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.p_LicenseRect, pFillSym);
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			//Aran.AranEnvironment.Symbols.PointSymbol ptSym = new AranEnvironment.Symbols.PointSymbol();
			//ptSym.Size = 8;
			//ptSym.Style = AranEnvironment.Symbols.ePointStyle.smsCircle;
			//ptSym.Color = 255;
			//GlobalVars.gAranGraphics.DrawPointWithText(pPtPrj, ptSym, "Arpt");
			//=====++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


			//GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.p_LicenseRect, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);
			GeometryOperators geomOperators = new GeometryOperators();
			if (!geomOperators.Contains(GlobalVars.p_LicenseRect, pPtPrj))
			    return -1;


			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			if (pADHP.MagneticVariation == null)
				CurrADHP.MagVar = 0.0;
			else
				CurrADHP.MagVar = pADHP.MagneticVariation.Value;

			CurrADHP.Elev = pPtGeo.Z;

			//CurADHP.MinTMA = ConverterToSI.Convert(pADHP.transitionAltitude, 2500.0);
			//CurADHP.TransitionAltitude = ConverterToSI.ConvertToSI(ah.TransitionAltitude);

			CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);
			//CurADHP.ReferenceTemperature = ConverterToSI.ConvertToSI(ah.ReferenceTemperature);

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}

		static RunwayCentrelinePoint GetNearestPoint(CLPoint[] CLPointArray, Point pPtGeo, double MaxDist = 5.0)
		{
			RunwayCentrelinePoint result = null;
			Point ptA = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
			double fDist = MaxDist;

			int n = CLPointArray.Length;
			for (int i = 0; i < n; i++)
			{
				if (CLPointArray[i].pPtPrj == null)
					continue;

				if (CLPointArray[i].pPtPrj.IsEmpty)
					continue;

				double fTmp = ARANFunctions.ReturnDistanceInMeters(ptA, CLPointArray[i].pPtPrj);

				if (fTmp < fDist)
				{
					result = CLPointArray[i].pCLPoint;
					fDist = fTmp;
				}
			}

			return result;
		}

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

			CLPoint[] CLPointArray;

			//AirportHeliportProtectionArea pAirportHeliportProtectionArea;
			//List<RunwayProtectArea> pRunwayProtectAreaList;
			//RunwayProtectArea pRunwayProtectArea;

			double fLDA, fTORA, fTODA;

			pAIXMRWYList = pObjectDir.GetRunwayList(Owner.pAirportHeliport.Identifier);	//, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

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
				int m = pRwyDRList.Count;

				//if (pRwyDRList.Count == 2)//{
				for (int j = 0; j < m; j++)
				{
					iRwyNum++;

					//RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
					//if (RWYList[iRwyNum].Length < 0)
					//{
					//    iRwyNum--;
					//    continue;
					//}

					pRwyDirection = pRwyDRList[j];
					pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
					RWYList[iRwyNum].Initialize();
					RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] = null;

					Point ptDTresh = null;
					CLPointArray = new CLPoint[pCenterLinePointList.Count];
					fLDA = fTORA = fTODA = 0.0;

					List<RunwayDeclaredDistance> pDeclaredDistList = null;
					//double PtEndZ = -9999.0;

					for (int k = 0; k < pCenterLinePointList.Count; k++)
					{
						pRunwayCenterLinePoint = pCenterLinePointList[k];
						pElevatedPoint = pRunwayCenterLinePoint.Location;

						if (pRunwayCenterLinePoint.Role != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							CLPointArray[k].pCLPoint = pRunwayCenterLinePoint;
							CLPointArray[k].pPtGeo = pElevatedPoint.Geo;
							CLPointArray[k].pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pElevatedPoint.Geo);

							switch (pRunwayCenterLinePoint.Role.Value)		//Select Case pRunwayCenterLinePoint.Role.Value
							{
								case CodeRunwayPointRole.START:
									RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);

									pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

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
									RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
								case CodeRunwayPointRole.END:
									RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] = CLPointArray[k].pPtGeo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
								case CodeRunwayPointRole.DISTHR:
									ptDTresh = CLPointArray[k].pPtGeo;
									ptDTresh.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									break;
							}
						}
					}

					if (RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] == null && ptDTresh != null)
						RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] = ptDTresh;

					if (RWYList[iRwyNum].pPtGeo[eRWY.ptStart] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] == null)
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
						NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.ptStart].X, RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Y, out TrueBearing, out fTmp);
						RWYList[iRwyNum].TrueBearing = TrueBearing;
					}

					//==============================================================================================================
					double ResX, ResY, dDT = fTORA - fLDA;

					if (dDT > 0.0)
					{
						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.ptStart]);
						if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = pElevatedPoint.Geo;
							RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
						}
						else
						{
							NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].Y, dDT, RWYList[iRwyNum].TrueBearing + 180.0, out ResX, out ResY);
							RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = new Point(ResX, ResY);
							//RWYList(iRwyNum).pPtGeo(eRWY.ptStart).PutCoords(ResX, ResY)

							pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.ptStart], 10000.0);
							if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
							{
								pElevatedPoint = pRunwayCenterLinePoint.Location;
								RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
							}
							else
								RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = Owner.Elev;
						}
					}
					else
						RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = RWYList[iRwyNum].pPtGeo[eRWY.ptTHR];
					//==============================================================================================================
					pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.ptEnd]);
					if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
					{
						pElevatedPoint = pRunwayCenterLinePoint.Location;
						RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] = pElevatedPoint.Geo;
						RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
					}
					else
					{
						NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].X, RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].Y, fLDA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
						RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] = new Point(ResX, ResY);

						pRunwayCenterLinePoint = GetNearestPoint(CLPointArray, RWYList[iRwyNum].pPtGeo[eRWY.ptEnd], 10000.0);
						if (pRunwayCenterLinePoint != null && pRunwayCenterLinePoint.Location != null)
						{
							pElevatedPoint = pRunwayCenterLinePoint.Location;
							RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
						}
						else
							RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = Owner.Elev;
					}

					RWYList[iRwyNum].ClearWay = fTODA - fTORA;
					if (RWYList[iRwyNum].ClearWay < 0.0)
						RWYList[iRwyNum].ClearWay = 0.0;

					if (RWYList[iRwyNum].ClearWay > 0.0)
					{
						NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].X, RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
						RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].X = ResX;
						RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Y = ResY;
					}

					//==============================================================================================================

					for (eRWY ek = eRWY.ptStart; ek <= eRWY.ptEnd; ek++)
					{
						if (ek == eRWY.ptDER && RWYList[iRwyNum].pPtGeo[ek] == null)
							continue;

						RWYList[iRwyNum].pPtPrj[ek] = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(RWYList[iRwyNum].pPtGeo[ek]);
						if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
						{
							iRwyNum--;
							goto NextI;
						}

						RWYList[iRwyNum].pPtGeo[ek].M = RWYList[iRwyNum].TrueBearing;
						RWYList[iRwyNum].pPtPrj[ek].M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
						//GlobalVars.gAranGraphics.DrawPointWithText(RWYList[iRwyNum].pPtPrj[ek], -1, ek.ToString());
					}

					RWYList[iRwyNum].Identifier = pRwyDirection.Identifier;
					RWYList[iRwyNum].Name = pRwyDirection.Designator;
					RWYList[iRwyNum].ADHP_ID = Owner.Identifier;
					//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

					pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
					RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
					RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;

					RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
					if (RWYList[iRwyNum].Length < 0)
						RWYList[iRwyNum].Length = ARANFunctions.ReturnDistanceInMeters(RWYList[iRwyNum].pPtPrj[eRWY.ptEnd], RWYList[iRwyNum].pPtPrj[eRWY.ptStart]);

				NextI: ;
				}
			}

			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
		{
			int n = NavaidList.GetLength(0);

			for (int i = 0; i < n; i++)
				if ((NavaidList[i].TypeCode == eNavaidType.LLZ) && (NavaidList[i].CallSign == ILS.CallSign))
					return i;

			System.Array.Resize<NavaidType>(ref NavaidList, n + 1);
			NavaidList[n] = ILS.ToNavaid();
			//NavaidList[n].pPtGeo = ILS.pPtGeo;
			//NavaidList[n].pPtPrj = ILS.pPtPrj;
			//NavaidList[n].Name = ILS.CallSign;
			//NavaidList[n].NAV_Ident = ILS.NAV_Ident;
			//NavaidList[n].Identifier = ILS.Identifier;
			//NavaidList[n].CallSign = ILS.CallSign;

			//NavaidList[n].MagVar = ILS.MagVar;
			//NavaidList[n].TypeCode = eNavaidType.LLZ;
			//NavaidList[n].Range = 40000.0;
			////NavaidList[n].index = ILS.index;
			////NavaidList[n].PairNavaidIndex = -1;

			//NavaidList[n].GPAngle = ILS.GPAngle;
			//NavaidList[n].GP_RDH = ILS.GP_RDH;

			//NavaidList[n].Course = ILS.Course;
			//NavaidList[n].LLZ_THR = ILS.LLZ_THR;
			//NavaidList[n].SecWidth = ILS.SecWidth;
			//NavaidList[n].AngleWidth = ILS.AngleWidth;
			//NavaidList[n].Tag = 0;

			return n + 1;
		}

		public static long GetILSByName(string ProcName, ADHPType ADHP, ref ILSType ILS)
		{
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

			//    ADHP.ID = OwnerID
			//    if FillADHPFields(ADHP) < 0  Exit Function

			RWYType[] RWYList;

			int n = FillRWYList(out RWYList, ADHP);

			for (int i = 0; i < n; i++)
				if (RWYList[i].Name == RWYName)
					return GetILS(RWYList[i], ref ILS, ADHP);

			return 0;

			//    if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		}

		public static long GetILS(RWYType RWY, ref ILSType ILS, ADHPType Owner)
		{
			NavaidEquipment pAIXMNAVEq;
			Localizer pAIXMLocalizer;
			Glidepath pAIXMGlidepath;
			ElevatedPoint pElevPoint;

			ILS.index = 0;

			//if RWY.ILSID = ""  Exit Function

			Navaid pNavaid = pObjectDir.GetILSNavaid(RWY.Identifier);
			if (pNavaid == null)
				return 0;

			List<NavaidComponent> pAIXMNAVEqList = pNavaid.NavaidEquipment;
			if (pAIXMNAVEqList.Count == 0)
				return 0;

			ILS.Category = 4;
			ILS.NAV_Ident = pNavaid.Identifier;

			if (pNavaid.SignalPerformance != null)
				ILS.Category = (int)pNavaid.SignalPerformance.Value + 1;

			if (ILS.Category > 3)
				ILS.Category = 1;

			ILS.RWY_ID = RWY.Identifier;

			pAIXMLocalizer = null;
			pAIXMGlidepath = null;

			int j = 0;

			for (int i = 0; i < pAIXMNAVEqList.Count; i++)
			{
				pAIXMNAVEq = (NavaidEquipment)pAIXMNAVEqList[i].TheNavaidEquipment.GetFeature();

				if (pAIXMNAVEq is Localizer)
				{
					pAIXMLocalizer = (Localizer)pAIXMNAVEq;
					j |= 1;
				}
				else if (pAIXMNAVEq is Glidepath)
				{
					pAIXMGlidepath = (Glidepath)pAIXMNAVEq;
					j |= 2;
				}

				if (j == 3)
					break;
			}

			if (pAIXMLocalizer != null)
			{
				pAIXMNAVEq = (NavaidEquipment)pAIXMLocalizer;
				pElevPoint = pAIXMNAVEq.Location;

				if (pAIXMNAVEq.MagneticVariation != null)
					ILS.MagVar = (double)pAIXMNAVEq.MagneticVariation.Value;
				else if (pAIXMLocalizer.TrueBearing != null && pAIXMLocalizer.MagneticBearing != null)
					ILS.MagVar = (double)(pAIXMLocalizer.MagneticBearing.Value - pAIXMLocalizer.TrueBearing.Value);
				else
					ILS.MagVar = Owner.MagVar;

				double Course;
				if (pAIXMLocalizer.TrueBearing != null)
					Course = pAIXMLocalizer.TrueBearing.Value;
				else if (pAIXMLocalizer.MagneticBearing != null)
					Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
				else
					goto NoLocalizer;
				//Course = Modulus(Course + 180.0, 360.0)

				ILS.pPtGeo = pElevPoint.Geo;
				ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.ptTHR].Z);
				ILS.pPtGeo.M = Course;
				//ILS.Course = Course;

				ILS.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ILS.pPtGeo);
				if (ILS.pPtPrj.IsEmpty)
					return 0;

				ILS.pPtPrj.M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(ILS.pPtGeo, ILS.pPtGeo.M);

				double dX = RWY.pPtPrj[eRWY.ptTHR].X - ILS.pPtPrj.X;
				double dY = RWY.pPtPrj[eRWY.ptTHR].Y - ILS.pPtPrj.Y;
				ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

				if (pAIXMLocalizer.WidthCourse != null)
				{
					ILS.AngleWidth = (double)pAIXMLocalizer.WidthCourse.Value;
					ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(ARANMath.DegToRad(ILS.AngleWidth));

					ILS.index = 1;
					ILS.Identifier = pAIXMNAVEq.Identifier;
					ILS.CallSign = pAIXMNAVEq.Designator;
				}
			}
		NoLocalizer:

			if (pAIXMGlidepath == null)
				return ILS.index;

			pAIXMNAVEq = pAIXMGlidepath;

			if (pAIXMGlidepath.Slope == null)
				return ILS.index;

			ILS.GPAngle = pAIXMGlidepath.Slope.Value;

			if (pAIXMGlidepath.Rdh == null)
				return ILS.index;

			ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, -9999.0);

			if (ILS.GP_RDH < 0.0)
				return ILS.index;

			ILS.index = ILS.index | 2;

			if (ILS.index == 2)
			{
				ILS.Identifier = pAIXMNAVEq.Identifier;
				ILS.CallSign = pAIXMNAVEq.Designator;
			}
			return ILS.index;
		}

		public static void FillNavaidList(out NavaidType[] NavaidList, out NavaidType[] DMEList, ADHPType CurrADHP, double Radius)
		{
			int i, j;
			int iNavaidNum, iDMENum;

			double fDist;

			eNavaidType NavTypeCode;
			List<Navaid> pNavaidList;

			Navaid pNavaid;
			NavaidEquipment AixmNavaidEquipment;

			Point pPtGeo;
			Point pPtPrj;

			ElevatedPoint pElevPoint;

			Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, Radius);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon pARANPolygon = new MultiPolygon();
			pARANPolygon.Add(pPolygon);
			pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

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

					if (AixmNavaidEquipment is VOR)
					{
						NavTypeCode = eNavaidType.VOR;
						//NavTypeName = "VOR";
					}
					else if (AixmNavaidEquipment is DME)
					{
						NavTypeCode = eNavaidType.DME;
						//NavTypeName = "DME";
					}
					else if (AixmNavaidEquipment is NDB)
					{
						NavTypeCode = eNavaidType.NDB;
						//NavTypeName = "NDB";
					}
					else if (AixmNavaidEquipment is TACAN)
					{
						NavTypeCode = eNavaidType.TACAN;
						//NavTypeName = "Tacan";
					}
					else
						continue;

					pElevPoint = AixmNavaidEquipment.Location;

					pPtGeo = pElevPoint.Geo;
					pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev);
					pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

					if (pPtPrj.IsEmpty)
						continue;

					if (NavTypeCode == eNavaidType.DME)
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
					fDist = ARANFunctions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
					if (fDist <= 2.0)
					{
						//NavaidList[j].PairNavaidIndex = i;
						//DMEList[i].PairNavaidIndex = j;
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

			Point pPtGeo;
			Point pPtPrj;

			Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, radius);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon pARANPolygon = new MultiPolygon();
			pARANPolygon.Add(pPolygon);

			pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

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

				pPtGeo = AIXMWPT.Location.Geo;
				pPtGeo.Z = CurrADHP.pPtGeo.Z + 300.0;
				pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

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
				WPTList[iWPTNum].CallSign = AIXMWPT.Designator;
				WPTList[iWPTNum].NAV_Ident = AIXMWPT.Identifier;
				WPTList[iWPTNum].Identifier = AIXMWPT.Identifier;

				if (WPTList[iWPTNum].CallSign == null)
					WPTList[iWPTNum].CallSign = "";
				if (WPTList[iWPTNum].Name == null)
					WPTList[iWPTNum].Name = "";

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

					if (AIXMNAVEq is VOR)
						NavTypeCode = eNavaidType.VOR;
					else if (AIXMNAVEq is NDB)
						NavTypeCode = eNavaidType.NDB;
					else if (AIXMNAVEq is TACAN)
						NavTypeCode = eNavaidType.TACAN;
					else
						continue;

					if (AIXMNAVEq.Location == null)
						continue;

					if (AIXMNAVEq.Location.Geo == null)
						continue;

					pPtGeo = AIXMNAVEq.Location.Geo;
					pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, CurrADHP.Elev);

					pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
					if (pPtPrj.IsEmpty)
						continue;

					iWPTNum++;

					WPTList[iWPTNum].pPtGeo = pPtGeo;
					WPTList[iWPTNum].pPtPrj = pPtPrj;

					if (AIXMNAVEq.MagneticVariation != null)
						WPTList[iWPTNum].MagVar = (double)AIXMNAVEq.MagneticVariation.Value;
					else
						WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

					WPTList[iWPTNum].Name = AIXMNAVEq.Designator;
					WPTList[iWPTNum].NAV_Ident = pNavaid.Identifier;
					WPTList[iWPTNum].Identifier = AIXMNAVEq.Identifier;

					//WPTList[iWPTNum].pFeature = pNavaid;
					WPTList[iWPTNum].TypeCode = NavTypeCode;
				}
			}
			//======================================================================
			iWPTNum++;
			System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

			return iWPTNum;
		}

		static public int GetObstaclesByDist(out ObstacleContainer ObstacleList, Point ptCenter, double MaxDist)
		{
			List<VerticalStructure> VerticalStructureList;
			VerticalStructure AixmObstacle;

			Geometry pGeoGeometry;
			Geometry pPrjGeometry;
			
			var centerGeo = GlobalVars.pspatialReferenceOperation.ToGeo(ptCenter);
			VerticalStructureList = pObjectDir.GetVerticalStructureList(centerGeo, MaxDist);
			int n = VerticalStructureList.Count;

			GeometryOperators geomOp = new GeometryOperators();
			geomOp.CurrentGeometry = ptCenter;

			ObstacleList.Obstacles = new Obstacle[n];
			ObstacleList.Parts = null;

			if (n == 0)
				return -1;

			int k = -1;
			int cap = n;
			for (int i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];

				int m = AixmObstacle.Part.Count;
				if (AixmObstacle.Part.Count == 0) continue;

				for (int j = 0; j < m; j++)
				{
					if (AixmObstacle.Part[j] == null) continue;
					if (AixmObstacle.Part[j].HorizontalProjection == null) continue;

					double ObstElevation, HorAccuracy, VertAccuracy;

					if (AixmObstacle.Part[j].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
					{
						ElevatedPoint pElevatedPoint = AixmObstacle.Part[j].HorizontalProjection.Location;
						if (pElevatedPoint == null) continue;
						if (pElevatedPoint.Elevation == null) continue;
						pGeoGeometry = pElevatedPoint.Geo;
						ObstElevation = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
						HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
						VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);
					}
					else if (AixmObstacle.Part[j].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
					{
						ElevatedCurve pElevatedCurve = AixmObstacle.Part[j].HorizontalProjection.LinearExtent;
						if (pElevatedCurve == null) continue;
						if (pElevatedCurve.Elevation == null) continue;
						pGeoGeometry = pElevatedCurve.Geo;
						ObstElevation = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
						HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
						VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);
					}
					else //if (AixmObstacle.Part[j].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface )
					{
						ElevatedSurface pElevatedSurface = AixmObstacle.Part[j].HorizontalProjection.SurfaceExtent;
						if (pElevatedSurface == null) continue;
						if (pElevatedSurface.Elevation == null) continue;
						pGeoGeometry = pElevatedSurface.Geo;
						ObstElevation = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
						HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
						VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);
					}

					pPrjGeometry = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeoGeometry);

					if (pPrjGeometry.IsEmpty)
						continue;

					k++;
					if (k >= cap)
					{
						cap += n >> 1;
						System.Array.Resize<Obstacle>(ref ObstacleList.Obstacles, cap);
					}

					ObstacleList.Obstacles[k].pGeomGeo = pGeoGeometry;
					ObstacleList.Obstacles[k].pGeomPrj = pPrjGeometry;

					ObstacleList.Obstacles[k].UnicalName = AixmObstacle.Name;
					if (AixmObstacle.Type == null)
						ObstacleList.Obstacles[k].TypeName = "";
					else
						ObstacleList.Obstacles[k].TypeName = AixmObstacle.Type.ToString();

					ObstacleList.Obstacles[k].Identifier = AixmObstacle.Identifier;
					ObstacleList.Obstacles[k].ID = AixmObstacle.Id;

					ObstacleList.Obstacles[k].HorAccuracy = HorAccuracy;
					ObstacleList.Obstacles[k].VertAccuracy = VertAccuracy;

					ObstacleList.Obstacles[k].Height = ObstElevation ;
					ObstacleList.Obstacles[k].Index = i;
				}
			}

			k++;
			System.Array.Resize<Obstacle>(ref ObstacleList.Obstacles, k);
			return k;
		}

		private static bool isOpen = false;

		public static void InitModule()
		{
			if (!isOpen)
			{
				pObjectDir = PandaSQPIFactory.Create();
				Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;
				pObjectDir.Open((DbProvider)GlobalVars.gAranEnv.DbProvider);

                var terrainDataReaderHandler = GlobalVars.gAranEnv.CommonData.GetObject("terrainDataReader") as TerrainDataReaderEventHandler;
                if (terrainDataReaderHandler != null)
                    pObjectDir.TerrainDataReader += terrainDataReaderHandler;

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
