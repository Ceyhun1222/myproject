using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Geometries;
using ESRI.ArcGIS.Geodatabase;

namespace Aran.Omega.Export
{
    static class ExportHelper
    {
        public static IFeature CreateFeature(GeometryType geoType, IFeatureClass ptFeatClass,
            IFeatureClass polyLineFeatClass, IFeatureClass polygonFeatureClass)
        {
            IFeature feat = null;
            if (geoType == GeometryType.Point)
                feat = ptFeatClass.CreateFeature();
            else if (geoType == GeometryType.MultiLineString || geoType == GeometryType.LineString)
                feat = polyLineFeatClass.CreateFeature();
            else if (geoType == GeometryType.MultiPolygon || geoType == GeometryType.Polygon)
                feat = polygonFeatureClass.CreateFeature();
            return feat;
        }

        public static IFeature CreateFeature(VerticalStructurePartGeometryChoice geometryChoice, IFeatureClass ptFeatClass,
            IFeatureClass polyLineFeatClass, IFeatureClass polygonFeatureClass)
        {
            IFeature feat = null;
            if (geometryChoice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                feat = ptFeatClass.CreateFeature();
            else if (geometryChoice== VerticalStructurePartGeometryChoice.ElevatedCurve)
                feat = polyLineFeatClass.CreateFeature();
            else if (geometryChoice == VerticalStructurePartGeometryChoice.ElevatedSurface)
                feat = polygonFeatureClass.CreateFeature();
            return feat;
        }

    }
}
