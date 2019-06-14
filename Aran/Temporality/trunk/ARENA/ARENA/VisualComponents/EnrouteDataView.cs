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
    public partial class EnrouteDataView : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public EnrouteDataView()
        {
            InitializeComponent();
        }

        public void LoadData(bool focusOnSelectedRow)
        {
            Point rte_CellAdr = route_dataGridView.CurrentCellAddress;
            Point rsg_CellAdr = routeSegment_dataGridView1.CurrentCellAddress;

            route_dataGridView.DataSource = null;
            routeSegment_dataGridView1.DataSource = null;

            //ARENA_DataReader.PathToPDM_DB = @"C:\PDM\TEMP_ALL_PDM_OBJ\pdm.mdb";

            DataTable RouteTbl = ARENA_DataReader.Table_GetRouteList();
            DataTable RouteSegmentTbl = ARENA_DataReader.Table_GetRouteSegmentList();
            DataTable SegmentPntTbl = ARENA_DataReader.Table_GetRoutesStartEndPointsList();

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(RouteTbl);
            ds.Tables.Add(RouteSegmentTbl);
            ds.Tables.Add(SegmentPntTbl);


            DataRelation relation_RTE_RSG = new DataRelation("Route_RouteSegment", RouteTbl.Columns["FeatureGUID"], RouteSegmentTbl.Columns["ID_Enroute"]);
            ds.Relations.Add(relation_RTE_RSG);

            DataRelation relation_RSG_START_PNT = new DataRelation("RouteSegment_StartPNT", RouteSegmentTbl.Columns["FeatureGUID"], SegmentPntTbl.Columns["Route_LEG_ID"]);
            ds.Relations.Add(relation_RSG_START_PNT);


            BindingSource route_BindingSource = new BindingSource();
            BindingSource routeSegment_BindingSource = new BindingSource();
            BindingSource routeStartPoint_BindingSource = new BindingSource();

            route_BindingSource.DataSource = ds;
            route_BindingSource.DataMember = ds.Tables[0].TableName;


            routeSegment_BindingSource.DataSource = route_BindingSource;
            routeSegment_BindingSource.DataMember = "Route_RouteSegment";


            routeStartPoint_BindingSource.DataSource = routeSegment_BindingSource;
            routeStartPoint_BindingSource.DataMember = "RouteSegment_StartPNT";


            route_dataGridView.DataSource = route_BindingSource;
            routeSegment_dataGridView1.DataSource = routeSegment_BindingSource;
            startPoint_dataGridView.DataSource = routeStartPoint_BindingSource;

            ARENA_DataReader.hideColumns(route_dataGridView);
            ARENA_DataReader.hideColumns(routeSegment_dataGridView1);
            ARENA_DataReader.showColumns(startPoint_dataGridView, "PointUse,PointChoice,pointChoiceID".Split(','));

            if (focusOnSelectedRow)
            {
                if ((rte_CellAdr.Y >= 0) && (rte_CellAdr.X >= 0)) route_dataGridView.CurrentCell = route_dataGridView.Rows[rte_CellAdr.Y].Cells[rte_CellAdr.X];
                if ((rsg_CellAdr.Y >= 0) && (rsg_CellAdr.X >= 0)) routeSegment_dataGridView1.CurrentCell = routeSegment_dataGridView1.Rows[rsg_CellAdr.Y].Cells[rsg_CellAdr.X];
            }
        }

        private void button_DeleteADHP_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedRoute();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);
            LoadData(false);
        }

        private PDMObject GetSelectedRoute()
        {
            PDMObject arp = null;
            if (this.Tag != null)
            {
                string routeId = route_dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();
                List<PDMObject> lst = this.Tag as List<PDMObject>;

                arp = (from element in lst where (element != null) && (element is Enroute) && (element.ID.CompareTo(routeId) == 0) select element).FirstOrDefault();

            }

            return arp;
        }

        private void route_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_editADHP_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(@"Delete all records?", "Delte all", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;


            List<PDMObject> lst = this.Tag as List<PDMObject>;
            var procList = (from element in lst where (element != null) && (element is Enroute) select element).ToList();

            foreach (var sellObj2 in procList)
            {
                if (sellObj2 != null)
                {

                    #region

                    foreach (var item in ((Enroute)sellObj2).Routes)
                    {
                        if (item.StartPoint != null) ARENA.Util.LogicUtil.RemoveFeature("SegmentPoint", "Route_LEG_ID", item.ID);
                        if (item.EndPoint != null) ARENA.Util.LogicUtil.RemoveFeature("SegmentPoint", "Route_LEG_ID", item.ID);
                    }

                    #endregion

                    ARENA.Util.LogicUtil.RemoveFeature(sellObj2);
                }
            }

            lst.RemoveAll(item => (item is Enroute));
            LoadData(false);


        }

        private void button_AddADHP_Click(object sender, EventArgs e)
        {

        }

    }
}
