using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using Aran.Omega.TypeB.Settings;

namespace Aran.Omega.TypeBEsri.EsriExtensions
{
    /// <summary>
    /// Summary description for NewProjectCommand.
    /// </summary>
    [Guid("74dbbf72-f1e4-4c74-a9fe-e2aa38ba7a47")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Omega.TypeBEsri.EsriExtensions.NewProjectCommand")]
    public sealed class NewProjectCommand : BaseCommand
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
        private OmegaExtension _pandaExt;
        public NewProjectCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "Settings";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "Settings";  //localizable text 
            base.m_name = "NewProject_Command";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        private bool InitalizeExtension()
        {
            try
            {
                if (_pandaExt == null)
                {
                    ESRI.ArcGIS.esriSystem.UID pID = new ESRI.ArcGIS.esriSystem.UID();
                    pID.Value = "TypeBExtension.OmegaExtension";
                    _pandaExt = m_application.FindExtensionByCLSID(pID) as OmegaExtension;
                }
            }
            catch
            {
                //Console.WriteLine("{0} Exception caught.", e);
            }

            return _pandaExt != null;
        }
        
        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        /// 

        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;
            GlobalParams.HWND = m_application.hWnd;
            
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
            if (InitalizeExtension())
            {
                var settings = Aran.Omega.TypeB.Settings.Globals.Settings;
                var settingsForm = new TypeB.Settings.SettingsForm();

                settingsForm.OnLoad(_pandaExt.Settings);

                Win32Windows win32Window = new Win32Windows(GlobalParams.HWND);
                settingsForm.Show(win32Window);
            }
        }

        #endregion
    }
}
