using System;
using PVT.Model;

namespace PVT.Graphics
{
    public class PointProcedureDrawer : ProcedureDrawerBase
    {

        protected override DrawObject DrawLeg(SegmentLeg leg)
        {
            var drawObject = new DrawObject(leg.Identifier);
            if (leg.StartPoint != null)
                DrawPoint(leg.StartPoint.PointChoice, drawObject);
            if (leg.EndPoint != null)
                DrawPoint(leg.EndPoint.PointChoice, drawObject);
            return drawObject;
        }

    }
}
