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
    public partial class ProcedureListDataView : UserControl
    {
        public Dictionary<Type, ITable> _AIRTRACK_TableDic;

        public ProcedureListDataView()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //splitContainerLegs.Panel2Collapsed = !splitContainerLegs.Panel2Collapsed;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //splitContainerReferencePoints.Panel2Collapsed = !splitContainerReferencePoints.Panel2Collapsed;
            splitContainerLegs.Panel2Collapsed = !splitContainerLegs.Panel2Collapsed;

        }

        public void LoadData(bool focusOnSelectedRow)
        {
            //Point arp_CellAdr = arp_dataGridView.CurrentCellAddress;
            //Point rwy_CellAdr = rwy_dataGridView.CurrentCellAddress;
            //Point rdn_CellAdr = rdn_dataGridView.CurrentCellAddress;
            //Point navaids_CellAdr = navaids_dataGridView.CurrentCellAddress;

            proceduresdataGridView.DataSource = null;
            transitionsdataGridView.DataSource = null;
            legsdataGridView.DataSource = null;
            segmentPointdataGridView.DataSource = null;
            facilitymakeUpdataGridView.DataSource = null;
            distIndicationdataGridView.DataSource = null;
            angleIndicationdataGridView.DataSource = null;



            PROC_TYPE_code curProcType = PROC_TYPE_code.Approach;
            switch (this.Name)
            {
                case ("SID"):
                    curProcType = PROC_TYPE_code.SID;
                    break;
                case ("STAR"):
                    curProcType = PROC_TYPE_code.STAR;
                    break;
                default:
                    curProcType = PROC_TYPE_code.Approach;
                    break;
            }


            DataTable ProceduresTbl = ARENA_DataReader.Table_GetProcedureList(curProcType);
            DataTable ProcedureTransitionsTbl = ARENA_DataReader.Table_GetProcedureTransitionsList(curProcType);
            DataTable ProcedureLegsTbl = ARENA_DataReader.Table_GetProcedureLegsList(curProcType);
            DataTable ProcSegmentPointTbl = ARENA_DataReader.Table_GetProcedureSegmentPointList(curProcType);
            DataTable FacilityMakeUpTbl = ARENA_DataReader.Table_GetProcedureFacilityMakeUptList(curProcType);
            DataTable FacilityAngleTbl = ARENA_DataReader.Table_GetProcedureFacilityAngleList(curProcType);
            DataTable FacilityDistanceTbl = ARENA_DataReader.Table_GetProcedureFacilityDistanceList(curProcType);

            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.InvariantCulture;

            ds.Tables.Add(ProceduresTbl);
            ds.Tables.Add(ProcedureTransitionsTbl);
            ds.Tables.Add(ProcedureLegsTbl);
            ds.Tables.Add(ProcSegmentPointTbl);
            ds.Tables.Add(FacilityMakeUpTbl);
            ds.Tables.Add(FacilityAngleTbl);
            ds.Tables.Add(FacilityDistanceTbl);


            DataRelation relation_Proc_Trans = new DataRelation("Procedure_Transition", ProceduresTbl.Columns["FeatureGUID"], ProcedureTransitionsTbl.Columns["ID_procedure"]);
            ds.Relations.Add(relation_Proc_Trans);

            DataRelation relation_Trans_Leg = new DataRelation("Transition_Leg", ProcedureTransitionsTbl.Columns["FeatureGUID"], ProcedureLegsTbl.Columns["ID_Transition"]);
            ds.Relations.Add(relation_Trans_Leg);

            DataRelation relation_Leg_ProcSegmentPnt = new DataRelation("Leg_procSegmentPnt", ProcedureLegsTbl.Columns["ProcFeatureGUID"], ProcSegmentPointTbl.Columns["Route_LEG_ID"]);
            ds.Relations.Add(relation_Leg_ProcSegmentPnt);


            DataRelation relation_ProcSegmentPnt_FmUp = new DataRelation("ProcSegmentPnt_FmUp", ProcSegmentPointTbl.Columns["SegPointFeatureGUID"], FacilityMakeUpTbl.Columns["SegmentPointID"]);
            ds.Relations.Add(relation_ProcSegmentPnt_FmUp);

            DataRelation relation_FmUp_FasAngle = new DataRelation("FmUp_FasAngle", FacilityMakeUpTbl.Columns["FeatureGUID"], FacilityAngleTbl.Columns["facilityMakeUp_ID"]);
            ds.Relations.Add(relation_FmUp_FasAngle);

            DataRelation relation_FmUp_FasDistance = new DataRelation("FmUp_FasDistance", FacilityMakeUpTbl.Columns["FeatureGUID"], FacilityDistanceTbl.Columns["facilityMakeUp_ID"]);
            ds.Relations.Add(relation_FmUp_FasDistance);



            BindingSource proc_BindingSource = new BindingSource();
            BindingSource trans_BindingSource = new BindingSource();
            BindingSource leg_BindingSource = new BindingSource();
            BindingSource segPnt_BindingSource = new BindingSource();
            BindingSource fasMkUP_BindingSource = new BindingSource();
            BindingSource fasAngle_BindingSource = new BindingSource();
            BindingSource fasDist_BindingSource = new BindingSource();

            proc_BindingSource.DataSource = ds;
            proc_BindingSource.DataMember = ds.Tables[0].TableName;


            trans_BindingSource.DataSource = proc_BindingSource;
            trans_BindingSource.DataMember = "Procedure_Transition";

            leg_BindingSource.DataSource = trans_BindingSource;
            leg_BindingSource.DataMember = "Transition_Leg";

            segPnt_BindingSource.DataSource = leg_BindingSource;
            segPnt_BindingSource.DataMember = "Leg_procSegmentPnt";

            fasMkUP_BindingSource.DataSource = segPnt_BindingSource;
            fasMkUP_BindingSource.DataMember = "ProcSegmentPnt_FmUp";

            fasAngle_BindingSource.DataSource = fasMkUP_BindingSource;
            fasAngle_BindingSource.DataMember = "FmUp_FasAngle";

            fasDist_BindingSource.DataSource = fasMkUP_BindingSource;
            fasDist_BindingSource.DataMember = "FmUp_FasDistance";


            proceduresdataGridView.DataSource = proc_BindingSource;
            transitionsdataGridView.DataSource = trans_BindingSource;
            legsdataGridView.DataSource = leg_BindingSource;
            segmentPointdataGridView.DataSource = segPnt_BindingSource;
            facilitymakeUpdataGridView.DataSource = fasMkUP_BindingSource;
            angleIndicationdataGridView.DataSource = fasAngle_BindingSource;
            distIndicationdataGridView.DataSource = fasDist_BindingSource;

            ARENA_DataReader.hideColumns(proceduresdataGridView);
            ARENA_DataReader.hideColumns(transitionsdataGridView);
            ARENA_DataReader.hideColumns(legsdataGridView);
            ARENA_DataReader.hideColumns(segmentPointdataGridView);
            ARENA_DataReader.hideColumns(facilitymakeUpdataGridView);
            ARENA_DataReader.hideColumns(angleIndicationdataGridView);
            ARENA_DataReader.hideColumns(distIndicationdataGridView);

            if (focusOnSelectedRow)
            {
                //if ((arp_CellAdr.Y >= 0) && (arp_CellAdr.X >= 0)) arp_dataGridView.CurrentCell = arp_dataGridView.Rows[arp_CellAdr.Y].Cells[arp_CellAdr.X];
                //if ((rwy_CellAdr.Y >= 0) && (rwy_CellAdr.X >= 0)) rwy_dataGridView.CurrentCell = rwy_dataGridView.Rows[rwy_CellAdr.Y].Cells[rwy_CellAdr.X];
                //if ((rdn_CellAdr.Y >= 0) && (rdn_CellAdr.X >= 0)) rdn_dataGridView.CurrentCell = rdn_dataGridView.Rows[rdn_CellAdr.Y].Cells[rdn_CellAdr.X];
                //if ((navaids_CellAdr.Y >= 0) && (navaids_CellAdr.X >= 0)) navaids_dataGridView.CurrentCell = navaids_dataGridView.Rows[navaids_CellAdr.Y].Cells[navaids_CellAdr.X];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(@"Delete all records?", "Delte all", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            PROC_TYPE_code curProcType = PROC_TYPE_code.Approach;
            switch (this.Name)
            {
                case ("SID"):
                    curProcType = PROC_TYPE_code.SID;
                    break;
                case ("STAR"):
                    curProcType = PROC_TYPE_code.STAR;
                    break;
                default:
                    curProcType = PROC_TYPE_code.Approach;
                    break;
            }


            List<PDMObject> lst = this.Tag as List<PDMObject>;
            var procList = (from element in lst where (element != null) && (element is Procedure) && ((Procedure)element).ProcedureType == curProcType select element).ToList();

            foreach (var sellObj2 in procList)
            {
                if (sellObj2 != null)
                {

                    #region  

                    foreach (var item in ((Procedure)sellObj2).Transitions)
                    {
                        foreach (var leg in item.Legs)
                        {
                            if (leg is MissaedApproachLeg)
                            {
                                ARENA.Util.LogicUtil.RemoveFeature("MissaedApproachLeg", "FeatureGUID", leg.ID);
                            }
                            if (leg is FinalLeg)
                            {
                                ARENA.Util.LogicUtil.RemoveFeature("FinalLeg", "FeatureGUID", leg.ID);
                            }

                            if (leg.StartPoint != null) ARENA.Util.LogicUtil.RemoveFeature("SegmentPoint", "Route_LEG_ID", leg.ID);
                            if (leg.EndPoint != null) ARENA.Util.LogicUtil.RemoveFeature("SegmentPoint", "Route_LEG_ID", leg.ID);
                            if (leg.ArcCentre != null) ARENA.Util.LogicUtil.RemoveFeature("SegmentPoint", "Route_LEG_ID", leg.ID);
                        }
                    }

                    #endregion

                    ARENA.Util.LogicUtil.RemoveFeature(sellObj2);
                }
            }

            lst.RemoveAll(item => (item is Procedure) && ((Procedure)item).ProcedureType == curProcType);
            LoadData(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

    }
}
