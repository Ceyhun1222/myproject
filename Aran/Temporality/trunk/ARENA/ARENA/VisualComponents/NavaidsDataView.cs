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
    public partial class NavaidsDataView : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public NavaidsDataView()
        {
            InitializeComponent();
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

        public void LoadData()
        {

            //ARENA_DataReader.PathToPDM_DB = @"C:\PDM\TEMP_ALL_PDM_OBJ\pdm.mdb";

            DataTable NavaidSystemTbl = ARENA_DataReader.Table_GetNavaidSystemList();
            DataTable VORTbl = ARENA_DataReader.GetVORList();
            DataTable DMETbl = ARENA_DataReader.GetDMEList();
            DataTable TacanTbl = ARENA_DataReader.GetTACANList();
            DataTable NDBTbl = ARENA_DataReader.GetNDBList();


            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(NavaidSystemTbl);
            ds.Tables.Add(VORTbl);
            ds.Tables.Add(DMETbl);
            ds.Tables.Add(TacanTbl);
            ds.Tables.Add(NDBTbl);


            BindingSource navaid_BindingSource = new BindingSource();
            navaid_BindingSource.DataSource = ds;
            navaid_BindingSource.DataMember = ds.Tables[0].TableName;

            BindingSource vor_BindingSource = new BindingSource();
            vor_BindingSource.DataSource = ds;
            vor_BindingSource.DataMember = ds.Tables[1].TableName;

            BindingSource dme_BindingSource = new BindingSource();
            dme_BindingSource.DataSource = ds;
            dme_BindingSource.DataMember = ds.Tables[2].TableName;

            BindingSource tacan_BindingSource = new BindingSource();
            tacan_BindingSource.DataSource = ds;
            tacan_BindingSource.DataMember = ds.Tables[3].TableName;

            BindingSource ndb_BindingSource = new BindingSource();
            ndb_BindingSource.DataSource = ds;
            ndb_BindingSource.DataMember = ds.Tables[4].TableName;

            navaids_dataGridView.DataSource = navaid_BindingSource;
            VOR_dataGridView.DataSource = vor_BindingSource;
            DME_dataGridView.DataSource = dme_BindingSource;
            TACAN_dataGridView.DataSource = tacan_BindingSource;
            NDB_dataGridView.DataSource = ndb_BindingSource;

            ARENA_DataReader.hideColumns(navaids_dataGridView);
            ARENA_DataReader.hideColumns(VOR_dataGridView);
            ARENA_DataReader.hideColumns(DME_dataGridView);
            ARENA_DataReader.hideColumns(TACAN_dataGridView);
            ARENA_DataReader.hideColumns(NDB_dataGridView);

        }

        //private void hideColumns(DataGridView dgw)
        //{
        //    string[] colnames = { "OBJECTID", "FeatureGUID", "ID_Runway", "ID_NavaidSystem", "ID_AirportHeliport", "ID_RunwayDirection", "ID_NavaidComponent" };

        //    foreach (DataGridViewColumn clmn in dgw.Columns)
        //    {
        //        if (Array.IndexOf(colnames, clmn.Name) >= 0) clmn.Visible = false;
        //    }

        //}

        private void button6_Click(object sender, EventArgs e)
        {

            PDMObject obj = GetSelectedNavaidSystem();

            Point CellAdr = navaids_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            navaids_dataGridView.Rows[CellAdr.Y].Selected = true;

        }

        private void StartEditingProcedure(PDMObject obj)
        {
            InputForm frm = new InputForm();
            frm.LinkedObject = obj;
            frm.ShowDialog();

            ITable tbl = _AIRTRACK_TableDic[obj.GetType()];
            obj.UpdateDB(tbl);

            Application.DoEvents();

            LoadData();
        }

        private PDMObject GetSelectedNavaidSystem()
        {
            PDMObject NavaidSys = null;
            List<PDMObject> lst = this.Tag as List<PDMObject>;

            string NavaidSysId = navaids_dataGridView.SelectedRows[0].Cells["ID_NavaidSystem"].Value.ToString();

            NavaidSys = (from element in lst where (element != null) && (element is NavaidSystem) && (element.ID.CompareTo(NavaidSysId) == 0) select element).FirstOrDefault();
            
            return NavaidSys;
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

        private PDMObject GetNavaidComponent(string NavaidCompId)
        {
            PDMObject NavaidSysComp = null;
            List<PDMObject> lst = this.Tag as List<PDMObject>;


            foreach (PDMObject obj in lst)
            {
                if (obj is NavaidSystem)
                {
                    NavaidSystem NavSys = (NavaidSystem)obj;
                    if (NavSys.Components != null)
                    {
                        foreach (PDMObject comp in NavSys.Components)
                        {
                            switch (comp.GetType().Name)
                            {
                                case ("Localizer"):
                                    if (((Localizer)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                                case ("GlidePath"):
                                    if (((GlidePath)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                                case ("VOR"):
                                    if (((VOR)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                                case ("DME"):
                                    if (((DME)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                                case ("TACAN"):
                                    if (((TACAN)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                                case ("NDB"):
                                    if (((NDB)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSysComp = comp;
                                    break;
                            }

                            if (NavaidSysComp != null) break;
                        }
                    }
                }
                if (NavaidSysComp != null) break;

            }

            return NavaidSysComp;
        }

        private PDMObject GetNavaidSystem_byComponentID(string NavaidCompId)
        {
            PDMObject NavaidSys = null;
            List<PDMObject> lst = this.Tag as List<PDMObject>;


            foreach (PDMObject obj in lst)
            {
                if (obj is NavaidSystem)
                {
                    NavaidSystem NavSys = (NavaidSystem)obj;
                    if (NavSys.Components != null)
                    {
                        foreach (PDMObject comp in NavSys.Components)
                        {
                            switch (comp.GetType().Name)
                            {
                                case ("Localizer"):
                                    if (((Localizer)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                                case ("GlidePath"):
                                    if (((GlidePath)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                                case ("VOR"):
                                    if (((VOR)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                                case ("DME"):
                                    if (((DME)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                                case ("TACAN"):
                                    if (((TACAN)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                                case ("NDB"):
                                    if (((NDB)comp).ID.CompareTo(NavaidCompId) == 0)
                                        NavaidSys = NavSys;
                                    break;
                            }

                            if (NavaidSys != null) break;
                        }
                    }
                }
                if (NavaidSys != null) break;

            }

            return NavaidSys;
        }



        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }



        private void button13_Click(object sender, EventArgs e)
        {
            string NavaidCompId = VOR_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject obj = GetNavaidComponent(NavaidCompId);

            Point CellAdr = VOR_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            VOR_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string NavaidCompId = DME_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject obj = GetNavaidComponent(NavaidCompId);

            Point CellAdr = DME_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            DME_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string NavaidCompId = TACAN_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject obj = GetNavaidComponent(NavaidCompId);

            Point CellAdr = TACAN_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            TACAN_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string NavaidCompId = NDB_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject obj = GetNavaidComponent(NavaidCompId);

            Point CellAdr = NDB_dataGridView.CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            NDB_dataGridView.Rows[CellAdr.Y].Selected = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedNavaidSystem();
            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;


            List<PDMObject> lst = this.Tag as List<PDMObject>;
            lst.Remove(obj);

            LoadData();

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string NavaidCompId = VOR_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject NavaidSys = GetNavaidSystem_byComponentID(NavaidCompId);
            PDMObject obj = GetNavaidComponent(NavaidCompId);

            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (NavaidSys as NavaidSystem).Components.Remove(obj);
            LoadData();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string NavaidCompId = DME_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject NavaidSys = GetNavaidSystem_byComponentID(NavaidCompId);
            PDMObject obj = GetNavaidComponent(NavaidCompId);

            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (NavaidSys as NavaidSystem).Components.Remove(obj);
            LoadData();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string NavaidCompId = TACAN_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject NavaidSys = GetNavaidSystem_byComponentID(NavaidCompId);
            PDMObject obj = GetNavaidComponent(NavaidCompId);

            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (NavaidSys as NavaidSystem).Components.Remove(obj);
            LoadData();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string NavaidCompId = NDB_dataGridView.SelectedRows[0].Cells["ID_NavaidComponent"].Value.ToString();

            PDMObject NavaidSys = GetNavaidSystem_byComponentID(NavaidCompId);
            PDMObject obj = GetNavaidComponent(NavaidCompId);

            if (obj.DeleteObject(_AIRTRACK_TableDic)==0) return;

            (NavaidSys as NavaidSystem).Components.Remove(obj);
            LoadData();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetSelectedNavaidSystem();
            if (obj != null)
            {
                obj.DeleteAll(_AIRTRACK_TableDic);
                List<PDMObject> lst = this.Tag as List<PDMObject>;
                lst.RemoveAll(item => item is NavaidSystem);
                LoadData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

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

        private void button18_Click(object sender, EventArgs e)
        {
            NavaidSystem NavaidSys = GetSelectedNavaidSystem() as NavaidSystem;

            PDMObject obj = GetNavaidComponent(getDataContextGrid());
            if (obj.DeleteObject(_AIRTRACK_TableDic) == 0) return;

            NavaidSys.Components.Remove(obj);
            //LoadData(true);
            LoadData();

        }

        private DataGridView getDataContextGrid()
        {
            DataGridView selected_dataGridView = dataGridView1;
            if (label2.BackColor == System.Drawing.SystemColors.GradientActiveCaption) selected_dataGridView = dataGridView2;
            if (label3.BackColor == System.Drawing.SystemColors.GradientActiveCaption) selected_dataGridView = dataGridView3;
            return selected_dataGridView;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            PDMObject obj = GetNavaidComponent(getDataContextGrid());

            Point CellAdr = getDataContextGrid().CurrentCellAddress;

            if (obj != null)
            {
                StartEditingProcedure(obj);
            }

            getDataContextGrid().Rows[CellAdr.Y].Selected = true;
        }


        private void addNavaidComponentButton1_onClicked()
        {

            NavaidComponent obj = null;
            switch (addNavaidComponentButton1.NavSystem)
            {
                case ("Add VOR"):
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
                    //LoadData(true);
                    LoadData();

                }
            }
        }

        

    }
}
