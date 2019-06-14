using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace SigmaChart
{
    /// <summary>
    /// Summary description for SigmaChartMenu.
    /// </summary>
    [Guid("f863bf61-46eb-4c15-9a90-c03eb987eeca")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaChartMenu")]
    public sealed class SigmaChartMenu : BaseMenu
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

        public SigmaChartMenu()
        {
            //
            // TODO: Define your menu here by adding items
            //SigmaChart.SigmaNewChart
            AddItem("SigmaChart.SigmaNewChart");
            AddItem("SigmaChart.SigmaUpdateChartMenu");
            AddItem ("SigmaChart.VisibilitySettings");

            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaArchive");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaLog");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaRestoreDefault");
            BeginGroup();
            AddItem("SigmaChart.SigmaWEBConvert");
            BeginGroup(); //Separator
            AddItem("ARENA.HelpCmd");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaChartInfo");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaAbout");
            //BeginGroup(); //Separator
            //AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
            //AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command

            
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Sigma Chart Menu";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "SigmaChartMenu";
            }
        }
    }
}