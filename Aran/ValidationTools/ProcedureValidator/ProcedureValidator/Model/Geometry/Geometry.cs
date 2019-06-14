using System.Collections.Generic;

namespace PVT.Model.Geometry
{
    public class Geometry
    {

    }

    public abstract class Geometry2D : Geometry, IGeometryTransform<Geometry3D>
    {
        public abstract Geometry3D Transform();
        public virtual System.Windows.Media.Geometry Convert()
        {
            return null;
        }

        public Box Box {
            get {
                var geom = this as PointGeometry2D;
                if (geom == null)
                    return null;
                else return Point2D.GetBox(geom.Points);
            }
        }

    }

    public abstract class Geometry3D : Geometry, IGeometryTransform<Geometry2D>
    {
        public abstract Geometry2D Transform();

        public Box3D Box
        {
            get
            {
                var geom = this as PointGeometry3D;
                if (geom == null)
                    return null;
                else return Point3D.GetBox(geom.Points);
            }
        }
    }

    public abstract class PointGeometry2D:Geometry2D 
    {
        public abstract List<Point2D> Points { get; }
    }

    public abstract class AnalyticGeometry2D : Geometry2D
    {

    }

    public abstract class PointGeometry3D : Geometry3D
    {
        public abstract List<Point3D> Points { get; }
    }

    public abstract class AnalyticGeometry3D : Geometry3D
    {

    }


    public interface IGeometryTransform<T> where T : Geometry 
    {
        T Transform();
    }
}
