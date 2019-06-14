#include "../include/EsriPolygon.h"
#include "../include/EsriGeometryCollection.h"
#include <unknwn.h>

namespace Esri
{
	Polygon* Polygon::create()
	{
		return (Polygon*)_CoCreateInstance(CLSID_Polygon, IID_IPolygon);
	}

	Geometry* Polygon::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	void Polygon::addRing(Ring* ring)
	{
		asGeometry()->asGeometryCollection()->AddGeometry(ring);
	}

	int Polygon::count()
	{
		return asGeometry()->asGeometryCollection()->count();
	}

	int Polygon::pack(Handle handle)
	{
		GeometryCollection* geomCol = asGeometry()->asGeometryCollection();
		int geomCount = geomCol->count();
		int result = Registry_putInt32 (handle, geomCount); if( result != rcOk) return result;
		
		for(int i=0 ; i<geomCount ; i++)
		{
			PointCollection* ptCol = geomCol->getGeometry(i)->asPointCollection();
			result = ptCol->pack(handle); if( result != rcOk) return result;
		}

		return result;
	}
	
	int Polygon::unpack(Handle handle)
	{
		GeometryCollection* geomCol = asGeometry()->asGeometryCollection();
		int geomCount = geomCol->count();
		geomCol->RemoveGeometries(0, geomCount);
		
		int result = Registry_getInt32 (handle, geomCount); if( result != rcOk) return result;

		for(int i=0 ; i<geomCount ; i++)
		{
			Ring* ring = Ring::create();
			result = ring->unpack(handle);
			if( result != rcOk)
			{
				//ring->Release();
				delete ring;
				return result;
			}
			addRing(ring);
		}

		//delete ring;

		return result;
	}
}