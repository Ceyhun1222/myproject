using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace PVT.Model
{
    public class ObstacleAssessmentArea
    {
        public Aran.Geometries.MultiPolygon Geo { get; }
        public string Type { get; }
        public ValDistanceVertical AssessedAltitude { get;}
        public ValDistanceVertical SlopeLowerAltitude { get;  }
        public double? GradientLowHigh { get; }
        public CodeObstructionIdSurfaceZone? SurfaceZone { get; }
        public List<AircraftCharacteristic> AircraftCategory { get; }

        public string AircraftCategoryString
        {
            get
            {
                if (AircraftCategory == null)
                    return null;
                return string.Join(",", AircraftCategory.Select(t => t.Type.ToString()).ToArray());
            }
        }

        public List<VerticalStructure> Obstacles
        {
            get {
                Fill();
                return _obstacles;
            }
        }

        private bool _filled;

        private readonly List<Aran.Aim.Features.Obstruction> _obstructions = new List<Aran.Aim.Features.Obstruction>();
        private readonly List<VerticalStructure> _obstacles;

        public ObstacleAssessmentArea(Aran.Aim.Features.ObstacleAssessmentArea area)
        {
            Type = area.Type?.ToString();
            Geo = area.Surface.Geo;
            AssessedAltitude = area.AssessedAltitude;
            SlopeLowerAltitude = area.SlopeLowerAltitude;
            GradientLowHigh = area.GradientLowHigh;
            SurfaceZone = area.SurfaceZone;
            AircraftCategory = area.AircraftCategory;

            _obstacles = new List<VerticalStructure>();
            if (area.SignificantObstacle != null)
                _obstructions.AddRange(area.SignificantObstacle);

        }

        private void Fill()
        {
            if (_filled) return;
            if (_obstructions.Count <= 0)
            {
                _filled = true;
                return;
            }
            var obs =
                Engine.Environment.Current.DbProvider.GetVerticalStructures(
                    _obstructions.Select(x => x.VerticalStructureObstruction.Identifier).ToList<Guid>());
            _obstacles.AddRange(obs.Select(x => new VerticalStructure(x)).ToList<VerticalStructure>());
            _filled = true;
        }
    }
}
