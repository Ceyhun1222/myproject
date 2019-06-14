using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Geometries;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class NewSegmentForm : Form
    {
        MainForm mainForm;
        MF_Page4 page;
        private NS_Page_new _page1;
        private bool isFinalSegmentStep;
        public NewSegmentForm(MF_Page4 page, MainForm mf, bool isFinalSegmentStep)
        {
            InitializeComponent();
            this.page = page;
            mainForm = mf;
            this.isFinalSegmentStep = isFinalSegmentStep;
        }

        private void MyNewStep_Load(object sender, EventArgs e)
        {
            _page1 = new NS_Page_new(this, isFinalSegmentStep);
            pnl_MainPanel.Controls.Add(_page1);
            pnl_MainPanel.Controls[0].Visible = true;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (VMManager.Instance.StepCentreline.Count <= 1)
            {
                MessageBox.Show("Please, set meaningful values for the step parameters.");
                return;
            }

            if (VMManager.Instance.StepDescription.Equals(""))
            {
                MessageBox.Show("Please, textually describe the step.");
                return;
            }            

            VM_TrackSegment newStep = new VM_TrackSegment();
            newStep.Name = "Segment " + (VMManager.Instance.TrackSegmentsList.Count + 1);
            newStep.Length = VMManager.Instance.StepLength;
            newStep.StartPointPrj = VMManager.Instance.InitialPosition;
            newStep.InitialDirectionDir = VMManager.Instance.InitialDirection;
            newStep.EndPointPrj = VMManager.Instance.FinalPosition;
            newStep.FinalDirectionDir = VMManager.Instance.FinalDirection;
            newStep.DistanceTillDivergence = VMManager.Instance.DistanceTillDivergence;
            newStep.DivergenceSide = VMManager.Instance.DivergenceSide;
            //newStep.DivergenceAngleRad = VMManager.Instance.DivergenceAngle;
            newStep.IntermediateDirectionDir = VMManager.Instance.IntermediateDirection;
            newStep.DistanceTillConvergence = VMManager.Instance.DistanceTillConvergence;
            newStep.ConvergenceSide = VMManager.Instance.ConvergenceSide;
            //newStep.ConvergenceAngleRad = VMManager.Instance.ConvergenceAngle;
            newStep.DistanceTillEndPoint = VMManager.Instance.DistanceTillEndPoint;
            newStep.Centreline = VMManager.Instance.StepCentreline;
            newStep.BufferPoly = VMManager.Instance.StepBufferPoly;
            newStep.Description = VMManager.Instance.StepDescription;
            newStep.IsFinalSegment = isFinalSegmentStep;

            //For GUI
            newStep.Length_GUI = Math.Round(GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.StepLength, eRoundMode.NERAEST),3);
            newStep.InitialDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection));
            //newStep.DivergenceAngleDeg = Math.Round(ARANMath.RadToDeg(VMManager.Instance.DivergenceAngle));
            //newStep.ConvergenceAngleDeg = Math.Round(ARANMath.RadToDeg(VMManager.Instance.ConvergenceAngle));
            newStep.IntermediateDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.InitialPosition, VMManager.Instance.IntermediateDirection));
            newStep.FinalDirectionAzt = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalPosition, VMManager.Instance.FinalDirection));
            


            //VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.StepBufferPoly;

            //VMManager.Instance.TrackSegmentObstacles.Add(new List<VM_VerticalStructure>());
            //for (int i = 0; i < VMManager.Instance.AllObstacles.Count; i++)
            //{
            //    var vs = new VM_VerticalStructure(VMManager.Instance.AllObstacles[i].VerticalStructure, VMManager.Instance.AllObstacles[i].PartGeoPrjList, VMManager.Instance.TrackSegmentsList.Count + 1);
            //    if (vs.PartGeometries.Count == 0)
            //        continue;                

            //    VMManager.Instance.TrackSegmentObstacles[VMManager.Instance.TrackSegmentObstacles.Count - 1].Add(vs);
            //    if (vs.Elevation + VMManager.Instance.MOC > newStep.FlightAltitude)
            //        newStep.FlightAltitude = vs.Elevation + VMManager.Instance.MOC;
            //}
            //VMManager.Instance.GeomOper = new JtsGeometryOperators();
            int idx;
            List<VM_VerticalStructure> segmentObstaclesList;
            newStep.FlightAltitude = Functions.MaxObstacleElevationInPoly(VMManager.Instance.AllObstacles, out segmentObstaclesList, VMManager.Instance.StepBufferPoly, out idx, VMManager.Instance.TrackSegmentsList.Count + 1) + 
                VMManager.Instance.MOC;
            if (newStep.FlightAltitude < VMManager.Instance.MinOCA)
                newStep.FlightAltitude = VMManager.Instance.MinOCA;
            VMManager.Instance.TrackSegmentObstacles.Add(segmentObstaclesList);

            VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.StepVisibilityPoly;
            VMManager.Instance.TrackSegmentVisualFeatures.Add(new List<VM_VisualFeature>());
            for (int i = 0; i < VMManager.Instance.AllVisualFeatures.Count; i++)
            {
                if (!VMManager.Instance.GeomOper.Disjoint(VMManager.Instance.AllVisualFeatures[i].pShape))
                    VMManager.Instance.TrackSegmentVisualFeatures[VMManager.Instance.TrackSegmentVisualFeatures.Count - 1].Add(VMManager.Instance.AllVisualFeatures[i]);
            }


            newStep.FlightHeight = newStep.FlightAltitude - GlobalVars.CurrADHP.Elev;
            if (newStep.FlightHeight < VMManager.Instance.MinOCH)
            {
                newStep.FlightAltitude = VMManager.Instance.MinOCH + GlobalVars.CurrADHP.Elev;
                newStep.FlightHeight = VMManager.Instance.MinOCH;
            }
            newStep.FlightAltitude_GUI = GlobalVars.unitConverter.HeightToDisplayUnits(newStep.FlightAltitude, eRoundMode.CEIL);
            newStep.FlightHeight_GUI = GlobalVars.unitConverter.HeightToDisplayUnits(newStep.FlightHeight, eRoundMode.CEIL);
            VMManager.Instance.TrackSegmentsList.Add(newStep);
            VMManager.Instance.StepMainPointsElements.Add(VMManager.Instance.TempMainPointElements);
            VMManager.Instance.StepCentrelinElements.Add(VMManager.Instance.StepCentrelineElement);
            VMManager.Instance.StepBufferPolyElements.Add(VMManager.Instance.StepBufferPolyElement);
            //VMManager.Instance.StepVisibilityPolyElements.Add(VMManager.Instance.StepVisibilityPolyElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElement);
            VMManager.Instance.StepCentrelineElement = -1;
            VMManager.Instance.StepBufferPolyElement = -1;
            VMManager.Instance.StepVisibilityPolyElement = -1;
            VMManager.Instance.RemoveTempMainPointElements = false;
            page.btn_removeLastStep.Enabled = true;
            if (VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].IsFinalSegment)
                page.btn_addNewStep.Enabled = false;
            page.SegmentCount = VMManager.Instance.TrackSegmentsList.Count;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {           
            this.Close();
        }

        private void MyNewStep_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepCentrelineElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepBufferPolyElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElement);
            //GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialPositionElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ConvergenceVFSelectionPolyElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedConvergenceVFElement);

            for (int i = 0; i < VMManager.Instance.TempMainPointElements.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TempMainPointElements[i]);
                VMManager.Instance.TempMainPointElements.RemoveAt(i);
                i--;
            }

            for (int i = 0; i < VMManager.Instance.StepTurnStartEndCrossLines.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepTurnStartEndCrossLines[i]);
            }

            if (VMManager.Instance.TrackSegmentsList.Count > 0)
                mainForm.btn_Ok.Enabled = true;

            VMManager.Instance.ConvergenceVFList.Clear();
        }
    }
}
