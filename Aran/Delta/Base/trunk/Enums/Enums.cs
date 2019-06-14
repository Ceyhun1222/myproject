using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Enums
{
    public enum CalcultaionType
    {
        DirectInverse,
        DistanceDistance,
        CourseDistance,
        CourseCourse
    }

    public enum PointChoiceType
    {
        DesignatedPoint,
        Navaid,
        Point,
        RunwayCenterlinePoint
    }

    public enum PossiblePointObjectType 
    {
        DesignatedPoint,
        Dme,
        Vor,
        Tacan,
        Localizer
    }

    public enum DrawedAreaEnum
    {
        Circle,
        ArcByThreePoint,
        ArcByRaduis,
        Polygon
    }

    public enum FeatureType
    {
        Airspace,
        Route,
        Legs
    }

    public enum CreatingAreaType
    {
        Border,
        Arc
    }

    internal enum CreatingGeoType
    {
        Line,
        Circle,
        Ellipse,
        Ctr,
        ArcByRadius,
        Text,
        None,
    }

    internal enum OperationType
    {
        Intersect,
        Union,
        Clip
    }
}
