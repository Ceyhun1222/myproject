
using System.Collections.Generic;
using System.Windows.Media;
using PVT.Utils;
using System.Windows;
using PVT.Model.Geometry;
using PVT.Model.Transformations;

namespace PVT.Model.Plot
{
    public class PlotBase
    {

        public Point Centre { get; protected set; }
        public PlotType PlotType { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public double ActualHeight {
            get {
                return Height - Margin.Bottom - Margin.Top;
            }
        }
        public double ActualWidth
        {
            get
            {
                return Width - Margin.Left - Margin.Right;
            }
        }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public Margin Margin { get; set; }
        public Box Box { get; }
        public PlotOriginType PlotOriginType { get; set; }
        public List<Axis> Axes { get; }
        public Color Color { get; set; }
        public int Thickness { get; set; }
        public Color FillingStyle { get; set; }

        protected double BottomBoundary {
            get
            {
                return Margin.Bottom + OffsetX;
            }
        }

        public delegate void PlotModelChangedHandler();
        public event PlotModelChangedHandler PlotModelChanged;

        public PlotBase()
        {
            PlotType = PlotType.XY;
            Axes = new List<Axis>();
            Color = System.Drawing.Color.Black.GetColor();
            PlotOriginType = PlotOriginType.LeftBottom;
            OffsetX = 10;
            OffsetY = 10;
            Margin = new Margin(1);
            Box = new Box(this);
            Thickness = 2;

            Axes.Add(new Axis { Parent = this, Position = AxisPostion.Horizontal });
            Axes.Add(new Axis { Parent = this, Position = AxisPostion.Vertical});

        }


        protected virtual void OnPlotModelChanded()
        {
            if (PlotModelChanged != null)
                PlotModelChanged();
        }
    }

    public class PlotModel<T>: PlotBase where T : Geometry.Geometry 
    {
        private readonly List<T> geometries;
        public List<PlotGeometry> DrawGeometries { get; }
        private readonly List<Geometry2D> transformedGeometries;
        public ITransformation<T> TransformFunction { get; set; }
        public ScaleTransformation ScaleTransformation { get; }

        public PlotModel()
        {
            geometries = new List<T>();
            DrawGeometries = new List<PlotGeometry>();
            transformedGeometries = new List<Geometry2D>();
            ScaleTransformation = new ScaleTransformation(1, 1);
        }

        public void SetSize(double width, double height)
        {
            Height = height;
            Width = width;
            ReCalculate();
        }

        public void AddGeometry(T geom)
        {
            geometries.Add(geom);
            Refresh();
        }

        public void AddGeometry(IList<T> geom)
        {
            for (var i = 0; i < geom.Count; i++)
            {
                geometries.Add(geom[i]);
            }
            Refresh();
        }
        public void Refresh()
        {
            ReCalculate();
            OnPlotModelChanded();
        }

        public void PlotSizeChanged(SizeChangedEventArgs e)
        {
            Height = e.NewSize.Height;
            Width = e.NewSize.Width;
            ReCalculate();
            OnPlotModelChanded();
        }
    
        private void ReCalculate()
        {
            CalculateCentre();
            DrawGeometries.Clear();
            CreateAxes();
            CreateGeometries();
        }
        private void CalculateCentre()
        {
            double x = 0;
            double y = 0;
            switch (PlotOriginType)
            {
                case PlotOriginType.Center:
                    x = ActualWidth / 2 + Box.Left + OffsetX;
                    y = ActualHeight / 2 + Box.Top + OffsetY;
                    break;
                case PlotOriginType.LeftCenter:
                    x = Box.Left+ OffsetX;
                    y = ActualHeight / 2 + Box.Top + OffsetY;
                    break;
                case PlotOriginType.RightCenter:
                    x = Box.Right - OffsetX;
                    y = ActualHeight / 2 + Box.Top + OffsetY;
                    break;
                case PlotOriginType.Bottom:
                    x = ActualWidth / 2 + Box.Left + OffsetX;
                    y = Box.Bottom - OffsetY;
                    break;
                case PlotOriginType.LeftBottom:
                    x = Box.Left+ OffsetX;
                    y = Box.Bottom - OffsetY;
                    break;
                case PlotOriginType.RightBottom:
                    x = Box.Right - OffsetX;
                    y = Box.Bottom - OffsetY;
                    break;
                case PlotOriginType.Top:
                    x = ActualWidth / 2 + Box.Left + OffsetX;
                    y = Box.Top + OffsetY;
                    break;
                case PlotOriginType.LeftTop:
                    x = Box.Left + OffsetX;
                    y = Box.Top + OffsetY;
                    break;
                case PlotOriginType.RightTop:
                    x = Box.Right - OffsetX;
                    y = Box.Top + OffsetY;
                    break;
            }
            Centre = new Point(x, y);
        }
        private void CreateGeometries()
        {
            if (TransformFunction != null)
            {
                transformedGeometries.Clear();
                for (var i = 0; i < geometries.Count; i++)
                    transformedGeometries.Add(TransformFunction.Transform(geometries[i]));

                CalcScale();

                for (var i = 0; i < transformedGeometries.Count; i++)
                {
                    var plotGeometry = new PlotGeometry { Color = Color, Geometry = Convert(ScaleTransformation.Transform(transformedGeometries[i])), Parent = this, Thickness = Thickness };
                    DrawGeometries.Add(plotGeometry);
                }
            }
        }

        private void CalcScale()
        {
            Geometry.Box box = null;
            if (transformedGeometries.Count == 0)
                return;
            for (var i = 0; i < transformedGeometries.Count ; i++)
            {
                if (i == 0)
                    box = transformedGeometries[i].Box;
                else
                    box = Geometry.Box.GetBox(box, transformedGeometries[i].Box);
            }

            ScaleTransformation.ScaleY = box.Max.Y - box.Min.Y < 10 ? 1: 0.5*System.Math.Abs((Box.Top - Box.Bottom) / (box.Max.Y - box.Min.Y));
            ScaleTransformation.ScaleX = box.Max.X - box.Min.X < 10 ? 1 : 0.5*System.Math.Abs((Box.Right - Box.Left) / (box.Max.X - box.Min.X));
        }


        private void CreateAxes()
        {
            foreach (var t in Axes)
                DrawAxis(t);
        }

        private void DrawAxis(Axis axis)
        {
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            var plotGeometry = new PlotGeometry { Color = Color, Thickness = Thickness };
            switch (axis.Position)
            {
                case AxisPostion.Horizontal:
                    x1 = Box.Left;
                    y1 = y2 = Centre.Y;
                    x2 = Box.Right;
                    break;
                case AxisPostion.Vertical:
                    y2 = Box.Bottom;
                    x1 = x2 = Centre.X;
                    y1 = Box.Top;
                    break;
            }

            plotGeometry.Geometry = new Line2D { Start = new Point2D(x1, y1), End = new Point2D(x2, y2) };
            DrawGeometries.Add(plotGeometry);
        }
        private Geometry2D Convert(Geometry2D geom)
        {
            if (geom.GetType() == typeof(Point2D))
                return Convert((Point2D)geom);
            if (geom.GetType() == typeof(Line2D))
                return Convert((Line2D)geom);
            if (geom.GetType() == typeof(PolyLine2D))
                return Convert((PolyLine2D)geom);

            return null;
        }
        private Point2D Convert(Point2D point)
        {
            double x;
            double y;

            if (PlotOriginType == PlotOriginType.LeftBottom || PlotOriginType == PlotOriginType.RightBottom || PlotOriginType == PlotOriginType.Bottom)
            {
                y = Centre.Y - point.Y;
            } else y = Centre.Y + point.Y;

            if (PlotOriginType == PlotOriginType.RightBottom || PlotOriginType == PlotOriginType.RightCenter|| PlotOriginType == PlotOriginType.RightTop)
            {
                x = Centre.X - point.X;
            }
            else x = Centre.X + point.X;

            return new Point2D(x, y);
        }
        private Line2D Convert(Line2D line)
        {
            return new Line2D {Start = Convert(line.Start), End = Convert(line.End)};
        }
        private PolyLine2D Convert(PolyLine2D polyline)
        {
            var result = new PolyLine2D();
            for (var i = 0; i < polyline.Points.Count; i++)
            {
                result.Points.Add(Convert(polyline.Points[i]));
            }
            return result;
        }

    }

    public enum PlotType
    {
        XY,
        Cartesian,
        Polar
    }

    public enum PlotOriginType
    {
        Center,
        LeftCenter,
        RightCenter,
        Bottom,
        LeftBottom,
        RightBottom,
        Top,
        LeftTop,
        RightTop,
    }
}
