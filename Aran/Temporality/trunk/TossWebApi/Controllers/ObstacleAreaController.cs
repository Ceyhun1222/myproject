using Aran.Aim;
using Aran.Temporality.Common.Id;
using AutoMapper;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Aran.Aim.Enums;
using TossWebApi.Common;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.Controllers
{
    [RoutePrefix("obstaclearea")]
    public class ObstacleAreaController : ApiController
    {
        private readonly IObstacleAreaRepository _repository;
        private IVerticalStructureRepository _verticalStructureRepository;
        private IAirportHeliportRepository _airportHeliportRepository;
        private readonly IRunwayRepository _runwayRepository;
        private readonly IRunwayDirectionRepository _runwayDirectionRepository;

        public ObstacleAreaController(IObstacleAreaRepository repository, 
            IVerticalStructureRepository verticalStructureRepository,
            IAirportHeliportRepository airportHeliportRepository,
            IRunwayRepository runwayRepository,
            IRunwayDirectionRepository runwayDirectionRepository)
        {
            _repository = repository;
            _verticalStructureRepository = verticalStructureRepository;
            _runwayDirectionRepository = runwayDirectionRepository;
            _runwayRepository = runwayRepository;
            _airportHeliportRepository = airportHeliportRepository;
        }

        [Route("{adhpIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<ObstacleAreaDto>))]
        public IHttpActionResult GetObstacleAreas(Guid adhpIdentifier, int workPackage, [FromUri] List<CodeObstacleArea> includedTypes = null)
        {
            var runways = _runwayRepository.GetRunways(workPackage, adhpIdentifier);
            if (runways == null || !runways.Any())
                return Ok();

            var runwayGuids = runways.Select(runway => runway.Identifier).ToList();

            var runwayDirections = _runwayDirectionRepository.GetRunwayDirections(workPackage, runwayGuids);
            if (runwayDirections == null || !runwayDirections.Any())
                return Ok();

            var runwayDirectionGuids = runwayDirections.Select(runwayDirection => runwayDirection.Identifier).ToList();

            var obstacleAreas = _repository.GetObstacleAreaBaseOnAdhpAndRwy(workPackage, adhpIdentifier, runwayDirectionGuids, includedTypes);

            if (obstacleAreas == null)
                return Ok();

            var features = obstacleAreas.Select(oa =>
            {
                var oaDto = Mapper.Map<ObstacleAreaDto>(oa);

                if(oa.Reference?.OwnerRunway?.Identifier != null)
                {
                    var runway = _runwayDirectionRepository.GetRunwayDirection(workPackage, oa.Reference.OwnerRunway.Identifier);
                    oaDto.RunwayDesignator = runway?.Designator;
                }

                return oaDto;
            });


            return Json(features);
        }

        [Route("geojson/{adhpIdentifier}/{workPackage}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetObstacleAreaFeatureCollection(Guid adhpIdentifier, int workPackage, [FromUri] List<CodeObstacleArea> includedTypes = null)
        {
            var runways = _runwayRepository.GetRunways(workPackage, adhpIdentifier);
            if (runways == null || !runways.Any())
                return Ok();

            var runwayGuids = runways.Select(runway => runway.Identifier).ToList();

            var runwayDirections = _runwayDirectionRepository.GetRunwayDirections(workPackage, runwayGuids);
            var runwayDirectionGuids = runwayDirections.Select(runwayDirection => runwayDirection.Identifier).ToList();

            var obstacleAreas = _repository.GetObstacleAreaBaseOnAdhpAndRwy(workPackage, adhpIdentifier, runwayDirectionGuids, includedTypes);

            if (obstacleAreas == null)
                return Ok();

            var features = obstacleAreas.Select(oa =>
            {
                var obstacleAreaFeature = Mapper.Map<GeoJSON.Net.Feature.Feature>(oa);

                if (oa.Reference?.OwnerRunway?.Identifier != null)
                {
                    var runway = _runwayDirectionRepository.GetRunwayDirection(workPackage, oa.Reference.OwnerRunway.Identifier);
                    obstacleAreaFeature.Properties.Add("RunwayDesignator", runway?.Designator);
                }

                return obstacleAreaFeature;
            }).ToList();
            
            var featureCollection = new FeatureCollection(features);

            return Json(featureCollection);
        }
    }
}
