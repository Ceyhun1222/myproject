using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.Features;
using PVT.Engine.Common.Database;


namespace PVT.Engine.CDOTMA.Database
{
     class DbProvider : CommonDbProvider, IDbProvider
    {
        public List<HoldingPattern> GetHoldingPatterns()
        {
            throw new NotImplementedException();
        }

        public AirportHeliport GetAirport(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public Feature GetFeature(FeatureType type, Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<Aran.Aim.Data.FeatureReport> GetFeatureReport(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<T> GetProcedures<T>(Guid airportIdentifier) where T : Procedure
        {
            throw new NotImplementedException();
        }

        public List<HoldingAssessment> GetHoldingAssessments()
        {
            throw new NotImplementedException();
        }

        public Runway GetRunway(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<RunwayCentrelinePoint> GetRunwayCentreLinePoints(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public RunwayDirection GetRunwayDirection(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<Model.Screenshot> GetScreenShots(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public List<VerticalStructure> GetVerticalStructures(List<Guid> uuids)
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
