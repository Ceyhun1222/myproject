using Aran.Omega.Export;
using Aran.Omega.Models;
using Aran.Omega.View;
using Aran.PANDA.Constants;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Omega.Enums;
using Cursor = System.Windows.Input.Cursor;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.MessageBox;

namespace Aran.Omega.ViewModels
{
    // ReSharper disable once InconsistentNaming
    public class OLSViewModel : ViewModel
    {
        #region :>Fields
        private EnumName<RunwayClassificationType> _selClassification;
        private EnumName<CategoryNumber> _selCatNumber;
        private ObservableCollection<EnumName<CategoryNumber>> _categoryList;
        private ElevationDatum _selElevationDatum;
        private RwyClass _selectedRwyClass;
        private Annex14Surfaces _annex14Surfaces;
        private bool _reportIsCalculated;
        private bool _saveIsEnabled;
        private RwyDirClass _selectedRwyDirection;
        private ManualReport _reportWindow;

        #endregion

        #region :>Constructor
        public OLSViewModel()
        {
        }

        #endregion

        #region :>Property

        #region :>Lists
        public ObservableCollection<RwyClass> RwyClassList { get; set; }
        public ObservableCollection<EnumName<RunwayClassificationType>> Classification { get; private set; }
        public ObservableCollection<EnumName<CategoryNumber>> CategoryList { get; private set; }
        public ObservableCollection<ElevationDatum> ElevationDatumList { get; set; }
        public ObservableCollection<DrawingSurface> AvailableSurfaceList { get; set; }
        public ObservableCollection<RwyDirClass> RwyDirClassList { get; set; }

        #endregion

        public ICommand DrawCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    int i = 0;
                    try
                    {
                        IsLoading = true;
                        GridIsEnabled = false;
                        // CurrCursor = new Cursor(@"D:\AirNav\bin\Debug\ajax-loader1.ani");

                        ClearAll();
                        ReloadSurfaceList();

                        if (AvailableSurfaceList.Count == 0)
                        {
                            MessageBox.Show(
                                "There are not any available Surface!Please change category or classification");
                            return;
                        }

                        var catNumber = CategoryNumber.One;
                        if (SelectedCategory != null)
                            catNumber = SelectedCategory.Enum;

                        var selectedRwyClassList = RwyClassList.Where(rwy => rwy.Checked).ToList<RwyClass>();

                        _annex14Surfaces = new Annex14Surfaces
                        (selectedRwyClassList, SelectedRwyDirection, SelectedElevationDatum, _selClassification.Enum,
                            catNumber, _selectedRwyClass.CodeNumber, TakeOffSlopeIsVisible, LengthOffInnerEdge);

                        var codeLetter = CodeLetter.A;
                        if (CodeLetterFIsEnabled && CodeLetterFIsChecked)
                            codeLetter = CodeLetter.F;


                        foreach (var drawingSurface in AvailableSurfaceList)
                        {
                            i++;
                            switch (drawingSurface.SurfaceType)
                            {
                                case SurfaceType.InnerHorizontal:
                                    {
                                        var innerHorizontal = _annex14Surfaces.CreateInnerHorizontal();
                                        drawingSurface.SurfaceBase = innerHorizontal;
                                    }
                                    break;
                                case SurfaceType.CONICAL:
                                    {
                                        var conical = _annex14Surfaces.CreateConicalPlane();
                                        drawingSurface.SurfaceBase = conical;
                                    }
                                    break;
                                case SurfaceType.OuterHorizontal:
                                    {
                                        var outer = _annex14Surfaces.CreateOuterHorizontal();
                                        drawingSurface.SurfaceBase = outer;
                                    }
                                    break;
                                case SurfaceType.Strip:
                                    {
                                        var strip = _annex14Surfaces.CreateStrip();
                                        drawingSurface.SurfaceBase = strip;
                                    }
                                    break;
                                case SurfaceType.Area2A:
                                    {
                                        var area2a = _annex14Surfaces.CreateArea2A();
                                        drawingSurface.SurfaceBase = area2a;
                                    }
                                    break;
                                case SurfaceType.Approach:
                                    {
                                        var approach = _annex14Surfaces.CreateApproach();
                                        drawingSurface.SurfaceBase = approach;
                                    }
                                    break;
                                case SurfaceType.InnerApproach:
                                    {
                                        var innerApproach = _annex14Surfaces.CreateInnerApproach(codeLetter);
                                        drawingSurface.SurfaceBase = innerApproach;
                                    }
                                    break;
                                case SurfaceType.Transitional:
                                    {
                                        var transitional = _annex14Surfaces.CreateTransitionalSurface();
                                        drawingSurface.SurfaceBase = transitional;
                                    }
                                    break;
                                case SurfaceType.BalkedLanding:
                                    {
                                        var balkedLanding = _annex14Surfaces.CreateBalkedLandingSurface(codeLetter);
                                        drawingSurface.SurfaceBase = balkedLanding;
                                    }
                                    break;
                                case SurfaceType.TakeOffClimb:
                                    {
                                        double takeOffSlope;
                                        //if (TakeOffSlopeIsVisible == Visibility.Visible)
                                        //{
                                        //    if (TakeOffOnePercentIsChecked)
                                        //        takeOffSlope = 1;
                                        //    else
                                        //        takeOffSlope = 1.2;
                                        //}
                                        //else 
                                        //{
                                        var tmpConstant =
                                            GlobalParams.Constant.RunwayConstants[SurfaceType.TakeOffClimb,
                                                DimensionType.Slope];
                                        takeOffSlope = tmpConstant.GetValue(SelectedClassification.Enum, 0,
                                            _selectedRwyClass.CodeNumber);
                                        //}
                                        var takeOff =
                                            _annex14Surfaces.CreateTakeOffSurface(ChangesGreaterThanFifteen, takeOffSlope);
                                        drawingSurface.SurfaceBase = takeOff;
                                    }
                                    break;
                                case SurfaceType.InnerTransitional:
                                    {
                                        var balkedLanding = _annex14Surfaces.CreateInnerTransitional(codeLetter);
                                        drawingSurface.SurfaceBase = balkedLanding;
                                    }
                                    break;
                                case SurfaceType.TakeOffFlihtPathArea:
                                    {
                                        double takeOffSlope;
                                        if (TakeOffSlopeIsVisible)
                                        {
                                            if (TakeOffOnePercentIsChecked)
                                                takeOffSlope = 1;
                                            else
                                                takeOffSlope = 1.2;
                                        }
                                        else
                                        {
                                            var tmpConstant =
                                                GlobalParams.Constant.RunwayConstants[SurfaceType.TakeOffClimb,
                                                    DimensionType.Slope];
                                            takeOffSlope = tmpConstant.GetValue(SelectedClassification.Enum, 0,
                                                _selectedRwyClass.CodeNumber);
                                        }
                                        var takeOff = _annex14Surfaces.CreateTakeOffPlane(takeOffSlope);
                                        drawingSurface.SurfaceBase = takeOff;
                                        break;
                                    }
                            }
                        }

                        SaveIsEnabled = false;
                        NotifyPropertyChanged(nameof(ReportIsEnabled));
                        NotifyPropertyChanged(nameof(ManualReportIsEnabled));
                        NotifyPropertyChanged(nameof(ExportAsEtodIsVisible));
                    }
                    catch (Exception e)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(e, e.Message);
                        MessageBox.Show(e.ToString() + "  " + i.ToString());
                    }

                    finally
                    {
                        IsLoading = false;
                        GridIsEnabled = true;
                    }
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        SaveDbWrapper dbWrapper = new SaveDbWrapper(SelectedRwyDirection.RwyDir, SelectedClassification, SelectedElevationDatum, SelectedCategory);

                        OmegaSaveView saveWindow = new OmegaSaveView(AvailableSurfaceList.ToList(),dbWrapper);
                        var helper = new WindowInteropHelper(saveWindow);
                        ElementHost.EnableModelessKeyboardInterop(saveWindow);
                        helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                        saveWindow.Show();
                    }
                    catch (Exception e)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(e, e.Message);
                        MessageBox.Show(e.Message);
                    }
                });
            }
        }

        public ICommand ReportCommand
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    try
                    {

                        IsLoading = true;
                        GridIsEnabled = false;

                        if (!_reportIsCalculated)
                            await CommonFunctions.GetandConvertVsToLocalParam();

                        await Task.Factory.StartNew(() =>
                        {
                            foreach (var surface in AvailableSurfaceList)
                            {
                                var tmpSurface = (DrawingSurface)surface;
                                if (tmpSurface?.SurfaceBase != null)
                                {
                                    var obstacleReports = tmpSurface.SurfaceBase.FilteredReport;
                                }
                            }
                        });

                        if (AvailableSurfaceList.Count > 0)
                        {
                            var reportWindow = new Report(this);

                            var helper = new WindowInteropHelper(reportWindow);
                            ElementHost.EnableModelessKeyboardInterop(reportWindow);
                            helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                            reportWindow.ShowInTaskbar = false; // hide from taskbar and alt-tab list
#if !Riga
                    reportWindow.Show();
#endif
                            _reportIsCalculated = true;
                            SaveIsEnabled = true;
                        }
                    }
                    catch (Exception e)
                    {
                        GlobalParams.Logger.Error(e);
                        MessageBox.Show(e.Message, "Omega", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        CurrCursor = Cursors.Arrow;
                        NotifyPropertyChanged(nameof(CurrCursor));
                        NotifyPropertyChanged(nameof(ExportAsEtodIsVisible));
                        IsLoading = false;
                        GridIsEnabled = true;
                    }
                });
            }
        }

        public ICommand ManualReport
        {
            get
            {
                return new RelayCommand(obj =>
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
                });
            }
        }

        public RelayCommand SaveSurface
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        var surface = obj as DrawingSurface;

                        if (surface == null)
                        {
                            GlobalParams.AranEnvironment.GetLogger("Omega").Info("Command has not recocnized!");
                            MessageBox.Show("Calling parametrs is not correct!");
                            return;
                        }

                        CurrCursor = Cursors.Wait;
                        SaveDbWrapper dbWrapper = new SaveDbWrapper(SelectedRwyDirection.RwyDir,
                            SelectedClassification, SelectedElevationDatum, SelectedCategory);


                        if (!dbWrapper.Save(new List<DrawingSurface> { surface }))
                        {
                            MessageBox.Show("Canceled!");
                            return;
                        }
                        MessageBox.Show("Data has saved to Database successfully!");
                    }
                    catch (Exception e)
                    {
                        GlobalParams.AranEnvironment.GetLogger("Omega").Error(e.Message);
                        MessageBox.Show(e.Message);
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

                        var helper = new ExportToGdb(AvailableSurfaceList.ToList());
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
                        var folderPath = ExportFolderPath.GetFolderPath(SelectedRwyDirection, SelectedClassification, SelectedCategory);

                        CurrCursor = Cursors.Arrow;
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            MessageBox.Show("Canceled!");
                            return;
                        }

                        ExportToGdb helper = new ExportToGdb(olsSurface);
                        helper.ExportAll(folderPath,ExportType.Geodatabase ,olsSurface.SurfaceBase.SurfaceType.ToString() + "_");

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

        public ICommand ExportAsEtodCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ExportToGdb();
                });
            }
        }

        private void ExportToGdb()
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

                var surfaceWithoutArea2A = AvailableSurfaceList.Where(surface => surface.SurfaceType !=SurfaceType.Strip);
                EtodExportToGdbHelper gdbHelper = new EtodExportToGdbHelper(null,GlobalParams.Database.AirportHeliport,_selectedRwyDirection);
                gdbHelper.ExportAnnex14(surfaceWithoutArea2A, folderPath, $"Annex14_Obstacles.mdb");

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

        public Annex14Surfaces Annex14Surfaces => _annex14Surfaces;

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

                NotifyPropertyChanged(nameof(CategoryIsEnabled));
                NotifyPropertyChanged(nameof(SelectedCategory));
            }
        }

        public bool CategoryIsEnabled => (CategoryList == null || CategoryList.Count > 0);

        public ElevationDatum SelectedElevationDatum
        {
            get => _selElevationDatum;
            set
            {
                _selElevationDatum = value;
                NotifyPropertyChanged(nameof(SelectedElevationDatum));
            }
        }

        public bool CodeLetterFIsEnabled { get; set; }

        public bool CodeLetterFIsChecked { get; set; }

        private bool _takeOffOnePercentIsChecked;

        public bool TakeOffOnePercentIsChecked
        {
            get { return _takeOffOnePercentIsChecked; }
            set
            {
                _takeOffOnePercentIsChecked = value;
                NotifyPropertyChanged("TakeOffOnePercentIsChecked");
            }
        }

        public bool TakeOffOneDotTwoPercentIsChecked { get; set; }

        public bool TakeOffSlopeIsVisible
        {
            get
            {
#if (Annex4)
                return true;
#else
                return false;
#endif
            }

        }

        public int Height => TakeOffSlopeIsVisible ? 400 : 370;

        public RwyDirClass SelectedRwyDirection
        {
            get { return _selectedRwyDirection; }
            set
            {
                _selectedRwyDirection = value;
                AssignClassification();
                CreateElevationDatum();
            }
        }

        private void AssignClassification()
        {
            if (_selectedRwyDirection?.RwyDir.PrecisionApproachGuidance == null)
            {
                SelectedClassification = Classification.FirstOrDefault(c => c.Enum == RunwayClassificationType.PrecisionApproach);
                SelectedCategory = _categoryList.FirstOrDefault(c => c.Enum == CategoryNumber.One);
                return;
            }

            switch (_selectedRwyDirection.RwyDir.PrecisionApproachGuidance.Value)
            {
                case CodeApproachGuidance.NON_PRECISION:
                    SelectedClassification =
                        Classification.FirstOrDefault(c => c.Enum == RunwayClassificationType.NonPrecisionApproach); 
                    break;
                case CodeApproachGuidance.ILS_PRECISION_CAT_I:
                    SelectedClassification = Classification.FirstOrDefault(c => c.Enum == RunwayClassificationType.PrecisionApproach);
                    SelectedCategory = _categoryList.FirstOrDefault(c => c.Enum == CategoryNumber.One);
                    break;
                case CodeApproachGuidance.ILS_PRECISION_CAT_II:
                case CodeApproachGuidance.ILS_PRECISION_CAT_IIIB:
                case CodeApproachGuidance.ILS_PRECISION_CAT_IIIC:
                case CodeApproachGuidance.ILS_PRECISION_CAT_IIID:
                case CodeApproachGuidance.MLS_PRECISION:
                    SelectedClassification = Classification.FirstOrDefault(c => c.Enum == RunwayClassificationType.PrecisionApproach);
                    SelectedCategory = _categoryList.FirstOrDefault(c => c.Enum == CategoryNumber.Two);
                    break;
                default:
                    SelectedClassification = Classification.FirstOrDefault(c => c.Enum == RunwayClassificationType.PrecisionApproach);
                    SelectedCategory = _categoryList.FirstOrDefault(c => c.Enum == CategoryNumber.One);
                    break;
            }
        }

        public Cursor CurrCursor { get; set; }

        public string Title { get; set; }

        public bool ChangesGreaterThanFifteen { get; set; }
        public bool ChangesGreaterThanFifteenIsEnabled { get; set; }

        public Action CloseAction { get; set; }

        public bool ReportIsEnabled => AvailableSurfaceList.Count > 0;

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

        private double _lenghtOffInnerEdge;
        private bool _isLoading;

        public double LengthOffInnerEdge
        {
            get { return _lenghtOffInnerEdge; }
            set
            {
                _lenghtOffInnerEdge = value;
                NotifyPropertyChanged(nameof(LengthOffInnerEdge));
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged(nameof(IsLoading));
            }
        }

        private bool _gridIsEnabled;
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
            ClearAll();
            AvailableSurfaceList = null;
            ElevationDatumList = null;
            GlobalParams.AdhpObstacleList = null;
            GlobalParams.Database = null;
            GlobalParams.OlsViewModel = null;
            GlobalParams.OLSWindow = null;
        }

        private void CreateElevationDatum()
        {
            ElevationDatumList = new ObservableCollection<ElevationDatum>();

            var adhpElevationDatum = new ElevationDatum(GlobalParams.Database.AirportHeliport, "ARP");
            ElevationDatumList.Add(adhpElevationDatum);

            if (_selectedRwyDirection == null)
                return;

            var startElevationDatum = new ElevationDatum(_selectedRwyDirection.StartCntlPt, "THR" + _selectedRwyDirection.Name);
            var endElevationDatum = new ElevationDatum(_selectedRwyDirection.EndCntlPt, "END" + _selectedRwyDirection.Name);
            var tdzElevationDatum = new ElevationDatum(_selectedRwyDirection.TDZElevation,"TDZ");
            ElevationDatumList.Add(startElevationDatum);
            ElevationDatumList.Add(endElevationDatum);
            ElevationDatumList.Add(tdzElevationDatum);
            ElevationDatumList.Add(new ElevationDatum(GlobalParams.Database.AirportHeliport.FieldElevation.Value, "AEL"));
            NotifyPropertyChanged("ElevationDatumList");
            SelectedElevationDatum = ElevationDatumList[0];
        }

        #endregion
        private void ClearAll()
        {
            foreach (DrawingSurface surface in AvailableSurfaceList)
                surface.SurfaceBase?.ClearAll();
        }

        public void Init()
        {
            try
            {
                AvailableSurfaceList = new ObservableCollection<DrawingSurface>();
                CommonParamModel = new CommonParam(InitOmega.ObstacleDistanceFromArp);
                RwyDirClassList = new ObservableCollection<RwyDirClass>();
                Title = "Omega - Airport/Heliport: " + GlobalParams.Database.AirportHeliport.Designator;
                TakeOffOneDotTwoPercentIsChecked = true;

                CurrCursor = Cursors.Arrow;
                _reportIsCalculated = false;
                SaveIsEnabled = false;

                //Init Category constant params
                _categoryList = new ObservableCollection<EnumName<CategoryNumber>>
                {
                    new EnumName<CategoryNumber> {Name = "I", Enum = CategoryNumber.One},
                    new EnumName<CategoryNumber> {Name = "II,III", Enum = CategoryNumber.Two}
                };
                CategoryList = new ObservableCollection<EnumName<CategoryNumber>>();

                CreateClassifications();
                CreateElevationDatum();
                CreateRwyList();

                if (GlobalParams.Settings.OLSQuery.ValidationReportIsCheked)
                {
                    var result = MessageBox.Show("Do you want to save validation report!", "Omega Validation report",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        ExportValidationToHtml();
                }


                GridIsEnabled = true; 
                IsLoading = false;
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("OMEGA").Error(e);
                throw new Exception("Error loading main form!" + Environment.NewLine + e.Message);
            }
        }

        private void CalculateInnerLengthOfEdge()
        {
            RunwayConstants lengthOfInnerEdgeConstant = GlobalParams.Constant.RunwayConstants[SurfaceType.Approach, DimensionType.LengthOfInnerEdge];
            var selectedCategory = SelectedCategory?.Enum ?? CategoryNumber.One;

            LengthOffInnerEdge = lengthOfInnerEdgeConstant.GetValue(SelectedClassification.Enum, selectedCategory, _selectedRwyClass.CodeNumber);
        }

        private void CreateRwyList()
        {
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
        }

        private void CreateClassifications()
        {
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
        }

        private void ExportValidationToHtml()
        {
            var validationExporter = new ValidationExporter();
            if (validationExporter.Export(RwyClassList.ToList()))
                MessageBox.Show("The document was saved successfully!", "Omega", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void rwyClass_RwyCheckedIsChanged(object sender, EventArgs e)
        {
            var selectedRwy = sender as RwyClass;

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

            if (RwyDirClassList.Count > 0)
                SelectedRwyDirection = RwyDirClassList[0];

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

        private void ReloadSurfaceList()
        {

            List<RunwayConstants> rwyConstantList = GlobalParams.Constant.RunwayConstants.List;
#if Annex4
            StripConstantRiga(rwyConstantList);
#endif
            StripConstant(rwyConstantList);
            //rwyConstantList.Add(StripConstant());

            if (rwyConstantList == null || rwyConstantList.Count == 0)
                throw new Exception("Runway Constant files is required!");

            rwyConstantList = rwyConstantList.OrderBy(rwyConst => rwyConst.Order).ToList<RunwayConstants>();

            List<SurfaceType> tmpSurfaces = new List<SurfaceType>();

            CategoryNumber catNumber = CategoryNumber.One;
            if (SelectedCategory != null)
                catNumber = SelectedCategory.Enum;

            AvailableSurfaceList.Clear();
            foreach (RunwayConstants rwyConstant in rwyConstantList)
            {
                if (tmpSurfaces.Contains(rwyConstant.Surface))
                {
                    continue;
                }

                var val = rwyConstant.GetValue(SelectedClassification.Enum, catNumber,
                    _selectedRwyClass.CodeNumber);
                if (val > 0.1 || rwyConstant.Surface == SurfaceType.Area2A ||
                    rwyConstant.Surface == SurfaceType.Strip || rwyConstant.Surface == SurfaceType.TakeOffClimb)
                {
                    tmpSurfaces.Add(rwyConstant.Surface);
                    AvailableSurfaceList.Add(new DrawingSurface(rwyConstant, _selectedRwyDirection));

#if Annex4
                    if (rwyConstant.Surface == SurfaceType.TakeOffClimb)
                    {
                        var takeOffArea = rwyConstant.DeepClone();
                        takeOffArea.SurfaceName = "TAKE OFF FLIGHT PATH AREA";
                        takeOffArea.Surface = SurfaceType.TakeOffFlihtPathArea;
                        ;
                        AvailableSurfaceList.Add(new DrawingSurface(takeOffArea, _selectedRwyDirection));
                    }
#endif
                }
            }
        }

        private void StripConstantRiga(List<RunwayConstants> constantList)
        {
            var stripConstant = new RunwayConstants
            {
                Order = 5,
                Surface = SurfaceType.Area2A,
                SurfaceName = "AREA 2A"
            };
            constantList.Add(stripConstant);
            //return stripConstant;
        }

        private void StripConstant(List<RunwayConstants> constantList)
        {
            var stripConstant = new RunwayConstants
            {
                Order = 5,
                Surface = SurfaceType.Strip,
                SurfaceName = "STRIP"
            };
            constantList.Add(stripConstant);
        }

        public List<IFeatureClass> ObstacleFeatureClassList { get; set; }

        #endregion
    }
}
