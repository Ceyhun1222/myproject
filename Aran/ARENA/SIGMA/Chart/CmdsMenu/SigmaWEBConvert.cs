using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ANCOR.MapElements;
using EsriWorkEnvironment;
using System.IO;
using ArenaStatic;
using ANCOR.MapCore;
using ANCOR;
using System.Xml.Serialization;
using PDM;
using ANCORTOCLayerView;
using System.Media;
using DataModule;
using ARENA.Enums_Const;
using AranSupport;
using ARENA;


namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("c83843b8-1a82-4589-8bcf-2a00433b0263")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaWEBConvert")]
    public sealed class SigmaWEBConvert : BaseCommand
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
        AlertForm alrtForm = null;
        public SigmaWEBConvert()
        {
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Sigma Convert to simple style";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Convert to simple style project";  //localizable text
            base.m_name = "SigmaWEBConvert";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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
            alrtForm = new AlertForm();

            alrtForm.FormBorderStyle = FormBorderStyle.None;
            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;
            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();


            #region Initialization

            IMap pMap = m_hookHelper.FocusMap;

            if (pMap.Description.Trim().Length > 0) SigmaDataCash.SigmaChartType = Convert.ToInt32(pMap.Description);

            ILayer _Layer = EsriUtils.getLayerByName(pMap, ChartElementsManipulator.DefinelayerName("AirportHeliport"));
            //if (_Layer == null) return;

            if (SigmaDataCash.environmentWorkspaceEdit == null)
            {
                if (_Layer == null) return;
                var fc = ((IFeatureLayer)_Layer).FeatureClass;
                var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;

                SigmaDataCash.environmentWorkspaceEdit = workspaceEdit;

            }


            if (SigmaDataCash.prototype_anno_lst == null || SigmaDataCash.prototype_anno_lst.Count <= 0)
            {

                string pathToTemplateFileXML = "";
                switch (SigmaDataCash.SigmaChartType)
                {
                    case (1):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\Enroute\", "enroute.sce");
                        break;
                    case (2):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\SID\", "sid.sce");
                        break;
                    case (4):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\STAR\", "star.sce");
                        break;
                    case (5):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\IAP\", "iap.sce");
                        break;

                }


                if (File.Exists(pathToTemplateFileXML)) SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);
            }

            if (SigmaDataCash.ChartElementList.Count == 0)
            ChartElementsManipulator.LoadChartElements(SigmaDataCash.environmentWorkspaceEdit);

            if (SigmaDataCash.ChartElementList.Count == 0) return;

            #endregion


            var arspc_featureList = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SigmaCollout_Airspace) select element).ToList();
            var dpn_featureList = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SigmaCollout_Designatedpoint) select element).ToList();
            var nav_featureList = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SigmaCollout_Navaid) select element).ToList();

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

            
            RemoveNewStyleElements(arspc_featureList, "Airspace_Simple", "SigmaCollout_Airspace", pMap);
            RemoveNewStyleElements(dpn_featureList, "DesignatedPoint", "SigmaCollout_Designatedpoint", pMap);
            RemoveNewStyleElements(nav_featureList, "Navaids", "SigmaCollout_Navaid", pMap);


            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

            for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
            {
                IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                if (cntxtName.StartsWith("ANCORTOCLayerView"))
                {
                    if (!((IMxDocument)m_application.Document).CurrentContentsView.Name.StartsWith("ANCORTOCLayerView"))
                        ((IMxDocument)m_application.Document).CurrentContentsView = cnts;

                    ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);

                }
            }

            alrtForm.Close();
            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Process successfuly finished", global::SigmaChart.Properties.Resources.SigmaMessage);
            msgFrm.TopMost = true;
            msgFrm.checkBox1.Visible = true;
            msgFrm.ShowDialog();
     
        }


        private void RemoveNewStyleElements(List<object> _featureList, string OldStylePrototype_name,string NewStylePrototype_name, IMap pMap)
        {
            if (_featureList == null || _featureList.Count <= 0) return;

            foreach (var chartEl_newStyle in _featureList)
            {
                try
                {

                    ChartElement_SimpleText cartoEl_newStyle = (ChartElement_SimpleText)chartEl_newStyle;

                    IGeometry geom = ChartElementsManipulator.GetLinkedGeometry(cartoEl_newStyle.Name, cartoEl_newStyle.LinckedGeoId);
                    IGeometry annoPos = TerminalChartsUtil.AnnotationsPosition(cartoEl_newStyle, pMap);

                    #region delete new style annotations

                    IQueryFilter queryFilter = new QueryFilterClass();
                    ILayer SelLayer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(((ChartElement_SimpleText)_featureList[0]).Name));
                    if (SelLayer == null) continue;

                    ITable Tbl = ((IFeatureLayer)SelLayer).FeatureClass as ITable;

                    queryFilter.WhereClause = "PdmUid = '" + cartoEl_newStyle.LinckedGeoId + "' AND AncorUID = '" + ((ChartElement_SimpleText)cartoEl_newStyle).Id.ToString() + "'";

                    Tbl.DeleteSearchedRows(queryFilter);

                    Application.DoEvents();

                    Marshal.ReleaseComObject(queryFilter);

                    #endregion


                    #region customize element
                    
                    var chrtEl_OldStyle = ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, OldStylePrototype_name);
                    ((ChartElement_SimpleText)chrtEl_OldStyle).TextContents.Clear();
                    ((ChartElement_SimpleText)chrtEl_OldStyle).TextContents.AddRange(cartoEl_newStyle.TextContents);

                    ((ChartElement_SimpleText)chrtEl_OldStyle).FillStyle = cartoEl_newStyle.FillStyle;
                    ((ChartElement_SimpleText)chrtEl_OldStyle).FillColor = new AncorColor(cartoEl_newStyle.FillColor.Red, cartoEl_newStyle.FillColor.Green, cartoEl_newStyle.FillColor.Blue);

                    if (chrtEl_OldStyle is ChartElement_BorderedText_Collout_CaptionBottom)
                    {
                        if (((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).CaptionTextLine != null && ((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).CaptionTextLine.Count>0)
                        {
                            ((ChartElement_BorderedText_Collout_CaptionBottom)chrtEl_OldStyle).CaptionTextLine.Clear();
                            ((ChartElement_BorderedText_Collout_CaptionBottom)chrtEl_OldStyle).CaptionTextLine.AddRange(((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).CaptionTextLine);
                        }

                        if (((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).BottomTextLine != null && ((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).BottomTextLine.Count > 0)
                        {
                            ((ChartElement_BorderedText_Collout_CaptionBottom)chrtEl_OldStyle).BottomTextLine.Clear();
                            ((ChartElement_BorderedText_Collout_CaptionBottom)chrtEl_OldStyle).BottomTextLine.AddRange(((ChartElement_BorderedText_Collout_CaptionBottom)chartEl_newStyle).BottomTextLine);
                        }
                    }

                    #endregion


                    #region Link to anchor point
                    
                    if (geom.GeometryType == esriGeometryType.esriGeometryPolygon)
                        ((ChartElement_SimpleText)chrtEl_OldStyle).Anchor = new AncorPoint(((IArea)geom).Centroid.X, ((IArea)geom).Centroid.Y);
                    else
                        ((ChartElement_SimpleText)chrtEl_OldStyle).Anchor = new AncorPoint(((IPoint)geom).X, ((IPoint)geom).Y);

                    IElement newIElement = newIElement = chrtEl_OldStyle.ConvertToIElement() as IElement;
                    chrtEl_OldStyle.LinckedGeoId = cartoEl_newStyle.LinckedGeoId;

                    #endregion


                    #region Store old style anno
                    

                    switch (chrtEl_OldStyle.GetType().Name.ToString())
                    {
                        case ("ChartElement_TextArrow"):

                            newIElement.Geometry = annoPos == null ? ((IArea)geom).Centroid : ((IArea)annoPos).Centroid;
                            ChartElement_TextArrow elementArspc =  (ChartElement_TextArrow)chrtEl_OldStyle;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_OldStyle.Name, chrtEl_OldStyle.LinckedGeoId.ToString(), newIElement, ref elementArspc, chrtEl_OldStyle.Id, pMap.MapScale);

                            break;

                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):

                            if (newIElement is IGroupElement)
                            {
                                for (int i = 0; i < ((IGroupElement)newIElement).ElementCount; i++)
                                {
                                    ((IGroupElement)newIElement).get_Element(i).Geometry = annoPos == null ? geom : ((IArea)annoPos).Centroid;
                                }
                            }
                            else
                                newIElement.Geometry = annoPos == null ? geom : ((IArea)annoPos).Centroid;

                            ChartElement_BorderedText_Collout_CaptionBottom element = (ChartElement_BorderedText_Collout_CaptionBottom)chrtEl_OldStyle;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_OldStyle.Name, chrtEl_OldStyle.LinckedGeoId.ToString(), newIElement, ref element, chrtEl_OldStyle.Id, pMap.MapScale);

                            break;
                       
                        default:
                            break;
                    }

                    #endregion

                }
                catch (Exception ex) { continue; }
                alrtForm.progressBar1.Value++;
            }

            SigmaDataCash.ChartElementList.RemoveAll(element => element is ChartElement_SimpleText && ((ChartElement_SimpleText)element).Name.StartsWith(NewStylePrototype_name));

        }

        #endregion
    }
}
