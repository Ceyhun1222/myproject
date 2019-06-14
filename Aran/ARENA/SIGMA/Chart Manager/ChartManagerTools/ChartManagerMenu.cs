using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace ChartManagerTools
{
    /// <summary>
    /// Summary description for ChartManagerMenu.
    /// </summary>
    [Guid("45a6e4bd-3f04-4070-8afd-633c5d4b00bf")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartManagerTools.ChartManagerMenu")]
    public sealed class ChartManagerMenu : BaseMenu
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

        public ChartManagerMenu()
        {
            AddItem("ChartManagerTools.InfoCmd"); 
            AddItem("ChartManagerTools.UnlockCmd");
            AddItem("ChartManagerTools.UpdateCmd");
            AddItem("ChartManagerTools.PublishCmd");
            AddItem("ChartManagerTools.PublishAndLockCmd");
            AddItem("ChartManagerTools.SaveAsNewChartCmd");
            AddItem("ChartManagerTools.PendingUpdatesCmd");
            BeginGroup();
            AddItem("ChartManagerTools.OpenManagerCmd");
        }

        public override string Caption { get; } = "Chart Manager Menu";


        public override string Name { get; } = "ChartManagerMenu";
    }
}