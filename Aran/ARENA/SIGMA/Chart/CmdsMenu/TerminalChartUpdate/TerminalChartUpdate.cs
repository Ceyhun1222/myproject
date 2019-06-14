using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using AranSupport;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ArenaStatic;
using ESRI.ArcGIS.Framework;
using System.IO;
using System.Linq;
using ARENA;
using SigmaChart.CmdsMenu;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;
using EsriWorkEnvironment;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using PDM;
using ANCOR.MapElements;
using OIS;
using ANCOR;
using System.Xml.Serialization;
using ANCOR.MapCore;
using ChartDataTabulation;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using ARENA.Enums_Const;
using DataModule;

namespace SigmaChart
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4d300cde-2f08-4fc3-ae97-0cdb47cac1b8")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.TerminalChartUpdate")]
    public sealed class TerminalChartUpdate : BaseCommand
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

        public TerminalChartUpdate()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "Update Terminal Chart";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "Update Terminal Chart";  //localizable text
            base.m_name = "UpdateTerminalChart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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

            m_application = hook as IApplication;

            m_hookHelper = new HookHelperClass();
            m_hookHelper.Hook = hook;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (DataCash.ProjectEnvironment == null || DataCash.ProjectEnvironment.Data.PdmObjectList.Count <= 0)
            {
                MessageBox.Show("Open Arena project ");
                return;
            }


            var openFileDialog1 = new OpenFileDialog { Filter = @"Sigma chart (*.mxd)|*.mxd" };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string pdmProjectFolder = System.IO.Path.GetDirectoryName(ArenaStaticProc.GetTargetDB());
   
            ArenaProjectType prjType = ArenaProjectType.ARENA;
            string selectedChart = openFileDialog1.FileName;

            List<string> CEFID_List = ChartsHelperClass.Read_CEFID(System.IO.Path.GetDirectoryName(selectedChart));

         

            List<PDMObject> oldPdmList = ArenaDataModule.GetObjectsFromPdmFile(System.IO.Path.GetDirectoryName(selectedChart), ref prjType);

            List<PDMObject> _list = (from element in oldPdmList where (element != null) && (CEFID_List.IndexOf(element.ID) >= 0) select element).ToList();

            var pathToTemplateFile = ArenaStaticProc.GetPathToTemplateFile();
            string filename = System.IO.Path.Combine(pathToTemplateFile, "ARENA.lyr");
            File.Copy(filename, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(selectedChart), "ARENA.lyr"), true);
            File.Copy(filename, System.IO.Path.Combine(pdmProjectFolder, "ARENA.lyr"), true);

            TerminalChartUpdateForm _updtFrm = new TerminalChartUpdateForm(oldPdmList, selectedChart, pdmProjectFolder);
            _updtFrm.ShowDialog();

           

        }

          #endregion

       

       
    }
}
