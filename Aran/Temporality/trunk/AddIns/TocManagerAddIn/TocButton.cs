using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace TocManagerAddIn
{
    [Guid("7961bd7f-5646-4599-9e83-0e652a234902")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TocManagerAddIn.TocButton")]
    public class TocButton : BaseCommand
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


        private IHookHelper m_hookHelper = null;
        public TocButton()
        {
            base.m_category = "Add-In Controls"; //localizable text
            base.m_caption = "Show / Hide TOC";  //localizable text 
            base.m_message = "Shows and hides table of contents.";  //localizable text
            base.m_toolTip = "Show / Hide TOC";  //localizable text
            base.m_name = "TocManager";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                m_bitmap = new Bitmap(GetType(), "toc.png");
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
            }
            catch
            {
                m_hookHelper = null;
            }

            base.m_enabled = m_hookHelper != null;
        }


        #endregion

        public static Action IsCheckedChanged; 

        private static bool _isChecked;
        public static bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                if (IsCheckedChanged!=null)
                {
                    IsCheckedChanged();
                }
            }
        }

        public override void OnClick()
        {
            IsChecked = !IsChecked;

            m_caption = (IsChecked ? "Hide" : "Show") + " TOC";

            m_checked = IsChecked;
        }
    }

}
