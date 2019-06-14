using GeoJSON.Net.Feature;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OmsApi.Data;
using OmsApi.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OmsApi.Interfaces
{
    public interface ITossClient
    {        
        Task<IList<PublicSlotDto>> GetPublicSlots();

        Task<FeatureCollection> GetAirports(long slotId);

        Task<IList<TossAirportHeliportDto>> GetAirportDtos(long slotId);

        Task<RequestTreeViewDto> GetObstacleAreaDtos(long slotId, string arpId, ILogger logger);

        Task<FeatureCollection> GetObstacleAreas(long slotId, string arpId);

        //Task<FeatureCollection> GetRunwayCentrelinePoints(long slotId, string arpId);
        //Task<FeatureCollection> GetVerticalstructuresByObstacleArea(long slotId, string obstacleAreaId);

        Task<FeatureCollection> GetVerticalStructuresByCircle(long slotId, string arpdId, double radius);

        Task<IList<TossVerticalStructureDto>> GetVerticalStructureDtos(long slotId, string arpdId, double radius);

        Task<string> Submit2Aixm(long slotId, TossRequestSubmit2AixmDto data);
    }
}