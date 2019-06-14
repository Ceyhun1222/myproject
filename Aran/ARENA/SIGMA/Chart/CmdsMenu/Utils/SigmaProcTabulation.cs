using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ArenaStatic;
using EsriWorkEnvironment;
using ARENA;
using SigmaChart.CmdsMenu.TemplatesManagerMenu;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using PDM;
using ESRI.ArcGIS.Geometry;
using AranSupport;
using ChartCodingTable;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("597ab8eb-42e5-4b10-842e-84cbd4ecbeb4")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaProcTabulation")]
    public sealed class SigmaProcTabulation : BaseCommand
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
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IApplication m_application;

        public SigmaProcTabulation()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Create Procedure Tabulation";  //localizable text 
            base.m_message = "Create Procedure Tabulation";  //localizable text
            base.m_toolTip = "Create Procedure Tabulation";  //localizable text
            base.m_name = "Create Procedure Tabulation";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.Sigma;
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

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;

                m_application = hook as IApplication;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            if (DataCash.ProjectEnvironment == null || DataCash.ProjectEnvironment.Data == null || DataCash.ProjectEnvironment.Data.PdmObjectList == null ) return;

            SigmaChartTypes chartTp = (SigmaChartTypes)SigmaDataCash.SigmaChartType;

            if (chartTp != SigmaChartTypes.SIDChart_Type && chartTp != SigmaChartTypes.STARChart_Type && chartTp != SigmaChartTypes.IAPChart_Type) return;

            string tmpN = "SID";

            if (MessageBox.Show("Do you want to create Procuderes tabulation document?", "SIGMA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string destPath2 = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Folder;
                string projectName = ((ESRI.ArcGIS.Carto.IDocumentInfo2)(m_application.Document)).Name;
                var selARP = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) select element).FirstOrDefault();
                var selectedProc = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.StandardInstrumentDeparture) select element).ToList();
                if (chartTp == SigmaChartTypes.STARChart_Type)
                {
                    selectedProc = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.StandardInstrumentArrival) select element).ToList();
                    tmpN = "STAR";
                }
                else if (chartTp == SigmaChartTypes.IAPChart_Type)
                {
                    selectedProc = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element.PDM_Type == PDM_ENUM.InstrumentApproachProcedure) select element).ToList();
                    tmpN = "IAP";
                }




                ProcedureTabulation cdnTbl = new ProcedureTabulation
                {
                    MagVar = ((AirportHeliport)selARP).MagneticVariation.Value,
                    TemplateName = System.IO.Path.Combine(ArenaStaticProc.GetPathToTemplate() + @"\" + tmpN + @"\", "TABULAR DESCRIPTION.xls"),
                    NewCodingTableName = System.IO.Path.Combine(destPath2, System.IO.Path.GetFileNameWithoutExtension(projectName)) + ".xls",
                    AltitudeUOM = "FT",
                    DistanceUOM = "NM",

                };


                var listprocOrderedByName = selectedProc.OrderBy(name => name.GetObjectLabel()).ToList();
                cdnTbl.CreateCodingTable(listprocOrderedByName);
            }

        }


        #endregion
    }
}
