using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Id;
using AutoMapper;
using GeoJSON.Net.Feature;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.DataAccess.Implementations
{
    public class ObstacleAreaRepository : IObstacleAreaRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public ObstacleAreaRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }
    
        private Filter GetObstacleAreaFilter(Guid adhpIdentifier, List<Guid> rwyDirs, List<CodeObstacleArea> includedTypes = null)
        {
            var binaryLogicalOperation = new BinaryLogicOp();

            var compOper = new ComparisonOps(ComparisonOpType.EqualTo, "reference.ownerAirport", adhpIdentifier);
            var operChoice = new OperationChoice(compOper);

            binaryLogicalOperation.OperationList.Add(operChoice);
            binaryLogicalOperation.Type = BinaryLogicOpType.Or;

            rwyDirs.ForEach(rwyDir =>
            {
                var rwyDirCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "reference.ownerRunway", rwyDir);
                var rwyDirCompOperChoice = new OperationChoice(rwyDirCompOper);

                binaryLogicalOperation.OperationList.Add(rwyDirCompOperChoice);
            });


            if (includedTypes?.Any() == true)
            {
                var mainBinaryLogicalOperation = new BinaryLogicOp { Type = BinaryLogicOpType.And };

                mainBinaryLogicalOperation.OperationList.Add(new OperationChoice(binaryLogicalOperation));
                mainBinaryLogicalOperation.OperationList.Add(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Type", includedTypes)));

                return new Filter( new OperationChoice(mainBinaryLogicalOperation));
            }
            else
            {
                OperationChoice filterOperChoice = new OperationChoice(binaryLogicalOperation);
                return new Filter(filterOperChoice);
            }
        }

        public IEnumerable<ObstacleArea> GetObstacleAreaBaseOnAdhpAndRwy(int workPackage, Guid adhpIdentifier, List<Guid> rwyDirs, List<CodeObstacleArea> includedTypes)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var filter = GetObstacleAreaFilter(adhpIdentifier, rwyDirs, includedTypes);

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
                FeatureTypeId = (int)FeatureType.ObstacleArea,
                WorkPackage = workPackage
            };

		    var projection = Projection.Include("Identifier", "Type", "SurfaceExtent.Geo", "Reference.OwnerRunway");

            var result = tossService.GetActualDataByDate(featureId, false, date, filter: filter, projection: projection);

            if (result != null && result.Count > 0)
                return result.Select(t => (ObstacleArea)t.Data.Feature);

            return null;
        }

        public IEnumerable<ObstacleArea> GetObstacleAreas(int workPackage, List<Guid> identifiers, DateTime actualDate)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var tossService = _tossServiceManager.GetDefaultTemporalityService();

            var obstacleAreas = new List<ObstacleArea>();
            var errors = new List<Guid>();

            foreach (var identifier in identifiers)
            {

                var result = tossService.GetActualDataByDate(
                    new FeatureId { FeatureTypeId = (int)FeatureType.ObstacleArea, WorkPackage = workPackage, Guid = identifier }, 
                    false, actualDate);

                if (result.FirstOrDefault()?.Data?.Feature is ObstacleArea obstacleArea)
                    obstacleAreas.Add(obstacleArea);
                else
                    errors.Add(identifier);
            }

            if (errors.Count > 0)
                throw new Exception("Not found some obstacle areas: " + string.Join(",", errors));

            return obstacleAreas;
        }
    }
}