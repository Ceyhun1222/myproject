using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Geometries;

namespace Aran.Queries.Common
{
    public partial class PolyGeometryForm : Form
    {
        private Dictionary<string, Geometries.Geometry> _sessionGeomDict;
        private Geometry _geom;

        public event EventHandler SaveClicked;
        public static SessionGeometriesEventHandler SessionGeometriesLoaded;

        public PolyGeometryForm ()
        {
            InitializeComponent ();

            ui_sessionGeomButton.Visible = (SessionGeometriesLoaded != null);
        }

        public void SetGeometry (Geometry geom)
        {
            ui_dgv.Rows.Clear ();

            if (geom.Type == GeometryType.MultiPolygon)
                SetPolygon (geom as MultiPolygon);
            else if (geom.Type == GeometryType.MultiLineString)
                SetPolyline (geom as MultiLineString);
            else
                return;

            _geom = geom.Clone () as Geometry;
        }

        public Geometry GetGeometry ()
        {
            return _geom;
        }


        private void SetPolygon (MultiPolygon multiPolygon)
        {
            if (multiPolygon.Count == 0)
                return;

            SetMultiPoint (multiPolygon [0].ExteriorRing);
        }

        private void SetPolyline (MultiLineString multiLineString)
        {
            if (multiLineString.Count == 0)
                return;

            SetMultiPoint (multiLineString [0]);
        }

        private void SetMultiPoint (MultiPoint multiPoint)
        {
            foreach (Aran.Geometries.Point point in multiPoint)
            {
                AddNewPointRow (point);
            }
        }

        private void AddNewPointRow (Aran.Geometries.Point point)
        {
            var index = ui_dgv.Rows.Add ();
            var row = ui_dgv.Rows [index];
            row.Tag = point;

            row.Cells [ui_colX.Index].Value = point.X;
            row.Cells [ui_colY.Index].Value = point.Y;
            row.Cells [ui_colZ.Index].Value = point.Z;
        }

        private void Close_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void SessionGeoms_Click (object sender, EventArgs e)
        {
            if (SessionGeometriesLoaded == null)
                return;

            SessionGeometriesEventArgs sg = new SessionGeometriesEventArgs ();
            _sessionGeomDict = sg.GeometriesDict;

            SessionGeometriesLoaded (this, sg);

            EventHandler menuClick = new EventHandler (SessionGeomKeyMenu_Clicked);
            ui_sessionGeomMenu.Items.Clear ();

            foreach (string key in _sessionGeomDict.Keys)
            {
                var geom = _sessionGeomDict [key];

                if ((_geom.Type == GeometryType.MultiPolygon && geom.Type == GeometryType.Polygon) ||
                    (_geom.Type == GeometryType.MultiLineString && geom.Type == GeometryType.LineString))
                {
                    ui_sessionGeomMenu.Items.Add (key, null, menuClick).Tag = key;
                }
            }

            ui_sessionGeomMenu.Show (ui_sessionGeomButton,
                new System.Drawing.Point (0, 0), ToolStripDropDownDirection.AboveRight);
        }

        private void SessionGeomKeyMenu_Clicked (object sender, EventArgs e)
        {
            var key = (sender as ToolStripItem).Tag as string;
            Geometries.Geometry geom = _sessionGeomDict [key];

            if (_geom.Type == GeometryType.MultiPolygon)
            {
                var mpolygon = _geom as MultiPolygon;
                mpolygon.Add (geom.Clone () as Polygon);
                geom = mpolygon;
            }
            else if (_geom.Type == GeometryType.MultiLineString)
            {
                var mls = _geom as MultiLineString;
                mls.Add (geom.Clone () as LineString);
                geom = mls;
            }

            SetGeometry (geom);
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (SaveClicked != null)
            {
                SaveClicked (this, e);
            }

            Close ();
        }
    }
}
