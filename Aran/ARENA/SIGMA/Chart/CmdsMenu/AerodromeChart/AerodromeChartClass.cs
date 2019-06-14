using ANCOR.MapCore;
using ANCOR.MapElements;
using AranSupport;
using ARENA;
using ArenaStatic;
using ChartCodingTable;
using ChartCompare;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using PDM;
using SigmaChart.CmdsMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public class AerodromeChartClass :  AbstaractSigmaChart
    {

        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference = null;

        private IFeatureClass Anno_AirportCartography_featClass = null;
        private IFeatureClass Anno_AircraftStandCartography_featClass = null;
        private IFeatureClass Anno_AircraftStandExtentCartography_featClass = null;
        private IFeatureClass Anno_AirportHotSpotCartography_featClass = null;
        private IFeatureClass Anno_ApronElementCartography_featClass = null;
        private IFeatureClass Anno_DeicingAreaCartography_featClass = null;
        private IFeatureClass Anno_GuidanceLineCartography_featClass = null;
        private IFeatureClass Anno_NonMovementAreaCartography_featClass = null;
        private IFeatureClass Anno_RadioCommunicationChanelCartography_featClass = null;
        private IFeatureClass Anno_RoadCartography_featClass = null;
        private IFeatureClass Anno_NavaidsCartography_featClass = null;
        private IFeatureClass Anno_VertStructureSurfaceCartography_featClass = null;
        private IFeatureClass Anno_VerticalStructurePointCartography_featClass = null;
        private IFeatureClass Anno_VertStructureCurveCartography_featClass = null;
        private IFeatureClass Anno_RunwayVisualRangeCartography_featClass = null;
        private IFeatureClass Anno_TaxiHoldingPositionCartography_featClass = null;
        private IFeatureClass Anno_TouchDownLiftOffCartography_featClass = null;
        private IFeatureClass Anno_TouchDownLiftOffAimingPointCartography_featClass = null;
        private IFeatureClass Anno_TouchDownLiftOffSafeAreaCartography_featClass = null;
        private IFeatureClass Anno_UnitCartography_featClass = null;
        private IFeatureClass Anno_WorkAreaCartography_featClass = null;
        private IFeatureClass Anno_AirportHeliportExtentCartography_featClass = null;
        private IFeatureClass Anno_LightElementCartography_featClass = null;
        private IFeatureClass Anno_RunwayDirectionCartography_featClass = null;
        private IFeatureClass Anno_RunwayDirectionCenterLinePointCartography_featClass = null;
        private IFeatureClass Anno_RunwayElementCartography_featClass = null;
        private IFeatureClass Anno_RunwayProtectAreaCartography_featClass = null;
        private IFeatureClass Anno_TaxiwayElementCartography_featClass = null;
        private IFeatureClass Anno_CheckpointCartography_featClass = null;
        private IFeatureClass Anno_MarkingCurveCartography_featClass = null;
        private IFeatureClass Anno_MarkingPointCartography_featClass = null;
        private IFeatureClass Anno_MarkingSurfaceCartography_featClass = null;
        private IFeatureClass Anno_BirdAirspaceCartography_featClass = null;
        private IFeatureClass Anno_RadioFrequencyAreaCartography_featClass = null;


        private IFeatureClass Anno_DecorPointCartography_featClass = null;
        private IFeatureClass Anno_DecorLineCartography_featClass = null;
        private IFeatureClass Anno_DecorPolygonCartography_featClass = null;


        AirportHeliport selARP = null;
        UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        List<string> _arp_chanels = null;

        public AerodromeChartClass()
        {
        }

        public override void CreateChart()
        {
            object Missing = Type.Missing;

            var acDate = DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
            int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);
            AerodromeChartWizardForm frm = new AerodromeChartWizardForm(_AiracCircle, this.ChartType);


            if (frm.ShowDialog() == DialogResult.Cancel) return;

            _AiracCircle = frm._AiracCircle;


            AlertForm alrtForm = new AlertForm();

            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

            alrtForm.progressBar1.Visible = true;
            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.label1.Text = "Start process";
            alrtForm.label1.Visible = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            var projectName = frm._ProjectName.EndsWith(".mxd") ? frm._ProjectName : frm._ProjectName + ".mxd";
            if (Directory.Exists(frm._FolderName + @"\" + frm._ProjectName)) Directory.Delete(frm._FolderName + @"\" + frm._ProjectName, true);

            var destPath2 = System.IO.Directory.CreateDirectory(frm._FolderName + @"\" + frm._ProjectName).FullName;
            var tmpName = frm._TemplatetName;
            var rnavFlag = frm._RNAVflag;
            selARP = frm.selectedADHP;
            vertUom = frm.vertUom;
            distUom = frm.distUom;

            _arp_chanels = frm._selectedChanels;
            if (_arp_chanels == null) { _arp_chanels = TerminalChartsUtil.getChanelsList(selARP.CommunicationChanels); }
            IAOIBookmark extentBookmark = frm._bookmark;

            ILayer _Layer = EsriUtils.getLayerByName(this.SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            pSpatialReference = (fc as IGeoDataset).SpatialReference;

            alrtForm.label1.Text = "Preparation";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

           // TerminalChartsUtil.SetMapScale(this.SigmaHookHelper.ActiveView.FocusMap, frm.selectedIAP, this.SigmaApplication);
            this.SigmaHookHelper.ActiveView.FocusMap.MapScale = frm._mapScale;
            //this.SigmaHookHelper.ActiveView.Refresh();
            Application.DoEvents();

            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

            alrtForm.label1.Text = "Data quering";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

         

            #region initialization


            double AnnoScale = this.SigmaHookHelper.ActiveView.FocusMap.MapScale;

            alrtForm.label1.Text = "Initialization";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

            FocusMap = ChartsHelperClass.ChartsPreparation(this.ChartType, projectName, destPath2, tmpName, this.SigmaApplication);
            Application.DoEvents();
            if (FocusMap == null) return;


            #region Data query

            #region Query 

            var selectedRec = EsriUtils.ToGeo(extentBookmark.Location, this.SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);//(this.SigmaHookHelper.ActiveView.FocusMap as IActiveView).Extent;
            //selectedRec.SpatialReference = pSpatialReference;

            List<PDMObject> navaidsList = DataCash.GetAirportNavaidByAirportID(selARP.ID); //GetObjectsWithinPolygon(selectedRec, PDM_ENUM.NavaidSystem);
            List<PDMObject> vertStructureList = DataCash.GetAerodromeObstacles(); // GetAerodromeObstaclesWithinPolygon(selectedRec, this.SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);
            List<PDMObject> birdAirspace = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(arsps => arsps.PDM_Type == PDM_ENUM.Airspace && ((Airspace)arsps).ActivationDescription != null &&
                                                                        ((Airspace)arsps).ActivationDescription.Count > 0 && ((Airspace)arsps).ActivationDescription.IndexOf("BIRD") >= 0).ToList();

            List<PDMObject> RFA = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(rfa => rfa.PDM_Type == PDM_ENUM.RadioFrequencyArea).ToList();


            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
            this.EffectiveDate = start;
            this.ADHP = selARP.Designator != null ? selARP.Designator : "";
            this.airacCircle = _AiracCircle.ToString();
            this.organization = selARP.OrganisationAuthority != null ? selARP.OrganisationAuthority : "";
            if (selARP.RunwayList != null)
            {
                this.RunwayDirectionsList = new List<string>();
                foreach (var rwy in selARP.RunwayList)
                {
                    this.RunwayDirectionsList.Add(rwy.Designator);
                }
            }

            #endregion

            #region Store using data to pdm file


            List<PDMObject> PdmElementsList = new List<PDMObject>();

            //PdmElementsList.AddRange(DataCash.ProjectEnvironment.Data.PdmObjectList.GetRange(0, DataCash.ProjectEnvironment.Data.PdmObjectList.Count));
            PdmElementsList.Add(frm.selectedADHP);

            if (navaidsList != null && navaidsList.Count > 0) PdmElementsList.AddRange(navaidsList.GetRange(0, navaidsList.Count));
            if (vertStructureList != null && vertStructureList.Count > 0) PdmElementsList.AddRange(vertStructureList.GetRange(0, vertStructureList.Count));
            if (birdAirspace != null && birdAirspace.Count > 0) PdmElementsList.AddRange(birdAirspace.GetRange(0, birdAirspace.Count));

            ChartsHelperClass.SaveSourcePDMFiles(destPath2, PdmElementsList);
            PdmElementsList = null;

            #endregion

            #endregion

            alrtForm.label1.Text = "Maps projection settings";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

            #region ChangeProjectionAndMeredian

            if (selARP.Geo == null) selARP.RebuildGeo();
            if (selARP.Geo != null)
            {
                double newX = ((IPoint)selARP.Geo).X;
                EsriUtils.ChangeProjectionAndMeredian(newX, FocusMap);
            }
            #endregion


            FocusMap.MapScale = AnnoScale;

            var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AerodromeElectronicChart\", "aerodrome.sce");
            SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.AerodromeElectronicChart;

            if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }


            alrtForm.label1.Text = "Work space initialization";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();
            SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

            #endregion


            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.BringToFront();
            Application.DoEvents();


            #region save chart info

            DateTime tmpStart = DateTime.MinValue, tmpEnd = DateTime.MinValue;
            AiracUtil.AiracUtil.GetAiracCirclePeriod(int.Parse(this.airacCircle), ref tmpStart, ref tmpEnd);
            EsriWorkEnvironment.chartInfo ci = new chartInfo
            {
                ADHP = this.ADHP != null ? this.ADHP : "",
                airacCircle = this.airacCircle != null ? this.airacCircle : "",
                efectiveDate = this.efectiveDate != null ? this.efectiveDate : DateTime.Now,
                organization = this.organization != null ? this.organization : "",
                chartName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.Combine(destPath2, projectName)),
                uomDist = distUom.ToString(),
                uomVert = vertUom.ToString(),
                widthBufer = -1,
                flag = false,
                publicationDate = tmpStart,
                baseTempName = frm._TemplatetName,
            };

            if (this.RunwayDirectionsList != null && this.RunwayDirectionsList.Count > 0)
            {
                ci.RunwayDirectionsList = new List<string>();
                ci.RunwayDirectionsList.AddRange(this.RunwayDirectionsList);
            }

            EsriWorkEnvironment.EsriUtils.StoreChartInfo(ci, (IFeatureWorkspace)SigmaDataCash.environmentWorkspaceEdit);

            #endregion

            alrtForm.label1.Text = "Creating annotations";
            alrtForm.progressBar1.Value++; alrtForm.BringToFront();

            #region Annotations creating

            FocusMap.Description = frm._mapAngle !=0 ? (frm._mapAngle - 90).ToString() : "0";

            if (selARP != null)
            {


                #region Birds & RadioFrequencyArea

                if (birdAirspace != null && birdAirspace.Count > 0)
                {
                    AerodromeChartUtils.CreateBirdPolygon(birdAirspace, Anno_BirdAirspaceCartography_featClass, FocusMap, pSpatialReference);

                }

                if (RFA != null && RFA.Count > 0)
                {
                    foreach (var item in RFA)
                    {
                        AerodromeChartUtils.CreateRadioFreqArea((RadioFrequencyArea)item, Anno_RadioFrequencyAreaCartography_featClass, FocusMap, pSpatialReference);

                    }
                }

                #endregion

                List<GuidanceLine> gdnsLnList = new List<GuidanceLine>();
                List<DeicingArea> dcngArea = new List<DeicingArea>();
                List<MarkingElement> mrkngList = new List<MarkingElement>();

                //alrtForm.label1.Text = "Creating annotations for runways";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                AerodromeChartUtils.CreateRunwayCartography(selARP.RunwayList, FocusMap, selARP.MagneticVariation,
                    pSpatialReference,
                    this.ChartType,
                    Anno_RunwayElementCartography_featClass,
                    Anno_RunwayProtectAreaCartography_featClass,
                    Anno_RunwayDirectionCenterLinePointCartography_featClass,
                    Anno_DecorLineCartography_featClass,
                    Anno_LightElementCartography_featClass,
                    Anno_TaxiHoldingPositionCartography_featClass,
                    Anno_RunwayVisualRangeCartography_featClass,
                    distUom, vertUom, ref gdnsLnList, ref mrkngList);

                //alrtForm.label1.Text = "Creating annotations for taxiways";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                AerodromeChartUtils.CreateTaxiwayCartography(selARP.TaxiwayList, FocusMap, pSpatialReference, Anno_TaxiwayElementCartography_featClass, distUom, ref gdnsLnList, ref dcngArea, ref mrkngList);


                //alrtForm.label1.Text = "Creating annotations for Vertical structures";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (vertStructureList != null && vertStructureList.Count > 0)
                    AerodromeChartUtils.CreateVerticalStructureCartography(vertStructureList, FocusMap, pSpatialReference, Anno_VertStructureSurfaceCartography_featClass, Anno_VerticalStructurePointCartography_featClass);




                //alrtForm.label1.Text = "Creating annotations for aprons";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                AerodromeChartUtils.CreateApronCartography(selARP.ApronList, FocusMap, pSpatialReference, Anno_ApronElementCartography_featClass, Anno_AircraftStandCartography_featClass,
                    Anno_AircraftStandExtentCartography_featClass, Anno_LightElementCartography_featClass, Anno_GuidanceLineCartography_featClass, distUom, ref gdnsLnList, ref dcngArea, ref mrkngList);

                //alrtForm.label1.Text = "Creating annotations for Checkpoints";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                AerodromeChartUtils.CreateCheckPoints(selARP.NavSystemCheckpoints, FocusMap, pSpatialReference, Anno_CheckpointCartography_featClass);


                //alrtForm.label1.Text = "Creating annotations for Navaids";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (navaidsList == null) navaidsList = new List<PDMObject>();

                foreach (var rwy in selARP.RunwayList)
                {
                    foreach (var rdn in rwy.RunwayDirectionList)
                    {
                        if (rdn.Related_NavaidSystem != null)
                            navaidsList.AddRange(rdn.Related_NavaidSystem);
                    }
                }

                if (navaidsList != null)
                {
                    List<string> NavaidList = new List<string>();
                    AerodromeChartUtils.CreateNavaids_ChartElements(navaidsList, FocusMap, ref NavaidList, Anno_NavaidsCartography_featClass, vertUom.ToString());
                }

                #region Airport, Extent, WorkAreaList,TouchDownLiftOffList, UnitList, CommunicationChanels, NonMovementAreaList

                //alrtForm.label1.Text = "Creating annotations for Airport group";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                List<AirportHeliport> ADHPList = new List<AirportHeliport>();
                ADHPList.Add(selARP);
                TerminalChartsUtil.CreateAirportAnno(ADHPList, Anno_AirportCartography_featClass, FocusMap, (frm._mapAngle - 90) * -1);

                if (selARP.AirportHotSpotList != null && selARP.AirportHotSpotList.Count > 0)
                    AerodromeChartUtils.CreateAirportHotSpotAnno(selARP.AirportHotSpotList, Anno_AirportHotSpotCartography_featClass, FocusMap, pSpatialReference);

                if (selARP.Extent != null)
                    AerodromeChartUtils.Save_ChartGeo(Anno_AirportHeliportExtentCartography_featClass, FocusMap, selARP, "AirportHeliportExtent", pSpatialReference);

                if (selARP.WorkAreaList != null && selARP.WorkAreaList.Count > 0)
                {
                    foreach (var item in selARP.WorkAreaList)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_WorkAreaCartography_featClass, FocusMap, item, "WorkArea", pSpatialReference);

                    }

                }

                if (selARP.UnitList != null && selARP.UnitList.Count > 0)
                {
                    foreach (var item in selARP.UnitList)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_UnitCartography_featClass, FocusMap, item, "Unit", pSpatialReference);

                    }

                }

                if (selARP.CommunicationChanels != null && selARP.CommunicationChanels.Count > 0)
                {
                    foreach (var item in selARP.CommunicationChanels)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_UnitCartography_featClass, FocusMap, item, "RadioCommunicationChanel", pSpatialReference);

                    }

                }

                if (selARP.NonMovementAreaList != null && selARP.NonMovementAreaList.Count > 0)
                {
                    foreach (var item in selARP.NonMovementAreaList)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_NonMovementAreaCartography_featClass, FocusMap, item, "NonMovementArea", pSpatialReference);

                    }

                }


                //alrtForm.label1.Text = "Creating annotations for Roads";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (selARP.RoadList != null && selARP.RoadList.Count > 0)
                {
                    foreach (var arprt in selARP.RoadList)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_RoadCartography_featClass, FocusMap, arprt, "Road", pSpatialReference);



                    }

                }

                //alrtForm.label1.Text = "Creating annotations for Touch DownL Lift Off";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (selARP.TouchDownLiftOffList != null)
                {
                    foreach (var tdLoff in selARP.TouchDownLiftOffList)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_TouchDownLiftOffCartography_featClass, FocusMap, tdLoff, "TouchDownLiftOff", pSpatialReference);
                        if (tdLoff.TouchDownSafeArea != null)
                            AerodromeChartUtils.Save_ChartGeo(Anno_TouchDownLiftOffSafeAreaCartography_featClass, FocusMap, tdLoff.TouchDownSafeArea, "TouchDownLiftOffSafeArea", pSpatialReference);
                        if (tdLoff.LightSystem != null)
                            AerodromeChartUtils.Save_ChartGeo(Anno_LightElementCartography_featClass, FocusMap, tdLoff.LightSystem, "LightElement", pSpatialReference);
                        if (tdLoff.AimingPoint != null)
                            AerodromeChartUtils.Save_ChartGeo(Anno_TouchDownLiftOffAimingPointCartography_featClass, FocusMap, tdLoff, "TouchDownLiftOffAimingPoint", pSpatialReference);

                        //tdLoff.TouchDownLiftOffMarkingList

                        if (tdLoff.GuidanceLineList != null)
                            gdnsLnList.AddRange(tdLoff.GuidanceLineList);

                        if (tdLoff.TouchDownLiftOffMarkingList != null && tdLoff.TouchDownLiftOffMarkingList.Count > 0)
                        {
                            foreach (var item in tdLoff.TouchDownLiftOffMarkingList)
                            {
                                mrkngList.AddRange(item.MarkingElementList);

                            }
                        }


                    }
                }

                #endregion


                //alrtForm.label1.Text = "Creating annotations for Guidance Lines";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                AerodromeChartUtils.CreateGuidanceLineCartography(gdnsLnList, FocusMap, pSpatialReference, Anno_GuidanceLineCartography_featClass, distUom, ref mrkngList);

                //alrtForm.label1.Text = "Creating annotations for Deicing Areas";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (dcngArea != null && dcngArea.Count > 0)
                {
                    foreach (var item in dcngArea)
                    {
                        AerodromeChartUtils.Save_ChartGeo(Anno_DeicingAreaCartography_featClass, FocusMap, item, "DeicingArea", pSpatialReference);
                        if (item.DeicingAreaMarkingList != null && item.DeicingAreaMarkingList.Count > 0)
                            foreach (var mkreL in item.DeicingAreaMarkingList)
                            {
                                mrkngList.AddRange(mkreL.MarkingElementList);
                            }
                    }
                }


                //alrtForm.label1.Text = "Creating annotations for Markers";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (mrkngList != null && mrkngList.Count > 0)
                {
                    foreach (var item in mrkngList)
                    {
                        IGeometry gm = AerodromeChartUtils.Save_ChartGeo(Anno_MarkingPointCartography_featClass, FocusMap, item, "Marking_Point", pSpatialReference);
                        if (gm == null) gm = AerodromeChartUtils.Save_ChartGeo(Anno_MarkingCurveCartography_featClass, FocusMap, item, "Marking_Curve", pSpatialReference);
                        if (gm == null) gm = AerodromeChartUtils.Save_ChartGeo(Anno_MarkingSurfaceCartography_featClass, FocusMap, item, "Marking_Surface", pSpatialReference);

                    }
                }

                #region Update dinamic text

                //string num = (_AiracCircle % 100).ToString();
                ////int y = 2000 + _AiracCircle / 100;
                //int y = _AiracCircle / 100;

                //while (num.Length < 2) num = "0" + num;
                //alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Airac", num + @"/" + y.ToString());

                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                alrtForm.label1.Text = "Update dinamic text";
                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, null, selARP, null, vertUom.ToString(), distUom.ToString(), 0, _arp_chanels);

                #region North Arrow

                double rotationValue = Double.Parse(FocusMap.Description);
                TerminalChartsUtil.UpdateNorthArrowText(selARP, rotationValue, SigmaHookHelper);

                #endregion

                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_ADHPName", selARP.Name, keepOld: false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_arpICAO", selARP.Designator, keepOld: false);



                #endregion
            }

            #endregion


            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

  
            #region Aerodrome tabulation document


            if (selARP != null)
            {
                if (MessageBox.Show("Do you want to create Aerodrome tabulation document?", "SIGMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Type officeType = Type.GetTypeFromProgID("Excel.Application");
                    if (officeType == null)
                    {
                        // Excel is not installed.
                        // Show message or alert that Excel is not installed.
                        System.Windows.Forms.MessageBox.Show("Microsoft Excel is not installed! It is impossible to create  Aerodrome tabulation document.", "SIGMA", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);

                        return;
                    }

                    ChartsHelperClass.CreateAerodromeTable(destPath2 + @"\Aerodrome.xls", selARP);

                }
            }
            #endregion


            alrtForm.label1.Text = "Finalisation";
            alrtForm.progressBar1.Value = alrtForm.progressBar1.Maximum; alrtForm.BringToFront();


            ChartsHelperClass.ChartsFinalisation(this.SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, projectName, _mapBookmarks: extentBookmark);

 

            ChartElementsManipulator.CreateSigmaLog(destPath2);
            EsriWorkEnvironment.EsriUtils.CreateJPEG_FromActiveView(this.SigmaHookHelper.ActiveView, destPath2 + @"\ContentImage.jpg");

            ((IGraphicsContainerSelect)SigmaHookHelper.ActiveView.GraphicsContainer).UnselectAllElements();
            
            AerodromeChartUtils.RotateMap(FocusMap, frm._mapAngle - 90);
            SigmaHookHelper.ActiveView.FocusMap.MapScale = frm._mapScale;

            ChartElementsManipulator.RefreshChart((IMxDocument)this.SigmaApplication.Document);


            alrtForm.Close();
            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Chart is created successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
            msgFrm.TopMost = true;
            msgFrm.checkBox1.Visible = true;
            msgFrm.ShowDialog();
            if (msgFrm.checkBox1.Checked) Process.Start(destPath2 + @"\SIGMA_ResultsInfo.txt");

        }


        public override IWorkspaceEdit InitEnvironment_Workspace(string pathToTemplateFileXML, IMap ActiveMap)
        {
            SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);

            SigmaDataCash.ChartElementList = new List<object>();
            SigmaDataCash.AnnotationFeatureClassList = null;

            ILayer _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(FocusMap, "AirportCartography");

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count == 0)
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);

            Application.DoEvents();


            try
            {
                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                    Anno_AirportCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AircraftStandCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AircraftStandCartography"))
                    Anno_AircraftStandCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AircraftStandCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AircraftStandCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AircraftStandCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AircraftStandExtentCartography"))
                    Anno_AircraftStandExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AircraftStandExtentCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AircraftStandExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AircraftStandExtentCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportHotSpotCartography"))
                    Anno_AirportHotSpotCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHotSpotCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AirportHotSpotCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHotSpotCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ApronElementCartography"))
                    Anno_ApronElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ApronElementCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_ApronElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ApronElementCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DeicingAreaCartography"))
                    Anno_DeicingAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DeicingAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DeicingAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DeicingAreaCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GuidanceLineCartography"))
                    Anno_GuidanceLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GuidanceLineCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_GuidanceLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GuidanceLineCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NonMovementAreaCartography"))
                    Anno_NonMovementAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NonMovementAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_NonMovementAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NonMovementAreaCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RadioCommunicationChanelCartography"))
                    Anno_RadioCommunicationChanelCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RadioCommunicationChanelCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RadioCommunicationChanelCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RadioCommunicationChanelCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RoadCartography"))
                    Anno_RoadCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RoadCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RoadCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RoadCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography"))
                    Anno_NavaidsCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_NavaidsCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VertStructureSurfaceCartography"))
                    Anno_VertStructureSurfaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VertStructureSurfaceCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_VertStructureSurfaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VertStructureSurfaceCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VerticalStructurePointCartography"))
                    Anno_VerticalStructurePointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_VerticalStructurePointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VertStructureCurveCartography"))
                    Anno_VertStructureCurveCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VertStructureCurveCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_VertStructureCurveCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VertStructureCurveCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayVisualRangeCartography"))
                    Anno_RunwayVisualRangeCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayVisualRangeCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RunwayVisualRangeCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayVisualRangeCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("TaxiHoldingPositionCartography"))
                    Anno_TaxiHoldingPositionCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TaxiHoldingPositionCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_TaxiHoldingPositionCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TaxiHoldingPositionCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("TouchDownLiftOffCartography"))
                    Anno_TouchDownLiftOffCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_TouchDownLiftOffCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("TouchDownLiftOffAimingPointCartography"))
                    Anno_TouchDownLiftOffAimingPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffAimingPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_TouchDownLiftOffAimingPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffAimingPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("TouchDownLiftOffSafeAreaCartography"))
                    Anno_TouchDownLiftOffSafeAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffSafeAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_TouchDownLiftOffSafeAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TouchDownLiftOffSafeAreaCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("UnitCartography"))
                    Anno_UnitCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["UnitCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_UnitCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["UnitCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("WorkAreaCartography"))
                    Anno_WorkAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["WorkAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_WorkAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["WorkAreaCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportHeliportExtentCartography"))
                    Anno_AirportHeliportExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHeliportExtentCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AirportHeliportExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHeliportExtentCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("LightElementCartography"))
                    Anno_LightElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["LightElementCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_LightElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["LightElementCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayDirectionCartography"))
                    Anno_RunwayDirectionCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayDirectionCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RunwayDirectionCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayDirectionCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayDirectionCenterLinePointCartography"))
                    Anno_RunwayDirectionCenterLinePointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayDirectionCenterLinePointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RunwayDirectionCenterLinePointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayDirectionCenterLinePointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayElementCartography"))
                    Anno_RunwayElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayElementCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RunwayElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayElementCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayProtectAreaCartography"))
                    Anno_RunwayProtectAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayProtectAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RunwayProtectAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayProtectAreaCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("TaxiwayElementCartography"))
                    Anno_TaxiwayElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TaxiwayElementCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_TaxiwayElementCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["TaxiwayElementCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("CheckpointCartography"))
                    Anno_CheckpointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["CheckpointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_CheckpointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["CheckpointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("MarkingCurveCartography"))
                    Anno_MarkingCurveCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingCurveCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_MarkingCurveCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingCurveCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("MarkingPointCartography"))
                    Anno_MarkingPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_MarkingPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("MarkingSurfaceCartography"))
                    Anno_MarkingSurfaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingSurfaceCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_MarkingSurfaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["MarkingSurfaceCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportHeliportExtentCartography"))
                    Anno_AirportHeliportExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHeliportExtentCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_AirportHeliportExtentCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportHeliportExtentCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                }


                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceCartography"))
                    Anno_BirdAirspaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_BirdAirspaceCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RadioFrequencyAreaCartography"))
                    Anno_RadioFrequencyAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RadioFrequencyAreaCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AerodromeElectronicChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_RadioFrequencyAreaCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RadioFrequencyAreaCartography"];
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            return workspaceEdit;

        }
    }
}
