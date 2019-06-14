using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Converters.ConverterJtsGeom;
using Aran.Panda.VisualManoeuvring.Forms;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    public class NS_Page_Helper
    {
        public NewSegmentForm nsf;
        public void showCurrentPositionAndDirection()
        {
            if (VMManager.Instance.TrackSegmentsList.Count == 0)
            {
                VMManager.Instance.InitialPosition = VMManager.Instance.TrackInitialPosition;
                VMManager.Instance.InitialDirection = VMManager.Instance.TrackInitialDirection;
                //VMManager.Instance.InitialDirection = GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.FinalNavaid.pPtPrj, ARANMath.RadToDeg(VMManager.Instance.TrueBRGAngle));
                
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
            else
            {
                VMManager.Instance.InitialPosition = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].EndPointPrj;
                VMManager.Instance.InitialDirection = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].FinalDirectionDir;
            }            

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialPositionElement);

            VMManager.Instance.InitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.InitialPosition, 255);

            //VMManager.Instance.StepMainPointsElements.Add(new List<int>());
        }

        /*public void ConstructStep(double d1, double alpha, TurnDirection side1, double d2, double beta, TurnDirection side2, double d3)
        {
            LineString d1Centreline = new LineString();
            LineString d2Centreline = new LineString();
            LineString d3Centreline = new LineString();
            
            Point centPnt;
            double intermDir = 0;
            double finalDir = 0;
            LineString centreline = new LineString();

            centreline.Add(VMManager.Instance.InitialPosition);
            if(d1 > 0)
            {
                d1Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], VMManager.Instance.InitialDirection, d1));
                d1Centreline.Add(centreline[centreline.Count - 1]);
            }

            if (side1 == TurnDirection.CCW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if (alpha > 0)
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) + alpha, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CCW));
                intermDir = ARANMath.Modulus(VMManager.Instance.InitialDirection + alpha, ARANMath.C_2xPI);
            }
            else if (side1 == TurnDirection.CW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if (alpha > 0)
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) - alpha, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CW));
                intermDir = ARANMath.Modulus(VMManager.Instance.InitialDirection - alpha, ARANMath.C_2xPI);
            }

            if (d2 > 0)
            {
                d2Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], intermDir, d2));
                d2Centreline.Add(centreline[centreline.Count - 1]);
            }

            if (side2 == TurnDirection.CCW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if(beta > 0)
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) + beta, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CCW));
                finalDir = ARANMath.Modulus(intermDir + beta, ARANMath.C_2xPI);
            }
            else if (side2 == TurnDirection.CW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if(beta > 0)
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) - beta, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CW));
                finalDir = ARANMath.Modulus(intermDir - beta, ARANMath.C_2xPI);
            }

            if (d3 > 0)
            {
                d3Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], finalDir, d3));
                d3Centreline.Add(centreline[centreline.Count - 1]);
            }

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepCentrelineElement);
            VMManager.Instance.StepCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(centreline, 255, 2);

            GeometryOperators geomOper = new GeometryOperators();

            VMManager.Instance.StepCentreline = centreline;
            if (centreline.Count > 1)
            {        
                MultiPolygon bufferArea = new MultiPolygon();
                var centrelineGeometry = ConvertToJtsGeo.FromGeometry(centreline);
                var buffer = centrelineGeometry.Buffer(VMManager.Instance.CorridorSemiWidth, GeoAPI.Operations.Buffer.BufferStyle.CapButt);
                bufferArea.Add(ConvertFromJtsGeo.ToGeometry(buffer) as Polygon);

                buffer = centrelineGeometry.Buffer(VMManager.Instance.VM_TurnRadius, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                var visibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                centrelineGeometry = ConvertToJtsGeo.FromGeometry(d1Centreline);
                buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                var d1VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                centrelineGeometry = ConvertToJtsGeo.FromGeometry(d2Centreline);
                buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                var d2VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                centrelineGeometry = ConvertToJtsGeo.FromGeometry(d3Centreline);
                buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                var d3VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                
                Aran.Geometries.MultiPolygon resultVisibilityArea = new MultiPolygon();

                resultVisibilityArea.Add(visibilityArea as Polygon);
                resultVisibilityArea.Add(d1VisibilityArea as Polygon);
                resultVisibilityArea.Add(d2VisibilityArea as Polygon);
                resultVisibilityArea.Add(d3VisibilityArea as Polygon);                
   
                VMManager.Instance.StepBufferPoly = bufferArea;
                VMManager.Instance.StepVisibilityPoly = resultVisibilityArea;
                VMManager.Instance.StepLength = centreline.Length;                
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElement);
                VMManager.Instance.StepVisibilityPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.StepVisibilityPoly, ARANFunctions.RGB(0, 255, 0), AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepBufferPolyElement);
                VMManager.Instance.StepBufferPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.StepBufferPoly, ARANFunctions.RGB(0, 0, 255), AranEnvironment.Symbols.eFillStyle.sfsCross);
            }
            VMManager.Instance.DivergenceSide = side1;
            VMManager.Instance.ConvergenceSide = side2;
            VMManager.Instance.DistanceTillDivergence = d1;
            VMManager.Instance.DistanceTillConvergence = d2;
            VMManager.Instance.DistanceTillEndPoint = d3;
            VMManager.Instance.FinalPosition = centreline[centreline.Count - 1];
            VMManager.Instance.FinalDirection = finalDir;
        }*/

        public String ContructStep(double d1, double intermDirection, double d2, double finalDirection, double d3, bool isFinalStep, bool isFromJSONFile)
        {
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceVFSelectionPolyElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedConvergenceVFElement);
            if(VMManager.Instance.TempMainPointElements != null && VMManager.Instance.RemoveTempMainPointElements)
                foreach (int elem in VMManager.Instance.TempMainPointElements)
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(elem);


            if (VMManager.Instance.StepTurnStartEndCrossLines != null)
                foreach (int elem in VMManager.Instance.StepTurnStartEndCrossLines)
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(elem);

            VMManager.Instance.TempMainPointElements = new List<int>();
            VMManager.Instance.StepTurnStartEndCrossLines = new List<int>();
            double intermDir = 0;
            double finalDir = 0;
            if (isFromJSONFile)
            {
                intermDir = intermDirection;
                finalDir = finalDirection;
            }
            else
            {
                intermDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, intermDirection), ARANMath.C_2xPI);
                finalDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, finalDirection), ARANMath.C_2xPI);
            }

            //VMManager.Instance.StepMainPointsElements.Add(new List<int>());
            //LineString d1Centreline = new LineString();
            //LineString d2Centreline = new LineString();
            //LineString d3Centreline = new LineString();

            double eps = 0.0175;

            TurnDirection side1;
            TurnDirection side2;

            double alpha = VMManager.Instance.InitialDirection - intermDir;            

            if (alpha > 0 || Math.Abs(alpha) > ARANMath.C_PI)
                side1 = TurnDirection.CW;
            else
                side1 = TurnDirection.CCW;
            
            alpha = Math.Abs(alpha);
            if (alpha > ARANMath.C_PI)
                alpha = ARANMath.C_2xPI - alpha;

            if (alpha < eps)
                alpha = 0;


            double beta = intermDir - finalDir;

            if (beta < 0)
            {
                if (VMManager.Instance.ConvergenceSide == TurnDirection.CW)
                {
                    beta = ARANMath.C_2xPI - Math.Abs(beta);
                }
                else
                {
                    beta = Math.Abs(beta);
                }
            }
            else
            {
                if (VMManager.Instance.ConvergenceSide == TurnDirection.CCW)
                {
                    beta = ARANMath.C_2xPI - Math.Abs(beta);
                }
                else
                {
                    beta = Math.Abs(beta);
                }
            }

            side2 = VMManager.Instance.ConvergenceSide;

            Point centPnt;
            LineString centreline = new LineString();

            centreline.Add(VMManager.Instance.InitialPosition);
            

            if (d1 > 0)
            {
                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP1"));
                //d1Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], VMManager.Instance.InitialDirection, d1));
                //d1Centreline.Add(centreline[centreline.Count - 1]);
                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP2"));
            }            

            if (side1 == TurnDirection.CCW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if (alpha > 0)
                {
                    LineString ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) + alpha, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CCW));
                    ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                }
            }
            else if (side1 == TurnDirection.CW)
            {
                centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                if (alpha > 0)
                {
                    LineString ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) - alpha, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CW));
                    ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                }
            }
            

            if (d2 > 0)
            {
                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP3"));
                //d2Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], intermDir, d2));
                //d2Centreline.Add(centreline[centreline.Count - 1]);
                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP4"));
                //constructConvergenceVFSelectionPoly(centreline[centreline.Count - 1], intermDir);
            }
            

            if (beta > 0)
            {
                if (side2 == TurnDirection.CCW)
                {
                    LineString ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                    centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) + beta, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CCW));
                    ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(finalDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(finalDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                }
                else if (side2 == TurnDirection.CW)
                {
                    LineString ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                    centPnt = ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(intermDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                    centreline.AddMultiPoint(ARANFunctions.CreateArcPrj(centPnt, centreline[centreline.Count - 1], ARANFunctions.PointAlongPlane(centPnt, ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(centPnt, centreline[centreline.Count - 1]) - beta, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius), TurnDirection.CW));
                    ls = new LineString();
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(finalDir - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    ls.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], ARANMath.Modulus(finalDir + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.CorridorSemiWidth));
                    VMManager.Instance.StepTurnStartEndCrossLines.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                }
            }

            if (d3 > 0)
            {
                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP5"));

                //d3Centreline.Add(centreline[centreline.Count - 1]);
                centreline.Add(ARANFunctions.PointAlongPlane(centreline[centreline.Count - 1], finalDir, d3));
                //d3Centreline.Add(centreline[centreline.Count - 1]);

                //VMManager.Instance.TempMainPointElements.Add(GlobalVars.gAranEnv.Graphics.DrawPointWithText(centreline[centreline.Count - 1], 255, "WP6"));
            }

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepCentrelineElement);
            VMManager.Instance.StepCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(centreline, 255, 2);

            GeometryOperators geomOper = new GeometryOperators();

            VMManager.Instance.StepCentreline = centreline;

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepBufferPolyElement);
            if (centreline.Count > 1)
            {
                MultiPolygon bufferArea = new MultiPolygon();
                var centrelineGeometry = ConvertToJtsGeo.FromGeometry(centreline);
                var buffer = centrelineGeometry.Buffer(VMManager.Instance.CorridorSemiWidth, GeoAPI.Operation.Buffer.BufferStyle.CapButt);
                bufferArea.Add(ConvertFromJtsGeo.ToGeometry(buffer) as Polygon);

                //buffer = centrelineGeometry.Buffer(VMManager.Instance.VM_TurnRadius, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                buffer = centrelineGeometry.Buffer(VMManager.Instance.MinVisibilityDistance, GeoAPI.Operation.Buffer.BufferStyle.CapRound);
                var visibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                //centrelineGeometry = ConvertToJtsGeo.FromGeometry(d1Centreline);
                //buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                //var d1VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                //centrelineGeometry = ConvertToJtsGeo.FromGeometry(d2Centreline);
                //buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                //var d2VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);

                //centrelineGeometry = ConvertToJtsGeo.FromGeometry(d3Centreline);
                //buffer = centrelineGeometry.Buffer(VMManager.Instance.VisibilityDistance, GeoAPI.Operations.Buffer.BufferStyle.CapRound);
                //var d3VisibilityArea = ConvertFromJtsGeo.ToGeometry(buffer);


                Aran.Geometries.MultiPolygon resultVisibilityArea = new MultiPolygon();
                
                resultVisibilityArea.Add(visibilityArea as Polygon);
                //resultVisibilityArea.Add(d1VisibilityArea as Polygon);
                //resultVisibilityArea.Add(d2VisibilityArea as Polygon);
                //resultVisibilityArea.Add(d3VisibilityArea as Polygon);

                VMManager.Instance.StepBufferPoly = bufferArea;
                VMManager.Instance.StepVisibilityPoly = resultVisibilityArea;
                VMManager.Instance.StepLength = centreline.Length;

                if (!isFromJSONFile)
                {
                    VMManager.Instance.StepVisibilityPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.StepVisibilityPoly, ARANFunctions.RGB(0, 255, 0), AranEnvironment.Symbols.eFillStyle.sfsDiagonalCross);
                    if (!VMManager.Instance.isTrackVisibilityBufferVisible)
                        GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.StepVisibilityPolyElement, false);
                }


                VMManager.Instance.StepBufferPolyElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.StepBufferPoly, ARANFunctions.RGB(30, 144, 255), AranEnvironment.Symbols.eFillStyle.sfsCross);
            }
            VMManager.Instance.DivergenceSide = side1;
            VMManager.Instance.ConvergenceSide = side2;
            VMManager.Instance.DistanceTillDivergence = d1;
            VMManager.Instance.DistanceTillConvergence = d2;
            VMManager.Instance.DistanceTillEndPoint = d3;
            VMManager.Instance.FinalPosition = centreline[centreline.Count - 1];
            VMManager.Instance.IntermediateDirection = intermDir;
            VMManager.Instance.FinalDirection = finalDir;

            bool alphaTurn = false;
            bool betaTurn = false;
            if(Math.Abs(intermDir - VMManager.Instance.InitialDirection) > eps)
                alphaTurn = true;
            if (Math.Abs(finalDir - intermDir) > eps)
                betaTurn = true;

            VMManager.Instance.RemoveTempMainPointElements = true;
            return constructStepDescription(d1, intermDirection, alphaTurn, side1, d2, finalDirection, betaTurn, side2, d3, isFinalStep);
        }

        private String constructStepDescription(double d1, double alphaAzt, bool alphaTurn, TurnDirection alphaSide, double d2, double betaAzt, bool betaTurn, TurnDirection betaSide, double d3, bool isFinalStep)
        {
            String descMessage = "";
            double t1;
            double t2;
            double t3;

            if (d1 > 0)
            {
                t1 = d1 / VMManager.Instance.VM_TASWind;
                descMessage += "Distance between segment start point and divergence start point: " + Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(Math.Round(d1), eRoundMode.NONE), 3) + " " + GlobalVars.unitConverter.DistanceUnit + System.Environment.NewLine + "Flight time till divergence start point: " + Math.Round(t1) + " s " + System.Environment.NewLine + System.Environment.NewLine;
            }

            if (alphaTurn)
            {
                if(alphaSide == TurnDirection.CCW)
                    descMessage += "Turn to the left to " + (alphaAzt - GlobalVars.CurrADHP.MagVar) + "° (MAG BRG) " + System.Environment.NewLine + System.Environment.NewLine;
                else if(alphaSide == TurnDirection.CW)
                    descMessage += "Turn to the right to " + (alphaAzt - GlobalVars.CurrADHP.MagVar) + "° (MAG BRG) " + System.Environment.NewLine + System.Environment.NewLine;
            }

            if (d2 > 0)
            {
                if (isFinalStep)
                {
                    t2 = d2 / VMManager.Instance.FinalSegmentTASWind;
                    descMessage += "Finel segment length: " + Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(Math.Round(d2), eRoundMode.NONE), 3) + " " + GlobalVars.unitConverter.DistanceUnit + System.Environment.NewLine + "Final segment flight time: " + Math.Round(t2) + " s " + System.Environment.NewLine + System.Environment.NewLine;
                }
                else
                {
                    t2 = d2 / VMManager.Instance.VM_TASWind;
                    descMessage += "Distance between divergence end point and convergence start point: " + Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(Math.Round(d2), eRoundMode.NONE), 3) + " " + GlobalVars.unitConverter.DistanceUnit + System.Environment.NewLine + "Flight time between divergence end point and convergence start point: " + Math.Round(t2) + " s " + System.Environment.NewLine + System.Environment.NewLine;
                }                    
            }

            if (betaTurn)
            {
                if (betaSide == TurnDirection.CCW)
                    descMessage += "Turn to the left to " + (betaAzt - GlobalVars.CurrADHP.MagVar) + "° (MAG BRG) " + System.Environment.NewLine + System.Environment.NewLine;
                else if (betaSide == TurnDirection.CW)
                    descMessage += "Turn to the right to " + (betaAzt - GlobalVars.CurrADHP.MagVar) + "° (MAG BRG) " + System.Environment.NewLine + System.Environment.NewLine;
            }

            if (d3 > 0)
            {
                t3 = d3 / VMManager.Instance.VM_TASWind;
                descMessage += "Distance between convergence end point and segment end point: " + Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(Math.Round(d3), eRoundMode.NONE), 3) + " " + GlobalVars.unitConverter.DistanceUnit + System.Environment.NewLine + "Flight time till segment end point: " + Math.Round(t3) + " s ";
            }
            return descMessage;
        }


        //public void constructDivergenceVFSelectionPoly(Point startPnt, double startDir)
        //{
        //    VMManager.Instance.ConvergenceVFSelectionPoly = new Polygon();
        //    double dir = ARANMath.Modulus(startDir - ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    Point pnt = ARANFunctions.PointAlongPlane(startPnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir + ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    Point pnt2 = ARANFunctions.PointAlongPlane(startPnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    /*pnt = ARANFunctions.PointAlongPlane(pnt, startDir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir + ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    pnt = ARANFunctions.PointAlongPlane(pnt, dir, 2 * VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir - ARANMath.C_PI, ARANMath.C_2xPI);
        //    pnt = ARANFunctions.PointAlongPlane(pnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);*/


        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcPrj(startPnt, pnt, pnt2, TurnDirection.CCW));

        //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceVFSelectionPolyElement);

        //    VMManager.Instance.ConvergenceVFSelectionPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(VMManager.Instance.ConvergenceVFSelectionPoly, ARANFunctions.RGB(0, 255, 0), AranEnvironment.Symbols.eFillStyle.sfsCross);


        //    findVFsWithinDivergenceVFSelectionPoly();
        //}



        //public void findVFsWithinDivergenceVFSelectionPoly()
        //{
        //    VMManager.Instance.ConvergenceVFList.Clear();
        //    VMManager.Instance.ConvergenceVFList.Add(null);
        //    VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvergenceVFSelectionPoly;
        //    for (int i = 0; i < VMManager.Instance.AllVisualFeatures.Count; i++)
        //    {
        //        if (VMManager.Instance.GeomOper.Contains(VMManager.Instance.AllVisualFeatures[i].pShape))
        //            VMManager.Instance.ConvergenceVFList.Add(VMManager.Instance.AllVisualFeatures[i]);
        //    }

        //    if (VMManager.Instance.ConvergenceVFList.Count > 1)
        //        nsf.btn_Ok.Enabled = true;
        //    else
        //        nsf.btn_Ok.Enabled = false;
        //}





        //public void constructConvergenceVFSelectionPoly(Point startPnt, double startDir) {
        //    VMManager.Instance.ConvergenceVFSelectionPoly = new Polygon();
        //    double dir = ARANMath.Modulus(startDir - ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    Point pnt = ARANFunctions.PointAlongPlane(startPnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir + ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    Point pnt2 = ARANFunctions.PointAlongPlane(startPnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    /*pnt = ARANFunctions.PointAlongPlane(pnt, startDir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir + ARANMath.C_PI_2, ARANMath.C_2xPI);
        //    pnt = ARANFunctions.PointAlongPlane(pnt, dir, 2 * VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);

        //    dir = ARANMath.Modulus(startDir - ARANMath.C_PI, ARANMath.C_2xPI);
        //    pnt = ARANFunctions.PointAlongPlane(pnt, dir, VMManager.Instance.MinVisibilityDistance);
        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.Add(pnt);*/


        //    VMManager.Instance.ConvergenceVFSelectionPoly.ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcPrj(startPnt, pnt, pnt2, TurnDirection.CCW));

        //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceVFSelectionPolyElement);

        //    VMManager.Instance.ConvergenceVFSelectionPolyElement = GlobalVars.gAranEnv.Graphics.DrawPolygon(VMManager.Instance.ConvergenceVFSelectionPoly, ARANFunctions.RGB(0, 255, 0), AranEnvironment.Symbols.eFillStyle.sfsCross);


        //    findVFsWithinConvergenceVFSelectionPoly();
        //}



        //public void findVFsWithinConvergenceVFSelectionPoly()
        //{
        //    VMManager.Instance.ConvergenceVFList.Clear();
        //    VMManager.Instance.ConvergenceVFList.Add(null);
        //    VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvergenceVFSelectionPoly;
        //    for (int i = 0; i < VMManager.Instance.AllVisualFeatures.Count; i++)
        //    {
        //        if (VMManager.Instance.GeomOper.Contains(VMManager.Instance.AllVisualFeatures[i].pShape))
        //            VMManager.Instance.ConvergenceVFList.Add(VMManager.Instance.AllVisualFeatures[i]);
        //    }

        //    if (VMManager.Instance.ConvergenceVFList.Count > 1)
        //        nsf.btn_Ok.Enabled = true;
        //    else
        //        nsf.btn_Ok.Enabled = false;
        //}
    }
}
