using System.Collections.Generic;
using System.Windows.Media;

namespace PVT.Model.Geometry
{
    public class PolyLine2D: PointGeometry2D
    {
        public PolyLine2D()
        {
            _points = new List<Point2D>();
        }

        private readonly List<Point2D> _points;

        public override List<Point2D> Points
        {
            get
            {
                return _points;
            }
        }

        public override Geometry3D Transform()
        {
            var polyLine3D = new PolyLine3D();
            foreach (var t in _points)
                polyLine3D.Points.Add((Point3D) t.Transform());
            return polyLine3D;
        }

        public  override System.Windows.Media.Geometry Convert()
        {
            if (_points.Count == 0)
                return null;

            var points = new PointCollection();
            for (var i = 1; i < _points.Count; i++)
            {
                points.Add(new System.Windows.Point(_points[i].X, _points[i].Y));
            }

            var segment = new PolyLineSegment {Points = points};
            var segmetColletion = new PathSegmentCollection {segment};

            var figure = new PathFigure(new System.Windows.Point(_points[0].X, _points[0].Y), segmetColletion, false);
            var figureCollection = new PathFigureCollection {figure};

            var geom = new PathGeometry(figureCollection);
            return geom;
            
        }
    }

    public class PolyLine3D: PointGeometry3D
    {
        public PolyLine3D()
        {
            _points = new List<Point3D>();
        }

        private readonly List<Point3D>  _points;

        public override List<Point3D> Points
        {
            get
            {
                return _points;
            }
        }

        public override Geometry2D Transform()
        {
            var polyLine = new PolyLine2D();
            foreach (var t in _points)
            {
                polyLine.Points.Add((Point2D)t.Transform());
            }
            return polyLine;
        }
    }
}
