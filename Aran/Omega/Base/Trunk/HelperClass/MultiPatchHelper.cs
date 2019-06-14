using Aran.Geometries;
using Aran.Omega.Models;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.HelperClass
{
    class MultiPatchHelper
    {
        public static IMultiPatch GeometryToMultipatch(MultiPolygon mltPolygon)
        {
            object _missing = Type.Missing;

            MultiPolygon surfaceGeo = GlobalParams.SpatialRefOperation.ToGeo(mltPolygon);

            IGeometryCollection multiPatchGeometryCollection = new MultiPatchClass();
            IMultiPatch multiPatch = multiPatchGeometryCollection as IMultiPatch;

            if (surfaceGeo == null || surfaceGeo.IsEmpty)
                return multiPatch;

            //var surface = this as IMultiplePlane;
            if (mltPolygon is IMultiplePlane surface)
            {
                foreach (var plane in surface.Planes)
                {
                    var planeGeo = Converters.ConvertToEsriGeom.FromGeometry(plane.Geo, true);
                    multiPatchGeometryCollection.AddGeometry(planeGeo);
                }
            }
            else
            {
                foreach (Aran.Geometries.Polygon polygon in surfaceGeo)
                {
                    var extRing = Converters.ConvertToEsriGeom.FromRing(polygon.ExteriorRing);
                    multiPatchGeometryCollection.AddGeometry(extRing as IGeometry, ref _missing, ref _missing);

                    multiPatch.PutRingType(extRing, esriMultiPatchRingType.esriMultiPatchOuterRing);

                    foreach (var aranInteriorRing in polygon.InteriorRingList)
                    {
                        var esriInteriorRing = Converters.ConvertToEsriGeom.FromRing(aranInteriorRing);
                        multiPatchGeometryCollection.AddGeometry(esriInteriorRing as IGeometry, ref _missing, ref _missing);

                        multiPatch.PutRingType(esriInteriorRing, esriMultiPatchRingType.esriMultiPatchInnerRing);
                    }
                }
            }

            return multiPatch;

        }
    }
}
