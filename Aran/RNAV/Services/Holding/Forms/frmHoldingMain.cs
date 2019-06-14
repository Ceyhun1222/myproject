using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Holding.Models;
using Aran.PANDA.Rnav.Holding.Properties;
using SignificantPointChoice = Aran.Aim.SignificantPointChoice;

namespace Holding.Forms
{
    public partial class frmHoldingMain : Form
    {
        private DBModule _database;
        private SpatialReferenceOperation _spatialOperation;
        private Bussines_Logic bLogic;
        private ModelWizardChange _modelWizard;

        private BaseArea baseArea;
        private Aran.AranEnvironment.IAranGraphics _ui;
        private Aran.Geometries.Operators.GeometryOperators _geomOperators;

        private frmDrawTest _settingsForm;
        private bool _pointPickerClicked;
        private IScreenCapture _screenCapture;

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

                _ui = GlobalParams.UI;
                
                bLogic = new Bussines_Logic(contCollection,
                    GlobalParams.AranSettings.Radius, 5000, ARANMath.DegToRad(270), 100, TurnDirection.CW, ConditionType.normal);
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

                udAltitude.Increment = (decimal)InitHolding.HeightPrecision;
                udAltitude.DecimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)InitHolding.HeightPrecision)[3])[2];

                udWD.DecimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)InitHolding.DistancePrecision)[3])[2];
                udWD.Increment = (decimal)InitHolding.DistancePrecision;

                udIas.Increment = (decimal)InitHolding.SpeedPrecision;
                udIas.DecimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)InitHolding.SpeedPrecision)[3])[2];

                GlobalParams.AranMapToolMenuItem = new AranTool();
                GlobalParams.AranMapToolMenuItem.Cursor = System.Windows.Forms.Cursors.Cross;
                GlobalParams.AranMapToolMenuItem.Visible = true;
                GlobalParams.AranMapToolMenuItem.MouseClickedOnMap +=
                    new MouseClickedOnMapEventHandler(OnMouseClickedOnMap);
                GlobalParams.AranEnvironment.AranUI.AddMapTool(GlobalParams.AranMapToolMenuItem);

                _screenCapture = GlobalParams.AranEnvironment.GetScreenCapture(FeatureType.HoldingPattern.ToString());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), Resources.Holding_Caption);
            }

        }

        private void frmHoldingMain_Load(object sender, EventArgs e)
        {
            _database = GlobalParams.Database;
            _ui = GlobalParams.UI;
            _spatialOperation = GlobalParams.SpatialRefOperation;
            _geomOperators = GlobalParams.GeomOperators;
            baseArea = new BaseArea();

            this.Text = "RNAV Holding 5.1 Aerodrome: " + InitHolding.CurAdhp.Name;

            lblIas.Text = "IAS";

            udAltitude.Increment = (decimal)InitHolding.HeightPrecision;
            lblAltitudeUnit.Text = InitHolding.HeightConverter.Unit;

            udIas.Increment = (decimal)InitHolding.SpeedPrecision;
            lblIasType.Text = InitHolding.SpeedConverter.Unit;

            lblWd.Text = "WD(" + InitHolding.DistanceConverter.Unit + ")";

            lblMOC.Text = InitHolding.HeightConverter.Unit;
            dGridNavaid.Columns[2].HeaderText = "Distance (" + InitHolding.DistanceConverter.Unit + " )";
            dGridNavaid.Columns[3].HeaderText = "Azimuth ( ° )";
            cmbWayPointChoise_SelectedIndexChanged(null, null);

        }

        #region Choose Points

        private void CreateUIPoint(Aran.Geometries.Point ptPrj)
        {
            bLogic.ModelPtChoise.DeletePoint();
            Aran.Geometries.Point ptGeo = GlobalParams.SpatialRefOperation.ToGeo(ptPrj);
            if (bLogic.ModelPtChoise.PointDistanceControl(ptGeo))
            {
                bLogic.ModelPtChoise.DrawPoint(ptPrj);
                bLogic.ModelPtChoise.CurPoint = ptGeo;
                //pointPicker2.Longitude = ptGeo.Y;
                //pointPicker2.Latitude = ptGeo.X;
            }
            else
                bLogic.ModelPtChoise.CurPoint = null;
        }
      
        internal void OnMouseClickedOnMap(object sender, MapMouseEventArg e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                double latitude = e.Y;
                double longtitude = e.X;
                var ptGeo =GlobalParams.SpatialRefOperation.ToGeo(new Aran.Geometries.Point(longtitude, latitude));
                pointPicker1.Latitude = ptGeo.Y;
                pointPicker1.Longitude = ptGeo.X;
                Aran.Geometries.Point ptPrj = new Aran.Geometries.Point(longtitude, latitude);
                CreateUIPoint(ptPrj);
            }
        }
        
        #endregion

        private void btnBaseArea_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bLogic.CalculateBaseArea();
            this.Cursor = Cursors.Default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _screenCapture.Save(this);
            this.Controls.Remove(_modelWizard.CurContainer);
            _modelWizard.CurrentWizard++;
            this.Controls.Add(_modelWizard.CurContainer);

            if (HoldingService.ToolClicked)
            {
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
            }
        }

        private void btnPrevius_Click(object sender, EventArgs e)
        {
            _screenCapture.Delete();
            this.Controls.Remove(_modelWizard.CurContainer);
            _modelWizard.CurrentWizard--;
            this.Controls.Add(_modelWizard.CurContainer);

            if (HoldingService.ToolClicked)
                GlobalParams.AranEnvironment.AranUI.SetCurrentTool(HoldingService.ByClickToolButton);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                bLogic.OnListChange(this, new ListChangedEventArgs(ListChangedType.ItemChanged, e.RowIndex));//list.BindList[e.RowIndex].Checked = !list.BindList[e.RowIndex].Checked;
                holdingNavListBindingSource.ResetBindings(false);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bLogic.CreateReport();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Resources.Holding_Caption);

            }
        }

        private void btnDmeCoverage_Click(object sender, EventArgs e)
        {
            bLogic.CreateDmeCoverage();
        }

        private void frmHoldingMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (bLogic != null)
                    bLogic.Dispose();
              //  this.Dispose(true);
            }
            catch (Exception)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _screenCapture.Save(this);
                bLogic.CreateSave(_screenCapture);
            }
            catch (Exception exception)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(exception);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            frmPInfo info = new frmPInfo(bLogic);
            info.Show(InitHolding.Win32Window);
        }

        private void udIas_ValueChanged(object sender, EventArgs e)
        {
            double val = (double)udIas.Value;
            if ((bLogic.ModelAreamParam.MinIas == bLogic.ModelAreamParam.MaxIas) ||
                                        (val < bLogic.ModelAreamParam.MinIas) ||
                                        (val > bLogic.ModelAreamParam.MaxIas))
                bLogic.ModelAreamParam.ChangeIas(bLogic.ModelAreamParam.Ias);
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            if (_settingsForm == null || _settingsForm.IsDisposed)
                _settingsForm = new frmDrawTest(bLogic);
            
            if (!_settingsForm.Visible)
                _settingsForm.Show(InitHolding.Win32Window);
        }

        private void ckbDrawDME_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            bLogic.HoldingNavaidOperation.DrawDME = ckbDrawDME.Checked;
            this.Cursor = Cursors.Default;
        }

        private void cmbWayPointChoise_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalParams.AranEnvironment.AranUI.SetPanTool();
            if (bLogic.ModelPtChoise.PointChoise.Choice == SignificantPointChoice.DesignatedPoint)
            {
                lblNavaid.Visible = cmbNavaid.Visible = false;
                lblDesignatedPt.Visible = cmbPointList.Visible = true;
            }
            else if (bLogic.ModelPtChoise.PointChoise.Choice == SignificantPointChoice.Navaid)
            {
                lblNavaid.Visible = cmbNavaid.Visible = true;
                lblDesignatedPt.Visible = cmbPointList.Visible = false;
            }
            else if (bLogic.ModelPtChoise.PointChoise.Choice == SignificantPointChoice.AixmPoint)
            {
                if (pointPicker1.ByClick)
                    GlobalParams.AranEnvironment.AranUI.SetCurrentTool(GlobalParams.AranMapToolMenuItem);

                lblNavaid.Visible = cmbNavaid.Visible = false;
                lblDesignatedPt.Visible = cmbPointList.Visible = false;
                pointPicker1.Visible = true;
            }
        }

        private void cmbPointList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (bLogic!=null)
                bLogic.ModelPtChoise.CurSigPoint = (DesignatedPoint)cmbPointList.SelectedItem;
        }

        private void cmbNavaid_SelectedValueChanged(object sender, EventArgs e)
        {
            if (bLogic != null)
                bLogic.ModelPtChoise.CurNavaid = (Navaid)cmbNavaid.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // NativeMethods.HtmlHelp(0, InitHolding.HelpFile, InitHolding.HhHelpContext, 440);
            Help.ShowHelp(this, "RNAV.chm", HelpNavigator.Topic, "rnav_holding.htm");
        }

        private void pointPicker1_ByClickChanged(object sender, EventArgs e)
        {
            _pointPickerClicked = !_pointPickerClicked;
            
            if (_pointPickerClicked)
            {
                GlobalParams.AranEnvironment.AranUI.SetCurrentTool(GlobalParams.AranMapToolMenuItem);
            }
            else
            {
                GlobalParams.AranEnvironment.AranUI.SetPanTool();
                pointPicker1.ByClick = false;
            }
        }

        private void pointPicker1_LatitudeChanged(object sender, EventArgs e)
        {
            if (_pointPickerClicked)
                return;

            var ptGeo = new Aran.Geometries.Point(pointPicker1.Longitude, pointPicker1.Latitude);
            var ptPrj = GlobalParams.SpatialRefOperation.ToPrj(ptGeo);
            
            if (bLogic.ModelPtChoise.PointDistanceControl(ptGeo))
            {
                bLogic.ModelPtChoise.DrawPoint(ptPrj);
                bLogic.ModelPtChoise.CurPoint = ptGeo;
            }
            else
                bLogic.ModelPtChoise.CurPoint = null;
        }

        private void aircraftCategoriesBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}

