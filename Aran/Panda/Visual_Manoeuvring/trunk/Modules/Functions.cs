using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Panda.Constants;
using System.Data.OleDb;
using Aran.Aim.Features;
using Aran.Converters.ConverterJtsGeom;

namespace Aran.Panda.VisualManoeuvring
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public static class Functions
    {
        public static double MaxObstacleElevationInPoly(List<VM_VerticalStructure> InObstList, out List<VM_VerticalStructure> OutObstList, MultiPolygon pPoly, out int Index, int segmentNumber = 0)
        {
            int N = -1;
            double MaxElevation = 0;

            Index = -1;
            N = InObstList.Count;

            OutObstList = new List<VM_VerticalStructure>();

            if (pPoly.IsEmpty || (N <= 0))
            {
                return 0;
            }

            VMManager.Instance.GeomOper.CurrentGeometry = pPoly;
            for (int i = 0; i < VMManager.Instance.AllObstacles.Count; i++)
            {
                //by creating a new vs vertical structure we also check whether VMManager.Instance.AllObstacles[i].VerticalStructure is within VMManager.Instance.GeomOper.CurrentGeometry
                //if it is not then vs.PartGeometries.Count will be equal to 0
                var vs = new VM_VerticalStructure(VMManager.Instance.AllObstacles[i].VerticalStructure, VMManager.Instance.AllObstacles[i].PartGeoPrjList, segmentNumber);
                if (vs.PartGeometries.Count == 0)
                    continue;

                if (MaxElevation < vs.Elevation)
                {
                    MaxElevation = vs.Elevation;
                    Index = OutObstList.Count;
                }
                OutObstList.Add(vs);           
            }
            VMManager.Instance.GeomOper = new JtsGeometryOperators();
            return MaxElevation;
        }        

        public static InSectorNav NavaidType2InSectorNav(NavaidType Val)
        {
		    InSectorNav Res = new InSectorNav();

		    Res.pPtGeo = Val.pPtGeo;
		    Res.pPtPrj = Val.pPtPrj;

		    Res.Name = Val.Name;
		    Res.Identifier = Val.Identifier;
		    Res.CallSign = Val.CallSign;
		    Res.MagVar = Val.MagVar;

		    Res.TypeCode = Val.TypeCode;
		    Res.Range = Val.Range;
		    Res.Index = (int) Val.index;
		    Res.PairNavaidIndex = (int) Val.PairNavaidIndex;

		    Res.GP_RDH = Val.GP_RDH;

		    Res.Course = Val.Course;
		    Res.LLZ_THR = Val.LLZ_THR;
		    Res.SecWidth = Val.SecWidth;
		    Res.pFeature = Val.pFeature;
		    //Res.IntersectionType = Val.IntersectionType;
		    Res.Tag = (int) Val.Tag;

            return Res;
        }

        public static double ArcSin(double X)
        {
		    if (System.Math.Abs(X) >= 1.0)
            {
			    if (X > 0.0)
				    return 0.5 * ARANMath.C_PI;
			    else
				    return -0.5 * ARANMath.C_PI;
            }
            else
			    return System.Math.Atan(X / System.Math.Sqrt(-X * X + 1.0));
        }

        public static double Max(double X, double Y)
        {
		    if (X > Y)
			    return X;
		    else
			    return Y;
        }

        public static void CreateNavaidZone(NavaidType NavFacil, double dirAngle, Point ptTHRprj, double Ss, double Vs,
	                                        double FrontLen, double BackLen, ref Polygon LPolygon, ref Polygon RPolygon, ref Polygon PrimPolygon)
        {
		    double BaseLength;
		    double ILSDir;
		    double Alpha;
		    double Betta;
		    double d0;
		    double d1;

		    Point pt0;
		    Point pt1;
		    Point pt2;
		    Point pt3;
		    Point pt4;
		    Point pt5 = new Point();            

		    D3DPolygone[] lOASPlanes = new D3DPolygone[9];

		    MultiPoint Xlf;
		    MultiPoint Xrt;
		    MultiPoint pZPlane;
		    Polygon pPlane01;
		    Polygon pPlane02;
		    //Dim pConstruct As ESRI.ArcGIS.Geometry.IConstructPoint;
		    //LineSegment pLine;
		    GeometryOperators pTopo = new GeometryOperators();

		    if (LPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < LPolygon.ExteriorRing.Count; i++)
                    LPolygon.ExteriorRing.Remove(i);
            }
            if (RPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < RPolygon.ExteriorRing.Count; i++)
                    RPolygon.ExteriorRing.Remove(i);
            }
            if (PrimPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < PrimPolygon.ExteriorRing.Count; i++)
                    PrimPolygon.ExteriorRing.Remove(i);
            }

		    if (NavFacil.TypeCode == eNavaidType.LLZ)
            {
			    ILSDir = GlobalVars.pspatialReferenceOperation.AztToDirGeo(NavFacil.pPtGeo, ARANMath.RadToDeg(NavFacil.pPtGeo.M));
			    OAS_DATABase(NavFacil.LLZ_THR, 3.0, 0.025, 1, NavFacil.GP_RDH, Ss, Vs, lOASPlanes);
			    CreateOASPlanes(ptTHRprj, ILSDir, 300.0, ref lOASPlanes, 1);

			    LPolygon.ExteriorRing.AddMultiPoint(lOASPlanes[(int) eOAS.YlPlane].Poly.ExteriorRing);
			    RPolygon.ExteriorRing.AddMultiPoint(lOASPlanes[(int) eOAS.YrPlane].Poly.ExteriorRing);

			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, ILSDir, 10.0 * FrontLen);

			    Xlf = ReArrangePolygon(lOASPlanes[ (int) eOAS.XlPlane].Poly, pt0, ILSDir);
			    Xrt = ReArrangePolygon(lOASPlanes[ (int) eOAS.XrPlane].Poly, pt0, ILSDir + ARANMath.DegToRad(180.0));

			    pt1 = new Point();
			    pt2 = new Point();
			    //pLine = new LineSegment();

			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, ILSDir + ARANMath.DegToRad(180.0), FrontLen);

			    Point pLineStart = Xlf[Xlf.Count - 1];
			    Point pLineEnd = Xlf[0];
                double pLineAngle = ARANFunctions.ReturnAngleInRadians(pLineStart, pLineEnd);

			    //pConstruct = pt1;
			    //pConstruct.ConstructAngleIntersection(pLine.Start, pLine.Angle, pt0, ARANMath.DegToRad(ILSDir + 90.0));
                pt1 = (Point)ARANFunctions.LineLineIntersect(pLineStart, pLineAngle, pt0, ILSDir + ARANMath.DegToRad(90.0));


			    pLineStart = Xrt[Xrt.Count - 1];
			    pLineEnd = Xrt[Xrt.Count - 2];

			    //pConstruct = pt2;
			    //pConstruct.ConstructAngleIntersection(pLine.End, pLine.Angle, pt0, ARANMath.DegToRad(ILSDir + 90.0));
                pt2 = (Point) ARANFunctions.LineLineIntersect(pLineEnd, pLineAngle, pt0, ILSDir + ARANMath.DegToRad(90.0));

			    pPlane01 = new Polygon();
			    pPlane01.ExteriorRing.Add(pt1);
			    pPlane01.ExteriorRing.Add(pt2);
			    pPlane01.ExteriorRing.Add(pLineStart);
			    pPlane01.ExteriorRing.Add(Xlf[Xlf.Count - 1]);

                Polygon tmpPoly = new Polygon();
                tmpPoly.ExteriorRing.AddMultiPoint(lOASPlanes[(int)eOAS.ZeroPlane].Poly.ExteriorRing);
			    pPlane02 = ((MultiPolygon) pTopo.UnionGeometry(pPlane01, tmpPoly))[0];
			    //=======================================================================
			    pZPlane = ReArrangePolygon(lOASPlanes[(int) eOAS.ZPlane].Poly, ptTHRprj, ILSDir);

			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, ILSDir, BackLen);

			    pLineStart = pZPlane[0];
			    pLineEnd = pZPlane[1];

			    //pConstruct = pt1;
			    //pConstruct.ConstructAngleIntersection(pZPlane[1], pLine.Angle - ARANMath.DegToRad(arMA_SplayAngle.Value), pt0, ARANMath.DegToRad(ILSDir - 90.0));
                pt1 = (Point)ARANFunctions.LineLineIntersect(pZPlane[1], pLineAngle - GlobalVars.constants.Pansops[ePANSOPSData.arMA_SplayAngle].Value, pt0, ILSDir - ARANMath.DegToRad(90.0));

			    pLineStart = pZPlane[3];
			    pLineEnd = pZPlane[2];

			    //pConstruct = pt2;
			    //pConstruct.ConstructAngleIntersection(pZPlane[2], pLine.Angle + ARANMath.DegToRad(arMA_SplayAngle.Value), pt0, ARANMath.DegToRad(ILSDir - 90.0));
                pt2 = (Point)ARANFunctions.LineLineIntersect(pZPlane[2], pLineAngle + GlobalVars.constants.Pansops[ePANSOPSData.arMA_SplayAngle].Value, pt0, ILSDir - ARANMath.DegToRad(90.0));


			    pPlane01 = new Polygon();

			    pPlane01.ExteriorRing.Add(pZPlane[0]);
			    pPlane01.ExteriorRing.Add(pZPlane[1]);
			    pPlane01.ExteriorRing.Add(pt1);
			    pPlane01.ExteriorRing.Add(pt2);
			    pPlane01.ExteriorRing.Add(pZPlane[2]);
			    pPlane01.ExteriorRing.Add(pZPlane[3]);
                
			    PrimPolygon.ExteriorRing = ((MultiPolygon) pTopo.UnionGeometry(pPlane01, pPlane02))[0].ExteriorRing;
            }
		    else
            {
			    if (NavFacil.TypeCode == eNavaidType.NDB)
                {
				    BaseLength = GlobalVars.constants.NavaidConstants.NDB.InitWidth * 0.5;
				    Alpha = GlobalVars.constants.NavaidConstants.NDB.SplayAngle;
                }
			    else if (NavFacil.TypeCode == eNavaidType.VOR)
                {
				    BaseLength = 0.5 * GlobalVars.constants.NavaidConstants.VOR.InitWidth;
				    Alpha = GlobalVars.constants.NavaidConstants.VOR.SplayAngle;
                }
			    else
				    return;

			    d0 = FrontLen / System.Math.Cos(Alpha);
			    d1 = BackLen / System.Math.Cos(Alpha);

			    Betta = 0.5 * System.Math.Tan(Alpha);
			    Betta = System.Math.Atan(Betta);
			    //Betta = ARANMath.RadToDeg(Betta);

			    //==========LeftPolygon
			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, dirAngle + ARANMath.DegToRad(90.0), BaseLength);
			    pt3 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, dirAngle + ARANMath.DegToRad(90.0), 0.5 * BaseLength);

			    pt1 = ARANFunctions.PointAlongPlane(pt0, dirAngle + Alpha, d1);
			    pt2 = ARANFunctions.PointAlongPlane(pt3, dirAngle + Betta, d1);

			    LPolygon.ExteriorRing.Add(pt0);
			    LPolygon.ExteriorRing.Add(pt1);
			    LPolygon.ExteriorRing.Add(pt2);
			    LPolygon.ExteriorRing.Add(pt3);

			    if (d1 > 0.0)
                {
				    pt4 = ARANFunctions.PointAlongPlane(pt3, dirAngle + ARANMath.DegToRad(180.0) - Betta, d0);
				    pt5 = ARANFunctions.PointAlongPlane(pt0, dirAngle + ARANMath.DegToRad(180.0) - Alpha, d0);
				    LPolygon.ExteriorRing.Add(pt4);
				    LPolygon.ExteriorRing.Add(pt5);
				    PrimPolygon.ExteriorRing.Add(pt4);
                }
			    PrimPolygon.ExteriorRing.Add(pt3);
			    PrimPolygon.ExteriorRing.Add(pt2);

			    //==========RightPolygon
			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, dirAngle - ARANMath.DegToRad(90.0), 0.5 * BaseLength);
			    pt3 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, dirAngle - ARANMath.DegToRad(90.0), BaseLength);
			    pt1 = ARANFunctions.PointAlongPlane(pt0, dirAngle - Betta, d1);
			    pt2 = ARANFunctions.PointAlongPlane(pt3, dirAngle - Alpha, d1);
                    
			    RPolygon.ExteriorRing.Add(pt0);
			    RPolygon.ExteriorRing.Add(pt1);
			    RPolygon.ExteriorRing.Add(pt2);
			    RPolygon.ExteriorRing.Add(pt3);

			    if (d1 > 0.0)
                {
				    pt4 = ARANFunctions.PointAlongPlane(pt3, dirAngle + ARANMath.DegToRad(180.0) + Alpha, d0);
				    pt5 = ARANFunctions.PointAlongPlane(pt0, dirAngle + ARANMath.DegToRad(180.0) + Betta, d0);
				    RPolygon.ExteriorRing.Add(pt4);
				    RPolygon.ExteriorRing.Add(pt5);
                }

			    PrimPolygon.ExteriorRing.Add(pt1);
			    PrimPolygon.ExteriorRing.Add(pt0);
			    if (d1 > 0.0) PrimPolygon.ExteriorRing.Add(pt5);
		    }
        }


        /*public static void CreateNavaidZone(NavaidType NavFacil, double dirAngle, Point ptTHRprj, double Ss, double Vs, double FrontLen, double BackLen, ref Polygon LPolygon, ref Polygon RPolygon, ref Polygon PrimPolygon) {

		    double BaseLength;
		    double ILSDir;
		    double Alpha;
		    double Betta;
		    double d0;
		    double d1;

		    Point pt0;
		    Point pt1;
		    Point pt2;
		    Point pt3;
		    Point pt4;
		    Point pt5;

            D3DPolygone[] lOASPlanes = new D3DPolygone[9];

		    MultiPoint Xlf;
		    MultiPoint Xrt;
		    MultiPoint pZPlane;
		    MultiPoint pPlane01;
		    MultiPoint pPlane02;
		    GeometryOperators pTopo = new GeometryOperators();

		    if (LPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < LPolygon.ExteriorRing.Count; i++)
                    LPolygon.ExteriorRing.Remove(i);
            }
            if (RPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < RPolygon.ExteriorRing.Count; i++)
                    RPolygon.ExteriorRing.Remove(i);
            }
            if (PrimPolygon.ExteriorRing.Count > 0)
            {
                for (int i = 0; i < PrimPolygon.ExteriorRing.Count; i++)
                    PrimPolygon.ExteriorRing.Remove(i);
            }

		    if (NavFacil.TypeCode == eNavaidType.LLZ) {
			    ILSDir = GlobalVars.pspatialReferenceOperation.AztToDirGeo(NavFacil.pPtGeo, NavFacil.pPtGeo.M);
			    OAS_DATABase(NavFacil.LLZ_THR, 3.0, 0.025, 1, NavFacil.GP_RDH, Ss, Vs, lOASPlanes);
			    CreateOASPlanes(ptTHRprj, ILSDir, 300.0, ref lOASPlanes, 1);

			    LPolygon = lOASPlanes[(int)eOAS.YlPlane].Poly;
			    RPolygon = lOASPlanes[(int)eOAS.YrPlane].Poly;

			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, ILSDir, 10.0 * FrontLen);

			    Xlf = ReArrangePolygon(lOASPlanes[(int) eOAS.XlPlane].Poly, pt0, ILSDir);
			    Xrt = ReArrangePolygon(lOASPlanes[(int) eOAS.XrPlane].Poly, pt0, ILSDir + 180.0);

			    pt1 = new Point();
			    pt2 = new Point();

			    pt0 = ARANFunctions.PointAlongPlane(NavFacil.pPtPrj, ILSDir + 180.0, FrontLen);

			    Alpha = ATan2(Xlf.Point(0).Y - Xlf.Point(Xlf.PointCount - 1).Y, Xlf.Point(0).X - Xlf.Point(Xlf.PointCount - 1).X)
			    pConstruct = pt1
			    pConstruct.ConstructAngleIntersection(Xlf.Point(0), Alpha, pt0, DegToRad(ILSDir + 90.0))

			    Betta = ATan2(Xrt.Point(Xrt.PointCount - 2).Y - Xrt.Point(Xrt.PointCount - 1).Y, Xrt.Point(Xrt.PointCount - 2).X - Xrt.Point(Xrt.PointCount - 1).X)

			    pConstruct = pt2
			    pConstruct.ConstructAngleIntersection(Xrt.Point(Xrt.PointCount - 2), Betta, pt0, DegToRad(ILSDir + 90.0))

			    pPlane01 = New ESRI.ArcGIS.Geometry.Polygon
			    pPlane01.AddPoint(pt1)
			    pPlane01.AddPoint(pt2)
			    pPlane01.AddPoint(Xrt.Point(Xrt.PointCount - 1))
			    pPlane01.AddPoint(Xlf.Point(Xlf.PointCount - 1))

			    pTopo = pPlane01
			    pTopo.IsKnownSimple_2 = False
			    pTopo.Simplify()

			    pPlane02 = pTopo.Union(lOASPlanes(eOAS.ZeroPlane).Poly)
			    '=======================================================================
			    pZPlane = ReArrangePolygon(lOASPlanes(eOAS.ZPlane).Poly, ptTHRprj, ILSDir)

			    pt0 = PointAlongPlane(NavFacil.pPtPrj, ILSDir, BackLen)

			    pConstruct = pt1
			    pConstruct.ConstructAngleIntersection(pZPlane.Point(1), DegToRad(ILSDir - arMA_SplayAngle.Value), pt0, DegToRad(ILSDir - 90.0))

			    pConstruct = pt2
			    pConstruct.ConstructAngleIntersection(pZPlane.Point(2), DegToRad(ILSDir + arMA_SplayAngle.Value), pt0, DegToRad(ILSDir - 90.0))

			    pPlane01 = New ESRI.ArcGIS.Geometry.Polygon

			    pPlane01.AddPoint(pZPlane.Point(0))
			    pPlane01.AddPoint(pZPlane.Point(1))
			    pPlane01.AddPoint(pt1)
			    pPlane01.AddPoint(pt2)
			    pPlane01.AddPoint(pZPlane.Point(2))
			    pPlane01.AddPoint(pZPlane.Point(3))

			    pTopo = pPlane01
			    pTopo.IsKnownSimple_2 = False
			    pTopo.Simplify()

			    PrimPolygon = pTopo.Union(pPlane02)
			    pTopo = PrimPolygon
			    pTopo.IsKnownSimple_2 = False
			    pTopo.Simplify()
		    Else
			    If NavFacil.TypeCode = eNavaidType.NDB Then
				    BaseLength = NDB.InitWidth * 0.5
				    Alpha = NDB.SplayAngle
			    ElseIf (NavFacil.TypeCode = eNavaidType.VOR) Or (NavFacil.TypeCode = eNavaidType.TACAN) Then
				    BaseLength = 0.5 * VOR.InitWidth
				    Alpha = VOR.SplayAngle
			    Else
				    Return
			    End If

			    d0 = FrontLen / System.Math.Cos(DegToRad(Alpha))
			    d1 = BackLen / System.Math.Cos(DegToRad(Alpha))

			    Betta = 0.5 * System.Math.Tan(DegToRad(Alpha))
			    Betta = System.Math.Atan(Betta)
			    Betta = RadToDeg(Betta)

			    '==========LeftPolygon
			    pt0 = PointAlongPlane(NavFacil.pPtPrj, dirAngle + 90.0, BaseLength)
			    pt3 = PointAlongPlane(NavFacil.pPtPrj, dirAngle + 90.0, 0.5 * BaseLength)

			    pt1 = PointAlongPlane(pt0, dirAngle + Alpha, d1)
			    pt2 = PointAlongPlane(pt3, dirAngle + Betta, d1)

			    LPolygon.AddPoint(pt0)
			    LPolygon.AddPoint(pt1)
			    LPolygon.AddPoint(pt2)
			    LPolygon.AddPoint(pt3)

			    If d1 > 0.0 Then
				    Pt4 = PointAlongPlane(pt3, dirAngle + 180.0 - Betta, d0)
				    pt5 = PointAlongPlane(pt0, dirAngle + 180.0 - Alpha, d0)
				    LPolygon.AddPoint(Pt4)
				    LPolygon.AddPoint(pt5)
				    PrimPolygon.AddPoint(Pt4)
			    End If

			    PrimPolygon.AddPoint(pt3)
			    PrimPolygon.AddPoint(pt2)

			    '==========RightPolygon
			    pt0 = PointAlongPlane(NavFacil.pPtPrj, dirAngle - 90.0, 0.5 * BaseLength)
			    pt3 = PointAlongPlane(NavFacil.pPtPrj, dirAngle - 90.0, BaseLength)
			    pt1 = PointAlongPlane(pt0, dirAngle - Betta, d1)
			    pt2 = PointAlongPlane(pt3, dirAngle - Alpha, d1)

			    RPolygon.AddPoint(pt0)
			    RPolygon.AddPoint(pt1)
			    RPolygon.AddPoint(pt2)
			    RPolygon.AddPoint(pt3)

			    If d1 > 0.0 Then
				    Pt4 = PointAlongPlane(pt3, dirAngle + 180.0 + Alpha, d0)
				    pt5 = PointAlongPlane(pt0, dirAngle + 180.0 + Betta, d0)
				    RPolygon.AddPoint(Pt4)
				    RPolygon.AddPoint(pt5)
			    End If

			    PrimPolygon.AddPoint(pt1)
			    PrimPolygon.AddPoint(pt0)

			    If d1 > 0.0 Then PrimPolygon.AddPoint(pt5)
		    End If
        }*/


















        private static void OAS_DATABase(double LLZ2THRDist, double GPAngle, double MisAprGr, int ILSCategory, double RDH, double Ss, double St, D3DPolygone[] OASPlanes)
        {
		    string sTabName;
		    string sMisAprGr;
		    string sGPAngle;
		    int CatOffset;
		    string sLLZ2THRDist;

		    double fTmp0;
		    double fTmp1;
		    double P;
		    double CwCorr;
		    double Cw_Corr;
		    double CxCorr;
		    double CyCorr;
		    double RDHCorr;

		    fTmp0 = GPAngle;
		    if (fTmp0 > GlobalVars.MaxRefGPAngle)
                fTmp0 = GlobalVars.MaxRefGPAngle;

		    sGPAngle = System.Math.Round(fTmp0 * 10.0 - 0.4999).ToString();

		    if (LLZ2THRDist < 2000.0)
                LLZ2THRDist = 2000.0;
		    if (LLZ2THRDist > 4500.0)
                LLZ2THRDist = 4500.0;
		    if (LLZ2THRDist > 4400.0)
			    sLLZ2THRDist = (System.Math.Round(0.01 * LLZ2THRDist - 0.4999) * 100).ToString();
		    else
			    sLLZ2THRDist = (System.Math.Round(0.005 * LLZ2THRDist - 0.4999) * 200).ToString();

		    sTabName = sGPAngle + "_" + sLLZ2THRDist;

		    sMisAprGr = System.Math.Round(MisAprGr * 1000.0).ToString();

		    CatOffset = 3 * (ILSCategory - 1);
            RDHCorr = RDH - GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

		    CwCorr = St - 6.0 - RDHCorr;
		    Cw_Corr = St - 6.0 - RDHCorr;

		    OASPlanes[(int) eOAS.ZeroPlane].Plane.A = 0.0;
		    OASPlanes[(int) eOAS.ZeroPlane].Plane.B = 0.0;
		    OASPlanes[(int) eOAS.ZeroPlane].Plane.C = -1.0;
		    OASPlanes[(int) eOAS.ZeroPlane].Plane.D = 0.0;

		    OASPlanes[(int) eOAS.CommonPlane].Plane.A = 0.0;
		    OASPlanes[(int) eOAS.CommonPlane].Plane.B = 0.0;
		    OASPlanes[(int) eOAS.CommonPlane].Plane.C = -1.0;
		    OASPlanes[(int) eOAS.CommonPlane].Plane.D = 300.0;

		    var conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GlobalVars.InstallDir + @"\constants\plans.mdb;Jet OLEDB:Database Password=test");
		    OleDbCommand pCommand;
		    OleDbDataReader dr;

		    conn.Open();

		    pCommand = conn.CreateCommand();
		    pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'W'";
		    dr = pCommand.ExecuteReader();

		    if (dr.Read())
            {
			    OASPlanes[(int) eOAS.WPlane].Plane.A = Convert.ToDouble(dr[1 + CatOffset]);
			    OASPlanes[(int) eOAS.WPlane].Plane.B = -Convert.ToDouble(dr[2 + CatOffset]);
			    OASPlanes[(int) eOAS.WPlane].Plane.C = -1.0;
			    OASPlanes[(int) eOAS.WPlane].Plane.D = Convert.ToDouble(dr[3 + CatOffset]) - CwCorr;

			    if (GPAngle > GlobalVars.MaxRefGPAngle)
                {
				    OASPlanes[(int) eOAS.WPlane].Plane.A = OASPlanes[(int) eOAS.WPlane].Plane.A + 0.0092 * (GPAngle - GlobalVars.MaxRefGPAngle);
				    OASPlanes[(int) eOAS.WPlane].Plane.D = -6.45 - CwCorr;
                }
            }
		    else
            {
			    dr.Close();
			    throw new Exception("Plans mdb is invalid. 'W' not found.");
            }
		    dr.Close();

		    // 'Region "X"
		    pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'X'";
            dr = pCommand.ExecuteReader();

		    if (dr.Read())
            {
			    OASPlanes[(int) eOAS.XlPlane].Plane.A = Convert.ToDouble(dr[1 + CatOffset]);
			    OASPlanes[(int) eOAS.XlPlane].Plane.B = -Convert.ToDouble(dr[2 + CatOffset]);
			    OASPlanes[(int) eOAS.XlPlane].Plane.C = -1.0;

			    fTmp0 = -St / OASPlanes[(int) eOAS.XlPlane].Plane.B;
			    fTmp1 = Ss - (St - 3) / OASPlanes[(int) eOAS.XlPlane].Plane.B;
			    CxCorr = Max(fTmp0, fTmp1);

			    fTmp0 = -6 / OASPlanes[(int) eOAS.XlPlane].Plane.B;
			    fTmp1 = 30 - 3 / OASPlanes[(int) eOAS.XlPlane].Plane.B;
			    P = CxCorr - Max(fTmp0, fTmp1);

			    CxCorr = -P * OASPlanes[(int) eOAS.XlPlane].Plane.B - RDHCorr;
			    OASPlanes[(int) eOAS.XlPlane].Plane.D = Convert.ToDouble(dr[3 + CatOffset]) - CxCorr;
            }
		    else
            {
			    dr.Close();
			    throw new Exception("Plans mdb is invalid. 'X' not found.");
            }
		    dr.Close();

		    pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'Y" + sMisAprGr + "'";
		    dr = pCommand.ExecuteReader();

		    if (dr.Read())
            {
			    OASPlanes[(int) eOAS.YlPlane].Plane.A = Convert.ToDouble(dr[1 + CatOffset]);
			    OASPlanes[(int) eOAS.YlPlane].Plane.B = -Convert.ToDouble(dr[2 + CatOffset]);
			    OASPlanes[(int) eOAS.YlPlane].Plane.C = -1.0;

			    CyCorr = -P * OASPlanes[(int) eOAS.YlPlane].Plane.B - RDHCorr;
			    OASPlanes[(int) eOAS.YlPlane].Plane.D = Convert.ToDouble(dr[3 + CatOffset]) - CyCorr;
            }
		    else
            {
			    dr.Close();
			    throw new Exception("Plans mdb is invalid. 'Y" + sMisAprGr + "'" + " not found.");
            }
		    dr.Close();

		    // 'Region "Z"

		    pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'Z" + sMisAprGr + "'";
		    dr = pCommand.ExecuteReader();

		    if (dr.Read())
            {
			    OASPlanes[(int) eOAS.ZPlane].Plane.A = Convert.ToDouble(dr[1 + CatOffset]);
			    OASPlanes[(int) eOAS.ZPlane].Plane.B = -Convert.ToDouble(dr[2 + CatOffset]);
			    OASPlanes[(int) eOAS.ZPlane].Plane.C = -1.0;
			    OASPlanes[(int) eOAS.ZPlane].Plane.D = Convert.ToDouble(dr[3 + CatOffset]);

			    if (GPAngle > GlobalVars.MaxRefGPAngle)
				    OASPlanes[(int) eOAS.ZPlane].Plane.D = OASPlanes[(int) eOAS.ZPlane].Plane.A * (GlobalVars.OASZOrigin + 500.0 * (GPAngle - GlobalVars.MaxRefGPAngle));
            }
		    else
            {
			    dr.Close();
			    throw new Exception("Plans mdb is invalid. 'Z" + sMisAprGr + "'" + " not found.");
            }
		    dr.Close();

		    OASPlanes[(int) eOAS.YrPlane].Plane.A = OASPlanes[(int) eOAS.YlPlane].Plane.A;
		    OASPlanes[(int) eOAS.YrPlane].Plane.B = -OASPlanes[(int) eOAS.YlPlane].Plane.B;
		    OASPlanes[(int) eOAS.YrPlane].Plane.C = -1.0;
		    OASPlanes[(int) eOAS.YrPlane].Plane.D = OASPlanes[(int) eOAS.YlPlane].Plane.D;

		    OASPlanes[(int) eOAS.XrPlane].Plane.A = OASPlanes[(int) eOAS.XlPlane].Plane.A;
		    OASPlanes[(int) eOAS.XrPlane].Plane.B = -OASPlanes[(int) eOAS.XlPlane].Plane.B;
		    OASPlanes[(int) eOAS.XrPlane].Plane.C = -1.0;
		    OASPlanes[(int) eOAS.XrPlane].Plane.D = OASPlanes[(int) eOAS.XlPlane].Plane.D;

		    // 'Region "W*"

		    if (ILSCategory == 3)
            {
			    pCommand.CommandText = "SELECT * FROM " + sTabName + " WHERE ID = 'W*'";
			    dr = pCommand.ExecuteReader();

			    if (dr.Read())
                {
				    OASPlanes[(int) eOAS.WsPlane].Plane.A = Convert.ToDouble(dr[1 + CatOffset]);
				    OASPlanes[(int) eOAS.WsPlane].Plane.B = -Convert.ToDouble(dr[2 + CatOffset]);
				    OASPlanes[(int) eOAS.WsPlane].Plane.C = -1.0;
				    OASPlanes[(int) eOAS.WsPlane].Plane.D = Convert.ToDouble(dr[3 + CatOffset]) - Cw_Corr;
                }
			    else
                {
				    dr.Close();
				    throw new Exception("Plans mdb is invalid. 'W*' not found.");
                }
			    dr.Close();
            }

		    conn.Close();
	    }

        private static MultiPoint ReArrangePolygon(Polygon pPolygon, Point PtDerL, double CLDir, bool bFlag = false)
        {
		    int I;
		    int J;

		    int N;
		    int iStart;

		    double dl;
		    double dm;

		    double dX0;
		    double dY0;

		    double dX1;
		    double dY1;
		    LineString pPoly;
		    Point pPt;
		    MultiPoint Result;

		    pPt = ARANFunctions.PointAlongPlane(PtDerL, CLDir + ARANMath.DegToRad(180.0), 30000.0);

		    pPoly = new LineString();
		    pPoly.AddMultiPoint(pPolygon.ExteriorRing);

		    N = pPoly.Count;

		    dm = ARANFunctions.Point2LineDistancePrj(pPoly[0], pPt, CLDir + 90.0) * (-1) * (int) ARANMath.SideDef(pPt, CLDir, pPoly[0]);

		    iStart = -1;
		    if (dm < 0) 
                iStart = 0;

		    for (I = 1; I < N; I++)
            {
			    dl = ARANFunctions.Point2LineDistancePrj(pPoly[I], pPt, CLDir + 90.0) * (-1) * (int) ARANMath.SideDef(pPt, CLDir, pPoly[I]);
			    if ((dl < 0.0) && ((dl > dm) || (dm >= 0.0)))
                {
				    dm = dl;
				    iStart = I;
                }
            }

		    if (bFlag)
            {
			    if (iStart == 0)
				    iStart = N - 1;
			    else
				    iStart = iStart - 1;
            }

		    dX0 = pPoly[1].X - pPoly[0].X;
		    dY0 = pPoly[1].Y - pPoly[0].Y;
		    I = 1;
		    while (I < N)
            {
			    J = (int) ARANMath.Modulus(I + 1, N);
			    dX1 = pPoly[J].X - pPoly[I].X;
			    dY1 = pPoly[J].Y - pPoly[I].Y;
			    dl = ARANFunctions.ReturnDistanceInMeters(pPoly[J], pPoly[I]);

			    if (dl < GlobalVars.distEps)
                {
				    pPoly.Remove(I);
				    N = N - 1;
				    J = (int) ARANMath.Modulus(I + 1, N);
				    if (I <= iStart)
                        iStart = iStart - 1;

				    dX1 = dX0;
				    dY1 = dY0;
                }
			    else if ((dY0 != 0.0) && (I != iStart))
                {
				    if (dY1 != 0.0)
                    {
					    if (System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001)
                        {
						    pPoly.Remove(I);
						    N = N - 1;
						    J = (I + 1) % N;
						    if (I <= iStart)
							    iStart = iStart - 1;
						    dX1 = dX0;
						    dY1 = dY0;
                        }
					    else
						    I = I + 1;
                    }
				    else
					    I = I + 1;
                }
			    else if ((dX0 != 0.0) && (I != iStart))
                {
				    if (dX1 != 0.0)
                    {
					    if (System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001)
                        {
						    pPoly.Remove(I);
						    N = N - 1;
						    J = (I + 1) % N;
						    if (I <= iStart)
                                iStart = iStart - 1;
						    dX1 = dX0;
						    dY1 = dY0;
                        }
					    else
						    I = I + 1;
                    }
				    else
					    I = I + 1;
                }
			    else
				    I = I + 1;
			    dX0 = dX1;
			    dY0 = dY1;
            }

		    N = pPoly.Count;
		    Result = new MultiPoint();

		    for (I = N - 1; I >= 0; I--)
            {
			    J = (int) ARANMath.Modulus(I + iStart, N);
			    Result.Add(pPoly[J]);
            }

		    return Result;
        }

        private static void CreateOASPlanes2(Point ptLHPrj, double ArDir, double hMax, ref D3DPolygone[] OASPlanes, int ILSCategory)
        {
            int I;
            int J;
            int K;
            int L;
            int N;
            int M;
            double hCons;

            LineString[] ResLine = new LineString[9];
            Point pFrmPoint;
            Point pToPoint;
            LineString pPolyline;
            GeometryOperators pTransform = new GeometryOperators();

            if (ILSCategory == 0)
                hCons = hMax;
            else if (ILSCategory == 1)
                hCons = GlobalVars.arHOASPlaneCat1;
            else
                hCons = GlobalVars.arHOASPlaneCat23;

            pPolyline = new LineString();
            pFrmPoint = new Point();
            pToPoint = new Point();

            N = OASPlanes.Length;

            for (I = 0; I < N; I++)
                OASPlanes[I].Poly = new Polygon();

            for (I = (int) eOAS.XlPlane; I <= (int) eOAS.YrPlane; I++)
            {
                J = I + 1;
                ResLine[I] = IntersectPlanes(OASPlanes[I].Plane, OASPlanes[I + 1].Plane, 0.0, hCons);

                pFrmPoint.SetCoords(ResLine[I][0].X + ptLHPrj.X, ResLine[I][0].Y + ptLHPrj.Y);
                pPolyline.Add(pFrmPoint);

                pToPoint.SetCoords(ResLine[I][ResLine[I].Count - 1].X + ptLHPrj.X, ResLine[I][ResLine[I].Count - 1].Y + ptLHPrj.Y);
                pPolyline.Add(pToPoint);

                pPolyline = ((MultiLineString) pTransform.Rotate(pPolyline, ptLHPrj, ArDir + ARANMath.DegToRad(180.0)))[0];

                OASPlanes[0].Poly.ExteriorRing.Add(pPolyline[0]);
                OASPlanes[N - 1].Poly.ExteriorRing.Add(pPolyline[pPolyline.Count - 1]);
            }

            for (I = 0; I <= 3; I++)
            {
                J = 1 + (I + 4) % 6;
                K = 1 + (I + 5) % 6;
                L = (I + 6) % 8;

                ResLine[L] = IntersectPlanes(OASPlanes[J].Plane, OASPlanes[K].Plane, 0.0, hMax);

                pFrmPoint.SetCoords(ResLine[L][0].X + ptLHPrj.X, ResLine[L][0].Y + ptLHPrj.Y);
                pPolyline[0] = pFrmPoint;

                pToPoint.SetCoords(ResLine[L][ResLine[L].Count - 1].X + ptLHPrj.X, ResLine[L][ResLine[L].Count - 1].Y + ptLHPrj.Y);
                pPolyline[1] = pToPoint;

                pPolyline = ((MultiLineString) pTransform.Rotate(pPolyline, ptLHPrj, ArDir + ARANMath.DegToRad(180.0)))[0];

                OASPlanes[0].Poly.ExteriorRing.Add(pPolyline[0]);
                OASPlanes[N - 1].Poly.ExteriorRing.Add(pPolyline[pPolyline.Count - 1]);
            }

            M = OASPlanes[0].Poly.ExteriorRing.Count;
            J = 6;
            for (I = 1; I <= 6; I++)
            {
                K = J % M;
                L = (J + M - 1) % M;

                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[0].Poly.ExteriorRing[K]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[0].Poly.ExteriorRing[L]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[N - 1].Poly.ExteriorRing[L]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[N - 1].Poly.ExteriorRing[K]);
                OASPlanes[I].Plane.pPt = OASPlanes[0].Poly.ExteriorRing[L];

                J = J + 1;
                if (J == 4) J = 5;
                if (J == 8) J = 1;
            }

            for (I = 0; I <= 8; I++)
                ResLine[I] = null;
        }

        private static void CreateOASPlanes(Point ptLHPrj, double ArDir, double hMax, ref D3DPolygone[] OASPlanes, int ILSCategory)
        {
            int I;
            int J;
            int K;
            int L;
            int N;
            int M;
            double hCons;

            LineString[] ResLine = new LineString[9];
            Point pFrmPoint;
            Point pToPoint;
            LineString pPolyline;
            GeometryOperators pTopo = new GeometryOperators();

            if (ILSCategory == 0)
                hCons = hMax;
            else if (ILSCategory == 1)
                hCons = GlobalVars.arHOASPlaneCat1;
            else
                hCons = GlobalVars.arHOASPlaneCat23;

            pPolyline = new LineString();
            pFrmPoint = new Point();
            pToPoint = new Point();

            N = OASPlanes.Length;

            for (I = 0; I < N; I++)
                OASPlanes[I].Poly = new Polygon();

            for (I = (int)eOAS.XlPlane; I <= (int)eOAS.YrPlane; I++)
            {
                J = I + 1;
                ResLine[I] = IntersectPlanes(OASPlanes[I].Plane, OASPlanes[I + 1].Plane, 0.0, hCons);

                pFrmPoint.SetCoords(ResLine[I][0].X + ptLHPrj.X, ResLine[I][0].Y + ptLHPrj.Y);
                pPolyline.Add(pFrmPoint);

                pToPoint.SetCoords(ResLine[I][ResLine[I].Count - 1].X + ptLHPrj.X, ResLine[I][ResLine[I].Count - 1].Y + ptLHPrj.Y);
                pPolyline.Add(pToPoint);

                pPolyline = ((MultiLineString)pTopo.Rotate(pPolyline, ptLHPrj, ArDir + ARANMath.DegToRad(180.0)))[0];

                OASPlanes[0].Poly.ExteriorRing.Add(pPolyline[0]);
                OASPlanes[N - 1].Poly.ExteriorRing.Add(pPolyline[pPolyline.Count - 1]);

                
            }
            //AranEnvironment.Symbols.FillSymbol symb = new AranEnvironment.Symbols.FillSymbol();
            //symb.Style = AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal;
            //symb.Color = ARANFunctions.RGB(255, 0, 0);
            //GlobalVars.gAranEnv.Graphics.DrawPolygon(OASPlanes[0].Poly, symb);
            //symb.Style = AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal;
            //symb.Color = ARANFunctions.RGB(0, 0, 255);
            //GlobalVars.gAranEnv.Graphics.DrawPolygon(OASPlanes[N-1].Poly, symb);
            //Leg.ProcessMessages(true);
            for (I = 0; I <= 3; I++)
            {
                J = 1 + (I + 4) % 6;
                K = 1 + (I + 5) % 6;
                L = (I + 6) % 8;

                ResLine[L] = IntersectPlanes(OASPlanes[J].Plane, OASPlanes[K].Plane, 0.0, hMax);

                pFrmPoint.SetCoords(ResLine[L][0].X + ptLHPrj.X, ResLine[L][0].Y + ptLHPrj.Y);
                pPolyline[0] = pFrmPoint;

                pToPoint.SetCoords(ResLine[L][ResLine[L].Count - 1].X + ptLHPrj.X, ResLine[L][ResLine[L].Count - 1].Y + ptLHPrj.Y);
                pPolyline[1] = pToPoint;

                pPolyline = ((MultiLineString)pTopo.Rotate(pPolyline, ptLHPrj, ArDir + ARANMath.DegToRad(180.0)))[0];


                OASPlanes[0].Poly.ExteriorRing.Add(pPolyline[0]);
                OASPlanes[N - 1].Poly.ExteriorRing.Add(pPolyline[pPolyline.Count - 1]);
            }

            M = OASPlanes[0].Poly.ExteriorRing.Count;
            J = 6;
            for (I = 1; I <= 6; I++)
            {
                K = J % M;
                L = (J + M - 1) % M;

                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[0].Poly.ExteriorRing[K]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[0].Poly.ExteriorRing[L]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[N - 1].Poly.ExteriorRing[L]);
                OASPlanes[I].Poly.ExteriorRing.Add(OASPlanes[N - 1].Poly.ExteriorRing[K]);
                OASPlanes[I].Plane.pPt = OASPlanes[0].Poly.ExteriorRing[L];

                J = J + 1;
                if (J == 4) J = 5;
                if (J == 8) J = 1;
            }

            for (I = 0; I <= 8; I++)
                ResLine[I] = null;
        }       

        public static LineString IntersectPlanes(D3DPlane PlaneA, D3DPlane PlaneB, double hMin, double hMax)
        {
		    double D;
		    double dX;
		    double dY;
		    Point pt0;
		    Point pt1;

		    LineString result = new LineString();

		    D = Det(PlaneA.A, PlaneA.B, PlaneB.A, PlaneB.B);
		    if (D == 0.0) return result;

		    dX = Det(-(PlaneA.D + PlaneA.C * hMin), PlaneA.B, -(PlaneB.D + PlaneB.C * hMin), PlaneB.B);
		    dY = Det(PlaneA.A, -(PlaneA.D + PlaneA.C * hMin), PlaneB.A, -(PlaneB.D + PlaneB.C * hMin));
		    pt0 = new Point();
		    pt0.X = dX / D;
		    pt0.Y = dY / D;

		    dX = Det(-(PlaneA.D + PlaneA.C * hMax), PlaneA.B, -(PlaneB.D + PlaneB.C * hMax), PlaneB.B);
		    dY = Det(PlaneA.A, -(PlaneA.D + PlaneA.C * hMax), PlaneB.A, -(PlaneB.D + PlaneB.C * hMax));
		    pt1 = new Point();
		    pt1.X = dX / D;
		    pt1.Y = dY / D;

		    result.Add(pt0);
            result.Add(pt1);
            return result;
        }

        public static double Det(double X0, double Y0, double X1, double Y1)
        {
            return X0 * Y1 - X1 * Y0;
        }

        public static void TextBoxFloat(ref char KeyChar, string BoxText)
        {
            if (KeyChar < 32)
                return;

            char DecSep = (1.1).ToString().ToCharArray()[1];

            if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
                KeyChar = '\0';
            else if (KeyChar == DecSep)
            {
                if (BoxText.Contains(DecSep.ToString()))
                    KeyChar = '\0';
            }
        }

        public static void TextBoxInteger(ref char KeyChar)
        {
            if (KeyChar < ' ')
                return;
            if ((KeyChar < '0') || (KeyChar > '9'))
                KeyChar = '\0';
        }

        public static void DD2DMS(double val, out double xDeg, out double xMin, out double xSec, int Sign)
        {
            double x;
            double dx;

            x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

            xDeg = Fix(x);
            dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);
        }

        public static double DMS2DD(double xDeg, double xMin, double xSec, int Sign)
        {
            double x;
            x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
            return System.Math.Abs(x);
        }

        private static int Fix(double x)
        {
            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }

        public static MultiPoint CalcTrajectoryFromMultiPoint(MultiPoint multiPoint)
        {
            MultiPoint calcTrajectoryFromMultiPointReturn = null;
            Point FromPt = null;
            Point CntPt = null;
            Point ToPt = null;

            double fTmp = 0;
            double fE = 0;

            SideDirection Side = 0;
            int I = 0;
            int N = 0;

            CntPt = new Point();
            calcTrajectoryFromMultiPointReturn = new MultiPoint();
            fE = GlobalVars.DegToRadValue * 0.5;

            N = multiPoint.Count - 2;

            calcTrajectoryFromMultiPointReturn.Add(multiPoint[0]);

            for (I = 0; I <= N; I++)
            {
                FromPt = multiPoint[I];
                ToPt = multiPoint[I + 1];
                fTmp = (FromPt.M - ToPt.M);
                if ((System.Math.Abs(System.Math.Sin(fTmp)) <= fE) && (System.Math.Cos(fTmp) > 0.0))
                {
                    calcTrajectoryFromMultiPointReturn.Add(ToPt);
                }
                else
                {
                    if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
                    {
                        CntPt = (Point)ARANFunctions.LineLineIntersect(FromPt, FromPt.M + ARANMath.C_PI_2, ToPt, ToPt.M + ARANMath.C_PI_2);
                    }
                    else
                    {
                        CntPt.SetCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));
                    }
                    Side = ARANMath.SideDef(FromPt, FromPt.M, ToPt);
                    TurnDirection turnDirection = (TurnDirection)((int)Side);
                    calcTrajectoryFromMultiPointReturn.AddMultiPoint(ARANFunctions.CreateArcPrj(CntPt, FromPt, ToPt, turnDirection));
                }
            }
            return calcTrajectoryFromMultiPointReturn;
        }

        public static int GetObstaclesInPoly(ObstacleType[] InObstList, out ObstacleMSA[] OutObstList, double RefHeight, Polygon pPoly)
        {
            GeometryOperators operators = new Aran.Geometries.Operators.GeometryOperators();
            operators.CurrentGeometry = pPoly;
            int N = InObstList.Length;

            if (pPoly.IsEmpty || (N == 0))
            {
                OutObstList = new ObstacleMSA[0];
                return 0;
            }

            OutObstList = new ObstacleMSA[N];

            int j = -1;

            for (int i = 0; i < N; i++)
            {
                if (operators.GetDistance(InObstList[i].pPtPrj) == 0)
                {
                    j = j + 1;
                    OutObstList[j] = new ObstacleMSA();
                    OutObstList[j].pPtGeo = InObstList[i].pPtGeo;
                    OutObstList[j].pPtPrj = InObstList[i].pPtPrj;
                    OutObstList[j].ID = InObstList[i].ID;
                    OutObstList[j].Name = InObstList[i].Name;

                    OutObstList[j].Height = OutObstList[j].pPtGeo.Z - RefHeight;
                    OutObstList[j].Index = InObstList[i].Index;
                }
            }

            if (j >= 0)
            {
                Array.Resize(ref OutObstList, j + 1);
            }
            else
            {
                OutObstList = new ObstacleMSA[0];
            }

            return N;
        }
    }
}
