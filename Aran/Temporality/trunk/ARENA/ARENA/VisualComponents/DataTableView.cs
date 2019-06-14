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

namespace ARENA
{
    public partial class DataTableViewControl : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public DataTableViewControl()
        {
            InitializeComponent();
        }

        public void LoadData(DataTable Dtbl)
        {

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(Dtbl);

            BindingSource _BindingSource = new BindingSource();
            _BindingSource.DataSource = ds;
            _BindingSource.DataMember = ds.Tables[0].TableName;

            _dataGridView.DataSource = _BindingSource;
            ARENA_DataReader.hideColumns(_dataGridView);

        }


        private void button22_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedWaypoint();

            Point CellAdr =_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            _dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void StartEditingProcedure(PDMObject obj)
        {
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;
            frm.ShowDialog();

            ITable tbl = _AIRTRACK_TableDic[obj.GetType()];
            obj.UpdateDB(tbl);

            Application.DoEvents();

            LoadData(ARENA_DataReader.Table_GetWaypointList());
        }

        private PDMObject GetSelectedWaypoint()
        {
            PDMObject wyp = null;
            List<PDMObject> lst = this.Tag as List<PDMObject>;

            string WypId = _dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();

            wyp = (from element in lst where (element != null) && (element is WayPoint) && (element.ID.CompareTo(WypId) == 0) select element).FirstOrDefault();

            return wyp;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedWaypoint();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;
            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);

            LoadData(ARENA_DataReader.Table_GetWaypointList());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedWaypoint();
            
            if (obj != null)
            {
                obj.DeleteAll(_AIRTRACK_TableDic);
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                lst.RemoveAll(item => item is WayPoint);
                DataTable WaypointTbl = ARENA_DataReader.Table_GetWaypointList();
                LoadData(WaypointTbl);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            WayPoint obj = new WayPoint();
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                obj.RebuildGeo();
                //Point CellAdr =rwy_dataGridView.CurrentCellAddress;
                ITable tbl = _AIRTRACK_TableDic[obj.GetType()];

                obj.ID = Guid.NewGuid().ToString();

                obj.StoreToDB(tbl);
                DataTable WaypointTbl = ARENA_DataReader.Table_GetWaypointList();
                LoadData(WaypointTbl);

                (this.Tag as List<PDMObject>).Add(obj);

                //rwy_dataGridView.CurrentCell = rwy_dataGridView.Rows[CellAdr.Y].Cells[CellAdr.X];
                _dataGridView.CurrentCell = _dataGridView.Rows[_dataGridView.RowCount - 1].Cells["designator"];
            }
        }

    }
}
