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
    public partial class NS_Page3 : UserControl
    {
        bool isInit = false;
        Point convergenceFlyByPoint;
        

        decimal prevMaxValue;
        private FormHelpers.NS_Page3_Helper pageHelper;
        int prevSelectedIdx = 0;
        public int prevPageNumber { get; set; }
        public int nextPageNumber { get; set; }
        double polySize;

        public NS_Page3()
        {
            InitializeComponent();
            pageHelper = new FormHelpers.NS_Page3_Helper();
            VMManager.Instance.ReachableVFs = new List<VM_VisualFeature>();
            VMManager.Instance.ReachableVFsElements = new List<int>();
            VMManager.Instance.DivergenceLineElements.Add(-1);
            prevPageNumber = 2;
            nextPageNumber = 4;
            isInit = true;
            nmrcUpDown_finalSegmentTime.Minimum = (decimal)VMManager.Instance.FinalSegmentTime;
            nmrcUpDown_finalSegmentTime.Maximum = 60;
            nmrcUpDown_finalSegmentTime.Value = (decimal)VMManager.Instance.FinalSegmentTime;
            isInit = false;
        }

        private void NS_Page3_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (prevPageNumber == 2)
                {
                    polySize = 10000;
                    txtBox_polySize.Enabled = false;
                    VMManager.Instance.ReachableVFs.Clear();
                    VMManager.Instance.ReachableVFsElements.Clear();
                    cmbBox_reachablePoints.DataSource = pageHelper.getReachableVisualFeatures();
                    txtBox_initialDirection.Text = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection)).ToString();
                    if (cmbBox_reachablePoints.Items.Count == 0)
                    {
                        nmrcUpDown_distanceRange.Enabled = false;
                        lbl_rangeDist.Text = "-";
                        lbl_rangeAngle.Text = "-";
                    }
                }

                for (int i = 0; i < VMManager.Instance.ReachableVFs.Count; i++)
                {
                    LineString ls = new LineString();
                    ls.AddMultiPoint(ARANFunctions.CreateCircle(VMManager.Instance.ReachableVFs[i].pShape, 500));
                    if(i == cmbBox_reachablePoints.SelectedIndex)
                        VMManager.Instance.ReachableVFsElements.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2));
                    else
                        VMManager.Instance.ReachableVFsElements.Add(GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 255, 0), 1));
                }
            }
            else
            {
                if (nextPageNumber == 2)
                {
                    pageHelper.onClose();
                }

                for (int i = 0; i < VMManager.Instance.ReachableVFsElements.Count; i++)
                {
                    if (VMManager.Instance.ReachableVFsElements[i] > -1)
                    {
                        GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ReachableVFsElements[i]);
                    }
                }
                VMManager.Instance.ReachableVFsElements.Clear();                
            }
        }

        private void cmbBox_reachablePoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VMManager.Instance.isFinalStep && VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].Name.Equals("THR" + VMManager.Instance.SelectedRWY.Name))
            {
                if (VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing != null)
                {
                    nmrcUpDown_finalSegmentTime.Visible = true;
                    if (nmrcUpDown_finalSegmentTime.Value == (decimal) VMManager.Instance.FinalSegmentTime)
                        nmrcUpDown_finalSegmentTime_ValueChanged(nmrcUpDown_finalSegmentTime, null);
                    else
                        nmrcUpDown_finalSegmentTime.Value = (decimal) VMManager.Instance.FinalSegmentTime;
                }
                else
                    MessageBox.Show("The value of the True Bearing of the selected Runway Direction is not set.");                    
            }
            else
            {
                nmrcUpDown_finalSegmentTime.Visible = false;
                convergenceFlyByPoint = VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].pShape;
                lbl_rangeAngle.Text = Math.Round(ARANMath.RadToDeg(pageHelper.Ranges[cmbBox_reachablePoints.SelectedIndex].MIN)) + " - " + Math.Round(ARANMath.RadToDeg(pageHelper.Ranges[cmbBox_reachablePoints.SelectedIndex].MAX));
                pageHelper.CalculateMaxMinDistance(cmbBox_reachablePoints.SelectedIndex, null, 0, 0);
                lbl_rangeDist.Text = Math.Round(pageHelper.MinDist) + " - " + Math.Round(pageHelper.MaxDist);
                pageHelper.DrawSelectedVF(cmbBox_reachablePoints.SelectedIndex);
                pageHelper.Side = ARANMath.SideDef(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].pShape);
                nmrcUpDown_distanceRange.Minimum = (decimal)Math.Round(pageHelper.MinDist);
                prevMaxValue = nmrcUpDown_distanceRange.Maximum;
                nmrcUpDown_distanceRange.Maximum = (decimal)Math.Floor(pageHelper.MaxDist);
                prevMaxValue = nmrcUpDown_distanceRange.Maximum;
                if (nmrcUpDown_distanceRange.Value != nmrcUpDown_distanceRange.Minimum)
                    nmrcUpDown_distanceRange.Value = nmrcUpDown_distanceRange.Minimum;
                else
                    nmrcUpDown_distanceRange_ValueChanged(nmrcUpDown_distanceRange, null);               

            }

            if (VMManager.Instance.ReachableVFsElements.Count > 0)
            {
                LineString ls = new LineString();

                if (VMManager.Instance.ReachableVFsElements[prevSelectedIdx] > -1)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ReachableVFsElements[prevSelectedIdx]);
                    ls.AddMultiPoint(ARANFunctions.CreateCircle(VMManager.Instance.ReachableVFs[prevSelectedIdx].pShape, 500));
                    VMManager.Instance.ReachableVFsElements[prevSelectedIdx] = GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 255, 0), 1);
                }

                ls = new LineString();
                if (VMManager.Instance.ReachableVFsElements[cmbBox_reachablePoints.SelectedIndex] > -1)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ReachableVFsElements[cmbBox_reachablePoints.SelectedIndex]);
                    ls.AddMultiPoint(ARANFunctions.CreateCircle(VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].pShape, 500));
                    VMManager.Instance.ReachableVFsElements[cmbBox_reachablePoints.SelectedIndex] = GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 0, 255), 2);
                }
            }

            prevSelectedIdx = cmbBox_reachablePoints.SelectedIndex;
        }

        private void nmrcUpDown_distanceRange_ValueChanged(object sender, EventArgs e)
        {
            if (prevMaxValue > nmrcUpDown_distanceRange.Maximum && nmrcUpDown_distanceRange.Value == nmrcUpDown_distanceRange.Maximum)
                return;
            double intermediateDirection = pageHelper.ConstructDivergenceLine((double)nmrcUpDown_distanceRange.Value, cmbBox_reachablePoints.SelectedIndex, convergenceFlyByPoint);
            txtBox_intermediateDirection.Text = Math.Round(intermediateDirection).ToString();
            if (txtBox_polySize.Text.Equals(polySize.ToString()))
                txtBox_polySize_TextChanged(txtBox_polySize, null);
            else
                txtBox_polySize.Text = polySize.ToString();
            txtBox_polySize.Enabled = true;
        }

        private void txtBox_polySize_TextChanged(object sender, EventArgs e)
        {
            polySize = double.Parse(txtBox_polySize.Text);
            pageHelper.ConstructConvergencePoly(polySize, /*cmbBox_reachablePoints.SelectedIndex*/ convergenceFlyByPoint);
        }

        private void txtBox_polySize_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }

        private void nmrcUpDown_finalSegmentTime_ValueChanged(object sender, EventArgs e)
        {
            if (isInit)
                return;
            double finalSegmentTime = (double)nmrcUpDown_finalSegmentTime.Value;
            double direction = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].pShape, VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing.Value) - ARANMath.C_PI, ARANMath.C_2xPI);
            double dist = VMManager.Instance.VM_TASWind * finalSegmentTime;            
            VMManager.Instance.FinalSegmentStartPoint = ARANFunctions.PointAlongPlane(VMManager.Instance.ReachableVFs[cmbBox_reachablePoints.SelectedIndex].pShape, direction, dist);            
            convergenceFlyByPoint = pageHelper.getConvergenceFlyByPoint();
            Point tempPnt = ARANFunctions.PrjToLocal(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, convergenceFlyByPoint);
            
            double minAngle, maxAngle;
            if (pageHelper.isReachable(tempPnt, out minAngle, out maxAngle))
            {
                lbl_rangeAngle.Text = Math.Round(ARANMath.RadToDeg(minAngle)) + " - " + Math.Round(ARANMath.RadToDeg(maxAngle));
                pageHelper.CalculateMaxMinDistance(-1, tempPnt, minAngle, maxAngle);
                lbl_rangeDist.Text = Math.Round(pageHelper.MinDist) + " - " + Math.Round(pageHelper.MaxDist);
                pageHelper.Side = ARANMath.SideDef(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, convergenceFlyByPoint);
                nmrcUpDown_distanceRange.Minimum = (decimal)Math.Round(pageHelper.MinDist);
                prevMaxValue = nmrcUpDown_distanceRange.Maximum;
                nmrcUpDown_distanceRange.Maximum = (decimal)Math.Floor(pageHelper.MaxDist);
                prevMaxValue = nmrcUpDown_distanceRange.Maximum;
                if (nmrcUpDown_distanceRange.Value != nmrcUpDown_distanceRange.Minimum)
                    nmrcUpDown_distanceRange.Value = nmrcUpDown_distanceRange.Minimum;
                else
                    nmrcUpDown_distanceRange_ValueChanged(nmrcUpDown_distanceRange, null);
            }
            else
            {
                MessageBox.Show("Is not reachable!");
                nmrcUpDown_finalSegmentTime.Value--;
            }

            if (VMManager.Instance.FinalSegmentStartPntElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentStartPntElement);
            VMManager.Instance.FinalSegmentStartPntElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.FinalSegmentStartPoint, 255);
        }

        
    }
}
