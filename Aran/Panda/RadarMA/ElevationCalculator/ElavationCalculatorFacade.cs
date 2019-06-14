using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RadarMA.Models;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ElevationCalculator
{
    public class ElavationCalculatorFacade
    {
        private readonly List<VerticalStructure> _vsList;
        private readonly IRaster _raster;
        private UnitConverter _unitConverter;

        public ElavationCalculatorFacade(List<VerticalStructure> vsList,IRaster raster,UnitConverter unitConverter)
        {
            _vsList = vsList;
            _raster = raster;
            _unitConverter = unitConverter;
        }

        public List<ObstacleReport> GetObstacleReports(IPolygon stateGeo)
        {
            var obstacleElevationCalculator = new ObstacleElevationCalculator(_vsList, _unitConverter);
            var obstacleReports = obstacleElevationCalculator.GetReports(stateGeo).ToList();

            var rasterElevationCalculator = new RasterObstacleReportCalculator(_raster, _unitConverter);
            var rasterReports = rasterElevationCalculator.GetReports(stateGeo);

            obstacleReports.ToList().AddRange(rasterReports);

            return obstacleReports;
        }
    }
}
