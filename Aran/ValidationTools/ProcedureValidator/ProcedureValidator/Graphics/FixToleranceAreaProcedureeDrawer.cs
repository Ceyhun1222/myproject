using PVT.Model;
using Aran.Geometries;

namespace PVT.Graphics
{
    class FixToleranceAreaProcedureeDrawer : ProcedureDrawerBase
    {
        protected override DrawObject DrawLeg(SegmentLeg leg)
        {
            var drawObject = new DrawObject(leg.Identifier);
            if (leg.StartPoint != null)
            {
                DrawFix(leg.StartPoint, drawObject);
            }

            if (leg.EndPoint != null)
            {
                DrawFix(leg.EndPoint, drawObject);
            }
            return drawObject;
        }


       
    }
}
