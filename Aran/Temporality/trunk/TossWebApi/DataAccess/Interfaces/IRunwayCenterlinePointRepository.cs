using Aran.Aim.Enums;
using Aran.Aim.Features;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IRunwayCenterlinePointRepository
    {
        IEnumerable<RunwayCenterlinePointDto> GetRunwayCenterlinePointDtos(int workPackage, Guid rwyDirectionIdentifier, CodeRunwayPointRole? role = null);

        IEnumerable<RunwayCentrelinePoint> GetRunwayCenterlinePoints(int workPackage, Guid rwyDirectionIdentifier, CodeRunwayPointRole? role = null);

        FeatureCollection GetRunwayCenterlinePointFeatureCollection(int workPackage, Guid rwyDirectionIdentifier);

        FeatureCollection GetRunwayCenterlinePointFeatureCollectionByRunwayDirections(int workPackage, List<RunwayDirection> runwayDirections, CodeRunwayPointRole? role = null);
    }
}
