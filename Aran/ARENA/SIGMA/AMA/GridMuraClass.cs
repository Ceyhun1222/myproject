using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    public class GridMuraClass
    {
        private IPolygon _geo;

        public IPolygon Geo
        {
            get { return _geo; }
            set
            {
                _geo = value;
                //if (_geo != null)
                //{
                //    var area = _geo as IArea;
                //    CenterPoint = area.Centroid;
                //}
            }
        }

        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
        public double? Elevation { get; set; }
        public IPoint CenterPoint 
        {
            get
            {
                var area = _geo as IArea;
                var centerPoint = area.Centroid;
                if (centerPoint!=null)
                    return new PointClass { X = centerPoint.X, Y = centerPoint.Y };
                return null;
            }
        }

        public string ObstacleGuid { get; internal set; }
        public string ObstacleName { get; internal set; }
    }
}
