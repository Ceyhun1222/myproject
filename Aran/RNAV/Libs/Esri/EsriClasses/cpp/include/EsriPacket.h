#ifndef ESRI_PACKET_H
#define ESRI_PACKET_H

#include "EsriGeometry.h"

namespace Esri
{
    int ToPandaGeometryType(esriGeometryType geomType);
    int packGeometry(Handle handle, Esri::Geometry* geom);
    int unpackGeometry(Handle handle, Esri::Geometry*& geom);
}


#endif //ESRI_PACKET_H