using GeoJSON.Net.Feature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OmsApi.Data;
using OmsApi.Dto;
using OmsApi.Entity;
using OmsApi.Interfaces;
using OmsApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class DataController : BaseController
    {
        private ITossClient _tossClient;
        private readonly long _slotId;
        private readonly ILogger<DataController> _logger;

        public DataController(ITossClient tossClient, ApplicationDbContext dbContext, ILogger<DataController> logger)
        {
            _tossClient = tossClient;
            _slotId = GetSlotId(dbContext).GetAwaiter().GetResult();
            _logger = logger;
        }

        [HttpGet("Airports/Geojson")]
        [SwaggerOperation("Gets all airports in geojson format")]
        public async Task<ActionResult<FeatureCollection>> GetAirports()
        {
            var data = await _tossClient.GetAirports(_slotId);
            return Ok(data);
        }

        [HttpGet("Airports")]
        [SwaggerOperation("Gets all airports")]
        [AllowAnonymous]
        public async Task<ActionResult<IList<TossAirportHeliportDto>>> GetAirportDtos()
        {
            var data = await _tossClient.GetAirportDtos(_slotId);
            return Ok(data);
        }
        
        //[HttpGet("Runways/{arpId}")]
        //[SwaggerOperation("Gets all runways for the given airport")]
        //public async Task<ActionResult<IList<RunwayDto>>> GetRunwayDtos(string arpId)
        //{
        //    var data = await _tossClient.GetRunways(_slotId, arpId);
        //    return Ok(data);
        //}

        [HttpGet("ObstacleAreas/{requestId}")]
        [SwaggerOperation("Gets Obstacle Areas for the given aeroport")]
        public async Task<ActionResult<FeatureCollection>> GetObstacleAreas(long requestId, [FromServices] ApplicationDbContext dbContext)
        {
            var request = await dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest(NotFound());
            var data = await _tossClient.GetObstacleAreas(_slotId, request.AirportId);
            return Ok(data);
        }

        [HttpGet("VerticalStructuresDto")]
        [SwaggerOperation("Gets VerticalStructures for the test purposes")]
        [Authorize]
        public async Task<ActionResult<IList<IList<TossVerticalStructureDto>>>> GetVerticalStructureDtos([FromServices] ApplicationDbContext dbContext)
        {
            var username = User.Identity.Name;
            //if (string.IsNullOrEmpty(username))
            //    username = "admin";
            var user = await dbContext.Users.Include(t => t.Company).Where(t => t.UserName == username).FirstOrDefaultAsync();
            if (user == null)
                return BadRequest(NotFound());
            var data = await _tossClient.GetVerticalStructureDtos(_slotId, user.Company.AirportId, 15000);
            return Ok(data);
        }

        [HttpGet("VerticalStructures/{requestId}/{radius}")]
        [SwaggerOperation("Gets VerticalStructures in geoJson format")]
        public async Task<ActionResult<FeatureCollection>> GetVerticalStructureByCircle(long  requestId, double radius, [FromServices] ApplicationDbContext dbContext)
        {
            var request = await dbContext.Requests.Include(t => t.CreatedBy).Where(t => t.Id == requestId).FirstOrDefaultAsync();
            if (request == null)
                return BadRequest(NotFound());
            var data = await _tossClient.GetVerticalStructuresByCircle(_slotId, request.AirportId, radius);
            return Ok(data);
        }

        //[HttpGet("VerticalStructures/{obstacleAreaId}")]
        //[SwaggerOperation("Gets VerticalStructures for the given ObstacleArea")]
        //public async Task<ActionResult<FeatureCollection>> GetVerticalStructuresByObstacleArea(string obstacleAreaId)
        //{
        //    var data = await _tossClient.GetVerticalstructuresByObstacleArea(_slotId, obstacleAreaId);
        //    return Ok(data);
        //}

        //[HttpGet("RunwayCenterlinePoints/{arpId}")]
        //public async Task<ActionResult<FeatureCollection>> GetRunwayCenterlinePoints(string arpId)
        //{
        //    var data = await _tossClient.GetRunwayCentrelinePoints(_slotId, arpId);
        //    return Ok(data);
        //}

        private async Task<long> GetSlotId(ApplicationDbContext dbContext)
        {
            var slot = await dbContext.GetSelectedSlot();
            var slotId = 0L;
            if (slot != null)
                slotId = slot.Private.TossId;
            return slotId;
        }
    }
}