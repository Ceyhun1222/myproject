#include "../include/EsriGeometryCollection.h"
#include <unknwn.h>

namespace Esri
{
	Geometry* GeometryCollection::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	void GeometryCollection::appen(Geometry* geom)
	{
		AddGeometry(geom, 0 , 0);
	}

	Geometry* GeometryCollection::getGeometry(int index)
	{
		Geometry* geom = 0;
		get_Geometry(index, (IGeometry**)&geom);
		return geom;
	}

	int GeometryCollection::count()
	{
		long geomCount = 0;
		get_GeometryCount(&geomCount);
		return geomCount;
	}
}