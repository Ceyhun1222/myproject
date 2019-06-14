using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ObstacleCalculator.Domain.Models;
using ObstacleChecker.API.Dto;

namespace ObstacleChecker.API.Model
{
    public interface ITossHttpClient
    {
        string BaseAddress { get;}

        Task<AirportHeliportDto> GetAirportHeliportDtoByDesignator(string adhpName,int workPackage);

        Task<AirportHeliportDto> GetAirportHeliportDtoByIdentifier(string identifier, int workpackage);

        Task<IEnumerable<SurfaceBase>> GetObstacleAreas(string adhpName, int workPackage);
    }
}
