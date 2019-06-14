using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ESRI.ArcGIS.Geometry;

namespace ChartTypeA.Models
{
    public enum ObstacleGeomType
    {
        Point,
        Polygon,
        PolyLine
    }

    public class ObstacleReport : NotifyClass
    {
        private SolidColorBrush _mySolidColorBrush;
        private int _selectedObstacleHandle;
        public ObstacleReport()
        {
            _mySolidColorBrush = new SolidColorBrush();
            _mySolidColorBrush.Color = Color.FromRgb(255, 0, 0);
           // SurfaceType = surfaceType;

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

        //  public CodeVerticalStructure? VsType { get; set; }

        public string RwyDir { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public double VerticalAccuracy { get; set; }
        public double HorizontalAccuracy { get; set; }

        public double Elevation { get; set; }

        public double Penetrate { get; set; }

        public string Plane { get; set; }

        public PDM.VerticalStructure Obstacle { get; set; }

        public ESRI.ArcGIS.Geometry.IGeometry GeomPrj { get; set; }
        public ESRI.ArcGIS.Geometry.IGeometry IntersectGeom { get; set; }

        public ESRI.ArcGIS.Geometry.IPoint ExactVertexGeom { get; set; }
        

        public double SurfaceElevation { get; set; }

        public string ShadowedBy { get; set; }

        private ObstacleReport _selectedObstacle;
      
        //public SurfaceType SurfaceType { get; set; }

        public void Draw()
        {
            Clear();
            if (GeomPrj != null && !GeomPrj.IsEmpty)
            {
                if (GeomPrj.GeometryType == esriGeometryType.esriGeometryPoint)
                    _selectedObstacleHandle = GlobalParams.UI.DrawEsriPoint(GeomPrj as IPoint);
                    //else if (GeomPrj is MultiLineString)
                    //    _selectedObstacleHandle = GlobalParams.UI.(GeomPrj as MultiLineString, 12314, 2);
                else if (GeomPrj.GeometryType == esriGeometryType.esriGeometryPolygon)
                    _selectedObstacleHandle = GlobalParams.UI.DrawEsriDefaultMultiPolygon(GeomPrj as IPolygon);
            }
        }

        public void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_selectedObstacleHandle);
        }

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
