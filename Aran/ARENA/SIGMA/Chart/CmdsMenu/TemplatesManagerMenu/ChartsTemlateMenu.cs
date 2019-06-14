using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace SigmaChart.TemplateManager
{
    /// <summary>
    /// Summary description for SigmaChartMenu.
    /// </summary>
    [Guid("7f2315ea-6d15-4bfd-9d75-ee0c9a41c262")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.TemplateManager.ChartsTemplateMenu")]
    public sealed class ChartsTemplateMenu : BaseMenu
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
            MxCommandBars.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Unregister(regKey);
        }

        #endregion
        #endregion

        public ChartsTemplateMenu()
        {

            //AddItem("SigmaChart.TemplateManager.NewTemplateCmd");
            //AddItem("SigmaChart.TemplateManager.SaveTemplateCmd");
            AddItem("SigmaChart.SigmaSaveTemplate");
            //BeginGroup(); //Separator
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Sigma Templates";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "SigmaTemplatesManagerMenu";
            }
        }
    }
}