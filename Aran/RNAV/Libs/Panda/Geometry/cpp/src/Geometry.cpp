#include "../include/Geometry.h"
#include "../include/Point.h"
#include "../include/MultiPoint.h"
#include "../include/Part.h"
#include "../include/Ring.h"
#include "../include/Polyline.h"
#include "../include/Polygon.h"

namespace Panda
{
	Geometry::~Geometry() throw ()
	{
	}

	Point& Geometry::asPoint() throw (std::bad_cast)
	{
		return dynamic_cast <Point&> (*this);
	}

	MultiPoint& Geometry::asMultiPoint() throw (std::bad_cast)
	{
		return dynamic_cast <MultiPoint&> (*this);
	}

	Part& Geometry::asPart() throw (std::bad_cast)
	{
		return dynamic_cast <Part&> (*this);
	}

	Ring& Geometry::asRing() throw (std::bad_cast)
	{
		return dynamic_cast<Ring&> (*this);
	}

	Polyline& Geometry::asPolyline() throw (std::bad_cast)
	{
		return dynamic_cast<Polyline&>(*this);
	}

	Polygon& Geometry::asPolygon() throw (std::bad_cast)
	{
		return dynamic_cast<Polygon&>(*this);
	}

	const Point& Geometry::asPoint() const throw (std::bad_cast)
	{
		return dynamic_cast<const Point&>(*this);
	}

	const MultiPoint& Geometry::asMultiPoint() const throw (std::bad_cast)
	{
		return dynamic_cast<const MultiPoint&>(*this);
	}

	const Part& Geometry::asPart() const throw (std::bad_cast)
	{
		return dynamic_cast<const Part&>(*this);
	}

	const Ring& Geometry::asRing() const throw (std::bad_cast)
	{
		return dynamic_cast<const Ring&>(*this);
	}

	const Polyline& Geometry::asPolyline() const throw (std::bad_cast)
	{
		return dynamic_cast<const Polyline&>(*this);
	}

	const Polygon& Geometry::asPolygon() const throw (std::bad_cast)
	{
		return dynamic_cast <const Polygon&>(*this);
	}
}
