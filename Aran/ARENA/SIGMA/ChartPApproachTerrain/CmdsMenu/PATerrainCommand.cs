using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using System.Windows.Forms;
using ARENA;

namespace ChartPApproachTerrain.CmdsMenu 
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4974a3c0-becd-4542-8eee-f550f027a350")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartPApproachTerrain.CmdsMenu.PATerrainCommand")]
    public sealed class PATerrainCommand : BaseCommand
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
            string regKey = $"HKEY_CLASSES_ROOT\\CLSID\\{{{registerType.GUID}}}";
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = $"HKEY_CLASSES_ROOT\\CLSID\\{{{registerType.GUID}}}";
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IApplication m_application;
        private HookHelperClass m_hookHelper;       

        public PATerrainCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            m_category = "PATC"; //localizable text
            m_caption = "PATC ";  //localizable text 
            m_message = "";  //localizable text
            m_toolTip = "";  //localizable text
            m_name = "PATC";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                m_bitmap = new Bitmap(GetType(), bitmapResourceName);

                m_bitmap = Properties.Resources.Sigma;
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
            m_hookHelper = new HookHelperClass {Hook = hook};

            GlobalParams.HookHelper = m_hookHelper;
            if (m_application != null) GlobalParams.HWND = m_application.hWnd;
            //Disable if it is not ArcMap
            if (hook is IMxApplication)
                m_enabled = true;
            else
                m_enabled = false;

            // TODO:  Add other initialization code


        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (DataCash.ProjectEnvironment == null)
            {
                MessageBox.Show("Error loading Database!");
                return;
            }

            if (InitChartPAT.InitCommand())
            {
                if (GlobalParams.IsOpened)
                    return;

                var frm = new PATerrainWizardForm();
                ArcMapWrapper wrapper = new ArcMapWrapper(m_application);

                GlobalParams.IsOpened = true;

                frm.Show(wrapper);

            }
        }

        #endregion
    }
}
