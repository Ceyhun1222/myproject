using PVT.Model;

namespace PVT.Graphics
{
    class ObstacleAssestmentAreaHoldingDrawer : HoldingDrawerBase
    {
        public int AreaIndex { get; set; }

        private ObstacleAssessmentArea GetArea(HoldingPattern pattern)
        {
            return pattern.AssessmentAreas.Count - 1 < AreaIndex ? null : pattern.AssessmentAreas[AreaIndex];
        }

        protected override DrawObject DrawHoldingPattern(HoldingPattern pattern)
        {
            var identifier = pattern.Identifier;
            var color = new Model.Drawing.Color(System.Windows.Media.Colors.Black).ToRGB();
            var size = 2;
            var area = GetArea(pattern);
            if(area == null)
                return new DrawObject(identifier);
            return DrawAssessmentArea(identifier, area, color, size);
        }
    }
}