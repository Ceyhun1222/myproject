#include "../include/EsriPointCollection.h"
#include <unknwn.h>

namespace Esri
{
	PointCollection* PointCollection::createPolygon()
	{
		return (PointCollection*)_CoCreateInstance(CLSID_Polygon, IID_IPointCollection);
	}

	PointCollection* PointCollection::create(const GUID& guid)
	{
		return (PointCollection*) _CoCreateInstance(guid, IID_IPointCollection);
	}

	Geometry* PointCollection::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	void PointCollection::append(Point* p)
	{
		AddPoint(p, 0, 0);
	}

	int PointCollection::count()
	{
		long pointCount = -1;
		get_PointCount(&pointCount);
		return pointCount;
	}

	Point* PointCollection::getPoint(int index)
	{
		Point* point;
		get_Point(index, (IPoint**)&point);
		return point;
	}

	int PointCollection::pack(Handle handle)
	{
		int _count = count();
		int result = Registry_putInt32 (handle, _count); if( result != rcOk) return result;

		for(int i=0 ; i<_count ; i++)
		{
			result = getPoint(i)->pack(handle); if( result != rcOk) return result;
		}

		return result;
	}
	
	int PointCollection::unpack(Handle handle)
	{
		RemovePoints(0, count());
		int _count = 0;
		int result = Registry_getInt32 (handle, _count); if( result != rcOk) return result;
		Point* point = Point::create();

		for(int i=0 ; i<_count ; i++)
		{
			result = point->unpack(handle);
			if( result != rcOk)
			{
				//point->Release();
				delete point;
				return result;
			}
			append(point);
		}

		//point->Release();
		//delete point;
		return result;
	}
}
