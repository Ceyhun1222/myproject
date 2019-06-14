
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries.Operators;
using Aran.Panda.Common;
using Aran.Geometries;

namespace Aran.Panda.VisualManoeuvring.FormHelpers
{
    class NS_Page4_Helper
    {
        GeometryOperators geomOper;
        List<double> diffAngles;

        public List<string> GetVFsWithinPoly()
        {
            List<string> names = new List<string>();
            diffAngles = new List<double>();
            VMManager.Instance.FinalDirections = new List<double>();

            VMManager.Instance.VFsWithinPoly = new List<VM_VisualFeature>();
            names.Add("");
            diffAngles.Add(0);
            geomOper = new GeometryOperators();
            double diffAngle;
            for (int i = 0; i < VMManager.Instance.AllVisualFeatures.Length; i++)
            {
                if (geomOper.Contains(VMManager.Instance.ConvergencePoly, VMManager.Instance.AllVisualFeatures[i].pShape))
                {
                    diffAngle = ARANMath.Modulus(VMManager.Instance.IntermediateDirection - ARANFunctions.ReturnAngleInRadians(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.AllVisualFeatures[i].pShape), ARANMath.C_2xPI);
                    if (diffAngle > ARANMath.C_PI)
                        diffAngle = ARANMath.C_2xPI - diffAngle;
                    if (ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.AllVisualFeatures[i].pShape) >=
                        VMManager.Instance.VM_TurnRadius * Math.Tan(diffAngle / 2))
                    {
                        names.Add(VMManager.Instance.AllVisualFeatures[i].Name);
                        VMManager.Instance.FinalDirections.Add(ARANFunctions.ReturnAngleInRadians(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.AllVisualFeatures[i].pShape));
                        VMManager.Instance.VFsWithinPoly.Add(VMManager.Instance.AllVisualFeatures[i]);
                        diffAngles.Add(diffAngle);
                    }
                }
            }

            return names;
        }

        public void ConstructConvergenceLine(int idx)
        {
            VMManager.Instance.ConvergenceLine = new LineString();
            if(idx == 0)
            {
                diffAngles[0] = Math.Abs(VMManager.Instance.IntermediateDirection - VMManager.Instance.FinalDirection);
                if (diffAngles[0] > ARANMath.C_PI)
                    diffAngles[0] = ARANMath.C_2xPI - diffAngles[0];
            }
            double dist = VMManager.Instance.VM_TurnRadius * Math.Tan(diffAngles[idx] / 2);
            Point turnStartPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.ConvergenceFlyByPoint, ARANMath.Modulus(VMManager.Instance.IntermediateDirection - ARANMath.C_PI, ARANMath.C_2xPI), dist);
            Point turnEndPnt = null;
            SideDirection side = SideDirection.sideLeft;
            if (idx > 0)
                side = ARANMath.SideDef(VMManager.Instance.ConvergenceStartPoint, VMManager.Instance.IntermediateDirection, VMManager.Instance.VFsWithinPoly[idx - 1].pShape);
            else
                side = ARANMath.SideDef(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.IntermediateDirection, ARANFunctions.PointAlongPlane(VMManager.Instance.ConvergenceFlyByPoint, VMManager.Instance.FinalDirection, dist));
            double mydist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.ConvergenceStartPoint, VMManager.Instance.ConvergenceFlyByPoint);
            Point cent = null;
            VMManager.Instance.ConvergenceLine.Add(VMManager.Instance.ConvergenceStartPoint);
            if (side == SideDirection.sideLeft)
            {
                cent = ARANFunctions.PointAlongPlane(turnStartPnt, ARANMath.Modulus(VMManager.Instance.IntermediateDirection + ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                turnEndPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.ConvergenceFlyByPoint, ARANMath.Modulus(VMManager.Instance.IntermediateDirection + diffAngles[idx], ARANMath.C_2xPI), dist);
                VMManager.Instance.ConvergenceLine.AddMultiPoint(ARANFunctions.CreateArcPrj(cent, turnStartPnt, turnEndPnt, TurnDirection.CCW));
            }
            else if (side == SideDirection.sideRight)
            {
                cent = ARANFunctions.PointAlongPlane(turnStartPnt, ARANMath.Modulus(VMManager.Instance.IntermediateDirection - ARANMath.C_PI_2, ARANMath.C_2xPI), VMManager.Instance.VM_TurnRadius);
                turnEndPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.ConvergenceFlyByPoint, ARANMath.Modulus(VMManager.Instance.IntermediateDirection - diffAngles[idx], ARANMath.C_2xPI), dist);
                VMManager.Instance.ConvergenceLine.AddMultiPoint(ARANFunctions.CreateArcPrj(cent, turnStartPnt, turnEndPnt, TurnDirection.CW));
            }

            if (idx > 0 && VMManager.Instance.isFinalStep && VMManager.Instance.VFsWithinPoly[idx - 1].Name.Equals("THR" + VMManager.Instance.SelectedRWY.Name))
            {
                VMManager.Instance.ConvergenceLine.Add(VMManager.Instance.VFsWithinPoly[idx - 1].pShape);
            }


            if (VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1] > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1]);
            VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1] = GlobalVars.gAranEnv.Graphics.DrawLineString(VMManager.Instance.ConvergenceLine, 255, 2);            
            VMManager.Instance.FinalPosition = VMManager.Instance.ConvergenceLine[VMManager.Instance.ConvergenceLine.Count - 1];
        }        

        public void onClose()
        {
            if (VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1] > -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1]);
                VMManager.Instance.ConvergenceLineElements[VMManager.Instance.ConvergenceLineElements.Count - 1] = -1;
            }
        }
    }
}
