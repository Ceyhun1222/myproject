using System;
using Aran.Geometries;
using Aran.PANDA.Common;
using Holding;

namespace Holding
{
	public static class Shablons
	{
		public const double degToRadValue = ARANMath.C_PI / 180;
		public const double degEps = 1.0 / 36000.0;
		public const double radEps = degEps * degToRadValue;
		public const double distEps = 0.0001;
		public static double guidanceTolerance = ARANMath.DegToRad(5);
		public static double sinGuidanceTolerance = Math.Sin(guidanceTolerance);
		public static double cosGuidanceTolerance = Math.Cos(guidanceTolerance);

		public static double TouchTurn180InRad { get; set; }
        
        
        public static int HoldingShablon(Point wayPoint, double ias, double altitude, double dIsa, double time, double axis, SideDirection side, int navType, out LineString inboundTrack, out Polygon shablon, out LineString line3, out LineString turn180, out Point ptE)
        {
            TurnDirection turnDir = (TurnDirection)side;

            double V, v3600, R, Rv, H, w, w_, E45, E, t_, L,
                 ab, ac, gi1, gi2, xe, ye;

            double Wb, Wc, Wd, We, Wf, Wg, Wh, Wo, Wp, Wi1,
                Wi3, Wi2, Wi4, Wj, Wk, Wl, Wm, Wn3, Wn4;

            double tmpD, tmpDirection, alpha;

            V = ARANMath.IASToTASForRnav(ias, altitude,0);
            v3600 = V;
            R = 943.27 * 0.277777777777778 / V;
            if ((R > 3.0))
                R = 3.0;

            Rv = 3600 * V / (62.83 * R);
            H = altitude / 1000.0;
            w = 12.0 * H + 87.0;

            w_ = 0.277777777777778 * w;

            E = w_ / R;
            E45 = 45.0 * E;
            t_ = 60.0 * time;

            L = v3600 * t_;
            ab = 5.0 * v3600;
            ac = 11.0 * v3600;
            gi1 = (t_ - 5.0) * v3600;
            gi2 = (t_ + 21.0) * v3600;

            Wb = 5 * w_;
            Wc = 11 * w_;
            Wd = Wc + E45;
            We = Wc + 2 * E45;
            Wf = Wc + 3 * E45;
            Wg = Wc + 4 * E45;
            Wh = Wb + 4 * E45;
            Wo = Wb + 5 * E45;
            Wp = Wb + 6 * E45;
            Wi1 = (t_ + 6) * w_ + 4 * E45;
            Wi3 = Wi1;
            Wi2 = Wi1 + 14 * w_;
            Wi4 = Wi2;
            Wj = Wi2 + E45;
            Wk = Wi2 + 2 * E45;
            Wl = Wk;
            Wm = Wi2 + 3 * E45;
            Wn3 = Wi1 + 4 * E45;
            Wn4 = Wi2 + 4 * E45;

            xe = 2.0 * Rv + (t_ + 15.0) * v3600 + (26.0 + 195.0 / R + t_) * w_;
            ye = 11.0 * v3600 * System.Math.Cos(ARANMath.DegToRad(20.0)) + Rv * System.Math.Sin(ARANMath.DegToRad(20.0)) + Rv + (t_ + 15.0) * v3600 * System.Math.Tan(ARANMath.DegToRad(5.0)) + (21.0 + 125.0 / R + t_) * w_;

            Point pntCntB = ARANFunctions.LocalToPrj(wayPoint, axis, -ab, -Rv * (int)turnDir);
            Point pntCntC = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * (int)turnDir);
            Point pntG = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * 2 * (int)turnDir);

            Point pntI1 = ARANFunctions.LocalToPrj(pntG, axis, gi1 * cosGuidanceTolerance, -gi1 * sinGuidanceTolerance * (int)turnDir);
            Point pntI2 = ARANFunctions.LocalToPrj(pntG, axis, gi2 * cosGuidanceTolerance, -gi2 * sinGuidanceTolerance * (int)turnDir);
            Point pntI3 = ARANFunctions.LocalToPrj(pntG, axis, gi1 * cosGuidanceTolerance, gi1 * sinGuidanceTolerance * (int)turnDir);
            Point pntI4 = ARANFunctions.LocalToPrj(pntG, axis, gi2 * cosGuidanceTolerance, gi2 * sinGuidanceTolerance * (int)turnDir);

            Point pntCntI1 = ARANFunctions.LocalToPrj(pntI1, axis, 0, (int)turnDir * Rv);
            Point pntCntI2 = ARANFunctions.LocalToPrj(pntI2, axis, 0, (int)turnDir * Rv);
            Point pntCntI3 = ARANFunctions.LocalToPrj(pntI3, axis, 0, (int)turnDir * Rv);
            Point pntCntI4 = ARANFunctions.LocalToPrj(pntI4, axis, 0, (int)turnDir * Rv);


            double quarterPI = ARANMath.C_PI * 0.25;
            Point pntJ = ARANFunctions.LocalToPrj(pntCntI2, axis, Rv * Math.Cos(quarterPI), -(int)turnDir * Rv * Math.Sin(quarterPI)); 
           // GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntJ, 1, "j");

            Point pntK = ARANFunctions.LocalToPrj(pntCntI2, axis, Rv, 0);
            //GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntK, 1, "k");

            Point pntL = ARANFunctions.LocalToPrj(pntCntI4, axis, Rv, 0);
//            GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntL, 1, "L");

            Point pntM = ARANFunctions.LocalToPrj(pntCntI4, axis, Rv * Math.Cos(quarterPI), (int)turnDir * Rv * Math.Sin(quarterPI));
  //          GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntM, 1, "M");

            Point pntN4 = ARANFunctions.LocalToPrj(pntCntI4, axis, 0, (int)turnDir * Rv);
    //        GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntN4, 1, "N4");

            //Point pntN3 = ARANFunctions.LocalToPrj ( pntG, axis, 0, ( int ) turnDir * 2 * Rv );
            Point pntN3 = ARANFunctions.LocalToPrj(pntCntI3, axis, 0, (int)turnDir * Rv);
         //   GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pntN3, 1, "N3");

            inboundTrack = CalculateHoldingArea(wayPoint, axis, (int)turnDir, Rv, L);
            //Turn180 
            turn180 = new LineString();
            turn180.Add(wayPoint);
            GlobalParams.UI.DrawLineString(turn180, 3, 1);
            CreateSpiralBy2Radial(pntCntB, Rv + 5 * w_, axis + (int)turnDir * ARANMath.C_PI_2, axis, E / ARANMath.DegToRad(1), (int)turnDir, turn180);
            //End Turn180

            Point tmpFromPoint = ARANFunctions.LocalToPrj(pntI1, axis, -Wi1, 0);
            Point tmpToPoint = ARANFunctions.LocalToPrj(pntI1, axis, Wi1, 0);

            LineString spiralA = null, Arc3N;

            Ring ArcI1 = new Ring();
            //ArcI1 = ARANFunctions.CreateArcPrj ( PtI1, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection ( turnDir ) );
            ArcI1 = ARANFunctions.CreateArcPrj(pntI1, tmpFromPoint, tmpToPoint, turnDir);
          //  GlobalParams.UI.DrawRing(ArcI1, 100, Aran.AranEnvironment.Symbols.eFillStyle.sfsNull);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntI2, axis, -Wi2, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntI2, axis, Wi2, 0);
            Ring ArcI2 = ARANFunctions.CreateArcPrj(pntI2, tmpFromPoint, tmpToPoint, turnDir);
         //   GlobalParams.UI.DrawRing(ArcI2, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntJ, axis, -Wj * Math.Cos(quarterPI), -(int)turnDir * Wj * Math.Sin(quarterPI));
            //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
            tmpToPoint = ARANFunctions.LocalToPrj(pntJ, axis, Wj * Math.Cos(quarterPI), (int)turnDir * Wj * Math.Sin(quarterPI));
            //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
            Ring ArcJ = ARANFunctions.CreateArcPrj(pntJ, tmpFromPoint, tmpToPoint, turnDir);
         //   GlobalParams.UI.DrawRing(ArcJ, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsNull);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntK, axis, 0, -(int)turnDir * Wk);
            tmpToPoint = ARANFunctions.LocalToPrj(pntK, axis, 0, (int)turnDir * Wk);
            Ring Arck = ARANFunctions.CreateArcPrj(pntK, tmpFromPoint, tmpToPoint, turnDir);
         //   GlobalParams.UI.DrawRing(Arck, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsNull);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntL, axis, 0, -(int)turnDir * Wl);
            tmpToPoint = ARANFunctions.LocalToPrj(pntL, axis, 0, (int)turnDir * Wl);
            Ring ArcL = ARANFunctions.CreateArcPrj(pntL, tmpFromPoint, tmpToPoint, turnDir);
         //   GlobalParams.UI.DrawRing(ArcL, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntM, axis, Wm * Math.Cos(quarterPI), -(int)turnDir * Wm * Math.Sin(quarterPI));
            tmpToPoint = ARANFunctions.LocalToPrj(pntM, axis, -Wm * Math.Cos(quarterPI), (int)turnDir * Wm * Math.Sin(quarterPI));
            Ring ArcM = ARANFunctions.CreateArcPrj(pntM, tmpFromPoint, tmpToPoint, turnDir);
       //     GlobalParams.UI.DrawRing(ArcM, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN4, axis, Wn4, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN4, axis, -Wn4, 0);
            Ring ArcN4 = ARANFunctions.CreateArcPrj(pntN4, tmpFromPoint, tmpToPoint, turnDir);
        //    GlobalParams.UI.DrawRing(ArcN4, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN3, axis, Wn3, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN3, axis, -Wn3, 0);
            Ring ArcN3 = ARANFunctions.CreateArcPrj(pntN3, tmpFromPoint, tmpToPoint, turnDir);
         //   GlobalParams.UI.DrawRing(ArcN3, 250, Aran.AranEnvironment.Symbols.eFillStyle.sfsVertical);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN3, axis, 0, (int)turnDir * Wn3);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN3, axis, 0, -(int)turnDir * Wn3);
            Arc3N = ARANFunctions.CreateArcAsPartPrj(pntN3, tmpFromPoint, tmpToPoint, (TurnDirection)turnDir);
          //  GlobalParams.UI.DrawLineString(Arc3N, 1, 2);
            //intersectArcN3Turn180 = null;
            Geometry geom = GlobalParams.GeomOperators.Intersect(Arc3N, ARANFunctions.CreatePolyLineFromParts(turn180));

            double tmpOutDouble = 2 * Wn3;
            if (geom.Type == GeometryType.MultiLineString || ((MultiPoint)geom).Count == 0)
            {
                tmpFromPoint = ARANFunctions.CircleVectorIntersect(pntN3, Wn3, wayPoint, axis + ARANMath.C_PI, out tmpOutDouble);
                double distance = ARANFunctions.ReturnDistanceInMeters(tmpFromPoint, wayPoint);
                tmpToPoint = ARANFunctions.LocalToPrj(wayPoint, axis, distance, 0);
                spiralA = ARANFunctions.CreateArcAsPartPrj(wayPoint, tmpFromPoint, tmpToPoint, (TurnDirection)turnDir);
            }

          //  GlobalParams.UI.DrawLineString(spiralA, 1, 3);
        

            shablon = ChainHullAlgorithm.ConvexHull(turn180, ArcI1, ArcI2, ArcJ, Arck, ArcL, ArcM, ArcN3, ArcN4, spiralA);
            Polygon turnShablon = (Polygon)TransForm.RotateGeometry(wayPoint, shablon, -axis);
            GeoMinMaxPoint minMax = TransForm.QueryCoords(turnShablon);
            Point ptMin = new Point(minMax.XMin, minMax.YMin);
            Point ptMax = new Point(minMax.XMax, minMax.YMax);
            ptE = new Point();

            if (turnDir == TurnDirection.CCW)
                ptE.SetCoords(minMax.XMax - xe, minMax.YMax - ye);
            else
                ptE.SetCoords(minMax.XMax - xe, minMax.YMin + ye);

            ptE = (Point)TransForm.RotateGeometry(wayPoint, ptE, axis);
            //End Calculate


            //Calculate Line3 Intersect
            line3 = new LineString();
            tmpDirection = ARANFunctions.ReturnAngleInRadians(pntI3, pntG);
            tmpD = ARANFunctions.ReturnDistanceInMeters(pntI3, pntG);
            alpha = (int)turnDir * Math.Acos((Wi3 - Wg) / tmpD);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntG, tmpDirection - alpha, Wg, 0);
            tmpFromPoint.M = tmpDirection - alpha - (int)turnDir * ARANMath.C_PI_2;
            line3.Add((Point)tmpFromPoint.Clone());
            tmpToPoint = ARANFunctions.LocalToPrj(pntI3, tmpDirection - alpha, Wi3, 0);
            tmpToPoint.M = tmpFromPoint.M;
            line3.Add((Point)tmpToPoint.Clone());

            tmpDirection = ARANFunctions.ReturnAngleInRadians(pntI4, pntI3);
            tmpD = ARANFunctions.ReturnDistanceInMeters(pntI4, pntI3);
            alpha = (int)turnDir * Math.Acos((Wi4 - Wi3) / tmpD);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntI3, tmpDirection - alpha, Wi3, 0);
            tmpFromPoint.M = tmpDirection - alpha - (int)turnDir * ARANMath.C_PI_2;
            line3.Add((Point)tmpFromPoint.Clone());
            tmpToPoint = ARANFunctions.LocalToPrj(pntI4, tmpDirection - alpha, Wi4, 0);
            tmpToPoint.M = tmpFromPoint.M;
            line3.Add((Point)tmpToPoint.Clone());

            Point tmpPoint = new Point();
            tmpPoint.M = tmpFromPoint.M;
            tmpPoint = ARANFunctions.LocalToPrj(line3[3], tmpPoint.M, 5 * L, 0);
            line3.Add((Point)tmpPoint.Clone());
            //End Calculate Line3

            return 1;
        }
         
        /*
        public static int HoldingShablon(Point wayPoint, double ias, double altitude, double dIsa, double time, double axis, SideDirection side, int navType, out LineString inboundTrack, out Polygon shablon, out LineString line3, out LineString turn180, out Point ptE)
        {
            double tmpD, tmpDirection, alpha;
            TurnDirection turnDir = (TurnDirection)side;
            double V = ARANMath.IASToTAS(ias, altitude, dIsa);
            double v3600 = V;
            double R = 943.27 * 0.277777777777778 / V;
            if ((R > 3.0))
                R = 3.0;

            double Rv = 3600 * V / (62.83 * R);
            double H = altitude / 1000.0;
            double w = 12.0 * H + 87.0;

            double w_ = 0.277777777777778 * w;

            double E = w_ / R;
            double E45 = 45.0 * E;
            double t_ = 60.0 * time;

            double L = v3600 * t_;
            double ab = 5.0 * v3600;
            double ac = 11.0 * v3600;
            double gi1 = (t_ - 5.0) * v3600;
            double gi2 = (t_ + 21.0) * v3600;

            double Wb = 5 * w_;
            double Wc = 11 * w_;
            double Wd = Wc + E45;
            double We = Wc + 2 * E45;
            double Wf = Wc + 3 * E45;
            double Wg = Wc + 4 * E45;
            double Wh = Wb + 4 * E45;
            double Wo = Wb + 5 * E45;
            double Wp = Wb + 6 * E45;
            double Wi1 = (t_ + 6) * w_ + 4 * E45;
            double Wi3 = Wi1;
            double Wi2 = Wi1 + 14 * w_;
            double Wi4 = Wi2;
            double Wj = Wi2 + E45;
            double Wk = Wi2 + 2 * E45;
            double Wl = Wk;
            double Wm = Wi2 + 3 * E45;
            double Wn3 = Wi1 + 4 * E45;
            double Wn4 = Wi2 + 4 * E45;

            double xe = 2.0 * Rv + (t_ + 15.0) * v3600 + (26.0 + 195.0 / R + t_) * w_;
            double ye = 11.0 * v3600 * System.Math.Cos(ARANMath.DegToRad(20.0)) + Rv * System.Math.Sin(ARANMath.DegToRad(20.0)) + Rv + (t_ + 15.0) * v3600 * System.Math.Tan(ARANMath.DegToRad(5.0)) + (21.0 + 125.0 / R + t_) * w_;

#warning Newly added
            axis = ARANMath.Modulus(axis - ARANMath.C_PI, ARANMath.C_2xPI);

            Point pntCntB = ARANFunctions.LocalToPrj(wayPoint, axis, -ab, -Rv * (int)turnDir);
            Point pntCntC = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * (int)turnDir);

            Point pntG = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * 2 * (int)turnDir);
            //GlobalParams.DrawPointWithText ( pntG, 1, "g" );

            Point pntI1 = ARANFunctions.LocalToPrj(pntG, axis, gi1 * cosGuidanceTolerance, -gi1 * sinGuidanceTolerance * (int)turnDir);
            //GlobalParams.DrawPointWithText ( pntI1, 1, "I1" );

            Point pntI2 = ARANFunctions.LocalToPrj(pntG, axis, gi2 * cosGuidanceTolerance, -gi2 * sinGuidanceTolerance * (int)turnDir);
            //GlobalParams.DrawPointWithText ( pntI2, 1, "I2" );

            Point pntI3 = ARANFunctions.LocalToPrj(pntG, axis, gi1 * cosGuidanceTolerance, gi1 * sinGuidanceTolerance * (int)turnDir);
            //GlobalParams.DrawPointWithText ( pntI3, 1, "I3" );

            Point pntI4 = ARANFunctions.LocalToPrj(pntG, axis, gi2 * cosGuidanceTolerance, gi2 * sinGuidanceTolerance * (int)turnDir);
            //GlobalParams.DrawPointWithText ( pntI4, 1, "I4" );

            Point pntCntI1 = ARANFunctions.LocalToPrj(pntI1, axis, 0, (int)turnDir * Rv);
            Point pntCntI2 = ARANFunctions.LocalToPrj(pntI2, axis, 0, (int)turnDir * Rv);
            Point pntCntI3 = ARANFunctions.LocalToPrj(pntI3, axis, 0, (int)turnDir * Rv);
            Point pntCntI4 = ARANFunctions.LocalToPrj(pntI4, axis, 0, (int)turnDir * Rv);


            double quarterPI = ARANMath.C_PI * 0.25;
            Point pntJ = ARANFunctions.LocalToPrj(pntCntI2, axis, Rv * Math.Cos(quarterPI), -(int)turnDir * Rv * Math.Sin(quarterPI));				//GlobalParams.DrawPointWithText(pntJ,1,"j");

            Point pntK = ARANFunctions.LocalToPrj(pntCntI2, axis, Rv, 0);
            //GlobalParams.DrawPointWithText(pntK,1,"k");

            Point pntL = ARANFunctions.LocalToPrj(pntCntI4, axis, Rv, 0);
            //GlobalParams.DrawPointWithText(pntL,1,"L");

            Point pntM = ARANFunctions.LocalToPrj(pntCntI4, axis, Rv * Math.Cos(quarterPI), (int)turnDir * Rv * Math.Sin(quarterPI));
            //GlobalParams.DrawPointWithText(pntM,1,"M");

            Point pntN4 = ARANFunctions.LocalToPrj(pntCntI4, axis, 0, (int)turnDir * Rv);
            //GlobalParams.DrawPointWithText(pntN4,1,"N4");

            //Point pntN3 = ARANFunctions.LocalToPrj ( pntG, axis, 0, ( int ) turnDir * 2 * Rv );
            Point pntN3 = ARANFunctions.LocalToPrj(pntCntI3, axis, 0, (int)turnDir * Rv);
            //GlobalParams.DrawPointWithText ( pntN3, 1, "N3" );

            inboundTrack = CalculateHoldingArea(wayPoint, axis, (int)turnDir, Rv, L);
            //Turn180 
            turn180 = new LineString();
            turn180.Add(wayPoint);
            CreateSpiralBy2Radial(pntCntB, Rv + 5 * w_, axis + (int)turnDir * ARANMath.C_PI_2, axis, E / ARANMath.DegToRad(1), (int)turnDir, turn180);
            //End Turn180

            Point tmpFromPoint = ARANFunctions.LocalToPrj(pntI1, axis, -Wi1, 0);
            Point tmpToPoint = ARANFunctions.LocalToPrj(pntI1, axis, Wi1, 0);

            LineString spiralA = null, Arc3N;

            //ArcI1 = ARANFunctions.CreateArcPrj ( PtI1, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection ( turnDir ) );			
            Ring ArcI1 = ARANFunctions.CreateArcPrj(pntI1, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntI2, axis, -Wi2, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntI2, axis, Wi2, 0);
            Ring ArcI2 = ARANFunctions.CreateArcPrj(pntI2, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntJ, axis, -Wj * Math.Cos(quarterPI), -(int)turnDir * Wj * Math.Sin(quarterPI));
            //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
            tmpToPoint = ARANFunctions.LocalToPrj(pntJ, axis, Wj * Math.Cos(quarterPI), (int)turnDir * Wj * Math.Sin(quarterPI));
            //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
            Ring ArcJ = ARANFunctions.CreateArcPrj(pntJ, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntK, axis, 0, -(int)turnDir * Wk);
            tmpToPoint = ARANFunctions.LocalToPrj(pntK, axis, 0, (int)turnDir * Wk);
            Ring Arck = ARANFunctions.CreateArcPrj(pntK, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntL, axis, 0, -(int)turnDir * Wl);
            tmpToPoint = ARANFunctions.LocalToPrj(pntL, axis, 0, (int)turnDir * Wl);
            Ring ArcL = ARANFunctions.CreateArcPrj(pntL, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntM, axis, Wm * Math.Cos(quarterPI), -(int)turnDir * Wm * Math.Sin(quarterPI));
            tmpToPoint = ARANFunctions.LocalToPrj(pntM, axis, -Wm * Math.Cos(quarterPI), (int)turnDir * Wm * Math.Sin(quarterPI));
            Ring ArcM = ARANFunctions.CreateArcPrj(pntM, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN4, axis, Wn4, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN4, axis, -Wn4, 0);
            Ring ArcN4 = ARANFunctions.CreateArcPrj(pntN4, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN3, axis, Wn3, 0);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN3, axis, -Wn3, 0);
            Ring ArcN3 = ARANFunctions.CreateArcPrj(pntN3, tmpFromPoint, tmpToPoint, turnDir);

            tmpFromPoint = ARANFunctions.LocalToPrj(pntN3, axis, 0, (int)turnDir * Wn3);
            tmpToPoint = ARANFunctions.LocalToPrj(pntN3, axis, 0, -(int)turnDir * Wn3);
            //LineString tmpTurn90 = new LineString ( );
            //CreateSpiralBy2Radial ( pntCntB, Rv + 5 * w_, axis + ( int ) turnDir * ARANMath.C_PI_2, axis - ( int ) turnDir * ARANMath.C_PI_2, E / ARANMath.DegToRad ( 1 ), ( int ) turnDir, tmpTurn90 );

            Arc3N = ARANFunctions.CreateArcAsPartPrj(pntN3, tmpFromPoint, tmpToPoint, (TurnDirection)turnDir);

            //intersectArcN3Turn180 = null;
            Geometry geom = GlobalParams.GeomOperators.Intersect(Arc3N, ARANFunctions.CreatePolyLineFromParts(turn180));

            double tmpOutDouble = 2 * Wn3;
            if (geom.Type == GeometryType.MultiLineString || ((MultiPoint)geom).Count == 0)
            {
#warning Test this variant
                tmpFromPoint = ARANFunctions.CircleVectorIntersect(pntN3, Wn3, wayPoint, axis + ARANMath.C_PI, out tmpOutDouble);
                double distance = ARANFunctions.ReturnDistanceInMeters(tmpFromPoint, wayPoint);
                tmpToPoint = ARANFunctions.LocalToPrj(wayPoint, axis, distance, 0);
                spiralA = ARANFunctions.CreateArcAsPartPrj(wayPoint, tmpFromPoint, tmpToPoint, (TurnDirection)turnDir);
            }

            //GlobalParams.DrawPoint ( ( ( MultiPoint ) geom ) [ 0 ], 255 );
            //GlobalParams.DrawPoint ( ( ( MultiPoint ) geom ) [ 1 ], 255 );

            //GlobalParams.DrawRing ( ArcI2, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( ArcJ, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( Arck, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( ArcL, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( ArcM, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( ArcN4, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawRing ( ArcN3, 1, AranEnvironment.Symbols.eFillStyle.sfsCross );

            //GlobalParams.DrawLineString ( spiralA, 1, 1 );

            shablon = ChainHullAlgorithm.ConvexHull(turn180, ArcI1, ArcI2, ArcJ, Arck, ArcL, ArcM, ArcN3, ArcN4, spiralA);
            Polygon turnShablon = (Polygon)TransForm.RotateGeometry(wayPoint, shablon, -axis);
            GeoMinMaxPoint minMax = TransForm.QueryCoords(turnShablon);
            Point ptMin = new Point(minMax.XMin, minMax.YMin);
            Point ptMax = new Point(minMax.XMax, minMax.YMax);
            ptE = new Point();

            if (turnDir == TurnDirection.CCW)
                ptE.SetCoords(minMax.XMax - xe, minMax.YMax - ye);
            else
                ptE.SetCoords(minMax.XMax - xe, minMax.YMin + ye);

            ptE = (Point)TransForm.RotateGeometry(wayPoint, ptE, axis);
            //End Calculate


            //Calculate Line3 Intersect
            line3 = new LineString();
            tmpDirection = ARANFunctions.ReturnAngleInRadians(pntI3, pntG);
            tmpD = ARANFunctions.ReturnDistanceInMeters(pntI3, pntG);
            alpha = (int)turnDir * Math.Acos((Wi3 - Wg) / tmpD);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntG, tmpDirection - alpha, Wg, 0);
            tmpFromPoint.M = tmpDirection - alpha - (int)turnDir * ARANMath.C_PI_2;
            line3.Add((Point)tmpFromPoint.Clone());
            tmpToPoint = ARANFunctions.LocalToPrj(pntI3, tmpDirection - alpha, Wi3, 0);
            tmpToPoint.M = tmpFromPoint.M;
            line3.Add((Point)tmpToPoint.Clone());

            tmpDirection = ARANFunctions.ReturnAngleInRadians(pntI4, pntI3);
            tmpD = ARANFunctions.ReturnDistanceInMeters(pntI4, pntI3);
            alpha = (int)turnDir * Math.Acos((Wi4 - Wi3) / tmpD);
            tmpFromPoint = ARANFunctions.LocalToPrj(pntI3, tmpDirection - alpha, Wi3, 0);
            tmpFromPoint.M = tmpDirection - alpha - (int)turnDir * ARANMath.C_PI_2;
            line3.Add((Point)tmpFromPoint.Clone());
            tmpToPoint = ARANFunctions.LocalToPrj(pntI4, tmpDirection - alpha, Wi4, 0);
            tmpToPoint.M = tmpFromPoint.M;
            line3.Add((Point)tmpToPoint.Clone());

            Point tmpPoint = new Point();
            tmpPoint.M = tmpFromPoint.M;
            tmpPoint = ARANFunctions.LocalToPrj(line3[3], tmpPoint.M, 5 * L, 0);
            line3.Add((Point)tmpPoint.Clone());
            //End Calculate Line3

            return 1;
        }
         */ 
         


		public static void HoldingShablonForRnaw(Point wayPoint, double IAS, double AbsH, double dISA, double T, double Axis, TurnDirection side, int NavType, out LineString HoldingArea, out Polygon Shablon, out LineString Line3, out LineString Turn180, out Point ptE)
		{

			double V, v3600, R, Rv, H, w, w_, E45, E, t_, L,
			 ab, ac, xe, ye;

			Point PtCntB;
			LineString Line2;

			Line3 = new LineString();
			Turn180 = new LineString();

			V = ARANMath.IASToTASForRnav(IAS, AbsH, dISA);
			v3600 = V;
			R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			Rv = 1000.0 * 3.6 * V / (62.83 * R);
			H = AbsH / 1000.0;
			w = 12.0 * H + 87.0;

			w_ = 0.277777777777778 * w;

			E = w_ / R;
			E45 = 45.0 * E;
			t_ = 60.0 * T;

			L = v3600 * t_;
			ab = 5.0 * v3600;
			ac = 11.0 * v3600;

			xe = 2.0 * Rv + L + 11 * v3600 + (22.0 + 195.0 / R) * w_;
			ye = 11.0 * v3600 * System.Math.Cos(ARANMath.DegToRad(20.0)) + Rv * System.Math.Sin(ARANMath.DegToRad(20.0)) + Rv + (22.0 + 125.0 / R) * w_;

			//Turn180 calculate 
			PtCntB = ARANFunctions.LocalToPrj(wayPoint, Axis, -ab, -Rv * (int)side);
			Turn180.Add(wayPoint);
			CreateSpiralBy2Radial(PtCntB, Rv + 5 * w_, Axis + (int)side * ARANMath.C_PI_2, Axis, E / ARANMath.DegToRad(1), (int)side, Turn180);
            

			//	End Calculate

			HoldingArea = CalculateHoldingArea(wayPoint, Axis, (int)side, Rv, L);

			//Calculate Line2
			TouchTurn180InRad = ARANFunctions.SpiralTouchAngle(Rv + 5 * w_, E / ARANMath.DegToRad(1), Axis, Axis + -(int)side * ARANMath.C_PI_2,(SideDirection) side);
            Line2 = new LineString();
			CreateSpiralBy2Radial(PtCntB, Rv + 5 * w_, Axis + (int)side * ARANMath.C_PI_2, Axis + (int)side * TouchTurn180InRad, E / ARANMath.DegToRad(1), (int)side, Line2);

       /*
            GlobalParams.UI.DrawPointWithText(PtCntB, 1, "PTCntB");
            GlobalParams.UI.DrawLineString(Turn180, 1, 1);
            GlobalParams.UI.DrawLineString(HoldingArea, 1, 2);
            GlobalParams.UI.DrawLineString(Line2, 1, 3);
			//End Calculate
            */
			//Calculate RnawShablon
			Point tmpPoint = ARANFunctions.LocalToPrj(wayPoint, Axis, 0, -(int)side * 2 * Rv);
			Point PtI = ARANFunctions.LocalToPrj(tmpPoint, Axis, L, 0);
			Point PtI1 = ARANFunctions.LineLineIntersect(Line2[Line2.Count - 1], Axis, PtI, Axis - (int)side * ARANMath.C_PI_2) as Point;

			LineString rotateAraoundPtPrj = TransForm.RotateGeometry(wayPoint, (Geometry)Turn180, ARANMath.C_PI) as LineString;
			LineString MovePartToI1 = TransForm.Move(rotateAraoundPtPrj, wayPoint, PtI1) as LineString;
			LineString MovePartToI = TransForm.Move(rotateAraoundPtPrj, wayPoint, PtI) as LineString;
			//
			Shablon = ChainHullAlgorithm.ConvexHull(Turn180, MovePartToI, MovePartToI1);

			Polygon turnShablon = (Polygon)TransForm.RotateGeometry(wayPoint, Shablon, -Axis);
			GeoMinMaxPoint minMax = TransForm.QueryCoords(turnShablon);
			Point ptMin = new Point(minMax.XMin, minMax.YMin);
			Point ptMax = new Point(minMax.XMax, minMax.YMax);
			ptE = new Point();

			if (side == TurnDirection.CCW)
			{
				ptE.SetCoords(minMax.XMax - xe, minMax.YMax - ye);
			}
			else
				ptE.SetCoords(minMax.XMax - xe, minMax.YMin + ye);

			ptE = (Point)TransForm.RotateGeometry(wayPoint, ptE, Axis);

			//	ui.DrawPointWithText(ptE, 1, "PTE");
			//ui.DrawPointWithText(ptMin, 1, "ptMin");
			//ui.DrawPointWithText(ptMax, 1, "ptMax");
			//ui.DrawLineString(Line2, 255, 7);
			//ui.DrawPolygon(turnShablon, 1, ARAN.Contracts.AranGraphics.eFillStyle.sfsDiagonalCross);
			//ui.DrawMultiLineString(HoldingArea, 1, 2);

			//End Calculate


		}

		public static LineString CalculateHoldingArea(Point ptNavPrj, double axis, int side, double Rv, double L)
		{
			MultiPoint holdingArea = new MultiPoint();
			holdingArea.Add(ptNavPrj);
			//PtC
			holdingArea[0].M = axis + ARANMath.C_PI;//change

			holdingArea.Add(ARANFunctions.LocalToPrj(ptNavPrj, axis, 0, -(int)side * 2.0 * Rv));
			//PtG '
			holdingArea[1].M = axis;

			holdingArea.Add(ARANFunctions.LocalToPrj(holdingArea[1], axis, L, 0));
			holdingArea[2].M = axis;

			holdingArea.Add(ARANFunctions.LocalToPrj(holdingArea[2], axis, 0, (int)side * 2 * Rv));
			holdingArea[3].M = axis + ARANMath.C_PI;
			holdingArea.Add(holdingArea[0]);

			return ARANFunctions.CalcTrajectoryFromMultiPoint(holdingArea);
		}

		public static void ChangeSpiralStartParam(double E, double StR0, double StRadial, ref double NewStR0, ref double NewStRadial, int side, int sideChange)
		{
			double TurnDeg;

			TurnDeg = ARANMath.Modulus((NewStRadial - StRadial) * (int)sideChange, ARANMath.C_2xPI);

			if (TurnDeg >= ARANMath.C_PI)
			{
				sideChange = -(sideChange);
				TurnDeg = ARANMath.Modulus((NewStRadial - StRadial) * (int)sideChange, ARANMath.C_2xPI);
			}

			NewStR0 = StR0 + E * TurnDeg * 180 / ARANMath.C_PI * (int)side * (int)sideChange;

			if (NewStR0 < 0.0)
			{
				TurnDeg = StR0 / E;
				NewStR0 = 0.0;
			}
			NewStRadial = ARANMath.Modulus(TurnDeg * (int)sideChange + StRadial, ARANMath.C_2xPI);
		}

		public static void CreateSpiralBy2Radial(Point ptCnt, double r0, double aztStRad, double aztEndRad, double E, int side, MultiPoint multiPoint)
		{
			double dAlpha, turnRad, r;
			int n;

			turnRad = ARANMath.Modulus((aztEndRad - aztStRad) * (int)side, ARANMath.C_2xPI);
			dAlpha = ARANMath.C_PI / 180.0;
			n = (int)Math.Ceiling(turnRad / dAlpha);
			if (n < 2)
				n = 2;
			else if (n < 5)
				n = 5;
			else if (n < 10)
				n = 10;
			dAlpha = turnRad / n;
			for (int i = 0; i <= n; i++)
			{
				r = r0 + (i * dAlpha * E);// * 180/Math.PI );
				// + dphi0 * coef
				Point ptCur = ARANFunctions.LocalToPrj(ptCnt, aztStRad + (i * dAlpha) * (int)side, r, 0);
				multiPoint.Add(ptCur);
			}
		}

		public static int TouchTo2Spiral(Point pntCnt1, double r10, double e1, double aztSt1, Point pntCnt2, double r20, double e2, double aztSt2, SideDirection side, ref double touchAngle1, ref double touchAngle2, out Point touchPnt1, out Point touchPnt2)
		{

			#region In Radian

			bool bOutOfSpiral;
			int i, j, k;
			double f, f_, r1, r2 = 0, fTmp, phi1 = 0, phi2 = 0, phi10 = 0, phi20 = 0, aztR1E, aztR2E, aztO1O2, distance;
			//Point touchPnt1, touchPnt2;

			aztO1O2 = ARANFunctions.ReturnAngleInRadians(pntCnt1, pntCnt2);
			bOutOfSpiral = false;

			touchPnt1 = null;
			touchPnt2 = null;

			for (k = 0; k <= 10; k++)
			{
				phi10 = ARANMath.Modulus(aztO1O2 - (ARANMath.C_PI_2 + 10 * (ARANMath.C_PI / 180) * k) * (int)side, ARANMath.C_2xPI);
				fTmp = ARANMath.Modulus((phi10 - aztSt1) * (int)side, ARANMath.C_2xPI);
				r1 = r10 + e1 * fTmp;// *180 / Math.PI;
				//pnt1 = ARANFunctions.LocalToPrj ( pntCnt1, phi10, R1, 0 );
				touchPnt1 = ARANFunctions.PointAlongPlane(pntCnt1, ARANMath.RadToDeg(phi10), r1);

				phi20 = ARANFunctions.ReturnAngleInRadians(pntCnt2, touchPnt1);
				distance = ARANFunctions.ReturnDistanceInMeters(pntCnt2, touchPnt1);
				fTmp = ARANMath.Modulus((phi20 - aztSt2) * (int)side, ARANMath.C_2xPI);
				r2 = r20 + e2 * fTmp;// *180/Math.PI;

				if (r2 < distance)
				{
					phi20 = phi10;
					bOutOfSpiral = true;
					break; // TODO: might not be correct. Was : Exit For
				}
			}

			if (!bOutOfSpiral)
			{
				return 0;
			}

			for (j = 0; j <= 30; j++)
			{
				fTmp = ARANMath.Modulus((phi10 - aztSt1) * (int)side, ARANMath.C_2xPI);
				r1 = r10 + e1 * fTmp;//*180/Math.PI;
				aztR1E = System.Math.Atan(e1 * (int)side / r1);

				for (i = 0; i <= 20; i++)
				{
					fTmp = ARANMath.Modulus((phi20 - aztSt2) * (int)side, ARANMath.C_2xPI);
					r2 = r20 + e2 * fTmp;// *180/Math.PI;
					aztR2E = System.Math.Atan(e2 * (int)side / r2);
					f = phi20 - phi10 + aztR1E - aztR2E;
					fTmp = e2 * e2;

					f_ = (1 + /* 180 * */ fTmp / ( /*Math.PI * */(r2 * r2 + fTmp)));

					phi2 = phi20 - (f / f_);

					if (System.Math.Abs(System.Math.Sin(f / f_)) < radEps)
					{
						break; // TODO: might not be correct. Was : Exit For
					}
					else
					{
						phi20 = phi2;
					}
				}

				touchPnt1 = ARANFunctions.LocalToPrj(pntCnt1, phi1, r1, 0);
				touchPnt2 = ARANFunctions.LocalToPrj(pntCnt2, phi2, r2, 0);
				fTmp = ARANFunctions.ReturnAngleInRadians(touchPnt1, touchPnt2);
				fTmp = ARANMath.SubtractAnglesWithSign(phi1 + (ARANMath.C_PI_2 * (int)side - aztR1E), fTmp, side);
				phi1 = phi10 + fTmp * (int)side;
				if (System.Math.Abs(fTmp) < (radEps * 50))
				{
					touchAngle1 = ARANMath.Modulus(phi1, ARANMath.C_2xPI);
					touchAngle2 = ARANMath.Modulus(phi2, ARANMath.C_2xPI);
					//InitHolding.AranGraphics.DrawPointWithText ( touchPnt1, 65357*255, "Pt1" );
					//InitHolding.AranGraphics.DrawPointWithText ( touchPnt2, 65357*255, "Pt2" );
					//double dist = ARANFunctions.ReturnDistanceAsMeter ( touchPnt1, touchPnt2 );
					//LineString tmpPart1 = new LineString (), tmpPart2 = new LineString ();
					//CreateSpiralBy2Radial ( pntCnt1, r10, aztSt1, aztSt1 - ( int ) side * ARANMath.C_PI_2, e1 / ARANFunctions.RadToDeg ( 1 ), ( int ) side, tmpPart1 );
					//CreateSpiralBy2Radial ( pntCnt2, r20, aztSt2, aztSt2 -( int ) side*ARANMath.C_PI_2, e2/ ARANFunctions.RadToDeg ( 1 ), ( int ) side, tmpPart2 );
					//InitHolding.AranGraphics.DrawLineString ( tmpPart1, 0, 1 );
					//InitHolding.AranGraphics.DrawLineString ( tmpPart2, 0, 1 );
					//LineString tmpLine = new LineString ();
					//tmpLine.Add ( touchPnt1 );
					//tmpLine.Add ( touchPnt2 );
					//InitHolding.AranGraphics.DrawLineString ( tmpLine, 0, 1 );
					return 1;
				}
				else
				{
					phi10 = phi1;
				}
			}
			return 0;

			#endregion

			#region In Degree

			//double fTmp, distance, phi1 = 0, phi2 = 0, phi10 = 0, phi20 = 0, aztR1E, aztR2E, aztO1O2;
			//double r1 = 0, r2 = 0, f, f_;
			//long i, j, k;
			//Point pt1;
			//Point pt2;
			//bool bOutOfSpiral;

			//aztSt1 = ARANFunctions.RadToDeg ( aztSt1 );
			//aztSt2 = ARANFunctions.RadToDeg ( aztSt2 );
			//e1 = e1 / ARANFunctions.RadToDeg ( 1 );
			//e2 = e2 / ARANFunctions.RadToDeg ( 1 );

			//aztO1O2 = ARANFunctions.ReturnAngleAsDegree ( pntCnt1, pntCnt2 );
			//bOutOfSpiral = false;
			//touchAngle1 = 0;
			//touchAngle1 = 0;
			//for ( k = 0; k <= 10; k++ )
			//{
			//    phi10 = ARANMath.Modulus ( aztO1O2 - ( 90 + 10 * k ) *( int ) side, 360 );
			//    fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, 360 );
			//    r1 = r10 + e1 * fTmp;
			//    pt1 = ARANFunctions.PointAlongPlane ( pntCnt1, phi10, r1 );

			//    phi20 = ARANFunctions.ReturnAngleAsDegree ( pntCnt2, pt1 );
			//    distance = ARANFunctions.ReturnDistanceAsMeter ( pntCnt2, pt1 );
			//    fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) *( int ) side, 360 );
			//    r2 = r20 + e2 * fTmp;

			//    if ( r2 < distance )
			//    {
			//        phi20 = phi10;
			//        bOutOfSpiral = true;
			//        break;
			//    }
			//}
			//if ( !bOutOfSpiral )
			//    return 0;

			//for ( j = 0; j<= 30; j++ )
			//{
			//    fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, 360 );
			//    r1 = r10 + e1 * fTmp;
			//    aztR1E = ARANFunctions.RadToDeg ( Math.Atan ( ARANFunctions.RadToDeg ( e1 * ( int ) side ) / r1 ) );

			//    for ( i = 0; i<= 20; i++ )
			//    {
			//        fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) * ( int ) side, 360 );
			//        r2 = r20 + e2 * fTmp;
			//        aztR2E = ARANFunctions.RadToDeg ( Math.Atan ( ARANFunctions.RadToDeg ( e2 * ( int ) side ) / r2 ) );
			//        f = phi20 - phi10 + aztR1E - aztR2E;
			//        fTmp = ARANFunctions.RadToDeg ( e2 ) * ARANFunctions.RadToDeg ( e2 );
			//        f_ = 1 + fTmp / ( r2 * r2 + fTmp );
			//        phi2 = phi20 - f / f_;

			//        if ( Math.Abs ( Math.Sin ( ARANFunctions.DegToRad ( f / f_ ) ) ) < degEps )
			//            break;
			//        else
			//            phi20 = phi2;
			//    }

			//    pt1 = ARANFunctions.PointAlongPlane ( pntCnt1, ARANFunctions.DegToRad ( phi1 ), r1 );
			//    pt2 = ARANFunctions.PointAlongPlane ( pntCnt2, ARANFunctions.DegToRad ( phi2 ), r2 );
			//    fTmp = ARANFunctions.ReturnAngleAsDegree ( pt1, pt2 );
			//    fTmp = ARANFunctions.RadToDeg ( ARANMath.SubtractAnglesWithSign ( ARANFunctions.DegToRad ( phi1 + ( 90 * ( int ) side - aztR1E ) ), ARANFunctions.DegToRad ( fTmp ), ( side ) ) );
			//    phi1 = phi10 + fTmp * ( int ) side;
			//    if ( Math.Abs ( fTmp ) < ( degEps * 50) )
			//    {
			//        touchAngle1 = ARANMath.Modulus ( phi1, 360 );
			//        InitHolding.ui.DrawPointWithText ( pt1, 0, "Pt 1" );
			//        touchAngle2 = ARANMath.Modulus ( phi2, 360 );
			//        InitHolding.ui.DrawPointWithText ( pt2, 0, "Pt 2" );
			//        //Point PtCnt1, double r10, double E1, double AztSt1, 
			//        //Point PtCnt2, double r20, double E2, double AztSt2, 
			//        //SideDirection side
			//        double dist = ARANFunctions.ReturnDistanceAsMeter ( pt1, pt2 );
			//        LineString tmpPart1 = new LineString (), tmpPart2 = new LineString ();
			//        CreateSpiralBy2Radial ( pntCnt1, r10, ARANFunctions.DegToRad ( aztSt1 ), ARANFunctions.DegToRad ( aztSt1 ) - ( int ) side * ARANMath.C_PI_2, e1, ( int ) side, tmpPart1 );
			//        CreateSpiralBy2Radial ( pntCnt2, r20, ARANFunctions.DegToRad ( aztSt2 ), ARANFunctions.DegToRad ( aztSt2 ) - ( int ) side * ARANMath.C_PI_2, e2, ( int ) side, tmpPart2 );
			//        InitHolding.ui.DrawLineString ( tmpPart1, 0, 1 );
			//        InitHolding.ui.DrawLineString ( tmpPart2, 0, 1 );
			//        LineString tmpLine = new LineString ();
			//        tmpLine.Add ( pt1 );
			//        tmpLine.Add ( pt2 );
			//        InitHolding.ui.DrawLineString ( tmpLine, 0, 1 );
			//        return 1;
			//    }
			//    else
			//        phi10 = phi1;
			//}
			//return 0;

			#endregion
		}

		public static double CalculateWd(double T, double ias, double dias, double absH, double att, double xtt, out double tMin)
		{
			double ds, Rv, V, v3600, R, WD,t_;
            double w, w_, E, E45, WDmin, Wc, Wg, H;

			V = ARANMath.IASToTASForRnav(ias, absH,0);
			v3600 = V;
			R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			Rv = 1000.0 * 3.6 * V / (62.83 * R);
			t_ = T * 60;
			ds = v3600 * t_;
			WD = Math.Sqrt(ds*ds + 4 * Rv*Rv);
			
            H = absH / 1000.0;
			w = 12.0 * H + 87.0;
			w_ = 0.277777777777778 * w;

			E = w_ / R;
			E45 = 45.0 * E;

			Wc = 11 * w_;
			Wg = Wc + 4 * E45;
			WDmin = Math.Sqrt(Math.Pow(att + 11 * v3600, 2) + Math.Pow(2 * Rv + xtt, 2)) + Wg;
            if (WD > WDmin)
                WDmin = WD;

            tMin = Math.Sqrt(Math.Pow(WDmin, 2) - 4 * Math.Pow(Rv, 2)) / v3600;
            return WDmin;
		}

		public static double SpiralTouchToFix(Point ptCnt, double E, double r0, double aztStRad, SideDirection side, Point fixPnt, int spiralIntercept, double axis)
		{
			int i, j;
			double r, f, f_, xA, yA, xSp, ySp, phi = 0, phi0, fTmp, sinA, cosA, dPhi, spAngle, aztToFix, distToFix;
			double result;

			aztToFix = ARANFunctions.ReturnAngleInRadians(ptCnt, fixPnt);
			distToFix = ARANFunctions.ReturnDistanceInMeters(ptCnt, fixPnt);

			fTmp = ARANMath.Modulus((aztToFix - aztStRad) * (int)side, ARANMath.C_2xPI);

			if ((fTmp > ARANMath.C_PI) && (r0 == 0.0))
			{
				fTmp = fTmp - ARANMath.C_2xPI;
			}
			Point touchPnt;
			r = r0 + E * fTmp;
			if (System.Math.Abs(r - distToFix) < distEps)
			{
				return aztToFix;
			}

			if (r < distToFix)
			{
				phi0 = ARANMath.Modulus(aztToFix + ARANMath.C_PI_2 * (1 + spiralIntercept), ARANMath.C_2xPI);
				for (i = 0; i <= 30; i++)
				{
					phi = phi0;
					spAngle = ARANFunctions.SpiralTouchAngle(r0, E, aztStRad + ARANMath.C_PI_2 * (int)side, phi, side);
					r = r0 + E * spAngle;

					result = ARANMath.Modulus(aztStRad + spAngle * (int)side, ARANMath.C_2xPI);

					xSp = ptCnt.X + r * System.Math.Cos(result);
					ySp = ptCnt.Y + r * System.Math.Sin(result);

					touchPnt = new Point(xSp, ySp);

					fTmp = System.Math.Atan2(fixPnt.Y - ySp, fixPnt.X - xSp);

					phi0 = ARANMath.Modulus(fTmp + ARANMath.C_PI_2 * (1 + spiralIntercept), ARANMath.C_2xPI);

					dPhi = ARANMath.SubtractAngles(phi, phi0);

					if (dPhi < radEps)
					{
						return result;
					}
				}
			}
			else
			{

				cosA = System.Math.Cos(axis + ARANMath.C_PI_2 * (int)side);
				sinA = System.Math.Sin(axis + ARANMath.C_PI_2 * (int)side);
				xA = distToFix * System.Math.Cos(aztToFix);
				yA = distToFix * System.Math.Sin(aztToFix);
				phi0 = aztToFix;
				for (j = 0; j <= 20; j++)
				{
					fTmp = ARANMath.Modulus((phi0 - aztStRad) * (int)side, ARANMath.C_2xPI);
					r = r0 + E * fTmp;
					f = r * (System.Math.Sin(phi0) * cosA - System.Math.Cos(phi0) * sinA) + xA * sinA - yA * cosA;
					f_ = ARANMath.RadToDeg(E) * (int)side * (System.Math.Sin(phi0) * cosA - System.Math.Cos(phi0) * sinA) + r * (System.Math.Cos(phi0) * cosA + System.Math.Sin(phi0) * sinA);
					phi = phi0 - f / f_;
					if (System.Math.Abs(System.Math.Sin(phi - phi0)) < 0.001)
					{
						return ARANMath.Modulus(phi, ARANMath.C_2xPI);
					}
					else
						phi0 = phi;
				}
			}
			return ARANMath.Modulus(phi, ARANMath.C_2xPI);
		}

		public static double TurnRadius(double ias, double altitude, double dIsa)
		{
			double V = ARANMath.IASToTASForRnav(ias, altitude,0);
			double R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			return 1000.0 * 3.6 * V / (62.83 * R);
		}
	}
}
