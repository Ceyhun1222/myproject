using Aran.Aim.Enums;
using Aran.Geometries;
using System.Windows.Media;
using Aran.PANDA.Constants;
using Aran.PANDA.Common;
using Aran.Aim.Features;
using System;

namespace Aran.Omega
{
    public enum ObstacleGeomType
    {
        Point,
        Polygon,
        PolyLine
    }
    public class ObstacleReport : ViewModels.ViewModel
    {
        private int _selectedObstacleHandle;
        private Geometries.Geometry _geoPrj;
        private Geometries.Geometry _geo;

        public ObstacleReport(SurfaceType surfaceType)
        {
            SurfaceType = surfaceType;
            EtodSurfaceType = EtodSurfaceType.Area2A;
        }

        public ObstacleReport(EtodSurfaceType etodSurfaceType)
        {
            EtodSurfaceType = etodSurfaceType;
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
                //NotifyPropertyChanged("CellColor");
            }
        }

        public string Designator => Part?.Designator;

        public ObstacleGeomType GeomType { get; set; }

        public CodeVerticalStructure? VsType { get; set; }

        public string Frangible
        {
            get
            {
                if (Part?.Frangible != null)
                {
                    if (Part.Frangible.Value)
                        return "Yes";
                }
                return "No";
            }
        }

        public string Lat
        {
            get
            {
                if (_geo == null)
                    RebuildGeo();

                if (!(_geo is Aran.Geometries.Point)) return "";

                return ARANFunctions.Degree2String(((Aran.Geometries.Point) _geo).Y, Degree2StringMode.DMSLat, 1);
            }
        }

        public string Long
        {
            get
            {
                if (_geo == null)
                    RebuildGeo();

                if (!(_geo is Aran.Geometries.Point)) return "";

                return ARANFunctions.Degree2String(((Aran.Geometries.Point)_geo).X, Degree2StringMode.DMSLon, 1);


            }
        }

        public double X { get; set; }
        public double Y { get; set; }

        public string Height { get; set; }

        public double VerticalAccuracy { get; set; }
        public double HorizontalAccuracy { get; set; }
        
        public double Elevation { get; set; }

        public double Penetrate { get; set; }

        public string Plane { get; set; }

        public void RebuildGeo()
        {
            if (_geoPrj != null)
                _geo = GlobalParams.SpatialRefOperation.ToGeo(_geoPrj);
        }

        public Aran.Aim.Features.VerticalStructure Obstacle { get; set; }

        public Aran.Geometries.Geometry GeomPrj
        {
            get{ return _geoPrj; }
            set
            {
                _geoPrj = value;
               //_geo = GlobalParams.SpatialRefOperation.ToGeo(_geo);
            }
        }

        public Aran.Geometries.Geometry Geo
        {
            get
            {
                if (_geo==null)
                    RebuildGeo();
                return _geo;
            }
        }

        public Aran.Geometries.Geometry BufferPrj { get; set; }
        public Aran.Geometries.Geometry IntersectGeom { get; set; }

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
                    _selectedObstacleHandle = GlobalParams.UI.DrawPoint(GeomPrj as Aran.Geometries.Point, Aran.AranEnvironment.Symbols.ePointStyle.smsCircle, 9821);
                else if (GeomPrj is MultiLineString)
                    _selectedObstacleHandle = GlobalParams.UI.DrawMultiLineString(GeomPrj as MultiLineString, 2, 12314);
                else if (GeomPrj is Aran.Geometries.MultiPolygon)
                   _selectedObstacleHandle = GlobalParams.UI.DrawMultiPolygon(GeomPrj as MultiPolygon, Aran.AranEnvironment.Symbols.eFillStyle.sfsCross, 2322);
            }
        }

        public SurfaceType SurfaceType { get; set; }

        public EtodSurfaceType EtodSurfaceType { get; set; }

        public double Distance { get; set; }

        public string Clr
        {
            get
            {
                if (Penetrate > 0)
                    return "Red";
                return "Black";
            }
        }

        public VerticalStructurePart Part { get; internal set; }


        #region ::GridTexts


        #endregion


    }
}
