using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    class NS_Page3_Helper
    {
        public SideDirection Side { get; set; }
        Point[] localPoints;
        List<int> reachableVFIdx;
        public List<Interval> Ranges { get; private set; }
        public double MinDist { get; private set; }
        public double MaxDist { get; private set; }
        Point divergeEndPnt = null;

        public NS_Page3_Helper()
        {
            Ranges = new List<Interval>();
            reachableVFIdx = new List<int>();
        }

        public List<string> getReachableVisualFeatures()
        {
            List<string> reachbleVFsNames = new List<string>(); ;
            double minAngle;
            double maxAngle;
            convertPrjToLocal();
            for (int i = 0; i < localPoints.Length; i++)
            {
                if (isReachable(localPoints[i], out minAngle, out maxAngle))
                {                    
                    VMManager.Instance.ReachableVFs.Add(VMManager.Instance.AllVisualFeatures[i]);
                    reachbleVFsNames.Add(VMManager.Instance.AllVisualFeatures[i].Name);
                    Ranges.Add(new Interval(minAngle, maxAngle));
                    reachableVFIdx.Add(i);                    
                }
            }
            return reachbleVFsNames;
        }        

        private void convertPrjToLocal()
        {
            localPoints = new Point[VMManager.Instance.AllVisualFeatures.Length];
            for (int i = 0; i < localPoints.Length; i++)
            {
                if(VMManager.Instance.isFinalStep && VMManager.Instance.AllVisualFeatures[i].Name.Equals("THR" + VMManager.Instance.SelectedRWY.Name))
                {
                    double finalSegmentTime = VMManager.Instance.FinalSegmentTime;
                    double direction = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.AllVisualFeatures[i].pShape, VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing.Value) - ARANMath.C_PI, ARANMath.C_2xPI);
                    double dist = VMManager.Instance.VM_TASWind * finalSegmentTime;            
                    VMManager.Instance.FinalSegmentStartPoint = ARANFunctions.PointAlongPlane(VMManager.Instance.AllVisualFeatures[i].pShape, direction, dist);
                    Point convPnt = getConvergenceFlyByPoint();
                    localPoints[i] = ARANFunctions.PrjToLocal(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, convPnt);
                }
                else
                    localPoints[i] = ARANFunctions.PrjToLocal(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, VMManager.Instance.AllVisualFeatures[i].pShape);
            }
        }

        public bool isReachable(Point pnt, out double minAngle, out double maxAngle)
        {
            #region 1
            minAngle = 0;
            maxAngle = 0;
            if (pnt.X < 0)
                return false;

            double eps = 0.000001;
            double r = VMManager.Instance.VM_TurnRadius;
            double z_min, z_max, tempZ;
            double a = Math.Abs(pnt.Y) - 2 * r;
            double b = 2 * pnt.X;
            double c = (-1) * Math.Abs(pnt.Y);
            List<Interval> intervals = new List<Interval>();

            if (Math.Abs(a) > eps) // a != 0
            {
                #region
                double D = b * b - 4 * a * c;

                if (D > eps)
                {
                    #region
                    z_max = ((-b) + Math.Sqrt(D)) / (2 * a);
                    z_min = ((-b) - Math.Sqrt(D)) / (2 * a);

                    if (z_min > z_max)
                    {
                        tempZ = z_min;
                        z_min = z_max;
                        z_max = tempZ;
                    }

                    if (a > 0)
                    {
                        if (z_max <= 0)
                        {
                            intervals.Add(new Interval(0 + eps, double.MaxValue));
                        }
                        else if (z_min <= 0 && z_max > 0)
                        {
                            intervals.Add(new Interval(z_max, double.MaxValue));
                        }
                        else
                        {
                            intervals.Add(new Interval(0 + eps, z_min));
                            intervals.Add(new Interval(z_max, double.MaxValue));
                        }
                    }
                    else // a < 0
                    {
                        if (z_min <= 0 && z_max > 0)
                        {
                            intervals.Add(new Interval(0 + eps, z_max));
                        }
                        else if (z_min > 0)
                        {
                            intervals.Add(new Interval(z_min, z_max));
                        }
                    }
                    #endregion
                }
                else if (D < eps && D >= 0) // D == 0
                {
                    #region
                    if (a > 0)
                    {
                        intervals.Add(new Interval(0 + eps, double.MaxValue));
                    }
                    #endregion
                }
                #endregion
            }
            else // a == 0
            {
                #region
                z_min = z_max = c / b;

                if (b > eps) // b > 0
                {
                    if (z_max <= 0)
                        intervals.Add(new Interval(0 + eps, double.MaxValue));
                    else
                        intervals.Add(new Interval(z_max, double.MaxValue));
                }
                else if (b < -eps) // b < 0
                {
                    if (z_max > 0)
                        intervals.Add(new Interval(0 + eps, z_max));
                }
                else // b == 0
                {
                    if (c >= 0)
                        intervals.Add(new Interval(0 + eps, double.MaxValue));
                }
                #endregion
            }
            #endregion
            
            if (intervals.Count > 0)
            {
                if (2 * Math.Atan(intervals[0].MIN) <= VMManager.Instance.MaxDivergenceAngle)
                {
                    minAngle = 2 * Math.Atan(intervals[0].MIN);
                    if (2 * Math.Atan(intervals[0].MAX) <= VMManager.Instance.MaxDivergenceAngle)
                    {
                        maxAngle = 2 * Math.Atan(intervals[0].MAX);
                    }
                    else
                    {
                        maxAngle = VMManager.Instance.MaxDivergenceAngle;
                    }

                    double tempMax;
                    double val1 = Math.Abs(pnt.Y);
                    double val2 = 2 * VMManager.Instance.VM_TurnRadius - val1;
                    if (val2 > 0)
                    {
                        tempMax = 2 * Math.Atan(Math.Sqrt(val1 / val2));
                    }
                    else
                    {
                        tempMax = ARANMath.C_PI_2;
                    }

                    if (tempMax < maxAngle)
                        maxAngle = tempMax;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private Point getIntersectPoint(Point endPnt, /*int idx,*/ Point localPnt, Point startPnt, double startDir)
        {
            Point intersectPnt = null;
            #region 1
            //double eps = 0.000001;
            //double dX = (double) nmrcUpDown_distanceRange.Value;
            //double r = VMManager.Instance.TurnRadius;
            //double a = Math.Abs(pnt.Y) - 2 * r;
            //double b = 2 * (pnt.X - dX);
            //double c = (-1) * Math.Abs(pnt.Y);
            //double z_min = 0, z_max = 0;

            //if (Math.Abs(a) > eps) // a != 0
            //{
            //    double D = b * b - 4 * a * c;

            //    if (D > 0)
            //    {
            //        z_max = ((-b) + Math.Sqrt(D)) / (2 * a);
            //        z_min = ((-b) - Math.Sqrt(D)) / (2 * a);

            //        double tempZ;

            //        if (z_min > z_max)
            //        {
            //            tempZ = z_min;
            //            z_min = z_max;
            //            z_max = tempZ;
            //        }

            //        if (2 * Math.Atan(z_max) > VMManager.Instance.MaxDivergenceAngle)
            //        {
            //            z_max = z_min;
            //        }
            //        if (z_min > 0 && z_max > 0)
            //        {
            //            int k = -1;
            //        }
            //    }
            //    else if (D == 0)
            //    {
            //        z_min = z_max = (-b) / (2 * a);                    
            //    }
            //}
            //else
            //{
            //    z_min = z_max = (-c) / b;
            //}

            //double dist = VMManager.Instance.TurnRadius * z_max;
            //intersectPnt = ARANFunctions.PointAlongPlane(startPnt, startDir, dist);
            //angle = 2 * Math.Atan(z_max);
            #endregion

            #region 2
            //SideDirection side = ARANMath.SideDef(startPnt, startDir, VMManager.Instance.ReachableVFs[idx].pShape);
            SideDirection side = ARANMath.SideDef(startPnt, startDir, endPnt);
            Point cent = null;
            if (side == SideDirection.sideLeft)
                cent = ARANFunctions.PointAlongPlane(startPnt, ARANMath.Modulus(startDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            else if (side == SideDirection.sideRight)
                cent = ARANFunctions.PointAlongPlane(startPnt, ARANMath.Modulus(startDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            else
            {
                intersectPnt = startPnt;
                return intersectPnt;
            }
            Point tg1, tg2;
            //ARANFunctions.getCircleTangentPoints(VMManager.Instance.TurnRadius, cent, VMManager.Instance.ReachableVFs[idx].pShape, out tg1, out tg2);
            ARANFunctions.getCircleTangentPoints(VMManager.Instance.VM_TurnRadius, cent, endPnt, out tg1, out tg2);
            if (ARANFunctions.ReturnDistanceInMeters(startPnt, tg1) > ARANFunctions.ReturnDistanceInMeters(startPnt, tg2))
                tg1 = tg2;
            
            if (ARANFunctions.ReturnDistanceInMeters(startPnt, tg1) <= ARANFunctions.ReturnDistanceInMeters(startPnt, localPnt))
                //intersectPnt = (Point)ARANFunctions.LineLineIntersect(VMManager.Instance.ReachableVFs[idx].pShape, ARANFunctions.ReturnAngleInRadians(VMManager.Instance.ReachableVFs[idx].pShape, tg1), startPnt, startDir);
                intersectPnt = (Point)ARANFunctions.LineLineIntersect(endPnt, ARANFunctions.ReturnAngleInRadians(endPnt, tg1), startPnt, startDir);
            else
                //intersectPnt = (Point)ARANFunctions.LineLineIntersect(VMManager.Instance.ReachableVFs[idx].pShape, ARANFunctions.ReturnAngleInRadians(tg1, VMManager.Instance.ReachableVFs[idx].pShape), startPnt, startDir);
                intersectPnt = (Point)ARANFunctions.LineLineIntersect(endPnt, ARANFunctions.ReturnAngleInRadians(tg1, endPnt), startPnt, startDir);
            #endregion
            
            return intersectPnt;
        }

        public void onClose()
        {
            if (VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1] > -1)
            {
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1]);
                VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1] = -1;
            }

            if (VMManager.Instance.ConvergencePolyElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergencePolyElement);
                VMManager.Instance.ConvergencePolyElement = -1;
            }

            if (VMManager.Instance.SelectedVFElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedVFElement);
                VMManager.Instance.SelectedVFElement = -1;
            }

            
        }

        public void CalculateMaxMinDistance(int idx, Point pnt, double minAngle, double maxAngle)
        {
            if (pnt == null)
            {
                MinDist = localPoints[reachableVFIdx[idx]].X
                    - Math.Abs(localPoints[reachableVFIdx[idx]].Y) / Math.Tan(Ranges[idx].MIN)
                    - VMManager.Instance.VM_TurnRadius * Math.Tan(Ranges[idx].MIN / 2);
                MaxDist = localPoints[reachableVFIdx[idx]].X
                    - Math.Abs(localPoints[reachableVFIdx[idx]].Y) / Math.Tan(Ranges[idx].MAX)
                    - VMManager.Instance.VM_TurnRadius * Math.Tan(Ranges[idx].MAX / 2);
            }
            else
            {
                MinDist = pnt.X
                    - Math.Abs(pnt.Y) / Math.Tan(minAngle)
                    - VMManager.Instance.VM_TurnRadius * Math.Tan(minAngle / 2);
                MaxDist = pnt.X
                    - Math.Abs(pnt.Y) / Math.Tan(maxAngle)
                    - VMManager.Instance.VM_TurnRadius * Math.Tan(maxAngle / 2);
            }
        }

        public void DrawSelectedVF(int idx)
        {
            if (VMManager.Instance.SelectedVFElement > -1)
            {
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.SelectedVFElement);
                VMManager.Instance.SelectedVFElement = -1;
            }
            VMManager.Instance.SelectedVFElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.ReachableVFs[idx].pShape, ARANFunctions.RGB(51, 0, 102));
        }

        public double ConstructDivergenceLine(double distFromInitialPnt, int idx, Point endPnt)
        {
            VMManager.Instance.DivergenceLine = new LineString();
            Point divergeStartPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, distFromInitialPnt);
            Point centPnt = null;
            divergeEndPnt = null;
            Point intersectPnt = null;

            double intermediateDirection = 0;

            if (Side == SideDirection.sideLeft)
                centPnt = ARANFunctions.PointAlongPlane(divergeStartPnt, ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            else if (Side == SideDirection.sideRight)
                centPnt = ARANFunctions.PointAlongPlane(divergeStartPnt, ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);

            intersectPnt = getIntersectPoint(endPnt, /*idx,*/ localPoints[reachableVFIdx[idx]], divergeStartPnt, VMManager.Instance.InitialDirection);
            Point tngPnt1 = null, tngPnt2 = null;
            ARANFunctions.getCircleTangentPoints(VMManager.Instance.VM_TurnRadius, centPnt, intersectPnt, out tngPnt1, out tngPnt2);
            if (ARANFunctions.ReturnDistanceInMeters(divergeStartPnt, tngPnt1) > ARANFunctions.ReturnDistanceInMeters(divergeStartPnt, tngPnt2))
                divergeEndPnt = tngPnt1;
            else
                divergeEndPnt = tngPnt2;


            //MultiLineString mls = new MultiLineString();

            VMManager.Instance.DivergenceLine.Add(VMManager.Instance.InitialPosition);
            VMManager.Instance.DivergenceLine.Add(divergeStartPnt);
            //mls.Add(ls);
            double diffAngle;
            //diffAngle = Math.Abs(ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(divergeEndPnt, VMManager.Instance.ReachableVFs[idx].pShape), ARANMath.C_2xPI) - VMManager.Instance.InitialDirection);
            diffAngle = Math.Abs(ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(divergeEndPnt, endPnt), ARANMath.C_2xPI) - VMManager.Instance.InitialDirection);
            if (diffAngle > ARANMath.C_PI)
                diffAngle = ARANMath.C_2xPI - diffAngle;

            if (Side == SideDirection.sideLeft)
            {
                //mls.Add(ARANFunctions.CreateArcAsPartPrj(centPnt, divergeStartPnt, divergeEndPnt, TurnDirection.CCW));
                VMManager.Instance.DivergenceLine.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, divergeStartPnt, divergeEndPnt, TurnDirection.CCW));
                intermediateDirection = ARANMath.Modulus(VMManager.Instance.InitialDirection + diffAngle);
            }
            else if (Side == SideDirection.sideRight)
            {
                //mls.Add(ARANFunctions.CreateArcAsPartPrj(centPnt, divergeStartPnt, divergeEndPnt, TurnDirection.CW));
                VMManager.Instance.DivergenceLine.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, divergeStartPnt, divergeEndPnt, TurnDirection.CW));
                intermediateDirection = ARANMath.Modulus(VMManager.Instance.InitialDirection - diffAngle);
            }

            if (VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1] > -1)
            {
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1]);
                VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1] = -1;
            }
            VMManager.Instance.DivergenceLineElements[VMManager.Instance.DivergenceLineElements.Count - 1] = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.DivergenceLine, 255, 2);            

            //VMManager.Instance.ConvergenceFlyByPoint = VMManager.Instance.ReachableVFs[idx].pShape;
            VMManager.Instance.ConvergenceFlyByPoint = endPnt;
            VMManager.Instance.ConvergenceStartPoint = ARANFunctions.PointAlongPlane(divergeEndPnt, intermediateDirection, 0);
            VMManager.Instance.IntermediateDirection = intermediateDirection;

            double dist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.ConvergenceStartPoint);
            diffAngle = 2 * Math.Atan(dist / VMManager.Instance.VM_TurnRadius);
            if (diffAngle > VMManager.Instance.MaxConvergenceAngle)
                diffAngle = VMManager.Instance.MaxConvergenceAngle;
            VMManager.Instance.MinConvergenceDirection = ARANMath.Modulus(intermediateDirection - diffAngle, ARANMath.C_2xPI);
            VMManager.Instance.MaxConvergenceDirection = ARANMath.Modulus(intermediateDirection + diffAngle, ARANMath.C_2xPI);
            return GlobalVars.pspatialReferenceOperation.DirToAztPrj(divergeEndPnt, intermediateDirection);
        }

        public void ConstructConvergencePoly(double dist, /*int idx*/ Point endPnt)
        {
            //Point pntStart = ARANFunctions.PointAlongPlane(VMManager.Instance.ReachableVFs[idx].pShape, VMManager.Instance.MinConvergenceDirection, dist);
            Point pntStart = ARANFunctions.PointAlongPlane(endPnt, VMManager.Instance.MinConvergenceDirection, dist);
            //Point pntEnd = ARANFunctions.PointAlongPlane(VMManager.Instance.ReachableVFs[idx].pShape, VMManager.Instance.MaxConvergenceDirection, dist);
            Point pntEnd = ARANFunctions.PointAlongPlane(endPnt, VMManager.Instance.MaxConvergenceDirection, dist);
            VMManager.Instance.ConvergencePoly = new MultiPolygon();
            VMManager.Instance.ConvergencePoly.Add(new Polygon());
            //VMManager.Instance.ConvergencePoly.ExteriorRing.Add(VMManager.Instance.ReachableVFs[idx].pShape);
            VMManager.Instance.ConvergencePoly[0].ExteriorRing.Add(endPnt);
            //VMManager.Instance.ConvergencePoly.ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcPrj(VMManager.Instance.ReachableVFs[idx].pShape, pntStart, pntEnd, TurnDirection.CCW));
            VMManager.Instance.ConvergencePoly[0].ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcPrj(endPnt, pntStart, pntEnd, TurnDirection.CCW));
            if (VMManager.Instance.ConvergencePolyElement > -1)
                GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.ConvergencePolyElement);
            VMManager.Instance.ConvergencePolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.ConvergencePoly, 255, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);            
        }

        public Point getConvergenceFlyByPoint()
        {
            SideDirection side;
            if (divergeEndPnt == null)
            {
                Point cent1 = null, cent2 = null;
                side = ARANMath.SideDef(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, VMManager.Instance.FinalSegmentStartPoint);
                if (side == SideDirection.sideLeft)
                {
                    cent1 = ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                    cent2 = ARANFunctions.PointAlongPlane(VMManager.Instance.FinalSegmentStartPoint, ARANMath.Modulus(VMManager.Instance.FinalSegmentStartPointDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                }
                else if(side == SideDirection.sideRight)
                {
                    cent1 = ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                    cent2 = ARANFunctions.PointAlongPlane(VMManager.Instance.FinalSegmentStartPoint, ARANMath.Modulus(VMManager.Instance.FinalSegmentStartPointDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                }

                double distBetweenCents = ARANFunctions.ReturnDistanceInMeters(cent1, cent2);
                double angle = 0;
                if (distBetweenCents >= 2 * VMManager.Instance.VM_TurnRadius)
                {
                    double b = distBetweenCents / 2;
                    double c = VMManager.Instance.VM_TurnRadius;
                    double a = Math.Sqrt(b * b - c * c);
                    angle = Math.Acos((b * b + c * c - a * a) / (2 * b * c));
                }

                if (side == SideDirection.sideLeft)
                {
                    divergeEndPnt = ARANFunctions.PointAlongPlane(cent1, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(cent1, cent2) - angle, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius); 
                }
                else if(side == SideDirection.sideRight)
                {
                    divergeEndPnt = ARANFunctions.PointAlongPlane(cent1, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(cent1, cent2) + angle, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                }
            }
            side = ARANMath.SideDef(VMManager.Instance.FinalSegmentStartPoint, VMManager.Instance.FinalSegmentStartPointDirection, divergeEndPnt);
            Point centPnt = null;
            if (side == SideDirection.sideLeft)
            {
                centPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.FinalSegmentStartPoint, ARANMath.Modulus(VMManager.Instance.FinalSegmentStartPointDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            }
            else if (side == SideDirection.sideRight)
            {
                centPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.FinalSegmentStartPoint, ARANMath.Modulus(VMManager.Instance.FinalSegmentStartPointDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            }
            Point tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.FinalSegmentStartPoint, ARANMath.Modulus(VMManager.Instance.FinalSegmentStartPointDirection - ARANMath.C_PI, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
            Point tg1, tg2;


            ARANFunctions.getCircleTangentPoints(VMManager.Instance.VM_TurnRadius, centPnt, divergeEndPnt, out tg1, out tg2);
            if (ARANFunctions.ReturnDistanceInMeters(tempPnt, tg2) < ARANFunctions.ReturnDistanceInMeters(tempPnt, tg1))
            {
                tg1 = tg2;
            }
            
            Point resultPoint = (Point)ARANFunctions.LineLineIntersect(divergeEndPnt, ARANFunctions.ReturnAngleInRadians(divergeEndPnt, tg1), VMManager.Instance.FinalSegmentStartPoint, VMManager.Instance.FinalSegmentStartPointDirection);
            
            return resultPoint;
        }


        
    }
}
