using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;
using Aran.Geometries;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class NS_Page_new : UserControl
    {
        bool isInit = true;
        //bool go = false;
        FormHelpers.NS_Page_Helper pageHelper;
        String[] TurnSide = new String[] { "Right", "Left" };
        String[] Segment_OCA_OCH = new String[] {"Segment OCA", "Segment OCH"};
        String[] CreatedTrack_OCA_OCH = new String[] { "Created track OCA", "Created track OCH" };
        bool isExtraAddedToD2 = true;

        double d1;
        double d2;
        double d3;

        double extraD2;

        double intermDirMinPrj;
        double intermDirMaxPrj;
        double intermDirMinGeo;
        double intermDirMaxGeo;

        bool isFinalStep;

        double segmentOCA;
        double trackOCA;

        public NS_Page_new(NewSegmentForm nsf, bool isFinalSegmentStep)
        {
            InitializeComponent();
            isFinalStep = isFinalSegmentStep;

            cmbBox_segment_OCA_OCH.DataSource = Segment_OCA_OCH;
            cmbBox_createdTrack_OCA_OCH.DataSource = CreatedTrack_OCA_OCH;

            cmbBox_segment_OCA_OCH.SelectedIndex = 0;
            cmbBox_createdTrack_OCA_OCH.SelectedIndex = 0;

            VMManager.Instance.StepBufferPoly = null;
            lbl_uom1.Text = GlobalVars.unitConverter.DistanceUnit;
            lbl_uom3.Text = GlobalVars.unitConverter.DistanceUnit;
            lbl_uom5.Text = GlobalVars.unitConverter.DistanceUnit;

            lbl_heighSign.Text = GlobalVars.unitConverter.HeightUnit;
            lbl_heighSign2.Text = GlobalVars.unitConverter.HeightUnit;

            pageHelper = new FormHelpers.NS_Page_Helper();
            pageHelper.nsf = nsf;

            VMManager.Instance.MaxDivergenceAngle = ARANMath.C_PI_2 / 2;
            VMManager.Instance.MaxConvergenceAngle = ARANMath.C_PI;

            pageHelper.showCurrentPositionAndDirection();

            VMManager.Instance.InitialDirection = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection), 0)), ARANMath.C_2xPI);

            intermDirMinPrj = ARANMath.Modulus(VMManager.Instance.InitialDirection - VMManager.Instance.MaxDivergenceAngle, ARANMath.C_2xPI);
            intermDirMaxPrj = ARANMath.Modulus(VMManager.Instance.InitialDirection + VMManager.Instance.MaxDivergenceAngle, ARANMath.C_2xPI);
            intermDirMinGeo = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, intermDirMaxPrj));
            intermDirMaxGeo = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, intermDirMinPrj));
            lbl_IntermDirRange.Text = "(" + (intermDirMinGeo - GlobalVars.CurrADHP.MagVar) + " - " + (intermDirMaxGeo - GlobalVars.CurrADHP.MagVar) + ")";
            lbl_FinalDirRange.Text = "(" + 0 + "-" + 360 + ")";

            nmrcUpDown_IntermDir.Minimum = -5;
            nmrcUpDown_IntermDir.Maximum = 365;
            nmrcUpDown_IntermDir.Value = (decimal) (Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection)) - GlobalVars.CurrADHP.MagVar);

            extraD2 = (VMManager.Instance.BankEstablishmentTime + VMManager.Instance.PilotReactionTime) * VMManager.Instance.VM_TASWind;

            nmrcUpDown_d1.DecimalPlaces = 3;
            nmrcUpDown_d1.Increment = (decimal)0.001;
            nmrcUpDown_d2.DecimalPlaces = 3;
            nmrcUpDown_d2.Increment = (decimal)0.001;
            nmrcUpDown_d3.DecimalPlaces = 3;
            nmrcUpDown_d3.Increment = (decimal)0.001;

            nmrcUpDown_d1.Minimum = 0;
            nmrcUpDown_d2.Minimum = 0;
            nmrcUpDown_d3.Minimum = 0;

            if (GlobalVars.settings.DistanceUnit == HorizantalDistanceType.KM)
            {                
                nmrcUpDown_d1.Maximum = 20;
                nmrcUpDown_d2.Maximum = 20;
                nmrcUpDown_d3.Maximum = 20;
            }
            else
            {
                nmrcUpDown_d1.Maximum = 10;
                nmrcUpDown_d2.Maximum = 10;
                nmrcUpDown_d3.Maximum = 10;
            }
            nmrcUpDown_d1.Value = 0;           
            nmrcUpDown_d2.Value = nmrcUpDown_d2.Minimum;
            nmrcUpDown_d3.Value = 0;

            if (isFinalSegmentStep)
            {
                nmrcUpDown_d1.Enabled = false;
                nmrcUpDown_d3.Enabled = false;
                nmrcUpDown_IntermDir.Enabled = false;
                nmrcUpDown_FinalDir.Enabled = false;
                label12.Text = "Final segment length:";
            }

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);
            VMManager.Instance.DivergenceSide = TurnDirection.CW;
            VMManager.Instance.ConvergenceSide = TurnDirection.CW;
            isInit = false;

            trackOCA = VMManager.Instance.MinOCA;
            for (int i = 0; i < VMManager.Instance.TrackSegmentsList.Count; i++)
			{
                if(trackOCA < VMManager.Instance.TrackSegmentsList[i].FlightAltitude)
                    trackOCA = VMManager.Instance.TrackSegmentsList[i].FlightAltitude;      			 
			}

            if (VMManager.Instance.TrackSegmentsList.Count == 0)
            {
                txtBox_track_OCA_OCH.Enabled = false;
                lbl_heighSign2.Enabled = false;
                cmbBox_createdTrack_OCA_OCH.Enabled = false;
            }
            else
            {
                lbl_heighSign2.Enabled = true;
                txtBox_track_OCA_OCH.Enabled = true;
                cmbBox_createdTrack_OCA_OCH.Enabled = true;
                if(cmbBox_createdTrack_OCA_OCH.SelectedIndex == 0)
                    txtBox_track_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(trackOCA, eRoundMode.CEIL).ToString();
                else
                    txtBox_track_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(trackOCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
            }

            //if (!isFinalStep)
            //{
            //    cmbBox_ConvergenceVF.DataSource = VMManager.Instance.ConvergenceVFList;
            //    cmbBox_ConvergenceVF.DisplayMember = "Name";
            //    if (cmbBox_ConvergenceVF.Items.Count > 0)
            //        cmbBox_ConvergenceVF.SelectedIndex = 0;
            //}
            //else
            //{
            //    cmbBox_ConvergenceVF.Enabled = false;
            //}
        }

        private void nmrcUpDown_d1_ValueChanged(object sender, EventArgs e)
        {
            d1 = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_d1.Value);
            if (isInit)
                return;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void nmrcUpDown_d2_ValueChanged(object sender, EventArgs e)
        {
            d2 = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_d2.Value);
            if (isInit)
                return;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void nmrcUpDown_d3_ValueChanged(object sender, EventArgs e)
        {
            d3 = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_d3.Value);
            if (isInit)
                return;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void txtBox_stepDescription_TextChanged(object sender, EventArgs e)
        {
            VMManager.Instance.StepDescription = txtBox_stepDescription.Text;
        }

        private void nmrcUpDown_IntermDir_ValueChanged(object sender, EventArgs e)
        {
            if(nmrcUpDown_IntermDir.Value < 0) 
            {
                nmrcUpDown_IntermDir.Value = nmrcUpDown_IntermDir.Value + 360;
            }
            else if(nmrcUpDown_IntermDir.Value > 359)
            {
                nmrcUpDown_IntermDir.Value = nmrcUpDown_IntermDir.Value - 360;
            }

            double diffAngle = 0;
            if (intermDirMinGeo > intermDirMaxGeo)
            {
                diffAngle = Math.Abs(intermDirMinGeo - intermDirMaxGeo);
                diffAngle /= 2;
                if ((double)nmrcUpDown_IntermDir.Value > intermDirMaxGeo - GlobalVars.CurrADHP.MagVar && (double)nmrcUpDown_IntermDir.Value < ARANMath.Modulus(intermDirMaxGeo - GlobalVars.CurrADHP.MagVar + diffAngle, 360))
                {
                    nmrcUpDown_IntermDir.Value = (decimal)(intermDirMaxGeo- GlobalVars.CurrADHP.MagVar);
                }

                if ((double)nmrcUpDown_IntermDir.Value < intermDirMinGeo - GlobalVars.CurrADHP.MagVar && (double)nmrcUpDown_IntermDir.Value > ARANMath.Modulus(intermDirMinGeo - GlobalVars.CurrADHP.MagVar - diffAngle, 360))
                {
                    nmrcUpDown_IntermDir.Value = (decimal)(intermDirMinGeo- GlobalVars.CurrADHP.MagVar);
                }
            }
            else
            {
                if ((double)nmrcUpDown_IntermDir.Value > intermDirMaxGeo - GlobalVars.CurrADHP.MagVar)
                {
                    nmrcUpDown_IntermDir.Value = (decimal)(intermDirMaxGeo- GlobalVars.CurrADHP.MagVar);
                }

                if ((double)nmrcUpDown_IntermDir.Value < intermDirMinGeo - GlobalVars.CurrADHP.MagVar)
                {
                    nmrcUpDown_IntermDir.Value = (decimal)(intermDirMinGeo- GlobalVars.CurrADHP.MagVar);
                }
            }

            double intermDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar), ARANMath.C_2xPI);
            double alpha = VMManager.Instance.InitialDirection - intermDir;
            alpha = Math.Abs(alpha);
            if (alpha > 0.017) //alpha is greater or equal than 1 degree
            {
                if (!isExtraAddedToD2)
                {
                    if (d2 < extraD2)
                    {
                        d2 = extraD2;
                        d2 = Math.Round(d2);
                        nmrcUpDown_d2.Minimum = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(extraD2, eRoundMode.NONE));
                        nmrcUpDown_d2.Value = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(d2, eRoundMode.NONE));
                    }                    
                    isExtraAddedToD2 = true;
                }
            }
            else
            {
                //d2 -= extraD2;
                //d2 = Math.Round(d2);
                nmrcUpDown_d2.Minimum = 0;
                //nmrcUpDown_d2.Value = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(d2, eRoundMode.NONE));
                isExtraAddedToD2 = false;
            }

            nmrcUpDown_FinalDir.Minimum = -5;
            nmrcUpDown_FinalDir.Maximum = 365;
            nmrcUpDown_FinalDir.Value = nmrcUpDown_IntermDir.Value;

            if (isInit)
                return;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void nmrcUpDown_FinalDir_ValueChanged(object sender, EventArgs e)
        {
            if (nmrcUpDown_FinalDir.Value < 0)
            {
                nmrcUpDown_FinalDir.Value = nmrcUpDown_FinalDir.Value + 360;
            }
            else if (nmrcUpDown_FinalDir.Value > 359)
            {
                nmrcUpDown_FinalDir.Value = nmrcUpDown_FinalDir.Value - 360;
            }

            //start
            double diffAngle = Math.Abs((double) (nmrcUpDown_FinalDir.Value - nmrcUpDown_IntermDir.Value));
            if(diffAngle > 180)
                diffAngle = 360 - diffAngle;

            if (diffAngle >= 135)
            {
                grpBox_ConvergeSide.Enabled = true;
            }
            else
            {
                grpBox_ConvergeSide.Enabled = false;
                diffAngle = (double)(nmrcUpDown_IntermDir.Value - nmrcUpDown_FinalDir.Value);
                //go = false;
                if (diffAngle > 0 || Math.Abs(diffAngle) > 180)
                {
                    VMManager.Instance.ConvergenceSide = TurnDirection.CCW;
                    rdBtn_convergeLeft.Checked = true;
                }
                else
                {
                    VMManager.Instance.ConvergenceSide = TurnDirection.CW;
                    rdBtn_convergeRight.Checked = true;
                }               
            }
            //end

            if (isInit)
                return;

            Point tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, d1);
            double intermDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(tempPnt, (double)nmrcUpDown_IntermDir.Value), ARANMath.C_2xPI);
            double alpha = VMManager.Instance.InitialDirection - intermDir;
            alpha = Math.Abs(alpha);
                        
            double finalDir = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.InitialPosition, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar), ARANMath.C_2xPI);
            double beta = intermDir - finalDir;
            beta = Math.Abs(beta);

            if (alpha <= 0.017)
            {
                if (beta > 0.017)
                {
                    if (!isExtraAddedToD2)
                    {
                        if (d2 < extraD2)
                        {
                            d2 += extraD2;
                            d2 = Math.Round(d2);
                            nmrcUpDown_d2.Minimum = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(extraD2, eRoundMode.NONE));
                            nmrcUpDown_d2.Value = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(d2, eRoundMode.NONE));
                        }
                        isExtraAddedToD2 = true;
                    }

                }
                else
                {
                    if (isExtraAddedToD2)
                    {
                        //d2 -= extraD2;
                        //d2 = Math.Round(d2);
                        nmrcUpDown_d2.Minimum = 0;
                        //nmrcUpDown_d2.Value = (decimal)(GlobalVars.unitConverter.DistanceToDisplayUnits(d2, eRoundMode.NONE));
                        isExtraAddedToD2 = false;
                    }
                }
            }

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;

            
        }
        private void rdBtn_convergeLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdBtn_convergeLeft.Checked)
                return;
            if (rdBtn_convergeLeft.Checked)
                VMManager.Instance.ConvergenceSide = TurnDirection.CCW;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void rdBtn_convergeRight_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdBtn_convergeRight.Checked)
                return;
            if (rdBtn_convergeRight.Checked)
                VMManager.Instance.ConvergenceSide = TurnDirection.CW;

            txtBox_stepDescription.Text = pageHelper.ContructStep(d1, (double)nmrcUpDown_IntermDir.Value + GlobalVars.CurrADHP.MagVar, d2, (double)nmrcUpDown_FinalDir.Value + GlobalVars.CurrADHP.MagVar, d3, isFinalStep, false);

            if (VMManager.Instance.StepBufferPoly != null && !isFinalStep)
                btn_SegmentOCAAssess.Enabled = true;
            txtBox_segment_OCA_OCH.Text = "";
            cmbBox_segment_OCA_OCH.Enabled = false;
        }

        private void btn_SegmentOCAAssess_Click(object sender, EventArgs e)
        {
            int idx;
            List<VM_VerticalStructure> list;

            segmentOCA = Functions.MaxObstacleElevationInPoly(VMManager.Instance.AllObstacles, out list, VMManager.Instance.StepBufferPoly, out idx);

            segmentOCA += VMManager.Instance.MOC;
            if (segmentOCA < VMManager.Instance.MinOCA)
                segmentOCA = VMManager.Instance.MinOCA;

            if(cmbBox_segment_OCA_OCH.SelectedIndex == 0)
                txtBox_segment_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segmentOCA, eRoundMode.CEIL).ToString();
            else
                txtBox_segment_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segmentOCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
            btn_SegmentOCAAssess.Enabled = false;
            cmbBox_segment_OCA_OCH.Enabled = true;
        }

        private void cmbBox_segment_OCA_OCH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_segment_OCA_OCH.SelectedIndex == 0)
                txtBox_segment_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segmentOCA, eRoundMode.CEIL).ToString();
            else
                txtBox_segment_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(segmentOCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
        }

        private void cmbBox_createdTrack_OCA_OCH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_createdTrack_OCA_OCH.SelectedIndex == 0)
                txtBox_track_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(trackOCA, eRoundMode.CEIL).ToString();
            else
                txtBox_track_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(trackOCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
        }
    }
}
