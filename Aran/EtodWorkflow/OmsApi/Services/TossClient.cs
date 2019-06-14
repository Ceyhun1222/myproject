using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OmsApi.Controllers;
using OmsApi.Dto;
using OmsApi.Configuration;
using OmsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OmsApi.Entity;
using System.Text;
using System.Diagnostics;
using OmsApi.Data;
using GeoJSON.Net.Feature;
using AutoMapper;
using System.IO;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace OmsApi.Services
{
    public class TossClient : ITossClient
    {
        private HttpClient _httpClient;

        public TossClient(HttpClient httpclient, TossConfig tossSettings)
        {
            _httpClient = httpclient;
            _httpClient.BaseAddress = new Uri(tossSettings.Url);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IList<PublicSlotDto>> GetPublicSlots()
        {
            var response = await _httpClient.GetAsync($"slot/all?{GetQueryString()}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IList<PublicSlotDto>>();
        }

        public async Task<FeatureCollection> GetAirports(long slotId)
        {
            var response = await _httpClient.GetAsync($"airportheliport/geojson/{slotId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<FeatureCollection>();
        }

        public async Task<IList<TossAirportHeliportDto>> GetAirportDtos(long slotId)
        {
            var response = await _httpClient.GetAsync($"airportheliport/{slotId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IList<TossAirportHeliportDto>>();
        }

        public async Task<FeatureCollection> GetObstacleAreas(long slotId, string arpId)
        {
            var response = await _httpClient.GetAsync($"ObstacleArea/geojson/{arpId}/{slotId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<FeatureCollection>();
        }

        //public async Task<FeatureCollection> GetRunwayCentrelinePoints(long slotId, string arpId)
        //{
        //    var role = "THR";
        //    var response = await _httpClient.GetAsync($"RunwayCenterlinePointByAdhp/geojson/{arpId}/{slotId}/{role}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsAsync<FeatureCollection>();
        //}

        //public async Task<HttpResponseMessage> GetVerticalStructures(long slotId)
        //{
        //    var response = await _httpClient.GetAsync($"VerticalStructure/geojson/{slotId}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsHttpResponseMessageAsync();// .ReadAsAsync<FeatureCollection>();
        //}

        //public async Task<FeatureCollection> GetVerticalstructuresByObstacleArea(long slotId, string obstacleAreaId)
        //{
        //    var response = await _httpClient.GetAsync($"VerticalStructure/geojson/{obstacleAreaId}/{slotId}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsAsync<FeatureCollection>();
        //}

        public async Task<FeatureCollection> GetVerticalStructuresByCircle(long slotId, string arpId, double radius)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = await _httpClient.GetAsync($"VerticalStructure/geojson/{arpId}/{radius}/{slotId}");
            stopwatch.Stop();
            var k = stopwatch.Elapsed;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<FeatureCollection>();
        }

        private string GetQueryString()
        {
            var excludeSlotStatuses = new List<SlotStatus>()
            {
                SlotStatus.Expired,
                SlotStatus.Empty,
                SlotStatus.Published,
            };
            string excludeSlotStatusKeyword = "excludedSlotStatuses";
            string includeSlotTypeKeyword = "includedSlotTypes";
            IList<string> res = new List<string>();
            foreach (var status in excludeSlotStatuses)
            {
                res.Add($"{excludeSlotStatusKeyword}={(int)status}");
            }
            return string.Join("&", res) + $"{includeSlotTypeKeyword}={(int)SlotType.PermanentDelta}";
        }

        public async Task<RequestTreeViewDto> GetObstacleAreaDtos(long slotId, string arpId, ILogger logger)
        {
            var response = await _httpClient.GetAsync($"runway/{arpId}/{slotId}");
            response.EnsureSuccessStatusCode();
            var runways = await response.Content.ReadAsAsync<IList<TossRunwayDto>>();
            var result = new RequestTreeViewDto()
            {
                ObstacleAreas = new List<ObstacleAreaDto>(),
                Runways = new List<RunwayDto>()
            };
            if (runways != null)
            {
                var typeList = new List<string>()
                {
                    ObstacleAreaType.OTHER_BALKEDLANDING.ToString(),
                    ObstacleAreaType.OTHER_INNERHORIZONTAL.ToString(),
                    ObstacleAreaType.OTHER_CONICAL.ToString(),
                    ObstacleAreaType.OTHER_OUTERHORIZONTAL.ToString(),
                    ObstacleAreaType.OTHER_APPROACH.ToString(),
                    ObstacleAreaType.OTHER_INNERAPPROACH.ToString(),
                    ObstacleAreaType.OTHER_TAKEOFFCLIMB.ToString(),
                    ObstacleAreaType.OTHER_TRANSITIONAL.ToString(),
                    ObstacleAreaType.OTHER_INNERTRANSITIONAL.ToString(),
                    ObstacleAreaType.OTHER_AREA2A.ToString(),
                    ObstacleAreaType.OTHER_TAKEOFFFLIGHTPATHAREA.ToString()
                };
                var tmpList = new List<string>();
                typeList.ForEach(t => tmpList.Add($"includedTypes={t}"));
                var typeQuery = string.Join('&', tmpList);
                response = await _httpClient.GetAsync($"obstaclearea/{arpId}/{slotId}?{typeQuery}");
                response.EnsureSuccessStatusCode();
                var obstacleAreas = await response.Content.ReadAsAsync<IList<TossObstacleAreaDto>>();
                var dictionaryAreas = new Dictionary<string, IList<ObstacleAreaDto>>();
                var arpObstacleAreas = new List<ObstacleAreaDto>();
                if (obstacleAreas != null)
                {
                    foreach (var item in obstacleAreas)
                    {
                        if (string.IsNullOrEmpty(item.RunwayDesignator))
                        {
                            result.ObstacleAreas.Add(new ObstacleAreaDto() { Id = item.Identifier, Name = item.Type });
                            continue;
                        }
                        if (!dictionaryAreas.ContainsKey(item.RunwayDesignator))
                            dictionaryAreas.Add(item.RunwayDesignator, new List<ObstacleAreaDto>());
                        dictionaryAreas[item.RunwayDesignator].Add(new ObstacleAreaDto() { Id = item.Identifier, Name = item.Type });
                    }
                }
                foreach (var tossRwy in runways)
                {
                    var rwy = new RunwayDto() { Name = tossRwy.Designator, RunwayDirections = new List<RunwayDirectionDto>() };
                    response = await _httpClient.GetAsync($"runwaydirection/{tossRwy.Identifier}/{slotId}");
                    response.EnsureSuccessStatusCode();
                    tossRwy.RunwayDirections = await response.Content.ReadAsAsync<IList<TossRunwayDirectionDto>>();
                    if (double.TryParse(tossRwy.RunwayDirections[0].Designator.AsSpan(0,2), out double dir1) &&
                        double.TryParse(tossRwy.RunwayDirections[1].Designator.AsSpan(0,2), out double dir2))
                    {
                        if (dir1 > dir2)
                        {
                            rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[1].Designator });
                            rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[0].Designator });
                        }
                        else
                        {
                            rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[0].Designator });
                            rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[1].Designator });
                        }
                    }
                    else
                    {
                        logger.LogError("Couldn't parse RunwayDirection to double");
                        rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[0].Designator });
                        rwy.RunwayDirections.Add(new RunwayDirectionDto() { Name = tossRwy.RunwayDirections[1].Designator });
                    }
                    result.Runways.Add(rwy);
                }
                foreach (var runway in result.Runways)
                {
                    foreach (var rwyDir in runway.RunwayDirections)
                    {
                        if (dictionaryAreas.ContainsKey(rwyDir.Name))
                        {
                            rwyDir.ObstacleAreas = dictionaryAreas[rwyDir.Name];
                        }
                    }
                }
            }
            return result;
        }

        public async Task<string> Submit2Aixm(long slotId, TossRequestSubmit2AixmDto data)
        {
            var json = JsonConvert.SerializeObject(new List<TossRequestSubmit2AixmDto>() { data });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType.CharSet = string.Empty;
            var response = await _httpClient.PostAsync($"verticalstructure/{slotId}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IList<TossVerticalStructureDto>> GetVerticalStructureDtos(long slotId, string arpId, double radius)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = await _httpClient.GetAsync($"VerticalStructure/{arpId}/{radius}/{slotId}");
            stopwatch.Stop();
            var k = stopwatch.Elapsed;
            response.EnsureSuccessStatusCode();
            try
            {
                return await response.Content.ReadAsAsync<IList<TossVerticalStructureDto>>();
            }
            catch (Exception ex)
            {
                var c = ex.Message;
                throw;
            }
        }

    }
    public enum ObstacleAreaType
    {
        OTHER_BALKEDLANDING,
        OTHER_INNERHORIZONTAL,
        OTHER_CONICAL,
        OTHER_OUTERHORIZONTAL,
        OTHER_APPROACH,
        OTHER_INNERAPPROACH,
        OTHER_TAKEOFFCLIMB,
        OTHER_TRANSITIONAL,
        OTHER_INNERTRANSITIONAL,
        OTHER_AREA2A,
        OTHER_TAKEOFFFLIGHTPATHAREA
    }
}