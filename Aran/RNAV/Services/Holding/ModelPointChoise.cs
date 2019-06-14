using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Delib.Classes.Features.AirportHeliport;
using Delib.Classes.Features.Organisation;
using System.Collections;
using Delib.Classes.Features.Navaid;
using ChoosePointNS;
using ARAN.Contracts.UI;
using ARAN.Common;

namespace Holding
{

    public class ModelPointChoise : Changed, INotifyPropertyChanged
    {
        #region :>Fields
        private SignificantPointChoice _curSignificantType;
        private OrganisationAuthority _curOrganisation;
        private AirportHeliport _curAdhp;
        private DesignatedPoint _curSigPoint;
        private bool _pointPickerIsActive;
        private DBModule _dbModule;
        private UIContract _ui;
        private SpatialReferenceOperation _spatialOperation;
        private int _bigCircleHandle, _littleCircleHandle, _ptHandle;
        private double _applyAtt, _applyXtt;
        private ARAN.GeometryClasses.Point _applyPoint;

        private int _changedCount;
        private bool _holdingAreaIsEnabled = true;
        //private double distanceOld;
        #endregion

        #region :>Constructor

        public ModelPointChoise(double distance, double minDistance, double maxDistance)
        {
            _ui = GlobalParams.UI;
            _spatialOperation = GlobalParams.SpatialRefOperation;
            _dbModule = GlobalParams.Database;

            _distance = distance;
            _minDistance = minDistance;
            _maxDistance = maxDistance;
            SignificantList = new List<SignificantPointChoice> { SignificantPointChoice.DesignatedPoint, SignificantPointChoice.Point };
            _curSignificantType = SignificantPointChoice.DesignatedPoint;

            OrganisationList = _dbModule.HoldingQpi.GetOrganisatioListDesignatorNotNull();
            if (OrganisationList.Count > 0)
                CurOrganisation = OrganisationList[0];
            _changedCount = 0;

        }

        #endregion

        #region :>Property
        #region Lists
        public List<OrganisationAuthority> OrganisationList { get; set; }

        public List<AirportHeliport> AdhpList { get; set; }

        public List<DesignatedPoint> PointList { get; set; }

        public List<SignificantPointChoice> SignificantList { get; set; }
        #endregion

        #region CurValue

        public OrganisationAuthority CurOrganisation
        {
            get { return _curOrganisation; }
            set
            {
                CurOrganisationChange(value);
            }
        }

        public AirportHeliport CurAdhp
        {
            get { return _curAdhp; }
            set
            {
                CurAdhpChange(value);
            }
        }

        public DesignatedPoint CurSigPoint
        {
            get { return _curSigPoint; }
            set
            {

                if (Equals(_curSigPoint, value))
                    return;
                if (value == null)
                {
                    DeletePoint();
                    _curPoint = null;
                    _curSigPoint = value;

                    ChangeCentralMeridian(GeomFunctions.GmlToAranPoint(_curAdhp));
                    
                }
                else
                    CurSigPointChange(value);

                if (CurPointChanged != null)
                    CurPointChanged(this, new EventArgs());

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurSigPoint"));



            }
        }

        private ARAN.GeometryClasses.Point _curPoint;

        public ARAN.GeometryClasses.Point CurPoint
        {
            get { return _curPoint; }
            set
            {
                if (Equals(_curPoint == value))
                    return;
                ChangeModelChanged(_curPoint, value, _applyPoint);
                _curPoint = value;
                ChangeCentralMeridian(_curPoint);
                if (CurPointChanged != null)
                    CurPointChanged(this, new EventArgs());
            }
        }

        public SignificantPointChoice PointChoise
        {
            get { return _curSignificantType; }
            set
            {
                if (Equals(_curSignificantType, value))
                    return;

                _curSignificantType = value;

                _pointPickerIsActive = value == SignificantPointChoice.Point;
                DeletePoint();
                if (_curSignificantType == SignificantPointChoice.DesignatedPoint)
                {
                    //  _curPoint = null;
                    HoldingService.ToolClicked = false;
                    HoldingService.ByClickToolButton.SetDownState(false);
                    PointList = _dbModule.HoldingQpi.GetDesignatedPointList(_curAdhp, _minDistance, _distance);

                }
                else if (_curSignificantType == SignificantPointChoice.Point)
                {
                    PointList = null;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PointChoise"));
            }

        }


        private double _att;

        public double ATT
        {
            get { return _att; }
            set
            {
                if (_xtt == value)
                    return;
                _att = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));
            }
        }

        private double _xtt;
        public double XTT
        {
            get { return _xtt; }
            set
            {
                if (_xtt == value)
                    return;
                ChangeModelChanged(_xtt, value, _applyXtt);
                _xtt = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));
            }

        }

        #endregion

        #region distance
        private double _minDistance;
        public double MinDistance
        {
            get { return Common.ConvertDistance(_minDistance,roundType.toUp); }
            set
            {
                _minDistance = Common.DeConvertDistance(value);

            }

        }

        private double _maxDistance;
        public double MaxDistance
        {
            get { return Common.ConvertDistance(_maxDistance, roundType.toDown); }
            set
            {
                _maxDistance = Common.DeConvertDistance(value);
            }
        }

        private double _distance;
        public double Distance
        {
            get { return Common.ConvertDistance(_distance, roundType.toNearest); }
            set { DistanceCondition(this, "Distance", Common.DeConvertDistance(value), PropertyChanged); }
        }

        private int _tag;
        public int TAG
        {
            get
            {

                return _tag;
            }
            set
            {
                _tag = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TAG"));
            }
        }
        #endregion

        #region ValueActive

        public bool PointPickerIsActive
        {
            get { return _pointPickerIsActive; }
            set { _pointPickerIsActive = value; }
        }

        public bool SigPointListIsActive
        {
            get { return (!_pointPickerIsActive && (PointList!=null && PointList.Count != 0)); }

        }

        public bool ADHPIsActive { get{return AdhpList!= null && AdhpList.Count>0;}}

        public bool CreatePointIsActive { get { return !HoldingService.ToolClicked && PointChoise == SignificantPointChoice.Point; } }

        #endregion

        #endregion

        public bool HoldingAreaIsEnabled
        {
            get { return (!IsApply && _holdingAreaIsEnabled) || (ChangedCount != 0 && _holdingAreaIsEnabled); }
            set
            {
                if (_holdingAreaIsEnabled == value)
                    return;
                _holdingAreaIsEnabled = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
            }
        }

        public int ChangedCount
        {
            get { return _changedCount; }
            set
            {
                _changedCount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
            }

        }

        public bool ReportIsEnabled
        {
            get { return !HoldingAreaIsEnabled && _holdingAreaIsEnabled; }
        }

        private bool _saveIsEnabled;
        public bool SaveIsEnabled 
        {
            get{return !HoldingAreaIsEnabled && _holdingAreaIsEnabled && _saveIsEnabled;}
            set
            {
                _saveIsEnabled = value;
                    if (PropertyChanged!=null)
                        PropertyChanged(this,new PropertyChangedEventArgs("ReportIsEnabled"));

            }
        }


        #region :>Methods
        public void SetMinMaxDistance(double minValue, double maxValue)
        {
            _minDistance = minValue;
            _maxDistance = maxValue;
            double tmpDouble = _distance;
            if (_distance < _minDistance)
                tmpDouble = 2 * _minDistance;
            else
                if (_distance > _maxDistance)
                    tmpDouble = _maxDistance;
            //if (tmpDouble != 0)
            DistanceCondition(this, "Distance", tmpDouble, PropertyChanged);
        }

        public Boolean PointDistanceControl(ARAN.GeometryClasses.Point ptGeo)
        {
            if ((ptGeo == null)||(_curAdhp==null))
                return false;                        

            ARAN.GeometryClasses.Point ptPrj = _spatialOperation.GeoToPrjPoint(ptGeo);
            ARAN.GeometryClasses.Point ptPrjAirpot = _spatialOperation.GeoToPrjPoint(GeomFunctions.GmlToAranPoint(_curAdhp));
            double ptToArpDistance = ARANFunctions.ReturnDistanceAsMeter(ptPrj, ptPrjAirpot);
            if (ptToArpDistance > _distance || ptToArpDistance < _minDistance)
            {
                return false;
            }
            else
                return true;

        }

        public void SetATTXTTValue(double att, double xtt)
        {
            if ((_att == att) && (_xtt == xtt))
                return;
            ChangeModelChanged(_att, att, _applyAtt);
            ChangeModelChanged(_xtt, xtt, _applyXtt);
            _att = att;
            _xtt = xtt;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ATTXTT"));

        }

        public void Dispose()
        {
            _ui.SafeDeleteGraphic(ref _ptHandle);
            _ui.SafeDeleteGraphic(ref _bigCircleHandle);
            _ui.SafeDeleteGraphic(ref _littleCircleHandle);

        }

        public void DrawPoint(ARAN.GeometryClasses.Point ptPrj)
        {
            _ptHandle = _ui.DrawPoint(ptPrj, 1);
        }

        public void DeletePoint()
        {
            _ui.SafeDeleteGraphic(ref _ptHandle);
        }

        private void DistanceCondition(object owner, string propName, double newValue, PropertyChangedEventHandler eventHandler)
        {
            if (owner.GetType().GetProperty(propName) == null)
            {
                throw new ArgumentException("No property named " + propName + " on " + owner.GetType().FullName);
            }

            if (!Equals(_distance, Common.DeConvertDistance(newValue)))
            {
                _distance = Common.AdaptToInterval(newValue, _minDistance, _maxDistance, 1);
                if (_curSignificantType == SignificantPointChoice.DesignatedPoint)
                {
                    PointList = _dbModule.HoldingQpi.GetDesignatedPointList(_curAdhp, _minDistance, _distance);

                }
                else if (_curSignificantType == SignificantPointChoice.Point)
                {
                    if (!PointDistanceControl(_curPoint))
                    {
                        CurPoint = null;
                        _ui.SafeDeleteGraphic(ref _ptHandle);
                    }

                }

                DrawPointArea(GeomFunctions.GmlToAranPoint(_curAdhp), _distance, _minDistance);

                if (eventHandler != null)
                {
                    eventHandler(owner, new PropertyChangedEventArgs(propName));
                }


            }


        }

        private void CurOrganisationChange(OrganisationAuthority value)
        {
            if (value != _curOrganisation)
            {
                _curOrganisation = value;
                AdhpList = _dbModule.HoldingQpi.GetAdhpListDesignatorNotNull(value);
                if (AdhpList.Count > 0)
                    CurAdhp = AdhpList[0];
                else
                {
                    CurAdhp = null;
                }
            }
        }

        private void CurAdhpChange(AirportHeliport value)
        {

            if (_curAdhp != value && value != null)
            {
                _curAdhp = value;
                if (_curSignificantType == SignificantPointChoice.DesignatedPoint)
                {
                    PointList = _dbModule.HoldingQpi.GetDesignatedPointList(value, _minDistance, _distance);

                    if (PointList.Count > 0)
                        CurSigPoint = PointList[0];
                    else
                    {
                        if (CurSigPoint == null)
                            ChangeCentralMeridian(GeomFunctions.GmlToAranPoint(_curAdhp));
                        else
                            CurSigPoint = null;
                    }

                }
                else if (_curSignificantType == SignificantPointChoice.Point)
                {

                    if (!PointDistanceControl(_curPoint))
                    {
                        CurPoint = null;
                        DeletePoint();
                    }

                    ChangeCentralMeridian(GeomFunctions.GmlToAranPoint(_curAdhp));

                }

                DrawPointArea(GeomFunctions.GmlToAranPoint(_curAdhp), _distance, _minDistance);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurAdhp"));

            }
            else if (_curAdhp!=value && value == null)
            {
                PointList = null;
                DeletePointArea();
                
                _curAdhp = value;
                CurPoint = null;
                DeletePoint();

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurAdhp"));
            }

        }

        private void CurSigPointChange(DesignatedPoint value)
        {
            if (_curSigPoint != value)
            {

                ARAN.GeometryClasses.Point tmpPt = GeomFunctions.Assign(value.location);
                ChangeModelChanged(_curPoint, tmpPt, _applyPoint);
                _curPoint = tmpPt;
                ChangeCentralMeridian(_curPoint);

                ARAN.GeometryClasses.Point ptPrj = _spatialOperation.GeoToPrjPoint(_curPoint);

                _ui.SafeDeleteGraphic(ref _ptHandle);

                _ptHandle = _ui.DrawPointWithText(ptPrj, 1, value.designator);

                _curSigPoint = value;

            }

        }

        private void DrawSignificantPoint(DesignatedPoint significantPoint)
        {
            _ui.SafeDeleteGraphic(ref _ptHandle);
            _curPoint = GeomFunctions.Assign(significantPoint.location);
            _ptHandle = _ui.DrawPointWithText(_spatialOperation.GeoToPrjPoint(_curPoint), 1, significantPoint.designator);
        }

        private void DrawPointArea(ARAN.GeometryClasses.Point centerPt, double maxDistance, double minDistance)
        {
            DeletePointArea();
            ARAN.GeometryClasses.Part bigCircle = ARANFunctions.CreateCircleAsPartPrj(_spatialOperation.GeoToPrjPoint(centerPt), maxDistance);
            ARAN.GeometryClasses.Part littleCircle = ARANFunctions.CreateCircleAsPartPrj(_spatialOperation.GeoToPrjPoint(centerPt), minDistance);
            _bigCircleHandle = _ui.DrawPart(bigCircle, 1, 1);
            _littleCircleHandle = _ui.DrawPart(littleCircle, 1, 1);
        }

        private void DeletePointArea()
        {
            _ui.SafeDeleteGraphic(ref _bigCircleHandle);
            _ui.SafeDeleteGraphic(ref _littleCircleHandle);
        }

        private void ChangeCentralMeridian(ARAN.GeometryClasses.Point pt)
        {
            if (pt == null)
                return;
            if (Math.Abs(pt.X - _spatialOperation.CentralMeridian) > 3)
            {
                GlobalParams.SpatialRefOperation.ChangeCentralMeridian(pt);
            }

        }


        #endregion

        #region :>SpecialView

        public string ATTXTT
        {
            get
            {
                if (_curPoint != null)
                    return Math.Round(ATT, 2) + " / " + Math.Round(XTT, 2);
                else
                    return "";
            }
        }
        #endregion

        #region :>Event
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CurPointChanged;
        #endregion

        public override void SetApplyParams()
        {
            _applyAtt = _att;
            _applyXtt = _xtt;
            _applyPoint = _curPoint;
            SaveIsEnabled = false;
        }
    }
}
