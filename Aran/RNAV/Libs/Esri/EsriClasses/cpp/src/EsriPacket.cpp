#include "../include/EsriPacket.h"
#include <Panda/Geometry/cpp/include/GeometryType.h>
#include <unknwn.h>

namespace Esri
{
      int ToPandaGeometryType(esriGeometryType geomType)
        {
                switch(geomType)
                {
                        case esriGeometryPoint:
                                return Panda::GeometryType::Point;
                        case esriGeometryMultipoint:
                                return Panda::GeometryType::MultiPoint;
                        case esriGeometryPath:
                                return Panda::GeometryType::Part;
                        case esriGeometryRing:
                                return Panda::GeometryType::Ring;
                        case esriGeometryPolyline:
                                return Panda::GeometryType::Polyline;
                        case esriGeometryPolygon:
                                return Panda::GeometryType::Polygon;
                }
                return 0;
        }

        int packGeometry(Handle handle, Esri::Geometry* geom)
        {
                int result;

                if( geom != 0)
                {
                    result = Registry_putInt32 ( handle, ToPandaGeometryType(geom->getType()));
                    if( result != rcOk) return result;
                    result = geom->pack(handle);
                }
                else
                    result = Registry_putInt32 ( handle, Panda::GeometryType::Null);

                return result;
        }

        int unpackGeometry(Handle handle, Esri::Geometry*& geom)
        {
                int geomType = 0;
                int result = Registry_getInt32 (handle, geomType); if( result != rcOk) return result;
                GUID guid;
                guid.Data1 = -1;

                switch( geomType)
                {
                    case Panda::GeometryType::Point:
                        guid = CLSID_Point;
                        break;
                    case Panda::GeometryType::MultiPoint:
                        guid = CLSID_Multipoint;
                        break;
                    case Panda::GeometryType::Part:
                        guid = CLSID_Path;
                        break;
                    case Panda::GeometryType::Ring:
                        guid = CLSID_Ring;
                        break;
                    case Panda::GeometryType::Polyline:
                        guid = CLSID_Polyline;
                        break;
                    case Panda::GeometryType::Polygon:
                        guid = CLSID_Polygon;
                        break;
                }

                if( guid.Data1 != -1)
                    geom = Esri::Geometry::create(guid);

                if( geom != 0)
                    (geom)->unpack(handle);

                return result;
        }
}