using Aran.AranEnvironment;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using GalaSoft.MvvmLight;
using System;
using Aran.Aim.Enums;

namespace Aran.AimEnvironment.Tools
{
    class AngleMeasure : ObservableObject
    {
        private static AngleMeasure _current;

        public static AngleMeasure Current
        {
            get { return _current ?? (_current = new AngleMeasure()); }
        }

        private IPoint _firstPoint;
        private IPoint _secondPoint;
        private IPoint _thirdPoint;
        private IPoint _snappedPoint;

        private readonly INewLineFeedback _lineFeedback;

        private int _mouseCounter;
        private bool _snapped;
        private bool _measureStarted;

        public double Epsillon { get; } = 0.0001;

        private HorizantalDistanceType _distanceType = HorizantalDistanceType.KM;

        public HorizantalDistanceType DistanceType
        {
            get
            {
                return _distanceType;
            }
            set
            {
                Set(() => DistanceType, ref _distanceType, value);
                SetRaduis();
                SetArc();
            }
        }

        private double _azimuthAngle;

        public double AzimuthAngle
        {
            get
            {
                return _azimuthAngle;
            }
            set
            {
                Set(() => AzimuthAngle, ref _azimuthAngle, value);
            }
        }

        private double _angle;

        public double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                Set(() => Angle, ref _angle, value);
            }
        }

        private double _azimuthCourse1;

        public double AzimuthCourse1
        {
            get
            {
                return _azimuthCourse1;
            }
            set
            {
                Set(() => AzimuthCourse1, ref _azimuthCourse1, value);
            }
        }

        private double _course1;

        public double Course1
        {
            get
            {
                return _course1;
            }
            set
            {
                Set(() => Course1, ref _course1, value);
            }
        }

        private double _azimuthCourse2;

        public double AzimuthCourse2
        {
            get
            {
                return _azimuthCourse2;
            }
            set
            {
                Set(() => AzimuthCourse2, ref _azimuthCourse2, value);
            }
        }

        private double _course2;

        public double Course2
        {
            get
            {
                return _course2;
            }
            set
            {
                Set(() => Course2, ref _course2, value);
            }
        }

        public double ArcInM { get; set; }

        private double _arc;

        public double Arc
        {
            get
            {
                return _arc;
            }
            set
            {
                Set(() => Arc, ref _arc, value);
            }
        }

        public double RadiusInM { get; set; }

        private double _radius;

        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                Set(() => Radius, ref _radius, value);
            }
        }

        private AngleMeasure()
        {
            _lineFeedback = new NewLineFeedback { Display = Context.ActiveView.ScreenDisplay };
        }

        public void OnMouseClickedOnMap(object sender, MapMouseEventArg e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var latitude = e.Y;
                var longtitude = e.X;

                if (_mouseCounter < 4)
                    _mouseCounter++;
                else {
                    Stop();
                    _mouseCounter = 1;
                }

                switch (_mouseCounter)
                {
                    case 1:
                        _firstPoint = FixPoint(latitude, longtitude);
                        _measureStarted = true;
                        _lineFeedback.Start(_firstPoint);
                        break;
                    case 2:
                        _secondPoint = FixPoint(latitude, longtitude);
                        _lineFeedback.AddPoint(_secondPoint);
                        SetCourse1(_firstPoint, _secondPoint);
                        SetRadiusInM(_firstPoint, _secondPoint);
                        break;
                    case 3:
                        _thirdPoint = FixPoint(latitude, longtitude);
                        _mouseCounter++;
                        _measureStarted = false;
                        SetCourse1(_firstPoint, _secondPoint);
                        SetCourse2(_secondPoint, _thirdPoint);
                        SetAngle();
                        SetArcInM();
                        break;
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                Stop();
        }



        public void OnMouseMoveOnMap(object sender, MapMouseEventArg e)
        {
            IPoint point;

            double latitude = e.Y;
            double longtitude = e.X;

			

			point = CreatePoint(latitude, longtitude);

            Snap(point);

            if (_snapped)
            {
                point = _snappedPoint;
            }

			if ( !_measureStarted )
				return;
			_lineFeedback.MoveTo(point);

			
			if (_mouseCounter == 1)
            {
                SetCourse1(_firstPoint, point);
            }
            if (_mouseCounter == 2)
            {
                SetCourse2(_secondPoint, point);
                SetAngle();
            }
        }

        public void Stop()
        {
            Clean();
            _lineFeedback.Stop();
        }

        public void Clean()
        {
            _measureStarted = false;
            _mouseCounter = 0;
            Course1 = 0;
            Course2 = 0;
            AzimuthCourse1 = 0;
            AzimuthCourse2 = 0;
            Angle = 0;
            AzimuthAngle = 0;
            Radius = 0;
            Arc = 0;
        }

        private void SetCourse1(IPoint point1, IPoint point2)
        {
            var angle = CalcCourse(point1, point2);
            Course1 = Math.Round(angle, 2);
            AzimuthCourse1 = DirToAzimuth(point2, angle);
        }

        private void SetRadiusInM(IPoint point1, IPoint point2)
        {
            RadiusInM = ARANFunctions.ReturnDistanceInMeters(new Geometries.Point(point1.X, point1.Y),
                new Geometries.Point(point2.X, point2.Y));
            SetRaduis();
        }

        private void SetRaduis()
        {
            Radius = Convert(RadiusInM);
        }

        private void SetArcInM()
        {
            ArcInM = RadiusInM*Math.PI*2*(Angle/360);
            SetArc();
        }


        private void SetArc()
        {
            Arc = Convert(ArcInM);
        }


        private double Convert(double valueInM)
        {
            if (Math.Abs(valueInM) < Epsillon)
              return 0;

            double result = 0;
            if (DistanceType == HorizantalDistanceType.KM)
                result = Converters.ConverterFromSI.Convert(UomDistance.KM, valueInM, 0);
            if (DistanceType == HorizantalDistanceType.NM)
                result = Converters.ConverterFromSI.Convert(UomDistance.NM, valueInM, 0);
            return Math.Round(result, 3);
        }


        private void SetCourse2(IPoint point1, IPoint point2)
        {
            var angle = CalcCourse(point1, point2);
            Course2 = Math.Round(angle, 2);
            AzimuthCourse2 = DirToAzimuth(point2, angle);
        }

        private double DirToAzimuth(IPoint point2, double angle)
        {
            return Math.Round(ARANFunctions.DirToAzimuth(new Aran.Geometries.Point(point2.X, point2.Y), ARANMath.DegToRad(angle), Context.SpatialReferenceOperation.SpRefPrj, Context.SpatialReferenceOperation.SpRefGeo), 2);
        }

        private double CalcCourse(IPoint point1, IPoint point2)
        {
            var angle = ARANFunctions.ReturnAngleInDegrees(new Aran.Geometries.Point(point1.X, point1.Y), new Aran.Geometries.Point(point2.X, point2.Y));
            if (angle < 0)
                angle = 360 + angle;
            if (Math.Abs(Math.Round(angle) - 360) < Epsillon)
                angle = 0;
            return angle;
        }

        private void SetAngle()
        {
            Angle = Math.Round(CalcAngle(Course1, ARANMath.Modulus(180 + Course2)), 2);
            AzimuthAngle = Math.Round(CalcAngle(AzimuthCourse1, ARANMath.Modulus(180 + AzimuthCourse2)), 2);
        }

        private double CalcAngle(double course1, double course2)
        {
            double angle;
            if (course2 > course1)
                angle = 360 - course2 + course1;
            else
                angle = 360 - course1 + course2;

            if (angle > 180)
                angle = Math.Abs(course2 - course1);
            return angle;
        }

        private IPoint FixPoint(double latitude, double longtitude)
        {
            if (_snapped)
            {
                return _snappedPoint;
            }
            else
            {
                return CreatePoint(latitude, longtitude);
            }
        }
        private IPoint CreatePoint(double latitude, double longtitude)
        {
            var point = new Point
            {
                X = longtitude,
                Y = latitude
            };
            return point;
        }

        private void Snap(IPoint point)
        {
            var snapResult = SnapClass.Current.Snapper.Snap(point);

            SnapClass.Current.SnappingFeedback.Update(snapResult, 0);

            if (snapResult != null)
            {//Snapping occurred

                //Set the current position to the snapped location
                _snapped = true;
                _snappedPoint = snapResult.Location;
            }
            else
            {
                _snapped = false;
            }
        }
    }
}

