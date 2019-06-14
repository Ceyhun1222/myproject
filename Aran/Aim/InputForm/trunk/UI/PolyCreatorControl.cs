using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Geometries.Operators;
using Aran.Geometries;
using Aran.AranEnvironment.Symbols;

namespace Aran.Aim.InputForm
{
    public partial class PolyCreatorControl : UserControl
    {
        public event EventHandler BackClicked;
        public event EventHandler OKClicked;

        private MapTool _mapTool;
        private IAranGraphics _aranGraphics;
        private int _geomGraphicsHandle;
        private int _selPointGraphicsHandle;
        private Geometry _currentGeometry;
        private MultiPoint _currMultiPoint;
		private int _currRowIndex;
        private GeometryOperators _geomOper;
        private Aran.AranEnvironment.Symbols.FillSymbol _fillSymbol;
		private FeatureType _featureType;
        
         
        public PolyCreatorControl ()
        {
            InitializeComponent ();

            _geomGraphicsHandle = -1;
            _selPointGraphicsHandle = -1;
			_currRowIndex = -1;
            _geomOper = new GeometryOperators ();

            _mapTool = new MapTool ();
            _mapTool.Cursor = Globals.GetCursor ("create_geom");

            _aranGraphics = Globals.Environment.Graphics;

            _fillSymbol = new FillSymbol ();
            _fillSymbol.Color = System.Drawing.Color.Blue.ToArgb ();
            _fillSymbol.Style = eFillStyle.sfsNull;

            LineSymbol lineSymbol = new LineSymbol (eLineStyle.slsSolid, 255, 1);
            lineSymbol.Width = 1;
            _fillSymbol.Outline = lineSymbol;
        }

        public Form OwnerForm { get; set; }

        public Queries.Viewer.ShowGeometryEventArgs ShowGeometryEventArgs { get; set; }

        public string PathText
        {
            get { return ui_pathLabel.Text; }
            set { ui_pathLabel.Text = value; }
        }

		public FeatureType FeatureType
		{
			get { return _featureType; }
			set
			{
				_featureType = value;
				ui_fromDeltaProTSMI.Visible = false;

				if (value == Aim.FeatureType.Airspace)
				{
					if (!DeltaProLoaderForm._isDeltaProParserObjectLoaded)
					{
						DeltaProLoaderForm._isDeltaProParserObjectLoaded = true;
						DeltaProLoaderForm.LoadDeltaParserAssembly ();
					}

					ui_fromDeltaProTSMI.Visible = (DeltaProLoaderForm._deltaProParserObject != null);
				}
			}
		}

        public Geometry GeomValue
        {
            get { return _currentGeometry; }
            set
            {
                _currMultiPoint = null;
                ui_partRingCB.Items.Clear ();
                _mapTool.MouseUp = MapTool_MouseUp;

                string partOrRing = "Ring";
                _currentGeometry = value.Clone () as Geometry;

                if (!IsLine)
                {
                    partOrRing = "Part";
                }

				ui_partOrRingGrBox.Text = partOrRing;

                if (_currentGeometry.IsEmpty)
                {
                    AddNewPartRing_Click (null, null);
                }
                else
                {
                    if (IsLine)
                    {
                        var mls = _currentGeometry as MultiLineString;
                        for (int i = 0; i < mls.Count; i++)
                            ui_partRingCB.Items.Add (i + 1);
                    }
                    else
                    {
                        var mpg = _currentGeometry as MultiPolygon;
                        for (int i = 0; i < mpg.Count; i++)
                            ui_partRingCB.Items.Add (i + 1);
                    }
                }
            }
        }

        public void Clear ()
        {
            _aranGraphics.SafeDeleteGraphic (_geomGraphicsHandle);
            _aranGraphics.SafeDeleteGraphic (_selPointGraphicsHandle);
            _aranGraphics.Refresh ();
            _mapTool.MouseUp = null;
            Globals.Environment.Graphics.SetMapTool (null);
        }

        
        private void Back_Click (object sender, EventArgs e)
        {
            if (ui_dgv.Rows.Count > 0)
            {
                var dr = MessageBox.Show ("Save Geometry Changes?", "Geometry Creator",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
                if (dr == DialogResult.Cancel)
                    return;

                if (dr == DialogResult.Yes)
                {
                    if (OKClicked != null)
                        OKClicked (this, e);

                    return;
                }
            }
            if (BackClicked != null)
                BackClicked (this, e);
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (OKClicked != null)
                OKClicked (this, e);
        }

        private void ByClick_CheckedChanged (object sender, EventArgs e)
        {
            Globals.Environment.Graphics.SetMapTool (ui_byClickChB.Checked ? _mapTool : null);
        }

        private void MapTool_MouseUp (object sender, ToolMouseUpEventArgs e)
        {
            if (_currMultiPoint == null)
                return;

			var curIndex = -1;

			if (ui_dgv.CurrentCell != null)
			{
				curIndex = ui_dgv.CurrentCell.RowIndex;
				curIndex++;
			}

			if (curIndex == -1)
				_currMultiPoint.Add (e.GeoPoint);
			else
				_currMultiPoint.Insert (curIndex, e.GeoPoint);

            AddPoint (e.GeoPoint, ref curIndex);
			ui_dgv.CurrentCell = ui_dgv.Rows [curIndex].Cells [ui_colX.Index];

            DrawGeometry ();
        }

        private void DrawGeometry ()
        {
            _aranGraphics.SafeDeleteGraphic (_geomGraphicsHandle);

            var geom = _currentGeometry;

            if (_aranGraphics.ViewProjection != null &&
                _aranGraphics.ViewProjection.Name != _aranGraphics.WGS84SR.Name)
            {
                geom = _geomOper.GeoTransformations (geom, _aranGraphics.WGS84SR, _aranGraphics.ViewProjection);
            }

            if (geom != null)
            {
                if (geom.Type == GeometryType.MultiPolygon)
                    _geomGraphicsHandle = _aranGraphics.DrawMultiPolygon (geom as MultiPolygon, _fillSymbol);
                else
                    _geomGraphicsHandle = _aranGraphics.DrawMultiLineString (geom as MultiLineString, _fillSymbol.Outline);
            }
        }

        private void DrawSelectedPoint (Aran.Geometries.Point point = null)
        {
            _aranGraphics.SafeDeleteGraphic (_selPointGraphicsHandle);

            if (point == null)
            {
				if (ui_dgv.CurrentRow == null || _currMultiPoint == null)
					return;

				if (ui_dgv.CurrentCell != null)
					point = _currMultiPoint [ui_dgv.CurrentCell.RowIndex];
            }

            if (point != null)
            {
                if (_aranGraphics.ViewProjection != null &&
                    _aranGraphics.ViewProjection.Name != _aranGraphics.WGS84SR.Name)
                {
                    point = _geomOper.GeoTransformations (point, _aranGraphics.WGS84SR, _aranGraphics.ViewProjection) as Geometries.Point;
                }
            }

            if (point != null)
                _selPointGraphicsHandle = _aranGraphics.DrawPoint (point, 255);
        }

        private void AddPoint (Aran.Geometries.Point point, ref int index)
        {
			if (index == -1)
				index = ui_dgv.Rows.Add ();
			else
				ui_dgv.Rows.Insert (index, 1);

            var row = ui_dgv.Rows [index];

            string [] sa = new string [2];

            int round = Aran.Queries.Common.Settings.Instance.CoordinateFormatAccuracy;

            if (Aran.Queries.Common.Settings.Instance.CoordinateFormatIsDMS)
            {
                sa [0] = Globals.DD2DMS (point.X, true, round);
                sa [1] = Globals.DD2DMS (point.Y, false, round);
            }
            else
            {
                string rt = new string ('#', round);

                sa [0] = point.X.ToString ("#." + rt);
                sa [1] = point.Y.ToString ("#." + rt);
            }

            row.Cells [ui_colX.Index].Value = sa [0];
            row.Cells [ui_colY.Index].Value = sa [1];
        }

        private bool IsLine
        {
            get { return (_currentGeometry.Type == GeometryType.MultiLineString); }
        }

        private void AddNewPartRing_Click (object sender, EventArgs e)
        {
            if (IsLine)
            {
                var mls = _currentGeometry as MultiLineString;
                var ls = new LineString ();
                mls.Add (ls);
            }
            else
            {
                var mpg = _currentGeometry as MultiPolygon;
                var pg = new Polygon ();
                mpg.Add (pg);
            }

            ui_partRingCB.Items.Add (ui_partRingCB.Items.Count + 1);
            ui_partRingCB.SelectedIndex = ui_partRingCB.Items.Count - 1;
        }

		private void RemoveCurrPartRing_Click (object sender, EventArgs e)
		{
			int index = ui_partRingCB.SelectedIndex;

			if (index < 0)
				return;

			if (IsLine)
			{
				var mls = _currentGeometry as MultiLineString;
				mls.Remove (index);
			}
			else
			{
				var mpg = _currentGeometry as MultiPolygon;
				mpg.Remove (index);
			}

			ui_partRingCB.SelectedIndex = index - 1;
			ui_partRingCB.Items.RemoveAt (index);
		}

        private void PartRing_SelectedIndexChanged (object sender, EventArgs e)
        {
            int index = ui_partRingCB.SelectedIndex;

            ui_dgv.Rows.Clear();

            if (index < 0 || index >ui_partRingCB.Items.Count-1) return;

            if (IsLine)
            {
                var mls = _currentGeometry as MultiLineString;
                _currMultiPoint = mls [index];
            }
            else
            {
                var mpg = _currentGeometry as MultiPolygon;
                var pg = mpg [index];
                _currMultiPoint = pg.ExteriorRing;
            }
            
			for (int i = 0; i < _currMultiPoint.Count; i++)
			{
				var x = -1;
				AddPoint (_currMultiPoint [i], ref x);
			}

            DrawGeometry ();

			if (_currRowIndex >= 0 && _currRowIndex < ui_dgv.Rows.Count)
				ui_dgv.CurrentCell = ui_dgv.Rows [_currRowIndex].Cells [0];
			
			DrawSelectedPoint ();
        }

        private void DGV_CurrentCellChanged (object sender, EventArgs e)
        {
            DrawSelectedPoint ();
        }

        private void RemoveSelCoord_Click (object sender, EventArgs e)
        {
			if (ui_dgv.CurrentCell == null)
                return;

			var mp = GetCurrPoints ();
			mp.Remove (ui_dgv.CurrentCell.RowIndex);

            PartRing_SelectedIndexChanged (null, null);
        }

        private void EditCoord_Click (object sender, EventArgs e)
        {
			if (ui_dgv.CurrentCell == null)
				return;

			var curIndex = ui_dgv.CurrentCell.RowIndex;
			var point = _currMultiPoint [curIndex];
            var tmpPoint = point.Clone () as Geometries.Point;
            var ecf = new EditCoordinateForm ();
            ecf.IsDD = !Aran.Queries.Common.Settings.Instance.CoordinateFormatIsDMS;
            ecf.Accuracy = Aran.Queries.Common.Settings.Instance.CoordinateFormatAccuracy;
            ecf.SetPoint (tmpPoint);

            if (ecf.ShowDialog (OwnerForm) == DialogResult.OK)
            {
				var pt = ecf.GetPoint ();
				_currMultiPoint [curIndex].Assign (pt);
				_currRowIndex = curIndex;
				PartRing_SelectedIndexChanged (null, null);
            }
        }

        private MultiPoint GetCurrPoints ()
        {
            var partRingIndex = ui_partRingCB.SelectedIndex;

            if (IsLine)
            {
                var mls = _currentGeometry as MultiLineString;
                return mls [partRingIndex];
            }
            else
            {
                var mpg = _currentGeometry as MultiPolygon;
                return mpg [partRingIndex].ExteriorRing;
            }
        }

        private int FindPoint (MultiPoint mp, Geometries.Point point)
        {
            for (int i = 0; i < mp.Count; i++)
            {
                if (mp [i] == point)
                    return i;
            }
            return -1;
        }

		private void ImportFrom_Click (object sender, EventArgs e)
		{
			ui_importContextMenu.Show ((sender as Control),
				new System.Drawing.Point (0, 0),  
				ToolStripDropDownDirection.AboveRight);
		}

		private void FromDeltaPro_Click (object sender, EventArgs e)
		{
			var dataTable = DeltaProLoaderForm.LoadDataTable ();

			if (dataTable == null)
				return;

			var dplf = new DeltaProLoaderForm ();
			dplf.SetData (dataTable);
			dplf.ShowDialog (this);

			var dataRow = dplf.SelectedRow;
			if (dataRow == null)
				return;

			var geom = dataRow ["Shape"] as Geometry;
			if (geom != null)
				GeomValue = geom;
		}
	}
}
