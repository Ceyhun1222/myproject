#include "../include/Point.h"
#include "../include/GeometryType.h"

namespace Panda
{

	Point::Point() throw ()
	{
		setEmpty ();
	}

	Point::Point (double x, double y) throw ()
	{
		setCoords (x, y);
	}

	void Point::setCoords (double x, double y) throw ()
	{
		setX(x);
		setY(y);
	}

	void Point::setX (double x) throw ()
	{
		_x = x;
	}

	double Point::getX () const throw ()
	{
		return _x;
	}

	void Point::setY(double y) throw ()
	{
		_y = y;
	}

	double Point::getY() const throw ()
	{
		return _y;
	}

	void Point::setZ(double z) throw ()
	{
		_z = z;
	}

	double Point::getZ() const throw ()
	{
		return _z;
	}

	void Point::setM(double m) throw ()
	{
		_m = m;
	}

	double Point::getM() const throw ()
	{
		return _m;
	}

	void Point::setT(double t) throw ()
	{
		_t = t;
	}

	double Point::getT() const throw ()
	{
		return _t;
	}

	bool Point::isEmpty() const throw ()
	{
		return  (isNaN(_x) || isNaN(_y));
	}

	void Point::setEmpty() throw ()
	{
		_x = NaN;
		_y = NaN;
		_z = NaN;
		_t = NaN;
		_m = NaN;
	}

	void Point::assign (const Geometry& geometry) throw (std::bad_cast)
	{
		const Point& src = dynamic_cast <const Point&> (geometry);
		_x = src._x;
		_y = src._y;
		_z = src._z;
		_m = src._m;
		_t = src._t;
	}

	Geometry* Point::clone() const throw (std::bad_cast)
	{
		Point* p = new Point();
        
        try
        {
		    p->assign (*this);
        }
        catch (std::bad_cast e)
        {
            delete p;
            throw e;
        }
        
		return p;
	}

	void Point::pack (Handle handle) const throw (Registry::Exception)
	{
		Registry::putDouble (handle, _x);
		Registry::putDouble (handle, _y);
		Registry::putDouble (handle, _z);
		Registry::putDouble (handle, _m);
		Registry::putDouble (handle, _t);
	}

	void Point::unpack (Handle handle) throw (Registry::Exception)
	{
		Registry::getDouble (handle, _x);
		Registry::getDouble (handle, _y);
		Registry::getDouble (handle, _z);
		Registry::getDouble (handle, _m);
		Registry::getDouble (handle, _t);
	}

	int Point::geometryType () const throw ()
	{
		return GeometryType::Point;
	}
}
