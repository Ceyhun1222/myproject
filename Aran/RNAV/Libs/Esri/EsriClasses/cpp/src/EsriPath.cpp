#include "../include/EsriPath.h"
#include "../include/EsriPointCollection.h"
#include <unknwn.h>

namespace Esri
{
	Path* Path::create()
	{
		return (Path*)_CoCreateInstance(CLSID_Path,IID_IPath);
	}

	Geometry* Path::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

	int Path::pack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->pack(handle);
	}

	int Path::unpack(Handle handle)
	{
		PointCollection* ptCol = asGeometry()->asPointCollection();
		return ptCol->unpack(handle);
	}
}