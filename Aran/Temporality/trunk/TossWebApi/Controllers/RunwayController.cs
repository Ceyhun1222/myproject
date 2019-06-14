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
    [RoutePrefix("runway")]
    public class RunwayController : ApiController
    {
        private readonly IRunwayRepository _repository;

        public RunwayController(IRunwayRepository repository)
        {
            _repository = repository;
        }

        [Route("{ahdpIdentifier}/{workPackage}")]
        [ResponseType(typeof(IEnumerable<RunwayDto>))]
        public IHttpActionResult GetRunways(Guid ahdpIdentifier, int workPackage)
        {
            var features = _repository.GetRunwayDtos(workPackage, ahdpIdentifier);

            if(features == null)
                return Ok();

            return Json(features);
        }
    }
}
