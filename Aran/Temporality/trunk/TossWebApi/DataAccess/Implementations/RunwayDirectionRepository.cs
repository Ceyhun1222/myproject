using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Temporality.Common.Id;
using AutoMapper;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.DataAccess.Implementations
{
    public class RunwayDirectionRepository : IRunwayDirectionRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public RunwayDirectionRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }

        public IEnumerable<RunwayDirectionDto> GetRunwayDirectionDtos(int workPackage, Guid rwyIdentifier)
        {
            var runwayDirections = GetRunwayDirections(workPackage, rwyIdentifier);

            if (runwayDirections == null)
                return null;

            return runwayDirections.Select(oa => Mapper.Map<RunwayDirectionDto>(oa));
        }

        public IEnumerable<RunwayDirection> GetRunwayDirections(int workPackage, Guid rwyIdentifier)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "usedRunway", rwyIdentifier);
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
                FeatureTypeId = (int)FeatureType.RunwayDirection,
                WorkPackage = workPackage
            };

            var projection = Projection.Include("Identifier", "Designator");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection, filter: filter);

            if (result != null && result.Count > 0)
                return result.Select(t => (RunwayDirection)t.Data.Feature);

            return null;
        }

        public RunwayDirection GetRunwayDirection(int workPackage, Guid rwyDirectionIdentifier)
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
                FeatureTypeId = (int)FeatureType.RunwayDirection,
                WorkPackage = workPackage,
                Guid = rwyDirectionIdentifier
            };

            var projection = Projection.Include("Identifier", "Designator");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
                return (RunwayDirection)result.Select(t => t.Data.Feature).FirstOrDefault();

            return null;
        }

        private Filter GetRunwayDirectionsFilter(List<Guid> runwayIdentifiers)
        {
            var binaryLogicalOperation = new BinaryLogicOp();
            binaryLogicalOperation.Type = BinaryLogicOpType.Or;

            runwayIdentifiers.ForEach(rwyDirIdentifier =>
            {
                var rwyDirCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "usedRunway", rwyDirIdentifier);
                var rwyDirCompOperChoice = new OperationChoice(rwyDirCompOper);

                binaryLogicalOperation.OperationList.Add(rwyDirCompOperChoice);
            });

            var filterOperChoice = new OperationChoice(binaryLogicalOperation);

            return new Filter(filterOperChoice);
        }

        public List<RunwayDirection> GetRunwayDirections(int workPackage, List<Guid> runwayIdentifiers)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var filter = GetRunwayDirectionsFilter(runwayIdentifiers);

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
                FeatureTypeId = (int)FeatureType.RunwayDirection,
                WorkPackage = workPackage
            };

            var result = tossService.GetActualDataByDate(featureId, false, date, filter: filter);
            if (result != null && result.Count > 0)
            {
                return result.Select(t => (RunwayDirection)t.Data.Feature).ToList();
            }

            return null;
        }
    }
}