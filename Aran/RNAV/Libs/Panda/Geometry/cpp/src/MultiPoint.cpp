#include "../include/MultiPoint.h"
#include "../include/Point.h"
#include "../include/GeometryType.h"

namespace Panda
{
	MultiPoint::MultiPoint() throw ()
	{
	}

	MultiPoint::~MultiPoint() throw ()
	{
		clear ();
	}

	void MultiPoint::addPoint (const Point& point)
	{
		_pointList.add (point);
	}

	void MultiPoint::insertPoint (int index, const Point& point)
	{
		_pointList.insert (index, point);
	}
	 
	const Point& MultiPoint::getPoint (int index) const
	{
		return _pointList.at (index);
	}

	void MultiPoint::removeAt ( int index)
	{
		_pointList.remove (index);
	}

	int MultiPoint::count() const
	{
		return _pointList.size ();
	}

	void MultiPoint::clear ()
	{
		_pointList.clear ();
	}

	Geometry* MultiPoint::clone() const throw (std::bad_cast)
	{
		MultiPoint* multiPoint = new MultiPoint ();
		try
		{
			multiPoint->assign (*this);
		}
		catch (std::bad_cast e)
		{
			delete multiPoint;
			throw e;
		}

		return multiPoint;
	}

	void MultiPoint::assign (const Geometry& geometry) throw (std::bad_cast)
	{
		const MultiPoint& src = dynamic_cast <const MultiPoint&> (geometry);
		_pointList.assign (src._pointList);
	}

	void MultiPoint::pack (Handle handle) const throw (Registry::Exception)
	{
		_pointList.pack (handle);
	}

	void MultiPoint::unpack (Handle handle) throw (Registry::Exception)
	{
		_pointList.unpack (handle);
	}

	int MultiPoint::geometryType () const throw ()
	{
		return GeometryType::MultiPoint;
	}
}
