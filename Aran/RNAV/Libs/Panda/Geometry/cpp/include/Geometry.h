#ifndef GEOMETRY_H
#define GEOMETRY_H

#include <cmath>
#include <Panda/Registry/cpp/include/Contract.h>
#include <typeinfo>

#ifndef isNaN
	#define isNaN(x) ((x) != (x))
#endif
#ifndef NaN
	#define NaN log(-1.0)
#endif

namespace Panda
{
	class Point;
	class MultiPoint;
	class Part;
	class Ring;
	class Polyline;
	class Polygon;

	class Geometry
	{
		public:
			virtual ~Geometry () throw ();
			virtual int geometryType() const throw () = 0;

			virtual void assign (const Geometry& geometry) throw (std::bad_cast) = 0;
			virtual Geometry* clone () const throw (std::bad_cast) = 0;

			virtual void pack (Handle handle) const throw (Registry::Exception) = 0;
			virtual void unpack (Handle handle) throw (Registry::Exception) = 0;

			Point& asPoint() throw (std::bad_cast);
			MultiPoint& asMultiPoint() throw (std::bad_cast);
			Part& asPart() throw (std::bad_cast);
			Ring& asRing() throw (std::bad_cast);
			Polyline& asPolyline() throw (std::bad_cast);
			Polygon& asPolygon() throw (std::bad_cast);

			const Point& asPoint() const throw (std::bad_cast);
			const MultiPoint& asMultiPoint() const throw (std::bad_cast);
			const Part& asPart() const throw (std::bad_cast);
			const Ring& asRing() const throw (std::bad_cast);
			const Polyline& asPolyline() const throw (std::bad_cast);
			const Polygon& asPolygon() const throw (std::bad_cast);
	};
}

#endif /*GOMETRY_H*/
