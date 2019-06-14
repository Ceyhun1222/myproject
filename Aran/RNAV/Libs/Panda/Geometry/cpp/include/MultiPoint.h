#ifndef MULTIPOINT_H
#define MULTIPOINT_H

#include "Geometry.h"
#include <Panda/Common/cpp/include/List.h>

namespace Panda
{
	class Point;

	class MultiPoint: public Geometry
	{
		public:
			MultiPoint() throw ();
			virtual ~MultiPoint () throw ();

			virtual void assign (const Geometry& geometry) throw (std::bad_cast);
			virtual Geometry* clone () const throw (std::bad_cast);

			virtual void pack (Handle handle) const throw (Registry::Exception);
			virtual void unpack (Handle handle) throw (Registry::Exception);
			virtual int geometryType () const throw ();

			void addPoint (const Point& point);
			void insertPoint (int index, const Point& point);
			const Point& getPoint (int index) const;
			void removeAt (int index);
			int count () const;
			void clear ();

		private:
			List <Point> _pointList;
	};
}

#endif
