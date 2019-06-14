using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.Models
{
    public class GeomOperators
    {
        public static IPolygon UnionPolygon(IPolygon geom1, IPolygon geom2)
        {
            try
            {
                var topOperator2 = geom1 as ITopologicalOperator2;
                if (topOperator2 != null)
                {
                    IGeometry unioGeometry = topOperator2.Union(geom2);
                    SimplifyGeometry(unioGeometry);
                    return unioGeometry as IPolygon;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;

            }
        }

        public static IPolygon Intersect(IPolygon geom1, IPolygon geom2)
        {
            try
            {
                SimplifyGeometry(geom1);
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IPolygon intersectGeometry =(IPolygon) topoOperator2.Intersect(geom2,
                        esriGeometryDimension.esriGeometry2Dimension);
                    SimplifyGeometry(intersectGeometry);
                    
                    return intersectGeometry;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;
            }
        }

        public static IGeometry Cut(IPolygon geom1, IPolyline geom2,bool isLeft)
        {
            try
            {
                SimplifyGeometry(geom1);
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IGeometry leftGeom, rightGeom;
                    topoOperator2.Cut(geom2, out leftGeom, out rightGeom);
                    if (isLeft)
                        return leftGeom;
                    else 
                        return rightGeom;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;
            }
        }

        public static IPolygon Difference(IPolygon geom1, IPolygon geom2)
        {
            try
            {
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IGeometry differenceGeometry = topoOperator2.Difference(geom2);
                    if (differenceGeometry == null || differenceGeometry.IsEmpty)
                        return null;

                    SimplifyGeometry(differenceGeometry);
                    return differenceGeometry as IPolygon;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IPolygon Buffer(IPolygon geom1, double distance)
        {
            try
            {
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IGeometry bufferGeometry = topoOperator2.Buffer(distance);
                    SimplifyGeometry(bufferGeometry);
                    var poly = bufferGeometry as IPolygon;
                    if (poly != null)
                    {
                        poly.Generalize(0.1);
                        SimplifyGeometry(poly);
                    }
                    return bufferGeometry as IPolygon;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;
            }
        }

        public static Boolean Disjoint(IGeometry geom1,IGeometry other)
        {
            IRelationalOperator relOper = (IRelationalOperator)geom1;
            return relOper.Disjoint(other);
        }

        public static void SimplifyGeometry(IGeometry geom)
        {
            var topOper2 = geom as ITopologicalOperator2;
            if (topOper2 != null)
            {
                topOper2.IsKnownSimple_2 = false;
                topOper2.Simplify();
            }
        }
    }
}
