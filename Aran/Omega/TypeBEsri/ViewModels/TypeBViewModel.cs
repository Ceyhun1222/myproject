using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Integration;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Aran.Omega.TypeBEsri.Models;
using Aran.Omega.TypeBEsri.View;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Interop;
using System.Diagnostics;
using Aran.Aim.Enums;
using Aran.Queries;
using Aran.Geometries;
using ESRI.ArcGIS;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using Aran.AranEnvironment.Symbols;

namespace Aran.Omega.TypeBEsri.ViewModels
{
    public class TypeBViewModel : ViewModel
    {
        #region :>Fields
        private EnumName<RunwayClassificationType> _selClassification;
        private EnumName<CategoryNumber> _selCatNumber;
        private ObservableCollection<EnumName<CategoryNumber>> _categoryList;
        private ElevationDatum _selElevationDatum;
        private RwyClass _selectedRwyClass;
        private string _errorLog;
        private TypeBSurfaces _typeBSurfaces;
        private bool _reportIsCalculated;
        private bool _saveIsEnabled;
        private RwyView _selectedRwyView;
        //private BackgroundWorker _worker;
        //private ProgressBar progWindow;
        #endregion

        #region :>Constructor
        public TypeBViewModel()
        {
        }

        #endregion

        #region :>Property

        #region :>Lists
       // public ObservableCollection<RwyView> RwyViewList { get;private set; }
        public ObservableCollection<RwyClass> RwyClassList { get; set; }
        public ObservableCollection<EnumName<RunwayClassificationType>> Classification { get; private set; }
        public ObservableCollection<EnumName<CategoryNumber>> CategoryList { get; private set; }
        public ObservableCollection<ElevationDatum> ElevationDatumList { get; set; }
        public ObservableCollection<DrawingSurface> AvailableSurfaceList { get; set; }
        public ObservableCollection<RwyDirClass> RwyDirClassList { get; set; }

        #endregion

        public TypeBSurfaces Annex14Surfaces
        {
            get { return _typeBSurfaces; }
        }
        public CommonParam CommonParamModel { get; set; }

        public EnumName<RunwayClassificationType> SelectedClassification
        {
            get { return _selClassification; }
            set
            {
                if (_selClassification == value)
                    return;

                _selClassification = value;
                if (value.Enum == RunwayClassificationType.PrecisionApproach)
                {
                    CategoryList.Clear();
                    foreach (var cat in _categoryList)
                        CategoryList.Add(cat);

                    SelectedCategory = CategoryList[0];
                }
                else
                    CategoryList.Clear();

                NotifyPropertyChanged("SelectedCategory");
            }
        }

        public EnumName<CategoryNumber> SelectedCategory
        {
            get { return _selCatNumber; }
            set
            {
                if (_selCatNumber == value)
                    return;

                _selCatNumber = value;
                NotifyPropertyChanged("CategoryIsEnabled");
                NotifyPropertyChanged("SelectedCategory");
            }
        }

        public bool CategoryIsEnabled { get { return (CategoryList == null || CategoryList.Count > 0); } }

        public ElevationDatum SelectedElevationDatum
        {
            get { return _selElevationDatum; }
            set
            {
                _selElevationDatum = value;
                NotifyPropertyChanged("SelectedElevationDatum");
            }
        }

        public bool CodeLetterFIsEnabled { get; set; }

        public bool CodeLetterFIsChecked { get; set; }

        public ICommand DrawCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand ReportCommand { get; set; }

        public ICommand ManualReport { get; set; }

        public ICommand BackCommand { get; set; }

        public ICommand UpdateAllCommand { get; set; }

        private RwyDirClass _selectedRwyDirection;

        public RwyDirClass SelectedRwyDirection
        {
            get { return _selectedRwyDirection; }
            set
            {
                _selectedRwyDirection = value;
                CreateElevationDatum();
            }
        }

        public System.Windows.Input.Cursor CurrCursor { get; set; }

        public string Title { get; set; }

        public bool ChangesGreaterThanFifteen { get; set; }
        public bool ChangesGreaterThanFifteenIsEnabled { get; set; }

        public Action CloseAction { get; set; }

        public bool ReportIsEnabled
        {
            get { return AvailableSurfaceList.Count > 0; }
        }

        //Charting Properties
        public int PageIndex { get; set; }
        public string Organisation { get; set; }
        public string CityAerodrome { get; set; }
        public string ICAOCode { get; set; }
        public string ChartHeader { get; set; }
        public string ChartChanging { get; set; }
        public string ChartingCreationDate { get; set; }
        public string AerodromeElevation { get; set; }
        public string RunwayComposition { get; set; }
        public string CivilAviation { get; set; }
        public string  GridText { get; set; }
        public string UnitsText { get; set; }

        private ManualReport _reportWindow;
        private string _gdbFileName;
        private IFeatureWorkspace _featureWorkspace;
        private string _fileName;
        private IWorkspaceFactory2 _workspaceFactory;
        private IWorkspace _workspace;
        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set
            {
                _saveIsEnabled = value;
                NotifyPropertyChanged("SaveIsEnabled");
            }
        }

        public bool ManualReportIsEnabled { get { return AvailableSurfaceList.Count > 0; } }
        #endregion

        #region :>Methods

        #region :>Events

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ClearAll();
        }

        private void CreateElevationDatum()
        {
            ElevationDatumList = new ObservableCollection<ElevationDatum>();

            var adhpElevationDatum = new ElevationDatum(GlobalParams.Database.AirportHeliport);
            ElevationDatumList.Add(adhpElevationDatum);

            NotifyPropertyChanged("ElevationDatumList");
            SelectedElevationDatum = ElevationDatumList[0];
        }

        private void drawCommand_onClick(object obj)
        {
            //Change window cursor to wait mode

            CurrCursor = Cursors.Wait;
            NotifyPropertyChanged("CurrCursor");

            ClearAll();
            AvailableSurfaceList.Clear();

            var selectedRwyViewList = RwyClassList.Where(rwy => rwy.Checked == true).ToList<RwyClass>();

            //var selectedRwyClassList = selectedRwyViewList.Select(rwy => rwy.Rwy).Distinct().ToList<RwyClass>();

            if (selectedRwyViewList.Count == 0)
                return;

            var catNumber = CategoryNumber.One;
            if (SelectedCategory != null)
                catNumber = SelectedCategory.Enum;
            _selectedRwyClass = selectedRwyViewList[0];
            GetSurfaceList();

            _typeBSurfaces = new TypeBSurfaces(selectedRwyViewList[0], SelectedRwyDirection, SelectedElevationDatum, _selClassification.Enum,
                catNumber, _selectedRwyClass.CodeNumber);


            foreach (var drawingSurface in AvailableSurfaceList)
            {
                switch (drawingSurface.SurfaceType)
                {
                    case SurfaceType.InnerHorizontal:
                        {
                            var innerHorizontal = _typeBSurfaces.CreateInnerHorizontal();
                            drawingSurface.SurfaceBase = innerHorizontal;
                        }
                        break;
                    case SurfaceType.CONICAL:
                        {
                            var conical = _typeBSurfaces.CreateConicalPlane();
                            drawingSurface.SurfaceBase = conical;
                        }
                        break;
                    case SurfaceType.OuterHorizontal:
                        {
                            var outer = _typeBSurfaces.CreateOuterHorizontal();
                            drawingSurface.SurfaceBase = outer;
                        }
                        break;
                    case SurfaceType.Approach:
                        {
                            var approach = _typeBSurfaces.CreateApproach();
                            drawingSurface.SurfaceBase = approach;
                        }
                        break;
                    case SurfaceType.Transitional:
                        {
                            var transitional = _typeBSurfaces.CreateTransitionalSurface();
                            drawingSurface.SurfaceBase = transitional;
                        }
                        break;
                    case SurfaceType.TakeOffClimb:
                        {
                            var takeOff = _typeBSurfaces.CreateTakeOffSurface(CodeLetterFIsChecked);
                            drawingSurface.SurfaceBase = takeOff;
                        }
                        break;
                }

               
            }

            SurfacePenetration surfacePenetration = new SurfacePenetration(_typeBSurfaces,_selectedRwyClass,_selectedRwyDirection);
            surfacePenetration.CutInnerHorizontal();
            surfacePenetration.CutConicalSurface();
            foreach (var surface in AvailableSurfaceList)
            {
                surface.SurfaceBase.Draw(false);
            }
            //Change window cursor to normal mode
            CurrCursor = Cursors.Arrow;
            NotifyPropertyChanged("CurrCursor");

            SaveIsEnabled = true;
            NotifyPropertyChanged("ReportIsEnabled");
            NotifyPropertyChanged("ManualReportIsEnabled");
    
        }
    
        private void reportCommand_onClick(object obj)
        {
            //Change window cursor to wait mode
            CurrCursor = Cursors.Wait;
            //CurrCursor = new Cursor(@"D:\AirNav\bin\Debug\ajax-loader.ani");
            NotifyPropertyChanged("CurrCursor");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (!_reportIsCalculated)
                _typeBSurfaces.AnalysesAirportSurfaces();

            foreach (var surface in AvailableSurfaceList)
            {
                if (surface.SurfaceBase != null)
                {
                    var obstacleReports = surface.SurfaceBase.Report;
                }

            }

            var reportList =( from surface in AvailableSurfaceList
                          from report in surface.SurfaceBase.Report
                          group report by new {report.Id} //or group by new {p.ID, p.Name, p.Whatever}
                          into mygroup
                          select mygroup.OrderByDescending(report=>report.Penetrate).First()).ToList<ObstacleReport>();

            foreach (var surface in AvailableSurfaceList)
            {
                if (surface.SurfaceBase != null)
                {
                    surface.SurfaceBase.Report.Clear();
                    surface.SurfaceBase.Report = reportList.Where<ObstacleReport>(reportItem => reportItem.SurfaceType == surface.SurfaceBase.SurfaceType).ToList<ObstacleReport>();
                }
            }

            if (AvailableSurfaceList.Count > 0)
            {
                //var reportWindow = new Report(this);
                //var helper = new WindowInteropHelper(reportWindow);
                //ElementHost.EnableModelessKeyboardInterop(reportWindow);
                //helper.Owner = new IntPtr(GlobalParams.HWND);
                //reportWindow.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                //reportWindow.Show();
                _reportIsCalculated = true;
                SaveIsEnabled = true;
            }

            CurrCursor = Cursors.Arrow;
            NotifyPropertyChanged("CurrCursor");
        }

        private void manualReport_onClick(object obj)
        {
            if (_reportWindow == null || _reportWindow.IsClosed)
            {
                _reportWindow = new ManualReport();
                var helper = new WindowInteropHelper(_reportWindow);
                ElementHost.EnableModelessKeyboardInterop(_reportWindow);
                helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                _reportWindow.ShowInTaskbar = false;
                _reportWindow.SetAvailableSurfaces(AvailableSurfaceList);// hide from taskbar and alt-tab list
                _reportWindow.Show();
            }
        }

        private void saveCommand_onClick(object obj)
        {
            RuntimeManager.BindLicense(ProductCode.Desktop);
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            //openFileDialog1.InitialDirectory = "c:\\" ;
            openFileDialog1.Filter = "mdb files (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            reportCommand_onClick(null);

            SaveAdhpToGeoDatabase();
            SaveRunwayElementToDb();
            SaveObstacleAreaToDb();
            SaveObstaclesToDb();
            SaveThresholdsToGeoDatabase();

            MessageBox.Show("All data saved successfully!", "TypeB", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearAll();
            updateAllCommand_onClick(null);
            //GlobalParams.ActiveView.Refresh();
            GetTypeBData();
            PageIndex = 1;
            NotifyPropertyChanged("PageIndex");
            //workspaceEdit.StopEditOperation();
            //workspaceEdit.StopEditing(true);
        }

        #endregion

        private void ClearAll()
        {
            foreach (DrawingSurface surface in AvailableSurfaceList)
            {
                if (surface.SurfaceBase != null)
                    surface.SurfaceBase.ClearAll();
            }
        }

        public void Init()
        {
            try
            {
                AvailableSurfaceList = new ObservableCollection<DrawingSurface>();
                CommonParamModel = new CommonParam(InitOmega.ObstacleDistanceFromArp);
                DrawCommand = new RelayCommand(new Action<object>(drawCommand_onClick));
                ReportCommand = new RelayCommand(new Action<object>(reportCommand_onClick));
                SaveCommand = new RelayCommand(new Action<object>(saveCommand_onClick));
                ManualReport = new RelayCommand(new Action<object>(manualReport_onClick));
                BackCommand = new RelayCommand(new Action<object>(backCommand_onClick));
                UpdateAllCommand = new RelayCommand(new Action<object>(updateAllCommand_onClick));
                RwyDirClassList = new ObservableCollection<RwyDirClass>();
                Title = "Omega - Aerodrome: " + GlobalParams.Database.AirportHeliport.Designator;
                PageIndex = 0;

                CurrCursor = Cursors.Arrow;
                _reportIsCalculated = false;
                SaveIsEnabled = false;

                RwyClassList = new ObservableCollection<RwyClass>();
                var runwayList = GlobalParams.Database.Runways;

                for (int i = 0; i < runwayList.Count; i++)
                {
                    var rwyClass = new RwyClass(runwayList[i]);
                    if (i == 0)
                    {
                        rwyClass.ChangeChecked(true);
                        _selectedRwyClass = rwyClass;
                    }
                    rwyClass.RwyCheckedIsChanged += new EventHandler(rwyClass_RwyCheckedIsChanged);
                    
                    RwyClassList.Add(rwyClass);
                }

                rwyClass_RwyCheckedIsChanged(_selectedRwyClass, new EventArgs());

                //Init Category constant params
                _categoryList = new ObservableCollection<EnumName<CategoryNumber>>
                {
                    new EnumName<CategoryNumber> {Name = "I", Enum = CategoryNumber.One},
                    new EnumName<CategoryNumber> {Name = "II,III", Enum = CategoryNumber.Two}
                };
                CategoryList = new ObservableCollection<EnumName<CategoryNumber>>();

                //End init Category


                //Init Classifaction constant params
                Classification = new ObservableCollection<EnumName<RunwayClassificationType>>
                {
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Non-instrument",
                        Enum = RunwayClassificationType.NonInstrument
                    },
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Non-precision approach",
                        Enum = RunwayClassificationType.NonPrecisionApproach
                    },
                    new EnumName<RunwayClassificationType>
                    {
                        Name = "Precision approach",
                        Enum = RunwayClassificationType.PrecisionApproach
                    }
                };
                SelectedClassification = Classification[2];
                CreateElevationDatum();
            }
            catch (Exception e)
            {
                throw new Exception("Error load main form!" + Environment.NewLine + e.Message);

            }

        }

        private void updateAllCommand_onClick(object obj)
        {
            FindAndChangeElement("elemOrganisation",Organisation);
            FindAndChangeElement("elemDate",ChartingCreationDate);
            FindAndChangeElement("elemAdhpCity", CityAerodrome);
            FindAndChangeElement("elemHeader", ChartHeader);
            FindAndChangeElement("elemICAOCode", ICAOCode);
            FindAndChangeElement("elemChanging", ChartChanging);
            FindAndChangeElement("elemAdhpElevation", AerodromeElevation);
            FindAndChangeElement("elemRwyComposition", RunwayComposition);
            FindAndChangeElement("elemCompas", RunwayComposition);
            FindAndChangeElement("elemCivilAviation", CivilAviation);
            FindAndChangeElement("elemGridText", GridText);
            FindAndChangeElement("elemCoordUnits", UnitsText);
        }

        public IElement FindAndChangeElement(string elementName,string elemText)
        {
            IGraphicsContainer container = GlobalParams.PageLayout as IGraphicsContainer;
            IActiveView activeView = GlobalParams.PageLayout as IActiveView;
            container.Reset();
            IElement element = container.Next();
            IElement addingElement = null;
            GlobalParams.ActiveView = activeView;
            LineString lineString = new LineString();
            Aran.Geometries.Point aranPrj = new Geometries.Point();
            double magVariation = 0;
            var airportHeliport = GlobalParams.Database.AirportHeliport;
            IMarkerElement markerElem = null;

            while (element != null)
            {
                IElementProperties3 elemProperties = element as IElementProperties3;
                if (elementName == elemProperties.Name)
                {
                    var textElement = element as ITextElement;
                    if (textElement != null)
                    {
                        textElement.Text = elemText;
                        container.UpdateElement(element);
                        return element;
                    }
                    else if (element is IMarkerElement) 
                    {
                        markerElem = element as IMarkerElement;

                        var graphElement = markerElem as IMarkerElement;
                        var esriPoint = element.Geometry as ESRI.ArcGIS.Geometry.IPoint;
                        ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.Envelope() as ESRI.ArcGIS.Geometry.IEnvelope;
                        element.QueryBounds(activeView.ScreenDisplay, envelope);

                        if (GlobalParams.Database.AirportHeliport.MagneticVariation.HasValue)
                        {
                            magVariation = airportHeliport.MagneticVariation.Value;
                        }
                        aranPrj = Converters.ConvertFromEsriGeom.ToPoint(esriPoint);
                        var compPt1 = ARANFunctions.LocalToPrj(aranPrj,
                            ARANMath.DegToRad(90-magVariation), envelope.Width *2 / 5 ,0);

                        var compPt2 = ARANFunctions.LocalToPrj(aranPrj,
                            ARANMath.DegToRad(90 - magVariation), -envelope.Width *2 / 5, 0);

                        lineString = new LineString();
                        lineString.Add(compPt1);
                        lineString.Add(compPt2);
                    }
                }
                element = container.Next();
            }
            if (!lineString.IsEmpty)
            {
                //IGraphicsContainer pGraphics = activeView.GraphicsContainer;
                //pGraphics.AddElement(addingElement, 0);
                AranGraphics aranGraphics = new AranGraphics();

                MultiLineString mltString = new MultiLineString { lineString };
                LineSymbol lineSymbol = new LineSymbol(eLineStyle.slsDash, 100, 2) ;

                IElement lineElement = aranGraphics.DrawMultiLineString(mltString, lineSymbol);
                PointSymbol ptSymbol = new PointSymbol();
                ptSymbol.Size = 3;
                ptSymbol.Color = 100;
                
                string magText = "";
                
                if (GlobalParams.Database.AirportHeliport.MagneticVariation.HasValue)
                {
                    magText = "VAR " + magVariation + " E " + (char)13 + (char)10;
                    if (airportHeliport.DateMagneticVariation != null) 
                    {
                        magText ="VAR " + magVariation + " E "+ "(" + airportHeliport.DateMagneticVariation + ") " + (char)13 + (char)10;
                    }
                    if (airportHeliport.MagneticVariationChange.HasValue) 
                    {
                        magText += " & ANNUAL CHANGE "+ airportHeliport.MagneticVariationChange.Value + " E";
                    }
                }
                IElement pointElement = aranGraphics.DrawPointWithText(ARANFunctions.LocalToPrj(aranPrj,ARANMath.DegToRad(180),20,0), ptSymbol, magText,-90);
                IGroupElement pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
                
                pGroupElement.AddElement(lineElement);
                pGroupElement.AddElement(pointElement);
                if (markerElem != null)
                {
                    pGroupElement.AddElement(markerElem as IElement);

                    container.DeleteElement(markerElem as IElement);
                    container.AddElement(pGroupElement as IElement, 0);
                }
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                
            }
            return null;
        }

        private void GetTypeBData()
        {
            var org = GlobalParams.Database.GetOrganisation();
            Organisation = org.Designator;
            var adhp = GlobalParams.Database.AirportHeliport;

            CityAerodrome = adhp.Designator;
            foreach (var city in adhp.ServedCity)
            {
                CityAerodrome = city.Name + "/" + adhp.Name;
                break;
            }
            ICAOCode = adhp.LocationIndicatorICAO;
            ChartHeader = "AERODROME OBSTACLE CHART - ICAO TYPE B";
            ChartingCreationDate = DateTime.Now.ToString("dd MMMM yyyy");
            AerodromeElevation ="AERODROME ELEVATION : " + Common.ConvertHeight(SelectedElevationDatum.Height, Enums.RoundType.ToNearest) + " " + InitOmega.HeightConverter.Unit.ToUpper();

            CivilAviation="CIVIL AVIATION AGENCY"+(char)13+(char)10+org.Designator;
            GridText = "Grid lines and co-ordinates shown are based on WGS-84 datum";
            UnitsText = "DISTANCE IN METRES"+(char)13+(char)10+
                "ELEVATION AND HEIGHT IN ";
            if (InitOmega.HeightConverter.Unit=="ft")
                UnitsText+="FEET";
            else
                UnitsText+="METRES";

            var rwy = _selectedRwyClass.Runway;
            if (rwy.SurfaceProperties!=null){
                switch (_selectedRwyClass.Runway.SurfaceProperties.Composition.Value)
                {
                    case CodeSurfaceComposition.ASPH:
                        RunwayComposition = "Asphalt";
                        break;
                    case CodeSurfaceComposition.ASPH_GRASS:
                        RunwayComposition = "Asphalt and grass";
                        break;
                    case CodeSurfaceComposition.CONC:
                        break;
                    case CodeSurfaceComposition.CONC_ASPH:
                        break;
                    case CodeSurfaceComposition.CONC_GRS:
                        break;
                    case CodeSurfaceComposition.GRASS:
                        break;
                    case CodeSurfaceComposition.SAND:
                        break;
                    case CodeSurfaceComposition.WATER:
                        break;
                    case CodeSurfaceComposition.BITUM:
                        break;
                    case CodeSurfaceComposition.BRICK:
                        break;
                    case CodeSurfaceComposition.MACADAM:
                        break;
                    case CodeSurfaceComposition.STONE:
                        break;
                    case CodeSurfaceComposition.CORAL:
                        break;
                    case CodeSurfaceComposition.CLAY:
                        break;
                    case CodeSurfaceComposition.LATERITE:
                        break;
                    case CodeSurfaceComposition.GRAVEL:
                        break;
                    case CodeSurfaceComposition.EARTH:
                        break;
                    case CodeSurfaceComposition.ICE:
                        break;
                    case CodeSurfaceComposition.SNOW:
                        break;
                    case CodeSurfaceComposition.MEMBRANE:
                        break;
                    case CodeSurfaceComposition.METAL:
                        break;
                    case CodeSurfaceComposition.MATS:
                        break;
                    case CodeSurfaceComposition.PIERCED_STEEL:
                        break;
                    case CodeSurfaceComposition.WOOD:
                        RunwayComposition = "Wood";
                        break;
                    case CodeSurfaceComposition.NON_BITUM_MIX:
                        break;
                    default:
                        break;
                }
            }
            NotifyPropertyChanged("");
        }

        private void backCommand_onClick(object obj)
        {
            PageIndex = 0;
            NotifyPropertyChanged("PageIndex");
        }

        

        private void SaveAdhpToGeoDatabase()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("Aerodrom") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find Aerodrome layer");
                return;
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            IFeatureClass featureClass = layer.FeatureClass;

            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = featureClass.Fields;

            IFeature feat = featureClass.CreateFeature();

            var airportHeliport = GlobalParams.Database.AirportHeliport;

            ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.Point();
            point.X = airportHeliport.ARP.Geo.X;
            point.Y = airportHeliport.ARP.Geo.Y;

            feat.set_Value(1, point);
            feat.set_Value(2, airportHeliport.Designator);
            if (airportHeliport.Name!=null && airportHeliport.Name.Length>3)
                feat.set_Value(3, airportHeliport.Name);
            feat.set_Value(4, airportHeliport.ARP.Elevation.Value);
            feat.set_Value(5, airportHeliport.ARP.Elevation.Uom.ToString());
            if (airportHeliport.MagneticVariation!=null)
                feat.set_Value(6, airportHeliport.MagneticVariation.Value);

            feat.set_Value(9, "{"+airportHeliport.Identifier+"}");
            feat.Store();
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            
        }

        private void SaveThresholdsToGeoDatabase()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("Thresholds") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find Thresholds layer");
                return;
            }

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            IFeatureClass featureClass = layer.FeatureClass;

            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = featureClass.Fields;

            foreach (var rwyDirection in _selectedRwyClass.RwyDirClassList)
            {
                IFeature feat = featureClass.CreateFeature();
                ESRI.ArcGIS.Geometry.IPoint point = new ESRI.ArcGIS.Geometry.Point();
                point.X = rwyDirection.StartCntlPt.Location.Geo.X ;
                point.Y = rwyDirection.StartCntlPt.Location.Geo.Y;
                feat.set_Value(1, point);
                feat.set_Value(2, rwyDirection.Name);
                feat.Store();
            }
         
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

        }
        private void SaveObstaclesToDb()
        {
            IFeatureLayer pointLayer = EsriFunctions.GetLayerByName("Obstacle_Point") as IFeatureLayer;
            IFeatureLayer polygonFeatureLayer = EsriFunctions.GetLayerByName("Obstacle_Polygon") as IFeatureLayer;
            IFeatureLayer polylineLayer = EsriFunctions.GetLayerByName("Obstacle_Line") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (pointLayer == null)
            {
                MessageBox.Show("Cannot find Point layer");
                return;
            }

            if (polygonFeatureLayer == null)
            {
                MessageBox.Show("Cannot find Polygon layer");
                return;
            }

            if (polylineLayer == null)
            {
                MessageBox.Show("Cannot find Polyline layer");
                return;
            }
            IFeatureClass pointFeatureClass = pointLayer.FeatureClass;
            IFeatureClass polygonFeatureClass = polygonFeatureLayer.FeatureClass;
            IFeatureClass polylineFeatureClass = polylineLayer.FeatureClass;


            IDataset dataset = pointLayer as IDataset;
            _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();


            ITable table = pointFeatureClass as ITable;
            table.DeleteSearchedRows(null);

            table = polygonFeatureClass as ITable;
            table.DeleteSearchedRows(null);

            table = polylineFeatureClass as ITable;
            table.DeleteSearchedRows(null);

            var reportList = (from surface in AvailableSurfaceList
                              from report in surface.SurfaceBase.Report
                              group report by new { report.Id } //or group by new {p.ID, p.Name, p.Whatever}
                                  into mygroup
                                  select mygroup.OrderByDescending(report => report.Penetrate).First()).ToList<ObstacleReport>();

            foreach (var reportItem in reportList)
            {
                IFeature feat = null;
                if (reportItem.GeomType == ObstacleGeomType.Point)
                    feat = pointFeatureClass.CreateFeature();
                else if (reportItem.GeomType == ObstacleGeomType.Polygon)
                    feat = polygonFeatureClass.CreateFeature();
                else
                    feat = polylineFeatureClass.CreateFeature();

                var geometry = Converters.ConvertToEsriGeom.FromGeometry(GlobalParams.SpatialRefOperation.ToGeo(reportItem.GeomPrj));

                var zAware = (ESRI.ArcGIS.Geometry.IZAware)geometry;
                zAware.ZAware = false;

                feat.set_Value(1, geometry);
                feat.set_Value(2, reportItem.Name);
                if (reportItem.VsType != null)
                    feat.set_Value(3, reportItem.VsType);
                else 
                {
                    if (reportItem.Name.StartsWith("Ter"))
                        feat.set_Value(3, "Sport Height");
                    else
                        feat.set_Value(3, "Antenna");
                }
                feat.set_Value(4, reportItem.Obstacle.Lighted);
                feat.set_Value(5, reportItem.Obstacle.Group);
                double verticalExtent = 0;

                foreach (var vsPart in reportItem.Obstacle.Part)
                {
                    if (reportItem.GeomType == ObstacleGeomType.Point && vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedPoint)
                    {
                        if (vsPart.VerticalExtent != null)
                        {
                            feat.set_Value(6, Converters.ConverterToSI.Convert(vsPart.VerticalExtent.Value,0));
                            if (vsPart.VerticalExtentAccuracy != null) 
                            {
                                feat.set_Value(7, Converters.ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, 0));
                            }
                            //feat.set_Value(7, vsPart.VerticalExtent.Uom);
                        }

                        if (vsPart.HorizontalProjection.Location.Elevation!=null)
                            feat.set_Value(8,Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.Location.Elevation, 0));
                        
                    }
                    else if (reportItem.GeomType == ObstacleGeomType.Polygon && vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedSurface)
                    {
                        if (vsPart.VerticalExtent != null)
                        {
                            feat.set_Value(6, vsPart.VerticalExtent.Value);
                            if (vsPart.VerticalExtentAccuracy != null)
                            {
                                feat.set_Value(7, Converters.ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, 0));
                            }
                            //feat.set_Value(7, vsPart.VerticalExtent.Uom);
                        }

                        if (vsPart.HorizontalProjection.SurfaceExtent.Elevation != null)
                            feat.set_Value(8, Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.SurfaceExtent.Elevation, 0));

                    }
                    else if (reportItem.GeomType == ObstacleGeomType.PolyLine && vsPart.HorizontalProjection.Choice == Aim.VerticalStructurePartGeometryChoice.ElevatedCurve)
                    {
                        if (vsPart.VerticalExtent != null)
                        {
                            feat.set_Value(6, vsPart.VerticalExtent.Value);
                            if (vsPart.VerticalExtentAccuracy != null)
                            {
                                feat.set_Value(7, Converters.ConverterToSI.Convert(vsPart.VerticalExtentAccuracy, 0));
                            }
                            //feat.set_Value(7, vsPart.VerticalExtent.Uom);
                        }

                        if (vsPart.HorizontalProjection.LinearExtent.Elevation != null)
                            feat.set_Value(8, Converters.ConverterToSI.Convert(vsPart.HorizontalProjection.LinearExtent.Elevation, 0));
                    }
                }
                feat.set_Value(10, "M");
                feat.set_Value(9, reportItem.Penetrate);
                feat.set_Value(11, "{" + reportItem.Obstacle.Identifier + "}");
                feat.Store();
            }
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        private void SaveObstacleAreaToDb()
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("ObstacleArea") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("Cannot find ObstacleArea layer");
                return;
            }
            var featureClass = layer.FeatureClass;

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(null);

            IFields fields = featureClass.Fields;

            foreach (var surface in AvailableSurfaceList)
            {
                ESRI.ArcGIS.Geometry.IPolygon esriGeo = new ESRI.ArcGIS.Geometry.Polygon() as ESRI.ArcGIS.Geometry.IPolygon;

                var geomPrj = surface.SurfaceBase.GetCuttingGeometry();
                if (geomPrj == null || geomPrj.IsEmpty)
                    continue;
                if (surface.SurfaceBase.SurfaceType == SurfaceType.Approach)
                {
                    var approach = surface.SurfaceBase as Approach;
                    var appPlanes = approach.CuttingGeo1Planes.Concat(approach.CuttingGeo2Planes);
                    foreach (var plane in appPlanes)
                    {
                        IFeature featApproach = featureClass.CreateFeature();
                        var poly = plane.Geo;
                        var planeGeo = GlobalParams.SpatialRefOperation.ToGeo(poly);
                        var esriPlaneGeo = Converters.ConvertToEsriGeom.FromMultiPolygon(planeGeo,false);
                        featApproach.set_Value(1, esriPlaneGeo);
                        featApproach.set_Value(2, approach.SurfaceType.ToString()+"SURFACE ");
                        string value = "";
                        if (Math.Abs(plane.Slope)<0.01)
                            value = Common.ConvertHeight(approach.FinalElevation,Enums.RoundType.ToNearest)+" "+InitOmega.HeightConverter.Unit;
                        else
                            value = "Slope 1:" + Convert.ToString(Math.Round(100 / Convert.ToDouble(plane.Slope)));
                        featApproach.set_Value(3, value);
                        featApproach.Store();
                    }

                    continue;
                }

                IFeature feat = featureClass.CreateFeature();
                var surfaceGeo = GlobalParams.SpatialRefOperation.ToGeo(geomPrj);
                esriGeo = Converters.ConvertToEsriGeom.FromMultiPolygon(surfaceGeo,false);

                feat.set_Value(1, esriGeo);
                string surfacename = "";

                foreach (char letter in surface.SurfaceBase.SurfaceType.ToString())
                {
                    if (Char.IsUpper(letter) && 
                        surfacename.Length > 0)
                     surfacename += " " + letter;
                   else
                     surfacename += letter;
                }

                feat.set_Value(2, surfacename+" SURFACE");
                var slope = "";
                foreach (var property in surface.SurfaceBase.PropertyList)
                {
                    if (property.Name == "Slope" && property.Value!=null)
                        slope = "Slope 1: " + Convert.ToString(Math.Round(100 / Convert.ToDouble(property.Value)));
                }
                if (slope == "")
                {
                    if (surface.SurfaceBase.SurfaceType == SurfaceType.InnerHorizontal)
                        slope = Common.ConvertHeight((surface.SurfaceBase as InnerHorizontal).Elevation, Enums.RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
                    else if (surface.SurfaceBase.SurfaceType == SurfaceType.CONICAL)
                        slope = Common.ConvertHeight((surface.SurfaceBase as ConicalSurface).Elevation, Enums.RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
                    else if (surface.SurfaceBase.SurfaceType == SurfaceType.OuterHorizontal)
                        slope = Common.ConvertHeight((surface.SurfaceBase as OuterHorizontal).Elevation, Enums.RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
                }
                feat.set_Value(3, slope);
                feat.Store();
                
            }
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }

        private bool SaveRunwayElementToDb() 
        {
            IFeatureLayer layer = EsriFunctions.GetLayerByName("RunwayElement") as IFeatureLayer;
            //IFeatureClass featureClass= _featureWorkspace.OpenFeatureClass("Aerodrom");
            if (layer == null)
            {
                MessageBox.Show("RunwayElement Layer is not found!", "TypeB", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var featClass = layer.FeatureClass;

            IDataset dataset = layer as IDataset;
            _workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)_workspace;
            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            RunwayElement runwayElement = GlobalParams.Database.GetRunwayelement(_selectedRwyClass.Identifier);
         
            ITable table = featClass as ITable;
            table.DeleteSearchedRows(null);
            IFeature feat = featClass.CreateFeature();
            
            if (runwayElement == null)
            {
                MessageBox.Show("There are not any RunwayElement", "TypeB", MessageBoxButton.OK, MessageBoxImage.Warning);
                double runwayElementWidth = Converters.ConverterToSI.Convert(_selectedRwyClass.Runway.NominalWidth, 45);
                var startPrj = GlobalParams.SpatialRefOperation.ToPrj(_selectedRwyDirection.StartCntlPt.Location.Geo);
                var endPrj = GlobalParams.SpatialRefOperation.ToPrj(_selectedRwyDirection.EndCntlPt.Location.Geo);
                var dir = ARANFunctions.ReturnAngleInRadians(startPrj, endPrj);
                //create runwayelement geo
                var tmpPt1 = ARANFunctions.LocalToPrj(startPrj, dir, 0, runwayElementWidth / 2);
                var tmpPt2 = ARANFunctions.LocalToPrj(startPrj, dir, 0, -runwayElementWidth / 2);
                var tmpPt3 = ARANFunctions.LocalToPrj(endPrj, dir, 0, -runwayElementWidth / 2);
                var tmpPt4 = ARANFunctions.LocalToPrj(endPrj, dir, 0, runwayElementWidth / 2);
                var runwayElementGeo =GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Polygon { ExteriorRing = new Ring { tmpPt1, tmpPt2, tmpPt3, tmpPt4 } });
                var esriPolygon =  Aran.Converters.ConvertToEsriGeom.FromPolygon(runwayElementGeo,false);
                
                feat.set_Value(1, esriPolygon);
                feat.set_Value(2, _selectedRwyClass.Name);
                feat.set_Value(3, _selectedRwyClass.Runway.Type.ToString());
                if (_selectedRwyClass.Runway.NominalLength!=null)
                    feat.set_Value(4, _selectedRwyClass.Runway.NominalLength.Value);
                if (_selectedRwyClass.Runway.NominalWidth != null)
                {
                    feat.set_Value(5, _selectedRwyClass.Runway.NominalWidth.Value);
                    feat.set_Value(6, _selectedRwyClass.Runway.NominalWidth.Uom.ToString());
                }
                else
                {
                    feat.set_Value(5, runwayElementWidth);
                    feat.set_Value(6, "M");
                }
                feat.set_Value(8, "{" + _selectedRwyClass.Identifier + "}");
            }
            else 
            {
                var runwayElementGeo = runwayElement.Extent.Geo;
                var esriPolygon =Aran.Converters.ConvertToEsriGeom.FromMultiPolygon(runwayElementGeo);
                feat.set_Value(1, esriPolygon);
                feat.set_Value(2, _selectedRwyClass.Name);
                feat.set_Value(3, runwayElement.Type);
                feat.set_Value(4,runwayElement.Length.Value);
                feat.set_Value(5, runwayElement.Width.Value);
                feat.set_Value(6, runwayElement.Width.Uom);
                feat.set_Value(8, "{" + _selectedRwyClass.Identifier + "}");
            }
            feat.Store();
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
            return true;
        
        }

        private void ExportValidationToHtml()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "OmegaValidation"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Title = "Save Omega Validation Report";
            dlg.Filter = "Html documents|*.htm";
            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result != true) return;

            System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);

            string html = "<html><head>" +
                             "<meta http-equiv='content-type;' content='text/html;charset=utf-8' />" +
                             "<style type='text/css'>" +
                                 ".htmlReport { font: 13px Verdana;font-weight:bold}" +
                                 ".body { font: 11.5px Verdana;line-height:19px;margin-left:15px; }" +
                            "</style></head>";
            html += "<body>" +
                       "<div class='htmlReport'>" +
                       "<center>Omega Validation Report<br /></center>" +
                       "<div class='body'><br />Report Generated: " + DateTime.Now + "<br /> " +
                       "Aerodrome: " + GlobalParams.Database.AirportHeliport.Designator + "(" + GlobalParams.Database.AirportHeliport.Name + ")<br />";

            foreach (var rwyClass in RwyClassList)
            {
                html += "Rwy : " + rwyClass.Name + "<br />" +
                        "Nominal Length: " + rwyClass.Length + " " + InitOmega.DistanceConverter.Unit + " <br />" +
                        "Calculation Length: " + rwyClass.RwyDirClassList[0].CalculationLength + " " + InitOmega.DistanceConverter.Unit + " <br />";
                foreach (var rwyDir in rwyClass.RwyDirClassList)
                {
                    html += "Rwy direction : " + rwyDir.Name +
                            "<div style='margin-left:10px'>" +
                            " -True direction : " + Math.Round(ARANMath.Modulus(rwyDir.Direction), 1) + "<br />" +
                            " -TDZ Elevation : " + Common.ConvertHeight(rwyDir.TDZElevation, Enums.RoundType.ToNearest) + " " + InitOmega.HeightConverter.Unit + "<br />";

                    if (rwyDir.Validation.DeclaredDistanceNotAvailable)
                        html += "<span style='color:red;font-weight:bold'>" + rwyDir.StartCntlPt.Designator + " Declared distances not available<br /></span>";
                    else
                        html += " -Declared distances <br />" +
                                "   -TORA : " + rwyDir.Tora + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -TODA : " + rwyDir.Toda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -ASDA : " + rwyDir.Asda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -ClearWay : " + rwyDir.ClearWay + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -StopWay : " + rwyDir.StopWay + " " + InitOmega.DistanceConverter.Unit + "<br />";

                    if (!string.IsNullOrEmpty(rwyDir.Validation.ShiftLogs))
                        html += " -Center Line Points shifts" +
                               "<div style='padding-left:15px;color:red;font-weight:bold'>" + rwyDir.Validation.ShiftLogs + "</div>";

                    html += "</div>";
                }
            }
            html += "</div></div></body></html>";

            stream.Write(Encoding.ASCII.GetBytes(html), 0, html.Length);
            stream.Close();

            MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void rwyClass_RwyCheckedIsChanged(object sender, EventArgs e)
        {
            var selectedRwy = sender as RwyClass;
            _selectedRwyClass = selectedRwy;

            var selectedList = RwyClassList.Where(rwyClass => rwyClass.Checked == true).ToList();
            if (selectedList.Count == 0 && !selectedRwy.Checked)
            {
                selectedRwy.ChangeChecked(true);
                return;
            }

            if (RwyClassList.Count > 1 && selectedRwy.Checked)
            {
                foreach (var rwyClass in RwyClassList)
                {
                    if (rwyClass.Checked && !rwyClass.Name.Equals(selectedRwy.Name)
                                        && rwyClass.CodeNumber != selectedRwy.CodeNumber)
                    {
                        rwyClass.ChangeChecked(false);
                    }
                }

            }


            RwyDirClassList.Clear();
            foreach (var rwyClass in selectedList)
            {
                if (rwyClass.Checked)
                    foreach (var rwyDir in rwyClass.RwyDirClassList)
                    {
                        RwyDirClassList.Add(rwyDir);
                    }
            }

            //If runway code number 4 then code letter f must be enabled
            CodeLetterFIsEnabled = false;
            foreach (var rwyClass in RwyClassList)
            {
                if (rwyClass.Checked && rwyClass.CodeNumber == 4)
                {
                    CodeLetterFIsEnabled = true;
                }
            }

            //end 

            if (RwyDirClassList.Count > 0)
                SelectedRwyDirection = RwyDirClassList[0];

            NotifyPropertyChanged("SelectedRwyDirection");
            NotifyPropertyChanged("RwyDirClassList");
            NotifyPropertyChanged("CodeLetterFIsEnabled");
        }

        private void GetSurfaceList()
        {
            try
            {
                List<RunwayConstants> rwyConstantList = GlobalParams.Constant.RunwayConstants.List;
                rwyConstantList = rwyConstantList.OrderBy(rwyConst => rwyConst.Order).ToList<RunwayConstants>();

                List<SurfaceType> tmpSurfaces = new List<SurfaceType>();
                CategoryNumber catNumber = CategoryNumber.One;
                if (SelectedCategory != null)
                    catNumber = SelectedCategory.Enum;

                foreach (RunwayConstants rwyConstant in rwyConstantList)
                {
                    if (tmpSurfaces.Contains(rwyConstant.Surface))
                        continue;
                    try
                    {
                        double val = rwyConstant.GetValue(SelectedClassification.Enum, catNumber, _selectedRwyClass.CodeNumber);
                        if ((val > 0.1 || rwyConstant.Surface == SurfaceType.TakeOffClimb) && (!(rwyConstant.Surface== SurfaceType.BalkedLanding || rwyConstant.Surface== SurfaceType.InnerApproach || rwyConstant.Surface== SurfaceType.InnerTransitional || rwyConstant.Surface== SurfaceType.Strip)))
                        {
                            tmpSurfaces.Add(rwyConstant.Surface);
                            AvailableSurfaceList.Add(new DrawingSurface(rwyConstant));
                        }
                    }
                    catch (Exception e)
                    {
                        _errorLog += e.Message + "#13#10";
                    }
                }
            }
            catch (Exception e)
            {
                _errorLog += e.Message + "#13#10";
            }
        }

        private RunwayConstants StripConstant()
        {
            var stripConstant = new RunwayConstants
            {
                Order = 5,
                Surface = SurfaceType.Strip,
                SurfaceName = "STRIP"
            };
            return stripConstant;
        }
        
        #endregion
    }
}
