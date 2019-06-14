#include "../include/Polygon.h"
#include "../include/Ring.h"
#include "../include/GeometryType.h"

namespace Panda
{
	Geometry* Polygon::clone() const throw (std::bad_cast)
	{
		Polygon* polygon = new Polygon ();
        try
        {
		    polygon->assign (*this);
        }
        catch (std::bad_cast e)
        {
            delete polygon;
            throw e;
        }
		return polygon;
	}

	int Polygon::geometryType () const throw ()
	{
		return GeometryType::Polygon;
	}
}
