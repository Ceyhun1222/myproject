using ANCOR.MapCore;
using ANCOR.MapElements;
using AranSupport;
using ARENA;
using ArenaStatic;
using ChartDataTabulation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using OIS;
using PDM;
using SigmaChart.CmdsMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChartCodingTable;
using System.Globalization;
using ESRI.ArcGIS.ArcMapUI;

namespace SigmaChart
{
    public class MinimumAltitudeChart : AbstaractSigmaChart
    {

        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference = null;


        private IFeatureClass Anno_LegGeo_featClass = null;
        private IFeatureClass Anno_DesignatedGeo_featClass = null;
        private IFeatureClass Anno_NavaidGeo_featClass = null;
        private IFeatureClass Anno_ObstacleGeo_featClass = null;
        private IFeatureClass AnnoAirspaceGeo_featClass = null;
        private IFeatureClass AnnoRWYGeo_featClass = null;
        private IFeatureClass AnnoFacilitymakeUpGeo_featClass = null;
        private IFeatureClass Anno_HoldingGeo_featClass = null;
        private IFeatureClass Anno_GeoBorder_Geo_featClass = null;
        private IFeatureClass Anno_Airport_Geo_featClass = null;
        private IFeatureClass Anno_DecorPointCartography_featClass = null;
        private IFeatureClass Anno_DecorLineCartography_featClass = null;
        private IFeatureClass Anno_DecorPolygonCartography_featClass = null;



        List<SegmentPoint> DpnList = null;
        List<string> NavaidList = null;
        AirportHeliport selARP = null;
        UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        List<string> _arp_chanels = null;
        NavaidSystem selNvd = null;

        public MinimumAltitudeChart()
        {
        }


        public override void CreateChart()
        {
            try
            {

                object Missing = Type.Missing;


                var acDate = DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
                int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);

                AreaMinimaWizardForm frm = new AreaMinimaWizardForm(_AiracCircle);

                if (frm.ShowDialog() == DialogResult.Cancel) return;

                _AiracCircle = frm._AiracCircle;

                AlertForm alrtForm = new AlertForm();

                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();


                alrtForm.progressBar1.Visible = true;
                alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                alrtForm.progressBar1.Maximum = 20;
                alrtForm.progressBar1.Value = 0;


                alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                alrtForm.label1.Text = "Start process";
                alrtForm.label1.Visible = true;
                alrtForm.BringToFront();

                var projectName = frm._ProjectName.EndsWith(".mxd") ? frm._ProjectName : frm._ProjectName + ".mxd";
                if (Directory.Exists(frm._FolderName + @"\" + frm._ProjectName)) Directory.Delete(frm._FolderName + @"\" + frm._ProjectName, true);

                var destPath2 = System.IO.Directory.CreateDirectory(frm._FolderName + @"\" + frm._ProjectName).FullName;


                var tmpName = frm._TemplatetName;
                selARP = frm.selectedADHP;
                selNvd = frm.selectedVORDME;

                vertUom = frm.vertUom;
                distUom = frm.distUom;
                _arp_chanels = frm._selectedChanels;
                if (_arp_chanels == null) { _arp_chanels = TerminalChartsUtil.getChanelsList(selARP.CommunicationChanels); }
                int arspBufWidth = frm.arspBufWidth;
                bool createArspsSgnFlag = frm.arspSign;
                int circleStep = frm.circleStep;
                double maxradius = frm.maxRadius;
                IAOIBookmark extentBookmark = frm._bookmark;


                ILayer _Layer = EsriUtils.getLayerByName(this.SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;

                alrtForm.label1.Text = "Preparation";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                this.SigmaHookHelper.ActiveView.FocusMap.MapScale = frm._mapScale;

                Application.DoEvents();


                alrtForm.progressBar1.Value++;



                //alrtForm.label1.Text = "Validation";
                //alrtForm.progressBar1.Value++;
                //alrtForm.BringToFront();

                #region Validator


                //ChartValidator.ProcedureValidator vldr = new ChartValidator.ProcedureValidator(this.SigmaApplication, ChartValidator.ChartType.SID);
                ////vldr.AddDestroyEventHandler(ClosedSidValidatorReportWindow);

                //string er;
                //byte result = vldr.Check(DataCash.ProjectEnvironment.Data.PdmObjectList, out er);
                ////byte result = vldr.Check(ListToCheck, out er);

                //if (result == 2)
                //{
                //    MessageBox.Show(er);
                //}
                //else if (result == 4) return;



                #endregion


                alrtForm.label1.Text = "Data quering";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                #region Data Query

                var selectedRec = EsriUtils.ToGeo((this.SigmaHookHelper.ActiveView.FocusMap as IActiveView).Extent, this.SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);


                List<PDMObject> arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Airspace) && 
                                     ((((Airspace)element).CodeType != AirspaceType.SECTOR) && (((Airspace)element).CodeType != AirspaceType.SECTOR_C)) select element).ToList();

                List<PDMObject> navaidsList = DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.NavaidSystem);

                List<PDMObject> wypntsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.WayPoint).ToList();
                List<PDMObject> GeoBorderList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.GeoBorder).ToList();
                //List<PDMObject> ADHPList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.AirportHeliport).ToList();
                List<AirportHeliport> ADHPList = DataCash.GetAirportlist();

                List<PDMObject> obstacleList = new List<PDMObject>();

                if (obstacleList.Count <= 0)
                    obstacleList = DataCash.FilterObstaclesWithinPolygon(selectedRec, frm.VS_Min_Elev, RadiusInMeters: frm.VS_Radius);


                #region Store using data to pdm file

                List<PDMObject> PdmElementsList = new List<PDMObject>();

                if (arspc_featureList != null && arspc_featureList.Count > 0) PdmElementsList.AddRange(arspc_featureList.GetRange(0, arspc_featureList.Count));
                if (wypntsList != null && wypntsList.Count > 0) PdmElementsList.AddRange(wypntsList.GetRange(0, wypntsList.Count));
                if (navaidsList != null && navaidsList.Count > 0) PdmElementsList.AddRange(navaidsList.GetRange(0, navaidsList.Count));
                if (obstacleList != null && obstacleList.Count > 0) PdmElementsList.AddRange(obstacleList.GetRange(0, obstacleList.Count));
                if (GeoBorderList != null && GeoBorderList.Count > 0) PdmElementsList.AddRange(GeoBorderList.GetRange(0, GeoBorderList.Count));
                if (ADHPList != null && ADHPList.Count > 0) PdmElementsList.AddRange(ADHPList.GetRange(0, ADHPList.Count));


                PdmElementsList.Add(frm.selectedADHP);

                ChartsHelperClass.SaveSourcePDMFiles(destPath2, PdmElementsList);
                PdmElementsList = null;

                #endregion

                //this.EffectiveDate = DataCash.GetEfectiveDate();
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
                this.EffectiveDate = start;
                this.ADHP = selARP.Designator != null ? selARP.Designator : "";
                this.airacCircle = _AiracCircle.ToString();
                this.organization = selARP.OrganisationAuthority != null ? selARP.OrganisationAuthority : "";


                DpnList = frm._selectedWYPNT;

                ///////////////////////

                #endregion


                double AnnoScale = this.SigmaHookHelper.ActiveView.FocusMap.MapScale;

                alrtForm.label1.Text = "Initialization";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.MinimumAltitudeChart, projectName, destPath2, tmpName, this.SigmaApplication);
                Application.DoEvents();
                if (FocusMap == null) return;

                alrtForm.label1.Text = "Maps projection settings";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                #region ChangeProjectionAndMeredian

                if (selARP.Geo == null) selARP.RebuildGeo();
                double newX = ((IPoint)selARP.Geo).X;
                EsriUtils.ChangeProjectionAndMeredian(newX, FocusMap);

                #endregion


                FocusMap.MapScale = AnnoScale;

                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\MinimumAltitudeChart\", "minalt.sce");
                SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.MinimumAltitudeChart;

                if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

                alrtForm.label1.Text = "Work space initialization";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

                //////////////////////////////////////////////
                //try
                //{
                //    alrtForm.label1.Text = "Validation";
                //    alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                //    SigmaTerminal_EnrouteChartExamination sidExam = new SigmaTerminal_EnrouteChartExamination { _projectName = projectName, ExaminationData = new Dictionary<string, List<PDMObject>>(), SigmaChartTypes = (int)SigmaChartTypes.SIDChart_Type };
                //    sidExam.FillExaminationData(arspc_featureList, navaidsList, ADHPList, GeoBorderList, selectedLegs);
                //    sidExam.ChartExamination();
                //    sidExam.ShowExamResults();
                //}
                //catch { }
                ////////////////////////////////////////////

                
                NavaidList = new List<string>();

                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.BringToFront();
                Application.DoEvents();


                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                #region save chart info

                DateTime tmpStart = DateTime.MinValue, tmpEnd = DateTime.MinValue;
                AiracUtil.AiracUtil.GetAiracCirclePeriod(int.Parse(this.airacCircle), ref tmpStart, ref tmpEnd);
                EsriWorkEnvironment.chartInfo ci = new chartInfo
                {
                    ADHP = this.ADHP,
                    airacCircle = this.airacCircle,
                    efectiveDate = tmpStart,
                    organization = this.organization,
                    chartName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.Combine(destPath2, projectName)),
                    uomDist = distUom.ToString(),
                    uomVert = vertUom.ToString(),
                    widthBufer = arspBufWidth,
                    flag = createArspsSgnFlag,
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
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                #region creating annotations

                alrtForm.progressBar1.Value++;


                TerminalChartsUtil.SaveVerticalStructureGeo(obstacleList, Anno_ObstacleGeo_featClass, 0, vertUom.ToString(), distUom.ToString(), FocusMap.MapScale);


                alrtForm.progressBar1.Value++;


                if (arspc_featureList != null && arspc_featureList.Count > 0)
                    TerminalChartsUtil.CreateAirspace_ChartElements(arspc_featureList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale, vertUom.ToString(), distUom.ToString(), arspBufWidth, createArspsSgnFlag);


                if (frm._selectedArspVol!=null && frm._selectedArspVol.Count>0)
                {
                    IGeometry sectorCenter = null;
                    if (selNvd != null)
                    {
                        if (selNvd.Components[0].Geo == null) selNvd.Components[0].RebuildGeo();
                        sectorCenter = selNvd.Components[0].Geo;

                    }
                    else
                    {
                        if (selARP.Geo == null)  selARP.RebuildGeo();
                        sectorCenter = selARP.Geo;

                    }

                    if (sectorCenter!=null)
                    TerminalChartsUtil.CreateSectorAirspace_ChartElements(frm._selectedArspVol, sectorCenter, FocusMap, SigmaHookHelper, pSpatialReference, 
                        AnnoAirspaceGeo_featClass,Anno_DecorPolygonCartography_featClass, Anno_DecorPointCartography_featClass, distUom, circleStep, maxradius);
                }


                alrtForm.progressBar1.Value++;
            
                alrtForm.progressBar1.Value++;


                TerminalChartsUtil.CreateNavaids_ChartElements(navaidsList, FocusMap.MapScale, ref NavaidList, Anno_NavaidGeo_featClass, vertUom.ToString());

                TerminalChartsUtil.CreateGeoBorderAnno(GeoBorderList, FocusMap);

                TerminalChartsUtil.CreateAirportAnno(ADHPList, selARP.ID, Anno_Airport_Geo_featClass, AnnoRWYGeo_featClass, FocusMap, pSpatialReference, frm.allRwyFlag, selArpDesignator: this.ADHP);

                if (DpnList!=null)
                {
                    foreach (var item in DpnList)
                    {
                        ChartElement_TextArrow chrtEl_DesigPoint = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "DesignatedPoint_Simple");
                        IElement el = ChartsHelperClass.CreateWayPointAnno(item,chrtEl_DesigPoint);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, item.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, item, item.Geo,false);
                    }
                }


                #endregion

                alrtForm.label1.Text = "Updating grahics elements";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                #region Update Graphics Elements

                #region MSA

                alrtForm.progressBar1.Value++;

                #endregion

                #region North Arrow

                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.CreateNorthArrow(SigmaHookHelper, selARP);

                #endregion

                #region Update dinamic text

                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, null, selARP, null, vertUom.ToString(), distUom.ToString(), 0, _arp_chanels);

                //string num = (_AiracCircle % 100).ToString();
                ////int y = 2000 + _AiracCircle / 100;
                //int y = _AiracCircle / 100;

                //while (num.Length < 2) num = "0" + num;

                //alrtForm.progressBar1.Value++;
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Airac", " " + num + @"/" + y.ToString(), true);

                //alrtForm.progressBar1.Value++;
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_EffectiveDate", (this.EffectiveDate.Day.ToString() + " " + this.EffectiveDate.ToString("MMM", CultureInfo.CreateSpecificCulture("en")) + " " + this.EffectiveDate.Year.ToString()).ToUpper(), false);


                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Authority", selARP.OrganisationAuthority, false);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_ARP_HEADER", selARP.Designator);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportICAOCode", selARP.Designator, false);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportIATA", selARP.DesignatorIATA);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_ADHPNAME", selARP.Name, false);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_dateHeader", (this.EffectiveDate.Day.ToString() + " " + this.EffectiveDate.ToString("MMM", CultureInfo.CreateSpecificCulture("en")) + " " + this.EffectiveDate.Year.ToString()).ToUpper(), false);

                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_chanels", _arp_chanels);

                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_uomDist", distUom.ToString(), true);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_uomVert", vertUom.ToString(), true);

                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_country", TerminalChartsUtil.GetCountryName(selARP.Designator), false);
                //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_TransitionAlt", selARP.TransitionAltitude.Value.ToString(), false);

                //if (selARP.Elev.HasValue)
                //{
                //    double val = ArenaStaticProc.UomTransformation(selARP.Elev_UOM.ToString(), vertUom.ToString(), selARP.Elev.Value, 0);
                //    TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportElev", val.ToString(), true);

                //}

                #endregion



                #endregion


               
                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                alrtForm.label1.Text = "Finalisation";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                _Layer = EsriUtils.getLayerByName(this.SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");

                ChartsHelperClass.ChartsFinalisation(this.SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, projectName, "ProcedureLegs", extentBookmark);




                ChartElementsManipulator.CreateSigmaLog(destPath2);
                EsriWorkEnvironment.EsriUtils.CreateJPEG_FromActiveView(this.SigmaHookHelper.ActiveView, destPath2 + @"\ContentImage.jpg");


                ((IGraphicsContainerSelect)SigmaHookHelper.ActiveView.GraphicsContainer).UnselectAllElements();
                ChartElementsManipulator.RefreshChart((IMxDocument)this.SigmaApplication.Document);

                alrtForm.Close();
                ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Chart is created successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
                msgFrm.TopMost = true;
                msgFrm.checkBox1.Visible = true;
                msgFrm.ShowDialog();
                if (msgFrm.checkBox1.Checked) Process.Start(destPath2 + @"\SIGMA_ResultsInfo.txt");

    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }




        }

        public override ESRI.ArcGIS.Geodatabase.IWorkspaceEdit InitEnvironment_Workspace(string pathToTemplateFileXML, ESRI.ArcGIS.Carto.IMap ActiveMap)
        {
            SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);

            SigmaDataCash.ChartElementList = new List<object>();
            SigmaDataCash.AnnotationFeatureClassList = null;

            ILayer _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportCartography");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count == 0)
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);

            Application.DoEvents();


            try
            {
                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ProcedureLegsCartography")) Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC")) AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography")) Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VerticalStructurePointCartography"))
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayCartography")) AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("FacilityMakeUpCartography")) AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography")) Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
                }


                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GeoBorderCartography"))
                    Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.MinimumAltitudeChart, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
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
