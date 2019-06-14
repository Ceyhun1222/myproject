#ifndef POLYLINE_H
#define POLYLINE_H

#include "Poly.h"

namespace Panda
{
	class Part;

    class Polyline: public Poly <Part>
    {
		public:
			virtual Geometry* clone () const throw (std::bad_cast);
			virtual int geometryType () const throw ();
    };
}

#endif /*POLYLINE_H*/
