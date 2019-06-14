using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class DrawnElements : Form
    {
        MainForm mainForm;
        int currentPageIndex;
        public DrawnElements(MainForm mf, int currentPageIndex)
        {
            InitializeComponent();
            this.mainForm = mf;
            this.currentPageIndex = currentPageIndex;

            if (VMManager.Instance.isRighthandCircuitPolygonVisible)
                chkBox_showRHCircuit.Checked = true;
            else
                chkBox_showRHCircuit.Checked = false;

            if (VMManager.Instance.isRighthandCircuitCentrelineVisible)
                chkBox_showRHCircuitCentreline.Checked = true;
            else
                chkBox_showRHCircuitCentreline.Checked = false;

            if (VMManager.Instance.isLefthandCircuitPolygonVisible)
                chkBox_showLHCircuit.Checked = true;
            else
                chkBox_showLHCircuit.Checked = false;

            if (VMManager.Instance.isLefthandCircuitCentrelineVisible)
                chkBox_showLHCircuitCentreline.Checked = true;
            else
                chkBox_showLHCircuitCentreline.Checked = false;

            if (VMManager.Instance.isFinalSegmentPolygonVisible)
                chkBox_showFinalSegment.Checked = true;
            else
                chkBox_showFinalSegment.Checked = false;

            if (VMManager.Instance.isFinalSegmentCentrelineVisible)
                chkBox_showFinalSegmentCentreline.Checked = true;
            else
                chkBox_showFinalSegmentCentreline.Checked = false;


            if (currentPageIndex >= 2)
            {
                grpBox_Circuits.Enabled = true;
            }
            else
            {
                grpBox_Circuits.Enabled = false;
            }

            if (VMManager.Instance.isAllVisualFeaturesVisible)
                chkBox_showVisualFeatures.Checked = true;
            else
                chkBox_showVisualFeatures.Checked = false;            

            if (VMManager.Instance.isIAPFinalSegmentLegsVisible)
                chkBox_IAPFinalSegmentLegs.Checked = true;
            else
                chkBox_IAPFinalSegmentLegs.Checked = false;

            if (VMManager.Instance.isFinalSegmentLegsPointsVisible)
                chkBox_IAPFinalSegmentLegsPoints.Checked = true;
            else
                chkBox_IAPFinalSegmentLegsPoints.Checked = false;

            if (VMManager.Instance.isTrackInitialPositionVisible)
                chkBox_TrackInitialPositionPoint.Checked = true;
            else
                chkBox_TrackInitialPositionPoint.Checked = false;

            if (VMManager.Instance.isTrackInitialDirectionVisible)
                chkBox_TrackInitialPositionDirectionArrow.Checked = true;
            else
                chkBox_TrackInitialPositionDirectionArrow.Checked = false;

            if (VMManager.Instance.isDestinationRWYVisible)
                chkBox_DestinationRWY.Checked = true;
            else
                chkBox_DestinationRWY.Checked = false;

            if (VMManager.Instance.isDestinationRWYTHRVisible)
                chkBox_DestinationRWYTHR.Checked = true;
            else
                chkBox_DestinationRWYTHR.Checked = false;

            if (VMManager.Instance.isDivergenceVFSelectionPolyVisible)
                chkBox_showDivergenceVFSelectionPoly.Checked = true;
            else
                chkBox_showDivergenceVFSelectionPoly.Checked = false;
        }

        private void chkBox_showRHCircuit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showRHCircuit.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RighthandCircuitPolygonElement, true);
                VMManager.Instance.isRighthandCircuitPolygonVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RighthandCircuitPolygonElement, false);
                VMManager.Instance.isRighthandCircuitPolygonVisible = false;
            }
        }

        private void chkBox_showRHCircuitCentreline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showRHCircuitCentreline.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RighthandCircuitCentrelineElement, true);
                VMManager.Instance.isRighthandCircuitCentrelineVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RighthandCircuitCentrelineElement, false);
                VMManager.Instance.isRighthandCircuitCentrelineVisible = false;
            }
        }

        private void chkBox_showLHCircuit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showLHCircuit.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.LefthandCircuitPolygonElement, true);
                VMManager.Instance.isLefthandCircuitPolygonVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.LefthandCircuitPolygonElement, false);
                VMManager.Instance.isLefthandCircuitPolygonVisible = false;
            }
        }

        private void chkBox_showLHCircuitCentreline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showLHCircuitCentreline.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.LefthandCircuitCentrelineElement, true);
                VMManager.Instance.isLefthandCircuitCentrelineVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.LefthandCircuitCentrelineElement, false);
                VMManager.Instance.isLefthandCircuitCentrelineVisible = false;
            }
        }

        private void chkBox_showFinalSegment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showFinalSegment.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentPolygonElement, true);
                VMManager.Instance.isFinalSegmentPolygonVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentPolygonElement, false);
                VMManager.Instance.isFinalSegmentPolygonVisible = false;
            }
        }

        private void chkBox_showFinalSegmentCetreline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showFinalSegmentCentreline.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentCentrelineElement, true);
                VMManager.Instance.isFinalSegmentCentrelineVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentCentrelineElement, false);
                VMManager.Instance.isFinalSegmentCentrelineVisible = false;
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DrawnElements_Load(object sender, EventArgs e)
        {

        }

        private void chkBox_visualFeatures_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showVisualFeatures.Checked)
            {
                VMManager.Instance.isAllVisualFeaturesVisible = true;

                for (int i = 0; i < VMManager.Instance.AllVisualFeatureElements.Count; i++)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.AllVisualFeatureElements[i]);
                    VMManager.Instance.AllVisualFeatureElements.RemoveAt(0);
                    i--;
                }

                for(int i = 0; i < VMManager.Instance.AllVisualFeatures.Count; i++)
                {
                    int elem = GlobalVars.gAranEnv.Graphics.DrawPointWithText(VMManager.Instance.AllVisualFeatures[i].pShape, ARANFunctions.RGB(0, 172, 237), VMManager.Instance.AllVisualFeatures[i].Name);
                    VMManager.Instance.AllVisualFeatureElements.Add(elem);
                }
            }
            else
            {
                VMManager.Instance.isAllVisualFeaturesVisible = false;
                for (int i = 0; i < VMManager.Instance.AllVisualFeatureElements.Count; i++)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.AllVisualFeatureElements[i]);
                    VMManager.Instance.AllVisualFeatureElements.RemoveAt(0);
                    i--;
                }
            }
        }

        private void chkBox_IAPFinalSegmentLegs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_IAPFinalSegmentLegs.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement, true);
                VMManager.Instance.isIAPFinalSegmentLegsVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement, false);
                VMManager.Instance.isIAPFinalSegmentLegsVisible = false;
            }
        }

        private void chkBox_IAPFinalSegmentLegsPoints_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_IAPFinalSegmentLegsPoints.Checked)
            {
                for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i], true);
                VMManager.Instance.isFinalSegmentLegsPointsVisible = true;
            }
            else
            {
                for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i], false);
                VMManager.Instance.isFinalSegmentLegsPointsVisible = false;
            }
        }

        private void chkBox_TrackInitialPositionPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_TrackInitialPositionPoint.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialPositionElement, true);
                VMManager.Instance.isTrackInitialPositionVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialPositionElement, false);
                VMManager.Instance.isTrackInitialPositionVisible = false;
            }
        }

        private void chkBox_TrackInitialPositionDirectionArrow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_TrackInitialPositionDirectionArrow.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialDirectionElement, true);
                VMManager.Instance.isTrackInitialDirectionVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialDirectionElement, false);
                VMManager.Instance.isTrackInitialDirectionVisible = false;
            }
        }

        private void chkBox_DestinationRWY_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_DestinationRWY.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RWYElement, true);
                VMManager.Instance.isDestinationRWYVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RWYElement, false);
                VMManager.Instance.isDestinationRWYVisible = false;
            }
        }

        private void chkBox_DestinationRWYTHR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_DestinationRWYTHR.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RWYTHRSelectElement, true);
                VMManager.Instance.isDestinationRWYTHRVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.RWYTHRSelectElement, false);
                VMManager.Instance.isDestinationRWYTHRVisible = false;
            }
        }

        private void chkBox_showDivergenceVFSelectionPoly_CheckedChanged(object sender, EventArgs e)
        {
            if(chkBox_showDivergenceVFSelectionPoly.Checked)
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement, true);
                VMManager.Instance.isDivergenceVFSelectionPolyVisible = true;
            }
            else
            {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement, false);
                VMManager.Instance.isDivergenceVFSelectionPolyVisible = false;
            }
        }

        private void chkBox_visibilityBuffer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_visibilityBuffer.Checked)
            {
                //foreach (int element in VMManager.Instance.StepVisibilityPolyElements)
                // {
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.StepVisibilityPolyElement, true);
                    VMManager.Instance.isTrackVisibilityBufferVisible = true;
                //}
            }
            else
            {
                //foreach (int element in VMManager.Instance.StepVisibilityPolyElements)
                //{
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.StepVisibilityPolyElement, false);
                    VMManager.Instance.isTrackVisibilityBufferVisible = false;
                //}
            }
        }

        private void DrawnElements_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.isDrawnElementsFormOpen = false;
        }
    }
}
