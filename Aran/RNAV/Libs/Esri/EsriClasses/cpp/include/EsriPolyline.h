#ifndef ESRI_POLYLINE_H
#define ESRI_POLYLINE_H

#include "EsriGeometry.h"

namespace Esri
{
	class Polyline : public IPolyline
	{
	public:
		static Polyline* create();

		Geometry* asGeometry(); 

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_POLYLINE_H*/