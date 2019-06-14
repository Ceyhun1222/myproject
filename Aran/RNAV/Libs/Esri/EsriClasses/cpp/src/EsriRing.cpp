#include "../include/EsriRing.h"
#include <unknwn.h>

namespace Esri
{
	Ring* Ring::create()
	{
		return (Ring*)_CoCreateInstance(CLSID_Ring, IID_IRing);
	}
	
	Geometry* Ring::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	int Ring::pack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->pack(handle);
	}

	int Ring::unpack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->unpack(handle);
	}
}