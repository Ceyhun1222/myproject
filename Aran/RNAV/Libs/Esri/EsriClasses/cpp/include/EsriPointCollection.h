#ifndef ESRI_POINTCOLLECTION_H
#define ESRI_POINTCOLLECTION_H

#include "EsriGeometry.h"
#include "EsriPoint.h"

namespace Esri
{
	class PointCollection : public IPointCollection
	{
	public:
		static PointCollection* createPolygon();
		static PointCollection* create(const GUID& guid);

		Geometry* asGeometry(); 

		void append(Point* p);
		int count();
		Point* getPoint(int index);

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_POINTCOLLECTION_H*/