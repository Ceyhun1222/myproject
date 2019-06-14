using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Aim.Data;
using Aran.Delta.Model;
using Aran.Delta.Properties;
using Aran.Delta.View;
using Aran.PANDA.Common;
using Aran.Aim.Features;

namespace Aran.Delta.ViewModels
{
    public class CalculatorViewModel : ViewModel
    {
        private int _resultPointCount;

        private int _leftResultPtHandle;
        private int _rightResultPtHandle;
        private int _line1Handle;
        private int _line2Handle;
        private int _line3Handle;
        private int _line4Handle;

        private Aran.Geometries.Point _leftSelectedPoint;
        private Aran.Geometries.Point _rightSelectedPoint;

        #region :>Ctor
      
        public CalculatorViewModel(Enums.CalcultaionType calculationType)
        {
            _calculationType = calculationType;
            if (_calculationType == Enums.CalcultaionType.CourseCourse)
                Title = "Course and Course";
            else if (_calculationType == Enums.CalcultaionType.CourseDistance)
                Title = "Course and Distance";
            else if (_calculationType == Enums.CalcultaionType.DistanceDistance)
                Title = "Distance and Distance";

            PointViewModel = new PointSelectViewModel(_calculationType);
            PointViewModel.PointSelectChanged += PointViewModel_PointSelectChanged;
            _leftSelectedPoint = PointViewModel.LeftSelectedPoint;
            _rightSelectedPoint = PointViewModel.RightSelectedPoint;

            IsDD = GlobalParams.Settings.DeltaInterface.CoordinateUnit == Settings.model.CoordinateUnitType.DD;
            ApplyCommand = new RelayCommand(new Action<object>(applyCommand_onclick));
            CloseCommand = new RelayCommand(new Action<object>(closeCommand_onclick));
            SaveCommand = new RelayCommand(new Action<object>(saveCommand_onClick));

            ResultLeftCoordContEnabled = false;
            ResultRightCoordContEnabled = false;
        }
        
        #endregion
      
        #region :>Property
        public int Resolution { get { return Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision); } }

        public string DistanceUnit { get { return InitDelta.DistanceConverter.Unit; } }

        public string Title { get; set; }

        private double _leftInverseAzimuth;
        public double LeftInverseAzimuth
        {
            get { return Common.ConvertAngle(_leftInverseAzimuth,RoundType.ToNearest); }
            set
            {
                _leftInverseAzimuth =Common.DeConvertAngle(value);
                NotifyPropertyChanged("LeftInverseAzimuth");
            }
        }

        private double _leftAzimuth;
        public double LeftAzimuth
        {
            get { return Common.ConvertAngle(_leftAzimuth,RoundType.ToNearest); }
            set
            {
                _leftAzimuth =Common.DeConvertAngle(value);
                SetRightControlsToolTip();
                NotifyPropertyChanged("LeftAzimuth");
            }
        }

        private double _leftDistance;
        public double LeftDistance
        {
            get
            {
                return Common.ConvertDistance(_leftDistance,RoundType.ToNearest); 
            }
            set
            {
                _leftDistance =Common.DeConvertDistance(value);
                SetMinMax();
                NotifyPropertyChanged("LeftDistance");
            }
        }

        private double _resultLeftLatitude;
        public double ResultLeftLatitude
        {
            get { return _resultLeftLatitude; }
            set 
            {
                _resultLeftLatitude = value;
                NotifyPropertyChanged("ResultLeftLatitude");
            }
        }

        private double _resultLeftLongtitude;
        public double ResultLeftLongtitude
        {
            get { return _resultLeftLongtitude; }
            set
            {
                _resultLeftLongtitude = value;
                NotifyPropertyChanged("ResultLeftLongtitude");
            }
        }

        private double _rightInverseAzimuth;
        public double RightInverseAzimuth
        {
            get { return Common.ConvertAngle(_rightInverseAzimuth, RoundType.ToNearest); }
            set
            {
                _rightInverseAzimuth =Common.DeConvertAngle(value);
                NotifyPropertyChanged("RightInverseAzimuth");
            }
        }

        private double _rightAzimuth;
        public double RightAzimuth
        {
            get { return Common.ConvertAngle(_rightAzimuth, RoundType.ToNearest); }
            set
            {
                _rightAzimuth =Common.DeConvertAngle(value);
                NotifyPropertyChanged("RightAzimuth");
            }
        }

        private double _rightDistance;
        public double RightDistance
        {
            get
            {
                return Common.ConvertDistance(_rightDistance, RoundType.ToNearest);
            }
            set
            {
                _rightDistance = Common.DeConvertDistance(value);
                NotifyPropertyChanged("RightDistance");
            }
        }


        private double _resultRightLatitude;
        public double ResultRightLatitude
        {
            get { return _resultRightLatitude; }
            set
            {
                _resultRightLatitude = value;
                NotifyPropertyChanged("ResultRightLatitude");
            }
        }

        private double _resultRightLongtitude;
        public double ResultRightLongtitude
        {
            get { return _resultRightLongtitude; }
            set
            {
                _resultRightLongtitude = value;
                NotifyPropertyChanged("ResultRightLongtitude");
            }
        }

        private bool _resultLeftCoordContEnabled;
        public bool ResultLeftCoordContEnabled
        {
            get { return _resultLeftCoordContEnabled; }
            set 
            {
                _resultLeftCoordContEnabled = value;
                NotifyPropertyChanged("ResultLeftCoordContEnabled");
            }
        }

        private bool _resultRightCoordContEnabled;
        public bool ResultRightCoordContEnabled
        {
            get { return _resultRightCoordContEnabled; }
            set
            {
                _resultRightCoordContEnabled = value;
                NotifyPropertyChanged("ResultRightCoordContEnabled");
            }
        }

        private bool _saveIsEnabled;
        private Enums.CalcultaionType _calculationType;
      
        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set 
            {
                _saveIsEnabled = value;
                NotifyPropertyChanged("SaveIsEnabled");
            }
        }

        private string _rightAzimuthToolTip;
        public string RightAzimuthToolTip
        {
            get { return _rightAzimuthToolTip; }
            set 
            {
                _rightAzimuthToolTip = value;
                NotifyPropertyChanged("RightAzimuthToolTip");
            }
        }

        private string _lefttAzimuthToolTip;
        public string LeftAzimuthToolTip
        {
            get { return _lefttAzimuthToolTip; }
            set 
            {
                _lefttAzimuthToolTip = value;
                NotifyPropertyChanged("LeftAzimuthToolTip");
            }
        }

        private string _rightDistanceToolTip;
        public string RightDistanceToolTip
        {
            get { return _rightDistanceToolTip; }
            set 
            {
                _rightDistanceToolTip = value;
                NotifyPropertyChanged("RightDistanceToolTip");
            }
        }

        private string _leftDistanceToolTip;

        public string LeftDistanceToolTip
        {
            get { return _leftDistanceToolTip; }
            set 
            {
                _leftDistanceToolTip = value;
                NotifyPropertyChanged("LeftDistanceToolTip");
            }
        }
        
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public PointSelectViewModel PointViewModel { get; set; }

        public bool IsDD { get; private set; }

        public bool DisplayGraphics { get; set; }

        private double _leftAccuracy;
        public double LeftAccuracy
        {
            get { return Math.Round(_leftAccuracy,1); }
            set { _leftAccuracy = value; }
        }

        private double _rightAccuracy;
        public double RightAccuracy
        {
            get { return Math.Round(_rightAccuracy,1); }
            set { _rightAccuracy = value; }
        }
        
        

        #endregion

        #region :>Methods
        void PointViewModel_PointSelectChanged(object sender, EventArgs e)
        {
            ClearResult();
            
            _leftSelectedPoint = PointViewModel.LeftSelectedPoint;
            _rightSelectedPoint = PointViewModel.RightSelectedPoint;
            if (_leftSelectedPoint == null || _rightSelectedPoint == null)
                return;
            
            if (_calculationType == Enums.CalcultaionType.DistanceDistance)
            {
                SetMinMax();
            }
            else if (_calculationType == Enums.CalcultaionType.CourseCourse || _calculationType == Enums.CalcultaionType.CourseDistance)
            {
                double directAzimuth, inverseAzimuth;
                var twoPointAzimuth = NativeMethods.ReturnGeodesicAzimuth(_leftSelectedPoint.X, _leftSelectedPoint.Y, _rightSelectedPoint.X, _rightSelectedPoint.Y, out directAzimuth, out inverseAzimuth);
                LeftAzimuthToolTip = Resources.azimuthTwoPoints +Math.Round(directAzimuth,1)+ " °";
                SetRightControlsToolTip();
            }
        }

        private void SetMinMax() 
        {
            _leftSelectedPoint = PointViewModel.LeftSelectedPoint;
            _rightSelectedPoint = PointViewModel.RightSelectedPoint;

            var twoPointDistance = NativeMethods.ReturnGeodesicDistance(_leftSelectedPoint.X, _leftSelectedPoint.Y, _rightSelectedPoint.X, _rightSelectedPoint.Y);
                LeftDistanceToolTip = Resources.distanceTwoPoints + " " + Common.ConvertDistance(twoPointDistance, RoundType.ToNearest) + DistanceUnit;

            RightDistanceToolTip = Resources.min + Common.ConvertDistance(Math.Abs(twoPointDistance - _leftDistance), RoundType.ToNearest) + DistanceUnit;
            if (_leftDistance <= twoPointDistance)
                RightDistanceToolTip += " "+Resources.max + Common.ConvertDistance(twoPointDistance + _leftDistance, RoundType.ToNearest) + DistanceUnit;
            
        }

        private void SetRightControlsToolTip()
        {
            double xRes = 0.0, yRes = 0, azimuthRes = 0;
            var dist = NativeMethods.DistFromPointToLine(_leftSelectedPoint.X, _leftSelectedPoint.Y, _rightSelectedPoint.X, _rightSelectedPoint.Y, _leftAzimuth, ref xRes, ref yRes, ref azimuthRes);

            if (_calculationType == Enums.CalcultaionType.CourseCourse)
            {
                double tmpAzimuth, tmpInverseAzimuth;
                NativeMethods.ReturnGeodesicAzimuth(_leftSelectedPoint.X, _leftSelectedPoint.Y, xRes, yRes, out tmpAzimuth, out tmpInverseAzimuth);
                RightAzimuthToolTip = "Azimuth to the nearest point on the line :" + Math.Round(tmpAzimuth, 1) + " °";
            }
            else if (_calculationType == Enums.CalcultaionType.CourseDistance)
            {
                RightDistanceToolTip = Resources.distanceToLine + Common.ConvertDistance(dist, RoundType.ToNearest) + " " + DistanceUnit;
            }

        }

        public void closeCommand_onclick(object obj)
        {
            Close();
        }

        private void applyCommand_onclick(object obj)
        {
            ClearResult();
            
            _leftSelectedPoint = PointViewModel.LeftSelectedPoint;
            _rightSelectedPoint = PointViewModel.RightSelectedPoint;
            
            double resX0 = 0, resY0 = 0, resX1 = 0, resY1 =0;
            SaveIsEnabled = true;
            
            if (_leftSelectedPoint != null && _rightSelectedPoint != null)
            {
                _resultPointCount = 0;

                if (_calculationType == Enums.CalcultaionType.CourseDistance)
                {
                    _resultPointCount = NativeMethods.CalcByCourseDistance(_leftSelectedPoint.X, _leftSelectedPoint.Y, _leftAzimuth, _rightSelectedPoint.X, _rightSelectedPoint.Y, _rightDistance, out resX0, out resY0, out resX1, out resY1);
                }
                else if (_calculationType == Enums.CalcultaionType.CourseCourse)
                {
                    _resultPointCount = NativeMethods.Calc2VectIntersect(_leftSelectedPoint.X, _leftSelectedPoint.Y, _leftAzimuth, _rightSelectedPoint.X, _rightSelectedPoint.Y, _rightAzimuth, out resX0, out resY0);
                }
                else if (_calculationType == Enums.CalcultaionType.DistanceDistance)
                {
                    _resultPointCount = NativeMethods.Calc2DistIntersects(_leftSelectedPoint.X, _leftSelectedPoint.Y, _leftDistance, _rightSelectedPoint.X, _rightSelectedPoint.Y, _rightDistance, out resX0, out resY0, out resX1, out resY1);
                }

                if (_resultPointCount == 0)
                {
                    SaveIsEnabled = false;
                    Messages.Warning("No Solution!");
                    return;
                }
                ResultLeftLatitude = resY0;
                ResultLeftLongtitude = resX0;

                //Calculation accuracy

                //var leftResultAccuracy = PointViewModel.CalcPointChangeAccuracy(ResultLeftLongtitude, ResultLeftLatitude);

                _leftAccuracy = _calculationType == Enums.CalcultaionType.DistanceDistance?CalcDistanceDistanceAccuracy(): CalcAccuracy(ResultLeftLongtitude, ResultLeftLatitude);

                //_leftAccuracy =Math.Sqrt( PointViewModel.LeftAccuracy *PointViewModel.LeftAccuracy+PointViewModel.RightAccuracy *PointViewModel.RightAccuracy+leftResultAccuracy *leftResultAccuracy);

                if (_calculationType == Enums.CalcultaionType.CourseDistance || _calculationType == Enums.CalcultaionType.CourseCourse)
                {
                    _leftDistance =NativeMethods.ReturnGeodesicDistance(_leftSelectedPoint.X, _leftSelectedPoint.Y, resX0, resY0);

                    double directAzimuth, inverseAzimuth;
                    NativeMethods.ReturnGeodesicAzimuth(_leftSelectedPoint.X, _leftSelectedPoint.Y, resX0, resY0, out directAzimuth, out inverseAzimuth);
                    LeftInverseAzimuth = inverseAzimuth;

                    _rightDistance = NativeMethods.ReturnGeodesicDistance(_rightSelectedPoint.X, _rightSelectedPoint.Y, resX0, resY0);

                    NativeMethods.ReturnGeodesicAzimuth(_rightSelectedPoint.X, _rightSelectedPoint.Y, resX0, resY0, out directAzimuth, out inverseAzimuth);
                    RightInverseAzimuth = inverseAzimuth;
                }
                
                if (DisplayGraphics)
                    DrawLeftResult();
                
                if (_resultPointCount == 2)
                {
                    ResultRightLatitude = resY1;
                    ResultRightLongtitude = resX1;

                    _rightAccuracy = _calculationType == Enums.CalcultaionType.DistanceDistance ? CalcDistanceDistanceAccuracy() : CalcAccuracy(ResultRightLongtitude, ResultRightLatitude);
                    
                    //CalcAccuracy(ResultRightLongtitude,ResultRightLatitude);
                    //var rightResultAccuracy = PointViewModel.CalcPointChangeAccuracy(ResultRightLongtitude, ResultRightLatitude);

                    //_rightAccuracy =Math.Sqrt(PointViewModel.RightAccuracy *PointViewModel.RightAccuracy+PointViewModel.LeftAccuracy *PointViewModel.LeftAccuracy+rightResultAccuracy *rightResultAccuracy);

                    if (DisplayGraphics)
                        DrawRightResult();
                }
                NotifyPropertyChanged("");
            }
        }

        private void SaveAngleAndDistanceIndication(DesignatedPoint dPoint,Note note)
        {
            if (PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.DesignatedPoint || PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.Navaid)
            {
                GlobalParams.Database.DeltaQPI.ClearAllFeatures();

                var questionIndication = Messages.Question("Do you want to save Angle and Distance Indication!");
                if (questionIndication != System.Windows.MessageBoxResult.Yes)
                    return;

                var angleIndication = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.AngleIndication>();
                angleIndication.Annotation.Add(note);
                angleIndication.Angle = RightAzimuth;
                angleIndication.AngleType = Aim.Enums.CodeBearing.TRUE;
                angleIndication.Fix = new Aim.DataTypes.FeatureRef(dPoint.Identifier);
                angleIndication.PointChoice = new Aim.Features.SignificantPoint();
                if (PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.DesignatedPoint || PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.Navaid)
                    angleIndication.PointChoice.FixDesignatedPoint = new Aim.DataTypes.FeatureRef(PointViewModel.LeftPointModel.Feat.Identifier);

                var distanceIndication = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.DistanceIndication>();
                distanceIndication.Annotation.Add(note);

                distanceIndication.Distance = new Aim.DataTypes.ValDistance();
                distanceIndication.Distance.Value = LeftDistance;
                if (InitDelta.DistanceConverter.Unit == "km")
                    distanceIndication.Distance.Uom = Aim.Enums.UomDistance.KM;
                else
                    distanceIndication.Distance.Uom = Aim.Enums.UomDistance.NM;

                distanceIndication.Fix = new Aim.DataTypes.FeatureRef(dPoint.Identifier);

                distanceIndication.PointChoice = new Aim.Features.SignificantPoint();
                if (PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.DesignatedPoint || PointViewModel.LeftSelectedPointChoise == Enums.PointChoiceType.Navaid)
                    angleIndication.PointChoice.FixDesignatedPoint = new Aim.DataTypes.FeatureRef(PointViewModel.LeftPointModel.Feat.Identifier);



                GlobalParams.Database.DeltaQPI.SetFeature(angleIndication);
                GlobalParams.Database.DeltaQPI.SetFeature(distanceIndication);
                GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.AngleIndication);
                GlobalParams.Database.DeltaQPI.Commit();
            }

        }

        private void saveCommand_onClick(object obj)
        {
            if (_resultPointCount == 0)
                return;

            var note = new Note();
            note.Purpose = Aim.Enums.CodeNotePurpose.REMARK;
            var linguisticNote = new LinguisticNote();
            linguisticNote.Note = new Aim.DataTypes.TextNote();
            var noteText = "Has created by Delta!";
            linguisticNote.Note.Value = noteText;
            note.TranslatedNote.Add(linguisticNote);

            GlobalParams.Database.DeltaQPI.ClearAllFeatures();
            for (int i = 0; i < _resultPointCount; i++)
            {
                var questionResult = Messages.Question("Do you want to save Result Point " + (i+1).ToString());
                if (questionResult != System.Windows.MessageBoxResult.Yes)
                    continue;

                if (GlobalParams.AranEnv != null)
                {
                    var dPoint = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.DesignatedPoint>();
                    dPoint.Annotation.Add(note);

                    dPoint.Location = new Aim.Features.AixmPoint();
                    dPoint.Location.Geo.X = _resultLeftLongtitude;
                    dPoint.Location.Geo.Y = _resultLeftLatitude;
                    dPoint.Location.HorizontalAccuracy = new Aim.DataTypes.ValDistance();
                    dPoint.Location.HorizontalAccuracy.Uom = Aim.Enums.UomDistance.M;
                    dPoint.Location.HorizontalAccuracy.Value = LeftAccuracy;
                    if (i == 1)
                    {
                        dPoint.Location.Geo.X = _resultRightLongtitude;
                        dPoint.Location.Geo.Y = _resultRightLatitude;
                        dPoint.Location.HorizontalAccuracy.Value = RightAccuracy ;
                    }
                    
                    GlobalParams.Database.DeltaQPI.SetFeature(dPoint);
                    GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.DesignatedPoint);


                    var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;

                    if (dbProvider != null)
                    {
                        GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                        //GlobalParams.Database.DeltaQPI.Commit();
                        GlobalParams.Database.DeltaQPI.ExcludeFeature(dPoint.Identifier);
                    }

                    SaveAngleAndDistanceIndication(dPoint, note);
                }
                else
                {
                    
                    var designingPoint = new Model.DesigningPoint();
                    Aran.Geometries.Point savePt = null;
                    if (i == 1)
                        savePt = new Aran.Geometries.Point { X = _resultRightLongtitude, Y = _resultRightLatitude };
                    else
                        savePt = new Aran.Geometries.Point { X = _resultLeftLongtitude, Y = ResultLeftLatitude };
                    savePt.Z = 0;
                    savePt.M = 0;
                    designingPoint.Geo = savePt;
                   // string longStr, latStr;

                    designingPoint.Lon = ARANFunctions.Degree2String(savePt.X, Degree2StringMode.DMSLon,
                    Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

                    designingPoint.Lat = ARANFunctions.Degree2String(savePt.Y, Degree2StringMode.DMSLat,
                     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

					//Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(savePt.X, savePt.Y, ".", "E", "N", 1, Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out longStr, out latStr);
					//designingPoint.Lat = latStr;
					//designingPoint.Lon = longStr;

					if (GlobalParams.DesigningAreaReader.SavePoint(designingPoint))
                    {
                        Messages.Info("Feature saved database successfully");
                        Clear();
                        Functions.SaveArenaProject();
                    }
                }
            }
        }

        private void SaveToArena() 
        {
            var designingPoint = new Model.DesigningPoint();
            designingPoint.Geo = new Aran.Geometries.Point { X = _resultLeftLongtitude, Y = ResultLeftLatitude };
            if (GlobalParams.DesigningAreaReader.SavePoint(designingPoint))
                Messages.Info("Feature saved database successfully");
        
        }

        public void DrawLeftResult()
        {
            GlobalParams.UI.SafeDeleteGraphic(_leftResultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_line1Handle);
            GlobalParams.UI.SafeDeleteGraphic(_line2Handle);

            var resultPt = new Aran.Geometries.Point(_resultLeftLongtitude, _resultLeftLatitude);
            _leftResultPtHandle = GlobalParams.UI.DrawPointWithText(GlobalParams.SpatialRefOperation.ToPrj(resultPt),"pt1");

            Aran.Geometries.LineString tmpLine1 = new Geometries.LineString();
            tmpLine1.Add(resultPt);
            tmpLine1.Add(_leftSelectedPoint);
            _line1Handle = GlobalParams.UI.DrawMultiLineString(new Aran.Geometries.MultiLineString { tmpLine1 }, GlobalParams.Settings.SymbolModel.LineCourseSymbol);

            Aran.Geometries.LineString tmpLine2 = new Geometries.LineString();
            tmpLine2.Add(resultPt);
            tmpLine2.Add(_rightSelectedPoint);
            _line2Handle = GlobalParams.UI.DrawMultiLineString(new Aran.Geometries.MultiLineString { tmpLine2 }, GlobalParams.Settings.SymbolModel.LineCourseSymbol);
        }

        public void DrawRightResult()
        {
            GlobalParams.UI.SafeDeleteGraphic(_rightResultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_line3Handle);
            GlobalParams.UI.SafeDeleteGraphic(_line4Handle);

            var resultPt = new Aran.Geometries.Point(_resultRightLongtitude, _resultRightLatitude);
            _rightResultPtHandle = GlobalParams.UI.DrawPointWithText(GlobalParams.SpatialRefOperation.ToPrj(resultPt),"pt2");

            Aran.Geometries.LineString tmpLine1 = new Geometries.LineString();
            tmpLine1.Add(resultPt);
            tmpLine1.Add(_leftSelectedPoint);
            _line3Handle = GlobalParams.UI.DrawMultiLineString(new Aran.Geometries.MultiLineString { tmpLine1 }, GlobalParams.Settings.SymbolModel.LineCourseSymbol);

            Aran.Geometries.LineString tmpLine2 = new Geometries.LineString();
            tmpLine2.Add(resultPt);
            tmpLine2.Add(_rightSelectedPoint);
            _line4Handle = GlobalParams.UI.DrawMultiLineString(new Aran.Geometries.MultiLineString { tmpLine2 }, GlobalParams.Settings.SymbolModel.LineCourseSymbol);
        }


        public void ClearResult()
        {
            SaveIsEnabled = false;
            GlobalParams.UI.SafeDeleteGraphic(_leftResultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_rightResultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_line1Handle);
            GlobalParams.UI.SafeDeleteGraphic(_line2Handle);
            GlobalParams.UI.SafeDeleteGraphic(_line3Handle);
            GlobalParams.UI.SafeDeleteGraphic(_line4Handle);
        }

        public void Clear()
        {
            PointViewModel.Clear();
            ClearResult();
        }

        private double CalcAccuracy(double longtitude, double latitude)
        {
            var horAccuracy = new HorizontalAccuracyCalculator();
            var ptResult = GlobalParams.SpatialRefOperation.ToPrj(new Geometries.Point(longtitude, latitude));

            var ptNav1 = new NavaidType { GeoPrj = GlobalParams.SpatialRefOperation.ToPrj(_leftSelectedPoint) };
            ptNav1.TypeCode = _calculationType == Enums.CalcultaionType.DistanceDistance ? eNavaidType.DME : eNavaidType.VOR;
            // TODO accuracy for madrid
            //ptNav1.HorAccuracy = PointViewModel.LeftPointModel.Accuracy;// _leftAccuracy;
            ptNav1.HorAccuracy = PointViewModel.LeftAccuracy;// _leftAccuracy;

            var ptNav2 = new NavaidType { GeoPrj = GlobalParams.SpatialRefOperation.ToPrj(_rightSelectedPoint) };
            // TODO accuracy for madrid
            ptNav2.TypeCode = _calculationType == Enums.CalcultaionType.CourseCourse ? eNavaidType.VOR : eNavaidType.DME;
            //ptNav2.HorAccuracy = PointViewModel.RightPointModel.Accuracy;// _rightAccuracy;
            ptNav2.HorAccuracy = PointViewModel.RightAccuracy;// _rightAccuracy;

            var accuracy = horAccuracy.CalcHorisontalAccuracy(ptResult, ptNav1, ptNav2);
            return accuracy;

            //if (_resul)
            //var ptLeft = new Geometries.Point(ResultLeftLongtitude, ResultLeftLatitude);
            //var ptNav1 = new NavaidType { GeoPrj = _leftSelectedPoint };
            //ptNav1.TypeCode = _calculationType == Enums.CalcultaionType.CourseDistance ? eNavaidType.DME : eNavaidType.VOR;
            //ptNav1.HorAccuracy = _leftAccuracy;
        }

        private double CalcDistanceDistanceAccuracy()
        {
            //var L = GetDistance();//

            var L = ARANFunctions.ReturnGeodesicDistance(_leftSelectedPoint, _rightSelectedPoint);

            var D1 = _leftDistance;
            var D2 = _rightDistance;

            var dL = (L * L - D1 * D1 + D2 * D2) / (2 * L * L);

            var dD1 = _leftDistance / L;
            var dD2 = -_rightDistance / L;
            // TODO accuracy for madrid
            //var SD12 = PointViewModel.LeftPointModel.Accuracy; // Error Null for new created point
            var SD1 = PointViewModel.LeftAccuracy;
            //var SD21 = PointViewModel.RightPointModel.Accuracy; // Error Null for new created point
            var SD2 = PointViewModel.RightAccuracy;

            var Sx = Math.Sqrt(dD1 * dD1 * SD1 * SD1 + dD2 * dD2 * SD2 * SD2 + dL * dL * (SD1 * SD1 + SD2 * SD2));


            var yD1 = D1 * D1 / (D1 * D1 - Sx * Sx);

            var X3= (D1 * D1 - D2 * D2 + L * L) / (2 * L);

            var Dy1 = D1 * D1 / (D1 * D1 - X3 * X3);

            var Dy2 = D2 * D2 / (D2 * D2 - (L - X3) * (L - X3));

            var Sy = Math.Sqrt(Dy1 + Dy2);

            var Q = Math.Sqrt(Sx * Sx + Sy * Sy);

            return Q;
        }

        #endregion
    }


}
