using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;
using ARENA;

namespace SigmaChart
{
    /// <summary>
    /// Summary description for SigmaNewChart.
    /// </summary>
    [Guid("5109df90-10e1-4dc7-a2ba-7ba65457eb65")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaUtilsMenu")]
    public sealed class SigmaUtilsMeny : BaseMenu
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

        public SigmaUtilsMeny()
        {
            AddItem("SigmaChart.SigmaProcTabulation");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaAddAirspace");
            AddItem("SigmaChart.SigmaAirspaceBufer");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaAddVerticalStructure");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaAddNavaid");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaAddAirport");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmaHoldingPattern");
            BeginGroup(); //Separator
            AddItem("SigmaChart.SigmareCreateChanelsList");
            BeginGroup(); //Separator
            AddItem("SigmaChart.FindAndReplaceCommand");
            //if (System.Diagnostics.Debugger.IsAttached)


        }


        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Sigma Utils Menu";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "SigmaUtilsMenu";
            }
        }

        
    }
}