using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Queries;
using Aran.Queries.Panda_2;
//using Aran.Geometries.Operators;

namespace Aran.PANDA.RNAV.Arrival
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

		public static ValDistanceVertical CreateValAltitudeType(UomDistanceVertical uom, double value)
		{
			ValDistanceVertical res = new ValDistanceVertical();
			res.Uom = uom;
			res.Value = value;
			return res;
		}

		//public static int FillADHPList(ref ADHPType[] ADHPList, Guid organ, bool CheckILS = false)
		//{
		//	int i, n;

		//	Descriptor pName;
		//	List<Descriptor> pADHPNameList;

		//	ADHPList = new ADHPType[0];

		//	pADHPNameList = pObjectDir.GetAirportHeliportList(organ, CheckILS);	//, AirportHeliportFields_Designator + AirportHeliportFields_Id + AirportHeliportFields_ElevatedPoint

		//	if (pADHPNameList == null)
		//		return -1;

		//	n = pADHPNameList.Count;

		//	if (n == 0)
		//		return -1;

		//	System.Array.Resize(ref ADHPList, n);

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


			var geomOperators = new Geometries.Operators.JtsGeometryOperators();// GeometryOperators();

			//            GlobalVars.gAranGraphics.DrawMultiPolygon(prj, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);

			GlobalVars.p_LicenseRect = geomOperators.SimplifyGeometry(GlobalVars.p_LicenseRect, 0.001) as Aran.Geometries.MultiPolygon;
			if (!geomOperators.Contains(GlobalVars.p_LicenseRect, pPtPrj))
				return -1;

			CurrADHP.Name = pADHP.Designator;
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

						if (pElevatedPoint == null || pRunwayCenterLinePoint.Role == null)
							continue;

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

					//for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtDER; ek++)
					//{
					//	if (RWYList[iRwyNum].pPtGeo[ek] == null)
					//	{
					//		//if (ek == eRWY.PtTHR)									continue;
					//		iRwyNum--;
					//		goto NextI;
					//	}
					//}

					if (RWYList[iRwyNum].pPtGeo[eRWY.ptStart] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] == null || RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] == null)
					{
						iRwyNum--;
						continue;
						//goto NextI;
					}

					if (pRwyDirection.TrueBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
					else if (pRwyDirection.MagneticBearing != null)
						RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value - Owner.MagVar;
					else
					{
						double TrueBearing, fTmp;
						NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.ptStart].X, RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.ptDER].X, RWYList[iRwyNum].pPtGeo[eRWY.ptDER].Y, out TrueBearing, out fTmp);
						RWYList[iRwyNum].TrueBearing = TrueBearing;
					}

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
									fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fTODA);
								else if (pDirDeclaredDist.Type == CodeDeclaredDistance.DTHR)
									fDTHR = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fDTHR);
							}
						}
					}

					//List<RunwayDeclaredDistance> GetDeclaredDistance(Guid RCLPIdentifier);
					//DeclaredDistance pDirDeclaredDist;
					//pDeclaredDistList  = pObjectDir.g GetDeclaredDistanceList(pRunway.Identifier);

					if (fTODA < 0)
					{
						System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
						iRwyNum--;
						continue;
						//goto NextI;
					}
					RWYList[iRwyNum].TODA = fTODA;

					double ResX, ResY;
					NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.ptStart].X, RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Y, RWYList[iRwyNum].TODA, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.ptDER] = new Point(ResX, ResY);
					RWYList[iRwyNum].pPtGeo[eRWY.ptDER].Z = RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z; //PtEndZ;

					//RWYList[iRwyNum].ClearWay = 0.0;
					//pRunwayProtectAreaList = pObjectDir.GetRunwayProtectAreaList(pRwyDirection.Identifier);
					//for (int k = 0; k < pRunwayProtectAreaList.Count; k++)
					//{
					//    pRunwayProtectArea = pRunwayProtectAreaList[k];
					//    pAirportHeliportProtectionArea = pRunwayProtectArea;
					//    if (pRunwayProtectArea.Type.Value == CodeRunwayProtectionArea.CWY && pAirportHeliportProtectionArea.Length != null)
					//    {
					//        RWYList[iRwyNum].ClearWay = ConverterToSI.Convert(pAirportHeliportProtectionArea.Length, 0.0);
					//        break;
					//    }
					//}

					for (eRWY ek = eRWY.ptStart; ek <= eRWY.ptDER; ek++)
					{
						RWYList[iRwyNum].pPtPrj[ek] = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(RWYList[iRwyNum].pPtGeo[ek]);
						if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
						{
							iRwyNum--;
							goto NextI;
						}

						RWYList[iRwyNum].pPtGeo[ek].M = ARANMath.DegToRad(RWYList[iRwyNum].TrueBearing);
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
				//}

				//NextI: ;
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		//public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
		//{
		//	int n = NavaidList.GetLength(0);

		//	for (int i = 0; i < n; i++)
		//		if ((NavaidList[i].TypeCode == eNavaidType.LLZ) && (NavaidList[i].CallSign == ILS.CallSign))
		//			return i;

		//	System.Array.Resize<NavaidType>(ref NavaidList, n + 1);

		//	NavaidList[n].pPtGeo = ILS.pPtGeo;
		//	NavaidList[n].pPtPrj = ILS.pPtPrj;
		//	NavaidList[n].Name = ILS.CallSign;
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
		//	int pos = ProcName.IndexOf("RWY");

		//	if (pos <= 0)
		//		return 0;

		//	pos += 3;
		//	string RWYName = "";

		//	char chr = ProcName.ToCharArray(pos, 1)[0];
		//	while (chr != ' ')
		//	{
		//		RWYName = RWYName + chr;
		//		pos++;
		//		chr = ProcName.ToCharArray(pos, 1)[0];
		//	}

		//	//    ADHP.ID = OwnerID
		//	//    if FillADHPFields(ADHP) < 0  Exit Function

		//	RWYType[] RWYList;

		//	int n = FillRWYList(out RWYList, ADHP);

		//	for (int i = 0; i < n; i++)
		//		if (RWYList[i].Name == RWYName)
		//			return GetILS(RWYList[i], ref ILS, ADHP);

		//	return 0;

		//	//    if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		//}

		//public static long GetILS(RWYType RWY, ref ILSType ILS, ADHPType Owner)
		//{
		//	NavaidEquipment pAIXMNAVEq;
		//	Localizer pAIXMLocalizer;
		//	Glidepath pAIXMGlidepath;
		//	ElevatedPoint pElevPoint;

		//	ILS.index = 0;

		//	//if RWY.ILSID = ""  Exit Function

		//	Navaid pNavaid = pObjectDir.GetILSNavaid(RWY.Identifier);
		//	if (pNavaid == null)
		//		return 0;

		//	List<NavaidComponent> pAIXMNAVEqList = pNavaid.NavaidEquipment;
		//	if (pAIXMNAVEqList.Count == 0)
		//		return 0;

		//	ILS.Category = 4;
		//	//ILS.pFeature = pNavaid;
		//	ILS.NAV_Ident = pNavaid.Identifier;

		//	if (pNavaid.SignalPerformance != null)
		//		ILS.Category = (int)pNavaid.SignalPerformance.Value + 1;

		//	if (ILS.Category > 3)
		//		ILS.Category = 1;

		//	ILS.RWY_ID = RWY.Identifier;

		//	pAIXMLocalizer = null;
		//	pAIXMGlidepath = null;

		//	int j = 0;

		//	for (int i = 0; i < pAIXMNAVEqList.Count; i++)
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

		//		//ILS.Course = Modulus(ILS.Course + 180.0, 360.0)

		//		ILS.pPtGeo = pElevPoint.Geo;
		//		ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.ptTHR].Z);
		//		ILS.pPtGeo.M = ILS.Course;

		//		ILS.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ILS.pPtGeo);

		//		if (ILS.pPtPrj.IsEmpty)
		//			return 0;

		//		ILS.pPtPrj.M = ARANMath.DegToRad(GlobalVars.pspatialReferenceOperation.AztToDirGeo(ILS.pPtGeo, ILS.pPtGeo.M));

		//		double dX = RWY.pPtPrj[eRWY.ptTHR].X - ILS.pPtPrj.X;
		//		double dY = RWY.pPtPrj[eRWY.ptTHR].Y - ILS.pPtPrj.Y;
		//		ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

		//		if (pAIXMLocalizer.WidthCourse != null)
		//		{
		//			ILS.AngleWidth = (double)pAIXMLocalizer.WidthCourse.Value;
		//			ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(ARANMath.DegToRad(ILS.AngleWidth));

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
		//				if (ILS.index == 0)
		//				{
		//					ILS.Identifier = pAIXMNAVEq.Identifier;
		//					ILS.CallSign = pAIXMNAVEq.Designator;	//pNavaid.Designator
		//				}
		//				//        ILS.ID = pAIXMNAVEq.ID
		//				//        ILS.CallSign = pAIXMNAVEq.Designator
		//				ILS.index = ILS.index | 2;
		//			}
		//		}
		//	}

		//	return ILS.index;
		//}

		//public static void FillNavaidList(out NavaidType[] NavaidList, out NavaidType[] DMEList, ADHPType CurrADHP, double Radius)
		//{
		//	int i, j;
		//	int iNavaidNum, iDMENum;

		//	double fDist;

		//	eNavaidType NavTypeCode;
		//	List<Navaid> pNavaidList;

		//	Navaid pNavaid;
		//	NavaidEquipment AixmNavaidEquipment;

		//	Point pPtGeo;
		//	Point pPtPrj;

		//	ElevatedPoint pElevPoint;

		//	Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, Radius);

		//	Polygon pPolygon = new Polygon();
		//	pPolygon.ExteriorRing = ring;

		//	MultiPolygon pARANPolygon = new MultiPolygon();
		//	pARANPolygon.Add(pPolygon);
		//	pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

		//	pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

		//	// .GetNavaidEquipmentList (pARANPolygon)
		//	// Set ILSDataList = pObjectDir.GetILSNavaidEquipmentList(CurrADHP.ID)


		//	if (pNavaidList.Count == 0) 	//And (ILSDataList.Count = 0)
		//	{
		//		NavaidList = new NavaidType[0];
		//		DMEList = new NavaidType[0];
		//		return;
		//	}

		//	NavaidList = new NavaidType[pNavaidList.Count];		//+ ILSDataList.Count
		//	DMEList = new NavaidType[pNavaidList.Count];		//+ ILSDataList.Count

		//	iNavaidNum = -1;
		//	iDMENum = -1;

		//	for (j = 0; j < pNavaidList.Count; j++)
		//	{
		//		pNavaid = pNavaidList[j];
		//		for (i = 0; i < pNavaid.NavaidEquipment.Count; i++)
		//		{
		//			if (pNavaid.NavaidEquipment[i] == null)
		//				continue;

		//			if (pNavaid.NavaidEquipment[i].TheNavaidEquipment == null)
		//				continue;

		//			AixmNavaidEquipment = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

		//			if (AixmNavaidEquipment is VOR)
		//				NavTypeCode = eNavaidType.VOR;					//NavTypeName = "VOR";
		//			else if (AixmNavaidEquipment is DME)
		//				NavTypeCode = eNavaidType.DME;					//NavTypeName = "DME";
		//			else if (AixmNavaidEquipment is NDB)
		//				NavTypeCode = eNavaidType.NDB;					//NavTypeName = "NDB";
		//			else if (AixmNavaidEquipment is TACAN)
		//				NavTypeCode = eNavaidType.TACAN;				//NavTypeName = "Tacan";
		//			else
		//				continue;

		//			pElevPoint = AixmNavaidEquipment.Location;

		//			pPtGeo = pElevPoint.Geo;
		//			pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev);
		//			pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

		//			if (pPtPrj.IsEmpty)
		//				continue;

		//			if (NavTypeCode == eNavaidType.DME)
		//			{
		//				iDMENum++;

		//				DMEList[iDMENum].pPtGeo = pPtGeo;
		//				DMEList[iDMENum].pPtPrj = pPtPrj;

		//				if (AixmNavaidEquipment.MagneticVariation != null)
		//					DMEList[iDMENum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
		//				else
		//					DMEList[iDMENum].MagVar = CurrADHP.MagVar;

		//				DMEList[iDMENum].Range = 350000.0; //DME.Range
		//				DMEList[iDMENum].PairNavaidIndex = -1;

		//				DMEList[iDMENum].Name = AixmNavaidEquipment.Name;
		//				DMEList[iDMENum].NAV_Ident = pNavaid.Identifier;
		//				DMEList[iDMENum].Identifier = AixmNavaidEquipment.Identifier;
		//				DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

		//				DMEList[iDMENum].TypeCode = NavTypeCode;

		//				DMEList[iDMENum].index = iNavaidNum + iDMENum;

		//				//DMEList[iDMENum].pFeature = pNavaid;
		//			}
		//			else
		//			{
		//				iNavaidNum++;

		//				NavaidList[iNavaidNum].pPtGeo = pPtGeo;
		//				NavaidList[iNavaidNum].pPtPrj = pPtPrj;

		//				if (AixmNavaidEquipment.MagneticVariation != null)
		//					NavaidList[iNavaidNum].MagVar = (double)AixmNavaidEquipment.MagneticVariation.Value;
		//				else
		//					NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar;

		//				if (NavTypeCode == eNavaidType.NDB)
		//					NavaidList[iNavaidNum].Range = 350000.0;		//NDB.Range
		//				else
		//					NavaidList[iNavaidNum].Range = 350000.0;		//VOR.Range

		//				NavaidList[iNavaidNum].PairNavaidIndex = -1;

		//				NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name;
		//				NavaidList[iNavaidNum].NAV_Ident = pNavaid.Identifier;
		//				NavaidList[iNavaidNum].Identifier = AixmNavaidEquipment.Identifier;
		//				NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator;

		//				NavaidList[iNavaidNum].TypeCode = NavTypeCode;
		//				NavaidList[iNavaidNum].index = iNavaidNum + iDMENum;

		//				//NavaidList[iNavaidNum].pFeature = pNavaid;
		//			}
		//		}
		//	}

		//	for (j = 0; j <= iNavaidNum; j++)
		//		for (i = 0; i <= iDMENum; i++)
		//		{
		//			fDist = ARANFunctions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
		//			if (fDist <= 2.0)
		//			{
		//				NavaidList[j].PairNavaidIndex = i;
		//				DMEList[i].PairNavaidIndex = j;
		//				break;
		//			}
		//		}

		//	//    For I = 0 To ILSDataList.Count - 1
		//	//        Set AixmNavaidEquipment = ILSDataList.Item(I)
		//	//
		//	//        if Not (AixmNavaidEquipment is ILocalizer)  continue For
		//	//
		//	//        Set pElevPoint = AixmNavaidEquipment.ElevatedPoint
		//	//        Set pGMLPoint = pElevPoint
		//	//        Set pAIXMLocalizer = AixmNavaidEquipment
		//	//
		//	//        if pAIXMLocalizer.TrueBearing is Nothing  continue For
		//	//
		//	//        Set pPtGeo = pGMLPoint.Tag
		//	//        pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev)
		//	//        pPtGeo.M = pAIXMLocalizer.TrueBearing
		//	//
		//	//        Set pPtPrj = Functions.ToPrj(pPtGeo)
		//	//        if pPtPrj.IsEmpty()  continue For
		//	//
		//	//        pPtPrj.M = Functions.Azt2Dir(pPtGeo, pPtGeo.M)
		//	//'=====================================================
		//	//
		//	//        iNavaidNum = iNavaidNum + 1
		//	//
		//	//        Set NavaidList[iNavaidNum].pPtGeo = pPtGeo
		//	//        Set NavaidList[iNavaidNum].pPtPrj = pPtPrj
		//	//
		//	//        NavaidList[iNavaidNum].Course = pPtGeo.M
		//	//'        dX = RWY.pPtPrj(eRWY.PtTHR).X - pPtPrj.X
		//	//'        dY = RWY.pPtPrj(eRWY.PtTHR).Y - pPtPrj.Y
		//	//'        NavaidList[iNavaidNum].LLZ_THR = Sqr(dX * dX + dY * dY)
		//	//
		//	//        if Not pAIXMLocalizer.WidthCourse is Nothing 
		//	//            NavaidList[iNavaidNum].AngleWidth = pAIXMLocalizer.WidthCourse
		//	//'           NavaidList[iNavaidNum].SecWidth = NavaidList[iNavaidNum].LLZ_THR * Tan(Functions.DegToRad(NavaidList[iNavaidNum].AngleWidth))  'NavaidList[iNavaidNum].SecWidth = pAIXMLocalizer.WidthCourse
		//	//        End if
		//	//'=====================================================
		//	//
		//	//        if (Not AixmNavaidEquipment.MagneticVariation is Nothing) 
		//	//            NavaidList[iNavaidNum].MagVar = AixmNavaidEquipment.MagneticVariation.Value
		//	//        else
		//	//            NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar
		//	//        End if
		//	//
		//	//        NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name
		//	//        NavaidList[iNavaidNum].ID = AixmNavaidEquipment.Identifier
		//	//        NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator
		//	//
		//	//        NavaidList[iNavaidNum].Range = 35000#
		//	//        NavaidList[iNavaidNum].TypeCode = CodeLLZ
		//	//        NavaidList[iNavaidNum].TypeName = "ILS"
		//	//
		//	//'    GP_RDH = 0
		//	//
		//	//'    GPAngle = 0
		//	//
		//	//    Next I

		//	System.Array.Resize<NavaidType>(ref NavaidList, iNavaidNum + 1);
		//	System.Array.Resize<NavaidType>(ref DMEList, iDMENum + 1);
		//}

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

					if (AIXMNAVEq == null)
						continue;

					if (AIXMNAVEq.Location == null)
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

			System.Array.Sort<WPT_FIXType>(WPTList,
				(delegate(WPT_FIXType a, WPT_FIXType b)
			{
				return a.ToString().CompareTo(b.ToString());
			}));

			return iWPTNum;
		}

		//public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, MultiPolygon pARANPolygon, double refHeight = 0.0)
		//{
		//	int iWPTNum, i, j, n;

		//	eNavaidType NavTypeCode;

		//	List<DesignatedPoint> AIXMWPTList;
		//	List<Navaid> pNavaidList;

		//	Navaid pNavaid;
		//	NavaidEquipment AIXMNAVEq;
		//	DesignatedPoint AIXMWPT;

		//	Point pPtGeo;
		//	Point pPtPrj;

		//	//Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, radius);

		//	//Polygon pPolygon = new Polygon();
		//	//pPolygon.ExteriorRing = ring;

		//	//MultiPolygon pARANPolygon = new MultiPolygon();
		//	//pARANPolygon.Add(pPolygon);

		//	pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

		//	AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon);
		//	pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

		//	n = AIXMWPTList.Count + 2 * pNavaidList.Count;
		//	if (n == 0)
		//	{
		//		WPTList = new WPT_FIXType[0];
		//		return -1;
		//	}

		//	iWPTNum = -1;
		//	WPTList = new WPT_FIXType[n];


		//	for (i = 0; i < AIXMWPTList.Count; i++)
		//	{
		//		AIXMWPT = AIXMWPTList[i];

		//		pPtGeo = AIXMWPT.Location.Geo;
		//		pPtGeo.Z = refHeight + 300.0;
		//		pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

		//		if (pPtPrj.IsEmpty)
		//			continue;
		//		iWPTNum++;

		//		//if Not AIXMWPT.MagneticVariation is Nothing 
		//		//	WPTList[iWPTNum].MagVar = AIXMWPT.MagneticVariation.Value
		//		//else
		//		//	WPTList[iWPTNum].MagVar = CurrADHP.MagVar
		//		//End if
		//		WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

		//		WPTList[iWPTNum].pPtGeo = pPtGeo;
		//		WPTList[iWPTNum].pPtPrj = pPtPrj;

		//		WPTList[iWPTNum].Name = AIXMWPT.Designator;
		//		WPTList[iWPTNum].CallSign = AIXMWPT.Designator;
		//		WPTList[iWPTNum].NAV_Ident = AIXMWPT.Identifier;
		//		WPTList[iWPTNum].Identifier = AIXMWPT.Identifier;

		//		if (WPTList[iWPTNum].CallSign == null)
		//			WPTList[iWPTNum].CallSign = "";
		//		if (WPTList[iWPTNum].Name == null)
		//			WPTList[iWPTNum].Name = "";

		//		//WPTList[iWPTNum].pFeature = AIXMWPT;
		//		WPTList[iWPTNum].TypeCode = eNavaidType.NONE;
		//	}

		//	//======================================================================

		//	for (j = 0; j < pNavaidList.Count; j++)
		//	{
		//		pNavaid = pNavaidList[j];
		//		for (i = 0; i < pNavaid.NavaidEquipment.Count; i++)
		//		{
		//			if (pNavaid.NavaidEquipment[i] == null)
		//				continue;

		//			if (pNavaid.NavaidEquipment[i].TheNavaidEquipment == null)
		//				continue;

		//			AIXMNAVEq = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

		//			if (AIXMNAVEq is VOR)
		//				NavTypeCode = eNavaidType.VOR;
		//			else if (AIXMNAVEq is NDB)
		//				NavTypeCode = eNavaidType.NDB;
		//			else if (AIXMNAVEq is TACAN)
		//				NavTypeCode = eNavaidType.TACAN;
		//			else
		//				continue;

		//			if (AIXMNAVEq.Location == null)
		//				continue;

		//			pPtGeo = AIXMNAVEq.Location.Geo;
		//			pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, refHeight);

		//			pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
		//			if (pPtPrj.IsEmpty)
		//				continue;

		//			iWPTNum++;

		//			WPTList[iWPTNum].pPtGeo = pPtGeo;
		//			WPTList[iWPTNum].pPtPrj = pPtPrj;

		//			if (AIXMNAVEq.MagneticVariation != null)
		//				WPTList[iWPTNum].MagVar = (double)AIXMNAVEq.MagneticVariation.Value;
		//			else
		//				WPTList[iWPTNum].MagVar = 0.0; //CurrADHP.MagVar

		//			WPTList[iWPTNum].Name = AIXMNAVEq.Designator;
		//			WPTList[iWPTNum].NAV_Ident = pNavaid.Identifier;
		//			WPTList[iWPTNum].Identifier = AIXMNAVEq.Identifier;

		//			//WPTList[iWPTNum].pFeature = pNavaid;
		//			WPTList[iWPTNum].TypeCode = NavTypeCode;
		//		}
		//	}
		//	//======================================================================
		//	iWPTNum++;
		//	System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

		//	System.Array.Sort<WPT_FIXType>(WPTList,
		//		(delegate(WPT_FIXType a, WPT_FIXType b)
		//		{
		//			return a.ToString().CompareTo(b.ToString());
		//		}));

		//	return iWPTNum;
		//}

		public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, Aran.Geometries.Point start, Aran.Geometries.Point end, double direction, double refHeight = 0.0)
		{
			int iWPTNum, i, j, n;

			eNavaidType NavTypeCode;

			List<DesignatedPoint> AIXMWPTList = new List<DesignatedPoint>();
			List<Navaid> pNavaidList = new List<Navaid>();

			Navaid pNavaid;
			NavaidEquipment AIXMNAVEq;
			DesignatedPoint AIXMWPT;

			Point pPtGeo;
			Point pPtPrj;

			//pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

			//AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon);
			//pNavaidList = pObjectDir.GetNavaidList(pARANPolygon);

			var dpPtList = pObjectDir.GetFeatureList(FeatureType.DesignatedPoint).Cast<DesignatedPoint>().ToList();
			var navaidList = pObjectDir.GetFeatureList(FeatureType.Navaid).Cast<Navaid>().ToList();

			var distStartToEnd = ARANFunctions.ReturnDistanceInMeters(start, end) + 0.1;

			foreach (var dPoint in dpPtList)
			{
				if (dPoint == null) continue;
				if (dPoint.Location == null) continue;
				if (dPoint.Location.Geo == null) continue;

				var ptPrj = GlobalVars.pspatialReferenceOperation.ToPrj(dPoint.Location.Geo);
				if (ptPrj == null) continue;
				double dist, yShift;
				ARANFunctions.PrjToLocal(start, direction, ptPrj, out dist, out yShift);

				if (dist < distStartToEnd && Math.Abs(yShift) < 1)
					AIXMWPTList.Add(dPoint);
			}

			foreach (var dNavid in navaidList)
			{
				if (dNavid == null) continue;
				if (dNavid.Location == null) continue;
				if (dNavid.Location.Geo == null) continue;

				var ptPrj = GlobalVars.pspatialReferenceOperation.ToPrj(dNavid.Location.Geo);
				if (ptPrj == null) continue;
				double dist, yShift;
				ARANFunctions.PrjToLocal(start, direction, ptPrj, out dist, out yShift);

				if (dist < distStartToEnd && Math.Abs(yShift) < 1)
					pNavaidList.Add(dNavid);
			}

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
				pPtGeo.Z = refHeight + 300.0;
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

					if (AIXMNAVEq.Location == null)
						continue;

					//    For I = 0 To AIXMNAVList.Count - 1
					//        Set AIXMNAVEq = AIXMNAVList.Item(I).Cast()

					//        Set pElevPoint = AIXMNAVEq.Location
					//        Set pGMLPoint = pElevPoint

					pPtGeo = AIXMNAVEq.Location.Geo;
					pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, refHeight);

					pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
					if (pPtPrj.IsEmpty)
						continue;

					if (AIXMNAVEq is VOR)
						NavTypeCode = eNavaidType.VOR;
					else if (AIXMNAVEq is NDB)
						NavTypeCode = eNavaidType.NDB;
					else if (AIXMNAVEq is TACAN)
						NavTypeCode = eNavaidType.TACAN;
					else
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

			System.Array.Sort<WPT_FIXType>(WPTList,
				(delegate(WPT_FIXType a, WPT_FIXType b)
				{
					return a.ToString().CompareTo(b.ToString());
				}));

			return iWPTNum;
		}

		//public static int GetObstaclesByPoly(out ObstacleType[] ObstacleList, MultiPolygon pPoly, double fRefHeight = 0.0)
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

		//		j++;
		//		ObstacleList[j].pGeoGeometry = pPtGeo;
		//		ObstacleList[j].pPrjGeometry = pPtPrj;

		//		ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//		ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//		ObstacleList[j].Name = AixmObstacle.Name;
		//		ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//		ObstacleList[j].ID = AixmObstacle.Id;
		//		ObstacleList[j].Height = pPtGeo.Z - fRefHeight;
		//		ObstacleList[j].X = 0.0;
		//		ObstacleList[j].index = i;
		//	}
		//	j++;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//	return j;
		//}

		//static public int GetObstaclesByDist(out ObstacleType[] ObstacleList, Point ptCenter, double MaxDist, double fRefHeight = 0.0)
		//{
		//	int i, j, n;

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

		//	j = -1;

		//	for (i = 0; i < n; i++)
		//	{
		//		//if ((i & 0x3FF) == 0)
		//		//	System.Windows.Forms.Application.DoEvents();

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

		//		j++;
		//		ObstacleList[j].pGeoGeometry = pPtGeo;
		//		ObstacleList[j].pPrjGeometry = pPtPrj;
		//		ObstacleList[j].Name = AixmObstacle.Name;
		//		ObstacleList[j].Identifier = AixmObstacle.Identifier;
		//		ObstacleList[j].ID = AixmObstacle.Id;

		//		ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//		ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//		ObstacleList[j].Height = pPtGeo.Z - fRefHeight;
		//		ObstacleList[j].X = ARANFunctions.ReturnDistanceInMeters(ptCenter, pPtPrj);
		//		ObstacleList[j].index = i;
		//	}
		//	j++;
		//	System.Array.Resize<ObstacleType>(ref ObstacleList, j);
		//	return j;
		//}

		//static void RemoveSeamPoints(ref MultiPoint pPoints)
		//{
		//	int n = pPoints.Count;
		//	int j = 0;
		//	while (j < n - 1)
		//	{
		//		Point pCurrPt = pPoints[j];
		//		int i = j + 1;
		//		while (i < n)
		//		{
		//			double dx = pCurrPt.X - pPoints[i].X;
		//			double dy = pCurrPt.Y - pPoints[i].Y;

		//			double fDist = dx * dx + dy * dy;
		//			if (fDist < ARANMath.EpsilonDistance)
		//			{
		//				pPoints.Remove(i);
		//				n--;
		//			}
		//			else
		//				i++;
		//		}
		//		j++;
		//	}
		//}

		//public static int GetObstaclesByPoly(out ObstacleContainer ObstacleList, MultiPolygon pPoly, double fRefHeight = 0.0)
		//{
		//	MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

		//	List<VerticalStructure> VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
		//	int n = VerticalStructureList.Count;

		//	ObstacleList.Parts = new ObstacleData[0];
		//	ObstacleList.Obstacles = new Obstacle[n];

		//	if (n == 0)
		//		return 0;

		//	var fullGeoOp = new Geometries.Operators.JtsGeometryOperators();// GeometryOperators();

		//	int maxObstacles = n, l = -1;

		//	double Z, HorAccuracy, VertAccuracy;
		//	Z = HorAccuracy = VertAccuracy = 0.0;

		//	Geometry pGeomGeo = null, pGeomPrj;

		//	for (int i = 0; i < n; i++)
		//	{
		//		ElevatedPoint pElevatedPoint;
		//		ElevatedCurve pElevatedCurve;
		//		ElevatedSurface pElevatedSurface;

		//		VerticalStructure AixmObstacle = VerticalStructureList[i];
		//		int m = AixmObstacle.Part.Count;

		//		for (int JJ = 0; JJ < m; JJ++)
		//		{
		//			if (AixmObstacle.Part[JJ].HorizontalProjection == null)
		//				continue;

		//			VerticalStructurePart ObstaclePart = AixmObstacle.Part[JJ];

		//			switch (ObstaclePart.HorizontalProjection.Choice)
		//			{
		//				case VerticalStructurePartGeometryChoice.ElevatedPoint:
		//					pElevatedPoint = ObstaclePart.HorizontalProjection.Location;
		//					if (pElevatedPoint == null) continue;
		//					if (pElevatedPoint.Elevation == null) continue;

		//					HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
		//					VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

		//					pGeomGeo = pElevatedPoint.Geo;
		//					//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
		//					Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
		//					break;
		//				case VerticalStructurePartGeometryChoice.ElevatedCurve:
		//					pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent;
		//					if (pElevatedCurve == null) continue;
		//					if (pElevatedCurve.Elevation == null) continue;

		//					HorAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
		//					VertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

		//					pGeomGeo = pElevatedCurve.Geo;
		//					//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
		//					Z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
		//					break;
		//				case VerticalStructurePartGeometryChoice.ElevatedSurface:
		//					pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
		//					if (pElevatedSurface == null) continue;
		//					if (pElevatedSurface.Elevation == null) continue;

		//					HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
		//					VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

		//					pGeomGeo = pElevatedSurface.Geo;
		//					//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
		//					Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
		//					break;
		//				default:
		//					continue;
		//			}

		//			pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
		//			if (pGeomPrj.IsEmpty) continue;

		//			if (VertAccuracy > 0.0)
		//				Z += VertAccuracy;

		//			//HorAccuracy = 0.0;
		//			if (HorAccuracy > 0.0)
		//			{
		//				if (pGeomPrj.Type == GeometryType.Point && (HorAccuracy <= 2.0))
		//				{
		//					Polygon pOstGeom = new Polygon();
		//					pOstGeom.ExteriorRing = ARANFunctions.CreateCirclePrj((Point)pGeomPrj, HorAccuracy, 18);
		//					pGeomPrj = pOstGeom;
		//				}
		//				else
		//					pGeomPrj = fullGeoOp.Buffer(pGeomPrj, HorAccuracy);

		//				pGeomGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Geometry>(pGeomPrj);
		//			}

		//			pGeomPrj.SetConstantZ(Z);
		//			pGeomGeo.SetConstantZ(Z);

		//			l++;

		//			if (l >= maxObstacles)
		//			{
		//				maxObstacles += n;
		//				Array.Resize<Obstacle>(ref ObstacleList.Obstacles, maxObstacles);
		//			}

		//			ObstacleList.Obstacles[l].pGeomGeo = pGeomGeo;
		//			ObstacleList.Obstacles[l].pGeomPrj = pGeomPrj;
		//			//ObstacleList.Obstacles[l].pFeature = AixmObstacle;
		//			ObstacleList.Obstacles[l].UnicalName = AixmObstacle.Name;
		//			if (AixmObstacle.Type == null)
		//				ObstacleList.Obstacles[l].TypeName = "";
		//			else
		//				ObstacleList.Obstacles[l].TypeName = AixmObstacle.Type.ToString();

		//			ObstacleList.Obstacles[l].Identifier = AixmObstacle.Identifier;
		//			ObstacleList.Obstacles[l].ID = AixmObstacle.Id;

		//			ObstacleList.Obstacles[l].HorAccuracy = HorAccuracy;
		//			ObstacleList.Obstacles[l].VertAccuracy = VertAccuracy;

		//			ObstacleList.Obstacles[l].Height = Z - fRefHeight;
		//			ObstacleList.Obstacles[l].Index = l;

		//			ObstacleList.Obstacles[l].PartsNum = 0;
		//			ObstacleList.Obstacles[l].Parts = null;
		//		}
		//	}

		//	l++;
		//	System.Array.Resize<Obstacle>(ref ObstacleList.Obstacles, l);

		//	return l;
		//}

		public static int GetObstaclesByDist(out ObstacleContainer ObstacleList, Point ptCenter, double MaxDist, double fRefHeight = 0.0)
		{
			var pAranPointPrj = GlobalVars.pspatialReferenceOperation.ToGeo<Aran.Geometries.Point>(ptCenter);
			var verticalStructureList = pObjectDir.GetVerticalStructureList(pAranPointPrj, MaxDist);

			return ProcessObstacles(out ObstacleList, verticalStructureList, fRefHeight);
		}

		public static int GetAnnexObstacle(out ObstacleContainer obstList, Point ptCenter, AirportHeliport adhp, double fRefHeight = 0.0)
		{
			List<VerticalStructure> verticalStructureList = pObjectDir.GetAnnexVerticalStructureList(adhp.Identifier);
			return ProcessObstacles(out obstList, verticalStructureList, fRefHeight);
		}

		static int ProcessObstacles(out ObstacleContainer obstList, List<VerticalStructure> verticalStructureList, double fRefHeight = 0.0)
		{
			int n = verticalStructureList.Count;

			obstList.Parts = new ObstacleData[0];
			obstList.Obstacles = new Obstacle[n];

			if (n == 0)
				return 0;

			//Stopwatch st = new Stopwatch();
			//st.Start();

			//GeometryOperators fullGeoOp = new GeometryOperators();
			var fullGeoOp = new Geometries.Operators.JtsGeometryOperators();

			int maxObstacles = n, l = -1;

			double horAccuracy, vertAccuracy;
			var z = horAccuracy = vertAccuracy = 0.0;

			for (int i = 0; i < n; i++)
			{
				VerticalStructure aixmObstacle = verticalStructureList[i];
				int m = aixmObstacle.Part.Count;

				for (int jj = 0; jj < m; jj++)
				{
					if (aixmObstacle.Part[jj].HorizontalProjection == null)
						continue;

					VerticalStructurePart ObstaclePart = aixmObstacle.Part[jj];

					Geometry pGeomGeo = null;
					switch (ObstaclePart.HorizontalProjection.Choice)
					{
						case VerticalStructurePartGeometryChoice.ElevatedPoint:
							var pElevatedPoint = ObstaclePart.HorizontalProjection.Location;
							if (pElevatedPoint.Elevation == null) continue;

							horAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
							vertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedPoint.Geo;
							//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
							z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							var pElevatedCurve = ObstaclePart.HorizontalProjection.LinearExtent;
							if (pElevatedCurve.Elevation == null) continue;

							horAccuracy = ConverterToSI.Convert(pElevatedCurve.HorizontalAccuracy, 0.0);
							vertAccuracy = ConverterToSI.Convert(pElevatedCurve.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedCurve.Geo;
							//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
							z = ConverterToSI.Convert(pElevatedCurve.Elevation, -9999.0);
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							var pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
							if (pElevatedSurface.Elevation == null) continue;

							horAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
							vertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

							pGeomGeo = pElevatedSurface.Geo;
							//pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
							z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
							break;
						default:
							continue;
					}

					var pGeomPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Geometry>(pGeomGeo);
					if (pGeomPrj.IsEmpty) continue;

					if (vertAccuracy > 0.0)
						z += vertAccuracy;

					// HorAccuracy = 0.0;
					if (horAccuracy > 0.0)
					{
						if (pGeomPrj.Type != GeometryType.Point)
						{
							int rd = horAccuracy <= 2.0 ? 18 : 36;
							pGeomPrj = fullGeoOp.Buffer(pGeomPrj, horAccuracy);
						}

						// pGeomGeo = GlobalVars.pspatialReferenceOperation.ToGeo<Geometry>(pGeomPrj);
					}

					pGeomPrj.SetConstantZ(z);
					pGeomGeo.SetConstantZ(z);

					l++;

					if (l >= maxObstacles)
					{
						maxObstacles += n;
						Array.Resize<Obstacle>(ref obstList.Obstacles, maxObstacles);
					}

					obstList.Obstacles[l].pGeomGeo = pGeomGeo;
					obstList.Obstacles[l].pGeomPrj = pGeomPrj;
					//ObstacleList.Obstacles[l].pFeature = AixmObstacle;
					obstList.Obstacles[l].UnicalName = aixmObstacle.Name;
					obstList.Obstacles[l].TypeName = aixmObstacle.Type == null ? "" : aixmObstacle.Type.ToString();

					obstList.Obstacles[l].Identifier = aixmObstacle.Identifier;
					obstList.Obstacles[l].ID = aixmObstacle.Id;

					obstList.Obstacles[l].HorAccuracy = horAccuracy;
					obstList.Obstacles[l].VertAccuracy = vertAccuracy;

					obstList.Obstacles[l].Height = z - fRefHeight;
					obstList.Obstacles[l].Index = l;

					obstList.Obstacles[l].PartsNum = 0;
					//obstList.Obstacles[l].Parts = null;
				}
			}

			l++;
			System.Array.Resize<Obstacle>(ref obstList.Obstacles, l);
			//st.Stop();
			return l;
		}

		// SignificantPoint 
		public static Feature CreateDesignatedPoint(WayPoint wayPoint, double fDirInRad = -1000.0, double sDistTresh =0.1)
		{
			Point pPtPrj = wayPoint.PrjPt;
			string Name = wayPoint.Name;  

			WPT_FIXType WptFIX = default(WPT_FIXType);			//= new WPT_FIXType();
			double fMinDist = double.MaxValue;
			int n = GlobalVars.WPTList.Length;
			bool bExist = false;

			if (wayPoint.Id != Guid.Empty)
			{
				for (int i = 0; i < n; i++)
					if(wayPoint.Id == GlobalVars.WPTList[i].Identifier)
					{
						WptFIX = GlobalVars.WPTList[i];
						bExist = true;
						break;
					}
			}
			else
			{
				for (int i = 0; i < n; i++)
				{
					double fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, GlobalVars.WPTList[i].pPtPrj);

					if (fDist < fMinDist)
					{
						fMinDist = fDist;
						WptFIX = GlobalVars.WPTList[i];
					}

					if (fMinDist == 0.0)
						break;
				}

				bExist = fMinDist <= sDistTresh;

				if (!bExist && fMinDist <= 100.0 && fDirInRad != -1000.0)			//???????????????
				{
					double fDirToPt = ARANFunctions.ReturnAngleInRadians(pPtPrj, WptFIX.pPtPrj);
					bExist = ARANMath.SubtractAngles(fDirInRad, fDirToPt) < ARANMath.EpsilonRadian;
				}
			}

			if (bExist)
				return WptFIX.GetFeature();
			//return WptFIX.pFeature;

			DesignatedPoint pFixDesignatedPoint = DBModule.pObjectDir.CreateFeature<DesignatedPoint>();
			if (Name == "")
				Name = "COORD";

			pFixDesignatedPoint.Designator = Name;
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

				//pConnectionInfo.Server = new Uri("C:\\Program Files\\R.I.S.K. AirNavLab\\ProcViewer\\Data");
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

/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/