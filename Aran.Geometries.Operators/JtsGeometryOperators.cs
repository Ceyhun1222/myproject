using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Converters.ConverterJtsGeom;
using GeoAPI.Geometries;

namespace Aran.Geometries.Operators
{
    public  class JtsGeometryOperators : IGeometryOperators
    {
        public JtsGeometryOperators()
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
                _convertedGeometry = ConvertToJtsGeo.FromGeometry(value);
                if (!_convertedGeometry.IsValid)
                   _convertedGeometry = SimplifyGeometry(_convertedGeometry);
            }
        }

        #region Topological Operators

        public Geometry Boundary(Geometry geom)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(geom);
          //  if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");

            var boundary = jtsGeom.Boundary;
            Geometry result = ConvertFromJtsGeo.ToGeometry(boundary);
            return result;
        }

        public Geometry Intersect(Aran.Geometries.Geometry geom1, Aran.Geometries.Geometry geom2)
        {
            IGeometry myGeom1 = ConvertToJtsGeo.FromGeometry(geom1);
            IGeometry myGeom2 = ConvertToJtsGeo.FromGeometry(geom2);

         //   if (!(myGeom1.IsValid && myGeom2.IsValid)) throw new Exception("Geometry is not valid!");

            IGeometry geom = myGeom1.Intersection(myGeom2);

            var aranIntersectGeom = ConvertFromJtsGeo.ToGeometry(geom);

            if (geom.OgcGeometryType == OgcGeometryType.Polygon && (geom1.Type == GeometryType.MultiPolygon || geom2.Type == GeometryType.MultiPolygon))
                return new Aran.Geometries.MultiPolygon { aranIntersectGeom as Aran.Geometries.Polygon };

            if (geom.OgcGeometryType == OgcGeometryType.LineString && (geom1.Type == GeometryType.MultiLineString || geom2.Type == GeometryType.MultiLineString))
                return new Aran.Geometries.MultiLineString { aranIntersectGeom as Aran.Geometries.LineString };

            return aranIntersectGeom;
        }

        public Geometry Intersect(Aran.Geometries.Geometry geom)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(geom);

          //  if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            var intersectGeom = _convertedGeometry.Intersection(jtsGeom);
            var aranIntersectGeom = ConvertFromJtsGeo.ToGeometry(intersectGeom);

            if (intersectGeom.OgcGeometryType == OgcGeometryType.Polygon && (_currentGeometry.Type == GeometryType.MultiPolygon || geom.Type == GeometryType.MultiPolygon))
                return new Aran.Geometries.MultiPolygon { aranIntersectGeom as Aran.Geometries.Polygon };

            if (intersectGeom.OgcGeometryType == OgcGeometryType.LineString && (_currentGeometry.Type == GeometryType.MultiLineString || geom.Type == GeometryType.MultiLineString))
                return new Aran.Geometries.MultiLineString { aranIntersectGeom as Aran.Geometries.LineString };

            return aranIntersectGeom;
        }

        public Boolean Contains(Geometry geom, Geometry other)
        {
            IGeometry geom1 = ConvertToJtsGeo.FromGeometry(geom);
            IGeometry geom2 = ConvertToJtsGeo.FromGeometry(other);

          //  if (!(geom1.IsValid && geom2.IsValid)) throw new Exception("Geometry is not valid!");
            return geom1.Contains(geom2);
        }

        public Boolean Contains(Geometry other)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(other);
          //  if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            return _convertedGeometry.Contains(jtsGeom);
        }

        public Geometry UnionGeometry(Aran.Geometries.Geometry geom1, Aran.Geometries.Geometry geom2)
        {
            IGeometry myGeom1 = ConvertToJtsGeo.FromGeometry(geom1);
            IGeometry myGeom2 = ConvertToJtsGeo.FromGeometry(geom2);
            //IGeometry intersectGeom = null;

//            if (!(myGeom1.IsValid && myGeom2.IsValid)) throw new Exception("Geometry is not valid!");

            var geom = myGeom1.Union(myGeom2);
            if (geom != null)
            {
                var result = ConvertFromJtsGeo.ToGeometry(geom);
                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon { result as Polygon };
                return result;
            }

            return null;
        }

        public Geometry UnionGeometry(Aran.Geometries.Geometry geom1)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(geom1);
            //IGeometry intersectGeom = null;
          //  if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            IGeometry geom = _convertedGeometry.Union(jtsGeom);
            return ConvertFromJtsGeo.ToGeometry(geom);
        }

        public double GetDistance(Geometry geom, Geometry other)
        {
            IGeometry geom1 = ConvertToJtsGeo.FromGeometry(geom);
            IGeometry geom2 = ConvertToJtsGeo.FromGeometry(other);

          //  if (!(geom1.IsValid && geom2.IsValid)) throw new Exception("Geometry is not valid!");
            double distance = geom1.Distance(geom2);
            return distance;
        }

        public double GetDistance(Geometry other)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(other);
        //    if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            double distance = _convertedGeometry.Distance(jtsGeom);
            return distance;
        }

        public Geometry ConvexHull(Geometry geom)
        {
            IGeometry myGeom = ConvertToJtsGeo.FromGeometry(geom);
            IGeometry convexGeom = myGeom.ConvexHull();
            Geometry result = ConvertFromJtsGeo.ToGeometry(convexGeom);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Geometry BufferWithCapStyle(Geometry geom, double width, GeoAPI.Operation.Buffer.EndCapStyle bufferStyle = GeoAPI.Operation.Buffer.EndCapStyle.Round)
        {
            IGeometry myGeom = ConvertToJtsGeo.FromGeometry(geom);
            if (myGeom != null && !myGeom.IsEmpty)
            {
                IGeometry bufferGeom = myGeom.Buffer(width, 360, bufferStyle);
                Geometry result = ConvertFromJtsGeo.ToGeometry(bufferGeom);
                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon {result as Polygon};
                return result;
            }
            return null;
        }

        public Geometry Buffer(Geometry geom, double width)
        {
            IGeometry myGeom = ConvertToJtsGeo.FromGeometry(geom);
            if (myGeom != null && !myGeom.IsEmpty)
            {
                IGeometry bufferGeom = myGeom.Buffer(width,90);
                Geometry result = ConvertFromJtsGeo.ToGeometry(bufferGeom);
                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon { result as Polygon };
                return result;
            }
            return null;
        }

        public Geometry Buffer(Geometry geom, double width,int quadrantSegments)
        {
            IGeometry myGeom = ConvertToJtsGeo.FromGeometry(geom);
            if (myGeom != null && !myGeom.IsEmpty)
            {
                IGeometry bufferGeom = myGeom.Buffer(width, quadrantSegments);
                Geometry result = ConvertFromJtsGeo.ToGeometry(bufferGeom);
                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon { result as Polygon };
                return result;
            }

            return null;
        }

        public Geometry Difference(Geometry geom1, Geometry geom2)
        {
            IGeometry topoGeom1 = ConvertToJtsGeo.FromGeometry(geom1);
            IGeometry topoGeom2 = ConvertToJtsGeo.FromGeometry(geom2);

            ///if (!(topoGeom1.IsValid && topoGeom2.IsValid)) throw new Exception("Geometry is not valid!");

            try
            {
                IGeometry difference = topoGeom1.Difference(topoGeom2);
                Geometry result = ConvertFromJtsGeo.ToGeometry(difference);

                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon {result as Polygon};
                return result;
            }
            catch (Exception)
            {
                var buffer1 = topoGeom1.Buffer(0.0001);
                var buffer2 = topoGeom2.Buffer(0.0001);

                IGeometry difference = buffer1.Difference(buffer2);
                Geometry result = ConvertFromJtsGeo.ToGeometry(difference);

                if (result.Type == GeometryType.Polygon)
                    return new MultiPolygon { result as Polygon };
                return result;
            }
        }

        public Geometry Difference(Geometry geom1)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(geom1);

         //   if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");

            IGeometry difference = _convertedGeometry.Difference(jtsGeom);
            Geometry result = ConvertFromJtsGeo.ToGeometry(difference);
            if (result.Type == GeometryType.Polygon)
                return new MultiPolygon { result as Polygon };
            return result;
        }

        public Boolean Disjoint(Geometry geom, Geometry other)
        {
            IGeometry topoGeo1 = ConvertToJtsGeo.FromGeometry(geom);
            IGeometry topoGeo2 = ConvertToJtsGeo.FromGeometry(other);
            return topoGeo1.Disjoint(topoGeo2);
        }

        public Boolean Disjoint(Geometry other)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(other);
          //  if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            return _convertedGeometry.Disjoint(jtsGeom);
        }

        public bool Crosses(Geometry geom, Geometry other)
        {
            IGeometry geom1 = ConvertToJtsGeo.FromGeometry(geom);
            IGeometry geom2 = ConvertToJtsGeo.FromGeometry(other);

         //   if (!(geom1.IsValid && geom2.IsValid)) throw new Exception("Geometry is not valid!");
            return geom1.Crosses(geom2);
        }

        public bool Crosses(Geometry other)
        {
            IGeometry jtsGeom = ConvertToJtsGeo.FromGeometry(other);
            if (!jtsGeom.IsValid) throw new Exception("Geometry is not valid!");
            return _convertedGeometry.Crosses(jtsGeom);
        }

        public Geometry SimplifyGeometry(Geometry geom,double tolerance=0.001)
        {
            IGeometry jtsGeo = ConvertToJtsGeo.FromGeometry(geom);

            jtsGeo = SimplifyGeometry(jtsGeo, tolerance);// NetTopologySuite.Simplify.DouglasPeuckerSimplifier.Simplify(jtsGeo, tolerance);
            Geometry result = null;
            if (jtsGeo == null) return null;

            if (jtsGeo.OgcGeometryType == OgcGeometryType.Polygon)
                result = new Aran.Geometries.MultiPolygon {ConvertFromJtsGeo.ToGeometry(jtsGeo) as Aran.Geometries.Polygon};
            else
                result = ConvertFromJtsGeo.ToGeometry(jtsGeo);
            return result;
        }

        private IGeometry SimplifyGeometry(IGeometry geom, double tolerance = 0.001)
        {
            if (geom!=null)
                return NetTopologySuite.Simplify.DouglasPeuckerSimplifier.Simplify(geom, tolerance);
            return null;
        }


        #endregion
        private Geometry _currentGeometry;
        private IGeometry _convertedGeometry;
    }
}
