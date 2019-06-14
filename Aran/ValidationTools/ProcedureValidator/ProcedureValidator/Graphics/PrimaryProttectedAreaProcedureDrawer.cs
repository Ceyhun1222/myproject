using PVT.Model;

namespace PVT.Graphics
{
    class PrimaryProttectedAreaProcedureDrawer : ObstacleAssestmentAreaProcedureDrawer
    {
        protected override ObstacleAssessmentArea GetArea(SegmentLeg leg)
        {
            return leg.PrimaryProttectedArea;
        }
    }
}
