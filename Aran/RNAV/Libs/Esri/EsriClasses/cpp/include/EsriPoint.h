#ifndef ESRI_POINT_H
#define ESRI_POINT_H

#include "EsriGeometry.h"

namespace Esri
{
	class Point : public IPoint
	{
	public:
		static Point* create ();
		static Point* create (double x, double y);

		Geometry* asGeometry(); 

		double getX();
		double getY();
		double getZ();
		double getM();

		void setX(double x);
		void setY(double y);
		void setZ(double z);
		void setM(double m);

		int pack(Handle handle);
		int unpack(Handle handle);
	};
}

#endif /*ESRI_POINT_H*/