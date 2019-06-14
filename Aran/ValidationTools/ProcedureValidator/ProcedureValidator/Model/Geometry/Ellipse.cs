namespace PVT.Model.Geometry
{
    public class Ellipse : AnalyticGeometry2D
    {
        private double _radiusX;
        private double _radiusY;
        public Point2D Centre { get; set; }
        public double RadiusX
        {
            get { return _radiusX; }
            set { if (value < 0) _radiusX = 0; else _radiusX = value; }
        }
        public double RadiusY
        {
            get { return _radiusY; }
            set { if (value < 0) _radiusY = 0; else _radiusY = value; }
        }

        public override Geometry3D Transform()
        {
            return new Ellipse3D{ Centre = (Point3D)Centre.Transform(), RadiusX = RadiusX, RadiusY = RadiusY, Angle = new Vector3D()};
        }
    }

    public class Ellipse3D : AnalyticGeometry3D
    {
        private double _radiusX;
        private double _radiusY;


        public Point3D Centre { get; set; }
        public double RadiusX
        {
            get { return _radiusX; }
            set { if (value < 0) _radiusX = 0; else _radiusX = value; }
        }
        public double RadiusY
        {
            get { return _radiusY; }
            set { if (value < 0) _radiusY = 0; else _radiusY = value; }
        }

        public Vector3D Angle { get; set; }

        public override Geometry2D Transform()
        {
            return new Ellipse { Centre = (Point2D)Centre.Transform(), RadiusX = RadiusX, RadiusY = RadiusY};
        }
    }
}
