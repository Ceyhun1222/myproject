using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    public class BufferModel:ViewModels.ViewModel
    {
        public BufferModel(ViewModels.RouteBufferViewModel routeBufferViewModel)
        {
            _routeViewModel = routeBufferViewModel;
        }

        public BufferModel(ViewModels.AirspaceBufferViewModel airspaceBufferViewModel)
        {
            _airspaceBufferViewModel = airspaceBufferViewModel;
        }
        private int _bufferHandle;
        private ViewModels.RouteBufferViewModel _routeViewModel;
        
        public string Name { get; set; }
        public Aran.Geometries.MultiPolygon BufferGeom { get; set; }
        public double Width { get; set; }
        public Aran.Geometries.Geometry FeatureGeo { get; set; }
        public Aran.Aim.Features.Feature SelectedFeature { get; set; }
        public List<BufferSegmentModel> RouteSegmentList { get; set; }

        public string MarkerLayer { get; set; }
        public string MarkerObjectName { get; set; }

        public void Draw()
        {
            Clear();
            if (BufferGeom != null && !BufferGeom.IsEmpty)
                _bufferHandle = GlobalParams.UI.DrawMultiPolygon(BufferGeom, GlobalParams.Settings.SymbolModel.BufferSymbol);
        }

        public void Clear() 
        {
            GlobalParams.UI.SafeDeleteGraphic(_bufferHandle);
        }

        private bool _isSelected;
        private ViewModels.AirspaceBufferViewModel _airspaceBufferViewModel;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if ((_routeViewModel!=null && _routeViewModel.IsDrawBuffer) || (_airspaceBufferViewModel!=null && _airspaceBufferViewModel.IsDrawBuffer))
                {
                    if (_isSelected)
                        Draw();
                    else
                        Clear();
                }
                NotifyPropertyChanged("IsSelected");
            }
        }
    }
}
