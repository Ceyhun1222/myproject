using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RadarMA.Models;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ElevationCalculator
{
    class ObstacleElevationCalculator:IObstacleReportCalculator
    {
        private const string SourceName = "Verticalstructure";
        private readonly List<VerticalStructure> _vsList;
        private UnitConverter _unitConverter;

        public ObstacleElevationCalculator(List<VerticalStructure> vsList,UnitConverter unitConverter)
        {
            _vsList = vsList;
            _unitConverter = unitConverter;
        }

        public IEnumerable<ObstacleReport> GetReports(IPolygon stateGeo)
        {
            if (_vsList==null || _vsList.Count == 0)
                return new List<ObstacleReport>();

            var obstacleWithinStateArea = _vsList.Where(vs => IsInside(stateGeo, vs.GeoPrj))
                .OrderByDescending(vs => vs.Elevation)
                .Select(vs => new ObstacleReport(vs.Name, vs.Geo,vs.GeoPrj, vs.Elevation, SourceName));

            return obstacleWithinStateArea;
        }

        public static bool IsInside(IPolygon poly, IGeometry insideGeo)
        {
            IRelationalOperator relOper = poly as IRelationalOperator;
            return relOper != null && !relOper.Disjoint(insideGeo);
        }
    }
}
