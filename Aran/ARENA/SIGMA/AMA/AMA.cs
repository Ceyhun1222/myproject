using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using ArenaStatic;

namespace SigmaChart
{
    /// <summary>
    /// Summary description for AMA.
    /// </summary>
    [Guid("48313ce9-6466-40bb-94bf-41b5c8d10936")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("FindAMA.AMA")]
    public sealed class AMA : BaseCommand
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

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        private IHookHelper m_hookHelper =null;
        public AMA()
        {
            //
            // TODO: Define values for the public properties
            //
            //
            base.m_category = "Sigma Chart"; //localizable text
            base.m_caption = "AMA";  //localizable text 
            base.m_message = "AMA";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "AMA";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources._3DAnalystInterpolatePoint16;
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
            if (m_hookHelper.ActiveView == null)
                m_hookHelper = null;

            GlobalParams.HookHelper = m_hookHelper;
            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            if (SigmaDataCash.prototype_anno_lst == null)
            {
                var pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate(), "enroute.sce");
                switch (SigmaDataCash.SigmaChartType)
                {
                    case (2):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate(), "sid.sce");
                        break;
                    case (4):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate(), "star.sce");
                        break;
                    case (5):
                        pathToTemplateFileXML = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate(), "iap.sce");
                        break;
                    default:
                        break;
                }
                if (File.Exists(pathToTemplateFileXML)) SigmaDataCash.GetProtoTypeAnnotation(pathToTemplateFileXML);
            }

            var window = new MainWindow(m_hookHelper.FocusMap, new LayerHelper(m_hookHelper.FocusMap));
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(window) { Owner = parentHandle };
            ElementHost.EnableModelessKeyboardInterop(window);
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
        }

        #endregion
    }
}
