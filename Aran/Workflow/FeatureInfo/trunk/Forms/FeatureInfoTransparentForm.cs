using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;

namespace Aran.Aim.FeatureInfo
{
    internal partial class FeatureInfoTransparentForm : Form
    {
        internal EventHandler CurrentFeatureChanged;

        private Pen _linePen;
        private Brush _brush;
        private Point [] _trianglePoints;
        private Point _footHold;
        private bool _captionMouseDown;
        private Point _startPoint;

        public FeatureInfoTransparentForm ()
        {
            InitializeComponent ();

            BackColor = Color.Magenta;

            _linePen = new Pen (Brushes.Black, 1);
            _brush = SystemBrushes.Control;
            _trianglePoints = new Point [3];

            #region Events
            ui_mainPanel.LocationChanged += MainPanel_LocationChanged;
            ui_featureContainerCont.CloseClicked += Close_Click;
            ui_featureContainerCont.TopMouseDown += Caption_MouseDown;
            ui_featureContainerCont.TopMouseUp += Caption_MouseUp;
            ui_featureContainerCont.TopMouseMove += Caption_MouseMove;
            ui_featureContainerCont.PropertyChanged += FeatureContainer_PropertyChanged;
            #endregion


            if (Global.IsOSVErsionXP)
                System.Windows.Media.RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
        }

        private void FeatureContainer_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentFeature")
            {
                if (CurrentFeatureChanged != null)
                    CurrentFeatureChanged (ui_featureContainerCont.CurrentFeaure, null);
            }
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);

            if (!_trianglePoints [0].IsEmpty)
            {
                Graphics gr = e.Graphics;

                gr.FillPolygon (_brush, _trianglePoints);
                gr.DrawPolygon (_linePen, _trianglePoints);
            }
        }

        public Point FootHold
        {
            get { return _footHold; }
            set
            {
                _footHold = PointToClient (value);

                DrawTriangle ();
            }
        }

        public void SetFeatures (IEnumerable<Feature> featureList)
        {
            ui_featureContainerCont.SetFeature (featureList);
        }


        private void DrawTriangle ()
        {
            Point centrePoint = new Point (
                ui_mainPanel.Left + ui_mainPanel.Width / 2,
                ui_mainPanel.Top + ui_mainPanel.Height / 2);

            double d = GetDistance (centrePoint, _footHold);
            double m = d * Math.Sin (DegToRad (5));
            if (m < 35)
                m = 35;
            double angle = ReturnAngleInDegrees (centrePoint, _footHold);
            
            PointF ptF = PointAlongPlane (centrePoint, angle - 90.0, m);
            Point pt1 = new Point ((int) ptF.X, (int) ptF.Y);

            ptF = PointAlongPlane (centrePoint, angle + 90.0, m);
            Point pt2 = new Point ((int) ptF.X, (int) ptF.Y);

            _trianglePoints [0] = _footHold;
            _trianglePoints [1] = pt1;
            _trianglePoints [2] = pt2;

            Refresh ();
        }

        private void Close_Click (object sender, EventArgs e)
        {
            Hide ();
        }

        private void MainPanel_LocationChanged (object sender, EventArgs e)
        {
            DrawTriangle ();
        }

        private void Caption_MouseDown (object sender, MouseEventArgs e)
        {
            _captionMouseDown = true;
            _startPoint = e.Location;
        }

        private void Caption_MouseUp (object sender, MouseEventArgs e)
        {
            _captionMouseDown = false;
        }

        private void Caption_MouseMove (object sender, MouseEventArgs e)
        {
            if (!_captionMouseDown)
                return;

            Point pt = e.Location;
            Point offset = new Point (pt.X - _startPoint.X, pt.Y - _startPoint.Y);

            pt = ui_mainPanel.Location;
            ui_mainPanel.Location = new Point (pt.X + offset.X, pt.Y + offset.Y);
        }        

        #region Calc Functions

        internal static double GetDistance (PointF point1, PointF point2)
        {
            double a = (double) (point2.X - point1.X);
            double b = (double) (point2.Y - point1.Y);

            return Math.Sqrt (a * a + b * b);
        }

        internal static PointF PointAlongPlane (PointF fromPoint, double dirAngle, double distance)
        {
            dirAngle = DegToRad (dirAngle);

            PointF res = new PointF ();
            res.X = (float) (fromPoint.X + distance * Math.Cos (dirAngle));
            res.Y = (float) (fromPoint.Y + distance * Math.Sin (dirAngle));
            return res;
        }

        internal static double ReturnAngleInDegrees (Point fromPoint, Point toPoint)
        {
            double fdX = toPoint.X - fromPoint.X;
            double fdY = toPoint.Y - fromPoint.Y;
            return Modulus (RadToDeg (Math.Atan2 (fdY, fdX)), 360.0);
        }

        internal static double DegToRad (double value)
        {
            return value * DegToRadValue;
        }

        internal static double RadToDeg (double value)
        {
            return value * RadToDegValue;
        }

        internal static double Modulus (double value, double divisor)
        {
            long q = (long) Math.Floor (value / divisor);
            return value - q * divisor;
        }

        internal const double DegToRadValue = Math.PI / 180.0;
        internal const double RadToDegValue = 180.0 / Math.PI;

        #endregion
    }
}
