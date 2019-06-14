using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using EsriWorkEnvironment;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ArenaStatic;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using ARENA;
using ESRI.ArcGIS.Carto;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("71b81fed-97ca-4081-868c-0e3543ff6fd4")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.STARChart")]
    public sealed class STARChart : BaseCommand
    {
        private IHookHelper m_hookHelper = null;
        private IApplication m_application;

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

        public STARChart()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "STAR Chart";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "STAR Chart";  //localizable text
            base.m_name = "NewSTARChart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

            m_application = hook as IApplication;

            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;
        }

        /// <summary>
        /// Occurs when this command is clicked=-
        /// </summary>
        public override void OnClick()
        {

            if (((IMapDocument)m_application.Document).DocumentFilename.Length > 0)
            {
                string _DocumentFilename = System.IO.Path.GetDirectoryName(((IMapDocument)m_application.Document).DocumentFilename);

                if (!TerminalChartsUtil.CheckPdmProjectType(SigmaChartTypes.STARChart_Type, _DocumentFilename)) return;
            }

            // TODO: Add SigmaChart.OnClick implementation
            if (!((IMxDocument)m_application.Document).CurrentContentsView.Name.StartsWith("TOCLayerFilter"))
            {
                ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "TOCLayerFilter");
                Application.DoEvents();

                if (DataCash.ProjectEnvironment != null && DataCash.ProjectEnvironment.Data != null && DataCash.ProjectEnvironment.Data.CurProjectName == null)
                {
                    ArenaStaticProc.AutoSaveArenaProject(m_application);
                    Application.DoEvents();
                }
            }
            ArenaStaticProc.SetEnvironmentPath();


            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            STARChartClass _Chart = new STARChartClass { SigmaHookHelper = m_hookHelper, SigmaApplication = m_application };
            _Chart.CreateChart();

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

        }

        #endregion
    }
}
