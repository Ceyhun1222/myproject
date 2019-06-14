#ifndef ESRI_PATH_H
#define ESRI_PATH_H

#include "EsriGeometry.h"

namespace Esri
{
    class Path : public IPath
    {
	public:
		static Path* create ();

		Geometry* asGeometry(); 

		int pack(Handle handle);
		int unpack(Handle handle);
    };
}

#endif /*ESRI_PATH_H*/