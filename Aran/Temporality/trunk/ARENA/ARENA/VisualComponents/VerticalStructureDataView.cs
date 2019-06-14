using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using PDM;
using ESRI.ArcGIS.Geometry;

namespace ARENA
{
    public partial class VerticalStructureDataView : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public VerticalStructureDataView()
        {
            InitializeComponent();
        }

        public void LoadData(bool focusOnSelectedRow)
        {
           System.Drawing.Point vs_CellAdr = verticalStructureGridView.CurrentCellAddress;
           System.Drawing.Point vsp_CellAdr = verticalStructurePartGridView.CurrentCellAddress;

            verticalStructureGridView.DataSource = null;
            verticalStructurePartGridView.DataSource = null;


            DataTable VertStrucTbl = ARENA_DataReader.Table_GetVerticalStructureList();
            DataTable PartsTbl = ARENA_DataReader.Table_GetVerticalStructurePartList();

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(VertStrucTbl);
            ds.Tables.Add(PartsTbl);


            DataRelation relation_VertStruct_part = new DataRelation("VerticalStructure_VerticalStructurePart", VertStrucTbl.Columns["FeatureGUID"], PartsTbl.Columns["VerticalStructure_ID"]);
            ds.Relations.Add(relation_VertStruct_part);


            BindingSource VerticalStructure_BindingSource = new BindingSource();
            BindingSource VerticalStructurePart_BindingSource = new BindingSource();

            VerticalStructure_BindingSource.DataSource = ds;
            VerticalStructure_BindingSource.DataMember = ds.Tables[0].TableName;


            VerticalStructurePart_BindingSource.DataSource = VerticalStructure_BindingSource;
            VerticalStructurePart_BindingSource.DataMember = "VerticalStructure_VerticalStructurePart";


            verticalStructureGridView.DataSource = VerticalStructure_BindingSource;
            verticalStructurePartGridView.DataSource = VerticalStructurePart_BindingSource;

            ARENA_DataReader.hideColumns(verticalStructureGridView);
            ARENA_DataReader.hideColumns(verticalStructurePartGridView);

            if (focusOnSelectedRow)
            {
                if ((vs_CellAdr.Y >= 0) && (vs_CellAdr.X >= 0)) verticalStructureGridView.CurrentCell = verticalStructureGridView.Rows[vs_CellAdr.Y].Cells[vs_CellAdr.X];
                if ((vsp_CellAdr.Y >= 0) && (vsp_CellAdr.X >= 0)) verticalStructurePartGridView.CurrentCell = verticalStructurePartGridView.Rows[vsp_CellAdr.Y].Cells[vsp_CellAdr.X];
            }
        }

        private PDMObject GetSelectedVerticalStructure()
        {
            PDMObject VS = null;
            if (this.Tag != null)
            {
                string routeId = verticalStructureGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();
                List<PDMObject> lst = this.Tag as List<PDMObject>;

                VS = (from element in lst where (element != null) && (element is VerticalStructure) && (element.ID.CompareTo(routeId) == 0) select element).FirstOrDefault();

            }

            return VS;
        }

        private PDMObject GetSelectedVerticalStructurePart()
        {
            VerticalStructure VertStruct = GetSelectedVerticalStructure() as VerticalStructure;
            VerticalStructurePart VertStructPart = null;
            if (VertStruct != null)
            {
                string partId = verticalStructurePartGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();

                if ((VertStruct != null) && (VertStruct.Parts != null))
                {
                    VertStructPart = (from element in VertStruct.Parts where (element != null) && (element is VerticalStructurePart) && (element.ID.CompareTo(partId) == 0) select element).FirstOrDefault() as VerticalStructurePart;
                }

            }

            return VertStructPart;
        }

        private void verticalStructurePartGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void verticalStructurePartGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void verticalStructurePartGridView_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void verticalStructurePartGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void verticalStructurePartGridView_MouseClick(object sender, MouseEventArgs e)
        {
            PDMObject sellObj = GetSelectedVerticalStructurePart();
            textBox1.Clear();
            try
            {
                if (sellObj != null)
                {
                    textBox1.Text = (sellObj as VerticalStructurePart).Designator+" coordinates" + (char)13 + (char)10;
                    if (sellObj.Geo == null) sellObj.RebuildGeo();
                    switch (sellObj.Geo.GeometryType)
                    {
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint):

                            IPoint pntGeo = sellObj.Geo as IPoint;
                            textBox1.Text = textBox1.Text+ "   X= " + pntGeo.X.ToString() + " Y= " + pntGeo.Y.ToString();
                            break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline):
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine):

                            PolylineClass Ln = sellObj.Geo as PolylineClass;
                            textBox1.Text = textBox1.Text + "   X1= " + Ln.FromPoint.X.ToString() + " Y1= " + Ln.FromPoint.Y.ToString() + (char)13 + (char)10;
                            textBox1.Text = textBox1.Text + "   X2= " + Ln.ToPoint.X.ToString() + " Y2= " + Ln.ToPoint.Y.ToString() + (char)13 + (char)10;

                            break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon):

                            IPointCollection polyObs = (sellObj.Geo as IPointCollection);
                            for (int i = 0; i <= polyObs.PointCount - 1; i++)
                            {
                                textBox1.Text = textBox1.Text + "   X" + (i + 1).ToString() + "= " + polyObs.get_Point(i).X.ToString() + "Y" + (i + 1).ToString() + "= " + polyObs.get_Point(i).Y.ToString() + (char)13 + (char)10;

                            }

                                break;
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedVerticalStructure();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);
            LoadData(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedVerticalStructurePart();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);
            LoadData(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedVerticalStructure();
            if (obj != null)
            {
                obj.DeleteAll(_AIRTRACK_TableDic);
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                lst.RemoveAll(item => item is VerticalStructure);
                LoadData(false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }




    }
}
