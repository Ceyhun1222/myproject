using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Delta.Enums;
using Aran.Geometries;
using Aran.PANDA.Common;
using AranSupport;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using MessageBox = System.Windows.MessageBox;
using System.IO;
using System.Linq;
using Aran.Aim.Data;
using Aran.Delta.Model;
using Buffer = Aran.Delta.EsriClasses.Buffer;
using System.Threading.Tasks;
using System.Windows.Interop;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Metadata.Utils;

namespace Aran.Delta.ViewModels
{
    public class CreateAirspaceViewModel:ViewModel
    {
        #region :> Fields
        private readonly List<Aran.Geometries.Point> _pointList;
        private int _curGeometryHandle;
        private int _curSelectedPointHandle;
        private IActiveView _activeView;

        private IScreenDisplay _screenDisp;

        private ESRI.ArcGIS.Display.INewCircleFeedback _lineFeedback;
        private ESRI.ArcGIS.Display.INewPolygonFeedback _polygonFeedback;
        private ESRI.ArcGIS.Display.INewArcFeedback _arcFeedback;
        private bool _isFirstClick;
        

        private Aran.Geometries.MultiPolygon _curGeometry;
        private bool _isFinished;

        private readonly ISymbol _pointSymbol;

        private double _circleRadius;
        private bool _isSnapped;
        private ESRI.ArcGIS.Geometry.IPoint _snappedEsriPoint;
        private int _drawingHandle;
        private Aran.Geometries.Point _tdzCenterPoint;
        private AreaPointModel _tdzCenterPointModel;
        private int _ctrCenterPointHandle;

        private bool _geoBorderIsStarting;

        private CreatingGeoType _selectedGeoType;
        private Aran.Geometries.MultiPoint _firMltPrj;

        private bool _tdzCenterPointIsChecked;
        private int _firstBorderIndex;
        private int _secondBorderIndex;
        private Visibility _ellipseIsVisible;
        private MultiPoint _firMltPt;
        private Enums.CreatingAreaType _snapAirspaceType;

        private Airspace _selectedClipAirspace;
        private Aran.Geometries.MultiPolygon _selectedOperAirspaceGeom;
        private int _selectedClipAirspaceHandle;

        private bool _tdzCoordContEnabled;
        private Visibility _clipAreaIsVisible;
        private bool _applyCommandIsEnabled;
        private Airspace _selectedFirAirspace;
        private bool _drawFirIsChecked;
        private MultiPolygon _firGeometry;
        private int _firGeometryHandle;
        private FillSymbol _firSymbol;

        private List<NotamPointClass> _pointFormatList;

        private OperationType _operationType;

        private bool _unionIsChecked;
        private Visibility _textFormatIsVisible;

        private string _airspaceText;
        private NotamFormatClass _selectedWaterMark;
        private bool _intersecIsChecked;
        private bool _clipIsChecked;

        private int _selectedAirspaceType;
        private Model.AreaPointModel _selectedPoint;

        private double _tdzWidth;
        private bool _addPointIsEnabled;
        private double _tdzRadius;
        private double _tdzDirection;
        private double _tdzLongtitude;
        private double _tdzRadiusDirection;
        private double _tdzRadiusBackward;

        private bool _operationIsExpanded;
        private Visibility _tdzCreateIsVisible;
        private double _movingLatitude;
        private double _movingLongtitude;

        #endregion

        #region :> Ctor

        public CreateAirspaceViewModel()
        {
            try
            {
                _selectedGeoType = CreatingGeoType.None;
                TdzCreateIsVisible = Visibility.Collapsed;
                TdzCenterPointIsChecked = false;

                EllipseIsVisible = Visibility.Collapsed;

                TextFormatIsVisible = Visibility.Collapsed;

                ClearCommand = new RelayCommand(new Action<object>(clearCommand_onClick));
                AddPointCommand = new RelayCommand(new Action<object>(addPoint_onClick));
                UpdatePointCommand = new RelayCommand(new Action<object>(updatePoint_onClick));

                DrawTdzCommand = new RelayCommand(new Action<object>(drawTdzCommand_onClick));
                RemoveCommand = new RelayCommand(new Action<object>(removePoint_onClick));
                ContinueCommand = new RelayCommand(new Action<object>(continue_onClick));
                DrawEllipseCommand = new RelayCommand(new Action<object>(DrawEllipse));
                ApplyOperationCommand = new RelayCommand(new Action<object>(Operation_onClick));
                CloseCommand = new RelayCommand(new Action<object>(close_onClick));
                ViewTextFormatGeoCommand = new RelayCommand(new Action<object>(ViewTextFormatGeo));

                if (GlobalParams.AranEnv != null)
                    SaveCommand = new RelayCommand(new Action<object>(saveCommand_onClick));
                else
                    SaveCommand = new RelayCommand(new Action<object>(SaveToArena));

                IsDD = GlobalParams.Settings.DeltaInterface.CoordinateUnit == Settings.model.CoordinateUnitType.DD;
                _pointList = new List<Aran.Geometries.Point>();
                PointList = new ObservableCollection<Model.AreaPointModel>();
                AddPointIsEnabled = true;

                _screenDisp = GlobalParams.HookHelper.ActiveView.ScreenDisplay;
                _activeView = GlobalParams.HookHelper.ActiveView;

                GlobalParams.Tool.MouseClickedOnMap +=
                    new AranEnvironment.MouseClickedOnMapEventHandler(AranMapToolMenuItem_Click);

                GlobalParams.Tool.MouseMoveOnMap +=
                    new AranEnvironment.MouseMoveOnMapEventHandler(AranMapToolMenuItem_Move);

                GlobalParams.Tool.MouseDownOnMap +=
                    new AranEnvironment.MouseClickedOnMapEventHandler(AranMapToolMenuItem_MouseDown);

                GlobalParams.Tool.MouseOnDblClickOnMap +=
                    new AranEnvironment.MouseDblClickOnMapEventHandler(AranMapToolMenuItem_OnDblClick);

                GlobalParams.Tool.MouseOnRightClickOnMap +=
                    new AranEnvironment.MouseRightlClickOnMapEventHandler(AranMapToolMenuItem_RightClick);

                PointList.CollectionChanged += PointList_CollectionChanged;

                _curGeometry = new Aran.Geometries.MultiPolygon();

                var pMarkerSym = new SimpleMarkerSymbol();
                IColor color = new RgbColor();
                color.RGB = 255;
                pMarkerSym.Color = color;
                pMarkerSym.Size = 10;
                pMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                _pointSymbol = (ISymbol) pMarkerSym;

                _firstBorderIndex = -1;
                _secondBorderIndex = -1;

                _firSymbol = new FillSymbol
                {
                    Outline = new LineSymbol(eLineStyle.slsDash, 180, 4),
                    Style = eFillStyle.sfsVertical,
                    Color = 100
                };

                FirAirspaceList = GlobalParams.Database.GetAirspaceList;
                if (FirAirspaceList.Count > 0)
                {
                    var firAirspace = FirAirspaceList.Find(airspace => airspace.Type == Aim.Enums.CodeAirspace.FIR);
                    if (firAirspace != null)
                        SelectedFirAirspace = firAirspace;
                    else
                        SelectedFirAirspace = FirAirspaceList[0];
                }
                AddedLatitude = 0;
                AddedLongtitude = 0;

                OperationIsExpanded = false;
                ClipIsChecked = true;
                ApplyCommandIsEnabled = true;

                var sortedAirspaceList = GlobalParams.Database.GetAirspaceList.OrderBy(airsapce => airsapce.Name).ToList();
                AirspaceList = new ObservableCollection<Airspace>(sortedAirspaceList);
                
                _pointFormatList = new List<NotamPointClass>();

                WaterMarkList = Functions.CreateFormatList();
                SelectedWaterMark = WaterMarkList[0];

                if (Common.TryCalculateResolutionBasedOnAccuracy(GlobalParams.Accuracy, out var resolution))
                {
                    Resolution = resolution;
                    ResolutionDecimalCount = Common.CalculateResolutionDecimalCount(Resolution);
                }
                else
                {
                    throw new Exception("Wrong accuracy entered!");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
      
        #endregion

        #region :> Property

        public ObservableCollection<Model.AreaPointModel> PointList { get; set; }
        public ObservableCollection<Airspace> AirspaceList { get; set; }
        public List<Airspace> FirAirspaceList { get; set; }
        public List<NotamFormatClass> WaterMarkList{ get; set; }
        public List<Model.AreaPointModel> SelectedPointList { get; set; }

        public RelayCommand ClearCommand { get; set; }
        public RelayCommand AddPointCommand { get; set; }
        public RelayCommand UpdatePointCommand { get; set; }
        public RelayCommand DrawTdzCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand DrawEllipseCommand { get; set; }
        public RelayCommand ApplyOperationCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand ViewTextFormatGeoCommand { get; set; }

        public Airspace SelectedClipAirspace
        {
            get { return _selectedClipAirspace; }
            set
            {
                _selectedClipAirspace = value;

                GlobalParams.UI.SafeDeleteGraphic(_selectedClipAirspaceHandle);
                if (_selectedClipAirspace != null)
                {
                    if (_selectedClipAirspace.GeometryComponent.Count > 0 &&
                        _selectedClipAirspace.GeometryComponent[0].TheAirspaceVolume != null)
                    {
                        var tmpAirspaceGeo =
                            _selectedClipAirspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo;

                        _selectedOperAirspaceGeom = GlobalParams.SpatialRefOperation.ToPrj(tmpAirspaceGeo);
                        if (OperationIsExpanded)
                        {
                            _selectedClipAirspaceHandle = GlobalParams.UI.DrawMultiPolygon(_selectedOperAirspaceGeom,
                                201, eFillStyle.sfsCross, true,
                                false);
                        }
                    }
                }
            }
        }

        public bool ApplyCommandIsEnabled
        {
            get { return _applyCommandIsEnabled; }
            set
            {
                _applyCommandIsEnabled = value;
                NotifyPropertyChanged("ApplyCommandIsEnabled");
            }
        }

        public int SelectedAirspaceType
        {
            get { return _selectedAirspaceType; }
            set 
            {
                _selectedAirspaceType = value;
                //if (PointList!=null)
                //    PointList.Clear();
                NotifyPropertyChanged("SelectedAirspaceType");
            }
        }
        
        public Model.AreaPointModel SelectedPoint
        {
            get { return _selectedPoint; }
            set 
            {
                _selectedPoint = value;
                if (_selectedPoint == null) return;

                AddedLatitude = _selectedPoint.GetLatitude();
                AddedLongtitude = _selectedPoint.GetLongtitude();

                GlobalParams.UI.SafeDeleteGraphic(_curSelectedPointHandle);
                if (_selectedPoint.PrjPoint!=null)
                    _curSelectedPointHandle = GlobalParams.UI.DrawPointPrj(_selectedPoint.PrjPoint, _pointSymbol);
                
                NotifyPropertyChanged("AddedLatitude");
                NotifyPropertyChanged("AddedLongtitude");
            }
        }

        public Airspace SelectedFirAirspace
        {
            get { return _selectedFirAirspace; }
            set
            {
                _selectedFirAirspace = value;
                if (_selectedFirAirspace != null)
                {
                    try
                    {
                        var firGeo = _selectedFirAirspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo;
                        _firGeometry = GlobalParams.SpatialRefOperation.ToPrj(firGeo);
                        _firMltPrj = _firGeometry.ToMultiPoint();
                        _firMltPt = firGeo.ToMultiPoint();
                        if (DrawFirIsChecked)
                        {
                            GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
                            _firGeometryHandle = GlobalParams.UI.DrawMultiPolygon(_firGeometry, _firSymbol);
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error occured when get geometry Airspace");
                    }
                }
                else
                {
                    GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
                }
                NotifyPropertyChanged("SelectedFirAirspace");
            }
        }
        
        public bool DrawCircleIsChecked
        {
            get { return _selectedGeoType == CreatingGeoType.Circle; }
            set
            {
                Clear();

                _selectedGeoType = value ? CreatingGeoType.Circle : CreatingGeoType.None;

                TdzCreateIsVisible = EllipseIsVisible = TextFormatIsVisible =  Visibility.Hidden;

                CircleRadius = 0;

                if (value)
                {
                   Functions.SetTool();
                    _isFirstClick = true;
                    
                    SelectedAirspaceType = 1;
                }
                else
                {
                    _lineFeedback = null;
                    SelectedAirspaceType = 0;
                   Functions.SetPreviousTool();
                }

                UpdateButtons();
            }
        }

        public bool DrawLineIsChecked
        {
            get { return _selectedGeoType== CreatingGeoType.Line; }
            set 
            {
                Clear();

                _selectedGeoType = value ? CreatingGeoType.Line : CreatingGeoType.None;

                TdzCreateIsVisible=EllipseIsVisible =TextFormatIsVisible = Visibility.Hidden;
               
                if (value)
                {
                   Functions.SetTool();
                    _isFirstClick = true;
                    _polygonFeedback = new ESRI.ArcGIS.Display.NewPolygonFeedback
                    {
                        Symbol = GlobalParams.Settings.SymbolModel.BufferSymbol,
                        Display = _screenDisp
                    };
                    _geoBorderIsStarting = false;
                    _firstBorderIndex = -1;
                    _secondBorderIndex = -1;
                    SelectedAirspaceType = 1;
                    _snapAirspaceType = CreatingAreaType.Border;
                }     
                else
                {
                    SelectedAirspaceType = 0;
                    _polygonFeedback = null;
                   Functions.SetPreviousTool();
                    _isFirstClick = true;
                    //_airspaceTypeView.Close();
                }
                UpdateButtons();
            }
        }

        public bool DrawCtrIsChecked
        {
            get { return _selectedGeoType== CreatingGeoType.Ctr; }
            set 
            {
                Clear();

                _selectedGeoType = value ? CreatingGeoType.Ctr : CreatingGeoType.None;

                _curGeometry = new Geometries.MultiPolygon();

                if (value)
                {
                    EllipseIsVisible = Visibility.Collapsed;
                    TextFormatIsVisible = Visibility.Collapsed;
                    TdzCreateIsVisible = Visibility.Visible;

                    if (_tdzCenterPointIsChecked)
                       Functions.SetTool();
                    else
                       Functions.SetPreviousTool();
                    SelectedAirspaceType = 2;
                }
                else
                {
                    TdzCreateIsVisible = Visibility.Collapsed;
                   Functions.SetPreviousTool();
                    SelectedAirspaceType = 0;
                }
                UpdateButtons();
            }
        }

        public bool DrawEllipseIsChecked
        {
            get { return _selectedGeoType== CreatingGeoType.Ellipse; }
            set
            {
                Clear();

                _selectedGeoType = value ? CreatingGeoType.Ellipse : CreatingGeoType.None;
                _curGeometry = new Geometries.MultiPolygon();

                if (value)
                {
                    EllipseIsVisible= Visibility.Visible;
                    TdzCreateIsVisible = Visibility.Collapsed;
                    TextFormatIsVisible = Visibility.Collapsed;
                    if (_tdzCenterPointIsChecked)
                       Functions.SetTool();
                    else
                       Functions.SetPreviousTool();
                    SelectedAirspaceType = 3;
                }
                else
                {
                    EllipseIsVisible = Visibility.Collapsed;
                   Functions.SetPreviousTool();
                    SelectedAirspaceType = 0;
                }
                UpdateButtons();
            }
        }

        public bool DrawArcByRadiusIsChecked
        {
            get { return _selectedGeoType == CreatingGeoType.ArcByRadius; }
            set
            {
               Clear();
                _selectedGeoType = value ? CreatingGeoType.ArcByRadius : CreatingGeoType.None;
                
                _curGeometry = new MultiPolygon();

                CircleRadius = 400;

                if (value)
                {
                   Functions.SetTool();
                    _isFirstClick = true;
                    _arcFeedback = new NewArcFeedbackClass()
                    {
                        Symbol = GlobalParams.Settings.SymbolModel.BufferSymbol,
                        Display = _screenDisp
                    };
                    SelectedAirspaceType = 1;
                }
                else
                {
                    _arcFeedback = null;
                    SelectedAirspaceType = 0;
                   Functions.SetPreviousTool();
                }

                UpdateButtons();

            }

        }

        public bool DrawTextAirspaceIsChecked
        {
            get { return _selectedGeoType== CreatingGeoType.Text; }
            set
            {
                Clear();

                _selectedGeoType = value ? CreatingGeoType.Text : CreatingGeoType.None;
                _curGeometry = new Geometries.MultiPolygon();

                if (value)
                {
                    TextFormatIsVisible = Visibility.Visible;
                    EllipseIsVisible= Visibility.Collapsed;
                    TdzCreateIsVisible = Visibility.Collapsed;
                    
                    SelectedAirspaceType = 4;
                }
                else
                {
                    TextFormatIsVisible = Visibility.Collapsed;
                    SelectedAirspaceType = 0;
                }
                UpdateButtons();
            }
        }

        public bool DrawFirIsChecked
        {
            get { return _drawFirIsChecked; }
            set
            {
                _drawFirIsChecked = value;
                
                GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
                
                if (_drawFirIsChecked)
                    _firGeometryHandle = GlobalParams.UI.DrawMultiPolygon(_firGeometry, _firSymbol);
                
                NotifyPropertyChanged("DrawFirIsChecked");
            }
        }

        public bool OperationIsExpanded
        {
            get { return _operationIsExpanded; }
            set
            {
                _operationIsExpanded = value;
                GlobalParams.UI.SafeDeleteGraphic(_selectedClipAirspaceHandle);
                if (value)
                    if (AirspaceList.Count > 0)
                        SelectedClipAirspace = AirspaceList[0];

                NotifyPropertyChanged("OperationIsExpanded");
            }
        }

        public bool IsDD { get; private set; }

        public Visibility MovingPointIsVisible 
        {
            get 
            {
                if (TdzCreateIsVisible == Visibility.Visible || EllipseIsVisible == Visibility.Visible)
                    return Visibility.Collapsed;

               if ( (_selectedGeoType== CreatingGeoType.Line || _selectedGeoType== CreatingGeoType.ArcByRadius || _selectedGeoType== CreatingGeoType.Circle) && !_isFinished)
                    return  Visibility.Visible;
                else 
                    return  Visibility.Collapsed; 
            }
        }

        public Visibility PointListIsVisible 
        {
            get 
            {
                if (TdzCreateIsVisible == Visibility.Visible || EllipseIsVisible == Visibility.Visible || TextFormatIsVisible == Visibility.Visible || MovingPointIsVisible == Visibility.Visible)
                    return Visibility.Collapsed;

                return Visibility.Visible; 
            } 
        }

        public Visibility CircleRadiusIsVisible
        {
            get { return (_selectedGeoType == CreatingGeoType.Circle) ? Visibility.Visible : Visibility.Hidden; }
            set
            {
                NotifyPropertyChanged("CircleRadiusIsVisible");
            }
        }

        public Visibility TdzCreateIsVisible
        {
            get { return _tdzCreateIsVisible; }
            set 
            {
                _tdzCreateIsVisible = value;
                NotifyPropertyChanged("TdzCreateIsVisible");
            }
        }

        public Visibility EllipseIsVisible
        {
            get { return _ellipseIsVisible; }
            set
            {
                _ellipseIsVisible = value;
                NotifyPropertyChanged("EllipseIsVisible");
            }
        }

        public Visibility TextFormatIsVisible
        {
            get { return _textFormatIsVisible; }
            set
            {
                _textFormatIsVisible = value;
                NotifyPropertyChanged("TextFormatIsVisible");
            }
        }

        public Visibility ClipAreaIsVisible
        {
            get { return _clipAreaIsVisible; }
            set
            {
                _clipAreaIsVisible = value;
                //if (AirspaceList == null)
                //{
                //    AirspaceList = GlobalParams.Database.GetAirspaceList;
                //    if (AirspaceList.Count > 0)
                //        SelectedClipAirspace = AirspaceList[0];
                //}
                NotifyPropertyChanged("ClipAreaIsVisible");
            }
        }
        
        public double MovingLatitude
        {
            get { return _movingLatitude; }
            set 
            {
                _movingLatitude = value;
                NotifyPropertyChanged("MovingLatitude");
            }
        }
        public double MovingLongtitude
        {
            get { return _movingLongtitude; }
            set
            {
                _movingLongtitude = value;
                NotifyPropertyChanged("MovingLongtitude");
            }
        }

        public double AddedLatitude { get; set; }
        public double AddedLongtitude { get; set; }

        public string AddedLatitudeString { get; set; }
        public string AddedLongtitudeString { get; set; }
        public decimal CoordinateResolution { get; set; }

        private double _tdzLatitude;
        //private ICircularArc circualArc;

        public double TdzLatitude
        {
            get { return _tdzLatitude; }
            set 
            {
                _tdzLatitude = value;
                DrawTdzGeo();
                NotifyPropertyChanged("TdzLatitude");
            }
        }
        public double TdzLongtitude
        {
            get { return _tdzLongtitude; }
            set 
            {
                _tdzLongtitude = value;
                DrawTdzGeo();
                NotifyPropertyChanged("TdzLongtitude");
            }
        }

        public bool AddPointIsEnabled
        {
            get { return _addPointIsEnabled; }
            set 
            {
                _addPointIsEnabled = value;
                NotifyPropertyChanged("AddPointIsEnabled");
            }
        }
        
        public double TdzRadius
        {
            get { return Common.ConvertDistance(_tdzRadius,RoundType.ToNearest); }
            set 
            {
                _tdzRadius = Common.DeConvertDistance(value);
                NotifyPropertyChanged("TdzRadius");
            }
        }

        public double TdzDirection
        {
            get { return _tdzDirection; }
            set 
            {
                _tdzDirection = value;
                NotifyPropertyChanged("TdzDirection");
            }
        }
       
        public double TdzRadiusDirection
        {
            get { return Common.ConvertDistance(_tdzRadiusDirection,RoundType.ToNearest); }
            set 
            {
                _tdzRadiusDirection =Common.DeConvertDistance(value);
                NotifyPropertyChanged("TdzRadiusDirection");
            }
        }
        
        public double TdzRadiusBackward
        {
            get { return Common.ConvertDistance(_tdzRadiusBackward,RoundType.ToNearest); }
            set 
            {
                _tdzRadiusBackward =Common.DeConvertDistance(value);
                NotifyPropertyChanged("TdzRadiusBackward");
            }
        }
       
        public double TdzWidth
        {
            get { return Common.ConvertDistance(_tdzWidth,RoundType.ToNearest); }
            set 
            {
                _tdzWidth =Common.DeConvertDistance(value);
                NotifyPropertyChanged("TdzWidth");
            }
        }

        public bool TdzCoordContEnabled
        {
            get { return _tdzCoordContEnabled; }
            set 
            {
                _tdzCoordContEnabled = value;
                NotifyPropertyChanged("TdzCoordContEnabled");
            }
        }

        public bool TdzCenterPointIsChecked
        {
            get { return _tdzCenterPointIsChecked; }
            set 
            {
                _tdzCenterPointIsChecked = value;
                TdzCoordContEnabled = !_tdzCenterPointIsChecked;
                if (_tdzCenterPointIsChecked)
                   Functions.SetTool();
                else
                   Functions.SetPreviousTool();
                NotifyPropertyChanged("TdzCenterPointIsChecked");
            }
        }
        
        public double CircleRadius
        {
            get { return Common.ConvertDistance(_circleRadius,RoundType.ToNearest); }
            set 
            {
                _circleRadius =Common.DeConvertDistance(value);
                if (PointList.Count>0)
                {
                    _curGeometry = Aran.PANDA.Common.ARANFunctions.CreateCircleAsMultiPolyPrj(PointList[0].PrjPoint, _circleRadius);
                    Draw();
                }
                NotifyPropertyChanged("CircleRadius");
            }
        }

        public string DistanceUnit
        {
            get {return InitDelta.DistanceConverter.Unit; }
        }

        public int ResolutionDecimalCount { get; set; }

        public double Resolution { get; set; }

        public double GlobalAccuracy { get { return GlobalParams.Accuracy; } }

        public bool IntersecIsChecked
        {
            get { return _intersecIsChecked; }
            set 
            {
                _intersecIsChecked = value;
                if (value)
                    _operationType = OperationType.Intersect;
            }
        }

        public bool ClipIsChecked
        {
            get { return _clipIsChecked; }
            set 
            {
                _clipIsChecked = value;
                if (value)
                    _operationType = OperationType.Clip;
            }
        }

        public bool UnionIsChecked
        {
            get { return _unionIsChecked; }
            set 
            {
                _unionIsChecked = value;
                if (value)
                    _operationType = OperationType.Union;
            }
        }
        
        public string AirspaceText
        {
            get { return _airspaceText; }
            set 
            {
                _airspaceText = value;
                if (_airspaceText.Length > 0)
                {
                    int formatNumber = -1;
                    _pointFormatList = Functions.ParseText(_airspaceText,ref formatNumber);
                    if (_pointFormatList.Count > 2)
                        SelectedWaterMark = WaterMarkList[formatNumber - 1];
                }
                NotifyPropertyChanged("AirspaceText");
            }
        }

        public NotamFormatClass SelectedWaterMark
        {
            get { return _selectedWaterMark; }
            set
            {
                _selectedWaterMark = value;
                NotifyPropertyChanged("SelectedWaterMark");
            }
        }

        #endregion

        #region :> Methods
        //needless
        public void Clear()
        {
            PointList.Clear();
            GlobalParams.UI.SafeDeleteGraphic(_curGeometryHandle);
            GlobalParams.UI.SafeDeleteGraphic(_curSelectedPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_ctrCenterPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_ctrCenterPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_selectedClipAirspaceHandle);
            GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
            Functions.SetPreviousTool();
            _geoBorderIsStarting = false;
        }
        //needless
        private void removePoint_onClick(object obj)
        {
            if (PointList.Count < 1)
                return;

            foreach (var areaPointModel in SelectedPointList)
            {
                //if (PointList.Count < 4)
                //    break;
                
                PointList.Remove(areaPointModel);
            }

            if (_selectedPoint != null)
                PointList.Remove(SelectedPoint);

           CreateGeometry();
        }
        //needless
        private void drawTdzCommand_onClick(object obj)
        {
            if (_tdzRadius < 0.001 || _tdzRadiusBackward < 0.01 || _tdzWidth < 0.01)
            {
                Model.Messages.Error("All parametrs must be set,before drawing area!");
                return;
            }

            var resultRing = new Aran.Geometries.Ring();

            var tdzRad =GlobalParams.SpatialRefOperation.AztToDirPrj(_tdzCenterPoint, _tdzDirection);
            if (_tdzWidth > _tdzRadiusDirection * 2)
            {
                Model.Messages.Error("Width/2 cannot be greater than R Direction)");
                return;
            }

            if (_tdzWidth > _tdzRadiusBackward * 2)
            {
                Model.Messages.Error("Width/2 cannot be greater than R Backward)");
                return;
            }

            if (_tdzWidth >= _tdzRadius * 2) 
            {
                MessageBox.Show("Width can not be greater than Circle diametr!");
                return;
            }

            if (_tdzRadiusDirection <= _tdzRadius)
            {
                MessageBox.Show("Direction radius can not be smaller than radius!");
                return;
            }

            if (_tdzRadiusBackward <= _tdzRadius)
            {
                MessageBox.Show("Backward radius can not be smaller than radius!");
                return;
            }

            var radDir = Math.Asin(_tdzWidth / (_tdzRadiusDirection * 2));

            var ptDir2 = ARANFunctions.LocalToPrj(_tdzCenterPoint, tdzRad + radDir, _tdzRadiusDirection, 0);
            var ptDir3 = ARANFunctions.LocalToPrj(_tdzCenterPoint, tdzRad - radDir, _tdzRadiusDirection, 0);

            var radBackward = Math.Asin(_tdzWidth / (_tdzRadiusBackward * 2));
            var ptBack2 = ARANFunctions.LocalToPrj(_tdzCenterPoint, tdzRad - radBackward, -_tdzRadiusBackward);
            var ptBack3 = ARANFunctions.LocalToPrj(_tdzCenterPoint, tdzRad + radBackward, -_tdzRadiusBackward);

            var ptDir1 = ARANFunctions.CircleVectorIntersect(_tdzCenterPoint, _tdzRadius, ptDir2, tdzRad);
            var ptBack1 = ARANFunctions.CircleVectorIntersect(_tdzCenterPoint, _tdzRadius, ptBack2, tdzRad + Math.PI);

            var ptDir4 = ARANFunctions.CircleVectorIntersect(_tdzCenterPoint, _tdzRadius, ptDir3, tdzRad);
            var ptBack4 = ARANFunctions.CircleVectorIntersect(_tdzCenterPoint, _tdzRadius, ptBack3, tdzRad + Math.PI);

            var arc1 = ARANFunctions.CreateArcAsPartPrj(_tdzCenterPoint, ptBack1, ptDir1, TurnDirection.CW);

            var arc2 = ARANFunctions.CreateArcAsPartPrj(_tdzCenterPoint, ptDir2, ptDir3, TurnDirection.CW);

            var arc3 = ARANFunctions.CreateArcAsPartPrj(_tdzCenterPoint, ptDir4, ptBack4, TurnDirection.CW);

            var arc4 = ARANFunctions.CreateArcAsPartPrj(_tdzCenterPoint, ptBack3, ptBack2, TurnDirection.CW);

            if (arc1.IsEmpty || arc2.IsEmpty || arc3.IsEmpty || arc4.IsEmpty)
            {
                Model.Messages.Error("System can not create TIZ geometry!Please change parametrs!");
                return;
            }

            resultRing.AddMultiPoint(arc1);
            resultRing.AddMultiPoint(arc2);
            resultRing.AddMultiPoint(arc3);
            resultRing.AddMultiPoint(arc4);
            var resultPoly = new Aran.Geometries.Polygon { ExteriorRing = resultRing };
            _curGeometry = new Aran.Geometries.MultiPolygon { resultPoly };
            //_selectedPoint = _tdzCenterPoint;
            Draw();
        }
        //needless
        private void DrawEllipse(object obj)
        {
            if (_tdzRadiusDirection < 0.001 || _tdzRadiusBackward < 0.01)
            {
                Model.Messages.Error("All parametrs must be set,before drawing area!");
                return;
            }

            if (_tdzCenterPoint == null || _tdzCenterPoint.IsEmpty)
            {
                Model.Messages.Error("Please set first center point!");
            }

            //var pt1 = Converters.ConvertToEsriGeom.FromPoint(ARANFunctions.LocalToPrj(_tdzCenterPoint, _tdzDirection, _tdzRadiusDirection, 0));
            //var pt2 = Converters.ConvertToEsriGeom.FromPoint((ARANFunctions.LocalToPrj(_tdzCenterPoint, _tdzDirection, -_tdzRadiusDirection, 0)));
            //var pt3 = Converters.ConvertToEsriGeom.FromPoint((ARANFunctions.LocalToPrj(_tdzCenterPoint, _tdzDirection, 0, _tdzRadiusBackward)));
            //var pt4 =Converters.ConvertToEsriGeom.FromPoint((ARANFunctions.LocalToPrj(_tdzCenterPoint, _tdzDirection, 0, -_tdzRadiusBackward)));
            var pt5 = new PointClass
            {
                X = _tdzCenterPoint.X,
                Y = _tdzCenterPoint.Y
            };


            IEnvelope pEnv = pt5.Envelope;
            double width = _tdzRadiusDirection/2;
            double height = _tdzRadiusBackward/2;

            pEnv.Expand(width,height,false);


            IConstructEllipticArc pConstructEllipticArc = new EllipticArcClass();
            pConstructEllipticArc.ConstructEnvelope(pEnv);


            //pConstructEllipticArc.ConstructUpToFivePoints(pt1, pt1, pt3,pt4, pt2);
            //ICurve ellipse = pConstructEllipticArc as ICurve;

            ISegmentCollection segColl = new RingClass();
            segColl.AddSegment(pConstructEllipticArc as ISegment);

            IPolygon poly = new PolygonClass();
            IGeometryCollection geomColl = poly as IGeometryCollection;
            geomColl.AddGeometry(segColl as IRing);


            var azimuth =ARANMath.DegToRad(GlobalParams.SpatialRefOperation.DirToAztPrj(_tdzCenterPoint,ARANMath.DegToRad(_tdzDirection)));

            ITransform2D pTrans2D = poly as ITransform2D;
            pTrans2D.Rotate(pt5,azimuth);

           

            poly.Densify(10,0.1);

            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)poly;
            IGeometry convexGeom = topoOper2.ConvexHull();
            SimplifyGeometry(convexGeom);

            IPointCollection ptCollection = poly as IPointCollection;

            if (_curGeometry==null)
                _curGeometry = new MultiPolygon();

            _curGeometry.Clear();

            var ring = new Aran.Geometries.Ring();
            for (int i = 0; i < ptCollection.PointCount; i++)
            {
                ring.Add(new Aran.Geometries.Point(ptCollection.Point[i].X, ptCollection.Point[i].Y));
            }

            _curGeometry.Add(new Aran.Geometries.Polygon {ExteriorRing = ring});

            Draw();
        }
        //needless
        private void continue_onClick(object obj)
        {
           Functions.SetTool();
        }
        //done
        private void saveCommand_onClick(object obj)
        {
            if (_curGeometry != null && !_curGeometry.IsEmpty)
            {
                var mlt = _curGeometry.ToMultiPoint();
                if (mlt.Count < 3) return;

                double maxAccuracy = 0;
                double maxResolution = 0;

                if (_selectedGeoType == CreatingGeoType.Ctr || _selectedGeoType == CreatingGeoType.Ellipse)
                {
                    maxResolution = _tdzCenterPointModel.Resolution;
                    maxAccuracy = _tdzCenterPointModel.Accuracy;
                }
                else
                {
                    foreach (var pointModel in PointList)
                    {
                        if (pointModel.Accuracy > maxAccuracy)
                            maxAccuracy = pointModel.Accuracy;

                        if (pointModel.Resolution > maxResolution)
                            maxResolution = pointModel.Resolution;
                    }
                }

                var numericalData = new List<GeoNumericalDataModel>()
                {
                    new GeoNumericalDataModel
                    {
                        Accuracy = maxAccuracy,
                        Resolution = maxResolution
                    }
                };

                GlobalParams.Database.DeltaQPI.ClearAllFeatures();

                var airspace = GlobalParams.Database.DeltaQPI.CreateFeature<Airspace>();
                var note = new Note { Purpose = Aim.Enums.CodeNotePurpose.REMARK };
                var linguisticNote = new LinguisticNote { Note = new Aim.DataTypes.TextNote() };
                var noteText = "Has created by Delta!";

                linguisticNote.Note.Value = noteText;
                note.TranslatedNote.Add(linguisticNote);
                airspace.Annotation.Add(note);

                var airspaceComponent = new AirspaceGeometryComponent { TheAirspaceVolume = new AirspaceVolume() };
                airspaceComponent.TheAirspaceVolume.HorizontalProjection = new Surface();

                var airsapceGeo = GlobalParams.SpatialRefOperation.ToGeo(_curGeometry);
                foreach (Geometries.Polygon geom in airsapceGeo)
                {
                    airspaceComponent.TheAirspaceVolume.HorizontalProjection.Geo.Add(geom);
                    airspaceComponent.TheAirspaceVolume.HorizontalProjection.HorizontalAccuracy = new ValDistance {
                        Value = maxAccuracy,
                        Uom = UomDistance.M
                    };
                }

                airspace.GeometryComponent.Add(airspaceComponent);
                GlobalParams.Database.DeltaQPI.SetFeature(airspace);
                GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.Airspace);

                var spatialReference = GlobalParams.AranEnv.Graphics.ViewProjection;

                if (GlobalParams.AranEnv.DbProvider is DbProvider dbProvider)
                {
                    bool result;
                    if (dbProvider.ProviderType == DbProviderType.TDB && GlobalParams.AranEnv.UseWebApi)
                    {
                        result = GlobalParams.Database.DeltaQPI.CommitWithMetadataViewer(
                            GlobalParams.AranEnv.Graphics.ViewProjection.Name, numericalData,
                            dbProvider.ProviderType != DbProviderType.TDB);
                    }
                    else
                    {
                        result = GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                    }

                    if (result)
                    {
                        GlobalParams.Database.DeltaQPI.ExcludeFeature(airspace.Identifier);
                        Model.Messages.Info("Airspace successfully saved to Aixm 5.1 DB");
                    }
                }
            }
            else
            {
                Model.Messages.Warning("First create area,then save!");
            }
        }
        //needless
        private void SaveToArena(object obj)
        {
            try
            {
                if (_curGeometry != null && !_curGeometry.IsEmpty)
                {
                    var mlt = _curGeometry.ToMultiPoint();
                    if (mlt.Count < 3) return;

                    var designingArea = new Model.DesigningArea();
                    designingArea.Geo = GlobalParams.SpatialRefOperation.ToGeo(_curGeometry);

                    if (GlobalParams.DesigningAreaReader.SaveArea(designingArea))
                    {
                        Aran.Delta.Model.Messages.Info("Feature saved database successfully");
                        SaveAirspaceVertexToDocument(designingArea.Geo as Aran.Geometries.MultiPolygon,designingArea.Name);
                        Clear();
                        Functions.SaveArenaProject();
                    }
                }
                else
                {
                    Model.Messages.Warning("Area cannot be empty!Please firs create Area then Save to Database!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error appered when trying save feature to DB!" + e.Message);
            }
        }
        //done
        private void updatePoint_onClick(object obj)
        {
            if (_selectedPoint != null)
            {
                var newPtPrj = GlobalParams.SpatialRefOperation.ToPrj(new Aran.Geometries.Point(AddedLongtitude, AddedLatitude));

                _selectedPoint.Latitude = AddedLatitude;
                _selectedPoint.Longtitude = AddedLongtitude;

                Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(AddedLongtitude, AddedLatitude, ".", "E", "N", 1,
                    Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out var _longStr,
                    out var _latStr);
                
                _selectedPoint.LatStr = _latStr;
                _selectedPoint.LongStr = _longStr;

                _selectedPoint.Resolution = Resolution;
                _selectedPoint.Accuracy = GlobalParams.Accuracy;

                if (_selectedGeoType== CreatingGeoType.Line || _selectedGeoType== CreatingGeoType.Text)
                {
                    _selectedPoint.PrjPoint = newPtPrj;

                    CreateGeometry();
                }
                else if (_selectedGeoType == CreatingGeoType.Circle) 
                {
                    _selectedPoint.PrjPoint = newPtPrj;
                    _curGeometry = ARANFunctions.CreateCircleAsMultiPolyPrj(newPtPrj, _circleRadius);
                }
               
                Draw();
            }
        }
        //needless
        public void Draw()
        {
            GlobalParams.UI.SafeDeleteGraphic(_curSelectedPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_curGeometryHandle);
        
            if (_selectedPoint != null)
                _curSelectedPointHandle = GlobalParams.UI.DrawPointPrj(_selectedPoint.PrjPoint, _pointSymbol);
            
            if (_curGeometry!=null)
                _curGeometryHandle = GlobalParams.UI.DrawDefaultMultiPolygon(_curGeometry);
        }
        //done
        private void DrawTdzGeo()
        {
            try
            {
                var ptPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(new Geometries.Point(TdzLongtitude, TdzLatitude));
                if (_tdzCenterPoint == null)
                    _tdzCenterPoint = new Geometries.Point();
                if (ptPrj.IsEmpty)
                    return;

                _tdzCenterPoint.X = ptPrj.X;
                _tdzCenterPoint.Y = ptPrj.Y;

                _tdzCenterPointModel = new Model.AreaPointModel();
                _tdzCenterPointModel.Latitude = TdzLatitude;
                _tdzCenterPointModel.Longtitude = TdzLongtitude;
                _tdzCenterPointModel.Type = "CenterPoint";
                _tdzCenterPointModel.PrjPoint = _tdzCenterPoint;

                _tdzCenterPointModel.Resolution = Resolution;
                _tdzCenterPointModel.Accuracy = GlobalParams.Accuracy;

                GlobalParams.UI.SafeDeleteGraphic(_ctrCenterPointHandle);
                _ctrCenterPointHandle = GlobalParams.UI.DrawPointPrj(_tdzCenterPoint);
            }
            catch (Exception)
            {
                
            }
          
        }
        //done
        private void addPoint_onClick(object obj)
        {
            var newPointModel = new Model.AreaPointModel();
            Aran.Geometries.Point prjPt = GlobalParams.SpatialRefOperation.ToPrj(new Aran.Geometries.Point(AddedLongtitude, AddedLatitude));
            newPointModel.Latitude = AddedLatitude;
            newPointModel.Longtitude = AddedLongtitude;
            newPointModel.Type = "Line";
            newPointModel.PrjPoint = prjPt;

            Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(AddedLongtitude, AddedLatitude, ".", "E", "N", 1,
                Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out var _longStr,
                out var _latStr);

            newPointModel.LatStr = _latStr;
            newPointModel.LongStr = _longStr;

            newPointModel.Resolution = Resolution;
            newPointModel.Accuracy = GlobalParams.Accuracy;

            if (SelectedPoint!=null)
                PointList.Insert(SelectedPoint.Index,newPointModel);
            else
                PointList.Add(newPointModel);
            
            CreateGeometry();
            Draw();
            
        }
        //needless
        private void clearCommand_onClick(object obj)
        {
            PointList.Clear();
            GlobalParams.UI.SafeDeleteGraphic(_curGeometryHandle);
            GlobalParams.UI.SafeDeleteGraphic(_curSelectedPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_ctrCenterPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_selectedClipAirspaceHandle);
            _curGeometry.Clear();
            _geoBorderIsStarting = false;
            _firstBorderIndex = -1;
            //if (_drawArcByRadiusIsChecked || _drawArcThreeIsChecked || _drawCircleIsChecked || _drawLineIsChecked)
            //    GlobalParams.AranEnv.AranUI.SetCurrentTool(GlobalParams.AranMapToolMenuItem);
        }
        //needless
        private void CreateGeometry(bool isMoving, Aran.Geometries.Point movePoint)
        {
            GlobalParams.UI.SafeDeleteGraphic(_curGeometryHandle);
            if (isMoving)
                _pointList.Add(movePoint);

            if (_pointList.Count == 1)
                _curGeometryHandle = GlobalParams.UI.DrawPointPrj(_pointList[0], GlobalParams.Settings.SymbolModel.ResultPointSymbol);
            else if (_pointList.Count == 2)
            {
                Aran.Geometries.MultiLineString mltString = new Geometries.MultiLineString 
                {
                    new Aran.Geometries.LineString { _pointList[0], _pointList[1] } 
                };
                _curGeometryHandle = GlobalParams.UI.DrawMultiLineStringPrj(mltString, GlobalParams.Settings.SymbolModel.LineDistanceSymbol);
            }
            else if (_pointList.Count > 2)
            {
               // if (_curGeometry != null)
                 //   GlobalParams.UI.DrawMultiPolygon(_curGeometry, true);

                var poly = new Aran.Geometries.Polygon();
                poly.ExteriorRing = new Geometries.Ring();
                foreach (var point in _pointList)
                    poly.ExteriorRing.Add(point);

                poly.ExteriorRing.Add(_pointList[0]);

                _curGeometry = new Geometries.MultiPolygon();
                _curGeometry.Add(poly);
                _drawingHandle = GlobalParams.UI.DrawDefaultMultiPolygon(_curGeometry,_drawingHandle);
                //GlobalParams.UI.DrawMultiPolygon(_curGeometry, false);
            }

            if (isMoving)
                _pointList.RemoveAt(_pointList.Count - 1);

        }
        //needless
        private void UpdateButtons()
        {
            GlobalParams.UI.SafeDeleteGraphic(_curSelectedPointHandle);
            _isFinished = false;
            //CircleRadiusIsVisible = _drawCircleIsChecked ? Visibility.Visible : Visibility.Hidden;

            AddPointIsEnabled = _selectedGeoType != CreatingGeoType.Circle;
            NotifyPropertyChanged("DrawArcThreeIsChecked");
            NotifyPropertyChanged("DrawEllipseIsChecked");
            NotifyPropertyChanged("DrawLineIsChecked");
            NotifyPropertyChanged("DrawCircleIsChecked");
            NotifyPropertyChanged("DrawCtrIsChecked");
            NotifyPropertyChanged("DrawArcByRadiusIsChecked");
            NotifyPropertyChanged("DrawTextAirspaceIsChecked");
            
            NotifyPropertyChanged("MovingPointIsVisible");
            NotifyPropertyChanged("PointListIsVisible");
            NotifyPropertyChanged("TdzCreateIsVisible");
            NotifyPropertyChanged("EllipseIsVisible");
            NotifyPropertyChanged("CircleRadiusIsVisible");
            NotifyPropertyChanged("TextFormatIsVisible");

        }
        //needless
        private void AranMapToolMenuItem_Move(object sender, AranEnvironment.MapMouseEventArg arg)
        {
            ESRI.ArcGIS.Geometry.IPoint movePt = new ESRI.ArcGIS.Geometry.Point();
            movePt.X = arg.X;
            movePt.Y = arg.Y;

            ISnappingResult snapResult = null;

            //Try to snap the current position

            snapResult = GlobalParams.FetureSnap.Snapper.Snap(movePt);

            GlobalParams.FetureSnap.SnappingFeedback.Update(snapResult, 0);

            if (snapResult != null)
            {//Snapping occurred

                //Set the current position to the snapped location
                _isSnapped = true;
                _snappedEsriPoint = snapResult.Location;
            }
            else
            {
                _isSnapped = false;
            }

            var geoPt = GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(movePt.X, movePt.Y));

            if (double.IsNaN(geoPt.X))
                return;

            MovingLatitude = geoPt.Y;
            MovingLongtitude = geoPt.X;

            if (!_isFirstClick)
            {
                if (_selectedGeoType== CreatingGeoType.Line && _polygonFeedback != null)
                {
                    if (_isSnapped)
                    {
                        if (_geoBorderIsStarting)
                        {
                           // AranMapToolMenuItem_Click(sender, arg);
                        }
                        else
                        {
                          //  _polygonFeedback.MoveTo(_snappedEsriPoint);
                        }
                        _polygonFeedback.MoveTo(_snappedEsriPoint);
                    }
                    else
                        _polygonFeedback.MoveTo(movePt);
                }
                else if (_selectedGeoType== CreatingGeoType.Circle && _lineFeedback != null)
                {
                    if (_isSnapped)
                        _lineFeedback.MoveTo(_snappedEsriPoint);
                    else
                        _lineFeedback.MoveTo(movePt);
                }
            }
            //Do something with the polyline here

        }
        //done
        private void AranMapToolMenuItem_Click(object sender, AranEnvironment.MapMouseEventArg arg)
        {
            var actualAccuracy = 0.0005 * GlobalParams.Map.MapScale;
            
            if (!Common.TryCalculateResolutionBasedOnAccuracy(actualAccuracy, out var actualResolution) || Resolution < actualResolution)
            {
                //todo clear exist point
                //Clear();
                Common.CalculateSuggestedScaleBasedOnResolution(Resolution, out var fromScale, out var toScale);
                Model.Messages.Error("Resolution is not acceptable in this map scale. Suggested scale range is 1:" + fromScale + ((toScale != null) ? " - 1:" + toScale : " and less"));
                return;
            }

            IPoint point = new ESRI.ArcGIS.Geometry.Point
            {
                X = arg.X,
                Y = arg.Y
            };

            if (_selectedGeoType == CreatingGeoType.Line)
            {
                if (_polygonFeedback == null) return;

                var tmpEsriPt = point;
                if (_isSnapped)
                    tmpEsriPt = _snappedEsriPoint;

                if (arg.Button == MouseButtons.Right)
                {
                    if (_snapAirspaceType == CreatingAreaType.Border)
                    {
                        if (_geoBorderIsStarting)
                        {
                            if (_firstBorderIndex > -1)
                                _secondBorderIndex = FindSelectedVertex();
                         
                            _geoBorderIsStarting = false;

                        }
                        else
                        {
                            _geoBorderIsStarting = _isSnapped;
                            if (_geoBorderIsStarting)
                                _firstBorderIndex = FindSelectedVertex();
                        }
                    }

                    else if (_snapAirspaceType == CreatingAreaType.Arc)
                    {

                        var tmpArcPt = point;

                        if (_isSnapped)
                        {
                            tmpArcPt.X = _snappedEsriPoint.X;
                            tmpArcPt.Y = _snappedEsriPoint.Y;
                        }
                        //_airspaceTypeView.PointChanged(tmpArcPt);
                        return;
                    }
                    else
                    {
                        _geoBorderIsStarting = false;
                        _firstBorderIndex = -1;
                        _secondBorderIndex = -1;
                    }
                }

                var prjPt = new Aran.Geometries.Point(tmpEsriPt.X, tmpEsriPt.Y);
                var geoPt = GlobalParams.SpatialRefOperation.ToGeo(prjPt);
                var pointModel = new Model.AreaPointModel
                {
                    Latitude = geoPt.Y,
                    Longtitude = geoPt.X,
                    Type = "Line",
                    PrjPoint = prjPt,

                    Accuracy = actualAccuracy,
                    Resolution = Resolution
                };

                PointList.Add(pointModel);

                if (_isFirstClick)
                {
                    if (_isSnapped)
                        _polygonFeedback.Start(_snappedEsriPoint);
                    else
                        _polygonFeedback.Start(point);
                    _isFirstClick = false;
                }
                else
                {
                    if (_isSnapped)
                        _polygonFeedback.AddPoint(_snappedEsriPoint);
                    else
                        _polygonFeedback.AddPoint(point);
                }
            }
            else if (_selectedGeoType == CreatingGeoType.Circle)
            {
                if (_lineFeedback == null) return;

                var circualArc = _lineFeedback.Stop();
                if (circualArc != null)
                {
                    var centerPt = new Aran.Geometries.Point(circualArc.CenterPoint.X, circualArc.CenterPoint.Y);
                    CircleRadius = Common.ConvertDistance(circualArc.Radius, RoundType.RealValue);

                    var centerPtGeo = GlobalParams.SpatialRefOperation.ToGeo(centerPt);
                    var pointModel = new Model.AreaPointModel
                    {
                        Latitude = centerPtGeo.Y,
                        Longtitude = centerPtGeo.X,
                        Type = "Circle",
                        PrjPoint = centerPt,

                        Accuracy = actualAccuracy,
                        Resolution = Resolution
                    };

                    PointList.Add(pointModel);

                    var circleGeo = Aran.PANDA.Common.ARANFunctions.CreateCircleAsMultiPolyPrj(centerPt, circualArc.Radius);
                    _curGeometry = circleGeo;
                    Draw();
                    //DrawCircleIsChecked = false;
                }
                _lineFeedback = null;
               Functions.SetPreviousTool();

                _isFirstClick = true;
                _isFinished = true;
                SelectedAirspaceType = 0;
                NotifyPropertyChanged("MovingPointIsVisible");
                NotifyPropertyChanged("PointListIsVisible");
            }
            else if (_selectedGeoType == CreatingGeoType.Ctr)
            {
                _tdzCenterPoint =new Geometries.Point(point.X,point.Y);
                if (_isSnapped)
                    _tdzCenterPoint =new Geometries.Point(_snappedEsriPoint.X,_snappedEsriPoint.Y);
                
                var geoPt = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point>(_tdzCenterPoint);
                TdzLatitude = geoPt.Y;
                TdzLongtitude = geoPt.X;

                _tdzCenterPointModel = new Model.AreaPointModel
                {
                    Latitude = geoPt.Y,
                    Longtitude = geoPt.X,
                    Type = "CenterPoint",
                    PrjPoint = _tdzCenterPoint,

                    Accuracy = actualAccuracy,
                    Resolution = Resolution
                };

            }
            else if (_selectedGeoType == CreatingGeoType.Ellipse)
            {
                _tdzCenterPoint = new Geometries.Point(point.X, point.Y);
                if (_isSnapped)
                    _tdzCenterPoint = new Geometries.Point(_snappedEsriPoint.X, _snappedEsriPoint.Y);
                var geoPt = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point>(_tdzCenterPoint);
                TdzLatitude = geoPt.Y;
                TdzLongtitude = geoPt.X;


                _tdzCenterPointModel = new Model.AreaPointModel
                {
                    Latitude = geoPt.Y,
                    Longtitude = geoPt.X,
                    Type = "CenterPoint",
                    PrjPoint = _tdzCenterPoint,

                    Accuracy = actualAccuracy,
                    Resolution = Resolution
                };
            }
            else if (_selectedGeoType == CreatingGeoType.ArcByRadius)
            {
                if (_arcFeedback == null) return;

                var tmpEsriPt = point;
                if (_isSnapped)
                    tmpEsriPt = _snappedEsriPoint;

                var prjPt = new Aran.Geometries.Point(tmpEsriPt.X, tmpEsriPt.Y);
                var geoPt = GlobalParams.SpatialRefOperation.ToGeo(prjPt);
                var pointModel = new Model.AreaPointModel
                {
                    Latitude = geoPt.Y,
                    Longtitude = geoPt.X,
                    Type = "Line",
                    PrjPoint = prjPt,

                    Accuracy = actualAccuracy,
                    Resolution = Resolution
                };

                PointList.Add(pointModel);

                if (_isFirstClick)
                {
                    if (_isSnapped)
                        _arcFeedback.Start(_snappedEsriPoint);
                    else
                        _arcFeedback.Start(point);
                    _isFirstClick = false;
                }
                else
                {
                    if (_isSnapped)
                        _arcFeedback.SetMidpoint(_snappedEsriPoint);
                    else
                        _arcFeedback.SetEndpoint(point);
                }
                _arcFeedback.Stop(point, out ICircularArc circularArc);
            }
        }
        //needless
        private void AranMapToolMenuItem_MouseDown(object sender, AranEnvironment.MapMouseEventArg arg)
        {
            if (_selectedGeoType != CreatingGeoType.Circle) return;

            IPoint point = new ESRI.ArcGIS.Geometry.Point();
            point.X = arg.X;
            point.Y = arg.Y;

            if (_lineFeedback == null)
            {
                _lineFeedback = new NewCircleFeedbackClass();
            }
            _lineFeedback.Display = _screenDisp;
            if (_isSnapped)
                _lineFeedback.Start(_snappedEsriPoint);
            else
                _lineFeedback.Start(point);
            _isFirstClick = false;
        }
        //needless
        private void AranMapToolMenuItem_RightClick()
        {
            Clear();
        }
        //done
        private int FindSelectedVertex()
        {
            if (_selectedFirAirspace == null) return -1;

            var snapPt = new Aran.Geometries.Point(_snappedEsriPoint.X, _snappedEsriPoint.Y);

           // var geoSnapPt= GlobalParams.SpatialRefOperation.ToGeo(snapPt);
            int index = -1;
            int i = 0;

            foreach (Aran.Geometries.Point pt in _firMltPrj)
            {
                if (ARANFunctions.ReturnDistanceInMeters(pt, snapPt) < 100)
                {
                    index = i;
                    break;
                }
                i++;
            }

            if (_firstBorderIndex > -1 && index > -1)
            {
                var borderLine = new Aran.Geometries.LineString();
                if (_firstBorderIndex < index)
                {
                    for (int j = _firstBorderIndex; j <= index; j++)
                    {
                        
                        IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                        esriPt.X = _firMltPrj[j].X;
                        esriPt.Y = _firMltPrj[j].Y;
                        borderLine.Add(_firMltPrj[j]);

                        _polygonFeedback.MoveTo(esriPt);
                        _polygonFeedback.AddPoint(esriPt);

                        //PointList.Add(pointModel);
                    }
                }
                else
                {
                    for (int j = _firstBorderIndex; j < _firMltPrj.Count; j++)
                    {
                        IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                        esriPt.X = _firMltPrj[j].X;
                        esriPt.Y = _firMltPrj[j].Y;
                        borderLine.Add(_firMltPrj[j]);

                        _polygonFeedback.MoveTo(esriPt);
                        _polygonFeedback.AddPoint(esriPt);
                    }

                    for (int j = 0; j < index; j++)
                    {
                        IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                        esriPt.X = _firMltPrj[j].X;
                        esriPt.Y = _firMltPrj[j].Y;
                        borderLine.Add(_firMltPrj[j]);

                        _polygonFeedback.MoveTo(esriPt);
                        _polygonFeedback.AddPoint(esriPt);
                    }

                }
                var lnString = GlobalParams.SpatialRefOperation.ToGeo(borderLine);

                if (!lnString.IsEmpty)
                {
                    int k = 0;
                    foreach (Aran.Geometries.Point geoPt in lnString)
                    {
                        var pointModel = new Model.AreaPointModel();
                        pointModel.Latitude = geoPt.Y;
                        pointModel.Longtitude = geoPt.X;
                        pointModel.Type = "Border";
                        pointModel.PrjPoint = borderLine[k];

                        pointModel.Accuracy = GlobalParams.Accuracy;
                        pointModel.Resolution = Resolution;

                        PointList.Add(pointModel);
                        k++;
                    }
                }
                _firstBorderIndex = index;
                _secondBorderIndex = -1;
            }

            return index;
        }
        //needless
        private void AranMapToolMenuItem_OnDblClick()
        {
            ICircularArc circle;
            IPolygon poly;
            _isFirstClick = true;

            if (_polygonFeedback != null)
            {
                poly = _polygonFeedback.Stop();

                if (poly==null || poly.IsEmpty)
                    return;

                CreateGeometry();

                _polygonFeedback = null;
               Functions.SetPreviousTool();

                Draw();
                _isFinished = true;
                SelectedAirspaceType = 0;
                NotifyPropertyChanged("MovingPointIsVisible");
                NotifyPropertyChanged("PointListIsVisible");
            }

        }
        //needless
        private void SimplifyGeometry(IGeometry geom)
        {
            ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)geom;
            topoOper2.IsKnownSimple_2 = false;
            topoOper2.Simplify();
        }
        //needless
        private void CreateGeometry()
        {
            var extRing = new Aran.Geometries.Ring();
            foreach (var ptModel in PointList)
            {
                extRing.Add(ptModel.PrjPoint);
            }
            _curGeometry = new Aran.Geometries.MultiPolygon();
            _curGeometry.Add(new Aran.Geometries.Polygon{ExteriorRing = extRing});
            Draw();
        }
        //done
        private void Operation_onClick(object obj)
        {
            try
            {
                if (SelectedClipAirspace != null && _curGeometry != null)
                {
                    Aran.Geometries.Geometry operResultGeom = null;
                    if (_operationType == OperationType.Clip)
                    {
                        operResultGeom = GlobalParams.GeometryOperators.Difference(_curGeometry,
                            _selectedOperAirspaceGeom);
                    }
                    else if (_operationType == OperationType.Union)
                    {
                        operResultGeom = GlobalParams.GeometryOperators.UnionGeometry(_curGeometry,
                            _selectedOperAirspaceGeom);
                    }
                    else if (_operationType == OperationType.Intersect)
                    {
                        operResultGeom = GlobalParams.GeometryOperators.Intersect(_curGeometry,
                            _selectedOperAirspaceGeom);
                    }

                    if (operResultGeom != null)
                    {
                        var isAirspaceAccuracyNull = false;
                        var isAirspaceAccuracyValid = true;
                        double? actualAccuracy = null, actualResolution = null;

                        foreach (var geomComponent in SelectedClipAirspace.GeometryComponent)
                        {
                            var horizontalAccuracy = geomComponent.TheAirspaceVolume?.HorizontalProjection?.HorizontalAccuracy;
                            if (horizontalAccuracy == null)
                            {
                                isAirspaceAccuracyNull = true;
                                break;
                            }

                            actualAccuracy = horizontalAccuracy.Value;
                            if (Common.TryCalculateResolutionBasedOnAccuracy(actualAccuracy.Value, out var tempActualAccuracy))
                                actualResolution = tempActualAccuracy;

                            if (actualResolution == null || Resolution < actualResolution)
                                isAirspaceAccuracyValid = false;
                        }

                        if (isAirspaceAccuracyNull)
                        {
                            if (MessageBox.Show("Selected airspace has geometry component without valid accuracy. If you proceed, new geometry will be without accuracy and resolution. Continue?",
                                "Accuracy warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return;

                            actualAccuracy = actualResolution = null;
                        }
                        else if(!isAirspaceAccuracyValid)
                        {
                            if (MessageBox.Show("Selected airspace has geometry with resolution worse than " + Resolution + " second. If you proceed, new geometry will have worse accuracy and resolution. Continue?",
                                "Accuracy warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                                return;
                        }

                        if (operResultGeom.Type == GeometryType.MultiPolygon)
                        {
                            _curGeometry = (Aran.Geometries.MultiPolygon) operResultGeom;
                            Draw();
                        }
                        else if (operResultGeom.Type == GeometryType.Polygon)
                        {
                            _curGeometry = new MultiPolygon {(Aran.Geometries.Polygon) operResultGeom};
                            Draw();
                        }

                        CreatePointListFromGeo(_operationType, actualAccuracy, actualResolution);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Messages.Error(e.Message);
            }
        }
        //done
        void CreatePointListFromGeo(OperationType operation, double? actualAccuracy, double? actualResolution)
        {
            PointList.Clear();
            if (_curGeometry == null || _curGeometry.IsEmpty) return;
            foreach (var poly in _curGeometry)
            {
                var mlt = _curGeometry.ToMultiPoint();
                foreach (Aran.Geometries.Point ptPrj in mlt)
                {
                    var ptGeo = GlobalParams.SpatialRefOperation.ToGeo(ptPrj);
                    var ptModel = new Model.AreaPointModel
                    {
                        Latitude = ptGeo.Y,
                        Longtitude = ptGeo.X,
                        Type = operation.ToString(),
                        PrjPoint = ptPrj,
                    };

                    if(actualAccuracy != null)
                    {
                        ptModel.Accuracy = actualAccuracy.Value;
                        ptModel.Resolution = actualResolution.Value;
                    }

                    PointList.Add(ptModel);
                }
            }

        }
        //needless
        void PointList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < PointList.Count; i++)
            {
                PointList[i].Index = i + 1;
            }
        }
        //needless
        private void SaveAirspaceVertexToDocument(Aran.Geometries.MultiPolygon geometry,string projectName)
        {
            try
            {
                if (geometry == null) return;
                var deltaInterface = GlobalParams.Settings.DeltaInterface;

                var arenaDbModule = GlobalParams.Database as ArenaDBModule;

                if (arenaDbModule == null)
                {
                    Delta.Model.Messages.Warning("Project path is empty!");
                    return;
                }
                var filePath = (GlobalParams.Database as ArenaDBModule).ProjectPath + @"\" + projectName+".txt";

                using (StreamWriter writetext = new StreamWriter(filePath, false))
                {
                    //if polygon is circle

                    if (_selectedGeoType == CreatingGeoType.Circle)
                    {
                        if (PointList.Count == 0)
                        {
                            Model.Messages.Warning("Circle is empty!Cannot export airspace to file!");
                            return;
                        }
                        var circleCenter = PointList[0];
                        var pt = (Aran.Geometries.Point)GlobalParams.SpatialRefOperation.ToGeo(circleCenter.PrjPoint);

                        string lat, longtitude;
                        Functions.Dd2DmsStr(pt.X, pt.Y, ".", "E", "N", 1,
                                    Convert.ToInt32(deltaInterface.CoordinatePrecision), out lat, out longtitude);

                        writetext.WriteLine("Circle with radius " + CircleRadius + " " + deltaInterface.DistanceUnit
                            + " Centered " + longtitude + "  " + lat);
                    }
                    else
                    {
                        string lat, longtitude;
                        foreach (var ptModel in PointList)
                        {
                            var pt = (Aran.Geometries.Point)GlobalParams.SpatialRefOperation.ToGeo(ptModel.PrjPoint);

                            Functions.Dd2DmsStr(pt.X, pt.Y, ".", "E", "N", 1,
                                    Convert.ToInt32(deltaInterface.CoordinatePrecision), out longtitude, out lat);

                            writetext.WriteLine(lat + " " + longtitude);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Model.Messages.Error(e.Message);
            }
        }
        //done
        private void ViewTextFormatGeo(object obj)
        {
            try
            {
                if (_airspaceText.Length > 0 && _pointFormatList.Count <= 2)
                {
                    Messages.Error("Format is not correct!");
                    return;
                }


                var newPointModelList = new List<Model.AreaPointModel>();
                _curGeometry = new MultiPolygon();
                foreach (var ptFormat in _pointFormatList)
                {
                    var latitude = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd(ptFormat.Y, true);
                    var longtitude = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd(ptFormat.X, false);

                    var actualResolution = Common.CalculateResolutionBasedOnPoint(latitude, longtitude);

                    if (Resolution < actualResolution)
                    {
                        Model.Messages.Error("Resolution is not acceptable, it must be less than " + Resolution);
                        return;
                    }

                    var ptPrj = GlobalParams.SpatialRefOperation.ToPrj(new Aran.Geometries.Point(longtitude, latitude));

                    var newPointModel = new Model.AreaPointModel()
                    {
                        Latitude = latitude,
                        Longtitude = longtitude,
                        Type = "Text",
                        PrjPoint = ptPrj
                    };
                    
                    newPointModel.Resolution = Resolution;
                    newPointModelList.Add(newPointModel);
                }

                PointList.Clear();
                newPointModelList.ToList().ForEach(PointList.Add);

                CreateGeometry();

                SelectedAirspaceType = 0;

                NotifyPropertyChanged("TextFormatIsVisible");
                NotifyPropertyChanged("PointListIsVisible");
            }
            catch (Exception e)
            {
                Messages.Error("Format is not correct!");
            }
        }
        //needless
        private void close_onClick(object obj)
        {
            Clear();
            Close();
        }
        //needless
        private void AirspaceTypeView_AirsapceCreationTypeIsChanged(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            _snapAirspaceType = (Enums.CreatingAreaType)sender;
        }
        //needless
        private void DrawBuffer()
        {

            //Test circle
            //ISegmentCollection ring1 = new RingClass();
            //ring1.AddSegment(circualArc as ISegment);
            //IPolygon poly = new PolygonClass();
            //IGeometryCollection geometryCollection = poly as IGeometryCollection;
            //geometryCollection.AddGeometry(ring1 as IGeometry);

            ////GlobalParams.UI.DrawEsriDefaultMultiPolygon(poly as IPolygon);

            //ITopologicalOperator2 topopOperator6 = poly as ITopologicalOperator2;
            //var bufferGeom =topopOperator6.Buffer(5000);


            //var esriGeo = Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(_selectedOperAirspaceGeom);

            // var result = topopOperator6.Intersect(esriGeo,esriGeometryDimension.esriGeometry2Dimension);

            //GlobalParams.UI.DrawEsriDefaultMultiPolygon(result as IPolygon);

            //end
            
        }
        #endregion
    }
}
