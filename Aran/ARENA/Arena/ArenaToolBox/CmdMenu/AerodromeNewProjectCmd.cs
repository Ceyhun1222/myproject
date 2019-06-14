using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.CatalogUI;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ARENA.Enums_Const;
using ArenaStatic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ArenaLogManager;
using ESRI.ArcGIS.Carto;

namespace ARENA.CmdMenu
{
    /// <summary>
    /// Summary description for ArenaNewProjectCmd.
    /// </summary>
    [Guid("6f1bc42c-d99a-46b2-bb3b-d9c5868fe342")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ARENA.AerodromeNewProjectCmd")]
    public sealed class AerodromeNewProjectCmd : BaseCommand
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
        public AerodromeNewProjectCmd()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Create Aerodrome project file"; //localizable text
            base.m_caption = "Aerodrome Project";  //localizable text
            base.m_message = "New Aerodrome project file";  //localizable text 
            base.m_toolTip = "New Aerodrome project file";  //localizable text 
            base.m_name = "AerodromeProjectNewCmd";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                base.m_bitmap = global::ArenaToolBox.Properties.Resources.avia_icon;//new Bitmap(GetType(), bitmapResourceName);
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

            LogManager.GetLogger(GetType().Name).Info($" ----------Create new Aerodrome project---------- ");
            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            IMap pMap = pNewDocument.FocusMap;

            Environment.Environment Environment = new ARENA.Environment.Environment { mxApplication = m_application };
            m_application.Caption = "Untitled - ArcMap";

            var tempDirName = System.IO.Path.GetTempPath();

            try
            {
                if (Directory.Exists(tempDirName))
                {
                    Directory.Delete(tempDirName, true);
                }

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "Error when deleting an old project. Path: " + tempDirName);
            }

            ArenaStatic.ArenaStaticProc.SetTargetDB("");
            if (Environment.CreateEmptyProject(ArenaProjectType.AERODROME))
            {
                DataCash.ProjectEnvironment = Environment;
                //MessageBox.Show("Ok");
                ArenaMessageForm msgFrm = new ArenaMessageForm("AERODROME", "The project is successfully created", global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }
            else
            {
                ArenaMessageForm msgFrm = new ArenaMessageForm("AERODROME", "Error when creating new project",
                    global::ArenaToolBox.Properties.Resources.ArenaMessage);
                msgFrm.ShowDialog();
            }   
        }

        #endregion
    }
}
