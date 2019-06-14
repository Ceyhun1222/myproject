namespace PVT.Engine.Common.Geometry
{
    public interface IGeometry
    {
        void Init();
        T ToPrj<T>(T geoGeom) where T : Aran.Geometries.Geometry;
        T ToGeo<T>(T prjGeom) where T : Aran.Geometries.Geometry;
    }
}
