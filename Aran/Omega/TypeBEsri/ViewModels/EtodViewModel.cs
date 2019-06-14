using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Omega.Models;
using Aran.Panda.Constants;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Omega.View;
using System.Windows.Interop;
using Aran.AranEnvironment.Symbols;
using System.Diagnostics;
using Aran.Aim.Enums;
using Aran.Queries.Common;
using Aran.Queries;
using Aran.Geometries;

namespace Omega.ViewModels
{
    public class EtodViewModel:ViewModel
    {
        
        #region :>Fields
        private EnumName<RunwayClassificationType> _selClassification;
        private EnumName<CategoryNumber> _selCatNumber;
        private ObservableCollection<EnumName<CategoryNumber>> _categoryList;
        private ElevationDatum _selElevationDatum;
        private RwyClass _selectedRwyClass;
        private string _errorLog;
        //private BackgroundWorker _worker;
        //private ProgressBar progWindow;
        #endregion

        #region :>Constructor
        public EtodViewModel()
        {
           
        }

        #endregion

        #region :>Property

        #region :>Lists
        public ObservableCollection<RwyClass> RwyClassList { get; set; }
        public ObservableCollection<EnumName<RunwayClassificationType>> Classification { get; private set; }
        public ObservableCollection<EnumName<CategoryNumber>> CategoryList { get;private set; }
        public ObservableCollection<ElevationDatum> ElevationDatumList { get; set; }
        public ObservableCollection<DrawingSurface> AvailableSurfaceList { get; set; }
        public ObservableCollection<RwyDirClass> RwyDirClassList { get; set; }

        #endregion

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

        public bool CategoryIsEnabled { get { return (CategoryList== null || CategoryList.Count > 0); } }

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
        
        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get
            {
                return _calculateCommand;
            }
            set
            {
                _calculateCommand = value;
            }
        }

        public ICommand SaveCommand { get; set; }

        private ICommand _reportCommand;
        public ICommand ReportCommand
        {
            get
            {
                return _reportCommand;
            }
            set
            {
                _reportCommand = value;
            }
        }

        public RwyDirClass SelectedRwyDirection
        { get; set; }

        public Cursor CurrCursor { get; set; }

        public string Title { get; set; }

        public bool ChangesGreaterThanFifteen { get; set; }
        public bool ChangesGreaterThanFifteenIsEnabled { get; set; }

        public Action CloseAction { get; set; }

        #endregion

        #region :>Methods

        #region :>Events

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ClearAll();
        }

        private void rwyClass_RwyDirectionChanged(object sender, EventArgs e)
        {
            _selectedRwyClass = (RwyClass)sender;
            CreateElevationDatum();
            NotifyPropertyChanged("ElevationDatumList");
        }

        private void CreateElevationDatum()
        {
            ElevationDatumList = new ObservableCollection<ElevationDatum>();

            ElevationDatum adhpElevationDatum = new ElevationDatum(GlobalParams.Database.AirportHeliport);
            ElevationDatumList.Add(adhpElevationDatum);

            foreach (RwyClass rwyClass in RwyClassList)
            {

                if (rwyClass.Checked)
                {
                    ElevationDatum startElevationDatum = new ElevationDatum(rwyClass.SelectedRwyDirection.StartCntlPt);
                    ElevationDatum endElevationDatum = new ElevationDatum(rwyClass.SelectedRwyDirection.EndCntlPt);
                    ElevationDatumList.Add(startElevationDatum);
                    ElevationDatumList.Add(endElevationDatum);
                }
            }

            SelectedElevationDatum = ElevationDatumList[0];
        }

        private void calculateCommand_onClick(object obj)
        {
            //Change window cursor to wait mode

            CurrCursor = Cursors.Wait;
            NotifyPropertyChanged("CurrCursor");

            ClearAll();

            Annex15Surfaces annerAnnex15Surfaces = new Annex15Surfaces(this._selectedRwyClass, SelectedElevationDatum);
            foreach (var profilePoint in annerAnnex15Surfaces.ProfileLinePoint)
            {
                GlobalParams.UI.DrawPoint(profilePoint,100);
            }


            //Change window cursor to normal mode
            CurrCursor = Cursors.Arrow;
            NotifyPropertyChanged("CurrCursor");
             
        }

        private void reportCommand_onClick(object obj)
        {
            //Change window cursor to wait mode
            CurrCursor = Cursors.Wait;
            NotifyPropertyChanged("CurrCursor");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //End
            int i = 0;
            foreach (var surface in AvailableSurfaceList)
            {
                try
                {
                    i++;
                    IList<ObstacleReport> report;
                    if (surface.SurfaceBase != null)
                        report = surface.SurfaceBase.Report;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            stopWatch.Stop();

            MessageBox.Show(stopWatch.Elapsed.ToString());
            CurrCursor = Cursors.Arrow;
            NotifyPropertyChanged("CurrCursor");

            Report reportWindow; ;
            if (AvailableSurfaceList.Count > 0)
            {
                //reportWindow = new ReportViewModel(this);
                //var helper = new WindowInteropHelper(reportWindow);
                //helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
                //reportWindow.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                //reportWindow.Show();
            }

        }

        private void saveCommand_onClick(object obj)
        {
            List<Feature> obstacleAreaList = new List<Feature>();

            foreach (var surface in AvailableSurfaceList)
            {
                if (surface == null)
                    return;

                ObstacleArea obstacleArea = GlobalParams.Database.OmegaQPI.CreateFeature<ObstacleArea>();
                obstacleArea.Type = CodeObstacleArea.OLS;
                obstacleArea.Reference = new ObstacleAreaOrigin();
             
                if (surface.SurfaceType == SurfaceType.InnerHorizontal || surface.SurfaceType == SurfaceType.OuterHorizontal && surface.SurfaceType == SurfaceType.CONICAL)
                    obstacleArea.Reference.OwnerAirport = GlobalParams.Database.AirportHeliport.GetFeatureRef();
                else
                    obstacleArea.Reference.OwnerRunway = SelectedRwyDirection.RwyDir.GetFeatureRef();
                foreach (var obstacle in surface.SurfaceBase.Report)
                {
                    obstacleArea.Obstacle.Add(obstacle.Obstacle.GetFeatureRefObject());            
                }

                obstacleArea.SurfaceExtent = new Surface();
                switch (surface.SurfaceType)
                {
                    case SurfaceType.Approach:
                        Approach approach = surface.SurfaceBase as Approach;
                        Aran.Geometries.Polygon section1 = new Aran.Geometries.Polygon();
                        section1.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(approach.Section1.Geo);
                        obstacleArea.SurfaceExtent.Geo.Add (section1);
                        if (approach.Section2 != null) 
                        {
                            Aran.Geometries.Polygon section2 = new Aran.Geometries.Polygon();
                            section2.ExteriorRing =GlobalParams.SpatialRefOperation.ToGeo(approach.Section2.Geo);
                            Aran.Geometries.Polygon section3 = new Aran.Geometries.Polygon();
                            section3.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(approach.Section3.Geo);
                            obstacleArea.SurfaceExtent.Geo.Add(section2);
                            obstacleArea.SurfaceExtent.Geo.Add(section3);
                        }
                        break;
                    case SurfaceType.Transitional:
                        Transitional transitional = surface.SurfaceBase as Transitional;
                        foreach (var plane in transitional.Planes)
                        {
                            Aran.Geometries.Polygon poly = new Aran.Geometries.Polygon();
                            poly.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(plane.Geo);
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                        break;
                    case SurfaceType.InnerTransitional:
                        InnerTransitional innerTransitional = surface.SurfaceBase as InnerTransitional;
                        foreach (var plane in innerTransitional.Planes)
                        {
                            Aran.Geometries.Polygon poly = new Aran.Geometries.Polygon();
                            poly.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(plane.Geo);
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                        break;
                    case SurfaceType.Strip:
                        Strip strip = surface.SurfaceBase as Strip;
                        foreach (var plane in strip.Planes)
                        {
                            Aran.Geometries.Polygon poly = new Aran.Geometries.Polygon();
                            poly.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo(plane.Geo);
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }
                        break;
                    default:
                        MultiPolygon mltGeo = GlobalParams.SpatialRefOperation.ToGeo(surface.SurfaceBase.Geo);
                        foreach (Polygon poly in mltGeo)
                        {
                            obstacleArea.SurfaceExtent.Geo.Add(poly);
                        }

                        break;
                }
                obstacleAreaList.Add(obstacleArea);
                GlobalParams.Database.OmegaQPI.SetFeature(obstacleArea);
            }
            
            Aran.Aim.FeatureInfo.ROFeatureViewer featViewer = new Aran.Aim.FeatureInfo.ROFeatureViewer();
            System.Windows.Forms.IWin32Window window = new Win32Windows(GlobalParams.HWND);
            featViewer.SetOwner(window);
            featViewer.ShowFeaturesForm(obstacleAreaList,true,true);
            //GlobalParams.Database.OmegaQPI.Commit(new Aran.Aim.FeatureType[] { Aran.Aim.FeatureType.ObstacleArea }); 
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
                CalculateCommand = new RelayCommand(new Action<object>(calculateCommand_onClick));
                ReportCommand = new RelayCommand(new Action<object>(reportCommand_onClick));
                SaveCommand = new RelayCommand(new Action<object>(saveCommand_onClick));
                RwyDirClassList = new ObservableCollection<RwyDirClass>();
                Title = "Omega Aerodrome: " + GlobalParams.Database.AirportHeliport.Name;

                CurrCursor = Cursors.Arrow;

                RwyClassList = new ObservableCollection<RwyClass>();
                List<Runway> runwayList = GlobalParams.Database.Runways;

                for (int i = 0; i < runwayList.Count; i++)
                {
                    RwyClass rwyClass = new RwyClass(runwayList[i]);
                    rwyClass.RwyCheckedIsChanged += new EventHandler(rwyClass_RwyCheckedIsChanged);
                    if (i == 0)
                    {
                        rwyClass.ChangeChecked(true);   
                        _selectedRwyClass = rwyClass;
                    }
                    RwyClassList.Add(rwyClass);
                }
                rwyClass_RwyCheckedIsChanged(_selectedRwyClass, new EventArgs());
                
                //Init Category constant params
                _categoryList = new ObservableCollection<EnumName<CategoryNumber>>();
                _categoryList.Add(new EnumName<CategoryNumber> { Name = "I", Enum = CategoryNumber.One });
                _categoryList.Add(new EnumName<CategoryNumber> { Name = "II,III", Enum = CategoryNumber.Two });
                CategoryList = new ObservableCollection<EnumName<CategoryNumber>>();
             
                //End init Category


                //Init Classifaction constant params
                Classification = new ObservableCollection<EnumName<RunwayClassificationType>>();
                Classification.Add(
                    new EnumName<RunwayClassificationType> { Name = "Non-instrument", Enum = RunwayClassificationType.NonInstrument });
                Classification.Add(
                    new EnumName<RunwayClassificationType> { Name = "Non-precision approach", Enum = RunwayClassificationType.NonPrecisionApproach });
                Classification.Add(
                    new EnumName<RunwayClassificationType> { Name = "Precision approach", Enum = RunwayClassificationType.PrecisionApproach });
                _selClassification = Classification[0];
                CreateElevationDatum();
            }
            catch (Exception)
            {
                throw new Exception("Error load main form!");
                
            }

        }

        private void rwyClass_RwyCheckedIsChanged(object sender, EventArgs e)
        {
            RwyClass selectedRwy = sender as RwyClass;

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

            if (RwyDirClassList.Count>0)
                SelectedRwyDirection = RwyDirClassList[0];

            

            //If runway code number 4 then code letter f must be enabled
            CodeLetterFIsEnabled = false;
            foreach (var rwyClass in RwyClassList)
            {
                if (rwyClass.Checked && rwyClass.CodeNumber==4)
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

        #endregion
    }
}
