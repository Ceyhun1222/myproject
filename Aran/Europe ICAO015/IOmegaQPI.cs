using System;
using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Queries;

namespace Europe_ICAO015
{
    public interface IOmegaQPI : Aran.Queries.ICommonQPI
    {
        Feature GetFeatureLst(FeatureType featureType, Guid identifier);

        AirportHeliport GetAdhp(Guid identifier);
        List<Runway> GetRunwayList(Guid airportIdentifier);

        List<Descriptor> GetRunwayListDescriptor(Guid airportidentifier);

        List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier);

        List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier);

        List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon);
        List<VerticalStructure> GetVerticalStructureList();
        List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance);
        List<ObstacleArea> GetObstacleArea();
        List<Airspace> GetTMAList();
        List<ObstacleArea> GetObstacleAreaList();
        RunwayElement GetRunwayElement(Guid rwyIdentifier);
        List<Airspace> GetAirspaceList();
        List<DME> GetDMEList();
        List<VOR> GetVORList();
        List<Navaid> GetNavaidList();
        List<Localizer> GetLocalizerList();
        List<Glidepath> GetGlidePathList();
        List<NDB> GetNDBList();
        List<MarkerBeacon> GetMArkersList();
    }
}