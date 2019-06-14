using GalaSoft.MvvmLight;

namespace PVT.Model.Drawing
{
    public class Styles: ObservableObject
    {
        public Styles()
        {
            Enabled = true;
            _pointStyle = PointStyle.Circle;
            _lineStyle = LineStyle.Solid;
        }

        public Styles(PointStyle style)
        {
            Enabled = true;
            _pointStyle = style;
            _lineStyle = LineStyle.Solid;
        }

        public Styles(LineStyle style)
        {
            Enabled = true;
            _lineStyle = style;
            _pointStyle = PointStyle.Circle;
        }

        public Styles(PointStyle pointStyle, LineStyle lineStyle)
        {
            Enabled = true;
            _lineStyle = lineStyle;
            _pointStyle = pointStyle;
        }

        private PointStyle _pointStyle;
        public PointStyle PointStyle
        {
            get
            {
                return _pointStyle;
            }
            set
            {
                Set(() => PointStyle, ref _pointStyle, value);
            }
        }

        private LineStyle _lineStyle;
        public LineStyle LineStyle
        {
            get
            {
                return _lineStyle;
            }
            set
            {
                Set(() => LineStyle, ref _lineStyle, value);
            }
        }

        public bool Enabled { get; set; }

    }

    public class AreaStyles: Styles
    {
        public AreaStyles()
        {

        }

        public AreaStyles(PointStyle style):base(style)
        {
        }

        public AreaStyles(LineStyle style):base(style)
        {
        }

        public AreaStyles(PointStyle pointStyle, LineStyle lineStyle):base(pointStyle, lineStyle)
        {
        }

        private Styles _obstacleStyle;
        public Styles ObstacleStyle
        {
            get
            {
                return _obstacleStyle;
            }
            set
            {
                Set(() => ObstacleStyle, ref _obstacleStyle, value);
            }
        }

    }
}
