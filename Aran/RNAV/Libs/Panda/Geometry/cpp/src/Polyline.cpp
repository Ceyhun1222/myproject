#include "../include/Polyline.h"
#include "../include/Part.h"
#include "../include/GeometryType.h"    

namespace Panda
{
	Geometry* Polyline::clone() const throw (std::bad_cast)
	{
		Polyline* polyline = new Polyline ();
        try
        {
    		polyline->assign (*this);
        }
        catch (std::bad_cast e)
        {
            delete polyline;
            throw e;
        }
		return polyline;
	}

	int Polyline::geometryType () const throw ()
	{
		return GeometryType::Polyline;
	}
}
