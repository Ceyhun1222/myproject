#ifndef NULL_GEOMETRY_H
#define NULL_GEOMETRY_H

#include "GeometryType.h"
#include "Geometry.h"

namespace Panda
{
    class NullGeometry : public Geometry
    {
	public:
		virtual int geometryType() const throw ()
		{
			return GeometryType::Null;
		};

		virtual void assign (const Geometry&) throw ()
		{
		};
		
		virtual Geometry* clone () const throw ()
		{
			return new NullGeometry ();
		};

		virtual void pack (Handle) const throw ()
		{
		};

		virtual void unpack (Handle) throw ()
		{
		};
    };
}

#endif //NULL_GEOMETRY_H
