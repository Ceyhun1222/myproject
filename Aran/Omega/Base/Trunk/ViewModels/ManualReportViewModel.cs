using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Aran.Omega.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Aran.Omega.ViewModels
{
    public enum PointType
    {
        Local,
        DdDms
    }

    public class ManualReportViewModel : ViewModel
    {
        private PointType _pointType;
        private Aran.Geometries.Point _selectedPoint;
        private int _ptHandle;
        private Geometries.Point _refPointPrj;
        private double _direction;

        public ManualReportViewModel()
        {
            _pointType = PointType.Local;
            CalculateCommand = new RelayCommand(new Action<object>(calculateCommand_onclick));
        }

        public ObservableCollection<PointPenetrateModel> SurfacePenetrationList { get; set; }

        private ObservableCollection<DrawingSurface> _availableSurfaceList;

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                _selectedPoint = Aran.PANDA.Common.ARANFunctions.LocalToPrj(_refPointPrj, _direction + Math.PI, _x, _y);
                DrawPt();
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                _selectedPoint = Aran.PANDA.Common.ARANFunctions.LocalToPrj(_refPointPrj, _direction + Math.PI, _x, _y);
                DrawPt();
            }
        }

        private void DrawPt()
        {
            GlobalParams.UI.SafeDeleteGraphic(_ptHandle);
			_ptHandle = GlobalParams.UI.DrawPoint(_selectedPoint, AranEnvironment.Symbols.ePointStyle.smsCircle,
				Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0));
        }

        public double Altitude { get; set; }

        private PointType _selectedPointType;

        public int SelectedPointType
        {
            get { return (int) _selectedPointType; }
            set
            {
                if (_selectedPointType == ((PointType) value))
                    return;

                _selectedPointType = (PointType) value;
                if (_selectedPointType == PointType.DdDms)
                {
                    if (DdDmsIsActive != null)
                    {
                        DdDmsIsActive(_selectedPoint, new EventArgs());
                    }
                }
            }
        }

        public ICommand CalculateCommand { get; set; }

        public void SetSelectedPoint(Aran.Geometries.Point mapSelectedPoint)
        {
            _selectedPoint = mapSelectedPoint;
            Aran.PANDA.Common.ARANFunctions.PrjToLocal(_refPointPrj, _direction + Math.PI, _selectedPoint,
                            out _x,
                            out _y);
            _x = Common.ConvertDistance(_x, Enums.RoundType.ToNearest);
            _y = Common.ConvertDistance(_y, Enums.RoundType.ToNearest);

            NotifyPropertyChanged("X");
            NotifyPropertyChanged("Y");
            DrawPt();
        }

        private void calculateCommand_onclick(object obj)
        {
            SurfacePenetrationList = new ObservableCollection<PointPenetrateModel>();
            if (_selectedPoint == null)
                return;
            _selectedPoint.Z =Common.DeConvertHeight(Altitude);
            foreach (var surfaces in _availableSurfaceList)
            {
                var manualReport = surfaces.SurfaceBase.GetManualReport(_selectedPoint);
                if (manualReport != null)
                {
                    manualReport.Surface = surfaces.ViewCaption;
                    SurfacePenetrationList.Add(manualReport);
                }
            }
            NotifyPropertyChanged("SurfacePenetrationList");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_ptHandle);
        }

        public void SetAvailableSurfaces(ObservableCollection<DrawingSurface> surfaceList)
        {
            _availableSurfaceList = surfaceList;

            if (_availableSurfaceList.Count > 0)
            {
                _refPointPrj = _availableSurfaceList[1].SurfaceBase.StartPoint;
                _direction = _availableSurfaceList[1].SurfaceBase.Direction;
            }
        }

        public event EventHandler DdDmsIsActive;
    }
}
