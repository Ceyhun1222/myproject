using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Delta.Model;
using Aran.Queries.DeltaQPI;
using ESRI.ArcGIS.Carto;

namespace Aran.Delta
{
    public interface IDBModule
    {
        List<Route> RouteList { get; }

        List<RouteSegment> GetRouteSegmentList(Guid routeIdentifier);
        AirportHeliport AirportHeliport { get;}
        List<DesignatedPoint> DesignatedPointList { get;}
        List<Navaid> NavaidListByTypes { get;}
        List<Navaid> NavaidList { get; }
        List<Airspace> GetAirspaceList { get;}
        string GetFeatureName(Aim.FeatureType featureType, Guid guid);
        IDeltaQPI DeltaQPI { get; }
        List<RunwayCentrelinePoint> RunwayCenterlineList { get;}
    }
}
