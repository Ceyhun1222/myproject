using System;
using System.Windows;
using Aran.AranEnvironment;
using ChoosePointNS;
using System.Collections.ObjectModel;
using Aran.Omega.TypeBEsri.Models;

namespace Aran.Omega.TypeBEsri.View
{
    /// <summary>
    /// Interaction logic for ManualReport.xaml
    /// </summary>
    public partial class ManualReport : Window
    {
        private bool _pointPickerClicked;
        private PointPicker _pointPicker1;
        private ViewModels.ManualReportViewModel _reportViewModel;

        public ManualReport()
        {
            InitializeComponent();
            _reportViewModel = new ViewModels.ManualReportViewModel();
            this.DataContext = _reportViewModel;
            GlobalParams.AranMapToolMenuItem = new AranEnvironment.AranTool();
            GlobalParams.AranMapToolMenuItem.Cursor = System.Windows.Forms.Cursors.Cross;
            GlobalParams.AranMapToolMenuItem.Visible = true;
            GlobalParams.AranMapToolMenuItem.MouseClickedOnMap +=
                new AranEnvironment.MouseClickedOnMapEventHandler(AranMapToolMenuItem_Click);
            GlobalParams.AranEnvironment.AranUI.AddMapTool(GlobalParams.AranMapToolMenuItem);
            IsClosed = false;

            LblObstacleAltitudeGeo.Text= InitOmega.HeightConverter.Unit;
            Closing += _reportViewModel.OnWindowClosing;
            _reportViewModel.DdDmsIsActive += new EventHandler(_reportViewModel_DdDmsIsActive);
        }

        private void _reportViewModel_DdDmsIsActive(object sender, EventArgs e)
        {
            var selectedPoint = sender as Aran.Geometries.Point;
            if (selectedPoint != null)
            {
                var geo =GlobalParams.SpatialRefOperation.ToGeo(selectedPoint);
                _pointPicker1.Latitude = geo.Y;
                _pointPicker1.Longitude = geo.X;
            }
        }

        public void SetAvailableSurfaces(ObservableCollection<DrawingSurface> availableSurfaceList )
        {
            _reportViewModel.SetAvailableSurfaces(availableSurfaceList);
        }

        private void AranMapToolMenuItem_Click(object sender, MapMouseEventArg e)
        {
            var pnt = new Aran.Geometries.Point(e.X, e.Y);
            _reportViewModel.SetSelectedPoint(pnt);
            SetPointPickerCoords(pnt);
        }

        private void PointPicker_ByClickChanged(object sender, EventArgs e)
        {
            _pointPickerClicked = !_pointPickerClicked;
            _pointPicker1 = (sender as PointPicker);
            if (_pointPicker1 == null)
                return;

            if (_pointPickerClicked)
            {
                GlobalParams.AranEnvironment.AranUI.SetCurrentTool(GlobalParams.AranMapToolMenuItem);
            }
            else
            {
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
                _pointPicker1.ByClick = false;
            }
        }

        private void SetPointPickerCoords(Aran.Geometries.Point pntPrj)
        {
            var pntGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point>(pntPrj);
            _pointPicker1.Latitude = pntGeo.Y;
            _pointPicker1.Longitude = pntGeo.X;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }

        public bool IsClosed { get; set; }
    }
}
