using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Converters;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Common;
using Aran.Queries;
using Aran.Queries.Panda_2;
using eNavaidType = Aran.PANDA.Common.eNavaidType;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;
using NavaidType = Aran.PANDA.Common.NavaidType;
using RunwayDirection = Aran.Aim.Features.RunwayDirection;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class DataContext
    {

        public string Username { get; private set; }
        public Guid CurrentAirportId { get; }
        public DateTime EffectiveDate { get; }
        public double Radius { get; }
        public double ModellingRadius { get; private set; } = 150000;

        public bool IsOpen { get; private set; }

        private AppEnvironment _environment;
        private IPandaSpecializedQPI _pandaQPI;


        private ADHPType _currADHP;
        private RWYType[] _rWYList;
        public ObstacleContainer ObstacleList;
        private NavaidType[] _navaidList;
        private NavaidType[] _dMEList;
        private WPT_FIXType[] _wPTList;


        public ADHPType CurrADHP => _currADHP;
        public RWYType[] RWYList => _rWYList;

        public NavaidType[] NavaidList => _navaidList;
        public NavaidType[] DMEList => _dMEList;
        public WPT_FIXType[] WPTList => _wPTList;

        protected internal UnitConverter UnitConverter => UnitContext.UnitConverter;
        protected internal UnitContext UnitContext => Env.Current.UnitContext;

        protected internal PreFinalState PreFinalState => Env.Current.RNPContext.PreFinalPhase.CurrentState;
        protected internal FinalState FinalState => Env.Current.RNPContext.FinalPhase.CurrentState;
        protected internal MissedApproachState MissedApproachState => Env.Current.RNPContext.MissedApproachPhase.CurrentState;
        protected internal IntermediateState IntermediateState => Env.Current.RNPContext.IntermediatePhase.CurrentState;

        public DataContext(AppEnvironment environment)
        {
            _environment = environment;
            EffectiveDate = environment.AranEnv.CurrentAiracDateTime.Value;
            Radius = ModellingRadius = environment.Settings.Radius;
            CurrentAirportId = environment.Settings.Aeroport;
        }

        public void Init()
        {
            if (!IsOpen)
            {
                DbProvider dbPro = (DbProvider)_environment.AranEnv.DbProvider;

                _pandaQPI = PandaSQPIFactory.Create();
                ExtensionFeature.CommonQPI = _pandaQPI;
                _pandaQPI.Open(dbPro);

                var terrainDataReaderHandler = _environment.AranEnv.CommonData.GetObject("terrainDataReader") as TerrainDataReaderEventHandler;
                if (terrainDataReaderHandler != null)
                    _pandaQPI.TerrainDataReader += terrainDataReaderHandler;

                Username = dbPro.CurrentUser.Name;

                IsOpen = true;
            }
        }

        public void Load()
        {
            FillADHPFields(ref _currADHP);
            FillRWYList(out _rWYList, _currADHP);
            FillNavaidList(out _navaidList, out _dMEList, _currADHP, Radius);
            FillWPTList(out _wPTList, _currADHP, Radius);
        }

        private void FillADHPFields(ref ADHPType CurrADHP)
        {
            AirportHeliport pADHP = _pandaQPI.GetFeature(FeatureType.AirportHeliport, CurrentAirportId) as AirportHeliport;
            CurrADHP.pAirportHeliport = pADHP;
            if (pADHP == null)
                throw new Exception("AirportHeliport is not found");


            CurrADHP.Identifier = pADHP.Identifier;
            CurrADHP.Name = pADHP.Name;
            Point pPtGeo = pADHP.ARP.Geo;
            pPtGeo.Z = ConverterToSI.Convert(pADHP.ARP.Elevation, 0);

            Point pPtPrj = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToPrj(pPtGeo);

            if (pPtPrj.IsEmpty)
                throw new Exception("AirportHeliport geo is empty");


            GeometryOperators geomOperators = new GeometryOperators();

            CurrADHP.pPtGeo = pPtGeo;
            CurrADHP.pPtPrj = pPtPrj;
            CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

            if (pADHP.MagneticVariation == null)
                CurrADHP.MagVar = 0.0;
            else
                CurrADHP.MagVar = pADHP.MagneticVariation.Value;


            CurrADHP.Elev = pPtGeo.Z;
            CurrADHP.AltimeterSource = pADHP.AltimeterSource;

            CurrADHP.ISAtC = ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);

            CurrADHP.TransitionLevel = ConverterToSI.Convert(pADHP.TransitionLevel, 2500.0);
            CurrADHP.WindSpeed = 56.0;
        }

        private int FillRWYList(out RWYType[] RWYList, ADHPType Owner)
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

            pAIXMRWYList = _pandaQPI.GetRunwayList(Owner.pAirportHeliport.Identifier); //, RunwayFields_Designator + RunwayFields_Id + RunwayFields_Length + RunwayFields_Profile + RunwayFields_Type

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
                pRunway = (Runway)_pandaQPI.GetFeature(FeatureType.Runway, pName.Identifier);

                pRwyDRList = _pandaQPI.GetRunwayDirectionList(pRunway.Identifier);

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

                    pCenterLinePointList = _pandaQPI.GetRunwayCentrelinePointList(pRwyDirection.Identifier);
                    for (int k = 0; k < pCenterLinePointList.Count; k++)
                    {
                        pRunwayCenterLinePoint = pCenterLinePointList[k];
                        pElevatedPoint = pRunwayCenterLinePoint.Location;

                        if (pElevatedPoint == null)
                            continue;

                        if (pRunwayCenterLinePoint.Role != null)
                        {
                            switch (pRunwayCenterLinePoint.Role.Value)      //Select Case pRunwayCenterLinePoint.Role.Value
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
                        MessageBox.Show(pRwyDirection.Designator + " START point: TODA not defined.");
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
                        RWYList[iRwyNum].pPtPrj[ek] = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToPrj(RWYList[iRwyNum].pPtGeo[ek]);
                        if (RWYList[iRwyNum].pPtPrj[ek].IsEmpty)
                        {
                            iRwyNum--;
                            goto NextI;
                        }

                        RWYList[iRwyNum].pPtGeo[ek].M = RWYList[iRwyNum].TrueBearing;
                        RWYList[iRwyNum].pPtPrj[ek].M = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.AztToDirGeo(RWYList[iRwyNum].pPtGeo[ek], RWYList[iRwyNum].TrueBearing);
                    }

                    RWYList[iRwyNum].Identifier = pRwyDirection.Identifier;
                    RWYList[iRwyNum].Name = pRwyDirection.Designator;
                    RWYList[iRwyNum].ADHP_ID = Owner.Identifier;

                    pRwyDirectinPair = pRwyDRList[(j + 1) % 2];
                    RWYList[iRwyNum].PairID = pRwyDirectinPair.Identifier;
                    RWYList[iRwyNum].PairName = pRwyDirectinPair.Designator;
                NextI:;
                }
            }

            Array.Resize(ref RWYList, iRwyNum + 1);
            return iRwyNum + 1;
        }

        public void FillNavaidList(out NavaidType[] NavaidList, out NavaidType[] DMEList, ADHPType CurrADHP, double Radius)
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
            pARANPolygon = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToGeo(pARANPolygon);

            pNavaidList = _pandaQPI.GetNavaidList(pARANPolygon);

            if (pNavaidList.Count == 0)     //And (ILSDataList.Count = 0)
            {
                NavaidList = new NavaidType[0];
                DMEList = new NavaidType[0];
                return;
            }

            NavaidList = new NavaidType[pNavaidList.Count];     //+ ILSDataList.Count
            DMEList = new NavaidType[pNavaidList.Count];        //+ ILSDataList.Count

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
                        NavTypeCode = eNavaidType.VOR;                  //NavTypeName = "VOR";
                    else if (AixmNavaidEquipment is DME)
                        NavTypeCode = eNavaidType.DME;                  //NavTypeName = "DME";
                    else if (AixmNavaidEquipment is NDB)
                        NavTypeCode = eNavaidType.NDB;                  //NavTypeName = "NDB";
                    else if (AixmNavaidEquipment is TACAN)
                        NavTypeCode = eNavaidType.TACAN;                //NavTypeName = "Tacan";
                    else
                        continue;

                    pElevPoint = AixmNavaidEquipment.Location;

                    pPtGeo = pElevPoint.Geo;
                    pPtGeo.Z = ConverterToSI.Convert(pElevPoint.Elevation, CurrADHP.Elev);
                    pPtPrj = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToPrj(pPtGeo);

                    if (pPtPrj.IsEmpty)
                        continue;

                    if (NavTypeCode == eNavaidType.DME)
                    {
                        iDMENum++;

                        DMEList[iDMENum].pPtGeo = pPtGeo;
                        DMEList[iDMENum].pPtPrj = pPtPrj;

                        if (AixmNavaidEquipment.MagneticVariation != null)
                            DMEList[iDMENum].MagVar = AixmNavaidEquipment.MagneticVariation.Value;
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
                            NavaidList[iNavaidNum].MagVar = AixmNavaidEquipment.MagneticVariation.Value;
                        else
                            NavaidList[iNavaidNum].MagVar = CurrADHP.MagVar;

                        if (NavTypeCode == eNavaidType.NDB)
                            NavaidList[iNavaidNum].Range = 350000.0;        //NDB.Range
                        else
                            NavaidList[iNavaidNum].Range = 350000.0;        //VOR.Range

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
                        NavaidList[j].PairNavaidIndex = i;
                        DMEList[i].PairNavaidIndex = j;
                        break;
                    }
                }

            Array.Resize(ref NavaidList, iNavaidNum + 1);
            Array.Resize(ref DMEList, iDMENum + 1);
        }

        public int FillWPTList(out WPT_FIXType[] WPTList, ADHPType CurrADHP, double radius)
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

            pARANPolygon = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToGeo(pARANPolygon);

            AIXMWPTList = _pandaQPI.GetDesignatedPointList(pARANPolygon);
            pNavaidList = _pandaQPI.GetNavaidList(pARANPolygon);

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
                pPtPrj = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToPrj(pPtGeo);

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

                    pPtPrj = AppEnvironment.Current.SpatialContext.SpatialReferenceOperation.ToPrj(pPtGeo);
                    if (pPtPrj.IsEmpty)
                        continue;

                    iWPTNum++;

                    WPTList[iWPTNum].pPtGeo = pPtGeo;
                    WPTList[iWPTNum].pPtPrj = pPtPrj;

                    if (AIXMNAVEq.MagneticVariation != null)
                        WPTList[iWPTNum].MagVar = AIXMNAVEq.MagneticVariation.Value;
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
            Array.Resize(ref WPTList, iWPTNum);

            Array.Sort(WPTList,
                (delegate (WPT_FIXType a, WPT_FIXType b)
                {
                    return a.ToString().CompareTo(b.ToString());
                }));

            return iWPTNum;
        }


        public int GetObstaclesByDist(double fRefHeight = 0.0)
        {

            VerticalStructure AixmObstacle;
            ElevatedPoint pElevatedPoint;
            ElevatedCurve pElevatedCurve;
            ElevatedSurface pElevatedSurface;

            GeometryOperators pTopooper = new GeometryOperators();

            Ring ring = ARANFunctions.CreateCirclePrj(CurrADHP.pPtPrj, ModellingRadius);
            Polygon pPolygon = new Polygon();
            pPolygon.ExteriorRing = ring;

            MultiPolygon TmpPolygon = new MultiPolygon();
            TmpPolygon.Add(pPolygon);

            MultiPolygon pARANPolygon = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(TmpPolygon);
            //==========================================================================================================================
            List<VerticalStructure> VerticalStructureList = Env.Current.Settings.AnnexObstalce?_pandaQPI.GetAnnexVerticalStructureList(CurrentAirportId):_pandaQPI.GetVerticalStructureList(pARANPolygon);
            
            int n = VerticalStructureList.Count;

            ObstacleList.Parts = new ObstacleData[0];
            ObstacleList.Obstacles = new Obstacle[n];

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

                    pGeomPrj = Env.Current.SpatialContext.SpatialReferenceOperation.ToPrj(pGeomGeo);
                    if (pGeomPrj.IsEmpty) continue;

                    if (VertAccuracy > 0.0)
                        Z += VertAccuracy;

                    if (false) //(HorAccuracy > 0.0)//HorAccuracy = 0.0;
                    {
                        if (pGeomPrj.Type == GeometryType.Point && (HorAccuracy <= 2.0))
                            pGeomPrj = ARANFunctions.CreateCirclePrj((Point)pGeomPrj, HorAccuracy); //, 18
                        pGeomPrj = pTopooper.Buffer(pGeomPrj, HorAccuracy);

                        pGeomGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(pGeomPrj);
                    }

                    if (pGeomGeo.Type == GeometryType.Point)
                    {
                        (pGeomGeo as Point).Z = Z;
                        (pGeomPrj as Point).Z = Z;
                    }

                    k++;

                    if (k >= C)
                    {
                        C += n;
                        Array.Resize(ref ObstacleList.Obstacles, C);
                    }

                    ObstacleList.Obstacles[k].pGeomGeo = pGeomGeo;
                    ObstacleList.Obstacles[k].pGeomPrj = pGeomPrj;
                    ObstacleList.Obstacles[k].UnicalName = AixmObstacle.Name;
                    if (AixmObstacle.Type == null)
                        ObstacleList.Obstacles[k].TypeName = "";
                    else
                        ObstacleList.Obstacles[k].TypeName = AixmObstacle.Type.ToString();

                    ObstacleList.Obstacles[k].Identifier = AixmObstacle.Identifier;
                    ObstacleList.Obstacles[k].ID = AixmObstacle.Id;

                    ObstacleList.Obstacles[k].HorAccuracy = HorAccuracy;
                    ObstacleList.Obstacles[k].VertAccuracy = VertAccuracy;

                    ObstacleList.Obstacles[k].Height = Z - fRefHeight;
                    ObstacleList.Obstacles[k].Index = k;

                    //double fDist = pTopooper.GetDistance(pGeomPrj);
                    //if (fDist > MaxDist)
                    //    MaxDist = fDist;
                }
            }

            k++;
            Array.Resize(ref ObstacleList.Obstacles, k);

            return k;
        }

        public bool SaveProcedure()
        {
            ProcedureTransition pTransition;
            LandingTakeoffAreaCollection pLandingTakeoffAreaCollection;
            AircraftCharacteristic IsLimitedTo;
            FeatureRef featureRef;
            FeatureRefObject featureRefObject;
            GuidanceService pGuidanceServiceChose = new GuidanceService();
            SegmentLeg pSegmentLeg;
            ProcedureTransitionLeg ptl;

            string sProcName;

            _pandaQPI.ClearAllFeatures();

            InstrumentApproachProcedure pProcedure = _pandaQPI.CreateFeature<InstrumentApproachProcedure>();

            // Procedure =================================================================================================

            pLandingTakeoffAreaCollection = new LandingTakeoffAreaCollection();
            pLandingTakeoffAreaCollection.Runway.Add(PreFinalState._SelectedRWY.GetFeatureRefObject());

            pProcedure.RNAV = true;
            pProcedure.FlightChecked = false;
            pProcedure.CodingStandard = CodeProcedureCodingStandard.PANS_OPS;
            pProcedure.DesignCriteria = CodeDesignStandard.PANS_OPS;
            pProcedure.Landing = pLandingTakeoffAreaCollection;

            sProcName =
                $"RNP AR APCH {PreFinalState._SelectedRWY.Name}_Cat{(CodeAircraftCategory)PreFinalState._Category}";
            pProcedure.Name = sProcName;

            //pProcedure.CommunicationFailureDescription
            //pProcedure.Description =
            //pProcedure.ID
            //pProcedure.Note =
            //pProcedure.ProtectsSafeAltitudeAreaId = `

            IsLimitedTo = new AircraftCharacteristic();

            switch (PreFinalState._Category)
            {
                case 0:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.A;
                    break;
                case 1:

                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.B;
                    break;
                case 2:

                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.C;
                    break;
                case 3:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.D;
                    break;
                case 4:
                    IsLimitedTo.AircraftLandingCategory = CodeAircraftCategory.E;
                    break;
            }

            pProcedure.AircraftCharacteristic.Add(IsLimitedTo);

            featureRefObject = new FeatureRefObject();
            featureRef = new FeatureRef { Identifier = CurrADHP.pAirportHeliport.Identifier };
            featureRefObject.Feature = featureRef;
            pProcedure.AirportHeliport.Add(featureRefObject);

            TerminalSegmentPoint pEndPoint = null;

            //Transition 1 =========================================================================================

            pTransition = new ProcedureTransition { Type = CodeProcedurePhase.APPROACH };

            //	pTransition.Description =
            //	pTransition.ID =
            //	pTransition.Procedure =


            //Initial Legs ===============================================================================================
            for (int NO_SEQ = 1, i = IntermediateState._ImASLegs.Count - 1; i >= 1; NO_SEQ++, i--)
            {
                
                if (IntermediateState._ImASLegs[i].legType == LegType.FlyBy)
                {
                    NO_SEQ--;
                    continue;
                }

                RFLeg curLeg = IntermediateState._ImASLegs[i];
                RFLeg nextLeg = default(RFLeg);
                if (i - 2 >= 0)
                    nextLeg = IntermediateState._ImASLegs[i - 1].legType == LegType.FlyBy
                        ? IntermediateState._ImASLegs[i - 2]
                        : IntermediateState._ImASLegs[i - 1];
                pSegmentLeg = ApproachLeg(i, curLeg, nextLeg, IsLimitedTo, ref pEndPoint, i == 1);

                ptl = new ProcedureTransitionLeg
                {
                    SeqNumberARINC = (uint)NO_SEQ,
                    TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
                };
                pTransition.TransitionLeg.Add(ptl);
            }

            RFLeg leg = IntermediateState._ImASLegs[0];
            RFLeg finalLeg = FinalState._FASLegs[FinalState._FASLegs.Count - 1];
            pSegmentLeg = ApproachLeg(0, leg, finalLeg, IsLimitedTo, ref pEndPoint, true, false);
            ptl = new ProcedureTransitionLeg
            {
                SeqNumberARINC = (uint)Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs.Count,
                TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
            };
            pTransition.TransitionLeg.Add(ptl);

            pProcedure.FlightTransition.Add(pTransition);

            //Transition 2 =========================================================================================

            pTransition = new ProcedureTransition { Type = CodeProcedurePhase.FINAL };

            //Final Legs ===============================================================================================

            int seqNum = 0;
            for (int NO_SEQ = 1, i = FinalState._FASLegs.Count - 1; i >= 0; NO_SEQ++, i--)
            {
                seqNum = NO_SEQ;
                pSegmentLeg = FinalApproachLeg(i, pProcedure, IsLimitedTo, ref pEndPoint, i == 0);

                ptl = new ProcedureTransitionLeg
                {
                    SeqNumberARINC = (uint)NO_SEQ,
                    TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
                };
                pTransition.TransitionLeg.Add(ptl);
            }

            if (FinalState._FASLegs[0].legType == LegType.FixedRadius)
            {
                seqNum++;
                pSegmentLeg = FinalApproachLegFROP(pProcedure, IsLimitedTo, ref pEndPoint);

                ptl = new ProcedureTransitionLeg
                {
                    SeqNumberARINC = (uint)seqNum,
                    TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
                };
                pTransition.TransitionLeg.Add(ptl);
            }

            pProcedure.FlightTransition.Add(pTransition);

            //Missed Approach Legs ============================================================================================
            //===

            seqNum++;
            pEndPoint = null;
            pSegmentLeg = MissedApproachSplayLeg(pProcedure, IsLimitedTo, ref pEndPoint);

            ptl = new ProcedureTransitionLeg
            {
                SeqNumberARINC = (uint)seqNum,
                TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
            };
            pTransition.TransitionLeg.Add(ptl);

            seqNum++;
            for (int NO_SEQ = seqNum, i = 0; i < MissedApproachState._MASLegs.Count; NO_SEQ++, i++)
            {
                if (MissedApproachState._MASLegs[i].legType == LegType.FlyBy)
                {
                    NO_SEQ--;
                    continue;
                }

                pSegmentLeg = MissedApproachLeg(i, pProcedure, IsLimitedTo, ref pEndPoint);

                ptl = new ProcedureTransitionLeg
                {
                    SeqNumberARINC = (uint)NO_SEQ,
                    TheSegmentLeg = pSegmentLeg.GetAbstractFeatureRef<AbstractSegmentLegRef>()
                };
                pTransition.TransitionLeg.Add(ptl);
            }

            pProcedure.FlightTransition.Add(pTransition);





            try
            {
                _pandaQPI.SetRootFeatureType(FeatureType.InstrumentApproachProcedure);

                bool saveRes = _pandaQPI.Commit(new[]{
                    FeatureType.DesignatedPoint,
                    FeatureType.AngleIndication,
                    FeatureType.DistanceIndication,
                    FeatureType.StandardInstrumentDeparture,
                    FeatureType.StandardInstrumentArrival,
                    FeatureType.InstrumentApproachProcedure,
                    FeatureType.ArrivalFeederLeg,
                    FeatureType.ArrivalLeg,
                    FeatureType.DepartureLeg,
                    FeatureType.FinalLeg,
                    FeatureType.InitialLeg,
                    FeatureType.IntermediateLeg,
                    FeatureType.MissedApproachLeg});

                Env.Current.AranEnv.RefreshAllAimLayers();

                return saveRes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on commit." + "\r\n" + ex.Message);
            }

        }

        private SegmentLeg ApproachLeg(int i, RFLeg leg, RFLeg nextLeg, AircraftCharacteristic isLimitedTo, ref TerminalSegmentPoint pStartPoint, bool last, bool initial = true)
        {

            string startName = initial ? "IAF" : "IF";
            string endName = initial ? "IAF" : "IF";
            AngleIndication pAngleIndication;


            SegmentPoint pSegmentPoint;
            Feature pFixDesignatedPoint;
            SignificantPoint pFIXSignPt;

            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;
            ValSpeed pSpeed;

            //int i = (int)NO_SEQ - 1;

            ApproachLeg pInitialLeg = null;
            if (initial)
                pInitialLeg = _pandaQPI.CreateFeature<InitialLeg>();
            else pInitialLeg = _pandaQPI.CreateFeature<IntermediateLeg>();


            pInitialLeg.AircraftCategory.Add(isLimitedTo);
            //pArrivalLeg.Approach = pProcedure.GetFeatureRef();

            SegmentLeg pSegmentLeg = pInitialLeg;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LegPath = leg.legType == LegType.FixedRadius ? CodeTrajectory.ARC : CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            pSegmentLeg.LegTypeARINC = leg.legType == LegType.FixedRadius ? CodeSegmentPath.RF : CodeSegmentPath.TF;

            if (leg.legType != LegType.Straight)
                if (leg.TurnDir == TurnDirection.CW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT;
                else if (leg.TurnDir == TurnDirection.CCW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT;



            pSegmentLeg.CourseDirection = CodeDirectionReference.TO;

            double course = Env.Current.SpatialContext.SpatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
            pSegmentLeg.Course = course;

            //====================================================================================================


            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.RollOutAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.StartAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

            pDistance = new ValDistance(UnitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), UnitContext.UomHDistance);
            pSegmentLeg.Length = pDistance;

            pSegmentLeg.BankAngle = leg.BankAngle;

            pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(Math.Atan(leg.DescentGR));

            pSpeed = new ValSpeed(UnitConverter.SpeedToDisplayUnits(leg.IAS, eRoundMode.SPECIAL_NEAREST), UnitContext.UomSpeed);
            pSegmentLeg.SpeedLimit = pSpeed;

            if (pSegmentLeg.LegTypeARINC == CodeSegmentPath.RF)
            {
                var pCenterPoint = new TerminalSegmentPoint { Role = CodeProcedureFixRole.OTHER_WPT, Waypoint = false };
                pFixDesignatedPoint = CreateDesignatedPoint(leg.Center, "CENTER" + (i).ToString("00"), leg.StartDir);
                pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };
                pCenterPoint.PointChoice = pFIXSignPt;
                pSegmentLeg.ArcCentre = pCenterPoint;
            }


            if (pStartPoint == null)
            {
                pStartPoint = new TerminalSegmentPoint { Role = initial ? (last ? CodeProcedureFixRole.IAF : CodeProcedureFixRole.OTHER_WPT) : CodeProcedureFixRole.IF };

                pSegmentPoint = pStartPoint;
                pSegmentPoint.FlyOver = false;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = true;

                // ========================

                if (leg.IsWpt)
                {
                    pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.startWpt.Identifier };
                }
                else
                {
                    pFixDesignatedPoint = CreateDesignatedPoint(leg.StartPrj, startName + (i).ToString("00"), leg.StartDir);
                }

                pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

                pSegmentPoint.PointChoice = pFIXSignPt;
            }
            //========
            pSegmentLeg.StartPoint = pStartPoint;

            // End Of Start Point ========================

            // Angle ========================
            if (leg.legType != LegType.Straight)
            {
                double Angle = NativeMethods.Modulus(course - CurrADHP.MagVar, 360.0);
                pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pStartPoint.PointChoice);
                pAngleIndication.TrueAngle = course;
                pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
                pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();
            }

            //=======================================================================================

            // End Point ========================
            pStartPoint = new TerminalSegmentPoint();
            if (last)
            {
                pStartPoint.Role = initial ? CodeProcedureFixRole.IF : CodeProcedureFixRole.FAF;
            }
            else
            {
                pStartPoint.Role = CodeProcedureFixRole.OTHER_WPT;
            }

            pSegmentPoint = pStartPoint;

            pSegmentPoint.FlyOver = false;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = true;

            // ========================

            if (!nextLeg.Equals(default(RFLeg)))
            {
                if (nextLeg.IsWpt)
                {
                    pFixDesignatedPoint = new DesignatedPoint { Identifier = nextLeg.startWpt.Identifier };
                }
                else
                {
                    string wptName;
                    wptName = last ? endName : "COORD";
                    pFixDesignatedPoint = CreateDesignatedPoint(nextLeg.StartPrj, wptName + (i).ToString("00"), leg.StartDir);
                }

            }else 
            {
                string wptName;
                wptName = last ? endName : "COORD";
                pFixDesignatedPoint = CreateDesignatedPoint(leg.RollOutPrj, wptName + (i).ToString("00"), leg.StartDir);
            }

            pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };
            pSegmentPoint.PointChoice = pFIXSignPt;

            //////////////////////////

            //pSegmentLeg.StartPoint = pEndPoint;
            pSegmentLeg.EndPoint = pStartPoint;

            // End Of EndPoint =====================================================================================

            // Trajectory ===========================================
            Curve pCurve = new Curve();
            MultiLineString ls = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Nominal);
            pCurve.Geo.Add(ls);
            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =====================================================
            // protected Area =================================================

            Surface pSurface = new Surface();
            MultiPolygon pl = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Protection);
            pSurface.Geo.Add(pl);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea
            {
                Surface = pSurface,
                SectionNumber = 0,
                Type = CodeObstacleAssessmentSurface.PRIMARY
            };
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //  END ======================================================

            // Protection Area Obstructions list ==================================================
            ObstacleContainer obstacles = leg.ObstaclesList;

            Functions.Sort(ref obstacles, 2);
            int saveCount = Math.Min(obstacles.Obstacles.Length, 15);

            for (int j = 0; j < obstacles.Parts.Length; j++)
            {
                if (saveCount == 0)
                    break;

                double RequiredClearance = 0.0;

                Obstacle tmpObstacle = obstacles.Obstacles[obstacles.Parts[j].Owner];
                if (tmpObstacle.NIx > -1)
                    continue;

                tmpObstacle.NIx = 0;

                //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
                RequiredClearance = obstacles.Parts[j].MOC;

                Obstruction obs = new Obstruction
                {
                    VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[j].Identifier)
                };

                //ReqH
                double MinimumAltitude = obstacles.Obstacles[j].Height + RequiredClearance + PreFinalState.ptTHRprj.Z;

                pDistanceVertical = new ValDistanceVertical
                {
                    Uom = UnitContext.UomVDistance,
                    Value = UnitConverter.HeightToDisplayUnits(MinimumAltitude)
                };
                obs.MinimumAltitude = pDistanceVertical;

                // MOC
                pDistance = new ValDistance();
                pDistance.Uom = UomDistance.M;
                pDistance.Value = RequiredClearance;
                obs.RequiredClearance = pDistance;
                pPrimProtectedArea.SignificantObstacle.Add(obs);

                pDistanceVertical = null;
                pDistance = null;
                saveCount -= 1;
            }

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //=================================================================================================================
            ApproachCondition pCondition = new ApproachCondition();
            pCondition.AircraftCategory.Add(isLimitedTo);

            Minima pMinimumSet = new Minima
            {
                AltitudeCode = CodeMinimumAltitude.OCA,
                AltitudeReference = CodeVerticalReference.MSL,
                HeightCode = CodeMinimumHeight.OCH,
                HeightReference = CodeHeightReference.HAT
            };

            // protected Area =================================================

            return pInitialLeg;
        }

        private ApproachLeg FinalApproachLeg(int i, InstrumentApproachProcedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pStartPoint, bool last)
        {
            SegmentPoint pSegmentPoint;
            AngleIndication pAngleIndication;

            ValSpeed pSpeed;
            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;

            Feature pFixDesignatedPoint;
            SignificantPoint pFIXSignPt;

            //int i = (int)NO_SEQ - 1;

            RFLeg leg = FinalState._FASLegs[i];

            FinalLeg pFinalLeg = _pandaQPI.CreateFeature<FinalLeg>();
            pFinalLeg.GuidanceSystem = CodeFinalGuidance.LPV;
            pFinalLeg.AircraftCategory.Add(IsLimitedTo);

            //pFinalLeg.Approach = pProcedure.GetFeatureRef();

            SegmentLeg pSegmentLeg = pFinalLeg;
            // pSegmentLeg.AltitudeOverrideATC =
            // pSegmentLeg.AltitudeOverrideReference =
            // pSegmentLeg.Duration = ' ???????????????????????????????? pLegs(I).valDur
            // pSegmentLeg.Note =
            // pSegmentLeg.ProcedureTurnRequired =
            // pSegmentLeg.ReqNavPerformance = 
            // pSegmentLeg.SpeedInterpretation = CodeAltitudeUse.RECOMMENDED;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LegPath = leg.legType == LegType.FixedRadius ? CodeTrajectory.ARC : CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            pSegmentLeg.LegTypeARINC = leg.legType == LegType.FixedRadius ? CodeSegmentPath.RF : CodeSegmentPath.TF;

            if (leg.legType != LegType.Straight)
            {
                if (leg.TurnDir == TurnDirection.CW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT;
                else if (leg.TurnDir == TurnDirection.CCW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT;
            }

            pSegmentLeg.CourseDirection = CodeDirectionReference.TO;

            double course = Env.Current.SpatialContext.SpatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
            pSegmentLeg.Course = course;

            //====================================================================================================

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.RollOutAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.StartAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

            pDistance = new ValDistance(UnitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), UnitContext.UomHDistance);
            pSegmentLeg.Length = pDistance;

            pSegmentLeg.BankAngle = leg.BankAngle;
            pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(Math.Atan(leg.DescentGR));


            pSpeed = new ValSpeed(UnitConverter.SpeedToDisplayUnits(PreFinalState._IAS, eRoundMode.SPECIAL_NEAREST), UnitContext.UomSpeed);
            pSegmentLeg.SpeedLimit = pSpeed;

            // Start Point ========================
            if (pStartPoint == null)
            {
                pStartPoint = new TerminalSegmentPoint { Role = CodeProcedureFixRole.FAF };
                //	pStartPoint.IndicatorFACF =      ??????????
                //	pStartPoint.LeadDME =            ??????????
                //	pStartPoint.LeadRadial =         ??????????

                //==

                pSegmentPoint = pStartPoint;
                pSegmentPoint.FlyOver = false;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = true;

                // ========================

                if (leg.IsWpt)
                {
                    pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.startWpt.Identifier };
                }
                else
                    pFixDesignatedPoint = CreateDesignatedPoint(leg.StartPrj, "FAF" + (i).ToString("00"), leg.StartDir);
                pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

                pSegmentPoint.PointChoice = pFIXSignPt;
            }
            //========

            pSegmentLeg.StartPoint = pStartPoint;
            // End Of Start Point ========================

            // Angle ========================
            double Angle = NativeMethods.Modulus(course - CurrADHP.MagVar, 360.0);

            pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pStartPoint.PointChoice);
            pAngleIndication.TrueAngle = course;
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

            // EndPoint ========================
            pStartPoint = new TerminalSegmentPoint();
            //pEndPoint.IndicatorFACF =      ??????????
            //pEndPoint.LeadDME =            ??????????
            //pEndPoint.LeadRadial =         ??????????
            pStartPoint.Role = CodeProcedureFixRole.FROP;

            pSegmentPoint = pStartPoint;

            pSegmentPoint.FlyOver = true;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = false;

            // ========================

            if (leg.IsEndWpt)
            {
                pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.endWpt.Identifier };
            }
            else
                pFixDesignatedPoint = CreateDesignatedPoint(leg.RollOutPrj, "FROP" + (i).ToString("00"), leg.RollOutDir);

            pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

            pSegmentPoint.PointChoice = pFIXSignPt;

            //////////////////////////

            pSegmentLeg.EndPoint = pStartPoint;

            // End Of EndPoint =====================================================================================
            // Trajectory =====================================================

            Curve pCurve = new Curve();
            MultiLineString ls = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Nominal);
            pCurve.Geo.Add(ls);
            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =====================================================
            // protected Area =================================================

            Surface pSurface = new Surface();
            MultiPolygon pl = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Protection);
            pSurface.Geo.Add(pl);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = pSurface;
            pPrimProtectedArea.SectionNumber = 0;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //  END ======================================================

            // Protection Area Obstructions list ==================================================
            ObstacleContainer obstacles = leg.ObstaclesList;

            Functions.Sort(ref obstacles, 2);
            int saveCount = Math.Min(obstacles.Obstacles.Length, 15);

            for (i = 0; i < obstacles.Parts.Length; i++)
            {
                if (saveCount == 0)
                    break;

                double RequiredClearance = 0.0;

                Obstacle tmpObstacle = obstacles.Obstacles[obstacles.Parts[i].Owner];
                if (tmpObstacle.NIx > -1)
                    continue;

                tmpObstacle.NIx = 0;

                //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
                RequiredClearance = obstacles.Parts[i].MOC;

                Obstruction obs = new Obstruction();
                obs.VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[i].Identifier);

                //ReqH
                double MinimumAltitude = obstacles.Obstacles[i].Height + RequiredClearance + PreFinalState.ptTHRprj.Z;

                pDistanceVertical = new ValDistanceVertical
                {
                    Uom = UnitContext.UomVDistance,
                    Value = UnitConverter.HeightToDisplayUnits(MinimumAltitude)
                };
                obs.MinimumAltitude = pDistanceVertical;

                // MOC
                pDistance = new ValDistance { Uom = UomDistance.M, Value = RequiredClearance };
                obs.RequiredClearance = pDistance;
                pPrimProtectedArea.SignificantObstacle.Add(obs);

                pDistanceVertical = null;
                pDistance = null;
                saveCount -= 1;
            }

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //=================================================================================================================
            ApproachCondition pCondition = new ApproachCondition();
            pCondition.AircraftCategory.Add(IsLimitedTo);

            Minima pMinimumSet = new Minima
            {
                AltitudeCode = CodeMinimumAltitude.OCA,
                AltitudeReference = CodeVerticalReference.MSL,
                HeightCode = CodeMinimumHeight.OCH,
                HeightReference = CodeHeightReference.HAT
            };

            return pFinalLeg;
        }

        private ApproachLeg FinalApproachLegFROP(InstrumentApproachProcedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pStartPoint)
        {
            SegmentPoint pSegmentPoint;
            AngleIndication pAngleIndication;

            ValSpeed pSpeed;
            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;


            //int i = (int)NO_SEQ - 1;

            RFLeg leg = FinalState._FASLegs[0];

            FinalLeg pFinalLeg = _pandaQPI.CreateFeature<FinalLeg>();
            pFinalLeg.GuidanceSystem = CodeFinalGuidance.LPV;
            pFinalLeg.AircraftCategory.Add(IsLimitedTo);

            //pFinalLeg.Approach = pProcedure.GetFeatureRef();

            SegmentLeg pSegmentLeg = pFinalLeg;
            // pSegmentLeg.AltitudeOverrideATC =
            // pSegmentLeg.AltitudeOverrideReference =
            // pSegmentLeg.Duration = ' ???????????????????????????????? pLegs(I).valDur
            // pSegmentLeg.Note =
            // pSegmentLeg.ProcedureTurnRequired =
            // pSegmentLeg.ReqNavPerformance = 
            // pSegmentLeg.SpeedInterpretation = CodeAltitudeUse.RECOMMENDED;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LegPath = CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;
            pSegmentLeg.CourseDirection = CodeDirectionReference.FROM;

            double course = Env.Current.SpatialContext.SpatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
            pSegmentLeg.Course = course;

            //====================================================================================================

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.RollOutAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

            //pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.StartAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            //pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            pDistance = new ValDistance(UnitConverter.DistanceToDisplayUnits(PreFinalState._FASegmentNominal.Length, eRoundMode.NEAREST), UnitContext.UomHDistance);
            pSegmentLeg.Length = pDistance;

            pSegmentLeg.BankAngle = leg.BankAngle;
            pSegmentLeg.VerticalAngle = -ARANMath.RadToDeg(Math.Atan(leg.DescentGR));


            pSpeed = new ValSpeed(UnitConverter.SpeedToDisplayUnits(PreFinalState._IAS, eRoundMode.SPECIAL_NEAREST), UnitContext.UomSpeed);
            pSegmentLeg.SpeedLimit = pSpeed;

            // Start Point ========================

            // End Of Start Point ========================

            // Angle ========================
            double Angle = NativeMethods.Modulus(course - CurrADHP.MagVar, 360.0);

            pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pStartPoint.PointChoice);
            pAngleIndication.TrueAngle = course;
            pAngleIndication.IndicationDirection = CodeDirectionReference.FROM;
            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

            // EndPoint =======================

            pSegmentLeg.EndPoint = pStartPoint;

            // End Of EndPoint =====================================================================================
            // Trajectory =====================================================

            Curve pCurve = new Curve();
            MultiLineString ls = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(PreFinalState._FASegmentNominal);
            pCurve.Geo.Add(ls);
            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =====================================================
            // protected Area =================================================

            Surface pSurface = new Surface();
            MultiPolygon pl = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(PreFinalState._FASegmentPolygon);
            pSurface.Geo.Add(pl);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = pSurface;
            pPrimProtectedArea.SectionNumber = 0;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //  END ======================================================

            // Protection Area Obstructions list ==================================================
            ObstacleContainer obstacles = PreFinalState.FASObstacleList;

            Functions.Sort(ref obstacles, 2);
            int saveCount = Math.Min(obstacles.Obstacles.Length, 15);


            for (int i = 0; i < obstacles.Parts.Length; i++)
            {
                if (saveCount == 0)
                    break;

                double RequiredClearance = 0.0;

                Obstacle tmpObstacle = obstacles.Obstacles[obstacles.Parts[i].Owner];
                if (tmpObstacle.NIx > -1)
                    continue;

                tmpObstacle.NIx = 0;

                //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
                RequiredClearance = obstacles.Parts[i].MOC;

                Obstruction obs = new Obstruction();
                obs.VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[i].Identifier);

                //ReqH
                double MinimumAltitude = obstacles.Obstacles[i].Height + RequiredClearance + PreFinalState.ptTHRprj.Z;

                pDistanceVertical = new ValDistanceVertical
                {
                    Uom = UnitContext.UomVDistance,
                    Value = UnitConverter.HeightToDisplayUnits(MinimumAltitude)
                };
                obs.MinimumAltitude = pDistanceVertical;

                // MOC
                pDistance = new ValDistance { Uom = UomDistance.M, Value = RequiredClearance };
                obs.RequiredClearance = pDistance;
                pPrimProtectedArea.SignificantObstacle.Add(obs);

                pDistanceVertical = null;
                pDistance = null;
                saveCount -= 1;
            }

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //=================================================================================================================
            ApproachCondition pCondition = new ApproachCondition();
            pCondition.AircraftCategory.Add(IsLimitedTo);

            Minima pMinimumSet = new Minima
            {
                AltitudeCode = CodeMinimumAltitude.OCA,
                AltitudeReference = CodeVerticalReference.MSL,
                HeightCode = CodeMinimumHeight.OCH,
                HeightReference = CodeHeightReference.HAT
            };



            //If CheckBox0301.Checked Then
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //Else
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), UnitContext.UomVDistance)
            //End If
            //pCondition.MinimumSet = pMinimumSet;
            //pFinalLeg.Condition.Add(pCondition);

            // protected Area =================================================

            return pFinalLeg;
        }

        private ApproachLeg MissedApproachLeg(int i, InstrumentApproachProcedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pStartPoint)
        {
            SegmentPoint pSegmentPoint;
            AngleIndication pAngleIndication;

            ValSpeed pSpeed;
            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;

            Feature pFixDesignatedPoint;
            SignificantPoint pFIXSignPt;

            //int i = (int)NO_SEQ - 1;

            RFLeg leg = MissedApproachState._MASLegs[i];

            MissedApproachLeg missedApproachLeg = _pandaQPI.CreateFeature<MissedApproachLeg>();
            missedApproachLeg.AircraftCategory.Add(IsLimitedTo);

            //pFinalLeg.Approach = pProcedure.GetFeatureRef();

            SegmentLeg pSegmentLeg = missedApproachLeg;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LegPath = leg.legType == LegType.FixedRadius ? CodeTrajectory.ARC : CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            pSegmentLeg.LegTypeARINC = leg.legType == LegType.Straight ? CodeSegmentPath.TF : CodeSegmentPath.RF;

            if (leg.legType != LegType.Straight)
            {
                if (leg.TurnDir == TurnDirection.CW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.RIGHT;
                else if (leg.TurnDir == TurnDirection.CCW)
                    pSegmentLeg.TurnDirection = CodeDirectionTurn.LEFT;
            }

            pSegmentLeg.CourseDirection = CodeDirectionReference.TO;

            double course = Env.Current.SpatialContext.SpatialReferenceOperation.DirToAztPrj(leg.RollOutPrj, leg.RollOutDir);
            pSegmentLeg.Course = course;

            //====================================================================================================

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.RollOutAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.StartAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

            pDistance = new ValDistance(UnitConverter.DistanceToDisplayUnits(leg.Nominal.Length, eRoundMode.NEAREST), UnitContext.UomHDistance);
            pSegmentLeg.Length = pDistance;

            pSegmentLeg.BankAngle = leg.BankAngle;
            pSegmentLeg.VerticalAngle = ARANMath.RadToDeg(Math.Atan(leg.DescentGR));


            pSpeed = new ValSpeed(UnitConverter.SpeedToDisplayUnits(leg.IAS, eRoundMode.SPECIAL_NEAREST), UnitContext.UomSpeed);
            pSegmentLeg.SpeedLimit = pSpeed;

            // END Point ========================
            if (pStartPoint == null)
            {
                pStartPoint = new TerminalSegmentPoint { Role = CodeProcedureFixRole.OTHER_WPT };

                pSegmentPoint = pStartPoint;
                pSegmentPoint.FlyOver = false;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = true;

                // ========================

                if (leg.IsEndWpt)
                {
                    pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.endWpt.Identifier };
                }
                else
                    pFixDesignatedPoint = CreateDesignatedPoint(leg.RollOutPrj, "COORD" + (i).ToString("00"), leg.RollOutDir);
                pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

                pSegmentPoint.PointChoice = pFIXSignPt;
            }
            //========

            pSegmentLeg.StartPoint = pStartPoint;
            // End Of END Point ========================

            // Angle ========================
            double Angle = NativeMethods.Modulus(course - CurrADHP.MagVar, 360.0);

            pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pStartPoint.PointChoice);
            pAngleIndication.TrueAngle = course;
            pAngleIndication.IndicationDirection = CodeDirectionReference.TO;
            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();

            // Start ========================

            pStartPoint = new TerminalSegmentPoint();
            bool isTp = MissedApproachState._MASLegs.Count - 1 > i + 1 &&
                        MissedApproachState._MASLegs[i + 1].legType == LegType.FlyBy;
            bool isLast = MissedApproachState._MASLegs.Count - 1 == i;

            pStartPoint.Role = isLast ? CodeProcedureFixRole.MAHF : CodeProcedureFixRole.OTHER_WPT;

            pSegmentPoint = pStartPoint;

            pSegmentPoint.FlyOver = true;
            pSegmentPoint.RadarGuidance = false;
            pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
            pSegmentPoint.Waypoint = false;

            // ========================


            if (leg.IsWpt)
            {
                pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.startWpt.Identifier };
            }
            else
                pFixDesignatedPoint = CreateDesignatedPoint(leg.StartPrj, isLast ? "MAHF" : isTp ? "MATF" : "COORD" + (i).ToString("00"), leg.StartDir);
            pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

            pSegmentPoint.PointChoice = pFIXSignPt;

            //////////////////////////

            pSegmentLeg.EndPoint = pStartPoint;

            // End Of EndPoint =====================================================================================
            // Trajectory =====================================================

            Curve pCurve = new Curve();
            MultiLineString ls = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Nominal);
            pCurve.Geo.Add(ls);
            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =====================================================
            // protected Area =================================================

            Surface pSurface = new Surface();
            MultiPolygon pl = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Protection);
            pSurface.Geo.Add(pl);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = pSurface;
            pPrimProtectedArea.SectionNumber = 0;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //  END ======================================================

            // Protection Area Obstructions list ==================================================
            ObstacleContainer obstacles = leg.ObstaclesList;

            Functions.Sort(ref obstacles, 2);
            int saveCount = Math.Min(obstacles.Obstacles.Length, 15);

            for (i = 0; i < obstacles.Parts.Length; i++)
            {
                if (saveCount == 0)
                    break;

                double RequiredClearance = 0.0;

                Obstacle tmpObstacle = obstacles.Obstacles[obstacles.Parts[i].Owner];
                if (tmpObstacle.NIx > -1)
                    continue;

                tmpObstacle.NIx = 0;

                //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
                RequiredClearance = obstacles.Parts[i].MOC;

                Obstruction obs = new Obstruction();
                obs.VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[i].Identifier);

                //ReqH
                double MinimumAltitude = obstacles.Obstacles[i].Height + RequiredClearance + PreFinalState.ptTHRprj.Z;

                pDistanceVertical = new ValDistanceVertical
                {
                    Uom = UnitContext.UomVDistance,
                    Value = UnitConverter.HeightToDisplayUnits(MinimumAltitude)
                };
                obs.MinimumAltitude = pDistanceVertical;

                // MOC
                pDistance = new ValDistance { Uom = UomDistance.M, Value = RequiredClearance };
                obs.RequiredClearance = pDistance;
                pPrimProtectedArea.SignificantObstacle.Add(obs);

                pDistanceVertical = null;
                pDistance = null;
                saveCount -= 1;
            }

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //=================================================================================================================
            ApproachCondition pCondition = new ApproachCondition();
            pCondition.AircraftCategory.Add(IsLimitedTo);

            Minima pMinimumSet = new Minima
            {
                AltitudeCode = CodeMinimumAltitude.OCA,
                AltitudeReference = CodeVerticalReference.MSL,
                HeightCode = CodeMinimumHeight.OCH,
                HeightReference = CodeHeightReference.HAT
            };



            //If CheckBox0301.Checked Then
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //Else
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), UnitContext.UomVDistance)
            //End If
            //pCondition.MinimumSet = pMinimumSet;
            //pFinalLeg.Condition.Add(pCondition);

            // protected Area =================================================

            return missedApproachLeg;
        }

        private ApproachLeg MissedApproachSplayLeg(InstrumentApproachProcedure pProcedure, AircraftCharacteristic IsLimitedTo, ref TerminalSegmentPoint pStartPoint)
        {
            SegmentPoint pSegmentPoint;
            AngleIndication pAngleIndication;

            ValSpeed pSpeed;
            ValDistance pDistance;
            ValDistanceVertical pDistanceVertical;

            Feature pFixDesignatedPoint;
            SignificantPoint pFIXSignPt;

            //int i = (int)NO_SEQ - 1;

            RFLeg leg = MissedApproachState._MASLegs[0];

            MissedApproachLeg missedApproachLeg = _pandaQPI.CreateFeature<MissedApproachLeg>();
            missedApproachLeg.AircraftCategory.Add(IsLimitedTo);

            //pFinalLeg.Approach = pProcedure.GetFeatureRef();

            SegmentLeg pSegmentLeg = missedApproachLeg;

            pSegmentLeg.AltitudeInterpretation = CodeAltitudeUse.BETWEEN;
            pSegmentLeg.UpperLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LowerLimitReference = CodeVerticalReference.MSL;
            pSegmentLeg.LegPath = leg.legType == LegType.FixedRadius ? CodeTrajectory.ARC : CodeTrajectory.STRAIGHT;
            pSegmentLeg.CourseType = CodeCourse.TRUE_TRACK;
            //pSegmentLeg.EndConditionDesignator = Aran.Aim.Enums.CodeSegmentTermination.INTERCEPT;
            pSegmentLeg.SpeedReference = CodeSpeedReference.IAS;

            pSegmentLeg.LegTypeARINC = CodeSegmentPath.CF;


            pSegmentLeg.CourseDirection = CodeDirectionReference.TO;

            double course = Env.Current.SpatialContext.SpatialReferenceOperation.DirToAztPrj(leg.StartPrj, leg.StartDir);
            pSegmentLeg.Course = course;

            //====================================================================================================

            //pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.RollOutAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            //pSegmentLeg.LowerLimitAltitude = pDistanceVertical;

            pDistanceVertical = new ValDistanceVertical(UnitConverter.HeightToDisplayUnits(leg.StartAltitude + PreFinalState.ptTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance);
            pSegmentLeg.UpperLimitAltitude = pDistanceVertical;

            pDistance = new ValDistance(UnitConverter.DistanceToDisplayUnits(PreFinalState._MASSplaySegmentNominal.Length, eRoundMode.NEAREST), UnitContext.UomHDistance);
            pSegmentLeg.Length = pDistance;

            pSegmentLeg.BankAngle = leg.BankAngle;
            pSegmentLeg.VerticalAngle = ARANMath.RadToDeg(Math.Atan(leg.DescentGR));


            pSpeed = new ValSpeed(UnitConverter.SpeedToDisplayUnits(leg.IAS, eRoundMode.SPECIAL_NEAREST), UnitContext.UomSpeed);
            pSegmentLeg.SpeedLimit = pSpeed;

            // END Point ========================
            if (pStartPoint == null)
            {
                pStartPoint = new TerminalSegmentPoint { Role = CodeProcedureFixRole.OTHER_WPT };

                pSegmentPoint = pStartPoint;
                pSegmentPoint.FlyOver = false;
                pSegmentPoint.RadarGuidance = false;

                pSegmentPoint.ReportingATC = CodeATCReporting.NO_REPORT;
                pSegmentPoint.Waypoint = true;

                // ========================

                if (leg.IsEndWpt)
                {
                    pFixDesignatedPoint = new DesignatedPoint { Identifier = leg.endWpt.Identifier };
                }
                else
                    pFixDesignatedPoint = CreateDesignatedPoint(leg.RollOutPrj, "COORD" + (0).ToString("00"), leg.RollOutDir);
                pFIXSignPt = new SignificantPoint { FixDesignatedPoint = pFixDesignatedPoint.GetFeatureRef() };

                pSegmentPoint.PointChoice = pFIXSignPt;
            }
            //========

            pSegmentLeg.StartPoint = pStartPoint;
            // End Of END Point ========================

            // Angle ========================
            double Angle = NativeMethods.Modulus(course - CurrADHP.MagVar, 360.0);

            pAngleIndication = CreateAngleIndication(Angle, CodeBearing.MAG, pStartPoint.PointChoice);
            pAngleIndication.TrueAngle = course;
            pAngleIndication.IndicationDirection = CodeDirectionReference.TO;
            pSegmentLeg.Angle = pAngleIndication.GetFeatureRef();


            // Trajectory =====================================================

            Curve pCurve = new Curve();
            MultiLineString ls = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(PreFinalState._MASSplaySegmentNominal);
            pCurve.Geo.Add(ls);
            pSegmentLeg.Trajectory = pCurve;

            // Trajectory =====================================================
            // protected Area =================================================

            Surface pSurface = new Surface();
            MultiPolygon pl = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(PreFinalState._MASSplaySegmentPolygon);
            pSurface.Geo.Add(pl);

            ObstacleAssessmentArea pPrimProtectedArea = new ObstacleAssessmentArea();
            pPrimProtectedArea.Surface = pSurface;
            pPrimProtectedArea.SectionNumber = 0;
            pPrimProtectedArea.Type = CodeObstacleAssessmentSurface.PRIMARY;
            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //  END ======================================================

            // Protection Area Obstructions list ==================================================
            ObstacleContainer obstacles = PreFinalState.MASObstacleList;

            Functions.Sort(ref obstacles, 2);
            int saveCount = Math.Min(obstacles.Obstacles.Length, 15);


            for (int i = 0; i < obstacles.Parts.Length; i++)
            {
                if (saveCount == 0)
                    break;

                double RequiredClearance = 0.0;

                Obstacle tmpObstacle = obstacles.Obstacles[obstacles.Parts[i].Owner];
                if (tmpObstacle.NIx > -1)
                    continue;

                tmpObstacle.NIx = 0;

                //MinimumAltitude = Math.Max(MinimumAltitude, ostacles.Parts[ostacles.Obstacles[i].Parts[j]].ReqH)
                RequiredClearance = obstacles.Parts[i].MOC;

                Obstruction obs = new Obstruction();
                obs.VerticalStructureObstruction = new FeatureRef(obstacles.Obstacles[i].Identifier);

                //ReqH
                double MinimumAltitude = obstacles.Obstacles[i].Height + RequiredClearance + PreFinalState.ptTHRprj.Z;

                pDistanceVertical = new ValDistanceVertical
                {
                    Uom = UnitContext.UomVDistance,
                    Value = UnitConverter.HeightToDisplayUnits(MinimumAltitude)
                };
                obs.MinimumAltitude = pDistanceVertical;

                // MOC
                pDistance = new ValDistance { Uom = UomDistance.M, Value = RequiredClearance };
                obs.RequiredClearance = pDistance;
                pPrimProtectedArea.SignificantObstacle.Add(obs);

                pDistanceVertical = null;
                pDistance = null;
                saveCount -= 1;
            }

            pSegmentLeg.DesignSurface.Add(pPrimProtectedArea);

            //=================================================================================================================
            ApproachCondition pCondition = new ApproachCondition();
            pCondition.AircraftCategory.Add(IsLimitedTo);

            Minima pMinimumSet = new Minima
            {
                AltitudeCode = CodeMinimumAltitude.OCA,
                AltitudeReference = CodeVerticalReference.MSL,
                HeightCode = CodeMinimumHeight.OCH,
                HeightReference = CodeHeightReference.HAT
            };



            //If CheckBox0301.Checked Then
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(fTA_OCH + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(fTA_OCH, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //Else
            //	pMinimumSet.Altitude = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH) + FicTHRprj.Z, eRoundMode.NEAREST), UnitContext.UomVDistance)
            //	pMinimumSet.Height = New ValDistanceVertical(ConvertHeight(Math.Max(fMisAprOCH, _CurrFAPOCH), eRoundMode.NEAREST), UnitContext.UomVDistance)
            //End If
            //pCondition.MinimumSet = pMinimumSet;
            //pFinalLeg.Condition.Add(pCondition);

            // protected Area =================================================

            return missedApproachLeg;
        }


        public Feature CreateDesignatedPoint(Point pPtPrj, string Name = "COORD", double fDir = -1000.0, double distTresh = 0.1)
        {
            bool bExist;

            double fDist;
            double fDirToPt;

            WPT_FIXType WptFIX = new WPT_FIXType();
            DesignatedPoint pFixDesignatedPoint;

            double fMinDist = 10000.0;
            int n = WPTList.Length;


            for (int i = 0; i < n; i++)
            {
                fDist = ARANFunctions.ReturnDistanceInMeters(pPtPrj, WPTList[i].pPtPrj);

                if (fDist < fMinDist)
                {
                    fMinDist = fDist;
                    WptFIX = WPTList[i];

                    if (fMinDist == 0.0)
                        break;
                }
            }

            bExist = fMinDist <= distTresh;

            if (!bExist && fMinDist <= 100.0 && fDir != -1000.0)    //????????????
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

            pFixDesignatedPoint = _pandaQPI.CreateFeature<DesignatedPoint>();
            pFixDesignatedPoint.Designator = "COORD";
            if (Name == "")
                Name = "COORD";

            pFixDesignatedPoint.Name = Name;

            AixmPoint aixmPoint = new AixmPoint();
            Point pPoint = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(pPtPrj);

            aixmPoint.Geo.X = pPoint.X;
            aixmPoint.Geo.Y = pPoint.Y;
            aixmPoint.Geo.Z = pPoint.Z;
            aixmPoint.Geo.M = pPoint.M;
            aixmPoint.Geo.T = 0;

            pFixDesignatedPoint.Location = aixmPoint;
            //pFixDesignatedPoint.Note;
            //pFixDesignatedPoint.Tag;
            pFixDesignatedPoint.Type = CodeDesignatedPoint.DESIGNED;
            return pFixDesignatedPoint;
        }


        public AngleIndication CreateAngleIndication(Double Angle, CodeBearing angleType, SignificantPoint pSignificantPoint)
        {
            var pAngleIndication = _pandaQPI.CreateFeature<AngleIndication>();
            pAngleIndication.Angle = Angle;
            pAngleIndication.AngleType = angleType;
            pAngleIndication.PointChoice = pSignificantPoint;

            return pAngleIndication;
        }
    }




}
