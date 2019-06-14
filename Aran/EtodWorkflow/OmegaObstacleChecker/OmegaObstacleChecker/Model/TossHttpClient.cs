using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ObstacleCalculator.Domain.Models;
using ObstacleChecker.API.Dto;

namespace ObstacleChecker.API.Model
{
    public class TossHttpClient:ITossHttpClient
    {
        public TossHttpClient(string baseAdress)
        {
            BaseAddress = baseAdress;
        }

        public string BaseAddress { get;}

        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        public async Task<AirportHeliportDto> GetAirportHeliportDtoByDesignator(string designator,int workpackage)
        {
            var adhpList =await GetAllAirports(workpackage);
            return adhpList.FirstOrDefault(adhp => adhp.Designator.Equals(designator));
        }


        public async Task<AirportHeliportDto> GetAirportHeliportDtoByIdentifier(string identifier, int workpackage)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{BaseAddress}airportheliport/{identifier}/{workpackage}")
                .ConfigureAwait(false);
            var adhp = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<AirportHeliportDto>(adhp);
        }

        public async Task<IEnumerable<AirportHeliportDto>> GetAllAirports(int workpackage)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(BaseAddress + "airportheliport/" + workpackage)
                .ConfigureAwait(false);
            var adhpList = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<IEnumerable<AirportHeliportDto>>(adhpList);
        }

        public async Task<IEnumerable<SurfaceBase>> GetObstacleAreas(string adhpName, int workpackage)
        {
           var adhp = await GetAirportHeliportDtoByIdentifier(adhpName, workpackage);
            if (adhp == null)
                throw new ArgumentException("Airport not found");

            var allAirports = GetAllAirports(workpackage);

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(
                    BaseAddress + $"obstaclearea/{adhp.Identifier}/{workpackage}")
                .ConfigureAwait(false);
            var surfaces = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonConvert.DeserializeObject<IEnumerable<SurfaceBase>>(surfaces);
        }

    }
}
