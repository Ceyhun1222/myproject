#ifndef ESRI_MULTIPOINT_H
#define ESRI_MULTIPOINT_H

#include "EsriGeometry.h"

namespace Esri
{
	class MultiPoint : public IMultipoint
	{
	public:
		static MultiPoint* create();

		Geometry* asGeometry();

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_MULTIPOINT_H*/