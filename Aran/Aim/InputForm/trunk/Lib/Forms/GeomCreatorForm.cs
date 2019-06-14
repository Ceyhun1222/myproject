using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Geometries;

namespace Aran.Aim.InputFormLib
{
    public partial class GeomCreatorForm : Form
    {
        public event EventHandler GeometryChanged;
        public event EventHandler SelectedPointChanged;
        public event EventHandler FormCloseClicked;

        public GeomCreatorForm ()
        {
            InitializeComponent ();

            GeometryDictionary = new Dictionary<string, Geometry> ();
        }

        public static Dictionary<string, Geometry> GeometryDictionary;

        public static Bitmap PolygonImage
        {
            get { return Properties.Resources.polygon_16; }
        }

        public void AddPoint (double x, double y)
        {
            var geom = CurrentGeometry;

            if (geom == null)
            {
                AddNewGeomElement (CreatorGeomType.Polygon);
                geom = CurrentGeometry;
            }

            var point = new Aran.Geometries.Point (x, y);
            var newIndex = AddNewPointRow (point);

            if (geom.Type == GeometryType.Polygon)
                (geom as Polygon).ExteriorRing.Add (point);
            else
                (geom as LineString).Add (point);

            ui_dgv.CurrentCell = ui_dgv.Rows [newIndex].Cells [ui_colX.Index];

            DoGeometryChanged ();
            DoSelectedPointChanged ();
        }

        public Geometry CurrentGeometry
        {
            get
            {
                var row = ui_groupDGV.CurrentRow;
                if (row == null)
                    return null;

                return row.Tag as Geometry;
            }
        }

        public Aran.Geometries.Point SelectedPoint
        {
            get
            {
                if (ui_dgv.CurrentRow == null)
                    return null;

                return ui_dgv.CurrentRow.Tag as Aran.Geometries.Point;
            }
        }


        protected override void OnVisibleChanged (EventArgs e)
        {
            base.OnVisibleChanged (e);

            GroupDGV_CurrentCellChanged (null, null);
        }

        private void DoGeometryChanged ()
        {
            if (GeometryChanged != null)
                GeometryChanged (this, null);
        }

        private void DoSelectedPointChanged ()
        {
            if (SelectedPointChanged != null)
                SelectedPointChanged (this, null);
        }

        private void Close_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void GeomCreatorForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                ui_dgv.Rows.Clear ();
                
                Hide ();

                if (FormCloseClicked != null)
                    FormCloseClicked (this, null);
            }
        }

        private void AddNewGeomElement (CreatorGeomType geomType)
        {
            string name = GenerateNewName (geomType);

            Geometry geom = null;

            if (geomType == CreatorGeomType.Polygon)
                geom = new Polygon ();
            else
                geom = new LineString ();

            var index = ui_groupDGV.Rows.Add ();
            var row = ui_groupDGV.Rows [index];
            row.Tag = geom;
            row.Cells [ui_colName.Index].Value = name;
            row.Cells [ui_colName.Index].Tag = name;

            row.Cells [ui_colImage.Index].Value = (geomType == CreatorGeomType.Polygon ?
                Properties.Resources.polygon_16 : Properties.Resources.polyline_16);
            row.Cells [ui_colImage.Index].Tag = geomType;

            ui_groupDGV.CurrentCell = row.Cells [ui_colName.Index];

            GeometryDictionary.Add (name, geom);
        }

        private string GenerateNewName (CreatorGeomType geomType)
        {
            string s = "New " + (geomType == CreatorGeomType.Line ? "Line" : "Polygon");
            string name = s;
            int n = 1;

            while (IsNameExists (name))
            {
                name = s + "-" + (n++);
            }

            return name;
        }

        private bool IsNameExists (string name)
        {
            foreach (DataGridViewRow row in ui_groupDGV.Rows)
            {
                if (row.Cells [0].Value.ToString () == name)
                    return true;
            }
            return false;
        }

        private void GroupDGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (!Visible)
                return;

            ui_dgv.Rows.Clear ();

            var geom = CurrentGeometry;
            if (geom == null)
                return;

            if (geom.Type == GeometryType.Polygon)
            {
                foreach (Aran.Geometries.Point point in (geom as Polygon).ExteriorRing)
                    AddNewPointRow (point);
            }
            else
            {
                foreach (Aran.Geometries.Point point in (geom as LineString))
                    AddNewPointRow (point);
            }

            DoGeometryChanged ();
            DoSelectedPointChanged ();
        }

        private int AddNewPointRow (Aran.Geometries.Point point)
        {
            var index = ui_dgv.Rows.Add ();
            var row = ui_dgv.Rows [index];
            row.Tag = point;

            row.Cells [ui_colX.Index].Value = point.X;
            row.Cells [ui_colY.Index].Value = point.Y;
            row.Cells [ui_colZ.Index].Value = point.Z;

            return index;
        }

        private void GroupDGV_CellValueChanged (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;

            if (e.ColumnIndex == ui_colName.Index)
            {
                var cell = ui_groupDGV.Rows [e.RowIndex].Cells [e.ColumnIndex];
                if (cell == null || cell.Tag == null)
                    return;

                string oldName = cell.Tag.ToString ();
                string newName = cell.Value.ToString ();

                var geom = GeometryDictionary [oldName];
                GeometryDictionary.Remove (oldName);
                GeometryDictionary.Add (newName, geom);

                cell.Tag = newName;
            }
        }

        private void NewPolygon_Click (object sender, EventArgs e)
        {
            AddNewGeomElement (CreatorGeomType.Polygon);
        }

        private void NewPolyline_Click (object sender, EventArgs e)
        {
            AddNewGeomElement (CreatorGeomType.Line);
        }

        private void ChangeGeomType_Click (object sender, EventArgs e)
        {
            var currCell = ui_groupDGV.CurrentCell;

            if (currCell == null || currCell.ColumnIndex != ui_colImage.Index)
                return;

            var row = ui_groupDGV.Rows [currCell.RowIndex];

            var oldGeom = row.Tag as Geometry;
            var geomType = (CreatorGeomType) currCell.Tag;
            Geometry newGeom = null;
            CreatorGeomType newGeomType;

            if (geomType == CreatorGeomType.Line)
            {
                var polygon = new Polygon ();
                polygon.ExteriorRing.AddMultiPoint (oldGeom as LineString);
                newGeom = polygon;
                newGeomType = CreatorGeomType.Polygon;
            }
            else
            {
                var lineString = new LineString ();
                lineString.AddMultiPoint ((oldGeom as Polygon).ExteriorRing);
                newGeom = lineString;
                newGeomType = CreatorGeomType.Line;
            }

            row.Tag = newGeom;
            row.Cells [ui_colImage.Index].Value = (newGeomType == CreatorGeomType.Polygon ?
                Properties.Resources.polygon_16 : Properties.Resources.polyline_16);
            row.Cells [ui_colImage.Index].Tag = newGeomType;

            string key = row.Cells [ui_colName.Index].Value.ToString ();
            GeometryDictionary [key] = newGeom;
        }

        private void DGV_CurrentCellChanged (object sender, EventArgs e)
        {
            DoSelectedPointChanged ();
        }

        private void DGV_RowsAdded (object sender, DataGridViewRowsAddedEventArgs e)
        {
            SetPointCountText ();
        }

        private void DGV_RowsRemoved (object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SetPointCountText ();
        }

        private void DGV_CellValueChanged (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;

            var row = ui_dgv.Rows [e.RowIndex];
            var point = row.Tag as Aran.Geometries.Point;

            var cell = row.Cells [e.ColumnIndex];
            double d;
            if (e.ColumnIndex == ui_colX.Index)
                d = point.X;
            else if (e.ColumnIndex == ui_colY.Index)
                d = point.Y;
            else
                d = point.Z;

            try
            {
                d = Convert.ToDouble (cell.Value);

                if (e.ColumnIndex == ui_colX.Index)
                    point.X = d;
                else if (e.ColumnIndex == ui_colY.Index)
                    point.Y = d;
                else
                    point.Z = d;

                DoGeometryChanged ();
                DoSelectedPointChanged ();
            }
            catch
            {
                cell.Value = d;
            }

        }

        private void SetPointCountText ()
        {
            ui_pointCountLabel.Text = ui_dgv.Rows.Count.ToString ();
        }
    }

    public enum CreatorGeomType
    {
        Line = GeometryType.LineString,
        Polygon = GeometryType.Polygon
    }
}
