using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;
using System.IO;
using Encryptor;

namespace SigmaChart
{
    /// <summary>
    /// Summary description for ArenaChartToolbar.
    /// </summary>
    [Guid("84fe14a0-3cd6-4095-8020-93d96f3ff97d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaChartToolbar")]
    public sealed class SigmaChartToolbar : BaseToolbar
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

        public SigmaChartToolbar()
        {
            //
            // TODO: Define your toolbar here by adding items 
            //
            if (DateTime.Now > SigmaEncryptor.GetEncryptedDate()) return;

            AddItem("SigmaChart.SigmaChartMenu");
            //if (System.Diagnostics.Debugger.IsAttached)
            AddItem("SigmaChart.TemplateManager.ChartsTemplateMenu");
            AddItem("SigmaChart.SigmaUtilsMenu");
            AddItem("SigmaChart.ChartElementSelector");
            AddItem("FindAMA.AMA");
            AddItem("SigmaChart.SigmaFrame");
            AddItem("SigmaChart.LegNoneScaleTool");
            AddItem("EnrouteTools.CrossRoute");
            AddItem("SigmaChart.IsogonalLines");
            AddItem("LayoutPageClip.LayoutpageClip");
            AddItem("SigmaChart.ChartColorSelector");
            AddItem("SigmaChart.SigmaSnappingTool");

            //BeginGroup(); //Separator
            //AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
            //AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Sigma Chart Toolbar";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "SigmaChartToolbar";
            }
        }

        

        
    }
}