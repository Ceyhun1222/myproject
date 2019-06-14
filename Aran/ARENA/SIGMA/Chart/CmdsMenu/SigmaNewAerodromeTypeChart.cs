using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace SigmaChart
{

    [Guid("0a1c305b-3295-421c-bc82-c92da784a2d4")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaNewAerodromeTypeChart")]
    public sealed class SigmaNewAerodromeTypeChart : BaseMenu
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

        public SigmaNewAerodromeTypeChart()
        {
            AddItem("ChartTypeA.CmdsMenu.TypeAMenu");
            BeginGroup(); //Separator
            AddItem("ChartPApproachTerrain.CmdsMenu.PATerrainCommand");
            BeginGroup(); //Separator
            AddItem("SigmaChart.AerodromeElectronicChart");
            BeginGroup(); //Separator
            AddItem("SigmaChart.AerodromeChart");
            AddItem("SigmaChart.AerodromeParkingDockingChart"); 
            AddItem("SigmaChart.AerodromeGroundMovementChart"); 
            AddItem("SigmaChart.AerodromeBirdChart");
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Aerodrome chart";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "SigmaNewAerodromeTypeChart";
            }
        }
    }
}
