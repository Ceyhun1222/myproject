using System;
using PVT.Engine.Common.Geometry;

namespace PVT.Engine.CDOTMA.Geometry
{
    class Geometry : CommonGeometry, IGeometry
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public T ToGeo<T>(T prjGeom) where T : Aran.Geometries.Geometry
        {
            throw new NotImplementedException();
        }

        public T ToPrj<T>(T geoGeom) where T : Aran.Geometries.Geometry
        {
            throw new NotImplementedException();
        }
    }
}
