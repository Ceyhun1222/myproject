using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;
using Aran.Panda.Constants;
using Aran.Geometries.Operators;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class MF_Page3 : UserControl
    {
        public int prevPageIndex;
        public int nextPageIndex;
        private bool _callEventHandler;
        FormHelpers.MF_Page3_Helper pageHelper;
        bool isInit = true;
        bool updateFinalIAS = true;

        String[] OCA_OCH = new String[] { "OCA", "OCH" };

        bool isCircuitObstaclesAssessed = false;

        public MF_Page3()
        {
            InitializeComponent();
            pageHelper = new FormHelpers.MF_Page3_Helper();

            lbl_kmhSign1.Text = GlobalVars.unitConverter.SpeedUnit;
            lbl_kmhSign2.Text = GlobalVars.unitConverter.SpeedUnit;
            lbl_kmhSign3.Text = GlobalVars.unitConverter.SpeedUnit;
            lbl_kmhSign4.Text = GlobalVars.unitConverter.SpeedUnit;
            lbl_kmSign1.Text = GlobalVars.unitConverter.DistanceUnit;
            
            lbl_meterSign.Text = GlobalVars.unitConverter.HeightUnit;
            lbl_meterSign2.Text = GlobalVars.unitConverter.HeightUnit;
            lbl_meterSign3.Text = GlobalVars.unitConverter.HeightUnit;
            lbl_hightSign.Text = GlobalVars.unitConverter.HeightUnit;

            nmrcUpDown_DescentGradient.DecimalPlaces = 1;
            nmrcUpDown_DescentGradient.Increment = (decimal)0.1;

            cmbBox_IAS2TAS_OCA_OCH.DataSource = OCA_OCH;
            cmbBox_IAS2TAS_OCA_OCH.SelectedIndex = 0;
        }

        private void MF_Page3_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if(prevPageIndex == 0)
                {
                    switch (VMManager.Instance.Category)
                    {
                        case 0: // Cat A
                            VMManager.Instance.CorridorSemiWidth = 1400;
                            VMManager.Instance.MaxRateOfDescent = 200;
                            VMManager.Instance.MinRateOfDescent = 120;
                            VMManager.Instance.FinalSegmentIAS = 51.38888889; // 185 km/h
                            VMManager.Instance.MinFinalSegmentIAS = 36.11111111; // 130 km/h
                            VMManager.Instance.MaxFinalSegmentIAS = 51.38888889; // 185 km/h
                            VMManager.Instance.VM_IAS = 51.38888889; // 185 km/h
                            VMManager.Instance.Min_VM_IAS = 36.11111111; // 130 km/h
                            VMManager.Instance.Max_VM_IAS = 51.38888889; // 185 km/h     
                            VMManager.Instance.MinVisibilityDistance = 1900;
                            break;
                        case 1: // Cat B
                            VMManager.Instance.CorridorSemiWidth = 1500;
                            VMManager.Instance.MaxRateOfDescent = 200;
                            VMManager.Instance.MinRateOfDescent = 120;
                            VMManager.Instance.FinalSegmentIAS = 66.66666667; // 240 km/h
                            VMManager.Instance.MinFinalSegmentIAS = 43.05555556; // 155 km/h
                            VMManager.Instance.MaxFinalSegmentIAS = 66.66666667; // 240 km/h
                            VMManager.Instance.VM_IAS = 69.44444444; // 250 km/h
                            VMManager.Instance.Min_VM_IAS = 43.05555556; // 155 km/h
                            VMManager.Instance.Max_VM_IAS = 69.44444444; // 250 km/h
                            VMManager.Instance.MinVisibilityDistance = 2800;
                            break;
                        case 2: // Cat C
                            VMManager.Instance.CorridorSemiWidth = 1800;
                            VMManager.Instance.MaxRateOfDescent = 305;
                            VMManager.Instance.MinRateOfDescent = 180;
                            VMManager.Instance.FinalSegmentIAS = 81.94444444; // 295 km/h
                            VMManager.Instance.MinFinalSegmentIAS = 59.72222222; // 215 km/h
                            VMManager.Instance.MaxFinalSegmentIAS = 81.94444444; // 295 km/h
                            VMManager.Instance.VM_IAS = 93.05555556; // 335 km/h
                            VMManager.Instance.Min_VM_IAS = 59.72222222; // 215 km/h
                            VMManager.Instance.Max_VM_IAS = 93.05555556; // 335 km/h
                            VMManager.Instance.MinVisibilityDistance = 3700;
                            break;
                        case 3: // Cat D
                            VMManager.Instance.CorridorSemiWidth = 2100;
                            VMManager.Instance.MaxRateOfDescent = 305;
                            VMManager.Instance.MinRateOfDescent = 180;
                            VMManager.Instance.FinalSegmentIAS = 95.83333333; // 345 km/h
                            VMManager.Instance.MinFinalSegmentIAS = 66.66666667; // 240 km/h
                            VMManager.Instance.MaxFinalSegmentIAS = 95.83333333; // 345 km/h
                            VMManager.Instance.VM_IAS = 105.55555556; // 380 km/h
                            VMManager.Instance.Min_VM_IAS = 66.66666667; // 240 km/h
                            VMManager.Instance.Max_VM_IAS = 105.55555556; // 380 km/h
                            VMManager.Instance.MinVisibilityDistance = 4600;
                            break;
                        case 5: // Cat E
                            VMManager.Instance.CorridorSemiWidth = 2600;
                            VMManager.Instance.MaxRateOfDescent = 305;
                            VMManager.Instance.MinRateOfDescent = 180;
                            VMManager.Instance.FinalSegmentIAS = 118.05555556; // 425 km/h
                            VMManager.Instance.MinFinalSegmentIAS = 79.16666667; // 285 km/h
                            VMManager.Instance.MaxFinalSegmentIAS = 118.05555556; // 425 km/h
                            VMManager.Instance.VM_IAS = 123.61111111; // 445 km/h
                            VMManager.Instance.Min_VM_IAS = 79.16666667; // 285 km/h
                            VMManager.Instance.Max_VM_IAS = 123.61111111; // 445 km/h
                            VMManager.Instance.MinVisibilityDistance = 6500;
                            break;
                    }

                    VMManager.Instance.MinDescentGradient = 5.2;
                    VMManager.Instance.MaxDescentGradient = 9.9;

                    

                    //VMManager.Instance.VisibilityDistance = VMManager.Instance.MinVisibilityDistance;
                    VMManager.Instance.BankEstablishmentTime = 3; //s
                    VMManager.Instance.PilotReactionTime = 3; //s
                    VMManager.Instance.ISA = 15;
                    VMManager.Instance.VM_OCA = VMManager.Instance.CA_OCA;
                    if (cmbBox_IAS2TAS_OCA_OCH.SelectedIndex == 0)
                        txtBox_IAS2TAS_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.VM_OCA, eRoundMode.CEIL).ToString();
                    else
                        txtBox_IAS2TAS_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.VM_OCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();

                    VMManager.Instance.FinalSegmentTAS = ARANMath.IASToTAS(VMManager.Instance.FinalSegmentIAS, VMManager.Instance.VM_OCA, VMManager.Instance.ISA + 15);
                    VMManager.Instance.VM_TAS = ARANMath.IASToTAS(VMManager.Instance.VM_IAS, VMManager.Instance.VM_OCA, VMManager.Instance.ISA + 15);
                    VMManager.Instance.VM_TASWind = VMManager.Instance.VM_TAS + GlobalVars.pansopsCoreDatabase.ArVisualWs.Value;

                    txtBox_finalSegmentLength.Text = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.FinalSegmentLength, eRoundMode.NONE), 3).ToString();
                    VMManager.Instance.VM_BankAngle = ARANMath.DegToRad(25);
                    VMManager.Instance.VM_TurnRadius = Math.Round(ARANMath.BankToRadius(VMManager.Instance.VM_BankAngle, VMManager.Instance.VM_TASWind), 0);
                    //txtBox_MaxAreaOCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.CA_OCH, eRoundMode.NERAEST).ToString();
                    //double minFST = ((VMManager.Instance.MinOCH + GlobalVars.CurrADHP.Elev) * 60 / VMManager.Instance.MaxRateOfDescent);
                    //VMManager.Instance.FinalSegmentTime = Math.Round(minFST, 0);
                    VMManager.Instance.FinalSegmentTime = 30;
                    VMManager.Instance.FinalSegmentLength = Math.Round((VMManager.Instance.FinalSegmentTime * VMManager.Instance.FinalSegmentTASWind), 0);
                    VMManager.Instance.FinalSegmentLength = Math.Round((VMManager.Instance.FinalSegmentTime * VMManager.Instance.VM_TASWind), 0);
                    _callEventHandler = false;
                    nmrcUpDown_visualManoeuvringIAS.Minimum = (decimal)GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.Min_VM_IAS, eRoundMode.NERAEST);
                    nmrcUpDown_visualManoeuvringIAS.Maximum = (decimal)GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.Max_VM_IAS, eRoundMode.NERAEST);
                    _callEventHandler = true;
                    if (nmrcUpDown_visualManoeuvringIAS.Value != nmrcUpDown_visualManoeuvringIAS.Maximum)
                        nmrcUpDown_visualManoeuvringIAS.Value = nmrcUpDown_visualManoeuvringIAS.Maximum;
                    else
                        nmrcUpDown_visualManoeuvringIAS_ValueChanged(nmrcUpDown_visualManoeuvringIAS, null);
                

                    lbl_visualManoeuvringIASrange.Text = "(" + nmrcUpDown_visualManoeuvringIAS.Minimum + " - " + nmrcUpDown_visualManoeuvringIAS.Maximum + " " + GlobalVars.unitConverter.SpeedUnit + ")";
                    _callEventHandler = false;
                    nmrcUpDown_FinalSegmentIAS.Minimum = (decimal)GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.MinFinalSegmentIAS, eRoundMode.NERAEST);
                    nmrcUpDown_FinalSegmentIAS.Maximum = (decimal)GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.MaxFinalSegmentIAS, eRoundMode.NERAEST);
                    _callEventHandler = true;
                    if (nmrcUpDown_FinalSegmentIAS.Value != nmrcUpDown_FinalSegmentIAS.Maximum)
                        nmrcUpDown_FinalSegmentIAS.Value = nmrcUpDown_FinalSegmentIAS.Maximum;
                    else
                        nmrcUpDown_FinalSegmentIAS_ValueChanged(nmrcUpDown_FinalSegmentIAS, null);
                    lbl_finalSegmentIASrange.Text = "(" + nmrcUpDown_FinalSegmentIAS.Minimum + " - " + nmrcUpDown_FinalSegmentIAS.Maximum + " " + GlobalVars.unitConverter.SpeedUnit + ")";
                    if ((decimal) VMManager.Instance.FinalSegmentTime == 0)
                        nmrcUpDown_FinalSegmentTime_ValueChanged(nmrcUpDown_FinalSegmentTime, null);
                    else
                        nmrcUpDown_FinalSegmentTime.Minimum = (decimal) VMManager.Instance.FinalSegmentTime;
                    nmrcUpDown_FinalSegmentTime.Maximum = 60;
                    if(nmrcUpDown_FinalSegmentTime.Value != nmrcUpDown_FinalSegmentTime.Maximum)
                    nmrcUpDown_FinalSegmentTime.Value = (decimal) VMManager.Instance.FinalSegmentTime;
                    isInit = false;

                    nmrcUpDown_RateOfDescent.Minimum = (decimal) VMManager.Instance.MinRateOfDescent;
                    nmrcUpDown_RateOfDescent.Maximum = (decimal) VMManager.Instance.MaxRateOfDescent;
                    nmrcUpDown_RateOfDescent.Value = nmrcUpDown_RateOfDescent.Maximum;

                    nmrcUpDown_DescentGradient.Minimum = (decimal) VMManager.Instance.MinDescentGradient;
                    nmrcUpDown_DescentGradient.Maximum = (decimal)VMManager.Instance.MaxDescentGradient;

                    //Here is the logic of midpoint search algorithm

                    bool isWithin = false;
                    double maxVal = (double)nmrcUpDown_visualManoeuvringIAS.Maximum;
                    double minVal = (double)nmrcUpDown_visualManoeuvringIAS.Minimum;
                    double prevMaxVal = 0;
                    double prevMinVal = 0;
                    double val = -1;

                    VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
                    if (VMManager.Instance.GeomOper.Contains(VMManager.Instance.LefthandCircuitPolygon) && VMManager.Instance.GeomOper.Contains(VMManager.Instance.RighthandCircuitPolygon))
                        isWithin = true;
                    else
                        isWithin = false;

                    updateFinalIAS = false;
                    if (!isWithin)
                        while (true)
                        {
                            if (maxVal == minVal)
                            {
                                val = maxVal;
                                break;
                            }
                            else if (prevMinVal == minVal && prevMaxVal == maxVal) {
                                val = minVal;
                                break;
                            }
                            else
                                val = Math.Floor((maxVal - minVal) / 2 + minVal);

                            nmrcUpDown_visualManoeuvringIAS.Value = (decimal)val;

                            prevMaxVal = maxVal;
                            prevMinVal = minVal;

                            if (VMManager.Instance.GeomOper.Contains(VMManager.Instance.LefthandCircuitPolygon) && VMManager.Instance.GeomOper.Contains(VMManager.Instance.RighthandCircuitPolygon))
                                minVal = val;
                            else
                                maxVal = val;
                        }
                    VMManager.Instance.GeomOper = new JtsGeometryOperators();
                    updateFinalIAS = true;
                    //nmrcUpDown_visualManoeuvringIAS_ValueChanged(this, null);
                    if(val > -1)
                        nmrcUpDown_visualManoeuvringIAS.Value = (decimal)val;

                    
                }
                else
                    VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
            }
            else if (!this.Visible)
            {
                if (nextPageIndex == 0)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RighthandCircuitCentrelineElement);
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RighthandCircuitPolygonElement);
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LefthandCircuitCentrelineElement);
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LefthandCircuitPolygonElement);
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentCentrelineElement);
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentPolygonElement);
                }
                else
                {
                    if (!isCircuitObstaclesAssessed)
                    {
                        if (!isInit)
                        {
                            pageHelper.assessCircuitObstacles();
                            pageHelper.DrawCircuits();
                        }
                    }
                }
                VMManager.Instance.GeomOper = new JtsGeometryOperators();
            }                
        }

        private void nmrcUpDown_visualManoeuvringIAS_ValueChanged(object sender, EventArgs e)
        {
            if (_callEventHandler)
            {
                VMManager.Instance.VM_IAS = GlobalVars.unitConverter.SpeedToInternalUnits((double)nmrcUpDown_visualManoeuvringIAS.Value);                
                VMManager.Instance.VM_TAS = ARANMath.IASToTAS(VMManager.Instance.VM_IAS, VMManager.Instance.VM_OCA, VMManager.Instance.ISA + 15);
                VMManager.Instance.VM_TASWind = VMManager.Instance.VM_TAS + GlobalVars.pansopsCoreDatabase.ArVisualWs.Value;
                txtBox_visualManoeuvringTASWind.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.VM_TASWind, eRoundMode.NERAEST).ToString();
                VMManager.Instance.VM_TurnRadius = Math.Round(ARANMath.BankToRadius(VMManager.Instance.VM_BankAngle, VMManager.Instance.VM_TASWind), 0);
                if (nmrcUpDown_visualManoeuvringIAS.Value < nmrcUpDown_FinalSegmentIAS.Value)
                {
                    nmrcUpDown_FinalSegmentIAS.Value = nmrcUpDown_visualManoeuvringIAS.Value;
                    return;
                }

                //VMManager.Instance.FinalSegmentLength = Math.Round(VMManager.Instance.FinalSegmentTime * VMManager.Instance.VM_TASWind, 0);
                //txtBox_finalSegmentLength.Text = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.FinalSegmentLength, eRoundMode.NONE), 3).ToString();


                double maxDistance;
                if (!isInit)
                {
                    pageHelper.ConstructCircuits(out maxDistance);
                    btn_circuitsObstacleAssesment.Enabled = true;
                    isCircuitObstaclesAssessed = false;
                    pageHelper.DrawCircuits();

                    if (VMManager.Instance.GeomOper.CurrentGeometry == null)
                    {
                        VMManager.Instance.GeomOper = new JtsGeometryOperators();
                        VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
                    }

                    if (!VMManager.Instance.GeomOper.Contains(VMManager.Instance.LefthandCircuitPolygon) || !VMManager.Instance.GeomOper.Contains(VMManager.Instance.RighthandCircuitPolygon))
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Black;
                    }
                }

                VMManager.Instance.FinalSegmentStartPointAltitude = VMManager.Instance.FinalSegmentLength * VMManager.Instance.DescentGradient / 100 + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
                txtBox_FinalSegmentStartPointHeight.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
                txtBox_FinalSegmentStartPointAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude, eRoundMode.CEIL).ToString();
            }
        }

        private void nmrcUpDown_FinalSegmentIAS_ValueChanged(object sender, EventArgs e)
        {
            if (_callEventHandler)
            {
                VMManager.Instance.FinalSegmentIAS = GlobalVars.unitConverter.SpeedToInternalUnits((double)nmrcUpDown_FinalSegmentIAS.Value);
                VMManager.Instance.FinalSegmentTAS = ARANMath.IASToTAS(VMManager.Instance.FinalSegmentIAS, VMManager.Instance.VM_OCA, VMManager.Instance.ISA + 15);
                VMManager.Instance.FinalSegmentTASWind = VMManager.Instance.FinalSegmentTAS + GlobalVars.pansopsCoreDatabase.ArVisualWs.Value;
                txtBox_finalSegmentTASWind.Text = GlobalVars.unitConverter.SpeedToDisplayUnits(VMManager.Instance.FinalSegmentTASWind, eRoundMode.NERAEST).ToString();
                VMManager.Instance.FinalSegmentLength = Math.Round(VMManager.Instance.FinalSegmentTime * VMManager.Instance.FinalSegmentTASWind, 0);
                //VMManager.Instance.FinalSegmentLength = Math.Round(VMManager.Instance.FinalSegmentTime * VMManager.Instance.VM_TASWind, 0);
                txtBox_finalSegmentLength.Text = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.FinalSegmentLength, eRoundMode.NONE), 3).ToString();

                if (nmrcUpDown_visualManoeuvringIAS.Value < nmrcUpDown_FinalSegmentIAS.Value)
                {
                    nmrcUpDown_visualManoeuvringIAS.Value = nmrcUpDown_FinalSegmentIAS.Value;
                    return;
                }

                double maxDistance;
                if (!isInit)
                {
                    pageHelper.ConstructCircuits(out maxDistance);
                    btn_circuitsObstacleAssesment.Enabled = true;
                    isCircuitObstaclesAssessed = false;
                    pageHelper.DrawCircuits();

                    if (VMManager.Instance.GeomOper.CurrentGeometry == null)
                    {
                        VMManager.Instance.GeomOper = new JtsGeometryOperators();
                        VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
                    }

                    if (!VMManager.Instance.GeomOper.Contains(VMManager.Instance.LefthandCircuitPolygon) || !VMManager.Instance.GeomOper.Contains(VMManager.Instance.RighthandCircuitPolygon))
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Black;
                    }
                }
                VMManager.Instance.FinalSegmentStartPointAltitude = VMManager.Instance.FinalSegmentLength * VMManager.Instance.DescentGradient / 100 + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
                txtBox_FinalSegmentStartPointAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude, eRoundMode.CEIL).ToString();
            }
        }

        private void nmrcUpDown_FinalSegmentTime_ValueChanged(object sender, EventArgs e)
        {
            if (_callEventHandler)
            {
                VMManager.Instance.FinalSegmentTime = (double)nmrcUpDown_FinalSegmentTime.Value;
                VMManager.Instance.FinalSegmentLength = Math.Round(VMManager.Instance.FinalSegmentTime * VMManager.Instance.FinalSegmentTASWind, 0);
                VMManager.Instance.FinalSegmentLength = Math.Round(VMManager.Instance.FinalSegmentTime * VMManager.Instance.VM_TASWind, 0);
                txtBox_finalSegmentLength.Text = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.FinalSegmentLength, eRoundMode.NONE), 3).ToString();
                double maxDistance;
                pageHelper.ConstructCircuits(out maxDistance);
                btn_circuitsObstacleAssesment.Enabled = true;
                isCircuitObstaclesAssessed = false;
                pageHelper.DrawCircuits();
                //VMManager.Instance.FinalSegmentStartPointHeight = VMManager.Instance.RateOfDescent / 60 * VMManager.Instance.FinalSegmentTime + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
                VMManager.Instance.FinalSegmentStartPointAltitude = VMManager.Instance.FinalSegmentLength * VMManager.Instance.DescentGradient / 100 + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
                txtBox_FinalSegmentStartPointAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude, eRoundMode.CEIL).ToString();

                if (!isInit)
                {
                    pageHelper.ConstructCircuits(out maxDistance);
                    btn_circuitsObstacleAssesment.Enabled = true;
                    isCircuitObstaclesAssessed = false;
                    pageHelper.DrawCircuits();

                    if (VMManager.Instance.GeomOper.CurrentGeometry == null)
                    {
                        VMManager.Instance.GeomOper = new JtsGeometryOperators();
                        VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.ConvexPoly;
                    }

                    if (!VMManager.Instance.GeomOper.Contains(VMManager.Instance.LefthandCircuitPolygon) || !VMManager.Instance.GeomOper.Contains(VMManager.Instance.RighthandCircuitPolygon))
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Red;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        nmrcUpDown_visualManoeuvringIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentIAS.ForeColor = System.Drawing.Color.Black;
                        nmrcUpDown_FinalSegmentTime.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }
        

        private void btn_circuitsObstacleAssesment_Click(object sender, EventArgs e)
        {
            pageHelper.assessCircuitObstacles();
            pageHelper.DrawCircuits();
            btn_circuitsObstacleAssesment.Enabled = false;
        }

        private void nmrcUpDown_RateOfDescent_ValueChanged(object sender, EventArgs e)
        {
            //VMManager.Instance.RateOfDescent = (double) nmrcUpDown_RateOfDescent.Value;
            ////VMManager.Instance.FinalSegmentStartPointHeight = VMManager.Instance.RateOfDescent / 60 * VMManager.Instance.FinalSegmentTime + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
            //VMManager.Instance.FinalSegmentStartPointAltitude = VMManager.Instance.FinalSegmentLength * VMManager.Instance.DescentGradient / 100 + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
            //txtBox_FinalSegmentStartPointAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude, eRoundMode.CEIL).ToString();
        }

        private void nmrcUpDown_DescentGradient_ValueChanged(object sender, EventArgs e)
        {
            /*VMManager.Instance.DescentGradient = (double) nmrcUpDown_DescentGradient.Value;
            VMManager.Instance.FinalSegmentStartPointAltitude = VMManager.Instance.FinalSegmentLength * VMManager.Instance.DescentGradient / 100 + (VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z + 15);
            txtBox_FinalSegmentStartPointAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.FinalSegmentStartPointAltitude, eRoundMode.CEIL).ToString();*/
        }

        private void cmbBox_IAS2TAS_OCA_OCH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_IAS2TAS_OCA_OCH.SelectedIndex == 0)
                txtBox_IAS2TAS_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.VM_OCA, eRoundMode.CEIL).ToString();
            else
                txtBox_IAS2TAS_OCA_OCH.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.VM_OCA - GlobalVars.CurrADHP.Elev, eRoundMode.CEIL).ToString();
        }
    }
}
