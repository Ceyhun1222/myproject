#include "../include/GeometryPacket.h"
#include "../include/GeometryType.h"
#include "../include/Point.h"
#include "../include/MultiPoint.h"
#include "../include/Part.h"
#include "../include/Ring.h"
#include "../include/Polyline.h"
#include "../include/Polygon.h"
#include "../include/NullGeometry.h"

namespace Panda
{
	void packGeometry (Handle handle, const Geometry& geom) throw (Registry::Exception)
    {
		Registry::putInt32 (handle, geom.geometryType());
		geom.pack(handle);
    }
    
	const Geometry& unpackGeometry(Handle handle) throw (Registry::Exception)
	{
		//static Geometry* geom = 0;
		Geometry	*geom;
		int			geomType;
		Registry::getInt32(handle, geomType);

		//delete geom;

		switch( geomType)
		{
			case GeometryType::Null:
				geom = new NullGeometry();
				break;
			case GeometryType::Point:
				geom = new Point();
				break;
			case GeometryType::MultiPoint:
				geom = new MultiPoint();
				break;
			case GeometryType::Part:
				geom = new Part();
				break;
			case GeometryType::Ring:
				geom = new Ring();
				break;
			case GeometryType::Polyline:
				geom = new Polyline();
				break;
			case GeometryType::Polygon:
				geom = new Polygon();
				break;

			default:
				Registry::throwError (rcInvalid);
		}

		geom->unpack(handle);
		return *geom;
	}
}
