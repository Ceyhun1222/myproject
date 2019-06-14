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


    public class IAPChartClass : AbstaractSigmaChart
    {

        private IMap FocusMap = null;
        private ISpatialReference pSpatialReference = null;


        private IFeatureClass Anno_LegGeo_featClass = null;
        private IFeatureClass Anno_DesignatedGeo_featClass = null;
        private IFeatureClass Anno_NavaidGeo_featClass = null;
        private IFeatureClass Anno_ObstacleGeo_featClass = null;
        private IFeatureClass AnnoAirspaceGeo_featClass = null;
        private IFeatureClass AnnoRWYGeo_featClass = null;
        private IFeatureClass AnnoGPGeo_featClass = null;
        private IFeatureClass AnnoFacilitymakeUpGeo_featClass = null;
        private IFeatureClass Anno_HoldingGeo_featClass = null;
        private IFeatureClass Anno_GeoBorder_Geo_featClass = null; 
        private IFeatureClass Anno_Airport_Geo_featClass = null;
        private IFeatureClass Anno_DecorPointCartography_featClass = null;
        private IFeatureClass Anno_DecorLineCartography_featClass = null;
        private IFeatureClass Anno_DecorPolygonCartography_featClass = null;
        private IFeatureClass Anno_ILSCartography_featClass = null;
        private IFeatureClass Anno_HoldingpathGeo_featClass = null;

        List<string> DpnList = null;
        List<string> NavaidList = null;
        List<RunwayDirection> selRwyDir = null;
        AirportHeliport selARP = null;
        UOM_DIST_VERT vertUom = UOM_DIST_VERT.FT;
        UOM_DIST_HORZ distUom = UOM_DIST_HORZ.NM;
        List<string> _arp_chanels = null;
        List<PDMObject> _holdingList = null;

        public IAPChartClass()
        {
        }


        public override void CreateChart()
        {
            try
            {

                object Missing = Type.Missing;

                //int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(DataCash.ProjectEnvironment.Data.PdmObjectList[0].ActualDate);
                var acDate = DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
                int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);
                IAPWizardForm frm = new IAPWizardForm(_AiracCircle);


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

                int arspBufWidth = frm.arspBufWidth;
                bool createArspsSgnFlag = frm.arspSign;
                _holdingList = frm._selectedHoldings;
                bool showFirstProcDesignator = frm._firstProcDesignator;

                _arp_chanels = frm._selectedChanels;
                if (_arp_chanels == null) { _arp_chanels = TerminalChartsUtil.getChanelsList(selARP.CommunicationChanels); }
                IAOIBookmark extentBookmark = frm._bookmark;

                ILayer _Layer = EsriUtils.getLayerByName(this.SigmaHookHelper.ActiveView.FocusMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;

                pSpatialReference = (fc as IGeoDataset).SpatialReference;

                alrtForm.label1.Text = "Preparation";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                TerminalChartsUtil.SetMapScale(this.SigmaHookHelper.ActiveView.FocusMap, frm.selectedIAP, this.SigmaApplication);
                this.SigmaHookHelper.ActiveView.FocusMap.MapScale = frm._mapScale;
                Application.DoEvents();

                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                //////////////////////////////////////////////////////////////////////
                // ChartsHelperClass.SaveSourcePDMFiles(destPath2);
                //////////////////////////////////////////////////////////////////////


                #region Validator




                //ChartValidator.ProcedureValidator vldr = new ChartValidator.ProcedureValidator(this.SigmaApplication, ChartValidator.ChartType.IAP);

                //string er;
                //byte result = vldr.Check(DataCash.ProjectEnvironment.Data.PdmObjectList, out er);

                //if (result == 2)
                //{
                //    MessageBox.Show(er);
                //}
                //else if (result == 4) return;



                #endregion


                alrtForm.label1.Text = "Data quering";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region Data Query

                var selectedRec = frm._ext;

                // List<PDMObject> obstacleList = DataCash.GetObstaclesWithinPolygon(selectedRec);
                //List<PDMObject> obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure && ((PDM.VerticalStructure)pdm).ObstacleAreaType != null && ((PDM.VerticalStructure)pdm).ObstacleAreaType.Contains(CodeObstacleAreaType.AREA2)).ToList();
                //if (obstacleList == null || obstacleList.Count == 0) obstacleList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure).ToList();

                List<PDMObject> selectedProc = frm.selectedIAP;

                List<PDMObject> arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Airspace) select element).ToList();

                //List<PDMObject> navaidsList = DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.NavaidSystem);
                List<PDMObject> navaidsList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) select element).ToList();//DataCash.GetObjectsWithinPolygon(selectedRec, PDM_ENUM.NavaidSystem);

                List<PDMObject> wypntsList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.WayPoint).ToList();
                List<PDMObject> GeoBorderList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.GeoBorder).ToList();
                //List<PDMObject> ADHPList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.AirportHeliport).ToList();
                List<AirportHeliport> ADHPList = DataCash.GetAirportlist();
                //List<PDMObject> obstacleList = DataCash.GetObstaclesWithinPolygon(selectedRec);
                List<PDMObject> vertStructList = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(pdm => pdm is PDM.VerticalStructure).ToList();
                List<PDMObject> HoldingPatternFeatureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.HoldingPattern) select element).ToList();


                List<PDMObject> obstacleList = new List<PDMObject>();

                DataCash.AddAssesmentAreaObjectsToObsList(ref obstacleList, selectedProc, frm.VS_Min_Elev > -1 ? frm.VS_Min_Elev : 0); // замедляет процесс генерации карты!!!!
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

                selRwyDir = TerminalChartsUtil.GetSelectedRunwayDirectionList(frm.selectedIAP);
                Runway pdmRunway = selRwyDir != null ? DataCash.GetRWY(selRwyDir[0].ID_AirportHeliport, selRwyDir[0].ID_Runway) : null;


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


                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                DataCash.GetEfectiveDate(_AiracCircle, ref start, ref end);
                this.EffectiveDate = start;
                this.ADHP = selARP.Designator != null ? selARP.Designator : "";
                this.airacCircle = _AiracCircle.ToString();
                this.organization = selARP.OrganisationAuthority != null ? selARP.OrganisationAuthority : "";

                var la = selectedProc.Where(x => x is Procedure && ((Procedure)x).LandingArea != null).ToList();

                if (la != null && la.Count > 0)
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
                Dictionary<string, ProcedureLeg> dicLeg = new Dictionary<string, ProcedureLeg>();
                string uId = "";
                foreach (Procedure proc in selectedProc)
                {
                    foreach (ProcedureTransitions trans in proc.Transitions)
                    {
                        if (trans.RouteType != ProcedurePhaseType.APPROACH) continue;
                        foreach (ProcedureLeg leg in trans.Legs)
                        {
                            leg.ProcedureIdentifier = proc.ProcedureIdentifier;

                            if (leg.StartPoint != null && leg.EndPoint != null)
                                uId = leg.StartPoint.PointChoiceID + ":" + leg.EndPoint.PointChoiceID;
                            else if (leg.StartPoint != null && leg.EndPoint == null)
                                uId = leg.StartPoint.PointChoiceID + ":" + leg.ID;
                            else if (leg.StartPoint == null && leg.EndPoint != null)
                                uId = leg.ID + ":" + leg.EndPoint.PointChoiceID;
                            else if (leg.StartPoint == null && leg.EndPoint == null)
                                uId = leg.ID + ":" + leg.ID;

                            if (!dicLeg.ContainsKey(uId))
                            {
                                dicLeg.Add(uId, leg);
                            }
                            else
                            {
                                dicLeg[uId].ProcedureIdentifier = dicLeg[uId].ProcedureIdentifier + "/" + proc.ProcedureIdentifier;
                            }
                        }
                    }

                }

                foreach (Procedure proc in selectedProc)
                {
                    foreach (ProcedureTransitions trans in proc.Transitions)
                    {
                        if (trans.RouteType == ProcedurePhaseType.APPROACH) continue;
                        foreach (ProcedureLeg leg in trans.Legs)
                        {
                            leg.ProcedureIdentifier = proc.ProcedureIdentifier;
                            leg.Lat = trans.RouteType.ToString();
                            if (leg.StartPoint != null && leg.EndPoint != null)
                                uId = leg.StartPoint.PointChoiceID + ":" + leg.EndPoint.PointChoiceID;
                            else if (leg.StartPoint != null && leg.EndPoint == null)
                                uId = leg.StartPoint.PointChoiceID + ":" + leg.ID;
                            else if (leg.StartPoint == null && leg.EndPoint != null)
                                uId = leg.ID + ":" + leg.EndPoint.PointChoiceID;
                            else if (leg.StartPoint == null && leg.EndPoint == null)
                                uId = leg.ID + ":" + leg.ID;

                            if (!dicLeg.ContainsKey(uId))
                            {
                                dicLeg.Add(uId, leg);
                            }
                            else
                            {
                                dicLeg[uId].ProcedureIdentifier = dicLeg[uId].ProcedureIdentifier + "/" + proc.ProcedureIdentifier;
                            }
                        }
                    }

                }

                List<ProcedureLeg> selectedLegs = new List<ProcedureLeg>();
                selectedLegs.AddRange(dicLeg.Values);


                ///////////////////////

                #endregion


                double AnnoScale = this.SigmaHookHelper.ActiveView.FocusMap.MapScale;

                alrtForm.label1.Text = "Initialization";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.IAPChart_Type, projectName, destPath2, tmpName, this.SigmaApplication);
                Application.DoEvents();
                if (FocusMap == null) return;

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

                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "iap.sce");
                SigmaDataCash.SigmaChartType = (int)SigmaChartTypes.IAPChart_Type;

                if (!File.Exists(pathToTemplateFileXML)) { MessageBox.Show("File not exist!"); return; }


                alrtForm.label1.Text = "Work space initialization";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                SigmaDataCash.environmentWorkspaceEdit = this.InitEnvironment_Workspace(pathToTemplateFileXML, FocusMap);

                //////////////////////////////////////////////
                //try
                //{
                //    alrtForm.label1.Text = "Validation";
                //    alrtForm.progressBar1.Value++; alrtForm.BringToFront();
                //    SigmaTerminal_EnrouteChartExamination sidExam = new SigmaTerminal_EnrouteChartExamination { _projectName = projectName, ExaminationData = new Dictionary<string, List<PDMObject>>(), SigmaChartTypes = (int)SigmaChartTypes.IAPChart_Type };
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



                //#region creating annotations

                #region VerticalStructureGeo

                List<PDMObject> assesmentObs = new List<PDMObject>();

                var _FinalLegList = (from element in selectedLegs
                                     where (element != null)
                                            && (element is FinalLeg)
                                         && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                     orderby element.SeqNumberARINC
                                     select element).ToList();

                //if (_FinalLegList != null && _FinalLegList.Count > 0)
                //{
                //    foreach (var fl in _FinalLegList)
                //    {

                //        if (((FinalLeg)fl).AssessmentArea != null && ((FinalLeg)fl).AssessmentArea.Count > 0)
                //        {
                //            foreach (var assAreae in ((FinalLeg)fl).AssessmentArea)
                //            {
                //                if (((ObstacleAssessmentArea)assAreae).SignificantObstacle != null && ((ObstacleAssessmentArea)assAreae).SignificantObstacle.Count > 0)
                //                {
                //                    foreach (var obs in ((ObstacleAssessmentArea)assAreae).SignificantObstacle)
                //                    {
                //                        if (((Obstruction)obs).VerticalStructure != null) assesmentObs.Add(((Obstruction)obs).VerticalStructure);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}


                //var _LegList = (from element in selectedLegs
                //                     where (element != null)
                //                         && (element.AssessmentArea !=null)
                //                     orderby element.SeqNumberARINC
                //                     select element).ToList();

                //if (_LegList != null && _LegList.Count > 0)
                //{
                //    foreach (var fl in _LegList)
                //    {

                //        if (((ProcedureLeg)fl).AssessmentArea != null && ((ProcedureLeg)fl).AssessmentArea.Count > 0)
                //        {
                //            foreach (var assAreae in ((ProcedureLeg)fl).AssessmentArea)
                //            {
                //                if (((ObstacleAssessmentArea)assAreae).SignificantObstacle != null && ((ObstacleAssessmentArea)assAreae).SignificantObstacle.Count > 0)
                //                {
                //                    foreach (var obs in ((ObstacleAssessmentArea)assAreae).SignificantObstacle)
                //                    {
                //                        if (((Obstruction)obs).VerticalStructure != null) assesmentObs.Add(((Obstruction)obs).VerticalStructure);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                obstacleList = new List<PDMObject>();

                DataCash.AddAssesmentAreaObjectsToObsList(ref obstacleList, selectedProc, frm.VS_Min_Elev);
                if (obstacleList.Count > 30)
                    obstacleList = DataCash.FilterObstaclesWithinPolygon(selectedRec, frm.VS_Min_Elev, RadiusInMeters: frm.VS_Radius);
                TerminalChartsUtil.SaveVerticalStructureGeo(obstacleList, Anno_ObstacleGeo_featClass, 0, vertUom.ToString(), distUom.ToString(), FocusMap.MapScale);


                #endregion


                #region save Glide Path Geo

                //if (selRwyDir != null && selRwyDir.Related_NavaidSystem != null && selRwyDir.Related_NavaidSystem.Count > 0 && !rnavFlag)
                //{
                //    foreach (var nvd in selRwyDir.Related_NavaidSystem)
                //    {
                //        if (nvd.CodeNavaidSystemType == NavaidSystemType.ILS || nvd.CodeNavaidSystemType == NavaidSystemType.ILS_DME)
                //        {
                //            foreach (var cmpnt in nvd.Components)
                //            {
                //                if (cmpnt.PDM_Type == PDM_ENUM.GlidePath)
                //                {
                //                    ChartsHelperClass.SaveGP_ChartGeo(AnnoGPGeo_featClass, cmpnt, selARP, selRwyDir);
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

                #endregion


                #region  убрать тип airspace в первой строке аннотации

                ChartElement_SimpleText airspacePrototype = (ChartElement_SimpleText)SigmaDataCash.prototype_anno_lst[0];
                if (airspacePrototype.TextContents[0].Count > 1) airspacePrototype.TextContents[0].RemoveRange(1, 1);

                #endregion

                if (arspc_featureList != null && arspc_featureList.Count > 0)
                    TerminalChartsUtil.CreateAirspace_ChartElements(arspc_featureList, SigmaHookHelper, AnnoAirspaceGeo_featClass, FocusMap.MapScale, vertUom.ToString(),
                        distUom.ToString(), arspBufWidth, createArspsSgnFlag, _extent: selectedRec);

                TerminalChartsUtil.CreateProcedureLegs_ChartElements(selectedLegs, rnavFlag, FocusMap, pSpatialReference, selARP.MagneticVariation.Value, 
                    Anno_LegGeo_featClass, Anno_DesignatedGeo_featClass, Anno_NavaidGeo_featClass, AnnoFacilitymakeUpGeo_featClass, Anno_HoldingGeo_featClass, Anno_HoldingpathGeo_featClass,
                     vertUom, distUom, ref NavaidList, ref DpnList, ShowOnlyFirstProcDesignator : showFirstProcDesignator);

                TerminalChartsUtil.CreateHeightAnnotation(selectedProc, FocusMap, vertUom.ToString(), distUom.ToString());


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
                                            ((HoldingPattern)hld).EndPoint, vertUom.ToString(), distUom.ToString(), ref angleIndicationDictionary, ref NavaidList);

                                    }

                                    #endregion

                                }
                            }

                        }


                    }
                }


                TerminalChartsUtil.CreateGeoBorderAnno(GeoBorderList, FocusMap);

                TerminalChartsUtil.CreateAirportAnno(ADHPList, selARP.ID, Anno_Airport_Geo_featClass, AnnoRWYGeo_featClass, FocusMap, pSpatialReference, frm.allRwyFlag, selArpDesignator: this.ADHP);

                //#endregion

                alrtForm.label1.Text = "Updating grahics elements";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                #region Update Graphics Elements

                #region MSA

                TerminalChartsUtil.CreateMsa(frm.selectedMSA, SigmaHookHelper, selARP, vertUom, distUom);

                #endregion

                #region North Arrow

                TerminalChartsUtil.CreateNorthArrow(SigmaHookHelper, selARP);

                #endregion

                #region Update dinamic text
                List<RunwayDirection> rwyList = new List<RunwayDirection>();
                rwyList.Add(selRwyDir[0]);

                TerminalChartsUtil.UpdateDinamicLabels(SigmaHookHelper, this.EffectiveDate, _AiracCircle, frm.selectedIAP, selARP, rwyList, vertUom.ToString(), distUom.ToString(), 0, _arp_chanels);

                #endregion

                #endregion

                alrtForm.label1.Text = "Creating procedures profile";
                alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                string selectedProcInstruction = ((InstrumentApproachProcedure)selectedProc[0]).MissedInstruction;
                //double OCA = 773 * 0.3048;


                /// ProfileTable Update
                if (TerminalChartsUtil.ProfiletableAnalizator((InstrumentApproachProcedure)selectedProc[0])) 
                {
                    double _OCA = -100;
                    int FLC = 0;
                    if (_FinalLegList != null && _FinalLegList.Count > 0)
                    {
                        FLC = _FinalLegList.Count; 
                        foreach (FinalLeg fl in _FinalLegList)
                        {
                            if (fl.Condition_Minima != null && fl.Condition_Minima.Count >0)
                            {

                                var _OCA_LIST = (from element in fl.Condition_Minima
                                            where (element != null)
                                                            && (element is ApproachCondition)
                                                         && (element.MinAltitudeCode == CodeMinimumAltitude.OCA)
                                                     orderby element.MinAltitude
                                            select element).ToList();

                                if (_OCA_LIST != null && _OCA_LIST.Count > 0)
                                {
                                    _OCA =fl.ConvertValueToFeet(_OCA_LIST[_OCA_LIST.Count - 1].MinAltitude.Value, _OCA_LIST[_OCA_LIST.Count - 1].MinAltitudeUOM.ToString());

                                    break;
                                }
                                
                            }
                        }
                    }

                        //((InstrumentApproachProcedure)selectedProc[0]).Profile = TerminalChartsUtil.UpdateProfile(selRwyDir[0], (InstrumentApproachProcedure)selectedProc[0], _OCA, FLC, FocusMap,pSpatialReference, vertUom, distUom);
                }


                if (((InstrumentApproachProcedure)selectedProc[0]).Profile != null)
                {
                    TerminalChartsUtil.CreateProfile(SigmaHookHelper, selectedProc[0].ID, ((InstrumentApproachProcedure)selectedProc[0]).Profile, selectedProcInstruction, selARP, selRwyDir[0], pdmRunway, selectedLegs, FocusMap,
                        pSpatialReference, vertUom, distUom, rnavFlag);
                }

                ////////////////////////////////////////////




                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);


                alrtForm.label1.Text = "Finalisation";
                alrtForm.progressBar1.Value = alrtForm.progressBar1.Maximum; alrtForm.BringToFront();

                ChartsHelperClass.ChartsFinalisation(this.SigmaApplication, FocusMap, SigmaDataCash.SigmaChartType, projectName, "ProcedureLegsCartography", extentBookmark);

 

                ChartElementsManipulator.CreateSigmaLog(destPath2 );
                EsriWorkEnvironment.EsriUtils.CreateJPEG_FromActiveView(this.SigmaHookHelper.ActiveView, destPath2 + @"\ContentImage.jpg");

                ((IGraphicsContainerSelect)SigmaHookHelper.ActiveView.GraphicsContainer).UnselectAllElements();
                //SigmaHookHelper.ActiveView.Refresh();
                

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
                        TemplateName = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "TABULAR DESCRIPTION.xls"),
                        NewCodingTableName = System.IO.Path.Combine(destPath2, System.IO.Path.GetFileNameWithoutExtension(projectName)) + ".xls",
                        AltitudeUOM = this.vertUom.ToString(),
                        DistanceUOM = this.distUom.ToString(),
                    };


                    var listprocOrderedByName = selectedProc.OrderBy(name => name.GetObjectLabel()).ToList();
                    cdnTbl.CreateCodingTable_IAC(listprocOrderedByName);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }
        }

        private double GetOCA(List<PDMObject> list)
        {
            double OCA = 0;
            try
            {
                PDM.Procedure procIap = (PDM.Procedure)list[0];

                foreach (var trans in procIap.Transitions)
                {
                    bool flag = false;
                    foreach (var finalLeg in trans.Legs)
                    {
                        if (finalLeg is FinalLeg)
                        {
                            OCA = finalLeg.LowerLimitAltitude.HasValue ? finalLeg.ConvertValueToMeter(finalLeg.LowerLimitAltitude.Value, finalLeg.LowerLimitAltitudeUOM.ToString()) : 0;


                            if (finalLeg is FinalLeg && ((FinalLeg)finalLeg).Condition_Minima != null && ((FinalLeg)finalLeg).Condition_Minima.Count >0)
                            {
                                OCA = finalLeg.ConvertValueToMeter(((FinalLeg)finalLeg).Condition_Minima[0].MinAltitude.Value, ((FinalLeg)finalLeg).Condition_Minima[0].MinAltitudeUOM.ToString());
                            }

                            flag = true;
                            break;
                        }
                        if (flag) break;
                    }
                }
            }
            catch
            {
                OCA = 0;
            }
            
            return OCA;
            
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
                ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);

            Application.DoEvents();


            try
            {
                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ProcedureLegsCartography"))
                    Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_LegGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirspaceC")) 
                    AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoAirspaceGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceC"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DesignatedPointCartography"))
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("NavaidsCartography"))
                    Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("VerticalStructurePointCartography"))
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("RunwayCartography"))
                    AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoRWYGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("FacilityMakeUpCartography"))
                    AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("GlidePathCartography")) 
                    AnnoGPGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GlidePathCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    AnnoGPGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GlidePathCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("HoldingCartography"))
                    Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
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
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_GeoBorder_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["GeoBorderCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("AirportCartography"))
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];//null;
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_Airport_Geo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPointCartography"))
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPointCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPointCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorLineCartography"))
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorLineCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorLineCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("DecorPolygonCartography"))
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_DecorPolygonCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DecorPolygonCartography"];
                }

                if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("ProcedureLegsAnnoILSCartography"))
                    Anno_ILSCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsAnnoILSCartography"];
                else
                {
                    ChartElementsManipulator.LoadAnnotation_LinkedGeomFeatureClasses((int)SigmaChartTypes.IAPChart_Type, workspaceEdit, (int)ActiveMap.MapScale);
                    Anno_ILSCartography_featClass = SigmaDataCash.AnnotationLinkedGeometryList["ProcedureLegsAnnoILSCartography"];
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
