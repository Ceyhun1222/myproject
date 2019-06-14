using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Aran.Aim.Features;
using Aran.PANDA.Common;
using Aran.Queries;
using ESRI.ArcGIS.Display;

namespace Aran.Delta.Model
{
    public enum ObjectStatus
    {
        New,
        Existing,
        Changed,
        Split
    }

    public class RouteSegmentModel:ViewModels.ViewModel
    {
        private List<IPointModel> _pointList;
        private Aran.Geometries.MultiLineString _geo;
        private int _geoHandle;
        private int _endPointHandle;
        private int _startPointHandle;
        private ISymbol _segmentSymbol;

        #region :>Ctor
        public RouteSegmentModel(ObjectStatus status)
        {
            OpenSegmentCommand = new RelayCommand(new Action<object>(openSegment));
            StartPtIsReadOnly = true;
            Status = status;

            StartPointList = new ObservableCollection<IPointModel>();
            EndPointList = new ObservableCollection<IPointModel>();
            CreateSymbol();
        }
        #endregion

        #region :>Properties
        public ObservableCollection<IPointModel> StartPointList { get; set; }
        public ObservableCollection<IPointModel> EndPointList { get; set; }

        public RouteSegment RSegment { get; set; }

        private IPointModel _selectedStartPoint;
        public IPointModel SelectedStartPoint
        {
            get { return _selectedStartPoint; }
            set 
            {
                if (_selectedStartPoint!=null)
                    EndPointList.Add(_selectedStartPoint);

                _selectedStartPoint = value;
                
                if (_selectedStartPoint != null)
                    EndPointList.Remove(_selectedStartPoint);

                if (Status == ObjectStatus.New || Status == ObjectStatus.Split)
                    CreateRouteSegment();

                if (Status == ObjectStatus.Existing && RSegment != null)
                    Status = ObjectStatus.Changed;

                if (Status== ObjectStatus.Split)
                    SegmentPointChanged?.Invoke(this, new SegmentChangeEventArgs { StartIsChanged = true });

                NotifyPropertyChanged("SelectedStartPoint");
            }
        }

        private IPointModel _selectedEndPoint;
        public IPointModel SelectedEndPoint
        {
            get { return _selectedEndPoint; }
            set
            {
                _selectedEndPoint = value;

                if (Status == ObjectStatus.New || Status == ObjectStatus.Split)
                    CreateRouteSegment();

                if (_selectedStartPoint!=null)
                    InitializeSegment();

                if (Status == ObjectStatus.Existing && RSegment != null)
                    Status = ObjectStatus.Changed;

                if (Status == ObjectStatus.Split)
                    SegmentPointChanged?.Invoke(this, new SegmentChangeEventArgs {StartIsChanged=false });

                NotifyPropertyChanged("SelectedEndPoint");
            }
        }

        public int Index { get; set; }
        public double ReverseTrueTrack { get; set; }

        private double _direction;
        public double Direction
        {
            get { return Common.ConvertAngle(_direction,RoundType.ToNearest); }
            set 
            {
                _direction =Common.DeConvertAngle(value);
                NotifyPropertyChanged("Direction");
            }
        }

        private double _length;
        public double Length
        {
            get { return Common.ConvertDistance(_length,RoundType.ToNearest); }
            set 
            {
                _length =Common.DeConvertDistance(value);
                NotifyPropertyChanged("Length");
            }
        }
        
        private bool _startPointIsReadOnly;
        private int _selectedGeoHandle;

        public bool StartPtIsReadOnly
        {
            get { return _startPointIsReadOnly; }
            set 
            {
                _startPointIsReadOnly = value;
                NotifyPropertyChanged("StartPointIsReadOnly");
            }
        }

        public Aran.Geometries.MultiLineString Geo {
            get { return _geo; }
        }
      
        public RelayCommand OpenSegmentCommand { get; set; }

        private ObjectStatus _status;
        public ObjectStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }


        public event EventHandler ApplyAllClicked;
        public event SegmentPointChangedEvent SegmentPointChanged;


        #endregion

        #region :>Methods

        public void InitializeSegment()
        {
            CreateGeo();
            SetSegmentParams();
        }

        public void ChangeEndPoint(IPointModel ptModel)
        {
            _selectedEndPoint = ptModel;
            CreateRouteSegment();

            if (_selectedStartPoint != null)
                InitializeSegment();
            NotifyPropertyChanged("SelectedEndPoint");
        }

        public void ChangeStartPoint(IPointModel ptModel)
        {
            _selectedStartPoint = ptModel;

            CreateRouteSegment();

            if (_selectedEndPoint != null)
                InitializeSegment();
            
            NotifyPropertyChanged("SelectedStartPoint");
        }

        public void SetPointList(List<IPointModel> pointList, IPointModel startPoint = null)
        {
            _pointList = pointList;

            StartPointList.Clear();
            EndPointList.Clear();

            if (Index == 1)
            {
                foreach (var ptModel in _pointList)
                {
                    StartPointList.Add(ptModel);
                    EndPointList.Add(ptModel);
                }
                SelectedStartPoint = StartPointList[0];
                SelectedEndPoint = EndPointList[1];
            }
            else
            {
                if (startPoint != null)
                {
                    StartPointList.Add(startPoint);
                    SelectedStartPoint = startPoint;
                }
                _pointList.ForEach(ptModel => EndPointList.Add(ptModel));
            }
        }

        public void SetList(IList<IPointModel> pointList)
        {
            _pointList = (List<IPointModel>)pointList;     
        }

        public void RemoveEndPoints() 
        {
            for (int i = 0; i < _pointList.Count; i++)
            {
                if (_pointList[i]!=_selectedEndPoint)
                    EndPointList.Remove(_pointList[i]);
            }
        }

        public void BackEndPoints() 
        {
            for (int i = 0; i < _pointList.Count; i++)
            {
                if (_pointList[i] != _selectedEndPoint)
                    EndPointList.Add(_pointList[i]);
            }
        }

        public void Draw() 
        {
            Clear();
            if (Index == 1)
            {
                if (_selectedStartPoint != null)
                    _startPointHandle = GlobalParams.UI.DrawPointWithText(_selectedStartPoint.GeoPrj, _selectedStartPoint.Name);
            }
            if (_selectedEndPoint != null)
                _endPointHandle = GlobalParams.UI.DrawPointWithText(_selectedEndPoint.GeoPrj, _selectedEndPoint.Name);

            if (_geo!=null)
                _geoHandle = GlobalParams.UI.DrawMultiLineString(_geo, GlobalParams.Settings.SymbolModel.LineCourseSymbol);

        }

        public void DrawSelected()
        {
            ClearSelected();   

            if (_geo != null)
                _selectedGeoHandle = GlobalParams.UI.DrawMultiLineString(_geo, _segmentSymbol);

        }

        public void Clear()
        {
            ClearSelected();
            GlobalParams.UI.SafeDeleteGraphic(_geoHandle);
            GlobalParams.UI.SafeDeleteGraphic(_startPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_endPointHandle);
        }

        public void ClearSelected()
        {
            GlobalParams.UI.SafeDeleteGraphic(_selectedGeoHandle);
        }

        private void CreateGeo()
        {
            var lineString = new Geometries.LineString();
            if (_selectedStartPoint != null && _selectedEndPoint != null)
            {
                lineString.Add(_selectedStartPoint.Geo);
                lineString.Add(_selectedEndPoint.Geo);

                _geo = new Geometries.MultiLineString();
                _geo.Add(lineString);

                if (RSegment != null)
                {
                    RSegment.CurveExtent.Geo.Clear();
                    RSegment.CurveExtent.Geo.Add(_geo[0]);
                }
            }
        }

        private void CreateRouteSegment()
        {
            if (_selectedStartPoint != null && _selectedEndPoint != null)
            {
                CreateGeo();

                if (GlobalParams.AranEnv != null)
                {
                    RSegment = GlobalParams.Database.DeltaQPI.CreateFeature<RouteSegment>();
                    GlobalParams.Database.DeltaQPI.ExcludeFeature(RSegment.Identifier);
                }
                else
                {
                    RSegment = new RouteSegment();
                }
                RSegment.CurveExtent = new Curve();
                RSegment.CurveExtent.Geo.Add(_geo[0]);

                RSegment.Start = CreateSegmentPoint(SelectedStartPoint);
                RSegment.End = CreateSegmentPoint(SelectedEndPoint);

                SetSegmentParams();
                RSegment.Length = new Aim.DataTypes.ValDistance();
                RSegment.Length.Value = Length;
                if (InitDelta.DistanceConverter.Unit == "km")
                    RSegment.Length.Uom = Aim.Enums.UomDistance.KM;
                else
                    RSegment.Length.Uom = Aim.Enums.UomDistance.NM;

                RSegment.TrueTrack = Direction;
                RSegment.ReverseTrueTrack = ReverseTrueTrack;
            }
            Draw();
        }

        private void SetSegmentParams()
        {
            Length = Common.ConvertDistance(NativeMethods.ReturnGeodesicDistance(_selectedStartPoint.Geo.X, _selectedStartPoint.Geo.Y, _selectedEndPoint.Geo.X, _selectedEndPoint.Geo.Y), RoundType.RealValue);
            double dirAzimuth = 0, dirInverseAzimuth = 0;
            NativeMethods.ReturnGeodesicAzimuth(_selectedStartPoint.Geo.X, _selectedStartPoint.Geo.Y, _selectedEndPoint.Geo.X, _selectedEndPoint.Geo.Y, out dirAzimuth, out dirInverseAzimuth);
            Direction = dirAzimuth;
            ReverseTrueTrack = dirInverseAzimuth;
        }

        private EnRouteSegmentPoint CreateSegmentPoint(IPointModel ptModel)
        {
            var segmentPt = new EnRouteSegmentPoint();
            segmentPt.PointChoice = new SignificantPoint();
            if (ptModel.ObjectType == Enums.PointChoiceType.DesignatedPoint)
                segmentPt.PointChoice.FixDesignatedPoint = ptModel.Feat.GetFeatureRef();
            else if (ptModel.ObjectType == Enums.PointChoiceType.Navaid)
                segmentPt.PointChoice.NavaidSystem = ptModel.Feat.GetFeatureRef();
            return segmentPt;
        }

        private void openSegment(object value)
        {
            if (RSegment != null)
            {
                Aran.Queries.Common.ViewFeature viewFeature = new Aran.Queries.Common.ViewFeature(RSegment);
                var resultView = viewFeature.View();
                if (resultView == System.Windows.Forms.DialogResult.Yes)
                {
                    if (ApplyAllClicked != null)
                        ApplyAllClicked(RSegment, new EventArgs());
                }
                else if (resultView == System.Windows.Forms.DialogResult.OK)
                {
                    if (Status == ObjectStatus.Existing)
                        Status = ObjectStatus.Changed;
                }
            }
        }

        private void CreateSymbol()
        {
            IRgbColor pRGB = new RgbColor();
            pRGB.RGB = Aran.PANDA.Common.ARANFunctions.RGB(100, 200, 140);
            ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Color = pRGB;
            pLineSym.Style = esriSimpleLineStyle.esriSLSDash;
            pLineSym.Width = 3;
            _segmentSymbol = pLineSym as ISymbol;

        }

        #endregion

    }
}
