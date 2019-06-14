using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.ADF.BaseClasses;
using System.Windows.Forms;
using System.IO;
using Aran.Aim.InputFormLib;
using ESRI.ArcGIS.Geometry;
using Aran.AranEnvironment.Symbols;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;

namespace MapEnv
{
    public class GeomCreatorTool : BaseTool
    {
        public event EventHandler FormHidden;

        public GeomCreatorTool ()
        {
            m_cursor = Globals.GetCursor ("create_geom");
            _geomGraphicsHandle = -1;
            _pointGraphicsHandle = -1;
        }

        public override void OnCreate (object hook)
        {
        }

        public override void OnMouseUp (int Button, int Shift, int X, int Y)
        {
            if (Button == 1)
            {
                ShowForm ();

                IPoint pt = Globals.MainForm.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint (X, Y);
                pt.Project (EsriSR_WGS84);
                _creatorForm.AddPoint (pt.X, pt.Y);
            }
        }

        public void ShowForm ()
        {
            if (_creatorForm == null)
            {
                _creatorForm = new GeomCreatorForm ();
                _creatorForm.GeometryChanged += new EventHandler (CreatorForm_GeometryChanged);
                _creatorForm.FormCloseClicked += new EventHandler (CreatorForm_FormCloseClicked);
                _creatorForm.SelectedPointChanged += new EventHandler (CreatorForm_SelectedPointChanged);
                _creatorForm.Show (Globals.MainForm);
            }

            _creatorForm.Visible = true;
            
        }

        public void HideForm ()
        {
            if (_creatorForm == null)
                return;

            _creatorForm.Hide ();
        }

        
        private ISpatialReference EsriSR_WGS84
        {
            get
            {
                if (_esriSR_wgs84 == null)
                    _esriSR_wgs84 = Globals.CreateWGS84SR ();
                return _esriSR_wgs84;
            }
        }

        private Aran.Geometries.SpatialReferences.SpatialReference AranSR_WGS84
        {
            get
            {
                if (_aranSR_wgs84 == null)
                {
                    SpatRefConverter srConv = new SpatRefConverter ();
                    _aranSR_wgs84 = srConv.FromEsriSpatRef (EsriSR_WGS84);
                }

                return _aranSR_wgs84;
            }
        }

        private GeometryOperators GeomOper
        {
            get
            {
                if (_geomOper == null)
                {
                    _geomOper = new GeometryOperators ();
                }
                return _geomOper;
            }
        }

        private FillSymbol FillSymbol
        {
            get
            {
                if (_fillSymbol == null)
                {
                    _fillSymbol = new FillSymbol ();
                    _fillSymbol.Color = System.Drawing.Color.Blue.ToArgb ();
                    _fillSymbol.Style = eFillStyle.sfsNull;

                    LineSymbol lineSymbol = new LineSymbol (eLineStyle.slsSolid, 255, 1);
                    lineSymbol.Width = 1;
                    _fillSymbol.Outline = lineSymbol;
                }

                return _fillSymbol;
            }
        }

        private PointSymbol PointSymbol
        {
            get
            {
                if (_pointSymbol == null)
                {
                    _pointSymbol = new PointSymbol (255, 8);
                }

                return _pointSymbol;
            }
        }

        private IAranGraphics AranGraphics
        {
            get { return Globals.Environment.Graphics; }
        }

        private void CreatorForm_FormCloseClicked (object sender, EventArgs e)
        {
            AranGraphics.SafeDeleteGraphic (_geomGraphicsHandle);
            AranGraphics.Refresh ();

            if (FormHidden != null)
                FormHidden (this, e);
        }

        private void CreatorForm_GeometryChanged (object sender, EventArgs e)
        {
            if (_creatorForm == null)
                return;

            AranGraphics.SafeDeleteGraphic (_geomGraphicsHandle);

            var geom = _creatorForm.CurrentGeometry;

            if (AranGraphics.ViewProjection != null &&
                AranGraphics.ViewProjection.Name != EsriSR_WGS84.Name)
            {
                geom = GeomOper.GeoTransformations (geom, AranSR_WGS84, AranGraphics.ViewProjection);

                Aran.Geometries.Geometry tmpGeom = null;

                if (geom.Type == Aran.Geometries.GeometryType.MultiPolygon)
                {
                    var mp = geom as Aran.Geometries.MultiPolygon;
                    if (mp.Count > 0)
                        tmpGeom = mp [0];
                }
                else if (geom.Type == Aran.Geometries.GeometryType.MultiLineString)
                {
                    var mls = geom as Aran.Geometries.MultiLineString;
                    if (mls.Count > 0)
                        tmpGeom = mls [0];
                }

                geom = tmpGeom;
            }

            if (geom != null)
            {
                if (geom.Type == Aran.Geometries.GeometryType.Polygon)
                    _geomGraphicsHandle = AranGraphics.DrawPolygon (geom as Aran.Geometries.Polygon, FillSymbol);
                else
                    _geomGraphicsHandle = AranGraphics.DrawLineString (geom as Aran.Geometries.LineString, FillSymbol.Outline);
            }
        }

        private void CreatorForm_SelectedPointChanged (object sender, EventArgs e)
        {
            AranGraphics.SafeDeleteGraphic (_pointGraphicsHandle);

            var point = _creatorForm.SelectedPoint;

            if (point == null)
                return;

            if (AranGraphics.ViewProjection != null &&
                AranGraphics.ViewProjection.Name != EsriSR_WGS84.Name)
            {
                var geom = GeomOper.GeoTransformations (point, AranSR_WGS84, AranGraphics.ViewProjection);
                point = geom as Aran.Geometries.Point;
            }

            if (point != null)
                _pointGraphicsHandle = AranGraphics.DrawPoint (point, PointSymbol);
        }

        private FillSymbol _fillSymbol;
        private PointSymbol _pointSymbol;
        private GeomCreatorForm _creatorForm;
        private ISpatialReference _esriSR_wgs84;
        private Aran.Geometries.SpatialReferences.SpatialReference _aranSR_wgs84;
        private GeometryOperators _geomOper;
        private int _geomGraphicsHandle;
        private int _pointGraphicsHandle;
    }
}
