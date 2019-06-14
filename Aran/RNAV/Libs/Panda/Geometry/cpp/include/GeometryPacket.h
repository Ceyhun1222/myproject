#ifndef GEOMETRY_PACKET_H
#define GEOMETRY_PACKET_H

#include "Geometry.h"

namespace Panda
{
	void packGeometry (Handle handle, const Geometry& geom) throw (Registry::Exception);
	const Geometry& unpackGeometry(Handle handle) throw (Registry::Exception);
}

#endif //GEOMETRY_PACKET_H
