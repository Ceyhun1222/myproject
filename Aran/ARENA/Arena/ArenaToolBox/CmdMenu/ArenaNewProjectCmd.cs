using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.CatalogUI;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ARENA.Enums_Const;
using ArenaStatic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ArenaLogManager;

namespace ARENA.CmdMenu
{
    /// <summary>
    /// Summary description for ArenaNewProjectCmd.
    /// </summary>
    [Guid("ba927117-8e2f-4fa9-97c6-6f52f9cd9993")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ARENA.ArenaNewProjectCmd")]
    public sealed class ArenaNewProjectCmd : BaseCommand
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
            GxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        public ArenaNewProjectCmd()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Create Arena project file"; //localizable text
            base.m_caption = "Area and Terminal Project";  //localizable text
            base.m_message = "New Arena project file";  //localizable text 
            base.m_toolTip = "New Arena project file";  //localizable text 
            base.m_name = "ArenaProjectNewCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.arena32;//new Bitmap(GetType(), bitmapResourceName);
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

            //Disable if it is not ArcCatalog
            if (hook is IGxApplication)
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

            LogManager.GetLogger(GetType().Name).Info($" ----------Create new Arena project---------- ");
            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            IMap pMap = pNewDocument.FocusMap;

            Environment.Environment Environment = new ARENA.Environment.Environment { mxApplication = m_application };
            m_application.Caption = "Untitled - ArcMap";

            var tempDirName = System.IO.Path.GetTempPath();

            //var tempDirPath = Path.GetTempPath();

            tempDirName = System.IO.Path.GetDirectoryName(ArenaStaticProc.GetTargetDB());
            try
            {
                if (tempDirName.Length > 5 && tempDirName.ToUpper().Contains("TEMP"))
                {
                    if (Directory.Exists(tempDirName))
                    {
                        Directory.Delete(tempDirName, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when deleting an old project. Path: " + tempDirName);
            }

            ArenaStatic.ArenaStaticProc.SetTargetDB("");

            if (Environment.CreateEmptyProject(ArenaProjectType.ARENA))
            {
                DataCash.ProjectEnvironment = Environment;
                //MessageBox.Show("Ok");
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "The project is successfully created",
                    global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }
            else
            {
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "Error when creating new project",
                    global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }
        }

        #endregion
    }
}
