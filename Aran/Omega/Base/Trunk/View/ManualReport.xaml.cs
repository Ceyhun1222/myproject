using System;
using System.Windows;
using Aran.AranEnvironment;
using ChoosePointNS;
using System.Collections.ObjectModel;
using Aran.Omega.Models;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for ManualReport.xaml
    /// </summary>
    public partial class ManualReport : MetroWindow
    {
        private bool _pointPickerClicked;
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
                PointPicker1.Latitude = geo.Y;
                PointPicker1.Longitude = geo.X;
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
            PointPicker1 = (sender as PointPicker);
            if (PointPicker1 == null)
                return;

            if (_pointPickerClicked)
                GlobalParams.AranEnvironment.AranUI.SetCurrentTool(GlobalParams.AranMapToolMenuItem);
            else
            {
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
                PointPicker1.ByClick = false;
            }
        }

        private void SetPointPickerCoords(Aran.Geometries.Point pntPrj)
        {
            var pntGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point>(pntPrj);
            PointPicker1.Latitude = pntGeo.Y;
            PointPicker1.Longitude = pntGeo.X;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
            GlobalParams.AranEnvironment.AranUI.SetPanTool();
        }

        public bool IsClosed { get; set; }

        private void PointPicker_OnLatitudeChanged(object sender, EventArgs e)
        {
            PointPicker_OnLongitudeChanged(null, null);
        }

        private void PointPicker_OnLongitudeChanged(object sender, EventArgs e)
        {
            var pnt = new Aran.Geometries.Point(PointPicker1.Longitude, PointPicker1.Latitude);
            var pntPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(pnt);
            _reportViewModel.SetSelectedPoint(pntPrj);
           // SetPointPickerCoords(pnt);
        }
    }
}
