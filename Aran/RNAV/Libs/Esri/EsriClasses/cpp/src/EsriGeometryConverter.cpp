#include "../include/EsriGeometryConverter.h"

namespace Esri
{
	GeometryConverter::~GeometryConverter ()
	{
	}

	Geometry* GeometryConverter::toGeometry()
	{
		return (Geometry*) asIGeometry();
	}

	Point* GeometryConverter::toPoint()
	{
		Point* obj = 0;
		IGeometry* geom = asIGeometry();
		if( geom != 0) 
			geom->QueryInterface(__uuidof(IPoint), (void**)&obj);
		return obj;
	}

	Ring* GeometryConverter::toRing()
	{
		Ring* obj = 0;
		IGeometry* geom = asIGeometry();
		if( geom != 0) 
			geom->QueryInterface(__uuidof(IRing), (void**)&obj);
		return obj;
	}

	Polygon* GeometryConverter::toPolygon()
	{
		Polygon* obj = 0;
		IGeometry* geom = asIGeometry();
		if( geom != 0) 
			geom->QueryInterface(__uuidof(IRing), (void**)&obj);
		return obj;
	}

	PointCollection* GeometryConverter::toPointCollection()
	{
		PointCollection* obj = 0;
		IGeometry* geom = asIGeometry();
		if( geom != 0) 
			geom->QueryInterface(__uuidof(IPointCollection), (void**)&obj);
		return obj;
	}

	GeometryCollection* GeometryConverter::toGeometryCollection()
	{
		GeometryCollection* obj = 0;
		IGeometry* geom = asIGeometry();
		if( geom != 0) 
			geom->QueryInterface(__uuidof(IGeometryCollection), (void**)&obj);
		return obj;
	}

	void GeometryConverter::free ()
	{
		delete this;
	}
}