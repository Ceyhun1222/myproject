using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Newtonsoft.Json.Linq;
using Aran.Geometries;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class MainForm : Form
    {
        Report reportForm;
        Parameters parametersForm;
        DrawnElements drawnElementsForm;
        VisualFeatureCreator vfCreatorForm;

        public bool isParameterFormOpen = false;
        public bool isDrawnElementsFormOpen = false;
        public bool isVisualFeatureCreaterOpen = false;

        MF_Page1 _page1;
        //MF_Page2 _page2;
        MF_Page3 _page3;
        MF_Page4 _page4;

        int _currentPageIndex;

        public MainForm()
        {
            InitializeComponent();            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _page1 = new MF_Page1(this);
            //_page2 = new MF_Page2();
            _page3 = new MF_Page3();
            _page4 = new MF_Page4(this);

            pnl_MainPanel.Controls.Add(_page1);
            //pnl_MainPanel.Controls.Add(_page2);
            pnl_MainPanel.Controls.Add(_page3);
            pnl_MainPanel.Controls.Add(_page4);

            for (int i = 0; i < pnl_MainPanel.Controls.Count; i++)
            {
                pnl_MainPanel.Controls[i].Dock = DockStyle.Fill;
                if (i == 0)
                {
                    pnl_MainPanel.Controls[i].Visible = true;
                }
                else
                {
                    pnl_MainPanel.Controls[i].Visible = false;
                }
            }

            btn_Prev.Enabled = false;
            _currentPageIndex = 0;
        }

        private void btn_Prev_Click(object sender, EventArgs e)
        {
            bool toSwitch = true;
            switch (_currentPageIndex)
            {
                case 3:
                    
                case 2:
                    btn_Next.Enabled = true;
                    _page4.nextPageIndex = 1;
                    _page3.prevPageIndex = 2;
                    
                    String text = "Are you sure you want to navigate to the previous page? This will result in loss of the constructed track.";
                    String caption = "Work loss danger";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    MessageBoxIcon icon = MessageBoxIcon.Warning;
                    if (MessageBox.Show(text, caption, buttons, icon) == DialogResult.No)
                    {
                        toSwitch = false;
                    }

                    break;
                case 1:
                    _page3.nextPageIndex = 0;
                    btn_Prev.Enabled = false;
                    break;
            }

            if (toSwitch)
            {
                _currentPageIndex--;
                pnl_MainPanel.Controls[_currentPageIndex].Visible = true;
                pnl_MainPanel.Controls[_currentPageIndex + 1].Visible = false;
            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            switch (_currentPageIndex)
            {
                case 0:
                    btn_Prev.Enabled = true;
                    _page3.prevPageIndex = 0;
                    break;
                case 1:
                    btn_Next.Enabled = false;
                    _page3.nextPageIndex = 2;
                    break;              
            }
            _currentPageIndex++;            

            pnl_MainPanel.Controls[_currentPageIndex].Visible = true;
            pnl_MainPanel.Controls[_currentPageIndex - 1].Visible = false;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.CirclingAreaPolyElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement);

            for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i]); 

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.ArrivalAreaPolyElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RWYTHRSelectElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RWYElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialPositionElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialDirectionElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialPositionElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialDirectionElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DesignatedPointElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DesignatedPointToNavaidSegmentElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FASegmElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RighthandCircuitPolygonElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RighthandCircuitCentrelineElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LefthandCircuitPolygonElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.LefthandCircuitCentrelineElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentPolygonElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentCentrelineElement);

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentStartPntElement);

            for (int i = 0; i < VMManager.Instance.AllVisualFeatureElements.Count; i++)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.AllVisualFeatureElements[i]);

            if (VMManager.Instance.StepCentrelinElements.Count > 0)
            {
                foreach (int element in VMManager.Instance.StepCentrelinElements)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(element);
                }
            }

            if (VMManager.Instance.StepBufferPolyElements.Count > 0)
            {
                foreach (int element in VMManager.Instance.StepBufferPolyElements)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(element);
                }
            }

            if (VMManager.Instance.StepVisibilityPolyElements.Count > 0)
            {
                foreach (int element in VMManager.Instance.StepVisibilityPolyElements)
                {
                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(element);
                }
            }

            if (VMManager.Instance.StepMainPointsElements.Count > 0)
            {
                for (int i = VMManager.Instance.StepMainPointsElements.Count - 1; i >= 0; i--)
                {
                    foreach (int elem in VMManager.Instance.StepMainPointsElements[VMManager.Instance.StepMainPointsElements.Count - 1])
                    {
                        GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(elem);
                    }
                    VMManager.Instance.StepMainPointsElements.RemoveAt(i);
                }                
            }

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement);           

            VMManager.Instance.TrackSegmentsList = new BindingList<VM_TrackSegment>();

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedDivergenceVFElement);
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement);
            VMManager.Instance.AllVisualFeatures.Clear();
        }

        private void btn_Report_CheckedChanged(object sender, EventArgs e)
        {
            if (btn_Report.Checked)
            {
                reportForm = new Report(this);
                reportForm.FillPageVisualFeatures(VMManager.Instance.AllVisualFeatures);
                reportForm.FillPageCirclingAreaObstacles(VMManager.Instance.MSAObstacles);
                if (_currentPageIndex >= 1)
                {
                    reportForm.FillPageLeftCircuitObstacles(VMManager.Instance.LefthandCircuitObstaclesList);
                    reportForm.FillPageRightCircuitObstacles(VMManager.Instance.RighthandCircuitObstaclesList);
                }

                if (_currentPageIndex >= 2)
                {
                    if (VMManager.Instance.TrackSegmentsList.Count > 0)
                    {
                        int n = 0;
                        for (int i = 0; i < VMManager.Instance.TrackSegmentObstacles.Count; i++)
                        {
                            n += VMManager.Instance.TrackSegmentObstacles[i].Count;
                        }
                        List<VM_VerticalStructure> TrackObstacles = new List<VM_VerticalStructure>();
                        List<ParameterObject> parameterObjects = new List<ParameterObject>();
                        double trackLength = 0;
                        double trackFlightAltitude = 0;
                        double trackFlightHeight = 0;
                        VM_TrackSegment trackLastStep = VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1];
                        GeometryOperators geomOpers = new GeometryOperators();
                        int k;
                        if (trackLastStep.IsFinalSegment)
                            k = VMManager.Instance.TrackSegmentsList.Count - 1;
                        else
                            k = VMManager.Instance.TrackSegmentsList.Count;

                        for (int i = 0; i < k; i++)
                        {
                            foreach (var obstacle in VMManager.Instance.TrackSegmentObstacles[i])
                                TrackObstacles.Add(obstacle);

                            if (trackFlightAltitude < VMManager.Instance.TrackSegmentsList[i].FlightAltitude)
                                trackFlightAltitude = VMManager.Instance.TrackSegmentsList[i].FlightAltitude;
                            trackLength += VMManager.Instance.TrackSegmentsList[i].Length;
                        }                        

                        trackFlightHeight = trackFlightAltitude - GlobalVars.CurrADHP.Elev;
                        if (trackFlightHeight < VMManager.Instance.MinOCH)
                        {
                            trackFlightAltitude = VMManager.Instance.MinOCH + GlobalVars.CurrADHP.Elev;
                            trackFlightHeight = VMManager.Instance.MinOCH;
                        }

                        parameterObjects.Add(new ParameterObject("Prescribed track length", trackLength, UoM.Distance));
                        //parameterObjects.Add(new ParameterObject("Prescribed track initial position altitude", VMManager.Instance.TrackInitialPositionAltitude, UoM.Height));
                        parameterObjects.Add(new ParameterObject("Prescribed track min. flight altitude", trackFlightAltitude, UoM.Height));

                        //double altitudeDiff = VMManager.Instance.TrackInitialPositionAltitude - trackFlightAltitude;
                        //double straightSegmentLength = 0;
                        //for (int i = 0; i < VMManager.Instance.TrackSegmentsList.Count; i++)
                        //{
                        //    if (!VMManager.Instance.TrackSegmentsList[i].IsFinalSegment)
                        //    {
                        //        straightSegmentLength += VMManager.Instance.TrackSegmentsList[i].DistanceTillDivergence;
                        //        straightSegmentLength += VMManager.Instance.TrackSegmentsList[i].DistanceTillConvergence;
                        //        straightSegmentLength += VMManager.Instance.TrackSegmentsList[i].DistanceTillEndPoint;
                        //    }
                        //}

                        //double requiredDescentGradient = altitudeDiff / straightSegmentLength * 100;

                        //parameterObjects.Add(new ParameterObject("Required min. descent gradient on straight segments", requiredDescentGradient, UoM.Gradient));

                        if (VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].IsFinalSegment)
                        {
                            VMManager.Instance.ActualFinalSegmentLength = ARANFunctions.ReturnDistanceInMeters(trackLastStep.StartPointPrj, VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR]);
                            //VMManager.Instance.ActualFinalSegmentTime = VMManager.Instance.ActualFinalSegmentLength / VMManager.Instance.FinalSegmentTASWind;
                            VMManager.Instance.ActualFinalSegmentTime = VMManager.Instance.ActualFinalSegmentLength / VMManager.Instance.VM_TASWind;
                            VMManager.Instance.ActualRateOfDescent = (trackFlightAltitude - VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z - 15) / (VMManager.Instance.ActualFinalSegmentTime / 60);
                            parameterObjects.Add(new ParameterObject("Final segment length", VMManager.Instance.TrackSegmentsList[VMManager.Instance.TrackSegmentsList.Count - 1].Length, UoM.Distance));
                            //parameterObjects.Add(new ParameterObject("Final segment rate of descent", VMManager.Instance.ActualRateOfDescent, UoM.RateOfDescent));

                            VMManager.Instance.DescentGradient = (trackFlightAltitude - VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z - 15) / VMManager.Instance.ActualFinalSegmentLength * 100;

                            parameterObjects.Add(new ParameterObject("Final segment descent gradient", VMManager.Instance.DescentGradient, UoM.Gradient));                            
                        }                        

                        reportForm.FillPageTrackSteps(VMManager.Instance.TrackSegmentsList);
                        reportForm.FillPageTrackObstacles(TrackObstacles);
                        reportForm.FillPageTrackResults(parameterObjects);
                    }
                }

                reportForm.Show(this);
            }
            else
            {
                reportForm.Close();
            }
        }

        private void btn_Parameters_Click(object sender, EventArgs e)
        {
            List<ParameterObject> parameterObjects = new List<ParameterObject>();
            parameterObjects.Add(new ParameterObject("Circling Area Construction", 0, UoM.Empty));
            if (_currentPageIndex >= 0)
            {
                switch (VMManager.Instance.Category)
                {
                    case 0:
                        parameterObjects.Add(new ParameterObject("Aircraft category", 0, UoM.Empty, "A"));
                        break;
                    case 1:
                        parameterObjects.Add(new ParameterObject("Aircraft category", 0, UoM.Empty, "B"));
                        break;
                    case 2:
                        parameterObjects.Add(new ParameterObject("Aircraft category", 0, UoM.Empty, "C"));
                        break;
                    case 3:
                        parameterObjects.Add(new ParameterObject("Aircraft category", 0, UoM.Empty, "D"));
                        break;
                    case 5:
                        parameterObjects.Add(new ParameterObject("Aircraft category", 0, UoM.Empty, "E"));
                        break;
                }
            }

            parameterObjects.Add(new ParameterObject("Circling area IAS", VMManager.Instance.CA_IAS, UoM.Speed));            
            parameterObjects.Add(new ParameterObject("Circling area IAS to TAS altitude", VMManager.Instance.CA_OCA, UoM.Height));
            parameterObjects.Add(new ParameterObject("Circling area TAS", VMManager.Instance.CA_TAS, UoM.Speed));
            parameterObjects.Add(new ParameterObject("Wind speed", GlobalVars.pansopsCoreDatabase.ArVisualWs.Value, UoM.Speed));
            parameterObjects.Add(new ParameterObject("Circling area TAS+Wind", VMManager.Instance.CA_TASWind, UoM.Speed));
            parameterObjects.Add(new ParameterObject("Circling area bank angle", ARANMath.RadToDeg(VMManager.Instance.CA_BankAngle), UoM.Angle));
            parameterObjects.Add(new ParameterObject("Circling area rate of turn", VMManager.Instance.CA_RateOfTurn, UoM.RateOfTurn));
            parameterObjects.Add(new ParameterObject("Circling area radius of turn", VMManager.Instance.CA_RadiusOfTurn, UoM.Distance));
            parameterObjects.Add(new ParameterObject("Circling area straight segment", VMManager.Instance.CA_StraightSegment, UoM.Distance));
            parameterObjects.Add(new ParameterObject("Circling area radius from threshold", VMManager.Instance.CA_RadiusFromTHR, UoM.Distance));                        
            parameterObjects.Add(new ParameterObject("Minimum OCA", VMManager.Instance.MinOCA, UoM.Height));
            parameterObjects.Add(new ParameterObject("Minimum OCH", VMManager.Instance.MinOCH, UoM.Height));
            parameterObjects.Add(new ParameterObject("Circling area OCA", VMManager.Instance.CA_OCA, UoM.Height));
            parameterObjects.Add(new ParameterObject("Circling area OCH", VMManager.Instance.CA_OCH, UoM.Height));
            parameterObjects.Add(new ParameterObject("Aerodrome elevation", GlobalVars.CurrADHP.Elev, UoM.Height));

            
            if (_currentPageIndex > 0)
            {
                //parameterObjects.Add(new ParameterObject("RWY and Navaid Selection", 0, UoM.Empty));
                parameterObjects.Add(new ParameterObject("Selected RWY THR", 0, UoM.Empty, VMManager.Instance.SelectedRWY.Name));
                parameterObjects.Add(new ParameterObject("Selected RWY THR Elevation", VMManager.Instance.SelectedRWY.pPtGeo[eRWY.ptTHR].Z, UoM.Height));
                //parameterObjects.Add(new ParameterObject("Final navaid", 0, UoM.Empty, VMManager.Instance.FinalNavaid.Name));
                //parameterObjects.Add(new ParameterObject("Circling area entrance direction (TRUE BRG)", GlobalVars.pspatialReferenceOperation.DirToAztPrj(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.SelectedDesignatedPoint.Location.Geo), VMManager.Instance.TrueBRGAngle), UoM.Angle));
                parameterObjects.Add(new ParameterObject("Circling area entrance direction (TRUE BRG)", GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.TrackInitialPosition, VMManager.Instance.TrackInitialDirection), UoM.Angle));
                
                parameterObjects.Add(new ParameterObject("Visual Manoeuvring bank angle", ARANMath.RadToDeg(VMManager.Instance.VM_BankAngle), UoM.Angle));
                parameterObjects.Add(new ParameterObject("Visual Manoeuvring IAS to TAS altitude", VMManager.Instance.VM_OCA, UoM.Height));
                parameterObjects.Add(new ParameterObject("Final segment IAS to TAS altitude", VMManager.Instance.VM_OCA, UoM.Height));
                parameterObjects.Add(new ParameterObject("Min. AD visibility", VMManager.Instance.VisibilityDistance, UoM.Distance));
            }

            
            if (_currentPageIndex > 1)
            {
                parameterObjects.Add(new ParameterObject("Circuit Construction", 0, UoM.Empty));
                parameterObjects.Add(new ParameterObject("Visual Manoeuvring IAS", VMManager.Instance.VM_IAS, UoM.Speed));                
                parameterObjects.Add(new ParameterObject("Visual Manoeuvring TAS + Wind", VMManager.Instance.VM_TASWind, UoM.Speed));
                parameterObjects.Add(new ParameterObject("Final segment IAS", VMManager.Instance.FinalSegmentIAS, UoM.Speed));                
                parameterObjects.Add(new ParameterObject("Final segment TAS+Wind", VMManager.Instance.FinalSegmentTASWind, UoM.Speed));
                parameterObjects.Add(new ParameterObject("Final segment time", VMManager.Instance.FinalSegmentTime, UoM.Time));
                parameterObjects.Add(new ParameterObject("Final segment length", VMManager.Instance.FinalSegmentLength, UoM.Distance));
                //parameterObjects.Add(new ParameterObject("Min rate of descent", VMManager.Instance.MinRateOfDescent, UoM.RateOfDescent));
                //parameterObjects.Add(new ParameterObject("Max rate of descent", VMManager.Instance.MaxRateOfDescent, UoM.RateOfDescent));
                parameterObjects.Add(new ParameterObject("Righthand circuit OCH", VMManager.Instance.RighthandCircuitOCH, UoM.Height));
                parameterObjects.Add(new ParameterObject("Lefthand circuit OCH", VMManager.Instance.LefthandCircuitOCH, UoM.Height));                
                
                parameterObjects.Add(new ParameterObject("Bank establishment time", VMManager.Instance.BankEstablishmentTime, UoM.Time));
                parameterObjects.Add(new ParameterObject("Pilot reaction time", VMManager.Instance.PilotReactionTime, UoM.Time));
                //parameterObjects.Add(new ParameterObject("Final segment start point altitude", VMManager.Instance.FinalSegmentStartPointAltitude, UoM.Height));
                //parameterObjects.Add(new ParameterObject("Descent gradient", VMManager.Instance.DescentGradient, UoM.Gradient));
            }

            if (!isParameterFormOpen)
            {
                parametersForm = new Parameters(this);
                parametersForm.FillParameterList(parameterObjects);
                parametersForm.Show(this);
                isParameterFormOpen = true;
            }
            else
            {
                parametersForm.Close();
                isParameterFormOpen = false;
            }
        }

        private void btn_DrawnElements_Click(object sender, EventArgs e)
        {
            if (!isDrawnElementsFormOpen)
            {
                drawnElementsForm = new DrawnElements(this, _currentPageIndex);
                drawnElementsForm.Show(this);
                isDrawnElementsFormOpen = true;
            }
            else
            {
                drawnElementsForm.Close();
                isDrawnElementsFormOpen = false;
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.json)|*.json";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    JArray jsonArray = new JArray();
                    foreach (VM_TrackSegment trackStep in VMManager.Instance.TrackSegmentsList)
                    {
                        JObject obj = trackStep.ToJson();
                        if(obj != null)
                            jsonArray.Add(obj);
                    }
                    
                    JObject tempObj = new JObject(new JProperty("effectiveDate", DBModule.pObjectDir.TimeSlice.EffectiveDate),
                        new JProperty("airportHeliportDesignator", GlobalVars.CurrADHP.pAirportHeliport.Designator),
                        //new JProperty("finalNavaidName", VMManager.Instance.FinalNavaid.Name),
                        new JProperty("selectedRWYName", VMManager.Instance.SelectedRWY.Name),
                        new JProperty("segments", jsonArray));
                    StreamWriter writer = new StreamWriter(myStream);                    

                    string str = JsonConvert.SerializeObject(tempObj);
                    writer.WriteLine(str);
                    writer.Close();
                    myStream.Close();
                    MessageBox.Show("Done!");
                }
            }
        }

        private void btn_newVisualFeature_Click(object sender, EventArgs e)
        {
            if (!isVisualFeatureCreaterOpen)
            {
                vfCreatorForm = new VisualFeatureCreator(this, _page1);
                vfCreatorForm.Show(this);
                isVisualFeatureCreaterOpen = true;
            }
            else
            {
                vfCreatorForm.Close();
                isVisualFeatureCreaterOpen = false;
            }
        }
    }
}
