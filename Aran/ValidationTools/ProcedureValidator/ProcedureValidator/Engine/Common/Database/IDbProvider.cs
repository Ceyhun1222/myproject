using Aran.Aim;
using Aran.Aim.Features;
using System;
using System.Collections.Generic;


namespace PVT.Engine.Common.Database
{
    public interface IDbProvider
    {
        void Open();

        List<T> GetProcedures<T> (Guid airportIdentifier) where T:Procedure;
        List<HoldingAssessment> GetHoldingAssessments();
        List<HoldingPattern> GetHoldingPatterns();
        AirportHeliport GetAirport(Guid identifier);
        RunwayDirection GetRunwayDirection(Guid identifier);
        Runway GetRunway(Guid identifier);
        Feature GetFeature(FeatureType type, Guid identifier);
        List<Aran.Aim.Data.FeatureReport> GetFeatureReport(Guid identifier);
        List<VerticalStructure> GetVerticalStructures(List<Guid> uuids);
        List<RunwayCentrelinePoint> GetRunwayCentreLinePoints(Guid identifier);
        List<Model.Screenshot> GetScreenShots(Guid uuid);
        void Load();
    }
}
