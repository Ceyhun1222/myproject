using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.AranEnvironment;
using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;
using Holding.Models;
using Aran.PANDA.Rnav.Holding.Properties;
using Aran.Panda.Rnav.Holding.Properties;

namespace Holding
{
    public class BaseArea
    {
        public const double areaWidth = 1900;
        private IAranEnvironment _ui;

        public BaseArea()
        {
            _ui = GlobalParams.AranEnvironment;
        }

        public void CreateBaseArea(ProcedureType procedureType, DistanceType distanceType, Point wayPoint, double ias, double altitude,
                            double direction, double time, TurnDirection turn, double att, double xtt, double wd, protectionSectorType protectionType, HoldingGeometry holdingGeom)
        {
            var fullArea = new MultiPolygon();
            Polygon shablon = null;
            var sectorProtection = new MultiPolygon();
            Ring toleranceArea = null;
            LineString holdingTrack = null;
            LineString turn180 = null;
            Point ptE = null;
            Geometry buffer = null;

            try
            {
                MultiPolygon areaWithSectors ;
                Polygon holdingArea = null;
                if (distanceType == DistanceType.Wd)
                {
                    BaseAreaWithWd(wayPoint, ias, wd, altitude, time, direction, turn, att, xtt, protectionType, out holdingArea, out holdingTrack, ref sectorProtection, out toleranceArea, out shablon, out turn180, out ptE);

                    areaWithSectors = new MultiPolygon();
                    areaWithSectors.Add(holdingArea);
                }


                else if (procedureType == ProcedureType.RNP)
                {
                    RNPBaseArea(wayPoint, ias, altitude, time, direction, turn, att, xtt, protectionType, out holdingArea, out holdingTrack, out buffer);
                    // areaWithSectors = (MultiPolygon)GlobalParams.GeomOperators.UnionGeometry(holdingArea, buffer);
                    areaWithSectors = new MultiPolygon();
                    areaWithSectors.Add(holdingArea);
                    holdingGeom.Buffer = (MultiPolygon)buffer;
                    holdingGeom.Areas.Add(buffer);
                }
                else
                {
                    LineString line3;
                    if (procedureType == ProcedureType.withHoldingFunc)
                    {
                        Shablons.HoldingShablonForRnaw(wayPoint, ias, altitude, 15, time, direction + Math.PI, turn, 0, out holdingTrack, out shablon, out line3, out turn180, out ptE);
                    }
                    else if (procedureType == ProcedureType.withoutHoldingFunc)
                    {
                        Shablons.HoldingShablon(wayPoint, ias, altitude, 15, time, direction + Math.PI, (SideDirection)turn, 0, out holdingTrack, out shablon, out line3, out turn180, out  ptE);
                    }

                    toleranceArea = ARANFunctions.ToleranceArea(wayPoint, att, xtt, direction, (SideDirection)turn);
                    holdingArea = GeomFunctions.MoveShablonAroundTolArea(shablon, toleranceArea, wayPoint);
                    areaWithSectors = new MultiPolygon();
                    areaWithSectors.Add(holdingArea);

                    if (protectionType == protectionSectorType.omnidirectional)
                    {
                        areaWithSectors = OmnidirictionalArea(holdingArea, wayPoint, shablon, turn180, toleranceArea, ptE, turn);
                        MultiPolygon sectors = (MultiPolygon)GlobalParams.GeomOperators.Difference(areaWithSectors, holdingArea);
                        if (sectors != null)
                            sectorProtection = sectors;
                    }

                }

                holdingGeom.HoldingArea = holdingArea;
                holdingGeom.HoldingTrack = holdingTrack;
                holdingGeom.ToleranceArea = toleranceArea;
                holdingGeom.SectorProtection = sectorProtection;

                holdingGeom.Shablon = shablon;
                holdingGeom.Areas.Clear();
                holdingGeom.AreaWithSectors = areaWithSectors;
                fullArea.Add(areaWithSectors[0]);
                int areaCount = Enum.GetValues(typeof(mocArea)).GetLength(0);
                if (procedureType != ProcedureType.RNP)
                {
                    for (int i = 0; i < areaCount; i++)
                    {
                        try
                        {
                            fullArea = GlobalParams.GeomOperators.Buffer(areaWithSectors, areaWidth * (i + 1)) as Aran.Geometries.MultiPolygon;
                            if (fullArea != null)
                                holdingGeom.Areas.Add(fullArea);
                        }   
                        catch (Exception)
                        {
                            try
                            {
                                if (i > 0 && holdingGeom.Areas.Count > 0)
                                {
                                    fullArea = GlobalParams.GeomOperators.Buffer(holdingGeom.Areas[holdingGeom.Areas.Count - 1], areaWidth) as Aran.Geometries.MultiPolygon;
                                    if (fullArea != null)
                                        holdingGeom.Areas.Add(fullArea);
                                }
                            }
                            catch (Exception e)
                            {
                                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                                MessageBox.Show(Resources.Error_creating_buffer, Resources.Holding_Caption);
                            }                        
                        }

                    }
                }
                holdingGeom.FullArea = fullArea;
                if (buffer == null)
                    holdingGeom.Buffer = null;
            }

            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);
                MessageBox.Show("Error occured during creating base area:" + e.Message,Resources.Holding_Caption);
            }

        }
   
        public void BaseAreaWithWd(Point wayPoint, double ias, double wd, double altitude, double time, double direction, TurnDirection turn, double att, double xtt, protectionSectorType protectionType, out Polygon baseArea, out LineString holdingTrack, ref MultiPolygon sectorProtection, out Ring toleranceArea, out Polygon shablon,out LineString turn180,out Aran.Geometries.Point ptE)
        {

            double tmpRad, dc2 = 0, dc3 = 0, fTmp;
            double V, R, Rv;

            LineString line3, arcC3;
            Aran.Geometries.Point tmpFromPoint, tmpToPoint, ptC1 = null, ptC2 = null, ptC2_ = null, ptC3 = null,
                                        ptC7, ptC8;
            Aran.Geometries.Point ptA1, ptA3, C1_, C3_, ptA4;
            LineString arcD2, arcD1, arcC1, Rp2, turnArcD2, rotateTurn180, turn180InC8,
                    rotateTurn180By30, turn180InC7, turn180InA3;
            LineString protectionC1, protectionC2, protectionC3, protectionC2_ = null, protectionC1_C3_;
            MultiPoint tmpMp;
            Ring arcC1C3;
            Polygon shablonInA1, rotateShablon;
            bool flag;
            GeometryOperators geomOperators = GlobalParams.GeomOperators;

            Aran.Geometries.Point longDistancePoint = new Aran.Geometries.Point();

            toleranceArea = ARANFunctions.ToleranceArea(wayPoint, att, xtt, direction,(SideDirection)turn);

            V = ARANMath.IASToTASForRnav(ias, altitude,0);
            R = 943.27 * 0.277777777777778 / V;
            if ((R > 3.0))
                R = 3.0;

            Rv = 1000.0 * 3.6 * V / (62.83 * R);
            time = Math.Sqrt(wd * wd - 4 * Rv * Rv) / (V * 60);

            Shablons.HoldingShablon(wayPoint, ias, altitude, 15, time, direction + Math.PI,(SideDirection) turn, 0, out holdingTrack, out shablon, out line3, out turn180, out ptE);
            ptA1 = toleranceArea[3];
            ptA3 = toleranceArea[2];
            ptA4 = toleranceArea[1];
            tmpRad = ARANFunctions.ReturnAngleInRadians(line3[2], line3[3]);
            line3.Add(ARANFunctions.LocalToPrj(line3[3], tmpRad, wd, 0));
            
            //Move Shablon and line3 to A1 and A3 

            shablonInA1 = TransForm.Move(shablon, wayPoint, ptA1) as Polygon;
            turn180InA3 = TransForm.Move(turn180, wayPoint, ptA3) as LineString;
            LineString turn180InA1 =(LineString) TransForm.Move(turn180, wayPoint, ptA1);

            //		 End

            //Fix C1 point
            tmpFromPoint = ARANFunctions.LocalToPrj(wayPoint, direction, (wd + att), 0);
            tmpToPoint = ARANFunctions.LocalToPrj(wayPoint, direction, -(wd + att), 0);
            arcD2 = ARANFunctions.RingToPart(ARANFunctions.CreateArcPrj(wayPoint, tmpFromPoint, tmpToPoint, turn));
            tmpMp =(MultiPoint) geomOperators.Intersect(ARANFunctions.CreatePolyLineFromParts(arcD2), ARANFunctions.PolygonToPolyLine(shablonInA1));
            if (tmpMp.Count >= 1)
                ptC1 = tmpMp[0];
            //end
            //Fix C2 point
            //In here i changed Line3InA3 as Line3

            tmpFromPoint = ARANFunctions.LocalToPrj(wayPoint, direction, 0, -(int)turn * (wd - att));
            tmpToPoint = ARANFunctions.LocalToPrj(wayPoint, direction, 0, (int)turn * (wd - att));
            arcD1 = ARANFunctions.RingToPart(ARANFunctions.CreateArcPrj(wayPoint, tmpFromPoint, tmpToPoint,ARANMath.ChangeDirection(turn)));
            
            tmpMp =(MultiPoint) geomOperators.Intersect(ARANFunctions.CreatePolyLineFromParts(arcD1), ARANFunctions.CreatePolyLineFromParts(line3));
            if (tmpMp.Count > 1 || (tmpMp.Count == 0))
                MessageBox.Show("Wrong in Intersect",Resources.Holding_Caption);
            else
                ptC2 = tmpMp[0];
            //end
            
            //Fix C3 point
            //In here also changed line3InA3 as line3
            turnArcD2 = (LineString)TransForm.RotateGeometry(wayPoint, arcD2, (int)turn * Math.PI / 2);   //ARANFunctions.RingToPart(ARANFunctions.CreateArcPrj(ptNavPrj, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(turn)));
            tmpMp =(MultiPoint) geomOperators.Intersect(ARANFunctions.CreatePolyLineFromParts(turnArcD2), ARANFunctions.CreatePolyLineFromParts(line3));

            if (tmpMp.Count >= 1)
                ptC3 = tmpMp[0];
            //end
            if (ptC2 != null)
                dc2 = ARANFunctions.Point2LineDistancePrj(ptC2, wayPoint, direction + Math.PI);
            if (ptC3 != null)
                dc3 = ARANFunctions.Point2LineDistancePrj(ptC3, wayPoint, direction + Math.PI);
            //Draw Rp2 Line

            Rp2 = new LineString();
            Rp2.Add(ptA3);
            Rp2.Add(ARANFunctions.LocalToPrj(ptA3, direction, -30000, 0));
            //end
            flag = false;
            if (dc2 > xtt)
            {
                flag = true;
                tmpMp = (geomOperators.Intersect(ARANFunctions.CreatePolyLineFromParts(Rp2), ARANFunctions.CreatePolyLineFromParts(arcD1)) as MultiPoint);
                if (tmpMp.Count >= 1)
                    ptC2 = tmpMp[0];
            }

            if (dc3 > xtt)
            {
                tmpMp = (geomOperators.Intersect(ARANFunctions.CreatePolyLineFromParts(Rp2), ARANFunctions.CreatePolyLineFromParts(turnArcD2)) as MultiPoint);
                if (tmpMp.Count >= 1)
                    ptC3 = tmpMp[0];

                if (flag == false)
                {

                    tmpMp = (geomOperators.Intersect(line3, ARANFunctions.CreatePolyLineFromParts(Rp2)) as MultiPoint);
                    if (tmpMp.Count >= 1)
                        ptC2_ = tmpMp[0];
                }
            }

            //
            rotateShablon = (Polygon)TransForm.RotateGeometry(wayPoint, shablon, Math.PI);
            rotateTurn180 = (LineString)TransForm.RotateGeometry(wayPoint, turn180, Math.PI);
            arcC1 = (LineString)TransForm.Move(rotateTurn180, wayPoint, ptC1);
            double tmpDistance = GeomFunctions.MaxDistFromPointToGeomety(wayPoint, arcC1, out C1_);
            arcC3 = (LineString)TransForm.Move(rotateTurn180, wayPoint, ptC3);
            tmpDistance = GeomFunctions.MaxDistFromPointToGeomety(wayPoint, arcC3, out C3_);
            arcC1C3 = ARANFunctions.CreateArcPrj(wayPoint, ptC1, ptC3, turn);
            protectionC1 =(LineString) GeomFunctions.PointsOnGeometry(arcC1, C1_);

            protectionC1_C3_ = (LineString)GeomFunctions.MaxPointsFromArcToSpiral(wayPoint, arcC1C3, rotateTurn180);
            protectionC2 = TransForm.Move(rotateTurn180, wayPoint, ptC2) as LineString;
            protectionC3 = TransForm.Move(rotateTurn180, wayPoint, ptC3) as LineString;
            if (ptC2_ != null)
                protectionC2_ = (LineString)TransForm.Move(rotateTurn180, wayPoint, ptC2_);

            if (turn == TurnDirection.CW)
                baseArea = Holding.ChainHullAlgorithm.ConvexHull(turn180, protectionC1_C3_, protectionC1, protectionC3, protectionC2, protectionC2_, turn180InA3,turn180InA1);
            else
                baseArea = Holding.ChainHullAlgorithm.ConvexHull(turn180, protectionC1_C3_, protectionC1, protectionC3, protectionC2, protectionC2_, turn180InA3,turn180InA1);
           
            //Calculate sector protection 2
            
            if (protectionType == protectionSectorType.direct)
                return;
            else
                if (protectionType == protectionSectorType.omnidirectional)
                {
                    rotateTurn180By30 = (LineString)TransForm.RotateGeometry(wayPoint, turn180, ARANMath.C_PI - (int)turn * ARANMath.DegToRad(30));
                    ptC7 = ARANFunctions.RingVectorIntersect(ARANFunctions.PartToRing(arcD2), ptA1, direction + ARANMath.C_PI - (int)turn * ARANMath.DegToRad(35), out fTmp);

                    ptC8 = ARANFunctions.RingVectorIntersect(ARANFunctions.PartToRing(arcD2), ptA4, direction + ARANMath.C_PI - (int)turn * ARANMath.DegToRad(25), out fTmp);
                    turn180InC7 = (LineString)TransForm.Move(rotateTurn180By30, wayPoint, ptC7);
                    turn180InC8 = (LineString)TransForm.Move(rotateTurn180By30, wayPoint, ptC8);

                   // GlobalParams.UI.DrawLineString(rotateTurn180By30, 1, 1);
                    Polygon protectPart1 = GeomFunctions.MoveGeometryAroundTwoPoint(rotateTurn180By30, wayPoint, ptC7, ptC8, turn);
                   
                    //calculate sector protection 1
                    Aran.Geometries.Point ptTmp = ARANFunctions.RingVectorIntersect(arcC1C3, ptA1, direction + Math.PI, out fTmp);
                    LineString tmpPart = (LineString)TransForm.Flip(turn180, wayPoint, ARANMath.C_PI_2);
                    Polygon protectPart2 = GeomFunctions.MoveGeometryAroundTwoPoint(tmpPart, wayPoint, ptTmp, ptC3, turn);

                    LineString flipTurn180By30 = (LineString)TransForm.Flip(rotateTurn180By30, wayPoint, ARANMath.C_PI_2);

                    Ring rotateToleranceBy70 = (Ring)TransForm.RotateGeometry(wayPoint, toleranceArea, -(int)turn * 70 * Math.PI / 180);

                    //Flip turn180 in the waypoint 180.Then rotate result goemetry 70.then move this rotate tolerance area point 2.
                    LineString flipTurn180By180 = (LineString)TransForm.Flip(turn180, wayPoint, ARANMath.C_PI);
                    LineString rotateFlip = (LineString)TransForm.RotateGeometry(wayPoint, flipTurn180By180, -(int)turn * ARANMath.DegToRad(70));
                    LineString protectPart3 = (LineString)TransForm.Move(rotateFlip, wayPoint, rotateToleranceBy70[2]);

                    
                    //sector 1 protection end
                    //calculate sector 3 protection
                    LineString sector3RotateTurn180 = (LineString)TransForm.RotateGeometry(wayPoint, turn180, -(int)turn * 70 * Math.PI / 180);
                    LineString protectPart4 = (LineString)TransForm.Move(sector3RotateTurn180, wayPoint, rotateToleranceBy70[3]);
                    
                    
                    LineString rotateTurn180By70 = (LineString)TransForm.RotateGeometry(wayPoint, turn180, -(int)turn * 70 * Math.PI / 180);

                    LineString rotateTurn180InA1 = (LineString)TransForm.Move(rotateTurn180By70, wayPoint, rotateToleranceBy70[3]);

                    Aran.Geometries.Point ptDistanceToArc6;
                    double longDistancetoArc6 = GeomFunctions.MaxDistFromPointToGeomety(wayPoint,rotateTurn180InA1, out ptDistanceToArc6);

                    tmpRad = ARANFunctions.ReturnAngleInRadians(wayPoint, ptDistanceToArc6);
                    Ring protectPart5 = ARANFunctions.CreateArcPrj(wayPoint, ptDistanceToArc6, ARANFunctions.LocalToPrj(wayPoint, tmpRad + (int)turn * 70 * Math.PI / 180, longDistancetoArc6, 0), turn);

                    //
                    
                    Aran.Geometries.Polygon fullArea = ChainHullAlgorithm.ConvexHull(baseArea,protectPart1,protectPart2,protectPart3,protectPart4,protectPart5);
                    sectorProtection =(MultiPolygon) geomOperators.Difference(fullArea,baseArea);
                    baseArea = fullArea;
                }
        }

        public void RNPBaseArea(Point wayPoint, double ias, double altitude, double time, double direction, TurnDirection turn, double att, double xtt, protectionSectorType protectionType, out Polygon baseArea, out LineString holdingTrack, out Geometry buffer)
        {
            double V, t_, radius, fTmp;
            double d1, d2, d3, d4 = 0, Ep, alpha, w, H;
            Ring arcLef, arcRight;
            LineString rightRNPArea, leftRNPArea;
            LineString arcBufferRight;
            Point ptFrom, ptTo, ptCenter, ptIntersectLeftTop, ptIntersectLeftDown,
            ptIntersectRightTop, ptIntersectRightDown, ptTop, ptDown;

            Geometry bufferRight = null;
            buffer = null;

            MultiPoint holdingMultPoints = new MultiPoint();


            V = ARANMath.IASToTASForRnav(ias, altitude,0);
            t_ = 60.0 * time;

            //Calculate HoldingAread for RNP HOLDING
            d1 = V * t_;
            Ep = altitude / 30.48;
            H = altitude / 1000;
            w = (12 * H + 87) / 3.6;

            if (Ep < 245)
                alpha = 23 * Math.PI / 180;
            else
                alpha = 15 * Math.PI / 180;

            d2 = (Math.Pow((V + w), 2) * 3.6 * 3.6 * 1000) / (63547 * Math.Tan(alpha));
            if (protectionType == protectionSectorType.omnidirectional)
                d4 = d2 * (1 - Math.Sin(ARANMath.DegToRad(20))) / (2 * Math.Cos(ARANMath.DegToRad(20)));
            d3 = xtt;

            holdingMultPoints.Add(ARANFunctions.LocalToPrj(wayPoint, direction, d4, 0));
            holdingMultPoints[0].M = direction;

            holdingMultPoints.Add(ARANFunctions.LocalToPrj(wayPoint, direction, d4, (int)turn * d2));
            holdingMultPoints[1].M = direction + Math.PI;

            holdingMultPoints.Add(ARANFunctions.LocalToPrj(holdingMultPoints[1], direction, -d1 - d4, 0));
            holdingMultPoints[2].M = direction + Math.PI;

            holdingMultPoints.Add(ARANFunctions.LocalToPrj(holdingMultPoints[2], direction, 0, -(int)turn * d2));
            holdingMultPoints[3].M = direction;
            holdingMultPoints.Add(holdingMultPoints[0]);
            holdingTrack = ARANFunctions.CalcTrajectoryFromMultiPoint(holdingMultPoints);

            //

            radius = d2 / 2;
            ptCenter = ARANFunctions.LocalToPrj(wayPoint, direction, d4, (int)turn * radius);
            ptFrom = ARANFunctions.LocalToPrj(ptCenter, direction, 0, (int)turn * (d3 * Math.Sqrt(2) + radius));
            ptTo = ARANFunctions.LocalToPrj(ptCenter, direction, 0, -(int)turn * (d3 * Math.Sqrt(2) + radius));
            arcLef = ARANFunctions.CreateArcPrj(ptCenter, ptFrom, ptTo,ARANMath.ChangeDirection(turn));
            ptTop = ARANFunctions.LocalToPrj(ptCenter, direction, 0, (int)turn * (radius + d3));
            ptDown = ARANFunctions.LocalToPrj(ptCenter, direction, 0, -(int)turn * (radius + d3));
			ptIntersectLeftTop = ARANFunctions.RingVectorIntersect(arcLef, ptTop, direction, out fTmp);
			ptIntersectLeftDown = ARANFunctions.RingVectorIntersect(arcLef, ptDown, direction, out fTmp);
            Aran.Geometries.Point tt = ARANFunctions.LocalToPrj(ptDown, direction, 2 * radius,0);
            leftRNPArea = ARANFunctions.CreateArcAsPartPrj(ptCenter, ptIntersectLeftTop, ptIntersectLeftDown, ARANMath.ChangeDirection(turn));

            arcRight = (Ring)TransForm.Move(TransForm.RotateGeometry(ptCenter, arcLef, Math.PI), ptCenter, ARANFunctions.LocalToPrj(ptCenter, direction, -(d4 + d1), 0));

			ptIntersectRightTop = ARANFunctions.RingVectorIntersect(arcRight, ptTop, direction + Math.PI, out fTmp);
			ptIntersectRightDown = ARANFunctions.RingVectorIntersect(arcRight, ptDown, direction + Math.PI, out fTmp);

            rightRNPArea = ARANFunctions.CreateArcAsPartPrj(ARANFunctions.LocalToPrj(ptCenter, direction, -(d4 + d1), 0), ptIntersectRightTop, ptIntersectRightDown, turn);
            baseArea = ChainHullAlgorithm.ConvexHull(rightRNPArea, leftRNPArea, ptCenter);

            double bufferWith;
            if ((d3 + 3700) > 9300)
                bufferWith = 9300;
            else
                bufferWith = d3 + 3700;

            ptTop = ARANFunctions.LocalToPrj(ptIntersectLeftTop, direction, 0, (int)turn * bufferWith);
            ptDown = ARANFunctions.LocalToPrj(ptIntersectLeftDown, direction, 0, -(int)turn * bufferWith);
            LineString arcBufferLeft = ARANFunctions.CreateArcAsPartPrj(ptCenter, ptTop, ptDown, ARANMath.ChangeDirection(turn));

            arcBufferRight = (LineString)TransForm.Move(TransForm.RotateGeometry(ptCenter, arcBufferLeft, Math.PI), ptCenter, ARANFunctions.LocalToPrj(ptCenter, direction, -(d4 + d1), 0));
            GlobalParams.GeomOperators.Cut(ChainHullAlgorithm.ConvexHull(arcBufferLeft, arcBufferRight, ptCenter), ARANFunctions.PolygonToPolyLine(baseArea), out buffer, out bufferRight);
        }

        private MultiPolygon OmnidirictionalArea(Polygon holdingArea,Point wayPoint, Polygon shablon, LineString turn180, Ring toleranceArea, Point ptE, TurnDirection turn)
        {
            IList<Polygon> moveShablon = new List<Polygon>();
            Point ptDistanceToArc6;
            Ring rotateToleranceBy70,arc6;
            Polygon convexParam;
            MultiPolygon unionParam;
            LineString rotateTurn180By70, rotateTurn180InA1, rotateTurn180InA3, flipTurn180InA3;
            double longDistancetoArc6, tmpRad;
            Polygon Omindirectianal = GeomFunctions.MoveGeometryAroundCircle(shablon, ptE, wayPoint, ARANFunctions.ReturnDistanceInMeters(wayPoint, toleranceArea[0]));
            //rotate turn180 and ToleranceArea to 70
            rotateTurn180By70 = (LineString)TransForm.RotateGeometry(wayPoint, turn180, -(int)turn * 70 * Math.PI / 180);
            
            rotateToleranceBy70 = (Ring)TransForm.RotateGeometry(wayPoint, toleranceArea, -(int)turn * 70 * Math.PI / 180);
            rotateTurn180InA1 = (LineString)TransForm.Move(rotateTurn180By70, wayPoint, rotateToleranceBy70[3]);
            rotateTurn180InA3 = (LineString)TransForm.Move(rotateTurn180By70, wayPoint, rotateToleranceBy70[2]);
            flipTurn180InA3 = (LineString)TransForm.Flip(rotateTurn180InA3, wayPoint, -(int)turn * 70 * Math.PI / 180);
            //Distance to 6th arc
            longDistancetoArc6 = GeomFunctions.MaxDistFromPointToGeomety(wayPoint, rotateTurn180InA1, out ptDistanceToArc6);
            tmpRad = ARANFunctions.ReturnAngleInRadians(wayPoint, ptDistanceToArc6);
            arc6 = ARANFunctions.CreateArcPrj(wayPoint, ptDistanceToArc6, ARANFunctions.LocalToPrj(wayPoint, tmpRad + (int)turn * 70 * Math.PI / 180, longDistancetoArc6, 0), turn);
            arc6.Add(wayPoint);
            convexParam = ChainHullAlgorithm.ConvexHull(Omindirectianal, holdingArea, rotateTurn180InA1, rotateTurn180InA3, flipTurn180InA3,arc6);
            unionParam = new MultiPolygon();
            unionParam.Add(convexParam);
            return unionParam;
        }

    }
   
}
