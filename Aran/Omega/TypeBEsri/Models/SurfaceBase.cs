using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Panda.Constants;
using Aran.AranEnvironment.Symbols;

namespace Aran.Omega.TypeBEsri.Models
{
    public abstract class SurfaceBase:ViewModels.ViewModel
    {
        public Aran.Geometries.MultiPolygon Geo { get; set; }
        public SurfaceType SurfaceType { get; set; }

        public virtual Aran.Geometries.MultiPolygon GetCuttingGeometry() 
        {
            return Geo;
        }

        public int GeomDefautlHandle { get; set; }
        public int GeomSelectedHandle { get; set; }
        public FillSymbol DefaultSymbol { get; set; }
        public FillSymbol SelectedSymbol { get; set; }
        public FillSymbol CuttingGeoSymbol { get; set; }
        public Aran.Geometries.Point StartPoint { get; set; }
        public double Direction { get; set; }

        public abstract void CreateReport();
        public abstract IList<Info> PropertyList { get; set; }

        public abstract PointPenetrateModel GetManualReport(Aran.Geometries.Point obstaclePt); 

        protected IList<ObstacleReport> _report;
        public  IList<ObstacleReport> Report
        {
            get
            {
                if (_report == null)
                {
                    CreateReport();
                }
                if (_report != null && !string.IsNullOrEmpty(this.SearchName))
                    return _report.Where(obsReport => obsReport.Name.ToLower().
                        StartsWith(this.SearchName.ToLower())).ToList<ObstacleReport>();

                return _report;
            }
            set
            {
                _report = value;
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
                
                NotifyPropertyChanged("Report");
                NotifyPropertyChanged("SearchName");
            }
        }     

        public virtual void Draw(bool isSelected)
        {
            FillSymbol symbol = DefaultSymbol;
            if (isSelected)
            {
                ClearSelected();
                if (this.Geo != null)
                    GeomSelectedHandle = GlobalParams.UI.DrawMultiPolygon(Geo, SelectedSymbol,true,false);
            }
            else
            {
                ClearDefault();
                if (this.Geo != null)
                    GeomDefautlHandle = GlobalParams.UI.DrawMultiPolygon(Geo, symbol,true,false);
            }
        }

        public virtual void ClearSelected() 
        {
            GlobalParams.UI.SafeDeleteGraphic(GeomSelectedHandle);
        }

        public virtual void ClearAll() 
        {
            ClearDefault();
            ClearSelected();
        }

        public virtual void ClearDefault()
        {
            GlobalParams.UI.SafeDeleteGraphic(GeomDefautlHandle);
        
        }

    }
}
