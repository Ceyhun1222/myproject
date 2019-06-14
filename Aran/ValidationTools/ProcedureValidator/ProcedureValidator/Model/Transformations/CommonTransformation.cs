using PVT.Model.Geometry;

namespace PVT.Model.Transformations
{
    public abstract class CommonTransformation2D : ITransformation<Geometry2D>
    {
        public Geometry2D Transform(Geometry2D geom)
        {
            if (geom.GetType() == typeof(Point2D))
                return Transform((Point2D)geom);
            if (geom.GetType() == typeof(Line2D))
                return Transform((Line2D)geom);
            if (geom.GetType() == typeof(PolyLine2D))
                return Transform((PolyLine2D)geom);

            throw new TransformationException("Unknown geometry type");
        }

        protected abstract Point2D Transform(Point2D point2D);

        private Line2D Transform(Line2D line2d)
        {
            var line = new Line2D();
            line.Start = Transform(line2d.Start);
            line.End = Transform(line2d.End);
            return line;
        }

        private PolyLine2D Transform(PolyLine2D polyline2d)
        {
            var polyline = new PolyLine2D();
            for (var i = 0; i < polyline2d.Points.Count; i++)
            {
                polyline.Points.Add(Transform(polyline2d.Points[i]));
            }
            return polyline;
        }
    }

    public abstract class CommonTransformation3D : ITransformation<Geometry3D>
    {
        public Geometry2D Transform(Geometry3D geom)
        {
            if (geom.GetType() == typeof(Point3D))
                return Transform((Point3D)geom);
            if (geom.GetType() == typeof(Line3D))
                return Transform((Line3D)geom);
            if (geom.GetType() == typeof(PolyLine3D))
                return Transform((PolyLine3D)geom);

            throw new TransformationException("Unknown geometry type");
        }

        protected abstract Point2D Transform(Point3D point3D);

        private Line2D Transform(Line3D line3d)
        {
            var line = new Line2D();
            line.Start = Transform(line3d.Start);
            line.End = Transform(line3d.End);
            return line;
        }

        private PolyLine2D Transform(PolyLine3D polyline3d)
        {
            var polyline = new PolyLine2D();
            for (var i = 0; i < polyline3d.Points.Count; i++)
            {
                polyline.Points.Add(Transform(polyline3d.Points[i]));
            }
            return polyline;
        }
    }
}
