using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Aran.Geometries;
using GeoAPI.Geometries;

namespace Aran.Omega.Topology
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

        public Geometry Boundary(Geometry geom)
        {
            IGeometry esriGeom = ConvertToMyGeo.FromGeometry(geom);
            var boundary =  esriGeom.Boundary;
            Geometry result = ConvertToAranGeo.ToGeometry(boundary);
            return result;  
        }

        public Geometry Intersect(Aran.Geometries.Geometry geom1, Aran.Geometries.Geometry geom2)
        {
            IGeometry myGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry myGeom2 = ConvertToMyGeo.FromGeometry(geom2);

            IGeometry geom = myGeom1.Intersection(myGeom2);

            var aranIntersectGeom = ConvertToAranGeo.ToGeometry(geom);

            if (geom.OgcGeometryType == OgcGeometryType.Polygon && (geom1.Type == GeometryType.MultiPolygon || geom2.Type == GeometryType.MultiPolygon))
                return new Aran.Geometries.MultiPolygon { aranIntersectGeom as Aran.Geometries.Polygon };

            if (geom.OgcGeometryType == OgcGeometryType.LineString && (geom1.Type == GeometryType.MultiLineString || geom2.Type == GeometryType.MultiLineString))
                return new Aran.Geometries.MultiLineString { aranIntersectGeom as Aran.Geometries.LineString };

            return aranIntersectGeom;
        }

        public Geometry Intersect(Aran.Geometries.Geometry geom)
        {
            IGeometry myGeom = ConvertToMyGeo.FromGeometry(geom);

            var intersectGeom = _convertedGeometry.Intersection(myGeom);
            var aranIntersectGeom = ConvertToAranGeo.ToGeometry(intersectGeom);

            if (intersectGeom.OgcGeometryType == OgcGeometryType.Polygon && (_currentGeometry.Type == GeometryType.MultiPolygon || geom.Type == GeometryType.MultiPolygon))
                return new Aran.Geometries.MultiPolygon { aranIntersectGeom as Aran.Geometries.Polygon };

            if (intersectGeom.OgcGeometryType == OgcGeometryType.LineString && (_currentGeometry.Type == GeometryType.MultiLineString || geom.Type == GeometryType.MultiLineString))
                return new Aran.Geometries.MultiLineString { aranIntersectGeom as Aran.Geometries.LineString };

            return aranIntersectGeom;
        }

        public Geometry UnionGeometry(Aran.Geometries.Geometry geom1, Aran.Geometries.Geometry geom2)
        {
            IGeometry myGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry myGeom2 = ConvertToMyGeo.FromGeometry(geom2);
            //IGeometry intersectGeom = null;

            IGeometry geom = myGeom1.Union(myGeom2);
            return ConvertToAranGeo.ToGeometry(geom);
        }

        public Geometry UnionGeometry(Aran.Geometries.Geometry geom1)
        {
            IGeometry myGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            //IGeometry intersectGeom = null;

            IGeometry geom = _convertedGeometry.Union(myGeom1);
            return ConvertToAranGeo.ToGeometry(geom);
        }

        public double GetDistance(Geometry geom, Geometry other)
        {
            IGeometry geom1 = ConvertToMyGeo.FromGeometry(geom);
            IGeometry geom2 = ConvertToMyGeo.FromGeometry(other);
            double distance =geom1.Distance(geom2);
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
            return result;
        }

		public Geometry Buffer(Geometry geom, double width, GeoAPI.Operation.Buffer.EndCapStyle bufferStyle = GeoAPI.Operation.Buffer.EndCapStyle.Round)
		{
			IGeometry myGeom = ConvertToMyGeo.FromGeometry(geom);
			IGeometry bufferGeom = myGeom.Buffer(width, 360, bufferStyle);
			Geometry result = ConvertToAranGeo.ToGeometry(bufferGeom);
			if (result.Type == GeometryType.Polygon)
				return new MultiPolygon { result as Polygon };
			return result;
		}

        public Geometry Difference(Geometry geom1, Geometry geom2)
        {
            IGeometry topoGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry topoGeom2 = ConvertToMyGeo.FromGeometry(geom2);
            IGeometry difference = topoGeom1.Difference(topoGeom2);
            Geometry result = ConvertToAranGeo.ToGeometry(difference);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Geometry Difference(Geometry geom1)
        {
            IGeometry topoGeom1 = ConvertToMyGeo.FromGeometry(geom1);
            IGeometry difference = _convertedGeometry.Difference(topoGeom1);
            Geometry result = ConvertToAranGeo.ToGeometry(difference);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Boolean Disjoint(Geometry geom, Geometry other)
        {
            try
            {
                IGeometry topoGeo1 = ConvertToMyGeo.FromGeometry(geom);
                IGeometry topoGeo2 = ConvertToMyGeo.FromGeometry(other);
                return topoGeo1.Disjoint(topoGeo2);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message+"Disjoin error");
                return true;
            }
            
        }

        public Boolean Disjoint(Geometry other)
        {
            IGeometry topoGeo2 = ConvertToMyGeo.FromGeometry(other);
            return _convertedGeometry.Disjoint(topoGeo2);
        }

        public bool Crosses(Geometry geom, Geometry other)
        {
            IGeometry geom1 = ConvertToMyGeo.FromGeometry(geom);
            IGeometry geom2 = ConvertToMyGeo.FromGeometry(other);
            return geom1.Crosses(geom2);
        }

        public bool Crosses(Geometry other)
        {
            IGeometry geom2 = ConvertToMyGeo.FromGeometry(other);
            return _convertedGeometry.Crosses(geom2);
        }

        #endregion
        private Geometry _currentGeometry;
        private IGeometry _convertedGeometry;
    }

}
