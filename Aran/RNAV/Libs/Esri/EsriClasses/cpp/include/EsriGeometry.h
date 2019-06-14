#ifndef ESRI_GEOMETRY_H
#define ESRI_GEOMETRY_H

#include "ImportEsri.h"

#include <Panda/Registry/cpp/include/Contract.h>

namespace Esri
{
	class Geometry;
    class Point;
	class Path;
    class Ring;
	class Polyline;
    class Polygon;
    class PointCollection;
    class GeometryCollection;

	class Geometry: public IGeometry
	{
	public:
		static Geometry* create(const GUID& guid);

		esriGeometryType getType();

		Point* asPoint();
		Path* asPath();
		Ring* asRing();
		Polyline* asPolyline();
		Polygon* asPolygon();
		PointCollection* asPointCollection();
		GeometryCollection* asGeometryCollection();

		// #region ITopologicalOperators
		void Simplify();
		Geometry* Buffer(double distance);
		Geometry* Difference(Geometry* other);
		Geometry* Union(Geometry* other);
		Geometry* ConvexHull();
		void Cut(Polyline* cutter, Geometry*& geomLeft, Geometry*& geomRight);
		Geometry* Intersect(Geometry* other, esriGeometryDimension geomDimension);
		Geometry* Boundary();
		// #endregion

		// #region IProximityOperators
		Point* getNearestPoint(Point* point, esriSegmentExtension extension);
		double getDistanse(Geometry* other);
		// #endregion

		//#region IRelationalOperators
		bool Contains(Geometry* other);
		bool Crosses(Geometry* other);
		bool Disjoint(Geometry* other);
		bool Equals(Geometry* other);
		//#endregion

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif
