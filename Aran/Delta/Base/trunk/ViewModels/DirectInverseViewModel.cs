using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Aran.Delta.Model;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;
using Aran.Delta.View;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using Aran.Aim.Data;
using PDM;
using Aran.Aim.Features;

namespace Aran.Delta.ViewModels
{
    public class DirectInverseViewModel:ViewModel
    {
        private int _resultPtHandle;
        private int _line1Handle;
        private int _labelHandle;

        private Aran.Geometries.Point _leftSelectedPoint;
        private Aran.Geometries.Point _rightSelectedPoint;
        private bool _createSegment;
        private Geometries.LineString _segment;

        public PointSelectViewModel PointViewModel { get; set; }

        #region :>Ctor

        public DirectInverseViewModel(bool createSegment)
        {
            _createSegment = createSegment;

            Title = "Direct/Inverse";

            PointViewModel = new PointSelectViewModel(Enums.CalcultaionType.DirectInverse);
            PointViewModel.PointSelectChanged += PointViewModel_PointSelectChanged;
            IsDD = GlobalParams.Settings.DeltaInterface.CoordinateUnit == Settings.model.CoordinateUnitType.DD;
            ApplyCommand = new RelayCommand(new Action<object>(applyCommand_onclick));
            CloseCommand = new RelayCommand(new Action<object>(closeCommand_onclick));

            LabelGraphicsIsEnabled = false;
            if (createSegment)
            {
                 SaveCommand = new RelayCommand(new Action<object>(SaveSegment));
                EndPointIsChecked = true;
                DistInverseIsChecked = false;
                Title = "Create Segment";
                SaveCommandText = "Ok";
                DirectInverseIsVisible =Visibility.Collapsed;
            }
            else
            {
                SaveCommand = new RelayCommand(new Action<object>(saveCommand_onClick));
                EndPointIsChecked = false;
                DistInverseIsChecked = true;
                SaveCommandText = "Ok";
                DirectInverseIsVisible = Visibility.Visible;
            }
            
            //PointViewModel.Clear();
           // PointViewModel.LeftSelectedPoint = PointViewModel.LeftSelectedPoint;
        }

        
        
        #endregion
      
        #region :>Property
        public string DistanceUnit { get { return InitDelta.DistanceConverter.Unit; } }

        public string Title { get; set; }
        public string SaveCommandText { get; set; }

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
                NotifyPropertyChanged("LeftDistance");
            }
        }

        private bool _leftDistanceIsEnabled;

        public bool LeftDistanceIsEnabled
        {
            get { return _leftDistanceIsEnabled; }
            set 
            {
                _leftDistanceIsEnabled = value;
                NotifyPropertyChanged("LeftDistanceIsEnabled");
            }
        }

        private double _resultLatitude;
        public double ResultLatitude
        {
            get { return _resultLatitude; }
            set 
            {
                _resultLatitude = value;
                NotifyPropertyChanged("ResultLeftLatitude");
            }
        }

        private double _resultLongtitude;
        public double ResultLongtitude
        {
            get { return _resultLongtitude; }
            set
            {
                _resultLongtitude = value;
                NotifyPropertyChanged("ResultLeftLongtitude");
            }
        }

        private double _rightInverseAzimuth;
        public double RightInverseAzimuth
        {
            get { return Common.ConvertAngle(_rightInverseAzimuth, RoundType.ToNearest); }
            set
            {
                _rightInverseAzimuth = Common.DeConvertAngle(value);
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

        private bool _rightAzimuthIsEnabled;

        public bool RightAzimuthIsEnabled
        {
            get { return _rightAzimuthIsEnabled; }
            set
            {
                _rightAzimuthIsEnabled = value;
                NotifyPropertyChanged("RightAzimuthIsEnabled");
            }
        }
      
        private bool _saveIsEnabled;
      
        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set 
            {
                _saveIsEnabled = value;
                NotifyPropertyChanged("SaveIsEnabled");
            }
        }
        
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public bool IsDD { get; private set; }

        private bool _displayGraphics;
        public bool DisplayGraphics 
        {
            get { return _displayGraphics; }
            set 
            {
                _displayGraphics = value;
                LabelGraphicsIsEnabled = value;
                
                NotifyPropertyChanged("LabelGraphicsIsEnabled");
                NotifyPropertyChanged("DisplayGraphics");
            }
        }

        public bool LabelGraphics { get; set; }

        public bool LabelGraphicsIsEnabled { get; set; }

        public Visibility DirectInverseIsVisible { get; set; }

        private double _accuracy;
        public double Accuracy
        {
            get { return Math.Round(_accuracy,1); }
            set 
            {
                _accuracy = value;
                NotifyPropertyChanged("Accuracy");
            }
        }
        

        #endregion

        #region :>Methods
        void PointViewModel_PointSelectChanged(object sender, EventArgs e)
        {
            ClearResult();
        }

        public void closeCommand_onclick(object obj)
        {
            Close();
        }

        private void applyCommand_onclick(object obj)
        {
            try
            {
                ClearResult();
                _leftSelectedPoint = PointViewModel.LeftSelectedPoint;
                _rightSelectedPoint = PointViewModel.RightSelectedPoint;

                double resX = 0, resY = 0;
                

                if (_leftSelectedPoint != null && _rightSelectedPoint != null)
                {
                    if (DistInverseIsChecked)
                    {
                        NativeMethods.PointAlongGeodesic(_leftSelectedPoint.X, _leftSelectedPoint.Y, _leftDistance, _rightAzimuth, out resX, out resY);
                        NativeMethods.ReturnGeodesicAzimuth(_leftSelectedPoint.X, _leftSelectedPoint.Y, resX, resY, out _rightAzimuth, out _rightInverseAzimuth);
                        ResultLatitude = resY;
                        ResultLongtitude = resX;
                        //PointViewModel.RightLatitude = resY;
                        // PointViewModel.RightLongtitude = resX;
                        PointViewModel.RightSelectedPoint = new Geometries.Point(resX, resY);

                        int coordPrecision = Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision);

                        //var resultAccuracy = PointViewModel.CalcPointChangeAccuracy(ResultLongtitude, ResultLatitude);
                        //_accuracy =Math.Sqrt(PointViewModel.LeftAccuracy *PointViewModel.LeftAccuracy +resultAccuracy*resultAccuracy);
                        _accuracy = CalcHorisontalAccuracy();
                        SaveIsEnabled = true;
                    }
                    else
                    {
                        NativeMethods.ReturnGeodesicAzimuth(_leftSelectedPoint.X, _leftSelectedPoint.Y, _rightSelectedPoint.X, _rightSelectedPoint.Y, out _rightAzimuth, out _rightInverseAzimuth);

                        _leftDistance = NativeMethods.ReturnGeodesicDistance(_leftSelectedPoint.X, _leftSelectedPoint.Y, _rightSelectedPoint.X, _rightSelectedPoint.Y);

                        _accuracy = Math.Sqrt(PointViewModel.LeftAccuracy *PointViewModel.LeftAccuracy +PointViewModel.RightAccuracy*PointViewModel.RightAccuracy);

                        

                        if (_createSegment)
                            SaveIsEnabled = true;
                        else
                            SaveIsEnabled = false;
                    }


                    if (DisplayGraphics)
                        DrawResult();

                    NotifyPropertyChanged("");
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnv.GetLogger("Delta").Error(e);
                Messages.Error(e.Message.ToString());
            }
        }

        private void saveCommand_onClick(object obj)
        {
            try
            {

                var questionResult = Messages.Question("Do you want to save Result Point!");
                if (questionResult != System.Windows.MessageBoxResult.Yes)
                    return;

                if (GlobalParams.AranEnv != null)
                {
                    GlobalParams.Database.DeltaQPI.ClearAllFeatures();
                    var dPoint = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.DesignatedPoint>();

                    var note = new Note();
                    note.Purpose = Aim.Enums.CodeNotePurpose.REMARK;
                    var linguisticNote = new LinguisticNote();
                    linguisticNote.Note = new Aim.DataTypes.TextNote();
                    var noteText = "Has created by Delta!";
                    linguisticNote.Note.Value = noteText;
                    note.TranslatedNote.Add(linguisticNote);
                    dPoint.Annotation.Add(note);

                    dPoint.Location = new Aim.Features.AixmPoint();
                    dPoint.Location.Geo.X = _resultLongtitude;
                    dPoint.Location.Geo.Y = _resultLatitude;
                    dPoint.Location.HorizontalAccuracy = new Aim.DataTypes.ValDistance();
                    dPoint.Location.HorizontalAccuracy.Uom = Aim.Enums.UomDistance.M;
                    dPoint.Location.HorizontalAccuracy.Value = Accuracy;

                   
                    GlobalParams.Database.DeltaQPI.SetFeature(dPoint);
                    GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.DesignatedPoint);

                    var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;
                    if (dbProvider == null) return;
                    GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                    GlobalParams.Database.DeltaQPI.ExcludeFeature(dPoint.Identifier);

                    //Create Angleindigation
                    return;

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
                else
                {

                    var designingPoint = new Model.DesigningPoint();
                    Aran.Geometries.Point savePt = null;
                        savePt = new Aran.Geometries.Point { X = _resultLongtitude, Y = _resultLatitude };
                    savePt.Z = 0;
                    savePt.M = 0;
                    designingPoint.Geo = savePt;


                    designingPoint.Lon = ARANFunctions.Degree2String(savePt.X, Degree2StringMode.DMSLon,
                   Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

                    designingPoint.Lat = ARANFunctions.Degree2String(savePt.Y, Degree2StringMode.DMSLat,
                     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

					//string longStr, latStr;
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void SaveSegment(object obj)
        {
            var questionResult = Messages.Question("Do you want to save Result Point!");
            if (questionResult != System.Windows.MessageBoxResult.Yes)
                return;

            if (GlobalParams.AranEnv != null)
            {
                var rSegment = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.RouteSegment>();
                //dPoint.Location = new Aim.Features.AixmPoint();
                //dPoint.Location.Geo.X = _resultLongtitude;
                //dPoint.Location.Geo.Y = _resultLatitude;

                //GlobalParams.Database.DeltaQPI.SetFeature(dPoint);
                //GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.DesignatedPoint);
                //GlobalParams.Database.DeltaQPI.Commit();
            }
            else
            {
                var pdmObject = new PDM.RouteSegment();
                _leftSelectedPoint.Z = 0;
                _leftSelectedPoint.M = 0;

                _rightSelectedPoint.Z = 0;
                _rightSelectedPoint.M = 0;

                pdmObject.Geo = Aran.Converters.ConvertToEsriGeom.FromGeometry(GlobalParams.SpatialRefOperation.ToGeo(_segment),true);

                var window = new SavePoint(pdmObject);

                var helper = new WindowInteropHelper(window);
                helper.Owner = new IntPtr(GlobalParams.HWND);
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.ShowDialog();
            }

        }

        public void DrawResult()
        {
            GlobalParams.UI.SafeDeleteGraphic(_resultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_line1Handle);
            GlobalParams.UI.SafeDeleteGraphic(_labelHandle);

            Aran.Geometries.Point resultPt;
            if (DistInverseIsChecked)
            {

                resultPt = new Aran.Geometries.Point(_resultLongtitude, _resultLatitude);
                _resultPtHandle = GlobalParams.UI.DrawPoint(resultPt, GlobalParams.Settings.SymbolModel.ResultPointSymbol);
            }
            else
                resultPt = _rightSelectedPoint;

            _segment = new Geometries.LineString();
            _segment.Add(_leftSelectedPoint);
            _segment.Add(resultPt);


            _line1Handle = GlobalParams.UI.DrawMultiLineString(new Aran.Geometries.MultiLineString { _segment }, GlobalParams.Settings.SymbolModel.LineDistanceSymbol);

            if (LabelGraphics)
            {
                string lineText = LeftDistance + " " + InitDelta.DistanceConverter.Unit + "  " + RightAzimuth + "˚";
                double d = GlobalParams.SpatialRefOperation.DirToAztGeo(_leftSelectedPoint, ARANMath.DegToRad(_rightAzimuth));
                if (_rightAzimuth > 180 && _rightAzimuth < 360)
                    d += 180;
                _labelHandle = GlobalParams.UI.DrawText(new Aran.Geometries.MultiLineString { _segment }, GlobalParams.Settings.SymbolModel.TextSymbol, lineText, d);
            }         
        }

        public void ClearResult()
        {
            SaveIsEnabled = false;
            GlobalParams.UI.SafeDeleteGraphic(_resultPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_line1Handle);
            GlobalParams.UI.SafeDeleteGraphic(_labelHandle);
        }

        public void Clear()
        {
            PointViewModel.Clear();
            ClearResult();
        }

        #endregion

        private bool _directInverseIsChecked;
        
        public bool DistInverseIsChecked { get { return _directInverseIsChecked; }
            set 
            {
                _directInverseIsChecked = value;
                PointViewModel.RightGroupBoxIsEnabled = !_directInverseIsChecked;
                LeftDistanceIsEnabled = RightAzimuthIsEnabled = _directInverseIsChecked;

                NotifyPropertyChanged("DistInverseIsChecked");
            }
        }

        public double CalcHorisontalAccuracy()
        {
            var interfaceModel = GlobalParams.Settings.DeltaInterface;
            double bearingAccurasy = interfaceModel.AnglePrecision * ARANMath.DegToRadValue;
            double distanceAccurasy = interfaceModel.DistancePrecision;

            double D = _leftDistance;//  ARANFunctions.ReturnDistanceInMeters(prevWPT.PrjPt, wpt.PrjPt);
            double sigP;
            // TODO accuracy for madrid (...sy named property)
            //sigP = Math.Sqrt(distanceAccurasy * distanceAccurasy + PointViewModel.LeftPointModel.Accuracy * PointViewModel.LeftPointModel.Accuracy + D * D * bearingAccurasy * bearingAccurasy);   //(33)
            sigP = Math.Sqrt(distanceAccurasy * distanceAccurasy + PointViewModel.LeftAccuracy * PointViewModel.LeftAccuracy + D * D * bearingAccurasy * bearingAccurasy);   //(33)
            return sigP;
        }


        public bool EndPointIsChecked { get; set; }
    }
}
