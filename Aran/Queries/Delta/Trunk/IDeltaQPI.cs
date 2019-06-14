using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Queries.DeltaQPI
{
    public interface IDeltaQPI:ICommonQPI
    {
        AirportHeliport GetAdhp(Guid identifier);
        List<DesignatedPoint> GetDesignatedPointList();
        List<DesignatedPoint> GetDesignatedPointList(Aran.Geometries.Point ptCenter,double radius);
        List<Navaid> GetNavaidList();
        List<Navaid> GetNavaidListByTypes(params CodeNavaidService[] navaidServiceTypes);
        List<Navaid> GetNavaidListByTypes(Aran.Geometries.Point centerGeo, double radius, params CodeNavaidService[] navaidServiceTypes);
        List<RouteSegment> GetRouteSegmentList(Guid routeidentifier);
        List<Route> GetRouteList();
        Airspace GetFir();
        List<Airspace> GetAirspaceList();
        List<RunwayCentrelinePoint> GetRunwayCenterlinePointList();
    }
}
