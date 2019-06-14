using System;
using Aran.Geometries;
using PVT.Model;
using System.Collections.Generic;
using System.Linq;

namespace PVT.Graphics
{
    abstract class ObstacleAssestmentAreaProcedureDrawer : ProcedureDrawerBase
    {
        protected override DrawObject DrawLeg(SegmentLeg leg)
        {
            var identifier = leg.Identifier;
            var color = new Model.Drawing.Color(System.Windows.Media.Colors.Black).ToRGB();
            var size = 2;
            var area = GetArea(leg);
            return DrawAssessmentArea(identifier, area, color, size);
        }

        protected abstract ObstacleAssessmentArea GetArea(SegmentLeg leg);
    }
}
