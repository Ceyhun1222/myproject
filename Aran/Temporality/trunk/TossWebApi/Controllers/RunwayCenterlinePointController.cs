using Aran.Aim.Enums;
using Aran.Aim.Features;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;

namespace TossWebApi.Controllers
{
    [RoutePrefix("runwaycenterlinepoint")]
    public class RunwayCenterlinePointController : ApiController
    {
        private readonly IRunwayCenterlinePointRepository _repository;
        private readonly IRunwayRepository _runwayRepository;
        private readonly IRunwayDirectionRepository _runwayDirectionRepository;

        public RunwayCenterlinePointController(
            IRunwayCenterlinePointRepository repository,
            IRunwayRepository runwayRepository,
            IRunwayDirectionRepository runwayDirectionRepository
            )
        {
            _repository = repository;
            _runwayRepository = runwayRepository;
            _runwayDirectionRepository = runwayDirectionRepository;
        }

        [Route("{rwyDirectionIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<RunwayCenterlinePointDto>))]
        public IHttpActionResult GetRunwayCenterLinePoint(Guid rwyDirectionIdentifier, int workPackage)
        {
            var features = _repository.GetRunwayCenterlinePointDtos(workPackage, rwyDirectionIdentifier);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("~/runwaycenterlinepointbyadhp/{adhpIdentifier}/{workPackage}/{role?}")]
        [ResponseType(typeof(IEnumerable<RunwayCenterlinePointDto>))]
        public IHttpActionResult GetRunwayCenterLinePointByAirportHeliport(Guid adhpIdentifier, int workPackage, CodeRunwayPointRole? role)
        {
            var runways = _runwayRepository.GetRunways(workPackage, adhpIdentifier);
            var runwayDirections = new List<RunwayDirection>();
            var runwayCentrelinePoints = new List<RunwayCenterlinePointDto>();

            if (runways != null)
            {
                foreach (var runway in runways)
                {
                    runwayDirections.AddRange(_runwayDirectionRepository.GetRunwayDirections(workPackage, runway.Identifier));
                }
            }

            if (runwayDirections != null)
            {
                foreach (var runwayDirection in runwayDirections)
                {
                    var runwayCenterLinePointsDto = _repository.GetRunwayCenterlinePointDtos(workPackage, runwayDirection.Identifier, role);
                    if(runwayCenterLinePointsDto != null)
                    {
                        runwayCentrelinePoints.AddRange(runwayCenterLinePointsDto.Select(rc => 
                        {
                            rc.RunwayDirectionName = runwayDirection.Designator;
                            return rc;
                        }).ToList());
                    }
                }
            }


            if (runwayCentrelinePoints == null)
                return Ok();

            return Json(runwayCentrelinePoints);
        }

        [Route("geojson/{rwyDirectionIdentifier}/{workPackage}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetRunwayCenterLinePointFeatureCollection(Guid rwyDirectionIdentifier, int workPackage)
        {
            var featureCollection = _repository.GetRunwayCenterlinePointFeatureCollection(workPackage, rwyDirectionIdentifier);

            if (featureCollection == null)
                return Ok();

            return Json(featureCollection);
        }

        [Route("~/runwaycenterlinepointbyadhp/geojson/{adhpIdentifier}/{workPackage}/{role?}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetRunwayCenterLinePointFeatureCollectionByAirportHelipor(Guid adhpIdentifier, int workPackage, CodeRunwayPointRole? role)
        {
            var runways = _runwayRepository.GetRunways(workPackage, adhpIdentifier);
            var runwayDirections = new List<RunwayDirection>();
            var featureCollection = new FeatureCollection();

            if (runways != null)
            {
                foreach (var runway in runways)
                {
                    var runwayDirectionsByRunway = _runwayDirectionRepository.GetRunwayDirections(workPackage, runway.Identifier);
                    if(runwayDirectionsByRunway != null && runwayDirectionsByRunway.Count() > 0)
                        runwayDirections.AddRange(_runwayDirectionRepository.GetRunwayDirections(workPackage, runway.Identifier));
                }
            }

            if (runwayDirections != null)
            {
                featureCollection = _repository.GetRunwayCenterlinePointFeatureCollectionByRunwayDirections(workPackage, runwayDirections, role);
            }


            if (featureCollection == null)
                return Ok();

            return Json(featureCollection);
        }
    }
}

/**/
