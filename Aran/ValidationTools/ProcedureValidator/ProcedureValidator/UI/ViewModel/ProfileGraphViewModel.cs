using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using PVT.Model.Geometry;
using PVT.Model.Plot;
using PVT.Model.Transformations;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProfileGraphViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ProfileGraphViewModel class.
        /// </summary>
        /// 

        public ProcedureBase Procedure { get; set; }
        public PlotModel<Geometry3D> PlotModel { get; set; }
        public ProfileGraphViewModel(ProcedureBase procedure)
        {
            Procedure = procedure;
            PlotModel = new PlotModel<Geometry3D>();
            //var endPoint = RunwayDirection.CentreLinePoints[RunwayCentreLinePointType.End].Projection;
            //var startPoint = RunwayDirection.CentreLinePoints[RunwayCentreLinePointType.Start].Projection;
            //var dX = endPoint.X - startPoint.X;
            //var dY = endPoint.Y - startPoint.Y;
            //var length = System.Math.Sqrt(dX * dX + dY * dY);
            //var cosA = dX / length;
            //var sinA = dY / length;

            //var localCentreX = (startPoint.X + endPoint.X) / 2;
            //var localCentreY = (startPoint.Y + endPoint.Y) / 2;
            //var centreX = localCentreX - sinA * length;
            //var centreY = localCentreY + cosA * length;
            //var angle = (centreY - localCentreY) / (centreX - localCentreY);

            //Point3D centre = new Point3D(0, 10, 0);


            //PlotModel.TransformFunction = new XZTransformation(PlotModel, centre, -System.Math.PI / 2);

            //PolyLine3D polyLine = new PolyLine3D();
            //if (Procedure.Original.FinalProfile != null)
            //{
            //    var altitudes = Procedure.Original.FinalProfile.Altitude;
            //    var distances = Procedure.Original.FinalProfile.Distance;

            //    double x = 0;
            //    for (int i = 0; i < altitudes.Count; i++)
            //    {
            //        if (i == 0)
            //            x = 0;
            //        else
            //            x = x + Aran.Converters.ConverterToSI.Convert(distances[i - 1].Distance, 0);

            //        polyLine.Points.Add(new Point3D(x, 0, Aran.Converters.ConverterToSI.Convert(altitudes[i].Altitude, 0)));
            //    }
             
            //    PlotModel.AddGeometry(polyLine);
            //}
            //SetPoints(polyLine, Transition.IntermediateLegs);
            //SetPoints(polyLine, Transition.FinalLegs);
            //SetPoints(polyLine, Transition.MissedApproachLegs);
            // PlotModel.AddGeometry(new Line3D { Start = new Point3D(startPoint.X, startPoint.Y, startPoint.Z), End = new Point3D(endPoint.X, endPoint.Y, endPoint.Z) });

        }

        //private static void SetPoints(PolyLine3D polyLine, List<TransitionLeg> legs)
        //{
        //    for (int i = 0; i < legs.Count; i++)
        //    {
        //        SetPoints(polyLine, legs[i]);
        //    }
        //}

        //private static void SetPoints(PolyLine3D polyLine, TransitionLeg leg)
        //{
        //    if(!leg.IsEmpty && leg.SegmentLeg != null)
        //    {
        //        if (leg.SegmentLeg.StartPoint != null)
        //            SetPoint(polyLine, leg.SegmentLeg.StartPoint, leg.SegmentLeg.Original.UpperLimitAltitude.Value);
        //        if (leg.SegmentLeg.EndPoint != null)
        //            SetPoint(polyLine, leg.SegmentLeg.EndPoint, leg.SegmentLeg.Original.LowerLimitAltitude.Value);
        //    }
        //}

        //private static void SetPoint(PolyLine3D polyLine, TerminalSegmentPoint point, double z)
        //{
        //    if (point !=null && point.PointChoice != null && !point.PointChoice.IsEmpty)
        //        polyLine.Points.Add(new Point3D(point.PointChoice.X, point.PointChoice.Y, z));
        //}

        private void AddAngle(double angle)
        {
            var transform = PlotModel.TransformFunction as XZTransformation;
            transform.Angle += angle;
            transform.Refresh();
            PlotModel.Refresh();
        }

        private RelayCommand _leftCommand;

        public RelayCommand LeftCommand
        {
            get
            {
                return _leftCommand
                    ?? (_leftCommand = new RelayCommand(
                    () =>
                    {
                        AddAngle(1 * (System.Math.PI / 180.0));
                    }));
            }
        }

        private RelayCommand _rightCommand;

        public RelayCommand RightCommand
        {
            get
            {
                return _rightCommand
                    ?? (_rightCommand = new RelayCommand(
                    () =>
                    {
                        AddAngle(-1 * (System.Math.PI / 180.0));
                    }));
            }
        }
    }
}