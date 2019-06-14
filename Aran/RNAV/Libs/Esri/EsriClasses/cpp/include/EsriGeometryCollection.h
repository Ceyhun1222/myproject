#ifndef ESRI_GEOMETRY_COLLECTION_H
#define ESRI_GEOMETRY_COLLECTION_H

#include "EsriGeometry.h"

namespace Esri
{
	class GeometryCollection : public IGeometryCollection
	{
	public:
		Geometry* asGeometry();

		void appen(Geometry* geom);
		Geometry* getGeometry(int index);
		int count();
	};
}

#endif