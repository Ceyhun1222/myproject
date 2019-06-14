using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.CommonUtil.Util;
using AutoMapper;
using GeoJSON.Net;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using MongoDB.Bson.IO;
using MongoDB.Driver.GeoJsonObjectModel;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace TossWebApi.Controllers
{
    [RoutePrefix("verticalstructure")]
    public class VerticalStructureController : ApiController
    {
        private readonly IVerticalStructureRepository _repository;
        private readonly IAirportHeliportRepository _adhpRepository;
        private readonly IObstacleAreaRepository _obstacleAreaRepository;
        private readonly ISlotRepository _slotRepository;

        public VerticalStructureController(IVerticalStructureRepository repository, IAirportHeliportRepository adhpRepository, ISlotRepository slotRepository, IObstacleAreaRepository obstacleAreaRepository)
        {
            _repository = repository;
            _adhpRepository = adhpRepository;
            _slotRepository = slotRepository;
            _obstacleAreaRepository = obstacleAreaRepository;
        }

        [HttpGet]
        [Route("{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetVerticalStructures(int workPackage)
        {
            var features = _repository.GetVerticalStructureDtos(workPackage);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("geojson/{workPackage}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetVerticalStructureFeatureCollection(int workPackage)
        {
            var features = _repository.GetVerticalStructureFeatureCollection(workPackage);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("{point1_latitude},{point1_longitude},{point2_latitude},{point2_longitude}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetVerticalStructuresByBbox(double point1_latitude, double point1_longitude, 
                                                                double point2_latitude, double point2_longitude, int workPackage)
        {
            var minX = (point1_longitude < point2_longitude) ? point1_longitude : point2_longitude;
            var maxX = (point1_longitude < point2_longitude) ? point2_longitude : point1_longitude;

            var minY = (point1_latitude < point2_latitude) ? point1_latitude : point2_latitude;
            var maxY = (point1_latitude < point2_latitude) ? point2_latitude : point1_latitude;

            var features = _repository.GetVerticalStructureDtosByBbox(workPackage, minX, minY, maxX, maxY);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("geojson/{point1_latitude},{point1_longitude},{point2_latitude},{point2_longitude}/{workPackage}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetVerticalStructureFeatureCollectionByBbox(double point1_latitude, double point1_longitude,
                                                        double point2_latitude, double point2_longitude, int workPackage)
        {
            var minX = (point1_longitude < point2_longitude) ? point1_longitude : point2_longitude;
            var maxX = (point1_longitude < point2_longitude) ? point2_longitude : point1_longitude;

            var minY = (point1_latitude < point2_latitude) ? point1_latitude : point2_latitude;
            var maxY = (point1_latitude < point2_latitude) ? point2_latitude : point1_latitude;

            var features = _repository.GetVerticalStructureFeatureCollectionByBbox(workPackage, minX, minY, maxX, maxY);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("{oaIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetObstacles(Guid oaIdentifier, int workPackage)
        {
            var obstacleArea = _repository.GetObstacleByObstacleArea(workPackage, oaIdentifier);

            var features = _repository.GetVerticalStructureDtosByObstacles(workPackage, obstacleArea.Obstacle);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("geojson/{oaIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetObstaclesFeatureCollection(Guid oaIdentifier, int workPackage)
        {
            var obstacleArea = _repository.GetObstacleByObstacleArea(workPackage, oaIdentifier);

            var features = _repository.GetVerticalStructureFeatureCollectionByObstacles(workPackage, obstacleArea.Obstacle);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("{adhpIdentifier}/{radius}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetObstacles(Guid adhpIdentifier, double radius, int workPackage)
        {
            var adhpPoint = _adhpRepository.GetAirportHeliportPoint(workPackage, adhpIdentifier);

            var features = _repository.GetVerticalStructureDtosByAdhpRadius(workPackage, adhpPoint, radius);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("geojson/{adhpIdentifier}/{radius}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<VerticalStructureDto>))]
        public IHttpActionResult GetObstaclesFeatureCollection(Guid adhpIdentifier, double radius, int workPackage)
        {
            var adhpPoint = _adhpRepository.GetAirportHeliportPoint(workPackage, adhpIdentifier);

            var features = _repository.GetVerticalStructureFeatureCollectionByAdhpRadius(workPackage, adhpPoint, radius);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("{workPackage}")]
        [HttpPost]
        public IHttpActionResult PostVerticalStructure(int workPackage, List<VerticalStructureCreateDto> verticalStructureCreateDtos)
        {
            if (workPackage == 0)
                return BadRequest("Can not commit to publish slot");

            if (verticalStructureCreateDtos == null)
                return BadRequest("Dto list is null");

            var slots = _slotRepository.GetSlots();

            var actualDate =
                slots.FirstOrDefault(publicSlot => publicSlot.PrivateSlots.Any(x => x.Id == workPackage))
                    ?.EffectiveDate ?? throw new Exception("Can not find workPackage");

            var obstacleAreas = new Dictionary<Guid, List<ObstacleArea>>();
            foreach (var verticalStructureCreateDto in verticalStructureCreateDtos)
            {
                try
                {
                    obstacleAreas[verticalStructureCreateDto.Identifier] = _obstacleAreaRepository.GetObstacleAreas(
                            workPackage, verticalStructureCreateDto.ObstacleAreadIdList, actualDate)
                        .ToList();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            foreach (var verticalStructureCreateDto in verticalStructureCreateDtos)
            {
                var verticalStructure = Mapper.Map<VerticalStructureCreateDto, VerticalStructure>(verticalStructureCreateDto);
                verticalStructure.WorksPackageId = workPackage;

                Migrator.AddFeatureToSlot(verticalStructure, workPackage, actualDate, false);

                foreach (var obstacleArea in obstacleAreas[verticalStructure.Identifier])
                {
                    if (obstacleArea.Obstacle.All(x => x.Feature.Identifier != verticalStructure.Identifier))
                    {
                        obstacleArea.Obstacle.Add(new FeatureRefObject(verticalStructure.Identifier));
                        Migrator.AddFeatureToSlot(obstacleArea, workPackage, actualDate, false);
                    }
                }
            }
            
            return Ok();
        }
    }
}
