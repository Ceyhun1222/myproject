using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    class ArcModel
    {
        private int _geoHandle;

        public ESRI.ArcGIS.Geometry.IPoint ArcPt1 { get; set; }
        public ESRI.ArcGIS.Geometry.IPoint ArcPt2 { get; set; }
        public bool IsCw { get; set; }

        public ESRI.ArcGIS.Geometry.IPolygon ArcGeometry { get; set; }
        public double Radius { get; set; }

        public void Draw()
        {
            DeleteGraphics();
            if (ArcGeometry != null && !ArcGeometry.IsEmpty)
               _geoHandle = GlobalParams.UI.DrawEsriDefaultMultiPolygon(ArcGeometry);
        }

        public void DeleteGraphics()
        {
            GlobalParams.UI.SafeDeleteGraphic(_geoHandle);
        }
    }
}
