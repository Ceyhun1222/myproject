#include "../include/EsriPolyline.h"
#include "../include/EsriGeometryCollection.h"
#include "../include/EsriPointCollection.h"
#include "../include/EsriPath.h"
#include <unknwn.h>

namespace Esri
{
	Polyline* Polyline::create()
	{
		return (Polyline*)_CoCreateInstance(CLSID_Polyline, IID_IPolyline);
	}

	Geometry* Polyline::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	int Polyline::pack(Handle handle)
	{
		GeometryCollection* geomCol = asGeometry()->asGeometryCollection();
		int pathCount = geomCol->count();
		int result = Registry_putInt32 (handle, pathCount); if( result != rcOk) return result;

		for(int i=0 ; i<pathCount ; i++)
		{
			PointCollection* ptCol = geomCol->getGeometry(i)->asPointCollection();
			result = ptCol->pack(handle); if( result != rcOk) return result;
		}

		return result;
	}

	int Polyline::unpack(Handle handle)
	{
		GeometryCollection* geomCol = asGeometry()->asGeometryCollection();
		int pathCount = geomCol->count();
		geomCol->RemoveGeometries(0, pathCount);
		int result = Registry_getInt32 (handle, pathCount); if( result != rcOk) return result;

		for(int i=0 ; i<pathCount ; i++)
		{
			Path* path = Path::create();
			result = path->unpack(handle);
			if( result != rcOk)
			{
				//path->Release();
				delete path;
				return result;
			}
			geomCol->appen(path->asGeometry());
		}

		return result;
	}
}