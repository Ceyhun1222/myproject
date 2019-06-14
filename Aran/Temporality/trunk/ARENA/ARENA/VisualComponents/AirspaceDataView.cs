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
    public partial class AirspaceDataView : UserControl
    {

        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public AirspaceDataView()
        {
            InitializeComponent();
        }



      public void LoadData(bool focusOnSelectedRow)
        {
            Point arsp_CellAdr = airspace_dataGridView.CurrentCellAddress;
            Point arspsVol_CellAdr = airspaceVolume_dataGridView1.CurrentCellAddress;

            airspace_dataGridView.DataSource = null;
            airspaceVolume_dataGridView1.DataSource = null;

            //ARENA_DataReader.PathToPDM_DB = @"C:\PDM\TEMP_ALL_PDM_OBJ\pdm.mdb";

            DataTable ArspcTbl = ARENA_DataReader.Table_GetAirspaceList();
            DataTable AirspcVolTbl = ARENA_DataReader.Table_GetAirspaceVolumeList();

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(ArspcTbl);
            ds.Tables.Add(AirspcVolTbl);


            DataRelation relation_ARSP_VOL = new DataRelation("Airspace_AirspaceVolume", ArspcTbl.Columns["FeatureGUID"], AirspcVolTbl.Columns["AirspaceID"]);
            ds.Relations.Add(relation_ARSP_VOL);


            BindingSource airspace_BindingSource = new BindingSource();
            BindingSource airspaceVolume_BindingSource = new BindingSource();

            airspace_BindingSource.DataSource = ds;
            airspace_BindingSource.DataMember = ds.Tables[0].TableName;


            airspaceVolume_BindingSource.DataSource = airspace_BindingSource;
            airspaceVolume_BindingSource.DataMember = "Airspace_AirspaceVolume";


            airspace_dataGridView.DataSource = airspace_BindingSource;
            airspaceVolume_dataGridView1.DataSource = airspaceVolume_BindingSource;

            ARENA_DataReader.hideColumns(airspace_dataGridView);
            ARENA_DataReader.hideColumns(airspaceVolume_dataGridView1);

            if (focusOnSelectedRow)
            {
                if ((arsp_CellAdr.Y >= 0) && (arsp_CellAdr.X >= 0)) airspace_dataGridView.CurrentCell = airspace_dataGridView.Rows[arsp_CellAdr.Y].Cells[arsp_CellAdr.X];
                if ((arspsVol_CellAdr.Y >= 0) && (arspsVol_CellAdr.X >= 0)) airspaceVolume_dataGridView1.CurrentCell = airspaceVolume_dataGridView1.Rows[arspsVol_CellAdr.Y].Cells[arspsVol_CellAdr.X];
            }
        }



      private PDMObject GetSelectedAirspace()
      {
          PDMObject ARSPS = null;
          List<PDMObject> lst = this.Tag as List<PDMObject>;

          string arspsId = airspace_dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();

          ARSPS = (from element in lst where (element != null) && (element is Airspace) && (element.ID.CompareTo(arspsId) == 0) select element).FirstOrDefault();

          return ARSPS;
      }

        private void button22_Click(object sender, EventArgs e)
        {
            //PDMObject obj = GetSelectedAirspace();
            //if (obj.DeleteObject(TableDictionary) == 0) return;
            //List<PDMObject> lst = this.Tag as List<PDMObject>;
            //lst.Remove(obj);

            //LoadData(ARENA_DataReader.GetAirspaceList());
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedAirspace();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;
            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);

            LoadData(true);
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedAirspace();
            Point CellAdr = airspace_dataGridView.CurrentCellAddress;


            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            if (CellAdr.Y < 0) CellAdr.Y = 0;
            airspace_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void StartEditingProcedure(PDMObject obj)
        {
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;
            frm.ShowDialog();

            ITable tbl = _AIRTRACK_TableDic[obj.GetType()];
            obj.UpdateDB(tbl);

            Application.DoEvents();

            LoadData(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedAirspace();
            if (obj != null)
            {
                obj.DeleteAll(_AIRTRACK_TableDic);
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                lst.RemoveAll(item => item is Airspace);
                LoadData(false);
            }
        }
    }
}
