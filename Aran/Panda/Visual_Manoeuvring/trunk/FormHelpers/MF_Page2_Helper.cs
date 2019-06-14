using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    class MF_Page2_Helper
    {
        public MF_Page2_Helper()
        {
        }

        public int FillArNavListForCircling(Aran.Geometries.Point pCentroid, out NavaidType[] ArNavList)
        {
            int n, m, k, j;
            double fDist;
            ILSType ILS = new ILSType();

            n = GlobalVars.NavaidList.Length;
            m = GlobalVars.RWYList.Length;

            ArNavList = new NavaidType[n + m];

            j = -1;

            for (int i = 0; i < m; i++)
			{
                if(GlobalVars.RWYList[i].Selected)
                {
                    k = (int) DBModule.GetILS(GlobalVars.RWYList[i], ref ILS, GlobalVars.CurrADHP);
                    if(k > 0)
                    {
                        j = j + 1;
                        ArNavList[j].pPtGeo = ILS.pPtGeo;
                        ArNavList[j].pPtPrj = ILS.pPtPrj;

                        ArNavList[j].Name = ILS.CallSign; //ILS.RWY_ID
                        ArNavList[j].CallSign = ILS.CallSign;
                        ArNavList[j].Identifier = ILS.Identifier;

                        ArNavList[j].MagVar = ILS.MagVar;
                        ArNavList[j].TypeCode = eNavaidType.LLZ;

                        ArNavList[j].Range = 70000.0; //LLZData.Range
                        ArNavList[j].index = ILS.index;
                        ArNavList[j].PairNavaidIndex = -1;

                        ArNavList[j].GPAngle = ILS.GPAngle;
                        ArNavList[j].GP_RDH = ILS.GP_RDH;

                        ArNavList[j].Course = ILS.Course;
                        ArNavList[j].LLZ_THR = ILS.LLZ_THR;
                        ArNavList[j].SecWidth = ILS.SecWidth;
                        ArNavList[j].AngleWidth = ILS.AngleWidth;
                        ArNavList[j].pFeature = ILS.pFeature;
                    }
                }
			}

            if (n == 0 && j < 0)
            {
                ArNavList = null;
                return 0;
            }

            for (int i = 0; i < n; i++)
            {
                fDist = ARANFunctions.ReturnDistanceInMeters(GlobalVars.NavaidList[i].pPtPrj, pCentroid);
                if (fDist < GlobalVars.ArPANSOPS_MaxNavDist)
                {
                    j = j + 1;
                    ArNavList[j] = GlobalVars.NavaidList[i];
                }
            }

            if (j >= 0)
            {
                Array.Resize(ref ArNavList, j + 1);
            }
            else
            {
                ArNavList = null;
            }

            return j + 1;
        }

        public void NavInSector(NavaidType ForNavaid, out InSectorNav[] OutList, ref MultiPoint pSector, double fLow, Aran.Geometries.Point RWYTHRPrj, double m_ArDir)
        {
            int n;
            double tmpDist;
            double tmpDir;
            double fTmp;
            double d0;

            n = GlobalVars.NavaidList.Length;

            if (pSector.IsEmpty)
            {
                OutList = null;
                return;
            }
            else
                OutList = new InSectorNav[n];

            GeometryOperators geomOper = new GeometryOperators();
            geomOper.CurrentGeometry = pSector;
            
            int j = -1;
            for (int i = 0; i < n; i++)
            {
                tmpDist = ARANFunctions.ReturnDistanceInMeters(ForNavaid.pPtPrj, GlobalVars.NavaidList[i].pPtPrj);
                if (tmpDist <= GlobalVars.distEps)
                    continue;
                if (geomOper.GetDistance(GlobalVars.NavaidList[i].pPtPrj) > 0.0)
                    continue;

                if (GlobalVars.NavaidList[i].TypeCode == eNavaidType.VOR || GlobalVars.NavaidList[i].TypeCode == eNavaidType.NDB || GlobalVars.NavaidList[i].TypeCode == eNavaidType.TACAN)
                {
                    j++;
                    OutList[j] = Functions.NavaidType2InSectorNav(GlobalVars.NavaidList[i]);
                    OutList[j].Distance = ARANFunctions.Point2LineDistancePrj(OutList[j].pPtPrj, RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0)) * (-1) * (int) ARANMath.SideDef(RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0), OutList[j].pPtPrj);
                    d0 = GlobalVars.constants.NavaidConstants.OnNAVShift(OutList[j].TypeCode, 150.0, GlobalVars.constants);

                    tmpDir = ARANFunctions.ReturnAngleInDegrees(ForNavaid.pPtPrj, OutList[j].pPtPrj);
                    if (ARANMath.SubtractAnglesInDegs(tmpDir, ARANMath.RadToDeg(m_ArDir)) > 90.0)
                        tmpDir += 180.0;

                    fTmp = ARANMath.RadToDeg(Functions.ArcSin(d0 / tmpDist));
                    OutList[j].FromAngle = ARANMath.Modulus(tmpDir - fTmp, 360.0);
                    OutList[j].ToAngle = ARANMath.Modulus(tmpDir + fTmp, 360.0);
                    OutList[j].Tag = 0;
                }
            }

            j++;
            OutList[j] = Functions.NavaidType2InSectorNav(ForNavaid);
            OutList[j].Distance = ARANFunctions.Point2LineDistancePrj(OutList[j].pPtPrj, RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0)) * (-1) * (int)ARANMath.SideDef(RWYTHRPrj, RWYTHRPrj.M - ARANMath.DegToRad(90.0), OutList[j].pPtPrj);
            d0 = GlobalVars.constants.NavaidConstants.OnNAVShift(OutList[j].TypeCode, 150.0, GlobalVars.constants);

            OutList[j].FromAngle = 0;
            OutList[j].ToAngle = 360;
            OutList[j].Tag = 1;

            if (j < 0)
            {
                OutList = new InSectorNav[0];
            }
            else
                Array.Resize(ref OutList, j);         
        }

        public void AzimuthChanged(double NewAzt, Aran.Geometries.Point RWYTHRPrj, ref double m_ArDir, double Ss, double Vs, ref string TrueBRGRangeText)
        {
            double FA_Range;
            double fDist;
            double MaxDh;
            double fTmp;
            double K;

            Polygon pPolyLeft = new Polygon();
            Polygon pPolyRight = new Polygon();
            Polygon pPolyPrime = new Polygon();

            GeometryOperators pTopo = new GeometryOperators();
            MultiPoint pPoints;
            MultiLineString pLine;
            LineString pLine1;
            Aran.Geometries.Point pTmpPoint;
            Aran.Geometries.Point ptTmp;
            LineString pPath;
            Aran.Geometries.Point FictTHR;

            int J;
            int N;

            pTmpPoint = new Aran.Geometries.Point();

            if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
            {
                TrueBRGRangeText = ARANMath.RadToDeg(VMManager.Instance.FinalNavaid.pPtGeo.M).ToString();
                m_ArDir = VMManager.Instance.FinalNavaid.pPtPrj.M;
                FictTHR = (Aran.Geometries.Point)ARANFunctions.LineLineIntersect(RWYTHRPrj, VMManager.Instance.FinalNavaid.pPtPrj.M + ARANMath.DegToRad(90.0), VMManager.Instance.FinalNavaid.pPtPrj, VMManager.Instance.FinalNavaid.pPtPrj.M);
                FictTHR.Z = GlobalVars.CurrADHP.pPtGeo.Z;
            }
            else
            {
                TrueBRGRangeText = ARANMath.Modulus(NewAzt, 360.0).ToString();
                m_ArDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, NewAzt), 2 * ARANMath.C_PI);

                pLine = new MultiLineString();

                N = GlobalVars.RWYList.Length;

                for (int i = 0; i < N; i += 2)
                {
                    if (GlobalVars.RWYList[i].Selected)
                    {
                        pPath = new LineString();
                        pPath.Add(GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR]);
                        pPath.Add(GlobalVars.RWYList[i + 1].pPtPrj[eRWY.ptTHR]);
                        pLine.Add(pPath);
                    }
                }

                pLine1 = new LineString();
                pLine1.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir, 400000.0));
                pLine1.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir + ARANMath.DegToRad(180.0), 400000.0));

                if (pTopo.Crosses(pLine, pLine1))
                {
                    pPoints = (MultiPoint) pTopo.Intersect(pLine, pLine1);
                    MaxDh = 900000.0;
                    J = -1;

                    for (int i = 0; i < pPoints.Count; i++)
                    {
                        fDist = ARANFunctions.ReturnDistanceInMeters(pLine1[pLine1.Count - 1], pPoints[i]);
                        if (fDist < MaxDh)
                        {
                            J = i;
                            MaxDh = fDist;
                        }
                    }

                    if (J >= 0)
                        FictTHR = pPoints[J];
                    else if (pPoints.Count > 0)
                        FictTHR = pPoints[0];
                    else
                        FictTHR = new Aran.Geometries.Point();
                }
                else
                {
                    pPoints = pLine.ToMultiPoint();
                    MaxDh = 90000.0;
                    J = -1;
                    for (int i = 0; i < pPoints.Count; i++)
                    {
                        fDist = ARANFunctions.Point2LineDistancePrj(pPoints[i], pLine1[0], ARANFunctions.ReturnAngleInRadians(pLine1[0], pLine1[pLine1.Count - 1]));
                        fTmp = GlobalVars.pspatialReferenceOperation.AztToDirGeo(GlobalVars.CurrADHP.pPtGeo, ARANMath.RadToDeg(pPoints[i].M));
                        if (fDist < MaxDh && ARANMath.SubtractAngles(fTmp + ARANMath.DegToRad(180.0), m_ArDir) <= ARANMath.DegToRad(90.0))
                        {
                            J = i;
                            MaxDh = fDist;
                        }
                    }

                    FictTHR = new Aran.Geometries.Point();
                    if (J >= 0)
                    {
                        ptTmp = pPoints[J];
                        FictTHR = (Aran.Geometries.Point)ARANFunctions.LineLineIntersect(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir, ptTmp, m_ArDir + ARANMath.DegToRad(90.0));
                    }
                    else
                        FictTHR.SetCoords(pPoints[0].X, pPoints[0].Y);
                }
                FictTHR.Z = GlobalVars.CurrADHP.pPtGeo.Z;
            }

            FA_Range = 0;
            if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.VOR)
                FA_Range = GlobalVars.constants.NavaidConstants.VOR.FA_Range;
            else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.NDB)
                FA_Range = GlobalVars.constants.NavaidConstants.NDB.FA_Range;
            else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
                FA_Range = GlobalVars.constants.NavaidConstants.LLZ.Range;

            K = 2 * FA_Range;

            pLine = new MultiLineString();
            LineString tmp = new LineString();
            tmp.Add(VMManager.Instance.FinalNavaid.pPtPrj);
            tmp.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, m_ArDir + ARANMath.DegToRad(180.0), FA_Range));
            pLine.Add(tmp);

            //Functions.CreateNavaidZone(VMManager.Instance.FinalNavaid, m_ArDir, FictTHR, Ss, Vs, FA_Range, K, ref pPolyLeft, ref pPolyRight, ref pPolyPrime);

            string[] aStr = new string[3];
            aStr[0] = Properties.Resources.str10315;
            aStr[1] = "";
            aStr[2] = Properties.Resources.str10314;

            if (VMManager.Instance.FASegmElement >= 0) GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.FASegmElement);
            //if (VMManager.Instance.LeftPolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LeftPolyElement);
            //if (VMManager.Instance.RightPolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RightPolyElement);
            //if (VMManager.Instance.PrimePolyElement >= 0) GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.PrimePolyElement);

            //VMManager.Instance.LeftPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyLeft, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsNull);

            //VMManager.Instance.RightPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyRight, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsNull);

            //VMManager.Instance.PrimePolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(pPolyPrime, 0, AranEnvironment.Symbols.eFillStyle.sfsNull);

            VMManager.Instance.FASegmElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(pLine, ARANFunctions.RGB(0, 0, 255), 1);
        }
    }
}
