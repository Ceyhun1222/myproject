using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace SigmaChart
{
    public class AirspaceBuffer
    {
        private IHookHelper _hookHelper;
        private double _mapScale;
        private SpatialReferenceOperation _spOperation;
        public AirspaceBuffer(IHookHelper hookHelper)
        {
            _hookHelper = hookHelper;
             IPageLayout pageLayout = hookHelper.PageLayout;
              IActiveView activeView = (IActiveView)pageLayout;
            _mapScale = activeView.FocusMap.MapScale;

            var map =_hookHelper.FocusMap;
            var graphicsContainer =GlobalParams.HookHelper.PageLayout as IGraphicsContainer;
            if (graphicsContainer != null)
            {
                var frameElement = graphicsContainer.FindFrame(map);
                var mapFrame = frameElement as IMapFrame;
                IEnvelope env = mapFrame.MapBounds;
                IArea area = env as IArea;
                if (area != null && !((IGeometry)area).IsEmpty)
                {
                    var centroid = area.Centroid;
                    if (centroid !=null) _spOperation = new SpatialReferenceOperation(centroid.X);
                }
            }
            else
            {
                _spOperation = new SpatialReferenceOperation();
            }

        }

        public Bagel Buffer(IPolygon poly, double lengthInMm, bool changeProjection = true)
        {
            if (poly == null || poly.IsEmpty) return null;

            if (_spOperation == null) _spOperation = new SpatialReferenceOperation();
            Bagel res = new Bagel();

            if (changeProjection)
            {
                var area = poly as IArea;
                if (area != null)
                {
                    var centerPoint = area.Centroid;
                    _spOperation.ChangeCentralMeridian(centerPoint.X);
                }

            }
            var bufferWidth = lengthInMm*_mapScale/1000;
            
            IPolygon polyPrj = (IPolygon)_spOperation.ToEsriPrj(poly);

            var interiorBuffer = GeomOperators.Buffer((IPolygon)polyPrj, -bufferWidth);
            if (interiorBuffer.IsEmpty)
            {
                bufferWidth = (lengthInMm-1)*_mapScale/1000;
                interiorBuffer = GeomOperators.Buffer((IPolygon) polyPrj, -bufferWidth);
            }
          
            if (interiorBuffer != null && !interiorBuffer.IsEmpty)
            {
                var differ = GeomOperators.Difference((IPolygon) polyPrj, interiorBuffer);

                if (differ != null && !differ.IsEmpty)
                {
                    res.BagelPoly = _spOperation.ToEsriGeo(differ);
                    res.BagelCodeId = "";
                    res.MasterID = "";
                    return res;
                     
                }
            }

            res.BagelPoly = poly;
            res.BagelCodeId = "";
            res.MasterID = "";
            return res;
        }
    }

    public class Bagel
    {
        public string BagelCodeId { get; set; }
         public string BagelTxtName { get; set; }
         public string BagelCodeClass { get; set; }
         public string BagelLocalType { get; set; }

        public IPolygon BagelPoly { get; set; }
        public string MasterID { get; set; }
        public Bagel()
        {
        }
    }
}
