#include "../include/EsriGeometry.h"
#include "../include/EsriPoint.h"
#include "../include/EsriMultiPoint.h"
#include "../include/EsriPath.h"
#include "../include/EsriRing.h"
#include "../include/EsriPolyline.h"
#include "../include/EsriPolygon.h"
//#include <unknwn.h>

namespace Esri
{
	Geometry* Geometry::create(const GUID& guid)
	{
		return (Geometry*) _CoCreateInstance(guid, IID_IGeometry);
	}

	esriGeometryType Geometry::getType ()
	{
		esriGeometryType geomType = esriGeometryNull;
		get_GeometryType(&geomType);
		return geomType;
	}

	Point* Geometry::asPoint()
	{
		Point* obj = 0;
		QueryInterface(__uuidof(IPoint), (void**)&obj);
		return obj;
	}

	Path* Geometry::asPath()
	{
		Path* obj = 0;
		QueryInterface(__uuidof(IPath), (void**)&obj);
		return obj;
	}
	
	Ring* Geometry::asRing()
	{
		Ring* obj = 0;
		QueryInterface(__uuidof(IRing), (void**)&obj);
		return obj;
	}

	Polyline* Geometry::asPolyline()
	{
		Polyline* obj = 0;
		QueryInterface(__uuidof(IPolyline), (void**)&obj);
		return obj;
	}

	Polygon* Geometry::asPolygon()
	{
		Polygon* obj = 0;
		QueryInterface(__uuidof(IPolygon), (void**)&obj);
		return obj;
	}

	PointCollection* Geometry::asPointCollection()
	{
		PointCollection* obj = 0;
		QueryInterface(__uuidof(IPointCollection), (void**)&obj);
		return obj;
	}

	GeometryCollection* Geometry::asGeometryCollection()
	{
		GeometryCollection* obj = 0;
		QueryInterface(__uuidof(IGeometryCollection), (void**)&obj);
		return obj;
	}

	int Geometry::pack(Handle handle)
	{
		switch( getType())
		{
			case esriGeometryPoint:
				return asPoint()->pack(handle);
			case esriGeometryMultipoint:
			case esriGeometryPath:
			case esriGeometryRing:
				return asPointCollection()->pack(handle);
			case esriGeometryPolyline:
				return asPolyline()->pack(handle);
			case esriGeometryPolygon:
				return asPolygon()->pack(handle);
		}

		return rcInvalid;
	}

	int Geometry::unpack(Handle handle)
	{
		switch( getType())
		{
			case esriGeometryPoint:
				return asPoint()->unpack(handle);
			case esriGeometryMultipoint:
			case esriGeometryPath:
			case esriGeometryRing:
				return asPointCollection()->unpack(handle);
			case esriGeometryPolyline:
				return asPolyline()->unpack(handle);
			case esriGeometryPolygon:
				return asPolygon()->unpack(handle);
		}

		return rcInvalid;
	}

	void Geometry::Simplify()
	{
		esriGeometryType geomType = esriGeometryNull;
		get_GeometryType(&geomType);
		
		if (geomType != esriGeometryPolygon)
			return;

		ITopologicalOperator2* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator2), (void**)&topoOper);
		
		if( topoOper != 0)
			topoOper->put_IsKnownSimple(VARIANT_FALSE);
		else
			QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);

		topoOper->Simplify();
	}

	Geometry* Geometry::Buffer(double distance)
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		Geometry* resultGeom = 0;
		topoOper->Buffer(distance, (IGeometry**)&resultGeom);
		return resultGeom;
	}

	Geometry* Geometry::Difference(Geometry* other)
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		Geometry* resultGeom = 0;
		topoOper->Difference(other, (IGeometry**)&resultGeom);
		return resultGeom;
	}

	Geometry* Geometry::Union(Geometry* other)
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		Geometry* resultGeom = 0;
		topoOper->Union(other, (IGeometry**)&resultGeom);
		return resultGeom;
	}

	Geometry* Geometry::ConvexHull()
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		
		Geometry* resultGeom = 0;
		topoOper->ConvexHull((IGeometry**)&resultGeom);
		return resultGeom;
	}

	void Geometry::Cut(Polyline* cutter, Geometry*& geomLeft, Geometry*& geomRight)
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		
		topoOper->Cut(cutter, (IGeometry**)&geomLeft, (IGeometry**)&geomRight);
	}

	Geometry* Geometry::Intersect(Geometry* otherGeometry, esriGeometryDimension geomDimension)
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		
		Geometry* resultGeom = 0;
		topoOper->Intersect(otherGeometry, geomDimension, (IGeometry**)&resultGeom);		
		return resultGeom;
	}

	Geometry* Geometry::Boundary()
	{
		ITopologicalOperator* topoOper = 0;
		QueryInterface(__uuidof(ITopologicalOperator), (void**)&topoOper);
		
		Geometry* resultGeom = 0;
		topoOper->get_Boundary((IGeometry**)&resultGeom);
		return resultGeom;
	}

	Point* Geometry::getNearestPoint(Point* point, esriSegmentExtension extension)
	{
		IProximityOperator* proxOper = 0;
		QueryInterface(__uuidof(IProximityOperator), (void**)&proxOper);
		Point* nearestPoint = 0;
		proxOper->ReturnNearestPoint(point, extension,(IPoint**)&nearestPoint);
		return nearestPoint;
	}

	double Geometry::getDistanse(Geometry* other)
	{
		IProximityOperator* proxOper = 0;
		QueryInterface(__uuidof(IProximityOperator), (void**)&proxOper);
		double distance = 0;
		proxOper->ReturnDistance(other, &distance);
		return distance;
	}

	bool Geometry::Contains(Geometry* other)
	{
		IRelationalOperator* relOper = 0;
		QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
		VARIANT_BOOL resultVal;
		relOper->Contains(other, &resultVal);
		return (resultVal == VARIANT_TRUE);
	}

	bool Geometry::Crosses(Geometry* other)
	{
		IRelationalOperator* relOper = 0;
		QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
		VARIANT_BOOL resultVal;
		relOper->Crosses(other, &resultVal);
		return (resultVal == VARIANT_TRUE);
	}

	bool Geometry::Disjoint(Geometry* other)
	{
		IRelationalOperator* relOper = 0;
		QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
		VARIANT_BOOL resultVal;
		relOper->Disjoint(other, &resultVal);
		return (resultVal == VARIANT_TRUE);
	}

	bool Geometry::Equals(Geometry* other)
	{
		IRelationalOperator* relOper = 0;
		QueryInterface(__uuidof(IRelationalOperator), (void**)&relOper);
		VARIANT_BOOL resultVal;
		relOper->Equals(other, &resultVal);
		return (resultVal == VARIANT_TRUE);
	}
}