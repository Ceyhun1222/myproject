using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PDM;
using ESRI.ArcGIS.Geodatabase;
using System.Threading;

namespace ARENA
{
    public partial class AirportDataView : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public AirportDataView()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ARP_splitContainer.Panel2Collapsed = !ARP_splitContainer.Panel2Collapsed;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            rwy_splitContainer.Panel2Collapsed = !rwy_splitContainer.Panel2Collapsed;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            navaids_splitContainer.Panel2Collapsed = !navaids_splitContainer.Panel2Collapsed;
        }

        private void AirportDataView_Load(object sender, EventArgs e)
        {


        }

        private void navaids_dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            firstNavaid_panel.Visible = false;
            secondNavaid_panel.Visible = false;
            thirdNavaid_panel.Visible = false;

            foreach (DataGridViewRow DR in navaids_dataGridView.SelectedRows)
            {
                List<DataTable> NavaidSystemEqp = ARENA_DataReader.Table_GetNavaidEquipment(DR.Cells["ID_NavaidSystem"].Value.ToString());

                if (NavaidSystemEqp.Count > 0)
                {
                    dataGridView1.DataSource = NavaidSystemEqp[0];
                    firstNavaid_panel.Visible = true;
                    label1.Text = NavaidSystemEqp[0].TableName;
                    ARENA_DataReader.hideColumns(dataGridView1);

                    if (NavaidSystemEqp.Count > 1)
                    {
                        dataGridView2.DataSource = NavaidSystemEqp[1];
                        secondNavaid_panel.Visible = true;
                        label2.Text = NavaidSystemEqp[1].TableName;
                        ARENA_DataReader.hideColumns(dataGridView2);

                        if (NavaidSystemEqp.Count > 2)
                        {
                            dataGridView3.DataSource = NavaidSystemEqp[2];
                            thirdNavaid_panel.Visible = true;
                            label3.Text = NavaidSystemEqp[2].TableName;
                            ARENA_DataReader.hideColumns(dataGridView3);
                        }

                    }

                }
               
            }
        }

        public void LoadData(bool focusOnSelectedRow)
        {


            Point arp_CellAdr = arp_dataGridView.CurrentCellAddress;
            Point rwy_CellAdr = rwy_dataGridView.CurrentCellAddress;
            Point rdn_CellAdr = rdn_dataGridView.CurrentCellAddress;
            Point navaids_CellAdr = navaids_dataGridView.CurrentCellAddress;
            
            arp_dataGridView.DataSource = null;
            rwy_dataGridView.DataSource = null;
            rdn_dataGridView.DataSource = null;
            navaids_dataGridView.DataSource = null;

            //ARENA_DataReader.PathToPDM_DB = @"C:\PDM\TEMP_ALL_PDM_OBJ\pdm.mdb";

            DataTable AirportTbl = ARENA_DataReader.Table_GetAirportList();
            DataTable RunwayTbl = ARENA_DataReader.Table_GetRunwayList();
            DataTable RdnTbl = ARENA_DataReader.Table_GetRunwayDirectionList();
            DataTable NavaidSystemTbl = ARENA_DataReader.Table_GetNavaidSystemList("(NavaidSystem.ID_RunwayDirection Is not Null) AND (NavaidSystem.ID_RunwayDirection<>'')");

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(AirportTbl);
            ds.Tables.Add(RunwayTbl);
            ds.Tables.Add(RdnTbl);
            ds.Tables.Add(NavaidSystemTbl);
            

            DataRelation relation_ARP_RWY = new DataRelation("Airport_Runway", AirportTbl.Columns["FeatureGUID"], RunwayTbl.Columns["ID_AirportHeliport"]);
            ds.Relations.Add(relation_ARP_RWY);

            DataRelation relation_RWY_RDN = new DataRelation("Runway_RunwayDirection", RunwayTbl.Columns["FeatureGUID"], RdnTbl.Columns["ID_Runway"]);
            ds.Relations.Add(relation_RWY_RDN);

            try
            {
                DataRelation relation_RDN_Navaid = new DataRelation("RunwayDirection_Navaid", RdnTbl.Columns["FeatureGUID"], NavaidSystemTbl.Columns["ID_RunwayDirection"]);
                ds.Relations.Add(relation_RDN_Navaid);
            }
            catch { }


            BindingSource aerport_rBindingSource = new BindingSource();
            BindingSource rwy_BindingSource = new BindingSource();
            BindingSource rdn_BindingSource = new BindingSource();
            BindingSource navaid_BindingSource = new BindingSource();

            aerport_rBindingSource.DataSource = ds;
            aerport_rBindingSource.DataMember = ds.Tables[0].TableName;


            rwy_BindingSource.DataSource = aerport_rBindingSource;
            rwy_BindingSource.DataMember = "Airport_Runway";

            rdn_BindingSource.DataSource = rwy_BindingSource;
            rdn_BindingSource.DataMember = "Runway_RunwayDirection";

            navaid_BindingSource.DataSource = rdn_BindingSource;
            navaid_BindingSource.DataMember = "RunwayDirection_Navaid";


            arp_dataGridView.DataSource = aerport_rBindingSource;
            rwy_dataGridView.DataSource = rwy_BindingSource;
            rdn_dataGridView.DataSource = rdn_BindingSource;
            navaids_dataGridView.DataSource = navaid_BindingSource;

            ARENA_DataReader.hideColumns(arp_dataGridView);
            ARENA_DataReader.hideColumns(rwy_dataGridView);
            ARENA_DataReader.hideColumns(rdn_dataGridView);
            ARENA_DataReader.hideColumns(navaids_dataGridView);



            if (focusOnSelectedRow)
            {
                if ((arp_CellAdr.Y >= 0) && (arp_CellAdr.X >= 0) && (arp_dataGridView.CurrentCell !=null)) arp_dataGridView.CurrentCell = arp_dataGridView.Rows[arp_CellAdr.Y].Cells[arp_CellAdr.X];
                if ((rwy_CellAdr.Y >= 0) && (rwy_CellAdr.X >= 0) && (rwy_dataGridView.CurrentCell != null)) rwy_dataGridView.CurrentCell = rwy_dataGridView.Rows[rwy_CellAdr.Y].Cells[rwy_CellAdr.X];
                if ((rdn_CellAdr.Y >= 0) && (rdn_CellAdr.X >= 0) && (rdn_dataGridView.CurrentCell != null)) rdn_dataGridView.CurrentCell = rdn_dataGridView.Rows[rdn_CellAdr.Y].Cells[rdn_CellAdr.X];
                if ((navaids_CellAdr.Y >= 0) && (navaids_CellAdr.X >= 0) && (navaids_dataGridView.CurrentCell != null)) navaids_dataGridView.CurrentCell = navaids_dataGridView.Rows[navaids_CellAdr.Y].Cells[navaids_CellAdr.X];
            }
        }

 

        private void button_editADHP_Click(object sender, EventArgs e)
        {

            PDMObject obj = GetSelectedAirport();
            Point CellAdr = arp_dataGridView.CurrentCellAddress;

            
            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            if (CellAdr.Y < 0) CellAdr.Y = 0;
            arp_dataGridView.Rows[CellAdr.Y].Selected = true;

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

        private void arp_dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("arp_dataGridView_RowEnter");
        }

        private void arp_dataGridView_SelectionChanged(object sender, EventArgs e)
        {

            //MessageBox.Show("arp_dataGridView_SelectionChanged");

        }

        private PDMObject GetSelectedAirport()
        {
            PDMObject arp = null;
            if (this.Tag != null)
            {
                string airportId = arp_dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();
                List<PDMObject> lst = this.Tag as List<PDMObject>;

                arp = (from element in lst where (element != null) && (element is AirportHeliport) && (element.ID.CompareTo(airportId) == 0) select element).FirstOrDefault();

            }

            return arp;
        }

        private PDMObject GetSelectedRwy()
        {
            AirportHeliport arp = GetSelectedAirport() as AirportHeliport;
            Runway rwy = null;
            if (arp != null)
            {
                string rwyId = rwy_dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();

                if ((arp != null) && (arp.RunwayList != null))
                {
                    rwy = (from element in arp.RunwayList where (element != null) && (element is Runway) && (element.ID.CompareTo(rwyId) == 0) select element).FirstOrDefault() as Runway;
                }

            }

            return rwy;
        }

        private PDMObject GetSelectedTHR()
        {
            Runway rwy = GetSelectedRwy() as Runway;
            RunwayDirection rdn = null;
            if (rwy != null)
            {
                string rdnId = rdn_dataGridView.SelectedRows[0].Cells["FeatureGUID"].Value.ToString();

                if ((rwy != null) && (rwy.RunwayDirectionList != null))
                {
                    rdn = (from element in rwy.RunwayDirectionList where (element != null) && (element is RunwayDirection) && (element.ID.CompareTo(rdnId) == 0) select element).FirstOrDefault() as RunwayDirection;
                }

            }

            return rdn;
        }

        private PDMObject GetSelectedNavaidSystem()
        {
            RunwayDirection rdn = GetSelectedTHR() as RunwayDirection;
            PDMObject NavaidSys = null;
            if (rdn != null)
            {
                string NavaidSysId = navaids_dataGridView.SelectedRows[0].Cells["ID_NavaidSystem"].Value.ToString();

                if (rdn.Related_NavaidSystem != null)
                {
                    NavaidSys = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && (element.ID.CompareTo(NavaidSysId) == 0) select element).FirstOrDefault();
                }
            }

            return NavaidSys;
        }

        private void button_EditRWY_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedRwy();

            Point CellAdr = rwy_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            rwy_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button_EditRDN_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedTHR();

            Point CellAdr = rdn_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            rdn_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button_EditNavaidSystem_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedNavaidSystem();

            Point CellAdr = navaids_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            navaids_dataGridView.Rows[CellAdr.Y].Selected = true;
        }



        private PDMObject GetNavaidComponent(DataGridView selected_dataGridView)
        {
            
            NavaidSystem NavaidSys = GetSelectedNavaidSystem() as NavaidSystem;
            PDMObject navaidComp = null;
            if (NavaidSys != null)
            {
                string NavaidCompId = selected_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

                if (NavaidSys.Components != null)
                {
                    navaidComp = (from element in NavaidSys.Components where (element != null) && (element.ID.CompareTo(NavaidCompId) == 0) select element).FirstOrDefault();
                }

                
            }

            return navaidComp;
        }

        private DataGridView getDataContextGrid()
        {
            DataGridView selected_dataGridView = dataGridView1;
            if (label2.BackColor == System.Drawing.SystemColors.GradientActiveCaption) selected_dataGridView = dataGridView2;
            if (label3.BackColor == System.Drawing.SystemColors.GradientActiveCaption) selected_dataGridView = dataGridView3;
            return selected_dataGridView;
        }


        private void button_DeleteADHP_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedAirport();
            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);
            LoadData(false);
        }

        private void button_DeleteRWY_Click(object sender, EventArgs e)
        {
            PDMObject arp = GetSelectedAirport();
            PDMObject obj = GetSelectedRwy();
            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (arp as AirportHeliport).RunwayList.Remove(obj as Runway);
            LoadData(false);
        }

        private void button_DeleteRDN_Click(object sender, EventArgs e)
        {
            PDMObject rwy = GetSelectedRwy();
            PDMObject obj = GetSelectedTHR();
            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (rwy as Runway).RunwayDirectionList.Remove(obj as RunwayDirection);
            LoadData(false);
        }

        private void button_DeleteNavaidSystem_Click(object sender, EventArgs e)
        {
            PDMObject rdn = GetSelectedTHR();

            PDMObject obj = GetSelectedNavaidSystem();
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            (rdn as RunwayDirection).Related_NavaidSystem.Remove(obj as NavaidSystem);


            LoadData(false);
        }

        private void button_AddADHP_Click(object sender, EventArgs e)
        {
            AirportHeliport obj = new AirportHeliport();
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                ITable tbl = _AIRTRACK_TableDic[obj.GetType()];
                obj.RebuildGeo();
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                obj.StoreToDB(tbl);
                lst.Add(obj);
                LoadData(true);
                arp_dataGridView.CurrentCell = arp_dataGridView.Rows[arp_dataGridView.RowCount - 1].Cells["designator"];
            }
        }

        private void button_AddRWY_Click(object sender, EventArgs e)
        {
            AirportHeliport masterObj = GetSelectedAirport() as AirportHeliport;
            Runway obj = new Runway();
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                //obj.RebuildGeo();
                 //Point CellAdr = arp_dataGridView.CurrentCellAddress;
                 ITable tbl = _AIRTRACK_TableDic[obj.GetType()];

                if (masterObj.RunwayList == null) masterObj.RunwayList = new List<Runway>();
                obj.ID = Guid.NewGuid().ToString();
                obj.ID_AirportHeliport = masterObj.ID;
                masterObj.RunwayList.Add(obj);
                obj.StoreToDB(tbl);
                LoadData(true);

                //arp_dataGridView.CurrentCell = arp_dataGridView.Rows[CellAdr.Y].Cells[CellAdr.X];
                rwy_dataGridView.CurrentCell = rwy_dataGridView.Rows[rwy_dataGridView.RowCount - 1].Cells["designator"];
            }
        }

        private void button_AddRDN_Click(object sender, EventArgs e)
        {
            Runway masterObj = GetSelectedRwy() as Runway;
            RunwayDirection obj = new RunwayDirection();
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                obj.RebuildGeo();
                //Point CellAdr =rwy_dataGridView.CurrentCellAddress;
                ITable tbl = _AIRTRACK_TableDic[obj.GetType()];

                if (masterObj.RunwayDirectionList == null) masterObj.RunwayDirectionList = new List<RunwayDirection>();
                obj.ID = Guid.NewGuid().ToString();
                obj.ID_AirportHeliport = masterObj.ID_AirportHeliport;
                obj.ID_Runway = masterObj.ID;
                masterObj.RunwayDirectionList.Add(obj);
                obj.StoreToDB(tbl);
                LoadData(true);

                //rwy_dataGridView.CurrentCell = rwy_dataGridView.Rows[CellAdr.Y].Cells[CellAdr.X];
                rdn_dataGridView.CurrentCell = rdn_dataGridView.Rows[rdn_dataGridView.RowCount - 1].Cells["designator"];
            }
        }

        private void button_AddNavaidSystem_Click(object sender, EventArgs e)
        {


            RunwayDirection masterObj = GetSelectedTHR() as RunwayDirection;
            NavaidSystem obj = new NavaidSystem();
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                obj.RebuildGeo();
                //Point CellAdr =rwy_dataGridView.CurrentCellAddress;
                ITable tbl = _AIRTRACK_TableDic[obj.GetType()];

                if (masterObj.Related_NavaidSystem == null) masterObj.Related_NavaidSystem = new List<NavaidSystem>();
                obj.ID = Guid.NewGuid().ToString();
                obj.ID_AirportHeliport = masterObj.ID_AirportHeliport;
                obj.ID_RunwayDirection = masterObj.ID;
                masterObj.Related_NavaidSystem.Add(obj);
                obj.StoreToDB(tbl);
                LoadData(true);

                navaids_dataGridView.CurrentCell = navaids_dataGridView.Rows[navaids_dataGridView.RowCount - 1].Cells["designator"];
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            PDMObject obj = GetSelectedAirport();
            if (obj != null)
            {
                obj.DeleteAll(_AIRTRACK_TableDic);
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                lst.RemoveAll(item => item is AirportHeliport);
                LoadData(false);
            }
        }

    
 

        private void label1_Click(object sender, EventArgs e)
        {
            label1.BackColor = System.Drawing.SystemColors.Highlight;
            label2.BackColor = System.Drawing.SystemColors.Highlight;
            label3.BackColor = System.Drawing.SystemColors.Highlight;
            (sender as Label).BackColor = System.Drawing.SystemColors.GradientActiveCaption;

        }

 

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            label2.BackColor = System.Drawing.SystemColors.Highlight;
            label3.BackColor = System.Drawing.SystemColors.Highlight;
        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            label1.BackColor = System.Drawing.SystemColors.Highlight;
            label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            label3.BackColor = System.Drawing.SystemColors.Highlight;
        }

        private void dataGridView3_MouseClick(object sender, MouseEventArgs e)
        {
            label1.BackColor = System.Drawing.SystemColors.Highlight;
            label2.BackColor = System.Drawing.SystemColors.Highlight;
            label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetNavaidComponent(getDataContextGrid());

            Point CellAdr = getDataContextGrid().CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            getDataContextGrid().Rows[CellAdr.Y].Selected = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavaidSystem NavaidSys = GetSelectedNavaidSystem() as NavaidSystem;

            PDMObject obj = GetNavaidComponent(getDataContextGrid());
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            NavaidSys.Components.Remove(obj);
            LoadData(true);
        }



        private void addNavaidComponentButton1_onClicked()
        {

            NavaidComponent obj = null;
            switch (addNavaidComponentButton1.NavSystem)
            {
               case("Add VOR"):
                    obj = new VOR();
               break;

               case ("Add DME"):
                    obj = new DME();
               break;

               case ("Add NDB"):
                    obj = new NDB();
               break;

               case ("Add TACAN"):
                    obj = new TACAN();
               break;

               case ("Add Localizer"):
                    obj = new Localizer();
               break;

               case ("Add GlidePath"):
                    obj = new GlidePath();
               break;
            }

            if (obj != null)
            {
                NavaidSystem NavaidSys = GetSelectedNavaidSystem() as NavaidSystem;

                InputForm frm = new InputForm();
                frm.LinkedObject = obj;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ITable tbl = _AIRTRACK_TableDic[obj.GetType()];

                    if (NavaidSys.Components == null) NavaidSys.Components = new List<PDMObject>();
                    obj.ID = Guid.NewGuid().ToString();
                    obj.ID_NavaidSystem = NavaidSys.ID;
                    NavaidSys.Components.Add(obj);
                    obj.StoreToDB(tbl);
                    LoadData(true);

                }
            }
        }

        private void addNavaidComponentButton1_Load(object sender, EventArgs e)
        {

        }

        

    }
}
