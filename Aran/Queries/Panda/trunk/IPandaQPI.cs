using System;
using System.Collections.Generic;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.Queries;
using Aran.Aim.Data;

namespace Aran.Queries.Panda_2
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

		List<Navaid> GetNavaidList();

        Navaid GetILSNavaid (Guid rwyDirIdentifier);

        List<VerticalStructure> GetVerticalStructureList (MultiPolygon polygon);

		List<Route> GetRouteList(Guid orgId);

		List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Point ptCenter, double distance);
        List<VerticalStructure> GetVerticalStructureList(List<Guid> uuids);
        List<VerticalStructure> GetVerticalStructureList();
        List<VerticalStructure> GetAnnexVerticalStructureList(Guid airportIdentifier);
        List<DesignatedPoint> GetDesignatedPointList (MultiPolygon polygon);

        List<SafeAltitudeArea> GetSafeAltitudeAreaList (Guid navaidIdentifier);

        List<InstrumentApproachProcedure> GetIAPList (Guid airportIdentifier);

        List<InstrumentApproachProcedure> GetRnavIAPList(Guid airportIdentifier);

        List<StandardInstrumentDeparture> GetSIDList(Guid airportIdentifier);

        List<StandardInstrumentDeparture> GetRNAVSIDList(Guid airportIdentifier);

        List<StandardInstrumentArrival> GetSTARList(Guid airportIdentifier);

        List<StandardInstrumentArrival> GetRNAVSTARList(Guid airportIdentifier);

        List<T> GetProcedureList<T>(Guid airportIdentifier, bool? isRnav) where T : Procedure;

        void AddToSrcLocalStorage(Feature feature);
        
        void AddToSrcLocalStorage(IEnumerable<Feature> features);

        void AddCreatedRefToSrcLocalStorage();

        void SaveSrcLocalStorage(string fileName);

        bool SaveAsXml(string fileName);
    }
}