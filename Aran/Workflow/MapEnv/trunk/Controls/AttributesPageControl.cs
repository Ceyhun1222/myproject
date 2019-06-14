using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace MapEnv
{
    public partial class AttributesPageControl : UserControl, IAttributePageControl
    {
        private IFeatureLayer _featureLayer;
        private string _geomTypeText;
        private string _text;

        public event ZoomToEventHandler ZoomToClicked;

        public AttributesPageControl ()
        {
            InitializeComponent ();

            ui_zoomToTSMI.Image = Globals.ZoomToImage;

            Globals.SetDGV_DoubleBuffered(ui_dgv);
        }

        public void OpenLayer (ILayer layer)
        {
            IFeatureLayer featLayer = layer as IFeatureLayer;

            _text = layer.Name;

            _featureLayer = featLayer;
            ui_dgv.Rows.Clear ();
            ui_dgv.Columns.Clear ();

            IFeatureClass featClass = featLayer.FeatureClass;
            ui_dgv.Tag = featClass;

            #region Set GeomTypeText

            IField geomField = featClass.Fields.get_Field (featClass.FindField (featClass.ShapeFieldName));
            string s = geomField.GeometryDef.GeometryType.ToString ();
            s = s.Substring ("esriGeometry".Length);

            if (geomField.GeometryDef.HasZ)
                s += "Z";
            if (geomField.GeometryDef.HasM)
                s += "M";

            _geomTypeText = s;
            ui_geomTypeLabel.Text = "Geometry Type: " + _geomTypeText;

            #endregion

            FillColumns (featClass);

            IFeatureCursor featCursor = featClass.Search (null, false);
            IFeature feat;

            while ((feat = featCursor.NextFeature ()) != null)
            {
                var row = new DataGridViewRow ();
                row.Tag = feat;

                foreach (DataGridViewColumn dgvCol in ui_dgv.Columns)
                {
                    object value = feat.get_Value ((int) dgvCol.Tag);
                    DataGridViewCell dgvCell = dgvCol.CellTemplate.Clone () as DataGridViewCell;

                    if (value is IGeometry)
                        value = GetShapeText (feat.Shape, featClass.ShapeType);

                    dgvCell.Value = value;
                    dgvCell.ContextMenuStrip = ui_contextMSdgv;

                    row.Cells.Add (dgvCell);
                }
                ui_dgv.Rows.Add (row);
            }

            ui_rowCountLabel.Text = string.Format("Count:  {0}", ui_dgv.Rows.Count);
        }

        public void OnClose ()
        {
        }

        public override string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ILayer Layer
        {
            get { return _featureLayer; }
        }

        
        private void FillColumns (IFeatureClass featClass)
        {
            for (int i = 0; i < featClass.Fields.FieldCount; i++)
            {
                IField field = featClass.Fields.get_Field (i);
                DataGridViewColumn dgvCol = ToColumn (field);
                dgvCol.SortMode = DataGridViewColumnSortMode.Automatic;
                if (dgvCol != null)
                {
                    dgvCol.Tag = i;
                    ui_dgv.Columns.Add (dgvCol);
                }
            }
        }

        private DataGridViewColumn ToColumn (IField field)
        {
            DataGridViewColumn dgvCol = new DataGridViewColumn ();
            dgvCol.HeaderText = (field.AliasName != null ? field.AliasName : field.Name);
            switch (field.Type)
            {
                case esriFieldType.esriFieldTypeOID:
                case esriFieldType.esriFieldTypeDouble:
                case esriFieldType.esriFieldTypeInteger:
                case esriFieldType.esriFieldTypeSmallInteger:
                    dgvCol.CellTemplate = new DataGridViewTextBoxCell ();
                    dgvCol.DefaultCellStyle = new DataGridViewCellStyle (ui_dgv.DefaultCellStyle);
                    dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                    break;
                case esriFieldType.esriFieldTypeSingle:
                    dgvCol.CellTemplate = new DataGridViewCheckBoxCell ();
                    break;
                default:
                    dgvCol.CellTemplate = new DataGridViewTextBoxCell ();
                    break;

            }
            return dgvCol;
        }

        private string GetShapeText (IGeometry geom, esriGeometryType geomType)
        {
            if (geom == null)
                return null;

            if (geomType != esriGeometryType.esriGeometryPoint)
                return _geomTypeText;

            IPoint point = geom as IPoint;
            string result = string.Format ("{0:F4}; {1:F4}", point.X, point.Y);
            if (!double.IsNaN (point.Z))
                result += string.Format ("; {0:F4}", point.Z);

            return result;
        }

        private void ZoomToTSMI_Click (object sender, EventArgs e)
        {
            if (ui_dgv.CurrentRow == null)
                return;

            var feature = ui_dgv.CurrentRow.Tag as IFeature;

            if (ZoomToClicked != null) {
                var geom = (feature.Shape as ESRI.ArcGIS.esriSystem.IClone).Clone() as IGeometry;
                geom.Project(Globals.MainForm.Map.SpatialReference);
                ZoomToClicked(this, new ZoomToEventArgs(geom));
            }
        }
    }
}
