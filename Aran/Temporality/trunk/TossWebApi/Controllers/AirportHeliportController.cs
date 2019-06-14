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
    [RoutePrefix("airportheliport")]
    public class AirportHeliportController : ApiController
    {
        private readonly IAirportHeliportRepository _repository;

        public AirportHeliportController(IAirportHeliportRepository repository)
        {
            _repository = repository;
        }

        [Route("{workPackage}")]
        [ResponseType(typeof(IEnumerable<AirportHeliportDto>))]
        public IHttpActionResult GetAirportHeliports(int workPackage)
        {
            var features = _repository.GetAirportHeliportDtos(workPackage);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("{adhpIdentifier}/{workPackage}")]
        [ResponseType(typeof(AirportHeliportDto))]
        public IHttpActionResult GetAirportHeliportByIdentifier(Guid adhpIdentifier, int workPackage)
        {
            var features = _repository.GetAirportHeliportByIdentifierDtos(workPackage, adhpIdentifier);

            if (features == null)
                return Ok();

            return Json(features);
        }

        [Route("geojson/{workPackage}")]
        [ResponseType(typeof(FeatureCollection))]
        public IHttpActionResult GetAirportHeliportFeatureCollection(int workPackage)
        {
            var featureCollection = _repository.GetAirportHeliportFeatureCollection(workPackage);

            if (featureCollection == null)
                return Ok();

            return Json(featureCollection);
        }
    }
}
