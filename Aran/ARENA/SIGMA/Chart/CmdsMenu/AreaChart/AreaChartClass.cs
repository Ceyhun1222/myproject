using ANCOR.MapCore;
using ANCOR.MapElements;
using AranSupport;
using ARENA;
using ArenaStatic;
using ChartCompare;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public class AreaChartClass : AbstaractSigmaChart
    {

        private IFeatureClass Anno_RouteGeo_featClass = null;
        private IFeatureClass Anno_DesignatedGeo_featClass = null;
        private IFeatureClass Anno_NavaidGeo_featClass = null;
        private IFeatureClass AnnoAirspaceGeo_featClass = null;
        private IFeatureClass Anno_HoldingGeo_featClass = null;
        private IFeatureClass Anno_GeoBorder_Geo_featClass = null;
        private IFeatureClass Anno_Airport_Geo_featClass = null;

        private IFeatureClass Anno_LegGeo_featClass = null;
        private IFeatureClass Anno_ObstacleGeo_featClass = null;
        private IFeatureClass AnnoRWYGeo_featClass = null;
        private IFeatureClass AnnoFacilitymakeUpGeo_featClass = null;
        private IFeatureClass Anno_DecorPointCartography_featClass = null;
        private IFeatureClass Anno_DecorLineCartography_featClass = null;
        private IFeatureClass Anno_DecorPolygonCartography_featClass = null;
        private IFeatureClass Anno_HoldingpathGeo_featClass = null;

        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference =null;
        List<string> DpnList = null;
        List<string> NavaidList = null;

        public AreaChartClass()
        {

        }


        #region ChartCreate 

        public override void CreateChart()
        {
            object Missing = Type.Missing;

            try
            {
                var acDate = DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
                int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);
                //int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(DataCash.ProjectEnvironment.Data.PdmObjectList[0].ActualDate);
                AreaChartWizardForm frm = new AreaChartWizardForm(_AiracCircle);


                if (frm.ShowDialog() == DialogResult.Cancel) return;

                _AiracCircle = frm._AiracCircle;
                IAOIBookmark extentBookmark = frm._bookmark;
                UOM_DIST_VERT vertUom = frm.vertUom;
                UOM_DIST_HORZ distUom = frm.distUom;
                List<string> _enrt_chanels = null;
                int arspBufWidth = frm.arspBufWidth;

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

                var projectName = frm._ProjectName.EndsWith(".mxd") ? frm._ProjectName : frm._ProjectName + ".mxd";
                if (Directory.Exists(frm._FolderName + @"\" + frm._ProjectName)) Directory.Delete(frm._FolderName + @"\" + frm._ProjectName, true);

                var destPath2 = System.IO.Directory.CreateDirectory(frm._FolderName + @"\" + frm._ProjectName).FullName;
                var tmpName = frm._TemplatetName;
                var rnavFlag = frm._RNAVflag;
                bool createArspsSgnFlag = frm.arspSign;
                List<Airspace> selectedTmaLst = frm.tmaLst;


                CODE_ROUTE_SEGMENT_CODE_LVL selectedLevel = frm.Level < 3 ? (CODE_ROUTE_SEGMENT_CODE_LVL)frm.Level + 1 : CODE_ROUTE_SEGMENT_CODE_LVL.OTHER;

                _enrt_chanels = frm._selectedChanels;
                if (_enrt_chanels == null)
                {
                    List<RadioCommunicationChanel> cnlList = null;
                    switch (selectedLevel)
                    {
                        case (CODE_ROUTE_SEGMENT_CODE_LVL.LOWER):
                            cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.LOWER);
                            break;
                        case (CODE_ROUTE_SEGMENT_CODE_LVL.BOTH):
                            cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.BOTH);
                            break;
                        case (CODE_ROUTE_SEGMENT_CODE_LVL.UPPER):
                            cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.UPPER);
                            break;
                        case (CODE_ROUTE_SEGMENT_CODE_LVL.OTHER):
                            cnlList = DataCash.GetEnroutesChanels(CODE_ROUTE_SEGMENT_CODE_LVL.OTHER);

                            break;
                        default:
                            break;
                    }

                    List<RadioCommunicationChanel> arspsChanels = DataCash.GetAirspaceChanels();
                    if (cnlList == null) cnlList = new List<RadioCommunicationChanel>();
                    if (arspsChanels != null) cnlList.AddRange(arspsChanels);

                    _enrt_chanels = TerminalChartsUtil.getChanelsList(cnlList);
                }

                ILayer _Layer = EsriUtils.getLayerByName(SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;

                alrtForm.label1.Text = "Preparation";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                alrtForm.label1.Text = "Data quering";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region data Query


                //var selectedRec = EsriUtils.ToGeo((this.SigmaHookHelper.ActiveView.FocusMap as IActiveView).Extent, this.SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);
                var selectedRec = frm._ext;

                List<PDMObject> ENRT_featureList = DataCash.GetEnroutes(true, selectedLevel);

                List<PDMObject> hlng_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                    where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.HoldingPattern)
                                                        && (((PDM.HoldingPattern)element).Type == PDM.CodeHoldingUsage.ENR)
                                                    select element).ToList();

                // Получить ВСЕ Airspace
                List<PDMObject> arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Airspace) select element).ToList();
                //Убрать ВСЕ TMA
                arspc_featureList.RemoveAll( x => x.PDM_Type == PDM_ENUM.Airspace && ((Airspace)x).CodeType.ToString().Contains("TMA"));
                //Добавить только те TMA, которые выбраны
                arspc_featureList.AddRange(selectedTmaLst);

                List<PDMObject> wypntsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.WayPoint).ToList();
                List<PDMObject> nvdsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.NavaidSystem).ToList();
                List<PDMObject> GeoBorderList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.GeoBorder).ToList();
                //List<PDMObject> ADHPList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.AirportHeliport).ToList();
                List<AirportHeliport> ADHPList = DataCash.GetAirportlist();

                List<PDMObject> obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => (pdm is PDM.VerticalStructure) && 
                     ((PDM.VerticalStructure)pdm).ObstacleAreaType!=null && ((PDM.VerticalStructure)pdm).ObstacleAreaType.Contains(CodeObstacleAreaType.AREA1)).ToList();
                if (obstacleList == null || obstacleList.Count == 0)
                {
                    //obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure).ToList();
                    obstacleList = DataCash.FilterObstaclesWithinPolygon(selectedRec, frm.VS_Min_Elev, RadiusInMeters: frm.VS_Radius);
                }

                List<PDMObject> selectedProc = frm.selectedProcedures;

                //this.EffectiveDate = DataCash.GetEfectiveDate();
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
                this.EffectiveDate = start;
                this.ADHP = ((AirportHeliport)ADHPList[0]).Designator != null ? ((AirportHeliport)ADHPList[0]).Designator : "";
                this.organization = ((AirportHeliport)ADHPList[0]).OrganisationAuthority != null ? ((AirportHeliport)ADHPList[0]).OrganisationAuthority : "";
                this.airacCircle = _AiracCircle.ToString();

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<PDMObject> PdmElementsList = new List<PDMObject>();

                List<PDMObject> ENRT_featureList_ALL = DataCash.GetEnroutes(selectedLevel);
                if (ENRT_featureList_ALL != null && ENRT_featureList_ALL.Count > 0) PdmElementsList.AddRange(ENRT_featureList_ALL.GetRange(0, ENRT_featureList.Count));
                if (hlng_featureList != null && hlng_featureList.Count > 0) PdmElementsList.AddRange(hlng_featureList.GetRange(0, hlng_featureList.Count));
                if (arspc_featureList != null && arspc_featureList.Count > 0) PdmElementsList.AddRange(arspc_featureList.GetRange(0, arspc_featureList.Count));
                if (wypntsList != null && wypntsList.Count > 0) PdmElementsList.AddRange(wypntsList.GetRange(0, wypntsList.Count));
                if (nvdsList != null && nvdsList.Count > 0) PdmElementsList.AddRange(nvdsList.GetRange(0, nvdsList.Count));
                if (GeoBorderList != null && GeoBorderList.Count > 0) PdmElementsList.AddRange(GeoBorderList.GetRange(0, GeoBorderList.Count));
                if (obstacleList != null && obstacleList.Count > 0) PdmElementsList.AddRange(obstacleList.GetRange(0, obstacleList.Count));
                if (selectedProc != null && selectedProc.Count > 0) PdmElementsList.AddRange(selectedProc.GetRange(0, selectedProc.Count));


                ChartsHelperClass.SaveSourcePDMFiles(destPath2, PdmElementsList);
                PdmElementsList = null;


                ////////////////////////

                //фильтрация легов

                Dictionary<string, ProcedureLeg> dicLeg = TerminalChartsUtil.LegFilter(selectedProc);

                List<ProcedureLeg> selectedLegs = new List<ProcedureLeg>();
                selectedLegs.AddRange(dicLeg.Values);

                double RNPFlag = -1;

                foreach (var item in selectedLegs)
                {
                    if (item.RequiredNavigationPerformance.HasValue)
                        RNPFlag = item.RequiredNavigationPerformance.Value;

                }


                
                #region фильтрация RouteSegment

                List<SegmentPoint> stPntsList = GetProceduresPointsList(selectedProc.Where(pdm => pdm is Procedure && ((Procedure)pdm).ProcedureType == PROC_TYPE_code.STAR ).ToList());
                List<SegmentPoint> endPntsList = GetProceduresPointsList(selectedProc.Where(pdm => pdm is Procedure && ((Procedure)pdm).ProcedureType == PROC_TYPE_code.SID ).ToList());

                foreach (Enroute enrt in ENRT_featureList)
                {
                    if (enrt.Routes == null) continue;
                    foreach (var rtSeg in enrt.Routes)
                    {
                        if (rtSeg.EndPoint == null) continue;
                        if (rtSeg.StartPoint == null) continue;

                        if (stPntsList !=null)
                        {
                            var l = stPntsList.Select(pdm => pdm.PointChoiceID.CompareTo(rtSeg.EndPoint.PointChoiceID) == 0).ToList();
                            if (l.Where(b => b == true).ToList().Count > 0 && rtSeg.StartPoint.PointChoice == PointChoice.DesignatedPoint)
                            {
                                //enrt.Lat = "skip";
                                enrt.Lat = "inArea";

                                //RouteSegment rs = new RouteSegment();
                                //rs = rtSeg;
                                //rs.CodeType = "inArea";
                                //enrt.Routes.Clear();
                                //enrt.Routes.Add(rs);
                                //break;
                            }
                        }


                        if (endPntsList != null)
                        {
                            var l = endPntsList.Select(pdm => pdm.PointChoiceID.CompareTo(rtSeg.StartPoint.PointChoiceID) == 0).ToList();
                            if (l.Where(b => b == true).ToList().Count > 0 && rtSeg.StartPoint.PointChoice == PointChoice.DesignatedPoint)
                            {
                                //enrt.Lat = "skip";
                                enrt.Lat = "inArea";

                                //RouteSegment rs = new RouteSegment();
                                //rs = rtSeg;
                                //rs.CodeType = "inArea";
                                //enrt.Routes.Clear();
                                //enrt.Routes.Add(rs);
                                //break;
                            }
                        }
                    }
                }

                #endregion
                ///////////////////////

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                #endregion






                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region CalcGeometryBag (selected features extent)

                IGeometryCollection bag = new GeometryBagClass() as IGeometryCollection;

                var _featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.Enroute) select element).ToList();
                Application.DoEvents();

                if (_featureList != null && _featureList.Count > 0)
                {
                    try
                    {

                        foreach (Enroute enrt in _featureList)
                        {

                            System.Diagnostics.Debug.WriteLine(enrt.ToString());

                            foreach (RouteSegment rtseg in enrt.Routes)
                            {
 
                                try
                                {
                                    if (rtseg.StartPoint == null || rtseg.EndPoint == null) continue;
                                    if (rtseg.StartPoint.Geo == null) rtseg.StartPoint.RebuildGeo();
                                    if (rtseg.EndPoint.Geo == null) rtseg.EndPoint.RebuildGeo();
                                    if (rtseg.StartPoint.Geo == null || rtseg.EndPoint.Geo == null) continue;

                                    bag.AddGeometry(rtseg.StartPoint.Geo, ref Missing, ref Missing);
                                    bag.AddGeometry(rtseg.EndPoint.Geo, ref Missing, ref Missing);
                                }
                                catch
                                {
                                    System.Diagnostics.Debug.WriteLine(rtseg.ToString());
                                }

                            }
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("");
                    }
                }
                #endregion



 
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                 double AnnoScale = frm._mapScale;//ChartsHelperClass.Calc_Scale(frm.mapSize_Height, frm.mapSize_Width, bag, SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);

                alrtForm.label1.Text = "Initialization";
                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.AreaChart, projectName, destPath2, tmpName, SigmaApplication);
                if (FocusMap == null) return;

                alrtForm.label1.Text = "Maps projection settings";
                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region ChangeProjectionAndMeredian

                if (bag != null && !((IGeometryBag)bag).Envelope.IsEmpty)
                {
                    double newX = (((IGeometryBag)bag).Envelope as IArea).Centroid.X;
                    EsriUtils.ChangeProjectionAndMeredian(newX, FocusMap);

                }

                #endregion


                FocusMap.MapScale = AnnoScale;

                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\AreaChart\", "areachart.sce");
                SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.AreaChart;

                if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

                alrtForm.label1.Text = "Work space initialization";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

                //////////////////////////////////////////////
                DpnList = new List<string>();
                NavaidList = new List<string>();

                alrtForm.label1.Text = "Validation";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

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
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                TerminalChartsUtil.SaveVerticalStructureGeo(obstacleList, Anno_ObstacleGeo_featClass, 0, vertUom.ToString(), distUom.ToString());


                TerminalChartsUtil.CreateAirportAnno(ADHPList, Anno_Airport_Geo_featClass,  FocusMap);

                EnrouteChartUtils.DpnList.Clear();
                EnrouteChartUtils.NavaidList.Clear();
                EnrouteChartUtils.CreateRouteSegments_ChartElements(ENRT_featureList, vertUom.ToString(), distUom.ToString(), Anno_RouteGeo_featClass, Anno_NavaidGeo_featClass, Anno_DesignatedGeo_featClass, FocusMap, pSpatialReference);


                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                 #region  убрать тип airspace в первой строке аннотации

                 ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airspace");
                 if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);
 
                 #endregion


                 TerminalChartsUtil.CreateAirspace_ChartElements(arspc_featureList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale,vertUom.ToString(),distUom.ToString(), arspBufWidth, createArspsSgnFlag, _extent: selectedRec);

                  alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                EnrouteChartUtils.CreateHolding_ChartElements(hlng_featureList, vertUom.ToString(), distUom.ToString(), Anno_HoldingGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference);


                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.CreateProcedureLegs_ChartElements(selectedLegs, rnavFlag, FocusMap, pSpatialReference, -99, 
                    Anno_LegGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, AnnoFacilitymakeUpGeo_featClass, Anno_HoldingGeo_featClass, Anno_HoldingpathGeo_featClass,
                    vertUom, distUom, ref NavaidList, ref DpnList);

                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                if (frm._AllVOR_DMEflag)
                {
                    EnrouteChartUtils.CreateNavaids_ChartElements(nvdsList, vertUom.ToString(), Anno_NavaidGeo_featClass, FocusMap);


                }


                TerminalChartsUtil.CreateGeoBorderAnno(GeoBorderList, FocusMap);

                



                alrtForm.label1.Text = "Updating grahics elements";
                #region Update dinamic text
                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, null, null, null, vertUom.ToString(), distUom.ToString(), (int)RNPFlag, null);


                #endregion


                


                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                alrtForm.label1.Text = "Finalisation";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                ChartsHelperClass.ChartsFinalisation(SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, projectName, "Airspace", extentBookmark, "codeType = 'TMA' OR codeType = 'TMA_P'");



                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                ChartElementsManipulator.CreateSigmaLog(destPath2 );
                EsriWorkEnvironment.EsriUtils.CreateJPEG_FromActiveView(this.SigmaHookHelper.ActiveView, destPath2 + @"\ContentImage.jpg");


                alrtForm.Close();
                ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Chart is created successfully", global::SigmaChart.Properties.Resources.SigmaMessage);
                msgFrm.TopMost = true;
                msgFrm.checkBox1.Visible = true;
                msgFrm.ShowDialog();
                if (msgFrm.checkBox1.Checked) Process.Start(destPath2 + @"\SIGMA_ResultsInfo.txt");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private List<SegmentPoint> GetProceduresPointsList(List<PDMObject> selectedProc)
        {
            List<SegmentPoint> res = new List<SegmentPoint>();
            bool flag = false;
            foreach (Procedure proc in selectedProc)
            {
                foreach (ProcedureTransitions trans in proc.Transitions)
                {
                    foreach (var leg in trans.Legs)
                    {
                        if (leg.StartPoint == null && proc.ProcedureType == PROC_TYPE_code.STAR) continue;
                        if (leg.EndPoint == null && proc.ProcedureType == PROC_TYPE_code.SID) continue;

                        if (proc.ProcedureType == PROC_TYPE_code.STAR) res.Add(leg.StartPoint);
                        if (proc.ProcedureType == PROC_TYPE_code.SID) res.Add(leg.EndPoint);
                        flag = true;
                        break;
                    }
                    if (flag) break;

                }
            }

            return res;
        }

        public override IWorkspaceEdit InitEnvironment_Workspace(string pathToTemplateFileXML, ESRI.ArcGIS.Carto.IMap ActiveMap)
        {
            SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);
            SigmaDataCash.ChartElementList = new List<object>();
            SigmaDataCash.AnnotationFeatureClassList = null;

            ILayer _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(ActiveMap, "AirportCartography");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;
            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count == 0)
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RouteSegmentCartography"))
                Anno_RouteGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_RouteGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography"))
                Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC"))
                AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography"))
                Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GeoBorderCartography"))
                Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];
            }
            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ProcedureLegsCartography")) Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VerticalStructurePointCartography"))
                Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayCartography")) AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("FacilityMakeUpCartography")) AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.AreaChart, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingPath")) Anno_HoldingpathGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingPath"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.STARChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_HoldingpathGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingPath"];
            }

            return workspaceEdit;
        }

        public void ClosedEnrouteValidatorReportWindow(object sender, EventArgs arg)
        {

            //CreateEnrouteChart();


        }

    
 
   
        //private void CreateHolding_ChartElements(List<PDMObject> hlng_featureList, string vertUom, string distUom)
        //{

        //    foreach (PDM.HoldingPattern hlng in hlng_featureList)
        //    {

        //        try
        //        {

        //            #region

        //            if (hlng.HoldingPoint == null) continue;
        //            if (hlng.HoldingPoint.Geo == null) hlng.HoldingPoint.RebuildGeo();
        //            PDMObject pdmObj = ChartsHelperClass.SaveHolding_PointChartGeo(Anno_HoldingGeo_featClass, hlng);

        //            ChartElement_SigmaCollout_Designatedpoint chrtEl_HldPnt = null;


        //            if (hlng.HoldingPoint.PointChoice == PDM.PointChoice.DesignatedPoint)
        //            {
        //                if (DpnList.IndexOf(hlng.HoldingPoint.PointChoiceID) < 0)
        //                {

        //                    DpnList.Add(hlng.HoldingPoint.PointChoiceID);
        //                    ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, hlng.HoldingPoint, hlng.HoldingPoint.Geo);

        //                    chrtEl_HldPnt = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
        //                    IElement el = ChartsHelperClass.CreateSegmentPointAnno(hlng.HoldingPoint, chrtEl_HldPnt);
        //                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldPnt.Name, hlng.HoldingPoint.PointChoiceID, el, ref chrtEl_HldPnt, chrtEl_HldPnt.Id, FocusMap.MapScale);
        //                    Application.DoEvents();
        //                }
        //            }

        //            if (hlng.HoldingPoint.PointChoice == PDM.PointChoice.Navaid)
        //            {


        //                if (NavaidList.IndexOf(hlng.HoldingPoint.PointChoiceID) < 0 && pdmObj!=null)
        //                {

        //                    NavaidList.Add(hlng.HoldingPoint.PointChoiceID);
        //                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)pdmObj);

        //                    ChartElement_SigmaCollout_Navaid chrtEl_HldPnt_NAV = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
        //                    IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)pdmObj, chrtEl_HldPnt_NAV,vertUom);
        //                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldPnt_NAV.Name, hlng.HoldingPoint.PointChoiceID, el, ref chrtEl_HldPnt_NAV, chrtEl_HldPnt_NAV.Id, FocusMap.MapScale);
        //                    Application.DoEvents();
        //                }
        //            }

        //            ///// limits
        //            ChartElement_SimpleText chrtEl_Hldng = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst,"HoldingPattern"); 

        //            chrtEl_Hldng.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][0].DataSource);
        //            chrtEl_Hldng.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][1].DataSource);

        //            chrtEl_Hldng.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][0].DataSource);
        //            chrtEl_Hldng.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][1].DataSource);

        //            chrtEl_Hldng.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][0].DataSource,1);
        //            chrtEl_Hldng.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][1].DataSource);


        //            chrtEl_Hldng.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
        //            if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_Hldng.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;

        //            chrtEl_Hldng.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_Hldng.Slope);

        //            IElement hldng_lim_el = (IElement)chrtEl_Hldng.ConvertToIElement();
        //            hldng_lim_el.Geometry = hlng.HoldingPoint.Geo;

        //            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Hldng.Name, hlng.HoldingPoint.ID, hldng_lim_el, ref chrtEl_Hldng, chrtEl_Hldng.Id, FocusMap.MapScale);



        //            double magVar;
        //            try
        //            {
        //                double? altitude = hlng.ConvertValueToMeter(hlng.LowerLimit.Value, hlng.LowerLimitUOM.ToString()) / 1000;
        //                magVar = ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(hlng.HoldingPoint.Y.Value), Convert.ToDouble(hlng.HoldingPoint.X.Value), altitude.Value,
        //                                    hlng.ActualDate.Day, hlng.ActualDate.Month, hlng.ActualDate.Year, 1);
        //            }
        //            catch { magVar = 0; }


        //            ///// InboundCourse
        //            ChartElement_SimpleText chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst,"HoldingPatternInboundCource");

        //            chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource,0,0, magVar);
        //            if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
        //                chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);


        //            chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
        //            if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
        //            chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);

        //            IElement hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
        //            hldng_cource_el.Geometry = ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, 1000, FocusMap, pSpatialReference); 

        //            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.HoldingPoint.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);





        //            ////////////////////////////////////

        //            ///// OutboundCourse
        //            chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst,"HoldingPatternOutboundCource");

        //            chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource, 0, 0, magVar);
        //            if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
        //                chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);

        //            chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
        //            if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
        //            chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);


        //            hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
        //            hldng_cource_el.Geometry = ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, -1000, FocusMap, pSpatialReference); 

        //            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.HoldingPoint.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);
        //            ////////////////////////////////////


        //            #endregion
        //        }
        //        catch
        //        {
        //            continue;
        //        }

        //    }



        //}


        private void CreateNavaids_ChartElements(List<PDMObject> nvdsList, string vertUom)
        {
            foreach (NavaidSystem navSystem in nvdsList)
            {
                try
                {
                    if (NavaidList.IndexOf(navSystem.ID) < 0)
                    {

                         NavaidList.Add(navSystem.ID);

                        ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                        IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, vertUom);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                        ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)navSystem);
                    }
                }
                catch { 
                    continue; 
                }
            }
        }

        #endregion

        #region ChartUpdate

        public override void UpdateChart(List<ChartCompare.DetailedItem> UpdateList, List<PDMObject> oldPdmList, List<PDMObject> newPdmList)
        {
            var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
            SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.EnrouteChart_Type;

            if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

            FocusMap = SigmaHookHelper.ActiveView.FocusMap;

            SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

            foreach (var item in UpdateList)
            {

                switch (item.ChangedStatus)
                {
                    case Status.New:

                        AddMapElements(item);

                        break;

                    case Status.Changed:

                        ChangeMapElements(item, oldPdmList);

                        break;

                    case Status.Deleted:

                        DeleteMapElements(item, oldPdmList);

                        break;

                    case Status.Missing:
                        break;
                    default:
                        break;
                }


            }

        }

        private void AddMapElements(DetailedItem item)
        {
            List<PDMObject> newList = new List<PDMObject>();
            newList.Add(item.Feature);


            switch (item.Feature.PDM_Type)
            {

                //case PDM_ENUM.NavaidSystem:
                //    CreateNavaids_ChartElements(newList);

                //    break;
                //case PDM_ENUM.SegmentPoint:
                //    break;
                //case PDM_ENUM.Enroute:
                //    CreateRouteSegments_ChartElements(newList);

                //    break;
                //case PDM_ENUM.HoldingPattern:
                //    CreateHolding_ChartElements(newList);
                //    break;
                //case PDM_ENUM.Airspace:

                //    #region  убрать тип airspace в первой строке аннотации

                //    ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst,"Airspace");
                //    if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);

                //    #endregion

                //    TerminalChartsUtil.CreateAirspace_ChartElements(newList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale, 5);
                //    break;
                //case PDM_ENUM.RouteSegment:
                //    RouteSegment newRs = (RouteSegment)item.Feature;
                //    Enroute tmpEnrt = new Enroute { TxtDesig = newRs.RouteFormed, Routes = new List<RouteSegment>() };
                //    newRs.SourceDetail = "";
                //    tmpEnrt.Routes.Add(newRs);
                //    newList[0] = tmpEnrt;
                //    CreateRouteSegments_ChartElements(newList);
                //    break;
                default:
                    break;
            }

        }


        private void ChangeMapElements(DetailedItem item, List<PDMObject> oldPdmList)
        {
            // найти аннотацию с текущим item.ID
            List<object> sigmaEl_collection = GetChartElement_List(item, oldPdmList);

            // Обновить текстовые поля
            for (int i = 0; i <= sigmaEl_collection.Count - 1; i++)
            {
                if (sigmaEl_collection[i] is ChartElement_SimpleText)
                {

                    ChartElement_SimpleText sigmaEl = (ChartElement_SimpleText)sigmaEl_collection[i];

                    foreach (FieldLog _FieldLog in item.FieldLogList)
                    {

                        if (_FieldLog.FieldName.StartsWith("Designator") && item.Feature.PDM_Type == PDM_ENUM.WayPoint) _FieldLog.FieldName = "SegmentPointDesignator";
                        if (_FieldLog.FieldName.StartsWith("TxtDesig") && item.Feature.PDM_Type == PDM_ENUM.Enroute) _FieldLog.FieldName = "RouteFormed";

                        UpdateTextContents(sigmaEl.TextContents, _FieldLog);

                        if (sigmaEl is ChartElement_BorderedText_Collout_CaptionBottom)
                        {
                            ChartElement_BorderedText_Collout_CaptionBottom sigmaEl1 = (ChartElement_BorderedText_Collout_CaptionBottom)sigmaEl;

                            if (sigmaEl1.CaptionTextLine != null && sigmaEl1.CaptionTextLine.Count > 0)
                            {
                                UpdateTextContents(sigmaEl1.CaptionTextLine, _FieldLog);
                            }

                            if (sigmaEl1.BottomTextLine != null && sigmaEl1.BottomTextLine.Count > 0)
                            {
                                UpdateTextContents(sigmaEl1.BottomTextLine, _FieldLog);

                            }

                        }

                        if (sigmaEl is ChartElement_RouteDesignator)
                        {
                            ChartElement_RouteDesignator sigmaEl1 = (ChartElement_RouteDesignator)sigmaEl;
                            bool wrapflag = sigmaEl1.WrapRouteDesignatorText;

                            if(!wrapflag) sigmaEl1.WrapDesignatorText();

                            foreach (var ln in sigmaEl1.RouteDesignatorSource)
                            {
                                foreach (var wrd in ln)
                                {
                                    if (wrd.DataSource.Condition.CompareTo(item.Id) == 0)
                                    {
                                        wrd.TextValue = _FieldLog.NewValueText;
                                    }
                                }
                            }

                            if (!wrapflag) sigmaEl1.ExpandDesignatorText();


                        }


                    }

                    IElement el = sigmaEl.ConvertToIElement() as IElement;

                    ChartElementsManipulator.UpdateSingleElementToDataSet(sigmaEl.Name, sigmaEl.Id.ToString(), el, ref sigmaEl);

                }

            }
        }

        private void UpdateTextContents(List<List<ANCOR.MapCore.AncorChartElementWord>> _TextContents, FieldLog _FieldLog)
        {
            foreach (var Line in _TextContents)
            {
                foreach (ANCOR.MapCore.AncorChartElementWord Word in Line)
                {
                    if (Word.DataSource.Value.StartsWith(_FieldLog.FieldName))
                    {
                        Word.TextValue = _FieldLog.NewValueText;
                    }
                }
            }
        }

        private List<object> GetChartElement_List(DetailedItem DetailedItem, List<PDMObject> oldPdmList)
        {
            List<string> pdmId = GetElementID(DetailedItem, oldPdmList);

            List<object> _res = SigmaDataCash.ChartElementList.FindAll(obj => (obj is AbstractChartElement) && IsLinckedGeoIdPresented((AbstractChartElement)obj, pdmId));

            return _res;
        }


        private bool IsLinckedGeoIdPresented(AbstractChartElement obj, List<string> IdList)
        {
            bool res = false;
            foreach (var item in IdList)
            {

                string uid = obj.LinckedGeoId;
                if (obj is ChartElement_RouteDesignator)
                {
                    uid = ""; 
                    foreach (var line in ((ChartElement_RouteDesignator)obj).RouteDesignatorSource)
                    {

                        foreach (var wrd in line)
                        {
                            uid = uid + wrd.DataSource.Condition + "/";

                        }
                    }


                    if (uid.Contains(item))
                    {
                        res = true;
                        break;
                    }
                    
                }

                if (item.Contains(uid))
                {
                    res = true;
                    break;
                }
            }

            return res;
        }


        private void DeleteMapElements(DetailedItem item, List<PDMObject> oldPdmList)
        {
            // найти аннотацию с текущим item.ID
            List<string> _ID_List = GetElementID(item, oldPdmList);
            IFeatureClass geoClass = null;

            switch (item.Feature.PDM_Type)
            {

                case PDM_ENUM.NavaidSystem:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["Navaids_C"]; ;
                    break;
                case PDM_ENUM.SegmentPoint:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPoint_C"];
                    break;
                case PDM_ENUM.Enroute:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegment_C"];
                    break;
                case PDM_ENUM.HoldingPattern:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["Holding_C"];
                    break;
                case PDM_ENUM.Airspace:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["Airspace_C"];
                    break;
                default:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["Airspace_C"];
                    break;
            }

            foreach (KeyValuePair<string, object> pair in SigmaDataCash.AnnotationFeatureClassList)
            {
                IFeatureClass fC = (IFeatureClass)pair.Value;
                if (fC.AliasName.StartsWith("Mirror")) continue;

                foreach (var id in _ID_List)
                {

                    string qry = fC.FindField("FeatureGUID") > 0 ? "FeatureGUID = '" + id + "'" : "PdmUid= '" + id + "'";

                    IQueryFilter featFilter = new QueryFilterClass();

                    featFilter.WhereClause = qry;

                    IFeatureCursor featCur = fC.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    Marshal.ReleaseComObject(featCur);
                }

               

            }

            foreach (var id in _ID_List)
            {
                string qry = "FeatureGUID = '" + id + "'";
                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = qry;

                IFeatureCursor featCur = geoClass.Search(featFilter, false);

                IFeature _Feature = null;
                while ((_Feature = featCur.NextFeature()) != null)
                {
                    _Feature.Delete();
                }

                Marshal.ReleaseComObject(featCur);

            }


        }

        private List<string> GetElementID(DetailedItem item, List<PDMObject> oldPdmList)
        {
            List<string> res = new List<string>();

            res.Add(item.Feature.ID);

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.WayPoint )
            {
                #region

                // определяем использовался ли этот Way point как точка маршрута
                var list1 = (from element in oldPdmList
                             where (element != null) && (element is Enroute)
                                 && (((Enroute)element).Routes != null)
                                 && (((Enroute)element).Routes.Count > 0)
                             select element).ToList();

                foreach (Enroute Enrt in list1)
                {
                    foreach (RouteSegment Rt in Enrt.Routes)
                    {
                        if (Rt.StartPoint != null && Rt.StartPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            res.Add(Rt.StartPoint.ID);
                            break;
                        }

                        if (Rt.EndPoint != null && Rt.EndPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            res.Add(Rt.EndPoint.ID);
                            break;
                        }
                    }
                }

                #endregion

            }

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
            {
                #region

                // определяем использовался ли этот Way point как точка маршрута
                var list1 = (from element in oldPdmList
                             where (element != null) && (element is Enroute)
                                 && (((Enroute)element).Routes != null)
                                 && (((Enroute)element).Routes.Count > 0)
                             select element).ToList();

                foreach (Enroute Enrt in list1)
                {
                    foreach (RouteSegment Rt in Enrt.Routes)
                    {
                        if (Rt.StartPoint != null && Rt.StartPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            res.Add(Rt.StartPoint.PointChoiceID);
                            break;
                        }

                        if (Rt.EndPoint != null && Rt.EndPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            res.Add(Rt.EndPoint.PointChoiceID);
                            break;
                        }
                    }
                }

                #endregion

            }

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.Enroute)
            {
                #region

                var list1 = (from element in oldPdmList
                             where (element != null) && (element is Enroute)
                                 && (((Enroute)element).Routes != null)
                                 && (((Enroute)element).Routes.Count > 0)
                                 && (((Enroute)element).ID.CompareTo(((Enroute)item.Feature).ID)==0)
                             select element).ToList();

                foreach (Enroute Enrt in list1)
                {
                    foreach (RouteSegment Rt in Enrt.Routes)
                    {

                        res.Add(Rt.ID);

                    }
                }

                #endregion
            }

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.RouteSegment)
            {
                #region

                var list1 = (from element in oldPdmList
                             where (element != null) && (element is Enroute)
                                 && (((Enroute)element).Routes != null)
                                 && (((Enroute)element).Routes.Count > 0)
                             select element).ToList();

                foreach (Enroute Enrt in list1)
                {
                    foreach (RouteSegment Rt in Enrt.Routes)
                    {
                        if (((RouteSegment)item.Feature).RouteFormed.CompareTo(Enrt.TxtDesig) == 0)
                        res.Add(Rt.ID);

                    }
                }

                #endregion
            }

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.Airspace)
            {
                #region

                var list1 = (from element in oldPdmList
                             where (element != null) && (element is Airspace)
                                 && (((Airspace)element).AirspaceVolumeList != null)
                                 && (((Airspace)element).AirspaceVolumeList.Count > 0)
                                 && (((Airspace)element).ID.CompareTo(((Airspace)item.Feature).ID) == 0)
                             select element).ToList();

                foreach (Airspace arsps in list1)
                {
                    foreach (AirspaceVolume vol in arsps.AirspaceVolumeList)
                    {

                        res.Add(vol.ID);

                    }
                }

                #endregion
            }
                

            return res;
        }
        
        #endregion



    }
}
