using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Temporality.Common.Id;
using AutoMapper;
using GeoJSON.Net;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TossWebApi.Common;
using TossWebApi.DataAccess.Interfaces;
using TossWebApi.Models.DTO;
using TossWebApi.Utils;

namespace TossWebApi.DataAccess.Implementations
{
    public class VerticalStructureRepository : IVerticalStructureRepository
    {
        #region props
        private readonly ITossServicesManager _tossServiceManager;
        #endregion

        #region ctor
        public VerticalStructureRepository(ITossServicesManager tossServicesManager)
        {
            _tossServiceManager = tossServicesManager;
        }
        #endregion

        #region dto
        public IEnumerable<VerticalStructureDto> GetVerticalStructureDtos(int workPackage)
        {
            var verticalStructures = GetVerticalStructures(workPackage);
            if (verticalStructures == null)
                return null;

            var verticalStructureDtos = GetVerticalStructureDtosWithGeometry(verticalStructures);

            if (verticalStructureDtos == null)
                return null;

            return verticalStructureDtos;
        }

        public IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByBbox(int workPackage, double minX, double minY, double maxX, double maxY)
        {
            var verticalStructures = GetVerticalStructuresByBbox(workPackage, minX, minY, maxX, maxY);            
            if (verticalStructures == null)
                return null;

            var verticalStructureDtos = GetVerticalStructureDtosWithGeometry(verticalStructures);

            if (verticalStructureDtos == null)
                return null;

            return verticalStructureDtos;
        }

        public IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByObstacles(int workPackage, List<FeatureRefObject> featureRefs)
        {
            List<VerticalStructureDto> verticalStructureDtos = new List<VerticalStructureDto>();

            if (featureRefs != null && featureRefs.Count > 0)
            {
                var filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Identifier",
                    featureRefs.Select(x => x.Feature.Identifier).ToList())));

                var projection = Projection.Include("Identifier", "Name", "Type", "Part");

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

                var result = tossService.GetActualDataByDate(
                    new FeatureId { WorkPackage = workPackage, FeatureTypeId = (int)FeatureType.VerticalStructure },
                    false, date, filter: filter, projection: projection);

                if (result != null)
                {
                    foreach (var abstractState in result)
                    {
                        verticalStructureDtos.AddRange(Mapper.Map<List<VerticalStructureDto>>(abstractState.Data.Feature));
                    }
                }
            }

            return verticalStructureDtos;
        }

        public IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByAdhpRadius(int workPackage, Aran.Geometries.Point adhpPoint, double radius)
        {
            var verticalStructures = GetVerticalStructuresByAdhpRadius(workPackage, adhpPoint, radius);
            if (verticalStructures == null)
                return null;

            var verticalStructureDtos = GetVerticalStructureDtosWithGeometry(verticalStructures);

            if (verticalStructureDtos == null)
                return null;

            return verticalStructureDtos;
        }
        #endregion

        #region getter
        public IEnumerable<VerticalStructure> GetVerticalStructures(int workPackage, Guid? identifier = null)
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
                FeatureTypeId = (int)FeatureType.VerticalStructure,
                WorkPackage = workPackage,
                Guid = identifier
            };

            var projection = Projection.Include("Identifier", "Name", "Type", "Part");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
                return result.Select(t => (VerticalStructure)t.Data.Feature);

            return null;
        }

        public IEnumerable<VerticalStructure> GetVerticalStructuresByBbox(int workPackage, double minX, double minY, double maxX, double maxY)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var filter = GetVerticalStructureByBboxFilter(minX, minY, maxX, maxY);

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
                FeatureTypeId = (int)FeatureType.VerticalStructure,
                WorkPackage = workPackage
            };

            var projection = Projection.Include("Identifier", "Name", "Type", "Part");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection, filter: filter);

            if (result != null && result.Count > 0)
                return result.Select(t => (VerticalStructure)t.Data.Feature);

            return null;
        }

        public ObstacleArea GetObstacleByObstacleArea(int workPackage, Guid oaIdentifier)
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
                FeatureTypeId = (int)FeatureType.ObstacleArea,
                WorkPackage = workPackage,
                Guid = oaIdentifier
            };

            var projection = Projection.Include("Obstacle");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
            {
                return (ObstacleArea)result.Select(t => t.Data.Feature).FirstOrDefault();
            }

            return null;
        }

        private VerticalStructure GetVerticalStructureByIdentifier(int workPackage, Guid identifier)
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
                FeatureTypeId = (int)FeatureType.VerticalStructure,
                WorkPackage = workPackage,
                Guid = identifier
            };

            var projection = Projection.Include("Identifier", "Name", "Type", "Part");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection);

            if (result != null && result.Count > 0)
                return (VerticalStructure)result.Select(t => t.Data.Feature).FirstOrDefault();

            return null;
        }

        public IEnumerable<VerticalStructure> GetVerticalStructuresByAdhpRadius(int workPackage, Aran.Geometries.Point adhpPoint, double radius)
        {
            if (_tossServiceManager == null)
                throw new Exception("Toss Service is empty");

            var filter = GetVerticalStructureByAdhpRadiusFilter(adhpPoint, radius);

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
                FeatureTypeId = (int)FeatureType.VerticalStructure,
                WorkPackage = workPackage
            };

            var projection = Projection.Include("Identifier", "Name", "Type", "Part");

            var result = tossService.GetActualDataByDate(featureId, false, date, projection: projection, filter: filter);

            if (result != null && result.Count > 0)
                return result.Select(t => (VerticalStructure)t.Data.Feature);

            return null;
        }
        #endregion

        #region geojson
        public FeatureCollection GetVerticalStructureFeatureCollection(int workPackage)
        {
            var verticalStructures = GetVerticalStructures(workPackage);

            if (verticalStructures == null)
                return null;

            var verticalStructureFeatures = new List<GeoJSON.Net.Feature.Feature>();
            foreach (var verticalStructure in verticalStructures)
            {
                verticalStructureFeatures.AddRange(Mapper.Map<List<GeoJSON.Net.Feature.Feature>>(verticalStructure));
            }

            return new FeatureCollection(verticalStructureFeatures);
        }

        public FeatureCollection GetVerticalStructureFeatureCollectionByBbox(int workPackage, double minX, double minY, double maxX, double maxY)
        {
            var verticalStructures = GetVerticalStructuresByBbox(workPackage, minX, minY, maxX, maxY);

            if (verticalStructures == null)
                return null;

            var verticalStructureFeatures = new List<GeoJSON.Net.Feature.Feature>();
            foreach (var verticalStructure in verticalStructures)
            {
                verticalStructureFeatures.AddRange(Mapper.Map<List<GeoJSON.Net.Feature.Feature>>(verticalStructure));
            }

            return new FeatureCollection(verticalStructureFeatures);
        }

        public FeatureCollection GetVerticalStructureFeatureCollectionByObstacles(int workPackage, List<FeatureRefObject> featureRefs)
        {
            var verticalStructureFeatures = new List<GeoJSON.Net.Feature.Feature>();

            if (featureRefs != null && featureRefs.Count > 0)
            {
                var filter = new Filter(new OperationChoice(new ComparisonOps(ComparisonOpType.In, "Identifier",
                    featureRefs.Select(x => x.Feature.Identifier).ToList())));

                var projection = Projection.Include("Identifier", "Name", "Type", "Part");

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

                var result = tossService.GetActualDataByDate(
                    new FeatureId {WorkPackage = workPackage, FeatureTypeId = (int) FeatureType.VerticalStructure},
                    false, date, filter: filter, projection: projection);

                if (result != null)
                {
                    foreach (var abstractState in result)
                    {
                        verticalStructureFeatures.AddRange(
                            Mapper.Map<List<GeoJSON.Net.Feature.Feature>>(
                                abstractState.Data.Feature as VerticalStructure));
                    }
                }
            }
            
            return new FeatureCollection(verticalStructureFeatures);
        }

        public FeatureCollection GetVerticalStructureFeatureCollectionByAdhpRadius(int workPackage, Aran.Geometries.Point adhpPoint, double radius)
        {
            var verticalStructures = GetVerticalStructuresByAdhpRadius(workPackage, adhpPoint, radius);

            if (verticalStructures == null)
                return null;

            var verticalStructureFeatures = new List<GeoJSON.Net.Feature.Feature>();
            foreach (var verticalStructure in verticalStructures)
            {
                verticalStructureFeatures.AddRange(Mapper.Map<List<GeoJSON.Net.Feature.Feature>>(verticalStructure));
            }

            return new FeatureCollection(verticalStructureFeatures);
        }
        #endregion
        

        private IEnumerable<VerticalStructureDto> GetVerticalStructureDtosWithGeometry(IEnumerable<VerticalStructure> verticalStructures)
        {
            var verticalStructureDtos = new List<VerticalStructureDto>();

            foreach (var verticalStructure in verticalStructures)
            {
                verticalStructureDtos.AddRange(Mapper.Map<List<VerticalStructureDto>>(verticalStructure));
            }

            return verticalStructureDtos;
        }

        private Filter GetVerticalStructureByBboxFilter(double minX, double minY, double maxX, double maxY)
        {
            var binaryLogicalOperation = new BinaryLogicOp();

            var spatialOpsPoint = new InExtend
            {
                PropertyName = "Part.HorizontalProjection.Location.Geo",
                MinX = minX,
                MaxX = maxX,
                MinY = minY,
                MaxY = maxY
            };

            var spatialOpsMultiLineString = new InExtend
            {
                PropertyName = "Part.HorizontalProjection.LinearExtent.Geo",
                MinX = minX,
                MaxX = maxX,
                MinY = minY,
                MaxY = maxY
            };

            var spatialOpsMultiPolygon = new InExtend
            {
                PropertyName = "Part.HorizontalProjection.SurfaceExtent.Geo",
                MinX = minX,
                MaxX = maxX,
                MinY = minY,
                MaxY = maxY
            };

            var operChoicePoint = new OperationChoice(spatialOpsPoint);
            var operChoiceMultiLineString = new OperationChoice(spatialOpsMultiLineString);
            var operChoiceMultiPolygon = new OperationChoice(spatialOpsMultiPolygon);

            binaryLogicalOperation.Type = BinaryLogicOpType.Or;
            binaryLogicalOperation.OperationList.Add(operChoicePoint);
            binaryLogicalOperation.OperationList.Add(operChoiceMultiLineString);
            binaryLogicalOperation.OperationList.Add(operChoiceMultiPolygon);

            var filterOperChoice = new OperationChoice(binaryLogicalOperation);

            return new Filter(filterOperChoice);
        }

        private Filter GetVerticalStructureByAdhpRadiusFilter(Aran.Geometries.Point adhpPoint, double radius)
        {
            var binaryLogicalOperation = new BinaryLogicOp();

            var spatialOpsPoint = new DWithin
            {
                PropertyName = "Part.HorizontalProjection.Location.Geo",
                Distance = new ValDistance
                {
                    Value = radius,
                    Uom = UomDistance.M
                },
                Point = adhpPoint
            };

            var spatialOpsMultiLineString = new DWithin
            {
                PropertyName = "Part.HorizontalProjection.LinearExtent.Geo",
                Distance = new ValDistance
                {
                    Value = radius,
                    Uom = UomDistance.M
                },
                Point = adhpPoint
            };

            var spatialOpsMultiPolygon = new DWithin
            {
                PropertyName = "Part.HorizontalProjection.SurfaceExtent.Geo",
                Distance = new ValDistance
                {
                    Value = radius,
                    Uom = UomDistance.M
                },
                Point = adhpPoint
            };

            var operChoicePoint = new OperationChoice(spatialOpsPoint);
            var operChoiceMultiLineString = new OperationChoice(spatialOpsMultiLineString);
            var operChoiceMultiPolygon = new OperationChoice(spatialOpsMultiPolygon);

            binaryLogicalOperation.Type = BinaryLogicOpType.Or;
            binaryLogicalOperation.OperationList.Add(operChoicePoint);
            binaryLogicalOperation.OperationList.Add(operChoiceMultiLineString);
            binaryLogicalOperation.OperationList.Add(operChoiceMultiPolygon);

            var filterOperChoice = new OperationChoice(binaryLogicalOperation);

            return new Filter(filterOperChoice);
        }
    }
}