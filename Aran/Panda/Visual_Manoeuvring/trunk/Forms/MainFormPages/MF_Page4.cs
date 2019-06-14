using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Aran.Geometries.Operators;
using Aran.Panda.Common;
namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class MF_Page4 : UserControl
    {
        public bool makeInvisible = true;
        private int segmentCount;
        public int SegmentCount
        {
            get { return segmentCount; }
            set
            {
                segmentCount = value;
                var newItem = new SegmentDetailsControl();
                newItem.Name = "Segment " + segmentCount;
                newItem.Length = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].Length_GUI;
                newItem.InitialDirection = (VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].InitialDirectionAzt - GlobalVars.CurrADHP.MagVar);
                newItem.IntermediateDirection = (VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].IntermediateDirectionAzt - GlobalVars.CurrADHP.MagVar);
                newItem.FinalDirection = (VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].FinalDirectionAzt - GlobalVars.CurrADHP.MagVar); ;
                newItem.FlightAltitude = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].FlightAltitude_GUI;
                newItem.Description = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].Description;
                flwLytPnl_TrackSegments.Controls.Add(newItem);
            }
        }

        MainForm mainForm;
        public int nextPageIndex;
        public MF_Page4(MainForm mf)
        {
            InitializeComponent();
            mainForm = mf;
            trackStepBindingSource.DataSource = VMManager.Instance.TrackSegmentsList;

            label6.Text = "Length" + System.Environment.NewLine + "(" + GlobalVars.unitConverter.DistanceUnit + ")";
            label8.Text = "Min. Flight Altitude" + System.Environment.NewLine + "(" + GlobalVars.unitConverter.HeightUnit + ")";
        }

        private void btn_addNewStep_Click(object sender, EventArgs e)
        {
            NewSegmentForm mnsf = new NewSegmentForm(this, mainForm, chkBox_isFinalSegmentStep.Checked);
            mnsf.Show(this);
        }

        private void btn_removeLastStep_Click(object sender, EventArgs e)
        {
            
            String text = "Are you sure you want to permanently remove the last segment?";
            String caption = "Work loss danger";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            if (MessageBox.Show(text, caption, buttons, icon) == DialogResult.No)
                return;

            if (VMManager.Instance.TrackSegmentsList.Count > 0)
            {
                VMManager.Instance.TrackSegmentsList.RemoveAt(VMManager.Instance.TrackSegmentsList.Count - 1);
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepCentrelinElements[VMManager.Instance.StepCentrelinElements.Count - 1]);
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepBufferPolyElements[VMManager.Instance.StepBufferPolyElements.Count - 1]);
                //GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElements[VMManager.Instance.StepVisibilityPolyElements.Count - 1]);
                foreach (int elem in VMManager.Instance.StepMainPointsElements[VMManager.Instance.StepMainPointsElements.Count - 1])
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(elem);
                VMManager.Instance.StepMainPointsElements.RemoveAt(VMManager.Instance.StepMainPointsElements.Count - 1);                
                VMManager.Instance.StepCentrelinElements.RemoveAt(VMManager.Instance.StepCentrelinElements.Count - 1);
                VMManager.Instance.StepBufferPolyElements.RemoveAt(VMManager.Instance.StepBufferPolyElements.Count - 1);
                //VMManager.Instance.StepVisibilityPolyElements.RemoveAt(VMManager.Instance.StepVisibilityPolyElements.Count - 1);
                VMManager.Instance.TrackSegmentObstacles.RemoveAt(VMManager.Instance.TrackSegmentObstacles.Count - 1);
                flwLytPnl_TrackSegments.Controls.RemoveAt(flwLytPnl_TrackSegments.Controls.Count - 1);
            }

            if (VMManager.Instance.TrackSegmentsList.Count == 0)
            {
                btn_removeLastStep.Enabled = false;
                btn_addNewStep.Enabled = true;
                mainForm.btn_Ok.Enabled = false;
            }
            else
            {
                if (!VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].IsFinalSegment)
                    btn_addNewStep.Enabled = true;
            }
        }

        private void btn_LoadFromFile_Click(object sender, EventArgs e)
        {
            if (VMManager.Instance.TrackSegmentsList.Count > 0)
            {
                MessageBox.Show("Please, remove existing segments and try again.");
                return;
            }
            OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.FileName = null;

            // Display the openFile dialog.
            DialogResult result = openFileDialog.ShowDialog();

            // OK button was pressed. 
            if (result == DialogResult.OK)
            {
                try
                {
                    using (var sr = File.OpenText(openFileDialog.FileName))
                    {
                        var str = sr.ReadToEnd();
                        JObject obj = JsonConvert.DeserializeObject<JObject>(str);
                        var effectiveDate = obj["effectiveDate"].Value<DateTime>();
                        var airportHeliportDesignator = obj["airportHeliportDesignator"].Value<String>();
                        //var finalNavaidName = obj["finalNavaidName"].Value<String>();
                        var selectedRWYName = obj["selectedRWYName"].Value<String>();

                        bool error = false;
                        string errorMsg = "";
                        if(DateTime.Compare(effectiveDate, DBModule.pObjectDir.TimeSlice.EffectiveDate) > 0)
                        {
                            error = true;
                            errorMsg = "The data in the file is not valid for this effective date...";
                        }
                        else if (!airportHeliportDesignator.Equals(GlobalVars.CurrADHP.pAirportHeliport.Designator))
                        {
                            error = true;
                            errorMsg = "The data in the file is not valid for the current airport/heliport...";
                        }
                        //else if (!finalNavaidName.Equals(VMManager.Instance.FinalNavaid.Name))
                        //{
                        //    error = true;
                        //    errorMsg = "The data in the file is not valid for the selected final navaid...";
                        //}
                        else if (!selectedRWYName.Equals(VMManager.Instance.SelectedRWY.Name))
                        {
                            error = true;
                            errorMsg = "The data in the file is not valid for the selected runway...";
                        }

                        if (error)
                        {
                            MessageBox.Show(errorMsg);
                            return;
                        }

                        var jArr = obj["segments"].Value<JArray>();
                        foreach (JObject jObj in jArr)
                        {
                            var newItem = new VM_TrackSegment();
                            newItem.FromJson(jObj);
                            VMManager.Instance.TrackSegmentsList.Add(newItem);
                            
                            VMManager.Instance.InitialPosition = newItem.StartPointPrj;
                            VMManager.Instance.InitialDirection = newItem.InitialDirectionDir;
                            FormHelpers.NS_Page_Helper helper = new FormHelpers.NS_Page_Helper();
                            VMManager.Instance.DivergenceSide = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].DivergenceSide;
                            VMManager.Instance.ConvergenceSide = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].ConvergenceSide;
                            helper.ContructStep(newItem.DistanceTillDivergence, newItem.IntermediateDirectionDir, newItem.DistanceTillConvergence,
                                newItem.FinalDirectionDir, newItem.DistanceTillEndPoint, newItem.IsFinalSegment, true);

                            VMManager.Instance.StepMainPointsElements.Add(VMManager.Instance.TempMainPointElements);
                            VMManager.Instance.RemoveTempMainPointElements = false;
                            VMManager.Instance.TrackSegmentObstacles.Add(new List<VM_VerticalStructure>());                            
                            VMManager.Instance.GeomOper.CurrentGeometry = VMManager.Instance.StepBufferPoly;
                            for (int i = 0; i < VMManager.Instance.AllObstacles.Count; i++)
                            {
                                var vs = new VM_VerticalStructure(VMManager.Instance.AllObstacles[i].VerticalStructure, VMManager.Instance.AllObstacles[i].PartGeoPrjList, VMManager.Instance.TrackSegmentsList.Count);
                                if (vs.PartGeometries.Count == 0)
                                    continue;

                                VMManager.Instance.TrackSegmentObstacles[VMManager.Instance.TrackSegmentObstacles.Count - 1].Add(vs);
                                if (vs.Elevation + VMManager.Instance.MOC > newItem.FlightAltitude)
                                    newItem.FlightAltitude = vs.Elevation + VMManager.Instance.MOC;
                            }
                            newItem.FlightHeight = newItem.FlightAltitude - GlobalVars.CurrADHP.Elev;
                            if (newItem.FlightHeight < VMManager.Instance.MinOCH)
                            {
                                newItem.FlightAltitude = VMManager.Instance.MinOCH + GlobalVars.CurrADHP.Elev;
                                newItem.FlightHeight = VMManager.Instance.MinOCH;
                            }
                            VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].FlightAltitude = newItem.FlightAltitude;
                            VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].FlightAltitude_GUI = GlobalVars.unitConverter.HeightToDisplayUnits(newItem.FlightAltitude, eRoundMode.CEIL);
                            SegmentCount = VMManager.Instance.TrackSegmentsList.Count;
                            VMManager.Instance.GeomOper = new JtsGeometryOperators();
                            VMManager.Instance.StepCentrelinElements.Add(VMManager.Instance.StepCentrelineElement);
                            VMManager.Instance.StepBufferPolyElements.Add(VMManager.Instance.StepBufferPolyElement);
                            VMManager.Instance.StepVisibilityPolyElements.Add(VMManager.Instance.StepVisibilityPolyElement);
                            VMManager.Instance.StepCentrelineElement = -1;
                            VMManager.Instance.StepBufferPolyElement = -1;
                            VMManager.Instance.StepVisibilityPolyElement = -1;
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                }
                Invalidate();
            }

            // Cancel button was pressed. 
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            if (VMManager.Instance.TrackSegmentsList.Count > 0)
                btn_removeLastStep.Enabled = true;
        }

        private void MF_Page4_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                makeInvisible = true;
            }
            else
            {
                if(nextPageIndex == 1)
                {
                    for (int i = 0; i < VMManager.Instance.TrackSegmentsList.Count; i++)
                    {
                        VMManager.Instance.TrackSegmentsList.RemoveAt(VMManager.Instance.TrackSegmentsList.Count - 1);
                        GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepCentrelinElements[VMManager.Instance.StepCentrelinElements.Count - 1]);
                        GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepBufferPolyElements[VMManager.Instance.StepBufferPolyElements.Count - 1]);
                        //GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.StepVisibilityPolyElements[VMManager.Instance.StepVisibilityPolyElements.Count - 1]);
                        foreach (int elem in VMManager.Instance.StepMainPointsElements[VMManager.Instance.StepMainPointsElements.Count - 1])
                            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(elem);
                        VMManager.Instance.StepMainPointsElements.RemoveAt(VMManager.Instance.StepMainPointsElements.Count - 1);
                        VMManager.Instance.StepCentrelinElements.RemoveAt(VMManager.Instance.StepCentrelinElements.Count - 1);
                        VMManager.Instance.StepBufferPolyElements.RemoveAt(VMManager.Instance.StepBufferPolyElements.Count - 1);
                        //VMManager.Instance.StepVisibilityPolyElements.RemoveAt(VMManager.Instance.StepVisibilityPolyElements.Count - 1);
                        VMManager.Instance.TrackSegmentObstacles.RemoveAt(VMManager.Instance.TrackSegmentObstacles.Count - 1);
                        flwLytPnl_TrackSegments.Controls.RemoveAt(flwLytPnl_TrackSegments.Controls.Count - 1);
                        i--;
                    }
                    mainForm.btn_Ok.Enabled = false;           
                }
            }
        }        
    }
}
