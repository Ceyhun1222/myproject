using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aran.Delta.Model;
using ESRI.ArcGIS.Geometry;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for AirspaceType.xaml
    /// </summary>
    public partial class AirspaceType : Window
    {
        public event EventHandler AirsapceCreationTypeIsChanged;
        private Model.ArcModel _arcModel;
        private double _circleRadius;

        public AirspaceType()
        {
            InitializeComponent();

            TxtRadius.Text = 10.ToString();
            StackArc.Visibility = Visibility.Collapsed;
            StackButton.Visibility = Visibility.Collapsed;
        }


        public double CircleRadius
        {
            get { return _circleRadius; }
            set
            {
                _circleRadius = value;
                if (_arcModel != null)
                    _arcModel.Radius = Common.DeConvertDistance(_circleRadius);
            }
        }

        private void RdbSnap_OnChecked(object sender, RoutedEventArgs e)
        {
            if (RdbSnap.IsChecked!=null && RdbSnap.IsChecked==true)
            {
                if (_arcModel!=null)
                    _arcModel.DeleteGraphics();

                if (AirsapceCreationTypeIsChanged != null)
                    AirsapceCreationTypeIsChanged(Enums.CreatingAreaType.Border, new EventArgs());
            }
        }


     

        private void RdbArc_OnChecked(object sender, RoutedEventArgs e)
        {
            if (RdbArc.IsChecked != null && RdbArc.IsChecked == true)
            {
                _arcModel = new ArcModel();
                _arcModel.Radius = Common.DeConvertDistance(Convert.ToDouble(TxtRadius.Text));
                StackArc.Visibility = Visibility.Visible;
                StackButton.Visibility = Visibility.Visible;

                if (AirsapceCreationTypeIsChanged!=null)
                    AirsapceCreationTypeIsChanged(Enums.CreatingAreaType.Arc,new EventArgs());
            }
        }

        private void RdbIsCw_OnChecked(object sender, RoutedEventArgs e)
        {
            if (_arcModel!=null)
                CreateGeometry();
        }

        private void CreateGeometry()
        {
            if (_arcModel == null)
                return;

            if (_arcModel.ArcPt1 == null || _arcModel.ArcPt1.IsEmpty)
                return;

            if (_arcModel.ArcPt2 == null || _arcModel.ArcPt2.IsEmpty)
                return;

            IConstructCircularArc curCircularArc = new CircularArcClass();
            curCircularArc.ConstructEndPointsRadius(_arcModel.ArcPt1, _arcModel.ArcPt2, _arcModel.IsCw, _arcModel.Radius,true);
            ISegmentCollection ring = new RingClass();
            ring.AddSegment(curCircularArc as ISegment);
            
            IGeometryCollection poly = new PolygonClass();
            poly.AddGeometry(ring as IGeometry);
            _arcModel.ArcGeometry = poly as IPolygon;
            _arcModel.Draw();
        }

        public void PointChanged(IPoint arcPt)
        {
            if (_arcModel != null)
            {
                if (_arcModel.ArcPt1 != null && !_arcModel.ArcPt1.IsEmpty)
                    _arcModel.ArcPt2 = arcPt;
                else
                    _arcModel.ArcPt1 = arcPt;
                CreateGeometry();
            }
        }

        private void AirspaceType_OnClosing(object sender, CancelEventArgs e)
        {
            if (_arcModel!=null)
                _arcModel.DeleteGraphics();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_arcModel != null)
                _arcModel.Radius = Common.DeConvertDistance(Convert.ToDouble(TxtRadius.Text));
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            _arcModel.ArcPt1.SetEmpty();
            _arcModel.ArcPt2.SetEmpty();
            _arcModel.DeleteGraphics();
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            _arcModel.DeleteGraphics();
        }
    }
}
