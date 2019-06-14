using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
    public class LineElementCreater
    {
        private int _color;
        public LineElementCreater(int color)
        {
            _color = color;
        }
        public IElement CreateLineElement(params IPoint[] points)
        {
            List<IPoint> list = new List<IPoint>();
            foreach (var item in points)
                list.Add(item);
            return GlobalParams.UI.GetPolylineElement(list, _color);
        }
    }
}
