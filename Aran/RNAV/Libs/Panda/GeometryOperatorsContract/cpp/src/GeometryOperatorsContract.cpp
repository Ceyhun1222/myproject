#include "../include/GeometryOperatorsContract.h"

#include <Panda/Geometry/cpp/include/GeometryType.h>
#include <Panda/Geometry/cpp/include/Geometry.h>
#include <Panda/Geometry/cpp/include/GeometryPacket.h>
#include <Panda/Geometry/cpp/include/NullGeometry.h>
#include <Panda/Geometry/cpp/include/Point.h>
#include <Panda/Geometry/cpp/include/MultiPoint.h>
#include <Panda/Geometry/cpp/include/Part.h>
#include <Panda/Geometry/cpp/include/Ring.h>
#include <Panda/Geometry/cpp/include/Polygon.h>
#include <Panda/Geometry/cpp/include/Polyline.h>
#include <../include/SpatialReference.h>

namespace Panda
{
	GeometryOperatorsContract::GeometryOperatorsContract () throw ()
	{
        try
        {
		    Registry::getInstance ("GeometryOperatorsService", _handle);
        }
        catch (Registry::Exception)
        {
        }
	}

	GeometryOperatorsContract::~GeometryOperatorsContract () throw ()
	{
		try
		{
			Registry::freeInstance (_handle);
		}
		catch (Registry::Exception)
		{
		}
	}

    bool GeometryOperatorsContract::isValid () const throw ()
    {
		return (_handle != 0);
    }

	const Geometry& GeometryOperatorsContract::unionGeometry (const Geometry& geom1, const Geometry& geom2) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::union_);
	    packGeometry (_handle, geom1);
        packGeometry (_handle, geom2);
		Registry::endMessage (_handle);

		return unpackGeometry (_handle);
	}

	const Geometry& GeometryOperatorsContract::convexHull (const Geometry& geom) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::convexHull);
		packGeometry (_handle, geom);
		Registry::endMessage (_handle);

		return unpackGeometry (_handle);
	}

	void GeometryOperatorsContract::cut (const Geometry& geom,const Polyline& cutter, const Geometry*& geomLeft, const Geometry*& geomRight) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::cut);
		packGeometry (_handle, geom);
		packGeometry (_handle, cutter); 
		Registry::endMessage (_handle);

		geomLeft = &(unpackGeometry(_handle));
		geomRight = &(unpackGeometry(_handle));
	}

	const Geometry& GeometryOperatorsContract::intersect (const Geometry& geom1, const Geometry& geom2) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::intersect);
		packGeometry (_handle, geom1);
		packGeometry (_handle, geom2);
		Registry::endMessage (_handle);

		return unpackGeometry(_handle);
	}

	const Geometry& GeometryOperatorsContract::boundary (const Geometry& geom) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::boundary);
		packGeometry (_handle, geom);
		Registry::endMessage (_handle);

		return unpackGeometry(_handle);
	}

	const Geometry& GeometryOperatorsContract::buffer (const Geometry& geom, double width) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::buffer);
		packGeometry (_handle, geom);
		Registry::putDouble (_handle, width);
		Registry::endMessage (_handle);

		return unpackGeometry(_handle);
	}

	const Geometry& GeometryOperatorsContract::difference (const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::difference);
		packGeometry (_handle, geom);
		packGeometry (_handle, other); 
		Registry::endMessage (_handle);

		return unpackGeometry(_handle);
	}

	const Point& GeometryOperatorsContract::nearestPoint(const Geometry& geom, const Point& point) throw (Registry::Exception, std::bad_cast)
	{
		Registry::beginMessage (_handle, Commands::getNearestPoint);
		packGeometry (_handle, geom);
		packGeometry (_handle, point);
		Registry::endMessage (_handle);
		
		return unpackGeometry (_handle).asPoint ();
	}

	double GeometryOperatorsContract::distance(const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::getDistance);
		packGeometry (_handle, geom);
		packGeometry (_handle, other);
		Registry::endMessage (_handle);

		return Registry::getDouble(_handle);
	}

	bool GeometryOperatorsContract::contains (const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::contains);
		packGeometry (_handle, geom);
		packGeometry (_handle, other);
		Registry::endMessage (_handle);
		
		return Registry::getBool (_handle);
	}

	bool GeometryOperatorsContract::crosses (const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::crosses);
		packGeometry (_handle, geom);
		packGeometry (_handle, other);
		Registry::endMessage (_handle);
		
		return Registry::getBool (_handle);
	}

	bool GeometryOperatorsContract::disjoint (const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::disjoint);
		packGeometry (_handle, geom);
		packGeometry (_handle, other);
		Registry::endMessage (_handle);
		
		return Registry::getBool (_handle);
	}

	bool GeometryOperatorsContract::equals (const Geometry& geom, const Geometry& other) throw (Registry::Exception)
	{
		Registry::beginMessage (_handle, Commands::equals);
		packGeometry (_handle, geom);
		packGeometry (_handle, other);
		Registry::endMessage (_handle);
		
		return Registry::getBool (_handle);
	}

	const Geometry& GeometryOperatorsContract::geoTransformations(
			const Geometry& geom, 
			const SpatialReference& fromSR, 
			const SpatialReference& toSR)
	{
		Registry::beginMessage (_handle, Commands::geoTransformations);
		packGeometry(_handle, geom);
		fromSR.pack(_handle);
		toSR.pack(_handle);
		Registry::endMessage (_handle);

		return unpackGeometry (_handle);
	}
}
