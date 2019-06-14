using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Queries.Common;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.PANDA.Common;

namespace Aran.Queries.Rnav
{
    public interface IRNAVSpecializedQPI : ICommonQPI
    {
        List<DesignatedPoint> GetDesignatedPointList(Aran.Geometries.Polygon polygon);
        List<DesignatedPoint> GetDesignatedPointList(Aran.Geometries.Point pt, double distance);
        List<Navaid> GetNavaidList(Aran.Geometries.Polygon polygon);
        List<Navaid> GetNavaidList(Aran.Geometries.Point centerGeo, double distance);
        List<Navaid> GetNavaidListByTypes(Aran.Geometries.Polygon polygon, SpatialReferenceOperation spatialReferenceOperation = null, params CodeNavaidService[] navaidServiceTypes);
        //List<Navaid> GetNavaidListByTypes(Aran.Geometries.Geometry centerGeo, double distance, params CodeNavaidService[] navaidServiceTypes);
        VOR GetVor(Guid identifier);
        DME GetDme(Guid identifier);
		List<OrganisationAuthority> GetOrganisatioListDesignatorNotNull ( );
		OrganisationAuthority GetOrganisation ( Guid identifier );
        List<AirportHeliport> GetAdhpListDesignatorNotNull(Guid orgGuid);
        List<VerticalStructure> GetVerticalStructureList(Aran.Geometries.Polygon polygon);
        AirportHeliport GetAirportHeliport(Guid identifier);

        List<VerticalStructure> GetVerticalStructureList();
    }
}
