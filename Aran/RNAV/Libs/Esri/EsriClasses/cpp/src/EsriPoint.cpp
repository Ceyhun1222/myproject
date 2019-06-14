#include "../include/EsriPoint.h"
#include <unknwn.h>

namespace Esri
{
    Point* Point::create()
    {
		return (Point*)_CoCreateInstance(CLSID_Point,IID_IPoint);
    }

    Point* Point::create(double x, double y)
    {
        Point* p = create();
        if( p != 0)
			p->PutCoords(x,y);
        return p;
    }

	Geometry* Point::asGeometry()
	{
		Geometry* geom = 0;
		QueryInterface(__uuidof(IGeometry), (void**)&geom);
		return geom;
	}

    double Point::getX()
    {
        double x;
        get_X(&x);
        return x;
    }
    
	double Point::getY()
    {
        double y;
        get_Y(&y);
        return y;
    }

    double Point::getZ()
    {
        double z;
        get_Z(&z);
        return z;
    }

    double Point::getM()
    {
        double m;
        get_M(&m);
        return m;
    }

	void Point::setX(double x)
	{
		put_X(x);
	}
	
	void Point::setY(double y)
	{
		put_Y(y);
	}

	void Point::setZ(double z)
	{
		put_Z(z);
	}

	void Point::setM(double m)
	{
		put_M(m);
	}

	int Point::pack(Handle handle)
	{
		int result;

		result = Registry_putDouble (handle, getX()); if (result != rcOk) return result;
		result = Registry_putDouble (handle, getY()); if (result != rcOk) return result;
		result = Registry_putDouble (handle, getZ()); if (result != rcOk) return result;
		result = Registry_putDouble (handle, getM()); if (result != rcOk) return result;
		double t = 0.0;
		result = Registry_putDouble (handle, t);

		return result;
	}

	int Point::unpack(Handle handle)
	{
		double x, y, z, m, t;
		int result;
		result = Registry_getDouble(handle, x); if( result != rcOk) return result;
		result = Registry_getDouble(handle, y); if( result != rcOk) return result;
		result = Registry_getDouble(handle, z); if( result != rcOk) return result;
		result = Registry_getDouble(handle, m); if( result != rcOk) return result;
		result = Registry_getDouble(handle, t); if( result != rcOk) return result;

		setX(x);
		setY(y);
		setZ(z);
		setM(m);

		return result;
	}
}