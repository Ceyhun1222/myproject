using PVT.Drawing.Symbols;
using PVT.Utils;

namespace PVT.Model.Drawing
{
    public class LineStyle: Style
    {

        public LineGeometry Geometry { get; set; }
        public int Width { get; set; }
        public Color Color { get; set; }

        private LineStyle(LineGeometry geom):this(geom, new Color(System.Drawing.Color.Black.GetColor()))
        {

        }

        private LineStyle(LineGeometry geom,  Color color)
        {
            Geometry = geom;
            Width = 1;
            Color = color;
        }

        public static LineStyle Solid { get {return new LineStyle(LineGeometry.Solid); } }
        public static LineStyle Dash { get { return new LineStyle(LineGeometry.Dash); } }
        public static LineStyle DashDot { get { return new LineStyle(LineGeometry.DashDot); } }
        public static LineStyle DashDotDot { get { return new LineStyle(LineGeometry.DashDotDot); } }

    }

    public class LineGeometry
    {
        public LineStyles Style { get; private set; }
        public System.Windows.Media.DashStyle DashStyle { get; private set; }

        private LineGeometry(LineStyles style, System.Windows.Media.DashStyle dashStyle)
        {
            Style = style;
            DashStyle = dashStyle;
        }

        static LineGeometry()
        {
            Solid = new LineGeometry(LineStyles.slsSolid, System.Windows.Media.DashStyles.Solid);
            Dash = new LineGeometry(LineStyles.slsDash, System.Windows.Media.DashStyles.Dash);
            DashDot = new LineGeometry(LineStyles.slsDashDot, System.Windows.Media.DashStyles.DashDot);
            DashDotDot = new LineGeometry(LineStyles.slsDashDotDot, System.Windows.Media.DashStyles.DashDotDot);
        }

        public static LineGeometry Solid { get; }
        public static LineGeometry Dash { get; }
        public static LineGeometry DashDot { get; }
        public static LineGeometry DashDotDot { get; }
    }
}
