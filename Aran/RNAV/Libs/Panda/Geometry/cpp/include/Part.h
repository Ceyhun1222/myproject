#ifndef PART_H
#define PART_H

#include "MultiPoint.h"

namespace Panda
{
    class Part: public MultiPoint
    {
		public:
			virtual int geometryType () const throw ();
			virtual Geometry* clone () const throw (std::bad_cast);
    };
}

#endif
