using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartTypeA.Utils
{
    class RectancleCreater
    {
        public static IGeometry CreateRectancle(IPoint refPoint, double direction, double length, double width)
        {

            var pt1 = EsriFunctions.LocalToPrj(refPoint, direction, 0, width / 2);
            var pt2 = EsriFunctions.LocalToPrj(refPoint, direction, 0, -width / 2);
            var pt3 = EsriFunctions.LocalToPrj(refPoint, direction, length, -width / 2);
            var pt4 = EsriFunctions.LocalToPrj(refPoint, direction, length, width / 2);

            var ptColl = new Ring() as IPointCollection;
            ptColl.AddPoint(pt1);
            ptColl.AddPoint(pt2);
            ptColl.AddPoint(pt3);
            ptColl.AddPoint(pt4);
            var poly = new Polygon() as IGeometryCollection;
            poly.AddGeometry((IGeometry)ptColl);
            EsriFunctions.SimplifyGeometry((IGeometry)poly);
            return GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry)poly);
        }

        public static IGeometry CreateRectancle(IPoint startPt, IPoint endPoint, double direction, double width)
        {

            var pt1 = EsriFunctions.LocalToPrj(startPt, direction, 0, width / 2);
            var pt2 = EsriFunctions.LocalToPrj(startPt, direction, 0, -width / 2);
            var pt3 = EsriFunctions.LocalToPrj(endPoint, direction, 0, -width / 2);
            var pt4 = EsriFunctions.LocalToPrj(endPoint, direction, 0, width / 2);

            var ptColl = new Ring() as IPointCollection;
            ptColl.AddPoint(pt1);
            ptColl.AddPoint(pt2);
            ptColl.AddPoint(pt3);
            ptColl.AddPoint(pt4);
            var poly = new Polygon() as IGeometryCollection;
            poly.AddGeometry((IGeometry)ptColl);
            EsriFunctions.SimplifyGeometry((IGeometry)poly);
            return GlobalParams.SpatialRefOperation.ToEsriGeo((IGeometry)poly);
        }
    }
}
