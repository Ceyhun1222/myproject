using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using GeoJSON.Net.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TossWebApi.Models.DTO;

namespace TossWebApi.DataAccess.Interfaces
{
    public interface IVerticalStructureRepository
    {
        IEnumerable<VerticalStructureDto> GetVerticalStructureDtos(int workPackage);
        IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByBbox(int workPackage, double p1X, double p1Y, double p2X, double p2Y);
        IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByObstacles(int workPackage, List<FeatureRefObject> featureRefs);
        IEnumerable<VerticalStructureDto> GetVerticalStructureDtosByAdhpRadius(int workPackage, Point adhpPoint, double radius);

        IEnumerable<VerticalStructure> GetVerticalStructures(int workPackage, Guid? identifier = null);
        IEnumerable<VerticalStructure> GetVerticalStructuresByBbox(int workPackage, double p1X, double p1Y, double p2X, double p2Y);

        FeatureCollection GetVerticalStructureFeatureCollection(int workPackage);
        FeatureCollection GetVerticalStructureFeatureCollectionByBbox(int workPackage, double minX, double minY, double maxX, double maxY);
        FeatureCollection GetVerticalStructureFeatureCollectionByObstacles(int workPackage, List<FeatureRefObject> featureRefs);
        FeatureCollection GetVerticalStructureFeatureCollectionByAdhpRadius(int workPackage, Point adhpPoint, double radius);
        
        ObstacleArea GetObstacleByObstacleArea(int workPackage, Guid oaIdentifier);
    }
}
