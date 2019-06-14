using Aran.Aim.Features;
using Aran.Geometries;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IAirportHeliportRepository
    {
        Point GetAirportHeliportPoint(int workPackage, Guid adhpIdentifier);

        IEnumerable<AirportHeliportDto> GetAirportHeliportDtos(int workPackage);

        FeatureCollection GetAirportHeliportFeatureCollection(int workPackage);

        AirportHeliportDto GetAirportHeliportByIdentifierDtos(int workPackage, Guid adhpIdentifier);

        IEnumerable<AirportHeliport> GetAirportHeliports(int workPackage, Guid? adhpIdentifier);
    }
}
