#include "../include/Ring.h"
#include "../include/GeometryType.h"

namespace Panda
{
	int Ring::geometryType () const throw ()
	{
		return GeometryType::Ring;
	}

	Geometry* Ring::clone () const throw (std::bad_cast)
	{
	    Ring* ring = new Ring;
        
        try
        {
    		ring->assign (*this);
        }
        catch (std::bad_cast e)
        {
            delete ring;
            throw e;
        }
        
		return ring;
	}
}
