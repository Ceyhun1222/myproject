using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using AranSupport;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ArenaStatic;
using ESRI.ArcGIS.Framework;
using System.IO;
using System.Linq;
using ARENA;
//using SigmaChart.CmdsMenu;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;
using EsriWorkEnvironment;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using PDM;
using ANCOR.MapElements;
using ANCOR;
using System.Xml.Serialization;
using ANCOR.MapCore;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace SigmaChart.TemplateManager
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("2fb2e36b-ce88-4c3d-b007-6387f634bdfb")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.TemplateManager.NewTemplateCmd")]
    public sealed class NewTemplateCmd : BaseCommand
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

        public NewTemplateCmd()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Chart Template ";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "ChartTemplateManger";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //base.m_bitmap = global::TemplateManager.Properties.Resources.PortalItemPITEMFile16;
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

            //Disable if it is not ArcMap
            //if (hook is IMxApplication)
            //    base.m_enabled = EsriUtils.CheckLicense(esriLicenseProductCode.esriLicenseProductCodeBasic) == esriLicenseStatus.esriLicenseAvailable;
            //else
            //    base.m_enabled = false;

            //if (m_hookHelper == null)
            //    base.m_enabled = false;
            //else
            //    base.m_enabled = true;

            // TODO:  Add other initialization code



                m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

           
        }

        #endregion
    }
}
