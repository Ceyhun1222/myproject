using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Aerodrome.Metadata;
using AmdbManager;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;

namespace ChartManagerTools
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("15A20F85-ECFA-4BE0-AA8F-740AAE086644")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("AmdbManagerTools.SaveAsNewAmdbCmd")]
    public sealed class SaveAsNewAmdbCmd : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);
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

        private IApplication _app;

        public SaveAsNewAmdbCmd()
        {
            m_category = "SaveAsNewAmdbCmd"; //localizable text
            m_caption = "Save as new amdb";  //localizable text 
            m_message = "";  //localizable text
            m_toolTip = "Save as new amdb";  //localizable text
            m_name = "AmdbManager_SaveAsNewAmdbCmd";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                string bitmapResourceName = GetType().Name + ".bmp";
                m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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

            _app = hook as IApplication;

            //Disable if it is not ArcMap
            m_enabled = hook is IMxApplication;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Clear();
            HelperMethods.SaveAmdbProject(_app, isCommitFeatures: false);
        }

        public override bool Enabled
        {
            get
            {
                //if editing is started, then enable the command.  
                return ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.ProjectIsOpened;
            }
        }

        #endregion
    }
}
