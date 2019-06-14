using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Holding.HoldingSave;

namespace Holding
{
    public partial class frmHoldingSave :Form
    {
        private SegmentPointModel   _segmentPointModel;
        private HoldingPatternModel _hPatternModel;
        private ObstacleAssesmentAreaModel _obstacleAssesModel;
        private HoldingAssessmentModel _hAssessmentModel;
        private int _curStep,_stepCount;

        public frmHoldingSave(Bussines_Logic logic)
        {
            InitializeComponent();
            InitUnits();
            _curStep = 0;
            _stepCount = tabControl1.TabCount;
            double assessedAltitude = 0;
           
            if (logic.HReport.ObstacleReport != null && logic.HReport.ObstacleReport.Count > 0)
            {
                assessedAltitude = logic.HReport.ObstacleReport.Max(rep => rep.Elevation) + logic.ModelAreamParam.CurMoc ;
             
            }

            assessedAltitude = assessedAltitude == 0 ? logic.ModelAreamParam.CurMoc : assessedAltitude;

            _segmentPointModel = new SegmentPointModel(logic.ModelPBN,logic.ModelPtChoise,logic.HoldingGeom.ToleranceArea);
            wayPointModelBindingSource.DataSource = _segmentPointModel.DesignatedPointModel;
            segmentPointModelBindingSource.DataSource = _segmentPointModel;
            
            cmbReportinATC.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.ATCReportingType));
            cmbFlyOver.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.YesNoType));
            cmbRadarGuidance.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.YesNoType));
            cmbRole.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.ReferenceRoleType));
            cmbWayPoint.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.YesNoType));
                        
            _hPatternModel = new HoldingPatternModel(logic.ModelPBN, logic.ModelAreamParam, logic.ProcedureType,logic.HoldingGeom,assessedAltitude);
            holdingPatternModelBindingSource.DataSource = _hPatternModel;

            _obstacleAssesModel = new ObstacleAssesmentAreaModel(logic.HReport,logic.ModelAreamParam,logic.HoldingGeom,assessedAltitude);
            obstacleAssesmentAreaModelBindingSource.DataSource = _obstacleAssesModel;
            cmbOAAType.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.ObstacleAssessmentSurfaceType));

            _hAssessmentModel = new HoldingAssessmentModel(logic.ModelAreamParam, logic.ProcedureType.Time,logic.ProcedureType.WD,logic.ProcedureType.CurDistanceType);
            holdingAssessmentModelBindingSource.DataSource = _hAssessmentModel;
            cmbTurboLenAir.DataSource = Enum.GetValues(typeof(Delib.Classes.Codes.YesNoType));

        }

        private void InitUnits()
        {
            lblPriorFixToleranceUnits.Text = InitHolding.DistanceConverter.Unit;
            lblPostFixToleranceUnit.Text = InitHolding.DistanceConverter.Unit;
            lblHPLowerLimitUnit.Text = InitHolding.HeightConverter.Unit;
            lblHPUpperLimitUnit.Text = InitHolding.HeightConverter.Unit;
            lblHPSpeedLimitUnit.Text = InitHolding.SpeedConverter.Unit;
            lblAssessedAltitudeUnit.Text = InitHolding.HeightConverter.Unit;
            lblLeglengthTowardUnit.Text = InitHolding.DistanceConverter.Unit;
            lblLenglengthAwayUnit.Text = InitHolding.DistanceConverter.Unit;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _curStep++;
            CheckPage();
            tabControl1.SelectedTab = tabControl1.TabPages[_curStep];
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            string message = "";
            if (_segmentPointModel.DesignatedPointModel.Designator == null || _segmentPointModel.DesignatedPointModel.Designator == "")
                message += "Please fill  DesignatedPoint's designator\r\n";

            if (message.Length > 0)
            {
                MessageBox.Show(message, "Fill all fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _hPatternModel.HPattern.holdingPoint = _segmentPointModel.SegmentPt;
            _hAssessmentModel.HAssessMent.holdingPoint = _segmentPointModel.SegmentPt;
            _hAssessmentModel.HAssessMent.assessedHoldingPattern = _hPatternModel.HPattern;
            _hAssessmentModel.HAssessMent.obstacleAssessment.Add(_obstacleAssesModel.ObstacleAssessment);
            _hAssessmentModel.HAssessMent.lowerLimit = _hPatternModel.HPattern.lowerLimit;
            _hAssessmentModel.HAssessMent.lowerLimitReference = _hPatternModel.HPattern.lowerLimitReference;
            _hAssessmentModel.HAssessMent.speedLimit = _hPatternModel.HPattern.speedLimit;
            _hAssessmentModel.HAssessMent.upperLimit = _hPatternModel.HPattern.upperLimit;
            _hAssessmentModel.HAssessMent.upperLimitReference = _hPatternModel.HPattern.upperLimitReference;
            GlobalParams.Database.HoldingQpi.Store(_hAssessmentModel.HAssessMent);
            GlobalParams.Database.HoldingQpi.Commit();
            MessageBox.Show("All values were writen successfully", "Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void CheckPage()
        {
            if (_curStep > 0 && _curStep < _stepCount - 1)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            else if (_curStep == 0)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }
          
            else if (_curStep == _stepCount - 1)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _curStep--;
            CheckPage();
            tabControl1.SelectedTab = tabControl1.TabPages[_curStep];
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _curStep = tabControl1.SelectedIndex;
            CheckPage();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
