#ifndef POLYGON_H
#define POLYGON_H

#include "Poly.h"

namespace Panda
{
	class Ring;

	class Polygon:  public Poly <Ring>
	{
		public:
			virtual Geometry* clone () const throw (std::bad_cast);
			virtual int geometryType () const throw ();
	};
}

#endif
