#include "../include/Part.h"
#include "../include/GeometryType.h"    

namespace Panda
{
	int Part::geometryType () const throw ()
	{
		return GeometryType::Part;
	}

	Geometry* Part::clone () const throw (std::bad_cast)
	{
		Part* part = new Part;
        try
        {
    		part->assign (*this);
        }
        catch (std::bad_cast e)
        {
            delete part;
            throw e;
        }
        
		return part;
	}
}
