#define checkLicenze

using System.Collections.Generic;
using System;

using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Enums;

using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Queries.Panda_2;
using Aran.Queries;
using Aran.Geometries.Operators;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		public static IPandaSpecializedQPI pObjectDir;

		//public IPolygon CreateGMLCircle(IPoint ptCenterGeo, double Radius)
		//{
		//	IGMLPoint pGMLPoint = (IGMLPoint)(new gmlPoint());
		//	pGMLPoint.X = ptCenterGeo.X;
		//	pGMLPoint.Y = ptCenterGeo.Y;

		//	IGMLRing pGMLRing = (IGMLRing)(new GMLRing());
		//	pGMLRing.Type = RingType_Circle;
		//	pGMLRing.centrePoint = pGMLPoint;
		//	pGMLRing.Radius = Radius;

		//	IGMLPolygon pGMLPoly = (IGMLPolygon)(new GMLPolygon());
		//	pGMLPoly.Add(pGMLRing);

		//	return pGMLPoly;
		//}

		//public Function CreateGMLCircleP(ptCenterPRJ As IPoint, Radius As Double) As GMLPolygon
		//    Dim ptCenterGeo As IPoint
		//    Set ptCenterGeo = Functions.ToGeo(ptCenterPRJ)
		//    Set CreateGMLCircleP = CreateGMLCircle(ptCenterGeo, Radius)
		//End Function

		//public Function ConvertToGMLPoly(pPolyGEO As IPolygon) As GMLPolygon
		//    Dim i               As Long
		//    Dim j               As Long
		//    Dim n               As Long
		//    Dim m               As Long
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
		//    n = pPolyGEO.ExteriorRingCount
		//    if n = 0  Exit Function
		//
		//    ReDim pExteriorRing(0 To n - 1)
		//
		//    pPolyGEO.QueryExteriorRings pExteriorRing(0)
		//
		//    For i = 0 To n - 1
		//        Set pGMLRing = New GMLRing
		//        pGMLRing.Type = RingType_PointSeq
		//
		//        Set pPointCol = pExteriorRing(i)
		//        m = pPointCol.PointCount
		//        dX = Abs(pPointCol.Point(0).X - pPointCol.Point(m - 1).X)
		//        dY = Abs(pPointCol.Point(0).Y - pPointCol.Point(m - 1).Y)
		//        if dX * dY < 0.001  m = m - 1
		//
		//        For j = 0 To m - 1
		//            Set pGMLPoint = New gmlPoint
		//            Set pPoint = pPointCol.Point(j)
		//            pGMLPoint.X = pPoint.X
		//            pGMLPoint.Y = pPoint.Y
		//            pGMLRing.Add pGMLPoint
		//        Next j
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
				//		For j = 0 To pAIXMILSList.Count - 1
				//			pAIXMNAVEq = pAIXMILSList(j)

				//			if (pAIXMNAVEq is Localizer) 
				//				T = T Or 1
				//			else if (pAIXMNAVEq is Glidepath) 
				//				T = T Or 2
				//			End if
				//			if T = 3  Exit For
				//		Next j
				//	End if

				//	ADHPList(i).Index = T
				//else
				//	ADHPList(i).Index = i
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

			//CurrADHP.Identifier = pADHP.Identifier;

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

			GeometryOperators geomOperators = new GeometryOperators();

			//GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.p_LicenseRect, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);

#if checkLicenze
			if (!geomOperators.Contains(GlobalVars.p_LicenseRect, pPtPrj))
				return -1;
#endif
			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			if (pADHP.MagneticVariation == null)
				CurrADHP.MagVar = 0.0;
			else
				CurrADHP.MagVar = pADHP.MagneticVariation.Value;

			CurrADHP.Elev = pPtGeo.Z;
			CurrADHP.AltimeterSource = pADHP.AltimeterSource;
			//CurADHP.MinTMA = ConverterToSI.Convert(pADHP.transitionAltitude, 2500.0);
			//CurADHP.TransitionAltitude = ConverterToSI.ConvertToSI(ah.TransitionAltitude);

			CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);
			CurrADHP.LowestTemperature = ConverterToSI.Convert(pADHP.LowestTemperature, 0);
			//CurrADHP.MaximumTemperature = ConverterToSI.Convert(pADHP.max, 0);

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
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

			//AirportHeliportProtectionArea pAirportHeliportProtectionArea;
			//List<RunwayProtectArea> pRunwayProtectAreaList;
			//RunwayProtectArea pRunwayProtectArea;

			double fLDA, fTORA, fTODA, fDTHR, fASDA;

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
					//double PtEndZ = -9999.0;

					pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
					for (int k = 0; k < pCenterLinePointList.Count; k++)
					{
						pRunwayCenterLinePoint = pCenterLinePointList[k];
						pElevatedPoint = pRunwayCenterLinePoint.Location;

                        if (pElevatedPoint == null)
                            continue;

						if (pRunwayCenterLinePoint.Role != null)
						{
							switch (pRunwayCenterLinePoint.Role.Value)		//Select Case pRunwayCenterLinePoint.Role.Value
							{
								case CodeRunwayPointRole.START:
									RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = pElevatedPoint.Geo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									//RWYList[iRwyNum].pSignificantPoint[eRWY.PtStart] = pRunwayCenterLinePoint;
									pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

									break;
								case CodeRunwayPointRole.THR:

								case CodeRunwayPointRole.DISTHR:
									RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] = pElevatedPoint.Geo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									//RWYList[iRwyNum].pSignificantPoint[eRWY.PtTHR] = pRunwayCenterLinePoint;
									break;
								case CodeRunwayPointRole.END:
									RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] = pElevatedPoint.Geo;
									RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									//PtEndZ = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									//RWYList[iRwyNum].pSignificantPoint[eRWY.PtEnd] = pRunwayCenterLinePoint;
									break;
							}
						}
					}

					if (RWYList[iRwyNum].pPtGeo[eRWY.ptStart] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] == null)
					{
						iRwyNum--;
						continue;
					}

					if (pRwyDirection.MagneticBearing != null)
						RWYList[iRwyNum].MagneticBearing = pRwyDirection.MagneticBearing.Value;

					if (pRwyDirection.TrueBearing != null)
					{
						RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
						if (pRwyDirection.MagneticBearing == null)
							RWYList[iRwyNum].MagneticBearing = NativeMethods.Modulus(pRwyDirection.TrueBearing.Value + Owner.MagVar);
					}
					else if (pRwyDirection.MagneticBearing != null)
						RWYList[iRwyNum].TrueBearing = NativeMethods.Modulus(pRwyDirection.MagneticBearing.Value - Owner.MagVar);
					else
					{
						double TrueBearing, fTmp;
						NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.ptStart].X, RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.ptDER].X, RWYList[iRwyNum].pPtGeo[eRWY.ptDER].Y, out TrueBearing, out fTmp);
						RWYList[iRwyNum].TrueBearing = TrueBearing;
						RWYList[iRwyNum].MagneticBearing = NativeMethods.Modulus(TrueBearing + Owner.MagVar);
					}

					fASDA = fLDA = fTORA = fTODA = fDTHR = -1;

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
									fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fTODA);
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.DTHR)
									fDTHR = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fDTHR);
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.ASDA)
									fASDA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fASDA);
							}
						}
					}

					if (fTODA < 0)
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
						iRwyNum--;
						continue;
					}
					RWYList[iRwyNum].TODA = fTODA;
					RWYList[iRwyNum].ASDA = fASDA;

					double ResX, ResY;
					NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.ptStart].X, RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Y, RWYList[iRwyNum].TODA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.ptDER] = new Point(ResX, ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.ptDER].Z = RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z; //PtEndZ;

					for (eRWY ek = eRWY.ptStart; ek <= eRWY.ptDER; ek++)
					{
						RWYList[iRwyNum].pPtPrj[ek] = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(RWYList[iRwyNum].pPtGeo[ek]);
						if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
						{
							iRwyNum--;
							goto NextI;
						}

						RWYList[iRwyNum].pPtGeo[ek].M = RWYList[iRwyNum].TrueBearing;
						RWYList[iRwyNum].pPtPrj[ek].M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
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

		//Function GetRWYByProcedureName(ByVal ProcName As String, ByVal ADHP As ADHPType, ByRef RWY As RWYType) As Boolean
		//	Dim i As Integer
		//	Dim n As Integer
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

		//	n = FillRWYList(RWYList, ADHP)

		//	bRWYFound = False
		//	For i = 0 To n
		//		Char_Renamed = RWYList(i).Name
		//		bRWYFound = RWYList(i).Name = RWYName
		//		if bRWYFound 
		//			GetRWYByProcedureName = True
		//			RWY = RWYList(i)
		//			Exit For
		//		End if
		//	Next i
		//End Function

		public static int GetILSByName(string ProcName, ADHPType ADHP, out ILSType ILS)
		{
			ILS = default(ILSType);

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
					return GetILS(RWYList[i], ADHP, out ILS);

			return 0;

			//    if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		}

		public static int GetILS(RWYType RWY, ADHPType Owner, out ILSType ILS)
		{
			NavaidEquipment pAIXMNAVEq;
			Localizer pAIXMLocalizer;
			Glidepath pAIXMGlidepath;
			ElevatedPoint pElevPoint;

			ILS = default(ILSType);

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

			//    Set pAIXMNAVEq = pAIXMNAVEqList.Item(0).TheNavaidEquipment
			//    Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(0).TheNavaidEquipment).Cast()

			//    if (pAIXMNAVEq is LocalizerCom) 
			//        Set pAIXMLocalizer = pAIXMNAVEq
			//    else if (pAIXMNAVEq is GlidepathCom) 
			//        Set pAIXMGlidepath = pAIXMNAVEq
			//    End if

			//    if pAIXMNAVEqList.Count > 1 
			//        Set pAIXMNAVEq = pObjectDir.GetFeature(pAIXMNAVEqList.Item(1).TheNavaidEquipment).Cast()
			//        'Set pAIXMNAVEq = pAIXMNAVEqList.Item(1).TheNavaidEquipment
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
					ILS.CallSign = pAIXMNAVEq.Designator;	//pNavaid.Designator '

					//        ID = pAIXMNAVEq.Identifier
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
					ILS.GPAngle = ARANMath.DegToRad(pAIXMGlidepath.Slope.Value);

					if (pAIXMGlidepath.Rdh != null)
					{
						ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, GlobalVars.constants.Pansops[Aran.PANDA.Constants.ePANSOPSData.arAbv_Treshold].Value);
						if (ILS.index == 0)
						{
							ILS.Identifier = pAIXMNAVEq.Identifier;
							ILS.CallSign = pAIXMNAVEq.Designator;	//pNavaid.Designator
						}
						//        ILS.ID = pAIXMNAVEq.ID
						//        ILS.CallSign = pAIXMNAVEq.Designator
						ILS.index = ILS.index | 2;
					}
				}
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
					if (AixmNavaidEquipment == null)
						continue;

					if (AixmNavaidEquipment is VOR)
						NavTypeCode = eNavaidType.VOR;					//NavTypeName = "VOR";
					else if (AixmNavaidEquipment is DME)
						NavTypeCode = eNavaidType.DME;					//NavTypeName = "DME";
					else if (AixmNavaidEquipment is NDB)
						NavTypeCode = eNavaidType.NDB;					//NavTypeName = "NDB";
					else if (AixmNavaidEquipment is TACAN)
						NavTypeCode = eNavaidType.TACAN;				//NavTypeName = "Tacan";
					else
						continue;

					if (AixmNavaidEquipment.Location == null)
						continue;

					pElevPoint = AixmNavaidEquipment.Location;

					if (pElevPoint.Geo == null)
						continue;

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

						DME pAIXMDME = (DME)AixmNavaidEquipment;

						DMEList[iDMENum].Disp = ConverterToSI.Convert(pAIXMDME.Displace, 0.0);

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

					WPTList[iWPTNum].TypeCode = NavTypeCode;
				}
			}
			//======================================================================
			iWPTNum++;
			System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

			System.Array.Sort<WPT_FIXType>(WPTList,
				(delegate(WPT_FIXType a, WPT_FIXType b){    return a.ToString().CompareTo(b.ToString());
				}));

			return iWPTNum;
		}

		//public static int GetObstaclesByPoly(out ObstacleType[] ObstacleList, MultiPolygon pPoly)
		//{
		//	int i, j, k, n, m;

		//	List<VerticalStructure> VerticalStructureList;
		//	VerticalStructure AixmObstacle;
		//	ElevatedPoint pElevatedPoint;

		//	Point pPtGeo;
		//	Point pPtPrj;

		//	MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

		//	VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//	n = VerticalStructureList.Count;

		//	ObstacleList = new ObstacleType[0];

		//	if (n == 0)
		//		return -1;

		//	m = n;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, m);
		//	k = -1;

		//	for (i = 0; i < n; i++)
		//	{
		//		AixmObstacle = VerticalStructureList[i];
		//		if (AixmObstacle.Part.Count == 0) continue;

		//		for (j = 0; j < AixmObstacle.Part.Count; j++)
		//		{
		//			if (AixmObstacle.Part[j].HorizontalProjection == null)
		//				continue;

		//			pElevatedPoint = null;
		//			if (AixmObstacle.Part[j].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//				pElevatedPoint = AixmObstacle.Part[j].HorizontalProjection.Location;

		//			if (pElevatedPoint == null) continue;
		//			if (pElevatedPoint.Elevation == null) continue;

		//			pPtGeo = pElevatedPoint.Geo;
		//			pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

		//			pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
		//			if (pPtPrj.IsEmpty) continue;

		//			k++;
		//			if (k >= m)
		//			{
		//				m += n;
		//				System.Array.Resize<ObstacleType>(ref ObstacleList, m);
		//			}

		//			ObstacleList[k].pGeoGeometry = pPtGeo;
		//			ObstacleList[k].pPrjGeometry = pPtPrj;

		//			ObstacleList[k].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//			ObstacleList[k].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//			ObstacleList[k].Name = AixmObstacle.Name;
		//			ObstacleList[k].Identifier = AixmObstacle.Identifier;
		//			ObstacleList[k].ID = AixmObstacle.Id;
		//			ObstacleList[k].Height = pPtGeo.Z;
		//			ObstacleList[k].X = 0.0;
		//			ObstacleList[k].index = i;
		//		}
		//	}
		//	k++;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, k);
		//	return k;
		//}

		public static double GetObstListInPoly(out ObstacleContainer ObstList, MultiPolygon pPoly, Point ptCenter, out double MaxDist, double fRefHeight = 0.0)
		{
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;
			ElevatedCurve pElevatedCurve;
			ElevatedSurface pElevatedSurface;

			GeometryOperators pTopooper = new GeometryOperators();
			pTopooper.CurrentGeometry = ptCenter;

			MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);
			List<VerticalStructure> VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
			int n = VerticalStructureList.Count;

			ObstList.Parts = new ObstacleData[0];
			ObstList.Obstacles = new Obstacle[n];

			MaxDist = 0.0;
			if (n == 0)
				return 0.0;

			int C = n;
			int k = -1;

			double Z, HorAccuracy, VertAccuracy;
			Z = HorAccuracy = VertAccuracy = 0.0;
			Geometry pGeomGeo = null, pGeomPrj;

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

							pGeomGeo = pElevatedPoint.Geo;
							Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent;
							if (pElevatedCurve == null) continue;
							if (pElevatedCurve.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedCurve.Geo;
							Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							//continue;

							pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
							if (pElevatedSurface == null) continue;
							if (pElevatedSurface.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedSurface.Geo;
							Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
							break;
						default:
							continue;
					}

					pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
					if (pGeomPrj.IsEmpty) continue;

					if (VertAccuracy > 0.0)
						Z += VertAccuracy;

					if (false) //(HorAccuracy > 0.0)//HorAccuracy = 0.0;
					{
						if (pGeomPrj.Type == GeometryType.Point && (HorAccuracy <= 2.0))
							pGeomPrj = ARANFunctions.CreateCirclePrj((Point)pGeomPrj, HorAccuracy);	//, 18
						else
							pGeomPrj = pTopooper.Buffer(pGeomPrj, HorAccuracy);

						pGeomGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Geometry>(pGeomPrj);
					}

					if (pGeomGeo.Type == GeometryType.Point)
					{
						(pGeomGeo as Point).Z = Z;
						(pGeomPrj as Point).Z = Z;
					}
					else
					{
						//pGeomGeo.SetConstantZ(Z);
						//pGeomPrj.SetConstantZ(Z);
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

					double fDist = pTopooper.GetDistance(pGeomPrj);
					if (fDist > MaxDist)
						MaxDist = fDist;
				}
			}

			k++;
			System.Array.Resize<Obstacle>(ref ObstList.Obstacles, k);

			return MaxDist;
		}

		//public static int GetObstaclesByPoly(out ObstacleType[] ObstacleList, MultiPolygon pPoly, double fRefHeight)
		//{
		//	int i, j, n;

		//	List<VerticalStructure> VerticalStructureList;
		//	VerticalStructure AixmObstacle;
		//	ElevatedPoint pElevatedPoint;
		//	Point pPtGeo;
		//	Point pPtPrj;

		//	MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

		//	VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//	n = VerticalStructureList.Count;
		//	ObstacleList = new ObstacleType[0];

		//	if (n == 0)
		//		return -1;

		//	System.Array.Resize<ObstacleType>(ref ObstacleList, n);

		//	j = -1;

		//	for (i = 0; i < n; i++)
		//	{
		//		AixmObstacle = VerticalStructureList[i];

		//		if (AixmObstacle.Part.Count == 0)
		//			continue;
		//		if (AixmObstacle.Part[0].HorizontalProjection == null)
		//			continue;

		//		pElevatedPoint = null;
		//		if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//			pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

		//		if (pElevatedPoint == null) continue;
		//		if (pElevatedPoint.Elevation == null) continue;

		//		pPtGeo = pElevatedPoint.Geo;
		//		pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

		//		pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
		//		if (pPtPrj.IsEmpty)
		//			continue;

		//		j++;
		//		ObstacleList[j].pGeoGeometry = pPtGeo;
		//		ObstacleList[j].pPrjGeometry = pPtPrj;
		//		ObstacleList[j].Name = AixmObstacle.Name;
		//		ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//		ObstacleList[j].ID = AixmObstacle.Id;

		//		ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//		ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//		ObstacleList[j].Height = pPtGeo.Z - fRefHeight;
		//		ObstacleList[j].X = 0.0;
		//		ObstacleList[j].index = i;
		//	}
		//	j++;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//	return j;
		//}

		//static public int GetObstaclesByDist(out ObstacleType[] ObstacleList, Point ptCenter, double MaxDist)
		//{
		//    ObstacleList = new ObstacleType[0];
		//    //return -1;

		//    int i, j, n;

		//    List<VerticalStructure> VerticalStructureList;
		//    VerticalStructure AixmObstacle;
		//    ElevatedPoint pElevatedPoint;

		//    Point pPtGeo;
		//    Point pPtPrj;

		//    Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

		//    Polygon pPolygon = new Polygon();
		//    pPolygon.ExteriorRing = ring;

		//    MultiPolygon TmpPolygon = new MultiPolygon();
		//    TmpPolygon.Add(pPolygon);
		//    //GlobalVars.gAranGraphics.DrawMultiPolygon(TmpPolygon, 125, AranEnvironment.Symbols.eFillStyle.sfsCross);

		//    MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

		//    VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//    n = VerticalStructureList.Count;

		//    ObstacleList = new ObstacleType[0];

		//    if (n == 0)
		//        return -1;

		//    System.Array.Resize<ObstacleType>(ref ObstacleList, n);

		//    j = -1;

		//    for (i = 0; i < n; i++)
		//    {
		//        AixmObstacle = VerticalStructureList[i];

		//        if (AixmObstacle.Part.Count == 0) continue;
		//        if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

		//        pElevatedPoint = null;
		//        if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//            pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

		//        if (pElevatedPoint == null) continue;
		//        if (pElevatedPoint.Elevation == null) continue;

		//        pPtGeo = pElevatedPoint.Geo;
		//        pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

		//        pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
		//        if (pPtPrj.IsEmpty) continue;

		//        j++;
		//        ObstacleList[j].pGeoGeometry = pPtGeo;
		//        ObstacleList[j].pPrjGeometry = pPtPrj;
		//        ObstacleList[j].Name = AixmObstacle.Name;
		//        ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//        ObstacleList[j].ID = AixmObstacle.Id;

		//        ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//        ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//        ObstacleList[j].Height = pPtGeo.Z;
		//        ObstacleList[j].X = ARANFunctions.ReturnDistanceInMeters(ptCenter, pPtPrj);
		//        ObstacleList[j].index = i;
		//    }

		//    j++;
		//    System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//    return j;
		//}

		//static public int GetObstaclesByDist(out ObstacleType[] ObstacleList, Point ptCenter, double MaxDist, double fRefHeight = 0.0)
		//{
		//	int i, k, n;

		//	List<VerticalStructure> VerticalStructureList;
		//	VerticalStructure AixmObstacle;
		//	ElevatedPoint pElevatedPoint;

		//	Point pPtGeo;
		//	Point pPtPrj;

		//	Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);
		//	Polygon pPolygon = new Polygon();
		//	pPolygon.ExteriorRing = ring;

		//	MultiPolygon TmpPolygon = new MultiPolygon();
		//	TmpPolygon.Add(pPolygon);

		//	MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

		//	VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//	n = VerticalStructureList.Count;

		//	ObstacleList = new ObstacleType[0];

		//	if (n == 0)
		//		return -1;

		//	System.Array.Resize<ObstacleType>(ref ObstacleList, n);

		//	k = -1;

		//	for (i = 0; i < n; i++)
		//	{
		//		AixmObstacle = VerticalStructureList[i];

		//		if (AixmObstacle.Part.Count == 0) continue;
		//		if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

		//		pElevatedPoint = null;
		//		if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
		//			pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

		//		if (pElevatedPoint == null) continue;
		//		if (pElevatedPoint.Elevation == null) continue;

		//		pPtGeo = pElevatedPoint.Geo;
		//		pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

		//		pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
		//		if (pPtPrj.IsEmpty) continue;

		//		k++;
		//		ObstacleList[k].pGeoGeometry = pPtGeo;
		//		ObstacleList[k].pPrjGeometry = pPtPrj;
		//		ObstacleList[k].Name = AixmObstacle.Name;
		//		ObstacleList[k].Identifier = AixmObstacle.Identifier;
		//		ObstacleList[k].ID = AixmObstacle.Id;

		//		ObstacleList[k].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//		ObstacleList[k].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//		ObstacleList[k].Height = pPtGeo.Z - fRefHeight;
		//		ObstacleList[k].X = ARANFunctions.ReturnDistanceInMeters(ptCenter, pPtPrj);
		//		ObstacleList[k].index = i;
		//	}
		//	k++;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, k);
		//	return k;
		//}

		//public static double GetObstListInPoly(out ObstacleContainer ObstList, MultiPolygon pPoly, Point ptCenter, out double MaxDist, double fRefHeight = 0.0)

		public static int GetObstaclesByDist(out ObstacleContainer ObstList, Point ptCenter, double MaxDist, double fRefHeight = 0.0)
		{
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;
			ElevatedCurve pElevatedCurve;
			ElevatedSurface pElevatedSurface;

			GeometryOperators pTopooper = new GeometryOperators();

			//pTopooper.CurrentGeometry = ptCenter;
			//MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

			Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);
			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon TmpPolygon = new MultiPolygon();
			TmpPolygon.Add(pPolygon);

			MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);
			//==========================================================================================================================
			List<VerticalStructure> VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
			int n = VerticalStructureList.Count;

			ObstList.Parts = new ObstacleData[0];
			ObstList.Obstacles = new Obstacle[n];

			//MaxDist = 0.0;
			if (n == 0)
				return 0;

			int C = n;
			int k = -1;

			double Z, HorAccuracy, VertAccuracy;
			Z = HorAccuracy = VertAccuracy = 0.0;
			Geometry pGeomGeo = null, pGeomPrj;

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

							pGeomGeo = pElevatedPoint.Geo;
							Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent;
							if (pElevatedCurve == null) continue;
							if (pElevatedCurve.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedCurve.Geo;
							Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							//continue;

							pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
							if (pElevatedSurface == null) continue;
							if (pElevatedSurface.Elevation == null) continue;

							HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
							VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedSurface.Geo;
							Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
							break;
						default:
							continue;
					}

					pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
					if (pGeomPrj.IsEmpty) continue;

					if (VertAccuracy > 0.0)
						Z += VertAccuracy;

					if (false) //(HorAccuracy > 0.0)//HorAccuracy = 0.0;
					{
						if (pGeomPrj.Type == GeometryType.Point && (HorAccuracy <= 2.0))
							pGeomPrj = ARANFunctions.CreateCirclePrj((Point)pGeomPrj, HorAccuracy);	//, 18
						else
							pGeomPrj = pTopooper.Buffer(pGeomPrj, HorAccuracy);

						pGeomGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Geometry>(pGeomPrj);
					}

					if (pGeomGeo.Type == GeometryType.Point)
					{
						(pGeomGeo as Point).Z = Z;
						(pGeomPrj as Point).Z = Z;
					}
					else
					{
						//pGeomGeo.SetConstantZ(Z);
						//pGeomPrj.SetConstantZ(Z);
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

					//double fDist = pTopooper.GetDistance(pGeomPrj);
					//if (fDist > MaxDist)
					//    MaxDist = fDist;
				}
			}

			k++;
			System.Array.Resize<Obstacle>(ref ObstList.Obstacles, k);

			return k;
		}


		/*
	public int FillSegObstList(ByVal LastPoint As StepDownFIX, ByVal fRefHeight As Double, ByVal IAF_FullAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal IAF_BaseAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByRef ObstList() As ObstacleAr)
{
		Dim i As Integer
		Dim j As Integer
		Dim n As Integer

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
		Dim ConverterToSI As ConvertToSI
		Dim pARANPolygon As MultiPolygon

		ReDim ObstList(-1)

		pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(IAF_FullAreaPoly) as IPolygon)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		n = VerticalStructureList.Count - 1

		if n < 0  Return -1

		ConverterToSI = New ConvertToSI
		ReDim ObstList(n)

		pBaseProxi = IAF_BaseAreaPoly

		FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		fDir = LastPoint.InDir
		GuidNav = LastPoint.GuidNav

		fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)
		j = -1
		For i = 0 To n
			AixmObstacle = VerticalStructureList.Item(i)

			if AixmObstacle.Part.Count = 0  continue For
			if AixmObstacle.Part.Item(0).HorizontalProjection is Nothing  continue For

			pElevatedPoint = Nothing
			if AixmObstacle.Part.Item(0).HorizontalProjection.geometryType = Delib.Classes.Enums.GeometryType.Point 
				pElevatedPoint = AixmObstacle.Part.Item(0).HorizontalProjection
			End if

			if pElevatedPoint is Nothing  continue For
			if pElevatedPoint.Elevation is Nothing  continue For

			pPtGeo = ARANPointToESRIPoint(pElevatedPoint)
			pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation.Elevation, -9999.0) + ConverterToSI.Convert(AixmObstacle.Part(0).verticalExtent, 0)

			pPtPrj = Functions.ToPrj(pPtGeo)
			if pPtPrj.IsEmpty()  continue For

			j = j + 1

			ObstList[j].pPtGeo = pPtGeo
			ObstList[j].pPtPrj = pPtPrj

			ObstList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
			ObstList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.Elevation.VerticalAccuracy, 0.0)

			fDist = pBaseProxi.ReturnDistance(ObstList[j].pPtPrj)

			ObstList[j].Height = ObstList[j].pPtGeo.Z - fRefHeight
			ObstList[j].Flags = -CShort(fDist = 0.0)
			ObsMOC = arIASegmentMOC.Value * (1.0 - 2 * fDist / arIFHalfWidth.Value)

			if FIXHeight <= ObstList[j].Height 
				ObsR = arIFHalfWidth.Value
			else
				ObsR = arIFHalfWidth.Value - 0.5 * arIFHalfWidth.Value * (FIXHeight - ObstList[j].Height) / arIASegmentMOC.Value 'MOC '
			End if

			if ObsR > arIFHalfWidth.Value  ObsR = arIFHalfWidth.Value

			if GuidNav.TypeCode <> 1 
				ObstList[j].Dist = Point2LineDistancePrj(ObstList[j].pPtPrj, LastPoint.pPtPrj, fDir + 90.0)
				ObstList[j].CLDist = Point2LineDistancePrj(ObstList[j].pPtPrj, LastPoint.pPtPrj, fDir)
			else
				ObstList[j].Dist = Functions.DegToRad(SubtractAngles(fAlpha, ReturnAngleInDegrees(GuidNav.pPtPrj, ObstList[j].pPtPrj))) * fL
				ObstList[j].CLDist = System.Math.Abs(fL - ReturnDistanceInMeters(ObstList[j].pPtPrj, GuidNav.pPtPrj))
			End if

			ObstList[j].MOC = ObsMOC
			ObstList[j].ReqH = ObstList[j].Height + ObsMOC
			ObstList[j].hPent = ObstList[j].ReqH - FIXHeight
			ObstList[j].fTmp = ObstList[j].hPent / ObstList[j].Dist

			ObstList[j].Name = AixmObstacle.Name
			ObstList[j].Identifier = AixmObstacle.Identifier
			ObstList[j].ID = AixmObstacle.Designator
			ObstList[j].Index = i
		Next i

		if j >= 0 
			ReDim Preserve ObstList[j]
		else
			ReDim ObstList(-1)
		End if

		Return j
}

	public int CalcIFProhibitions(ByVal LastPoint As StepDownFIX, ByVal fRefHeight As Double, ByVal IAF_FullAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByVal IAF_BaseAreaPoly As ESRI.ArcGIS.Geometry.IPolygon, ByRef ObstList() As ObstacleAr, ByRef ProhibitionSectors() As IFProhibitionSector)
{
		Dim i As Integer
		Dim j As Integer
		Dim n As Integer

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
		Dim pARANPolygon As MultiPolygon
		Dim ConverterToSI As ConvertToSI

		ReDim ObstList(-1)
		ReDim ProhibitionSectors(-1)

		pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(IAF_FullAreaPoly) as IPolygon)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		n = VerticalStructureList.Count - 1

		if n < 0  Return -1

		ConverterToSI = New ConvertToSI
		ReDim ObstList(n)
		ReDim ProhibitionSectors(n)

		pBaseProxi = IAF_BaseAreaPoly

		FIXHeight = LastPoint.pPtPrj.Z - fRefHeight
		fDir = LastPoint.InDir
		GuidNav = LastPoint.GuidNav

		fL = ReturnDistanceInMeters(GuidNav.pPtPrj, LastPoint.pPtPrj)
		fAlpha = ReturnAngleInDegrees(GuidNav.pPtPrj, LastPoint.pPtPrj)

		j = -1

		For i = 0 To n
			AixmObstacle = VerticalStructureList.Item(i)

			if AixmObstacle.Part.Count = 0  continue For
			if AixmObstacle.Part.Item(0).HorizontalProjection is Nothing  continue For

			pElevatedPoint = Nothing
			if AixmObstacle.Part.Item(0).HorizontalProjection.geometryType = Delib.Classes.Enums.GeometryType.Point 
				pElevatedPoint = AixmObstacle.Part.Item(0).HorizontalProjection
			End if

			if pElevatedPoint is Nothing  continue For
			if pElevatedPoint.Elevation is Nothing  continue For

			pPtGeo = ARANPointToESRIPoint(pElevatedPoint)
			pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation.Elevation, -9999.0) + ConverterToSI.Convert(AixmObstacle.Part(0).verticalExtent, 0)

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
				j = j + 1
				ObstList[j].pPtGeo = pPtGeo
				ObstList[j].pPtPrj = pPtPrj

				ObstList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
				ObstList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.Elevation.VerticalAccuracy, 0.0)

				ObstList[j].Dist = ObsDist
				ObstList[j].CLDist = ObsCLDist
				ObstList[j].Height = ObsHeight
				ObstList[j].Flags = -CShort(fDist = 0.0)

				ObstList[j].Name = AixmObstacle.Name
				ObstList[j].Identifier = AixmObstacle.Identifier
				ObstList[j].ID = AixmObstacle.Designator

				ObstList[j].MOC = ObsMOC
				ObstList[j].ReqH = ObsHeight + ObsMOC
				ObstList[j].hPent = ObstList[j].ReqH - FIXHeight
				ObstList[j].fTmp = ObstList[j].hPent / ObsDist
				ObstList[j].Index = j

				ProhibitionSectors(j).MOC = ObsMOC
				ProhibitionSectors(j).rObs = ObsR
				ProhibitionSectors(j).ObsArea = CreatePrjCircle(pPtPrj, ObsR)

				ProhibitionSectors(j).DirToObst = ReturnAngleInDegrees(LastPoint.pPtPrj, pPtPrj)
				ProhibitionSectors(j).DistToObst = ReturnDistanceInMeters(LastPoint.pPtPrj, pPtPrj)

				//            if ObsR < ProhibitionSectors(j).DistToObst 
				//                fTmp = ArcSin(rObs / ProhibitionSectors(j).DistToObst)
				//                ProhibitionSectors(j).AlphaFrom = Round(Modulus(ProhibitionSectors(j).DirToObst - RadToDeg(fTmp), 360.0) - 0.4999)
				//                ProhibitionSectors(j).AlphaTo = Round(Modulus(ProhibitionSectors(j).DirToObst + RadToDeg(fTmp), 360.0) + 0.4999)
				//            else
				//                ProhibitionSectors(j).AlphaFrom = 0
				//                ProhibitionSectors(j).AlphaTo = 360
				//            End if

				ProhibitionSectors(j).dHObst = ObsHeight - FIXHeight + ObsMOC
				ProhibitionSectors(j).Index = i
			End if
		Next i

		if j >= 0 
			ReDim Preserve ObstList[j]
			ReDim Preserve ProhibitionSectors(j)
		else
			ReDim ObstList(-1)
			ReDim ProhibitionSectors(-1)
		End if

		Return j
}

	public bool GetMSASectors(ByVal Nav As NavaidType, ByRef MSAList() As MSAType)
{
		Dim i As Integer
		Dim j As Integer
		Dim k As Integer
		Dim n As Integer
		Dim m As Integer
		Dim fTmp As Double
		Dim FromAngle As Double
		Dim ToAngle As Double

		Dim SAAList As List<SafeAltitudeArea)
		Dim SAASectorList As List<Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector)
		Dim SectorDefinition As Delib.Classes.Objects.Procedure.CircleSector

		Dim SafeAltitudeArea As SafeAltitudeArea
		Dim SafeAltitudeSector As Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector
		Dim ConverterToSI As ConvertToSI

		SAAList = pObjectDir.GetSafeAltitudeAreaList(Nav.pSignificantPoint)
		n = SAAList.Count

		if n = 0 
			ReDim MSAList(-1)
			Return False
		End if

		ReDim MSAList(n - 1)
		ConverterToSI = New ConvertToSI
		j = -1

		Dim pSignificantPoint As SignificantPoint
		Dim pNAVEq As NavaidEquipment
		Dim pADHP As AirportHeliport
		Dim pDesign As DesignatedPoint
		For i = 0 To n - 1
			SafeAltitudeArea = SAAList.Item(i)

			if SafeAltitudeArea.safeAreaType.Value = CodeSafeAltitudeType.MSA 

				SAASectorList = SafeAltitudeArea.sector
				//Set SAASectorList = pObjectDir.GetSafeAltitudeAreaSectorList(SafeAltitudeArea.Identifier)
				m = SAASectorList.Count
			if m> 0 then
				j = j + 1
				MSAList(j).Name = SafeAltitudeArea.Identifier
				ReDim MSAList(j).Sectors(m - 1)

				//            Set pFeaturePoint = pObjectDir.GetFeature(SafeAltitudeArea.CentrePoint.featurePoint).Cast()
				//            pFeaturePoint.


				pSignificantPoint = SafeAltitudeArea.centrePoint
				if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.Navaid 
					pNAVEq = pSignificantPoint
					//ptCenter = ARANPointToESRIPoint(pNAVEq.Location)
				else if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.AirportHeliport 
					pADHP = pSignificantPoint
					//ptCenter = ARANPointToESRIPoint(pADHP.ARP)
				else if pSignificantPoint.significantPointChoice = Delib.Classes.Enums.SignificantPointChoice.DesignatedPoint 
					pDesign = pSignificantPoint
					//ptCenter = ARANPointToESRIPoint(pDesign.Location)
				else
					continue For
				End if

				For k = 0 To m - 1
					SafeAltitudeSector = SAASectorList.Item(k)
					SectorDefinition = SafeAltitudeSector.sectorDefinition

					FromAngle = SectorDefinition.fromAngle.Value
					ToAngle = SectorDefinition.toAngle.Value

					if SectorDefinition.angleDirectionReference.Value = CodeDirectionReferenceType.TO 
						FromAngle = FromAngle + Math.PI
						ToAngle = ToAngle + Math.PI
					End if

					if SectorDefinition.arcDirection.Value = CodeArcDirectionType.CCA 
						fTmp = FromAngle
						FromAngle = ToAngle
						ToAngle = fTmp
					End if

					if SectorDefinition.AngleType.Value <> CodeBearing.TRUE 

					End if

					MSAList(j).Sectors(k).LowerLimit = ConverterToSI.Convert(SectorDefinition.lowerLimit, 0.0)
					MSAList(j).Sectors(k).UpperLimit = ConverterToSI.Convert(SectorDefinition.upperLimit, 0.0)

					MSAList(j).Sectors(k).InnerDist = ConverterToSI.Convert(SectorDefinition.innerDistance, 0.0)
					MSAList(j).Sectors(k).OuterDist = ConverterToSI.Convert(SectorDefinition.outerDistance, 19000.0)
					MSAList(j).Sectors(k).FromAngle = FromAngle
					MSAList(j).Sectors(k).ToAngle = ToAngle
					MSAList(j).Sectors(k).AbsAngle = SubtractAngles(FromAngle, ToAngle)

					//                Set MSAList(j).Sectors(k).Sector = CreateMSASectorPrj(ptCenter, MSAList(j).Sectors(k))
				Next k
			End if
			End if
		Next i

		if j >= 0 
			ReDim Preserve MSAList(j)
		else
			ReDim MSAList(-1)
		End if

		Return False
}
		*/
		//Private Function CreateMSASectorPrj(ByVal ptCntGeo As ESRI.ArcGIS.Geometry.IPoint, ByVal Sector As SectorMSA) As ESRI.ArcGIS.Geometry.IPolygon
		//	Dim i As Integer
		//	Dim n As Integer

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

		//	n = System.Math.Round(dAngle / AngStep)

		//	if (n < 1) 
		//		n = 1
		//	else if (n < 5) 
		//		n = 5
		//	else if (n < 10) 
		//		n = 10
		//	End if

		//	AngStep = dAngle / n

		//	For i = 0 To n
		//		iInRad = Functions.DegToRad(AngleFrom + i * AngStep)
		//		CosI = System.Math.Cos(iInRad)
		//		SinI = System.Math.Sin(iInRad)

		//		ptInner.X = ptCntPrj.X + Sector.InnerDist * CosI
		//		ptInner.Y = ptCntPrj.Y + Sector.InnerDist * SinI

		//		ptOuter.X = ptCntPrj.X + Sector.OuterDist * CosI
		//		ptOuter.Y = ptCntPrj.Y + Sector.OuterDist * SinI

		//		pPolygon.AddPoint(ptInner)
		//		pPolygon.AddPoint(ptOuter, 0)
		//	Next i

		//	pTopo = pPolygon
		//	pTopo.IsKnownSimple_2 = False
		//	pTopo.Simplify()

		//	Return pPolygon
		//End Function

		//public Function GetADHPWorkspace() As ESRI.ArcGIS.Geodatabase.IWorkspace
		//	Dim i As Integer
		//	Dim Map As ESRI.ArcGIS.Carto.IMap
		//	Dim fL As ESRI.ArcGIS.Carto.IFeatureLayer
		//	Dim ds As ESRI.ArcGIS.Geodatabase.IDataset

		//	Map = GetMap()

		//	For i = 0 To Map.LayerCount - 1
		//		if Map.Layer(i) is ESRI.ArcGIS.Carto.IFeatureLayer 
		//			if Map.Layer(i).Name = "ADHP" 
		//				fL = Map.Layer(i)
		//				ds = fL.FeatureClass
		//				Return ds.Workspace
		//			End if
		//		End if
		//	Next i

		//	Return Nothing
		//End Function

		public static Feature CreateDesignatedPoint(Point pPtPrj, string Name = "COORD", double fDir = -1000.0)
		{
			bool bExist;

			double fDist;
			double fDirToPt;

			WPT_FIXType WptFIX = new WPT_FIXType();
			DesignatedPoint pFixDesignatedPoint;

			double fMinDist = 10000.0;
			int n = GlobalVars.WPTList.Length;


			for (int i = 0; i < n; i++)
			{
				fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, GlobalVars.WPTList[i].pPtPrj);

				if (fDist < fMinDist)
				{
					fMinDist = fDist;
					WptFIX = GlobalVars.WPTList[i];

					if (fMinDist == 0.0)
						break;
				}
			}

			bExist = fMinDist <= 0.10;

			if (!bExist && fMinDist <= 100.0 && fDir != -1000.0)
			{
				fDirToPt = ARANFunctions.ReturnAngleInDegrees(pPtPrj, WptFIX.pPtPrj);
				bExist = ARANMath.SubtractAngles(fDir, fDirToPt) < 0.1;
			}

			if (bExist)
			{
				//return WptFIX.GetDesignatedPoint();

				pFixDesignatedPoint = new DesignatedPoint();

				if (WptFIX.TypeCode <= eNavaidType.NONE)
					pFixDesignatedPoint.Identifier = WptFIX.Identifier;
				else
					pFixDesignatedPoint.Identifier = WptFIX.NAV_Ident;
				return pFixDesignatedPoint;
			}

			pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature<DesignatedPoint>();
			pFixDesignatedPoint.Designator = "COORD";
			if (Name == "")
				Name = "COORD";

			pFixDesignatedPoint.Name = Name;

			AixmPoint aixmPoint = new AixmPoint();
			Point pPoint = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(pPtPrj);

			aixmPoint.Geo.X = pPoint.X;
			aixmPoint.Geo.Y = pPoint.Y;
			aixmPoint.Geo.Z = pPoint.Z;
			aixmPoint.Geo.M = pPoint.M;
			aixmPoint.Geo.T = 0;

			pFixDesignatedPoint.Location = aixmPoint;
			//pFixDesignatedPoint.Note;
			//pFixDesignatedPoint.Tag;
			pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.DESIGNED;
			return pFixDesignatedPoint;
		}

		//public static DesignatedPoint CreateDesignatedPoint(Point pPtPrj, string Name = "COORD", double fDirInRad = -1000.0)
		//{
		//	bool bExist;
		//	int i, n;
		//	Double fDist;
		//	Double fMinDist;
		//	Double fDirToPt;

		//	WPT_FIXType WptFIX = new WPT_FIXType();
		//	DesignatedPoint pFixDesignatedPoint;

		//	fMinDist = 10000.0;
		//	n = GlobalVars.WPTList.Length - 1;

		//	if (n >= 0)
		//		for (i = 0; i <= n; i++)
		//		{
		//			fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, GlobalVars.WPTList[i].pPtPrj);

		//			if (fDist < fMinDist)
		//			{
		//				fMinDist = fDist;
		//				WptFIX = GlobalVars.WPTList[i];
		//			}
		//		}

		//	bExist = fMinDist <= 10.0;

		//	if (!bExist && fMinDist <= 100.0 && fDirInRad != -1000.0)
		//	{
		//		fDirToPt = ARANFunctions.ReturnAngleInRadians(pPtPrj, WptFIX.pPtPrj);
		//		bExist = ARANMath.SubtractAngles(fDirInRad, fDirToPt) < ARANMath.EpsilonRadian;
		//	}

		//	if (bExist)
		//		return WptFIX.pFeature as DesignatedPoint;


		//	pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature<DesignatedPoint>();
		//	pFixDesignatedPoint.Designator = "COORD";
		//	if (Name == "")
		//		Name = "COORD";

		//	pFixDesignatedPoint.Name = Name;

		//	AixmPoint aixmPoint = new AixmPoint();
		//	Point pPoint = GlobalVars.pspatialReferenceOperation.ToGeo<Point>(pPtPrj);

		//	aixmPoint.Geo.X = pPoint.X;
		//	aixmPoint.Geo.Y = pPoint.Y;
		//	aixmPoint.Geo.Z = pPoint.Z;
		//	aixmPoint.Geo.M = pPoint.M;
		//	aixmPoint.Geo.T = 0;

		//	pFixDesignatedPoint.Location = aixmPoint;
		//	//pFixDesignatedPoint.Note;
		//	//pFixDesignatedPoint.Tag;
		//	pFixDesignatedPoint.Type = Aran.Aim.Enums.CodeDesignatedPoint.DESIGNED;
		//	return pFixDesignatedPoint;
		//}

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

				//pConnectionInfo.Server = new Uri("C:\\Program Files\\R.i.S.k. AirNavLab\\ProcViewer\\Data");
				//pConnectionInfo.UserName = "risk";
				//pConnectionInfo.Password = "CSaimdb";

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
