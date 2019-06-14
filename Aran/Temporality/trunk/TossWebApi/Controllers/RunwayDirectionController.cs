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
    [RoutePrefix("runwaydirection")]
    public class RunwayDirectionController : ApiController
    {
        private readonly IRunwayDirectionRepository _repository;

        public RunwayDirectionController(IRunwayDirectionRepository repository)
        {
            _repository = repository;
        }

        [Route("{rwyIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<RunwayDirectionDto>))]
        public IHttpActionResult GetRunwayDirections(Guid rwyIdentifier, int workPackage)
        {
            var features = _repository.GetRunwayDirectionDtos(workPackage, rwyIdentifier);

            if (features == null)
                return Ok();

            return Json(features);
        }
    }
}
