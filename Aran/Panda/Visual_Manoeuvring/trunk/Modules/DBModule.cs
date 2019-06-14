using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran;
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

namespace Aran.Panda.VisualManoeuvring
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

            GeometryOperators geomOperators = new GeometryOperators();

            //GlobalVars.gAranGraphics.DrawMultiPolygon(GlobalVars.p_LicenseRect, -1, AranEnvironment.Symbols.eFillStyle.sfsCross);

            //if (!geomOperators.Contains(GlobalVars.p_LicenseRect, pPtPrj))
            //return -1;

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

                //if (pRwyDRList.Count == 2)
                //{
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
                    RWYList[iRwyNum].pRunwayDir = pRwyDirection;

                    List<RunwayDeclaredDistance> pDeclaredDistList = null;
                    //double PtEndZ = -9999.0;

                    pCenterLinePointList = pObjectDir.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
                    for (int k = 0; k < pCenterLinePointList.Count; k++)
                    {
                        pRunwayCenterLinePoint = pCenterLinePointList[k];
                        pElevatedPoint = pRunwayCenterLinePoint.Location;

                        if (pRunwayCenterLinePoint.Role != null)
                        {
                            switch (pRunwayCenterLinePoint.Role.Value)		//Select Case pRunwayCenterLinePoint.Role.Value
                            {
                                case CodeRunwayPointRole.START:
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptStart] = pElevatedPoint.Geo;
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptStart].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
                                    //RWYList[iRwyNum].pSignificantPoint[eRWY.ptStart] = pRunwayCenterLinePoint;
                                    pDeclaredDistList = pRunwayCenterLinePoint.AssociatedDeclaredDistance;

                                    break;
                                case CodeRunwayPointRole.THR:
                                case CodeRunwayPointRole.DISTHR:
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptTHR] = pElevatedPoint.Geo;
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptTHR].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
                                    //RWYList[iRwyNum].pSignificantPoint[eRWY.ptTHR] = pRunwayCenterLinePoint;
                                    break;
                                case CodeRunwayPointRole.END:
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptEnd] = pElevatedPoint.Geo;
                                    RWYList[iRwyNum].pPtGeo[eRWY.ptEnd].Z = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
                                    //PtEndZ = ConverterToSI.Convert(pElevatedPoint.Elevation, Owner.Elev);
                                    //RWYList[iRwyNum].pSignificantPoint[eRWY.ptEnd] = pRunwayCenterLinePoint;
                                    break;
                            }
                        }
                    }

                    //for (eRWY ek = eRWY.ptStart; ek <= eRWY.ptDER; ek++)
                    //{
                    //	if (RWYList[iRwyNum].pPtGeo[ek] == null)
                    //	{
                    //		//if (ek == eRWY.ptTHR)									continue;
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


                    #region "Attention to this part!!!"

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
                                //else if (pDirDeclaredDist.Type == CodeDeclaredDistance.TODA)
                                //    fTODA = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fTODA);
                                //else if (pDirDeclaredDist.Type == CodeDeclaredDistance.DTHR)
                                //    fDTHR = ConverterToSI.Convert(pDirDeclaredDist.DeclaredValue[0].Distance, fDTHR);
                            }
                        }
                    }

                    //List<RunwayDeclaredDistance> GetDeclaredDistance(Guid RCLPIdentifier);
                    //DeclaredDistance pDirDeclaredDist;
                    //pDeclaredDistList  = pObjectDir.g GetDeclaredDistanceList(pRunway.Identifier);

                    //if (fTODA < 0)
                    //{
                    //    System.Windows.Forms.MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
                    //    iRwyNum--;
                    //    continue;
                    //    //goto NextI;
                    //}
                    //RWYList[iRwyNum].TODA = fTODA;
                    RWYList[iRwyNum].TODA = fTORA;

                    #endregion

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

                    for (eRWY ek = eRWY.ptStart; ek <= eRWY.ptEnd; ek++)
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

        public static int AddILSToNavList(ILSType ILS, ref NavaidType[] NavaidList)
        {
            int n = NavaidList.GetLength(0);

            for (int i = 0; i < n; i++)
                if ((NavaidList[i].TypeCode == eNavaidType.LLZ) && (NavaidList[i].CallSign == ILS.CallSign))
                    return i;

            System.Array.Resize<NavaidType>(ref NavaidList, n + 1);

            NavaidList[n].pPtGeo = ILS.pPtGeo;
            NavaidList[n].pPtPrj = ILS.pPtPrj;
            NavaidList[n].Name = ILS.CallSign;
            NavaidList[n].Identifier = ILS.Identifier;
            NavaidList[n].CallSign = ILS.CallSign;

            NavaidList[n].MagVar = ILS.MagVar;
            NavaidList[n].TypeCode = eNavaidType.LLZ;
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
            Guid ID = Guid.Empty;

            ILS.index = 0;

            //if RWY.ILSID = ""  Exit Function

            Navaid pNavaidCom = pObjectDir.GetILSNavaid(RWY.pRunwayDir.Identifier);
            if (pNavaidCom == null)
                return 0;

            List<NavaidComponent> pAIXMNAVEqList = pNavaidCom.NavaidEquipment;
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

            string CallSign = "";
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

                if (pAIXMLocalizer.TrueBearing != null)
                    ILS.Course = pAIXMLocalizer.TrueBearing.Value;
                else if (pAIXMLocalizer.MagneticBearing != null)
                    ILS.Course = pAIXMLocalizer.MagneticBearing.Value - ILS.MagVar;
                else
                    goto NoLocalizer;

                //ILS.Course = Modulus(ILS.Course + 180.0, 360.0)

                ILS.pPtGeo = pElevPoint.Geo;
                ILS.pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, RWY.pPtGeo[eRWY.ptTHR].Z);
                ILS.pPtGeo.M = ARANMath.DegToRad(ILS.Course);

                ILS.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(ILS.pPtGeo);

                if (ILS.pPtPrj.IsEmpty)
                    return 0;

                ILS.pPtPrj.M = GlobalVars.pspatialReferenceOperation.AztToDirGeo(ILS.pPtGeo, ARANMath.RadToDeg(ILS.pPtGeo.M));

                double dX = RWY.pPtPrj[eRWY.ptTHR].X - ILS.pPtPrj.X;
                double dY = RWY.pPtPrj[eRWY.ptTHR].Y - ILS.pPtPrj.Y;
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
                        DMEList[iDMENum].Identifier = AixmNavaidEquipment.Identifier;
                        DMEList[iDMENum].CallSign = AixmNavaidEquipment.Designator;

                        DMEList[iDMENum].TypeCode = NavTypeCode;

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

                        if (NavTypeCode == eNavaidType.NDB)
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
            //'        dX = RWY.pPtPrj(eRWY.ptTHR).X - pPtPrj.X
            //'        dY = RWY.pPtPrj(eRWY.ptTHR).Y - pPtPrj.Y
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

        public static int FillWPT_FIXList(out WPT_FIXType[] WPTList, ADHPType CurrADHP, double radius)
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

            Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, radius);

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

                if (WPTList[iWPTNum].CallSign == null)
                    WPTList[iWPTNum].CallSign = "";
                if (WPTList[iWPTNum].Name == null)
                    WPTList[iWPTNum].Name = "";

                WPTList[iWPTNum].pFeature = AIXMWPT;

                WPTList[iWPTNum].TypeCode = eNavaidType.NONE;
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

        public static int GetVerticalStructuresByPoly(out List<VerticalStructure> VerticalStructureList, MultiPolygon pPoly)
        {
            MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(pPoly);
            VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
            return VerticalStructureList.Count;
        }

        public static int GetVerticalStructuresByDist(out List<VerticalStructure> VerticalStructureList, Point ptCenter, double MaxDist)
        {
            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            MultiPolygon TmpPolygon = new MultiPolygon();
            TmpPolygon.Add(pPolygon);

            MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

            VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);
            return VerticalStructureList.Count;
        }

        public static int GetObstaclesByPoly(out ObstacleType[] ObstacleList, MultiPolygon pPoly)
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
                pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

                pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
                if (pPtPrj.IsEmpty) continue;

                j++;

                ObstacleList[j].pPtGeo = pPtGeo;
                ObstacleList[j].pPtPrj = pPtPrj;

                ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
                ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

                ObstacleList[j].Name = AixmObstacle.Name;
                ObstacleList[j].Identifier = AixmObstacle.Identifier;
                ObstacleList[j].ID = AixmObstacle.Id.ToString();
                ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z;
                ObstacleList[j].Index = i;
            }
            j++;
            System.Array.Resize<ObstacleType>(ref ObstacleList, j);
            return j;
        }

        public static int GetObstaclesByPoly(out ObstacleType[] ObstacleList, MultiPolygon pPoly, double fRefHeight)
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
                pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

                pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
                if (pPtPrj.IsEmpty)
                    continue;

                j++;
                ObstacleList[j].pPtGeo = pPtGeo;
                ObstacleList[j].pPtPrj = pPtPrj;
                ObstacleList[j].Name = AixmObstacle.Name;
                ObstacleList[j].Identifier = AixmObstacle.Identifier;
                ObstacleList[j].ID = AixmObstacle.Id.ToString();

                ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
                ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

                ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
                //ObstacleList[j].X = 0.0;
                ObstacleList[j].Index = i;
            }
            j++;
            System.Array.Resize<ObstacleType>(ref ObstacleList, j);
            return j;
        }

        /*
        static public int GetObstaclesByDist(out ObstacleType[] ObstacleList, Point ptCenter, double MaxDist)
        {
            int i, j, n;

            List<VerticalStructure> VerticalStructureList;
            VerticalStructure AixmObstacle;
            ElevatedPoint pElevatedPoint;

            Point pPtGeo;
            Point pPtPrj;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            MultiPolygon TmpPolygon = new MultiPolygon();
            TmpPolygon.Add(pPolygon);
            //GlobalVars.gAranGraphics.DrawMultiPolygon(TmpPolygon, 125, AranEnvironment.Symbols.eFillStyle.sfsCross);

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
                pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

                pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
                if (pPtPrj.IsEmpty) continue;

                j++;
                ObstacleList[j] = new ObstacleType();
                ObstacleList[j].pPtGeo = pPtGeo;
                ObstacleList[j].pPtPrj = pPtPrj;
                ObstacleList[j].Name = AixmObstacle.Name;
                ObstacleList[j].Identifier = AixmObstacle.Identifier;
                ObstacleList[j].ID = AixmObstacle.Id.ToString();

                ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
                ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

                ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z;
                //ObstacleList[j].X = ARANFunctions.ReturnDistanceInMeters(ptCenter, ObstacleList[j].pPtPrj);
                ObstacleList[j].Index = i;
            }
            j++;
            System.Array.Resize<ObstacleType>(ref ObstacleList, j);
            return j;
        }

        static public int GetObstaclesByDist(out ObstacleType[] ObstacleList, Point ptCenter, double MaxDist, double fRefHeight)
        {
            int i, j, n;

            List<VerticalStructure> VerticalStructureList;
            VerticalStructure AixmObstacle;
            ElevatedPoint pElevatedPoint;

            Point pPtGeo;
            Point pPtPrj;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

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
                pPtGeo.Z = ConverterToSI.Convert(pElevatedPoint.Elevation, -9999.0);

                pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(pPtGeo);
                if (pPtPrj.IsEmpty) continue;

                j++;
                ObstacleList[j].pPtGeo = pPtGeo;
                ObstacleList[j].pPtPrj = pPtPrj;
                ObstacleList[j].Name = AixmObstacle.Name;
                ObstacleList[j].Identifier = AixmObstacle.Identifier;
                ObstacleList[j].ID = AixmObstacle.Id.ToString();

                ObstacleList[j].HorAccuracy = ConverterToSI.Convert(pElevatedPoint.HorizontalAccuracy, 0.0);
                ObstacleList[j].VertAccuracy = ConverterToSI.Convert(pElevatedPoint.VerticalAccuracy, 0.0);

                ObstacleList[j].Height = ObstacleList[j].pPtGeo.Z - fRefHeight;
                //ObstacleList[j].X = ARANFunctions.ReturnDistanceInMeters(ptCenter, ObstacleList[j].pPtPrj);
                ObstacleList[j].Index = i;
            }
            j++;
            System.Array.Resize<ObstacleType>(ref ObstacleList, j);
            return j;
        }*/
        
        /*public static double GetObstListInPoly(out ObstacleContainer ObstList, IPolygon pPoly, IPoint ptCenter, double fRefHeight = 0.0)
        {
            VerticalStructure AixmObstacle;
            ElevatedPoint pElevatedPoint;
            ElevatedCurve pElevatedCurve;
            ElevatedSurface pElevatedSurface;

            MultiPolygon pARANPolygon = Converters.ESRIPolygonToARANPolygon((IPolygon)Functions.ToGeo(pPoly));
            List<VerticalStructure> VerticalStructureList = pObjectDir.GetVerticalStructureList(pARANPolygon);

            ESRI.ArcGIS.Geometry.IZ pZv;
            ESRI.ArcGIS.Geometry.IZAware pZAware;
            ESRI.ArcGIS.Geometry.ITopologicalOperator pTopop;
            ESRI.ArcGIS.Geometry.ITopologicalOperator2 pTopo;
            ESRI.ArcGIS.Geometry.IProximityOperator pProxy = (ESRI.ArcGIS.Geometry.IProximityOperator)ptCenter;

            int n = VerticalStructureList.Count;
            ObstList.Parts = new ObstacleData[0];
            ObstList.Obstacles = new Obstacle[n];

            if (n == 0)
                return 0.0;

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
                    //try
                    //{
                    //if (AixmObstacle.Name == "RIX_N_1")
                    //{
                    //    Z = 100.0;
                    //}

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
                            pElevatedSurface = ObstaclePart.HorizontalProjection.SurfaceExtent;
                            if (pElevatedSurface == null) continue;
                            if (pElevatedSurface.Elevation == null) continue;

                            HorAccuracy = ConverterToSI.Convert(pElevatedSurface.HorizontalAccuracy, 0.0);
                            VertAccuracy = ConverterToSI.Convert(pElevatedSurface.VerticalAccuracy, 0.0);

                            pGeomGeo = ConvertToEsriGeom.FromMultiPolygon(pElevatedSurface.Geo);
                            Z = ConverterToSI.Convert(pElevatedSurface.Elevation, -9999.0);
                            break;
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
                    ObstList.Obstacles[k].Name = AixmObstacle.Name;
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
        }*/

        public static int GetObstaclesByDist(out List<VerticalStructure> ObstacleList, Point ptCenter, double MaxDist)
        {
            int n;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            MultiPolygon TmpPolygon = new MultiPolygon();
            TmpPolygon.Add(pPolygon);

            MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

            ObstacleList = pObjectDir.GetVerticalStructureList(pARANPolygon);
            
            n = ObstacleList.Count;

            return n;
        }

        public static int GetDesignatedPointByDist(out List<DesignatedPoint> DesignatedPointList, Point ptCenter, double MaxDist)
        {
            int n;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            MultiPolygon TmpPolygon = new MultiPolygon();
            TmpPolygon.Add(pPolygon);

            MultiPolygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<MultiPolygon>(TmpPolygon);

            DesignatedPointList = pObjectDir.GetDesignatedPointList(pARANPolygon);

            n = DesignatedPointList.Count;

            return n;
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

        public static void InitModule()	//As String
        {
            if (!isOpen)
            {
                pObjectDir = PandaSQPIFactory.Create();
                Aran.Queries.ExtensionFeature.CommonQPI = pObjectDir;
                pObjectDir.Open((DbProvider)GlobalVars.gAranEnv.DbProvider);

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



        public static int GetVisualFeaturesByDist_old(out VM_VisualFeature[] visualFeatures, Point ptCenter, double MaxDist)
        {
            int n;
            List<AeronauticalGroundLight> AllVisualFeaturesList;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            Polygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPolygon);
            Within within = new Within();
            within.Geometry = pARANPolygon;
            within.PropertyName = "location.geo";
            OperationChoice operChoice = new OperationChoice(within);
            Filter filter = new Filter(operChoice);

            AllVisualFeaturesList = (List<AeronauticalGroundLight>)pObjectDir.GetFeatureList(Aim.FeatureType.AeronauticalGroundLight, filter);
            n = AllVisualFeaturesList.Count;

            visualFeatures = new VM_VisualFeature[0];

            if (n == 0)
                return -1;

            System.Array.Resize<VM_VisualFeature>(ref visualFeatures, n);

            for (int i = 0; i < n; i++)
            {
                visualFeatures[i] = new VM_VisualFeature();
                visualFeatures[i].gShape = AllVisualFeaturesList[i].Location.Geo;
                visualFeatures[i].pShape = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(AllVisualFeaturesList[i].Location.Geo);
                visualFeatures[i].Name = AllVisualFeaturesList[i].Name;
                visualFeatures[i].Type = AllVisualFeaturesList[i].Type.ToString();
                visualFeatures[i].Description = AllVisualFeaturesList[i].Annotation[0].TranslatedNote[0].Note.Value;
            }

            return n;
        }

        public static int GetVisualFeaturesByDist(out VM_VisualFeature[] visualFeatures, Point ptCenter, double MaxDist)
        {
            int n;
            List<DesignatedPoint> AllVisualFeaturesList;

            Ring ring = ARANFunctions.CreateCirclePrj(ptCenter, MaxDist);

            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            Polygon pARANPolygon = GlobalVars.pspatialReferenceOperation.ToGeo<Polygon>(pPolygon);

            Within within = new Within();
            within.Geometry = pARANPolygon;
            within.PropertyName = "location.geo";
            OperationChoice operChoiceWithin = new OperationChoice(within);

            ComparisonOps compOperType = new ComparisonOps(ComparisonOpType.EqualTo, "Type", CodeDesignatedPoint.DESIGNED); //Change this to OTHER-VF
            OperationChoice operChoiceType = new OperationChoice(compOperType);


            BinaryLogicOp binOper = new BinaryLogicOp();
            binOper.Type = BinaryLogicOpType.And;
            binOper.OperationList.Add(operChoiceWithin);
            binOper.OperationList.Add(operChoiceType);

            OperationChoice operChoiceAll = new OperationChoice(binOper);
            Filter filter = new Filter(operChoiceAll);

            AllVisualFeaturesList = (List<DesignatedPoint>)pObjectDir.GetFeatureList(Aim.FeatureType.DesignatedPoint, filter);

            n = AllVisualFeaturesList.Count;

            visualFeatures = new VM_VisualFeature[0];

            if (n == 0)
                return -1;

            System.Array.Resize<VM_VisualFeature>(ref visualFeatures, n);

            for (int i = 0; i < n; i++)
            {
                visualFeatures[i] = new VM_VisualFeature();
                visualFeatures[i].gShape = AllVisualFeaturesList[i].Location.Geo;
                visualFeatures[i].pShape = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(AllVisualFeaturesList[i].Location.Geo);
                visualFeatures[i].Name = AllVisualFeaturesList[i].Name;
                visualFeatures[i].Type = AllVisualFeaturesList[i].Type.ToString();
                if (AllVisualFeaturesList[i].Annotation.Count > 0)
                    visualFeatures[i].Description = AllVisualFeaturesList[i].Annotation[0].TranslatedNote[0].Note.Value;
            }

            return n;
        }

        public static void InsertVisualFeatures()
        {
            try
            {
                FeatureType[] featureTypes = { FeatureType.DesignatedPoint };
                bool result = pObjectDir.Commit(featureTypes);
                if (!result)
                    MessageBox.Show("Failed to add the new point.");
                else
                    MessageBox.Show("The new point is added. Please refresh the layer");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void CreateVisualFeature(string name, string description, Point geoPnt)
        {
            pObjectDir.SetRootFeatureType(Aim.FeatureType.DesignatedPoint);
            DesignatedPoint visualFeature = pObjectDir.CreateFeature<DesignatedPoint>();
            visualFeature.Location = new AixmPoint();
            visualFeature.Location.Geo.X = geoPnt.X;
            visualFeature.Location.Geo.Y = geoPnt.Y;
            visualFeature.Type = Aim.Enums.CodeDesignatedPoint.DESIGNED;
            visualFeature.Name = name;

            Note note = new Note();
            LinguisticNote ln = new LinguisticNote();
            ln.Note = new Aran.Aim.DataTypes.TextNote();
            ln.Note.Lang = Aim.Enums.language.ENG;
            ln.Note.Value = description;

            note.TranslatedNote.Add(ln);
            visualFeature.Annotation.Add(note);
        }


        public static void GetProcedureTransitionLegByIAP(Guid iapIdentifier)
        {
            int n;
            List<ProcedureTransitionLeg> ProcedureTransitionLegList;


        }


        /*public static void InsertVMTrack(List<TrackStep> trackSteps, Guid associatedProcedureID)
        {
            VM_TrackStep tempSegmentLeg;
            VM_Track resultTrack = new VM_Track(associatedProcedureID);
            for (int i = 0; i < trackSteps.Count; i++)
            {
                tempSegmentLeg = new VM_TrackStep();
                tempSegmentLeg.Trajectory.Add(trackSteps[i].stepCentreline);
                tempSegmentLeg.Trajectory.
                resultTrack.trackSteps.Add(tempSegmentLeg);
            }
        }*/
    }
}
