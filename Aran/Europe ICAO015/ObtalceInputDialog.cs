using Aran.AranEnvironment;
using Europe_ICAO015;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aran.Geometries;


namespace ICAO015
{
    public partial class ObtalceInputDialog : Form
    {
        private AranTool _aranToolItem;
        //private bool _pointPickerClicked;
        private int _handleNavaid;
        //public bool CalcOnlyRadiusCheckTreeview = false;
        //public bool CalcOnly2DgraphicCheckTreeview = false;
        public Aran.Geometries.Point ObstclPntPrj = new Aran.Geometries.Point();

        //Calculate for only Radius {
        List<ParameterForDmeN> ParamListDMEN = new List<ParameterForDmeN>();
        List<ParameterForCVOR> ParamListCVOR = new List<ParameterForCVOR>();
        List<ParameterForDVOR> ParamListDVOR = new List<ParameterForDVOR>();
        List<ParameterForMarkers> ParamListMarker = new List<ParameterForMarkers>();
        List<ParameterForNDB> ParamListNDB = new List<ParameterForNDB>();
        //Calculate for only Radius }
        //Calculate for only 2D Graphic {
        List<Obstacle_ParamListPolygons> Obstcl_Input_ParamList_2DGrpahic = new List<Obstacle_ParamListPolygons>();
        //Calculate for only 2D Graphic }

        public ObtalceInputDialog()
        {
            InitializeComponent();
        }
        public ObtalceInputDialog(AranTool aranToolItem) : this()
        {
            _aranToolItem = aranToolItem;
            _aranToolItem.MouseClickedOnMap += OnMouseClickedOnMap;
        }
        DataTable tablefordme = new DataTable();
        DataTable tableforILS = new DataTable();
        private void ObtalceInputDialog_Load(object sender, EventArgs e)
        {

            tableforILS.Columns.Add("Type of Navigation");//table for ILS
            tableforILS.Columns.Add("Navaid Name");
            tableforILS.Columns.Add("Polygon Type");
            tableforILS.Columns.Add("Obstacle Distance");
            tableforILS.Columns.Add("Penetrate");
            tableforILS.Columns.Add("Equation");

            tablefordme.Columns.Add("TypeOfNavigation");//table for dme
            tablefordme.Columns.Add("Navaid Name");
            tablefordme.Columns.Add("Radius");
            tablefordme.Columns.Add("Obstacle Distance");
            tablefordme.Columns.Add("Penetrate");
            tablefordme.Columns.Add("Equation");

        }
        internal void OnMouseClickedOnMap(object sender, MapMouseEventArg e)
        {
            Aran.Geometries.Point pntPrj = new Aran.Geometries.Point(e.X, e.Y);
            Aran.Geometries.Point pntGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point>(pntPrj);
            pointPicker1.Latitude = pntGeo.Y;
            pointPicker1.Longitude = pntGeo.X;
            DrawPoint(pntPrj);
            ObstclPntPrj = pntPrj;
        }
        public void DrawPoint(Aran.Geometries.Point pnt)
        {
            GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(_handleNavaid);
            _handleNavaid = GlobalParams.AranEnvironment.Graphics.DrawPoint(pnt, 255 * 255, true, false);
        }

        private void ObtalceInputDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pointPicker1.ByClick)
            {
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
                pointPicker1.ByClick = false;
            }
            _aranToolItem.MouseClickedOnMap = null;
            GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(_handleNavaid);
        }

        private void pointPicker1_ByClickChanged(object sender, EventArgs e)
        {
            if (pointPicker1.ByClick)
                GlobalParams.AranEnvironment.AranUI.SetCurrentTool(_aranToolItem);
            else
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
        }
        //public void AddListParameters(List<ParameterForDmeN> ListDMEN, List<ParameterForCVOR> ListCVOR, List<ParameterForDVOR> ListDVOR, List<ParameterForMarkers> ListMarker, List<ParameterForNDB> ListNDB, List<Obstacle_ParamListPolygons> List2DGraphics)
        //{
        //    ParamListDMEN = ListDMEN;
        //    ParamListCVOR = ListCVOR;
        //    ParamListDVOR = ListDVOR;
        //    ParamListMarker = ListMarker;
        //    ParamListNDB = ListNDB;
        //    Obstcl_Input_ParamList_2DGrpahic = List2DGraphics;
        //}
        private void BtnObstclInptCalc_Click(object sender, EventArgs e)
        {
            ObstacleInputCalculatorForOnlyRadius obstclinputcalc = new ObstacleInputCalculatorForOnlyRadius();
            List<AddReportListObstInputForOnlyRadiusCalculated> ListForOnlyRadiusCalculated = new List<AddReportListObstInputForOnlyRadiusCalculated>();
            ShowGridObstacleInputReportForRadiusCalculated ShowReport_InGrid = new ShowGridObstacleInputReportForRadiusCalculated();

            ObstacleInputCalculatorForOnly2DGraphic Obstcl_Input_Calc2D = new ObstacleInputCalculatorForOnly2DGraphic();
            List<AddReportListObstInputForOnly2DGraphicCalculated> ListOnly2DGraphic = new List<AddReportListObstInputForOnly2DGraphicCalculated>();
            ShowGridObstacleInputReportFor_2D_Graphic_Calculated ShowReport_In2D_Graphic = new ShowGridObstacleInputReportFor_2D_Graphic_Calculated();


            ParamListDMEN = ObstacleInputAddParameterList.GetListParamDMEN();
            ParamListCVOR = ObstacleInputAddParameterList.GetListParamCVOR();
            ParamListDVOR = ObstacleInputAddParameterList.GetListParamDVOR();
            ParamListMarker = ObstacleInputAddParameterList.GetListParamMarker();
            ParamListNDB = ObstacleInputAddParameterList.GetListParamNDB();
            Obstcl_Input_ParamList_2DGrpahic = ObstacleInputAddParameterList.GetListParamList2DGraphic();


            if (TxtBoxElevation.Text != "")
            {
                tablefordme.Clear();

                if (ParamListDVOR.Count > 0)
                {
                    ListForOnlyRadiusCalculated = obstclinputcalc.CalculatorForDVOR(ParamListDVOR, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                }
                if (ParamListCVOR.Count > 0)
                {
                    ListForOnlyRadiusCalculated = obstclinputcalc.CalculatorForCVOR(ParamListCVOR, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                }
                if (ParamListDMEN.Count > 0)
                {
                    ListForOnlyRadiusCalculated = obstclinputcalc.CalculatorForDMEN(ParamListDMEN, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                }
                if (ParamListMarker.Count > 0)
                {
                    ListForOnlyRadiusCalculated = obstclinputcalc.CalculatorForMarker(ParamListMarker, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                }
                if (ParamListNDB.Count > 0)
                {
                    ListForOnlyRadiusCalculated = obstclinputcalc.CalculatorForNDB(ParamListNDB, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                }

                if (ListForOnlyRadiusCalculated.Count > 0)
                {
                    ShowReport_InGrid.ShowGridObstclInputReportForRadiusCalc(ListForOnlyRadiusCalculated, GridObstclInptReportForDME, tablefordme);
                }

                tableforILS.Clear();

                if (Obstcl_Input_ParamList_2DGrpahic.Count > 0)
                {
                    ListOnly2DGraphic = Obstcl_Input_Calc2D.Calc_2DGraphics(Obstcl_Input_ParamList_2DGrpahic, ObstclPntPrj, Convert.ToDouble(TxtBoxElevation.Text));
                    ShowReport_In2D_Graphic.ShowGridObstclInputReportFor_2DGraphic(ListOnly2DGraphic, GridObstclInptReportFor_ILS, tableforILS);
                }

                LblObstacleCalcTxt.Visible = true;

                if (ListForOnlyRadiusCalculated.Count > 0 && ListOnly2DGraphic.Count == 0)
                {
                    TabPage tab = tabControl1.TabPages[0];
                    tabControl1.SelectTab(tab);
                    LblObstacleCalcTxt.Text = "Obstacle calculated is only for DME";
                }
                if (ListForOnlyRadiusCalculated.Count == 0 && ListOnly2DGraphic.Count > 0)
                {
                    TabPage tab = tabControl1.TabPages[1];
                    tabControl1.SelectTab(tab);

                    LblObstacleCalcTxt.Text = "Obstacle calculated is only for ILS";
                }
                if (ListForOnlyRadiusCalculated.Count > 0 && ListOnly2DGraphic.Count > 0)
                {
                    LblObstacleCalcTxt.Text = "Obstacle calculated both of ILS and DME";
                }
                if (ListForOnlyRadiusCalculated.Count == 0 && ListOnly2DGraphic.Count == 0)
                {
                    LblObstacleCalcTxt.Text = "Obstacle include never Area:Please click correct Area";
                }

            }


            if (GridObstclInptReportForDME.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclInptReportForDME.Rows)
                {
                    if (Convert.ToDouble(row.Cells[4].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            if (GridObstclInptReportFor_ILS.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclInptReportFor_ILS.Rows)
                {
                    if (Convert.ToDouble(row.Cells[4].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }






        }

        private void GridObstclInptReportForDME_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridObstclInptReportForDME.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclInptReportForDME.Rows)
                {
                    if (Convert.ToDouble(row.Cells[4].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void GridObstclInptReportFor_ILS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridObstclInptReportFor_ILS.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclInptReportFor_ILS.Rows)
                {
                    if (Convert.ToDouble(row.Cells[4].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}