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
    [Guid("675303f8-81a4-4527-b8b2-400f7e0f2092")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaAddNavaid")]
    public sealed class SigmaAddNavaid : BaseCommand
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

        public SigmaAddNavaid()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create Navaid Annotation";  //localizable text 
            base.m_message = "Add new Navaid Annotation";  //localizable text
            base.m_toolTip = "Add new navaid Annotation";  //localizable text
            base.m_name = "CreateNavaideAnno";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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

            var _featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.NavaidSystem) 
                                && ((PDM.NavaidSystem)element).Components != null && ((PDM.NavaidSystem)element).Components.Count > 0
                                orderby ((NavaidSystem)element).CodeNavaidSystemType select element).ToList();

            if (_featureList != null && _featureList.Count > 0)
            {

                try
                {

                    AddNewObject inptFrm = new AddNewObject("SIGMA", _featureList, PDM_ENUM.NavaidSystem);
                    inptFrm.TopMost = true;
                    var dr = inptFrm.ShowDialog();
                    if (dr == DialogResult.Cancel) return;

                   

                    string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
                    chartInfo ci = EsriUtils.GetChartIno(FN);


                    List<PDMObject> selected_featureList = new List<PDMObject>();
                    IFeatureClass AnnoNavaidGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];
                    IFeatureClass Anno_navaid_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["NavaidsAnno"];
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

                            selected_featureList.Add(_featureList[i]);
                            foreach (var vPart in ((NavaidSystem)_featureList[i]).Components)
                            {
                                qry = qry + "'" + vPart.ID_NavaidSystem + "',";
                            }

                            alrtForm.progressBar1.Value++; alrtForm.BringToFront();


                        }
                    }


                    inptFrm.Dispose();

                    #region Delete from Geo_featClass

                    string qry1 = "FeatureGUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry1;

                    IFeatureCursor featCur = AnnoNavaidGeo_featClass.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    #region Delete from Anno_featClass

                    string qry2 = "PdmUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry2;

                    featCur = Anno_navaid_featClass.Search(featFilter, false);

                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    Marshal.ReleaseComObject(featCur);



                    alrtForm.label1.Text = "Saving.....";
                    alrtForm.BringToFront();

                    List<string> navaidListID = new List<string>();
                    TerminalChartsUtil.CreateNavaids_ChartElements(selected_featureList, m_hookHelper.ActiveView.FocusMap.MapScale, ref navaidListID, AnnoNavaidGeo_featClass, ci.uomVert);

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
