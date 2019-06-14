using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using System.Diagnostics;
using ArenaStatic;
using System.IO;
using System.Reflection;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaDataFromAIXM45.
    /// </summary>
    [Guid("7cee9616-6d66-447a-9d75-204a6295a121")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaDataFromAIXM45")]
    public sealed class ArenaDataFromAIXM45 : BaseCommand
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
        public ArenaDataFromAIXM45()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Load data to Arena project file"; //localizable text
            base.m_caption = "load AIXM 4.5 data";  //localizable text
            base.m_message = "Load data from AIXM 4.5 file";  //localizable text 
            base.m_toolTip = "Load data from AIXM 4.5 file";  //localizable text 
            base.m_name = "ArenaDataFromAIXM45";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.GeoprocessingScript16;
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

            //base.m_enabled = false;///////////

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {


            ArenaStaticProc.SetEnvironmentPath();

            Environment.Environment curEnvironment = DataCash.ProjectEnvironment; //new Environment.Environment(m_application);

            if (curEnvironment.pMap != null)
            {

                ILayer _Layer = EsriUtils.getLayerByName(curEnvironment.pMap, "AirportHeliport");
                var fc = ((IFeatureLayer)_Layer).FeatureClass;
                ARENA.DataLoaders.IARENA_DATA_Converter _data_loader = new ARENA.DataLoaders.AIXM45_DataConverter(curEnvironment);
                bool res = _data_loader.Convert_Data(fc);

                if (!res) return;

                if (curEnvironment.Data.PdmObjectList.Count > 0)
                {
                    //FillObjectsTree();
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

                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "The file successfully loaded", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.checkBox1.Visible = true;
                msgFrm.ShowDialog();
                if (msgFrm.checkBox1.Checked) Process.Start(System.IO.Path.GetDirectoryName(ArenaStaticProc.GetPathToARINCSpecificationFile()) + @"\ARENA_ResultsInfo.txt");

            }

        }

        #endregion
    }
}
