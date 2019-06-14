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
using System.Threading;

namespace SigmaChart
{
    public class SIDChartClass : AbstaractSigmaChart
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
        private IFeatureClass Anno_HoldingpathGeo_featClass = null;


        List<string> DpnList = null;
        List<string> NavaidList = null;
        List<RunwayDirection> selRwyDir = null;
        AirportHeliport selARP = null;
        UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        List<string> _arp_chanels =null;
        List<PDMObject> _holdingList = null;
        

        public SIDChartClass()
        {
        }


        public override void CreateChart()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            try
            {

                object Missing = Type.Missing;

              
               var acDate= DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
               int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);

                SIDWizardForm frm = new SIDWizardForm(_AiracCircle);


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
                if (Directory.Exists(frm._FolderName + @"\" + frm._ProjectName))  Directory.Delete(frm._FolderName + @"\" + frm._ProjectName, true);

                var destPath2 = System.IO.Directory.CreateDirectory(frm._FolderName + @"\" + frm._ProjectName).FullName;
                

                var tmpName = frm._TemplatetName;
                var rnavFlag = frm._RNAVflag;
                selARP = frm.selectedADHP;
                vertUom = frm.vertUom;
                distUom = frm.distUom;
                _arp_chanels = frm._selectedChanels;
                if (_arp_chanels == null) { _arp_chanels = TerminalChartsUtil.getChanelsList( selARP.CommunicationChanels); }
                int arspBufWidth = frm.arspBufWidth;
                _holdingList = frm._selectedHoldings;
                bool createArspsSgnFlag = frm.arspSign;
                bool showFirstProcDesignator = frm._firstProcDesignator;

                IAOIBookmark extentBookmark = frm._bookmark;
                

                ILayer _Layer = EsriUtils.getLayerByName(this.SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;

                alrtForm.label1.Text = "Preparation";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                TerminalChartsUtil.SetMapScale(this.SigmaHookHelper.ActiveView.FocusMap, frm.selectedSID,this.SigmaApplication);
                this.SigmaHookHelper.ActiveView.FocusMap.MapScale = frm._mapScale;

                Application.DoEvents();

                
                alrtForm.progressBar1.Value++;
                //////////////////////////////////////////////////////////////////////
                // ChartsHelperClass.SaveSourcePDMFiles(destPath2);
                //////////////////////////////////////////////////////////////////////



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

                var selectedRec = frm._ext;


                List<PDMObject> arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Airspace) select element).ToList();
                //var sss= DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.Airspace, arspc_featureList);

                List<PDMObject> selectedProc = frm.selectedSID;

                List<PDMObject> navaidsList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();//DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.NavaidSystem);

                foreach (var rwy in selARP.RunwayList)
                {
                    if (rwy.RunwayDirectionList == null) continue;
                    foreach (var rdn in rwy.RunwayDirectionList)
                    {
                        if (rdn.Related_NavaidSystem == null) continue;
                        foreach (var nvd in rdn.Related_NavaidSystem)
                        {
                            if (nvd.Components == null) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.ILS) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.ILS_DME) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.LOC) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.LOC_DME) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.MLS) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.MLS_DME) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.SDF) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.TLS) continue;
                            if (nvd.CodeNavaidSystemType == NavaidSystemType.DF) continue;

                            if (!navaidsList.Contains(nvd)) navaidsList.Add(nvd);

                        }
                    }
                }
                List<PDMObject> wypntsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.WayPoint).ToList();
                List<PDMObject> GeoBorderList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.GeoBorder).ToList();
                //List<PDMObject> ADHPList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.AirportHeliport).ToList();
                List<AirportHeliport> ADHPList = DataCash.GetAirportlist();
                List<PDMObject> vertStructList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure).ToList();
                List<PDMObject> HoldingPatternFeatureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.HoldingPattern) select element).ToList();


                List<PDMObject> obstacleList = new List<PDMObject>();

                DataCash.AddAssesmentAreaObjectsToObsList(ref obstacleList, selectedProc, frm.VS_Min_Elev > -1? frm.VS_Min_Elev : 0); // замедляет процесс генерации карты!!!! 
                if (frm.VS_Min_Elev > -1)
                {
                    if (obstacleList.Count > 30)
                        obstacleList = DataCash.FilterObstaclesWithinPolygon(selectedRec, frm.VS_Min_Elev, RadiusInMeters: frm.VS_Radius);

                }

                //            if (obstacleList.Count <= 0)
                //            {
                //                obstacleList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                //                                where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.VerticalStructure &&
                //((VerticalStructure)element).Parts != null && (((VerticalStructure)element).Parts.Select(p => p.Elev.HasValue && p.ConvertValueToMeter(p.Elev, p.Elev_UOM.ToString()) > 100).Count() > 0))
                //                                select element).ToList();

                //                if (obstacleList.Count > 30)
                //                    obstacleList = DataCash.FilterObstaclesWithinPolygon(selectedRec, 100);

                //            }

                selRwyDir = TerminalChartsUtil.GetSelectedRunwayDirectionList(frm.selectedSID);

                #region Store using data to pdm file
                
                List<PDMObject> PdmElementsList = new List<PDMObject>();

                if (arspc_featureList != null && arspc_featureList.Count > 0) PdmElementsList.AddRange(arspc_featureList.GetRange(0, arspc_featureList.Count));
                if (wypntsList != null && wypntsList.Count > 0) PdmElementsList.AddRange(wypntsList.GetRange(0, wypntsList.Count));
                if (navaidsList != null && navaidsList.Count > 0) PdmElementsList.AddRange(navaidsList.GetRange(0, navaidsList.Count));
                //if (obstacleList != null && obstacleList.Count > 0) PdmElementsList.AddRange(obstacleList.GetRange(0, obstacleList.Count));
                if (vertStructList != null && vertStructList.Count > 0) PdmElementsList.AddRange(vertStructList.GetRange(0, vertStructList.Count));
                if (selectedProc != null && selectedProc.Count > 0) PdmElementsList.AddRange(selectedProc.GetRange(0, selectedProc.Count));
                if (GeoBorderList != null && GeoBorderList.Count > 0) PdmElementsList.AddRange(GeoBorderList.GetRange(0, GeoBorderList.Count));
                if (ADHPList != null && ADHPList.Count > 0) PdmElementsList.AddRange(ADHPList.GetRange(0, ADHPList.Count));
                //if (_holdingList != null && _holdingList.Count > 0) PdmElementsList.AddRange(_holdingList.GetRange(0, _holdingList.Count));
                if (HoldingPatternFeatureList != null && HoldingPatternFeatureList.Count > 0) PdmElementsList.AddRange(HoldingPatternFeatureList.GetRange(0, HoldingPatternFeatureList.Count));


                PdmElementsList.Add(frm.selectedADHP);
                PdmElementsList.Add(frm.selectedMSA);

                ChartsHelperClass.SaveSourcePDMFiles(destPath2, PdmElementsList);
                PdmElementsList = null;

                #endregion
                
                //this.EffectiveDate = DataCash.GetEfectiveDate();
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
                this.EffectiveDate = start;
                this.ADHP = selARP.Designator !=null ? selARP.Designator : "";
                this.airacCircle = _AiracCircle.ToString();
                this.organization = selARP.OrganisationAuthority != null ? selARP.OrganisationAuthority : "";

                var la = selectedProc.Where(x => x is Procedure && ((Procedure)x).LandingArea != null).ToList();

                if (la!=null && la.Count > 0)
                {
                    this.RunwayDirectionsList = new List<string>();

                    foreach (Procedure proc in la)
                    {
                        foreach (PDMObject item in proc.LandingArea)
                        {
                            if (this.RunwayDirectionsList.IndexOf(item.GetObjectLabel()) < 0) this.RunwayDirectionsList.Add(item.GetObjectLabel());

                        }

                    }
                }
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

                ///////////////////////

                #endregion


                double AnnoScale = this.SigmaHookHelper.ActiveView.FocusMap.MapScale;

                alrtForm.label1.Text = "Initialization";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.SIDChart_Type, projectName, destPath2, tmpName, this.SigmaApplication);
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

                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "sid.sce");
                SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.SIDChart_Type;

                if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }

                alrtForm.label1.Text = "Work space initialization";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

                //////////////////////////////////////////////
                //try
                //{
                //    alrtForm.label1.Text = "Validation";
                //    //alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                //    SigmaTerminal_EnrouteChartExamination sidExam = new SigmaTerminal_EnrouteChartExamination { _projectName = projectName, ExaminationData = new Dictionary<string, List<PDMObject>>(), SigmaChartTypes = (int)SigmaChartTypes.SIDChart_Type };
                //    sidExam.FillExaminationData(arspc_featureList, navaidsList, ADHPList, GeoBorderList, selectedLegs);
                //    sidExam.ChartExamination();
                //    sidExam.ShowExamResults();
                //}
                //catch { }
                ////////////////////////////////////////////

                DpnList = new List<string>();
                NavaidList = new List<string>();

                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.BringToFront();
                Application.DoEvents();


                #region Store chart Info


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

                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

              
                alrtForm.label1.Text = "Creating annotations";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();
                #region creating annotations

                alrtForm.progressBar1.Value++;

                TerminalChartsUtil.SaveVerticalStructureGeo(obstacleList, Anno_ObstacleGeo_featClass, 0, vertUom.ToString(), distUom.ToString(), FocusMap.MapScale);


                alrtForm.progressBar1.Value++;

               #region  убрать тип airspace в первой строке аннотации

               ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)SigmaDataCash.prototype_anno_lst[0];
               if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);

                #endregion


                //====================


                ////var selectedRec = EsriUtils.ToGeo(frm._ext, this.SigmaHookHelper.ActiveView.FocusMap, pSpatialReference);
                //Airspace rect = (Airspace)arspc_featureList[0];
                ////rect.TxtName = "RECT";

                ////IPolygon plg = new PolygonClass();
                ////ISegmentCollection _col = (ISegmentCollection)plg;
                ////_col.SetRectangle((IEnvelope)selectedRec);

                //IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)selectedRec;
                //zAware.ZAware = true;


                //IZ2 z2 = (ESRI.ArcGIS.Geometry.IZ2)plg;
                //z2.SetNonSimpleZs(0.0);

                //IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)selectedRec;
                //mAware.MAware = true;

                //rect.AirspaceVolumeList[0].Geo = selectedRec;


                //===================


                if (arspc_featureList != null && arspc_featureList.Count > 0)
                    TerminalChartsUtil.CreateAirspace_ChartElements(arspc_featureList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale, vertUom.ToString(), 
                        distUom.ToString(), arspBufWidth, createArspsSgnFlag, _extent:selectedRec);


                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.CreateProcedureLegs_ChartElements(selectedLegs, rnavFlag, FocusMap, pSpatialReference, selARP.MagneticVariation.Value, 
                    Anno_LegGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, AnnoFacilitymakeUpGeo_featClass, Anno_HoldingGeo_featClass, Anno_HoldingpathGeo_featClass,
                    vertUom, distUom, ref NavaidList, ref DpnList, ShowOnlyFirstProcDesignator : showFirstProcDesignator);


                TerminalChartsUtil.CreateHeightAnnotation(selectedProc, FocusMap, vertUom.ToString(), distUom.ToString());

               alrtForm.progressBar1.Value++;
                if (frm._AllVOR_DMEflag)
                {

                    TerminalChartsUtil.CreateNavaids_ChartElements(navaidsList, FocusMap.MapScale, ref NavaidList, Anno_NavaidGeo_featClass, vertUom.ToString());

                }


                if (_holdingList != null && _holdingList.Count > 0)
                {
                    List<string> angleIndicationDictionary = new List<string>();

                    foreach (var hld in _holdingList)
                    {
                        TerminalChartsUtil.CreateHoldingPatterns_ChartElement((HoldingPattern)hld, null, rnavFlag, FocusMap, selARP.MagneticVariation.Value,  Anno_HoldingGeo_featClass, vertUom, distUom, Anno_HoldingpathGeo_featClass);


                        if (((HoldingPattern)hld).HoldingPoint != null)
                        {
                            SegmentPoint hldPnt = ((HoldingPattern)hld).HoldingPoint;
                            if (hldPnt.Geo == null) hldPnt.RebuildGeo();



                            if (hldPnt.Geo != null)
                            {
                                #region PDM.PointChoice.DesignatedPoint

                                if (hldPnt.PointChoice == PDM.PointChoice.DesignatedPoint)
                                {
                                    if (DpnList.IndexOf(hldPnt.PointChoiceID) < 0)
                                    {
                                        DpnList.Add(hldPnt.PointChoiceID);

                                        ChartElement_SigmaCollout_Designatedpoint chrtEl_DesigPoint = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                                        IElement el = ChartsHelperClass.CreateSegmentPointAnno(hldPnt, chrtEl_DesigPoint);
                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, hldPnt.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, hldPnt, hldPnt.Geo);
                                    }
                                }


                                #endregion

                                #region PDM.PointChoice.Navaid

                                if (hldPnt.PointChoice == PDM.PointChoice.Navaid)
                                {
                                    NavaidSystem _NAVAID = null;


                                    _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                             where (element != null)
                                                                 && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                                 && (element.ID.StartsWith(hldPnt.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(hldPnt.PointChoiceID.Trim()))
                                                             select element).FirstOrDefault();

                                    if (_NAVAID != null && (NavaidList.IndexOf(_NAVAID.ID) < 0))
                                    {
                                        NavaidList.Add(_NAVAID.ID);

                                        ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                                        IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString());
                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                                        ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)_NAVAID);

                                    }


                                }





                                #endregion
                            }

                            if (((HoldingPattern)hld).EndPoint != null)
                            {
                                if (((HoldingPattern)hld).EndPoint != null)
                                {
                                    #region AngleIndication

                                    if (((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.AngleIndication != null)
                                    {
                                        TerminalChartsUtil.CreateStoreAngleIndication(((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, selARP.MagneticVariation.Value,
                                             ((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.ID, ((HoldingPattern)hld).EndPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidList);


                                    }

                                    #endregion

                                    #region DistanceIndication

                                    if (((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                                    {

                                        TerminalChartsUtil.CreateStoreDistanceIndication(((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, selARP.MagneticVariation.Value,
                                            ((HoldingPattern)hld).EndPoint, vertUom.ToString(),distUom.ToString(),ref angleIndicationDictionary, ref NavaidList);

                                    }

                                    #endregion

                                }
                            }

                        }


                    }
                }


                TerminalChartsUtil.CreateGeoBorderAnno(GeoBorderList, FocusMap);



                TerminalChartsUtil.CreateAirportAnno(ADHPList, selARP.ID, Anno_Airport_Geo_featClass, AnnoRWYGeo_featClass, FocusMap, pSpatialReference, frm.allRwyFlag, selArpDesignator: this.ADHP);


                #endregion

                alrtForm.label1.Text = "Updating grahics elements";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();

                #region Update Graphics Elements

                #region MSA

                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.CreateMsa(frm.selectedMSA, SigmaHookHelper, selARP, vertUom, distUom);

                #endregion

                #region North Arrow

                alrtForm.progressBar1.Value++;
                TerminalChartsUtil.CreateNorthArrow(SigmaHookHelper, selARP);

                #endregion

                #region Update dinamic text

                alrtForm.progressBar1.Value++;

                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, frm.selectedSID, selARP, selRwyDir, vertUom.ToString(), distUom.ToString(), (int)RNPFlag, _arp_chanels);


                #endregion

                #region Fill tables

                //TerminalChartsUtil.CreateEOSIdtable(SigmaHookHelper, selectedProc, FocusMap, pSpatialReference, vertUom, distUom);

                #endregion

                #endregion



                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                alrtForm.label1.Text = "Finalisation";
                alrtForm.progressBar1.Value++;
                alrtForm.BringToFront();



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

                if (MessageBox.Show("Do you want to create Procuderes tabulation document?", "SIGMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ProcedureTabulation cdnTbl = new ProcedureTabulation
                    {
                        MagVar = selARP.MagneticVariation.Value,
                        TemplateName = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "TABULAR DESCRIPTION.xls"),
                        NewCodingTableName = System.IO.Path.Combine(destPath2, System.IO.Path.GetFileNameWithoutExtension(projectName)) + ".xls",
                        AltitudeUOM = this.vertUom.ToString(),
                        DistanceUOM = this.distUom.ToString(),

                    };


                    var listprocOrderedByName = selectedProc.OrderBy(name => name.GetObjectLabel()).ToList();
                    cdnTbl.CreateCodingTable(listprocOrderedByName);
                }

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
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);

            Application.DoEvents();


            try
            {
                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ProcedureLegsCartography")) Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC")) AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography")) Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VerticalStructurePointCartography"))
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayCartography")) AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("FacilityMakeUpCartography")) AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography")) Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingPath")) Anno_HoldingpathGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingPath"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.STARChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_HoldingpathGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingPath"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GeoBorderCartography"))
                    Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.SIDChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
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
