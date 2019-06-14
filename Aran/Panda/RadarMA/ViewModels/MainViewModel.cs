using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Aran.AranEnvironment;
using Aran.Converters;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Panda.RadarMA.Models;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.SpatialAnalystTools;
using Polygon = ESRI.ArcGIS.Geometry.Polygon;
using System.Windows.Interop;
using Aran.Panda.RadarMA.ElevationCalculator;
using Aran.Panda.RadarMA.Utils;
using UnitConverter = Aran.PANDA.Common.UnitConverter;

namespace Aran.Panda.RadarMA.ViewModels
{
    internal enum CreatingGeoType
    {
        Line,
        Circle,
    }

    public class MainViewModel:NotifyableBase
    {
        private const double RadarDistanceConst = 55600;
        private const double FtToM = 0.3014;
        private const double NmToM = 1852;

        #region :>Fields

        private CreatingGeoType _creatingGeoType;

        private RadarPoint _selectedAdhp;
        private RadarPoint _selectedRadarPoint;
        private List<RadarPoint> _allRadarPointsList;
        private List<int> _radarCircleHandles;

        private double _radarAreaRadius;
        private double _magVar;

        private int _stepNumber;
        private int _radarPointHandle;
        private int _circleHandle;
        private bool _createSectorIsChecked;
        private ICommandItem _drawLineCommandItem;
        private NewPolygonFeedback _polygonFeedback;
        private INewCircleFeedback _cirleNewFeedback;
        private int _pointCount;

        private IPolygon _unionGeometry;
        private IGeometry _circleGeo;
        private Sector _selectedSector;
        private double _filterCircleRadius;
        private TerrainClass _selectedRaster;
        private bool _isSnapped;
        private IPoint _snappedEsriPoint;

        private double _radarOperationDistance;
        private readonly PANDA.Common.UnitConverter _unitConverter;
        private double _elevationRounding;
        private double _selectedBufferValue;
        private double _selectedMocValue;
        private double _sectorElevation;
        private State _curState;
        private bool _createSectorIsEnabled;
        private bool _filterByDistanceIsChecked;
        private int _filterCircleHandle;
        private bool _drawBufferIsChecked;
        private IPolygon _filterCircleGeo;
        private bool _differFromSectorIsEnabled;
        private bool _differFromRadarOperAreaIsEnabled;
        private bool _cutByCircleIsEnabled;
        private bool _mergeSectorIsEnabled;
        private bool _undoIsEnabled;
        private System.Windows.Input.Cursor _currCursor;
        private bool _drawRadarCircleIsChecked;
        private double _radarCircleRadius;
        private int _adhpHandle;
        private RadarAirspace _selectedAirspace;
        private int _airspaceHandle;
        private bool _filterByRadialIsChecked;
        private int _filterByRadialHandle;
        private bool _applyIsEnabled;
        private double _filterRadial;
        private IPolyline _radialLine;
        private IGeometry _radarVectoringArea;
        private double _minAllowableElevation;
        private RadarAirspace _selectedVectoringArea;
        private int _selectedVectoringAreaHandle;
        private int _unionVectoringAreaHandle;
        private bool _nextCommandIsEnabled;
        private IPoint _adhpGeo;
        private string _nextButtonContent;
        private string _statusText;
        private bool _createCircleIsChecked;

        private List<VerticalStructure> _vsList;
        private readonly ElavationCalculatorFacade _elevationCalculatorFacade;

        #endregion

        #region   :>Ctor

        /// <exception cref="ArgumentNullException">The <paramref name="list" /> parameter cannot be null.</exception>
        public MainViewModel(UnitConverter unitConverter)
        {
            _unitConverter = unitConverter;

            _radarCircleHandles = new List<int>();

            VectoringAreaList = new ObservableCollection<RadarAirspace>();

            StateList = new List<State>();

            InitializeAll();

            StepNumber = 0;

            GlobalParams.UnAssignedSectors = new ObservableCollection<Sector>();
            GlobalParams.UnAssignedSectors.CollectionChanged += UnAssignedSectors_CollectionChanged;

            Title = "Radar Minimum Altitude";

            SectorList = new ObservableCollection<Sector>();
            UnAssignedSectorList = new ObservableCollection<Sector>();

            _unionGeometry = new Polygon() as IPolygon;

            var gridTools = new GridTools();
            RasterLayerList = new ObservableCollection<TerrainClass>(gridTools.RasterLayers);
            if (RasterLayerList.Count > 0)
                SelectedRaster = RasterLayerList[0];

            _elevationCalculatorFacade = new ElevationCalculator.ElavationCalculatorFacade(_vsList,_selectedRaster?.RasterLayer?.Raster,_unitConverter);
        }

        private void InitializeAll()
        {
            InitializeCommands();
            LoadUnits();
            LoadData();
            InitializeCheckedItems();
            InitializeEsriCommands();
        }

        private void InitializeEsriCommands()
        {
            AranTool aranTool = GlobalParams.AranMapToolMenuItem.Tool;

            aranTool.MouseClickedOnMap +=
                AranMapToolMenuItem_Click;

            aranTool.MouseMoveOnMap +=
                AranMapToolMenuItem_Move;

            aranTool.MouseOnDblClickOnMap += AranMapToolMenuItem_OnDblClick;

            aranTool.MouseDownOnMap += AranMapToolMenuItem_OnMouseDown;

            ESRI.ArcGIS.Framework.ICommandBars commandBars = GlobalParams.Application.Document.CommandBars;
            ESRI.ArcGIS.esriSystem.UID commandID = new ESRI.ArcGIS.esriSystem.UIDClass();
            commandID.Value = "Aran.Panda.RadarMA.DrawLineTool"; // example: "Aran.Panda.RadarMA.Tool1";
            _drawLineCommandItem = commandBars.Find(commandID, false, false);

            var drawLineTool = _drawLineCommandItem?.Command as DrawLineTool;
            drawLineTool?.SetAranTool(aranTool);
            //(commandItem as DrawLineTool).SetAranTool(aranTool);
        }

        private void LoadUnits()
        {
            ElevationRounding = 100; // 5m  default Elevation rounding

            RadarCircleRadius = 10000; //30 NM

            BufferValueList = new ObservableCollection<double> { 3*NmToM, 5*NmToM};
            SelectedBufferValue = BufferValueList[0];

            MOCList = new ObservableCollection<double>
                { 1000 *FtToM, 1500 *FtToM, 2000*FtToM };
            SelectedMOCValue = MOCList[0];
        }

        private void LoadData()
        {
            AdhpList = GlobalParams.DbModule.GetAirportList();
            if (AdhpList.Count > 0)
                SelectedAdhp = AdhpList[0];

            _allRadarPointsList = new List<RadarPoint>();
            _allRadarPointsList.AddRange(GlobalParams.DbModule.GetRadarSystemList());

            RadarPointList = new List<RadarPoint>();
            RadarPointList.AddRange(_allRadarPointsList);
            if (RadarPointList.Count > 0)
                SelectedRadarPoint = RadarPointList[0];

            AirspaceList = new ObservableCollection<RadarAirspace>(GlobalParams.DbModule.GetRadarAirspaces());

            if (AirspaceList.Count > 0)
                SelectedAirspace = AirspaceList[0];

            _vsList = GlobalParams.DbModule.GetVerticalstructureList();
        }

        private void InitializeCheckedItems()
        {
            CreateSectorIsEnabled = false;
            DifferFromRadarOperAreaIsEnabled = false;
            DifferFromSectorIsEnabled = false;
            FilterByDistanceIsEnabled = false;
            MergeSectorIsEnabled = false;
            UndoIsEnabled = false;

            NextButtonContent = "Next";

            #region Page2
            IntersectIsChecked = true;
            RadialLeftSideIsChecked = true;
            DrawRadarCircleIsChecked = true;

            #endregion
        }

        private void InitializeCommands()
        {
            NextCommand = new RelayCommand(Next_Onclick);
            BackCommand = new RelayCommand(Back_Onclick);
            ApplyCommand = new RelayCommand(Apply_Onclick);
            UndoCommand = new RelayCommand(Undo_Onclick);
            ClearCommand = new RelayCommand(Clear_onclick);
            DifferFromSectorCommand = new RelayCommand(DifferFromSector_OnClick);
            DifferFromRadarOperArea = new RelayCommand(DifferFromRadarOperArea_OnClick);
            FilterByDistanceCommand = new RelayCommand(FilterByDistance_OnClick);
            MergeSectorsCommand = new RelayCommand(MergeSectors_OnClick);
            ReportCommand = new RelayCommand(Report_OnClick);
            FilterByRadialCommand = new RelayCommand(Filter_Radial_OnClick);
            AddAirspaceCommand = new RelayCommand(AddAirspace);
            RemoveAirspaceCommand = new RelayCommand(RemoveAirspace_FromList);
        }

        void UnAssignedSectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (GlobalParams.UnAssignedSectors == null) return;

            if (GlobalParams.UnAssignedSectors.Count == 0 && StepNumber==1)
                NextCommandIsEnabled = true;
            else
                NextCommandIsEnabled = false;
        }
       
        private void Report_OnClick(object obj)
        {
            var window = new View.Report(SectorList,_vsList,_elevationCalculatorFacade,_unitConverter,_radarVectoringArea);

            var parentHandle = new IntPtr(GlobalParams.Handle);
            var helper = new WindowInteropHelper(window) { Owner = parentHandle };
            ElementHost.EnableModelessKeyboardInterop(window);
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list

            GlobalParams.StateList = StateList;
            GlobalParams.SectorList = SectorList;

            window.Show();
        }

        private void MergeSectors_OnClick(object obj)
        {
            var window = new View.MergeSectorView(SectorList,StateList,MOCList,BufferValueList);

            var parentHandle = new IntPtr(GlobalParams.Handle);
            var helper = new WindowInteropHelper(window) { Owner = parentHandle };
            ElementHost.EnableModelessKeyboardInterop(window);
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
            window.Closed+=MergeIsClosing;
        }

        private void MergeIsClosing(object sender, EventArgs e)
        {
            _curState = StateList[StateList.Count-1];
        }
       
        #endregion

        #region :>Property

        #region List

        public List<RadarPoint> AdhpList { get; set; }
        public List<RadarPoint> RadarPointList { get; set; }
        public ObservableCollection<Sector> SectorList { get; set; }
        public ObservableCollection<TerrainClass> RasterLayerList { get; set; }
        public ObservableCollection<double> BufferValueList { get; set; }
        public ObservableCollection<double> MOCList { get; set; }
        public ObservableCollection<RadarAirspace> AirspaceList { get; set; }
        public ObservableCollection<RadarAirspace> VectoringAreaList { get; set; }
        public ObservableCollection<Sector> UnAssignedSectorList { get; set; }

        public List<State> StateList { get; set; }

        public System.Windows.Input.Cursor CurrCursor
        {
            get { return _currCursor; }
            set
            {
                _currCursor = value;
                NotifyPropertyChanged("CurrCursor");
            }
        }

        public RelayCommand NextCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand ApplyCommand { get; set; }
        public RelayCommand UndoCommand { get; set; }
        public RelayCommand RedoCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand DifferFromSectorCommand { get; set; }
        public RelayCommand DifferFromRadarOperArea { get; set; }
        public RelayCommand FilterByDistanceCommand { get; set; }
        public RelayCommand MergeSectorsCommand{ get; set; }
        public RelayCommand ReportCommand { get; set; }
        public RelayCommand FilterByRadialCommand { get; set; }
        public RelayCommand AddAirspaceCommand { get; set; }
        public RelayCommand RemoveAirspaceCommand { get; set; }

        #endregion

        #region Page 1

        public bool DrawRadarCircleIsChecked
        {
            get => _drawRadarCircleIsChecked;
            set
            {
                _drawRadarCircleIsChecked = value;
                if (_drawRadarCircleIsChecked)
                {
                    //DrawRadarCircles();
                    DrawRadarCircles2();
                    GlobalParams.UI.SafeDeleteGraphic(_circleHandle);
                }
                else
                {
                    DrawRadarOperationCircle();
                    ClearRadarCircles();
                }

                NotifyPropertyChanged(nameof(DrawRadarCircleIsChecked));
            }
        }

        public RadarPoint SelectedAdhp
        {
            get { return _selectedAdhp; }
            set
            {
                _selectedAdhp = value;

                GlobalParams.UI.SafeDeleteGraphic(_radarPointHandle);
                if (_selectedAdhp != null)
                {
                    _adhpGeo = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedAdhp.Geo);
                    _radarPointHandle = GlobalParams.UI.DrawPointWithText((IPoint)_adhpGeo, _selectedAdhp.Name, 11, ARANFunctions.RGB(255, 0, 0));

                   SetNextCommandEnabled();
                }
                NotifyPropertyChanged("SelectedAdhp");
            }

        }

        private void SetNextCommandEnabled()
        {
            NextCommandIsEnabled = false;
            if (_radarVectoringArea != null)
            {
                //if (!GeomOperators.Disjoint(_radarVectoringArea, _adhpGeo))
                //{
                NextCommandIsEnabled = true;
                StatusText = "Please press Next button!";
                //}
                //else
                //{
                //    StatusText = "Airport reference point must be inside Radar Vectoring Area.";
                //}
            }
            else
            {
                //if (_radarVectoringArea == null)
                StatusText = "Please select Radar Vectoring Area";
                //else
                //    StatusText = "Please select Airport reference Point";
            }
        }

        public RadarPoint SelectedRadarPoint
        {
            get { return _selectedRadarPoint; }
            set
            {
                _selectedRadarPoint = value;

                GlobalParams.UI.SafeDeleteGraphic(_adhpHandle);
                if (_selectedRadarPoint != null)
                {
                    RadarOperationDistance = _selectedRadarPoint.Range*1000;
                 
                    var prjPt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedRadarPoint.Geo);
                    _adhpHandle = GlobalParams.UI.DrawEsriPoint((IPoint)prjPt,8,ARANFunctions.RGB(0,0,255));
                    _magVar = _selectedRadarPoint.MagVar;
                    //  DrawRadarCircles();
                }
                
                NotifyPropertyChanged("SelectedRadarPoint");
            }
        }

        public TerrainClass SelectedRaster
        {
            get { return _selectedRaster; }
            set
            {
                _selectedRaster = value;
                //double[,] map = null;
                //if (_selectedRaster != null)
                //    map = _selectedRaster.ApproxiMap;
                NotifyPropertyChanged("SelectedRaster");
            }
        }

        public double RadarOperationDistance
        {
            get { return _radarOperationDistance; }
            set
            {
                _radarOperationDistance = value;

                if (DrawRadarCircleIsChecked)
                    DrawRadarCircles();
                else
                    DrawRadarOperationCircle();

                NotifyPropertyChanged(nameof(RadarOperationDistance));
            }
        }

        public double RadarCircleRadius
        {
            get { return _radarCircleRadius; }
            set
            {
                _radarCircleRadius =value;
                if (DrawRadarCircleIsChecked)
                    DrawRadarCircles2();
                NotifyPropertyChanged(nameof(RadarCircleRadius));
            }
        }

        public RadarAirspace SelectedAirspace
        {
            get { return _selectedAirspace; }
            set
            {
                _selectedAirspace = value;
                GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
                if (_selectedAirspace != null)
                {
                    var prjAirspace =GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedAirspace.Geo);
                    _airspaceHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon)prjAirspace, GlobalParams.RadarSymbol.VectoringAreaSymbol);
                }

                NotifyPropertyChanged("SelectedAirspace");
            }
        }

        public RadarAirspace SelectedVectoringArea
        {
            get { return _selectedVectoringArea; }
            set
            {
                _selectedVectoringArea = value;
                GlobalParams.UI.SafeDeleteGraphic(_selectedVectoringAreaHandle);
                if (_selectedVectoringArea != null)
                {
                    //_radarVectoringArea = GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedAirspace.Geo);
                    var prjVectoringArea = GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedVectoringArea.Geo);
                    _selectedVectoringAreaHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon)prjVectoringArea, GlobalParams.RadarSymbol.SectorProcessingSectorSybmol);
                }

                NotifyPropertyChanged("SelectedVectoringArea");
            }
        }

        private void AddAirspace(object obj)
        {
            if (SelectedAirspace == null) return;

            var prjGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(SelectedAirspace.Geo);

            if (_radarVectoringArea==null || _radarVectoringArea.IsEmpty)
            {
                _radarVectoringArea = prjGeo;
            }
            else
            {
                if (!GeomOperators.Disjoint(prjGeo, _radarVectoringArea))
                    _radarVectoringArea = GeomOperators.UnionPolygon((IPolygon) _radarVectoringArea, (IPolygon) prjGeo);
                else
                {
                    MessageBox.Show("Airspace must be connected other sectors", "Radar Minimum Altitude",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

           DrawRadarArea();

            VectoringAreaList.Add(SelectedAirspace);
            int index = AirspaceList.IndexOf(SelectedAirspace);
            AirspaceList.Remove(SelectedAirspace);
            if (index < AirspaceList.Count)
                SelectedAirspace = AirspaceList[index];

            SetNextCommandEnabled();
        }

        public string NextButtonContent
        {
            get { return _nextButtonContent; }
            set
            {
                _nextButtonContent = value;
                NotifyPropertyChanged("NextButtonContent");
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }

        private void DrawRadarArea()
        {
            GlobalParams.UI.SafeDeleteGraphic(_unionVectoringAreaHandle);
            _unionVectoringAreaHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon)_radarVectoringArea,
                GlobalParams.RadarSymbol.VectoringAreaSymbol);
        }

        #endregion

        #region Page 2

        public Sector SelectedSector
        {
            get => _selectedSector;
            set
            {
                _selectedSector?.Clear();

                _selectedSector = value;

                _selectedSector?.Draw();

                NotifyPropertyChanged(nameof(SelectedSector));
            }
        }

        public bool CreateSectorIsChecked
        {
            get { return _createSectorIsChecked; }
            set
            {
                _createSectorIsChecked = value;
                if (_createSectorIsChecked)
                {
                    if (_drawLineCommandItem!=null)
                        GlobalParams.Application.CurrentTool = _drawLineCommandItem;

                    _polygonFeedback = new ESRI.ArcGIS.Display.NewPolygonFeedback();
                    _polygonFeedback.Symbol = GlobalParams.RadarSymbol.DrawingSymbol;
                    _polygonFeedback.Display = GlobalParams.ActiveView.ScreenDisplay;
                    _pointCount = 0;
                    _creatingGeoType  = CreatingGeoType.Line;
                    _createCircleIsChecked = false;
                }
                else
                {
                    GlobalParams.Application.CurrentTool = null;
                    _polygonFeedback = null;
                }
                NotifyPropertyChanged("CreateSectorIsChecked");

            }
        }

        public bool CreateSectorIsEnabled
        {
            get { return _createSectorIsEnabled; }
            set
            {
                _createSectorIsEnabled = value;
                NotifyPropertyChanged("CreateSectorIsEnabled");
            }
        }


        public bool CreateCircleIsChecked
        {
            get { return _createCircleIsChecked; }
            set
            {
                _createCircleIsChecked = value;
                if (_createCircleIsChecked)
                {
                    if (_drawLineCommandItem != null)
                        GlobalParams.Application.CurrentTool = _drawLineCommandItem;

                    _cirleNewFeedback = new ESRI.ArcGIS.Display.NewCircleFeedbackClass();
                   // _cirleNewFeedback.Symbol = GlobalParams.RadarSymbol.DrawingSymbol;
                    _cirleNewFeedback.Display = GlobalParams.ActiveView.ScreenDisplay;
                    _creatingGeoType = CreatingGeoType.Circle;
                    _createSectorIsChecked = false;
                }
                else
                {
                    GlobalParams.Application.CurrentTool = null;
                    _cirleNewFeedback = null;
                }
                NotifyPropertyChanged("CreateCircleIsChecked");
            }
        }

        public bool DrawBufferIsChecked
        {
            get { return _drawBufferIsChecked; }
            set
            {
                _drawBufferIsChecked = value;
                if (_curState!=null)
                    _curState.Draw(_drawBufferIsChecked);
                NotifyPropertyChanged("DrawBufferIsChecked");
            }
        }
      
        public int StepNumber
        {
            get { return _stepNumber; }
            set
            {
                _stepNumber = value;
                NotifyPropertyChanged("StepNumber");
            }
        }

        public bool RadarSelectPointTabVisible => StepNumber==0;

        public bool CreateSectorIsVisible => StepNumber == 1;

        public bool FilterByDistanceIsChecked
        {
            get { return _filterByDistanceIsChecked; }
            set
            {
                _filterByDistanceIsChecked = value;

                if (_filterByDistanceIsChecked)
                {
                    DrawFilterCircle();
                }
                else
                {
                    GlobalParams.UI.SafeDeleteGraphic(_filterCircleHandle);
                }
                NotifyPropertyChanged("FilterByDistanceIsChecked");
            }
        }

        public bool FilterByRadialIsChecked
        {
            get { return _filterByRadialIsChecked; }
            set
            {
                _filterByRadialIsChecked = value;

                if (_filterByRadialIsChecked)
                    DrawFilterLine();
                else
                    GlobalParams.UI.SafeDeleteGraphic(_filterByRadialHandle);
               
                NotifyPropertyChanged("FilterByRadialIsChecked");
            }
        }

        private void DrawFilterLine()
        {
            var filterRadialWithMag = FilterRadial + _magVar;
            GlobalParams.UI.SafeDeleteGraphic(_filterByRadialHandle);
            var direction = GlobalParams.SpatialRefOperation.AztToDirGeo(new Aran.Geometries.Point(75, 66),
                filterRadialWithMag);
            var aranPt = new Aran.Geometries.Point(_selectedRadarPoint.GeoPrj.X, _selectedRadarPoint.GeoPrj.Y);

            var pt1 = ARANFunctions.LocalToPrj(aranPt, direction, _radarOperationDistance);
            IPoint esriPt1 = new PointClass {X = pt1.X, Y = pt1.Y};

            var pt2 = ARANFunctions.LocalToPrj(aranPt, direction, -_radarOperationDistance);
            IPoint esriPt2 = new PointClass { X = pt2.X, Y = pt2.Y };

            IPointCollection ptColl = new PolylineClass();
            ptColl.AddPoint(esriPt1);
            ptColl.AddPoint(esriPt2);
            _radialLine = ptColl as IPolyline;

            var red = ARANFunctions.RGB(255, 0, 0);
            _filterByRadialHandle = GlobalParams.UI.DrawMultiLineString(ptColl as IPolyline,red,esriSimpleLineStyle.esriSLSSolid);
        }

        public bool DifferFromSectorIsEnabled
        {
            get { return _differFromSectorIsEnabled; }
            set
            {
                _differFromSectorIsEnabled = value;
                NotifyPropertyChanged("DifferFromSectorIsEnabled");
            }
        }

        public bool DifferFromRadarOperAreaIsEnabled
        {
            get { return _differFromRadarOperAreaIsEnabled; }
            set
            {
                _differFromRadarOperAreaIsEnabled = value;
                NotifyPropertyChanged("DifferFromRadarOperAreaIsEnabled");
            }
        }

        public bool FilterByDistanceIsEnabled
        {
            get { return _cutByCircleIsEnabled; }
            set
            {
                _cutByCircleIsEnabled = value;
                NotifyPropertyChanged("CutByCircleIsEnabled");
            }
        }

        public bool MergeSectorIsEnabled
        {
            get { return _mergeSectorIsEnabled; }
            set
            {
                _mergeSectorIsEnabled = value;
                NotifyPropertyChanged("MergeSectorIsEnabled");
            }
        }

        public bool UndoIsEnabled
        {
            get { return _undoIsEnabled; }
            set
            {
                _undoIsEnabled = value;
                NotifyPropertyChanged("UndoIsEnabled");
            }
        }

        public double FilterCircleRadius
        {
            get { return _filterCircleRadius; }
            set
            {
                _filterCircleRadius =value;
                if (_filterCircleRadius > _radarOperationDistance)
                    _filterCircleRadius = _radarOperationDistance;
                //if (_filterCircleRadius < 5000)
                //    _filterCircleRadius = 5000;

                if (FilterByDistanceIsChecked)
                {
                    DrawFilterCircle();
                }
                NotifyPropertyChanged("FilterCircleRadius");
            }
        }

        public double SectorElevation
        {
            get => _sectorElevation;
            set
            {
                _sectorElevation = value;

                NotifyPropertyChanged(nameof(SectorElevation));
            }
        }

        public double SelectedBufferValue
        {
            get => _selectedBufferValue;
            set => _selectedBufferValue =value;
        }

// ReSharper disable once InconsistentNaming
        public double SelectedMOCValue
        {
            get => _selectedMocValue;
            set
            {
                _selectedMocValue =value;
                NotifyPropertyChanged(nameof(SelectedMOCValue));
            }
        }

        public bool IntersectIsChecked { get; set; }

        public bool DifferenceIsChecked { get; set; }

        public bool RadialLeftSideIsChecked { get; set; }

        public bool RadialRightSideIsChecked { get; set; }

        public bool ApplyIsEnabled
        {
            get { return _applyIsEnabled; }
            set
            {
                _applyIsEnabled = value;
                NotifyPropertyChanged("ApplyIsEnabled");
            }
        }

        public double FilterRadial
        {
            get { return _filterRadial; }
            set
            {
                _filterRadial = value;
                if (FilterByRadialIsChecked)
                    DrawFilterLine();

                NotifyPropertyChanged("FilterRadial");
            }
        }

        #endregion

        public bool BackCommandIsEnabled => _stepNumber > 0;

        public bool NextCommandIsEnabled
        {
            get { return _nextCommandIsEnabled; }
            set
            {
                _nextCommandIsEnabled = value;
                NotifyPropertyChanged("NextCommandIsEnabled");
            }
        }

        public string Title { get; set; }

        public string DistanceUnit => _unitConverter.DistanceUnit;

        public string HeightUnit => _unitConverter.HeightUnit;

        public double ElevationRounding
        {
            get { return _elevationRounding; }
            set
            {
                _elevationRounding = value;
                _unitConverter.HeightConverter[0].Rounding= _elevationRounding;
                _unitConverter.HeightConverter[1].Rounding = _elevationRounding;
                NotifyPropertyChanged("ElevationRounding");
            }
        }

        private RadarPoint _selectedCircleCenterPoint;
        public RadarPoint SelectedCircleCenter
        {
            get => _selectedCircleCenterPoint;
            set
            {
                _selectedCircleCenterPoint = value;
                if (_selectedCircleCenterPoint!=null)
                    CreateRadarAreaCircle();
                    
                NotifyPropertyChanged(nameof(SelectedCircleCenter));
            }
        }

        

        public double RadarAreaRadius
        {
            get => _radarAreaRadius;
            set
            {
                _radarAreaRadius = value;

                CreateRadarAreaCircle();
                NotifyPropertyChanged(nameof(RadarAreaRadius));
            }
        }

        #endregion

        #region :>Methods

        #region Page1

        private void ClearRadarCircles()
        {
            if (_radarCircleHandles == null)
                return;

            foreach (var radarCircleHandle in _radarCircleHandles)
                GlobalParams.UI.SafeDeleteGraphic(radarCircleHandle);
            
            _radarCircleHandles.Clear();
        }

        private void DrawRadarCircles()
        {
            ClearRadarCircles();

            if (_selectedRadarPoint != null)
            {
                int circleCount =Convert.ToInt32(Math.Ceiling(RadarOperationDistance/RadarCircleRadius));

                for (int i = 0; i < circleCount; i++)
                {
                    if (_selectedRadarPoint != null && _selectedRadarPoint.Geo != null)
                    {
                        var prjPt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedRadarPoint.Geo);
                        double radius = _radarCircleRadius*(i + 1);
                        var poly = Aran.PANDA.Common.ARANFunctions.CreateCircleAsPartPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y),radius);

                        var circlePolyLine = Aran.Converters.ConvertToEsriGeom.FromGeometry(poly);
                        if (circlePolyLine != null)
                        {
                            var handle = GlobalParams.UI.DrawMultiLineString((IPolyline) circlePolyLine,
                                GlobalParams.RadarSymbol.CircleDashSymbol);
                            _radarCircleHandles.Add(handle);

                            var textPt1 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), 0,
                                _radarCircleRadius*(i + 1)-3000, 0);
                            var textHandle1 = GlobalParams.UI.DrawText(new PointClass { X = textPt1.X, Y = textPt1.Y }, (RadarCircleRadius * (i + 1) + " NM").ToString(), 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle1);

                            var textPt2 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), Math.PI/2,
                                _radarCircleRadius * (i + 1) - 3000, 0);
                            var textHandle2 = GlobalParams.UI.DrawText(new PointClass { X = textPt2.X, Y = textPt2.Y }, (RadarCircleRadius * (i + 1) + " NM").ToString(), 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle2);

                            var textPt3 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), Math.PI,
                                _radarCircleRadius * (i + 1) - 3000, 0);
                            var textHandle3 = GlobalParams.UI.DrawText(new PointClass { X = textPt3.X, Y = textPt3.Y }, (RadarCircleRadius * (i + 1) + " NM").ToString(), 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle3);

                            var textPt4 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), 3*Math.PI/2,
                                _radarCircleRadius * (i + 1) - 3000, 0);
                            var textHandle4 = GlobalParams.UI.DrawText(new PointClass { X = textPt4.X, Y = textPt4.Y }, (RadarCircleRadius * (i + 1) + " NM").ToString(), 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle4);
                        }
                    }
                }
            }
        }

        private void DrawRadarCircles2()
        {
            ClearRadarCircles();

            if (_selectedRadarPoint != null)
            {
                int circleCount = 5;

                var startCircleRadius = 1000 * 15;

                for (int i = 0; i < circleCount; i++)
                {
                    if (_selectedRadarPoint?.Geo != null)
                    {
                        var prjPt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedRadarPoint.Geo);
                        double radius = startCircleRadius + 10*1000*i;
                        var poly = Aran.PANDA.Common.ARANFunctions.CreateCircleAsPartPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), radius);

                        var radiusText = radius / 1000 + "KM";

                        var circlePolyLine = Aran.Converters.ConvertToEsriGeom.FromGeometry(poly);
                        if (circlePolyLine != null)
                        {
                            var handle = GlobalParams.UI.DrawMultiLineString((IPolyline)circlePolyLine,
                                GlobalParams.RadarSymbol.CircleDashSymbol);
                            _radarCircleHandles.Add(handle);

                            var textPt1 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), 0,
                                radius - 3000, 0);
                            var textHandle1 = GlobalParams.UI.DrawText(new PointClass { X = textPt1.X, Y = textPt1.Y }, radiusText, 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle1);

                            var textPt2 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), Math.PI / 2,
                                radius - 3000, 0);
                            var textHandle2 = GlobalParams.UI.DrawText(new PointClass { X = textPt2.X, Y = textPt2.Y }, radiusText, 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle2);

                            var textPt3 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), Math.PI,
                                radius - 3000, 0);
                            var textHandle3 = GlobalParams.UI.DrawText(new PointClass { X = textPt3.X, Y = textPt3.Y }, radiusText, 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle3);

                            var textPt4 = ARANFunctions.LocalToPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), 3 * Math.PI / 2,
                                radius - 3000, 0);
                            var textHandle4 = GlobalParams.UI.DrawText(new PointClass { X = textPt4.X, Y = textPt4.Y }, radiusText, 13, ARANFunctions.RGB(255, 0, 0));
                            _radarCircleHandles.Add(textHandle4);
                        }
                    }
                }
            }
        }

        private void DrawRadarOperationCircle()
        {
            GlobalParams.UI.SafeDeleteGraphic(_circleHandle);
            

            if (_selectedRadarPoint != null && _selectedRadarPoint.Geo != null)
            {
                var prjPt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedRadarPoint.Geo);
                var poly = ARANFunctions.CreateCircleAsMultiPolyPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), _radarOperationDistance);

                _circleGeo = Aran.Converters.ConvertToEsriGeom.FromGeometry(poly);
                if (_circleGeo != null)
                    _circleHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon)_circleGeo, GlobalParams.RadarSymbol.CircleSymbol);
            }

        }

        private void RemoveAirspace_FromList(object obj)
        {
            if (_selectedVectoringArea != null)
            {
                int index = VectoringAreaList.IndexOf(_selectedVectoringArea);
                if (index == VectoringAreaList.Count - 1)
                {
                    var vectoringAreaPrj = GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedVectoringArea.Geo);
                    _radarVectoringArea = GeomOperators.Difference((IPolygon)_radarVectoringArea,
                        (IPolygon)vectoringAreaPrj);

                    GlobalParams.UI.SafeDeleteGraphic(_selectedVectoringAreaHandle);
                    GlobalParams.UI.SafeDeleteGraphic(_unionVectoringAreaHandle);
                    if (_radarVectoringArea != null)
                    {
                        _unionVectoringAreaHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon) _radarVectoringArea,
                            GlobalParams.RadarSymbol.SectorProcessingSectorSybmol);
                    }

                    AirspaceList.Add(_selectedVectoringArea);
                    VectoringAreaList.Remove(_selectedVectoringArea);
                }
                else
                {
                    MessageBox.Show("You can remove last element in the list!", "Radar Minimum Altitude",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                SetNextCommandEnabled();
            }
        }

        private void CreateRadarAreaCircle()
        {
            if (_selectedCircleCenterPoint == null)
                return;

            var prjPt = _selectedCircleCenterPoint.GeoPrj;
            var poly = Aran.PANDA.Common.ARANFunctions.CreateCircleAsMultiPolyPrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), _radarAreaRadius);
            _radarVectoringArea = Aran.Converters.ConvertToEsriGeom.FromGeometry(poly);

            SetNextCommandEnabled();
            DrawRadarArea();
        }

        #endregion

        #region Page 2

        private void DifferFromRadarOperArea_OnClick(object obj)
        {
            CurrCursor = Cursors.Wait;
            var drawingGeo = GeomOperators.Intersect(_curState.StateGeo,(IPolygon)_radarVectoringArea);
            if (!drawingGeo.IsEmpty)
                AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol , drawingGeo, OperationType.DifferFromOperationArea);
            CurrCursor = Cursors.Arrow;
        }

        private void DifferFromSector_OnClick(object obj)
        {
            CurrCursor = Cursors.Wait;
            var drawingGeo = GetDifferFromSector(_curState.StateGeo);

            if (!drawingGeo.IsEmpty)
                AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol, drawingGeo, OperationType.DifferFromOtherSectors);
            CurrCursor = Cursors.Arrow;
        }

        private void FilterByDistance_OnClick(object obj)
        {
            CurrCursor = Cursors.Wait;
            IPolygon drawingGeo = null;
            drawingGeo = DifferenceIsChecked ? GetDifferFromCircle(_curState.StateGeo) : GetIntersectWithCircle(_curState.StateGeo);

            if (drawingGeo != null && !drawingGeo.IsEmpty)
                AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol, drawingGeo,
                    OperationType.DifferFromOtherSectors);
            else
            {
                MessageBox.Show("System cannot proceed this operation!", "Radar Minimum Altitude", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            CurrCursor = Cursors.Arrow;
        }

        private void Filter_Radial_OnClick(object obj)
        {
            CurrCursor = Cursors.Wait;
            var drawingGeo = GetRadialGeom(_curState.StateGeo,_radialLine);

            if (drawingGeo != null && !drawingGeo.IsEmpty)
                AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol, drawingGeo,
                    OperationType.RadialOperation);
            else
            {
                MessageBox.Show("System cannot proceed this operation!", "Radar Minimum Altitude", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            CurrCursor = Cursors.Arrow;
        }

        private IPolygon GetDifferFromCircle(IPolygon geomPolygon)
        {
            var differGeo = GeomOperators.Difference(geomPolygon, _filterCircleGeo);

            var geomCollection = differGeo as IGeometryCollection;
            if (geomCollection != null && geomCollection.GeometryCount > 1)
            {
                //MessageBox.Show("Result cannot be multi polygon!");
                //Open new window to show choice of sectors
               // return null;
            }
            return differGeo;
        }

        private IPolygon GetIntersectWithCircle(IPolygon geomPolygon)
        {
            return GeomOperators.Intersect(geomPolygon, _filterCircleGeo);
        }

        private IPolygon GetDifferFromSector(IPolygon geomPolygon)
        {
            var drawingGeo = GeomOperators.Difference(geomPolygon, _unionGeometry);

            var geomCollection = drawingGeo as IGeometryCollection;
            if (geomCollection != null && geomCollection.GeometryCount > 2)
            {
               // MessageBox.Show("Result cannot be multi polygon!");
                //Open new window to show choice of sectors
                //return null;
            }
            return drawingGeo;
        }

        private IPolygon GetRadialGeom(IPolygon geomPolygon,IPolyline cutter)
        {
            var drawingGeo = GeomOperators.Cut(geomPolygon, cutter,RadialLeftSideIsChecked);

            var geomCollection = drawingGeo as IGeometryCollection;
            if (geomCollection != null && geomCollection.GeometryCount > 1)
            {
                MessageBox.Show("Result cannot be multi polygon!");
                //Open new window to show choice of sectors
                return null;
            }
            return drawingGeo as IPolygon;
        }

        private void Apply_Onclick(object obj)
        {
            if (_curState == null) return;

            CurrCursor = Cursors.Wait;

            var intersectWithRadarOperArea = GeomOperators.Intersect(_curState.StateGeo, (IPolygon) _radarVectoringArea);
            if (intersectWithRadarOperArea.IsEmpty)
                intersectWithRadarOperArea = _curState.StateGeo;

            var differFromSector = GetDifferFromSector(intersectWithRadarOperArea);
            if (differFromSector == null || differFromSector.IsEmpty)
            {
                CurrCursor = Cursors.Wait;
                return;
            }

            var stateGeo = differFromSector;

            
            if (!AddState(GlobalParams.RadarSymbol.SectorSymbol, stateGeo, OperationType.CreateSector))
            {
                CurrCursor = Cursors.Arrow;
                return;
            }

            var heighestPoint = StateList[StateList.Count - 1].StateMaxElevPoint;
            var resultUnionGeometry = GeomOperators.UnionPolygon(_unionGeometry, stateGeo);
            _unionGeometry = resultUnionGeometry;

            var newSector = new Sector(_curState.StateGeo, GlobalParams.RadarSymbol.CircleSymbol,_curState.ObstacleReports,_unitConverter)
            {
                Height = _curState.Altitude,
                Number = SectorList.Count + 1,
                MOC = _curState.Moc,
                BufferValue = _curState.BufferValue,
                UnionGeometry = resultUnionGeometry,
                Buffer = _curState.BufferGeo,
                StateMaxElevPoint = heighestPoint
            };
            SectorList.Add(newSector);

            CreateSectorIsEnabled = false;

            MergeSectorIsEnabled = SectorList.Count>1;

            if (SectorList.Count >0)
                NextCommandIsEnabled = true;

            CurrCursor = Cursors.Arrow;
            
        }

        private void Clear_onclick(object obj)
        {
        }

        private void Undo_Onclick(object obj)
        {
            if (StateList.Count > 0)
            {
                _curState = StateList[StateList.Count - 1];
                _curState.Clear();
                if (_curState.OperType == OperationType.CreateSector)
                {
                    SectorList.RemoveAt(SectorList.Count - 1);
                    if (SectorList.Count > 0)
                        _unionGeometry = SectorList[SectorList.Count - 1].UnionGeometry;
                    else
                        _unionGeometry.SetEmpty();

                    if (SectorList.Count == 1)
                        MergeSectorIsEnabled = false;

                }

                switch (_curState.OperType)
                {
                    case OperationType.DifferFromOperationArea:
                        DifferFromRadarOperAreaIsEnabled = true;
                        break;
                    case OperationType.DifferFromOtherSectors:
                        DifferFromSectorIsEnabled = true;
                        break;
                    case OperationType.CutByCircle:
                        FilterByDistanceIsEnabled = true;
                        break;
                }

                if (_curState.OperType == OperationType.JoinSectors)
                {
                    SectorList.RemoveAt(SectorList.Count-1);
                    foreach (var joinedSector in _curState.JoinSectorList)
                    {
                        foreach (var state in StateList)
                        {
                            if (joinedSector.Number== state.SectorNumber)
                                state.Draw(false);
                        }

                        SectorList.Add(joinedSector);
                    }
                  
                    int secNumber = 1;
                    foreach (var sector in SectorList)
                        sector.Number = secNumber++;
                }

                StateList.RemoveAt(StateList.Count - 1);
                if (StateList.Count > 0)
                {
                    _curState = StateList[StateList.Count - 1];
                    if (_curState.OperType != OperationType.CreateSector && _curState.OperType!= OperationType.JoinSectors)
                    {
                        _curState.Draw(_drawBufferIsChecked);
                        CreateSectorIsEnabled = true;
                        SectorElevation = _curState.Altitude;
                        _minAllowableElevation = SectorElevation;                     }
                }
                else
                {
                    _curState = null;
                    _unionGeometry.SetEmpty();
                    CreateSectorIsEnabled = false;
                    _minAllowableElevation = 0;
                }

                NextCommandIsEnabled = false;
            }
            else
            {
                UndoIsEnabled = false;
            }
        }

        private void AranMapToolMenuItem_OnDblClick()
        {
            try
            {
                if (_pointCount > 2)
                {
                    var drawingGeo = _polygonFeedback.Stop();

                    GeomOperators.SimplifyGeometry(drawingGeo);

                    AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol, drawingGeo, OperationType.DrawSector);

                    _polygonFeedback = null;
                    CreateSectorIsChecked = false;
                    CreateSectorIsEnabled = true;
                    _pointCount = 0;

                    if (SectorList.Count == 0)
                        NextCommandIsEnabled = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Radar Minimum Altitude", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AranMapToolMenuItem_Move(object sender, MapMouseEventArg arg)
        {
            ESRI.ArcGIS.Geometry.IPoint movePt = new ESRI.ArcGIS.Geometry.Point();
            movePt.X = arg.X;
            movePt.Y = arg.Y;

            ISnappingResult snapResult = null;

            //Try to snap the current position

            snapResult = GlobalParams.FetureSnap.Snapper.Snap(movePt);

            GlobalParams.FetureSnap.SnappingFeedback.Update(snapResult, 0);

            _isSnapped = false;
            //Snapping occurred
            if (snapResult != null)
            {
                //Set the current position to the snapped location
                _isSnapped = true;
                _snappedEsriPoint = snapResult.Location;
            }

            if (_pointCount > 0)
            {
                var movePoint = _isSnapped ? _snappedEsriPoint : movePt;

                if (_creatingGeoType == CreatingGeoType.Line)
                {
                    _polygonFeedback?.MoveTo(movePoint);
                }
                else if (_creatingGeoType == CreatingGeoType.Circle)
                {
                    _cirleNewFeedback?.MoveTo(movePoint);
                }
            }
        }

        private void AranMapToolMenuItem_Click(object sender, MapMouseEventArg arg)
        {
            IPoint point = new ESRI.ArcGIS.Geometry.Point();
            point.X = arg.X;
            point.Y = arg.Y;

            if (_isSnapped)
                point = _snappedEsriPoint;

            if (_polygonFeedback == null && _cirleNewFeedback==null) return;

            if (_creatingGeoType == CreatingGeoType.Line)
            {
                if (_pointCount == 0)
                {
                    _polygonFeedback.Start(point);
                }
                else
                    _polygonFeedback.AddPoint(point);
                _pointCount++;
            }
            else if (_creatingGeoType == CreatingGeoType.Circle)
            {
                if (_cirleNewFeedback == null) return;

                var circualArc = _cirleNewFeedback.Stop();
                if (circualArc != null)
                {
                    var centerPt = new Aran.Geometries.Point(circualArc.CenterPoint.X, circualArc.CenterPoint.Y);

                    var circleGeo = ARANFunctions.CreateCircleAsMultiPolyPrj(centerPt, circualArc.Radius);
                    var drawingGeo =(IPolygon) ConvertToEsriGeom.FromGeometry(circleGeo);

                    AddState(GlobalParams.RadarSymbol.SectorProcessingSectorSybmol, drawingGeo, OperationType.DrawCircle);
                  
                    //DrawCircleIsChecked = false;
                }
                _cirleNewFeedback = null;
                CreateCircleIsChecked = false;
                CreateSectorIsEnabled = true;
                _pointCount = 0;
            }
        }

        private void AranMapToolMenuItem_OnMouseDown(object sender, MapMouseEventArg arg)
        {
            if (_creatingGeoType != CreatingGeoType.Circle) return;

            IPoint point = new ESRI.ArcGIS.Geometry.Point();
            point.X = arg.X;
            point.Y = arg.Y;

            if (_cirleNewFeedback == null)
            {
                _cirleNewFeedback = new NewCircleFeedbackClass();
                _cirleNewFeedback.Display = GlobalParams.ActiveView.ScreenDisplay;
            }
        
            if (_isSnapped)
                _cirleNewFeedback.Start(_snappedEsriPoint);
            else
                _cirleNewFeedback.Start(point);
            _pointCount = 1;
        }

        private void DrawFilterCircle()
        {
            GlobalParams.UI.SafeDeleteGraphic(_filterCircleHandle);

            if (_selectedRadarPoint != null && _selectedRadarPoint.Geo != null)
            {
                var prjPt = (IPoint)GlobalParams.SpatialRefOperation.ToEsriPrj(_selectedRadarPoint.Geo);
        
                var poly = ARANFunctions.CreateCirclePrj(new Aran.Geometries.Point(prjPt.X, prjPt.Y), _filterCircleRadius);
                
                _filterCircleGeo = Aran.Converters.ConvertToEsriGeom.FromPolygon(new Geometries.Polygon{ExteriorRing = poly});
                if (_filterCircleGeo != null)
                    _filterCircleHandle = GlobalParams.UI.DrawEsriPolygon((IPolygon)_filterCircleGeo,GlobalParams.RadarSymbol.CircleSymbol);
            }

        }

        private bool AddState(ISymbol symbol, IPolygon stateGeo, OperationType sOperationType)
        {
            try
            {
                if (stateGeo == null || stateGeo.IsEmpty) return false;
                MergeSectorIsEnabled = false;
                
                var state = new State(symbol)
                {
                    BufferValue = SelectedBufferValue,
                    Moc = SelectedMOCValue,
                    OperType = sOperationType,
                    StateGeo = stateGeo,
                    CircleRadius = _filterCircleRadius
                };

                var bufferGeo = GeomOperators.Buffer(stateGeo, _selectedBufferValue);
                if (bufferGeo != null)
                    state.BufferGeo = bufferGeo;

                if (StateList.Count > 0 && _curState.OperType != OperationType.CreateSector && _curState.OperType!=OperationType.JoinSectors)
                    _curState.Clear();


                var obstacleReports = _elevationCalculatorFacade.GetObstacleReports(bufferGeo);
                var maxElevationReport = obstacleReports.OrderByDescending(obs => obs.Elevation)
                    .FirstOrDefault();

                if (maxElevationReport != null)
                {
                    _minAllowableElevation = maxElevationReport.Elevation;

                    SectorElevation = SelectedMOCValue;
                    if (Math.Abs(_minAllowableElevation - double.MinValue)>0.001)
                        SectorElevation +=_minAllowableElevation;
                
                
                    state.Altitude = SectorElevation;
                    state.StateMaxElevPoint = maxElevationReport.GeoPrj as IPoint;
                }

                state.Draw(_drawBufferIsChecked);
                state.ObstacleReports = obstacleReports;

                StateList.Add(state);

                if (_curState!=null && _curState.OperType !=OperationType.CreateSector && _curState.OperType== OperationType.JoinSectors )
                    _curState.ClearHeighestPoint();
               
                _curState = state;
                if (_curState.OperType == OperationType.DifferFromOtherSectors)
                    DifferFromSectorIsEnabled = false;
                else if (_curState.OperType == OperationType.DifferFromOperationArea)
                    DifferFromRadarOperAreaIsEnabled = false;
                else if (_curState.OperType == OperationType.CutByCircle)
                    FilterByDistanceIsEnabled = false;
                else if (_curState.OperType == OperationType.DrawSector || _curState.OperType== OperationType.DrawCircle)
                {
                    DifferFromSectorIsEnabled = true;
                    DifferFromRadarOperAreaIsEnabled = true;
                    FilterByDistanceIsEnabled = true;
                    ApplyIsEnabled = true;
                }
                
                else if (_curState.OperType == OperationType.CreateSector)
                {
                    DifferFromSectorIsEnabled = false;
                    DifferFromRadarOperAreaIsEnabled = false;
                    FilterByDistanceIsEnabled = false;
                    FilterByDistanceIsChecked = false;
                    state.SectorNumber = SectorList.Count + 1;
                    FilterByRadialIsChecked = false;
                    ApplyIsEnabled = false;
                }

                if (StateList.Count > 0)
                    UndoIsEnabled = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error extracting Elevation from Raster!"+e.Message);
                return false;
            }
        }

        internal void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_circleHandle);
            GlobalParams.UI.SafeDeleteGraphic(_radarPointHandle);
            GlobalParams.UI.SafeDeleteGraphic(_filterCircleHandle);
            GlobalParams.UI.SafeDeleteGraphic(_adhpHandle);
            GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
            GlobalParams.UI.SafeDeleteGraphic(_filterByRadialHandle);
            GlobalParams.UI.SafeDeleteGraphic(_selectedVectoringAreaHandle);
            GlobalParams.UI.SafeDeleteGraphic(_unionVectoringAreaHandle);
            ClearRadarCircles();

            SectorList.ToList().ForEach(sector => sector.Clear());
            SectorList.Clear();

            StateList.ForEach(state=>state.Clear());
            StateList.Clear();
        }
        
        #endregion

        private void Back_Onclick(object obj)
        {
            StepNumber--;
            NextCommandIsEnabled = true;
            if (StepNumber == 0)
            {
                NextButtonContent = "Next";
                StatusText = "Please press Next button!";
            }

            NotifyPropertyChanged("");
        }

        private void Next_Onclick(object obj)
        {
            StepNumber++;

            //Next to create sector
            if (StepNumber == 1)
            {
                GlobalParams.SelectedRaster = SelectedRaster?.RasterLayer?.Raster;

                GlobalParams.RadarVectoringArea = _radarVectoringArea;

                GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
                GlobalParams.UI.SafeDeleteGraphic(_selectedVectoringAreaHandle);

                if (SectorList.Count == 0)
                    NextCommandIsEnabled = false;
                else
                    NextCommandIsEnabled = true;
                NextButtonContent = "Save";
                StatusText = "Create all sectors to save it to Db";
            }
                //Save result to db
            else
            {
                var result = MessageBox.Show("Do you really want to save radar areas to GeoDatabase!",
                    "Radar Minimum Altitude", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                    return;

                //var window = new View.SaveForm();
                //var parentHandle = new IntPtr(GlobalParams.Handle);
                //var helper = new WindowInteropHelper(window) {Owner = parentHandle};
                //ElementHost.EnableModelessKeyboardInterop(window);
                //window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                //if (window.ShowDialog() == true)
                //{
                    var projectName = "Surgut";// window.ProjectName;
                    if (GlobalParams.DbModule.SaveRadarAreaToDb(projectName, SectorList))
                    {
                        MessageBox.Show("Radar areas saved to GeoDatabase successfully!", "Radar Minimum Altitude",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        NextCommandIsEnabled = false;
                        UndoIsEnabled = false;
                        // Clear();
                    }
                //}
                //else
                //    StepNumber--;
            }

            NotifyPropertyChanged("");
        }

        #endregion
    }
}
