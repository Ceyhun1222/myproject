using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using Aerodrome.Metadata;

namespace ChartManagerTools
{
    /// <summary>
    /// Summary description for PublishCmd.
    /// </summary>
    [Guid("9323B590-7975-4304-A2F1-69EAD8845796")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("AmdbManagerTools.PublishCmd")]
    public sealed class PublishCmd : BaseCommand
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

        private IApplication _app;
        public PublishCmd()
        {
            m_category = "PublishCmd"; //localizable text
            m_caption = "Publish";  //localizable text
            m_message = "";  //localizable text 
            m_toolTip = "Publish amdb";  //localizable text 
            m_name = "AmdbManager_Publish";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

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
            GlobalController.PublishAsync(_app,false);
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