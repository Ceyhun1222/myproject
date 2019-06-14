using System.Windows.Media;
using PVT.Drawing.Symbols;
using PVT.Utils;

namespace PVT.Model.Drawing
{
    public class PointStyle:Style
    {
        public PointGeometry Geometry { get;  set; }
        public int Size { get;  set; }
        public Color Color { get; set; }

        private PointStyle(PointGeometry geom):this(geom, new Color(System.Drawing.Color.Black.GetColor()))
        {
        }

        private PointStyle(PointGeometry geom, Color color)
        {
            Geometry = geom;
            Size = 1;
            Color = color;
        }

        public static PointStyle Circle { get { return new PointStyle(PointGeometry.Circle); } }
        public static PointStyle Cross { get { return new PointStyle(PointGeometry.Cross); } }
        public static PointStyle Diamond { get { return new PointStyle(PointGeometry.Diamond); } }
        public static PointStyle Square { get { return new PointStyle(PointGeometry.Square); } }
        public static PointStyle X { get { return new PointStyle(PointGeometry.X); } }

    }

    public class PointGeometry
    {
        public PointStyles Style { get; private set; }
        public System.Windows.Media.Geometry Geometry { get; private set; }

        private PointGeometry(PointStyles style)
        {
            Style = style;
        }

        static PointGeometry()
        {
            CreateDiamond();
            CreateCircle();
            CreateCross();
            CreateX();
            CreateSquare();
        }



        public static PointGeometry Circle { get; private set; }
        public static PointGeometry Cross { get; private set; }
        public static PointGeometry Diamond { get; private set; }
        public static PointGeometry Square { get; private set; }
        public static PointGeometry X { get; private set; }


        private static void CreateCircle()
        {
            Circle = new PointGeometry(PointStyles.smsCircle)
            {
                Geometry = new EllipseGeometry(new System.Windows.Point(6, 6), 6, 6)
            };
        }

        private static void CreateDiamond()
        {
            Diamond = new PointGeometry(PointStyles.smsDiamond) {Geometry = new PathGeometry()};
            var figures = new PathFigureCollection();

            var figure = new PathFigure {StartPoint = new System.Windows.Point(0, 6)};
            var segmentCollection = new PathSegmentCollection
            {
                new LineSegment(new System.Windows.Point(6, 0), true),
                new LineSegment(new System.Windows.Point(12, 6), true),
                new LineSegment(new System.Windows.Point(6, 12), true),
                new LineSegment(new System.Windows.Point(0, 6), true)
            };


            figure.Segments = segmentCollection;
            figures.Add(figure);

            ((PathGeometry)Diamond.Geometry).Figures = figures;
        }

        private static void CreateSquare()
        {
            Square = new PointGeometry(PointStyles.smsSquare) {Geometry = new PathGeometry()};
            var figures = new PathFigureCollection();

            var figure = new PathFigure {StartPoint = new System.Windows.Point(0, 0)};
            var segmentCollection = new PathSegmentCollection
            {
                new LineSegment(new System.Windows.Point(12, 0), true),
                new LineSegment(new System.Windows.Point(12, 12), true),
                new LineSegment(new System.Windows.Point(0, 12), true),
                new LineSegment(new System.Windows.Point(0, 0), true)
            };


            figure.Segments = segmentCollection;
            figures.Add(figure);

            ((PathGeometry)Square.Geometry).Figures = figures;
        }

        private static void CreateCross()
        {
            Cross = new PointGeometry(PointStyles.smsCross) {Geometry = new PathGeometry()};
            var figures = new PathFigureCollection();

            var horizontalLine = new PathFigure {StartPoint = new System.Windows.Point(6, 0)};
            var hSegmentCollection = new PathSegmentCollection {new LineSegment(new System.Windows.Point(6, 12), true)};

            var verticalLine = new PathFigure {StartPoint = new System.Windows.Point(0, 6)};
            var vSegmentCollection = new PathSegmentCollection {new LineSegment(new System.Windows.Point(12, 6), true)};

            horizontalLine.Segments = hSegmentCollection;
            verticalLine.Segments = vSegmentCollection;

            figures.Add(horizontalLine);
            figures.Add(verticalLine);

            ((PathGeometry)Cross.Geometry).Figures = figures;
        }

        private static void CreateX()
        {
            X = new PointGeometry(PointStyles.smsX) {Geometry = new PathGeometry()};
            var figures = new PathFigureCollection();

            var horizontalLine = new PathFigure {StartPoint = new System.Windows.Point(0, 0)};
            var hSegmentCollection = new PathSegmentCollection {new LineSegment(new System.Windows.Point(12, 12), true)};

            var verticalLine = new PathFigure {StartPoint = new System.Windows.Point(0, 12)};
            var vSegmentCollection = new PathSegmentCollection {new LineSegment(new System.Windows.Point(12, 0), true)};

            horizontalLine.Segments = hSegmentCollection;
            verticalLine.Segments = vSegmentCollection;

            figures.Add(horizontalLine);
            figures.Add(verticalLine);

            ((PathGeometry)X.Geometry).Figures = figures;
        }
    }
}
