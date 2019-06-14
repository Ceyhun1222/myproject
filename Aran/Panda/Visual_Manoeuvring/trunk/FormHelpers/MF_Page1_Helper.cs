using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Panda.Common;
using Aran.Panda.Constants;
using Aran.Geometries.Operators;
using Aran.Geometries;
using Aran.Aim.Features;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    public class MF_Page1_Helper
    {
        public void setCategory(int cat, int catVal)
        {            
            VMManager.Instance.Category = catVal;
            VMManager.Instance.CA_OCA = GlobalVars.CurrADHP.Elev + 300;
            switch (VMManager.Instance.Category)
            {
                case 0: // Cat A                    
                    VMManager.Instance.MinVisibilityDistance = 1900;
                    break;
                case 1: // Cat B
                    VMManager.Instance.MinVisibilityDistance = 2800;
                    break;
                case 2: // Cat C
                    VMManager.Instance.MinVisibilityDistance = 3700;
                    break;
                case 3: // Cat D
                    VMManager.Instance.MinVisibilityDistance = 4600;
                    break;
                case 5: // Cat E
                    VMManager.Instance.MinVisibilityDistance = 6500;
                    break;
            }
        }

        /// <summary>
        /// This method calcultes BankAngle, IAS, TAS, TAS+wind, rate of turn, radius of turn, straight segment length values for the specified aircraft category.
        /// </summary>
        /// <param name="Cat">Aircraft category</param>
        /// <param name="MSAObstacles">Array</param>
        public void vsManev(int Cat)
        {
            double _windSpeedInKm_h = GlobalVars.pansopsCoreDatabase.ArVisualWs.Value / GlobalVars.pansopsCoreDatabase.ArVisualWs.Multiplier;
            VMManager.Instance.Category = Cat;
            int OIx;

            VMManager.Instance.CA_IAS = GlobalVars.constants.AircraftCategory[aircraftCategoryData.Vva].Value[VMManager.Instance.Category];
            VMManager.Instance.CA_TAS = ARANMath.IASToTAS(3.6 * VMManager.Instance.CA_IAS, VMManager.Instance.CA_OCA, GlobalVars.CurrADHP.ISAtC) * 0.277777777777778;
            VMManager.Instance.CA_BankAngle = System.Math.Atan(0.003 * ARANMath.C_PI * (3.6 * VMManager.Instance.CA_TAS + _windSpeedInKm_h) / 6.355);
            if (VMManager.Instance.CA_BankAngle > GlobalVars.constants.Pansops[ePANSOPSData.arVisAverBank].Value)
                VMManager.Instance.CA_BankAngle = GlobalVars.constants.Pansops[ePANSOPSData.arVisAverBank].Value;
            VMManager.Instance.CA_RateOfTurn = System.Math.Round(6355.0 * System.Math.Tan(VMManager.Instance.CA_BankAngle) / (ARANMath.C_PI * (3.6 * VMManager.Instance.CA_TAS + _windSpeedInKm_h)), 1);
            VMManager.Instance.CA_TASWind = VMManager.Instance.CA_TAS + GlobalVars.pansopsCoreDatabase.ArVisualWs.Value;
            VMManager.Instance.CA_RadiusOfTurn = ARANMath.BankToRadius(VMManager.Instance.CA_BankAngle, VMManager.Instance.CA_TASWind);
            VMManager.Instance.CA_StraightSegment = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arStraightSegmen].Value[VMManager.Instance.Category];
            VMManager.Instance.CA_RadiusFromTHR = 2.0 * VMManager.Instance.CA_RadiusOfTurn + VMManager.Instance.CA_StraightSegment;
            VMManager.Instance.MOC = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arObsClearance].Value[VMManager.Instance.Category];
            VMManager.Instance.MinOCH = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinOCH].Value[VMManager.Instance.Category];
            VMManager.Instance.MinOCA = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMinOCH].Value[VMManager.Instance.Category] + GlobalVars.CurrADHP.Elev;
            VMManager.Instance.CA_OCA_Actual = CalculateMOCA(out OIx);
            VMManager.Instance.CA_OCH_Actual = VMManager.Instance.CA_OCA_Actual - GlobalVars.CurrADHP.Elev;
            

            if (OIx >= 0)
                VMManager.Instance.CA_ObstacleID = VMManager.Instance.MSAObstacles[OIx].VerticalStructure.Id.ToString();
            else
                VMManager.Instance.CA_ObstacleID = "";

            if (VMManager.Instance.CA_OCA_Actual > VMManager.Instance.CA_OCA)
            {
                VMManager.Instance.CA_OCA = VMManager.Instance.CA_OCA_Actual;
                VMManager.Instance.CA_OCH = VMManager.Instance.CA_OCA - GlobalVars.CurrADHP.Elev;
                //vsManev(Cat);
            }


            if (VMManager.Instance.CirclingAreaPolyElement >= 0)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.CirclingAreaPolyElement);
            VMManager.Instance.CirclingAreaPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.ConvexPoly, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsNull);
        }

        private double CalculateMOCA(out int OIx)
        {
            double newElevation;

            GeometryOperators geomOper = new GeometryOperators();
            MultiPolygon sumPoly;
            Point ptCentr;

            sumPoly = new Aran.Geometries.MultiPolygon();

            for (int i = 0; i < GlobalVars.RWYList.Length; i++)
            {
                if (GlobalVars.RWYList[i].Selected)
                {
                    ptCentr = GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR];

                    if (sumPoly.IsEmpty)
                    {
                        sumPoly = ARANFunctions.CreateCircleAsMultiPolyPrj(ptCentr, VMManager.Instance.CA_RadiusFromTHR);
                    }
                    else
                    {
                        sumPoly = (MultiPolygon)geomOper.UnionGeometry(ARANFunctions.CreateCircleAsMultiPolyPrj(ptCentr, VMManager.Instance.CA_RadiusFromTHR), sumPoly);
                    }
                }
            }

            VMManager.Instance.ConvexPoly = (MultiPolygon)geomOper.ConvexHull(sumPoly);

            newElevation = Functions.MaxObstacleElevationInPoly(VMManager.Instance.AllObstacles, out VMManager.Instance.MSAObstacles, VMManager.Instance.ConvexPoly, out OIx);

            newElevation = newElevation + VMManager.Instance.MOC;

            if (newElevation < VMManager.Instance.MinOCA)
            {
                newElevation = VMManager.Instance.MinOCA;
                OIx = -1;
            }

            return newElevation;
        }

        public void ConstructArrivalArea()
        {
            //MultiPolygon poly = new MultiPolygon();
            //poly = ARANFunctions.CreateCircleAsMultiPolyPrj(GlobalVars.CurrADHP.pPtPrj, VMManager.Instance.ArrivalRadius);

            //if(VMManager.Instance.ArrivalAreaPolyElement > -1)
            //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ArrivalAreaPolyElement);

            //VMManager.Instance.ArrivalAreaPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(poly, 255, AranEnvironment.Symbols.eFillStyle.sfsNull);
        }

        public void GetDesignatedPointsWithinArrivalArea()
        {
            //DBModule.GetDesignatedPointByDist(out VMManager.Instance.DesignatedPointsList, GlobalVars.CurrADHP.pPtPrj, VMManager.Instance.ArrivalRadius);
            //for (int i = 0; i < VMManager.Instance.DesignatedPointsList.Count; i++)
            //{
            //    if (VMManager.Instance.DesignatedPointsList[i].Designator != null && VMManager.Instance.DesignatedPointsList[i].Designator.Equals("COORD"))
            //    {
            //        VMManager.Instance.DesignatedPointsList.RemoveAt(i);
            //        i--;
            //    }
            //}
            //ExcludeDesignatedPointsWithinCirclingArea();
        }

        public void ExcludeDesignatedPointsWithinCirclingArea()
        {
            //if (VMManager.Instance.DesignatedPointsList == null)
            //    return;
            //VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
            //for (int i = 0; i < VMManager.Instance.DesignatedPointsList.Count; i++)
            //{
            //    if (VMManager.Instance.GeomOper.Contains(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.DesignatedPointsList[i].Location.Geo)))
            //    {
            //        VMManager.Instance.DesignatedPointsList.RemoveAt(i);
            //        i--;
            //    }
            //}
        }








        public int FillArNavListForCircling(Aran.Geometries.Point pCentroid, out NavaidType[] ArNavList)
        {
            ArNavList = null;
            //int n, m, k, j;
            //double fDist;
            //ILSType ILS = new ILSType();

            //n = GlobalVars.NavaidList.Length;
            //m = GlobalVars.RWYList.Length;

            //ArNavList = new NavaidType[n + m];

            //j = -1;

            //for (int i = 0; i < m; i++)
            //{
            //    if (GlobalVars.RWYList[i].Selected)
            //    {
            //        k = (int)DBModule.GetILS(GlobalVars.RWYList[i], ref ILS, GlobalVars.CurrADHP);
            //        if (k > 0)
            //        {
            //            j = j + 1;
            //            ArNavList[j].pPtGeo = ILS.pPtGeo;
            //            ArNavList[j].pPtPrj = ILS.pPtPrj;

            //            ArNavList[j].Name = ILS.CallSign; //ILS.RWY_ID
            //            ArNavList[j].CallSign = ILS.CallSign;
            //            ArNavList[j].Identifier = ILS.Identifier;

            //            ArNavList[j].MagVar = ILS.MagVar;
            //            ArNavList[j].TypeCode = eNavaidType.LLZ;

            //            ArNavList[j].Range = 70000.0; //LLZData.Range
            //            ArNavList[j].index = ILS.index;
            //            ArNavList[j].PairNavaidIndex = -1;

            //            ArNavList[j].GPAngle = ILS.GPAngle;
            //            ArNavList[j].GP_RDH = ILS.GP_RDH;

            //            ArNavList[j].Course = ILS.Course;
            //            ArNavList[j].LLZ_THR = ILS.LLZ_THR;
            //            ArNavList[j].SecWidth = ILS.SecWidth;
            //            ArNavList[j].AngleWidth = ILS.AngleWidth;
            //            ArNavList[j].pFeature = ILS.pFeature;
            //        }
            //    }
            //}

            //if (n == 0 && j < 0)
            //{
            //    ArNavList = null;
            //    return 0;
            //}

            //for (int i = 0; i < n; i++)
            //{
            //    fDist = ARANFunctions.ReturnDistanceInMeters(GlobalVars.NavaidList[i].pPtPrj, pCentroid);
            //    if (fDist < GlobalVars.ArPANSOPS_MaxNavDist)
            //    {
            //        j = j + 1;
            //        ArNavList[j] = GlobalVars.NavaidList[i];
            //    }
            //}

            //if (j >= 0)
            //{
            //    Array.Resize(ref ArNavList, j + 1);
            //}
            //else
            //{
            //    ArNavList = null;
            //}

            //return j + 1;
            return 0;
        }

        public void NavInSector(NavaidType ForNavaid, out InSectorNav[] OutList, ref MultiPoint pSector, double fLow, Aran.Geometries.Point RWYTHRPrj, double m_ArDir)
        {
            OutList = null;
            //int n;
            //double tmpDist;
            //double tmpDir;
            //double fTmp;
            //double d0;

            //n = GlobalVars.NavaidList.Length;

            //if (pSector.IsEmpty)
            //{
            //    OutList = null;
            //    return;
            //}
            //else
            //    OutList = new InSectorNav[n];

            //GeometryOperators geomOper = new GeometryOperators();
            //geomOper.CurrentGeometry = pSector;

            //int j = -1;
            //for (int i = 0; i < n; i++)
            //{
            //    tmpDist = ARANFunctions.ReturnDistanceInMeters(ForNavaid.pPtPrj, GlobalVars.NavaidList[i].pPtPrj);
            //    if (tmpDist <= GlobalVars.distEps)
            //        continue;
            //    if (geomOper.GetDistance(GlobalVars.NavaidList[i].pPtPrj) > 0.0)
            //        continue;

            //    if (GlobalVars.NavaidList[i].TypeCode == eNavaidType.VOR || GlobalVars.NavaidList[i].TypeCode == eNavaidType.NDB || GlobalVars.NavaidList[i].TypeCode == eNavaidType.CodeTACAN)
            //    {
            //        j++;
            //        OutList[j] = Functions.NavaidType2InSectorNav(GlobalVars.NavaidList[i]);
            //        OutList[j].Distance = ARANFunctions.Point2LineDistancePrj(OutList[j].pPtPrj, RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0)) * (-1) * (int)ARANMath.SideDef(RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0), OutList[j].pPtPrj);
            //        d0 = GlobalVars.constants.NavaidConstants.OnNAVShift(OutList[j].TypeCode, 150.0, GlobalVars.constants);

            //        tmpDir = ARANFunctions.ReturnAngleInDegrees(ForNavaid.pPtPrj, OutList[j].pPtPrj);
            //        if (ARANMath.SubtractAnglesInDegs(tmpDir, ARANMath.RadToDeg(m_ArDir)) > 90.0)
            //            tmpDir += 180.0;

            //        fTmp = ARANMath.RadToDeg(Functions.ArcSin(d0 / tmpDist));
            //        OutList[j].FromAngle = ARANMath.Modulus(tmpDir - fTmp, 360.0);
            //        OutList[j].ToAngle = ARANMath.Modulus(tmpDir + fTmp, 360.0);
            //        OutList[j].Tag = 0;
            //    }
            //}

            //j++;
            //OutList[j] = Functions.NavaidType2InSectorNav(ForNavaid);
            //OutList[j].Distance = ARANFunctions.Point2LineDistancePrj(OutList[j].pPtPrj, RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0)) * (-1) * (int)ARANMath.SideDef(RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0), OutList[j].pPtPrj);
            //d0 = GlobalVars.constants.NavaidConstants.OnNAVShift(OutList[j].TypeCode, 150.0, GlobalVars.constants);

            //OutList[j].FromAngle = 0;
            //OutList[j].ToAngle = 360;
            //OutList[j].Tag = 1;

            //if (j < 0)
            //{
            //    OutList = new InSectorNav[0];
            //}
            //else
            //    Array.Resize(ref OutList, j);
        }

        public void AzimuthChanged(double NewAzt, Aran.Geometries.Point RWYTHRPrj, ref double m_ArDir, double Ss, double Vs, ref string TrueBRGRangeText)
        {
            //double FA_Range;
            //double fDist;
            //double MaxDh;
            //double fTmp;
            //double K;

            //Polygon pPolyLeft = new Polygon();
            //Polygon pPolyRight = new Polygon();
            //Polygon pPolyPrime = new Polygon();

            //GeometryOperators pTopo = new GeometryOperators();
            //MultiPoint pPoints;
            //MultiLineString pLine;
            //LineString pLine1;
            //Aran.Geometries.Point pTmpPoint;
            //Aran.Geometries.Point ptTmp;
            //LineString pPath;
            //Aran.Geometries.Point FictTHR;

            //int J;
            //int N;

            //pTmpPoint = new Aran.Geometries.Point();

            //if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
            //{
            //    TrueBRGRangeText = ARANMath.RadToDeg(VMManager.Instance.FinalNavaid.pPtGeo.M).ToString();
            //    m_ArDir = VMManager.Instance.FinalNavaid.pPtPrj.M;
            //    FictTHR = (Aran.Geometries.Point)ARANFunctions.LineLineIntersect(RWYTHRPrj, VMManager.Instance.FinalNavaid.pPtPrj.M + ARANMath.DegToRad(90.0), VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.FinalNavaid.pPtPrj.M);
            //    FictTHR.Z = GlobalVars.CurrADHP.pPtGeo.Z;
            //}
            //else
            //{
            //    TrueBRGRangeText = ARANMath.Modulus(NewAzt, 360.0).ToString();
            //    m_ArDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, NewAzt), 2 * ARANMath.C_PI);

            //    pLine = new MultiLineString();

            //    N = GlobalVars.RWYList.Length;

            //    for (int i = 0; i < N; i += 2)
            //    {
            //        if (GlobalVars.RWYList[i].Selected)
            //        {
            //            pPath = new LineString();
            //            pPath.Add(GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR]);
            //            pPath.Add(GlobalVars.RWYList[i + 1].pPtPrj[eRWY.ptTHR]);
            //            pLine.Add(pPath);
            //        }
            //    }

            //    pLine1 = new LineString();
            //    pLine1.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir, 400000.0));
            //    pLine1.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir + ARANMath.DegToRad(180.0), 400000.0));

            //    if (pTopo.Crosses(pLine, pLine1))
            //    {
            //        pPoints = (MultiPoint)pTopo.Intersect(pLine, pLine1);
            //        MaxDh = 900000.0;
            //        J = -1;

            //        for (int i = 0; i < pPoints.Count; i++)
            //        {
            //            fDist = ARANFunctions.ReturnDistanceInMeters(pLine1[pLine1.Count - 1], pPoints[i]);
            //            if (fDist < MaxDh)
            //            {
            //                J = i;
            //                MaxDh = fDist;
            //            }
            //        }

            //        if (J >= 0)
            //            FictTHR = pPoints[J];
            //        else if (pPoints.Count > 0)
            //            FictTHR = pPoints[0];
            //        else
            //            FictTHR = new Aran.Geometries.Point();
            //    }
            //    else
            //    {
            //        pPoints = pLine.ToMultiPoint();
            //        MaxDh = 90000.0;
            //        J = -1;
            //        for (int i = 0; i < pPoints.Count; i++)
            //        {
            //            fDist = ARANFunctions.Point2LineDistancePrj(pPoints[i], pLine1[0], ARANFunctions.ReturnAngleInRadians(pLine1[0], pLine1[pLine1.Count - 1]));
            //            fTmp = GlobalVars.pspatialReferenceOperation.AztToDirGeo(GlobalVars.CurrADHP.pPtGeo, ARANMath.RadToDeg(pPoints[i].M));
            //            if (fDist < MaxDh && ARANMath.SubtractAngles(fTmp + ARANMath.DegToRad(180.0), m_ArDir) <= ARANMath.DegToRad(90.0))
            //            {
            //                J = i;
            //                MaxDh = fDist;
            //            }
            //        }

            //        FictTHR = new Aran.Geometries.Point();
            //        if (J >= 0)
            //        {
            //            ptTmp = pPoints[J];
            //            FictTHR = (Aran.Geometries.Point)ARANFunctions.LineLineIntersect(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir, ptTmp, m_ArDir + ARANMath.DegToRad(90.0));
            //        }
            //        else
            //            FictTHR.SetCoords(pPoints[0].X, pPoints[0].Y);
            //    }
            //    FictTHR.Z = GlobalVars.CurrADHP.pPtGeo.Z;
            //}

            //FA_Range = 0;
            //if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.VOR)
            //    FA_Range = GlobalVars.constants.NavaidConstants.VOR.FA_Range;
            //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.NDB)
            //    FA_Range = GlobalVars.constants.NavaidConstants.NDB.FA_Range;
            //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
            //    FA_Range = GlobalVars.constants.NavaidConstants.LLZ.Range;

            //K = 2 * FA_Range;

            //pLine = new MultiLineString();
            //LineString tmp = new LineString();
            //tmp.Add(VMManager.Instance.FinalNavaid.pPtPrj);
            //tmp.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir + ARANMath.DegToRad(180.0), FA_Range));
            //pLine.Add(tmp);

            ////Functions.CreateNavaidZone(VMManager.Instance.FinalNavaid, m_ArDir, FictTHR, Ss, Vs, FA_Range, K, ref pPolyLeft, ref pPolyRight, ref pPolyPrime);

            //string[] aStr = new string[3];
            //aStr[0] = Properties.Resources.str10315;
            //aStr[1] = "";
            //aStr[2] = Properties.Resources.str10314;

            //if (VMManager.Instance.FASegmElement >= 0) GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.FASegmElement);
            ////if (VMManager.Instance.LeftPolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LeftPolyElement);
            ////if (VMManager.Instance.RightPolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RightPolyElement);
            ////if (VMManager.Instance.PrimePolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.PrimePolyElement);

            ////VMManager.Instance.LeftPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyLeft, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsNull);

            ////VMManager.Instance.RightPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyRight, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsNull);

            ////VMManager.Instance.PrimePolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyPrime, 0, AranEnvironment.Symbols.eFillStyle.sfsNull);

            //VMManager.Instance.FASegmElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(pLine, ARANFunctions.RGB(0, 0, 255), 1);
        }


        public List<NavaidType> GetUsableNavaids(DesignatedPoint dp, NavaidType[] ArNavList, MultiPoint RWYCollection, Point m_pCentroid)
        {
            //List<NavaidType> resultList = new List<NavaidType>();
            //bool bSolution;
            //LowHigh[] Solutions;
            //GeometryOperators TopoOper = new GeometryOperators();

            //LineString pRWYLine;

            //bool OnAero;

            //double Rad1 = 0;
            //double Rad2 = 0;
            //double Rad3 = 0;
            //double Rad4 = 0;
            //double fTmp1;
            //double fTmp;

            ////===========Constants==================
            //double MinDistStraightInApproach;
            //double MaxDistStraightInApproach;
            //double OnAeroRange;
            //double TolerDist;
            //double Theta1;
            //double Theta2;

            //double dRad;
            //double dRad1;
            //double dRad2;
            //double dRad3;
            //double dRad4;

            //double Dist;
            //double Dist1;

            //double RadToAirport;
            //int Side1;

            ////System.Windows.Forms.ListViewItem itmX;
            //string ItemStr;

            //Point RWYTHRPrj = VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR];

            //for (int K = 0; K < ArNavList.Length; K++)
            //{
            //    VMManager.Instance.FinalNavaid = ArNavList[K];

            //    if (VMManager.Instance.FinalNavaid.TypeCode != eNavaidType.LLZ)
            //        VMManager.Instance.FinalNavaid.GP_RDH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

            //    //===========================================================================

            //    OnAeroRange = GlobalVars.constants.Pansops[ePANSOPSData.arCirclAprShift].Value;
            //    TolerDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;
            //    Theta2 = GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value;
            //    Theta1 = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[VMManager.Instance.Category];

            //    MinDistStraightInApproach = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value;
            //    MaxDistStraightInApproach = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value +
            //        GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value /
            //            System.Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value); //arFAFLenght.Value

            //    //lstVw_Solutions.Items.Clear();

            //    pRWYLine = new LineString();


            //    //========== Проверка приаэродромного средства =====

            //    if (RWYCollection.Count < 3) //Расстояние РНС от порога ВПП :
            //    {
            //        Dist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, RWYTHRPrj);

            //        //txtBox_ShortestFacARPDist.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist, eRoundMode.NERAEST).ToString();
            //        //lbl_ShortestFacARPDist.Text = Properties.Resources.str10204;  // "Ближайшее расстояние от РНС до порога ВПП :"

            //        pRWYLine.Add(RWYCollection[0]);
            //        pRWYLine.Add(RWYCollection[1]);
            //        TopoOper.CurrentGeometry = pRWYLine;
            //    }
            //    else
            //    {
            //        Dist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, m_pCentroid);

            //        //txtBox_ShortestFacARPDist.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, GlobalVars.CurrADHP.pPtPrj), eRoundMode.NERAEST).ToString();
            //        //lbl_ShortestFacARPDist.Text = Properties.Resources.str10209;  //"Ближайшее расстояние от РНС до КТА :"

            //        TopoOper.CurrentGeometry = TopoOper.ConvexHull(RWYCollection);

            //        //pRWYsPolygon = ;
            //        //TopoOper = pRWYsPolygon;
            //        //TopoOper.IsKnownSimple_2 = false;
            //        //TopoOper.Simplify();
            //        //pRoxi = pRWYsPolygon;
            //    }

            //    //if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.VOR)         //        bSolution = VOR.FA_Range > Dist
            //    //{
            //    //    if (GlobalVars.constants.NavaidConstants.VOR.FA_Range <= Dist)
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    //    else
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //    //}
            //    //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.NDB)         //        bSolution = NDB.FA_Range > Dist
            //    //{
            //    //    if (GlobalVars.constants.NavaidConstants.NDB.FA_Range <= Dist)
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    //    else
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //    //}
            //    //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)         //        bSolution = (LLZ.Range > Dist) 'And (abs(Azt2Dir(vmManager.FinalNavaid.pPtGeo, _FinalNav.Course)-)
            //    //{
            //    //    if (GlobalVars.constants.NavaidConstants.LLZ.Range <= Dist)
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    //    else
            //    //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //    //}

            //    bSolution = GlobalVars.ArPANSOPS_MaxNavDist > Dist;
            //    Dist = TopoOper.GetDistance(VMManager.Instance.FinalNavaid.pPtPrj);
            //    OnAero = Dist < OnAeroRange;

            //    //if (OnAero)
            //    //    lbl_OnOffARDFac.Text = Properties.Resources.str10210;  //"Аэродромный"
            //    //else
            //    //    lbl_OnOffARDFac.Text = Properties.Resources.str10211;  //"Не аэродромный"

            //    Solutions = null;

            //    if (!bSolution)
            //    {
            //        ItemStr = Properties.Resources.str303;
            //        ItemStr.Replace(((char)10).ToString(), " ");
            //        //Replace(ItemStr, Chr(10), " ");
            //        //itmX = lstVw_Solutions.Items.Add(ItemStr);
            //        //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ItemStr));
            //        //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ItemStr));
            //    }
            //    else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
            //    {
            //        Solutions = new LowHigh[1];
            //        Solutions[0].Low = VMManager.Instance.FinalNavaid.Course;
            //        Solutions[0].High = VMManager.Instance.FinalNavaid.Course;
            //        Solutions[0].Tag = 0;

            //        //itmX = lstVw_Solutions.Items.Add("LLZ");
            //        //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].Low + "°"));
            //        //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].High + "°"));
            //    }
            //    else //=========По кругу
            //    {
            //        if (OnAero)
            //        {
            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10221);

            //            Solutions = new LowHigh[1];
            //            Solutions[0].Low = 0.0;
            //            Solutions[0].High = 359.0;
            //            Solutions[0].Tag = 0;
            //        }
            //        else
            //        {
            //            Solutions = new LowHigh[6];
            //            RadToAirport = ARANMath.DegToRad(ARANMath.Modulus(ARANFunctions.ReturnAngleInDegrees(VMManager.Instance.FinalNavaid.pPtPrj, m_pCentroid), 360));

            //            Solutions[0].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, RadToAirport) + 0.4999);
            //            Solutions[0].High = Solutions[0].Low;
            //            Solutions[0].Tag = 0;

            //            Solutions[1].Low = ARANMath.Modulus(Solutions[0].Low + 180.0, 360.0);
            //            Solutions[1].High = Solutions[1].Low;
            //            Solutions[1].Tag = 0;

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10218);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].Low + "°"));

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10218);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[1].Low + "°"));

            //            dRad1 = 0.0;
            //            dRad2 = 0.0;
            //            dRad3 = 0.0;
            //            dRad4 = 0.0;

            //            for (int i = 0; i < RWYCollection.Count; i++)
            //            {
            //                Side1 = (int)ARANMath.SideDef(VMManager.Instance.FinalNavaid.pPtPrj, RadToAirport, RWYCollection[i]);
            //                Dist1 = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, RWYCollection[i]);

            //                fTmp1 = OnAeroRange / Dist1;
            //                fTmp = ARANMath.Modulus(System.Math.Atan(fTmp1 / System.Math.Sqrt(-fTmp1 * fTmp1 + 1)), 2 * ARANMath.C_PI);
            //                fTmp1 = ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(VMManager.Instance.FinalNavaid.pPtPrj, RWYCollection[i]), 2 * ARANMath.C_PI);

            //                if (Side1 == (int)SideDirection.sideLeft)
            //                {
            //                    dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1);
            //                    if (dRad > dRad1)
            //                    {
            //                        dRad1 = dRad;
            //                        Rad1 = fTmp1;
            //                    }
            //                }
            //                else
            //                {
            //                    dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1);
            //                    if (dRad > dRad2)
            //                    {
            //                        dRad2 = dRad;
            //                        Rad2 = fTmp1;
            //                    }
            //                }

            //                dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1 + fTmp);
            //                if (dRad > dRad3)
            //                {
            //                    dRad3 = dRad;
            //                    Rad3 = fTmp1 + fTmp;
            //                }

            //                dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1 - fTmp);
            //                if (dRad > dRad4)
            //                {
            //                    dRad4 = dRad;
            //                    Rad4 = fTmp1 - fTmp;
            //                }
            //            }

            //            Solutions[2].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad1) + 0.4999);
            //            Solutions[2].High = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad2) - 0.4999);
            //            if (Solutions[2].Low > Solutions[2].High)
            //            {
            //                Solutions[2].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad1));
            //                Solutions[2].High = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad2));
            //            }
            //            Solutions[2].Tag = 1;

            //            Solutions[3].Low = ARANMath.Modulus(Solutions[2].Low + 180.0, 360.0);
            //            Solutions[3].High = ARANMath.Modulus(Solutions[2].High + 180.0, 360.0);
            //            Solutions[3].Tag = 1;

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10219);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[2].Low + "°"));
            //            //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[2].High + "°"));

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10219);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[3].Low + "°"));
            //            //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[3].High + "°"));


            //            Rad1 = GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad3);
            //            Rad2 = GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad4);

            //            Rad3 = ARANMath.Modulus(Rad1 + 180.0, 360.0);
            //            Rad4 = ARANMath.Modulus(Rad2 + 180.0, 360.0);

            //            Solutions[4].Low = System.Math.Round(Rad1 + 0.4999);
            //            Solutions[4].High = System.Math.Round(Rad2 - 0.4999);
            //            Solutions[4].Tag = 2;

            //            Solutions[5].Low = System.Math.Round(Rad3 + 0.4999);
            //            Solutions[5].High = System.Math.Round(Rad4 - 0.4999);
            //            Solutions[5].Tag = 2;

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10220);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[4].Low + "°"));
            //            //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[4].High + "°"));

            //            //itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10220);
            //            //itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[5].Low + "°"));
            //            //itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[5].High + "°"));
            //        }
            //    }

            //    if (VMManager.Instance.FinalNavaid.TypeCode != eNavaidType.VOR)
            //        continue;

            //    double direction = ARANFunctions.ReturnAngleInRadians(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(dp.Location.Geo), ArNavList[K].pPtPrj);
            //    double aztDirection = GlobalVars.pspatialReferenceOperation.DirToAztPrj(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(dp.Location.Geo), direction);

            //    for (int i = 0; i < Solutions.Length; i++)
            //    {
            //        if (aztDirection >= Solutions[i].Low && aztDirection <= Solutions[i].High)
            //        {
            //            resultList.Add(ArNavList[K]);
            //            break;
            //        }
            //    }
            //}
            //return resultList;
            return null;
        }

        public void getInitialPositionAndDirection()
        {
            //VMManager.Instance.InitialDirection = VMManager.Instance.TrueBRGAngle;
            //if (VMManager.Instance.InitialDirection < 0)
            //    VMManager.Instance.InitialDirection = ARANMath.C_2xPI + VMManager.Instance.InitialDirection;
            //double _distToPoly;
            //Line line;
            //GeometryOperators geomOper = new GeometryOperators();
            //geomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
            //if (geomOper.Contains(VMManager.Instance.FinalNavaid.pPtPrj))
            //{
            //    VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], VMManager.Instance.FinalNavaid.pPtPrj,
            //            ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI, ARANMath.C_2xPI), out _distToPoly, true);
            //}
            //else
            //{
            //    line = new Line(VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.InitialDirection);
            //    VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], line, out _distToPoly, true);
            //    if (VMManager.Instance.InitialPosition == null)
            //    {
            //        VMManager.Instance.InitialDirection = ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI, ARANMath.C_2xPI);
            //        line = new Line(VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.InitialDirection);
            //        VMManager.Instance.InitialPosition = ARANFunctions.PolygonVectorIntersect(VMManager.Instance.ConvexPoly[0], line, out _distToPoly, true);
            //    }
            //}
        }

        public void constructDivergenceVFSelectionPolygon(Point initialPoint, double dist)
        {
            VMManager.Instance.DivergenceVFSelectionPoly = new Polygon();
            double dir = ARANMath.Modulus(VMManager.Instance.TrackInitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI);
            Point pnt = ARANFunctions.PointAlongPlane(initialPoint, dir, VMManager.Instance.VisibilityDistance);
            VMManager.Instance.DivergenceVFSelectionPoly.ExteriorRing.Add(pnt);

            pnt = ARANFunctions.PointAlongPlane(pnt, VMManager.Instance.TrackInitialDirection, dist);
            VMManager.Instance.DivergenceVFSelectionPoly.ExteriorRing.Add(pnt);

            dir = ARANMath.Modulus(VMManager.Instance.TrackInitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI);
            pnt = ARANFunctions.PointAlongPlane(pnt, dir, 2 * VMManager.Instance.VisibilityDistance);
            VMManager.Instance.DivergenceVFSelectionPoly.ExteriorRing.Add(pnt);

            dir = ARANMath.Modulus(VMManager.Instance.TrackInitialDirection - ARANMath.C_PI, ARANMath.C_2xPI);
            pnt = ARANFunctions.PointAlongPlane(pnt, dir, dist);
            VMManager.Instance.DivergenceVFSelectionPoly.ExteriorRing.Add(pnt);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement);

            VMManager.Instance.DivergenceVFSelectionPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(VMManager.Instance.DivergenceVFSelectionPoly, ARANFunctions.RGB(0, 0, 0), AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);


            VMManager.Instance.DivergenceVFList.Clear();
            findVFsWithinDivergenceVFSelectionPoly();
        }

        public void findVFsWithinDivergenceVFSelectionPoly()
        {
            VMManager.Instance.DivergenceVFList.Clear();
            VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.DivergenceVFSelectionPoly;
            for (int i = 0; i < VMManager.Instance.AllVisualFeatures.Count; i++)
            {
                if (VMManager.Instance.GeomOper.Contains(VMManager.Instance.AllVisualFeatures[i].pShape))
                    VMManager.Instance.DivergenceVFList.Add(VMManager.Instance.AllVisualFeatures[i]);
            }
        }
    }
}
