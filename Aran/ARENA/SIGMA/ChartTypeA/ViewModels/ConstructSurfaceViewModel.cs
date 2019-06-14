using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ChartTypeA.Models;
using PDM;
using System.Windows.Data;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using DoddleReport;

namespace ChartTypeA.ViewModels
{
    public enum TakeoffSide { 
        Left,
        Right
    }

    public class ConstructSurfaceViewModel:ViewModel
    {
        private double _slope;
        private string _widthPointOfOrigin;
        private string _divergence;
        private string _width;
        private string _length;
        private ObstacleReport _selectedPenetratedObstacle;
        private List<int> _takeOffHandles;
        private List<Models.RwyDirWrapper> _rwyDirlist; 

        public ConstructSurfaceViewModel(List<Models.RwyDirWrapper> rwyDirList)
        {
            Header = " ICAO Chart Type A (Construct Take Off flight Path Area)";
            _rwyDirlist = rwyDirList;
            RwyDirlist = new List<RwyDirWrapper>(rwyDirList);
            SelectedRwyDir = RwyDirlist[0];

            PenetratedObstacleList = new ObservableCollection<ObstacleReport>();
            ShadowedObstacleList = new ObservableCollection<ObstacleReport>();
            _takeOffHandles = new List<int>();
            TakeOffClimbs = new List<TakeOffClimb>();

            Offset = 15;
            Slope = 1.2;// takeOffArea.Slope;
           
            var takeOffArray= Enum.GetValues(typeof(TakeoffSide));
            TakeOffSideList = new List<TakeoffSide>();
            foreach (var item in takeOffArray)
                TakeOffSideList.Add((TakeoffSide)item);
           
            if (TakeOffSideList.Count>0)
                SelectedTakeOffSide = TakeOffSideList[0];
            //Calculate();
        }

        public List<TakeOffClimb> TakeOffClimbs { get; set; }
        public ObservableCollection<ObstacleReport> PenetratedObstacleList { get; set; }
        public ObservableCollection<ObstacleReport> ShadowedObstacleList { get; set; }
        public List<Models.RwyDirWrapper> RwyDirlist { get; set; }
        public List<TakeoffSide> TakeOffSideList { get; set; }

        public ObstacleReport SelectedObstacle
        {
            get { return _selectedPenetratedObstacle; }
            set
            {
                if (_selectedPenetratedObstacle != null)
                    _selectedPenetratedObstacle.Clear();

                _selectedPenetratedObstacle = value;
                if (_selectedPenetratedObstacle != null)
                    _selectedPenetratedObstacle.Draw();
                
                NotifyPropertyChanged("SelectedPenetratedObstacle");
            }
        }

        //public ObstacleReport SelectedShadowedObstacle
        //{
        //    get { return _selectedShadowedObstacle; }
        //    set
        //    {
        //        _selectedShadowedObstacle = value;
        //        if (_selectedShadowedObstacle != null)
        //        {
        //            _selectedShadowedObstacle.Draw();
        //        }
        //        NotifyPropertyChanged("SelectedShadowedObstacle");
        //    }
        //}
         
        public double Slope
        {
            get { return _slope; }
            set
            {
                if (_slope != value)
                {
                    _slope = value;
                    CalculateWithoutTurn();
                }
                NotifyPropertyChanged("Slope");
            }
        }

        public string WidthPointOfOrigin
        {
            get { return _widthPointOfOrigin + InitChartTypeA.DistanceConverter.Unit;}
            set
            {
                _widthPointOfOrigin = value;
                NotifyPropertyChanged("WidthPointOfOrigin");
            }
        }

        public string Divergence
        {
            get { return _divergence+" %"; }
            set
            {
                _divergence = value;
            }
        }

        public string Width
        {
            get { return _width + " "+ InitChartTypeA.DistanceConverter.Unit; }
            set
            {
                _width = value;
                NotifyPropertyChanged("Width");
            }
        }

        public string Length
        {
            get { return _length + " " + InitChartTypeA.DistanceConverter.Unit; }
            set
            {
                _length = value;
                NotifyPropertyChanged("Length");
            }
        }

        public int PenetratedObsCount
        {
            get { return PenetratedObstacleList.Count; }
        }

        public int ChartObsCount
        {
            get { return ShadowedObstacleList.Count; }
        }

        public double GetMapChartWidth()
        {
            return Common.DeConvertDistance(ShadowedObstacleList.Max(obs => obs.X)+_rwyDirlist[0].Length);
        }

        public double GetMapChartHeight()
        {
            double maxDistance = ShadowedObstacleList.Max(obs => obs.X);

            var chartHeight =Common.DeConvertDistance( Convert.ToDouble(_widthPointOfOrigin) +
                            2 * (maxDistance * Convert.ToDouble(_divergence)) / 100)+100;
            return chartHeight;
        }

        private bool _isOffset;
        public bool IsOffset
        {
            get { return _isOffset; }
            set 
            {
                _isOffset = value;
                if (_isOffset)
                    CalculateWithTurn();
                else
                    CalculateWithoutTurn();
            }
        }

        private double _offset;
        public double Offset
        {
            get { return _offset; }
            set 
            {
                _offset = value;
                if (_isOffset)
                    CalculateWithTurn();

                NotifyPropertyChanged("Offset");
            }
        }

        private RwyDirWrapper _selectedRwyDir;
        public RwyDirWrapper SelectedRwyDir
        {
            get { return _selectedRwyDir; }
            set 
            {
                _selectedRwyDir = value;

                if (IsOffset)
                    CalculateWithTurn();

                NotifyPropertyChanged("SelectedRwyDir");
            }
        }

        private TakeoffSide _selectedTakeOffSide;
        public TakeoffSide SelectedTakeOffSide
        {
            get { return _selectedTakeOffSide; }
            set
            {
                _selectedTakeOffSide = value;
                NotifyPropertyChanged("SelectedTakeOffSide");
            }
        }

        private void CalculateWithTurn()
        {
            Clear();
            Reset();
            Calc(SelectedRwyDir);
          
        }

        private void CalculateWithoutTurn()
        {
            Clear();
            Reset();
            foreach (var rwyDirWrapper in _rwyDirlist)
                Calc(rwyDirWrapper);
        }

        private void Calc(RwyDirWrapper rwyDir) {

            var a =IsOffset?Aran.PANDA.Common.ARANMath.DegToRad(_offset):0;
            var takeOffArea = new TakeOffClimb(rwyDir,a);

            takeOffArea.CreateTakeOffPlane(_slope);
            takeOffArea.CreateReport();

            takeOffArea.CalculateObstacleInChart();

            if (takeOffArea.ShadowedObstacleList != null)
                takeOffArea.ShadowedObstacleList.ForEach(report => PenetratedObstacleList.Add(report));

            if (takeOffArea.ObstacleUsedInChart != null)
                takeOffArea.ObstacleUsedInChart.ForEach(report => ShadowedObstacleList.Add(report));


            Width = Common.ConvertDistance(takeOffArea.FinalWidth, roundType.toNearest).ToString();
            Length = Common.ConvertDistance(takeOffArea.Length, roundType.toNearest).ToString();
            Divergence = takeOffArea.Divergence.ToString();
            WidthPointOfOrigin = Common.ConvertDistance(takeOffArea.WidthPointOfOrigin, roundType.toNearest).ToString();

            TakeOffClimbs.Add(takeOffArea);
            if (takeOffArea.Geo != null)
            {
                _takeOffHandles.Add(
                    GlobalParams.UI.DrawEsriDefaultMultiPolygon(
                        takeOffArea.Geo as ESRI.ArcGIS.Geometry.IPolygon));

                _takeOffHandles.Add(
                    GlobalParams.UI.DrawEsriDefaultMultiPolygon(
                        takeOffArea.ClearWayGeo as ESRI.ArcGIS.Geometry.IPolygon));

            }

        }

        private void Reset() {

            TakeOffClimbs.Clear();
            ShadowedObstacleList.Clear();
            PenetratedObstacleList.Clear();
        }

        public override void Clear()
        {
            

            TakeOffClimbs.Clear();
            if (_takeOffHandles != null)
            {
                _takeOffHandles.ForEach(_takOffHandle => GlobalParams.UI.SafeDeleteGraphic(_takOffHandle));
                if (_selectedPenetratedObstacle != null)
                    _selectedPenetratedObstacle.Clear();
            }
        }

        public ESRI.ArcGIS.Geometry.IPolygon CalculateTaxiwayElement(List<TaxiwayElement> taxiwayElementList, List<RunwayElement> rwyElementList)
        {
            var allTaxiwayIntersectingList = new List<ESRI.ArcGIS.Geometry.IGeometry>();

            if (taxiwayElementList != null && taxiwayElementList.Count > 0)
            {
                bool isIntersect = false;
                for (int i = 1; i < taxiwayElementList.Count; i++)
                {
                    taxiwayElementList[i].RebuildGeo();
                    if (!isIntersect)
                    {
                        taxiwayElementList[i-1].RebuildGeo();
                        var prjGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(taxiwayElementList[i-1].Geo);
                        allTaxiwayIntersectingList.Add(prjGeo);
                    }

                    isIntersect = false;
                    for (int j = 0; j < allTaxiwayIntersectingList.Count; j++)
                    {
                        if (taxiwayElementList[i].Geo == null) continue;

                        var tmpGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(taxiwayElementList[i].Geo);
                        if (EsriFunctions.ReturnDistanceAsMetr(tmpGeo, allTaxiwayIntersectingList[j]) < 5)
                        {
                            isIntersect = true;
                            allTaxiwayIntersectingList[j] = EsriFunctions.Union(tmpGeo, allTaxiwayIntersectingList[j]);
                        }
                    }
                }
            }

            ESRI.ArcGIS.Geometry.IPolygon normResultGeo = new ESRI.ArcGIS.Geometry.PolygonClass();
            foreach (var rwyElement in rwyElementList)
            {
                rwyElement.RebuildGeo();
                if (rwyElement.Geo == null) continue;
                
                var elementGeo =GlobalParams.SpatialRefOperation.ToEsriPrj(rwyElement.Geo);
                foreach (var taxiwayInterGeo in allTaxiwayIntersectingList)
                {
                    if (EsriFunctions.ReturnDistanceAsMetr(taxiwayInterGeo, elementGeo) < 5) 
                    {
                        var tmpGeo = EsriFunctions.Union(taxiwayInterGeo, elementGeo);
                        normResultGeo =(ESRI.ArcGIS.Geometry.IPolygon) EsriFunctions.Union(normResultGeo, tmpGeo);
                    }
                }
            }

            return normResultGeo;
        
        }
    }

    public class TakeOffSideToStringConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            return GetDescription((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }
    }
}
