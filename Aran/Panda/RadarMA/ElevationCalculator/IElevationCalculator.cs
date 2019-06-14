using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.ElevationCalculator
{
    interface IObstacleReportCalculator
    {
        IEnumerable<ObstacleReport> GetReports(IPolygon stateGeo);
    }
}
