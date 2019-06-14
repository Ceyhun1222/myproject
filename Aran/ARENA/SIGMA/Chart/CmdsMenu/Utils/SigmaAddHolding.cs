using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ArenaStatic;
using EsriWorkEnvironment;
using ARENA;
using SigmaChart.CmdsMenu.TemplatesManagerMenu;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using PDM;
using ESRI.ArcGIS.Geometry;
using AranSupport;
using SigmaChart.CmdsMenu;
using ANCOR.MapElements;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("19426696-dce0-49f9-8a79-ab4d6c703705")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaHoldingPattern")]
    public sealed class SigmaHoldingPattern : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IApplication m_application;

        public SigmaHoldingPattern()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create Holding Pattern Annotation";  //localizable text 
            base.m_message = "Add new Holding Pattern Annotation";  //localizable text
            base.m_toolTip = "Add new Holding Pattern Annotation";  //localizable text
            base.m_name = "CreateHoldingtAnno";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.Sigma;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;

                m_application = hook as IApplication;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            if (DataCash.ProjectEnvironment == null || DataCash.ProjectEnvironment.Data == null || DataCash.ProjectEnvironment.Data.PdmObjectList == null) return;

            List<PDMObject> _featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                where (element != null) && (element.PDM_Type == PDM_ENUM.HoldingPattern) &&
                                (((HoldingPattern)element).HoldingPoint != null)
                                select element).ToList();

            if (_featureList == null) _featureList = new List<PDMObject>();

            var _featureListProc = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                where (element != null) && (element is Procedure) &&
                                (((Procedure)element).Transitions != null) 
                                select element).ToList();

            foreach (Procedure prc in _featureListProc)
            {
                foreach (ProcedureTransitions trans in prc.Transitions)
                {
                    foreach (var lg in trans.Legs)
                    {
                        if (lg.HoldingUse != null && lg.HoldingUse.HoldingPoint !=null)
                        {
                            PDMObject _hld = (from element in _featureList
                                                    where (element != null) && (element.ID.StartsWith(lg.HoldingUse.ID))                                                            
                                                            select element).FirstOrDefault();
                            if (_hld == null) _featureList.Add(lg.HoldingUse);
                        }
                    }
                }
            }

            if (_featureList != null && _featureList.Count > 0)
            {

                try
                {

                    HoldingsListForm inptFrm = new HoldingsListForm(_featureList);
                    inptFrm.TopMost = true;
                    var dr = inptFrm.ShowDialog();
                    if (dr == DialogResult.Cancel) return;



                    string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
                    chartInfo ci = EsriUtils.GetChartIno(FN);


                    List<PDMObject> selected_featureList = new List<PDMObject>();
                    IFeatureClass Anno_HoldingpathGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingPath"];
                    IFeatureClass Anno_featClass_DPN = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["DesignatedPointAnno"];
                    IFeatureClass Anno_featClass_Crs = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["ProcedureLegsAnnoCourseCartography"];
                    IFeatureClass Anno_HoldingGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["HoldingCartography"];
                    IFeatureClass Anno_DesignatedGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["DesignatedPointCartography"];
                    IFeatureClass Anno_NavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                    IFeatureClass AnnoFacilitymakeUpGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["FacilityMakeUpCartography"];
                    
                    string qryHoldGeo = "";
                    string qryHoldPnt = "";
                    bool CreateHldPntAnno = true;
                    IQueryFilter featFilter = new QueryFilterClass();

                    AlertForm alrtForm = new AlertForm();

                    alrtForm.FormBorderStyle = FormBorderStyle.None;


                    alrtForm.Opacity = 0.5;
                    alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

                    if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                    alrtForm.progressBar1.Visible = true;
                    alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                    alrtForm.progressBar1.Maximum = inptFrm.SelectedHoldingsList.Count;
                    alrtForm.progressBar1.Value = 0;

                    alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                    alrtForm.label1.Text = "Start process";
                    alrtForm.label1.Visible = true;

                    for (int i = 0; i < inptFrm.SelectedHoldingsList.Count; i++)
                    {
                        alrtForm.label1.Text = inptFrm.SelectedHoldingsList[i].GetObjectLabel();
                        Application.DoEvents();

                        selected_featureList.Add(inptFrm.SelectedHoldingsList[i]);

                        qryHoldGeo = qryHoldGeo + "'" + ((HoldingPattern)inptFrm.SelectedHoldingsList[i]).ID + "',";
                        qryHoldPnt = qryHoldPnt + "'" + ((HoldingPattern)inptFrm.SelectedHoldingsList[i]).HoldingPoint.PointChoiceID + "',";
                        alrtForm.progressBar1.Value++; alrtForm.BringToFront();

                    }


                    inptFrm.Dispose();

                    #region Delete from Geo_featClass

                    string qry1 = "FeatureGUID IN (" + qryHoldGeo + "'00000')";
                    featFilter.WhereClause = qry1;

                    IFeatureCursor featCur = Anno_HoldingpathGeo_featClass.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    #region Delete from Anno_featClass

                    string qry2 = "PdmUID IN (" + qryHoldPnt + "'00000')";
                    featFilter.WhereClause = qry2;

                    featCur = Anno_featClass_DPN.Search(featFilter, false);

                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        //_Feature.Delete();
                        CreateHldPntAnno = false;
                        break;
                    }

                    featCur = Anno_featClass_Crs.Search(featFilter, false);

                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }


                    #endregion

                    Marshal.ReleaseComObject(featCur);



                    alrtForm.label1.Text = "Saving.....";
                    alrtForm.BringToFront();


                    if (selected_featureList != null && selected_featureList.Count > 0)
                    {
                        UOM_DIST_VERT vertUom = ci.uomVert.CompareTo("FT") == 0 ? UOM_DIST_VERT.FT : UOM_DIST_VERT.M;
                        UOM_DIST_HORZ distUom = ci.uomDist.CompareTo("NM") == 0 ? UOM_DIST_HORZ.NM : UOM_DIST_HORZ.KM;
                        List<string> DpnList = new List<string>();
                        List<string> NavaidList = new List<string>();

                        List<string> angleIndicationDictionary = new List<string>();

                        foreach (var hld in selected_featureList)
                        {


                            TerminalChartsUtil.CreateHoldingPatterns_ChartElement((HoldingPattern)hld, null, true, m_hookHelper.ActiveView.FocusMap, 0,  Anno_HoldingGeo_featClass,
                                vertUom, distUom, Anno_HoldingpathGeo_featClass, CreateHldPntAnno);


                            if (((HoldingPattern)hld).HoldingPoint != null && CreateHldPntAnno)
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
                                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, hldPnt.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, m_hookHelper.ActiveView.FocusMap.MapScale);
                                            ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, hldPnt, hldPnt.Geo);
                                        }
                                    }


                                    #endregion

                                    #region PDM.PointChoice.Navaid

                                    if (hldPnt.PointChoice == PDM.PointChoice.Navaid && hldPnt.PointChoiceID != null)
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
                                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, m_hookHelper.ActiveView.FocusMap.MapScale);
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
                                            TerminalChartsUtil.CreateStoreAngleIndication(((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, m_hookHelper.ActiveView.FocusMap, (Anno_NavaidGeo_featClass as IGeoDataset).SpatialReference, 0,
                                                 ((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.ID, ((HoldingPattern)hld).EndPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidList);


                                        }

                                        #endregion

                                        #region DistanceIndication

                                        if (((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                                        {

                                            TerminalChartsUtil.CreateStoreDistanceIndication(((HoldingPattern)hld).EndPoint.PointFacilityMakeUp.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, m_hookHelper.ActiveView.FocusMap, (Anno_NavaidGeo_featClass as IGeoDataset).SpatialReference, 0,
                                                ((HoldingPattern)hld).EndPoint, vertUom.ToString(), distUom.ToString(), ref angleIndicationDictionary, ref NavaidList);

                                        }

                                        #endregion

                                    }
                                }

                            }


                        }
                    }


                    alrtForm.Close();


                    ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Successfully created", global::SigmaChart.Properties.Resources.SigmaMessage);
                    msgFrm.TopMost = true;
                    msgFrm.checkBox1.Visible = false;
                    msgFrm.ShowDialog();

                }
                catch (Exception ex)
                {
                    ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Something get wrong...", global::SigmaChart.Properties.Resources.GenericDeleteRed32);
                    msgFrm.TopMost = true;
                    msgFrm.checkBox1.Visible = false;
                    msgFrm.ShowDialog();
                    
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }

            ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView", true);

            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }


        #endregion
    }
}
