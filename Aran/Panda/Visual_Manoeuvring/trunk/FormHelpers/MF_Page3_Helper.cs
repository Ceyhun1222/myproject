using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using GeoAPI.Geometries;
using Aran.Converters.ConverterJtsGeom;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    class MF_Page3_Helper
    {
        public void ConstructCircuits(out double maxDistance)
        {
            #region Vars
            double Deg90InRad = ARANMath.DegToRad(90);
            double Deg180InRad = ARANMath.DegToRad(180);

            double dirRWYTrueBearing = GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], VMManager.Instance.SelectedRWY.TrueBearing);
            double distBetweenThrEnd = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptEnd]);

            Point pnt1;
            Point pnt2;
            Point pnt3;
            Point pnt4;
            Point centPnt;
            Point tempPnt;
            Point tempPnt2;
            #endregion

            #region Construct Final segment
            VMManager.Instance.FinalSegmentPolygon = new MultiPolygon();
            VMManager.Instance.FinalSegmentPolygon.Add(new Polygon());
            VMManager.Instance.FinalSegmentCentreline = new LineString();

            pnt1 = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing + Deg90InRad, VMManager.Instance.CorridorSemiWidth);
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            pnt3 = ARANFunctions.PointAlongPlane(pnt2, dirRWYTrueBearing - Deg90InRad, 2 * VMManager.Instance.CorridorSemiWidth);
            pnt4 = ARANFunctions.PointAlongPlane(pnt3, dirRWYTrueBearing, VMManager.Instance.FinalSegmentLength);

            VMManager.Instance.FinalSegmentPolygon[0].ExteriorRing.Add(pnt1);
            VMManager.Instance.FinalSegmentPolygon[0].ExteriorRing.Add(pnt2);
            VMManager.Instance.FinalSegmentPolygon[0].ExteriorRing.Add(pnt3);
            VMManager.Instance.FinalSegmentPolygon[0].ExteriorRing.Add(pnt4);

            VMManager.Instance.FinalSegmentCentreline.Add(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR]);
            tempPnt2 = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.FinalSegmentCentreline.Add(tempPnt2);

            VMManager.Instance.FinalSegmentStartPoint = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            //VMManager.Instance.PreFinalSegmentStartPoint = VMManager.Instance.FinalSegmentStartPoint;
            VMManager.Instance.FinalSegmentStartPointDirection = dirRWYTrueBearing;
            #endregion

            #region Construct Righthand circuit
            VMManager.Instance.RighthandCircuitCentreline = new LineString();
            VMManager.Instance.RighthandCircuitPolygon = new MultiPolygon();
            VMManager.Instance.RighthandCircuitPolygon.Add(new Polygon());

            pnt1 = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing + Deg90InRad, VMManager.Instance.CorridorSemiWidth);
            VMManager.Instance.RighthandCircuitPolygon[0].ExteriorRing.Add(pnt1);

            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.RighthandCircuitPolygon[0].ExteriorRing.Add(pnt2);

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg90InRad, 2 * VMManager.Instance.CorridorSemiWidth + 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg90InRad, VMManager.Instance.CorridorSemiWidth + VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.RighthandCircuitPolygon[0].ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, pnt1, pnt2, TurnDirection.CCW));

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing, distBetweenThrEnd + VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.RighthandCircuitPolygon[0].ExteriorRing.Add(pnt2);

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing + Deg90InRad, 2 * VMManager.Instance.CorridorSemiWidth + 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing + Deg90InRad, VMManager.Instance.CorridorSemiWidth + VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.RighthandCircuitPolygon[0].ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, pnt1, pnt2, TurnDirection.CCW));


            tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing - Deg90InRad, 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing - Deg90InRad, VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.RighthandCircuitCentreline.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, tempPnt, tempPnt2, TurnDirection.CCW));

            VMManager.Instance.rightExtendedDownwindEndIndex = VMManager.Instance.RighthandCircuitCentreline.Count - 1;

            double distToCentre = Math.Sqrt(VMManager.Instance.FinalSegmentLength * VMManager.Instance.FinalSegmentLength + VMManager.Instance.VM_TurnRadius * VMManager.Instance.VM_TurnRadius);
            maxDistance = distToCentre + VMManager.Instance.VM_TurnRadius + VMManager.Instance.CorridorSemiWidth;

            tempPnt = tempPnt2;
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing, distBetweenThrEnd + VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.RighthandCircuitCentreline.Add(tempPnt2);

            VMManager.Instance.rightExtendedDownwindStartIndex = VMManager.Instance.RighthandCircuitCentreline.Count - 1;

            tempPnt = tempPnt2;
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing + Deg90InRad, 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing + Deg90InRad, VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.RighthandCircuitCentreline.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, tempPnt, tempPnt2, TurnDirection.CCW));
            #endregion

            #region Construct Lefthand circuit
            VMManager.Instance.LefthandCircuitCentreline = new LineString();
            VMManager.Instance.LefthandCircuitPolygon = new MultiPolygon();
            VMManager.Instance.LefthandCircuitPolygon.Add(new Polygon());

            pnt1 = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing - Deg90InRad, VMManager.Instance.CorridorSemiWidth);
            VMManager.Instance.LefthandCircuitPolygon[0].ExteriorRing.Add(pnt1);

            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.LefthandCircuitPolygon[0].ExteriorRing.Add(pnt2);

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing + Deg90InRad, 2 * VMManager.Instance.CorridorSemiWidth + 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing + Deg90InRad, VMManager.Instance.CorridorSemiWidth + VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.LefthandCircuitPolygon[0].ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, pnt1, pnt2, TurnDirection.CW));

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing, distBetweenThrEnd + VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.LefthandCircuitPolygon[0].ExteriorRing.Add(pnt2);

            pnt1 = pnt2;
            pnt2 = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg90InRad, 2 * VMManager.Instance.CorridorSemiWidth + 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(pnt1, dirRWYTrueBearing - Deg90InRad, VMManager.Instance.CorridorSemiWidth + VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.LefthandCircuitPolygon[0].ExteriorRing.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, pnt1, pnt2, TurnDirection.CW));


            tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], dirRWYTrueBearing - Deg180InRad, VMManager.Instance.FinalSegmentLength);
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing + Deg90InRad, 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing + Deg90InRad, VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.LefthandCircuitCentreline.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, tempPnt, tempPnt2, TurnDirection.CW));

            VMManager.Instance.leftExtendedDownwindEndIndex = VMManager.Instance.LefthandCircuitCentreline.Count - 1;

            tempPnt = tempPnt2;
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing, distBetweenThrEnd + VMManager.Instance.FinalSegmentLength);
            VMManager.Instance.LefthandCircuitCentreline.Add(tempPnt2);

            VMManager.Instance.leftExtendedDownwindStartIndex = VMManager.Instance.LefthandCircuitCentreline.Count - 1;

            tempPnt = tempPnt2;
            tempPnt2 = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing - Deg90InRad, 2 * VMManager.Instance.VM_TurnRadius);
            centPnt = ARANFunctions.PointAlongPlane(tempPnt, dirRWYTrueBearing - Deg90InRad, VMManager.Instance.VM_TurnRadius);
            VMManager.Instance.LefthandCircuitCentreline.AddMultiPoint(ARANFunctions.CreateArcAsPartPrj(centPnt, tempPnt, tempPnt2, TurnDirection.CW));
            #endregion

            VMManager.Instance.RighthandCircuitOCA = 0;
            VMManager.Instance.LefthandCircuitOCA = 0;
            VMManager.Instance.RighthandCircuitOCH = 0;
            VMManager.Instance.LefthandCircuitOCH = 0;
            VMManager.Instance.RighthandCircuitObstaclesList = new List<VM_VerticalStructure>();
            VMManager.Instance.LefthandCircuitObstaclesList = new List<VM_VerticalStructure>();         
        }

        public void assessCircuitObstacles()
        {
            VMManager.Instance.RighthandCircuitOCA = CalculateCircuitOCA(CircuitSide.Righthand);
            VMManager.Instance.LefthandCircuitOCA = CalculateCircuitOCA(CircuitSide.Lefthand);
            VMManager.Instance.RighthandCircuitOCH = VMManager.Instance.RighthandCircuitOCA - GlobalVars.CurrADHP.Elev;
            VMManager.Instance.LefthandCircuitOCH = VMManager.Instance.LefthandCircuitOCA - GlobalVars.CurrADHP.Elev;
        }

        public void DrawCircuits()
        {
            #region Delete all graphics
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.FinalSegmentPolygonElement);
            VMManager.Instance.FinalSegmentPolygonElement = -1;
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.FinalSegmentCentrelineElement);
            VMManager.Instance.FinalSegmentCentrelineElement = -1;
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.RighthandCircuitPolygonElement);
            VMManager.Instance.RighthandCircuitPolygonElement = -1;
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.RighthandCircuitCentrelineElement);
            VMManager.Instance.RighthandCircuitCentrelineElement = -1;
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.LefthandCircuitPolygonElement);
            VMManager.Instance.LefthandCircuitPolygonElement = -1;
            GlobalVars.gAranEnv.Graphics.DeleteGraphic(VMManager.Instance.LefthandCircuitCentrelineElement);
            VMManager.Instance.LefthandCircuitCentrelineElement = -1;
            #endregion

            #region Draw Final Segment
            //if (drawFinalSegmentPolygon)
            VMManager.Instance.FinalSegmentPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.FinalSegmentPolygon, ARANFunctions.RGB(0, 153, 76) /*green*/, AranEnvironment.Symbols.eFillStyle.sfsForwardDiagonal);
            VMManager.Instance.isFinalSegmentPolygonVisible = true;
            //if (drawFinalSegmentCentreline)
            VMManager.Instance.FinalSegmentCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.FinalSegmentCentreline, ARANFunctions.RGB(0, 153, 76) /*green*/, 1);
            VMManager.Instance.isFinalSegmentCentrelineVisible = true;
            #endregion

            #region Draw Righthand and Lefthand Circuits
            if (VMManager.Instance.RighthandCircuitOCH < VMManager.Instance.LefthandCircuitOCH)
                VMManager.Instance.bestCircuit = BestCircuit.Righthand;
            else if (VMManager.Instance.LefthandCircuitOCH < VMManager.Instance.RighthandCircuitOCH)
                VMManager.Instance.bestCircuit = BestCircuit.Lefthand;
            else
                VMManager.Instance.bestCircuit = BestCircuit.Both;

            if (VMManager.Instance.bestCircuit == BestCircuit.Both)
            {
                //if(drawRighthandCircuitPolygon)
                VMManager.Instance.RighthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.RighthandCircuitPolygon, ARANFunctions.RGB(0, 153, 76) /*green*/, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                VMManager.Instance.isRighthandCircuitPolygonVisible = true;
                //if(drawRighthandCircuitCentreline)
                VMManager.Instance.RighthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.RighthandCircuitCentreline, ARANFunctions.RGB(0, 153, 76) /*green*/, 1);
                VMManager.Instance.isRighthandCircuitCentrelineVisible = true;
                //if(drawLefthandCircuitPolygon)
                VMManager.Instance.LefthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.LefthandCircuitPolygon, ARANFunctions.RGB(0, 153, 76) /*green*/, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                VMManager.Instance.isLefthandCircuitPolygonVisible = true;
                //if(drawLefthandCircutCentreline)
                VMManager.Instance.LefthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.LefthandCircuitCentreline, ARANFunctions.RGB(0, 153, 76) /*green*/, 1);
                VMManager.Instance.isLefthandCircuitCentrelineVisible = true;
            }
            else
            {
                if (VMManager.Instance.bestCircuit == BestCircuit.Righthand)
                {
                    //if (drawRighthandCircuitPolygon)
                    VMManager.Instance.RighthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.RighthandCircuitPolygon, ARANFunctions.RGB(0, 153, 76) /*green*/, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                    VMManager.Instance.isRighthandCircuitPolygonVisible = true;
                    //if (drawRighthandCircuitCentreline)
                    VMManager.Instance.RighthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.RighthandCircuitCentreline, ARANFunctions.RGB(0, 153, 76) /*green*/, 1);
                    VMManager.Instance.isRighthandCircuitCentrelineVisible = true;
                    //if (drawLefthandCircuitPolygon)
                    VMManager.Instance.LefthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.LefthandCircuitPolygon, ARANFunctions.RGB(255, 0, 0), AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                    VMManager.Instance.isLefthandCircuitPolygonVisible = true;
                    //if (drawLefthandCircutCentreline)
                    VMManager.Instance.LefthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.LefthandCircuitCentreline, ARANFunctions.RGB(255, 0, 0), 1);
                    VMManager.Instance.isLefthandCircuitCentrelineVisible = true;
                }
                else
                {
                    //if (drawRighthandCircuitPolygon)
                    VMManager.Instance.RighthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.RighthandCircuitPolygon, ARANFunctions.RGB(255, 0, 0), AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                    VMManager.Instance.isRighthandCircuitPolygonVisible = true;
                    //if (drawRighthandCircuitCentreline)
                    VMManager.Instance.RighthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.RighthandCircuitCentreline, ARANFunctions.RGB(255, 0, 0), 1);
                    VMManager.Instance.isRighthandCircuitCentrelineVisible = true;
                    //if (drawLefthandCircuitPolygon)
                    VMManager.Instance.LefthandCircuitPolygonElement = GlobalVars.gAranEnv.Graphics.DrawMultiPolygon(VMManager.Instance.LefthandCircuitPolygon, ARANFunctions.RGB(0, 153, 76) /*green*/, AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                    VMManager.Instance.isLefthandCircuitPolygonVisible = true;
                    //if (drawLefthandCircutCentreline)
                    VMManager.Instance.LefthandCircuitCentrelineElement = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.LefthandCircuitCentreline, ARANFunctions.RGB(0, 153, 76) /*green*/, 1);
                    VMManager.Instance.isLefthandCircuitCentrelineVisible = true;
                }
            }
            #endregion

        }

        private double CalculateCircuitOCA(CircuitSide side)
        {
            double maxAltitude = -100000;            

            if (side == CircuitSide.Righthand)
            {
                VMManager.Instance.RighthandCircuitOCA = -100000;
                VMManager.Instance.RighthandCircuitOCH = -100000;
                VMManager.Instance.RighthandCircuitObstaclesList = new List<VM_VerticalStructure>();
                //VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.RighthandCircuitPolygon;

                //for (int i = 0; i < VMManager.Instance.AllObstacles.Count; i++)
                //{
                //    var vs = new VM_VerticalStructure(VMManager.Instance.AllObstacles[i].VerticalStructure, VMManager.Instance.AllObstacles[i].PartGeoPrjList);
                //    if (vs.PartGeometries.Count == 0)
                //        continue;

                //    if (MaxAltitude < vs.Elevation)
                //    {
                //        MaxAltitude = vs.Elevation;
                //        VMManager.Instance.RighthandCircuitHighestObstacleIndex = VMManager.Instance.RighthandCircuitObstaclesList.Count;
                //    }

                //    VMManager.Instance.RighthandCircuitObstaclesList.Add(vs);                    
                //}
                //VMManager.Instance.GeomOper = new JtsGeometryOperators();

                maxAltitude = Functions.MaxObstacleElevationInPoly(VMManager.Instance.AllObstacles, out VMManager.Instance.RighthandCircuitObstaclesList, VMManager.Instance.RighthandCircuitPolygon, out VMManager.Instance.RighthandCircuitHighestObstacleIndex);
            }
            else
            {
                VMManager.Instance.LefthandCircuitOCA = -100000;
                VMManager.Instance.LefthandCircuitOCH = -100000;
                VMManager.Instance.LefthandCircuitObstaclesList = new List<VM_VerticalStructure>();
                //VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.LefthandCircuitPolygon;

                //for (int i = 0; i < VMManager.Instance.AllObstacles.Count; i++)
                //{
                //    var vs = new VM_VerticalStructure(VMManager.Instance.AllObstacles[i].VerticalStructure, VMManager.Instance.AllObstacles[i].PartGeoPrjList);
                //    if (vs.PartGeometries.Count == 0)
                //        continue;

                //    if (maxAltitude < vs.Elevation)
                //    {
                //        maxAltitude = vs.Elevation;
                //        VMManager.Instance.LefthandCircuitHighestObstacleIndex = VMManager.Instance.LefthandCircuitObstaclesList.Count;
                //    }

                //    VMManager.Instance.LefthandCircuitObstaclesList.Add(vs);
                //}
                //VMManager.Instance.GeomOper = new JtsGeometryOperators();

                maxAltitude = Functions.MaxObstacleElevationInPoly(VMManager.Instance.AllObstacles, out VMManager.Instance.LefthandCircuitObstaclesList, VMManager.Instance.LefthandCircuitPolygon, out VMManager.Instance.LefthandCircuitHighestObstacleIndex);
            }

            double OCA = maxAltitude + VMManager.Instance.MOC;
            if (OCA < VMManager.Instance.MinOCA)
                OCA = VMManager.Instance.MinOCA;

            return OCA;
        }
    }
}
