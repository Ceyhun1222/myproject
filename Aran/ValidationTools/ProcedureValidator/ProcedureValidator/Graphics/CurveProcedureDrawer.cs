using System;
using PVT.Model;
using Aran.Geometries;

namespace PVT.Graphics
{
    public class CurveProcedureDrawer : ProcedureDrawerBase
    {
        protected override DrawObject DrawLeg(SegmentLeg leg)
        {
            if (leg.NominalTrack != null)
            {
                return DrawCurve(leg.NominalTrack, leg.Original.Identifier);
            }

            return new DrawObject(leg.Identifier);
        }

      
    }
}
