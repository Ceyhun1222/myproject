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
    [Guid("1866962a-2a12-44a6-bbb1-5379382bb0b3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaAddAirport")]
    public sealed class SigmaAddAirport : BaseCommand
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

        public SigmaAddAirport()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create Airport Annotation";  //localizable text 
            base.m_message = "Add new Airport Annotation";  //localizable text
            base.m_toolTip = "Add new Airport Annotation";  //localizable text
            base.m_name = "CreateAirportAnno";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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

            var _featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.AirportHeliport) orderby ((AirportHeliport)element).Designator select element).ToList();
            if (_featureList != null && _featureList.Count > 0)
            {

                try
                {

                    AddNewObject inptFrm = new AddNewObject("SIGMA", _featureList, PDM_ENUM.AirportHeliport);
                    inptFrm.TopMost = true;
                    var dr = inptFrm.ShowDialog();
                    if (dr == DialogResult.Cancel) return;



                    string FN = System.IO.Path.GetDirectoryName((m_application.Document as IDocumentInfo2).Path);
                    chartInfo ci = EsriUtils.GetChartIno(FN);


                    List<AirportHeliport> selected_featureList = new List<AirportHeliport>();
                    IFeatureClass AnnoGeo_featClass = SigmaDataCash.AnnotationLinkedGeometryList["AirportCartography"];
                    IFeatureClass Anno_featClass = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList["AirportAnnoCartography"]; 
                    IFeatureClass AnnoRWYGeo_featClass = (IFeatureClass)SigmaDataCash.AnnotationLinkedGeometryList["RunwayCartography"];

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

                            ((AirportHeliport)_featureList[i]).CertifiedICAO = true;

                            selected_featureList.Add((AirportHeliport)_featureList[i]);

                            qry = qry + "'" + _featureList[i].ID + "',";
                            alrtForm.progressBar1.Value++; alrtForm.BringToFront();


                        }
                    }


                    inptFrm.Dispose();

                    #region Delete from Geo_featClass

                    string qry1 = "FeatureGUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry1;

                    IFeatureCursor featCur = AnnoGeo_featClass.Search(featFilter, false);

                    IFeature _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    #region Delete from Anno_featClass

                    string qry2 = "PdmUID IN (" + qry + "'00000')";
                    featFilter.WhereClause = qry2;

                    featCur = Anno_featClass.Search(featFilter, false);

                    _Feature = null;
                    while ((_Feature = featCur.NextFeature()) != null)
                    {
                        _Feature.Delete();
                    }

                    #endregion

                    Marshal.ReleaseComObject(featCur);



                    alrtForm.label1.Text = "Saving.....";
                    alrtForm.BringToFront();

                    
                    TerminalChartsUtil.CreateAirportAnno(selected_featureList, "", AnnoGeo_featClass, AnnoRWYGeo_featClass, m_hookHelper.ActiveView.FocusMap, (AnnoGeo_featClass as IGeoDataset).SpatialReference, true);


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
