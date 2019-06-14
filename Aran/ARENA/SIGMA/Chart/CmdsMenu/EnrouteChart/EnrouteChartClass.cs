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
    public class EnrouteChartClass : AbstaractSigmaChart
    {

        private IFeatureClass Anno_RouteGeo_featClass = null;
        private IFeatureClass Anno_DesignatedGeo_featClass = null;
        private IFeatureClass Anno_NavaidGeo_featClass = null;
        private IFeatureClass AnnoAirspaceGeo_featClass = null;
        private IFeatureClass Anno_HoldingGeo_featClass = null;
        private IFeatureClass Anno_GeoBorder_Geo_featClass = null;
        private IFeatureClass Anno_Airport_Geo_featClass = null;
        private IFeatureClass Anno_DecorPointCartography_featClass = null;
        private IFeatureClass Anno_DecorLineCartography_featClass = null;
        private IFeatureClass Anno_DecorPolygonCartography_featClass = null;

        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference =null;
        List<string> DpnList = null;
        List<string> NavaidList = null;

        public EnrouteChartClass()
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
                EnroutWizardForm frm = new EnroutWizardForm(_AiracCircle);

                if (frm.ShowDialog() == DialogResult.Cancel) return;


                _AiracCircle = frm._AiracCircle;
                IAOIBookmark extentBookmark = frm._bookmark;
                UOM_DIST_VERT vertUom = frm.vertUom;
                UOM_DIST_HORZ distUom = frm.distUom;
                List<string> _enrt_chanels = null;
                int arspBufWidth = frm.arspBufWidth;
                bool createArspsSgnFlag = frm.arspSign;
                bool MagTrack = frm.magBearing;
                int upperSeparationValue = frm._upperSeparation;
                int lowerSeparationValue = frm._lowerSeparation;


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

                alrtForm.label1.Text = "Preparation";
                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region Validator

                //ChartValidator.EnrouteValidator vldr = new ChartValidator.EnrouteValidator(SigmaApplication);


                //vldr.AddDestroyEventHandler(ClosedEnrouteValidatorReportWindow);


                //string er;
                ////byte result = vldr.Check(DataCash.ProjectEnvironment.Data.PdmObjectList, out er);
                //byte result = vldr.Check(DataCash.ProjectEnvironment.Data.PdmObjectList, out er);

                //if (result == 2)
                //{
                //    MessageBox.Show(er);
                //}
                //else if (result == 4) return;

                #endregion

                alrtForm.label1.Text = "Data quering";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region data Query

                List<PDMObject> ENRT_featureList = DataCash.GetEnroutes(true, selectedLevel);

                List<PDMObject> hlng_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                    where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.HoldingPattern)
                                                        && (((PDM.HoldingPattern)element).Type == PDM.CodeHoldingUsage.ENR)
                                                    select element).ToList();

                List<PDMObject> arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Airspace) select element).ToList();

                List<PDMObject> wypntsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.WayPoint).ToList();
                List<PDMObject> nvdsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.NavaidSystem).ToList();
                List<PDMObject> GeoBorderList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.GeoBorder).ToList();
                //List<PDMObject> ADHPList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.AirportHeliport).ToList();
                List<AirportHeliport> ADHPList = DataCash.GetAirportlist();

                List<PDMObject> obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => (pdm is PDM.VerticalStructure) && 
                     ((PDM.VerticalStructure)pdm).ObstacleAreaType!=null && ((PDM.VerticalStructure)pdm).ObstacleAreaType.Contains(CodeObstacleAreaType.AREA1)).ToList();
                if (obstacleList == null || obstacleList.Count == 0) obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure).ToList();

                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
                this.EffectiveDate = start;
                this.ADHP = "";
                this.airacCircle = _AiracCircle.ToString();
                this.organization = "";

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<PDMObject> PdmElementsList = new List<PDMObject>();

                List<PDMObject> ENRT_featureList_ALL = DataCash.GetEnroutes();
                if (ENRT_featureList_ALL != null && ENRT_featureList_ALL.Count > 0) PdmElementsList.AddRange(ENRT_featureList_ALL.GetRange(0, ENRT_featureList_ALL.Count));
                if (hlng_featureList != null && hlng_featureList.Count > 0) PdmElementsList.AddRange(hlng_featureList.GetRange(0, hlng_featureList.Count));
                if (arspc_featureList != null && arspc_featureList.Count > 0) PdmElementsList.AddRange(arspc_featureList.GetRange(0, arspc_featureList.Count));
                if (wypntsList != null && wypntsList.Count > 0) PdmElementsList.AddRange(wypntsList.GetRange(0, wypntsList.Count));
                if (nvdsList != null && nvdsList.Count > 0) PdmElementsList.AddRange(nvdsList.GetRange(0, nvdsList.Count));
                if (GeoBorderList != null && GeoBorderList.Count > 0) PdmElementsList.AddRange(GeoBorderList.GetRange(0, GeoBorderList.Count));
                if (obstacleList != null && obstacleList.Count > 0) PdmElementsList.AddRange(obstacleList.GetRange(0, obstacleList.Count));
                if (ADHPList != null && ADHPList.Count > 0) PdmElementsList.AddRange(ADHPList.GetRange(0, ADHPList.Count));


                ChartsHelperClass.SaveSourcePDMFiles(destPath2, PdmElementsList);
                PdmElementsList = null;


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                #endregion


                ILayer _Layer = EsriUtils.getLayerByName(SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                if (_Layer == null) _Layer = EsriUtils.getLayerByName(SigmaHookHelper.ActiveView.FocusMap, "AirportCartography");

                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;

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

                            //System.Diagnostics.Debug.WriteLine(enrt.ToString());

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
                FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.EnrouteChart_Type, projectName, destPath2, tmpName, SigmaApplication);
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

                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
                SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.EnrouteChart_Type;

                if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

                alrtForm.label1.Text = "Work space initialization";
                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

                ////////////////////////////////////////////
                //try
                //{
                //    //alrtForm.label1.Text = "Validation";
                //    //alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                //    SigmaTerminal_EnrouteChartExamination sidExam = new SigmaTerminal_EnrouteChartExamination { _projectName = projectName, ExaminationData = new Dictionary<string, List<PDMObject>>(), SigmaChartTypes = (int)SigmaChartTypes.EnrouteChart_Type };
                //    sidExam.FillExaminationData(arspc_featureList, nvdsList, ADHPList, GeoBorderList, ENRT_featureList, hlng_featureList);
                //    sidExam.ChartExamination();

                //    sidExam.ShowExamResults();
                //}
                //catch { }
                //////////////////////////////////////////


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
                    RouteLevel = selectedLevel.ToString(),
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

                alrtForm.label1.Text = "Creating annotations: Airports";
                TerminalChartsUtil.CreateAirportAnno(ADHPList, Anno_Airport_Geo_featClass, FocusMap);

                EnrouteChartUtils.DpnList.Clear();
                EnrouteChartUtils.NavaidList.Clear();

                alrtForm.label1.Text = "Creating annotations: Route Segments";
                EnrouteChartUtils.CreateRouteSegments_ChartElements(ENRT_featureList, vertUom.ToString(), distUom.ToString(), Anno_RouteGeo_featClass, Anno_NavaidGeo_featClass, Anno_DesignatedGeo_featClass, FocusMap, pSpatialReference, MagTrack);

                 alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                 #region  убрать тип airspace в первой строке аннотации

                 ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airspace");
                 if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);

                #endregion

                alrtForm.label1.Text = "Creating annotations: Airspaces";
                TerminalChartsUtil.CreateAirspace_ChartElements(arspc_featureList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale,vertUom.ToString(),
                    distUom.ToString(), arspBufWidth, createArspsSgnFlag,UpperSeparation_FL: upperSeparationValue, LowerSeparation_FL: lowerSeparationValue);

                  alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                alrtForm.label1.Text = "Creating annotations: Holdings";
                EnrouteChartUtils.CreateHolding_ChartElements(hlng_featureList, vertUom.ToString(), distUom.ToString(), Anno_HoldingGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference);

                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                if (frm._AllVOR_DMEflag)
                {
                    alrtForm.label1.Text = "Creating annotations: Navaids";
                    EnrouteChartUtils.CreateNavaids_ChartElements(nvdsList, vertUom.ToString(), Anno_NavaidGeo_featClass,FocusMap);

                }

                alrtForm.label1.Text = "Creating annotations: Geo Borders";
                TerminalChartsUtil.CreateGeoBorderAnno(GeoBorderList, FocusMap);

                alrtForm.label1.Text = "Updating grahics elements";
                #region Update dinamic text

                alrtForm.progressBar1.Value++;

                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, null, null, null, vertUom.ToString(), distUom.ToString(), 0, _enrt_chanels);


                #endregion


                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                alrtForm.label1.Text = "Finalisation";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                ChartsHelperClass.ChartsFinalisation(SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, projectName, "DesignatedPointCartography", extentBookmark);



                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                ChartElementsManipulator.CreateSigmaLog(destPath2);
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
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RouteSegmentCartography"))
                Anno_RouteGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_RouteGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography"))
                Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC"))
                AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography"))
                Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GeoBorderCartography"))
                Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];
            }
            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
            }

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
            else
            {
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.EnrouteChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
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

            if (UpdateList == null) return;
            if (oldPdmList == null) return;
            if (newPdmList == null) return;

            AlertForm alrtForm = new AlertForm();

            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;



            alrtForm.progressBar1.Visible = true;
            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.progressBar1.Maximum = UpdateList.Count;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.label1.Text = "Update chart...";
            //Application.DoEvents();
            alrtForm.label1.Visible = true;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
            SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.EnrouteChart_Type;

            if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

            FocusMap = SigmaHookHelper.ActiveView.FocusMap;



            string FN = System.IO.Path.GetDirectoryName((SigmaApplication.Document as IDocumentInfo2).Path);
            chartInfo ci = EsriUtils.GetChartIno(FN);

            #region MyRegion

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RouteSegmentCartography"))
                Anno_RouteGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];//null;
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography"))
                Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];//null;
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC"))
                AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];//null;
           

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography"))
                Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GeoBorderCartography"))
                Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];//null;
            
            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
            

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
           

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];


            #region Чистка битых ссылок

            var adhpOMAA = (from element in oldPdmList
                         where (element != null) && (element is AirportHeliport)
                             && (((AirportHeliport)element).Designator != null)
                             && (((AirportHeliport)element).Designator.StartsWith("OMAA"))
                         select element).FirstOrDefault();

            if (adhpOMAA != null)
            {
                List<object> _res = SigmaDataCash.ChartElementList.FindAll(obj => (obj is AbstractChartElement) && (((AbstractChartElement)obj).Name.StartsWith("HoldingPatternInboundCource") ||
                ((AbstractChartElement)obj).Name.StartsWith("HoldingPatternOutboundCource") || ((AbstractChartElement)obj).Name.StartsWith("HoldingPattern")));


                string qry1 = "AncorUID IS NULL OR AncorUID IN (";
                if (_res != null)
                {
                    foreach (var item in _res)
                    {
                        qry1 = qry1 + "'" + ((AbstractChartElement)item).Id + "',";
                    }

                    qry1 = qry1 + "'0')";
                }

                IQueryFilter featFilter = new QueryFilterClass();
                featFilter.WhereClause = qry1;

                IFeatureCursor featCur = (SigmaDataCash.AnnotationFeatureClassList["RouteSegmentAnnoMagTrack"] as IFeatureClass).Search(featFilter, false);

                IFeature _Feature = null;
                while ((_Feature = featCur.NextFeature()) != null)
                {
                    _Feature.Delete();
                }

                featCur = (SigmaDataCash.AnnotationFeatureClassList["DesignatedPointAnno"] as IFeatureClass).Search(featFilter, false);

                _Feature = null;
                while ((_Feature = featCur.NextFeature()) != null)
                {
                    _Feature.Delete();
                }

                featCur = (SigmaDataCash.AnnotationFeatureClassList["Mirror"] as IFeatureClass).Search(featFilter, false);

                _Feature = null;
                while ((_Feature = featCur.NextFeature()) != null)
                {
                    _Feature.Delete();
                }


                featFilter.WhereClause = "FeatureGUID IS NULL";
                foreach (var FC in SigmaDataCash.AnnotationLinkedGeometryList.Keys)
                {
                    IFeatureClass featureClass = SigmaDataCash.AnnotationLinkedGeometryList[FC] as IFeatureClass;
                    if (featureClass.Fields.FindField("FeatureGUID") < 0) continue;
                    featCur = featureClass.Search(featFilter, false);
                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    int fID = featureClass.Fields.FindField("FeatureGUID");
                    IField upField = featureClass.Fields.Field[fID];

                    IFieldEdit fieldEdit = upField as IFieldEdit;
                    fieldEdit.IsNullable_2 = true;

                }






                #region Re-create Inbound OutBound anno  

                List<PDMObject> HldList = oldPdmList.FindAll(obj => (obj.PDM_Type == PDM_ENUM.HoldingPattern && ((HoldingPattern)obj).Type == CodeHoldingUsage.ENR));


                if (HldList != null)
                {
                    foreach (HoldingPattern hlng in HldList)
                    {
                        hlng.HoldingPoint.RebuildGeo();
                        double magVar;
                        try
                        {
                            double? altitude = hlng.ConvertValueToMeter(hlng.LowerLimit.Value, hlng.LowerLimitUOM.ToString()) / 1000;
                            magVar = ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(hlng.HoldingPoint.Y.Value), Convert.ToDouble(hlng.HoldingPoint.X.Value), altitude.Value,
                                                hlng.ActualDate.Day, hlng.ActualDate.Month, hlng.ActualDate.Year, 1);
                        }
                        catch { magVar = 0; }


                        #region Limits

                        ChartElement_SimpleText chrtEl_Hldng = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPattern");

                        chrtEl_Hldng.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][0].DataSource);
                        chrtEl_Hldng.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][1].DataSource);


                        if (hlng.UpperLimitUOM == UOM_DIST_VERT.FL)
                        {
                            var ds = chrtEl_Hldng.TextContents[1][0].DataSource.Clone();
                            chrtEl_Hldng.TextContents[1][0].DataSource = chrtEl_Hldng.TextContents[1][1].DataSource;
                            chrtEl_Hldng.TextContents[1][1].DataSource = (AncorDataSource)ds;
                        }

                        chrtEl_Hldng.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][0].DataSource);
                        chrtEl_Hldng.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][1].DataSource);

                        chrtEl_Hldng.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][0].DataSource, 1);
                        chrtEl_Hldng.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][1].DataSource);


                        chrtEl_Hldng.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                        if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_Hldng.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;

                        chrtEl_Hldng.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_Hldng.Slope);

                        IElement hldng_lim_el = (IElement)chrtEl_Hldng.ConvertToIElement();
                        hldng_lim_el.Geometry = hlng.HoldingPoint.Geo;


                        chrtEl_Hldng.Placed = false;

                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Hldng.Name, hlng.ID, hldng_lim_el, ref chrtEl_Hldng, chrtEl_Hldng.Id, FocusMap.MapScale);

                        #endregion

                        #region InboundCourse

                        ChartElement_SimpleText chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternInboundCource");

                        chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource, 0, 0, magVar);
                        if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
                            chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);


                        chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                        if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
                        chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);

                        IElement hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
                        hldng_cource_el.Geometry = hlng.HoldingPoint.Geo;//ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, 1000, FocusMap, pSpatialReference);


                        chrtEl_HldngCource.Placed = false;
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);

                        #endregion

                        #region OutboundCourse

                        chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternOutboundCource");

                        chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource, 0, 0, magVar);
                        if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
                            chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);

                        chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                        if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
                        chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);


                        hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
                        hldng_cource_el.Geometry = hlng.HoldingPoint.Geo;//ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, -1000, FocusMap, pSpatialReference);

                        chrtEl_HldngCource.Placed = false;

                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);

                        #endregion

                        #region Holding

                        IFeatureClass hldClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];

                        featFilter = new QueryFilterClass();
                        featFilter.WhereClause = "SegmentPointID = '" + hlng.HoldingPoint.SegmentPointDesignator + "' AND" +
                            " duration_Time = " + hlng.Duration_Distance + " AND upperLimit = " + hlng.UpperLimit.Value +
                            " AND lowerLimit = " + hlng.LowerLimit.Value;

                        featCur = hldClass.Update(featFilter, false);
                        _Feature = null;
                        while ((_Feature = featCur.NextFeature()) != null)
                        {
                            _Feature.Value[_Feature.Fields.FindField("FeatureGUID")] = hlng.ID;
                            _Feature.Store();
                        }


                        #endregion

                    }
                }

                #endregion

                Marshal.ReleaseComObject(featCur);
            }

            ChartsHelperClass.Create_CEFID_File(System.IO.Path.GetDirectoryName(((IMapDocument)SigmaApplication.Document).DocumentFilename));






            #endregion

            #endregion



            var delList = UpdateList.FindAll(di => di.ChangedStatus == Status.Deleted).ToList();
            foreach (var item in delList)
            {
                alrtForm.label1.Text = item.ToString();
                DeleteMapElements(item, oldPdmList);

                alrtForm.progressBar1.Value++;
            }

            bool r = false;
            foreach (var item in UpdateList)
            {
                if (item.ChangedStatus == Status.Deleted) continue;

                alrtForm.label1.Text = item.ToString();
                System.Diagnostics.Debug.WriteLine(item.ToString());

                switch (item.ChangedStatus)
                {
                    case Status.New:

                        r = AddMapElements(item, ci);

                        break;

                    case Status.Changed:

                        r = ChangeMapElements(item, oldPdmList, ci);

                        break;

                    case Status.Deleted:

                        DeleteMapElements(item, oldPdmList);

                        break;

                    case Status.Missing:
                        break;
                    default:
                        break;
                }

                //if (!r)
                //    MessageBox.Show("Error " + item.ToString());
                alrtForm.progressBar1.Value++;


            }

            ChartsHelperClass.Create_CEFID_File(System.IO.Path.GetDirectoryName(((IMapDocument)SigmaApplication.Document).DocumentFilename));

            alrtForm.Close();
        }

        private bool AddMapElements(DetailedItem item, chartInfo ci)
        {
            try
            {
                ILayer _Layer = EsriUtils.getLayerByName(SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                if (_Layer == null) _Layer = EsriUtils.getLayerByName(SigmaHookHelper.ActiveView.FocusMap, "AirportCartography");

                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;


                List<PDMObject> newList = new List<PDMObject>();
                newList.Add(item.Feature);

                switch (item.Feature.PDM_Type)
                {

                    case PDM_ENUM.NavaidSystem:
                        List<string> NavaidList = new List<string>();
                        TerminalChartsUtil.CreateNavaids_ChartElements(newList, FocusMap.MapScale, ref NavaidList, Anno_NavaidGeo_featClass, ci.uomVert);

                        break;

                    //case PDM_ENUM.SegmentPoint:
                    //    break;

                    case PDM_ENUM.Enroute:
                        ci.uomVert = "FT";
                        ci.uomDist = "NM";

                        List<PDMObject> ENRT_featureList = DataCash.GetEnroutes(true, CODE_ROUTE_SEGMENT_CODE_LVL.BOTH, newList);
                        EnrouteChartUtils.CreateRouteSegments_ChartElements(ENRT_featureList, ci.uomVert, ci.uomDist, Anno_RouteGeo_featClass, Anno_NavaidGeo_featClass, Anno_DesignatedGeo_featClass, FocusMap, pSpatialReference);

                        break;

                    case PDM_ENUM.HoldingPattern:
                        EnrouteChartUtils.CreateHolding_ChartElements(newList, ci.uomVert, ci.uomDist, Anno_HoldingGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference);
                        break;

                    case PDM_ENUM.Airspace:

                        #region  убрать тип airspace в первой строке аннотации

                        ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airspace");
                        if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);

                        #endregion

                        #region Delete airspace if exist

                        IFeatureClass AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                        IFeatureClass Anno_Airspace_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["AirspaceAnno"];
                        string qry = "";
                        IQueryFilter featFilter = new QueryFilterClass();

                        foreach (var arsps in newList)
                        {
                            foreach (var vPart in ((Airspace)arsps).AirspaceVolumeList)
                            {
                                qry = qry + "'" + vPart.ID + "',";
                            }
                        }

                        #region Delete from Geo_featClass

                        string qry1 = "FeatureGUID IN (" + qry + "'00000')";
                        featFilter.WhereClause = qry1;

                        IFeatureCursor featCur = AnnoAirspaceGeo_featClass.Search(featFilter, false);

                        IFeature _Feature = null;
                        while ((_Feature = featCur.NextFeature()) != null)
                        {
                            _Feature.Delete();
                        }

                        #endregion

                        #region Delete from Anno_featClass

                        string qry2 = "PdmUID IN (" + qry + "'00000')";
                        featFilter.WhereClause = qry2;

                        featCur = Anno_Airspace_featClass.Search(featFilter, false);

                        _Feature = null;
                        while ((_Feature = featCur.NextFeature()) != null)
                        {
                            _Feature.Delete();
                        }

                        #endregion

                        Marshal.ReleaseComObject(featCur);


                        #endregion

                        TerminalChartsUtil.CreateAirspace_ChartElements(newList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale, ci.uomVert, ci.uomDist, ci.widthBufer, ci.flag);

                        break;

                    case PDM_ENUM.RouteSegment:
                        RouteSegment newRs = (RouteSegment)item.Feature;
                        Enroute tmpEnrt = new Enroute { TxtDesig = newRs.RouteFormed, Routes = new List<RouteSegment>() };
                        if (newRs.ID_Route != null) tmpEnrt.ID = newRs.ID_Route;
                        else tmpEnrt.ID = Guid.NewGuid().ToString();
                        newRs.SourceDetail = "";
                        tmpEnrt.Routes.Add(newRs);
                        newList[0] = tmpEnrt;
                        EnrouteChartUtils.CreateRouteSegments_ChartElements(newList, ci.uomVert, ci.uomDist, Anno_RouteGeo_featClass, Anno_NavaidGeo_featClass, Anno_DesignatedGeo_featClass, FocusMap, pSpatialReference);

                        break;

                    default:
                        break;
                }

                return true;
            }
            catch {


                return false;
            }


        }


        private bool ChangeMapElements(DetailedItem item, List<PDMObject> oldPdmList, chartInfo ci)
        {
            bool res = false;
            try
            {
                // найти аннотацию с текущим item.ID
                List<string> pdmID = new List<string>();
                List<object> sigmaEl_collection = GetChartElement_List(item, oldPdmList);

                if ((sigmaEl_collection == null || sigmaEl_collection.Count == 0) && item.Feature.PDM_Type == PDM_ENUM.WayPoint)
                {
                    IQueryFilter featFilter = new QueryFilterClass();
                    if (Anno_DesignatedGeo_featClass == null) return false;
                    IFeatureCursor featCur = null;

                    featFilter.WhereClause = "SegmentPointDesignator = '" + ((WayPoint)item.Feature).Designator + "'";
                    featCur = Anno_DesignatedGeo_featClass.Search(featFilter, false);
                    IFeature pFeat = featCur.NextFeature();


                    if (pFeat != null)
                    {
                        int fID = pFeat.Fields.FindField("FeatureGUID");
                        if (fID > 0)
                        {
                            string fgd = pFeat.Value[fID].ToString();
                            sigmaEl_collection = SigmaDataCash.ChartElementList.FindAll(obj => (obj != null) && (obj is ChartElement_SimpleText) && ((ChartElement_SimpleText)obj).LinckedGeoId != null
                                                                                        && ((ChartElement_SimpleText)obj).LinckedGeoId.ToString().CompareTo(fgd) == 0).ToList();
                        }
                    }

                    Marshal.ReleaseComObject(featCur);


                }

                AirspaceBuffer buf = new AirspaceBuffer(this.SigmaHookHelper);

                // Обновить текстовые поля
                for (int i = 0; i <= sigmaEl_collection.Count - 1; i++)
                {
                    if (sigmaEl_collection[i] is ChartElement_SimpleText)
                    {

                        ChartElement_SimpleText sigmaEl = (ChartElement_SimpleText)sigmaEl_collection[i];

                        res = false;
                        foreach (FieldLog _FieldLog in item.FieldLogList)
                        {
                            if (!CheckFieldname(sigmaEl.TextContents, _FieldLog.FieldName) && !(sigmaEl is ChartElement_RouteDesignator)) continue;
                            if (sigmaEl is ChartElement_RouteDesignator && !CheckFieldname((ChartElement_RouteDesignator)sigmaEl, ((RouteSegment)item.Feature).ID_Route)) continue;


                            if (_FieldLog.FieldName.StartsWith("Designator") && item.Feature.PDM_Type == PDM_ENUM.WayPoint) _FieldLog.FieldName = "SegmentPointDesignator";
                            if (_FieldLog.FieldName.StartsWith("TxtDesig") && item.Feature.PDM_Type == PDM_ENUM.Enroute) _FieldLog.FieldName = "RouteFormed";


                            if (sigmaEl is ChartElement_SigmaCollout_Designatedpoint)
                            {
                                coordtype ct = ((ChartElement_SigmaCollout_Designatedpoint)sigmaEl).CoordType;
                                res = UpdateTextContents(sigmaEl.TextContents, _FieldLog, ct) || res;
                            }
                            //else if (!(sigmaEl is ChartElement_RouteDesignator))
                            else 
                                res = UpdateTextContents(sigmaEl.TextContents, _FieldLog) || res;

                            if (sigmaEl is ChartElement_BorderedText_Collout_CaptionBottom)
                            {
                                ChartElement_BorderedText_Collout_CaptionBottom sigmaEl1 = (ChartElement_BorderedText_Collout_CaptionBottom)sigmaEl;

                                if (sigmaEl1.CaptionTextLine != null && sigmaEl1.CaptionTextLine.Count > 0)
                                {
                                    res = UpdateTextContents(sigmaEl1.CaptionTextLine, _FieldLog) || res;
                                }

                                if (sigmaEl1.BottomTextLine != null && sigmaEl1.BottomTextLine.Count > 0)
                                {
                                    res = UpdateTextContents(sigmaEl1.BottomTextLine, _FieldLog) || res;

                                }

                            }

                            if (sigmaEl is ChartElement_RouteDesignator)
                            {
                                ChartElement_RouteDesignator sigmaEl1 = (ChartElement_RouteDesignator)sigmaEl;
                                bool wrapflag = sigmaEl1.WrapRouteDesignatorText;

                                if (!wrapflag) sigmaEl1.WrapDesignatorText();

                                foreach (var ln in sigmaEl1.RouteDesignatorSource)
                                {
                                    foreach (var wrd in ln)
                                    {
                                        if (wrd.DataSource.Condition.CompareTo(((RouteSegment)item.Feature).ID_Route) == 0 && _FieldLog.FieldName.StartsWith("RouteFormed"))
                                        {
                                            wrd.TextValue = _FieldLog.NewValueText;
                                            res = true;
                                        }
                                    }
                                }

                                if (!wrapflag) sigmaEl1.ExpandDesignatorText();


                                if (_FieldLog.FieldName.StartsWith("CodeDir"))
                                {
                                    if (_FieldLog.NewValueText.ToUpper().CompareTo("FORWARD") == 0)
                                    {
                                        sigmaEl1.RouteSegmentDirection = routeSegmentDirection.Forward;
                                        res = true;
                                    }
                                    else if (_FieldLog.NewValueText.ToUpper().CompareTo("BACKWARD") == 0)
                                    {
                                        sigmaEl1.RouteSegmentDirection = routeSegmentDirection.Backward;
                                        res = true;
                                    }
                                    else if (_FieldLog.NewValueText.ToUpper().CompareTo("BOTH") == 0)
                                    {
                                        sigmaEl1.RouteSegmentDirection = routeSegmentDirection.Both;
                                        res = true;
                                    }
                                }
                            }

                            if (res) ArenaStaticProc.SetObjectValue(item.Feature, _FieldLog.FieldName, _FieldLog.NewValueText);

                            if (item.Feature.PDM_Type == PDM_ENUM.Airspace)
                            {
                                Airspace arps = (Airspace)item.Feature;
                                if (arps.AirspaceVolumeList != null)
                                {
                                    //_FieldLog.FieldName = _FieldLog.FieldName.CompareTo("CodeID") == 0 ? "CodeId" : _FieldLog.FieldName;
                                    foreach (var vol in arps.AirspaceVolumeList)
                                    {
                                        ArenaStaticProc.SetObjectValue(vol, _FieldLog.FieldName, _FieldLog.NewValueText);
                                        res = true;

                                    }
                                }
                            }


                            if (item.Feature.PDM_Type == PDM_ENUM.Enroute)
                            {
                                Enroute enrt = (Enroute)item.Feature;
                                if (enrt.Routes != null)
                                {
                                    foreach (var rtSeg in enrt.Routes)
                                    {
                                        ArenaStaticProc.SetObjectValue(rtSeg, _FieldLog.FieldName, _FieldLog.NewValueText);
                                        res = true;

                                    }
                                }
                            }

                        }

                        if (res)
                        {
                            IElement el = sigmaEl.ConvertToIElement() as IElement;

                            ChartElementsManipulator.UpdateSingleElementToDataSet(sigmaEl.Name, sigmaEl.Id.ToString(), el, ref sigmaEl, false);
                            ChartElementsManipulator.SynchronizeMirror(sigmaEl);

                        }

                        var fLog = item.FieldLogList.Where(fl => fl.FieldName.ToLower().Contains("geometry")).FirstOrDefault();

                        #region обновить геометрию (пока только для Airspace)

                        if (fLog != null && item.Feature.PDM_Type == PDM_ENUM.Airspace)
                        {
                            AirspaceType[] arspcTypes = {AirspaceType.TMA, AirspaceType.TMA_P, AirspaceType.ATZ, AirspaceType.ATZ_P,AirspaceType.FIR, AirspaceType.FIR_P, AirspaceType.CTR, AirspaceType.CTR_P, AirspaceType.D,
                                            AirspaceType.P,AirspaceType.R, AirspaceType.UIR, AirspaceType.UIR_P};

                            foreach (var vol in ((Airspace)item.Feature).AirspaceVolumeList)
                            {
                                vol.Geo = fLog.OldGeometry;
                                vol.BrdrGeometry = AranSupport.HelperClass.SetObjectToBlob(vol.Geo, "Border");
                                ChartsHelperClass.UpdateAirspace_ChartGeo(AnnoAirspaceGeo_featClass, vol);

                                //if (arspcTypes.Contains(vol.CodeType) && ci.widthBufer > 0)
                                {
                                    Bagel bgl = buf.Buffer((IPolygon)vol.Geo, 1);
                                    if (bgl.MasterID == null || bgl.MasterID.Length <= 0)
                                    {
                                        string id2 = vol.ID;
                                        bgl.MasterID = id2;
                                    }

                                    if (bgl.BagelCodeId == null || bgl.BagelCodeId.Length <= 0) bgl.BagelCodeId = vol.CodeType.ToString();
                                    if (bgl.BagelCodeClass == null || bgl.BagelCodeClass.Length <= 0) bgl.BagelCodeClass = vol.CodeClass;
                                    if (bgl.BagelTxtName == null || bgl.BagelTxtName.Length <= 0) bgl.BagelTxtName = vol.TxtName.ToString();
                                    if (bgl.BagelLocalType == null || bgl.BagelLocalType.Length <= 0) bgl.BagelLocalType = vol.CodeType.ToString();


                                    IFeatureClass FC = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceB"];
                                    ChartsHelperClass.UpdateBuffer(bgl, FC);
                                }
                            }

                        }

                        #endregion

                        #region обновить геометрию (пока только для RouteSegment)

                        if (fLog != null && item.Feature.PDM_Type == PDM_ENUM.RouteSegment)
                        {

                            ChartsHelperClass.UpdateRouteSegmant_ChartGeo(Anno_RouteGeo_featClass, item);

                        }

                        #endregion


                        #region обновить символизацию SegmentPoint, если это необходимо

                        if (item.FieldLogList != null && item.Feature.PDM_Type == PDM_ENUM.RouteSegment)
                        {
                            foreach (var _log in item.FieldLogList)
                            {
                                string _id = _log.ChangeText.Contains("StartPoint") ? ((RouteSegment)item.Feature).StartPoint.PointChoiceID : ((RouteSegment)item.Feature).EndPoint.PointChoiceID;
                                string _desig = _log.ChangeText.Contains("StartPoint") ? ((RouteSegment)item.Feature).StartPoint.SegmentPointDesignator : ((RouteSegment)item.Feature).EndPoint.SegmentPointDesignator;
                                ChartsHelperClass.UpdateSegmentPoint_ChartGeo(Anno_DesignatedGeo_featClass, _id, _desig, _log.FieldName, _log.NewValueText);
                            }
                        }

                        if (item.FieldLogList != null && item.Feature.PDM_Type == PDM_ENUM.WayPoint)
                        {
                            foreach (var _log in item.FieldLogList)
                            {
                                string _id = ((WayPoint)item.Feature).ID;
                                string _desig = ((WayPoint)item.Feature).Designator;
                                ChartsHelperClass.UpdateSegmentPoint_ChartGeo(Anno_DesignatedGeo_featClass, _id, _desig, _log.FieldName, _log.NewValueText);
                            }
                        }

                        #endregion

                        #region обновить направление HoldingPattern при необходимости
                        var turnDir = item.FieldLogList.Where(fl => fl.FieldName.ToLower().Contains("turndirection")).FirstOrDefault();

                        if (turnDir != null && item.Feature.PDM_Type == PDM_ENUM.HoldingPattern)
                        {
                            ((HoldingPattern)item.Feature).TurnDirection = turnDir.NewValueText.CompareTo("LEFT") == 0 ? DirectionTurnType.LEFT : DirectionTurnType.RIGHT;
                            ChartsHelperClass.UpdateHoldingPattern_Direction(Anno_HoldingGeo_featClass, ((HoldingPattern)item.Feature));

                        }

                        #endregion


                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " " + item.Id);
                res = false;
            }

            return res;

        }

 
        private bool UpdateTextContents(List<List<ANCOR.MapCore.AncorChartElementWord>> _TextContents, FieldLog _FieldLog, coordtype _coordTP = coordtype.OTHER)
        {
            bool res = false;

            bool ContinueFlag = CheckFieldname(_TextContents, _FieldLog.FieldName);

            if (ContinueFlag)
            {
                foreach (var Line in _TextContents)
                {
                    foreach (ANCOR.MapCore.AncorChartElementWord Word in Line)
                    {
                        if (Word.DataSource.Value.ToUpper().CompareTo(_FieldLog.FieldName.ToUpper()) == 0)
                        {
                            if (_FieldLog.FieldName.Contains("Track") || _FieldLog.FieldName.Contains("Course"))
                                Word.TextValue = ChartsHelperClass.MakeText(Convert.ToDouble(_FieldLog.NewValueText), 0, 3);
                            else if (_FieldLog.FieldName.CompareTo("Len") == 0)
                                Word.TextValue = ChartsHelperClass.MakeText(Convert.ToDouble(_FieldLog.NewValueText), 1);
                            else if (_FieldLog.FieldName.CompareTo("ValLen") == 0)
                                Word.TextValue = ChartsHelperClass.MakeText(Convert.ToDouble(_FieldLog.NewValueText), 0);
                            else if (_FieldLog.FieldName.CompareTo("Lat") == 0)
                                Word.TextValue = ArenaStaticProc.LatToDDMMSS(_FieldLog.NewValueText, _coordTP);
                            else if (_FieldLog.FieldName.CompareTo("Lon") == 0)
                                Word.TextValue = ArenaStaticProc.LonToDDMMSS(_FieldLog.NewValueText, _coordTP);
                            else
                                Word.TextValue = _FieldLog.NewValueText;

                            res = true;
                        }
                    }
                }
            }

            return res;
        }

        private bool CheckFieldname(List<List<AncorChartElementWord>> textContents, string _FieldName)
        {
            bool res = false;
            foreach (var Line in textContents)
            {
                foreach (ANCOR.MapCore.AncorChartElementWord Word in Line)
                {
                    if (Word.DataSource.Value.StartsWith("ValDistVerLower") && (Word.TextValue.StartsWith("FL")))
                    {
                        Word.DataSource.Value = "UomValDistVerLower";
                    }
                    else if (Word.DataSource.Value.StartsWith("ValDistVerUpper") && (Word.TextValue.StartsWith("FL")))
                    {
                        Word.DataSource.Value = "UomValDistVerUpper";
                    }
                    else if (Word.DataSource.Value.StartsWith("UomValDistVerLower") && (TerminalChartsUtil.IsNumeric(Word.TextValue)))
                    {
                        Word.DataSource.Value = "ValDistVerLower";
                    }
                    else if (Word.DataSource.Value.StartsWith("UomValDistVerUpper") && (Word.TextValue.StartsWith("FL")))
                    {
                        Word.DataSource.Value = "UomValDistVerUpper";
                    }

                    if (!res)
                        res = Word.DataSource.Value.ToUpper().StartsWith(_FieldName.ToUpper());
                }
            }

            return res;
        }

        private bool CheckFieldname(ChartElement_RouteDesignator sigmaEl1, string _ID_Route)
        {
            bool res = false; ;
            bool flag = false;
            foreach (var ln in sigmaEl1.RouteDesignatorSource)
            {
                foreach (var wrd in ln)
                {
                    res = (wrd.DataSource.Condition.CompareTo(_ID_Route) == 0);
                    flag = res;
                    if (res) break;
                }
                if (flag) break;

            }

            return res;
        }

        private List<object> GetChartElement_List(DetailedItem DetailedItem, List<PDMObject> oldPdmList)
        {
            List<string> pdmId = GetElementID(DetailedItem, oldPdmList);

            List<object> _res = SigmaDataCash.ChartElementList.FindAll(obj => (obj is AbstractChartElement) && IsLinckedGeoIdPresented((AbstractChartElement)obj, DetailedItem.Id, pdmId));

            //if (DetailedItem.Feature.PDM_Type == PDM_ENUM.RouteSegment)
            //{
            //    var rs_el = _res.FindAll(obj => (obj is ChartElement_RouteDesignator) ).FirstOrDefault();

            //    if (rs_el != null)
            //    {

                   
            //        var chartEl = (from obj in SigmaDataCash.ChartElementList
            //                       where (obj != null) && (obj is ChartElement_SimpleText) &&
            //                       ((ChartElement_SimpleText)obj).Tag != null &&
            //                  ((ChartElement_SimpleText)obj).Tag.ToString().CompareTo(((AbstractChartElement)rs_el).Id.ToString()) == 0 &&
            //                  //     ((ChartElement_SimpleText)obj).Name != null &&
            //                  //((ChartElement_SimpleText)obj).Name.CompareTo(cartoEl.Name) == 0 &&
            //                  //((ChartElement_SimpleText)obj).RelatedFeature != null &&
            //                  //((ChartElement_SimpleText)obj).RelatedFeature.CompareTo(cartoEl.RelatedFeature) == 0 &&
            //                  ((ChartElement_SimpleText)obj).LinckedGeoId != null &&
            //                  ((ChartElement_SimpleText)obj).LinckedGeoId.CompareTo(DetailedItem.Feature.ID) == 0
            //                       select obj).FirstOrDefault();

            //        //if (chartEl != null) _res.Add(chartEl);
            //    }

            //}

            return _res;
        }


        private bool IsLinckedGeoIdPresented(AbstractChartElement obj, string linGeoId , List<string> IdList)
        {
            bool res = false;
            foreach (var item in IdList)
            {
                if (obj.LinckedGeoId.Trim().Length <= 0) continue;

                string uid = obj.LinckedGeoId;
                if (obj is ChartElement_RouteDesignator)
                {
                    if (obj.LinckedGeoId.StartsWith(linGeoId))
                    {
                        res = true;
                        break;
                    }
                    else
                    {
                        uid = "";
                        foreach (var line in ((ChartElement_RouteDesignator)obj).RouteDesignatorSource)
                        {

                            foreach (var wrd in line)
                            {
                                uid = uid + wrd.DataSource.Condition + "/";

                            }
                        }
                        foreach (var line in ((ChartElement_RouteDesignator)obj).TextContents)
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
            IFeatureClass geoBuf = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceB"];


            switch (item.Feature.PDM_Type)
            {

                case PDM_ENUM.NavaidSystem:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"]; 
                    break;
                case PDM_ENUM.SegmentPoint:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                    break;
                case PDM_ENUM.Enroute:
                case PDM_ENUM.RouteSegment:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["RouteSegmentCartography"];
                    break;
                case PDM_ENUM.HoldingPattern:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
                    break;
                case PDM_ENUM.Airspace:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                    break;
                case PDM_ENUM.WayPoint:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                    break;
                default:
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
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

            if (item.Feature.PDM_Type == PDM_ENUM.Airspace)
            {
                foreach (var id in _ID_List)
                {
                    string qry = "MasterID = '" + id + "'";
                    IQueryFilter featFilter = new QueryFilterClass();

                    featFilter.WhereClause = qry;

                    IFeatureCursor featCur = geoBuf.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    Marshal.ReleaseComObject(featCur);

                }

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
                            if(!res.Contains(Rt.StartPoint.ID)) res.Add(Rt.StartPoint.ID);
                            break;
                        }

                        if (Rt.EndPoint != null && Rt.EndPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            if (!res.Contains(Rt.EndPoint.ID)) res.Add(Rt.EndPoint.ID);
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
                            if (!res.Contains(Rt.StartPoint.ID)) res.Add(Rt.StartPoint.PointChoiceID);
                            break;
                        }

                        if (Rt.EndPoint != null && Rt.EndPoint.PointChoiceID.StartsWith(item.Id))
                        {
                            if (!res.Contains(Rt.EndPoint.ID)) res.Add(Rt.EndPoint.PointChoiceID);
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

                        if (!res.Contains(Rt.ID)) res.Add(Rt.ID);

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
                       // if (((RouteSegment)item.Feature).RouteFormed.CompareTo(Enrt.TxtDesig) == 0)
                       if (Rt.ID.CompareTo(item.Id) == 0)
                            if (!res.Contains(Rt.ID)) res.Add(Rt.ID);

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

                        if (!res.Contains(vol.ID)) res.Add(vol.ID);

                    }
                }

                #endregion
            }

            if (item.Feature.PDM_Type == PDM.PDM_ENUM.WayPoint)
            {
                #region

                if (Anno_DesignatedGeo_featClass != null)
                {
                    IQueryFilter featFilter = new QueryFilterClass();


                    IFeatureCursor featCur = null;

                    featFilter.WhereClause = "SegmentPointDesignator = '" + ((WayPoint)item.Feature).Designator + "'";
                    featCur = Anno_DesignatedGeo_featClass.Search(featFilter, false);
                    IFeature pFeat = featCur.NextFeature();


                    if (pFeat != null)
                    {
                        int fID = pFeat.Fields.FindField("FeatureGUID");
                        if (fID > 0)
                        {
                            string fgd = pFeat.Value[fID].ToString();
                            if (!res.Contains(fgd)) res.Add(fgd);
                           
                        }
                    }

                    Marshal.ReleaseComObject(featCur);


                   
                }
               

                #endregion
            }


            return res;
        }
        
        #endregion



    }
}
