using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Geometries;
using GeoAPI.Geometries;

namespace Aran.Omega.TypeBEsri.Topology
{
    public class GeometryOperators
    {
        public GeometryOperators()
        {
        }

        public Geometry CurrentGeometry
        {
            get
            {
                return _currentGeometry;
            }
            set
            {
                _currentGeometry = value;
                _convertedGeometry = ConvertToMyGeo.FromGeometry(value);
            }
        }

        #region Topological Operators

        public Geometry Intersect(Aran.Geometries.Geometry geom1, Aran.Geometries.Geometry geom2)
        {
            IGeometry myGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry myGeom2 = ConvertToMyGeo.FromGeometry(geom2);
            IGeometry intersectGeom = null;

            IGeometry geom = myGeom1.Intersection(myGeom2);
            return ConvertToAranGeo.ToGeometry(geom);
        }

        public double GetDistance(Geometry geom, Geometry other)
        {
            IGeometry esriGeom1 = ConvertToMyGeo.FromGeometry(geom);
            IGeometry esriGeom2 = ConvertToMyGeo.FromGeometry(other);
            double distance =esriGeom1.Distance(esriGeom2);
            return distance;
        }
        
        public double GetDistance(Geometry other)
        {
            IGeometry myGeom2 = ConvertToMyGeo.FromGeometry(other);
            double distance = _convertedGeometry.Distance(myGeom2);
            return distance;
        }

        public Geometry ConvexHull(Geometry geom)
        {
            IGeometry myGeom = ConvertToMyGeo.FromGeometry(geom);
            IGeometry convexGeom = myGeom.ConvexHull();
            Geometry result = ConvertToAranGeo.ToGeometry(convexGeom);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            if (result.Type == GeometryType.LineString)
                return new MultiLineString { result as LineString};
            return result;
        }

        public Geometry Buffer(Geometry geom, double width)
        {
            IGeometry myGeom = ConvertToMyGeo.FromGeometry(geom);
            IGeometry bufferGeom = myGeom.Buffer(width,90);
            Geometry result = ConvertToAranGeo.ToGeometry(bufferGeom);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Geometry Difference(Geometry geom1, Geometry geom2)
        {
            IGeometry esriGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry esriGeom2 = ConvertToMyGeo.FromGeometry(geom2);
            IGeometry difference = esriGeom1.Difference(esriGeom2);
            Geometry result = ConvertToAranGeo.ToGeometry(difference);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Boolean Disjoint(Geometry geom, Geometry other)
        {
            IGeometry esriGeom1 = ConvertToMyGeo.FromGeometry(geom);
            IGeometry esriGeom2 = ConvertToMyGeo.FromGeometry(other);
            return esriGeom1.Disjoint(esriGeom2);
        }

        #endregion
        private Geometry _currentGeometry;
        private IGeometry _convertedGeometry;
    }

}
