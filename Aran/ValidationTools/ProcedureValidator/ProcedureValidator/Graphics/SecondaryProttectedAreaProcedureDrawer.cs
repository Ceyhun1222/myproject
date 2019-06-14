using PVT.Model;

namespace PVT.Graphics
{
    class SecondaryProttectedAreaProcedureDrawer : ObstacleAssestmentAreaProcedureDrawer
    {
        protected override ObstacleAssessmentArea GetArea(SegmentLeg leg)
        {
            return leg.SecondaryProttectedArea;
        }
    }
}
