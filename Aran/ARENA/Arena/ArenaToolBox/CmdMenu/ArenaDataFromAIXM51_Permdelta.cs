﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using System.Windows.Forms;
using System.Diagnostics;
using ArenaStatic;
using System.Reflection;
using ArenaLogManager;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaDataFromAIXM51.
    /// </summary>
    [Guid("9fb8b825-3d20-461e-a2d1-e77ad0e3489a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaDataFromAIXM51_Permdelta")]
    public sealed class ArenaDataFromAIXM51_Permdelta : BaseCommand
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
        public ArenaDataFromAIXM51_Permdelta()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Load data to Arena project file"; //localizable text
            base.m_caption = "load AIXM 5.1 Permdelta data";  //localizable text
            base.m_message = "Load data from AIXM 5.1 Permdelta file";  //localizable text 
            base.m_toolTip = "Load data from AIXM 5.1 Permdelta file";  //localizable text 
            base.m_name = "ArenaDataFromAIXM51_Permdelta";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.CadastralCreateLineString16;
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
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

            ArenaStaticProc.SetEnvironmentPath();

            Environment.Environment curEnvironment = DataCash.ProjectEnvironment;

            if (curEnvironment.pMap != null)
            {

                ILayer _Layer = EsriUtils.getLayerByName(curEnvironment.pMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;


                DateTime start = DateTime.Now;

                ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.AIXM51_DataConverter(curEnvironment);

                ((ARENA.DataLoaders.AIXM51_DataConverter)_data_loader).getDataFromDB = false;

               

                if (!((ARENA.DataLoaders.AIXM51_DataConverter)_data_loader).Convert_Permdelta(fc))
                {

                    ArenaMessageForm msgFrm_ret = new ArenaMessageForm("ARENA", "Selected file is not a “Permdelta”.", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                    msgFrm_ret.checkBox1.Visible = true;
                    msgFrm_ret.ShowDialog();
                    LogManager.GetLogger(GetType().Name).Info($" ----------Convert permdelta data from AIXM 5.1 TOSSM DB to PDM is canceled ---------- ");
                    return;
                }

                curEnvironment.SetCenter_and_Projection();
                ((IActiveView)curEnvironment.pMap).Refresh();
                curEnvironment.SaveLog();

                for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
                {
                    IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                    string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                    if (cntxtName.StartsWith("TOCLayerFilter"))
                        ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
                }

                DateTime finish = DateTime.Now;

                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "The file successfully loaded \n " + ((TimeSpan)(finish - start)).ToString(), global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.checkBox1.Visible = true;
                msgFrm.ShowDialog();
                if (msgFrm.checkBox1.Checked) Process.Start(System.IO.Path.GetDirectoryName(ArenaStaticProc.GetPathToARINCSpecificationFile()) + @"\ARENA_ResultsInfo.txt");
                LogManager.GetLogger(GetType().Name).Info($"----------Convert data from AIXM 5.1 XML to PDM is finished; Elapsed time: " + ((TimeSpan)(finish - start)).ToString());

            }

        }

        #endregion
    }
}
