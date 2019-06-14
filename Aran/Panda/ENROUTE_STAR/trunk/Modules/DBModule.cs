using System.Collections.Generic;
using System;

//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.ArcMapUI;
//using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Aim.Enums;

using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;

using Aran.Queries.Panda_2;
using Aran.Queries;
using Aran.Panda.Common;

namespace Aran.Panda.EnrouteStar
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

		public static void OpenTableFromFile(out ITable pTable, string sFolderName, string sFileName)
		{
			IWorkspaceFactory pFact = new ShapefileWorkspaceFactory();
			IWorkspace pWorkspace = pFact.OpenFromFile(sFolderName, GlobalVars.GetApplicationHWnd());
			pTable = ((IFeatureWorkspace)(pWorkspace)).OpenTable(sFileName);
		}

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

			ADHPList = GlobalVars.InitArray<ADHPType>(0);

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

			Point pPtGeo = pADHP.ARP.Geo;
			pPtGeo.Z = ConverterToSI.Convert(pADHP.ARP.Elevation, 0);

			Point pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);

			if (pPtPrj.IsEmpty)
				return -1;

			//Functions.DrawPolygon(GlobalVars.p_LicenseRect);
			//Functions.DrawPointWithText(pPtPrj, "Arpt");

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

			//CurADHP.MinTMA = ConverterToSI.Convert(pADHP.transitionAltitude, 2500.0)
			//            CurADHP.TransitionAltitude = ConverterToSI.ConvertToSI(ah.TransitionAltitude)

			CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);
			//            CurADHP.ReferenceTemperature = ConverterToSI.ConvertToSI(ah.ReferenceTemperature)

			CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}

		public static int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
		{
			int iRwyNum, i, j, k;
			double ResX, ResY, fTmp, TrueBearing;

			Descriptor pName;
			List<Descriptor> pAIXMRWYList;
			List<RunwayDirection> pRwyDRList;
			ElevatedPoint pElevatedPoint;
			List<RunwayCentrelinePoint> pCenterLinePointList;
			List<RunwayProtectArea> pRunwayProtectAreaList;

			Runway pRunway;
			RunwayDirection pRwyDirection;
			RunwayDirection pRwyDirectinPair;
			RunwayProtectArea pRunwayProtectArea;
			RunwayCentrelinePoint pRunwayCenterLinePoint;

			AirportHeliportProtectionArea pAirportHeliportProtectionArea;

			pAIXMRWYList = pObjectDir.GetRunwayList(Owner.pAirportHeliport.Identifier);	//, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

			iRwyNum = -1;
			if (pAIXMRWYList.Count == 0)
			{
				RWYList = new RWYType[0];
				return -1;
			}

			RWYList = GlobalVars.InitArray<RWYType>(2 * pAIXMRWYList.Count);

			ResX = 0;
			ResY = 0;
			TrueBearing = 0;
			fTmp = 0;

			for (i = 0; i < pAIXMRWYList.Count; i++)
			{
				pName = pAIXMRWYList[i];
				pRunway = (Runway)pObjectDir.GetFeature(FeatureType.Runway, pName.Identifier);

				pRwyDRList = pObjectDir.GetRunwayDirectionList(pRunway.Identifier);

				if (pRwyDRList.Count == 2)
				{
					for (j = 0; j < 2; j++)
					{
						iRwyNum++;

						RWYList[iRwyNum].Length = ConverterToSI.Convert(pRunway.NominalLength, -9999.0);
						if (RWYList[iRwyNum].Length < 0)
						{
							iRwyNum--;
							break;
						}

						pRwyDirection = pRwyDRList[j];
						RWYList[iRwyNum].Initialize();
						RWYList[iRwyNum].pRunwayDir = pRwyDirection;

						pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
						for (k = 0; k < pCenterLinePointList.Count; k++)
						{
							pRunwayCenterLinePoint = pCenterLinePointList[k];
							pElevatedPoint = pRunwayCenterLinePoint.Location;

							if (pRunwayCenterLinePoint.Role != null)
							{
								switch (pRunwayCenterLinePoint.Role.Value)//Select Case pRunwayCenterLinePoint.Role.Value
								{
									case CodeRunwayPointRole.START:
										RWYList[iRwyNum].pPtGeo[eRWY.PtStart] = pElevatedPoint.Geo;
										RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
										//RWYList[iRwyNum].pSignificantPoint[eRWY.PtStart] = pRunwayCenterLinePoint;
										break;
									case CodeRunwayPointRole.THR:
										RWYList[iRwyNum].pPtGeo[eRWY.PtTHR] = pElevatedPoint.Geo;
										RWYList[iRwyNum].pPtGeo[eRWY.PtTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
										//RWYList[iRwyNum].pSignificantPoint[eRWY.PtTHR] = pRunwayCenterLinePoint;
										break;
									//case CodeRunwayPointRole.END:
									//    RWYList[iRwyNum].pPtGeo[eRWY.PtDER] = pElevatedPoint.Geo;
									//    RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
									//    //RWYList[iRwyNum].pSignificantPoint[eRWY.PtEnd] = pRunwayCenterLinePoint;
									//    break;
								}
							}
						}

						for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtTHR; ek++)
						{
							if (RWYList[iRwyNum].pPtGeo[ek] == null)
							{
								iRwyNum--;
								goto NextI;
							}
						}

						if (pRwyDirection.TrueBearing != null)
							RWYList[iRwyNum].TrueBearing = pRwyDirection.TrueBearing.Value;
						else if (pRwyDirection.MagneticBearing != null)
							RWYList[iRwyNum].TrueBearing = pRwyDirection.MagneticBearing.Value - Owner.MagVar;
						else
						{
							NativeMethods.ReturnGeodesicAzimuth(RWYList[iRwyNum].pPtGeo[eRWY.PtStart].X, RWYList[iRwyNum].pPtGeo[eRWY.PtStart].Y, RWYList[iRwyNum].pPtGeo[eRWY.PtDER].X, RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Y, out TrueBearing, out fTmp);
							RWYList[iRwyNum].TrueBearing = TrueBearing;
						}


						RWYList[iRwyNum].Identifier = pRwyDirection.Identifier;
						RWYList[iRwyNum].Name = pRwyDirection.Designator;
						RWYList[iRwyNum].ADHP_ID = Owner.Identifier;
						//RWYList[iRwyNum].ILSID = pRwyDirection .ILS_ID;

						pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
						RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
						RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;


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
						//    NativeMethods.PointAlongGeodesic(RWYList[iRwyNum].pPtGeo[eRWY.PtDER].X, RWYList[iRwyNum].pPtGeo[eRWY.PtDER].Y, RWYList[iRwyNum].ClearWay, RWYList[iRwyNum].TrueBearing, out ResX, out ResY);
						//    RWYList[iRwyNum].pPtGeo[eRWY.PtDER].SetCoords(ResX, ResY);
						//}

						for (eRWY ek = eRWY.PtStart; ek <= eRWY.PtTHR; ek++)
						{
							RWYList[iRwyNum].pPtPrj[ek] = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(RWYList[iRwyNum].pPtGeo[ek]);
							if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
							{
								iRwyNum--;
								break;
							}

							RWYList[iRwyNum].pPtGeo[ek].M = ARANMath.DegToRad(RWYList[iRwyNum].TrueBearing);
							RWYList[iRwyNum].pPtPrj[ek].M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
						}
					}
				}
			NextI: ;
			}
			System.Array.Resize(ref RWYList, iRwyNum + 1);
			return iRwyNum + 1;
		}

		public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
		{
			int i, n;

			n = NavaidList.GetLength(0);

			for (i = 0; i < n; i++)
				if ((NavaidList[i].TypeCode == eNavaidType.CodeLLZ) && (NavaidList[i].CallSign == ILS.CallSign))
					return i;

			System.Array.Resize<NavaidType>(ref NavaidList, n + 1);

			NavaidList[n].pPtGeo = ILS.pPtGeo;
			NavaidList[n].pPtPrj = ILS.pPtPrj;
			NavaidList[n].Name = ILS.CallSign;
			NavaidList[n].Identifier = ILS.Identifier;
			NavaidList[n].CallSign = ILS.CallSign;

			NavaidList[n].MagVar = ILS.MagVar;
			NavaidList[n].TypeCode = eNavaidType.CodeLLZ;
			NavaidList[n].Range = 40000.0;
			NavaidList[n].index = ILS.index;
			NavaidList[n].PairNavaidIndex = -1;

			NavaidList[n].GPAngle = ILS.GPAngle;
			NavaidList[n].GP_RDH = ILS.GP_RDH;

			NavaidList[n].Course = ILS.Course;
			NavaidList[n].LLZ_THR = ILS.LLZ_THR;
			NavaidList[n].SecWidth = ILS.SecWidth;
			NavaidList[n].AngleWidth = ILS.AngleWidth;
			NavaidList[n].Tag = 0;
			NavaidList[n].pFeature = ILS.pFeature;

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

		public static long GetILSByName(string ProcName, ADHPType ADHP, ref ILSType ILS)
		{
			char chr;
			int i, n, pos;
			string RWYName;
			RWYType[] RWYList;

			pos = ProcName.IndexOf("RWY");

			if (pos <= 0)
				return 0;

			pos += 3;
			RWYName = "";

			chr = ProcName.ToCharArray(pos, 1)[0];
			while (chr != ' ')
			{
				RWYName = RWYName + chr;
				pos++;
				chr = ProcName.ToCharArray(pos, 1)[0];
			}

			//    ADHP.ID = OwnerID
			//    if FillADHPFields(ADHP) < 0  Exit Function

			n = FillRWYList(out RWYList, ADHP);

			for (i = 0; i < n; i++)
				if (RWYList[i].Name == RWYName)
					return GetILS(RWYList[i], ref ILS, ADHP);

			return 0;

			//    if ILS.CallSign <> ILSCallSign  GetILSByName = -1
		}

		public static long GetILS(RWYType RWY, ref ILSType ILS, ADHPType Owner)
		{
			int i, j;
			double dX, dY;
			string CallSign;

			Navaid pNavaidCom;
			List<NavaidComponent> pAIXMNAVEqList;

			NavaidEquipment pAIXMNAVEq;
			Localizer pAIXMLocalizer;
			Glidepath pAIXMGlidepath;
			ElevatedPoint pElevPoint;
			Guid ID = Guid.Empty;

			ILS.index = 0;

			//if RWY.ILSID = ""  Exit Function

			pNavaidCom = pObjectDir.GetILSNavaid(RWY.pRunwayDir.Identifier);
			if (pNavaidCom == null)
				return 0;

			pAIXMNAVEqList = pNavaidCom.NavaidEquipment;
			if (pAIXMNAVEqList.Count == 0)
				return 0;

			ILS.Category = 4;
			ILS.pFeature = pNavaidCom;

			if (pNavaidCom.SignalPerformance != null)
				ILS.Category = (int)pNavaidCom.SignalPerformance.Value + 1;

			if (ILS.Category > 3)
				ILS.Category = 1;

			ILS.RWY_ID = RWY.Identifier;

			pAIXMLocalizer = null;
			pAIXMGlidepath = null;
			//ID = null;
			CallSign = "";
			j = 0;

			for (i = 0; i < pAIXMNAVEqList.Count; i++)
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

				if (pAIXMLocalizer.TrueBearing != null)
					ILS.Course = pAIXMLocalizer.TrueBearing.Value;
				else if (pAIXMLocalizer.MagneticBearing != null)
					ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
				else
					goto NoLocalizer;

				//ILS.Course = Modulus(ILS.Course + 180.0, 360.0)

				ILS.pPtGeo = pElevPoint.Geo;
				ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.PtTHR].Z);
				ILS.pPtGeo.M = ARANMath.DegToRad(ILS.Course);

				ILS.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ILS.pPtGeo);

				if (ILS.pPtPrj.IsEmpty)
					return 0;

				ILS.pPtPrj.M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(ILS.pPtGeo, ILS.pPtGeo.M);

				dX = RWY.pPtPrj[eRWY.PtTHR].X - ILS.pPtPrj.X;
				dY = RWY.pPtPrj[eRWY.PtTHR].Y - ILS.pPtPrj.Y;
				ILS.LLZ_THR = System.Math.Sqrt(dX * dX + dY * dY);

				if (pAIXMLocalizer.WidthCourse != null)
				{
					ILS.AngleWidth = (double)pAIXMLocalizer.WidthCourse.Value;
					ILS.SecWidth = ILS.LLZ_THR * System.Math.Tan(ARANMath.DegToRad(ILS.AngleWidth));

					ILS.index = 1;
					ILS.Identifier = pAIXMNAVEq.Identifier;
					ILS.CallSign = pAIXMNAVEq.Designator;	//pNavaidCom.Designator '

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
					ILS.GPAngle = pAIXMGlidepath.Slope.Value;

					if (pAIXMGlidepath.Rdh != null)
					{
						ILS.GP_RDH = ConverterToSI.Convert(pAIXMGlidepath.Rdh, ILS.pPtGeo.Z);

						ID = pAIXMNAVEq.Identifier;
						CallSign = pAIXMNAVEq.Designator;	//pNavaidCom.Designator
						//        ILS.ID = pAIXMNAVEq.ID
						//        ILS.CallSign = pAIXMNAVEq.Designator
						ILS.index = ILS.index | 2;
					}
				}
			}

			if (ILS.index == 2)
			{
				ILS.Identifier = ID;
				ILS.CallSign = CallSign;
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
					AixmNavaidEquipment = (NavaidEquipment)pNavaid.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

					if (AixmNavaidEquipment is VOR)
					{
						NavTypeCode = eNavaidType.CodeVOR;
						//NavTypeName = "VOR";
					}
					else if (AixmNavaidEquipment is DME)
					{
						NavTypeCode = eNavaidType.CodeDME;
						//NavTypeName = "DME";
					}
					else if (AixmNavaidEquipment is NDB)
					{
						NavTypeCode = eNavaidType.CodeNDB;
						//NavTypeName = "NDB";
					}
					else if (AixmNavaidEquipment is TACAN)
					{
						NavTypeCode = eNavaidType.CodeTACAN;
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

					if (NavTypeCode == eNavaidType.CodeDME)
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
						DMEList[iDMENum].Identifier = AixmNavaidEquipment.Identifier;
						DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

						DMEList[iDMENum].TypeCode = NavTypeCode;
						//DMEList[iDMENum].TypeName_Renamed = NavTypeName;
						DMEList[iDMENum].index = iNavaidNum + iDMENum;

						DMEList[iDMENum].pFeature = pNavaid;
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

						if (NavTypeCode == eNavaidType.CodeNDB)
							NavaidList[iNavaidNum].Range = 350000.0;		//NDB.Range
						else
							NavaidList[iNavaidNum].Range = 350000.0;		//VOR.Range

						NavaidList[iNavaidNum].PairNavaidIndex = -1;

						NavaidList[iNavaidNum].Name = AixmNavaidEquipment.Name;
						NavaidList[iNavaidNum].Identifier = AixmNavaidEquipment.Identifier;
						NavaidList[iNavaidNum].CallSign = AixmNavaidEquipment.Designator;

						NavaidList[iNavaidNum].TypeCode = NavTypeCode;
						NavaidList[iNavaidNum].index = iNavaidNum + iDMENum;

						NavaidList[iNavaidNum].pFeature = pNavaid;
					}
				}
			}

			for (j = 0; j <= iNavaidNum; j++)
				for (i = 0; i <= iDMENum; i++)
				{
					fDist = ARANFunctions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);
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
			//        pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev)
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
			//        NavaidList[iNavaidNum].Course = pPtGeo.M
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
			//        NavaidList[iNavaidNum].ID = AixmNavaidEquipment.Identifier
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

		public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, ADHPType CurrADHP, double Radius)
		{
			int iWPTNum, i, j, n;

			eNavaidType NavTypeCode;

			List<DesignatedPoint> AIXMWPTList;
			List<Navaid> pNavaidComList;

			Navaid pNavaidCom;
			NavaidEquipment AIXMNAVEq;
			DesignatedPoint AIXMWPT;

			Point pPtGeo;
			Point pPtPrj;

			Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, Radius);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon pARANPolygon = new MultiPolygon();
			pARANPolygon.Add(pPolygon);

			pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pARANPolygon);

			AIXMWPTList = pObjectDir.GetDesignatedPointList(pARANPolygon);
			pNavaidComList = pObjectDir.GetNavaidList(pARANPolygon);

			n = AIXMWPTList.Count + 2 * pNavaidComList.Count;
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
				WPTList[iWPTNum].Identifier = AIXMWPT.Identifier;

				WPTList[iWPTNum].pFeature = AIXMWPT;

				WPTList[iWPTNum].TypeCode = eNavaidType.CodeNONE;
			}

			//======================================================================

			for (j = 0; j < pNavaidComList.Count; j++)
			{
				pNavaidCom = pNavaidComList[j];
				for (i = 0; i < pNavaidCom.NavaidEquipment.Count; i++)
				{
					AIXMNAVEq = (NavaidEquipment)pNavaidCom.NavaidEquipment[i].TheNavaidEquipment.GetFeature();

					//    For I = 0 To AIXMNAVList.Count - 1
					//        Set AIXMNAVEq = AIXMNAVList.Item(I).Cast()

					//        Set pElevPoint = AIXMNAVEq.Location
					//        Set pGMLPoint = pElevPoint

					pPtGeo = AIXMNAVEq.Location.Geo;
					pPtGeo.Z = ConverterToSI.Convert(AIXMNAVEq.Location.Elevation, CurrADHP.Elev);

					pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
					if (pPtPrj.IsEmpty)
						continue;

					if (AIXMNAVEq is VOR)
						NavTypeCode = eNavaidType.CodeVOR;
					else if (AIXMNAVEq is NDB)
						NavTypeCode = eNavaidType.CodeNDB;
					else if (AIXMNAVEq is TACAN)
						NavTypeCode = eNavaidType.CodeTACAN;
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
					WPTList[iWPTNum].Identifier = AIXMNAVEq.Identifier;

					WPTList[iWPTNum].pFeature = pNavaidCom;
					WPTList[iWPTNum].TypeCode = NavTypeCode;
				}
			}
			//======================================================================
			iWPTNum++;
			System.Array.Resize<WPT_FIXType>(ref WPTList, iWPTNum);

			return iWPTNum;
		}

		public static int GetArObstaclesByPoly(ref ObstacleType[] ObstacleList, MultiPolygon pPoly, double fRefHeight)
		{
			int i, j, n;

			List<VerticalStructure> VerticalStructureList;
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;
			Point pPtGeo;
			Point pPtPrj;

			MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

			VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
			n = VerticalStructureList.Count;
			ObstacleList = new ObstacleType[0];

			if (n == 0)
				return -1;

			System.Array.Resize<ObstacleType>(ref ObstacleList, n);
			//Set pProxiOperator = CurrADHP.pPtPrj
			j = -1;

			for (i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];

				if (AixmObstacle.Part.Count == 0)
					continue;
				if (AixmObstacle.Part[0].HorizontalProjection == null)
					continue;

				pElevatedPoint = null;
				if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
					pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

				if (pElevatedPoint == null) continue;
				if (pElevatedPoint.Elevation == null) continue;

				pPtGeo = pElevatedPoint.Geo;
				pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);// +ConverterToSI.Convert(AixmObstacle.Part[0].verticalExtent, 0);

				pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
				if (pPtPrj.IsEmpty)
					continue;

				j++;
				ObstacleList[j].pPtGeo = pPtGeo;
				ObstacleList[j].pPtPrj = pPtPrj;
				ObstacleList[j].Name = AixmObstacle.Name;
				ObstacleList[j].Identifier = AixmObstacle.Identifier;
				ObstacleList[j].ID = AixmObstacle.Id;

				ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
				ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

				ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
				//ObstacleList[J].Dist = pProxiOperator.ReturnDistance(ObstacleList[J].pPtPrj)
				ObstacleList[j].index = i;
			}
			j++;
			System.Array.Resize<ObstacleType>(ref ObstacleList, j);
			return j;
		}

		//public static int GetHDObstacles(ref ObstacleHd[] ObstacleList, ADHPType CurrADHP, double MaxDist, double fRefHeight)
		//{
		//    ESRI.ArcGIS.Geometry.IPolygon pPoly;
		//    pPoly = (ESRI.ArcGIS.Geometry.IPolygon)Functions.CreatePrjCircle(CurrADHP.pPtPrj, MaxDist);

		//    return GetArObstaclesByPoly(ref ObstacleList, pPoly, fRefHeight);
		//}

		static public int GetHDObstacles(ref ObstacleType[] ObstacleList, ADHPType CurrADHP, double MaxDist, double fRefHeight)
		{
			int i, j, n;

			List<VerticalStructure> VerticalStructureList;
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;

			Point pPtGeo;
			Point pPtPrj;

			Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, MaxDist);

			Polygon pPolygon = new Polygon();
			pPolygon.ExteriorRing = ring;

			MultiPolygon TmpPolygon = new MultiPolygon();
			TmpPolygon.Add(pPolygon);

			MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

			VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
			n = VerticalStructureList.Count;

			ObstacleList = new ObstacleType[0];

			if (n == 0)
				return -1;

			System.Array.Resize<ObstacleType>(ref ObstacleList, n);

			j = -1;

			for (i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];

				if (AixmObstacle.Part.Count == 0) continue;
				if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

				pElevatedPoint = null;
				if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
					pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

				if (pElevatedPoint == null) continue;
				if (pElevatedPoint.Elevation == null) continue;

				pPtGeo = pElevatedPoint.Geo;
				pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);// +ConverterToSI.Convert(AixmObstacle.Part[0].verticalExtent, 0);

				pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
				if (pPtPrj.IsEmpty) continue;

				j++;
				ObstacleList[j].pPtGeo = pPtGeo;
				ObstacleList[j].pPtPrj = pPtPrj;
				ObstacleList[j].Name = AixmObstacle.Name;
				ObstacleList[j].Identifier = AixmObstacle.Identifier;
				ObstacleList[j].ID = AixmObstacle.Id;

				ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
				ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

				ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
				ObstacleList[j].X = ARANFunctions.ReturnDistanceInMeters(CurrADHP.pPtPrj, ObstacleList[j].pPtPrj);
				ObstacleList[j].index = i;
			}
			j++;
			System.Array.Resize<ObstacleType>(ref ObstacleList, j);
			return j;
		}

		public static int GetObstListInPoly(ref ObstacleType[] ObstList, MultiPolygon pPoly)
		{
			int i, j, n;

			List<VerticalStructure> VerticalStructureList;
			VerticalStructure AixmObstacle;
			ElevatedPoint pElevatedPoint;

			Point pPtGeo;
			Point pPtPrj;

			MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);

			VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
			n = VerticalStructureList.Count;

			ObstList = new ObstacleType[0];

			if (n == 0)
				return -1;

			System.Array.Resize<ObstacleType>(ref ObstList, n);
			j = -1;

			for (i = 0; i < n; i++)
			{
				AixmObstacle = VerticalStructureList[i];

				if (AixmObstacle.Part.Count == 0) continue;
				if (AixmObstacle.Part[0].HorizontalProjection == null) continue;

				pElevatedPoint = null;
				if (AixmObstacle.Part[0].HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
					pElevatedPoint = AixmObstacle.Part[0].HorizontalProjection.Location;

				if (pElevatedPoint == null) continue;
				if (pElevatedPoint.Elevation == null) continue;

				pPtGeo = pElevatedPoint.Geo;
				pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);// +ConverterToSI.Convert(AixmObstacle.Part[0].verticalExtent, 0);

				pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
				if (pPtPrj.IsEmpty) continue;

				j++;

				ObstList[j].pPtGeo = pPtGeo;
				ObstList[j].pPtPrj = pPtPrj;

				ObstList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
				ObstList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

				ObstList[j].Name = AixmObstacle.Name;
				ObstList[j].Identifier = AixmObstacle.Identifier;
				ObstList[j].ID = AixmObstacle.Id;
				ObstList[j].Height = ObstList[j].pPtGeo.Z;
				ObstList[j].index = i;
			}
			j++;
			System.Array.Resize<ObstacleType>(ref ObstList, j);
			return j;
		}
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
		Dim ConverterToSI As ConvertToSI
		Dim pARANPolygon As MultiPolygon

		ReDim ObstList(-1)

		pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(IAF_FullAreaPoly) as IPolygon)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1

		if N < 0  Return -1

		ConverterToSI = New ConvertToSI
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

			J = J + 1

			ObstList[J].pPtGeo = pPtGeo
			ObstList[J].pPtPrj = pPtPrj

			ObstList[J].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
			ObstList[J].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.Elevation.VerticalAccuracy, 0.0)

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

			ObstList[J].Name = AixmObstacle.Name
			ObstList[J].Identifier = AixmObstacle.Identifier
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
		Dim pARANPolygon As MultiPolygon
		Dim ConverterToSI As ConvertToSI

		ReDim ObstList(-1)
		ReDim ProhibitionSectors(-1)

		pARANPolygon = Converters.ESRIPolygonToARANPolygon(Functions.ToGeo(IAF_FullAreaPoly) as IPolygon)

		VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon)
		N = VerticalStructureList.Count - 1

		if N < 0  Return -1

		ConverterToSI = New ConvertToSI
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
				J = J + 1
				ObstList[J].pPtGeo = pPtGeo
				ObstList[J].pPtPrj = pPtPrj

				ObstList[J].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0)
				ObstList[J].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.Elevation.VerticalAccuracy, 0.0)

				ObstList[J].Dist = ObsDist
				ObstList[J].CLDist = ObsCLDist
				ObstList[J].Height = ObsHeight
				ObstList[J].Flags = -CShort(fDist = 0.0)

				ObstList[J].Name = AixmObstacle.Name
				ObstList[J].Identifier = AixmObstacle.Identifier
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

		Dim SAAList As List<SafeAltitudeArea)
		Dim SAASectorList As List<Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector)
		Dim SectorDefinition As Delib.Classes.Objects.Procedure.CircleSector

		Dim SafeAltitudeArea As SafeAltitudeArea
		Dim SafeAltitudeSector As Delib.Classes.Objects.Procedure.SafeAltitudeAreaSector
		Dim ConverterToSI As ConvertToSI

		SAAList = pObjectDir.GetSafeAltitudeAreaList(Nav.pSignificantPoint)
		N = SAAList.Count

		if N = 0 
			ReDim MSAList(-1)
			Return False
		End if

		ReDim MSAList(N - 1)
		ConverterToSI = New ConvertToSI
		J = -1

		Dim pSignificantPoint As SignificantPoint
		Dim pNAVEq As NavaidEquipment
		Dim pADHP As AirportHeliport
		Dim pDesign As DesignatedPoint
		For I = 0 To N - 1
			SafeAltitudeArea = SAAList.Item(I)

			if SafeAltitudeArea.safeAreaType.Value = CodeSafeAltitudeType.MSA 

				SAASectorList = SafeAltitudeArea.sector
				//Set SAASectorList = pObjectDir.GetSafeAltitudeAreaSectorList(SafeAltitudeArea.Identifier)
				M = SAASectorList.Count
			if m> 0 then
				J = J + 1
				MSAList(J).Name = SafeAltitudeArea.Identifier
				ReDim MSAList(J).Sectors(M - 1)

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

				For K = 0 To M - 1
					SafeAltitudeSector = SAASectorList.Item(K)
					SectorDefinition = SafeAltitudeSector.sectorDefinition

					FromAngle = SectorDefinition.fromAngle.Value
					ToAngle = SectorDefinition.toAngle.Value

					if SectorDefinition.angleDirectionReference.Value = CodeDirectionReferenceType.TO 
						FromAngle = FromAngle + 180.0
						ToAngle = ToAngle + 180.0
					End if

					if SectorDefinition.arcDirection.Value = CodeArcDirectionType.CCA 
						fTmp = FromAngle
						FromAngle = ToAngle
						ToAngle = fTmp
					End if

					if SectorDefinition.AngleType.Value <> CodeBearing.TRUE 

					End if

					MSAList(J).Sectors(K).LowerLimit = ConverterToSI.Convert(SectorDefinition.lowerLimit, 0.0)
					MSAList(J).Sectors(K).UpperLimit = ConverterToSI.Convert(SectorDefinition.upperLimit, 0.0)

					MSAList(J).Sectors(K).InnerDist = ConverterToSI.Convert(SectorDefinition.innerDistance, 0.0)
					MSAList(J).Sectors(K).OuterDist = ConverterToSI.Convert(SectorDefinition.outerDistance, 19000.0)
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

		public static DesignatedPoint CreateDesignatedPoint(Point pPtPrj, string Name = "COORD", double fDirInRad = -1000.0)
		{
			bool bExist;
			int i, n;
			Double fDist;
			Double fMinDist;
			Double fDirToPt;

			WPT_FIXType WptFIX = new WPT_FIXType();
			DesignatedPoint pFixDesignatedPoint;

			fMinDist = 10000.0;
			n = GlobalVars.WPTList.Length - 1;

			if (n >= 0)
				for (i = 0; i <= n; i++)
				{
					fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, GlobalVars.WPTList[i].pPtPrj);

					if (fDist < fMinDist)
					{
						fMinDist = fDist;
						WptFIX = GlobalVars.WPTList[i];
					}
				}


			bExist = fMinDist <= 10.0;

			if (!bExist && fMinDist <= 100.0 && fDirInRad != -1000.0)
			{
				fDirToPt = ARANFunctions.ReturnAngleInRadians(pPtPrj, WptFIX.pPtPrj);
				bExist = ARANMath.SubtractAngles(fDirInRad, fDirToPt) < ARANMath.EpsilonRadian;
			}

			if (bExist)
				return WptFIX.pFeature as DesignatedPoint;


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

		public static string InitModule()
		{
			Aran.Aim.Data.IDbProvider dbPro = (IDbProvider)GlobalVars.gAranEnv.DbProvider;
			if (!isOpen)
			{
				pObjectDir = PandaSQPIFactory.Create();
				Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;
				pObjectDir.Open(dbPro);

				isOpen = true;
			}
			return dbPro.CurrentUser.Name;
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
