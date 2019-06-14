using PVT.Model;

namespace PVT.Graphics
{
    public class PointHoldingDrawer : HoldingDrawerBase
    {
        protected override DrawObject DrawHoldingPattern(HoldingPattern pattern)
        {
            var drawObject = new DrawObject(pattern.Identifier);
            if (pattern.HoldingPoint != null)
                DrawPoint(pattern.HoldingPoint.Value.PointChoice, drawObject);
            return drawObject;
        }
    }
}