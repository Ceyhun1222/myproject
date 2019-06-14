#ifndef ESRI_GEOMETRY_CONVERTER_H
#define ESRI_GEOMETRY_CONVERTER_H

#include "esriImport.h"

namespace Esri
{
	class Geometry;
	class Point;
	class Ring;
	class Polygon;
	class PointCollection;
	class GeometryCollection;


	class GeometryConverter
	{
	public:
		virtual IGeometry* asIGeometry() = 0;
		virtual ~GeometryConverter ();
		Geometry* toGeometry();
		Point* toPoint();
		Ring* toRing();
		Polygon* toPolygon();
		PointCollection* toPointCollection();
		GeometryCollection* toGeometryCollection();
		void free ();
	};
}

#endif /*ESRI_GEOMETRY_CONVERTER_H*/