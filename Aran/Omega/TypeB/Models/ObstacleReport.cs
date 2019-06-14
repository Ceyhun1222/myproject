using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Aim;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using Aran.Panda.Constants;

namespace Aran.Omega.TypeB
{
    public enum ObstacleGeomType
    {
        Point,
        Polygon,
        PolyLine
    }

    public class ObstacleReport : ViewModels.ViewModel
    {
        private SolidColorBrush _mySolidColorBrush;
        private int _selectedObstacleHandle;
        public ObstacleReport(SurfaceType surfaceType)
        {
            _mySolidColorBrush = new SolidColorBrush();
            _mySolidColorBrush.Color = Color.FromRgb(255, 0, 0);
            SurfaceType = surfaceType;
            
        }
        public long Id { get; set; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
                NotifyPropertyChanged("CellColor");
            }
        }

        public ObstacleGeomType GeomType { get; set; }

        public CodeVerticalStructure? VsType { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        
        public double Elevation { get; set; }

        public double Penetrate { get; set; }

        public string Plane { get; set; }

        public Aran.Aim.Features.VerticalStructure Obstacle { get; set; }

        public Aran.Geometries.Geometry GeomPrj { get; set; }

        public Aran.Geometries.Point ExactVertexGeom { get; set; }

        public double SurfaceElevation { get; set; }

        private ObstacleReport _selectedObstacle;

        public ObstacleReport SelectedObstacle
        {
            get { return _selectedObstacle; }
            set 
            {
                _selectedObstacle = value;
                GlobalParams.UI.SafeDeleteGraphic(_selectedObstacleHandle);
                
                if (GeomPrj is  Aran.Geometries.Point)
                    _selectedObstacleHandle = GlobalParams.UI.DrawPoint(GeomPrj as Aran.Geometries.Point, 9821, Aran.AranEnvironment.Symbols.ePointStyle.smsCircle);
                else if (GeomPrj is MultiLineString)
                    _selectedObstacleHandle = GlobalParams.UI.DrawMultiLineString(GeomPrj as MultiLineString, 12314, 2);
                else if (GeomPrj is Aran.Geometries.MultiPolygon)
                   _selectedObstacleHandle = GlobalParams.UI.DrawMultiPolygon(GeomPrj as MultiPolygon, 2322, Aran.AranEnvironment.Symbols.eFillStyle.sfsCross);
            }
        }

        public SurfaceType SurfaceType { get; set; }

        public double Distance { get; set; }


        public Brush CellColor
        {
            get
            {
                if (Penetrate > 0)
                    return _mySolidColorBrush;
                return Brushes.Black;
            }

        }


        #region ::GridTexts

       
        #endregion


    }
}
