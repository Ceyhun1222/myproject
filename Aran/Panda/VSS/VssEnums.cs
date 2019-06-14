using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Vss
{
    public enum DrawElementType
    {
        GuidanceFacilityCourse,
        RwyDirCourse,
        FafPoint,
        TrackLine,
        FicTHR,
        TrackCourseIntersect,
        Point1400FromThr,
        Point60FromThr,
        Point60FromThrRight,
        Point60FromThrLeft,
        VssArea
    }

    public enum DrawOperType
    {
        Draw,
        Redraw,
        Clear
    }
}
