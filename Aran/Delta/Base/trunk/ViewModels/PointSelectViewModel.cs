using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Delta.Enums;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Controls;

namespace Aran.Delta.ViewModels
{
    public class PointSelectViewModel:ViewModel
    {
        #region :> Fields
        private Enums.CalcultaionType _calculatorType;
        private Enums.PointChoiceType _selectedPointChoise;

        private Enums.PointChoiceType _rightPtChoise;

        private IList<DesignatedPoint> _designatedPointList;
        private IList<Navaid> _navaidEquipmentList;
        
        private IList<Model.PointModel> _allLeftPointList;
        private IList<Model.PointModel> _allRightPointList;
        private IList<Aim.NavaidEquipmentType> _leftPossibleObjectTypeList;
        private IList<Aim.NavaidEquipmentType> _rightPossibleObjectTypeList;

        private Aran.Geometries.Point _rightSelectedPoint;
        private Aran.Geometries.Point _leftSelectedPoint;

        private double _leftLatitude;
        private double _rightLatitude;

        private double _leftLongtitude;
        private double _rightLongtitude;
        private int _leftPtHandle;
        private int _rightPtHandle;

        #endregion
        
        #region :>Ctor
        public PointSelectViewModel(Enums.CalcultaionType calculatorType)
        {
            CalculatorType = calculatorType;
            LeftPointHeader = "Point 1";
            RightPointHeader = "Point 2";
            PointChoiseList = new List<Enums.PointChoiceType>();
            PointChoiseList.Add(Enums.PointChoiceType.DesignatedPoint);
            PointChoiseList.Add(Enums.PointChoiceType.Navaid);
            PointChoiseList.Add(Enums.PointChoiceType.Point);
         
            if (calculatorType != Enums.CalcultaionType.DirectInverse)
            {
                LeftSelectedPointChoise = Enums.PointChoiceType.DesignatedPoint;
                RightGroupBoxIsEnabled = true;

            }
            else
            {
                LeftSelectedPointChoise = PointChoiceType.Point;
            }

            
            RightSelectedPointChoise = Enums.PointChoiceType.DesignatedPoint;

            GlobalParams.Tool.MouseClickedOnMap +=
                new AranEnvironment.MouseClickedOnMapEventHandler(AranMapToolMenuItem_Click);

            GlobalParams.Tool.MouseMoveOnMap +=
                new AranEnvironment.MouseMoveOnMapEventHandler(AranMapToolMenuItem_Move);
        }

     
        #endregion

        #region Property

        #region :>Lists
        
        public List<Model.PointModel> LeftPointList { get; set; }
        public List<Model.PointModel> RightPointList { get; set; }
        
        public List<Enums.PointChoiceType> PointChoiseList { get; set; }

        #endregion


        public bool IsDD { get { return GlobalParams.Settings.DeltaInterface.CoordinateUnit == Settings.model.CoordinateUnitType.DD; } }

        public int Resolution { get {return Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision); } }

        public string LeftPointHeader { get; set; }
        public string RightPointHeader { get; set; }
        public bool LeftCoordContEnabled { get { return LeftSelectedPointChoise == Enums.PointChoiceType.Point && !LeftPointIsChecked; } }
        public bool LeftPointPickerIsEnabled { get { return LeftSelectedPointChoise == Enums.PointChoiceType.Point; } }

        private bool _rightGroupBoxIsEnabled;

        public bool RightGroupBoxIsEnabled
        {
            get { return _rightGroupBoxIsEnabled; }
            set
            {
                _rightGroupBoxIsEnabled = value;
                if (value)
                    DrawRightPoint();
                else
                    GlobalParams.UI.SafeDeleteGraphic(_rightPtHandle);
                NotifyPropertyChanged("RightGroupBoxIsEnabled");
            }
        }

        public Enums.CalcultaionType CalculatorType
        {
            get { return _calculatorType; }
            set 
            {
                _calculatorType = value;
                _leftPossibleObjectTypeList = new List<Aim.NavaidEquipmentType>();
                _rightPossibleObjectTypeList = new List<Aim.NavaidEquipmentType>();

                if (_calculatorType == Enums.CalcultaionType.CourseDistance)
                {
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.VOR);
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);

                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.DME);
                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);
                    LeftPointHeader = "Point (Course)";
                    RightPointHeader = "Point (Distance)";
                }
                else if (_calculatorType == Enums.CalcultaionType.CourseCourse)
                {
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.VOR);
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);

                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.VOR);
                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);
                }
                else if (_calculatorType == Enums.CalcultaionType.DistanceDistance)
                {
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.DME);
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);

                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.DME);
                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);
                }
                else if (_calculatorType == Enums.CalcultaionType.DirectInverse)
                {
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.VOR);
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);
                    _leftPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.DME);

                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.DME);
                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.TACAN);
                    _rightPossibleObjectTypeList.Add(Aim.NavaidEquipmentType.VOR);
                }
                LoadAllPointsData();
            }
        }
                
        public Enums.PointChoiceType LeftSelectedPointChoise
        {
            get { return _selectedPointChoise; }
            set
            {
                _selectedPointChoise = value;
                if (_allLeftPointList != null)
                {
                    LeftPointList = _allLeftPointList.Where(ptModel => 
                        ptModel.ObjectType == _selectedPointChoise).OrderBy(ptModel=>ptModel.Name).ToList<Model.PointModel>();
                    
                    if (LeftPointList.Count > 0)
                        LeftPointModel = LeftPointList[0];
                    else
                        LeftPointModel = null;
                }

               Functions.SetPreviousTool();
                
                if (_selectedPointChoise == Enums.PointChoiceType.DesignatedPoint)
                    LeftPointChoiseName = Delta.Properties.Resources.dPoint;
                else if (_selectedPointChoise == Enums.PointChoiceType.Navaid)
                    LeftPointChoiseName = Delta.Properties.Resources.navaid;
                else
                {
                    if (_leftPointIsChecked)
                       Functions.SetTool();
                    
                    LeftSelectedPoint = new Geometries.Point(_leftLongtitude,_leftLatitude);
                    LeftPointChoiseName = Delta.Properties.Resources.point;
                }

                if (PointSelectChanged != null)
                    PointSelectChanged(this, new EventArgs());

                NotifyPropertyChanged("");
            }
        }

        public double LeftLatitude
        {
            get
            {
                return _leftLatitude;
            }
            set 
            {
                _leftLatitude = value;
                if (_leftSelectedPoint != null)
                {
                    _leftSelectedPoint.Y = _leftLatitude;
                    if (LeftCoordContEnabled)
                        LeftAccuracy = CalcPointChangeAccuracy(_leftLatitude, _leftLongtitude);
                }

                DrawLeftPoint();
                NotifyPropertyChanged("LeftLatitude");
            }
        }

        public double LeftLongtitude
        {
            get { return _leftLongtitude; }
            set
            {

                _leftLongtitude= value;
                if (_leftSelectedPoint != null)
                {
                    _leftSelectedPoint.X = _leftLongtitude;
                    if (LeftCoordContEnabled)
                        LeftAccuracy = CalcPointChangeAccuracy(_leftLatitude, _leftLongtitude);
                }
                DrawLeftPoint();
                NotifyPropertyChanged("LeftLongtitude");
            }
        }

        private Model.PointModel _leftPointModel;
        public Model.PointModel LeftPointModel
        {
            get { return _leftPointModel; }
            set 
            {
                _leftPointModel = value;
                if (_leftPointModel != null)
                {
                    LeftSelectedPoint = _leftPointModel.Geo;
                    LeftAccuracy = _leftPointModel.Accuracy;
                }
                else
                {
                   // LeftSelectedPoint = null;
                    LeftAccuracy = 0;
                }
                NotifyPropertyChanged("LeftPointModel");
            }
        }

        private Model.PointModel _rightPointModel;
        public Model.PointModel RightPointModel
        {
            get { return _rightPointModel; }
            set 
            {
                _rightPointModel = value;
                if (_rightPointModel != null)
                {
                    RightSelectedPoint = _rightPointModel.Geo;
                    RightAccuracy = _rightPointModel.Accuracy;
                }
                else 
                {
                    RightAccuracy = 0;
                }

                //else
                //    RightSelectedPoint = null;
                //NotifyPropertyChanged("RightPointModel");
            }
        }
        
        public Aran.Geometries.Point LeftSelectedPoint
        {
            get { return _leftSelectedPoint; }
            set
            {
                _leftSelectedPoint = value;
                if (_leftSelectedPoint != null)
                {
                    LeftLatitude = _leftSelectedPoint.Y;
                    LeftLongtitude = _leftSelectedPoint.X;
                    DrawLeftPoint();
                   
                    if (LeftSelectedPointChoise== Enums.PointChoiceType.Point)
                        LeftAccuracy = CalcPixelAccuracy();
                }

                if (PointSelectChanged != null)
                    PointSelectChanged(this, new EventArgs());
              //  NotifyPropertyChanged("LeftSelectedPoint");
            }
        }

        public string LeftPointChoiseName { get; set; }

        public bool LeftPointListIsEnabled { get { return !LeftCoordContEnabled; } }

        public double LeftAccuracy { get; set; }
        
        public Aran.Geometries.Point RightSelectedPoint
        {
            get { return _rightSelectedPoint; }
            set 
            {
                _rightSelectedPoint = value;
                if (_rightSelectedPoint != null) 
                {
                    RightLatitude = _rightSelectedPoint.Y;
                    RightLongtitude = _rightSelectedPoint.X;
                    DrawRightPoint();

                    if (RightSelectedPointChoise == Enums.PointChoiceType.Point)
                        RightAccuracy = CalcPixelAccuracy();
                }
                if (PointSelectChanged != null)
                    PointSelectChanged(this, new EventArgs());

               // NotifyPropertyChanged("RightSelectedPoint");
            }
        }

        public bool RightCoordContEnabled
        {
            get
            { return RightSelectedPointChoise == Enums.PointChoiceType.Point && !RightPointIsChecked; }
        }

        public bool RightPointPickerIsEnabled { get { return RightSelectedPointChoise == Enums.PointChoiceType.Point; } }

        public Enums.PointChoiceType RightSelectedPointChoise
        {
            get { return _rightPtChoise; }
            set
            {
                _rightPtChoise = value;
                if (_allRightPointList != null)
                {
                    RightPointList = _allRightPointList.Where(ptModel => ptModel.ObjectType == _rightPtChoise).OrderBy(ptModel=>ptModel.Name).ToList<Model.PointModel>();
                    if (RightPointList.Count > 0)
                        RightPointModel = RightPointList[1];
                    else
                        RightSelectedPoint = null;
                }

               Functions.SetPreviousTool();
                if (_rightPtChoise == Enums.PointChoiceType.DesignatedPoint)
                    RightPointChoiseName = Delta.Properties.Resources.dPoint;
                else if (_rightPtChoise == Enums.PointChoiceType.Navaid)
                    RightPointChoiseName = Delta.Properties.Resources.navaid;
                else
                {
                    if (_rightPointIsChecked)
                       Functions.SetTool();
                    

                    RightSelectedPoint = new Geometries.Point(_rightLongtitude, _rightLatitude);
                    RightPointChoiseName = Delta.Properties.Resources.point;
                }

                NotifyPropertyChanged("");
            }
        }
        
        public double RightLatitude
        {
            get
            {
                return _rightLatitude;
            }
            set
            {
                _rightLatitude = value;
                if (_rightSelectedPoint != null)
                {
                    _rightSelectedPoint.Y = _rightLatitude;
                    if (RightCoordContEnabled)
                        RightAccuracy =  CalcPointChangeAccuracy(_rightLatitude, _rightLongtitude);
                }

                DrawRightPoint();
                NotifyPropertyChanged("RightLatitude");
            }
        }

        public double RightLongtitude
        {
            get { return _rightLongtitude; }
            set
            {

                _rightLongtitude = value;
                if (_rightSelectedPoint != null)
                {
                    _rightSelectedPoint.X = _rightLongtitude;
                    if (RightCoordContEnabled)
                        RightAccuracy = CalcPointChangeAccuracy(_rightLatitude, _rightLongtitude);
                }
                DrawRightPoint();
                NotifyPropertyChanged("RightLongtitude");
            }
        }

        public string RightPointChoiseName { get; set; }

        public bool RightPointListIsEnabled { get { return !RightCoordContEnabled; } }

        public double RightAccuracy { get; set; }

        private bool _leftPointIsChecked;
        public bool LeftPointIsChecked
        {
            get { return _leftPointIsChecked; }
            set
            {
                if (_rightPointIsChecked)
                    RightPointIsChecked = false;

                _leftPointIsChecked = value;
                if (_leftPointIsChecked)
                   Functions.SetTool();
                else
                   Functions.SetPreviousTool();
                NotifyPropertyChanged("LeftPointIsChecked");
                NotifyPropertyChanged("LeftCoordContEnabled");
            }
        }

        private bool _rightPointIsChecked;
        public bool RightPointIsChecked
        {
            get { return _rightPointIsChecked; }
            set
            {
                if (_leftPointIsChecked)
                    LeftPointIsChecked = false;

                _rightPointIsChecked = value;

                if (_rightPointIsChecked)
                   Functions.SetTool();
                else
                   Functions.SetPreviousTool();

                NotifyPropertyChanged("RightPointIsChecked");
                NotifyPropertyChanged("RightCoordContEnabled");
            }
        }

        #endregion

        #region :>Methods

        private void AranMapToolMenuItem_Click(object sender, MapMouseEventArg e)
        {
            var pt = new Aran.Geometries.Point(e.X, e.Y);
            if (_isSnapped)
                pt = new Aran.Geometries.Point(m_Position.X, m_Position.Y);

            GlobalParams.FetureSnap.SnappingFeedback.Refresh(GlobalParams.HWND);
            var ptGeo = GlobalParams.SpatialRefOperation.ToGeo(pt);
            if (LeftPointIsChecked)
                LeftSelectedPoint = ptGeo;
            else
                RightSelectedPoint = ptGeo;
            if (PointSelectChanged != null)
                PointSelectChanged(this, new EventArgs());
        }

        private void AranMapToolMenuItem_Move(object sender, MapMouseEventArg arg)
        {
            IHookHelper2 hookHelper2 = GlobalParams.HookHelper as IHookHelper2;

            m_Position = new ESRI.ArcGIS.Geometry.Point();
            m_Position.X = arg.X;
            m_Position.Y = arg.Y;

            ISnappingResult snapResult = null;

            //Try to snap the current position

            snapResult = GlobalParams.FetureSnap.Snapper.Snap(m_Position);

            GlobalParams.FetureSnap.SnappingFeedback.Update(snapResult, 0);

            if (snapResult != null)
            {//Snapping occurred

                //Set the current position to the snapped location
                _isSnapped = true;
                m_Position = snapResult.Location;
            }

        }


        private void LoadAllPointsData()
        {
            _designatedPointList = GlobalParams.Database.DesignatedPointList;
            _navaidEquipmentList = GlobalParams.Database.NavaidListByTypes;
            _allLeftPointList = new List<Model.PointModel>();
            _allRightPointList = new List<Model.PointModel>();

            if (_designatedPointList.Count > 0)
            {
                var dpList = _designatedPointList.Select(designatedPoint => new Model.PointModel(GlobalParams.SpatialRefOperation)
                {
                    Name = designatedPoint.Designator,
                    Geo = designatedPoint.Location.Geo,
                    Type = designatedPoint.Type.ToString(),
                    Feat=designatedPoint,
                    ObjectType = Enums.PointChoiceType.DesignatedPoint
                }).ToList<Model.PointModel>();

                foreach (var dp in dpList)
                {
                    _allLeftPointList.Add(dp);
                    _allRightPointList.Add(dp);
                }
            }
            if (_navaidEquipmentList != null && _navaidEquipmentList.Count > 0)
            {
                Func<Aran.Aim.Features.Navaid, Model.PointModel> createPointModel = delegate(Aran.Aim.Features.Navaid navEquipment)
                {
                    var result = new Model.PointModel(GlobalParams.SpatialRefOperation);
                    result.Name = navEquipment.Designator;
                    result.Geo = navEquipment.Location.Geo;
                    result.Type = navEquipment.Type.ToString();
                    result.Feat = navEquipment;
                    //result.Geo = new NetTopologySuite.Geometries.Point(navEquipment.Location.Geo.X, navEquipment.Location.Geo.Y, navEquipment.Location.Geo.Z);
                    result.ObjectType = Enums.PointChoiceType.Navaid;
                    return result;
                };

                var leftNavList = (from nav in _navaidEquipmentList
                                   // from navEquipment in nav.NavaidEquipment
                                   // where _leftPossibleObjectTypeList.Contains(navEquipment.TheNavaidEquipment.Type)
                                   //select createPointModel((NavaidEquipment)GlobalParams.Database.DeltaQPI.GetAbstractFeature(navEquipment.TheNavaidEquipment))).ToList<Model.PointModel>();
                                   select createPointModel(nav)).ToList<Model.PointModel>();

                foreach (var nav in leftNavList)
                    _allLeftPointList.Add(nav);

                var rightNavList = (from nav in _navaidEquipmentList
                                    // from navEquipment in nav.NavaidEquipment
                                    //where _rightPossibleObjectTypeList.Contains(navEquipment.TheNavaidEquipment.Type)
                                    //select createPointModel((NavaidEquipment)GlobalParams.Database.DeltaQPI.GetAbstractFeature(navEquipment.TheNavaidEquipment))).ToList<Model.PointModel>();
                                    select createPointModel(nav)).ToList<Model.PointModel>();
                foreach (var nav in rightNavList)
                    _allRightPointList.Add(nav);

            }

        }

        public void DrawLeftPoint() 
        {
            GlobalParams.UI.SafeDeleteGraphic(_leftPtHandle);
            if (LeftSelectedPoint!=null)
                _leftPtHandle = GlobalParams.UI.DrawPoint(LeftSelectedPoint, GlobalParams.Settings.SymbolModel.PointSymbol);
            //Model.Drawing.DrawPoint(LeftSelectedPoint, GlobalParams.Settings.SymbolModel.PointSymbol);
        }

        public void DrawRightPoint()
        {
            if (RightGroupBoxIsEnabled)
            {
                GlobalParams.UI.SafeDeleteGraphic(_rightPtHandle);
                if (_rightSelectedPoint != null)
                {
                    _rightPtHandle = GlobalParams.UI.DrawPoint(RightSelectedPoint, GlobalParams.Settings.SymbolModel.PointSymbol);
                }
            }
        }

        public void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_rightPtHandle);
            GlobalParams.UI.SafeDeleteGraphic(_leftPtHandle);
           Functions.SetPreviousTool();
        }

        private double CalcPixelAccuracy()
        {
            var pixelInSm = 2.54 / GlobalParams.HookHelper.ActiveView.ScreenDisplay.DisplayTransformation.Resolution;
            var perPixelAccuracy = pixelInSm * GlobalParams.HookHelper.FocusMap.MapScale / 100;
            return perPixelAccuracy;
        }

        public double CalcPointChangeAccuracy(double lat,double longtitude) 
        {
            double latDeg,latMin,latSec;
            double longDeg,longMin,longSec;

            Functions.DD2DMS(lat, out latDeg, out latMin, out latSec, 1);
            Functions.DD2DMS(longtitude,out longDeg,out longMin,out longSec,1);

            int coordPrecision = Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision);
            var latSecNew =latSec+Math.Pow(0.1,coordPrecision);

            var longSecNew = longSec+Math.Pow(0.1, coordPrecision);
        
            var latNew = Functions.DMS2DD(latDeg,latMin,latSecNew,1);
            var longNew = Functions.DMS2DD(longDeg,longMin,longSecNew,1);

           return NativeMethods.ReturnGeodesicDistance(longtitude,lat,longNew,latNew);
        }
        
        public event EventHandler PointSelectChanged;
        private ESRI.ArcGIS.Geometry.IPoint m_Position;
        private bool _isSnapped;
        #endregion
    }
}
