using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AIP.BaseLib.Airac;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.CommonUtil.Util;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace AIP.DataSet
{
    public partial class Main : Telerik.WinControls.UI.RadForm
    {
        private AiracCycleDateTime airacBox = new AiracCycleDateTime();
        private AiracCycleDateTime airacBox2 = new AiracCycleDateTime();
        public Main()
        {
            InitializeComponent();
        }

        private void UpdateTitle()
        {
            try
            {
                this.Text = Common.TitleVersion();
                //this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        #region Init
        private void InitializeAiracControl()
        {
            try
            {
                DateTime airacDate = AiracCycle.AiracCycleList?.FirstOrDefault(x => x.Airac > DateTime.UtcNow.AddDays(42))?.Airac ?? DateTime.UtcNow;
                airacBox.DateTimeFormat = "yyyy - MM - dd  HH:mm";
                airacBox.DTValue = airacDate.ToString("yyyy - MM - dd");// "09.11.2017";
                airacBox.Name = "airacBox";
                airacBox.SelectionMode = (AiracCycle.AiracCycleList?.Any(d => d.Airac == airacDate) == true) ? AiracSelectionMode.Airac : AiracSelectionMode.Custom;
                airacBox.Size = new System.Drawing.Size(289, 21);
                airacBox.TabIndex = 7;
                airacBox.Value = airacDate;
                airacBox.Dock = DockStyle.Fill;
                airacPanel.Controls.Add(airacBox);

                DateTime airacDate2 = airacDate.AddDays(28);
                airacBox2.DateTimeFormat = "yyyy - MM - dd  HH:mm";
                airacBox2.DTValue = airacDate2.ToString("yyyy - MM - dd");// "09.11.2017";
                airacBox2.Name = "airacBox2";
                airacBox2.SelectionMode = AiracSelectionMode.Custom;
                airacBox2.Size = new System.Drawing.Size(289, 21);
                airacBox2.TabIndex = 7;
                airacBox2.Value = airacDate2;
                airacBox2.Dock = DockStyle.Fill;

                airacPanel2.Controls.Add(airacBox2);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Common.Errors = ((int)cbx_Interpretation.SelectedValue >= (int)InterpretationTypes.AllStatesInRange) ? Common.ErrorLevel.SuppressErrors : Common.ErrorLevel.Unknown;
                Common.Errors = Properties.Settings.Default.SuppressErrors ? Common.ErrorLevel.SuppressErrors : Common.ErrorLevel.Unknown;
                Common.ErrorNumber.Clear();
                log_output.Clear();
                InitAipData();
                progress.StartWaiting();
                gbxDataSet.Enabled = false;
                radMenuItem1.Enabled = false;
                radMenuItem2.Enabled = false;
                progress.Visible = true;
                Task taskA = Task.Factory.StartNew(() => RunAsync());

                for (; ; )
                {
                    Application.DoEvents();
                    lock (Common.OutputQueue)
                    {
                        while (Common.OutputQueue.Count > 0)
                        {
                            Common.Output msg = Common.OutputQueue.Dequeue();
                            AddOutput(msg.Message, msg.Color, msg.Style);
                        }
                        if (taskA.IsCompleted || taskA.IsFaulted) break;
                    }
                }
                taskA.Wait();
                ReportResult();
                Application.DoEvents();
                Common.CreateZipFile();
                AddOutput($@"AIP data set package ZIP file has been created!", null, null, false);
                btnFileOpen.Enabled = true;
                btnFolderOpen.Enabled = true;
                btnFileAnalize.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                progress.StopWaiting();
                radMenuItem1.Enabled = true;
                radMenuItem2.Enabled = true;
                gbxDataSet.Enabled = true;
                progress.Visible = false;
            }
        }

        private void ReportResult()
        {
            try
            {
                AddOutput("");
                if (Common.Errors.HasFlag(Common.ErrorLevel.RefLink))
                    AddOutput($@"Error occured: {Common.ErrorNumber[Common.ErrorLevel.RefLink]} reference links aren`t available.", Color.DarkRed, FontStyle.Bold);
                if (Common.Errors.HasFlag(Common.ErrorLevel.SubLink))
                    AddOutput("Error occured: One or more subreference links aren`t available.", Color.DarkRed, FontStyle.Bold);
                //if (Common.Errors.HasFlag(Common.ErrorLevel.RefLink) || Common.Errors.HasFlag(Common.ErrorLevel.SubLink))
                //    AddOutput("AIP data set generate process has been failed.", Color.DarkRed, FontStyle.Bold);
                if (Common.Errors.HasFlag(Common.ErrorLevel.SuppressErrors) || Common.Errors.HasFlag(Common.ErrorLevel.Success))
                    AddOutput("AIP data set file generation process has been completed!", Color.Black, FontStyle.Bold);
                AddOutput("");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private void AddOutputHeader()
        {
            try
            {
                AddOutput($@"{Common.TitleVersion()} Report.");
                AddOutput($@"Publishing organisation: {AIP.Company}");
                AddOutput($@"Current date (UTC): {DateTime.UtcNow.ToLongFormatString()}");
                if (AIP.IsUnpublished)
                {
                    AddOutput(@"Attention: Contains Pending Data", Color.DarkOrange, FontStyle.Bold);
                    if (!string.IsNullOrEmpty(AIP.ProjectName)) AddOutput($@"Project name: {AIP.ProjectName}", Color.DarkOrange, FontStyle.Bold);
                    if (!string.IsNullOrEmpty(AIP.SpaceName)) AddOutput($@"Space name: {AIP.SpaceName}", Color.DarkOrange, FontStyle.Bold);
                }
                if (Properties.Settings.Default.SuppressErrors)
                {
                    AddOutput(@"Attention: Suppress errors is enabled", Color.DarkOrange, FontStyle.Bold);
                }
                //if ((int)AIP.Interpretation > (int)InterpretationTypes.AllStatesInRange)
                //{
                //    AddOutput(@"Attention: Because of partially requested states, the reference errors are suppressed", Color.DarkOrange, FontStyle.Bold);
                //}
                AddOutput(new String('-', 40));
                AddOutput($@"ICAO Country Code: {AIP.Country}");
                AddOutput(@"AIP Data Set Type: " + AIP.Interpretation.GetEnumDescription());
                //AddOutput(@"Generated by " + TitleVersion());
                if ((int)AIP.Interpretation >= (int)InterpretationTypes.AllStatesInRange)
                {
                    AddOutput($@"Date/Time from (UTC): {AIP.EffectiveDate.ToLongFormatString()}", Color.Black, FontStyle.Bold);
                    AddOutput($@"Date/Time to (UTC): {AIP.CancelDate.ToLongFormatString()}", Color.Black, FontStyle.Bold);
                }
                else
                {
                    if (AIP.IsAIRAC)
                    {
                        AddOutput($@"Effective Date: {AIP.EffectiveDate.ToShortFormatString()}", Color.Black, FontStyle.Bold);
                        AddOutput(@"AIRAC: Yes");
                    }
                    else
                    {
                        AddOutput($@"Effective Date/Time (UTC): {AIP.EffectiveDate.ToLongFormatString()}", Color.Black, FontStyle.Bold);
                        AddOutput(@"AIRAC: No");
                    }
                }
                AddOutput($@"File Name: {AIP.DataSetFile}");
                AddOutput(new String('-', 40));
                AddOutput($@"Starting AIP Data Set Generation...{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void InitAipData()
        {
            try
            {
                AIP.EffectiveDate = airacBox.Value;
                AIP.CancelDate = airacBox2.Value;
                AIP.Company = Properties.Settings.Default.Organization;
                AIP.Country = tbxCountry.Text;
                AIP.IsAIRAC = AiracCycle.AiracCycleList.Any(d => d.Airac == AIP.EffectiveDate);
                AIP.IsUnpublished = chkUnpublishedData.Checked;
                AIP.Interpretation = (InterpretationTypes)cbx_Interpretation.SelectedValue;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private async Task RunAsync()
        {
            try
            {
                Cache.Clear();
                // Filling Project and Space, and deactivating if required
                Globals.InitializeConnection();
                Globals.FillActiveSlotAIPData();
                //Globals.DeactivatePrivateSlot();

                // Initializing AIP properties, such as folder path, etc
                AIP.Initialize();

                // Adding header info to log
                AddOutputHeader();

                // Step 1. Adding list of features require to receive from AIXM
                List<DataSetRule> dataSetRules = new List<DataSetRule>
                {
                    DefineRequiredFeatures(FeatureType.Airspace),
                    DefineRequiredFeatures(FeatureType.Route),
                    DefineRequiredFeatures(FeatureType.DesignatedPoint),
                    DefineRequiredFeatures(FeatureType.AirportHeliport),
                    DefineRequiredFeatures(FeatureType.Runway),
                    DefineRequiredFeatures(FeatureType.TouchDownLiftOff),
                    DefineRequiredFeatures(FeatureType.Navaid),
                    DefineRequiredFeatures(FeatureType.AeronauticalGroundLight),
                    DefineRequiredFeatures(FeatureType.HoldingPattern)
                };

                // Step 2. Receiving these features into Cache
                FillFeaturesCache(dataSetRules);

                // Clear before begin collecting features
                Common.DataSet.Clear();

                // Step 3. Configuring required subfeatures by Main Feature ID + Collecting features with references
                // Filling Common.DataSet with all required features
                AddOutput($@"{Environment.NewLine}2. Analizing Data{Environment.NewLine}", Color.Black, FontStyle.Bold);
                CodingDataSets(FeatureType.Airspace);
                CodingDataSets(FeatureType.Route);
                CodingDataSets(FeatureType.DesignatedPoint);
                CodingDataSets(FeatureType.AirportHeliport);
                CodingDataSets(FeatureType.Runway);
                CodingDataSets(FeatureType.TouchDownLiftOff);
                CodingDataSets(FeatureType.Navaid);
                CodingDataSets(FeatureType.AeronauticalGroundLight);
                CodingDataSets(FeatureType.HoldingPattern);

                // Getting Unique values of all collected features
                //Common.DataSet = Common.DataSet?.Distinct(FeatureComparer.Instance).ToList();

                // Order by FeatureType, then by Identifier (order require for possible better text compare 2 aip data set in the future)
                // Order by FeatureType goes first for counting written Types in the WriteAipDataSet to show in the Report
                //Common.DataSet = Common.DataSet?.OrderBy(x=>x.FeatureType).ThenBy(x=>x.Identifier).ToList();

                // Getting BaseLine or TempDelta, if requested
                if (AIP.Interpretation == InterpretationTypes.Snapshot)
                {
                    Common.DataSet = Common.DataSet?.Where(x =>
                    x.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE ||
                    (x.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA &&
                    (x.TimeSlice.ValidTime.EndPosition == null ||
                    x.TimeSlice.ValidTime.BeginPosition.AddMonths(3) >= x.TimeSlice.ValidTime.EndPosition)))
                    .ToList();
                    AddOutput($@"{Environment.NewLine}Filtered TimeSlices with interpretation TEMPDELTA that have a validity period of three months or longer.");
                }
                else if (AIP.Interpretation == InterpretationTypes.TempDeltaStatesInRange)
                {
                    Common.DataSet = Common.DataSet?.Where(x => x.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA).ToList();
                    AddOutput($@"{Environment.NewLine}Filtered TimeSlices with interpretation TEMPDELTA.");
                }

                // If no errors, writing all collected features into xml
                // Contains excluded feature>references entire WriteAipDataSet\WriteFeature\
                if (Common.Errors == Common.ErrorLevel.Unknown || Common.Errors.HasFlag(Common.ErrorLevel.SuppressErrors))
                {
                    Common.Errors = Common.ErrorLevel.Success;
                    Common.WriteAipDataSet();
                }
                else if (Common.Errors != Common.ErrorLevel.Success)
                {
                    Application.DoEvents(); // To show last errors
                    DialogResult dialogResult = MessageBox.Show(
                        @"One or more errors occured. Do you want to continue create AIP Data Set file?",
                        @"Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Common.Errors |= Common.ErrorLevel.SuppressErrors;
                        Common.WriteAipDataSet();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                //Globals.ActivatePrivateSlot();
            }
        }

        private void FillFeaturesCache(List<DataSetRule> dataSetRules)
        {
            string output = "";
            try
            {
                List<Feature> ft = new List<Feature>();
                List<Feature> refft = new List<Feature>();
                List<Feature> subft = new List<Feature>();
                AddOutput($@"1. Receiving Data{Environment.NewLine}", Color.Black, FontStyle.Bold);
                AddOutput("Receiving features from AIXM.");
                bool StatesCountReq = (int) AIP.Interpretation >= (int) InterpretationTypes.Snapshot;
                bool isSnapshot = (int) AIP.Interpretation == (int) InterpretationTypes.Snapshot;
                foreach (DataSetRule feat in dataSetRules)
                {
                    var featList = Globals.GetFeaturesByED(feat.MainFeatureType);
                    int cntAll = featList.Count;
                    if (StatesCountReq)
                    {
                        int cntUnique = featList.Distinct(FeatureComparer.Instance).ToList().Count;
                        int cntBL = featList.Count(x =>
                            x.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE);
                        int cntTD = featList.Count(x =>
                            x.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA);
                        string states = isSnapshot ? "" : $" States: {cntAll},";
                        output = $"(Total: {cntUnique},{states} BaseLine: {cntBL}, TempDelta: {cntTD})";
                    }
                    else output = $"(Total: {cntAll})";
                    AddOutput($"{Environment.NewLine}> {feat.MainFeatureType.ToString()} {output}", null, FontStyle.Bold);
                    if (feat.RefFeatureTypes != null)
                    {
                        foreach (FeatureType subtype in feat.RefFeatureTypes)
                        {
                            featList = Globals.GetFeaturesByED(subtype);
                            cntAll = featList.Count;
                            if (StatesCountReq)
                            {
                                int cntUnique = featList.Distinct(FeatureComparer.Instance).ToList().Count;
                                int cntBL = featList.Count(x =>
                                    x.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE);
                                int cntTD = featList.Count(x =>
                                    x.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA);
                                string states = isSnapshot ? "" : $" States: {cntAll},";
                                output = $"(Total: {cntUnique},{states} BaseLine: {cntBL}, TempDelta: {cntTD})";
                            }
                            else output = $"(Total: {cntAll})";
                            AddOutput($">>    {subtype.ToString()} {output}");
                        }
                    }

                    if (feat.SubFeatureTypes != null)
                    {
                        foreach (FeatureType subtype in feat.SubFeatureTypes)
                        {
                            featList = Globals.GetFeaturesByED(subtype);
                            cntAll = featList.Count;
                            if (StatesCountReq)
                            {
                                int cntUnique = featList.Distinct(FeatureComparer.Instance).ToList().Count;
                                int cntBL = featList.Count(x =>
                                    x.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE);
                                int cntTD = featList.Count(x =>
                                    x.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA);
                                string states = isSnapshot ? "" : $" States: {cntAll},";
                                output = $"(Total: {cntUnique},{states} BaseLine: {cntBL}, TempDelta: {cntTD})";
                            }
                            else output = $"(Total: {cntAll})";
                            AddOutput($"<<    {subtype.ToString()} {output}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private DataSetRule DefineRequiredFeatures(FeatureType FeatureType)
        {
            try
            {
                DataSetRule dataSetRule = new DataSetRule { MainFeatureType = FeatureType };
                switch (FeatureType)
                {
                    case FeatureType.Airspace:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.Route,
                            FeatureType.OrganisationAuthority,
                            FeatureType.StandardLevelColumn,
                            FeatureType.Unit,
                            FeatureType.RadioCommunicationChannel,
                            FeatureType.DesignatedPoint,
                        };
                        dataSetRule.SubFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.AirTrafficControlService,
                            FeatureType.InformationService,
                            FeatureType.SearchRescueService,
                            FeatureType.SignificantPointInAirspace, // ToDo: Not Specified in the EC Specification. Added by Teodor request, for Andrey Grishanovs Visualizer. 
                        };
                        break;
                    case FeatureType.Route:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.OrganisationAuthority,
                            FeatureType.StandardLevelColumn,
                            FeatureType.RadioFrequencyArea,
                            FeatureType.DesignatedPoint,
                            FeatureType.Navaid,
                            FeatureType.AirportHeliport,
                            FeatureType.AirTrafficControlService,
                            FeatureType.Unit,
                            FeatureType.RadioCommunicationChannel,
                            FeatureType.VOR,
                            FeatureType.DME,
                            FeatureType.Airspace
                        };
                        dataSetRule.SubFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.RouteSegment,
                            FeatureType.AirTrafficControlService,
                            FeatureType.InformationService,
                            FeatureType.SearchRescueService
                        };
                        break;
                    case FeatureType.DesignatedPoint:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.TouchDownLiftOff,
                            FeatureType.AirportHeliport,
                            FeatureType.RunwayCentrelinePoint,
                            FeatureType.Navaid,
                            FeatureType.Azimuth, //NavaidEquipment
                            FeatureType.DirectionFinder, //NavaidEquipment
                            FeatureType.DME, //NavaidEquipment
                            FeatureType.Elevation, //NavaidEquipment
                            FeatureType.Glidepath, //NavaidEquipment
                            FeatureType.Localizer, //NavaidEquipment
                            FeatureType.MarkerBeacon, //NavaidEquipment
                            FeatureType.NDB, //NavaidEquipment
                            FeatureType.SDF, //NavaidEquipment
                            FeatureType.TACAN, //NavaidEquipment
                            FeatureType.VOR, //NavaidEquipment
                            FeatureType.Localizer,
                            FeatureType.Glidepath,
                            FeatureType.RunwayDirection,
                            FeatureType.Runway,
                            FeatureType.OrganisationAuthority,
                        };
                        dataSetRule.SubFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.DistanceIndication,
                            FeatureType.AngleIndication,
                            FeatureType.SignificantPointInAirspace, // ToDo: Not Specified in the EC Specification. Added by Teodor request, for Andrey Grishanovs Visualizer
                        };
                        break;
                    case FeatureType.AirportHeliport:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.AltimeterSource
                        };
                        break;
                    case FeatureType.Runway:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.AirportHeliport,
                            FeatureType.RunwayElement,
                            FeatureType.OrganisationAuthority,
                        };
                        dataSetRule.SubFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.RunwayDirection,
                            FeatureType.RunwayCentrelinePoint,// point to prev
                            FeatureType.RunwayElement // ToDo: Not Specified in the EC Specification. Added by Gvido request, for Andrey Grishanovs Visualizer. Base: Email at 16/04/2018
                        };
                        break;
                    case FeatureType.TouchDownLiftOff:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.AirportHeliport,
                            FeatureType.Runway
                        };
                        break;
                    case FeatureType.Navaid:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.TouchDownLiftOff,
                            FeatureType.RunwayDirection,
                            FeatureType.AirportHeliport,
                            FeatureType.Azimuth, //NavaidEquipment
                            FeatureType.DirectionFinder, //NavaidEquipment
                            FeatureType.DME, //NavaidEquipment
                            FeatureType.Elevation, //NavaidEquipment
                            FeatureType.Glidepath, //NavaidEquipment
                            FeatureType.Localizer, //NavaidEquipment
                            FeatureType.MarkerBeacon, //NavaidEquipment
                            FeatureType.NDB, //NavaidEquipment
                            FeatureType.SDF, //NavaidEquipment
                            FeatureType.TACAN, //NavaidEquipment
                            FeatureType.VOR, //NavaidEquipment
                            FeatureType.OrganisationAuthority
                        };
                        break;
                    case FeatureType.AeronauticalGroundLight:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.VerticalStructure,
                            FeatureType.AirportHeliport
                        };
                        break;
                    case FeatureType.HoldingPattern:
                        dataSetRule.RefFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.RadioFrequencyArea,
                            FeatureType.DesignatedPoint,
                            FeatureType.Navaid,
                            FeatureType.Unit,
                            FeatureType.RadioCommunicationChannel,
                            FeatureType.Azimuth, //NavaidEquipment
                            FeatureType.DirectionFinder, //NavaidEquipment
                            FeatureType.DME, //NavaidEquipment
                            FeatureType.Elevation, //NavaidEquipment
                            FeatureType.Glidepath, //NavaidEquipment
                            FeatureType.Localizer, //NavaidEquipment
                            FeatureType.MarkerBeacon, //NavaidEquipment
                            FeatureType.NDB, //NavaidEquipment
                            FeatureType.SDF, //NavaidEquipment
                            FeatureType.TACAN, //NavaidEquipment
                            FeatureType.VOR, //NavaidEquipment
                            FeatureType.AirportHeliport,
                            FeatureType.OrganisationAuthority
                        };
                        dataSetRule.SubFeatureTypes = new List<FeatureType>()
                        {
                            FeatureType.AirTrafficControlService,
                            FeatureType.InformationService
                        };
                        break;
                }


                return dataSetRule;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void CodingDataSets(FeatureType featureType)
        {
            try
            {
                List<Feature> subFeat = new List<Feature>();
                // Getting main Guid Ids
                List<Guid> guidList = Cache.Get(featureType)?.Select(x => x.Identifier).ToList();
                if (guidList != null)
                {
                    switch (featureType)
                    {
                        case FeatureType.Airspace:
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)
                                    .OfType<AirTrafficControlService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.InformationService).OfType<InformationService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.SearchRescueService)
                                    .OfType<SearchRescueService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.SignificantPointInAirspace)
                                    .OfType<SignificantPointInAirspace>()?
                                    .Where(x => x?.ContainingAirspace != null &&
                                                  guidList.Contains(x.ContainingAirspace.Identifier))
                                    .ToList());
                            break;
                        case FeatureType.Route:
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.RouteSegment)
                                    .OfType<RouteSegment>()?
                                    .Where(x => x?.RouteFormed != null && guidList.Contains(x.RouteFormed.Identifier))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)
                                    .OfType<AirTrafficControlService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.InformationService).OfType<InformationService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.SearchRescueService)
                                    .OfType<SearchRescueService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            break;
                        case FeatureType.DesignatedPoint:
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.DistanceIndication)
                                    .OfType<DistanceIndication>()?
                                    .Where(x => x?.Fix != null && guidList.Contains(x.Fix.Identifier))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.AngleIndication)
                                    .OfType<AngleIndication>()?
                                    .Where(x => x?.Fix != null && guidList.Contains(x.Fix.Identifier))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.SignificantPointInAirspace)
                                    .OfType<SignificantPointInAirspace>()?
                                    .Where(x => x?.Location != null && guidList.Contains(x.Location.FixDesignatedPoint.Identifier))
                                    .ToList());
                            break;
                        case FeatureType.AirportHeliport: // No sub
                            break;
                        case FeatureType.Runway:
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.RunwayDirection)
                                    .OfType<RunwayDirection>()?
                                    .Where(x => x?.UsedRunway != null && guidList.Contains(x.UsedRunway.Identifier))
                                    .ToList());
                            List<Guid> guidList2 = Cache.Get(FeatureType.RunwayDirection).Select(x => x.Identifier)
                                .ToList();
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.RunwayCentrelinePoint)
                                    .OfType<RunwayCentrelinePoint>()?
                                    .Where(x => x?.OnRunway != null && guidList2.Contains(x.OnRunway.Identifier))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.RunwayElement)
                                    .OfType<RunwayElement>()?
                                    .Where(x => x?.AssociatedRunway != null &&
                                           x.AssociatedRunway.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            break;
                        case FeatureType.TouchDownLiftOff: // No sub
                            break;
                        case FeatureType.AeronauticalGroundLight: // No sub
                            break;
                        case FeatureType.HoldingPattern:
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)
                                    .OfType<AirTrafficControlService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            subFeat.AddRange(
                                Globals.GetFeaturesByED(FeatureType.InformationService).OfType<InformationService>()?
                                    .Where(x => x?.ClientAirspace != null &&
                                                x.ClientAirspace.Any(y => guidList.Contains(y.Feature.Identifier)))
                                    .ToList());
                            break;
                    }
                }
                AddOutput($@"Analizing and validating {featureType} references features and properties.");

                // Collecting one Airspace
                Common.CollectAipDataSet(Cache.Get(featureType), subFeat);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void btnFolderOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(AIP.MainFolder))
                {
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", AIP.DataSetFileFullPath));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(AIP.DataSetFileFullPath))
                {
                    Process.Start(AIP.DataSetFileFullPath);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public delegate void AddOutputDelegate(string Action, Color? color = null, FontStyle? style = null, bool AddToFile = true);
        public void AddOutput(string Action, Color? color = null, FontStyle? style = null, bool AddToFile = true)
        {
            if (InvokeRequired)
            {
                Invoke(new AddOutputDelegate(AddOutput), Action, color, style, AddToFile);
            }
            else
            {
                if (color == null) color = Color.Black;
                //string fileName = Path.Combine(AIP.MainFolder, $@"Report_AIP_DataSet_{(AIP.IsAIRAC ? "AIRAC_" : "")}{AIP.EffectiveDate:yyyy-MM-dd}.log");
                string fileName = AIP.ReportFileFullPath;
                if (log_output.Text.Length == 0)
                {
                    if(AddToFile) File.AppendAllText(fileName, Action);
                    log_output.AppendColorText(Action, color, style);
                }
                else
                {
                    if (AddToFile) File.AppendAllText(fileName, Environment.NewLine + Action);
                    log_output.AppendColorText(Environment.NewLine + Action, color, style);
                }
                log_output.SelectionStart = log_output.Text.Length;
                log_output.ScrollToCaret();
            }
        }

        private void mnu_DataSetManager_Click(object sender, EventArgs e)
        {
            try
            {
                DataSetManager dsm = new DataSetManager();
                dsm.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void gbxReport_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {

        }

        private void log_output_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                var arr = e.LinkText.Split('/');
                if (arr[3] != null && arr[4] != null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    if (!String.IsNullOrEmpty(AIP.DataSetFileFullPath) && File.Exists(AIP.DataSetFileFullPath))
                    {
                        xDoc.Load(AIP.DataSetFileFullPath);
                        XmlElement xRoot = xDoc.DocumentElement;
                        XmlNamespaceManager ns = new XmlNamespaceManager(xDoc.NameTable);
                        ns.AddNamespace("aixm-5.1", "http://www.aixm.aero/schema/5.1");
                        ns.AddNamespace("gml", "http://www.opengis.net/gml/3.2");

                        XmlNode childnode =
                            xRoot?.SelectSingleNode($@"//aixm-5.1:{arr[3]}[@gml:id='urn.uuid.{arr[4]}']", ns);


                        if (childnode != null)
                        {
                            string xmlText =
                                $@"<?xml version=""1.0"" encoding=""utf-8""?><aixm-message-5.1:AIXMBasicMessage xsi:schemaLocation=""http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd"" gml:id=""gmlAranID1"" xmlns:aixm-5.1=""http://www.aixm.aero/schema/5.1"" xmlns:gml=""http://www.opengis.net/gml/3.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:aixm-message-5.1=""http://www.aixm.aero/schema/5.1/message"">{childnode.OuterXml}</aixm-message-5.1:AIXMBasicMessage>";
                            string post = @"</aixm-message-5.1:AIXMBasicMessage>";

                            //Debug.WriteLine(childnode.OuterXml);
                            XmlViewer xmlViewer = new XmlViewer { wb = { DocumentText = xmlText } };
                            xmlViewer.ShowDialog();
                        }
                        else
                        {
                            ErrorLog.ShowWarning($@"No such ID found");
                        }
                    }
                    else
                    {
                        ErrorLog.ShowWarning($@"File {AIP.DataSetFileFullPath ?? ""} doesn`t created to analyze it or not available");
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            
            InitializeAiracControl();
            cbx_Interpretation.DisplayMember = "Description";
            cbx_Interpretation.ValueMember = "Value";
            cbx_Interpretation.DataSource = Enum.GetValues(typeof(InterpretationTypes))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
            UpdateTitle();
            //Enum.GetValues(typeof(InterpretationTypes)).Cast<InterpretationTypes>();
            Program.CloseSplash();

            Visible = true;
            WindowState = FormWindowState.Maximized;
        }


        private void lnkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Globals.InitializeConnection();
                ErrorLog.ShowInfo(Globals.ActiveSlotMessage());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btnFileAnalyze_Click(object sender, EventArgs e)
        {

            DataSetViewer frm = new DataSetViewer();
            if (AIP.DataSetFileFullPath != null && File.Exists(AIP.DataSetFileFullPath))
            {
                XmlFileConnection.fileName = AIP.DataSetFileFullPath;
                frm.Show();
            }
        }

        private void mnu_Analyze_Click(object sender, EventArgs e)
        {
            DataSetViewer frm = new DataSetViewer();
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = AIP.MainFolder;
                dialog.Filter = @"Xml files (*.xml)|*.Xml";
                dialog.Title = @"Select a AIP Data Set Xml file";
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK) 
                {
                    if (File.Exists(dialog.FileName))
                    {
                        XmlFileConnection.fileName = dialog.FileName;
                        frm.Show();
                    }
                }
            }
        }

        private void mnu_About_Click(object sender, EventArgs e)
        {
            OpenAbout();
        }

        private void OpenAbout()
        {
            try
            {
                About stn = new About();
                DialogResult result = stn.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void mnu_Help_Click(object sender, EventArgs e)
        {
            string helpFile = @"Help\AIPDataset.chm";
            if (File.Exists(helpFile)) Help.ShowHelp(this, helpFile);
        }

        private void mnu_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void cbx_Interpretation_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            lbl_Description.Text = ((InterpretationTypes) cbx_Interpretation.SelectedValue).GetAttribute<LongDescription>()?.description;
            if (((int) cbx_Interpretation.SelectedValue >=
                 (int) InterpretationTypes.AllStatesInRange))
            {
                airacPanel2.Visible = true;
                airacBox.SelectionMode = AiracSelectionMode.Custom;
                lbl_AIRAC.Text = @"Period";
            }
            else
            {
                airacPanel2.Visible = false;
                lbl_AIRAC.Text = @"Effective Date";
                //airacBox.SelectionMode = AiracSelectionMode.Airac;
            }
        }

        private void mnu_Settings_Click(object sender, EventArgs e)
        {
            Settings frm = new Settings();
            frm.ShowDialog();
        }
    }
}
