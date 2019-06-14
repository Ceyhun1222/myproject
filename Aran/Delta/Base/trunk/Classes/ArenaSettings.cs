using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Delta.Settings;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;

namespace Aran.Delta.Classes
{
    /// <summary>
    /// Summary description for ArenaSettings.
    /// </summary>
    [Guid("ebfbb7df-b402-4ed6-9e82-90f396ceb786")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Delta.Classes.ArenaSettings")]
    public sealed class ArenaSettings : BaseCommand
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
        private HookHelperClass m_hookHelper;
        public ArenaSettings()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "Settings";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "Arena Settings";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

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

       

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;
            GlobalParams.Application = m_application;

            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;

            GlobalParams.HookHelper = m_hookHelper;
            if (m_application != null) GlobalParams.HWND = m_application.hWnd;

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
            try
            {
                if (Functions.InitalizeExtension())
                {
                    var settings = GlobalParams.DeltaExt.Settings;
                    if (settings == null)
                        settings = new DeltaSettings();
                    
                    var settingsForm = new SettingsArenaWindow(settings);

                    Win32Windows win32Window = new Win32Windows(GlobalParams.HWND);

                    var helper = new WindowInteropHelper(settingsForm);
                    helper.Owner = new IntPtr(GlobalParams.HWND);
                    ElementHost.EnableModelessKeyboardInterop(settingsForm);
                    settingsForm.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    settingsForm.Show();
                }
                else
                {
                    MessageBox.Show("Cannot initialized settings extension", "Delta", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        #endregion
    }
}
