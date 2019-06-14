using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OmsApi.Data;
using OmsApi.Dto;
using OmsApi.Entity;
using OmsApi.Interfaces;
using OmsApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Roles.Admin))]
    //[Consumes("application/json")]
    [Produces("application/json")]
    public class SlotsController : BaseController
    {
        private ITossClient _tossClient;
        private readonly ILogger<SlotsController> _logger;

        public SlotsController(ITossClient tossClient, ILogger<SlotsController> logger)
        {
            _tossClient = tossClient;
            _logger = logger;
        }


        [HttpGet]
        [SwaggerOperation("Gets public slots which includes private slots")]
        [SwaggerResponse((int)StatusCodes.Status400BadRequest)]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<PublicSlotDto>>> GetPublicSlots()
        {
            try
            {
                var response = await _tossClient.GetPublicSlots();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPublicSlot error");
                return BadRequest();
            }
        }

        [HttpPost]
        [SwaggerOperation("Sets default slot")]
        [SwaggerResponse((int)StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Post(SlotDto value, [FromServices]ApplicationDbContext dbContext, [FromServices] IMapper mapper)
        {
            try
            {
                var slot = mapper.Map<Slot>(value);
                await dbContext.SetSelectedSlot(slot);                
            }
            catch (Exception ex)
            {
                var k = ex.Message;
            }
            return NoContent();
        }

        [HttpGet("Selected")]
        [SwaggerOperation("Gets default slot")]
        [SwaggerResponse((int)StatusCodes.Status200OK)]
        [SwaggerResponse((int)StatusCodes.Status204NoContent,"Not selected yet")]
        public async Task<ActionResult<SlotDto>> GetSelectedSlot([FromServices]ApplicationDbContext dbContext, [FromServices] IMapper mapper)
        {
            var entry = await dbContext.GetSelectedSlot();
            if (entry != null)
            {
                try
                {
                    var k = mapper.Map<SlotDto>(entry);
                    return Ok(k);
                }
                catch (Exception ex)
                {
                    var b = ex.Message;
                }
            }
            return NoContent();
        }

    }
}
