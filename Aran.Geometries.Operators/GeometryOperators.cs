using System;
using System.Collections.Generic;
using System.Text;
using Aran;
using ESRI.ArcGIS.Geometry;
using Aran.Geometries;
using Aran.Converters;
using Aran.Geometries.SpatialReferences;
using GeoAPI.Operation.Buffer;

namespace Aran.Geometries.Operators
{
	public class GeometryOperators:IGeometryOperators
	{
		public GeometryOperators()
		{
		}

        private static GeometryOperators _instance;
        public static GeometryOperators Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GeometryOperators();
                return _instance;
            }
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
				_convertedGeometry = ConvertToEsriGeom.FromGeometry(value);

				//_convertedRelOper = _convertedGeometry as IRelationalOperator;
				_convertedRelOper = (IRelationalOperator)_convertedGeometry;

				//_convertedProxiOperator = _convertedGeometry as IProximityOperator;
				_convertedProxiOperator = (IProximityOperator)_convertedGeometry;
			}
		}

		#region Topological Operators
		public Geometry ConvexHull(Geometry geom)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
			IGeometry convexGeom = topoOper2.ConvexHull();
			SimplifyGeometry(convexGeom);
			Geometry result = ConvertFromEsriGeom.ToGeometry(convexGeom);
			return result;
		}

		public Geometry Boundary(Geometry geom)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
			IGeometry boundGeom = topoOper2.Boundary;
			SimplifyGeometry(boundGeom);
			Geometry result = ConvertFromEsriGeom.ToGeometry(boundGeom);
			return result;
		}

		public Geometry Buffer(Geometry geom, double width)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry bufferGeom;

			if (esriGeom.GeometryType != esriGeometryType.esriGeometryPoint)
			{
				ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
				SimplifyGeometry(esriGeom);
				bufferGeom = topoOper2.Buffer(width);
			}
			else
			{
				ITopologicalOperator topoOper = (ITopologicalOperator)esriGeom;
				bufferGeom = topoOper.Buffer(width);
			}

			SimplifyGeometry(bufferGeom);
			Geometry result = ConvertFromEsriGeom.ToGeometry(bufferGeom);
			return result;
		}

		public Geometry UnionGeometry(Geometry geom1, Geometry geom2)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(geom2);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
			if (topoOper2 != null)
			{
				IGeometry unionGeom = topoOper2.Union(esriGeom2);
				SimplifyGeometry(unionGeom);
				Geometry result = ConvertFromEsriGeom.ToGeometry(unionGeom);
				return result;
			}
			else
			{
				return null;
			}
		}

		public Geometry Intersect(Geometry geom1, Geometry geom2)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(geom2);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
			IGeometry intersectGeom = null;

			if (topoOper2 != null)
			{
				esriGeometryType geomType1 = esriGeom1.GeometryType;
				esriGeometryType geomType2 = esriGeom2.GeometryType;
				switch (geomType1)
				{
					case esriGeometryType.esriGeometryPolyline:
						if (geomType2 == esriGeometryType.esriGeometryPolygon)
							intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
						else
							intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
						break;
					case esriGeometryType.esriGeometryPolygon:
						if (geomType2 == esriGeometryType.esriGeometryPolyline)
							intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
						else if (geomType2 == esriGeometryType.esriGeometryPolygon)
							intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry2Dimension);
						else
							intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
						break;
					default:
						intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
						break;
				}

				if (intersectGeom.GeometryType != esriGeometryType.esriGeometryPoint)
					SimplifyGeometry(intersectGeom);
				Geometry result = ConvertFromEsriGeom.ToGeometry(intersectGeom);
				return result;
			}
			else
			{
				return null;
			}
		}

        public Geometry Intersect(Geometry geom1)
        {
            IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
            IGeometry esriGeom2 = _convertedGeometry;
            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
            IGeometry intersectGeom = null;

            if (topoOper2 != null)
            {
                esriGeometryType geomType1 = esriGeom1.GeometryType;
                esriGeometryType geomType2 = esriGeom2.GeometryType;
                switch (geomType1)
                {
                    case esriGeometryType.esriGeometryPolyline:
                        if (geomType2 == esriGeometryType.esriGeometryPolygon)
                            intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
                        else
                            intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        if (geomType2 == esriGeometryType.esriGeometryPolyline)
                            intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry1Dimension);
                        else if (geomType2 == esriGeometryType.esriGeometryPolygon)
                            intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry2Dimension);
                        else
                            intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
                        break;
                    default:
                        intersectGeom = topoOper2.Intersect(esriGeom2, esriGeometryDimension.esriGeometry0Dimension);
                        break;
                }

                if (intersectGeom.GeometryType != esriGeometryType.esriGeometryPoint)
                    SimplifyGeometry(intersectGeom);
                Geometry result = ConvertFromEsriGeom.ToGeometry(intersectGeom);
                return result;
            }
            else
            {
                return null;
            }
        }

		public Geometry Difference(Geometry geom1, Geometry geom2)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(geom2);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
			if (topoOper2 != null)
			{
				IGeometry differenceGeom = topoOper2.Difference(esriGeom2);
				if (differenceGeom.IsEmpty)
					return null;

				SimplifyGeometry(differenceGeom);
				Geometry result = ConvertFromEsriGeom.ToGeometry(differenceGeom);
				return result;
			}
			else
			{
				return null;
			}
		}

		public void Cut(Geometry geometry, MultiLineString cutter, out Geometry geomLeft, out Geometry geomRight)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geometry);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
			IPolyline esriCutterGeom = ConvertToEsriGeom.FromMultiLineString(cutter);
			SimplifyGeometry(esriGeom);
			IGeometry cutterLeft, cutterRight;
			topoOper2.Cut(esriCutterGeom, out cutterLeft, out cutterRight);
			SimplifyGeometry(cutterLeft);
			SimplifyGeometry(cutterRight);
			geomLeft = ConvertFromEsriGeom.ToGeometry(cutterLeft);
			geomRight = ConvertFromEsriGeom.ToGeometry(cutterRight);
		}

		public void Cut(Geometry geometry, LineString cutter, out Geometry geomLeft, out Geometry geomRight)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geometry);
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
			IPolyline esriCutterGeom = ConvertToEsriGeom.FromLineString(cutter);

			//SimplifyGeometry(esriGeom);
			IGeometry cutterLeft, cutterRight;
			topoOper2.Cut(esriCutterGeom, out cutterLeft, out cutterRight);
			SimplifyGeometry(cutterLeft);
			SimplifyGeometry(cutterRight);
			geomLeft = ConvertFromEsriGeom.ToGeometry(cutterLeft);
			geomRight = ConvertFromEsriGeom.ToGeometry(cutterRight);
		}

		/// <summary>
		/// Rotates about the specified origin point. The angle is in radians. The origin can be in a different spatial reference than the geometry being rotated.
		/// </summary>
		/// <param name="geometry">Geometry to be rotated</param>
		/// <param name="point">Rotation origin point</param>
		/// <param name="angleInRad">Angle of rotation in radians</param>
		/// <returns>Rotated copy of the initial geometry</returns>
		public Geometry Rotate(Geometry geometry, Point point, double angleInRad)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geometry);
			IPoint esriPoint = ConvertToEsriGeom.FromPoint(point);
			ITransform2D pTransform = (ITransform2D)esriGeom;
			pTransform.Rotate(esriPoint, angleInRad);
			return ConvertFromEsriGeom.ToGeometry(esriGeom);
		}
		#endregion

		#region Proximity Operators
		public Aran.Geometries.Point GetNearestPoint(Geometry geom, Aran.Geometries.Point point)
		{
			IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
			IPoint esriPoint = ConvertToEsriGeom.FromPoint(point);
			IProximityOperator proximtyOperator = esriGeom as IProximityOperator;
			IPoint nearestPoint = new ESRI.ArcGIS.Geometry.Point();
			proximtyOperator.QueryNearestPoint(esriPoint, esriSegmentExtension.esriNoExtension, nearestPoint);
			Aran.Geometries.Point result = ConvertFromEsriGeom.ToPoint(nearestPoint);
			return result;
		}

		public Aran.Geometries.Point GetNearestPoint(Aran.Geometries.Point point)
		{
			IPoint esriPoint = ConvertToEsriGeom.FromPoint(point);
			IPoint nearestPoint = new ESRI.ArcGIS.Geometry.Point();
			_convertedProxiOperator.QueryNearestPoint(esriPoint, esriSegmentExtension.esriNoExtension, nearestPoint);
			Aran.Geometries.Point result = ConvertFromEsriGeom.ToPoint(nearestPoint);
			return result;
		}

		public double GetDistance(Geometry geom, Geometry other)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			IProximityOperator proximtyOperator = esriGeom1 as IProximityOperator;
			double distance = proximtyOperator.ReturnDistance(esriGeom2);
			return distance;
		}

		public double GetDistance(Geometry other)
		{
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			double distance = _convertedProxiOperator.ReturnDistance(esriGeom2);
			return distance;
		}

		#endregion

		#region Relational Operators

		public Boolean Contains(Geometry geom, Geometry other)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
			return relOper.Contains(esriGeom2);
		}

		public Boolean Contains(Geometry other)
		{
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			return _convertedRelOper.Contains(esriGeom2);
		}

		public Boolean Crosses(Geometry geom, Geometry other)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
			return relOper.Crosses(esriGeom2);
		}

		public Boolean Disjoint(Geometry geom, Geometry other)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
			return relOper.Disjoint(esriGeom2);
		}

		public Boolean Disjoint(Geometry other)
		{
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			return _convertedRelOper.Disjoint(esriGeom2);
		}

		public Boolean Equals(Geometry geom, Geometry other)
		{
			IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom);
			IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(other);
			IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
			return relOper.Equals(esriGeom2);
		}
		#endregion

		public Geometry GeoTransformations(Geometry geom, SpatialReference fromSR, SpatialReference toSR)
		{
			SpatRefConverter spatRefConverter = new SpatRefConverter();
            ISpatialReference fromEsriSpatRef = spatRefConverter.ToEsriSpatRef(fromSR);
            ISpatialReference toEsriSpatRef = spatRefConverter.ToEsriSpatRef(toSR);
            
            //In geometry convertation is needed z parametr
            IGeometry esriGeometry = ConvertToEsriGeom.FromGeometry(geom, true, fromEsriSpatRef);
            
          
			esriGeometry.SpatialReference = fromEsriSpatRef;
			esriGeometry.Project(toEsriSpatRef);

			if (esriGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
			{
				ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeometry;
				topoOper2.IsKnownSimple_2 = false;
				topoOper2.Simplify();
			}

			return ConvertFromEsriGeom.ToGeometry(esriGeometry,
				toSR.SpatialReferenceType == SpatialReferenceType.srtGeographic);
		}

		private void SimplifyGeometry(IGeometry geom)
		{
			ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)geom;
			topoOper2.IsKnownSimple_2 = false;
			topoOper2.Simplify();
		}

        public Geometry Buffer(Geometry geom, double width, int quadrantSegments)
        {
            IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
            IGeometry bufferGeom;

            if (esriGeom.GeometryType != esriGeometryType.esriGeometryPoint)
            {
                ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
                SimplifyGeometry(esriGeom);
                bufferGeom = topoOper2.Buffer(width);
            }
            else
            {
                ITopologicalOperator topoOper = (ITopologicalOperator)esriGeom;
                bufferGeom = topoOper.Buffer(width);
            }

            SimplifyGeometry(bufferGeom);
            Geometry result = ConvertFromEsriGeom.ToGeometry(bufferGeom);
            return result;
        }

        public Geometry BufferWithCapStyle(Geometry geom, double width, EndCapStyle bufferStyle = EndCapStyle.Round)
        {
            IGeometry esriGeom = ConvertToEsriGeom.FromGeometry(geom);
            IGeometry bufferGeom;

            if (esriGeom.GeometryType != esriGeometryType.esriGeometryPoint)
            {
                ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
                SimplifyGeometry(esriGeom);
                bufferGeom = topoOper2.Buffer(width);
            }
            else
            {
                ITopologicalOperator topoOper = (ITopologicalOperator)esriGeom;
                bufferGeom = topoOper.Buffer(width);
            }

            SimplifyGeometry(bufferGeom);
            Geometry result = ConvertFromEsriGeom.ToGeometry(bufferGeom);
            return result;
        }

        public bool Crosses(Geometry other)
        {
            IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(other);
            IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
            return relOper.Crosses(_convertedGeometry);
        }

        public Geometry Difference(Geometry geom1)
        {
            IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
            if (topoOper2 != null)
            {
                IGeometry differenceGeom = topoOper2.Difference(_convertedGeometry);
                if (differenceGeom.IsEmpty)
                    return null;

                SimplifyGeometry(differenceGeom);
                Geometry result = ConvertFromEsriGeom.ToGeometry(differenceGeom);
                return result;
            }
            else
            {
                return null;
            }
        }

        public Geometry SimplifyGeometry(Geometry geom, double tolerance = 0.001)
        {
            var esriGeom = Aran.Converters.ConvertToEsriGeom.FromGeometry(geom);
            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom;
            topoOper2.IsKnownSimple_2 = false;
            topoOper2.Simplify();
            return Aran.Converters.ConvertFromEsriGeom.ToGeometry(esriGeom);
        }

        public Geometry UnionGeometry(Geometry geom1)
        {
            IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(geom1);
            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)esriGeom1;
            if (topoOper2 != null)
            {
                IGeometry unionGeom = topoOper2.Union(_convertedGeometry);
                SimplifyGeometry(unionGeom);
                Geometry result = ConvertFromEsriGeom.ToGeometry(unionGeom);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// prefetched geometry
        /// </summary>
        private Geometry _currentGeometry;
		private IGeometry _convertedGeometry;
		private IRelationalOperator _convertedRelOper;
		private IProximityOperator _convertedProxiOperator;
	}
}
