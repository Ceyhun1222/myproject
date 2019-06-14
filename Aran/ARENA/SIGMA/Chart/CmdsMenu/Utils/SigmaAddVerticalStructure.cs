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

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("35619666-0c5d-41c7-be30-70452e7e59ac")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaAddVerticalStructure")]
    public sealed class SigmaAddVerticalStructure : BaseCommand
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

        public SigmaAddVerticalStructure()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create VS Annotation";  //localizable text 
            base.m_message = "Add new Vertical Structure Annotation";  //localizable text
            base.m_toolTip = "Add new Vertical Structure Annotation";  //localizable text
            base.m_name = "CreateVertiacalStructureAnno";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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
//            string[] ObsId = { "AI1164",
//"AI1172",
//"AI1177",
//"AI1181",
//"AI1256",
//"DSM_I-1018",
//"DSM_I-1152",
//"DSM_I-1261",
//"DSM_I-1489",
//"DSM_I-156",
//"DSM_I-34",
//"DSM_I-385",
//"DSM_I-497",
//"DSM_I-519",
//"DSM_I-597",
//"DSM_I-858",
//"DSM_I-922",
//"DSM_I-948",
//"DSM_O-1577",
//"DSM_O-1599",
//"DSM_O-936",
//"DSM_O-937"};

            if (DataCash.ProjectEnvironment == null || DataCash.ProjectEnvironment.Data == null || DataCash.ProjectEnvironment.Data.PdmObjectList == null) return;

            var _featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null)
                                &&  (element.PDM_Type == PDM_ENUM.VerticalStructure)
                                && (((PDM.VerticalStructure)element).Parts != null) && (((PDM.VerticalStructure)element).Parts.Count > 0)
                                //&&  ObsId.Contains((((PDM.VerticalStructure)element).Name))
                                orderby ((VerticalStructure)element).Name select element).ToList();

            if (_featureList != null && _featureList.Count > 0)
            {

                try
                {

                    AddNewObject inptFrm = new AddNewObject("SIGMA", _featureList, PDM_ENUM.VerticalStructure);
                    inptFrm.TopMost = true;
                    var dr = inptFrm.ShowDialog();
                    if (dr == DialogResult.Cancel) return;

                    string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
                    chartInfo ci = EsriUtils.GetChartIno(FN);


                    List<PDMObject> obstacleList = new List<PDMObject>();
                    IFeatureClass Anno_ObstacleGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["VerticalStructurePointCartography"];
                    IFeatureClass Anno_Obstacle_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["VerticalStructureAnnoCartography"];
                    string qry = "";
                    IQueryFilter featFilter = new QueryFilterClass();

                    AlertForm alrtForm = new AlertForm();

                    alrtForm.FormBorderStyle = FormBorderStyle.None;


                    alrtForm.Opacity = 0.5;
                    alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

                    if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                    alrtForm.progressBar1.Visible = true;
                    alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                    alrtForm.progressBar1.Maximum = inptFrm.checkedListBox1.CheckedItems.Count;
                    alrtForm.progressBar1.Value = 0;

                    alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
                    alrtForm.label1.Text = "Start process";
                    alrtForm.label1.Visible = true;

                    for (int i = 0; i < inptFrm.checkedListBox1.Items.Count; i++)
                    {
                        if (inptFrm.checkedListBox1.GetItemChecked(i))
                        {
                            alrtForm.label1.Text = _featureList[i].GetObjectLabel();
                            Application.DoEvents();

                            obstacleList.Add(_featureList[i]);
                            foreach (var vPart in ((VerticalStructure)_featureList[i]).Parts)
                            {
                                qry = qry + "'" + vPart.ID + "',";
                            }

                            alrtForm.progressBar1.Value++; alrtForm.BringToFront();


                        }
                    }


                    inptFrm.Dispose();

                    #region Delete from Geo_featClass

                    string qry1 = "FeatureGUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry1;

                    IFeatureCursor featCur = Anno_ObstacleGeo_featClass.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    #region Delete from Anno_featClass

                    string qry2 = "PdmUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry2;

                    featCur = Anno_Obstacle_featClass.Search(featFilter, false);

                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    Marshal.ReleaseComObject(featCur);



                    alrtForm.label1.Text = "Saving.....";
                    alrtForm.BringToFront();

                    TerminalChartsUtil.SaveVerticalStructureGeo(obstacleList, Anno_ObstacleGeo_featClass, 0, ci.uomVert, ci.uomDist, m_hookHelper.ActiveView.FocusMap.MapScale);

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
