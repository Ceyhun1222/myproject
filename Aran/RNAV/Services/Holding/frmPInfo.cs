using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARAN.Common;

namespace Holding
{
    public partial class frmPInfo : Form
    {
        public frmPInfo()
        {
            InitializeComponent();
        }

        public frmPInfo(Bussines_Logic logic)
        {
            InitializeComponent();
            this.LostFocus += new EventHandler(frmPInfo_LostFocus);
            double RV = 0;
            ModelAreaParams modelAreaParam = logic.ModelAreamParam;
            
            double altitude =Common.DeConvertHeight( modelAreaParam.Altitude);
            double ias = Common.DeConvertSpeed(modelAreaParam.Ias);

            if (logic.ProcedureType.PropType == ProcedureType.withHoldingFunc)
                txtHoldingType.Text = "RNAV systems with holding functionality";
            else
                if (logic.ProcedureType.PropType == ProcedureType.withoutHoldingFunc)
                    txtHoldingType.Text = "RNAV systems without holding functionality";
                else
                    if (logic.ProcedureType.PropType == ProcedureType.RNP)
                        txtHoldingType.Text = "RNP holding";

            txtHoldingType.Text = logic.ProcedureType.PropType.ToString();
            txtProtection.Text = logic.ProcedureType.ProtectionType.ToString();
            
            txtATT.Text = Common.ConvertDistance(logic.ModelPtChoise.ATT,roundType.toNearest).ToString("0.0");
            
            txtXTT.Text = Common.ConvertDistance(logic.ModelPtChoise.XTT, roundType.toNearest).ToString("0.0");
            
            txtTAS.Text = ARANMath.IASToTAS(modelAreaParam.Ias,altitude, 15).ToString("0.0");
            txtIAS.Text =modelAreaParam.Ias.ToString("0.0");
           

            txtAircraftCategory.Text = modelAreaParam.Category.ToString();
            
            txtAltitude.Text = modelAreaParam.Altitude.ToString("0.0");
           
            RV = Common.ConvertDistance(Shablons.TurnRadius(ias, altitude, 15),roundType.toNearest);
            txtRadius.Text = RV.ToString("0.0");
            if (logic.ProcedureType.CurDistanceType == DistanceType.Time)
            {
                txtTime.Text = logic.ProcedureType.Time.ToString("0.0");
                lblTimeUnit.Text = "Min";
                double length = ARANMath.IASToTAS(ias, altitude, 15) * logic.ProcedureType.Time * 60;
                txtInboundLegSpan.Text = Common.ConvertDistance(length, roundType.toNearest).ToString("0.0");
            }
            else
            {
                txtTime.Text = logic.ProcedureType.WD.ToString("0.0");
                lblTimeUnit.Text = InitHolding.DistanceConverter.Unit;
                lblTime.Text = "WD:";
                
                txtInboundLegSpan.Text = Math.Sqrt(logic.ProcedureType.WD * logic.ProcedureType.WD - 4 *RV * RV).ToString("0.0");
                
            }

            txtInboundCourse.Text = modelAreaParam.Radial.ToString("0");

            txtMOC.Text = modelAreaParam.CurMoc.ToString("0.0");
                        
           double  H = altitude / 1000.0;
		   double w = (12.0 * H + 87.0)/3.6;

           txtWindSpeed.Text = Common.ConvertSpeed_(w, roundType.toNearest).ToString("0.0");
            

            lblAltitudeUnit.Text = lblMocUnit.Text = InitHolding.HeightConverter.Unit;
            lblATTUnit.Text = lblXTTUnit.Text = lblInboundLegUnit.Text = lblInboundLegUnit.Text = lblRadiusUnit.Text = InitHolding.DistanceConverter.Unit;
            lblIASUnit.Text = lblTASUnit.Text = lblWindSpeedUnit.Text = InitHolding.SpeedConverter.Unit;
            lblInboundCourseUnit.Text = "°";
        }

        void frmPInfo_LostFocus(object sender, EventArgs e)
        {
            this.Close();
        }


        private void frmPInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
