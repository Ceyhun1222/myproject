using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Converters;
using Aran.Omega.Enums;
using Aran.Omega.Export;
using Aran.Omega.Models;
using Aran.Omega.SettingsUI;
using Aran.Omega.Validation;
using Aran.Omega.View;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using Aran.Queries;

namespace Aran.Omega.ViewModels
{
    public class EtodViewModel : ViewModel
    {
        #region :>Fields

        private EnumName<RunwayClassificationType> _selClassification;
        private EnumName<CategoryNumber> _selCatNumber;
        private ObservableCollection<EnumName<CategoryNumber>> _categoryList;
        private ElevationDatum _selElevationDatum;
        private RwyClass _selectedRwyClass;
        private Annex15Surfaces _annex15Surfaces;
        private bool _reportIsCalculated;
        private bool _saveIsEnabled;

        private ManualReport _obstacleInputWindow;
        private Airspace _selectedFirAirspace;
        private Aran.Geometries.MultiPolygon _firGeometry;
        private Aran.Geometries.MultiPoint _firMltPt;
        private int _firGeometryHandle;


        private Cursor _currCursor;
        private double _lenghtOffInnerEdge;

        #endregion

        #region :>Constructor

        public EtodViewModel()
        {

        }

        #endregion

        #region :>Property

        #region :>Lists

        public ObservableCollection<RwyClass> RwyClassList { get; set; }
        public ObservableCollection<EnumName<RunwayClassificationType>> ClassificationList { get; private set; }
        public ObservableCollection<EnumName<CategoryNumber>> CategoryList { get; private set; }
        public ObservableCollection<ElevationDatum> ElevationDatumList { get; set; }
        public ObservableCollection<DrawingSurface> AvailableSurfaceList { get; set; }
        public ObservableCollection<RwyDirClass> RwyDirClassList { get; set; }
        public List<Airspace> FirAirspaceList { get; set; }

        #endregion

        public Annex15Surfaces Annex15Surfaces
        {
            get { return _annex15Surfaces; }
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

                CalculateInnerLengthOfEdge();

                NotifyPropertyChanged("SelectedCategory");
            }
        }

        public EnumName<CategoryNumber> SelectedCategory
        {
            get => _selCatNumber;
            set
            {
                if (_selCatNumber == value)
                    return;

                _selCatNumber = value;

                CalculateInnerLengthOfEdge();
                NotifyPropertyChanged("CategoryIsEnabled");
                NotifyPropertyChanged("SelectedCategory");
            }
        }

        public bool CategoryIsEnabled => CategoryList.Count > 0;

        // Fir airspace
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

                        var firGeo = _selectedFirAirspace.GeometryComponent[0]
                            .TheAirspaceVolume.HorizontalProjection.Geo;
                        _firGeometry = GlobalParams.SpatialRefOperation.ToPrj(firGeo);
                        GlobalParams.FirGeomPrj = _firGeometry;
                        //_firMltPrj = _firGeometry.ToMultiPoint();
                        //_firMltPt = firGeo.ToMultiPoint();
                        //if (DrawFirIsChecked)
                        //{
                        GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
                        _firGeometryHandle = GlobalParams.UI.DrawMultiPolygon(_firGeometry, eFillStyle.sfsNull, 121212);
                        //}

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error occured when get geometry Airspace");
                    }
                }
                NotifyPropertyChanged("SelectedFirAirspace");
            }
        }

        public ElevationDatum SelectedElevationDatum
        {
            get { return _selElevationDatum; }
            set
            {
                _selElevationDatum = value;
                NotifyPropertyChanged("SelectedElevationDatum");
            }
        }

        public ICommand CalculateCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand ReportCommand => new RelayCommand(async obj =>
        {
            //Change window cursor to wait mode
            try
            {


                GridIsEnabled = false;
                IsLoading = true;

                if (!_reportIsCalculated)
                    await CommonFunctions.GetandConvertVsToLocalParam();

                IList<ObstacleReport> tmpReport = null;
                await Task.Factory.StartNew(() =>
                {
                    foreach (var surface in AvailableSurfaceList)
                    {
                        if (surface.SurfaceBase != null)
                            tmpReport = surface.SurfaceBase.FilteredReport;
                    }
                });

                if (AvailableSurfaceList.Count <= 0) return;

                var reportWindow = new EtodReport(this);
                var helper = new WindowInteropHelper(reportWindow);
                ElementHost.EnableModelessKeyboardInterop(reportWindow);
                helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                reportWindow.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                reportWindow.Show();

                _reportIsCalculated = true;
                SaveIsEnabled = true;

                NotifyPropertyChanged(nameof(ExportAsEtodIsVisible));
            }
            finally
            {
                IsLoading = false;
                GridIsEnabled = true;
            }
        });

        public ICommand ValidationReport { get; set; }

        public ICommand ClearCommand { get; set; }

        public ICommand ObstacleInputCommand { get; set; }

        public ICommand ExportSurface
        {
            get
            {
                return new RelayCommand((surface) =>
                {
                    try
                    {
                        var olsSurface = surface as DrawingSurface;
                        if (olsSurface == null)
                        {
                            MessageBox.Show("Command parametr is not correc!", "Omega", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }
                        var folderPath = ExportFolderPath.GetFolderPath(SelectedRwyDirection,SelectedClassification, SelectedCategory);

                        CurrCursor = Cursors.Arrow;
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            MessageBox.Show("Canceled!");
                            return;
                        }

                        ExportToGdb helper = new ExportToGdb(olsSurface);
                        helper.ExportAll(folderPath,ExportType.Geodatabase);

                        MessageBox.Show("Surface has exported successfully");
                    }
                    catch (Exception ex)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(ex);
                        MessageBox.Show(ex.Message);
                    }

                    finally
                    {
                        CurrCursor = Cursors.Arrow;
                    }
                });
            }
        }

        public RelayCommand ExportCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        var folderPath = ExportFolderPath.GetFolderPath(SelectedRwyDirection, SelectedClassification, SelectedCategory);

                        CurrCursor = Cursors.Arrow;
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            MessageBox.Show("Canceled!");
                            return;
                        }

                        ExportToGdb helper = new ExportToGdb(AvailableSurfaceList.ToList());
                        helper.ExportAll(folderPath,ExportType.Geodatabase);

                        MessageBox.Show("All data has exported successfully");
                    }
                    catch (Exception ex)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(ex);
                        MessageBox.Show(ex.Message);
                    }

                    finally
                    {
                        CurrCursor = Cursors.Arrow;
                    }
                });
            }
        }

        public ICommand ExportAsEtodCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    try
                    {
                        var folderPath = ExportFolderPath.GetFolderPath(SelectedRwyDirection, SelectedClassification,
                            SelectedCategory);

                        CurrCursor = Cursors.Arrow;
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            MessageBox.Show("Canceled!");
                            return;
                        }

                        EtodExportToGdbHelper gdbHelper = new EtodExportToGdbHelper(null,GlobalParams.Database.AirportHeliport,_selectedRwyDirection);
                        gdbHelper.Export(AvailableSurfaceList.Where(surface=>surface.SurfaceBase.EtodSurfaceType!=EtodSurfaceType.Area1), folderPath, $"Etod_Obstacles.mdb");

                        ExportArea1(gdbHelper, folderPath);

                        MessageBox.Show("All data has exported successfully");
                    }
                    catch (Exception ex)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(ex);
                        MessageBox.Show(ex.Message);
                    }

                    finally
                    {
                        CurrCursor = Cursors.Arrow;
                    }
                });
            }
        }

        private void ExportArea1(EtodExportToGdbHelper gdbHelper, string folderPath)
        {
            if (MessageBox.Show($"Do you want to export Area1 obstacles?", "Omega", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            var area1 = AvailableSurfaceList.FirstOrDefault(surface =>
                surface.SurfaceBase.EtodSurfaceType == EtodSurfaceType.Area1);
            if (area1 != null)
            {
                gdbHelper.Export(new List<DrawingSurface>(){area1}, folderPath, $"Etod_Area1_Obstacles.mdb");
            }
        }

        public ICommand InfoCommand
        {
            get
            {
                return new RelayCommand((surface) =>
                {

                    var olsSurface = surface as DrawingSurface;
                    if (olsSurface == null)
                    {
                        MessageBox.Show("Command parametr is not correc!", "Omega", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    ShowSurfaceInfo.Show(olsSurface);
                });
            }
        }

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

        public Cursor CurrCursor
        {
            get => _currCursor;
            set 
            {
                _currCursor = value;
                NotifyPropertyChanged(nameof(CurrCursor));
            }
        }
        
        public string Title { get; set; }

        public bool IsMountain { get; set; }
        public bool ChangesGreaterThanFifteenIsEnabled { get; set; }

        public Action CloseAction { get; set; }

        public bool ReportIsEnabled
        {
            get { return AvailableSurfaceList.Count > 0; }
        }

        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set
            {
                _saveIsEnabled = value;
                NotifyPropertyChanged(nameof(SaveIsEnabled));
            }
        }

        public bool ManualReportIsEnabled => AvailableSurfaceList.Count > 0;

        public double LengthOffInnerEdge
        {
            get { return _lenghtOffInnerEdge; }
            set
            {
                _lenghtOffInnerEdge = value;
                NotifyPropertyChanged(nameof(LengthOffInnerEdge));
            }
        }

        private bool _IsLoading;

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyPropertyChanged(nameof(IsLoading));
            }
        }

        private bool _gridIsEnabled = true;
        public bool GridIsEnabled
        {
            get { return _gridIsEnabled; }
            set
            {
                _gridIsEnabled = value;
                NotifyPropertyChanged(nameof(GridIsEnabled));
            }
        }

        public bool ExportAsEtodIsVisible
        {
            get
            {
#if (Kaz)
                return SaveIsEnabled;
#endif
                return false;

            }
        }


        #endregion

#region :>Methods

#region :>Events

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ClearAll(null);
        }

        private void CreateElevationDatum()
        {
            ElevationDatumList = new ObservableCollection<ElevationDatum>();

            var adhpElevationDatum = new ElevationDatum(GlobalParams.Database.AirportHeliport,"ARP");
            ElevationDatumList.Add(adhpElevationDatum);

            if (_selectedRwyDirection == null)
                return;

            var startElevationDatum = new ElevationDatum(_selectedRwyDirection.StartCntlPt,"THR "+_selectedRwyDirection.Name);
            var endElevationDatum = new ElevationDatum(_selectedRwyDirection.EndCntlPt,"THR"+_selectedRwyDirection.Name);
            var tdzElevationDatum = new ElevationDatum(_selectedRwyDirection.TDZElevation, "TDZ");
            ElevationDatumList.Add(startElevationDatum);
            ElevationDatumList.Add(endElevationDatum);
            ElevationDatumList.Add(tdzElevationDatum);
            NotifyPropertyChanged("ElevationDatumList");
            SelectedElevationDatum = ElevationDatumList[0];
        }

        private void calculateCommand_onClick(object obj)
        {
            //Change window cursor to wait mode
            int i = 0;
            try
            {
                CurrCursor = Cursors.Wait;
                NotifyPropertyChanged(nameof(CurrCursor));

                ClearAll(null);
                AvailableSurfaceList.Clear();

                _selectedRwyClass.RwyDirClassList[0].SelectedClassification = SelectedClassification.Enum;
                if (_selCatNumber != null)
                    _selectedRwyClass.RwyDirClassList[0].SelectedCategory = _selCatNumber.Enum;

                var catNumber = CategoryNumber.One;
                if (_selCatNumber != null)
                    catNumber = _selCatNumber.Enum;

                _annex15Surfaces = new Annex15Surfaces(_selectedRwyClass, SelectedRwyDirection,
                    SelectedClassification.Enum, catNumber, _selectedRwyClass.CodeNumber,LengthOffInnerEdge);

                var drawingSurface = new Aran.Omega.Models.DrawingSurface("Area2A");
                drawingSurface.RwyDirClass = _selectedRwyDirection;
                var area2A = _annex15Surfaces.CreateArea2A();
                drawingSurface.SurfaceBase = area2A;
                AvailableSurfaceList.Add(drawingSurface);

                var area2B = new Aran.Omega.Models.DrawingSurface("Area2B");
                area2B.RwyDirClass = _selectedRwyDirection;
                var area2BSurfaceBase = _annex15Surfaces.CreateArea2B();
                area2B.SurfaceBase = area2BSurfaceBase;
                AvailableSurfaceList.Add(area2B);

                var area2C = new Aran.Omega.Models.DrawingSurface("Area2C");
                area2C.RwyDirClass = _selectedRwyDirection;
                var area2CSurfaceBase = _annex15Surfaces.CreateArea2C();
                area2C.SurfaceBase = area2CSurfaceBase;
                AvailableSurfaceList.Add(area2C);

                var area2D = new Aran.Omega.Models.DrawingSurface("Area2D");
                area2D.RwyDirClass = _selectedRwyDirection;
                var area2DSurfaceBase = _annex15Surfaces.CreateArea2D();
                area2D.SurfaceBase = area2DSurfaceBase;
                AvailableSurfaceList.Add(area2D);

                var area3 = new Aran.Omega.Models.DrawingSurface("Area3");
                area3.RwyDirClass = _selectedRwyDirection;
                var area3SurfaceBase = _annex15Surfaces.CreateArea3();
                if (area3SurfaceBase != null)
                {
                    area3.SurfaceBase = area3SurfaceBase;
                    AvailableSurfaceList.Add(area3);
                }
                if (SelectedClassification.Enum == RunwayClassificationType.PrecisionApproach &&
                    _selCatNumber.Enum == CategoryNumber.Two)
                {
                    var area4 = new Aran.Omega.Models.DrawingSurface("Area4");
                    area4.RwyDirClass = _selectedRwyDirection;
                    var area4SurfaceBase = _annex15Surfaces.CreateArea4(IsMountain);
                    area4.SurfaceBase = area4SurfaceBase;
                    AvailableSurfaceList.Add(area4);
                }


                //var list = new List<DrawingSurface>(AvailableSurfaceList);

                var stackList = new Stack<DrawingSurface>(AvailableSurfaceList.Reverse());

                foreach (var settingsModel in GlobalParams.Settings.OLSModelList)
                {
                    if (settingsModel.Type == MenuType.Surface)
                    {
                        if (stackList.Count > 0)
                        {
                            var surfaceModel = settingsModel as SurfaceModel;
                            if (surfaceModel != null)
                            {
                                var curObj = stackList.Pop();
                                if (curObj == null) break;
                                curObj.SurfaceBase.DefaultSymbol = surfaceModel.Symbol;
                                curObj.SurfaceBase.SelectedSymbol = surfaceModel.SelectedSymbol;
                                curObj.SurfaceBase.Draw(false);
                                //list[0].SurfaceBase.Draw(false);
                            }
                        }
                    }
                }


                //Change window cursor to normal mode

                SaveIsEnabled = false;
                NotifyPropertyChanged(nameof(ReportIsEnabled));
                NotifyPropertyChanged(nameof(ManualReportIsEnabled));
                NotifyPropertyChanged(nameof(ExportAsEtodIsVisible));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GlobalParams.AranEnvironment?.GetLogger("Omega").Error(ex, ex.Message);
            }
            finally
            {
                CurrCursor = Cursors.Arrow;
                _reportIsCalculated = false;
                NotifyPropertyChanged(nameof(CurrCursor));
            }
        }

        private void SaveToDb(object obj)
        {
            try
            {
                if (AvailableSurfaceList.Count > 0)
                {
                    var window = new EtodSaveDbView(AvailableSurfaceList.ToList());
                    var helper = new WindowInteropHelper(window);
                    ElementHost.EnableModelessKeyboardInterop(window);
                    helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.Show();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error when trying save Feature to Database! "+e.Message);
            }
        }
        
        private void ObstaclInput(object obj)
        {
            if (_obstacleInputWindow == null || _obstacleInputWindow.IsClosed)
            {
                _obstacleInputWindow = new ManualReport();
                var helper = new WindowInteropHelper(_obstacleInputWindow);
                ElementHost.EnableModelessKeyboardInterop(_obstacleInputWindow);
                helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                _obstacleInputWindow.ShowInTaskbar = false;
                _obstacleInputWindow.SetAvailableSurfaces(AvailableSurfaceList);// hide from taskbar and alt-tab list
                _obstacleInputWindow.Show();
            }
        }

#endregion

        private void ClearAll(object obj)
        {
            foreach (DrawingSurface surface in AvailableSurfaceList)
            {
                if (surface.SurfaceBase != null)
                    surface.SurfaceBase.ClearAll();
            }
            GlobalParams.UI.SafeDeleteGraphic(_firGeometryHandle);
        }

        public void Init()
        {
            try
            {
                AvailableSurfaceList = new ObservableCollection<DrawingSurface>();
                CommonParamModel = new CommonParam(InitOmega.ObstacleDistanceFromArp);
                CalculateCommand = new RelayCommand(calculateCommand_onClick);
                SaveCommand = new RelayCommand(SaveToDb);
                ValidationReport = new RelayCommand(validation_onClick);
                ClearCommand = new RelayCommand(ClearAll);
                ObstacleInputCommand = new RelayCommand(ObstaclInput);
                RwyDirClassList = new ObservableCollection<RwyDirClass>();
                Title = "Omega(Annex 15) - Airport /Heliport: " + GlobalParams.Database.AirportHeliport.Designator;


                CurrCursor = Cursors.Arrow;
                _reportIsCalculated = false;
                SaveIsEnabled = false;

                RwyClassList = new ObservableCollection<RwyClass>();
                var runwayList = GlobalParams.Database.Runways;

                for (int i = 0; i < runwayList.Count; i++)
                {
                    var rwyClass = new RwyClass(runwayList[i]);
                    rwyClass.RwyCheckedIsChanged += new EventHandler(rwyClass_RwyCheckedIsChanged);
                    if (i == 0)
                    {
                        rwyClass.ChangeChecked(true);
                        _selectedRwyClass = rwyClass;
                    }
                    RwyClassList.Add(rwyClass);
                }
                rwyClass_RwyCheckedIsChanged(_selectedRwyClass, new EventArgs());

                if (GlobalParams.Settings.OLSQuery.ValidationReportIsCheked)
                {
                    var result = MessageBox.Show("Do you want to save validation report!", "eTOD Validation report",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        ExportValidationToHtml();
                }

                //Init Category constant params
                _categoryList = new ObservableCollection<EnumName<CategoryNumber>>
                {
                    new EnumName<CategoryNumber> {Name = "I", Enum = CategoryNumber.One},
                    new EnumName<CategoryNumber> {Name = "II,III", Enum = CategoryNumber.Two}
                };
                CategoryList = new ObservableCollection<EnumName<CategoryNumber>>();

                //Init Classifaction constant params
                ClassificationList = new ObservableCollection<EnumName<RunwayClassificationType>>
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
                SelectedClassification = ClassificationList[2];

                CreateElevationDatum();

                FirAirspaceList = GlobalParams.Database.AirspaceList?
                                                .Where(airsp => airsp.Type == CodeAirspace.FIR).ToList();

                SelectedFirAirspace = FirAirspaceList.FirstOrDefault(airspace =>
                {
                    foreach (var geoComp in airspace.GeometryComponent)
                    {
                        var geo = geoComp.TheAirspaceVolume?.HorizontalProjection?.Geo;
                        if (geo != null) {
                            var isInside= geo.IsPointInside(GlobalParams.Database.AirportHeliport.ARP.Geo);
                            if (isInside)
                                return true;
                        }
                    }
                    return false;
                });

            }
            catch (Exception e)
            {
                throw new Exception("Error load main form!" + Environment.NewLine + e.Message);

            }

        }

        private async void validation_onClick(object obj)
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await SaveValidationReportAsync(null, null).ConfigureAwait(false);
        }

        public async Task SaveValidationReportAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                EtodValidationReport etodValReport = new EtodValidationReport(AvailableSurfaceList.ToList());
                var saveReport = await etodValReport.SaveAsync(RwyClassList.ToList()).ConfigureAwait(false);
                if (saveReport)
                    MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK,
                        MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                GlobalParams.Logger.Error(ex);
                MessageBox.Show(ex.Message, "Omega", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void ExportValidationToHtml()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "EtodAerodromeValidation"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Title = "Save eTOD Validation Report";
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
                       "Organisation: " + GlobalParams.Database.Organisation.Designator + "(" + GlobalParams.Database.Organisation.Name + ")<br />" +
                       "Airport/Heliport: " + GlobalParams.Database.AirportHeliport.Designator + "(" + GlobalParams.Database.AirportHeliport.Name + ")<br />";
            if (GlobalParams.Database.GetTma() != null)
                html += "Airspace(TMA): Yes<br />";
            else
                html += "<span style='color:red;font-weight:bold'>Airspace(TMA): No(It is optional parametr for creating Area2D coverage area)</span></br>";

             var guidanceLineList =GlobalParams.Database.GetGuidanceLine();
             if (guidanceLineList.Count == 0)
             {
                 html += "<span style='color:red;font-weight:bold'>GuidanceLine: There are not any GuidanceLine.GuidanceLine are strictly required for creating of Area 3</span>";
             }
             else
             {
                 foreach (var guidanceLine in guidanceLineList)
                 {
                     if (guidanceLine.ConnectedTaxiway.Count == 0)
                     {
                         html += "<span style='color:red;font-weight:bold'>GuidanceLine " + guidanceLine.Designator + " : There are not any connected taxiway.Taxiway  is strictly required for creating of Area 3</span><br />";
                         continue;
                     }
                     var taxiWay = guidanceLine.ConnectedTaxiway[0].Feature.GetFeature() as Taxiway;

                     if (taxiWay.Width == null)
                     {
                         html += "<span style='color:red;font-weight:bold'>GuidanceLine " + guidanceLine.Designator + " : Connected taxiway width is empty.Taxiway width is strictly required for creating of Area 3</span><br />";
                     }
                     else
                     {
                         double width = ConverterToSI.Convert(taxiWay.Width, 0);
                         html += "GuidanceLine " + guidanceLine.Designator + ": ok.Taxiway width is " + width +" m<br />";
                     }
                 }
             }

            foreach (var rwyClass in RwyClassList)
            {
                html += "Rwy : " + rwyClass.Name + "<br />" +
                        "Nominal Length: " + rwyClass.Length + " " + InitOmega.DistanceConverter.Unit + " <br />" +
                        "Calculation Length: " + rwyClass.RwyDirClassList[0].CalculationLength + " " + InitOmega.DistanceConverter.Unit + " <br />";
                foreach (var rwyDir in rwyClass.RwyDirClassList)
                {
                    html += "Rwy direction : " + rwyDir.Name +
                            "<div style='margin-left:10px'>" +
                            " -True direction : " + Math.Round(ARANMath.Modulus(rwyDir.Aziumuth), 1) + " °<br />" +
                            " -TDZ Elevation : " + Common.ConvertHeight(rwyDir.TDZElevation, Aran.Omega.Enums.RoundType.ToNearest) + " " + InitOmega.HeightConverter.Unit + "<br />";

                    if (rwyDir.Validation.DeclaredDistanceNotAvailable)
                        html += "<span style='color:red;font-weight:bold'>" + rwyDir.StartCntlPt.Designator + " Declared distances not available<br /></span>";
                    else
                        html += " -Declared distances <br />" +
                                "   -TORA : " + rwyDir.Tora + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -TODA : " + rwyDir.Toda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -ASDA : " + rwyDir.Asda + " " + InitOmega.DistanceConverter.Unit + "<br />" +
                                "   -LDA : " + rwyDir.LDA + " " + InitOmega.DistanceConverter.Unit + "<br />" +
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

            if (selectedRwy == null) return;

            if (!selectedRwy.Checked)
                return;

            _selectedRwyClass = selectedRwy;

            //var selectedList = RwyClassList.Where(rwyClass => rwyClass.Checked == true).ToList();
            //if (selectedList.Count == 0 && selectedRwy!=null)
            //{
            //    selectedRwy.ChangeChecked(true);
            //    return;
            //}

            //if (RwyClassList.Count > 1 && selectedRwy.Checked)
            //{
            //    foreach (var rwyClass in RwyClassList)
            //    {
            //        if (rwyClass.Checked && !rwyClass.Name.Equals(selectedRwy.Name)
            //                            && rwyClass.CodeNumber != selectedRwy.CodeNumber)
            //        {
            //            rwyClass.ChangeChecked(false);
            //        }
            //    }

            //}

            RwyDirClassList.Clear();
            //foreach (var rwyClass in selectedList)
            //{
            if (selectedRwy.Checked)
            {
                foreach (var rwyDir in selectedRwy.RwyDirClassList)
                {
                    RwyDirClassList.Add(rwyDir);
                }
            }
            //}

            if (RwyDirClassList.Count > 0)
                SelectedRwyDirection = RwyDirClassList[0];


            //end 

            if (RwyDirClassList.Count > 0)
                SelectedRwyDirection = RwyDirClassList[0];

            NotifyPropertyChanged("SelectedRwyDirection");
            NotifyPropertyChanged("RwyDirClassList");
            NotifyPropertyChanged("CodeLetterFIsEnabled");
        }

        private void CalculateInnerLengthOfEdge()
        {
            RunwayConstants lengthOfInnerEdgeConstant = GlobalParams.Constant.RunwayConstants[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            var selectedCategory = SelectedCategory?.Enum ?? CategoryNumber.One;

            LengthOffInnerEdge = lengthOfInnerEdgeConstant.GetValue(SelectedClassification.Enum, selectedCategory, _selectedRwyClass.CodeNumber);
        }

#endregion
    }
}
