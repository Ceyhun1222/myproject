using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.CommonUtil.Context;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.DataAccess.Implementations
{
    public class SlotRepository : ISlotRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public SlotRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }

        public IEnumerable<PrivateSlotDto> GetPrivateSlots(int publicSlotId, SlotStatus[] excludedSlotStatuses = null, int userId = 0)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            if (userId == 0)
                userId = CurrentDataContext.UserId;

            var tossService = _tossServiceManager.GetDefaultNoAixmDataService();

            var privateSlots = tossService.GetPrivateSlots(publicSlotId, userId);

            if (excludedSlotStatuses != null && excludedSlotStatuses.Count() > 0)
                privateSlots = privateSlots.Where(ps => !excludedSlotStatuses.Contains(ps.Status)).ToList();

            return privateSlots.Select(ps => Mapper.Map<PrivateSlotDto>(ps));
        }

        public IEnumerable<PublicSlotDto> GetPublicSlots(SlotStatus[] excludedSlotStatuses = null, PublicSlotType[] includedSlotTypes = null)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultNoAixmDataService();
            
            var publicSlots = tossService.GetPublicSlots();

            if (excludedSlotStatuses != null && excludedSlotStatuses.Count() > 0)
                publicSlots = publicSlots.Where(ps => !excludedSlotStatuses.Contains(ps.Status)).ToList();

            if (includedSlotTypes != null && includedSlotTypes.Count() > 0)
                publicSlots = publicSlots.Where(ps => includedSlotTypes.Contains((PublicSlotType)ps.SlotType)).ToList();

            return publicSlots.Select(ps => Mapper.Map<PublicSlotDto>(ps));
        }

        public IEnumerable<PublicSlotDto> GetSlots(SlotStatus[] excludedSlotStatuses = null, PublicSlotType[] includedSlotTypes = null, int userId = 0)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultNoAixmDataService();

            var publicSlots = tossService.GetPublicSlots();

            if (excludedSlotStatuses != null && excludedSlotStatuses.Count() > 0)
                publicSlots = publicSlots.Where(ps => !excludedSlotStatuses.Contains(ps.Status)).ToList();

            if (includedSlotTypes != null && includedSlotTypes.Count() > 0)
                publicSlots = publicSlots.Where(ps => includedSlotTypes.Contains((PublicSlotType)ps.SlotType)).ToList();

            var publicSlotsDto = new List<PublicSlotDto>();
            if (publicSlots != null)
            {
                publicSlotsDto = publicSlots.Select(ps => Mapper.Map<PublicSlotDto>(ps)).ToList();

                if (userId == 0)
                    userId = CurrentDataContext.UserId;

                foreach (var publicSlotDto in publicSlotsDto)
                {
                    var privateSlots = tossService.GetPrivateSlots(publicSlotDto.Id, userId);
                    
                    publicSlotDto.PrivateSlots = privateSlots.Select(ps => Mapper.Map<PrivateSlotDto>(ps)).ToList();
                }
            }

            return publicSlotsDto;
        }

        public int CreatePublicSlot(PublicSlotDto publicSlotDto)
        {
            if (publicSlotDto.EffectiveDate.Date.CompareTo(DateTime.Today) <= 0)
                return 0;

            var publicSlot = Mapper.Map<PublicSlot>(publicSlotDto);

            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultNoAixmDataService();

            return tossService.CreatePublicSlot(publicSlot);
        }

        public int CreatePrivateSlot(int publicSlotId, PrivateSlotDto privateSlotDto)
        {
            var privateSlot = Mapper.Map<PrivateSlot>(privateSlotDto);

            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultNoAixmDataService();

            privateSlot.PublicSlot = tossService.GetPublicSlotById(publicSlotId);

            return tossService.CreatePrivateSlot(privateSlot);
        }
    }
}