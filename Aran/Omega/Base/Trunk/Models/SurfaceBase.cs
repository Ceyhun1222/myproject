using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Constants;
using Aran.AranEnvironment.Symbols;
using ESRI.ArcGIS.Geometry;
using Aran.Geometries;
using Aran.Omega.Strategy;
using Aran.Omega.Strategy.UI;
using Aran.Omega.SettingsUI;
using Aran.Omega.Strategy.ObstacleCalculation;

namespace Aran.Omega.Models
{
    public abstract class SurfaceBase:ViewModels.ViewModel
    {
        protected IList<Info> _propertyList = new List<Info>();
        private IObstacleCalculation _obstacleCalculation;
        private IPlaneDrawing _planeDrawing;
        private SurfaceModel _surfaceModel;

        protected MtObservableCollection<ObstacleReport> _report;

        public SurfaceBase BuildDrawing(IPlaneDrawing drawing)
        {
            _planeDrawing = drawing;
            _planeDrawing.SetSurface(this);
            return this;
        }

        public SurfaceBase BuildObstacleCalculation(IObstacleCalculation obstacleCalculation)
        {
            _obstacleCalculation = obstacleCalculation;
            return this;
        }

        public SurfaceBase BuildStyles()
        {
            _surfaceModel = CommonFunctions.GetSurfaceModel(this.SurfaceType);

            if (_surfaceModel == null)
                throw new ArgumentNullException("There is not any style for " + SurfaceType);

            DefaultSymbol = _surfaceModel.Symbol;
            SelectedSymbol = _surfaceModel.SelectedSymbol;

            return this;
        }

        private List<ObstacleReport> _filteredReport;
        protected object _lockObject = new Object();

        public Aran.Geometries.MultiPolygon GeoPrj { get; set; }
        public SurfaceType SurfaceType { get; set; }
        public EtodSurfaceType EtodSurfaceType { get; set; }

        public int GeomDefautlHandle { get; set; }
        public int GeomSelectedHandle { get; set; }
        public FillSymbol DefaultSymbol { get; set; }
        public FillSymbol SelectedSymbol { get; set; }
        public Aran.Geometries.Point StartPoint { get; set; }
        public double Direction { get; set; }

        public virtual void CreateReport()
        {
            try
            {
                _report = _obstacleCalculation.CalculateReport(this);
            }
            catch (Exception e)
            {
                GlobalParams.Logger.Error(e,this.SurfaceType.ToString());
            }
        }
        public abstract IList<Info> PropertyList { get; set; }

        public Aim.Features.ObstacleArea ObsArea { get; set; }

        public PlaneParam PlaneParam { get; set; }

        public virtual List<Plane> Planes { get; set; }

        public abstract PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt); 

        public List<ObstacleReport> FilteredReport
        {
            get
            {
                if (_report == null)
                    CreateReport();

                _filteredReport = _report?.ToList();
                if (_report != null)
                {
                    if (!string.IsNullOrEmpty(this.SearchName))
                    {
                        _filteredReport = _filteredReport.Where(obsReport => obsReport.Name.ToLower().
                            StartsWith(this.SearchName.ToLower())).ToList<ObstacleReport>();
                    }
                    if (ViewModels.EtodReportViewModel.ShowOnlyPenetrated)
                    {
                        _filteredReport = _filteredReport.Where(obsReport => obsReport.Penetrate > 0).ToList<ObstacleReport>();
                    }
                    if (ViewModels.ReportViewModel.ShowOnlyPenetrated)
                    {
                        _filteredReport = _filteredReport.Where(obsReport => obsReport.Penetrate > 0).ToList<ObstacleReport>();
                    }
                    return _filteredReport;
                }

                return _filteredReport;
            }
        }

        public IList<ObstacleReport> GetReport { get { return _report; } }

        private string _searchName;
        public string SearchName 
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                
                NotifyPropertyChanged("FilteredReport");
                NotifyPropertyChanged("SearchName");
            }
        }

        public void IsPenetratedAction()
        {
            NotifyPropertyChanged("FilteredReport");
        }
        
        public virtual void Draw(bool isSelected)
        {
            _planeDrawing?.Draw(isSelected);
        }

        public virtual void ClearSelected() 
        {
            _planeDrawing?.ClearSelected();
        }

        public virtual void ClearAll() 
        {
            _planeDrawing?.ClearAll();
        }

        public virtual void ClearDefault()
        {
            _planeDrawing?.ClearDefault();
        }

        public IMultiPatch GetGeomAsMultiPatch()
        {
            return HelperClass.MultiPatchHelper.GeometryToMultipatch(GeoPrj);
        }

    }
}
