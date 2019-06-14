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
    [Guid("01dcaa9e-f164-4c42-b221-df0995b5eedd")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaAirspaceBufer")]
    public sealed class SigmaAirspaceBufer : BaseCommand
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

        public SigmaAirspaceBufer()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create Sigma airspace bufer";  //localizable text 
            base.m_message = "Create Sigma airspace bufer";  //localizable text
            base.m_toolTip = "Create Sigma airspace bufer";  //localizable text
            base.m_name = "Create Sigma airspace bufer";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
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

            ArenaInputForm inptFrm = new ArenaInputForm("SIGMA", "Please, input new buffer width (mm)", img: global::SigmaChart.Properties.Resources.SigmaMessage);
            inptFrm.TopMost = true;
            var dr = inptFrm.ShowDialog();
            if (dr == DialogResult.Cancel) return;

            

            var arspc_featureList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.Airspace) select element).ToList();

            AirspaceBuffer buf = new AirspaceBuffer(m_hookHelper);
            var bufferList = new List<Bagel>();
            int airspaceBuferWidth = inptFrm.IntValue;
            inptFrm.Dispose();


            AlertForm alrtForm = new AlertForm();

            alrtForm.FormBorderStyle = FormBorderStyle.None;


            alrtForm.Opacity = 0.5;
            alrtForm.BackgroundImage = SigmaChart.Properties.Resources.SigmaSplash;

            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

            alrtForm.progressBar1.Visible = true;
            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.progressBar1.Maximum = arspc_featureList.Count;
            alrtForm.progressBar1.Value = 0;

            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.label1.Text = "Start process";
            alrtForm.label1.Visible = true;

            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            foreach (PDM.Airspace arsps in arspc_featureList)
            {
                alrtForm.label1.Text = arsps.GetObjectLabel();
                Application.DoEvents();

                foreach (PDM.AirspaceVolume arspsVol in arsps.AirspaceVolumeList)
                {

                    if (arspsVol.CodeType == AirspaceType.OTHER) continue;
                    if (arspsVol.CodeType == AirspaceType.AMA) continue;

                    arspsVol.Geo = ArnUtil.GetGeometryFromChart(arspsVol.ID, "AirspaceC", ArenaStatic.ArenaStaticProc.GetTargetDB());
                    if (arspsVol.Geo == null || arspsVol.Geo.IsEmpty) continue;
                    if (((IArea)arspsVol.Geo).Area == 0) continue;


                    IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)arspsVol.Geo;
                    zAware.ZAware = false;
                    IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)arspsVol.Geo;
                    mAware.MAware = false;

                    
                    if (airspaceBuferWidth > 0)
                    {
                        Bagel bgl = buf.Buffer((IPolygon)arspsVol.Geo, airspaceBuferWidth);

                        bgl.BagelCodeId = arspsVol.CodeType.ToString();
                        bgl.BagelCodeClass = arspsVol.CodeClass;
                        bgl.BagelTxtName = arspsVol.TxtName;
                        bgl.MasterID = arspsVol.ID;
                        bgl.BagelLocalType = arspsVol.TxtLocalType;
                        bufferList.Add(bgl);
                    }
                }

                alrtForm.progressBar1.Value++; alrtForm.BringToFront();
            }

            IFeatureClass arspBuf = SigmaDataCash.AnnotationLinkedGeometryList["AirspaceB"];

            string qry = "OBJECTID > 0";

            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = qry;

            IFeatureCursor featCur = arspBuf.Search(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {
                _Feature.Delete();
            }

            Marshal.ReleaseComObject(featCur);


            alrtForm.label1.Text = "Saving.....";
            alrtForm.BringToFront();

            ChartsHelperClass.SaveBuffer(bufferList, arspBuf);


            alrtForm.Close();

            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Successfully created", global::SigmaChart.Properties.Resources.SigmaMessage);
            msgFrm.TopMost = true;
            msgFrm.checkBox1.Visible = false;
            msgFrm.ShowDialog();

            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
        }


        #endregion
    }
}
