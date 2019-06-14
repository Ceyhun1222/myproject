using System;
using System.Drawing;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Holding.Models;

namespace Holding.Forms
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

            double altitude = Common.DeConvertHeight(modelAreaParam.Altitude);
            double ias = Common.DeConvertSpeed(modelAreaParam.Ias);

            if (logic.ProcedureType.PropType == ProcedureType.withHoldingFunc)
                lblHoldingTypeText.Text = "RNAV systems with holding functionality";
            else
                if (logic.ProcedureType.PropType == ProcedureType.withoutHoldingFunc)
                    lblHoldingTypeText.Text = "RNAV systems without holding functionality";
                else
                    if (logic.ProcedureType.PropType == ProcedureType.RNP)
                        lblHoldingTypeText.Text = "RNP holding";

            lblProtectionText.Text = logic.ProcedureType.ProtectionType.ToString();

            lblATTText.Text = Common.ConvertDistance(logic.ModelPtChoise.ATT, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;

            lblXTTText.Text = Common.ConvertDistance(logic.ModelPtChoise.XTT, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;

            lblTASText.Text =Common.ConvertSpeed_(ARANMath.IASToTASForRnav(Common.DeConvertSpeed( modelAreaParam.Ias), altitude, 15),roundType.toNearest) + " " + InitHolding.SpeedConverter.Unit;
            
            lblIASText.Text = modelAreaParam.Ias + " " + InitHolding.SpeedConverter.Unit;

            if (modelAreaParam.Category!=null)
                lblAircaftCategoryText.Text = modelAreaParam.Category.ToString();

            lblAltitudeText.Text = modelAreaParam.Altitude + " " + InitHolding.HeightConverter.Unit; 

            RV = Shablons.TurnRadius(ias, altitude, 15);
            lblRadiusText.Text =Common.ConvertDistance(RV,roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit ;
            if (logic.ProcedureType.CurDistanceType == DistanceType.Time)
            {
                lblTimeText.Text = logic.ProcedureType.Time + " Min";
                double length = ARANMath.IASToTASForRnav(ias, altitude, 15) * logic.ProcedureType.Time * 60;
                lblInboundLegText.Text = Common.ConvertDistance(length, roundType.toNearest)+" "+InitHolding.DistanceConverter.Unit;
            }
            else
            {
                lblTimeText.Text = logic.ProcedureType.WD + " " + InitHolding.DistanceConverter.Unit;
                lblTime.Text = "WD:";
                double wd = Common.DeConvertDistance(logic.ProcedureType.WD);

                lblInboundLegText.Text =Common.ConvertDistance(Math.Sqrt(wd * wd - 4 * RV * RV),roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;

            }

            lblInboundCourseText.Text = modelAreaParam.Radial + " " + "°";

            lblMocText.Text = modelAreaParam.CurMoc+ " "+InitHolding.HeightConverter.Unit ;

            double H = altitude / 1000.0;
            double w = (12.0 * H + 87.0) / 3.6;

            lblWindSpeedText.Text = Common.ConvertSpeed_(w, roundType.toNearest)+" "+InitHolding.SpeedConverter.Unit;
        }

        void frmPInfo_LostFocus(object sender, EventArgs e)
        {
            //this.Close();
        }


        private void frmPInfo_Load(object sender, EventArgs e)
        {
            lblHoldingTypeText.BackColor = Color.White;
            lblATTText.BackColor = Color.White;
            lblXTTText.BackColor = Color.White;
            lblAircaftCategoryText.BackColor = Color.White;
            lblAltitudeText.BackColor = Color.White;
            lblIASText.BackColor = Color.White;
            lblInboundCourseText.BackColor = Color.White;
            lblInboundLegText.BackColor = Color.White;
            lblMocText.BackColor = Color.White;
            lblProtectionText.BackColor = Color.White;
            lblRadiusText.BackColor = Color.White;
            lblTASText.BackColor = Color.White;
            lblTimeText.BackColor = Color.White;
            lblWindSpeedText.BackColor = Color.White;
        }

    }
}
