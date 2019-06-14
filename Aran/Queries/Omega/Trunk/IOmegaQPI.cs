using System;
using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;

namespace Aran.Queries.Omega
{
    public interface IOmegaQPI : ICommonQPI
    {
        AirportHeliport GetAdhp(Guid identifier);
        List<Runway> GetRunwayList (Guid airportIdentifier);
            
        List<RunwayDirection> GetRunwayDirectionList (Guid runwayIdentifier);

        List<RunwayCentrelinePoint> GetRunwayCentrelinePointList (Guid rwyDirIdentifier);

        List<VerticalStructure> GetVerticalStructureList (MultiPolygon polygon);
        List<VerticalStructure> GetVerticalStructureList();
        List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance);
        List<ObstacleArea> GetObstacleArea();
        List<Airspace> GetTMAList();
        List<GuidanceLine> GetTaxiwayGuidanceLineList(Guid airportIdentifier);
        List<ApronElement> GetApronElementList(Guid airportIdentifier);
        List<ObstacleArea> GetObstacleAreaList();
        RunwayElement GetRunwayElement(Guid rwyIdentifier);
        List<Airspace> GetAirspaceList();
    }
}