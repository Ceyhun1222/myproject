using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aran.Geometries;
using ESRI.ArcGIS.Display;

namespace Aran.Delta.Model
{
    public class IntersectionItem
    {
        private int _geoHandle;
        private ISymbol _segmentSymbol;
        public IntersectionItem()
        {
            CreateSymbol();
        }

        public string Header { get; set; }

        public Aran.Geometries.Geometry Geo { get; set; }

        public Enums.FeatureType FeatureType { get; set; }

        public Aran.Aim.Features.Feature Feat { get; set; }

        public void Draw()
        {
            Clear();
            if (Geo == null) return;
            if (Geo.Type== GeometryType.MultiLineString)
                _geoHandle = GlobalParams.UI.DrawMultiLineStringPrj((MultiLineString)Geo, _segmentSymbol);
            else if (Geo.Type== GeometryType.MultiPolygon)
                _geoHandle = GlobalParams.UI.DrawDefaultMultiPolygon((MultiPolygon)Geo);
        }

        public void Clear()
        {
           GlobalParams.UI.SafeDeleteGraphic(_geoHandle);
        }

        private void CreateSymbol()
        {
            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = Aran.PANDA.Common.ARANFunctions.RGB(100, 200, 140);
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = esriSimpleLineStyle.esriSLSDash;
            pLineSym.Width = 3;
            _segmentSymbol = pLineSym as ISymbol;

        }
    }
}
