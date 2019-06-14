#include "../include/EsriMultiPoint.h"
#include "../include/EsriPointCollection.h"
#include <unknwn.h>

namespace Esri
{
    MultiPoint* MultiPoint::create()
    {
        return (MultiPoint*)_CoCreateInstance(CLSID_Multipoint, IID_IMultipoint);
	}

	Geometry* MultiPoint::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	int MultiPoint::pack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->pack(handle);
	}

	int MultiPoint::unpack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->unpack(handle);
	}
}