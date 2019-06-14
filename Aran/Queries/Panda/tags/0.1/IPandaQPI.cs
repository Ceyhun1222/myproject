using System;
using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;

namespace Aran.Queries.Panda
{
    public interface IPandaSpecializedQPI : ICommonQPI
    {
        List<Descriptor> GetAirportHeliportList (Guid organisationIdentifier, bool checkILS);

        List<AirportHeliport> GetAirportHeliportList ( Guid organisationIdentifier );

        List<Descriptor> GetRunwayList (Guid airportIdentifier);

        List<RunwayDirection> GetRunwayDirectionList (Guid runwayIdentifier);

        List<RunwayCentrelinePoint> GetRunwayCentrelinePointList (Guid rwyDirIdentifier);

        List<RunwayProtectArea> GetRunwayProtectAreaList (Guid rwyDirIdentifier);

        List<Navaid> GetNavaidList (MultiPolygon polygon);

        Navaid GetILSNavaid (Guid rwyDirIdentifier);

        List<VerticalStructure> GetVerticalStructureList (MultiPolygon polygon);

        List<DesignatedPoint> GetDesignatedPointList (MultiPolygon polygon);

        List<SafeAltitudeArea> GetSafeAltitudeAreaList (Guid navaidIdentifier);

        List<InstrumentApproachProcedure> GetIAPList (Guid airportIdentifier);
    }
}