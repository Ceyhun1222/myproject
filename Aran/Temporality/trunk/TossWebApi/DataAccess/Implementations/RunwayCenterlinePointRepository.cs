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
    public class RunwayCenterlinePointRepository : IRunwayCenterlinePointRepository
    {
        private readonly ITossServicesManager _tossServiceManager;

        public RunwayCenterlinePointRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }

        public IEnumerable<RunwayCenterlinePointDto> GetRunwayCenterlinePointDtos(int workPackage, Guid rwyDirectionIdentifier, CodeRunwayPointRole? role = null)
        {
            var runwayCentrelinePoints = GetRunwayCenterlinePoints(workPackage, rwyDirectionIdentifier, role);

            if (runwayCentrelinePoints == null)
                return null;

            return runwayCentrelinePoints.Select(oa => Mapper.Map<RunwayCenterlinePointDto>(oa));
        }
        
        public FeatureCollection GetRunwayCenterlinePointFeatureCollection(int workPackage, Guid rwyDirectionIdentifier)
        {
            var runwayCentrelinePoints = GetRunwayCenterlinePoints(workPackage, rwyDirectionIdentifier);

            if (runwayCentrelinePoints == null)
                return null;

            var runwayCentrelinePointFeatures = runwayCentrelinePoints.Select(rcp => Mapper.Map<GeoJSON.Net.Feature.Feature>(rcp)).ToList();

            return new FeatureCollection(runwayCentrelinePointFeatures);
        }

        public FeatureCollection GetRunwayCenterlinePointFeatureCollectionByRunwayDirections(int workPackage, List<RunwayDirection> runwayDirections, CodeRunwayPointRole? role = null)
        {
            if (runwayDirections == null || runwayDirections.Count == 0)
                return null;

            //var runwayCentrelinePoints = new List<RunwayCentrelinePoint>();
            var runwayCentrelinePointFeatures = new List<GeoJSON.Net.Feature.Feature>();

            foreach (var runwayDirection in runwayDirections)
            {
                var runwayCentrelinePoints = GetRunwayCenterlinePoints(workPackage, runwayDirection.Identifier, role);
                var runwayCenterlinePointsFeaturesByRunwayDirection = runwayCentrelinePoints?.Select(rcp => Mapper.Map<GeoJSON.Net.Feature.Feature>(rcp)).ToList();

                runwayCenterlinePointsFeaturesByRunwayDirection = runwayCenterlinePointsFeaturesByRunwayDirection?.Select(rcp =>
                {
                    rcp.Properties.Add("RunwayDirectionName", runwayDirection.Designator);
                    return rcp;
                }).ToList();

                if(runwayCenterlinePointsFeaturesByRunwayDirection != null)
                    runwayCentrelinePointFeatures.AddRange(runwayCenterlinePointsFeaturesByRunwayDirection);
            }

            if (runwayCentrelinePointFeatures == null)
                return null;

            //runwayCentrelinePointFeatures = runwayCentrelinePoints.Select(rcp => Mapper.Map<GeoJSON.Net.Feature.Feature>(rcp)).ToList();
            return new FeatureCollection(runwayCentrelinePointFeatures);
        }

        public IEnumerable<RunwayCentrelinePoint> GetRunwayCenterlinePoints(int workPackage, Guid rwyDirectionIdentifier, CodeRunwayPointRole? role = null)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var binaryLogicalOperation = new BinaryLogicOp
            {
                Type = BinaryLogicOpType.And
            };

            var rwyDirCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "OnRunway", rwyDirectionIdentifier);
            var rwyDirCompOperChoice = new OperationChoice(rwyDirCompOper);
            binaryLogicalOperation.OperationList.Add(rwyDirCompOperChoice);

            if( role != null )
            {
                var rwyPointRoleCompOper = new ComparisonOps(ComparisonOpType.EqualTo, "Role", role);
                var rwyPointRoleCompOperChoice = new OperationChoice(rwyPointRoleCompOper);
                binaryLogicalOperation.OperationList.Add(rwyPointRoleCompOperChoice);
            }

            var filterOperChoice = new OperationChoice(binaryLogicalOperation);
            var filter = new Filter(filterOperChoice);


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
                FeatureTypeId = (int)FeatureType.RunwayCentrelinePoint,
                WorkPackage = workPackage
            };

            var projection = Projection.Include("Identifier", "Designator", "Role");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection, filter: filter);

            if (result != null && result.Count > 0)
                return result.Select(t => (RunwayCentrelinePoint)t.Data.Feature);

            return null;
        }
    }
}
