using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Delib.Classes;
using Delib.Classes.Features;
using Delib.Classes.Features.Organisation;
using Delib.Specialized.Holding;
using Delib.Classes.Features.AirportHeliport;
using Delib.Specialized;
using Delib.Classes.Features.Navaid;
using ARAN.Contracts.UI;
using ARAN.Contracts.Constants;
using ARAN.Contracts.GeometryOperators;
using ARAN.GeometryClasses;
using Delib.Classes.Codes;
using System.Threading;
using Holding.Convential;
using ARAN.Common;
using ARAN;
using ChoosePointNS;
using Delib.Classes.Features.Obstacle;
using System.Diagnostics;

namespace Holding
{
	public partial class frmHoldingMain : Form
	{
        private DBModule _database;
        private UIContract _ui;
        private GeometryOperators _geomOperators;
        private SpatialReferenceOperation _spatialOperation;
        private bool tool_Clicked;
        private Bussines_Logic bLogic;
        private ModelWizardChange _modelWizard;
        private int _shablonHandle;
        
        private BaseArea baseArea;
        
        public frmHoldingMain()
		{
            try
            {

                InitializeComponent();
                this.Controls.Remove(tabControl1);
                this.Controls.Remove(panel2);
                this.Controls.Add(panel1);
                List<Control> contCollection = new List<Control>();
                contCollection.Add(panel1);
                contCollection.Add(panel2);

                bLogic = new Bussines_Logic(contCollection, 100000, 3000, ARANFunctions.DegToRad(270), 100, SideDirection.sideRight, ConditionType.normal);
                _modelWizard = bLogic.WizardChange;

                modelPBNBindingSource.DataSource = bLogic.ModelPBN;
                modelPointChoiseBindingSource.DataSource = bLogic.ModelPtChoise;
                modelWizardChangeBindingSource.DataSource = _modelWizard;
                modelAreaParamsBindingSource.DataSource = bLogic.ModelAreamParam;
                modelProcedureTypeBindingSource.DataSource = bLogic.ProcedureType;

                modelPointChoiseBindingSource.ResetBindings(true);
                modelPBNBindingSource.ResetBindings(true);
                modelWizardChangeBindingSource.ResetBindings(true);
                modelAreaParamsBindingSource.ResetBindings(true);
                modelProcedureTypeBindingSource.ResetBindings(true);
                holdingNavOperationBindingSource.DataSource = bLogic.HoldingNavaidOperation;

                validationClassBindingSource.DataSource = bLogic.Validation;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

		}

        private void frmHoldingMain_Load(object sender, EventArgs e)
        {
            _database = GlobalParams.Database;
            _ui = GlobalParams.UI;
            _spatialOperation = GlobalParams.SpatialRefOperation;
            _geomOperators = GlobalParams.GeomOperators;
            baseArea = new BaseArea();

            lblDistanceUnit.Text = InitHolding.DistanceConverter.Unit;

            lblIas.Text = "IAS";

            udAltitude.Increment = (decimal)InitHolding.HeightPrecision;
            lblAltitudeUnit.Text = InitHolding.HeightConverter.Unit;

            udIas.Increment = (decimal)InitHolding.SpeedPrecision;
            lblIasType.Text = InitHolding.SpeedConverter.Unit;

            lblWd.Text = "WD(" + InitHolding.DistanceConverter.Unit + ")";

            lblMOC.Text = InitHolding.HeightConverter.Unit;
            dataGridView1.Columns[2].HeaderText = "Distance (" + InitHolding.DistanceConverter.Unit + " )";
            dataGridView1.Columns[3].HeaderText = "Azimuth ( ° )";
         
        }

        #region Choose Points

        public void OnMapMouseUp(UIMouseEventArgs e)
        {
            bLogic.ModelPtChoise.DeletePoint();
            ARAN.GeometryClasses.Point ptGeo = _spatialOperation.PrjToGeoPoint(e.mLocation);
            if (bLogic.ModelPtChoise.PointDistanceControl(ptGeo))
            {
                bLogic.ModelPtChoise.DrawPoint(e.mLocation);
                bLogic.ModelPtChoise.CurPoint = _spatialOperation.PrjToGeoPoint(e.mLocation);

            }
            else
            {
                bLogic.ModelPtChoise.CurPoint = null;
                MessageBox.Show("Select the point only from interior of the circle", "Select point", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void OnMapMouseMove(UIMouseEventArgs e)
        {
            ARAN.GeometryClasses.Point ptGeo = _spatialOperation.PrjToGeoPoint(e.mLocation);
            pointPicker2.Latitude = ptGeo.Y;
            pointPicker2.Longitude = ptGeo.X;
            
        }   

        private void CreateUIPoint(ARAN.GeometryClasses.Point ptPrj)
        {
            bLogic.ModelPtChoise.DeletePoint();
            ARAN.GeometryClasses.Point ptGeo = _spatialOperation.PrjToGeoPoint(ptPrj);
            if (bLogic.ModelPtChoise.PointDistanceControl(ptGeo))
            {
                bLogic.ModelPtChoise.DrawPoint(ptPrj);
                bLogic.ModelPtChoise.CurPoint = ptGeo;

            }
            else
                bLogic.ModelPtChoise.CurPoint = null;
        }

        private void pointPicker2_ByClickChanged(object sender, EventArgs e)
        {
            tool_Clicked = pointPicker2.ByClick;

            if (HoldingService.ByClickToolButton != null)
            {
                HoldingService.ToolClicked = tool_Clicked;
                HoldingService.ByClickToolButton.SetDownState(tool_Clicked);
                btnCreatePt.Enabled = !tool_Clicked;

            }
        }
               
        #endregion

        private void btnBaseArea_Click(object sender, EventArgs e)
        {
            bLogic.CalculateBaseArea();
        }
		
        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(_modelWizard.CurContainer);
            _modelWizard.CurrentWizard++;
            this.Controls.Add(_modelWizard.CurContainer);
         
            if (HoldingService.ToolClicked)
                HoldingService.ByClickToolButton.SetDownState(false);
            
        }

        private void btnPrevius_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(_modelWizard.CurContainer);
            _modelWizard.CurrentWizard--;
            this.Controls.Add(_modelWizard.CurContainer);

            if (HoldingService.ToolClicked)
                HoldingService.ByClickToolButton.SetDownState(true);
        }

              
        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{

        //Action<List<OrganisationAuthority>,string> act = (organisationList,designator) =>
        //{
        //    cmbHldgProc.DataSource = organisationList;
        //    cmbHldgProc.DisplayMember = designator;
        //};

        //Action actImage =()=>
        //{
        //    pictureBox1.Visible = false;
        //} ;

        //try
        //{
        //    if (cmbHldgProc.InvokeRequired)
        //    {

        //        List<OrganisationAuthority> orgAuthList = GlobalParams.Database.HoldingQpi.GetOrganisationAuthorityList();

        //        cmbHldgProc.Invoke(act, orgAuthList,"designator");
        //        pictureBox1.Invoke(actImage);//pictureBox1.Visible = false;

        //    }
        //}
        //catch (Exception)
        //{

        //    //throw;
        //}

        //}

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
                bLogic.OnListChange(this, new ListChangedEventArgs(ListChangedType.ItemChanged, e.RowIndex));//list.BindList[e.RowIndex].Checked = !list.BindList[e.RowIndex].Checked;
           holdingNavListBindingSource.ResetBindings(false);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.AppStarting;
                bLogic.CreateReport();
                bLogic.ModelPtChoise.SaveIsEnabled = (bLogic.HReport.ObstacleReport.Where(rep => !rep.Validation).Count() == 0 || bLogic.HReport.ReportCount == 0)
                    && (bLogic.ModelAreamParam.Altitude >= bLogic.ModelAreamParam.CurMoc);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }            
        }

        private void btnDmeCoverage_Click(object sender, EventArgs e)
        {
            bLogic.CreateDmeCoverage();
        }

        private void frmHoldingMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            bLogic.Dispose();
            HoldingService.ByClickToolButton.SetDownState(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bLogic.CreateSave();
            
        }

        private void pointPicker2_KeyPressChanged_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                double latitude = pointPicker2.Latitude;
                double longtitude = pointPicker2.Longitude;
                //if (pointPicker2.LatitudeDirection == EarthDirection.W)
                //    latitude *= -1;
                //if (pointPicker2.LongtitudeDirection == EarthDirection.S)
                //    longtitude *= -1;

                ARAN.GeometryClasses.Point ptGeo = new ARAN.GeometryClasses.Point(longtitude, latitude);
                ARAN.GeometryClasses.Point ptPrj = _spatialOperation.GeoToPrjPoint(ptGeo);
                CreateUIPoint(ptPrj);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreatePt_Click(object sender, EventArgs e)
        {
            ARAN.GeometryClasses.Point ptPrj = _spatialOperation.GeoToPrjPoint( new ARAN.GeometryClasses.Point( pointPicker2.Longitude, pointPicker2.Latitude));
            CreateUIPoint(ptPrj);
        }

        private void frmHoldingMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                if (bLogic.HoldingGeom.Shablon != null)
                   _shablonHandle = _ui.DrawPolygon(bLogic.HoldingGeom.Shablon, 1, eFillStyle.sfsHorizontal);

            }
            else 
                if(e.Control && e.KeyCode== Keys.Y) 
            {
                _ui.SafeDeleteGraphic(ref _shablonHandle);
            }

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            frmPInfo info = new frmPInfo(bLogic);
            info.Show(InitHolding.win32Window);
        }

       



     

      

    }
}
