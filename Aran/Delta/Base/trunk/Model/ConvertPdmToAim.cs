using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace Aran.Delta.Model
{
    public class ConvertPdmToAim
    {
        public static Aran.Aim.Features.AirportHeliport ToAirportHeliport(PDM.AirportHeliport pdmAirportHeliport)
        {
            var result = new Aran.Aim.Features.AirportHeliport();
            result.Name = pdmAirportHeliport.Name;
            result.Designator = pdmAirportHeliport.Designator;

            if (pdmAirportHeliport.Geo == null)
            {
                pdmAirportHeliport.RebuildGeo();
                if (pdmAirportHeliport.Geo == null || pdmAirportHeliport.Geo.IsEmpty)
                    return null;
            }

            if (pdmAirportHeliport.Geo != null)
            {
                result.ARP = new ElevatedPoint();
                var geoPoint = (IPoint) pdmAirportHeliport.Geo;
                result.ARP.Geo.X = geoPoint.X;
                result.ARP.Geo.Y = geoPoint.Y;
            }
            return result;
        }

        public static Aran.Aim.Features.DesignatedPoint ToDesignatedPoint(PDM.WayPoint pdmWayPoint)
        {
            var result = new DesignatedPoint();
            result.Identifier = new Guid(pdmWayPoint.ID);
            if (pdmWayPoint.Geo == null)
            {
               pdmWayPoint.RebuildGeo();
                if (pdmWayPoint.Geo == null || pdmWayPoint.Geo.IsEmpty)
                    return null;
            }
            
            result.Location = new AixmPoint();
            result.Location.Geo.X = ((IPoint)pdmWayPoint.Geo).X;
            result.Location.Geo.Y = ((IPoint)pdmWayPoint.Geo).Y;
            result.Name = pdmWayPoint.Name;
            result.Designator = pdmWayPoint.Designator;
            result.Type =(CodeDesignatedPoint) pdmWayPoint.Type;
            return result;
        }

        public static Aran.Aim.Features.Navaid ToNavaid(PDM.NavaidSystem pdmNavaidSystem)
        {
            var result = new Navaid();
            result.Identifier = new Guid(pdmNavaidSystem.ID);
            if (pdmNavaidSystem.Geo == null)
                pdmNavaidSystem.RebuildGeo();

            result.Location = new ElevatedPoint();
            if (pdmNavaidSystem.Components.Count > 0)
            {
                pdmNavaidSystem.Components[0].RebuildGeo();
                var geo = pdmNavaidSystem.Components[0].Geo;

                if (geo != null)
                {
                    result.Location.Geo.X = ((IPoint)geo).X;
                    result.Location.Geo.Y = ((IPoint)geo).Y;
                }
                else return null;

            }
            else return null;
            
           // result.Location.Geo.X = ((IPoint)pdmNavaidSystem.Geo).X;
            //result.Location.Geo.Y = ((IPoint)pdmNavaidSystem.Geo).Y;
            result.Name = pdmNavaidSystem.Name;
            result.Designator = pdmNavaidSystem.Designator;
            return result;
        }

        public static Aran.Aim.Features.Route ToRoute(PDM.Enroute pdmEnroute)
        {
            var tmpAimRoute = new Aran.Aim.Features.Route();
            tmpAimRoute.Name = pdmEnroute.TxtDesig;
            tmpAimRoute.Identifier = new Guid(pdmEnroute.ID);
            return tmpAimRoute;
        }

        public static Aran.Aim.Features.Airspace ToAirspace(PDM.Airspace pdmAirspace)
        {
            try
            {
                var result = new Aran.Aim.Features.Airspace();
                result.Designator = pdmAirspace.CodeID;
                result.Name = pdmAirspace.TxtName;
                foreach (var airspaceVolume in pdmAirspace.AirspaceVolumeList)
                {
                    var vaAirspaceVolume = new Aran.Aim.Features.AirspaceVolume {HorizontalProjection = new Surface()};

                    airspaceVolume.RebuildGeo2();

                    if (airspaceVolume.Geo != null && !airspaceVolume.Geo.IsEmpty)
                    {
                        var volumeGeo =
                            (Aran.Geometries.MultiPolygon)
                                Aran.Converters.ConvertFromEsriGeom.ToGeometry(airspaceVolume.Geo, true);

                        //Here must be look again
                        foreach (Aran.Geometries.Polygon poly in volumeGeo)
                            vaAirspaceVolume.HorizontalProjection.Geo.Add(poly);

                        result.Type = (CodeAirspace) (int) airspaceVolume.CodeType;

                        if (airspaceVolume.ValDistVerLower.HasValue)
                        {
                            vaAirspaceVolume.LowerLimit = new ValDistanceVertical();
                            if (airspaceVolume.UomValDistVerLower== UOM_DIST_VERT.M)
                                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.M;
                            else if (airspaceVolume.UomValDistVerLower== UOM_DIST_VERT.FT)
                                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FT;
                            else
                                vaAirspaceVolume.LowerLimit.Uom = UomDistanceVertical.FL;
 
                            vaAirspaceVolume.LowerLimit.Value = airspaceVolume.ValDistVerLower.Value;
                        }
                        if (airspaceVolume.ValDistVerUpper.HasValue)
                        {
                            vaAirspaceVolume.UpperLimit = new ValDistanceVertical();
                            if (airspaceVolume.UomValDistVerUpper == UOM_DIST_VERT.M)
                                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.M;
                            else if (airspaceVolume.UomValDistVerUpper == UOM_DIST_VERT.FT)
                                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FT;
                            else
                                vaAirspaceVolume.UpperLimit.Uom = UomDistanceVertical.FL;
                            vaAirspaceVolume.UpperLimit.Value = airspaceVolume.ValDistVerUpper.Value;
                        }

                        result.GeometryComponent.Add(new AirspaceGeometryComponent
                        {
                            TheAirspaceVolume = vaAirspaceVolume
                        });
                    }
                    else
                    {
                        GlobalParams.LogList.Add("Airspace : "+pdmAirspace.TxtName + " Airspace's has not Geometry!It will be ignored");
                       // Messages.Warning(pdmAirspace.CodeID+" Airspace's has not Geometry!It will be ignored");
                        return null;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                GlobalParams.LogList.Add("Airspace : " + pdmAirspace.TxtName + "-Not Correct!");
                return null;
            }
        }

        public static Aran.Aim.Features.RouteSegment ToRouteSegment(PDM.RouteSegment pdmRouteSegment)
        {
            try
            {
                var aimRouteSegment = new Aran.Aim.Features.RouteSegment();
                aimRouteSegment.Identifier = new Guid(pdmRouteSegment.ID);

                if (pdmRouteSegment.StartPoint != null)
                    aimRouteSegment.Start = ToRouteSegmentPoint(pdmRouteSegment.StartPoint);

                if (pdmRouteSegment.EndPoint != null)
                    aimRouteSegment.End = ToRouteSegmentPoint(pdmRouteSegment.EndPoint);

                aimRouteSegment.UpperLimit = new ValDistanceVertical();
                aimRouteSegment.UpperLimit.Uom = ToUomDistanceVertical(pdmRouteSegment.UomValDistVerUpper);
                if (pdmRouteSegment.ValDistVerUpper.HasValue)
                    aimRouteSegment.UpperLimit.Value = (double) pdmRouteSegment.ValDistVerUpper.Value;

                aimRouteSegment.LowerLimit = new ValDistanceVertical();
                aimRouteSegment.LowerLimit.Uom = ToUomDistanceVertical(pdmRouteSegment.UomValDistVerLower);
                if (pdmRouteSegment.ValDistVerLower.HasValue)
                    aimRouteSegment.LowerLimit.Value = (double) pdmRouteSegment.ValDistVerLower.Value;

                aimRouteSegment.CurveExtent = new Curve();
                if (pdmRouteSegment.Geo == null)
                {
                    pdmRouteSegment.RebuildGeo2();
                    if (pdmRouteSegment.Geo == null)
                        return null;
                }

                var routeGeo = (MultiLineString) Aran.Converters.ConvertFromEsriGeom.ToGeometry(pdmRouteSegment.Geo);
                if (routeGeo != null)
                    aimRouteSegment.CurveExtent.Geo.Add(routeGeo);

                return aimRouteSegment;
            }
            catch (Exception)
            {
                GlobalParams.LogList.Add("Route Segment : "+pdmRouteSegment.ID+"-Not Correct");
                return null;
            }
        }

        public static Aran.Aim.Features.EnRouteSegmentPoint ToRouteSegmentPoint(PDM.RouteSegmentPoint pdmRouteSegmentPoint)
        {
            var result = new EnRouteSegmentPoint();
            result.PointChoice = new SignificantPoint();
            switch (pdmRouteSegmentPoint.PointChoice)
            {
                case PDM.PointChoice.AirportHeliport:
                    result.PointChoice.AirportReferencePoint = new FeatureRef { FeatureType = FeatureType.AirportHeliport, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                    break;
                case PDM.PointChoice.DesignatedPoint:
                    result.PointChoice.FixDesignatedPoint = new FeatureRef { FeatureType = FeatureType.DesignatedPoint, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                    break;
                case PDM.PointChoice.Navaid:
                    result.PointChoice.NavaidSystem = new FeatureRef { FeatureType = FeatureType.Navaid, Identifier = new Guid(pdmRouteSegmentPoint.Route_LEG_ID) };
                    break;
                case PDM.PointChoice.RunwayCentrelinePoint:
                    result.PointChoice.RunwayPoint = new FeatureRef { FeatureType = FeatureType.Runway, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                    break;
            }
            return result;
        }

        public static UomDistanceVertical ToUomDistanceVertical(UOM_DIST_VERT pdmUomDistVert)
        {
            var result = UomDistanceVertical.M;
            switch (pdmUomDistVert)
            {
                case UOM_DIST_VERT.FL:
                    result = UomDistanceVertical.FL;
                    break;
                case UOM_DIST_VERT.FT:
                    result = UomDistanceVertical.FT;
                    break;

            }
            return result;
        }

    }
}
