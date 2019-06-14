using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace SigmaChart
{

    /// <summary>
    /// Summary description for SigmaArchive.
    /// </summary>
    [Guid("062961fe-0e14-48cf-987d-167e18ba3a60")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaNewAreaChartType")]
    public sealed class NewAreaTypeChart : BaseMenu
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

        public NewAreaTypeChart()
        {
            //
            // TODO: Define your menu here by adding items
            //
            AddItem("SigmaChart.EnrouteChart");
            AddItem("SigmaChart.AreaChart");
            //if (System.Diagnostics.Debugger.IsAttached)
            {
               // BeginGroup(); //Separator
                AddItem("SigmaChart.AreaMinimumChart");
            }
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Area chart";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "NeaAreaTypeChart";
            }
        }
    }
}
