#ifndef ESRI_RING_H
#define ESRI_RING_H

#include "EsriGeometry.h"
#include "EsriPointCollection.h"

namespace Esri
{
	class Ring : public IRing
	{
	public:
		static Ring* create();

		Geometry* asGeometry(); 

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_RING_H*/