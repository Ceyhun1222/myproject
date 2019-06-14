using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaExportDataToAIM.
    /// </summary>
    [Guid("054b30bd-6795-4ac3-b1a1-5b04f4b25eeb")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaExportDataToAIM")]
    public sealed class ArenaExportDataToAIM : BaseCommand
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
        public ArenaExportDataToAIM()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Export data from Arena project file"; //localizable text
            base.m_caption = "Export data to AIM AIXM 5.1";  //localizable text
            base.m_message = "Export data";  //localizable text 
            base.m_toolTip = "Export data";  //localizable text 
            base.m_name = "ArenaExportDataToAIM";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.ExplorerNMCFile16;
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
           // base.m_enabled = false; //‚ÂÏÂÌÌÓ. ”¡–¿“‹!!!!!!
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ArenaExportDataToAIM.OnClick implementation
            Environment.Environment curEnvironment = DataCash.ProjectEnvironment; //new Environment.Environment(m_application);

            if (curEnvironment.pMap != null)
            {

                ILayer _Layer = EsriUtils.getLayerByName(curEnvironment.pMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;
                ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.PDM_AIM_DataConverter(curEnvironment);
                _data_loader.Convert_Data(fc);


                curEnvironment.SetCenter_and_Projection();
                ((IActiveView)curEnvironment.pMap).Refresh();
                curEnvironment.SaveLog();

                
            }
        }

        #endregion
    }
}
