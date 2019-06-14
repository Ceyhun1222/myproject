using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Delta.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    class ConvertDesigningClassToAim
    {

        public static Aran.Aim.Features.DesignatedPoint ToDesignatedPoint(DesigningPoint designingPoint)
        {
            var result = new DesignatedPoint();
            result.Name = designingPoint.Name;
            result.Designator = designingPoint.Name;
            result.Location = new AixmPoint();
            var pt = designingPoint.Geo as Aran.Geometries.Point;
            if (pt == null)
                return null;
            result.Location.Geo.X =pt.X;
            result.Location.Geo.Y = pt.Y;
            return result;
        }

        public static Airspace ToAirspace(DesigningArea designingArea)
        {
            var result = new Airspace();
            result.Name = designingArea.Name;
            result.Designator = designingArea.Name;
            var vaAirspaceVolume = new Aran.Aim.Features.AirspaceVolume { HorizontalProjection = new Surface() };
            if (designingArea.Geo.Type == Geometries.GeometryType.MultiPolygon)
            {
                foreach (Aran.Geometries.Polygon poly in (Aran.Geometries.MultiPolygon)designingArea.Geo)
                    vaAirspaceVolume.HorizontalProjection.Geo.Add(poly);
            }
            else if (designingArea.Geo.Type == Geometries.GeometryType.Polygon)
            {
                vaAirspaceVolume.HorizontalProjection.Geo.Add(designingArea.Geo as Aran.Geometries.Polygon);
            }

            vaAirspaceVolume.LowerLimit = new ValDistanceVertical();
            if (designingArea.UomLowerLimit == "M")
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.M;
            else if (designingArea.UomLowerLimit == "FT")
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FT;
            else
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FL;

            vaAirspaceVolume.LowerLimit.Value = designingArea.LowerLimit ;

            vaAirspaceVolume.UpperLimit = new ValDistanceVertical();
            if (designingArea.UomUpperLimit == "M")
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.M;
            else if (designingArea.UomUpperLimit == "FT")
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FT;
            else
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FL;

            vaAirspaceVolume.UpperLimit.Value = designingArea.UpperLimit;

            result.GeometryComponent.Add(new AirspaceGeometryComponent
            {
                TheAirspaceVolume = vaAirspaceVolume
            });
            return result;
        }

        public static Airspace ToAirspace(DesigningBuffer designingBuffer)
        {
            var result = new Airspace();
            result.Name = designingBuffer.Name;
            result.Designator = designingBuffer.Name;
            var vaAirspaceVolume = new Aran.Aim.Features.AirspaceVolume { HorizontalProjection = new Surface() };
            if (designingBuffer.Geo.Type == Geometries.GeometryType.MultiPolygon)
            {
                foreach (Aran.Geometries.Polygon poly in (Aran.Geometries.MultiPolygon)designingBuffer.Geo)
                    vaAirspaceVolume.HorizontalProjection.Geo.Add(poly);
            }
            else if (designingBuffer.Geo.Type == Geometries.GeometryType.Polygon)
            {
                vaAirspaceVolume.HorizontalProjection.Geo.Add(designingBuffer.Geo as Aran.Geometries.Polygon);
            }

            vaAirspaceVolume.LowerLimit = new ValDistanceVertical();
            if (designingBuffer.UomLowerLimit == "M")
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.M;
            else if (designingBuffer.UomLowerLimit == "FT")
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FT;
            else
                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FL;

            vaAirspaceVolume.LowerLimit.Value = designingBuffer.LowerLimit;

            vaAirspaceVolume.UpperLimit = new ValDistanceVertical();
            if (designingBuffer.UomUpperLimit == "M")
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.M;
            else if (designingBuffer.UomUpperLimit == "FT")
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FT;
            else
                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FL;

            vaAirspaceVolume.UpperLimit.Value = designingBuffer.UpperLimit;

            result.GeometryComponent.Add(new AirspaceGeometryComponent
            {
                TheAirspaceVolume = vaAirspaceVolume
            });
            return result;
        }


        public static Aran.Aim.Features.Route ToRoute(DesigningRoute designingRoute)
        {
            var result = new Aran.Aim.Features.Route();
            result.Name = designingRoute.Name;
            result.Identifier = new Guid();
            return result;
        }

        public static Aran.Aim.Features.RouteSegment ToRouteSegment(DesigningSegment pdmRouteSegment)
        {
            try
            {
                var aimRouteSegment = new Aran.Aim.Features.RouteSegment();
                aimRouteSegment.Identifier = new Guid();

                if (pdmRouteSegment.WptStart != null)
                    aimRouteSegment.Start = ToRouteSegmentPoint(pdmRouteSegment.WptStart);

                if (pdmRouteSegment.WptEnd != null)
                    aimRouteSegment.End = ToRouteSegmentPoint(pdmRouteSegment.WptEnd);
                
                aimRouteSegment.CurveExtent = new Curve();
                if (pdmRouteSegment.Geo == null)
                {
                    return null;
                }

                var routeGeo =(Aran.Geometries.MultiLineString) pdmRouteSegment.Geo;
                if (routeGeo != null)
                    aimRouteSegment.CurveExtent.Geo.Add(routeGeo);

                return aimRouteSegment;
            }
            catch (Exception)
            {
                GlobalParams.LogList.Add("Route Segment : " + pdmRouteSegment.Name + "-Not Correct");
                return null;
            }
        }

        public static Aran.Aim.Features.EnRouteSegmentPoint ToRouteSegmentPoint(string pdmRouteSegmentPoint)
        {
            var result = new EnRouteSegmentPoint();
            result.PointChoice = new SignificantPoint();

            result.PointChoice.FixDesignatedPoint = new FeatureRef { FeatureType = Aran.Aim.FeatureType.DesignatedPoint, Identifier = new Guid() };
            return result;
        }


    }
}
