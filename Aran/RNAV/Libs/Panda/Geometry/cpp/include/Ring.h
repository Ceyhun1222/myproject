#ifndef RING_H
#define RING_H

#include "MultiPoint.h"

namespace Panda
{
	class Ring: public MultiPoint
	{
		public:
			virtual int geometryType () const throw ();
			virtual Geometry* clone () const throw (std::bad_cast);
	};

}

#endif
