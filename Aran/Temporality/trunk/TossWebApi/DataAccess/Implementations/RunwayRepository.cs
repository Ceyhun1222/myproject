using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Temporality.Common.Id;
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
    public class RunwayRepository: IRunwayRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public RunwayRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }

        public IEnumerable<RunwayDto> GetRunwayDtos(int workPackage, Guid adhpIdentifier)
        {
            var runways = GetRunways(workPackage, adhpIdentifier);

            if (runways == null)
                return null;

            return runways.Select(oa => Mapper.Map<RunwayDto>(oa));
        }

        public IEnumerable<Runway> GetRunways(int workPackage, Guid adhpIdentifier)
        {

            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "associatedAirportHeliport", adhpIdentifier);
            var operChoice = new OperationChoice(compOper);
            var filter = new Filter(operChoice);

            var tossService = _tossServiceManager.GetDefaultTemporalityService();

            var date = DateTime.Now;
            if (workPackage != 0)
            {
                var noaixmService = _tossServiceManager.GetDefaultNoAixmDataService();
                var privateSlot = noaixmService.GetPrivateSlotById(workPackage);

                if (privateSlot == null)
                    return null;

                date = privateSlot.PublicSlot.EffectiveDate;
            }

            var featureId = new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Runway,
                WorkPackage = workPackage
            };

            var projection = Projection.Include("Identifier", "Designator");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection, filter: filter);

            if (result != null && result.Count > 0)
                return result.Select(t => (Runway)t.Data.Feature);

            return null;
        }

        public Runway GetRunway(int workPackage, Guid rwyIdentifier)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultTemporalityService();

            var date = DateTime.Now;
            if (workPackage != 0)
            {
                var noaixmService = _tossServiceManager.GetDefaultNoAixmDataService();
                var privateSlot = noaixmService.GetPrivateSlotById(workPackage);

                if (privateSlot == null)
                    return null;

                date = privateSlot.PublicSlot.EffectiveDate;
            }

            var featureId = new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Runway,
                WorkPackage = workPackage,
                Guid = rwyIdentifier
            };

            var projection = Projection.Include("Identifier", "Designator");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
                return (Runway)result.Select(t => t.Data.Feature).FirstOrDefault();

            return null;
        }
    }
}