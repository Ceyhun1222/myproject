
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WpfUI;

namespace AerodromeToolBox.ArcMap_components
{
    [Guid("4BF5FEDE-B60F-443D-8A2C-3D0042FC8113")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeOpenUI")]
    public sealed class AerodromeOpenUI : BaseCommand
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
        public AerodromeOpenUI()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Open Data Manager"; //localizable text
            base.m_caption = "Data Manager";  //localizable text
            base.m_message = "Open Data Manager";  //localizable text 
            base.m_toolTip = "Open Data Manager";  //localizable text 
            base.m_name = "AerodromeOpenUI";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = global::ArenaToolBox.Properties.Resources.ArcReader16;



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

            if (AerodromeDataCash.ProjectEnvironment is null)
            {
                System.Windows.MessageBox.Show("Empty project", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            try
            {
                MainWindow mainWindow = new MainWindow(AerodromeDataCash.ProjectEnvironment.Context)
                {
                    ShowInTaskbar = false,

                };
                var parentHandle = new IntPtr(m_application.hWnd);
                var helper = new WindowInteropHelper(mainWindow) { Owner = parentHandle };

                mainWindow.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           

        }

        public override bool Enabled
        {
            get
            {
                //if editing is started, then enable the command.  
                if (AerodromeDataCash.ProjectEnvironment is null)
                    return false;
                else
                    return true;
            }
        }


        #endregion
    }
}
