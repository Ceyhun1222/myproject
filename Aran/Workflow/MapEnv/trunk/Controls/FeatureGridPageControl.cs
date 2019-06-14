using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Queries.Common;
using ESRI.ArcGIS.Carto;
using EG = ESRI.ArcGIS.Geometry;
using MapEnv.Layers;
using System.Reflection;

namespace MapEnv
{
    public partial class FeatureGridPageControl : UserControl, IAttributePageControl
    {
		private AimFeatureLayer _myFeatureLayer;
		private string _text;
		private bool _reactColSelect;


        public event ZoomToEventHandler ZoomToClicked;
        public event FeatureEventHandler FeatureOpened;


        public FeatureGridPageControl ()
        {
            InitializeComponent ();
			_text = string.Empty;
			_reactColSelect = true;

            var pi = ui_dgv.GetType().GetProperty("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            pi.SetValue(ui_dgv, true, null);

            ui_dgv.SortCompare += UIUtilities.DGV_SortCompare;
        }

        public void OpenLayer (ILayer layer)
        {
            _text = layer.Name;

			ui_dgv.Rows.Clear ();
			ui_selectAllChB.Visible = false;

            _myFeatureLayer = layer as AimFeatureLayer;
            
            RefreshGrid();
        }

        public void OnClose ()
        {
        }

        public override string Text
        {
            get { return _text; }
        }

        public ILayer Layer
        {
            get { return _myFeatureLayer; }
        }



        private void RefreshGrid()
        {
            if (_myFeatureLayer == null)
                return;

            UIUtilities.FillColumns(
                UIMetadata.Instance.GetClassInfo((int)_myFeatureLayer.FeatureType), ui_dgv);

            foreach (var feat in _myFeatureLayer.AimFeatures)
                UIUtilities.SetRow(ui_dgv, feat);

            ui_selectAllChB.Visible = true;
            ui_selectAllChB.Checked = _myFeatureLayer.AllAimFeatureSelected;

            InsertSelectionColumn();

            ui_featCountLab.Text = "Feature Count: " + ui_dgv.Rows.Count;
        }

        private void InsertSelectionColumn ()
        {
            var col = new DataGridViewCheckBoxColumn ();
            col.Name = "col_selection";
            col.HeaderText = "";
            
            ui_dgv.Columns.Insert (0, col);

			if (_myFeatureLayer != null)
			{
				var selGuids = _myFeatureLayer.GetSelectedGuids ();

				foreach (DataGridViewRow row in ui_dgv.Rows)
				{
					var feat = row.Tag as Feature;
					var isSelected = _myFeatureLayer.AllAimFeatureSelected;
					if (!isSelected)
						isSelected = selGuids.Contains (feat.Identifier);
					row.Cells [col.Index].Value = isSelected;
				}
			}

			col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void DGV_CurrentCellChanged (object sender, EventArgs e)
        {
            if (ui_dgv.CurrentCell == null)
                return;

            var dgvRow = ui_dgv.Rows [ui_dgv.CurrentCell.RowIndex];

			if (_myFeatureLayer != null)
				AimFeatureLayerCurrentCellChanged (dgvRow);
        }

		private void AimFeatureLayerCurrentCellChanged (DataGridViewRow dgvRow)
		{
			if (dgvRow.ContextMenuStrip == null)
			{
				ContextMenuStrip rowMenuStrip = new ContextMenuStrip ();
				dgvRow.ContextMenuStrip = rowMenuStrip;

				ToolStripMenuItem copyMenuItem = new ToolStripMenuItem ();
				copyMenuItem.Text = "Copy";
				copyMenuItem.Click += new EventHandler (CopyMenuItem_Click);
				rowMenuStrip.Items.Add (copyMenuItem);

				var shapeInfoList = _myFeatureLayer.ShapeInfoList;
				
				if (shapeInfoList.Count == 0)
					return;

				Feature feature = dgvRow.Tag as Feature;
				var dict = new Dictionary<TableShapeInfo, List<EG.IGeometry>> ();

				for (int i = 0; i < shapeInfoList.Count; i++)
				{
					var shapeInfo = shapeInfoList [i];
					var geomList = _myFeatureLayer.GetShapes (feature, shapeInfo);

					if (geomList == null || geomList.Count == 0)
						continue;

					dict.Add (shapeInfo, geomList);
				}

				if (dict.Count == 0)
					return;

				foreach (var shapeInfo in dict.Keys)
				{
					var geomList = dict [shapeInfo];
					var menuText = "Zoom To";

					if (dict.Count > 1)
						menuText += " - " + Globals.GeoPropertyNameCaption (shapeInfo.GeoProperty);

					for (int i = 0; i < geomList.Count; i++)
					{
						var geom = geomList [i];

						if (geomList.Count > 1)
							menuText += " - " + (i + 1);

						var zoomToMenuItem = new ToolStripMenuItem ();
						zoomToMenuItem.Image = Globals.ZoomToImage;
						zoomToMenuItem.Text = menuText;
						zoomToMenuItem.Click += ZoomTo_Click;
						zoomToMenuItem.Tag = geomList [i];
						rowMenuStrip.Items.Add (zoomToMenuItem);
					}
				}
			}
		}

        private void CopyMenuItem_Click (object sender, EventArgs e)
        {
            DataGridViewCell dgvCell = ui_dgv.CurrentCell;
            if (dgvCell != null && dgvCell.Value != null)
                Clipboard.SetText (dgvCell.Value.ToString ());
        }

		private void ZoomTo_Click (object sender, EventArgs e)
		{
			if (ZoomToClicked == null)
				return;

			ToolStripItem tsi = sender as ToolStripItem;
			ESRI.ArcGIS.Geometry.IGeometry esriGeom = null;

			if (tsi.Tag is ShapePair)
			{
				esriGeom = (tsi.Tag as ShapePair).Prj;
			}
			else if (tsi.Tag is ESRI.ArcGIS.Geometry.IGeometry)
			{
				esriGeom = tsi.Tag as ESRI.ArcGIS.Geometry.IGeometry;

				if (_myFeatureLayer.DbSpatialReference.Name != 
					Globals.MainForm.Map.SpatialReference.Name)
				{
					esriGeom.Project (Globals.MainForm.Map.SpatialReference);
				}
			}

			if (esriGeom != null)
				ZoomToClicked (this, new ZoomToEventArgs (esriGeom));
		}

        private void DGV_MouseUp (object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView dgv = sender as DataGridView;
            DataGridView.HitTestInfo hti = dgv.HitTest (e.X, e.Y);

            if (hti.ColumnIndex == -1 && hti.RowIndex == -1)
            {
				Aran.Aim.FeatureType featType = 0;

				if (_myFeatureLayer != null)
					featType = _myFeatureLayer.FeatureType;

                UIUtilities.ShowFieldsContextMenu (dgv,
					UIMetadata.Instance.GetClassInfo ((int) featType),
                    DataGridView_Refresh);
            }
        }

        private void uiEvents_openFeatureTSMB_Click (object sender, EventArgs e)
        {
            if (FeatureOpened == null || ui_dgv.CurrentCell == null)
                return;

            DataGridViewRow row = ui_dgv.Rows [ui_dgv.CurrentCell.RowIndex];
            Feature feature = row.Tag as Feature;

            if (feature == null)
                return;

            FeatureOpened (this, new FeatureEventArgs (feature));
        }

        private void DataGridView_Refresh (object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void toolStripButton1_Click (object sender, EventArgs e)
        {
            if (ui_dgv.CurrentCell == null)
                return;

            var row = ui_dgv.Rows [ui_dgv.CurrentCell.RowIndex];
            Feature feature = row.Tag as Feature;

            if (feature == null)
                return;

            var rofv = new Aran.Aim.FeatureInfo.ROFeatureViewer ();
            rofv.SetOwner (Globals.MainForm);
            rofv.GettedFeature += Globals.FeatureViewer_GetFeature;
            List<Feature> list = new List<Feature> ();
            list.Add (feature);
            rofv.ShowFeaturesForm (list);
        }

        private void DGV_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && 
				ui_dgv.Columns [e.ColumnIndex].Name == "col_selection" &&
				_reactColSelect)
            {
                var row = ui_dgv.Rows [e.RowIndex];
                var cbCell = row.Cells [e.ColumnIndex] as DataGridViewCheckBoxCell;
                
                bool isChecked = false;
                if (cbCell.Value != null)
                {
                    isChecked = (bool) cbCell.Value;
                    isChecked = !isChecked;
                }
                cbCell.Value = isChecked;

                OnFeatureSelectedChanged (row.Tag as Feature, isChecked);
            }
        }

        private void OnFeatureSelectedChanged (Feature feature, bool isSelected)
        {
			if (_myFeatureLayer != null)
			{
				_myFeatureLayer.SelectAimFature (feature.Identifier, isSelected);
				Globals.MainForm.ActiveView.Refresh ();
			}
        }

		private void SelectAllFeature_CheckedChanged (object sender, EventArgs e)
		{
			if (_myFeatureLayer != null)
			{
				_myFeatureLayer.SelectAimFature (Guid.Empty, ui_selectAllChB.Checked);

				_reactColSelect = false;
				var col = ui_dgv.Columns ["col_selection"];
				if (col != null)
				{
					col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

					foreach (DataGridViewRow row in ui_dgv.Rows)
					{
						row.Cells [col.Index].Value = ui_selectAllChB.Checked;
					}
					_reactColSelect = true;
					col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

					Globals.MainForm.ActiveView.Refresh ();
				}
			}

			ui_selectAllChB.Text = (!ui_selectAllChB.Checked ? "Select All" : "Deselect All");
		}
    }
}
