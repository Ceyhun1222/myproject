using Aran.Aim.Features;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IObstacleAreaRepository
    {
        IEnumerable<ObstacleArea> GetObstacleAreaBaseOnAdhpAndRwy(int workPackage, Guid adhpIdentifier, List<Guid> rwyDirs, List<Aran.Aim.Enums.CodeObstacleArea> includedTypes);

        IEnumerable<ObstacleArea> GetObstacleAreas(int workPackage, List<Guid> identifiers, DateTime actualDate);
    }
}