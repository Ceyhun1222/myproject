using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using Aran.Aim.Features;
using ESRI.ArcGIS.Display;

namespace Aran.Delta.Model
{
    public class BufferSegmentModel:ViewModels.ViewModel
    {
        private int _geoHandle;
        private ISymbol _segmentSymbol;
        public BufferSegmentModel(ViewModels.RouteBufferViewModel routeViewModel, Aran.Aim.Features.RouteSegment routeSegment)
        {
            CreateSymbol();

            _routeViewModel = routeViewModel;
            RouteSegment = routeSegment;
            if (routeSegment.CurveExtent.Geo!=null)
                Geo = GlobalParams.SpatialRefOperation.ToPrj(routeSegment.CurveExtent.Geo);

            Name = "";
            if (routeSegment.Start != null)
            {
                StartPoint  = GetSegmentPtName(routeSegment.Start);
                Name += StartPoint;
            }
            else
                IsValid = false;

            IsValid = true;
            if (routeSegment.End != null)
            {
                EndPoint = GetSegmentPtName(routeSegment.End);
                Name += " - " + EndPoint;
            }
            else
                IsValid = false;

            IsSelected = true;
            
            SelectAllCommand = new RelayCommand(new Action<object>(select_All));
            UnselectAllCommand = new RelayCommand(new Action<object>(unselect_All));
        }

        public BufferSegmentModel(ViewModels.RouteBufferViewModel routeViewModel, DesigningSegment designingSegment)
        {
            CreateSymbol();

            _routeViewModel = routeViewModel;
            if (designingSegment.Geo != null)
                Geo = GlobalParams.SpatialRefOperation.ToPrj((Aran.Geometries.MultiLineString)designingSegment.Geo);

            Name = "";
            if (designingSegment.WptStart != null)
            {
                StartPoint = designingSegment.WptStart;
                Name += StartPoint;
            }
            else
                IsValid = false;

            IsValid = true;
            if (designingSegment.WptEnd != null)
            {
                EndPoint = designingSegment.WptEnd;
                Name += " - " + EndPoint;
            }
            else
                IsValid = false;

            IsSelected = true;

            SelectAllCommand = new RelayCommand(new Action<object>(select_All));
            UnselectAllCommand = new RelayCommand(new Action<object>(unselect_All));
        }

        public int RouteId { get; set; }
        public int Index { get; set; }

        public RelayCommand SelectAllCommand { get; set; }
        public RelayCommand UnselectAllCommand { get; set; }
        
        public string Name { get; set; }
        public Aran.Geometries.MultiLineString Geo { get; set; }
        public Aran.Aim.Features.RouteSegment RouteSegment { get; private set; }

        private bool _isSelected;
        private ViewModels.RouteBufferViewModel _routeViewModel;

        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                if (_routeViewModel.IsDrawSegments)
                {
                    if (_isSelected)
                        Draw();
                    else
                        Clear();
                }
                NotifyPropertyChanged("IsSelected");
            }
        }

        public bool IsValid { get; set; }

        public void Draw() 
        {
            Clear();
            _geoHandle = GlobalParams.UI.DrawMultiLineStringPrj(Geo, _segmentSymbol);
        }

        public void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_geoHandle);
        }

        public override string ToString()
        {
            return Name;
        }

        private string GetSegmentPtName(SegmentPoint segmentPt)
        {
            var start = segmentPt;
            var resultName = "";
            if (start.PointChoice.Choice == SignificantPointChoice.DesignatedPoint)
            {
                resultName = GlobalParams.Database.GetFeatureName(Aim.FeatureType.DesignatedPoint, start.PointChoice.FixDesignatedPoint.Identifier);
            }
            else if (start.PointChoice.Choice == SignificantPointChoice.RunwayCentrelinePoint)
            {
                resultName = GlobalParams.Database.GetFeatureName(Aim.FeatureType.RunwayCentrelinePoint,
                    start.PointChoice.RunwayPoint.Identifier);
            }

            else if (start.PointChoice.Choice == SignificantPointChoice.Navaid)
            {
                resultName = GlobalParams.Database.GetFeatureName(Aim.FeatureType.Navaid,
                    start.PointChoice.NavaidSystem.Identifier);
            }

            else if (start.PointChoice.Choice == SignificantPointChoice.AirportHeliport)
            {
                resultName = GlobalParams.Database.GetFeatureName(Aim.FeatureType.AirportHeliport,
                    start.PointChoice.AirportReferencePoint.Identifier);

            }
            return resultName;

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

        private void unselect_All(object obj)
        {
            _routeViewModel.UnselectAllSegments();
        }

         private void select_All(object obj)
        {
            _routeViewModel.SelectAllSegments();
        }

    }
}
