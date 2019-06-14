#ifndef ESRI_POLYGON_H
#define ESRI_POLYGON_H

#include "EsriGeometry.h"
#include "EsriRing.h"

namespace Esri
{
	class Polygon : public IPolygon
	{
	public:
		static Polygon* create();

		Geometry* asGeometry(); 

		void addRing(Ring* ring);
		int count();

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_POLYGON_H*/