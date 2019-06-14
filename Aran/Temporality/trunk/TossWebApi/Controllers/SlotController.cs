using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
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
    [RoutePrefix("slot")]
    public class SlotController : ApiController
    {
        private readonly ISlotRepository _repository;

        public SlotController(ISlotRepository repository)
        {
            _repository = repository;
        }

        [Route("all")]
        [ResponseType(typeof(IEnumerable<PublicSlotDto>))]
        public IHttpActionResult GetSlots([FromUri] SlotStatus[] excludedSlotStatuses = null, [FromUri] PublicSlotType[] includedSlotTypes = null)
        {
            var publicSlots = _repository.GetSlots(excludedSlotStatuses, includedSlotTypes);

            if (publicSlots == null)
                return BadRequest();

            return Json(publicSlots);
        }

        [Route("public")]
        [ResponseType(typeof(IEnumerable<PublicSlotDto>))]
        public IHttpActionResult GetPublicSlots([FromUri] SlotStatus[] excludedSlotStatuses = null, [FromUri] PublicSlotType[] includedSlotTypes = null)
        {
            var publicSlots = _repository.GetPublicSlots(excludedSlotStatuses, includedSlotTypes);

            if (publicSlots == null)
                return BadRequest();

            return Json(publicSlots);
        }

        [Route("private/{publicSlotId}")]
        [ResponseType(typeof(IEnumerable<PrivateSlotDto>))]
        public IHttpActionResult GetPrivateSlots(int publicSlotId, [FromUri] SlotStatus[] excludedSlotStatuses = null)
        {

            var privateSlotds = _repository.GetPrivateSlots(publicSlotId, excludedSlotStatuses);

            if (privateSlotds == null)
                return BadRequest();

            return Json(privateSlotds);
        }


        [Route("public")]
        [ResponseType(typeof(int))]
        [HttpPost]
        public IHttpActionResult CreatePublicSlot(PublicSlotDto publicSlotDto)
        {
            var publicSlotId = _repository.CreatePublicSlot(publicSlotDto);

            if (publicSlotId == 0)
                return BadRequest();

            publicSlotDto.PrivateSlots.ForEach(privateSlot =>
                {
                    _repository.CreatePrivateSlot(publicSlotId, privateSlot);
                });

            return Json(publicSlotId);
        }

        [Route("private/{publicSlotId}")]
        [ResponseType(typeof(int))]
        [HttpPost]
        public IHttpActionResult CreatePrivateSlot(int publicSlotId, PrivateSlotDto privateSlotDto)
        {
            var privateSlot = _repository.CreatePrivateSlot(publicSlotId, privateSlotDto);

            if (privateSlot == 0)
                return BadRequest();

            return Json(privateSlot);
        }
    }
}
